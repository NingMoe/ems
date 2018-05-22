$(function(){
	var $isPostCheck = $("#IsPostCheck"),
	$kpiIDString = $("#kpiidstr"),
	$kpiListArea = $("#KPIListArea");

	// 更改按钮事件
	var changeButton = function() {
		if($isPostCheck.is(":checked")){
			$("#"+globalIDPrefix+"next").attr("value","下一步").removeClass("complete").addClass("next");
		}
		else{
			$("#"+globalIDPrefix+"next").attr("value","保存提交").removeClass("next").addClass("complete");
		}
	}	

	// 是否岗位复选框勾选事件
	$isPostCheck.change(function(){
	    var companyid=parseInt($("#CompanyId").val());

	    changeButton();
    	if($(this).is(':checked')){
        	if(companyid > 0){
                $.post("Ajax.aspx?Action=GetKPIListByCompanyId",{CompanyId:companyid},function(data){
                    $kpiListArea.data("typeid", "0").html(data);
                })
        	}
    	}
    });

	// 公司名称下拉菜单
	$("#"+globalIDPrefix+"CompanyName").bigAutocomplete({
	    width:"400px",
		url:"Ajax.aspx?Action=SearchCompanyName",
		callback:function(data){
		    $("#CompanyId").val(data.result.Id);
		    $.post("Ajax.aspx?Action=GetWorkPostList",{CompanyId:data.result.Id},function(data){
                $("#"+globalIDPrefix+"ParentId").html(data);
            })
		    $isPostCheck.change();
		}
	});

	// 指标勾选 “通用" 设置
	$kpiListArea.on("click",".schedule", function() {
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
		}else {
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

	$kpiListArea.on("click",".choose", function() {
		$(this).nextAll(".schedule").click();
	});

	var getKPISelected = function() {
		var kpiID = "";
		$(".schedule").each(function() {
			if ($(this).text() == "√") {
				var dataId = $(this).siblings(".evaluation_content").attr("data-id");
				if (kpiID == "") {
					kpiID = dataId;
				}
				else{
					kpiID += "," + dataId;
				}
			}			
		});
		return kpiID
	}

	// 评价指标载入时原始数据打勾
	function setKpi(kpi){
		// 清空现有设置
		$kpiListArea.find(".schedule").text("").parent().removeClass("active");
		var kpiidArray = kpi.split(",");
		$.each(kpiidArray, function(index, val) {
			$kpiListArea.find("[data-id='"+val+"']").siblings(".schedule").click();
		});
	}

	// 相关岗位指标预设
	var currentDataid = "";
	$(".preset_post li").click(function(event) {			
		var dataKpi = $(this).attr("data-kpi");
		if ($(".preset_post li.active").length > 0) {
			if($(this).hasClass("active")){
				$(this).removeClass("active");
				setKpi(currentDataid);
				currentDataid = "";
			}
			else{
				$(this).addClass("active").siblings().removeClass('active');
				setKpi(dataKpi);
			}	
		}
		else{
			currentDataid = getKPISelected();
			$(this).addClass("active");
			setKpi(dataKpi);
		}
	});

	// KPI列表中 行显示 设置
	var setKPIRowShow = function(typeID) {
		$kpiListArea.show().find('[data-type]').hide().filter("[data-type='" + typeID + "']").show();
		$kpiListArea.data("typeid", typeID);
		$("#prev").show();
	}

	function scrollTop(){
		$("body,html").animate({ scrollTop: 0 }, 0);
	}

	// 评价指标下一步按钮
	$("#"+globalIDPrefix+"next").click(function(e) {
		var currentTypeID = parseInt($kpiListArea.data("typeid")) + 1;
		if (!validateForm()) {e.preventDefault();return false;}
		if(currentTypeID > 3) {
			$("#kpiidstr").val(getKPISelected());
		}
		else {
			$(".add").hide();
			$(".listTable").show();
			$("#preset_indicator").show();
			setKPIRowShow(currentTypeID);
			if(currentTypeID == 3) {
				$(this).attr("value","完成");
			}
			else {
				$(this).attr("value","下一步");
			}
			e.preventDefault();
			scrollTop();
		}
	});

	// 评价指标上一步按钮
	$("#prev").click(function() {
		var currentTypeID = parseInt($("#KPIListArea").data("typeid")) - 1;
		if(currentTypeID >= 0) {
			setKPIRowShow(currentTypeID);
			$("#"+globalIDPrefix+"next").attr("value","下一步");
			if(currentTypeID == 0) {
				$("#preset_indicator").hide();
				$(".listTable").hide();
				$(".add").show();
				$(this).hide();
			}
		}
	});

	/*评价指标岗位验证*/
	var validateForm = function (){
		var companyName = $("#"+globalIDPrefix+"CompanyName").val(),
		companyID =  parseInt($("#CompanyId").val()),
		parentID=$("#"+globalIDPrefix+"ParentId").val(),
		postName=$("#"+globalIDPrefix+"PostName").val();

		if (companyName == "") {
			return false;
		}
		if (companyID < 0) {
			return false;
		}
		if (postName == "") {
			return false;
		}
		return true;
	}

	// 如果有已选指标数据，则设置勾选
	if ($kpiIDString.val().length > 0){
		$isPostCheck.prop("checked", true);
		changeButton();
		setKpi($kpiIDString.val());
	}
})