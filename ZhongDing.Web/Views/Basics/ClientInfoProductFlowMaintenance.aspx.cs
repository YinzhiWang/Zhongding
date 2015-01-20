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
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Basics
{
    public partial class ClientInfoProductFlowMaintenance : BasePage
    {
        #region Members

        private IClientInfoProductSettingRepository _PageClientInfoProductSettingRepository;
        private IClientInfoProductSettingRepository PageClientInfoProductSettingRepository
        {
            get
            {
                if (_PageClientInfoProductSettingRepository == null)
                    _PageClientInfoProductSettingRepository = new ClientInfoProductSettingRepository();

                return _PageClientInfoProductSettingRepository;
            }
        }

        private IClientUserRepository _PageClientUserRepository;
        private IClientUserRepository PageClientUserRepository
        {
            get
            {
                if (_PageClientUserRepository == null)
                    _PageClientUserRepository = new ClientUserRepository();

                return _PageClientUserRepository;
            }
        }

        private IClientCompanyRepository _PageClientCompanyRepository;
        private IClientCompanyRepository PageClientCompanyRepository
        {
            get
            {
                if (_PageClientCompanyRepository == null)
                    _PageClientCompanyRepository = new ClientCompanyRepository();

                return _PageClientCompanyRepository;
            }
        }

        private IClientInfoRepository _PageClientInfoRepository;
        private IClientInfoRepository PageClientInfoRepository
        {
            get
            {
                if (_PageClientInfoRepository == null)
                    _PageClientInfoRepository = new ClientInfoRepository();

                return _PageClientInfoRepository;
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

        private IDepartmentRepository _PageDepartmentRepository;
        private IDepartmentRepository PageDepartmentRepository
        {
            get
            {
                if (_PageDepartmentRepository == null)
                    _PageDepartmentRepository = new DepartmentRepository();

                return _PageDepartmentRepository;
            }
        }

        private IDeptMarketDivisionRepository _PageDeptMarketDivisionRepository;
        private IDeptMarketDivisionRepository PageDeptMarketDivisionRepository
        {
            get
            {
                if (_PageDeptMarketDivisionRepository == null)
                    _PageDeptMarketDivisionRepository = new DeptMarketDivisionRepository();

                return _PageDeptMarketDivisionRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ClientInfoProductFlowManage;

            if (!IsPostBack)
            {
                BindClientUsers();

                BindProducts();

                BindDepartments();

                LoadCurrentEntity();
            }
        }

        #region Private Methods

        private void BindClientUsers()
        {
            var clientUsers = PageClientUserRepository.GetDropdownItems();
            rcbxClientUser.DataSource = clientUsers;
            rcbxClientUser.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxClientUser.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxClientUser.DataBind();

            rcbxClientUser.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindClientCompanies()
        {
            ddlClientCompany.Items.Clear();

            var uiSearchObj = new UISearchDropdownItem();

            if (!string.IsNullOrEmpty(rcbxClientUser.SelectedValue))
            {
                int clientUserID;
                if (int.TryParse(rcbxClientUser.SelectedValue, out clientUserID))
                    uiSearchObj.Extension = new UISearchExtension { ClientUserID = clientUserID };
            }

            var clientCompanies = PageClientCompanyRepository.GetDropdownItems(uiSearchObj);
            ddlClientCompany.DataSource = clientCompanies;
            ddlClientCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            ddlClientCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            ddlClientCompany.DataBind();

            ddlClientCompany.Items.Insert(0, new DropDownListItem("", ""));
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
                ddlProductSpecification.Items.Clear();

                var productSpecifications = PageProductSpecificationRepository.GetDropdownItems(new UISearchDropdownItem()
                {
                    Extension = new UISearchExtension { ProductID = productID }
                });

                ddlProductSpecification.DataSource = productSpecifications;
                ddlProductSpecification.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                ddlProductSpecification.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                ddlProductSpecification.DataBind();
            }
        }

        private void BindDepartments()
        {
            var departments = PageDepartmentRepository.GetDropdownItems();

            rcbxDepartment.DataSource = departments;
            rcbxDepartment.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxDepartment.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxDepartment.DataBind();

            rcbxDepartment.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindDeptMarketDivisions(int departmentID)
        {
            var deptMarketDivisions = PageDeptMarketDivisionRepository.GetDropdownItems(new UISearchDropdownItem()
            {
                Extension = new UISearchExtension { DepartmentID = departmentID }
            });

            ddlDeptMarketDivision.DataSource = deptMarketDivisions;
            ddlDeptMarketDivision.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            ddlDeptMarketDivision.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            ddlDeptMarketDivision.DataBind();

            ddlDeptMarketDivision.Items.Insert(0, new DropDownListItem("", ""));
        }

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                var currentEntity = PageClientInfoProductSettingRepository.GetByID(this.CurrentEntityID);

                if (currentEntity != null)
                {
                    if (currentEntity.ClientInfo != null)
                    {
                        if (currentEntity.ClientInfo.ClientUserID > 0)
                            rcbxClientUser.SelectedValue = currentEntity.ClientInfo.ClientUserID.ToString();

                        BindClientCompanies();

                        if (currentEntity.ClientInfo.ClientCompanyID > 0)
                            ddlClientCompany.SelectedValue = currentEntity.ClientInfo.ClientCompanyID.ToString();
                    }

                    if (currentEntity.ProductID > 0)
                    {
                        rcbxProduct.SelectedValue = currentEntity.ProductID.ToString();

                        BindProductSpecifications(currentEntity.ProductID);

                        ddlProductSpecification.SelectedValue = currentEntity.ProductSpecificationID.ToString();

                        if (currentEntity.Product != null && currentEntity.Product.Supplier != null)
                            lblFactoryName.Text = currentEntity.Product.Supplier.FactoryName;
                    }

                    txtHighPrice.DbValue = currentEntity.HighPrice;
                    txtBasicPrice.DbValue = currentEntity.BasicPrice;
                    txtMonthlyTask.DbValue = currentEntity.MonthlyTask;
                    txtRefundPrice.DbValue = currentEntity.RefundPrice;

                    if (currentEntity.UseFlowData)
                        radioIsUseFlowData.Checked = true;
                    else
                    {
                        radioIsNotUseFlowData.Checked = true;

                        if (currentEntity.DepartmentID.HasValue && currentEntity.DepartmentID > 0)
                        {
                            rcbxDepartment.SelectedValue = currentEntity.DepartmentID.ToString();

                            BindDeptMarketDivisions(currentEntity.DepartmentID.Value);

                            ddlDeptMarketDivision.SelectedValue = currentEntity.DeptMarketID.ToString();
                        }
                    }
                }
                else
                    btnDelete.Visible = false;
            }
            else
                btnDelete.Visible = false;
        }

        #endregion

        protected void rcbxProduct_ItemDataBound(object sender, Telerik.Web.UI.RadComboBoxItemEventArgs e)
        {
            var dataItem = (UIDropdownItem)e.Item.DataItem;

            if (dataItem != null)
                e.Item.Attributes["Extension"] = Utility.JsonSeralize(dataItem.Extension);
        }

        protected void rcbxProduct_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value))
            {
                int productID;

                if (int.TryParse(e.Value, out productID))
                    BindProductSpecifications(productID);
            }
        }

        protected void rcbxClientUser_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindClientCompanies();
        }

        protected void rcbxDepartment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value))
            {
                int departmentID;

                if (int.TryParse(e.Value, out departmentID))
                    BindDeptMarketDivisions(departmentID);
            }
        }

        protected void cvDepartment_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (radioIsNotUseFlowData.Checked && string.IsNullOrEmpty(args.Value))
                args.IsValid = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (radioIsNotUseFlowData.Checked)
            {
                if (string.IsNullOrEmpty(rcbxDepartment.SelectedValue))
                    cvDepartment.IsValid = false;

                if (string.IsNullOrEmpty(ddlDeptMarketDivision.SelectedValue))
                    cvDeptMarketDivision.IsValid = false;
            }

            if (!IsValid) return;

            ClientInfoProductSetting currentEntity = null;

            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                currentEntity = PageClientInfoProductSettingRepository.GetByID(this.CurrentEntityID);

            if (currentEntity == null)
            {
                currentEntity = new ClientInfoProductSetting();
                PageClientInfoProductSettingRepository.Add(currentEntity);
            }

            if (!string.IsNullOrEmpty(rcbxClientUser.SelectedValue)
                && !string.IsNullOrEmpty(ddlClientCompany.SelectedValue))
            {
                var clientUserID = int.Parse(rcbxClientUser.SelectedValue);
                var clientCompanyID = int.Parse(ddlClientCompany.SelectedValue);

                var curClientInfo = PageClientInfoRepository.GetList(x => x.ClientUserID == clientUserID
                        && x.ClientCompanyID == clientCompanyID).FirstOrDefault();

                if (curClientInfo != null)
                {
                    currentEntity.ClientInfoID = curClientInfo.ID;
                    currentEntity.ProductID = int.Parse(rcbxProduct.SelectedValue);
                    currentEntity.ProductSpecificationID = int.Parse(ddlProductSpecification.SelectedValue);
                    currentEntity.HighPrice = (decimal?)txtHighPrice.Value;
                    currentEntity.BasicPrice = (decimal?)txtBasicPrice.Value;

                    if (radioIsUseFlowData.Checked)
                    {
                        currentEntity.UseFlowData = true;
                        currentEntity.DepartmentID = null;
                        currentEntity.DeptMarketID = null;
                    }
                    else if (radioIsNotUseFlowData.Checked)
                    {
                        currentEntity.UseFlowData = false;
                        currentEntity.DepartmentID = int.Parse(rcbxDepartment.SelectedValue);
                        currentEntity.DeptMarketID = int.Parse(ddlDeptMarketDivision.SelectedValue);
                    }

                    currentEntity.MonthlyTask = (int?)txtMonthlyTask.Value;
                    currentEntity.RefundPrice = (decimal?)txtRefundPrice.Value;

                    PageClientInfoProductSettingRepository.Save();

                    this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
                }
                else
                {
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_FAILED_SAEVED);
                }
            }
            else
            {
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_FAILED_SAEVED);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
            {
                var currentEntity = PageClientInfoProductSettingRepository.GetByID(this.CurrentEntityID);

                if (currentEntity != null)
                {
                    PageClientInfoProductSettingRepository.Delete(currentEntity);
                    PageClientInfoProductSettingRepository.Save();

                    this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                }
            }
        }

    }
}