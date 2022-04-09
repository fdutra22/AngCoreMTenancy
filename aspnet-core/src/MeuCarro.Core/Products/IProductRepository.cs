using Abp.Domain.Repositories;
using System.Linq;

namespace MeuCarro.Products
{
    public interface IProductRepository : IRepository<Product, int>
    {
        int GetIdealQuantity(int productId);
        IQueryable<Product> GetAll();
    }
}
