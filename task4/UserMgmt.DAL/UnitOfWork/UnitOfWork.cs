using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgmt.DAL.Interfaces;
using UserMgmt.DAL.Models;
using UserMgmt.DAL.Repos;

namespace UserMgmt.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public IUser User { get; private set; }
        public IUserLogin UserLogin { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            User = new UserRepo(_db);
            UserLogin = new UserLoginRepo(_db);
        }
    }
}
