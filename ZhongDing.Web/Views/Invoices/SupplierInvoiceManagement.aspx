﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SupplierInvoiceManagement.aspx.cs" Inherits="ZhongDing.Web.Views.Invoices.SupplierInvoiceManagement" %>

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
                    <telerik:AjaxUpdatedControl ControlID="rgSupplierInvoices" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgSupplierInvoices" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgSupplierInvoices">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgSupplierInvoices" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <!-- Main Container -->
    <div class="container">
        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-table-1" id="lblTitle" runat="server">供应商发票管理</span>
            </div>
            <div class="mws-panel-body">
                <table runat="server" id="tblSearch" class="leftmargin10">
                    <tr class="height40">
                        <th class="width100 middle-td">开票日期：</th>
                        <td class="middle-td" colspan="4">
                            <telerik:RadDatePicker runat="server" ID="rdpBeginDate" Width="120"></telerik:RadDatePicker>
                            -&nbsp;&nbsp;
                            <telerik:RadDatePicker runat="server" ID="rdpEndDate" Width="120"></telerik:RadDatePicker>
                        </td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr class="height40">
                        <th class="width60 middle-td">发票号：</th>
                        <td class="middle-td width280">
                            <telerik:RadTextBox runat="server" ID="txtInvoiceNumber" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>

                        </td>
                        <th class="width60 middle-td">供应商：</th>
                        <td class="middle-td width280-percent">
                            <telerik:RadComboBox runat="server" ID="rcbxSupplier" Filter="Contains" AutoPostBack="true"
                                Width="260" EmptyMessage="--请选择--">
                            </telerik:RadComboBox>
                        </td>
                        <td class="middle-td leftpadding20"></td>

                    </tr>
                    <tr class="height40">
                        <th class="width60 middle-td">按货品合并：</th>
                        <td class="middle-td width280">

                            <telerik:RadButton runat="server" ID="cbxIsGroupByProduct" ButtonType="ToggleButton"
                                ToggleType="CheckBox" AutoPostBack="false">
                            </telerik:RadButton>
                        </td>

                        <td class="middle-td leftpadding20" colspan="3">
                            <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />

                        </td>

                    </tr>
                </table>
                <telerik:RadGrid ID="rgSupplierInvoices" runat="server" PageSize="10"
                    AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                    MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                    ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                    OnNeedDataSource="rgSupplierInvoices_NeedDataSource" OnDeleteCommand="rgSupplierInvoices_DeleteCommand"
                    OnItemCreated="rgSupplierInvoices_ItemCreated" OnColumnCreated="rgSupplierInvoices_ColumnCreated" OnItemDataBound="rgSupplierInvoices_ItemDataBound">
                    <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                        ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                        <Columns>
                            <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false">
                                <ItemStyle HorizontalAlign="Left" Width="50" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="InvoiceDate" HeaderText="开票日期" DataFormatString="{0:yyyy/MM/dd}" DataField="InvoiceDate">
                                <ItemStyle HorizontalAlign="Left" Width="50" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="SupplierName" HeaderText="开票单位" DataField="SupplierName">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="CompanyName" HeaderText="收票单位" DataField="CompanyName">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="CompanyName" HeaderText="客户" DataField="CompanyName">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="InvoiceNumber" HeaderText="发票号" DataField="InvoiceNumber">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="ProcureOrderApplicationOrderCode" HeaderText="出库单号" DataField="ProcureOrderApplicationOrderCode">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ProcureCount" HeaderText="数量" DataField="ProcureCount">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="TotalAmount" HeaderText="金额" DataField="TotalAmount" DataFormatString="￥{0:f2}">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="TaxAmount" HeaderText="发票金额" DataField="TaxAmount" DataFormatString="￥{0:f2}">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="TaxAmount" HeaderText="快递号" DataField="TaxAmount">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>

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

            window.location.href = "SupplierInvoiceMaintenance.aspx?EntityID=" + id;
        }
    </script>
</asp:Content>