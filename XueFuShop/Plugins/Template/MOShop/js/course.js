 $(document).ready(function(){    
    // $(".course-category li").each(function(index, el) {
    //     var obj = $(".course-category-li[data-id=" + $(this).data("id") + "]");
    //     $(obj).find("li:eq(0)").removeClass("selected");
    //     $(obj).find("li:contains('" + $(this).text() + "')").addClass("selected");
    // });

    $(".more-category").click(function() {
        var obj = $(this).siblings(".category-list");
        if (obj.height() == 30 ) {
            obj.height("auto");
        }
        else{
            obj.height(30);
        }
    });
});

 function goPage(page){
    tempPage=page;
    searchProduct();
}