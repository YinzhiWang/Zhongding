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
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Web.Extensions;

namespace ZhongDing.Web.Views.Procures.Editors
{
    public partial class ChooseProcureOrderProducts : BasePage
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

        /// <summary>
        /// 供应商ID
        /// </summary>
        private int? SupplierID
        {
            get
            {
                string sSupplierID = Request.QueryString["SupplierID"];

                int iSupplierID;

                if (int.TryParse(sSupplierID, out iSupplierID))
                    return iSupplierID;
                else
                    return null;
            }
        }

        #endregion

        #region Members

        private IProcureOrderAppDetailRepository _PageProcureOrderAppDetailRepository;
        private IProcureOrderAppDetailRepository PageProcureOrderAppDetailRepository
        {
            get
            {
                if (_PageProcureOrderAppDetailRepository == null)
                    _PageProcureOrderAppDetailRepository = new ProcureOrderAppDetailRepository();

                return _PageProcureOrderAppDetailRepository;
            }
        }

        private IStockInDetailRepository _PageStockInDetailRepository;
        private IStockInDetailRepository PageStockInDetailRepository
        {
            get
            {
                if (_PageStockInDetailRepository == null)
                    _PageStockInDetailRepository = new StockInDetailRepository();

                return _PageStockInDetailRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.OwnerEntityID.HasValue && this.OwnerEntityID > 0
                && this.SupplierID.HasValue && this.SupplierID > 0)
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

        protected void rgSupplierProcureOrderDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchProcureOrderAppDetail()
            {
                SupplierID = this.SupplierID.HasValue
                ? this.SupplierID.Value : GlobalConst.INVALID_INT
            };

            //var excludeIDs = PageStockInDetailRepository
            //    .GetList(x => x.StockInID == this.OwnerEntityID)
            //    .Select(x => x.ProcureOrderAppDetailID).ToList();

            var uiStockInDetails = (List<UIStockInDetail>)Session[WebUtility.WebSessionNames.StockInDetailData];

            var excludeIDs = uiStockInDetails.Where(x => x.StockInID == this.OwnerEntityID)
                .Select(x => x.ProcureOrderAppDetailID).ToList();

            uiSearchObj.ExcludeIDs = excludeIDs;

            int totalRecords;

            var orderProducts = PageProcureOrderAppDetailRepository.GetToBeInUIList(uiSearchObj,
                rgSupplierProcureOrderDetails.CurrentPageIndex, rgSupplierProcureOrderDetails.PageSize, out totalRecords);

            rgSupplierProcureOrderDetails.DataSource = orderProducts.Where(x => x.ToBeInQty > 0).ToList();

            rgSupplierProcureOrderDetails.VirtualItemCount = totalRecords;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var selectedItems = rgSupplierProcureOrderDetails.SelectedItems;

            var uiStockInDetails = (List<UIStockInDetail>)Session[WebUtility.WebSessionNames.StockInDetailData];

            foreach (var item in selectedItems)
            {
                var editableItem = ((GridEditableItem)item);
                int id = Convert.ToInt32(editableItem.GetDataKeyValue("ID").ToString());
                int procureOrderAppID = Convert.ToInt32(editableItem.GetDataKeyValue("ProcureOrderAppID").ToString());
                int productID = Convert.ToInt32(editableItem.GetDataKeyValue("ProductID").ToString());
                int productSpecificationID = Convert.ToInt32(editableItem.GetDataKeyValue("ProductSpecificationID").ToString());
                int warehouseID = Convert.ToInt32(editableItem.GetDataKeyValue("WarehouseID").ToString());
                int? numberInLargePackage = Convert.ToInt32(editableItem.GetDataKeyValue("NumberInLargePackage") != null
                    ? editableItem.GetDataKeyValue("NumberInLargePackage").ToString() : "1");

                Hashtable newValues = new Hashtable();
                editableItem.ExtractValues(newValues);

                string orderCode = Utility.GetValueFromObject(newValues["OrderCode"]);
                string warehouse = Utility.GetValueFromObject(newValues["Warehouse"]);
                string productName = Utility.GetValueFromObject(newValues["ProductName"]);
                string specification = Utility.GetValueFromObject(newValues["Specification"]);
                string factoryName = Utility.GetValueFromObject(newValues["FactoryName"]);
                string unitOfMeasurement = Utility.GetValueFromObject(newValues["UnitOfMeasurement"]);
                int procureCount = Convert.ToInt32(Utility.GetValueFromObject(newValues["ProcureCount"]));
                decimal procurePrice = Convert.ToDecimal(Utility.GetValueFromObject(newValues["ProcurePrice"]));
                decimal numberOfPackages = Convert.ToDecimal(Utility.GetValueFromObject(newValues["NumberOfPackages"]));
                string licenseNumber = Utility.GetValueFromObject(newValues["LicenseNumber"]);
                int inQty = Convert.ToInt32(Utility.GetValueFromObject(newValues["InQty"]));
                int toBeInQty = Convert.ToInt32(Utility.GetValueFromObject(newValues["ToBeInQty"]));

                if (!uiStockInDetails.Any(x => x.ProcureOrderAppDetailID == id))
                {
                    var uiStockInDetail = new UIStockInDetail
                    {
                        StockInID = this.OwnerEntityID.Value,
                        ProcureOrderAppDetailID = id,
                        ProcureOrderAppID = procureOrderAppID,
                        ProductID = productID,
                        ProductSpecificationID = productSpecificationID,
                        WarehouseID = warehouseID,
                        OrderCode = orderCode,
                        Warehouse = warehouse,
                        ProductName = productName,
                        Specification = specification,
                        FactoryName = factoryName,
                        UnitOfMeasurement = unitOfMeasurement,
                        ProcureCount = procureCount,
                        ProcurePrice = procurePrice,
                        LicenseNumber = licenseNumber,
                        NumberInLargePackage = numberInLargePackage,
                        NumberOfPackages = numberOfPackages,
                        InQty = toBeInQty,
                        ToBeInQty = toBeInQty,
                        IsDeleted = false
                    };

                    uiStockInDetails.Add(uiStockInDetail);
                }
            }

            Session[WebUtility.WebSessionNames.StockInDetailData] = uiStockInDetails;

            this.Master.BaseNotification.OnClientHidden = "onClientHidden";
            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_CLOSE_WIN);
        }

    }
}