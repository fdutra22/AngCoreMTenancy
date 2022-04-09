using System.Threading.Tasks;
using Abp.Application.Services;
using MeuCarro.Authorization.Accounts.Dto;

namespace MeuCarro.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
