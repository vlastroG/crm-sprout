using System.Net.Http;

using Consulting.Desktop.Helpers;
using Consulting.Models;

namespace Consulting.Desktop.Services {
    public class ConsultingTaskStatusesRepository : EntityRepository<ConsultingTaskStatus> {
        public ConsultingTaskStatusesRepository(AccountService accountService, IHttpClientFactory httpClientFactory)
            : base(accountService, httpClientFactory) { }


        private protected override string Url => Constants.ConsultingTasksUri;


        public override Task<bool> Create(ConsultingTaskStatus item) {
            throw new NotSupportedException();
        }

        public override Task<bool> Update(ConsultingTaskStatus item) {
            throw new NotSupportedException();
        }

        public override Task<bool> Delete(int id) {
            throw new NotSupportedException();
        }
    }
}
