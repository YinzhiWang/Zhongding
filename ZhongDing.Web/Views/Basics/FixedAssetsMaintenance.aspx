<%@ Page Title="固定资产维护" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FixedAssetsMaintenance.aspx.cs" Inherits="ZhongDing.Web.Views.Basics.FixedAssetsMaintenance" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">固定资产维护</span>
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
                                <label>资产编号</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtCode" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>
                                    <telerik:RadToolTip ID="rttCode" runat="server" TargetControlID="txtCode" ShowEvent="OnMouseOver"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                    <asp:RequiredFieldValidator ID="rfvCode" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtCode"
                                        ErrorMessage="资产编号必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvCode" runat="server" ErrorMessage="资产编号已存在，请使用其他名称"
                                        ControlToValidate="txtCode" ValidationGroup="vgMaintenance"
                                        Text="*" CssClass="field-validation-error" OnServerValidate="cvCode_ServerValidate">
                                    </asp:CustomValidator>

                                </div>

                            </div>
                            <div class="float-left">
                                <label>资产类别</label>
                                <div class="mws-form-item small">
                                    <telerik:RadComboBox runat="server" ID="rcbxFixedAssetsType" Filter="Contains" AllowCustomText="true"
                                        MarkFirstMatch="true" Height="160px" EmptyMessage="--请选择--"  >
                                    </telerik:RadComboBox>
                                    <telerik:RadToolTip ID="rttFixedAssetsType" runat="server" TargetControlID="rcbxFixedAssetsType" ShowEvent="OnMouseOver"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                    <asp:CustomValidator ID="cvFixedAssetsType" runat="server" ErrorMessage="请选择资产类别"
                                        ControlToValidate="rcbxFixedAssetsType" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>资产名称</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtName" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtName"
                                        ErrorMessage="资产名称必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                </div>

                            </div>

                        </div>


                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>计量单位</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtUnit" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>
                                </div>

                            </div>
                            <div class="float-left">
                                <label>数量</label>
                                <div class="mws-form-item small">
                                    <telerik:RadNumericTextBox ShowSpinButtons="true" MinValue="0" IncrementSettings-InterceptArrowKeys="true" IncrementSettings-InterceptMouseWheel="true"
                                        Label="" runat="server" ID="txtQuantity" Width="160px">
                                    </telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtQuantity"
                                        ErrorMessage="数量必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>规格型号</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtSpecification" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>
                                </div>

                            </div>
                            <div class="float-left">
                                <label>产地</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtProducingArea" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>制造商</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtManufacturer" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>
                                </div>

                            </div>
                            <div class="float-left">
                                <label>使用状态</label>
                                <div class="mws-form-item small">
                                    <telerik:RadComboBox runat="server" ID="rcbUseStatus" Filter="Contains" AllowCustomText="true"
                                        MarkFirstMatch="true" Height="160px" EmptyMessage="--请选择--">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="" Text="" />
                                            <telerik:RadComboBoxItem Value="1" Text="正在使用" />
                                            <telerik:RadComboBoxItem Value="2" Text="停用" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    <telerik:RadToolTip ID="RadToolTip1" runat="server" TargetControlID="rcbUseStatus" ShowEvent="OnMouseOver"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                    <asp:CustomValidator ID="cvUseStatus" runat="server" ErrorMessage="请选择使用状态"
                                        ControlToValidate="rcbUseStatus" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>使用部门</label>
                                <div class="mws-form-item small">
                                    <telerik:RadComboBox runat="server" ID="rcbxDepartment" Filter="Contains" AllowCustomText="true" Width="200px"
                                        MarkFirstMatch="true" Height="160px" EmptyMessage="--请选择--">
                                    </telerik:RadComboBox>
                                     <telerik:RadToolTip ID="RadToolTip4" runat="server" TargetControlID="rcbxDepartment" ShowEvent="OnMouseOver"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                    <asp:CustomValidator ID="cvDepartment" runat="server" ErrorMessage="请选择使用部门"
                                        ControlToValidate="rcbxDepartment" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                </div>

                            </div>
                            <div class="float-left">
                                <label>使用人</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtUsePeople" CssClass="mws-textinput" Width="200px" MaxLength="100"></telerik:RadTextBox>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>存放地点</label>
                                <div class="mws-form-item small">
                                    <telerik:RadComboBox runat="server" ID="rcbStorageLocation" Filter="Contains" AllowCustomText="true" Width="200px"
                                        MarkFirstMatch="true" Height="160px" EmptyMessage="--请选择--"  >
                                    </telerik:RadComboBox>
                                     <telerik:RadToolTip ID="RadToolTip5" runat="server" TargetControlID="rcbStorageLocation" ShowEvent="OnMouseOver"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                    <asp:CustomValidator ID="cvStorageLocation" runat="server" ErrorMessage="请选择存放地点"
                                        ControlToValidate="rcbStorageLocation" ValidationGroup="vgMaintenance" Display="Dynamic"
                                        Text="*" CssClass="field-validation-error">
                                    </asp:CustomValidator>
                                </div>

                            </div>
                            <div class="float-left">
                                <label>折旧方法</label>
                                <div class="mws-form-item small">
                                    <label>直线折旧法</label>
                                </div>
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>原值</label>
                                <div class="mws-form-item small">
                                    <telerik:RadNumericTextBox ShowSpinButtons="true" MinValue="0" IncrementSettings-InterceptArrowKeys="true"
                                        IncrementSettings-InterceptMouseWheel="true"
                                        Label="" runat="server" ID="txtOriginalValue" Width="160px">
                                    </telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="rfvFee" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtOriginalValue"
                                        ErrorMessage="原值必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                </div>

                            </div>
                            <div class="float-left">

                                <label>开始使用时间</label>
                                <div class="mws-form-item small">
                                    <telerik:RadDatePicker ID="txtStartUsedDate" Width="200px" runat="server">
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="rfvSendDate" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtStartUsedDate"
                                        ErrorMessage="开始使用时间必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                    <telerik:RadToolTip ID="rttSendDate" runat="server" TargetControlID="txtStartUsedDate" ShowEvent="OnMouseOver"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                </div>
                            </div>
                        </div>

                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>预计净残值</label>
                                <div class="mws-form-item small">
                                    <telerik:RadNumericTextBox ShowSpinButtons="true" MinValue="0" IncrementSettings-InterceptArrowKeys="true"
                                        IncrementSettings-InterceptMouseWheel="true"
                                        Label="" runat="server" ID="txtEstimateNetSalvageValue" Width="160px">
                                    </telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtEstimateNetSalvageValue"
                                        ErrorMessage="预计净残值必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                    <telerik:RadToolTip ID="RadToolTip3" runat="server" TargetControlID="txtEstimateNetSalvageValue" ShowEvent="OnMouseOver"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                </div>

                            </div>
                            <div class="float-left">

                                <label>预计使用年限</label>
                                <div class="mws-form-item small">
                                    <telerik:RadNumericTextBox ShowSpinButtons="true" MinValue="0" IncrementSettings-InterceptArrowKeys="true"
                                        IncrementSettings-InterceptMouseWheel="true"
                                        Label="" runat="server" ID="txtEstimateUsedYear" Width="160px">
                                    </telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="vgMaintenance" ControlToValidate="txtEstimateUsedYear"
                                        ErrorMessage="预计使用年限必填" Text="*" CssClass="field-validation-error">
                                    </asp:RequiredFieldValidator>
                                    <telerik:RadToolTip ID="RadToolTip2" runat="server" TargetControlID="txtEstimateUsedYear" ShowEvent="OnMouseOver"
                                        Position="MiddleRight" RelativeTo="Element" Text="该项是必填项" AutoCloseDelay="0">
                                    </telerik:RadToolTip>
                                </div>
                            </div>
                        </div>

                        <div class="mws-form-row">
                            <div class="float-left width40-percent">
                                <label>累计折旧</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtAllDepreciation" CssClass="mws-textinput" Width="200px" MaxLength="100" Enabled="false"></telerik:RadTextBox>

                                </div>

                            </div>
                            <div class="float-left">

                                <label>净值</label>
                                <div class="mws-form-item small">
                                    <telerik:RadTextBox runat="server" ID="txtNetValue" CssClass="mws-textinput" Width="200px" MaxLength="100" Enabled="false"></telerik:RadTextBox>
                                </div>
                            </div>
                        </div>

                        <div class="mws-form-row">
                            <label>备注</label>
                            <div class="mws-form-item medium">
                                <telerik:RadTextBox runat="server" ID="txtRemark" Width="90%" MaxLength="1000"
                                    TextMode="MultiLine" Height="80">
                                </telerik:RadTextBox>
                            </div>
                        </div>
                        <div class="height20"></div>
                    </div>
                    <div class="mws-button-row">
                        <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="mws-button green" CausesValidation="true" ValidationGroup="vgMaintenance" OnClick="btnSave_Click" />
                        <asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="mws-button orange" CausesValidation="false" OnClick="btnDelete_Click" OnClientClick="return onConfirmDelete();" />
                        <asp:Button ID="btnCancel" runat="server" Text="取消" UseSubmitBehavior="false" CssClass="mws-button green" OnClientClick="redirectToPage('Views/Basics/FixedAssetsManagement.aspx');return false;" />
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script type="text/javascript">

        function onClientHidden(sender, args) {
            redirectToPage("Views/Basics/FixedAssetsManagement.aspx");
        }

    </script>
</asp:Content>
