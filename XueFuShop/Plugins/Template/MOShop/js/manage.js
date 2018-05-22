$(function(){
	/*侧边栏高度设置*/
	function setHeight(){
		var tabNavHeight = $(".tabs_nav").height();
		var rHeight = $(".right").height() + 50;
		if(rHeight <= 820 && tabNavHeight <= 820){
			$(".left,.left .tabs").css({"height":850 + "px"});
		}
		else if(rHeight <= 820 && tabNavHeight > 820){
			$(".left,.left .tabs").css({"height":tabNavHeight + 50 +"px"});
		}
		else if(rHeight > 820 && tabNavHeight <= 820){
			$(".left,.left .tabs").css({"height":rHeight + 30 +"px"});
		}
		else if(rHeight > 820 && tabNavHeight > 820){
			if (rHeight <= tabNavHeight) {
				$(".left,.left .tabs").css({"height":tabNavHeight + 50 +"px"});
			}
			else{
				$(".left,.left .tabs").css({"height":rHeight + 30 +"px"});
			}
		}
	}
	window.onload = function(){setHeight();};
	window.onresize = function(){setHeight();}

	function scrollTop(){
		$("body,html").animate({ scrollTop: 0 }, 0);
	}

	//过滤<script><\/script>标签
	function clearScript(s) {  
	    return s.replace(/<script.*?>.*?<\/script>/ig, '');  
	}

	$(document).ready(function(){	
		/*左侧导航切换*/
		$(".tabs li").click(function() {
			$(this).addClass("on");
			$(this).siblings().removeClass("on");
			if ($(this).hasClass("ems")) {
				$(".tabs_nav").find(".ems").show();
				$(".tabs_nav").find(".tpr").hide();
				$(".tabs_nav").find(".base").hide();
			}
			else if($(this).hasClass("tpr")){
				$(".tabs_nav").find(".ems").hide();
				$(".tabs_nav").find(".tpr").show();
				$(".tabs_nav").find(".base").hide();
			}
			else{
				$(".tabs_nav").find(".ems").hide();
				$(".tabs_nav").find(".tpr").hide();
				$(".tabs_nav").find(".base").show();
			}
		});

		/*是否岗位勾选情况*/
		if($(".form_zjgw_checkbox").is(":checked")){
			$("#next").attr("value","下一步");
			$("#isPost_illustration").hide();
			$(".step").show();
			$("#next").addClass("next").removeClass("complete");
		}
		$(".form_zjgw_checkbox").click(function(){
			if($(this).is(":checked")){
				$("#next").attr("value","下一步");
				$("#isPost_illustration").hide();
				$(".step").show();
				$("#next").addClass("next").removeClass("complete");
			}
			else{
				$("#next").attr("value","保存并提交");
				$(".step").hide();
				$("#isPost_illustration").show();
				$("#next").removeClass("next").addClass("complete");
			}
		});

		/*指标勾选设置*/
		$(".evaluation_sheet").on("click",".schedule", function() {
			var rowItems = $(this).siblings();
			//判断是否永久指标
			if($(rowItems).hasClass("forever") && $(this).parent().children().hasClass("originPos")) {
				var selectObj = $(this).parent().children(".schedule");
				var originCheckedPos = selectObj.index(selectObj.filter(".originPos"));
				var currentCheckedPos = selectObj.index($(this));
				/*if(currentCheckedPos >= originCheckedPos){
					$(this).text("√");
					$(rowItems).filter(".schedule").text("");
				}*/
				if(originCheckedPos == 4){}
				else{
					$(this).text("√");
					$(rowItems).filter(".schedule").text("");
				}
			}else{
				if($(this).text()=="√"){
					$(this).text("");
					$(this).parent().removeClass("active");
				}else{
					$(this).text("√");
					$(rowItems).filter(".schedule").text("");
					$(this).parent().addClass("active");
				}
			}
		});

		$("#KPIListArea").on("click",".choose", function() {
			$(this).nextAll(".schedule").click();
		});

		/*评估表载入时原始数据打勾*/
		var datavalueArray = new Array();
		$("[data-value]").each(function() {
			datavalueArray.push($(this).attr("data-value"));
		});
		var kpiStr = $("#evaluation_sheet").find("#kpiidstr").val();
		if (kpiStr) {
			var kpiArray = kpiStr.split(",");
			$.each(kpiArray, function(key, val) { 
				var rate = val.split(":");
				var ratePos = $.inArray(rate[1], datavalueArray);
				$(".evaluation_content").each(function() {
					if ($(this).attr("data-id") == rate[0] && ratePos != -1) {
						$(this).nextAll().eq(ratePos).click();
					}
				});
			}); 
		}
		/*载入时对评估表勾选位置进行定位*/
		$(".schedule").each(function() {
			if ($(this).siblings().hasClass("forever") && $(this).parent().find(".schedule").text() != "") {
				$(this).parent().addClass("active").find("td:contains('√')").addClass("originPos");
			}
		});
		/*评价指标载入时原始数据打勾*/
		function setKpi(kpi){
			$(".schedule").text("").parent().removeClass("active");
			var kpiidArray = kpi.split(",");
			$.each(kpiidArray, function(index, val) {
				$("[data-id='"+val+"']").siblings(".schedule").click();
			});
		}

		var kpiidStr = $("#evaluating_indicator").find("#kpiidstr").val();
		if (kpiidStr){
			if($(".form_zjgw_checkbox").prop("checked") == false){
				$(".form_zjgw_checkbox").click();
			}
			setKpi(kpiidStr);
		}

		/*相关岗位指标预设*/
		var currentDataid = "";
		$("#preset_indicator").find(".post_name").click(function() {
			var dataKpi = $(this).attr("data-kpi");
			var isChecked = false;
			if ($(".post_name").hasClass("current")) {
				if($(this).hasClass("current")){
					$(this).removeClass("current");
					setKpi(currentDataid);
					currentDataid = "";
				}
				else{
					$(".preset_post").find("td").removeClass("current");
					$(this).addClass("current");
					isChecked = true;
				}	
			}
			else{
				$(this).addClass("current");
				$(".schedule").each(function() {
					if ($(this).text() == "√") {
						var dataId = $(this).siblings(".evaluation_content").attr("data-id");
						if (currentDataid == "") {
							currentDataid = dataId;
						}
						else{
							currentDataid += "," + dataId;
						}
					}			
				});
				isChecked = true;
			}
			/*勾选开关*/
			if (isChecked) {
				setKpi(dataKpi);
			}
		});

		/*综合能力评估载入时评估类型评估人选择*/
		var EvaluateType = $("#general_evaluation").find("#EvaluateType").val();
		var EvaluateUserId = $("#general_evaluation").find("#EvaluateUserId").val();
		if (parseInt(EvaluateType) > 0){
			$("#EvaluationType").val(EvaluateType);
		}
		if (parseInt(EvaluateUserId) > 0) {
			$("#EvaluationUserId").val(EvaluateUserId);
		}
		
		/*综合能力评估*/
		$("#general_evaluation").find("[data-value]").click(function() {
			$(this).addClass("active").siblings().removeClass("active");
			/*$(this).siblings(".project_score").text($(this).attr("data-value"));*/
			var UserId = $("#UserId").val();
			var EvaluateDate = $("#EvaluateDate").val();
			var EvaluateNameId = $("#EvaluationName").val();
			var EvaluateUserId = $("#EvaluationUserId").val();
			var datavalue = $(this).attr("data-value");
			var dataid = $(this).siblings("[data-id]").attr("data-id");
			var currentObj = $(this);
			$.get("/Ajax.aspx?Action=staffEvaluate&UserId="+UserId+"&EvaluateDate="+EvaluateDate+"&EvaluateNameId="+EvaluateNameId+"&EvaluateUserId="+EvaluateUserId+"&DataValue="+datavalue+"&DataId="+dataid, function(data) {
				data = clearScript(data);
				if (data != "ok") {
					$(this).removeClass("active");
				}
			});
		});
		
		/*评价指标下一步按钮*/
		$("#next").click(function(e) {
			if ($(this).hasClass("job") && $(this).hasClass("next") && validateForm()) {
				$(".step .step2").addClass("current");
				$("#addjob").hide();
				$("#preset_indicator").show();
				$("#evaluation_set").show();
				$("[data-type='2']").hide();
				$("[data-type='3']").hide();
				$(this).removeClass("job").addClass("learning");
				$("#prev").show().addClass("learning");
				e.preventDefault();
				scrollTop();
			}
			else if ($(this).hasClass("job") && $(this).hasClass("complete") && validateForm()) {
				$("#evaluating_indicator").submit();
			}
			else if ($(this).hasClass("learning")){
				$(".step .step3").addClass("current");
				$("[data-type='1']").hide();
				$("[data-type='2']").show();
				$(this).removeClass("learning").addClass("ability");
				$("#prev").removeClass("learning").addClass("ability");
				e.preventDefault();
				scrollTop();
			}
			else if ($(this).hasClass("ability")){
				$(".step .step4").addClass("current");
				$("[data-type='2']").hide();
				$("[data-type='3']").show();
				$(this).removeClass("next ability").addClass("complete contribution").attr("value","完成");
				$("#prev").removeClass("ability").addClass("contribution");
				e.preventDefault();
				scrollTop();
			}
			else if ($(this).hasClass("complete") && $(this).hasClass("contribution")) {
				var kpiidstr = "";
				$(".schedule").each(function() {
					if ($(this).text() == "√") {
						var dataId = $(this).siblings(".evaluation_content").attr("data-id");
						if (kpiidstr == "") {
							kpiidstr = dataId;
						}
						else{
							kpiidstr += "," + dataId;
						}				
					}
				});
				$("#kpiidstr").val(kpiidstr);
			}
			$(window).trigger("resize");
		});
		/*评价指标上一步按钮*/
		$("#prev").click(function() {
			if ($(this).hasClass("learning")) {
				$(".step .step2").removeClass("current");
				$("#addjob").show();
				$("#preset_indicator").hide();
				$("#evaluation_set").hide();
				$(this).removeClass("learning").hide();
				$("#next").removeClass("learning").addClass("job");
				scrollTop();
			}
			else if ($(this).hasClass("ability")){
				$(".step .step3").removeClass("current");
				$("[data-type='1']").show();
				$("[data-type='2']").hide();
				$(this).removeClass("ability").addClass("learning");
				$("#next").removeClass("ability").addClass("learning");
				scrollTop();
			}
			else if ($(this).hasClass("contribution")) {
				$(".step .step4").removeClass("current");
				$("[data-type='2']").show();
				$("[data-type='3']").hide();
				$(this).removeClass("contribution").addClass("ability");
				$("#next").removeClass("complete contribution").addClass("next ability").attr("value","下一步");
				scrollTop();
			}
			$(window).trigger("resize");
		});

		/*综合能力评估验证*/
		function generalEvaluationForm(){ 
			var validator = $(".general_evaluation").validate({
			    rules: {
			      	UserName: "required",
			    },
			    messages: {
			    	UserName: "请填写被评估人姓名",
			      	EvaluationType: "请选择评估类型",
			      	EvaluationName: "请选择评估名称",
			      	EvaluationUserId: "请选择评估人"
			    },
			});
			return validator.form();
		}
		/*评估人组成数组*/
		var EvaluationUserIdArray = new Array();
	    $("#EvaluationUserId option").each(function(){
	        var pgr = $(this).val();
	        if( pgr != "" )
	           	EvaluationUserIdArray.push(pgr);
	    });
		/*综合能力评估下一步按钮*/
		$("#zhsubmit").click(function(e) {
			/*$(".project_score").text("");*/
			var UserName = $("#UserName").val();
			var CompanyId = $("#CompanyID").val()?$("#CompanyID").val():$("#CompanyId").val();
			var EvaluateNameId = $("#EvaluationName").val();
			var EvaluateUserId = $("#EvaluationUserId").val();
			var EvaluateName = $("#EvaluationName option:selected").text();
			e.preventDefault();
			if (generalEvaluationForm()) {
				var currentObj = $(this);
				if($(currentObj).hasClass("next")){
					$.get("/Ajax.aspx?Action=GetEvaluateInfo&UserName="+UserName+"&CompanyId="+CompanyId+"&EvaluateNameId="+EvaluateNameId+"&EvaluateUserId="+EvaluateUserId, function(data) {
						data = JSON.parse(clearScript(data)); 
						if (data.UserId > 0) {
							$("#UserId").val(data.UserId);
							$("#EvaluateDate").val(data.EvaluateDate);
							$("#scorestr").val(data.ScoreStr);
							$("#general_evaluation").find(".RealName").text(UserName);
							$("#general_evaluation").find(".Department").text(data.Department);
							$("#general_evaluation").find(".PostName").text(data.PostName);
							$("#general_evaluation").find(".EvaluateName").text(EvaluateName);
							$("#general_evaluation").find(".EvaluateTime").text(data.EvaluateStartDate+"到"+data.EvaluateEndDate);
							$(".step .step2").addClass("current");
							$("#general_evaluation").find(".general_evaluation_info").hide();
							if( $("#EvaluationType").val() == "530" ){
								$(".staff_estimate").show();
							}
							else if( $("#EvaluationType").val() == "541" ){
								$(".director_estimate").show();
							}
							else if( $("#EvaluationType").val() == "564" ){
								$(".manager_estimate").show();
							}
							$("#general_evaluation").find("[data-value]").removeClass("active");
							if (data.ScoreStr.length > 0) {
								var ScoreArray = data.ScoreStr.split(",");
								$.each(ScoreArray, function(key, val) { 
									var Score = val.split(":");
									$("#general_evaluation").find(".project_content").each(function() {
										if ($(this).attr("data-id") == Score[0]) {
											$(this).siblings("[data-value='"+ Score[1] +"']").addClass("active");
											/*$(this).siblings(".project_score").text(Score[1]);*/
										}
									});
								}); 
							}
							$(currentObj).addClass("complete").removeClass("next").attr("value","完成");
							$("#zhprev").show();
							$(window).trigger("resize");
						}
						else{
							alert("您填写的用户不存在！");
							$("#UserName").focus();
						}
					});
				}
				else if($(currentObj).hasClass("complete")){
					if( EvaluationUserIdArray.length > 1 ) {
						EvaluationUserIdArray.splice($.inArray(EvaluateUserId,EvaluationUserIdArray),1);
						layer.confirm('是否更换为"评估人'+EvaluationUserIdArray[0]+'"继续录入？', {
							btn: ['是','否'],
							closeBtn: 0,
							title: '提示'
						}, function(){
							$("#scorestr").val("");
						    if ( EvaluationUserIdArray.length > 0 && EvaluationUserIdArray[0] != "" ) {
						    	var EvaluationUser = EvaluationUserIdArray[0];
						    	$("#EvaluationUserId").val(EvaluationUser);
						    	$.get("/Ajax.aspx?Action=GetEvaluateInfo&UserName="+UserName+"&CompanyId="+CompanyId+"&EvaluateNameId="+EvaluateNameId+"&EvaluateUserId="+EvaluationUser, function(data) {
									data = JSON.parse(clearScript(data)); 
									$("#general_evaluation").find("[data-value]").removeClass("active");
									if (data.ScoreStr.length > 0) {
										var ScoreArray = data.ScoreStr.split(",");
										$.each(ScoreArray, function(key, val) { 
											var Score = val.split(":");
											$("#general_evaluation").find(".project_content").each(function() {
												if ($(this).attr("data-id") == Score[0]) {
													$(this).siblings("[data-value='"+ Score[1] +"']").addClass("active");
													/*$(this).siblings(".project_score").text(Score[1]);*/
												}
											});
										}); 
									}
								});
								layer.closeAll();
						    }
						    else{
						    	location.reload();
						    }
						}, function(){
							location.reload();
						});
					}
					else{
						location.reload();
					}				
				}		
			}
		});

		/*综合能力评估上一步按钮*/
		$("#zhprev").click(function() {
			$(".step .step2").removeClass("current");
			$("#general_evaluation").find(".general_evaluation_info").show();
			$("#general_evaluation").find(".sheet").hide();
			$("#zhprev").hide();
			$("#zhsubmit").addClass("next").removeClass("complete").attr("value","下一步");
			$(window).trigger("resize");
		});

		/*评估表完成按钮*/
		$("#complete").click(function() {
			var kpiidstr = "";
			$(".schedule").each(function() {
				if ($(this).text() == "√") {
					var dataId = $(this).prevAll(".evaluation_content").attr("data-id");
					var indexPos = $(this).parent().children(".schedule").index($(this));
					var gouPos = dataId + ":" + datavalueArray[indexPos];
					if (kpiidstr == "") {
						kpiidstr = gouPos;
					}
					else{
						kpiidstr += "," + gouPos;
					}
				}
			});
			$("#kpiidstr").val(kpiidstr);
		});

		/*指标勾选移除开关*/
		if($("[data-edit]").attr("data-edit") == "disable"){
			$(".evaluation_sheet").off("click",".schedule");
			$("tr").removeClass("active");
		}

		$(".listblock ul li").click(function() {
			$(this).addClass("liston").siblings().removeClass("liston");
		});

		/*评估报表下拉菜单切换*/
		if(typeof(EvaluateNameIDDefaultValue) != "undefined" && EvaluateNameIDDefaultValue != "" && parseInt(EvaluateNameIDDefaultValue) > 0){
			$(".choose_company").find("#EvaluationName").val(EvaluateNameIDDefaultValue);
		}
		if(typeof(WorkingPostIDDefaultValue) != "undefined" && WorkingPostIDDefaultValue != "" && parseInt(WorkingPostIDDefaultValue) > 0){
			$(".choose_company").find("#PostId").val(WorkingPostIDDefaultValue);
		}
		$(".choose_company").find("#CompanyID").change(function() {
			var CompanyID = $(this).val();
			var PostId = $(".choose_company").find("#PostId");
			var PostName = $(".choose_company").find("#PostName");
			if (CompanyID != "") {
				$.post("/Ajax.aspx?Action=GetEvaluateNameList", {
				CompanyID:CompanyID,
				Type:1
				},function(result) {
					result = clearScript(result);
					$(".choose_company").find("#EvaluationName").html(result);
				});
				if (CompanyID == "0" && $(PostName).length > 0) {
					$(PostId).addClass("form_hide");
					$(PostName).removeClass("form_hide");
				}
				else{
					$(PostId).removeClass("form_hide");
					$(PostName).addClass("form_hide");
					$.post("/Ajax.aspx?Action=GetWorkingPostList", {
						CompanyID:CompanyID
					},function(result) {
						result = clearScript(result);
						$(PostId).html(result);
					});
				}
			}
		});

		/*评价指标岗位验证*/
		function validateForm(){ 
			var validator = $("#evaluating_indicator").validate({
			    rules: {
				    CompanyName: "required",
				    PostName: "required",
			    },
			    messages: {
			      	CompanyName: "请输入公司名称",
			      	PostName: "请输入岗位",
			    },
			});
			return validator.form();
		}
		/*评估表验证及调整窗口*/
		$("#evaluation_sheet").find(".form_pgb_button").click(function() {
			$("#evaluation_information").validate({
				rules: {
			      	UserName: "required",
			      	PostId: "required",
			      	EvaluationName: "required",
			    },
			    messages: {
			      	UserName: "请输入姓名",
			      	PostId: "请选择评估岗位",
			      	EvaluationName: "请选择评估名称",
			    },
			});
			$(window).trigger("resize");
		});
		/*增加指标验证*/
		$("#add_index").find(".form_zjzb_button").click(function() {
			$("#add_index").validate({
				rules: {
			      	CompanyID: "required",
			      	ClassID: "required",
			      	Name: "required",
			      	Method: "required",
			      	Score: "required",
			      	Type: "required",
			      	Sort: "required",
			    },
			    messages: {
			    	CompanyID: "请选择公司名称",
			      	ClassID: "请选择上级分类",
			      	Name: "请填写指标名称",
			      	Method: "请填写评估人/部门",
			      	Score: "请填写分数",
			      	Type: "请选择指标类型",
			      	Sort: "请填写排序ID",
			    },
			});
		});
		/*评估报表验证*/
		$("#completion_rate").find(".form_tgl_button").click(function() {
			layer.load(1, {
	            shade: [0.8,'#000'] 
	        });
			if ($("#qualified_rate").find("#PostName").hasClass("form_hide") == true) {
				$("#completion_rate").validate({
					groups: {  
				       	condition: "EvaluationName Name PostId"   //condition定义的组名，Name PostId是输入框的名字  
				    },
				    /*errorPlacement: function(error, element) {  //错误提示在什么地方  
				        if (element.attr("name") == "Name" || element.attr("name") == "PostId" ){  
				        	alert(error.text());    //如果是Name和PostId呢，就#PostId后面  
				        }else{  
				          	error.insertAfter(element);  
				        }  
				    },*/
			       	rules:{  
			       		EvaluationName:{  
			                required: {  
			                    depends:function(){ //三选一  
			                        return ($("input[name=Name]").val().length <= 0 && $("select[name=PostId]").val().length <= 0);  
			                    }  
			                }  
			            },  
			            Name:{  
			                required: {  
			                    depends:function(){ //三选一  
			                        return ($("select[name=EvaluationName]").val().length <= 0 && $("select[name=PostId]").val().length <= 0);  
			                    }  
			                }  
			            },  
			            PostId:{  
			                required: {  
			                    depends:function(){ //三选一  
			                        return ($("select[name=EvaluationName]").val().length <= 0 && $("input[name=Name]").val().length <= 0);  
			                    }  
			                }  
			            } 
			        },  
			        messages:{ //提示报错 
			        	EvaluationName: "评估名称，姓名和评估岗位至少填写一个",
			           	Name: "评估名称，姓名和评估岗位至少填写一个",
			            PostId:"评估名称，姓名和评估岗位至少填写一个",
			        },
			        /* 重写错误显示消息方法,以alert方式弹出错误消息 */   
					showErrors: function(errorMap, errorList) {     
					   	alert(errorList[1].message);
					   	layer.closeAll();
					},
				});
			}
			else{
				$("#completion_rate").validate({
					groups: {  
				       	condition: "EvaluationName Name PostName"   //condition定义的组名，Name PostId是输入框的名字  
				    },
			       	rules:{  
			       		EvaluationName:{  
			                required: {  
			                    depends:function(){ //三选一  
			                        return ($("input[name=Name]").val().length <= 0 && $("input[name=PostName]").val().length <= 0);  
			                    }  
			                }  
			            },  
			            Name:{  
			                required: {  
			                    depends:function(){ //三选一  
			                        return ($("select[name=EvaluationName]").val().length <= 0 && $("input[name=PostName]").val().length <= 0);  
			                    }  
			                }  
			            },  
			            PostName:{  
			                required: {  
			                    depends:function(){ //三选一  
			                        return ($("select[name=EvaluationName]").val().length <= 0 && $("input[name=Name]").val().length <= 0);  
			                    }  
			                }  
			            } 
			        },  
			        /* 重写错误显示消息方法,以alert方式弹出错误消息 */   
					showErrors: function(errorMap, errorList) {     
					   	alert(errorList[1].message);
					   	layer.closeAll();
					},
			        messages:{ //提示报错 
			        	EvaluationName: "评估名称，姓名和评估岗位至少填写一个",
			           	Name: "评估名称，姓名和评估岗位至少填写一个",
			            PostName:"评估名称，姓名和评估岗位至少填写一个",
			        }, 
				});
			}
		});
		/*增加评估验证*/
		$("#add_evaluation").find(".form_button").click(function() {
			$("#add_evaluation form").validate({
				rules: {
					CompanyID:"required",
			      	EvaluationName: "required",
			      	EvaluationTime: "required",
			      	EvaluationStartTime: "required",
			      	EvaluationEndTime: "required",
			    },
			    messages: {
			    	CompanyID:"请选择公司名称",
			    	EvaluationName: "请填写评估名称",
			      	EvaluationTime: "请选择评估时间",
			      	EvaluationStartTime: "",
			      	EvaluationEndTime: "请选择评估时间段",
			    },
			});
		});
		/*综合能力评估详情验证*/
		/*$("#general_evaluation_countdetail").find(".form_button").click(function() {
			$("#general_evaluation_countdetail form").validate({
			    messages: {
			      	EvaluationType: "请选择评估类型",
			      	EvaluationName: "请选择评估名称",
			    },
			});
		});*/

		/*密码规范*/
		function passwordLevel(password) {
			var Modes = 0;
			for (i = 0; i < password.length; i++) {
			 	Modes |= CharMode(password.charCodeAt(i));
			}
				return bitTotal(Modes);
			//CharMode函数
			function CharMode(iN) {
			 	if (iN >= 48 && iN <= 57)//数字
			  		return 1;
			 	if (iN >= 65 && iN <= 90) //大写字母
			  		return 2;
			 	if ((iN >= 97 && iN <= 122) || (iN >= 65 && iN <= 90))
			 	//大小写
			  		return 4;
			 	else
			  		return 8; //特殊字符
			}
			//bitTotal函数
			function bitTotal(num) {
			 	modes = 0;
			 	for (i = 0; i < 4; i++) {
				  	if (num & 1) modes++;
				  		num >>>= 1;
				}
			 	return modes;
			}
		}

		/*修改密码验证*/
		$("#modify_password").find(".form_xgmm_button").click(function() {
			$.validator.addMethod("strongPsw",function(value,element){
				var spsw = /(?=^[0-9a-zA-Z._@#]{6,16}$)((?=.*[0-9])(?=.*[^0-9])|(?=.*[a-zA-Z])(?=.*[^a-zA-Z]))/;
    			return this.optional(element) || (spsw.test(value));
			},"必须是字母和数字的组合");
			$("#modify_password").validate({
				rules: {
			      	OldPassword: "required",
			      	UserPassword1: {
			      		required:true,
			      		rangelength:[6,16],
			      		strongPsw:true,
			      	},
			      	UserPassword2: {
			      		required:true,
			      		equalTo: "#UserPassword1"
			      	},
			    },
			    messages: {
			      	OldPassword: "请输入正确的密码",
			      	UserPassword1: {
			      		required:"请输入新密码",
			      		rangelength:"密码长度大于6位少于16位",
			      	},
			      	UserPassword2: {
			      		required:"请再次输入新密码",
			      		equalTo: "两次输入必须一致"
			      	},
			    },
			});
		});
		/*增加修改员工验证*/
		$("#add_staff").find(".form_zjyg_button").click(function() {
			$.validator.addMethod("isCardID",function(value,element){
				var cardid = /^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$/;
    			return this.optional(element) || (cardid.test(value));
			},"请输入正确的身份证号码");
			$.validator.addMethod("strongPsw",function(value,element){
				var spsw = /(?=^[0-9a-zA-Z._@#]{6,16}$)((?=.*[0-9])(?=.*[^0-9])|(?=.*[a-zA-Z])(?=.*[^a-zA-Z]))/;
    			return this.optional(element) || (spsw.test(value));
			},"必须是字母和数字的组合");
			$.validator.addMethod("isMobile", function(value, element) {
			    var length = value.length;
			    var mobile = /^(13[0-9]{9})|(18[0-9]{9})|(19[0-9]{9})|(14[0-9]{9})|(17[0-9]{9})|(15[0-9]{9})$/;
			    return this.optional(element) || (length == 11 && mobile.test(value));
			}, "请正确填写您的手机号码");
			/*$.validator.addMethod("hasMobile", function(value, element) {
				var phone = $("#Mobile").val();
				var ispass = false;
				$.ajax({
				  	type: 'POST',
				  	url: '/Ajax.aspx?Action=CheckMobile',
				  	data: {Mobile: phone,UserName: $("#UserName").val()},
				  	async : false,  
				  	success: function(data){
				  		data = JSON.parse(data);
						if (data.result) {
							ispass = true;
						}
				  	},
				});
				return ispass;
			}, "该手机号已存在");*/
			$.validator.addMethod("Username", function(value, element) {
			    var username =  /^[a-zA-Z]{1}[a-zA-Z0-9_.]{4,19}$/;
			    if (value.substr(0,2) == "20") {
			    	username = /^[a-zA-Z0-9_.]{5,20}$/;
			    }
    			return this.optional(element) || (username.test(value));
			}, "用户名格式不正确");
			$.validator.addMethod("hasUsername", function(value, element) {
				var username = $("#UserName").val();
				var ispass = true;
				$.ajax({
				  	type: 'GET',
				  	url: '/Ajax.aspx?Action=CheckUserName',
				  	data: {UserName: username},
				  	async : false,  
				  	success: function(data){
						if (data == "2") {
							ispass = false;
						}
				  	},
				});
				return ispass;
			}, "用户名已存在");
			$.validator.addMethod("isEmail", function(value, element) {
			    var email = /^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$/;
    			return this.optional(element) || (email.test(value));
			}, "电子邮箱格式不正确");
			$("#add_staff").validate({
				errorPlacement: function(error, element) {  
				    error.appendTo(element.parent());  
				},
				rules: {
					UserName:{
						required:true,
						Username:true,
						hasUsername:true
					},
					Mobile:{
						required:true,
						isMobile:true,
						/*hasMobile:true*/
					},
					Email:{
						required:true,
						isEmail:true,
						email:true
					},
					UserPassword: {
			      		required:true,
			      		rangelength:[6,16],
			      		strongPsw:true
			      	},
			      	UserPassword2: {
			      		required:true,
			      		equalTo: "#UserPassword"
			      	},
			      	RealName:"required",
			      	IDCard:{
			      		required:true,
			      		isCardID:true
			      	},
			      	EntryDate:"required",
			      	WorkingPostName:"required",
			    },
			    messages: {
			    	UserName: {
			    		required:"用户名不能为空"
			    	},
			    	Mobile: {
			    		required:"手机号码不能为空"
			    	},
			    	Email:{
						required:"电子邮箱不能为空",
						email:"电子邮箱格式不正确",
					},
					UserPassword: {
			      		required:"请输入密码",
			      		rangelength:"密码长度大于6位少于16位",
			      	},
			      	UserPassword2: {
			      		required:"请再次输入密码",
			      		equalTo: "两次输入必须一致"
			      	},
			      	IDCard:{
			      		required:"身份证号不能为空"
			      	},
			      	EntryDate:"入职时间不能为空",
			      	RealName:"真实姓名不能为空",
			      	Sex:"性别不能为空",
			      	Status:"状态不能为空",
			      	Department:"部门不能为空",
			      	WorkingPostID:"岗位不能为空",
			      	WorkingPostName:"店内岗位不能为空",
			      	StudyPostID:"学习岗位不能为空",
			    },
			});
		});
		/*工作岗位选择部门后岗位下拉菜单显示*/
		$("#add_staff").find(".department").on("click",  function() {
			if ($(this).val() != "") {
				$(".postlist").show();
			}
			else{
				$(".postlist").hide();
			}
		});

		/*增加修改公司验证*/
		$("#modify_info").find(".form_xgxx_button").click(function() {
			$.validator.addMethod("isTel", function(value, element) {
				var tel = /^(\d{3,4}-?)?\d{7,8}$/g;
				return this.optional(element) || (tel.test(value));
			}, "请正确填写您的电话号码");
			$("#modify_info").validate({
				errorPlacement: function(error, element) {  
				    error.appendTo(element.parents(".form_validate"));  
				},
				rules: {
					CompanyTel:{
						required:true,
						isTel:true,
					},
			      	CompanyName: "required",
			      	CompanySimpleName: "required",
			    },
			    messages: {
			    	CompanyTel: {
			    		required:"电话号码不能为空"
			    	},
			      	CompanyName: "请输入公司全称",
			      	CompanySimpleName: "请输入公司简称",
			      	BrandID:"请选择品牌",
			      	Post:"请选择岗位",
			    },
			});
		});
		/*添加项目分类和项目名称*/
		/*$("#add_project-cute").find(".form_xmfl_button").click(function(event) {
			$("#add_project-cute").validate({
				rules: {
					PostName:"required",
			    },
			    messages: {
			    	PostName:"请填写分类名称",
			    },
			});
		});*/
		function addProjectNameForm(){ 
			var validator = $("#add_project-name").validate({
			    rules: {
					PostName:"required",
			    },
			    messages: {
			    	ParentID:"请选择分类名称",
			    	PostName:"请填写项目名称"
			    },
			});
			return validator.form();
		}
		$("#add_project-name").find(".form_xmmc_button").click(function(event) {
			addProjectNameForm();
		});

		/*公司类型选择是否弹出隶属公司*/
		$("#modify_info").find(".companytype").find("input[name='CompanyType']").change(function(){
	        if($(this).val() == "2" || $(this).val() == "3"){
	            $(".form_group_bloc").show();
		    }
		    else{
		        $(".form_group_bloc").hide();
		    }
		    $(window).trigger("resize");
	    });
		/*点击添加隶属集团弹出下拉菜单*/
	    $("#AddGroupName").on("click", function() {
	    	$("#AddGroupId").show();
	    	$(window).trigger("resize");
	    });
	    /*选择隶属集团进行添加*/
	    $("#GroupId").change(function() {
	    	var groupid = $(this).val();
	    	var grouplist = "";
	    	$.each($(".bloclist li"), function() {
	    		if (grouplist == "") {
	    			grouplist += $(this).attr("id");
	    		}
	    		else{
	    			grouplist += "," + $(this).attr("id");
	    		}
	    	});
	    	if (groupid != "" && $.inArray("li_"+ groupid, grouplist.split(",")) == "-1") {
		    	$(".bloclist").append("<li id='li_"+ groupid +"'>"+  $(this).find("option[value='"+ groupid +"']").text() +"<a class='del' href='javascript:'>删除</a></li>");
			    $("#AddGroupId").hide();
		    }
		    $(window).trigger("resize");
	    });
	    /*删除隶属集团*/
	    $(".bloclist").on("click",".del", function() {
	    	$(this).parentsUntil(".bloclist").remove();
	    	$(window).trigger("resize");
	    });
	    /*所有品牌点击切换*/
	    $(".allbrand").find("input").change(function() {
	    	if ($(this).prop("checked") == true) {
	    		$(this).parentsUntil(".form_brand").find("input[name='brandid']").prop("checked", true);
	    	}
	    	else{
	    		$(this).parentsUntil(".form_brand").find("input[name='brandid']").prop("checked", false);
	    	}
	    });
	    /*岗位设置点击部门则下面所有岗位切换*/
	    $(".post_set").find("input[name='Department']").change(function() {
	    	if ($(this).prop("checked") == true) {
	    		$(this).parentsUntil(".post_set").find("input[name='Post']").prop("checked", true);
	    	}
	    	else{
	    		$(this).parentsUntil(".post_set").find("input[name='Post']").prop("checked", false);
	    	}
	    });

	    /*获取导航栏的数据*/
	    function GetQueryString(name)
        {
            var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if(r!=null)return  unescape(r[2]); return null;
        }
        /*var PGtype = GetQueryString("EvaluateType");
        if (PGtype == 541) {
        	$(".sheet tr:not(.count) th:eq(1)").hide();
        	$(".sheet tr:not(.count)").each(function(index, el) {
        		$(this).find("td:eq(1)").hide();
        	});
        	$(".sheet tr.count td:eq(0)").removeAttr("colspan");
        }*/

        /*报表筛选*/
        $(".content_select").on('click', 'li', function(event) {
        	if ($(this).hasClass("current") == true) {
        		$(this).removeClass("current");
        	}
        	else{
        		$(this).addClass("current");
        	}
        });
        $(".content_select").find(".leftlist a").click(function() {
        	var liobj = $(this).parents("tr").find("li");
        	if ($(this).hasClass("select_all") == true) {
        		if( liobj.hasClass("current") == true){
	        		liobj.removeClass("current");
	        	}
	        	else{
	        		liobj.addClass("current");
	        	}
        	}
        	else{ 
        		liobj.each(function() {
        			if( $(this).hasClass("current") == false ){
        				$(this).addClass("current");
        			}
        			else{
        				$(this).removeClass("current");
        			}
        		});
        	}
        	GetCatId($(this));
        });
       	// HR人资报表红黄绿灯选择
	    $(".content_select").on("click", ".tgstate li", function(event) {
	    	 $("tr[data-style='data']").css("display","none");
	        $(".tgstate li.current").each(function(){$("td."+$(this).attr("id")).parent().css("display","");});
	    });
	    $("body").on("click", ".checkbox-li li", function(event) {
	    	GetCatId($(this));
	    });

        /*预设岗位折叠*/
        $(".flex_button").click(function() {
        	var obj =  $(this).find("span");
        	if( obj.text() == "展开"){
        		$(".icon-flex").css("background-position", "-20px 0px");
        		obj.text("收缩");
        		$(".preset_post").show();
        	}
        	else{
        		$(".icon-flex").css("background-position", "0px 0px");
        		obj.text("展开");
        		$(".preset_post").hide();
        	}
        	$(window).trigger("resize");
        });

        /*报表筛选区域折叠*/
        $(".filter_range_btn").click(function() {
        	var obj =  $(this).find("span");
        	if( obj.text() == "展开"){
        		$(".icon-flex").css("background-position", "-20px 0px");
        		obj.text("收缩");
        		$(".filter_range").css("border-bottom","none");
        		$(".content_select").show();
        	}
        	else{
        		$(".icon-flex").css("background-position", "0px 0px");
        		obj.text("展开");
        		$(".filter_range").css("border-bottom","1px solid #5f9cef");
        		$(".content_select").hide();
        	}
        	$(window).trigger("resize");
        });
        if($("#hr_report").find(".filter_range_btn").hasClass("active") == true){
        	$(".filter_range_btn").click();
        }

        /*报表升序降序切换*/
    	$("[data-sort]").click(function() {
        	if($(this).find("i").hasClass("icon_arrow_up") == false && $(this).find("i").hasClass("icon_arrow_down") == false){
        		$(this).find("i").addClass("icon_arrow_up");
        	}
        	else if($(this).find("i").hasClass("icon_arrow_up") == true){
        		$(this).find("i").removeClass("icon_arrow_up");
        		$(this).find("i").addClass("icon_arrow_down");
        	}
        	else if($(this).find("i").hasClass("icon_arrow_down") == true){
        		$(this).find("i").removeClass("icon_arrow_down");
        		$(this).find("i").addClass("icon_arrow_up");
        	}
        	$(this).siblings("[data-sort]").find("i").removeClass("icon_arrow_up icon_arrow_down");
        });

        /*筛选区域折叠*/
        $(".filter").find(".filter_btn").click(function() {
        	var obj =  $(this).find("span");
        	if( obj.text() == "展开"){
        		$(".icon-flex").css("background-position", "-20px 0px");
        		obj.text("收缩");
        		$(".filter_area").css("border-bottom","none");
        		$(".filter_table").show();
        	}
        	else{
        		$(".icon-flex").css("background-position", "0px 0px");
        		obj.text("展开");
        		$(".filter_area").css("border-bottom","1px solid #5f9cef");
        		$(".filter_table").hide();
        	}
        	$(window).trigger("resize");
        });

        /*修改项目按钮组*/
        $("#AddProductAll").click(function(event) {
        	var prArray = new Array();
        	$(".project_result select option").each(function() {
				prArray.push($(this).val());
			});
			$(".project_select select option").each(function() {
				if ($.inArray($(this).val(), prArray) == -1) {
					$(".project_result select").append($(this).clone());
					prArray.push($(this).val());
				}
			});
			$("#TrainingCourseID").val(prArray.join());
			prArray = [];
        });
        $("#AddProductSingle").click(function(event) {
        	var prArray = new Array();
        	$(".project_result select option").each(function() {
				prArray.push($(this).val());
			});
			$(".project_select select option:selected").each(function() {
				if ($.inArray($(this).val(), prArray) == -1) {
					$(".project_result select").append($(this).clone());
					prArray.push($(this).val());
				}
			});
			$("#TrainingCourseID").val(prArray.join());
			prArray = [];
        });
        $("#DropProductSingle").click(function(event) {
        	$(".project_result select option:selected").remove();
        	var prArray = new Array();
        	$(".project_result select option").each(function() {
				prArray.push($(this).val());
			});
			$("#TrainingCourseID").val(prArray.join());
			prArray = [];
        });
        $("#DropProductAll").click(function(event) {
        	$(".project_result select option").remove();
			$("#TrainingCourseID").val("");
        });

        $(document).on('dblclick', '.project_select select option', function(event) {
        	var prArray = new Array();
        	$(".project_result select option").each(function() {
				prArray.push($(this).val());
			});
			if ($.inArray($(this).val(), prArray) == -1) {
				$(".project_result select").append($(this).clone());
				prArray.push($(this).val());
				$("#TrainingCourseID").val(prArray.join());
			}
			prArray = [];
        });

        $(document).on('dblclick', '.project_result select option', function(event) {
        	$(this).remove();
        	var prArray = new Array();
        	$(".project_result select option").each(function() {
				prArray.push($(this).val());
			});
			$("#TrainingCourseID").val(prArray.join());
			prArray = [];
        });
	});
});