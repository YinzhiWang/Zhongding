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
using ZhongDing.Common.Extension;
using Telerik.Web.UI;

namespace ZhongDing.Web.Views.Basics
{
    public partial class FixedAssetsMaintenance : BasePage
    {

        #region Members

        private IFixedAssetsRepository _PageFixedAssetsRepository;
        private IFixedAssetsRepository PageFixedAssetsRepository
        {
            get
            {
                if (_PageFixedAssetsRepository == null)
                    _PageFixedAssetsRepository = new FixedAssetsRepository();
                return _PageFixedAssetsRepository;
            }
        }
        private IDepartmentRepository _PageDepartmentRepository;
        private IDepartmentRepository PageDepartmentRepository
        {
            get
            {
                if (_PageDepartmentRepository == null)
                    _PageDepartmentRepository = new DepartmentRepository();

                return _PageDepartmentRepository;
            }
        }

        private IStorageLocationRepository _PageStorageLocationRepository;
        private IStorageLocationRepository PageStorageLocationRepository
        {
            get
            {
                if (_PageStorageLocationRepository == null)
                    _PageStorageLocationRepository = new StorageLocationRepository();

                return _PageStorageLocationRepository;
            }
        }

        private IFixedAssetsTypeRepository _PageFixedAssetsTypeRepository;
        private IFixedAssetsTypeRepository PageFixedAssetsTypeRepository
        {
            get
            {
                if (_PageFixedAssetsTypeRepository == null)
                    _PageFixedAssetsTypeRepository = new FixedAssetsTypeRepository();

                return _PageFixedAssetsTypeRepository;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.FixedAssetsManage;

            if (!IsPostBack)
            {
                BindDepartments();
                BindStorageLocations();
                BindFixedAssetsTypes();
                LoadEntityData();
                base.PermissionOptionCheckButtonDelete(btnDelete);
            }

        }
        private void BindDepartments()
        {
            var departments = PageDepartmentRepository.GetDropdownItems();

            rcbxDepartment.DataSource = departments;
            rcbxDepartment.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxDepartment.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxDepartment.DataBind();

            rcbxDepartment.Items.Insert(0, new RadComboBoxItem("", ""));
        }
        private void BindStorageLocations()
        {
            var departments = PageStorageLocationRepository.GetDropdownItems();

            rcbStorageLocation.DataSource = departments;
            rcbStorageLocation.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbStorageLocation.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbStorageLocation.DataBind();

            rcbStorageLocation.Items.Insert(0, new RadComboBoxItem("", ""));
        }
        private void BindFixedAssetsTypes()
        {
            var departments = PageFixedAssetsTypeRepository.GetDropdownItems();

            rcbxFixedAssetsType.DataSource = departments;
            rcbxFixedAssetsType.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxFixedAssetsType.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxFixedAssetsType.DataBind();

            rcbxFixedAssetsType.Items.Insert(0, new RadComboBoxItem("", ""));
        }
        private void LoadEntityData()
        {

            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                var currentEntity = PageFixedAssetsRepository.GetByID(this.CurrentEntityID);

                if (currentEntity != null)
                {
                    txtCode.Text = currentEntity.Code;
                    rcbxFixedAssetsType.SelectedValue = currentEntity.FixedAssetsTypeID.ToString();
                    txtName.Text = currentEntity.Name;
                    txtUnit.Text = currentEntity.Unit;
                    txtQuantity.Value = currentEntity.Quantity;
                    txtSpecification.Text = currentEntity.Specification;
                    txtProducingArea.Text = currentEntity.ProducingArea;
                    txtManufacturer.Text = currentEntity.Manufacturer;
                    rcbUseStatus.SelectedValue = currentEntity.UseStatus.ToString();
                    rcbxDepartment.SelectedValue = currentEntity.DepartmentID.ToString();
                    txtUsePeople.Text = currentEntity.UsePeople;
                    rcbStorageLocation.SelectedValue = currentEntity.StorageLocationID.ToString();
                    //currentEntity.DepreciationType = (int)EDepreciationType.Line;
                    txtOriginalValue.Value = currentEntity.OriginalValue.ToDouble();
                    txtStartUsedDate.SelectedDate = currentEntity.StartUsedDate;
                    txtEstimateNetSalvageValue.Value = currentEntity.EstimateNetSalvageValue.ToDouble();
                    txtEstimateUsedYear.Value = currentEntity.EstimateUsedYear;

                    txtRemark.Text = currentEntity.Comment;

                    decimal allDepreciation = GetAllDepreciation(currentEntity.StartUsedDate, currentEntity.OriginalValue, currentEntity.EstimateNetSalvageValue, currentEntity.EstimateUsedYear);
                    txtAllDepreciation.Text = allDepreciation.ToString("C");
                    decimal netValue = GetNetValue(currentEntity.StartUsedDate, currentEntity.OriginalValue, currentEntity.EstimateNetSalvageValue, currentEntity.EstimateUsedYear);
                    txtNetValue.Text = netValue.ToString("C");
                }
            }
            else
            {
                btnDelete.Visible = false;
            }
        }

        private decimal GetNetValue(DateTime startUsedDate, decimal originalValue, decimal estimateNetSalvageValue, int estimateUsedYear)
        {
            decimal allDepreciation = GetAllDepreciation(startUsedDate, originalValue, estimateNetSalvageValue, estimateUsedYear);

            decimal netValue = originalValue - allDepreciation;
            return netValue;
        }

        private decimal GetAllDepreciation(DateTime startUsedDate, decimal originalValue, decimal estimateNetSalvageValue, int estimateUsedYear)
        {
            int monthOfUsed = (int)((DateTime.Now - startUsedDate).TotalDays / 30);
            if (monthOfUsed <= 0)
                return originalValue;
            decimal depreciationOfMonth = (originalValue - estimateNetSalvageValue) / estimateUsedYear / 12;

            return depreciationOfMonth * monthOfUsed;
        }


        protected void cvCode_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (PageFixedAssetsRepository.GetList(x => x.ID != this.CurrentEntityID
                && x.Code == txtCode.Text.Trim()).Count() > 0)
            {
                args.IsValid = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!rcbUseStatus.SelectedValue.ToIntOrNull().BiggerThanZero())
            {
                cvUseStatus.IsValid = false;
            }

            if (!rcbxFixedAssetsType.SelectedValue.ToIntOrNull().BiggerThanZero())
            {
                cvFixedAssetsType.IsValid = false;
            }

            if (!rcbxDepartment.SelectedValue.ToIntOrNull().BiggerThanZero())
            {
                cvDepartment.IsValid = false;
            }

            if (!rcbStorageLocation.SelectedValue.ToIntOrNull().BiggerThanZero())
            {
                cvStorageLocation.IsValid = false;
            }
            if (!IsValid) return;

            FixedAssets currentEntity = null;

            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
                currentEntity = PageFixedAssetsRepository.GetByID(this.CurrentEntityID);
            else
            {
                currentEntity = new FixedAssets();
                PageFixedAssetsRepository.Add(currentEntity);
            }

            if (currentEntity != null)
            {
                currentEntity.Code = txtCode.Text.Trim();
                currentEntity.FixedAssetsTypeID = rcbxFixedAssetsType.SelectedValue.ToInt();
                currentEntity.Name = txtName.Text.Trim();
                currentEntity.Unit = txtUnit.Text.Trim();
                currentEntity.Quantity = txtQuantity.Value.ToInt();
                currentEntity.Specification = txtSpecification.Text.Trim();
                currentEntity.ProducingArea = txtProducingArea.Text.Trim();
                currentEntity.Manufacturer = txtManufacturer.Text.Trim();
                currentEntity.UseStatus = rcbUseStatus.SelectedValue.ToInt();
                currentEntity.DepartmentID = rcbxDepartment.SelectedValue.ToInt();
                currentEntity.UsePeople = txtUsePeople.Text.Trim();
                currentEntity.StorageLocationID = rcbStorageLocation.SelectedValue.ToInt();
                currentEntity.DepreciationType = (int)EDepreciationType.Line;
                currentEntity.OriginalValue = txtOriginalValue.Value.ToDecimal();
                currentEntity.StartUsedDate = txtStartUsedDate.SelectedDate.Value;
                currentEntity.EstimateNetSalvageValue = txtEstimateNetSalvageValue.Value.ToDecimal();
                currentEntity.EstimateUsedYear = txtEstimateUsedYear.Value.ToInt();

                currentEntity.Comment = txtRemark.Text.Trim();

                PageFixedAssetsRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntityID.HasValue
                   && this.CurrentEntityID > 0)
            {
                PageFixedAssetsRepository.DeleteByID(this.CurrentEntityID);
                PageFixedAssetsRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_DELETED_REDIRECT);
            }
        }



        protected override EPermission PagePermissionID()
        {
            return EPermission.FixedAssetsManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Edit;
        }
    }
}