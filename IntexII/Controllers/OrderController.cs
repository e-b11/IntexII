using Microsoft.AspNetCore.Mvc;
using IntexII.Models;

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
            return View(new Order());

        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (cart.Lines.Count()==0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                _repo.AddOrder(order);
                cart.Clear();

                return RedirectToPage("/OrderConfirmation", new { transactionId = order.TransactionId });        
            }
            else
            {
                return View();
            }
        }
    }


}
