<%@ Page Title="配送公司库存核对表" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DCInventoryChecklistReport.aspx.cs" Inherits="ZhongDing.Web.Views.Reports.DCInventoryChecklistReport" %>

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
                    <telerik:AjaxUpdatedControl ControlID="divSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgEntities" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divSearch" />
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
                <span class="mws-i-24 i-table-1" id="lblTitle" runat="server">配送公司库存核对表</span>
            </div>
            <div class="mws-panel-body">
                <div class="mws-form">
                    <div class="mws-form-inline">
                        <div runat="server" id="divSearch">
                            <div class="mws-form-row">
                                <div class="float-left width40-percent">
                                    <label>库存日期</label>
                                    <div class="mws-form-item">
                                        <telerik:RadDatePicker runat="server" ID="rdpSettlementDate" Width="120"
                                            EnableShadows="true"
                                            MonthYearNavigationSettings-CancelButtonCaption="取消"
                                            MonthYearNavigationSettings-OkButtonCaption="确定"
                                            MonthYearNavigationSettings-TodayButtonCaption="今天"
                                            MonthYearNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                            MonthYearNavigationSettings-EnableScreenBoundaryDetection="true">
                                        </telerik:RadDatePicker>
                                    </div>
                                </div>
                                <div class="float-left">
                                    <label class="leftpadding10">配送公司</label>
                                    <div class="mws-form-item">
                                        <telerik:RadComboBox runat="server" ID="rcbxDistributionCompany" Filter="Contains"
                                            AllowCustomText="false" Height="160px" EmptyMessage="--请选择--">
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>
                            <div class="mws-form-row">
                                <div class="float-left width40-percent">&nbsp;</div>
                                <div class="float-left">
                                    <div class="mws-form-item">
                                        <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnExport" runat="server" Text="导出" CssClass="mws-button green" OnClientClick="exportExcel();return false;" Visible="false"/>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="mws-form-row bottommargin20">
                            <telerik:RadGrid ID="rgEntities" runat="server" PageSize="10" AllowPaging="True"
                                AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                OnNeedDataSource="rgEntities_NeedDataSource" OnItemDataBound="rgEntities_ItemDataBound">
                                <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                    ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                    <Columns>
                                        <telerik:GridBoundColumn UniqueName="DistributionCompanyName" HeaderText="配送公司" DataField="DistributionCompanyName">
                                            <ItemStyle HorizontalAlign="Left" Width="30%" />
                                            <HeaderStyle Width="30%" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification">
                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                            <HeaderStyle Width="10%" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="BookBalanceQty" HeaderText="公司账面库存" DataField="BookBalanceQty">
                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            <HeaderStyle Width="15%" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="DCBalanceQty" HeaderText="配送公司库存" DataField="DCBalanceQty">
                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            <HeaderStyle Width="15%" />
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                    <CommandItemTemplate>
                                        <table class="width100-percent">
                                            <tr>
                                                <td></td>
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
            </div>
        </div>
    </div>
    <div style="display: none;">
        <asp:Button ID="btnExportHidden" runat="server" Text="导出" CssClass="mws-button green" OnClick="btnExportHidden_Click" />
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

        function exportExcel() {
            $("#<%=btnExportHidden.ClientID%>").click();
        }
    </script>
</asp:Content>
