<%@ Page Title="入库单打印预览" Language="C#" MasterPageFile="~/Site.Window.Master" AutoEventWireup="true" CodeBehind="PrintStockIn.aspx.cs" Inherits="ZhongDing.Web.Views.Procures.Printers.PrintStockIn" %>

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
                <div class="center-td f16 bold toppadding5 bottompadding5" style="border-bottom: 1px solid #e3e3e3;">入库单</div>
                <div class="mws-form-inline">
                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>入库单编号</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblCode" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="float-left">
                            <label>申请人</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblCreateBy" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>供应商</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblSupplier" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="float-left">
                            <label>入库日期</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblEntryDate" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="mws-form-row">
                        <div class="float-left width50-percent">
                            <label>入库单状态</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblWorkflowStatus" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="float-left">
                            <label>打印时间</label>
                            <div class="mws-form-item toppadding5">
                                <asp:Label ID="lblPrintTime" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <telerik:RadGrid ID="rgStockInDetails" runat="server" AutoGenerateColumns="false"
                        Skin="Silk" Width="99.8%" ShowHeader="true" OnNeedDataSource="rgStockInDetails_NeedDataSource">
                        <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true" BorderWidth="0" BackColor="#fafafa">
                            <Columns>
                                <telerik:GridBoundColumn UniqueName="OrderCode" HeaderText="采购订单编号" DataField="OrderCode" ReadOnly="true">
                                    <HeaderStyle Width="160" />
                                    <ItemStyle HorizontalAlign="Left" Width="160" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="Warehouse" HeaderText="入库仓库" DataField="Warehouse" ReadOnly="true">
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
                                <telerik:GridBoundColumn UniqueName="InQty" HeaderText="基本数量" DataField="InQty" ReadOnly="true">
                                    <HeaderStyle Width="80" />
                                    <ItemStyle HorizontalAlign="Left" Width="80" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="NumberOfPackages" HeaderText="件数" DataField="NumberOfPackages" DataFormatString="{0:N2}" ReadOnly="true">
                                    <HeaderStyle Width="80" />
                                    <ItemStyle HorizontalAlign="Left" Width="80" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="BatchNumber" HeaderText="货品批号" DataField="BatchNumber" ReadOnly="true">
                                    <HeaderStyle Width="80" />
                                    <ItemStyle HorizontalAlign="Left" Width="80" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="ExpirationDate" HeaderText="过期日期" DataField="ExpirationDate" DataFormatString="{0:yyyy/MM/dd}" ReadOnly="true">
                                    <HeaderStyle Width="80" />
                                    <ItemStyle HorizontalAlign="Left" Width="80" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="LicenseNumber" HeaderText="批准文号" DataField="LicenseNumber" ReadOnly="true">
                                    <HeaderStyle Width="80" />
                                    <ItemStyle HorizontalAlign="Left" Width="80" />
                                </telerik:GridBoundColumn>
                                <telerik:GridCheckBoxColumn UniqueName="IsMortgagedProduct" HeaderText="抵款货物" DataField="IsMortgagedProduct" SortExpression="IsMortgagedProduct" ReadOnly="true">
                                    <HeaderStyle Width="80" />
                                    <ItemStyle HorizontalAlign="Left" Width="80" />
                                </telerik:GridCheckBoxColumn>
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
