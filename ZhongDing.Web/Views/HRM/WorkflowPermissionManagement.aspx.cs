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

namespace ZhongDing.Web.Views.HRM
{
    public partial class WorkflowPermissionManagement : BasePage
    {
        #region Members

        private IWorkflowRepository _PageWorkflowRepository;
        private IWorkflowRepository PageWorkflowRepository
        {
            get
            {
                if (_PageWorkflowRepository == null)
                    _PageWorkflowRepository = new WorkflowRepository();

                return _PageWorkflowRepository;
            }
        }

        private IWorkflowStepRepository _PageWorkflowStepRepository;
        private IWorkflowStepRepository PageWorkflowStepRepository
        {
            get
            {
                if (_PageWorkflowStepRepository == null)
                    _PageWorkflowStepRepository = new WorkflowStepRepository();

                return _PageWorkflowStepRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.WorkflowPermissionManage;

            if (!IsPostBack)
            {
                BindWorkflows();

                BindWorkflowSteps(GlobalConst.INVALID_INT);
            }
        }

        #region Private Methods

        private void BindWorkflows()
        {
            var workflows = PageWorkflowRepository.GetDropdownItems();

            rcbxWorkflow.DataSource = workflows;
            rcbxWorkflow.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxWorkflow.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxWorkflow.DataBind();

            rcbxWorkflow.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindWorkflowSteps(int workflowID)
        {
            rcbxWorkflowStep.ClearSelection();

            var uiSearchObj = new UISearchDropdownItem()
            {
                Extension = new UISearchExtension()
                {
                    WorkflowID = workflowID
                }
            };

            var products = PageWorkflowStepRepository.GetDropdownItems(uiSearchObj);

            rcbxWorkflowStep.DataSource = products;
            rcbxWorkflowStep.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxWorkflowStep.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxWorkflowStep.DataBind();

            rcbxWorkflowStep.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindEntities(bool isNeedRebind)
        {
            UISearchWorkflowStep uiSearchObj = new UISearchWorkflowStep();

            if (!string.IsNullOrEmpty(rcbxWorkflow.SelectedValue))
            {
                int workflowID;
                if (int.TryParse(rcbxWorkflow.SelectedValue, out workflowID))
                    uiSearchObj.WorkfolwID = workflowID;
            }

            if (!string.IsNullOrEmpty(rcbxWorkflowStep.SelectedValue))
            {
                int workflowStepID;
                if (int.TryParse(rcbxWorkflowStep.SelectedValue, out workflowStepID))
                    uiSearchObj.ID = workflowStepID;
            }

            int totalRecords;

            var entities = PageWorkflowStepRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

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

        protected void rcbxWorkflow_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            int workflowID = GlobalConst.INVALID_INT;

            int.TryParse(e.Value, out workflowID);

            BindWorkflowSteps(workflowID);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindEntities(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rcbxWorkflow.ClearSelection();
            rcbxWorkflowStep.ClearSelection();

            BindWorkflowSteps(GlobalConst.INVALID_INT);

            BindEntities(true);
        }

    }
}