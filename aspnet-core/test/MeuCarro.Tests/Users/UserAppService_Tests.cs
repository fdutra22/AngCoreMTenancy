using MeuCarro.Users;
using MeuCarro.Users.Dto;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace MeuCarro.Tests.Users
{
    public class UserAppService_Tests : MeuCarroTestBase
    {
        private readonly IUserAppService _userAppService;

        public UserAppService_Tests()
        {
            _userAppService = Resolve<IUserAppService>();
        }

        [Fact]
        public async Task GetUsers_Test()
        {
            // Act
            var output = await _userAppService.GetAllAsync(new PagedUserResultRequestDto{MaxResultCount=20, SkipCount=0} );

            // Assert
            output.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task CreateUser_Test()
        {
            // Act
            await _userAppService.CreateAsync(
                new CreateUserDto
                {
                    EmailAddress = "fabiod@mail.com",
                    IsActive = true,
                    Name = "Fabio",
                    Surname = "Dutra",
                    Password = "123qwe",
                    UserName = "fabio.dutra"
                });

            await UsingDbContextAsync(async context =>
            {
                var johnNashUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "fabio.dutra");
                johnNashUser.ShouldNotBeNull();
            });
        }
    }
}
