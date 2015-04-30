<%@ Page Title="挂靠发票结算维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientAttachedInvoiceSettlementMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Invoices.ClientAttachedInvoiceSettlementMaintenance" %>

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
                    <telerik:AjaxUpdatedControl ControlID="rgClientInvoices" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgClientInvoices" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbxClientUser">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbxClientCompany" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbxClientCompany">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgClientInvoices" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgClientInvoices">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgClientInvoices" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgAppPayments">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divPaymentSummary" />
                    <telerik:AjaxUpdatedControl ControlID="rgAppPayments" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="container">

        <div class="mws-panel grid_full">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">挂靠发票结算维护</span>
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
                                <label>客户</label>
                                <div class="mws-form-item">
                                    <telerik:RadComboBox runat="server" ID="rcbxClientUser" Filter="Contains"
                                        AllowCustomText="false" Height="160px" Width="80%" EmptyMessage="--请选择--"
                                        AutoPostBack="true" OnSelectedIndexChanged="rcbxClientUser_SelectedIndexChanged">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="float-left width60-percent">
                                <label>商业单位</label>
                                <div class="mws-form-item">
                                    <telerik:RadComboBox runat="server" ID="rcbxClientCompany" Filter="Contains"
                                        AllowCustomText="false" MarkFirstMatch="true" Height="160px" Width="80%" EmptyMessage="--请选择--"
                                        AutoPostBack="true" OnSelectedIndexChanged="rcbxClientCompany_SelectedIndexChanged">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="rfvClientCompany"
                                        runat="server"
                                        ErrorMessage="请选择商业单位"
                                        ControlToValidate="rcbxClientCompany"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>实际回款</label>
                                <div class="mws-form-item small">
                                    <telerik:RadNumericTextBox runat="server" ID="txtReceiveAmount" CssClass="mws-textinput" Type="Currency"
                                        NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999"
                                        MaxLength="10" Enabled="false">
                                    </telerik:RadNumericTextBox>
                                    <asp:CustomValidator ID="cvReceiveAmount" runat="server" Display="Dynamic" ErrorMessage="实际回款金额有误"
                                        ControlToValidate="txtReceiveAmount" ValidationGroup="vgMaintenance" Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                </div>
                            </div>
                            <div class="float-left width60-percent">
                                <label>收款账户</label>
                                <div class="mws-form-item toppadding5">
                                    <telerik:RadComboBox runat="server" ID="rcbxToAccount" Filter="Contains"
                                        AllowCustomText="false" Height="160px" Width="80%" EmptyMessage="--请选择--">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="rfvToAccount"
                                        runat="server"
                                        ErrorMessage="请选择收款账户"
                                        ControlToValidate="rcbxToAccount"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>其他费用</label>
                                <div class="mws-form-item toppadding5">
                                    <telerik:RadComboBox runat="server" ID="rcbxOtherCostType" Filter="Contains"
                                        AllowCustomText="false" Height="160px" Width="80%" EmptyMessage="--请选择--">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="float-left width60-percent">
                                <label>金额</label>
                                <div class="mws-form-item small">
                                    <telerik:RadNumericTextBox runat="server" ID="txtOtherCostAmount" CssClass="mws-textinput" Type="Currency" ShowSpinButtons="true"
                                        NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="10">
                                        <ClientEvents OnValueChanging="onClientOtherCostAmountChanging" OnValueChanged="onClientOtherCostAmountChanged" />
                                    </telerik:RadNumericTextBox>
                                    <asp:CustomValidator ID="cvOtherCostAmount" runat="server" Display="Dynamic" ErrorMessage="请填写其他费用对应的金额"
                                        ControlToValidate="txtOtherCostAmount" ValidationGroup="vgMaintenance" Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>总部认款日期</label>
                                <div class="mws-form-item toppadding5">
                                    <telerik:RadDatePicker runat="server" ID="rdpConfirmDate"
                                        Calendar-EnableShadows="true"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                        Calendar-FirstDayOfWeek="Monday">
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="rfvConfirmDate"
                                        runat="server"
                                        ErrorMessage="总部认款日期必填"
                                        ControlToValidate="rdpConfirmDate"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="float-left width60-percent">
                                <label>结算日期</label>
                                <div class="mws-form-item toppadding5">
                                    <telerik:RadDatePicker runat="server" ID="rdpSettlementDate"
                                        Calendar-EnableShadows="true"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                        Calendar-FirstDayOfWeek="Monday">
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="rfvSettlementDate"
                                        runat="server"
                                        ErrorMessage="结算日期必填"
                                        ControlToValidate="rdpSettlementDate"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
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

                        <div class="mws-form-row" runat="server" id="divCancel" visible="false">
                            <label>撤销理由</label>
                            <div class="mws-form-item large">
                                <telerik:RadTextBox runat="server" ID="txtCancelComment" Width="90%" MaxLength="1000"
                                    TextMode="MultiLine" Height="80">
                                </telerik:RadTextBox>
                                <asp:CustomValidator ID="cvCancelComment" runat="server" Display="Dynamic" ErrorMessage="撤销理由必填"
                                    ControlToValidate="txtCancelComment" ValidationGroup="vgMaintenance" Text="*" CssClass="field-validation-error">
                                </asp:CustomValidator>
                            </div>
                        </div>

                        <div class="mws-form-row">
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-settlements">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">结算明细</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <table runat="server" id="tblSearch" class="leftmargin10">
                                            <tr class="height40">
                                                <td class="middle-td leftpadding10">
                                                    <label>开票日期</label>
                                                    <div class="mws-form-item">
                                                        <telerik:RadDatePicker runat="server" ID="rdpBeginDate" Width="120"
                                                            Calendar-EnableShadows="true"
                                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                                            Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                            Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                                            Calendar-FirstDayOfWeek="Monday">
                                                        </telerik:RadDatePicker>
                                                        -&nbsp;&nbsp;
                                                        <telerik:RadDatePicker runat="server" ID="rdpEndDate" Width="120"
                                                            Calendar-EnableShadows="true"
                                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                                            Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                            Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                                            Calendar-FirstDayOfWeek="Monday">
                                                        </telerik:RadDatePicker>
                                                    </div>
                                                </td>
                                                <td class="middle-td leftpadding20">
                                                    <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                                                </td>
                                                <td class="middle-td leftpadding20">
                                                    <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <telerik:RadGrid ID="rgClientInvoices" runat="server" PageSize="10" AllowCustomPaging="true"
                                            AllowPaging="false" AllowSorting="True" AutoGenerateColumns="false" AllowMultiRowSelection="true"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Height="460" ShowHeader="true" ShowFooter="false"
                                            ClientSettings-ClientEvents-OnRowMouseOver="gridInvoiceRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgClientInvoices_NeedDataSource" OnColumnCreated="rgClientInvoices_ColumnCreated"
                                            OnItemDataBound="rgClientInvoices_ItemDataBound">
                                            <MasterTableView Width="100%" DataKeyNames="ID,ClientInvoiceDetailID,StockOutDetailID,InvoiceQty,SalesPrice,InvoicePrice,InvoiceSettlementRatio" CommandItemDisplay="None"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa" ClientDataKeyNames="InvoicePrice,SalesPrice,ToBeSettlementQty">
                                                <Columns>
                                                    <telerik:GridClientSelectColumn UniqueName="ClientSelect" HeaderText="全选">
                                                        <HeaderStyle Width="40" />
                                                        <ItemStyle Width="40" />
                                                    </telerik:GridClientSelectColumn>
                                                    <telerik:GridBoundColumn UniqueName="InvoiceDate" HeaderText="开票日期" DataField="InvoiceDate" DataFormatString="{0:yyyy/MM/dd}">
                                                        <HeaderStyle Width="80" />
                                                        <ItemStyle HorizontalAlign="Left" Width="80" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="InvoiceNumber" HeaderText="发票号" DataField="InvoiceNumber">
                                                        <HeaderStyle Width="140" />
                                                        <ItemStyle HorizontalAlign="Left" Width="140" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品" DataField="ProductName">
                                                        <HeaderStyle Width="180" />
                                                        <ItemStyle HorizontalAlign="Left" Width="180" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification">
                                                        <HeaderStyle Width="60" />
                                                        <ItemStyle HorizontalAlign="Left" Width="60" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="TotalInvoiceAmount" HeaderText="发票金额" DataField="TotalInvoiceAmount" DataFormatString="{0:C2}">
                                                        <HeaderStyle Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="SalesPrice" HeaderText="销售单价" DataField="SalesPrice" DataFormatString="{0:C2}">
                                                        <HeaderStyle Width="80" />
                                                        <ItemStyle HorizontalAlign="Left" Width="80" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="InvoicePrice" HeaderText="挂靠单价" DataField="InvoicePrice" DataFormatString="{0:C2}">
                                                        <HeaderStyle Width="80" />
                                                        <ItemStyle HorizontalAlign="Left" Width="80" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="InvoiceQty" HeaderText="发票数量" DataField="InvoiceQty">
                                                        <HeaderStyle Width="80" />
                                                        <ItemStyle HorizontalAlign="Left" Width="60" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="SettledQty" HeaderText="已结算数量" DataField="SettledQty">
                                                        <HeaderStyle Width="80" />
                                                        <ItemStyle HorizontalAlign="Left" Width="60" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="ToBeSettlementQty" HeaderText="未结算数量" DataField="ToBeSettlementQty">
                                                        <HeaderStyle Width="80" />
                                                        <ItemStyle HorizontalAlign="Left" Width="80" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="SettlementQty" HeaderText="本次结算数量" DataField="SettlementQty" SortExpression="SettlementQty">
                                                        <HeaderStyle Width="100" />
                                                        <ItemStyle HorizontalAlign="Left" Width="100" />
                                                        <ItemTemplate>
                                                            <telerik:RadNumericTextBox runat="server" ID="txtSettlementQty" Type="Number" MaxLength="9" Width="80" ShowSpinButtons="true"
                                                                MinValue="1" MaxValue="99999999" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="">
                                                                <ClientEvents OnValueChanging="onClientSettlementQtyChanging" OnValueChanged="onClientSettlementQtyChanged" />
                                                            </telerik:RadNumericTextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="SettlementAmount" HeaderText="本次结算金额" DataField="SettlementAmount" SortExpression="SettlementAmount">
                                                        <HeaderStyle Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                                        <ItemTemplate>
                                                            <telerik:RadNumericTextBox runat="server" ID="txtSettlementAmount" Type="Currency" MaxLength="9" Width="100" ShowSpinButtons="true"
                                                                MinValue="1" MaxValue="99999999" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="" Enabled="false" ReadOnly="true">
                                                            </telerik:RadNumericTextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                                <NoRecordsTemplate>
                                                    没有任何数据
                                                </NoRecordsTemplate>
                                                <ItemStyle Height="30" />
                                                <AlternatingItemStyle BackColor="#f2f2f2" />
                                            </MasterTableView>
                                            <ClientSettings EnableRowHoverStyle="true">
                                                <Selecting AllowRowSelect="True" UseClientSelectColumnOnly="true" />
                                                <Scrolling AllowScroll="true" FrozenColumnsCount="3" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                <ClientEvents OnGridCreated="GetsGridObject" OnRowSelecting="onRowSelecting"
                                                    OnRowSelected="onRowSelected" OnRowDeselected="onRowDeselected" />
                                            </ClientSettings>
                                        </telerik:RadGrid>
                                        <div class="float-right">
                                            <span class="bold">结算总金额</span>：<asp:Label ID="lblTotalSettlementAmount" runat="server"></asp:Label>元&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span class="bold">大写</span>：<asp:Label ID="lblCapitalTSAmount" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span class="bold">应返款总金额</span>：<asp:Label ID="lblTotalRefundAmount" runat="server"></asp:Label>元&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span class="bold">大写</span>：<asp:Label ID="lblCapitalTRAmount" runat="server"></asp:Label>
                                        </div>
                                        <telerik:RadToolTip ID="radToolTip" runat="server" ShowEvent="FromCode" AutoCloseDelay="0">
                                            <p class="field-validation-error">当前账套未配置相应的税率, 不能加入结算</p>
                                        </telerik:RadToolTip>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="mws-form-row" runat="server" id="divOtherSections">

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
                                            OnUpdateCommand="rgAppPayments_UpdateCommand">
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
                                                    <telerik:GridTemplateColumn UniqueName="Amount" HeaderText="付款金额" DataField="Amount" SortExpression="Amount">
                                                        <HeaderStyle Width="20%" />
                                                        <ItemStyle Width="20%" />
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
                                                        <HeaderStyle Width="15%" />
                                                        <ItemStyle Width="15%" />
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

                            <!--审核 -->
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-procure-audit">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">审核信息</span>
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
                            <asp:Button ID="btnAudit" runat="server" Text="审核通过" CssClass="mws-button green" CausesValidation="true" OnClick="btnAudit_Click" />
                            <asp:Button ID="btnReturn" runat="server" Text="退回" CssClass="mws-button orange" CausesValidation="true" OnClick="btnReturn_Click" />
                            <asp:Button ID="btnPay" runat="server" Text="确认支付" CssClass="mws-button green" CausesValidation="true" OnClick="btnPay_Click" Visible="false" />
                            <asp:Button ID="btnCancel" runat="server" Text="撤销" CssClass="mws-button orange" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnCancel_Click" OnClientClick="if(confirm('您确定要撤销该笔发票结算吗？')){return true;}else{return false;}" Visible="false" />
                            <asp:Button ID="btnBack" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Invoices/ClientInvoiceSettlementManagement.aspx');return false;" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnCurrentEntityID" runat="server" Value="-1" />

    <telerik:RadNotification ID="radNotification" runat="server" EnableRoundedCorners="true"
        AutoCloseDelay="1000" Skin="Silk" Animation="Fade" EnableShadow="true" Title="提示"
        TitleIcon="none" Opacity="95" Position="Center" ContentIcon="~/Content/icons/32/cross.png"
        Width="300" Height="100">
    </telerik:RadNotification>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <style>
        div#divGridCombox td, td:first-child {
            border-left-style: solid;
        }
    </style>

    <script type="text/javascript">
        var currentEntityID = -1;

        var gridOfRefresh = null;

        function GetsGridObject(sender, eventArgs) {
            gridOfRefresh = sender;
        }

        function refreshGrid() {
            gridOfRefresh.get_masterTableView().rebind();
        }

        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/Invoices/ClientInvoiceSettlementManagement.aspx");
        }

        function refreshMaintenancePage(sender, args) {

            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            redirectToPage("Views/Invoices/ClientInvoiceSettlementMaintenance.aspx?EntityID=" + currentEntityID);
        }

        var toolTip;
        $(window).load(function () {

            toolTip = $telerik.findControl(document, "radToolTip");

            calculateGridTotalAmount();
        });

        function gridInvoiceRowMouseOver(sender, args) {

            onRowMouseOver(sender, args);

            var item = args.get_gridDataItem();

            if (item.get_selectable() == false) {

                toolTip.set_targetControl(item.get_element());

                setTimeout(function () {

                    toolTip.show();

                }, 11);

            }
            else {
                toolTip.hide();
            }
        };

        function onClientOtherCostAmountChanging(sender, eventArgs) {
            var newValue = eventArgs.get_newValue();

            if (newValue) {

                var selectedTotalAmount = getSelectedTotalAmount();

                if (newValue > selectedTotalAmount) {
                    var radNotification = $find("<%=radNotification.ClientID%>");
                    radNotification.set_text("其他费用不能大于总结算金额：" + selectedTotalAmount);
                    radNotification.show();
                    eventArgs.set_cancel(true);
                }
            }
        }

        function onClientOtherCostAmountChanged(sender, eventArgs) {
            calculateGridTotalAmount();
        }

        function onClientSettlementQtyChanging(sender, eventArgs) {
            var gridItem = sender.get_parent();

            if (gridItem) {

                var gridItemElement = gridItem.get_element();
                var newValue = eventArgs.get_newValue();

                if (newValue) {
                    var toBeSettlementQty = gridItem.getDataKeyValue("ToBeSettlementQty");
                    if (newValue > toBeSettlementQty) {
                        var radNotification = $find("<%=radNotification.ClientID%>");
                        radNotification.set_text("本次结算数量不能大于未结算数量：" + toBeSettlementQty);
                        radNotification.show();
                        eventArgs.set_cancel(true);
                    }
                }
                else {
                    var radNotification = $find("<%=radNotification.ClientID%>");
                    radNotification.set_text("本次结算数量为必填项");
                    radNotification.show();

                    eventArgs.set_cancel(true);
                }
            }
        }

        function onClientSettlementQtyChanged(sender, eventArgs) {

            var gridItem = sender.get_parent();

            if (gridItem) {

                var gridItemElement = gridItem.get_element();

                var newValue = eventArgs.get_newValue();

                if (newValue) {

                    var invoicePrice = gridItem.getDataKeyValue("InvoicePrice");

                    var txtSettlementAmount = $telerik.findControl(gridItemElement, "txtSettlementAmount");
                    var curSettlementAmount = txtSettlementAmount.get_value();

                    if (curSettlementAmount > parseFloat(newValue)) {
                        txtSettlementAmount.set_value(newValue * invoicePrice);
                    }
                }
            }

            calculateGridTotalAmount();
        }

        function onRowSelecting(sender, eventArgs) {
            var selectingItem = eventArgs.get_gridDataItem();
            var selectingElement = selectingItem.get_element();

            var txtSettlementQty = $telerik.findControl(selectingElement, "txtSettlementQty");
            var curSettlementQty = txtSettlementQty.get_value();

            if (curSettlementQty == null || curSettlementQty == "") {
                var radNotification = $find("<%=radNotification.ClientID%>");

                radNotification.set_text("本次结算数量为必填项");
                radNotification.show();

                eventArgs.set_cancel(true);
            }
            else {
                var toBeSettlementQty = parseInt(selectingItem.getDataKeyValue("ToBeSettlementQty"));

                if (toBeSettlementQty > 0 && curSettlementQty > toBeSettlementQty) {

                    var radNotification = $find("<%=radNotification.ClientID%>");

                    radNotification.set_text("本次结算数量不能大于未结算数量：" + toBeSettlementQty);
                    radNotification.show();

                    eventArgs.set_cancel(true);
                }
            }
        }

        function onRowSelected(sender, eventArgs) {
            calculateTotalAmount(sender, eventArgs);
        }

        function onRowDeselected(sender, eventArgs) {
            calculateTotalAmount(sender, eventArgs);
        }

        //计算动态操作时grid选中项的总金额
        function calculateTotalAmount(sender, eventArgs) {
            //debugger;
            var selectedTotalSettlementAmount = 0;

            //获取已经选中的items
            var selectedItems = eventArgs.get_tableView().get_selectedItems();

            for (var i = 0; i < selectedItems.length; i++) {
                var curSelectedItem = selectedItems[i];
                var curSelectedItemElement = curSelectedItem.get_element();

                var txtSettlementAmount = $telerik.findControl(curSelectedItemElement, "txtSettlementAmount");
                var curSettlementAmount = txtSettlementAmount.get_value();

                if (curSettlementAmount) {
                    selectedTotalSettlementAmount += curSettlementAmount;
                }
            }

            selectedTotalSettlementAmount = selectedTotalSettlementAmount.toFixed(2);

            $("#<%=lblTotalSettlementAmount.ClientID%>").html("¥" + selectedTotalSettlementAmount);
            $("#<%=lblCapitalTSAmount.ClientID%>").html($.convertToCapitalChinese(selectedTotalSettlementAmount));

            var calculatedReceiveAmount = selectedTotalSettlementAmount;
            var otherCostAmount = $find("<%=txtOtherCostAmount.ClientID %>").get_value();
            if (otherCostAmount && otherCostAmount != "") {
                calculatedReceiveAmount -= parseFloat(otherCostAmount);
            }
            $find("<%=txtReceiveAmount.ClientID%>").set_value(calculatedReceiveAmount);
        }

        //计算grid里选中项的总金额
        function calculateGridTotalAmount() {
            //debugger;
            var selectedTotalSettlementAmount = getSelectedTotalAmount();

            //获取已经选中的items
            var selectedItems = gridOfRefresh.get_masterTableView().get_selectedItems();

            if (selectedTotalSettlementAmount > 0) {

                selectedTotalSettlementAmount = selectedTotalSettlementAmount.toFixed(2);

                $("#<%=lblTotalSettlementAmount.ClientID%>").html("¥" + selectedTotalSettlementAmount);
                $("#<%=lblCapitalTSAmount.ClientID%>").html($.convertToCapitalChinese(selectedTotalSettlementAmount));

                var calculatedReceiveAmount = selectedTotalSettlementAmount;
                var otherCostAmount = $find("<%=txtOtherCostAmount.ClientID %>").get_value();
                if (otherCostAmount && otherCostAmount != "") {
                    calculatedReceiveAmount -= parseFloat(otherCostAmount);
                }
                $find("<%=txtReceiveAmount.ClientID%>").set_value(calculatedReceiveAmount);
            }
        }

        //获取grid里选中行的结算金额总和
        function getSelectedTotalAmount() {
            var selectedTotalSettlementAmount = 0;

            //获取已经选中的items
            var selectedItems = gridOfRefresh.get_masterTableView().get_selectedItems();

            for (var i = 0; i < selectedItems.length; i++) {
                var curSelectedItem = selectedItems[i];
                var curSelectedItemElement = curSelectedItem.get_element();

                var txtSettlementAmount = $telerik.findControl(curSelectedItemElement, "txtSettlementAmount");
                var curSettlementAmount = txtSettlementAmount.get_value();

                if (curSettlementAmount) {
                    selectedTotalSettlementAmount += curSettlementAmount;
                }
            }

            return selectedTotalSettlementAmount;
        }

    </script>
</asp:Content>

