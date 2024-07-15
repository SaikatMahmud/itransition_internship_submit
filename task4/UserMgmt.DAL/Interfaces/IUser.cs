using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgmt.DAL.Models;

namespace UserMgmt.DAL.Interfaces
{
    public interface IUser : IRepo<User>
    {
        User Create(User obj, out string errorMsg);
        bool DeleteRange(IEnumerable<User> obj);
    }
}
