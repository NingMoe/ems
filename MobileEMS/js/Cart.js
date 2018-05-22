//读取购物车
function readCart(){
    //loading("CartProductAjax","购物车");
    $.get("CartAjax.aspx?Action=Read",dealReadCart);
}
function dealReadCart(data){
    $("#CartProductAjax").html(data);
}
//清空购物车
function clearCart(){
    $.get("CartAjax.aspx?Action=ClearCart",dealClearCart)
}
function dealClearCart(data){
    $("#CartProductAjax").html("");
    $("#ProductBuyCount").html("0");
    $("#ProductTotalPrice").html("0");
}
//改变购买数量
function changeOrderProductBuyCount(strCartID,buyCountObj,price,productCount,leftStorageCount,productWeight){
    var buyCount=buyCountObj.value;
    var oldCount=$("#BuyCount"+strCartID).val();
    if(buyCount!=oldCount){
        if(parseInt(buyCount)>0){
            //if(parseInt(buyCount)<=leftStorageCount){   
                $.get("CartAjax.aspx?Action=ChangeBuyCount&StrCartID="+strCartID+"&BuyCount="+buyCount+"&OldCount="+oldCount+"&Price="+price+"&ProductCount="+productCount+"&ProductWeight="+productWeight,dealChangeCart);
            //}
//            else{
//                buyCountObj.value=oldCount;
//                alertMessage("当前库存不能满足您的购买数量");
//            }
        }
        else{
            //alertMessage("数量填写有错误");
        }
    }
}
function dealChangeCart(data){
    try{
        var dataArray=data.split("|");
        //o("ProductBuyCount").innerHTML=dataArray[1];
//        o("ProductTotalPrice").innerHTML=dataArray[2];
        $("#CartProductTotalPrice").html("￥"+dataArray[2]);
        $("#CartProductPrice"+dataArray[0]).html("￥"+parseFloat(dataArray[3]).toFixed(2));
        $("#BuyCount"+dataArray[0]).val(dataArray[4]);
     }
    catch(e){//alertMessage("修改失败");
	}
}
//删除购物车
function deleteOrderProduct(strCartID,price,productCount,productWeight){
    var oldCount=$("#BuyCount"+strCartID).val();
    $.get("CartAjax.aspx?Action=Delete&StrCartID="+strCartID+"&OldCount="+oldCount+"&Price="+price+"&ProductCount="+productCount+"&ProductWeight="+productWeight,dealDeleteCart);    
}
function dealDeleteCart(data){
    try{
        var dataArray=data.split("|");
        //$("#ProductBuyCount").html(dataArray[1]);
        //$("#ProductTotalPrice").html(dataArray[2]);
        $("#CartProductTotalPrice").html("￥"+dataArray[2]);
        $("#Cart"+dataArray[0]).remove();
    }
    catch(e){//alertMessage("删除失败");
	}
}

//添加到购物车
function addToCart(productID,productName,currentMemberPrice,productStandardType){
    var check=true;
    var attributeName="";
    if(productStandardType=="1"){
        var standardValue=getTextValue(os("name","StandardValue"));       
        for(var i=0;i<standardValue.length;i++){
            attributeName+=standardValue[i]+",";
            if(standardValue[i]==""){
                check=false;
                break;
            }
        }
    }
    if(check){
        var buyCount=1;//$("#BuyCount").val();
        if(parseInt(buyCount)>0){
            if(attributeName!=""){
                productName=productName+"("+attributeName.substr(0,attributeName.length-1)+")";
            }
            var url="CartAjax.aspx?Action=AddToCart&ProductID="+productID+"&ProductName="+ encodeURIComponent(productName)+"&BuyCount="+buyCount+"&CurrentMemberPrice=0";//+currentMemberPrice;
            $.get(url,dealAddToCart);
        }
        else{
            alertMessage("数量填写有错误");
        }
    }
    else{
        alertMessage("请选择规格");
    }
}

//立即购买
var redirect=false;
function buyNow(productID,productName,productStandardType){
    addToCart(productID,productName,productStandardType);
    redirect=true;
}
function dealAddToCart(content){
    if(content=="ok"){
        if(redirect){//立即购买 
            redirect=false; 
            window.location.href="/Cart.aspx";
        }
        else{//添加到购物车
		    alert("添加成功");
            //alertMessage("添加成功");
            //var buyCount=$("#BuyCount").val();
//            var currentMemberPrice=$("#CurrentMemberPrice").val(); 
//            $("#ProductBuyCount").html(parseInt($("#ProductBuyCount").html())+parseInt(buyCount));
//            $("#ProductTotalPrice").html(parseFloat($("#ProductTotalPrice").html())+parseInt(buyCount)*parseFloat(currentMemberPrice));
        }
    }
    else{
        redirect=false; 
        alert(content);
    }
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
function closeAlertDiv() {
    var alerObj = $("#AlertMessage");
    if (alerObj != null) {
       // document.body.removeChild(alerObj);
		$(document).remove(alerObj);
    }
 }
readCart();