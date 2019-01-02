/*!
 * File: iseiQ.EstiamateRegister.js
 * Company: i-Enter Asia
 * Copyright © 2015 i-Enter Asia. All rights reserved.
 * Project: iseiQ
 * Author: GiangVT
 * Created date: 2015/02/04
 */

$(document).ready(function () {
    var startDate = $('#PRJ_PERIOD_START').val();
    var endDate = $('#PRJ_PERIOD_END').val();

    iseiQ.utility.validFullHalfSize($(".ime-mode"));
    iseiQ.utility.imeControl($("#BP_NAME_DISP, #PRJ_NAME, #PRJ_DSP_NAME, #PRJ_DESC_NAME, #PROJECT_MEMO_ENTITY_MEMO, #ESTIMATE_CONDITION_ENTITY_EST_CONDITION"), 'active');
    iseiQ.utility.imeControl($("#EST_NO, #EST_NO_SC, #EST_AMOUNT_TOTAL, .datefield, .ime-mode"), 'disable');

    iseiQ.utility.formatMoney();
    iseiQ.utility.formatMoneyLabel();
    iseiQ.utility.focusTextbox();

    // Save temp start date and end date
    $('#hdnTempStart').val(startDate);
    $('#hdnTempTo').val(endDate);

    if ($('#BP_NAME_DISP').val().length > 0) {
        $("#btnSelectPIC").removeAttr("disabled");
    }

    displayEstimateDetail();
    displayTargetYearMonth();
    $('.product-segno').val($('#PRODUCT_INDEX').val());
    calculateAmount();

    if ($('#EST_SEQ_NO').val() != 0) {
        $('.select-contract-type').removeAttr("name");
        $('.select-contract-sub-type').removeAttr("name");
        $('.select-product').removeAttr("name");
    }
});

//calculate amount of each product
function calculateAmount() {
    $('.col-center .subproduct-content').each(function() {
        var unitprice = iseiQ.utility.convertMoneyToInt($(this).find('.unit-price').val(), true);
        var rounding = $('#ROUNDING_TYPE').val();
        var quantity = $(this).find('.quantity').val();
        var amount = iseiQ.utility.roundByType(rounding, unitprice * quantity);
        var stramount = iseiQ.utility.convertIntToMoney(amount);
        $(this).find('.sc-payment').val(stramount);
    });
    calTotalestimate();
}

// Validate data input
function validateData() {
    var invalidMess = [];

    // Validate customer name  
    var customerName = $('#BP_NAME_DISP').val().trim();
    if (customerName.length === 0) { // required customer name
        invalidMess.push(Constant.CUSTOMER_NAME + Constant.ERR_REQUIRED);
    }
    else if (customerName.length > Constant.MAX_CUSTOMER_NAME) { // valid string length
        invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.CUSTOMER_NAME).replace('{1}', Constant.MAX_CUSTOMER_NAME));
    }

    // Validate customer name  BP_STAFF_NAME
    var staffName = $('#BP_STAFF_NAME').val().trim();
    if (staffName.length === 0) { // required customer name
        invalidMess.push(Constant.CHARGE_PERSON + Constant.ERR_REQUIRED);
    }
    else if (customerName.length > Constant.MAX_CUSTOMER_NAME) { // valid string length
        invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.CHARGE_PERSON).replace('{1}', Constant.MAX_CUSTOMER_NAME));
    }

    // Validate project name
    var projectName = $('#PRJ_NAME').val().trim();
    if (projectName.length === 0) { // required project name
        invalidMess.push(Constant.PROJECT_NAME + Constant.ERR_REQUIRED);
    }
    else if (projectName.length > Constant.MAX_PROJECT_NAME) { // valid string length
        invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.PROJECT_NAME).replace('{1}', Constant.MAX_PROJECT_NAME));
    }

    // Validate project display name
    var projectDisplayName = $('#PRJ_DSP_NAME').val().trim();
    if (projectDisplayName.length > Constant.MAX_PROJECT_NAME) { // valid string length
        invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.PROJECT_DISPLAY_NAME).replace('{1}', Constant.MAX_PROJECT_NAME));
    }

    // Validate project detail name
    var projectDetailName = $('#PRJ_DESC_NAME').val().trim();
    if (projectDetailName.length > Constant.MAX_PROJECT_NAME) { // valid string length
        invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.PROJECT_DETAIL_NAME).replace('{1}', Constant.MAX_PROJECT_NAME));
    }

    // Validate 主担当者名 - project PIC
    var staffId = $('#PROJECT_REPRESENTATIVE_ENTITY_STAFF_ID').val();
    if (staffId == null) {
        invalidMess.push(Constant.PROJECT_PIC + Constant.ERR_REQUIRED);
    }

    //initialize date
    var startDate = $('#PRJ_PERIOD_START').val().trim();
    var endDate = $('#PRJ_PERIOD_END').val().trim();
    var deliveryDate = $('#DELIVERY_DATE').val().trim();
    var acceptanceDate = $('#ACCEPTANCE_DATE').val().trim();
    var existInvalidDate = false;
    var errStartDate = null;

    // Validate start date
    if (startDate.length === 0) { // required start date
        existInvalidDate = true;
        errStartDate = Constant.PERIOD_START + Constant.ERR_REQUIRED;
        invalidMess.push(errStartDate);
    }
    else if (startDate.length > 0) {
        errStartDate = iseiQ.utility.validDate(startDate, Constant.DATE_FORMAT, Constant.PERIOD_START);
        if (errStartDate != null) {
            existInvalidDate = true;
            invalidMess.push(errStartDate);
        }
    }
    else if (errStartDate == null && startDate.length > Constant.MAX_DATE) {
        existInvalidDate = true;
        invalidMess.push(Constant.PERIOD_START + Constant.ERR_INCORRECT_DATE);
    }

    // Validate end date
    var errEndDate = null;
    if (endDate.length === 0) { // required end date
        existInvalidDate = true;
        errEndDate = Constant.PERIOD_END + Constant.ERR_REQUIRED;
        invalidMess.push(errEndDate);
    }
    else if (endDate.length > 0) {
        errEndDate = iseiQ.utility.validDate(endDate, Constant.DATE_FORMAT, Constant.PERIOD_END);
        if (errEndDate != null) {
            existInvalidDate = true;
            invalidMess.push(errEndDate);
        }
    }
    else if (errEndDate == null && endDate.length > Constant.MAX_DATE) {  // validate end date
        existInvalidDate = true;
        invalidMess.push(Constant.PERIOD_END + Constant.ERR_INCORRECT_DATE);
    }

    // Validate delivery date
    var errDeliveryDate = null;
    if (deliveryDate.length === 0) { // required delivery date        
        errDeliveryDate = Constant.DELIVERY_DATE + Constant.ERR_REQUIRED;
        invalidMess.push(errDeliveryDate);
    }
    else if (deliveryDate.length > 0) {
        errDeliveryDate = iseiQ.utility.validDate(deliveryDate, Constant.DATE_FORMAT, Constant.DELIVERY_DATE);
        if (errDeliveryDate != null) {
            invalidMess.push(errDeliveryDate);
        }
        else if (!iseiQ.utility.compareDate(startDate, deliveryDate, Constant.DATE_FORMAT)) {
            errDeliveryDate = Constant.DELIVERY_DATE + Constant.ERR_COMPARE_DATE;
            invalidMess.push(errDeliveryDate);
        }
        else if (!iseiQ.utility.compareDate(deliveryDate, endDate, Constant.DATE_FORMAT)) {
            errDeliveryDate = Constant.DELIVERY_DATE + Constant.ERR_COMPARE_END_DATE;
            invalidMess.push(errDeliveryDate);
        }
    }
    else if (errDeliveryDate == null && deliveryDate.length > Constant.MAX_DATE) { // valid string length
        invalidMess.push(Constant.DELIVERY_DATE + Constant.ERR_INCORRECT_DATE);
    }

    // Validate acceptance date
    var errAcceptanceDate = null;
    if (acceptanceDate.length === 0) { // required acceptance date        
        errAcceptanceDate = Constant.ACCEPTANCE_DATE + Constant.ERR_REQUIRED;
        invalidMess.push(errAcceptanceDate);
    }
    else if (acceptanceDate.length > 0) {
        errAcceptanceDate = iseiQ.utility.validDate(acceptanceDate, Constant.DATE_FORMAT, Constant.ACCEPTANCE_DATE);
        if (errAcceptanceDate != null) {
            invalidMess.push(errAcceptanceDate);
        }
        else if (!iseiQ.utility.compareDate(startDate, acceptanceDate, Constant.DATE_FORMAT)) {
            errAcceptanceDate = Constant.ACCEPTANCE_DATE + Constant.ERR_COMPARE_DATE;
            invalidMess.push(errAcceptanceDate);
        }
        else if (!iseiQ.utility.compareDate(deliveryDate, acceptanceDate, Constant.DATE_FORMAT)) {
            errAcceptanceDate = Constant.ACCEPTANCE_DATE + Constant.ERR_COMPARE_ACCEPTANCE_DATE;
            invalidMess.push(errAcceptanceDate);
        }
        else if (!iseiQ.utility.compareDate(acceptanceDate, endDate, Constant.DATE_FORMAT)) {
            errAcceptanceDate = Constant.ACCEPTANCE_DATE + Constant.ERR_COMPARE_END_DATE;
            invalidMess.push(errAcceptanceDate);
        }
    }
    else if (errAcceptanceDate == null && acceptanceDate.length > Constant.MAX_DATE) { // valid string length      
        invalidMess.push(Constant.ACCEPTANCE_DATE + Constant.ERR_INCORRECT_DATE);
    }

    // compare period
    if (!existInvalidDate &&
        !iseiQ.utility.compareDate(startDate, endDate, Constant.DATE_FORMAT)) {
        existInvalidDate = true;
        invalidMess.push(Constant.PERIOD_END + Constant.ERR_COMPARE_DATE);
    }

    // Payment site type
    var paymentSiteType = $('#PAYMENT_SITE_TYPE').val();
    if (paymentSiteType == 0) {
        invalidMess.push(Constant.PAYMENT_SITE_TYPE + Constant.ERR_REQUIRED_CHOOSE);
    }

    // Payment day
    var paymentDay = $('#PAYMENT_DAY').val();
    if (paymentDay == 0) {
        invalidMess.push(Constant.PAYMENT_DAY + Constant.ERR_REQUIRED_CHOOSE);
    }

    // validate tax rate length
    var taxrate = $('#TAX_RATE').val().trim();
    if (taxrate.length === 0) { // required delivery date        
        invalidMess.push(Constant.TAX_RATE + Constant.ERR_REQUIRED);
    }
    if (taxrate.length > Constant.MAX_TAXRATE) { // valid string length        
        invalidMess.push(Constant.TAX_RATE + Constant.ERR_RANGE_TAXRATE);
    }

    // validate estmate total amount
    var estAmount = iseiQ.utility.convertMoneyToInt($('#EST_AMOUNT_TOTAL').val(), true);
    if (estAmount.toString().length > 0 && (estAmount < Constant.MIN_AMOUNT || estAmount > Constant.MAX_AMOUNT)) {
        invalidMess.push(Constant.ESTTIMATE_AMOUNT_TOTAL + Constant.ERR_RANGE_AMOUNT);
    }

    // validate estmate total amount on tax
    var estAmountOnTax = iseiQ.utility.convertMoneyToInt($('#EST_AMOUNT_TOTAL_ON_TAX').val(), true);
    if (estAmountOnTax.toString().length > 0 && (estAmountOnTax < Constant.MIN_AMOUNT || estAmountOnTax > Constant.MAX_AMOUNT)) {
        invalidMess.push(Constant.ESTTIMATE_AMOUNT_TOTAL_ON_TAX + Constant.ERR_RANGE_AMOUNT);
    }

    // validate estmate taxable amount
    var estTaxableAmount = iseiQ.utility.convertMoneyToInt($('#EST_TAXABLE_AMOUNT_TOTAL').val(), true);
    if (estTaxableAmount.toString().length > 0 && (estTaxableAmount < Constant.MIN_AMOUNT || estTaxableAmount > Constant.MAX_AMOUNT)) {
        invalidMess.push(Constant.ESTTIMATE_TAXABLE_AMOUNT_TOTAL + Constant.ERR_RANGE_AMOUNT);
    }

    // validate estmate tax
    var tax = iseiQ.utility.convertMoneyToInt($('#TAX').val(), true);
    if (tax.toString().length > 0 && (tax < Constant.MIN_AMOUNT || tax > Constant.MAX_AMOUNT)) {
        invalidMess.push(Constant.ESTTIMATE_TAX + Constant.ERR_RANGE_AMOUNT);
    }

    // validate base on contract input type
    var estimateType = setDisplaySubProductByContractInputType() ? Constant.SES_SELECT : Constant.NO_SELECT;
    var $scList = $('.subproduct-content');

    if ($scList.length > 0) {
        for (var i = 0; i < $scList.length; i++) {
            var $targetRow = $($scList[i]);

            // validate item-segno
            var itemsegno = $targetRow.find('.item-segno').val();
            if (!itemsegno) {
                invalidMess.push(Constant.ITEM_SEQ_NO + Constant.ERR_REQUIRED);
                break;
            }

            // validate product name
            var productname = $targetRow.find('.product-name').val();

            if (productname.length === 0) {
                invalidMess.push(Constant.PRODUCT_NAME + Constant.ERR_REQUIRED);
                break;
            }
            else if (productname.length > Constant.MAX_PRODUCT_NAME) {
                invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.PRODUCT_NAME).replace('{1}', Constant.MAX_PRODUCT_NAME));
                break;
            }

            // validate unit price
            var unitprice = iseiQ.utility.convertMoneyToInt($targetRow.find('.unit-price').val(), true);
            if (unitprice.toString().length === 0) {
                invalidMess.push(Constant.UNIT_PRICE + Constant.ERR_REQUIRED);
                break;
            }
            else if (unitprice < Constant.MIN_UNIT_PRICE || unitprice > Constant.MAX_UNIT_PRICE) {
                invalidMess.push(Constant.UNIT_PRICE + Constant.ERR_RANGE_UNIT_PRICE);
                break;
            }

            // validate quantity
            var quantity = $targetRow.find('.quantity').val();
            if (quantity.length === 0) {
                invalidMess.push(Constant.QUANTITY + Constant.ERR_REQUIRED);
                break;
            }
            else if ((quantity < Constant.MIN_QUANTITY) || (quantity > Constant.MAX_QUANTITY)) {
                invalidMess.push(Constant.QUANTITY + Constant.ERR_RANGE_QUANTITY);
                break;
            }
            else if (quantity === 0) {
                invalidMess.push(Constant.QUANTITY + Constant.ERR_SPECIAL_QUANTITY);
                break;
            }

            // validate amount
            var paymentAmount = iseiQ.utility.convertMoneyToInt($targetRow.find('.sc-payment').val(), true);
            if (paymentAmount.toString().length > 0 && (paymentAmount < Constant.MIN_AMOUNT || paymentAmount > Constant.MAX_AMOUNT)) {
                invalidMess.push(Constant.AMOUNT + Constant.ERR_RANGE_AMOUNT);
                break;
            }

            if (estimateType == Constant.SES_SELECT) {
                var paymentMethodType = $targetRow.find('.payment_method').val();
                if (paymentMethodType != PaymentMethodType.Fixed)
                {
                    // validate unit time
                    var unittime = $targetRow.find('.unittime').val();
                    if (unittime.length == 0 || unittime.length > Constant.MAX_TIME_UNIT) {
                        invalidMess.push(Constant.UNIT_TIME + Constant.ERR_RANGE_UNITTIME);
                        break;
                    }
                    if ((unittime < Constant.UNIT_TIME_DATA_MIN) || (unittime > Constant.UNIT_TIME_DATA_MAX)) {
                        invalidMess.push(Constant.UNIT_TIME + Constant.ERR_RANGE_VALUE_UNITTIME);
                        break;
                    }
                }                

                // validate base time lower
                var basetimelower = $targetRow.find('.basetime_lower').val();
                if (basetimelower.length > Constant.MAX_BASE_TIME) {
                    invalidMess.push(Constant.BASE_TIME_LOWER + Constant.ERR_RANGE_BASETIMELOWER);
                    break;
                }

                // validate base time upper
                var basetimeupper = $targetRow.find('.basetime_upper').val();
                if (basetimeupper.length > Constant.MAX_BASE_TIME) {
                    invalidMess.push(Constant.BASE_TIME_UPPER + Constant.ERR_RANGE_BASETIMEUPPER);
                    break;
                }
                
                var errCompareBase = iseiQ.utility.checkCompareBasetime(basetimelower, basetimeupper, paymentMethodType);
                if (errCompareBase) {
                    invalidMess.push(errCompareBase);
                    break;
                }                

                // validate exceed unit                
                var exceedunit = iseiQ.utility.convertMoneyToInt($targetRow.find('.exceed_unit').val());
                if (exceedunit.length > Constant.MAX_EXCEED_UNIT) {
                    invalidMess.push(Constant.EXCEED_UNIT + Constant.ERR_RANGE_UNIT_PRICE);
                    break;
                }

                // validate deduction unit
                var deductionunit = iseiQ.utility.convertMoneyToInt($targetRow.find('.deduction_unit').val());
                if (deductionunit.length > Constant.MAX_EXCEED_UNIT) {
                    invalidMess.push(Constant.DEDUCTION_UNIT + Constant.ERR_RANGE_UNIT_PRICE);
                    break;
                }
            }
        }
    }

    //Validate estimation condition
    var estimatecondition = $('#ESTIMATE_CONDITION_ENTITY_EST_CONDITION').val().trim();
    if (estimatecondition.length > Constant.MAX_AREA_LENGTH) { // valid string length        
        invalidMess.push(Constant.ESTIMATE_CONDITION + Constant.ERR_MAX_CONDITION_MEMO);
    }

    //Validate memo
    var memo = $('#PROJECT_MEMO_ENTITY_MEMO').val().trim();
    if (memo.length > Constant.MAX_AREA_LENGTH) { // valid string length        
        invalidMess.push(Constant.MEMO + Constant.ERR_MAX_CONDITION_MEMO);
    }

    // Attache file name
    $('.txt-file-name').each(function () {
        var attFileName = $(this).val();
        if (attFileName.length > Constant.MAX_ATTACHED_FILE_NAME) {
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.ATTACHED_FILE_NAME).replace('{1}', Constant.MAX_ATTACHED_FILE_NAME));
            return;
        }
    });

    $('.txt-file-comment').each(function () {
        var attNote = $(this).val();
        if (attNote.length > Constant.MAX_NOTE) {
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.ATTACHED_MEMO).replace('{1}', Constant.MAX_NOTE));
            return;
        }
    });

    // check file length
    $('.tbFileList tr:not(.hide) .input-file').each(function () {
        if ($(this).prop('files').length == 0 || $(this).prop('files')[0].size == 0) {
            invalidMess.push(Constant.ERR_UPLOAD_EMPTY_FILE);
            return false;
        }
    });

    // Validate issue date
    var errIssueDate = null;
    var issueDate = $('#ISSUE_DATE').val().trim();
    if (issueDate.length === 0) { // required delivery date        
        errIssueDate = Constant.ISSUE_DATE + Constant.ERR_REQUIRED;
        invalidMess.push(errIssueDate);
    }
    else if (issueDate.length > 0) { // validate delivery date
        errIssueDate = iseiQ.utility.validDate(issueDate, Constant.DATE_FORMAT, Constant.ISSUE_DATE);
        if (errIssueDate != null) {
            invalidMess.push(errIssueDate);
        }
    }
    else if (errIssueDate == null && issueDate.length > Constant.MAX_DATE) { // valid string length
        invalidMess.push(Constant.ERR_FORMAT_DATE.replace('{0}', Constant.ISSUE_DATE));
    }

    //Validate target year month
    var errTargetYearMonth;
    var targetYMs = [];
    var closingDay = $('#CLOSING_DAY').val();
    $.each($('.targetYM'), function () {
        if ($(this).val() != '') {
            var targetYearMonth = $(this).val();
            if (errTargetYearMonth == null && targetYearMonth.length > 0) { // validate Target year month
                errTargetYearMonth = iseiQ.utility.validDate(targetYearMonth, Constant.YM_FORMAT, Constant.TARGET_YM);
            }
            else if (errTargetYearMonth == null && targetYearMonth.length > 7) { // validate string length
                errTargetYearMonth = Constant.ERR_FORMAT_YM.replace('{0}', Constant.TARGET_YM);
            }
            if (!iseiQ.utility.isValidTargetYM(startDate, endDate, targetYearMonth, closingDay)) {  // TargetYM is not out of range of project duration
                invalidMess.push(Constant.ERR_TARGETYM_INVALID);
            }
            targetYMs.push(targetYearMonth);
        }
        else {
            var $tbCenterTargetRow = $(this).parents('tr');
            var index = $tbCenterTargetRow.index();
            var $tbLeftTargetRow = $($('.tbLeft tbody tr')[index]);
            if ($tbLeftTargetRow.find('.base_time_type').val() == "1") {
                invalidMess.push(Constant.TARGET_YM + Constant.ERR_REQUIRED);
            }
	    }
    });
    if (errTargetYearMonth != null) {
        invalidMess.push(errTargetYearMonth);
    }
    var hasDups= !targetYMs.every(function (v, i) {
        return (targetYMs.indexOf(v) == i);
    });
    if (hasDups) {
        invalidMess.push(Constant.ERR_DUPLICATE_TARGET_YEAR_MONTH);
    }

    //Validate duplicate normal base time type product
    var displaySes = setDisplaySubProductByContractInputType();
    if (displaySes) {
        var numberNormalType = 0;
        $('.base_time_type').each(function() {
            if ($(this).val() == 0) {
                numberNormalType++;
            }
        });
        if (numberNormalType > 1) {
            invalidMess.push(Constant.ERR_DUPLICATE_NORMAL_BASE_TIME_TYPE_PRODUCT);
        }
    }
    return invalidMess;
}

// Event change issue date
$(document).off('#ISSUE_DATE');
$(document).on('change', '#ISSUE_DATE', function () {
    $(this).val(iseiQ.utility.autoCompleteYearMonthDay($(this).val()));
    if ("" !== $(this).val()) {
        $(this).parents(".datepicker-days").datepicker('update');
    }

    var issueDate = $(this).val();
    var errInvalid;
    if (issueDate.length === 0) {
        return;
    }

    errInvalid = iseiQ.utility.validDate(issueDate, Constant.DATE_FORMAT, Constant.ISSUE_DATE);

    if (errInvalid != null) {
        iseiQ.utility.showMessageModal(errInvalid, true);
        return;
    }

    if (issueDate.length > Constant.MAX_DATE) { // valid string length
        errInvalid = Constant.ISSUE_DATE + Constant.ERR_INCORRECT_DATE;
        iseiQ.utility.showMessageModal(errInvalid, true);
        return;
    }
});

function validatePeriodStart() {
    var startDate = $('#PRJ_PERIOD_START').val();
    var errInvalid;
    if (startDate.length === 0) {
        return;
    }

    errInvalid = iseiQ.utility.validDate(startDate, Constant.DATE_FORMAT, Constant.PERIOD_START);

    if (errInvalid != null) {
        iseiQ.utility.showMessageModal(errInvalid, true);
        return;
    }

    if (startDate.length > Constant.MAX_DATE) { // valid string length
        errInvalid = Constant.PERIOD_START + Constant.ERR_INCORRECT_DATE;
        iseiQ.utility.showMessageModal(errInvalid, true);
        return;
    }

    var endDate = $('#PRJ_PERIOD_END').val();
    var errInvalidEndDate = iseiQ.utility.validDate(endDate, Constant.DATE_FORMAT, Constant.PERIOD_END);

    if (errInvalidEndDate == null) {
        if (endDate.length > 0 && !iseiQ.utility.compareDate(startDate, endDate, Constant.DATE_FORMAT)) {
            iseiQ.utility.showMessageModal(Constant.PERIOD_END + Constant.ERR_COMPARE_DATE, true);
            return;
        }
    }
}

function validatePeriodEnd() {
    var endDate = $('#PRJ_PERIOD_END').val();
    var errInvalid;

    if (endDate.length === 0) {
        return;
    }
    errInvalid = iseiQ.utility.validDate(endDate, Constant.DATE_FORMAT, Constant.PERIOD_END);
    if (errInvalid != null) {
        iseiQ.utility.showMessageModal(errInvalid, true);
        return;
    }

    if (endDate.length > Constant.MAX_DATE) { // valid string length
        errInvalid = Constant.PERIOD_END + Constant.ERR_INCORRECT_DATE;
        iseiQ.utility.showMessageModal(errInvalid, true);
        return;
    }

    var startDate = $('#PRJ_PERIOD_START').val();
    var errInvalidStartDate = iseiQ.utility.validDate(startDate, Constant.DATE_FORMAT, Constant.PERIOD_START);

    if (errInvalidStartDate != null) {
        iseiQ.utility.showMessageModal(errInvalidStartDate, true);
        return;
    }

    if (startDate.length > Constant.MAX_DATE) { // valid string length
        var errInvalid = Constant.PERIOD_START + Constant.ERR_INCORRECT_DATE;
        iseiQ.utility.showMessageModal(errInvalid, true);
        return;
    }
    if (startDate.length > 0 && !iseiQ.utility.compareDate(startDate, endDate, Constant.DATE_FORMAT)) {
        iseiQ.utility.showMessageModal(Constant.PERIOD_END + Constant.ERR_COMPARE_DATE, true);
        return;
    }

    var deliveryDate = $('#DELIVERY_DATE').val();
    if (startDate.length > 0 && !iseiQ.utility.compareDate(startDate, deliveryDate, Constant.DATE_FORMAT)) {
        iseiQ.utility.showMessageModal(Constant.DELIVERY_DATE + Constant.ERR_COMPARE_DELIVERY_DATE, true);
        return;
    }
}

// Event change start date
$('.period-start').on('changeDate', function () {
    if ($('#PRJ_PERIOD_START').is(':focus')) {
        return;
    }
    validatePeriodStart();
    if ($('#PRJ_PERIOD_START').val().length > 0 && $('#DELIVERY_DATE').val().length > 0) {
        validateDeliveryDate();
    }
});

$(document).off('#PRJ_PERIOD_START');
$(document).on('change', '#PRJ_PERIOD_START', function () {
    $(this).val(iseiQ.utility.autoCompleteYearMonthDay($(this).val()));
    if ("" !== $(this).val()) {
        $(this).parents(".datepicker-days").datepicker('update');
    }
    validatePeriodStart();
    if ($('#PRJ_PERIOD_START').val().length > 0 && $('#DELIVERY_DATE').val().length > 0) {
        validateDeliveryDate();
    }
});

// Event change end date
$('.period-end').on('changeDate', function () {
    if ($('#PRJ_PERIOD_END').is(':focus')) {
        return;
    }
    validatePeriodEnd();
});

$(document).off('#PRJ_PERIOD_END');
$(document).on('change', '#PRJ_PERIOD_END', function () {
    $(this).val(iseiQ.utility.autoCompleteYearMonthDay($(this).val()));
    if ("" !== $(this).val()) {
        $(this).parents(".datepicker-days").datepicker('update');
    }
    validatePeriodEnd();
});

function validateDeliveryDate() {
    var deliveryDate = $('#DELIVERY_DATE').val();
    var errInvalid;

    if (deliveryDate.length === 0) {
        return;
    }

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

    var startDate = $('#PRJ_PERIOD_START').val();
    var errInvalidStartDate = iseiQ.utility.validDate(startDate, Constant.DATE_FORMAT, Constant.PERIOD_START);

    if (errInvalidStartDate != null) {
        iseiQ.utility.showMessageModal(errInvalidStartDate, true);
        return;
    }

    if (startDate.length > Constant.MAX_DATE) { // valid string length
        errInvalid = Constant.PERIOD_START + Constant.ERR_INCORRECT_DATE;
        iseiQ.utility.showMessageModal(errInvalid, true);
        return;
    }

    var endDate = $('#PRJ_PERIOD_END').val();
    var errInvalidEndDate = iseiQ.utility.validDate(endDate, Constant.DATE_FORMAT, Constant.PERIOD_END);

    if (errInvalidEndDate != null) {
        iseiQ.utility.showMessageModal(errInvalidEndDate, true);
        return;
    }

    if (endDate.length > Constant.MAX_DATE) { // valid string length
        errInvalid = Constant.PERIOD_END + Constant.ERR_INCORRECT_DATE;
        iseiQ.utility.showMessageModal(errInvalid, true);
        return;
    }
    if (startDate.length > 0 && !iseiQ.utility.compareDate(startDate, deliveryDate, Constant.DATE_FORMAT)) {
        iseiQ.utility.showMessageModal(Constant.DELIVERY_DATE + Constant.ERR_COMPARE_DELIVERY_DATE, true);
        return;
    }
    if (endDate.length > 0 && !iseiQ.utility.compareDate(deliveryDate, endDate, Constant.DATE_FORMAT)) {
        iseiQ.utility.showMessageModal(Constant.DELIVERY_DATE + Constant.ERR_COMPARE_END_DATE, true);
        return;
    }

    var acceptanceDate = $('#ACCEPTANCE_DATE').val();
    var errInvalidacceptDate = iseiQ.utility.validDate(acceptanceDate, Constant.DATE_FORMAT, Constant.ACCEPTANCE_DATE);

    if (errInvalidacceptDate != null) {
        iseiQ.utility.showMessageModal(errInvalidacceptDate, true);
        return;
    }
    if (acceptanceDate.length > 0 && !iseiQ.utility.compareDate(deliveryDate, acceptanceDate, Constant.DATE_FORMAT)) {
        iseiQ.utility.showMessageModal(Constant.ACCEPTANCE_DATE + Constant.ERR_COMPARE_ACCEPTANCE_DATE, true);
        return;
    }
}

// Event change delivery date
$('.delivery-date').on('changeDate', function () {
    if ($('#DELIVERY_DATE').is(':focus')) {
        return;
    }
    validateDeliveryDate();
});

$(document).off('#DELIVERY_DATE');
$(document).on('change', '#DELIVERY_DATE', function () {
    $(this).val(iseiQ.utility.autoCompleteYearMonthDay($(this).val()));
    if ("" !== $(this).val()) {
        $(this).parents(".datepicker-days").datepicker('update');
    }
    validateDeliveryDate();
});

function validateAcceptanceDate() {
    var acceptanceDate = $('#ACCEPTANCE_DATE').val();
    var errInvalid;

    if (acceptanceDate.length === 0) {
        return;
    }

    var startDate = $('#PRJ_PERIOD_START').val();
    var errInvalidStartDate = iseiQ.utility.validDate(startDate, Constant.DATE_FORMAT, Constant.PERIOD_START);

    if (errInvalidStartDate != null) {
        iseiQ.utility.showMessageModal(errInvalidStartDate, true);
        return;
    }

    if (startDate.length > Constant.MAX_DATE) { // valid string length
        errInvalid = Constant.PERIOD_START + Constant.ERR_INCORRECT_DATE;
        iseiQ.utility.showMessageModal(errInvalid, true);
        return;
    }
    if (startDate.length > 0 && !iseiQ.utility.compareDate(startDate, acceptanceDate, Constant.DATE_FORMAT)) {
        iseiQ.utility.showMessageModal(Constant.ACCEPTANCE_DATE + Constant.ERR_COMPARE_DATE, true);
        return;
    }
    
    var endDate = $('#PRJ_PERIOD_END').val();

    var errInvalidEndDate = iseiQ.utility.validDate(endDate, Constant.DATE_FORMAT, Constant.PERIOD_END);

    if (errInvalidEndDate != null) {
        iseiQ.utility.showMessageModal(errInvalidEndDate, true);
        return;
    }

    if (endDate.length > Constant.MAX_DATE) { // valid string length
        errInvalid = Constant.PERIOD_END + Constant.ERR_INCORRECT_DATE;
        iseiQ.utility.showMessageModal(errInvalid, true);
        return;
    }
    if (endDate.length > 0 && !iseiQ.utility.compareDate(acceptanceDate, endDate, Constant.DATE_FORMAT)) {
        iseiQ.utility.showMessageModal(Constant.ACCEPTANCE_DATE + Constant.ERR_COMPARE_END_DATE, true);
        return;
    }

    errInvalid = iseiQ.utility.validDate(acceptanceDate, Constant.DATE_FORMAT, Constant.ACCEPTANCE_DATE);

    if (errInvalid != null) {
        iseiQ.utility.showMessageModal(errInvalid, true);
        return;
    }

    if (acceptanceDate.length > Constant.MAX_DATE) { // valid string length
        errInvalid = Constant.ACCEPTANCE_DATE + Constant.ERR_INCORRECT_DATE;
        iseiQ.utility.showMessageModal(errInvalid, true);
        return;
    }

    var deliveryDate = $('#DELIVERY_DATE').val();
    var errInvalidDeliveryDate = iseiQ.utility.validDate(deliveryDate, Constant.DATE_FORMAT, Constant.DELIVERY_DATE);

    if (errInvalidDeliveryDate != null) {
        iseiQ.utility.showMessageModal(errInvalidDeliveryDate, true);
        return;
    }

    if (deliveryDate.length > 0 && !iseiQ.utility.compareDate(deliveryDate, acceptanceDate, Constant.DATE_FORMAT)) {
        iseiQ.utility.showMessageModal(Constant.ACCEPTANCE_DATE + Constant.ERR_COMPARE_ACCEPTANCE_DATE, true);
        return;
    }
}

// Event change acceptance date
$('.acceptance-date').on('changeDate', function () {
    if ($('#ACCEPTANCE_DATE').is(':focus')) {
        return;
    }
    validateAcceptanceDate();
});

$(document).off('#ACCEPTANCE_DATE');
$(document).on('change', '#ACCEPTANCE_DATE', function () {
    $(this).val(iseiQ.utility.autoCompleteYearMonthDay($(this).val()));
    if ("" !== $(this).val()) {
        $(this).parents(".datepicker-days").datepicker('update');
    }
    validateAcceptanceDate();
});

// Event change unit price
$(document).off('.unit-price');
$(document).on('blur', '.unit-price', function () {
    var control = this;
    var $targetRow = $(this).parents('.subproduct-content');
    var rounding = $('#ROUNDING_TYPE').val();
    var quantity = $targetRow.find('.quantity').val();

    setTimeout(function () {
        var price = control.value;

        if (price.length > 0 && quantity.length > 0) {
            var unitprice = iseiQ.utility.convertMoneyToInt(price, true);
            var amount = iseiQ.utility.roundByType(rounding, unitprice * quantity);
            var stramount = iseiQ.utility.convertIntToMoney(amount);

            $targetRow.find('.sc-payment').val(stramount);
        }

        calTotalestimate();
    }, 100);
});

// Event change quantity
$(document).off('.quantity');
$(document).on('blur', '.quantity', function () {
    var control = this;

    setTimeout(function () {
        var rounding = $('#ROUNDING_TYPE').val();
        var $targetRow = $(control).parents('.subproduct-content');
        var quantity = control.value;
        var price = $targetRow.find('.unit-price').val();

        if (price.length > 0 && quantity.length > 0) {
            var unitprice = iseiQ.utility.convertMoneyToInt(price, true);
            var amount = iseiQ.utility.roundByType(rounding, unitprice * quantity);
            var stramount = iseiQ.utility.convertIntToMoney(amount);

            $targetRow.find('.sc-payment').val(stramount);
        }

        calTotalestimate();
    }, 100);
});

// Event change tax-rate
$(document).off('.tax-rate');
$(document).on('focusout', '.tax-rate', function () {
    calTotalestimate();
});

// Action add more subproduct
$(document).off('#btnAddSubProduct');
$(document).on('click', '#btnAddSubProduct', function () {
    var strTbCenterLastRow = '.subproduct-content:last';
    var $tbCenterLastRow = $(strTbCenterLastRow);
    var $tbRightLastRow = $('.tbRight tbody tr:last');
    var $tbLeftLastRow = $('.tbLeft tbody tr:last');

    $tbCenterLastRow.after($tbCenterLastRow.prop('outerHTML'));
    $tbRightLastRow.after($tbRightLastRow.prop('outerHTML'));
    $tbLeftLastRow.after($tbLeftLastRow.prop('outerHTML'));

    resetValueFirstSubproduct($(strTbCenterLastRow));
    resetArrSubproduct();
    resetPaymenMethod();
    displayTargetYearMonth();
    //Init datepickers
    iseiQ.utility.InitDatepickerMonths();
});

// Action delete a subproduct
$(document).off('.btnDeleteSubproduct');
$(document).on('click', '.btnDeleteSubproduct', function () {
    var $tbRightTargetRow = $(this).parents('tr');
    var index = $tbRightTargetRow.index();
    var $tbCenterTargetRow = $($('.tbCenter tbody tr')[index]);

    if ($('.subproduct-content').length > 1) {
        $tbRightTargetRow.remove();
        $($('.tbLeft tbody tr')[index]).remove();
        $tbCenterTargetRow.remove();

        if ($('.subproduct-content').length > 1) {
            $('.btnDeleteSubproduct').removeClass('invisible');
        }
        else {
            $('.btnDeleteSubproduct').addClass('invisible');
        }
    }
    else {
        resetValueFirstSubproduct($tbCenterTargetRow);
    }

    resetArrSubproduct();
    resetPaymenMethod();
    calTotalestimate();
    displayTargetYearMonth();
});

function resetValueFirstSubproduct($targetRow) {
    $targetRow.find('input:text').val('').attr('title', '').attr('value', '');
    $targetRow.find('input:hidden').val('').attr('value', '');
    $targetRow.find('.product-segno').val($('#PRODUCT_INDEX').val());

    $targetRow.find('.item-segno, .unit-price, .sc-payment').val(0);
    $targetRow.find('.quantity').val(1);
    $targetRow.find('.unittype').val('式');
    $targetRow.find('.unittime').val(Constant.DEFAULT_UNIT_TIME);
    $targetRow.find('.taxable').prop('selectedIndex', 0);

    if (setDisplaySubProductByContractInputType()) {
        $targetRow.find('.calc_unit, .payment_method').prop('selectedIndex', 0);
        $targetRow.find('.basetime_lower, .basetime_upper, .exceed_unit, .deduction_unit').val(0);
        $targetRow.find('.unittime').val(Constant.DEFAULT_UNIT_TIME);
        $($('.tbLeft tbody tr')[$targetRow.index()]).find('.base_time_type').prop('selectedIndex', 0);
    }

    if ($('.subproduct-content').length > 1) {
        $('.btnDeleteSubproduct').removeClass('invisible');
    }
    else {
        $('.btnDeleteSubproduct').addClass('invisible');
    }
}

// Reset index of element in subproduct array
function resetArrSubproduct() {
    var productItem = $("#PRODUCT_INDEX").val();
    var $tbCenter = $('.tbCenter .subproduct-content');
    var $tbLeft = $('.tbLeft tbody tr');
    var displaySes = setDisplaySubProductByContractInputType();

    var scId = 'PRODUCT_INFO[{0}].EST_SEQ_NO';
    var scTargetYm = 'PRODUCT_INFO[{0}].TARGET_YM';
    var scTargetYearMonth = 'PRODUCT_INFO[{0}].TARGET_YEAR_MONTH';
    var scOrder = 'PRODUCT_INFO[{0}].DSP_ORDER';
    var scProductSegno = 'PRODUCT_INFO[{0}].PRODUCT_SEQ_NO';
    var scItemSegno = 'PRODUCT_INFO[{0}].ITEM_SEQ_NO';
    var scProductName = 'PRODUCT_INFO[{0}].NOMEN';
    var scUnitPrice = 'PRODUCT_INFO[{0}].UNIT_PRICE';
    var scQty = 'PRODUCT_INFO[{0}].QTY';
    var scUnitype = 'PRODUCT_INFO[{0}].UNIT_TYPE';
    var scAmount = 'PRODUCT_INFO[{0}].AMOUNT';
    var scTax = 'PRODUCT_INFO[{0}].TAX_NOTAX_INDEX';
    var scStandardTime = 'PRODUCT_INFO[{0}].PAYMENT_METHOD_TYPE_INDEX';
    var scBaseTimeLower = 'PRODUCT_INFO[{0}].BASE_TIME_LOWER';
    var scBaseTimeUpper = 'PRODUCT_INFO[{0}].BASE_TIME_UPPER';
    var scDeductionUnit = 'PRODUCT_INFO[{0}].EXCEED_UNIT_PRICE';
    var scExceedUnit = 'PRODUCT_INFO[{0}].DEDUCTION_UNIT_PRICE';
    var scUnitTime = 'PRODUCT_INFO[{0}].UNIT_TIME';
    var scUnitTime_dataval = Constant.UNIT_TIME_DATA_VAL;
    var scUnitTime_datavalmax = Constant.UNIT_TIME_DATA_MAX;
    var scUnitTime_datavalmin = Constant.UNIT_TIME_DATA_MIN;
    var scUnitTime_datavalrange = Constant.UNIT_TIME + Constant.ERR_RANGE_VALUE_UNITTIME;
    var scBaseTimeType = 'PRODUCT_INFO[{0}].BASE_TIME_TYPE_INDEX';
    var scSplitUnit = 'PRODUCT_INFO[{0}].OUTER_TIME_CALC_UNIT_INDEX';
    
    for (var i = 0; i < $tbCenter.length; i++) {
        var $dataCenter = $($tbCenter[i]);
        var index = $dataCenter.index();
        var $dataLeft = $($tbLeft[index]);

        $dataCenter.find('.sp-id').attr('name', scId.replace('{0}', i));
        $dataCenter.find('.dsp-order').attr('name', scOrder.replace('{0}', i));
        $dataCenter.find('.product-segno').attr('name', scProductSegno.replace('{0}', i)).val(productItem);
        $dataCenter.find('.item-segno').attr('name', scItemSegno.replace('{0}', i)).val(0);
        $dataCenter.find('.product-name').attr('name', scProductName.replace('{0}', i));
        $dataCenter.find('.unit-price').attr('name', scUnitPrice.replace('{0}', i));
        $dataCenter.find('.quantity').attr('name', scQty.replace('{0}', i));
        $dataCenter.find('.unittype').attr('name', scUnitype.replace('{0}', i));        
        $dataCenter.find('.sc-payment').attr('name', scAmount.replace('{0}', i));
        $dataCenter.find('.taxable').attr('name', scTax.replace('{0}', i));
        $dataCenter.find('.target-ym').attr('name', scTargetYm.replace('{0}', i));

        if (displaySes) {
            $dataLeft.find('.base_time_type').attr('name', scBaseTimeType.replace('{0}', i));            
            $dataCenter.find('.targetYM').attr('name', scTargetYearMonth.replace('{0}', i));
            $dataCenter.find('.payment_method').attr('name', scStandardTime.replace('{0}', i));
            $dataCenter.find('.basetime_lower').attr('name', scBaseTimeLower.replace('{0}', i));
            $dataCenter.find('.basetime_upper').attr('name', scBaseTimeUpper.replace('{0}', i));
            $dataCenter.find('.exceed_unit').attr('name', scDeductionUnit.replace('{0}', i));
            $dataCenter.find('.deduction_unit').attr('name', scExceedUnit.replace('{0}', i));
            $dataCenter.find('.unittype').attr('readonly', 'readonly');
            $dataCenter.find('.unittime').attr('name', scUnitTime.replace('{0}', i));
            $dataCenter.find('.unittime').attr('data-val', scUnitTime_dataval);
            $dataCenter.find('.unittime').attr('data-val-range', scUnitTime_datavalrange);
            $dataCenter.find('.unittime').attr('data-val-range-max', scUnitTime_datavalmax);
            $dataCenter.find('.unittime').attr('data-val-range-min', scUnitTime_datavalmin);
            $dataCenter.find('.calc_unit').attr('name', scSplitUnit.replace('{0}', i));
        }
        else
        {
            $dataCenter.find('.unittype').removeAttr('readonly');
        }
    }

    if (displaySes == false) {
        $('.base_time_type, .payment_method, .basetime_lower, .basetime_upper, .calc_unit, .exceed_unit, .deduction_unit, .unittime,.targetYM').removeAttr('name');
        $('.unit_time_type').text('円');
    }
}

// Caculate total estimation
function calTotalestimate() {
    var totalAmount = 0;
    var totalAmountOnTax = 0;
    var totalTaxableAmount = 0;
    var rounding = $('#ROUNDING_TYPE').val();

    $('.subproduct-content').each(function () {
        var value = $(this).find('.sc-payment').val();
        var isTaxable = $(this).find('.taxable').val();

        if (value.length > 0) {
            var amount = iseiQ.utility.convertMoneyToInt(value,true);
            totalAmount += amount;
            if (isTaxable == 1) {
                totalTaxableAmount += amount;
            }
        }
    });

    var strTotalAmount = iseiQ.utility.convertIntToMoney(totalAmount);
    $('#EST_AMOUNT_TOTAL').first().val(strTotalAmount);

    var strTaxableAmount = iseiQ.utility.convertIntToMoney(totalTaxableAmount);
    $('#EST_TAXABLE_AMOUNT_TOTAL').first().val(strTaxableAmount);

    var taxRate = $.isNumeric($('#TAX_RATE').val()) & $('#TAX_RATE').val().indexOf('-') == -1 ? $('#TAX_RATE').val() : 0;
    totalAmountOnTax = iseiQ.utility.roundByType(rounding, totalTaxableAmount * taxRate / 100);

    var strTotalAmountOnTax = iseiQ.utility.convertIntToMoney(totalAmountOnTax);
    $('#TAX').first().val(strTotalAmountOnTax);

    var consumptionTax = $('#TAX').val();
    totalAmount = totalAmount + iseiQ.utility.convertMoneyToInt(consumptionTax, true);

    var strTotalAmountAndTax = iseiQ.utility.convertIntToMoney(totalAmount);
    $('#EST_AMOUNT_TOTAL_ON_TAX').first().val(strTotalAmountAndTax);
}

// Event change tax/not tax
$(document).off('.taxable');
$(document).on('change', '.taxable', function () {
    calTotalestimate();
});

// function to visible object follow payment method
function resetPaymenMethod() {
    if (setDisplaySubProductByContractInputType()) {
        $('.subproduct-content').each(function () {
            var timeIndex = $(this).find('.payment_method').val();
            if (timeIndex == TimeIndex.CONTRACT_PERIOD_START) {
                $(this).find('.basetime_lower, .basetime_upper, .exceed_unit, .deduction_unit, .unit_exceed, .unit_deduction, .calc_unit, .btnCalculation_SES, .lblBaseTimeLowerUnit, .lblBaseTimeUpperUnit, .unittime, .unit').prop('disabled',false);
                $(this).find('.basetime_lower, .basetime_upper, .exceed_unit, .deduction_unit, .unit_exceed, .unit_deduction, .calc_unit, .btnCalculation_SES, .lblBaseTimeLowerUnit, .lblBaseTimeUpperUnit, .unittime, .unit').css('opacity', '1');
                $(this).find('.unit_time_type').text('円/月');
                $(this).find('.unittype').val('月');
            }
            else {
                $(this).find('.basetime_lower, .basetime_upper, .exceed_unit, .deduction_unit, .unit_exceed, .unit_deduction, .calc_unit, .btnCalculation_SES, .lblBaseTimeLowerUnit, .lblBaseTimeUpperUnit, .unittime, .unit').prop('disabled', true);
                $(this).find('.basetime_lower, .basetime_upper, .exceed_unit, .deduction_unit, .unit_exceed, .unit_deduction, .calc_unit, .btnCalculation_SES, .lblBaseTimeLowerUnit, .lblBaseTimeUpperUnit, .unittime, .unit').css('opacity', '0');
                $(this).find('.unit_time_type').text('円/月');
                $(this).find('.unittype').val('月');

                if (timeIndex == TimeIndex.DELIVERY_DATE) {
                    $(this).find('.unittime, .unit').prop('disabled', false);
                    $(this).find('.unittime, .unit').css('opacity', '1');
                    $(this).find('.unit_time_type').text('円/h');
                    $(this).find('.unittype').val('h');
                }
            }
        });
    }
    else {
        $('.subproduct-list .unit_time_type').text('円');
    }
}

// Event change Standard time classifcation
$(document).off('.payment_method');
$(document).on('change', '.payment_method', function () {
    resetPaymenMethod();
    var $targetRow = $(this).parents('tr');
    $targetRow.find('.unittime').val(Constant.DEFAULT_UNIT_TIME);
    if ($(this).TIME_INDEX == TimeIndex.CONTRACT_PERIOD_END || $(this).TIME_INDEX == TimeIndex.DELIVERY_DATE) {
        $targetRow.find('.basetime_lower, .basetime_upper, .exceed_unit, .deduction_unit').val('0');
        $targetRow.find('.calc_unit').val('1');
    }
});

// Action calculation
$(document).off('.btnCalculation_SES');
$(document).on('click', '.btnCalculation_SES', function () {
    var $targetRow = $(this).parents('.subproduct-content');

    var calculationType = $targetRow.find('.calc_unit').val();
    var unitPrice = iseiQ.utility.convertMoneyToInt($targetRow.find('.unit-price').val(), true);
    var valueBaseTimeLower = $targetRow.find('.basetime_lower').val();
    var valueBaseTimeUpper = $targetRow.find('.basetime_upper').val();
    var baseTimeLower = valueBaseTimeLower.length > 0 ? parseFloat(valueBaseTimeLower) : 0;
    var baseTimeUpper = valueBaseTimeUpper.length > 0 ? parseFloat(valueBaseTimeUpper) : 0;
    var average = (baseTimeLower + baseTimeUpper) / 2;

    switch (calculationType) {
        case OuterTimeCalcUnitType.Normal:
            var exceedUnit = baseTimeUpper != 0 ? iseiQ.utility.convertIntToMoney(Math.floor(unitPrice / baseTimeUpper)) : 0;
            var deductionUnit = baseTimeLower != 0 ? iseiQ.utility.convertIntToMoney(Math.floor(unitPrice / baseTimeLower)) : 0;

            $targetRow.find('.exceed_unit').val(exceedUnit);
            $targetRow.find('.deduction_unit').val(deductionUnit);
            break;
        case OuterTimeCalcUnitType.Upper:
            var unit = baseTimeUpper != 0 ? iseiQ.utility.convertIntToMoney(Math.floor(unitPrice / baseTimeUpper)) : 0;
            $targetRow.find('.exceed_unit, .deduction_unit').val(unit);
            break;
        case OuterTimeCalcUnitType.Middle:
            var unit = average != 0 ? iseiQ.utility.convertIntToMoney(Math.floor(unitPrice / average)) : 0;
            $targetRow.find('.exceed_unit, .deduction_unit').val(unit);
            break;
        case OuterTimeCalcUnitType.Lower:
            var unit = baseTimeLower != 0 ? iseiQ.utility.convertIntToMoney(Math.floor(unitPrice / baseTimeLower)) : 0;
            $targetRow.find('.exceed_unit, .deduction_unit').val(unit);
            break;
        case OuterTimeCalcUnitType.Other:
            $targetRow.find('.exceed_unit').val("");
            $targetRow.find('.deduction_unit').val("");
            break;
        default:
            break;
    }
});

function displayEstimateDetail() {
    resetPaymenMethod();

    var displaySes = setDisplaySubProductByContractInputType();

    if (displaySes) {
        $('.div-subproduct, .ses').show();
        $('.unittype').attr('readonly', 'readonly');
        $('.sesinvisible').hide();
        $('.ses_special_payment').show();
        displayTargetYearMonth();
    }
    else if (displaySes == null) {
        $('.div-subproduct').hide();
    }
    else {
        $('.div-subproduct').show();
        $('.unittype').removeAttr('readonly');
        $('.ses').hide();
        $('.sesinvisible').show();
        $('.ses_special_payment').hide();
    }
}

function displayTargetYearMonth() {
    var isDisplay = null;
    $('.base_time_type').each(function () {
        var idOfSelect = $(this).attr("name");
        if (idOfSelect) {
            var indexOfSelect = idOfSelect.replace('PRODUCT_INFO[', '').replace('].BASE_TIME_TYPE_INDEX', '');
            if ($(this).val() === "1") {
                $('.span-target-ym').eq(indexOfSelect).show();
                isDisplay = true;
            }
            else {
                $('.span-target-ym').eq(indexOfSelect).hide();
                $('.targetYM').eq(indexOfSelect).val('');
            }
        }
    });
    if (isDisplay) {
        $('.ses_special_payment').show();
    }
    else {
        $('.ses_special_payment').hide();
    }
}

$(document).off('.span-target-ym');
$(document).on('changeDate', '.span-target-ym', function () {
    if ($(this).children('.targetYM').is(':focus')) {
        return;
    }
    OnChangeTargetYearMonth($(this).children('.targetYM'));
});

$(document).off('.targetYM');
$(document).on('change', '.targetYM', function () {
    $(this).val(iseiQ.utility.autoCompleteYearMonth($(this).val()));
    if ("" !== $(this).val()) {
        $(this).parents(".datepicker-months").datepicker('update');
    }
    OnChangeTargetYearMonth(this);
});

function OnChangeTargetYearMonth(control) {
    var targetYM = $(control).val();
    if (targetYM) {
        errInvalid = iseiQ.utility.validDate(targetYM, Constant.YM_FORMAT, Constant.TARGET_YM);

        if (errInvalid != null) {
            iseiQ.utility.showMessageModal(errInvalid, true);
            return false;
        }
    }
    return true;
}


function ChangeEstimateDetail() {
    displayEstimateDetail();

    //if ($('.subproduct-content').length > 1) {
    //    $('.tbLeft tbody tr:not(:first), .tbCenter tbody tr:not(:first), .tbRight tbody tr:not(:first)').remove();
    //}

    //resetValueFirstSubproduct($('.subproduct-content:first'));
    resetArrSubproduct();    
    calTotalestimate();
}

function ApplyCustomer(res) {
    if (res === undefined || res === null) {
        /// Debug only
        console.log("Could not get customer.");
    }
    else if ($("#BP_SEQ_NO").val() != res[0].BP_SEQ_NO || $("#BP_NAME_DISP").val() != res[0].BP_NAME_DISP) {
        // Display selected customer
        $("#BP_STAFF_NAME").val("");
        $("#BP_STAFF_SEQ_NO").val("");
        $("#BP_NAME_DISP").val(iseiQ.utility.htmlDecode(res[0].BP_NAME_DISP));
        $("#BP_SEQ_NO").val(iseiQ.utility.htmlDecode(res[0].BP_SEQ_NO));
        $("#btnSelectPIC").removeAttr("disabled");
        inputChangedDuplicateEstimate = true;
        inputChanged = true;
    }
}

function ApplyPartnerStaff(res) {
    if (res === undefined || res === null) {
        /// Debug only
        console.log("Could not get BPStaff.");
    }
    else {
        $("#BP_NAME_DISP").val(iseiQ.utility.htmlDecode(res[0].BP_NAME_DISP));
        $("#BP_SEQ_NO").val(iseiQ.utility.htmlDecode(res[0].BP_SEQ_NO));
        $("#btnSelectPIC").removeAttr("disabled");
        $("#BP_STAFF_NAME").val(iseiQ.utility.htmlDecode(res[0].BP_STAFF_NAME));
        $("#BP_STAFF_SEQ_NO").val(iseiQ.utility.htmlDecode(res[0].BP_STAFF_SEQ_NO));
        inputChangedDuplicateEstimate = true;
        inputChanged = true;
    }
}

$(document).off('.btnSearchSubProduct');
$(document).on('click', '.btnSearchSubProduct', function () {
    $('.btnSearchSubProduct').removeClass('clicked');
    $(this).addClass('clicked');
    $(this).blur();

    CallProductList();
});

// Handle File
function resetFileElementName() {
    $('.tbFileList tbody tr:not(.hide, .file-attached)').each(function (index, element) {
        var prefix = 'FILE_LIST[' + index + '].';

        $(element).find('.txt-file-name').attr('name', prefix + 'PRJ_ATTACHED_FILE_NAME');
        $(element).find('.txt-file-comment').attr('name', prefix + 'PRJ_ATTACHED_NOTE');
        $(element).find('.input-file').attr('name', 'files');
    });
}

$('#selectFile').click(function () {
    var index = 0;

    if ($('.tbFileList tbody tr.hide').length > 0) {
        index = $('.tbFileList tbody tr.hide:last').data('file-index');
    }
    else {
        index = $('.tbFileList tbody tr:not(.hide)').length + 1;

        var html = '<tr class="hide" data-file-index="' + index + '">'
            + '<td>'
            + ' <input type="file" class="input-file">'
            + ' <input class="txt-no-border short-text text-overflow txt-file-name  no-padding-left" maxlength="50" type="text" readonly="readonly">'
            + '</td>'
            + '<td class="cell_center">'
            + ' <input class="txt-file-comment" type="text" maxlength="255">'
            + '</td>'
            + '<td>'
            + ' <button type="button" class="btnDeleteNewFile btn btn-red"><i class="btn-icon btn-delete-left">削除</i></button>'
            + '</td></tr>';

        if (index > 1) {
            $('.tbFileList tbody tr:last').after(html);
        }
        else {
            $('.tbFileList tbody').append(html);
        }
    }

    $(this).data('file-index', index);
    $('.tbFileList tr[data-file-index="' + index + '"]').find('.input-file').click();
});

// Action add more attached line
$('#btnAddLineAttached').click(function () {
    var index = $('#selectFile').data('file-index');

    if ($('.tbFileList tr[data-file-index="' + index + '"]').find('.input-file').prop('files').length > 0 && $('#fileInput').val().length > 0) {
        $('.tbFileList tr[data-file-index="' + index + '"]').removeClass('hide');
        resetFileElementName();
    }

    $('#fileInput').attr('title', '').val('');
});

$(document).off('.btnDeleteNewFile');
$(document).on('click', '.btnDeleteNewFile', function () {
    $(this).parents('tr').remove();
    resetFileElementName();
});

// Action to display file name
$(document).off('.input-file');
$(document).on('change', '.input-file', function () {
    var file = $(this).val();

    if (file.length > 0) {
        var filename = file.split('\\').pop();
        var $divContent = $(this).parent();
        $divContent.find('.txt-file-name').attr('title', filename).val(filename);
        $('#fileInput').attr('title', filename).val(filename);
    }
    else {
        $('#fileInput').attr('title', '').val('');
    }
});

$('#PRJ_NAME').on('blur', function () {
    var $dspName = $('#PRJ_DSP_NAME');
    var $descName = $('#PRJ_DESC_NAME');

    if ($dspName.val().trim().length == 0) {
        $dspName.val(this.value);
    }

    if ($descName.val().trim().length == 0) {
        $descName.val(this.value);
    }
});

function setDisplaySubProductByContractInputType() {
    var displaySES = null;

    switch ($('#CONTRACT_INPUT_TYPE').val()) {
        case Constant.OUTSOURCING:
        case Constant.STAFF:
        case Constant.CONTRACT:
        case Constant.DELEGATION:
        case Constant.GENERAL_DISPATCH:
        case Constant.SPECIFIC_DISPATCH:
            displaySES = true;
            break;
        case Constant.TRUSTEE:
        case Constant.PRODUCT_SALE:
        case Constant.PRODUCT_SALE_MONTH:
        case Constant.MAINTAINANCE:
            displaySES = false;
            break;
        default:
            break;
    }

    return displaySES;
}


//Fix behavior of web browser when press Tab key
function findNextElement(control) {
    var $nextTd = $(control).closest('td').next('td');
    while ($nextTd.length > 0 && ($nextTd.find(':input').eq(0).offset() == undefined
        || $nextTd.find(':input').eq(0).offset().left == 0 || $nextTd.find(':input').eq(0).css('opacity') == '0')) {
        $nextTd = $nextTd.next('td');
    }

    var $nextElement = $nextTd.find(':input').eq(0);
    return $nextElement;
}

function findPrevElement(control) {
    var $prevTd = $(control).closest('td').prev('td');
    while ($prevTd.length > 0 && ($prevTd.find(':input').eq(0).offset() == undefined
        || $prevTd.find(':input').eq(0).offset().left == 0 || $prevTd.find(':input').eq(0).css('opacity') == '0')) {
        $prevTd = $prevTd.prev('td');
    }

    var $prevElement = $prevTd.find(':input').eq(0);
    return $prevElement;
}

function findPrevElementInPrevRow(control) {
    var prevRow = $(control).closest('tr').prev('tr');
    var $prevTd = $(prevRow).find('td:last');
    while ($prevTd.length > 0 && ($prevTd.find(':input').eq(0).offset() == undefined
        || $prevTd.find(':input').eq(0).offset().left == 0|| $prevTd.find(':input').eq(0).css('opacity') == '0')) {
        $prevTd = $prevTd.prev('td');
    }

    var $prevElement = $prevTd.find(':input').eq(0);
    return $prevElement;
}

$('.col-center').off('input:text, textarea, select, button');
$('.col-center').on('keydown', 'input:text, textarea, select, button', function (e) {
    var keyCode = e.keyCode || e.which;
    if (keyCode == 9 && !e.shiftKey) {
        if ($(this).next(':input, button').length == 0 && findNextElement(this).length > 0) {
            var space = findNextElement(this).offset().left - $('.col-center').width() + $('.col-center').scrollLeft() - $('.col-center').offset().left + findNextElement(this).parents('td').width();

            if (space > 0 && space > $('.col-center').scrollLeft()) {
                $(".col-center").animate({
                    scrollLeft: space
                }, 50);
            }
        }
        if (findNextElement(this).length == 0 && $(this).closest('tr').next('tr').length > 0) {
            $(".col-center").animate({
                scrollLeft: 0
            }, 50);
        }
    }

    if (keyCode == 9 && e.shiftKey) {
        if ($(this).prev(':input, button').length == 0 && findPrevElement(this).length > 0) {
            var space = $('.col-center').scrollLeft() + findPrevElement(this).parents('td').offset().left - $('.col-center').offset().left;
            
            if (space > 0 && space < $('.col-center').scrollLeft()) {
                $(".col-center").animate({
                    scrollLeft: space
                }, 50);
            }
        }
        if ($(this).prev(':input, button').length == 0 &&
            ($(this).parents('td').index() == 0 ||
            ($(this).parents('td').index() == 1 && $(this).parents('td').prev('td').find('.span-target-ym').css("display") == "none"))) {
            if (findPrevElementInPrevRow(this).length > 0) {
                var space = $('.col-center').scrollLeft() + findPrevElementInPrevRow(this).offset().left - $('.col-center').width() - $('.col-center').offset().left + findPrevElementInPrevRow(this).parents('td').width();
                if (space > 0) {
                    $(".col-center").animate({
                        scrollLeft: space
                    }, 50);
                }
            }
        }
    }
});

