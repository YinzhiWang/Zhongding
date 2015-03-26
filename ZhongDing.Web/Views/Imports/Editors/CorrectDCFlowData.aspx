<%@ Page Title="纠正流向数据" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="CorrectDCFlowData.aspx.cs" Inherits="ZhongDing.Web.Views.Imports.Editors.CorrectDCFlowData" %>

<%@ MasterType VirtualPath="~/Site.Window.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
                        <div class="float-left width50-percent">
                            <label>配送公司</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblDistributionCompany" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="float-left">
                            <label>销售日期</label>
                            <div class="mws-form-item">
                                <telerik:RadDatePicker runat="server" ID="rdpSaleDate" Width="120"
                                    Calendar-EnableShadows="true"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                    Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                    Calendar-FastNavigationSettings-DisableOutOfRangeMonths="true"
                                    Calendar-FirstDayOfWeek="Monday">
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="rfvSaleDate"
                                    runat="server"
                                    ErrorMessage="请选择销售日期"
                                    ControlToValidate="rdpSaleDate"
                                    Display="Dynamic" CssClass="field-validation-error"
                                    ValidationGroup="vgMaintenance" Text="*">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>货品编号</label>
                            <div class="mws-form-item">
                                <telerik:RadTextBox runat="server" ID="txtProductCode" CssClass="mws-textinput"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="rfvProductCode"
                                    runat="server"
                                    ErrorMessage="货品编号必填"
                                    ControlToValidate="txtProductCode"
                                    Display="Dynamic" CssClass="field-validation-error"
                                    ValidationGroup="vgMaintenance" Text="*">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="float-left">
                            <label>货品名称</label>
                            <div class="mws-form-item">
                                <telerik:RadTextBox runat="server" ID="txtProductName" CssClass="mws-textinput"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="rfvProductName"
                                    runat="server"
                                    ErrorMessage="货品名称必填"
                                    ControlToValidate="txtProductName"
                                    Display="Dynamic" CssClass="field-validation-error"
                                    ValidationGroup="vgMaintenance" Text="*">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>货品规格</label>
                            <div class="mws-form-item">
                                <telerik:RadTextBox runat="server" ID="txtSpecification" CssClass="mws-textinput"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="rfvSpecification"
                                    runat="server"
                                    ErrorMessage="货品规格必填"
                                    ControlToValidate="txtSpecification"
                                    Display="Dynamic" CssClass="field-validation-error"
                                    ValidationGroup="vgMaintenance" Text="*">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="float-left">
                            <label>生产企业</label>
                            <div class="mws-form-item">
                                <telerik:RadTextBox runat="server" ID="txtFactoryName" CssClass="mws-textinput"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="rfvFactoryName"
                                    runat="server"
                                    ErrorMessage="生产企业必填"
                                    ControlToValidate="txtFactoryName"
                                    Display="Dynamic" CssClass="field-validation-error"
                                    ValidationGroup="vgMaintenance" Text="*">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>出货数量</label>
                            <div class="mws-form-item">
                                <telerik:RadNumericTextBox runat="server" ID="txtSaleQty" CssClass="mws-textinput" Type="Number" ShowSpinButtons="true"
                                    NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999"
                                    MaxLength="10">
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="rfvSaleQty"
                                    runat="server"
                                    ErrorMessage="出货数量必填"
                                    ControlToValidate="txtSaleQty"
                                    Display="Dynamic" CssClass="field-validation-error"
                                    ValidationGroup="vgMaintenance" Text="*">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="float-left">
                            <label>流向医院</label>
                            <div class="mws-form-item">
                                <telerik:RadComboBox runat="server" ID="rcbxHospital" Filter="Contains"
                                    AllowCustomText="false" Height="160px" EmptyMessage="--请选择--">
                                </telerik:RadComboBox>
                                <telerik:RadToolTip ID="rttHospital" runat="server" TargetControlID="rcbxHospital" ShowEvent="OnClick"
                                    Position="MiddleRight" RelativeTo="Element" Text="该项是必选项" AutoCloseDelay="0">
                                </telerik:RadToolTip>
                                <asp:RequiredFieldValidator ID="rfvHospital" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="rcbxHospital"
                                    ErrorMessage="请选择配送公司" Text="*" CssClass="field-validation-error">
                                </asp:RequiredFieldValidator>
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

    <asp:HiddenField ID="hdnNewEntityID" runat="server" />

    <asp:HiddenField ID="hdnGridClientID" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">

    <script type="text/javascript">
        function closeWindow(needRebindGrid) {

            var oWin = $.getRadWindow();

            if (oWin) {

                if (needRebindGrid) {

                    var browserWindow = oWin.get_browserWindow();

                    var newEntityID = $("#<%= hdnNewEntityID.ClientID%>").val();

                    if (!newEntityID.isNullOrEmpty()) {

                        browserWindow.location.href = $.getRootPath() + "Views/Imports/DCFlowDataMaintenance.aspx?EntityID=" + newEntityID;
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

    </script>
</asp:Content>
