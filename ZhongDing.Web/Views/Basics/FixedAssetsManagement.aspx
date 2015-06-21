<%@ Page Title="固定资产管理" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FixedAssetsManagement.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.FixedAssetsManagement" %>

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
                    <telerik:AjaxUpdatedControl ControlID="rgFixedAssetss" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgFixedAssetss" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgFixedAssetss">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgFixedAssetss" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <!-- Main Container -->
    <div class="container">
        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-table-1">固定资产管理</span>
            </div>
            <div class="mws-panel-body">


                <div class="mws-form-row" runat="server" id="divRefund">
                    <telerik:RadTabStrip ID="tabStripRefunds" runat="server" MultiPageID="multiPageRefunds" Skin="Default">
                        <Tabs>
                            <telerik:RadTab Text="固定资产登记" Value="tabRefund" PageViewID="pvRefund" Selected="true"></telerik:RadTab>
                            <telerik:RadTab Text="存放地点维护" Value="tabDeduction" PageViewID="pvDeduction"></telerik:RadTab>
                        </Tabs>
                    </telerik:RadTabStrip>
                    <telerik:RadMultiPage ID="multiPageRefunds" runat="server" CssClass="multi-page-wrapper">
                        <telerik:RadPageView ID="pvRefund" runat="server" Selected="true">
                            <table runat="server" id="tblSearch" class="leftmargin10">
                                <tr class="height40">

                                    <td class="middle-td leftpadding10 width50-percent">
                                        <telerik:RadTextBox runat="server" ID="txtName" Label="名称：" LabelWidth="60px" Width="200px" MaxLength="100"></telerik:RadTextBox>
                                    </td>
                                    <td class="middle-td leftpadding20">
                                        <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                                    </td>
                                    <td class="middle-td leftpadding20">
                                        <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                                    </td>
                                </tr>
                            </table>
                            <telerik:RadGrid ID="rgFixedAssetss" runat="server" PageSize="10"
                                AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                OnNeedDataSource="rgFixedAssetss_NeedDataSource" OnDeleteCommand="rgFixedAssetss_DeleteCommand"
                                OnItemCreated="rgFixedAssetss_ItemCreated" OnColumnCreated="rgFixedAssetss_ColumnCreated" OnItemDataBound="rgFixedAssetss_ItemDataBound">
                                <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                    ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                    <Columns>
                                        <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false">
                                            <ItemStyle HorizontalAlign="Left" Width="50" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="FixedAssetsTypeName" HeaderText="资产类别" DataField="FixedAssetsTypeName">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Code" HeaderText="资产编号" DataField="Code">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Name" HeaderText="资产名称" DataField="Name">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Unit" HeaderText="计量单位" DataField="Unit">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Quantity" HeaderText="数量" DataField="Quantity">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="StorageLocationName" HeaderText="存放地点" DataField="StorageLocationName">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="DepartmentName" HeaderText="使用部门" DataField="DepartmentName">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="UsePeople" HeaderText="使用人" DataField="UsePeople">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="UseStatusText" HeaderText="使用状态" DataField="UseStatusText">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>

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

                        </telerik:RadPageView>
                        <telerik:RadPageView ID="pvDeduction" runat="server">
                            <div style="margin: 20px;">
                                <telerik:RadGrid ID="rgStorageLocations" runat="server" PageSize="10"
                                    AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                    MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="100%" ShowHeader="true" ShowFooter="false"
                                    ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                    OnNeedDataSource="rgStorageLocations_NeedDataSource" OnItemDataBound="rgStorageLocations_ItemDataBound"
                                    OnColumnCreated="rgStorageLocations_ColumnCreated" OnInsertCommand="rgStorageLocations_InsertCommand"
                                    OnUpdateCommand="rgStorageLocations_UpdateCommand" OnDeleteCommand="rgStorageLocations_DeleteCommand">
                                    <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                        ShowHeadersWhenNoRecords="true" BackColor="#fafafa" EditMode="InPlace">
                                        <CommandItemSettings AddNewRecordText="添加" RefreshText="刷新" />
                                        <Columns>





                                            <telerik:GridTemplateColumn UniqueName="Name" HeaderText="地点" DataField="Name" SortExpression="Name">

                                                <ItemTemplate>
                                                    <span><%# Eval("Name") %></span>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div id="divGridCombox">
                                                        <telerik:RadTextBox runat="server" ID="txtName" MaxLength="500" Width="100%"></telerik:RadTextBox>
                                                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="必填" CssClass="field-validation-error"
                                                            ControlToValidate="txtName"></asp:RequiredFieldValidator>

                                                    </div>
                                                </EditItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="Comment" HeaderText="备注" DataField="Comment" SortExpression="Comment">

                                                <ItemTemplate>
                                                    <span><%# Eval("Comment") %></span>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div id="divGridCombox">
                                                        <telerik:RadTextBox runat="server" ID="txtComment" MaxLength="500" Width="100%"></telerik:RadTextBox>
                                                    </div>
                                                </EditItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridEditCommandColumn UniqueName="Edit" ButtonType="LinkButton" InsertText="保存" EditText="编辑" UpdateText="更新" CancelText="取消">
                                                <HeaderStyle Width="100" />
                                                <ItemStyle Width="100" />
                                            </telerik:GridEditCommandColumn>
                                            <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="100" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />
                                        </Columns>
                                        <NoRecordsTemplate>
                                            没有任何数据
                                        </NoRecordsTemplate>
                                        <ItemStyle Height="30" />
                                        <CommandItemStyle Height="20" />
                                        <AlternatingItemStyle BackColor="#f2f2f2" />
                                        <PagerStyle PagerTextFormat="{4} 第{0}页/共{1}页, 第{2}-{3}条 共{5}条"
                                            PageSizeControlType="RadComboBox" PageSizeLabelText="每页条数:"
                                            FirstPageToolTip="第一页" PrevPageToolTip="上一页" NextPageToolTip="下一页" LastPageToolTip="最后一页" />
                                    </MasterTableView>
                                    <ClientSettings EnableRowHoverStyle="true" />
                                </telerik:RadGrid>
                            </div>
                        </telerik:RadPageView>
                    </telerik:RadMultiPage>
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

        function redirectToMaintenancePage(id) {
            $.showLoading();
            window.location.href = "FixedAssetsMaintenance.aspx?EntityID=" + id;
        }
    </script>
</asp:Content>
