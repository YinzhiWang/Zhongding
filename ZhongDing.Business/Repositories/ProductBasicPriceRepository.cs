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
    public class ProductBasicPriceRepository : BaseRepository<ProductBasicPrice>, IProductBasicPriceRepository
    {
        public IList<UIProductBasicPrice> GetUIList(UISearchProduct uiSearchObj = null)
        {
            IList<UIProductBasicPrice> uiEntities = new List<UIProductBasicPrice>();

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
                              join bp in DB.ProductBasicPrice.Where(x => x.IsDeleted == false)
                                on new { ProductID = q.ProductID.HasValue ? q.ProductID.Value : GlobalConst.INVALID_INT, ProductSpecificationID = q.ID }
                                equals new { bp.ProductID, bp.ProductSpecificationID } into tempBP
                              from tbp in tempBP.DefaultIfEmpty()
                              select new UIProductBasicPrice()
                              {
                                  ID = tbp == null ? GlobalConst.INVALID_INT : tbp.ID,
                                  ProductID = q.ProductID.HasValue ? q.ProductID.Value : GlobalConst.INVALID_INT,
                                  ProductSpecificationID = q.ID,
                                  ProductCode = p.ProductCode,
                                  ProductName = p.ProductName,
                                  Specification = q.Specification,
                                  FactoryName = s.FactoryName,
                                  ProcurePrice = tbp == null ? null : tbp.ProcurePrice,
                                  SalePrice = tbp == null ? null : tbp.SalePrice,
                                  Comment = tbp == null ? null : tbp.Comment

                              }).OrderBy(x => x.ProductID).ToList();
            }

            return uiEntities;
        }

        public IList<UIProductBasicPrice> GetUIList(UISearchProduct uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIProductBasicPrice> uiEntities = new List<UIProductBasicPrice>();

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
                              join bp in DB.ProductBasicPrice.Where(x => x.IsDeleted == false)
                                on new { ProductID = q.ProductID.HasValue ? q.ProductID.Value : GlobalConst.INVALID_INT, ProductSpecificationID = q.ID }
                                equals new { bp.ProductID, bp.ProductSpecificationID } into tempBP
                              from tbp in tempBP.DefaultIfEmpty()
                              select new UIProductBasicPrice()
                              {
                                  ID = tbp == null ? GlobalConst.INVALID_INT : tbp.ID,
                                  ProductID = q.ProductID.HasValue ? q.ProductID.Value : GlobalConst.INVALID_INT,
                                  ProductSpecificationID = q.ID,
                                  ProductCode = p.ProductCode,
                                  ProductName = p.ProductName,
                                  Specification = q.Specification,
                                  FactoryName = s.FactoryName,
                                  ProcurePrice = tbp == null ? null : tbp.ProcurePrice,
                                  SalePrice = tbp == null ? null : tbp.SalePrice,
                                  Comment = tbp == null ? null : tbp.Comment

                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
