﻿<%@ Page Title="商业客户流向配置" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientInfoProductFlowManagement.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.ClientInfoProductFlowManagement" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbxClientUser">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgEntities" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgEntities" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgEntities">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgEntities" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <!-- Main Container -->
    <div class="container">
        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-table-1">商业客户流向配置</span>
            </div>
            <div class="mws-panel-body">
                <table runat="server" id="tblSearch" class="leftmargin10 width80-percent">
                    <tr class="height40">
                        <th class="width70 middle-td">客户名称：</th>
                        <td class="middle-td leftpadding10 width20-percent">
                            <telerik:RadComboBox runat="server" ID="rcbxClientUser" Filter="Contains"
                                AllowCustomText="false" Height="160px" Width="100%" EmptyMessage="--请选择--"
                                AutoPostBack="true" OnSelectedIndexChanged="rcbxClientUser_SelectedIndexChanged">
                            </telerik:RadComboBox>
                        </td>
                        <th class="width70 middle-td leftpadding10">商业单位：</th>
                        <td class="middle-td leftpadding10 width40-percent">
                            <telerik:RadComboBox runat="server" ID="rcbxClientCompany" Height="160px" Width="100%" Filter="Contains"
                                EmptyMessage="--请选择--" AllowCustomText="false">
                            </telerik:RadComboBox>
                        </td>
                        <td class="middle-td leftpadding20 width50">
                            <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                        </td>
                        <td class="middle-td leftpadding20">
                            <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                        </td>
                    </tr>
                </table>
                <telerik:RadGrid ID="rgEntities" runat="server" PageSize="10"
                    AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                    MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                    ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                    OnNeedDataSource="rgEntities_NeedDataSource" OnDeleteCommand="rgEntities_DeleteCommand"
                    OnItemCreated="rgEntities_ItemCreated" OnColumnCreated="rgEntities_ColumnCreated" OnItemDataBound="rgEntities_ItemDataBound">
                    <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                        ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                        <Columns>
                            <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ClientName" HeaderText="客户名称" DataField="ClientName">
                                <HeaderStyle Width="70" />
                                <ItemStyle HorizontalAlign="Left" Width="70" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ClientCompany" HeaderText="商业单位" DataField="ClientCompany">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品" DataField="ProductName">
                                <HeaderStyle Width="15%" />
                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ProductSpecification" HeaderText="规格" DataField="ProductSpecification">
                                <HeaderStyle Width="80" />
                                <ItemStyle HorizontalAlign="Left" Width="80" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="FactoryName" HeaderText="生产企业" DataField="FactoryName">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="HighPrice" HeaderText="高开价" DataField="HighPrice" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="BasicPrice" HeaderText="基本销售价" DataField="BasicPrice" DataFormatString="{0:C2}">
                                <HeaderStyle Width="120" />
                                <ItemStyle HorizontalAlign="Left" Width="120" />
                            </telerik:GridBoundColumn>
                            <telerik:GridCheckBoxColumn UniqueName="UseFlowData" HeaderText="使用流向?" DataField="UseFlowData">
                                <HeaderStyle Width="60" />
                                <ItemStyle HorizontalAlign="Left" Width="60" />
                            </telerik:GridCheckBoxColumn>
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
                    <ClientSettings EnableRowHoverStyle="true">
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
            window.location.href = "ClientInfoProductFlowMaintenance.aspx?EntityID=" + id;
        }

    </script>
</asp:Content>

