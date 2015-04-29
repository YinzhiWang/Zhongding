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
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.CautionMoneys
{
    public partial class SupplierCautionMoneyMaintenance : BasePage
    {

        #region Members


        private ISupplierCautionMoneyRepository _PageSupplierCautionMoneyRepository;
        private ISupplierCautionMoneyRepository PageSupplierCautionMoneyRepository
        {
            get
            {
                if (_PageSupplierCautionMoneyRepository == null)
                    _PageSupplierCautionMoneyRepository = new SupplierCautionMoneyRepository();
                return _PageSupplierCautionMoneyRepository;
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

        private ISupplierRepository _PageSupplierRepository;
        private ISupplierRepository PageSupplierRepository
        {
            get
            {
                if (_PageSupplierRepository == null)
                    _PageSupplierRepository = new SupplierRepository();

                return _PageSupplierRepository;
            }
        }
        private ICautionMoneyTypeRepository _PageCautionMoneyTypeRepository;
        private ICautionMoneyTypeRepository PageCautionMoneyTypeRepository
        {
            get
            {
                if (_PageCautionMoneyTypeRepository == null)
                    _PageCautionMoneyTypeRepository = new CautionMoneyTypeRepository();

                return _PageCautionMoneyTypeRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            this.Master.MenuItemID = (int)EMenuItem.SupplierCautionMoneyApplyManage;


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
                var currentEntity = PageSupplierCautionMoneyRepository.GetByID(this.CurrentEntityID);

                if (currentEntity != null)
                {
                    hdnCurrentEntityID.Value = currentEntity.ID.ToString();



                    txtRemark.Text = currentEntity.Remark;

                }
            }
            else
            {
                BindSuppliers();
                BindProducts();
                BindCautionMoneyTypes();
                if (rcbxProduct.SelectedValue.ToIntOrNull().BiggerThanZero())
                    BindProductSpecifications(rcbxProduct.SelectedValue.ToInt());
                btnDelete.Visible = false;

            }

            lblOperator.Text = "操作人：" + ZhongDing.Web.Extensions.SiteUser.GetCurrentSiteUser().UserName;
        }


        private void BindProducts()
        {
            var products = PageProductRepository.GetDropdownItems(new UISearchDropdownItem()
            {
                Extension = new UISearchExtension { CompanyID = CurrentUser.CompanyID }
            });

            rcbxProduct.DataSource = products;
            rcbxProduct.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxProduct.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxProduct.DataBind();

            rcbxProduct.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindProductSpecifications(int productID)
        {
            if (productID > 0)
            {
                rcbxProductSpecification.Items.Clear();

                var productSpecifications = PageProductSpecificationRepository.GetDropdownItems(new UISearchDropdownItem()
                {
                    Extension = new UISearchExtension { ProductID = productID }
                });

                rcbxProductSpecification.DataSource = productSpecifications;
                rcbxProductSpecification.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                rcbxProductSpecification.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                rcbxProductSpecification.DataBind();
            }
        }
        private void BindSuppliers()
        {
            var suppliers = PageSupplierRepository.GetDropdownItems(new UISearchDropdownItem
            {
                Extension = new UISearchExtension
                {
                    CompanyID = CurrentUser.CompanyID
                }
            });

            rcbxSupplier.DataSource = suppliers;
            rcbxSupplier.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxSupplier.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxSupplier.DataBind();

            rcbxSupplier.Items.Insert(0, new RadComboBoxItem("", ""));
        }
        private void BindCautionMoneyTypes()
        {
            var items = PageCautionMoneyTypeRepository.GetDropdownItems(new UISearchDropdownItem
            {
                Extension = new UISearchExtension
                {
                     
                }
            });

            rcbxCautionMoneyType.DataSource = items;
            rcbxCautionMoneyType.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxCautionMoneyType.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxCautionMoneyType.DataBind();

            rcbxCautionMoneyType.Items.Insert(0, new RadComboBoxItem("", ""));
        }
        protected void rcbxProduct_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value))
            {
                int productID;
                if (int.TryParse(e.Value, out productID))
                {
                    BindProductSpecifications(productID);
                }
            }
        }
        protected void cvCompanyName_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (rcbxProduct.SelectedValue.IsNullOrEmpty())
                cvProductName.IsValid = false;
            if (rcbxProductSpecification.SelectedValue.IsNullOrEmpty())
                cvProductSpecification.IsValid = false;

            if (rcbxSupplier.SelectedValue.IsNullOrEmpty())
                cvSupplier.IsValid = false;
            if (rcbxCautionMoneyType.SelectedValue.IsNullOrEmpty())
                cvCautionMoneyType.IsValid = false;



            if (!IsValid) return;

            SupplierCautionMoney currentEntity = null;

            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
                currentEntity = PageSupplierCautionMoneyRepository.GetByID(this.CurrentEntityID);
            else
            {
                currentEntity = new SupplierCautionMoney()
                {
                    ApplyDate = rdpApplyDate.SelectedDate.Value,
                    CautionMoneyTypeID = rcbxCautionMoneyType.SelectedValue.ToInt(),
                    EndDate = rdpEndDate.SelectedDate.Value,
                    IsStop = false,
                    PaymentCautionMoney = txtPaymentCautionMoney.Value.ToDecimal(),
                    ProductID = rcbxProduct.SelectedValue.ToInt(),
                    ProductSpecificationID = rcbxProductSpecification.SelectedValue.ToInt(),
                    Remark = txtRemark.Text.Trim(),
                    SupplierID = rcbxSupplier.SelectedValue.ToInt(),
                    WorkflowStatusID = 1
                };

                PageSupplierCautionMoneyRepository.Add(currentEntity);
            }

            if (currentEntity != null)
            {
                currentEntity.Remark = txtRemark.Text;

                PageSupplierCautionMoneyRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntityID.HasValue
                   && this.CurrentEntityID > 0)
            {
                PageSupplierCautionMoneyRepository.DeleteByID(this.CurrentEntityID);
                PageSupplierCautionMoneyRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_DELETED_REDIRECT);
            }
        }


        //Grid
        protected void rgStockIns_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (this.CurrentEntityID.HasValue
                 && this.CurrentEntityID > 0)
            {
                //int totalRecords;
                //var stockIns = PageSupplierCautionMoneyStockInRepository.GetSupplierCautionMoneyStockInsBySupplierCautionMoneyID(CurrentEntityID.Value, out totalRecords);
                //rgStockIns.DataSource = stockIns;
                //rgStockIns.VirtualItemCount = totalRecords;
            }

        }

        protected void rgStockIns_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            var SupplierCautionMoneyStockInId = editableItem.GetDataKeyValue("ID").ToIntOrNull();
            if (SupplierCautionMoneyStockInId.HasValue && SupplierCautionMoneyStockInId.Value > 0)
            {

                rgStockIns.Rebind();
            }
        }
        //Grid
        protected void rgStockOuts_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (this.CurrentEntityID.HasValue
                 && this.CurrentEntityID > 0)
            {
                //int totalRecords;
                //var stockOuts = PageSupplierCautionMoneyStockOutRepository.GetSupplierCautionMoneyStockOutsBySupplierCautionMoneyID(CurrentEntityID.Value, out totalRecords);
                //rgStockOuts.DataSource = stockOuts;
                //rgStockOuts.VirtualItemCount = totalRecords;
            }

        }

        protected void rgStockOuts_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            var SupplierCautionMoneyStockInId = editableItem.GetDataKeyValue("ID").ToIntOrNull();
            if (SupplierCautionMoneyStockInId.HasValue && SupplierCautionMoneyStockInId.Value > 0)
            {

                rgStockOuts.Rebind();
            }
        }
    }
}