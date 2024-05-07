using System.ComponentModel.DataAnnotations;

using Consulting.WebClient.Attributes;

using Microsoft.AspNetCore.Mvc;

namespace Consulting.WebClient.Models {

    public class ProjectViewModel {
        public ProjectViewModel() {

        }

        [Required]
        public int Id { get; set; } = 0;

        [Required]
        [MaxLength(64)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(1024)]
        public string Description { get; set; } = string.Empty;

        [BindProperty]
        [IFormFileMaxLength(1024 * 128, ErrorMessage = "File must be not greater than 128KB")]
        public IFormFile? Photo { get; set; }
    }
}
