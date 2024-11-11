using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgmt.BLL.Services;
using UserMgmt.DAL.UnitOfWork;

namespace UserMgmt.BLL.ServiceAccess
{
    public class BusinessService : IBusinessService
    {
        public UserService UserService { get; set; }
        public AuthService AuthService { get; set; }
        public BusinessService(IUnitOfWork unitOfWork)
        {
            UserService = new UserService(unitOfWork);
            AuthService = new AuthService(unitOfWork);
        }
    }
}
