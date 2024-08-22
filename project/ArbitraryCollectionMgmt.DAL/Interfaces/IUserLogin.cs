using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArbitraryCollectionMgmt.DAL.Models;

namespace ArbitraryCollectionMgmt.DAL.Interfaces
{
    public interface IUserLogin : IRepo<UserLogin>
    {
        bool Create(UserLogin obj);
        User Authenticate(string email, string password);
    }
}
