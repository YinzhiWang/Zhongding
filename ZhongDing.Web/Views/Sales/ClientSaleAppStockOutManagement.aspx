<%@ Page Title="客户订单出库单管理" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientSaleAppStockOutManagement.aspx.cs" Inherits="ZhongDing.Web.Views.Sales.ClientSaleAppStockOutManagement" %>

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
                    <telerik:AjaxUpdatedControl ControlID="rgEntities" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgEntities" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgEntities">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgEntities" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <!-- Main Container -->
    <div class="container">
        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-table-1">客户订单出库单管理</span>
            </div>
            <div class="mws-panel-body">
                <table runat="server" id="tblSearch" class="leftmargin10">
                    <tr class="height40">
                        <th class="width100 middle-td">起止日期：</th>
                        <td class="middle-td" colspan="3">
                            <telerik:RadDatePicker runat="server" ID="rdpBeginDate" Width="120"></telerik:RadDatePicker>
                            -&nbsp;&nbsp;
                            <telerik:RadDatePicker runat="server" ID="rdpEndDate" Width="120"></telerik:RadDatePicker>
                        </td>
                    </tr>
                    <tr class="height40">
                        <th class="width70 middle-td">客户名称：</th>
                        <td class="middle-td width20-percent">
                            <telerik:RadComboBox runat="server" ID="rcbxClientUser" Filter="Contains"
                                AllowCustomText="false" Height="160px" Width="100%" EmptyMessage="--请选择--"
                                AutoPostBack="true" OnSelectedIndexChanged="rcbxClientUser_SelectedIndexChanged">
                            </telerik:RadComboBox>
                        </td>
                        <th class="width100 middle-td right-td">商业单位：</th>
                        <td class="middle-td leftpadding10">
                            <telerik:RadComboBox runat="server" ID="rcbxClientCompany" Height="160px" Width="60%" Filter="Contains"
                                EmptyMessage="--请选择--" AllowCustomText="false">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr class="height40">
                        <th class="width60 middle-td">状态：</th>
                        <td class="middle-td">
                            <telerik:RadComboBox runat="server" ID="rcbxWorkflowStatus" Filter="Contains" AutoPostBack="true"
                                AllowCustomText="true" EmptyMessage="--请选择--">
                            </telerik:RadComboBox>
                        </td>
                        <td class="middle-td leftpadding20" colspan="2">
                            <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                        </td>
                    </tr>
                </table>
                <telerik:RadGrid ID="rgEntities" runat="server" PageSize="10"
                    AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                    MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                    ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                    OnNeedDataSource="rgEntities_NeedDataSource" OnDeleteCommand="rgEntities_DeleteCommand"
                    OnItemCreated="rgEntities_ItemCreated" OnColumnCreated="rgEntities_ColumnCreated"
                    OnItemDataBound="rgEntities_ItemDataBound">
                    <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                        ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                        <Columns>
                            <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false">
                                <ItemStyle HorizontalAlign="Left" Width="50" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="Code" HeaderText="出库单号" DataField="Code">
                                <HeaderStyle Width="20%" />
                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ClientUserName" HeaderText="客户" DataField="ClientUserName">
                                <HeaderStyle Width="100" />
                                <ItemStyle HorizontalAlign="Left" Width="100" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ClientCompany" HeaderText="商业单位" DataField="ClientCompany">
                                <HeaderStyle Width="15%" />
                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="CreatedBy" HeaderText="制单人" DataField="CreatedBy">
                                <HeaderStyle Width="80" />
                                <ItemStyle HorizontalAlign="Left" Width="80" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ReceiverName" HeaderText="收货人" DataField="ReceiverName">
                                <HeaderStyle Width="80" />
                                <ItemStyle HorizontalAlign="Left" Width="80" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ReceiverPhone" HeaderText="收货电话" DataField="ReceiverPhone">
                                <HeaderStyle Width="140" />
                                <ItemStyle HorizontalAlign="Left" Width="140" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ReceiverAddress" HeaderText="收货地址" DataField="ReceiverAddress">
                                <HeaderStyle Width="20%" />
                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="WorkflowStatus" HeaderText="状态" DataField="WorkflowStatus">
                                <HeaderStyle Width="80" />
                                <ItemStyle HorizontalAlign="Left" Width="80" />
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="Edit">
                                <HeaderStyle Width="60" />
                                <ItemStyle HorizontalAlign="Center" Width="60" />
                                <ItemTemplate>
                                    <a href="javascript:void(0);" onclick="redirectToMaintenancePage(<%#DataBinder.Eval(Container.DataItem,"ID")%>)">编辑</a>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="Print" HeaderText="打印">
                                <HeaderStyle HorizontalAlign="Center" Width="60" />
                                <ItemStyle HorizontalAlign="Center" Width="60" />
                                <ItemTemplate>
                                    <a href="javascript:void(0);" onclick="openPrintPage(<%#DataBinder.Eval(Container.DataItem,"ID")%>)">打印</a>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridButtonColumn Text="删除" HeaderText="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton"
                                HeaderStyle-Width="60" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />
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
            window.location.href = "ClientSaleAppStockOutMaintenance.aspx?EntityID=" + id;
        }

        function openPrintPage(entityID) {

            var targetUrl = $.getRootPath() + "Views/Sales/Printers/PrintStockOut.aspx?EntityID=" + entityID;

            window.open(targetUrl, "_blank");
        }
    </script>
</asp:Content>
