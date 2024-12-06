using AssigmentBackend.Business.Abstractions;
using AssigmentBackend.Business.Configs;
using AssigmentBackend.Business.Services;
using AssigmentBackend.Database;

namespace AssignmentBackend.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;

            // Configs
            //
            builder.Services.Configure<RoomConfig>(configuration.GetSection("Room"));

            // Database
            //
            builder.Services.AddScoped<DbContext>();
            
            // Business Services
            //
            builder.Services.AddScoped<IRoomGetService, RoomGetService>();
            builder.Services.AddScoped<IRoomAvailabilityService, RoomAvailabilityService>();
            
            builder.Services.AddControllers();

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
