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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.DepartmentManage;

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
                ExtensionEntityID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            });

            rcbxDirectorUser.DataSource = deptUsers;
            rcbxDirectorUser.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxDirectorUser.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxDirectorUser.DataBind();
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

                        var deptDistricts = PageDeptDistrictRepository.GetDropdownItems(new UISearchDropdownItem() { ExtensionEntityID = departmentTypeID });

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

        }

        protected void rgDeptMarketDivisions_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                PageDeptMarketDivisionRepository.DeleteByID(id);
                PageDeptMarketDivisionRepository.Save();
            }

            rgDeptProductEvaluations.Rebind();
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

                    departmentRepository.SetDbModel(db);
                    deptMarketDivisionRepository.SetDbModel(db);
                    deptMarketProductRepository.SetDbModel(db);
                    deptProductEvaluationRepository.SetDbModel(db);

                    Department currentEntity = PageDepartmentRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        foreach (var item in currentEntity.DeptProductEvaluation)
                        {
                            deptProductEvaluationRepository.Delete(item);
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