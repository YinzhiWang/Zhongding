<%@ Page Title="大包收款维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DBClientInvoiceSettlementMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Invoices.DBClientInvoiceSettlementMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgDBClientInvoices" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgDBClientInvoices" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbxDistributionCompany">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgDBClientInvoices" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgDBClientInvoices">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgDBClientInvoices" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="container">

        <div class="mws-panel grid_full">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">大包收款维护</span>
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
                            <div class="float-left width40-percent">
                                <label>收款日期</label>
                                <div class="mws-form-item toppadding5">
                                    <telerik:RadDatePicker runat="server" ID="rdpReceiveDate"
                                        Calendar-EnableShadows="true"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                        Calendar-FirstDayOfWeek="Monday">
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="rfvReceiveDate"
                                        runat="server"
                                        ErrorMessage="收款日期必填"
                                        ControlToValidate="rdpReceiveDate"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="float-left width60-percent">
                                <label>收款账户</label>
                                <div class="mws-form-item toppadding5">
                                    <telerik:RadComboBox runat="server" ID="rcbxToAccount" Filter="Contains"
                                        AllowCustomText="false" Height="160px" Width="80%" EmptyMessage="--请选择--">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="rfvToAccount"
                                        runat="server"
                                        ErrorMessage="请选择收款账户"
                                        ControlToValidate="rcbxToAccount"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>总部认款日期</label>
                                <div class="mws-form-item toppadding5">
                                    <telerik:RadDatePicker runat="server" ID="rdpConfirmDate"
                                        Calendar-EnableShadows="true"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                        Calendar-FirstDayOfWeek="Monday">
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="rfvConfirmDate"
                                        runat="server"
                                        ErrorMessage="总部认款日期必填"
                                        ControlToValidate="rdpConfirmDate"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="float-left width60-percent">

                                <label>配送公司</label>
                                <div class="mws-form-item">
                                    <telerik:RadComboBox runat="server" ID="rcbxDistributionCompany" Filter="Contains"
                                        AllowCustomText="false" Height="160px" EmptyMessage="--请选择--"
                                        AutoPostBack="true" OnSelectedIndexChanged="rcbxDistributionCompany_SelectedIndexChanged">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="rfvDistributionCompany"
                                        runat="server"
                                        ErrorMessage="请选择配送公司"
                                        ControlToValidate="rcbxDistributionCompany"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>总金额</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="mws-form-row" runat="server" id="divCancel">
                            <label>撤销理由</label>
                            <div class="mws-form-item large">
                                <telerik:RadTextBox runat="server" ID="txtCancelComment" Width="90%" MaxLength="1000"
                                    TextMode="MultiLine" Height="80">
                                </telerik:RadTextBox>
                                <asp:CustomValidator ID="cvCancelComment" runat="server" Display="Dynamic" ErrorMessage="撤销理由必填"
                                    ControlToValidate="txtCancelComment" ValidationGroup="vgMaintenance" Text="*" CssClass="field-validation-error">
                                </asp:CustomValidator>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-invoices">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">收款明细</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <table runat="server" id="tblSearch" class="leftmargin10">
                                            <tr class="height40">
                                                <td class="middle-td leftpadding10">
                                                    <label>开票日期</label>
                                                    <div class="mws-form-item">
                                                        <telerik:RadDatePicker runat="server" ID="rdpBeginDate" Width="120"></telerik:RadDatePicker>
                                                        -&nbsp;&nbsp;
                                                        <telerik:RadDatePicker runat="server" ID="rdpEndDate" Width="120"></telerik:RadDatePicker>
                                                    </div>
                                                </td>
                                                <td class="middle-td leftpadding20">
                                                    <label>发票号</label>
                                                    <div class="mws-form-item">
                                                        <telerik:RadTextBox runat="server" ID="txtInvoiceNumber" MaxLength="1000">
                                                        </telerik:RadTextBox>
                                                    </div>
                                                </td>
                                                <td class="middle-td leftpadding20">
                                                    <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                                                </td>
                                                <td class="middle-td leftpadding20">
                                                    <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <telerik:RadGrid ID="rgDBClientInvoices" runat="server" PageSize="10" AllowCustomPaging="true"
                                            AllowPaging="false" AllowSorting="True" AutoGenerateColumns="false" AllowMultiRowSelection="true"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" Height="380" ShowHeader="true" ShowFooter="false"
                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgDBClientInvoices_NeedDataSource" OnColumnCreated="rgDBClientInvoices_ColumnCreated"
                                            OnItemDataBound="rgDBClientInvoices_ItemDataBound">
                                            <MasterTableView Width="100%" DataKeyNames="ID,DBClientInvoiceID" CommandItemDisplay="None"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa" ClientDataKeyNames="ID,DBClientInvoiceID,ToBeReceiveAmount">
                                                <Columns>
                                                    <telerik:GridClientSelectColumn UniqueName="ClientSelect" HeaderText="全选">
                                                        <HeaderStyle Width="40" />
                                                        <ItemStyle Width="40" />
                                                    </telerik:GridClientSelectColumn>
                                                    <telerik:GridBoundColumn UniqueName="InvoiceDate" HeaderText="开票日期" DataField="InvoiceDate" DataFormatString="{0:yyyy/MM/dd}">
                                                        <HeaderStyle Width="80" />
                                                        <ItemStyle HorizontalAlign="Left" Width="80" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="CompanyName" HeaderText="开票单位" DataField="CompanyName">
                                                        <HeaderStyle Width="180" />
                                                        <ItemStyle HorizontalAlign="Left" Width="180" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="DistributionCompanyName" HeaderText="收票单位" DataField="DistributionCompanyName">
                                                        <HeaderStyle Width="180" />
                                                        <ItemStyle HorizontalAlign="Left" Width="180" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="InvoiceNumber" HeaderText="发票号" DataField="InvoiceNumber">
                                                        <HeaderStyle Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="InvoiceAmount" HeaderText="发票金额" DataField="InvoiceAmount" DataFormatString="{0:C2}">
                                                        <HeaderStyle Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="ReceivedAmount" HeaderText="已收款" DataField="ReceivedAmount" DataFormatString="{0:C2}">
                                                        <HeaderStyle Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="ToBeReceiveAmount" HeaderText="未收款" DataField="ToBeReceiveAmount" DataFormatString="{0:C2}">
                                                        <HeaderStyle Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="CurrentReceiveAmount" HeaderText="本次收款" DataField="CurrentReceiveAmount" SortExpression="CurrentReceiveAmount">
                                                        <HeaderStyle Width="140" />
                                                        <ItemStyle HorizontalAlign="Left" Width="140" />
                                                        <ItemTemplate>
                                                            <telerik:RadNumericTextBox runat="server" ID="txtCurrentReceiveAmount" Type="Currency" MaxLength="9" Width="120" ShowSpinButtons="true"
                                                                MinValue="0" MaxValue="99999999" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="">
                                                                <ClientEvents OnValueChanging="onCurrentReceiveAmountChanging" OnValueChanged="onCurrentReceiveAmountChanged" />
                                                            </telerik:RadNumericTextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                                <CommandItemTemplate>
                                                    <table class="width100-percent">
                                                        <tr>
                                                            <td></td>
                                                            <td class="right-td rightpadding10">
                                                                <input type="button" class="rgRefresh" onclick="refreshGrid(); return false;" />
                                                                <a href="javascript:void(0);" onclick="refreshGrid(); return false;">刷新</a>
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
                                                <Selecting AllowRowSelect="True" UseClientSelectColumnOnly="true" />
                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" />
                                                <ClientEvents OnGridCreated="GetsGridObject" OnRowSelecting="onRowSelecting"
                                                    OnRowSelected="onRowSelected" OnRowDeselected="onRowDeselected" />
                                            </ClientSettings>
                                            <HeaderStyle Width="99.8%" />
                                        </telerik:RadGrid>
                                        <div class="float-right">
                                            <span class="bold">收款总金额</span>：<asp:Label ID="lblTotalReceiveAmount" runat="server"></asp:Label>元&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span class="bold">大写</span>：<asp:Label ID="lblCapitalTotalReceiveAmount" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="mws-button-row">
                            <asp:Button ID="btnSubmit" runat="server" Text="确认收款" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="撤销" CssClass="mws-button orange" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnCancel_Click" />
                            <asp:Button ID="btnBack" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Invoices/DBClientInvoiceSettlementManagement.aspx');return false;" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <telerik:RadNotification ID="radNotification" runat="server" EnableRoundedCorners="true"
        AutoCloseDelay="1000" Skin="Silk" Animation="Fade" EnableShadow="true" Title="提示"
        TitleIcon="none" Opacity="95" Position="Center" ContentIcon="~/Content/icons/32/cross.png"
        Width="300" Height="100">
    </telerik:RadNotification>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">
        var gridOfRefresh = null;

        function GetsGridObject(sender, eventArgs) {
            gridOfRefresh = sender;
        }

        function refreshGrid() {
            gridOfRefresh.get_masterTableView().rebind();
        }

        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/Invoices/DBClientInvoiceSettlementManagement.aspx");
        }

        function onCurrentReceiveAmountChanging(sender, eventArgs) {
            //debugger;

            var gridItem = sender.get_parent();

            if (gridItem) {

                var newValue = eventArgs.get_newValue();

                if (newValue) {

                    var toBeReceiveAmount = parseInt(gridItem.getDataKeyValue("ToBeReceiveAmount"));

                    if (newValue > toBeReceiveAmount) {

                        var radNotification = $find("<%=radNotification.ClientID%>");

                        radNotification.set_text("本次收款金额不能大于未收款：" + toBeReceiveAmount);
                        radNotification.show();

                        eventArgs.set_cancel(true);
                    }
                }
                else {
                    var radNotification = $find("<%=radNotification.ClientID%>");

                    radNotification.set_text("本次收款金额为必填项");
                    radNotification.show();

                    eventArgs.set_cancel(true);
                }
            }
        }

        function onCurrentReceiveAmountChanged(sender, eventArgs) {

            calculateGridTotalAmount();
        }

        function onRowSelecting(sender, eventArgs) {
            var selectingItem = eventArgs.get_gridDataItem();
            var selectingElement = selectingItem.get_element();

            var txtCurrentReceiveAmount = $telerik.findControl(selectingElement, "txtCurrentReceiveAmount");
            var currentReceiveAmount = txtCurrentReceiveAmount.get_value();

            if (currentReceiveAmount == null || currentReceiveAmount == "") {
                var radNotification = $find("<%=radNotification.ClientID%>");

                radNotification.set_text("本次收款金额为必填项");
                radNotification.show();

                eventArgs.set_cancel(true);
            }
            else {
                var toBeReceiveAmount = parseInt(selectingItem.getDataKeyValue("ToBeReceiveAmount"));

                if (toBeReceiveAmount > 0 && currentReceiveAmount > toBeReceiveAmount) {

                    var radNotification = $find("<%=radNotification.ClientID%>");

                    radNotification.set_text("本次收款金额不能大于未收款：" + toBeReceiveAmount);
                    radNotification.show();

                    eventArgs.set_cancel(true);
                }
            }
        }

        function onRowSelected(sender, eventArgs) {
            calculateTotalAmount(sender, eventArgs);
        }

        function onRowDeselected(sender, eventArgs) {
            calculateTotalAmount(sender, eventArgs);
        }

        //计算动态操作时grid选中项的总金额
        function calculateTotalAmount(sender, eventArgs) {
            var selectedTotalReceiveAmount = 0;

            //获取已经选中的items
            var selectedItems = eventArgs.get_tableView().get_selectedItems();

            for (var i = 0; i < selectedItems.length; i++) {
                var curSelectedItem = selectedItems[i];
                var curSelectedItemElement = curSelectedItem.get_element();

                var txtCurrentReceiveAmount = $telerik.findControl(curSelectedItemElement, "txtCurrentReceiveAmount");
                var currentReceiveAmount = txtCurrentReceiveAmount.get_value();

                if (currentReceiveAmount) {
                    selectedTotalReceiveAmount += currentReceiveAmount;
                }
            }

            selectedTotalReceiveAmount = selectedTotalReceiveAmount.toFixed(2);

            $("#<%=lblTotalAmount.ClientID%>").html("¥" + selectedTotalReceiveAmount);
            $("#<%=lblTotalReceiveAmount.ClientID%>").html("¥" + selectedTotalReceiveAmount);

            $("#<%=lblCapitalTotalReceiveAmount.ClientID%>").html($.convertToCapitalChinese(selectedTotalReceiveAmount));
        }

        //计算grid里选中项的总金额
        function calculateGridTotalAmount() {
            debugger;
            var selectedTotalReceiveAmount = 0;

            //获取已经选中的items
            var selectedItems = gridOfRefresh.get_masterTableView().get_selectedItems();

            for (var i = 0; i < selectedItems.length; i++) {
                var curSelectedItem = selectedItems[i];
                var curSelectedItemElement = curSelectedItem.get_element();

                var txtCurrentReceiveAmount = $telerik.findControl(curSelectedItemElement, "txtCurrentReceiveAmount");
                var currentReceiveAmount = txtCurrentReceiveAmount.get_value();

                if (currentReceiveAmount) {
                    selectedTotalReceiveAmount += currentReceiveAmount;
                }
            }

            if (selectedTotalReceiveAmount > 0) {

                selectedTotalReceiveAmount = selectedTotalReceiveAmount.toFixed(2);

                $("#<%=lblTotalAmount.ClientID%>").html("¥" + selectedTotalReceiveAmount);
                $("#<%=lblTotalReceiveAmount.ClientID%>").html("¥" + selectedTotalReceiveAmount);

                $("#<%=lblCapitalTotalReceiveAmount.ClientID%>").html($.convertToCapitalChinese(selectedTotalReceiveAmount));
            }
        }

        $(window).load(function () {

            calculateGridTotalAmount();

        });

    </script>
</asp:Content>
