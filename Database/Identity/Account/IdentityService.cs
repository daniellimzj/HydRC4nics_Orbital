using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Identity.Account.Payload;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Account
{
    public class IdentityService : IIdentityService
    {
        private IAccountService _account;


        public IdentityService(IAccountService account)
        {
            _account = account;
        }

        public async Task<(RegisterResponseModel, bool)> Register(RegisterRequestModel requestModel)
        {
            // Attempt to register
            var (user, success) = await _account.Register(requestModel);
            // convert to response model if success, and return
            var ret = success ? new RegisterResponseModel {Email = user.Email, Id = user.Guid} : null;
            return (ret, success);
        }

        public async Task<(LoginResponseModel, bool)> Login(LoginRequestModel loginRequestModel)
        {
            // attempt to login and exit if fails
            var (appUser, success) = await _account.Login(loginRequestModel);
            if (!success) return (null, false);

            var domain = appUser.Email.Split("@")[1];
            var job = domain == "teacher.com" ? "teacher" : domain == "student.com" ? "student" : "none";

            // Generate JWT Claims
            var claims = new[]
            {
                new Claim("id", appUser.Guid),
                new Claim("email", appUser.Email),
                new Claim("name", appUser.Name),
                new Claim("job", job),
            };
            // Lets use a fake secret
            var secret = Encoding.ASCII.GetBytes("super_secret_and_kind_of_confidential");

            // Create token handlers and descriptor
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            // Generating the token 
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var model = new LoginResponseModel
            {
                Token = tokenHandler.WriteToken(token),
                Id = appUser.Guid,
            };
            return (model, true);
        }
    }
}