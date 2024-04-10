using Microsoft.AspNetCore.Mvc;
using IntexII.Models;

namespace IntexII.Components
{
    public class CategoriesViewComponent : ViewComponent
    {
        private IIntexRepository _repo;

        //Constructor
        public CategoriesViewComponent(IIntexRepository temp)
        {
            _repo = temp;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];

            var categories = _repo.Products
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x);

            return View(categories);
        }
    }
}
