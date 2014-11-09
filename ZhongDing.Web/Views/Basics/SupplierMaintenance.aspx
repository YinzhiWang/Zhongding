<%@ Page Title="供应商维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SupplierMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.SupplierMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="cbxIsProducer">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divFactoryName" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="divPanelCertificates" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
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
            <telerik:AjaxSetting AjaxControlID="rgContracts">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgContracts" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
        <ClientEvents OnResponseEnd="onResponseEnd" />
    </telerik:RadAjaxManager>

    <div class="container" runat="server" id="divContainer">

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
                                <telerik:RadButton runat="server" ID="cbxIsProducer" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="true" Text="是否生产企业？" OnCheckedChanged="cbxIsProducer_CheckedChanged"></telerik:RadButton>
                            </div>
                        </div>
                        <div class="mws-form-row" runat="server" id="divFactoryName" visible="false">
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
                                    <telerik:RadTextBox runat="server" ID="txtContactPerson" CssClass="mws-textinput" MaxLength="50"></telerik:RadTextBox>
                                    <telerik:RadToolTip ID="rttContactPerson" runat="server" TargetControlID="txtContactPerson" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                        runat="server"
                                        ErrorMessage="联系人必填"
                                        ControlToValidate="txtContactPerson"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>联系电话</label>
                                <div class="mws-form-item">
                                    <telerik:RadTextBox runat="server" ID="txtPhoneNumber" CssClass="mws-textinput" MaxLength="20"></telerik:RadTextBox>
                                    <telerik:RadToolTip ID="rttPhoneNumber" runat="server" TargetControlID="txtPhoneNumber" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                    <asp:RequiredFieldValidator ID="rfvPhoneNumber"
                                        runat="server"
                                        ErrorMessage="联系电话必填"
                                        ControlToValidate="txtPhoneNumber"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revPhoneNumber" runat="server"
                                        ControlToValidate="txtPhoneNumber"
                                        ErrorMessage="联系电话格式不正确！"
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
                            <div class="float-left width40-percent">
                                <label>地区</label>
                                <div class="mws-form-item">
                                    <telerik:RadTextBox runat="server" ID="txtDistrict" CssClass="mws-textinput" MaxLength="50"></telerik:RadTextBox>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>邮编</label>
                                <div class="mws-form-item">
                                    <telerik:RadTextBox runat="server" ID="txtPostalCode" CssClass="mws-textinput" MaxLength="10"></telerik:RadTextBox>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>地址</label>
                            <div class="mws-form-item medium">
                                <telerik:RadTextBox runat="server" ID="txtContactAddress" CssClass="mws-textinput" Width="100%" MaxLength="255"></telerik:RadTextBox>
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
                                                    <telerik:GridTemplateColumn UniqueName="Edit">
                                                        <ItemStyle HorizontalAlign="Center" Width="30" />
                                                        <ItemTemplate>
                                                            <a href="javascript:void(0);" onclick="openBankAccountWindow(<%#DataBinder.Eval(Container.DataItem,"ID")%>)">
                                                                <u>编辑</u></a>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="40" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />
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
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-certificates">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">供应商证照</span>
                                </div>
                                <div class="mws-panel-body" runat="server" id="divPanelCertificates">
                                    <div class="mws-panel-content">

                                        <telerik:RadTabStrip ID="tabStripCertificates" runat="server" MultiPageID="multiPageCertificates" Skin="Default">
                                            <Tabs>
                                                <telerik:RadTab Text="生产企业" Value="tabProducer" PageViewID="pvProducer" Selected="true"></telerik:RadTab>
                                                <telerik:RadTab Text="供货商" Value="tabSupplier" PageViewID="pvSupplier"></telerik:RadTab>
                                            </Tabs>
                                        </telerik:RadTabStrip>
                                        <telerik:RadMultiPage ID="multiPageCertificates" runat="server" CssClass="multi-page-wrapper">
                                            <telerik:RadPageView ID="pvProducer" runat="server" Selected="true">
                                                <%--生产企业证照--%>
                                                <telerik:RadGrid ID="rgProducerCertificates" runat="server" PageSize="10"
                                                    AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                                    MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                                    ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                                    OnNeedDataSource="rgProducerCertificates_NeedDataSource" OnDeleteCommand="rgProducerCertificates_DeleteCommand">
                                                    <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                        ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                                        <Columns>
                                                            <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="CertificateTypeName" HeaderText="证照类型" DataField="CertificateTypeName">
                                                                <ItemStyle HorizontalAlign="Left" Width="25%" />
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
                                                                        <input type="button" class="rgRefresh" onclick="refreshGrid(gridClientIDs.gridProducerCertificates); return false;" />
                                                                        <a href="javascript:void(0);" onclick="refreshGrid(gridClientIDs.gridProducerCertificates); return false;">刷新</a>
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
                                            </telerik:RadPageView>
                                            <telerik:RadPageView ID="pvSupplier" runat="server">
                                                <%--供货商证照--%>
                                                <telerik:RadGrid ID="rgSupplierCertificates" runat="server" PageSize="10"
                                                    AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                                    MasterTableView-PagerStyle-AlwaysVisible="true" Width="99.8%" ShowHeader="true"
                                                    ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                                    OnNeedDataSource="rgSupplierCertificates_NeedDataSource" OnDeleteCommand="rgSupplierCertificates_DeleteCommand">
                                                    <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                        ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                                        <Columns>
                                                            <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="CertificateTypeName" HeaderText="证照类型" DataField="CertificateTypeName">
                                                                <ItemStyle HorizontalAlign="Left" Width="25%" />
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
                                                                        <input type="button" class="rgRefresh" onclick="refreshGrid(gridClientIDs.gridSupplierCertificates); return false;" />
                                                                        <a href="javascript:void(0);" onclick="refreshGrid(gridClientIDs.gridSupplierCertificates); return false;">刷新</a>
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
                                            </telerik:RadPageView>
                                        </telerik:RadMultiPage>
                                    </div>
                                </div>
                            </div>
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-certificates">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">供应商合同</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <telerik:RadGrid ID="rgContracts" runat="server" PageSize="10"
                                            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgContracts_NeedDataSource" OnDeleteCommand="rgContracts_DeleteCommand">
                                            <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                                <Columns>
                                                    <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="ContractCode" HeaderText="合同编号" DataField="ContractCode">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="SupplierName" HeaderText="供应商" DataField="SupplierName">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="ProductSpecification" HeaderText="规格" DataField="ProductSpecification">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="UnitPrice" HeaderText="单价" DataField="UnitPrice" DataFormatString="{0:C2}">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="ExpirationDate" HeaderText="合同终止日期" DataField="ExpirationDate" DataFormatString="{0:yyyy/MM/dd}">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Edit">
                                                        <ItemStyle HorizontalAlign="Center" Width="30" />
                                                        <ItemTemplate>
                                                            <a href="javascript:void(0);" onclick="openContractWindow(<%#DataBinder.Eval(Container.DataItem,"ID")%>)">
                                                                <u>编辑</u></a>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="40" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />
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
                                                                <input type="button" class="rgRefresh" onclick="refreshGrid(gridClientIDs.gridContracts); return false;" />
                                                                <a href="javascript:void(0);" onclick="refreshGrid(gridClientIDs.gridContracts); return false;">刷新</a>
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
                            <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Basics/SupplierManagement.aspx');return false;" />
                        </div>
                    </div>
                </div>
                <asp:HiddenField ID="hdnSupplierID" runat="server" Value="-1" />
            </div>
        </div>

        <style type="text/css">
            .RadTabStrip_Default .rtsLI, .RadTabStrip_Default .rtsLink
            {
                color: #323232;
            }
        </style>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script src="../../Scripts/WebForms/WebUIValidation.js"></script>

    <script type="text/javascript">
        var supplierID = -1;

        var gridClientIDs = {
            gridBankAccounts: "<%= rgBankAccounts.ClientID %>",
            gridProducerCertificates: "<%= rgProducerCertificates.ClientID %>",
            gridSupplierCertificates: "<%= rgSupplierCertificates.ClientID %>",
            gridContracts: "<%= rgContracts.ClientID %>",
        };

        function refreshGrid(gridClientID) {
            var gridObj = $find(gridClientID);

            if (gridObj)
                gridObj.get_masterTableView().rebind();
        }

        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/Basics/SupplierManagement.aspx");
        }

        function refreshMaintenancePage(sender, args) {

            var supplierID = $("#<%= hdnSupplierID.ClientID %>").val();

            redirectToPage("Views/Basics/SupplierMaintenance.aspx?SupplierID=" + supplierID);
        }

        function openBankAccountWindow(id) {
            $.showLoading();

            var targetUrl = $.getRootPath() + "Views/Basics/Editors/SupplierBankAccountMaintain.aspx?EntityID=" + id
                + "&SupplierID=" + supplierID + "&GridClientID=" + gridClientIDs.gridBankAccounts;

            $.openRadWindow(targetUrl, "winSupplierBankAccount", true, 800, 380);
        }

        function openCertificateWindow(id, ownerTypeID, gridClientID) {
            $.showLoading();

            var targetUrl = $.getRootPath() + "Views/Basics/Editors/CertificateMaintain.aspx?EntityID=" + id
                + "&SupplierID=" + supplierID + "&OwnerTypeID=" + ownerTypeID + "&GridClientID=" + gridClientID;

            $.openRadWindow(targetUrl, "winCertificate", true, 800, 400);
        }

        function openContractWindow(id) {
            $.showLoading();

            var targetUrl = $.getRootPath() + "Views/Basics/Editors/SupplierContractMaintain.aspx?EntityID=" + id
                + "&SupplierID=" + supplierID + "&GridClientID=" + gridClientIDs.gridContracts;

            $.openRadWindow(targetUrl, "winSupplierContract", true, 1000, 600);
        }

        function onResponseEnd(sender, args) {

            var eventTarget = args.get_eventTarget();

            if (eventTarget && eventTarget.indexOf("cbxIsProducer") >= 0) {
                resetTabStripCss();
            }
        }

        function resetTabStripCss() {
            if (BrowserDetect.browser == "Explorer") {
                if (BrowserDetect.version > 8) //IE8+
                {
                    $(".RadTabStrip_Default .rtsLink").css({ "line-height": "24px" });
                }
                else if (BrowserDetect.version == 8) {
                    $(".RadTabStrip_Default .rtsLink").css({ "line-height": "25px" });
                }
            }
        }

        $(document).ready(function () {

            BrowserDetect.init();

            resetTabStripCss();

            supplierID = $("#<%= hdnSupplierID.ClientID %>").val();

        });

    </script>
</asp:Content>
