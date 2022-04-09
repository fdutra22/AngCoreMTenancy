using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.UI;
using Application.Sales.Dto;
using MeuCarro;
using MeuCarro.Authorization;
using MeuCarro.Parameters;
using MeuCarro.Products;
using MeuCarro.Sales;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Sales
{
    [AbpAuthorize(PermissionNames.Pages_Products)]
    public class SaleAppService : AsyncCrudAppService<Sale, SaleDto, int, PagedAndSortedResultRequestDto, SaleDto>
    {

        private readonly IAbpSession _session;
        private readonly ISaleRepository _repository;
        private readonly IProductRepository _pRepository; 
        private readonly IParametersRepository _pPrepository;

        public SaleAppService(ISaleRepository repository, IAbpSession session, IProductRepository pRepository, IParametersRepository pPrepository) : base(repository)
        {
            _repository = repository;
            _pRepository = pRepository;
            _session = session;
            _pPrepository = pPrepository;
            LocalizationSourceName = MeuCarroConsts.LocalizationSourceName;
        }

        public override async Task<PagedResultDto<SaleDto>> GetAllAsync(PagedAndSortedResultRequestDto input)
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
                return new PagedResultDto<SaleDto>(totalCount, items);
            }

            return await base.GetAllAsync(input);
        }

        public override Task<SaleDto> CreateAsync(SaleDto input)
        {
            Task<SaleDto> retorno = null;

            var TenantId = _session.TenantId != null ? _session.TenantId.Value : 1;
            var parametro = _pPrepository.FirstOrDefaultAsync(x => x.TenantId == TenantId);

            if(parametro.Result == null)
            {
                throw new UserFriendlyException("Atenção!", "Inserir valores de parametros antes de prosseguir com a venda.!");

            }
            var margemIdeal = parametro.Result.MargemIdealPercentual;
            var margemMinima = parametro.Result.VendaMinimaPercentual;
            var carro = _pRepository.FirstOrDefaultAsync(x => x.Id == input.CarroId);

            var diferencaValor = input.ValorVendido - carro.Result.Value;

            var percent = (diferencaValor / input.ValorVendido) * 100;

            input.TenantId = TenantId;

            input.CreatorUserId = _session.UserId.Value;
            input.CreationTime = System.DateTime.Now;
            input.MargemVendidaPercentual = percent;

            if (percent >= margemIdeal)
            {               
                retorno = base.CreateAsync(input);
            }
            else if (percent >= margemMinima)
            {               
                retorno = base.CreateAsync(input);
            }
            else
            {
                throw new UserFriendlyException("Atenção!", "O mímino tolerado para a venda deste modelo é de: " + margemMinima + "% sobre o valor de compra.");
            }

            return retorno;
        }

        public override Task DeleteAsync(EntityDto<int> input)
        {
            return base.DeleteAsync(input);
        }

        public override Task<SaleDto> GetAsync(EntityDto<int> input)
        {
            return base.GetAsync(input);
        }

        public override Task<SaleDto> UpdateAsync(SaleDto input)
        {
            return base.UpdateAsync(input);
        }
    }
}
