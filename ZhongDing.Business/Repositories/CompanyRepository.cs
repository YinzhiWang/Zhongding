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
                uiCompanyList = (from q in query
                                 join cu in this.DB.Users on q.CreatedBy equals cu.UserID into tempCU
                                 from tcu in tempCU.DefaultIfEmpty()
                                 join mu in this.DB.Users on q.LastModifiedBy equals mu.UserID into tempMU
                                 from tmu in tempMU.DefaultIfEmpty()
                                 //orderby q.CreatedOn descending
                                 select new UICompany()
                                 {
                                     ID = q.ID,
                                     CompanyCode = q.CompanyCode,
                                     CompanyName = q.CompanyName,
                                     ProviderTexRatio = q.ProviderTexRatio,
                                     ClientTaxHighRatio = q.ClientTaxHighRatio,
                                     ClientTaxLowRatio = q.ClientTaxLowRatio,
                                     EnableTaxDeduction = q.EnableTaxDeduction,
                                     ClientTaxDeductionRatio = q.ClientTaxDeductionRatio,
                                     CreatedOn = q.CreatedOn,
                                     CreatedBy = tcu == null ? string.Empty : tcu.UserName,
                                     LastModifiedOn = q.LastModifiedOn,
                                     LastModifiedBy = tmu == null ? string.Empty : tmu.UserName,
                                 }).ToList();

            }

            return uiCompanyList;
        }

        public IList<UICompany> GetUIList(UISearchCompany uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
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
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiCompanyList = (from q in query
                                 join cu in this.DB.Users on q.CreatedBy equals cu.UserID into tempCU
                                 from tcu in tempCU.DefaultIfEmpty()
                                 join mu in this.DB.Users on q.LastModifiedBy equals mu.UserID into tempMU
                                 from tmu in tempMU.DefaultIfEmpty()
                                 //orderby q.CreatedOn descending
                                 select new UICompany()
                                 {
                                     ID = q.ID,
                                     CompanyCode = q.CompanyCode,
                                     CompanyName = q.CompanyName,
                                     ProviderTexRatio = q.ProviderTexRatio,
                                     ClientTaxHighRatio = q.ClientTaxHighRatio,
                                     ClientTaxLowRatio = q.ClientTaxLowRatio,
                                     EnableTaxDeduction = q.EnableTaxDeduction,
                                     ClientTaxDeductionRatio = q.ClientTaxDeductionRatio,
                                     CreatedOn = q.CreatedOn,
                                     CreatedBy = tcu == null ? string.Empty : tcu.UserName,
                                     LastModifiedOn = q.LastModifiedOn,
                                     LastModifiedBy = tmu == null ? string.Empty : tmu.UserName,
                                 }).ToList();

            }

            totalRecords = total;

            return uiCompanyList;
        }

        public int? GetMaxEntityID()
        {
            if (this.DB.Company.Count() > 0)
                return this.DB.Company.Max(x => x.ID);
            else
                return null;
        }

        public IList<UICompany> GetUIListForDLL()
        {
            return GetList(x => x.IsDeleted == false).Select(x => new UICompany() { ID = x.ID, CompanyName = x.CompanyName }).ToList();
        }
    }
}
