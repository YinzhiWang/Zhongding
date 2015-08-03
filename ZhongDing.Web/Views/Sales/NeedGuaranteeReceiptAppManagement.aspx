<%@ Page Title="担保收款管理" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NeedGuaranteeReceiptAppManagement.aspx.cs" Inherits="ZhongDing.Web.Views.Sales.NeedGuaranteeReceiptAppManagement" %>

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
                    <telerik:AjaxUpdatedControl ControlID="rgEntitiesSelected" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="btnSettle" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgEntitiesSelected">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgEntities" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="rgEntitiesSelected" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="btnSettle" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <!-- Main Container -->
    <div class="container">
        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-table-1">担保收款管理</span>
            </div>
            <div class="mws-panel-body">
                <div class="height10"></div>
                <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-contact">
                    <div class="mws-panel-header">
                        <span class="mws-i-24 i-creditcard">未收款订单</span>
                    </div>
                    <div class="mws-panel-body">
                        <div class="mws-panel-content">
                            <table runat="server" id="tblSearch" class="leftmargin10">
                                <tr class="height40">
                                    <th class="width100 middle-td">担保有效期：</th>
                                    <td class="width300 middle-td">
                                        <telerik:RadDatePicker runat="server" ID="rdpBeginDate" Width="120"></telerik:RadDatePicker>
                                        -&nbsp;&nbsp;
                            <telerik:RadDatePicker runat="server" ID="rdpEndDate" Width="120"></telerik:RadDatePicker>
                                    </td>
                                    <th class="width70 middle-td">客户名称：</th>
                                    <td class="middle-td width20-percent">
                                        <telerik:RadComboBox runat="server" ID="rcbxClientUser" Filter="Contains"
                                            AllowCustomText="false" Height="160px" Width="260px" EmptyMessage="--请选择--"
                                            AutoPostBack="true" OnSelectedIndexChanged="rcbxClientUser_SelectedIndexChanged">
                                        </telerik:RadComboBox>
                                    </td>

                                    <td class="middle-td leftpadding10">
                                        <%--<telerik:RadComboBox runat="server" ID="rcbxClientCompany" Height="160px" Width="60%" Filter="Contains"
                                EmptyMessage="--请选择--" AllowCustomText="false">
                            </telerik:RadComboBox>--%>
                                        <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                                        &nbsp;&nbsp;
                            <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnLook" runat="server" Text="查看已结算" CssClass="mws-button green"  OnClientClick="redirectToGuaranteeReceiptManagementPage();"/>
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
                                        <telerik:GridBoundColumn UniqueName="OrderCode" HeaderText="订单编号" DataField="OrderCode">
                                            <ItemStyle HorizontalAlign="Left" Width="120" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="OrderDate" HeaderText="订单日期" DataField="OrderDate" DataFormatString="{0:yyyy/MM/dd}">
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="GuaranteeAmount" HeaderText="担保金额" DataField="GuaranteeAmount" DataFormatString="￥{0:f2}">
                                            <ItemStyle HorizontalAlign="Left" Width="60" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="GuaranteeExpirationDate" HeaderText="担保有效期" DataField="GuaranteeExpirationDate" DataFormatString="{0:yyyy/MM/dd}">
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="CreatedBy" HeaderText="业务员" DataField="CreatedBy">
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ClientUserName" HeaderText="客户名称" DataField="ClientUserName">
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ClientCompanyName" HeaderText="商业单位" DataField="ClientCompanyName">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridButtonColumn Text="选择" HeaderText="选择" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton"
                                            HeaderStyle-Width="60" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center" />

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
                </div>

                <div class="height20"></div>
                <!--联系人管理 -->
                <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-contact">
                    <div class="mws-panel-header">
                        <span class="mws-i-24 i-creditcard">已选择订单</span>
                        <span style="float: right; margin-top: -28px; margin-right: 30px;">
                            <asp:Button ID="btnSettle" Visible="false" runat="server" Text="立即结算" CssClass="mws-button green" OnClientClick="openSettleWindow();" /></span>
                    </div>
                    <div class="mws-panel-body">
                        <div class="mws-panel-content">
                            <telerik:RadGrid ID="rgEntitiesSelected" runat="server" PageSize="10"
                                AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                OnNeedDataSource="rgEntitiesSelected_NeedDataSource" OnDeleteCommand="rgEntitiesSelected_DeleteCommand"
                                OnItemCreated="rgEntitiesSelected_ItemCreated" OnColumnCreated="rgEntitiesSelected_ColumnCreated"
                                OnItemDataBound="rgEntitiesSelected_ItemDataBound" OnItemCommand="rgEntitiesSelected_ItemCommand">
                                <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                    ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                    <Columns>
                                        <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false">
                                            <ItemStyle HorizontalAlign="Left" Width="50" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="OrderCode" HeaderText="订单编号" DataField="OrderCode">
                                            <ItemStyle HorizontalAlign="Left" Width="120" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="OrderDate" HeaderText="订单日期" DataField="OrderDate" DataFormatString="{0:yyyy/MM/dd}">
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="SaleOrderTypeName" HeaderText="担保金额" DataField="SaleOrderTypeName">
                                            <ItemStyle HorizontalAlign="Left" Width="60" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="SaleOrderTypeName" HeaderText="担保有效期" DataField="SaleOrderTypeName">
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="SaleOrderTypeName" HeaderText="业务员" DataField="SaleOrderTypeName">
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ClientUserName" HeaderText="客户名称" DataField="ClientUserName">
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ClientCompanyName" HeaderText="商业单位" DataField="ClientCompanyName">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridButtonColumn Text="删除" HeaderText="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton"
                                            HeaderStyle-Width="60" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center" />


                                    </Columns>
                                    <CommandItemTemplate>
                                        <table class="width100-percent">
                                            <tr>
                                                <td>

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
                </div>

            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">
        var gridOfRefresh = null;
        var winSaleOrderType = null;
        var gridClientIDs = {
            rgEntitiesSelected: "<%= rgEntitiesSelected.ClientID  %>"
        };
        function GetsGridObject(sender, eventArgs) {
            gridOfRefresh = sender;
        }

        function refreshGrid() {
            gridOfRefresh.get_masterTableView().rebind();
        }

        function redirectToMaintenancePage(id) {
            $.showLoading();
            window.location.href = "ClientSaleAppMaintenance.aspx?EntityID=" + id;
        }
        function redirectToGuaranteeReceiptManagementPage() {
            $.showLoading();
            window.location.href = "GuaranteeReceiptManagement.aspx";
        }
        function openSettleWindow() {
            

            var targetUrl = $.getRootPath() + "Views/Sales/Editors/GuaranteeReceiptSettle.aspx?GridClientID=" + gridClientIDs.rgEntitiesSelected;

            $.openRadWindow(targetUrl, "winContract", true, 800, 380);
        }
        $(document).ready(function () {


        });

    </script>
</asp:Content>
