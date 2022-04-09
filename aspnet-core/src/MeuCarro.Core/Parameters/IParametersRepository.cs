using Abp.Domain.Repositories;
using System.Linq;

namespace MeuCarro.Parameters
{
    public interface IParametersRepository : IRepository<Parameter, int>
    {
        int GetIdealQuantity(int productId);
        IQueryable<Parameter> GetAll();
    }
}
