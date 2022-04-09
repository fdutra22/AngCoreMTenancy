using Abp.Data;
using Abp.EntityFrameworkCore;
using MeuCarro.Parameters;
using MeuCarro.Sales;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Data.Common;

namespace MeuCarro.EntityFrameworkCore.Repositories
{
    public class ParametersRepository : MeuCarroRepositoryBase<Parameter, int>, IParametersRepository
    {
        private readonly IActiveTransactionProvider _transactionProvider;

        public ParametersRepository(IDbContextProvider<MeuCarroDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider) : base(dbContextProvider)
        {
            _transactionProvider = transactionProvider;
        }

        public int GetIdealQuantity(int productId)
        {
            // 1. If it returns a set of Product you can do Context.Set<Product>().FromSql("SPROC @Param1", param1)
            // 2. To execute sprocs that don't return anything: Context.Database.ExecuteSqlCommandAsync("SPROC @Param1", param1)
            // 3. The ADO.Net Approach, for custom column or anything more complicated
            var dbCommand = this.GetConnection().CreateCommand();
            dbCommand.Transaction = GetActiveTransaction();
            dbCommand.CommandText = "GetIdealProductQuantity";
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@ProductId", SqlDbType.Int) { Value = productId });
            using (var dbDataReader = dbCommand.ExecuteReader())
            {
                while (dbDataReader.Read())
                {
                    var idealQuantity = dbDataReader.GetInt32(0);
                    return idealQuantity;
                }
            }
            throw new ArgumentException("No ideal quantity exists for product: " + productId);
        }

        private DbTransaction GetActiveTransaction()
        {
            return (DbTransaction)_transactionProvider.GetActiveTransaction(new ActiveTransactionProviderArgs
            {
                {"ContextType", typeof(MeuCarroDbContext) },
                {"MultiTenancySide", MultiTenancySide }
            });
        }
    }
}
