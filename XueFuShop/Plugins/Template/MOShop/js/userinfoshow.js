$(function(){
    $(".regisubmit").click(function() {
        $.validator.addMethod("isEmail", function(value, element) {
            var email = /^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$/;
            return this.optional(element) || (email.test(value));
        }, "电子邮箱格式不正确");
        $.validator.addMethod("isTel", function(value, element) {
            var tel = /^(\d{3,4}-?)?\d{7,8}$/g;
            return this.optional(element) || (tel.test(value));
        }, "请正确填写您的电话号码");
        $("#rightform").validate({
            rules: {
                Email:{
                    required:true,
                    isEmail:true,
                },
                Tel:{
                    isTel:true,
                },
                WorkingPostName:{
                    required:true,
                },
            },
            messages: {
                Email: {
                    required:"电子邮箱不能为空"
                },
                WorkingPostName:{
                    required:"职位不能为空",
                },
            },
        });
    });
})