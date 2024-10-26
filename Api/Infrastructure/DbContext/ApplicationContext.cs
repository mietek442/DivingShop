using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Api.Infrastructure.DbContext
{
    public class ApplicationContext : IdentityDbContext, IApplicationContext
    {
 

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}