using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading.Tasks;
using Identity.Account.Payload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Account
{
    [ApiController]
    [Route("Account")]
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterRequestModel>> Register([FromBody] RegisterRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(GetErrors());
            var (user, success) = await _identityService.Register(request);
            if (!success) return BadRequest("Failed to register");
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseModel>> Login([FromBody] LoginRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(GetErrors());
            var (payload, success) = await _identityService.Login(request);
            if (!success) return Unauthorized();
            return Ok(payload);
        }
        
        [HttpPut("update")]
        public async Task<ActionResult<RegisterRequestModel>> Update([FromBody] RegisterRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(GetErrors());
            var (payload, success) = await _identityService.Update(request);
            if (!success) return Unauthorized();
            return Ok(payload);
        }

        [Authorize(Policy = "MasterOnly"), HttpDelete("delete/{email}")]
        public async Task<ActionResult<RegisterRequestModel>> Update(string email)
        {
            if (!ModelState.IsValid) return BadRequest(GetErrors());
            var (user, success) = await _identityService.Delete(email);
            if (!success) return BadRequest("Failed to delete");
            return Ok(user);
        }
        
        [Authorize(Policy = "MasterOnly"), HttpPost("claim/add")]
        public async Task<ActionResult<RegisterResponseModel>> AddClaim([FromBody] ClaimRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(GetErrors());
            var (user, success) = await _identityService.AddClaim(request);
            if (!success) return BadRequest("Failed to add claim");
            return Ok(user);
        }
        
        [Authorize(Policy = "MasterOnly"), HttpPost("claim/remove")]
        public async Task<ActionResult<RegisterResponseModel>> RemoveClaim([FromBody] ClaimRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(GetErrors());
            var (user, success) = await _identityService.RemoveClaim(request);
            if (!success) return BadRequest("Failed to remove claim");
            return Ok(user);
        }
        
        [Authorize(Policy = "NUSOnly"), HttpGet("users")]
        public async Task<ActionResult<IEnumerable<RegisterResponseModel>>> GetUsers()
        {
            if (!ModelState.IsValid) return BadRequest(GetErrors());
            var (users, success) = await _identityService.GetUsers();
            if (!success) return BadRequest("Failed to get users");
            return Ok(users);
        }
        
        [Authorize(Policy = "NUSOnly"), HttpGet("users/{claimType}/{claimValue}")]
        public async Task<ActionResult<IEnumerable<RegisterResponseModel>>> GetUsersByClaim(string claimType, string claimValue)
        {
            if (!ModelState.IsValid) return BadRequest(GetErrors());
            var request = new ClaimRequestModel
            {
                Type = claimType,
                Value = claimValue
            };
            var (users, success) = await _identityService.GetUsersByClaim(request);
            if (!success) return BadRequest("Failed to get users");
            return Ok(users);
        }

        private string GetErrors()
        {
            return ModelState.Values
                .SelectMany(entry => entry.Errors)
                .Select(error => error.ErrorMessage).Aggregate((a, b) => a + "|" + b);
        }
    }
}