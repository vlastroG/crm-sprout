using Consulting.Models;

namespace Consulting.API.Data.Repos {
    public class CompanyServicesRepository : ConsultingDbRepository<CompanyService> {
        public CompanyServicesRepository(ConsultingDbContext context) : base(context) {
        }
    }
}
