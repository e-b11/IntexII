using Microsoft.AspNetCore.Mvc;
using IntexII.Models;
using IntexII.Models.ViewModels;

namespace IntexII.Controllers
{
    public class OrderController : Controller
    {
        private IIntexRepository _repo;
        private Cart cart;

        public OrderController(IIntexRepository temp, Cart cartService)
        {
            _repo = temp;
            cart = cartService;
        }
        public IActionResult Checkout()
        {
            var checkoutDetails = new CheckoutViewModel
            {
                Cart = cart
            };
            
            return View(checkoutDetails);

        }

        [HttpPost]
        public IActionResult Checkout(CheckoutViewModel checkoutDetails)
        {
            if (cart.Lines.Count()==0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                //checkoutDetails.Order.CustomerId = 1;
                //checkoutDetails.Order.OrderDate = DateTime.Now.Date;
                //checkoutDetails.Order.DayOfWeek = DateTime.Now.ToString("ddd");
                //checkoutDetails.Order.Time = DateTime.Now.Hour;
                //checkoutDetails.Order.EntryMode = "CVC";
                //checkoutDetails.Order.TypeOfTransaction = "Online";

                checkoutDetails.Order.Amount = cart.CalculateTotal();


                
                _repo.AddOrder(checkoutDetails.Order);

                foreach (var l in cart?.Lines ?? Enumerable.Empty<Cart.CartLine>())
                {
                    var lineItem = new LineItem
                    {
                        TransactionId = checkoutDetails.Order.TransactionId,
                        ProductId = l.Product.ProductId,
                        Quantity = l.Quantity,

                    };

                    _repo.AddLineItem(lineItem);
                }



                cart.Clear();

                return View("OrderConfirmation", checkoutDetails.Order);      
            }
            else
            {
                return View();
            }
        }
    }


}
