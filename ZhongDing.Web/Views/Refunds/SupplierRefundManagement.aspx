<%@ Page Title="供应商返款管理" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SupplierRefundManagement.aspx.cs" Inherits="ZhongDing.Web.Views.Refunds.SupplierRefundManagement" %>

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
            <telerik:AjaxSetting AjaxControlID="rcbxCompany">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbxWarehouse" />
                    <telerik:AjaxUpdatedControl ControlID="rcbxProduct" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <!-- Main Container -->
    <div class="container">
        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-table-1">供应商返款管理</span>
            </div>
            <div class="mws-panel-body">
                <table runat="server" id="tblSearch" class="leftmargin10">
                    <tr class="height40">
                        <th class="width60 middle-td">账套：</th>
                        <td class="middle-td">
                            <table>
                                <tr>
                                    <td class="middle-td">
                                        <telerik:RadComboBox runat="server" ID="rcbxCompany" Filter="Contains"
                                            AllowCustomText="false" Height="160px" EmptyMessage="--请选择--" AutoPostBack="true"
                                            OnSelectedIndexChanged="rcbxCompany_SelectedIndexChanged">
                                        </telerik:RadComboBox>
                                    </td>
                                    <th class="width100 middle-td right-td rightpadding10">仓库：</th>
                                    <td class="middle-td">
                                        <telerik:RadComboBox runat="server" ID="rcbxWarehouse" Filter="Contains"
                                            AllowCustomText="false" Height="160px" EmptyMessage="--请选择--">
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td></td>
                    </tr>
                    <tr class="height40">
                        <th class="width60 middle-td">货品：</th>
                        <td class="middle-td width40-percent">
                            <telerik:RadComboBox runat="server" ID="rcbxProduct" Filter="Contains"
                                AllowCustomText="false" Height="160px" Width="100%" EmptyMessage="--请选择--">
                            </telerik:RadComboBox>
                        </td>
                        <td class="middle-td leftpadding20" colspan="2">
                            <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                        </td>
                    </tr>
                </table>
                <telerik:RadGrid ID="rgEntities" runat="server" PageSize="10"
                    AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                    MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                    ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                    OnNeedDataSource="rgEntities_NeedDataSource" OnItemDataBound="rgEntities_ItemDataBound">
                    <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                        ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                        <Columns>
                            <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false">
                                <ItemStyle HorizontalAlign="Left" Width="50" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="SupplierName" HeaderText="供应商" DataField="SupplierName">
                                <HeaderStyle Width="15%" />
                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName">
                                <HeaderStyle Width="15%" />
                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification">
                                <HeaderStyle Width="100" />
                                <ItemStyle HorizontalAlign="Left" Width="100" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="UnitName" HeaderText="单位" DataField="UnitName">
                                <HeaderStyle Width="60" />
                                <ItemStyle HorizontalAlign="Left" Width="60" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="TotalCount" HeaderText="订单数量" DataField="TotalCount">
                                <HeaderStyle Width="100" />
                                <ItemStyle HorizontalAlign="Left" Width="100" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="TotalAmount" HeaderText="金额" DataField="TotalAmount" DataFormatString="{0:C2}">
                                <HeaderStyle Width="100" />
                                <ItemStyle HorizontalAlign="Left" Width="100" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="TotalNeedRefundAmount" HeaderText="应返款" DataField="TotalNeedRefundAmount" DataFormatString="{0:C2}">
                                <HeaderStyle Width="100" />
                                <ItemStyle HorizontalAlign="Left" Width="100" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="TotalRefundedAmount" HeaderText="已返款" DataField="TotalRefundedAmount" DataFormatString="{0:C2}">
                                <HeaderStyle Width="100" />
                                <ItemStyle HorizontalAlign="Left" Width="100" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="TotalToBeRefundAmount" HeaderText="未返款" DataField="TotalToBeRefundAmount" DataFormatString="{0:C2}">
                                <HeaderStyle Width="100" />
                                <ItemStyle HorizontalAlign="Left" Width="100" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="CompanyName" HeaderText="账套" DataField="CompanyName">
                                <HeaderStyle Width="10%" />
                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="Edit">
                                <HeaderStyle Width="60" />
                                <ItemStyle HorizontalAlign="Center" Width="60" />
                                <ItemTemplate>
                                    <a href="javascript:void(0);" onclick="redirectToMaintenancePage(<%#DataBinder.Eval(Container.DataItem,"ID")%>)">编辑</a>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <CommandItemTemplate>
                            <table class="width100-percent">
                                <tr>
                                    <td>
                                        <%--<asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
                                            <input type="button" class="rgAdd" onclick="redirectToMaintenancePage(-1); return false;" />
                                            <a href="javascript:void(0)" onclick="redirectToMaintenancePage(-1); return false;">添加</a>
                                        </asp:Panel>
                                        <asp:Panel ID="plExportCommand" runat="server" CssClass="width80 float-left">
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

        function redirectToMaintenancePage(id, companyID, supplierID, productID, productSpecificationID) {
            $.showLoading();
            window.location.href = "SupplierRefundMaintenance.aspx?EntityID=" + id + "&CompanyID=" + companyID
                + "&SupplierID=" + supplierID + "&ProductID=" + productID + "&ProductSpecificationID=" + productSpecificationID;
        }

    </script>
</asp:Content>
