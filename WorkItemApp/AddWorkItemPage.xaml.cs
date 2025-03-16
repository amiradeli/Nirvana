using Microsoft.Extensions.Logging;
using WorkItemApp.Models;
using WorkItemApp.Services;
using System.Text.Json;
using System.Text.Json.Serialization;
using Refit;

namespace WorkItemApp;

public partial class AddWorkItemPage : ContentPage
{
    private readonly IWorkItemService _workItemService;
    private readonly ILogger<AddWorkItemPage> _logger;

    public AddWorkItemPage(IWorkItemService workItemService, ILogger<AddWorkItemPage> logger)
    {
        InitializeComponent();
        _workItemService = workItemService;
        _logger = logger;

        // Initialize the status picker
        StatusPicker.ItemsSource = new List<string> { "Not Started", "In Progress", "Completed" };
        StatusPicker.SelectedIndex = 0;

        // Initialize the type picker with enum values
        TypePicker.ItemsSource = Enum.GetValues(typeof(WorkItemType)).Cast<WorkItemType>().ToList();
        TypePicker.SelectedIndex = 0; // Default to first item

        // Set default dates
        StartDatePicker.Date = DateTime.Today;
        DueDatePicker.Date = DateTime.Today.AddDays(7);
        EndDatePicker.Date = DateTime.Today;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(TitleEntry.Text))
            {
                await DisplayAlert("Error", "Title is required", "OK");
                return;
            }

            var workItem = new WorkItem
            {
                Title = TitleEntry.Text,
                Description = DescriptionEditor.Text,
                Status = StatusPicker.SelectedItem?.ToString() ?? "Not Started",
                Type = (WorkItemType)TypePicker.SelectedItem,
                StartDate = StartDatePicker.Date,
                DueDate = DueDatePicker.Date,
                EndDate = EndDatePicker.Date
            };

            _logger.LogInformation($"Creating new work item with title: {workItem.Title}");
            await _workItemService.CreateWorkItem(workItem);
            _logger.LogInformation("Work item created successfully");

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating work item");
            await DisplayAlert("Error", "Failed to create work item", "OK");
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
} 