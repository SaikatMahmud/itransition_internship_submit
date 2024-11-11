using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserMgmt.DAL.Models;
namespace UserMgmt.DAL.Interfaces
{
    public interface IUserLogin : IRepo<UserLogin>
    {
        bool Create(UserLogin obj);
        User Authenticate(string email, string password);
    }
}
