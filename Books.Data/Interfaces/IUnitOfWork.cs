using System.Threading.Tasks;
using Books.Data.Entities;

namespace Books.Data.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Book> BooksRepository { get; }

        Task SaveAsync();
    }
}