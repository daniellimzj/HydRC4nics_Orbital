using System.Collections.Generic;
using System.Threading.Tasks;
using Identity.Account.Payload;

namespace Identity.Account
{
    public interface IAccountService
    {
        Task<(AccountResponseModel,bool)> Register(RegisterRequestModel registerRequest);
        Task<(AccountResponseModel,bool)> Login(LoginRequestModel loginRequest);
        Task<(AccountResponseModel,bool)> Update(RegisterRequestModel registerRequest);
        Task<(AccountResponseModel,bool)> Delete(string email);
        Task<(AccountResponseModel, bool)> AddClaim(ClaimRequestModel claimRequest);
        Task<(AccountResponseModel, bool)> RemoveClaim(ClaimRequestModel claimRequest);
        Task<(IEnumerable<AccountResponseModel>, bool)> GetUsers();
        Task<(IEnumerable<AccountResponseModel>, bool)> GetUsersByClaim(ClaimRequestModel claimRequest);
    }
}