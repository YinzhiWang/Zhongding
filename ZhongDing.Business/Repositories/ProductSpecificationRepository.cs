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
        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();

            List<Expression<Func<ProductSpecification, bool>>> whereFuncs = new List<Expression<Func<ProductSpecification, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ItemValues != null
                    && uiSearchObj.ItemValues.Count > 0)
                    whereFuncs.Add(x => uiSearchObj.ItemValues.Contains(x.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.ItemText))
                    whereFuncs.Add(x => x.Specification.Contains(uiSearchObj.ItemText));

                if (uiSearchObj.ParentItemValue.HasValue)
                    whereFuncs.Add(x => x.ProductID == uiSearchObj.ParentItemValue);
            }

            uiDropdownItems = GetList(whereFuncs).Select(x => new UIDropdownItem()
            {
                ItemValue = x.ID,
                ItemText = x.Specification
            }).ToList();

            return uiDropdownItems;
        }
    }
}
