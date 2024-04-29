namespace Consulting.API.Helpers {
    public enum TaskStatuses {
        /// <summary>
        /// Получена
        /// </summary>
        New = 1,
        /// <summary>
        /// В работе
        /// </summary>
        InProgress = 2,
        /// <summary>
        /// Выполнена
        /// </summary>
        Completed = 3,
        /// <summary>
        /// Отклонена
        /// </summary>
        Rejected = 4,
        /// <summary>
        /// Отменена
        /// </summary>
        Canceled = 5
    }
}
