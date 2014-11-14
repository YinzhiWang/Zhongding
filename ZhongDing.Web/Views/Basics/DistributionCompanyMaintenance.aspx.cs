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
    public partial class DistributionCompanyMaintenance : BasePage
    {
        #region Members

        private IDistributionCompanyRepository _PageDistributionCompanyRepository;
        private IDistributionCompanyRepository PageDistributionCompanyRepository
        {
            get
            {
                if (_PageDistributionCompanyRepository == null)
                    _PageDistributionCompanyRepository = new DistributionCompanyRepository();

                return _PageDistributionCompanyRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.DistributionCompanyManage;

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
                var currentEntity = PageDistributionCompanyRepository.GetByID(this.CurrentEntityID);

                if (currentEntity != null)
                {
                    txtSerialNo.Text = currentEntity.SerialNo;
                    txtName.Text = currentEntity.Name;
                    txtReceiverName.Text = currentEntity.ReceiverName;
                    txtPhoneNumber.Text = currentEntity.PhoneNumber;
                    txtAddress.Text = currentEntity.Address;
                }
            }
            else
            {
                btnDelete.Visible = false;
                txtSerialNo.Text = Utility.GenerateAutoSerialNo(PageDistributionCompanyRepository.GetMaxEntityID(),
                    GlobalConst.EntityAutoSerialNo.SerialNoPrefix.DISTRIBUTION_COMPANY);
            }
        }

        protected void cvName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (PageDistributionCompanyRepository.GetList(x => x.ID != this.CurrentEntityID
                && x.Name.Equals(txtName.Text.Trim())).Count() > 0)
            {
                args.IsValid = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            DistributionCompany currentEntity = null;

            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
                currentEntity = PageDistributionCompanyRepository.GetByID(this.CurrentEntityID);
            else
            {
                currentEntity = new DistributionCompany();
                currentEntity.SerialNo = Utility.GenerateAutoSerialNo(PageDistributionCompanyRepository.GetMaxEntityID(),
                    GlobalConst.EntityAutoSerialNo.SerialNoPrefix.DISTRIBUTION_COMPANY);

                PageDistributionCompanyRepository.Add(currentEntity);
            }

            if (currentEntity != null)
            {
                currentEntity.Name = txtName.Text.Trim();
                currentEntity.ReceiverName = txtReceiverName.Text.Trim();
                currentEntity.PhoneNumber = txtPhoneNumber.Text.Trim();
                currentEntity.Address = txtAddress.Text.Trim();

                PageDistributionCompanyRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntityID.HasValue
                   && this.CurrentEntityID > 0)
            {
                PageDistributionCompanyRepository.DeleteByID(this.CurrentEntityID);
                PageDistributionCompanyRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_DELETED_REDIRECT);
            }
        }
    }
}