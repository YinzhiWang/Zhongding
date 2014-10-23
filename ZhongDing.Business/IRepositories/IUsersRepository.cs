﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;

namespace ZhongDing.Business.IRepositories
{
    public interface IUsersRepository : IBaseRepository<Users>
    {
        Users GetByProviderUserKey(Guid providerUserKey);

        string GetUserNameByID(int userID);
    }
}
