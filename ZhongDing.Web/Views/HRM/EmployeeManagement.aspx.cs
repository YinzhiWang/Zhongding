using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.HRM
{
    public partial class EmployeeManagement : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.EmployeeManage;
        }

        #region Private Methods

        private void BindEntities(bool isNeedRebind)
        {
            UISearchUser uiSearchObj = new UISearchUser()
            {
                FullName = txtFullName.Text.Trim(),
                UserName = txtUserName.Text.Trim()
            };

            int totalRecords;

            var entities = PageUsersRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

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

                    IUsersRepository userRepository = new UsersRepository();
                    IDeptMarketDivisionRepository deptMarketDivisionRepository = new DeptMarketDivisionRepository();

                    userRepository.SetDbModel(db);
                    deptMarketDivisionRepository.SetDbModel(db);

                    var user = userRepository.GetByID(this.CurrentEntityID);

                    if (user != null)
                    {
                        foreach (var item in user.DeptMarketDivision)
                        {
                            deptMarketDivisionRepository.Delete(item);
                        }

                        if (user.aspnet_Users != null && user.aspnet_Users.aspnet_Membership != null)
                            user.aspnet_Users.aspnet_Membership.IsApproved = false;

                        userRepository.Delete(user);
                    }

                    unitOfWork.SaveChanges();
                }
            }

            rgEntities.Rebind();
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
            txtFullName.Text = string.Empty;
            txtUserName.Text = string.Empty;

            BindEntities(true);
        }
    }
}