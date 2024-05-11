using Consulting.Models;
using Consulting.Models.Exceptions;

namespace Consulting.Desktop.Services {
    public interface IRepository<T> where T : Entity {
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UnauthorizedUserException"></exception>
        /// <exception cref="AccessDeniedException"></exception>
        /// <exception cref="ServerNotResponseException"></exception>
        Task<bool> Create(T item);

        /// <exception cref="UnauthorizedUserException"></exception>
        /// <exception cref="AccessDeniedException"></exception>
        /// <exception cref="ServerNotResponseException"></exception>
        Task<IEnumerable<T>> Get();

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UnauthorizedUserException"></exception>
        /// <exception cref="AccessDeniedException"></exception>
        /// <exception cref="ServerNotResponseException"></exception>
        Task<bool> Update(T item);

        /// <exception cref="UnauthorizedUserException"></exception>
        /// <exception cref="AccessDeniedException"></exception>
        /// <exception cref="ServerNotResponseException"></exception>
        Task<bool> Delete(int id);
    }
}
