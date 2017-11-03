$(function () {
    $("Select[name='Type']").change(function () {
        var val = $(this).val();
        var node = $("#Cate");
        if (val == "Daily" || val == "Transfer") {
            node.show(500);
        } else {
            node.hide(500);
        }
    });

    $("Select[name='GroupId']").change(function () {
        var val = $(this).val();
        var node = $("#User");
        var select = $("Select[name='Name']");
        if (val == undefined || val == "") {
            node.hide(500);
        } else {
            node.show(500);
            $.getJSON("/Bank/GetUsers?groupId=" + val, null, function (data) {
                select.empty().append("<option value=''>所有用户</option>");
                $.each(data, function (key, val) {
                    select.append("<option value='" + val.RealName + "'>" + val.RealName + "</option>");
                });
            });

        }
    });

   
});