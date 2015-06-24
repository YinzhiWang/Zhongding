<%@ Page Title="工资结算管理" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalarySettleManagement.aspx.cs" Inherits="ZhongDing.Web.Views.HRM.SalarySettleManagement" %>

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
                    <telerik:AjaxUpdatedControl ControlID="rgSalarySettles" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgSalarySettles" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgSalarySettles">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgSalarySettles" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <!-- Main Container -->
    <div class="container">
        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-table-1" id="lblTitle" runat="server">工资结算管理</span>
            </div>
            <div class="mws-panel-body">
                <table runat="server" id="tblSearch" class="leftmargin10">
                    <tr class="height40">
                        <th class="width100 middle-td">结算年月：</th>
                        <td class="middle-td" colspan="1">
                            <table>
                                <tr>
                                    <td> <telerik:RadMonthYearPicker runat="server" ID="rdpBeginDate" Width="120"
                                EnableShadows="true"
                                MonthYearNavigationSettings-CancelButtonCaption="取消"
                                MonthYearNavigationSettings-OkButtonCaption="确定"
                                MonthYearNavigationSettings-TodayButtonCaption="今天"
                                MonthYearNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                MonthYearNavigationSettings-EnableScreenBoundaryDetection="true">
                            </telerik:RadMonthYearPicker></td>
                                    <td> -&nbsp;&nbsp;</td>
                                    <td><telerik:RadMonthYearPicker runat="server" ID="rdpEndDate" Width="120"
                                 EnableShadows="true"
                                 MonthYearNavigationSettings-CancelButtonCaption="取消"
                                 MonthYearNavigationSettings-OkButtonCaption="确定"
                                 MonthYearNavigationSettings-TodayButtonCaption="今天"
                                 MonthYearNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                 MonthYearNavigationSettings-EnableScreenBoundaryDetection="true">
                             </telerik:RadMonthYearPicker></td>
                                </tr>
                            </table>
                        </td>
                        <td class="width80 middle-td">部门：</td>
                        <td class="middle-td width380">
                            <telerik:RadComboBox runat="server" ID="rcbxDepartment" Filter="Contains"
                                AllowCustomText="true" Height="160px" Width="200px" EmptyMessage="--请选择--">
                            </telerik:RadComboBox>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                             <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                        </td>

                    </tr>


                </table>
                <telerik:RadGrid ID="rgSalarySettles" runat="server" PageSize="10"
                    AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                    MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                    ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                    OnNeedDataSource="rgSalarySettles_NeedDataSource" OnDeleteCommand="rgSalarySettles_DeleteCommand"
                    OnItemCreated="rgSalarySettles_ItemCreated" OnColumnCreated="rgSalarySettles_ColumnCreated" OnItemDataBound="rgSalarySettles_ItemDataBound">
                    <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                        ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                        <Columns>
                            <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false">
                                <ItemStyle HorizontalAlign="Left" Width="50" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="SettleDate" HeaderText="结算月份" DataField="SettleDate" DataFormatString="{0:yyyy/MM}">
                                <ItemStyle HorizontalAlign="Left" Width="160px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="DepartmentName" HeaderText="部门" DataField="DepartmentName">
                                <ItemStyle HorizontalAlign="Left" Width="160px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="CreatedByUserName" HeaderText="操作人" DataField="CreatedByUserName">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="WorkflowStatus" HeaderText="状态" DataField="WorkflowStatus">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>


                            <telerik:GridTemplateColumn UniqueName="Edit">
                                <ItemStyle HorizontalAlign="Center" Width="60" />
                                <ItemTemplate>
                                    <a href="javascript:void(0);" onclick="redirectToMaintenancePage(<%#DataBinder.Eval(Container.DataItem,"ID")%>)">编辑</a>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="40" ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />
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
                    <ClientSettings>
                        <ClientEvents OnGridCreated="GetsGridObject" />
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

        function redirectToMaintenancePage(id) {
            $.showLoading();

            window.location.href = "SalarySettleMaintenance.aspx?EntityID=" + id;
        }


    </script>
</asp:Content>
