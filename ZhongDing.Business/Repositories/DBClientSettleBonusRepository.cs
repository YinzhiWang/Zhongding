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
    public class DBClientSettleBonusRepository : BaseRepository<DBClientSettleBonus>, IDBClientSettleBonusRepository
    {
        public IList<UIDBClientSettleBonus> GetUIList(UISearchDBClientSettleBonus uiSearchObj)
        {
            IList<UIDBClientSettleBonus> uiEntities = new List<UIDBClientSettleBonus>();

            IQueryable<DBClientSettleBonus> query = null;

            var whereFuncs = new List<Expression<Func<DBClientSettleBonus, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.DBClientSettlementID > 0)
                    whereFuncs.Add(x => x.DBClientSettlementID.Equals(uiSearchObj.DBClientSettlementID));

                if (!string.IsNullOrEmpty(uiSearchObj.ClientUserName))
                    whereFuncs.Add(x => x.DBClientBonus != null && x.DBClientBonus.ClientUser != null
                        && x.DBClientBonus.ClientUser.ClientName.Contains(uiSearchObj.ClientUserName));

                if (uiSearchObj.OnlyIncludeNeedSettlement)
                    whereFuncs.Add(x => x.IsNeedSettlement == true);
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join cb in DB.DBClientBonus on q.DBClientBonusID equals cb.ID
                              join p in DB.Product on cb.ProductID equals p.ID
                              join ps in DB.ProductSpecification on cb.ProductSpecificationID equals ps.ID
                              join cu in DB.ClientUser on cb.ClientUserID equals cu.ID
                              join ba in DB.BankAccount on cu.DBBankAccountID equals ba.ID into tempBA
                              from tba in tempBA.DefaultIfEmpty()
                              select new UIDBClientSettleBonus()
                              {
                                  ID = q.ID,
                                  ClientUserName = cu.ClientName,
                                  HospitalIDs = cb.DBClientBonusHospital.Select(x => x.HospitalID),
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  SettlementDate = cb.SettlementDate,
                                  IsNeedSettlement = q.IsNeedSettlement,
                                  BonusAmount = cb.BonusAmount,
                                  PerformanceAmount = cb.PerformanceAmount,
                                  TotalPayAmount = q.TotalPayAmount,
                                  IsSettled = cb.IsSettled,
                                  IsManualSettled = q.IsManualSettled,
                                  ClientDBBankAccount = tba != null
                                    ? (tba.AccountName + " " + tba.BankBranchName + " " + tba.Account)
                                    : string.Empty
                              }).ToList();

                foreach (var uiEntity in uiEntities)
                {
                    uiEntity.Hospitals = string.Join(", ", DB.Hospital.Where(x => uiEntity.HospitalIDs.Contains(x.ID)).Select(x => x.HospitalName).ToList());

                }
            }

            return uiEntities;
        }

        public IList<UIDBClientSettleBonus> GetUIList(UISearchDBClientSettleBonus uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIDBClientSettleBonus> uiEntities = new List<UIDBClientSettleBonus>();

            int total = 0;

            IQueryable<DBClientSettleBonus> query = null;

            var whereFuncs = new List<Expression<Func<DBClientSettleBonus, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.DBClientSettlementID > 0)
                    whereFuncs.Add(x => x.DBClientSettlementID.Equals(uiSearchObj.DBClientSettlementID));

                if (!string.IsNullOrEmpty(uiSearchObj.ClientUserName))
                    whereFuncs.Add(x => x.DBClientBonus != null && x.DBClientBonus.ClientUser != null
                        && x.DBClientBonus.ClientUser.ClientName.Contains(uiSearchObj.ClientUserName));

                if (uiSearchObj.OnlyIncludeNeedSettlement)
                    whereFuncs.Add(x => x.IsNeedSettlement == true);
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join cb in DB.DBClientBonus on q.DBClientBonusID equals cb.ID
                              join p in DB.Product on cb.ProductID equals p.ID
                              join ps in DB.ProductSpecification on cb.ProductSpecificationID equals ps.ID
                              join cu in DB.ClientUser on cb.ClientUserID equals cu.ID
                              join ba in DB.BankAccount on cu.DBBankAccountID equals ba.ID into tempBA
                              from tba in tempBA.DefaultIfEmpty()
                              select new UIDBClientSettleBonus()
                              {
                                  ID = q.ID,
                                  ClientUserName = cu.ClientName,
                                  HospitalIDs = cb.DBClientBonusHospital.Select(x => x.HospitalID),
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  SettlementDate = cb.SettlementDate,
                                  IsNeedSettlement = q.IsNeedSettlement,
                                  BonusAmount = cb.BonusAmount,
                                  PerformanceAmount = cb.PerformanceAmount,
                                  TotalPayAmount = q.TotalPayAmount,
                                  IsSettled = cb.IsSettled,
                                  IsManualSettled = q.IsManualSettled,
                                  ClientDBBankAccount = tba != null
                                    ? (tba.AccountName + " " + tba.BankBranchName + " " + tba.Account)
                                    : string.Empty
                              }).ToList();

                foreach (var uiEntity in uiEntities)
                {
                    uiEntity.Hospitals = string.Join(", ", DB.Hospital.Where(x => uiEntity.HospitalIDs.Contains(x.ID)).Select(x => x.HospitalName).ToList());

                }
            }

            totalRecords = total;

            return uiEntities;
        }

        public IList<UIDBClientSettleBonusPayment> GetNeedPayUIList(UISearchDBClientSettleBonus uiSearchObj)
        {
            IList<UIDBClientSettleBonusPayment> uiEntities = new List<UIDBClientSettleBonusPayment>();

            IQueryable<DBClientSettleBonus> query = null;

            var whereFuncs = new List<Expression<Func<DBClientSettleBonus, bool>>>();

            whereFuncs.Add(x => x.IsNeedSettlement == true && x.DBClientBonus.IsSettled != true);

            if (uiSearchObj != null)
            {
                whereFuncs.Add(x => x.DBClientSettlementID.Equals(uiSearchObj.DBClientSettlementID));

                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.ClientUserName))
                    whereFuncs.Add(x => x.DBClientBonus != null && x.DBClientBonus.ClientUser != null
                        && x.DBClientBonus.ClientUser.ClientName.Contains(uiSearchObj.ClientUserName));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join cb in DB.DBClientBonus on q.DBClientBonusID equals cb.ID
                              join p in DB.Product on cb.ProductID equals p.ID
                              join ps in DB.ProductSpecification on cb.ProductSpecificationID equals ps.ID
                              join cu in DB.ClientUser on cb.ClientUserID equals cu.ID
                              join ba in DB.BankAccount on cu.DBBankAccountID equals ba.ID into tempBA
                              from tba in tempBA.DefaultIfEmpty()
                              select new UIDBClientSettleBonusPayment()
                              {
                                  ID = q.ID,
                                  ClientUserName = cu.ClientName,
                                  SettlementDate = cb.SettlementDate,
                                  IsNeedSettlement = q.IsNeedSettlement,
                                  TotalPayAmount = q.TotalPayAmount,
                                  IsSettled = cb.IsSettled,
                                  ClientDBBankAccount = tba != null
                                    ? (tba.AccountName + " " + tba.BankBranchName + " " + tba.Account)
                                    : string.Empty,
                              }).ToList();
            }

            return uiEntities;
        }


    }
}
