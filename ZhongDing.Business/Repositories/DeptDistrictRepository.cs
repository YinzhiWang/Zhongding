﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories
{
    public class DeptDistrictRepository : BaseRepository<DeptDistrict>, IDeptDistrictRepository
    {
        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();

            List<Expression<Func<DeptDistrict, bool>>> whereFuncs = new List<Expression<Func<DeptDistrict, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ItemValues != null
                    && uiSearchObj.ItemValues.Count > 0)
                    whereFuncs.Add(x => uiSearchObj.ItemValues.Contains(x.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.ItemText))
                    whereFuncs.Add(x => x.DistrictName.Contains(uiSearchObj.ItemText));

                if (uiSearchObj.ExtensionEntityID > 0)
                    whereFuncs.Add(x => x.DepartmentTypeID == uiSearchObj.ExtensionEntityID);
            }

            uiDropdownItems = GetList(whereFuncs).Select(x => new UIDropdownItem()
            {
                ItemValue = x.ID,
                ItemText = x.DistrictName
            }).ToList();

            return uiDropdownItems;
        }
    }
}
