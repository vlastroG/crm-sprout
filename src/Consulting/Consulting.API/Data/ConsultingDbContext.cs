using System.ComponentModel.DataAnnotations;

using Consulting.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Consulting.API.Data {
    public class ConsultingDbContext : IdentityDbContext<ApplicationUser> {
        public ConsultingDbContext(DbContextOptions<ConsultingDbContext> options) : base(options) {
        }


        public DbSet<CompanyService> CompanyServices { get; set; }

        public DbSet<ConsultingTaskStatus> ConsultingTaskStatuses { get; set; }

        public DbSet<ConsultingTask> ConsultingTasks { get; set; }

        public DbSet<ConsultingProject> ConsultingProjects { get; set; }

        public DbSet<BlogPost> BlogPosts { get; set; }

        public DbSet<CompanyStatement> CompanyStatements { get; set; }


        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<ConsultingTaskStatus>().HasData(
                new ConsultingTaskStatus() {
                    Id = 1,
                    Name = "Получена"
                },
                new ConsultingTaskStatus() {
                    Id = 2,
                    Name = "В работе"
                },
                new ConsultingTaskStatus() {
                    Id = 3,
                    Name = "Выполнена"
                },
                new ConsultingTaskStatus() {
                    Id = 4,
                    Name = "Отклонена"
                },
                new ConsultingTaskStatus() {
                    Id = 5,
                    Name = "Отменена"
                }
            );

            base.OnModelCreating(builder);
        }

        public override int SaveChanges() {
            ChangeTracker.Entries()
                .Where(e => e.State is EntityState.Added or EntityState.Modified)
                .Select(e => e.Entity)
                .ToList()
                .ForEach(entity => {
                    var validationContext = new ValidationContext(entity);
                    Validator.ValidateObject(
                        entity,
                        validationContext,
                        validateAllProperties: true);
                });

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
            ChangeTracker.Entries()
                .Where(e => e.State is EntityState.Added or EntityState.Modified)
                .Select(e => e.Entity)
                .ToList()
                .ForEach(entity => {
                    var validationContext = new ValidationContext(entity);
                    Validator.ValidateObject(
                        entity,
                        validationContext,
                        validateAllProperties: true);
                });

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
