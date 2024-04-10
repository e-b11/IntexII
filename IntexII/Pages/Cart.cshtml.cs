using IntexII.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IntexII.Pages
{
    public class CartModel : PageModel
    {
        private IIntexRepository _repo;

        public CartModel(IIntexRepository temp, Cart cartService)
        {
            _repo = temp;
            Cart = cartService;
        }

        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; } = "/Products/Page/1";

        //public void OnGet(string returnUrl)
        //{
        //    ReturnUrl = returnUrl ?? "/Products/Page/1";
        //    //Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
        //}

        public void OnGet() 
        {
            ReturnUrl = "/Products/Page/1";
        }

        public IActionResult OnPost(int productId, string returnUrl)
        {
            Product prod = _repo.Products
                .FirstOrDefault(x => x.ProductId == productId);

            if (prod != null)
            {
                Cart.AddItem(prod, 1);
            }

            return RedirectToPage(new { returnUrl = ReturnUrl });


        }

        public IActionResult OnPostRemove(int productId, string returnUrl)
        {
            Cart.RemoveLine(Cart.Lines.First(x => x.Product.ProductId == productId).Product);

            return RedirectToPage(new { returnUrl = ReturnUrl });
        }
    }
}
