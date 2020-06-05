using System;
using System.ComponentModel.DataAnnotations;

namespace EFCoreSample.Controls.Requests
{
    public class CommandRequest
    {
        [Required]
        public int Value { get; set; }
        [Required]
        public string Units { get; set; }
        [Required]
        public string Issuer { get; set; }
        [Required]
        public string Purpose { get; set; }
        [Required]
        public DateTime IssueDate { get; set; }
        [Required]
        public DateTime ExecuteDate { get; set; }
        [Required]
        public int Repeat { get; set; }
    }
}