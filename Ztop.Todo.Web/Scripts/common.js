(function () {
    $.fn.setUpload = function (uploadUrl, callback, beforeUpload) {
        var url = uploadUrl;
        if (typeof (uploadUrl) == "function") {
            url = uploadUrl();
        }
        var file = $(this);
        var fileId = file.attr("id");
        if (!fileId) {
            fileId = Math.random();
            file.attr("id", "file-" + fileId);
        }
        var inputName = file.attr("name");
        if (!inputName) {
            file.attr("name", fileId);
            inputName = fileId;
        }
        if (url.indexOf("?") == -1) {
            url += "?";
        }
        url += "&inputName=" + inputName;
        var form = file.parents("form");
        var formAction = form.attr("action");
        var formTarget = form.attr("target");

        file.change(function () {
            if (beforeUpload && !beforeUpload()) {
                reset();
                return;
            }
            var targetId = "iframe_upload" + Math.random();
            var iframe = $('<iframe width="0" height="0" frameborder="0" id="' + targetId + '" name="' + targetId + '">');
            document.body.appendChild(iframe[0]);
            form.attr({
                target: targetId,
                action: url,
                enctype: "multipart/form-data",
                method: "POST"

            });
            form.submit();
            iframe.load(function () {
                var content = $(this).contents().find("pre").html() || $(this).contents().find("body").html();
                try {
                    var json = eval("(" + content + ")");
                    callback(json);
                } catch (ex) {
                    alert("上传出错了" + ex.message);
                }
                reset();
                iframe.remove();
            });
        });

        function reset() {
            var fileId = file.attr("id");
            var newFile = file.clone();
            newFile.value = "";
            file.replaceWith(newFile);
            form.removeAttr("target");
            form.removeAttr("enctype");
            form.attr("action", formAction);
            if (formTarget) {
                form.attr("target", formTarget);
            }
            $("#" + fileId).setUpload(uploadUrl, callback, beforeUpload);
        }
    };

    $.wait = function (canDo, callback) {
        var waite = setInterval(function () {
            if (canDo()) {
                clearInterval(waite);
                callback();
            }
        }, 10);
    };

    $.fn.serializeObject = function () {
        var form = this[0];
        if (!form) return null;
        var data = {};

        var arr = $(this).serializeArray();
        $.each(arr, function (i, item) {
            setFields(data, item.name, item.value);
        });

        return data;

        function setFields(data, name, value) {
            if (name.indexOf('.') > 0) {
                var names = name.split('.');
                var subName = names[1];
                name = names[0];
                if (!data[name]) data[name] = {};
                setFields(data[name], subName, value);
                return;
            }

            if (data[name]) {
                data[name] += "," + value;
            }
            else {
                data[name] = value;
            }
        }
    };

    $.fn.POST = function (options) {

        options = options || {};
        if (options.validate && !options.validate()) {
            return false;
        }

        options.url = options.url || $(this).attr("action");

        options.data = options.data || $(this).serializeObject();

        $.request(options);
    };

    $.request = function (url, data, success, error, global) {
        var options = null;
        if (arguments.length == 1) {
            switch (typeof (arguments[0])) {
                case "object":
                    options = arguments[0];
                    break;
                case "string":
                    options = { url: url };
            }
        } else {
            switch (typeof (arguments[1])) {
                case "function":
                    options = {
                        url: arguments[0],
                        data: null,
                        success: arguments[1],
                        error: arguments.length > 2 ? arguments[2] : null,
                        global: arguments.length > 3 ? arguments[3] : null,
                    };
                    break;
                default:
                    options = {
                        url: url,
                        data: data,
                        success: success,
                        error: error,
                        global: global
                    };
                    break;
            }
        }
        if (options.url.indexOf('?') == -1) {
            options.url += "?";
        }
        options.url += "&_=" + Math.random();
        $.ajax({
            type: options.data ? "POST" : "GET",
            dataType: "json",
            global: options.global == undefined,
            url: options.url,
            data: options.data
        }).done(function (json, statusText, xhr) {
            if (options.success) {
                options.success(json, statusText, xhr);
            }
        }).fail(function (xhr) {
            try {
                if (xhr.responseText) {
                    var data = eval("(" + xhr.responseText + ")");
                    if (options.error) {
                        options.error(data);
                    }
                }
            } catch (err) {
                alert(err);
            }
        });
    };

})();

$.ajaxSetup({
    beforeSend: function (xhr) {
        xhr.setRequestHeader("submit-type", "ajax");
    }
});

$(document).ajaxError(function (event, jqxhr, settings, exception) {
    if (jqxhr.responseText) {
        try {
            var result = $.parseJSON(jqxhr.responseText);
            alert(result.message || result.content || "未知错误");
        } catch (ex) {

        }
    }
});
