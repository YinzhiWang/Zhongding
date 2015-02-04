<%@ Page Title="高开高返-供应商返款维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SupplierRefundMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Refunds.SupplierRefundMaintenance" %>

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
                    <telerik:AjaxUpdatedControl ControlID="rgProcureOrderAppDetails" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="divSearchDates" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgProcureOrderAppDetails" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="divSearchDates" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgProcureOrderAppDetails">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgProcureOrderAppDetails" LoadingPanelID="loadingPanel" />
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
            <telerik:AjaxSetting AjaxControlID="tblSearchDeduction">
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

        <div class="mws-panel grid_full">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">高开高返-供应商返款维护</span>
            </div>
            <div class="mws-panel-body">
                <div class="mws-form">
                    <div class="mws-form-inline">
                        <table class="width100-percent">
                            <tr>
                                <td class="width40-percent">
                                    <div class="mws-form-row">
                                        <label>供应商</label>
                                        <div class="mws-form-item small">
                                            <asp:Label ID="lblSupplierName" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>
                                    <div class="mws-form-row">
                                        <label>货品名称</label>
                                        <div class="mws-form-item small">
                                            <asp:Label ID="lblProductName" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>
                                    <div class="mws-form-row">
                                        <label>采购日期</label>
                                        <div class="mws-form-item small" runat="server" id="divSearchDates">
                                            <telerik:RadDatePicker runat="server" ID="rdpBeginDate" Width="100"
                                                Calendar-EnableShadows="true"
                                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                                Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                                Calendar-FastNavigationSettings-DisableOutOfRangeMonths="true"
                                                Calendar-FirstDayOfWeek="Monday">
                                            </telerik:RadDatePicker>
                                            -&nbsp;&nbsp;
                                            <telerik:RadDatePicker runat="server" ID="rdpEndDate" Width="100"
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
                                        <div class="mws-form-item small">
                                            <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="mws-report-container clearfix">
                                        <a class="mws-report" href="#">
                                            <span class="mws-report-icon mws-ic ic-money"></span>
                                            <span class="mws-report-content">
                                                <span class="mws-report-title">应返款</span><br />
                                                <span class="mws-report-value">
                                                    <asp:Label ID="lblNeedRefundAmount" runat="server" Text=""></asp:Label></span>
                                            </span>
                                        </a>

                                        <a class="mws-report" href="#">
                                            <span class="mws-report-icon mws-ic ic-money-bag"></span>
                                            <span class="mws-report-content">
                                                <span class="mws-report-title">已返款</span><br />
                                                <span class="mws-report-value">
                                                    <asp:Label ID="lblRefundedAmount" runat="server" Text=""></asp:Label></span>
                                            </span>
                                        </a>

                                        <a class="mws-report" href="#">
                                            <span class="mws-report-icon mws-ic ic-money-yen"></span>
                                            <span class="mws-report-content">
                                                <span class="mws-report-title">未返款</span><br />
                                                <span class="mws-report-value">
                                                    <asp:Label ID="lblToBeRefundAmount" runat="server" Text=""></asp:Label></span>
                                            </span>
                                        </a>

                                    </div>
                                </td>
                            </tr>
                        </table>

                        <div class="mws-form-row">
                            <telerik:RadGrid ID="rgProcureOrderAppDetails" runat="server" PageSize="10"
                                AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                                MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" Height="480" ShowHeader="true" ShowFooter="true"
                                ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                OnNeedDataSource="rgProcureOrderAppDetails_NeedDataSource">
                                <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                    ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                    <Columns>
                                        <telerik:GridBoundColumn UniqueName="SupplierName" HeaderText="供应商" DataField="SupplierName" ReadOnly="true">
                                            <HeaderStyle Width="180" />
                                            <ItemStyle HorizontalAlign="Left" Width="180" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="OrderDate" HeaderText="采购日期" DataField="OrderDate" SortExpression="ExpirationDate" DataFormatString="{0:yyyy/MM/dd}" ReadOnly="true">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="OrderCode" HeaderText="采购单编号" DataField="OrderCode" ReadOnly="true">
                                            <HeaderStyle Width="160" />
                                            <ItemStyle HorizontalAlign="Left" Width="160" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName" ReadOnly="true">
                                            <HeaderStyle Width="180" />
                                            <ItemStyle HorizontalAlign="Left" Width="180" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification" ReadOnly="true">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="UnitName" HeaderText="单位" DataField="UnitName" ReadOnly="true">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ProcureCount" HeaderText="订货数量" DataField="ProcureCount" ReadOnly="true">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ProcurePrice" HeaderText="单价" DataField="ProcurePrice" DataFormatString="{0:C2}" ReadOnly="true">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="TotalAmount" HeaderText="订货金额" DataField="TotalAmount" DataFormatString="{0:C2}" ReadOnly="true">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="SupplierTaxRatio" HeaderText="税率" DataField="SupplierTaxRatio" DataFormatString="{0:P2}" ReadOnly="true">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="TotalNeedRefundAmount" HeaderText="应返款" DataField="TotalNeedRefundAmount" DataFormatString="{0:C2}" ReadOnly="true">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                    <CommandItemTemplate>
                                        <table class="width100-percent">
                                            <tr>
                                                <td class="right-td rightpadding10">
                                                    <input type="button" class="rgRefresh" onclick="refreshGrid(gridClientIDs.gridProcureOrderAppDetails); return false;" />
                                                    <a href="javascript:void(0);" onclick="refreshGrid(gridClientIDs.gridProcureOrderAppDetails); return false;">刷新</a>
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
                                    <Scrolling AllowScroll="true" FrozenColumnsCount="3" SaveScrollPosition="true" UseStaticHeaders="true" />
                                </ClientSettings>
                                <HeaderStyle Width="100%" />
                            </telerik:RadGrid>
                        </div>

                        <div class="mws-form-row">
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
                                                            ErrorMessage="返款(含手续费)和抵扣之和不能大于应返款总额" OnServerValidate="cvAmount_ServerValidate"></asp:CustomValidator>
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
                                                                ErrorMessage="返款(含手续费)和抵扣之和不能大于应返款总额" OnServerValidate="cvFee_ServerValidate"></asp:CustomValidator>
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
                                                            ErrorMessage="抵扣和返款(含手续费)之和不能大于应返款总额" OnServerValidate="cvAmount_ServerValidate"></asp:CustomValidator>
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
                        <div class="mws-form-row"></div>
                        <div class="mws-button-row">
                            <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Refunds/SupplierRefundManagement.aspx');return false;" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnCurrentEntityID" runat="server" Value="-1" />
    <asp:HiddenField ID="hdnNeedRefreshPage" runat="server" Value="false" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <style>
        div#divGridCombox td, td:first-child {
            border-left-style: solid;
        }

        .mws-report {
            width: auto !important;
        }
    </style>

    <script type="text/javascript">

        var currentEntityID = -1;

        var gridClientIDs = {
            gridProcureOrderAppDetails: "<%= rgProcureOrderAppDetails.ClientID %>",

        };

        function refreshGrid(gridClientID) {
            var gridObj = $find(gridClientID);

            if (gridObj)
                gridObj.get_masterTableView().rebind();
        }

        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/Refunds/SupplierRefundManagement.aspx");
        }

        function onResponseEnd(sender, args) {

            var eventTarget = args.get_eventTarget();

            if ((eventTarget.indexOf("rgSupplierRefunds") >= 0
                    || eventTarget.indexOf("rgSupplierDeduction") >= 0)
                && (eventTarget.indexOf("lbtnInsert") >= 0)) {

                var needRefreshPage = $("#<%= hdnNeedRefreshPage.ClientID%>").val();

                if (needRefreshPage && needRefreshPage.toLowerCase() == "true") {

                    $.showLoading();

                    var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();
                    var companyID = ("<%=this.CompanyID%>");
                    var supplierID = ("<%=this.SupplierID%>");
                    var productID = ("<%=this.ProductID%>");
                    var productSpecificationID = ("<%=this.ProductSpecificationID%>");

                    $("#<%= hdnNeedRefreshPage.ClientID%>").val("false");

                    window.location.href = "SupplierRefundMaintenance.aspx?EntityID=" + currentEntityID + "&CompanyID=" + companyID
                        + "&SupplierID=" + supplierID + "&ProductID=" + productID + "&ProductSpecificationID=" + productSpecificationID;
                }
            }
        }

        $(document).ready(function () {
            currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();
        });

    </script>
</asp:Content>
