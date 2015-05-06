<%@ Page Title="入库单维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StockInMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Procures.StockInMaintenance" %>

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
            <telerik:AjaxSetting AjaxControlID="rgStockInDetails">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgStockInDetails" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="divFailValid" />
                    <telerik:AjaxUpdatedControl ControlID="hdnGridCellValueChangedCount" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
        <ClientEvents OnResponseEnd="onRequestEnd" />
    </telerik:RadAjaxManager>

    <div class="container">

        <div class="mws-panel grid_full">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">入库单维护</span>
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
                                <label>入库单编号</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtCode" CssClass="mws-textinput" Width="80%" Enabled="false"></telerik:RadTextBox>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>操作人</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblCreateBy" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width50-percent">
                                <label>供应商</label>
                                <div class="mws-form-item small">
                                    <telerik:RadComboBox runat="server" ID="rcbxSupplier" Filter="Contains"
                                        AllowCustomText="false" Height="160px" Width="260" EmptyMessage="--请选择--">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="rfvSupplier"
                                        runat="server"
                                        ErrorMessage="请选择供应商"
                                        ControlToValidate="rcbxSupplier"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>入库日期</label>
                                <div class="mws-form-item small">
                                    <telerik:RadDatePicker runat="server" ID="rdpEntryDate"
                                        Calendar-EnableShadows="true"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                        Calendar-FirstDayOfWeek="Monday">
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="rfvOrderDate"
                                        runat="server"
                                        ErrorMessage="入库日期必填"
                                        ControlToValidate="rdpEntryDate"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
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
                                    <span class="mws-i-24 i-creditcard">入库货品维护</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <div class="validate-message-wrapper bottommargin10" runat="server" id="divFailValid">
                                            <asp:CustomValidator ID="cvStockInDetails" runat="server" Display="Dynamic" CssClass="field-validation-error"></asp:CustomValidator>
                                        </div>
                                        <telerik:RadGrid ID="rgStockInDetails" runat="server" PageSize="10"
                                            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" Height="480" ShowHeader="true" ShowFooter="true"
                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgStockInDetails_NeedDataSource" OnDeleteCommand="rgStockInDetails_DeleteCommand"
                                            OnItemCreated="rgStockInDetails_ItemCreated" OnColumnCreated="rgStockInDetails_ColumnCreated"
                                            OnBatchEditCommand="rgStockInDetails_BatchEditCommand">
                                            <MasterTableView Width="100%" DataKeyNames="ID,ProcureOrderAppID,ProcureOrderAppDetailID,ProductID,ProductSpecificationID,WarehouseID" CommandItemDisplay="Top" EditMode="Batch"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa" ClientDataKeyNames="ID,ProcureCount,ToBeInQty">
                                                <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="添加" ShowSaveChangesButton="true" ShowCancelChangesButton="false"
                                                    SaveChangesText="保存" CancelChangesText="取消" RefreshText="刷新" />
                                                <BatchEditingSettings EditType="Cell" />
                                                <Columns>
                                                    <telerik:GridBoundColumn UniqueName="OrderCode" HeaderText="采购订单编号" DataField="OrderCode" ReadOnly="true">
                                                        <HeaderStyle Width="160" />
                                                        <ItemStyle HorizontalAlign="Left" Width="160" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="WarehouseID" HeaderText="入库仓库" DataField="WarehouseID" SortExpression="Warehouse">
                                                        <HeaderStyle Width="160" />
                                                        <ItemStyle HorizontalAlign="Left" Width="160" />
                                                        <ItemTemplate>
                                                            <span><%# Eval("Warehouse") %></span>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <telerik:RadComboBox runat="server" ID="rcbxWarehouse" OnLoad="rcbxWarehouse_Load" Width="140">
                                                            </telerik:RadComboBox>
                                                        </EditItemTemplate>
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
                                                    <telerik:GridTemplateColumn UniqueName="InQty" HeaderText="基本数量" DataField="InQty" SortExpression="InQty">
                                                        <HeaderStyle Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInQty" runat="server" Text='<%# Eval("InQty") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <telerik:RadNumericTextBox runat="server" ID="txtInQty" Value='<%# Eval("InQty") %>' Type="Number" MaxLength="9"
                                                                Width="100" ShowSpinButtons="true" MinValue="1" MaxValue="99999999" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="">
                                                            </telerik:RadNumericTextBox>
                                                            <span style="color: Red">
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvInQty" ControlToValidate="txtInQty" ToolTip="基本数量必填"
                                                                    ErrorMessage="基本数量必填" Display="Dynamic" ValidationGroup="BatchEditingValidationGroup" CssClass="field-validation-error" Text="*">
                                                                </asp:RequiredFieldValidator>
                                                            </span>
                                                        </EditItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn UniqueName="NumberOfPackages" HeaderText="件数" DataField="NumberOfPackages" DataFormatString="{0:N2}" ReadOnly="true">
                                                        <HeaderStyle Width="80" />
                                                        <ItemStyle HorizontalAlign="Left" Width="80" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="BatchNumber" HeaderText="货品批号" DataField="BatchNumber" SortExpression="BatchNumber">
                                                        <HeaderStyle Width="160" />
                                                        <ItemStyle HorizontalAlign="Left" Width="160" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBatchNumber" runat="server" Text='<%# Eval("BatchNumber") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <telerik:RadTextBox runat="server" ID="txtBatchNumber" Text='<%# Eval("BatchNumber") %>' Width="130" MaxLength="200"></telerik:RadTextBox>
                                                            <span style="color: Red">
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvBatchNumber" ControlToValidate="txtBatchNumber"
                                                                    ToolTip="货品批号必填" ErrorMessage="货品批号必填" Display="Dynamic" ValidationGroup="BatchEditingValidationGroup" CssClass="field-validation-error" Text="*">
                                                                </asp:RequiredFieldValidator>
                                                            </span>
                                                        </EditItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="ExpirationDate" HeaderText="过期日期" DataField="ExpirationDate" SortExpression="ExpirationDate">
                                                        <HeaderStyle Width="140" />
                                                        <ItemStyle HorizontalAlign="Left" Width="140" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblExpirationDate" runat="server" Text='<%# Eval("ExpirationDate","{0:yyyy/MM/dd}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <telerik:RadDatePicker runat="server" ID="rdpExpirationDate" SelectedDate='<%# Eval("ExpirationDate") %>' Width="120"></telerik:RadDatePicker>
                                                            <span style="color: Red">
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvExpirationDate" ControlToValidate="rdpExpirationDate" ToolTip="过期日期必选"
                                                                    ErrorMessage="过期日期必选" Display="Dynamic" ValidationGroup="BatchEditingValidationGroup" CssClass="field-validation-error" Text="*">
                                                                </asp:RequiredFieldValidator>
                                                            </span>
                                                        </EditItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="LicenseNumber" HeaderText="批准文号" DataField="LicenseNumber" SortExpression="LicenseNumber">
                                                        <HeaderStyle Width="160" />
                                                        <ItemStyle HorizontalAlign="Left" Width="160" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLicenseNumber" runat="server" Text='<%# Eval("LicenseNumber") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <telerik:RadTextBox runat="server" ID="txtLicenseNumber" Text='<%# Eval("LicenseNumber") %>' Width="130" MaxLength="200"></telerik:RadTextBox>
                                                            <span style="color: Red">
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvLicenseNumber" ControlToValidate="txtLicenseNumber" ToolTip="批准文号必填" ErrorMessage="批准文号必填"
                                                                    Display="Dynamic" ValidationGroup="BatchEditingValidationGroup" CssClass="field-validation-error" Text="*">
                                                                </asp:RequiredFieldValidator>
                                                            </span>
                                                        </EditItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridCheckBoxColumn UniqueName="IsMortgagedProduct" HeaderText="抵款货物?" DataField="IsMortgagedProduct" SortExpression="IsMortgagedProduct">
                                                        <HeaderStyle Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                                    </telerik:GridCheckBoxColumn>
                                                    <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete"
                                                        ButtonType="LinkButton" ConfirmText="确认删除该条数据吗？">
                                                        <HeaderStyle Width="140" />
                                                        <ItemStyle HorizontalAlign="Justify" Width="140" />
                                                    </telerik:GridButtonColumn>
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
                            <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClientClick="return checkGridCellValueChanged();" OnClick="btnSave_Click" />
                            <asp:Button ID="btnSubmit" runat="server" Text="提交" CssClass="mws-button green" CausesValidation="true" OnClientClick="return checkGridCellValueChanged();" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnEntryStock" runat="server" Text="确认入库" CssClass="mws-button green" CausesValidation="true" OnClick="btnEntryStock_Click" />
                            <asp:Button ID="btnPrint" runat="server" Text="打印" CssClass="mws-button green" CausesValidation="true" OnClientClick="openPrintPage();return false;" Visible="false" />
                            <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Procures/StockInManagement.aspx');return false;" />
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
            gridStockInDetails: "<%= rgStockInDetails.ClientID %>",

        };

        function refreshGrid(gridClientID) {
            var gridObj = $find(gridClientID);

            if (gridObj)
                gridObj.get_masterTableView().rebind();
        }

        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/Procures/StockInManagement.aspx");
        }

        function refreshMaintenancePage(sender, args) {

            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            redirectToPage("Views/Procures/StockInMaintenance.aspx?EntityID=" + currentEntityID);
        }

        function openPrintPage() {

            var targetUrl = $.getRootPath() + "Views/Procures/Printers/PrintStockIn.aspx?EntityID=" + currentEntityID;

            window.open(targetUrl, "_blank");
        }

        function openChooseOrderProductWindow() {
            $.showLoading();

            var supplierID = $find("<%= rcbxSupplier.ClientID %>").get_value();

            if (supplierID && supplierID > 0) {
                var targetUrl = $.getRootPath() + "Views/Procures/Editors/ChooseProcureOrderProducts.aspx?SupplierID=" + supplierID
                + "&OwnerEntityID=" + currentEntityID + "&GridClientID=" + gridClientIDs.gridStockInDetails;

                $.openRadWindow(targetUrl, "winProductSpecification", true, 800, 600);
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

            if (colUniqueName.toLowerCase() === "inqty") {

                var tableView = args.get_tableView();

                var dataItemIndex = args.get_row().rowIndex - 1;

                var procureCount = parseInt(tableView.get_dataItems()[dataItemIndex].getDataKeyValue("ProcureCount"));
                var toBeInQty = parseInt(tableView.get_dataItems()[dataItemIndex].getDataKeyValue("ToBeInQty"));

                if (toBeInQty === 0) {
                    if (parseInt(args.get_editorValue()) > procureCount) {

                        var radNotification = $find("<%=radNotification.ClientID%>");

                        radNotification.set_text("基本数量必须小于采购数量：" + procureCount);
                        radNotification.show();

                        args.set_cancel(true);
                    }
                }
                else {
                    if (parseInt(args.get_editorValue()) > toBeInQty) {

                        var radNotification = $find("<%=radNotification.ClientID%>");

                        radNotification.set_text("基本数量必须小于未入库数量：" + toBeInQty);
                        radNotification.show();

                        args.set_cancel(true);
                    }
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

                case "InQty":
                    if (isNullOrEmptyEditValue)
                        $($telerik.findElement(args.get_cell(), "rfvInQty")).show();
                    else
                        $($telerik.findElement(args.get_cell(), "rfvInQty")).hide();
                    break;

                case "BatchNumber":
                    if (isNullOrEmptyEditValue)
                        $($telerik.findElement(args.get_cell(), "rfvBatchNumber")).show();
                    else
                        $($telerik.findElement(args.get_cell(), "rfvBatchNumber")).hide();
                    break;

                case "ExpirationDate":
                    if (isNullOrEmptyEditValue)
                        $($telerik.findElement(args.get_cell(), "rfvExpirationDate")).show();
                    else
                        $($telerik.findElement(args.get_cell(), "rfvExpirationDate")).hide();
                    break;

                case "LicenseNumber":
                    if (isNullOrEmptyEditValue)
                        $($telerik.findElement(args.get_cell(), "rfvLicenseNumber")).show();
                    else
                        $($telerik.findElement(args.get_cell(), "rfvLicenseNumber")).hide();
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
                    case "InQty":
                        editValue = $telerik.findControl(container, "txtInQty").get_value();
                        break;

                    case "BatchNumber":
                        editValue = $telerik.findControl(container, "txtBatchNumber").get_value();
                        break;

                    case "ExpirationDate":
                        editValue = $telerik.findControl(container, "rdpExpirationDate").get_selectedDate();
                        break;
                    case "LicenseNumber":
                        editValue = $telerik.findControl(container, "txtLicenseNumber").get_value();
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

        function rebuildAddNewCommands() {
            var addNewBtn = $("input[id$='_AddNewRecordButton']");
            var addNewLink = $("a[id$='_InitInsertButton']");

            if (addNewBtn.length > 0 && addNewLink.length > 0) {

                var commandParent = addNewBtn.parent();
                commandParent.prepend("<input type=\"button\" class=\"rgAdd\" title=\"添加\" onclick=\"openChooseOrderProductWindow(); return false;\" />"
                    + "<a href=\"javascript:void(0)\" title=\"添加\" onclick=\"openChooseOrderProductWindow(); return false;\">添加</a>");
                //删除必须放在获取parent对象后面
                addNewBtn.remove();
                addNewLink.remove();
            }
        }

        function onRequestEnd(sender, args)
        {
            rebuildAddNewCommands();
        }

        $(document).ready(function () {
            currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            rebuildAddNewCommands();
        });

    </script>
</asp:Content>
