<%@ Page Title="选择关联单据" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="ChooseStockOutForTransportFee.aspx.cs" Inherits="ZhongDing.Web.Views.Procures.Editors.ChooseStockOutForTransportFee" %>

<%@ MasterType VirtualPath="~/Site.Window.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rgStockIns">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgStockIns" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="mws-panel grid_full" style="margin-bottom: 10px;">

        <div class="mws-panel-body">
            <div class="mws-form">
                <div class="mws-form-inline">
                    <table runat="server" id="tblSearch" class="leftmargin10">
                        <tr class="height40">
                            <th class="width100 middle-td">起止日期：</th>
                            <td class="middle-td" colspan="3">
                                <telerik:RadDatePicker runat="server" ID="rdpBeginDate" Width="120"></telerik:RadDatePicker>
                                -&nbsp;&nbsp;
                            <telerik:RadDatePicker runat="server" ID="rdpEndDate" Width="120"></telerik:RadDatePicker>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr class="height40">
                            <th class="width60 middle-td">单号：</th>
                            <td class="middle-td width35-percent">
                                <telerik:RadTextBox runat="server" ID="txtCode" Width="200px" MaxLength="100"></telerik:RadTextBox>

                            </td>

                            <td class="middle-td leftpadding20">
                                <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                            </td>

                        </tr>
                    </table>
                    <div class="mws-form-row">
                        <div class="validate-message-wrapper">
                            <asp:ValidationSummary ID="vsMaintenance" runat="server" ValidationGroup="vgMaintenance" DisplayMode="BulletList" HeaderText="请更正以下错误:" CssClass="validation-summary-errors" />
                        </div>
                    </div>

                    <div class="mws-form-row" style="padding-top: 0px; padding-left: 0px; padding-right: 1px;">
                        <telerik:RadGrid ID="rgStockOuts" runat="server" PageSize="10"
                            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false" AllowMultiRowSelection="true"
                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true" ShowFooter="true"
                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                            OnNeedDataSource="rgStockOuts_NeedDataSource">
                            <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="None"
                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                <Columns>
                                    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderText="全选">
                                        <HeaderStyle Width="40" />
                                        <ItemStyle Width="40" />
                                    </telerik:GridClientSelectColumn>
                                    <telerik:GridBoundColumn UniqueName="Code" HeaderText="编号" DataField="Code">
                                        <HeaderStyle Width="200" />
                                        <ItemStyle HorizontalAlign="Left" Width="200" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="ReceiverName" HeaderText="收货人" DataField="ReceiverName" DataFormatString="{0:yyyy/MM/dd}">
                                        <HeaderStyle Width="120" />
                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="ReceiverPhone" HeaderText="收货电话" DataField="ReceiverPhone">
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="ReceiverAddress" HeaderText="收货地址" DataField="ReceiverAddress">
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>


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
                            <ClientSettings EnableRowHoverStyle="true">
                                <Selecting AllowRowSelect="True" />
                                <Scrolling AllowScroll="true" FrozenColumnsCount="4" SaveScrollPosition="true" UseStaticHeaders="true" />
                            </ClientSettings>
                            <HeaderStyle Width="99.8%" />
                        </telerik:RadGrid>
                    </div>
                    <div class="mws-button-row">
                        <asp:Button ID="btnSave" runat="server" Text="加入关联单据" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="closeWindow(false);return false;" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnGridClientID" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">
        function closeWindow(needRebindGrid) {

            var oWin = $.getRadWindow();

            if (oWin) {

                if (needRebindGrid) {

                    var browserWindow = oWin.get_browserWindow();

                    var gridClientID = $("#<%= hdnGridClientID.ClientID%>").val();

                    if (!gridClientID.isNullOrEmpty()) {
                        var refreshGrid = browserWindow.$find(gridClientID);

                        if (refreshGrid) {
                            refreshGrid.get_masterTableView().rebind();
                        }
                    }
                }

                var isDestroyOnClose = oWin.get_destroyOnClose();
                if (isDestroyOnClose) {
                    oWin.set_destroyOnClose(false);
                }

                if (!oWin.isClosed()) {
                    oWin.close();
                }
            }
        }

        function onClientHidden(sender, args) {
            closeWindow(true);
        }

        function onError(sender, args) {
            closeWindow(false);
        }

    </script>
</asp:Content>
