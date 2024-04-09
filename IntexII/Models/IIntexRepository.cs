namespace IntexII.Models
{
    public interface IIntexRepository
    {
        IQueryable<Customer> Customers { get; }
        // public Customer getCustomerById(int customerId);
        IQueryable<Product> Products { get; }
        public List<Product> getAllProducts();
        // public Product getProductById(int productId);
        IQueryable<Order> Orders { get; }
        // public List<Order> getAllOrders();
        // public List<Order> getOrdersForCustomer(int customerId);
        IQueryable<LineItem> LineItems { get; }
    }
}
