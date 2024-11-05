using ArbitraryCollectionMgmt.DAL.Models;
using ArbitraryCollectionMgmt.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.Services
{
    public class LikeService
    {
        private readonly IUnitOfWork DataAccess;
        public LikeService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }
        public bool CheckUserLike(int userId, int itemId)
        {
            var data = DataAccess.Like.Get(l => l.UserId == userId && l.ItemId == itemId);
            return data != null;
        }

        public bool AddItemLike(int userId, int itemId)
        {
            var like = new Like()
            {
                UserId = userId,
                ItemId = itemId,
            };
            var data = DataAccess.Like.Create(like);
            return data != null;
        }
        public bool RemoveItemLike(int userId, int itemId)
        {
            var data = DataAccess.Like.Get(l => l.UserId == userId && l.ItemId == itemId);
            if (data != null) return DataAccess.Like.Delete(data);
            return false;
        }
    }
}
