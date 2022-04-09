using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using MeuCarro.Authorization.Roles;
using MeuCarro.Authorization.Users;
using MeuCarro.MultiTenancy;
using MeuCarro.Products;
using MeuCarro.Sales;
using MeuCarro.Parameters;

namespace MeuCarro.EntityFrameworkCore
{
    public class MeuCarroDbContext : AbpZeroDbContext<Tenant, Role, User, MeuCarroDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public MeuCarroDbContext(DbContextOptions<MeuCarroDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Parameter> Parameters { get; set; }
    }
}
