<%@ Page Title="大包出库单维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DBStockOutMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Sales.DBStockOutMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rgAppNotes">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgAppNotes" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgStockOutDetails">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgStockOutDetails" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="divFailValid" />
                    <telerik:AjaxUpdatedControl ControlID="hdnGridCellValueChangedCount" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="container">

        <div class="mws-panel grid_full">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">大包出库单维护</span>
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
                                <label>出库单编号</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtCode" CssClass="mws-textinput" Width="80%" Enabled="false"></telerik:RadTextBox>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>开单日期</label>
                                <div class="mws-form-item toppadding5">
                                    <telerik:RadDatePicker runat="server" ID="rdpBillDate"
                                        Calendar-EnableShadows="true"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                        Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                        Calendar-FastNavigationSettings-DisableOutOfRangeMonths="true"
                                        Calendar-FirstDayOfWeek="Monday">
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="rfvBillDate"
                                        runat="server"
                                        ErrorMessage="开单日期必填"
                                        ControlToValidate="rdpBillDate"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>配送公司</label>
                            <div class="mws-form-item small">
                                <telerik:RadComboBox runat="server" ID="rcbxDistributionCompany" Filter="Contains"
                                    AllowCustomText="false" Height="160px" Width="260" EmptyMessage="--请选择--"
                                    OnItemDataBound="rcbxDistributionCompany_ItemDataBound" OnClientSelectedIndexChanged="onClientSelectedDistributionCompany">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="rfvDistributionCompany"
                                    runat="server"
                                    ErrorMessage="请选择配送公司"
                                    ControlToValidate="rcbxDistributionCompany"
                                    Display="Dynamic" CssClass="field-validation-error"
                                    ValidationGroup="vgMaintenance" Text="*">
                                </asp:RequiredFieldValidator>
                                <asp:Button ID="btnSearchOrders" runat="server" Text="查询订单" CssClass="mws-button green"
                                    CausesValidation="false" OnClientClick="openChooseOrderProductWindow();return false;" />
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width50-percent">
                                <label>收货人</label>
                                <div class="mws-form-item small toppadding5">
                                    <asp:Label ID="lblReceiverName" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>收货电话</label>
                                <div class="mws-form-item small toppadding5">
                                    <asp:Label ID="lblReceiverPhone" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>收货地址</label>
                            <div class="mws-form-item medium toppadding5">
                                <asp:Label ID="lblReceiverAddress" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="mws-form-row" runat="server" id="divComment">
                            <label>备注</label>
                            <div class="mws-form-item medium">
                                <telerik:RadTextBox runat="server" ID="txtComment" Width="90%" MaxLength="1000"
                                    TextMode="MultiLine" Height="80">
                                </telerik:RadTextBox>
                            </div>
                        </div>
                        <div class="mws-form-row" runat="server" id="divComments">
                            <label>备注历史</label>
                            <div class="mws-form-item medium">
                                <telerik:RadDockLayout runat="server" ID="RadDockLayout1">
                                    <telerik:RadDockZone runat="server" ID="RadDockZone1" Orientation="Vertical"
                                        Width="99%" FitDocks="true" BorderStyle="None">
                                        <telerik:RadDock ID="RadDock1" Title="备注历史记录" runat="server" AllowedZones="RadDockZone1" Font-Size="12px"
                                            DefaultCommands="ExpandCollapse" EnableAnimation="true" EnableDrag="false"
                                            DockMode="Docked" ExpandText="展开" CollapseText="折叠">
                                            <ContentTemplate>
                                                <div class="toppadding10"></div>
                                                <telerik:RadGrid ID="rgAppNotes" runat="server"
                                                    AllowPaging="false" AllowSorting="true" AutoGenerateColumns="false" Skin="Silk" Width="99.5%"
                                                    ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                                    OnNeedDataSource="rgAppNotes_NeedDataSource">
                                                    <MasterTableView Width="100%" DataKeyNames="ID" ShowHeader="true" BackColor="#fafafa">
                                                        <Columns>
                                                            <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="CreatedOn" HeaderText="创建时间" DataField="CreatedOn" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}">
                                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="CreatedBy" HeaderText="创建人" DataField="CreatedBy">
                                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="Note" HeaderText="备注内容" DataField="Note">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                        <NoRecordsTemplate>
                                                            没有任何数据
                                                        </NoRecordsTemplate>
                                                        <ItemStyle Height="30" />
                                                        <AlternatingItemStyle BackColor="#f2f2f2" />
                                                    </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                            </ContentTemplate>
                                        </telerik:RadDock>
                                    </telerik:RadDockZone>
                                </telerik:RadDockLayout>
                            </div>
                        </div>
                        <div class="height20"></div>
                        <div class="mws-form-row" runat="server" id="divOtherSections">
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-procure-order">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">出库货品维护</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <div class="validate-message-wrapper bottommargin10" runat="server" id="divFailValid">
                                            <asp:CustomValidator ID="cvStockOutDetails" runat="server" Display="Dynamic" CssClass="field-validation-error"></asp:CustomValidator>
                                        </div>
                                        <telerik:RadGrid ID="rgStockOutDetails" runat="server" PageSize="10"
                                            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" Height="480" ShowHeader="true" ShowFooter="true"
                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgStockOutDetails_NeedDataSource" OnColumnCreated="rgStockOutDetails_ColumnCreated"
                                            OnDeleteCommand="rgStockOutDetails_DeleteCommand">
                                            <MasterTableView Width="100%" DataKeyNames="ID,SalesOrderApplicationID,SalesOrderAppDetailID,ProductID,ProductSpecificationID" CommandItemDisplay="Top"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa" ClientDataKeyNames="ID,SalesQty,ToBeOutQty,BalanceQty,NumberInLargePackage,SalesPrice">
                                                <CommandItemSettings ShowAddNewRecordButton="false" ShowSaveChangesButton="false" ShowCancelChangesButton="false" ShowRefreshButton="true" RefreshText="刷新" />
                                                <Columns>
                                                    <telerik:GridBoundColumn UniqueName="OrderCode" HeaderText="订单编号" DataField="OrderCode" ReadOnly="true">
                                                        <HeaderStyle Width="160" />
                                                        <ItemStyle HorizontalAlign="Left" Width="160" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="WarehouseID" HeaderText="仓库" DataField="WarehouseID" SortExpression="Warehouse">
                                                        <HeaderStyle Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                                        <ItemTemplate>
                                                            <span><%# Eval("Warehouse") %></span>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName" ReadOnly="true">
                                                        <HeaderStyle Width="180" />
                                                        <ItemStyle HorizontalAlign="Left" Width="180" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification" ReadOnly="true">
                                                        <HeaderStyle Width="100" />
                                                        <ItemStyle HorizontalAlign="Left" Width="100" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="FactoryName" HeaderText="生产企业" DataField="FactoryName" ReadOnly="true">
                                                        <HeaderStyle Width="180" />
                                                        <ItemStyle HorizontalAlign="Left" Width="180" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="UnitOfMeasurement" HeaderText="基本单位" DataField="UnitOfMeasurement" ReadOnly="true">
                                                        <HeaderStyle Width="80" />
                                                        <ItemStyle HorizontalAlign="Left" Width="80" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="OutQty" HeaderText="基本数量" DataField="OutQty" SortExpression="OutQty">
                                                        <HeaderStyle Width="100" />
                                                        <ItemStyle HorizontalAlign="Left" Width="100" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOutQty" runat="server" Text='<%# Eval("OutQty") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="NumberOfPackages" HeaderText="件数" DataField="NumberOfPackages" SortExpression="NumberOfPackages" ReadOnly="true">
                                                        <HeaderStyle Width="60" />
                                                        <ItemStyle HorizontalAlign="Left" Width="60" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNumberOfPackages" runat="server" Text='<%# Eval("NumberOfPackages","{0:N2}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="SalesPrice" HeaderText="单价" DataField="SalesPrice" SortExpression="SalesPrice" ReadOnly="true">
                                                        <HeaderStyle Width="60" />
                                                        <ItemStyle HorizontalAlign="Left" Width="60" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSalesPrice" runat="server" Text='<%# Eval("SalesPrice","{0:C2}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="TotalSalesAmount" HeaderText="货款" DataField="TotalSalesAmount" SortExpression="TotalSalesAmount" ReadOnly="true">
                                                        <HeaderStyle Width="80" />
                                                        <ItemStyle HorizontalAlign="Left" Width="80" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotalSalesAmount" runat="server" Text='<%# Eval("TotalSalesAmount","{0:C2}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn UniqueName="BatchNumber" HeaderText="货品批号" DataField="BatchNumber" SortExpression="BatchNumber" ReadOnly="true">
                                                        <HeaderStyle Width="160" />
                                                        <ItemStyle HorizontalAlign="Left" Width="160" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="ExpirationDate" HeaderText="过期日期" DataField="ExpirationDate" SortExpression="ExpirationDate" DataFormatString="{0:yyyy/MM/dd}" ReadOnly="true">
                                                        <HeaderStyle Width="140" />
                                                        <ItemStyle HorizontalAlign="Left" Width="140" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="LicenseNumber" HeaderText="批准文号" DataField="LicenseNumber" SortExpression="LicenseNumber" ReadOnly="true">
                                                        <HeaderStyle Width="160" />
                                                        <ItemStyle HorizontalAlign="Left" Width="160" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete"
                                                        ButtonType="LinkButton" ConfirmText="确认删除该条数据吗？">
                                                        <HeaderStyle Width="80" />
                                                        <ItemStyle HorizontalAlign="Justify" Width="80" />
                                                    </telerik:GridButtonColumn>
                                                    <telerik:GridTemplateColumn UniqueName="ToBeOutQty" DataField="ToBeOutQty" HeaderText="">
                                                        <HeaderStyle Width="80" />
                                                        <ItemStyle HorizontalAlign="Left" Width="100" />
                                                        <ItemTemplate>
                                                            <span class="width60">&nbsp;</span>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                                <NoRecordsTemplate>
                                                    没有任何数据
                                                </NoRecordsTemplate>
                                                <ItemStyle Height="30" />
                                                <AlternatingItemStyle BackColor="#f2f2f2" />
                                                <PagerStyle PagerTextFormat="{4} 第{0}页/共{1}页, 第{2}-{3}条 共{5}条"
                                                    PageSizeControlType="RadComboBox" PageSizeLabelText="每页条数:"
                                                    FirstPageToolTip="第一页" PrevPageToolTip="上一页" NextPageToolTip="下一页" LastPageToolTip="最后一页" />
                                            </MasterTableView>
                                            <ClientSettings EnableRowHoverStyle="true">
                                                <Scrolling AllowScroll="true" FrozenColumnsCount="3" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                <ClientEvents OnBatchEditCellValueChanging="onCellValueChanging" OnBatchEditClosing="onBatchEditClosing"
                                                    OnBatchEditGetEditorValue="onGetEditorValue" OnBatchEditCellValueChanged="onCellValueChanged" />
                                            </ClientSettings>
                                            <HeaderStyle Width="100%" />
                                        </telerik:RadGrid>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="mws-button-row">
                            <asp:Button ID="btnSave" runat="server" Text="暂存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClientClick="return checkGridCellValueChanged();" OnClick="btnSave_Click" />
                            <asp:Button ID="btnSubmit" runat="server" Text="提交" CssClass="mws-button green" CausesValidation="true" OnClientClick="return checkGridCellValueChanged();" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnOutStock" runat="server" Text="确认出库" CssClass="mws-button green" CausesValidation="true" OnClick="btnOutStock_Click" />
                            <asp:Button ID="btnPrint" runat="server" Text="打印" CssClass="mws-button green" CausesValidation="true" OnClientClick="openPrintPage();return false;" Visible="false" />
                            <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Sales/DBStockOutManagement.aspx');return false;" />
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

    <asp:HiddenField ID="hdnCurrentEntityID" runat="server" Value="-1" />
    <asp:HiddenField ID="hdnGridCellValueChangedCount" runat="server" Value="0" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <style>
        div#divGridCombox td, td:first-child {
            border-left-style: solid;
        }
    </style>

    <script type="text/javascript">

        var currentEntityID = -1;

        var gridClientIDs = {
            gridStockOutDetails: "<%= rgStockOutDetails.ClientID %>",

        };

        function refreshGrid(gridClientID) {
            var gridObj = $find(gridClientID);

            if (gridObj)
                gridObj.get_masterTableView().rebind();
        }

        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/Sales/DBStockOutManagement.aspx");
        }

        function refreshMaintenancePage(sender, args) {

            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            redirectToPage("Views/Sales/DBStockOutMaintenance.aspx?EntityID=" + currentEntityID);
        }

        function openPrintPage() {

            var targetUrl = $.getRootPath() + "Views/Sales/Printers/PrintStockOut.aspx?EntityID=" + currentEntityID;

            window.open(targetUrl, "_blank");
        }

        function openChooseOrderProductWindow() {
            $.showLoading();

            var distCompanyID = $find("<%= rcbxDistributionCompany.ClientID %>").get_value();

            if (distCompanyID && distCompanyID > 0) {
                var targetUrl = $.getRootPath() + "Views/Sales/Editors/ChooseSalesOrderProducts.aspx?DistributionCompanyID=" + distCompanyID
                + "&OwnerEntityID=" + currentEntityID + "&GridClientID=" + gridClientIDs.gridStockOutDetails;

                $.openRadWindow(targetUrl, "winStockOutProduct", true, 1000, 600);
            }
        }

        function onCellValueChanged(sender, args) {
            //debugger;

            var tableView = args.get_tableView();

            var hdnGridCellValueChangedCount = $("#<%=hdnGridCellValueChangedCount.ClientID%>");

            var oChangedCount = parseInt(hdnGridCellValueChangedCount.val(), 0);

            if (args.get_editorValue() != args.get_cellValue())
                oChangedCount = oChangedCount + 1;
            else
                oChangedCount = oChangedCount - 1;

            hdnGridCellValueChangedCount.val(oChangedCount);
        }

        function onCellValueChanging(sender, args) {
            //debugger;

            var colUniqueName = args.get_columnUniqueName();

            if (colUniqueName.toLowerCase() === "outqty") {

                var tableView = args.get_tableView();

                var dataItemIndex = args.get_row().rowIndex - 1;

                var BalanceQty = parseInt(tableView.get_dataItems()[dataItemIndex].getDataKeyValue("BalanceQty"));
                var salesQty = parseInt(tableView.get_dataItems()[dataItemIndex].getDataKeyValue("SalesQty"));
                var toBeOutQty = parseInt(tableView.get_dataItems()[dataItemIndex].getDataKeyValue("ToBeOutQty"));

                var numberInLargePackage = parseInt(tableView.get_dataItems()[dataItemIndex].getDataKeyValue("NumberInLargePackage"));

                var editorValue = args.get_editorValue();

                var radNotification = $find("<%=radNotification.ClientID%>");

                if (editorValue > BalanceQty) {

                    radNotification.set_text("基本数量必须小于库存数量：" + BalanceQty);
                    radNotification.show();

                    args.set_cancel(true);

                    return;
                }

                if (editorValue > salesQty) {
                    radNotification.set_text("基本数量必须小于需发货数量：" + salesQty);
                    radNotification.show();

                    args.set_cancel(true);

                    return;
                }

                if (editorValue > toBeOutQty) {
                    radNotification.set_text("基本数量必须小于剩余发货数量：" + toBeOutQty);
                    radNotification.show();

                    args.set_cancel(true);

                    return;
                }

                if (numberInLargePackage && numberInLargePackage > 0) {
                    //args.get_row().cells['8'].innerText = Math.round(editorValue / numberInLargePackage).toFixed(2);

                    var lblNumberOfPackages = $($telerik.findElement(args.get_row(), "lblNumberOfPackages"));

                    if (lblNumberOfPackages)
                        lblNumberOfPackages.text((editorValue / numberInLargePackage).toFixed(2));
                }

                var salesPrice = parseFloat(tableView.get_dataItems()[dataItemIndex].getDataKeyValue("SalesPrice"));

                if (salesPrice) {
                    var lblTotalSalesAmount = $($telerik.findElement(args.get_row(), "lblTotalSalesAmount"));

                    if (lblTotalSalesAmount)
                        lblTotalSalesAmount.text("¥" + (editorValue * salesPrice).toFixed(2));
                }

            }
        }

        var isNullOrEmptyEditValue = false;

        function onBatchEditClosing(sender, args) {
            //debugger;

            var uniqueName = args.get_columnUniqueName();

            if (isNullOrEmptyEditValue) {
                var radNotification = $find("<%=radNotification.ClientID%>");

                radNotification.set_text("该项为必填项");
                radNotification.show();

                args.set_cancel(true);
            }

            switch (uniqueName) {

                case "OutQty":
                    if (isNullOrEmptyEditValue)
                        $($telerik.findElement(args.get_cell(), "rfvOutQty")).show();
                    else
                        $($telerik.findElement(args.get_cell(), "rfvOutQty")).hide();
                    break;
            }
        }

        function onGetEditorValue(sender, args) {
            //debugger;

            var uniqueName = args.get_columnUniqueName();
            var container = args.get_container();
            var editValue = null;
            var needValid = true;

            if (container) {

                switch (uniqueName) {
                    case "OutQty":
                        editValue = $telerik.findControl(container, "txtOutQty").get_value();
                        break;

                    default:
                        needValid = false;
                }

                if (needValid) {
                    if (editValue == null || editValue.toString().isNullOrEmpty())
                        isNullOrEmptyEditValue = true;
                    else
                        isNullOrEmptyEditValue = false;
                }
                else
                    isNullOrEmptyEditValue = false;
            }
        }

        window.onbeforeunload = function (e) {
            //debugger;
            var gridCellValueChangedCount = parseInt($("#<%= hdnGridCellValueChangedCount.ClientID%>").val(), 0);

            if (gridCellValueChangedCount > 0) {

                e.preventDefault();

                var returnValue = "入库货品信息还没有保存";

                if ($telerik.isIE)
                    returnValue += ", 确定要离开此页吗？"

                window.event.returnValue = returnValue;
            }
        }

        function checkGridCellValueChanged() {
            //debugger;

            if (parseInt(currentEntityID, 0) > 0) {
                var gridCellValueChangedCount = parseInt($("#<%= hdnGridCellValueChangedCount.ClientID%>").val(), 0);

                if (gridCellValueChangedCount > 0) {
                    return confirm("入库货品信息还没有保存, 确定要放弃保存吗？");
                }
            }
        }

        function onClientSelectedDistributionCompany(sender, eventArgs) {
            var item = sender.get_selectedItem();
            var extension = item.get_attributes().getAttribute("Extension");

            if (extension) {
                var extensionObj = JSON.parse(extension);

                if (extensionObj) {
                    $("#<%= lblReceiverName.ClientID %>").html(extensionObj.ReceiverName);
                    $("#<%= lblReceiverPhone.ClientID %>").html(extensionObj.PhoneNumber);
                    $("#<%= lblReceiverAddress.ClientID %>").html(extensionObj.Address);
                }
            }
        }

        $(document).ready(function () {
            currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();
        });

    </script>
</asp:Content>
