using System.ComponentModel.DataAnnotations;

using Consulting.API.Data;
using Consulting.API.Data.Repos;
using Consulting.API.Helpers;
using Consulting.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Consulting.API.Controllers {
    [Route("api/Tasks")]
    [ApiController]
    [Authorize(Helpers.Constants.Strings.AuthPolicy.AdminPolicy)]
    public class ConsultingTasksController : ControllerBase {
        private readonly IRepository<ConsultingTask> _repository;
        private readonly ConsultingDbContext _context;

        public ConsultingTasksController(IRepository<ConsultingTask> consultingTasksRepo, ConsultingDbContext context) {
            _repository = consultingTasksRepo ?? throw new ArgumentNullException(nameof(consultingTasksRepo));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        [HttpGet]
        public async Task<IEnumerable<ConsultingTask>> Get() {
            return await _repository.Items.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) {
            var item = await _repository.GetAsync(id);
            return item != null ? Ok(item) : NotFound();
        }

        [HttpPost("Create")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] ConsultingTask item) {
            if(item is null) {
                return BadRequest();
            }

            try {
                var service = await _context.CompanyServices.FindAsync(item.CompanyService?.Id);
                if(service is null) {
                    return BadRequest("Company service doesn't exist.");
                }
                item.CompanyService = service;
                item.Status = await _context.ConsultingTaskStatuses.FindAsync(TaskStatuses.New);
                await _repository.AddAsync(item);
                return Ok();
            } catch(ValidationException e) { return BadRequest(e.Message); }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ConsultingTask item) {
            if(item is not null && item.Id == id && id > 0) {
                try {
                    var existItem = await _repository.GetAsync(item.Id);
                    if(existItem is null) {
                        return NotFound();
                    }
                    var status = await _context.ConsultingTaskStatuses.FindAsync(item.Status?.Id);
                    if(status is null) {
                        return BadRequest("Task status doesn't exist.");
                    }
                    var service = await _context.CompanyServices.FindAsync(item.CompanyService?.Id);
                    if(service is null) {
                        return BadRequest("Company service doesn't exist.");
                    }
                    item.CompanyService = service;
                    item.Status = status;
                    await _repository.UpdateAsync(item);
                    return Ok();
                } catch(ValidationException e) { return BadRequest(e.Message); }
            } else {
                return BadRequest();
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id) {
            bool success = await _repository.RemoveAsync(id);
            return success ? Ok() : NotFound();
        }
    }
}
