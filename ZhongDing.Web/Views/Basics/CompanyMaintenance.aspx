<%@ Page Title="账套维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CompanyMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.CompanyMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">帐套管理</span>
            </div>
            <div class="mws-panel-body">
                <div class="mws-form">
                    <div class="mws-form-inline">
                        <div class="mws-form-row">
                            <label>帐套编号</label>
                            <div class="mws-form-item small">
                                <telerik:RadTextBox runat="server" ID="txtCompanyCode" CssClass="mws-textinput" Width="40%"></telerik:RadTextBox>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>帐套名称</label>
                            <div class="mws-form-item small">
                                <telerik:RadTextBox runat="server" ID="txtCompanyName" CssClass="mws-textinput" Width="40%"></telerik:RadTextBox>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label style="font-weight: bold">发票税点维护</label>
                            <div class="mws-form-item small">
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>供应商发票返点</label>
                            <div class="mws-form-item small">
                                <telerik:RadNumericTextBox runat="server" ID="txtProviderTexRatio" CssClass="mws-textinput"
                                    Type="Percent" NumberFormat-DecimalDigits="2" ShowSpinButtons="true" EmptyMessage="0.00%">
                                </telerik:RadNumericTextBox>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label style="font-weight: bold">客户发票返点</label>
                            <div class="mws-form-item small">
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>高开税率</label>
                            <div class="mws-form-item small">
                                <telerik:RadNumericTextBox runat="server" ID="txtClientTaxHighRatio" CssClass="mws-textinput"
                                    Type="Percent" NumberFormat-DecimalDigits="2" ShowSpinButtons="true" EmptyMessage="0.00%">
                                </telerik:RadNumericTextBox>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>低开税率</label>
                            <div class="mws-form-item small">
                                <telerik:RadNumericTextBox runat="server" ID="txtClientTaxLowRatio" CssClass="mws-textinput"
                                    Type="Percent" NumberFormat-DecimalDigits="2" ShowSpinButtons="true" EmptyMessage="0.00%">
                                </telerik:RadNumericTextBox>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>平进平出税率</label>
                            <div class="mws-form-item small">
                                <telerik:RadNumericTextBox runat="server" ID="txtClientTaxDeductionRatio" CssClass="mws-textinput"
                                    Type="Percent" NumberFormat-DecimalDigits="2" ShowSpinButtons="true" EmptyMessage="0.00%">
                                </telerik:RadNumericTextBox>&nbsp;&nbsp;&nbsp;&nbsp;启用
                                <telerik:RadButton runat="server" ID="cbxEnableTaxDeduction" AutoPostBack="false"
                                    ButtonType="ToggleButton" ToggleType="CheckBox" OnClientCheckedChanged="onCheckedChanged">
                                </telerik:RadButton>
                            </div>
                        </div>
                    </div>
                    <div class="mws-button-row">
                        <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" OnClick="btnSave_Click" />
                        <asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="mws-button green" OnClick="btnDelete_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Basics/CompanyManagement.aspx');return false;" />
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //debugger;
            var cbxEnableTaxDeduction = $find("<%= cbxEnableTaxDeduction.ClientID %>");

            if (cbxEnableTaxDeduction) {
                var enableTaxDeduction = cbxEnableTaxDeduction.get_checked();

                var txtClientTaxDeductionRatio = $find("<%= txtClientTaxDeductionRatio.ClientID%>");

                if (txtClientTaxDeductionRatio) {
                    if (enableTaxDeduction === true)
                        txtClientTaxDeductionRatio.enable();
                    else
                        txtClientTaxDeductionRatio.disable();
                }
            }

        });

        function onCheckedChanged(e) {
            //debugger;
            var isChecked = e.get_checked();

            var txtClientTaxDeductionRatio = $find("<%= txtClientTaxDeductionRatio.ClientID%>");

            if (txtClientTaxDeductionRatio) {
                if (isChecked === true)
                    txtClientTaxDeductionRatio.enable();
                else
                    txtClientTaxDeductionRatio.disable();
            }
        }

    </script>
</asp:Content>
