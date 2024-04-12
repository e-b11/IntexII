using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static IntexII.Models.Cart;

namespace IntexII.Models
{
    public class Order
    { 
        [Key]
        public int TransactionId { get; set; }
        [Required]
        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

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
        [Required(ErrorMessage = "The County of Transaction field is required.")]
        public string CountryOfTransaction { get; set; }
        [Required(ErrorMessage = "The Shipping Address Country field is required.")]
        public string ShippingAddress { get; set; }
        [Required(ErrorMessage = "The Bank field is required.")]
        public string Bank { get; set; }
        [Required(ErrorMessage = "The Type of Card field is required.")]
        public string TypeOfCard { get; set; }
        [Required]
        public int Fraud { get; set; }
        public string? FraudFlag { get; set; }
    }
}
