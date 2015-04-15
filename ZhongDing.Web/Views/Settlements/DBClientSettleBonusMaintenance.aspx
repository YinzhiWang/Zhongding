<%@ Page Title="大包客户结算维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DBClientSettleBonusMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Settlements.DBClientSettleBonusMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rgAppNotes">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgAppNotes" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgAuditNotes">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgAuditNotes" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgDBClientSettleBonus">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgDBClientSettleBonus" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgDBClientSettleBonus" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgDBClientSettleBonus" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgAppPayments">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgAppPayments" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="container">
        <div class="mws-panel grid_full">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">大包客户结算维护</span>
            </div>
            <div class="mws-panel-body">
                <div class="mws-form">
                    <div class="mws-form-inline">
                        <div class="mws-form-row">
                            <div class="validate-message-wrapper">
                                <asp:ValidationSummary ID="vsMaintenance" runat="server" ValidationGroup="vgMaintenance" DisplayMode="BulletList" HeaderText="请更正以下错误:" CssClass="validation-summary-errors" />
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>结算表生成日期</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblCreatedOn" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>操作人</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblCreateBy" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>结算日期</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblSettlementOperateDate" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>性质</label>
                                <div class="mws-form-item small toppadding5">
                                    <asp:Label ID="lblHospitalType" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row" runat="server" id="divComment">
                            <label>备注</label>
                            <div class="mws-form-item medium">
                                <telerik:RadTextBox runat="server" ID="txtComment" Width="90%" MaxLength="1000"
                                    TextMode="MultiLine" Height="80">
                                </telerik:RadTextBox>
                            </div>
                        </div>
                        <div class="mws-form-row" runat="server" id="divComments">
                            <label>备注历史</label>
                            <div class="mws-form-item medium">
                                <telerik:RadDockLayout runat="server" ID="RadDockLayout1">
                                    <telerik:RadDockZone runat="server" ID="RadDockZone1" Orientation="Vertical"
                                        Width="99%" FitDocks="true" BorderStyle="None">
                                        <telerik:RadDock ID="RadDock1" Title="备注历史记录" runat="server" AllowedZones="RadDockZone1" Font-Size="12px"
                                            DefaultCommands="ExpandCollapse" EnableAnimation="true" EnableDrag="false"
                                            DockMode="Docked" ExpandText="展开" CollapseText="折叠">
                                            <ContentTemplate>
                                                <div class="toppadding10"></div>
                                                <telerik:RadGrid ID="rgAppNotes" runat="server"
                                                    AllowPaging="false" AllowSorting="true" AutoGenerateColumns="false" Skin="Silk" Width="99.5%"
                                                    ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                                    OnNeedDataSource="rgAppNotes_NeedDataSource">
                                                    <MasterTableView Width="100%" DataKeyNames="ID" ShowHeader="true" BackColor="#fafafa">
                                                        <Columns>
                                                            <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="CreatedOn" HeaderText="创建时间" DataField="CreatedOn" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}">
                                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="CreatedBy" HeaderText="创建人" DataField="CreatedBy">
                                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="Note" HeaderText="备注内容" DataField="Note">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                        <NoRecordsTemplate>
                                                            没有任何数据
                                                        </NoRecordsTemplate>
                                                        <ItemStyle Height="30" />
                                                        <AlternatingItemStyle BackColor="#f2f2f2" />
                                                    </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true" />
                                                </telerik:RadGrid>
                                            </ContentTemplate>
                                        </telerik:RadDock>
                                    </telerik:RadDockZone>
                                </telerik:RadDockLayout>
                            </div>
                        </div>
                        <div class="height20"></div>
                        <div class="mws-form-row" runat="server" id="divOtherSections">
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-procure-order">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">结算明细</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <table runat="server" id="tblSearch" class="leftmargin10">
                                            <tr class="height40">
                                                <td class="middle-td leftpadding10">
                                                    <telerik:RadTextBox runat="server" ID="txtClientUserName" Label="客户名称：" LabelWidth="75" Width="260" MaxLength="100"></telerik:RadTextBox>
                                                </td>
                                                <td class="middle-td leftpadding20">
                                                    <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                                                </td>
                                                <td class="middle-td leftpadding20">
                                                    <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                                                </td>
                                            </tr>
                                        </table>

                                        <telerik:RadGrid ID="rgDBClientSettleBonus" runat="server" PageSize="10"
                                            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true" ShowFooter="true"
                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgDBClientSettleBonus_NeedDataSource" OnColumnCreated="rgDBClientSettleBonus_ColumnCreated"
                                            OnItemDataBound="rgDBClientSettleBonus_ItemDataBound">
                                            <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                                <Columns>
                                                    <telerik:GridBoundColumn UniqueName="ClientUserName" HeaderText="客户" DataField="ClientUserName" FooterText="合计："
                                                        FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="Hospitals" HeaderText="医院" DataField="Hospitals">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品" DataField="ProductName">
                                                        <ItemStyle HorizontalAlign="Left" Width="60" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="ClientDBBankAccount" HeaderText="银行账号" DataField="ClientDBBankAccount">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="SettlementDate" HeaderText="年月" DataField="SettlementDate" DataFormatString="{0:yyyy/MM}">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="BonusAmount" HeaderText="提成" DataField="BonusAmount" DataFormatString="{0:C2}">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="IsNeedSettlement" HeaderText="需结算" DataField="IsNeedSettlement" SortExpression="IsNeedSettlement">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <span>
                                                                <%#DataBinder.Eval(Container.DataItem,"IsNeedSettlement")!=null
                                                                    ?(Convert.ToBoolean( DataBinder.Eval(Container.DataItem,"IsNeedSettlement").ToString())==true 
                                                                        ? ZhongDing.Common.GlobalConst.BoolChineseDescription.TRUE
                                                                            : ZhongDing.Common.GlobalConst.BoolChineseDescription.FALSE)
                                                                    :string.Empty%>
                                                            </span>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn UniqueName="PerformanceAmount" HeaderText="绩效" DataField="PerformanceAmount" DataFormatString="{0:C2}">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="TotalPayAmount" HeaderText="应发" DataField="TotalPayAmount" DataFormatString="{0:C2}"
                                                        Aggregate="Sum" FooterStyle-Font-Bold="true">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="IsSettled" HeaderText="支付状态" DataField="IsSettled" SortExpression="IsSettled">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <span>
                                                                <%#DataBinder.Eval(Container.DataItem,"IsSettled")!=null
                                                                    ?(Convert.ToBoolean( DataBinder.Eval(Container.DataItem,"IsSettled").ToString())==true 
                                                                        ? ZhongDing.Common.GlobalConst.PaymentStatus.PAID
                                                                            : ZhongDing.Common.GlobalConst.PaymentStatus.TO_BE_PAY)
                                                                    :string.Empty%>
                                                            </span>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Edit" HeaderStyle-Width="60">
                                                        <ItemStyle HorizontalAlign="Center" Width="60" />
                                                        <ItemTemplate>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                                <CommandItemTemplate>
                                                    <table class="width100-percent">
                                                        <tr>
                                                            <td></td>
                                                            <td class="right-td rightpadding10">
                                                                <input type="button" class="rgRefresh" onclick="refreshGrid(gridClientIDs.gridDBClientSettleBonus); return false;" />
                                                                <a href="javascript:void(0);" onclick="refreshGrid(gridClientIDs.gridDBClientSettleBonus); return false;">刷新</a>
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
                                            </ClientSettings>
                                        </telerik:RadGrid>
                                    </div>
                                </div>
                            </div>

                            <!--支付信息 -->
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-payment" runat="server" id="divAppPayments">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">支付信息</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <telerik:RadGrid ID="rgAppPayments" runat="server" PageSize="10"
                                            AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true" ShowFooter="true"
                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgAppPayments_NeedDataSource" OnItemCreated="rgAppPayments_ItemCreated">
                                            <MasterTableView Width="100%" CommandItemDisplay="Top"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                                <Columns>
                                                    <telerik:GridTemplateColumn UniqueName="PayDate" HeaderText="转账日期" DataField="PayDate" SortExpression="PayDate"
                                                        FooterText="合计：" FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right">
                                                        <HeaderStyle Width="10%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        <ItemTemplate>
                                                            <a href="javascript:void(0);" onclick="openViewAppPaymentsWindow('<%#DataBinder.Eval(Container.DataItem,"PayDate","{0:yyyy/MM/dd}")%>')"><%#DataBinder.Eval(Container.DataItem,"PayDate","{0:yyyy/MM/dd}")%></a>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn UniqueName="FromAccount" HeaderText="转出账号" DataField="FromAccount">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="Amount" HeaderText="金额" DataField="Amount" DataFormatString="{0:C2}"
                                                        FooterAggregateFormatString="{0:C2}" Aggregate="Sum" FooterStyle-Font-Bold="true" SortExpression="Amount">
                                                        <HeaderStyle Width="15%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="Fee" HeaderText="手续费" DataField="Fee" DataFormatString="{0:C2}"
                                                        FooterAggregateFormatString="{0:C2}" Aggregate="Sum" FooterStyle-Font-Bold="true" SortExpression="Fee">
                                                        <HeaderStyle Width="10%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                                <CommandItemTemplate>
                                                    <table class="width100-percent">
                                                        <tr>
                                                            <td>
                                                                <asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
                                                                    <input type="button" class="rgAdd" onclick="openAppPaymentWindow(-1); return false;" />
                                                                    <a href="javascript:void(0)" onclick="openAppPaymentWindow(-1); return false;">添加</a>
                                                                </asp:Panel>
                                                            </td>
                                                            <td class="right-td rightpadding10">
                                                                <input type="button" class="rgRefresh" onclick="refreshGrid(gridClientIDs.gridAppPayments); return false;" />
                                                                <a href="javascript:void(0);" onclick="refreshGrid(gridClientIDs.gridAppPayments); return false;">刷新</a>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </CommandItemTemplate>
                                                <NoRecordsTemplate>
                                                    没有任何数据
                                                </NoRecordsTemplate>
                                                <ItemStyle Height="30" />
                                                <CommandItemStyle Height="30" />
                                                <AlternatingItemStyle BackColor="#f2f2f2" />
                                                <PagerStyle PagerTextFormat="{4} 第{0}页/共{1}页, 第{2}-{3}条 共{5}条"
                                                    PageSizeControlType="RadComboBox" PageSizeLabelText="每页条数:"
                                                    FirstPageToolTip="第一页" PrevPageToolTip="上一页" NextPageToolTip="下一页" LastPageToolTip="最后一页" />
                                            </MasterTableView>
                                            <ClientSettings EnableRowHoverStyle="true" />
                                        </telerik:RadGrid>
                                    </div>
                                </div>
                            </div>

                            <!--审核 -->
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-procure-audit">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">审核</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <telerik:RadDockLayout runat="server" ID="RadDockLayout2">
                                            <telerik:RadDockZone runat="server" ID="RadDockZone2" Orientation="Vertical"
                                                Width="99%" FitDocks="true" BorderStyle="None">
                                                <telerik:RadDock ID="RadDock2" Title="审核历史记录" runat="server" AllowedZones="RadDockZone1" Font-Size="12px"
                                                    DefaultCommands="ExpandCollapse" EnableAnimation="true" EnableDrag="false"
                                                    DockMode="Docked" ExpandText="展开" CollapseText="折叠">
                                                    <ContentTemplate>
                                                        <div class="toppadding10"></div>
                                                        <telerik:RadGrid ID="rgAuditNotes" runat="server"
                                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                                            AllowPaging="false" AllowSorting="true" AutoGenerateColumns="false" Skin="Silk" Width="99.5%"
                                                            OnNeedDataSource="rgAuditNotes_NeedDataSource">
                                                            <MasterTableView Width="100%" DataKeyNames="ID" ShowHeader="true" BackColor="#fafafa">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn UniqueName="CreatedOn" HeaderText="创建时间" DataField="CreatedOn" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}">
                                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn UniqueName="CreatedBy" HeaderText="创建人" DataField="CreatedBy">
                                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn UniqueName="Note" HeaderText="审核意见" DataField="Note">
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </telerik:GridBoundColumn>
                                                                </Columns>
                                                                <NoRecordsTemplate>
                                                                    没有任何数据
                                                                </NoRecordsTemplate>
                                                                <ItemStyle Height="30" />
                                                                <AlternatingItemStyle BackColor="#f2f2f2" />
                                                            </MasterTableView>
                                                            <ClientSettings EnableRowHoverStyle="true" />
                                                        </telerik:RadGrid>
                                                    </ContentTemplate>
                                                </telerik:RadDock>
                                            </telerik:RadDockZone>
                                        </telerik:RadDockLayout>

                                        <div class="mws-form-row" runat="server" id="divAudit">
                                            <label>审核意见</label>
                                            <div class="mws-form-item large">
                                                <telerik:RadTextBox runat="server" ID="txtAuditComment" Width="90%" MaxLength="1000"
                                                    TextMode="MultiLine" Height="80">
                                                </telerik:RadTextBox>
                                                <asp:CustomValidator ID="cvAuditComment" runat="server" Display="Dynamic" ErrorMessage="审核意见必填"
                                                    ControlToValidate="txtAuditComment" ValidationGroup="vgMaintenance" Text="*" CssClass="field-validation-error">
                                                </asp:CustomValidator>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="mws-button-row">
                            <asp:Button ID="btnSubmit" runat="server" Text="提交" CssClass="mws-button green" CausesValidation="true" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnAudit" runat="server" Text="审核通过" CssClass="mws-button green" CausesValidation="true" Visible="false" OnClick="btnAudit_Click" />
                            <asp:Button ID="btnReturn" runat="server" Text="退回" CssClass="mws-button orange" CausesValidation="true" Visible="false" OnClick="btnReturn_Click" />
                            <asp:Button ID="btnConfirmPay" runat="server" Text="确认支付" CssClass="mws-button green" CausesValidation="true" Visible="false" OnClick="btnConfirmPay_Click" />
                            <asp:Button ID="btnExport" runat="server" Text="导出" CssClass="mws-button green" CausesValidation="true" Visible="false" OnClick="btnExport_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Settlements/DBClientSettleBonusManagement.aspx');return false;" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnCurrentEntityID" runat="server" Value="-1" />
    <asp:HiddenField ID="hdnSaleOrderTypeID" runat="server" Value="-1" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <style>
        div#divGridCombox td, td:first-child {
            border-left-style: solid;
        }
    </style>

    <script type="text/javascript">

        var currentEntityID = -1;

        var gridClientIDs = {
            gridDBClientSettleBonus: "<%= rgDBClientSettleBonus.ClientID %>",
            gridAppPayments: "<%= rgAppPayments.ClientID %>",
        };

        function refreshGrid(gridClientID) {
            var gridObj = $find(gridClientID);

            if (gridObj)
                gridObj.get_masterTableView().rebind();
        }

        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/Settlements/DBClientSettleBonusManagement.aspx");
        }

        function openManualSettleWindow(id, actionTypeID) {
            $.showLoading();

            var targetUrl = $.getRootPath() + "Views/Settlements/Editors/ManualSettleDBClientBonus.aspx?EntityID=" + id
                + "&ManualSettleActionTypeID=" + actionTypeID + "&GridClientID=" + gridClientIDs.gridDBClientSettleBonus;

            $.openRadWindow(targetUrl, "winManualSettle", true, 400, 260);
        }

        function openAppPaymentWindow() {
            $.showLoading();

            var targetUrl = $.getRootPath() + "Views/Settlements/Editors/DBClientSettleBonusPayment.aspx?OwnerEntityID="
                + currentEntityID + "&GridClientID=" + gridClientIDs.gridAppPayments;

            $.openRadWindow(targetUrl, "winAppPayment", true, 800, 600);
        }

        function openViewAppPaymentsWindow(payDate) {
            $.showLoading();

            var targetUrl = $.getRootPath() + "Views/Settlements/Editors/ViewDBClinetBonusGroupPayments.aspx?OwnerEntityID="
                + currentEntityID + "&PayDate=" + payDate;

            $.openRadWindow(targetUrl, "winViewAppPayments", true, 800, 600);
        }

        $(document).ready(function () {
            currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

        });

    </script>
</asp:Content>
