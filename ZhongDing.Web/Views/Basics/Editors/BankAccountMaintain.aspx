<%@ Page Title="银行帐号维护" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="BankAccountMaintain.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.Editors.BankAccountMaintain" %>

<%@ MasterType VirtualPath="~/Site.Window.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="mws-panel grid_full" style="margin-bottom:10px;">

        <div class="mws-panel-body">
            <div class="mws-form">
                <div class="mws-form-inline">
                    <div class="mws-form-row">
                        <div class="validate-message-wrapper">
                            <asp:ValidationSummary ID="vsMaintenance" runat="server" ValidationGroup="vgMaintenance" DisplayMode="BulletList" HeaderText="请更正以下错误:" CssClass="validation-summary-errors" />
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <label>户名</label>
                        <div class="mws-form-item small">
                            <telerik:RadTextBox runat="server" ID="txtAccountName" CssClass="mws-textinput" Width="40%" MaxLength="100"></telerik:RadTextBox>
                            <telerik:RadToolTip ID="rttAccountName" runat="server" TargetControlID="txtAccountName" ShowEvent="OnClick"
                                Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                            </telerik:RadToolTip>
                            <asp:RequiredFieldValidator ID="rfvAccountName" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtAccountName"
                                ErrorMessage="户名必填" Text="*" CssClass="field-validation-error">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <label>开户行</label>
                        <div class="mws-form-item small">
                            <telerik:RadTextBox runat="server" ID="txtBankBranchName" CssClass="mws-textinput" Width="40%" MaxLength="200"></telerik:RadTextBox>
                            <telerik:RadToolTip ID="rttBankBranchName" runat="server" TargetControlID="txtBankBranchName" ShowEvent="OnClick"
                                Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                            </telerik:RadToolTip>
                            <asp:RequiredFieldValidator ID="rfvBankBranchName" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtBankBranchName"
                                ErrorMessage="开户行必填" Text="*" CssClass="field-validation-error">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <label>账号</label>
                        <div class="mws-form-item small">
                            <telerik:RadTextBox runat="server" ID="txtAccount" ShowButton="false" CssClass="mws-textinput" Width="40%" MaxLength="24">
                            </telerik:RadTextBox>
                            <telerik:RadToolTip ID="rttAccount" runat="server" TargetControlID="txtAccount" ShowEvent="OnClick"
                                Position="MiddleRight" RelativeTo="Element" AutoCloseDelay="0">
                                <div>
                                    <p>帐号为16-19位数字，或如下格式:</p>
                                    <ol style="margin-left: 20px">
                                        <li>0000 0000 0000 [4-7位数字]</li>
                                        <li>0000-0000-0000-[4-7位数字]</li>
                                        <li>0000 0000 0000 0000 000</li>
                                        <li>0000-0000-0000-0000-000</li>
                                    </ol>
                                </div>
                            </telerik:RadToolTip>
                            <asp:RequiredFieldValidator ID="rfvAccount" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtAccount"
                                ErrorMessage="帐号必填" Text="*" CssClass="field-validation-error">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revAccount" runat="server" ValidationGroup="vgMaintenance"
                                ControlToValidate="txtAccount" ErrorMessage="帐号格式不正确，请重新输入" CssClass="field-validation-error"
                                ValidationExpression="^\d{16,19}$|^\d{6}[- ]\d{10,13}$|^\d{4}[- ]\d{4}[- ]\d{4}[- ]\d{4,7}$|^\d{4}[- ]\d{4}[- ]\d{4}[- ]\d{4}[- ]\d{3}$" Text="*"></asp:RegularExpressionValidator>
                            <asp:CustomValidator ID="cvAccount" runat="server" ControlToValidate="txtAccount" ValidationGroup="vgMaintenance" OnServerValidate="cvAccount_ServerValidate"
                                Text="*" CssClass="field-validation-error" ErrorMessage="帐号无效，请重新输入"></asp:CustomValidator>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <label>备注</label>
                        <div class="mws-form-item medium">
                            <telerik:RadTextBox runat="server" ID="txtComment" Width="90%" MaxLength="1000"
                                TextMode="MultiLine" Height="80">
                            </telerik:RadTextBox>
                        </div>
                    </div>
                    <div class="height20"></div>
                </div>
                <div class="mws-button-row">
                    <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="closeWindow(false);return false;" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnGridClientID" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">
        function closeWindow(needRebindGrid) {

            var oWin = $.getRadWindow();

            if (oWin) {

                if (needRebindGrid) {

                    var browserWindow = oWin.get_browserWindow();

                    var gridClientID = $("#<%= hdnGridClientID.ClientID%>").val();

                    if (!gridClientID.isNullOrEmpty()) {
                        var refreshGrid = browserWindow.$find(gridClientID);

                        if (refreshGrid) {
                            refreshGrid.get_masterTableView().rebind();
                        }
                    }
                }

                var isDestroyOnClose = oWin.get_destroyOnClose();
                if (isDestroyOnClose) {
                    oWin.set_destroyOnClose(false);
                }

                if (!oWin.isClosed()) {
                    oWin.close();
                }
            }
        }

        function onClientHidden(sender, args) {
            closeWindow(true);
        }

        function onError(sender, args) {
            closeWindow(false);
        }

    </script>
</asp:Content>
