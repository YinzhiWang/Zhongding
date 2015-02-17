<%@ Page Title="供应商任务返款维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SupplierTaskRefundMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Refunds.SupplierTaskRefundMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbxCompany">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divSuppliers" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="divProducts" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="divProductSpecifications" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="divPaymentMethods" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbxSupplier">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divProducts" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="divProductSpecifications" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbxProduct">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divProductSpecifications" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlPaymentMethod">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divPaymentMethods" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rdpBeginDate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divEndDate" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rdpEndDate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divBeginDate" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="container">
        <div class="mws-panel grid_full">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">供应商任务返款维护</span>
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
                                <label>供应商</label>
                                <div class="mws-form-item" runat="server" id="divSuppliers">
                                    <telerik:RadComboBox runat="server" ID="rcbxSupplier" Filter="Contains"
                                        AllowCustomText="false" Height="160px" Width="260" EmptyMessage="--请选择--"
                                        AutoPostBack="true" OnSelectedIndexChanged="rcbxSupplier_SelectedIndexChanged">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="rfvSupplier"
                                        runat="server"
                                        ErrorMessage="请选择供应商"
                                        ControlToValidate="rcbxSupplier"
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
                                    <telerik:RadDropDownList runat="server" ID="ddlProductSpecification">
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
                                <label>起始日期</label>
                                <div class="mws-form-item toppadding5" runat="server" id="divBeginDate">
                                    <telerik:RadDatePicker runat="server" ID="rdpBeginDate"
                                        Calendar-EnableShadows="true"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                        Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                        Calendar-FastNavigationSettings-DisableOutOfRangeMonths="true"
                                        Calendar-FirstDayOfWeek="Monday"
                                        AutoPostBack="true" OnSelectedDateChanged="rdpBeginDate_SelectedDateChanged">
                                    </telerik:RadDatePicker>
                                    <asp:CustomValidator ID="cvBeginDate" runat="server"
                                        ControlToValidate="rdpBeginDate" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                    <asp:RequiredFieldValidator ID="rfvBeginDate" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="rdpBeginDate"
                                        ErrorMessage="起始日期必填" Text="*" Display="Dynamic" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="float-left width50-percent">
                                <label>结束日期</label>
                                <div class="mws-form-item toppadding5" runat="server" id="divEndDate">
                                    <telerik:RadDatePicker runat="server" ID="rdpEndDate"
                                        Calendar-EnableShadows="true"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                        Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                        Calendar-FastNavigationSettings-DisableOutOfRangeMonths="true"
                                        Calendar-FirstDayOfWeek="Monday"
                                        AutoPostBack="true" OnSelectedDateChanged="rdpEndDate_SelectedDateChanged">
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="rdpEndDate"
                                        ErrorMessage="结束日期必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width50-percent">
                                <label>收款日期</label>
                                <div class="mws-form-item toppadding5">
                                    <telerik:RadDatePicker runat="server" ID="rdpRefundDate"
                                        Calendar-EnableShadows="true"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                        Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                        Calendar-FastNavigationSettings-DisableOutOfRangeMonths="true"
                                        Calendar-FirstDayOfWeek="Monday">
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="rfvRefundDate" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="rdpRefundDate"
                                        ErrorMessage="收款日期必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="float-left width50-percent">
                                <label>收款类型</label>
                                <div class="mws-form-item toppadding5">
                                    <telerik:RadDropDownList runat="server" ID="ddlPaymentMethod" DefaultMessage="--请选择--"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlPaymentMethod_SelectedIndexChanged">
                                    </telerik:RadDropDownList>
                                    <asp:RequiredFieldValidator ID="rfvPaymentMethod"
                                        runat="server"
                                        ErrorMessage="请选择收款类型"
                                        ControlToValidate="ddlPaymentMethod"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width50-percent">
                                <label>收款金额</label>
                                <div class="mws-form-item small">
                                    <telerik:RadNumericTextBox runat="server" ID="txtRefundAmount" CssClass="mws-textinput" Type="Currency"
                                        NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999"
                                        MaxLength="10">
                                    </telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="rfvRefundAmount"
                                        runat="server"
                                        ErrorMessage="收款金额必填"
                                        ControlToValidate="txtRefundAmount"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="float-left width50-percent" runat="server" id="divPaymentMethods">
                                <div runat="server" id="divRefundAccount">
                                    <label>收款账号</label>
                                    <div class="mws-form-item small">
                                        <telerik:RadComboBox runat="server" ID="rcbxToAccount" Filter="Contains" AllowCustomText="false"
                                            MarkFirstMatch="true" Height="160px" Width="100%" EmptyMessage="--请选择--">
                                        </telerik:RadComboBox>
                                        <asp:CustomValidator ID="cvToAccount" runat="server" ErrorMessage="请选择收款账号"
                                            ControlToValidate="rcbxToAccount" ValidationGroup="vgMaintenance" Display="Dynamic"
                                            Text="*" CssClass="field-validation-error">
                                        </asp:CustomValidator>
                                    </div>
                                </div>
                                <div runat="server" id="divDeductSupplier" visible="false">
                                    <label>抵扣供应商</label>
                                    <div class="mws-form-item small">
                                        <telerik:RadComboBox runat="server" ID="rcbxDeductSupplier" Filter="Contains"
                                            AllowCustomText="false" Height="160px" Width="260" EmptyMessage="--请选择--">
                                        </telerik:RadComboBox>
                                        <asp:CustomValidator ID="cvDeductSupplier" runat="server" ErrorMessage="请选择抵扣供应商"
                                            ControlToValidate="rcbxDeductSupplier" ValidationGroup="vgMaintenance" Display="Dynamic"
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

                        <div class="mws-button-row">
                            <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Refunds/SupplierTaskRefundManagement.aspx');return false;" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnCurrentEntityID" runat="server" Value="-1" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">
        var currentEntityID = -1;

        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/Refunds/SupplierTaskRefundManagement.aspx");
        }

        $(document).ready(function () {
            currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();
        });

    </script>

</asp:Content>

