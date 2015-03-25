<%@ Page Title="仓库维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TransportCompanyMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.TransportCompanyMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">物流公司维护</span>
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
                            <label>物流公司名称</label>
                            <div class="mws-form-item small">
                                <telerik:RadTextBox runat="server" ID="txtCompanyName" CssClass="mws-textinput" Width="40%" MaxLength="100"></telerik:RadTextBox>
                                <telerik:RadToolTip ID="rttCompanyName" runat="server" TargetControlID="txtCompanyName" ShowEvent="OnClick"
                                    Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                </telerik:RadToolTip>
                                <asp:RequiredFieldValidator ID="rfvCompanyName" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtCompanyName"
                                    ErrorMessage="物流公司名称必填" Text="*" CssClass="field-validation-error">
                                </asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvCompanyName" runat="server" ErrorMessage="物流公司名称已存在，请使用其他名称"
                                    ControlToValidate="txtCompanyName" ValidationGroup="vgMaintenance"
                                    Text="*" CssClass="field-validation-error" OnServerValidate="cvCompanyName_ServerValidate">
                                </asp:CustomValidator>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>地址</label>
                            <div class="mws-form-item small">
                                <telerik:RadTextBox runat="server" ID="txtCompanyAddress" CssClass="mws-textinput" Width="40%" MaxLength="200"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="rfvCompanyAddress" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtCompanyAddress"
                                    ErrorMessage="物流公司地址必填" Text="*" CssClass="field-validation-error">
                                </asp:RequiredFieldValidator>
                                <telerik:RadToolTip ID="rttCompanyAddress" runat="server" TargetControlID="txtCompanyAddress" ShowEvent="OnClick"
                                    Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                </telerik:RadToolTip>
                            </div>
                        </div>

                        <div class="mws-form-row">
                            <label>电话</label>
                            <div class="mws-form-item small">
                                <telerik:RadTextBox runat="server" ID="txtTelephone" CssClass="mws-textinput" Width="40%" MaxLength="200"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="rfvTelephone" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtTelephone"
                                    ErrorMessage="电话必填" Text="*" CssClass="field-validation-error">
                                </asp:RequiredFieldValidator>
                                <telerik:RadToolTip ID="rttTelephone" runat="server" TargetControlID="txtTelephone" ShowEvent="OnClick"
                                    Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                </telerik:RadToolTip>
                                <asp:RegularExpressionValidator ID="revTelephone" runat="server"
                                    ControlToValidate="txtTelephone"
                                    ErrorMessage="电话格式不正确"
                                    ValidationExpression="(\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$"
                                    CssClass="field-validation-error" Display="Dynamic"
                                    ValidationGroup="vgMaintenance" Text="*"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>司机</label>
                            <div class="mws-form-item small">
                                <telerik:RadTextBox runat="server" ID="txtDriver" CssClass="mws-textinput" Width="40%" MaxLength="200"></telerik:RadTextBox>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>司机电话</label>
                            <div class="mws-form-item small">
                                <telerik:RadTextBox runat="server" ID="txtDriverTelephone" CssClass="mws-textinput" Width="40%" MaxLength="200"></telerik:RadTextBox>
                                  <asp:RegularExpressionValidator ID="revDriverTelephone" runat="server"
                                        ControlToValidate="txtDriverTelephone"
                                        ErrorMessage="司机电话格式不正确"
                                        ValidationExpression="(\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$"
                                        CssClass="field-validation-error" Display="Dynamic"
                                        ValidationGroup="vgMaintenance" Text="*"></asp:RegularExpressionValidator>
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
                        <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Basics/TransportCompanyManagement.aspx');return false;" />
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">

        function onClientHidden(sender, args) {
            redirectToPage("Views/Basics/TransportCompanyManagement.aspx");
        }

    </script>
</asp:Content>
