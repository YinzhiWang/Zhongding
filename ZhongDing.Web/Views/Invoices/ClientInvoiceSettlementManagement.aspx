<%@ Page Title="客户发票结算管理" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientInvoiceSettlementManagement.aspx.cs" Inherits="ZhongDing.Web.Views.Invoices.ClientInvoiceSettlementManagement" %>

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
                <span class="mws-i-24 i-table-1">供应商发票结算</span>
            </div>
            <div class="mws-panel-body">
                <div class="mws-form">
                    <div class="mws-form-inline">
                        <div runat="server" id="divSearch">
                            <div class="mws-form-row">
                                <label>结算日期</label>
                                <div class="mws-form-item">
                                    <telerik:RadDatePicker runat="server" ID="rdpBeginDate" Width="120"></telerik:RadDatePicker>
                                    -&nbsp;&nbsp;
                                    <telerik:RadDatePicker runat="server" ID="rdpEndDate" Width="120"></telerik:RadDatePicker>
                                </div>
                            </div>
                            <div class="mws-form-row">
                                <div class="float-left width60-percent">
                                    <label>商业单位</label>
                                    <div class="mws-form-item">
                                        <telerik:RadComboBox runat="server" ID="rcbxClientCompany" Filter="Contains"
                                            AllowCustomText="false" MarkFirstMatch="true" Height="160px" Width="60%" EmptyMessage="--请选择--">
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                                <div class="float-left">
                                    <label>状态</label>
                                    <div class="mws-form-item">
                                        <telerik:RadComboBox runat="server" ID="rcbxWorkflowStatus" Filter="Contains"
                                            AllowCustomText="false" EmptyMessage="--请选择--">
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>
                            <div class="mws-form-row">
                                <label></label>
                                <div class="mws-form-item">
                                    <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                                </div>
                            </div>
                        </div>

                        <div class="mws-form-row bottommargin20">
                            <telerik:RadGrid ID="rgEntities" runat="server" PageSize="10"
                                AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                OnNeedDataSource="rgEntities_NeedDataSource" OnItemCreated="rgEntities_ItemCreated"
                                OnColumnCreated="rgEntities_ColumnCreated" OnItemDataBound="rgEntities_ItemDataBound">
                                <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                    ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                    <Columns>
                                        <telerik:GridBoundColumn UniqueName="SettlementDate" HeaderText="结算日期" DataField="SettlementDate" DataFormatString="{0:yyyy/MM/dd}">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="InvoiceNumbers" HeaderText="发票号" DataField="InvoiceNumbers">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="TotalInvoiceAmount" HeaderText="发票金额" DataField="TotalInvoiceAmount" DataFormatString="{0:C2}">
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="TaxRatio" HeaderText="返点比率" DataField="TaxRatio" DataFormatString="{0:P2}">
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="TotalPayAmount" HeaderText="结算金额" DataField="TotalPayAmount" DataFormatString="{0:C2}">
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="CompanyName" HeaderText="收票单位" DataField="CompanyName">
                                            <HeaderStyle Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn UniqueName="Edit">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Center" Width="100" />
                                            <ItemTemplate>
                                                <a href="javascript:void(0);" onclick="redirectToMaintenancePage(<%#DataBinder.Eval(Container.DataItem,"ID")%>)">编辑</a>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridButtonColumn UniqueName="Delete" Text="删除" HeaderText="删除" CommandName="Delete" ButtonType="LinkButton"
                                            HeaderStyle-Width="60" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" Visible="false" />
                                    </Columns>
                                    <CommandItemTemplate>
                                        <table class="width100-percent">
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
                                                        <input type="button" class="rgAdd" onclick="redirectToMaintenancePage(-1); return false;" />
                                                        <a href="javascript:void(0)" onclick="redirectToMaintenancePage(-1); return false;">添加</a>
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
            window.location.href = "ClientInvoiceSettlementMaintenance.aspx?EntityID=" + id;
        }

    </script>
</asp:Content>

