<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SupplierCautionMoneyMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.CautionMoneys.SupplierCautionMoneyMaintenance" %>


<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <style>
        div#divGridCombox td, td:first-child {
            border-left-style: solid;
        }
    </style>
     <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            
            <telerik:AjaxSetting AjaxControlID="rgAppPayments">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgAppPayments" LoadingPanelID="loadingPanel" />
                     <telerik:AjaxUpdatedControl ControlID="divPaymentSummary" LoadingPanelID="loadingPanel" />
                    
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgSupplierRefunds">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgSupplierRefunds" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="hdnNeedRefreshPage" />
                    <telerik:AjaxUpdatedControl ControlID="hdnCurrentEntityID" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSearchRefund">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearchRefund" />
                    <telerik:AjaxUpdatedControl ControlID="rgSupplierRefunds" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="hdnNeedRefreshPage" />
                    <telerik:AjaxUpdatedControl ControlID="hdnCurrentEntityID" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgSupplierDeduction">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgSupplierDeduction" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="hdnNeedRefreshPage" />
                    <telerik:AjaxUpdatedControl ControlID="hdnCurrentEntityID" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSearchDeduction">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearchDeduction" />
                    <telerik:AjaxUpdatedControl ControlID="rgSupplierDeduction" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="hdnNeedRefreshPage" />
                    <telerik:AjaxUpdatedControl ControlID="hdnCurrentEntityID" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
        <ClientEvents OnResponseEnd="onResponseEnd" />
    </telerik:RadAjaxManager>
    <div class="container">

        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">供应商保证金申请维护</span>
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
                                <label>申请日期</label>
                                <div class="mws-form-item small">
                                    <telerik:RadDatePicker ID="rdpApplyDate" Width="200px" runat="server">
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="rfvApplyDate" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="rdpApplyDate"
                                        ErrorMessage="申请日期必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                    <telerik:RadToolTip ID="rttApplyDate" runat="server" TargetControlID="rdpApplyDate" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                </div>

                            </div>
                            <div class="float-left">

                                <asp:Label ID="lblOperator" runat="server" Text="操作人：" />
                            </div>
                        </div>

                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>货品名称</label>
                                <div class="mws-form-item small">
                                    <telerik:RadComboBox runat="server" ID="rcbxProduct" Filter="Contains" AllowCustomText="true"
                                        Width="200px"
                                        MarkFirstMatch="true" Height="200px" EmptyMessage="--请选择--" AutoPostBack="true" OnSelectedIndexChanged="rcbxProduct_SelectedIndexChanged">
                                    </telerik:RadComboBox>
                                    <telerik:RadToolTip ID="rttProductName" runat="server" TargetControlID="rcbxProduct" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                    <asp:CustomValidator ID="cvProductName" runat="server" ErrorMessage="请选择货品名称"
                                        ControlToValidate="rcbxProduct" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                </div>

                            </div>
                            <div class="float-left">
                                <label>规格</label>
                                <div class="mws-form-item small">
                                    <telerik:RadComboBox runat="server" ID="rcbxProductSpecification" Filter="Contains" AllowCustomText="true"
                                        Width="200px"
                                        MarkFirstMatch="true" Height="200px" EmptyMessage="--请选择--">
                                    </telerik:RadComboBox>
                                    <telerik:RadToolTip ID="rttProductSpecification" runat="server" TargetControlID="rcbxProductSpecification" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                    <asp:CustomValidator ID="cvProductSpecification" runat="server" ErrorMessage="请选择规格"
                                        ControlToValidate="rcbxProductSpecification" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>供应商</label>
                                <div class="mws-form-item small">
                                    <telerik:RadComboBox runat="server" ID="rcbxSupplier" Filter="Contains" AllowCustomText="true"
                                        Width="200px"
                                        MarkFirstMatch="true" Height="200px" EmptyMessage="--请选择--">
                                    </telerik:RadComboBox>
                                    <telerik:RadToolTip ID="rttSupplier" runat="server" TargetControlID="rcbxSupplier" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                    <asp:CustomValidator ID="cvSupplier" runat="server" ErrorMessage="请选择供应商"
                                        ControlToValidate="rcbxSupplier" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                </div>

                            </div>
                            <div class="float-left">
                                <label>保证金金额</label>
                                <div class="mws-form-item small">
                                    <telerik:RadNumericTextBox ShowSpinButtons="true" MinValue="0" IncrementSettings-InterceptArrowKeys="true" IncrementSettings-InterceptMouseWheel="true"
                                        Label="" runat="server" ID="txtPaymentCautionMoney" Width="200px">
                                    </telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="rfvPaymentCautionMoney" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtPaymentCautionMoney"
                                        ErrorMessage="保证金金额必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                    <telerik:RadToolTip ID="rttPaymentCautionMoney" runat="server" TargetControlID="txtPaymentCautionMoney" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                </div>
                            </div>
                        </div>


                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>保证金类别</label>
                                <div class="mws-form-item small">
                                    <telerik:RadComboBox runat="server" ID="rcbxCautionMoneyType" Filter="Contains" AllowCustomText="true"
                                        Width="200px"
                                        MarkFirstMatch="true" Height="200px" EmptyMessage="--请选择--">
                                    </telerik:RadComboBox>
                                    <telerik:RadToolTip ID="rttCautionMoneyType" runat="server" TargetControlID="rcbxCautionMoneyType" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                    <asp:CustomValidator ID="cvCautionMoneyType" runat="server" ErrorMessage="请选择保证金类别"
                                        ControlToValidate="rcbxCautionMoneyType" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                </div>

                            </div>
                            <div class="float-left">
                                <label>保证金终止日期</label>
                                <div class="mws-form-item small">
                                    <telerik:RadDatePicker ID="rdpEndDate" Width="200px" runat="server">
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="rdpEndDate"
                                        ErrorMessage="保证金终止日期必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                    <telerik:RadToolTip ID="rttEndDate" runat="server" TargetControlID="rdpEndDate" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                </div>
                            </div>
                        </div>


                        <div class="mws-form-row">
                            <label>备注</label>
                            <div class="mws-form-item medium">
                                <telerik:RadTextBox runat="server" ID="txtRemark" Width="90%" MaxLength="1000"
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
                        <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-procure-audit" runat="server" id="divAuditAll">
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
                        <!--支付信息维护 -->
                        <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-payment" runat="server" id="divAppPayments">
                            <div class="mws-panel-header">
                                <span class="mws-i-24 i-creditcard">支付信息维护</span>
                            </div>
                            <div class="mws-panel-body">
                                <div class="mws-panel-content">
                                    <telerik:RadGrid ID="rgAppPayments" runat="server" PageSize="10"
                                        AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                        MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true" ShowFooter="false"
                                        ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                        OnNeedDataSource="rgAppPayments_NeedDataSource" OnItemDataBound="rgAppPayments_ItemDataBound"
                                        OnColumnCreated="rgAppPayments_ColumnCreated" OnInsertCommand="rgAppPayments_InsertCommand"
                                        OnUpdateCommand="rgAppPayments_UpdateCommand" OnDeleteCommand="rgAppPayments_DeleteCommand">
                                        <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                            ShowHeadersWhenNoRecords="true" BackColor="#fafafa" EditMode="InPlace">
                                            <CommandItemSettings AddNewRecordText="添加" RefreshText="刷新" />
                                            <Columns>
                                                <telerik:GridTemplateColumn UniqueName="PayDate" HeaderText="转账日期" DataField="PayDate" SortExpression="PayDate">
                                                    <HeaderStyle Width="10%" />
                                                    <ItemStyle Width="10%" />
                                                    <ItemTemplate>
                                                        <span><%# Eval("PayDate","{0:yyyy/MM/dd}") %></span>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <div id="divGridCombox">
                                                            <telerik:RadDatePicker runat="server" ID="rdpPayDate"
                                                                Calendar-EnableShadows="true"
                                                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                                Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                                                Calendar-FirstDayOfWeek="Monday">
                                                            </telerik:RadDatePicker>
                                                        </div>
                                                        <asp:RequiredFieldValidator ID="rfvPayDate" runat="server" ErrorMessage="请选择转账日期" CssClass="field-validation-error"
                                                            ControlToValidate="rdpPayDate"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="FromAccount" HeaderText="转出账号" DataField="FromAccount" SortExpression="FromAccount">

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
                                                <telerik:GridTemplateColumn UniqueName="ToAccount" HeaderText="收款账号" DataField="ToAccount"
                                                    SortExpression="ToAccount" FooterText="合计：" FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right">

                                                    <ItemTemplate>
                                                        <span><%# Eval("ToAccount") %></span>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <div id="divGridCombox">
                                                            <telerik:RadComboBox runat="server" ID="rcbxToAccount" Filter="Contains" AllowCustomText="false"
                                                                MarkFirstMatch="true" Height="160px" Width="100%" EmptyMessage="--请选择--">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                        <asp:RequiredFieldValidator ID="rfvToAccount" runat="server" ErrorMessage="请选择收款账号" CssClass="field-validation-error"
                                                            ControlToValidate="rcbxToAccount" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="Amount" HeaderText="付款金额" DataField="Amount" SortExpression="Amount">
                                                    <HeaderStyle Width="100px" />
                                                    <ItemStyle Width="100px" />
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
                                                <telerik:GridTemplateColumn UniqueName="Fee" HeaderText="手续费" DataField="Fee" SortExpression="Fee">
                                                    <HeaderStyle Width="100px" />
                                                    <ItemStyle Width="100px" />
                                                    <ItemTemplate>
                                                        <span><%# Eval("Fee","{0:C2}") %></span>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <div id="divGridCombox">
                                                            <telerik:RadNumericTextBox runat="server" ID="txtFee" ShowSpinButtons="true"
                                                                Type="Currency" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator=""
                                                                MinValue="0" MaxValue="999999999" MaxLength="9" Width="100%">
                                                            </telerik:RadNumericTextBox>
                                                        </div>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridEditCommandColumn UniqueName="Edit" ButtonType="LinkButton" InsertText="保存" EditText="编辑" UpdateText="更新" CancelText="取消">
                                                    <HeaderStyle Width="10%" />
                                                    <ItemStyle Width="10%" />
                                                </telerik:GridEditCommandColumn>
                                                <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="60" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />

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
                                    <div class="float-right" runat="server" id="divPaymentSummary">
                                        <span class="bold">支付总金额</span>：<asp:Label ID="lblTotalPaymentAmount" runat="server"></asp:Label>元&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span class="bold">大写</span>：<asp:Label ID="lblCapitalTotalPaymentAmount" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row" runat="server" id="divRefund">
                            <telerik:RadTabStrip ID="tabStripRefunds" runat="server" MultiPageID="multiPageRefunds" Skin="Default">
                                <Tabs>
                                    <telerik:RadTab Text="返款记录" Value="tabRefund" PageViewID="pvRefund" Selected="true"></telerik:RadTab>
                                    <telerik:RadTab Text="抵扣记录" Value="tabDeduction" PageViewID="pvDeduction"></telerik:RadTab>
                                </Tabs>
                            </telerik:RadTabStrip>
                            <telerik:RadMultiPage ID="multiPageRefunds" runat="server" CssClass="multi-page-wrapper">
                                <telerik:RadPageView ID="pvRefund" runat="server" Selected="true">
                                    <%--返款--%>
                                    <table runat="server" id="tblSearchRefund" class="leftmargin10">
                                        <tr class="height40">
                                            <th class="middle-td">返款日期：
                                            </th>
                                            <td class="middle-td leftpadding10">
                                                <telerik:RadDatePicker runat="server" ID="rdpRefundBeginDate" Width="100"
                                                    Calendar-EnableShadows="true"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                                    Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                                    Calendar-FastNavigationSettings-DisableOutOfRangeMonths="true"
                                                    Calendar-FirstDayOfWeek="Monday">
                                                </telerik:RadDatePicker>
                                                -&nbsp;&nbsp;
                                                <telerik:RadDatePicker runat="server" ID="rdpRefundEndDate" Width="100"
                                                    Calendar-EnableShadows="true"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                                    Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                                    Calendar-FastNavigationSettings-DisableOutOfRangeMonths="true"
                                                    Calendar-FirstDayOfWeek="Monday">
                                                </telerik:RadDatePicker>
                                            </td>
                                            <td class="middle-td leftpadding20">
                                                <asp:Button ID="btnSearchRefund" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearchRefund_Click" />
                                            </td>
                                            <td class="middle-td leftpadding20">
                                                <asp:Button ID="btnResetRefund" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnResetRefund_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <telerik:RadGrid ID="rgSupplierRefunds" runat="server" PageSize="10"
                                        AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                        MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                        ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                        OnNeedDataSource="rgSupplierRefunds_NeedDataSource" OnItemDataBound="rgSupplierRefunds_ItemDataBound"
                                        OnItemCommand="rgSupplierRefunds_ItemCommand">
                                        <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                            ShowHeadersWhenNoRecords="true" BackColor="#fafafa" EditMode="InPlace">
                                            <CommandItemSettings AddNewRecordText="添加" RefreshText="刷新" />
                                            <Columns>
                                                <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="PayDate" HeaderText="返款日期" DataField="PayDate" SortExpression="FromAccount">
                                                    <ItemStyle Width="10%" />
                                                    <ItemTemplate>
                                                        <span><%# Eval("PayDate","{0:yyyy/MM/dd}") %></span>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <div id="divGridCombox">
                                                            <telerik:RadDatePicker runat="server" ID="rdpPayDate" Width="100"
                                                                Calendar-EnableShadows="true"
                                                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                                Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                                                Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                                                Calendar-FastNavigationSettings-DisableOutOfRangeMonths="true"
                                                                Calendar-FirstDayOfWeek="Monday">
                                                            </telerik:RadDatePicker>
                                                        </div>
                                                        <asp:RequiredFieldValidator ID="rfvPayDate" runat="server" ErrorMessage="请选择返款日期" CssClass="field-validation-error"
                                                            ControlToValidate="rdpPayDate" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="ToAccount" HeaderText="收款账号" DataField="ToAccount"
                                                    SortExpression="ToAccount" FooterText="合计：" FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right">
                                                    <ItemStyle Width="30%" />
                                                    <ItemTemplate>
                                                        <span><%# Eval("ToAccount") %></span>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <div id="divGridCombox">
                                                            <telerik:RadComboBox runat="server" ID="rcbxToAccount" Filter="Contains" AllowCustomText="false"
                                                                MarkFirstMatch="true" Height="160px" Width="100%" EmptyMessage="--请选择--">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                        <asp:RequiredFieldValidator ID="rfvToAccount" runat="server" ErrorMessage="请选择收款账号" CssClass="field-validation-error"
                                                            ControlToValidate="rcbxToAccount" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="Amount" HeaderText="返款金额" DataField="Amount"
                                                    FooterAggregateFormatString="{0:C2}" Aggregate="Sum" FooterStyle-Font-Bold="true" SortExpression="Amount">
                                                    <HeaderStyle Width="12%" />
                                                    <ItemStyle Width="12%" />
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
                                                        <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ErrorMessage="返款金额必填" CssClass="field-validation-error"
                                                            ControlToValidate="txtAmount" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <br />
                                                        <asp:CustomValidator ID="cvAmount" runat="server" ControlToValidate="txtAmount" CssClass="field-validation-error" Display="Dynamic"
                                                            ErrorMessage="返款(含手续费)和抵扣之和不能大于应返款总额" ></asp:CustomValidator>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="Fee" HeaderText="手续费" DataField="Fee"
                                                    FooterAggregateFormatString="{0:C2}" Aggregate="Sum" FooterStyle-Font-Bold="true" SortExpression="Fee">
                                                    <HeaderStyle Width="10%" />
                                                    <ItemStyle Width="10%" />
                                                    <ItemTemplate>
                                                        <span><%# Eval("Fee","{0:C2}") %></span>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <div id="divGridCombox">
                                                            <telerik:RadNumericTextBox runat="server" ID="txtFee" ShowSpinButtons="true"
                                                                Type="Currency" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator=""
                                                                MinValue="0" MaxValue="999999999" MaxLength="9" Width="100%">
                                                            </telerik:RadNumericTextBox>
                                                            <br />
                                                            <asp:CustomValidator ID="cvFee" runat="server" ControlToValidate="txtFee" CssClass="field-validation-error" Display="Dynamic"
                                                                ErrorMessage="返款(含手续费)和抵扣之和不能大于应返款总额" ></asp:CustomValidator>
                                                        </div>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn UniqueName="CreatedBy" HeaderText="操作人" DataField="CreatedBy" ReadOnly="true">
                                                    <HeaderStyle Width="80" />
                                                    <ItemStyle HorizontalAlign="Left" Width="80" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="Comment" HeaderText="备注" DataField="Comment" SortExpression="Comment">
                                                    <HeaderStyle Width="20%" />
                                                    <ItemStyle Width="20%" />
                                                    <ItemTemplate>
                                                        <span><%# Eval("Comment") %></span>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <div id="divGridCombox">
                                                            <telerik:RadTextBox runat="server" ID="txtComment" MaxLength="500" Width="100%"></telerik:RadTextBox>
                                                        </div>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn>
                                                    <HeaderStyle Width="80" />
                                                    <ItemStyle Width="80" />
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lbtnInsert" runat="server" CommandName="Insert">保存</asp:LinkButton>
                                                        &nbsp;
                                                        <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CausesValidation="false">取消</asp:LinkButton>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <%--//--%>
                                                    <%--     <telerik:GridEditCommandColumn UniqueName="Edit" ButtonType="LinkButton" InsertText="保存" EditText="编辑" UpdateText="更新" CancelText="取消">
                                                    <HeaderStyle Width="10%" />
                                                    <ItemStyle Width="10%" />
                                                </telerik:GridEditCommandColumn>--%>
                                                <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="60" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />

                                                <%--//--%>
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
                                </telerik:RadPageView>
                                <telerik:RadPageView ID="pvDeduction" runat="server">
                                    <%--抵扣--%>
                                    <table runat="server" id="tblSearchDeduction" class="leftmargin10">
                                        <tr class="height40">
                                            <th class="middle-td">抵扣日期：
                                            </th>
                                            <td class="middle-td leftpadding10">
                                                <telerik:RadDatePicker runat="server" ID="rdpDeductionBeginDate" Width="100"
                                                    Calendar-EnableShadows="true"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                                    Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                                    Calendar-FastNavigationSettings-DisableOutOfRangeMonths="true"
                                                    Calendar-FirstDayOfWeek="Monday">
                                                </telerik:RadDatePicker>
                                                -&nbsp;&nbsp;
                                                <telerik:RadDatePicker runat="server" ID="rdpDeductionEndDate" Width="100"
                                                    Calendar-EnableShadows="true"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                                    Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                                    Calendar-FastNavigationSettings-DisableOutOfRangeMonths="true"
                                                    Calendar-FirstDayOfWeek="Monday">
                                                </telerik:RadDatePicker>
                                            </td>
                                            <td class="middle-td leftpadding20">
                                                <asp:Button ID="btnSearchDeduction" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearchDeduction_Click" />
                                            </td>
                                            <td class="middle-td leftpadding20">
                                                <asp:Button ID="btnResetDeduction" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnResetDeduction_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <telerik:RadGrid ID="rgSupplierDeduction" runat="server" PageSize="10"
                                        AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                        MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                        ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                        OnNeedDataSource="rgSupplierDeduction_NeedDataSource" OnItemDataBound="rgSupplierDeduction_ItemDataBound"
                                        OnItemCommand="rgSupplierDeduction_ItemCommand">
                                        <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                            ShowHeadersWhenNoRecords="true" BackColor="#fafafa" EditMode="InPlace">
                                            <CommandItemSettings AddNewRecordText="添加" RefreshText="刷新" />
                                            <Columns>
                                                <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="DeductedDate" HeaderText="抵扣日期" DataField="DeductedDate" SortExpression="FromAccount">
                                                    <ItemStyle Width="10%" />
                                                    <ItemTemplate>
                                                        <span><%# Eval("DeductedDate","{0:yyyy/MM/dd}") %></span>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <div id="divGridCombox">
                                                            <telerik:RadDatePicker runat="server" ID="rdpDeductedDate" Width="100"
                                                                Calendar-EnableShadows="true"
                                                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                                Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                                                Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                                                Calendar-FastNavigationSettings-DisableOutOfRangeMonths="true"
                                                                Calendar-FirstDayOfWeek="Monday">
                                                            </telerik:RadDatePicker>
                                                        </div>
                                                        <asp:RequiredFieldValidator ID="rfvDeductedDate" runat="server" ErrorMessage="请选择抵扣日期" CssClass="field-validation-error"
                                                            ControlToValidate="rdpDeductedDate" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="Amount" HeaderText="抵扣金额" DataField="Amount"
                                                    FooterAggregateFormatString="{0:C2}" Aggregate="Sum" FooterStyle-Font-Bold="true" SortExpression="Amount">
                                                    <HeaderStyle Width="12%" />
                                                    <ItemStyle Width="12%" />
                                                    <ItemTemplate>
                                                        <span><%# Eval("Amount","{0:C2}") %></span>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <div id="divGridCombox">
                                                            <telerik:RadNumericTextBox runat="server" ID="txtAmount" ShowSpinButtons="true"
                                                                Type="Currency" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator=""
                                                                MinValue="-999999999" MaxValue="999999999" MaxLength="9" Width="100%">
                                                            </telerik:RadNumericTextBox>
                                                        </div>
                                                        <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ErrorMessage="抵扣金额必填" CssClass="field-validation-error"
                                                            ControlToValidate="txtAmount" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <br />
                                                        <asp:CustomValidator ID="cvAmount" runat="server" ControlToValidate="txtAmount" CssClass="field-validation-error" Display="Dynamic"
                                                            ErrorMessage="抵扣和返款(含手续费)之和不能大于应返款总额" ></asp:CustomValidator>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="SupplierName" HeaderText="抵扣供应商" DataField="SupplierName" SortExpression="SupplierName">
                                                    <ItemStyle Width="20%" />
                                                    <ItemTemplate>
                                                        <span><%# Eval("SupplierName") %></span>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <div id="divGridCombox">
                                                            <telerik:RadComboBox runat="server" ID="rcbxSupplier" Filter="Contains" AllowCustomText="false"
                                                                MarkFirstMatch="true" Height="160px" Width="100%" EmptyMessage="--请选择--">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                        <asp:RequiredFieldValidator ID="rfvSupplier" runat="server" ErrorMessage="请选择抵扣供应商" CssClass="field-validation-error"
                                                            ControlToValidate="rcbxSupplier" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn UniqueName="CreatedBy" HeaderText="操作人" DataField="CreatedBy" ReadOnly="true">
                                                    <HeaderStyle Width="80" />
                                                    <ItemStyle HorizontalAlign="Left" Width="80" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="Comment" HeaderText="备注" DataField="Comment" SortExpression="Comment">
                                                    <HeaderStyle Width="20%" />
                                                    <ItemStyle Width="20%" />
                                                    <ItemTemplate>
                                                        <span><%# Eval("Comment") %></span>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <div id="divGridCombox">
                                                            <telerik:RadTextBox runat="server" ID="txtComment" MaxLength="500" Width="100%"></telerik:RadTextBox>
                                                        </div>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn>
                                                    <HeaderStyle Width="80" />
                                                    <ItemStyle Width="80" />
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lbtnInsert" runat="server" CommandName="Insert">保存</asp:LinkButton>
                                                        &nbsp;
                                                        <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CausesValidation="false">取消</asp:LinkButton>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                 <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="60" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />

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
                                </telerik:RadPageView>
                            </telerik:RadMultiPage>
                        </div>

                        <div class="height20"></div>
                    </div>
                    <div class="mws-button-row">
                        <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                        <asp:Button ID="btnSubmit" runat="server" Text="提交" CssClass="mws-button green" CausesValidation="true" OnClick="btnSubmit_Click" />
                        <asp:Button ID="btnAudit" runat="server" Text="审核通过" CssClass="mws-button green" CausesValidation="true" Visible="false" OnClick="btnAudit_Click" />
                        <asp:Button ID="btnReturn" runat="server" Text="退回" CssClass="mws-button orange" CausesValidation="true" Visible="false" OnClick="btnReturn_Click" />
                        <asp:Button ID="btnPay" runat="server" Text="确认支付" CssClass="mws-button green" CausesValidation="true" OnClick="btnPay_Click" Visible="false" />

                        <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/CautionMoneys/SupplierCautionMoneyApplyManagement.aspx');return false;" />
                    </div>
                </div>
            </div>
        </div>

    </div>
    <asp:HiddenField ID="hdnCurrentEntityID" runat="server" Value="-1" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">
        var gridClientIDs = {

        };

        function refreshGrid(gridClientID) {
            var gridObj = $find(gridClientID);

            if (gridObj)
                gridObj.get_masterTableView().rebind();
        }

        function onClientHidden(sender, args) {
            redirectToPage('Views/CautionMoneys/SupplierCautionMoneyApplyManagement.aspx');
        }

        function onClientBlur(sender, args) {

        }
        function onBtnCancelClick() {

            redirectToPage('Views/CautionMoneys/SupplierCautionMoneyApplyManagement.aspx');
        }

        function redirectToManagementPage() {
            $.showLoading();

            window.location.href = "SupplierCautionMoneyApplyManagement.aspx";
        }
        function refreshMaintenancePage(sender, args) {

            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            redirectToPage("Views/CautionMoneys/SupplierCautionMoneyMaintenance.aspx?EntityID=" + currentEntityID);
         }
        function onResponseEnd(sender, args) {

            var eventTarget = args.get_eventTarget();

            if ((eventTarget.indexOf("rgSupplierRefunds") >= 0
                    || eventTarget.indexOf("rgSupplierDeduction") >= 0)
                && (eventTarget.indexOf("lbtnInsert") >= 0)) {

            
                }
            
        }
    </script>
</asp:Content>
