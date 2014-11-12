using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Domain.Models;

namespace ZhongDing.Web.Views.Basics
{
    public partial class WarehouseMaintenance : BasePage
    {

        #region Members

        private IWarehouseRepository _PageWarehouseRepository;
        private IWarehouseRepository PageWarehouseRepository
        {
            get
            {
                if (_PageWarehouseRepository == null)
                    _PageWarehouseRepository = new WarehouseRepository();

                return _PageWarehouseRepository;
            }
        }

        private ISaleTypeRepository _PageSaleTypeRepository;
        private ISaleTypeRepository PageSaleTypeRepository
        {
            get
            {
                if (_PageSaleTypeRepository == null)
                    _PageSaleTypeRepository = new SaleTypeRepository();

                return _PageSaleTypeRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = 8;

            if (!IsPostBack)
            {
                BindSaleTypes();

                LoadEntityData();
            }

        }

        private void BindSaleTypes()
        {
            var saleTypes = PageSaleTypeRepository.GetDropdownItems();

            ddlSaleType.DataSource = saleTypes;
            ddlSaleType.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            ddlSaleType.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            ddlSaleType.DataBind();
        }

        private void LoadEntityData()
        {
            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                var currentEntity = PageWarehouseRepository.GetByID(this.CurrentEntityID);

                if (currentEntity != null)
                {
                    txtSerialNo.Text = currentEntity.WarehouseCode;
                    txtName.Text = currentEntity.Name;
                    txtAddress.Text = currentEntity.Address;

                    if (currentEntity.SaleTypeID.HasValue)
                        ddlSaleType.SelectedValue = currentEntity.SaleTypeID.ToString();

                    txtComment.Text = currentEntity.Comment;
                }
            }
            else
            {
                btnDelete.Visible = false;
                txtSerialNo.Text = Utility.GenerateAutoSerialNo(PageWarehouseRepository.GetMaxEntityID(),
                    GlobalConst.EntityAutoSerialNo.SerialNoPrefix.WAREHOUSE);
            }
        }


        protected void cvName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (PageWarehouseRepository.GetList(x => x.ID != this.CurrentEntityID
                && x.Name.Equals(txtName.Text.Trim())).Count() > 0)
            {
                args.IsValid = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            Warehouse currentEntity = null;

            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
                currentEntity = PageWarehouseRepository.GetByID(this.CurrentEntityID);
            else
            {
                currentEntity = new Warehouse();
                currentEntity.CompanyID = CurrentUser.CompanyID;
                currentEntity.WarehouseCode = Utility.GenerateAutoSerialNo(PageWarehouseRepository.GetMaxEntityID(),
                    GlobalConst.EntityAutoSerialNo.SerialNoPrefix.WAREHOUSE);

                PageWarehouseRepository.Add(currentEntity);
            }

            if (currentEntity != null)
            {
                currentEntity.Name = txtName.Text.Trim();
                currentEntity.Address = txtAddress.Text.Trim();
                currentEntity.Comment = txtComment.Text.Trim();

                if (!string.IsNullOrEmpty(ddlSaleType.SelectedValue))
                    currentEntity.SaleTypeID = Convert.ToInt32(ddlSaleType.SelectedValue);

                PageWarehouseRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntityID.HasValue
                   && this.CurrentEntityID > 0)
            {
                PageWarehouseRepository.DeleteByID(this.CurrentEntityID);
                PageWarehouseRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_DELETED_REDIRECT);
            }
        }
    }
}