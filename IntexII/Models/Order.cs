using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntexII.Models
{
    public class Order
    {
        [Key]
        public int TransactionId { get; set; }
        [Required]
        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        [Required]
        public DateTime OrderDate {  get; set; }
        [Required]
        public string DayOfWeek { get; set; }
        [Required]
        public int Time { get; set; }
        [Required]
        public string EntryMode { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public string TypeOfTransaction { get; set; }
        [Required]
        public string CountryOfTransaction { get; set; }
        [Required]
        public string ShippingAddress { get; set; }
        [Required]
        public string Bank { get; set; }
        [Required]
        public string TypeOfCard { get; set; }
        [Required]
        public int Fraud { get; set; }
        public int? FraudFlag { get; set; } = 0;
    }
}
