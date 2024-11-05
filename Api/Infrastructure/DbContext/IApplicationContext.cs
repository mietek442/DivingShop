using Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
namespace Api.Infrastructure.DbContext
{
    public interface IApplicationContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<ProductParam> ProductParams { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}