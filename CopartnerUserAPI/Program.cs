using CopartnerUser.DataAccessLayer.MongoDBSettings;
using CopartnerUser.DataAccessLayer.Repository;
using CopartnerUser.DataAccessLayerADO;
using CopartnerUser.ServiceLayer.Services;
using CopartnerUser.ServiceLayer.Services.Interfaces;
using CopartnerUser.ServiceLayer.UserService;
using CopartnerUserAPI.ErrorHandling;
using Microsoft.Extensions.Options;
using Serilog;
using System.Configuration;

namespace CopartnerUserAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            // Serilog
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);


            //MongoDb Service
            builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDbSettings"));

            builder.Services.AddSingleton<IMongoDBSettings>(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<MongoDBSettings>>().Value);

            builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));


            // Add your DAL services
            // Assuming "DefaultConnection" is the name of your connection string in appsettings.json
            // and "System.Data.SqlClient" is the provider name. Adjust these as necessary.
            builder.Services.AddScoped<DBManager>(sp => new DBManager(builder.Configuration, "DefaultConnection", "System.Data.SqlClient"));

            // Add your Service Layer services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAvailabilityTypesService, AvailabilityTypesService>();



            // Add Exception Middleware
            //builder.Services.AddTransient<ExceptionHandlerMiddleware>();

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

            // Add Exception Middleware
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
