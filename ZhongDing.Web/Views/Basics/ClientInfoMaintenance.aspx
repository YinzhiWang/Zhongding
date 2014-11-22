<%@ Page Title="客户维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientInfoMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.ClientInfoMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rgBankAccounts">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgBankAccounts" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgContacts">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgContacts" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="container">

        <div class="mws-panel grid_full">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">客户维护</span>
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
                            <label>客户编号</label>
                            <div class="mws-form-item small">
                                <telerik:RadTextBox runat="server" ID="txtClientCode" CssClass="mws-textinput" Width="40%" Enabled="false"></telerik:RadTextBox>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>客户名称</label>
                            <div class="mws-form-item">
                                <telerik:RadComboBox runat="server" ID="rcbxClientUser" Filter="Contains" AllowCustomText="true"
                                    MarkFirstMatch="true" Height="160px" EmptyMessage="--请选择--" OnClientBlur="onClientBlur">
                                </telerik:RadComboBox>
                                <telerik:RadToolTip ID="rttClientName" runat="server" TargetControlID="rcbxClientUser" ShowEvent="OnClick"
                                    Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                </telerik:RadToolTip>
                                <asp:CustomValidator ID="cvClientName" runat="server" ErrorMessage="请选择或输入客户名称"
                                    ControlToValidate="rcbxClientUser" ValidationGroup="vgMaintenance" Display="Dynamic"
                                    Text="*" CssClass="field-validation-error">
                                </asp:CustomValidator>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>商业单位</label>
                            <div class="mws-form-item">
                                <telerik:RadComboBox runat="server" ID="rcbxClientCompany" Filter="Contains"
                                    AllowCustomText="false" MarkFirstMatch="true" Height="160px" Width="60%" EmptyMessage="--请选择--">
                                </telerik:RadComboBox>
                                <telerik:RadToolTip ID="rttClientCompany" runat="server" TargetControlID="rcbxClientCompany" ShowEvent="OnClick"
                                    Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                </telerik:RadToolTip>
                                <asp:RequiredFieldValidator ID="rfvClientCompany"
                                    runat="server"
                                    ErrorMessage="请选择商业单位"
                                    ControlToValidate="rcbxClientCompany"
                                    Display="Dynamic" CssClass="field-validation-error"
                                    ValidationGroup="vgMaintenance" Text="*">
                                </asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvClientCompany" runat="server" ErrorMessage="商业单位不存在，请重新选择"
                                    ControlToValidate="rcbxClientCompany" ValidationGroup="vgMaintenance" Display="Dynamic"
                                    Text="*" CssClass="field-validation-error" OnServerValidate="cvClientCompany_ServerValidate">
                                </asp:CustomValidator>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>收货人</label>
                                <div class="mws-form-item">
                                    <telerik:RadTextBox runat="server" ID="txtReceiverName" CssClass="mws-textinput" MaxLength="50"></telerik:RadTextBox>
                                    <telerik:RadToolTip ID="rttReceiverName" runat="server" TargetControlID="txtReceiverName" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                    <asp:RequiredFieldValidator ID="rfvReceiverName"
                                        runat="server"
                                        ErrorMessage="收货人必填"
                                        ControlToValidate="txtReceiverName"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>收货电话</label>
                                <div class="mws-form-item">
                                    <telerik:RadTextBox runat="server" ID="txtPhoneNumber" CssClass="mws-textinput" MaxLength="20"></telerik:RadTextBox>
                                    <telerik:RadToolTip ID="rttPhoneNumber" runat="server" TargetControlID="txtPhoneNumber" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                    <asp:RequiredFieldValidator ID="rfvPhoneNumber"
                                        runat="server"
                                        ErrorMessage="收货电话必填"
                                        ControlToValidate="txtPhoneNumber"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revPhoneNumber" runat="server"
                                        ControlToValidate="txtPhoneNumber"
                                        ErrorMessage="收货电话格式不正确！"
                                        ValidationExpression="(\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$"
                                        CssClass="field-validation-error" Display="Dynamic"
                                        ValidationGroup="vgMaintenance" Text="*"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>传真</label>
                            <div class="mws-form-item">
                                <telerik:RadTextBox runat="server" ID="txtFax" CssClass="mws-textinput" MaxLength="20"></telerik:RadTextBox>
                                <asp:RegularExpressionValidator ID="revFax" runat="server"
                                    ControlToValidate="txtFax"
                                    ErrorMessage="传真格式不正确"
                                    ValidationExpression="(\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$"
                                    CssClass="field-validation-error" Display="Dynamic"
                                    ValidationGroup="vgMaintenance" Text="*"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>收货地址</label>
                            <div class="mws-form-item medium">
                                <telerik:RadTextBox runat="server" ID="txtReceiverAddress" CssClass="mws-textinput" Width="100%" MaxLength="255"></telerik:RadTextBox>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>发票配送地址</label>
                            <div class="mws-form-item medium">
                                <telerik:RadTextBox runat="server" ID="txtReceiptAddress" CssClass="mws-textinput" Width="100%" MaxLength="255"></telerik:RadTextBox>
                            </div>
                        </div>

                        <div class="mws-form-row" runat="server" id="divOtherSections">
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-bank-account">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">银行账号管理</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <telerik:RadGrid ID="rgBankAccounts" runat="server" PageSize="10"
                                            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgBankAccounts_NeedDataSource" OnDeleteCommand="rgBankAccounts_DeleteCommand">
                                            <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                                <Columns>
                                                    <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="AccountName" HeaderText="户名" DataField="AccountName">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="BankBranchName" HeaderText="开户行" DataField="BankBranchName">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="Account" HeaderText="帐号" DataField="Account">
                                                        <ItemStyle HorizontalAlign="Left" Width="160" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Comment" HeaderText="备注" DataField="Comment" SortExpression="Comment">
                                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                        <ItemTemplate>
                                                            <span title="<%#DataBinder.Eval(Container.DataItem,"Comment")%>">
                                                                <%#DataBinder.Eval(Container.DataItem,"Comment")!=null
                                                                ?DataBinder.Eval(Container.DataItem,"Comment").ToString().CutString(12)
                                                                :string.Empty%>
                                                            </span>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Edit" HeaderStyle-Width="40">
                                                        <ItemStyle HorizontalAlign="Center" Width="40" />
                                                        <ItemTemplate>
                                                            <a href="javascript:void(0);" onclick="openBankAccountWindow(<%#DataBinder.Eval(Container.DataItem,"ID")%>)">
                                                                <u>编辑</u></a>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="40" ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />
                                                </Columns>
                                                <CommandItemTemplate>
                                                    <table class="width100-percent">
                                                        <tr>
                                                            <td>
                                                                <asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
                                                                    <input type="button" class="rgAdd" onclick="openBankAccountWindow(-1); return false;" />
                                                                    <a href="javascript:void(0)" onclick="openBankAccountWindow(-1); return false;">添加</a>
                                                                </asp:Panel>
                                                            </td>
                                                            <td class="right-td rightpadding10">
                                                                <input type="button" class="rgRefresh" onclick="refreshGrid(gridClientIDs.gridBankAccounts); return false;" />
                                                                <a href="javascript:void(0);" onclick="refreshGrid(gridClientIDs.gridBankAccounts); return false;">刷新</a>
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
                                        </telerik:RadGrid>
                                    </div>
                                </div>
                            </div>

                            <!--联系人管理 -->
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-contact">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">联系人管理</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <telerik:RadGrid ID="rgContacts" runat="server" PageSize="10"
                                            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgContacts_NeedDataSource" OnDeleteCommand="rgContacts_DeleteCommand">
                                            <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                                <Columns>
                                                    <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="ContactName" HeaderText="联系人" DataField="ContactName">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="PhoneNumber" HeaderText="联系电话" DataField="PhoneNumber">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="Address" HeaderText="地址" DataField="Address">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Comment" HeaderText="备注" DataField="Comment" SortExpression="Comment">
                                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                        <ItemTemplate>
                                                            <span title="<%#DataBinder.Eval(Container.DataItem,"Comment")%>">
                                                                <%#DataBinder.Eval(Container.DataItem,"Comment")!=null
                                                                ?DataBinder.Eval(Container.DataItem,"Comment").ToString().CutString(12)
                                                                :string.Empty%>
                                                            </span>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Edit" HeaderStyle-Width="40">
                                                        <ItemStyle HorizontalAlign="Center" Width="40" />
                                                        <ItemTemplate>
                                                            <a href="javascript:void(0);" onclick="openContractWindow(<%#DataBinder.Eval(Container.DataItem,"ID")%>)">
                                                                <u>编辑</u></a>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="40" ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />
                                                </Columns>
                                                <CommandItemTemplate>
                                                    <table class="width100-percent">
                                                        <tr>
                                                            <td>
                                                                <asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
                                                                    <input type="button" class="rgAdd" onclick="openContractWindow(-1); return false;" />
                                                                    <a href="javascript:void(0)" onclick="openContractWindow(-1); return false;">添加</a>
                                                                </asp:Panel>
                                                            </td>
                                                            <td class="right-td rightpadding10">
                                                                <input type="button" class="rgRefresh" onclick="refreshGrid(gridClientIDs.gridContacts); return false;" />
                                                                <a href="javascript:void(0);" onclick="refreshGrid(gridClientIDs.gridContacts); return false;">刷新</a>
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
                                        </telerik:RadGrid>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="mws-button-row">
                            <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                            <asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="mws-button orange" CausesValidation="false" OnClick="btnDelete_Click" OnClientClick="return onConfirmDelete();" />
                            <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Basics/ClientInfoManagement.aspx');return false;" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnCurrentEntityID" runat="server" Value="-1" />
    <asp:HiddenField ID="hdnCustomClientName" runat="server" Value="" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">

        var currentEntityID = -1;

        var gridClientIDs = {
            gridBankAccounts: "<%= rgBankAccounts.ClientID %>",
            gridContacts: "<%= rgContacts.ClientID %>",
        };

        function refreshGrid(gridClientID) {
            var gridObj = $find(gridClientID);

            if (gridObj)
                gridObj.get_masterTableView().rebind();
        }

        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/Basics/ClientInfoManagement.aspx");
        }

        function refreshMaintenancePage(sender, args) {

            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            redirectToPage("Views/Basics/ClientInfoMaintenance.aspx?EntityID=" + currentEntityID);
        }

        function openBankAccountWindow(id) {
            $.showLoading();

            var targetUrl = $.getRootPath() + "Views/Basics/Editors/BankAccountMaintain.aspx?EntityID=" + id
                + "&OwnerTypeID=" + EOwnerTypes.Client + "&OwnerEntityID=" + currentEntityID + "&GridClientID=" + gridClientIDs.gridBankAccounts;

            $.openRadWindow(targetUrl, "winBankAccount", true, 800, 380);
        }

        function openContractWindow(id) {
            $.showLoading();

            var targetUrl = $.getRootPath() + "Views/Basics/Editors/ClientInfoContactMaintain.aspx?EntityID=" + id
                + "&OwnerEntityID=" + currentEntityID + "&GridClientID=" + gridClientIDs.gridContacts;

            $.openRadWindow(targetUrl, "winContract", true, 800, 380);
        }

        function onClientBlur(sender, args) {

            var hdnCustomClientName = $("#<%= hdnCustomClientName.ClientID %>");
            hdnCustomClientName.val(sender.get_text());
        }

        $(document).ready(function () {
            currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();
        });

    </script>
</asp:Content>
