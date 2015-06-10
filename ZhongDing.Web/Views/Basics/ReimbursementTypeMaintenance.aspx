<%@ Page Title="报销类型维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReimbursementTypeMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.ReimbursementTypeMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="uc" TagName="CurrentCompany" Src="~/UserControls/UCCurrentCompany.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">报销类型维护</span>
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
                        <div class="mws-form-row">
                            <telerik:RadGrid ID="rgReimbursementTypes" runat="server" PageSize="10"
                                AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                OnNeedDataSource="rgReimbursementTypes_NeedDataSource" OnDeleteCommand="rgReimbursementTypes_DeleteCommand"
                                OnItemCreated="rgReimbursementTypes_ItemCreated" OnColumnCreated="rgReimbursementTypes_ColumnCreated" OnItemDataBound="rgReimbursementTypes_ItemDataBound">
                                <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                    ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                    <Columns>
                                        <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false">
                                            <ItemStyle HorizontalAlign="Left" Width="50" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Name" HeaderText="报销子类别" DataField="Name">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Comment" HeaderText="备注" DataField="Comment">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>


                                        <telerik:GridTemplateColumn UniqueName="Edit" HeaderStyle-Width="40">
                                            <ItemStyle HorizontalAlign="Center" Width="40" />
                                            <ItemTemplate>
                                                <a href="javascript:void(0);" onclick="redirectToMaintenancePage(<%#DataBinder.Eval(Container.DataItem,"ID")%>)">
                                                    <u>编辑</u></a>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="40" ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />
                                    </Columns>
                                    <CommandItemTemplate>
                                        <table class="width100-percent">
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
                                                        <input type="button" class="rgAdd" onclick="redirectToMaintenancePage(-1); return false;" />
                                                        <a href="javascript:void(0)" onclick="redirectToMaintenancePage(-1); return false;">添加</a>
                                                    </asp:Panel>
                                                    <%--<asp:Panel ID="plExportCommand" runat="server" CssClass="width80 float-left">
                                            <input type="button" class="rgExpXLS" onclick="exportExcel(); return false;" />
                                            <a href="javascript:void(0);" onclick="exportExcel(); return false;">导出excel</a>
                                        </asp:Panel>--%>
                                                </td>
                                                <td class="right-td rightpadding10">
                                                    <input type="button" class="rgRefresh" onclick="refreshGrid(); return false;" />
                                                    <a href="javascript:void(0);" onclick="refreshGrid(); return false;">刷新</a>
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
                                    <ClientEvents OnGridCreated="GetsGridObject" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </div>

                        <div class="height20"></div>
                    </div>
                    <div class="mws-button-row">
                        <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                        <asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="mws-button orange" CausesValidation="false" OnClick="btnDelete_Click" OnClientClick="return onConfirmDelete();" />
                        <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Basics/ReimbursementTypeManagement.aspx');return false;" />
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">
        var gridOfRefresh = null;

        function GetsGridObject(sender, eventArgs) {
            gridOfRefresh = sender;
        }

        function refreshGrid() {
            gridOfRefresh.get_masterTableView().rebind();
        }
        function onClientHidden(sender, args) {

            redirectToPage("Views/Basics/ReimbursementTypeManagement.aspx");
        }
        function redirectToMaintenancePage(id) {
            $.showLoading();
            var parentID = $.getQueryString("EntityID");
            window.location.href = "ReimbursementTypeChildMaintenance.aspx?EntityID=" + id + "&ParentID=" + parentID;
        }

    </script>
</asp:Content>
