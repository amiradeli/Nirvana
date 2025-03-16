using Microsoft.AspNetCore.Mvc;
using WorkItemAPI.Models;
using WorkItemAPI.Services;
using Microsoft.Extensions.Logging;

namespace WorkItemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkItemController : ControllerBase
    {
        private readonly IWorkItemService _workItemService;
        private readonly ILogger<WorkItemController> _logger;

        public WorkItemController(IWorkItemService workItemService, ILogger<WorkItemController> logger)
        {
            _workItemService = workItemService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkItem>>> GetWorkItems()
        {
            var items = await _workItemService.GetWorkItemsAsync();
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<WorkItem>> PostWorkItem([FromBody] WorkItem workItem)
        {
            try
            {
                _logger.LogInformation("Received work item: {@WorkItem}", workItem);
                
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state: {@ModelState}", ModelState);
                    return BadRequest(ModelState);
                }

                var createdItem = await _workItemService.CreateWorkItemAsync(workItem);
                _logger.LogInformation("Created work item: {@CreatedItem}", createdItem);
                return CreatedAtAction(nameof(GetWorkItems), new { id = createdItem.Id }, createdItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating work item");
                return StatusCode(500, "An error occurred while creating the work item");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkItem(int id)
        {
            var result = await _workItemService.DeleteWorkItemAsync(id);
            
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
