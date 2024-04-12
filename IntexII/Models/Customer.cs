using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntexII.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set;}
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string CountryOfResidence { get; set; }
        [Required]
        public string Gender {  get; set; }
        [Required]
        public double Age { get; set; }

        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        // [ForeignKey("Id")]
        // public string? UserId { get; set; }
        // public ApplicationUser? User { get; set; }

    }
}
