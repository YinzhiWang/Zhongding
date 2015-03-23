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

namespace ZhongDing.Web.Views.Basics
{
    public partial class TransportCompanyManagement : BasePage
    {
        #region Members

        private ITransportCompanyRepository _PageTransportCompanyRepository;
        private ITransportCompanyRepository PageTransportCompanyRepository
        {
            get
            {
                if (_PageTransportCompanyRepository == null)
                    _PageTransportCompanyRepository = new TransportCompanyRepository();

                return _PageTransportCompanyRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.TransportCompanyManage;

        }

        private void BindTransportCompany(bool isNeedRebind)
        {
            UISearchTransportCompany uiSearchObj = new UISearchTransportCompany()
            {
                CompanyName = txtCompanyName.Text.Trim()
            };

            int totalRecords = 0;

            var uiTransportCompanys = PageTransportCompanyRepository.GetUIList(uiSearchObj, rgTransportCompanys.CurrentPageIndex, rgTransportCompanys.PageSize, out totalRecords);

            rgTransportCompanys.VirtualItemCount = totalRecords;

            rgTransportCompanys.DataSource = uiTransportCompanys;

            if (isNeedRebind)
                rgTransportCompanys.Rebind();
        }


        protected void rgTransportCompanys_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindTransportCompany(false);
        }

        protected void rgTransportCompanys_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {

                PageTransportCompanyRepository.DeleteByID(id);
                PageTransportCompanyRepository.Save();
            }

            rgTransportCompanys.Rebind();
        }

        protected void rgTransportCompanys_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgTransportCompanys_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgTransportCompanys_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindTransportCompany(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {

            txtCompanyName.Text = string.Empty;

            BindTransportCompany(true);
        }
    }
}