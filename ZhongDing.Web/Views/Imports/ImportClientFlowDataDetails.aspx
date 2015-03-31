﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportClientFlowDataDetails.aspx.cs" Inherits="ZhongDing.Web.Views.Imports.ImportClientFlowDataDetails" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rgSucceedLogs">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgSucceedLogs" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgFailedLogs">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgFailedLogs" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="container">
        <div class="mws-panel grid_full">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">配送公司流向数据导入结果</span>
            </div>
            <div class="mws-panel-body">
                <div class="mws-form">
                    <div class="mws-form-inline">
                        <div class="mws-form-row">
                            <div class="validate-message-wrapper">
                                <asp:ValidationSummary ID="vsMaintenance" runat="server" ValidationGroup="vgMaintenance" DisplayMode="BulletList" HeaderText="请更正以下错误:" CssClass="validation-summary-errors" />
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width50-percent">
                                <label>文件名</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblFileName" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>文件路径</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblFilePath" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width50-percent">
                                <label>客户</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblClientUser" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>商业单位</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblClientCompany" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width50-percent">
                                <label>开始时间</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblImportBeginDate" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>结束时间</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblImportEndDate" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>导入统计：</label>
                            <div class="mws-form-item toppadding5">
                                <table>
                                    <tr>
                                        <td>总条数：</td>
                                        <td>
                                            <asp:Label ID="lblTotalCount" runat="server"></asp:Label>
                                        </td>
                                        <td class="leftpadding20">导入成功：</td>
                                        <td>
                                            <asp:Label ID="lblSucceedCount" runat="server"></asp:Label>
                                        </td>
                                        <td class="leftpadding20">导入失败：</td>
                                        <td>
                                            <asp:Label ID="lblFailedCount" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="mws-panel-content">
                                <telerik:RadTabStrip ID="tabStripPrices" runat="server" MultiPageID="multiPageImportLogs" Skin="Default">
                                    <Tabs>
                                        <telerik:RadTab Text="导入成功" Value="tabSucceed" PageViewID="pvSucceedLogs" Selected="true"></telerik:RadTab>
                                        <telerik:RadTab Text="导入失败" Value="tabFailed" PageViewID="pvFailedLogs"></telerik:RadTab>
                                    </Tabs>
                                </telerik:RadTabStrip>
                                <telerik:RadMultiPage ID="multiPageImportLogs" runat="server" CssClass="multi-page-wrapper">
                                    <telerik:RadPageView ID="pvSucceedLogs" runat="server" Selected="true">
                                        <telerik:RadGrid ID="rgSucceedLogs" runat="server" PageSize="10"
                                            AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true" ShowFooter="false"
                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgSucceedLogs_NeedDataSource">
                                            <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                                <CommandItemSettings ShowAddNewRecordButton="false" RefreshText="刷新" />
                                                <Columns>
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
                                                    <telerik:GridBoundColumn UniqueName="FactoryName" HeaderText="生产企业" DataField="FactoryName">
                                                        <HeaderStyle Width="15%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="SaleDate" HeaderText="销售日期" DataField="SaleDate" DataFormatString="{0:yyyy/MM/dd}">
                                                        <HeaderStyle Width="100" />
                                                        <ItemStyle HorizontalAlign="Left" Width="100" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="SaleQty" HeaderText="销售数量" DataField="SaleQty">
                                                        <HeaderStyle Width="100" />
                                                        <ItemStyle HorizontalAlign="Left" Width="100" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="FlowTo" HeaderText="医院" DataField="FlowTo">
                                                        <HeaderStyle Width="10%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="HospitalType" HeaderText="医院性质" DataField="HospitalType">
                                                        <HeaderStyle Width="80" />
                                                        <ItemStyle HorizontalAlign="Left" Width="80" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="MarketName" HeaderText="区域" DataField="MarketName">
                                                        <HeaderStyle Width="80" />
                                                        <ItemStyle HorizontalAlign="Left" Width="80" />
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                                <NoRecordsTemplate>
                                                    没有任何数据
                                                </NoRecordsTemplate>
                                                <ItemStyle Height="30" />
                                                <CommandItemStyle Height="30" />
                                                <AlternatingItemStyle BackColor="#f2f2f2" />
                                                <PagerStyle PagerTextFormat="{4} 第{0}页/共{1}页, 第{2}-{3}条 共{5}条"
                                                    PageSizeControlType="RadComboBox" PageSizeLabelText="每页条数:"
                                                    FirstPageToolTip="第一页" PrevPageToolTip="上一页" NextPageToolTip="下一页" LastPageToolTip="最后一页" />
                                            </MasterTableView>
                                            <ClientSettings EnableRowHoverStyle="true" />
                                        </telerik:RadGrid>
                                    </telerik:RadPageView>
                                    <telerik:RadPageView ID="pvFailedLogs" runat="server">
                                        <telerik:RadGrid ID="rgFailedLogs" runat="server" PageSize="10"
                                            AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true" ShowFooter="false"
                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgFailedLogs_NeedDataSource">
                                            <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                                <CommandItemSettings ShowAddNewRecordButton="false" RefreshText="刷新" />
                                                <Columns>
                                                    <telerik:GridBoundColumn UniqueName="ErrorRowIndex" HeaderText="行号" DataField="ErrorRowIndex">
                                                        <HeaderStyle Width="10%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="ErrorRowData" HeaderText="行数据" DataField="ErrorRowData">
                                                        <HeaderStyle Width="40%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="40%" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="ErrorMsg" HeaderText="错误信息" DataField="ErrorMsg" SortExpression="ErrorMsg">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <span title="<%# Eval("ErrorMsg") %>"><%# Eval("AbbrErrorMsg") %></span>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                                <NoRecordsTemplate>
                                                    没有任何数据
                                                </NoRecordsTemplate>
                                                <ItemStyle Height="30" />
                                                <CommandItemStyle Height="30" />
                                                <AlternatingItemStyle BackColor="#f2f2f2" />
                                                <PagerStyle PagerTextFormat="{4} 第{0}页/共{1}页, 第{2}-{3}条 共{5}条"
                                                    PageSizeControlType="RadComboBox" PageSizeLabelText="每页条数:"
                                                    FirstPageToolTip="第一页" PrevPageToolTip="上一页" NextPageToolTip="下一页" LastPageToolTip="最后一页" />
                                            </MasterTableView>
                                            <ClientSettings EnableRowHoverStyle="true" />
                                        </telerik:RadGrid>
                                    </telerik:RadPageView>
                                </telerik:RadMultiPage>
                            </div>
                        </div>
                        <div class="mws-button-row">
                            <asp:Button ID="btnCancel" runat="server" Text="返回" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Imports/ImportClientFlowData.aspx');return false;" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">

        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/Imports/ImportClientFlowData.aspx");
        }

    </script>
</asp:Content>
