<%@ Page Title="客户发票结算维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientInvoiceSettlementMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Invoices.ClientInvoiceSettlementMaintenance" %>

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
                <span class="mws-i-24 i-sign-post">客户发票结算维护</span>
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
                            <div class="float-left">
                                <label>操作人</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblCreateBy" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>商业单位</label>
                            <div class="mws-form-item">
                                <telerik:RadComboBox runat="server" ID="rcbxClientCompany" Filter="Contains"
                                    AllowCustomText="false" MarkFirstMatch="true" Height="160px" Width="60%" EmptyMessage="--请选择--"
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
                                                        <telerik:RadDatePicker runat="server" ID="rdpBeginDate" Width="120"></telerik:RadDatePicker>
                                                        -&nbsp;&nbsp;
                                                        <telerik:RadDatePicker runat="server" ID="rdpEndDate" Width="120"></telerik:RadDatePicker>
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
                                            OnNeedDataSource="rgClientInvoices_NeedDataSource"
                                            OnColumnCreated="rgClientInvoices_ColumnCreated" OnItemDataBound="rgClientInvoices_ItemDataBound">
                                            <MasterTableView Width="100%" DataKeyNames="ID,ClientInvoiceID,ClientCompanyID,InvoiceDate,InvoiceNumber,TotalInvoiceAmount,ClientTaxHighRatio,ClientTaxLowRatio,ClientTaxDeductionRatio,HighRatioAmount,LowRatioAmount,DeductionRatioAmount,PayAmount" CommandItemDisplay="None"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
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
                                                    <telerik:GridBoundColumn UniqueName="ClientCompanyName" HeaderText="商业单位" DataField="ClientCompanyName">
                                                        <HeaderStyle Width="240" />
                                                        <ItemStyle HorizontalAlign="Left" Width="240" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="TotalInvoiceAmount" HeaderText="发票总金额" DataField="TotalInvoiceAmount" DataFormatString="{0:C2}">
                                                        <HeaderStyle Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="HighRatioAmount" HeaderText="高开金额" DataField="HighRatioAmount" DataFormatString="{0:C2}">
                                                        <HeaderStyle Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="LowRatioAmount" HeaderText="低开金额" DataField="LowRatioAmount" DataFormatString="{0:C2}">
                                                        <HeaderStyle Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="DeductionRatioAmount" HeaderText="平进平出金额" DataField="DeductionRatioAmount" DataFormatString="{0:C2}">
                                                        <HeaderStyle Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="PayAmount" HeaderText="结算金额" DataField="PayAmount" DataFormatString="{0:C2}">
                                                        <HeaderStyle Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                                    </telerik:GridBoundColumn>
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


                                            </ClientSettings>
                                        </telerik:RadGrid>
                                        <div class="float-right">
                                            <span class="bold">结算总金额</span>：<asp:Label ID="lblTotalPayAmount" runat="server"></asp:Label>元&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span class="bold">大写</span>：<asp:Label ID="lblCapitalAmount" runat="server"></asp:Label>
                                        </div>
                                        <telerik:RadToolTip ID="radToolTip" runat="server" ShowEvent="FromCode" AutoCloseDelay="0">
                                            <p class="field-validation-error">当前账套未启用平进平出或未配置平进平出税率, 不能加入结算</p>
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <style>
        div#divGridCombox td, td:first-child {
            border-left-style: solid;
        }
    </style>

    <script type="text/javascript">
        var currentEntityID = -1;

        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/Invoices/ClientInvoiceSettlementManagement.aspx");
        }

        function refreshMaintenancePage(sender, args) {

            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            redirectToPage("Views/Invoices/ClientInvoiceSettlementMaintenance.aspx?EntityID=" + currentEntityID);
        }

        var toolTip;

        window.onload = function () {
            toolTip = $telerik.findControl(document, "radToolTip");
        };

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

    </script>
</asp:Content>
