function getCalendarOption() {
    var option = {
        closeText: '閉じる',
        prevText: '&#x3c;前',
        nextText: '次&#x3e;',
        currentText: '今日',
        monthNames: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],
        monthNamesShort: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],
        dayNames: ['日曜日', '月曜日', '火曜日', '水曜日', '木曜日', '金曜日', '土曜日'],
        dayNamesShort: ['日', '月', '火', '水', '木', '金', '土'],
        dayNamesMin: ['日', '月', '火', '水', '木', '金', '土'],
        weekHeader: '週',
        dateFormat: 'yy/mm/dd',
        firstDay: 0,
        isRTL: false,
        showButtonPanel: true,
        showOtherMonths: true,
        showMonthAfterYear: true,
        yearSuffix: '年',
        showOn: "button",
        buttonImageOnly: false
    };

    return option;
};

function getInitialCalendar() {
    var option = {
        closeText: '閉じる',
        prevText: '&#x3c;前',
        nextText: '次&#x3e;',
        currentText: '今日',
        monthNames: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],
        monthNamesShort: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],
        dayNames: ['日曜日', '月曜日', '火曜日', '水曜日', '木曜日', '金曜日', '土曜日'],
        dayNamesShort: ['日', '月', '火', '水', '木', '金', '土'],
        dayNamesMin: ['日', '月', '火', '水', '木', '金', '土'],
        weekHeader: '週',
        dateFormat: 'yy/mm/dd',
        firstDay: 0,
        isRTL: false,
        showButtonPanel: true,
        showOtherMonths: true,
        showMonthAfterYear: true,
        yearSuffix: '年',
        showOn: "button",
        buttonImageOnly: false,
        defaultDate: '1980/01/01'
    };

    return option;
};

function getCalendarOnlyMonthYearOption() {
    var option = {
        closeText: '閉じる',
        prevText: '&#x3c;前',
        nextText: '次&#x3e;',
        currentText: '今日',
        monthNames: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],
        monthNamesShort: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],
        dateFormat: 'yy/mm',
        isRTL: false,
        firstDay: 0,
        showButtonPanel: true,
        showOn: "button",
        buttonImageOnly: false,
        changeMonth: true,
        changeYear: true,
        onChangeMonthYear: function (year, month, inst) {
            $(this).val($.datepicker.formatDate('yy/mm', new Date(year, month - 1, 1)));
        },
        beforeShow: function (input, inst) {
            var defaultDate = new Date();

            if ($(input).val().length > 0) {
                var check = iseiQ.utility.validDate($(input).val(), 'yyyy/mm', '');
                if (iseiQ.utility.validDate($(input).val(), 'yyyy/mm', '') == null) {
                    defaultDate = $(input).val() + '/01';
                }
            }

            $(this).datepicker('option', 'defaultDate', new Date(defaultDate));
            $(this).val($.datepicker.formatDate('yy/mm', new Date(defaultDate)));
        }
    };

    return option;
};