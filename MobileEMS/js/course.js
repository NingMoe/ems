(function () {
    var PAGE_NUM = 20,
		orderBy = 10,
		// cateId = getQueryString("cate") || 0,
        cateId = $('#hidCateID').val(),
		isCoupon = $('#hidIsCoupon').val(),
		$loadMore = $('#loadMore'),
		$courseList = $('#courseList'),
		$mask = $('#layerMask'),
		$courseNavWrapper = $('#courseNavWrapper'),
		$courseNav = $('#courseNav'),
		$selectType = $('#hidSelecttype'),
		$courseNavList = $('#courseNavList'),
		VideoListId = $('#VideoListId').val(),
		winHeight = $(window).height(),
		$firstNavList,
		pageIndex = 1,
		loading = true,
		isLastPage = false,
		$footer = $('footer'),
		myScroll = null;

    var DISCOUNT_CLASS = ['', 'discount_1', 'discount_1', 'discount_3', 'discount_4', '', 'discount_6', 'discount_7', 'discount_8'];
    var getCourseList = function () {
        $loadMore.html('<i class="loading"></i>加载中...').show();

        $.wsajax(
			"CourseAjax.aspx",
			"GetCoursesByFcate", {
			    classID: cateId,
			    pageIndex: pageIndex,
			    pageSize: PAGE_NUM,
			    orderBy: orderBy,
			    isCoupon: isCoupon
			},
			function (data) {
			    isLastPage = true;
			    loading = false;
			    $loadMore.html('').hide();
			    $footer.removeClass('footer_b');
			    var html = '';
			    if (!data && pageIndex === 1) {
			        html = '<div class="no_course">' +
						'<div class="no_course_text">该分类下还没有课程哦~</div>' +
						'<div class="no_course_cc"></div>' +
						'<div id="backAllCourse" class="back_all">查看全部课程</div>' +
						'</div>';

			    }
			    else if (data) {
			        var list = JSON.parse(data);
			        //for (var i = 0, len = list.length; i < len; i++) {
			        //						if (DISCOUNT_CLASS[list[i].SaleEventType]) {
			        //							list[i].discount_class = DISCOUNT_CLASS[list[i].SaleEventType];
			        //						}
			        //
			        //					}
			        html = getTemplate(list, "course-tmpl");
			        if (list.length > 0 && pageIndex < list[0].PageCount) {
			            $loadMore.hide();
			            isLastPage = false;
			        }
			    }

			    if (pageIndex === 1) {
			        $courseList.html(html);
			    }
			    else {
			        $courseList.append(html);
			    }
			    // if ($courseList.height() + 189 < $(window).height()) {
			    // 	$footer.addClass('footer_b');
			    // }

			}
		);
    };

    function getQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r !== null) return unescape(r[2]);
        return null;
    }

    function showCourseNav() {
        $("#container").css({ "height": "100%" });
        $courseNav.addClass("show");
        $mask.show();
        $courseNavWrapper.show();
        setContentH();
        if ($(".playContent div iframe").length > 0) {
            $(".playContent div iframe").hide();
        }
    }

    function hideCourseNav() {
        $courseNav.removeClass("show");
        $mask.hide();
        $courseNavWrapper.hide();
        $("#container").css({ "height": "auto" });
        if ($(".playContent div iframe").length > 0) {
            $(".playContent div iframe").show();
        }
    }

    function touchMoveHandler(e) {
        e.preventDefault();
    }

    function setContentH() {
        var clientH = document.body.clientHeight,
			$container = $("#courseNavWrapper"),
			fixH = parseInt($("header").height()) + parseInt($(".top_nav").height()),
			contentH = fixH + parseInt($(".courseNavCon").height()),
			maxH = clientH - fixH;

        if (contentH > clientH) {
            $container.css({ "height": maxH });
        } else {
            $container.css({ "height": parseInt($(".courseNavCon").height()) + 25 });
        }
        //myScroll.refresh();
    }

    var addIscroll = function () {
        myScroll = new IScroll('#courseNavWrapper', {
            scrollbars: true,
            mouseWheel: true,
            interactiveScrollbars: true,
            shrinkScrollbars: 'scale',
            fadeScrollbars: true
        });
    };

    var initPage = function () {
        getCourseList();
        if (pageIndex == 1) {
            GetFcate();
        }
    };

    var GetFcate = function () {
        $.wsajax(
			"CourseAjax.aspx",
			"GetFcate", {
			    encryptFcateID: $('#hidePostId').val(),
			    questType: $('#questType').val()
			},
			function (data) {
			    var list = JSON.parse(data),
					html = getTemplate(list, "catelist-tmpl");
			    $courseNavList.html(html);

			    $firstNavList = $('#courseNavList').children('li');
			    if ($("#questType").val() != "") $courseNav.html('<i class="icon-all_nav"></i>全部课程');
			    else $courseNav.html('<i class="icon-all_nav"></i>所有岗位课程');
			    if (cateId) {
			        var j = 0,
						i = 0,
						arr;
			        for (i = 0, len = list.length; i < len; i++) {
			            arr = list[i].ChildCourseFCateView;
			            for (j = 0, l = arr.length; j < l; j++) {
			                if (arr[j].EncryptFcateID == cateId) {
			                    $courseNav.html('<i class="' + list[i].Icon + '-small"></i>' + arr[j].Title);
			                }
			            }
			        }
			    }
			    // addIscroll();
			    var needToHideNav = $('#hHideNav').length > 0 && $('#hHideNav').val() === '1';

			    if (!cateId && !needToHideNav) {
			        showCourseNav();
			    }
			    if ($("#questType").val() != "") $courseNavList.find(".cateItem")[0].click();
			}
		);
    }

    var getQuestionsList = function () {
        $.wsajax(
			"CourseAjax.aspx",
			"GetQuestionsList",
			{
			    CateId: cateId
			},
			function (data) {
			    var list = JSON.parse(data),
				jsonlist = JSON.parse(list.QuestionsStyle),
				html = getTemplate(jsonlist, "questionlist-tmpl");
			    $courseNavList.html(html);
			    $firstNavList = $courseNavList.children('li');
			    $(".questionarea").attr("data-questionnum", list.QuestionNum);
			    $(".questionarea").html('<div style="text-align:center;"><h2>考试说明</h2>试卷总分：100分<br />题目数量：' + list.QuestionNum + '题<br />考试时长：12分钟<br />通过分数：80分<br /><a id="teststart" class="btn btnGreen" href="#">开始答题</a></div><div style="margin:50px auto;"><table style="width:200px; margin:0px auto;"><tr><td style="padding-right:10px; text-align:right;">上海加禾汽修服务<br>上海孟特管理咨询</td><td style=" border-left:1px solid #CCC; text-align: left; padding-left:10px;">联合<br>出品</td></tr></table></div>');
			    window.QuestionList = list.QuestionsList;
			    if (cateId) {
			        $courseNav.html('<i class="' + jsonlist[0].Icon + '-small"></i>' + jsonlist[0].StyleName);
			    }
			    var needToHideNav = $('#hHideNav').length > 0 && $('#hHideNav').val() === '1';

			    if (!cateId && !needToHideNav) {
			        showCourseNav();
			    }
			});
    }

    var getVideoList = function () {
        $.wsajax(
			"CourseAjax.aspx",
			"GetVideo",
			null,
			function (data) {
			    var list = JSON.parse(JSON.parse(data)),
					html = getTemplate(list, "videolist-tmpl");
			    $courseNavList.html(html);
			    var i = VideoListId;
			    //alert("http://yuntv.letv.com/bcloud.html?uu=debb2235d3&vu=" + list.VideoID[i] + "&auto_play=1&gpcflag=1&width=400&height=235&payer_name=" + list.Mobile + "&check_code=" + list.Check_Code + "&extend=0&share=0");
			    $("#loadvideo").attr("src", "http://yuntv.letv.com/bcloud.html?uu=debb2235d3&vu=" + list.VideoID[i] + "&auto_play=1&gpcflag=1&width=400&height=235&payer_name=" + list.Mobile + "&check_code=" + list.Check_Code + "&extend=0&share=0");
			    $firstNavList = $courseNavList.children('li');
			    var j = 0,
                    arr = list.VideoID;
			    for (j = 0, l = arr.length; j < l; j++) {
			        if (j == i) {
			            $courseNav.html('<i class="-small"></i>第' + (j * 1 + 1) + '集');
			        }
			    }
			    // addIscroll();
			    var needToHideNav = $('#hHideNav').length > 0 && $('#hHideNav').val() === '1';

			    if (!cateId && !needToHideNav) {
			        showCourseNav();
			    }
			}
		);
    }

    var getRecordList = function () {
        $.wsajax(
			"CourseAjax.aspx",
			"GetRecordList", {
			    pageIndex: pageIndex,
			    pageSize: 20
			},
			function (data) {
			    isLastPage = true;
			    loading = false;
			    $loadMore.html('').hide();
			    $footer.removeClass('footer_b');
			    var html = '';
			    if (!data && pageIndex === 1) {
			        html = '<div class="no_course">' +
						'<div class="no_course_text">还没有记录哦~</div>' +
						'<div class="no_course_cc"></div>' +
						'<div id="backAllCourse" class="back_all">查看课程</div>' +
						'</div>';

			    }
			    else if (data) {
			        var list = JSON.parse(data),
                        html = getTemplate(list, "record-tmpl");
			        if (list.length > 0 && pageIndex < list[0].PageCount) {
			            $loadMore.hide();
			            isLastPage = false;
			        }
			    }
			    if (pageIndex === 1) {
			        $courseList.html(html);
			    }
			    else {
			        $courseList.append(html);
			    }
			    if ($courseList.height() + 189 < $(window).height()) {
			        $footer.addClass('footer_b');
			    }
			}
		);
    }

    var getCertList = function () {
        $.wsajax(
			"CourseAjax.aspx",
			"GetCertList", {
			    pageIndex: pageIndex,
			    pageSize: 3
			},
			function (data) {
			    var list = JSON.parse(data),
					html = getTemplate(list, "cert-tmpl");
			    if (pageIndex === 1) {
			        $courseList.html(html);
			    }
			    else {
			        $courseList.append(html);
			    }
			    if ($courseList.height() + 189 < $(window).height()) {
			        $footer.addClass('footer_b');
			    }
			}
		);
    }

    var getPrepaidList = function () {
        $.wsajax(
			"CourseAjax.aspx",
			"GetPerPaidCourseList",
			null,
			function (data) {
			    var list = JSON.parse(data),
					html = getTemplate(list, "course-tmpl");
			    $courseList.html(html);
			    if ($courseList.height() + 189 < $(window).height()) {
			        $footer.addClass('footer_b');
			    }
			}
		);
    }

    var setCss = function () {
        $firstNavList.css('margin-bottom', 0).removeClass('cur');
        $('.nav_list').hide();
    };

    // SearchCourse
    var SearchCourse = function () {
        this.searchKey = "";
        this.curPageIndex = 1;
        this.isLoading = false;
        this.pageCount = 1;
        this.Dom = {
            win: $(window),
            searchInput: $("#searchInput"),
            searchBtn: $(".searchbtn"),
            suggestBox: $(".suggest-box"),
            loadMore: $('#loadMore'),
            courseList: $('#courseList')
        };
    };

    SearchCourse.prototype = {
        init: function () {
            this._initEvent();
        },
        _initEvent: function () {
            var self = this;

            self.Dom.searchBtn.on({
                click: function () {
                    self.Dom.courseList.html("");
                    self.curPageIndex = 1;
                    self.destroyRelateKeyWord();
                    self.renderSearchResult(self.curPageIndex);
                }
            });

            self.Dom.win.scroll(function () {
                var top = document.body.scrollTop,
					bodyHeight = $('body').height(),
					winHeight = $(window).height();

                if (self.Dom.courseList.find(".ke").length) {
                    if (self.isLoading || (self.curPageIndex - 1) >= self.pageCount) {
                        return false;
                    }

                    if (bodyHeight - top - winHeight <= 40) {
                        self.renderSearchResult(self.curPageIndex);
                    }
                }
            });

            self.Dom.searchInput.on({
                input: function () {
                    var value = self.getSearchKey();

                    self.destroySearchResult();
                    self.destroyRelateKeyWord();
                    if (value) {
                        self.renderRelateKeyWord();
                    }
                },
                focus: function () {
                    $("#emptyTip").hide();
                }
            });

            self.Dom.suggestBox.delegate(".suggest-keyword", click, function () {
                var $this = $(this),
					keyword = $this.attr("key");

                self.curPageIndex = 1;
                self.setSearchKey(keyword);
                self.destroyRelateKeyWord();
                self.renderSearchResult(self.curPageIndex);
            });

        },
        getSearchKey: function () {
            var key = this.Dom.searchInput.val();
            return key;
        },
        setSearchKey: function (value) {
            this.Dom.searchInput.val(value);
        },
        renderRelateKeyWord: function () {
            var self = this;

            self.searchKey = self.getSearchKey();

            $.wsajax(
				"CourseAjax.aspx",
				"GetAutoComplete",
				{
				    word: self.searchKey,
				    rowCount: 10
				},
				function (data) {
				    if (data) {
				        var list = JSON.parse(data),
							html = getTemplate(list, "suggest-tmpl");

				        self.Dom.suggestBox.html("").append(html);
				    } else {
				        self.Dom.suggestBox.html('<p class="txt_c mt20">没有相关关键词~</p>');
				    }
				}
			);
        },
        destroyRelateKeyWord: function () {
            this.Dom.suggestBox.empty();
        },
        renderSearchResult: function (pageIndex) {
            var self = this;

            self.searchKey = this.getSearchKey();
            self.isLoading = true;
            self.Dom.loadMore.html('<i class="loading"></i>加载中...').show();

            $.wsajax(
				"CourseAjax.aspx",
				"GetCoursesByKeyWord",
				{
				    keyWord: self.searchKey,
				    pageIndex: pageIndex,
				    pageSize: PAGE_NUM,
				    orderBy: orderBy
				},
				function (data) {
				    var $emptyTip = $("#emptyTip");

				    self.Dom.loadMore.html('').hide();
				    if (data) {
				        var list = JSON.parse(data);
				        for (var i = 0, len = list.length; i < len; i++) {
				            if (DISCOUNT_CLASS[list[i].SaleEventType]) {
				                list[i].discount_class = DISCOUNT_CLASS[list[i].SaleEventType];
				            }

				        }
				        var html = getTemplate(list, "course-tmpl");

				        self.Dom.courseList.append(html);
				        $emptyTip.length && $emptyTip.remove();
				        pageIndex === 1 && (self.pageCount = list[0].PageCount);
				        self.curPageIndex++;
				        self.isLoading = false;
				    } else {
				        var emptyHtml = ''
							+ '<div id="emptyTip" class="pt40">'
							+ '	    <p class="txt_c mb20"><i class="icon_wrong vmiddle mr10"></i>查不到您要的课程，请换个关键词试试！</p>     '
							+ '	    <p class="txt_c mt10 mb20"><a href="CourseCenter.aspx" class="green">点击返回选课中心&gt;&gt;</a></p>  '
							+ '</div>';

				        self.Dom.courseList.html("");
				        $emptyTip.length && $emptyTip.show();
				        $emptyTip.length || self.Dom.loadMore.before(emptyHtml);
				    }
				}
			);
        },
        destroySearchResult: function () {
            this.Dom.courseList.empty();
        }
    };

    var GetQuestType = function () {
        switch (parseInt($selectType.val())) {
            case 0:
                initPage();
                break;

            case 1:
                var searchCourse = new SearchCourse();
                searchCourse.init();
                if ($(window).height() - $(".search-box").height() - $("section").height() - 45 > $footer.height()) {
                    $footer.css("padding", "0px");
                    $(".content").height($(window).height() - $footer.height());
                }
                break;

            case 2:
                $footer.hide();
                $(".top_nav").hide();
                getQuestionsList();
                break;

            case 3:
                //getVideoList();
                if ($('body').height() < $(window).height()) {
                    $('body').height($(window).height());
                }
                break;

            case 4:
                getRecordList();
                break;

            case 5:
                getCertList();
                break;

            case 6:
                getPrepaidList();
                break;
        }
    }

    $(function () {
        GetQuestType();

        var setContanierH = function () {
            var $contanier = $(".content");

            $contanier.css({
                "min-height": $(window).height() - $("footer").height() - $("header").height()
            });
        };


        setContanierH();
        $courseNav.on('click', function (event) {
            if ($courseNavWrapper.css('display') === 'block') {
                hideCourseNav();
            } else {
                showCourseNav();
            }
        });

        $('#allCourseBtn').bind('click', function () {
            $courseNav.removeClass('show');
            $mask.hide();
            $courseNavWrapper.hide();
            $courseNav.html('<i class="icon-all_nav"></i>全部课程');
            $("#container").css({ "height": "auto" });
            cateId = 0;
            //cateId = $('#hidePostId').val();
            pageIndex = 1;
            getCourseList();
        });

        $mask.on("click", function () {
            hideCourseNav();
        });

        $courseList.delegate('.back_all', 'click', function () {
            $courseNav.html('<i class="icon-all_nav"></i>全部分类');
            cateId = 0;
            pageIndex = 1;
            getCourseList();
        });


        $('#courseNavList').delegate('.cateItem', 'click', function (event) {
            var $this = $(this),
				$itemLi = $this.parent("li");

            if ($itemLi.hasClass('cur')) {
                $itemLi.removeClass('cur');
                setCss();
                setContentH();
                return false;
            }
            setCss();
            var index = $itemLi.index(),
				$navList = $itemLi.children('.nav_list'),
				$list = $navList.children('span'),
				count = $list.length,
				height = Math.ceil(count / 2) * 36;
            if (parseInt($selectType.val()) == 2) {
                height = Math.ceil(count / 5) * 36;
                $list.css("width", "20%");
            }

            $itemLi.addClass('cur');
            $navList.show().css({
                'width': $(window).width(),
                'height': height,
                'margin-left': -$this.offset().left
            });
            var start = Math.floor(index / 4) * 4,
				end = Math.min(start + 4, $firstNavList.length);

            for (var i = start; i < end; i++) {
                $firstNavList.eq(i).css({
                    'margin-bottom': height + 25
                });
            }
            setContentH();
        });
        $('#courseNavList').delegate('.sub_cate', "click", function (e) {
            var $this = $(this),
				text = $this.text(),
				className = $this.parents('li').eq(0).find('i').attr('class');
            $courseNav.html('<i class="' + className + '-small"></i>' + text).trigger('click');
            pageIndex = 1;
            cateId = $this.data('id');

            if (parseInt($selectType.val()) != 2) {
                getCourseList();
            }
            // else{
            // 	location.href = "CourseCenter.aspx?hideNav=1&Action="+$('#questType').val()+"&VideoListId="+VideoListId+"&CateId=" + cateId;
            // }
        });

        var $orderBtn = $('#orderBtn'),
			$orderList = $('#orderList');
        $orderBtn.bind('click', function () {
            if ($orderList.css('display') === 'block') {
                $orderList.hide();
                $orderBtn.removeClass('active');
            }
            else {
                $orderBtn.addClass('active');
                $orderList.show();
            }
        });


        $orderList.delegate('li', 'click', function () {
            var $this = $(this),
				text = $this.data('title'),
				className = $this.children('i').attr('class');
            $this.addClass('cur').siblings().removeClass('cur');
            $orderBtn.html(text + '<i class="' + className + '_grey"></i>');
            $orderList.hide();
            orderBy = $this.data('id');
            pageIndex = 1;
            getCourseList();
            $orderBtn.removeClass('active');

            // 触屏业务打点需求
            if (orderBy == 10) {
                SendEvent(25, 291);
            }
            else if (orderBy == 20) {
                SendEvent(25, 292);
            }
            else if (orderBy == 30) {
                SendEvent(25, 293);
            }
            else if (orderBy == 31) {
                SendEvent(25, 294);
            }
        });

        $('html').bind('touchstart', function (event) {
            var $event = $(event.target);
            if (!$event.closest('#courseNavWrapper').length && !$event.closest('#courseNav').length && !$event.closest('#layerMask').length) {
                $mask.hide();
                $courseNavWrapper.hide();
            }
            if (!$event.closest('#orderList').length && !$event.closest('#orderBtn').length) {
                $orderList.hide();
                $orderBtn.removeClass('active');
            }
        });
        var $win = $(window);
        $(document).scroll(function () {
            var top = document.body.scrollTop,
				bodyHeight = $('body').height(),
				winHeight = $(window).height();

            var $topSubNav = $(".top_nav .nav");
            if ($topSubNav.length > 0) {
                var $topNav = $(".top_nav");
                var navTop = $topNav.offset().top;

                if (top >= navTop) {
                    $topSubNav.addClass("fixed");
                } else {
                    $topSubNav.removeClass("fixed");
                }
            }

            if (loading || isLastPage || $mask.css('display') === 'block') {
                return false;
            }

            if (bodyHeight - top - winHeight <= 40) {
                loading = true;
                $loadMore.html('<i class="loading"></i>加载中...').show();
                pageIndex++;
                GetQuestType();
            }
        });

        $win.bind('orientationchange', function () {
            if ($mask.css('display') === 'block') {
                var $cur = $courseNavList.children('.cur'),
					$nav = $cur.children('.nav_list');
                $nav.css({
                    'width': $win.width(),
                    'margin-left': -$cur.offset().left
                });
            }
        });
    });

})();