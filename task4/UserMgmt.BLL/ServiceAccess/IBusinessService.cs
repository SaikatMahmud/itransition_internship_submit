using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgmt.BLL.Services;

namespace UserMgmt.BLL.ServiceAccess
{
    public interface IBusinessService
    {
        public UserService UserService { get; set; }
        public AuthService AuthService { get; set; }
    }
}
