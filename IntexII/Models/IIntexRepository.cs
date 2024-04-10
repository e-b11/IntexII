namespace IntexII.Models
{
    public interface IIntexRepository
    {
        public IQueryable<Customer> Customers { get; }
        // public Customer getCustomerById(int customerId);
        public IQueryable<Product> Products { get; }
        public void AddProduct(Product product);
        public Product GetProductById(int productId);
        public void EditProduct(Product product);
        public void DeleteProduct(Product product);
        public IQueryable<Order> Orders { get; }
        // public List<Order> getAllOrders();
        // public List<Order> getOrdersForCustomer(int customerId);
        public IQueryable<LineItem> LineItems { get; }
    }
}
