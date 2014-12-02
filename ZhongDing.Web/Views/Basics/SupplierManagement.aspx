﻿<%@ Page Title="供应商管理" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SupplierManagement.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.SupplierManagement" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgSuppliers" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgSuppliers" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgSuppliers">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgSuppliers" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <!-- Main Container -->
    <div class="container">
        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-table-1">供应商管理</span>
            </div>
            <div class="mws-panel-body">
                <table runat="server" id="tblSearch" class="leftmargin10">
                    <tr class="height40">
                        <th class=" width80  middle-td">供应商编号：</th>
                        <td class="middle-td">
                            <telerik:RadTextBox runat="server" ID="txtSupplierCode" MaxLength="50"></telerik:RadTextBox>
                        </td>
                        <th class="width80 middle-td right-td leftpadding10">供应商名称：</th>
                        <td class="middle-td width35-percent">
                            <telerik:RadTextBox runat="server" ID="txtSupplierName" Width="100%" MaxLength="100"></telerik:RadTextBox>
                        </td>
                        <td class="middle-td leftpadding20">
                            <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                        </td>
                        <td class="middle-td leftpadding20">
                            <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                        </td>
                    </tr>
                </table>
                <telerik:RadGrid ID="rgSuppliers" runat="server" PageSize="10"
                    AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                    MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                    ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                    OnNeedDataSource="rgSuppliers_NeedDataSource" OnDeleteCommand="rgSuppliers_DeleteCommand"
                    OnItemCreated="rgSuppliers_ItemCreated" OnColumnCreated="rgSuppliers_ColumnCreated" OnItemDataBound="rgSuppliers_ItemDataBound">
                    <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                        ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                        <Columns>
                            <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false">
                                <ItemStyle HorizontalAlign="Left" Width="50" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="SupplierCode" HeaderText="供应商编号" DataField="SupplierCode">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="SupplierName" HeaderText="供应商名称" DataField="SupplierName">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="FactoryName" HeaderText="生产企业" DataField="FactoryName">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ContactPerson" HeaderText="联系人" DataField="ContactPerson">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="PhoneNumber" HeaderText="联系电话" DataField="PhoneNumber">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <%--<telerik:GridBoundColumn UniqueName="CreatedBy" HeaderText="创建人" DataField="CreatedBy">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="CreatedOn" HeaderText="创建时间" DataField="CreatedOn" DataFormatString="{0:yyyy/MM/dd}">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="LastModifiedBy" HeaderText="修改人" DataField="LastModifiedBy">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="LastModifiedOn" HeaderText="修改时间" DataField="LastModifiedOn" DataFormatString="{0:yyyy/MM/dd}">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="View">
                                <ItemStyle HorizontalAlign="Center" Width="30" />
                                <ItemTemplate>
                                    <a href="javascript:void(0)" onclick="redirectToMaintenancePage(<%#DataBinder.Eval(Container.DataItem,"ID")%>); return false;">
                                        <u>查看</u></a>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                            <telerik:GridTemplateColumn UniqueName="Edit" HeaderStyle-Width="40">
                                <ItemStyle HorizontalAlign="Center" Width="40" />
                                <ItemTemplate>
                                    <a href="javascript:void(0);" onclick="redirectToMaintenancePage(<%#DataBinder.Eval(Container.DataItem,"ID")%>)">
                                        <u>编辑</u></a>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--<telerik:GridTemplateColumn UniqueName="Audit">
                                <ItemStyle HorizontalAlign="Center" Width="30" />
                                <ItemTemplate>
                                    <a href="javascript:void(0)" onclick="openAuditWindow(<%#DataBinder.Eval(Container.DataItem,"ID")%>); return false;">
                                        <u>审核</u></a>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
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

        function redirectToMaintenancePage(id) {
            $.showLoading();
            window.location.href = "SupplierMaintenance.aspx?SupplierID=" + id;
        }
    </script>
</asp:Content>

