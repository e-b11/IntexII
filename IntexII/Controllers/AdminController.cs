using Microsoft.AspNetCore.Identity;
using IntexII.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using IntexII.ViewModels;
using IntexII.Models.ViewModels;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace IntexII.Controllers;
public class AdminController : Controller
{
    private IIntexRepository _repo;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    
    

    public AdminController(UserManager<ApplicationUser> userManager, IIntexRepository temp, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _repo = temp;
        
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Home()
    {
      return View("AdminHome");
    }
    [Authorize(Roles = "Admin")]
    public IActionResult ManageUsers()
    {
        // Retrieve all users from the database
        var users = _userManager.Users.ToList();

        // Pass the list of users to the view
        return View(users);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> EditUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Get all roles
        var roles = _roleManager.Roles.Select(r => r.Name).ToList();

        // Get the roles the user is currently in
        var userRoles = await _userManager.GetRolesAsync(user);

        // Pass user, roles, and user's current roles to the view
        var model = new EditUserViewModel
        {
            User = user,
            Roles = roles,
            SelectedRoles = userRoles.ToList()
        };

        return View(model);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> EditUser(EditUserViewModel model)
    {
      var user = await _userManager.FindByIdAsync(model.User.Id);
      if (user == null)
      {
          return NotFound();
      }

      // Update user properties
      user.Email = model.User.Email;
      user.CustomerId = model.User.CustomerId;

      // Update user roles
      var userRoles = await _userManager.GetRolesAsync(user);
      var result = await _userManager.RemoveFromRolesAsync(user, userRoles);
      if (!result.Succeeded)
      {
          // Handle error
      }

      result = await _userManager.AddToRolesAsync(user, model.SelectedRoles);
      if (!result.Succeeded)
      {
          // Handle error
      }

      await _userManager.UpdateAsync(user);
      return RedirectToAction("ManageUsers");
    }

    [Authorize(Roles = "Admin")]
    public IActionResult ConfirmDeleteUser(string id)
    {
      var user = _userManager.Users.Single(u => u.Id == id);
      return View(user);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> DeleteUser(string Id)
    {
      var user = await _userManager.FindByIdAsync(Id);
      if (user == null)
      {
          return NotFound(); // User not found
      }

      var result = await _userManager.DeleteAsync(user);
      if (result.Succeeded)
      {
          // User deleted successfully
          return RedirectToAction("ManageUsers"); // Redirect to the list of users
      }
      else
      {
          // Failed to delete user, handle error (e.g., display error message)
          Console.WriteLine("Something went wrong trying to delete the user " + user.Email);
          return RedirectToAction("Home", "Error");
      }
    }

    [Authorize(Roles = "Admin")]
    public IActionResult ManageProducts()
    {
      var products = _repo.Products.ToList();
      return View(products);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult AddProduct()
    {
      return View("Product", new Product());
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult AddProduct(Product product)
    {
      _repo.AddProduct(product);
      return RedirectToAction("ManageProducts");
    }
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult EditProduct(int id)
    {
      var product = _repo.Products.Single(p => p.ProductId == id);
      return View("Product", product);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult EditProduct(Product product)
    {
      _repo.EditProduct(product);
      return RedirectToAction("ManageProducts");
    }
    [Authorize(Roles = "Admin")]
    public IActionResult ConfirmDeleteProduct(int id)
    {
      var product = _repo.Products.Single(p => p.ProductId == id);
      return View(product);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult DeleteProduct(Product product)
    {
      _repo.DeleteProduct(product);
      return RedirectToAction("ManageProducts");
    }

    [Authorize(Roles = "Admin")]
    public IActionResult ManageOrders(int pageNum = 1)
    {
      int defaultPageSize = 50;

      int pageSize = HttpContext.Session.GetInt32("pageSize") ?? defaultPageSize;

      var OrderManager = new OrdersListViewModel
      {
        Orders = _repo.Orders
                              .OrderBy(x => x.TransactionId)
                              .Skip((pageNum - 1) * pageSize)
                              .Take(pageSize),
        PaginationInfo = new PaginationInfo
        {
          CurrentPage = pageNum,
          ItemsPerPage = pageSize,
          TotalItems = _repo.Orders.Count()
        }
      };              
      return View(OrderManager);
    }
    [HttpPost]
    public IActionResult ChangePageSize(int pageSize)
    {
        // Validate and set the pageSize into session
        HttpContext.Session.SetInt32("pageSize", pageSize);

        // Redirect back to the Index action or wherever appropriate
        return RedirectToAction("ManageOrders");
    }
    [Authorize(Roles = "Admin")]
    public IActionResult ReviewOrders()
    {
        var records = _repo.Orders
            .Where(o => o.FraudFlag == "Fraud")
            .OrderByDescending(o => o.OrderDate);
		     //Fetch the 20 most recent records
	    

	    

	    return View(records);
    }
    [HttpGet]
    public IActionResult ReviewOrder(int id)
    {
      Order order = _repo.GetOrderById(id);
      return View(order);
    }
    [HttpPost]
    public IActionResult ApproveOrder(Order order)
    {
      order.FraudFlag = "Not Fraud";
      order.Fraud = 0;
      _repo.EditOrder(order);
      return RedirectToAction("ReviewOrders");
    }
    [HttpPost]
    public IActionResult BlockOrder(Order order)
    {
      order.FraudFlag = "Not Fraud";
      order.Fraud = 1;
      _repo.EditOrder(order);
      return RedirectToAction("ReviewOrders");
    }
}
