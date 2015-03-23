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
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Imports
{
    public partial class ImportDCFlowData : BasePage
    {
        #region Members

        private IDCImportFileLogRepository _PageDCImportFileLogRepository;
        private IDCImportFileLogRepository PageDCImportFileLogRepository
        {
            get
            {
                if (_PageDCImportFileLogRepository == null)
                    _PageDCImportFileLogRepository = new DCImportFileLogRepository();

                return _PageDCImportFileLogRepository;
            }
        }

        private IDistributionCompanyRepository _PageDistributionCompanyRepository;
        private IDistributionCompanyRepository PageDistributionCompanyRepository
        {
            get
            {
                if (_PageDistributionCompanyRepository == null)
                    _PageDistributionCompanyRepository = new DistributionCompanyRepository();

                return _PageDistributionCompanyRepository;
            }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.DCFlowData;

            if (!IsPostBack)
            {
                BindDistributionCompanies();
            }
        }

        #region Private Methods

        private void BindDistributionCompanies()
        {
            var distributionCompanies = PageDistributionCompanyRepository.GetDropdownItems();

            rcbxDistributionCompany.DataSource = distributionCompanies;
            rcbxDistributionCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxDistributionCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxDistributionCompany.DataBind();

            rcbxDistributionCompany.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindEntities(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchDCImportFileLog()
            {
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate
            };

            if (rmypSettlementDate.SelectedDate.HasValue)
            {
                var tempDate = rmypSettlementDate.SelectedDate.Value;
                uiSearchObj.SettlementDate = new DateTime(tempDate.Year, tempDate.Month, 1);
            }

            if (!string.IsNullOrEmpty(rcbxDistributionCompany.SelectedValue))
            {
                int distributionCompanyID;
                if (int.TryParse(rcbxDistributionCompany.SelectedValue, out distributionCompanyID))
                    uiSearchObj.DistributionCompanyID = distributionCompanyID;
            }

            int totalRecords;

            var entities = PageDCImportFileLogRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

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

        protected void rgEntities_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                var uiEntity = (UIDCImportFileLog)gridDataItem.DataItem;

                if (uiEntity != null)
                {
                    bool isShowEditLink = false;
                    bool isShowDeleteLink = false;

                    if (uiEntity.ImportStatusID == (int)EImportStatus.ToBeImport)
                    {
                        isShowEditLink = true;
                        isShowDeleteLink = true;
                    }

                    var editColumn = rgEntities.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT);

                    if (editColumn != null)
                    {
                        var editCell = gridDataItem.Cells[editColumn.OrderIndex];

                        if (editCell != null && !isShowEditLink)
                            editCell.Text = string.Empty;
                    }

                    var deleteColumn = rgEntities.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE);

                    if (deleteColumn != null)
                    {
                        var deleteCell = gridDataItem.Cells[deleteColumn.OrderIndex];

                        if (deleteCell != null && !isShowDeleteLink)
                            deleteCell.Text = string.Empty;
                    }
                }
            }
        }

        protected void rgEntities_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                var entity = PageDCImportFileLogRepository.GetByID(id);

                if (entity!=null)
                {
                    entity.ImportFileLog.IsDeleted = true;
                    entity.IsDeleted = true;

                    Utility.DeleteFile(entity.ImportFileLog.FilePath);

                    PageDCImportFileLogRepository.Save();
                }

                rgEntities.Rebind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindEntities(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rmypSettlementDate.Clear();

            rdpBeginDate.Clear();
            rdpEndDate.Clear();

            rcbxDistributionCompany.ClearSelection();

            BindEntities(true);
        }
    }
}