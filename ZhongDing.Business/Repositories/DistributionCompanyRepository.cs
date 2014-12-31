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
    public class DistributionCompanyRepository : BaseRepository<DistributionCompany>, IDistributionCompanyRepository
    {
        public IList<UIDistributionCompany> GetUIList(UISearchDistributionCompany uiSearchObj = null)
        {
            IList<UIDistributionCompany> uiEntities = new List<UIDistributionCompany>();

            IQueryable<DistributionCompany> query = null;

            List<Expression<Func<DistributionCompany, bool>>> whereFuncs = new List<Expression<Func<DistributionCompany, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.SerialNo))
                    whereFuncs.Add(x => x.SerialNo.Contains(uiSearchObj.SerialNo));

                if (!string.IsNullOrEmpty(uiSearchObj.Name))
                    whereFuncs.Add(x => x.Name.Contains(uiSearchObj.Name));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              select new UIDistributionCompany()
                              {
                                  ID = q.ID,
                                  SerialNo = q.SerialNo,
                                  Name = q.Name,
                                  ReceiverName = q.ReceiverName,
                                  PhoneNumber = q.PhoneNumber
                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIDistributionCompany> GetUIList(UISearchDistributionCompany uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIDistributionCompany> uiEntities = new List<UIDistributionCompany>();

            int total = 0;

            IQueryable<DistributionCompany> query = null;

            List<Expression<Func<DistributionCompany, bool>>> whereFuncs = new List<Expression<Func<DistributionCompany, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.SerialNo))
                    whereFuncs.Add(x => x.SerialNo.Contains(uiSearchObj.SerialNo));

                if (!string.IsNullOrEmpty(uiSearchObj.Name))
                    whereFuncs.Add(x => x.Name.Contains(uiSearchObj.Name));
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              select new UIDistributionCompany()
                              {
                                  ID = q.ID,
                                  SerialNo = q.SerialNo,
                                  Name = q.Name,
                                  ReceiverName = q.ReceiverName,
                                  PhoneNumber = q.PhoneNumber
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }

        public int? GetMaxEntityID()
        {
            if (this.DB.DistributionCompany.Count() > 0)
                return this.DB.DistributionCompany.Max(x => x.ID);
            else return null;
        }

        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();

            List<Expression<Func<DistributionCompany, bool>>> whereFuncs = new List<Expression<Func<DistributionCompany, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.IncludeItemValues != null
                    && uiSearchObj.IncludeItemValues.Count > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeItemValues.Contains(x.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.ItemText))
                    whereFuncs.Add(x => x.Name.Contains(uiSearchObj.ItemText));
            }

            uiDropdownItems = GetList(whereFuncs).Select(x => new UIDropdownItem()
            {
                ItemValue = x.ID,
                ItemText = x.Name,
                Extension = new { ReceiverName = x.ReceiverName, PhoneNumber = x.PhoneNumber, Address = x.Address }
            }).ToList();

            return uiDropdownItems;
        }
    }
}
