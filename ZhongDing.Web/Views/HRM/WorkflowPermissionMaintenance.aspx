<%@ Page Title="权限维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WorkflowPermissionMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.HRM.WorkflowPermissionMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="mws-panel grid_full">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">权限维护</span>
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
                            <label>工作流名称</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblWorkflowName" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>操作权限</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblWorkflowStepName" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width55-percent">
                                <telerik:RadListBox runat="server" ID="lbxAllUsers" AllowTransfer="true" AllowTransferOnDoubleClick="true"
                                    TransferMode="Move" TransferToID="lbxSelectedUsers" Width="98%" Height="200">
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
                                <telerik:RadListBox runat="server" ID="lbxSelectedUsers" AllowTransferOnDoubleClick="true" Width="100%" Height="200">
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
                          <div class="mws-form-row">
                            <div class="float-left width55-percent">
                                <telerik:RadListBox runat="server" ID="lbxAllUserGroup" AllowTransfer="true" AllowTransferOnDoubleClick="true"
                                    TransferMode="Move" TransferToID="lbxSelectedUserGroup" Width="98%" Height="200">
                                    <HeaderTemplate>
                                        <table class="width100-percent">
                                            <tr>
                                                <td>全部用户组</td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ButtonSettings Position="Right" ShowDelete="false" ShowReorder="false"
                                        TransferButtons="All" VerticalAlign="Bottom" AreaWidth="35" />
                                    <Localization AllToRight="全部移到右边" AllToLeft="全部移到左边" ToRight="移到右边" ToLeft="移到左边" />
                                </telerik:RadListBox>
                            </div>
                            <div class="float-left width45-percent">
                                <telerik:RadListBox runat="server" ID="lbxSelectedUserGroup" AllowTransferOnDoubleClick="true" Width="100%" Height="200">
                                    <HeaderTemplate>
                                        <table class="width100-percent">
                                            <tr>
                                                <td>已选用户组</td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </telerik:RadListBox>
                            </div>
                        </div>
                        <div class="height20">
                        </div>
                        <div class="mws-button-row">
                            <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/HRM/WorkflowPermissionManagement.aspx');return false;" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">

        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/HRM/WorkflowPermissionManagement.aspx");
        }

    </script>
</asp:Content>
