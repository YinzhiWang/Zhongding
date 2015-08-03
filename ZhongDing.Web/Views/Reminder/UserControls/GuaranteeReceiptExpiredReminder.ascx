<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GuaranteeReceiptExpiredReminder.ascx.cs" Inherits="ZhongDing.Web.Views.Reminder.UserControls.GuaranteeReceiptExpiredReminder" %>


<%--<table runat="server" id="tblSearch" class="leftmargin10">
    <tr class="height40">
        <td class="middle-td">
            <telerik:RadTextBox runat="server" ID="txtWarehouseName" Label="仓库：" LabelWidth="45" MaxLength="50"></telerik:RadTextBox>
        </td>
        <td class="middle-td leftpadding10 width70-percent">
            <telerik:RadTextBox runat="server" ID="txtProductName" Label="货品名称：" LabelWidth="15%" Width="100%" MaxLength="100"></telerik:RadTextBox>
        </td>
        <td class="middle-td leftpadding20">
            <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
        </td>
        <td class="middle-td leftpadding20">
            <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
        </td>
    </tr>
</table>--%>
<telerik:RadGrid ID="rgEntities" runat="server" PageSize="10"
    AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
    MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
    ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
    OnNeedDataSource="rgEntities_NeedDataSource">
    <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
        ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
        <Columns>
            <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false">
                <ItemStyle HorizontalAlign="Left" Width="50" />
            </telerik:GridBoundColumn>
               <telerik:GridTemplateColumn UniqueName="OrderCode" HeaderText="订单号">
                <ItemStyle HorizontalAlign="Left"  />
                <ItemTemplate>
                    <a target="_blank" href='<%# Page.ResolveUrl("~/Views/Sales/NeedGuaranteeReceiptAppManagement.aspx")%>'>
                        <%# DataBinder.Eval(Container.DataItem,"OrderCode")%></a>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridBoundColumn UniqueName="OrderCode" HeaderText="订单号" DataField="OrderCode">
                <ItemStyle HorizontalAlign="Left" />
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="GuaranteeAmount" HeaderText="担保金额" DataField="GuaranteeAmount" DataFormatString="￥{0:f2}">
                <ItemStyle HorizontalAlign="Left" />
            </telerik:GridBoundColumn>

            <telerik:GridBoundColumn UniqueName="GuaranteeExpirationDate" HeaderText="担保有效期" DataField="GuaranteeExpirationDate" DataFormatString="{0:yyyy/MM/dd}">
                <ItemStyle HorizontalAlign="Left" />
            </telerik:GridBoundColumn>

            <telerik:GridBoundColumn UniqueName="GuaranteebyFullName" HeaderText="担保业务员" DataField="GuaranteebyFullName">
                <ItemStyle HorizontalAlign="Left" />
            </telerik:GridBoundColumn>


        </Columns>
        <CommandItemTemplate>
            <table class="width100-percent">
                <tr>
                    <td>
                        <%-- <asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
                                            <input type="button" class="rgAdd" onclick="redirectToMaintenancePage(-1); return false;" />
                                            <a href="javascript:void(0)" onclick="redirectToMaintenancePage(-1); return false;">添加</a>
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
    <ClientSettings>
        <ClientEvents OnGridCreated="GetsGridObject" />
    </ClientSettings>
</telerik:RadGrid>
<asp:HiddenField ID="hfIsBindData" runat="server" Value="False" />
<script type="text/javascript">
    var gridOfRefresh = null;

    function GetsGridObject(sender, eventArgs) {
        gridOfRefresh = sender;
    }

    function refreshGrid() {
        gridOfRefresh.get_masterTableView().rebind();
    }
</script>
