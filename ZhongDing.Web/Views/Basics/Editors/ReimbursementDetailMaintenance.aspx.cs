using System;
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
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Common.Extension;

namespace ZhongDing.Web.Views.Basics.Editors
{
    public partial class ReimbursementDetailMaintenance : WorkflowBasePage
    {
        #region Fields

        /// <summary>
        /// 所有者类型ID
        /// </summary>
        /// <value>The owner type ID.</value>
        private int? OwnerTypeID
        {
            get
            {
                string sOwnerTypeID = Request.QueryString["OwnerTypeID"];

                int iOwnerTypeID;

                if (int.TryParse(sOwnerTypeID, out iOwnerTypeID))
                    return iOwnerTypeID;
                else
                    return null;
            }
        }

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

        #endregion

        #region Members

        private ISupplierRepository _PageSupplierRepository;
        private ISupplierRepository PageSupplierRepository
        {
            get
            {
                if (_PageSupplierRepository == null)
                {
                    _PageSupplierRepository = new SupplierRepository();
                }

                return _PageSupplierRepository;
            }
        }

        private IClientInfoRepository _PageClientInfoRepository;
        private IClientInfoRepository PageClientInfoRepository
        {
            get
            {
                if (_PageClientInfoRepository == null)
                {
                    _PageClientInfoRepository = new ClientInfoRepository();
                }

                return _PageClientInfoRepository;
            }
        }

        private ITransportFeeRepository _PageTransportFeeRepository;
        private ITransportFeeRepository PageTransportFeeRepository
        {
            get
            {
                if (_PageTransportFeeRepository == null)
                {
                    _PageTransportFeeRepository = new TransportFeeRepository();
                }

                return _PageTransportFeeRepository;
            }
        }

        private IReimbursementDetailRepository _PageReimbursementDetailRepository;
        private IReimbursementDetailRepository PageReimbursementDetailRepository
        {
            get
            {
                if (_PageReimbursementDetailRepository == null)
                {
                    _PageReimbursementDetailRepository = new ReimbursementDetailRepository();
                }

                return _PageReimbursementDetailRepository;
            }
        }

        private IReimbursementTypeRepository _PageReimbursementTypeRepository;
        private IReimbursementTypeRepository PageReimbursementTypeRepository
        {
            get
            {
                if (_PageReimbursementTypeRepository == null)
                {
                    _PageReimbursementTypeRepository = new ReimbursementTypeRepository();
                }

                return _PageReimbursementTypeRepository;
            }
        }





        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //if ((!this.OwnerEntityID.HasValue
            //    || this.OwnerEntityID <= 0)
            //    || (!this.OwnerTypeID.HasValue
            //    || this.OwnerTypeID <= 0))
            //{
            //    this.Master.BaseNotification.OnClientHidden = "onError";
            //    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
            //    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_CLOSE_WIN);

            //    return;
            //}

            if (!IsPostBack)
            {
                hdnGridClientID.Value = GridClientID;

                LoadData();
            }
        }

        protected void rcbxReimbursementType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var id = rcbxReimbursementType.SelectedValue.ToIntOrNull();
            if (id.BiggerThanZero())
            {
                BindReimbursementChildTypes(id.Value);
            }
            SwitchDiv();
        }

        private void BindReimbursementTypes()
        {
            var reimbursementTypes = PageReimbursementTypeRepository.GetDropdownItems(new UISearchDropdownItem() { Extension = new UISearchExtension() { ParentID = null } });
            rcbxReimbursementType.DataSource = reimbursementTypes;
            rcbxReimbursementType.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxReimbursementType.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxReimbursementType.DataBind();

            rcbxReimbursementType.Items.Insert(0, new RadComboBoxItem("", ""));
        }
        private void BindReimbursementChildTypes(int parentID)
        {
            var reimbursementTypes = PageReimbursementTypeRepository.GetDropdownItems(new UISearchDropdownItem() { Extension = new UISearchExtension() { ParentID = parentID } });
            rcbxReimbursementChildType.DataSource = reimbursementTypes;
            rcbxReimbursementChildType.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxReimbursementChildType.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxReimbursementChildType.DataBind();

            rcbxReimbursementChildType.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void LoadData()
        {
            BindReimbursementTypes();
            if (this.CurrentEntityID.BiggerThanZero())
            {
                var currentEntity = PageReimbursementDetailRepository.GetByID(this.CurrentEntityID);
                rcbxReimbursementType.SelectedValue = currentEntity.ReimbursementTypeID.ToString();
                rcbxReimbursementChildType.SelectedValue = currentEntity.ReimbursementTypeChildID.ToStringOrNull();
                BindReimbursementChildTypes(currentEntity.ReimbursementTypeID);

                txtAmount.Value = currentEntity.Amount.ToDoubleOrNull();
                txtComment.Text = currentEntity.Comment;
                txtStartDate.SelectedDate = currentEntity.StartDate;
                txtEndDate.SelectedDate = currentEntity.EndDate;
                txtQuantity.Value = currentEntity.Quantity.ToDoubleOrNull();

            }

            SwitchDiv();
        }

        private void SwitchDiv()
        {
            if (rcbxReimbursementType.SelectedValue.ToIntOrNull().BiggerThanZero())
            {
                if (rcbxReimbursementType.SelectedItem.Text == "物流费用")
                {
                    divTransportFee.Visible = true;
                    divOtherTypes.Visible = false;
                    BindTransportFees(true);
                }
                else
                {
                    divTransportFee.Visible = false;
                    divOtherTypes.Visible = true;
                }
            }
            else
            {
                divTransportFee.Visible = false;
                divOtherTypes.Visible = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!rcbxReimbursementType.SelectedValue.ToIntOrNull().BiggerThanZero())
            {
                cvReimbursementType.IsValid = false;
            }
            if (txtAmount.Value.HasValue && txtAmount.Value.Value <= 0)
            {
                rfvAmount.IsValid = false;
                rfvAmount.ErrorMessage = "金额必须大于0";
            }
            if (rcbxReimbursementType.SelectedItem.Text == GlobalConst.REIMBURSEMENTTYPE_EVECTION)
            {
                if (!txtStartDate.SelectedDate.HasValue)
                {
                    cvStartDate.IsValid = false;
                }
                if (!txtEndDate.SelectedDate.HasValue)
                {
                    cvEndDate.IsValid = false;
                }

                if (txtStartDate.SelectedDate.HasValue && txtEndDate.SelectedDate.HasValue &&
                    (txtEndDate.SelectedDate.Value - txtStartDate.SelectedDate.Value).Days > 60)
                {
                    cvEndDate.ErrorMessage = "时间间隔不能超过2个月";
                    cvEndDate.IsValid = false;
                }
            }
            if (!IsValid) return;

            bool isSuccessSaved = false;

            using (IUnitOfWork unitOfWork = new UnitOfWork())
            {
                DbModelContainer db = unitOfWork.GetDbModel();
                IReimbursementDetailRepository reimbursementDetailRepository = new ReimbursementDetailRepository();
                IReimbursementDetailTransportFeeRepository reimbursementDetailTransportFeeRepository = new ReimbursementDetailTransportFeeRepository();
                reimbursementDetailRepository.SetDbModel(db);
                reimbursementDetailTransportFeeRepository.SetDbModel(db);



                if (rcbxReimbursementType.SelectedItem.Text == GlobalConst.REIMBURSEMENTTYPE_TRANSPORTFEE)
                {
                    var selectedItems = rgTransportFees.Items;
                    foreach (var item in selectedItems)
                    {
                        var editableItem = ((GridEditableItem)item);
                        int transportFeeID = editableItem.GetDataKeyValue("ID").ToInt();
                        int reimbursementDetailTransportFeeID = editableItem.GetDataKeyValue("ReimbursementDetailTransportFeeID").ToInt();
                        int reimbursementDetailID = editableItem.GetDataKeyValue("ReimbursementDetailID").ToInt();
                        DateTime sendDate = editableItem.GetDataKeyValue("SendDate").ToDateTime();
                        var comment = (RadTextBox)editableItem.FindControl("txtComment");
                        var amount = editableItem.GetDataKeyValue("Fee").ToString().ToDecimal();
                        var cBoxHasSelect = (CheckBox)editableItem.FindControl("cBoxHasSelect");
                        if (cBoxHasSelect.Checked)
                        {
                            ReimbursementDetail reimbursementDetail = null;
                            {
                                if (reimbursementDetailID > 0)
                                    reimbursementDetail = reimbursementDetailRepository.GetByID(reimbursementDetailID);
                                else
                                {
                                    reimbursementDetail = new ReimbursementDetail()
                                    {
                                        ReimbursementID = this.OwnerEntityID.Value
                                    };
                                    reimbursementDetailRepository.Add(reimbursementDetail);
                                }
                                reimbursementDetail.Amount = reimbursementDetail.ReimbursementDetailTransportFee.Where(x => x.IsDeleted == false).Any() ?
                                reimbursementDetail.ReimbursementDetailTransportFee.Where(x => x.IsDeleted == false).Sum(x => x.Amount) : 0M;
                                reimbursementDetail.ReimbursementTypeID = rcbxReimbursementType.SelectedValue.ToInt();
                                reimbursementDetail.ReimbursementTypeChildID = rcbxReimbursementChildType.SelectedValue.ToIntOrNull();
                                //Reset filed
                                reimbursementDetail.Comment = comment.Text.Trim();
                                //reimbursementDetail.StartDate = sendDate;
                                //reimbursementDetail.EndDate = sendDate;
                                //reimbursementDetail.Quantity = 1;
                                reimbursementDetail.Amount = amount;
                            }
                            {
                                ReimbursementDetailTransportFee reimbursementDetailTransportFee = null;
                                if (reimbursementDetailTransportFeeID > 0)
                                    reimbursementDetailTransportFee = reimbursementDetailTransportFeeRepository.GetByID(reimbursementDetailTransportFeeID);
                                else
                                {
                                    reimbursementDetailTransportFee = new ReimbursementDetailTransportFee()
                                    {
                                        ReimbursementDetail = reimbursementDetail,
                                        TransportFeeID = transportFeeID
                                    };
                                    reimbursementDetailTransportFeeRepository.Add(reimbursementDetailTransportFee);
                                }
                                reimbursementDetailTransportFee.Amount = amount;
                                reimbursementDetailTransportFee.Comment = comment.Text.Trim();
                            }

                        }
                        else
                        {
                            if (reimbursementDetailTransportFeeID > 0)
                                reimbursementDetailTransportFeeRepository.DeleteByID(reimbursementDetailTransportFeeID);
                        }
                        unitOfWork.SaveChanges();
                    }

                    unitOfWork.SaveChanges();

                }
                else
                {
                    ReimbursementDetail reimbursementDetail = null;

                    if (this.CurrentEntityID.BiggerThanZero())
                        reimbursementDetail = reimbursementDetailRepository.GetByID(this.CurrentEntityID);
                    else
                    {
                        reimbursementDetail = new ReimbursementDetail()
                        {
                            ReimbursementID = this.OwnerEntityID.Value
                        };
                        reimbursementDetailRepository.Add(reimbursementDetail);
                    }


                    //ReimbursementType1 是first type
                    if (reimbursementDetail.ReimbursementTypeObject != null &&
                        reimbursementDetail.ReimbursementTypeObject.Name == GlobalConst.REIMBURSEMENTTYPE_TRANSPORTFEE)
                    {
                        var reimbursementDetailTransportFees = reimbursementDetail.ReimbursementDetailTransportFee.Where(x => x.IsDeleted == false).ToList();
                        reimbursementDetailTransportFees.ForEach(x =>
                        {
                            reimbursementDetailTransportFeeRepository.Delete(x);
                        });
                    }

                    reimbursementDetail.ReimbursementTypeID = rcbxReimbursementType.SelectedValue.ToInt();
                    reimbursementDetail.ReimbursementTypeChildID = rcbxReimbursementChildType.SelectedValue.ToIntOrNull();


                    reimbursementDetail.Amount = txtAmount.Value.ToDecimal();
                    reimbursementDetail.Comment = txtComment.Text.Trim();
                    reimbursementDetail.StartDate = txtStartDate.SelectedDate;
                    reimbursementDetail.EndDate = txtEndDate.SelectedDate;
                    reimbursementDetail.Quantity = txtQuantity.Value.ToIntOrNull();
                    unitOfWork.SaveChanges();
                }


                // hdnSupplierID.Value = reimbursementDetail.ID.ToString();
                unitOfWork.SaveChanges();
                isSuccessSaved = true;
            }
            if (isSuccessSaved)
            {
                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_CLOSE_WIN);
            }
            else
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_FAILED_SAEVED_CLOSE_WIN);
            }
        }


        private void BindTransportFees(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchTransportFee()
            {
                //SupplierID = rcbxSupplier.SelectedValue.ToIntOrNull().GetValueOrDefault(0)
                UserID = CurrentUser.UserID,
                ReimbursementDetailID = CurrentEntityID,
            };

            int totalRecords;

            var TransportFees = PageTransportFeeRepository.GetUIListReimbursementDetail(uiSearchObj,
                rgTransportFees.CurrentPageIndex, rgTransportFees.PageSize, out totalRecords);


            rgTransportFees.VirtualItemCount = totalRecords;
            rgTransportFees.DataSource = TransportFees;

            if (isNeedRebind)
                rgTransportFees.Rebind();

        }
        protected void rgTransportFees_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            //if (rcbxSupplier.SelectedValue.ToIntOrNull().BiggerThanZero())
            BindTransportFees(false);
        }

        protected void rgTransportFees_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            //if (this.CurrentOwnerEntity != null
            //    && this.CurrentOwnerEntity.ReceiverTypeID == (int)EReceiverType.ClientUser)
            //    e.OwnerTableView.Columns.FindByUniqueName("CurrentTaxQty").Visible = true;
            //else
            //    e.OwnerTableView.Columns.FindByUniqueName("CurrentTaxQty").Visible = false;
        }

        protected void rgTransportFees_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;


            }
        }




        protected override EWorkflow PagePermissionWorkflowID()
        {
            return EWorkflow.ReimbursementManagement;
        }
        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.ReimbursementManagement;
        }



    }
}