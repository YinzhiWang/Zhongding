<%@ Page Title="货品维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Products.ProductMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlProductCategory">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divDepartment" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgProductSpecifications">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgProductSpecifications" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgCertificates">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgCertificates" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="container">

        <div class="mws-panel grid_full">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">货品维护</span>
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
                                <label>货品编号</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtProductCode" CssClass="mws-textinput" Width="80%"></telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="rfvProductCode"
                                        runat="server"
                                        ErrorMessage="货品编号必填"
                                        ControlToValidate="txtProductCode"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvProductCode" runat="server" Display="Dynamic" ErrorMessage="货品编号不能重复，请重新输入"
                                        ControlToValidate="txtProductCode" ValidationGroup="vgMaintenance" OnServerValidate="cvProductCode_ServerValidate" Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>货品类别</label>
                                <div class="mws-form-item small">
                                    <telerik:RadDropDownList runat="server" ID="ddlProductCategory" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlProductCategory_SelectedIndexChanged" DefaultMessage="--请选择--">
                                    </telerik:RadDropDownList>
                                    <asp:RequiredFieldValidator ID="rfvProductCategory"
                                        runat="server"
                                        ErrorMessage="请选择货品类别"
                                        ControlToValidate="ddlProductCategory"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>货品名称</label>
                            <div class="mws-form-item">
                                <telerik:RadTextBox runat="server" ID="txtProductName" CssClass="mws-textinput" Width="40%"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="rfvProductName"
                                    runat="server"
                                    ErrorMessage="货品名称必填"
                                    ControlToValidate="txtProductName"
                                    Display="Dynamic" CssClass="field-validation-error"
                                    ValidationGroup="vgMaintenance" Text="*">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>供应商</label>
                                <div class="mws-form-item">
                                    <telerik:RadComboBox runat="server" ID="rcbxSupplier" Filter="Contains" AllowCustomText="false"
                                        MarkFirstMatch="true" Height="160px" EmptyMessage="--请选择--"
                                        OnItemDataBound="rcbxSupplier_ItemDataBound" OnClientSelectedIndexChanged="onClientSelectedSupplier">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="rfvSupplier"
                                        runat="server"
                                        ErrorMessage="请选择供应商"
                                        ControlToValidate="rcbxSupplier"
                                        Display="Dynamic" CssClass="field-validation-error"
                                        ValidationGroup="vgMaintenance" Text="*">
                                    </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvSupplier" runat="server" ErrorMessage="供应商不存在，请重新选择"
                                        ControlToValidate="rcbxSupplier" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error" OnServerValidate="cvSupplier_ServerValidate">
                                    </asp:CustomValidator>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>生产企业</label>
                                <div class="mws-form-item">
                                    <asp:Label ID="lblProducer" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>有效期（天）</label>
                                <div class="mws-form-item">
                                    <telerik:RadNumericTextBox runat="server" ID="txtValidDays" CssClass="mws-textinput" Type="Number" ShowSpinButtons="true"
                                        NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="10">
                                    </telerik:RadNumericTextBox>
                                </div>
                            </div>
                            <div class="float-left">
                                <label>是否批号管理</label>
                                <div class="mws-form-item">
                                    <telerik:RadButton runat="server" ID="cbxIsManagedByBatchNumber" ButtonType="ToggleButton"
                                        ToggleType="CheckBox" AutoPostBack="false">
                                    </telerik:RadButton>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>成本核算方法</label>
                                <div class="mws-form-item">
                                    <label>实际计价法</label>
                                </div>
                            </div>
                            <div class="float-left" runat="server" id="divDepartment" visible="false">
                                <label>所属部门</label>
                                <div class="mws-form-item">
                                    <telerik:RadComboBox runat="server" ID="rcbxDepartment" Filter="Contains" AllowCustomText="false" MarkFirstMatch="true" Height="160px" EmptyMessage="--请选择--">
                                    </telerik:RadComboBox>
                                    <asp:CustomValidator ID="cvRequiredDepartment" runat="server" ErrorMessage="请选择所属部门"
                                        ControlToValidate="rcbxDepartment" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                    <asp:CustomValidator ID="cvDepartment" runat="server" ErrorMessage="部门不存在，请重新选择"
                                        ControlToValidate="rcbxDepartment" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error" OnServerValidate="cvDepartment_ServerValidate">
                                    </asp:CustomValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>安全库存值</label>
                            <div class="mws-form-item medium">
                                <telerik:RadNumericTextBox runat="server" ID="txtSafetyStock" CssClass="mws-textinput" Type="Number" ShowSpinButtons="true"
                                    NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MinValue="0" MaxValue="999999999" MaxLength="10">
                                </telerik:RadNumericTextBox>
                            </div>
                        </div>

                        <div class="mws-form-row" runat="server" id="divOtherSections">
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-product-specification">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">规格维护</span>
                                </div>
                                <div class="mws-panel-body">
                                    <div class="mws-panel-content">
                                        <telerik:RadGrid ID="rgProductSpecifications" runat="server" PageSize="10"
                                            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgProductSpecifications_NeedDataSource" OnDeleteCommand="rgProductSpecifications_DeleteCommand">
                                            <MasterTableView Width="100%" DataKeyNames="ID" CommandItemDisplay="Top"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                                <Columns>
                                                    <telerik:GridBoundColumn UniqueName="ID" HeaderText="ID" DataField="ID" Visible="false" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="Specification" HeaderText="规格" DataField="Specification">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="UnitOfMeasurement" HeaderText="基本单位" DataField="UnitOfMeasurement">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="NumberInSmallPackage" HeaderText="小包装数量" DataField="NumberInSmallPackage">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="NumberInLargePackage" HeaderText="每件数量" DataField="NumberInLargePackage">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Edit" HeaderStyle-Width="40">
                                                        <ItemStyle HorizontalAlign="Center" Width="40" />
                                                        <ItemTemplate>
                                                            <a href="javascript:void(0);" onclick="openProductSpecificationWindow(<%#DataBinder.Eval(Container.DataItem,"ID")%>)">
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
                                                                    <input type="button" class="rgAdd" onclick="openProductSpecificationWindow(-1); return false;" />
                                                                    <a href="javascript:void(0)" onclick="openProductSpecificationWindow(-1); return false;">添加</a>
                                                                </asp:Panel>
                                                            </td>
                                                            <td class="right-td rightpadding10">
                                                                <input type="button" class="rgRefresh" onclick="refreshGrid(gridClientIDs.gridProductSpecifications); return false;" />
                                                                <a href="javascript:void(0);" onclick="refreshGrid(gridClientIDs.gridProductSpecifications); return false;">刷新</a>
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

                            <!--证照信息维护 -->
                            <div class="mws-panel grid_8 mws-collapsible" data-collapseid="panel-certificates">
                                <div class="mws-panel-header">
                                    <span class="mws-i-24 i-creditcard">证照信息维护</span>
                                </div>
                                <div class="mws-panel-body" runat="server" id="divPanelCertificates">
                                    <div class="mws-panel-content">
                                        <telerik:RadGrid ID="rgCertificates" runat="server" PageSize="10"
                                            AllowPaging="True" AllowCustomPaging="true" AllowSorting="True" AutoGenerateColumns="false"
                                            MasterTableView-PagerStyle-AlwaysVisible="true" Skin="Silk" Width="99.8%" ShowHeader="true"
                                            ClientSettings-ClientEvents-OnRowMouseOver="onRowMouseOver" ClientSettings-ClientEvents-OnRowMouseOut="onRowMouseOut"
                                            OnNeedDataSource="rgCertificates_NeedDataSource" OnDeleteCommand="rgCertificates_DeleteCommand">
                                            <MasterTableView Width="100%" DataKeyNames="OwnerEntityID" CommandItemDisplay="Top"
                                                ShowHeadersWhenNoRecords="true" BackColor="#fafafa">
                                                <Columns>
                                                    <telerik:GridBoundColumn UniqueName="OwnerEntityID" HeaderText="OwnerEntityID" DataField="OwnerEntityID" Visible="false" ReadOnly="true">
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
                                                    <telerik:GridTemplateColumn UniqueName="IsNeedAlert" HeaderText="是否<br/>提醒" DataField="IsNeedAlert" SortExpression="IsNeedAlert">
                                                        <ItemStyle HorizontalAlign="Left" Width="40" />
                                                        <ItemTemplate>
                                                            <span>
                                                                <%#DataBinder.Eval(Container.DataItem,"IsNeedAlert")!=null
                                                                        ?(Convert.ToBoolean( DataBinder.Eval(Container.DataItem,"IsNeedAlert").ToString())==true ? "X":
                                                                        string.Empty):string.Empty%>
                                                            </span>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="AlertBeforeDays" HeaderText="提醒<br/>期限" DataField="AlertBeforeDays" SortExpression="AlertBeforeDays">
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
                                                    <telerik:GridTemplateColumn UniqueName="Edit" HeaderStyle-Width="40">
                                                        <ItemStyle HorizontalAlign="Center" Width="40" />
                                                        <ItemTemplate>
                                                            <a href="javascript:void(0);" onclick="openCertificateWindow(<%#DataBinder.Eval(Container.DataItem,"OwnerEntityID")%>, EOwnerTypes.Client, gridClientIDs.gridCertificates);">
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
                                                                    <input type="button" class="rgAdd" onclick="openCertificateWindow(-1, EOwnerTypes.Product, gridClientIDs.gridCertificates); return false;" />
                                                                    <a href="javascript:void(0)" onclick="openCertificateWindow(-1, EOwnerTypes.Product, gridClientIDs.gridCertificates); return false;">添加</a>
                                                                </asp:Panel>
                                                            </td>
                                                            <td class="right-td rightpadding10">
                                                                <input type="button" class="rgRefresh" onclick="refreshGrid(gridClientIDs.gridCertificates); return false;" />
                                                                <a href="javascript:void(0);" onclick="refreshGrid(gridClientIDs.gridCertificates); return false;">刷新</a>
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
                            <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Products/ProductManagement.aspx');return false;" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnCurrentEntityID" runat="server" Value="-1" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">

        var currentEntityID = -1;

        var gridClientIDs = {
            gridProductSpecifications: "<%= rgProductSpecifications.ClientID %>",
            gridCertificates: "<%= rgCertificates.ClientID %>",
        };

        function refreshGrid(gridClientID) {
            var gridObj = $find(gridClientID);

            if (gridObj)
                gridObj.get_masterTableView().rebind();
        }

        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/Products/ProductManagement.aspx");
        }

        function refreshMaintenancePage(sender, args) {

            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            redirectToPage("Views/Products/ProductMaintenance.aspx?EntityID=" + currentEntityID);
        }

        function openProductSpecificationWindow(id) {
            $.showLoading();

            var targetUrl = $.getRootPath() + "Views/Products/Editors/ProductSpecificationMaintain.aspx?EntityID=" + id
                + "&OwnerEntityID=" + currentEntityID + "&GridClientID=" + gridClientIDs.gridProductSpecifications;

            $.openRadWindow(targetUrl, "winProductSpecification", true, 800, 380);
        }

        function openCertificateWindow(id, ownerTypeID, gridClientID) {
            $.showLoading();

            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            var targetUrl = $.getRootPath() + "Views/Basics/Editors/CertificateMaintain.aspx?EntityID=" + id
                + "&OwnerEntityID=" + currentEntityID + "&OwnerTypeID=" + ownerTypeID + "&GridClientID=" + gridClientID;

            $.openRadWindow(targetUrl, "winCertificate", true, 800, 400);
        }

        function onClientSelectedSupplier(sender, eventArgs) {
            //debugger;

            var item = eventArgs.get_item();

            var factoryName = item.get_attributes().getAttribute("FactoryName");

            $("#<%=lblProducer.ClientID%>").html(factoryName);

        }

        $(document).ready(function () {
            currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();
        });

    </script>
</asp:Content>
