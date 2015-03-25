using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Domain.Models;

namespace ZhongDing.Web.Views.Imports.Editors
{
    public partial class CorrectFlowData : BasePage
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
                string sOwnerEntityID = Request.QueryString["OwnerEntityID"];

                int iOwnerEntityID;

                if (int.TryParse(sOwnerEntityID, out iOwnerEntityID))
                    return iOwnerEntityID;
                else
                    return null;
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!this.OwnerEntityID.HasValue
                || this.OwnerEntityID <= 0))
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_CLOSE_WIN);

                return;
            }

            if (!IsPostBack)
            {
                hdnGridClientID.Value = GridClientID;

                BindHospitals();
            }
        }

        private void BindHospitals()
        {
            var hospitals = PageHospitalRepository.GetDropdownItems();

            rcbxHospital.DataSource = hospitals;
            rcbxHospital.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxHospital.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxHospital.DataBind();

            rcbxHospital.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            var oldDCFlowData = PageDCFlowDataRepository.GetByID(this.OwnerEntityID);

            if (oldDCFlowData != null)
            {
                int hospitalID = int.Parse(rcbxHospital.SelectedValue);

                DBContract dBContract = PageDBContractRepository.GetList(x =>
                    x.ProductID == oldDCFlowData.ProductID && x.ProductSpecificationID == oldDCFlowData.ProductSpecificationID
                    && x.DBContractHospital.Any(y => y.HospitalID == hospitalID)).FirstOrDefault();

                if (dBContract != null)
                {
                    oldDCFlowData.IsOverwritten = true;

                    var dCFlowData = new DCFlowData()
                    {
                        HospitalID = hospitalID,
                        DistributionCompanyID = oldDCFlowData.DistributionCompanyID,
                        ImportFileLogID = oldDCFlowData.ImportFileLogID,
                        ProductID = oldDCFlowData.ProductID,
                        ProductName = oldDCFlowData.ProductName,
                        ProductCode = oldDCFlowData.ProductCode,
                        ProductSpecificationID = oldDCFlowData.ProductSpecificationID,
                        ProductSpecification = oldDCFlowData.ProductSpecification,
                        SaleDate = oldDCFlowData.SaleDate,
                        SaleQty = oldDCFlowData.SaleQty,
                        SettlementDate = oldDCFlowData.SettlementDate,
                        FlowTo = oldDCFlowData.FlowTo,
                        FactoryName = oldDCFlowData.FactoryName,
                        IsCorrectlyFlow = true,
                        IsOverwritten = false,
                        OldDCFlowDataID = oldDCFlowData.ID,
                    };

                    PageDCFlowDataRepository.Add(dCFlowData);

                    var dCFlowDataDetail = new DCFlowDataDetail()
                    {
                        DBContractID = dBContract.ID,
                        ContractCode = dBContract.ContractCode,
                        IsTempContract = dBContract.IsTempContract,
                        HospitalID = hospitalID,
                        HospitalName = rcbxHospital.SelectedItem.Text,
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
                        SaleQty = oldDCFlowData.SaleQty
                    };

                    dCFlowData.DCFlowDataDetail.Add(dCFlowDataDetail);

                    PageDCFlowDataRepository.Save();
                }
                else
                {
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                    this.Master.BaseNotification.AutoCloseDelay = 1000;
                    this.Master.BaseNotification.Show("没有匹配的大包协议，请重新选择");
                }
            }
            else
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_CLOSE_WIN);
            }
        }
    }
}