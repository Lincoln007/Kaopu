$(function () {

    var cityNode = $("#City");
    var districtNode = $("#District");
    var citySelect = $("Select[name='CID']");
    var districtSelect = $("Select[name='DID']");

    $("Select[name='PID']").change(function () {

        districtNode.hide(500);
        districtSelect.empty();
        var val = $(this).val();
        if (val == undefined || val == "") {
            cityNode.hide(500);
            citySelect.empty();
            return false;
        }
        $.getJSON("/City/Get?Rank=City&&CityID=" + val, null, function (data) {
            citySelect.empty().append("<option value=''>请选择</option>");
            $.each(data, function (key, val) {
                citySelect.append("<option value='" + val.ID + "'>" + val.Name + "</option>");
            });
            cityNode.show(500);
        });

        return false;
    });

    $("Select[name='CID']").change(function () {
        var val = $(this).val();
        if (val == undefined || val == "") {
            districtNode.hide(500);

            return false;
        }

        $.getJSON("/City/Get?Rank=District&&CityID=" + val, null, function (data) {
            districtSelect.empty().append("<option value=''>请选择</option>")
            $.each(data, function (key, val) {
                districtSelect.append("<option value='" + val.Name + "'>" + val.Name + "</option>");
            });
            districtNode.show(500);
        });
        return false;
    });

    $("button[name='OK']").click(function () {
        var province = $("Select[name='PID']").find("option:selected").text();
        var city = $("Select[name='CID']").find("option:selected").text();
        var name = $("Select[name='DID']").val();
        if (name == undefined || name == "") {
            alert("未获取城市信息,请重新尝试");
            return;
        }
        $("input[name='CityName']").val(province + "-" + city + "-" + name);
    });
});