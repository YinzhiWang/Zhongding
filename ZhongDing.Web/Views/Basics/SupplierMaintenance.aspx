<%@ Page Title="供应商维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SupplierMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.SupplierMaintenance" %>

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
            <telerik:AjaxSetting AjaxControlID="rgProducerCertificates">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgProducerCertificates" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgSupplierCertificates">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgSupplierCertificates" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="container">

        <div class="mws-panel grid_full">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">供应商维护</span>
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
                            <label>供应商编号</label>
                            <div class="mws-form-item small">
                                <telerik:RadTextBox runat="server" ID="txtSupplierCode" CssClass="mws-textinput" Width="40%" Enabled="false"></telerik:RadTextBox>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>供应商名称</label>
                            <div class="mws-form-item">
                                <telerik:RadTextBox runat="server" ID="txtSupplierName" CssClass="mws-textinput" Width="40%" MaxLength="100"></telerik:RadTextBox>
                                <telerik:RadToolTip ID="rttSupplierName" runat="server" TargetControlID="txtSupplierName" ShowEvent="OnClick"
                                    Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                </telerik:RadToolTip>
                                <asp:RequiredFieldValidator ID="rfvSupplierName" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtSupplierName"
                                    ErrorMessage="供应商名称必填" Text="*" CssClass="field-validation-error">
                                </asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvSupplierName" runat="server" ErrorMessage="供应商名称已存在，请使用其他供应商名称"
                                    ControlToValidate="txtSupplierName" ValidationGroup="vgMaintenance" Display="Dynamic"
                                    Text="*" CssClass="field-validation-error" OnServerValidate="cvSupplierName_ServerValidate">
                                </asp:CustomValidator>
                                <telerik:RadButton runat="server" ID="cbxIsProducer" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false" Text="是否生产企业？" OnClientCheckedChanged="onCheckedChanged"></telerik:RadButton>
                            </div>
                        </div>
                        <div class="mws-form-row hide" id="row-FactoryName">
                            <label>生产企业</label>
                            <div class="mws-form-item small">
                                <telerik:RadTextBox runat="server" ID="txtFactoryName" CssClass="mws-textinput" Width="40%" MaxLength="100"></telerik:RadTextBox>
                                <asp:CustomValidator ID="cvFactoryName" runat="server" ErrorMessage="该供应商是生产企业，所以生产企业名必须填写"
                                    ControlToValidate="txtFactoryName" ValidationGroup="vgMaintenance" Display="Dynamic"
                                    Text="*" CssClass="field-validation-error">
                                </asp:CustomValidator>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>联系人</label>
                                <div class="mws-form-item">
                                    <telerik:RadTextBox runat="server" ID="txtContactPerson" CssClass="mws-textinput" MaxLength="100"></telerik:RadTextBox>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>联系电话</label>
                                <div class="mws-form-item">
                                    <telerik:RadTextBox runat="server" ID="txtPhoneNumber" CssClass="mws-textinput" MaxLength="100"></telerik:RadTextBox>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>传真</label>
                            <div class="mws-form-item">
                                <telerik:RadTextBox runat="server" ID="txtFax" CssClass="mws-textinput" MaxLength="100"></telerik:RadTextBox>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>地区</label>
                                <div class="mws-form-item">
                                    <telerik:RadTextBox runat="server" ID="txtDistrict" CssClass="mws-textinput" MaxLength="100"></telerik:RadTextBox>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>邮编</label>
                                <div class="mws-form-item">
                                    <telerik:RadTextBox runat="server" ID="txtPostalCode" CssClass="mws-textinput" MaxLength="100"></telerik:RadTextBox>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>地址</label>
                            <div class="mws-form-item medium">
                                <telerik:RadTextBox runat="server" ID="txtContactAddress" CssClass="mws-textinput" Width="100%" MaxLength="100"></telerik:RadTextBox>
                            </div>
                        </div>
                        <div class="mws-form-row" runat="server" id="divOtherSections">
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-bank-account">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">供应商银行账号</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <telerik:RadGrid ID="rgBankAccounts" runat="server" PageSize="10"
                                            AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="100%" ShowHeader="true"
                                            OnNeedDataSource="rgBankAccounts_NeedDataSource" OnDeleteCommand="rgBankAccounts_DeleteCommand">
                                            <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                                <Columns>
                                                    <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Left" Width="50" />
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
                                                    <telerik:GridTemplateColumn UniqueName="Edit">
                                                        <ItemStyle HorizontalAlign="Center" Width="30" />
                                                        <ItemTemplate>
                                                            <a href="javascript:void(0);" onclick="openBankAccountWindow(<%#DataBinder.Eval(Container.DataItem,"ID")%>)">
                                                                <u>编辑</u></a>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="40" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />
                                                </Columns>
                                                <CommandItemSettings AddNewRecordText="添加" RefreshText="刷新" />

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
                                                                <input type="button" class="rgRefresh" onclick="refreshBankAccountGrid(); return false;" />
                                                                <a href="javascript:void(0);" onclick="refreshBankAccountGrid(); return false;">刷新</a>
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
                                                <ClientEvents OnGridCreated="GetsBankAccountGridObject" />
                                            </ClientSettings>
                                        </telerik:RadGrid>
                                    </div>
                                </div>
                            </div>
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-certificates">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">供应商证照</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">

                                        <telerik:RadTabStrip ID="tabStripCertificates" runat="server" MultiPageID="multiPageCertificates" Skin="Default">
                                            <Tabs>
                                                <telerik:RadTab Text="生产企业" PageViewID="pvProducer" Selected="true"></telerik:RadTab>
                                                <telerik:RadTab Text="供货商" PageViewID="pvSupplier"></telerik:RadTab>
                                            </Tabs>
                                        </telerik:RadTabStrip>
                                        <telerik:RadMultiPage ID="multiPageCertificates" runat="server" CssClass="multi-page-wrapper">
                                            <telerik:RadPageView ID="pvProducer" runat="server" Selected="true">
                                                <%--生产企业证照--%>
                                                <telerik:RadGrid ID="rgProducerCertificates" runat="server" PageSize="10"
                                                    AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                                    MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                                    OnNeedDataSource="rgProducerCertificates_NeedDataSource" OnDeleteCommand="rgProducerCertificates_DeleteCommand">
                                                    <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                        ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                                        <Columns>
                                                            <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                                <ItemStyle HorizontalAlign="Left" Width="50" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="CertificateTypeName" HeaderText="证照类型" DataField="CertificateTypeName">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="GottenDescription" HeaderText="有/无" DataField="GottenDescription">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="EffectiveDateDescription" HeaderText="有效期" DataField="EffectiveDateDescription">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn UniqueName="IsNeedAlert" HeaderText="是否提醒？" DataField="IsNeedAlert" SortExpression="IsNeedAlert">
                                                                <ItemTemplate>
                                                                    <span>
                                                                        <%#DataBinder.Eval(Container.DataItem,"IsNeedAlert")!=null
                                                                        ?(Convert.ToBoolean( DataBinder.Eval(Container.DataItem,"IsNeedAlert").ToString())==true ? "X":
                                                                        string.Empty):string.Empty%>
                                                                    </span>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="AlertBeforeDays" HeaderText="提醒期限" DataField="AlertBeforeDays" SortExpression="AlertBeforeDays">
                                                                <ItemTemplate>
                                                                    <span>
                                                                        <%#DataBinder.Eval(Container.DataItem,"AlertBeforeDays")!=null
                                                                        ?DataBinder.Eval(Container.DataItem,"AlertBeforeDays") + "天"
                                                                        :string.Empty%>
                                                                    </span>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
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
                                                            <telerik:GridTemplateColumn UniqueName="Edit">
                                                                <ItemStyle HorizontalAlign="Center" Width="30" />
                                                                <ItemTemplate>
                                                                    <a href="javascript:void(0);" onclick="openCertificateWindow(<%#DataBinder.Eval(Container.DataItem,"ID")%>, EOwnerTypes.Producer, gridClientIDs.gridProducerCertificates);">
                                                                        <u>编辑</u></a>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="40" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />
                                                        </Columns>
                                                        <CommandItemSettings AddNewRecordText="添加" RefreshText="刷新" />

                                                        <CommandItemTemplate>
                                                            <table class="width100-percent">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
                                                                            <input type="button" class="rgAdd" onclick="openCertificateWindow(-1, EOwnerTypes.Producer, gridClientIDs.gridProducerCertificates); return false;" />
                                                                            <a href="javascript:void(0)" onclick="openCertificateWindow(-1, EOwnerTypes.Producer, gridClientIDs.gridProducerCertificates); return false;">添加</a>
                                                                        </asp:Panel>
                                                                    </td>
                                                                    <td class="right-td rightpadding10">
                                                                        <input type="button" class="rgRefresh" onclick="refreshProducerCertificatesGrid(); return false;" />
                                                                        <a href="javascript:void(0);" onclick="refreshProducerCertificatesGrid(); return false;">刷新</a>
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
                                                        <ClientEvents OnGridCreated="GetsProducerCertificatesGridObject" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                            </telerik:RadPageView>
                                            <telerik:RadPageView ID="pvSupplier" runat="server">
                                                <%--供货商证照--%>
                                                <telerik:RadGrid ID="rgSupplierCertificates" runat="server" PageSize="10"
                                                    AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                                    MasterTableView-PagerStyle-AlwaysVisible="true" Width="99.8%" ShowHeader="true"
                                                    OnNeedDataSource="rgSupplierCertificates_NeedDataSource" OnDeleteCommand="rgSupplierCertificates_DeleteCommand">
                                                    <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                        ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                                        <Columns>
                                                            <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                                <ItemStyle HorizontalAlign="Left" Width="50" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="CertificateTypeName" HeaderText="证照类型" DataField="CertificateTypeName">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="GottenDescription" HeaderText="有/无" DataField="GottenDescription">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="EffectiveDateDescription" HeaderText="有效期" DataField="EffectiveDateDescription">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn UniqueName="IsNeedAlert" HeaderText="是否提醒？" DataField="IsNeedAlert" SortExpression="IsNeedAlert">
                                                                <ItemTemplate>
                                                                    <span>
                                                                        <%#DataBinder.Eval(Container.DataItem,"IsNeedAlert")!=null
                                                                        ?(Convert.ToBoolean( DataBinder.Eval(Container.DataItem,"IsNeedAlert").ToString())==true ? "X":
                                                                        string.Empty):string.Empty%>
                                                                    </span>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="AlertBeforeDays" HeaderText="提醒期限" DataField="AlertBeforeDays" SortExpression="AlertBeforeDays">
                                                                <ItemTemplate>
                                                                    <span>
                                                                        <%#DataBinder.Eval(Container.DataItem,"AlertBeforeDays")!=null
                                                                        ?DataBinder.Eval(Container.DataItem,"AlertBeforeDays") + "天"
                                                                        :string.Empty%>
                                                                    </span>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
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
                                                            <telerik:GridTemplateColumn UniqueName="Edit">
                                                                <ItemStyle HorizontalAlign="Center" Width="30" />
                                                                <ItemTemplate>
                                                                    <a href="javascript:void(0);" onclick="openCertificateWindow(<%#DataBinder.Eval(Container.DataItem,"ID")%>, EOwnerTypes.Supplier, gridClientIDs.gridSupplierCertificates)">
                                                                        <u>编辑</u></a>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="40" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />
                                                        </Columns>
                                                        <CommandItemSettings AddNewRecordText="添加" RefreshText="刷新" />

                                                        <CommandItemTemplate>
                                                            <table class="width100-percent">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
                                                                            <input type="button" class="rgAdd" onclick="openCertificateWindow(-1, EOwnerTypes.Supplier, gridClientIDs.gridSupplierCertificates); return false;" />
                                                                            <a href="javascript:void(0)" onclick="openCertificateWindow(-1, EOwnerTypes.Supplier, gridClientIDs.gridSupplierCertificates); return false;">添加</a>
                                                                        </asp:Panel>
                                                                    </td>
                                                                    <td class="right-td rightpadding10">
                                                                        <input type="button" class="rgRefresh" onclick=" refreshSupplierCertificatesGrid(); return false;" />
                                                                        <a href="javascript:void(0);" onclick="refreshSupplierCertificatesGrid(); return false;">刷新</a>
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
                                                        <ClientEvents OnGridCreated="GetsSupplierCertificatesGridObject" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                            </telerik:RadPageView>
                                        </telerik:RadMultiPage>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="mws-button-row">
                            <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Basics/SupplierManagement.aspx');return false;" />
                        </div>
                    </div>
                </div>
                <asp:HiddenField ID="hdnSupplierID" runat="server" Value="-1" />
            </div>
        </div>
    </div>
    <style>
        .RadTabStrip_Default .rtsLI, .RadTabStrip_Default .rtsLink
        {
            color: #323232;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script src="../../Scripts/WebForms/WebUIValidation.js"></script>

    <script type="text/javascript">
        var supplierID = -1;

        var gridBankAccount = null;
        var gridProducerCertificates = null;
        var gridSupplierCertificates = null;

        var gridClientIDs = {
            gridBankAccount: "<%= rgBankAccounts.ClientID %>",
            gridProducerCertificates: "<%= rgProducerCertificates.ClientID %>",
            gridSupplierCertificates: "<%= rgSupplierCertificates.ClientID %>"
        };

        function GetsBankAccountGridObject(sender, eventArgs) {
            gridBankAccount = sender;
        }

        function refreshBankAccountGrid() {
            gridBankAccount.get_masterTableView().rebind();
        }

        function GetsProducerCertificatesGridObject(sender, eventArgs) {
            gridProducerCertificates = sender;
        }

        function refreshSupplierCertificatesGrid() {
            gridSupplierCertificates.get_masterTableView().rebind();
        }

        function GetsSupplierCertificatesGridObject(sender, eventArgs) {
            gridSupplierCertificates = sender;
        }

        function refreshProducerCertificatesGrid() {
            gridProducerCertificates.get_masterTableView().rebind();
        }


        function redirectToMaintenancePage(id) {
            $.showLoading();
            window.location.href = "BankAccountMaintenance.aspx?BankAccountID=" + id;
        }

        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/Basics/SupplierManagement.aspx");
        }

        function refreshMaintenancePage(sender, args) {

            var supplierID = $("#<%= hdnSupplierID.ClientID %>").val();

            redirectToPage("Views/Basics/SupplierMaintenance.aspx?SupplierID=" + supplierID);
        }

        function onCheckedChanged(e) {
            //debugger;
            var isChecked = e.get_checked();

            var factoryNameRow = $("#row-FactoryName");

            if (factoryNameRow) {
                if (isChecked === true)
                    factoryNameRow.show();
                else
                    factoryNameRow.hide();
            }
        }

        function openBankAccountWindow(id) {
            $.showLoading();

            var targetUrl = $.getRootPath() + "Views/Basics/Editors/SupplierBankAccountMaintain.aspx?EntityID=" + id + "&SupplierID=" + supplierID;

            $.openRadWindow(targetUrl, "winSupplierBankAccount", true, 800, 380);
        }

        function openCertificateWindow(id, ownerTypeID, gridClientID) {
            $.showLoading();

            var targetUrl = $.getRootPath() + "Views/Basics/Editors/CertificateMaintain.aspx?EntityID=" + id
                + "&SupplierID=" + supplierID + "&OwnerTypeID=" + ownerTypeID + "&GridClientID=" + gridClientID;

            $.openRadWindow(targetUrl, "winCertificate", true, 800, 500);
        }

        $(document).ready(function () {

            BrowserDetect.init();

            if (BrowserDetect.browser == "Explorer") {
                if (BrowserDetect.version > 8) //IE8+
                {
                    $(".RadTabStrip_Default .rtsLink").css({ "line-height": "24px" });
                }
                else if (BrowserDetect.version == 8) {
                    $(".RadTabStrip_Default .rtsLink").css({ "line-height": "25px" });
                }
            }


            supplierID = $("#<%= hdnSupplierID.ClientID %>").val();

            var cbxIsProducer = $find("<%= cbxIsProducer.ClientID %>");

            if (cbxIsProducer) {
                var isProducer = cbxIsProducer.get_checked();

                var factoryNameRow = $("#row-FactoryName");

                if (factoryNameRow) {
                    if (isProducer === true)
                        factoryNameRow.show();
                    else
                        factoryNameRow.hide();
                }
            }

        });

    </script>
</asp:Content>
