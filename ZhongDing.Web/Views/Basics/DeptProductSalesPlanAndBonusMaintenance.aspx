<%@ Page Title="部门货品销售计划及提成策略维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DeptProductSalesPlanAndBonusMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.DeptProductSalesPlanAndBonusMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbxDepartment">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divProducts" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgInsideFloatRatios">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgInsideFloatRatios" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgOutsideFloatRatios">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgOutsideFloatRatios" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgSalesRecords">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgSalesRecords" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="container">

        <div class="mws-panel grid_full">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">部门货品销售计划及提成策略维护</span>
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
                            <div class="width30-percent float-left">
                                <label style="width: 60px;">部门</label>
                                <div class="mws-form-item small" style="margin-left: 0px;">
                                    <telerik:RadComboBox runat="server" ID="rcbxDepartment" Filter="Contains" AllowCustomText="false"
                                        MarkFirstMatch="true" Height="160px" EmptyMessage="--请选择--"
                                        AutoPostBack="true" OnSelectedIndexChanged="rcbxDepartment_SelectedIndexChanged">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="rfvDepartment" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="rcbxDepartment"
                                        ErrorMessage="请选择所属部门" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvDepartment" runat="server" ErrorMessage="该部门不存在，请重新选择"
                                        ControlToValidate="rcbxDepartment" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error" OnServerValidate="cvDepartment_ServerValidate">
                                    </asp:CustomValidator>
                                </div>
                            </div>
                            <div class="width60-percent float-left">
                                <label style="width: 60px;">货品</label>
                                <div runat="server" id="divProducts" class="mws-form-item small" style="margin-left: 0px;">
                                    <telerik:RadComboBox runat="server" ID="rcbxProduct" Filter="Contains" AllowCustomText="false"
                                        MarkFirstMatch="true" Height="160px" Width="50%" EmptyMessage="--请选择--">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="rfvProduct"
                                        runat="server"
                                        ErrorMessage="请选择货品"
                                        ControlToValidate="rcbxProduct"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvProduct" runat="server" ErrorMessage="该货品不存在，请重新选择"
                                        ControlToValidate="rcbxProduct" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error" OnServerValidate="cvProduct_ServerValidate">
                                    </asp:CustomValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="mws-panel grid_full">
                                <div class="mws-panel-header">
                                    <span>提成比例设置</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <table class="width100-percent">
                                            <tr>
                                                <td class="top-td width15-percent">
                                                    <label>基础提成比例</label>
                                                </td>
                                                <td>
                                                    <div class="width100-percent height40">
                                                        <div class="width15-percent float-left toppadding5">
                                                            <telerik:RadButton runat="server" ID="radioIsFixedOfInside" ButtonType="ToggleButton" CommandArgument="IsFixedOfInside"
                                                                ToggleType="Radio" AutoPostBack="false" GroupName="InsideSalesBonus" Text="固定比例" Value="true" OnClientCheckedChanged="onInsideRadioCheckedChanged">
                                                            </telerik:RadButton>
                                                        </div>
                                                        <div class="width60-percent float-left leftpadding10">
                                                            <telerik:RadNumericTextBox runat="server" ID="txtFixedRatioOfInside" CssClass="mws-textinput" Width="180"
                                                                Type="Percent" NumberFormat-DecimalDigits="2" ShowSpinButtons="true" EmptyMessage="0.00%" MinValue="0" MaxValue="100" DbValueFactor="100">
                                                            </telerik:RadNumericTextBox>
                                                            <asp:CustomValidator ID="cvFixedRatioOfInside" runat="server" ErrorMessage="基础提成比例：固定比例的值必填"
                                                                ControlToValidate="txtFixedRatioOfInside" ValidationGroup="vgMaintenance" Display="Dynamic"
                                                                Text="*" CssClass="field-validation-error">
                                                            </asp:CustomValidator>
                                                        </div>
                                                    </div>
                                                    <div class="height40 toppadding5">
                                                        <telerik:RadButton runat="server" ID="radioIsNotFixedOfInside" ButtonType="ToggleButton" CommandArgument="IsNotFixedOfInside"
                                                            ToggleType="Radio" AutoPostBack="false" GroupName="InsideSalesBonus" Text="浮动比例" Value="false" OnClientCheckedChanged="onInsideRadioCheckedChanged">
                                                        </telerik:RadButton>
                                                    </div>
                                                    <div id="divInsideFloatRatios" class="bottommargin20 hide">
                                                        <telerik:RadGrid ID="rgInsideFloatRatios" runat="server"
                                                            AutoGenerateColumns="false" Skin="Silk" Width="99.8%" ShowHeader="true" AllowSorting="true"
                                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                                            OnNeedDataSource="rgInsideFloatRatios_NeedDataSource" OnInsertCommand="rgInsideFloatRatios_InsertCommand"
                                                            OnUpdateCommand="rgInsideFloatRatios_UpdateCommand" OnDeleteCommand="rgInsideFloatRatios_DeleteCommand"
                                                            OnItemDataBound="rgInsideFloatRatios_ItemDataBound">
                                                            <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa" EditMode="InPlace">
                                                                <CommandItemSettings ShowAddNewRecordButton="true" ShowRefreshButton="true"
                                                                    AddNewRecordText="添加" RefreshText="刷新" />
                                                                <Columns>
                                                                    <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridTemplateColumn UniqueName="SalesPrice" HeaderText="销售单价" DataField="SalesPrice"
                                                                        SortExpression="SalesPrice">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="lblCompareOperator" Text='<%# Eval("CompareOperator") %>'></asp:Label>
                                                                            &nbsp;
                                                                            <asp:Label runat="server" ID="lblSalesPrice" Text='<%# Eval("SalesPrice", "{0:C2}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <span>
                                                                                <telerik:RadDropDownList runat="server" ID="ddlCompareOperator" Width="60">
                                                                                    <Items>
                                                                                        <telerik:DropDownListItem Text=">" Value="1" />
                                                                                        <telerik:DropDownListItem Text="=" Value="2" />
                                                                                        <telerik:DropDownListItem Text="<" Value="3" />
                                                                                    </Items>
                                                                                </telerik:RadDropDownList>
                                                                                <telerik:RadNumericTextBox runat="server" ID="txtSalesPrice" Type="Currency" DbValue='<%# Eval("SalesPrice") %>'
                                                                                    MinValue="0" MaxValue="999999999" MaxLength="9" ShowSpinButtons="true">
                                                                                </telerik:RadNumericTextBox>
                                                                                <span style="color: Red">
                                                                                    <asp:RequiredFieldValidator ID="rfvSalesPrice" ControlToValidate="txtSalesPrice"
                                                                                        ToolTip="销售单价必填" ErrorMessage="*" runat="server" Display="Dynamic">
                                                                                    </asp:RequiredFieldValidator>
                                                                                </span>
                                                                            </span>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn UniqueName="BonusRatio" HeaderText="提成比例" DataField="BonusRatio"
                                                                        SortExpression="BonusRatio">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="lblBonusRatio" Text='<%# Eval("BonusRatio", "{0:P2}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <span>
                                                                                <telerik:RadNumericTextBox runat="server" ID="txtBonusRatio" Type="Percent" DbValue='<%# Eval("BonusRatio") %>'
                                                                                    MinValue="0" MaxValue="100" DbValueFactor="100" MaxLength="9" ShowSpinButtons="true">
                                                                                </telerik:RadNumericTextBox>
                                                                                <span style="color: Red">
                                                                                    <asp:RequiredFieldValidator ID="rfvBonusRatio" ControlToValidate="txtBonusRatio"
                                                                                        ToolTip="提成比例必填" ErrorMessage="*" runat="server" Display="Dynamic">
                                                                                    </asp:RequiredFieldValidator>
                                                                                </span>
                                                                            </span>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridEditCommandColumn ButtonType="LinkButton" InsertText="保存" EditText="编辑" UpdateText="更新" CancelText="取消" HeaderStyle-Width="80">
                                                                    </telerik:GridEditCommandColumn>
                                                                    <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="40" ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />
                                                                </Columns>
                                                                <NoRecordsTemplate>
                                                                    没有任何数据
                                                                </NoRecordsTemplate>
                                                                <ItemStyle Height="30" />
                                                                <AlternatingItemStyle BackColor="#f2f2f2" />
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="top-td">
                                                    <label>超出提成比例</label>
                                                </td>
                                                <td>
                                                    <div class="width100-percent height40">
                                                        <div class="width15-percent float-left toppadding5">
                                                            <telerik:RadButton runat="server" ID="radioIsFixedOfOutside" ButtonType="ToggleButton" CommandArgument="IsFixedOfOutside"
                                                                ToggleType="Radio" AutoPostBack="false" GroupName="OutsideSalesBonus" Text="固定比例" Value="true" OnClientCheckedChanged="onOutsideRadioCheckedChanged">
                                                            </telerik:RadButton>
                                                        </div>
                                                        <div class="width60-percent float-left leftpadding10">
                                                            <telerik:RadNumericTextBox runat="server" ID="txtFixedRatioOfOutside" CssClass="mws-textinput" Width="180"
                                                                Type="Percent" NumberFormat-DecimalDigits="2" ShowSpinButtons="true" EmptyMessage="0.00%" MinValue="0" MaxValue="100" DbValueFactor="100">
                                                            </telerik:RadNumericTextBox>
                                                            <asp:CustomValidator ID="cvFixedRatioOfOutside" runat="server" ErrorMessage="超出提成比例：固定比例的值必填"
                                                                ControlToValidate="txtFixedRatioOfOutside" ValidationGroup="vgMaintenance" Display="Dynamic"
                                                                Text="*" CssClass="field-validation-error">
                                                            </asp:CustomValidator>
                                                        </div>
                                                    </div>
                                                    <div class="height40 toppadding5">
                                                        <telerik:RadButton runat="server" ID="radioIsNotFixedOfOutside" ButtonType="ToggleButton" CommandArgument="IsNotFixedOfOutside"
                                                            ToggleType="Radio" AutoPostBack="false" GroupName="OutsideSalesBonus" Text="浮动比例" Value="false" OnClientCheckedChanged="onOutsideRadioCheckedChanged">
                                                        </telerik:RadButton>
                                                    </div>
                                                    <div id="divOutsideFloatRatios" class="bottommargin20 hide">
                                                        <telerik:RadGrid ID="rgOutsideFloatRatios" runat="server"
                                                            AutoGenerateColumns="false" Skin="Silk" Width="99.8%" ShowHeader="true" AllowSorting="true"
                                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                                            OnNeedDataSource="rgOutsideFloatRatios_NeedDataSource" OnInsertCommand="rgOutsideFloatRatios_InsertCommand"
                                                            OnUpdateCommand="rgOutsideFloatRatios_UpdateCommand" OnDeleteCommand="rgOutsideFloatRatios_DeleteCommand"
                                                            OnItemDataBound="rgOutsideFloatRatios_ItemDataBound">
                                                            <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa" EditMode="InPlace">
                                                                <CommandItemSettings ShowAddNewRecordButton="true" ShowRefreshButton="true"
                                                                    AddNewRecordText="添加" RefreshText="刷新" />
                                                                <Columns>
                                                                    <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridTemplateColumn UniqueName="SalesPrice" HeaderText="销售单价" DataField="SalesPrice"
                                                                        SortExpression="SalesPrice">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="lblCompareOperator" Text='<%# Eval("CompareOperator") %>'></asp:Label>
                                                                            &nbsp;
                                                                            <asp:Label runat="server" ID="lblSalesPrice" Text='<%# Eval("SalesPrice", "{0:C2}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <span>
                                                                                <telerik:RadDropDownList runat="server" ID="ddlCompareOperator" Width="60">
                                                                                    <Items>
                                                                                        <telerik:DropDownListItem Text=">" Value="1" />
                                                                                        <telerik:DropDownListItem Text="=" Value="2" />
                                                                                        <telerik:DropDownListItem Text="<" Value="3" />
                                                                                    </Items>
                                                                                </telerik:RadDropDownList>
                                                                                <telerik:RadNumericTextBox runat="server" ID="txtSalesPrice" Type="Currency" DbValue='<%# Eval("SalesPrice") %>'
                                                                                    MinValue="0" MaxValue="999999999" MaxLength="9" ShowSpinButtons="true">
                                                                                </telerik:RadNumericTextBox>
                                                                                <span style="color: Red">
                                                                                    <asp:RequiredFieldValidator ID="rfvSalesPrice" ControlToValidate="txtSalesPrice"
                                                                                        ToolTip="销售单价必填" ErrorMessage="*" runat="server" Display="Dynamic">
                                                                                    </asp:RequiredFieldValidator>
                                                                                </span>
                                                                            </span>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn UniqueName="BonusRatio" HeaderText="提成比例" DataField="BonusRatio"
                                                                        SortExpression="BonusRatio">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="lblBonusRatio" Text='<%# Eval("BonusRatio", "{0:P2}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <span>
                                                                                <telerik:RadNumericTextBox runat="server" ID="txtBonusRatio" Type="Percent" DbValue='<%# Eval("BonusRatio") %>'
                                                                                    MinValue="0" MaxValue="100" DbValueFactor="100" MaxLength="9" ShowSpinButtons="true">
                                                                                </telerik:RadNumericTextBox>
                                                                                <span style="color: Red">
                                                                                    <asp:RequiredFieldValidator ID="rfvBonusRatio" ControlToValidate="txtBonusRatio"
                                                                                        ToolTip="提成比例必填" ErrorMessage="*" runat="server" Display="Dynamic">
                                                                                    </asp:RequiredFieldValidator>
                                                                                </span>
                                                                            </span>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridEditCommandColumn ButtonType="LinkButton" InsertText="保存" EditText="编辑" UpdateText="更新" CancelText="取消" HeaderStyle-Width="80">
                                                                    </telerik:GridEditCommandColumn>
                                                                    <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="40" ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />
                                                                </Columns>
                                                                <NoRecordsTemplate>
                                                                    没有任何数据
                                                                </NoRecordsTemplate>
                                                                <ItemStyle Height="30" />
                                                                <AlternatingItemStyle BackColor="#f2f2f2" />
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="mws-form-row" runat="server" id="divOtherSections">
                            <!--年销量设置 -->
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-sales-records">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">年销量情况</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <telerik:RadGrid ID="rgSalesRecords" runat="server" PageSize="10"
                                            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgSalesRecords_NeedDataSource">
                                            <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="刷新" />
                                                <Columns>
                                                    <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="Year" HeaderText="年份" DataField="Year">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="Task" HeaderText="任务销量" DataField="Task">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="Actual" HeaderText="实际销量" DataField="Actual">
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
                                        </telerik:RadGrid>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="mws-button-row">
                            <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                            <asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="mws-button orange" CausesValidation="false" OnClick="btnDelete_Click" OnClientClick="return onConfirmDelete();" />
                            <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Basics/DeptProductSalesPlanAndBonusManagement.aspx');return false;" />
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

        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/Basics/DeptProductSalesPlanAndBonusManagement.aspx");
        }

        function refreshMaintenancePage(sender, args) {

            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            redirectToPage("Views/Basics/DeptProductSalesPlanAndBonusMaintenance.aspx?EntityID=" + currentEntityID);
        }

        function onInsideRadioCheckedChanged(sender, args) {
            if (args.get_checked() && args.get_commandArgument() === "IsNotFixedOfInside")
                $("#divInsideFloatRatios").show();
            else
                $("#divInsideFloatRatios").hide();
        }

        function onOutsideRadioCheckedChanged(sender, args) {
            if (args.get_checked() && args.get_commandArgument() === "IsNotFixedOfOutside")
                $("#divOutsideFloatRatios").show();
            else
                $("#divOutsideFloatRatios").hide();
        }

        $(document).ready(function () {
            currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            var radioIsNotFixedOfInside = $find("<%= radioIsNotFixedOfInside.ClientID%>");

            if (radioIsNotFixedOfInside && radioIsNotFixedOfInside.get_checked() === true) {
                $("#divInsideFloatRatios").show();
            }

            var radioIsNotFixedOfOutside = $find("<%= radioIsNotFixedOfOutside.ClientID%>");

            if (radioIsNotFixedOfOutside && radioIsNotFixedOfOutside.get_checked() === true) {
                $("#divOutsideFloatRatios").show();
            }
        });

    </script>
</asp:Content>
