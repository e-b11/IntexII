namespace IntexII.Models
{
    public class EFIntexRepository : IIntexRepository
    {
        private IntexContext _context;

        public EFIntexRepository(IntexContext temp)
        {
            _context = temp;
        }

        public IQueryable<Customer> Customers => _context.Customers;
        public IQueryable<Product> Products => _context.Products;
        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }
        public Product GetProductById(int id)
        {
            var product = _context.Products.Single(p => p.ProductId == id);
            return product;
        }
        public void EditProduct(Product product)
        {
            _context.Update(product);
            _context.SaveChanges();
        }
        public void DeleteProduct(Product product)
        {
            _context.Remove(product);
        }

        public IQueryable<Order> Orders => _context.Orders;
        public IQueryable<LineItem> LineItems => _context.LineItems;

    }
}
