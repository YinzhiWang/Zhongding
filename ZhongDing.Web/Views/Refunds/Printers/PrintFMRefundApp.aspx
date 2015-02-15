<%@ Page Title="厂家经理返款单" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="PrintFMRefundApp.aspx.cs" Inherits="ZhongDing.Web.Views.Refunds.Printers.PrintFMRefundApp" %>

<%@ MasterType VirtualPath="~/Site.Window.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="center-td bottompadding10 no-print">
        <asp:Button ID="btnPrint" runat="server" Text="打印" CssClass="mws-button green" CausesValidation="true" OnClientClick="window.print();return false;" />
        &nbsp;&nbsp;
        <asp:Button ID="Button1" runat="server" Text="取消" CssClass="mws-button orange" CausesValidation="true" OnClientClick="window.close();return false;" />
    </div>
    <div class="mws-panel" style="margin-bottom: 10px;">
        <div class="mws-panel-body">
            <div class="mws-form">
                <div class="center-td f16 bold toppadding5 bottompadding5" style="border-bottom: 1px solid #e3e3e3;">厂家经理返款单</div>
                <div class="mws-form-inline">

                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>申请日期</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblCreatedOn" runat="server"></asp:Label>
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
                        <div class="float-left width50-percent">
                            <label>账套</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblCompany" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="float-left width50-percent">
                            <label>客户</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblClientUser" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>货品</label>
                            <div class="mws-form-item toppadding5" runat="server" id="divProducts">
                                <asp:Label ID="lblProductName" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="float-left width50-percent">
                            <label>货品规格</label>
                            <div class="mws-form-item toppadding5" runat="server" id="divProductSpecifications">
                                <asp:Label ID="lblProductSpecification" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>结算起始日期</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblBeginDate" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="float-left width50-percent">
                            <label>结算结束日期</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblEndDate" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>入库数量</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblStockInQty" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="float-left width50-percent">
                            <label>出库数量</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblStockOutQty" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>返款单价</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblRefundPrice" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="float-left width50-percent">
                            <label>应返金额</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblRefundAmount" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <p class="bold leftpadding5 bottommargin5 topmargin20">支付信息</p>

                    <telerik:RadGrid ID="rgAppPayments" runat="server" PageSize="10"
                        AllowPaging="false" AllowCustomPaging="false" AllowSorting="false" AutoGenerateColumns="false"
                        MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true" ShowFooter="true"
                        ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                        OnNeedDataSource="rgAppPayments_NeedDataSource">
                        <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="None"
                            ShowHeadersWhenNoRecords="true" BackColor="#fafafa" EditMode="InPlace">
                            <Columns>
                                <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn UniqueName="PayDate" HeaderText="转账日期" DataField="PayDate" SortExpression="PayDate" ReadOnly="true">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle Width="10%" />
                                    <ItemTemplate>
                                        <span><%# Eval("PayDate","{0:yyyy/MM/dd}") %></span>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="FromAccount" HeaderText="转出账号" DataField="FromAccount" SortExpression="FromAccount" ReadOnly="true">
                                    <ItemStyle Width="35%" />
                                    <ItemTemplate>
                                        <span><%# Eval("FromAccount") %></span>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Amount" HeaderText="付款金额" DataField="Amount" SortExpression="Amount" ReadOnly="true">
                                    <HeaderStyle Width="15%" />
                                    <ItemStyle Width="15%" />
                                    <ItemTemplate>
                                        <span><%# Eval("Amount","{0:C2}") %></span>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Fee" HeaderText="手续费" DataField="Fee" SortExpression="Fee" ReadOnly="true">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle Width="10%" />
                                    <ItemTemplate>
                                        <span><%# Eval("Fee","{0:C2}") %></span>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Comment" HeaderText="备注" DataField="Comment" SortExpression="Comment" ReadOnly="true">
                                    <HeaderStyle Width="15%" />
                                    <ItemStyle Width="15%" />
                                    <ItemTemplate>
                                        <span><%# Eval("Comment") %></span>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <NoRecordsTemplate>
                                没有任何数据
                            </NoRecordsTemplate>
                            <ItemStyle Height="30" />
                            <CommandItemStyle Height="30" />
                            <AlternatingItemStyle BackColor="#f2f2f2" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" />
                    </telerik:RadGrid>
                    <div class="mws-form-row">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <style type="text/css">
        .RadGrid_Silk .rgHeader {
            color: #333333;
            padding-top: 10px;
            padding-bottom: 10px;
            border-left-color: #cccccc;
            border-left-width: 1px;
            border-left-style: solid;
        }

        .RadGrid_Silk .rgHeader {
            color: #333333;
            font-size: 13px;
            font-family: 'Microsoft Yahei',Arial,Helvetica,sans-serif,'微软雅黑';
            font-weight: normal;
            border-style: solid;
            border-bottom: 1px solid #e3e3e3;
        }

        .RadGrid_Silk .rgRow td, .RadGrid_Silk .rgAltRow td, .RadGrid_Silk .rgEditRow td {
            border-left-color: #bebebe !important;
            border-left-width: 1px;
            border-left-style: solid;
        }

            .RadGrid_Silk .rgRow td:first-child, .RadGrid_Silk .rgAltRow td:first-child, .RadGrid_Silk .rgEditRow td:first-child {
                border-left-style: none;
            }
    </style>

    <script type="text/javascript">

        function onError(sender, args) {
            window.close();
        }

    </script>

</asp:Content>
