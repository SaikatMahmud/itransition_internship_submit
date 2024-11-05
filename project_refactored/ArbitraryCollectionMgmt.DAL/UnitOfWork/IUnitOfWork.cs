using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArbitraryCollectionMgmt.DAL.Interfaces;

namespace ArbitraryCollectionMgmt.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICategory Category { get; }
        ICollection Collection { get; }
        IComment Comment { get; }
        ILike Like { get; }
        ICustomAttribute CustomAttribute { get; }
        IItem Item { get; }
        ICustomValue CustomValue { get; }
        IItemTag ItemTag { get; }
        ITag Tag { get; }
        IUser User { get; }
        IUserLogin UserLogin { get; }
        ISearch Search { get; }
        IApiToken ApiToken { get; }
    }
}
