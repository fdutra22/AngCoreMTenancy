using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Runtime.Session;
using Application.Sales.Dto;
using MeuCarro.Authorization;
using MeuCarro.Parameters;
using MeuCarro.Products;
using MeuCarro.Sales;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Application.Reports
{
    [AbpAuthorize(PermissionNames.Pages_Products)]
    public class ReportsAppService : AsyncCrudAppService<Sale, SaleDto, int, PagedAndSortedResultRequestDto, SaleDto>
    {

        private readonly IAbpSession _session;
        private readonly ISaleRepository _repository;
        private readonly IProductRepository _pRepository; 
        private readonly IParametersRepository _pPrepository;
        private readonly UserListPdfExporter _userListPdfExporter;

        public ReportsAppService(ISaleRepository repository, IAbpSession session, IProductRepository pRepository, IParametersRepository pPrepository, UserListPdfExporter userListPdfExporter) : base(repository)
        {
            _repository = repository;
            _pRepository = pRepository;
            _session = session;
            _pPrepository = pPrepository;
            _userListPdfExporter = userListPdfExporter;
        }


        public async Task<ActionResult> DownloadAsPdfAsync(bool tipo = false)
        {
            var file = await _userListPdfExporter.GetDataAsPdfAsync();
            if (!tipo)
            {
                file = await _userListPdfExporter.GetDataAsPdfCompraAsync();
            }
            
            return new FileContentResult(file.FileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = file.FileName
            };
        }
        public async Task<ActionResult> DownloadAsHtmlAsync(bool tipo = false)
        {
            var file = await _userListPdfExporter.GetDataAsHtmlAsync();
            if (!tipo)
            {
                file = await _userListPdfExporter.GetDataAsHtmlCompraAsync();
            }
            return new FileContentResult(file.FileBytes, "text/html")
            {
                FileDownloadName = file.FileName
            };
        }

    }
}
