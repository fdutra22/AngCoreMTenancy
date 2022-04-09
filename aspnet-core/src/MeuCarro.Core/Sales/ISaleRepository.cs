using Abp.Domain.Repositories;
using System.Linq;

namespace MeuCarro.Sales
{
    public interface ISaleRepository : IRepository<Sale, int>
    {
        int GetIdealQuantity(int productId);
        IQueryable<Sale> GetAll();
    }
}
