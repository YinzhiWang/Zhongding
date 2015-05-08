<%@ Page Title="仓库维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TransportFeeMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Procures.TransportFeeMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>

            <telerik:AjaxSetting AjaxControlID="rgStockIns">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgStockIns" LoadingPanelID="loadingPanel" />

                </UpdatedControls>
            </telerik:AjaxSetting>
              <telerik:AjaxSetting AjaxControlID="rgStockOuts">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgStockOuts" LoadingPanelID="loadingPanel" />

                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbxTransportCompany">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbxTransportCompany" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="rcbxTransportCompany" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="txtDriver" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="txtDriverTelephone" LoadingPanelID="loadingPanel" />

                </UpdatedControls>
            </telerik:AjaxSetting>


        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="container">

        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">物流费用维护</span>
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
                                <label>物流单类别</label>
                                <div class="mws-form-item small">
                                    <telerik:RadButton ID="rbtnStockIn" Checked="true" runat="server" ToggleType="Radio" ButtonType="StandardButton" GroupName="rbtnStock">
                                        <ToggleStates>
                                            <telerik:RadButtonToggleState Text="入库" PrimaryIconCssClass="rbToggleRadioChecked" />
                                            <telerik:RadButtonToggleState Text="入库" PrimaryIconCssClass="rbToggleRadio" />
                                        </ToggleStates>
                                    </telerik:RadButton>
                                    <telerik:RadButton ID="rbtnStockOut" runat="server" ToggleType="Radio" ButtonType="StandardButton" GroupName="rbtnStock">
                                        <ToggleStates>
                                            <telerik:RadButtonToggleState Text="出库" PrimaryIconCssClass="rbToggleRadioChecked" />
                                            <telerik:RadButtonToggleState Text="出库" PrimaryIconCssClass="rbToggleRadio" />
                                        </ToggleStates>
                                    </telerik:RadButton>
                                </div>

                            </div>
                            <div class="float-left">

                                <asp:Label ID="lblOperator" runat="server" Text="操作人：" />
                            </div>
                        </div>

                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>物流公司</label>
                                <div class="mws-form-item small">
                                    <telerik:RadComboBox runat="server" ID="rcbxTransportCompany" Filter="Contains" AllowCustomText="true" OnSelectedIndexChanged="rcbxTransportCompany_SelectedIndexChanged"
                                        MarkFirstMatch="true" Height="160px" EmptyMessage="--请选择--" AutoPostBack="true">
                                    </telerik:RadComboBox>
                                    <telerik:RadToolTip ID="rttTransportCompany" runat="server" TargetControlID="rcbxTransportCompany" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                    <asp:CustomValidator ID="cvTransportCompany" runat="server" ErrorMessage="请选择物流公司"
                                        ControlToValidate="rcbxTransportCompany" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                </div>

                            </div>
                            <div class="float-left">
                                <label>物流单号</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtTransportCompanyNumber" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="rfvTransportCompanyNumber" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtTransportCompanyNumber"
                                        ErrorMessage="物流单号必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                    <telerik:RadToolTip ID="RadToolTip1" runat="server" TargetControlID="txtTransportCompanyNumber" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>司机</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtDriver" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>

                                </div>

                            </div>
                            <div class="float-left">
                                <label>司机电话</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtDriverTelephone" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>
                                    <asp:RegularExpressionValidator ID="revDriverTelephone" runat="server"
                                        ControlToValidate="txtDriverTelephone"
                                        ErrorMessage="司机电话格式不正确"
                                        ValidationExpression="(\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$"
                                        CssClass="field-validation-error" Display="Dynamic"
                                        ValidationGroup="vgMaintenance" Text="*"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>


                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>起点</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtStartPlace" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>

                                </div>

                            </div>
                            <div class="float-left">
                                <label>起点电话</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtStartTelephone" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>
                                    <asp:RegularExpressionValidator ID="revStartTelephone" runat="server"
                                        ControlToValidate="txtStartTelephone"
                                        ErrorMessage="起点电话格式不正确"
                                        ValidationExpression="(\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$"
                                        CssClass="field-validation-error" Display="Dynamic"
                                        ValidationGroup="vgMaintenance" Text="*"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>终点</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtEndPlace" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>

                                </div>

                            </div>
                            <div class="float-left">
                                <label>终点电话</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtEndPlaceTelephone" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>
                                    <asp:RegularExpressionValidator ID="revEndPlaceTelephone" runat="server"
                                        ControlToValidate="txtEndPlaceTelephone"
                                        ErrorMessage="终点电话格式不正确"
                                        ValidationExpression="(\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$"
                                        CssClass="field-validation-error" Display="Dynamic"
                                        ValidationGroup="vgMaintenance" Text="*"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>费用金额</label>
                                <div class="mws-form-item small">
                                    <telerik:RadNumericTextBox ShowSpinButtons="true" MinValue="0" IncrementSettings-InterceptArrowKeys="true" IncrementSettings-InterceptMouseWheel="true"
                                        Label="" runat="server" ID="txtFee" Width="160px">
                                    </telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="rfvFee" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtFee"
                                        ErrorMessage="费用金额必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                </div>

                            </div>
                            <div class="float-left">

                                <label>发货日期</label>
                                <div class="mws-form-item small">
                                    <telerik:RadDatePicker ID="txtSendDate" Width="200px" runat="server">
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="rfvSendDate" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtSendDate"
                                        ErrorMessage="发货日期必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                    <telerik:RadToolTip ID="rttSendDate" runat="server" TargetControlID="txtSendDate" ShowEvent="OnClick"
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
                        <div class="mws-form-row" runat="server" id="divOtherSections">
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-product-specification">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">关联单据</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <telerik:RadGrid ID="rgStockIns" runat="server" PageSize="10"
                                            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgStockIns_NeedDataSource" OnDeleteCommand="rgStockIns_DeleteCommand">
                                            <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                                <Columns>
                                                    <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="Code" HeaderText="入库单编号" DataField="Code">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn UniqueName="EntryDate" HeaderText="入库日期" DataField="EntryDate">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn UniqueName="CreatedByText" HeaderText="操作人" DataField="CreatedByText">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="40" ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />
                                                </Columns>
                                                <CommandItemTemplate>
                                                    <table class="width100-percent">
                                                        <tr>
                                                            <td>
                                                                <asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
                                                                    <input type="button" class="rgAdd" onclick="openSelectStockInWindow(); return false;" />
                                                                    <a href="javascript:void(0)" onclick="openSelectStockInWindow(); return false;">添加</a>
                                                                </asp:Panel>
                                                            </td>
                                                            <td class="right-td rightpadding10">
                                                                <input type="button" class="rgRefresh" onclick="refreshGrid(gridClientIDs.gridStockIns); return false;" />
                                                                <a href="javascript:void(0);" onclick="refreshGrid(gridClientIDs.gridStockIns); return false;">刷新</a>
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
                                        </telerik:RadGrid>

                                        <telerik:RadGrid ID="rgStockOuts" runat="server" PageSize="10"
                                            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgStockOuts_NeedDataSource" OnDeleteCommand="rgStockOuts_DeleteCommand">
                                            <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                                <Columns>
                                                    <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="Code" HeaderText="出库单编号" DataField="Code">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn UniqueName="ReceiverName" HeaderText="收货人" DataField="ReceiverName">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn UniqueName="ReceiverPhone" HeaderText="收货电话" DataField="ReceiverPhone">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="ReceiverAddress" HeaderText="收货地址" DataField="ReceiverAddress">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="LastTransportFeeStockOutSmsReminderDate" HeaderText="上次提醒时间" DataField="LastTransportFeeStockOutSmsReminderDate">
                                                        <HeaderStyle Width="80" />
                                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="SmsReminder">
                                                        <HeaderStyle Width="60" />
                                                        <ItemStyle HorizontalAlign="Center" Width="60" />
                                                        <ItemTemplate>
                                                            <a href="javascript:void(0);" onclick="openStockOutSmsReminderWindow(<%#DataBinder.Eval(Container.DataItem,"ID")%>)">短信提醒</a>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="40" ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />
                                                </Columns>
                                                <CommandItemTemplate>
                                                    <table class="width100-percent">
                                                        <tr>
                                                            <td>
                                                                <asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
                                                                    <input type="button" class="rgAdd" onclick="openSelectStockOutWindow(); return false;" />
                                                                    <a href="javascript:void(0)" onclick="openSelectStockOutWindow(); return false;">添加</a>
                                                                </asp:Panel>
                                                            </td>
                                                            <td class="right-td rightpadding10">
                                                                <input type="button" class="rgRefresh" onclick="refreshGrid(gridClientIDs.gridStockOuts); return false;" />
                                                                <a href="javascript:void(0);" onclick="refreshGrid(gridClientIDs.gridStockOuts); return false;">刷新</a>
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
                                        </telerik:RadGrid>
                                    </div>
                                </div>
                            </div>



                        </div>
                        <div class="height20"></div>
                    </div>
                    <div class="mws-button-row">
                        <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                        <asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="mws-button orange" CausesValidation="false" OnClick="btnDelete_Click" OnClientClick="return onConfirmDelete();" />
                        <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="onBtnCancelClick();return false;" />
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
            gridStockIns: "<%= rgStockIns.ClientID %>",
            gridStockOuts: "<%= rgStockOuts.ClientID %>",
        };

        function refreshGrid(gridClientID) {
            var gridObj = $find(gridClientID);

            if (gridObj)
                gridObj.get_masterTableView().rebind();
        }

        function openSelectStockInWindow() {
            $.showLoading();

            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            var targetUrl = $.getRootPath() + "Views/Procures/Editors/ChooseStockInForTransportFee.aspx?OwnerEntityID=" + currentEntityID + "&GridClientID=" + gridClientIDs.gridStockIns;

            $.openRadWindow(targetUrl, "winChooseStockInForTransportFee", true, 1000, 660);
        }
        function openSelectStockOutWindow() {
            $.showLoading();

            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            var targetUrl = $.getRootPath() + "Views/Procures/Editors/ChooseStockOutForTransportFee.aspx?OwnerEntityID=" + currentEntityID + "&GridClientID=" + gridClientIDs.gridStockOuts;

            $.openRadWindow(targetUrl, "winChooseStockOutForTransportFee", true, 1000, 660);
        }
        function onClientHidden(sender, args) {
            var transportFeeType = $.getQueryString("TransportFeeType");
            redirectToPage('Views/Procures/TransportFeeManagement.aspx?TransportFeeType=' + transportFeeType);
        }
        function refreshMaintenancePage(sender, args) {

            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();
            var transportFeeType = $.getQueryString("TransportFeeType");
            redirectToPage("Views/Procures/TransportFeeMaintenance.aspx?TransportFeeType=" + transportFeeType + "&EntityID=" + currentEntityID);
        }
        function onClientBlur(sender, args) {


        }
        function onBtnCancelClick() {
            var transportFeeType = $.getQueryString("TransportFeeType");
            redirectToPage('Views/Procures/TransportFeeManagement.aspx?TransportFeeType=' + transportFeeType);
        }

        function openStockOutSmsReminderWindow(id) {
            $.showLoading();

            var currentEntityID = id;

            var targetUrl = $.getRootPath() + "Views/Sales/Editors/StockOutSmsReminder.aspx?OwnerEntityID=" + currentEntityID + "&GridClientID=" + gridClientIDs.gridStockOuts;

            $.openRadWindow(targetUrl, "winStockOutSmsReminderWindow", true, 700, 400);
        }
    </script>
</asp:Content>
