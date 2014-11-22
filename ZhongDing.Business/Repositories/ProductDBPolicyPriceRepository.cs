using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Common;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories
{
    public class ProductDBPolicyPriceRepository : BaseRepository<ProductDBPolicyPrice>, IProductDBPolicyPriceRepository
    {
        public IList<UIProductDBPolicyPrice> GetUIList(UISearchProduct uiSearchObj = null)
        {
            IList<UIProductDBPolicyPrice> uiEntities = new List<UIProductDBPolicyPrice>();

            IQueryable<ProductSpecification> query = this.DB.ProductSpecification;

            List<Expression<Func<ProductSpecification, bool>>> whereFuncs = new List<Expression<Func<ProductSpecification, bool>>>();

            whereFuncs.Add(x => x.IsDeleted == false);

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.Product != null && x.Product.CompanyID == uiSearchObj.CompanyID);

                if (!string.IsNullOrEmpty(uiSearchObj.ProductName))
                    whereFuncs.Add(x => x.Product != null && x.Product.ProductName.Contains(uiSearchObj.ProductName));
            }

            foreach (var item in whereFuncs)
            {
                query = query.Where(item);
            }

            if (query != null)
            {
                uiEntities = (from q in query
                              join p in DB.Product on q.ProductID equals p.ID
                              join s in DB.Supplier on p.SupplierID equals s.ID
                              join um in DB.UnitOfMeasurement on q.UnitOfMeasurementID equals um.ID into tempUM
                              from tum in tempUM.DefaultIfEmpty()
                              join bp in DB.ProductDBPolicyPrice.Where(x => x.IsDeleted == false)
                                on new { ProductID = q.ProductID.HasValue ? q.ProductID.Value : GlobalConst.INVALID_INT, ProductSpecificationID = q.ID }
                                equals new { bp.ProductID, bp.ProductSpecificationID } into tempBP
                              from tbp in tempBP.DefaultIfEmpty()
                              select new UIProductDBPolicyPrice()
                              {
                                  ID = tbp == null ? GlobalConst.INVALID_INT : tbp.ID,
                                  ProductID = q.ProductID.HasValue ? q.ProductID.Value : GlobalConst.INVALID_INT,
                                  ProductSpecificationID = q.ID,
                                  NumberInLargePackage = q.NumberInLargePackage,
                                  UnitOfMeasurement = tum == null ? string.Empty : tum.UnitName,
                                  ProductCode = p.ProductCode,
                                  ProductName = p.ProductName,
                                  Specification = q.Specification,
                                  FactoryName = s.FactoryName,
                                  BidPrice = tbp == null ? null : tbp.BidPrice,
                                  FeeRatio = tbp == null ? null : tbp.FeeRatio,
                                  PreferredPrice = tbp == null ? null : tbp.PreferredPrice,
                                  PolicyPrice = tbp == null ? null : tbp.PolicyPrice,
                                  Comment = tbp == null ? null : tbp.Comment
                              }).OrderBy(x => x.ProductID).ToList();

                foreach (var entity in uiEntities)
                {
                    if (entity.NumberInLargePackage > 0
                        && !string.IsNullOrEmpty(entity.UnitOfMeasurement))
                    {
                        entity.PackageDescription = (entity.NumberInLargePackage.HasValue ? entity.NumberInLargePackage.Value : 0)
                            + entity.UnitOfMeasurement + "/件";
                    }
                }
            }

            return uiEntities;
        }

        public IList<UIProductDBPolicyPrice> GetUIList(UISearchProduct uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIProductDBPolicyPrice> uiEntities = new List<UIProductDBPolicyPrice>();

            int total = 0;

            IQueryable<ProductSpecification> query = this.DB.ProductSpecification;

            List<Expression<Func<ProductSpecification, bool>>> whereFuncs = new List<Expression<Func<ProductSpecification, bool>>>();

            whereFuncs.Add(x => x.IsDeleted == false);

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.Product != null && x.Product.CompanyID == uiSearchObj.CompanyID);

                if (!string.IsNullOrEmpty(uiSearchObj.ProductName))
                    whereFuncs.Add(x => x.Product != null && x.Product.ProductName.Contains(uiSearchObj.ProductName));
            }

            foreach (var item in whereFuncs)
            {
                query = query.Where(item);
            }

            if (query != null)
            {
                total = query.Count();

                query = query.AsQueryable().OrderBy(x => x.ProductID).Skip(pageSize * pageIndex).Take(pageSize);

                uiEntities = (from q in query
                              join p in DB.Product on q.ProductID equals p.ID
                              join s in DB.Supplier on p.SupplierID equals s.ID
                              join um in DB.UnitOfMeasurement on q.UnitOfMeasurementID equals um.ID into tempUM
                              from tum in tempUM.DefaultIfEmpty()
                              join bp in DB.ProductDBPolicyPrice.Where(x => x.IsDeleted == false)
                                on new { ProductID = q.ProductID.HasValue ? q.ProductID.Value : GlobalConst.INVALID_INT, ProductSpecificationID = q.ID }
                                equals new { bp.ProductID, bp.ProductSpecificationID } into tempBP
                              from tbp in tempBP.DefaultIfEmpty()
                              select new UIProductDBPolicyPrice()
                              {
                                  ID = tbp == null ? GlobalConst.INVALID_INT : tbp.ID,
                                  ProductID = q.ProductID.HasValue ? q.ProductID.Value : GlobalConst.INVALID_INT,
                                  ProductSpecificationID = q.ID,
                                  NumberInLargePackage = q.NumberInLargePackage,
                                  UnitOfMeasurement = tum == null ? string.Empty : tum.UnitName,
                                  ProductCode = p.ProductCode,
                                  ProductName = p.ProductName,
                                  Specification = q.Specification,
                                  FactoryName = s.FactoryName,
                                  BidPrice = tbp == null ? null : tbp.BidPrice,
                                  FeeRatio = tbp == null ? null : tbp.FeeRatio,
                                  PreferredPrice = tbp == null ? null : tbp.PreferredPrice,
                                  PolicyPrice = tbp == null ? null : tbp.PolicyPrice,
                                  Comment = tbp == null ? null : tbp.Comment
                              }).ToList();

                foreach (var entity in uiEntities)
                {
                    if (entity.NumberInLargePackage > 0
                        && !string.IsNullOrEmpty(entity.UnitOfMeasurement))
                    {
                        entity.PackageDescription = (entity.NumberInLargePackage.HasValue ? entity.NumberInLargePackage.Value : 0)
                            + entity.UnitOfMeasurement + "/件";
                    }
                }
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
