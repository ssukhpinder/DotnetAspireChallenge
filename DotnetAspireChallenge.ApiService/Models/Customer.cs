using System.ComponentModel.DataAnnotations;

namespace DotnetAspireChallenge.ApiService.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
