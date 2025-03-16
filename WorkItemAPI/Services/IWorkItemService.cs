using WorkItemAPI.Models;

namespace WorkItemAPI.Services
{
    public interface IWorkItemService
    {
        Task<IEnumerable<WorkItem>> GetWorkItemsAsync();
        Task<WorkItem> CreateWorkItemAsync(WorkItem workItem);
        Task<bool> DeleteWorkItemAsync(int id);
    }
} 