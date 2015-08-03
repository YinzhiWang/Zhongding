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
using ZhongDing.Common.Extension;

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

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);

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
                                  join mu in DB.Users on q.LastModifiedBy equals mu.UserID into tempMU
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

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);

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
                                  join mu in DB.Users on q.LastModifiedBy equals mu.UserID into tempMU
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
                                  //join cu in DB.Users on sba.CreatedBy equals cu.UserID into tempCU
                                  //from tcu in tempCU.DefaultIfEmpty()
                                  //join mu in DB.Users on sba.LastModifiedBy equals mu.UserID into tempMU
                                  //from tmu in tempMU.DefaultIfEmpty()
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
                                      //CreatedOn = sba.CreatedOn,
                                      //CreatedBy = tcu == null ? string.Empty : tcu.UserName,
                                      //LastModifiedOn = sba.LastModifiedOn,
                                      //LastModifiedBy = tmu == null ? string.Empty : tmu.UserName,
                                  }).ToList();
            }

            return uiBankAccounts;
        }


        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();

            List<Expression<Func<Supplier, bool>>> whereFuncs = new List<Expression<Func<Supplier, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.IncludeItemValues != null
                    && uiSearchObj.IncludeItemValues.Count > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeItemValues.Contains(x.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.ItemText))
                    whereFuncs.Add(x => x.SupplierName.Contains(uiSearchObj.ItemText));

                if (uiSearchObj.Extension != null)
                {
                    if (uiSearchObj.Extension.CompanyID > 0)
                        whereFuncs.Add(x => x.CompanyID == uiSearchObj.Extension.CompanyID);
                }
            }

            uiDropdownItems = GetList(whereFuncs).Select(x => new UIDropdownItem()
            {
                ItemValue = x.ID,
                ItemText = x.SupplierName,
                Extension = new { FactoryName = x.FactoryName }
            }).ToList();

            return uiDropdownItems;
        }
        public Supplier GetBySupplierName(string supplierName)
        {
            var item = DB.Supplier.Where(x => x.IsDeleted == false && x.SupplierName == supplierName).FirstOrDefault();
            return item;
        }


        public IList<UISupplierContact> GetContacts(int? supplyID)
        {

            IList<UISupplierContact> uiContacts = new List<UISupplierContact>();

            if (supplyID.BiggerThanZero())
            {
                uiContacts = (from cic in DB.SupplierContact.Where(x => x.IsDeleted == false && x.SupplierID == supplyID)
                              select new UISupplierContact()
                                  {
                                      ID = cic.ID,
                                      SupplierID = cic.SupplierID,
                                      ContactName = cic.ContactName,
                                      PhoneNumber = cic.PhoneNumber,
                                      Address = cic.Address,
                                      Comment = cic.Comment
                                  }).ToList();
            }

            return uiContacts;

        }
    }
}
