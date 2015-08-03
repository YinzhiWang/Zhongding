<%@ Page Title="首页" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="ZhongDing.Web.Home" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Views/Reminder/UserControls/InventoryReminder.ascx" TagPrefix="uc1" TagName="InventoryReminder" %>
<%@ Register Src="~/Views/Reminder/UserControls/CautionMoneyReminder.ascx" TagPrefix="uc1" TagName="CautionMoneyReminder" %>
<%@ Register Src="~/Views/Reminder/UserControls/ProductInfoExpiredReminder.ascx" TagPrefix="uc1" TagName="ProductInfoExpiredReminder" %>
<%@ Register Src="~/Views/Reminder/UserControls/ClientInfoReminder.ascx" TagPrefix="uc1" TagName="ClientInfoReminder" %>
<%@ Register Src="~/Views/Reminder/UserControls/SupplierInfoReminder.ascx" TagPrefix="uc1" TagName="SupplierInfoReminder" %>
<%@ Register Src="~/Views/Reminder/UserControls/ProductExpiredReminder.ascx" TagPrefix="uc1" TagName="ProductExpiredReminder" %>
<%@ Register Src="~/Views/Reminder/UserControls/BorrowMoneyExpiredReminder.ascx" TagPrefix="uc1" TagName="BorrowMoneyExpiredReminder" %>
<%@ Register Src="~/Views/Reminder/UserControls/GuaranteeReceiptExpiredReminder.ascx" TagPrefix="uc1" TagName="GuaranteeReceiptExpiredReminder" %>








<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .rtsUL {
            zoom: 1.0;
        }-+
    </style>
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="tdLeft">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tdLeft" LoadingPanelID="loadingPanel" />
                    <%--<telerik:AjaxUpdatedControl ControlID="multiPages" LoadingPanelID="loadingPanel" />--%>
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="container">

        <div class="mws-panel grid_8">


            <table class="width100-percent" id="tbAll" runat="server">
                <tr>
                    <td>
                        <div id="tdLeft" runat="server">


                            <div class="mws-panel-header">
                                <span class="mws-i-24 i-sign-post">提醒详情</span>
                            </div>
                            <div class="mws-panel-body">
                                <telerik:RadTabStrip ID="tabStrips" runat="server" MultiPageID="multiPages" Skin="Office2010Silver"
                                    OnTabClick="tabStrips_TabClick"  >
                                    <Tabs>
                                        <telerik:RadTab Text="库存提醒" Value="tabInventory" PageViewID="pvInventory" Selected="true"></telerik:RadTab>
                                        <telerik:RadTab Text="保证金提醒" Value="tabCautionMoney" PageViewID="pvCautionMoney"></telerik:RadTab>
                                        <telerik:RadTab Text="货品资料过期" Value="tabProductInfoExpired" PageViewID="pvProductInfoExpired"></telerik:RadTab>
                                        <telerik:RadTab Text="客户资料过期" Value="tabClientInfo" PageViewID="pvClientInfo"></telerik:RadTab>
                                        <telerik:RadTab Text="供应商资料过期" Value="tabSupplier" PageViewID="pvSupplierInfo"></telerik:RadTab>
                                        <telerik:RadTab Text="货品过期提醒" Value="tabProductExpired" PageViewID="pvProductExpired"></telerik:RadTab>
                                        <telerik:RadTab Text="借款提醒" Value="tabBorrowMoneyExpired" PageViewID="pvBorrowMoneyExpired"></telerik:RadTab>
                                        <telerik:RadTab Text="担保提醒" Value="tabGuaranteeReceiptExpired" PageViewID="pvGuaranteeReceiptExpired"></telerik:RadTab>
                                    </Tabs>
                                </telerik:RadTabStrip>
                                <telerik:RadMultiPage ID="multiPages" runat="server" CssClass="multi-page-wrapper">
                                    <telerik:RadPageView ID="pvInventory" runat="server" Selected="true">
                                        <uc1:InventoryReminder runat="server" ID="inventoryReminder" />
                                    </telerik:RadPageView>
                                    <telerik:RadPageView ID="pvCautionMoney" runat="server">
                                        <uc1:CautionMoneyReminder runat="server" ID="cautionMoneyReminder" />

                                    </telerik:RadPageView>
                                    <telerik:RadPageView ID="pvProductInfoExpired" runat="server">
                                        <uc1:ProductInfoExpiredReminder runat="server" ID="productInfoExpiredReminder" />
                                    </telerik:RadPageView>
                                    <telerik:RadPageView ID="pvClientInfo" runat="server">
                                        <uc1:ClientInfoReminder runat="server" ID="clientInfoReminder" />
                                    </telerik:RadPageView>
                                    <telerik:RadPageView ID="pvSupplierInfo" runat="server">
                                        <uc1:SupplierInfoReminder runat="server" ID="supplierInfoReminder" />
                                    </telerik:RadPageView>
                                    <telerik:RadPageView ID="pvProductExpired" runat="server">
                                        <uc1:ProductExpiredReminder runat="server" ID="productExpiredReminder" />
                                    </telerik:RadPageView>
                                    <telerik:RadPageView ID="pvBorrowMoneyExpired" runat="server">
                                        <uc1:BorrowMoneyExpiredReminder runat="server" ID="borrowMoneyExpiredReminder" />
                                    </telerik:RadPageView>
                                    <telerik:RadPageView ID="pvGuaranteeReceiptExpired" runat="server">
                                        <uc1:GuaranteeReceiptExpiredReminder runat="server" ID="guaranteeReceiptExpiredReminder" />
                                    </telerik:RadPageView>
                                </telerik:RadMultiPage>
                            </div>
                        </div>
                    </td>
                    <td class="width10"></td>
                    <td class="width300">
                        <div class="mws-panel-header">
                            <span class="mws-i-24 i-sign-post">数量</span>
                        </div>
                        <div class="mws-panel-body">
                            <ul class="mws-summary">
                                <li id="liInventory" runat="server">
                                    <span>
                                        <asp:Label ID="lblInventory" runat="server" Text="-"></asp:Label></span> 库存预警
                                </li>
                                <li id="liCautionMoney" runat="server">
                                    <span>
                                        <asp:Label ID="lblCautionMoney" runat="server" Text="-"></asp:Label></span> 保证金到期
                                </li>
                                <li id="liProductInfoExpired" runat="server">
                                    <span>
                                        <asp:Label ID="lblProductInfoExpired" runat="server" Text="-"></asp:Label></span> 货品资料过期
                                </li>
                                <li id="liClientInfo" runat="server">
                                    <span>
                                        <asp:Label ID="lblClientInfo" runat="server" Text="-"></asp:Label></span> 客户资料过期
                                </li>
                                <li id="liSupplier" runat="server">
                                    <span>
                                        <asp:Label ID="lblSupplier" runat="server" Text="-"></asp:Label></span> 供应商资料过期
                                </li>
                                <li id="liProductExpired" runat="server">
                                    <span>
                                        <asp:Label ID="lblProductExpired" runat="server" Text="-"></asp:Label></span> 货品过期
                                </li>
                                <li id="liBorrowMoneyExpired" runat="server">
                                    <span>
                                        <asp:Label ID="lblBorrowMoneyExpired" runat="server" Text="-"></asp:Label></span> 借款到期
                                </li>
                                <li id="liGuaranteeReceiptExpired" runat="server">
                                    <span>
                                        <asp:Label ID="lblGuaranteeReceiptExpired" runat="server" Text="-"></asp:Label></span> 担保收款到期
                                </li>
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>




        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script>

    </script>
  
</asp:Content>
