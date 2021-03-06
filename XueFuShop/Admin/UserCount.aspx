﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/MasterPage.Master" CodeBehind="UserCount.aspx.cs" Inherits="XueFuShop.Admin.UserCount" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">	
<div class="position"><img src="Style/Images/PositionIcon.png"  alt=""/>会员分析</div>	
<div class="listBlock">
    <ul>
        <li class="listOn"><a href="UserCount.aspx">数量分析</a></li>
        <li><a href="UserStatus.aspx">状态分析</a></li>
        <li><a href="UserActive.aspx">活跃度分析</a></li>
        <li><a href="UserConsume.aspx">消费分析</a></li>      
    </ul>	
</div>
<div class="pageMark">	
    <div class="statisticsSearch">
		    注册时间：<asp:DropDownList ID="Year" runat="server"></asp:DropDownList> <asp:CompareValidator ID="CheckYear" runat="server"  ControlToValidate="Year" Display="Dynamic" ErrorMessage="* 请选择年份" Operator="NotEqual" ValueToCompare="0"></asp:CompareValidator>  年 
		    <asp:DropDownList ID="Month" runat="server"></asp:DropDownList> 月 <asp:Button CssClass="button" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" />
    </div>
    <object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0" width="700" height="220" id="Object1" > 
	    <param name="movie" value="Flash/MSLine.swf?ChartNoDataText=%E6%B2%A1%E6%9C%89%E5%8F%AF%E6%98%BE%E7%A4%BA%E7%9A%84%E6%95%B0%E6%8D%AE&PBarLoadingText=%E6%AD%A3%E5%9C%A8%E8%BD%BD%E5%85%A5%E5%9B%BE%E8%A1%A8%EF%BC%8C%E8%AF%B7%E7%A8%8D%E5%80%99&XMLLoadingText=%E6%AD%A3%E5%9C%A8%E8%8E%B7%E5%8F%96%E6%95%B0%E6%8D%AE%EF%BC%8C%E8%AF%B7%E7%A8%8D%E5%80%99&ParsingDataText=%E6%AD%A3%E5%9C%A8%E8%AF%BB%E5%8F%96%E6%95%B0%E6%8D%AE%EF%BC%8C%E8%AF%B7%E7%A8%8D%E5%80%99&RenderingChartText=%E6%AD%A3%E5%9C%A8%E6%B8%B2%E6%9F%93%E5%9B%BE%E8%A1%A8%EF%BC%8C%E8%AF%B7%E7%A8%8D%E5%80%99&LoadDataErrorText=%E8%BD%BD%E5%85%A5%E6%95%B0%E6%8D%AE%E6%97%B6%E5%8F%91%E7%94%9F%E9%94%99%E8%AF%AF&InvalidXMLText=无效的XML数据" /> 
	    <param name="FlashVars" value="&dataURL=UserCountData.aspx<%=queryString %>" /> 
	    <param name="quality" value="high" /> 
	    <param name="wmode" value="transparent" /> 
	    <embed src="Flash/MSLine.swf?ChartNoDataText=%E6%B2%A1%E6%9C%89%E5%8F%AF%E6%98%BE%E7%A4%BA%E7%9A%84%E6%95%B0%E6%8D%AE&PBarLoadingText=%E6%AD%A3%E5%9C%A8%E8%BD%BD%E5%85%A5%E5%9B%BE%E8%A1%A8%EF%BC%8C%E8%AF%B7%E7%A8%8D%E5%80%99&XMLLoadingText=%E6%AD%A3%E5%9C%A8%E8%8E%B7%E5%8F%96%E6%95%B0%E6%8D%AE%EF%BC%8C%E8%AF%B7%E7%A8%8D%E5%80%99&ParsingDataText=%E6%AD%A3%E5%9C%A8%E8%AF%BB%E5%8F%96%E6%95%B0%E6%8D%AE%EF%BC%8C%E8%AF%B7%E7%A8%8D%E5%80%99&RenderingChartText=%E6%AD%A3%E5%9C%A8%E6%B8%B2%E6%9F%93%E5%9B%BE%E8%A1%A8%EF%BC%8C%E8%AF%B7%E7%A8%8D%E5%80%99&LoadDataErrorText=%E8%BD%BD%E5%85%A5%E6%95%B0%E6%8D%AE%E6%97%B6%E5%8F%91%E7%94%9F%E9%94%99%E8%AF%AF&InvalidXMLText=无效的XML数据" flashVars="&dataURL=UserCountData.aspx<%=queryString %>" quality="high" wmode="transparent" width="700" height="220" name="MSLine" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" /> 
	</object> 
	<div  class="note">
	    <ul>
	        <li class="title">说明</li>
	        <li>1、横坐标表示选择时间内的月或者日，纵坐标表示注册会员数量。</li>
	        <li>2、选择月份则表示该月每天注册的人数，否则就就表示该年每月注册的人数。</li>
	        <li>3、会员是指所有状态的会员。</li>
	    </ul>
	</div>
</div>
</asp:Content>
