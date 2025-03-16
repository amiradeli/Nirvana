using Microsoft.EntityFrameworkCore;
using WorkItemAPI.Data;
using WorkItemAPI.Models;

namespace WorkItemAPI.Services
{
    public class WorkItemService : IWorkItemService
    {
        private readonly WorkItemDbContext _context;

        public WorkItemService(WorkItemDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WorkItem>> GetWorkItemsAsync()
        {
            return await _context.WorkItems.ToListAsync();
        }

        public async Task<WorkItem> CreateWorkItemAsync(WorkItem workItem)
        {
            _context.WorkItems.Add(workItem);
            await _context.SaveChangesAsync();
            return workItem;
        }

        public async Task<bool> DeleteWorkItemAsync(int id)
        {
            var workItem = await _context.WorkItems.FindAsync(id);
            
            if (workItem == null)
            {
                return false;
            }

            _context.WorkItems.Remove(workItem);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 