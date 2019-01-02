/*!
 * File: iseiQ.EstiamateView.js
 * Company: i-Enter Asia
 * Copyright © 2015 i-Enter Asia. All rights reserved.
 * Project: iseiQ
 * Author: GiangVT
 * Created date: 2015/02/04
 */

$(document).ready(function () {
    iseiQ.utility.formatMoney();
    iseiQ.utility.formatMoneyLabel();
    iseiQ.utility.focusTextbox();

    setPaymenMethod();
    var displaySes = setDisplaySubProductByContractInputType();

    if (displaySes == false) {        
        $('.unit_time_type').text('円');
    }
    displayTargetYearMonth();
});

function setPaymenMethod() {
    $('.payment_method').each(function () {
        var timeIndex = this.value;

        if (timeIndex > TimeIndex.CONTRACT_PERIOD_START) {
            var $targetRow = $(this).parents('.subproduct-content');

            $targetRow.find('.basetime_lower, .basetime_upper, .exceed_unit, .deduction_unit, .unit_exceed, .unit_deduction, .calc_unit, .timeunit, .unit').css('opacity', '0');
            $targetRow.find('.unit_time_type').text('円/月');

            if (timeIndex == TimeIndex.DELIVERY_DATE) {
                $targetRow.find('.timeunit, .unit').css('opacity', '1');
                $targetRow.find('.unit_time_type').text('円/h');
            }
        }
    });
}

function setDisplaySubProductByContractInputType() {
    var displaySes = null;

    switch ($('#CONTRACT_INPUT_TYPE').val()) {
        case Constant.OUTSOURCING:
        case Constant.STAFF:
        case Constant.CONTRACT:
        case Constant.DELEGATION:
        case Constant.GENERAL_DISPATCH:
        case Constant.SPECIFIC_DISPATCH:
            displaySes = true;
            break;
        case Constant.TRUSTEE:
        case Constant.PRODUCT_SALE:
        case Constant.MAINTAINANCE:
            displaySes = false;
            break;
        default:
            break;
    }

    return displaySes;
}
function displayTargetYearMonth() {
    var isDisplay = null;

    $('.base_time_type').each(function (index) {
        var idOfSelect = $(this).attr("name");
        if (idOfSelect) {
            if ($(this).val() === "1") {
                isDisplay = true;
            } 
        }
    });

    if (isDisplay) {
        $('.ses_special_payment').show();
    } else {
        $('.ses_special_payment').hide();
    }
}