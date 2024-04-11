

namespace IntexII.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public Order Order { get; set; } = new Order();
        public ICollection<LineItem> LineItems { get; set; } = new List<LineItem>();

        public Cart? Cart { get; set; }
        
    }
}
