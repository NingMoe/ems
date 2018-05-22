$(function() {

    var UserCenter = function() {
        this.isLoading = false;
        this.curPageIndex = 1;
        this.isLoading = false;
        this.pageCount = 1;
        this.doCarded = false;
        this.Dom = {
            win: $(window),
            container: $('.wrapper'),
            loadMore: $('#loadMore'),
            userCenter: $('.user_center'),
            courseList: $('#courseList'),
            userImgHead: $('#userImgHead img')
        };
    };

    UserCenter.prototype = {
        init: function() {
            this.renderUserCenter();
            this._initEvent();
        },
        _initEvent: function() {
            var self = this;

            $(document).delegate(".btnCard", "touchstart", function(){
                !self.doCarded && self.doCard();
            });
        },
        doCard: function(){
            var self = this,
                url = "/handler/UserHandler.ashx",
                $btnCard = $(".btnCard");

            self.doCarded = true;
            $.ajax({
                type: "POST",
                url: url,
                data: {
                    action: "PunchCard"
                },
                dataType: "json",
                success: function(data) {
                    if (data.IsSuccess) {
                        var xb = JSON.parse(data.Content).xb;
                        
                        $btnCard.after('<span class="carded-txt"><i class="icon-carded"></i>已打卡</span>').remove();
                        $("#xb-data").html(xb);
                    }
                    self.doCarded = false;
                }
            });
        },
        renderUserCenter: function() {
            var self = this,
                url = "/handler/UserHandler.ashx";

            $.ajax({
                type: "POST",
                url: url,
                data: {
                    action: "GetUserCenter"
                },
                dataType: "html",
                success: function(data) {
                    var data = JSON.parse(data),
                        userImgHead = "";

                    if (data.IsSuccess) {
                        data = JSON.parse(data.Content),
                        data.logined = true;
                        userImgHead = data.Favicon;
                    } else {
                        data.logined = false;
                        userImgHead = "/images/anonymous.png";
                    }
                    
                    data.backUrl = location.href;
                    html = getTemplate(data, "usercenter-tmpl");
                    self.Dom.userImgHead.attr("src", userImgHead);
                    self.Dom.userCenter.append(html);
                }
            });
        }
    };

    var userCenter = new UserCenter();
    userCenter.init();
});