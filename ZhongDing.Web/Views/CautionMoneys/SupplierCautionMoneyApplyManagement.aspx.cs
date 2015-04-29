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
    public partial class SupplierCautionMoneyApplyManagement : BasePage
    {
        #region Members

        private ISupplierCautionMoneyRepository _PageSupplierCautionMoneyRepository;
        private ISupplierCautionMoneyRepository PageSupplierCautionMoneyRepository
        {
            get
            {
                if (_PageSupplierCautionMoneyRepository == null)
                    _PageSupplierCautionMoneyRepository = new SupplierCautionMoneyRepository();

                return _PageSupplierCautionMoneyRepository;
            }
        }


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.SupplierCautionMoneyApplyManage;
            if (!IsPostBack)
            {

            }
        }

        private void BindSupplierCautionMoney(bool isNeedRebind)
        {
            UISearchSupplierCautionMoney uiSearchObj = new UISearchSupplierCautionMoney()
            {
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,

            };

            int totalRecords = 0;

            var uiSupplierCautionMoneys = PageSupplierCautionMoneyRepository.GetSupplierCautionMoneyApplyUIList(uiSearchObj, rgSupplierCautionMoneys.CurrentPageIndex, rgSupplierCautionMoneys.PageSize, out totalRecords);

            rgSupplierCautionMoneys.VirtualItemCount = totalRecords;

            rgSupplierCautionMoneys.DataSource = uiSupplierCautionMoneys;


            if (isNeedRebind)
                rgSupplierCautionMoneys.Rebind();
        }


        protected void rgSupplierCautionMoneys_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindSupplierCautionMoney(false);
        }

        protected void rgSupplierCautionMoneys_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;
            var SupplierCautionMoneyID = editableItem.GetDataKeyValue("ID").ToIntOrNull();

            if (SupplierCautionMoneyID.BiggerThanZero())
            {

                PageSupplierCautionMoneyRepository.Save();
                rgSupplierCautionMoneys.Rebind();
            }


        }

        protected void rgSupplierCautionMoneys_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgSupplierCautionMoneys_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgSupplierCautionMoneys_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindSupplierCautionMoney(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpEndDate.SelectedDate = rdpBeginDate.SelectedDate = null;

            txtProductName.Text = txtSupplierName.Text = string.Empty;

            BindSupplierCautionMoney(true);
        }
    }
}