using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Consulting.Models {
    public class ConsultingTask : Entity, IValidatableObject {
        public ConsultingTask() : base() {
            CreationDate = DateTime.Now;
            CreatorName = string.Empty;
            CreatorEmail = string.Empty;
            Description = string.Empty;
        }


        [Required]
        [DisplayName("Дата создания")]
        public DateTime CreationDate { get; set; }

        [Required]
        [MaxLength(64)]
        [DisplayName("ФИО")]
        public string CreatorName { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string CreatorEmail { get; set; }

        [Required]
        [MaxLength(512)]
        [DisplayName("Описание заявки")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Статус")]
        public ConsultingTaskStatus? Status { get; set; }

        [Required]
        [DisplayName("Вид услуги")]
        public CompanyService? CompanyService { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            var textErrors = ValidateTextProperties();
            foreach(var error in textErrors) {
                yield return error;
            }

            if(CreationDate > DateTime.Now) {
                yield return new ValidationResult("Cannot create task in future", new[] { nameof(CreationDate) });
            }
        }
    }
}
