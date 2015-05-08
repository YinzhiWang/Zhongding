using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Web.Extensions;

namespace ZhongDing.Web.Views.Procures.Editors
{
    public partial class ChooseStockOutForTransportFee : BasePage
    {
        #region Fields

        /// <summary>
        /// 所属的实体ID
        /// </summary>
        /// <value>The owner entity ID.</value>
        private int? OwnerEntityID
        {
            get
            {
                string sOwnerEntityID = Request.QueryString["OwnerEntityID"];

                int iOwnerEntityID;

                if (int.TryParse(sOwnerEntityID, out iOwnerEntityID))
                    return iOwnerEntityID;
                else
                    return null;
            }
        }
        private IStockOutRepository _PageStockOutRepository;
        private IStockOutRepository PageStockOutRepository
        {
            get
            {
                if (_PageStockOutRepository == null)
                    _PageStockOutRepository = new StockOutRepository();

                return _PageStockOutRepository;
            }
        }
        private ITransportFeeStockOutRepository _PageTransportFeeStockOutRepository;
        private ITransportFeeStockOutRepository PageTransportFeeStockOutRepository
        {
            get
            {
                if (_PageTransportFeeStockOutRepository == null)
                    _PageTransportFeeStockOutRepository = new TransportFeeStockOutRepository();

                return _PageTransportFeeStockOutRepository;
            }
        }

        #endregion

        #region Members


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.OwnerEntityID.HasValue && this.OwnerEntityID > 0)
            {
                if (!IsPostBack)
                {
                    hdnGridClientID.Value = base.GridClientID;
                }
            }
            else
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_CLOSE_WIN);

                return;
            }
        }

        protected void rgStockOuts_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindStockIn(false);
        }

        private void BindStockIn(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchStockOut()
            {
                Code = txtCode.Text.Trim(),
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,
            };


            int totalRecords;

            var items = PageStockOutRepository.GetUIListForTransportFee(uiSearchObj, rgStockOuts.CurrentPageIndex, rgStockOuts.PageSize, out totalRecords);

            rgStockOuts.DataSource = items;

            rgStockOuts.VirtualItemCount = totalRecords;

            if (isNeedRebind)
                rgStockOuts.Rebind();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            var selectedItems = rgStockOuts.SelectedItems;
            int transportFeeId = this.OwnerEntityID.Value;
            foreach (var item in selectedItems)
            {
                var editableItem = ((GridEditableItem)item);
                int stockOutID = Convert.ToInt32(editableItem.GetDataKeyValue("ID").ToString());

                bool exist = PageTransportFeeStockOutRepository.ExistByTransportFeeIdAndStockOutID(transportFeeId, stockOutID);
                if (!exist)
                {
                    TransportFeeStockOut transportFeeStockOut = new TransportFeeStockOut()
                    {
                        TransportFeeID = transportFeeId,
                        StockOutID = stockOutID,
                    };
                    PageTransportFeeStockOutRepository.Add(transportFeeStockOut);
                }
            }
            PageTransportFeeStockOutRepository.Save();
            this.Master.BaseNotification.OnClientHidden = "onClientHidden";
            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_CLOSE_WIN);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindStockIn(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpBeginDate.Clear();
            rdpEndDate.Clear();
            txtCode.Text = string.Empty;
            BindStockIn(true);
        }
    }
}