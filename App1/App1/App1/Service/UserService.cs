using App1.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App1.Service
{
    public interface IUserService
    {
        Task<List<UserModel>> GetUsers();
    }
}
