using Consulting.Models;

namespace Consulting.Desktop.Services {
    public interface IRepository<T> where T : Entity {
        Task<bool> Create(T item);

        Task<IEnumerable<T>> Get();

        Task<bool> Update(T item);

        Task<bool> Delete(int id);
    }
}
