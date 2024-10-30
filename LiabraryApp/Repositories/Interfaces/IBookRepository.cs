using LiabraryApp.Models;
using System.Collections;

namespace LiabraryApp.Repositories.Interfaces
{
    public interface IBookRepository : IGenericRepository<CBook>
    {
        Task<bool> ReturningBook(Hashtable criteria);

        Task<bool> BorrowingBook(Hashtable criteria);


    }
}
