using Microsoft.Maui.Controls;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using WorkItemApp.Models;
using WorkItemApp.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace WorkItemApp
{
    public partial class MainPage : ContentPage
    {
        private readonly IWorkItemService _workItemService;
        private readonly ILogger<MainPage> _logger;
        private readonly ILogger<AddWorkItemPage> _addPageLogger;

        public ObservableCollection<WorkItem> WorkItems { get; } = new();
        public ObservableCollection<LogMessage> LogMessages { get; } = new();
        public IAsyncRelayCommand AddNewItem { get; }
        public IAsyncRelayCommand<WorkItem> Delete { get; }
        public IAsyncRelayCommand<WorkItem> Edit { get; }
        public IRelayCommand ClearLogs { get; }

        public MainPage(IWorkItemService workItemService, ILogger<MainPage> logger, ILogger<AddWorkItemPage> addPageLogger)
        {
            InitializeComponent();
            _workItemService = workItemService;
            _logger = logger;
            _addPageLogger = addPageLogger;

            BindingContext = this;

            AddNewItem = new AsyncRelayCommand(OnAddItemClicked);
            Delete = new AsyncRelayCommand<WorkItem>(OnDeleteClicked!);
            Edit = new AsyncRelayCommand<WorkItem>(OnEditClicked!);
            ClearLogs = new RelayCommand(() => LogMessages.Clear());

            // Add custom logger provider
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddProvider(new UILoggerProvider(LogMessages));
            });
            _logger = loggerFactory.CreateLogger<MainPage>();
            _addPageLogger = loggerFactory.CreateLogger<AddWorkItemPage>();
        }

        private async void OnAddItemButtonClicked(object sender, EventArgs e)
        {
            try
            {
                _logger.LogInformation("Button clicked - Navigating to Add Work Item page");
                await Shell.Current.GoToAsync("addworkitem");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error navigating to AddWorkItemPage from button click");
                await DisplayAlert("Error", "Failed to open add item page", "OK");
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadWorkItems();
        }

        private async Task LoadWorkItems()
        {
            try
            {
                _logger.LogInformation("Loading work items...");
                var items = await _workItemService.GetWorkItems();
                WorkItems.Clear();
                foreach (var item in items)
                {
                    WorkItems.Add(item);
                }
                _logger.LogInformation($"Loaded {items.Count} work items");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading work items");
                await DisplayAlert("Error", "Failed to load work items", "OK");
            }
        }

        private async Task OnAddItemClicked()
        {
            try
            {
                _logger.LogInformation("Navigating to Add Work Item page");
                await Shell.Current.GoToAsync("addworkitem");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error navigating to AddWorkItemPage");
                await DisplayAlert("Error", "Failed to open add item page", "OK");
            }
        }

        private async Task OnDeleteClicked(WorkItem item)
        {
            try
            {
                var confirm = await DisplayAlert("Confirm Delete", 
                    $"Are you sure you want to delete the work item '{item.Title}'?", 
                    "Yes", "No");

                if (confirm)
                {
                    _logger.LogInformation($"Deleting work item {item.Id}");
                    await _workItemService.DeleteWorkItem(item.Id);
                    WorkItems.Remove(item);
                    _logger.LogInformation($"Work item {item.Id} deleted successfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting work item {item.Id}");
                await DisplayAlert("Error", "Failed to delete work item", "OK");
            }
        }

        private async Task OnEditClicked(WorkItem item)
        {
            try
            {
                _logger.LogInformation($"Navigating to edit work item {item.Id}");
                var navigationParameter = new Dictionary<string, object>
                {
                    { "WorkItem", item }
                };
                await Shell.Current.GoToAsync("editworkitem", navigationParameter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error navigating to edit page");
                await DisplayAlert("Error", "Failed to open edit page", "OK");
            }
        }
    }

    public class LogMessage
    {
        public string Message { get; set; }
        public Color Color { get; set; }

        public LogMessage(string message, Color color)
        {
            Message = message;
            Color = color;
        }
    }

    public class UILoggerProvider : ILoggerProvider
    {
        private readonly ObservableCollection<LogMessage> _logMessages;

        public UILoggerProvider(ObservableCollection<LogMessage> logMessages)
        {
            _logMessages = logMessages;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new UILogger(_logMessages, categoryName);
        }

        public void Dispose() { }
    }

    public class UILogger : ILogger
    {
        private readonly ObservableCollection<LogMessage> _logMessages;
        private readonly string _categoryName;

        public UILogger(ObservableCollection<LogMessage> logMessages, string categoryName)
        {
            _logMessages = logMessages;
            _categoryName = categoryName;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                var color = logLevel switch
                {
                    LogLevel.Trace => Colors.Gray,
                    LogLevel.Debug => Colors.LightGray,
                    LogLevel.Information => Colors.Green,
                    LogLevel.Warning => Colors.Orange,
                    LogLevel.Error => Colors.Red,
                    LogLevel.Critical => Colors.DarkRed,
                    _ => Colors.Black
                };

                var shortCategory = _categoryName.Split('.').Last();
                var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
                var message = $"[{timestamp}] [{shortCategory}] {logLevel}: {formatter(state, exception)}";
                
                if (exception != null)
                {
                    message += $"\nException: {exception.Message}";
                    if (exception.StackTrace != null)
                    {
                        message += $"\nStack Trace: {exception.StackTrace}";
                    }
                }

                _logMessages.Insert(0, new LogMessage(message, color));

                // Keep only the last 100 messages
                while (_logMessages.Count > 100)
                {
                    _logMessages.RemoveAt(_logMessages.Count - 1);
                }
            });
        }
    }
}
