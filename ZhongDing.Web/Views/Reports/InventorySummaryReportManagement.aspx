<%@ Page Title="库存汇总表" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InventorySummaryReportManagement.aspx.cs" Inherits="ZhongDing.Web.Views.Reports.InventorySummaryReportManagement" %>

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
                    <telerik:AjaxUpdatedControl ControlID="rgInventorySummaryReports" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgInventorySummaryReports" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgInventorySummaryReports">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgInventorySummaryReports" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <!-- Main Container -->
    <div class="container">
        <div class="mws-panel grid_8">
            <div class="mws-panel-header">

                <span class="mws-i-24 i-table-1" id="lblTitle" runat="server">库存汇总表</span>
            </div>
            <div class="mws-panel-body">
                <table runat="server" id="tblSearch" class="leftmargin10">
                    <tr class="height40">
                        <th class="width100 middle-td">起止日期：</th>
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

                        <th class="width60 middle-td">货品：</th>
                        <td class="middle-td width280-percent">
                            <telerik:RadComboBox runat="server" ID="rcbxProduct" Filter="Contains"
                                AllowCustomText="false" Width="260" EmptyMessage="--请选择--">
                            </telerik:RadComboBox>
                        </td>
                        <td class="middle-td leftpadding20">
                            <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                              <asp:Button ID="btnExport" runat="server" Text="导出" CssClass="mws-button green" OnClientClick="exportExcel();return false;" />

                        </td>

                    </tr>
                </table>
                <telerik:RadGrid ID="rgInventorySummaryReports" runat="server" PageSize="10" Height="480"
                    AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                    MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                    ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                    OnNeedDataSource="rgInventorySummaryReports_NeedDataSource" OnDeleteCommand="rgInventorySummaryReports_DeleteCommand"
                    OnItemCreated="rgInventorySummaryReports_ItemCreated" OnColumnCreated="rgInventorySummaryReports_ColumnCreated" OnItemDataBound="rgInventorySummaryReports_ItemDataBound">
                    <MasterTableView Width="100%" CommandItemDisplay="Top"
                        ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                        <ColumnGroups>
                            <telerik:GridColumnGroup Name="Pre" HeaderText="上期结存" HeaderStyle-Font-Size="Small"
                                HeaderStyle-HorizontalAlign="Center" />
                            <telerik:GridColumnGroup Name="CurrentIn" HeaderText="本期收入" HeaderStyle-Font-Size="Small"
                                HeaderStyle-HorizontalAlign="Center" />
                            <telerik:GridColumnGroup Name="CurrentOut" HeaderText="本期发出" HeaderStyle-Font-Size="Small"
                                HeaderStyle-HorizontalAlign="Center" />
                            <telerik:GridColumnGroup Name="Current" HeaderText="本期结存" HeaderStyle-Font-Size="Small"
                                HeaderStyle-HorizontalAlign="Center" />
                        </ColumnGroups>
                        <Columns>

                            <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false">
                                <ItemStyle HorizontalAlign="Left" Width="50" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ProductCode" HeaderText="货品编码" DataField="ProductCode">
                                <ItemStyle HorizontalAlign="Left" Width="120px" />
                                <HeaderStyle Width="120px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName">
                                <ItemStyle HorizontalAlign="Left" Width="200px" />
                                <HeaderStyle Width="200px" />
                                <ItemTemplate>
                                    <a href="javascript:void(0);" style="color: blue;" onclick="redirectToInventorySummaryDetailReportManagementPage(<%#
                                    "'"+DataBinder.Eval(Container.DataItem,"WarehouseID")+"','"+DataBinder.Eval(Container.DataItem,"ProductID")
                                    +"','"+DataBinder.Eval(Container.DataItem,"ProductSpecificationID")+"','"+DataBinder.Eval(Container.DataItem,"BatchNumber")
                                    +"','"+DataBinder.Eval(Container.DataItem,"LicenseNumber")+"','"+DataBinder.Eval(Container.DataItem,"ExpirationDate")+"'"
                                    %>)">
                                        <u><%#DataBinder.Eval(Container.DataItem,"ProductName")%></u></a>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification">
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                <HeaderStyle Width="100px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="UnitName" HeaderText="基本单位" DataField="UnitName">
                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                <HeaderStyle Width="80px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="BatchNumber" HeaderText="批号" DataField="BatchNumber">
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                <HeaderStyle Width="100px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ExpirationDate" HeaderText="有效期限" DataField="ExpirationDate" DataFormatString="{0:yyyy/MM/dd}">
                                <ItemStyle HorizontalAlign="Left" Width="120px" />
                                <HeaderStyle Width="120px" />
                            </telerik:GridBoundColumn>

                            <%-- Pre --%>
                            <telerik:GridBoundColumn UniqueName="PreBalanceQty" HeaderText="基本数量" DataField="PreBalanceQty" ColumnGroupName="Pre">
                                <ItemStyle HorizontalAlign="Left" Width="120px" />
                                <HeaderStyle Width="120px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="PreBalanceQtyPackages" HeaderText="件数" DataField="PreBalanceQtyPackages" ColumnGroupName="Pre" DataFormatString="{0:f2}">
                                <ItemStyle HorizontalAlign="Left" Width="120px" />
                                <HeaderStyle Width="120px" />
                            </telerik:GridBoundColumn>

                            <%-- CurrentIn --%>
                            <telerik:GridBoundColumn UniqueName="InQty" HeaderText="基本数量" DataField="InQty" ColumnGroupName="CurrentIn">
                                <ItemStyle HorizontalAlign="Left" Width="120px" />
                                <HeaderStyle Width="120px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="InQtyPackages" HeaderText="件数" DataField="InQtyPackages" ColumnGroupName="CurrentIn" DataFormatString="{0:f2}">
                                <ItemStyle HorizontalAlign="Left" Width="120px" />
                                <HeaderStyle Width="120px" />
                            </telerik:GridBoundColumn>

                            <%-- CurrentOut --%>
                            <telerik:GridBoundColumn UniqueName="OutQty" HeaderText="基本数量" DataField="OutQty" ColumnGroupName="CurrentOut">
                                <ItemStyle HorizontalAlign="Left" Width="120px" />
                                <HeaderStyle Width="120px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="OutQtyPackages" HeaderText="件数" DataField="OutQtyPackages" ColumnGroupName="CurrentOut" DataFormatString="{0:f2}">
                                <ItemStyle HorizontalAlign="Left" Width="120px" />
                                <HeaderStyle Width="120px" />
                            </telerik:GridBoundColumn>

                            <%-- Current --%>
                            <telerik:GridBoundColumn UniqueName="CurrentBalanceQty" HeaderText="基本数量" DataField="CurrentBalanceQty" ColumnGroupName="Current">
                                <ItemStyle HorizontalAlign="Left" Width="120px" />
                                <HeaderStyle Width="120px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="CurrentBalanceQtyPackages" HeaderText="件数" DataField="CurrentBalanceQtyPackages" ColumnGroupName="Current" DataFormatString="{0:f2}">
                                <ItemStyle HorizontalAlign="Left" Width="120px" />
                                <HeaderStyle Width="120px" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="Amount" HeaderText="金额" DataField="Amount" DataFormatString="￥{0:f2}" ColumnGroupName="Current">
                                <ItemStyle HorizontalAlign="Left" Width="120px" />
                                <HeaderStyle Width="120px" />
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
                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
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
        //WarehouseID, x.ProductID, x.ProductSpecificationID, x.BatchNumber, x.LicenseNumber, x.ExpirationDate
        function redirectToInventorySummaryDetailReportManagementPage(WarehouseID, ProductID, ProductSpecificationID, BatchNumber, LicenseNumber, ExpirationDate) {
            $.showLoading();

            var url = "InventorySummaryDetailReportManagement.aspx?"
                + "WarehouseID=" + WarehouseID
                + "&ProductID=" + ProductID
             + "&ProductSpecificationID=" + ProductSpecificationID
             + "&BatchNumber=" + encodeURI(BatchNumber)
             + "&LicenseNumber=" + encodeURI(LicenseNumber)
             + "&ExpirationDate=" + encodeURI(ExpirationDate);
            var beginDatePicker = $find("<%= rdpBeginDate.ClientID %>"); 
            var endDatePicker = $find("<%= rdpEndDate.ClientID %>"); 
            if (beginDatePicker.get_selectedDate())
            {
                url += "&BeginDate=" + encodeURI(beginDatePicker.get_selectedDate().format("yyyy/MM/dd"));
            }
            if (endDatePicker.get_selectedDate()) {
                url += "&EndDate=" + encodeURI(endDatePicker.get_selectedDate().format("yyyy/MM/dd"));
            }
            window.location.href = url;
        }
        //redirectToInventorySummaryDetailReportManagementPage('1','1','1','20141206','测试3454646','2017/1/23 0:00:00')
        function exportExcel() {
            $("#<%=btnExportHidden.ClientID%>").click();
        }
    </script>
</asp:Content>
