<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="XueFuShop.Admin.Default" %>
<%@ Import Namespace="XueFuShop.Common" %>
<%@ Import Namespace="XueFuShop.BLL" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=ShopConfig.ReadConfigInfo().Title%>管理平台</title>
    <META http-equiv=Content-Type content="text/html; charset=utf-8">
    <script language="javascript" type="text/javascript" src="Js/Common.js"></script>
    <script language="javascript" type="text/javascript" src="Js/Admin.js"></script>
    <script language="javascript" type="text/javascript" src="Js/ScrollText.js"></script>
    <script type="text/javascript" src="Pop/lhgcore.min.js"></script>
    <script type="text/javascript" src="Pop/lhgdialog.min.js?s=chrome"></script>	
    <link href="Style/style.css" type="text/css" rel="stylesheet" media="all" />
</head>
<body>
    <form id="form1" runat="server">
    <DIV class=head>
        <div class="top">
	        <div class="logo"><img src="Style/Images/logo2.png" /></div>
            <div class="welcome">
                <%=Cookies.Admin.GetAdminName(false)%>：您好，
                <script language="javascript" type="text/javascript">
                var today=new Date();
                document.write("今天："+today.getFullYear()+ "年"+(today.getMonth()+1)+"月"+today.getDate()+"日，");
                </script>
                权限组：<%=AdminGroupBLL.ReadAdminGroupCache(Cookies.Admin.GetGroupID(false)).Name%>
               <a href="javascript:goUrl('ChangePassword.aspx')" >修改密码</a>
               <a href="Logout.aspx" target="_top">安全退出</a>
           </div>                             
	    </div>
        <ul class="channel">
            <li class="on" id="aCommon"><a href="javascript:switchBlock('Common','Left.aspx?ID=1','Right.aspx')">基础设置</a></li>
            <li ><img src="Style/Images/channelPadding.gif" alt="" /></li>
            <li id="aProduct"><a href="javascript:switchBlock('Product','Left.aspx?ID=2','Right.aspx')">课程管理</a></li>
            <li><img src="Style/Images/channelPadding.gif"  alt=""/></li>
            <li id="aUser"><a href="javascript:switchBlock('User','Left.aspx?ID=3','Right.aspx')">用户中心</a></li>
            <%--<li><img src="Style/Images/channelPadding.gif"  alt=""/></li>
            <li id="aMarket"><a href="javascript:switchBlock('Market','Left.aspx?ID=4','Right.aspx')">市场营销</a></li>--%>
            <li><img src="Style/Images/channelPadding.gif"  alt=""/></li>
            <li id="aTPR"><a href="javascript:switchBlock('TPR','Left.aspx?ID=109','Right.aspx')">TPR管理</a></li>
            <li><img src="Style/Images/channelPadding.gif"  alt=""/></li>
            <li id="aOrder"><a href="javascript:switchBlock('Order','Left.aspx?ID=5','Right.aspx')">订单与统计</a></li>	         
        </ul>
    <div class="menu">
	        <div class="text">公告：</div><script language="javascript" src="" type="text/javascript"></script>
	        <ul>
	            <li><a href="/Help-I28.aspx" target="_blank">帮助中心</a></li>	   
	            <li><img src="Style/Images/menuPadding.gif"  alt=""/></li>
	            <li><a href="/" target="_blank" >网站主页</a></li>
	            <li><img src="Style/Images/menuPadding.gif"  alt=""/></li>
	            <li><a href="javascript:goUrl('Menu.aspx')" >菜单设置</a></li>
	            <li><img src="Style/Images/menuPadding.gif"  alt=""/></li>
	            <li><a href="javascript:goUrl('HardDisk.aspx')" >共享硬盘</a></li>
	            <li><img src="Style/Images/menuPadding.gif"  alt=""/></li>
	            <li><a href="javascript:popPageOnly('NoteBook.aspx',500,280,'记事本','NoteBook')"/>记事本</a></li>
	            <li><img src="Style/Images/menuPadding.gif"  alt=""/></li>
	            <li><a href="javascript:popPageOnly('SendEmail.aspx',600,450,'发邮件','SendEmail')"/>发邮件</a></li>
	        </ul>
	    </div>
    </DIV>
    <ul class="body" id="Body">
	    <li class="leftFrame"><iframe src="Left.aspx" height="100%" frameborder="0" id="LeftFrame"></iframe></li>
	    <li class="rightFrame"><iframe src="Right.aspx" height="100%" width="100%" frameborder="0" id="RightFrame"></iframe></li>
	</ul>
	</form>
	<script language="javascript" type="text/javascript" src="Js/Default.js"></script>
</body>
</html>
