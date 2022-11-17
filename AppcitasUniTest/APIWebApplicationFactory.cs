using Apcitas;
using Apcitas.WebService.Data;
using Apcitas.WebService.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using Xunit.Sdk;

namespace AppcitasUniTest;

public  class APIWebApplicationFactory<IStarup> : WebApplicationFactory<Startup>
{
    public IConfiguration? Configuration { get; set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((ctx, cbld) =>
        {
            cbld.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

            Configuration = cbld.Build();
        })
        .ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<DataContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging();
            });

        })
        .ConfigureTestServices(async services =>
        {
            var sp = services.BuildServiceProvider();

            using (var scope  = sp.CreateScope())
            {
                var context = sp.GetRequiredService<DataContext>();

                try
                {
                    await context.Database.MigrateAsync();
                    await Seed.SeedUsers(context);
                }
                catch(Exception e)
                {
                    var logger = sp.GetRequiredService<ILogger<Program>>();
                    logger.LogError(e, "An error ocurred during migration/seeding");
                }
            }
        });
    }

    private void _loadTestData(DataContext appDbContext)
    {
        appDbContext.Database.EnsureCreated();

        if (!LoadTestData<AppUser>.Run(appDbContext, "UserSeedData.json")) throw new Exception("Unable to seed user data");
    }
}

internal static class LoadTestData<T> where T : class
{
    internal static readonly string FLD_SEED_DATA = "SeedData";

    internal static bool Run(DbContext context, string fileName)
    {
        if (context.Set<T>().Any()) return true;

        var seedType = new List<T>();

        using (StreamReader r = new(Path.Combine(Environment.CurrentDirectory, FLD_SEED_DATA, fileName)))
        {
            string json = r.ReadToEnd();
            seedType = JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions { IgnoreNullValues = true });
        }

        if (seedType == null) return false;

        context.AddRange(seedType);
        context.SaveChanges();

        return true;
    }
}