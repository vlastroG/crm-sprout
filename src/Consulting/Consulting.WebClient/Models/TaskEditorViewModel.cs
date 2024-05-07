using System.ComponentModel;

namespace Consulting.WebClient.Models {
    public class TaskEditorViewModel {
        public TaskEditorViewModel() {

        }


        [DisplayName("Дата создания")]
        public DateTime CreationDate { get; set; }

        [DisplayName("ФИО")]
        public string CreatorName { get; set; }

        [DisplayName("Email")]
        public string CreatorEmail { get; set; }

        [DisplayName("Описание заявки")]
        public string Description { get; set; }

        [DisplayName("Вид услуги")]
        public string CompanyService { get; set; }

        [DisplayName("Id")]
        public int ConsultingTaskId { get; set; }

        public int StatusId { get; set; }
    }
}
