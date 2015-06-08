<%@ Page Title="报销类型子类别维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReimbursementTypeChildMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.ReimbursementTypeChildMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="uc" TagName="CurrentCompany" Src="~/UserControls/UCCurrentCompany.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">报销类型子类别维护</span>
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
                            <label>报销类型</label>
                            <div class="mws-form-item small">
                                <telerik:RadTextBox runat="server" ID="txtName" CssClass="mws-textinput" Width="40%" MaxLength="100"></telerik:RadTextBox>
                                <telerik:RadToolTip ID="rttName" runat="server" TargetControlID="txtName" ShowEvent="OnClick"
                                    Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                </telerik:RadToolTip>
                                <asp:RequiredFieldValidator ID="rfvName" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtName"
                                    ErrorMessage="报销类型必填" Text="*" CssClass="field-validation-error">
                                </asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvName" runat="server" ErrorMessage="报销类型已存在，请使用其他名称"
                                    ControlToValidate="txtName" ValidationGroup="vgMaintenance"
                                    Text="*" CssClass="field-validation-error" OnServerValidate="cvName_ServerValidate">
                                </asp:CustomValidator>
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
                        <asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="mws-button orange" CausesValidation="false" OnClick="btnDelete_Click" OnClientClick="return onConfirmDelete();" />
                        <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="onClientHidden();return false;" />
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">

        function onClientHidden(sender, args) {
            var id = $.getQueryString("ParentID");
            redirectToPage("Views/Basics/ReimbursementTypeMaintenance.aspx?EntityID=" + id);
        }

    </script>
</asp:Content>
