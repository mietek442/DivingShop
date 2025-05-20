using Api.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Api.Infrastructure.DbContext
{
    public class ApplicationContext : IdentityDbContext, IApplicationContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductParam> ProductParams { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await Database.BeginTransactionAsync(cancellationToken);
        }
    }
}
