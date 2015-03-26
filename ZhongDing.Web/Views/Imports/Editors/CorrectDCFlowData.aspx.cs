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
using ZhongDing.Web.Extensions;

namespace ZhongDing.Web.Views.Imports.Editors
{
    public partial class CorrectDCFlowData : BasePage
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
                return WebUtility.GetValueFromQueryString("OwnerEntityID");
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

        private IProductRepository _PageProductRepository;
        private IProductRepository PageProductRepository
        {
            get
            {
                if (_PageProductRepository == null)
                    _PageProductRepository = new ProductRepository();

                return _PageProductRepository;
            }
        }

        private IProductSpecificationRepository _PageProductSpecificationRepository;
        private IProductSpecificationRepository PageProductSpecificationRepository
        {
            get
            {
                if (_PageProductSpecificationRepository == null)
                    _PageProductSpecificationRepository = new ProductSpecificationRepository();

                return _PageProductSpecificationRepository;
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

                lblDistributionCompany.Text = this.CurrentOwnerEntity.DistributionCompany.Name;

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

            if (this.CurrentOwnerEntity != null)
            {
                ////test codes
                //hdnNewEntityID.Value = "2";
                //this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                //this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_CLOSE_WIN);
                //return;

                var oldDCFlowData = this.CurrentOwnerEntity;

                int hospitalID = int.Parse(rcbxHospital.SelectedValue);
                string productCode = txtProductCode.Text.Trim();
                string productName = txtProductName.Text.Trim();
                string specification = txtSpecification.Text.Trim();
                string factoryName = txtFactoryName.Text.Trim();
                DateTime saleDate = rdpSaleDate.SelectedDate.Value;
                int saleQty = txtSaleQty.Value.HasValue ? (int)txtSaleQty.Value.Value : 0;
                string flowTo = rcbxHospital.SelectedItem.Text;

                var product = PageProductRepository.GetList(x =>
                    x.ProductCode.ToLower().Equals(productCode.ToLower())).FirstOrDefault();

                if (product != null)
                {
                    var productSpecification = PageProductSpecificationRepository
                        .GetList(x => x.Specification.ToLower().Equals(specification.ToLower()))
                        .FirstOrDefault();

                    if (productSpecification != null)
                    {
                        DBContract dBContract = PageDBContractRepository.GetList(x =>
                            x.ProductID == product.ID && x.ProductSpecificationID == productSpecification.ID
                            && x.DBContractHospital.Any(y => y.HospitalID == hospitalID)).FirstOrDefault();

                        if (dBContract != null)
                        {
                            var dCFlowData = PageDCFlowDataRepository.GetList(x => x.ID != oldDCFlowData.ID
                                && x.DistributionCompanyID == oldDCFlowData.DistributionCompanyID
                                && x.ProductID == product.ID && x.ProductSpecificationID == productSpecification.ID
                                && x.SaleDate == saleDate && x.SettlementDate == oldDCFlowData.SettlementDate
                                && x.FlowTo == flowTo).FirstOrDefault();

                            if (dCFlowData == null)
                            {
                                oldDCFlowData.IsOverwritten = true;

                                dCFlowData = new DCFlowData()
                                {
                                    HospitalID = hospitalID,
                                    DistributionCompanyID = oldDCFlowData.DistributionCompanyID,
                                    ImportFileLogID = oldDCFlowData.ImportFileLogID,
                                    ProductID = product.ID,
                                    ProductName = productName,
                                    ProductCode = productCode,
                                    ProductSpecificationID = productSpecification.ID,
                                    ProductSpecification = specification,
                                    SaleDate = saleDate,
                                    SaleQty = saleQty,
                                    SettlementDate = oldDCFlowData.SettlementDate,
                                    FlowTo = flowTo,
                                    FactoryName = factoryName,
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
                                    HospitalName = flowTo,
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
                                    SaleQty = saleQty
                                };

                                dCFlowData.DCFlowDataDetail.Add(dCFlowDataDetail);

                                PageDCFlowDataRepository.Save();

                                hdnNewEntityID.Value = dCFlowData.ID.ToString();

                                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_CLOSE_WIN);
                            }
                            else
                            {
                                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                                this.Master.BaseNotification.AutoCloseDelay = 1000;
                                this.Master.BaseNotification.Show("该条流向数据已存在，请勿重复操作");
                            }
                        }
                        else
                        {
                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                            this.Master.BaseNotification.AutoCloseDelay = 1000;
                            this.Master.BaseNotification.Show("没有匹配的大包协议");
                        }
                    }
                    else
                    {
                        this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                        this.Master.BaseNotification.AutoCloseDelay = 1000;
                        this.Master.BaseNotification.Show("该货品规则不存在，请确认货品规格");
                    }
                }
                else
                {
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                    this.Master.BaseNotification.AutoCloseDelay = 1000;
                    this.Master.BaseNotification.Show("该货品不存在，请确认货品编号");
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