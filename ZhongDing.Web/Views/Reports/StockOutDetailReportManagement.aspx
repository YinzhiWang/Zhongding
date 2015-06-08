<%@ Page Title="出库明细报表" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StockOutDetailReportManagement.aspx.cs" Inherits="ZhongDing.Web.Views.Reports.StockOutDetailReportManagement" %>

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
                    <telerik:AjaxUpdatedControl ControlID="rgStockOutDetailReports" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgStockOutDetailReports" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgStockOutDetailReports">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgStockOutDetailReports" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <!-- Main Container -->
    <div class="container">
        <div class="mws-panel grid_8">
            <div class="mws-panel-header">

                <span class="mws-i-24 i-table-1" id="lblTitle" runat="server">出库明细报表</span>
            </div>
            <div class="mws-panel-body">
                <table runat="server" id="tblSearch" class="leftmargin10">
                    <tr class="height40">
                        <th class="width100 middle-td">出库日期：</th>
                        <td class="middle-td" colspan="3">
                            <telerik:RadDatePicker runat="server" ID="rdpBeginDate" Width="120"></telerik:RadDatePicker>
                            -&nbsp;&nbsp;
                            <telerik:RadDatePicker runat="server" ID="rdpEndDate" Width="120"></telerik:RadDatePicker>
                        </td>

                    </tr>
                    <tr class="height40">
                        <th class="width60 middle-td">客户名称：</th>
                        <td class="middle-td width280">
                            <telerik:RadComboBox runat="server" ID="rcbxClientUser" Filter="Contains"
                                AllowCustomText="false"  Width="260px"  EmptyMessage="--请选择--"
                                AutoPostBack="true" OnSelectedIndexChanged="rcbxClientUser_SelectedIndexChanged">
                            </telerik:RadComboBox>
                        </td>
                        <th class="width80 middle-td">商业单位：</th>
                        <td class="middle-td width280">
                            <telerik:RadComboBox runat="server" ID="rcbxClientCompany"   Width="260px" Filter="Contains"
                                EmptyMessage="--请选择--" AllowCustomText="false">
                            </telerik:RadComboBox>
                        </td>
                      

                    </tr>
                        <tr class="height40">
                      
                         <th class="width60 middle-td">货品：</th>
                        <td class="middle-td width280-percent">
                            <telerik:RadComboBox runat="server" ID="rcbxProduct" Filter="Contains"
                                AllowCustomText="false" Width="260" EmptyMessage="--请选择--">
                            </telerik:RadComboBox>
                        </td>
                        <td class="middle-td leftpadding20" colspan="2" >
                            <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                              <asp:Button ID="btnExport" runat="server" Text="导出" CssClass="mws-button green" OnClientClick="exportExcel();return false;" />

                        </td>

                    </tr>
                </table>
                <telerik:RadGrid ID="rgStockOutDetailReports" runat="server" PageSize="10"
                    Height="480"
                    AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                    MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                    ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                    OnNeedDataSource="rgStockOutDetailReports_NeedDataSource" OnDeleteCommand="rgStockOutDetailReports_DeleteCommand"
                    OnItemCreated="rgStockOutDetailReports_ItemCreated" OnColumnCreated="rgStockOutDetailReports_ColumnCreated" OnItemDataBound="rgStockOutDetailReports_ItemDataBound">
                    <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                        ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                         <ColumnGroups>
                            <telerik:GridColumnGroup Name="StockOutCount" HeaderText="出库数量"  HeaderStyle-Font-Size="Small"
                                HeaderStyle-HorizontalAlign="Center" />
                          
                        </ColumnGroups>
                        <Columns>
                            <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="OutDate" HeaderText="出库日期" DataField="OutDate" DataFormatString="{0:yyyy/MM/dd}">
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                <HeaderStyle Width="100px" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="StockOutCode" HeaderText="出库单号" DataField="StockOutCode">
                                <ItemStyle HorizontalAlign="Left" Width="180px" />
                                <HeaderStyle Width="180px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="SalesOrderApplicationOrderCode" HeaderText="销售订单号" DataField="SalesOrderApplicationOrderCode">
                                <ItemStyle HorizontalAlign="Left" Width="160px" />
                                <HeaderStyle Width="160px" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="ClientName" HeaderText="客户" DataField="ClientName">
                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                <HeaderStyle Width="80px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ClientCompanyName" HeaderText="商业单位" DataField="ClientCompanyName">
                                <ItemStyle HorizontalAlign="Left" Width="260px" />
                                <HeaderStyle Width="260px" />
                            </telerik:GridBoundColumn>




                            <telerik:GridBoundColumn UniqueName="WarehouseName" HeaderText="仓库" DataField="WarehouseName">
                                <ItemStyle HorizontalAlign="Left" Width="120px" />
                                <HeaderStyle Width="120px" />
                            </telerik:GridBoundColumn>


                            <telerik:GridBoundColumn UniqueName="ProductCode" HeaderText="货品编号" DataField="ProductCode">
                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                <HeaderStyle Width="80px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="CategoryName" HeaderText="货品类别" DataField="CategoryName">
                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                <HeaderStyle Width="80px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName">
                                <ItemStyle HorizontalAlign="Left" Width="200px" />
                                <HeaderStyle Width="200px" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification">
                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                <HeaderStyle Width="80px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="UnitName" HeaderText="基本单位" DataField="UnitName">
                                <ItemStyle HorizontalAlign="Left" Width="60px" />
                                <HeaderStyle Width="60px" />
                            </telerik:GridBoundColumn>

                          
                            <telerik:GridBoundColumn UniqueName="OutQty" HeaderText="出库数量" DataField="OutQty"  ColumnGroupName="StockOutCount">
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                <HeaderStyle Width="100px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="NumberOfPackages" HeaderText="件数" DataField="NumberOfPackages"  ColumnGroupName="StockOutCount">
                                <ItemStyle HorizontalAlign="Left" Width="60px" />
                                <HeaderStyle Width="60px" />
                            </telerik:GridBoundColumn>

                             <telerik:GridBoundColumn UniqueName="BatchNumber" HeaderText="批号" DataField="BatchNumber">
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                <HeaderStyle Width="100px" />
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn UniqueName="ExpirationDate" HeaderText="有效期" DataField="ExpirationDate" DataFormatString="{0:yyyy/MM/dd}" >
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                <HeaderStyle Width="100px" />
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn UniqueName="ProcurePrice" HeaderText="采购单价" DataField="ProcurePrice" DataFormatString="￥{0:f2}">
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                <HeaderStyle Width="100px" />
                            </telerik:GridBoundColumn>
                            
                              <telerik:GridBoundColumn UniqueName="SalesPrice" HeaderText="销售单价" DataField="SalesPrice" DataFormatString="￥{0:f2}">
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                <HeaderStyle Width="100px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="TotalSalesAmount" HeaderText="销售货款" DataField="TotalSalesAmount" DataFormatString="￥{0:f2}">
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                <HeaderStyle Width="100px" />
                            </telerik:GridBoundColumn>


                        </Columns>
                        <CommandItemTemplate>
                            <table class="width100-percent">
                                <tr>
                                    <td>
                                        <%--<asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
                                            <input type="button" class="rgAdd" onclick="redirectToMaintenancePage(-1); return false;" />
                                            <a href="javascript:void(0)" onclick="redirectToMaintenancePage(-1); return false;">添加</a>
                                        </asp:Panel>--%>
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
                        <Scrolling AllowScroll="true"  SaveScrollPosition="true" UseStaticHeaders="true" />
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
            var StockOutDetailReportType = $.getQueryString("StockOutDetailReportType");
            window.location.href = "StockOutDetailReportMaintenance.aspx?StockOutDetailReportType=" + StockOutDetailReportType + "&EntityID=" + id;
        }
        function exportExcel() {
            $("#<%=btnExportHidden.ClientID%>").click();
        }
    </script>
</asp:Content>
