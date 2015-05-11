<%@ Page Title="客户订单维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientSaleAppMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Sales.ClientSaleAppMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbxClientUser">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divClientCompanies" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="divClientContacts" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="lblReceiverName" />
                    <telerik:AjaxUpdatedControl ControlID="lblReceiverPhone" />
                    <telerik:AjaxUpdatedControl ControlID="lblReceiverAddress" />
                    <telerik:AjaxUpdatedControl ControlID="lblReceiverFax" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlClientCompany">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divClientContacts" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="lblReceiverName" />
                    <telerik:AjaxUpdatedControl ControlID="lblReceiverPhone" />
                    <telerik:AjaxUpdatedControl ControlID="lblReceiverAddress" />
                    <telerik:AjaxUpdatedControl ControlID="lblReceiverFax" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgAppNotes">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgAppNotes" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgAuditNotes">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgAuditNotes" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgOrderProducts">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgOrderProducts" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgAppPayments">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgAppPayments" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="container">
        <div class="mws-panel grid_full">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">客户订单维护</span>
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
                                <label>订单编号</label>
                                <div class="mws-form-item toppadding5">
                                    <telerik:RadTextBox runat="server" ID="txtOrderCode" CssClass="mws-textinput"
                                        Width="80%" Enabled="false">
                                    </telerik:RadTextBox>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>操作人</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblCreateBy" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>订单日期</label>
                                <div class="mws-form-item toppadding5">
                                    <telerik:RadDatePicker runat="server" ID="rdpOrderDate"
                                        Calendar-EnableShadows="true"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                        Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                        Calendar-FastNavigationSettings-DisableOutOfRangeMonths="true"
                                        Calendar-FirstDayOfWeek="Monday">
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="rfvOrderDate" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="rdpOrderDate"
                                        ErrorMessage="订单日期必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>订单类型</label>
                                <div class="mws-form-item small toppadding5">
                                    <asp:Label ID="lblSalesOrderType" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>客户</label>
                                <div class="mws-form-item">
                                    <telerik:RadComboBox runat="server" ID="rcbxClientUser" Filter="Contains" AllowCustomText="false"
                                        MarkFirstMatch="true" Height="160px" EmptyMessage="--请选择--"
                                        AutoPostBack="true" OnSelectedIndexChanged="rcbxClientUser_SelectedIndexChanged">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="rfvClientUser"
                                        runat="server"
                                        ErrorMessage="请选择客户"
                                        ControlToValidate="rcbxClientUser"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="float-left width60-percent">
                                <label>商业单位</label>
                                <div class="mws-form-item" runat="server" id="divClientCompanies">
                                    <telerik:RadDropDownList runat="server" ID="ddlClientCompany" Width="360"
                                        DefaultMessage="--请选择--" AutoPostBack="true" OnSelectedIndexChanged="ddlClientCompany_SelectedIndexChanged">
                                    </telerik:RadDropDownList>
                                    <asp:RequiredFieldValidator ID="rfvClientCompany"
                                        runat="server"
                                        ErrorMessage="请选择商业单位"
                                        ControlToValidate="ddlClientCompany"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>客户余额</label>
                                <div class="mws-form-item  small toppadding5">
                                    <asp:Label ID="lblClientBalanceAmount" runat="server" Text="0.0"></asp:Label>元
                                </div>
                            </div>
                            <div class="float-left" runat="server" id="divStop">
                                <label>中止执行</label>
                                <div class="mws-form-item small toppadding5">
                                    <telerik:RadButton runat="server" ID="cbxIsStop" ButtonType="ToggleButton"
                                        ToggleType="CheckBox" AutoPostBack="false" Enabled="false">
                                    </telerik:RadButton>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row" id="divDeliveryMode">
                            <label>发货模式</label>
                            <div class="mws-form-item">
                                <telerik:RadDropDownList runat="server" ID="ddlDeliveryMode" DefaultMessage="--请选择--"
                                    OnClientSelectedIndexChanged="onClientSelectedDeliveryMode">
                                </telerik:RadDropDownList>
                            </div>
                        </div>
                        <div class="mws-form-row" id="divReceivingBankAccount">
                            <label>收款账号</label>
                            <div class="mws-form-item medium">
                                <telerik:RadComboBox runat="server" ID="rcbxReceivingBankAccount" Filter="Contains" AllowCustomText="false"
                                    MarkFirstMatch="true" Height="160px" Width="60%" EmptyMessage="--请选择--"
                                    CheckBoxes="true" EnableCheckAllItemsCheckBox="true">
                                    <Localization CheckAllString="全选" AllItemsCheckedString="已全选" ItemsCheckedString="项已选择" />
                                </telerik:RadComboBox>
                                <asp:CustomValidator ID="cvReceivingBankAccount" runat="server" ErrorMessage="请选择收款账号"
                                    ControlToValidate="rcbxReceivingBankAccount" ValidationGroup="vgMaintenance" Display="Dynamic"
                                    Text="*" CssClass="field-validation-error">
                                </asp:CustomValidator>
                                <table runat="server" id="tblReceivingBankAccounts" visible="false"></table>
                            </div>
                        </div>
                        <div class="mws-form-row" id="divGuaranteeInfo">
                            <div class="float-left width40-percent">
                                <label>担保有效期</label>
                                <div class="mws-form-item">
                                    <telerik:RadDatePicker runat="server" ID="rdpGuaranteeExpiration"
                                        Calendar-EnableShadows="true"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="今天"
                                        Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="日期超出范围"
                                        Calendar-FastNavigationSettings-DisableOutOfRangeMonths="true"
                                        Calendar-FirstDayOfWeek="Monday">
                                    </telerik:RadDatePicker>
                                    <asp:CustomValidator ID="cvGuaranteeExpiration" runat="server" ErrorMessage="请选择担保有效期"
                                        ControlToValidate="rdpGuaranteeExpiration" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>担保业务员</label>
                                <div class="mws-form-item small">
                                    <telerik:RadComboBox runat="server" ID="rcbxGuaranteeby" Filter="Contains" AllowCustomText="false"
                                        MarkFirstMatch="true" Height="160px" EmptyMessage="--请选择--">
                                    </telerik:RadComboBox>
                                    <asp:CustomValidator ID="cvGuaranteeby" runat="server" ErrorMessage="请选择担保业务员"
                                        ControlToValidate="rcbxGuaranteeby" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>联系人</label>
                                <div class="mws-form-item" runat="server" id="divClientContacts">
                                    <telerik:RadComboBox runat="server" ID="rcbxClientContact" Filter="Contains" AllowCustomText="false"
                                        MarkFirstMatch="true" Height="160px" EmptyMessage="--请选择--"
                                        OnItemDataBound="rcbxClientContact_ItemDataBound" OnClientSelectedIndexChanged="onClientSelectedClientContact">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                        runat="server"
                                        ErrorMessage="请选择联系人"
                                        ControlToValidate="rcbxClientContact"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>联系电话</label>
                                <div class="mws-form-item small toppadding5">
                                    <asp:Label ID="lblContactPhone" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>收货人</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblReceiverName" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>收货电话</label>
                                <div class="mws-form-item small toppadding5">
                                    <asp:Label ID="lblReceiverPhone" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>收货地址</label>
                            <div class="mws-form-item medium toppadding5">
                                <asp:Label ID="lblReceiverAddress" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>传真</label>
                            <div class="mws-form-item small toppadding5">
                                <asp:Label ID="lblReceiverFax" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="mws-form-row" runat="server" id="divComment">
                            <label>备注</label>
                            <div class="mws-form-item medium">
                                <telerik:RadTextBox runat="server" ID="txtComment" Width="90%" MaxLength="1000"
                                    TextMode="MultiLine" Height="80">
                                </telerik:RadTextBox>
                            </div>
                        </div>
                        <div class="mws-form-row" runat="server" id="divComments">
                            <label>备注历史</label>
                            <div class="mws-form-item medium">
                                <telerik:RadDockLayout runat="server" ID="RadDockLayout1">
                                    <telerik:RadDockZone runat="server" ID="RadDockZone1" Orientation="Vertical"
                                        Width="99%" FitDocks="true" BorderStyle="None">
                                        <telerik:RadDock ID="RadDock1" Title="备注历史记录" runat="server" AllowedZones="RadDockZone1" Font-Size="12px"
                                            DefaultCommands="ExpandCollapse" EnableAnimation="true" EnableDrag="false"
                                            DockMode="Docked" ExpandText="展开" CollapseText="折叠">
                                            <ContentTemplate>
                                                <div class="toppadding10"></div>
                                                <telerik:RadGrid ID="rgAppNotes" runat="server"
                                                    AllowPaging="false" AllowSorting="true" AutoGenerateColumns="false" Skin="Silk" Width="99.5%"
                                                    ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                                    OnNeedDataSource="rgAppNotes_NeedDataSource">
                                                    <MasterTableView Width="100%" DataKeyNames="ID" ShowHeader="true" BackColor="#fafafa">
                                                        <Columns>
                                                            <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="CreatedOn" HeaderText="创建时间" DataField="CreatedOn" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}">
                                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="CreatedBy" HeaderText="创建人" DataField="CreatedBy">
                                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="Note" HeaderText="备注内容" DataField="Note">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                        <NoRecordsTemplate>
                                                            没有任何数据
                                                        </NoRecordsTemplate>
                                                        <ItemStyle Height="30" />
                                                        <AlternatingItemStyle BackColor="#f2f2f2" />
                                                    </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true" />
                                                </telerik:RadGrid>
                                            </ContentTemplate>
                                        </telerik:RadDock>
                                    </telerik:RadDockZone>
                                </telerik:RadDockLayout>
                            </div>
                        </div>
                        <div class="height20"></div>
                        <div class="mws-form-row" runat="server" id="divOtherSections">
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-procure-order">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">货品维护</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <telerik:RadGrid ID="rgOrderProducts" runat="server" PageSize="10"
                                            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true" ShowFooter="true"
                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgOrderProducts_NeedDataSource" OnDeleteCommand="rgOrderProducts_DeleteCommand"
                                            OnItemCreated="rgOrderProducts_ItemCreated" OnColumnCreated="rgOrderProducts_ColumnCreated"
                                            OnItemDataBound="rgOrderProducts_ItemDataBound">
                                            <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                                <Columns>
                                                    <telerik:GridBoundColumn UniqueName="Warehouse" HeaderText="出库仓库" DataField="Warehouse">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="UnitOfMeasurement" HeaderText="基本单位" DataField="UnitOfMeasurement">
                                                        <ItemStyle HorizontalAlign="Left" Width="60" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="SalesQty" HeaderText="基本数量" DataField="SalesQty">
                                                        <ItemStyle HorizontalAlign="Left" Width="60" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="NumberOfPackages" HeaderText="件数" DataField="NumberOfPackages">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="SalesPrice" HeaderText="单价" DataField="SalesPrice" DataFormatString="{0:C2}">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="InvoicePrice" HeaderText="挂靠单价" DataField="InvoicePrice" DataFormatString="{0:C2}">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="GiftCount" HeaderText="赠送数量" DataField="GiftCount" FooterText="合计："
                                                        FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="TotalSalesAmount" HeaderText="货款" DataField="TotalSalesAmount" DataFormatString="{0:C2}"
                                                        Aggregate="Sum" FooterStyle-Font-Bold="true">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Edit" HeaderStyle-Width="60">
                                                        <ItemStyle HorizontalAlign="Center" Width="60" />
                                                        <ItemTemplate>
                                                            <a href="javascript:void(0);" onclick="openOrderProductWindow(<%#DataBinder.Eval(Container.DataItem,"ID")%>)">
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
                                                                    <input type="button" class="rgAdd" onclick="openOrderProductWindow(-1); return false;" />
                                                                    <a href="javascript:void(0)" onclick="openOrderProductWindow(-1); return false;">添加</a>
                                                                </asp:Panel>
                                                            </td>
                                                            <td class="right-td rightpadding10">
                                                                <input type="button" class="rgRefresh" onclick="refreshGrid(gridClientIDs.gridOrderProducts); return false;" />
                                                                <a href="javascript:void(0);" onclick="refreshGrid(gridClientIDs.gridOrderProducts); return false;">刷新</a>
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
                                            </ClientSettings>
                                        </telerik:RadGrid>
                                    </div>
                                </div>
                            </div>

                            <!--收款信息维护 -->
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-payment" runat="server" id="divAppPayments">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">确认收款</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <telerik:RadGrid ID="rgAppPayments" runat="server" PageSize="10"
                                            AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true" ShowFooter="true"
                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgAppPayments_NeedDataSource" OnItemCreated="rgAppPayments_ItemCreated"
                                            OnColumnCreated="rgAppPayments_ColumnCreated" OnDeleteCommand="rgAppPayments_DeleteCommand">
                                            <MasterTableView Width="100%" DataKeyNames="ID,PaymentMethodID" CommandItemDisplay="Top"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                                <Columns>
                                                    <telerik:GridBoundColumn UniqueName="PaymentMethod" HeaderText="收款方式" DataField="PaymentMethod">
                                                        <HeaderStyle Width="15%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="Amount" HeaderText="金额" DataField="Amount" DataFormatString="{0:C2}">
                                                        <HeaderStyle Width="15%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="Fee" HeaderText="手续费" DataField="Fee" DataFormatString="{0:C2}">
                                                        <HeaderStyle Width="15%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="PayDate" HeaderText="到账日期" DataField="PayDate">
                                                        <HeaderStyle Width="15%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="Comment" HeaderText="备注" DataField="Comment">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Edit" HeaderStyle-Width="60">
                                                        <ItemStyle HorizontalAlign="Center" Width="60" />
                                                        <ItemTemplate>
                                                            <a href="javascript:void(0);" onclick="openAppPaymentWindow(<%#DataBinder.Eval(Container.DataItem,"ID")%>, <%#DataBinder.Eval(Container.DataItem,"PaymentMethodID")%>)">
                                                                <u>编辑</u></a>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridButtonColumn Text="删除" UniqueName="Delete" CommandName="Delete" ButtonType="LinkButton" HeaderStyle-Width="60" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center" ConfirmText="确认删除该条数据吗？" />
                                                </Columns>
                                                <CommandItemTemplate>
                                                    <table class="width100-percent">
                                                        <tr>
                                                            <td>
                                                                <asp:Panel ID="plAddCommand" runat="server" CssClass="width60 float-left">
                                                                    <input type="button" class="rgAdd" onclick="openAppPaymentWindow(-1); return false;" />
                                                                    <a href="javascript:void(0)" onclick="openAppPaymentWindow(-1); return false;">添加</a>
                                                                </asp:Panel>
                                                            </td>
                                                            <td class="right-td rightpadding10">
                                                                <input type="button" class="rgRefresh" onclick="refreshGrid(gridClientIDs.gridAppPayments); return false;" />
                                                                <a href="javascript:void(0);" onclick="refreshGrid(gridClientIDs.gridAppPayments); return false;">刷新</a>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </CommandItemTemplate>
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
                                    </div>
                                </div>
                            </div>

                            <!--审核 -->
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-procure-audit">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">审核</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <telerik:RadDockLayout runat="server" ID="RadDockLayout2">
                                            <telerik:RadDockZone runat="server" ID="RadDockZone2" Orientation="Vertical"
                                                Width="99%" FitDocks="true" BorderStyle="None">
                                                <telerik:RadDock ID="RadDock2" Title="审核历史记录" runat="server" AllowedZones="RadDockZone1" Font-Size="12px"
                                                    DefaultCommands="ExpandCollapse" EnableAnimation="true" EnableDrag="false"
                                                    DockMode="Docked" ExpandText="展开" CollapseText="折叠">
                                                    <ContentTemplate>
                                                        <div class="toppadding10"></div>
                                                        <telerik:RadGrid ID="rgAuditNotes" runat="server"
                                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                                            AllowPaging="false" AllowSorting="true" AutoGenerateColumns="false" Skin="Silk" Width="99.5%"
                                                            OnNeedDataSource="rgAuditNotes_NeedDataSource">
                                                            <MasterTableView Width="100%" DataKeyNames="ID" ShowHeader="true" BackColor="#fafafa">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn UniqueName="CreatedOn" HeaderText="创建时间" DataField="CreatedOn" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}">
                                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn UniqueName="CreatedBy" HeaderText="创建人" DataField="CreatedBy">
                                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn UniqueName="Note" HeaderText="审核意见" DataField="Note">
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </telerik:GridBoundColumn>
                                                                </Columns>
                                                                <NoRecordsTemplate>
                                                                    没有任何数据
                                                                </NoRecordsTemplate>
                                                                <ItemStyle Height="30" />
                                                                <AlternatingItemStyle BackColor="#f2f2f2" />
                                                            </MasterTableView>
                                                            <ClientSettings EnableRowHoverStyle="true" />
                                                        </telerik:RadGrid>
                                                    </ContentTemplate>
                                                </telerik:RadDock>
                                            </telerik:RadDockZone>
                                        </telerik:RadDockLayout>

                                        <div class="mws-form-row" runat="server" id="divAudit">
                                            <label>审核意见</label>
                                            <div class="mws-form-item large">
                                                <telerik:RadTextBox runat="server" ID="txtAuditComment" Width="90%" MaxLength="1000"
                                                    TextMode="MultiLine" Height="80">
                                                </telerik:RadTextBox>
                                                <asp:CustomValidator ID="cvAuditComment" runat="server" Display="Dynamic" ErrorMessage="审核意见必填"
                                                    ControlToValidate="txtAuditComment" ValidationGroup="vgMaintenance" Text="*" CssClass="field-validation-error">
                                                </asp:CustomValidator>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="mws-button-row">
                            <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                            <asp:Button ID="btnSubmit" runat="server" Text="提交" CssClass="mws-button green" CausesValidation="true" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnAudit" runat="server" Text="审核通过" CssClass="mws-button green" CausesValidation="true" Visible="false" OnClick="btnAudit_Click" />
                            <asp:Button ID="btnReturn" runat="server" Text="退回" CssClass="mws-button orange" CausesValidation="true" Visible="false" OnClick="btnReturn_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Sales/ClientSaleAppManagement.aspx');return false;" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnCurrentEntityID" runat="server" Value="-1" />
    <asp:HiddenField ID="hdnSaleOrderTypeID" runat="server" Value="-1" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <style>
        div#divGridCombox td, td:first-child {
            border-left-style: solid;
        }
    </style>

    <script type="text/javascript">

        var currentEntityID = -1;

        var gridClientIDs = {
            gridOrderProducts: "<%= rgOrderProducts.ClientID %>",
            gridAppPayments: "<%= rgAppPayments.ClientID %>",
        };

        var deliveryModes = {
            ReceiptedDelivery: 1,
            GuaranteeDelivery: 2
        };

        function refreshGrid(gridClientID) {
            var gridObj = $find(gridClientID);

            if (gridObj)
                gridObj.get_masterTableView().rebind();
        }

        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/Sales/ClientSaleAppManagement.aspx");
        }

        function refreshMaintenancePage(sender, args) {

            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            redirectToPage("Views/Sales/ClientSaleAppMaintenance.aspx?EntityID=" + currentEntityID);
        }

        function openOrderProductWindow(id) {
            $.showLoading();

            var targetUrl = $.getRootPath() + "Views/Sales/Editors/ClientOrderProductMaintain.aspx?EntityID=" + id
                + "&OwnerEntityID=" + currentEntityID + "&GridClientID=" + gridClientIDs.gridOrderProducts;

            $.openRadWindow(targetUrl, "winCleintOrderProduct", true, 800, 400);
        }

        function openAppPaymentWindow(id, paymentMethodID) {
            $.showLoading();

            var targetUrl = $.getRootPath() + "Views/Sales/Editors/ClientOrderPaymentMaintain.aspx?EntityID=" + id
                + "&OwnerEntityID=" + currentEntityID + "&PaymentMethodID=" + paymentMethodID
                + "&GridClientID=" + gridClientIDs.gridAppPayments;

            $.openRadWindow(targetUrl, "winCleintOrderPayment", true, 800, 440);
        }

        function onClientSelectedClientContact(sender, eventArgs) {
            var item = sender.get_selectedItem();
            var extension = item.get_attributes().getAttribute("Extension");

            if (extension) {
                var extensionObj = JSON.parse(extension);

                if (extensionObj) {
                    $("#<%= lblContactPhone.ClientID %>").html(extensionObj.PhoneNumber);
                }
            }
        }

        function onClientSelectedDeliveryMode(sender, eventArgs) {

            var selectedItem = sender.getItem(eventArgs.get_index());
            var selDeliveryModeID = parseInt(selectedItem.get_value());

            if (selDeliveryModeID === deliveryModes.ReceiptedDelivery) {
                $("#divReceivingBankAccount").show();
                $("#divGuaranteeInfo").hide();
            }
            else if (selDeliveryModeID === deliveryModes.GuaranteeDelivery) {
                $("#divReceivingBankAccount").hide();
                $("#divGuaranteeInfo").show();
            }
        }

        $(document).ready(function () {
            currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            var rcbxClientContact = $find("<%= rcbxClientContact.ClientID %>");

            var selectedItem = rcbxClientContact.get_selectedItem();
            if (selectedItem) {
                var extension = selectedItem.get_attributes().getAttribute("Extension");

                if (extension) {
                    var extensionObj = JSON.parse(extension);

                    if (extensionObj) {
                        $("#<%= lblContactPhone.ClientID %>").html(extensionObj.PhoneNumber);
                    }
                }
            }

            var saleOrderTypeID = $("#<%= hdnSaleOrderTypeID.ClientID %>").val();

            //招商模式
            if (saleOrderTypeID == ESaleOrderTypes.AttractBusinessMode) {
                $("#divDeliveryMode").show();

                var ddlDeliveryMode = $find("<%= ddlDeliveryMode.ClientID %>");

                if (ddlDeliveryMode) {

                    var selectedItem = ddlDeliveryMode.get_selectedItem();
                    var selDeliveryModeID = parseInt(selectedItem.get_value());

                    if (selDeliveryModeID === deliveryModes.ReceiptedDelivery) {
                        $("#divReceivingBankAccount").show();
                        $("#divGuaranteeInfo").hide();
                    }
                    else if (selDeliveryModeID === deliveryModes.GuaranteeDelivery) {
                        $("#divReceivingBankAccount").hide();
                        $("#divGuaranteeInfo").show();
                    }
                }
            }
            else if (saleOrderTypeID == ESaleOrderTypes.AttachedMode) {//挂靠模式
                $("#divDeliveryMode").hide();
                $("#divReceivingBankAccount").show();
                $("#divGuaranteeInfo").hide();
            }
        });

    </script>
</asp:Content>

