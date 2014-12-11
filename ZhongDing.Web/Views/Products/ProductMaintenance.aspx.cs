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

namespace ZhongDing.Web.Views.Products
{
    public partial class ProductMaintenance : BasePage
    {
        #region Members

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

        private IProductCategoryRepository _PageProductCategoryRepository;
        private IProductCategoryRepository PageProductCategoryRepository
        {
            get
            {
                if (_PageProductCategoryRepository == null)
                    _PageProductCategoryRepository = new ProductCategoryRepository();

                return _PageProductCategoryRepository;
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

        private ICertificateRepository _PageCertificateRepository;
        private ICertificateRepository PageCertificateRepository
        {
            get
            {
                if (_PageCertificateRepository == null)
                    _PageCertificateRepository = new CertificateRepository();

                return _PageCertificateRepository;
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

        private IProductSpecificationRepository _PageSpecificationRepository;
        private IProductSpecificationRepository PageSpecificationRepository
        {
            get
            {
                if (_PageSpecificationRepository == null)
                    _PageSpecificationRepository = new ProductSpecificationRepository();

                return _PageSpecificationRepository;
            }
        }

        private Product _CurrentEntity;
        private Product CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageProductRepository.GetByID(this.CurrentEntityID);

                return _CurrentEntity;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ProductManage;

            //新增时隐藏其他sections
            if (this.CurrentEntity == null)
                divOtherSections.Visible = false;

            if (!IsPostBack)
            {
                BindProductCategories();

                BindSuppliers();

                BindDepartments();

                LoadCurrentEntity();
            }
        }

        #region Private Methods

        private void BindProductCategories()
        {
            var productCategories = PageProductCategoryRepository.GetDropdownItems();

            ddlProductCategory.DataSource = productCategories;
            ddlProductCategory.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            ddlProductCategory.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            ddlProductCategory.DataBind();
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

        private void BindDepartments()
        {
            var departments = PageDepartmentRepository.GetDropdownItems();

            rcbxDepartment.DataSource = departments;
            rcbxDepartment.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxDepartment.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxDepartment.DataBind();

            rcbxDepartment.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                var product = PageProductRepository.GetByID(this.CurrentEntityID);

                if (product != null)
                {
                    hdnCurrentEntityID.Value = product.ID.ToString();

                    txtProductCode.Text = product.ProductCode;

                    if (product.CategoryID.HasValue && product.CategoryID > 0)
                        ddlProductCategory.SelectedValue = product.CategoryID.ToString();

                    txtProductName.Text = product.ProductName;

                    if (product.SupplierID.HasValue && product.SupplierID > 0)
                        rcbxSupplier.SelectedValue = product.SupplierID.ToString();

                    if (product.Supplier != null)
                        lblProducer.Text = product.Supplier.FactoryName;

                    txtValidDays.Value = (double?)product.ValidDays;

                    cbxIsManagedByBatchNumber.Checked = product.IsManagedByBatchNumber.HasValue
                        ? product.IsManagedByBatchNumber.Value : false;

                    if (product.DepartmentID.HasValue && product.DepartmentID > 0)
                        rcbxDepartment.SelectedValue = product.DepartmentID.ToString();

                    txtSafetyStock.Value = (double?)product.SafetyStock;

                    if (!string.IsNullOrEmpty(ddlProductCategory.SelectedValue))
                    {
                        if (ddlProductCategory.SelectedValue == ((int)EProductCategory.BaseMedicine).ToString())
                            divDepartment.Visible = false;
                        else
                            divDepartment.Visible = true;
                    }
                }
                else
                    btnDelete.Visible = false;
            }
            else
                btnDelete.Visible = false;
        }

        #endregion

        protected void rgProductSpecifications_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            UISearchProductSpecification uiSearchObj = new UISearchProductSpecification()
            {
                ProductID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT,
            };

            int totalRecords;

            var productSpecifications = PageSpecificationRepository.GetUIList(uiSearchObj,
                rgProductSpecifications.CurrentPageIndex, rgProductSpecifications.PageSize, out totalRecords);

            rgProductSpecifications.DataSource = productSpecifications;

            rgProductSpecifications.VirtualItemCount = totalRecords;
        }

        protected void rgProductSpecifications_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                PageSpecificationRepository.DeleteByID(id);
                PageSpecificationRepository.Save();
            }

            rgProductSpecifications.Rebind();
        }

        protected void rgCertificates_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            UISearchCertificate uiSearchObj = new UISearchCertificate()
            {
                OwnerEntityID = this.CurrentEntityID.Value,
                OwnerTypeID = (int)EOwnerType.Product
            };

            int totalRecords;

            rgCertificates.DataSource = PageCertificateRepository
                .GetUIList(uiSearchObj, rgCertificates.CurrentPageIndex, rgCertificates.PageSize, out totalRecords);

            rgCertificates.VirtualItemCount = totalRecords;
        }

        protected void rgCertificates_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("OwnerEntityID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    DbModelContainer db = unitOfWork.GetDbModel();

                    ICertificateRepository certificateRepository = new CertificateRepository();
                    ProductCertificateRepository productCertificateRepository = new ProductCertificateRepository();

                    certificateRepository.SetDbModel(db);
                    productCertificateRepository.SetDbModel(db);

                    var productCertificate = productCertificateRepository.GetByID(id);

                    if (productCertificate != null)
                        certificateRepository.Delete(productCertificate.Certificate);

                    productCertificateRepository.Delete(productCertificate);

                    unitOfWork.SaveChanges();
                }
            }

            rgCertificates.Rebind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlProductCategory.SelectedValue)
                && ddlProductCategory.SelectedValue != ((int)EProductCategory.BaseMedicine).ToString()
                && string.IsNullOrEmpty(rcbxDepartment.SelectedValue))
            {
                cvRequiredDepartment.IsValid = false;
            }

            if (!IsValid) return;

            Product product = null;

            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                product = PageProductRepository.GetByID(this.CurrentEntityID);

            if (product == null)
            {
                product = new Product()
                {
                    CompanyID = CurrentUser.CompanyID
                };

                PageProductRepository.Add(product);
            }

            if (product != null)
            {
                product.ProductCode = txtProductCode.Text.Trim();
                product.ProductName = txtProductName.Text.Trim();

                if (!string.IsNullOrEmpty(ddlProductCategory.SelectedValue))
                    product.CategoryID = int.Parse(ddlProductCategory.SelectedValue);

                if (!string.IsNullOrEmpty(rcbxSupplier.SelectedValue))
                    product.SupplierID = Convert.ToInt32(rcbxSupplier.SelectedValue);

                product.ValidDays = (int?)txtValidDays.Value;
                product.IsManagedByBatchNumber = cbxIsManagedByBatchNumber.Checked;

                if (!string.IsNullOrEmpty(rcbxDepartment.SelectedValue))
                    product.DepartmentID = Convert.ToInt32(rcbxDepartment.SelectedValue);

                if (product.CategoryID == (int)EProductCategory.BaseMedicine)
                    product.DepartmentID = null;

                product.SafetyStock = (int?)txtSafetyStock.Value;

                PageProductRepository.Save();

                hdnCurrentEntityID.Value = product.ID.ToString();

                if (this.CurrentEntityID.HasValue
                    && this.CurrentEntityID > 0)
                {
                    this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
                }
                else
                {
                    this.Master.BaseNotification.OnClientHidden = "refreshMaintenancePage";
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REFRESH);
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    DbModelContainer db = unitOfWork.GetDbModel();

                    IProductRepository productRepository = new ProductRepository();
                    IProductSpecificationRepository productSpecificationRepository = new ProductSpecificationRepository();
                    IProductCertificateRepository productCertificateRepository = new ProductCertificateRepository();
                    ICertificateRepository certificateRepository = new CertificateRepository();
                    IProductBasicPriceRepository basicPriceRepository = new ProductBasicPriceRepository();
                    IProductHighPriceRepository highPriceRepository = new ProductHighPriceRepository();
                    IProductDBPolicyPriceRepository dBPolicyPriceRepository = new ProductDBPolicyPriceRepository();

                    productRepository.SetDbModel(db);
                    productSpecificationRepository.SetDbModel(db);
                    productCertificateRepository.SetDbModel(db);
                    certificateRepository.SetDbModel(db);
                    basicPriceRepository.SetDbModel(db);
                    highPriceRepository.SetDbModel(db);
                    dBPolicyPriceRepository.SetDbModel(db);

                    var product = productRepository.GetByID(this.CurrentEntityID);

                    if (product != null)
                    {
                        foreach (var productSpecification in product.ProductSpecification)
                        {
                            if (productSpecification != null)
                                productSpecificationRepository.Delete(productSpecification);
                        }

                        foreach (var productCertificate in product.ProductCertificate)
                        {
                            if (productCertificate != null)
                            {
                                if (productCertificate.Certificate != null)
                                    certificateRepository.Delete(productCertificate.Certificate);

                                productCertificateRepository.Delete(productCertificate);
                            }
                        }

                        foreach (var basicPrice in product.ProductBasicPrice)
                        {
                            if (basicPrice != null)
                                basicPriceRepository.Delete(basicPrice);
                        }

                        foreach (var highPrice in product.ProductHighPrice)
                        {
                            if (highPrice != null)
                                highPriceRepository.Delete(highPrice);
                        }

                        foreach (var dBPolicyPrice in product.ProductDBPolicyPrice)
                        {
                            if (dBPolicyPrice != null)
                                dBPolicyPriceRepository.Delete(dBPolicyPrice);
                        }

                        productRepository.Delete(product);
                    }

                    unitOfWork.SaveChanges();
                }

                this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_DELETED_REDIRECT);
            }
        }

        protected void rcbxSupplier_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            UIDropdownItem dataItem = (UIDropdownItem)e.Item.DataItem;
            System.Reflection.PropertyInfo factoryNamePropertyInfo = dataItem.Extension.GetType().GetProperty("FactoryName");

            if (factoryNamePropertyInfo != null)
            {
                e.Item.Attributes["FactoryName"] = factoryNamePropertyInfo.GetValue(dataItem.Extension).ToString();
            }
        }

        protected void cvProductCode_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (PageProductRepository.GetList(x => x.IsDeleted == false && x.ID != this.CurrentEntityID
                && x.ProductCode.ToLower() == txtProductCode.Text.Trim().ToLower()).Count() > 0)
            {
                args.IsValid = false;
            }
        }

        protected void cvSupplier_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!string.IsNullOrEmpty(rcbxSupplier.Text))
            {
                UISearchDropdownItem uiSearchObj = new UISearchDropdownItem()
                {
                    ItemText = rcbxSupplier.Text.Trim()
                };

                var suppliers = PageSupplierRepository.GetDropdownItems(uiSearchObj);

                if (suppliers.Count <= 0)
                    args.IsValid = false;
            }
        }

        protected void cvDepartment_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!string.IsNullOrEmpty(rcbxDepartment.Text))
            {
                UISearchDropdownItem uiSearchObj = new UISearchDropdownItem()
                {
                    ItemText = rcbxDepartment.Text.Trim()
                };

                var departments = PageDepartmentRepository.GetDropdownItems(uiSearchObj);

                if (departments.Count <= 0)
                    args.IsValid = false;
            }
        }

        protected void ddlProductCategory_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlProductCategory.SelectedValue))
            {
                if (ddlProductCategory.SelectedValue == ((int)EProductCategory.BaseMedicine).ToString())
                    divDepartment.Visible = false;
                else
                    divDepartment.Visible = true;
            }
        }

    }
}