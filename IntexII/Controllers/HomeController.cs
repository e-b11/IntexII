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

        public IActionResult BrowseProducts(string? category, int pageNum = 1)
        {
            int pageSize = 5;

            var products = new ProductsListViewModel
            {
                Products = _repo.Products
                    .Where(x => x.Category == category || category == null)
                    .OrderBy(x => x.ProductId)
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize),

                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ? _repo.Products.Count() : _repo.Products.Where(x => x.Category == category).Count()
                },

                CurrentCategory = category
            };
            
            //var products = _repo.Products;
            
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
