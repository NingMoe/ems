<%@ Page Title="" Language="C#" MasterPageFile="~/Course.Master" AutoEventWireup="true" CodeBehind="RCDetail.aspx.cs" Inherits="MobileEMS.RCDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="ctl00_left_nav_pnlLast">		
        <a href="javascript:history.go(-1);" class="link-back" id="Previous"><i class="icon-back"></i></a>
    </div>
	<asp:Label ID="CenterTitle" runat="server" Text="Label"></asp:Label>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <iframe src="http://ems.mostool.com/Plugins/Template/MOShop/pdfjs/web/viewer.html?file=<%=XueFu.EntLib.RequestHelper.GetQueryString<string>("Url") %>" width="100%" height="100%"></iframe>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <script>
        function setContentH() {
            var clientH = document.documentElement.clientHeight > document.body.clientHeight ? document.documentElement.clientHeight : document.body.clientHeight;
            var $container = $("iframe"),
                fixH = parseInt($("#ctl00_pnlTemp").height()) + parseInt($("footer").height()),
                maxH = clientH - fixH;
                $container.css({ "height": maxH });
                console.log(clientH);
        }
        $(function () {
        setContentH();
        })
    </script>
</asp:Content>
