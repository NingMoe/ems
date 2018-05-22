var PreAname = "aCommon";
function switchBlock(Oname,MenuURL,RightURL){
    Aname = "a" + Oname;
    o(PreAname).className = "";
    o(Aname).className = "on";
    PreAname=Aname;
    o("LeftFrame").src = MenuURL;
    o("RightFrame").src = RightURL;
}
function goUrl(url){
    o("RightFrame").src = url;
}
function countHeight(){
    var bodyHeight=(document.body.clientHeight-85)+"px";
    if (window.innerHeight)
    bodyHeight = window.innerHeight;
    else if ((document.body) && (document.body.clientHeight))	
    bodyHeight = document.body.clientHeight;
	if(document.documentElement.clientHeight>bodyHeight)
	bodyHeight=document.documentElement.clientHeight;
    o("Body").style.height=bodyHeight-85+"px";
    o("LeftFrame").style.height=bodyHeight;
}
countHeight();
bindEvent("resize", countHeight);