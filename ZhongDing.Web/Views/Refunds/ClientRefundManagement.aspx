<%@ Page Title="客户返款管理" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientRefundManagement.aspx.cs" Inherits="ZhongDing.Web.Views.Refunds.ClientRefundManagement" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnSearchOrder">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divSearchOrders" />
                    <telerik:AjaxUpdatedControl ControlID="rgOrders" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnResetOrder">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divSearchOrders" />
                    <telerik:AjaxUpdatedControl ControlID="rgOrders" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgOrders">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgOrders" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbxCompany">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbxWarehouse" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbxClientUser">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbxClientCompany" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSearchClientRefunds">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divSearchClientRefunds" />
                    <telerik:AjaxUpdatedControl ControlID="rgClientRefunds" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnResetClientRefunds">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divSearchClientRefunds" />
                    <telerik:AjaxUpdatedControl ControlID="rgClientRefunds" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgClientRefunds">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgClientRefunds" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbxApplyCompany">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbxApplyWarehouse" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbxApplyClientUser">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbxApplyClientCompany" />
                </UpdatedControls>
            </telerik:AjaxSetting>


        </AjaxSettings>
    </telerik:RadAjaxManager>

    <!-- Main Container -->
    <div class="container">
        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-table-1">高开高返 - 客户返款管理</span>
            </div>
            <div class="mws-panel-body">
                <div class="mws-form">
                    <div class="mws-form-inline">
                        <div runat="server" id="divSearchOrders">
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
                                    <label class="leftpadding10">仓库</label>
                                    <div class="mws-form-item">
                                        <telerik:RadComboBox runat="server" ID="rcbxWarehouse" Filter="Contains"
                                            AllowCustomText="false" Height="160px" EmptyMessage="--请选择--">
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>
                            <div class="mws-form-row">
                                <div class="float-left width40-percent">
                                    <label>客户</label>
                                    <div class="mws-form-item">
                                        <telerik:RadComboBox runat="server" ID="rcbxClientUser" Filter="Contains"
                                            AllowCustomText="false" Height="160px" Width="100%" EmptyMessage="--请选择--"
                                            AutoPostBack="true" OnSelectedIndexChanged="rcbxClientUser_SelectedIndexChanged">
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                                <div class="float-left width60-percent">
                                    <label class="leftpadding10">商业单位</label>
                                    <div class="mws-form-item">
                                        <telerik:RadComboBox runat="server" ID="rcbxClientCompany" Filter="Contains"
                                            AllowCustomText="false" Height="160px" Width="100%" EmptyMessage="--请选择--">
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>
                            <div class="mws-form-row">
                                <label>订单日期</label>
                                <div class="mws-form-item">
                                    <telerik:RadDatePicker runat="server" ID="rdpOrderBeginDate" Width="120"
                                        Calendar-EnableShadows="true"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                        Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                        Calendar-FastNavigationSettings-DisableOutOfRangeMonths="true"
                                        Calendar-FirstDayOfWeek="Monday">
                                    </telerik:RadDatePicker>
                                    -&nbsp;&nbsp;
                                    <telerik:RadDatePicker runat="server" ID="rdpOrderEndDate" Width="120"
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
                            <div class="mws-form-row">
                                <label></label>
                                <div class="mws-form-item">
                                    <asp:Button ID="btnSearchOrder" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearchOrder_Click" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnResetOrder" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnResetOrder_Click" />
                                </div>
                            </div>
                        </div>

                        <!--高开高返订单-->
                        <div class="mws-form-row">
                            <telerik:RadGrid ID="rgOrders" runat="server" PageSize="10"
                                AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                OnNeedDataSource="rgOrders_NeedDataSource" OnItemDataBound="rgOrders_ItemDataBound"
                                OnColumnCreated="rgOrders_ColumnCreated">
                                <MasterTableView Width="100%" DataKeyNames="ClientSaleAppID" CommandItemDisplay="Top"
                                    ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="刷新" />
                                    <Columns>
                                        <telerik:GridBoundColumn UniqueName="CompanyName" HeaderText="账套" DataField="CompanyName">
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="OrderCode" HeaderText="订单号" DataField="OrderCode">
                                            <HeaderStyle Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ClientUserName" HeaderText="客户名称" DataField="ClientUserName">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ClientCompanyName" HeaderText="商业单位" DataField="ClientCompanyName">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="RefundAmount" HeaderText="应返款" DataField="RefundAmount" DataFormatString="{0:C2}">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
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
                                        <telerik:GridTemplateColumn UniqueName="Edit">
                                            <HeaderStyle Width="60" />
                                            <ItemStyle HorizontalAlign="Center" Width="60" />
                                            <ItemTemplate>
                                                <a href="javascript:void(0);" onclick="redirectToMaintenancePage(-1, <%#DataBinder.Eval(Container.DataItem,"ClientSaleAppID")%>)">申请返款</a>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
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
                                </ClientSettings>
                            </telerik:RadGrid>
                        </div>

                        <!--返款管理-->
                        <div class="mws-form-row">
                            <div class="mws-panel grid_8" style="margin-left: 0px; margin-right: 0px; width: 100%;">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-table-1">返款申请</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-form">
                                        <div class="mws-form-inline">
                                            <div runat="server" id="divSearchClientRefunds">
                                                <div class="mws-form-row">
                                                    <label>申请日期</label>
                                                    <div class="mws-form-item">
                                                        <telerik:RadDatePicker runat="server" ID="rdpApplyBeginDate" Width="120"
                                                            Calendar-EnableShadows="true"
                                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                                            Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                            Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                                            Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                                            Calendar-FastNavigationSettings-DisableOutOfRangeMonths="true"
                                                            Calendar-FirstDayOfWeek="Monday">
                                                        </telerik:RadDatePicker>
                                                        -&nbsp;&nbsp;
                                                        <telerik:RadDatePicker runat="server" ID="rdpApplyEndDate" Width="120"
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
                                                <div class="mws-form-row">
                                                    <div class="float-left width40-percent">
                                                        <label>账套</label>
                                                        <div class="mws-form-item">
                                                            <telerik:RadComboBox runat="server" ID="rcbxApplyCompany" Filter="Contains"
                                                                AllowCustomText="false" Height="160px" EmptyMessage="--请选择--" AutoPostBack="true"
                                                                OnSelectedIndexChanged="rcbxApplyCompany_SelectedIndexChanged">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                    </div>
                                                    <div class="float-left">
                                                        <label class="leftpadding10">仓库</label>
                                                        <div class="mws-form-item">
                                                            <telerik:RadComboBox runat="server" ID="rcbxApplyWarehouse" Filter="Contains"
                                                                AllowCustomText="false" Height="160px" EmptyMessage="--请选择--">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="mws-form-row">
                                                    <div class="float-left width40-percent">
                                                        <label>客户</label>
                                                        <div class="mws-form-item">
                                                            <telerik:RadComboBox runat="server" ID="rcbxApplyClientUser" Filter="Contains"
                                                                AllowCustomText="false" Height="160px" Width="100%" EmptyMessage="--请选择--"
                                                                AutoPostBack="true" OnSelectedIndexChanged="rcbxApplyClientUser_SelectedIndexChanged">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                    </div>
                                                    <div class="float-left width60-percent">
                                                        <label class="leftpadding10">商业单位</label>
                                                        <div class="mws-form-item">
                                                            <telerik:RadComboBox runat="server" ID="rcbxApplyClientCompany" Filter="Contains"
                                                                AllowCustomText="false" Height="160px" Width="100%" EmptyMessage="--请选择--">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="mws-form-row">
                                                    <label>申请状态</label>
                                                    <div class="mws-form-item">
                                                        <telerik:RadComboBox runat="server" ID="rcbxWorkflowStatus" Filter="Contains"
                                                            AllowCustomText="true" EmptyMessage="--请选择--">
                                                        </telerik:RadComboBox>
                                                    </div>
                                                </div>

                                                <div class="mws-form-row">
                                                    <label></label>
                                                    <div class="mws-form-item">
                                                        <asp:Button ID="btnSearchClientRefunds" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearchClientRefunds_Click" />
                                                        &nbsp;&nbsp;
                                                        <asp:Button ID="btnResetClientRefunds" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnResetClientRefunds_Click" />
                                                    </div>
                                                </div>
                                            </div>

                                            <!--返款申请列表-->
                                            <div class="mws-form-row">
                                                <telerik:RadGrid ID="rgClientRefunds" runat="server" PageSize="10"
                                                    AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                                    MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                                    ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                                    OnNeedDataSource="rgClientRefunds_NeedDataSource" OnItemDataBound="rgClientRefunds_ItemDataBound"
                                                    OnColumnCreated="rgClientRefunds_ColumnCreated">
                                                    <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                        ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="刷新" />
                                                        <Columns>
                                                            <telerik:GridBoundColumn UniqueName="CreatedOn" HeaderText="申请日期" DataField="CreatedOn" DataFormatString="{0:yyyy/MM/dd}">
                                                                <HeaderStyle Width="100" />
                                                                <ItemStyle HorizontalAlign="Left" Width="100" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="CompanyName" HeaderText="账套" DataField="CompanyName">
                                                                <HeaderStyle Width="10%" />
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="OrderCode" HeaderText="订单号" DataField="OrderCode">
                                                                <HeaderStyle Width="15%" />
                                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="ClientUserName" HeaderText="客户名称" DataField="ClientUserName">
                                                                <HeaderStyle Width="100" />
                                                                <ItemStyle HorizontalAlign="Left" Width="100" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="ClientCompanyName" HeaderText="商业单位" DataField="ClientCompanyName">
                                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="RefundAmount" HeaderText="返款金额" DataField="RefundAmount" DataFormatString="{0:C2}">
                                                                <HeaderStyle Width="100" />
                                                                <ItemStyle HorizontalAlign="Left" Width="100" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="CreatedBy" HeaderText="申请人" DataField="CreatedBy">
                                                                <HeaderStyle Width="60" />
                                                                <ItemStyle HorizontalAlign="Left" Width="60" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="WorkflowStatus" HeaderText="状态" DataField="WorkflowStatus">
                                                                <HeaderStyle Width="120" />
                                                                <ItemStyle HorizontalAlign="Left" Width="120" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn UniqueName="Edit">
                                                                <HeaderStyle Width="60" />
                                                                <ItemStyle HorizontalAlign="Center" Width="60" />
                                                                <ItemTemplate>
                                                                    <a href="javascript:void(0);" onclick="redirectToMaintenancePage(<%#DataBinder.Eval(Container.DataItem,"ID")%>)">编辑</a>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton"
                                                                HeaderStyle-Width="60" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" Visible="false" />
                                                        </Columns>
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
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                            </div>
                                            <div class="mws-form-row"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">
        

        function redirectToMaintenancePage(id, clientSaleAppID) {
            $.showLoading();

            var targetUrl = "ClientRefundMaintenance.aspx?EntityID=" + id;

            if (clientSaleAppID)
                targetUrl += "&ClientSaleAppID=" + clientSaleAppID;

            window.location.href = targetUrl;
        }

    </script>
</asp:Content>

