<%@ Page Title="商业客户流向维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientInfoProductFlowMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.ClientInfoProductFlowMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbxClientUser">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divClientCompanies" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbxProduct">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divProductSpecifications" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbxDepartment">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divDeptMarkets" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="container">

        <div class="mws-panel grid_full">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">商业客户流向维护</span>
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
                                <label>客户</label>
                                <div class="mws-form-item">
                                    <telerik:RadComboBox runat="server" ID="rcbxClientUser" Filter="Contains" AllowCustomText="false"
                                        MarkFirstMatch="true" Height="160px" EmptyMessage="--请选择--"
                                        AutoPostBack="true" OnSelectedIndexChanged="rcbxClientUser_SelectedIndexChanged">
                                    </telerik:RadComboBox>
                                    <telerik:RadToolTip ID="rttClientName" runat="server" TargetControlID="rcbxClientUser" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                    <asp:RequiredFieldValidator ID="rfvClientUser"
                                        runat="server"
                                        ErrorMessage="请选择客户"
                                        ControlToValidate="rcbxClientUser"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="float-left width60-percent">
                                <label>商业单位</label>
                                <div class="mws-form-item" runat="server" id="divClientCompanies">
                                    <telerik:RadDropDownList runat="server" ID="ddlClientCompany" Width="360" EmptyMessage="--请选择--">
                                    </telerik:RadDropDownList>
                                    <telerik:RadToolTip ID="rttClientCompany" runat="server" TargetControlID="ddlClientCompany" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                    <asp:RequiredFieldValidator ID="rfvClientCompany"
                                        runat="server"
                                        ErrorMessage="请选择商业单位"
                                        ControlToValidate="ddlClientCompany"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>货品</label>
                            <div class="mws-form-item medium">
                                <telerik:RadComboBox runat="server" ID="rcbxProduct" Filter="Contains" AutoPostBack="true"
                                    AllowCustomText="false" Height="160px" Width="60%" EmptyMessage="--请选择--"
                                    OnItemDataBound="rcbxProduct_ItemDataBound" OnClientSelectedIndexChanged="onClientSelectedProduct"
                                    OnSelectedIndexChanged="rcbxProduct_SelectedIndexChanged">
                                </telerik:RadComboBox>
                                <telerik:RadToolTip ID="rttProduct" runat="server" TargetControlID="rcbxProduct" ShowEvent="OnClick"
                                    Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                </telerik:RadToolTip>
                                <asp:RequiredFieldValidator ID="rfvProduct"
                                    runat="server"
                                    ErrorMessage="请选择货品"
                                    ControlToValidate="rcbxProduct"
                                    Display="Dynamic" CssClass="field-validation-error"
                                    ValidationGroup="vgMaintenance" Text="*">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>货品规格</label>
                                <div class="mws-form-item small" runat="server" id="divProductSpecifications">
                                    <telerik:RadDropDownList runat="server" ID="ddlProductSpecification" DefaultMessage="--请选择--">
                                    </telerik:RadDropDownList>
                                    <telerik:RadToolTip ID="rttProductSpecification" runat="server" TargetControlID="ddlProductSpecification" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                    <asp:RequiredFieldValidator ID="rfvProductSpecification"
                                        runat="server"
                                        ErrorMessage="请选择货品规格"
                                        ControlToValidate="ddlProductSpecification"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>生产企业</label>
                                <div class="mws-form-item" style="padding-top: 8px;">
                                    <asp:Label ID="lblFactoryName" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>高开价</label>
                                <div class="mws-form-item small">
                                    <telerik:RadNumericTextBox runat="server" ID="txtHighPrice" CssClass="mws-textinput" Type="Currency"
                                        NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999"
                                        MaxLength="10" ShowSpinButtons="true">
                                    </telerik:RadNumericTextBox>
                                </div>
                            </div>
                            <div class="float-left width50-percent">
                                <label>基本销售价</label>
                                <div class="mws-form-item small">
                                    <telerik:RadNumericTextBox runat="server" ID="txtBasicPrice" CssClass="mws-textinput" Type="Currency"
                                        NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999"
                                        MaxLength="10" ShowSpinButtons="true">
                                    </telerik:RadNumericTextBox>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>使用流向数据？</label>
                                <div class="mws-form-item small">
                                    <telerik:RadButton runat="server" ID="radioIsUseFlowData" ButtonType="ToggleButton" ToggleType="Radio" AutoPostBack="false"
                                        GroupName="UseFlowData" Text="是" Value="true" Checked="true" CommandArgument="IsUseFlowData" OnClientCheckedChanged="onUseFlowDataCheckedChanged">
                                    </telerik:RadButton>
                                    <telerik:RadButton runat="server" ID="radioIsNotUseFlowData" ButtonType="ToggleButton" ToggleType="Radio" AutoPostBack="false"
                                        GroupName="UseFlowData" Text="否" Value="false" CommandArgument="IsNotUseFlowData" OnClientCheckedChanged="onUseFlowDataCheckedChanged">
                                    </telerik:RadButton>
                                </div>
                            </div>
                            <div class="float-left width50-percent" id="divDepartmentsDistricts">
                                <label>部门地区</label>
                                <div class="mws-form-item medium" runat="server" id="divDeptMarkets" >
                                    <telerik:RadComboBox runat="server" ID="rcbxDepartment" Filter="Contains" AllowCustomText="false"
                                        MarkFirstMatch="true" Height="160px" EmptyMessage="--请选择--"
                                        AutoPostBack="true" OnSelectedIndexChanged="rcbxDepartment_SelectedIndexChanged">
                                    </telerik:RadComboBox>
                                    <asp:CustomValidator ID="cvDepartment" runat="server" ErrorMessage="请选择部门"
                                        ControlToValidate="rcbxDepartment" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                    &nbsp;
                                    <telerik:RadDropDownList runat="server" ID="ddlDeptMarketDivision" DefaultMessage="--请选择--" Width="100">
                                    </telerik:RadDropDownList>
                                    <asp:CustomValidator ID="cvDeptMarketDivision" runat="server" ErrorMessage="请选择部门地区"
                                        ControlToValidate="ddlDeptMarketDivision" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>月任务量</label>
                                <div class="mws-form-item small">
                                    <telerik:RadNumericTextBox runat="server" ID="txtMonthlyTask" CssClass="mws-textinput" Type="Number"
                                        NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999"
                                        MaxLength="10" ShowSpinButtons="true">
                                    </telerik:RadNumericTextBox>
                                </div>
                            </div>
                            <div class="float-left width50-percent">
                                <label>返款价</label>
                                <div class="mws-form-item small">
                                    <telerik:RadNumericTextBox runat="server" ID="txtRefundPrice" CssClass="mws-textinput" Type="Currency"
                                        NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999"
                                        MaxLength="10" ShowSpinButtons="true">
                                    </telerik:RadNumericTextBox>
                                </div>
                            </div>
                        </div>
                        <div class="mws-button-row">
                            <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                            <asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="mws-button orange" CausesValidation="false" OnClick="btnDelete_Click" OnClientClick="return onConfirmDelete();" />
                            <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Basics/ClientInfoProductFlowManagement.aspx');return false;" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">
        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/Basics/ClientInfoProductFlowManagement.aspx");
        }

        function onClientSelectedProduct(sender, eventArgs) {
            var item = sender.get_selectedItem();

            var extension = item.get_attributes().getAttribute("Extension");

            if (extension) {
                var extensionObj = JSON.parse(extension);
                if (extensionObj) {
                    $("#<%= lblFactoryName.ClientID%>").html(extensionObj.FactoryName);
                }
            }
        }

        function onUseFlowDataCheckedChanged(sender, eventArgs) {
            var commandArgument = eventArgs.get_commandArgument();
            var checked = eventArgs.get_checked();

            if (checked && commandArgument === "IsUseFlowData") {
                $("#divDepartmentsDistricts").hide();
            }
            else if (checked && commandArgument === "IsNotUseFlowData") {
                $("#divDepartmentsDistricts").show();
            }
        }

        $(document).ready(function () {
            var radioIsNotUseFlowData = $find("<%= radioIsNotUseFlowData.ClientID%>");

            if (radioIsNotUseFlowData.get_checked() == true)
                $("#divDepartmentsDistricts").show();
            else
                $("#divDepartmentsDistricts").hide();
        });
    </script>
</asp:Content>
