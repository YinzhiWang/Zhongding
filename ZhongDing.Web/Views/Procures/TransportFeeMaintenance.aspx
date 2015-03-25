<%@ Page Title="仓库维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TransportFeeMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Procures.TransportFeeMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">

        function onClientHidden(sender, args) {
            redirectToPage("Views/Procures/TransportFeeManagement.aspx");
        }

        function onClientBlur(sender, args) {


        }
        function onBtnCancelClick() {
            var transportFeeType = $.getQueryString("TransportFeeType");
            redirectToPage('Views/Procures/TransportFeeManagement.aspx?TransportFeeType=' + transportFeeType);
        }
    </script>
</asp:Content>
