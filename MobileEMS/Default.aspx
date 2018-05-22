<%@ Page Language="C#" MasterPageFile="~/Course.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MobileEMS.Default" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="ctl00_left_nav_pnlUserCenter">		
             <a href="javascript:;" class="link-myinfo" onclick="SendEvent(25, 203)"><i id="userImgHead"><img src="images/31959858.jpg" width="23" height="23"></i></a>
            </div>
            我的班级
            <a href="Cart.aspx" class="link-cart"><i class="icon-cart"></i></a>
            <a href="#" class="link-search" onclick="SendEvent(25, 200)"><i class="icon-search-s"></i></a>
            <a href="javascript:;" class="link-menu" onclick="SendEvent(25, 202)"><i class="icon-menu"></i></a>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
<div class="wrapper"><div id="emptyTip" class="pt40">您还没有开通课程哦~ 去看看 <a href="CourseCenter.aspx" class="green">岗位计划</a></div></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <script id="course-tmpl" type="text/tmpl">
        {{~it:v:i}}
            <div class="ke">
                <a href="/{{=v.ClassID}}" class="ke_info">
                    <img src="{{=v.CoverUrl}}" alt="{{=v.ClassName}}" class="cover" onerror="javascript:this.src='/images/common_course_bigblank@2x.png';">
                    <div class="tit">{{=v.ShortName}}</div>
                    <p>进度：<span class="percentage">{{=v.StudyRate }}%</span><span class="progressBar"><i class="myporgress" style="width: {{=v.StudyRate }}%;"></i></span></p>
                    <div class="desc">{{=v.LessonNum}}课时  距毕业{{=v.ToGraduate}}天</div>
                </a>
            </div>
        {{~}}
    </script>
</asp:Content>
