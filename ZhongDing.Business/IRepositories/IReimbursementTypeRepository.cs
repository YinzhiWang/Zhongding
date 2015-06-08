﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface IReimbursementTypeRepository : IBaseRepository<ReimbursementType>
    {
        IList<UIReimbursementType> GetUIList(Domain.UISearchObjects.UISearchReimbursementType uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
    }
}