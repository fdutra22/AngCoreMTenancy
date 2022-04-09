using Abp.Authorization;
using MeuCarro.Authorization.Roles;
using MeuCarro.Authorization.Users;

namespace MeuCarro.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
