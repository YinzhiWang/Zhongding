<%@ Page Title="部门维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DepartmentMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.HRM.DepartmentMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlDepartmentType">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divDeptDistrict" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgDeptMarketDivisions">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgDeptMarketDivisions" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgDeptProductEvaluations">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgDeptProductEvaluations" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="container">

        <div class="mws-panel grid_full">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">部门维护</span>
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
                            <div class="float-left width40-percent">
                                <label>部门名称</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtDepartmentName" CssClass="mws-textinput" Width="80%"></telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="rfvDepartmentName"
                                        runat="server"
                                        ErrorMessage="部门名称必填"
                                        ControlToValidate="txtDepartmentName"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvDepartmentName" runat="server" Display="Dynamic" ErrorMessage="部门名称不能重复，请重新输入"
                                        ControlToValidate="txtDepartmentName" ValidationGroup="vgMaintenance" OnServerValidate="cvDepartmentName_ServerValidate" Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>部门性质</label>
                                <div class="mws-form-item small">
                                    <telerik:RadDropDownList runat="server" ID="ddlDepartmentType" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlDepartmentType_SelectedIndexChanged" DefaultMessage="--请选择--">
                                    </telerik:RadDropDownList>
                                    <asp:RequiredFieldValidator ID="rfvProductCategory"
                                        runat="server"
                                        ErrorMessage="请选择部门性质"
                                        ControlToValidate="ddlDepartmentType"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>部门经理</label>
                                <div class="mws-form-item small">
                                    <telerik:RadComboBox runat="server" ID="rcbxDirectorUser" Filter="Contains" AllowCustomText="false"
                                        MarkFirstMatch="true" Height="160px" EmptyMessage="--请选择--">
                                    </telerik:RadComboBox>
                                    <asp:CustomValidator ID="cvDirectorUser" runat="server" ErrorMessage="该员工不存在，请重新选择"
                                        ControlToValidate="rcbxDirectorUser" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error" OnServerValidate="cvDirectorUser_ServerValidate">
                                    </asp:CustomValidator>
                                </div>
                            </div>
                            <div class="float-left" runat="server" id="divDeptDistrict" visible="false">
                                <label>负责地区</label>
                                <div class="mws-form-item small">
                                    <telerik:RadDropDownList runat="server" ID="ddlDeptDistrict" DefaultMessage="--请选择--">
                                    </telerik:RadDropDownList>
                                    <asp:CustomValidator ID="cvDeptDistrict" runat="server" ErrorMessage="请选择负责地区"
                                        ControlToValidate="ddlDeptDistrict" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                </div>
                            </div>
                        </div>

                        <div class="mws-form-row" runat="server" id="divOtherSections">
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-market-divisions">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">部门产品线设置</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <telerik:RadGrid ID="rgDeptMarketDivisions" runat="server" PageSize="10"
                                            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgDeptMarketDivisions_NeedDataSource" OnDeleteCommand="rgDeptMarketDivisions_DeleteCommand">
                                            <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                                <Columns>
                                                    <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="BusinessManager" HeaderText="业务经理" DataField="BusinessManager">
                                                        <ItemStyle HorizontalAlign="Left" Width="120" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="Markets" HeaderText="对应市场" DataField="Markets">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="Products" HeaderText="对应产品" DataField="Products">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Edit" HeaderStyle-Width="40">
                                                        <ItemStyle HorizontalAlign="Center" Width="40" />
                                                        <ItemTemplate>
                                                            <a href="javascript:void(0);" onclick="openDeptMarketDivisionWindow(<%#DataBinder.Eval(Container.DataItem,"ID")%>)">
                                                                <u>编辑</u></a>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="40" ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />
                                                </Columns>
                                                <CommandItemTemplate>
                                                    <table class="width100-percent">
                                                        <tr>
                                                            <td>
                                                                <asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
                                                                    <input type="button" class="rgAdd" onclick="openDeptMarketDivisionWindow(-1); return false;" />
                                                                    <a href="javascript:void(0)" onclick="openDeptMarketDivisionWindow(-1); return false;">添加</a>
                                                                </asp:Panel>
                                                            </td>
                                                            <td class="right-td rightpadding10">
                                                                <input type="button" class="rgRefresh" onclick="refreshGrid(gridClientIDs.gridDeptMarketDivisions); return false;" />
                                                                <a href="javascript:void(0);" onclick="refreshGrid(gridClientIDs.gridDeptMarketDivisions); return false;">刷新</a>
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
                                        </telerik:RadGrid>
                                    </div>
                                </div>
                            </div>

                            <!--部门考核产品设置 -->
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-product-evaluations">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">部门考核产品设置</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <telerik:RadGrid ID="rgDeptProductEvaluations" runat="server" PageSize="10"
                                            AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgDeptProductEvaluations_NeedDataSource" OnDeleteCommand="rgDeptProductEvaluations_DeleteCommand"
                                            ShowFooter="true">
                                            <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                                <Columns>
                                                    <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="ProductName" FooterText="合计" HeaderText="货品名称" DataField="ProductName">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="InvestigateRatio" HeaderText="开发考核/季度基础量提成百分比" DataField="InvestigateRatio" DataFormatString="{0:P2}">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="SalesRatio" HeaderText="销售考核/季度基础量提成百分比" DataField="SalesRatio" DataFormatString="{0:P2}">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="SubtotalRatio" Aggregate="Sum" HeaderText="合计/季度基础量提成百分比" DataField="SubtotalRatio" DataFormatString="{0:P2}">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Edit" HeaderStyle-Width="40">
                                                        <ItemStyle HorizontalAlign="Center" Width="40" />
                                                        <ItemTemplate>
                                                            <a href="javascript:void(0);" onclick="openDeptProductEvaluationWindow(<%#DataBinder.Eval(Container.DataItem,"ID")%>);">
                                                                <u>编辑</u></a>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="40" ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />
                                                </Columns>
                                                <CommandItemTemplate>
                                                    <table class="width100-percent">
                                                        <tr>
                                                            <td>
                                                                <asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
                                                                    <input type="button" class="rgAdd" onclick="openDeptProductEvaluationWindow(-1); return false;" />
                                                                    <a href="javascript:void(0)" onclick="openDeptProductEvaluationWindow(-1); return false;">添加</a>
                                                                </asp:Panel>
                                                            </td>
                                                            <td class="right-td rightpadding10">
                                                                <input type="button" class="rgRefresh" onclick="refreshGrid(gridClientIDs.gridDeptProductEvaluations); return false;" />
                                                                <a href="javascript:void(0);" onclick="refreshGrid(gridClientIDs.gridDeptProductEvaluations); return false;">刷新</a>
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

                                        </telerik:RadGrid>

                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="mws-button-row">
                            <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                            <asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="mws-button orange" CausesValidation="false" OnClick="btnDelete_Click" OnClientClick="return onConfirmDelete();" />
                            <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/HRM/DepartmentManagement.aspx');return false;" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnCurrentEntityID" runat="server" Value="-1" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">

        var currentEntityID = -1;

        var gridClientIDs = {
            gridDeptMarketDivisions: "<%= rgDeptMarketDivisions.ClientID %>",
            gridDeptProductEvaluations: "<%= rgDeptProductEvaluations.ClientID %>",
        };

        function refreshGrid(gridClientID) {
            var gridObj = $find(gridClientID);

            if (gridObj)
                gridObj.get_masterTableView().rebind();
        }

        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/HRM/DepartmentManagement.aspx");
        }

        function refreshMaintenancePage(sender, args) {

            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            redirectToPage("Views/HRM/DepartmentMaintenance.aspx?EntityID=" + currentEntityID);
        }

        function openDeptMarketDivisionWindow(id) {
            $.showLoading();

            var targetUrl = $.getRootPath() + "Views/HRM/Editors/DeptMarketDivisionMaintain.aspx?EntityID=" + id
                + "&OwnerEntityID=" + currentEntityID + "&GridClientID=" + gridClientIDs.gridDeptMarketDivisions;

            $.openRadWindow(targetUrl, "winDeptMarketDivision", true, 800, 620);
        }

        function openDeptProductEvaluationWindow(id) {
            $.showLoading();

            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            var targetUrl = $.getRootPath() + "Views/HRM/Editors/DeptProductEvaluationMaintain.aspx?EntityID=" + id
                + "&OwnerEntityID=" + currentEntityID + "&GridClientID=" + gridClientIDs.gridDeptProductEvaluations;

            $.openRadWindow(targetUrl, "winDeptProductEvaluation", true, 800, 400);
        }

        $(document).ready(function () {
            currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();
        });

    </script>
</asp:Content>
