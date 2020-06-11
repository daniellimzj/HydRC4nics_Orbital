using System.ComponentModel.DataAnnotations;

namespace Identity.Account.Payload
{
    public class RegisterRequestModel
    {
        [Required, EmailAddress] public string Email { get; set; }

        [Required] public string Password { get; set; }

        [Required] public string Name { get; set; }
    }
}