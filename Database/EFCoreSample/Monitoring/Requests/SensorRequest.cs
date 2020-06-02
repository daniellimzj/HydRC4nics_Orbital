using System.ComponentModel.DataAnnotations;

namespace EFCoreSample.Monitoring.Requests
{
    public class SensorRequest
    {
        [Required]
        public string Position { get; set; }
        [Required]
        public string Type { get; set; }
    }
}