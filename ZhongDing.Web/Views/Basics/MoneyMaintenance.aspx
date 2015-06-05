<%@ Page Title="银行账号维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MoneyMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.MoneyMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">现金管理维护</span>
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
                            <label>类型</label>
                            <div class="mws-form-item small">
                                <telerik:RadDropDownList runat="server" ID="rddPaymentType" DefaultMessage="--请选择--" OnSelectedIndexChanged="rddPaymentType_SelectedIndexChanged" AutoPostBack="true">
                                    <Items>
                                        <telerik:DropDownListItem Value="-1" Text="" />
                                        <telerik:DropDownListItem Value="1" Text="收款" />
                                        <telerik:DropDownListItem Value="2" Text="付款" />
                                        <telerik:DropDownListItem Value="2" Text="转账" />
                                    </Items>
                                </telerik:RadDropDownList>
                                <telerik:RadToolTip ID="RadToolTip1" runat="server" TargetControlID="rddPaymentType" ShowEvent="OnClick"
                                    Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                </telerik:RadToolTip>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="rddPaymentType"
                                    ErrorMessage="类型必填" Text="*" CssClass="field-validation-error">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label id="lblAccount" runat="server">付款账号</label>
                            <div class="mws-form-item small">
                                <telerik:RadComboBox runat="server" ID="rcbxFromAccount" Filter="Contains" AllowCustomText="false" NoWrap="true"
                                    MarkFirstMatch="true" Height="160px" Width="400px" EmptyMessage="--请选择--">
                                </telerik:RadComboBox>
                                <telerik:RadToolTip ID="rttBankBranchName" runat="server" TargetControlID="rcbxFromAccount" ShowEvent="OnClick"
                                    Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                </telerik:RadToolTip>
                                <asp:RequiredFieldValidator ID="rfvFromAccount" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="rcbxFromAccount"
                                    ErrorMessage="付款账号必填" Text="*" CssClass="field-validation-error">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>金额</label>
                            <div class="mws-form-item small">
                                <telerik:RadNumericTextBox ShowSpinButtons="true" MinValue="0" IncrementSettings-InterceptArrowKeys="true" IncrementSettings-InterceptMouseWheel="true"
                                    Label="" runat="server" ID="txtAmount" Width="160px">
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="rfvFee" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtAmount"
                                    ErrorMessage="金额必填" Text="*" CssClass="field-validation-error">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div id="receiveAccountInfo" runat="server" visible="false">

                            <div class="mws-form-row">
                                <div class="float-left width40-percent">
                                    <label><strong>收款账号信息</strong></label>
                                    <div class="mws-form-item small">
                                    </div>

                                </div>

                            </div>
                            <div class="mws-form-row">
                                <div class="float-left width40-percent">
                                    <label>户名</label>
                                    <div class="mws-form-item small">
                                        <telerik:RadTextBox runat="server" ID="txtAccountName" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtAccountName"
                                            ErrorMessage="户名必填" Text="*" CssClass="field-validation-error">
                                        </asp:RequiredFieldValidator>
                                    </div>

                                </div>
                                <div class="float-left">
                                    <label>开户行</label>
                                    <div class="mws-form-item small">
                                        <telerik:RadTextBox runat="server" ID="txtBank" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtBank"
                                            ErrorMessage="开户行必填" Text="*" CssClass="field-validation-error">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="mws-form-row">
                                <div class="float-left width40-percent">
                                    <label>账号</label>
                                    <div class="mws-form-item small">
                                        <telerik:RadTextBox runat="server" ID="txtAccount" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>
                                        <telerik:RadToolTip ID="rttAccount" runat="server" TargetControlID="txtAccount" ShowEvent="OnClick"
                                            Position="MiddleRight" RelativeTo="Element" AutoCloseDelay="0">
                                            <div>
                                                <p>帐号为16-19位数字，或如下格式:</p>
                                                <ol style="margin-left: 20px">
                                                    <li>0000 0000 0000 [4-7位数字]</li>
                                                    <li>0000-0000-0000-[4-7位数字]</li>
                                                    <li>0000 0000 0000 0000 000</li>
                                                    <li>0000-0000-0000-0000-000</li>
                                                </ol>
                                            </div>
                                        </telerik:RadToolTip>
                                        <asp:RequiredFieldValidator ID="rfvAccount" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtAccount"
                                            ErrorMessage="帐号必填" Text="*" CssClass="field-validation-error">
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revAccount" runat="server" ValidationGroup="vgMaintenance"
                                            ControlToValidate="txtAccount" ErrorMessage="帐号格式不正确，请重新输入" CssClass="field-validation-error"
                                            ValidationExpression="^\d{16,19}$|^\d{6}[- ]\d{10,13}$|^\d{4}[- ]\d{4}[- ]\d{4}[- ]\d{4,7}$|^\d{4}[- ]\d{4}[- ]\d{4}[- ]\d{4}[- ]\d{3}$" Text="*"></asp:RegularExpressionValidator>
                                        <asp:CustomValidator ID="cvAccount" runat="server" ControlToValidate="txtAccount" ValidationGroup="vgMaintenance" OnServerValidate="cvAccount_ServerValidate"
                                            Text="*" CssClass="field-validation-error" ErrorMessage="帐号无效，请重新输入"></asp:CustomValidator>
                                    </div>

                                </div>

                            </div>

                        </div>
                        <div class="mws-form-row">
                            <label>备注</label>
                            <div class="mws-form-item medium">
                                <telerik:RadTextBox runat="server" ID="txtComment" Width="90%" MaxLength="1000"
                                    TextMode="MultiLine" Height="80">
                                </telerik:RadTextBox>
                            </div>
                        </div>
                        <div class="height20"></div>
                    </div>
                    <div class="mws-button-row">
                        <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                        <%--<asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="mws-button orange" CausesValidation="false" OnClick="btnDelete_Click" OnClientClick="return onConfirmDelete();" />--%>
                        <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Basics/MoneyManagement.aspx');return false;" />
                    </div>
                </div>
            </div>
        </div>

    </div>
    <style>
        .RadToolTip .rtWrapper td.rtWrapperContent {
            padding-top: 3px;
            padding-bottom: 3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">
        // <![CDATA[

        function onClientHidden(sender, args) {
            redirectToPage("Views/Basics/MoneyManagement.aspx");
        }

        // ]]>
    </script>
</asp:Content>
