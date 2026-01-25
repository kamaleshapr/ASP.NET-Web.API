using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Business.Utils;

namespace TaskManagement.Business.Services.Interface
{
    public interface IAuthService
    {
        Task<IEnumerable<IdentityError>> Register(RegisterInput registerInput);

        Task<object> Login(LoginInput loginInput);
    }
}
