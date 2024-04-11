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

namespace IntexII.Controllers;
public class AdminController : Controller
{
    private IIntexRepository _repo;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly InferenceSession _session;
    private readonly IWebHostEnvironment _environment;
    

    public AdminController(UserManager<ApplicationUser> userManager, IIntexRepository temp, IWebHostEnvironment environment, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _repo = temp;
        _environment = environment;
        // Combine the base path with the relative path to your ONNX file
        string modelPath = Path.Combine(_environment.ContentRootPath, "decision_tree_model.onnx");
        _session = new InferenceSession(modelPath);
    }

    public IActionResult Home()
    {
      return View("AdminHome");
    }
    public IActionResult ManageUsers()
    {
        // Retrieve all users from the database
        var users = _userManager.Users.ToList();

        // Pass the list of users to the view
        return View(users);
    }
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

    public IActionResult ConfirmDeleteUser(string id)
    {
      var user = _userManager.Users.Single(u => u.Id == id);
      return View(user);
    }
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



    public IActionResult ManageProducts()
    {
      var products = _repo.Products.ToList();
      return View(products);
    }
    [HttpGet]
    public IActionResult AddProduct()
    {
      return View("Product", new Product());
    }
    [HttpPost]
    public IActionResult AddProduct(Product product)
    {
      _repo.AddProduct(product);
      return RedirectToAction("ManageProducts");
    }
    [HttpGet]
    public IActionResult EditProduct(int id)
    {
      var product = _repo.Products.Single(p => p.ProductId == id);
      return View("Product", product);
    }
    [HttpPost]
    public IActionResult EditProduct(Product product)
    {
      _repo.EditProduct(product);
      return RedirectToAction("ManageProducts");
    }
    public IActionResult ConfirmDeleteProduct(int id)
    {
      var product = _repo.Products.Single(p => p.ProductId == id);
      return View(product);
    }
    public IActionResult DeleteProduct(Product product)
    {
      _repo.DeleteProduct(product);
      return RedirectToAction("ManageProducts");
    }

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
    
    public IActionResult ReviewOrders()
    {
        var records = _repo.Orders
            .Where(o => o.FraudFlag == "Fraud")
            .OrderByDescending(o => o.OrderDate);
		     //Fetch the 20 most recent records
	    

	    

	    return View(records);
    }

}
