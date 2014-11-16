<%@ Page Title="商业单位维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientCompanyMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.ClientCompanyMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rgCertificates">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgCertificates" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="container">
        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">商业单位维护</span>
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
                            <label>商业单位名称</label>
                            <div class="mws-form-item small">
                                <telerik:RadTextBox runat="server" ID="txtName" CssClass="mws-textinput" Width="40%" MaxLength="100"></telerik:RadTextBox>
                                <telerik:RadToolTip ID="rttName" runat="server" TargetControlID="txtName" ShowEvent="OnClick"
                                    Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                </telerik:RadToolTip>
                                <asp:RequiredFieldValidator ID="rfvName" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtName"
                                    ErrorMessage="商业单位名称必填" Text="*" CssClass="field-validation-error">
                                </asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvName" runat="server" ErrorMessage="商业单位名称已存在，请使用其他名称"
                                    ControlToValidate="txtName" ValidationGroup="vgMaintenance"
                                    Text="*" CssClass="field-validation-error" OnServerValidate="cvName_ServerValidate">
                                </asp:CustomValidator>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>地区</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtDistrict" CssClass="mws-textinput" Width="60%" MaxLength="50"></telerik:RadTextBox>
                                </div>
                            </div>
                            <div class="float-left width40-percent">
                                <label>邮编</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtPostalCode" CssClass="mws-textinput" Width="60%" MaxLength="10"></telerik:RadTextBox>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>地址</label>
                            <div class="mws-form-item medium">
                                <telerik:RadTextBox runat="server" ID="txtAddress" CssClass="mws-textinput" Width="80%" MaxLength="200"></telerik:RadTextBox>
                            </div>
                        </div>
                        <div class="mws-form-row" runat="server" id="divOtherSections">
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
                                                                    <input type="button" class="rgAdd" onclick="openCertificateWindow(-1, EOwnerTypes.Client, gridClientIDs.gridCertificates); return false;" />
                                                                    <a href="javascript:void(0)" onclick="openCertificateWindow(-1, EOwnerTypes.Client, gridClientIDs.gridCertificates); return false;">添加</a>
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
                    </div>
                    <div class="mws-button-row">
                        <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                        <asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="mws-button orange" CausesValidation="false" OnClick="btnDelete_Click" OnClientClick="return onConfirmDelete();" />
                        <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Basics/ClientCompanyManagement.aspx');return false;" />
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnCurrentEntityID" runat="server" Value="-1" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">

        var gridClientIDs = {
            gridCertificates: "<%= rgCertificates.ClientID %>"
        };

        function refreshGrid(gridClientID) {
            var gridObj = $find(gridClientID);

            if (gridObj)
                gridObj.get_masterTableView().rebind();
        }

        function redirectToManagementPage(sender, args) {
            redirectToPage("Views/Basics/ClientCompanyManagement.aspx");
        }

        function refreshMaintenancePage(sender, args) {
            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            redirectToPage("Views/Basics/ClientCompanyMaintenance.aspx?EntityID=" + currentEntityID);
        }

        function openCertificateWindow(id, ownerTypeID, gridClientID) {
            $.showLoading();

            var currentEntityID = $("#<%= hdnCurrentEntityID.ClientID %>").val();

            var targetUrl = $.getRootPath() + "Views/Basics/Editors/CertificateMaintain.aspx?EntityID=" + id
                + "&OwnerEntityID=" + currentEntityID + "&OwnerTypeID=" + ownerTypeID + "&GridClientID=" + gridClientID;

            $.openRadWindow(targetUrl, "winCertificate", true, 800, 400);
        }

        $(document).ready(function () {

        });

    </script>
</asp:Content>
