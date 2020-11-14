using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Services.Interfaces
{
    public interface IAccountService
    {
        Task AddKeyword(string keyword);
        Task<Users> ChangeAddress(Users entity);
        Task<Users> GetUserInfo(int id);
        Task<Users> ValidateUser(Users entity);
    }
}
