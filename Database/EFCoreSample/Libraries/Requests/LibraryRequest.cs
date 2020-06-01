using System.ComponentModel.DataAnnotations;

namespace EFCoreSample.Libraries.Requests
{
    public class LibraryRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
    }
}