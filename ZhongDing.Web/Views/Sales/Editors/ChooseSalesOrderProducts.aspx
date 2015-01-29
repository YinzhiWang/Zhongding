<%@ Page Title="选择待出库货品" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="ChooseSalesOrderProducts.aspx.cs" Inherits="ZhongDing.Web.Views.Sales.Editors.ChooseSalesOrderProducts" %>

<%@ MasterType VirtualPath="~/Site.Window.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rgSalesOrderAppDetails">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgSalesOrderAppDetails" />
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
                    <div class="mws-form-row" style="padding-top: 0px; padding-left: 0px; padding-right: 1px;">
                        <telerik:RadGrid ID="rgSalesOrderAppDetails" runat="server" PageSize="10"
                            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false" AllowMultiRowSelection="true"
                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" Height="460" ShowHeader="true" ShowFooter="true"
                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                            OnNeedDataSource="rgSalesOrderAppDetails_NeedDataSource" OnItemDataBound="rgSalesOrderAppDetails_ItemDataBound"
                            OnColumnCreated="rgSalesOrderAppDetails_ColumnCreated">
                            <MasterTableView Width="100%" DataKeyNames="ID,SalesOrderApplicationID,ProductID,ProductSpecificationID" CommandItemDisplay="None"
                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa" ClientDataKeyNames="ID,ProductID,ProductSpecificationID,ToBeOutQty">
                                <Columns>
                                    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderText="全选">
                                        <HeaderStyle Width="40" />
                                        <ItemStyle Width="40" />
                                    </telerik:GridClientSelectColumn>
                                    <telerik:GridBoundColumn UniqueName="OrderCode" HeaderText="订单编号" DataField="OrderCode" ReadOnly="true">
                                        <HeaderStyle Width="160" />
                                        <ItemStyle HorizontalAlign="Left" Width="160" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName" ReadOnly="true">
                                        <HeaderStyle Width="180" />
                                        <ItemStyle HorizontalAlign="Left" Width="180" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification" ReadOnly="true">
                                        <HeaderStyle Width="80" />
                                        <ItemStyle HorizontalAlign="Left" Width="80" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn UniqueName="Warehouse" HeaderText="仓库" DataField="Warehouse" SortExpression="Warehouse">
                                        <HeaderStyle Width="120" />
                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                        <ItemTemplate>
                                            <telerik:RadDropDownList runat="server" ID="ddlWarehouse" DefaultMessage="--请选择--" Width="100" OnItemDataBound="ddlWarehouse_ItemDataBound"
                                                OnClientItemSelecting="onClientSelectingWarehouse" OnClientSelectedIndexChanged="onClientSelectedWarehouse">
                                            </telerik:RadDropDownList>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn UniqueName="OutQty" HeaderText="已发数量" DataField="OutQty" ReadOnly="true">
                                        <HeaderStyle Width="80" />
                                        <ItemStyle HorizontalAlign="Left" Width="80" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="ToBeOutQty" HeaderText="未发数量" DataField="ToBeOutQty" ReadOnly="true">
                                        <HeaderStyle Width="80" />
                                        <ItemStyle HorizontalAlign="Left" Width="80" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn UniqueName="CurrentOutQty" HeaderText="本次发出数量" DataField="CurrentOutQty" SortExpression="CurrentOutQty">
                                        <HeaderStyle Width="100" />
                                        <ItemStyle HorizontalAlign="Left" Width="100" />
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox runat="server" ID="txtCurrentOutQty" Type="Number" MaxLength="9" Width="80" ShowSpinButtons="true"
                                                MinValue="1" MaxValue="99999999" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="">
                                                <ClientEvents OnValueChanging="onCurrentOutQtyChanging" OnValueChanged="onCurrentOutQtyChanged" />
                                            </telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="CurrentTaxQty" HeaderText="需开票数量" DataField="CurrentTaxQty" SortExpression="CurrentTaxQty">
                                        <HeaderStyle Width="100" />
                                        <ItemStyle HorizontalAlign="Left" Width="100" />
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox runat="server" ID="txtCurrentTaxQty" Type="Number" MaxLength="9" Width="80" ShowSpinButtons="true"
                                                MinValue="1" MaxValue="99999999" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="">
                                                <ClientEvents OnValueChanging="onCurrentTaxQtyChanging" />
                                            </telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="BalanceQty" HeaderText="库存数量" DataField="BalanceQty" SortExpression="BalanceQty" ReadOnly="true">
                                        <HeaderStyle Width="160" />
                                        <ItemStyle HorizontalAlign="Left" Width="160" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblBalanceQty" runat="server" Text='<%# Eval("BalanceQty") %>'></asp:Label>
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
                                <Selecting AllowRowSelect="True" />
                                <Scrolling AllowScroll="true" FrozenColumnsCount="4" SaveScrollPosition="true" UseStaticHeaders="true" />
                                <ClientEvents OnRowSelecting="onRowSelecting" />
                            </ClientSettings>
                            <HeaderStyle Width="99.8%" />
                        </telerik:RadGrid>
                    </div>
                    <div class="mws-button-row">
                        <asp:Button ID="btnSave" runat="server" Text="加入出库单" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
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

        function onClientSelectingWarehouse(sender, eventArgs) {
            //debugger;

            var gridItem = sender.get_parent();

            if (gridItem) {

                var selectingItem = eventArgs.get_item();

                var validBalanceQty = calculateValidInventory(gridItem, selectingItem);

                if (validBalanceQty <= 0) {
                    var radNotification = $find("<%=radNotification.ClientID%>");

                    radNotification.set_text("该货品在 " + selectingItem.get_text() + " 仓库中可用库存为0，不能出库");
                    radNotification.show();

                    eventArgs.set_cancel(true);
                }
            }
        }

        function onClientSelectedWarehouse(sender, eventArgs) {
            //debugger;

            var item = sender.get_selectedItem();

            var extensionAttr = item.get_attributes().getAttribute("Extension");

            if (extensionAttr) {
                var extensionData = JSON.parse(extensionAttr);

                var gridItem = sender.get_parent();

                if (gridItem) {

                    var gridItemElement = gridItem.get_element();

                    var lblBalanceQty = $($telerik.findElement(gridItemElement, "lblBalanceQty"));

                    if (lblBalanceQty)
                        lblBalanceQty.text(extensionData.BalanceQty);

                    var txtCurrentOutQty = $telerik.findControl(gridItemElement, "txtCurrentOutQty");

                    var toBeOutQty = parseInt(gridItem.getDataKeyValue("ToBeOutQty"));

                    if (toBeOutQty > extensionData.BalanceQty)
                        txtCurrentOutQty.set_maxValue(extensionData.BalanceQty);
                    else
                        txtCurrentOutQty.set_maxValue(toBeOutQty);

                    var currentOutQty = txtCurrentOutQty.get_value();

                    if (currentOutQty > extensionData.BalanceQty)
                        txtCurrentOutQty.set_value(extensionData.BalanceQty);

                    var txtCurrentTaxQty = $telerik.findControl(gridItemElement, "txtCurrentTaxQty");
                    txtCurrentTaxQty.set_value("");
                }
            }
        }

        function onCurrentOutQtyChanging(sender, eventArgs) {
            //debugger;

            var gridItem = sender.get_parent();

            if (gridItem) {

                var validBalanceQty = calculateValidInventory(gridItem);

                var newValue = eventArgs.get_newValue();

                if (newValue) {

                    var toBeOutQty = parseInt(gridItem.getDataKeyValue("ToBeOutQty"));

                    if (newValue > toBeOutQty) {

                        var radNotification = $find("<%=radNotification.ClientID%>");

                        radNotification.set_text("本次发出数量不能大于未发数量：" + toBeOutQty);
                        radNotification.show();

                        eventArgs.set_cancel(true);
                    }

                    if (newValue > validBalanceQty) {

                        var radNotification = $find("<%=radNotification.ClientID%>");

                        radNotification.set_text("本次发出数量不能大于可用库存数量：" + validBalanceQty);
                        radNotification.show();

                        sender.set_value(validBalanceQty);
                        sender.set_maxValue(validBalanceQty);
                    }
                }
                else {
                    var radNotification = $find("<%=radNotification.ClientID%>");

                    radNotification.set_text("本次发出数量为必填项");
                    radNotification.show();

                    eventArgs.set_cancel(true);
                }
            }
        }

        function onCurrentOutQtyChanged(sender, eventArgs) {
            debugger;

            var gridItem = sender.get_parent();

            if (gridItem) {

                var gridItemElement = gridItem.get_element();

                var newValue = eventArgs.get_newValue();

                if (newValue) {

                    var txtCurrentTaxQty = $telerik.findControl(gridItemElement, "txtCurrentTaxQty");
                    var currentTaxQty = txtCurrentTaxQty.get_value();

                    if (currentTaxQty > parseInt(newValue)) {
                        txtCurrentTaxQty.set_value(newValue);
                    }
                }
            }
        }

        function onCurrentTaxQtyChanging(sender, eventArgs) {
            var gridItem = sender.get_parent();

            if (gridItem) {

                var gridItemElement = gridItem.get_element();

                var newValue = eventArgs.get_newValue();

                if (newValue) {

                    var txtCurrentOutQty = $telerik.findControl(gridItemElement, "txtCurrentOutQty");
                    var currentOutQty = txtCurrentOutQty.get_value();

                    if (newValue > currentOutQty) {

                        var radNotification = $find("<%=radNotification.ClientID%>");

                        radNotification.set_text("需开票数量不能大于本次发出数量：" + currentOutQty);
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

            var txtCurrentOutQty = $telerik.findControl(selectingElement, "txtCurrentOutQty");
            var currentOutQty = txtCurrentOutQty.get_value();

            if (currentOutQty == null) {
                var radNotification = $find("<%=radNotification.ClientID%>");

                radNotification.set_text("本次发出数量为必填项");
                radNotification.show();

                eventArgs.set_cancel(true);
            }
            else {
                var totalBalanceQty = 0;

                var toBeOutQty = parseInt(selectingItem.getDataKeyValue("ToBeOutQty"));

                var productID = parseInt(selectingItem.getDataKeyValue("ProductID"));
                var productSpecificationID = parseInt(selectingItem.getDataKeyValue("ProductSpecificationID"));
                var warehouseID = 0;

                var ddlWarehouse = $telerik.findControl(selectingElement, "ddlWarehouse");
                var selectedWarehouseItem = ddlWarehouse.get_selectedItem();
                var warehouseName = "";
                if (selectedWarehouseItem) {
                    warehouseID = selectedWarehouseItem.get_value();
                    warehouseName = selectedWarehouseItem.get_text();

                    var extensionAttr = selectedWarehouseItem.get_attributes().getAttribute("Extension");
                    if (extensionAttr) {
                        var extensionData = JSON.parse(extensionAttr);
                        totalBalanceQty = extensionData.BalanceQty
                    }
                }

                var selectedTotalOutQty = 0;

                //获取已经选中的items
                var selectedItems = eventArgs.get_tableView().get_selectedItems();

                for (var i = 0; i < selectedItems.length; i++) {
                    var curSelectedItem = selectedItems[i];
                    var curSelectedItemElement = curSelectedItem.get_element();

                    var curProductID = parseInt(curSelectedItem.getDataKeyValue("ProductID"));
                    var curProductSpecificationID = parseInt(curSelectedItem.getDataKeyValue("ProductSpecificationID"));
                    var curWarehouseID = 0;

                    var curWarehouseControl = $telerik.findControl(curSelectedItemElement, "ddlWarehouse");
                    var curSelectedWarehouseItem = curWarehouseControl.get_selectedItem();
                    if (curSelectedWarehouseItem) {
                        curWarehouseID = curSelectedWarehouseItem.get_value();
                    }

                    if (curProductID === productID && curWarehouseID === warehouseID
                        && curProductSpecificationID === productSpecificationID) {

                        var curCurrentOutQtyControl = $telerik.findControl(curSelectedItemElement, "txtCurrentOutQty");
                        var curCurrentOutQty = curCurrentOutQtyControl.get_value();

                        if (curCurrentOutQty) {
                            selectedTotalOutQty += parseInt(curCurrentOutQty);
                        }
                    }
                }

                var curBalanceQty = totalBalanceQty - selectedTotalOutQty;

                if (curBalanceQty == 0) {
                    var radNotification = $find("<%=radNotification.ClientID%>");

                    radNotification.set_text("该货品在 " + warehouseName + " 仓库中可用库存为0，不能出库");
                    radNotification.show();

                    eventArgs.set_cancel(true);
                }
                else {
                    if (currentOutQty > curBalanceQty) {
                        txtCurrentOutQty.set_value(curBalanceQty);
                        txtCurrentOutQty.set_maxValue(curBalanceQty);
                    }
                    else {
                        if (curBalanceQty > toBeOutQty) {

                            if (currentOutQty > toBeOutQty)
                                txtCurrentOutQty.set_value(toBeOutQty);

                            txtCurrentOutQty.set_maxValue(toBeOutQty);
                        }
                        else {

                            if (currentOutQty > curBalanceQty)
                                txtCurrentOutQty.set_value(curBalanceQty);

                            txtCurrentOutQty.set_maxValue(curBalanceQty);
                        }
                    }
                }
            }
        }

        function calculateValidInventory(gridItem, selectingWarehouseItem) {

            var gridItemElement = gridItem.get_element();

            var totalBalanceQty = 0;

            var id = parseInt(gridItem.getDataKeyValue("ID"));
            var toBeOutQty = parseInt(gridItem.getDataKeyValue("ToBeOutQty"));
            var productID = parseInt(gridItem.getDataKeyValue("ProductID"));
            var productSpecificationID = parseInt(gridItem.getDataKeyValue("ProductSpecificationID"));
            var warehouseID = 0;
            var currentOutQty = 0;

            var warehouseItem = null;

            if (selectingWarehouseItem) {
                warehouseItem = selectingWarehouseItem;
            }
            else {
                var ddlWarehouse = $telerik.findControl(gridItemElement, "ddlWarehouse");
                var selectedWarehouseItem = ddlWarehouse.get_selectedItem();
                if (selectedWarehouseItem) {
                    warehouseItem = selectedWarehouseItem;
                }
            }

            if (warehouseItem) {
                warehouseID = warehouseItem.get_value();
                var extensionAttr = warehouseItem.get_attributes().getAttribute("Extension");
                if (extensionAttr) {
                    var extensionData = JSON.parse(extensionAttr);
                    totalBalanceQty = extensionData.BalanceQty
                }
            }

            var txtCurrentOutQty = $telerik.findControl(gridItemElement, "txtCurrentOutQty");
            var tempCurrentOutQty = txtCurrentOutQty.get_value();
            if (tempCurrentOutQty) {
                currentOutQty = parseInt(tempCurrentOutQty);
            }

            var selectedTotalOutQty = 0;

            //获取已经选中的items
            var selectedItems = gridItem.get_owner().get_selectedItems();

            for (var i = 0; i < selectedItems.length; i++) {
                var curSelectedItem = selectedItems[i];
                var curSelectedItemElement = curSelectedItem.get_element();

                var curId = parseInt(curSelectedItem.getDataKeyValue("ID"));
                var curProductID = parseInt(curSelectedItem.getDataKeyValue("ProductID"));
                var curProductSpecificationID = parseInt(curSelectedItem.getDataKeyValue("ProductSpecificationID"));
                var curWarehouseID = 0;

                var curWarehouseControl = $telerik.findControl(curSelectedItemElement, "ddlWarehouse");
                var curSelectedWarehouseItem = curWarehouseControl.get_selectedItem();
                if (curSelectedWarehouseItem) {
                    curWarehouseID = curSelectedWarehouseItem.get_value();
                }

                if (curProductID === productID && curWarehouseID === warehouseID
                    && curProductSpecificationID === productSpecificationID && curId != id) {

                    var curCurrentOutQtyControl = $telerik.findControl(curSelectedItemElement, "txtCurrentOutQty");
                    var curCurrentOutQty = curCurrentOutQtyControl.get_value();

                    if (curCurrentOutQty) {
                        selectedTotalOutQty += parseInt(curCurrentOutQty);
                    }
                }
            }

            return totalBalanceQty - selectedTotalOutQty;
        }

    </script>
</asp:Content>

