using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Consulting.WebClient.Attributes;

using Microsoft.AspNetCore.Mvc;

namespace Consulting.WebClient.Models {
    public class BlogPostViewModel : ValidatableViewModel, IValidatableObject {
        public BlogPostViewModel() {

        }


        [Required]
        public int Id { get; set; } = 0;

        [Required]
        [MaxLength(32)]
        [DisplayName("Название")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(256)]
        [DisplayName("Контент сокращенно")]
        public string ContentShort { get; set; } = string.Empty;

        [MaxLength(1024)]
        public string? ContentFull { get; set; }

        [BindProperty]
        [IFormFileMaxLength(1024 * 128, ErrorMessage = "File must be not greater than 128KB")]
        [DisplayName("Контент полностью")]
        public IFormFile? Photo { get; set; }

        [DisplayName("Текущее фото")]
        public string? ExistPhoto { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            var textErrors = ValidateTextProperties();
            foreach(var error in textErrors) {
                yield return error;
            }
        }
    }
}
