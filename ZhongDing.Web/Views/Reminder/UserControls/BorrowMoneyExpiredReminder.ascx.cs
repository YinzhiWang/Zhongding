using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Web.Extensions;

namespace ZhongDing.Web.Views.Reminder.UserControls
{
    public partial class BorrowMoneyExpiredReminder : BaseReminder
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.View;
        }
        protected override EPermission PagePermissionID()
        {
            return EPermission.BorrowMoneyExpiredReminder;
        }
        public override void BindData(bool isBindData)
        {
            base.BindData(isBindData);
            BindReminders(true);
        }


        #region Private Methods

        private void BindReminders(bool isNeedRebind)
        {
            if (!IsBindData) return;


            UISearchBorrowMoneyExpiredReminder uiSearchObj = new UISearchBorrowMoneyExpiredReminder()
            {
                //WarehouseName = txtWarehouseName.Text.Trim(),
                //ProductName = txtProductName.Text.Trim(),
                //CompanyID = CurrentUser.CompanyID
            };

            int totalRecords;

            var entities = PageReminderRepository.GetUIBorrowMoneyExpiredReminder(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

            rgEntities.VirtualItemCount = totalRecords;

            rgEntities.DataSource = entities;

            if (isNeedRebind)
                rgEntities.Rebind();
        }

        #endregion

        protected void rgEntities_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindReminders(false);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindReminders(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            //txtWarehouseName.Text = txtProductName.Text = string.Empty;

            BindReminders(true);
        }
    }
}