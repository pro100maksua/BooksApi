using System.Threading.Tasks;
using BooksApi.Data.Interfaces;
using BooksApi.Entities;

namespace BooksApi.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IRepository<Book> _booksRepository;

        public IRepository<Book> BooksRepository =>
            _booksRepository ?? (_booksRepository = new Repository<Book>(_context));

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}