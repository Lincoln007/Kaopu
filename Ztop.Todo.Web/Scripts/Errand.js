
$(function () {

    $("button[name='Traffic-OK']").click(function () {
        StatisticTraffic();
    });

    //公司车栏中选择司机的时候，自动计算车补
    $("select[name='Driver']").change(function () {
        var money = .0;
        var kilo = parseFloat($("input[name='KiloMeters']")[0].value);
        if (kilo != NaN && kilo != undefined) {
            var driver = $(this).val();
            if (driver == "无") {
                money = 0;
            } else if (driver == "孙海杰") {
                money = kilo * 0.5;
            } else {
                money = kilo * 0.2;
            }
            $("input[name='CarPetty']").val(money);
            StatisticCompany();
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



//自动统计公司车的金额
function StatisticCompany() {
    var sum = 0;
    var carPetty = parseFloat($("input[name='CarPetty']").val());
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
    if (!isNaN(kile)) {
        $("input[name='CostPersonal']").val(kile * 1.5);
    }
}