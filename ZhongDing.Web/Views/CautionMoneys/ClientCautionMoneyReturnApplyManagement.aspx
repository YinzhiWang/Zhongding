<%@ Page Title="供应商保证金管理" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientCautionMoneyReturnApplyManagement.aspx.cs" Inherits="ZhongDing.Web.Views.CautionMoneys.ClientCautionMoneyReturnApplyManagement" %>

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
                    <telerik:AjaxUpdatedControl ControlID="rgClientCautionMoneys" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgClientCautionMoneys" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgClientCautionMoneys">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgClientCautionMoneys" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <!-- Main Container -->
    <div class="container">
        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-table-1" id="lblTitle" runat="server">客户保证金退回申请管理</span>
            </div>
            <div class="mws-panel-body">
                <table runat="server" id="tblSearch" class="leftmargin10">
                    <tr class="height40">
                        <th class="width100 middle-td">申请日期：</th>
                        <td class="middle-td" colspan="1">
                            <telerik:RadDatePicker runat="server" ID="rdpBeginDate" Width="120"></telerik:RadDatePicker>
                            -&nbsp;&nbsp;
                            <telerik:RadDatePicker runat="server" ID="rdpEndDate" Width="120"></telerik:RadDatePicker>
                        </td>
                        <td class="width80 middle-td">部门：</td>
                        <td class="middle-td width380">
                            <telerik:RadComboBox runat="server" ID="rcbxDepartment" Filter="Contains"
                                AllowCustomText="true" Height="160px" Width="200px" EmptyMessage="--请选择--">
                            </telerik:RadComboBox>
                        </td>

                    </tr>
                    <tr class="height40">
                        <th class="width60 middle-td">客户名称：</th>
                        <td class="middle-td width280">
                            <telerik:RadTextBox runat="server" ID="txtClientName" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>
                        </td>
                        <th class="width80 middle-td">货品名称：</th>
                        <td class="middle-td width280-percent" colspan="2">

                            <telerik:RadTextBox runat="server" ID="txtProductName" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>

                            &nbsp;&nbsp;&nbsp;&nbsp;
                             <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />

                        </td>

                    </tr>

                </table>
                <telerik:RadGrid ID="rgClientCautionMoneys" runat="server" PageSize="10"
                    AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                    MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                    ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                    OnNeedDataSource="rgClientCautionMoneys_NeedDataSource" OnDeleteCommand="rgClientCautionMoneys_DeleteCommand"
                    OnItemCreated="rgClientCautionMoneys_ItemCreated" OnColumnCreated="rgClientCautionMoneys_ColumnCreated" OnItemDataBound="rgClientCautionMoneys_ItemDataBound">
                    <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                        ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                        <Columns>
                            <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false">
                                <ItemStyle HorizontalAlign="Left" Width="50" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="DepartmentName" HeaderText="部门" DataField="DepartmentName">
                                <ItemStyle HorizontalAlign="Left" Width="160px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ClientName" HeaderText="客户名称" DataField="ClientName">
                                <ItemStyle HorizontalAlign="Left" Width="160px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName">
                                <ItemStyle HorizontalAlign="Left" Width="160px" />
                            </telerik:GridBoundColumn>


                            <telerik:GridBoundColumn UniqueName="ProductSpecification" HeaderText="规格" DataField="ProductSpecification">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="CautionMoneyTypeName" HeaderText="保证金类别" DataField="CautionMoneyTypeName">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="ApplyDate" HeaderText="申请日期" DataField="ApplyDate" DataFormatString="{0:yyyy/MM/dd}">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="EndDate" HeaderText="保证金终止日期" DataField="EndDate" DataFormatString="{0:yyyy/MM/dd}">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="Amount" HeaderText="退回金额" DataField="Amount" DataFormatString="￥{0:f2}">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="CreatedByUserName" HeaderText="操作人" DataField="CreatedByUserName">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="WorkflowStatus" HeaderText="状态" DataField="WorkflowStatus">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>




                            <telerik:GridTemplateColumn UniqueName="Edit">
                                <ItemStyle HorizontalAlign="Center" Width="60" />
                                <ItemTemplate>
                                    <a href="javascript:void(0);" onclick="redirectToClientCautionMoneyReturnApplyMaintenancePage(<%#DataBinder.Eval(Container.DataItem,"ID")%>,<%#DataBinder.Eval(Container.DataItem,"ClientCautionMoneyID")%>)">编辑</a>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="40" ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />
                        </Columns>
                        <CommandItemTemplate>
                            <table class="width100-percent">
                                <tr>
                                    <td>
                                 <%--       <asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
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

        function redirectToClientCautionMoneyReturnApplyMaintenancePage(id, clientCautionMoneyID) {
            $.showLoading();


            window.location.href = "ClientCautionMoneyReturnApplyMaintenance.aspx?EntityID=" + id + "&ClientCautionMoneyID=" + clientCautionMoneyID;
        }
    </script>
</asp:Content>
