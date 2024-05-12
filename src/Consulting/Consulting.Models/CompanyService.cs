using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Consulting.Models {
    public class CompanyService : Entity, IEquatable<CompanyService>, IValidatableObject {
        public CompanyService() : base() {
            Name = "default";
            Description = "default description";
        }


        [Required]
        [MaxLength(32)]
        [DisplayName("Название")]
        public string Name { get; set; }

        [Required]
        [MaxLength(512)]
        [DisplayName("Описание")]
        public string Description { get; set; }


        public bool Equals(CompanyService? other) {
            if(other is null) { return false; }
            if(ReferenceEquals(this, other)) { return true; }

            return Id == other.Id
                && Name == other.Name
                && Description == other.Description
                ;
        }

        public override int GetHashCode() {
            return HashCode.Combine(Id, Name, Description);
        }

        public override bool Equals(object? obj) {
            return base.Equals(obj as CompanyService);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            var textErrors = ValidateTextProperties();
            foreach(var error in textErrors) {
                yield return error;
            }
        }
    }
}
