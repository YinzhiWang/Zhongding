<%@ Page Title="每月现金流量情况表-详情" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CashFlowReportDetail.aspx.cs" Inherits="ZhongDing.Web.Views.Reports.CashFlowReportDetail" %>

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
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <!-- Main Container -->
    <div class="container">
        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-table-1" id="lblTitle" runat="server">每月现金流量情况表-详情</span>
            </div>
            <div class="mws-panel-body">

                <telerik:RadGrid ID="rgCashFlowReports" runat="server" PageSize="1000"
                    AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                    MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                    ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                    OnNeedDataSource="rgCashFlowReports_NeedDataSource" OnDeleteCommand="rgCashFlowReports_DeleteCommand"
                    OnItemCreated="rgCashFlowReports_ItemCreated" OnColumnCreated="rgCashFlowReports_ColumnCreated" OnItemDataBound="rgCashFlowReports_ItemDataBound">
                    <MasterTableView Width="100%" CommandItemDisplay="Top" ShowHeader="false"
                        ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                        <Columns>
                            <telerik:GridBoundColumn UniqueName="FirstColName" HeaderText="FirstColName" DataField="FirstColName">
                                <ItemStyle Width="260" />
                                <HeaderStyle Width="260" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="Month1" HeaderText="Month1" DataField="Month1">
                                <ItemStyle Width="100" />
                                <HeaderStyle Width="100" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="Month2" HeaderText="Month2" DataField="Month2">
                                <ItemStyle Width="100" />
                                <HeaderStyle Width="100" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="Month3" HeaderText="Month3" DataField="Month3">
                                <ItemStyle Width="100" />
                                <HeaderStyle Width="100" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="Month4" HeaderText="Month4" DataField="Month4">
                                <ItemStyle Width="100" />
                                <HeaderStyle Width="100" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="Month5" HeaderText="Month5" DataField="Month5">
                                <ItemStyle Width="100" />
                                <HeaderStyle Width="100" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="Month6" HeaderText="Month6" DataField="Month6">
                                <ItemStyle Width="100" />
                                <HeaderStyle Width="100" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="Month7" HeaderText="Month7" DataField="Month7">
                                <ItemStyle Width="100" />
                                <HeaderStyle Width="100" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="Month8" HeaderText="Month8" DataField="Month8">
                                <ItemStyle Width="100" />
                                <HeaderStyle Width="100" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="Month9" HeaderText="Month9" DataField="Month9">
                                <ItemStyle Width="100" />
                                <HeaderStyle Width="100" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="Month10" HeaderText="Month10" DataField="Month10">
                                <ItemStyle Width="100" />
                                <HeaderStyle Width="100" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="Month11" HeaderText="Month11" DataField="Month11">
                                <ItemStyle Width="100" />
                                <HeaderStyle Width="100" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="Month12" HeaderText="Month12" DataField="Month12">
                                <ItemStyle Width="100" />
                                <HeaderStyle Width="100" />
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
                        <Scrolling  AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
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
