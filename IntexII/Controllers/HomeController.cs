using IntexII.Models;
using IntexII.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using NuGet.ProjectModel;
using System.Diagnostics;

namespace IntexII.Controllers
{
    public class HomeController : Controller
    {
        
        private IIntexRepository _repo;

        public HomeController(IIntexRepository temp)
        {
            _repo = temp;
        }

        public IActionResult Index()
        {
            //var products = _repo.Products;
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

            return View(singleProduct);
        }

        public ViewResult Checkout()
        {
            var checkoutDetails = new CheckoutViewModel
            {
                Order = new Order(),

                Lines = new List<Cart.CartLine>(),
            };


            return View(checkoutDetails);
        }
    }
}
