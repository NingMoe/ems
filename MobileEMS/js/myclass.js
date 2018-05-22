$(function() {
	var PAGESIZE = 10;

	var MyClassList = function() {
		this.isLoading = false;
		this.curPageIndex = 1;
		this.isLoading = false;
		this.pageCount = 1;
		this.Dom = {
			win: $(window),
			container: $('.wrapper'),
			loadMore: $('#loadMore'),
			courseList: $('#courseList')
		};
	};

	MyClassList.prototype = {
		init: function() {
			this.renderMyClass(this.curPageIndex);
			this._initEvent();
		},
		_initEvent: function() {
			var self = this;

			self.Dom.win.scroll(function() {
				var top = document.body.scrollTop,
					bodyHeight = $('body').height(),
					winHeight = $(window).height();

				if (self.Dom.courseList.find(".ke").length) {
					if (self.isLoading || (self.curPageIndex - 1) >= self.pageCount) {
						return false;
					}

					if (bodyHeight - top - winHeight <= 40) {
						self.renderMyClass(self.curPageIndex);
					}
				}
			});
		},
		renderMyClass: function(pageIndex) {
			var self = this,
				url = "/handler/UserHandler.ashx";

			self.isLoading = true;
			self.Dom.loadMore.html('<i class="loading"></i>加载中...').show();

			$.ajax({
				type: "POST",
				url: url,
				data: {
					action: "GetMyClass",
					pageIndex: pageIndex,
					pageSize: PAGESIZE
				},
				dataType: "html",
				success: function(data) {
					var $emptyTip = $("#emptyTip"),
						data = JSON.parse(data);

					if (data.IsSuccess && data.Content) {
						var list = JSON.parse(data.Content),
							html = getTemplate(list, "course-tmpl");

						self.Dom.loadMore.html('').hide();
						self.Dom.courseList.append(html);
						$emptyTip.length && $emptyTip.remove();
						pageIndex === 1 && (self.pageCount = list[0].PageCount);
						self.curPageIndex++;
						self.isLoading = false;
						HJMCommon.initFooter();
					} else {
					    var emptyHtml = '' + '<div id="emptyTip" class="pt40">' + '您还没有开通课程哦~ 去看看 <a href="/course/?hideNav=1" class="green">岗位计划</a>' + '</div>';

						self.Dom.courseList.html("");
						$emptyTip.length || self.Dom.container.html(emptyHtml);
					}
				}
			});
		}
	};

	var myClassList = new MyClassList();
	myClassList.init();

});