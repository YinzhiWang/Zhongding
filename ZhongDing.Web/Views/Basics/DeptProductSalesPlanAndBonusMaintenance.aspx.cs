using System;
using System.Collections;
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
    public partial class DeptProductSalesPlanAndBonusMaintenance : BasePage
    {
        #region Consts

        private const string SESSION_DEPT_INSIDE_SALES_BONUS = "DeptInsideSalesBonus";
        private const string SESSION_DEPT_OUTSIDE_SALES_BONUS = "DeptOutsideSalesBonus";

        #endregion

        #region Members

        private IDeptProductSalesPlanRepository _PageDeptProductSalesPlanRepository;
        private IDeptProductSalesPlanRepository PageDeptProductSalesPlanRepository
        {
            get
            {
                if (_PageDeptProductSalesPlanRepository == null)
                    _PageDeptProductSalesPlanRepository = new DeptProductSalesPlanRepository();

                return _PageDeptProductSalesPlanRepository;
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

        private IDeptProductSalesBonusRepository _PageDeptProductSalesBonusRepository;
        private IDeptProductSalesBonusRepository PageDeptProductSalesBonusRepository
        {
            get
            {
                if (_PageDeptProductSalesBonusRepository == null)
                    _PageDeptProductSalesBonusRepository = new DeptProductSalesBonusRepository();

                return _PageDeptProductSalesBonusRepository;
            }
        }

        private IDeptProductRecordRepository _PageDeptProductRecordRepository;
        private IDeptProductRecordRepository PageDeptProductRecordRepository
        {
            get
            {
                if (_PageDeptProductRecordRepository == null)
                    _PageDeptProductRecordRepository = new DeptProductRecordRepository();

                return _PageDeptProductRecordRepository;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.DeptProductSalesPlanAndBonusManage;

            if (!IsPostBack)
            {
              
                BindDepartments();

                LoadCurrentEntity();
                base.PermissionOptionCheckButtonDelete(btnDelete);
            }

        }

        #region Private Methods

        private void BindDepartments()
        {
            var departments = PageDepartmentRepository.GetDropdownItems();

            rcbxDepartment.DataSource = departments;
            rcbxDepartment.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxDepartment.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxDepartment.DataBind();

            rcbxDepartment.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindProducts(int departmentID)
        {
            rcbxProduct.ClearSelection();

            var uiSearchObj = new UISearchDropdownItem()
            {
                Extension = new UISearchExtension()
                {
                    DepartmentID = departmentID
                }
            };

            var products = PageProductRepository.GetDropdownItems(uiSearchObj);

            rcbxProduct.DataSource = products;
            rcbxProduct.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxProduct.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxProduct.DataBind();

            rcbxProduct.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                var currentEntity = PageDeptProductSalesPlanRepository.GetByID(this.CurrentEntityID);

                if (currentEntity != null)
                {
                    hdnCurrentEntityID.Value = currentEntity.ID.ToString();

                    rcbxDepartment.SelectedValue = currentEntity.DepartmentID.ToString();

                    BindProducts(currentEntity.DepartmentID);

                    rcbxProduct.SelectedValue = currentEntity.ProductID.ToString();

                    if (currentEntity.IsFixedOfInside)
                    {
                        radioIsFixedOfInside.Checked = true;
                        txtFixedRatioOfInside.DbValue = currentEntity.FixedRatioOfInside;
                    }
                    else
                        radioIsNotFixedOfInside.Checked = true;

                    if (currentEntity.IsFixedOfOutside)
                    {
                        radioIsFixedOfOutside.Checked = true;
                        txtFixedRatioOfOutside.DbValue = currentEntity.FixedRatioOfOutside;
                    }
                    else
                        radioIsNotFixedOfOutside.Checked = true;
                }

                var uiSearchObj = new UISearchDeptProductSalesBonus()
                {
                    SalesPlanID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT,
                };

                var uiDeptProductSalesBonus = PageDeptProductSalesBonusRepository.GetUIList(uiSearchObj);

                //初始化基础提成浮动比例设置
                Session[SESSION_DEPT_INSIDE_SALES_BONUS] = uiDeptProductSalesBonus
                    .Where(x => x.SalesPlanTypeID == (int)ESalesPlanType.Inside).ToList();

                //初始化超出提成浮动比例设置
                Session[SESSION_DEPT_OUTSIDE_SALES_BONUS] = uiDeptProductSalesBonus
                    .Where(x => x.SalesPlanTypeID == (int)ESalesPlanType.Outside).ToList();
            }
            else
            {
                btnDelete.Visible = false;
                divOtherSections.Visible = false;

                Session[SESSION_DEPT_INSIDE_SALES_BONUS] = new List<UIDeptProductSalesBonus>();
                Session[SESSION_DEPT_OUTSIDE_SALES_BONUS] = new List<UIDeptProductSalesBonus>();
            }
        }

        private List<UIDeptProductSalesBonus> GetSalesBonusListFromSession(ESalesPlanType salesPlanType)
        {
            List<UIDeptProductSalesBonus> uiDeptProductSalesBonus = new List<UIDeptProductSalesBonus>();
            switch (salesPlanType)
            {
                case ESalesPlanType.Inside:
                    uiDeptProductSalesBonus = (List<UIDeptProductSalesBonus>)Session[SESSION_DEPT_INSIDE_SALES_BONUS];
                    break;

                case ESalesPlanType.Outside:
                    uiDeptProductSalesBonus = (List<UIDeptProductSalesBonus>)Session[SESSION_DEPT_OUTSIDE_SALES_BONUS];
                    break;
            }

            return uiDeptProductSalesBonus;
        }

        #endregion

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

        protected void cvProduct_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!string.IsNullOrEmpty(rcbxProduct.Text))
            {
                UISearchDropdownItem uiSearchObj = new UISearchDropdownItem()
                {
                    ItemText = rcbxProduct.Text.Trim()
                };

                var products = PageProductRepository.GetDropdownItems(uiSearchObj);

                if (products.Count <= 0)
                    args.IsValid = false;
            }
        }

        protected void rcbxDepartment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value))
            {
                int departmentID;

                if (int.TryParse(e.Value, out departmentID))
                    BindProducts(departmentID);
            }
        }

        #region 基础提成比例

        protected void rgInsideFloatRatios_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var uiEntities = GetSalesBonusListFromSession(ESalesPlanType.Inside)
                .Where(x => x.IsDeleted == false).ToList();

            rgInsideFloatRatios.DataSource = uiEntities;
        }

        protected void rgInsideFloatRatios_InsertCommand(object sender, GridCommandEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;

                var ddlCompareOperator = dataItem.FindControl("ddlCompareOperator") as RadDropDownList;
                var txtSalesPrice = dataItem.FindControl("txtSalesPrice") as RadNumericTextBox;
                var txtBonusRatio = dataItem.FindControl("txtBonusRatio") as RadNumericTextBox;

                var uiEntities = GetSalesBonusListFromSession(ESalesPlanType.Inside);

                int minEntityID = 0;
                var newEntities = uiEntities.Where(x => x.ID < 0);
                if (newEntities.Count() > 0)
                    minEntityID = newEntities.Min(x => x.ID);
                minEntityID = minEntityID - 1;

                uiEntities.Add(new UIDeptProductSalesBonus()
                {
                    ID = minEntityID,
                    SalesPlanTypeID = (int)ESalesPlanType.Inside,
                    CompareOperatorID = Convert.ToInt32(ddlCompareOperator.SelectedValue),
                    CompareOperator = ddlCompareOperator.SelectedText,
                    SalesPrice = (decimal?)txtSalesPrice.Value,
                    BonusRatio = Convert.ToDouble(txtBonusRatio.DbValue),
                });

                Session[SESSION_DEPT_INSIDE_SALES_BONUS] = uiEntities;

                rgInsideFloatRatios.Rebind();
            }
        }

        protected void rgInsideFloatRatios_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            var editableItem = ((GridEditableItem)e.Item);
            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;

                    var ddlCompareOperator = dataItem.FindControl("ddlCompareOperator") as RadDropDownList;
                    var txtSalesPrice = dataItem.FindControl("txtSalesPrice") as RadNumericTextBox;
                    var txtBonusRatio = dataItem.FindControl("txtBonusRatio") as RadNumericTextBox;

                    var uiEntities = GetSalesBonusListFromSession(ESalesPlanType.Inside);

                    var uiEntity = uiEntities.FirstOrDefault(x => x.ID == id);

                    if (uiEntity != null)
                    {
                        uiEntity.CompareOperatorID = Convert.ToInt32(ddlCompareOperator.SelectedValue);
                        uiEntity.CompareOperator = ddlCompareOperator.SelectedText;
                        uiEntity.SalesPrice = (decimal?)txtSalesPrice.Value;
                        uiEntity.BonusRatio = Convert.ToDouble(txtBonusRatio.DbValue);

                        Session[SESSION_DEPT_INSIDE_SALES_BONUS] = uiEntities;
                    }

                    rgInsideFloatRatios.Rebind();
                }
            }
        }

        protected void rgInsideFloatRatios_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var editableItem = ((GridEditableItem)e.Item);
            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                var uiEntities = GetSalesBonusListFromSession(ESalesPlanType.Inside);

                var uiEntity = uiEntities.FirstOrDefault(x => x.ID == id);

                if (uiEntity != null)
                {
                    if (id > 0)
                        uiEntity.IsDeleted = true;
                    else
                        uiEntities.Remove(uiEntity);

                    Session[SESSION_DEPT_INSIDE_SALES_BONUS] = uiEntities;
                }

                rgInsideFloatRatios.Rebind();
            }
        }

        protected void rgInsideFloatRatios_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.EditItem && e.Item.ItemIndex >= 0)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;

                UIDeptProductSalesBonus uiEntity = (UIDeptProductSalesBonus)gridDataItem.DataItem;

                var ddlCompareOperator = (RadDropDownList)e.Item.FindControl("ddlCompareOperator");

                if (ddlCompareOperator != null)
                {
                    ddlCompareOperator.SelectedText = uiEntity.CompareOperator;
                }
            }
        }

        #endregion

        #region 超出提成比例

        protected void rgOutsideFloatRatios_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var uiEntities = GetSalesBonusListFromSession(ESalesPlanType.Outside)
                .Where(x => x.IsDeleted == false).ToList();

            rgOutsideFloatRatios.DataSource = uiEntities;
        }

        protected void rgOutsideFloatRatios_InsertCommand(object sender, GridCommandEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;

                var ddlCompareOperator = dataItem.FindControl("ddlCompareOperator") as RadDropDownList;
                var txtSalesPrice = dataItem.FindControl("txtSalesPrice") as RadNumericTextBox;
                var txtBonusRatio = dataItem.FindControl("txtBonusRatio") as RadNumericTextBox;

                var uiEntities = GetSalesBonusListFromSession(ESalesPlanType.Outside);

                int minEntityID = 0;
                var newEntities = uiEntities.Where(x => x.ID < 0);
                if (newEntities.Count() > 0)
                    minEntityID = newEntities.Min(x => x.ID);
                minEntityID = minEntityID - 1;

                uiEntities.Add(new UIDeptProductSalesBonus()
                {
                    ID = minEntityID,
                    SalesPlanTypeID = (int)ESalesPlanType.Inside,
                    CompareOperatorID = Convert.ToInt32(ddlCompareOperator.SelectedValue),
                    CompareOperator = ddlCompareOperator.SelectedText,
                    SalesPrice = (decimal?)txtSalesPrice.Value,
                    BonusRatio = Convert.ToDouble(txtBonusRatio.DbValue),
                });

                Session[SESSION_DEPT_OUTSIDE_SALES_BONUS] = uiEntities;

                rgOutsideFloatRatios.Rebind();
            }
        }

        protected void rgOutsideFloatRatios_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            var editableItem = ((GridEditableItem)e.Item);
            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;

                    var ddlCompareOperator = dataItem.FindControl("ddlCompareOperator") as RadDropDownList;
                    var txtSalesPrice = dataItem.FindControl("txtSalesPrice") as RadNumericTextBox;
                    var txtBonusRatio = dataItem.FindControl("txtBonusRatio") as RadNumericTextBox;

                    var uiEntities = GetSalesBonusListFromSession(ESalesPlanType.Outside);

                    var uiEntity = uiEntities.FirstOrDefault(x => x.ID == id);

                    if (uiEntity != null)
                    {
                        uiEntity.CompareOperatorID = Convert.ToInt32(ddlCompareOperator.SelectedValue);
                        uiEntity.CompareOperator = ddlCompareOperator.SelectedText;
                        uiEntity.SalesPrice = (decimal?)txtSalesPrice.Value;
                        uiEntity.BonusRatio = Convert.ToDouble(txtBonusRatio.DbValue);

                        Session[SESSION_DEPT_OUTSIDE_SALES_BONUS] = uiEntities;
                    }

                    rgOutsideFloatRatios.Rebind();
                }
            }
        }

        protected void rgOutsideFloatRatios_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var editableItem = ((GridEditableItem)e.Item);
            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                var uiEntities = GetSalesBonusListFromSession(ESalesPlanType.Outside);

                var uiEntity = uiEntities.FirstOrDefault(x => x.ID == id);

                if (uiEntity != null)
                {
                    if (id > 0)
                        uiEntity.IsDeleted = true;
                    else
                        uiEntities.Remove(uiEntity);

                    Session[SESSION_DEPT_OUTSIDE_SALES_BONUS] = uiEntities;
                }

                rgOutsideFloatRatios.Rebind();
            }
        }

        protected void rgOutsideFloatRatios_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.EditItem && e.Item.ItemIndex >= 0)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;

                UIDeptProductSalesBonus uiEntity = (UIDeptProductSalesBonus)gridDataItem.DataItem;

                var ddlCompareOperator = (RadDropDownList)e.Item.FindControl("ddlCompareOperator");

                if (ddlCompareOperator != null)
                {
                    ddlCompareOperator.SelectedText = uiEntity.CompareOperator;
                }
            }
        }

        #endregion

        protected void rgSalesRecords_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchDeptProductRecord();

            if (!string.IsNullOrEmpty(rcbxDepartment.SelectedValue)
                && !string.IsNullOrEmpty(rcbxProduct.SelectedValue))
            {
                uiSearchObj.DepartmentID = Convert.ToInt32(rcbxDepartment.SelectedValue);
                uiSearchObj.ProductID = Convert.ToInt32(rcbxProduct.SelectedValue);
            }
            else
            {
                uiSearchObj.DepartmentID = GlobalConst.INVALID_INT;
                uiSearchObj.ProductID = GlobalConst.INVALID_INT;
            }

            int totalRecords;

            var uiEntities = PageDeptProductRecordRepository.GetUIList(uiSearchObj, rgSalesRecords.CurrentPageIndex, rgSalesRecords.PageSize, out totalRecords);

            rgSalesRecords.DataSource = uiEntities;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (radioIsFixedOfInside.Checked && !txtFixedRatioOfInside.Value.HasValue)
                cvFixedRatioOfInside.IsValid = false;

            if (radioIsFixedOfOutside.Checked && !txtFixedRatioOfOutside.Value.HasValue)
                cvFixedRatioOfOutside.IsValid = false;

            if (!IsValid) return;

            DepartmentProductSalesPlan currentEntity = null;

            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                currentEntity = PageDeptProductSalesPlanRepository.GetByID(this.CurrentEntityID);

            if (currentEntity == null)
            {
                currentEntity = new DepartmentProductSalesPlan();
                PageDeptProductSalesPlanRepository.Add(currentEntity);
            }

            currentEntity.DepartmentID = Convert.ToInt32(rcbxDepartment.SelectedValue);
            currentEntity.ProductID = Convert.ToInt32(rcbxProduct.SelectedValue);

            #region 处理基础提成比例

            if (radioIsFixedOfInside.Checked)
            {
                currentEntity.IsFixedOfInside = true;
                currentEntity.FixedRatioOfInside = Convert.ToDouble(txtFixedRatioOfInside.DbValue);

                foreach (var salesBonus in currentEntity.DepartmentProductSalesBonus
                    .Where(x => x.SalesPlanTypeID == (int)ESalesPlanType.Inside))
                {
                    salesBonus.IsDeleted = true;
                }
            }
            else if (radioIsNotFixedOfInside.Checked)
            {
                currentEntity.IsFixedOfInside = false;
                currentEntity.FixedRatioOfInside = null;

                var insideSalesBonusList = GetSalesBonusListFromSession(ESalesPlanType.Inside);

                foreach (var salesBonus in insideSalesBonusList)
                {
                    if (salesBonus.ID > 0)
                    {
                        var dbSalesBonus = currentEntity.DepartmentProductSalesBonus.FirstOrDefault(x => x.ID == salesBonus.ID);

                        if (dbSalesBonus != null)
                        {
                            dbSalesBonus.CompareOperatorID = salesBonus.CompareOperatorID;
                            dbSalesBonus.SalesPrice = salesBonus.SalesPrice;
                            dbSalesBonus.BonusRatio = salesBonus.BonusRatio;
                            dbSalesBonus.IsDeleted = salesBonus.IsDeleted;
                        }
                    }
                    else
                    {
                        var dbSalesBonus = new DepartmentProductSalesBonus()
                        {
                            SalesPlanTypeID = (int)ESalesPlanType.Inside,
                            CompareOperatorID = salesBonus.CompareOperatorID,
                            SalesPrice = salesBonus.SalesPrice,
                            BonusRatio = salesBonus.BonusRatio
                        };

                        currentEntity.DepartmentProductSalesBonus.Add(dbSalesBonus);
                    }

                }
            }

            #endregion

            #region 处理超出提成比例

            if (radioIsFixedOfOutside.Checked)
            {
                currentEntity.IsFixedOfOutside = true;
                currentEntity.FixedRatioOfOutside = Convert.ToDouble(txtFixedRatioOfOutside.DbValue);

                foreach (var salesBonus in currentEntity.DepartmentProductSalesBonus
                    .Where(x => x.SalesPlanTypeID == (int)ESalesPlanType.Outside))
                {
                    salesBonus.IsDeleted = true;
                }
            }
            else if (radioIsNotFixedOfOutside.Checked)
            {
                currentEntity.IsFixedOfOutside = false;
                currentEntity.FixedRatioOfOutside = null;

                var outsideSalesBonusList = GetSalesBonusListFromSession(ESalesPlanType.Outside);

                foreach (var salesBonus in outsideSalesBonusList)
                {
                    if (salesBonus.ID > 0)
                    {
                        var dbSalesBonus = currentEntity.DepartmentProductSalesBonus.FirstOrDefault(x => x.ID == salesBonus.ID);

                        if (dbSalesBonus != null)
                        {
                            dbSalesBonus.CompareOperatorID = salesBonus.CompareOperatorID;
                            dbSalesBonus.SalesPrice = salesBonus.SalesPrice;
                            dbSalesBonus.BonusRatio = salesBonus.BonusRatio;
                            dbSalesBonus.IsDeleted = salesBonus.IsDeleted;
                        }
                    }
                    else
                    {
                        var dbSalesBonus = new DepartmentProductSalesBonus()
                        {
                            SalesPlanTypeID = (int)ESalesPlanType.Outside,
                            CompareOperatorID = salesBonus.CompareOperatorID,
                            SalesPrice = salesBonus.SalesPrice,
                            BonusRatio = salesBonus.BonusRatio
                        };

                        currentEntity.DepartmentProductSalesBonus.Add(dbSalesBonus);
                    }
                }
            }

            #endregion

            PageDeptProductSalesPlanRepository.Save();

            hdnCurrentEntityID.Value = currentEntity.ID.ToString();

            this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
            {
                var currentEntity = PageDeptProductSalesPlanRepository.GetByID(this.CurrentEntityID);

                if (currentEntity != null)
                {
                    foreach (var salesBonus in currentEntity.DepartmentProductSalesBonus)
                    {
                        salesBonus.IsDeleted = true;
                    }

                    currentEntity.IsDeleted = true;

                    PageDeptProductSalesPlanRepository.Save();

                    this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_DELETED_REDIRECT);
                }
            }

        }

        protected override EPermission PagePermissionID()
        {
            return EPermission.DeptProductSalesPlanAndBonusManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Edit;
        }
    }
}