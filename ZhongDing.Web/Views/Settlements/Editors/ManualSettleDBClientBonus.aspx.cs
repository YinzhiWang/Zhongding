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
using ZhongDing.Web.Extensions;

namespace ZhongDing.Web.Views.Settlements.Editors
{
    public partial class ManualSettleDBClientBonus : WorkflowBasePage
    {
        #region Fields

        /// <summary>
        /// 手动计算动作类型ID
        /// </summary>
        private int? ManualSettleActionTypeID
        {
            get
            {
                return WebUtility.GetIntFromQueryString("ManualSettleActionTypeID");
            }
        }

        #endregion

        #region Members

        private IDBClientSettleBonusRepository _PageDBClientSettleBonusRepository;
        private IDBClientSettleBonusRepository PageDBClientSettleBonusRepository
        {
            get
            {
                if (_PageDBClientSettleBonusRepository == null)
                    _PageDBClientSettleBonusRepository = new DBClientSettleBonusRepository();

                return _PageDBClientSettleBonusRepository;
            }
        }

        private DBClientSettleBonus _CurrentEntity;
        private DBClientSettleBonus CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageDBClientSettleBonusRepository.GetByID(this.CurrentEntityID);

                return _CurrentEntity;
            }
        }

        private IList<int> _CanAccessUserIDs;
        private IList<int> CanAccessUserIDs
        {
            get
            {
                if (_CanAccessUserIDs == null || _CanAccessUserIDs.Count == 0)
                {
                    _CanAccessUserIDs = PageWorkflowStatusRepository.GetCanAccessUserIDsByID(this.CurrentWorkFlowID, this.CurrentEntity.DBClientSettlement.WorkflowStatusID);
                }

                return _CanAccessUserIDs;
            }
        }

        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditDBClientSettleBonus);

                return _CanEditUserIDs;
            }
        }

        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.DBClientSettleBonus;
        }
        protected override EWorkflow PagePermissionWorkflowID()
        {
            return EWorkflow.DBClientSettleBonus;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsNeedHandleData())
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_CLOSE_WIN);

                return;
            }

            if (!hasPermission())
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_NO_PERMISSION_CLOSE_WIN);

                return;
            }

            if (!IsPostBack)
            {
                hdnGridClientID.Value = GridClientID;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            CurrentEntity.Comment += "[操作人：" + CurrentUser.FullName;

            var manualSettleActionType = (EManualSettleActionType)ManualSettleActionTypeID;

            switch (manualSettleActionType)
            {
                case EManualSettleActionType.IncludeSettlement:
                    CurrentEntity.IsManualSettled = true;
                    CurrentEntity.ManualSettledBy = CurrentUser.UserID;
                    CurrentEntity.IsNeedSettlement = true;
                    CurrentEntity.TotalPayAmount = CurrentEntity.DBClientBonus.BonusAmount
                        + CurrentEntity.DBClientBonus.PerformanceAmount;
                    CurrentEntity.Comment += " 点击了 “加入结算”, ";
                    break;
                case EManualSettleActionType.ExcludeSettlement:
                    CurrentEntity.IsNeedSettlement = false;
                    CurrentEntity.IsManualSettled = false;
                    CurrentEntity.TotalPayAmount = null;
                    CurrentEntity.Comment += " 点击了 “取消结算”, ";
                    break;
            }

            CurrentEntity.Comment += "原因是：" + txtComment.Text.Trim() + ";]";

            PageDBClientSettleBonusRepository.Save();

            this.Master.BaseNotification.OnClientHidden = "onClientHidden";
            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_CLOSE_WIN);
        }

        private bool IsNeedHandleData()
        {
            bool isNeedHandle = false;

            if (this.CurrentEntity != null
                && (ManualSettleActionTypeID == (int)EManualSettleActionType.IncludeSettlement
                    || ManualSettleActionTypeID == (int)EManualSettleActionType.ExcludeSettlement))
            {
                if (this.CurrentEntity.DBClientBonus.IsSettled != true)
                {
                    if (ManualSettleActionTypeID == (int)EManualSettleActionType.IncludeSettlement)
                    {
                        if (this.CurrentEntity.IsNeedSettlement != true
                            && this.CurrentEntity.IsManualSettled != true)
                        {
                            isNeedHandle = true;
                        }
                    }
                    else if (ManualSettleActionTypeID == (int)EManualSettleActionType.ExcludeSettlement)
                    {
                        if (this.CurrentEntity.IsManualSettled == true)
                        {
                            isNeedHandle = true;
                        }
                    }
                }
            }

            return isNeedHandle;
        }

        private bool hasPermission()
        {
            if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
                    || this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                && (this.CurrentEntity.DBClientSettlement.WorkflowStatusID == (int)EWorkflowStatus.ToBeSettle
                    || this.CurrentEntity.DBClientSettlement.WorkflowStatusID == (int)EWorkflowStatus.ReturnBasicInfo))
            {
                return true;
            }
            return false;
        }
    }
}