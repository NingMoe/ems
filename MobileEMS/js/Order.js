//订单操作
var tempOrderStatus;
var tempOrderID;
function orderOperate(orderID,orderStatus){
    if(window.confirm("确定操作？")){
        tempOrderID=orderID;
        tempOrderStatus=orderStatus;
        var url = "CartAjax.aspx?Action=OrderOperate&OrderID=" + orderID+"&OrderStatus=" + orderStatus;
        $.get(url, dealOrderOperate);
    }
}
function dealOrderOperate(content){
    if(content!=""){
        alertMessage(content);
    }
    else{
        alert("操作成功");
        if(tempOrderStatus=="1" || tempOrderStatus=="2"){//未付款或者未审核
            $("#OrderStatus"+tempOrderID).html("无效");
        }
        else if(tempOrderStatus=="5"){//已发货
            $("#OrderStatus"+tempOrderID).html("已收货");
        }
        else{
        }
        $("#OrderOperate"+tempOrderID).html("<a href=\"/User/OrderDetail.aspx?ID="+tempOrderID+"\">查看</a>");
    }
}