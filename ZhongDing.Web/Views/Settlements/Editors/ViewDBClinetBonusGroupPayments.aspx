<%@ Page Title="支付明细" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="ViewDBClinetBonusGroupPayments.aspx.cs" Inherits="ZhongDing.Web.Views.Settlements.Editors.ViewDBClinetBonusGroupPayments" %>

<%@ MasterType VirtualPath="~/Site.Window.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rgAppPayments">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgAppPayments" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="mws-panel grid_full" style="margin-bottom: 10px;">

        <div class="mws-panel-body">
            <div class="mws-form">
                <div class="mws-form-inline">

                    <div class="mws-form-row">
                        <telerik:RadGrid ID="rgAppPayments" runat="server" PageSize="5"
                            AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" Height="450" ShowHeader="true" ShowFooter="true"
                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                            OnNeedDataSource="rgAppPayments_NeedDataSource">
                            <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                <CommandItemSettings ShowAddNewRecordButton="false" RefreshText="刷新" />
                                <Columns>
                                    <telerik:GridBoundColumn UniqueName="PayDate" HeaderText="转账日期" DataField="PayDate" DataFormatString="{0:yyyy/MM/dd}"
                                        FooterText="合计：" FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right">
                                        <HeaderStyle Width="160" />
                                        <ItemStyle HorizontalAlign="Left" Width="160" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn UniqueName="FromAccount" HeaderText="转出账号" DataField="FromAccount" SortExpression="FromAccount">
                                        <ItemStyle Width="30%" />
                                        <ItemTemplate>
                                            <span><%# Eval("FromAccount") %></span>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="ToAccount" HeaderText="转入账号" DataField="ToAccount"
                                        SortExpression="ToAccount">
                                        <ItemStyle Width="30%" />
                                        <ItemTemplate>
                                            <span><%# Eval("ToAccount") %></span>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Amount" HeaderText="付款金额" DataField="Amount"
                                        FooterAggregateFormatString="{0:C2}" Aggregate="Sum" FooterStyle-Font-Bold="true" SortExpression="Amount">
                                        <HeaderStyle Width="15%" />
                                        <ItemStyle Width="15%" />
                                        <ItemTemplate>
                                            <span><%# Eval("Amount","{0:C2}") %></span>
                                        </ItemTemplate>

                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Fee" HeaderText="手续费" DataField="Fee"
                                        FooterAggregateFormatString="{0:C2}" Aggregate="Sum" FooterStyle-Font-Bold="true" SortExpression="Fee">
                                        <HeaderStyle Width="15%" />
                                        <ItemStyle Width="15%" />
                                        <ItemTemplate>
                                            <span><%# Eval("Fee","{0:C2}") %></span>
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
                            <ClientSettings EnableRowHoverStyle="true">
                                <Scrolling AllowScroll="true" SaveScrollPosition="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </div>
                    <div class="mws-form-row"></div>
                    <div class="mws-button-row">
                        <asp:Button ID="btnCancel" runat="server" Text="关闭" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="closeWindow();return false;" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">
        function closeWindow() {

            var oWin = $.getRadWindow();

            if (oWin) {

                var isDestroyOnClose = oWin.get_destroyOnClose();
                if (isDestroyOnClose) {
                    oWin.set_destroyOnClose(false);
                }

                if (!oWin.isClosed()) {
                    oWin.close();
                }
            }
        }

    </script>
</asp:Content>
