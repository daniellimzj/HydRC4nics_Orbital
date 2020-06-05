using System;
using System.ComponentModel.DataAnnotations;

namespace EFCoreSample.Monitoring.Requests
{
    public class ReadingRequest
    {
        [Required]
        public int Value { get; set; }
        [Required]
        public string Units { get; set; }
        [Required]
        public DateTime TimeStamp { get; set; }
    }
}