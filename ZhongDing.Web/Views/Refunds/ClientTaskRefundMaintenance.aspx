<%@ Page Title="客户奖励返款维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientTaskRefundMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Refunds.ClientTaskRefundMaintenance" %>

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

            <telerik:AjaxSetting AjaxControlID="rcbxCompany">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divFormContent" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbxProduct">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divFormContent" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlProductSpecification">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divFormContent" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rmypRefundDate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divFormContent" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbxClientUser">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divFormContent" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbxClientCompany">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divFormContent" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="txtRefundPrice">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divFormContent" LoadingPanelID="loadingPanel" />
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
                <span class="mws-i-24 i-sign-post">客户奖励返款维护</span>
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
                            <div class="float-left width50-percent">
                                <label>申请日期</label>
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
                        <div runat="server" id="divFormContent">
                            <div class="mws-form-row">
                                <div class="float-left width50-percent">
                                    <label>账套</label>
                                    <div class="mws-form-item">
                                        <telerik:RadComboBox runat="server" ID="rcbxCompany" Filter="Contains"
                                            AllowCustomText="false" Height="160px" EmptyMessage="--请选择--" AutoPostBack="true"
                                            OnSelectedIndexChanged="rcbxCompany_SelectedIndexChanged">
                                        </telerik:RadComboBox>
                                        <asp:RequiredFieldValidator ID="rfvCompany"
                                            runat="server"
                                            ErrorMessage="请选择账套"
                                            ControlToValidate="rcbxCompany"
                                            Display="Dynamic" CssClass="field-validation-error"
                                            ValidationGroup="vgMaintenance" Text="*">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="float-left width50-percent">
                                    <label>奖励年月</label>
                                    <div class="mws-form-item">
                                        <telerik:RadMonthYearPicker runat="server" ID="rmypRefundDate" Width="120"
                                            EnableShadows="true"
                                            MonthYearNavigationSettings-CancelButtonCaption="取消"
                                            MonthYearNavigationSettings-OkButtonCaption="确定"
                                            MonthYearNavigationSettings-TodayButtonCaption="今天"
                                            MonthYearNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                            MonthYearNavigationSettings-EnableScreenBoundaryDetection="true"
                                            AutoPostBack="true" OnSelectedDateChanged="rmypRefundDate_SelectedDateChanged">
                                        </telerik:RadMonthYearPicker>
                                        <asp:RequiredFieldValidator ID="rfvRefundDate"
                                            runat="server"
                                            ErrorMessage="请选择奖励年月"
                                            ControlToValidate="rmypRefundDate"
                                            Display="Dynamic" CssClass="field-validation-error"
                                            ValidationGroup="vgMaintenance" Text="*">
                                        </asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvRefundDate" runat="server"
                                            ControlToValidate="rmypRefundDate" ValidationGroup="vgMaintenance" Display="Dynamic"
                                            Text="*" CssClass="field-validation-error">
                                        </asp:CustomValidator>
                                    </div>
                                </div>
                            </div>

                            <div class="mws-form-row">
                                <div class="float-left width50-percent">
                                    <label>客户</label>
                                    <div class="mws-form-item" runat="server" id="divClientUser">
                                        <telerik:RadComboBox runat="server" ID="rcbxClientUser" Filter="Contains" AllowCustomText="false"
                                            MarkFirstMatch="true" Height="160px" EmptyMessage="--请选择--"
                                            AutoPostBack="true" OnSelectedIndexChanged="rcbxClientUser_SelectedIndexChanged">
                                        </telerik:RadComboBox>
                                        <asp:RequiredFieldValidator ID="rfvClientUser"
                                            runat="server"
                                            ErrorMessage="请选择客户"
                                            ControlToValidate="rcbxClientUser"
                                            Display="Dynamic" CssClass="field-validation-error"
                                            ValidationGroup="vgMaintenance" Text="*">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="float-left width50-percent">
                                    <label>商业单位</label>
                                    <div class="mws-form-item" runat="server" id="divClientCompany">
                                        <telerik:RadComboBox runat="server" ID="rcbxClientCompany" Filter="Contains"
                                            AllowCustomText="false" Height="160px" Width="100%" EmptyMessage="--请选择--"
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
                                <div class="float-left width50-percent">
                                    <label>货品</label>
                                    <div class="mws-form-item small" runat="server" id="divProducts">
                                        <telerik:RadComboBox runat="server" ID="rcbxProduct" Filter="Contains" AutoPostBack="true"
                                            AllowCustomText="false" Height="160px" Width="95%" EmptyMessage="--请选择--"
                                            OnSelectedIndexChanged="rcbxProduct_SelectedIndexChanged">
                                        </telerik:RadComboBox>
                                        <asp:RequiredFieldValidator ID="rfvProduct"
                                            runat="server"
                                            ErrorMessage="请选择货品"
                                            ControlToValidate="rcbxProduct"
                                            Display="Dynamic" CssClass="field-validation-error"
                                            ValidationGroup="vgMaintenance" Text="*">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="float-left width50-percent">
                                    <label>货品规格</label>
                                    <div class="mws-form-item small" runat="server" id="divProductSpecifications">
                                        <telerik:RadDropDownList runat="server" ID="ddlProductSpecification" DefaultMessage="--请选择--"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlProductSpecification_SelectedIndexChanged">
                                        </telerik:RadDropDownList>
                                        <asp:RequiredFieldValidator ID="rfvProductSpecification"
                                            runat="server"
                                            ErrorMessage="请选择货品规格"
                                            ControlToValidate="ddlProductSpecification"
                                            Display="Dynamic" CssClass="field-validation-error"
                                            ValidationGroup="vgMaintenance" Text="*">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="mws-form-row">
                                <div class="float-left width50-percent">
                                    <label>任务量</label>
                                    <div class="mws-form-item toppadding5">
                                        <asp:Label ID="lblTaskQty" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="float-left width50-percent">
                                    <label>出库数量</label>
                                    <div class="mws-form-item toppadding5">
                                        <asp:Label ID="lblStockOutQty" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="mws-form-row">
                                <div class="float-left width50-percent">
                                    <label>是否有流向？</label>
                                    <div class="mws-form-item toppadding5">
                                        <asp:Label ID="lblUseFlowData" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="float-left width50-percent">
                                    <label>流回数量</label>
                                    <div class="mws-form-item toppadding5">
                                        <asp:Label ID="lblBackQty" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="mws-form-row">
                                <div class="float-left width50-percent">
                                    <label>返款单价</label>
                                    <div class="mws-form-item small">
                                        <telerik:RadNumericTextBox runat="server" ID="txtRefundPrice" CssClass="mws-textinput" Type="Currency" ShowSpinButtons="true"
                                            NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999"
                                            MaxLength="10" AutoPostBack="true" OnTextChanged="txtRefundPrice_TextChanged">
                                        </telerik:RadNumericTextBox>
                                        <asp:RequiredFieldValidator ID="rfvRefundPrice"
                                            runat="server"
                                            ErrorMessage="返款单价必填"
                                            ControlToValidate="txtRefundPrice"
                                            Display="Dynamic" CssClass="field-validation-error"
                                            ValidationGroup="vgMaintenance" Text="*">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="float-left width50-percent">
                                    <label>应返金额</label>
                                    <div class="mws-form-item small">
                                        <telerik:RadNumericTextBox runat="server" ID="txtRefundAmount" CssClass="mws-textinput" Type="Currency"
                                            NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999"
                                            MaxLength="10" Enabled="false">
                                        </telerik:RadNumericTextBox>
                                        <asp:CustomValidator ID="cvRefundAmount" runat="server"
                                            ControlToValidate="txtRefundAmount" ValidationGroup="vgMaintenance" Display="Dynamic"
                                            Text="*" CssClass="field-validation-error">
                                        </asp:CustomValidator>
                                    </div>
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
                                                    <telerik:GridTemplateColumn UniqueName="PayDate" HeaderText="转账日期" DataField="PayDate" SortExpression="PayDate">
                                                        <HeaderStyle Width="10%" />
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
                                                            <asp:RequiredFieldValidator ID="rfvPayDate" runat="server" ErrorMessage="转账日期必填" CssClass="field-validation-error"
                                                                ControlToValidate="rdpPayDate" EnableClientScript="true"></asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                    </telerik:GridTemplateColumn>
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
                                                    <telerik:GridTemplateColumn UniqueName="Amount" HeaderText="付款金额" DataField="Amount" SortExpression="Amount">
                                                        <HeaderStyle Width="15%" />
                                                        <ItemStyle Width="15%" />
                                                        <ItemTemplate>
                                                            <span><%# Eval("Amount","{0:C2}") %></span>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div id="divGridCombox">
                                                                <telerik:RadNumericTextBox runat="server" ID="txtAmount" ShowSpinButtons="true"
                                                                    Type="Currency" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator=""
                                                                    MinValue="0" MaxValue="999999999" MaxLength="9" Width="120">
                                                                </telerik:RadNumericTextBox>
                                                            </div>
                                                            <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ErrorMessage="付款金额必填" CssClass="field-validation-error"
                                                                ControlToValidate="txtAmount"></asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Fee" HeaderText="手续费" DataField="Fee" SortExpression="Fee">
                                                        <HeaderStyle Width="10%" />
                                                        <ItemStyle Width="10%" />
                                                        <ItemTemplate>
                                                            <span><%# Eval("Fee","{0:C2}") %></span>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div id="divGridCombox">
                                                                <telerik:RadNumericTextBox runat="server" ID="txtFee" ShowSpinButtons="true"
                                                                    Type="Currency" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator=""
                                                                    MinValue="0" MaxValue="999999999" MaxLength="9" Width="100">
                                                                </telerik:RadNumericTextBox>
                                                            </div>
                                                        </EditItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Comment" HeaderText="备注" DataField="Comment" SortExpression="Comment">
                                                        <HeaderStyle Width="15%" />
                                                        <ItemStyle Width="15%" />
                                                        <ItemTemplate>
                                                            <span><%# Eval("Comment") %></span>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div id="divGridCombox">
                                                                <telerik:RadTextBox runat="server" ID="txtComment" Width="100%">
                                                                </telerik:RadTextBox>
                                                            </div>
                                                        </EditItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridEditCommandColumn UniqueName="Edit" ButtonType="LinkButton" InsertText="保存" EditText="编辑" UpdateText="更新" CancelText="取消" HeaderStyle-Width="80" ItemStyle-Width="80">
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
                            <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Refunds/ClientTaskRefundManagement.aspx');return false;" />
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
            redirectToPage("Views/Refunds/ClientTaskRefundManagement.aspx");
        }

        function refreshMaintenancePage(sender, args) {

            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            redirectToPage("Views/Refunds/ClientTaskRefundMaintenance.aspx?EntityID=" + currentEntityID);
        }

        $(document).ready(function () {
            currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();
        });

    </script>

</asp:Content>

