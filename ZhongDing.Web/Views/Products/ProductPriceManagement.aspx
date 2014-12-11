<%@ Page Title="货品定价管理" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductPriceManagement.aspx.cs" Inherits="ZhongDing.Web.Views.Products.ProductPriceManagement" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnSearchBasicPrice">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearchBasicPrice" />
                    <telerik:AjaxUpdatedControl ControlID="rgProductBasicPrices" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnResetBasicPrice">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearchBasicPrice" />
                    <telerik:AjaxUpdatedControl ControlID="rgProductBasicPrices" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgProductBasicPrices">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgProductBasicPrices" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="hdnBasicPricesCellValueChangedCount" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSearchHighPrice">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearchHighPrice" />
                    <telerik:AjaxUpdatedControl ControlID="rgProductHighPrices" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnResetHighPrice">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearchHighPrice" />
                    <telerik:AjaxUpdatedControl ControlID="rgProductHighPrices" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgProductHighPrices">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgProductHighPrices" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="hdnHighPricesCellValueChangedCount" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSearchDBPolicyPrice">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearchDBPolicyPrice" />
                    <telerik:AjaxUpdatedControl ControlID="rgDBPolicyPrices" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnResetDBPolicyPrice">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearchDBPolicyPrice" />
                    <telerik:AjaxUpdatedControl ControlID="rgDBPolicyPrices" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgDBPolicyPrices">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgDBPolicyPrices" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="hdnDBPolicyPricesCellValueChangedCount" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <!-- Main Container -->
    <div class="container">
        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-table-1">货品定价管理</span>
            </div>
            <div class="mws-panel-body">
                <div class="mws-panel-content">
                    <telerik:RadTabStrip ID="tabStripPrices" runat="server" MultiPageID="multiPagePrices" Skin="Default">
                        <Tabs>
                            <telerik:RadTab Text="低价" Value="tabBasicPrice" PageViewID="pvBasicPrice" Selected="true"></telerik:RadTab>
                            <telerik:RadTab Text="高价" Value="tabHighPrice" PageViewID="pvHighPrice"></telerik:RadTab>
                            <telerik:RadTab Text="大包政策价" Value="tabDBPolicyPrice" PageViewID="pvDBPolicyPrice"></telerik:RadTab>
                        </Tabs>
                    </telerik:RadTabStrip>
                    <telerik:RadMultiPage ID="multiPagePrices" runat="server" CssClass="multi-page-wrapper">
                        <telerik:RadPageView ID="pvBasicPrice" runat="server" Selected="true">
                            <%--低价--%>
                            <table runat="server" id="tblSearchBasicPrice" class="leftmargin10">
                                <tr class="height40">
                                    <td class="middle-td leftpadding10">
                                        <telerik:RadTextBox runat="server" ID="txtBPProductName" Label="货品名称：" LabelWidth="75" Width="260" MaxLength="100"></telerik:RadTextBox>
                                    </td>
                                    <td class="middle-td leftpadding20">
                                        <asp:Button ID="btnSearchBasicPrice" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearchBasicPrice_Click" />
                                    </td>
                                    <td class="middle-td leftpadding20">
                                        <asp:Button ID="btnResetBasicPrice" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnResetBasicPrice_Click" />
                                    </td>
                                </tr>
                            </table>
                            <telerik:RadGrid ID="rgProductBasicPrices" runat="server" PageSize="10"
                                AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" Height="480" ShowHeader="true"
                                ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                OnNeedDataSource="rgProductBasicPrices_NeedDataSource" OnBatchEditCommand="rgProductBasicPrices_BatchEditCommand"
                                ClientSettings-ClientEvents-OnBatchEditCellValueChanged="onBasicPricesCellValueChanged">
                                <ValidationSettings EnableValidation="true" />
                                <MasterTableView Width="100%" DataKeyNames="ID,ProductID,ProductSpecificationID" CommandItemDisplay="Top" EditMode="Batch"
                                    ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                    <BatchEditingSettings EditType="Cell" />
                                    <CommandItemSettings ShowAddNewRecordButton="false" ShowSaveChangesButton="true"
                                        ShowCancelChangesButton="true" ShowRefreshButton="true"
                                        SaveChangesText="保存" CancelChangesText="取消" RefreshText="刷新" />
                                    <Columns>
                                        <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ProductCode" HeaderText="货品编号" DataField="ProductCode" ReadOnly="true">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName" ReadOnly="true">
                                            <HeaderStyle Width="260" />
                                            <ItemStyle HorizontalAlign="Left" Width="260" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification" ReadOnly="true">
                                            <HeaderStyle Width="120" />
                                            <ItemStyle HorizontalAlign="Left" Width="120" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="FactoryName" HeaderText="生产企业" DataField="FactoryName" ReadOnly="true">
                                            <HeaderStyle Width="160" />
                                            <ItemStyle HorizontalAlign="Left" Width="160" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn UniqueName="ProcurePrice" HeaderText="采购单价" DataField="ProcurePrice"
                                            SortExpression="ProcurePrice">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblProcurePrice" Text='<%# Eval("ProcurePrice", "{0:C2}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <span>
                                                    <telerik:RadNumericTextBox Width="90px" runat="server" ID="txtProcurePrice" Type="Currency" MinValue="0" MaxValue="999999999" MaxLength="9" ShowSpinButtons="true">
                                                    </telerik:RadNumericTextBox>
                                                    <span style="color: Red">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" EnableClientScript="true"
                                                            ControlToValidate="txtProcurePrice" ErrorMessage="*Required" runat="server" Display="Dynamic">
                                                        </asp:RequiredFieldValidator>
                                                    </span>
                                                </span>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="SalePrice" HeaderText="销售单价" DataField="SalePrice"
                                            SortExpression="SalePrice">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblSalePrice" Text='<%# Eval("SalePrice", "{0:C2}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <span>
                                                    <telerik:RadNumericTextBox Width="90px" runat="server" ID="txtSalePrice" Type="Currency" MinValue="0" MaxValue="999999999" MaxLength="9" ShowSpinButtons="true">
                                                    </telerik:RadNumericTextBox>
                                                    <span style="color: Red">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" EnableClientScript="true"
                                                            ControlToValidate="txtSalePrice" ErrorMessage="*Required" runat="server" Display="Dynamic">
                                                        </asp:RequiredFieldValidator>
                                                    </span>
                                                </span>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Comment" HeaderText="备注" DataField="Comment" SortExpression="Comment">
                                            <HeaderStyle Width="200" />
                                            <ItemStyle HorizontalAlign="Left" Width="200" />
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblComment" Text='<%# Eval("Comment")!=null ? Eval("Comment").ToString().CutString(12) :string.Empty %>'
                                                    ToolTip='<%# Eval("Comment")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadTextBox runat="server" ID="txtComment" MaxLength="200" Width="100%"></telerik:RadTextBox>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <%--<telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="40" ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />--%>
                                    </Columns>
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
                                    <Scrolling AllowScroll="true" FrozenColumnsCount="3" SaveScrollPosition="true" UseStaticHeaders="true" />
                                </ClientSettings>
                                <HeaderStyle Width="99.8%" />
                            </telerik:RadGrid>
                        </telerik:RadPageView>
                        <telerik:RadPageView ID="pvHighPrice" runat="server">
                            <%--高价--%>
                            <table runat="server" id="tblSearchHighPrice" class="leftmargin10">
                                <tr class="height40">
                                    <td class="middle-td leftpadding10">
                                        <telerik:RadTextBox runat="server" ID="txtHPProductName" Label="货品名称：" LabelWidth="75" Width="260" MaxLength="100"></telerik:RadTextBox>
                                    </td>
                                    <td class="middle-td leftpadding20">
                                        <asp:Button ID="btnSearchHighPrice" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearchHighPrice_Click" />
                                    </td>
                                    <td class="middle-td leftpadding20">
                                        <asp:Button ID="btnResetHighPrice" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnResetHighPrice_Click" />
                                    </td>
                                </tr>
                            </table>
                            <telerik:RadGrid ID="rgProductHighPrices" runat="server" PageSize="10"
                                AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" Height="480" ShowHeader="true"
                                ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                OnNeedDataSource="rgProductHighPrices_NeedDataSource" OnBatchEditCommand="rgProductHighPrices_BatchEditCommand"
                                ClientSettings-ClientEvents-OnBatchEditCellValueChanged="onHighPricesCellValueChanged">
                                <MasterTableView Width="100%" DataKeyNames="ID,ProductID,ProductSpecificationID" CommandItemDisplay="Top" EditMode="Batch"
                                    ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                    <BatchEditingSettings EditType="Cell" />
                                    <CommandItemSettings ShowAddNewRecordButton="false" ShowSaveChangesButton="true"
                                        ShowCancelChangesButton="true" ShowRefreshButton="true"
                                        SaveChangesText="保存" CancelChangesText="取消" RefreshText="刷新" />
                                    <Columns>
                                        <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ProductCode" HeaderText="货品编号" DataField="ProductCode" ReadOnly="true">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName" ReadOnly="true">
                                            <HeaderStyle Width="260" />
                                            <ItemStyle HorizontalAlign="Left" Width="260" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification" ReadOnly="true">
                                            <HeaderStyle Width="120" />
                                            <ItemStyle HorizontalAlign="Left" Width="120" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="FactoryName" HeaderText="生产企业" DataField="FactoryName" ReadOnly="true">
                                            <HeaderStyle Width="120" />
                                            <ItemStyle HorizontalAlign="Left" Width="120" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn UniqueName="HighPrice" HeaderText="高开单价" DataField="HighPrice"
                                            SortExpression="HighPrice">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblHighPrice" Text='<%# Eval("HighPrice", "{0:C2}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <span>
                                                    <telerik:RadNumericTextBox Width="90px" runat="server" ID="txtHighPrice" Type="Currency" MinValue="0" MaxValue="999999999" MaxLength="9" ShowSpinButtons="true">
                                                    </telerik:RadNumericTextBox>
                                                </span>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="ActualProcurePrice" HeaderText="实际采购单价" DataField="ActualProcurePrice"
                                            SortExpression="ActualProcurePrice">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblActualProcurePrice" Text='<%# Eval("ActualProcurePrice", "{0:C2}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <span>
                                                    <telerik:RadNumericTextBox Width="90px" runat="server" ID="txtActualProcurePrice" Type="Currency" MinValue="0" MaxValue="999999999" MaxLength="9" ShowSpinButtons="true">
                                                    </telerik:RadNumericTextBox>
                                                </span>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="ActualSalePrice" HeaderText="实际销售单价" DataField="ActualSalePrice"
                                            SortExpression="ActualSalePrice">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblActualSalePrice" Text='<%# Eval("ActualSalePrice", "{0:C2}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <span>
                                                    <telerik:RadNumericTextBox Width="90px" runat="server" ID="txtActualSalePrice" Type="Currency" MinValue="0" MaxValue="999999999" MaxLength="9" ShowSpinButtons="true">
                                                    </telerik:RadNumericTextBox>
                                                </span>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="SupplierTaxRatio" HeaderText="厂家税率" DataField="SupplierTaxRatio"
                                            SortExpression="SupplierTaxRatio">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblSupplierTaxRatio" Text='<%# Eval("SupplierTaxRatio", "{0:P2}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <span>
                                                    <telerik:RadNumericTextBox Width="90px" runat="server" ID="txtSupplierTaxRatio" Type="Percent" MinValue="0" MaxValue="100" MaxLength="9" ShowSpinButtons="true">
                                                    </telerik:RadNumericTextBox>
                                                </span>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="ClientTaxRatio" HeaderText="客户税率" DataField="ClientTaxRatio"
                                            SortExpression="ClientTaxRatio">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblClientTaxRatio" Text='<%# Eval("ClientTaxRatio", "{0:P2}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <span>
                                                    <telerik:RadNumericTextBox Width="90px" runat="server" ID="txtClientTaxRatio" Type="Percent" MinValue="0" MaxValue="100" MaxLength="9" ShowSpinButtons="true">
                                                    </telerik:RadNumericTextBox>
                                                </span>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Comment" HeaderText="备注" DataField="Comment" SortExpression="Comment">
                                            <HeaderStyle Width="200" />
                                            <ItemStyle HorizontalAlign="Left" Width="200" />
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblComment" Text='<%# Eval("Comment")!=null ? Eval("Comment").ToString().CutString(12) :string.Empty %>'
                                                    ToolTip='<%# Eval("Comment")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadTextBox runat="server" ID="txtComment" MaxLength="200" Width="100%"></telerik:RadTextBox>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
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
                                    <Scrolling AllowScroll="true" FrozenColumnsCount="3" SaveScrollPosition="true" UseStaticHeaders="true" />
                                </ClientSettings>
                                <HeaderStyle Width="99.8%" />
                            </telerik:RadGrid>
                        </telerik:RadPageView>
                        <telerik:RadPageView ID="pvDBPolicyPrice" runat="server">
                            <%--大包政策价--%>
                            <table runat="server" id="tblSearchDBPolicyPrice" class="leftmargin10">
                                <tr class="height40">
                                    <td class="middle-td leftpadding10">
                                        <telerik:RadTextBox runat="server" ID="txtDBPProductName" Label="货品名称：" LabelWidth="75" Width="260" MaxLength="100"></telerik:RadTextBox>
                                    </td>
                                    <td class="middle-td leftpadding20">
                                        <asp:Button ID="btnSearchDBPolicyPrice" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearchDBPolicyPrice_Click" />
                                    </td>
                                    <td class="middle-td leftpadding20">
                                        <asp:Button ID="btnResetDBPolicyPrice" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnResetDBPolicyPrice_Click" />
                                    </td>
                                </tr>
                            </table>
                            <telerik:RadGrid ID="rgDBPolicyPrices" runat="server" PageSize="10"
                                AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" Height="480" ShowHeader="true"
                                ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                OnNeedDataSource="rgDBPolicyPrices_NeedDataSource" OnBatchEditCommand="rgDBPolicyPrices_BatchEditCommand"
                                ClientSettings-ClientEvents-OnBatchEditCellValueChanged="onDBPolicyPricesCellValueChanged">
                                <MasterTableView Width="100%" DataKeyNames="ID,ProductID,ProductSpecificationID" CommandItemDisplay="Top" EditMode="Batch"
                                    ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                    <BatchEditingSettings EditType="Cell" />
                                    <CommandItemSettings ShowAddNewRecordButton="false" ShowSaveChangesButton="true"
                                        ShowCancelChangesButton="true" ShowRefreshButton="true"
                                        SaveChangesText="保存" CancelChangesText="取消" RefreshText="刷新" />
                                    <Columns>
                                        <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ProductCode" HeaderText="货品编号" DataField="ProductCode" ReadOnly="true">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName" ReadOnly="true">
                                            <HeaderStyle Width="260" />
                                            <ItemStyle HorizontalAlign="Left" Width="260" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification" ReadOnly="true">
                                            <HeaderStyle Width="120" />
                                            <ItemStyle HorizontalAlign="Left" Width="120" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="FactoryName" HeaderText="生产企业" DataField="FactoryName" ReadOnly="true">
                                            <HeaderStyle Width="160" />
                                            <ItemStyle HorizontalAlign="Left" Width="160" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="PackageDescription" HeaderText="包装" DataField="PackageDescription" ReadOnly="true">
                                            <HeaderStyle Width="120" />
                                            <ItemStyle HorizontalAlign="Left" Width="120" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn UniqueName="BidPrice" HeaderText="中标价" DataField="BidPrice"
                                            SortExpression="BidPrice">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblBidPrice" Text='<%# Eval("BidPrice", "{0:C2}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <span>
                                                    <telerik:RadNumericTextBox Width="90px" runat="server" ID="txtBidPrice" Type="Currency" MinValue="0" MaxValue="999999999" MaxLength="9" ShowSpinButtons="true">
                                                    </telerik:RadNumericTextBox>
                                                </span>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="FeeRatio" HeaderText="商业扣率" DataField="FeeRatio" SortExpression="FeeRatio">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblFeeRatio" Text='<%# Eval("FeeRatio", "{0:P2}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <span>
                                                    <telerik:RadNumericTextBox Width="90px" runat="server" ID="txtFeeRatio" Type="Percent" MinValue="0" MaxValue="100" MaxLength="9" ShowSpinButtons="true">
                                                    </telerik:RadNumericTextBox>
                                                </span>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="PreferredPrice" HeaderText="招商价" DataField="PreferredPrice"
                                            SortExpression="PreferredPrice">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblPreferredPrice" Text='<%# Eval("PreferredPrice", "{0:C2}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <span>
                                                    <telerik:RadNumericTextBox Width="90px" runat="server" ID="txtPreferredPrice" Type="Currency" MinValue="0" MaxValue="999999999" MaxLength="9" ShowSpinButtons="true">
                                                    </telerik:RadNumericTextBox>
                                                </span>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="PolicyPrice" HeaderText="大包政策价" DataField="PolicyPrice"
                                            SortExpression="PolicyPrice">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblPolicyPrice" Text='<%# Eval("PolicyPrice", "{0:C2}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <span>
                                                    <telerik:RadNumericTextBox Width="90px" runat="server" ID="txtPolicyPrice" Type="Currency" MinValue="0" MaxValue="999999999" MaxLength="9" ShowSpinButtons="true">
                                                    </telerik:RadNumericTextBox>
                                                </span>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Comment" HeaderText="备注" DataField="Comment" SortExpression="Comment">
                                            <HeaderStyle Width="200" />
                                            <ItemStyle HorizontalAlign="Left" Width="200" />
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblComment" Text='<%# Eval("Comment")!=null ? Eval("Comment").ToString().CutString(12) :string.Empty %>'
                                                    ToolTip='<%# Eval("Comment")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadTextBox runat="server" ID="txtComment" MaxLength="200" Width="100%"></telerik:RadTextBox>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
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
                                    <Scrolling AllowScroll="true" FrozenColumnsCount="3" SaveScrollPosition="true" UseStaticHeaders="true" />
                                </ClientSettings>
                                <HeaderStyle Width="99.8%" />
                            </telerik:RadGrid>
                        </telerik:RadPageView>
                    </telerik:RadMultiPage>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnBasicPricesCellValueChangedCount" runat="server" Value="0" />
    <asp:HiddenField ID="hdnHighPricesCellValueChangedCount" runat="server" Value="0" />
    <asp:HiddenField ID="hdnDBPolicyPricesCellValueChangedCount" runat="server" Value="0" />

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">
        function resetTabStripCss() {
            if (BrowserDetect.browser == "Explorer") {
                if (BrowserDetect.version > 8) //IE8+
                {
                    $(".RadTabStrip_Default .rtsLink").css({ "line-height": "24px" });
                }
                else if (BrowserDetect.version == 8) {
                    $(".RadTabStrip_Default .rtsLink").css({ "line-height": "25px" });
                }
            }
        }

        function onBasicPricesCellValueChanged(sender, args) {
            //debugger;

            var hdnGridCellValueChangedCount = $("#<%=hdnBasicPricesCellValueChangedCount.ClientID%>");

            var oChangedCount = parseInt(hdnGridCellValueChangedCount.val(), 0);

            if (args.get_editorValue() != args.get_cellValue())
                oChangedCount = oChangedCount + 1;
            else
                oChangedCount = oChangedCount - 1;

            hdnGridCellValueChangedCount.val(oChangedCount);
        }

        function onHighPricesCellValueChanged(sender, args) {
            //debugger;

            var hdnGridCellValueChangedCount = $("#<%=hdnHighPricesCellValueChangedCount.ClientID%>");

            var oChangedCount = parseInt(hdnGridCellValueChangedCount.val(), 0);

            if (args.get_editorValue() != args.get_cellValue())
                oChangedCount = oChangedCount + 1;
            else
                oChangedCount = oChangedCount - 1;

            hdnGridCellValueChangedCount.val(oChangedCount);
        }

        function onDBPolicyPricesCellValueChanged(sender, args) {
            //debugger;

            var hdnGridCellValueChangedCount = $("#<%=hdnDBPolicyPricesCellValueChangedCount.ClientID%>");

            var oChangedCount = parseInt(hdnGridCellValueChangedCount.val(), 0);

            if (args.get_editorValue() != args.get_cellValue())
                oChangedCount = oChangedCount + 1;
            else
                oChangedCount = oChangedCount - 1;

            hdnGridCellValueChangedCount.val(oChangedCount);
        }

        window.onbeforeunload = function (e) {
            //debugger;
            var basicPricesCellValueChangedCount = parseInt($("#<%= hdnBasicPricesCellValueChangedCount.ClientID%>").val(), 0);
            var highPricesCellValueChangedCount = parseInt($("#<%= hdnHighPricesCellValueChangedCount.ClientID%>").val(), 0);
            var dBPolicyPricesCellValueChangedCount = parseInt($("#<%= hdnDBPolicyPricesCellValueChangedCount.ClientID%>").val(), 0);

            if (basicPricesCellValueChangedCount > 0
                || highPricesCellValueChangedCount > 0
                || dBPolicyPricesCellValueChangedCount > 0) {

                e.preventDefault();

                var returnValue = "价格信息还没有保存";

                if ($telerik.isIE)
                    returnValue+=", 确定要离开此页吗？"

                window.event.returnValue = returnValue;
            }
        }

        $(document).ready(function () {

            BrowserDetect.init();

            resetTabStripCss();

        });

    </script>
</asp:Content>
