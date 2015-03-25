<%@ Page Title="配送公司流向数据维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DCFlowDataMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Imports.DCFlowDataMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rgDetails">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgDetails" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="container">
        <div class="mws-panel grid_full">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">配送公司流向数据维护</span>
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
                        <div class="mws-form-row">
                            <!--提成分配 -->
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-payment" runat="server" id="divAppPayments">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">流向提成</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <telerik:RadGrid ID="rgDetails" runat="server" PageSize="10"
                                            AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true" ShowFooter="true"
                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgDetails_NeedDataSource" OnUpdateCommand="rgDetails_UpdateCommand">
                                            <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa" EditMode="InPlace">
                                                <CommandItemSettings ShowAddNewRecordButton="false" RefreshText="刷新" />
                                                <Columns>
                                                    <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="ContractCode" HeaderText="协议编号" DataField="ContractCode" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="ClientUserName" HeaderText="客户" DataField="ClientUserName" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="InChargeUserFullName" HeaderText="业务经理" DataField="InChargeUserFullName" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="HospitalName" HeaderText="医院" DataField="HospitalName" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="UnitName" HeaderText="单位" DataField="UnitName" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="SaleQty" HeaderText="数量" DataField="SaleQty" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Comment" HeaderText="备注" DataField="Comment" SortExpression="Comment">
                                                        <HeaderStyle Width="15%" />
                                                        <ItemStyle Width="15%" />
                                                        <ItemTemplate>
                                                            <span><%# Eval("Comment") %></span>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div id="divGridCombox">
                                                                <telerik:RadTextBox runat="server" ID="txtComment" Width="100%" MaxLength="500" Text='<%# Eval("Comment") %>'>
                                                                </telerik:RadTextBox>
                                                            </div>
                                                        </EditItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridEditCommandColumn UniqueName="Edit" ButtonType="LinkButton" InsertText="保存" EditText="修改备注" UpdateText="更新" CancelText="取消" HeaderStyle-Width="80" ItemStyle-Width="80">
                                                    </telerik:GridEditCommandColumn>
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
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="mws-button-row">
                            <asp:Button ID="btnCorrect" runat="server" Text="纠正流向" CssClass="mws-button green" UseSubmitBehavior="false" CausesValidation="false" />
                            <asp:Button ID="btnImport" runat="server" Text="导入医院流向" CssClass="mws-button green" UseSubmitBehavior="false" CausesValidation="false" />
                            <asp:Button ID="btnCancel" runat="server" Text="返回" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Imports/DCFlowDataManagement.aspx');return false;" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">

        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/Imports/DCFlowDataManagement.aspx");
        }

    </script>
</asp:Content>
