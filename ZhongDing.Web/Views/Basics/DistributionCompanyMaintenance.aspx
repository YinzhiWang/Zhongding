<%@ Page Title="配送公司维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DistributionCompanyMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.DistributionCompanyMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">配送公司维护</span>
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
                            <label>配送公司编号</label>
                            <div class="mws-form-item small">
                                <telerik:RadTextBox runat="server" ID="txtSerialNo" CssClass="mws-textinput" Width="40%" Enabled="false"></telerik:RadTextBox>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>配送公司名称</label>
                            <div class="mws-form-item small">
                                <telerik:RadTextBox runat="server" ID="txtName" CssClass="mws-textinput" Width="40%" MaxLength="100"></telerik:RadTextBox>
                                <telerik:RadToolTip ID="rttName" runat="server" TargetControlID="txtName" ShowEvent="OnClick"
                                    Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                </telerik:RadToolTip>
                                <asp:RequiredFieldValidator ID="rfvName" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtName"
                                    ErrorMessage="配送公司名称必填" Text="*" CssClass="field-validation-error">
                                </asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvName" runat="server" ErrorMessage="配送公司名称已存在，请使用其他名称"
                                    ControlToValidate="txtName" ValidationGroup="vgMaintenance"
                                    Text="*" CssClass="field-validation-error" OnServerValidate="cvName_ServerValidate">
                                </asp:CustomValidator>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>收货人</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtReceiverName" CssClass="mws-textinput" Width="60%" MaxLength="50"></telerik:RadTextBox>
                                </div>
                            </div>
                            <div class="float-left width40-percent">
                                <label>收货电话</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtPhoneNumber" CssClass="mws-textinput" Width="60%" MaxLength="20"></telerik:RadTextBox>
                                    <asp:RegularExpressionValidator ID="revPhoneNumber" runat="server"
                                        ControlToValidate="txtPhoneNumber"
                                        ErrorMessage="收货电话格式不正确！"
                                        ValidationExpression="(\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$"
                                        CssClass="field-validation-error" Display="Dynamic"
                                        ValidationGroup="vgMaintenance" Text="*"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>收货地址</label>
                            <div class="mws-form-item medium">
                                <telerik:RadTextBox runat="server" ID="txtAddress" CssClass="mws-textinput" Width="80%" MaxLength="200"></telerik:RadTextBox>
                            </div>
                        </div>

                        <div class="height20"></div>
                    </div>
                    <div class="mws-button-row">
                        <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                        <asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="mws-button orange" CausesValidation="false" OnClick="btnDelete_Click" OnClientClick="return onConfirmDelete();" />
                        <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Basics/DistributionCompanyManagement.aspx');return false;" />
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">

        function onClientHidden(sender, args) {
            redirectToPage("Views/Basics/DistributionCompanyManagement.aspx");
        }

    </script>
</asp:Content>
