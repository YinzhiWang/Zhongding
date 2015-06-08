<%@ Page Title="大包客户季度考核表" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DBClientQuarterlyAssessmentReport.aspx.cs" Inherits="ZhongDing.Web.Views.Reports.DBClientQuarterlyAssessmentReport" %>

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
                <span class="mws-i-24 i-table-1" id="lblTitle" runat="server">大包客户季度考核表</span>
            </div>
            <div class="mws-panel-body">
                <div class="mws-form">
                    <div class="mws-form-inline">
                        <div runat="server" id="divSearch">
                            <div class="mws-form-row">
                                <div class="float-left width40-percent">
                                    <label>年份季度</label>
                                    <div class="mws-form-item">
                                        <%--<telerik:RadNumericTextBox runat="server" ID="txtYear" Type="Number"
                                            MinValue="1900" MaxValue="3000" Width="80" MaxLength="4">
                                            <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                        </telerik:RadNumericTextBox>--%>
                                        <telerik:RadComboBox runat="server" ID="rcbxYear" Filter="Contains"
                                            AllowCustomText="false" EmptyMessage="--请选择--" Width="100" Height="160">
                                        </telerik:RadComboBox>
                                        <telerik:RadComboBox runat="server" ID="rcbxQuarter" Filter="Contains"
                                            AllowCustomText="false" EmptyMessage="--请选择--" Width="100">
                                            <Items>
                                                <telerik:RadComboBoxItem />
                                                <telerik:RadComboBoxItem Text="第1季度" Value="1" />
                                                <telerik:RadComboBoxItem Text="第2季度" Value="2" />
                                                <telerik:RadComboBoxItem Text="第3季度" Value="3" />
                                                <telerik:RadComboBoxItem Text="第4季度" Value="4" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                                <div class="float-left">
                                    <label class="leftpadding10">客户</label>
                                    <div class="mws-form-item">
                                        <telerik:RadComboBox runat="server" ID="rcbxClientUser" Filter="Contains"
                                            AllowCustomText="false" Height="160px" EmptyMessage="--请选择--">
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>
                            <div class="mws-form-row">
                                <div class="float-left width40-percent">&nbsp;</div>
                                <div class="float-left">
                                    <%--<div class="mws-form-item"></div>--%>
                                        <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnExport" runat="server" Text="导出" CssClass="mws-button green" OnClientClick="exportExcel();return false;" Visible="false" />
                                    
                                </div>
                            </div>
                        </div>

                        <div class="mws-form-row bottommargin20">
                            <telerik:RadGrid ID="rgEntities" runat="server" PageSize="10" AllowPaging="True"
                                AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                OnNeedDataSource="rgEntities_NeedDataSource" OnColumnCreated="rgEntities_ColumnCreated" >
                                <MasterTableView Width="100%" CommandItemDisplay="Top"
                                    ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                    <Columns>
                                        <telerik:GridBoundColumn UniqueName="ClientUserName" HeaderText="客户" DataField="ClientUserName">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="HospitalType" HeaderText="医院性质" DataField="HospitalType">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName">
                                            <HeaderStyle Width="160" />
                                            <ItemStyle HorizontalAlign="Left" Width="160" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="HospitalName" HeaderText="医院" DataField="HospitalName">
                                            <HeaderStyle Width="120" />
                                            <ItemStyle HorizontalAlign="Left" Width="120" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="PromotionExpense" HeaderText="推广费" DataField="PromotionExpense" DataFormatString="{0:C2}">
                                            <HeaderStyle Width="60" />
                                            <ItemStyle HorizontalAlign="Left" Width="60" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="QuarterTaskAssignment" HeaderText="季度任务量" DataField="QuarterTaskAssignment">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="FirstMonthSalesQty" HeaderText="" DataField="FirstMonthSalesQty">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="SecondMonthSalesQty" HeaderText="" DataField="SecondMonthSalesQty">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ThirdMonthSalesQty" HeaderText="" DataField="ThirdMonthSalesQty">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="QuarterAmount" HeaderText="季度金额" DataField="QuarterAmount" DataFormatString="{0:C2}">
                                            <HeaderStyle Width="120" />
                                            <ItemStyle HorizontalAlign="Left" Width="120" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="RewardRate" HeaderText="奖罚率" DataField="RewardRate" DataFormatString="{0:P2}">
                                            <HeaderStyle Width="60" />
                                            <ItemStyle HorizontalAlign="Left" Width="60" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="RewardAmount" HeaderText="奖罚金额" DataField="RewardAmount" DataFormatString="{0:C2}">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
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
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
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

