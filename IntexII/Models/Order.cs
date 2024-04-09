using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntexII.Models
{
    public class Order
    {
        [Key]
        public int TransactionId { get; set; }
        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime OrderDate {  get; set; }
        public string DayOfWeek { get; set; }
        public int Time { get; set; }
        public string TypeOfCard { get; set; }
        public string EntryMode { get; set; }
        public double Amount { get; set; }
        public string TypeOfTransaction { get; set; }
        public string CountryOfTransaction { get; set; }
        public string ShippingAddress { get; set; }
        public string Bank { get; set; }
        public bool Fraud { get; set; }
    }
}
