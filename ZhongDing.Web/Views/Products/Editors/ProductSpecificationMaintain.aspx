<%@ Page Title="货品规格维护" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="ProductSpecificationMaintain.aspx.cs" Inherits="ZhongDing.Web.Views.Products.Editors.ProductSpecificationMaintain" %>

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
                        <label>规格</label>
                        <div class="mws-form-item small">
                            <telerik:RadTextBox runat="server" ID="txtSpecification" CssClass="mws-textinput" Width="40%" MaxLength="100"></telerik:RadTextBox>
                            <telerik:RadToolTip ID="rttSpecification" runat="server" TargetControlID="txtSpecification" ShowEvent="OnClick"
                                Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                            </telerik:RadToolTip>
                            <asp:RequiredFieldValidator ID="rfvAccountName" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtSpecification"
                                ErrorMessage="规格必填" Text="*" CssClass="field-validation-error">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <label>基本单位</label>
                        <div class="mws-form-item small">
                            <telerik:RadDropDownList runat="server" ID="ddlUnitOfMeasurement" DefaultMessage="--请选择--">
                            </telerik:RadDropDownList>
                            <telerik:RadToolTip ID="rttUnitOfMeasurement" runat="server" TargetControlID="ddlUnitOfMeasurement" ShowEvent="OnClick"
                                Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                            </telerik:RadToolTip>
                            <asp:RequiredFieldValidator ID="rfvUnitOfMeasurement"
                                runat="server"
                                ErrorMessage="请选择基本单位"
                                ControlToValidate="ddlUnitOfMeasurement"
                                Display="Dynamic" CssClass="field-validation-error"
                                ValidationGroup="vgMaintenance" Text="*">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <label>小包装数量</label>
                        <div class="mws-form-item small">
                            <telerik:RadNumericTextBox runat="server" ID="txtNumberInSmallPackage" CssClass="mws-textinput" Type="Number" ShowSpinButtons="true"
                                NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="10">
                            </telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <label>每件数量</label>
                        <div class="mws-form-item small">
                            <telerik:RadNumericTextBox runat="server" ID="txtNumberInLargePackage" CssClass="mws-textinput" Type="Number" ShowSpinButtons="true"
                                NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="10">
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

    </script>
</asp:Content>
