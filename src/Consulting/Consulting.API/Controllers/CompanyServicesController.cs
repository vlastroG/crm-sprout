using System.ComponentModel.DataAnnotations;

using Consulting.API.Data.Repos;
using Consulting.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Consulting.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Helpers.Constants.Strings.AuthPolicy.AdminPolicy)]
    public class CompanyServicesController : ControllerBase {
        private readonly IRepository<CompanyService> _repository;

        public CompanyServicesController(IRepository<CompanyService> repository) {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<CompanyService>> Get() {
            return await _repository.Items.ToListAsync();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id) {
            var item = await _repository.GetAsync(id);
            return item != null ? Ok(item) : NotFound();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CompanyService item) {
            if(item is null) { return BadRequest(); }

            try {
                await _repository.AddAsync(item);
                return Ok();
            } catch(ValidationException e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CompanyService item) {
            if(item is null || id != item.Id || id < 1) { return BadRequest(); }

            try {
                var existItem = await _repository.GetAsync(item.Id);
                if(existItem is null) {
                    return NotFound();
                }
                await _repository.UpdateAsync(item);
                return Ok();
            } catch(ValidationException e) {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id) {
            bool success = await _repository.RemoveAsync(id);
            return success ? Ok() : NotFound();
        }
    }
}
