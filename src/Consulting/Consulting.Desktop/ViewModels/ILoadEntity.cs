using Consulting.Models;

namespace Consulting.Desktop.ViewModels {
    public interface ILoadEntity<T> where T : Entity {
        void LoadEntity(T entity);
    }
}
