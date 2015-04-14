using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Web.Extensions;

namespace ZhongDing.Web.Views.Settlements.Editors
{
    public partial class ViewDBClinetBonusGroupPayments : WorkflowBasePage
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
                return WebUtility.GetIntFromQueryString("OwnerEntityID");
            }
        }

        /// <summary>
        /// 转账日期
        /// </summary>
        private DateTime? PayDate
        {
            get
            {
                return WebUtility.GetDateTimeFromQueryString("PayDate");
            }
        }

        #endregion

        #region Members

        private IApplicationPaymentRepository _PageAppPaymentRepository;
        private IApplicationPaymentRepository PageAppPaymentRepository
        {
            get
            {
                if (_PageAppPaymentRepository == null)
                    _PageAppPaymentRepository = new ApplicationPaymentRepository();

                return _PageAppPaymentRepository;
            }
        }

        private IDBClientSettlementRepository _PageDBClientSettlementRepository;
        private IDBClientSettlementRepository PageDBClientSettlementRepository
        {
            get
            {
                if (_PageDBClientSettlementRepository == null)
                    _PageDBClientSettlementRepository = new DBClientSettlementRepository();

                return _PageDBClientSettlementRepository;
            }
        }

        private DBClientSettlement _CurrentOwnerEntity;
        private DBClientSettlement CurrentOwnerEntity
        {
            get
            {
                if (_CurrentOwnerEntity == null)
                    if (this.OwnerEntityID.HasValue && this.OwnerEntityID > 0)
                        _CurrentOwnerEntity = PageDBClientSettlementRepository.GetByID(this.OwnerEntityID);

                return _CurrentOwnerEntity;
            }
        }

        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.DBClientSettleBonus;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.CurrentOwnerEntity == null || !this.PayDate.HasValue)
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_CLOSE_WIN);

                return;
            }
        }

        #region Private Methods


        #endregion

        protected void rgAppPayments_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchApplicationPayment
            {
                WorkflowID = this.CurrentWorkFlowID,
                ApplicationID = this.CurrentOwnerEntity.ID,
                PayDate = this.PayDate
            };

            int totalRecords;

            var appPayments = PageAppPaymentRepository.GetUIList(uiSearchObj, rgAppPayments.CurrentPageIndex, rgAppPayments.PageSize, out totalRecords);

            rgAppPayments.DataSource = appPayments;
            rgAppPayments.VirtualItemCount = totalRecords;
        }

    }
}