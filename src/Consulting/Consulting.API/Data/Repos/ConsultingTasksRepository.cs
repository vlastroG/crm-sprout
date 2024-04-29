using Consulting.Models;

using Microsoft.EntityFrameworkCore;

namespace Consulting.API.Data.Repos {
    public class ConsultingTasksRepository : ConsultingDbRepository<ConsultingTask> {
        public ConsultingTasksRepository(ConsultingDbContext context) : base(context) {
        }


        public override IQueryable<ConsultingTask> Items =>
            base.Items
            .Include(item => item.CompanyService)
            .Include(item => item.Status)
            ;
    }
}
