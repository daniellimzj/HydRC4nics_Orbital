using System.Threading.Tasks;
using Identity.Account.Payload;

namespace Identity.Account
{
    public interface IAccountService
    {
        Task<(AccountResponseModel,bool)> Register(RegisterRequestModel requestModel);
        Task<(AccountResponseModel,bool)> Login(LoginRequestModel loginRequestModel);
    }
}