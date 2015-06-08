<%@ Page Title="用户组权限维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserGroupPermissionMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.HRM.UserGroupPermissionMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .rgUserGroupPermissionsHeaderTemplate input, label {
            float: left;
        }

        .rgUserGroupPermissionsHeaderTemplate label {
            width:40px !important;
        }
    </style>
    <div class="container">

        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">用户组权限维护</span>
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
                            <div class="validate-message-wrapper">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vgMaintenance" DisplayMode="BulletList" HeaderText="请更正以下错误:" CssClass="validation-summary-errors" />
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <asp:Label ID="lblUserGroupName" runat="server"></asp:Label>

                        </div>
                        <div class="height20">
                        </div>
                    </div>
                    <div class="rgUserGroupPermissions">
                        <telerik:RadGrid ID="rgUserGroupPermissions" runat="server" PageSize="10"
                            AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                            OnNeedDataSource="rgUserGroupPermissions_NeedDataSource" OnDeleteCommand="rgUserGroupPermissions_DeleteCommand"
                            OnItemCreated="rgUserGroupPermissions_ItemCreated" OnColumnCreated="rgUserGroupPermissions_ColumnCreated" OnItemDataBound="rgUserGroupPermissions_ItemDataBound">
                            <MasterTableView Width="100%" DataKeyNames="PermissionID" CommandItemDisplay="Top"
                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                <Columns>
                                    <telerik:GridBoundColumn UniqueName="PermissionID" HeaderText="PermissionID" DataField="PermissionID" Visible="false">
                                        <ItemStyle HorizontalAlign="Left" Width="50" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="Name" HeaderText="权限名称" DataField="Name">
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn UniqueName="Create">
                                        <HeaderStyle Width="70" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="70" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cBoxHasCreate" runat="server" Enabled='<%#DataBinder.Eval(Container.DataItem,"HasCreate") %>' Checked='<%#DataBinder.Eval(Container.DataItem,"HasPermissionCreate") %>' />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <div class="rgUserGroupPermissionsHeaderTemplate">
                                                <asp:CheckBox ID="cBoxHasCreateAll" runat="server" Text="创建" /></div>

                                        </HeaderTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="Edit">
                                        <HeaderStyle Width="70" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="70" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cBoxHasEdit" runat="server" Enabled='<%#DataBinder.Eval(Container.DataItem,"HasEdit") %>' Checked='<%#DataBinder.Eval(Container.DataItem,"HasPermissionEdit") %>' />
                                        </ItemTemplate>
                                        <HeaderTemplate> <div class="rgUserGroupPermissionsHeaderTemplate">
                                            <asp:CheckBox ID="cBoxHasEditAll" runat="server" Text="修改" /></div>
                                        </HeaderTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="Delete">
                                        <HeaderStyle Width="70" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="70" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cBoxHasDelete" runat="server" Enabled='<%#DataBinder.Eval(Container.DataItem,"HasDelete") %>' Checked='<%#DataBinder.Eval(Container.DataItem,"HasPermissionDelete") %>' />
                                        </ItemTemplate>
                                        <HeaderTemplate> <div class="rgUserGroupPermissionsHeaderTemplate">
                                            <asp:CheckBox ID="cBoxHasDeleteAll" runat="server" Text="删除" /></div>
                                        </HeaderTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="View">
                                        <HeaderStyle Width="70" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="70" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cBoxHasView" runat="server" Enabled='<%#DataBinder.Eval(Container.DataItem,"HasView") %>' Checked='<%#DataBinder.Eval(Container.DataItem,"HasPermissionView") %>' />
                                        </ItemTemplate>
                                        <HeaderTemplate> <div class="rgUserGroupPermissionsHeaderTemplate">
                                            <asp:CheckBox ID="cBoxHasViewAll" runat="server" Text="查看" /></div>
                                        </HeaderTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="Print">
                                        <HeaderStyle Width="70" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="70" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cBoxHasPrint" runat="server" Enabled='<%#DataBinder.Eval(Container.DataItem,"HasPrint") %>' Checked='<%#DataBinder.Eval(Container.DataItem,"HasPermissionPrint") %>' />
                                        </ItemTemplate>
                                        <HeaderTemplate> <div class="rgUserGroupPermissionsHeaderTemplate">
                                            <asp:CheckBox ID="cBoxHasPrintAll" runat="server" Text="打印" /></div>
                                        </HeaderTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Export">
                                        <HeaderStyle Width="70" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="70" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cBoxHasExport" runat="server" Enabled='<%#DataBinder.Eval(Container.DataItem,"HasExport") %>' Checked='<%#DataBinder.Eval(Container.DataItem,"HasPermissionExport") %>' />
                                        </ItemTemplate>
                                        <HeaderTemplate> <div class="rgUserGroupPermissionsHeaderTemplate">
                                            <asp:CheckBox ID="cBoxHasExportAll" runat="server" Text="导出" /></div>
                                        </HeaderTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="All">
                                        <HeaderStyle Width="70" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="70" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cBoxRowAll" runat="server" />
                                            <%--  <input  type="hidden"  name="rowIndex" value=''/>--%>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            行全选
                                        </HeaderTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <CommandItemTemplate>
                                    <table class="width100-percent">
                                        <tr>
                                            <td>
                                                <%-- <asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
                                                            <input type="button" class="rgAdd" onclick="redirectToMaintenancePage(-1); return false;" />
                                                            <a href="javascript:void(0)" onclick="redirectToMaintenancePage(-1); return false;">添加</a>
                                                        </asp:Panel>--%>
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



                    <div class="mws-button-row">
                        <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                        <%--<asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="mws-button orange" CausesValidation="false" OnClick="btnDelete_Click" OnClientClick="return onConfirmDelete();" />--%>
                        <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/HRM/UserGroupManagement.aspx');return false;" />
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
            redirectToPage("Views/HRM/UserGroupManagement.aspx");
        }

        var rgUserGroupPermissions = null;
        var masterTableView = null;
        var allItems = null;
        var headers = null;
        $(document).ready(function () {
            rgUserGroupPermissions = $find("<%=rgUserGroupPermissions.ClientID%>")
            masterTableView = rgUserGroupPermissions.get_masterTableView();
            allItems = masterTableView.get_dataItems();
            headers = $(masterTableView.HeaderRow).find("input[type='checkbox']");


            $("input[name$='cBoxHasCreateAll']").click(function () { onCBoxAllClick(this, 0); });
            $("input[name$='cBoxHasEditAll']").click(function () { onCBoxAllClick(this, 1); });
            $("input[name$='cBoxHasDeleteAll']").click(function () { onCBoxAllClick(this, 2); });
            $("input[name$='cBoxHasViewAll']").click(function () { onCBoxAllClick(this, 3); });
            $("input[name$='cBoxHasPrintAll']").click(function () { onCBoxAllClick(this, 4); });
            $("input[name$='cBoxHasExportAll']").click(function () { onCBoxAllClick(this, 5); });
            $("input[name$='cBoxRowAll']").click(function () { onCBoxRowAllClick(this); });


            for (var i = 0; i < allItems.length; i++) {
                var curSelectedItem = allItems[i];
                var curSelectedItemElement = curSelectedItem.get_element();
                var cBoxs = $(curSelectedItemElement).find("input[type='checkbox']");
                $(cBoxs).each(function () {
                    if ($(this).attr("disabled") == "disabled")
                        $(this).css("display", "none");
                });
            }
        });
            function onCBoxAllClick(checkBox, col) {
                //获取已经选中的items
                var checkAll = $(checkBox).prop("checked");
                for (var i = 0; i < allItems.length; i++) {
                    var curSelectedItem = allItems[i];
                    var curSelectedItemElement = curSelectedItem.get_element();
                    var cBoxHasCreate = $(curSelectedItemElement).find("input[type='checkbox']")[col];
                    if (checkAll) {
                        if ($(cBoxHasCreate).attr("disabled") != "disabled")
                            $(cBoxHasCreate).prop("checked", true);
                    }
                    else {
                        $(cBoxHasCreate).prop("checked", false);
                    }
                }
            }
            function onCBoxRowAllClick(checkBox) {
                //获取已经选中的items
                var checkAll = $(checkBox).prop("checked");
                var index = -1;
                for (var i = 0; i < allItems.length; i++) {
                    var curSelectedItem = allItems[i];
                    var curSelectedItemElement = curSelectedItem.get_element();
                    var cBoxHasCreate = $(curSelectedItemElement).find("input[type='checkbox']")[6];
                    if (cBoxHasCreate == checkBox) {
                        index = i;
                        break;
                    }
                }

                var curSelectedItem = allItems[index];
                var curSelectedItemElement = curSelectedItem.get_element();
                var cBoxs = $(curSelectedItemElement).find("input[type='checkbox']");
                $(cBoxs).each(function () {
                    if (checkAll) {
                        if ($(this).attr("disabled") != "disabled")
                            $(this).prop("checked", true);
                    }
                    else {
                        $(this).prop("checked", false);
                    }
                });
            }

    </script>
</asp:Content>
