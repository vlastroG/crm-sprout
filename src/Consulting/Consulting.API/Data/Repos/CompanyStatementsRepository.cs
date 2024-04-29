using Consulting.Models;

namespace Consulting.API.Data.Repos {
    public class CompanyStatementsRepository : ConsultingDbRepository<CompanyStatement> {
        public CompanyStatementsRepository(ConsultingDbContext context) : base(context) {
        }
    }
}
