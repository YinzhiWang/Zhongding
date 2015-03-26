<%@ Page Title="导入医院流向数据" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="ImportDCHospitalFlowData.aspx.cs" Inherits="ZhongDing.Web.Views.Imports.Editors.ImportDCHospitalFlowData" %>

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
                        <label>文件</label>
                        <div class="mws-form-item toppadding5">
                            <asp:FileUpload ID="FileExcel" runat="server" AllowMultiple="false" />
                            <asp:CustomValidator ID="cvUploadFile" runat="server" Display="Dynamic"
                                ValidationGroup="vgMaintenance" Text="*" ControlToValidate="FileExcel"
                                CssClass="field-validation-error">
                            </asp:CustomValidator>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:HyperLink ID="hlkModelExcel" runat="server" NavigateUrl="~/Content/Templates/XXXX配送公司医院流向数据(XXXX年XX月).xlsx">Excel模板下载</asp:HyperLink>
                        </div>
                    </div>
                    <div class="mws-button-row">
                        <asp:Button ID="btnImport" runat="server" Text="导入" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnImport_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="closeWindow(false);return false;" />
                    </div>
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
