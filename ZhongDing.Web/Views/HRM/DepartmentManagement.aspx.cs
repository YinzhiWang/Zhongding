using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.HRM
{
    public partial class DepartmentManagement : BasePage
    {
        #region Members

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
            this.Master.MenuItemID = (int)EMenuItem.DepartmentManage;
        }

        #region Private Methods

        private void BindEntities(bool isNeedRebind)
        {
            UISearchDepartment uiSearchObj = new UISearchDepartment()
            {
                DepartmentName = txtEntityName.Text.Trim()
            };

            int totalRecords;

            var entities = PageDepartmentRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

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

        protected void rgEntities_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    DbModelContainer db = unitOfWork.GetDbModel();

                    IDepartmentRepository departmentRepository = new DepartmentRepository();
                    IDeptMarketDivisionRepository deptMarketDivisionRepository = new DeptMarketDivisionRepository();
                    IDeptMarketProductRepository deptMarketProductRepository = new DeptMarketProductRepository();
                    IDeptProductEvaluationRepository deptProductEvaluationRepository = new DeptProductEvaluationRepository();

                    departmentRepository.SetDbModel(db);
                    deptMarketDivisionRepository.SetDbModel(db);
                    deptMarketProductRepository.SetDbModel(db);
                    deptProductEvaluationRepository.SetDbModel(db);

                    Department currentEntity = PageDepartmentRepository.GetByID(id);

                    if (currentEntity != null)
                    {
                        foreach (var item in currentEntity.DeptProductEvaluation)
                        {
                            deptProductEvaluationRepository.Delete(item);
                        }

                        foreach (var deptUser in currentEntity.Users1)
                        {
                            foreach (var deptMarketDivision in deptUser.DeptMarketDivision)
                            {
                                foreach (var deptMarketProduct in deptMarketDivision.DeptMarketProduct)
                                {
                                    deptMarketProductRepository.Delete(deptMarketProduct);
                                }

                                deptMarketDivisionRepository.Delete(deptMarketDivision);
                            }
                        }

                        departmentRepository.Delete(currentEntity);

                        unitOfWork.SaveChanges();
                    }
                }
            }

            rgEntities.Rebind();
        }

        protected void rgEntities_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgEntities_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgEntities_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindEntities(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtEntityName.Text = string.Empty;

            BindEntities(true);
        }
    }
}