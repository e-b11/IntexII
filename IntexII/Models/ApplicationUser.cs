using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace IntexII.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Additional custom properties can be added here if needed
        // [ForeignKey("CustomerId")]
        public int? CustomerId {get;set;}
        public Customer? Customer {get;set;}
    }
}
