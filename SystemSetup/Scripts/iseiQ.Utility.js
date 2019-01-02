/*!
 * File: iseiQ.Utility.js
 * Company: i-Enter Asia
 * Copyright © 2015 i-Enter Asia. All rights reserved.
 * Project: iseiQ
 * Author: GiangVT
 * Created date: 2015/01/13
 */

var iseiQ = iseiQ || {};

iseiQ.utility = (function () {
    //display multiline attribute
    function displayMultiline() {
        $('.display-multiline').each(function () {
            $(this).html($(this).text().replace(/\n/g, '<br />'));
        });
    }

    // delete "-" characters at the begining
    function deleteHyphenCharacters(groupName) {
        var groupNameLength = groupName.length;
        while (groupName.charAt(0) == '-') {
            groupName = groupName.substring(1, groupNameLength);
        }
        return groupName;
    }

    function displayGroupName() {
        setTimeout(function () {
            $('.ddl-group-id :selected').each(function () {
                var currentGroupName = $(this).text();
                var resultGroupName = deleteHyphenCharacters(currentGroupName);

                // Contract View screen
                if ($('#contractView').length != 0) {
                    $(this).parents('.content').find('.display-ddl-group-id').text(resultGroupName).addClass('txt-width-250');
                }

                // Estimate View screen
                if ($('#estimateView').length != 0) {
                    $('.ddl-group-id').parent().find('.display-ddl-group-id').text(resultGroupName);
                }
            });
        }, 500);
    }

    function formatJSONDate(jsonDate) {
        return $.datepicker.formatDate('yy年mm月dd日', new Date(parseInt(jsonDate.substr(6))));
    }

    function formatYearMonth(targetYM) {
        return targetYM.substr(0, 4) + '年' + targetYM.substr(4, 2) + '月';
    }

    // Format Time: decimal number to {hour:minute}
    function formatMinute(x) {
        if (x == "" || x == 0) {
            return "00";
        } else if (x < 10) {
            return '0' + x;
        } else {
            return '' + x;
        }
    }

    function formatHour(x) {
        if (x == "" || x == 0) {
            return "00";
        }
        else if (x >= 10 || x <= -10) {
            return x;
        } else if (x < 10 && x > 0) {
            return '0' + x;
        } else if (x < 0 && x > -10) {
            return '-0' + x * (-1);
        }
    }

    function formatDecimalToTime(inputTime)
    {
        inputTime = Math.round(inputTime * 100) / 100;
        var minActual = Math.round(Math.floor(inputTime) == 0 && inputTime < 0
                            ? ((Math.floor(inputTime) + 1) - inputTime) * 60
                            : (inputTime - Math.floor(inputTime)) * 60);
        minActual = minActual != 0 && inputTime < 0 ? 60 - minActual : minActual;

        var hourActual = '00';
        if (minActual != 0 && inputTime < 0) {
            hourActual = Math.floor(inputTime) + 1;
        } else {
            hourActual = Math.floor(inputTime);
        }

        var resultHour = (hourActual == 0 && inputTime < 0 ? '-' + formatHour(hourActual) : formatHour(hourActual));
        var resultMin = formatMinute(minActual);

        return resultHour + '<span class="space_total">:</span>' + resultMin;
    }

   

    function focusTextbox() {
        $(document).off('input:text, textarea, input:password');
        $(document).on('focus', 'input:text, textarea, input:password', function () {
            $(this).focus(function () { $(this).select(); })
            $(this).mouseup(function (e) {
                e.preventDefault();
                $(this).unbind(e.type);
            });
        });
    }

    function setFocusAmount(control)
    {
        if ($(control).is(":focus")) {
            $(control).focus();
            $(control).select();
        }
    }

    // Format money string to integer
    function convertMoneyToInt(input, canNegative) {
        if (input.length == 0)
            return 0;

        var strValue = input.replace(/,/g, '');
        var intValue = validPositiveNumeric(strValue) ? parseInt(strValue) : 0;

        if (typeof (canNegative) !== "undefined" && canNegative == true)
            intValue = validNegativeNumeric(strValue) ? parseInt(strValue) : 0;

        return intValue;
    }

    // Format integer to money string with symbol ','
    function convertIntToMoney(input) {
        var result = '0';

        if ($.isNumeric(input)) {
            result = parseInt(input).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
        }

        return result;
    }

    // Replace all symbol ',' in money
    function replaceAllMoney() {
        $('.money, .negative-money, .negative-money-max').each(function () {
            var data = $(this).val();
            if (data.length > 0) {
                $(this).val(data.replace(/,/g, ''));
            }
        });
    }

    // Format money in textbox to string with symbol ','
    function formatMoney(obj) {
        obj = obj != null ? obj : '.money, .negative-money, .negative-money-max';

        $(obj).each(function () {
            var data = convertMoneyToInt($(this).val(), true);
            var money = convertIntToMoney(data);

            $(this).val(money);
        });
    }

    // Format money Type of Decimal and allow null in textbox to string with symbol ','
    function formatMoneyTypeDecimal(obj) {
        obj = obj != null ? obj : '.init_display_money, .init_display_money_decimal';

        $(obj).each(function () {
            if ($(this).val() != '') {
                var arr = $(this).val().split('.');
                var integerPart = arr[0];
                var decimalPart = arr[1];

                var integerMoney = convertIntToMoney(integerPart);
                var value = integerMoney + ($(this).val().indexOf('.') == -1 ? '' : '.' + decimalPart);

                $(this).val(value);
            }
        });
    }

    // Format money in label to string with symbol ','
    function formatMoneyLabel() {
        $('label.lbl-money, label.money').each(function () {
            var data = convertMoneyToInt($(this).text(), true);
            var money = convertIntToMoney(data);

            $(this).text(money);
        });
    }

    // Check isvalid date
    // Return true if valid, false if invalid
    function isValidDate(date) {
        var bits = date.split('/');
        var checkingDate = new Date(bits[0], bits[1] - 1, bits[2]);
        return checkingDate && (checkingDate.getMonth() + 1) == bits[1] && checkingDate.getDate() == Number(bits[2]);
    }

    // Check validation of date field
    // date is input data
    // format is datetime type (yyyy/mm/dd or yyyy/mm)
    // control is field name
    // return invalid message if invalid, null if valid
    function validDate(date, format, control) {
        var constantErr = Constant.ERR_FORMAT_DATE;
        if (date.length > 0) {
            if (format == 'yyyy/mm') {
                date += '/01';
                constantErr = Constant.ERR_FORMAT_YM;
            }

            if (!/^[0-9]{4}\/[0-9]{1,2}\/[0-9]{1,2}/.test(date) || !isValidDate(date)) {
                return control + constantErr;
            }

            var year = parseInt(date.split('/')[0]);

            if (Constant.MIN_YEAR > year || year > Constant.MAX_YEAR) {
                if (format == 'yyyy/mm') {
                    return control + Constant.ERR_INCORRECT_DATE;
                }
                return control + Constant.ERR_INCORRECT_DATE;
            }
        }

        return null;
    }

    // Compare startDate with endDate
    // Return true if valid, false if invalid
    function compareDate(startDate, endDate, format) {
        var start = $.datepicker.formatDate('yy/mm/dd', new Date(startDate));
        var end = $.datepicker.formatDate('yy/mm/dd', new Date(endDate));

        if (format == 'yyyy/mm') {
            start = $.datepicker.formatDate('yy/mm/dd', new Date(startDate + '/01'));
            end = $.datepicker.formatDate('yy/mm/dd', new Date(endDate + '/01'));
        }

        var valid = true;
        if (start > end) {
            valid = false;
        }

        return valid;
    }

    // Compare startDate with endDate
    // Return true if valid, false if invalid
    function compareDateRange(startDate, endDate, rangeMonth) {
        var start = new Date(startDate + '/01');
        var end = new Date(endDate + '/01');

        var valid = true;

        if ((end.getMonth() + end.getFullYear() * 12 - start.getMonth() - start.getFullYear() * 12) > rangeMonth) {
            valid = false;
        }

        return valid;
    }

    // Check range of a duration time
    function validateRangeYear(startYear, endYear, rangeYear) {
        var arrStart = startYear.split('/');
        var arrEnd = endYear.split('/');
        var sYear = parseInt(arrStart[0]);
        var sMonth = parseInt(arrStart[1]);
        var eYear = parseInt(arrEnd[0]);
        var eMonth = parseInt(arrEnd[1]);

        if ((eYear - sYear) * 12 + (eMonth - sMonth) > rangeYear) {
            return false;
        }
        return true;
    }

    function onChangeStartAndEndDateContract() {
        var startDate = $('#CONTRACT_PRJ_PERIOD_START').val();
        var endDate = $('#CONTRACT_PRJ_PERIOD_END').val();
        var errInvalid;
        if (startDate.length === 0) {
            return;
        }
        if (endDate.length === 0) {
            return;
        }
        errInvalid = iseiQ.utility.validDate(startDate, Constant.DATE_FORMAT, Constant.WORK_PERIOD_START);
        if (errInvalid != null) {
            iseiQ.utility.showMessageModal(errInvalid, true);
            return;
        }

        if (startDate.length > Constant.MAX_DATE) { // valid string length
            errInvalid = Constant.WORK_PERIOD_START + Constant.ERR_INCORRECT_DATE;
            iseiQ.utility.showMessageModal(errInvalid, true);
            return;
        }

        if (endDate.length > Constant.MAX_DATE) { // valid string length
            errInvalid = Constant.WORK_PERIOD_END + Constant.ERR_INCORRECT_DATE;
            iseiQ.utility.showMessageModal(errInvalid, true);
            return;
        }

        var errInvalidEndDate = iseiQ.utility.validDate(endDate, Constant.DATE_FORMAT, Constant.WORK_PERIOD_END);
        if (errInvalidEndDate != null) {
            iseiQ.utility.showMessageModal(errInvalidEndDate, true);
            return;
        }

        var errInvalidStartDate = iseiQ.utility.validDate(endDate, Constant.DATE_FORMAT, Constant.WORK_PERIOD_START);
        if (errInvalidStartDate != null) {
            iseiQ.utility.showMessageModal(errInvalidStartDate, true);
            return;
        }

        if (errInvalidEndDate == null) {
            if (endDate.length > 0 && !iseiQ.utility.compareDate(startDate, endDate, Constant.DATE_FORMAT)) {
                iseiQ.utility.showMessageModal(Constant.ERR_COMPARE_TOW_DATE.replace('{0}', Constant.WORK_PERIOD_END).replace('{1}', Constant.WORK_PERIOD_START), true);
                return;
            }
        }
        if (errInvalidStartDate == null) {
            if (endDate.length > 0 && !iseiQ.utility.compareDate(startDate, endDate, Constant.DATE_FORMAT)) {
                iseiQ.utility.showMessageModal(Constant.ERR_COMPARE_TOW_DATE.replace('{0}', Constant.WORK_PERIOD_END).replace('{1}', Constant.WORK_PERIOD_START), true);
                return;
            }
        }
    }

    function validateDeliveryDate() {
        var deliveryDate = $('#CONTRACT_DELIVERY_DATE').val();
        var errInvalid;

        errInvalid = iseiQ.utility.validDate(deliveryDate, Constant.DATE_FORMAT, Constant.DELIVERY_DATE);

        if (errInvalid != null) {
            iseiQ.utility.showMessageModal(errInvalid, true);
            return;
        }

        if (deliveryDate.length > Constant.MAX_DATE) { // valid string length
            errInvalid = Constant.DELIVERY_DATE + Constant.ERR_INCORRECT_DATE;
            iseiQ.utility.showMessageModal(errInvalid, true);
            return;
        }

        var startDate = $('#CONTRACT_PRJ_PERIOD_START').val();
        var errInvalidStartDate = iseiQ.utility.validDate(startDate, Constant.DATE_FORMAT, Constant.WORK_PERIOD_START);

        if (errInvalidStartDate != null) {
            iseiQ.utility.showMessageModal(errInvalidStartDate, true);
            return;
        }

        if (startDate.length > Constant.MAX_DATE) { // valid string length
            errInvalid = Constant.WORK_PERIOD_START + Constant.ERR_INCORRECT_DATE;
            iseiQ.utility.showMessageModal(errInvalid, true);
            return;
        }

        var endDate = $('#CONTRACT_PRJ_PERIOD_END').val();

        var errInvalidEndDate = iseiQ.utility.validDate(endDate, Constant.DATE_FORMAT, Constant.WORK_PERIOD_END);

        if (errInvalidEndDate != null) {
            iseiQ.utility.showMessageModal(errInvalidEndDate, true);
            return;
        }

        if (endDate.length > Constant.MAX_DATE) { // valid string length
            errInvalid = Constant.WORK_PERIOD_END + Constant.ERR_INCORRECT_DATE;
            iseiQ.utility.showMessageModal(errInvalid, true);
            return;
        }

        if (startDate.length > 0 && endDate.length > 0 && deliveryDate.length == 0) {
            onChangeStartAndEndDateContract();
            return;
        }
        else if (deliveryDate.length != 0) {
            if (startDate.length > 0 && !iseiQ.utility.compareDate(startDate, deliveryDate, Constant.DATE_FORMAT)) {
                iseiQ.utility.showMessageModal(Constant.ERR_COMPARE_TOW_DATE.replace('{0}', Constant.DELIVERY_DATE).replace('{1}', Constant.WORK_PERIOD_START), true);
                return;
            }
            if (endDate.length > 0 && !iseiQ.utility.compareDate(deliveryDate, endDate, Constant.DATE_FORMAT)) {
                iseiQ.utility.showMessageModal(Constant.ERR_COMPARE_OVE_DATE.replace('{0}', Constant.DELIVERY_DATE).replace('{1}', Constant.WORK_PERIOD_END), true);
                return;
            }
            if (startDate.length > 0 && endDate.length > 0 && deliveryDate.length > 0) {
                onChangeStartAndEndDateContract();
                return;
            }
        }
    }

    // Check alphanumeric only
    function validAlphanumeric(input) {
        return /^[a-zA-Z0-9]*$/.test(input);
    }

    // Check positive number only
    function validPositiveNumeric(value) {
        var data = value.trim().length == 0 ? '0' : value;

        if (data.indexOf('-') !== -1 || data.indexOf('.') !== -1 || !$.isNumeric(data)) {
            return false;
        }

        return true;
    }

    // Check negative number only
    function validNegativeNumeric(input) {
        if (input.indexOf('.') !== -1 || !$.isNumeric(input)) {
            return false;
        }
        return true;
    }

    // Check decimal number only
    function validDecimalNumeric(input) {
        if (input.indexOf('-') !== -1 || input.indexOf(',') !== -1 || !$.isNumeric(input)) {
            return false;
        }
        return true;
    }

    function validDecimalByType(value, type) {
        var data = value.trim().length == 0 ? '0' : value;

        if (validDecimalNumeric(data)) {
            var arr = data.toString().split('.');
            var positivePartMaxLength = type == Constant.DECIMAL_4_2 ? 2 : (type == Constant.DECIMAL_5_2 ? 3: 5);

            if (arr[0].length == 0
                || arr[0].length > positivePartMaxLength
                || (typeof (arr[1]) != 'undefined' && (arr[1].length == 0 || arr[1].length > 2))) {
                return false;
            }
        } else {
            return false;
        }

        return true;
    }

    // Show client error message on the top of page
    function showClientError(errMessage, position) {
        position = typeof (position) === 'undefined' ? '#title' : position;

        $('.validation-summary-errors').remove();
        var html = '<div class="validation-summary-errors"><ul>';

        for (var i = 0; i < errMessage.length; i++) {
            html += '<li class="first last">' + errMessage[i] + '</li>';
        }
        html += '</ul></div>';
        $(position).after(html);
        $(document).scrollTop(0);

        return;
    }

    // Check user acount and password
    function validAccount(input) {
        var re = Constant.REGEX_PASSWORD;
        return !re.test(input);
    }

    // Check numeric only
    function validNumeric(input) {
        return /^[0-9]+$/.test(input);
    }

    // Check phone number only
    function validPhone(input) {
        return /^[0-9/-]+$/.test(input);
    }

    // Check URL
    function validURL(input) {
        var re = /^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$/;
        return re.test(input);
    }
    
    // Check email
    function validEmail(input) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(input);
    }

    //Check fullsize and half size
    function validFullHalfSize(control) {
        control.on("change keyup", function (e) {
            var text = control.val();
            var regX = /[^a-zA-Z0-9\!\""\#\$\%\&\'\(\)\=\~\|\-\^\@@\[\;\:\]\,\.\/\`\{\+\*\}\>\?\<\'\?\/\\\_\ ]/g;
            if (regX.test(text)) {
                text = text.replace(regX, '');
                control.val(text);
            }
        });
    }

    function checkValidFullHalfSize(text) {
        var regX = /[^a-zA-Z0-9\!\""\#\$\%\&\'\(\)\=\~\|\-\^\@@\[\;\:\]\,\.\/\`\{\+\*\}\>\?\<\'\?\/\\\_\ ]/g;
        if (regX.test(text)) {
            return false;
        }

        return true;
    }

    //Check for Login Screen Forms
    function validLoginScreen(control) {
        control.on("change keyup", function (e) {
            var text = control.val();
            var regX = /[^a-zA-Z0-9\!\""\#\$\%\&\'\(\)\=\~\|\-\_\^\@@\[\;\:\]\,\.\/\`\{\+\*\}\>\?]/g;
            if (regX.test(text)) {
                text = text.replace(regX, '');
                control.val(text);
            }
        });
    }

    //Check fullsize and half size
    function checkInputNumber(control) {
        control.on("change keyup", function (e) {
            var text = control.val();
            var regX = /[^0-9]/g;
            if (regX.test(text)) {
                control.val(text.replace(regX, ''));
            }
        });
    }

    function checkInputPhone(control) {
        control.on("change keyup", function (e) {
            var text = control.val();
            var regX = /[^0-9/-]/g;
            if (regX.test(text)) {
                control.val(text.replace(regX, ''));
            }
        });
    }

    function checkCompareBasetime(basetimelower, basetimeupper, paymentMethodType) {
        // Check payment method Type 
        if (paymentMethodType != '1') // not 時間幅精算
        {
            return null;
        }
        var _basetimelower = basetimelower.length > 0 ? parseFloat(basetimelower.trim()) : null;
        var _basetimeupper = basetimeupper.length > 0 ? parseFloat(basetimeupper.trim()) : null;

        if (_basetimelower < _basetimeupper) {
            return null;
        }
        return Constant.ERR_COMPARE_BASETIME;
    }

    // Check TargetYM is not out of range of project duration
    function isValidTargetYM(startDate, endDate, targetYm, closingDay) {
        var targetYmDate;
        if (closingDay == ClosingDay.Day31) {
            targetYm += '/01';
            targetYmDate = new Date(targetYm);
            targetYmDate.setMonth(targetYmDate.getMonth() + 1);
            targetYmDate.setDate(targetYmDate.getDate() - 1);
        } else {
            targetYm += '/' + (closingDay.length == 1 ? "0" + closingDay : closingDay);
            targetYmDate = new Date(targetYm);
        }

        if (targetYmDate < new Date(startDate)
            || (new Date(targetYm).getYear() > new Date(endDate).getYear())
            || (new Date(targetYm).getYear() == new Date(endDate).getYear()
                && new Date(targetYm).getMonth() - new Date(endDate).getMonth() > 1)
            || (new Date(targetYm).getYear() == new Date(endDate).getYear()
                && new Date(targetYm).getMonth() - new Date(endDate).getMonth() == 1 && closingDay >= new Date(endDate).getDate())) {
            return false;
        }
        return true;
    }

    // Encode html to string
    function htmlEncode(value) {
        return $('<div/>').text(value).html();
    }

    // Decode string to html
    function htmlDecode(value) {
        return $('<div/>').html(value).text();
    }

    // Count byte of string ( japanese character is 3 byte)
    function byteCount(s) {
        return encodeURI(s).split(/%..|./).length - 1;
    }   
    // Rounding decimal data
    function roundingDecimal(number, roundType, isPercent) {
        if (isPercent) {
            switch (roundType) {
                case "01": // Round Down
                    number = number >= 0 ? Math.floor(number * 10000) / 100 : (Math.floor(number * 10000) + 1) / 100;
                    break;
                case "02": // Round Up
                    number = number <= 0 ? Math.floor(number * 10000) / 100 : (Math.floor(number * 10000) + 1) / 100;
                    break;
                case "03": // Round auto
                    var multiplier = 1;
                    if (number < 0)
                        multiplier = -1;
                    number = Math.round((number * 10000 * multiplier).toFixed(2)) / (100 * multiplier);
                    break;
            }
        } else {
            switch (roundType) {
                case "01": // Round Down
                    number = number >= 0 ? Math.floor(number) : Math.floor(number) + 1;
                    break;
                case "02": // Round Up
                    number = number <= 0 ? Math.floor(number) : Math.floor(number) + 1;
                    break;
                case "03": // Round auto
                    number = Math.round(number);
                    break;
            }
        }
        return number;
    };

    function roundDecimalPercent(value) {
        var multiplier = 1;

        if (value < 0)
            multiplier = -1;

        value = Math.round((value * 10000 * multiplier).toFixed(2)) / (100 * multiplier);

        return value;
    }

    function roundByType(roundingType, value) {
        if (value.toString().indexOf('.') == -1)
        {
            return value;
        }
        switch (roundingType) {
            case Constant.ROUND_TYPE.DOWN:
                value = value >= 0 ? Math.floor(value) : Math.floor(value) + 1;
                break;
            case Constant.ROUND_TYPE.UP:
                value = value <= 0 ? Math.floor(value) : Math.floor(value) + 1;
                break;
            case Constant.ROUND_TYPE.AUTO:
                value = value < 0 ? Math.round(value * (-1)) * (-1) : Math.round(value);
                break;
            default:
                value = Math.round(value);
                break;
        }

        return value;
    }

    // Rounding time (hours) by unit (minute)
    function roundTimeByUnit(value, unit) {

        // minute overbalance
        var minute = (value % 1 * 60).toFixed(0);

        if (minute !== 0 && unit !== 0 && minute !== unit && minute % unit !== 0) {

            // return value by cut all decimal of working time
            if (minute < unit) {
                return parseInt(value);
            }

            // convert minute to hour
            var newMinute = parseInt(minute / unit) * unit; // minute rounded (round down)
            var hourOverbalanceArr = (newMinute / 60).toString().split('.');
            var hourOverbalance = hourOverbalanceArr.length > 1 ? hourOverbalanceArr[1].substring(0, 2) :'00';

            value = parseInt(value).toString() + '.' + hourOverbalance;
        }

        return parseFloat(value);
    }

    // Create html for dialog
    function htmlDialog(type, message) {
        var dialogId = type == 1 ? 'dialog-message' : 'dialog-confirm';
        var iconClass = type == 1 ? 'ui-icon-circle-check' : 'ui-icon-alert';

        var html = '<div id="' + dialogId + '">'
            + '<p>'
            + '<span class="ui-icon ' + iconClass + '"></span>'
            + message;

        if (type != 1)
            html += '<br><img src="/Images/ajax-loader.gif" />';

        html += '</p></div>';

        return html;
    }

    // Show message dialog
    function showMessageDialog(message, urlRedirect, alert, formId) {
        var type = (typeof (alert) !== 'undefined' && alert !== null) ? 2 : 1;
        var dialogId = (typeof (alert) !== 'undefined' && alert !== null) ? '#dialog-confirm' : '#dialog-message';
        if (typeof (body) !== 'undefined')
        {
            $(body).before(htmlDialog(type, message));
        }
        else
        {
            $(login).before(htmlDialog(type, message));
        }        

        $(dialogId).dialog({
            modal: true,
            closeOnEscape: false,
            buttons: {
                OK: function () {
                    $(this).dialog("close");

                    if (typeof(urlRedirect) !== 'undefined' && urlRedirect !== null)
                        window.location.href = urlRedirect;

                    if (typeof (formId) !== 'undefined' && formId !== null)
                        $(formId).submit();
                }
            }
        });
        $(".ui-dialog-titlebar").hide();
    }    

    // Show confirm dialog when submit form
    function showSubmitConfirmDialog(message, formId, urlRedirect) {
        $(body).before(htmlDialog(2, message));

        $("#dialog-confirm").dialog({
            modal: true,
            closeOnEscape: false,
            buttons: {
                OK: function () {
                    if (typeof (formId) !== 'undefined' && formId !== null) {
                        $('#dialog-confirm img').css('display', 'block');
                        $('div.ui-dialog button').attr('disabled', 'disabled').children('span').addClass('disabled');

                        iseiQ.utility.replaceAllMoney();
                        $(formId).submit();
                    }

                    if (typeof (urlRedirect) !== 'undefined' && urlRedirect !== null)
                        window.location.href = urlRedirect;
                },
                'キャンセル': function () {
                    $(this).dialog("close");
                }
            }
        });
        $(".ui-dialog-titlebar").hide();
    }

    // Show message dialog
    function showMessageDialogCB(alert, message, callback) {
        var type = (typeof (alert) !== 'undefined' && alert !== null) ? 2 : 1;
        var dialogId = (typeof (alert) !== 'undefined' && alert !== null) ? '#dialog-confirm' : '#dialog-message';
        $(body).before(htmlDialog(type, message));

        $(dialogId).dialog({
            modal: true,
            closeOnEscape: false,
            buttons: {
                OK: function () {
                    $(this).dialog("close");
                    callback(null);
                }
            }
        });
        $(".ui-dialog-titlebar").hide();
    }

    // set status is session timeout
    function setStatusTimeOut() {
        $(window).off('beforeunload');
        localStorage.setItem('sessiontimeout', 'true');
        window.location.href = Constant.URL_REDIRECT_TIMEOUT;
    }

    function IsAuthenticateTimeout(message, form, surl) {
        var success = 1;
        
        $.ajax({
            type: "GET",
            url: surl,            
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            async: false,
            cache: false,
            success: function () {
                if (form != '')
                {
                    if (message != '')
                    {
                        $(form).submit();
                    }
                    else
                    {
                        showSubmitConfirmDialog(message, form);
                    }
                }                

                success = 1;
                return;
            },
            error: function (error) {
                if (error.status == HTTPCode.TIME_OUT) { //419: Authentication Timeout
                    setStatusTimeOut();
                }
                success = 0;
                return;
            }
        });
        return success;
    }

    // check session timeout
    function checkTimeout() {
        $.ajax({
            type: "GET",
            url: '/Common/Common/CheckTimeOut',
            dataType: 'json',
            cache: false,
            success: function () {
                return;
            },
            error: function (error) {
                if (error.status == HTTPCode.TIME_OUT) { //419: Authentication Timeout
                    setStatusTimeOut();
                }
                return;
            }
        });
    }

    function buildColumShortText(data, className) {
        data = data != null ? data : '';
        className = className != null ? className : '';

        var html = '<div class="short-text text-overflow ' + className+ '" title="' + data + '">' + data + '</div>';
        return html;
    }

    function buildColumShortTextArea(data, className, maxRow) {
        data = nvl(data, '');
        className = nvl(className, '');
        maxRow = typeof (maxRow) != 'undefined' ? maxRow : 0;

        var html = '';

        if (data.length == 0) {
            html = '&nbsp;';
        } else {
            var dataArea = data.trim().split('\n');
            var valueLength = 0;

            for (var i = 0; i < dataArea.length; i++) {
                valueLength += dataArea[i].length;

                if (i == dataArea.length - 1 || valueLength > 50 || (maxRow > 0 && i == maxRow - 1)) {
                    className += ' short-text text-overflow';
                }

                html += '<div class="wrap-text ' + className + '" title="' + data + '">' + dataArea[i] + '</div>';

                if (valueLength > 50) {
                    break;
                }
            }
        }

        return html;
    }

    function buildColumTextBox(data, haveButton, haveLabel, value) {
        data = data != null ? data : '';
        var html = '';
        if (haveButton == 1)
        {
            if (haveLabel == 1)
            {
                html = '<label class="lblDestination" >' + nvl(value, '') + '</label>'
                     + '<input type="hidden" name="BILLING_SETTING_LIST[0].PAYEE_BANK_ACCOUNT_SEQ_NO" class="table-column"' + ' value="' + data + '">'
                     + '<button type="button" id="btnShowDestination" name="btnShowDestination" class="btn btn-default">選択</button>';                     
            }
            else
            {
                html = '<input type="text" name="BILLING_SETTING_LIST[0].DEBIT_INFO" class="txt-no-border"' + ' value="' + data + '" title="' + data + '">'
                     + '<button type="button" id="btnShowDebitAccountInfo" name="btnShowDebitAccountInfo" class="btn btn-default">...</button>';
            }
            
        }
        else
        {
            html = '<input type="text" name="txtTableColum" class="table-column"' + ' value="' + data + '">';
        }        
        
        return html;
    }

    function buildColumRadio(depositeType, bilSeqNo) {
        depositeType = depositeType != null ? depositeType : '';
        bilSeqNo = bilSeqNo != null ? bilSeqNo : '';

        var transfer = depositeType == 0 ? 'checked' : '';
        var withdrawal = depositeType == 1 ? 'checked' : '';
        var html = '<input id="deposit_type_1" type="radio" name="raOption' + bilSeqNo + '" class="radio-button" value="0" ' + transfer + ' />'
                 + '<label>振込</label>'
                 + '<input id="deposit_type_2" type="radio" name="raOption' + bilSeqNo + '" class="radio-button" value="1" ' + withdrawal + ' />'
                 + '<label>引落</label>'
                 + '<input type="hidden" name="BILLING_SETTING_LIST[0].DEPOSIT_TYPE" class="deposit-type"' + ' value="' + depositeType + '">'
                 + '<input type="hidden" name="BILLING_SETTING_LIST[0].CONT_BILLING_SEQ_NO" class="billing-seg-no"' + ' value="' + bilSeqNo + '">';

        return html;
    }

    function buildColumDropDownList(data, className) {
        data = data != null ? data : '';
        className = className != null ? className : '';

        var html = '';

        switch (data)
        {
            case "0":
                html = '<select class="' + className + ' col_6 tab-right_2" id="seBillingDate" name="BILLING_SETTING_LIST[0].BILLING_DESC_NAME">'
             + '<option value="0" selected>契約どおりの日付を表示</option>'
             + '<option value="1">請求締日を表示</option>'
             + '<option value="2">本日の日付を表示</option>'
             + '</select>';
                break;
            case "1":
                html = '<select class="' + className + ' col_6 tab-right_2" id="seBillingDate" name="BILLING_SETTING_LIST[0].BILLING_DESC_NAME">'
             + '<option value="0">契約どおりの日付を表示</option>'
             + '<option value="1" selected>請求締日を表示</option>'
             + '<option value="2">本日の日付を表示</option>'
             + '</select>';
                break;
            case "2":
                html = '<select class="' + className + ' col_6 tab-right_2" id="seBillingDate" name="BILLING_SETTING_LIST[0].BILLING_DESC_NAME">'
             + '<option value="0">契約どおりの日付を表示</option>'
             + '<option value="1">請求締日を表示</option>'
             + '<option value="2" selected>本日の日付を表示</option>'
             + '</select>';
                break;
            default :
                break;
        }      

        return html;
    }

    function builDetailFormCode(url, id) {
        var html = '<form method="POST" action="' + url + '">'
            + '<input type="hidden" name="id" value="' + id + '"/>'
            + '<a href="#" class="detail-link" onclick="$(this).parent().submit();">詳細</a>'
            + '</form>';
        return html;
    }

    function builDetailFormCodeLink(id) {
        var s = '<form method="post" action="@Url.Action("Select", "Customer")">';
        s += '<input type="hidden" name="id" value="' + id + '"/>';
        s += '<a href="#"><button type="submit" class="formSubmitLink">編集</button></a>';
        s += '</form>';
            
        return s;
    }

    function imeControl(control, type) {
        if (type === 'active') {
            control.css('ime-mode', 'active');
        } else if (type === 'inactive') {
            control.css('ime-mode', 'inactive');
        }else {
            control.css('ime-mode', 'disabled');
        }
    }

    // get data from server by Ajax GET. Return result
    function getDataByAjax(url, param) {
        var result;

        $.ajax({
            type: 'GET',
            url: url,
            data: param,
            dataType: 'json',
            async: false,
            cache: false,
            success: function (data) {
                result = data;
            },
            error: function (err) {
                if (err.status == HTTPCode.TIME_OUT) { //419: Authentication Timeout
                    setStatusTimeOut();
                }
                return;
            }
        });
        return result;
    }

    // get data from server by Ajax GET. Callback result
    function getDataByAjaxCB(url, param, callback) {
        $.ajax({
            type: 'GET',
            url: url,
            data: param,
            dataType: 'json',
            async: false,
            cache: false,
            success: function (data) {
                callback(data);
            },
            error: function (err) {
                if (err.status == HTTPCode.TIME_OUT) { //419: Authentication Timeout
                    setStatusTimeOut();
                }
                return;
            }
        });
    }

    // get data from server by Ajax GET with param is serialize to check valid data. Return result
    function checkDataExistByAjax(url, param) {
        var result;

        $.ajax({
            type: 'GET',
            url: url,
            data: param,
            dataType: 'json',
            traditional: true,
            async: false,
            cache: false,
            success: function (data) {
                result = data;
            },
            error: function (err) {
                if (err.status == HTTPCode.TIME_OUT) { //419: Authentication Timeout
                    setStatusTimeOut();
                }
                return;
            }
        });
        return result;
    }

    // get image data by ajax
    function getImageByAjax(url, param, callback) {
        $.ajax({
            type: 'GET',
            url: url,
            data: param,
            contentType: 'application/json',
            dataType: 'json',
            cache: false,
            success: function (data) {
                callback(data);
            },
            error: function () {
                if (error.status == HTTPCode.TIME_OUT) { //419: Authentication Timeout
                    setStatusTimeOut();
                }
                callback(null);
            }
        });
    }

    // Get month colum array on data table list bt start date and end date of project
    function getMonthCols(startDate, endDate) {
        var sY = parseInt(startDate.split('/')[0]);
        var sM = parseInt(startDate.split('/')[1]);
        var eY = parseInt(endDate.split('/')[0]);
        var eM = parseInt(endDate.split('/')[1]);
        var columArr = [];

        for (var Y = eY, M = eM, i = 0; Y > sY || (Y == sY && M >= sM) ; i++, M--) {
            var YM;
            if (M == 0) {
                M = 12;
                Y--;
            }

            if (M < 10)
                YM = Y.toString() + '/0' + M.toString();
            else
                YM = Y.toString() + '/' + M.toString();
            columArr.unshift(YM);
        }
        return columArr;
    }

    // Set title colum with month for data table list
    function bindMonthCols(startDate, endDate) {
        var colums = getMonthCols(startDate, endDate);

        $('table.tb-detail tr th.th-month').remove();
        for (var i = 0; i < colums.length; i++) {
            $('table.tb-detail tr.month-colum').append('<th class="th-month">' + colums[i] + '</th>');
        }
    }

    function bindTotalCols(colums, startDate, endDate) {
        var htmlManday = '';
        var htmlMoney = '';

        if (colums == null) {
            colums = getMonthCols(startDate, endDate);
        }

        for (var i = 0; i < colums.length; i++) {
            htmlManday += '<td class="right">'
                + '<label class="font-normal" name="' + colums[i] + '">0.0</label>'
                + '<label>人日</label>'
                + '</td>';
            htmlMoney += '<td class="right">'
                + '<label class="font-normal money" name="' + colums[i] + '">0</label>'
                + '<label>円</label>'
                + '</td>';
        }

        $('table.tb-ma-center tr.tr-total, table.tb-ma-sales-center tr.tr-total, table.tb-sc-center tr.tr-total').empty();
        $('table.tb-ma-center tr.tr-total').append(htmlManday);
        $('table.tb-ma-sales-center tr.tr-total, table.tb-sc-center tr.tr-total').append(htmlMoney);
    }

    // Bind data for tag link list by customer
    function bindTagsByCustomer(customerId, control) {
        var param = { customerId: customerId };
        var data = getDataByAjax('/Common/GetTagListJson', param);

        var $ddlTagLink = $(control);
        $ddlTagLink.empty();

        if (data.length > 0) {
            var html = '<option value="">指定なし</option>';

            for (var i = 0; i < data.length; i++) {
                html += '<option value="' + data[i].tag_id + '">' + data[i].tag_name + '</option>';
            }

            $ddlTagLink.append(html);
        }
    }

    // Build edit form 
    function buildEditForm(url, idName, idValue, isNewOrEdit, actionLinkName, isExtend, extendUrl, extendLinkName) {
        var editForm = '';
        var IDCopy = '';
        if (isExtend == 1)
        {
            if (idValue != 0) {
                IDCopy = 'registSource';
                editForm = '<span style="float:left;">'
                editForm += '<form method="post" action="' + url + '">';
                editForm += '<input type="hidden" name="' + idName + '" value="' + idValue + '"/>';
                editForm += '<input type="hidden" name="' + IDCopy + '" value="' + 2 + '"/>';
                editForm += '<a href="#"><button type="submit" class="formSubmitLink">' + actionLinkName + '</button></a>';
                editForm += '</form>';
                //s += '／';
                editForm += '</span>';

                editForm += '<span >';
                editForm += '／';
                editForm += '</span>';

                editForm += '<span style="float:right;">';
                editForm += '<form method="post" action="' + extendUrl + '">';
                editForm += '<input type="hidden" name="' + idName + '" value="' + idValue + '"/>';
                editForm += '<input type="hidden" name="' + IDCopy + '" value="' + 3 + '"/>';
                editForm += '<a href="#"><button type="submit" class="formSubmitLink">' + extendLinkName + '</button></a>';
                editForm += '</form>';
                editForm += '</span>';
            }                       
        }
        else
        {
            if (idValue != 0) {
                if (isNewOrEdit == 0) {
                    IDCopy = 'isLinkToCopy';
                    editForm = '<form method="post" action="' + url + '">';
                    editForm += '<input type="hidden" name="' + idName + '" value="' + idValue + '"/>';
                    editForm += '<input type="hidden" name="' + IDCopy + '" value="' + 1 + '"/>';
                    editForm += '<a href="#"><button type="submit" class="formSubmitLink">' + actionLinkName + '</button></a>';
                    editForm += '</form>';
                }
                else if (isNewOrEdit == 1) {
                    editForm = '<form method="post" action="' + url + '">';
                    editForm += '<input type="hidden" name="' + idName + '" value="' + idValue + '"/>';
                    editForm += '<a href="#"><button type="submit" class="formSubmitLink">' + actionLinkName + '</button></a>';
                    editForm += '</form>';
                }
                else if (isNewOrEdit == 2)
                {
                    editForm = '<form method="post" action="' + url + '">';
                    editForm += '<input type="hidden" name="' + idName + '" value="' + idValue + '"/>';
                    editForm += '<input type="hidden" name="registSource" value="' + 4 + '"/>';
                    editForm += '<a href="#"><button type="submit" class="formSubmitLink">' + actionLinkName + '</button></a>';
                    editForm += '</form>';
                }
                else
                {
                    editForm = '<form method="post" action="' + url + '">';
                    editForm += '<input type="hidden" name="' + idName + '" value="' + idValue + '"/>';
                    editForm += '<input type="hidden" name="registSource" value="' + 1 + '"/>';
                    editForm += '<a href="#"><button type="submit" class="formSubmitLink">' + actionLinkName + '</button></a>';
                    editForm += '</form>';
                }
            }            
        }
                
        return editForm;
    }

    function buildConfirmForm(havelink, url, idName, idValue, actionName,idLink) {
        var confirmForm = '';
        if (havelink != 0)
        {
            confirmForm = '<form method="post" id= "frmEstimateSend" action="' + url + '">';
            confirmForm += '<input type="hidden" name="' + idName + '" value="' + idValue + '"/>';
            confirmForm += '<a href="#"><button type="button" id="' + idLink + '"  class="formSubmitLink">' + actionName + '</button></a>';
            confirmForm += '</form>';
        }       

        return confirmForm;
    }

    function GetDateTimeNow() {
        // create Date object for current location
        var d = new Date();

        // convert to msec
        // add local time zone offset 
        // get UTC time in msec
        var utc = d.getTime() + (d.getTimezoneOffset() * 60000);

        // create new Date object Japanese
        var currentdate = new Date(utc + (3600000 * 9));
        var month = currentdate.getMonth() + 1;
        var day = currentdate.getDate();
        var hour = currentdate.getHours();
        var minutes = currentdate.getMinutes();

        var nowDate = currentdate.getFullYear() + '/' +
        (month < 10 ? '0' : '') + month + '/' +
        (day < 10 ? '0' : '') + day + ' ' + hour + ":" + (minutes < 10 ? '0' : '') + minutes;
            //+ '現在';

        return nowDate;
    }

    function GetDateNow() {
        // create Date object for current location
        var d = new Date();

        // convert to msec
        // add local time zone offset 
        // get UTC time in msec
        var utc = d.getTime() + (d.getTimezoneOffset() * 60000);

        // create new Date object Japanese
        var currentdate = new Date(utc + (3600000 * 9));
        var month = currentdate.getMonth() + 1;
        var day = currentdate.getDate();
        var nowDate = currentdate.getFullYear() + '/' +
            (month < 10 ? '0' : '') + month + '/' +
            (day < 10 ? '0' : '') + day;

        return nowDate;
    }

    function CalcPaymentPlanDate(control, index) {
        var $parentPayment = $(control).parents('div.payment-info');
        if ($parentPayment.length) {
            var $OPPaymentDay = $parentPayment.find('.payment-day-op');
            var $OPPaymentSiteType = $parentPayment.find('.payment-site-type-op');
            if ($OPPaymentDay.val() != 0
            && $OPPaymentSiteType.val() != 0
            && $(control).val().length == 7
            && iseiQ.utility.validDate($(control).val(), Constant.YM_FORMAT, Constant.TARGET_YM) == null) {
                var paymentDay = $OPPaymentDay.val();
                var paymentSiteType = parseInt($OPPaymentSiteType.val());
                var targetYm = $(control).val();
                var arrYm = targetYm.split('/');
                var paymentYear = parseInt(arrYm[0]);
                var paymentMonth = parseInt(arrYm[1]);
                var paymentPlanDate;
                paymentSiteType = paymentSiteType != 0 ? paymentSiteType - 1 : 0;

                if (paymentDay == PaymentDay.Day31) {
                    paymentPlanDate = new Date(paymentYear, paymentMonth - 1 + paymentSiteType + 1, 0);
                }
                else {
                    paymentPlanDate = new Date(paymentYear, paymentMonth - 1 + paymentSiteType, paymentDay);
                }

                var textValueDate = $.datepicker.formatDate('yy/mm/dd', paymentPlanDate);

                if (Contract == ContractDelegation) {
                    if (index == 0) {
                        $(control).parents('.split-payment-content').children('.col-center').find('.payment-plan-date-lb').text(textValueDate);
                        $(control).parents('.split-payment-content').children('.col-center').find('.payment-plan-date-hdd').val(textValueDate);
                    }
                    else {
                        $($(control).parents('.split-payment-content').find('.col-center tbody tr')[index]).find('.payment-plan-date-lb').text(textValueDate);
                        $($(control).parents('.split-payment-content').find('.col-center tbody tr')[index]).find('.payment-plan-date-hdd').val(textValueDate);

                    }
                }
                else if (index == 0) {
                    $(control).parents('.split-payment-content').children('.col-center').find('.payment-plan-date').val(textValueDate);
                }
                else {
                    $($(control).parents('.split-payment-content').find('.col-center tbody tr')[index]).find('.payment-plan-date').val(textValueDate);
                }
            }
            else {
                if (Contract == ContractDelegation) {
                    if (index == 0) {
                        $(control).parents('.split-payment-content').children('.col-center').find('.payment-plan-date-lb').text('');
                        $(control).parents('.split-payment-content').children('.col-center').find('.payment-plan-date-hdd').val('');
                    }
                    else {
                        $($(control).parents('.split-payment-content').find('.col-center tbody tr')[index]).find('.payment-plan-date-lb').text('');
                        $($(control).parents('.split-payment-content').find('.col-center tbody tr')[index]).find('.payment-plan-date-hdd').val('');
                    }
                }
                else if (index == 0) {
                    $(control).parents('.split-payment-content').children('.col-center').find('.payment-plan-date').val('');
                }
                else {
                    $($(control).parents('.split-payment-content').find('.col-center tbody tr')[index]).find('.payment-plan-date').val('');
                }
            }
        }
        $($(control).parents('.split-payment-content').find('.col-center tbody tr')[index]).find('.payment-plan-date').parents(".datepicker-days").datepicker('update');
    }

    function CalcDepositPlanDate(Contract, control, index) {
        if ($('#CONTRACT_OA_PAYMENT_DAY').val() != 0
        && $('#CONTRACT_OA_PAYMENT_SITE_TYPE').val() != 0
        && $(control).val().length == 7
        && iseiQ.utility.validDate($(control).val(), Constant.YM_FORMAT, Constant.TARGET_YM) == null) {
            var closingDay = $('#CONTRACT_OA_PAYMENT_DAY').val();
            var paymentSiteType = parseInt($('#CONTRACT_OA_PAYMENT_SITE_TYPE').val());
            var billingYm = $(control).val();
            var arrYm = billingYm.split('/');
            var billingYear = parseInt(arrYm[0]);
            var billingMonth = parseInt(arrYm[1]);
            var depositPlanDate;
            paymentSiteType = paymentSiteType != 0 ? paymentSiteType - 1 : 0;

            if (closingDay == ClosingDay.Day31) {
                depositPlanDate = new Date(billingYear, billingMonth - 1 + paymentSiteType + 1, 0);
            }
            else {
                depositPlanDate = new Date(billingYear, billingMonth - 1 + paymentSiteType, closingDay);
            }

            var textValueDate = $.datepicker.formatDate('yy/mm/dd', depositPlanDate);

            if (Contract == ContractDelegation)
            {
                if (index == 0) {
                    $(control).parents('.split-billing-content').children('.col-center').find('.deposit-plan-date-lb:not(.unchangeable)').text(textValueDate);
                    $(control).parents('.split-billing-content').children('.col-center').find('.deposit-plan-date-hdd').val(textValueDate);
                }
                else {
                    $($(control).parents('.split-billing-content').find('.col-center tbody tr')[index]).find('.deposit-plan-date-lb:not(.unchangeable)').text(textValueDate);
                    $($(control).parents('.split-billing-content').find('.col-center tbody tr')[index]).find('.deposit-plan-date-hdd').val(textValueDate);
                }
            }
            else if (index == 0)
            {
                $(control).parents('.split-billing-content').children('.col-center').find('.deposit-plan-date:not(.unchangeable)').val(textValueDate);
            }
            else
            {
                $($(control).parents('.split-billing-content').find('.col-center tbody tr')[index]).find('.deposit-plan-date:not(.unchangeable)').val(textValueDate);
            }
        }
        else {
            if (Contract == ContractDelegation)
            {
                if (index == 0) {
                    $(control).parents('.split-billing-content').children('.col-center').find('.deposit-plan-date-lb').text('');
                    $(control).parents('.split-billing-content').children('.col-center').find('.deposit-plan-date-hdd').val('');
                }
                else {
                    $($(control).parents('.split-billing-content').find('.col-center tbody tr')[index]).find('.deposit-plan-date-lb').text('');
                    $($(control).parents('.split-billing-content').find('.col-center tbody tr')[index]).find('.deposit-plan-date-hdd').val('');
                }
            }
            else if (index == 0)
            {
                $(control).parents('.split-billing-content').children('.col-center').find('.deposit-plan-date').val('');
            }
            else
            {
                $($(control).parents('.split-billing-content').find('.col-center tbody tr')[index]).find('.deposit-plan-date').val('');
            }
        }
        $($(control).parents('.split-billing-content').find('.col-center tbody tr')[index]).find('.deposit-plan-date').parents(".datepicker-days").datepicker('update');
    }

    function InitDatepickerMonths()
    {
        $(".date.datepicker-months").datepicker({
            format: "yyyy/mm",
            viewMode: "months",
            minViewMode: "months",
            autoclose: true,
            language: 'ja'
        });
    }

    function InitDatepickerDays()
    {
        $(".date.datepicker-days").datepicker({
            autoclose: true,
            language: 'ja'
        });
    }

    // Check user permission to show button or link
    function checkPermission(control,isAllow) {
        if ($(control).length > 0) {
            if (isAllow && isAllow == true) {
                return true;
            } else {
                return false;
            }            
        }
        return false;
    }

    function showConfirmModal(message, callback) {
        BootstrapDialog.closeAll();

        BootstrapDialog.show({
            title: '<i class="ui-icon ui-icon-alert"></i>',
            id: 'dlgConfirmModal',
            message: message,
            closable: false,
            buttons: [{
                label: 'Cancel',
                cssClass: 'btn btn-default btn-md-cancel',
                action: function (dialog) {
                    dialog.close();

                    if (typeof (callback) !== 'undefined') {
                        callback(false);
                    }
                }
            }, {
                label: 'OK',
                hotkey: 13,
                cssClass: 'btn btn-orange btn-md-ok',
                id: 'btnConfirmModalOK',
                action: function (dialog) {
                    dialog.close();

                    if (typeof (callback) == 'function') {
                        callback(true);
                    }
                }
            }]
        });
    }

    function showMessageModal(message, warning, callback) {
        BootstrapDialog.closeAll();
        var iconType = warning ? 'ui-icon-alert' : 'ui-icon-circle-check';

        if (typeof (message) != 'undefined' && message.length > 0) {
            message = message.replace(/\n/g, '</br>');
        }

        BootstrapDialog.show({
            title: '<i class="ui-icon ' + iconType + '"></i>',
            id: 'dlgMessageModal',
            message: message,
            closable: false,
            buttons: [{
                label: 'OK',
                hotkey: 13,
                cssClass: 'btn btn-orange btn-md-ok',
                id: 'btnMessageModalOK',
                action: function (dialog) {
                    dialog.close();

                    if (typeof (callback) == 'function') {
                        callback(null);
                    }
                }
            }]
        });
    }

    // Set data for product no
    function setProductNo(productSeqNo) {
        if ($("#PRODUCT_NO").length > 0) {
            if (productSeqNo == '') {
                $("#PRODUCT_NO").val('');
            } else {
                var param = { "productsegno": productSeqNo };
                var data = getDataByAjax('/Estimate/Estimate/GetExpandFlagByProductSegNo', param);
                $("#PRODUCT_NO").val(data.productno);
            }
        }
    }

    // Bind data of PIC to select list
    function bindPIC($departmentList, $staffList) {
        var department = $departmentList.find('option:selected').val();

        $staffList.find('option:not(:first)').each(function () {
            if ($(this).parent().is('i')) $(this).unwrap();
        });
        $staffList.find('option:not(:first)').wrap('<i></i>');

        if (department != null && department.length > 0 && $.isNumeric(department)) {
            var departmentId = { "departmentID": parseInt(department) };
            var dataPicList = getDataByAjax('/Common/Common/GetPICSelectList', departmentId);

            $.each(dataPicList, function (index, data) {
                var keyItem = data.key;

                $staffList.find('option:not(:first)').each(function () {
                    if (this.value == keyItem) {
                        $(this).unwrap();
                        return false;
                    }
                });
            });
        }
    }

    function bindPICExtend($departmentList, $staffList) {
        if (typeof ($departmentList) == 'undefined') {
            $departmentList = $('.department-index');
            $staffList = $('.pic-index');
        }
        bindPIC($departmentList, $staffList);
    }

    // Bind data sub contract type to select list
    function bindSubContractType($contractType, $subContractType, $product) {
        if (Constant.IS_DISPLAY_SUBCONTRACT_TYPE) {
            var contractType = $contractType.find('option:selected').val();

            $subContractType.find('option:not(:first)').each(function () {
                if ($(this).parent().is('i')) {
                    $(this).unwrap();
                }
            });

            $product.find('option:not(:first)').each(function () {
                if ($(this).parent().is('i')) {
                    $(this).unwrap();
                }
            });

            $subContractType.find('option:not(:first)').wrap('<i></i>');
            $product.find('option:not(:first)').wrap('<i></i>');

            if (contractType != null && contractType.length > 0 && $.isNumeric(contractType)) {
                var param = { "contractType": parseInt(contractType) };
                var dataSubContractType = getDataByAjax('/Common/Common/GetSubContractTypeSelectList', param);

                $.each(dataSubContractType, function (index, data) {
                    var keyItem = data.key;

                    $subContractType.find('option:not(:first)').each(function () {
                        var valueArr = this.value.split('_');

                        if (valueArr[0] == contractType && valueArr[1] == keyItem) {
                            $(this).unwrap();
                            return false;
                        }
                    });
                });
            }
        }

        bindProduct($contractType, $subContractType, $product);
        setProductNo('');
    }

    function bindSubContractTypeExtend($contractType, $subContractType, $product) {
        if (typeof ($contractType) == 'undefined') {
            $contractType = $('.ddlContractTypeMaster');
            $subContractType = $('.ddlSubContractTypeMaster');
            $product = $('.ddlProductMaster');
        }
        bindSubContractType($contractType, $subContractType, $product);
    }

    // Bind data product to select list
    function bindProduct($contractType, $subContractType, $product) {
        if ($product.length > 0) {
            var contractType = $contractType.find('option:selected').val();
            var subContractType = Constant.IS_DISPLAY_SUBCONTRACT_TYPE ? $subContractType.find('option:selected').val().split('_')[1] : Constant.DEFAULT_SUB_CONTRACT_TYPE_VALUE;

            $product.find('option:not(:first)').each(function () {
                if ($(this).parent().is('i')) {
                    $(this).unwrap();
                }
            });

            $product.find('option:not(:first)').wrap('<i></i>');

            if (contractType != null && subContractType != null
                && contractType.length > 0 && subContractType.length > 0
                && $.isNumeric(contractType) && $.isNumeric(subContractType)) {
                var param = { "contractType": parseInt(contractType), "subContractType": parseInt(subContractType) };
                var dataProduct = getDataByAjax('/Common/Common/GetProductSelectList', param);

                $.each(dataProduct, function (index, data) {
                    var keyItem = data.key;

                    $product.find('option:not(:first)').each(function () {
                        if (this.value == keyItem) {
                            $(this).unwrap();
                            return false;
                        }
                    });
                });
            }

            setProductNo('');
        }
    }

    function bindProductExtend($contractType, $subContractType, $product) {
        if (typeof ($contractType) == 'undefined') {
            $contractType = $('.ddlContractTypeMaster');
            $subContractType = $('.ddlSubContractTypeMaster');
            $product = $('.ddlProductMaster');
        }

        bindProduct($contractType, $subContractType, $product);
    }

    function bindContractTypeExtend($contractFirm, $contractType, $subContractType, $product) {
        if (typeof ($contractFirm) == 'undefined') {
            $contractFirm = $('.ddlContractFirmMaster');
            $contractType = $('.ddlContractTypeMaster');
            $subContractType = $('.ddlSubContractTypeMaster');
            $product = $('.ddlProductMaster');
        }
        bindContractType($contractFirm, $contractType, $subContractType, $product);
    }

    // Bind data sub contract type to select list
    function bindContractType($contractFirm, $contractType, $subContractType, $product) {
        var contractFirm = $contractFirm.val();

        $contractType.find('option:not(:first)').each(function () {
            if ($(this).parent().is('i')) {
                $(this).unwrap();
            }
        });

        $subContractType.find('option:not(:first)').each(function () {
            if ($(this).parent().is('i')) {
                $(this).unwrap();
            }
        });

        $product.find('option:not(:first)').each(function () {
            if ($(this).parent().is('i')) {
                $(this).unwrap();
            }
        });

        $contractType.find('option:not(:first)').wrap('<i></i>');
        $subContractType.find('option:not(:first)').wrap('<i></i>');
        $product.find('option:not(:first)').wrap('<i></i>');

        if (contractFirm != null && contractFirm.length > 0) {
            var param = { "contractFirm": contractFirm };
            var dataContractFirm = getDataByAjax('/Common/Common/GetContractTypeSelectListByOutside', param);

            $.each(dataContractFirm, function (index, data) {
                var keyItem = data.key;
                
                $contractType.find('option:not(:first)').each(function () {
                    var valueArr = this.value.split('_');

                    if (valueArr[0] == contractFirm && valueArr[1] == keyItem) {
                        $(this).unwrap();
                        return false;
                    }
                });
            });
        }

        bindSubContractTypeByOutside($contractFirm, $contractType, $subContractType, $product);
        setProductNo('');
    }

    function bindSubContractTypeByOutsideExtend($contractFirm, $contractType, $subContractType, $product) {
        if (typeof ($contractType) == 'undefined') {
            $contractFirm = $('.ddlContractFirmMaster');
            $contractType = $('.ddlContractTypeMaster');
            $subContractType = $('.ddlSubContractTypeMaster');
            $product = $('.ddlProductMaster');
        }
        bindSubContractTypeByOutside($contractFirm, $contractType, $subContractType, $product);
    }

    // Bind data sub contract type to select list
    function bindSubContractTypeByOutside($contractFirm, $contractType, $subContractType, $product) {
        var contractFirm = $contractFirm.val();
        var contractType = $contractType.find('option:selected').val().split('_')[1];

        $subContractType.find('option:not(:first)').each(function () {
            if ($(this).parent().is('i')) {
                $(this).unwrap();
            }
        });

        $product.find('option:not(:first)').each(function () {
            if ($(this).parent().is('i')) {
                $(this).unwrap();
            }
        });

        $subContractType.find('option:not(:first)').wrap('<i></i>');
        $product.find('option:not(:first)').wrap('<i></i>');

        if (contractType != null && contractType.length > 0 && $.isNumeric(contractType)) {
            var param = { "contractFirm": contractFirm, "contractType": parseInt(contractType) };
            var dataSubContractType = getDataByAjax('/Common/Common/GetSubContractTypeSelectListByOutside', param);

            $.each(dataSubContractType, function (index, data) {
                var keyItem = data.key;

                $subContractType.find('option:not(:first)').each(function () {
                    var valueArr = this.value.split('_');

                    if (valueArr[0] == contractFirm && valueArr[1] == contractType && valueArr[2] == keyItem) {
                        $(this).unwrap();
                        return false;
                    }
                });
            });
        }

        bindProductByOutside($contractFirm, $contractType, $subContractType, $product);
        setProductNo('');
    }

    function bindProductExtendByOutside($contractFirm, $contractType, $subContractType, $product) {
        if (typeof ($contractFirm) == 'undefined') {
            $contractFirm = $('.ddlContractFirmMaster');
            $contractType = $('.ddlContractTypeMaster');
            $subContractType = $('.ddlSubContractTypeMaster');
            $product = $('.ddlProductMaster');
        }

        bindProductByOutside($contractFirm, $contractType, $subContractType, $product);
    }

    // Bind data product to select list
    function bindProductByOutside($contractFirm, $contractType, $subContractType, $product) {
        if ($product.length > 0) {
            var contractFirm = $contractFirm.val();
            var contractType = $contractType.find('option:selected').val().split('_')[1];
            var subContractType = $subContractType.find('option:selected').val().split('_')[2];

            $product.find('option:not(:first)').each(function () {
                if ($(this).parent().is('i')) {
                    $(this).unwrap();
                }
            });

            $product.find('option:not(:first)').wrap('<i></i>');

            if (contractFirm != null && contractType != null && subContractType != null
                && contractFirm != "" && contractType.length > 0 && subContractType.length > 0
                && $.isNumeric(contractType) && $.isNumeric(subContractType)) {

                var param = { "contractFirm": contractFirm, "contractType": parseInt(contractType), "subContractType": parseInt(subContractType) };
                var dataProduct = getDataByAjax('/Common/Common/GetProductSelectListByOutside', param);

                $.each(dataProduct, function (index, data) {
                    var keyItem = data.key;

                    $product.find('option:not(:first)').each(function () {
                        if (this.value == keyItem) {
                            $(this).unwrap();
                            return false;
                        }
                    });
                });
            }

            setProductNo('');
        }
    }

    function getSameDayAddMonths(inputDate, addMonth) {
        var year = inputDate.getFullYear();
        var month = inputDate.getMonth();
        var day = inputDate.getDate();
        if (day === 0) {
            day = 1;
        }
        if (day >= new Date(year, month + 1, 0).getDate()) {
            return new Date(year, month + addMonth + 1, 0); // End of month
        }
        return new Date(year, month + addMonth, day);
    }

    function getFirstClosingDate(startDate, endDate, closingDay) {
        var startDateVal = new Date(startDate);
        var firstClosingDate;

        if (closingDay == 31) {
            // set last day of next month
            firstClosingDate = new Date(startDateVal.getFullYear(), startDateVal.getMonth() + 1, 0);
        }
        else if (closingDay >= startDateVal.getDate()) {
            // get closing day this month
            firstClosingDate = new Date(startDateVal.getFullYear(), startDateVal.getMonth(), closingDay);
        }
        else // closingDay < startDate.Day
        {
            //get closing day this month
            firstClosingDate = new Date(startDateVal.getFullYear(), startDateVal.getMonth() + 1, closingDay);
        }
        return firstClosingDate;
    }

    function CountBillingMonths(startDate, endDate, closingDay) {
        var countYm = 0;
        var startDateVal = new Date(startDate);
        var endDateVal = new Date(endDate);

        for (var tmpDate = getFirstClosingDate(startDateVal, endDateVal, closingDay);
            tmpDate.getTime() < getSameDayAddMonths(endDateVal, 1).getTime();
            tmpDate = getSameDayAddMonths(tmpDate, 1))
        {
            countYm++;
            if (tmpDate.getTime() >= endDateVal.getTime()) {
                break;
            }
        }
        return countYm;
    }

    function autoCompleteYearMonthDay(dateString) {
        var splittedString = dateString.split('/');
        if (splittedString.length == 3) {
            var year = splittedString[0], month = splittedString[1], day = splittedString[2];
            year = !parseInt(year) ? '2000' : parseInt(year) <= 999 ? (parseInt(year) + 2000).toString() : year;
            month = month.length >= 2 ? month : month.length == 1 ? '0' + month : month.length == 0 ? '01' : month;
            day = day.length >= 2 ? day : day.length == 1 ? '0' + day : day.length == 0 ? '01' : day;
            return year + '/' + month + '/' + day;
        }
        return dateString;
    }

    function autoCompleteYearMonth(ymString) {
        var splittedString = ymString.split('/');
        if (splittedString.length == 2) {
            var year = splittedString[0], month = splittedString[1];
            year = !parseInt(year) ? '2000' : parseInt(year) < 2000 ? (parseInt(year) + 2000).toString() : year;
            month = month.length >= 2 ? month : month.length == 1 ? '0' + month : month.length == 0 ? '01' : month;
            return year + '/' + month;
        }
        return ymString;
    }

    function checkInputDecimal(control) {
        control.on("change keyup", function (e) {
            var text = control.val();
            var regX = /[^0-9/.]/g;

            if (regX.test(text)) {
                control.val(text.replace(regX, ''));
            }
        });
    }

    //Check Alphanumeric and half size
    function checkInputAlphanumeric(control) {
        control.on("change keyup", function (e) {
            var text = control.val();
            var regX = /[^a-zA-Z0-9]/g;
            if (regX.test(text)) {
                text = text.replace(regX, '');
                control.val(text);
            }
        });
    }

    //  Gets the browser name or returns an empty string if unknown
    var checkBrowser = function () {
        // Return cached result if avalable, else get result then cache it.
        if (checkBrowser.prototype._cachedResult)
            return checkBrowser.prototype._cachedResult;

        // Opera 8.0+ (UA detection to detect Blink/v8-powered Opera)
        var isOpera = !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0;
        // Firefox 1.0+
        var isFirefox = typeof InstallTrigger !== 'undefined';
        // At least Safari 3+: "[object HTMLElementConstructor]"
        var isSafari = Object.prototype.toString.call(window.HTMLElement).indexOf('Constructor') > 0;
        // Chrome 1+
        var isChrome = !!window.chrome && !isOpera;
        // At least IE6
        var isIE = false || !!document.documentMode;

        return (checkBrowser.prototype._cachedResult =
            isOpera ? 'Opera' :
            isFirefox ? 'Firefox' :
            isSafari ? 'Safari' :
            isChrome ? 'Chrome' :
            isIE ? 'IE' :
            '');
    };

    return {
        formatJSONDate: formatJSONDate,
        formatYearMonth: formatYearMonth,
        formatDecimalToTime: formatDecimalToTime,
        formatMoney: formatMoney,
        formatMoneyTypeDecimal:formatMoneyTypeDecimal,
        formatMoneyLabel: formatMoneyLabel,
        convertMoneyToInt: convertMoneyToInt,
        convertIntToMoney: convertIntToMoney,
        replaceAllMoney: replaceAllMoney,
        validDate: validDate,
        validateRangeYear: validateRangeYear,
        validateDeliveryDate: validateDeliveryDate,
        compareDate: compareDate,
        compareDateRange: compareDateRange,
        validAlphanumeric: validAlphanumeric,
        validPositiveNumeric: validPositiveNumeric,
        validNegativeNumeric: validNegativeNumeric,
        showClientError: showClientError,
        validNumeric: validNumeric,
        validAccount: validAccount,
        validEmail: validEmail,
        validFullHalfSize: validFullHalfSize,
        validLoginScreen: validLoginScreen,
        htmlEncode: htmlEncode,
        htmlDecode: htmlDecode,
        byteCount: byteCount,
        roundingDecimal: roundingDecimal,
        roundDecimalPercent: roundDecimalPercent,
        roundByType: roundByType,
        roundTimeByUnit: roundTimeByUnit,
        showMessageDialog: showMessageDialog,
        showMessageDialogCB: showMessageDialogCB,
        htmlDialog: htmlDialog,
        showSubmitConfirmDialog: showSubmitConfirmDialog,
        checkInputNumber: checkInputNumber,
        checkCompareBasetime: checkCompareBasetime,
        buildColumShortText: buildColumShortText,
        buildColumShortTextArea: buildColumShortTextArea,
        buildColumTextBox: buildColumTextBox,
        buildColumRadio: buildColumRadio,
        buildColumDropDownList: buildColumDropDownList,
        builDetailFormCode: builDetailFormCode,
        IsAuthenticateTimeout: IsAuthenticateTimeout,
        setStatusTimeOut: setStatusTimeOut,
        checkTimeout: checkTimeout,
        validURL: validURL,
        focusTextbox: focusTextbox,
        setFocusAmount: setFocusAmount,
        validDecimalNumeric: validDecimalNumeric,
        validDecimalByType: validDecimalByType,
        checkInputPhone: checkInputPhone,
        imeControl: imeControl,
        getDataByAjax: getDataByAjax,
        getDataByAjaxCB: getDataByAjaxCB,
        checkDataExistByAjax: checkDataExistByAjax,
        getImageByAjax: getImageByAjax,
        getMonthCols: getMonthCols,
        bindMonthCols: bindMonthCols,
        bindTotalCols: bindTotalCols,
        bindTagsByCustomer: bindTagsByCustomer,
        buildEditForm: buildEditForm,
        buildConfirmForm: buildConfirmForm,
        validPhone: validPhone,
        GetDateTimeNow: GetDateTimeNow,
        GetDateNow: GetDateNow,
        CalcPaymentPlanDate: CalcPaymentPlanDate,
        CalcDepositPlanDate: CalcDepositPlanDate,
        InitDatepickerMonths: InitDatepickerMonths,
        InitDatepickerDays: InitDatepickerDays,
        checkPermission: checkPermission,
        showConfirmModal: showConfirmModal,
        showMessageModal: showMessageModal,
        displayMultiline: displayMultiline,
        bindPICExtend: bindPICExtend,
        bindPIC: bindPICExtend,
        bindSubContractTypeExtend: bindSubContractTypeExtend,
        bindProductExtend: bindProductExtend,
        setProductNo: setProductNo,
        displayGroupName: displayGroupName,
        checkBrowser: checkBrowser,
        CountBillingMonths: CountBillingMonths,
        GetSameDayAddMonths: getSameDayAddMonths,
        GetFirstClosingDate: getFirstClosingDate,
        autoCompleteYearMonthDay: autoCompleteYearMonthDay,
        autoCompleteYearMonth: autoCompleteYearMonth,
        isValidTargetYM: isValidTargetYM,
        bindContractTypeExtend: bindContractTypeExtend,
        bindSubContractTypeByOutsideExtend: bindSubContractTypeByOutsideExtend,
        checkInputDecimal: checkInputDecimal,
        checkInputAlphanumeric: checkInputAlphanumeric,
        bindProductExtendByOutside: bindProductExtendByOutside,
        checkValidFullHalfSize: checkValidFullHalfSize
    };
}());