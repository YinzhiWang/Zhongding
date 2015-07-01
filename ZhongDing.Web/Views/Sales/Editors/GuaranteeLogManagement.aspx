<%@ Page Title="担保收款管理-已收款-详情" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="GuaranteeLogManagement.aspx.cs" Inherits="ZhongDing.Web.Views.Sales.Editors.GuaranteeLogManagement" %>

<%@ MasterType VirtualPath="~/Site.Window.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <!-- Main Container -->
    <div class="container">
        <div class="mws-panel grid_8">
    
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
                     
             
                        <telerik:GridBoundColumn UniqueName="CreatedBy" HeaderText="担保人" DataField="CreatedBy">
                            <ItemStyle HorizontalAlign="Left" Width="80" />
                        </telerik:GridBoundColumn>
                        
                          <telerik:GridBoundColumn UniqueName="GuaranteeAmount" HeaderText="担保金额" DataField="GuaranteeAmount" DataFormatString="￥{0:f2}">
                            <ItemStyle HorizontalAlign="Left" Width="80" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn UniqueName="GuaranteeExpirationDate" HeaderText="担保过期日期" DataField="GuaranteeExpirationDate" DataFormatString="{0:yyyy/MM/dd}">
                            <ItemStyle HorizontalAlign="Left" Width="100" />
                        </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="GuaranteeReceiptDate" HeaderText="收款日期" DataField="GuaranteeReceiptDate" DataFormatString="{0:yyyy/MM/dd}">
                            <ItemStyle HorizontalAlign="Left"  />
                        </telerik:GridBoundColumn>

                  
          
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

        function redirectToMaintenancePage(id) {
            $.showLoading();
            window.location.href = "ClientSaleAppMaintenance.aspx?EntityID=" + id;
        }
        function redirectToNeedGuaranteeReceiptAppManagementPage() {
            $.showLoading();
            window.location.href = "NeedGuaranteeReceiptAppManagement.aspx";
        }
        $(document).ready(function () {


        });

    </script>
</asp:Content>

