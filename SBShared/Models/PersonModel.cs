using System.ComponentModel.DataAnnotations;

namespace SBShared.Models
{
    internal class PersonModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
