<%@ Page Title="配送公司流向数据" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StockInImportData.aspx.cs" Inherits="ZhongDing.Web.Views.Imports.StockInImportData" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgEntities" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblSearch" />
                    <telerik:AjaxUpdatedControl ControlID="rgEntities" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgEntities">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgEntities" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <!-- Main Container -->
    <div class="container">
        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-table-1">采购订单导入记录</span>
            </div>
            <div class="mws-panel-body">
                <table runat="server" id="tblSearch" class="leftmargin10" style="display: none;">
                    <tr class="height40">
                        <th class="width100 middle-td">订单起止日期：</th>
                        <td class="middle-td" colspan="3">
                            <telerik:RadDatePicker runat="server" ID="rdpBeginDate" Width="120"></telerik:RadDatePicker>
                            -&nbsp;&nbsp;
                            <telerik:RadDatePicker runat="server" ID="rdpEndDate" Width="120"></telerik:RadDatePicker>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr class="height40">
                        <th class="width60 middle-td right-td">供应商：</th>
                        <td class="middle-td width35-percent">
                            <telerik:RadComboBox runat="server" ID="rcbxSupplier" Filter="Contains"
                                Height="160px" Width="260" EmptyMessage="--请选择--">
                            </telerik:RadComboBox>
                        </td>

                        <td class="middle-td leftpadding20">
                            <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="mws-button green" OnClick="btnSearch_Click" />
                        </td>
                        <td class="middle-td leftpadding20">
                            <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="mws-button orange" OnClick="btnReset_Click" />
                             
                        </td>
                    </tr>
                </table>
                <div class="mws-form">
                    <div class="mws-form-inline">
                        <div class="mws-form-row">
                            <div class="validate-message-wrapper">
                                <asp:ValidationSummary ID="vsMaintenance" runat="server" ValidationGroup="vgMaintenance" DisplayMode="BulletList" HeaderText="请更正以下错误:" CssClass="validation-summary-errors" />
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>选择文件</label>
                                <div class="mws-form-item small">
                                    <table>
                                        <tr>
                                            <td style="width: 260px;">
                                                <telerik:RadAsyncUpload Culture="ZH-cn" runat="server" ID="radAsyncUpload"
                                                    Width="240px" HideFileInput="false" MultipleFileSelection="Disabled" AllowedFileExtensions=".xls,.xlsx"
                                                    ManualUpload="false"
                                                    OnClientFileUploading="onFileUploading" OnClientValidationFailed="onValidationFailed"
                                                    OnClientFileUploadRemoved="fileDeleted" OnClientFileUploaded="fileUploaded">
                                                    <Localization Select="选择" Cancel="取消" Remove="移除" />
                                                </telerik:RadAsyncUpload>
                                            </td>
                                            <td>
                                                <asp:CustomValidator ID="cvRadAsyncUpload" runat="server" ErrorMessage="请选择文件"
                                                    ValidationGroup="vgMaintenance" Display="Dynamic"
                                                    Text="*" CssClass="field-validation-error">
                                                </asp:CustomValidator></td>
                                        </tr>
                                    </table>


                                </div>

                            </div>
                            <div class="float-left">
                                <label>文件名称</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtFileName" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="rfvTransportCompanyNumber" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtFileName"
                                        ErrorMessage="文件名称必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                    <telerik:RadToolTip ID="RadToolTip1" runat="server" TargetControlID="txtFileName" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                </div>

                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <asp:Button ID="btnImportData" runat="server" Text="导入收货入库单" CssClass="mws-button green" OnClick="btnImportData_Click"
                                    CausesValidation="true" ValidationGroup="vgMaintenance" />
                                   &nbsp;&nbsp; &nbsp;&nbsp;
                                     <asp:HyperLink ID="hlkModelExcel" runat="server" NavigateUrl="~/Content/Templates/XXXX采购入库单导入(XXXX年XX月).xlsx">Excel模板下载</asp:HyperLink>
                        
                            </div>
                        </div>

                    </div>
                    <div class="mws-form-inline">
                        <div class="mws-form-row">
                            <div class="float-left width40-percent" runat="server" id="divInfo" visible="false">
                                <label>信息</label>
                                <div class="mws-form-item small">
                                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
                                </div>

                            </div>

                        </div>

                    </div>
                    <div class="height20"></div>
                </div>

                <telerik:RadGrid ID="rgEntities" runat="server" PageSize="10"
                    AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                    MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                    ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                    OnNeedDataSource="rgEntities_NeedDataSource" OnDeleteCommand="rgEntities_DeleteCommand"
                    OnItemCreated="rgEntities_ItemCreated" OnColumnCreated="rgEntities_ColumnCreated"
                    OnItemDataBound="rgEntities_ItemDataBound" OnItemCommand="rgEntities_ItemCommand">
                    <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                        ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                        <Columns>
                            <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false">
                                <ItemStyle HorizontalAlign="Left" Width="50" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ImportBeginDate" HeaderText="导入开始时间" DataField="ImportBeginDate" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}">
                                <HeaderStyle Width="15%" />
                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ImportEndDate" HeaderText="导入结束时间" DataField="ImportEndDate" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}">
                                <HeaderStyle Width="15%" />
                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ImportStatus" HeaderText="状态" DataField="ImportStatus">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="View" HeaderText="查看">
                                <HeaderStyle HorizontalAlign="Center" Width="60" />
                                <ItemStyle HorizontalAlign="Center" Width="60" />
                                <ItemTemplate>
                                    <a href="javascript:void(0);" onclick="redirectToMaintenancePage(<%#DataBinder.Eval(Container.DataItem,"ImportFileLogID")%>)">查看</a>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <CommandItemTemplate>
                            <table class="width100-percent">
                                <tr>
                                    <td>
                                        <asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
                                            <input type="button" class="rgAdd" onclick="redirectToMaintenancePage(-1); return false;" />
                                            <a href="javascript:void(0)" onclick="redirectToMaintenancePage(-1); return false;">添加</a>
                                        </asp:Panel>
                                        <%--<asp:Panel ID="plExportCommand" runat="server" CssClass="width80 float-left">
                                            <input type="button" class="rgExpXLS" onclick="exportExcel(); return false;" />
                                            <a href="javascript:void(0);" onclick="exportExcel(); return false;">导出excel</a>
                                        </asp:Panel>--%>
                                    </td>
                                    <td class="right-td rightpadding10">
                                        <input type="button" class="rgRefresh" onclick="refreshGrid(); return false;" />
                                        <a href="javascript:void(0);" onclick="refreshGrid(); return false;">刷新</a>
                                    </td>
                                </tr>
                            </table>
                        </CommandItemTemplate>
                        <NoRecordsTemplate>
                            没有任何数据
                        </NoRecordsTemplate>
                        <ItemStyle Height="30" />
                        <AlternatingItemStyle BackColor="#f2f2f2" />
                        <PagerStyle PagerTextFormat="{4} 第{0}页/共{1}页, 第{2}-{3}条 共{5}条"
                            PageSizeControlType="RadComboBox" PageSizeLabelText="每页条数:"
                            FirstPageToolTip="第一页" PrevPageToolTip="上一页" NextPageToolTip="下一页" LastPageToolTip="最后一页" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true">
                        <ClientEvents OnGridCreated="GetsGridObject" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">
        var gridOfRefresh = null;

        function GetsGridObject(sender, eventArgs) {
            gridOfRefresh = sender;
        }

        function refreshGrid() {
            gridOfRefresh.get_masterTableView().rebind();
        }

        function redirectToMaintenancePage(id) {
            $.showLoading();
            window.location.href = "ProcureOrderImportDataDetails.aspx?EntityID=" + id;
        }

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

        }

        function fileUploaded(sender, args) {
            //debugger;
            //alert("hit 1:" + args.get_fileName().substring(0, args.get_fileName().lastIndexOf('.') + 1));
            var fileName = args.get_fileName().substring(0, args.get_fileName().lastIndexOf('.'));

            var fileInfo = args.get_fileInfo();

            $find("<%= txtFileName.ClientID %>").set_value(fileName);

            //regenerateUploadedFiles(fileName);

        }

        function fileDeleted(sender, args) {
            //debugger;

        }
    </script>
</asp:Content>
