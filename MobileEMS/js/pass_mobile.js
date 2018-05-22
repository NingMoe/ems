
$(function(){
    setTimeout(scrollTo,0,0,0);
    $("input").focus(function(){
        $(this).addClass("focus");
    }).blur(function(){
        $(this).removeClass("focus");
    });
    $("#btn_register").click(function(){
        $("#register_suc").addClass("");
    });
    $(".link_top").click(function(){
        setTimeout("window.scrollTo(0,0)",300);
    });
});

function addFocus(obj,msg) {
    $(obj).nextAll('p:first').html(msg).show();
    $(obj).parent('div.input_box').addClass('error');
}
function clearFocus(obj) {
    $(obj).nextAll('p:first').html('');
    $(obj).parent('div.input_box').removeClass('error');
}


Array.prototype.unique =
  function () {
      var a = [];
      var l = this.length;
      for (var i = 0; i < l; i++) {
          for (var j = i + 1; j < l; j++) {
              // If this[i] is found later in the array
              if (this[i] === this[j])
                  j = ++i;
          }
          a.push(this[i]);
      }
      return a;
  };


function RemoveArray(array, attachId) {
    for (var i = 0, n = 0; i < array.length; i++) {
        if (array[i] != attachId) {
            array[n++] = array[i]
        }
    }
    array.length -= 1;
}


Array.prototype.contains = function (arr) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == arr) {
            return true;
        }
    }
    return false;
}

Array.prototype.remove = function (obj) {
    return RemoveArray(this, obj);
};