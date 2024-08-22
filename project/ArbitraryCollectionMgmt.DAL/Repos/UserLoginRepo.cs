using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArbitraryCollectionMgmt.DAL.Interfaces;
using ArbitraryCollectionMgmt.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace ArbitraryCollectionMgmt.DAL.Repos
{
    internal class UserLoginRepo : Repository<UserLogin>, IUserLogin
    {
        private readonly ApplicationDbContext _db;
        private readonly PasswordHasher<string> pw;

        public UserLoginRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
            pw = new PasswordHasher<string>();
        }

        public bool Create(UserLogin obj)
        {
            var hashedPassword = pw.HashPassword("", obj.Password);
            obj.Password = hashedPassword;
            _db.UserLogins.Add(obj);
            if (_db.SaveChanges() > 0) return true;
            return false;
        }

        public User Authenticate(string email, string password)
        {
            var user = _db.Users
               .Include(u => u.UserLogin)
               .Where(u => u.Email == email && u.UserId == u.UserLogin.UserId)
               .FirstOrDefault();
            if (user != null)
            {
                var verificationResult = pw.VerifyHashedPassword("", user.UserLogin.Password, password);
                if (verificationResult == PasswordVerificationResult.Success)
                {
                    if (user.UserStatus == "Active")
                    {
                        user.UserLogin.LastLogin = DateTime.Now;
                        _db.SaveChanges();
                    }
                    return user;
                }
            }
            return null;
        }
    }
}
