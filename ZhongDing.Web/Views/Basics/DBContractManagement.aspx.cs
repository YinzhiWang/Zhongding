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
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Basics
{
    public partial class DBContractManagement : BasePage
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.DBContractManage;

            if (!IsPostBack)
            {
                BindClientUsers();

                BindDepartments();
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

        private void BindEntities(bool isNeedRebind)
        {
            UISearchDBContract uiSearchObj = new UISearchDBContract()
            {
                ContractCode = txtContractCode.Text.Trim(),
                ProductName = txtProductName.Text.Trim()
            };

            if (!string.IsNullOrEmpty(rcbxClientUser.SelectedValue))
            {
                int clientUserID;
                if (int.TryParse(rcbxClientUser.SelectedValue, out clientUserID))
                    uiSearchObj.ClientUserID = clientUserID;
            }

            if (!string.IsNullOrEmpty(rcbxDepartment.SelectedValue))
            {
                int departmentID;
                if (int.TryParse(rcbxDepartment.SelectedValue, out departmentID))
                    uiSearchObj.DepartmentID = departmentID;
            }

            int totalRecords;

            var entities = PageDBContractRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

            rgEntities.VirtualItemCount = totalRecords;

            rgEntities.DataSource = entities;

            if (isNeedRebind)
                rgEntities.Rebind();
        }

        #endregion

        protected void rgEntities_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindEntities(false);
        }

        protected void rgEntities_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IDBContractRepository dbContractRepository = new DBContractRepository();
                    IDBContractTaskAssignmentRepository dBContractTaskAssignmentRepository = new DBContractTaskAssignmentRepository();
                    IHospitalRepository hospitalRepository = new HospitalRepository();

                    dbContractRepository.SetDbModel(db);
                    dBContractTaskAssignmentRepository.SetDbModel(db);
                    hospitalRepository.SetDbModel(db);

                    var currentEntity = dbContractRepository.GetByID(id);

                    if (currentEntity != null)
                    {
                        foreach (var taskAssignment in currentEntity.DBContractTaskAssignment)
                        {
                            dBContractTaskAssignmentRepository.Delete(taskAssignment);
                        }

                        foreach (var hospital in currentEntity.Hospital)
                        {
                            hospitalRepository.Delete(hospital);
                        }

                        dbContractRepository.Delete(currentEntity);

                        unitOfWork.SaveChanges();
                    }
                }

                rgEntities.Rebind();
            }
        }

        protected void rgEntities_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgEntities_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgEntities_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindEntities(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtContractCode.Text = string.Empty;
            rcbxClientUser.ClearSelection();
            txtProductName.Text = string.Empty;
            rcbxDepartment.ClearSelection();

            BindEntities(true);
        }
    }
}