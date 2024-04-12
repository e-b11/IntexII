using IntexII.Models;
using IntexII.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using System.Diagnostics;

namespace IntexII.Controllers
{
    public class HomeController : Controller
    {

        private IIntexRepository _repo;
        private Cart cart;

        public HomeController(IIntexRepository temp, Cart cartservice)
        {
            _repo = temp;
            cart = cartservice;
        }

        public IActionResult Index()
        {
            //var products = _repo.Products;
            int[] topProductIds = [23,19,21,22,20];
            var topProducts = new List<Product>();
            foreach (int id in topProductIds)
            {
                Product product = _repo.GetProductById(id);
                topProducts.Add(product);
            }
            ViewBag.TopProducts = topProducts;
            
            return View();
        }

        public IActionResult AboutUs()
        {
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        public IActionResult BrowseProducts(string? category, string? color, int pageNum = 1)
        {
            int defaultPageSize = 5;

            int pageSize = HttpContext.Session.GetInt32("pageSize") ?? defaultPageSize;


            var products = new ProductsListViewModel
            {
                Products = _repo.Products
                    .Where(x => (x.Category == category || category == null) &&
                        (color == null || x.PrimaryColor == color || x.SecondaryColor == color))
                    .OrderBy(x => x.ProductId)
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize),

                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = (category == null && color == null) ? _repo.Products.Count() : _repo.Products.Where(x => (x.Category == category || category == null) &&
                        (color == null || x.PrimaryColor == color || x.SecondaryColor == color)).Count()
                },

                CurrentCategory = category,
                CurrentColor = color
            };


            return View(products);
        }

        [HttpPost]
        public IActionResult ChangePageSize(int pageSize)
        {
            // Validate and set the pageSize into session
            HttpContext.Session.SetInt32("pageSize", pageSize);

            // Redirect back to the Index action or wherever appropriate
            return RedirectToAction("BrowseProducts");
        }

        public IActionResult SingleProduct(int productId)
        {
            var singleProduct = _repo.Products
                .Single(x => x.ProductId == productId);

            var recommendations = _repo.Products
                .Where(x => x.ProductId != productId && (x.Rec1 == productId || x.Rec2 == productId || x.Rec3 == productId || x.Rec4 == productId))
                .ToList();

            if (recommendations.Count < 5)
            {
                var additionalRecommendations = _repo.Products
                    .Where(x => x.ProductId != productId && x.Category == singleProduct.Category && !recommendations.Contains(x))
                    .Take(5 - recommendations.Count)
                    .ToList();

                recommendations.AddRange(additionalRecommendations);
            }

            ViewBag.TopRecommendations = recommendations.Take(5).ToList();

            return View(singleProduct);
        }


        //[HttpGet]
        //public IActionResult Checkout()
        //{
        //    var checkoutDetails = new CheckoutViewModel
        //    {
        //        Order = new Order(),

        //        Lines = new List<Cart.CartLine>(),

        //        Cart = cart,
        //    };


        //    return View("Checkout", checkoutDetails);
        //}

        //[HttpPost]
        //public IActionResult Checkout(CheckoutViewModel checkout)
        //{
        //    if (cart.Lines.Count() == 0)
        //    {
        //        ModelState.AddModelError("",
        //            "Sorry, your cart is empty!");
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        checkout.Lines = cart.Lines.ToArray();
        //        _repo.AddOrder(checkout.Order);
        //        cart.Clear();
        //        return View("OrderConfirmation", checkout);
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}
    }
}
