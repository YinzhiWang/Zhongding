using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Common.Extension;
using ZhongDing.Common;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories.Reminders
{
    public class ReminderRepository : IReminderRepository
    {
        #region Members

        private DbModelContainer db = null;
        protected internal DbModelContainer DB
        {
            get
            {
                if (db == null)
                    db = new DbModelContainer();

                return db;
            }
        }

        #endregion

        #region 构造函数



        static ReminderRepository()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{TEntity}"/> class.
        /// </summary>
        public ReminderRepository()
        {
            db = new DbModelContainer();
        }

        #endregion

        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        #region GetUIInventoryReminder

        public int GetUIInventoryReminderCount(Domain.UISearchObjects.UISearchInventoryReminder uiSearchObj)
        {
            int totalRecords = 0;
            var entities = GetUIInventoryReminder(uiSearchObj, 0, 100000000, out totalRecords);
            return totalRecords;
        }
        public IList<Domain.UIObjects.UIInventoryReminder> GetUIInventoryReminder(Domain.UISearchObjects.UISearchInventoryReminder uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UIInventoryReminder> inventoryHistories = new List<UIInventoryReminder>();

            var lastInventoryHistory = DB.InventoryHistory.OrderByDescending(x => x.ID).FirstOrDefault();

            DateTime? lastStatDate = null;
            if (lastInventoryHistory != null)
                lastStatDate = lastInventoryHistory.StatDate;
            DateTime? beginDate = lastStatDate.HasValue ? lastStatDate.Value.AddDays(1) : lastStatDate;
            DateTime endDate = DateTime.Now;
            IQueryable<UIInventoryReminder> query = null;

            query = (from sid in DB.StockInDetail
                     join si in DB.StockIn on sid.StockInID equals si.ID
                     join product in DB.Product on sid.ProductID equals product.ID
                     where si.IsDeleted == false && si.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse
                     && sid.IsDeleted == false
                     && product.SafetyStock > 0
                     && product.CompanyID == uiSearchObj.CompanyID
                     select new
                     {
                         sid.WarehouseID,
                         sid.ProductID,
                         sid.ProductSpecificationID,
                         //sid.BatchNumber,
                         //sid.LicenseNumber,
                         //sid.ExpirationDate,
                         //sid.ProcurePrice,
                         sid.InQty
                     })
                        .GroupBy(x => new { x.WarehouseID, x.ProductID, x.ProductSpecificationID })
                        .Select(x => new UIInventoryReminder
                        {
                            WarehouseID = x.Key.WarehouseID,
                            ProductID = x.Key.ProductID,
                            ProductSpecificationID = x.Key.ProductSpecificationID,
                            //BatchNumber = x.Key.BatchNumber,
                            //LicenseNumber = x.Key.LicenseNumber,
                            //ExpirationDate = x.Key.ExpirationDate,
                            //ProcurePrice = DB.StockInDetail.Any(y => y.WarehouseID == x.Key.WarehouseID
                            //    && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                            //    && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                            //    && y.ExpirationDate == x.Key.ExpirationDate)
                            //    ? DB.StockInDetail.FirstOrDefault(y => y.WarehouseID == x.Key.WarehouseID
                            //    && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                            //    && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                            //    && y.ExpirationDate == x.Key.ExpirationDate).ProcurePrice : 0,
                            InQty = (DB.StockInDetail.Any(y => y.IsDeleted == false && y.WarehouseID == x.Key.WarehouseID
                                    && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                    && (beginDate == null || y.StockIn.EntryDate >= beginDate) && y.StockIn.EntryDate < endDate
                                    && y.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse)
                                    ?
                                    DB.StockInDetail.Where(y => y.IsDeleted == false && y.WarehouseID == x.Key.WarehouseID
                                    && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                    && (beginDate == null || y.StockIn.EntryDate >= beginDate) && y.StockIn.EntryDate < endDate
                                    && y.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse).Sum(y => y.InQty) : 0),
                            OutQty = (DB.StockOutDetail.Any(y => y.IsDeleted == false && y.WarehouseID == x.Key.WarehouseID
                                    && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                    && y.StockOut.WorkflowStatusID == (int)EWorkflowStatus.OutWarehouse &&
                                    (beginDate == null || y.StockOut.OutDate >= beginDate) && y.StockOut.OutDate < endDate)
                                       ?
                                    DB.StockOutDetail.Where(y => y.IsDeleted == false && y.WarehouseID == x.Key.WarehouseID
                                        && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                        && (((beginDate == null || y.StockOut.OutDate >= beginDate) && y.StockOut.OutDate < endDate)
                                              )).Sum(y => y.OutQty) : 0),
                            WarehouseName = null,
                            ProductName = null,
                            ProductCode = null,
                            Specification = null,
                            UnitName = null,
                            NumberInLargePackage = 0,

                        });


            var queryEx = from q in query
                          join warehouse in DB.Warehouse on q.WarehouseID equals warehouse.ID
                          join product in DB.Product on q.ProductID equals product.ID
                          join productSpecification in DB.ProductSpecification on q.ProductSpecificationID equals productSpecification.ID
                          join unitName in DB.UnitOfMeasurement on productSpecification.UnitOfMeasurementID equals unitName.ID
                          select new UIInventoryReminder()
                          {
                              WarehouseID = q.WarehouseID,
                              ProductID = q.ProductID,
                              ProductSpecificationID = q.ProductSpecificationID,
                              //BatchNumber = q.BatchNumber,
                              //LicenseNumber = q.LicenseNumber,
                              //ExpirationDate = q.ExpirationDate,
                              //ProcurePrice = q.ProcurePrice,
                              InQty = q.InQty,
                              OutQty = q.OutQty,
                              WarehouseName = warehouse.Name,
                              ProductName = product.ProductName,
                              ProductCode = product.ProductCode,
                              Specification = productSpecification.Specification,
                              UnitName = unitName.UnitName,
                              NumberInLargePackage = productSpecification.NumberInLargePackage.HasValue ? productSpecification.NumberInLargePackage.Value : 1,
                          };
            //if (uiSearchObj.WarehouseName.IsNotNullOrEmpty())
            //{
            //    queryEx = queryEx.Where(x => x.WarehouseName.Contains(uiSearchObj.WarehouseName));
            //}
            //if (uiSearchObj.ProductName.IsNotNullOrEmpty())
            //{
            //    queryEx = queryEx.Where(x => x.ProductName.Contains(uiSearchObj.ProductName));
            //}
            //totalRecords = query.Count();
            //由于这里的计算 先计算最后一次Service跑完 到现在的库存变化，然后取出Service所计算出来的库存，两者合并得来的，金内存后才合并的，因此如果要在此过滤>0的 
            //没有办法在分页了，分页也是徒劳，因为数据已经在内存了。因此这里直接取出所有数据吧
            //query = query.OrderBy(x => x.WarehouseID).Skip(pageSize * pageIndex).Take(pageSize);
            inventoryHistories = queryEx.ToList();
            if (lastInventoryHistory != null)
            {
                inventoryHistories.ForEach(m =>
                {

                    int balanceQty = m.InQty - m.OutQty;
                    var oldInventoryHistorys = DB.InventoryHistory.Where(x => x.StatDate == lastStatDate
                          && x.WarehouseID == m.WarehouseID && x.ProductID == m.ProductID && x.ProductSpecificationID == m.ProductSpecificationID).Select(x => x.BalanceQty).ToList();
                    if (oldInventoryHistorys != null && oldInventoryHistorys.Count > 0)
                    {

                        m.BalanceQty = oldInventoryHistorys.Sum();
                        m.BalanceQty += balanceQty;
                    }
                    else
                    {
                        m.BalanceQty += balanceQty;
                    }
                    m.Amount = m.BalanceQty * m.ProcurePrice;
                    m.NumberOfPackages = m.BalanceQty.ToDecimal() / m.NumberInLargePackage.ToDecimal();
                });
            }
            //inventoryHistories = inventoryHistories.Where(x => x.BalanceQty > 0).ToList();

            var tempInventoryHistories = new List<UIInventoryReminder>();
            var noneInventoryHistories = new List<UIInventoryReminder>();
            var products = DB.Product.Where(x => x.IsDeleted == false && x.SafetyStock > 0 && x.CompanyID == uiSearchObj.CompanyID).ToList();
            products.ForEach(x =>
            {
                int qty = inventoryHistories.Where(m => m.ProductID == x.ID).Any() ? inventoryHistories.Where(m => m.ProductID == x.ID).Sum(m => m.BalanceQty) : 0;
                if (qty < x.SafetyStock.Value)
                {
                    tempInventoryHistories.AddRange(inventoryHistories.Where(m => m.ProductID == x.ID));
                }
                if (inventoryHistories.Any(m => m.ProductID == x.ID) == false)
                {
                    if (noneInventoryHistories.Any(m => m.ProductID == x.ID) == false)
                    {
                        noneInventoryHistories.Add(new UIInventoryReminder()
                        {
                            ProductID = x.ID,
                            ProductName = x.ProductName,
                            BalanceQty = 0,
                            ProductCode = x.ProductCode,
                            WarehouseName = "*",
                            Specification = "*",
                            UnitName = "*"
                        });
                    }
                }
            });
            tempInventoryHistories.AddRange(noneInventoryHistories);
            totalRecords = tempInventoryHistories.Count;
            tempInventoryHistories = tempInventoryHistories.OrderBy(x => x.WarehouseID).Skip(pageSize * pageIndex).Take(pageSize).ToList();
            return tempInventoryHistories;
        }
        #endregion
        #region GetUICautionMoneyReminder
        public int GetUICautionMoneyReminderCount(Domain.UISearchObjects.UISearchCautionMoneyReminder uiSearchObj)
        {
            int totalRecords = 0;
            var entities = GetUICautionMoneyReminder(uiSearchObj, 0, 100000000, out totalRecords);
            return totalRecords;
        }
        public IList<UICautionMoneyReminder> GetUICautionMoneyReminder(Domain.UISearchObjects.UISearchCautionMoneyReminder uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UICautionMoneyReminder> uiEntities = new List<UICautionMoneyReminder>();


            var queryClient = from q in DB.ClientCautionMoney
                              join cautionMoneyType in DB.CautionMoneyType on q.CautionMoneyTypeID equals cautionMoneyType.ID
                              join product in DB.Product on q.ProductID equals product.ID
                              join productSpecification in DB.ProductSpecification on q.ProductSpecificationID equals productSpecification.ID
                              join clientUser in DB.ClientUser on q.ClientUserID equals clientUser.ID
                              where q.IsDeleted == false
                              select new UICautionMoneyReminder()
                              {
                                  ID = q.ID,
                                  Type = "客户",
                                  CautionMoneyTypeName = cautionMoneyType.Name,
                                  EndDate = q.EndDate,
                                  PaymentCautionMoney = DB.ApplicationPayment.Any(x =>
                                      x.ApplicationID == q.ID &&
                                      x.WorkflowID == (int)EWorkflow.ClientCautionMoney &&
                                      x.PaymentTypeID == (int)EPaymentType.Income &&
                                      x.PaymentStatusID == (int)EPaymentStatus.Paid &&
                                      x.IsDeleted == false) ?
                                      DB.ApplicationPayment.Where(x =>
                                      x.ApplicationID == q.ID &&
                                      x.WorkflowID == (int)EWorkflow.ClientCautionMoney &&
                                      x.PaymentTypeID == (int)EPaymentType.Income &&
                                      x.PaymentStatusID == (int)EPaymentStatus.Paid &&
                                      x.IsDeleted == false).Sum(x => x.Amount) : 0,
                                  ProductName = product.ProductName,
                                  ClientOrSupplierName = clientUser.ClientName,
                                  ReturnCautionMoney = (from ccra in DB.ClientCautionMoneyReturnApplication.Where(x => x.ClientCautionMoneyID == q.ID)
                                                        join ap in DB.ApplicationPayment.Where(x =>
                                                        x.WorkflowID == (int)EWorkflow.ClientCautionMoneyReturnApply &&
                                                        x.PaymentTypeID == (int)EPaymentType.Expend &&
                                                        x.PaymentStatusID == (int)EPaymentStatus.Paid &&
                                                        x.IsDeleted == false) on ccra.ID equals ap.ApplicationID
                                                        select ap.Amount).Any() ?
                                                        (from ccra in DB.ClientCautionMoneyReturnApplication.Where(x => x.ClientCautionMoneyID == q.ID)
                                                         join ap in DB.ApplicationPayment.Where(x =>
                                                         x.WorkflowID == (int)EWorkflow.ClientCautionMoneyReturnApply &&
                                                         x.PaymentTypeID == (int)EPaymentType.Expend &&
                                                         x.PaymentStatusID == (int)EPaymentStatus.Paid &&
                                                         x.IsDeleted == false) on ccra.ID equals ap.ApplicationID
                                                         select ap.Amount).Sum() : 0,
                                  RefundedAmount = 0,
                                  DeductedAmount = 0
                              };

            DateTime timelineDate = DateTime.Now.AddDays(GlobalConst.Reminder.AheadDaysOfDefault);
            queryClient = queryClient.Where(x => x.ReturnCautionMoney < x.PaymentCautionMoney && timelineDate >= x.EndDate);



            var querySupplier = from q in DB.SupplierCautionMoney
                                join cautionMoneyType in DB.CautionMoneyType on q.CautionMoneyTypeID equals cautionMoneyType.ID
                                join supplier in DB.Supplier on q.SupplierID equals supplier.ID
                                join product in DB.Product on q.ProductID equals product.ID
                                where q.IsDeleted == false
                                select new UICautionMoneyReminder()
                                {
                                    ID = q.ID,
                                    Type = "供应商",
                                    CautionMoneyTypeName = cautionMoneyType.Name,
                                    EndDate = q.EndDate,
                                    PaymentCautionMoney = q.PaymentCautionMoney,
                                    ProductName = product.ProductName,
                                    ClientOrSupplierName = supplier.SupplierName,
                                    ReturnCautionMoney = 0,
                                    //已返款总额
                                    RefundedAmount = (from sra in DB.ApplicationPayment.Where(x => x.IsDeleted == false && x.WorkflowID == (int)EWorkflow.SupplierCautionMoneyApply
                                                          && x.ApplicationID == q.ID && x.PaymentTypeID == (int)EPaymentType.Income)
                                                      select new { sra.Amount, sra.Fee }).Any() ? (from sra in DB.ApplicationPayment.Where(x => x.IsDeleted == false && x.WorkflowID == (int)EWorkflow.SupplierCautionMoneyApply
                                                            && x.ApplicationID == q.ID && x.PaymentTypeID == (int)EPaymentType.Income)
                                                                                                   select new { sra.Amount, sra.Fee })
                                                           .Sum(x => (x.Amount.HasValue ? x.Amount.Value : 0)
                                                               + (x.Fee.HasValue ? x.Fee.Value : 0)) : 0,
                                    //已抵扣总额
                                    DeductedAmount = (from supplierCautionMoneyDeduction in DB.SupplierCautionMoneyDeduction.Where(x => x.IsDeleted == false && x.SupplierCautionMoneyID == q.ID)
                                                      select new { supplierCautionMoneyDeduction.Amount }).Any() ? (from supplierCautionMoneyDeduction in DB.SupplierCautionMoneyDeduction.Where(x => x.IsDeleted == false && x.SupplierCautionMoneyID == q.ID)
                                                                                                                    select new { supplierCautionMoneyDeduction.Amount })
                                                                                                                    .Sum(x => x.Amount) : 0

                                };
            querySupplier = querySupplier.Where(x => (x.RefundedAmount + x.DeductedAmount) < x.PaymentCautionMoney && timelineDate >= x.EndDate);

            var queryResult = queryClient.Union(querySupplier);

            totalRecords = queryResult.Count();
            uiEntities = queryResult.OrderBy(x => x.EndDate).Skip(pageSize * pageIndex).Take(pageSize).ToList();

            return uiEntities;
        }
        #endregion
        #region GetUIProductInfoExpiredReminder
        public int GetUIProductInfoExpiredReminderCount(Domain.UISearchObjects.UISearchProductInfoExpiredReminder uiSearchObj)
        {
            int totalRecords = 0;
            var entities = GetUIProductInfoExpiredReminder(uiSearchObj, 0, 100000000, out totalRecords);
            return totalRecords;
        }
        public IList<UIProductInfoExpiredReminder> GetUIProductInfoExpiredReminder(Domain.UISearchObjects.UISearchProductInfoExpiredReminder uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UIProductInfoExpiredReminder> uiEntities = new List<UIProductInfoExpiredReminder>();
            DateTime timelineDate = DateTime.Now.AddDays(GlobalConst.Reminder.AheadDaysOfDefault);
            var query = from product in DB.Product
                        join productCertificate in DB.ProductCertificate on product.ID equals productCertificate.ProductID
                        join certificate in DB.Certificate on productCertificate.CertificateID equals certificate.ID
                        join certificateType in DB.CertificateType on certificate.CertificateTypeID equals certificateType.ID
                        where product.IsDeleted == false
                        && product.CompanyID == uiSearchObj.CompanyID
                        && certificate.IsNeedAlert == true
                        && productCertificate.IsDeleted == false
                        && certificate.IsDeleted == false
                        select new UIProductInfoExpiredReminder()
                        {
                            ProductID = product.ID,
                            ProductCode = product.ProductCode,
                            ProductName = product.ProductName,
                            CertificateType = certificateType.CertificateType1,
                            EffectiveTo = certificate.EffectiveTo,
                            AlertBeforeDays = certificate.AlertBeforeDays
                        };
            uiEntities = query.ToList();
            var tempUIEntities = new List<UIProductInfoExpiredReminder>();
            uiEntities.ForEach(x =>
            {
                if ((x.EffectiveTo.Value - DateTime.Now.Date).Days <= x.AlertBeforeDays.Value)
                {
                    tempUIEntities.Add(x);
                }
            });
            totalRecords = tempUIEntities.Count();
            tempUIEntities = tempUIEntities.OrderBy(x => x.ProductID).Skip(pageSize * pageIndex).Take(pageSize).ToList();
            return tempUIEntities;
        }
        #endregion
        #region GetUIClientInfoReminder
        public int GetUIClientInfoReminderCount(Domain.UISearchObjects.UISearchClientInfoReminder uiSearchObj)
        {
            int totalRecords = 0;
            var entities = GetUIClientInfoReminder(uiSearchObj, 0, 100000000, out totalRecords);
            return totalRecords;
        }
        public IList<UIClientInfoReminder> GetUIClientInfoReminder(Domain.UISearchObjects.UISearchClientInfoReminder uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UIClientInfoReminder> uiEntities = new List<UIClientInfoReminder>();
            DateTime timelineDate = DateTime.Now.AddDays(GlobalConst.Reminder.AheadDaysOfDefault);
            var query = from clientInfo in DB.ClientInfo
                        join clientCompany in DB.ClientCompany on clientInfo.ClientCompanyID equals clientCompany.ID
                        join clientCompanyCertificate in DB.ClientCompanyCertificate on clientCompany.ID equals clientCompanyCertificate.ClientCompanyID
                        join certificate in DB.Certificate on clientCompanyCertificate.CertificateID equals certificate.ID
                        join certificateType in DB.CertificateType on certificate.CertificateTypeID equals certificateType.ID
                        where clientInfo.IsDeleted == false
                        && clientCompanyCertificate.IsDeleted == false
                        && certificate.IsDeleted == false
                        && certificate.IsNeedAlert == true
                        select new UIClientInfoReminder()
                        {
                            ClientInfoID = clientInfo.ID,
                            ClientCode = clientInfo.ClientCode,
                            ClientName = clientInfo.ClientUser.ClientName,
                            CertificateType = certificateType.CertificateType1,
                            EffectiveTo = certificate.EffectiveTo,
                            AlertBeforeDays = certificate.AlertBeforeDays,
                            ClientCompanyName = clientCompany.Name,
                            ClientCompanyID = clientCompany.ID
                        };
            uiEntities = query.ToList();
            var tempUIEntities = new List<UIClientInfoReminder>();
            uiEntities.ForEach(x =>
            {
                if ((x.EffectiveTo.Value - DateTime.Now.Date).Days <= x.AlertBeforeDays.Value)
                {
                    tempUIEntities.Add(x);
                }
            });
            totalRecords = tempUIEntities.Count();
            tempUIEntities = tempUIEntities.OrderBy(x => x.ClientInfoID).Skip(pageSize * pageIndex).Take(pageSize).ToList();
            return tempUIEntities;
        }
        #endregion

        #region GetUISupplierInfoReminder
        public int GetUISupplierInfoReminderCount(Domain.UISearchObjects.UISearchSupplierInfoReminder uiSearchObj)
        {
            int totalRecords = 0;
            var entities = GetUISupplierInfoReminder(uiSearchObj, 0, 100000000, out totalRecords);
            return totalRecords;
        }
        public IList<UISupplierInfoReminder> GetUISupplierInfoReminder(Domain.UISearchObjects.UISearchSupplierInfoReminder uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UISupplierInfoReminder> uiEntities = new List<UISupplierInfoReminder>();
            DateTime timelineDate = DateTime.Now.AddDays(GlobalConst.Reminder.AheadDaysOfDefault);
            var query = from supplier in DB.Supplier
                        join supplierCertificate in DB.SupplierCertificate on supplier.ID equals supplierCertificate.SupplierID
                        join certificate in DB.Certificate on supplierCertificate.CertificateID equals certificate.ID
                        join certificateType in DB.CertificateType on certificate.CertificateTypeID equals certificateType.ID
                        where supplier.IsDeleted == false
                        && supplierCertificate.IsDeleted == false
                        && certificate.IsDeleted == false
                        && certificate.IsNeedAlert == true
                        select new UISupplierInfoReminder()
                        {
                            SupplierID = supplier.ID,
                            SupplierCode = supplier.SupplierCode,
                            SupplierName = supplier.SupplierName,
                            CertificateType = certificateType.CertificateType1,
                            EffectiveTo = certificate.EffectiveTo,
                            AlertBeforeDays = certificate.AlertBeforeDays,
                        };
            uiEntities = query.ToList();
            var tempUIEntities = new List<UISupplierInfoReminder>();
            uiEntities.ForEach(x =>
            {
                if ((x.EffectiveTo.Value - DateTime.Now.Date).Days <= x.AlertBeforeDays.Value)
                {
                    tempUIEntities.Add(x);
                }
            });
            totalRecords = tempUIEntities.Count();
            tempUIEntities = tempUIEntities.OrderBy(x => x.SupplierID).Skip(pageSize * pageIndex).Take(pageSize).ToList();
            return tempUIEntities;
        }
        #endregion

        #region GetUIBorrowMoneyExpiredReminder
        public int GetUIBorrowMoneyExpiredReminderCount(Domain.UISearchObjects.UISearchBorrowMoneyExpiredReminder uiSearchObj)
        {
            int totalRecords = 0;
            var entities = GetUIBorrowMoneyExpiredReminder(uiSearchObj, 0, 100000000, out totalRecords);
            return totalRecords;
        }
        public IList<UIBorrowMoneyExpiredReminder> GetUIBorrowMoneyExpiredReminder(Domain.UISearchObjects.UISearchBorrowMoneyExpiredReminder uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UIBorrowMoneyExpiredReminder> uiEntities = new List<UIBorrowMoneyExpiredReminder>();
            DateTime timelineDate = DateTime.Now.AddDays(GlobalConst.Reminder.AheadDaysOfDefault);
            var query = from q in DB.BorrowMoney
                        select new UIBorrowMoneyExpiredReminder()
                        {
                            ID = q.ID,
                            BorrowAmount = q.BorrowAmount,
                            BorrowDate = q.BorrowDate,
                            BorrowName = q.BorrowName,
                            Comment = q.Comment,
                            ReturnDate = q.ReturnDate,
                            Status = q.Status,
                            ReturnAmount = DB.ApplicationPayment.Where(x => x.IsDeleted == false && x.WorkflowID == (int)EWorkflow.BorrowMoneyManagement
                                           && x.ApplicationID == q.ID && x.PaymentStatusID == (int)EPaymentStatus.Paid
                                           && x.PaymentTypeID == (int)EPaymentType.Income).Any() ?
                                           DB.ApplicationPayment.Where(x => x.IsDeleted == false && x.WorkflowID == (int)EWorkflow.BorrowMoneyManagement
                                           && x.ApplicationID == q.ID && x.PaymentStatusID == (int)EPaymentStatus.Paid
                                           && x.PaymentTypeID == (int)EPaymentType.Income).Sum(x => x.Amount) : 0M

                        };
            query = query.Where(x => x.ReturnAmount < x.BorrowAmount && timelineDate >= x.ReturnDate);

            totalRecords = query.Count();
            uiEntities = query.OrderBy(x => x.ID).Skip(pageSize * pageIndex).Take(pageSize).ToList();
            return uiEntities;
        }
        #endregion
        #region GetUIGuaranteeReceiptExpiredReminder
        public int GetUIGuaranteeReceiptExpiredReminderCount(Domain.UISearchObjects.UISearchGuaranteeReceiptExpiredReminder uiSearchObj)
        {
            int totalRecords = 0;
            var entities = GetUIGuaranteeReceiptExpiredReminder(uiSearchObj, 0, 100000000, out totalRecords);
            return totalRecords;
        }
        public IList<UIGuaranteeReceiptExpiredReminder> GetUIGuaranteeReceiptExpiredReminder(Domain.UISearchObjects.UISearchGuaranteeReceiptExpiredReminder uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIGuaranteeReceiptExpiredReminder> uiEntities = new List<UIGuaranteeReceiptExpiredReminder>();

            DateTime timelineDate = DateTime.Now.AddDays(GlobalConst.Reminder.AheadDaysOfDefault);

            var query = from q in DB.ClientSaleApplication
                        join soa in DB.SalesOrderApplication on q.SalesOrderApplicationID equals soa.ID
                        join sot in DB.SaleOrderType on soa.SaleOrderTypeID equals sot.ID
                        join cu in DB.ClientUser on q.ClientUserID equals cu.ID
                        join cc in DB.ClientCompany on q.ClientCompanyID equals cc.ID
                        join user in DB.Users on q.Guaranteeby equals user.UserID into tempCBU
                        from tUser in tempCBU.DefaultIfEmpty()
                        where q.IsDeleted == false
                        && q.IsGuaranteed == true
                        && uiSearchObj.IncludeWorkflowStatusIDs.Contains(q.WorkflowStatusID)
                        select new UIGuaranteeReceiptExpiredReminder()
                        {
                            ID = q.ID,
                            OrderCode = soa.OrderCode,
                            OrderDate = soa.OrderDate,
                            SaleOrderTypeName = sot.TypeName,
                            ClientUserName = cu.ClientName,
                            ClientCompanyName = cc.Name,
                            IsGuaranteed = q.IsGuaranteed,
                            IsReceiptedGuaranteeAmount = q.GuaranteeLog.Any(x => x.IsDeleted == false && x.IsReceipted == true),
                            GuaranteeExpirationDate = q.GuaranteeExpirationDate,
                            GuaranteeAmount = soa.SalesOrderAppDetail.Where(x => x.IsDeleted == false).Any() ?
                            soa.SalesOrderAppDetail.Where(x => x.IsDeleted == false).Sum(x => x.TotalSalesAmount) : 0M,
                            GuaranteebyFullName = tUser != null ? tUser.FullName : string.Empty

                        };

            query = query.Where(x => x.IsReceiptedGuaranteeAmount == false && timelineDate >= x.GuaranteeExpirationDate);

            totalRecords = query.Count();
            uiEntities = query.OrderBy(x => x.ID).Skip(pageSize * pageIndex).Take(pageSize).ToList();
            return uiEntities;
        }
        #endregion


        #region GetUIProductExpiredReminder
        public int GetUIProductExpiredReminderCount(Domain.UISearchObjects.UISearchProductExpiredReminder uiSearchObj)
        {
            int totalRecords = 0;
            var entities = GetUIProductExpiredReminder(uiSearchObj, 0, 100000000, out totalRecords);
            return totalRecords;
        }
        public IList<UIProductExpiredReminder> GetUIProductExpiredReminder(Domain.UISearchObjects.UISearchProductExpiredReminder uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {

            List<UIProductExpiredReminder> inventoryHistories = new List<UIProductExpiredReminder>();

            var lastInventoryHistory = DB.InventoryHistory.OrderByDescending(x => x.ID).FirstOrDefault();

            DateTime? lastStatDate = null;
            if (lastInventoryHistory != null)
                lastStatDate = lastInventoryHistory.StatDate;
            DateTime? beginDate = lastStatDate.HasValue ? lastStatDate.Value.AddDays(1) : lastStatDate;
            DateTime endDate = DateTime.Now;
            IQueryable<UIProductExpiredReminder> query = null;

            query = (from sid in DB.StockInDetail
                     join si in DB.StockIn on sid.StockInID equals si.ID
                     where si.IsDeleted == false && si.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse
                     && sid.IsDeleted == false
                     select new
                     {
                         sid.WarehouseID,
                         sid.ProductID,
                         sid.ProductSpecificationID,
                         sid.BatchNumber,
                         sid.LicenseNumber,
                         sid.ExpirationDate,
                         sid.ProcurePrice,
                         sid.InQty
                     })
                     .GroupBy(x => new { x.WarehouseID, x.ProductID, x.ProductSpecificationID, x.BatchNumber, x.LicenseNumber, x.ExpirationDate })
                     .Select(x => new UIProductExpiredReminder
                     {
                         WarehouseID = x.Key.WarehouseID,
                         ProductID = x.Key.ProductID,
                         ProductSpecificationID = x.Key.ProductSpecificationID,
                         BatchNumber = x.Key.BatchNumber,
                         LicenseNumber = x.Key.LicenseNumber,
                         ExpirationDate = x.Key.ExpirationDate,
                         ProcurePrice = DB.StockInDetail.Any(y => y.WarehouseID == x.Key.WarehouseID
                             && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                             && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                             && y.ExpirationDate == x.Key.ExpirationDate)
                             ? DB.StockInDetail.FirstOrDefault(y => y.WarehouseID == x.Key.WarehouseID
                             && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                             && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                             && y.ExpirationDate == x.Key.ExpirationDate).ProcurePrice : 0,
                         InQty = (DB.StockInDetail.Any(y => y.IsDeleted == false && y.WarehouseID == x.Key.WarehouseID
                                 && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                 && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                                 && y.ExpirationDate == x.Key.ExpirationDate && (beginDate == null || y.StockIn.EntryDate >= beginDate) && y.StockIn.EntryDate < endDate
                                 && y.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse)
                                ? DB.StockInDetail.Where(y => y.IsDeleted == false && y.WarehouseID == x.Key.WarehouseID
                                    && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                    && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                                    && y.ExpirationDate == x.Key.ExpirationDate && (beginDate == null || y.StockIn.EntryDate >= beginDate) && y.StockIn.EntryDate < endDate
                                    && y.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse).Sum(y => y.InQty) : 0),
                         OutQty = (DB.StockOutDetail.Any(y => y.IsDeleted == false && y.WarehouseID == x.Key.WarehouseID
                                 && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                 && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                                 && y.ExpirationDate == x.Key.ExpirationDate
                                 && y.StockOut.WorkflowStatusID == (int)EWorkflowStatus.OutWarehouse &&
                                 (beginDate == null || y.StockOut.OutDate >= beginDate) && y.StockOut.OutDate < endDate)
                                ? DB.StockOutDetail.Where(y => y.IsDeleted == false && y.WarehouseID == x.Key.WarehouseID
                                    && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                    && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                                    && y.ExpirationDate == x.Key.ExpirationDate
                                    && (((beginDate == null || y.StockOut.OutDate >= beginDate) && y.StockOut.OutDate < endDate)
                                       )).Sum(y => y.OutQty) : 0),
                         WarehouseName = null,
                         ProductName = null,
                         Specification = null,
                         UnitName = null,
                         NumberInLargePackage = 0,

                     });


            var queryEx = from q in query
                          join warehouse in DB.Warehouse on q.WarehouseID equals warehouse.ID
                          join product in DB.Product on q.ProductID equals product.ID
                          join productSpecification in DB.ProductSpecification on q.ProductSpecificationID equals productSpecification.ID
                          join unitName in DB.UnitOfMeasurement on productSpecification.UnitOfMeasurementID equals unitName.ID
                          select new UIProductExpiredReminder()
                          {
                              WarehouseID = q.WarehouseID,
                              ProductID = q.ProductID,
                              ProductSpecificationID = q.ProductSpecificationID,
                              BatchNumber = q.BatchNumber,
                              LicenseNumber = q.LicenseNumber,
                              ExpirationDate = q.ExpirationDate,
                              ProcurePrice = q.ProcurePrice,
                              InQty = q.InQty,
                              OutQty = q.OutQty,
                              WarehouseName = warehouse.Name,
                              ProductName = product.ProductName,
                              Specification = productSpecification.Specification,
                              UnitName = unitName.UnitName,
                              NumberInLargePackage = productSpecification.NumberInLargePackage.HasValue ? productSpecification.NumberInLargePackage.Value : 1,
                          };

            //totalRecords = query.Count();
            //由于这里的计算 先计算最后一次Service跑完 到现在的库存变化，然后取出Service所计算出来的库存，两者合并得来的，金内存后才合并的，因此如果要在此过滤>0的 
            //没有办法在分页了，分页也是徒劳，因为数据已经在内存了。因此这里直接取出所有数据吧
            //query = query.OrderBy(x => x.WarehouseID).Skip(pageSize * pageIndex).Take(pageSize);
            inventoryHistories = queryEx.ToList();
            if (lastInventoryHistory != null)
            {
                inventoryHistories.ForEach(m =>
                {

                    int balanceQty = m.InQty - m.OutQty;
                    var oldInventoryHistory = DB.InventoryHistory.Where(x => x.StatDate == lastStatDate
                          && x.WarehouseID == m.WarehouseID && x.ProductID == m.ProductID && x.ProductSpecificationID == m.ProductSpecificationID
                          && x.BatchNumber == m.BatchNumber && x.LicenseNumber == m.LicenseNumber && x.ExpirationDate == m.ExpirationDate)
                          .FirstOrDefault();
                    if (oldInventoryHistory != null)
                    {
                        m.BalanceQty = oldInventoryHistory.BalanceQty;
                        m.BalanceQty += balanceQty;
                    }
                    else
                    {
                        m.BalanceQty += balanceQty;
                    }
                    m.Amount = m.BalanceQty * m.ProcurePrice;
                    m.NumberOfPackages = m.BalanceQty.ToDecimal() / m.NumberInLargePackage.ToDecimal();
                });
            }
            inventoryHistories = inventoryHistories.Where(x => x.BalanceQty > 0 && DateTime.Now > x.ExpirationDate).ToList();
            totalRecords = inventoryHistories.Count;
            inventoryHistories = inventoryHistories.OrderBy(x => x.WarehouseID).Skip(pageSize * pageIndex).Take(pageSize).ToList();
            return inventoryHistories;

        }
        #endregion


    }
}
