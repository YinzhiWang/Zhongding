<%@ Page Title="员工维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserGroupMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.HRM.UserGroupMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">用户组维护</span>
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
                            <div class="float-left width30-percent">
                                <label>用户组名称</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtGroupName" CssClass="mws-textinput" Width="160px" MaxLength="100"></telerik:RadTextBox>
                                    <telerik:RadToolTip ID="rttCompanyName" runat="server" TargetControlID="txtGroupName" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                    <asp:RequiredFieldValidator ID="rfvCompanyName" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtGroupName"
                                        ErrorMessage="用户组名称必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvCompanyName" runat="server" ErrorMessage="物流公司名称已存在，请使用其他名称"
                                        ControlToValidate="txtGroupName" ValidationGroup="vgMaintenance"
                                        Text="*" CssClass="field-validation-error" OnServerValidate="cvCompanyName_ServerValidate">
                                    </asp:CustomValidator>
                                </div>

                            </div>
                            <div class="float-left">
                                <label>用户组描述</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtComment" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>

                                </div>

                            </div>
                        </div>

                        <div class="mws-form-row">
                            <div class="float-left width55-percent">
                                <telerik:RadListBox runat="server" ID="lbxAllUsers" AllowTransfer="true" AllowTransferOnDoubleClick="true"
                                    SelectionMode="Multiple"
                                    TransferMode="Move" TransferToID="lbxSelectedUsers" Width="98%" Height="500">
                                    <HeaderTemplate>
                                        <table class="width100-percent">
                                            <tr>
                                                <td>全部用户</td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ButtonSettings Position="Right" ShowDelete="false" ShowReorder="false"
                                        TransferButtons="All" VerticalAlign="Bottom" AreaWidth="35" />
                                    <Localization AllToRight="全部移到右边" AllToLeft="全部移到左边" ToRight="移到右边" ToLeft="移到左边" />
                                </telerik:RadListBox>
                            </div>
                            <div class="float-left width45-percent">
                                <telerik:RadListBox runat="server" ID="lbxSelectedUsers" AllowTransferOnDoubleClick="true" Width="100%" Height="500"
                                    SelectionMode="Multiple">
                                    <HeaderTemplate>
                                        <table class="width100-percent">
                                            <tr>
                                                <td>已选用户</td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </telerik:RadListBox>
                            </div>
                        </div>


                        <%--                        <div class="mws-form-row">
                            <label>备注</label>
                            <div class="mws-form-item medium">
                                <telerik:RadTextBox runat="server" ID="txtRemark" Width="90%" MaxLength="1000"
                                    TextMode="MultiLine" Height="80">
                                </telerik:RadTextBox>
                            </div>
                        </div>--%>
                        <div class="height20"></div>
                    </div>
                    <div class="mws-button-row">
                        <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                        <asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="mws-button orange" CausesValidation="false" OnClick="btnDelete_Click" OnClientClick="return onConfirmDelete();" />
                        <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/HRM/UserGroupManagement.aspx');return false;" />
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">

        function onClientHidden(sender, args) {
            redirectToPage("Views/HRM/UserGroupManagement.aspx");
        }

    </script>
</asp:Content>
