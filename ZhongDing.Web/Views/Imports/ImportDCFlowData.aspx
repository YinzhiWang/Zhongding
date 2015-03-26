<%@ Page Title="配送公司流向数据导入" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportDCFlowData.aspx.cs" Inherits="ZhongDing.Web.Views.Imports.ImportDCFlowData" %>


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
                <span class="mws-i-24 i-table-1">配送公司流向数据导入</span>
            </div>
            <div class="mws-panel-body">
                <div class="mws-form">
                    <div class="mws-form-inline">
                        <div runat="server" id="divSearch">
                            <div class="mws-form-row">
                                <div class="float-left width40-percent">
                                    <label>结算年月</label>
                                    <div class="mws-form-item">
                                        <telerik:RadMonthYearPicker runat="server" ID="rmypSettlementDate" Width="120"
                                            EnableShadows="true"
                                            MonthYearNavigationSettings-CancelButtonCaption="取消"
                                            MonthYearNavigationSettings-OkButtonCaption="确定"
                                            MonthYearNavigationSettings-TodayButtonCaption="今天"
                                            MonthYearNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                            MonthYearNavigationSettings-EnableScreenBoundaryDetection="true">
                                        </telerik:RadMonthYearPicker>
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
                                <label>执行时间</label>
                                <div class="mws-form-item">
                                    <telerik:RadDatePicker runat="server" ID="rdpBeginDate" Width="120"
                                        Calendar-EnableShadows="true"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                        Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                        Calendar-FastNavigationSettings-DisableOutOfRangeMonths="true"
                                        Calendar-FirstDayOfWeek="Monday">
                                    </telerik:RadDatePicker>
                                    -&nbsp;&nbsp;
                                        <telerik:RadDatePicker runat="server" ID="rdpEndDate" Width="120"
                                            Calendar-EnableShadows="true"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                            Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                            Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                            Calendar-FastNavigationSettings-DisableOutOfRangeMonths="true"
                                            Calendar-FirstDayOfWeek="Monday">
                                        </telerik:RadDatePicker>
                                </div>
                            </div>
                            <div class="mws-form-row">
                                <label></label>
                                <div class="mws-form-item">
                                    <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" Text="返回" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Imports/DCFlowDataManagement.aspx');return false;" />
                                    &nbsp;&nbsp;
                                    <asp:HyperLink ID="hlkModelExcel" runat="server" NavigateUrl="~/Content/Templates/XXXX配送公司流向数据(XXXX年XX月).xlsx">Excel模板下载</asp:HyperLink>
                                </div>
                            </div>
                        </div>

                        <div class="mws-form-row bottommargin20">
                            <telerik:RadGrid ID="rgEntities" runat="server" PageSize="10"
                                AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                OnNeedDataSource="rgEntities_NeedDataSource" OnItemDataBound="rgEntities_ItemDataBound"
                                OnDeleteCommand="rgEntities_DeleteCommand">
                                <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                    ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                    <Columns>
                                        <telerik:GridBoundColumn UniqueName="SettlementDate" HeaderText="结算年月" DataField="SettlementDate" DataFormatString="{0:yyyy/MM}">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="DistributionCompanyName" HeaderText="配送公司" DataField="DistributionCompanyName">
                                            <HeaderStyle Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ImportBeginDate" HeaderText="导入开始时间" DataField="ImportBeginDate" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}">
                                            <HeaderStyle Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ImportEndDate" HeaderText="导入结束时间" DataField="ImportEndDate" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}">
                                            <HeaderStyle Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ImportStatus" HeaderText="状态" DataField="ImportStatus">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn UniqueName="Edit" HeaderText="编辑">
                                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                                            <ItemStyle HorizontalAlign="Center" Width="60" />
                                            <ItemTemplate>
                                                <a href="javascript:void(0);" onclick="openUploadFileWindow(<%#DataBinder.Eval(Container.DataItem,"ID")%>)">编辑</a>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridButtonColumn UniqueName="Delete" Text="删除" HeaderText="删除" CommandName="Delete" ButtonType="LinkButton"
                                            HeaderStyle-Width="60" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />
                                    </Columns>
                                    <CommandItemTemplate>
                                        <table class="width100-percent">
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="plAddCommand" runat="server" CssClass="width120 float-left">
                                                        <input type="button" class="rgAdd" onclick="openUploadFileWindow(-1); return false;" />
                                                        <a href="javascript:void(0)" onclick="openUploadFileWindow(-1); return false;">新增导入</a>
                                                    </asp:Panel>
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
            window.location.href = "DCFlowDataMaintenance.aspx?EntityID=" + id;
        }

        function openUploadFileWindow(id) {
            $.showLoading();

            var targetUrl = $.getRootPath() + "Views/Imports/Editors/UploadDCFlowDataFile.aspx?EntityID=" + id
                + "&GridClientID=" + "<%= rgEntities.ClientID %>";

            $.openRadWindow(targetUrl, "winCertificate", true, 800, 300);
        }

    </script>
</asp:Content>


