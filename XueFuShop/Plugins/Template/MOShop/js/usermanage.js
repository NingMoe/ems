$(document).ready(function(){
    $("#DeleteButton").click(function(event) {
        var checkValue = getCheckValue($("input:checkbox[name='SelectID']"));
        if (checkValue.length > 0) {
            deleteItems(checkValue);
        }
        else {
            layer.msg("没有选择项目！");
        }
    });

    $("#FreezeButton").click(function(event) {
        var checkValue = getCheckValue($("input:checkbox[name='SelectID']"));
        if (checkValue.length > 0) {
            FreezeItems(checkValue);
        }
        else {
            layer.msg("没有选择项目！");
        }
    });

    $("#UnFreezeButton").click(function(event) {
        var checkValue = getCheckValue($("input:checkbox[name='SelectID']"));
        if (checkValue.length > 0) {
            UnFreezeItems(checkValue);
        }
        else {
            layer.msg("没有选择项目！");
        }
    });

    $("#FreeButton").click(function(event) {
        var checkValue = getCheckValue($("input:checkbox[name='SelectID']"));
        if (checkValue.length > 0) {
            FreeItems(checkValue);
        }
        else {
            layer.msg("没有选择项目！");
        }
    });

    var getCheckValue = function(obj) {
        var checkValue = "";
        if ($(obj).length > 0) {
            $(obj).each(function(index, el) {
                if ($(this).prop("checked") == true){
                    checkValue += "," + $(this).val();
                }
            });
            checkValue = checkValue.substring(1, checkValue.length);
        }
        return checkValue;
    }

    var FreezeItems = function(selectID){
        layer.confirm('确定要冻结用户吗？', {
            btn: ['确定','取消'], //按钮
        }, function(){
            layer.closeAll();
            window.location.href = "?Action=Freeze&SelectID=" + selectID;
        });
    }

    var UnFreezeItems = function(selectID){
        layer.confirm('确定要把用户状态更改为正常吗？', {
            btn: ['确定','取消'], //按钮
        }, function(){
            layer.closeAll();
            window.location.href = "?Action=UnFreeze&SelectID=" + selectID;
        });
    }

    var FreeItems = function(selectID){
        layer.confirm('确定要把用户设置为不考核吗？', {
            btn: ['确定','取消'], //按钮
        }, function(){
            layer.closeAll();
            window.location.href = "?Action=Free&SelectID=" + selectID;
        });
    }

    
});

var pop = function(url,title,width,height){
    layer.open({
      type: 2,
      title: title,
      shade: 0.8,
      area: [width, height],
      content: url //iframe的url
    }); 
}

var newPage = function(url) {
    window.location.href = url;
}

var deleteItems = function(selectID){
    layer.confirm('确定要删除吗？', {
        btn: ['确定','取消'], //按钮
    }, function(){
        layer.closeAll();
        window.location.href = "?Action=Delete&SelectID=" + selectID;
    });
}