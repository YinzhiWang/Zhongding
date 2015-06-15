<%@ Page Title="借款维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BorrowMoneyMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.BorrowMoneyMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">借款维护</span>
            </div>
            <div class="mws-panel-body">
                <div class="mws-form">
                    <div class="mws-form-inline">
                        <div class="mws-form-row">
                            <div class="validate-message-wrapper">
                                <asp:ValidationSummary ID="vsMaintenance" runat="server" ValidationGroup="vgMaintenance" DisplayMode="BulletList" HeaderText="请更正以下错误:" CssClass="validation-summary-errors" />
                            </div>
                        </div>

                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>借款日期</label>
                                <div class="mws-form-item small">
                                    <telerik:RadDatePicker ID="rdpBorrowDate" Width="200px" runat="server">
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="rdpBorrowDate"
                                        ErrorMessage="借款日期必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                    <telerik:RadToolTip ID="RadToolTip2" runat="server" TargetControlID="rdpBorrowDate" ShowEvent="OnMouseOver"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                </div>

                            </div>
                            <div class="float-left">
                                <label>借款人</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtBorrowName" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="rfvTransportCompanyNumber" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtBorrowName"
                                        ErrorMessage="借款人必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                    <telerik:RadToolTip ID="RadToolTip1" runat="server" TargetControlID="txtBorrowName" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>借款金额</label>
                                <div class="mws-form-item small">
                                    <telerik:RadNumericTextBox ShowSpinButtons="true" MinValue="0" IncrementSettings-InterceptArrowKeys="true" IncrementSettings-InterceptMouseWheel="true"
                                        Label="" runat="server" ID="txtBorrowAmount" Width="160px">
                                    </telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="rfvFee" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtBorrowAmount"
                                        ErrorMessage="借款金额必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                </div>

                            </div>
                            <div class="float-left">

                                <label>归还日期</label>
                                <div class="mws-form-item small">
                                    <telerik:RadDatePicker ID="rdpReturnDate" Width="200px" runat="server">
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="rfvSendDate" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="rdpReturnDate"
                                        ErrorMessage="归还日期必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                    <telerik:RadToolTip ID="rttSendDate" runat="server" TargetControlID="rdpReturnDate" ShowEvent="OnMouseOver"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
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
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>存档文件</label>
                                <div class="mws-form-item small">
                                    <table>
                                        <tr>
                                            <td style="width: 260px;">
                                                <telerik:RadAsyncUpload Culture="ZH-cn" runat="server" ID="radAsyncUpload"
                                                    Width="240px" HideFileInput="false" MultipleFileSelection="Automatic"
                                                    ManualUpload="false"
                                                    OnClientFileUploading="onFileUploading" OnClientValidationFailed="onValidationFailed"
                                                    OnClientFileUploaded="fileUploaded">
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
                        </div>
                        <div class="mws-form-row">
                            <telerik:RadGrid ID="rgAttachmentFiles" runat="server" PageSize="10"
                                AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                OnNeedDataSource="rgAttachmentFiles_NeedDataSource" OnDeleteCommand="rgAttachmentFiles_DeleteCommand"
                                OnItemCreated="rgAttachmentFiles_ItemCreated" OnColumnCreated="rgAttachmentFiles_ColumnCreated"
                                OnItemDataBound="rgAttachmentFiles_ItemDataBound"
                                OnUpdateCommand="rgAttachmentFiles_UpdateCommand">
                                <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                    ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                    <Columns>
                                        <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false">
                                            <ItemStyle HorizontalAlign="Left" Width="50" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="FileName" HeaderText="文件名" DataField="FileName">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>



                                        <telerik:GridButtonColumn Text="下载" UniqueName="Update" CommandName="Update" ButtonType="LinkButton" HeaderStyle-Width="40" ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center" />


                                        <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="40" ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />
                                    </Columns>
                                    <CommandItemTemplate>
                                        <table class="width100-percent">
                                            <tr>
                                                <td>
                                                    <strong style="margin-left: 5px;">借款存档
                                                    </strong>

                                                    <%--<asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
                                                        <input type="button" class="rgAdd" onclick="redirectToMaintenancePage(-1); return false;" />
                                                        <a href="javascript:void(0)" onclick="redirectToMaintenancePage(-1); return false;">添加</a>
                                                    </asp:Panel>--%>
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
                                <ClientSettings>
                                    <ClientEvents OnGridCreated="GetsGridObject" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </div>

                        <div class="height20"></div>
                        <!--支付信息维护 -->
                        <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-payment" runat="server" id="divAppPayments">
                            <div class="mws-panel-header">
                                <span class="mws-i-24 i-creditcard">明细</span>
                            </div>
                            <div class="mws-panel-body">
                                <div class="mws-panel-content">
                                    <telerik:RadGrid ID="rgAppPayments" runat="server" PageSize="10"
                                        AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                        MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true" ShowFooter="false"
                                        ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                        OnNeedDataSource="rgAppPayments_NeedDataSource" OnItemDataBound="rgAppPayments_ItemDataBound"
                                        OnColumnCreated="rgAppPayments_ColumnCreated" OnInsertCommand="rgAppPayments_InsertCommand"
                                        OnUpdateCommand="rgAppPayments_UpdateCommand" OnDeleteCommand="rgAppPayments_DeleteCommand">
                                        <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                            ShowHeadersWhenNoRecords="true" BackColor="#fafafa" EditMode="InPlace">
                                            <CommandItemSettings AddNewRecordText="添加" RefreshText="刷新" />
                                            <Columns>
                                                <telerik:GridTemplateColumn UniqueName="PayDate" HeaderText="收款日期" DataField="PayDate" SortExpression="PayDate">
                                                    <HeaderStyle Width="10%" />
                                                    <ItemStyle Width="10%" />
                                                    <ItemTemplate>
                                                        <span><%# Eval("PayDate","{0:yyyy/MM/dd}") %></span>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <div id="divGridCombox">
                                                            <telerik:RadDatePicker runat="server" ID="rdpPayDate"
                                                                Calendar-EnableShadows="true"
                                                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                                Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                                                Calendar-FirstDayOfWeek="Monday">
                                                            </telerik:RadDatePicker>
                                                        </div>
                                                        <asp:RequiredFieldValidator ID="rfvPayDate" runat="server" ErrorMessage="请选择转账日期" CssClass="field-validation-error"
                                                            ControlToValidate="rdpPayDate"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn UniqueName="PaymentType" HeaderText="类型" DataField="PaymentType"
                                                    SortExpression="PaymentType" FooterText="合计：" FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right">

                                                    <ItemTemplate>
                                                        <span><%# Eval("PaymentType") %></span>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <div id="divGridCombox">
                                                            <telerik:RadComboBox runat="server" ID="rcbxPaymentType" Filter="Contains" AllowCustomText="false"
                                                                MarkFirstMatch="true" Width="100%" EmptyMessage="--请选择--">
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Value="2" Text="借款" />
                                                                    <telerik:RadComboBoxItem Value="1" Text="还款" />
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                        </div>
                                                        <asp:RequiredFieldValidator ID="rfvToPaymentType" runat="server" ErrorMessage="请选择类型" CssClass="field-validation-error"
                                                            ControlToValidate="rcbxPaymentType" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn UniqueName="ToAccount" HeaderText="账号" DataField="ToAccount"
                                                    SortExpression="ToAccount" FooterText="合计：" FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right">

                                                    <ItemTemplate>
                                                        <span><%# Eval("ToAccount") %></span>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <div id="divGridCombox">
                                                            <telerik:RadComboBox runat="server" ID="rcbxToAccount" Filter="Contains" AllowCustomText="false"
                                                                MarkFirstMatch="true" Height="160px" Width="100%" EmptyMessage="--请选择--">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                        <asp:RequiredFieldValidator ID="rfvToAccount" runat="server" ErrorMessage="请选择收款账号" CssClass="field-validation-error"
                                                            ControlToValidate="rcbxToAccount" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="Amount" HeaderText="付款金额" DataField="Amount" SortExpression="Amount">
                                                    <HeaderStyle Width="100px" />
                                                    <ItemStyle Width="100px" />
                                                    <ItemTemplate>
                                                        <span><%# Eval("Amount","{0:C2}") %></span>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <div id="divGridCombox">
                                                            <telerik:RadNumericTextBox runat="server" ID="txtAmount" ShowSpinButtons="true"
                                                                Type="Currency" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator=""
                                                                MinValue="0" MaxValue="999999999" MaxLength="9" Width="100%">
                                                            </telerik:RadNumericTextBox>
                                                        </div>
                                                        <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ErrorMessage="付款金额必填" CssClass="field-validation-error"
                                                            ControlToValidate="txtAmount"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <%-- <telerik:GridTemplateColumn UniqueName="Fee" HeaderText="手续费" DataField="Fee" SortExpression="Fee">
                                                    <HeaderStyle Width="100px" />
                                                    <ItemStyle Width="100px" />
                                                    <ItemTemplate>
                                                        <span><%# Eval("Fee","{0:C2}") %></span>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <div id="divGridCombox">
                                                            <telerik:RadNumericTextBox runat="server" ID="txtFee" ShowSpinButtons="true"
                                                                Type="Currency" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator=""
                                                                MinValue="0" MaxValue="999999999" MaxLength="9" Width="100%">
                                                            </telerik:RadNumericTextBox>
                                                        </div>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>--%>
                                                  <telerik:GridTemplateColumn UniqueName="Comment" HeaderText="备注" DataField="Comment" SortExpression="Comment">
                                                    <HeaderStyle Width="20%" />
                                                    <ItemStyle Width="20%" />
                                                    <ItemTemplate>
                                                        <span><%# Eval("Comment") %></span>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <div id="divGridCombox">
                                                            <telerik:RadTextBox runat="server" ID="txtComment" MaxLength="500" Width="100%"></telerik:RadTextBox>
                                                        </div>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridEditCommandColumn UniqueName="Edit" ButtonType="LinkButton" InsertText="保存" EditText="编辑" UpdateText="更新" CancelText="取消">
                                                    <HeaderStyle Width="10%" />
                                                    <ItemStyle Width="10%" />
                                                </telerik:GridEditCommandColumn>
                                                <%--<telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="60" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />--%>
                                            </Columns>
                                            <NoRecordsTemplate>
                                                没有任何数据
                                            </NoRecordsTemplate>
                                            <ItemStyle Height="30" />
                                            <CommandItemStyle Height="30" />
                                            <AlternatingItemStyle BackColor="#f2f2f2" />
                                            <PagerStyle PagerTextFormat="{4} 第{0}页/共{1}页, 第{2}-{3}条 共{5}条"
                                                PageSizeControlType="RadComboBox" PageSizeLabelText="每页条数:"
                                                FirstPageToolTip="第一页" PrevPageToolTip="上一页" NextPageToolTip="下一页" LastPageToolTip="最后一页" />
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" />
                                    </telerik:RadGrid>
                                 <%--   <div class="float-right" runat="server" id="divPaymentSummary">
                                        <span class="bold">支付总金额</span>：<asp:Label ID="lblTotalPaymentAmount" runat="server"></asp:Label>元&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span class="bold">大写</span>：<asp:Label ID="lblCapitalTotalPaymentAmount" runat="server"></asp:Label>
                                    </div>--%>
                                </div>
                            </div>
                        </div>
                        <div class="height20"></div>
                    </div>
                    <div class="mws-button-row">
                        <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                        <asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="mws-button orange" CausesValidation="false" OnClick="btnDelete_Click" OnClientClick="return onConfirmDelete();" />
                        <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Basics/BorrowMoneyManagement.aspx');return false;" />
                    </div>
                </div>
            </div>
        </div>

    </div>
    <style>
        .RadToolTip .rtWrapper td.rtWrapperContent {
            padding-top: 3px;
            padding-bottom: 3px;
        }
    </style>
    <asp:HiddenField ID="hdnCurrentEntityID" runat="server" Value="-1" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">
        // <![CDATA[
        var gridOfRefresh = null;

        function GetsGridObject(sender, eventArgs) {
            gridOfRefresh = sender;
        }

        function refreshGrid() {
            gridOfRefresh.get_masterTableView().rebind();
        }
        function onClientHidden(sender, args) {
            redirectToPage("Views/Basics/BorrowMoneyManagement.aspx");
        }
        function refreshMaintenancePage(sender, args) {
            var id = "<%=hdnCurrentEntityID.Value%>";
            redirectToPage("Views/Basics/BorrowMoneyMaintenance.aspx?EntityID=" + id);
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


            //regenerateUploadedFiles(fileName);

        }

        // ]]>
    </script>
</asp:Content>
