using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MeuCarro.Configuration;

namespace MeuCarro.Web.Host.Startup
{
    [DependsOn(
       typeof(MeuCarroWebCoreModule))]
    public class MeuCarroWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public MeuCarroWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MeuCarroWebHostModule).GetAssembly());
        }
    }
}
