(function ($) {
    $.extend({
        getRadWindow: function () {
            var oWindow = null;
            if (window.radWindow)
                oWindow = window.radWindow;
            else if (window.frameElement.radWindow)
                oWindow = window.frameElement.radWindow;
            return oWindow;
        },

        openRadWindow: function (url, windowName, modal, width, height) {
            var oManager = GetRadWindowManager();
            var oWnd = oManager.open(url, windowName);

            if (width)
                oWnd.set_width(width);

            if (height)
                oWnd.set_height(height);

            oWnd.set_modal(modal);
            oWnd.center();

            return oWnd;
        },

        closeRadWindow: function (radWindow, refresh) {

            if (radWindow && !radWindow.isClosed()) {
                radWindow.close();
            }

            if (refresh) {
                top.location.href = top.location.href;
            }

        },

        refresh: function () {
            window.location.href = window.location.href;
        },

        AutoResizeImage: function (maxWidth, maxHeight, objImg) {
            var img = new Image();
            img.src = objImg.src;
            var hRatio;
            var wRatio;
            var Ratio = 1;
            var w = img.width;
            var h = img.height;
            if (w > 0 && h > 0) {
                wRatio = maxWidth / w;
                hRatio = maxHeight / h;
                if (maxWidth == 0 && maxHeight == 0) {
                    Ratio = 1;
                } else if (maxWidth == 0) {//
                    if (hRatio < 1) Ratio = hRatio;
                } else if (maxHeight == 0) {
                    if (wRatio < 1) Ratio = wRatio;
                } else if (wRatio < 1 || hRatio < 1) {
                    Ratio = (wRatio <= hRatio ? wRatio : hRatio);
                }
                if (Ratio < 1) {
                    w = w * Ratio;
                    h = h * Ratio;
                }
                objImg.height = h;
                objImg.width = w;
            }
        },

        showLoading: function () {
            if (top.loadingPanel != null)
                top.loadingPanel.show("mws-wrapper");
        },

        hideLoading: function () {
            if (top.loadingPanel != null)
                top.loadingPanel.hide("mws-wrapper");
        },

        doAjaxAction: function (handlerFileName, requestData, successCallBack, dataType) {

            $.showLoading();

            var requestUrl = $.getRootPath() + "HttpHandle/" + handlerFileName + ".ashx";

            var ajaxDataType;
            if (dataType)
                ajaxDataType = dataType;
            else
                ajaxDataType = "json";

            $.ajax({
                type: "post",
                dataType: ajaxDataType,
                url: requestUrl,
                data: { RequestData: requestData },
                success: function (returnStr) {
                    $.hideLoading();
                    var rtnObj;
                    if (ajaxDataType == "json")
                        rtnObj = returnStr;
                    else
                        rtnObj = JSON.parse(returnStr);

                    if (rtnObj.ReturnData != "")
                        rtnObj.ReturnData = JSON.parse(rtnObj.ReturnData);

                    successCallBack(rtnObj);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $.hideLoading();

                    radalert("程序错误，请稍后再试…", 300, 120, "错误");
                }

            });
        },
        getQueryString: function (name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
    });

})(jQuery);