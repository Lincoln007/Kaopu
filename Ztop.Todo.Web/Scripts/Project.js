$(function () {
    $("#Project-Number").submit(function () {
        var btn = $("#Project-Number button[type='submit']");
        ShowMessage("正在录入登记编号，请稍等......");
        var id = $("input[type='hidden'][name='ID']").val();
        var data = $(this).serialize();
        btn.attr("disabled", "disabled");
        var href = $(this).attr("action");
        $.request(href, data, function (json) {
            if (json.result == 0) {
                ShowErrorMessage(json.content);
                btn.removeAttr("disabled");
            } else {
                ShowSuccessMessage("成果输入登记编号，正在进行刷新，请稍等....");
                location.href = "/Project/Home/Detail?id=" + id;
            }

        });
        return false;
    });

   
    $("a[name='Post']").click(function () {
        var $btn = $(this);
        $btn.attr("disabled", "disabled");
        var url = $(this).attr("Url");
        var href = $(this).attr("href");
        $.request(href, null, function (json) {
            if (json.result === 1) {
                alert("成功提交");
                location.href = url;
            } else {
                alert(json.content);
                $btn.removeAttr("disabled");
            }
        });

        return false;
    });

});