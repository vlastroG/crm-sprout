using Consulting.Models;

namespace Consulting.API.Data.Repos {
    public class BlogPostsRepository : ConsultingDbRepository<BlogPost> {
        public BlogPostsRepository(ConsultingDbContext context) : base(context) {
        }
    }
}
