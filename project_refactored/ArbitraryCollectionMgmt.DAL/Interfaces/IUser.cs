using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArbitraryCollectionMgmt.DAL.Models;

namespace ArbitraryCollectionMgmt.DAL.Interfaces
{
    public interface IUser : IRepo<User>
    {
        User Create(User obj, out string errorMsg);
        bool DeleteRange(IEnumerable<User> obj);
    }
}
