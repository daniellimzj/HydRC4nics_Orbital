using System.ComponentModel.DataAnnotations;

namespace Identity.Account.Payload
{
    public class ClaimRequestModel
    {
        [Required] public string Email { get; set; }
        [Required] public string Type { get; set; }
        [Required] public string Value { get; set; }
    }
}