using System.Net.Http;

using Consulting.Desktop.Helpers;
using Consulting.Models;

namespace Consulting.Desktop.Services {
    public class CompanyServicesRepository : EntityRepository<CompanyService> {
        public CompanyServicesRepository(AccountService accountService, IHttpClientFactory httpClientFactory)
            : base(accountService, httpClientFactory) { }


        private protected override string Url => Constants.CompanyServicesUri;
    }
}
