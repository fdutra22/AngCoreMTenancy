using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.UI;
using Application.Products.Dto;
using MeuCarro;
using MeuCarro.Authorization;
using MeuCarro.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Products
{
    [AbpAuthorize(PermissionNames.Pages_Products)]
    public class ProductsAppService : AsyncCrudAppService<Product, ProductDto, int, PagedAndSortedResultRequestDto, ProductDto>
    {

        private readonly IAbpSession _session;
        private readonly IProductRepository _repository;

        public ProductsAppService(IProductRepository repository, IAbpSession session) : base(repository)
        {
            _repository = repository;
            _session = session;
            LocalizationSourceName = MeuCarroConsts.LocalizationSourceName;
        }




        public override async Task<PagedResultDto<ProductDto>> GetAllAsync(PagedAndSortedResultRequestDto input)
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
                return new PagedResultDto<ProductDto>(totalCount, items);
            }

            return await base.GetAllAsync(input);
        }


        public override Task<ProductDto> CreateAsync(ProductDto input)
        {
            Task<ProductDto> retorno = null;

            var dp = 0d;
            var marca = _repository.GetAllListAsync(x => x.Branch.Equals(input.Branch));
            var ultimoVendido = marca.Result.LastOrDefault(x => x.Name == input.Name);
            input.CreationTime = DateTime.Now;
            input.TenantId = _session.TenantId != null ? _session.TenantId.Value : 1;
            input.CreatorUserId = _session.UserId.Value;

            if (ultimoVendido != null)
            {
                var media = (input.Value + ultimoVendido.Value) / 2;
                dp = Math.Sqrt((Math.Pow(input.Value - media, 2) + Math.Pow(ultimoVendido.Value - media, 2)) / 2);

                var percent = (dp / input.Value) * 100;

                if (percent <= 20)
                {
                    retorno = base.CreateAsync(input);
                }
                else
                {
                    throw new UserFriendlyException("Atenção!", "Valor do veículo não está considerando o desvio padrao de 20% para compra do ultimo deste modelo.");
                }
            }
            else
            {
               
                retorno = base.CreateAsync(input);
            }



            return retorno;
        }

        public override Task DeleteAsync(EntityDto<int> input)
        {
            return base.DeleteAsync(input);
        }

        public override Task<ProductDto> GetAsync(EntityDto<int> input)
        {
            return base.GetAsync(input);
        }

        public override Task<ProductDto> UpdateAsync(ProductDto input)
        {
            return base.UpdateAsync(input);
        }

    }
}
