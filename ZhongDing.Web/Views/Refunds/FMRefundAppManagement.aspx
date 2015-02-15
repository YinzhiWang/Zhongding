<%@ Page Title="厂家经理返款管理" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FMRefundAppManagement.aspx.cs" Inherits="ZhongDing.Web.Views.Refunds.FMRefundAppManagement" %>

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
                    <telerik:AjaxUpdatedControl ControlID="divSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgEntities" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgEntities" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgEntities">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgEntities" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbxCompany">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbxProduct" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <!-- Main Container -->
    <div class="container">
        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-table-1">厂家经理返款管理</span>
            </div>
            <div class="mws-panel-body">
                <div class="mws-form">
                    <div class="mws-form-inline">
                        <div runat="server" id="divSearch">
                            <div class="mws-form-row">
                                <div class="float-left width40-percent">
                                    <label>账套</label>
                                    <div class="mws-form-item">
                                        <telerik:RadComboBox runat="server" ID="rcbxCompany" Filter="Contains"
                                            AllowCustomText="false" Height="160px" EmptyMessage="--请选择--" AutoPostBack="true"
                                            OnSelectedIndexChanged="rcbxCompany_SelectedIndexChanged">
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                                <div class="float-left">
                                    <label>申请日期</label>
                                    <div class="mws-form-item">
                                        <telerik:RadDatePicker runat="server" ID="rdpBeginDate" Width="120"
                                            Calendar-EnableShadows="true"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                            Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                            Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                            Calendar-FastNavigationSettings-DisableOutOfRangeMonths="true"
                                            Calendar-FirstDayOfWeek="Monday">
                                        </telerik:RadDatePicker>
                                        -&nbsp;&nbsp;
                                        <telerik:RadDatePicker runat="server" ID="rdpEndDate" Width="120"
                                            Calendar-EnableShadows="true"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                            Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                            Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                            Calendar-FastNavigationSettings-DisableOutOfRangeMonths="true"
                                            Calendar-FirstDayOfWeek="Monday">
                                        </telerik:RadDatePicker>
                                    </div>
                                </div>
                            </div>
                            <div class="mws-form-row">
                                <div class="float-left width40-percent">
                                    <label>客户</label>
                                    <div class="mws-form-item">
                                        <telerik:RadComboBox runat="server" ID="rcbxClientUser" Filter="Contains"
                                            AllowCustomText="false" Height="160px" Width="100%" EmptyMessage="--请选择--">
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                                <div class="float-left width60-percent">
                                    <label class="leftpadding10">货品</label>
                                    <div class="mws-form-item">
                                        <telerik:RadComboBox runat="server" ID="rcbxProduct" Filter="Contains"
                                            AllowCustomText="false" Height="160px" Width="60%" EmptyMessage="--请选择--">
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>
                            <div class="mws-form-row">
                                <label>状态</label>
                                <div class="mws-form-item">
                                    <telerik:RadComboBox runat="server" ID="rcbxWorkflowStatus" Filter="Contains"
                                        AllowCustomText="true" EmptyMessage="--请选择--">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="mws-form-row">
                                <label></label>
                                <div class="mws-form-item">
                                    <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                                </div>
                            </div>
                        </div>

                        <div class="mws-form-row bottommargin20">
                            <telerik:RadGrid ID="rgEntities" runat="server" PageSize="10"
                                AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                OnNeedDataSource="rgEntities_NeedDataSource" OnItemDataBound="rgEntities_ItemDataBound"
                                OnColumnCreated="rgEntities_ColumnCreated" OnItemCreated="rgEntities_ItemCreated"
                                OnDeleteCommand="rgEntities_DeleteCommand">
                                <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                    ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                    <Columns>
                                        <telerik:GridBoundColumn UniqueName="CreatedOn" HeaderText="申请日期" DataField="CreatedOn" DataFormatString="{0:yyyy/MM/dd}">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="CompanyName" HeaderText="账套" DataField="CompanyName">
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ClientUserName" HeaderText="客户" DataField="ClientUserName">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName">
                                            <HeaderStyle Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="BeginDate" HeaderText="结算起始日" DataField="BeginDate" DataFormatString="{0:yyyy/MM/dd}">
                                            <HeaderStyle Width="105" />
                                            <ItemStyle HorizontalAlign="Left" Width="105" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="EndDate" HeaderText="结算结束日" DataField="EndDate" DataFormatString="{0:yyyy/MM/dd}">
                                            <HeaderStyle Width="105" />
                                            <ItemStyle HorizontalAlign="Left" Width="105" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="RefundAmount" HeaderText="返款金额" DataField="RefundAmount" DataFormatString="{0:C2}">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="CreatedBy" HeaderText="操作人" DataField="CreatedBy">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="WorkflowStatus" HeaderText="状态" DataField="WorkflowStatus">
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
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
                                        <telerik:GridButtonColumn UniqueName="Delete" Text="删除" HeaderText="删除" CommandName="Delete" ButtonType="LinkButton"
                                            HeaderStyle-Width="60" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" Visible="false" />
                                    </Columns>
                                    <CommandItemTemplate>
                                        <table class="width100-percent">
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
                                                        <input type="button" class="rgAdd" onclick="redirectToMaintenancePage(-1); return false;" />
                                                        <a href="javascript:void(0)" onclick="redirectToMaintenancePage(-1); return false;">添加</a>
                                                    </asp:Panel>
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
            window.location.href = "FMRefundAppMaintenance.aspx?EntityID=" + id;
        }

        function openPrintPage(entityID) {

            var targetUrl = $.getRootPath() + "Views/Refunds/Printers/PrintFMRefundApp.aspx?EntityID=" + entityID;

            window.open(targetUrl, "_blank");
        }

    </script>
</asp:Content>

