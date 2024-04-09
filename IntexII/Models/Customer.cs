using System.ComponentModel.DataAnnotations;

namespace IntexII.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set;}
        public DateTime BirthDate { get; set; }
        public string CountryOfResidence { get; set; }
        public string Gender {  get; set; }
        public double Age { get; set; }

    }
}
