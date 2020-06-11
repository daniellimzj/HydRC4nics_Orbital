using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Identity.Account.Payload;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Account
{
    public class IdentityService : IIdentityService
    {
        private readonly IAccountService _accountService;

        public IdentityService(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<(RegisterResponseModel, bool)> Register(RegisterRequestModel registerRequest)
        {
            // Attempt to register
            var (account, success) = await _accountService.Register(registerRequest);
            // convert to response model if success, and return
            var ret = success
                ? new RegisterResponseModel {Name = account.Name, Email = account.Email, Id = account.Guid, Claims = account.Claims}
                : null;
            return (ret, success);
        }

        public async Task<(LoginResponseModel, bool)> Login(LoginRequestModel loginRequest)
        {
            // attempt to login and exit if fails
            var (account, success) = await _accountService.Login(loginRequest);
            if (!success) return (null, false);

            // Use a secret
            var secret = Encoding.UTF8.GetBytes(await File.ReadAllTextAsync("../secret.txt"));

            // Create token handlers and descriptor
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(account.Claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            // Generating the token 
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var model = new LoginResponseModel
            {
                Token = tokenHandler.WriteToken(token),
                Id = account.Guid,
                Name = account.Name
            };
            return (model, true);
        }
        
        public async Task<(RegisterResponseModel, bool)> Update(RegisterRequestModel registerRequest)
        {
            // Attempt to register
            var (account, success) = await _accountService.Update(registerRequest);
            // convert to response model if success, and return
            var ret = success
                ? new RegisterResponseModel {Name = account.Name, Email = account.Email, Id = account.Guid, Claims = account.Claims}
                : null;
            return (ret, success);
        }

        public async Task<(RegisterResponseModel, bool)> Delete(string email)
        {
            // Attempt to register
            var (account, success) = await _accountService.Delete(email);
            // convert to response model if success, and return
            var ret = success
                ? new RegisterResponseModel {Name = account.Name, Email = account.Email, Id = account.Guid, Claims = account.Claims}
                : null;
            return (ret, success);
        }

        public async Task<(RegisterResponseModel, bool)> AddClaim(ClaimRequestModel claimRequest)
        {
            // Attempt to add
            var (account, success) = await _accountService.AddClaim(claimRequest);
            // convert to response model if success, and return
            var ret = success
                ? new RegisterResponseModel {Name = account.Name, Email = account.Email, Id = account.Guid, Claims = account.Claims}
                : null;
            return (ret, success);
        }
        
        public async Task<(RegisterResponseModel, bool)> RemoveClaim(ClaimRequestModel claimRequest)
        {
            // Attempt to add
            var (account, success) = await _accountService.RemoveClaim(claimRequest);
            // convert to response model if success, and return
            var ret = success
                ? new RegisterResponseModel {Name = account.Name, Email = account.Email, Id = account.Guid, Claims = account.Claims}
                : null;
            return (ret, success);
        }

        public async Task<(IEnumerable<RegisterResponseModel>, bool)> GetUsers()
        {
            // Attempt to get users
            var (users, success) = await _accountService.GetUsers();
            // convert to response model if success, and return
            var ret = success
                ? users
                    .Select(user => new RegisterResponseModel
                        {Name = user.Name, Email = user.Email, Id = user.Guid, Claims = user.Claims})
                : null;
            return (ret, success);
        }

        public async Task<(IEnumerable<RegisterResponseModel>, bool)> GetUsersByClaim(ClaimRequestModel claimRequest)
        {
            // Attempt to get users
            var (users, success) = await _accountService.GetUsersByClaim(claimRequest);
            // convert to response model if success, and return
            var ret = success
                ? users
                    .Select(user => new RegisterResponseModel
                        {Name = user.Name, Email = user.Email, Id = user.Guid, Claims = user.Claims})
                : null;
            return (ret, success);
        }
    }
}