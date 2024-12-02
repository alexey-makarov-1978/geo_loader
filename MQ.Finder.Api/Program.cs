
using MQ.Finder.Data.Finder;
using MQ.Finder.Data.Finder.Cache;
using MQ.Finder.Data.Loader;

namespace MQ.Finder.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // not for production but just to ease
            // client-server communication in this scenario
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.AddControllers();

            builder.Services.AddMemoryCache();

            builder.Services.AddSingleton<ICacheService, CacheService>();
            builder.Services.AddSingleton<IDataLoader, DataLoader>();
            builder.Services.AddScoped<IDataFinder, CachedDataFinder>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseCors();

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
