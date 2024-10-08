﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserMgmt.BLL.DTOs;
using UserMgmt.DAL.Models;
using UserMgmt.DAL.UnitOfWork;

namespace UserMgmt.BLL.Services
{
    public class AuthService
    {
        private static IUnitOfWork DataAccess;
        public AuthService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }

        public UserDTO Authenticate(string email, string password, out string errorMsg)
        {
            errorMsg = string.Empty;
            var user = DataAccess.UserLogin.Authenticate(email, password);
            if (user == null)
            {
                errorMsg = "Invalid email or password!";
                return null;
            }
            if(user.UserStatus != "Active")
            {
                errorMsg = "You are currently blocked!";
                return null;
            }
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<User, UserDTO>();
            });
            var mapper = new Mapper(cfg);
            return mapper.Map<UserDTO>(user);

        }
        public bool IsActiveUser(int userId)
        {
            var data = DataAccess.User.Get(u => u.Id == userId && u.UserStatus == "Active");
            return data == null ? false : true;

        }
    }
}
