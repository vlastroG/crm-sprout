using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Consulting.WebClient.Attributes;

using Microsoft.AspNetCore.Mvc;

namespace Consulting.WebClient.Models {

    public class ProjectViewModel : ValidatableViewModel, IValidatableObject {
        public ProjectViewModel() {

        }

        [Required]
        public int Id { get; set; } = 0;

        [Required]
        [MaxLength(64)]
        [DisplayName("Название")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(1024)]
        [DisplayName("Описание")]
        public string Description { get; set; } = string.Empty;

        [BindProperty]
        [DisplayName("Фото")]
        [IFormFileMaxLength(1024 * 128, ErrorMessage = "File must be not greater than 128KB")]
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
