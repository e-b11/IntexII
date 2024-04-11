using System.ComponentModel.DataAnnotations;

namespace IntexII.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public int NumParts { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string ImgLink { get; set; }
        [Required]
        public string PrimaryColor { get; set; }
        [Required]
        public string SecondaryColor { get; set;}
        [Required]
        public string ProductDescription { get; set; }
        [Required]
        public string Category { get; set; }
        public int? Rec1 { get; set; }
        public int? Rec2 { get; set; }
        public int? Rec3 { get; set; }
        public int? Rec4 { get; set; }
        public int? Rec5 { get; set; }
    }
}
