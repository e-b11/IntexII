using Microsoft.AspNetCore.Identity;
using IntexII.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace IntexII.Controllers;
public class AdminController : Controller
{
    private IIntexRepository _repo;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(UserManager<ApplicationUser> userManager, IIntexRepository temp)
    {
        _userManager = userManager;
        _repo = temp;
    }

    public IActionResult ManageUsers()
    {
        // Retrieve all users from the database
        var users = _userManager.Users.ToList();

        // Pass the list of users to the view
        return View(users);
    }
    [HttpGet]
    public IActionResult EditUser(string userId)
    {
      var user = _userManager.Users.Single(u => u.Id == userId);
      return View(user);
    }

    [HttpPost]
    public IActionResult EditUser(ApplicationUser user)
    {
      _userManager.UpdateAsync(user);
      return RedirectToAction("Admin", "ManageUsers");

    }
    public IActionResult ConfirmDeleteUser(string userId)
    {
      var user = _userManager.Users.Single(u => u.Id == userId);
      return View(user);
    }
    public IActionResult DeleteUser(ApplicationUser user)
    {
      // _userManager.Users.Remove(user);
      return RedirectToAction("Admin", "ManageUsers");
    }

    public IActionResult ManageProducts()
    {
      var products = _repo.Products.ToList();
      return View(products);
    }
    public IActionResult AddProduct()
    {
      return View("Product");
    }
    [HttpGet]
    public IActionResult EditProduct(int productId)
    {
      var product = _repo.Products.Single(p => p.ProductId == productId);
      return View("Product", product);
    }
    [HttpPost]
    public IActionResult EditProduct(Product product)
    {
      _repo.EditProduct(product);
      return RedirectToAction("ManageProducts");
    }
}
