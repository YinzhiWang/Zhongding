<%@ Page Title="大包客户协议维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DBContractMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.DBContractMaintenance" %>

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
                    <telerik:AjaxUpdatedControl ControlID="divInChargeUsers" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbxProduct">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divProductSpecifications" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgHospitals">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgHospitals" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="hdnCurrentEditID" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="container">

        <div class="mws-panel grid_full">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">大包客户协议维护</span>
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
                            <label>协议编号</label>
                            <div class="mws-form-item small">
                                <telerik:RadTextBox runat="server" ID="txtContractCode" CssClass="mws-textinput" Width="40%"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="rfvContractCode"
                                    runat="server"
                                    ErrorMessage="协议编号必填"
                                    ControlToValidate="txtContractCode"
                                    Display="Dynamic" CssClass="field-validation-error"
                                    ValidationGroup="vgMaintenance" Text="*">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width50-percent">
                                <label>客户名称</label>
                                <div class="mws-form-item small">
                                    <telerik:RadComboBox runat="server" ID="rcbxClientUser" Filter="Contains" AllowCustomText="false"
                                        MarkFirstMatch="true" Height="160px" EmptyMessage="--请选择--">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="rfvClientUser"
                                        runat="server"
                                        ErrorMessage="客户名称必填"
                                        ControlToValidate="rcbxClientUser"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvClientUser" runat="server" ErrorMessage="该客户不存在，请重新选择"
                                        ControlToValidate="rcbxClientUser" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error" OnServerValidate="cvClientUser_ServerValidate">
                                    </asp:CustomValidator>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>临时协议?</label>
                                <div class="mws-form-item small">
                                    <telerik:RadButton runat="server" ID="cbxIsTempContract" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false">
                                    </telerik:RadButton>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width50-percent">
                                <label>所属部门</label>
                                <div class="mws-form-item small">
                                    <telerik:RadComboBox runat="server" ID="rcbxDepartment" Filter="Contains" AllowCustomText="false"
                                        MarkFirstMatch="true" Height="160px" EmptyMessage="--请选择--" AutoPostBack="true" OnSelectedIndexChanged="rcbxDepartment_SelectedIndexChanged">
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
                            <div class="float-left">
                                <label>业务经理</label>
                                <div class="mws-form-item small" runat="server" id="divInChargeUsers">
                                    <telerik:RadDropDownList runat="server" ID="ddlInChargeUser" DefaultMessage="--请选择--">
                                    </telerik:RadDropDownList>
                                    <asp:RequiredFieldValidator ID="rfvInChargeUser"
                                        runat="server"
                                        ErrorMessage="请选择业务经理"
                                        ControlToValidate="ddlInChargeUser"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width50-percent">
                                <label>货品</label>
                                <div class="mws-form-item small">
                                    <telerik:RadComboBox runat="server" ID="rcbxProduct" Filter="Contains" AllowCustomText="false"
                                        MarkFirstMatch="true" Height="160px" Width="90%" EmptyMessage="--请选择--"
                                        AutoPostBack="true" OnSelectedIndexChanged="rcbxProduct_SelectedIndexChanged">
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
                            <div class="float-left">
                                <label>规格</label>
                                <div class="mws-form-item small" runat="server" id="divProductSpecifications">
                                    <telerik:RadDropDownList runat="server" ID="ddlProductSpecification" DefaultMessage="--请选择--">
                                    </telerik:RadDropDownList>
                                    <asp:RequiredFieldValidator ID="rfvProductSpecification"
                                        runat="server"
                                        ErrorMessage="请选择货品规格"
                                        ControlToValidate="ddlProductSpecification"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width50-percent">
                                <label>推广费</label>
                                <div class="mws-form-item small">
                                    <telerik:RadNumericTextBox runat="server" ID="txtPromotionExpense" CssClass="mws-textinput" Width="100"
                                        Type="Currency" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="9">
                                    </telerik:RadNumericTextBox>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>协议过期日</label>
                                <div class="mws-form-item small">
                                    <telerik:RadDatePicker ID="rdpContractExpDate" runat="server"
                                        Calendar-EnableShadows="true" Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定" Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                        Calendar-FirstDayOfWeek="Monday">
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="rfvContractExpDate" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="rdpContractExpDate"
                                        ErrorMessage="协议过期日必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width50-percent">
                                <label>客户性质</label>
                                <div class="mws-form-item small">
                                    <telerik:RadButton runat="server" ID="radioIsNew" ButtonType="ToggleButton" ToggleType="Radio" AutoPostBack="false" GroupName="IsNewClient" Text="新客户" Value="true"></telerik:RadButton>
                                    <telerik:RadButton runat="server" ID="radioIsNotNew" ButtonType="ToggleButton" ToggleType="Radio" AutoPostBack="false" GroupName="IsNewClient" Text="老客户" Value="false"></telerik:RadButton>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>医院性质</label>
                                <div class="mws-form-item small">
                                    <telerik:RadDropDownList runat="server" ID="ddlHospitalType" DefaultMessage="--请选择--">
                                    </telerik:RadDropDownList>
                                    <asp:RequiredFieldValidator ID="rfvHospitalType"
                                        runat="server"
                                        ErrorMessage="请选择医院性质"
                                        ControlToValidate="ddlHospitalType"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>备注</label>
                            <div class="mws-form-item medium">
                                <telerik:RadTextBox runat="server" ID="txtComment" Width="90%" MaxLength="200"
                                    TextMode="MultiLine" Height="80">
                                </telerik:RadTextBox>
                            </div>
                        </div>
                        <div class="mws-form-row">
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

                        <div class="mws-form-row" runat="server" id="divOtherSections">

                            <!--医院设置 -->
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-hospital">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">医院设置</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <telerik:RadGrid ID="rgHospitals" runat="server" PageSize="10"
                                            AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgHospitals_NeedDataSource" OnEditCommand="rgHospitals_EditCommand"
                                            OnInsertCommand="rgHospitals_InsertCommand" OnUpdateCommand="rgHospitals_UpdateCommand"
                                            OnDeleteCommand="rgHospitals_DeleteCommand">
                                            <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa" EditMode="InPlace">
                                                <CommandItemSettings AddNewRecordText="添加" RefreshText="刷新" />
                                                <Columns>
                                                    <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="HospitalName" HeaderText="医院名称" DataField="HospitalName" SortExpression="HospitalName">
                                                        <ItemTemplate>
                                                            <span><%# Eval("HospitalName") %></span>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtHospitalName" runat="server" Text='<%# Bind("HospitalName") %>' CssClass="mws-textinput"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvHospitalName" runat="server" ErrorMessage="医院名称必填" CssClass="field-validation-error"
                                                                ControlToValidate="txtHospitalName"></asp:RequiredFieldValidator>
                                                            <asp:CustomValidator ID="cvHospitalName" runat="server" ControlToValidate="txtHospitalName" CssClass="field-validation-error"
                                                                ErrorMessage="医院名称已存在，请重新输入" OnServerValidate="cvHospitalName_ServerValidate"></asp:CustomValidator>
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
                                                <PagerStyle PagerTextFormat="{4} 第{0}页/共{1}页, 第{2}-{3}条 共{5}条"
                                                    PageSizeControlType="RadComboBox" PageSizeLabelText="每页条数:"
                                                    FirstPageToolTip="第一页" PrevPageToolTip="上一页" NextPageToolTip="下一页" LastPageToolTip="最后一页" />
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                        <asp:HiddenField ID="hdnCurrentEditID" runat="server" Value="-1" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="mws-button-row">
                            <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                            <asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="mws-button orange" CausesValidation="false" OnClick="btnDelete_Click" OnClientClick="return onConfirmDelete();" />
                            <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Basics/DBContractManagement.aspx');return false;" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnCurrentEntityID" runat="server" Value="-1" />

    <style type="text/css">
        .mws-table tbody td, .mws-table tfoot td {
            padding-left: 5px;
            padding-right: 5px;
        }

        .RadGrid input {
            height: 28px;
            border: 1px solid #c5c5c5;
            padding: 6px 7px;
            color: #323232;
            margin: 0;
            background-color: #ffffff;
            outline: none;
            /* CSS 3 */
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            -o-border-radius: 4px;
            -khtml-border-radius: 4px;
            border-radius: 4px;
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
            -moz-box-shadow: inset 0px 1px 3px rgba(128, 128, 128, 0.1);
            -o-box-shadow: inset 0px 1px 3px rgba(128, 128, 128, 0.1);
            -webkit-box-shadow: inset 0px 1px 3px rgba(128, 128, 128, 0.1);
            -khtml-box-shadow: inset 0px 1px 3px rgba(128, 128, 128, 0.1);
            box-shadow: inset 0px 1px 3px rgba(128, 128, 128, 0.1);
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">

        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/Basics/DBContractManagement.aspx");
        }

        function refreshMaintenancePage(sender, args) {

            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            redirectToPage("Views/Basics/DBContractMaintenance.aspx?EntityID=" + currentEntityID);
        }

    </script>
</asp:Content>
