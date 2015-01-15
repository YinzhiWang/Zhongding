<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ZhongDing.Web.Account.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>登录 - 众鼎医药咨询信息系统</title>
    <link href="../Content/Site.css" rel="stylesheet" />
    <link href="../Content/reset.css" rel="stylesheet" />
    <link href="../Content/text.css" rel="stylesheet" />
    <link href="../Content/core/core.css" rel="stylesheet" />
    <link href="../Content/core/login.css" rel="stylesheet" />
    <link href="../Content/icons/icons.css" rel="stylesheet" />
    <link href="../Content/mws.style.css" rel="stylesheet" />
    <style>
        /*adjust the form height for ie7*/
        body form {
            height: auto;
        }
    </style>
</head>
<body>
    <div id="mws-login">
        <h1>用户登录</h1>
        <div class="mws-login-lock">
            <img src="../Content/icons/24/locked-2.png" alt="" />
        </div>
        <div id="mws-login-form">
            <form id="loginForm" runat="server" class="mws-form">
                <div class="validate-message-wrapper">
                    <asp:ValidationSummary ID="vsLogin" runat="server" ValidationGroup="vgLogin" DisplayMode="BulletList" HeaderText="请更正以下错误:" CssClass="validation-summary-errors" />
                </div>
                <div class="mws-form-row">
                    <div class="mws-form-item large">
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="mws-login-username mws-textinput" MaxLength="200"></asp:TextBox>
                    </div>
                </div>
                <div class="mws-form-row">
                    <div class="mws-form-item large">
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="mws-login-password mws-textinput" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="mws-form-row">
                    <div class="mws-form-item large">
                        <asp:DropDownList ID="ddlCompany" CssClass="mws-login-company mws-textinput leftpadding20" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="mws-form-row">
                    <asp:Button ID="btnLogin" runat="server" Text="登  录" CssClass="mws-button green mws-login-button" ValidationGroup="vgLogin" CausesValidation="true" OnClick="btnLogin_Click" />
                </div>

                <asp:HiddenField ID="hdnErrorMsg" runat="server" />
                <asp:HiddenField ID="hdnErrorMsgPwd" runat="server" />
                <asp:HiddenField ID="hdnErrorMsgCompany" runat="server" />
            </form>
        </div>
    </div>

    <script src="../Scripts/jquery.js"></script>
    <script src="../Scripts/jquery.placeholder.min.js"></script>
    <script src="../Scripts/jquery.validate.min.js"></script>
    <script src="../Scripts/zd.base.extension.js"></script>
    <script src="../Scripts/zd.base.browserDetect.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {

            BrowserDetect.init();

            var txtUserName = $("#<%= txtUserName.ClientID%>");
            var txtPassword = $("#<%= txtPassword.ClientID%>");
            var ddlCompany = $("#<%= ddlCompany.ClientID%>");

            if (txtUserName)
                txtUserName.attr("placeholder", "用户名/邮箱");

            if (txtPassword)
                txtPassword.attr("placeholder", "密码");

            if (BrowserDetect.browser == "Explorer") {
                //IE8-则设置input宽度
                if (BrowserDetect.version < 8) {
                    txtUserName.css({ width: "245px" });
                    txtPassword.css({ width: "245px" });
                }

                if (BrowserDetect.version < 10) {
                    $('input').placeholder();
                }
            }

            var loginForm = $("#<%=loginForm.ClientID%>");

            // validate signup form on keyup and submit
            loginForm.validate({
                rules: {
                    txtUserName: "required",
                    txtPassword: "required",
                    ddlCompany: "required"
                },
                messages: {
                    txtUserName: "请输入您的用户名",
                    txtPassword: "请输入您的密码",
                    ddlCompany: "请选择账套"
                }
            });

            //debugger;
            var hdnErrorMsg = $("#<%= hdnErrorMsg.ClientID%>");
            if (!hdnErrorMsg.val().isNullOrEmpty())
                txtUserName.after("<label class=\"error\" id=\"txtUserName-error\" for=\"txtUserName\">" + hdnErrorMsg.val() + "</label>");

            var hdnErrorMsgPWD = $("#<%= hdnErrorMsgPwd.ClientID%>");
            if (!hdnErrorMsgPWD.val().isNullOrEmpty())
                txtPassword.after("<label class=\"error\" id=\"txtPassword-error\" for=\"txtPassword\">" + hdnErrorMsgPWD.val() + "</label>");

            var hdnErrorMsgCompany = $("#<%= hdnErrorMsgCompany.ClientID%>");
            if (!hdnErrorMsgCompany.val().isNullOrEmpty())
                ddlCompany.after("<label class=\"error\" id=\"ddlCompany-error\" for=\"ddlCompany\">" + hdnErrorMsgCompany.val() + "</label>");
        });

        //Disabled the back button of the browser
        window.history.forward(1);

        if (window != top)
            top.location.href = location.href;

    </script>
</body>
</html>
