<%@ Page Language="C#" MasterPageFile="~/Course.Master" AutoEventWireup="true" CodeBehind="MyCert.aspx.cs" Inherits="MobileEMS.MyCert" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="ctl00_left_nav_pnlLast">
        <a href="#" class="link-back" id="Previous"><i class="icon-back"></i></a>
    </div>
    <div id="ctl00_left_nav_pnlUserCenter">
        <a href="javascript:;" class="link-myinfo">
        <i id="userImgHead"><img src="images/31959858.jpg" width="23" height="23"></i></a>
    </div>
    <asp:Label ID="CenterTitle" runat="server" Text="Label"></asp:Label>
    <!--<a href="Cart.aspx" class="link-cart"><i class="icon-cart"></i></a>-->
    <a href="Search.aspx?Action=<%=Action %>" class="link-search"><i class="icon-search-s"></i></a>
    <!--<a href="javascript:;" class="link-menu"><i class="icon-menu"></i></a>-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="content" style="min-height: 581px;">    
	    <section>		
		    <article id="courseList" class="course_list">
            
            </article>
		    <div id="loadMore" class="load_more" style="display: none;"></div>
	    </section>
    </div>
    <div id="layerMask" class="mask"></div>
    <input type="hidden" id="questType" value="<%=Action %>">
    <input type="hidden" id="hidIsCoupon" value="0"> 
    <input type="hidden" id="hidSelecttype" value="5">
    <input type="hidden" id="hHideNav" value="1">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
<script id="cert-tmpl" type="text/tmpl">
    {{?it.length>0}}
    {{~it:v:i}}
   		<div class="cert">
            <img src="http://ems.mostool.com/{{=v.CertPath}}"><br />{{=v.PostName}}
		</div>
    {{~}}
    {{??}}
        <div class="cert">
            暂没有获取到证书，请加油哦~~
		</div>
    {{?}}
</script>
<script src="js/course.js" type="text/javascript"></script>
</asp:Content>

