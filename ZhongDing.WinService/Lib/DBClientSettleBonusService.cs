using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;

namespace ZhongDing.WinService.Lib
{
    /// <summary>
    /// 大包客户结算提成服务
    /// </summary>
    public class DBClientSettleBonusService
    {
        public static void ProcessWork()
        {
            Utility.WriteTrace("处理大包客户结算提成，开始于:" + DateTime.Now);

            using (IUnitOfWork unitOfWork = new UnitOfWork())
            {
                var db = unitOfWork.GetDbModel();

                IDCFlowDataRepository dcFlowDataRepository = new DCFlowDataRepository();
                IDBClientSettlementRepository dbClientSettlementRepository = new DBClientSettlementRepository();
                IDBClientBonusRepository dbClientBonusRepository = new DBClientBonusRepository();
                IDBClientSettleBonusRepository dbClientSettleBonusRepository = new DBClientSettleBonusRepository();

                dcFlowDataRepository.SetDbModel(db);
                dbClientSettlementRepository.SetDbModel(db);
                dbClientBonusRepository.SetDbModel(db);
                dbClientSettleBonusRepository.SetDbModel(db);

                //基药结算数据
                List<DBClientSettlement> listBaseMDBClientSettlement = new List<DBClientSettlement>();
                //招商结算数据
                List<DBClientSettlement> listBusinessMDBClientSettlement = new List<DBClientSettlement>();
                //基药提成数据
                List<DBClientBonus> listBaseMDBClientBonus = new List<DBClientBonus>();
                //招商提成数据
                List<DBClientBonus> listBusinessMDBClientBonus = new List<DBClientBonus>();

                var lastestClientSettlementDate = dbClientSettlementRepository
                    .GetList(x => x.IsDeleted == false).OrderByDescending(x => x.SettlementDate)
                    .Select(x => x.SettlementDate).FirstOrDefault();

                //lastestClientSettlementDate == DateTime.MinValue 表示首次生成大包客户结算数据

                var dateNow = DateTime.Now;
                DateTime maxSettlementDate = new DateTime(dateNow.Year, dateNow.Month, 1);

                //处理2个月前的所有数据
                var dcFlowDatas = dcFlowDataRepository
                    .GetList(x => x.IsCorrectlyFlow == true
                        && x.SettlementDate > lastestClientSettlementDate
                        && x.SettlementDate < maxSettlementDate)
                    .OrderBy(x => x.SettlementDate).ToList();

                foreach (var dcFlowData in dcFlowDatas)
                {
                    #region 处理协议的医院类型是基药的数据

                    var baseMedicineDBClientSettlement = dbClientSettlementRepository
                        .GetList(x => x.IsDeleted == false && x.SettlementDate == dcFlowData.SettlementDate
                            && x.HospitalTypeID == (int)EHospitalType.BaseMedicine).FirstOrDefault();

                    if (baseMedicineDBClientSettlement == null
                        && !listBaseMDBClientSettlement.Any(x => x.SettlementDate == dcFlowData.SettlementDate
                            && x.HospitalTypeID == (int)EHospitalType.BaseMedicine))
                    {
                        #region 保存结算主表数据
                        baseMedicineDBClientSettlement = new DBClientSettlement()
                        {
                            SettlementDate = dcFlowData.SettlementDate,
                            HospitalTypeID = (int)EHospitalType.BaseMedicine,
                            WorkflowStatusID = (int)EWorkflowStatus.ToBeSettle,
                        };

                        dbClientSettlementRepository.Add(baseMedicineDBClientSettlement);
                        //收集基药计算数据
                        listBaseMDBClientSettlement.Add(baseMedicineDBClientSettlement);

                        #endregion

                    }

                    #region 保存提成数据
                    {
                        //从流向数据中找出需要处理提成的数据
                        var tempNeedToBonusDatas = dcFlowData.DCFlowDataDetail
                            .Where(x => x.DBContract.HospitalTypeID == (int)EHospitalType.BaseMedicine && x.DBContract.IsTempContract != true)
                            .GroupBy(x => new { x.DBContractID, x.ClientUserID, x.HospitalID, x.DBContract.PromotionExpense })
                            .Select(g => new { g.Key, SaleQty = g.Sum(x => x.SaleQty) })
                            .ToList();

                        //合并同一个协议的数据
                        var needToBonusDatas = tempNeedToBonusDatas
                            .GroupBy(x => new { x.Key.DBContractID, x.Key.ClientUserID, x.Key.PromotionExpense })
                            .Select(g => new { g.Key, TotalSaleQty = g.Sum(x => x.SaleQty) })
                            .ToList();

                        foreach (var settleData in needToBonusDatas)
                        {
                            //var dbClientBonus = dbClientBonusRepository
                            //    .GetList(x => x.DBContractID == settleData.Key.DBContractID
                            //        && x.ClientUserID == settleData.Key.ClientUserID
                            //        && x.ProductID == dcFlowData.ProductID
                            //        && x.ProductSpecificationID == dcFlowData.ProductSpecificationID
                            //        && x.SettlementDate == dcFlowData.SettlementDate)
                            //    .FirstOrDefault();

                            var dbClientBonus = new DBClientBonus()
                            {
                                DBContractID = settleData.Key.DBContractID,
                                ClientUserID = settleData.Key.ClientUserID,
                                ProductID = dcFlowData.ProductID,
                                ProductSpecificationID = dcFlowData.ProductSpecificationID,
                                SettlementDate = dcFlowData.SettlementDate,
                                PromotionExpense = settleData.Key.PromotionExpense,
                                SaleQty = settleData.TotalSaleQty,
                                BonusAmount = (settleData.Key.PromotionExpense ?? 0M) * settleData.TotalSaleQty,
                                PerformanceAmount = 0,//暂时用0代替
                            };

                            #region 保存相关医院
                            var hospitals = tempNeedToBonusDatas
                                .Where(x => x.Key.DBContractID == dbClientBonus.DBContractID
                                    && x.Key.ClientUserID == dbClientBonus.ClientUserID).ToList();

                            foreach (var hospital in hospitals)
                            {
                                var dbClientBonusHospital = new DBClientBonusHospital()
                                {
                                    HospitalID = hospital.Key.HospitalID
                                };

                                dbClientBonus.DBClientBonusHospital.Add(dbClientBonusHospital);
                            }

                            #endregion

                            dbClientBonusRepository.Add(dbClientBonus);
                            //收集基药提成数据
                            listBaseMDBClientBonus.Add(dbClientBonus);
                        }
                    }

                    #endregion


                    #endregion

                    #region 处理协议的医院类型是招商的数据

                    var businessMedicineDBClientSettlement = dbClientSettlementRepository
                        .GetList(x => x.IsDeleted == false && x.SettlementDate == dcFlowData.SettlementDate
                            && x.HospitalTypeID == (int)EHospitalType.BusinessMedicine).FirstOrDefault();

                    if (businessMedicineDBClientSettlement == null
                        && !listBusinessMDBClientSettlement.Any(x => x.SettlementDate == dcFlowData.SettlementDate
                            && x.HospitalTypeID == (int)EHospitalType.BusinessMedicine))
                    {
                        businessMedicineDBClientSettlement = new DBClientSettlement()
                        {
                            SettlementDate = dcFlowData.SettlementDate,
                            HospitalTypeID = (int)EHospitalType.BusinessMedicine,
                            WorkflowStatusID = (int)EWorkflowStatus.ToBeSettle,
                        };

                        dbClientSettlementRepository.Add(businessMedicineDBClientSettlement);

                        listBusinessMDBClientSettlement.Add(businessMedicineDBClientSettlement);

                    }

                    #region 保存提成数据
                    {
                        //从流向数据中找出需要处理提成的数据
                        var tempNeedToBonusDatas = dcFlowData.DCFlowDataDetail
                            .Where(x => x.DBContract.HospitalTypeID == (int)EHospitalType.BusinessMedicine && x.DBContract.IsTempContract != true)
                            .GroupBy(x => new { x.DBContractID, x.ClientUserID, x.HospitalID, x.DBContract.PromotionExpense })
                            .Select(g => new { g.Key, SaleQty = g.Sum(x => x.SaleQty) })
                            .ToList();

                        //合并同一个协议的数据
                        var needToBonusDatas = tempNeedToBonusDatas
                            .GroupBy(x => new { x.Key.DBContractID, x.Key.ClientUserID, x.Key.PromotionExpense })
                            .Select(g => new { g.Key, TotalSaleQty = g.Sum(x => x.SaleQty) })
                            .ToList();

                        foreach (var settleData in needToBonusDatas)
                        {
                            var dbClientBonus = new DBClientBonus()
                            {
                                DBContractID = settleData.Key.DBContractID,
                                ClientUserID = settleData.Key.ClientUserID,
                                ProductID = dcFlowData.ProductID,
                                ProductSpecificationID = dcFlowData.ProductSpecificationID,
                                SettlementDate = dcFlowData.SettlementDate,
                                PromotionExpense = settleData.Key.PromotionExpense,
                                SaleQty = settleData.TotalSaleQty,
                                BonusAmount = (settleData.Key.PromotionExpense ?? 0M) * settleData.TotalSaleQty,
                                PerformanceAmount = 0,//暂时用0代替
                            };

                            #region 保存相关医院
                            var hospitals = tempNeedToBonusDatas
                                .Where(x => x.Key.DBContractID == dbClientBonus.DBContractID
                                    && x.Key.ClientUserID == dbClientBonus.ClientUserID).ToList();

                            foreach (var hospital in hospitals)
                            {
                                var dbClientBonusHospital = new DBClientBonusHospital()
                                {
                                    HospitalID = hospital.Key.HospitalID
                                };

                                dbClientBonus.DBClientBonusHospital.Add(dbClientBonusHospital);
                            }

                            #endregion

                            dbClientBonusRepository.Add(dbClientBonus);
                            //收集招商提成数据
                            listBusinessMDBClientBonus.Add(dbClientBonus);
                        }
                    }

                    #endregion

                    #endregion
                }

                var tempMaxSettlementDate = maxSettlementDate.AddMonths(-1);

                #region 保存基药提成结算交叉数据

                //加入当前结算期前未结算的数据
                var otherBaseMDBClientBonus = dbClientBonusRepository
                    .GetList(x => x.IsSettled != true && x.DBContract.HospitalTypeID == (int)EHospitalType.BaseMedicine
                        && x.SettlementDate < tempMaxSettlementDate)
                    .ToList();

                listBaseMDBClientBonus.AddRange(otherBaseMDBClientBonus);

                foreach (var clientSettlement in listBaseMDBClientSettlement)
                {
                    var tempSettlementDate = clientSettlement.SettlementDate;

                    foreach (var clientBonus in listBaseMDBClientBonus
                        .Where(x => x.SettlementDate >= tempSettlementDate.AddMonths(-2)
                            && x.SettlementDate < tempSettlementDate)
                        .OrderBy(x => x.SettlementDate))
                    {
                        var dbClientSettleBonus = new DBClientSettleBonus()
                        {
                            DBClientSettlement = clientSettlement,
                            DBClientBonus = clientBonus
                        };

                        if (clientBonus.SettlementDate == tempSettlementDate.AddMonths(-1))
                        {
                            bool isNeedSettlement = listBaseMDBClientBonus
                                .Any(x => x.SettlementDate == clientBonus.SettlementDate.AddMonths(1)
                                    && x.ProductID == clientBonus.ProductID
                                    && x.ProductSpecificationID == clientBonus.ProductSpecificationID
                                    && x.ClientUserID == clientBonus.ClientUserID
                                    && x.DBContractID == clientBonus.DBContractID
                                    && x.SaleQty > 0);

                            dbClientSettleBonus.IsNeedSettlement = isNeedSettlement;

                            //需结算的才计算应支付金额
                            if (isNeedSettlement)
                                dbClientSettleBonus.TotalPayAmount = clientBonus.BonusAmount + clientBonus.PerformanceAmount;//绩效有加减
                        }
                        else
                            dbClientSettleBonus.IsNeedSettlement = false;


                        dbClientSettleBonusRepository.Add(dbClientSettleBonus);
                    }
                }

                #endregion

                #region 保存招商提成结算交叉数据

                //加入当前结算期前未结算的数据
                var otherBusinessMDBClientBonus = dbClientBonusRepository
                    .GetList(x => x.IsSettled != true && x.DBContract.HospitalTypeID == (int)EHospitalType.BusinessMedicine
                        && x.SettlementDate < tempMaxSettlementDate)
                    .ToList();

                listBusinessMDBClientBonus.AddRange(otherBusinessMDBClientBonus);

                foreach (var clientSettlement in listBusinessMDBClientSettlement)
                {
                    var tempSettlementDate = clientSettlement.SettlementDate;

                    //var tempBusinessMDBClientBonusList = listBusinessMDBClientBonus
                    //    .Where(x => x.SettlementDate < tempSettlementDate)
                    //    .OrderBy(x => x.SettlementDate);

                    //var curNeedSettlementDate = tempSettlementDate.AddMonths(-2);

                    foreach (var clientBonus in listBusinessMDBClientBonus
                        .Where(x => x.SettlementDate >= tempSettlementDate.AddMonths(-2)
                            && x.SettlementDate < tempSettlementDate)
                        .OrderBy(x => x.SettlementDate))
                    {
                        var dbClientSettleBonus = new DBClientSettleBonus()
                        {
                            DBClientSettlement = clientSettlement,
                            DBClientBonus = clientBonus
                        };

                        if (clientBonus.SettlementDate == tempSettlementDate.AddMonths(-1))
                        {
                            bool isNeedSettlement = listBusinessMDBClientBonus
                                .Any(x => x.SettlementDate == clientBonus.SettlementDate.AddMonths(1)
                                    && x.ProductID == clientBonus.ProductID
                                    && x.ProductSpecificationID == clientBonus.ProductSpecificationID
                                    && x.ClientUserID == clientBonus.ClientUserID
                                    && x.DBContractID == clientBonus.DBContractID
                                    && x.SaleQty > 0);

                            dbClientSettleBonus.IsNeedSettlement = isNeedSettlement;

                            //需结算的才计算应支付金额
                            if (isNeedSettlement)
                                dbClientSettleBonus.TotalPayAmount = clientBonus.BonusAmount + clientBonus.PerformanceAmount;//绩效有加减
                        }
                        else
                            dbClientSettleBonus.IsNeedSettlement = false;

                        dbClientSettleBonusRepository.Add(dbClientSettleBonus);
                    }
                }

                #endregion

                unitOfWork.SaveChanges();
            }

            Utility.WriteTrace("处理大包客户结算提成，结束于:" + DateTime.Now);
        }
    }
}
