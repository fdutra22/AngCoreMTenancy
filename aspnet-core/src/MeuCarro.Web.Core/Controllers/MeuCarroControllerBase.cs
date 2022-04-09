using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace MeuCarro.Controllers
{
    public abstract class MeuCarroControllerBase: AbpController
    {
        protected MeuCarroControllerBase()
        {
            LocalizationSourceName = MeuCarroConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
