using Microsoft.EntityFrameworkCore;
using WorkItemAPI.Models;

namespace WorkItemAPI.Data
{
    public class WorkItemDbContext(DbContextOptions<WorkItemDbContext> options)
        : DbContext(options)
    {
        public DbSet<WorkItem> WorkItems { get; set; }
    }
}
