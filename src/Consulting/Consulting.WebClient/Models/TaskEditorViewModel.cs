using System.ComponentModel;

namespace Consulting.WebClient.Models {
    public class TaskEditorViewModel {
        public TaskEditorViewModel() {

        }


        [DisplayName("Дата создания")]
        public DateTime CreationDate { get; set; }

        [DisplayName("ФИО")]
        public string CreatorName { get; set; } = string.Empty;

        [DisplayName("Email")]
        public string CreatorEmail { get; set; } = string.Empty;

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
    }
}
