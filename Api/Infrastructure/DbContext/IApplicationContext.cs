using Api.Domain.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.DbContext
{
    public interface IApplicationContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<ProductParam> ProductParams { get; set; }
        DbSet<OrderItem> OrderItems { get; set; }
        DbSet<Order> Orders { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    }
}
