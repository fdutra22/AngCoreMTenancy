using System.Threading.Tasks;
using MeuCarro.Configuration.Dto;

namespace MeuCarro.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
