namespace IntexII.Models
{
    public interface IIntexRepository
    {
        public IQueryable<Customer> Customers { get; }
        // public Customer getCustomerById(int customerId);
        public IQueryable<Product> Products { get; }
        //public List<Product> getAllProducts();
        // public Product getProductById(int productId);
        public IQueryable<Order> Orders { get; }
        // public List<Order> getAllOrders();
        // public List<Order> getOrdersForCustomer(int customerId);
        public IQueryable<LineItem> LineItems { get; }
    }
}
