<%@ Page Title="配送公司库存数据" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DCInventoryDataManagement.aspx.cs" Inherits="ZhongDing.Web.Views.Imports.DCInventoryDataManagement" %>

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
                <span class="mws-i-24 i-table-1">配送公司库存数据</span>
            </div>
            <div class="mws-panel-body">
                <div class="mws-form">
                    <div class="mws-form-inline">
                        <div runat="server" id="divSearch">
                            <div class="mws-form-row">
                                <div class="float-left width40-percent">
                                    <label>配送公司</label>
                                    <div class="mws-form-item">
                                        <telerik:RadComboBox runat="server" ID="rcbxDistributionCompany" Filter="Contains"
                                            AllowCustomText="false" Height="160px" EmptyMessage="--请选择--">
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                                <div class="float-left">
                                    <label class="leftpadding10">库存日期</label>
                                    <div class="mws-form-item">
                                        <telerik:RadMonthYearPicker runat="server" ID="rdpBeginDate" Width="120"
                                            EnableShadows="true"
                                            MonthYearNavigationSettings-CancelButtonCaption="取消"
                                            MonthYearNavigationSettings-OkButtonCaption="确定"
                                            MonthYearNavigationSettings-TodayButtonCaption="今天"
                                            MonthYearNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                            MonthYearNavigationSettings-EnableScreenBoundaryDetection="true">
                                        </telerik:RadMonthYearPicker>
                                        -&nbsp;&nbsp;
                                        <telerik:RadMonthYearPicker runat="server" ID="rdpEndDate" Width="120"
                                            EnableShadows="true"
                                            MonthYearNavigationSettings-CancelButtonCaption="取消"
                                            MonthYearNavigationSettings-OkButtonCaption="确定"
                                            MonthYearNavigationSettings-TodayButtonCaption="今天"
                                            MonthYearNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                            MonthYearNavigationSettings-EnableScreenBoundaryDetection="true">
                                        </telerik:RadMonthYearPicker>
                                    </div>
                                </div>
                            </div>
                            <div class="mws-form-row">
                                <label>货品</label>
                                <div class="mws-form-item">
                                    <telerik:RadComboBox runat="server" ID="rcbxProduct" Filter="Contains"
                                        AllowCustomText="false" Height="160px" Width="60%" EmptyMessage="--请选择--">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="mws-form-row">
                                <label></label>
                                <div class="mws-form-item">
                                    <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnImportData" runat="server" Text="导入库存数据" CssClass="mws-button green" OnClientClick="redirectToImportPage(); return false;" />
                                </div>
                            </div>
                        </div>

                        <div class="mws-form-row bottommargin20">
                            <telerik:RadGrid ID="rgEntities" runat="server" PageSize="10"
                                AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                OnNeedDataSource="rgEntities_NeedDataSource">
                                <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                    ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                    <Columns>
                                        <telerik:GridBoundColumn UniqueName="SettlementDate" HeaderText="库存日期" DataField="SettlementDate" DataFormatString="{0:yyyy/MM/dd}">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="DistributionCompanyName" HeaderText="配送公司" DataField="DistributionCompanyName">
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ProductCode" HeaderText="货品编码" DataField="ProductCode">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName">
                                            <HeaderStyle Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="BalanceQty" HeaderText="库存数量" DataField="BalanceQty">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
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

        function redirectToImportPage() {
            $.showLoading();
            window.location.href = "ImportDCInventoryData.aspx";
        }

    </script>
</asp:Content>

