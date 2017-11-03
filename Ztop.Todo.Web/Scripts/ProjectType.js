$(function () {
    var SENode = $("#Second");
    var select = $("Select[name='SEID']");
    $("Select[name='FID']").change(function () {
        var val = $(this).val();
        if (val === undefined || val === "") {
            SENode.hide(500);
            select.empty();
            return;
        }
        $.getJSON("/ProjectType/Get?ppid=" + val, function (data) {
            select.empty().append("<option value=''>请选择二级</option>");
            $.each(data, function (key, val) {
                select.append("<option value='" + val.ID + "'>" + val.FullName + "</option>");
            });
            SENode.show(500);
        });

    });

    $("button[name='OK']").click(function () {
        var first = $("Select[name='FID']").find("option:selected").text();
        var second = $("Select[name='SEID']").find("option:selected").text();
        var name = select.val();

        if (name === undefined || name === "") {
            alert("获取二级项目类型失败，请重新尝试");
            return;
        }
        $("input[name='Type']").val(first + "-" + second);
        $("input[name='ProjectTypeId']").val(name);
    });

});