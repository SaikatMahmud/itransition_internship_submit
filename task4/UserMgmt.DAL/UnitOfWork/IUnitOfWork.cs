using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgmt.DAL.Interfaces;

namespace UserMgmt.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUser User { get; }
        IUserLogin UserLogin { get; }
    }
}
