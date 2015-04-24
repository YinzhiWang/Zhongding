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
        },
        convertToCapitalChinese: function (arg) {

            var strNum = String(arg);

            for (i = strNum.length - 1; i >= 0; i--) {
                strNum = strNum.replace(",", "");
                strNum = strNum.replace(" ", "");
            }
            strNum = strNum.replace("￥", "");
            strNum = strNum.replace("¥", "");
            if (isNaN(strNum)) {
                alert("请检查小写金额是否正确");
                return;
            }
            part = String(strNum).split(".");

            newchar = "";
            for (i = part[0].length - 1; i >= 0; i--) {
                if (part[0].length > 10) { alert("位数过大，无法计算"); return ""; }
                tmpnewchar = ""
                perchar = part[0].charAt(i);
                switch (perchar) {
                    case "0": tmpnewchar = "零" + tmpnewchar; break;
                    case "1": tmpnewchar = "壹" + tmpnewchar; break;
                    case "2": tmpnewchar = "贰" + tmpnewchar; break;
                    case "3": tmpnewchar = "叁" + tmpnewchar; break;
                    case "4": tmpnewchar = "肆" + tmpnewchar; break;
                    case "5": tmpnewchar = "伍" + tmpnewchar; break;
                    case "6": tmpnewchar = "陆" + tmpnewchar; break;
                    case "7": tmpnewchar = "柒" + tmpnewchar; break;
                    case "8": tmpnewchar = "捌" + tmpnewchar; break;
                    case "9": tmpnewchar = "玖" + tmpnewchar; break;
                }
                switch (part[0].length - i - 1) {
                    case 0: tmpnewchar = tmpnewchar + "元"; break;
                    case 1: if (perchar != 0) tmpnewchar = tmpnewchar + "拾"; break;
                    case 2: if (perchar != 0) tmpnewchar = tmpnewchar + "佰"; break;
                    case 3: if (perchar != 0) tmpnewchar = tmpnewchar + "仟"; break;
                    case 4: tmpnewchar = tmpnewchar + "万"; break;
                    case 5: if (perchar != 0) tmpnewchar = tmpnewchar + "拾"; break;
                    case 6: if (perchar != 0) tmpnewchar = tmpnewchar + "佰"; break;
                    case 7: if (perchar != 0) tmpnewchar = tmpnewchar + "仟"; break;
                    case 8: tmpnewchar = tmpnewchar + "亿"; break;
                    case 9: tmpnewchar = tmpnewchar + "拾"; break;
                }
                newchar = tmpnewchar + newchar;
            }
            if (strNum.indexOf(".") != -1) {
                if (part[1].length > 2) {
                    part[1] = part[1].substr(0, 2)
                }
                for (i = 0; i < part[1].length; i++) {
                    tmpnewchar = ""
                    perchar = part[1].charAt(i);
                    switch (perchar) {
                        case "0": tmpnewchar = "零" + tmpnewchar; break;
                        case "1": tmpnewchar = "壹" + tmpnewchar; break;
                        case "2": tmpnewchar = "贰" + tmpnewchar; break;
                        case "3": tmpnewchar = "叁" + tmpnewchar; break;
                        case "4": tmpnewchar = "肆" + tmpnewchar; break;
                        case "5": tmpnewchar = "伍" + tmpnewchar; break;
                        case "6": tmpnewchar = "陆" + tmpnewchar; break;
                        case "7": tmpnewchar = "柒" + tmpnewchar; break;
                        case "8": tmpnewchar = "捌" + tmpnewchar; break;
                        case "9": tmpnewchar = "玖" + tmpnewchar; break;
                    }
                    if (i == 0) tmpnewchar = tmpnewchar + "角";
                    if (i == 1) tmpnewchar = tmpnewchar + "分";
                    newchar = newchar + tmpnewchar;
                }
            }
            while (newchar.search("零零") != -1)
                newchar = newchar.replace("零零", "零");
            newchar = newchar.replace("零亿", "亿");
            newchar = newchar.replace("亿万", "亿");
            newchar = newchar.replace("零万", "万");
            newchar = newchar.replace("零元", "元");
            newchar = newchar.replace("零角", "");
            newchar = newchar.replace("零分", "");
            if (newchar.charAt(newchar.length - 1) == "元" || newchar.charAt(newchar.length - 1) == "角")
                newchar = newchar + "整"
            return newchar;
        }
    });

})(jQuery);