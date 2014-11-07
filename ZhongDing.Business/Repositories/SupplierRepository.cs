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
    public class SupplierRepository : BaseRepository<Supplier>, ISupplierRepository
    {
        public IList<UISupplier> GetUIList(UISearchSupplier uiSearchObj = null)
        {
            IList<UISupplier> uiSupplierList = new List<UISupplier>();

            IQueryable<Supplier> query = null;

            List<Expression<Func<Supplier, bool>>> whereFuncs = new List<Expression<Func<Supplier, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.SupplierCode))
                    whereFuncs.Add(x => x.SupplierCode.Contains(uiSearchObj.SupplierCode));

                if (!string.IsNullOrEmpty(uiSearchObj.SupplierName))
                    whereFuncs.Add(x => x.SupplierName.Contains(uiSearchObj.SupplierName));

                if (!string.IsNullOrEmpty(uiSearchObj.FactoryName))
                    whereFuncs.Add(x => x.FactoryName.Contains(uiSearchObj.FactoryName));

                if (!string.IsNullOrEmpty(uiSearchObj.ContactPerson))
                    whereFuncs.Add(x => x.ContactPerson.Contains(uiSearchObj.ContactPerson));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiSupplierList = (from q in query
                                  join cu in DB.Users on q.CreatedBy equals cu.UserID into tempCU
                                  from tcu in tempCU.DefaultIfEmpty()
                                  join mu in DB.Users on q.CreatedBy equals mu.UserID into tempMU
                                  from tmu in tempMU.DefaultIfEmpty()
                                  //orderby q.CreatedOn descending
                                  select new UISupplier()
                                  {
                                      ID = q.ID,
                                      SupplierCode = q.SupplierCode,
                                      SupplierName = q.SupplierName,
                                      FactoryName = q.FactoryName,
                                      ContactPerson = q.ContactPerson,
                                      PhoneNumber = q.PhoneNumber,
                                      CreatedOn = q.CreatedOn,
                                      CreatedBy = tcu == null ? string.Empty : tcu.UserName,
                                      LastModifiedOn = q.LastModifiedOn,
                                      LastModifiedBy = tmu == null ? string.Empty : tmu.UserName,
                                  }).ToList();
            }

            return uiSupplierList;
        }

        public IList<UISupplier> GetUIList(UISearchSupplier uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UISupplier> uiSupplierList = new List<UISupplier>();

            int total = 0;

            IQueryable<Supplier> query = null;

            List<Expression<Func<Supplier, bool>>> whereFuncs = new List<Expression<Func<Supplier, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.SupplierCode))
                    whereFuncs.Add(x => x.SupplierCode.Contains(uiSearchObj.SupplierCode));

                if (!string.IsNullOrEmpty(uiSearchObj.SupplierName))
                    whereFuncs.Add(x => x.SupplierName.Contains(uiSearchObj.SupplierName));

                if (!string.IsNullOrEmpty(uiSearchObj.FactoryName))
                    whereFuncs.Add(x => x.FactoryName.Contains(uiSearchObj.FactoryName));

                if (!string.IsNullOrEmpty(uiSearchObj.ContactPerson))
                    whereFuncs.Add(x => x.ContactPerson.Contains(uiSearchObj.ContactPerson));
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiSupplierList = (from q in query
                                  join cu in DB.Users on q.CreatedBy equals cu.UserID into tempCU
                                  from tcu in tempCU.DefaultIfEmpty()
                                  join mu in DB.Users on q.CreatedBy equals mu.UserID into tempMU
                                  from tmu in tempMU.DefaultIfEmpty()
                                  //orderby q.CreatedOn descending
                                  select new UISupplier()
                                  {
                                      ID = q.ID,
                                      SupplierCode = q.SupplierCode,
                                      SupplierName = q.SupplierName,
                                      FactoryName = q.FactoryName,
                                      ContactPerson = q.ContactPerson,
                                      PhoneNumber = q.PhoneNumber,
                                      CreatedOn = q.CreatedOn,
                                      CreatedBy = tcu == null ? string.Empty : tcu.UserName,
                                      LastModifiedOn = q.LastModifiedOn,
                                      LastModifiedBy = tmu == null ? string.Empty : tmu.UserName,
                                  }).ToList();
            }

            totalRecords = total;

            return uiSupplierList;
        }

        public int? GetMaxEntityID()
        {
            if (this.DB.Supplier.Count() > 0)
                return this.DB.Supplier.Max(x => x.ID);
            else
                return null;
        }

        public IList<UISupplierBankAccount> GetBankAccounts(int? supplierID)
        {
            IList<UISupplierBankAccount> uiBankAccounts = new List<UISupplierBankAccount>();

            if (supplierID.HasValue && supplierID > 0)
            {
                uiBankAccounts = (from sba in DB.SupplierBankAccount.Where(x => x.IsDeleted == false && x.SupplierID == supplierID)
                                  join ba in DB.BankAccount on sba.BankAccountID equals ba.ID into tempBA
                                  from tba in tempBA.DefaultIfEmpty()
                                  join cu in DB.Users on sba.CreatedBy equals cu.UserID into tempCU
                                  from tcu in tempCU.DefaultIfEmpty()
                                  join mu in DB.Users on sba.LastModifiedBy equals mu.UserID into tempMU
                                  from tmu in tempMU.DefaultIfEmpty()
                                  //orderby sba.CreatedOn descending
                                  select new UISupplierBankAccount()
                                  {
                                      ID = sba.ID,
                                      SupplierID = sba.SupplierID,
                                      BankAccountID = sba.BankAccountID,
                                      AccountName = tba == null ? string.Empty : tba.AccountName,
                                      BankBranchName = tba == null ? string.Empty : tba.BankBranchName,
                                      Account = tba == null ? string.Empty : tba.Account,
                                      Comment = tba == null ? string.Empty : tba.Comment,
                                      CreatedOn = sba.CreatedOn,
                                      CreatedBy = tcu == null ? string.Empty : tcu.UserName,
                                      LastModifiedOn = sba.LastModifiedOn,
                                      LastModifiedBy = tmu == null ? string.Empty : tmu.UserName,
                                  }).ToList();
            }

            return uiBankAccounts;
        }


        public IList<UISupplierCertificate> GetCertificates(int? supplierID, int? ownerTypeID)
        {
            IList<UISupplierCertificate> uiCertificates = new List<UISupplierCertificate>();

            if (supplierID.HasValue && supplierID > 0)
            {
                uiCertificates = (from sc in DB.SupplierCertificate.Where(x => x.IsDeleted == false && x.SupplierID == supplierID)
                                  join c in DB.Certificate.Where(x => x.OwnerTypeID == ownerTypeID) on sc.CertificateID equals c.ID into tempC
                                  from tc in tempC.DefaultIfEmpty()
                                  join ct in DB.CertificateType on tc.CertificateTypeID equals ct.ID into tempCT
                                  from tct in tempCT.DefaultIfEmpty()
                                  join cu in DB.Users on sc.CreatedBy equals cu.UserID into tempCU
                                  from tcu in tempCU.DefaultIfEmpty()
                                  join mu in DB.Users on sc.LastModifiedBy equals mu.UserID into tempMU
                                  from tmu in tempMU.DefaultIfEmpty()
                                  //orderby sc.CreatedOn descending
                                  select new UISupplierCertificate()
                                  {
                                      ID = sc.ID,
                                      SupplierID = sc.SupplierID,
                                      CertificateID = sc.CertificateID,
                                      CertificateTypeName = tct == null ? string.Empty : tct.CertificateType1,
                                      IsGotten = tc == null ? false : tc.IsGotten,
                                      GottenDescription = tc == null ? string.Empty
                                      : (tc.IsGotten.HasValue && tc.IsGotten.Value) ? GlobalConst.GOTTEN_DESC_HAVE : GlobalConst.GOTTEN_DESC_NONHAVE,
                                      EffectiveFrom = tc == null ? null : tc.EffectiveFrom,
                                      EffectiveTo = tc == null ? null : tc.EffectiveTo,
                                      IsNeedAlert = tc == null ? false : tc.IsNeedAlert,
                                      AlertBeforeDays = tc == null ? 0 : tc.AlertBeforeDays,
                                      Comment = tc == null ? string.Empty : tc.Comment
                                  }
                                 ).ToList();

                foreach (var uiCertificate in uiCertificates)
                {
                    if (uiCertificate.EffectiveFrom.HasValue
                        && uiCertificate.EffectiveTo.HasValue)
                    {
                        uiCertificate.EffectiveDateDescription = uiCertificate.EffectiveFrom.ToString("yyyy/MM/dd")
                            + " - " + uiCertificate.EffectiveTo.ToString("yyyy/MM/dd");
                    }
                }
            }

            return uiCertificates;
        }
    }
}
