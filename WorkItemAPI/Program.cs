using Microsoft.EntityFrameworkCore;
using WorkItemAPI.Data;

namespace WorkItemAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<WorkItemDbContext>(options =>
                options.UseSqlite("Data Source=workitems.db"));

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();

        }
    }
}
