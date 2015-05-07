<%@ Page Title="选择待入库货品" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="ChooseProcureOrderProducts.aspx.cs" Inherits="ZhongDing.Web.Views.Procures.Editors.ChooseProcureOrderProducts" %>

<%@ MasterType VirtualPath="~/Site.Window.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rgSupplierProcureOrderDetails">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgSupplierProcureOrderDetails" />
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
                    <div class="mws-form-row" style="padding-top:0px; padding-left: 0px; padding-right: 1px;">
                        <telerik:RadGrid ID="rgSupplierProcureOrderDetails" runat="server" PageSize="10"
                            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false" AllowMultiRowSelection="true"
                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" Height="460" ShowHeader="true" ShowFooter="true"
                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                            OnNeedDataSource="rgSupplierProcureOrderDetails_NeedDataSource">
                            <MasterTableView Width="100%" DataKeyNames="ID,ProcureOrderAppID,ProductID,ProductSpecificationID,WarehouseID,NumberInLargePackage" CommandItemDisplay="None"
                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                <Columns>
                                    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderText="全选">
                                        <HeaderStyle Width="40" />
                                        <ItemStyle Width="40" />
                                    </telerik:GridClientSelectColumn>
                                    <telerik:GridBoundColumn UniqueName="OrderCode" HeaderText="采购订单编号" DataField="OrderCode">
                                        <HeaderStyle Width="160" />
                                        <ItemStyle HorizontalAlign="Left" Width="160" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="Warehouse" HeaderText="仓库" DataField="Warehouse">
                                        <HeaderStyle Width="120" />
                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName">
                                        <HeaderStyle Width="180" />
                                        <ItemStyle HorizontalAlign="Left" Width="180" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification">
                                        <HeaderStyle Width="100" />
                                        <ItemStyle HorizontalAlign="Left" Width="100" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="FactoryName" HeaderText="生产企业" DataField="FactoryName">
                                        <HeaderStyle Width="160" />
                                        <ItemStyle HorizontalAlign="Left" Width="160" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="UnitOfMeasurement" HeaderText="基本单位" DataField="UnitOfMeasurement">
                                        <HeaderStyle Width="80" />
                                        <ItemStyle HorizontalAlign="Left" Width="80" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="ProcureCount" HeaderText="基本数量" DataField="ProcureCount">
                                        <HeaderStyle Width="80" />
                                        <ItemStyle HorizontalAlign="Left" Width="80" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="ProcurePrice" HeaderText="单价" DataField="ProcurePrice">
                                        <HeaderStyle Width="80" />
                                        <ItemStyle HorizontalAlign="Left" Width="80" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="NumberOfPackages" HeaderText="件数" DataField="NumberOfPackages" DataFormatString="{0:N2}">
                                        <HeaderStyle Width="80" />
                                        <ItemStyle HorizontalAlign="Left" Width="80" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="LicenseNumber" HeaderText="批准文号" DataField="LicenseNumber">
                                        <HeaderStyle Width="160" />
                                        <ItemStyle HorizontalAlign="Left" Width="160" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="InQty" HeaderText="已入库数量" DataField="InQty">
                                        <HeaderStyle Width="100" />
                                        <ItemStyle HorizontalAlign="Left" Width="100" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="ToBeInQty" HeaderText="未入库数量" DataField="ToBeInQty">
                                        <HeaderStyle Width="100" />
                                        <ItemStyle HorizontalAlign="Left" Width="100" />
                                    </telerik:GridBoundColumn>
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
                            </ClientSettings>
                            <HeaderStyle Width="99.8%" />
                        </telerik:RadGrid>
                    </div>
                    <div class="mws-button-row">
                        <asp:Button ID="btnSave" runat="server" Text="增至入库单" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
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

    </script>
</asp:Content>
