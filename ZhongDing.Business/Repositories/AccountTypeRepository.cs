using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.Repositories
{
    public class AccountTypeRepository : BaseRepository<AccountType>, IAccountTypeRepository
    {
        public IList<UIAccountType> GetUIList()
        {
            return GetList().Select(x => new UIAccountType()
            {
                ID = x.ID,
                AccountTypeName = x.AccountTypeName
            }).ToList();
        }
    }
}
