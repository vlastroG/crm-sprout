using Consulting.Models;

namespace Consulting.API.Data.Repos {
    public class ConsultingProjectsRepository : ConsultingDbRepository<ConsultingProject> {
        public ConsultingProjectsRepository(ConsultingDbContext context) : base(context) {
        }
    }
}
