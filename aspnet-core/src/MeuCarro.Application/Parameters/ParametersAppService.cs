using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.UI;
using Application.Parameters.Dto;
using Application.Sales.Dto;
using MeuCarro;
using MeuCarro.Authorization;
using MeuCarro.Parameters;
using MeuCarro.Products;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Parameters
{
    [AbpAuthorize(PermissionNames.Pages_Products)]
    public class ParametersAppService : AsyncCrudAppService<Parameter, ParameterDto, int, PagedAndSortedResultRequestDto, ParameterDto>
    {

        private readonly IAbpSession _session;
        private readonly IParametersRepository _repository;

        public ParametersAppService(IParametersRepository repository, IAbpSession session) : base(repository)
        {
            _repository = repository;
            _session = session;
            LocalizationSourceName = MeuCarroConsts.LocalizationSourceName;
        }

        public override async Task<PagedResultDto<ParameterDto>> GetAllAsync(PagedAndSortedResultRequestDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant))
            {
                CheckGetAllPermission();
                var query = _repository.GetAll();
                var totalCount = await query.CountAsync();
                query = ApplySorting(query, input);
                query = ApplyPaging(query, input);
                var entities = await AsyncQueryableExecuter.ToListAsync(query);
                var items = entities.Select(MapToEntityDto).ToList();
                return new PagedResultDto<ParameterDto>(totalCount, items);
            }

            return await base.GetAllAsync(input);
        }

        public override Task<ParameterDto> CreateAsync(ParameterDto input)
        {
            input.CreationTime = System.DateTime.Now;
            input.CreatorUserId = _session.UserId.Value;
            input.TenantId = _session.TenantId != null ? _session.TenantId.Value : 1;

            var query = _repository.GetAll();
            var totalCount = query.Count();
            if (totalCount > 0)
            {
                throw new UserFriendlyException("Atenção!", "Somente é permitido um parametro por loja!");
            }
            else
            {

                var retorno = base.CreateAsync(input);

                return retorno;
            }
        }

        public override Task DeleteAsync(EntityDto<int> input)
        {
            return base.DeleteAsync(input);
        }

        public override Task<ParameterDto> GetAsync(EntityDto<int> input)
        {
            return base.GetAsync(input);
        }

        public override Task<ParameterDto> UpdateAsync(ParameterDto input)
        {
            return base.UpdateAsync(input);
        }
    }
}
