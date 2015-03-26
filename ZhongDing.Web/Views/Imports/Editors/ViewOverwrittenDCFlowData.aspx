<%@ Page Title="被覆盖的流向数据" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="ViewOverwrittenDCFlowData.aspx.cs" Inherits="ZhongDing.Web.Views.Imports.Editors.ViewOverwrittenDCFlowData" %>

<%@ MasterType VirtualPath="~/Site.Window.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="mws-panel grid_full" style="margin-bottom: 10px;">
        <div class="mws-panel-body">
            <div class="mws-form">
                <div class="mws-form-inline">
                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>配送公司</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblDistributionCompany" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="float-left">
                            <label>销售日期</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblSaleDate" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>货品编号</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblProductCode" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="float-left">
                            <label>货品名称</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblProductName" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>货品规格</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblSpecification" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="float-left">
                            <label>生产企业</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblFactoryName" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>出货数量</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblSaleQty" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="float-left">
                            <label>流向</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblFlow" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="mws-button-row">
                        <asp:Button ID="btnCancel" runat="server" Text="关闭" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="closeWindow();return false;" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">
        function closeWindow() {

            var oWin = $.getRadWindow();

            if (oWin) {

                var isDestroyOnClose = oWin.get_destroyOnClose();
                if (isDestroyOnClose) {
                    oWin.set_destroyOnClose(false);
                }

                if (!oWin.isClosed()) {
                    oWin.close();
                }
            }
        }

        function onError(sender, args) {
            closeWindow();
        }

    </script>
</asp:Content>
