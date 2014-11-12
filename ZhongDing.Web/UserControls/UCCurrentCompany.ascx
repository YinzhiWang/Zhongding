<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCurrentCompany.ascx.cs" Inherits="ZhongDing.Web.UserControls.UCCurrentCompany" %>

<div>
    <label>当前账套</label>
    <div class="mws-form-item small">
        <label><%=this.CurrentUser.CompanyName %></label>
    </div>
</div>
