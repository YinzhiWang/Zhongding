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
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Common.Extension;

namespace ZhongDing.Web.Views.Procures
{
    public partial class TransportFeeMaintenance : BasePage
    {

        #region Members

        private ITransportCompanyRepository _PageTransportCompanyRepository;
        private ITransportCompanyRepository PageTransportCompanyRepository
        {
            get
            {
                if (_PageTransportCompanyRepository == null)
                    _PageTransportCompanyRepository = new TransportCompanyRepository();
                return _PageTransportCompanyRepository;
            }
        }
        private ITransportFeeRepository _PageTransportFeeRepository;
        private ITransportFeeRepository PageTransportFeeRepository
        {
            get
            {
                if (_PageTransportFeeRepository == null)
                    _PageTransportFeeRepository = new TransportFeeRepository();
                return _PageTransportFeeRepository;
            }
        }
        public ETransportFeeType TransportFeeType
        {
            get
            {
                string sEntityID = Request.QueryString["TransportFeeType"];
                ETransportFeeType eTransportFeeType = Common.Enums.ETransportFeeType.StockIn;
                Enum.TryParse<ETransportFeeType>(sEntityID, out eTransportFeeType);
                return eTransportFeeType;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (TransportFeeType == ETransportFeeType.StockIn)
            {
                this.Master.MenuItemID = (int)EMenuItem.TransportFeeManage_StockIn;
                rbtnStockIn.Checked = true;
            }
            else
            {
                this.Master.MenuItemID = (int)EMenuItem.TransportFeeManage_StockOut;
                rbtnStockOut.Checked = true;
            }


            if (!IsPostBack)
            {

                LoadEntityData();

            }

        }
        private void LoadEntityData()
        {
            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                var currentEntity = PageTransportFeeRepository.GetByID(this.CurrentEntityID);

                if (currentEntity != null)
                {
                    if (currentEntity.TransportFeeType == (int)ETransportFeeType.StockIn)
                    {
                        rbtnStockIn.Checked = true;
                    }
                    else
                    {
                        rbtnStockOut.Checked = true;
                    }
                    BindTransportCompanys(currentEntity.TransportCompanyID);

                    txtTransportCompanyNumber.Text = currentEntity.TransportCompanyNumber;

                    txtDriver.Text = currentEntity.Driver;
                    txtDriverTelephone.Text = currentEntity.DriverTelephone;
                    txtStartPlace.Text = currentEntity.StartPlace;
                    txtStartTelephone.Text = currentEntity.StartPlaceTelephone;
                    txtEndPlace.Text = currentEntity.EndPlace;
                    txtEndPlaceTelephone.Text = currentEntity.EndPlaceTelephone;
                    txtFee.Value = currentEntity.Fee.ToDouble();
                    txtSendDate.SelectedDate = currentEntity.SendDate;

                    txtRemark.Text = currentEntity.Remark;

                }
            }
            else
            {
                btnDelete.Visible = false;
                BindTransportCompanys();
            }

            lblOperator.Text = "操作人：" + ZhongDing.Web.Extensions.SiteUser.GetCurrentSiteUser().UserName;
        }
        private void BindTransportCompanys(int? ransportCompanyID = null)
        {
            var companys = PageTransportCompanyRepository.GetDropdownItems();
            rcbxTransportCompany.DataSource = companys;
            rcbxTransportCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxTransportCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxTransportCompany.DataBind();
            rcbxTransportCompany.Items.Insert(0, new RadComboBoxItem("", ""));
            if (ransportCompanyID.HasValue)
                rcbxTransportCompany.SelectedValue = ransportCompanyID.Value.ToString();
        }


        protected void cvCompanyName_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rcbxTransportCompany.SelectedValue))
                cvTransportCompany.IsValid = false;

            if (!IsValid) return;

            TransportFee currentEntity = null;

            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
                currentEntity = PageTransportFeeRepository.GetByID(this.CurrentEntityID);
            else
            {
                currentEntity = new TransportFee();

                PageTransportFeeRepository.Add(currentEntity);
            }

            if (currentEntity != null)
            {
                currentEntity.TransportCompanyID = rcbxTransportCompany.SelectedValue.ToInt();
                currentEntity.TransportCompanyNumber = txtTransportCompanyNumber.Text;
                currentEntity.TransportFeeType = rbtnStockIn.Checked ? ((int)ETransportFeeType.StockIn) : ((int)ETransportFeeType.StockOut);
                currentEntity.Driver = txtDriver.Text.Trim();
                currentEntity.DriverTelephone = txtDriverTelephone.Text.Trim();
                currentEntity.StartPlace = txtStartPlace.Text.Trim();
                currentEntity.StartPlaceTelephone = txtStartTelephone.Text.Trim();

                currentEntity.EndPlace = txtEndPlace.Text.Trim();
                currentEntity.EndPlaceTelephone = txtEndPlaceTelephone.Text.Trim();
                currentEntity.Fee = txtFee.Value.GetValueOrDefault(0).ToDecimal();
                currentEntity.SendDate = txtSendDate.SelectedDate.Value;

                currentEntity.Remark = txtRemark.Text;

                PageTransportFeeRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntityID.HasValue
                   && this.CurrentEntityID > 0)
            {
                PageTransportFeeRepository.DeleteByID(this.CurrentEntityID);
                PageTransportFeeRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_DELETED_REDIRECT);
            }
        }

        protected void rcbxTransportCompany_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var transportCompanyID = e.Value.ToIntOrNull();
            if (transportCompanyID.HasValue && transportCompanyID.Value > 0)
            {
                var transportCompany = PageTransportCompanyRepository.GetByID(transportCompanyID.Value);
                txtDriver.Text = transportCompany.Driver;
                txtDriverTelephone.Text = transportCompany.DriverTelephone;
            }
            else
            {
                txtDriver.Text = txtDriverTelephone.Text = string.Empty;

            }
        }
    }
}