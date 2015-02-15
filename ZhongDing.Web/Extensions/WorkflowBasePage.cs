using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;

namespace ZhongDing.Web
{
    public class WorkflowBasePage : BasePage
    {
        #region 属性

        private int _CurrentWorkflowID;
        /// <summary>
        /// 当前工作流ID
        /// </summary>
        /// <value>The current work flow ID.</value>
        protected int CurrentWorkFlowID
        {
            get
            {
                if (_CurrentWorkflowID <= 0)
                    _CurrentWorkflowID = GetCurrentWorkFlowID();

                return _CurrentWorkflowID;
            }
        }

        private IWorkflowRepository _PageWorkflowRepository;
        protected IWorkflowRepository PageWorkflowRepository
        {
            get
            {
                if (_PageWorkflowRepository == null)
                    _PageWorkflowRepository = new WorkflowRepository();

                return _PageWorkflowRepository;
            }
        }

        private IWorkflowStepRepository _PageWorkflowStepRepository;
        protected IWorkflowStepRepository PageWorkflowStepRepository
        {
            get
            {
                if (_PageWorkflowStepRepository == null)
                    _PageWorkflowStepRepository = new WorkflowStepRepository();

                return _PageWorkflowStepRepository;
            }
        }

        private IWorkflowStatusRepository _PageWorkflowStatusRepository;
        protected IWorkflowStatusRepository PageWorkflowStatusRepository
        {
            get
            {
                if (_PageWorkflowStatusRepository == null)
                    _PageWorkflowStatusRepository = new WorkflowStatusRepository();

                return _PageWorkflowStatusRepository;
            }
        }

        private IApplicationNoteRepository _PageAppNoteRepository;
        protected IApplicationNoteRepository PageAppNoteRepository
        {
            get
            {
                if (_PageAppNoteRepository == null)
                    _PageAppNoteRepository = new ApplicationNoteRepository();

                return _PageAppNoteRepository;
            }
        }

        /// <summary>
        /// 虚方法：获取当前工作流ID
        /// </summary>
        protected virtual int GetCurrentWorkFlowID()
        {
            return GlobalConst.INVALID_INT;
        }

        #endregion

    }
}