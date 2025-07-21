using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Audit.Core.Interfaces;
using Audit.Infrastructure.Repositories;

namespace Audit.Infrastructure.ServiceExtension
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddDIServices(this IServiceCollection services, IConfiguration configuration)
        {

             string? connectionString = configuration.GetConnectionString("MyAppDB");

            //string connectionString = configuration["ConnectionStrings.MyAppDB"];

            services.AddDbContext<DbContextClass>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProducerRepository, ProducerRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IPermissionElasticsearchRepository, PermissionElasticsearchRepository>();

            return services;
        }
    }
}
