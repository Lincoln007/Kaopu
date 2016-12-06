//作用：纯显示信息  对颜色无改变
//作者：汪建龙
//编写时间：2016年12月6日14:53:08
function ShowMessage(message) {
    $("div.alert.alert-warning[name='Message']>span").html(message);
    $("div.alert.alert-warning[name='Message']").show(500);
}
//作用：隐藏提示框
//作者：汪建龙
//编写时间：2016年12月6日14:52:10
function HideMessage() {
    $("div.alert[name='Message']").hide();
}

//作用显示提示信息
//作者：汪建龙
//编写时间：2016年12月6日14:52:35
function ShowWarningMessage(message) {
    $("div.alert[name='Message']>span").before("<i class='icon-spin icon-spinner icon-large'></i>").html(message);
    $("div.alert[name='Message']").removeClass("alert-success").removeClass("alert-danger").addClass("alert-warning").show(500);
}
//作用：显示成功信息
//作者：汪建龙
//编写时间：2016年12月6日14:53:48
function ShowSuccessMessage(message) {
    $("div.alert[name='Message']>span").html(message);
    $("div.alert[name='Message']").removeClass("alert-warning").removeClass("alert-danger").addClass("alert-success").remove("i").show(500);
}
//作用：显示错误信息
//作者：汪建龙
//编写时间：2016年12月6日14:54:10
function ShowErrorMessage(message) {
    $("div.alert[name='Message']>span").html(message);
    $("div.alert[name='Message']").removeClass("alert-warning").removeClass("alert-success").addClass("alert-danger").remove("i").show(500);
}

//作用：显示节点
//作者：汪建龙
//编写时间：2016年12月6日20:31:19
function ShowNode(targetName) {
    $("tr[name=" + targetName + "]").show(500);
}

//作用：隐藏节点
//作者：汪建龙
//编写时间：2016年12月6日20:31:45
function HideNode(targetName) {
    $("tr[name=" + targetName + "]").hide(500);
    $("tr[name^='" + targetName + "']").hide(500);
    $("a[data-target^='" + targetName + "'] i").removeClass("glyphicon-minus-sign").addClass("glyphicon-plus-sign");
}