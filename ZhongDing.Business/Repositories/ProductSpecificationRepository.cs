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
    public class ProductSpecificationRepository : BaseRepository<ProductSpecification>, IProductSpecificationRepository
    {
        public IList<UIProductSpecification> GetUIList(UISearchProductSpecification uiSearchObj = null)
        {
            IList<UIProductSpecification> uiEntities = new List<UIProductSpecification>();

            IQueryable<ProductSpecification> query = null;

            List<Expression<Func<ProductSpecification, bool>>> whereFuncs = new List<Expression<Func<ProductSpecification, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.ProductID > 0)
                    whereFuncs.Add(x => x.ProductID == uiSearchObj.ProductID);

                if (uiSearchObj.UnitOfMeasurementID > 0)
                    whereFuncs.Add(x => x.UnitOfMeasurementID == uiSearchObj.UnitOfMeasurementID);

                if (!string.IsNullOrEmpty(uiSearchObj.Specification))
                    whereFuncs.Add(x => x.Specification.Contains(uiSearchObj.Specification));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join um in DB.UnitOfMeasurement on q.UnitOfMeasurementID equals um.ID into tempUM
                              from tum in tempUM.DefaultIfEmpty()
                              select new UIProductSpecification()
                              {
                                  ID = q.ID,
                                  Specification = q.Specification,
                                  UnitOfMeasurement = tum == null ? string.Empty : tum.UnitName,
                                  NumberInSmallPackage = q.NumberInSmallPackage,
                                  NumberInLargePackage = q.NumberInLargePackage,
                                  LicenseNumber = q.LicenseNumber

                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIProductSpecification> GetUIList(UISearchProductSpecification uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIProductSpecification> uiEntities = new List<UIProductSpecification>();

            int total = 0;

            IQueryable<ProductSpecification> query = null;

            List<Expression<Func<ProductSpecification, bool>>> whereFuncs = new List<Expression<Func<ProductSpecification, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.ProductID > 0)
                    whereFuncs.Add(x => x.ProductID == uiSearchObj.ProductID);

                if (uiSearchObj.UnitOfMeasurementID > 0)
                    whereFuncs.Add(x => x.UnitOfMeasurementID == uiSearchObj.UnitOfMeasurementID);

                if (!string.IsNullOrEmpty(uiSearchObj.Specification))
                    whereFuncs.Add(x => x.Specification.Contains(uiSearchObj.Specification));
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join um in DB.UnitOfMeasurement on q.UnitOfMeasurementID equals um.ID into tempUM
                              from tum in tempUM.DefaultIfEmpty()
                              select new UIProductSpecification()
                              {
                                  ID = q.ID,
                                  Specification = q.Specification,
                                  UnitOfMeasurement = tum == null ? string.Empty : tum.UnitName,
                                  NumberInSmallPackage = q.NumberInSmallPackage,
                                  NumberInLargePackage = q.NumberInLargePackage,
                                  LicenseNumber = q.LicenseNumber
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }

        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();

            List<Expression<Func<ProductSpecification, bool>>> whereFuncs = new List<Expression<Func<ProductSpecification, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.IncludeItemValues != null
                    && uiSearchObj.IncludeItemValues.Count > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeItemValues.Contains(x.ID));

                if (uiSearchObj.ExcludeItemValues != null
                    && uiSearchObj.ExcludeItemValues.Count > 0)
                    whereFuncs.Add(x => !uiSearchObj.ExcludeItemValues.Contains(x.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.ItemText))
                    whereFuncs.Add(x => x.Specification.Contains(uiSearchObj.ItemText));

                if (uiSearchObj.Extension != null)
                {
                    if (uiSearchObj.Extension.ProductID > 0)
                        whereFuncs.Add(x => x.ProductID == uiSearchObj.Extension.ProductID);
                }
            }

            uiDropdownItems = GetList(whereFuncs).Select(x => new UIDropdownItem()
            {
                ItemValue = x.ID,
                ItemText = x.Specification,
                Extension = new
                {
                    //基本单位
                    UnitName = x.UnitOfMeasurement.UnitName,
                    //每件数量
                    NumberInLargePackage = x.NumberInLargePackage.HasValue
                    ? x.NumberInLargePackage.Value : 1
                }
            }).ToList();

            return uiDropdownItems;
        }


    }
}
