using Microsoft.EntityFrameworkCore;
namespace Api.Infrastructure.DbContext
{
    public interface IApplicationContext
    {
        Task<int> SaveChangesAsync();
    }
}