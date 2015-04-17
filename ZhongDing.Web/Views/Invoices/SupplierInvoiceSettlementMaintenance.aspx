<%@ Page Title="供应商发票结算维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SupplierInvoiceSettlementMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Invoices.SupplierInvoiceSettlementMaintenance" %>

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
                    <telerik:AjaxUpdatedControl ControlID="rgSupplierInvoices" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgSupplierInvoices" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="rgSupplierInvoices">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgSupplierInvoices" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="container">

        <div class="mws-panel grid_full">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">供应商发票结算维护</span>
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
                                    <telerik:RadDatePicker runat="server" ID="rdpSettlementDate" AutoPostBack="true"
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
                                <label>账套</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>发票返点比例</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblTaxRatio" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="float-left width60-percent">
                                <label>入账账号</label>
                                <div class="mws-form-item toppadding5">
                                    <telerik:RadComboBox runat="server" ID="rcbxToAccount" Filter="Contains"
                                        AllowCustomText="false" Height="160px" Width="80%" EmptyMessage="--请选择--">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="rfvToAccount"
                                        runat="server"
                                        ErrorMessage="请选择入账账号"
                                        ControlToValidate="rcbxToAccount"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row" runat="server" id="divCancel">
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
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-1">
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
                                        <telerik:RadGrid ID="rgSupplierInvoices" runat="server" PageSize="10" AllowCustomPaging="true"
                                            AllowPaging="false" AllowSorting="True" AutoGenerateColumns="false" AllowMultiRowSelection="true"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" Height="380" ShowHeader="true" ShowFooter="false"
                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgSupplierInvoices_NeedDataSource" OnColumnCreated="rgSupplierInvoices_ColumnCreated">
                                            <MasterTableView Width="100%" DataKeyNames="ID,SupplierInvoiceID,SupplierID" CommandItemDisplay="None"
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
                                                        <HeaderStyle Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="SupplierName" HeaderText="供应商" DataField="SupplierName">
                                                        <HeaderStyle Width="180" />
                                                        <ItemStyle HorizontalAlign="Left" Width="180" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="InvoiceAmount" HeaderText="发票金额" DataField="InvoiceAmount" DataFormatString="{0:C2}">
                                                        <HeaderStyle Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="PayAmount" HeaderText="结算金额" DataField="PayAmount" DataFormatString="{0:C2}">
                                                        <HeaderStyle Width="120" />
                                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                                <CommandItemTemplate>
                                                    <table class="width100-percent">
                                                        <tr>
                                                            <td></td>
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
                                            </MasterTableView>
                                            <ClientSettings EnableRowHoverStyle="true">
                                                <Selecting AllowRowSelect="True" />
                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" />
                                                <ClientEvents OnGridCreated="GetsGridObject" />
                                            </ClientSettings>
                                            <HeaderStyle Width="99.8%" />
                                        </telerik:RadGrid>
                                        <div class="float-right">
                                            <span class="bold">结算总金额</span>：<asp:Label ID="lblTotalPayAmount" runat="server"></asp:Label>元&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span class="bold">大写</span>：<asp:Label ID="lblCapitalAmount" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="mws-button-row">
                            <asp:Button ID="btnSubmit" runat="server" Text="结算" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="撤销" CssClass="mws-button orange" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnCancel_Click" />
                            <asp:Button ID="btnBack" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Invoices/SupplierInvoiceSettlementManagement.aspx');return false;" />
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

        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/Invoices/SupplierInvoiceSettlementManagement.aspx");
        }

    </script>
</asp:Content>
