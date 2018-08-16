$(function () {
    $("a[name='Delete']").click(function () {
        var href = $(this).attr("href");
        var url = $(this).attr("Url");
        if (confirm("您确定要删除吗？")) {
            $.getJSON(href, function (data) {
                if (data.result === 1) {
                    location.href = href;
                } else {
                    alert(data.content);
                }
            });
        }
        return false;
    });

});