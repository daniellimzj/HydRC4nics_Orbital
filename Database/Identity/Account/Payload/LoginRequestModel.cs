using System.ComponentModel.DataAnnotations;

namespace Identity.Account.Payload
{
    public class LoginRequestModel
    {
        [Required] public string Email { get; set; }
        [Required] public string Password { get; set; }
    }
}