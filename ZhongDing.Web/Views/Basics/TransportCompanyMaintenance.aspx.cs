using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;

namespace ZhongDing.Web.Views.Basics
{
    public partial class TransportCompanyMaintenance : BasePage
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
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.TransportCompanyManage;

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
                var currentEntity = PageTransportCompanyRepository.GetByID(this.CurrentEntityID);

                if (currentEntity != null)
                {
                    txtCompanyName.Text = currentEntity.CompanyName;
                    txtCompanyAddress.Text = currentEntity.CompanyAddress;
                    txtTelephone.Text = currentEntity.Telephone;
                    txtDriver.Text = currentEntity.Driver;
                    txtDriverTelephone.Text = currentEntity.DriverTelephone;
                    txtRemark.Text = currentEntity.Remark;

                }
            }
            else
            {
                btnDelete.Visible = false;

            }
        }


        protected void cvCompanyName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (PageTransportCompanyRepository.GetList(x => x.ID != this.CurrentEntityID
                && x.CompanyName.Equals(txtCompanyName.Text.Trim())).Count() > 0)
            {
                args.IsValid = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            TransportCompany currentEntity = null;

            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
                currentEntity = PageTransportCompanyRepository.GetByID(this.CurrentEntityID);
            else
            {
                currentEntity = new TransportCompany();
                PageTransportCompanyRepository.Add(currentEntity);
            }

            if (currentEntity != null)
            {
                currentEntity.CompanyName = txtCompanyName.Text.Trim();
                currentEntity.CompanyAddress = txtCompanyAddress.Text.Trim();
                currentEntity.Telephone = txtTelephone.Text.Trim();
                currentEntity.Driver = txtDriver.Text.Trim();
                currentEntity.DriverTelephone = txtDriverTelephone.Text.Trim();
                currentEntity.Remark = txtRemark.Text.Trim();
         
                PageTransportCompanyRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntityID.HasValue
                   && this.CurrentEntityID > 0)
            {
                PageTransportCompanyRepository.DeleteByID(this.CurrentEntityID);
                PageTransportCompanyRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_DELETED_REDIRECT);
            }
        }
    }
}