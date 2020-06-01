using System.Threading.Tasks;
using Identity.Account.Payload;

namespace Identity.Account
{
    public interface IIdentityService
    {
        Task<(RegisterResponseModel,bool)> Register(RegisterRequestModel requestModel);
        Task<(LoginResponseModel,bool)> Login(LoginRequestModel loginRequestModel);
    }
}