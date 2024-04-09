using System.ComponentModel.DataAnnotations;

namespace IntexII.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Year { get; set; }
        public int NumParts { get; set; }
        public double Price { get; set; }
        public string ImgLink { get; set; }
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set;}
        public string ProductDescription { get; set; }
        public string Category { get; set; }
    }
}
