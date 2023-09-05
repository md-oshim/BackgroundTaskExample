using BackgroundTaskExample.Services;

namespace BackgroundTaskExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<ITaskQueue, TaskQueue>();
            builder.Services.AddScoped<BackgroundTaskParallelService>();
            builder.Services.AddHostedService<BackgroundTaskQueueService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline. 

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            //app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}