using IntexII.Models;
using Microsoft.AspNetCore.Mvc;
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
            var products = _repo.Products;
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult BrowseProducts()
        {
            var products = _repo.Products;
            
            return View(products);
        }

        public IActionResult SingleProduct(int productId)
        {
            var singleProduct = _repo.Products
                .Single(x => x.ProductId == productId);

            return View(singleProduct);
        }
    }
}
