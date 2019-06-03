using System.Threading.Tasks;
using BooksApi.Entities;

namespace BooksApi.Data.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Book> BooksRepository { get; }

        Task SaveAsync();
    }
}