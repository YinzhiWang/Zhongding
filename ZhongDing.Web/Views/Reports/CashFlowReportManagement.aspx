<%@ Page Title="每月现金流量情况表" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CashFlowReportManagement.aspx.cs" Inherits="ZhongDing.Web.Views.Reports.CashFlowReportManagement" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <%--<telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgCashFlowReports" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgCashFlowReports" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgCashFlowReports">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgCashFlowReports" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <!-- Main Container -->
    <div class="container">
        <div class="mws-panel grid_8">
            <div class="mws-panel-header">

                <span class="mws-i-24 i-table-1" id="lblTitle" runat="server">每月现金流量情况表</span>
            </div>
            <div class="mws-panel-body">
                <table runat="server" id="tblSearch" class="leftmargin10">
                    <tr class="height40">
                        <th class="width100 middle-td">年月日期：</th>
                        <td class="middle-td" colspan="2">
                            <table>
                                <tr>
                                    <td>
                                        <telerik:RadMonthYearPicker runat="server" ID="rdpBeginDate" Width="120"
                                            EnableShadows="true"
                                            MonthYearNavigationSettings-CancelButtonCaption="取消"
                                            MonthYearNavigationSettings-OkButtonCaption="确定"
                                            MonthYearNavigationSettings-TodayButtonCaption="今天"
                                            MonthYearNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                            MonthYearNavigationSettings-EnableScreenBoundaryDetection="true">
                                        </telerik:RadMonthYearPicker>
                                    </td>
                                    <td>-&nbsp;&nbsp;</td>
                                    <td>
                                        <telerik:RadMonthYearPicker runat="server" ID="rdpEndDate" Width="120"
                                            EnableShadows="true"
                                            MonthYearNavigationSettings-CancelButtonCaption="取消"
                                            MonthYearNavigationSettings-OkButtonCaption="确定"
                                            MonthYearNavigationSettings-TodayButtonCaption="今天"
                                            MonthYearNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                            MonthYearNavigationSettings-EnableScreenBoundaryDetection="true">
                                        </telerik:RadMonthYearPicker>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>

                            <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />

                        </td>
                        <td></td>
                        <td></td>
                    </tr>

                </table>
                <telerik:RadGrid ID="rgCashFlowReports" runat="server" PageSize="10"
                    AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                    MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                    ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                    OnNeedDataSource="rgCashFlowReports_NeedDataSource" OnDeleteCommand="rgCashFlowReports_DeleteCommand"
                    OnItemCreated="rgCashFlowReports_ItemCreated" OnColumnCreated="rgCashFlowReports_ColumnCreated"
                     OnItemDataBound="rgCashFlowReports_ItemDataBound"
                     OnItemCommand="rgCashFlowReports_ItemCommand">
                    <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                        ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                        <Columns>
                            <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false">
                                <ItemStyle HorizontalAlign="Left" Width="120" />
                                <HeaderStyle Width="120" HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="CashFlowDate" HeaderText="报表名称" DataField="CashFlowDate">
                                <ItemStyle HorizontalAlign="Left" Width="120" />
                                <HeaderStyle Width="120" />
                                <ItemTemplate>
                                    <a target="_blank" href='<%# Page.ResolveUrl("~/Views/Reports/CashFlowReportDetail.aspx?EntityID="+DataBinder.Eval(Container.DataItem,"ID").ToString())%>'>
                                        <%# DateTime.Parse(DataBinder.Eval(Container.DataItem,"CashFlowDate").ToString()).ToString("yyyy/MM")%></a>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn UniqueName="CashFlowFileName" HeaderText="报表名称" DataField="CashFlowFileName">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn HeaderText="下载" Text="下载" UniqueName="Download" CommandName="Download" ButtonType="LinkButton"
                                HeaderStyle-Width="60" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />



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
                        <Scrolling AllowScroll="true" FrozenColumnsCount="4" SaveScrollPosition="true" UseStaticHeaders="true" />
                    </ClientSettings>

                </telerik:RadGrid>
            </div>
        </div>
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

        function redirectToProcureOrderMaintenancePage(id) {
            //$.showLoading();

            window.open($.getRootPath() + "/Views/Procures/ProcureOrderMaintenance.aspx?EntityID=" + id, "_blank");
        }
    </script>
</asp:Content>
