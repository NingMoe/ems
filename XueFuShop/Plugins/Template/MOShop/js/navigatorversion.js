/*function getNaviVer(){
	var navigatorVers={};
	var ua = navigator.userAgent.toLowerCase();
	var s;
	(s = ua.match(/msie ([\d.]+)/)) ? navigatorVers.ie = s[1] :
	(s = ua.match(/firefox\/([\d.]+)/)) ? navigatorVers.firefox = s[1] :
	(s = ua.match(/chrome\/([\d.]+)/)) ? navigatorVers.chrome = s[1] :
	(s = ua.match(/opera.([\d.]+)/)) ? navigatorVers.opera = s[1] :
	(s = ua.match(/version\/([\d.]+).*safari/)) ? navigatorVers.safari = s[1] : 0;
	
	if (navigatorVers.ie) return ('IE:' + navigatorVers.ie);
	if (navigatorVers.firefox) return ('Firefox:' + navigatorVers.firefox);
	if (navigatorVers.chrome) return ('Chrome:' + navigatorVers.chrome);
	if (navigatorVers.opera) return ('Opera:' + navigatorVers.opera);
	if (navigatorVers.safari) return ('Safari:' + navigatorVers.safari);
	return "unknow";
}
function checkNaviVer(){
	if("Microsoft Internet Explorer"==navigator.appName){
		var vNaviVer=getNaviVer();
		if(vNaviVer.indexOf("IE:") > -1){
			if(parseInt(vNaviVer.split(":")[1])<8){
				$('#tips_nav_vertoolow').get(0).style.display="block";
				var mask='<div id="naiVerTooLowMask" class="ccb_mask"></div>';
				$("body").css({"overflow":"hidden"}).append(mask);
			}
		}
	}
}*/
/*var navigatorName = "Microsoft Internet Explorer";
if( navigator.appName == navigatorName ){ 
   	$("#tips_nav_vertoolow").css("display","block");
	var mask = "<div id='naiVerTooLowMask' class='mt_mask'></div>";
	$("body").css({"overflow":"hidden"}).append(mask);
}*/
function isIE() { //ie?
    if (!!window.ActiveXObject || "ActiveXObject" in window)
    	return true;
    else
        return false;
}
var ie = isIE();
if( ie == true ){ 
   	$("#tips_nav_vertoolow").css("display","block");
	var mask = "<div id='naiVerTooLowMask' class='mt_mask'></div>";
	$("body").css({"overflow":"hidden"}).append(mask);
}