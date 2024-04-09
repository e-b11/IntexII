namespace IntexII.Models
{
    public class EFIntexRepository : IIntexRepository
    {
        private IntexContext _context;

        public EFIntexRepository(IntexContext temp)
        {
            _context = temp;
        }
    }
}
