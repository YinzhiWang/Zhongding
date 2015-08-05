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
using ZhongDing.Common.Extension;

namespace ZhongDing.Business.Repositories
{
    public class HospitalRepository : BaseRepository<Hospital>, IHospitalRepository
    {
        public IList<UIHospital> GetUIList(UISearchHospital uiSearchObj = null)
        {
            IList<UIHospital> uiEntities = new List<UIHospital>();

            IQueryable<Hospital> query = null;

            List<Expression<Func<Hospital, bool>>> whereFuncs = new List<Expression<Func<Hospital, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.HospitalName))
                    whereFuncs.Add(x => x.HospitalName.Contains(uiSearchObj.HospitalName));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              select new UIHospital()
                              {
                                  ID = q.ID,
                                  Code = q.HospitalName
                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIHospital> GetUIList(UISearchHospital uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIHospital> uiEntities = new List<UIHospital>();

            int total = 0;

            IQueryable<Hospital> query = null;

            List<Expression<Func<Hospital, bool>>> whereFuncs = new List<Expression<Func<Hospital, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.HospitalName))
                    whereFuncs.Add(x => x.HospitalName.Contains(uiSearchObj.HospitalName));
                if (uiSearchObj.HospitalCodeID.BiggerThanZero())
                {
                    whereFuncs.Add(x => x.HospitalCodeID == uiSearchObj.HospitalCodeID);
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              select new UIHospital()
                              {
                                  ID = q.ID,
                                  Code = q.HospitalName
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }

        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();

            List<Expression<Func<Hospital, bool>>> whereFuncs = new List<Expression<Func<Hospital, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.IncludeItemValues != null
                    && uiSearchObj.IncludeItemValues.Count > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeItemValues.Contains(x.ID));

                if (uiSearchObj.ExcludeItemValues != null
                    && uiSearchObj.ExcludeItemValues.Count > 0)
                    whereFuncs.Add(x => !uiSearchObj.ExcludeItemValues.Contains(x.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.ItemText))
                    whereFuncs.Add(x => x.HospitalName.Contains(uiSearchObj.ItemText));
            }

            uiDropdownItems = GetList(whereFuncs).Select(x => new UIDropdownItem()
            {
                ItemValue = x.ID,
                ItemText = x.HospitalName
            }).ToList();

            return uiDropdownItems;
        }
    }
}
