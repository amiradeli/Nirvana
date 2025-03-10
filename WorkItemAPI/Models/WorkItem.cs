namespace WorkItemAPI.Models
{
    public class WorkItem
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required string Status { get; set; }
    }
}
