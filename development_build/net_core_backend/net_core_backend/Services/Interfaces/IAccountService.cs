using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Services.Interfaces
{
    public interface IAccountService : IDataService<Users>
    {
        Task<Users> ChangeAddress(Users entity);
        Task<Users> GetAllInformation(int id);
        Task<Users> ValidateUser(Users entity);
    }
}
