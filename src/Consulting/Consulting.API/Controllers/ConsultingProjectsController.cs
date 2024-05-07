using System.ComponentModel.DataAnnotations;
using System.Reflection;

using Consulting.API.Data.Repos;
using Consulting.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;

namespace Consulting.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Helpers.Constants.Strings.AuthPolicy.AdminPolicy)]
    public class ConsultingProjectsController : ControllerBase {
        private readonly IRepository<ConsultingProject> _repository;
        private readonly int _photoMaxLength;
        private const int _photoHeight = 225;
        private const int _photoWidth = 400;
        private const double _tolerance = 0.1;

        public ConsultingProjectsController(IRepository<ConsultingProject> repository) {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _photoMaxLength = typeof(ConsultingProject)
                .GetProperty(nameof(ConsultingProject.Photo))!
                .GetCustomAttribute<MaxLengthAttribute>()!
                .Length;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<ConsultingProject>> Get() {
            return await _repository.Items.ToListAsync();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id) {
            var item = await _repository.GetAsync(id);
            return item != null ? Ok(item) : NotFound();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ConsultingProject item) {
            if(item is null) { return BadRequest(); }
            if(!ImageIsValid(item.Photo ?? Array.Empty<byte>())) {
                return BadRequest(
                    $"Photo size must be not greater than {_photoMaxLength / 1024} KB, " +
                    $"width must be {_photoWidth}, height must be {_photoHeight}");
            }
            try {
                await _repository.AddAsync(item);
                return Ok();
            } catch(ValidationException e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ConsultingProject item) {
            if(item is null || id != item.Id || id < 1) { return BadRequest(); }
            if(!ImageIsValid(item.Photo ?? Array.Empty<byte>())) {
                return BadRequest(
                    $"Photo size must be not greater than {_photoMaxLength / 1024} KB, " +
                    $"width must be {_photoWidth}, height must be {_photoHeight}");
            }
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

        private bool ImageIsValid(byte[] imgBytes) {
            if(imgBytes.Length == 0) { return true; }

            if(imgBytes.Length <= _photoMaxLength) {
                using(var ms = new MemoryStream(imgBytes)) {
                    IImage image = PlatformImage.FromStream(ms, ImageFormat.Jpeg);
                    var width = image.Height; //don't know why
                    var height = image.Width; //don't know why
                    return Math.Round(Math.Abs(width - _photoWidth), 5) <= _tolerance
                        && Math.Round(Math.Abs(height - _photoHeight), 5) <= _tolerance;
                }
            } else {
                return false;
            }
        }
    }
}
