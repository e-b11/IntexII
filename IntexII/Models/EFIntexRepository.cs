namespace IntexII.Models
{
    public class EFIntexRepository : IIntexRepository
    {
        private IntexContext _context;

        public EFIntexRepository(IntexContext temp)
        {
            _context = temp;
        }

        IQueryable<Customer> IIntexRepository.Customers => _context.Customers;
        IQueryable<Product> IIntexRepository.Products => _context.Products;
        public List<Product> getAllProducts()
        {
            return _context.Products.ToList();
        }
        IQueryable<Order> IIntexRepository.Orders => _context.Orders;
        IQueryable<LineItem> IIntexRepository.LineItems => _context.LineItems;
    }
}
