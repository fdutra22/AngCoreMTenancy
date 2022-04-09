using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MeuCarro.Authorization;

namespace MeuCarro
{
    [DependsOn(
        typeof(MeuCarroCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class MeuCarroApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<MeuCarroAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(MeuCarroApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
