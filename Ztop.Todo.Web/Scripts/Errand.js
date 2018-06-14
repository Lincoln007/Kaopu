
$(function () {

    $("button[name='Traffic-OK']").click(function () {
        var price = CalculateUnitPrice();
        $("input[name='UnitPrice']").val(price);
        StatisticTraffic();
        StatisticSubSide();
    });

    //公司车栏中选择司机的时候，自动计算车补
    $("select[name='Driver']").change(function () {
        var index = parseInt($(this).attr("targetIndex"));
        if (index == undefined || isNaN(index)) {
            alert("参数获取失败，请刷新网页");
            return false;
        }
        var money = .0;
        var kilo = parseFloat($("input[name='KiloMeters']")[index].value);
        if (!isNaN(kilo) && kilo != undefined) {
            var driver = $(this).val();
            if (driver == "无") {
                money = 0;
            } else if (driver == "孙海杰") {
                money = kilo * 0.5;
            } else {
                money = kilo * 0.3;
            }
            //var ID = index + "#CarPetty";
            //$(ID).val(money);
            $("#CarPetty" + index).val(money);
            if (index == 0) {
                StatisticCompany();
            } else {
                StatisticPersonal();
            }
            
        } else {
            alert("未获取公里数，无法计算车补金额！");
        }
    });

    //绑定公司车以及自备车栏中填写油费之后，自动计算合计金额
    $("input[name='Petrol']").change(function () {
        StatisticCompany();
    });

    //绑定公司车以及自备车栏中填写过路费之后，自动计算合计金额
    $("input[name='Toll']").change(function () {
        var type = $(this).attr("targetType");
        if (type == "Company") {
            StatisticCompany();
        } else if (type = "Personal") {
            StatisticPersonal();
        }
    });

    //绑定公司车以及自备车栏中填写公里数之后，自动计算合计金额
    $("input[name='KiloMeters']").change(function () {
        var type = $(this).attr("targetType");
        if (type == "Company") {
            StatisticCompany();
        } else if (type == "Personal") {
            StatisticPersonal();
        }
    });
});



//交通费用中点击确定，自动计算交通费用
function StatisticTraffic() {
    var sum = 0;
    $("input[name='BusType']:checked").each(function () {
        var type = $(this).val();
        if (type != "InternetCar") {
            var cost = parseFloat($("input[name='Cost" + type + "']").val());
            if (!isNaN(cost)) {
                sum += cost;
            }
        }
    });

    $("input[name='Traffic']").val(sum);
    SumErrand();
}

//计算出差补贴费用 应为60元每人还是80元每人
function CalculateUnitPrice() {
    var price = 80;
    $("input[name='BusType']:checked").each(function () {
        var type = $(this).val();
        if (type === "Company" || type === "Personal") {
            price = 60;
        }
    });

    return price;
}



//自动统计公司车的金额
function StatisticCompany() {
    var sum = 0;
    var carPetty = parseFloat($("input[name='CarPetty']")[0].value);
    if (!isNaN(carPetty)) {
        sum += carPetty;
    }
    var petrol = parseFloat($("input[name='Petrol']").val());
    if (!isNaN(petrol)) {
        sum += petrol;
    }
    var toll = parseFloat($("input[name='Toll']")[0].value);
    if (!isNaN(toll)) {
        sum += toll;
    }
    $("input[name='CostCompany']").val(sum);
}

//自动统计自备车的金额
function StatisticPersonal() {
    var sum = 0;
    var kile = parseFloat($("input[name='KiloMeters']")[1].value);
    if(!isNaN(kile)){
        sum += kile * 1.5;
    }
    var petty = parseFloat($("input[name='CarPetty']")[1].value);
    if (!isNaN(petty)) {
        sum += petty;
    }
    $("input[name='CostPersonal']").val(sum);
}


$("button[name='SubSide-OK']").click(function () {
    if (CheckUsers()) {

        if (StatisticSubSide()) {
            return true;
        } else {
            return false;
        }
    } else {
        alert("目前尚有用户未制定时间");
        return false;
    }

});
//核对是否所有用户都填写时间了
function CheckUsers() {
    var persons = $("input[name='Persons']").val().split(';');
    var checks = [];
    $("#ViewSubSide input[name='mans']:checked:disabled").each(function () {
        checks.push($(this).val());
    });
    if (checks.length != persons.length) {
        return false;
    } else {
        return true;
    }
}

//统计出差补贴金额
function StatisticSubSide() {
    var lines = [];
    var subSide = 0;
    var price = $("input[name='UnitPrice']").val();
    if (price === "") {
        alert("未获取差补金额，请点击‘取消’之后填写交通详情再填写补贴详情！");
        return false;
    }
    $("#ViewTime input[name*='Users']").each(function () {
        var users = $(this).val();
        var targetIndex = $(this).attr("targetIndex");
        var start = $("#ViewTime input[name='StartTime" + targetIndex + "']").val();
        var end = $("#ViewTime input[name='EndTime" + targetIndex + "']").val();
        var d1 = new Date(start);
        var d2 = new Date(end);
        if (d1 == undefined || d2 == undefined) {
            alert(users + "的时间信息获取失败！");
            return false;
        }
        if (d2 < d1) {
            alert(users + "的结束时间小于开始时间，请检查核对！");
            return false;
        }
        var day_d1 = d1.getTime() / 1000 / 3600 / 24;
        var day_d2 = d2.getTime() / 1000 / 3600 / 24;
        var peoples = users.split(';').length;
        var days = day_d2 - day_d1 + 1;
        $("#ViewTime input[name='Peoples" + targetIndex + "']").val(peoples);
        $("#ViewTime input[name='Days" + targetIndex + "']").val(days);
        subSide += peoples * days * price;
        lines.push(targetIndex);
    });
    $("input[name='SubSidy']").val(subSide);
    $("input[name='Lines']").val(lines.join(';'));
    SumErrand();
    return true;
}