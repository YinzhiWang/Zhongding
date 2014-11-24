<%@ Page Title="供应商合同维护" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="SupplierContractMaintain.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.Editors.SupplierContractMaintain" %>

<%@ MasterType VirtualPath="~/Site.Window.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbxProduct">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbxProductSpecification" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgContractFiles">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgContractFiles" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
        <%--<ClientEvents OnResponseEnd="onResponseEnd" />--%>
    </telerik:RadAjaxManager>

    <div class="mws-panel grid_full" style="margin-bottom: 10px;">
        <div class="mws-panel-body">
            <div class="mws-form">
                <div class="mws-form-inline">
                    <div class="mws-form-row">
                        <div class="validate-message-wrapper">
                            <asp:ValidationSummary ID="vsMaintenance" runat="server" ValidationGroup="vgMaintenance" DisplayMode="BulletList" HeaderText="请更正以下错误:" CssClass="validation-summary-errors" />
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <label>供应商名称</label>
                        <div class="mws-form-item small">
                            <asp:Label ID="lblSupplierName" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>合同编号</label>
                            <div class="mws-form-item">
                                <telerik:RadTextBox runat="server" ID="txtContractCode" CssClass="mws-textinput" Width="50%"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="rfvContractCode" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtContractCode"
                                    ErrorMessage="合同编号必填" Text="*" CssClass="field-validation-error">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="float-left">
                            <label>合同终止日期</label>
                            <div class="mws-form-item">
                                <telerik:RadDatePicker ID="rdpExpirationDate" runat="server"
                                    Calendar-EnableShadows="true" Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                    Calendar-FirstDayOfWeek="Monday">
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="rfvExpirationDate" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="rdpExpirationDate"
                                    ErrorMessage="合同终止日期必填" Text="*" CssClass="field-validation-error">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>货品</label>
                            <div class="mws-form-item">
                                <telerik:RadComboBox ID="rcbxProduct" runat="server" Filter="Contains" Height="160px" Width="260"
                                    AutoPostBack="true" EmptyMessage="--请选择--" OnSelectedIndexChanged="rcbxProduct_SelectedIndexChanged">
                                </telerik:RadComboBox>
                                <telerik:RadToolTip ID="rttProduct" runat="server" TargetControlID="rcbxProduct" ShowEvent="OnClick"
                                    Position="MiddleRight" RelativeTo="Element" Text="该项是必选项" AutoCloseDelay="0">
                                </telerik:RadToolTip>
                                <asp:RequiredFieldValidator ID="rfvProduct" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="rcbxProduct"
                                    ErrorMessage="请选择货品" Text="*" CssClass="field-validation-error">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="float-left" runat="server" id="divProductSpecification">
                            <label>规格</label>
                            <div class="mws-form-item">
                                <div class="float-left rightpadding5">
                                    <telerik:RadComboBox ID="rcbxProductSpecification" runat="server" Filter="Contains" Height="160px" EmptyMessage="--请选择--">
                                    </telerik:RadComboBox>
                                </div>
                                <telerik:RadToolTip ID="rttProductSpecification" runat="server" TargetControlID="rcbxProductSpecification" ShowEvent="OnClick"
                                    Position="MiddleRight" RelativeTo="Element" Text="该项是必选项" AutoCloseDelay="0">
                                </telerik:RadToolTip>
                                <asp:RequiredFieldValidator ID="rfvProductSpecification" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="rcbxProductSpecification"
                                    ErrorMessage="请选择货品的规格" Text="*" CssClass="field-validation-error">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>单价</label>
                            <div class="mws-form-item">
                                <telerik:RadNumericTextBox ID="txtUnitPrice" runat="server" CssClass="mws-textinput" NumberFormat-DecimalDigits="2" EmptyMessage="￥0.00"
                                    Width="50%" Type="Currency" MinValue="0" MaxValue="999999999" MaxLength="9" ShowSpinButtons="true">
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="rfvUnitPrice" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtUnitPrice"
                                    ErrorMessage="请输入货品的单价" Text="*" CssClass="field-validation-error">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="float-left">
                            <label>需任务量配置？</label>
                            <div class="mws-form-item">
                                <telerik:RadButton runat="server" ID="cbxIsNeedTaskAssignment"
                                    ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false" OnClientCheckedChanged="onClientCheckedChanged">
                                </telerik:RadButton>
                            </div>
                        </div>
                    </div>
                    <div class="mws-form-row hide" id="divTaskAssignments">
                        <div class="mws-panel grid_full">
                            <div class="mws-panel-header">
                                <span>任务量配置</span>
                            </div>
                            <div class="mws-panel-body">
                                <div class="mws-panel-content">
                                    <div class="mws-panel-body">
                                        <table class="mws-table" style="text-align: center">
                                            <thead>
                                                <tr>
                                                    <th>1月</th>
                                                    <th>2月</th>
                                                    <th>3月</th>
                                                    <th>4月</th>
                                                    <th>5月</th>
                                                    <th>6月</th>
                                                    <th>7月</th>
                                                    <th>8月</th>
                                                    <th>9月</th>
                                                    <th>10月</th>
                                                    <th>11月</th>
                                                    <th>12月</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr class="gradeX">
                                                    <td>
                                                        <telerik:RadNumericTextBox runat="server" ID="txtMonthTask1" CssClass="mws-textinput" Width="50"
                                                            Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="9">
                                                        </telerik:RadNumericTextBox>
                                                    </td>
                                                    <td>
                                                        <telerik:RadNumericTextBox runat="server" ID="txtMonthTask2" CssClass="mws-textinput" Width="50"
                                                            Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="9">
                                                        </telerik:RadNumericTextBox>
                                                    </td>
                                                    <td>
                                                        <telerik:RadNumericTextBox runat="server" ID="txtMonthTask3" CssClass="mws-textinput" Width="50"
                                                            Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="9">
                                                        </telerik:RadNumericTextBox>
                                                    </td>
                                                    <td>
                                                        <telerik:RadNumericTextBox runat="server" ID="txtMonthTask4" CssClass="mws-textinput" Width="50"
                                                            Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="9">
                                                        </telerik:RadNumericTextBox>
                                                    </td>
                                                    <td>
                                                        <telerik:RadNumericTextBox runat="server" ID="txtMonthTask5" CssClass="mws-textinput" Width="50"
                                                            Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="9">
                                                        </telerik:RadNumericTextBox>
                                                    </td>
                                                    <td>
                                                        <telerik:RadNumericTextBox runat="server" ID="txtMonthTask6" CssClass="mws-textinput" Width="50"
                                                            Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="9">
                                                        </telerik:RadNumericTextBox>
                                                    </td>
                                                    <td>
                                                        <telerik:RadNumericTextBox runat="server" ID="txtMonthTask7" CssClass="mws-textinput" Width="50"
                                                            Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="9">
                                                        </telerik:RadNumericTextBox>
                                                    </td>
                                                    <td>
                                                        <telerik:RadNumericTextBox runat="server" ID="txtMonthTask8" CssClass="mws-textinput" Width="50"
                                                            Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="9">
                                                        </telerik:RadNumericTextBox>
                                                    </td>
                                                    <td>
                                                        <telerik:RadNumericTextBox runat="server" ID="txtMonthTask9" CssClass="mws-textinput" Width="50"
                                                            Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="9">
                                                        </telerik:RadNumericTextBox>
                                                    </td>
                                                    <td>
                                                        <telerik:RadNumericTextBox runat="server" ID="txtMonthTask10" CssClass="mws-textinput" Width="50"
                                                            Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="9">
                                                        </telerik:RadNumericTextBox>
                                                    </td>
                                                    <td>
                                                        <telerik:RadNumericTextBox runat="server" ID="txtMonthTask11" CssClass="mws-textinput" Width="50"
                                                            Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="9">
                                                        </telerik:RadNumericTextBox>
                                                    </td>
                                                    <td>
                                                        <telerik:RadNumericTextBox runat="server" ID="txtMonthTask12" CssClass="mws-textinput" Width="50"
                                                            Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="9">
                                                        </telerik:RadNumericTextBox>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="mws-form-row" runat="server" id="divContractFiles">
                        <div class="mws-panel grid_full">
                            <div class="mws-panel-header">
                                <span>合同文件管理</span>
                            </div>
                            <div class="mws-panel-body">
                                <div class="mws-panel-content">
                                    <telerik:RadGrid ID="rgContractFiles" runat="server" PageSize="10"
                                        AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                                        MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                        ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                        OnNeedDataSource="rgContractFiles_NeedDataSource" OnDeleteCommand="rgContractFiles_DeleteCommand">
                                        <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                            ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                            <Columns>
                                                <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="FileName" HeaderText="文件名" DataField="FileName">
                                                    <ItemStyle HorizontalAlign="Left" Width="40%" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="Comment" HeaderText="备注" DataField="Comment" SortExpression="Comment">
                                                    <ItemStyle HorizontalAlign="Left" Width="35%" />
                                                    <ItemTemplate>
                                                        <span title="<%#DataBinder.Eval(Container.DataItem,"Comment")%>">
                                                            <%#DataBinder.Eval(Container.DataItem,"Comment")!=null
                                                                ?DataBinder.Eval(Container.DataItem,"Comment").ToString().CutString(12)
                                                                :string.Empty%>
                                                        </span>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="Download">
                                                    <ItemStyle HorizontalAlign="Center" Width="40" />
                                                    <ItemTemplate>
                                                        <a href="<%= this.BaseUrl %><%#DataBinder.Eval(Container.DataItem,"FilePath")%>" target="_blank">下载</a>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="Edit">
                                                    <ItemStyle HorizontalAlign="Center" Width="40" />
                                                    <ItemTemplate>
                                                        <a href="javascript:void(0);" onclick="openContractFileWindow(<%#DataBinder.Eval(Container.DataItem,"ID")%>)">
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
                                                                <input type="button" class="rgAdd" onclick="openContractFileWindow(-1); return false;" />
                                                                <a href="javascript:void(0)" onclick="openContractFileWindow(-1); return false;">添加</a>
                                                            </asp:Panel>
                                                        </td>
                                                        <td class="right-td rightpadding10">
                                                            <input type="button" class="rgRefresh" onclick="refreshGrid(gridClientIDs.gridContractFiles); return false;" />
                                                            <a href="javascript:void(0);" onclick="refreshGrid(gridClientIDs.gridContractFiles); return false;">刷新</a>
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
                        <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="closeWindow(false);return false;" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnCurrentEntityID" runat="server" />
    <asp:HiddenField ID="hdnSupplierID" runat="server" />
    <asp:HiddenField ID="hdnGridClientID" runat="server" />

    <style type="text/css">
        .mws-table tbody td, .mws-table tfoot td
        {
            padding-left: 5px;
            padding-right: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">

        var gridClientIDs = {
            gridContractFiles: "<%= rgContractFiles.ClientID %>"
        };

        function refreshGrid(gridClientID) {
            var gridObj = $find(gridClientID);

            if (gridObj)
                gridObj.get_masterTableView().rebind();
        }

        function openContractFileWindow(id) {
            $.showLoading();

            var contractID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            var targetUrl = $.getRootPath() + "Views/Basics/Editors/CertificateFileMaintain.aspx?EntityID=" + id
                + "&ContractID=" + contractID + "&GridClientID=" + gridClientIDs.gridContractFiles;

            $.openRadWindow(targetUrl, "winSupplierBankAccount", true, 800, 380);
        }

        function onClientCheckedChanged(sender, args) {

            var isChecked = args.get_checked();

            if (isChecked === true) {
                $("#divTaskAssignments").show();
            }
            else {
                $("#divTaskAssignments").hide();
            }
        }

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

        function refreshMaintenancePage(sender, args) {

            var supplierID = $("#<%= hdnSupplierID.ClientID %>").val();

            var entityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            var gridClientID = $("#<%= hdnGridClientID.ClientID%>").val();

            var targetUrl = $.getRootPath() + "Views/Basics/Editors/SupplierContractMaintain.aspx?EntityID=" + entityID
                + "&SupplierID=" + supplierID + "&GridClientID=" + gridClientID;

            redirectToPage(targetUrl);
        }

        function onClientHidden(sender, args) {
            closeWindow(true);
        }

        function onError(sender, args) {
            closeWindow(false);
        }

        $(document).ready(function () {

            var cbxIsNeedTaskAssignment = $find("<%= cbxIsNeedTaskAssignment.ClientID%>");

            if (cbxIsNeedTaskAssignment) {
                var isNeedTaskAssignment = cbxIsNeedTaskAssignment.get_checked();

                if (isNeedTaskAssignment === true) {
                    $("#divTaskAssignments").show();
                }
                else {
                    $("#divTaskAssignments").hide();
                }
            }
        });



    </script>
</asp:Content>
