using System.ComponentModel.DataAnnotations;

using Consulting.API.Data.Repos;
using Consulting.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Consulting.API.Controllers {
    [Route("api/Statements")]
    [ApiController]
    [Authorize(Helpers.Constants.Strings.AuthPolicy.AdminPolicy)]
    public class CompanyStatementsController : ControllerBase {
        private readonly IRepository<CompanyStatement> _companyStatementsRepo;

        public CompanyStatementsController(IRepository<CompanyStatement> companyStatementsRepo) {
            _companyStatementsRepo = companyStatementsRepo ?? throw new ArgumentNullException(nameof(companyStatementsRepo));
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<CompanyStatement>> Get() {
            return await _companyStatementsRepo.Items.ToListAsync();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id) {
            var item = await _companyStatementsRepo.GetAsync(id);
            return item != null ? Ok(item) : NotFound();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CompanyStatement item) {
            if(item is null) { return BadRequest(); }

            try {
                await _companyStatementsRepo.AddAsync(item);
                return Ok();
            } catch(ValidationException e) { return BadRequest(e.Message); }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id) {
            bool result = await _companyStatementsRepo.RemoveAsync(id);
            return result ? Ok() : NotFound();
        }
    }
}
