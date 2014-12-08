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
    public class ClientInfoRepository : BaseRepository<ClientInfo>, IClientInfoRepository
    {
        public IList<UIClientInfo> GetUIList(UISearchClientInfo uiSearchObj = null)
        {
            IList<UIClientInfo> uiEntities = new List<UIClientInfo>();

            IQueryable<ClientInfo> query = null;

            List<Expression<Func<ClientInfo, bool>>> whereFuncs = new List<Expression<Func<ClientInfo, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.ClientUserID > 0)
                    whereFuncs.Add(x => x.ClientUserID == uiSearchObj.ClientUserID);

                if (uiSearchObj.ClientCompanyID > 0)
                    whereFuncs.Add(x => x.ClientCompanyID == uiSearchObj.ClientCompanyID);

                if (!string.IsNullOrEmpty(uiSearchObj.ClientCode))
                    whereFuncs.Add(x => x.ClientCode.Contains(uiSearchObj.ClientCode));

                if (!string.IsNullOrEmpty(uiSearchObj.ClientName))
                    whereFuncs.Add(x => x.ClientUser != null && x.ClientUser.ClientName.Contains(uiSearchObj.ClientName));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join cu in DB.ClientUser on q.ClientUserID equals cu.ID
                              join cc in DB.ClientCompany on q.ClientCompanyID equals cc.ID
                              select new UIClientInfo()
                              {
                                  ID = q.ID,
                                  ClientCode = q.ClientCode,
                                  ClientName = cu.ClientName,
                                  ClientCompany = cc.Name,
                                  ReceiverName = q.ReceiverName,
                                  PhoneNumber = q.PhoneNumber
                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIClientInfo> GetUIList(UISearchClientInfo uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIClientInfo> uiEntities = new List<UIClientInfo>();

            int total = 0;

            IQueryable<ClientInfo> query = null;

            List<Expression<Func<ClientInfo, bool>>> whereFuncs = new List<Expression<Func<ClientInfo, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.ClientUserID > 0)
                    whereFuncs.Add(x => x.ClientUserID == uiSearchObj.ClientUserID);

                if (uiSearchObj.ClientCompanyID > 0)
                    whereFuncs.Add(x => x.ClientCompanyID == uiSearchObj.ClientCompanyID);

                if (!string.IsNullOrEmpty(uiSearchObj.ClientCode))
                    whereFuncs.Add(x => x.ClientCode.Contains(uiSearchObj.ClientCode));

                if (!string.IsNullOrEmpty(uiSearchObj.ClientName))
                    whereFuncs.Add(x => x.ClientUser != null && x.ClientUser.ClientName.Contains(uiSearchObj.ClientName));
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join cu in DB.ClientUser on q.ClientUserID equals cu.ID
                              join cc in DB.ClientCompany on q.ClientCompanyID equals cc.ID
                              select new UIClientInfo()
                              {
                                  ID = q.ID,
                                  ClientCode = q.ClientCode,
                                  ClientName = cu.ClientName,
                                  ClientCompany = cc.Name,
                                  ReceiverName = q.ReceiverName,
                                  PhoneNumber = q.PhoneNumber
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }

        public int? GetMaxEntityID()
        {
            if (this.DB.ClientInfo.Count() > 0)
                return this.DB.ClientInfo.Max(x => x.ID);
            else return null;
        }

        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();

            List<Expression<Func<ClientInfo, bool>>> whereFuncs = new List<Expression<Func<ClientInfo, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.IncludeItemValues != null
                    && uiSearchObj.IncludeItemValues.Count > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeItemValues.Contains(x.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.ItemText))
                    whereFuncs.Add(x => x.ClientUser != null && x.ClientUser.ClientName.Contains(uiSearchObj.ItemText));
            }

            uiDropdownItems = GetList(whereFuncs).Select(x => new UIDropdownItem()
            {
                ItemValue = x.ID,
                ItemText = x.ClientUser == null ? string.Empty : x.ClientUser.ClientName
            }).ToList();

            return uiDropdownItems;
        }

        public IList<UIClientInfoBankAccount> GetBankAccounts(int? clientInfoID)
        {
            IList<UIClientInfoBankAccount> uiBankAccounts = new List<UIClientInfoBankAccount>();

            if (clientInfoID.HasValue && clientInfoID > 0)
            {
                uiBankAccounts = (from cba in DB.ClientInfoBankAccount.Where(x => x.IsDeleted == false && x.ClientInfoID == clientInfoID)
                                  join ba in DB.BankAccount on cba.BankAccountID equals ba.ID into tempBA
                                  from tba in tempBA.DefaultIfEmpty()
                                  //join cu in DB.Users on cba.CreatedBy equals cu.UserID into tempCU
                                  //from tcu in tempCU.DefaultIfEmpty()
                                  //join mu in DB.Users on cba.LastModifiedBy equals mu.UserID into tempMU
                                  //from tmu in tempMU.DefaultIfEmpty()
                                  //orderby sba.CreatedOn descending
                                  select new UIClientInfoBankAccount()
                                  {
                                      ID = cba.ID,
                                      ClientInfoID = cba.ClientInfoID,
                                      BankAccountID = cba.BankAccountID,
                                      AccountName = tba == null ? string.Empty : tba.AccountName,
                                      BankBranchName = tba == null ? string.Empty : tba.BankBranchName,
                                      Account = tba == null ? string.Empty : tba.Account,
                                      Comment = tba == null ? string.Empty : tba.Comment,
                                      //CreatedOn = cba.CreatedOn,
                                      //CreatedBy = tcu == null ? string.Empty : tcu.UserName,
                                      //LastModifiedOn = cba.LastModifiedOn,
                                      //LastModifiedBy = tmu == null ? string.Empty : tmu.UserName,
                                  }).ToList();
            }

            return uiBankAccounts;
        }


        public IList<UIClientInfoContact> GetContacts(int? clientInfoID)
        {
            IList<UIClientInfoContact> uiContacts = new List<UIClientInfoContact>();

            if (clientInfoID.HasValue && clientInfoID > 0)
            {
                uiContacts = (from cic in DB.ClientInfoContact.Where(x => x.IsDeleted == false && x.ClientInfoID == clientInfoID)
                              select new UIClientInfoContact()
                                  {
                                      ID = cic.ID,
                                      ClientInfoID = cic.ClientInfoID,
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
