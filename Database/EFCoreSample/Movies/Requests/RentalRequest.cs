using System.ComponentModel.DataAnnotations;

namespace EFCoreSample.Movies.Requests
{
    public class RentalRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
    }
}