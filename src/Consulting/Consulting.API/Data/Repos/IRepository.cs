using Consulting.Models;

namespace Consulting.API.Data.Repos {
    public interface IRepository<TEntity> where TEntity : Entity {
        IQueryable<TEntity> Items { get; }
        bool AutoSaveChanges { get; set; }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException"></exception>
        Task<TEntity> AddAsync(TEntity item);
        Task<TEntity?> GetAsync(int id);
        Task<bool> RemoveAsync(int id);

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException"></exception>
        Task UpdateAsync(TEntity item);
        Task SaveChangesAsync();
    }
}
