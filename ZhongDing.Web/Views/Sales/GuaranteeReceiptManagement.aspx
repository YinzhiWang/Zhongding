<%@ Page Title="担保收款管理" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GuaranteeReceiptManagement.aspx.cs" Inherits="ZhongDing.Web.Views.Sales.GuaranteeReceiptManagement" %>

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
                    <telerik:AjaxUpdatedControl ControlID="rgEntities" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgEntities" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbxClientUser">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbxClientCompany" LoadingPanelID="loadingPanel" />
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
                <span class="mws-i-24 i-table-1">担保收款管理-已收款</span>
            </div>


            <table runat="server" id="tblSearch" class="leftmargin10">
                <tr class="height40">
                    <th class="width100 middle-td">收款日期：</th>
                    <td class="width300 middle-td">
                        <telerik:RadDatePicker runat="server" ID="rdpBeginDate" Width="120"></telerik:RadDatePicker>
                        -&nbsp;&nbsp;
                            <telerik:RadDatePicker runat="server" ID="rdpEndDate" Width="120"></telerik:RadDatePicker>
                    </td>
                   

                    <td class="middle-td leftpadding10">
                        <%--<telerik:RadComboBox runat="server" ID="rcbxClientCompany" Height="160px" Width="60%" Filter="Contains"
                                EmptyMessage="--请选择--" AllowCustomText="false">
                            </telerik:RadComboBox>--%>
                        <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                        &nbsp;&nbsp;
                            <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                           &nbsp;&nbsp;
                            <asp:Button ID="btnBack" runat="server" Text="返回" CssClass="mws-button green"  OnClientClick="redirectToNeedGuaranteeReceiptAppManagementPage();"/>
                    </td>
                </tr>


            </table>

            <telerik:RadGrid ID="rgEntities" runat="server" PageSize="10"
                AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                OnNeedDataSource="rgEntities_NeedDataSource" OnDeleteCommand="rgEntities_DeleteCommand"
                OnItemCreated="rgEntities_ItemCreated" OnColumnCreated="rgEntities_ColumnCreated"
                OnItemDataBound="rgEntities_ItemDataBound" OnItemCommand="rgEntities_ItemCommand">
                <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                    ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                    <Columns>
                        <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false">
                            <ItemStyle HorizontalAlign="Left" Width="50" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn UniqueName="ReceiptDate" HeaderText="收款日期" DataField="ReceiptDate" DataFormatString="{0:yyyy/MM/dd}">
                            <ItemStyle HorizontalAlign="Left" Width="100" />
                        </telerik:GridBoundColumn>

             
                          <telerik:GridTemplateColumn UniqueName="OrderCodesHtml" HeaderText="订单编号" SortExpression="OrderCodesHtml">
                                <ItemStyle HorizontalAlign="Left"  />
                                <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem,"OrderCodesHtml") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn UniqueName="ReceiptAmount" HeaderText="收款金额" DataField="ReceiptAmount" DataFormatString="￥{0:f2}">
                            <ItemStyle HorizontalAlign="Left" Width="80" />
                        </telerik:GridBoundColumn>
                         <telerik:GridTemplateColumn UniqueName="View" HeaderStyle-Width="40">
                                <ItemStyle HorizontalAlign="Center" Width="40" />
                                <ItemTemplate>
                                    <a href="javascript:void(0);" onclick="openGuaranteeLogManagementWindow(<%#DataBinder.Eval(Container.DataItem,"ID")%>)">
                                        <u>详情</u></a>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
             
                    </Columns>
                    <CommandItemTemplate>
                        <table class="width100-percent">
                            <tr>
                                <td>
                                    <strong></strong>
                                    <%-- <asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
                                            <input type="button" class="rgAdd" onclick="openSaleOrderTypeWin(); return false;" />
                                            <a href="javascript:void(0)" onclick="openSaleOrderTypeWin(); return false;">添加</a>
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
                    <ClientEvents OnGridCreated="GetsGridObject" />
                </ClientSettings>
            </telerik:RadGrid>



        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">
        var gridOfRefresh = null;
        var winSaleOrderType = null;
        function GetsGridObject(sender, eventArgs) {
            gridOfRefresh = sender;
        }

        function refreshGrid() {
            gridOfRefresh.get_masterTableView().rebind();
        }

        function redirectToNeedGuaranteeReceiptAppManagementPage(id) {
            $.showLoading();
            window.location.href = "NeedGuaranteeReceiptAppManagement.aspx?EntityID=" + id;
        }
        function openGuaranteeLogManagementWindow(id) {
            $.showLoading();

            var targetUrl = $.getRootPath() + "Views/Sales/Editors/GuaranteeLogManagement.aspx?EntityID=" + id;

            $.openRadWindow(targetUrl, "winContract", true, 800, 380);
        }
        $(document).ready(function () {


        });

    </script>
</asp:Content>

