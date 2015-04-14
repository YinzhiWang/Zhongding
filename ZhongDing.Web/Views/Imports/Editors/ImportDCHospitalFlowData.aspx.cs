using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Business.Repositories.Imports;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Web.Extensions;

namespace ZhongDing.Web.Views.Imports.Editors
{
    public partial class ImportDCHospitalFlowData : BasePage
    {
        #region Fields

        /// <summary>
        /// 所属的实体ID
        /// </summary>
        /// <value>The owner entity ID.</value>
        private int? OwnerEntityID
        {
            get
            {
                return WebUtility.GetIntFromQueryString("OwnerEntityID");
            }
        }

        #endregion

        #region Members

        private IDCFlowDataRepository _PageDCFlowDataRepository;
        private IDCFlowDataRepository PageDCFlowDataRepository
        {
            get
            {
                if (_PageDCFlowDataRepository == null)
                    _PageDCFlowDataRepository = new DCFlowDataRepository();

                return _PageDCFlowDataRepository;
            }
        }

        private IDCFlowDataDetailRepository _PageDCFlowDataDetailRepository;
        private IDCFlowDataDetailRepository PageDCFlowDataDetailRepository
        {
            get
            {
                if (_PageDCFlowDataDetailRepository == null)
                    _PageDCFlowDataDetailRepository = new DCFlowDataDetailRepository();

                return _PageDCFlowDataDetailRepository;
            }
        }

        private IHospitalRepository _PageHospitalRepository;
        private IHospitalRepository PageHospitalRepository
        {
            get
            {
                if (_PageHospitalRepository == null)
                    _PageHospitalRepository = new HospitalRepository();

                return _PageHospitalRepository;
            }
        }

        private IDBContractRepository _PageDBContractRepository;
        private IDBContractRepository PageDBContractRepository
        {
            get
            {
                if (_PageDBContractRepository == null)
                    _PageDBContractRepository = new DBContractRepository();

                return _PageDBContractRepository;
            }
        }

        private DCFlowData _CurrentOwnerEntity;
        private DCFlowData CurrentOwnerEntity
        {
            get
            {
                if (_CurrentOwnerEntity == null)
                    if (this.OwnerEntityID.HasValue && this.OwnerEntityID > 0)
                        _CurrentOwnerEntity = PageDCFlowDataRepository.GetByID(this.OwnerEntityID);

                return _CurrentOwnerEntity;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.CurrentOwnerEntity == null
                || (this.CurrentOwnerEntity != null
                    && (this.CurrentOwnerEntity.IsCorrectlyFlow == true
                        || this.CurrentOwnerEntity.IsOverwritten == true)))
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_CLOSE_WIN);

                return;
            }

            if (!IsPostBack)
            {
                hdnGridClientID.Value = GridClientID;
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            string fileExtension = string.Empty;

            if (!FileExcel.HasFile)
            {
                cvUploadFile.IsValid = false;
                cvUploadFile.ErrorMessage = "请选择要导入的Excel文件";
            }
            else
            {
                fileExtension = Path.GetExtension(FileExcel.FileName);

                if (!fileExtension.ToLower().Equals(".xls")
                    && !fileExtension.ToLower().Equals(".xlsx"))
                {
                    cvUploadFile.IsValid = false;
                    cvUploadFile.ErrorMessage = "文件格式不正确，请选择Excel文件导入";
                }
            }

            if (!IsValid) return;

            EExcelType excelType = EExcelType.Excel2007Plus;
            if (fileExtension.ToLower().Equals(".xls"))
                excelType = EExcelType.Excel2003;

            DataSet ds = ExcelHelper.ConvertExcelToDataSet(FileExcel.FileContent, excelType);

            if (ds != null && ds.Tables.Count > 0
                && ds.Tables[0].Rows.Count > 0)
            {
                string errorMsg = string.Empty;
                int errorRowCount = 0;
                int rowIndex = 0;
                int totalSaleQty = 0;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string tempSaleDate = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.SALE_DATE]);
                    string tempSaleQty = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.SALE_QTY]);
                    string flowTo = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.FLOW_TO]);
                    string comment = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.COMMENT]);

                    try
                    {
                        rowIndex++;

                        if (this.CurrentOwnerEntity != null)
                        {
                            DateTime saleDate;
                            if (DateTime.TryParse(tempSaleDate, out saleDate))
                            {
                                int saleQty;
                                if (int.TryParse(tempSaleQty, out saleQty))
                                {
                                    totalSaleQty += saleQty;

                                    var hospital = PageHospitalRepository.GetList(x =>
                                        x.HospitalName.ToLower().Equals(flowTo.ToLower())).FirstOrDefault();

                                    if (hospital != null)
                                    {
                                        DBContract dBContract = PageDBContractRepository.GetList(x =>
                                               x.ProductID == CurrentOwnerEntity.ProductID
                                               && x.ProductSpecificationID == CurrentOwnerEntity.ProductSpecificationID
                                               && x.DBContractHospital.Any(y => y.HospitalID == hospital.ID)).FirstOrDefault();

                                        if (dBContract != null)
                                        {
                                            var dCFlowDataDetail = new DCFlowDataDetail()
                                            {
                                                DBContractID = dBContract.ID,
                                                ContractCode = dBContract.ContractCode,
                                                IsTempContract = dBContract.IsTempContract,
                                                HospitalID = hospital.ID,
                                                HospitalName = hospital.HospitalName,
                                                ClientUserID = dBContract.ClientUserID.HasValue
                                                    ? dBContract.ClientUserID.Value : GlobalConst.INVALID_INT,
                                                ClientUserName = dBContract.ClientUser == null
                                                    ? string.Empty : dBContract.ClientUser.ClientName,
                                                InChargeUserID = dBContract.InChargeUserID,
                                                InChargeUserFullName = dBContract.Users == null
                                                    ? string.Empty : dBContract.Users.FullName,
                                                UnitOfMeasurementID = dBContract.ProductSpecification != null
                                                    ? dBContract.ProductSpecification.UnitOfMeasurementID : null,
                                                UnitName = (dBContract.ProductSpecification != null && dBContract.ProductSpecification.UnitOfMeasurement != null)
                                                    ? dBContract.ProductSpecification.UnitOfMeasurement.UnitName : string.Empty,
                                                SaleDate = saleDate,
                                                SaleQty = saleQty,
                                                Comment = comment
                                            };

                                            CurrentOwnerEntity.DCFlowDataDetail.Add(dCFlowDataDetail);
                                        }
                                    }
                                }
                                else
                                {
                                    errorRowCount++;

                                    errorMsg += "第" + rowIndex + "行" + GlobalConst.ImportDataColumns.SALE_QTY + "格式错误；";
                                }
                            }
                            else
                            {
                                errorRowCount++;

                                errorMsg += "第" + rowIndex + "行" + GlobalConst.ImportDataColumns.SALE_DATE + "格式错误；";
                            }
                        }
                    }
                    catch (Exception exp)
                    {
                        Utility.WriteExceptionLog(exp);
                    }
                }

                if (errorRowCount > 0
                    || totalSaleQty > CurrentOwnerEntity.SaleQty)
                {
                    cvUploadFile.IsValid = false;

                    if (totalSaleQty > CurrentOwnerEntity.SaleQty)
                    {
                        if (!string.IsNullOrEmpty(errorMsg))
                            errorMsg += "<br>";

                        errorMsg += "销售总数量（" + totalSaleQty + "）不能大于出货数量(" + CurrentOwnerEntity.SaleQty + ")";
                    }

                    cvUploadFile.ErrorMessage = "导入出错：" + errorMsg;
                }
                else
                {
                    CurrentOwnerEntity.IsCorrectlyFlow = true;

                    PageDCFlowDataRepository.Save();

                    this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_CLOSE_WIN);
                }
            }
        }
    }
}