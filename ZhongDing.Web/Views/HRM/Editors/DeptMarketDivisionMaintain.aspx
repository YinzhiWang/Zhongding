<%@ Page Title="部门产品线维护" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="DeptMarketDivisionMaintain.aspx.cs" Inherits="ZhongDing.Web.Views.HRM.Editors.DeptMarketDivisionMaintain" %>

<%@ MasterType VirtualPath="~/Site.Window.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="lbxAllProducts">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lbxAllProducts" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="lbxSelectedProducts" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lbxSelectedProducts">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lbxSelectedProducts" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="lbxAllProducts" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
        <ClientEvents OnResponseEnd="onResponseEnd" />
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
                        <label>业务经理</label>
                        <div class="mws-form-item small">
                            <telerik:RadComboBox runat="server" ID="rcbxDepartmentUsers" AllowCustomText="false" Height="160px" EmptyMessage="--请选择--">
                            </telerik:RadComboBox>
                            <telerik:RadToolTip ID="rttUsers" runat="server" TargetControlID="rcbxDepartmentUsers" ShowEvent="OnClick"
                                Position="MiddleRight" RelativeTo="Element" Text="该项是必选项" AutoCloseDelay="0">
                            </telerik:RadToolTip>
                            <asp:RequiredFieldValidator ID="rfvDepartmentUsers" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="rcbxDepartmentUsers"
                                ErrorMessage="请选择业务经理" Text="*" CssClass="field-validation-error">
                            </asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cvDepartmentUsers" runat="server" ErrorMessage="该员工不存在，请重新选择"
                                ControlToValidate="rcbxDepartmentUsers" ValidationGroup="vgMaintenance" Display="Dynamic"
                                Text="*" CssClass="field-validation-error" OnServerValidate="cvDepartmentUsers_ServerValidate">
                            </asp:CustomValidator>
                        </div>
                    </div>
                    <div class="mws-form-row" runat="server" id="divConfigProducts">
                        <div class="float-left width55-percent">
                            <telerik:RadListBox runat="server" ID="lbxAllProducts" AllowTransfer="true" AllowTransferOnDoubleClick="true"
                                TransferMode="Move" TransferToID="lbxSelectedProducts" Width="98%" Height="200">
                                <HeaderTemplate>
                                    <table class="width100-percent">
                                        <tr>
                                            <td colspan="2">货品</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="text" id="txtAllProductName" name="txtAllProductName" class="mws-textinput width100-percent" />
                                            </td>
                                            <td class="leftpadding10">
                                                <asp:Button ID="btnAllProductSearch" runat="server" Text="查询"
                                                    CssClass="mws-button green" OnClick="btnAllProductSearch_Click" OnClientClick="setSearchText(productListBoxTypes.AllProducts);" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class=" height5"></td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ButtonSettings Position="Right" ShowDelete="false" ShowReorder="false"
                                    TransferButtons="All" VerticalAlign="Bottom" AreaWidth="35" />
                                <Localization AllToRight="全部移到右边" AllToLeft="全部移到左边" ToRight="移到右边" ToLeft="移到左边" />
                            </telerik:RadListBox>
                        </div>
                        <div class="float-left width45-percent">
                            <telerik:RadListBox runat="server" ID="lbxSelectedProducts" AllowTransfer="true" AllowTransferOnDoubleClick="true" TransferMode="Move" Width="100%" Height="200">
                                <HeaderTemplate>
                                    <table class="width100-percent">
                                        <tr>
                                            <td colspan="2">负责货品</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="text" id="txtSelectedProductName" name="txtSelectedProductName" class="mws-textinput width100-percent" />
                                            </td>
                                            <td class="leftpadding10">
                                                <asp:Button ID="btnSearchSelectedProducts" runat="server" Text="查询"
                                                    CssClass="mws-button green" OnClick="btnSearchSelectedProducts_Click" OnClientClick="setSearchText(productListBoxTypes.SelectedProducts);" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class=" height5"></td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                            </telerik:RadListBox>
                        </div>
                    </div>

                    <div class="mws-form-row">
                        <div class="float-left width55-percent">
                            <telerik:RadListBox runat="server" ID="lbxAllDeptMarkets" AllowTransfer="true" AllowTransferOnDoubleClick="true"
                                TransferMode="Move" TransferToID="lbxSelectedDeptMarkets" Width="98%" Height="200">
                                <Items>
                                    <telerik:RadListBoxItem Text="Test1" Value="1" />
                                    <telerik:RadListBoxItem Text="Test2" Value="2" />
                                    <telerik:RadListBoxItem Text="Test3" Value="3" />
                                </Items>
                                <HeaderTemplate>
                                    <table class="width100-percent">
                                        <tr>
                                            <td>全部地区</td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ButtonSettings Position="Right" ShowDelete="false" ShowReorder="false"
                                    TransferButtons="All" VerticalAlign="Bottom" AreaWidth="35" />
                                <Localization AllToRight="全部移到右边" AllToLeft="全部移到左边" ToRight="移到右边" ToLeft="移到左边" />
                            </telerik:RadListBox>
                        </div>
                        <div class="float-left width45-percent">
                            <telerik:RadListBox runat="server" ID="lbxSelectedDeptMarkets" AllowTransferOnDoubleClick="true" Width="100%" Height="200">
                                <HeaderTemplate>
                                    <table class="width100-percent">
                                        <tr>
                                            <td>负责地区</td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                            </telerik:RadListBox>
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

    <asp:HiddenField ID="hdnSearchTextForAllProducts" runat="server" />
    <asp:HiddenField ID="hdnSearchTextForSelectedProducts" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">
        var productListBoxTypes = {
            AllProducts: 1,
            SelectedProducts: 2
        };

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

        function setSearchText(listBoxType) {
            switch (listBoxType) {
                case productListBoxTypes.AllProducts:
                    $("#<%=hdnSearchTextForAllProducts.ClientID%>").val($("#txtAllProductName").val());
                    break;

                case productListBoxTypes.SelectedProducts:
                    $("#<%= hdnSearchTextForSelectedProducts.ClientID%>").val($("#txtSelectedProductName").val());
                    break;
            }
        }

        function onResponseEnd(sender, args) {
            $("#txtAllProductName").val($("#<%=hdnSearchTextForAllProducts.ClientID%>").val());
            $("#txtSelectedProductName").val($("#<%= hdnSearchTextForSelectedProducts.ClientID%>").val());
        }
    </script>
</asp:Content>
