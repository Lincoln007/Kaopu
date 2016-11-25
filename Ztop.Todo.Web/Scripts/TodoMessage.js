function ShowMessage(message) {
    $("div.alert.alert-warning[name='Message']>span").html(message);
    $("div.alert.alert-warning[name='Message']").show(500);
}

function HideMessage() {
    $("div.alert.alert-warning[name='Message']").hide();
}