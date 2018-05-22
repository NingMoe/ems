<%@ Page Language="C#" MasterPageFile="~/Course.Master" AutoEventWireup="true" CodeBehind="TestPaperShow.aspx.cs" Inherits="MobileEMS.TestPaperShow" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="ctl00_left_nav_pnlLast">
        <a href="CourseCenter.aspx?Action=PostCourse" class="link-back" id="Previous"><i class="icon-back"></i></a>
    </div>
    <div id="ctl00_left_nav_pnlUserCenter">
        <a href="javascript:;" class="link-myinfo"><i id="userImgHead"><img src="images/31959858.jpg" width="23" height="23"></i></a>
    </div>
    成绩榜
    <a href="#" class="link-search"><i class="icon-search-s"></i></a>            
    <!--<a href="javascript:;" class="link-menu"><i class="icon-menu"></i></a>-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="content" style="min-height: 581px;">
        <div class="wrapper" runat="server" id="paperContent"><div id="emptyTip" class="pt40">您还没有开通课程哦~ 去看看 <a href="CourseCenter.aspx" class="green">岗位计划</a></div></div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

</asp:Content>
