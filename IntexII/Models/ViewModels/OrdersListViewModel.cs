using IntexII.Models;
using IntexII.Models.ViewModels;

namespace IntexII.ViewModels
{
  public class OrdersListViewModel
  {
    public IQueryable<Order> Orders { get; set; }
    public PaginationInfo PaginationInfo { get; set;} = new PaginationInfo();
    // public int? FraudFlag { get; set; } 
    // public string? CountryOfTransaction { get; set; }
    // public DateTime? OrderDate { get; set;}
  }
}