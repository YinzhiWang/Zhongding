﻿<%@ Page Title="采购订单报表" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProcureOrderReportManagement.aspx.cs" Inherits="ZhongDing.Web.Views.Reports.ProcureOrderReportManagement" %>

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
                    <telerik:AjaxUpdatedControl ControlID="rgProcureOrderReports" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgProcureOrderReports" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgProcureOrderReports">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgProcureOrderReports" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <!-- Main Container -->
    <div class="container">
        <div class="mws-panel grid_8">
            <div class="mws-panel-header">

                <span class="mws-i-24 i-table-1" id="lblTitle" runat="server">采购订单报表</span>
            </div>
            <div class="mws-panel-body">
                <table runat="server" id="tblSearch" class="leftmargin10">
                    <tr class="height40">
                        <th class="width100 middle-td">订单日期：</th>
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
                        <th class="width60 middle-td">供应商：</th>
                        <td class="middle-td width280">
                            <telerik:RadComboBox runat="server" ID="rcbxSupplier" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="rcbxSupplier_SelectedIndexChanged"
                                Width="260" EmptyMessage="--请选择--">
                            </telerik:RadComboBox>
                        </td>
                        <th class="width60 middle-td">货品：</th>
                        <td class="middle-td width280-percent">
                            <telerik:RadComboBox runat="server" ID="rcbxProduct" Filter="Contains"
                                AllowCustomText="false" Width="260" EmptyMessage="--请选择--">
                            </telerik:RadComboBox>
                        </td>
                        <td class="middle-td leftpadding20">
                            <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                              <asp:Button ID="btnExport" runat="server" Text="导出" CssClass="mws-button green" OnClientClick="exportExcel();return false;" />

                        </td>

                    </tr>
                </table>
                <telerik:RadGrid ID="rgProcureOrderReports" runat="server" PageSize="10"
                     AllowCustomPaging="true"
                                            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" Height="520" ShowHeader="true" ShowFooter="true"
                    ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                    OnNeedDataSource="rgProcureOrderReports_NeedDataSource" OnDeleteCommand="rgProcureOrderReports_DeleteCommand"
                    OnItemCreated="rgProcureOrderReports_ItemCreated" OnColumnCreated="rgProcureOrderReports_ColumnCreated" OnItemDataBound="rgProcureOrderReports_ItemDataBound">
                    <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                        ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                      <ColumnGroups>
                            <telerik:GridColumnGroup Name="AlreadyInQty" HeaderText="已执行数量" HeaderStyle-Font-Size="Small"
                                HeaderStyle-HorizontalAlign="Center" />
                            <telerik:GridColumnGroup Name="StopInQty" HeaderText="中止数量" HeaderStyle-Font-Size="Small"
                                HeaderStyle-HorizontalAlign="Center" />
                            <telerik:GridColumnGroup Name="NotInQtyGroup" HeaderText="未执行数量" HeaderStyle-Font-Size="Small"
                                HeaderStyle-HorizontalAlign="Center" />

                        </ColumnGroups>
                        <Columns>
                            <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="OrderDate" HeaderText="订单日期" DataField="OrderDate" DataFormatString="{0:yyyy/MM/dd}" ReadOnly="true">
                                <ItemStyle HorizontalAlign="Left" Width="100" />
                                <HeaderStyle Width="100" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="OrderCode" HeaderText="订单号" DataField="OrderCode" ReadOnly="true">
                                <ItemStyle HorizontalAlign="Left" Width="160" />
                                <HeaderStyle Width="160" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="SupplierName" HeaderText="供应商" DataField="SupplierName" ReadOnly="true">
                                <ItemStyle HorizontalAlign="Left" Width="260" />
                                <HeaderStyle Width="260" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="WarehouseName" HeaderText="仓库" DataField="WarehouseName" ReadOnly="true">
                                <ItemStyle HorizontalAlign="Left" Width="100" />
                                <HeaderStyle Width="100" />
                            </telerik:GridBoundColumn>


                            <telerik:GridBoundColumn UniqueName="ProductCode" HeaderText="货品编号" DataField="ProductCode" ReadOnly="true">
                                <ItemStyle HorizontalAlign="Left" Width="100" />
                                <HeaderStyle Width="100" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="CategoryName" HeaderText="货品类别" DataField="CategoryName" ReadOnly="true">
                                <ItemStyle HorizontalAlign="Left" Width="100" />
                                <HeaderStyle Width="100" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName" ReadOnly="true">
                                <ItemStyle HorizontalAlign="Left" Width="200" />
                                <HeaderStyle Width="200" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification" ReadOnly="true">
                                <ItemStyle HorizontalAlign="Left" Width="100" />
                                <HeaderStyle Width="100" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="UnitName" HeaderText="基本单位" DataField="UnitName" ReadOnly="true">
                                <ItemStyle HorizontalAlign="Left" Width="60" />
                                <HeaderStyle Width="60" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="ProcurePrice" HeaderText="采购单价" DataField="ProcurePrice" DataFormatString="￥{0:f2}"  ReadOnly="true">
                                <ItemStyle HorizontalAlign="Left" Width="80" />
                                <HeaderStyle Width="80" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ProcureCount" HeaderText="数量" DataField="ProcureCount" ReadOnly="true">
                                <ItemStyle HorizontalAlign="Left" Width="60" />
                                <HeaderStyle Width="60" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="TotalAmount" HeaderText="金额" DataField="TotalAmount" DataFormatString="￥{0:f2}" ReadOnly="true">
                                <ItemStyle HorizontalAlign="Left" Width="100" />
                                <HeaderStyle Width="100" />
                            </telerik:GridBoundColumn>

                            <%------%>

                            <telerik:GridBoundColumn UniqueName="AlreadyInQty" HeaderText="基本数量" DataField="AlreadyInQty" ColumnGroupName="AlreadyInQty"  ReadOnly="true">
                                <ItemStyle HorizontalAlign="Left" Width="80" />
                                <HeaderStyle Width="80" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="AlreadyInNumberOfPackages" HeaderText="件数" DataField="AlreadyInNumberOfPackages" ColumnGroupName="AlreadyInQty" ReadOnly="true">
                                <ItemStyle HorizontalAlign="Left" Width="60" />
                                <HeaderStyle Width="60" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="AlreadyInQtyProcurePrice" HeaderText="金额" DataField="AlreadyInQtyProcurePrice" DataFormatString="￥{0:f2}" ColumnGroupName="AlreadyInQty" ReadOnly="true">
                                <ItemStyle HorizontalAlign="Left" Width="100" />
                                <HeaderStyle Width="100" />
                            </telerik:GridBoundColumn>
                            <%------%>

                            <telerik:GridBoundColumn UniqueName="StopInQty" HeaderText="基本数量" DataField="StopInQty" ColumnGroupName="StopInQty" ReadOnly="true">
                                <ItemStyle HorizontalAlign="Left" Width="80" />
                                <HeaderStyle Width="80" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="StopInNumberOfPackages" HeaderText="件数" DataField="StopInNumberOfPackages" ColumnGroupName="StopInQty" ReadOnly="true">
                                <ItemStyle HorizontalAlign="Left" Width="60" />
                                <HeaderStyle Width="60" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="StopInQtyProcurePrice" HeaderText="金额" DataField="StopInQtyProcurePrice" DataFormatString="￥{0:f2}" ColumnGroupName="StopInQty" ReadOnly="true">
                                <ItemStyle HorizontalAlign="Left" Width="100" />
                                <HeaderStyle Width="100" />
                            </telerik:GridBoundColumn>

                            <%------%>

                            <telerik:GridBoundColumn UniqueName="NotInQty" HeaderText="基本数量" DataField="NotInQty" ColumnGroupName="NotInQtyGroup" ReadOnly="true">
                                <ItemStyle HorizontalAlign="Left" Width="80" />
                                <HeaderStyle Width="80" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="NotInNumberOfPackages" HeaderText="件数" DataField="NotInNumberOfPackages" ColumnGroupName="NotInQtyGroup" ReadOnly="true">
                                <ItemStyle HorizontalAlign="Left" Width="60" />
                                <HeaderStyle Width="60" />
                            </telerik:GridBoundColumn>

                               <telerik:GridBoundColumn UniqueName="NotInQtyProcurePrice" HeaderText="金额" DataField="NotInQtyProcurePrice" ColumnGroupName="NotInQtyGroup" ReadOnly="true">
                                <ItemStyle HorizontalAlign="Left" Width="100" />
                                <HeaderStyle Width="100" />
                            </telerik:GridBoundColumn>
                        </Columns>
                        <CommandItemTemplate>
                            <table class="width100-percent">
                                <tr>
                                    <td>
                                        <%--<asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
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

                    <ClientSettings EnableRowHoverStyle="true">
                        <ClientEvents  />
                        <Selecting AllowRowSelect="True" />
                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                    </ClientSettings>
                       <HeaderStyle Width="100%" />
                </telerik:RadGrid>
            </div>
        </div>
    </div>
    <div style="display: none;">
        <asp:Button ID="btnExportHidden" runat="server" Text="导出" CssClass="mws-button green" OnClick="btnExport_Click" />
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
            var ProcureOrderReportType = $.getQueryString("ProcureOrderReportType");
            window.location.href = "ProcureOrderReportMaintenance.aspx?ProcureOrderReportType=" + ProcureOrderReportType + "&EntityID=" + id;
        }
        function exportExcel() {
            $("#<%=btnExportHidden.ClientID%>").click();
        }
    </script>
</asp:Content>
