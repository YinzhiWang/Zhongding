﻿<%@ Page Title="客户返款维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientRefundMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Refunds.ClientRefundMaintenance" %>

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
            <telerik:AjaxSetting AjaxControlID="rgOrderProducts">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgOrderProducts" LoadingPanelID="loadingPanel" />
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
                <span class="mws-i-24 i-sign-post">客户返款维护</span>
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
                                <label>订单编号</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblOrderCode" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>申请人</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblCreateBy" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>订单日期</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblOrderDate" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>订单类型</label>
                                <div class="mws-form-item small toppadding5">
                                    <asp:Label ID="lblSalesOrderType" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>客户</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblClientUser" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="float-left width60-percent">
                                <label>商业单位</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblClientCompany" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>发货模式</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblDeliveryMode" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="float-left width60-percent">
                                <label>中止执行</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblIsStop" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
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
                        <div class="mws-form-row">
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-procure-order">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">货品列表</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <telerik:RadGrid ID="rgOrderProducts" runat="server" PageSize="10"
                                            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true" ShowFooter="true"
                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgOrderProducts_NeedDataSource" OnColumnCreated="rgOrderProducts_ColumnCreated"
                                            OnItemDataBound="rgOrderProducts_ItemDataBound" OnUpdateCommand="rgOrderProducts_UpdateCommand">
                                            <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa" EditMode="InPlace">
                                                <Columns>
                                                    <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="UnitOfMeasurement" HeaderText="基本单位" DataField="UnitOfMeasurement" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Left" Width="60" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="Count" HeaderText="基本数量" DataField="Count" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Left" Width="60" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="HighPrice" HeaderText="高开价" DataField="HighPrice" DataFormatString="{0:C2}" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="ActualSalePrice" HeaderText="实际销售价" DataField="ActualSalePrice" SortExpression="ActualSalePrice"
                                                        FooterAggregateFormatString="{0:C2}">
                                                        <HeaderStyle Width="12%" />
                                                        <ItemStyle Width="12%" />
                                                        <ItemTemplate>
                                                            <span><%# Eval("ActualSalePrice","{0:C2}") %></span>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div id="divGridCombox">
                                                                <telerik:RadNumericTextBox runat="server" ID="txtActualSalePrice" ShowSpinButtons="true"
                                                                    Type="Currency" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator=""
                                                                    MinValue="0" MaxValue="999999999" MaxLength="9" Width="100%">
                                                                </telerik:RadNumericTextBox>
                                                            </div>
                                                            <asp:RequiredFieldValidator ID="rfvActualSalePrice" runat="server" ErrorMessage="实际销售价必填" CssClass="field-validation-error"
                                                                ControlToValidate="txtActualSalePrice" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn UniqueName="TotalSalesAmount" HeaderText="货款" DataField="TotalSalesAmount" DataFormatString="{0:C2}" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="ClientTaxRatio" HeaderText="税率" DataField="ClientTaxRatio" DataFormatString="{0:P2}" ReadOnly="true"
                                                        FooterText="合计：" FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="RefundAmount" HeaderText="应返款额" DataField="RefundAmount" DataFormatString="{0:C2}"
                                                        Aggregate="Sum" FooterStyle-Font-Bold="true" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridEditCommandColumn UniqueName="Edit" ButtonType="LinkButton" EditText="编辑" UpdateText="更新" CancelText="取消" HeaderStyle-Width="80">
                                                    </telerik:GridEditCommandColumn>
                                                </Columns>
                                                <CommandItemTemplate>
                                                    <table class="width100-percent">
                                                        <tr>
                                                            <td></td>
                                                            <td class="right-td rightpadding10">
                                                                <input type="button" class="rgRefresh" onclick="refreshGrid(gridClientIDs.gridOrderProducts); return false;" />
                                                                <a href="javascript:void(0);" onclick="refreshGrid(gridClientIDs.gridOrderProducts); return false;">刷新</a>
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
                                            OnNeedDataSource="rgAppPayments_NeedDataSource" OnItemDataBound="rgAppPayments_ItemDataBound"
                                            OnColumnCreated="rgAppPayments_ColumnCreated" OnInsertCommand="rgAppPayments_InsertCommand"
                                            OnUpdateCommand="rgAppPayments_UpdateCommand">
                                            <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa" EditMode="InPlace">
                                                <CommandItemSettings AddNewRecordText="添加" RefreshText="刷新" />
                                                <Columns>
                                                    <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="FromAccount" HeaderText="转出账号" DataField="FromAccount" SortExpression="FromAccount">
                                                        <ItemStyle Width="35%" />
                                                        <ItemTemplate>
                                                            <span><%# Eval("FromAccount") %></span>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div id="divGridCombox">
                                                                <telerik:RadComboBox runat="server" ID="rcbxFromAccount" Filter="Contains" AllowCustomText="false" NoWrap="true"
                                                                    MarkFirstMatch="true" Height="160px" Width="100%" EmptyMessage="--请选择--">
                                                                </telerik:RadComboBox>
                                                            </div>
                                                            <asp:RequiredFieldValidator ID="rfvFromAccount" runat="server" ErrorMessage="请选择转出账号" CssClass="field-validation-error"
                                                                ControlToValidate="rcbxFromAccount"></asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="ToAccount" HeaderText="转入客户账号" DataField="ToAccount"
                                                        SortExpression="ToAccount" FooterText="合计：" FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right">
                                                        <ItemStyle Width="35%" />
                                                        <ItemTemplate>
                                                            <span><%# Eval("ToAccount") %></span>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div id="divGridCombox">
                                                                <telerik:RadComboBox runat="server" ID="rcbxToAccount" Filter="Contains" AllowCustomText="false"
                                                                    MarkFirstMatch="true" Height="160px" Width="100%" EmptyMessage="--请选择--">
                                                                </telerik:RadComboBox>
                                                            </div>
                                                            <asp:RequiredFieldValidator ID="rfvToAccount" runat="server" ErrorMessage="请选择转入客户账号" CssClass="field-validation-error"
                                                                ControlToValidate="rcbxToAccount"></asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Amount" HeaderText="付款金额" DataField="Amount"
                                                        FooterAggregateFormatString="{0:C2}" Aggregate="Sum" FooterStyle-Font-Bold="true" SortExpression="Amount">
                                                        <HeaderStyle Width="15%" />
                                                        <ItemStyle Width="15%" />
                                                        <ItemTemplate>
                                                            <span><%# Eval("Amount","{0:C2}") %></span>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div id="divGridCombox">
                                                                <telerik:RadNumericTextBox runat="server" ID="txtAmount" ShowSpinButtons="true"
                                                                    Type="Currency" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator=""
                                                                    MinValue="0" MaxValue="999999999" MaxLength="9" Width="100%">
                                                                </telerik:RadNumericTextBox>
                                                            </div>
                                                            <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ErrorMessage="付款金额必填" CssClass="field-validation-error"
                                                                ControlToValidate="txtAmount"></asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridEditCommandColumn UniqueName="Edit" ButtonType="LinkButton" InsertText="保存" EditText="编辑" UpdateText="更新" CancelText="取消" HeaderStyle-Width="80">
                                                    </telerik:GridEditCommandColumn>
                                                </Columns>
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
                            <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                            <asp:Button ID="btnSubmit" runat="server" Text="提交" CssClass="mws-button green" CausesValidation="true" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnAudit" runat="server" Text="审核通过" CssClass="mws-button green" CausesValidation="true" Visible="false" OnClick="btnAudit_Click" />
                            <asp:Button ID="btnReturn" runat="server" Text="退回" CssClass="mws-button orange" CausesValidation="true" Visible="false" OnClick="btnReturn_Click" />
                            <asp:Button ID="btnPay" runat="server" Text="确认支付" CssClass="mws-button green" CausesValidation="true" OnClick="btnPay_Click" Visible="false" />
                            <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Refunds/ClientRefundManagement.aspx');return false;" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnCurrentEntityID" runat="server" Value="-1" />
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
            gridOrderProducts: "<%= rgOrderProducts.ClientID %>",
            gridAppPayments: "<%= rgAppPayments.ClientID %>",
        };

        function refreshGrid(gridClientID) {
            var gridObj = $find(gridClientID);

            if (gridObj)
                gridObj.get_masterTableView().rebind();
        }

        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/Refunds/ClientRefundManagement.aspx");
        }

        function refreshMaintenancePage(sender, args) {

            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            redirectToPage("Views/Refunds/ClientRefundMaintenance.aspx?EntityID=" + currentEntityID);
        }

        function openAppPaymentWindow(id, paymentMethodID) {
            $.showLoading();

            var targetUrl = $.getRootPath() + "Views/Sales/Editors/ClientOrderPaymentMaintain.aspx?EntityID=" + id
                + "&OwnerEntityID=" + currentEntityID + "&PaymentMethodID=" + paymentMethodID
                + "&GridClientID=" + gridClientIDs.gridAppPayments;

            $.openRadWindow(targetUrl, "winCleintOrderPayment", true, 800, 440);
        }

        $(document).ready(function () {
            currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();
        });

    </script>
</asp:Content>


