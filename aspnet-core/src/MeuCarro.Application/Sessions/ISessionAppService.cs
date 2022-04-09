using System.Threading.Tasks;
using Abp.Application.Services;
using MeuCarro.Sessions.Dto;

namespace MeuCarro.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
