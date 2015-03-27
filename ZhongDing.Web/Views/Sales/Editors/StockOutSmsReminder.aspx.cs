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
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Web.Extensions;
using ZhongDing.Common.Extension;
using System.Text;

namespace ZhongDing.Web.Views.Sales.Editors
{
    public partial class StockOutSmsReminder : BasePage
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
                return WebUtility.GetValueFromQueryString("OwnerEntityID");
            }
        }



        #endregion

        #region Members

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
        private IStockOutDetailRepository _PageStockOutDetailRepository;
        private IStockOutDetailRepository PageStockOutDetailRepository
        {
            get
            {
                if (_PageStockOutDetailRepository == null)
                    _PageStockOutDetailRepository = new StockOutDetailRepository();

                return _PageStockOutDetailRepository;
            }
        }

        private ITransportFeeStockOutSmsReminderRepository _PageTransportFeeStockOutSmsReminderRepository;
        private ITransportFeeStockOutSmsReminderRepository PageTransportFeeStockOutSmsReminderRepository
        {
            get
            {
                if (_PageTransportFeeStockOutSmsReminderRepository == null)
                    _PageTransportFeeStockOutSmsReminderRepository = new TransportFeeStockOutSmsReminderRepository();

                return _PageTransportFeeStockOutSmsReminderRepository;
            }
        }


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (OwnerEntityID.BiggerThanZero())
            {

                if (!IsPostBack)
                {
                    hdnGridClientID.Value = base.GridClientID;
                    var transportFeeStockOut = PageTransportFeeStockOutRepository.GetByID(OwnerEntityID.Value);
                    var transportFee = PageTransportFeeRepository.GetByID(transportFeeStockOut.TransportFeeID);
                    var stockOutDetails = PageStockOutDetailRepository.GetUIListForSmsReminder(new UISearchStockOutDetail()
                    {
                        StockOutID = transportFeeStockOut.StockOutID
                    });

                    StringBuilder content = new StringBuilder();
                    content.Append("货物已发出:");
                    content.Append(transportFee.TransportCompany.CompanyName);
                    content.Append("(单号:");
                    content.Append(transportFee.TransportCompanyNumber);
                    content.Append(",电话:");
                    content.Append(transportFee.DriverTelephone);
                    content.Append(") ");

                    if (stockOutDetails != null)
                    {
                        foreach (var x in stockOutDetails)
                            content.Append(x.ProductName + "(" + x.NumberOfPackages + x.UnitOfMeasurement + ") ");
                    }

                    txtContent.Text = content.ToString();
                    txtMobileNumber.Text = transportFeeStockOut.StockOut.ReceiverPhone;
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            if (OwnerEntityID.BiggerThanZero())
            {
                TransportFeeStockOutSmsReminder transportFeeStockOutSmsReminder = new TransportFeeStockOutSmsReminder()
                {
                    Content = txtContent.Text.Trim(),
                    MobileNumber = txtMobileNumber.Text.Trim(),
                    TransportFeeStockOutID = OwnerEntityID.Value,
                };

                PageTransportFeeStockOutSmsReminderRepository.Add(transportFeeStockOutSmsReminder);
                PageTransportFeeStockOutSmsReminderRepository.Save();

                var transportFeeStockOut = PageTransportFeeStockOutRepository.GetByID(OwnerEntityID.Value);
                transportFeeStockOut.TransportFeeStockOutSmsReminderID = transportFeeStockOutSmsReminder.ID;
                PageTransportFeeStockOutRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_CLOSE_WIN);
            }
            else
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_CLOSE_WIN);
                return;
            }
        }
    }
}