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
    public class StorageLocationRepository : BaseRepository<StorageLocation>, IStorageLocationRepository
    {
        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();

            List<Expression<Func<StorageLocation, bool>>> whereFuncs = new List<Expression<Func<StorageLocation, bool>>>();

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
                ItemText = x.Name
            }).ToList();

            return uiDropdownItems;
        }

        public IList<UIStorageLocation> GetUIList(UISearchStorageLocation uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIStorageLocation> uiEntities = new List<UIStorageLocation>();
            int total = 0;

            IQueryable<StorageLocation> query = null;

            List<Expression<Func<StorageLocation, bool>>> whereFuncs = new List<Expression<Func<StorageLocation, bool>>>();

            if (uiSearchObj != null)
            {

            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                                select new UIStorageLocation()
                                {
                                    ID = q.ID,
                                    Name = q.Name,
                                    Comment = q.Comment

                                }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
