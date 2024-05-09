using System.Net.Http;

using Consulting.Desktop.Helpers;
using Consulting.Models;

namespace Consulting.Desktop.Services {
    public class ConsultingTasksRepository : EntityRepository<ConsultingTask> {
        public ConsultingTasksRepository(AccountService accountService, IHttpClientFactory httpClientFactory)
            : base(accountService, httpClientFactory) { }


        private protected override string Url => Constants.ConsultingTasksUri;


        public override Task<bool> Delete(int id) {
            throw new NotSupportedException();
        }
    }
}
