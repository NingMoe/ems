<%@ Page Language="C#" MasterPageFile="~/Course.Master" AutoEventWireup="true" CodeBehind="CourseVideo.aspx.cs" Inherits="MobileEMS.CourseVideo" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="ctl00_left_nav_pnlLast">
        <a href="#" class="link-back" id="Previous"><i class="icon-back"></i></a>
    </div>
    <div id="ctl00_left_nav_pnlUserCenter">
        <a href="javascript:;" class="link-myinfo"><i id="userImgHead">
            <img src="images/31959858.jpg" width="23" height="23"></i></a>
    </div>
    课程中心            
    <!--<a href="Cart.aspx" class="link-cart"><i class="icon-cart"></i></a>-->
    <a href="Search.aspx" class="link-search"><i class="icon-search-s"></i></a>
    <!--<a href="javascript:;" class="link-menu"><i class="icon-menu"></i></a>-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="content" style="min-height: 581px;">
        <nav class="top_nav">
            <div class="nav">
                <div id="courseNav" class="first_nav">
                    <i class="icon-all_nav"></i>视频列表
                </div>
                <%if (!String.IsNullOrEmpty(product.Accessory))
                  {%><div id="orderBtn" class="order_nav"><a href="TestCenter.aspx?CateId=<%=CateId %>">开始考试</a></div>
                <%} %>
                <div id="courseNavWrapper" class="second_nav">
                    <div class="courseNavCon">
                        <ul id="courseNavList" class="second_nav_list cf">
                            <%int i = 0; %>
                            <%foreach (string code in product.ProductNumber.Split('|'))
                              { %>
                            <li data-vid="<%=code %>">
                                <div>
                                    <%--class="cateItem"--%>
                                    <span class="name"><a href="javascript:void(0)">第<%=i+1%>集</a></span>
                                </div>
                            </li>
                            <%i++;
                              } %>
                        </ul>
                    </div>
                </div>
            </div>
        </nav>
        <script src='//player.polyv.net/script/polyvplayer.min.js'></script>
        <div class="playContent" data-code="<%=userID%>_<%=checkCode%>">
            <%--<iframe src='' id='loadvideo' frameborder='0' scrolling='no' width='100%' height='400'></iframe>--%>
        </div>
        <div class="nav_list" style="background-color: #40474d; padding: 10px 0 5px; text-align: center;">
            <%i = 0; %>
            <%foreach (string code in product.ProductNumber.Split('|'))
              { %>
            <span data-vid="<%=code %>" class="sub_cate" style="width: 20%;">第<%=i+1%>集</span>
            <%i++;
              } %>
            <div class="clearbox"></div>
        </div>
    </div>
    <div id="layerMask" class="mask"></div>
    <input type="hidden" id="hidCateID" value="<%=CateId %>">
    <input type="hidden" id="hidIsCoupon" value="0">
    <input type="hidden" id="VideoListId" value="<%=VideoListId %>">
    <input type="hidden" id="hidSelecttype" value="3">
    <input type="hidden" id="hHideNav" value="1">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <script id="videolist-tmpl" type="text/tmpl">
    {{~it.VideoID:v:i}}
    <li>
    	<div class="cateItem">
			<span class="name"><a href="?VideoListId={{=i}}&CateId=<%=CateId %>">第{{=i+1}}集</a></span>
		</div>
    </li>
    {{~}}
    </script>
    <script type="text/javascript">
        var player = polyvObject('.playContent').videoPlayer({
            'width': '100%',
            'height': '0',
            'vid': '<%=videoDic["vid"]%>',
            'code': $('.playContent').data("code"),
            'ts': '<%=videoDic["ts"]%>',
            'sign': '<%=videoDic["sign"]%>'
        });
        $(function () {
            $("#courseNavList li,.nav_list span").click(function (event) {
                $.get("/Ajax.aspx?Action=ChangeVideo&vid=" + $(this).data("vid"), function (data) {
                    data = JSON.parse(data);
                    try { player.changeVid(data.vid, 0, "on", data.ts, data.sign); }
                    catch (err) { player.changeVid(data.vid); }
                })
                $('#courseNav').html('<i class="-small"></i>' + $(this).text());
                $(this).addClass("currentquestion").siblings().removeClass("currentquestion");
            });
            $('#courseNav').html('<i class="-small"></i>' + $("#courseNavList li").eq(0).text());
            $(".nav_list span").eq(0).addClass("currentquestion");
        })
    </script>
    <script src="js/course.js?v=01.007" type="text/javascript"></script>
</asp:Content>
