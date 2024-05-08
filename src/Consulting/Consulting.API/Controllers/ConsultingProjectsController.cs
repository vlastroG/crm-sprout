using System.ComponentModel.DataAnnotations;
using System.Reflection;

using Consulting.API.Data.Repos;
using Consulting.API.Services;
using Consulting.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Consulting.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Helpers.Constants.Strings.AuthPolicy.AdminPolicy)]
    public class ConsultingProjectsController : ControllerBase {
        private readonly IRepository<ConsultingProject> _repository;
        private readonly ImageValidator _imageValidator;
        private readonly int _photoMaxLength;
        private const int _photoHeight = 225;
        private const int _photoWidth = 400;

        public ConsultingProjectsController(IRepository<ConsultingProject> repository, ImageValidator imageValidator) {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _imageValidator = imageValidator ?? throw new ArgumentNullException(nameof(imageValidator));
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
            if(!_imageValidator.ImageIsValid(item.Photo ?? Array.Empty<byte>(), _photoWidth, _photoHeight, _photoMaxLength)) {
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
            if(!_imageValidator.ImageIsValid(item.Photo ?? Array.Empty<byte>(), _photoWidth, _photoHeight)) {
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
    }
}
