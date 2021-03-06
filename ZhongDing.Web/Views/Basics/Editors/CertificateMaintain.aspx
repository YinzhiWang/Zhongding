﻿<%@ Page Title="证照维护" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="CertificateMaintain.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.Editors.CertificateMaintain" %>

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
                        <label>证照类型</label>
                        <div class="mws-form-item small">
                            <telerik:RadComboBox runat="server" ID="rcbxCertificateType" AllowCustomText="false" Height="160px" EmptyMessage="--请选择--">
                            </telerik:RadComboBox>
                            <telerik:RadToolTip ID="rttCertificateType" runat="server" TargetControlID="rcbxCertificateType" ShowEvent="OnClick"
                                Position="MiddleRight" RelativeTo="Element" Text="该项是必选项" AutoCloseDelay="0">
                            </telerik:RadToolTip>
                            <asp:RequiredFieldValidator ID="rfvCertificateType" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="rcbxCertificateType"
                                ErrorMessage="请选择证照类型" Text="*" CssClass="field-validation-error">
                            </asp:RequiredFieldValidator>
                            <telerik:RadButton runat="server" ID="radioIsGotten" ButtonType="ToggleButton" ToggleType="Radio" AutoPostBack="false" GroupName="IsGotten" Text="有" Value="true"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radioIsNoGotten" ButtonType="ToggleButton" ToggleType="Radio" AutoPostBack="false" GroupName="IsGotten" Text="无" Value="false"></telerik:RadButton>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <label>有效期</label>
                        <div class="mws-form-item small">
                            <telerik:RadDatePicker ID="rdpEffectiveFrom" runat="server"
                                Calendar-EnableShadows="true" Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定" Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                Calendar-FirstDayOfWeek="Monday">
                            </telerik:RadDatePicker>
                            <asp:CustomValidator ID="cvRequiredEffectiveFrom" runat="server" ValidationGroup="vgMaintenance" Text="*" CssClass="field-validation-error"
                                 ErrorMessage="有效期开始日期必填"></asp:CustomValidator>
                            至&nbsp;&nbsp;
                            <telerik:RadDatePicker ID="rdpEffectiveTo" runat="server"
                                Calendar-EnableShadows="true" Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定" Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                Calendar-FirstDayOfWeek="Monday">
                            </telerik:RadDatePicker>
                            <asp:CustomValidator ID="cvRequiredEffectiveTo" runat="server" ValidationGroup="vgMaintenance" Text="*" CssClass="field-validation-error"
                                 ErrorMessage="有效期结束日期必填"></asp:CustomValidator>
                            <asp:CustomValidator ID="cvEffectiveDate" runat="server" ValidationGroup="vgMaintenance" Text="*" CssClass="field-validation-error"
                                OnServerValidate="cvEffectiveDate_ServerValidate" ErrorMessage="开始日期不能大于结束日期"></asp:CustomValidator>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <label>是否提醒</label>
                        <div class="mws-form-item small">
                            <telerik:RadButton runat="server" ID="cbxIsNeedAlert" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false" OnClientCheckedChanged="onCheckedChanged"></telerik:RadButton>
                            <div id="divAlertDays" class="hide">
                                <label>提醒期限</label>
                                <telerik:RadNumericTextBox ID="txtAlertBeforeDays" runat="server" Width="60" Type="Number" NumberFormat-GroupSeparator="" NumberFormat-DecimalDigits="0" MinValue="0" MaxValue="2147483647" MaxLength="9"></telerik:RadNumericTextBox>
                                <asp:CustomValidator ID="cvAlertBeforeDays" runat="server" ValidationGroup="vgMaintenance" Text="*" CssClass="field-validation-error" ErrorMessage="提醒期限必填"></asp:CustomValidator>
                                &nbsp;&nbsp;天
                            </div>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <label>备注</label>
                        <div class="mws-form-item medium">
                            <telerik:RadTextBox runat="server" ID="txtComment" Width="90%" MaxLength="1000"
                                TextMode="MultiLine" Height="80">
                            </telerik:RadTextBox>
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

        $(document).ready(function () {

            var cbxIsNeedAlert = $find("<%= cbxIsNeedAlert.ClientID %>");

            if (cbxIsNeedAlert) {

                var divAlertDays = $("#divAlertDays");

                var isNeedAlert = cbxIsNeedAlert.get_checked();

                if (isNeedAlert == true)
                    divAlertDays.show();
                else
                    divAlertDays.hide();
            }
        });

        function onCheckedChanged(e) {

            var isChecked = e.get_checked();

            var divAlertDays = $("#divAlertDays");

            if (divAlertDays) {
                if (isChecked === true)
                    divAlertDays.show();
                else
                    divAlertDays.hide();
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

        function onClientHidden(sender, args) {
            closeWindow(true);
        }

        function onError(sender, args) {
            closeWindow(false);
        }

    </script>
</asp:Content>
