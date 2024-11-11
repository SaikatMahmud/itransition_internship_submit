using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArbitraryCollectionMgmt.DAL.Interfaces;
using ArbitraryCollectionMgmt.DAL.Models;

namespace ArbitraryCollectionMgmt.DAL.Repos
{
    internal class UserRepo : Repository<User>, IUser
    {
        private readonly ApplicationDbContext _db;
        public UserRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public User Create(User obj, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                dbSet.Add(obj);
                if (_db.SaveChanges() > 0) return obj;
            }
            catch(DbUpdateException ex)
            {
                errorMsg = "Email already exists!";
                return null;
            }
            catch(Exception ex)
            {
                errorMsg = "Internal server error";
                return null;
            }
            return null;

        }

        public bool DeleteRange(IEnumerable<User> obj)
        {
            _db.RemoveRange(obj);
            return _db.SaveChanges() > 0;
        }
    }
}
