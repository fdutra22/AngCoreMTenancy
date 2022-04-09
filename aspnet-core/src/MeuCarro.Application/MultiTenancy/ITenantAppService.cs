using Abp.Application.Services;
using MeuCarro.MultiTenancy.Dto;

namespace MeuCarro.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

