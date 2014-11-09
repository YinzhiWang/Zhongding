<%@ Page Title="合同文件维护" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="CertificateFileMaintain.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.Editors.CertificateFileMaintain" %>

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
                        <label>文件名称</label>
                        <div class="mws-form-item small">
                            <telerik:RadTextBox runat="server" ID="txtFileName" CssClass="mws-textinput" Width="40%" MaxLength="100"></telerik:RadTextBox>
                            <telerik:RadToolTip ID="rttAccountName" runat="server" TargetControlID="txtFileName" ShowEvent="OnClick"
                                Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                            </telerik:RadToolTip>
                            <asp:RequiredFieldValidator ID="rfvAccountName" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtFileName"
                                ErrorMessage="文件名必填" Text="*" CssClass="field-validation-error">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <label>上传文件</label>
                        <div class="mws-form-item small">
                            <telerik:RadAsyncUpload runat="server" ID="radUploadFile" AllowedFileExtensions="jpg,jpeg,png,gif,bmp,doc,docx,xls,xlsx,pdf"
                                MaxFileInputsCount="1" MultipleFileSelection="Disabled" MaxFileSize="5120000"
                                OnClientFileUploading="onFileUploading" OnClientValidationFailed="onValidationFailed"
                                OnClientFileUploadRemoved="fileDeleted" OnClientFileUploaded="fileUploaded"
                                HttpHandlerUrl="~/HttpHandle/AsyncUploadFileHandler.ashx"
                                OnFileUploaded="radUploadFile_FileUploaded" Width="95%">
                                <Localization Select="选择" Cancel="取消" Remove="移除" />
                            </telerik:RadAsyncUpload>
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
                    <div class="mws-form-row"></div>

                    <div class="mws-button-row">
                        <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="closeWindow(false);return false;" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnEntityID" runat="server" />
    <asp:HiddenField ID="hdnFilePath" runat="server" />
    <asp:HiddenField ID="hdnFileName" runat="server" />
    <asp:HiddenField ID="hdnGridClientID" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">

        function onValidationFailed(sender, args) {

            var isInvalidFile = false;

            var fileExtention = args.get_fileName().substring(args.get_fileName().lastIndexOf('.') + 1, args.get_fileName().length);
            if (args.get_fileName().lastIndexOf('.') != -1) {//this checks if the extension is correct
                if (sender.get_allowedFileExtensions().indexOf(fileExtention) == -1) {

                    isInvalidFile = true;
                    radalert("不支持该文件格式!", 300, 120, "警告");
                }
                else {
                    isInvalidFile = true;
                    radalert("请选择5MB以下的文件上传!", 300, 120, "警告");
                }
            }
            else {
                isInvalidFile = true;
                radalert("错误的文件格式!", 300, 120, "错误");

            }

            if (isInvalidFile) {
                sender.deleteAllFileInputs();
            }
        }

        function onFileUploading(sender, args) {

            //if (isNaN(imageWidth) || isNaN(imageHeight)
            //    || imageWidth <= 0 || imageHeight <= 0) {

            //    args.set_cancel(true);

            //    sender.deleteAllFileInputs();

            //    alert("请配置图片的宽度和高度！");
            //}
            //else {
            //    args.set_cancel(false);
            //}
        }

        function fileUploaded(sender, args) {
            //debugger;

            var fileName = args.get_fileName();

            var fileInfo = args.get_fileInfo();

            $("#<%= hdnFilePath.ClientID%>").val(fileInfo.FilePath);
            $("#<%= hdnFileName.ClientID %>").val(fileName);

            $find("<%= txtFileName.ClientID %>").set_value(fileInfo.FileNameWithoutExtension);

            regenerateUploadedFiles(fileName);

        }

        function fileDeleted(sender, args) {
            //debugger;
            var filePath = $("#<%= hdnFilePath.ClientID%>").val();

            if (filePath) {

                var entityID = $("#<%= hdnEntityID.ClientID%>").val();

                var requestObj = new Object();
                requestObj.AjaxActionType = AjaxActionTypes.DeleteFile;

                var uploadedFileObj = new Object();

                uploadedFileObj.OwnerTypeID = EOwnerTypes.Supplier;

                if (entityID && entityID > 0)
                    uploadedFileObj.SupplierContractFileID = entityID;

                uploadedFileObj.FilePath = filePath;

                requestObj.UploadedFile = uploadedFileObj;

                var requestData = JSON.stringify(requestObj);

                $.doAjaxAction("AsyncOperateFileHandler", requestData, function (reponse) {

                    if (!reponse.IsSuccess) {
                        radalert("删除文件失败", 300, 120, "提示");
                        return;
                    }
                    else {
                        $("#<%= hdnFilePath.ClientID%>").val("");
                        $("#<%= hdnFileName.ClientID %>").val("");
                    }
                });
            }
        }

        function regenerateUploadedFiles(fileName) {

            var uploadedFileHtml = "<li id=\"radUploadFilerow0\">";
            uploadedFileHtml += "<span class=\"ruFileWrap ruStyled\">";
            uploadedFileHtml += "<span class=\"ruUploadProgress ruUploadSuccess\">" + fileName + "</span>";
            uploadedFileHtml += "</span>";
            uploadedFileHtml += "<input name=\"RowRemove\" tabindex=\"0\" class=\"ruButton ruRemove\" type=\"button\" value=\"移除\" />";
            uploadedFileHtml += "</li>"

            $(".ruInputs").html(uploadedFileHtml);
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

        $(document).ready(function () {

            var fileName = $("#<%= hdnFileName.ClientID %>").val();

            if (!fileName.isNullOrEmpty()) {

                regenerateUploadedFiles(fileName);
            }
        });

    </script>
</asp:Content>
