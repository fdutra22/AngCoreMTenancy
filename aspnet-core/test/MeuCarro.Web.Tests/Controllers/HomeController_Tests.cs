using System.Threading.Tasks;
using MeuCarro.Models.TokenAuth;
using MeuCarro.Web.Controllers;
using Shouldly;
using Xunit;

namespace MeuCarro.Web.Tests.Controllers
{
    public class HomeController_Tests: MeuCarroWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}