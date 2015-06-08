<%@ Page Title="大包客户结算表" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DBClientSettlementReportManagement.aspx.cs" Inherits="ZhongDing.Web.Views.Reports.DBClientSettlementReportManagement" %>

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
                    <telerik:AjaxUpdatedControl ControlID="rgProcureOrderReports" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgProcureOrderReports" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgProcureOrderReports">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgProcureOrderReports" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <!-- Main Container -->
    <div class="container">
        <div class="mws-panel grid_8">
            <div class="mws-panel-header">

                <span class="mws-i-24 i-table-1" id="lblTitle" runat="server">大包客户结算表</span>
            </div>
            <div class="mws-panel-body">

                <table runat="server" id="tblSearch" class="leftmargin10" style="margin-top: 10px;">
                    <tr class="height40">
                        <th class="width100 middle-td">年份月份：</th>
                        <td class="middle-td" colspan="4">
                            <telerik:RadMonthYearPicker runat="server" ID="rmypSettlementDate" Width="120"
                                EnableShadows="true"
                                MonthYearNavigationSettings-CancelButtonCaption="取消"
                                MonthYearNavigationSettings-OkButtonCaption="确定"
                                MonthYearNavigationSettings-TodayButtonCaption="今天"
                                MonthYearNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                MonthYearNavigationSettings-EnableScreenBoundaryDetection="true">
                            </telerik:RadMonthYearPicker>
                        </td>
                        <th class="width120 middle-td">客户名称：</th>
                        <td class="middle-td width280">
                            <telerik:RadComboBox runat="server" ID="rcbxClientUser" Filter="Contains"
                                AllowCustomText="false" Width="260px" EmptyMessage="--请选择--"
                                AutoPostBack="true">
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                              <asp:Button ID="btnExport" runat="server" Text="导出" CssClass="mws-button green" OnClientClick="exportExcel();return false;" />
                        </td>
                        <td></td>
                    </tr>

                </table>
                <telerik:RadGrid ID="rgProcureOrderReports" runat="server" PageSize="10"
                    AllowCustomPaging="true"
                    AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                    MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true" 
                    ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                    OnNeedDataSource="rgProcureOrderReports_NeedDataSource" OnDeleteCommand="rgProcureOrderReports_DeleteCommand"
                    OnItemCreated="rgProcureOrderReports_ItemCreated" OnColumnCreated="rgProcureOrderReports_ColumnCreated" OnItemDataBound="rgProcureOrderReports_ItemDataBound">
                    <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                        ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                        <ColumnGroups>
                        </ColumnGroups>
                        <Columns>
                            <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ClientUserName" HeaderText="客户" DataField="ClientUserName" 
                               >
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <%--   <telerik:GridBoundColumn UniqueName="Hospitals" HeaderText="医院" DataField="Hospitals">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>--%>
                            <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName">
                                <ItemStyle HorizontalAlign="Left" Width="60" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="HospitalType" HeaderText="客户性质" DataField="HospitalType">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="PromotionExpense" HeaderText="推广价" DataField="PromotionExpense" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="SaleQty" HeaderText="数量" DataField="SaleQty">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="BonusAmount" HeaderText="已结算业务提成" DataField="BonusAmount" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Left" />
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
                        <ClientEvents />
                        <Selecting AllowRowSelect="True" />
                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                    </ClientSettings>
                    <HeaderStyle Width="100%" />
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
            var ProcureOrderReportType = $.getQueryString("ProcureOrderReportType");
            window.location.href = "ProcureOrderReportMaintenance.aspx?ProcureOrderReportType=" + ProcureOrderReportType + "&EntityID=" + id;
        }
        function exportExcel() {
            $("#<%=btnExportHidden.ClientID%>").click();
        }
    </script>
</asp:Content>
