using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Common.Extension;
using Telerik.Web.UI;
using ZhongDing.Web.Extensions;
using ZhongDing.Common;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Web.Views.CautionMoneys
{
    public partial class ClientCautionMoneyManagement : BasePage
    {
        #region Members

        private IClientCautionMoneyRepository _PageClientCautionMoneyRepository;
        private IClientCautionMoneyRepository PageClientCautionMoneyRepository
        {
            get
            {
                if (_PageClientCautionMoneyRepository == null)
                    _PageClientCautionMoneyRepository = new ClientCautionMoneyRepository();

                return _PageClientCautionMoneyRepository;
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
            this.Master.MenuItemID = (int)EMenuItem.ClientCautionMoneyManage;
            if (!IsPostBack)
            {
                BindDepartments();
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
        private void BindClientCautionMoney(bool isNeedRebind)
        {
            UISearchClientCautionMoney uiSearchObj = new UISearchClientCautionMoney()
            {
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,
                //WorkflowStatusID = (int)EWorkflowStatus.Paid,
                //NeedStatistics = true,
                ClientName = txtClientName.Text.Trim(),
                ProductName = txtProductName.Text.Trim(),
                DepartmentID = rcbxDepartment.SelectedValue.ToIntOrNull()
            };

            int totalRecords = 0;

            var uiClientCautionMoneys = PageClientCautionMoneyRepository.GetUIList(uiSearchObj, rgClientCautionMoneys.CurrentPageIndex, rgClientCautionMoneys.PageSize, out totalRecords);

            rgClientCautionMoneys.VirtualItemCount = totalRecords;

            rgClientCautionMoneys.DataSource = uiClientCautionMoneys;


            if (isNeedRebind)
                rgClientCautionMoneys.Rebind();
        }


        protected void rgClientCautionMoneys_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindClientCautionMoney(false);
        }

        protected void rgClientCautionMoneys_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;
            var ClientCautionMoneyID = editableItem.GetDataKeyValue("ID").ToIntOrNull();

            if (ClientCautionMoneyID.BiggerThanZero())
            {

                PageClientCautionMoneyRepository.Save();
                rgClientCautionMoneys.Rebind();
            }


        }

        protected void rgClientCautionMoneys_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgClientCautionMoneys_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgClientCautionMoneys_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindClientCautionMoney(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpEndDate.SelectedDate = rdpBeginDate.SelectedDate = null;

            txtProductName.Text = txtClientName.Text = string.Empty;

            BindClientCautionMoney(true);
        }
    }
}