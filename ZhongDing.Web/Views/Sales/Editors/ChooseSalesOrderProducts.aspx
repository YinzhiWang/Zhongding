﻿<%@ Page Title="选择待出库货品" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="ChooseSalesOrderProducts.aspx.cs" Inherits="ZhongDing.Web.Views.Sales.Editors.ChooseSalesOrderProducts" %>

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
            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgSalesOrderAppDetails" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgSalesOrderAppDetails" LoadingPanelID="loadingPanel" />
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
                    <div class="mws-form-row" style="padding-top: 0px; padding-left: 0px;">
                        <table runat="server" id="tblSearch" class="leftmargin10 hide">
                            <tr class="height40">
                                <th class="width100">仓库：</th>
                                <td>
                                    <telerik:RadComboBox runat="server" ID="rcbxWarehouse" Filter="Contains"
                                        AllowCustomText="false" Height="160px" EmptyMessage="--请选择--">
                                    </telerik:RadComboBox>
                                </td>
                                <td class="leftpadding20">
                                    <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="mws-form-row" style="padding-top: 0px; padding-left: 0px; padding-right: 1px;">
                        <telerik:RadGrid ID="rgSalesOrderAppDetails" runat="server" PageSize="10"
                            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false" AllowMultiRowSelection="true"
                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" Height="460" ShowHeader="true" ShowFooter="true"
                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                            OnNeedDataSource="rgSalesOrderAppDetails_NeedDataSource" OnItemDataBound="rgSalesOrderAppDetails_ItemDataBound">
                            <MasterTableView Width="100%" DataKeyNames="ID,SalesOrderApplicationID,ProductID,ProductSpecificationID" CommandItemDisplay="None"
                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa" ClientDataKeyNames="ID,ToBeOutQty">
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
                                                OnClientSelectedIndexChanged="onClientSelectedWarehouse">
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
                                                <ClientEvents OnValueChanging="onCurrentOutQtyChanging" />
                                            </telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="BalanceQty" HeaderText="库存数量" DataField="BalanceQty" SortExpression="BalanceQty" ReadOnly="true">
                                        <HeaderStyle Width="80" />
                                        <ItemStyle HorizontalAlign="Left" Width="80" />
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

        function onClientSelectedWarehouse(sender, eventArgs) {
            //debugger;

            var item = sender.get_selectedItem();

            var extensionAttr = item.get_attributes().getAttribute("Extension");

            if (extensionAttr) {
                var extensionData = JSON.parse(extensionAttr);

                var gridRow = sender.get_parent();
                if (gridRow) {

                    var lblBalanceQty = $($telerik.findElement(gridRow.get_element(), "lblBalanceQty"));

                    if (lblBalanceQty)
                        lblBalanceQty.text(extensionData.BalanceQty);

                    var txtCurrentOutQty = $telerik.findControl(gridRow.get_element(), "txtCurrentOutQty");

                    var dataItemIndex = gridRow.get_itemIndex();
                    var toBeOutQty = parseInt(gridRow.get_owner().get_dataItems()[dataItemIndex].getDataKeyValue("ToBeOutQty"));

                    if (toBeOutQty > extensionData.BalanceQty)
                        txtCurrentOutQty.set_maxValue(extensionData.BalanceQty);
                    else
                        txtCurrentOutQty.set_maxValue(toBeOutQty);

                    var currentOutQty = txtCurrentOutQty.get_value();

                    if (currentOutQty > extensionData.BalanceQty)
                        txtCurrentOutQty.set_value(extensionData.BalanceQty);
                }
            }
        }

        function onCurrentOutQtyChanging(sender, eventArgs) {
            //debugger;

            var newValue = eventArgs.get_newValue();

            if (newValue) {

                var gridRow = sender.get_parent();

                if (gridRow) {

                    var dataItemIndex = gridRow.get_itemIndex();
                    var toBeOutQty = parseInt(gridRow.get_owner().get_dataItems()[dataItemIndex].getDataKeyValue("ToBeOutQty"));

                    if (newValue > toBeOutQty) {

                        var radNotification = $find("<%=radNotification.ClientID%>");

                        radNotification.set_text("本次发出数量不能大于未发数量：" + toBeOutQty);
                        radNotification.show();

                        eventArgs.set_cancel(true);
                    }

                    var ddlWarehouse = $telerik.findControl(gridRow.get_element(), "ddlWarehouse");

                    var selectedItem = ddlWarehouse.get_selectedItem();

                    if (selectedItem) {

                        var extensionAttr = selectedItem.get_attributes().getAttribute("Extension");

                        if (extensionAttr) {

                            var extensionData = JSON.parse(extensionAttr);

                            if (newValue > extensionData.BalanceQty) {

                                var radNotification = $find("<%=radNotification.ClientID%>");

                                radNotification.set_text("本次发出数量必须小于库存数量：" + extensionData.BalanceQty);
                                radNotification.show();

                                eventArgs.set_cancel(true);
                            }
                        }
                    }
                }
            }
            else {
                var radNotification = $find("<%=radNotification.ClientID%>");

                radNotification.set_text("本次发出数量为必填项");
                radNotification.show();

                eventArgs.set_cancel(true);
            }
        }

        function onRowSelecting(sender, eventArgs) {

            var selectingItem = eventArgs.get_gridDataItem();
            var selectingElement = selectingItem.get_element();

            var ddlWarehouse = $telerik.findControl(selectingElement, "ddlWarehouse");
            var selectedWarehouseItem = ddlWarehouse.get_selectedItem();
            if (selectedWarehouseItem == null
                || selectedWarehouseItem == undefined) {

                var radNotification = $find("<%=radNotification.ClientID%>");

                radNotification.set_text("请选择仓库" + extensionData.BalanceQty);
                radNotification.show();

                eventArgs.set_cancel(true);

                return;
            }

            var txtCurrentOutQty = $telerik.findControl(selectingElement, "txtCurrentOutQty");
            var currentOutQty = txtCurrentOutQty.get_value();

            if (currentOutQty == null) {
                var radNotification = $find("<%=radNotification.ClientID%>");

                radNotification.set_text("本次发出数量为必填项" + extensionData.BalanceQty);
                radNotification.show();

                eventArgs.set_cancel(true);

                return;
            }
        }

    </script>
</asp:Content>

