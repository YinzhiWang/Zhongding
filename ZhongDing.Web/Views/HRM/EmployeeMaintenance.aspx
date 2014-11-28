<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmployeeMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.HRM.EmployeeMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">
        <div class="mws-panel grid_full">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">员工维护</span>
            </div>
            <div class="mws-panel-body">
                <div class="mws-form">
                    <div class="mws-form-inline">
                        <div class="mws-form-row">
                            <div class="validate-message-wrapper">
                                <asp:ValidationSummary ID="vsMaintenance" runat="server" ValidationGroup="vgMaintenance" DisplayMode="BulletList" HeaderText="请更正以下错误:" CssClass="validation-summary-errors" />
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width50-percent">
                                <label>登录名</label>
                                <div class="mws-form-item">
                                    <telerik:RadTextBox runat="server" ID="txtUserName" CssClass="mws-textinput" Width="50%"></telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtUserName"
                                        ErrorMessage="登录名必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvUserName" runat="server" Display="Dynamic" ErrorMessage="登录名不能重复，请重新输入"
                                        ControlToValidate="txtUserName" ValidationGroup="vgMaintenance" OnServerValidate="cvUserName_ServerValidate" Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                </div>
                            </div>
                            <div class="float-left width50-percent">
                                <label>邮箱</label>
                                <div class="mws-form-item">
                                    <telerik:RadTextBox runat="server" ID="txtEmail" CssClass="mws-textinput" Width="50%"></telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ValidationGroup="vgMaintenance"
                                        ErrorMessage="邮箱必填" ControlToValidate="txtEmail" Text="*" CssClass="field-validation-error"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                        ErrorMessage="邮箱格式不正确！" ValidationExpression="^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$"
                                        CssClass="field-validation-error" Display="Dynamic" ValidationGroup="vgMaintenance" Text="*"></asp:RegularExpressionValidator>
                                    <asp:CustomValidator ID="cvEmail" runat="server" ErrorMessage="邮箱已存在，请更换" ValidationGroup="vgMaintenance"
                                        ControlToValidate="txtEmail" Text="*" CssClass="field-validation-error" OnServerValidate="cvEmail_ServerValidate"></asp:CustomValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width50-percent">
                                <label>姓名</label>
                                <div class="mws-form-item">
                                    <telerik:RadTextBox runat="server" ID="txtFullName" CssClass="mws-textinput" Width="50%"></telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtFullName"
                                        ErrorMessage="姓名必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="float-left width50-percent">
                                <label>手机号</label>
                                <div class="mws-form-item">
                                    <telerik:RadTextBox runat="server" ID="txtMobilePhone" CssClass="mws-textinput" Width="50%"></telerik:RadTextBox>
                                    <asp:RegularExpressionValidator ID="revMobilePhone" runat="server"
                                        ControlToValidate="txtMobilePhone"
                                        ErrorMessage="手机格式不正确！"
                                        ValidationExpression="^(13[0-9]|15[0-9]|18[0-9]|14[5|7])\d{8}$"
                                        CssClass="field-validation-error" Display="Dynamic"
                                        ValidationGroup="vgMaintenance" Text="*"></asp:RegularExpressionValidator>
                                    <asp:CustomValidator ID="cvMobilePhone" runat="server" ErrorMessage="该手机号已存在，请更换" ValidationGroup="vgMaintenance"
                                        ControlToValidate="txtMobilePhone" Text="*" CssClass="field-validation-error" OnServerValidate="cvMobilePhone_ServerValidate"></asp:CustomValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width50-percent">
                                <label>密码</label>
                                <div class="mws-form-item">
                                    <telerik:RadTextBox runat="server" ID="txtPassword" CssClass="mws-textinput" TextMode="Password" Width="50%"></telerik:RadTextBox>
                                    <asp:CustomValidator ID="cvPassword" runat="server" Display="Dynamic"
                                        ControlToValidate="txtPassword" ValidationGroup="vgMaintenance" Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                </div>
                            </div>
                            <div class="float-left width50-percent">
                                <label>确认密码</label>
                                <div class="mws-form-item">
                                    <telerik:RadTextBox runat="server" ID="txtConfirmPassword" CssClass="mws-textinput" TextMode="Password" Width="50%"></telerik:RadTextBox>
                                    <asp:CustomValidator ID="cvConfirmPassword" runat="server" Display="Dynamic"
                                        ControlToValidate="txtConfirmPassword" ValidationGroup="vgMaintenance" Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width50-percent">
                                <label>所属部门</label>
                                <div class="mws-form-item">
                                    <telerik:RadComboBox runat="server" ID="rcbxDepartment" Filter="Contains" AllowCustomText="false"
                                        MarkFirstMatch="true" Height="160px" EmptyMessage="--请选择--">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="rfvDepartment" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="rcbxDepartment"
                                        ErrorMessage="请选择所属部门" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvDepartment" runat="server" ErrorMessage="该部门不存在，请重新选择"
                                        ControlToValidate="rcbxDepartment" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error" OnServerValidate="cvDepartment_ServerValidate">
                                    </asp:CustomValidator>
                                </div>
                            </div>
                            <div class="float-left width50-percent">
                                <label>职位</label>
                                <div class="mws-form-item">
                                    <telerik:RadTextBox runat="server" ID="txtPosition" CssClass="mws-textinput" Width="50%"></telerik:RadTextBox>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>入职日期</label>
                            <div class="mws-form-item small">
                                <telerik:RadDatePicker ID="rdpEnrollDate" runat="server"
                                    Calendar-EnableShadows="true" Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                    Calendar-FirstDayOfWeek="Monday">
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="rfvEnrollDate" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="rdpEnrollDate"
                                    ErrorMessage="入职日期必填" Text="*" CssClass="field-validation-error">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width50-percent">
                                <label>账号激活</label>
                                <div class="mws-form-item">
                                    <telerik:RadButton runat="server" ID="cbxIsApproved" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false">
                                    </telerik:RadButton>
                                </div>
                            </div>
                            <div class="float-left width50-percent" runat="server" id="divLockedOutUser">
                                <label>账号锁定</label>
                                <div class="mws-form-item">
                                    <telerik:RadButton runat="server" ID="cbxIsLockedOut" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false" Enabled="false">
                                    </telerik:RadButton>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="mws-panel grid_full">
                                <div class="mws-panel-header">
                                    <span>工资设置</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <div class="mws-panel-body">
                                            <table class="mws-table" style="text-align: center">
                                                <thead>
                                                    <tr>
                                                        <th>基本工资</th>
                                                        <th>岗位工资</th>
                                                        <th>话费补贴</th>
                                                        <th>办公费用</th>
                                                        <th>餐费补助</th>
                                                        <th>绩效工资</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr class="gradeX">
                                                        <td>
                                                            <telerik:RadNumericTextBox runat="server" ID="txtBasicSalary" CssClass="mws-textinput" Width="100"
                                                                Type="Currency" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="9">
                                                            </telerik:RadNumericTextBox>
                                                        </td>
                                                        <td>
                                                            <telerik:RadNumericTextBox runat="server" ID="txtPositionSalary" CssClass="mws-textinput" Width="100"
                                                                Type="Currency" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="9">
                                                            </telerik:RadNumericTextBox>
                                                        </td>
                                                        <td>
                                                            <telerik:RadNumericTextBox runat="server" ID="txtPhoneAllowance" CssClass="mws-textinput" Width="100"
                                                                Type="Currency" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="9">
                                                            </telerik:RadNumericTextBox>
                                                        </td>
                                                        <td>
                                                            <telerik:RadNumericTextBox runat="server" ID="txtOfficeExpense" CssClass="mws-textinput" Width="100"
                                                                Type="Currency" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="9">
                                                            </telerik:RadNumericTextBox>
                                                        </td>
                                                        <td>
                                                            <telerik:RadNumericTextBox runat="server" ID="txtMealAllowance" CssClass="mws-textinput" Width="100"
                                                                Type="Currency" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="9">
                                                            </telerik:RadNumericTextBox>
                                                        </td>
                                                        <td>
                                                            <telerik:RadNumericTextBox runat="server" ID="txtBonusPay" CssClass="mws-textinput" Width="100"
                                                                Type="Currency" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="9">
                                                            </telerik:RadNumericTextBox>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="mws-button-row">
                            <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                            <asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="mws-button orange" CausesValidation="false" OnClick="btnDelete_Click" OnClientClick="return onConfirmDelete();" />
                            <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/HRM/EmployeeManagement.aspx');return false;" />
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnCurrentEntityID" runat="server" Value="-1" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">

        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/HRM/EmployeeManagement.aspx");
        }

        function refreshMaintenancePage(sender, args) {

            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            redirectToPage("Views/HRM/EmployeeMaintenance.aspx?EntityID=" + currentEntityID);
        }

    </script>
</asp:Content>
