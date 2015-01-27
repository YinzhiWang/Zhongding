<%@ Page Title="收款维护" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="ClientOrderPaymentMaintain.aspx.cs" Inherits="ZhongDing.Web.Views.Sales.Editors.ClientOrderPaymentMaintain" %>

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
                        <label>收款方式</label>
                        <div class="mws-form-item small">
                            <telerik:RadButton runat="server" ID="radioIsBankTransfer" ButtonType="ToggleButton" ToggleType="Radio" AutoPostBack="false"
                                GroupName="PaymentMethod" Text="银行转账" Value="true" Checked="true" CommandArgument="IsBankTransfer" OnClientCheckedChanged="onPaymentMethodCheckedChanged">
                            </telerik:RadButton>
                            &nbsp;&nbsp;
                            <telerik:RadButton runat="server" ID="radioIsDeduction" ButtonType="ToggleButton" ToggleType="Radio" AutoPostBack="false"
                                GroupName="PaymentMethod" Text="抵扣" Value="false" CommandArgument="IsDeduction" OnClientCheckedChanged="onPaymentMethodCheckedChanged">
                            </telerik:RadButton>
                        </div>
                    </div>

                    <!--银行转账-->
                    <div id="divBankTransfer">
                        <div class="mws-form-row">
                            <label>客户账号</label>
                            <div class="mws-form-item small">
                                <telerik:RadComboBox runat="server" ID="rcbxFromAccount" Filter="Contains"
                                    AllowCustomText="false" Height="160px" Width="60%" EmptyMessage="--请选择--">
                                </telerik:RadComboBox>
                                <asp:CustomValidator ID="cvFromAccount" runat="server" ErrorMessage="请选择客户账号"
                                    ControlToValidate="rcbxFromAccount" ValidationGroup="vgMaintenance" Display="Dynamic"
                                    Text="*" CssClass="field-validation-error">
                                </asp:CustomValidator>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>收款账号</label>
                            <div class="mws-form-item small">
                                <telerik:RadComboBox runat="server" ID="rcbxToAccount" Filter="Contains"
                                    AllowCustomText="false" Height="160px" Width="60%" EmptyMessage="--请选择--">
                                </telerik:RadComboBox>
                                <asp:CustomValidator ID="cvToAccount" runat="server" ErrorMessage="请选择收款账号"
                                    ControlToValidate="rcbxToAccount" ValidationGroup="vgMaintenance" Display="Dynamic"
                                    Text="*" CssClass="field-validation-error">
                                </asp:CustomValidator>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>到账日期</label>
                            <div class="mws-form-item small">
                                <telerik:RadDatePicker runat="server" ID="rdpPayDate" Calendar-EnableShadows="true"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                    Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                    Calendar-FastNavigationSettings-DisableOutOfRangeMonths="true"
                                    Calendar-FirstDayOfWeek="Monday">
                                </telerik:RadDatePicker>
                                <asp:CustomValidator ID="cvPayDate" runat="server" ErrorMessage="请选择到账日期"
                                    ControlToValidate="rdpPayDate" ValidationGroup="vgMaintenance" Display="Dynamic"
                                    Text="*" CssClass="field-validation-error">
                                </asp:CustomValidator>
                            </div>
                        </div>
                    </div>

                    <div id="divDeduction">
                        <div class="mws-form-row">
                            <label>客户余额</label>
                            <div class="mws-form-item small">
                                <telerik:RadNumericTextBox runat="server" ID="txtBalanceAmount" CssClass="mws-textinput" Type="Currency" ShowSpinButtons="true"
                                    NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999"
                                    MaxLength="10" Enabled="false" Value="0">
                                </telerik:RadNumericTextBox>
                            </div>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>金额</label>
                            <div class="mws-form-item small">
                                <telerik:RadNumericTextBox runat="server" ID="txtAmount" CssClass="mws-textinput" Type="Currency" ShowSpinButtons="true"
                                    NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999"
                                    MaxLength="10" ClientEvents-OnValueChanging="onAmountChanging">
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="rfvAmount"
                                    runat="server"
                                    ErrorMessage="金额必填"
                                    ControlToValidate="txtAmount"
                                    Display="Dynamic" CssClass="field-validation-error"
                                    ValidationGroup="vgMaintenance" Text="*">
                                </asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvAmount" runat="server"
                                    ControlToValidate="txtAmount" ValidationGroup="vgMaintenance" Display="Dynamic"
                                    Text="*" CssClass="field-validation-error">
                                </asp:CustomValidator>
                            </div>
                        </div>
                        <div class="float-left" id="divFee">
                            <label>手续费</label>
                            <div class="mws-form-item small">
                                <telerik:RadNumericTextBox runat="server" ID="txtFee" CssClass="mws-textinput" Type="Currency" ShowSpinButtons="true"
                                    NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999"
                                    MaxLength="10" ClientEvents-OnValueChanging="onFeeChanging">
                                </telerik:RadNumericTextBox>
                            </div>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <label>备注</label>
                        <div class="mws-form-item">
                            <telerik:RadTextBox runat="server" ID="txtComment" Width="90%" MaxLength="1000"
                                TextMode="MultiLine" Height="60">
                            </telerik:RadTextBox>
                        </div>
                    </div>
                    <div class="height10"></div>

                    <div class="mws-button-row">
                        <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="closeWindow(false);return false;" />
                    </div>
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

        function onPaymentMethodCheckedChanged(sender, eventArgs) {
            var commandArgument = eventArgs.get_commandArgument();
            var checked = eventArgs.get_checked();

            if (checked && commandArgument === "IsBankTransfer") {
                $("#divBankTransfer").show();
                $("#divFee").show();
                $("#divDeduction").hide();
            }
            else if (checked && commandArgument === "IsDeduction") {
                $("#divDeduction").show();
                $("#divBankTransfer").hide();
                $("#divFee").hide();
            }
        }

        function onAmountChanging(sender, eventArgs) {
            var balanceAmount = $find("<%= txtBalanceAmount.ClientID %>").get_value();

            var newValue = eventArgs.get_newValue();

            if (!isNaN(newValue)) {
                if (newValue == 0) {
                    var radNotification = $.getErrorNotification();
                    radNotification.set_text("金额必须大于0");
                    radNotification.show();

                    eventArgs.set_cancel(true);
                }

                var radioIsDeduction = $find("<%= radioIsDeduction.ClientID%>");
                if (radioIsDeduction.get_checked() == true) {

                    if (newValue > balanceAmount) {
                        var radNotification = $.getErrorNotification();
                        radNotification.set_text("金额不能大于客户余额：" + balanceAmount);
                        radNotification.show();

                        eventArgs.set_cancel(true);
                    }
                }
            }
        }

        function onFeeChanging(sender, eventArgs) {
            var amount = $find("<%= txtAmount.ClientID %>").get_value();

            var newValue = eventArgs.get_newValue();

            if (!isNaN(newValue)) {

                if (newValue > amount) {
                    var radNotification = $.getErrorNotification();
                    radNotification.set_text("手续费不能大于金额");
                    radNotification.show();

                    eventArgs.set_cancel(true);
                }
            }
        }

        $(document).ready(function () {
            var radioIsBankTransfer = $find("<%= radioIsBankTransfer.ClientID%>");

            if (radioIsBankTransfer.get_checked() == true) {
                $("#divBankTransfer").show();
                $("#divFee").show();
                $("#divDeduction").hide();
            }
            else {
                $("#divDeduction").show();
                $("#divBankTransfer").hide();
                $("#divFee").hide();
            }
        });

    </script>
</asp:Content>
