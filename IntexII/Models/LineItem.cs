using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace IntexII.Models
{
    public class LineItem
    {
        [Key]
        public int LineItemId { get; set; }
        [Required]
        [ForeignKey("TransactionId")]
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }
        [Required]
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Rating { get; set; }
    }
}
