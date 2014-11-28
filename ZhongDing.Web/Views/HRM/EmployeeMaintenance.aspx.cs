using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.HRM
{
    public partial class EmployeeMaintenance : BasePage
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
            this.Master.MenuItemID = (int)EMenuItem.EmployeeManage;

            rdpEnrollDate.MaxDate = DateTime.Now;

            if (!IsPostBack)
            {
                BindDepartments();

                LoadCurrentEntity();
            }
        }

        private void BindDepartments()
        {
            var departments = PageDepartmentRepository.GetDropdownItems();

            rcbxDepartment.DataSource = departments;
            rcbxDepartment.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxDepartment.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxDepartment.DataBind();
        }

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                var currentEntity = PageUsersRepository.GetByID(this.CurrentEntityID);

                if (currentEntity != null)
                {
                    hdnCurrentEntityID.Value = currentEntity.UserID.ToString();

                    txtUserName.Text = currentEntity.UserName;
                    txtFullName.Text = currentEntity.FullName;
                    txtMobilePhone.Text = currentEntity.MobilePhone;
                    txtPosition.Text = currentEntity.Position;

                    if (currentEntity.DepartmentID.HasValue && currentEntity.DepartmentID > 0)
                        rcbxDepartment.SelectedValue = currentEntity.DepartmentID.ToString();

                    rdpEnrollDate.SelectedDate = currentEntity.EnrollDate;

                    txtBasicSalary.Value = (double?)currentEntity.BasicSalary;
                    txtPositionSalary.Value = (double?)currentEntity.PositionSalary;
                    txtPhoneAllowance.Value = (double?)currentEntity.PhoneAllowance;
                    txtOfficeExpense.Value = (double?)currentEntity.OfficeExpense;
                    txtMealAllowance.Value = (double?)currentEntity.MealAllowance;
                    txtBonusPay.Value = (double?)currentEntity.BonusPay;

                    if (currentEntity.aspnet_Users != null
                        && currentEntity.aspnet_Users.aspnet_Membership != null)
                    {
                        txtEmail.Text = currentEntity.aspnet_Users.aspnet_Membership.Email;
                        cbxIsApproved.Checked = currentEntity.aspnet_Users.aspnet_Membership.IsApproved;
                        cbxIsLockedOut.Checked = currentEntity.aspnet_Users.aspnet_Membership.IsLockedOut;

                        if (currentEntity.aspnet_Users.aspnet_Membership.IsLockedOut)
                        {
                            divLockedOutUser.Visible = true;

                            cbxIsLockedOut.Enabled = true;
                        }
                        else
                            divLockedOutUser.Visible = false;
                    }
                    else
                        divLockedOutUser.Visible = false;
                }
                else
                {
                    btnDelete.Visible = false;
                    divLockedOutUser.Visible = false;
                }
            }
            else
            {
                btnDelete.Visible = false;
                divLockedOutUser.Visible = false;
            }
        }

        protected void cvUserName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (PageUsersRepository.GetList(x => x.IsDeleted == false && x.UserID != this.CurrentEntityID
                && x.UserName.ToLower() == txtUserName.Text.Trim().ToLower()).Count() > 0)
            {
                args.IsValid = false;
            }
        }

        protected void cvDepartment_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!string.IsNullOrEmpty(rcbxDepartment.Text))
            {
                UISearchDropdownItem uiSearchObj = new UISearchDropdownItem()
                {
                    ItemText = rcbxDepartment.Text.Trim()
                };

                var departments = PageDepartmentRepository.GetDropdownItems(uiSearchObj);

                if (departments.Count <= 0)
                    args.IsValid = false;
            }
        }

        protected void cvMobilePhone_ServerValidate(object source, ServerValidateEventArgs args)
        {
            var iExistUserCount = PageUsersRepository.GetList(x => x.UserID != this.CurrentEntityID
                && x.MobilePhone.Equals(txtMobilePhone.Text.Trim().ToLower())).Count();

            if (iExistUserCount > 0)
                args.IsValid = false;
        }

        protected void cvEmail_ServerValidate(object source, ServerValidateEventArgs args)
        {
            var iExistUserCount = PageUsersRepository.GetList(x => x.UserID != this.CurrentEntityID
                && x.aspnet_Users != null && x.aspnet_Users.aspnet_Membership != null
                && x.aspnet_Users.aspnet_Membership.LoweredEmail.Equals(txtEmail.Text.Trim().ToLower())).Count();

            if (iExistUserCount > 0)
                args.IsValid = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            #region 验证密码

            if (!this.CurrentEntityID.HasValue
                || this.CurrentEntityID < 0)
            {
                if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
                {
                    cvPassword.IsValid = false;
                    cvPassword.ErrorMessage = "密码必填";
                }

                if (string.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
                {
                    cvConfirmPassword.IsValid = false;
                    cvConfirmPassword.ErrorMessage = "确认密码必填";
                }
            }

            if (!string.IsNullOrEmpty(txtPassword.Text.Trim())
                && string.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
            {
                cvConfirmPassword.IsValid = false;
                cvConfirmPassword.ErrorMessage = "确认密码必填";
            }
            else if (string.IsNullOrEmpty(txtPassword.Text.Trim())
                && !string.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
            {
                cvPassword.IsValid = false;
                cvPassword.ErrorMessage = "密码必填";
            }
            else if (!string.IsNullOrEmpty(txtPassword.Text.Trim())
                && !string.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
            {
                if (txtPassword.Text.Trim().Length < 6
                    || txtConfirmPassword.Text.Trim().Length < 6)
                {
                    cvPassword.IsValid = false;
                    cvPassword.ErrorMessage = "密码的最小长度是6位，请重新输入";
                }
                else
                {
                    if (txtPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
                    {
                        cvConfirmPassword.IsValid = false;
                        cvConfirmPassword.ErrorMessage = "确认密码与密码不一致，请重新输入";
                    }
                }
            }

            #endregion

            if (!IsValid) return;

            string userName = txtUserName.Text.Trim();

            Users user = null;

            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
            {
                user = PageUsersRepository.GetByID(this.CurrentEntityID);

                if (user != null)
                {
                    user.LastModifiedBy = CurrentUser.UserID;
                    user.LastModifiedOn = DateTime.Now;

                    MembershipUser mUser = Membership.GetUser(userName);

                    //如果用户被锁定并且未被选中，则解锁用户
                    if (cbxIsLockedOut.Enabled && cbxIsLockedOut.Checked == false)
                    {
                        mUser.UnlockUser();
                    }

                    //改变了激活状态时才需要保存当前激活状态
                    if (user.aspnet_Users != null
                        && user.aspnet_Users.aspnet_Membership != null)
                    {
                        if (cbxIsApproved.Checked != user.aspnet_Users.aspnet_Membership.IsApproved)
                        {
                            mUser.IsApproved = cbxIsApproved.Checked;

                            Membership.UpdateUser(mUser);
                        }
                    }

                    //修改密码
                    string newPassword = txtPassword.Text.Trim();

                    if (!string.IsNullOrEmpty(newPassword))
                    {
                        string autoPassword = mUser.ResetPassword();

                        mUser.ChangePassword(autoPassword, newPassword);

                        //if (!string.IsNullOrEmpty(mUser.Email)
                        //    && WebConfig.EmailEnabled)
                        //{
                        //    SendChangePasswordEmail(user, mUser.Email, newPassword);
                        //}
                    }
                }
            }
            else
            {
                MembershipCreateStatus createStatus;

                MembershipUser mUser = Membership.CreateUser(userName, txtConfirmPassword.Text.Trim(),
                    txtEmail.Text.Trim(), "What's your real name?", "My name is tester.", cbxIsApproved.Checked, out createStatus);

                if (createStatus == MembershipCreateStatus.Success
                    && mUser != null)
                {
                    user = new Users();
                    user.AspnetUserID = (Guid)mUser.ProviderUserKey;
                    user.UserName = mUser.UserName;

                    PageUsersRepository.Add(user);
                }
                else
                {
                    if (createStatus == MembershipCreateStatus.InvalidPassword)
                    {
                        cvPassword.IsValid = false;
                        cvPassword.ErrorMessage = "无效的密码，请重新输入";
                    }

                    return;
                }
            }

            if (user != null)
            {
                //修改用户名
                if (txtUserName.Text.Trim().ToLower() != user.UserName.ToLower())
                {
                    user.UserName = txtUserName.Text.Trim();

                    if (user.aspnet_Users != null)
                    {
                        user.aspnet_Users.UserName = user.UserName;
                        user.aspnet_Users.LoweredUserName = user.UserName.ToLower();
                    }
                }

                //修改邮箱
                if (user.aspnet_Users != null && user.aspnet_Users.aspnet_Membership != null &&
                    txtEmail.Text.Trim().ToLower() != user.aspnet_Users.aspnet_Membership.LoweredEmail)
                {
                    user.aspnet_Users.aspnet_Membership.Email = txtEmail.Text.Trim();
                    user.aspnet_Users.aspnet_Membership.LoweredEmail = txtEmail.Text.Trim().ToLower();
                }

                user.MobilePhone = txtMobilePhone.Text.Trim();
                user.FullName = txtFullName.Text.Trim();

                if (!string.IsNullOrEmpty(rcbxDepartment.SelectedValue))
                    user.DepartmentID = int.Parse(rcbxDepartment.SelectedValue);

                user.Position = txtPosition.Text.Trim();
                user.EnrollDate = rdpEnrollDate.SelectedDate;

                user.BasicSalary = (decimal?)txtBasicSalary.Value;
                user.PositionSalary = (decimal?)txtPositionSalary.Value;
                user.PhoneAllowance = (decimal?)txtPhoneAllowance.Value;
                user.OfficeExpense = (decimal?)txtOfficeExpense.Value;
                user.MealAllowance = (decimal?)txtMealAllowance.Value;
                user.BonusPay = (decimal?)txtBonusPay.Value;
            }

            PageUsersRepository.Save();

            hdnCurrentEntityID.Value = user.UserID.ToString();

            //if (this.CurrentEntityID.HasValue
            //    && this.CurrentEntityID > 0)
            //{
            this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
            //}
            //else
            //{
            //    this.Master.BaseNotification.OnClientHidden = "refreshMaintenancePage";
            //    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REFRESH);
            //}

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IUsersRepository userRepository = new UsersRepository();
                    IDeptMarketDivisionRepository deptMarketDivisionRepository = new DeptMarketDivisionRepository();

                    userRepository.SetDbModel(db);
                    deptMarketDivisionRepository.SetDbModel(db);

                    var user = userRepository.GetByID(this.CurrentEntityID);

                    if (user != null)
                    {
                        foreach (var item in user.DeptMarketDivision)
                        {
                            deptMarketDivisionRepository.Delete(item);
                        }

                        if (user.aspnet_Users != null && user.aspnet_Users.aspnet_Membership != null)
                            user.aspnet_Users.aspnet_Membership.IsApproved = false;

                        userRepository.Delete(user);
                    }

                    unitOfWork.SaveChanges();
                }
            }
        }

        private bool SendChangePasswordEmail(Users user, string email, string newPassword)
        {
            bool isSuccess = false;

            StringBuilder sbEmailBody = new StringBuilder();
            sbEmailBody.AppendLine("尊敬的用户 " + user.FullName + "， 您好:");
            sbEmailBody.AppendLine("<br />");
            sbEmailBody.AppendLine("<br />");
            sbEmailBody.AppendLine("您在&nbsp;&nbsp;<a href=\"" + WebConfig.WebsiteRootUrl + "\" target=\"_blank\">众鼎医药咨询信息系统</a>&nbsp;&nbsp;的用户登录密码已经被修改为:" + newPassword);
            sbEmailBody.AppendLine("<br />");
            sbEmailBody.AppendLine("<br />");
            sbEmailBody.AppendLine("请使用新密码登录系统。");

            isSuccess = EmailService.SendEmail(email, "密码修改", sbEmailBody.ToString(), true);

            return isSuccess;
        }


    }
}