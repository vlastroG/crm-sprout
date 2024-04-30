using System.ComponentModel.DataAnnotations;

namespace Consulting.Models {
    public class BlogPost : Entity, IValidatableObject {
        public BlogPost() : base() {
            Name = "default";
            ContentShort = "default";
        }


        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        public string ContentShort { get; set; }

        [MaxLength(1024)]
        public string? ContentFull { get; set; }

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
