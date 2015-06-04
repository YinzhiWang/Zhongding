using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Common.Enums;

namespace ZhongDing.Web.Views.Basics.Editors
{
    public partial class DeptProductAnnualSalesMaintain : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }




        protected override EPermission PagePermissionID()
        {
            return EPermission.DepartmentManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Edit;
        }
    }
}