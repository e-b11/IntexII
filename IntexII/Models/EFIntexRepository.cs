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
        public IQueryable<Order> Orders => _context.Orders;
        public IQueryable<LineItem> LineItems => _context.LineItems;
    }
}
