<%@ Page Title="首页" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="ZhongDing.Web.Home" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">

        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">帐套管理</span>
            </div>
            <div class="mws-panel-body">
                <div class="mws-form">
                    <div class="mws-form-inline">
                        <div class="mws-form-row">
                            <label>帐套编号</label>
                            <div class="mws-form-item small">
                                <input type="text" name="fullname" class="mws-textinput" />
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>帐套名称</label>
                            <div class="mws-form-item small">
                                <input type="text" name="email" class="mws-textinput" />
                            </div>
                        </div>


                        <div class="mws-form-row">
                            <label style="font-weight: bold">发票税点维护</label>
                            <div class="mws-form-item small">
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>供应商发票返点</label>
                            <div class="mws-form-item small">
                                <input type="text" name="supplierper" class="mws-textinput" style="width: 20%" />
                                %
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label style="font-weight: bold">客户发票返点</label>
                            <div class="mws-form-item small">
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>高开税率</label>
                            <div class="mws-form-item small">
                                <input type="text" name="clienthighper" class="mws-textinput" style="width: 20%" />
                                %
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>低开税率</label>
                            <div class="mws-form-item small">
                                <input type="text" name="clientlowper" class="mws-textinput" style="width: 20%" />
                                %
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>平进平出税率</label>
                            <div class="mws-form-item small">
                                <input type="text" name="clientlowper" class="mws-textinput" style="width: 20%" />
                                %   
                                         &nbsp;&nbsp;&nbsp;&nbsp;启用
                                            <input type="checkbox" name="enable" />
                            </div>
                            <div>
                                <label></label>


                            </div>
                        </div>
                    </div>
                    <div class="mws-button-row">

                        <input type="submit" value="保存" class="mws-button green" />
                        <input type="submit" value="删除" class="mws-button green" />
                        <input type="submit" value="取消" class="mws-button green" />
                    </div>
                </div>
            </div>
        </div>

    </div>

    <div class="container">

        <div class="mws-panel grid_8">
            <div class="mws-panel-header">
                <span class="mws-i-24 i-sign-post">帐套管理</span>
            </div>
            <div class="mws-panel-body">
                <div class="mws-form">
                    <div class="mws-form-inline">
                        <div class="mws-form-row">
                            <label>帐套编号</label>
                            <div class="mws-form-item small">
                                <input type="text" name="fullname" class="mws-textinput" />
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>帐套名称</label>
                            <div class="mws-form-item small">
                                <input type="text" name="email" class="mws-textinput" />
                            </div>
                        </div>


                        <div class="mws-form-row">
                            <label style="font-weight: bold">发票税点维护</label>
                            <div class="mws-form-item small">
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>供应商发票返点</label>
                            <div class="mws-form-item small">
                                <input type="text" name="supplierper" class="mws-textinput" style="width: 20%" />
                                %
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label style="font-weight: bold">客户发票返点</label>
                            <div class="mws-form-item small">
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>高开税率</label>
                            <div class="mws-form-item small">
                                <input type="text" name="clienthighper" class="mws-textinput" style="width: 20%" />
                                %
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>低开税率</label>
                            <div class="mws-form-item small">
                                <input type="text" name="clientlowper" class="mws-textinput" style="width: 20%" />
                                %
                            </div>
                        </div>
                        <div class="mws-form-row">
                            <label>平进平出税率</label>
                            <div class="mws-form-item small">
                                <input type="text" name="clientlowper" class="mws-textinput" style="width: 20%" />
                                %   
                                         &nbsp;&nbsp;&nbsp;&nbsp;启用
                                            <input type="checkbox" name="enable" />
                            </div>
                            <div>
                                <label></label>


                            </div>
                        </div>
                    </div>
                    <div class="mws-button-row">

                        <input type="submit" value="保存" class="mws-button green" />
                        <input type="submit" value="删除" class="mws-button green" />
                        <input type="submit" value="取消" class="mws-button green" />
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
</asp:Content>
