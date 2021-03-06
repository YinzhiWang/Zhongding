﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.WinService.Lib
{
    public class CalculateInventoryService
    {
        /// <summary>
        /// Processes the work.
        /// </summary>
        public static void ProcessWork()
        {
            Utility.WriteTrace("Process Calculate Inventory Work begin at:" + DateTime.Now);

            IInventoryHistoryRepository inventoryHistoryRepository = new InventoryHistoryRepository();

            #region 测试代码
            //var statDate = new DateTime(2015, 1, 14);
            //var needSavedInventoryHistories = inventoryHistoryRepository.CalculateInventoryByStatDate(statDate);
            //if (needSavedInventoryHistories.Count > 0)
            //{
            //    foreach (var curInventoryHistory in needSavedInventoryHistories)
            //    {
            //        var inventoryHistory = new InventoryHistory
            //        {
            //            WarehouseID = curInventoryHistory.WarehouseID,
            //            ProductID = curInventoryHistory.ProductID,
            //            ProductSpecificationID = curInventoryHistory.ProductSpecificationID,
            //            BatchNumber = curInventoryHistory.BatchNumber,
            //            LicenseNumber = curInventoryHistory.LicenseNumber,
            //            ExpirationDate = curInventoryHistory.ExpirationDate,
            //            ProcurePrice = curInventoryHistory.ProcurePrice,
            //            StatDate = curInventoryHistory.StatDate,
            //            InQty = curInventoryHistory.InQty,
            //            OutQty = curInventoryHistory.OutQty,
            //            BalanceQty = curInventoryHistory.BalanceQty
            //        };

            //        inventoryHistoryRepository.Add(inventoryHistory);
            //    }

            //    inventoryHistoryRepository.Save();
            //}

            #endregion

            //首次运行，直接计算前一天的库存
            if (inventoryHistoryRepository.GetCount() == 0)
            {
                var statDate = DateTime.Now.Date.AddDays(-1);
                var needSavedInventoryHistories = inventoryHistoryRepository.CalculateInventoryByStatDate(statDate);

                if (needSavedInventoryHistories.Count > 0)
                {
                    foreach (var curInventoryHistory in needSavedInventoryHistories)
                    {
                        var inventoryHistory = new InventoryHistory
                        {
                            WarehouseID = curInventoryHistory.WarehouseID,
                            ProductID = curInventoryHistory.ProductID,
                            ProductSpecificationID = curInventoryHistory.ProductSpecificationID,
                            BatchNumber = curInventoryHistory.BatchNumber,
                            LicenseNumber = curInventoryHistory.LicenseNumber,
                            ExpirationDate = curInventoryHistory.ExpirationDate,
                            ProcurePrice = curInventoryHistory.ProcurePrice,
                            StatDate = curInventoryHistory.StatDate,
                            InQty = curInventoryHistory.InQty,
                            OutQty = curInventoryHistory.OutQty,
                            BalanceQty = curInventoryHistory.BalanceQty
                        };

                        inventoryHistoryRepository.Add(inventoryHistory);
                    }

                    inventoryHistoryRepository.Save();
                }

            }
            else
            {
                var lastStatDate = inventoryHistoryRepository.GetList().Max(x => x.StatDate);

                long intervalDays = DateAndTime.DateDiff(DateInterval.Day, lastStatDate, DateTime.Now);

                for (int i = 0; i < intervalDays; i++)
                {
                    var statDate = lastStatDate.Date.AddDays(i + 1);

                    if (statDate.Date < DateTime.Now.Date)
                    {
                        var needSavedInventoryHistories = new List<UIInventoryHistory>();

                        var yesterdayDate = statDate.Date.AddDays(-1);
                        //获取前一天的所有库存数据
                        var yesterdayInventoryHistories = inventoryHistoryRepository.GetList(x => x.StatDate == yesterdayDate)
                            .Select(x => new UIInventoryHistory
                            {
                                WarehouseID = x.WarehouseID,
                                ProductID = x.ProductID,
                                ProductSpecificationID = x.ProductSpecificationID,
                                BatchNumber = x.BatchNumber,
                                LicenseNumber = x.LicenseNumber,
                                ExpirationDate = x.ExpirationDate,
                                ProcurePrice = x.ProcurePrice,
                                StatDate = statDate.Date,
                                InQty = x.InQty,
                                OutQty = x.OutQty,
                                BalanceQty = x.BalanceQty
                            })
                            .ToList();

                        needSavedInventoryHistories.AddRange(yesterdayInventoryHistories);

                        //计算需要更新的所有库存数据
                        var calculatedInventoryHistories = inventoryHistoryRepository.CalculateInventoryByStatDate(statDate);

                        //合并两种数据（前一天的和当天的数据）
                        foreach (var curInventoryHistory in calculatedInventoryHistories)
                        {
                            var curNeedSaved = needSavedInventoryHistories.FirstOrDefault(x => x.StatDate == curInventoryHistory.StatDate
                                && x.WarehouseID == curInventoryHistory.WarehouseID && x.ExpirationDate == curInventoryHistory.ExpirationDate
                                && x.ProductID == curInventoryHistory.ProductID && x.ProductSpecificationID == curInventoryHistory.ProductSpecificationID
                                && x.BatchNumber == curInventoryHistory.BatchNumber && x.LicenseNumber == curInventoryHistory.LicenseNumber);

                            if (curNeedSaved != null)
                            {
                                curNeedSaved.InQty = curInventoryHistory.InQty;
                                curNeedSaved.OutQty = curInventoryHistory.OutQty;
                                curNeedSaved.BalanceQty = curInventoryHistory.BalanceQty;
                                curNeedSaved.ProcurePrice = curInventoryHistory.ProcurePrice;
                            }
                            else
                            {
                                needSavedInventoryHistories.Add(curInventoryHistory);
                            }
                        }

                        //保存到数据库
                        foreach (var curInventoryHistory in needSavedInventoryHistories)
                        {
                            var inventoryHistory = new InventoryHistory
                            {
                                WarehouseID = curInventoryHistory.WarehouseID,
                                ProductID = curInventoryHistory.ProductID,
                                ProductSpecificationID = curInventoryHistory.ProductSpecificationID,
                                BatchNumber = curInventoryHistory.BatchNumber,
                                LicenseNumber = curInventoryHistory.LicenseNumber,
                                ExpirationDate = curInventoryHistory.ExpirationDate,
                                ProcurePrice = curInventoryHistory.ProcurePrice,
                                StatDate = curInventoryHistory.StatDate,
                                InQty = curInventoryHistory.InQty,
                                OutQty = curInventoryHistory.OutQty,
                                BalanceQty = curInventoryHistory.BalanceQty
                            };

                            inventoryHistoryRepository.Add(inventoryHistory);
                        }

                        inventoryHistoryRepository.Save();
                    }
                }
            }

            Utility.WriteTrace("Process Calculate Inventory Work end at:" + DateTime.Now);
        }
    }
}
