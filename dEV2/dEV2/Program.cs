using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConfigurationPractice
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IConfigurationService, ConfigurationService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    var configurationService = context.RequestServices.GetService<IConfigurationService>();

                    var companyWithMostEmployees = configurationService.GetCompanyWithMostEmployees();
                    await context.Response.WriteAsync($"Company with most employees: {companyWithMostEmployees}");
                });
            });
        }
    }

    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration _configuration;

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetCompanyWithMostEmployees()
        {
            int maxEmployees = 0;
            string companyWithMostEmployees = "";

            var companies = _configuration.GetSection("Companies").GetChildren();
            foreach (var company in companies)
            {
                int employees = company.GetValue<int>("Employees");
                if (employees > maxEmployees)
                {
                    maxEmployees = employees;
                    companyWithMostEmployees = company.GetValue<string>("Name");
                }
            }

            return companyWithMostEmployees;
        }
    }

    public interface IConfigurationService
    {
        string GetCompanyWithMostEmployees();
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

