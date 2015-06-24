<%@ Page Title="工资结算维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalarySettleMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.HRM.SalarySettleMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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

            <telerik:AjaxSetting AjaxControlID="rcbxDepartment">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbxDepartment" />
                    <telerik:AjaxUpdatedControl ControlID="rgSalarySettleDetails" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rmypSettleDate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rmypSettleDate" />
                    <telerik:AjaxUpdatedControl ControlID="rgSalarySettleDetails" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>

    </telerik:RadAjaxManager>
    <div class="container">

        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">工资结算维护</span>
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
                                <label>部门</label>
                                <div class="mws-form-item small">
                                    <telerik:RadComboBox runat="server" ID="rcbxDepartment" Filter="Contains" AllowCustomText="true" AutoPostBack="true"
                                        Width="200px"
                                        MarkFirstMatch="true" Height="200px" EmptyMessage="--请选择--" OnSelectedIndexChanged="rcbxDepartment_SelectedIndexChanged">
                                    </telerik:RadComboBox>

                                    <telerik:RadToolTip ID="rttSupplier" runat="server" TargetControlID="rcbxDepartment" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                    <asp:CustomValidator ID="cvDepartment" runat="server" ErrorMessage="请选择部门"
                                        ControlToValidate="rcbxDepartment" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                </div>

                            </div>
                            <div class="float-left">
                                <label>结算月份</label>
                                <div class="mws-form-item small">
                                    <telerik:RadMonthYearPicker runat="server" ID="rmypSettleDate" Width="120"
                                        EnableShadows="true"
                                        MonthYearNavigationSettings-CancelButtonCaption="取消"
                                        MonthYearNavigationSettings-OkButtonCaption="确定"
                                        MonthYearNavigationSettings-TodayButtonCaption="今天"
                                        MonthYearNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                        MonthYearNavigationSettings-EnableScreenBoundaryDetection="true"
                                        AutoPostBack="true"
                                        OnSelectedDateChanged="rmypSettleDate_SelectedDateChanged">
                                    </telerik:RadMonthYearPicker>
                                    <asp:RequiredFieldValidator ID="rfvPaymentCautionMoney" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="rmypSettleDate"
                                        ErrorMessage="结算月份必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                    <telerik:RadToolTip ID="rttPaymentCautionMoney" runat="server" TargetControlID="rmypSettleDate" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                </div>
                            </div>
                        </div>

                        <div class="mws-form-row">
                            <telerik:RadGrid ID="rgSalarySettleDetails" runat="server" PageSize="500"
                                MasterTableView-PagerStyle-PageSizes="500"
                                AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" Height="480" ShowHeader="true"
                                ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                OnNeedDataSource="rgSalarySettleDetails_NeedDataSource" OnItemDataBound="rgSalarySettleDetails_ItemDataBound">
                                <ValidationSettings EnableValidation="true" />
                                <MasterTableView Width="100%" DataKeyNames="UserID,ID" CommandItemDisplay="Top"
                                    ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                    <BatchEditingSettings EditType="Cell" />
                                    <CommandItemSettings ShowAddNewRecordButton="false" ShowSaveChangesButton="false"
                                        ShowCancelChangesButton="true" ShowRefreshButton="true"
                                        SaveChangesText="保存" CancelChangesText="取消" RefreshText="刷新" />
                                    <ColumnGroups>
                                        <telerik:GridColumnGroup Name="DeductColumnGroup" HeaderText="请假扣款(按天数)" HeaderStyle-Font-Size="Small"
                                            HeaderStyle-HorizontalAlign="Center" />

                                    </ColumnGroups>
                                    <Columns>
                                        <telerik:GridBoundColumn UniqueName="UserID" HeaderText="UserID" DataField="UserID" Visible="false" ReadOnly="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Order" HeaderText="编号" DataField="Order" ReadOnly="true">
                                            <HeaderStyle Width="40" />
                                            <ItemStyle HorizontalAlign="Left" Width="40" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="FullName" HeaderText="姓名" DataField="FullName" ReadOnly="true">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="EnrollDate" HeaderText="入职时间" DataField="EnrollDate" ReadOnly="true" DataFormatString="{0:yyyy-MM-dd}">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridTemplateColumn UniqueName="BasicSalary" HeaderText="基本工资" DataField="BasicSalary"
                                            SortExpression="ProcurePrice">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Center" Width="80" />
                                            <ItemTemplate>
                                                <telerik:RadNumericTextBox Width="100%" runat="server" ID="txtBasicSalary" NumberFormat-DecimalDigits="2"
                                                    Type="Number" MinValue="0" MaxValue="999999999" MaxLength="9" ShowSpinButtons="false">
                                                </telerik:RadNumericTextBox>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="WorkDay" HeaderText="上班天数" DataField="WorkDay"
                                            SortExpression="SalePrice">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                            <ItemTemplate>
                                                <telerik:RadNumericTextBox Width="100%" runat="server" ID="txtWorkDay" NumberFormat-DecimalDigits="2"
                                                    Type="Number" MinValue="0" MaxValue="999999999" MaxLength="9" ShowSpinButtons="false">
                                                </telerik:RadNumericTextBox>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="MealAllowance" HeaderText="餐费补助" DataField="MealAllowance"
                                            SortExpression="SalePrice">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                            <ItemTemplate>
                                                <telerik:RadNumericTextBox Width="100%" runat="server" ID="txtMealAllowance" NumberFormat-DecimalDigits="2"
                                                    Type="Number" MinValue="0" MaxValue="999999999" MaxLength="9" ShowSpinButtons="false">
                                                </telerik:RadNumericTextBox>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="PositionSalary" HeaderText="岗位工资" DataField="PositionSalary"
                                            SortExpression="SalePrice">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                            <ItemTemplate>
                                                <telerik:RadNumericTextBox Width="100%" runat="server" ID="txtPositionSalary" NumberFormat-DecimalDigits="2"
                                                    Type="Number" MinValue="0" MaxValue="999999999" MaxLength="9" ShowSpinButtons="false">
                                                </telerik:RadNumericTextBox>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="BonusPay" HeaderText="绩效工资" DataField="BonusPay"
                                            SortExpression="SalePrice">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                            <ItemTemplate>
                                                <telerik:RadNumericTextBox Width="100%" runat="server" ID="txtBonusPay" NumberFormat-DecimalDigits="2"
                                                    Type="Number" MinValue="0" MaxValue="999999999" MaxLength="9" ShowSpinButtons="false">
                                                </telerik:RadNumericTextBox>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="WorkAgeSalary" HeaderText="工龄工资" DataField="WorkAgeSalary"
                                            SortExpression="SalePrice">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                            <ItemTemplate>
                                                <telerik:RadNumericTextBox Width="100%" runat="server" ID="txtWorkAgeSalary" NumberFormat-DecimalDigits="2"
                                                    Type="Number" MinValue="0" MaxValue="999999999" MaxLength="9" ShowSpinButtons="false">
                                                </telerik:RadNumericTextBox>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="PhoneAllowance" HeaderText="话费补贴" DataField="PhoneAllowance"
                                            SortExpression="SalePrice">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                            <ItemTemplate>
                                                <telerik:RadNumericTextBox Width="100%" runat="server" ID="txtPhoneAllowance" NumberFormat-DecimalDigits="2"
                                                    Type="Number" MinValue="0" MaxValue="999999999" MaxLength="9" ShowSpinButtons="false">
                                                </telerik:RadNumericTextBox>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="OfficeExpense" HeaderText="办公费用" DataField="OfficeExpense"
                                            SortExpression="SalePrice">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                            <ItemTemplate>
                                                <telerik:RadNumericTextBox Width="100%" runat="server" ID="txtOfficeExpense" NumberFormat-DecimalDigits="2"
                                                    Type="Number" MinValue="0" MaxValue="999999999" MaxLength="9" ShowSpinButtons="false">
                                                </telerik:RadNumericTextBox>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="OtherAllowance" HeaderText="其他补助" DataField="OtherAllowance"
                                            SortExpression="SalePrice">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                            <ItemTemplate>
                                                <telerik:RadNumericTextBox Width="100%" runat="server" ID="txtOtherAllowance" NumberFormat-DecimalDigits="2"
                                                    Type="Number" MinValue="0" MaxValue="999999999" MaxLength="9" ShowSpinButtons="false">
                                                </telerik:RadNumericTextBox>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="NeedPaySalary" HeaderText="应付工资" DataField="NeedPaySalary"
                                            SortExpression="SalePrice">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                            <ItemTemplate>
                                                <telerik:RadNumericTextBox Width="90px" runat="server" ID="txtNeedPaySalary" NumberFormat-DecimalDigits="2"
                                                    Type="Number" MinValue="-999999999" MaxValue="999999999" MaxLength="9" ShowSpinButtons="false" ReadOnly="true">
                                                </telerik:RadNumericTextBox>
                                            </ItemTemplate>

                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="NeedDeduct" HeaderText="应扣" DataField="NeedDeduct"
                                            SortExpression="SalePrice">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                            <ItemTemplate>
                                                <telerik:RadNumericTextBox Width="100%" runat="server" ID="txtNeedDeduct" NumberFormat-DecimalDigits="2"
                                                    Type="Number" MinValue="0" MaxValue="999999999" MaxLength="9" ShowSpinButtons="false">
                                                </telerik:RadNumericTextBox>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="HolidayDeductOfSalary" HeaderText="工资" DataField="HolidayDeductOfSalary" ColumnGroupName="DeductColumnGroup"
                                            SortExpression="SalePrice">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                            <ItemTemplate>
                                                <telerik:RadNumericTextBox Width="100%" runat="server" ID="txtHolidayDeductOfSalary" NumberFormat-DecimalDigits="2"
                                                    Type="Number" MinValue="0" MaxValue="999999999" MaxLength="9" ShowSpinButtons="false">
                                                </telerik:RadNumericTextBox>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="HolidayDeductOfMealAllowance" HeaderText="餐补" DataField="HolidayDeductOfMealAllowance" ColumnGroupName="DeductColumnGroup"
                                            SortExpression="SalePrice">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                            <ItemTemplate>
                                                <telerik:RadNumericTextBox Width="100%" runat="server" ID="txtHolidayDeductOfMealAllowance" NumberFormat-DecimalDigits="2"
                                                    Type="Number" MinValue="0" MaxValue="999999999" MaxLength="9" ShowSpinButtons="false">
                                                </telerik:RadNumericTextBox>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="RealPaySalary" HeaderText="实发工资" DataField="RealPaySalary"
                                            SortExpression="SalePrice">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                            <ItemTemplate>
                                                <telerik:RadNumericTextBox Width="100%" runat="server" ID="txtRealPaySalary" NumberFormat-DecimalDigits="2"
                                                    Type="Number" MinValue="-999999999" MaxValue="999999999" MaxLength="9" ShowSpinButtons="false" ReadOnly="true">
                                                </telerik:RadNumericTextBox>
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
                                <ClientSettings>
                                    <Scrolling AllowScroll="true" FrozenColumnsCount="0" SaveScrollPosition="true" UseStaticHeaders="true" />
                                </ClientSettings>
                                <HeaderStyle Width="99.8%" />
                            </telerik:RadGrid>
                            <div class="float-right" runat="server" id="divTotalSalary" visible="false">
                                <span class="bold">应发工资总额</span>：<asp:Label ID="lblTotalNeedPaySalary" runat="server"></asp:Label>元&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span class="bold">实发工资总额</span>：<asp:Label ID="lblTotalRealPaySalary" runat="server"></asp:Label>元
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
                                        OnUpdateCommand="rgAppPayments_UpdateCommand" OnDeleteCommand="rgAppPayments_DeleteCommand"
                                        OnItemCommand="rgAppPayments_ItemCommand">
                                        <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                            ShowHeadersWhenNoRecords="true" BackColor="#fafafa" EditMode="InPlace">
                                            <CommandItemSettings AddNewRecordText="添加" RefreshText="刷新" ShowAddNewRecordButton="false" />
                                            <Columns>

                                                <telerik:GridBoundColumn UniqueName="Order" HeaderText="编号" DataField="Order" ReadOnly="true">
                                                    <HeaderStyle Width="40" />
                                                    <ItemStyle HorizontalAlign="Left" Width="40" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="FullName" HeaderText="姓名" DataField="FullName" ReadOnly="true">
                                                    <HeaderStyle Width="100" />
                                                    <ItemStyle HorizontalAlign="Left" Width="100" />
                                                </telerik:GridBoundColumn>

                                                <telerik:GridBoundColumn UniqueName="RealPaySalary" HeaderText="应付工资" DataField="RealPaySalary" ReadOnly="true" DataFormatString="￥{0:f2}">
                                                    <HeaderStyle Width="100" />
                                                    <ItemStyle HorizontalAlign="Left" Width="100" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="FromAccount" HeaderText="出账账户" DataField="FromAccount" SortExpression="FromAccount">
                                                    <ItemTemplate>
                                                        <table style="width: 100%;border-width:0px;border-left-width:0px;" border="0">
                                                            <tr>
                                                                <td style="border-width:0px;border-left-width:0px;">
                                                                        <div id="divGridCombox">
                                                                    <telerik:RadComboBox runat="server" ID="rcbxFromAccount" Filter="Contains" AllowCustomText="false" NoWrap="true"
                                                                        MarkFirstMatch="true" Height="160px" Width="100%" EmptyMessage="--请选择--">
                                                                    </telerik:RadComboBox>
                                                                            </div>
                                                                </td>
                                                                <td style="width: 200px;border-width:0px;border-left-width:0px;"">
                                                                <asp:CustomValidator ID="rfvFromAccount" runat="server" ErrorMessage="请选择出账账户"
                                        ControlToValidate="rcbxFromAccount" ValidationGroup="vgMaintenance" Display="Dynamic"
                                          CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                                                   </td>
                                                            </tr>
                                                        </table>


                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridButtonColumn Text="支付" UniqueName="Pay" CommandName="Pay" ButtonType="LinkButton" HeaderStyle-Width="60" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center" ConfirmText="确认支付吗？" />

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


                        <div class="height20"></div>
                    </div>
                    <div class="mws-button-row">
                        <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" OnClientClick="return onBtnSaveClick();" />
                        <asp:Button ID="btnSubmit" runat="server" Text="提交" CssClass="mws-button green" CausesValidation="true" OnClick="btnSubmit_Click" />
                        <asp:Button ID="btnAudit" runat="server" Text="审核通过" CssClass="mws-button green" CausesValidation="true" Visible="false" OnClick="btnAudit_Click" />
                        <asp:Button ID="btnReturn" runat="server" Text="退回" CssClass="mws-button orange" CausesValidation="true" Visible="false" OnClick="btnReturn_Click" />
                        <asp:Button ID="btnPay" runat="server" Text="确认支付" CssClass="mws-button green" CausesValidation="true" OnClick="btnPay_Click" Visible="false" />

                        <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToManagementPage();return false;" />
                    </div>
                </div>
            </div>
        </div>

    </div>
    <asp:HiddenField ID="hdnCurrentEntityID" runat="server" Value="-1" />
    <asp:HiddenField ID="hdnBasicPricesCellValueChangedCount" runat="server" Value="0" />
    <telerik:RadNotification ID="radNotification" runat="server" EnableRoundedCorners="true"
        AutoCloseDelay="1000" Skin="Silk" Animation="Fade" EnableShadow="true" Title="提示"
        TitleIcon="none" Opacity="95" Position="Center" ContentIcon="~/Content/icons/32/cross.png"
        Width="300" Height="100">
    </telerik:RadNotification>
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
            redirectToPage('Views/HRM/SalarySettleManagement.aspx');
        }

        function onClientBlur(sender, args) {

        }
        function onBtnCancelClick() {
            redirectToPage('Views/HRM/SalarySettleManagement.aspx');
        }

        function redirectToManagementPage() {
            $.showLoading();
            window.location.href = "SalarySettleManagement.aspx";
        }

        function refreshMaintenancePage(sender, args) {

            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();
            var clientCautionMoneyID = $.getQueryString("ClientCautionMoneyID");
            redirectToPage("Views/CautionMoneys/ClientCautionMoneyReturnApplyMaintenance.aspx?EntityID=" + currentEntityID + "&ClientCautionMoneyID=" + clientCautionMoneyID);
        }
        function onResponseEnd(sender, args) {

            var eventTarget = args.get_eventTarget();

            if ((eventTarget.indexOf("rgSupplierRefunds") >= 0
                    || eventTarget.indexOf("rgSupplierDeduction") >= 0)
                && (eventTarget.indexOf("lbtnInsert") >= 0)) {


            }

        }

        function onBasicPricesCellValueChanged(sender, args) {
            //debugger;

            var hdnGridCellValueChangedCount = $("#<%=hdnBasicPricesCellValueChangedCount.ClientID%>");

            var oChangedCount = parseInt(hdnGridCellValueChangedCount.val(), 0);

            if (args.get_editorValue() != args.get_cellValue())
                oChangedCount = oChangedCount + 1;
            else
                oChangedCount = oChangedCount - 1;

            hdnGridCellValueChangedCount.val(oChangedCount);
        }
        function onBtnSaveClick(sender, eventArgs) {
            var radNotification = $find("<%=radNotification.ClientID%>");

            var rgSalarySettleDetails = $find("<%=rgSalarySettleDetails.ClientID%>");
            if (!rgSalarySettleDetails.get_visible()) {
                return true;
            }

            if (!calculateHasValidItem()) {
                radNotification.set_text("请至少填写一条工资信息");
                radNotification.show();
                return false;
            }
            calculateRealPaySalary();
            return true;
        }
        function calculateHasValidItem() {
            var rgSalarySettleDetails = $find("<%=rgSalarySettleDetails.ClientID%>");
            var masterTableView = rgSalarySettleDetails.get_masterTableView();
            if (!masterTableView) return false;
            var selectedItems = masterTableView.get_dataItems();


            for (var i = 0; i < selectedItems.length; i++) {
                var curSelectedItem = selectedItems[i];
                var curSelectedItemElement = curSelectedItem.get_element();
                {
                    var txtRealPaySalary = $telerik.findControl(curSelectedItemElement, "txtRealPaySalary");
                    if (txtRealPaySalary.get_value() && txtRealPaySalary.get_value() != 0) {
                        basicSalary = txtBasicSalary.get_value();
                        return true;
                    }
                }
            }
            return false;
        }
        function calculateRealPaySalary() {

            var rgSalarySettleDetails = $find("<%=rgSalarySettleDetails.ClientID%>");
            if (!rgSalarySettleDetails) return;
            var masterTableView = rgSalarySettleDetails.get_masterTableView();
            if (!masterTableView) return;
            var selectedItems = masterTableView.get_dataItems();


            for (var i = 0; i < selectedItems.length; i++) {
                var curSelectedItem = selectedItems[i];
                var curSelectedItemElement = curSelectedItem.get_element();
                var basicSalary = 0;
                {
                    var txtBasicSalary = $telerik.findControl(curSelectedItemElement, "txtBasicSalary");
                    if (txtBasicSalary.get_value()) {
                        basicSalary = txtBasicSalary.get_value();
                    }
                }
                var workDay = 0;
                {
                    var txtWorkDay = $telerik.findControl(curSelectedItemElement, "txtWorkDay");
                    if (txtWorkDay.get_value()) {
                        workDay = txtWorkDay.get_value();
                    }
                }
                var mealAllowance = 0;
                {
                    var txtMealAllowance = $telerik.findControl(curSelectedItemElement, "txtMealAllowance");
                    if (txtMealAllowance.get_value()) {
                        mealAllowance = txtMealAllowance.get_value();
                    }
                }
                var positionSalary = 0;
                {
                    var txtPositionSalary = $telerik.findControl(curSelectedItemElement, "txtPositionSalary");
                    if (txtPositionSalary.get_value()) {
                        positionSalary = txtPositionSalary.get_value();
                    }
                }
                var bonusPay = 0;
                {
                    var txtBonusPay = $telerik.findControl(curSelectedItemElement, "txtBonusPay");
                    if (txtBonusPay.get_value()) {
                        bonusPay = txtBonusPay.get_value();
                    }
                }
                var workAgeSalary = 0;
                {
                    var txtWorkAgeSalary = $telerik.findControl(curSelectedItemElement, "txtWorkAgeSalary");
                    if (txtWorkAgeSalary.get_value()) {
                        workAgeSalary = txtWorkAgeSalary.get_value();
                    }
                }
                var phoneAllowance = 0;
                {
                    var txtPhoneAllowance = $telerik.findControl(curSelectedItemElement, "txtPhoneAllowance");
                    if (txtPhoneAllowance.get_value()) {
                        phoneAllowance = txtPhoneAllowance.get_value();
                    }
                }
                var officeExpense = 0;
                {
                    var txtOfficeExpense = $telerik.findControl(curSelectedItemElement, "txtOfficeExpense");
                    if (txtOfficeExpense.get_value()) {
                        officeExpense = txtOfficeExpense.get_value();
                    }
                }
                var otherAllowance = 0;
                {
                    var txtOtherAllowance = $telerik.findControl(curSelectedItemElement, "txtOtherAllowance");
                    if (txtOtherAllowance.get_value()) {
                        otherAllowance = txtOtherAllowance.get_value();
                    }
                }
                var needPaySalary = basicSalary + mealAllowance + positionSalary + bonusPay + workAgeSalary + phoneAllowance + officeExpense + otherAllowance;
                {
                    var txtNeedPaySalary = $telerik.findControl(curSelectedItemElement, "txtNeedPaySalary");
                    //alert(needPaySalary);
                    txtNeedPaySalary.set_value(needPaySalary);
                }
                var needDeduct = 0;
                {
                    var txtNeedDeduct = $telerik.findControl(curSelectedItemElement, "txtNeedDeduct");
                    if (txtNeedDeduct.get_value()) {
                        needDeduct = txtNeedDeduct.get_value();
                    }
                }
                var holidayDeductOfSalary = 0;
                {
                    var txtHolidayDeductOfSalary = $telerik.findControl(curSelectedItemElement, "txtHolidayDeductOfSalary");
                    if (txtHolidayDeductOfSalary.get_value()) {
                        holidayDeductOfSalary = txtHolidayDeductOfSalary.get_value();
                    }
                }
                var holidayDeductOfMealAllowance = 0;
                {
                    var txtHolidayDeductOfMealAllowance = $telerik.findControl(curSelectedItemElement, "txtHolidayDeductOfMealAllowance");
                    if (txtHolidayDeductOfMealAllowance.get_value()) {
                        holidayDeductOfMealAllowance = txtHolidayDeductOfMealAllowance.get_value();
                    }
                }
                var realPaySalary = basicSalary + mealAllowance + positionSalary + bonusPay + workAgeSalary + phoneAllowance + officeExpense + otherAllowance
                    - needDeduct - holidayDeductOfSalary - holidayDeductOfMealAllowance;
                {
                    var txtHolidayDeductOfMealAllowance = $telerik.findControl(curSelectedItemElement, "txtRealPaySalary");
                    txtHolidayDeductOfMealAllowance.set_value(realPaySalary);
                }


                //alert( $(curSelectedItemElement).html());span[id=^lblBasicSalary]
                //alert( txtProcurePrice.get_value());



                //if (!curCurrentClientInvoiceDetailAmount) {
                //    radNotification.set_text("勾选的订单必须填写本次开票金额");
                //    radNotification.show();
                //    return false;
                //}

            }
        }
        $(document).ready(function () {
            calculateRealPaySalary();
            window.setInterval(calculateRealPaySalary, 1000);
        });
    </script>
    <style>
        div#divGridCombox td, td:first-child {
            border-left-style: solid;
        }

        html body .RadInput_Silk .riDisabled, html body .RadInput_Disabled_Silk {
            Opacity: 1 !important;
            -moz-opacity: 1;
            filter: alpha(opacity=100);
            cursor: default;
        }
    </style>

</asp:Content>
