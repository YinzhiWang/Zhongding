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
    public class ProductHighPriceRepository : BaseRepository<ProductHighPrice>, IProductHighPriceRepository
    {
        public IList<UIProductHighPrice> GetUIList(UISearchProduct uiSearchObj = null)
        {
            IList<UIProductHighPrice> uiEntities = new List<UIProductHighPrice>();

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
                              join bp in DB.ProductHighPrice.Where(x => x.IsDeleted == false)
                                on new { ProductID = q.ProductID.HasValue ? q.ProductID.Value : GlobalConst.INVALID_INT, ProductSpecificationID = q.ID }
                                equals new { bp.ProductID, bp.ProductSpecificationID } into tempBP
                              from tbp in tempBP.DefaultIfEmpty()
                              select new UIProductHighPrice()
                              {
                                  ID = tbp == null ? GlobalConst.INVALID_INT : tbp.ID,
                                  ProductID = q.ProductID.HasValue ? q.ProductID.Value : GlobalConst.INVALID_INT,
                                  ProductSpecificationID = q.ID,
                                  ProductCode = p.ProductCode,
                                  ProductName = p.ProductName,
                                  Specification = q.Specification,
                                  FactoryName = s.FactoryName,
                                  HighPrice = tbp == null ? null : tbp.HighPrice,
                                  ActualProcurePrice = tbp == null ? null : tbp.ActualProcurePrice,
                                  ActualSalePrice = tbp == null ? null : tbp.ActualSalePrice,
                                  SupplierTaxRatio = tbp == null ? null : tbp.SupplierTaxRatio,
                                  ClientTaxRatio = tbp == null ? null : tbp.ClientTaxRatio,
                                  Comment = tbp == null ? null : tbp.Comment

                              }).OrderBy(x => x.ProductID).ToList();
            }

            return uiEntities;
        }

        public IList<UIProductHighPrice> GetUIList(UISearchProduct uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIProductHighPrice> uiEntities = new List<UIProductHighPrice>();

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
                              join bp in DB.ProductHighPrice.Where(x => x.IsDeleted == false)
                                on new { ProductID = q.ProductID.HasValue ? q.ProductID.Value : GlobalConst.INVALID_INT, ProductSpecificationID = q.ID }
                                equals new { bp.ProductID, bp.ProductSpecificationID } into tempBP
                              from tbp in tempBP.DefaultIfEmpty()
                              select new UIProductHighPrice()
                              {
                                  ID = tbp == null ? GlobalConst.INVALID_INT : tbp.ID,
                                  ProductID = q.ProductID.HasValue ? q.ProductID.Value : GlobalConst.INVALID_INT,
                                  ProductSpecificationID = q.ID,
                                  ProductCode = p.ProductCode,
                                  ProductName = p.ProductName,
                                  Specification = q.Specification,
                                  FactoryName = s.FactoryName,
                                  HighPrice = tbp == null ? null : tbp.HighPrice,
                                  ActualProcurePrice = tbp == null ? null : tbp.ActualProcurePrice,
                                  ActualSalePrice = tbp == null ? null : tbp.ActualSalePrice,
                                  SupplierTaxRatio = tbp == null ? null : tbp.SupplierTaxRatio,
                                  ClientTaxRatio = tbp == null ? null : tbp.ClientTaxRatio,
                                  Comment = tbp == null ? null : tbp.Comment

                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
