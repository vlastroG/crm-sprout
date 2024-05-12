using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Consulting.Models {
    public class ConsultingTaskStatus : Entity, IEquatable<ConsultingTaskStatus>, IValidatableObject {
        public ConsultingTaskStatus() : base() {
            Name = "default";
        }


        [Required]
        [MaxLength(32)]
        [DisplayName("Название")]
        public string Name { get; set; }

        public override bool Equals(object? obj) {
            return base.Equals(obj as ConsultingTaskStatus);
        }

        public bool Equals(ConsultingTaskStatus? other) {
            if(other is null) { return false; }
            if(ReferenceEquals(this, other)) { return true; }

            return Id == other.Id &&
                   Name == other.Name
                   ;
        }

        public override int GetHashCode() {
            return HashCode.Combine(Id, Name);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            var textErrors = ValidateTextProperties();
            foreach(var error in textErrors) {
                yield return error;
            }
        }
    }
}
