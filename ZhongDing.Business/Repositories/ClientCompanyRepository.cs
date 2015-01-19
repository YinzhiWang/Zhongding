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
    public class ClientCompanyRepository : BaseRepository<ClientCompany>, IClientCompanyRepository
    {
        public IList<UIClientCompany> GetUIList(UISearchClientCompany uiSearchObj = null)
        {
            IList<UIClientCompany> uiEntities = new List<UIClientCompany>();

            IQueryable<ClientCompany> query = null;

            List<Expression<Func<ClientCompany, bool>>> whereFuncs = new List<Expression<Func<ClientCompany, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.Name))
                    whereFuncs.Add(x => x.Name.Contains(uiSearchObj.Name));

                if (!string.IsNullOrEmpty(uiSearchObj.District))
                    whereFuncs.Add(x => x.District.Contains(uiSearchObj.District));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              select new UIClientCompany()
                              {
                                  ID = q.ID,
                                  Name = q.Name,
                                  District = q.District
                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIClientCompany> GetUIList(UISearchClientCompany uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIClientCompany> uiEntities = new List<UIClientCompany>();

            int total = 0;

            IQueryable<ClientCompany> query = null;

            List<Expression<Func<ClientCompany, bool>>> whereFuncs = new List<Expression<Func<ClientCompany, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.Name))
                    whereFuncs.Add(x => x.Name.Contains(uiSearchObj.Name));

                if (!string.IsNullOrEmpty(uiSearchObj.District))
                    whereFuncs.Add(x => x.District.Contains(uiSearchObj.District));
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              select new UIClientCompany()
                              {
                                  ID = q.ID,
                                  Name = q.Name,
                                  District = q.District
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }

        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();

            List<Expression<Func<ClientCompany, bool>>> whereFuncs = new List<Expression<Func<ClientCompany, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.IncludeItemValues != null
                    && uiSearchObj.IncludeItemValues.Count > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeItemValues.Contains(x.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.ItemText))
                    whereFuncs.Add(x => x.Name.Contains(uiSearchObj.ItemText));

                if (uiSearchObj.Extension != null && uiSearchObj.Extension.ClientUserID > 0)
                    whereFuncs.Add(x => x.ClientInfo.Any(y => y.ClientUserID == uiSearchObj.Extension.ClientUserID));
            }

            uiDropdownItems = GetList(whereFuncs).Select(x => new UIDropdownItem()
            {
                ItemValue = x.ID,
                ItemText = x.Name
            }).ToList();

            return uiDropdownItems;
        }
    }
}
