using System.ComponentModel.DataAnnotations;

namespace EFCoreSample.Controls.Requests
{
    public class ActuatorRequest
    {
        [Required]
        public string Position { get; set; }
        [Required]
        public string Type { get; set; }
    }
}