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
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        public IList<UICompany> GetUIList(UISearchCompany uiSearchObj = null)
        {
            IList<UICompany> uiCompanyList = new List<UICompany>();

            IQueryable<Company> query = null;

            List<Expression<Func<Company, bool>>> whereFuncs = new List<Expression<Func<Company, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.CompanyCode))
                    whereFuncs.Add(x => x.CompanyCode.Contains(uiSearchObj.CompanyCode));

                if (!string.IsNullOrEmpty(uiSearchObj.CompanyName))
                    whereFuncs.Add(x => x.CompanyCode.Contains(uiSearchObj.CompanyName));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiCompanyList = query.ToList().Select(x => new UICompany()
                {
                    ID = x.ID,
                    CompanyCode = x.CompanyCode,
                    CompanyName = x.CompanyName,
                    ProviderTexRatio = x.ProviderTexRatio,
                    ClientTaxHighRatio = x.ClientTaxHighRatio,
                    ClientTaxLowRatio = x.ClientTaxLowRatio,
                    EnableTaxDeduction = x.EnableTaxDeduction,
                    ClientTaxDeductionRatio = x.ClientTaxDeductionRatio,
                    CreatedOn = x.CreatedOn,
                    CreatedBy = x.CreatedBy.HasValue ? UsersRepository.Instance.GetUserNameByID(x.CreatedBy.Value) : string.Empty,
                    LastModifiedOn = x.LastModifiedOn,
                    LastModifiedBy = x.LastModifiedBy.HasValue ? UsersRepository.Instance.GetUserNameByID(x.LastModifiedBy.Value) : string.Empty,
                }).ToList();
            }

            return uiCompanyList;
        }

        public IList<UICompany> GetUIList(UISearchCompany uiSearchObj, out int totalRecords)
        {
            IList<UICompany> uiCompanyList = new List<UICompany>();

            int total = 0;

            IQueryable<Company> query = null;

            List<Expression<Func<Company, bool>>> whereFuncs = new List<Expression<Func<Company, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.CompanyCode))
                    whereFuncs.Add(x => x.CompanyCode.Contains(uiSearchObj.CompanyCode));

                if (!string.IsNullOrEmpty(uiSearchObj.CompanyName))
                    whereFuncs.Add(x => x.CompanyName.Contains(uiSearchObj.CompanyName));

                if (uiSearchObj.IsNeedPaging)
                    query = GetList(uiSearchObj.PageIndex, uiSearchObj.PageSize, whereFuncs, out total);
                else
                    query = GetList(whereFuncs);
            }
            else
                query = GetList(whereFuncs);

            if (query != null)
            {
                uiCompanyList = query.ToList().Select(x => new UICompany()
                {
                    ID = x.ID,
                    CompanyCode = x.CompanyCode,
                    CompanyName = x.CompanyName,
                    ProviderTexRatio = x.ProviderTexRatio,
                    ClientTaxHighRatio = x.ClientTaxHighRatio,
                    ClientTaxLowRatio = x.ClientTaxLowRatio,
                    EnableTaxDeduction = x.EnableTaxDeduction,
                    ClientTaxDeductionRatio = x.ClientTaxDeductionRatio,
                    CreatedOn = x.CreatedOn,
                    CreatedBy = x.CreatedBy.HasValue ? UsersRepository.Instance.GetUserNameByID(x.CreatedBy.Value) : string.Empty,
                    LastModifiedOn = x.LastModifiedOn,
                    LastModifiedBy = x.LastModifiedBy.HasValue ? UsersRepository.Instance.GetUserNameByID(x.LastModifiedBy.Value) : string.Empty,
                }).ToList();
            }

            totalRecords = total;

            return uiCompanyList;
        }


    }
}
