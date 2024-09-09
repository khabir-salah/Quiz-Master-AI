
using Infrastructure.Persistent.Context;
using Microsoft.EntityFrameworkCore;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connection = builder.Configuration.GetConnectionString("QuizMasterAi");
             // Add services to the container.
             var connectionString = builder.Configuration.GetConnectionString("FoodConnectConnection");
            builder.Services.AddControllers();
            builder.Services.AddDbContext<QuizMasterAiDb>(opt =>
            opt.UseMySql(connection, ServerVersion.AutoDetect(connection)));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
