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
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories
{
    public class DeptMarketDivisionRepository : BaseRepository<DeptMarketDivision>, IDeptMarketDivisionRepository
    {
        public IList<UIDeptMarketDivision> GetUIList(UISearchDeptMarketDivision uiSearchObj = null)
        {
            IList<UIDeptMarketDivision> uiEntities = new List<UIDeptMarketDivision>();

            IQueryable<DeptMarketDivision> query = null;

            List<Expression<Func<DeptMarketDivision, bool>>> whereFuncs = new List<Expression<Func<DeptMarketDivision, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.DepartmentID > 0)
                    whereFuncs.Add(x => x.Users.DepartmentID == uiSearchObj.DepartmentID);
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join u in DB.Users on q.UserID equals u.UserID
                              join dp in DB.Department on u.DepartmentID equals dp.ID
                              select new UIDeptMarketDivision()
                              {
                                  ID = q.ID,
                                  DepartmentTypeID = dp.DepartmentTypeID,
                                  BusinessManager = u.FullName,
                                  Markets = q.MarketID,
                                  ProductIDs = q.DeptMarketProduct.Where(x => x.IsDeleted == false).Select(x => x.ProductID)
                              }).ToList();

                foreach (var entity in uiEntities)
                {
                    if (!string.IsNullOrEmpty(entity.Markets))
                    {
                        var marketIDs = entity.Markets.Split(',').Select(x => Convert.ToInt32(x)).ToList();

                        entity.Markets = string.Join(", ", DB.DeptMarket.Where(x => marketIDs.Contains(x.ID)).Select(x => x.MarketName).ToList());
                    }

                    entity.Products = string.Join(", ", DB.Product.Where(x => entity.ProductIDs.Contains(x.ID)).Select(x => x.ProductName).ToList());
                }
            }

            return uiEntities;
        }

        public IList<UIDeptMarketDivision> GetUIList(UISearchDeptMarketDivision uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIDeptMarketDivision> uiEntities = new List<UIDeptMarketDivision>();

            int total = 0;

            IQueryable<DeptMarketDivision> query = null;

            List<Expression<Func<DeptMarketDivision, bool>>> whereFuncs = new List<Expression<Func<DeptMarketDivision, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.DepartmentID > 0)
                    whereFuncs.Add(x => x.Users.DepartmentID == uiSearchObj.DepartmentID);
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join u in DB.Users on q.UserID equals u.UserID
                              join dp in DB.Department on u.DepartmentID equals dp.ID
                              select new UIDeptMarketDivision()
                              {
                                  ID = q.ID,
                                  DepartmentTypeID = dp.DepartmentTypeID,
                                  BusinessManager = u.FullName,
                                  Markets = q.MarketID,
                                  ProductIDs = q.DeptMarketProduct.Where(x => x.IsDeleted == false).Select(x => x.ProductID)
                              }).ToList();

                foreach (var entity in uiEntities)
                {
                    if (!string.IsNullOrEmpty(entity.Markets))
                    {
                        var marketIDs = entity.Markets.Split(',').Select(x => Convert.ToInt32(x)).ToList();

                        entity.Markets = string.Join(", ", DB.DeptMarket.Where(x => marketIDs.Contains(x.ID)).Select(x => x.MarketName).ToList());
                    }

                    entity.Products = string.Join(", ", DB.Product.Where(x => entity.ProductIDs.Contains(x.ID)).Select(x => x.ProductName).ToList());
                }
            }

            totalRecords = total;

            return uiEntities;
        }

        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();

            List<Expression<Func<DeptMarketDivision, bool>>> whereFuncs = new List<Expression<Func<DeptMarketDivision, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.IncludeItemValues != null
                    && uiSearchObj.IncludeItemValues.Count > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeItemValues.Contains(x.ID));

                if (uiSearchObj.Extension != null)
                {
                    if (uiSearchObj.Extension.DepartmentID > 0)
                        whereFuncs.Add(x => x.Users.DepartmentID == uiSearchObj.Extension.DepartmentID);
                }
            }

            var query = GetList(whereFuncs).Select(x => x.MarketID).ToList();

            if (query != null)
            {
                List<int> marketIDs = new List<int>();

                foreach (var sMarketID in query)
                {
                    var curMarkIDs = sMarketID.Split(',').Select(x => Convert.ToInt32(x)).ToList();

                    marketIDs.AddRange(curMarkIDs);
                }

                marketIDs = marketIDs.Distinct().ToList();

                var tempDropdownItems = DB.DeptMarket.Where(x => marketIDs.Contains(x.ID))
                     .Select(x => new UIDropdownItem()
                     {
                         ItemValue = x.ID,
                         ItemText = x.MarketName
                     });

                if (uiSearchObj != null && !string.IsNullOrEmpty(uiSearchObj.ItemText))
                    tempDropdownItems = tempDropdownItems.Where(x => x.ItemText.Contains(uiSearchObj.ItemText));

                uiDropdownItems = tempDropdownItems.ToList();
            }

            return uiDropdownItems;
        }
    }
}
