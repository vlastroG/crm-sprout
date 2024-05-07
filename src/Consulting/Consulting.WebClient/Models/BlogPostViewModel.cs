using System.ComponentModel.DataAnnotations;

using Consulting.WebClient.Attributes;

using Microsoft.AspNetCore.Mvc;

namespace Consulting.WebClient.Models {
    public class BlogPostViewModel {
        public BlogPostViewModel() {

        }


        [Required]
        public int Id { get; set; } = 0;

        [Required]
        [MaxLength(32)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(256)]
        public string ContentShort { get; set; } = string.Empty;

        [MaxLength(1024)]
        public string? ContentFull { get; set; }

        [BindProperty]
        [IFormFileMaxLength(1024 * 128, ErrorMessage = "File must be not greater than 128KB")]
        public IFormFile? Photo { get; set; }
    }
}
