using System.Linq;
using System.Threading.Tasks;
using Identity.Account.Payload;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Account
{
    [Route("account")]
    public class IdentityController : Controller
    {
        private IIdentityService _identityService;


        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterRequestModel>> Register([FromBody] RegisterRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(GetErrors());
            var (user, success) = await this._identityService.Register(request);
            if (!success) return BadRequest("Failed to register");
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseModel>> Login([FromBody] LoginRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(GetErrors());
            var (payload, success) = await this._identityService.Login(request);
            if (!success) return Unauthorized();
            return Ok(payload);
        }

        private string GetErrors()
        {
            return ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage).Aggregate((a, b) => a + "|" + b);
        }
    }
}