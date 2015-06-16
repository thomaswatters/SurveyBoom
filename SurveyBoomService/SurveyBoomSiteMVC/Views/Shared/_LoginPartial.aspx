<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<% if (Request.IsAuthenticated) { %>
    <%var ws = new SurveyBoomSiteMVC.net.azurewebsites.surveyboom.SurveyBoomService(); %>
    <%int id = ws.GetUserID(User.Identity.Name); %>
    <%: Html.ActionLink("Details", "Details", "User", new { id = id }, htmlAttributes: new { title = "Manage" }) %>
    Hello, <%: Html.ActionLink(User.Identity.Name, "Manage", "Account", routeValues: null, htmlAttributes: new { @class = "username", title = "Manage" }) %>!
    <% using (Html.BeginForm("LogOff", "Account", FormMethod.Post, new { id = "logoutForm" })) { %>
        <%: Html.AntiForgeryToken() %>
        <a href="javascript:document.getElementById('logoutForm').submit()">Log off</a>
    <% } %>
<% } else { %>
    <ul>
        <li><%: Html.ActionLink("Register", "Register", "Account", routeValues: null, htmlAttributes: new { id = "registerLink" })%></li>
        <li><%: Html.ActionLink("Log in", "Login", "Account", routeValues: null, htmlAttributes: new { id = "loginLink" })%></li>
    </ul>
<% } %>