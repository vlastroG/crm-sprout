using Consulting.Models;

using Microsoft.EntityFrameworkCore;

namespace Consulting.API.Data.Repos {
    public abstract class ConsultingDbRepository<TEntity> : IRepository<TEntity> where TEntity : Entity {
        private readonly ConsultingDbContext _db;
        private readonly DbSet<TEntity> _dbSet;


        protected ConsultingDbRepository(ConsultingDbContext context) {
            _db = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<TEntity>() ?? throw new ArgumentException();
        }


        public virtual IQueryable<TEntity> Items => _dbSet;

        public bool AutoSaveChanges { get; set; } = true;


        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException"></exception>
        public async Task<TEntity> AddAsync(TEntity item) {
            if(item is null) { throw new ArgumentNullException(nameof(item)); }
            item.Id = 0;
            await _dbSet.AddAsync(item);
            if(AutoSaveChanges) {
                await _db.SaveChangesAsync();
            }
            return item;
        }

        public async Task<TEntity?> GetAsync(int id) {
            return await Items.SingleOrDefaultAsync(item => item.Id == id);
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException"></exception>
        public async Task UpdateAsync(TEntity item) {
            if(item is null) { throw new ArgumentNullException(nameof(item)); }
            _dbSet.Update(item);
            if(AutoSaveChanges) {
                await _db.SaveChangesAsync();
            }
        }

        public async Task<bool> RemoveAsync(int id) {
            var item = await _dbSet.FindAsync(id);
            if(item is null) {
                return false;
            } else {
                _dbSet.Remove(item);
                if(AutoSaveChanges) {
                    await _db.SaveChangesAsync();
                }
                return true;
            }
        }

        public async Task SaveChangesAsync() {
            await _db.SaveChangesAsync();
        }
    }
}
