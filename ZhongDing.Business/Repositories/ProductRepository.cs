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
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public IList<UIProduct> GetUIList(UISearchProduct uiSearchObj = null)
        {
            IList<UIProduct> uiEntities = new List<UIProduct>();

            IQueryable<Product> query = null;

            List<Expression<Func<Product, bool>>> whereFuncs = new List<Expression<Func<Product, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);

                if (!string.IsNullOrEmpty(uiSearchObj.ProductCode))
                    whereFuncs.Add(x => x.ProductCode.Contains(uiSearchObj.ProductCode));

                if (!string.IsNullOrEmpty(uiSearchObj.ProductName))
                    whereFuncs.Add(x => x.ProductName.Contains(uiSearchObj.ProductName));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join c in DB.ProductCategory on q.CategoryID equals c.ID into tempC
                              from tc in tempC.DefaultIfEmpty()
                              join s in DB.Supplier on q.SupplierID equals s.ID into tempS
                              from ts in tempS.DefaultIfEmpty()
                              join d in DB.Department on q.DepartmentID equals d.ID into tempD
                              from td in tempD.DefaultIfEmpty()
                              select new UIProduct()
                              {
                                  ID = q.ID,
                                  ProductCode = q.ProductCode,
                                  ProductName = q.ProductName,
                                  ProductCategoryName = tc == null ? string.Empty : tc.CategoryName,
                                  SupplierName = ts == null ? string.Empty : ts.SupplierName,
                                  DepartmentName = td == null ? string.Empty : td.DepartmentName

                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIProduct> GetUIList(UISearchProduct uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIProduct> uiEntities = new List<UIProduct>();

            int total = 0;

            IQueryable<Product> query = null;

            List<Expression<Func<Product, bool>>> whereFuncs = new List<Expression<Func<Product, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);

                if (!string.IsNullOrEmpty(uiSearchObj.ProductCode))
                    whereFuncs.Add(x => x.ProductCode.Contains(uiSearchObj.ProductCode));

                if (!string.IsNullOrEmpty(uiSearchObj.ProductName))
                    whereFuncs.Add(x => x.ProductName.Contains(uiSearchObj.ProductName));
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join c in DB.ProductCategory on q.CategoryID equals c.ID into tempC
                              from tc in tempC.DefaultIfEmpty()
                              join s in DB.Supplier on q.SupplierID equals s.ID into tempS
                              from ts in tempS.DefaultIfEmpty()
                              join d in DB.Department on q.DepartmentID equals d.ID into tempD
                              from td in tempD.DefaultIfEmpty()
                              select new UIProduct()
                              {
                                  ID = q.ID,
                                  ProductCode = q.ProductCode,
                                  ProductName = q.ProductName,
                                  ProductCategoryName = tc == null ? string.Empty : tc.CategoryName,
                                  SupplierName = ts == null ? string.Empty : ts.SupplierName,
                                  DepartmentName = td == null ? string.Empty : td.DepartmentName

                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }

        public int? GetMaxEntityID()
        {
            if (this.DB.Product.Count() > 0)
                return this.DB.Product.Max(x => x.ID);
            else
                return null;
        }

        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();

            List<Expression<Func<Product, bool>>> whereFuncs = new List<Expression<Func<Product, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ItemValues != null
                    && uiSearchObj.ItemValues.Count > 0)
                    whereFuncs.Add(x => uiSearchObj.ItemValues.Contains(x.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.ItemText))
                    whereFuncs.Add(x => x.ProductName.Contains(uiSearchObj.ItemText));

                if (uiSearchObj.ExtensionEntityID > 0)
                    whereFuncs.Add(x => x.CategoryID == uiSearchObj.ExtensionEntityID);
            }

            uiDropdownItems = GetList(whereFuncs).Select(x => new UIDropdownItem()
            {
                ItemValue = x.ID,
                ItemText = x.ProductName
            }).ToList();

            return uiDropdownItems;
        }
    }
}
