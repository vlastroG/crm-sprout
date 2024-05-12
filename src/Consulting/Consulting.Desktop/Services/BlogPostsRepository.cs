using System.Net.Http;

using Consulting.Desktop.Helpers;
using Consulting.Models;

namespace Consulting.Desktop.Services {
    public class BlogPostsRepository : EntityRepository<BlogPost> {
        public BlogPostsRepository(AccountService accountService, IHttpClientFactory httpClientFactory)
            : base(accountService, httpClientFactory) { }


        private protected override string Url => Constants.BlogPostsUri;
    }
}
