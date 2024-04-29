using Consulting.API.Data;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Consulting.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountsController : ControllerBase {
        private readonly ConsultingDbContext _consultingDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountsController(ConsultingDbContext consultingDbContext, UserManager<ApplicationUser> userManager) {
            _consultingDbContext = consultingDbContext ?? throw new ArgumentNullException(nameof(consultingDbContext));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Create([FromHeader] string email, [FromHeader] string password) {
            if(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) {
                return BadRequest();
            }

            var user = new ApplicationUser() { Email = email, UserName = email };
            try {
                var result = await _userManager.CreateAsync(user, password);
                if(!result.Succeeded) {
                    return new BadRequestObjectResult(result.Errors);
                }

                await _consultingDbContext.SaveChangesAsync();
                return Ok();
            } catch(System.ComponentModel.DataAnnotations.ValidationException e) {
                return BadRequest(e.Message);
            }
        }
    }
}
