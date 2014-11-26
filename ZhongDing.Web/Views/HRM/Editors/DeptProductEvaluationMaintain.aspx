<%@ Page Title="部门考核产品维护" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="DeptProductEvaluationMaintain.aspx.cs" Inherits="ZhongDing.Web.Views.HRM.Editors.DeptProductEvaluationMaintain" %>

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
                        <label style="width: 200px">货品</label>
                        <div class="mws-form-item small">
                            <telerik:RadComboBox runat="server" ID="rcbxProducts" Filter="Contains" AllowCustomText="false"
                                MarkFirstMatch="true" Height="160px" EmptyMessage="--请选择--" Width="60%">
                            </telerik:RadComboBox>
                            <telerik:RadToolTip ID="rttSpecification" runat="server" TargetControlID="rcbxProducts" ShowEvent="OnClick"
                                Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                            </telerik:RadToolTip>
                            <asp:RequiredFieldValidator ID="rfv"
                                runat="server"
                                ErrorMessage="货品必填"
                                ControlToValidate="rcbxProducts"
                                Display="Dynamic" CssClass="field-validation-error"
                                ValidationGroup="vgMaintenance" Text="*">
                            </asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cvProducts" runat="server" ErrorMessage="该货品不存在，请重新选择"
                                ControlToValidate="rcbxProducts" ValidationGroup="vgMaintenance" Display="Dynamic"
                                Text="*" CssClass="field-validation-error" OnServerValidate="cvProducts_ServerValidate">
                            </asp:CustomValidator>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <label style="width: 200px">开发考核占季度基础量提成百分比</label>
                        <div class="mws-form-item small">
                            <telerik:RadNumericTextBox runat="server" ID="txtInvestigateRatio" CssClass="mws-textinput" Width="180"
                                Type="Percent" NumberFormat-DecimalDigits="2" ShowSpinButtons="true" EmptyMessage="0.00%" MinValue="0" MaxValue="100" DbValueFactor="100">
                            </telerik:RadNumericTextBox>
                            <telerik:RadToolTip ID="rttInvestigateRatio" runat="server" TargetControlID="txtInvestigateRatio" ShowEvent="OnClick"
                                Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                            </telerik:RadToolTip>
                            <asp:RequiredFieldValidator ID="rfvInvestigateRatio"
                                runat="server"
                                ErrorMessage="开发考核占季度基础量提成百分比必填"
                                ControlToValidate="txtInvestigateRatio"
                                Display="Dynamic" CssClass="field-validation-error"
                                ValidationGroup="vgMaintenance" Text="*">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <label style="width: 200px">销售考核占季度基础量提成百分比</label>
                        <div class="mws-form-item small">
                            <telerik:RadNumericTextBox runat="server" ID="txtSalesRatio" CssClass="mws-textinput" Width="180"
                                Type="Percent" NumberFormat-DecimalDigits="2" ShowSpinButtons="true" EmptyMessage="0.00%" MinValue="0" MaxValue="100" DbValueFactor="100">
                            </telerik:RadNumericTextBox>
                            <telerik:RadToolTip ID="rttSalesRatio" runat="server" TargetControlID="txtSalesRatio" ShowEvent="OnClick"
                                Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                            </telerik:RadToolTip>
                            <asp:RequiredFieldValidator ID="rfvSalesRatio"
                                runat="server"
                                ErrorMessage="销售考核占季度基础量提成百分比必填"
                                ControlToValidate="txtSalesRatio"
                                Display="Dynamic" CssClass="field-validation-error"
                                ValidationGroup="vgMaintenance" Text="*">
                            </asp:RequiredFieldValidator>
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

    </script>
</asp:Content>

