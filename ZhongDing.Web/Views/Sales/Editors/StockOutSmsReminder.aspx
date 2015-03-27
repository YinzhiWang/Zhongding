<%@ Page Title="出库短信提醒" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="StockOutSmsReminder.aspx.cs" Inherits="ZhongDing.Web.Views.Sales.Editors.StockOutSmsReminder" %>

<%@ MasterType VirtualPath="~/Site.Window.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="mws-panel grid_full" style="margin-bottom: 10px;">

        <div class="mws-panel-body">
            <div class="mws-form">
                <div class="mws-form-inline">
                    <div class="mws-form-row">
                        <div class="validate-message-wrapper">
                            <asp:ValidationSummary ID="vsMaintenance" runat="server" ValidationGroup="vgMaintenance" DisplayMode="BulletList" HeaderText="请更正以下错误:" CssClass="validation-summary-errors" />
                        </div>
                    </div>
                    <div class="mws-form-row" style="padding-top: 0px; padding-left: 0px; padding-right: 1px;">
                        <table style="margin:10px;">
                             <tr>
                                <td>接收号码</td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadTextBox runat="server" ID="txtMobileNumber" CssClass="mws-textinput" Width="200px"  MaxLength="11"></telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="rfvMobileNumber" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtMobileNumber"
                                        ErrorMessage="接收号码必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                      <asp:RegularExpressionValidator ID="revMobileNumber" runat="server"
                                        ControlToValidate="txtMobileNumber"
                                        ErrorMessage="手机格式不正确"
                                        ValidationExpression="^[1][3-8]\d{9}$"
                                        CssClass="field-validation-error" Display="Dynamic"
                                        ValidationGroup="vgMaintenance" Text="*"></asp:RegularExpressionValidator>
                                    <telerik:RadToolTip ID="rttMobileNumber" runat="server" TargetControlID="txtMobileNumber" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                </td>
                            </tr>
                            <tr>
                                <td>提醒内容</td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadTextBox runat="server" ID="txtContent" CssClass="mws-textinput" Width="500px" TextMode="MultiLine" Height="100px" MaxLength="72"></telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="rfvContent" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtContent"
                                        ErrorMessage="提醒内容必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                    <telerik:RadToolTip ID="rttContent" runat="server" TargetControlID="txtContent" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="mws-button-row">
                        <asp:Button ID="btnSave" runat="server" Text="发送" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="closeWindow(false);return false;" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnGridClientID" runat="server" />

    <telerik:RadNotification ID="radNotification" runat="server" EnableRoundedCorners="true"
        AutoCloseDelay="1000" Skin="Silk" Animation="Fade" EnableShadow="true" Title="提示"
        TitleIcon="none" Opacity="95" Position="Center" ContentIcon="~/Content/icons/32/cross.png"
        Width="300" Height="100">
    </telerik:RadNotification>
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

