function checkLogin() {
    if(jQuery("#UserName").val()==""){
        alertMessage("用户名不能为空");
        return false;
    }
    if(jQuery("#UserPassword").val()==""){
        alertMessage("密码不能为空");
        return false;
    }
    if(jQuery("#SafeCode").val()==""){
        alertMessage("验证码不能为空");
        return false;
    }
    return true
}
//提示信息
function alertMessage(message, checked) {
    var alertDiv = document.createElement("div");
    var divWidth = 200;
    var divHeight = 50;
    var time = 2000;
    var screenWidth = (window.innerWidth || document.documentElement && document.documentElement.clientWidth || document.body.clientWidth);
    var screenHeight = (window.innerHeight || document.documentElement && document.documentElement.clientHeight || document.body.clientHeight); 
    var scrollLeft = (document.documentElement.scrollLeft || document.body.scrollLeft);
    var scrollTop = (document.documentElement.scrollTop || document.body.scrollTop);
    var top = (screenHeight - divHeight) / 2 + scrollTop;
    var left = (screenWidth - divWidth) / 2 + scrollLeft;
    with (alertDiv) {
        style.border = "#d78e42 1px solid";
        style.background = "#ffffcc";
        style.position = "absolute";
        style.padding = "12px";
        style.left = left + "px";
        style.top = top + "px";
        style.width = divWidth+"px";
        style.textAlign = "center";
        style.color="#FF0000";
        style.fontWeight="bold";
        id = "AlertMessage";
        innerHTML = message;
    }
    document.body.appendChild(alertDiv);
    if (checked != "1") {    
        setTimeout("closeAlertDiv()", time);
    }
}