using System.Collections;

namespace LiabraryApp.Repositories.Interfaces
{
    public interface IGenericRepository<T>where T : class
    {
        Task<List<T>> Get(Hashtable criteria);
        Task<T> Save(T entity);
        Task<T> Edit(T entity);
        Task<bool> Delete(int id);


    }
}
