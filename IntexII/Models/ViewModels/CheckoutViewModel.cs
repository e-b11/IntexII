

namespace IntexII.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public Order Order { get; set; }
        public ICollection<Cart.CartLine> Lines { get; set; } = new List<Cart.CartLine>();
    }
}
