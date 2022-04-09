using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using MeuCarro.Configuration.Dto;

namespace MeuCarro.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : MeuCarroAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
