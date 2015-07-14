<%@ Page Title="银行帐号维护" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="ReimbursementDetailMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.Editors.ReimbursementDetailMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Window.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="mws-panel grid_full" style="margin-bottom: 10px;">

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
                            <label>报销类型</label>
                            <div class="mws-form-item small">
                                <telerik:RadComboBox runat="server" ID="rcbxReimbursementType" Filter="Contains" AllowCustomText="true"
                                    MarkFirstMatch="true" Height="160px" EmptyMessage="--请选择--"
                                    AutoPostBack="true" OnSelectedIndexChanged="rcbxReimbursementType_SelectedIndexChanged">
                                </telerik:RadComboBox>
                                <telerik:RadToolTip ID="rttReimbursementType" runat="server" TargetControlID="rcbxReimbursementType" ShowEvent="OnMouseOver"
                                    Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                </telerik:RadToolTip>
                                <asp:CustomValidator ID="cvReimbursementType" runat="server" ErrorMessage="请选择资产类别"
                                    ControlToValidate="rcbxReimbursementType" ValidationGroup="vgMaintenance" Display="Dynamic"
                                    Text="*" CssClass="field-validation-error">
                                </asp:CustomValidator>

                            </div>

                        </div>
                        <div class="float-left">
                            <label>子类型</label>
                            <div class="mws-form-item small">
                                <telerik:RadComboBox runat="server" ID="rcbxReimbursementChildType" Filter="Contains" AllowCustomText="true"
                                    MarkFirstMatch="true" Height="160px" EmptyMessage="--请选择--">
                                </telerik:RadComboBox>
                                <telerik:RadToolTip ID="rttReimbursementChildType" runat="server" TargetControlID="rcbxReimbursementChildType" ShowEvent="OnMouseOver"
                                    Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                </telerik:RadToolTip>
                                <asp:CustomValidator ID="cvReimbursementChildType" runat="server" ErrorMessage="请选择资产类别"
                                    ControlToValidate="rcbxReimbursementChildType" ValidationGroup="vgMaintenance" Display="Dynamic"
                                    Text="*" CssClass="field-validation-error">
                                </asp:CustomValidator>
                            </div>
                        </div>
                    </div>

                    <div id="divOtherTypes" runat="server">
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>开始日期</label>
                                <div class="mws-form-item small">
                                    <telerik:RadDatePicker ID="txtStartDate" Width="200px" runat="server">
                                    </telerik:RadDatePicker>

                                    <asp:CustomValidator ID="cvStartDate" runat="server" ErrorMessage="开始日期必填"
                                        ControlToValidate="txtStartDate" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                    <telerik:RadToolTip ID="rttSendDate" runat="server" TargetControlID="txtStartDate" ShowEvent="OnMouseOver"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>

                                </div>

                            </div>
                            <div class="float-left">
                                <label>结束日期</label>
                                <div class="mws-form-item small">
                                    <telerik:RadDatePicker ID="txtEndDate" Width="200px" runat="server">
                                    </telerik:RadDatePicker>
                                    <asp:CustomValidator ID="cvEndDate" runat="server" ErrorMessage="结束日期必填"
                                        ControlToValidate="txtEndDate" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                    <telerik:RadToolTip ID="rttEndDate" runat="server" TargetControlID="txtEndDate" ShowEvent="OnMouseOver"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>数量</label>
                                <div class="mws-form-item small">
                                    <telerik:RadNumericTextBox ShowSpinButtons="true" MinValue="0" NumberFormat-DecimalDigits="0"
                                        IncrementSettings-InterceptArrowKeys="true" IncrementSettings-InterceptMouseWheel="true"
                                        Label="" runat="server" ID="txtQuantity" Width="160px">
                                    </telerik:RadNumericTextBox>
                                    <%-- <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtQuantity"
                                        ErrorMessage="数量必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>--%>
                                </div>

                            </div>
                            <div class="float-left">
                                <label>金额</label>
                                <div class="mws-form-item small">
                                    <telerik:RadNumericTextBox ShowSpinButtons="true" MinValue="0" IncrementSettings-InterceptArrowKeys="true"
                                        IncrementSettings-InterceptMouseWheel="true"
                                        Label="" runat="server" ID="txtAmount" Width="160px">
                                    </telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtAmount"
                                        ErrorMessage="金额必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>详细信息</label>
                            <div class="mws-form-item medium">
                                <telerik:RadTextBox runat="server" ID="txtComment" Width="90%" MaxLength="1000"
                                    TextMode="MultiLine" Height="80">
                                </telerik:RadTextBox>
                            </div>
                        </div>
                    </div>
                    <div id="divTransportFee" runat="server">
                        <div class="mws-form-row">
                            <telerik:RadGrid ID="rgTransportFees" runat="server" PageSize="10"
                                AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                OnNeedDataSource="rgTransportFees_NeedDataSource" OnItemDataBound="rgTransportFees_ItemDataBound"
                                OnColumnCreated="rgTransportFees_ColumnCreated">
                                <MasterTableView Width="100%" DataKeyNames="ID,Fee,ReimbursementDetailTransportFeeID,ReimbursementDetailID,SendDate" CommandItemDisplay="None"
                                    ShowHeadersWhenNoRecords="true" BackColor="#fafafa" ClientDataKeyNames="ID,Fee,ReimbursementDetailTransportFeeID,ReimbursementDetailID">
                                    <Columns>
                                        <telerik:GridTemplateColumn UniqueName="Create">
                                            <HeaderStyle Width="70" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="70" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cBoxHasSelect" runat="server" Checked='<%#int.Parse(DataBinder.Eval(Container.DataItem,"ReimbursementDetailTransportFeeID").ToString())>0?true:false %>' />
                                            </ItemTemplate>
                                            <HeaderTemplate>
                                                <%--    <asp:CheckBox ID="cBoxSelect" runat="server"/>--%>
                                                选择
                                            </HeaderTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn UniqueName="TransportCompanyNumber" HeaderText="物流单号" DataField="TransportCompanyNumber" ReadOnly="true">
                                            <HeaderStyle Width="160px" />
                                            <ItemStyle HorizontalAlign="Left" Width="160px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="TransportFeeTypeText" HeaderText="类别" DataField="TransportFeeTypeText" ReadOnly="true">
                                            <HeaderStyle Width="100px" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn UniqueName="SendDate" HeaderText="日期" DataField="SendDate" ReadOnly="true" DataFormatString="{0:yyyy/MM/dd}">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Fee" HeaderText="金额" DataField="Fee" ReadOnly="true" DataFormatString="￥{0:f2}">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                        </telerik:GridBoundColumn>


                                        <telerik:GridTemplateColumn UniqueName="Comment" HeaderText="备注" DataField="Comment">
                                            <HeaderStyle />
                                            <ItemStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <telerik:RadTextBox runat="server" ID="txtComment" Width="100%" MaxLength="200"
                                                    TextMode="SingleLine" Text='<%#DataBinder.Eval(Container.DataItem,"Comment") %>'>
                                                </telerik:RadTextBox>
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
                                </ClientSettings>
                            </telerik:RadGrid>

                        </div>

                    </div>
                    <div class="height20"></div>
                </div>
                <div class="mws-button-row">
                    <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" OnClientClick="return onBtnSaveClick();" />
                    <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="closeWindow(false);return false;" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnGridClientID" runat="server" />
    <telerik:RadNotification ID="radNotification" runat="server" EnableRoundedCorners="true"
        AutoCloseDelay="1000" Skin="Silk" Animation="Fade" EnableShadow="true" Title="提示"
        TitleIcon="none" Opacity="95" Position="Center" ContentIcon="~/Content/icons/32/cross.png"
        Width="300" Height="100">
    </telerik:RadNotification>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">
        function closeWindow(needRebindGrid) {

            var oWin = $.getRadWindow();

            if (oWin) {

                if (needRebindGrid) {

                    var browserWindow = oWin.get_browserWindow();

                    var gridClientID = $("#<%= hdnGridClientID.ClientID%>").val();

                    if (!gridClientID.isNullOrEmpty()) {
                        var refreshGrid = browserWindow.$find(gridClientID);

                        if (refreshGrid) {
                            refreshGrid.get_masterTableView().rebind();
                        }
                    }
                }

                var isDestroyOnClose = oWin.get_destroyOnClose();
                if (isDestroyOnClose) {
                    oWin.set_destroyOnClose(false);
                }

                if (!oWin.isClosed()) {
                    oWin.close();
                }
            }
        }

        function onClientHidden(sender, args) {
            closeWindow(true);
        }

        function onError(sender, args) {
            closeWindow(false);
        }

        function onBtnSaveClick(sender, args) {

            var rcbxReimbursementType = $find("<%= rcbxReimbursementType.ClientID %>");

            if (rcbxReimbursementType.get_text() == "物流费用") {
                var rgTransportFees = $find("<%=rgTransportFees.ClientID%>")
                var masterTableView = rgTransportFees.get_masterTableView();

                var hasChecked = false;
                var cBoxs = $(masterTableView.get_element()).find("input[type='checkbox']");
                $(cBoxs).each(function () {
                    if ($(this).prop("checked")) {
                        hasChecked = true;
                    }
                });
                if (hasChecked == false) {
                    var radNotification = $find("<%=radNotification.ClientID%>");
                    radNotification.set_text("必须选择物流单");
                    radNotification.show();
                    return false;
                }
                return true;
            }
            return true;
        }

    </script>
</asp:Content>
