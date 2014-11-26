using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.HRM.Editors
{
    public partial class DeptProductEvaluationMaintain : BasePage
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

        #endregion

        #region Members

        private IProductRepository _PageProductRepository;
        private IProductRepository PageProductRepository
        {
            get
            {
                if (_PageProductRepository == null)
                    _PageProductRepository = new ProductRepository();

                return _PageProductRepository;
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!this.OwnerEntityID.HasValue
                || this.OwnerEntityID <= 0))
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR);

                return;
            }

            if (!IsPostBack)
            {
                hdnGridClientID.Value = GridClientID;

                BindProducts();

                LoadCurrentEntity();
            }
        }

        private void BindProducts()
        {
            var products = PageProductRepository.GetDropdownItems();

            rcbxProducts.DataSource = products;
            rcbxProducts.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxProducts.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxProducts.DataBind();
        }

        private void LoadCurrentEntity()
        {
            if (this.OwnerEntityID.HasValue
                && this.OwnerEntityID > 0)
            {
                var department = PageDepartmentRepository.GetByID(this.OwnerEntityID);

                if (department != null)
                {
                    var deptProductEvaluation = department.DeptProductEvaluation
                        .Where(x => x.IsDeleted == false && x.ID == this.CurrentEntityID).FirstOrDefault();

                    if (deptProductEvaluation != null)
                    {
                        rcbxProducts.SelectedValue = deptProductEvaluation.ProductID.ToString();
                        txtInvestigateRatio.DbValue = deptProductEvaluation.InvestigateRatio;
                        txtSalesRatio.DbValue = deptProductEvaluation.SalesRatio;
                    }
                }
            }
        }

        protected void cvProducts_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!string.IsNullOrEmpty(rcbxProducts.Text.Trim()))
            {
                UISearchDropdownItem uiSearchObj = new UISearchDropdownItem()
                {
                    ItemText = rcbxProducts.Text.Trim()
                };

                var products = PageProductRepository.GetDropdownItems(uiSearchObj);

                if (products.Count <= 0)
                    args.IsValid = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            DeptProductEvaluation deptProductEvaluation = null;

            if (this.OwnerEntityID.HasValue
                && this.OwnerEntityID > 0)
            {
                var department = PageDepartmentRepository.GetByID(this.OwnerEntityID);

                if (department != null)
                {
                    deptProductEvaluation = department.DeptProductEvaluation
                        .Where(x => x.IsDeleted == false && x.ID == this.CurrentEntityID).FirstOrDefault();

                    if (deptProductEvaluation == null)
                    {
                        deptProductEvaluation = new DeptProductEvaluation();

                        department.DeptProductEvaluation.Add(deptProductEvaluation);
                    }

                    deptProductEvaluation.ProductID = int.Parse(rcbxProducts.SelectedValue);
                    deptProductEvaluation.InvestigateRatio = Convert.ToDouble(txtInvestigateRatio.DbValue);
                    deptProductEvaluation.SalesRatio = Convert.ToDouble(txtSalesRatio.DbValue);

                    department.LastModifiedOn = DateTime.Now;
                    department.LastModifiedBy = CurrentUser.UserID;

                    PageDepartmentRepository.Save();

                    this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_CLOSE_WIN);
                }

            }
        }
    }
}