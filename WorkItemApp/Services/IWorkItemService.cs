using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkItemApp.Models;
using System.Text.Json.Serialization;

namespace WorkItemApp.Services
{
    public interface IWorkItemService
    {
        [Get("/api/workitem")]
        Task<List<WorkItem>> GetWorkItems();

        [Post("/api/workitem")]
        Task<WorkItem> CreateWorkItem([Body] WorkItem workItem);

        [Delete("/api/workitem/{id}")]
        Task DeleteWorkItem(int id);
    }
}
