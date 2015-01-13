<%@ Page Title="出库单打印预览" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="PrintStockOut.aspx.cs" Inherits="ZhongDing.Web.Views.Sales.Printers.PrintStockOut" %>

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
                <div class="center-td f16 bold toppadding5 bottompadding5" style="border-bottom: 1px solid #e3e3e3;">出库单</div>
                <div class="mws-form-inline">
                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>出库单编号</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblCode" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="float-left">
                            <label>制单人</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblCreateBy" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>开单日期</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblBillDate" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="float-left">
                            <label>出库单状态</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblWorkflowStatus" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>出库日期</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblOutDate" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="float-left">
                            <label>打印日期</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblPrintTime" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="mws-form-row" runat="server" id="divDaBaoOrder" visible="false">
                        <div class="float-left width50-percent">
                            <label>配送公司</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblDistCompany" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="float-left">
                        </div>
                    </div>
                    <div runat="server" id="divClientUserOrder" visible="false">
                        <div class="mws-form-row">
                            <div class="float-left width50-percent">
                                <label>客户</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblClientUser" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>商业单位</label>
                                <div class="mws-form-item toppadding5">
                                    <asp:Label ID="lblClientCompany" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>收货人</label>
                            <div class="mws-form-item small toppadding5">
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

                    <telerik:RadGrid ID="rgStockOutDetails" runat="server" AutoGenerateColumns="false"
                        Skin="Silk" Width="99.8%" ShowHeader="true" OnNeedDataSource="rgStockOutDetails_NeedDataSource">
                        <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true" BorderWidth="0" BackColor="#fafafa">
                            <Columns>
                                <telerik:GridBoundColumn UniqueName="OrderCode" HeaderText="订单编号" DataField="OrderCode" ReadOnly="true">
                                    <HeaderStyle Width="160" />
                                    <ItemStyle HorizontalAlign="Left" Width="160" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="Warehouse" HeaderText="仓库" DataField="Warehouse" ReadOnly="true">
                                    <HeaderStyle Width="160" />
                                    <ItemStyle HorizontalAlign="Left" Width="160" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName" ReadOnly="true">
                                    <HeaderStyle Width="180" />
                                    <ItemStyle HorizontalAlign="Left" Width="180" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification" ReadOnly="true">
                                    <HeaderStyle Width="100" />
                                    <ItemStyle HorizontalAlign="Left" Width="100" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="FactoryName" HeaderText="生产企业" DataField="FactoryName" ReadOnly="true">
                                    <HeaderStyle Width="180" />
                                    <ItemStyle HorizontalAlign="Left" Width="180" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="UnitOfMeasurement" HeaderText="基本单位" DataField="UnitOfMeasurement" ReadOnly="true">
                                    <HeaderStyle Width="80" />
                                    <ItemStyle HorizontalAlign="Left" Width="80" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="OutQty" HeaderText="基本数量" DataField="OutQty" ReadOnly="true">
                                    <HeaderStyle Width="80" />
                                    <ItemStyle HorizontalAlign="Left" Width="80" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="NumberOfPackages" HeaderText="件数" DataField="NumberOfPackages" DataFormatString="{0:N2}" ReadOnly="true">
                                    <HeaderStyle Width="80" />
                                    <ItemStyle HorizontalAlign="Left" Width="80" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn UniqueName="SalesPrice" HeaderText="单价" DataField="SalesPrice" SortExpression="SalesPrice" ReadOnly="true">
                                    <HeaderStyle Width="60" />
                                    <ItemStyle HorizontalAlign="Left" Width="60" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblSalesPrice" runat="server" Text='<%# Eval("SalesPrice","{0:C2}") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="TotalSalesAmount" HeaderText="货款" DataField="TotalSalesAmount" SortExpression="TotalSalesAmount" ReadOnly="true">
                                    <HeaderStyle Width="80" />
                                    <ItemStyle HorizontalAlign="Left" Width="80" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalSalesAmount" runat="server" Text='<%# Eval("TotalSalesAmount","{0:C2}") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn UniqueName="BatchNumber" HeaderText="货品批号" DataField="BatchNumber" SortExpression="BatchNumber" ReadOnly="true">
                                    <HeaderStyle Width="160" />
                                    <ItemStyle HorizontalAlign="Left" Width="160" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="ExpirationDate" HeaderText="过期日期" DataField="ExpirationDate" SortExpression="ExpirationDate" DataFormatString="{0:yyyy/MM/dd}" ReadOnly="true">
                                    <HeaderStyle Width="140" />
                                    <ItemStyle HorizontalAlign="Left" Width="140" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="LicenseNumber" HeaderText="批准文号" DataField="LicenseNumber" SortExpression="LicenseNumber" ReadOnly="true">
                                    <HeaderStyle Width="160" />
                                    <ItemStyle HorizontalAlign="Left" Width="160" />
                                </telerik:GridBoundColumn>
                            </Columns>
                            <NoRecordsTemplate>
                                没有任何数据
                            </NoRecordsTemplate>
                            <ItemStyle Height="30" />
                            <AlternatingItemStyle BackColor="#f2f2f2" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true">
                        </ClientSettings>
                        <HeaderStyle Width="100%" />
                    </telerik:RadGrid>
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

