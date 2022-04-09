using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MeuCarro.EntityFrameworkCore;
using MeuCarro.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace MeuCarro.Web.Tests
{
    [DependsOn(
        typeof(MeuCarroWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class MeuCarroWebTestModule : AbpModule
    {
        public MeuCarroWebTestModule(MeuCarroEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MeuCarroWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(MeuCarroWebMvcModule).Assembly);
        }
    }
}