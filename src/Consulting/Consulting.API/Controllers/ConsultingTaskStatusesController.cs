using Consulting.API.Data;
using Consulting.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Consulting.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ConsultingTaskStatusesController : ControllerBase {
        private readonly ConsultingDbContext _context;

        public ConsultingTaskStatusesController(ConsultingDbContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        [HttpGet]
        public async Task<IEnumerable<ConsultingTaskStatus>> Get() {
            return await _context.ConsultingTaskStatuses.ToListAsync();
        }
    }
}
