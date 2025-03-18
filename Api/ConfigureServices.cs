using Api.Infrastructure.DbContext;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

                // options.AddOpenBehavior(typeof(AuthorizationBehaviour<,>));
                // options.AddOpenBehavior(typeof(PerformanceBehaviour<,>));
                // options.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            });

            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);

          
            services.AddScoped<IApplicationContext, ApplicationContext>();

            return services;
        }

        public static async Task<IServiceCollection> AddInfrastructureAsync(this IServiceCollection services, IConfiguration configuration)
        {
            

            
                services.AddDbContext<ApplicationContext>(options =>
                {
                    options.UseNpgsql(
                        configuration.GetConnectionString("DefaultConnection")
                    );
                });

                Console.WriteLine("Połączenie z bazą danych zostało nawiązane.");
            


            // services.AddScoped<IDomainEventService, DomainEventService>();
            // services.AddTransient<IDateTime, DateTimeService>();
            // services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

            return services;
        }

        public static async Task ApplyMigrationsAsync(this IServiceProvider serviceProvider)
        {
            
                using (var scope = serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

                    if (pendingMigrations != null)
                    {
                        await context.Database.MigrateAsync();
                        Console.WriteLine("Migracje zostały zastosowane.");
                    }
                    else
                    {
                        Console.WriteLine("Brak zaległych migracji.");
                    }
                }
            
        }
    }

}
