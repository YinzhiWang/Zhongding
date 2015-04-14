<%@ Page Title="添加支付信息" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="DBClientSettleBonusPayment.aspx.cs" Inherits="ZhongDing.Web.Views.Settlements.Editors.DBClientSettleBonusPayment" %>

<%@ MasterType VirtualPath="~/Site.Window.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rgDBClientSettleBonus">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgDBClientSettleBonus" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
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
                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>转账日期</label>
                            <div class="mws-form-item small">
                                <telerik:RadDatePicker runat="server" ID="rdpPayDate" Calendar-EnableShadows="true"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                    Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                    Calendar-FastNavigationSettings-DisableOutOfRangeMonths="true"
                                    Calendar-FirstDayOfWeek="Monday">
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="rfvPayDate"
                                    runat="server"
                                    ErrorMessage="转账日期必填"
                                    ControlToValidate="rdpPayDate"
                                    Display="Dynamic" CssClass="field-validation-error"
                                    ValidationGroup="vgMaintenance" Text="*">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="float-left">
                            <label>转出账号</label>
                            <div class="mws-form-item small">
                                <telerik:RadComboBox runat="server" ID="rcbxFromAccount" Filter="Contains"
                                    AllowCustomText="false" Height="160px" Width="60%" EmptyMessage="--请选择--">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="rfvFromAccount"
                                    runat="server"
                                    ErrorMessage="转出账号必填"
                                    ControlToValidate="rcbxFromAccount"
                                    Display="Dynamic" CssClass="field-validation-error"
                                    ValidationGroup="vgMaintenance" Text="*">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <telerik:RadGrid ID="rgDBClientSettleBonus" runat="server" PageSize="10" AllowCustomPaging="true"
                            AllowPaging="false" AllowSorting="True" AutoGenerateColumns="false" AllowMultiRowSelection="true"
                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" Height="380" ShowHeader="true" ShowFooter="false"
                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                            OnNeedDataSource="rgDBClientSettleBonus_NeedDataSource" OnColumnCreated="rgDBClientSettleBonus_ColumnCreated"
                            OnItemDataBound="rgDBClientSettleBonus_ItemDataBound">
                            <MasterTableView Width="100%" DataKeyNames="ID,TotalPayAmount" CommandItemDisplay="None"
                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa" ClientDataKeyNames="ID,TotalPayAmount">
                                <Columns>
                                    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderText="全选">
                                        <HeaderStyle Width="40" />
                                        <ItemStyle Width="40" />
                                    </telerik:GridClientSelectColumn>
                                    <telerik:GridBoundColumn UniqueName="ClientUserName" HeaderText="客户" DataField="ClientUserName">
                                        <HeaderStyle Width="80" />
                                        <ItemStyle HorizontalAlign="Left" Width="80" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="ClientDBBankAccount" HeaderText="银行账号" DataField="ClientDBBankAccount">
                                        <HeaderStyle Width="260" />
                                        <ItemStyle HorizontalAlign="Left" Width="260" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="SettlementDate" HeaderText="年月" DataField="SettlementDate" DataFormatString="{0:yyyy/MM}">
                                        <HeaderStyle Width="60" />
                                        <ItemStyle HorizontalAlign="Left" Width="60" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="TotalPayAmount" HeaderText="应发" DataField="TotalPayAmount" DataFormatString="{0:C2}">
                                        <HeaderStyle Width="120" />
                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn UniqueName="Fee" HeaderText="手续费" DataField="Fee" SortExpression="Fee">
                                        <HeaderStyle Width="100" />
                                        <ItemStyle HorizontalAlign="Left" Width="100" />
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox runat="server" ID="txtFee" Type="Currency" MaxLength="9" Width="80" ShowSpinButtons="true"
                                                MinValue="0" MaxValue="99999999" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="">
                                                <ClientEvents OnValueChanging="onFeeChanging" />
                                            </telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <CommandItemTemplate>
                                    <table class="width100-percent">
                                        <tr>
                                            <td></td>
                                            <td class="right-td rightpadding10">
                                                <input type="button" class="rgRefresh" onclick="refreshGrid(gridClientIDs.gridDBClientSettleBonus); return false;" />
                                                <a href="javascript:void(0);" onclick="refreshGrid(gridClientIDs.gridDBClientSettleBonus); return false;">刷新</a>
                                            </td>
                                        </tr>
                                    </table>
                                </CommandItemTemplate>
                                <NoRecordsTemplate>
                                    没有任何数据
                                </NoRecordsTemplate>
                                <ItemStyle Height="30" />
                                <AlternatingItemStyle BackColor="#f2f2f2" />
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true">
                                <Selecting AllowRowSelect="True" />
                                <Scrolling AllowScroll="true" SaveScrollPosition="true" />
                                <ClientEvents OnRowSelecting="onRowSelecting" />
                            </ClientSettings>
                            <HeaderStyle Width="99.8%" />
                        </telerik:RadGrid>
                    </div>

                    <div class="mws-button-row">
                        <asp:Button ID="btnPay" runat="server" Text="确认支付" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnPay_Click" />
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

        var gridClientIDs = {
            gridDBClientSettleBonus: "<%= rgDBClientSettleBonus.ClientID %>"
        };

        function refreshGrid(gridClientID) {
            var gridObj = $find(gridClientID);

            if (gridObj)
                gridObj.get_masterTableView().rebind();
        }

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

        function onFeeChanging(sender, eventArgs) {
            //debugger;

            var gridItem = sender.get_parent();

            if (gridItem) {

                var newValue = eventArgs.get_newValue();

                if (newValue) {

                    var totalPayAmount = parseFloat(gridItem.getDataKeyValue("TotalPayAmount"));

                    if (newValue > totalPayAmount) {

                        var radNotification = $find("<%=radNotification.ClientID%>");

                        radNotification.set_text("手续费不能大于应发金额：" + totalPayAmount);
                        radNotification.show();

                        eventArgs.set_cancel(true);
                    }
                }
            }
        }

        function onRowSelecting(sender, eventArgs) {
            //debugger;

            var selectingItem = eventArgs.get_gridDataItem();
            var selectingElement = selectingItem.get_element();

            var txtFee = $telerik.findControl(selectingElement, "txtFee");
            var currentFee = txtFee.get_value();

            if (currentFee == null) {

            }

        }


    </script>
</asp:Content>
