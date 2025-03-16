using Microsoft.Extensions.Logging;
using WorkItemApp.Models;
using WorkItemApp.Services;

namespace WorkItemApp;

[QueryProperty(nameof(WorkItem), "WorkItem")]
public partial class EditWorkItemPage : ContentPage
{
    private readonly IWorkItemService _workItemService;
    private readonly ILogger<EditWorkItemPage> _logger;
    private WorkItem? _workItem;

    public WorkItem WorkItem
    {
        get => _workItem!;
        set
        {
            _workItem = value;
            LoadWorkItem();
        }
    }

    public EditWorkItemPage(IWorkItemService workItemService, ILogger<EditWorkItemPage> logger)
    {
        InitializeComponent();
        _workItemService = workItemService;
        _logger = logger;

        // Initialize the status picker
        StatusPicker.ItemsSource = new List<string> { "Not Started", "In Progress", "Completed" };
        
        // Initialize the type picker with enum values
        TypePicker.ItemsSource = Enum.GetValues(typeof(WorkItemType)).Cast<WorkItemType>().ToList();
    }

    private void LoadWorkItem()
    {
        if (_workItem == null) return;

        TitleEntry.Text = _workItem.Title;
        DescriptionEditor.Text = _workItem.Description;
        StatusPicker.SelectedItem = _workItem.Status;
        TypePicker.SelectedItem = _workItem.Type;
        StartDatePicker.Date = _workItem.StartDate;
        DueDatePicker.Date = _workItem.DueDate;
        EndDatePicker.Date = _workItem.EndDate ?? DateTime.Today;
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

            var updatedWorkItem = new WorkItem
            {
                Id = _workItem.Id,
                Title = TitleEntry.Text,
                Description = DescriptionEditor.Text,
                Status = StatusPicker.SelectedItem?.ToString() ?? "Not Started",
                Type = (WorkItemType)TypePicker.SelectedItem,
                StartDate = StartDatePicker.Date,
                DueDate = DueDatePicker.Date,
                EndDate = EndDatePicker.Date
            };

            _logger.LogInformation($"Updating work item {updatedWorkItem.Id}");
            await _workItemService.UpdateWorkItem(updatedWorkItem.Id, updatedWorkItem);
            _logger.LogInformation("Work item updated successfully");

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating work item");
            await DisplayAlert("Error", "Failed to update work item", "OK");
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
} 