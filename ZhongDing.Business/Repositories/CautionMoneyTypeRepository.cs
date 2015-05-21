using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories
{
    public class CautionMoneyTypeRepository : BaseRepository<CautionMoneyType>, ICautionMoneyTypeRepository
    {
        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();

            List<Expression<Func<CautionMoneyType, bool>>> whereFuncs = new List<Expression<Func<CautionMoneyType, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.IncludeItemValues != null
                    && uiSearchObj.IncludeItemValues.Count > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeItemValues.Contains(x.ID));

                if (uiSearchObj.Extension != null && uiSearchObj.Extension.CautionMoneyTypeCategory > 0)
                {
                    whereFuncs.Add(x => x.Type == uiSearchObj.Extension.CautionMoneyTypeCategory);
                }

            }

            uiDropdownItems = GetList(whereFuncs).Select(x => new UIDropdownItem()
            {
                ItemValue = x.ID,
                ItemText = x.Name,

            }).ToList();

            return uiDropdownItems;
        }
    }
}
