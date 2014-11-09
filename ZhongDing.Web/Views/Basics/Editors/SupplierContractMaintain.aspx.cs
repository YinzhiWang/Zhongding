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
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Basics.Editors
{
    public partial class SupplierContractMaintain : BasePage
    {
        #region Fields

        /// <summary>
        /// 供应商ID
        /// </summary>
        /// <value>The supplier ID.</value>
        private int? SupplierID
        {
            get
            {
                string sSupplierID = Request.QueryString["SupplierID"];

                int iSupplierID;

                if (int.TryParse(sSupplierID, out iSupplierID))
                    return iSupplierID;
                else
                    return null;
            }
        }

        #endregion

        #region Members
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

        private ISupplierContractRepository _PageSupplierContractRepository;
        private ISupplierContractRepository PageSupplierContractRepository
        {
            get
            {
                if (_PageSupplierContractRepository == null)
                    _PageSupplierContractRepository = new SupplierContractRepository();

                return _PageSupplierContractRepository;
            }
        }

        private ISupplierContractFileRepository _PageSupplierContractFileRepository;
        private ISupplierContractFileRepository PageSupplierContractFileRepository
        {
            get
            {
                if (_PageSupplierContractFileRepository == null)
                    _PageSupplierContractFileRepository = new SupplierContractFileRepository();

                return _PageSupplierContractFileRepository;
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

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.SupplierID.HasValue
                || this.SupplierID <= 0)
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR);

                return;
            }

            //新增时隐藏其他sections
            if (!this.CurrentEntityID.HasValue
                || this.CurrentEntityID <= 0)
            {
                divContractFiles.Visible = false;
            }

            if (!IsPostBack)
            {
                hdnGridClientID.Value = base.GridClientID;
                hdnSupplierID.Value = this.SupplierID.ToString();

                BindProducts();

                LoadSupplierContract();
            }
        }

        private void LoadSupplierContract()
        {
            if (this.SupplierID.HasValue
                && this.SupplierID > 0)
            {
                var supplier = PageSupplierRepository.GetByID(this.SupplierID);

                if (supplier != null)
                {
                    lblSupplierName.Text = supplier.SupplierName;

                    if (CurrentEntityID.HasValue
                        && CurrentEntityID > 0)
                    {
                        var supplierContract = supplier.SupplierContract.Where(x => x.ID == CurrentEntityID).FirstOrDefault();

                        if (supplierContract != null)
                        {
                            hdnCurrentEntityID.Value = CurrentEntityID.ToString();

                            txtContractCode.Text = supplierContract.ContractCode;
                            rdpExpirationDate.SelectedDate = supplierContract.ExpirationDate;

                            if (supplierContract.ProductID.HasValue)
                                rcbxProduct.SelectedValue = supplierContract.ProductID.ToString();

                            BindProductSpecifications(supplierContract.ProductID);

                            if (supplierContract.ProductSpecificationID.HasValue)
                                rcbxProductSpecification.SelectedValue = supplierContract.ProductSpecificationID.ToString();

                            txtUnitPrice.Value = (double?)supplierContract.UnitPrice;

                            cbxIsNeedTaskAssignment.Checked = supplierContract.IsNeedTaskAssignment.HasValue
                                ? supplierContract.IsNeedTaskAssignment.Value : false;

                            #region 初始化配额

                            int currentYear = DateTime.Now.Year;

                            var taskAssignments = supplierContract.SupplierTaskAssignment.Where(x => x.IsDeleted == false && x.YearOfTask == currentYear).ToList();

                            var taskAssignmentMonth1 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.January);
                            if (taskAssignmentMonth1 != null)
                                txtMonthTask1.Value = (double?)taskAssignmentMonth1.Quantity;

                            var taskAssignmentMonth2 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.February);
                            if (taskAssignmentMonth2 != null)
                                txtMonthTask2.Value = (double?)taskAssignmentMonth2.Quantity;

                            var taskAssignmentMonth3 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.March);
                            if (taskAssignmentMonth3 != null)
                                txtMonthTask3.Value = (double?)taskAssignmentMonth3.Quantity;

                            var taskAssignmentMonth4 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.April);
                            if (taskAssignmentMonth4 != null)
                                txtMonthTask4.Value = (double?)taskAssignmentMonth4.Quantity;

                            var taskAssignmentMonth5 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.May);
                            if (taskAssignmentMonth5 != null)
                                txtMonthTask5.Value = (double?)taskAssignmentMonth5.Quantity;

                            var taskAssignmentMonth6 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.June);
                            if (taskAssignmentMonth6 != null)
                                txtMonthTask6.Value = (double?)taskAssignmentMonth6.Quantity;

                            var taskAssignmentMonth7 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.July);
                            if (taskAssignmentMonth7 != null)
                                txtMonthTask7.Value = (double?)taskAssignmentMonth7.Quantity;

                            var taskAssignmentMonth8 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.August);
                            if (taskAssignmentMonth8 != null)
                                txtMonthTask8.Value = (double?)taskAssignmentMonth8.Quantity;

                            var taskAssignmentMonth9 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.September);
                            if (taskAssignmentMonth9 != null)
                                txtMonthTask9.Value = (double?)taskAssignmentMonth9.Quantity;

                            var taskAssignmentMonth10 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.October);
                            if (taskAssignmentMonth10 != null)
                                txtMonthTask10.Value = (double?)taskAssignmentMonth10.Quantity;

                            var taskAssignmentMonth11 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.November);
                            if (taskAssignmentMonth11 != null)
                                txtMonthTask11.Value = (double?)taskAssignmentMonth11.Quantity;

                            var taskAssignmentMonth12 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.December);
                            if (taskAssignmentMonth12 != null)
                                txtMonthTask12.Value = (double?)taskAssignmentMonth12.Quantity;

                            #endregion

                        }
                        else
                            txtContractCode.Text = Utility.GenerateAutoSerialNo(PageSupplierContractRepository.GetMaxEntityID(),
                            GlobalConst.EntityAutoSerialNo.SerialNoPrefix.SUPPLIER_CONTRACT);
                    }
                    else
                        txtContractCode.Text = Utility.GenerateAutoSerialNo(PageSupplierContractRepository.GetMaxEntityID(),
                            GlobalConst.EntityAutoSerialNo.SerialNoPrefix.SUPPLIER_CONTRACT);
                }
            }
        }

        private void BindProducts()
        {
            var products = PageProductRepository.GetDropdownItems();
            rcbxProduct.DataSource = products;
            rcbxProduct.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxProduct.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxProduct.DataBind();
        }

        private void BindProductSpecifications(int? productID)
        {
            UISearchDropdownItem uiSearchObj = new UISearchDropdownItem()
            {
                ParentItemValue = productID.HasValue ? productID : GlobalConst.INVALID_INT
            };

            var productSpecifications = PageProductSpecificationRepository.GetDropdownItems(uiSearchObj);
            rcbxProductSpecification.DataSource = productSpecifications;
            rcbxProductSpecification.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxProductSpecification.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxProductSpecification.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            Supplier supplier = null;

            if (this.SupplierID.HasValue && this.SupplierID > 0)
            {
                supplier = PageSupplierRepository.GetByID(this.SupplierID);

                if (supplier != null)
                {
                    SupplierContract supplierContract = null;

                    if (base.CurrentEntityID.HasValue
                        && base.CurrentEntityID > 0)
                        supplierContract = supplier.SupplierContract.Where(x => x.ID == base.CurrentEntityID).FirstOrDefault();

                    if (supplierContract == null)
                    {
                        supplierContract = new SupplierContract();

                        supplier.SupplierContract.Add(supplierContract);
                    }

                    supplierContract.ContractCode = txtContractCode.Text;
                    supplierContract.ExpirationDate = rdpExpirationDate.SelectedDate;

                    if (!string.IsNullOrEmpty(rcbxProduct.SelectedValue))
                        supplierContract.ProductID = Convert.ToInt32(rcbxProduct.SelectedValue);

                    if (!string.IsNullOrEmpty(rcbxProductSpecification.SelectedValue))
                        supplierContract.ProductSpecificationID = Convert.ToInt32(rcbxProductSpecification.SelectedValue);

                    supplierContract.UnitPrice = (decimal?)txtUnitPrice.Value;

                    supplierContract.IsNeedTaskAssignment = cbxIsNeedTaskAssignment.Checked;

                    #region 配额


                    if (cbxIsNeedTaskAssignment.Checked)
                    {
                        int currentYear = DateTime.Now.Year;

                        //删除其他年份的配额
                        foreach (var taskAssignment in supplierContract.SupplierTaskAssignment
                            .Where(x => x.IsDeleted == false && x.YearOfTask != currentYear))
                        {
                            taskAssignment.IsDeleted = true;
                        }

                        var taskAssignments = supplierContract.SupplierTaskAssignment
                            .Where(x => x.IsDeleted == false && x.YearOfTask == currentYear).ToList();

                        #region 一月配额

                        var taskAssignmentMonth1 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.January);

                        if (taskAssignmentMonth1 == null)
                        {
                            taskAssignmentMonth1 = new SupplierTaskAssignment()
                            {
                                SupplierID = this.SupplierID,
                                YearOfTask = currentYear,
                                MonthOfTask = (int)EMonthOfYear.January
                            };

                            supplierContract.SupplierTaskAssignment.Add(taskAssignmentMonth1);
                        }

                        taskAssignmentMonth1.Quantity = (int?)txtMonthTask1.Value;

                        #endregion

                        #region 二月配额

                        var taskAssignmentMonth2 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.February);

                        if (taskAssignmentMonth2 == null)
                        {
                            taskAssignmentMonth2 = new SupplierTaskAssignment()
                            {
                                SupplierID = this.SupplierID,
                                YearOfTask = currentYear,
                                MonthOfTask = (int)EMonthOfYear.February
                            };

                            supplierContract.SupplierTaskAssignment.Add(taskAssignmentMonth2);
                        }

                        taskAssignmentMonth2.Quantity = (int?)txtMonthTask2.Value;

                        #endregion

                        #region 三月配额

                        var taskAssignmentMonth3 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.March);

                        if (taskAssignmentMonth3 == null)
                        {
                            taskAssignmentMonth3 = new SupplierTaskAssignment()
                            {
                                SupplierID = this.SupplierID,
                                YearOfTask = currentYear,
                                MonthOfTask = (int)EMonthOfYear.March
                            };

                            supplierContract.SupplierTaskAssignment.Add(taskAssignmentMonth3);
                        }

                        taskAssignmentMonth3.Quantity = (int?)txtMonthTask3.Value;

                        #endregion

                        #region 四月配额

                        var taskAssignmentMonth4 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.April);

                        if (taskAssignmentMonth4 == null)
                        {
                            taskAssignmentMonth4 = new SupplierTaskAssignment()
                            {
                                SupplierID = this.SupplierID,
                                YearOfTask = currentYear,
                                MonthOfTask = (int)EMonthOfYear.April
                            };

                            supplierContract.SupplierTaskAssignment.Add(taskAssignmentMonth4);
                        }

                        taskAssignmentMonth4.Quantity = (int?)txtMonthTask4.Value;

                        #endregion

                        #region 五月配额

                        var taskAssignmentMonth5 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.May);

                        if (taskAssignmentMonth5 == null)
                        {
                            taskAssignmentMonth5 = new SupplierTaskAssignment()
                            {
                                SupplierID = this.SupplierID,
                                YearOfTask = currentYear,
                                MonthOfTask = (int)EMonthOfYear.May
                            };

                            supplierContract.SupplierTaskAssignment.Add(taskAssignmentMonth5);
                        }

                        taskAssignmentMonth5.Quantity = (int?)txtMonthTask5.Value;

                        #endregion

                        #region 六月配额

                        var taskAssignmentMonth6 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.June);

                        if (taskAssignmentMonth6 == null)
                        {
                            taskAssignmentMonth6 = new SupplierTaskAssignment()
                            {
                                SupplierID = this.SupplierID,
                                YearOfTask = currentYear,
                                MonthOfTask = (int)EMonthOfYear.June
                            };

                            supplierContract.SupplierTaskAssignment.Add(taskAssignmentMonth6);
                        }

                        taskAssignmentMonth6.Quantity = (int?)txtMonthTask6.Value;

                        #endregion

                        #region 七月配额

                        var taskAssignmentMonth7 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.July);

                        if (taskAssignmentMonth7 == null)
                        {
                            taskAssignmentMonth7 = new SupplierTaskAssignment()
                            {
                                SupplierID = this.SupplierID,
                                YearOfTask = currentYear,
                                MonthOfTask = (int)EMonthOfYear.June
                            };

                            supplierContract.SupplierTaskAssignment.Add(taskAssignmentMonth7);
                        }

                        taskAssignmentMonth7.Quantity = (int?)txtMonthTask7.Value;

                        #endregion

                        #region 八月配额

                        var taskAssignmentMonth8 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.August);

                        if (taskAssignmentMonth8 == null)
                        {
                            taskAssignmentMonth8 = new SupplierTaskAssignment()
                            {
                                SupplierID = this.SupplierID,
                                YearOfTask = currentYear,
                                MonthOfTask = (int)EMonthOfYear.August
                            };

                            supplierContract.SupplierTaskAssignment.Add(taskAssignmentMonth8);
                        }

                        taskAssignmentMonth8.Quantity = (int?)txtMonthTask8.Value;

                        #endregion

                        #region 九月配额

                        var taskAssignmentMonth9 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.September);

                        if (taskAssignmentMonth9 == null)
                        {
                            taskAssignmentMonth9 = new SupplierTaskAssignment()
                            {
                                SupplierID = this.SupplierID,
                                YearOfTask = currentYear,
                                MonthOfTask = (int)EMonthOfYear.September
                            };

                            supplierContract.SupplierTaskAssignment.Add(taskAssignmentMonth9);
                        }

                        taskAssignmentMonth9.Quantity = (int?)txtMonthTask9.Value;

                        #endregion

                        #region 十月配额

                        var taskAssignmentMonth10 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.October);

                        if (taskAssignmentMonth10 == null)
                        {
                            taskAssignmentMonth10 = new SupplierTaskAssignment()
                            {
                                SupplierID = this.SupplierID,
                                YearOfTask = currentYear,
                                MonthOfTask = (int)EMonthOfYear.October
                            };

                            supplierContract.SupplierTaskAssignment.Add(taskAssignmentMonth10);
                        }

                        taskAssignmentMonth10.Quantity = (int?)txtMonthTask10.Value;

                        #endregion

                        #region 十一月配额

                        var taskAssignmentMonth11 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.November);

                        if (taskAssignmentMonth11 == null)
                        {
                            taskAssignmentMonth11 = new SupplierTaskAssignment()
                            {
                                SupplierID = this.SupplierID,
                                YearOfTask = currentYear,
                                MonthOfTask = (int)EMonthOfYear.November
                            };

                            supplierContract.SupplierTaskAssignment.Add(taskAssignmentMonth11);
                        }

                        taskAssignmentMonth11.Quantity = (int?)txtMonthTask11.Value;

                        #endregion

                        #region 十二月配额

                        var taskAssignmentMonth12 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.December);

                        if (taskAssignmentMonth12 == null)
                        {
                            taskAssignmentMonth12 = new SupplierTaskAssignment()
                            {
                                SupplierID = this.SupplierID,
                                YearOfTask = currentYear,
                                MonthOfTask = (int)EMonthOfYear.December
                            };

                            supplierContract.SupplierTaskAssignment.Add(taskAssignmentMonth12);
                        }

                        taskAssignmentMonth12.Quantity = (int?)txtMonthTask12.Value;

                        #endregion

                    }

                    #endregion

                    PageSupplierRepository.Save();

                    hdnCurrentEntityID.Value = supplierContract.ID.ToString();

                    if (this.CurrentEntityID.HasValue
                    && this.CurrentEntityID > 0)
                    {
                        this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                        this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_CLOSE_WIN);
                    }
                    else
                    {
                        this.Master.BaseNotification.OnClientHidden = "refreshMaintenancePage";
                        this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REFRESH);
                    }

                }
            }

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

        protected void rgContractFiles_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchSupplierContractFile()
            {
                ContractID = this.CurrentEntityID.Value
            };

            int totalRecords;

            var uiContractFiles = PageSupplierContractFileRepository.GetUIList(uiSearchObj, rgContractFiles.CurrentPageIndex, rgContractFiles.PageSize, out totalRecords);

            rgContractFiles.DataSource = uiContractFiles;
            rgContractFiles.VirtualItemCount = totalRecords;
        }

        protected void rgContractFiles_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                PageSupplierContractFileRepository.DeleteByID(id);
                PageSupplierContractFileRepository.Save();
            }
            rgContractFiles.Rebind();
        }

    }
}