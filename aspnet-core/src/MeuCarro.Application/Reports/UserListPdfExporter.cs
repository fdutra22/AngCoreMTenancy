using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using DinkToPdf;
using DinkToPdf.Contracts;
using MeuCarro.Authorization.Users;
using MeuCarro.Parameters;
using MeuCarro.Products;
using MeuCarro.Sales;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Reports
{
    public class UserListPdfExporter : ITransientDependency
    {
        private readonly IAbpSession _session;
        private readonly IRepository<User, long> _userRepository;
        private readonly ISaleRepository _salesRepository;
        private readonly IProductRepository _prodRepository;
        private readonly IParametersRepository _paramRepository;
        private readonly IConverter _converter;
        public UserListPdfExporter(IRepository<User, long> userRepository, IAbpSession session, IConverter converter, ISaleRepository repository, IProductRepository pRepository, IParametersRepository pPrepository)
        {
            _salesRepository = repository;
            _prodRepository = pRepository;
            _session = session;
            _paramRepository = pPrepository;
            _userRepository = userRepository;
            _converter = converter;
        }
        public async Task<FileDto> GetDataAsPdfAsync()
        {
            var vendas = await _salesRepository.GetAllListAsync(x => x.CreationTime.Day == System.DateTime.Now.Day);
         
            var html = BuildBodyReportVenda(vendas);
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait
                },
                Objects = {
                    new ObjectSettings()
                    {
                        HtmlContent = html
                    }
                }
            };
            return new FileDto("RelatorioVendas.pdf", _converter.Convert(doc));
        }

        public async Task<FileDto> GetDataAsPdfCompraAsync()
        {
            var compras = await _prodRepository.GetAllListAsync(x => x.CreationTime.Day == System.DateTime.Now.Day);

            var html = BuildBodyReportcompra(compras);
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait
                },
                Objects = {
                    new ObjectSettings()
                    {
                        HtmlContent = html
                    }
                }
            };
            return new FileDto("RelatorioCompras.pdf", _converter.Convert(doc));
        }

        public async Task<FileDto> GetDataAsHtmlAsync()
        {
            var vendas = await _salesRepository.GetAllListAsync(x => x.CreationTime.Day == System.DateTime.Now.Day);
          
            var html = BuildBodyReportVenda(vendas);
            
            return new FileDto("RelatorioVendas.html", Encoding.UTF8.GetBytes(html));
        }

        public async Task<FileDto> GetDataAsHtmlCompraAsync()
        {
            var vendas = await _prodRepository.GetAllListAsync(x => x.CreationTime.Day == System.DateTime.Now.Day);

            var html = BuildBodyReportcompra(vendas);

            return new FileDto("RelatorioCompras.html", Encoding.UTF8.GetBytes(html));
        }
        private string BuildBodyReportVenda(List<Sale> vendas)
        {
            var header1 = "<th>Marca</th>";
            var header2 = "<th>Modelo</th>";
            var header3 = "<th>Valor da Venda</th>";
            var header4 = "<th>Data da Venda</th>";
            var header5 = "<th>Margem Vendida (Percentual)</th>";
            var headers = $"<tr>{header1}{header2}{header3}{header4}{header5}</tr>";
            var rows = new StringBuilder();
            foreach (var v in vendas)
            {
                var p = _prodRepository.Get(v.CarroId);
                
                var column1 = $"<td>{p.Branch}</td>";
                var column2 = $"<td>{p.Name}</td>";
                var column3 = $"<td>{v.ValorVendido}</td>";
                var column4 = $"<td>{v.CreationTime}</td>"; 
                var column5 = $"<td>{v.MargemVendidaPercentual}</td>";
                var row = $"<tr>{column1}{column2}{column3}{column4}{column5}</tr>";
                rows.Append(row);
            }
            return $"<h3> Relatorio de Vendas</h3><table>{headers}{rows.ToString()}</table>";
        }

        private string BuildBodyReportcompra(List<Product> compras)
        {
            var header1 = "<th>Marca</th>";
            var header2 = "<th>Modelo</th>";
            var header3 = "<th>Valor da Venda</th>";
            var header4 = "<th>Data da Venda</th>";
            var headers = $"<tr>{header1}{header2}{header3}{header4}</tr>";
            var rows = new StringBuilder();
            foreach (var p in compras)
            {
                var column1 = $"<td>{p.Branch}</td>";
                var column2 = $"<td>{p.Name}</td>";
                var column3 = $"<td>{p.Value}</td>";
                var column4 = $"<td>{p.CreationTime}</td>";
                var row = $"<tr>{column1}{column2}{column3}{column4}</tr>";
                rows.Append(row);
            }
            return $"<h3> Relatorio de Compras</h3><table>{headers}{rows.ToString()}</table>";
        }
    }
    public class FileDto
    {
        public string FileName { get; set; }
        public byte[] FileBytes { get; set; }
        public FileDto(string fileName, byte[] fileBytes)
        {
            FileName = fileName;
            FileBytes = fileBytes;
        }
    }
}