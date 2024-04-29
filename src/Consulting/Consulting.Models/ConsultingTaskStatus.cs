using System.ComponentModel.DataAnnotations;

namespace Consulting.Models {
    public class ConsultingTaskStatus : Entity, IValidatableObject {
        public ConsultingTaskStatus() : base() {
            Name = "default";
        }


        [Required]
        [MaxLength(32)]
        public string Name { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            var textErrors = ValidateTextProperties();
            foreach(var error in textErrors) {
                yield return error;
            }
        }
    }
}
