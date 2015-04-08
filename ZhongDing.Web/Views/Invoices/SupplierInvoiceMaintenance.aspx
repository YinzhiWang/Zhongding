<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SupplierInvoiceMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Invoices.SupplierInvoiceMaintenance" %>


<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">供应商发票维护</span>
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
                                <label>发票号</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtInvoiceNumber" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="rfvInvoiceNumber" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtInvoiceNumber"
                                        ErrorMessage="发货日期必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                    <telerik:RadToolTip ID="rttInvoiceNumber" runat="server" TargetControlID="txtInvoiceNumber" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                </div>

                            </div>
                            <div class="float-left">
                                <label>开票日期</label>
                                <div class="mws-form-item small">
                                    <telerik:RadDatePicker ID="txtInvoiceDate" Width="200px" runat="server">
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="rfvSendDate" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtInvoiceDate"
                                        ErrorMessage="开票日期必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                    <telerik:RadToolTip ID="rttSendDate" runat="server" TargetControlID="txtInvoiceDate" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                </div>
                            </div>
                        </div>


                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>总金额</label>
                                <div class="mws-form-item small">
                                    <telerik:RadNumericTextBox ShowSpinButtons="true" MinValue="0" IncrementSettings-InterceptArrowKeys="true" IncrementSettings-InterceptMouseWheel="true"
                                        Label="" runat="server" ID="txtAmount" Width="160px">
                                    </telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtAmount"
                                        ErrorMessage="总金额必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                </div>

                            </div>
                            <div class="float-left">
                                <label>供应商名称</label>
                                <div class="mws-form-item small">
                                    <telerik:RadComboBox runat="server" ID="rcbxSupplier" Filter="Contains" AllowCustomText="true"
                                        MarkFirstMatch="true" Height="160px" EmptyMessage="--请选择--">
                                    </telerik:RadComboBox>
                                    <telerik:RadToolTip ID="rttSupplier" runat="server" TargetControlID="rcbxSupplier" ShowEvent="OnClick"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                    <asp:CustomValidator ID="cvSupplier" runat="server" ErrorMessage="请选择供应商"
                                        ControlToValidate="rcbxSupplier" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnSearchOrder" runat="server" Text="查询订单" CssClass="mws-button green" CausesValidation="false" ValidationGroup="vgMaintenance" OnClick="btnSearchOrder_Click" />
                                </div>
                            </div>
                        </div>
                        <%--rgProcureOrderAppDetails--%>
                        <div class="mws-form-row" style="padding-top: 10px; padding-left: 0px; padding-right: 1px;">
                            <telerik:RadGrid ID="rgProcureOrderAppDetails" runat="server" PageSize="10" AllowCustomPaging="true"
                                AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false" AllowMultiRowSelection="true"
                                MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" Height="500" ShowHeader="true" ShowFooter="true"
                                ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                OnNeedDataSource="rgProcureOrderAppDetails_NeedDataSource" OnItemDataBound="rgProcureOrderAppDetails_ItemDataBound"
                                OnColumnCreated="rgProcureOrderAppDetails_ColumnCreated">
                                <MasterTableView Width="100%" DataKeyNames="ID,NotTaxAmount" CommandItemDisplay="None"
                                    ShowHeadersWhenNoRecords="true" BackColor="#fafafa" ClientDataKeyNames="ID,NotTaxAmount">
                                    <Columns>
                                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderText="全选">
                                            <HeaderStyle Width="30" />
                                            <ItemStyle Width="30" />
                                        </telerik:GridClientSelectColumn>
                                        <telerik:GridBoundColumn UniqueName="ProcureOrderApplicationOrderCode" HeaderText="订单编号" DataField="ProcureOrderApplicationOrderCode" ReadOnly="true">
                                            <HeaderStyle Width="160px" />
                                            <ItemStyle HorizontalAlign="Left" Width="160px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ProcureOrderApplicationOrderDate" HeaderText="订单日期" DataFormatString="{0:yyyy/MM/dd}" DataField="ProcureOrderApplicationOrderDate" ReadOnly="true">
                                            <HeaderStyle Width="100px" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ProductName" HeaderText="货品名称" DataField="ProductName" ReadOnly="true">
                                            <HeaderStyle />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification" ReadOnly="true">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="UnitOfMeasurement" HeaderText="基本单位" DataField="UnitOfMeasurement" ReadOnly="true">
                                            <HeaderStyle Width="100" />
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn UniqueName="ProcureCount" HeaderText="数量" DataField="ProcureCount" ReadOnly="true">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="TotalAmount" HeaderText="订单金额" DataField="TotalAmount" ReadOnly="true" DataFormatString="￥{0:f2}">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="TaxAmount" HeaderText="需开票金额" DataField="TaxAmount" ReadOnly="true" DataFormatString="￥{0:f2}">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="NotTaxAmount" HeaderText="未开票金额" DataField="NotTaxAmount" ReadOnly="true" DataFormatString="￥{0:f2}">
                                            <HeaderStyle Width="80" />
                                            <ItemStyle HorizontalAlign="Left" Width="80" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridTemplateColumn UniqueName="CurrentTaxQty" HeaderText="本次开票金额" DataField="CurrentTaxQty" SortExpression="CurrentTaxQty">
                                            <HeaderStyle Width="160" />
                                            <ItemStyle HorizontalAlign="Left" Width="160" />
                                            <ItemTemplate>
                                                <telerik:RadNumericTextBox runat="server" ID="txtSupplierInvoiceDetailAmount" Type="Number" MaxLength="9" Width="120" ShowSpinButtons="true"
                                                    MinValue="1" MaxValue="99999999" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="">
                                                    <ClientEvents OnValueChanging="onSupplierInvoiceDetailAmountChanging" />
                                                </telerik:RadNumericTextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                    </Columns>
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
                                    <Selecting AllowRowSelect="True" />
                                    <Scrolling AllowScroll="true" FrozenColumnsCount="4" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <ClientEvents OnRowSelecting="onRowSelecting" />
                                </ClientSettings>
                                <HeaderStyle Width="99.8%" />
                            </telerik:RadGrid>
                        </div>

                        <div class="height20"></div>
                    </div>
                    <div class="mws-button-row">
                        <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" OnClientClick="return onBtnSaveClick();" />
                        <asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="mws-button orange" CausesValidation="false" OnClick="btnDelete_Click" OnClientClick="return onConfirmDelete();" />
                        <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="onBtnCancelClick();return false;" />
                    </div>
                </div>
            </div>
        </div>

    </div>
    <asp:HiddenField ID="hdnCurrentEntityID" runat="server" Value="-1" />
    <telerik:RadNotification ID="radNotification" runat="server" EnableRoundedCorners="true"
        AutoCloseDelay="1000" Skin="Silk" Animation="Fade" EnableShadow="true" Title="提示"
        TitleIcon="none" Opacity="95" Position="Center" ContentIcon="~/Content/icons/32/cross.png"
        Width="300" Height="100">
    </telerik:RadNotification>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">

        function onClientHidden(sender, args) {
            redirectToPage('Views/Invoices/SupplierInvoiceManagement.aspx');
        }

        function onClientBlur(sender, args) {


        }
        function onBtnCancelClick() {
            redirectToPage('Views/Invoices/SupplierInvoiceManagement.aspx');
        }
        function onSupplierInvoiceDetailAmountChanging(sender, eventArgs) {
            var gridItem = sender.get_parent();

            if (gridItem) {

                var gridItemElement = gridItem.get_element();

                var newValue = eventArgs.get_newValue();
                console.log("newValue:" + newValue);
                var radNotification = $find("<%=radNotification.ClientID%>");
                if (newValue) {
                    //var txtCurrentOutQty = $telerik.findControl(gridItemElement, "txtSupplierInvoiceDetailAmount");
                    //var currentOutQty = txtCurrentOutQty.get_value();
                    var notTaxAmount = gridItem.getDataKeyValue("NotTaxAmount");
                    if (newValue > notTaxAmount) {
                        radNotification.set_text("本次开票金额不能大于未开票金额：" + newValue);
                        radNotification.show();
                        eventArgs.set_cancel(true);
                    }
                }

                var txtAmount = $find("<%=txtAmount.ClientID%>");
                //var amount = txtAmount.get_value();
                //console.log("amount:" + amount);
                //if (amount) {
                //    var selectedTotalSupplierInvoiceDetailAmount = calculateSelectedTotalSupplierInvoiceDetailAmount();

                //    console.log("selectedTotalSupplierInvoiceDetailAmount:" + selectedTotalSupplierInvoiceDetailAmount);
                //    if (parseFloat(amount) < parseFloat(selectedTotalSupplierInvoiceDetailAmount)) {
                //        console.log("Error parseFloat(amount) < selectedTotalSupplierInvoiceDetailAmount");
                //        radNotification.set_text("本次开票金额总计不能大于总金额：" + amount);
                //        radNotification.show();

                //        eventArgs.set_cancel(true);
                //    }
                //}

            }
        }


        function onRowSelecting(sender, eventArgs) {
            //debugger;

            var selectingItem = eventArgs.get_gridDataItem();
            var selectingElement = selectingItem.get_element();

            var txtSupplierInvoiceDetailAmount = $telerik.findControl(selectingElement, "txtSupplierInvoiceDetailAmount");
            var supplierInvoiceDetailAmount = txtSupplierInvoiceDetailAmount.get_value();
            console.log("supplierInvoiceDetailAmount:" + supplierInvoiceDetailAmount);
            if (supplierInvoiceDetailAmount == null) {
                var radNotification = $find("<%=radNotification.ClientID%>");

                radNotification.set_text("本次开票金额为必填项");
                radNotification.show();

                eventArgs.set_cancel(true);
            }
            else {

            }
        }

        function onBtnSaveClick(sender, eventArgs) {
            var radNotification = $find("<%=radNotification.ClientID%>");
            var txtAmount = $find("<%=txtAmount.ClientID%>");
            var amount = txtAmount.get_value();
            if (!amount) {
                radNotification.set_text("总金额为必填项");
                radNotification.show();
                return false;
            }
            var selectedTotalSupplierInvoiceDetailAmount = calculateSelectedTotalSupplierInvoiceDetailAmount();

            if (amount != selectedTotalSupplierInvoiceDetailAmount) {

                radNotification.set_text("本次开票金额总计必须等于总金额：" + amount);
                radNotification.show();
                return false;
            }

            var rgProcureOrderAppDetails = $find("<%=rgProcureOrderAppDetails.ClientID%>")
            var selectedItems = rgProcureOrderAppDetails.get_masterTableView().get_selectedItems();

            for (var i = 0; i < selectedItems.length; i++) {
                var curSelectedItem = selectedItems[i];
                var curSelectedItemElement = curSelectedItem.get_element();


                var curCurrentOutQtyControl = $telerik.findControl(curSelectedItemElement, "txtCurrentOutQty");
                var curCurrentTxtSupplierInvoiceDetailAmount = $telerik.findControl(curSelectedItemElement, "txtSupplierInvoiceDetailAmount");

                var curCurrentSupplierInvoiceDetailAmount = curCurrentTxtSupplierInvoiceDetailAmount.get_value();

                if (!curCurrentSupplierInvoiceDetailAmount) {
                    radNotification.set_text("勾选的订单必须填写本次开票金额");
                    radNotification.show();
                    return false;
                }
            }

            return true;
        }
        function calculateSelectedTotalSupplierInvoiceDetailAmount() {
            var selectedTotalSupplierInvoiceDetailAmount = 0;
            //获取已经选中的items
            var rgProcureOrderAppDetails = $find("<%=rgProcureOrderAppDetails.ClientID%>")
            var selectedItems = rgProcureOrderAppDetails.get_masterTableView().get_selectedItems();

            for (var i = 0; i < selectedItems.length; i++) {
                var curSelectedItem = selectedItems[i];
                var curSelectedItemElement = curSelectedItem.get_element();


                var curCurrentOutQtyControl = $telerik.findControl(curSelectedItemElement, "txtCurrentOutQty");
                var curCurrentTxtSupplierInvoiceDetailAmount = $telerik.findControl(curSelectedItemElement, "txtSupplierInvoiceDetailAmount");

                var curCurrentSupplierInvoiceDetailAmount = curCurrentTxtSupplierInvoiceDetailAmount.get_value();

                if (curCurrentSupplierInvoiceDetailAmount) {
                    selectedTotalSupplierInvoiceDetailAmount += parseFloat(curCurrentSupplierInvoiceDetailAmount);
                }
            }

            return selectedTotalSupplierInvoiceDetailAmount;
        }
    </script>
</asp:Content>
