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

namespace ZhongDing.Web.Views.HRM.Editors
{
    public partial class DeptMarketDivisionMaintain : BasePage
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

        private IDeptMarketRepository _PageDeptMarketRepository;
        private IDeptMarketRepository PageDeptMarketRepository
        {
            get
            {
                if (_PageDeptMarketRepository == null)
                    _PageDeptMarketRepository = new DeptMarketRepository();

                return _PageDeptMarketRepository;
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

                LoadCurrentEntity();
            }
        }

        /// <summary>
        /// 绑定部门用户
        /// </summary>
        /// <param name="currentUserID">当前用户ID.</param>
        private void BindDepartmentUsers(int currentUserID)
        {
            //排除已经分配过的用户
            var excludeUserIDs = PageDeptMarketDivisionRepository.GetList(x => x.UserID != currentUserID)
                .Select(x => x.UserID).ToList();

            var deptUsers = PageUsersRepository.GetDropdownItems(new UISearchDropdownItem()
            {
                ExcludeItemValues = excludeUserIDs,
                Extension = new UISearchExtension()
                {
                    DepartmentID = this.OwnerEntityID.HasValue ? this.OwnerEntityID.Value : GlobalConst.INVALID_INT
                }
            });

            rcbxDepartmentUsers.DataSource = deptUsers;
            rcbxDepartmentUsers.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxDepartmentUsers.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxDepartmentUsers.DataBind();

            rcbxDepartmentUsers.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void LoadCurrentEntity()
        {
            if (this.OwnerEntityID.HasValue
                && this.OwnerEntityID > 0)
            {
                var department = PageDepartmentRepository.GetByID(this.OwnerEntityID);

                if (department != null)
                {
                    var selectedMarketIDs = new List<int>();
                    var selectedProductIDs = new List<int>();

                    var deptMarketDivision = PageDeptMarketDivisionRepository.GetByID(this.CurrentEntityID);

                    if (deptMarketDivision != null)
                    {
                        BindDepartmentUsers(deptMarketDivision.UserID);

                        rcbxDepartmentUsers.SelectedValue = deptMarketDivision.UserID.ToString();

                        if (!string.IsNullOrEmpty(deptMarketDivision.MarketID))
                            selectedMarketIDs = deptMarketDivision.MarketID.Split(',').Select(x => Convert.ToInt32(x)).ToList();

                        var uiSearchSelectedMarketsObj = new UISearchDropdownItem();
                        uiSearchSelectedMarketsObj.IncludeItemValues = selectedMarketIDs;
                        uiSearchSelectedMarketsObj.Extension = new UISearchExtension { DepartmentTypeID = department.DepartmentTypeID };

                        var selectedMarkets = PageDeptMarketRepository.GetDropdownItems(uiSearchSelectedMarketsObj);

                        lbxSelectedDeptMarkets.DataSource = selectedMarkets;
                        lbxSelectedDeptMarkets.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                        lbxSelectedDeptMarkets.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                        lbxSelectedDeptMarkets.DataBind();

                        selectedProductIDs = deptMarketDivision.DeptMarketProduct
                            .Where(x => x.IsDeleted == false).Select(x => x.ProductID).ToList();

                        if (selectedProductIDs.Count > 0)
                        {
                            var uiSearchSelectedProductsObj = new UISearchDropdownItem();
                            uiSearchSelectedProductsObj.IncludeItemValues = selectedProductIDs;
                            uiSearchSelectedProductsObj.Extension = new UISearchExtension
                            {
                                DepartmentID = department.ID
                            };

                            var selectedProducts = PageProductRepository.GetDropdownItems(uiSearchSelectedProductsObj);

                            lbxSelectedProducts.DataSource = selectedProducts;
                            lbxSelectedProducts.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                            lbxSelectedProducts.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                            lbxSelectedProducts.DataBind();
                        }
                    }
                    else
                    {
                        BindDepartmentUsers(GlobalConst.INVALID_INT);
                    }

                    UISearchDropdownItem uiSearchAllMarketObj = new UISearchDropdownItem();
                    uiSearchAllMarketObj.Extension = new UISearchExtension { DepartmentTypeID = department.DepartmentTypeID };

                    var allDeptMarkets = PageDeptMarketRepository.GetDropdownItems(uiSearchAllMarketObj)
                        .Where(x => !selectedMarketIDs.Contains(x.ItemValue));

                    lbxAllDeptMarkets.DataSource = allDeptMarkets;
                    lbxAllDeptMarkets.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                    lbxAllDeptMarkets.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                    lbxAllDeptMarkets.DataBind();

                    var uiSearchAllProductsObj = new UISearchDropdownItem();
                    uiSearchAllProductsObj.Extension = new UISearchExtension
                    {
                        DepartmentID = department.ID
                    };

                    var allProducts = PageProductRepository.GetDropdownItems(uiSearchAllProductsObj)
                        .Where(x => !selectedProductIDs.Contains(x.ItemValue)).ToList();

                    lbxAllProducts.DataSource = allProducts;
                    lbxAllProducts.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                    lbxAllProducts.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                    lbxAllProducts.DataBind();

                }
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            DeptMarketDivision deptMarketDivision = null;

            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
                deptMarketDivision = PageDeptMarketDivisionRepository.GetByID(this.CurrentEntityID);

            if (deptMarketDivision == null)
            {
                deptMarketDivision = new DeptMarketDivision();
                PageDeptMarketDivisionRepository.Add(deptMarketDivision);
            }

            deptMarketDivision.UserID = int.Parse(rcbxDepartmentUsers.SelectedValue);

            if (lbxSelectedDeptMarkets.Items.Count > 0)
                deptMarketDivision.MarketID = string.Join(",", lbxSelectedDeptMarkets.Items.Select(x => x.Value));
            else
                deptMarketDivision.MarketID = string.Empty;

            var oldDeptMarketProducts = deptMarketDivision.DeptMarketProduct.Where(x => x.IsDeleted == false);

            if (lbxSelectedProducts.Items.Count > 0)
            {
                foreach (RadListBoxItem item in lbxSelectedProducts.Items)
                {
                    var currentDeptMarketProduct = oldDeptMarketProducts.FirstOrDefault(x => x.ProductID.ToString() == item.Value);

                    if (currentDeptMarketProduct == null)
                    {
                        currentDeptMarketProduct = new DeptMarketProduct();
                        currentDeptMarketProduct.ProductID = int.Parse(item.Value);

                        deptMarketDivision.DeptMarketProduct.Add(currentDeptMarketProduct);
                    }
                }
            }

            //删除之前的未被选择的货品
            foreach (var item in oldDeptMarketProducts)
            {
                if (!lbxSelectedProducts.Items.Any(x => x.Value == item.ProductID.ToString()))
                    item.IsDeleted = true;
            }

            PageDeptMarketDivisionRepository.Save();

            this.Master.BaseNotification.OnClientHidden = "onClientHidden";
            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_CLOSE_WIN);
        }

        protected void cvDepartmentUsers_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!string.IsNullOrEmpty(rcbxDepartmentUsers.Text))
            {
                UISearchDropdownItem uiSearchObj = new UISearchDropdownItem()
                {
                    ItemText = rcbxDepartmentUsers.Text.Trim()
                };

                var directorUsers = PageUsersRepository.GetDropdownItems(uiSearchObj);

                if (directorUsers.Count <= 0)
                    args.IsValid = false;
            }
        }

        protected void btnAllProductSearch_Click(object sender, EventArgs e)
        {
            List<UIDropdownItem> allProducts = new List<UIDropdownItem>();

            var uiSearchAllProductsObj = new UISearchDropdownItem()
            {
                Extension = new UISearchExtension
                {
                    DepartmentID = this.OwnerEntityID.HasValue ? this.OwnerEntityID.Value : GlobalConst.INVALID_INT
                }
            };

            if (!string.IsNullOrEmpty(hdnSearchTextForAllProducts.Value.Trim()))
                uiSearchAllProductsObj.ItemText = hdnSearchTextForAllProducts.Value.Trim();

            var selectedProductIDs = lbxSelectedProducts.Items.Select(x => Convert.ToInt32(x.Value)).ToList();

            allProducts = PageProductRepository.GetDropdownItems(uiSearchAllProductsObj)
                    .Where(x => !selectedProductIDs.Contains(x.ItemValue)).ToList();

            lbxAllProducts.DataSource = allProducts;
            lbxAllProducts.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            lbxAllProducts.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            lbxAllProducts.DataBind();
        }

        protected void btnSearchSelectedProducts_Click(object sender, EventArgs e)
        {
            List<UIDropdownItem> allProducts = new List<UIDropdownItem>();

            var uiSearchAllProductsObj = new UISearchDropdownItem();
            uiSearchAllProductsObj.Extension = new UISearchExtension
            {
                DepartmentID = this.OwnerEntityID.HasValue ? this.OwnerEntityID.Value : GlobalConst.INVALID_INT
            };

            if (!string.IsNullOrEmpty(hdnSearchTextForSelectedProducts.Value.Trim()))
                uiSearchAllProductsObj.ItemText = hdnSearchTextForSelectedProducts.Value.Trim();

            var nonSelectedProductIDs = lbxAllProducts.Items.Select(x => Convert.ToInt32(x.Value)).ToList();

            allProducts = PageProductRepository.GetDropdownItems(uiSearchAllProductsObj)
                    .Where(x => !nonSelectedProductIDs.Contains(x.ItemValue)).ToList();

            lbxSelectedProducts.DataSource = allProducts;
            lbxSelectedProducts.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            lbxSelectedProducts.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            lbxSelectedProducts.DataBind();
        }
    }
}