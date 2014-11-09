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
