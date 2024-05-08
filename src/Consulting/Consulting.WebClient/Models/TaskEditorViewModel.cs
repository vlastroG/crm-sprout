using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Consulting.WebClient.Models {
    public class TaskEditorViewModel : ValidatableViewModel, IValidatableObject {
        public TaskEditorViewModel() {

        }


        [DisplayName("Дата создания")]
        public DateTime CreationDate { get; set; }

        [Required]
        [MaxLength(64)]
        [DisplayName("ФИО")]
        public string CreatorName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string CreatorEmail { get; set; } = string.Empty;

        [Required]
        [MaxLength(512)]
        [DisplayName("Описание заявки")]
        public string Description { get; set; } = string.Empty;

        [DisplayName("Вид услуги")]
        public string CompanyService { get; set; } = string.Empty;

        [DisplayName("Id услуги")]
        public int CompanyServiceId { get; set; }

        [DisplayName("Id")]
        public int ConsultingTaskId { get; set; }

        [DisplayName("Статус")]
        public int StatusId { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            var textErrors = ValidateTextProperties();
            foreach(var error in textErrors) {
                yield return error;
            }
        }
    }
}
