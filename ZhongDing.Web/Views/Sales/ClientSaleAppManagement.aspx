<%@ Page Title="客户订单管理" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientSaleAppManagement.aspx.cs" Inherits="ZhongDing.Web.Views.Sales.ClientSaleAppManagement" %>

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
            <telerik:AjaxSetting AjaxControlID="rcbxClientUser">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbxClientCompany" LoadingPanelID="loadingPanel" />
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
                <span class="mws-i-24 i-table-1">客户订单管理</span>
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
                    OnItemDataBound="rgEntities_ItemDataBound" OnItemCommand="rgEntities_ItemCommand" >
                    <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                        ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                        <Columns>
                            <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false">
                                <ItemStyle HorizontalAlign="Left" Width="50" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="OrderCode" HeaderText="订单编号" DataField="OrderCode">
                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="OrderDate" HeaderText="订单日期" DataField="OrderDate" DataFormatString="{0:yyyy/MM/dd}">
                                <ItemStyle HorizontalAlign="Left" Width="60" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="SaleOrderTypeName" HeaderText="订单类型" DataField="SaleOrderTypeName">
                                <ItemStyle HorizontalAlign="Left" Width="60" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ClientUserName" HeaderText="客户名称" DataField="ClientUserName">
                                <ItemStyle HorizontalAlign="Left" Width="80" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ClientCompanyName" HeaderText="商业单位" DataField="ClientCompanyName">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="CreatedBy" HeaderText="制单人" DataField="CreatedBy">
                                <ItemStyle HorizontalAlign="Left" Width="40" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="WorkflowStatus" HeaderText="状态" DataField="WorkflowStatus">
                                <ItemStyle HorizontalAlign="Left" Width="60" />
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="IconUrlOfGuarantee" HeaderText="担保" SortExpression="IconUrlOfGuarantee">
                                <ItemStyle HorizontalAlign="Center" Width="40" />
                                <ItemTemplate>
                                    <asp:Image runat="server" ID="iconOfGuarantee" ImageUrl='<%# Page.ResolveUrl(DataBinder.Eval(Container.DataItem,"IconUrlOfGuarantee")!=null
                                    ?DataBinder.Eval(Container.DataItem,"IconUrlOfGuarantee").ToString():string.Empty)%>'
                                        Width="24" Height="24"
                                        Visible='<%# DataBinder.Eval(Container.DataItem,"IsGuaranteed") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridCheckBoxColumn UniqueName="IsStop" HeaderText="中止执行?" DataField="IsStop">
                                <HeaderStyle Width="60" />
                                <ItemStyle HorizontalAlign="Left" Width="60" />
                            </telerik:GridCheckBoxColumn>
                            <telerik:GridTemplateColumn UniqueName="Edit">
                                <ItemStyle HorizontalAlign="Center" Width="60" />
                                <ItemTemplate>
                                    <a href="javascript:void(0);" onclick="redirectToMaintenancePage(<%#DataBinder.Eval(Container.DataItem,"ID")%>)">编辑</a>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridButtonColumn Text="中止" HeaderText="中止" UniqueName="Stop" CommandName="Stop" ButtonType="LinkButton"
                                HeaderStyle-Width="60" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center" ConfirmText="确认中止该条客户订单吗？" Visible="false" />
                            <telerik:GridButtonColumn Text="删除" HeaderText="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton"
                                HeaderStyle-Width="60" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" Visible="false" />
                        </Columns>
                        <CommandItemTemplate>
                            <table class="width100-percent">
                                <tr>
                                    <td>
                                        <asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
                                            <input type="button" class="rgAdd" onclick="openSaleOrderTypeWin(); return false;" />
                                            <a href="javascript:void(0)" onclick="openSaleOrderTypeWin(); return false;">添加</a>
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

    <telerik:RadWindow runat="server" ID="winSelectSaleOrderType" Width="400" Height="200"
        Modal="true" Behaviors="Close" Title="选择订单类型">
        <Localization Close="关闭" />
        <ContentTemplate>
            <div class="mws-panel grid_full" style="margin-bottom: 10px; margin-top: 10px;">
                <div class="mws-panel-body">
                    <div class="mws-form">
                        <div class="mws-form-inline">
                            <div class="mws-form-row">
                                <label>选择订单类型</label>
                                <div class="mws-form-item small  toppadding5">
                                    <telerik:RadButton runat="server" ID="radioAttractBusinessMode" ButtonType="ToggleButton" ToggleType="Radio" AutoPostBack="false"
                                        GroupName="SaleOrderType" Text="招商模式" Value="2" Checked="true">
                                    </telerik:RadButton>
                                    &nbsp;&nbsp;
                                    <telerik:RadButton runat="server" ID="radioAttachedMode" ButtonType="ToggleButton" ToggleType="Radio" AutoPostBack="false"
                                        GroupName="SaleOrderType" Text="挂靠模式" Value="3">
                                    </telerik:RadButton>
                                </div>
                            </div>
                            <div class="mws-button-row">
                                <asp:Button ID="btnCreateOrder" runat="server" Text="新建订单" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="createNewOrder();return false;" />
                                <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="closeSaleOrderTypeWin();return false;" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </telerik:RadWindow>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">
        var gridOfRefresh = null;
        var winSaleOrderType = null;

        function GetsGridObject(sender, eventArgs) {
            gridOfRefresh = sender;
        }

        function refreshGrid() {
            gridOfRefresh.get_masterTableView().rebind();
        }

        function redirectToMaintenancePage(id) {
            $.showLoading();
            window.location.href = "ClientSaleAppMaintenance.aspx?EntityID=" + id;
        }

        function openSaleOrderTypeWin() {
            if (winSaleOrderType) {
                winSaleOrderType.show();
                winSaleOrderType.center();
            }
        }

        function closeSaleOrderTypeWin() {
            if (winSaleOrderType
                && !winSaleOrderType.isClosed()) {
                winSaleOrderType.close();
            }
        }

        function createNewOrder() {

            var saleOrderTypeID = 0;

            var radioAttractBusinessMode = $find("<%= radioAttractBusinessMode.ClientID %>");
            var radioAttachedMode = $find("<%= radioAttachedMode.ClientID %>");

            if (radioAttractBusinessMode.get_checked() == true) {
                saleOrderTypeID = ESaleOrderTypes.AttractBusinessMode;
            }
            else if (radioAttachedMode.get_checked() == true) {
                saleOrderTypeID = ESaleOrderTypes.AttachedMode;
            }

            if (saleOrderTypeID > 0) {
                $.showLoading();
                window.location.href = "ClientSaleAppMaintenance.aspx?EntityID=-1&SaleOrderTypeID=" + saleOrderTypeID;
            }
        }

        $(document).ready(function () {
            winSaleOrderType = $find("<%=winSelectSaleOrderType.ClientID %>");

        });

    </script>
</asp:Content>
