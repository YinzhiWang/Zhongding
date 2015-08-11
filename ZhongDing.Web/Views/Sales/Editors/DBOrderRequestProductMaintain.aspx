<%@ Page Title="大包申请单货品维护" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="DBOrderRequestProductMaintain.aspx.cs" Inherits="ZhongDing.Web.Views.Sales.Editors.DBOrderRequestProductMaintain" %>

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
                    <telerik:AjaxUpdatedControl ControlID="divProductSpecifications" LoadingPanelID="loadingPanel" />
                     <telerik:AjaxUpdatedControl ControlID="divProductSalesPrice" LoadingPanelID="loadingPanel" />
                    
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlProductSpecification">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divProductSpecifications" LoadingPanelID="loadingPanel" />
                      <telerik:AjaxUpdatedControl ControlID="divProductSalesPrice" LoadingPanelID="loadingPanel" />
                    
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
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
                        <label>货品</label>
                        <div class="mws-form-item small">
                            <telerik:RadComboBox runat="server" ID="rcbxProduct" Filter="Contains" AutoPostBack="true"
                                AllowCustomText="false" Height="160px" Width="50%" EmptyMessage="--请选择--"
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
                        <label>货品规格</label>
                        <div class="mws-form-item small" runat="server" id="divProductSpecifications">
                            <telerik:RadDropDownList runat="server" ID="ddlProductSpecification" DefaultMessage="--请选择--" OnSelectedIndexChanged="ddlProductSpecification_SelectedIndexChanged"
                                AutoPostBack="true">
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
                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>单价</label>
                            <div class="mws-form-item small"  runat="server" id="divProductSalesPrice">
                                <telerik:RadNumericTextBox runat="server" ID="txtSalesPrice" CssClass="mws-textinput" Type="Currency" ShowSpinButtons="true"
                                    NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999"
                                    MaxLength="10" ClientEvents-OnValueChanged="onPriceValueChanged">
                                </telerik:RadNumericTextBox>
                                <telerik:RadToolTip ID="rttSalesPrice" runat="server" TargetControlID="txtSalesPrice" ShowEvent="OnClick"
                                    Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                </telerik:RadToolTip>
                                <asp:RequiredFieldValidator ID="rfvSalesPrice"
                                    runat="server"
                                    ErrorMessage="单价必填"
                                    ControlToValidate="txtSalesPrice"
                                    Display="Dynamic" CssClass="field-validation-error"
                                    ValidationGroup="vgMaintenance" Text="*">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="float-left">
                            <label>数量</label>
                            <div class="mws-form-item small">
                                <telerik:RadNumericTextBox runat="server" ID="txtCount" CssClass="mws-textinput" Type="Number" ShowSpinButtons="true"
                                    NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999"
                                    MaxLength="10" ClientEvents-OnValueChanged="onCountValueChanged">
                                </telerik:RadNumericTextBox>
                                <telerik:RadToolTip ID="rttCount" runat="server" TargetControlID="txtCount" ShowEvent="OnClick"
                                    Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                </telerik:RadToolTip>
                                <asp:RequiredFieldValidator ID="rfvCount"
                                    runat="server"
                                    ErrorMessage="数量必填"
                                    ControlToValidate="txtCount"
                                    Display="Dynamic" CssClass="field-validation-error"
                                    ValidationGroup="vgMaintenance" Text="*">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <label>申请单金额</label>
                        <div class="mws-form-item small">
                            <telerik:RadNumericTextBox runat="server" ID="txtTotalSalesAmount" CssClass="mws-textinput" Type="Currency"
                                NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999"
                                MaxLength="10" Enabled="false">
                            </telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="height20"></div>
                </div>
                <div class="mws-button-row">
                    <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="closeWindow(false);return false;" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnGridClientID" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">
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

        function onClientHidden(sender, args) {
            closeWindow(true);
        }

        function onError(sender, args) {
            closeWindow(false);
        }

        function onPriceValueChanged(sender, eventArgs) {
            calTotalAmount();
        }

        function onCountValueChanged(sender, eventArgs) {
            calTotalAmount();
        }

        function calTotalAmount() {

            var totalAmount = -1;

            var countValue = $find("<%= txtCount.ClientID %>").get_value();
            var priceValue = $find("<%= txtSalesPrice.ClientID %>").get_value();

            if (!isNaN(priceValue) && !isNaN(countValue)) {
                totalAmount = priceValue * countValue;
            }

            var txtTotalAmount = $find("<%=txtTotalSalesAmount.ClientID%>");

            if (totalAmount >= 0)
                txtTotalAmount.set_value(totalAmount);
            else
                txtTotalAmount.set_value("");
        }

        $(document).ready(function () {

            calTotalAmount();
        });

    </script>
</asp:Content>
