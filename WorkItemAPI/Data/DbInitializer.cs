using Microsoft.EntityFrameworkCore;
using WorkItemAPI.Models;

namespace WorkItemAPI.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<WorkItemDbContext>();
            await context.Database.EnsureCreatedAsync();

            // Check if there are any work items
            if (await context.WorkItems.AnyAsync())
            {
                return; // DB has been seeded
            }

            var workItems = new List<WorkItem>
            {
                new WorkItem
                {
                    Title = "Implement User Authentication",
                    Description = "Add user authentication using JWT tokens",
                    Status = "Not Started",
                    Type = WorkItemType.Feature,
                    StartDate = DateTime.Today,
                    DueDate = DateTime.Today.AddDays(7)
                },
                new WorkItem
                {
                    Title = "Fix Navigation Bug",
                    Description = "Fix the navigation issue in the mobile app",
                    Status = "In Progress",
                    Type = WorkItemType.Bug,
                    StartDate = DateTime.Today.AddDays(-2),
                    DueDate = DateTime.Today.AddDays(1)
                },
                new WorkItem
                {
                    Title = "Design Database Schema",
                    Description = "Create the initial database schema for the project",
                    Status = "Completed",
                    Type = WorkItemType.Task,
                    StartDate = DateTime.Today.AddDays(-5),
                    DueDate = DateTime.Today.AddDays(-1),
                    EndDate = DateTime.Today.AddDays(-1)
                },
                new WorkItem
                {
                    Title = "Research Cloud Providers",
                    Description = "Compare different cloud providers for hosting",
                    Status = "In Progress",
                    Type = WorkItemType.Spike,
                    StartDate = DateTime.Today.AddDays(-1),
                    DueDate = DateTime.Today.AddDays(3)
                },
                new WorkItem
                {
                    Title = "Implement Dashboard",
                    Description = "Create a dashboard for project metrics",
                    Status = "Not Started",
                    Type = WorkItemType.UserStory,
                    StartDate = DateTime.Today.AddDays(1),
                    DueDate = DateTime.Today.AddDays(10)
                },
                new WorkItem
                {
                    Title = "Optimize Database Queries",
                    Description = "Improve performance of slow database queries",
                    Status = "Not Started",
                    Type = WorkItemType.Task,
                    StartDate = DateTime.Today.AddDays(2),
                    DueDate = DateTime.Today.AddDays(5)
                },
                new WorkItem
                {
                    Title = "Add Export Feature",
                    Description = "Allow users to export data to CSV",
                    Status = "Not Started",
                    Type = WorkItemType.Feature,
                    StartDate = DateTime.Today.AddDays(3),
                    DueDate = DateTime.Today.AddDays(8)
                },
                new WorkItem
                {
                    Title = "Fix Memory Leak",
                    Description = "Investigate and fix memory leak in background service",
                    Status = "In Progress",
                    Type = WorkItemType.Bug,
                    StartDate = DateTime.Today.AddDays(-3),
                    DueDate = DateTime.Today.AddDays(1)
                },
                new WorkItem
                {
                    Title = "Update Documentation",
                    Description = "Update API documentation with new endpoints",
                    Status = "Not Started",
                    Type = WorkItemType.Task,
                    StartDate = DateTime.Today,
                    DueDate = DateTime.Today.AddDays(4)
                },
                new WorkItem
                {
                    Title = "Implement Dark Theme",
                    Description = "Add dark theme support to the application",
                    Status = "Not Started",
                    Type = WorkItemType.UserStory,
                    StartDate = DateTime.Today.AddDays(5),
                    DueDate = DateTime.Today.AddDays(15)
                }
            };

            await context.WorkItems.AddRangeAsync(workItems);
            await context.SaveChangesAsync();
        }
    }
} 