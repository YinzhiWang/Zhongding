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

namespace ZhongDing.Web.Views.HRM
{
    public partial class DepartmentMaintenance : BasePage
    {
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

        private IDeptDistrictRepository _PageDeptDistrictRepository;
        private IDeptDistrictRepository PageDeptDistrictRepository
        {
            get
            {
                if (_PageDeptDistrictRepository == null)
                    _PageDeptDistrictRepository = new DeptDistrictRepository();

                return _PageDeptDistrictRepository;
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

        private IDeptMarketProductRepository _PageDeptMarketProductRepository;
        private IDeptMarketProductRepository PageDeptMarketProductRepository
        {
            get
            {
                if (_PageDeptMarketProductRepository == null)
                    _PageDeptMarketProductRepository = new DeptMarketProductRepository();

                return _PageDeptMarketProductRepository;
            }
        }

        private IDeptProductEvaluationRepository _PageDeptProductEvaluationRepository;
        private IDeptProductEvaluationRepository PageDeptProductEvaluationRepository
        {
            get
            {
                if (_PageDeptProductEvaluationRepository == null)
                    _PageDeptProductEvaluationRepository = new DeptProductEvaluationRepository();

                return _PageDeptProductEvaluationRepository;
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
            this.Master.MenuItemID = (int)EMenuItem.DepartmentManage;
            this.Master.IsNeedAutoHideLoading = true;

            if (!IsPostBack)
            {
                BindDepartmentTypes();

                BindDepartmentUsers();

                LoadCurrentEntity();
            }
        }

        #region Private Methods

        private void BindDepartmentTypes()
        {
            ddlDepartmentType.Items.Add(new DropDownListItem(GlobalConst.DepartmentTypes.BASE_MEDICINE,
                ((int)EDepartmentType.BaseMedicine).ToString()));
            ddlDepartmentType.Items.Add(new DropDownListItem(GlobalConst.DepartmentTypes.BUSINESS_MEDICINE,
                ((int)EDepartmentType.BusinessMedicine).ToString()));

            ddlDepartmentType.DataBind();
        }

        private void BindDepartmentUsers()
        {
            var deptUsers = PageUsersRepository.GetDropdownItems(new UISearchDropdownItem()
            {
                Extension = new UISearchExtension()
                {
                    DepartmentID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
                }
            });

            rcbxDirectorUser.DataSource = deptUsers;
            rcbxDirectorUser.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxDirectorUser.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxDirectorUser.DataBind();

            rcbxDirectorUser.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindDeptDistricts()
        {
            if (!string.IsNullOrEmpty(ddlDepartmentType.SelectedValue))
            {
                int departmentTypeID;

                if (int.TryParse(ddlDepartmentType.SelectedValue, out departmentTypeID))
                {
                    if (departmentTypeID == (int)EDepartmentType.BaseMedicine)
                    {
                        divDeptDistrict.Visible = true;

                        var deptDistricts = PageDeptDistrictRepository.GetDropdownItems(new UISearchDropdownItem()
                        {
                            Extension = new UISearchExtension { DepartmentTypeID = departmentTypeID }
                        });

                        ddlDeptDistrict.DataSource = deptDistricts;
                        ddlDeptDistrict.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                        ddlDeptDistrict.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                        ddlDeptDistrict.DataBind();

                    }
                    else
                        divDeptDistrict.Visible = false;
                }
            }
        }

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                var currentEntity = PageDepartmentRepository.GetByID(this.CurrentEntityID);

                if (currentEntity != null)
                {
                    hdnCurrentEntityID.Value = currentEntity.ID.ToString();

                    txtDepartmentName.Text = currentEntity.DepartmentName;

                    ddlDepartmentType.SelectedValue = currentEntity.DepartmentTypeID.ToString();

                    if (currentEntity.DirectorUserID.HasValue)
                        rcbxDirectorUser.SelectedValue = currentEntity.DirectorUserID.ToString();

                    BindDeptDistricts();

                    if (currentEntity.DeptDistrictID.HasValue)
                        ddlDeptDistrict.SelectedValue = currentEntity.DeptDistrictID.ToString();
                }
                else
                {
                    btnDelete.Visible = false;
                    divOtherSections.Visible = false;
                }
            }
            else
            {
                btnDelete.Visible = false;
                divOtherSections.Visible = false;
            }
        }

        /// <summary>
        /// 更新货品销售计划
        /// </summary>
        private void UpdateDeptProductRecord()
        {
            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
            {
                Department curDepartment = PageDepartmentRepository.GetByID(this.CurrentEntityID);

                if (curDepartment != null)
                {
                    var deptMarketProducts = PageDeptMarketProductRepository
                        .GetList(x => x.DeptMarketDivision.Users.DepartmentID == curDepartment.ID
                            && (x.Q1Task.HasValue || x.Q2Task.HasValue || x.Q3Task.HasValue || x.Q4Task.HasValue));

                    var deptMarketProductTasks = from p in deptMarketProducts
                                                 group p by p.ProductID into gp
                                                 select new
                                                 {
                                                     ProductID = gp.Key,
                                                     SubtotalTaks = gp.Sum(x => (x.Q1Task ?? 0)
                                                         + (x.Q2Task ?? 0)
                                                         + (x.Q3Task ?? 0)
                                                         + (x.Q4Task ?? 0))
                                                 };

                    foreach (var productTask in deptMarketProductTasks.Where(x => x.SubtotalTaks > 0))
                    {
                        var curDeptProductRecord = curDepartment.DepartmentProductRecord
                            .FirstOrDefault(x => x.ProductID == productTask.ProductID && x.Year == DateTime.Now.Year);

                        if (curDeptProductRecord == null)
                        {
                            curDeptProductRecord = new DepartmentProductRecord()
                            {
                                ProductID = productTask.ProductID,
                                Year = DateTime.Now.Year
                            };

                            curDepartment.DepartmentProductRecord.Add(curDeptProductRecord);
                        }

                        curDeptProductRecord.Task = productTask.SubtotalTaks;
                    }

                    foreach (var productRecord in curDepartment.DepartmentProductRecord)
                    {
                        if (!deptMarketProductTasks.Any(x => x.ProductID == productRecord.ProductID))
                            productRecord.Task = null;
                    }

                    PageDepartmentRepository.Save();
                }
            }
        }

        #endregion

        protected void cvDepartmentName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (PageDepartmentRepository.GetList(x => x.IsDeleted == false && x.ID != this.CurrentEntityID
                && x.DepartmentName.ToLower() == txtDepartmentName.Text.Trim().ToLower()).Count() > 0)
            {
                args.IsValid = false;
            }
        }

        protected void ddlDepartmentType_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            //重新绑定地区
            BindDeptDistricts();
        }

        protected void cvDirectorUser_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!string.IsNullOrEmpty(rcbxDirectorUser.Text))
            {
                UISearchDropdownItem uiSearchObj = new UISearchDropdownItem()
                {
                    ItemText = rcbxDirectorUser.Text.Trim()
                };

                var directorUsers = PageUsersRepository.GetDropdownItems(uiSearchObj);

                if (directorUsers.Count <= 0)
                    args.IsValid = false;
            }
        }

        protected void rgDeptMarketDivisions_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {

            UISearchDeptMarketDivision uiSearchObj = new UISearchDeptMarketDivision()
            {
                DepartmentID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            int totalRecords;

            var deptMarketDivisions = PageDeptMarketDivisionRepository.GetUIList(uiSearchObj,
                rgDeptMarketDivisions.CurrentPageIndex, rgDeptMarketDivisions.PageSize, out totalRecords);

            rgDeptMarketDivisions.DataSource = deptMarketDivisions;
            rgDeptMarketDivisions.VirtualItemCount = totalRecords;

            hdnGridCellValueChangedCount.Value = "0";
        }

        protected void rgDeptMarketDivisions_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                var deptMarketDivision = PageDeptMarketDivisionRepository.GetByID(id);

                if (deptMarketDivision != null)
                {
                    foreach (var deptMarketProduct in deptMarketDivision.DeptMarketProduct)
                    {
                        deptMarketProduct.IsDeleted = true;
                    }

                    deptMarketDivision.IsDeleted = true;

                    PageDeptMarketDivisionRepository.Save();

                    UpdateDeptProductRecord();
                }
            }

            rgDeptProductEvaluations.Rebind();
        }

        protected void rgDeptMarketDivisions_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem parentDataItem = (GridDataItem)e.DetailTableView.ParentItem;

            String sParentID = parentDataItem.GetDataKeyValue("ID").ToString();

            int parentID = 0;
            if (int.TryParse(sParentID, out parentID))
            {
                var uiSearchObj = new UISearchDeptMarketProduct()
                {
                    MarketDivisionID = parentID
                };

                var childEntities = PageDeptMarketProductRepository.GetUIList(uiSearchObj);

                e.DetailTableView.DataSource = childEntities;
            }

            hdnGridCellValueChangedCount.Value = "0";
        }

        protected void rgDeptMarketDivisions_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
        {
            if (e.Commands.Count > 0)
            {
                foreach (var command in e.Commands)
                {
                    switch (command.Type)
                    {
                        case GridBatchEditingCommandType.Update:
                            int dataKeyValue;
                            string sdataKeyValue = command.NewValues["ID"].ToString();

                            if (int.TryParse(sdataKeyValue, out dataKeyValue))
                            {
                                var childEntity = PageDeptMarketProductRepository.GetByID(dataKeyValue);

                                if (childEntity != null)
                                {
                                    string sQ1Task = command.NewValues["Q1Task"] == null
                                        ? string.Empty : command.NewValues["Q1Task"].ToString();
                                    string sQ2Task = command.NewValues["Q2Task"] == null
                                        ? string.Empty : command.NewValues["Q2Task"].ToString();
                                    string sQ3Task = command.NewValues["Q3Task"] == null
                                        ? string.Empty : command.NewValues["Q3Task"].ToString();
                                    string sQ4Task = command.NewValues["Q4Task"] == null
                                        ? string.Empty : command.NewValues["Q4Task"].ToString();

                                    if (!string.IsNullOrEmpty(sQ1Task))
                                    {
                                        int iQ1Task;

                                        if (int.TryParse(sQ1Task, out iQ1Task))
                                            childEntity.Q1Task = iQ1Task;
                                    }
                                    else
                                        childEntity.Q1Task = null;

                                    if (!string.IsNullOrEmpty(sQ2Task))
                                    {
                                        int iQ2Task;

                                        if (int.TryParse(sQ2Task, out iQ2Task))
                                            childEntity.Q2Task = iQ2Task;
                                    }
                                    else
                                        childEntity.Q2Task = null;

                                    if (!string.IsNullOrEmpty(sQ3Task))
                                    {
                                        int iQ3Task;

                                        if (int.TryParse(sQ3Task, out iQ3Task))
                                            childEntity.Q3Task = iQ3Task;
                                    }
                                    else
                                        childEntity.Q3Task = null;

                                    if (!string.IsNullOrEmpty(sQ4Task))
                                    {
                                        int iQ4Task;

                                        if (int.TryParse(sQ4Task, out iQ4Task))
                                            childEntity.Q4Task = iQ4Task;
                                    }
                                    else
                                        childEntity.Q4Task = null;
                                }
                            }

                            break;
                    }
                }

                PageDeptMarketProductRepository.Save();

                UpdateDeptProductRecord();

                hdnGridCellValueChangedCount.Value = "0";
            }
        }

        protected void rgDeptProductEvaluations_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            UISearchDeptProductEvaluation uiSearchObj = new UISearchDeptProductEvaluation()
            {
                DepartmentID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            int totalRecords;

            var deptProductEvaluations = PageDeptProductEvaluationRepository.GetUIList(uiSearchObj,
                rgDeptProductEvaluations.CurrentPageIndex, rgDeptProductEvaluations.PageSize, out totalRecords);

            rgDeptProductEvaluations.DataSource = deptProductEvaluations;
            rgDeptProductEvaluations.VirtualItemCount = totalRecords;
        }

        protected void rgDeptProductEvaluations_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                PageDeptProductEvaluationRepository.DeleteByID(id);
                PageDeptProductEvaluationRepository.Save();
            }

            rgDeptProductEvaluations.Rebind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlDepartmentType.SelectedValue == ((int)EDepartmentType.BaseMedicine).ToString())
            {
                if (string.IsNullOrEmpty(ddlDeptDistrict.SelectedValue))
                    cvDeptDistrict.IsValid = false;
            }

            if (!IsValid) return;

            Department currentEntity = null;

            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                currentEntity = PageDepartmentRepository.GetByID(this.CurrentEntityID);

            if (currentEntity == null)
            {
                currentEntity = new Department();

                PageDepartmentRepository.Add(currentEntity);
            }

            currentEntity.DepartmentName = txtDepartmentName.Text.Trim();
            currentEntity.DepartmentTypeID = int.Parse(ddlDepartmentType.SelectedValue);

            if (!string.IsNullOrEmpty(rcbxDirectorUser.SelectedValue))
            {
                int directorUserID;

                if (int.TryParse(rcbxDirectorUser.SelectedValue, out directorUserID))
                    currentEntity.DirectorUserID = directorUserID;
            }

            if (currentEntity.DepartmentTypeID == (int)EDepartmentType.BaseMedicine)
            {
                if (!string.IsNullOrEmpty(ddlDeptDistrict.SelectedValue))
                {
                    int deptDistrictID;

                    if (int.TryParse(ddlDeptDistrict.SelectedValue, out deptDistrictID))
                        currentEntity.DeptDistrictID = deptDistrictID;
                }
            }
            else
                currentEntity.DeptDistrictID = null;

            PageDepartmentRepository.Save();

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
            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    DbModelContainer db = unitOfWork.GetDbModel();

                    IDepartmentRepository departmentRepository = new DepartmentRepository();
                    IDeptMarketDivisionRepository deptMarketDivisionRepository = new DeptMarketDivisionRepository();
                    IDeptMarketProductRepository deptMarketProductRepository = new DeptMarketProductRepository();
                    IDeptProductEvaluationRepository deptProductEvaluationRepository = new DeptProductEvaluationRepository();
                    IDeptProductRecordRepository deptProductRecordRepository = new DeptProductRecordRepository();

                    departmentRepository.SetDbModel(db);
                    deptMarketDivisionRepository.SetDbModel(db);
                    deptMarketProductRepository.SetDbModel(db);
                    deptProductEvaluationRepository.SetDbModel(db);
                    deptProductRecordRepository.SetDbModel(db);

                    Department currentEntity = PageDepartmentRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        foreach (var item in currentEntity.DeptProductEvaluation)
                        {
                            deptProductEvaluationRepository.Delete(item);
                        }

                        foreach (var item in currentEntity.DepartmentProductRecord)
                        {
                            deptProductRecordRepository.Delete(item);
                        }

                        foreach (var deptUser in currentEntity.Users1)
                        {
                            foreach (var deptMarketDivision in deptUser.DeptMarketDivision)
                            {
                                foreach (var deptMarketProduct in deptMarketDivision.DeptMarketProduct)
                                {
                                    deptMarketProductRepository.Delete(deptMarketProduct);
                                }

                                deptMarketDivisionRepository.Delete(deptMarketDivision);
                            }
                        }

                        departmentRepository.Delete(currentEntity);

                        unitOfWork.SaveChanges();
                    }
                }
            }
        }

    }
}