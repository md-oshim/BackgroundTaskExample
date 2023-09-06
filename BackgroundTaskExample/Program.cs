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
            builder.Services.AddSingleton<BackgroundTaskResponseHub>();
            builder.Services.AddSingleton<TaskQueue>();
            builder.Services.AddScoped<BackgroundTaskParallelService>();
            builder.Services.AddHostedService<BackgroundTaskQueueService>();
            builder.Services.AddSignalR();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "DefaultCorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline. 

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors("DefaultCorsPolicy");

            app.UseHttpsRedirection();

            app.MapControllers();
            app.MapHub<BackgroundTaskResponseHub>("/hubs/BackgroundTaskResponseHub");

            app.Run();
        }
    }
}