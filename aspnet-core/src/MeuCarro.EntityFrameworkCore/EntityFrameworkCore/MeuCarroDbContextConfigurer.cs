using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace MeuCarro.EntityFrameworkCore
{
    public static class MeuCarroDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<MeuCarroDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<MeuCarroDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
