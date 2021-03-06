﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCHeader.ascx.cs" Inherits="ZhongDing.Web.UserControls.UCHeader" %>
<!-- Header Wrapper -->
<div id="mws-header" class="clearfix">

    <!-- Logo Wrapper -->
    <div id="mws-logo-container">
        <span style="font-size: 32px; color: #c5d52b; font-weight: bold;">众鼎</span> <span style="font-size: 15px; color: whitesmoke; font-weight: bold;">医药咨询信息系统</span>
    </div>

    <!-- User Area Wrapper -->
    <div id="mws-user-tools" class="clearfix">

        <div id="mws-other-info" class="mws-dropdown-menu">
            <div class="float-left">
                <a class="mws-i-24 i-file-cabinet mws-dropdown-trigger"></a>
            </div>
            <div id="mws-user-company" title="当前账套">
                <%=this.CurrentUser.CompanyName %>
            </div>
        </div>
        <!-- User Functions -->
        <div id="mws-user-info" class="mws-inset">

            <div id="mws-user-photo">
                <img runat="server" id="userAvatar" src="~/Images/defaultAvatar.gif" alt="User Photo" />
            </div>
            <div id="mws-user-functions">
                <div id="mws-username">
                    您好, <%=this.CurrentUser.FullName %>
                </div>
                <ul>
                    <li><a href="#">修改个人信息</a></li>
                    <li><a href="#">修改密码</a></li>
                    <li><a runat="server" id="linkLogout" href="~/Account/Logout.aspx">退出</a></li>
                </ul>
            </div>
        </div>
        <!-- End User Functions -->

    </div>
</div>
