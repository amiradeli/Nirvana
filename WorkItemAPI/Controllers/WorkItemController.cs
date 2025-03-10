using Microsoft.AspNetCore.Mvc;
using WorkItemAPI.Data;
using WorkItemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace WorkItemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkItemController(WorkItemDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkItem>>> GetWorkItems()
        {
            return await context.WorkItems.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<WorkItem>> PostWorkItem(WorkItem workItem)
        {
            context.WorkItems.Add(workItem);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetWorkItems), new { id = workItem.Id }, workItem);
        }
    }

}
