<%@ Page Title="联系人维护" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="SupplierContactMaintain.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.Editors.SupplierContactMaintain" %>

<%@ MasterType VirtualPath="~/Site.Window.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="mws-panel grid_full" style="margin-bottom: 10px;">

        <div class="mws-panel-body">
            <div class="mws-form">
                <div class="mws-form-inline">
                    <div class="mws-form-row">
                        <div class="validate-message-wrapper">
                            <asp:ValidationSummary ID="vsMaintenance" runat="server" ValidationGroup="vgMaintenance" DisplayMode="BulletList" HeaderText="请更正以下错误:" CssClass="validation-summary-errors" />
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <label>联系人</label>
                        <div class="mws-form-item small">
                            <telerik:RadTextBox runat="server" ID="txtContactName" CssClass="mws-textinput" Width="40%" MaxLength="100"></telerik:RadTextBox>
                            <telerik:RadToolTip ID="rttContactName" runat="server" TargetControlID="txtContactName" ShowEvent="OnClick"
                                Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                            </telerik:RadToolTip>
                            <asp:RequiredFieldValidator ID="rfvAccountName" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtContactName"
                                ErrorMessage="联系人必填" Text="*" CssClass="field-validation-error">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <label>联系电话</label>
                        <div class="mws-form-item">
                            <telerik:RadTextBox runat="server" ID="txtPhoneNumber" CssClass="mws-textinput" MaxLength="20"></telerik:RadTextBox>
                            <telerik:RadToolTip ID="rttPhoneNumber" runat="server" TargetControlID="txtPhoneNumber" ShowEvent="OnClick"
                                Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                            </telerik:RadToolTip>
                            <asp:RequiredFieldValidator ID="rfvPhoneNumber"
                                runat="server"
                                ErrorMessage="联系电话必填"
                                ControlToValidate="txtPhoneNumber"
                                Display="Dynamic" CssClass="field-validation-error"
                                ValidationGroup="vgMaintenance" Text="*">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revPhoneNumber" runat="server"
                                ControlToValidate="txtPhoneNumber"
                                ErrorMessage="联系电话格式不正确！"
                                ValidationExpression="(\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$"
                                CssClass="field-validation-error" Display="Dynamic"
                                ValidationGroup="vgMaintenance" Text="*"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <label>联系地址</label>
                        <div class="mws-form-item medium">
                            <telerik:RadTextBox runat="server" ID="txtAddress" CssClass="mws-textinput" Width="100%" MaxLength="255"></telerik:RadTextBox>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <label>备注</label>
                        <div class="mws-form-item medium">
                            <telerik:RadTextBox runat="server" ID="txtComment" Width="90%" MaxLength="255"
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
