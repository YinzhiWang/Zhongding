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
using ZhongDing.Common.Extension;

namespace ZhongDing.Web.Views.Procures
{
    public partial class TransportFeeManagement : BasePage
    {
        #region Members

        private ITransportFeeRepository _PageTransportFeeRepository;
        private ITransportFeeRepository PageTransportFeeRepository
        {
            get
            {
                if (_PageTransportFeeRepository == null)
                    _PageTransportFeeRepository = new TransportFeeRepository();

                return _PageTransportFeeRepository;
            }
        }

        public ETransportFeeType TransportFeeType
        {
            get
            {
                string sEntityID = Request.QueryString["TransportFeeType"];
                ETransportFeeType eTransportFeeType = Common.Enums.ETransportFeeType.StockIn;
                Enum.TryParse<ETransportFeeType>(sEntityID, out eTransportFeeType);
                return eTransportFeeType;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (TransportFeeType == ETransportFeeType.StockIn)
            {
                this.Master.MenuItemID = (int)EMenuItem.TransportFeeManage_StockIn;
                rcbxTransportFeeType.SelectedValue = ((int)ETransportFeeType.StockIn).ToString();

            }
            else
            {
                this.Master.MenuItemID = (int)EMenuItem.TransportFeeManage_StockOut;
                rcbxTransportFeeType.SelectedValue = ((int)ETransportFeeType.StockOut).ToString();
            }

        }

        private void BindTransportFee(bool isNeedRebind)
        {
            UISearchTransportFee uiSearchObj = new UISearchTransportFee()
            {
                TransportFeeType = rcbxTransportFeeType.SelectedValue.ToIntOrNull().GetValueOrDefault(-1),
                TransportCompanyName = txtTransportCompanyName.Text.Trim(),
                TransportCompanyNumber = txtTransportCompanyNumber.Text.Trim()

            };

            int totalRecords = 0;

            var uiTransportFees = PageTransportFeeRepository.GetUIList(uiSearchObj, rgTransportFees.CurrentPageIndex, rgTransportFees.PageSize, out totalRecords);

            rgTransportFees.VirtualItemCount = totalRecords;

            rgTransportFees.DataSource = uiTransportFees;

            if (isNeedRebind)
                rgTransportFees.Rebind();
        }


        protected void rgTransportFees_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindTransportFee(false);
        }

        protected void rgTransportFees_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {

                PageTransportFeeRepository.DeleteByID(id);
                PageTransportFeeRepository.Save();
            }

            rgTransportFees.Rebind();
        }

        protected void rgTransportFees_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgTransportFees_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgTransportFees_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindTransportFee(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {

            txtTransportCompanyName.Text = txtTransportCompanyNumber.Text = string.Empty;
            rcbxTransportFeeType.SelectedIndex = 0;

            BindTransportFee(true);
        }
    }
}