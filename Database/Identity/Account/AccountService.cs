using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<(AccountResponseModel, bool)> Register(RegisterRequestModel model)
        {
            var user = new AppUser
            {
                Email = model.Email,
                Id = Guid.NewGuid(),
                UserName = model.Email,
                Name = model.Name,
            };

            // Create new user with .NET Core Identity Service
            var x = await _userManager.CreateAsync(user, model.Password);
            if (!x.Succeeded) return (null, false);

            // Obtain data and return
            var created = await _userManager.FindByEmailAsync(user.Email);
            return (ToResponseModel(created), true);
        }

        public async Task<(AccountResponseModel, bool)> Login(LoginRequestModel model)
        {
            // attempt to sign in
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            // return false if sign in fails
            if (!result.Succeeded) return (null, false);

            // if login succeeded, return updated user
            var logged = await _userManager.FindByEmailAsync(model.Email);
            return (ToResponseModel(logged), true);
        }

        // Converts from User to response model
        private AccountResponseModel ToResponseModel(AppUser user)
        {
            return new AccountResponseModel
            {
                Email = user.Email,
                Guid = user.Id.ToString(),
                Name = user.Name,
                SocialSecurityNumber = user.SocialSecurityNumber,
            };
        }
    }
}