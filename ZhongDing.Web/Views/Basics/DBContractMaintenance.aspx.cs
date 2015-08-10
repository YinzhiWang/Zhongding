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
using ZhongDing.Common.Extension;

namespace ZhongDing.Web.Views.Basics
{
    public partial class DBContractMaintenance : BasePage
    {
        #region Members

        private IDBContractRepository _PageDBContractRepository;
        private IDBContractRepository PageDBContractRepository
        {
            get
            {
                if (_PageDBContractRepository == null)
                {
                    _PageDBContractRepository = new DBContractRepository();
                }

                return _PageDBContractRepository;
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

        private IHospitalCodeRepository _PageHospitalCodeRepository;
        private IHospitalCodeRepository PageHospitalCodeRepository
        {
            get
            {
                if (_PageHospitalCodeRepository == null)
                    _PageHospitalCodeRepository = new HospitalCodeRepository();

                return _PageHospitalCodeRepository;
            }
        }

        private IDBContractHospitalRepository _PageDBContractHospitalRepository;
        private IDBContractHospitalRepository PageDBContractHospitalRepository
        {
            get
            {
                if (_PageDBContractHospitalRepository == null)
                    _PageDBContractHospitalRepository = new DBContractHospitalRepository();

                return _PageDBContractHospitalRepository;
            }
        }

        private DBContract _CurrentEntity;
        private DBContract CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageDBContractRepository.GetByID(this.CurrentEntityID);

                return _CurrentEntity;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.DBContractManage;

            if (!IsPostBack)
            {

                BindClientUsers();

                BindDepartments();

                BindProducts();

                BindHospitalTypes();

                LoadCurrentEntity();
                base.PermissionOptionCheckButtonDelete(btnDelete);
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

        private void BindDepartments()
        {
            var departments = PageDepartmentRepository.GetDropdownItems();

            rcbxDepartment.DataSource = departments;
            rcbxDepartment.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxDepartment.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxDepartment.DataBind();

            rcbxDepartment.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindInChargeUsers(int departmentID)
        {
            if (departmentID > 0)
            {
                ddlInChargeUser.Items.Clear();

                var inChargeUsers = PageUsersRepository.GetDropdownItems(new UISearchDropdownItem()
                {
                    Extension = new UISearchExtension()
                    {
                        DepartmentID = departmentID
                    }
                });

                ddlInChargeUser.DataSource = inChargeUsers;
                ddlInChargeUser.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                ddlInChargeUser.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                ddlInChargeUser.DataBind();
            }
        }

        private void BindProducts()
        {
            var products = PageProductRepository.GetDropdownItems();

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

        private void BindHospitalTypes()
        {
            ddlHospitalType.Items.Add(new DropDownListItem(GlobalConst.HospitalTypes.BASE_MEDICINE,
                ((int)EHospitalType.BaseMedicine).ToString()));
            ddlHospitalType.Items.Add(new DropDownListItem(GlobalConst.HospitalTypes.BUSINESS_MEDICINE,
                ((int)EHospitalType.BusinessMedicine).ToString()));

            ddlHospitalType.DataBind();
        }

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                var currentEntity = PageDBContractRepository.GetByID(this.CurrentEntityID);

                if (currentEntity != null)
                {
                    hdnCurrentEntityID.Value = currentEntity.ID.ToString();

                    txtContractCode.Text = currentEntity.ContractCode;

                    if (currentEntity.ClientUserID.HasValue && currentEntity.ClientUserID > 0)
                        rcbxClientUser.SelectedValue = currentEntity.ClientUserID.ToString();

                    cbxIsTempContract.Checked = currentEntity.IsTempContract.HasValue ? currentEntity.IsTempContract.Value : false;

                    if (currentEntity.DepartmentID.HasValue && currentEntity.DepartmentID > 0)
                    {
                        rcbxDepartment.SelectedValue = currentEntity.DepartmentID.ToString();

                        BindInChargeUsers(currentEntity.DepartmentID.Value);

                        if (currentEntity.InChargeUserID.HasValue && currentEntity.InChargeUserID > 0)
                            ddlInChargeUser.SelectedValue = currentEntity.InChargeUserID.ToString();
                    }

                    if (currentEntity.ProductID.HasValue && currentEntity.ProductID > 0)
                    {
                        rcbxProduct.SelectedValue = currentEntity.ProductID.ToString();

                        BindProductSpecifications(currentEntity.ProductID.Value);

                        if (currentEntity.ProductSpecificationID.HasValue && currentEntity.ProductSpecificationID > 0)
                            ddlProductSpecification.SelectedValue = currentEntity.ProductSpecificationID.ToString();
                    }

                    txtPromotionExpense.DbValue = currentEntity.PromotionExpense;
                    rdpContractExpDate.SelectedDate = currentEntity.ContractExpDate;

                    if (currentEntity.IsNew.HasValue)
                    {
                        if (currentEntity.IsNew.Value)
                            radioIsNew.Checked = true;
                        else
                            radioIsNotNew.Checked = true;
                    }

                    if (currentEntity.HospitalTypeID.HasValue && currentEntity.HospitalTypeID > 0)
                        ddlHospitalType.SelectedValue = currentEntity.HospitalTypeID.ToString();

                    txtComment.Text = currentEntity.Comment;

                    #region 初始化任务量配置

                    int currentYear = DateTime.Now.Year;

                    var taskAssignments = currentEntity.DBContractTaskAssignment.Where(x => x.IsDeleted == false).ToList();

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
                {
                    btnDelete.Visible = false;
                    divOtherSections.Visible = false;
                    txtContractCode.Text = Utility.GenerateAutoSerialNo(PageDBContractRepository.GetMaxEntityID(),
                    GlobalConst.EntityAutoSerialNo.SerialNoPrefix.DB_CONTRACT);
                }
            }
            else
            {
                btnDelete.Visible = false;
                divOtherSections.Visible = false;
                txtContractCode.Text = Utility.GenerateAutoSerialNo(PageDBContractRepository.GetMaxEntityID(),
                    GlobalConst.EntityAutoSerialNo.SerialNoPrefix.DB_CONTRACT);
            }
        }

        #endregion

        protected void cvClientUser_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!string.IsNullOrEmpty(rcbxClientUser.Text))
            {
                UISearchDropdownItem uiSearchObj = new UISearchDropdownItem()
                {
                    ItemText = rcbxClientUser.Text.Trim()
                };

                var clientUsers = PageClientUserRepository.GetDropdownItems(uiSearchObj);

                if (clientUsers.Count <= 0)
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

        protected void rcbxDepartment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value))
            {
                int departmentID;

                if (int.TryParse(e.Value, out departmentID))
                    BindInChargeUsers(departmentID);
            }
            else
                ddlInChargeUser.Items.Clear();
        }

        protected void rcbxProduct_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value))
            {
                int productID;

                if (int.TryParse(e.Value, out productID))
                    BindProductSpecifications(productID);
            }
            else
                ddlProductSpecification.Items.Clear();
        }

        protected void rgHospitals_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchHospital
            {
                DBContractID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            int totalRecords;

            var dBContractHospitals = PageDBContractHospitalRepository.GetUIList(uiSearchObj, rgHospitals.CurrentPageIndex, rgHospitals.PageSize, out totalRecords);

            rgHospitals.DataSource = dBContractHospitals;
            rgHospitals.VirtualItemCount = totalRecords;
        }

        protected void rgHospitals_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.EditItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;

                int id = 0;
                if (e.Item.ItemIndex >= 0)
                {
                    string sid = gridDataItem.GetDataKeyValue("ID").ToString();
                    int.TryParse(sid, out id);
                }

                var rcbxHospital = (RadComboBox)e.Item.FindControl("rcbxHospital");

                if (rcbxHospital != null)
                {
                    var uiSearchObj = new UISearchDropdownItem();

                    if (this.CurrentEntity != null
                        && this.CurrentEntity.DBContractHospital.Count > 0)
                    {
                        uiSearchObj.ExcludeItemValues = this.CurrentEntity
                            .DBContractHospital.Where(x => x.IsDeleted == false && x.ID != id)
                            .Select(x => x.HospitalCodeID).ToList();
                    }

                    var hospitals = PageHospitalCodeRepository.GetDropdownItems(uiSearchObj);
                    rcbxHospital.DataSource = hospitals;
                    rcbxHospital.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                    rcbxHospital.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                    rcbxHospital.DataBind();
                    rcbxHospital.Items.Insert(0, new RadComboBoxItem("", ""));
                    if (e.Item.ItemIndex >= 0)
                    {
                        var uiEntity = (UIDBContractHospital)gridDataItem.DataItem;
                        rcbxHospital.SelectedValue = uiEntity.HospitalCodeID.ToString();
                    }
                }
                if (e.Item.ItemIndex == -1)
                    hdnCurrentEditHospitalID.Value = "-1";
            }
        }

        protected void rgHospitals_EditCommand(object sender, GridCommandEventArgs e)
        {
            var editableItem = ((GridEditableItem)e.Item);

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                hdnCurrentEditID.Value = id.ToString();
            }
        }

        protected void rgHospitals_InsertCommand(object sender, GridCommandEventArgs e)
        {
            if (this.CurrentEntity != null)
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;

                    DBContractHospital dBContractHospital = new DBContractHospital();

                    var rcbxHospital = dataItem.FindControl("rcbxHospital") as RadComboBox;

                    if (!string.IsNullOrEmpty(rcbxHospital.SelectedValue))
                    {
                        int hospitalID;
                        if (int.TryParse(rcbxHospital.SelectedValue, out hospitalID))
                            dBContractHospital.HospitalCodeID = hospitalID;
                    }
                    //else
                    //{
                    //    HospitalCode hospitalCode = new HospitalCode { HospitalName = hdnCurrentEditHospitalName.Value.Trim() };
                    //    dBContractHospital.HospitalCode = hospitalCode;
                    //}

                    this.CurrentEntity.DBContractHospital.Add(dBContractHospital);

                    PageDBContractRepository.Save();
                }

                rgHospitals.Rebind();
            }
        }

        protected void rgHospitals_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            var editableItem = ((GridEditableItem)e.Item);
            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;

                    var dBContractHospital = PageDBContractHospitalRepository.GetByID(id);

                    var rcbxHospital = dataItem.FindControl("rcbxHospital") as RadComboBox;

                    if (!string.IsNullOrEmpty(rcbxHospital.SelectedValue))
                    {
                        int hospitalID;
                        if (int.TryParse(rcbxHospital.SelectedValue, out hospitalID))
                            dBContractHospital.HospitalCodeID = hospitalID;
                    }
                    //else
                    //{
                    //    Hospital hospital = new Hospital { HospitalName = hdnCurrentEditHospitalName.Value.Trim() };
                    //    dBContractHospital.Hospital = hospital;
                    //}

                    PageDBContractHospitalRepository.Save();
                }
            }
        }

        protected void rgHospitals_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            var editableItem = ((GridEditableItem)e.Item);
            var id = editableItem.GetDataKeyValue("ID").ToIntOrNull();
            if (id.BiggerThanZero())
            {
                PageDBContractHospitalRepository.DeleteByID(id);
                PageDBContractHospitalRepository.Save();
                rgHospitals.Rebind();
            }
        }

        protected void cvHospitalName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            var cvHospitalName = (CustomValidator)source;

            if (this.CurrentEntity != null)
            {
                if (hdnCurrentEditHospitalID.Value != GlobalConst.INVALID_INT.ToString())
                {
                    int curEditHospitalID;
                    if (int.TryParse(hdnCurrentEditHospitalID.Value, out curEditHospitalID))
                    {
                        if (PageDBContractHospitalRepository.GetList(x => x.DBContract != null
                            && x.DBContract.ProductID == CurrentEntity.ProductID
                            && x.DBContract.ProductSpecificationID == CurrentEntity.ProductSpecificationID
                            && x.DBContract.DBContractHospital.Any(y => y.HospitalCodeID == curEditHospitalID)).Count() > 0)
                        {
                            if (cvHospitalName != null)
                                cvHospitalName.ErrorMessage = "同一种货品和规格已关联过该医院，请重新选择";
                            args.IsValid = false;
                        }
                    }
                }
                else
                {
                    //if (!string.IsNullOrEmpty(hdnCurrentEditHospitalName.Value))
                    //{
                    //    if (PageHospitalRepository.GetList(x => x.HospitalName.ToLower()
                    //        .Equals(args.Value.Trim().ToLower())).Count() > 0)
                    //    {
                    //        if (cvHospitalName != null)
                    //            cvHospitalName.ErrorMessage = "医院名称已存在，请重新选择或输入";
                    //        args.IsValid = false;
                    //    }
                    //}
                    if (!hdnCurrentEditHospitalID.Value.ToIntOrNull().BiggerThanZero())
                    {
                        if (cvHospitalName != null)
                            cvHospitalName.ErrorMessage = "医院为必填，请选择";
                        args.IsValid = false;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            DBContract currentEntity = null;

            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                currentEntity = PageDBContractRepository.GetByID(this.CurrentEntityID);

            if (currentEntity == null)
            {
                currentEntity = new DBContract();
                PageDBContractRepository.Add(currentEntity);
            }

            currentEntity.ContractCode = txtContractCode.Text.Trim();
            currentEntity.ClientUserID = Convert.ToInt32(rcbxClientUser.SelectedValue);
            currentEntity.IsTempContract = cbxIsTempContract.Checked;
            currentEntity.DepartmentID = Convert.ToInt32(rcbxDepartment.SelectedValue);
            currentEntity.InChargeUserID = Convert.ToInt32(ddlInChargeUser.SelectedValue);
            currentEntity.ProductID = Convert.ToInt32(rcbxProduct.SelectedValue);
            currentEntity.ProductSpecificationID = Convert.ToInt32(ddlProductSpecification.SelectedValue);
            currentEntity.PromotionExpense = (decimal?)txtPromotionExpense.Value;
            currentEntity.ContractExpDate = rdpContractExpDate.SelectedDate;

            if (radioIsNew.Checked)
                currentEntity.IsNew = true;
            else if (radioIsNotNew.Checked)
                currentEntity.IsNew = false;

            currentEntity.HospitalTypeID = Convert.ToInt32(ddlHospitalType.SelectedValue);
            currentEntity.Comment = txtComment.Text.Trim();

            #region 任务量配置


            var taskAssignments = currentEntity.DBContractTaskAssignment
                .Where(x => x.IsDeleted == false).ToList();

            #region 一月配置

            var taskAssignmentMonth1 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.January);

            if (taskAssignmentMonth1 == null)
            {
                taskAssignmentMonth1 = new DBContractTaskAssignment()
                {
                    MonthOfTask = (int)EMonthOfYear.January
                };

                currentEntity.DBContractTaskAssignment.Add(taskAssignmentMonth1);
            }

            taskAssignmentMonth1.Quantity = (int?)txtMonthTask1.Value;

            #endregion

            #region 二月配置

            var taskAssignmentMonth2 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.February);

            if (taskAssignmentMonth2 == null)
            {
                taskAssignmentMonth2 = new DBContractTaskAssignment()
                {
                    MonthOfTask = (int)EMonthOfYear.February
                };

                currentEntity.DBContractTaskAssignment.Add(taskAssignmentMonth2);
            }

            taskAssignmentMonth2.Quantity = (int?)txtMonthTask2.Value;

            #endregion

            #region 三月配置

            var taskAssignmentMonth3 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.March);

            if (taskAssignmentMonth3 == null)
            {
                taskAssignmentMonth3 = new DBContractTaskAssignment()
                {
                    MonthOfTask = (int)EMonthOfYear.March
                };

                currentEntity.DBContractTaskAssignment.Add(taskAssignmentMonth3);
            }

            taskAssignmentMonth3.Quantity = (int?)txtMonthTask3.Value;

            #endregion

            #region 四月配置

            var taskAssignmentMonth4 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.April);

            if (taskAssignmentMonth4 == null)
            {
                taskAssignmentMonth4 = new DBContractTaskAssignment()
                {
                    MonthOfTask = (int)EMonthOfYear.April
                };

                currentEntity.DBContractTaskAssignment.Add(taskAssignmentMonth4);
            }

            taskAssignmentMonth4.Quantity = (int?)txtMonthTask4.Value;

            #endregion

            #region 五月配置

            var taskAssignmentMonth5 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.May);

            if (taskAssignmentMonth5 == null)
            {
                taskAssignmentMonth5 = new DBContractTaskAssignment()
                {
                    MonthOfTask = (int)EMonthOfYear.May
                };

                currentEntity.DBContractTaskAssignment.Add(taskAssignmentMonth5);
            }

            taskAssignmentMonth5.Quantity = (int?)txtMonthTask5.Value;

            #endregion

            #region 六月配置

            var taskAssignmentMonth6 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.June);

            if (taskAssignmentMonth6 == null)
            {
                taskAssignmentMonth6 = new DBContractTaskAssignment()
                {
                    MonthOfTask = (int)EMonthOfYear.June
                };

                currentEntity.DBContractTaskAssignment.Add(taskAssignmentMonth6);
            }

            taskAssignmentMonth6.Quantity = (int?)txtMonthTask6.Value;

            #endregion

            #region 七月配置

            var taskAssignmentMonth7 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.July);

            if (taskAssignmentMonth7 == null)
            {
                taskAssignmentMonth7 = new DBContractTaskAssignment()
                {
                    MonthOfTask = (int)EMonthOfYear.July
                };

                currentEntity.DBContractTaskAssignment.Add(taskAssignmentMonth7);
            }

            taskAssignmentMonth7.Quantity = (int?)txtMonthTask7.Value;

            #endregion

            #region 八月配置

            var taskAssignmentMonth8 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.August);

            if (taskAssignmentMonth8 == null)
            {
                taskAssignmentMonth8 = new DBContractTaskAssignment()
                {
                    MonthOfTask = (int)EMonthOfYear.August
                };

                currentEntity.DBContractTaskAssignment.Add(taskAssignmentMonth8);
            }

            taskAssignmentMonth8.Quantity = (int?)txtMonthTask8.Value;

            #endregion

            #region 九月配置

            var taskAssignmentMonth9 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.September);

            if (taskAssignmentMonth9 == null)
            {
                taskAssignmentMonth9 = new DBContractTaskAssignment()
                {
                    MonthOfTask = (int)EMonthOfYear.September
                };

                currentEntity.DBContractTaskAssignment.Add(taskAssignmentMonth9);
            }

            taskAssignmentMonth9.Quantity = (int?)txtMonthTask9.Value;

            #endregion

            #region 十月配置

            var taskAssignmentMonth10 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.October);

            if (taskAssignmentMonth10 == null)
            {
                taskAssignmentMonth10 = new DBContractTaskAssignment()
                {
                    MonthOfTask = (int)EMonthOfYear.October
                };

                currentEntity.DBContractTaskAssignment.Add(taskAssignmentMonth10);
            }

            taskAssignmentMonth10.Quantity = (int?)txtMonthTask10.Value;

            #endregion

            #region 十一月配置

            var taskAssignmentMonth11 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.November);

            if (taskAssignmentMonth11 == null)
            {
                taskAssignmentMonth11 = new DBContractTaskAssignment()
                {
                    MonthOfTask = (int)EMonthOfYear.November
                };

                currentEntity.DBContractTaskAssignment.Add(taskAssignmentMonth11);
            }

            taskAssignmentMonth11.Quantity = (int?)txtMonthTask11.Value;

            #endregion

            #region 十二月配置

            var taskAssignmentMonth12 = taskAssignments.FirstOrDefault(x => x.MonthOfTask == (int)EMonthOfYear.December);

            if (taskAssignmentMonth12 == null)
            {
                taskAssignmentMonth12 = new DBContractTaskAssignment()
                {
                    MonthOfTask = (int)EMonthOfYear.December
                };

                currentEntity.DBContractTaskAssignment.Add(taskAssignmentMonth12);
            }

            taskAssignmentMonth12.Quantity = (int?)txtMonthTask12.Value;

            #endregion


            #endregion

            PageDBContractRepository.Save();

            hdnCurrentEntityID.Value = currentEntity.ID.ToString();

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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IDBContractRepository dbContractRepository = new DBContractRepository();
                    IDBContractTaskAssignmentRepository dBContractTaskAssignmentRepository = new DBContractTaskAssignmentRepository();
                    IDBContractHospitalRepository dBContractHospitalRepository = new DBContractHospitalRepository();

                    dbContractRepository.SetDbModel(db);
                    dBContractTaskAssignmentRepository.SetDbModel(db);
                    dBContractHospitalRepository.SetDbModel(db);

                    var currentEntity = dbContractRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        foreach (var taskAssignment in currentEntity.DBContractTaskAssignment)
                        {
                            dBContractTaskAssignmentRepository.Delete(taskAssignment);
                        }

                        foreach (var dBContractHospital in currentEntity.DBContractHospital)
                        {
                            dBContractHospitalRepository.Delete(dBContractHospital);
                        }

                        dbContractRepository.Delete(currentEntity);

                        unitOfWork.SaveChanges();
                    }
                }

                this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_DELETED_REDIRECT);
            }
        }

        protected override EPermission PagePermissionID()
        {
            return EPermission.DBContractManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Edit;
        }
    }
}