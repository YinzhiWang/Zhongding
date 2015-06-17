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
using ZhongDing.Common.Extension;

namespace ZhongDing.Business.Repositories
{
    public class FixedAssetsRepository : BaseRepository<FixedAssets>, IFixedAssetsRepository
    {
        public IList<Domain.UIObjects.UIFixedAssets> GetUIList(Domain.UISearchObjects.UISearchFixedAssets uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UIFixedAssets> uiEntitys = new List<UIFixedAssets>();
            int total = 0;

            IQueryable<FixedAssets> query = null;

            List<Expression<Func<FixedAssets, bool>>> whereFuncs = new List<Expression<Func<FixedAssets, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                {
                    whereFuncs.Add(x => x.ID == uiSearchObj.ID);
                }
                if (uiSearchObj.Name.IsNotNullOrEmpty())
                {
                    whereFuncs.Add(x => x.Name.Contains(uiSearchObj.Name));
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                var queryResult = from q in query
                                  join department in DB.Department on q.DepartmentID equals department.ID
                                  join location in DB.StorageLocation on q.StorageLocationID equals location.ID
                                  join fixedAssetsType in DB.FixedAssetsType on q.FixedAssetsTypeID equals fixedAssetsType.ID
                                  select new UIFixedAssets()
                                  {
                                      ID = q.ID,
                                      Comment = q.Comment,
                                      Code = q.Code,
                                      DepartmentName = department.DepartmentName,
                                      EstimateNetSalvageValue = q.EstimateNetSalvageValue,
                                      EstimateUsedYear = q.EstimateUsedYear,
                                      Manufacturer = q.Manufacturer,
                                      Name = q.Name,
                                      OriginalValue = q.OriginalValue,
                                      ProducingArea = q.ProducingArea,
                                      Quantity = q.Quantity,
                                      Specification = q.Specification,
                                      StartUsedDate = q.StartUsedDate,
                                      StorageLocationName = location.Name,
                                      FixedAssetsTypeName = fixedAssetsType.Name,
                                      Unit = q.Unit,
                                      UsePeople = q.UsePeople,
                                      UseStatusText = q.UseStatus == (int)EUseStatus.Used ? "使用中" : "停用",

                                  };


                total = queryResult.Count();
                uiEntitys = queryResult.ToList();
            }

            totalRecords = total;
            return uiEntitys;
        }
    }
}
