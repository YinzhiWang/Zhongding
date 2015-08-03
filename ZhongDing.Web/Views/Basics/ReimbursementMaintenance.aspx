<%@ Page Title="供应商维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReimbursementMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.ReimbursementMaintenance" %>


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
                    <telerik:AjaxUpdatedControl ControlID="rgReimbursementDetails" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rmypSettleDate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rmypSettleDate" />
                    <telerik:AjaxUpdatedControl ControlID="rgReimbursementDetails" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>

    </telerik:RadAjaxManager>
    <div class="container">

        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">费用报销维护</span>
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
                                <label>申请人</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtCreatedByFullName" CssClass="mws-textinput" Width="200px" MaxLength="100" Enabled="false"></telerik:RadTextBox>

                                </div>

                            </div>
                            <div class="float-left">
                                <label>部门</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtDepartment" CssClass="mws-textinput" Width="200px" MaxLength="100" Enabled="false"></telerik:RadTextBox>

                                </div>
                            </div>
                        </div>

                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>申请日期</label>
                                <div class="mws-form-item small">
                                    <telerik:RadDatePicker ID="txtApplyDate" Width="200px" runat="server">
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="rfvSendDate" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtApplyDate"
                                        ErrorMessage="申请日期必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                    <telerik:RadToolTip ID="rttSendDate" runat="server" TargetControlID="txtApplyDate" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                </div>

                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-procure-audit" runat="server" id="divReimbursementDetails">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">报销明细</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <div class="mws-form-row">
                                            <telerik:RadGrid ID="rgReimbursementDetails" runat="server" PageSize="10"
                                                AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                                MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                                ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                                OnNeedDataSource="rgReimbursementDetails_NeedDataSource" OnItemDataBound="rgReimbursementDetails_ItemDataBound"
                                                OnDeleteCommand="rgReimbursementDetails_DeleteCommand"  OnItemCreated="rgReimbursementDetails_ItemCreated"
                                                 OnColumnCreated="rgReimbursementDetails_ColumnCreated">

                                                <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                    ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                                    <Columns>
                                                        <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false">
                                                            <ItemStyle HorizontalAlign="Left" />

                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="ReimbursementType" HeaderText="报销类型" DataField="ReimbursementType">
                                                            <ItemStyle HorizontalAlign="Left" Width="160px" />
                                                            <HeaderStyle Width="160px" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="ReimbursementTypeChild" HeaderText="子类型" DataField="ReimbursementTypeChild">
                                                            <ItemStyle HorizontalAlign="Left" Width="160px" />
                                                            <HeaderStyle Width="160px" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="StartDate" HeaderText="开始日期" DataField="StartDate" DataFormatString="{0:yyyy/MM/dd}">
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                            <HeaderStyle Width="100px" />
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="EndDate" HeaderText="结束日期" DataField="EndDate" DataFormatString="{0:yyyy/MM/dd}">
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                            <HeaderStyle Width="100px" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="Amount" HeaderText="金额" DataField="Amount" DataFormatString="￥{0:f2}">
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                            <HeaderStyle Width="100px" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="Quantity" HeaderText="数量" DataField="Quantity">
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                            <HeaderStyle Width="100px" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="Comment" HeaderText="详细信息" DataField="Comment">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridTemplateColumn UniqueName="Edit">
                                                            <ItemStyle HorizontalAlign="Center" Width="60" />
                                                            <HeaderStyle Width="60" />
                                                            <ItemTemplate>
                                                                <a href="javascript:void(0);" onclick="openReimbursementDetailWindow(<%#DataBinder.Eval(Container.DataItem,"ID")%>)">编辑</a>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="40" ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />
                                                    </Columns>
                                                    <CommandItemTemplate>
                                                        <table class="width100-percent">
                                                            <tr>
                                                                <td>
                                                                    <asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
                                                                        <input type="button" class="rgAdd" onclick="openReimbursementDetailWindow(-1); return false;" />
                                                                        <a href="javascript:void(0)" onclick="openReimbursementDetailWindow(-1); return false;">添加</a>
                                                                    </asp:Panel>
                                                                    <%--<asp:Panel ID="plExportCommand" runat="server" CssClass="width80 float-left">
                                            <input type="button" class="rgExpXLS" onclick="exportExcel(); return false;" />
                                            <a href="javascript:void(0);" onclick="exportExcel(); return false;">导出excel</a>
                                        </asp:Panel>--%>
                                                                </td>
                                                                <td class="right-td rightpadding10">
                                                                    <input type="button" class="rgRefresh" onclick="refreshGrid(gridClientIDs.rgReimbursementDetails); return false;" />
                                                                    <a href="javascript:void(0);" onclick="refreshGrid(gridClientIDs.rgReimbursementDetails); return false;">刷新</a>
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
                                                <ClientSettings>
                                                </ClientSettings>
                                            </telerik:RadGrid>

                                        </div>
                                    </div>
                                </div>
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
                                         OnItemCreated="rgAppPayments_ItemCreated"
                                        OnItemCommand="rgAppPayments_ItemCommand">
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

                                              

                                                <telerik:GridTemplateColumn UniqueName="ToAccount" HeaderText="转出账号" DataField="ToAccount"
                                                    SortExpression="ToAccount" FooterText="合计：" FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right">

                                                    <ItemTemplate>
                                                        <span><%# Eval("ToAccount") %></span>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <div id="divGridCombox">
                                                            <telerik:RadComboBox runat="server" ID="rcbxFromAccount" Filter="Contains" AllowCustomText="false"
                                                                MarkFirstMatch="true" Height="160px" Width="100%" EmptyMessage="--请选择--">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                        <asp:RequiredFieldValidator ID="rfvFromAccount" runat="server" ErrorMessage="请选择转出账号" CssClass="field-validation-error"
                                                            ControlToValidate="rcbxFromAccount" Display="Dynamic"></asp:RequiredFieldValidator>
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
                                                                <asp:CustomValidator ID="cvFee" runat="server" ControlToValidate="txtFee" CssClass="field-validation-error" Display="Dynamic"
                                                                ErrorMessage="" ></asp:CustomValidator>
                                                        </div>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
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
                                      <%--          <telerik:GridEditCommandColumn UniqueName="Edit" ButtonType="LinkButton" InsertText="保存" EditText="编辑" UpdateText="更新" CancelText="取消">
                                                    <HeaderStyle Width="10%" />
                                                    <ItemStyle Width="10%" />
                                                </telerik:GridEditCommandColumn>--%>

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
                                                <%--<telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="60" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />--%>


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
                        <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                        <asp:Button ID="btnSubmit" runat="server" Text="提交" CssClass="mws-button green" CausesValidation="true" OnClick="btnSubmit_Click" OnClientClick="return onBtnSaveClick();" />
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
            rgReimbursementDetails: "<%= rgReimbursementDetails.ClientID %>"
        };


        function refreshGrid(gridClientID) {
            var gridObj = $find(gridClientID);

            if (gridObj)
                gridObj.get_masterTableView().rebind();
        }

        function onClientHidden(sender, args) {
            redirectToPage('Views/HRM/ReimbursementManagement.aspx');
        }

        function onClientBlur(sender, args) {

        }
        function onBtnCancelClick() {
            redirectToPage('Views/HRM/ReimbursementManagement.aspx');
        }

        function redirectToManagementPage() {
            $.showLoading();
            window.location.href = "ReimbursementManagement.aspx";
        }

        function refreshMaintenancePage(sender, args) {

            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();
            redirectToPage("Views/Basics/ReimbursementMaintenance.aspx?EntityID=" + currentEntityID);
        }
        function onResponseEnd(sender, args) {

            var eventTarget = args.get_eventTarget();

            if ((eventTarget.indexOf("rgSupplierRefunds") >= 0
                    || eventTarget.indexOf("rgSupplierDeduction") >= 0)
                && (eventTarget.indexOf("lbtnInsert") >= 0)) {


            }
        }

        function onBtnSaveClick(sender, eventArgs) {
            var radNotification = $find("<%=radNotification.ClientID%>");

            var rgReimbursementDetails = $find("<%= rgReimbursementDetails.ClientID%>");

            if (rgReimbursementDetails.get_masterTableView()) {
                if (rgReimbursementDetails.get_masterTableView().get_dataItems().length > 0) {
                    return true
                }
            }
            radNotification.set_text("请至少填写一条报销明细");
            radNotification.show();
            return false;
        }

        function openReimbursementDetailWindow(id) {
            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();
            var targetUrl = $.getRootPath() + "Views/Basics/Editors/ReimbursementDetailMaintenance.aspx?OwnerEntityID=" + currentEntityID
                + "&EntityID=" + id + "&GridClientID=" + gridClientIDs.rgReimbursementDetails;
            $.openRadWindow(targetUrl, "winContract", true, 1000, 680);
        }
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

