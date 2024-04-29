using System.ComponentModel.DataAnnotations;

namespace Consulting.Models {
    public class ConsultingProject : Entity, IValidatableObject {
        public ConsultingProject() : base() {
            Name = "default";
            Description = "default description";
        }


        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [Required]
        [MaxLength(1024)]
        public string Description { get; set; }

        [MaxLength(128 * 1024)]
        public byte[]? Photo { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            var textErrors = ValidateTextProperties();
            foreach(var error in textErrors) {
                yield return error;
            }
        }
    }
}
