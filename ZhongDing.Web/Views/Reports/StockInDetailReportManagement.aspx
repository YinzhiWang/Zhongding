<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StockInDetailReportManagement.aspx.cs" Inherits="ZhongDing.Web.Views.Reports.StockInDetailReportManagement" %>

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
                    <telerik:AjaxUpdatedControl ControlID="rgStockInDetailReports" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgStockInDetailReports" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgStockInDetailReports">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgStockInDetailReports" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <!-- Main Container -->
    <div class="container">
        <div class="mws-panel grid_8">
            <div class="mws-panel-header">

                <span class="mws-i-24 i-table-1" id="lblTitle" runat="server">入库明细报表</span>
            </div>
            <div class="mws-panel-body">
                <table runat="server" id="tblSearch" class="leftmargin10">
                    <tr class="height40">
                        <th class="width100 middle-td">订单日期：</th>
                        <td class="middle-td" colspan="4">
                            <telerik:RadDatePicker runat="server" ID="rdpBeginDate" Width="120"></telerik:RadDatePicker>
                            -&nbsp;&nbsp;
                            <telerik:RadDatePicker runat="server" ID="rdpEndDate" Width="120"></telerik:RadDatePicker>
                        </td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr class="height40">
                        <th class="width60 middle-td">供应商：</th>
                        <td class="middle-td width280">
                            <telerik:RadComboBox runat="server" ID="rcbxSupplier" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="rcbxSupplier_SelectedIndexChanged"
                                Width="260" EmptyMessage="--请选择--">
                            </telerik:RadComboBox>
                        </td>
                        <th class="width60 middle-td">货品：</th>
                        <td class="middle-td width280-percent">
                            <telerik:RadComboBox runat="server" ID="rcbxProduct" Filter="Contains"
                                AllowCustomText="false" Width="260" EmptyMessage="--请选择--">
                            </telerik:RadComboBox>
                        </td>
                        <td class="middle-td leftpadding20"></td>

                    </tr>
                    <tr class="height40">
                        <th class="width60 middle-td">货品批号：</th>
                        <td class="middle-td width280">

                            <telerik:RadTextBox runat="server" ID="txtBatchNumber" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>

                        </td>

                        <td class="middle-td leftpadding20" colspan="3">
                            <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                              <asp:Button ID="btnExport" runat="server" Text="导出" CssClass="mws-button green" OnClientClick="exportExcel();return false;" />

                        </td>

                    </tr>
                </table>
                <telerik:RadGrid ID="rgStockInDetailReports" runat="server" PageSize="10"
                    Height="460"
                    AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                    MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                    ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                    OnNeedDataSource="rgStockInDetailReports_NeedDataSource" OnDeleteCommand="rgStockInDetailReports_DeleteCommand"
                    OnItemCreated="rgStockInDetailReports_ItemCreated" OnColumnCreated="rgStockInDetailReports_ColumnCreated" OnItemDataBound="rgStockInDetailReports_ItemDataBound">
                    <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                        ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                        <ColumnGroups>
                            <telerik:GridColumnGroup Name="StockInCount" HeaderText="入库数量"  HeaderStyle-Font-Size="Small"
                                HeaderStyle-HorizontalAlign="Center" />
                          
                        </ColumnGroups>
                        <Columns>
                            <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="EntryDate" HeaderText="入库日期" DataField="EntryDate" DataFormatString="{0:yyyy/MM/dd}">
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                <HeaderStyle Width="100px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="StockInCode" HeaderText="入库单号" DataField="StockInCode">
                                <ItemStyle HorizontalAlign="Left" Width="160px" />
                                <HeaderStyle Width="160px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="OrderCode" HeaderText="订单号" DataField="OrderCode">
                                <ItemStyle HorizontalAlign="Left" Width="160px" />
                                <HeaderStyle Width="160px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="SupplierName" HeaderText="供应商" DataField="SupplierName">
                                <ItemStyle HorizontalAlign="Left" Width="200px" />
                                <HeaderStyle Width="200px" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="WarehouseName" HeaderText="仓库" DataField="WarehouseName">
                                <ItemStyle HorizontalAlign="Left" Width="200px" />
                                <HeaderStyle Width="200px" />
                            </telerik:GridBoundColumn>


                            <telerik:GridBoundColumn UniqueName="ProductCode" HeaderText="货品编号" DataField="ProductCode">
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                <HeaderStyle Width="100px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="CategoryName" HeaderText="货品类别" DataField="CategoryName">
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                <HeaderStyle Width="100px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName">
                                <ItemStyle HorizontalAlign="Left" Width="200px" />
                                <HeaderStyle Width="200px" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification">
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                <HeaderStyle Width="100px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="UnitName" HeaderText="基本单位" DataField="UnitName">
                                <ItemStyle HorizontalAlign="Left" Width="60px" />
                                <HeaderStyle Width="60px" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="ProcurePrice" HeaderText="采购单价" DataField="ProcurePrice" DataFormatString="￥{0:f2}">
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                <HeaderStyle Width="100px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="InQty" HeaderText="数量" DataField="InQty" ColumnGroupName="StockInCount">
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                <HeaderStyle Width="100px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="NumberOfPackages" HeaderText="件数" DataField="NumberOfPackages" ColumnGroupName="StockInCount">
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                <HeaderStyle Width="100px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="BatchNumber" HeaderText="批号" DataField="BatchNumber">
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                <HeaderStyle Width="100px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ExpirationDate" HeaderText="有效期" DataField="ExpirationDate" DataFormatString="{0:yyyy/MM/dd}">
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                <HeaderStyle Width="100px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="TotalAmount" HeaderText="进货货款" DataField="TotalAmount" DataFormatString="￥{0:f2}">
                                <ItemStyle HorizontalAlign="Left" Width="160px" />
                                <HeaderStyle Width="160px" />
                            </telerik:GridBoundColumn>



                        </Columns>
                        <CommandItemTemplate>
                            <table class="width100-percent">
                                <tr>
                                    <td>
                                        <asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
                                            <input type="button" class="rgAdd" onclick="redirectToMaintenancePage(-1); return false;" />
                                            <a href="javascript:void(0)" onclick="redirectToMaintenancePage(-1); return false;">添加</a>
                                        </asp:Panel>
                                        <%--<asp:Panel ID="plExportCommand" runat="server" CssClass="width80 float-left">
                                            <input type="button" class="rgExpXLS" onclick="exportExcel(); return false;" />
                                            <a href="javascript:void(0);" onclick="exportExcel(); return false;">导出excel</a>
                                        </asp:Panel>--%>
                                    </td>
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
                        <PagerStyle PagerTextFormat="{4} 第{0}页/共{1}页, 第{2}-{3}条 共{5}条"
                            PageSizeControlType="RadComboBox" PageSizeLabelText="每页条数:"
                            FirstPageToolTip="第一页" PrevPageToolTip="上一页" NextPageToolTip="下一页" LastPageToolTip="最后一页" />
                    </MasterTableView>

                    <ClientSettings EnableRowHoverStyle="true">
                        <ClientEvents OnGridCreated="GetsGridObject" />
                        <Selecting AllowRowSelect="True" />
                        <Scrolling AllowScroll="true" FrozenColumnsCount="4" SaveScrollPosition="true" UseStaticHeaders="true" />
                    </ClientSettings>

                </telerik:RadGrid>
            </div>
        </div>
    </div>
    <div style="display: none;">
        <asp:Button ID="btnExportHidden" runat="server" Text="导出" CssClass="mws-button green" OnClick="btnExport_Click" />
    </div>
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

        function redirectToMaintenancePage(id) {
            $.showLoading();
            var StockInDetailReportType = $.getQueryString("StockInDetailReportType");
            window.location.href = "StockInDetailReportMaintenance.aspx?StockInDetailReportType=" + StockInDetailReportType + "&EntityID=" + id;
        }
        function exportExcel() {
            $("#<%=btnExportHidden.ClientID%>").click();
        }
    </script>
</asp:Content>
