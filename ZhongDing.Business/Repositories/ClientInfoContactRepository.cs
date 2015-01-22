using System;
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
    public class ClientInfoContactRepository : BaseRepository<ClientInfoContact>, IClientInfoContactRepository
    {
        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();

            List<Expression<Func<ClientInfoContact, bool>>> whereFuncs = new List<Expression<Func<ClientInfoContact, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.IncludeItemValues != null
                    && uiSearchObj.IncludeItemValues.Count > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeItemValues.Contains(x.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.ItemText))
                    whereFuncs.Add(x => x.ContactName.Contains(uiSearchObj.ItemText));

                if (uiSearchObj.Extension != null)
                {
                    if (uiSearchObj.Extension.ClientUserID > 0)
                        whereFuncs.Add(x => x.ClientInfo.ClientUserID == uiSearchObj.Extension.ClientUserID);

                    if (uiSearchObj.Extension.ClientCompanyID > 0)
                        whereFuncs.Add(x => x.ClientInfo.ClientCompanyID == uiSearchObj.Extension.ClientCompanyID);
                }
            }

            uiDropdownItems = GetList(whereFuncs).Select(x => new UIDropdownItem()
            {
                ItemValue = x.ID,
                ItemText = x.ContactName,
                Extension = new { PhoneNumber = x.PhoneNumber }
            }).ToList();

            return uiDropdownItems;
        }
    }
}
