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
    public class ClientUserRepository : BaseRepository<ClientUser>, IClientUserRepository
    {
        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();

            List<Expression<Func<ClientUser, bool>>> whereFuncs = new List<Expression<Func<ClientUser, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.IncludeItemValues != null
                    && uiSearchObj.IncludeItemValues.Count > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeItemValues.Contains(x.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.ItemText))
                    whereFuncs.Add(x => x.ClientName.Contains(uiSearchObj.ItemText));

                if (uiSearchObj.Extension != null)
                {
                    if (uiSearchObj.Extension.OnlyIncludeValidClientUser == true)
                        //如果一个client user 没有一个有效的client info，则该client user 是无效的
                        whereFuncs.Add(x => x.ClientInfo.Any(y => y.IsDeleted == false));
                }
            }

            uiDropdownItems = GetList(whereFuncs).Select(x => new UIDropdownItem()
            {
                ItemValue = x.ID,
                ItemText = x.ClientName
            }).ToList();

            return uiDropdownItems;
        }
        public ClientUser GetClientUserByClientName(string clientName)
        {
            var query = from cu in DB.ClientUser
                        join ci in DB.ClientInfo on cu.ID equals ci.ClientUserID
                        where cu.IsDeleted == false && ci.IsDeleted == false && cu.ClientName.ToLower() == clientName.ToLower()
                        select cu;
            var entity = query.FirstOrDefault();
            return entity;

        }
    }
}
