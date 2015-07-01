<%@ Page Title="担保收款结算" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="GuaranteeReceiptSettle.aspx.cs" Inherits="ZhongDing.Web.Views.Sales.Editors.GuaranteeReceiptSettle" %>

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

                        <label>收款日期</label>
                        <div class="mws-form-item small">
                            <telerik:RadDatePicker ID="rdpBorrowDate" Width="160px" runat="server">
                            </telerik:RadDatePicker>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="rdpBorrowDate"
                                ErrorMessage="收款日期必填" Text="*" CssClass="field-validation-error">
                            </asp:RequiredFieldValidator>
                            <telerik:RadToolTip ID="RadToolTip2" runat="server" TargetControlID="rdpBorrowDate" ShowEvent="OnMouseOver"
                                Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                            </telerik:RadToolTip>
                        </div>



                    </div>
                    <div class="mws-form-row">

                        <label>收款金额</label>
                        <div class="mws-form-item small">
                            <telerik:RadNumericTextBox Enabled="false" ShowSpinButtons="true" MinValue="0" IncrementSettings-InterceptArrowKeys="true" IncrementSettings-InterceptMouseWheel="true"
                                Label="" runat="server" ID="txtReceiptAmount" Width="160px">
                            </telerik:RadNumericTextBox>
                            <asp:RequiredFieldValidator ID="rfvReceiptAmount" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtReceiptAmount"
                                ErrorMessage="收款金额必填" Text="*" CssClass="field-validation-error">
                            </asp:RequiredFieldValidator>
                        </div>



                    </div>
                    <div class="mws-form-row">

                        <label>收款账户</label>
                        <div class="mws-form-item small">
                            <telerik:RadComboBox runat="server" ID="rcbxFromAccount" Filter="Contains" AllowCustomText="false" NoWrap="true"
                                MarkFirstMatch="true" Height="160px" Width="400px" EmptyMessage="--请选择--">
                            </telerik:RadComboBox>
                            <telerik:RadToolTip ID="rttBankBranchName" runat="server" TargetControlID="rcbxFromAccount" ShowEvent="OnClick"
                                Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                            </telerik:RadToolTip>
                            <asp:RequiredFieldValidator ID="rfvFromAccount" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="rcbxFromAccount"
                                ErrorMessage="收款账户必填" Text="*" CssClass="field-validation-error">
                            </asp:RequiredFieldValidator>
                        </div>


                    </div>

                    <div class="mws-button-row">
                        <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
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

