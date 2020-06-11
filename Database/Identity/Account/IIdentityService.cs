using System.Collections.Generic;
using System.Threading.Tasks;
using Identity.Account.Payload;

namespace Identity.Account
{
    public interface IIdentityService
    {
        Task<(RegisterResponseModel,bool)> Register(RegisterRequestModel registerRequest);
        Task<(LoginResponseModel,bool)> Login(LoginRequestModel loginRequest);
        Task<(RegisterResponseModel,bool)> Update(RegisterRequestModel registerRequest);
        Task<(RegisterResponseModel,bool)> Delete(string email);
        Task<(RegisterResponseModel,bool)> AddClaim(ClaimRequestModel claimRequest);
        Task<(RegisterResponseModel,bool)> RemoveClaim(ClaimRequestModel claimRequest);
        Task<(IEnumerable<RegisterResponseModel>,bool)> GetUsers();
        Task<(IEnumerable<RegisterResponseModel>,bool)> GetUsersByClaim(ClaimRequestModel claimRequest);
    }
}