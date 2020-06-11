using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.Account.Domain;
using Identity.Account.Payload;
using Microsoft.AspNetCore.Identity;

namespace Identity.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<(AccountResponseModel, bool)> Register(RegisterRequestModel registerRequest)
        {
            var user = new AppUser
            {
                Email = registerRequest.Email,
                Id = Guid.NewGuid(),
                UserName = registerRequest.Email,
                Name = registerRequest.Name,
            };

            // Create new user with .NET Core Identity Service
            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            if (!result.Succeeded) return (null, false);
            
            var domain = registerRequest.Email.Split("@")[1];
            var org = domain switch
            {
                "u.nus.edu" => "nus",
                _ => "none"
            };

            // Generate JWT Claims
            var x = await _userManager.AddClaimAsync(user, new Claim("org", org));
            if (!x.Succeeded) return (null, false);

            // Obtain data and return
            var created = await _userManager.FindByEmailAsync(user.Email);
            var claims = await _userManager.GetClaimsAsync(created);
            return (ToResponseModel(created, claims), true);
        }

        public async Task<(AccountResponseModel, bool)> Login(LoginRequestModel loginRequest)
        {
            // attempt to sign in
            var result = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, false, false);
            // return false if sign in fails
            if (!result.Succeeded) return (null, false);

            // if login succeeded, return updated user
            var logged = await _userManager.FindByEmailAsync(loginRequest.Email);
            var claims = await _userManager.GetClaimsAsync(logged);
            
            return (ToResponseModel(logged, claims), true);
        }
        
        public async Task<(AccountResponseModel, bool)> Update(RegisterRequestModel registerRequest)
        {
            // attempt to sign in
            var result = await _signInManager.PasswordSignInAsync(registerRequest.Email, registerRequest.Password, false, false);
            // return false if sign in fails
            if (!result.Succeeded) return (null, false);

            // if login succeeded, return updated user
            var logged = await _userManager.FindByEmailAsync(registerRequest.Email);

            logged.Email = registerRequest.Email;
            logged.UserName = registerRequest.Email;
            logged.Name = registerRequest.Name;

            var updated = await _userManager.UpdateAsync(logged);
            // return false if update fails
            if (!updated.Succeeded) return (null, false);
            var claims = await _userManager.GetClaimsAsync(logged);
            return (ToResponseModel(logged, claims), true);
        }

        public async Task<(AccountResponseModel, bool)> Delete(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.DeleteAsync(user);
            // return false if delete fails
            return result.Succeeded ? (ToResponseModel(user, claims), true) : (null, false);
        }

        public async Task<(AccountResponseModel, bool)> AddClaim(ClaimRequestModel claimRequest)
        {
            var user = await _userManager.FindByEmailAsync(claimRequest.Email);
            if (user == null) return (null, false);
            // Generate JWT Claims
            var result = await _userManager.AddClaimAsync(user, new Claim(claimRequest.Type, claimRequest.Value));
            if (!result.Succeeded) return (null, false);
            var claims = await _userManager.GetClaimsAsync(user);
            
            return (ToResponseModel(user, claims), true);
        }
        
        public async Task<(AccountResponseModel, bool)> RemoveClaim(ClaimRequestModel claimRequest)
        {
            var user = await _userManager.FindByEmailAsync(claimRequest.Email);
            if (user == null) return (null, false);
            // Generate JWT Claims
            var result = await _userManager.RemoveClaimAsync(user, new Claim(claimRequest.Type, claimRequest.Value));
            if (!result.Succeeded) return (null, false);
            var claims = await _userManager.GetClaimsAsync(user);
            
            return (ToResponseModel(user, claims), true);
        }

        public async Task<(IEnumerable<AccountResponseModel>, bool)> GetUsers()
        {
            // return users
            var users = _userManager.Users;
            var withClaims = users.AsEnumerable().Select(async user => ToResponseModel(user, await _userManager.GetClaimsAsync(user)));
            var accounts = await Task.WhenAll(withClaims);
            return (accounts, true);
        }

        public async Task<(IEnumerable<AccountResponseModel>, bool)> GetUsersByClaim(ClaimRequestModel claimRequest)
        {
            // return users
            var users = await _userManager.GetUsersForClaimAsync(new Claim(claimRequest.Type, claimRequest.Value));
            var withClaims = users.Select(async user => ToResponseModel(user, await _userManager.GetClaimsAsync(user)));
            var accounts = await Task.WhenAll(withClaims);
            return (accounts, true);
        }

        // Converts from User to response model
        private static AccountResponseModel ToResponseModel(AppUser user, IEnumerable<Claim> claims)
        {
            return new AccountResponseModel
            {
                Email = user.Email,
                Guid = user.Id.ToString(),
                Name = user.Name,
                Claims = claims
            };
        }
    }
}