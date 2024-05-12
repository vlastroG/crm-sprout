using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;
using System.Windows.Input;

using Consulting.Desktop.Commands;
using Consulting.Desktop.Services;
using Consulting.Models.Exceptions;

using Microsoft.Extensions.DependencyInjection;

namespace Consulting.Desktop.ViewModels {
    public class MainWindowViewModel : BaseViewModel {
        private readonly AccountService _accountService;
        private readonly IServiceProvider _serviceProvider;
        private readonly MessageBoxService _messageBoxService;

        public MainWindowViewModel(
            AccountService accountService,
            IServiceProvider serviceProvider,
            MessageBoxService messageBoxService) {

            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
            _selectedViewModel = _serviceProvider.GetRequiredService<AnonymMainViewModel>();

            LoginCommand = new RelayCommandAsync<PasswordBox>(Login!, CanLogin!);
            LogoutCommand = new RelayCommand(Logout);
            RegisterCommand = new RelayCommandAsync<PasswordBox>(Register!, CanRegister!);
            ShowMainPageCommand = new RelayCommand(ShowMainPage);
            ShowConsultingTasksCommand = new RelayCommand(ShowConsultingTasks);
            ShowCompanyServicesCommand = new RelayCommand(ShowCompanyServices);
            ShowConsultingProjectsCommand = new RelayCommand(ShowConsultingProjects);
            ShowBlogPostsCommand = new RelayCommand(ShowBlogPosts);
            ShowContactsCommand = new RelayCommand(ShowContacts);
        }


        private BaseViewModel? _selectedViewModel;

        public BaseViewModel? SelectedViewModel {
            get => _selectedViewModel;
            set => Set(ref _selectedViewModel, value);
        }


        private string _userName = string.Empty;

        public string UserName {
            get => _userName;
            set {
                if(Set(ref _userName, value)) {
                    IsAdmin = _accountService.GetUserRole() == UserRoles.Admin;
                }
            }
        }


        public ICommand LoginCommand { get; }

        public ICommand LogoutCommand { get; }

        public ICommand RegisterCommand { get; }

        public ICommand ShowMainPageCommand { get; }

        public ICommand ShowConsultingTasksCommand { get; }

        public ICommand ShowCompanyServicesCommand { get; }

        public ICommand ShowConsultingProjectsCommand { get; }

        public ICommand ShowBlogPostsCommand { get; }

        public ICommand ShowContactsCommand { get; }


        private string? _email;
        [EmailAddress]
        public string? Email {
            get => _email;
            set => Set(ref _email, value);
        }

        private bool _isAdmin;
        public bool IsAdmin {
            get => _isAdmin;
            set => Set(ref _isAdmin, value);
        }


        private bool _commandExecuting = false;
        public bool CommandExecuting {
            get => _commandExecuting;
            set => Set(ref _commandExecuting, value);
        }


        private bool CanLogin(PasswordBox parameter) =>
            !string.IsNullOrWhiteSpace(Email)
            && parameter is not null;

        private async Task Login(PasswordBox passwordBox) {
            bool result = false;
            try {
                CommandExecuting = true;
                result = await _accountService.Login(Email!, passwordBox);
            } catch(ServerNotResponseException) {
                _messageBoxService.ShowError(
                    "Сервер не отвечает",
                    "Статус входа");
                return;
            } finally {
                passwordBox.Password = string.Empty;
                Email = string.Empty;
                CommandExecuting = false;
                ShowMainPage();
            }

            if(result) {
                _messageBoxService.ShowInfo(
                    "Вход успешно выполнен, можете закрыть окно",
                    "Статус входа");
            } else {
                _messageBoxService.ShowError(
                    "Неверный логин пользователя или пароль",
                    "Статус входа");
            }
        }


        private void Logout() {
            _accountService.Logout();
            ShowMainPage();
        }

        private bool CanRegister(PasswordBox p) =>
            !string.IsNullOrWhiteSpace(Email)
            && p is not null;

        private async Task Register(PasswordBox passwordBox) {
            if(passwordBox.Password.Length < 8) {
                _messageBoxService.ShowError("Пароль должен быть не менее 8 символов", "Статус регистрации");
                return;
            }

            bool result = false;
            try {
                CommandExecuting = true;
                result = await _accountService.Register(Email!, passwordBox);
            } catch(ServerNotResponseException) {
                _messageBoxService.ShowError(
                    "Сервер не отвечает",
                    "Статус регистрации");
                return;
            } finally {
                passwordBox.Password = string.Empty;
                Email = string.Empty;
                CommandExecuting = false;
            }

            if(result) {
                _messageBoxService.ShowInfo(
                    $"Пользователь {Email} успешно зарегистрирован, можете закрыть окно",
                    "Статус регистрации");
            } else {
                _messageBoxService.ShowError(
                    $"Не удалось зарегистрировать пользователя {Email}",
                    "Статус регистрации");
            }
        }


        private void ShowMainPage() {
            ShowPage<AnonymMainViewModel>();
        }

        private void ShowConsultingTasks() {
            ShowPage<AdminConsultingTasksViewModel, AnonymMainViewModel>();
        }

        private void ShowCompanyServices() {
            ShowPage<AdminCompanyServicesViewModel, AnonymCompanyServicesViewModel>();
        }

        private void ShowConsultingProjects() {
            ShowPage<AdminConsultingProjectsViewModel, AnonymConsultingProjectsViewModel>();
        }

        private void ShowBlogPosts() {
            ShowPage<AdminBlogPostsViewModel, AnonymBlogPostsViewModel>();
        }

        private void ShowContacts() {
            ShowPage<AnonymContactsViewModel>();
        }

        private void ShowPage<T>() where T : UpdatableViewModel {
            UserName = _accountService.GetUserName();
            var viewModel = _serviceProvider.GetRequiredService<T>();
            viewModel.UpdateCommand?.Execute(null);
            SelectedViewModel = viewModel;
        }

        private void ShowPage<TAdmin, TAnon>() where TAdmin : UpdatableViewModel where TAnon : UpdatableViewModel {
            switch(_accountService.GetUserRole()) {
                case UserRoles.Admin:
                    ShowPage<TAdmin>();
                    break;
                default:
                    ShowPage<TAnon>();
                    break;
            };
        }
    }
}
