/*!
 * File: iseiQ.ContractContract.js
 * Company: i-Enter Asia
 * Copyright © 2015 i-Enter Asia. All rights reserved.
 * Project: i-seiQ2
 * Author: HoangLS
 * Created date: 2015/11/18
 */

var ContractContract = (function () {
    return {
        ValidateData: ValidateData,

        DeleteCoad: DeleteCoad,
        DeleteCopd: DeleteCopd,
        AddCoad: AddCoad,
        AddCopd: AddCopd,
        ResetInitValueCopd: ResetInitValueCopd,
        ResetInitValueCoad: ResetInitValueCoad,
        
        OnChangeCoadPaymentMethodType: OnChangeCoadPaymentMethodType,
        OnChangeCopdPaymentMethodType: OnChangeCopdPaymentMethodType,

        CalcExceedDedutionUnitPrice: CalcExceedDedutionUnitPrice,
        CalcAmountCoad: CalcAmountCoad,
        OnClickBtnCalcCoad: OnClickBtnCalcCoad,
        CalcAmountCopd: CalcAmountCopd,
        OnClickBtnCalcCopd: OnClickBtnCalcCopd,

        OnClickBtnCalcOA: OnClickBtnCalcOA,
        OnClickBtnCalcOP: OnClickBtnCalcOP,

        SelectCustomer: SelectCustomer,
        SelectCustomerPIC: SelectCustomerPIC,
        SelectSupplier: SelectSupplier,
        SelectSupplierPIC: SelectSupplierPIC,
        ApplyCustomer: ApplyCustomer,
        ApplyPartnerStaff: ApplyPartnerStaff

    };

    //////////////////////////////////////////////////////////////////
    // public function
    //Validate data before submit
    function ValidateData() {
        var invalidMess = [];

        // Project Name
        var prjName = $('#CONTRACT_PRJ_NAME').val();
        if (prjName.length === 0) // required Project name
            invalidMess.push(Constant.PROJECT_NAME + Constant.ERR_REQUIRED);
        else if (prjName.length > Constant.MAX_PRJ_NAME) // valid string length
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.PROJECT_NAME).replace('{1}', Constant.MAX_PRJ_NAME));

        // Working content
        var wrkContent = $('#CONTRACT_WORKING_CONTENT').val();
        if (wrkContent.length === 0) // required Working content
            invalidMess.push(Constant.WORKING_CONTENT + Constant.ERR_REQUIRED);
        else if (wrkContent.length > Constant.MAX_WORKING_CONTENT) // valid string length
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.WORKING_CONTENT).replace('{1}', Constant.MAX_WORKING_CONTENT));

        //// Delivery location
        //var deliLocation = $('#CONTRACT_DELIVERY_LOCATION').val().trim();
        //if (deliLocation.length === 0) // required Deliverables
        //    invalidMess.push(Constant.DELIVERY_LOCATION + Constant.ERR_REQUIRED);
        //else if (deliLocation.length > Constant.MAX_DELIVERY_LOCATION) // valid string length
        //    invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.DELIVERY_LOCATION).replace('{1}', Constant.MAX_DELIVERY_LOCATION));

        //// Deliverables
        //var deliv = $('#CONTRACT_DELIVERABLES').val();
        //if (deliv.length === 0) // required Deliverables
        //    invalidMess.push(Constant.DELIVERABLES + Constant.ERR_REQUIRED);
        //else if (deliv.length > Constant.MAX_DELIVERABLES) // valid string length
        //    invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.DELIVERABLES).replace('{1}', Constant.MAX_DELIVERABLES));

        // Worker Name
        var workerName = $('#CONTRACT_WORKER_NAME').val();
        if (workerName.length === 0) // required Worker Name
            invalidMess.push(Constant.WORKER_NAME + Constant.ERR_REQUIRED);
        else if (workerName.length > Constant.MAX_WORKER_NAME) // valid string length
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.WORKER_NAME).replace('{1}', Constant.MAX_WORKER_NAME));

        // Worker Name Kana
        var workerNameKana = $('#CONTRACT_WORKER_NAME_KANA').val();
        if (workerNameKana.length === 0) // required Worker Name Kana
            invalidMess.push(Constant.WORKER_NAME_KANA + Constant.ERR_REQUIRED);
        else if (workerNameKana.length > Constant.MAX_WORKER_NAME) // valid string length
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.WORKER_NAME_KANA).replace('{1}', Constant.MAX_WORKER_NAME));

        // Woker print name
        var workerPrintName = $('#CONTRACT_WORKER_PRINT_NAME').val();
        if (workerPrintName.length === 0) // required Worker print Name
        {
            if (workerName.length > 0 && workerName.length < Constant.MAX_WORKER_NAME)
            {
                $('#CONTRACT_WORKER_PRINT_NAME').val(workerName);
            }
            else
            {
                invalidMess.push(Constant.WORKER_PRINT_NAME + Constant.ERR_REQUIRED);
            }            
        }
        else if (workerPrintName.length > Constant.MAX_WORKER_NAME) // valid string length
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.WORKER_PRINT_NAME).replace('{1}', Constant.MAX_WORKER_NAME));

        // Initialize date
        var startDate = $('#CONTRACT_PRJ_PERIOD_START').val().trim();
        var endDate = $('#CONTRACT_PRJ_PERIOD_END').val().trim();
        var existInvalidDate = false;

        // Validate start date
        var errStartDate = null;
        if (startDate.length === 0) { // required start date
            existInvalidDate = true;
            errStartDate = Constant.WORK_PERIOD_START + Constant.ERR_REQUIRED;
            invalidMess.push(errStartDate);
        }
        else if (startDate.length > 0)
        {
            errStartDate = iseiQ.utility.validDate(startDate, Constant.DATE_FORMAT, Constant.WORK_PERIOD_START);
            if (errStartDate != null) {
                existInvalidDate = true;
                invalidMess.push(errStartDate);
            }
        }
        else if (errStartDate == null && startDate.length > Constant.MAX_DATE)
        { // valid string length
            existInvalidDate = true;
            errInvalid = Constant.WORK_PERIOD_START + Constant.ERR_INCORRECT_DATE;
            invalidMess.push(errInvalid);
        }

        // Validate end date
        var errEndDate = null;
        if (endDate.length === 0) { // required end date
            existInvalidDate = true;
            errEndDate = Constant.WORK_PERIOD_END + Constant.ERR_REQUIRED;
            invalidMess.push(errEndDate);
        } 
        else if (endDate.length > 0)
        {
            var errEndDate = iseiQ.utility.validDate(endDate, Constant.DATE_FORMAT, Constant.WORK_PERIOD_END);
            if (errEndDate != null) {
                existInvalidDate = true;
                invalidMess.push(errEndDate);
            }
        }
        else if (errEndDate == null && endDate.length > Constant.MAX_DATE)
        {
            existInvalidDate = true;
            errInvalid = Constant.WORK_PERIOD_END + Constant.ERR_INCORRECT_DATE;
            invalidMess.push(errInvalid);
        }

        // compare period
        if (!existInvalidDate &&
            !iseiQ.utility.compareDate(startDate, endDate, Constant.DATE_FORMAT)) {
            existInvalidDate = true;
            invalidMess.push(Constant.ERR_COMPARE_TOW_DATE.replace('{0}', Constant.WORK_PERIOD_END).replace('{1}', Constant.WORK_PERIOD_START));            
        }

        ///////////////////////////////////////////
        // >> Sales
        // Customer
        var customerDspName = $('#CONTRACT_OA_BP_DSP_NAME').val().trim();
        if (customerDspName.length === 0) // required customer name
            invalidMess.push(Constant.BILLING + Constant.ERR_REQUIRED);

        // Customer PIC
        var customerPicName = $('#CONTRACT_OA_BP_STAFF_NAME').val().trim();
        if (customerPicName.length === 0) // required customer name
            invalidMess.push(Constant.CUSTOMER_PIC_NAME + Constant.ERR_REQUIRED);

        // Staff
        var StaffIdOA = $('#CONTRACT_OA_STAFF_ID').val();
        if (StaffIdOA == null) // required Staff name
            invalidMess.push(Constant.STAFF_ID + Constant.ERR_REQUIRED);

        if ($('#CONTRACT_OA_PAYMENT_METHOD_TYPE').val() == PaymentMethodType.Duration)
        {
            // Base Time lower
            var basetimelower = $('#CONTRACT_OA_BASE_TIME_LOWER').val().trim();
            if (basetimelower.length > Constant.MAX_BASE_TIME) // valid string length
                invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.BASE_TIME_LOWER).replace('{1}', Constant.MAX_BASE_TIME));

            // Base time upper
            var basetimeupper = $('#CONTRACT_OA_BASE_TIME_UPPER').val().trim();
            if (basetimeupper.length > Constant.MAX_BASE_TIME) // valid string length
            {
                invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.BASE_TIME_LOWER).replace('{1}', Constant.MAX_BASE_TIME));
            }
            else {
                var paymentMethodType = $('#CONTRACT_OA_PAYMENT_METHOD_TYPE').val();
                var errCompareBase = iseiQ.utility.checkCompareBasetime(basetimelower, basetimeupper, paymentMethodType);
                if (errCompareBase) invalidMess.push(errCompareBase);
            }

            // Exceed unit price
            var exceedUnitPrice = $('#CONTRACT_OA_EXCEED_UNIT_PRICE').val().replace(/,/g, '');
            if (exceedUnitPrice.length > Constant.MAX_EXCEED_UNIT) // valid string length
                invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.EXCEED_UNIT).replace('{1}', Constant.MAX_EXCEED_UNIT));

            // Deduction unit price
            var deductionUnitPrice = $('#CONTRACT_OA_DEDUCTION_UNIT_PRICE').val().replace(/,/g, '');
            if (deductionUnitPrice.length > Constant.MAX_EXCEED_UNIT) // valid string length
                invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.DEDUCTION_UNIT).replace('{1}', Constant.MAX_EXCEED_UNIT));
        }
        
        if ($('#CONTRACT_OA_PAYMENT_METHOD_TYPE').val() == PaymentMethodType.Duration
            || $('#CONTRACT_OA_PAYMENT_METHOD_TYPE').val() == PaymentMethodType.Hour)
        {
            // Unit time
            var UnitTimeOA = $('#CONTRACT_OA_UNIT_TIME').val();
            if (UnitTimeOA.length === 0) {
                invalidMess.push(Constant.UNIT_TIME + Constant.ERR_REQUIRED);
            } else if ((UnitTimeOA < Constant.UNIT_TIME_DATA_MIN) || (UnitTimeOA > Constant.UNIT_TIME_DATA_MAX)) {
                invalidMess.push(Constant.UNIT_TIME + Constant.ERR_RANGE_VALUE_UNITTIME);
            }
        }       

        // Unit price
        var UnitPrice = $('#CONTRACT_OA_UNIT_PRICE').val().replace(/,/g, '');
        if (UnitPrice.length === 0) {
            invalidMess.push(Constant.UNIT_PRICE + Constant.ERR_REQUIRED);
        } else if ((parseInt(UnitPrice) < Constant.MIN_UNIT_PRICE) || (parseInt(UnitPrice) > Constant.MAX_UNIT_PRICE)) {
            invalidMess.push(Constant.UNIT_PRICE + Constant.ERR_RANGE_UNIT_PRICE);
        }

        // Tax rate
        var TaxRate = $('#CONTRACT_OA_TAX_RATE').val();
        if (TaxRate.length === 0)
            invalidMess.push(Constant.TAXRATE + Constant.ERR_REQUIRED);
        else if (TaxRate.length > Constant.MAX_TAXRATE) // valid string length
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.TAXRATE).replace('{1}', Constant.MAX_TAXRATE));

        // Closing day
        var closingDayOA = $('#CONTRACT_OA_CLOSING_DAY').val();
        if (closingDayOA == 0)
        {
            invalidMess.push(Constant.CLOSING_DAY + Constant.ERR_REQUIRED_CHOOSE);
        }

        // Payment site type
        var paymentSiteTypeOA = $('#CONTRACT_OA_PAYMENT_SITE_TYPE').val();
        if (paymentSiteTypeOA == 0) {
            invalidMess.push(Constant.PAYMENT_SITE_TYPE + Constant.ERR_REQUIRED_CHOOSE);
        }

        // Payment day
        var paymentDayOA = $('#CONTRACT_OA_PAYMENT_DAY').val();
        if (paymentDayOA == 0) {
            invalidMess.push(Constant.PAYMENT_DAY + Constant.ERR_REQUIRED_CHOOSE);
        }

        // Closing day cannot bigger than payment day. Refs #28177
        if (PaymentSiteType.ThisMonth === paymentSiteTypeOA && parseInt(closingDayOA) > parseInt(paymentDayOA)) {
            invalidMess.push(Constant.ERR_CLOSING_DAY_BIGGER_THAN_PAYMENT_DAY);
        }

        // Acceptance Note
        var aptNote = $('#CONTRACT_OA_ACCEPTANCE_NOTE').val();
        if (aptNote.length > Constant.MAX_NOTE) // valid string length
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.NOTE).replace('{1}', Constant.MAX_NOTE));

        ///////////////////////////////////////////////////////////////////////////////
        // >> Validate div-coad
        var $specialSalesContent = $('.special-sales-content');
        var $specialSalesDataRowsLeft = $specialSalesContent.find('.col-left tbody tr:not(.hide)');
        var $specialSalesDataRowsCenter = $specialSalesContent.find('.col-center tbody tr:not(.hide)');
        var errDuplicateYmCoad = null;

        if ($specialSalesContent.find('tbody tr:not(.hide)').length > 0) {
            $specialSalesDataRowsLeft.each(function (index, element) {
                // Validate target ym
                var $targetYm = $($specialSalesDataRowsLeft[index]).find('input.target-ym');

                if ($targetYm.val().length === 0 && $($specialSalesDataRowsLeft[index]).find('.cont-seq-no').val() != 0) {
                    invalidMess.push(Constant.ADJUSTED_YM + Constant.ERR_REQUIRED);
                }
                else if ($targetYm.val().length === 0) {
                    return false;
                } else if ($targetYm.val().length > 7) {
                    invalidMess.push(Constant.ERR_FORMAT_YM.replace('{0}', Constant.TARGET_YM));
                } else {
                    var errFormatYM = iseiQ.utility.validDate($targetYm.val(), Constant.YM_FORMAT, Constant.TARGET_YM);

                    if (errFormatYM != null) {
                        invalidMess.push(errFormatYM);
                    }
                    if (!iseiQ.utility.isValidTargetYM(startDate, endDate, $targetYm.val(), closingDayOA)) {
                        // Target YM out of range of project duration
                        invalidMess.push(Constant.ERR_TARGETYM_INVALID);
                    }
                    if (errDuplicateYmCoad == null)
                    {
                        $targetYm.parents('tbody').children('tr:not(.hide)').find('.target-ym').each(function () {
                            if (!$(this).is($targetYm)) {
                                var currentTargetYM = $(this).val();
                                if (iseiQ.utility.validDate(currentTargetYM, Constant.YM_FORMAT, Constant.TARGET_YM) == null
                                    && currentTargetYM == $targetYm.val()) {
                                    errDuplicateYmCoad = Constant.ERR_TARGETYM_DUPLICATE;
                                    invalidMess.push(errDuplicateYmCoad);
                                }
                            }
                        });
                    }
                }

                if ($targetYm.val().length > 0) {
                    // validate Unit price
                    var UnitPrice = $($specialSalesDataRowsCenter[index]).find('input.unit-price').val().replace(/,/g, '');
                    if (UnitPrice.length === 0) {
                        invalidMess.push(Constant.UNIT_PRICE + Constant.ERR_REQUIRED);
                    }
                    else if ((parseInt(UnitPrice) < Constant.MIN_UNIT_PRICE) || (parseInt(UnitPrice) > Constant.MAX_UNIT_PRICE)) {
                        invalidMess.push(Constant.UNIT_PRICE + Constant.ERR_RANGE_UNIT_PRICE);
                    }

                    // Validate Quantity
                    var Qty = $($specialSalesDataRowsCenter[index]).find('input.qty').val();
                    if (Qty.length === 0) {
                        invalidMess.push(Constant.QUANTITY + Constant.ERR_REQUIRED);
                    }

                    // Validate Amount
                    var Amount = $($specialSalesDataRowsCenter[index]).find('input.amount').val().replace(/,/g, '');
                    if (Amount.length === 0) {
                        invalidMess.push(Constant.AMOUNT + Constant.ERR_REQUIRED);
                    }
                    else if ((parseInt(Amount) < Constant.MIN_AMOUNT) || (parseInt(Amount) > Constant.MAX_AMOUNT)) {
                        invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.AMOUNT).replace('{1}', Constant.MAX_AMOUNT_LENGTH));
                    }

                    if ($($specialSalesDataRowsCenter[index]).find('.coad-payment-method-type').val() == PaymentMethodType.Duration) {
                        var basetimelowerCoad = $($specialSalesDataRowsCenter[index]).find('input.base-time-lower').val();
                        var basetimeupperCoad = $($specialSalesDataRowsCenter[index]).find('input.base-time-upper').val();
                        var paymentMethodType = $($specialSalesDataRowsCenter[index]).find('.coad-payment-method-type').val();
                        var errCompareBase = iseiQ.utility.checkCompareBasetime(basetimelowerCoad, basetimeupperCoad, paymentMethodType);
                        if (errCompareBase) invalidMess.push(errCompareBase);
                    }
                    if ($($specialSalesDataRowsCenter[index]).find('.coad-payment-method-type').val() == PaymentMethodType.Duration
                        || $($specialSalesDataRowsCenter[index]).find('.coad-payment-method-type').val() == PaymentMethodType.Hour) {
                        // Validate unit time
                        var UnitTimeCoad = $($specialSalesDataRowsCenter[index]).find('input.unit-time').val();
                        if (UnitTimeCoad.length === 0) {
                            invalidMess.push(Constant.UNIT_TIME + Constant.ERR_REQUIRED);
                        } else if ((UnitTimeCoad < Constant.UNIT_TIME_DATA_MIN) || (UnitTimeCoad > Constant.UNIT_TIME_DATA_MAX)) {
                            invalidMess.push(Constant.UNIT_TIME + Constant.ERR_RANGE_VALUE_UNITTIME);
                        }
                    }
                }
            });
        }

        ///////////////////////////////////////////
        // >> Payment
        // Supplier
        var supplierDspName = $('#CONTRACT_OP_BP_DSP_NAME').val().trim();
        if (supplierDspName.length === 0) // required customer name
            invalidMess.push(Constant.SUPPLIER_NAME + Constant.ERR_REQUIRED);

        // Supplier PIC
        var supplierPicName = $('#CONTRACT_OP_BP_STAFF_NAME').val().trim();
        if (supplierPicName.length === 0) // required customer name
            invalidMess.push(Constant.SUPPLIER_PIC + Constant.ERR_REQUIRED);

        // Supplier Staff Id
        var staffIdOp = $('#CONTRACT_OP_STAFF_ID').val();
        if (staffIdOp == null) // required Staff name
            invalidMess.push(Constant.STAFF_ID_OP + Constant.ERR_REQUIRED);

        if ($('#CONTRACT_OP_PAYMENT_METHOD_TYPE').val() == PaymentMethodType.Duration)
        {
            // Base Time lower
            var basetimelowerOp = $('#CONTRACT_OP_BASE_TIME_LOWER').val().trim();
            if (basetimelowerOp.length > Constant.MAX_BASE_TIME) // valid string length
                invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.BASE_TIME_LOWER).replace('{1}', Constant.MAX_BASE_TIME));

            // Base time upper
            var basetimeupperOp = $('#CONTRACT_OP_BASE_TIME_UPPER').val().trim();
            if (basetimeupperOp.length > Constant.MAX_BASE_TIME) // valid string length
                invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.BASE_TIME_UPPER).replace('{1}', Constant.MAX_BASE_TIME));
            else {
                var paymentMethodType = $('#CONTRACT_OP_PAYMENT_METHOD_TYPE').val();
                var errCompareBase = iseiQ.utility.checkCompareBasetime(basetimelowerOp, basetimeupperOp, paymentMethodType);
                if (errCompareBase) invalidMess.push(errCompareBase);
            }

            // Exceed unit price
            var exceedUnitPrice = $('#CONTRACT_OP_EXCEED_UNIT_PRICE').val().replace(/,/g, '');
            if (exceedUnitPrice.length > Constant.MAX_EXCEED_UNIT) // valid string length
                invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.EXCEED_UNIT).replace('{1}', Constant.MAX_EXCEED_UNIT));

            // Deduction unit price
            var deductionUnitPrice = $('#CONTRACT_OP_DEDUCTION_UNIT_PRICE').val().replace(/,/g, '');
            if (deductionUnitPrice.length > Constant.MAX_EXCEED_UNIT) {
                invalidMess.push(Constant.DEDUCTION_UNIT + Constant.ERR_RANGE_UNIT_PRICE);
            }
        }
        
        if ($('#CONTRACT_OP_PAYMENT_METHOD_TYPE').val() == PaymentMethodType.Duration
            || $('#CONTRACT_OP_PAYMENT_METHOD_TYPE').val() == PaymentMethodType.Hour) {
            // Unit Time OP
            var UnitTimeOP = $('#CONTRACT_OP_UNIT_TIME').val();
            if (UnitTimeOP.length === 0) {
                invalidMess.push(Constant.UNIT_TIME + Constant.ERR_REQUIRED);
            } else if ((UnitTimeOP < Constant.UNIT_TIME_DATA_MIN) || (UnitTimeOP > Constant.UNIT_TIME_DATA_MAX)) {
                invalidMess.push(Constant.UNIT_TIME + Constant.ERR_RANGE_VALUE_UNITTIME);
            }
        }
        // Unit Price OP
        var UnitPriceOP = $('#CONTRACT_OP_UNIT_PRICE').val().replace(/,/g, '');
        if (UnitPriceOP.length === 0) {
            invalidMess.push(Constant.UNIT_PRICE + Constant.ERR_REQUIRED);
        } else if ((parseInt(UnitPriceOP) < Constant.MIN_UNIT_PRICE) || (parseInt(UnitPriceOP) > Constant.MAX_UNIT_PRICE)) {
            invalidMess.push(Constant.UNIT_PRICE + Constant.ERR_RANGE_UNIT_PRICE);
        }

        // Tax rate
        var TaxRate = $('#CONTRACT_OP_TAX_RATE').val(); 
        if (TaxRate.length === 0)
            invalidMess.push(Constant.TAXRATE + Constant.ERR_REQUIRED);       
        else if (TaxRate.length > Constant.MAX_TAXRATE) // valid string length
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.TAXRATE).replace('{1}', Constant.MAX_TAXRATE));

        // Closing day
        var closingDayOP = $('#CONTRACT_OP_CLOSING_DAY').val();
        if (closingDayOP == 0) {
            invalidMess.push(Constant.CLOSING_DAY + Constant.ERR_REQUIRED_CHOOSE);
        }

        // Payment site type
        var paymentSiteTypeOP = $('#CONTRACT_OP_PAYMENT_SITE_TYPE').val();
        if (paymentSiteTypeOP == 0) {
            invalidMess.push(Constant.PAYMENT_SITE_TYPE + Constant.ERR_REQUIRED_CHOOSE);
        }

        // Payment day
        var paymentDayOP = $('#CONTRACT_OP_PAYMENT_DAY').val();
        if (paymentDayOP == 0) {
            invalidMess.push(Constant.PAYMENT_DAY + Constant.ERR_REQUIRED_CHOOSE);
        }

        // Closing day cannot bigger than payment day. Refs #28177
        if (PaymentSiteType.ThisMonth === paymentSiteTypeOP && parseInt(closingDayOP) > parseInt(paymentDayOP)) {
            invalidMess.push(Constant.ERR_CLOSING_DAY_BIGGER_THAN_PAYMENT_DAY);
        }

        var $specialPaymentContent = $('.special-payment-content');
        var $specialPaymentDataRowsLeft = $specialPaymentContent.find('.col-left tbody tr:not(.hide)');
        var $specialPaymentDataRowsCenter = $specialPaymentContent.find('.col-center tbody tr:not(.hide)');
        var errDuplicateYmCopd = null;
        if ($specialPaymentContent.find('tbody tr:not(.hide)').length > 0) {
            $specialPaymentDataRowsLeft.each(function (index, element) {
                // Validate target ym
                var $targetYm = $($specialPaymentDataRowsLeft[index]).find('input.target-ym');

                if ($targetYm.val().length === 0 && $($specialPaymentDataRowsLeft[index]).find('.cont-seq-no').val() != 0) {
                    invalidMess.push(Constant.ADJUSTED_YM + Constant.ERR_REQUIRED);
                }
                else if ($targetYm.val().length === 0) {
                    return false;
                } else if ($targetYm.val().length > 7) {
                    invalidMess.push(Constant.ERR_FORMAT_YM.replace('{0}', Constant.TARGET_YM));
                } else {
                    var errFormatYM = iseiQ.utility.validDate($targetYm.val(), Constant.YM_FORMAT, Constant.TARGET_YM);
                    if (errFormatYM != null) {
                        invalidMess.push(errFormatYM);
                    }
                    if (!iseiQ.utility.isValidTargetYM(startDate, endDate, $targetYm.val(), closingDayOP)) {
                        // Target YM out of range of project duration
                        invalidMess.push(Constant.ERR_TARGETYM_INVALID);
                    }
                    if(errDuplicateYmCopd == null)
                    {
                        $targetYm.parents('tbody').children('tr:not(.hide)').find('.target-ym').each(function () {
                            if (!$(this).is($targetYm)) {
                                var currentTargetYM = $(this).val();
                                if (iseiQ.utility.validDate(currentTargetYM, Constant.YM_FORMAT, Constant.TARGET_YM) == null
                                    && currentTargetYM == $targetYm.val()) {
                                    errDuplicateYmCopd = Constant.ERR_TARGETYM_DUPLICATE;
                                    invalidMess.push(errDuplicateYmCopd);
                                }
                            }
                        });
                    }
                }

                if ($targetYm.val().length > 0) {
                    // validate Unit price
                    var UnitPrice = $($specialPaymentDataRowsCenter[index]).find('input.unit-price-copd').val().replace(/,/g, '');
                    if (UnitPrice.length === 0) {
                        invalidMess.push(Constant.UNIT_PRICE + Constant.ERR_REQUIRED);
                    }
                    else if ((parseInt(UnitPrice) < Constant.MIN_UNIT_PRICE) || (parseInt(UnitPrice) > Constant.MAX_UNIT_PRICE)) {
                        invalidMess.push(Constant.UNIT_PRICE + Constant.ERR_RANGE_UNIT_PRICE);
                    }

                    // Validate Quantity
                    var Qty = $($specialPaymentDataRowsCenter[index]).find('input.qty-copd').val();
                    if (Qty.length === 0) {
                        invalidMess.push(Constant.QUANTITY + Constant.ERR_REQUIRED);
                    }

                    // Validate Amount
                    var Amount = $($specialPaymentDataRowsCenter[index]).find('input.amount-copd').val().replace(/,/g, '');
                    if (Amount.length === 0) {
                        invalidMess.push(Constant.AMOUNT + Constant.ERR_REQUIRED);
                    }
                    else if ((parseInt(Amount) < Constant.MIN_AMOUNT) || (parseInt(Amount) > Constant.MAX_AMOUNT)) {
                        invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.AMOUNT).replace('{1}', Constant.MAX_AMOUNT_LENGTH));
                    }

                    if ($($specialPaymentDataRowsCenter[index]).find('.copd-payment-method-type').val() == PaymentMethodType.Duration)
                    {
                        var basetimelowerCopd = $($specialPaymentDataRowsCenter[index]).find('input.base-time-lower-copd').val();
                        var basetimeupperCopd = $($specialPaymentDataRowsCenter[index]).find('input.base-time-upper-copd').val();
                        var paymentMethodType = $($specialPaymentDataRowsCenter[index]).find('.copd-payment-method-type').val();
                        var errCompareBase = iseiQ.utility.checkCompareBasetime(basetimelowerCopd, basetimeupperCopd, paymentMethodType);
                        if (errCompareBase) invalidMess.push(errCompareBase);
                    }
                    
                    if ($($specialPaymentDataRowsCenter[index]).find('.copd-payment-method-type').val() == PaymentMethodType.Duration
                        || $($specialPaymentDataRowsCenter[index]).find('.copd-payment-method-type').val() == PaymentMethodType.Hour)
                    {
                        // Validate unit time
                        var UnitTimeCoad = $($specialPaymentDataRowsCenter[index]).find('input.unit-time-copd').val();
                        if (UnitTimeCoad.length === 0) {
                            invalidMess.push(Constant.UNIT_TIME + Constant.ERR_REQUIRED);
                        } else if ((UnitTimeCoad < Constant.UNIT_TIME_DATA_MIN) || (UnitTimeCoad > Constant.UNIT_TIME_DATA_MAX)) {
                            invalidMess.push(Constant.UNIT_TIME + Constant.ERR_RANGE_VALUE_UNITTIME);
                        }
                    }                    
                }
            });
        }

        // Acceptance Note
        var plcNote = $('#CONTRACT_OP_PLACEMENT_NOTE').val();
        if (plcNote.length > Constant.MAX_NOTE) // valid string length
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.NOTE).replace('{1}', Constant.MAX_NOTE));
        ////////////////////////////////////////////////////////////////////////////////

        // Working location
        //var wrkLocation = $('#CONTRACT_WORKING_LOCATION').val();
        //if (wrkLocation.length === 0) // required Working location
        //    invalidMess.push(Constant.WORKING_LOCATION + Constant.ERR_REQUIRED);
        //else if (wrkLocation.length > Constant.MAX_WORKING_LOCATION) // valid string length
        //    invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.WORKING_LOCATION).replace('{1}', Constant.MAX_WORKING_LOCATION));

        // Project memo
        var prjMemo = $('#PROJECT_MEMO_MEMO').val();
        if (prjMemo.length > Constant.MAX_AREA_LENGTH) // valid string length
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.MEMO).replace('{1}', Constant.MAX_AREA_LENGTH));

        // Contract Note
        var contractNote = $('#CONTRACT_CONTRACT_NOTE').val();
        if (contractNote.length > Constant.MAX_NOTE) // valid string length
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.NOTE).replace('{1}', Constant.MAX_NOTE));

        return invalidMess;
    }

    // OnClick .btnAddCoad
    function AddCoad() {
        var $parentElement = $('.special-sales-content');
        var $hideRows = $parentElement.find('tbody tr.hide');

        if ($hideRows.length > 0
            && $parentElement.find('.cont-seq-no').first().val() == 0)
        {
            $hideRows.removeClass('hide');
        }
        else
        {
            var $tbLeftLastRow = $parentElement.find('.col-left tbody tr:last');
            var $tbCenterLastRow = $parentElement.find('.col-center tbody tr:last');
            var $tbRightLastRow = $parentElement.find('.col-right tbody tr:last');

            $tbLeftLastRow.after($tbLeftLastRow.prop('outerHTML'));
            $tbCenterLastRow.after($tbCenterLastRow.prop('outerHTML'));
            $tbRightLastRow.after($tbRightLastRow.prop('outerHTML'));
            $parentElement.find('.col-left tbody tr:last').removeClass('hide');
            $parentElement.find('.col-center tbody tr:last').removeClass('hide');
            $parentElement.find('.col-right tbody tr:last').removeClass('hide');
        }

        ResetInitValueCoad($parentElement);
        OnChangeCoadPaymentMethodType($parentElement.find('.col-center tbody tr:last .coad-payment-method-type'));

        resetArrCoad();
        iseiQ.utility.InitDatepickerMonths();
    }

    // OnClick .btnDeleteCoad
    function DeleteCoad(control) {
        var $parentElement = $('.special-sales-content');
        var $tbRightTargetRow = $(control).parents('tr');
        var index = $tbRightTargetRow.index();
        var $tbLeftTargetRow = $($parentElement.find('.col-left tbody tr')[index]);
        var $tbCenterTargetRow = $($parentElement.find('.col-center tbody tr')[index]);

        if ($tbLeftTargetRow.find('.cont-seq-no').val() == 0)
        {
            if ($parentElement.find('.col-left tbody tr').length > 1)
            {
                $tbLeftTargetRow.remove();
                $tbCenterTargetRow.remove();
                $tbRightTargetRow.remove();

                resetArrCoad();
            }
            else
            {
                $tbLeftTargetRow.addClass('hide').find('input:text, input:hidden, select, textarea').removeAttr('name');
                $tbCenterTargetRow.addClass('hide').find('input:text, input:hidden, select, textarea').removeAttr('name');
                $tbRightTargetRow.addClass('hide').find('input:text, input:hidden, select, textarea').removeAttr('name');
            }
        }
        else
        {
            $tbLeftTargetRow.addClass('hide');
            $tbCenterTargetRow.addClass('hide');
            $tbRightTargetRow.addClass('hide').find('.del-flg').val(1);
        }
    }

    // OnClick .btnAddCopd
    function AddCopd(control) {
        var $parentElement = $('.special-payment-content');
        var $hideRows = $parentElement.find('tbody tr.hide');

        if ($hideRows.length > 0
            && $parentElement.find('.cont-seq-no').first().val() == 0) {
            $hideRows.removeClass('hide');
        }
        else {
            var $tbLeftLastRow = $parentElement.find('.col-left tbody tr:last');
            var $tbCenterLastRow = $parentElement.find('.col-center tbody tr:last');
            var $tbRightLastRow = $parentElement.find('.col-right tbody tr:last');

            $tbLeftLastRow.after($tbLeftLastRow.prop('outerHTML'));
            $tbCenterLastRow.after($tbCenterLastRow.prop('outerHTML'));
            $tbRightLastRow.after($tbRightLastRow.prop('outerHTML'));

            $parentElement.find('.col-left tbody tr:last').removeClass('hide');
            $parentElement.find('.col-center tbody tr:last').removeClass('hide');
            $parentElement.find('.col-right tbody tr:last').removeClass('hide');
        }

        ResetInitValueCopd($parentElement);
        $parentElement.find('.col-center tbody tr:last .copd-payment-method-type').change();

        resetArrCopd();
        iseiQ.utility.InitDatepickerMonths();
    }

    // OnClick .btnDeleteCopd
    function DeleteCopd(control) {
        var $parentElement = $('.special-payment-content');
        var $tbRightTargetRow = $(control).parents('tr');
        var index = $tbRightTargetRow.index();
        var $tbLeftTargetRow = $($parentElement.find('.col-left tbody tr')[index]);
        var $tbCenterTargetRow = $($parentElement.find('.col-center tbody tr')[index]);

        if ($tbLeftTargetRow.find('.cont-seq-no').val() == 0) {
            if ($parentElement.find('.col-left tbody tr').length > 1) {
                $tbLeftTargetRow.remove();
                $tbCenterTargetRow.remove();
                $tbRightTargetRow.remove();

                resetArrCopd();
            }
            else {
                $tbLeftTargetRow.addClass('hide').find('input:text, input:hidden, select, textarea').removeAttr('name');
                $tbCenterTargetRow.addClass('hide').find('input:text, input:hidden, select, textarea').removeAttr('name');
                $tbRightTargetRow.addClass('hide').find('input:text, input:hidden, select, textarea').removeAttr('name');
            }
        }
        else {
            $tbLeftTargetRow.addClass('hide');
            $tbCenterTargetRow.addClass('hide');
            $tbRightTargetRow.addClass('hide').find('.del-flg').val(1);
        }
    }

    function ResetInitValueCoad($parentElement)
    {
        var $tbLeftLastRow = $parentElement.find('.col-left tr:last');
        var $tbCenterLastRow = $parentElement.find('.col-center tr:last');
        var $tbRightLastRow = $parentElement.find('.col-right tr:last');

        $tbLeftLastRow.find('.cont-seq-no').val(0);
        $tbLeftLastRow.find('.cont-apt-seq-no').val(0);
        $tbLeftLastRow.find('.dsp-order').val(0);
        $tbLeftLastRow.find('.target-ym').val('');

        $tbCenterLastRow.find('.coad-payment-method-type').val($('#CONTRACT_OA_PAYMENT_METHOD_TYPE').val());
        $tbCenterLastRow.find('.unit-price').val(0);
        $tbCenterLastRow.find('.qty').val(1);
        $tbCenterLastRow.find('.amount').val(0);
        $tbCenterLastRow.find('.adjustment-condition').val('');
        $tbCenterLastRow.find('.calc-unit').val($('#CONTRACT_OA_OUTER_TIME_CALC_UNIT').val());
        $tbCenterLastRow.find('.base-time-lower').val(0);
        $tbCenterLastRow.find('.base-time-upper').val(0);
        $tbCenterLastRow.find('.exceed-unit-price').val($('#CONTRACT_OA_EXCEED_UNIT_PRICE').val());
        $tbCenterLastRow.find('.deduction-unit-price').val($('#CONTRACT_OA_DEDUCTION_UNIT_PRICE').val());
        $tbCenterLastRow.find('.unit-time').val($('#CONTRACT_OA_UNIT_TIME').val());

        $parentElement.find('.col-right tr:last .del-flg').val(0);
    }

    function ResetInitValueCopd($parentElement) {
        var $tbLeftLastRow = $parentElement.find('.col-left tr:last');
        var $tbCenterLastRow = $parentElement.find('.col-center tr:last');
        var $tbRightLastRow = $parentElement.find('.col-right tr:last');

        $tbLeftLastRow.find('.cont-seq-no').val(0);
        $tbLeftLastRow.find('.cont-plc-seq-no').val(0);
        $tbLeftLastRow.find('.dsp-order').val(0);
        $tbLeftLastRow.find('.target-ym').val('');

        $tbCenterLastRow.find('.copd-payment-method-type').val($('#CONTRACT_OP_PAYMENT_METHOD_TYPE').val());
        $tbCenterLastRow.find('.unit-price-copd').val(0);
        $tbCenterLastRow.find('.qty-copd').val(1);
        $tbCenterLastRow.find('.amount-copd').val(0);
        $tbCenterLastRow.find('.adjustment-condition-copd').val('');
        $tbCenterLastRow.find('.calc-unit-copd').val($('#CONTRACT_OP_OUTER_TIME_CALC_UNIT').val());
        $tbCenterLastRow.find('.base-time-lower-copd').val(0);
        $tbCenterLastRow.find('.base-time-upper-copd').val(0);
        $tbCenterLastRow.find('.exceed-unit-price-copd').val($('#CONTRACT_OP_EXCEED_UNIT_PRICE').val());
        $tbCenterLastRow.find('.deduction-unit-price-copd').val($('#CONTRACT_OP_DEDUCTION_UNIT_PRICE').val());
        $tbCenterLastRow.find('.unit-time').val($('#CONTRACT_OP_UNIT_TIME').val());

        $parentElement.find('.col-right tr:last .del-flg').val(0);
    }

    // OnChange .coad-payment-method-type
    function OnChangeCoadPaymentMethodType(control) {
        var $parent = $(control).parents('tr');
        var time_index = $(control).val();

        if (time_index == TimeIndex.CONTRACT_PERIOD_START) {
            $parent.find('.base-time-lower').css('opacity', '1');
            $parent.find('.base-time-upper').css('opacity', '1');
            $parent.find('.exceed-unit-price').css('opacity', '1');
            $parent.find('.calc-unit').css('opacity', '1');
            $parent.find('.btnCalc_OAD').css('opacity', '1');
            $parent.find('.deduction-unit-price').css('opacity', '1');
            $parent.find('.lbUnit').css('opacity', '1');
            $parent.find('.unit-time, .lbUnitTime').css('opacity', '1');

            // Enable elements
            $parent.find('.base-time-lower').prop('disabled', false);
            $parent.find('.base-time-upper').prop('disabled', false);
            $parent.find('.exceed-unit-price').prop('disabled', false);
            $parent.find('.calc-unit').prop('disabled', false);
            $parent.find('.btnCalc_OAD').prop('disabled', false);
            $parent.find('.deduction-unit-price').prop('disabled', false);
            $parent.find('.lbUnit').prop('disabled', false);
            $parent.find('.unit-time, .lbUnitTime').prop('disabled', false);

            $parent.find('.unit_time_type').text('円/月');
        }
        else {
            $parent.find('.base-time-lower').css('opacity', '0');
            $parent.find('.base-time-upper').css('opacity', '0');
            $parent.find('.exceed-unit-price').css('opacity', '0');
            $parent.find('.calc-unit').css('opacity', '0');
            $parent.find('.btnCalc_OAD').css('opacity', '0');
            $parent.find('.deduction-unit-price').css('opacity', '0');
            $parent.find('.lbUnit').css('opacity', '0');
            $parent.find('.unit-time, .lbUnitTime').css('opacity', '0');

            // Disable elements
            $parent.find('.base-time-lower').prop('disabled', true);
            $parent.find('.base-time-upper').prop('disabled', true);
            $parent.find('.exceed-unit-price').prop('disabled', true);
            $parent.find('.calc-unit').prop('disabled', true);
            $parent.find('.btnCalc_OAD').prop('disabled', true);
            $parent.find('.deduction-unit-price').prop('disabled', true);
            $parent.find('.lbUnit').prop('disabled', true);
            $parent.find('.unit-time, .lbUnitTime').prop('disabled', true);

            $parent.find('.unit_time_type').text('円/月');

            if (time_index == TimeIndex.DELIVERY_DATE) {
                $parent.find('.unit-time, .lbUnitTime').css('opacity', '1');
                $parent.find('.unit-time, .lbUnitTime').prop('disabled', false);
                $parent.find('.unit_time_type').text('円/h');
            }
        }
    }

    // OnChange .copd-payment-method-type
    function OnChangeCopdPaymentMethodType(control) {
        var $parent = $(control).parents('tr');
        var time_index = $(control).val();

        if (time_index == TimeIndex.CONTRACT_PERIOD_START) {
            $parent.find('.base-time-lower-copd').css('opacity', '1');
            $parent.find('.base-time-upper-copd').css('opacity', '1');
            $parent.find('.exceed-unit-price-copd').css('opacity', '1');
            $parent.find('.deduction-unit-price-copd').css('opacity', '1');
            $parent.find('.calc-unit-copd').css('opacity', '1');
            $parent.find('.btnCalc_OPD').css('opacity', '1');
            $parent.find('.lbUnit').css('opacity', '1');
            $parent.find('.unit-time-copd, .lbUnitTimeCop').css('opacity', '1');

            $parent.find('.base-time-lower-copd').prop('disabled', false);
            $parent.find('.base-time-upper-copd').prop('disabled', false);
            $parent.find('.exceed-unit-price-copd').prop('disabled', false);
            $parent.find('.deduction-unit-price-copd').prop('disabled', false);
            $parent.find('.calc-unit-copd').prop('disabled', false);
            $parent.find('.btnCalc_OPD').prop('disabled', false);
            $parent.find('.lbUnit').prop('disabled', false);
            $parent.find('.unit-time-copd, .lbUnitTimeCop').prop('disabled', false);

            $parent.find('.unit_time_type').text('円/月');
        }
        else {
            $parent.find('.base-time-lower-copd').css('opacity', '0');
            $parent.find('.base-time-upper-copd').css('opacity', '0');
            $parent.find('.exceed-unit-price-copd').css('opacity', '0');
            $parent.find('.deduction-unit-price-copd').css('opacity', '0');
            $parent.find('.calc-unit-copd').css('opacity', '0');
            $parent.find('.btnCalc_OPD').css('opacity', '0');
            $parent.find('.lbUnit').css('opacity', '0');
            $parent.find('.unit-time-copd, .lbUnitTimeCop').css('opacity', '0');

            $parent.find('.base-time-lower-copd').prop('disabled', true);
            $parent.find('.base-time-upper-copd').prop('disabled', true);
            $parent.find('.exceed-unit-price-copd').prop('disabled', true);
            $parent.find('.deduction-unit-price-copd').prop('disabled', true);
            $parent.find('.calc-unit-copd').prop('disabled', true);
            $parent.find('.btnCalc_OPD').prop('disabled', true);
            $parent.find('.lbUnit').prop('disabled', true);
            $parent.find('.unit-time-copd, .lbUnitTimeCop').prop('disabled', true);

            $parent.find('.unit_time_type').text('円/月');

            if (time_index == TimeIndex.DELIVERY_DATE) {
                $parent.find('.unit-time-copd, .lbUnitTimeCop').css('opacity', '1');
                $parent.find('.unit-time-copd, .lbUnitTimeCop').prop('disabled', false);
                $parent.find('.unit_time_type').text('円/h');
            }
        }
    }

    // OnChange Coad.unit-price, Coad.qty
    function CalcAmountCoad(control) {
        var $parentElement = $(control).parents('tr');
        var price = $parentElement.find('.unit-price').val();
        var quantity = $parentElement.find('.qty').val();
        var unitprice = iseiQ.utility.convertMoneyToInt(price);
        var amount = Math.round(unitprice * quantity);
        $parentElement.find('.amount').val(iseiQ.utility.convertIntToMoney(amount));
    }

    // OnClick btnCalc_OAD
    function OnClickBtnCalcCoad() {
        var $clickedButton = $('.special-sales-content .btnCalc_OAD_clicked').removeClass('btnCalc_OAD_clicked');
        var $parentElement = $clickedButton.parents('tr');
        var calculation_type = $parentElement.find('.calc-unit').val();
        var unit_price = iseiQ.utility.convertMoneyToInt($parentElement.find('.unit-price').val());
        var base_time_lower = parseFloat($parentElement.find('.base-time-lower').val());
        var base_time_upper = parseFloat($parentElement.find('.base-time-upper').val());
        var is_included_tax = $('#IN_TAX_2').prop("checked");
        var tax_rate = $('#CONTRACT_OA_TAX_RATE').val();

        var refObj = { exceedUnitPrice: 0, deductionUnitPrice: 0 };
        ContractSES.CalcExceedDedutionUnitPrice(refObj, calculation_type, unit_price, base_time_lower, base_time_upper, is_included_tax, tax_rate);
        $parentElement.find('.exceed-unit-price').val(refObj.exceedUnitPrice);
        $parentElement.find('.deduction-unit-price').val(refObj.deductionUnitPrice);
    }

    // OnChange Copd.unit-price-copd, Copd.qty-copd
    function CalcAmountCopd(control) {
        var $parentElement = $(control).parents('tr');
        var price = $parentElement.find('.unit-price-copd').val();
        var quantity = $parentElement.find('.qty-copd').val();
        var unitprice = iseiQ.utility.convertMoneyToInt(price);
        var amount = Math.round(unitprice * quantity);
        $parentElement.find('.amount-copd').val(iseiQ.utility.convertIntToMoney(amount));
    }

    // OnClick .btnCalc_OPD
    function OnClickBtnCalcCopd() {
        var $clickedButton = $('.special-payment-content .btnCalc_OPD_clicked').removeClass('btnCalc_OPD_clicked');
        var $parentElement = $clickedButton.parents('tr');
        var calculation_type = $parentElement.find('.calc-unit-copd').val();
        var unit_price = iseiQ.utility.convertMoneyToInt($parentElement.find('.unit-price-copd').val());
        var base_time_lower = parseFloat($parentElement.find('.base-time-lower-copd').val());
        var base_time_upper = parseFloat($parentElement.find('.base-time-upper-copd').val());
        var is_included_tax = $('#IN_TAX_4').prop("checked");
        var tax_rate = $('#CONTRACT_OP_TAX_RATE').val();

        var refObj = { exceedUnitPrice: 0, deductionUnitPrice: 0 };
        ContractSES.CalcExceedDedutionUnitPrice(refObj, calculation_type, unit_price, base_time_lower, base_time_upper, is_included_tax, tax_rate);
        $parentElement.find('.exceed-unit-price-copd').val(refObj.exceedUnitPrice);
        $parentElement.find('.deduction-unit-price-copd').val(refObj.deductionUnitPrice);
    }


    // OnClick btnCalc_OA
    function OnClickBtnCalcOA() {
        var calculation_type = $('#CONTRACT_OA_OUTER_TIME_CALC_UNIT').val();
        var unit_price = iseiQ.utility.convertMoneyToInt($('#CONTRACT_OA_UNIT_PRICE').val());
        var base_time_lower = parseFloat($('#CONTRACT_OA_BASE_TIME_LOWER').val());
        var base_time_upper = parseFloat($('#CONTRACT_OA_BASE_TIME_UPPER').val());
        var is_included_tax = $('#IN_TAX_2').prop("checked");
        var tax_rate = $('#CONTRACT_OA_TAX_RATE').val();

        var refObj = { exceedUnitPrice: 0, deductionUnitPrice: 0 };
        ContractSES.CalcExceedDedutionUnitPrice(refObj, calculation_type, unit_price, base_time_lower, base_time_upper, is_included_tax, tax_rate);
        $('#CONTRACT_OA_EXCEED_UNIT_PRICE').val(refObj.exceedUnitPrice);
        $('#CONTRACT_OA_DEDUCTION_UNIT_PRICE').val(refObj.deductionUnitPrice);
    }

    // OnClick btnCalc_OP
    function OnClickBtnCalcOP() {
        var calculation_type = $('#CONTRACT_OP_OUTER_TIME_CALC_UNIT').val();
        var unit_price = iseiQ.utility.convertMoneyToInt($('#CONTRACT_OP_UNIT_PRICE').val());
        var base_time_lower = parseFloat($('#CONTRACT_OP_BASE_TIME_LOWER').val());
        var base_time_upper = parseFloat($('#CONTRACT_OP_BASE_TIME_UPPER').val());
        var is_included_tax = $('#IN_TAX_4').prop("checked");
        var tax_rate = $('#CONTRACT_OP_TAX_RATE').val();

        var refObj = { exceedUnitPrice: 0, deductionUnitPrice: 0 };
        ContractSES.CalcExceedDedutionUnitPrice(refObj, calculation_type, unit_price, base_time_lower, base_time_upper, is_included_tax, tax_rate);
        $('#CONTRACT_OP_EXCEED_UNIT_PRICE').val(refObj.exceedUnitPrice);
        $('#CONTRACT_OP_DEDUCTION_UNIT_PRICE').val(refObj.deductionUnitPrice);
    }

    function CalcExceedDedutionUnitPrice(refObj, calculation_type, unit_price, base_time_lower, base_time_upper)
    {
        switch (calculation_type) {
            case OuterTimeCalcUnitType.Normal:
                if (base_time_upper != 0) {
                    refObj.exceedUnitPrice = iseiQ.utility.convertIntToMoney(iseiQ.utility.roundByType(Constant.ROUND_TYPE.DOWN, unit_price / base_time_upper));
                }
                if (base_time_lower != 0) {
                    refObj.deductionUnitPrice = iseiQ.utility.convertIntToMoney(iseiQ.utility.roundByType(Constant.ROUND_TYPE.DOWN, unit_price / base_time_lower));
                }
                break;
            case OuterTimeCalcUnitType.Upper:
                if (base_time_upper != 0) {
                    refObj.exceedUnitPrice = iseiQ.utility.convertIntToMoney(iseiQ.utility.roundByType(Constant.ROUND_TYPE.DOWN, unit_price / base_time_upper));
                    refObj.deductionUnitPrice = iseiQ.utility.convertIntToMoney(iseiQ.utility.roundByType(Constant.ROUND_TYPE.DOWN, unit_price / base_time_upper));
                }
                break;
            case OuterTimeCalcUnitType.Middle:
                var average = (base_time_lower + base_time_upper) / 2
                if (average != 0) {
                    refObj.exceedUnitPrice = iseiQ.utility.convertIntToMoney(iseiQ.utility.roundByType(Constant.ROUND_TYPE.DOWN, unit_price / average));
                    refObj.deductionUnitPrice = iseiQ.utility.convertIntToMoney(iseiQ.utility.roundByType(Constant.ROUND_TYPE.DOWN, unit_price / average));
                }
                break;
            case OuterTimeCalcUnitType.Lower:
                if (base_time_lower != 0) {
                    refObj.exceedUnitPrice = iseiQ.utility.convertIntToMoney(iseiQ.utility.roundByType(Constant.ROUND_TYPE.DOWN, unit_price / base_time_lower));
                    refObj.deductionUnitPrice = iseiQ.utility.convertIntToMoney(iseiQ.utility.roundByType(Constant.ROUND_TYPE.DOWN, unit_price / base_time_lower));
                }
                break;
            case OuterTimeCalcUnitType.Other:
                refObj.exceedUnitPrice = 0;
                refObj.deductionUnitPrice = 0;
                break;
            default:
                refObj.exceedUnitPrice = 0;
                refObj.deductionUnitPrice = 0;
                break;
        }
    }

    //btnSelectCustomer
    function SelectCustomer() {
        // Set OrderAcceptance FLAG
        $('#isOrderAcceptance').val("1");
        CallSearchCustomer();
    }

    //btnSelectCustomerPIC
    function SelectCustomerPIC() {
        $('#isOrderAcceptance').val("1");
        var res = new Array();
        res.push({
            BP_NAME_DISP: $('#CONTRACT_OA_BP_DSP_NAME').val(),
            BP_SEQ_NO: $('#CONTRACT_OA_BP_SEQ_NO').val()
        });
        CallSearchPartnerStaff(res);
    }

    //btnSelectSupplier
    function SelectSupplier(control) {
        $('#isOrderAcceptance').val("2");
        $('#nameOpBpDspName').val($(control).parent('div').find('input.op-bp-dis-name').attr('name'));
        CallSearchCustomer();
    }

    //btnSelectSupPIC
    function SelectSupplierPIC(control) {
        $('#isOrderAcceptance').val("2");
        $('#nameOpBpDspName').val($(control).parent('div').find('input.op-bp-dis-name').attr('name'));
        var res = new Array();
        res.push({
            BP_NAME_DISP: $('#CONTRACT_OP_BP_DSP_NAME').val(),
            BP_SEQ_NO: $('#CONTRACT_OP_BP_SEQ_NO').val()
        });
        CallSearchPartnerStaff(res);
    }

    // Apply Only Customer
    function ApplyCustomer(res, result) {
        if (res === undefined || res === null) {
            /// Debug only
            console.log("Could not get customer.");
        } 
        else if ($('#isOrderAcceptance').val() == '1' && $("#CONTRACT_OA_BP_SEQ_NO").val() != iseiQ.utility.htmlDecode(res[0].BP_SEQ_NO)) 
        {
            // Display selected customer
            $("#CONTRACT_OA_BP_STAFF_NAME").val("");
            $("#CONTRACT_OA_BP_STAFF_SEQ_NO").val("");
            $("#CONTRACT_OA_BP_DSP_NAME").val(iseiQ.utility.htmlDecode(res[0].BP_NAME_DISP));
            $("#CONTRACT_OA_BP_SEQ_NO").val(iseiQ.utility.htmlDecode(res[0].BP_SEQ_NO));
            $('#CONTRACT_OA_PRIVATE_NOTE').val(iseiQ.utility.htmlDecode(result.BP_APT_PRIVATE_NOTE));
            $(".btnSelectCustomerPIC").removeAttr("disabled");
        }
        else if ($('#isOrderAcceptance').val() == '2' && $("#CONTRACT_OP_BP_SEQ_NO").val() != iseiQ.utility.htmlDecode(res[0].BP_SEQ_NO))
        {
            // Display selected customer
            $("#CONTRACT_OP_BP_STAFF_NAME").val("");
            $("#CONTRACT_OP_BP_STAFF_SEQ_NO").val("");
            $("#CONTRACT_OP_BP_DSP_NAME").val(iseiQ.utility.htmlDecode(res[0].BP_NAME_DISP));
            $("#CONTRACT_OP_BP_SEQ_NO").val(iseiQ.utility.htmlDecode(res[0].BP_SEQ_NO));
            $('#CONTRACT_OP_PRIVATE_NOTE').val(iseiQ.utility.htmlDecode(result.BP_PLC_PRIVATE_NOTE));
            $(".btnSelectSupplierPIC").removeAttr("disabled");
        }
    }

    // Apply Customer And PIC
    function ApplyPartnerStaff(res, result) {
        if (res === undefined || res === null) {
            /// Debug only
            console.log("Could not get BPStaff.");
        } else if ($('#isOrderAcceptance').val() == '1') {
            $("#CONTRACT_OA_BP_DSP_NAME").val(iseiQ.utility.htmlDecode(res[0].BP_NAME_DISP));
            $("#CONTRACT_OA_BP_SEQ_NO").val(iseiQ.utility.htmlDecode(res[0].BP_SEQ_NO));
            $("#CONTRACT_OA_BP_STAFF_NAME").val(iseiQ.utility.htmlDecode(res[0].BP_STAFF_NAME));
            $("#CONTRACT_OA_BP_STAFF_SEQ_NO").val(iseiQ.utility.htmlDecode(res[0].BP_STAFF_SEQ_NO));
            $('#CONTRACT_OA_PRIVATE_NOTE').val(iseiQ.utility.htmlDecode(result.BP_APT_PRIVATE_NOTE));
            $(".btnSelectCustomerPIC").removeAttr("disabled");
        } else {
            $("#CONTRACT_OP_BP_DSP_NAME").val(iseiQ.utility.htmlDecode(res[0].BP_NAME_DISP));
            $("#CONTRACT_OP_BP_SEQ_NO").val(iseiQ.utility.htmlDecode(res[0].BP_SEQ_NO));
            $("#CONTRACT_OP_BP_STAFF_NAME").val(iseiQ.utility.htmlDecode(res[0].BP_STAFF_NAME));
            $("#CONTRACT_OP_BP_STAFF_SEQ_NO").val(iseiQ.utility.htmlDecode(res[0].BP_STAFF_SEQ_NO));
            $('#CONTRACT_OP_PRIVATE_NOTE').val(iseiQ.utility.htmlDecode(result.BP_PLC_PRIVATE_NOTE));
            $(".btnSelectSupplierPIC").removeAttr("disabled");
        }
    }

    ///////////////////////////////////////////////////////////////////
    // private functions
    function ControlAdjTypeElements(idprefix, disabled) {
        $('#CONTRACT_' + idprefix + '_PAYMENT_METHOD_TYPE').attr("disabled", disabled);
        $('#CONTRACT_' + idprefix + '_OUTER_TIME_CALC_UNIT').attr("disabled", disabled);
        $('#CONTRACT_' + idprefix + '_BASE_TIME_LOWER').attr("disabled", disabled);
        $('#CONTRACT_' + idprefix + '_BASE_TIME_UPPER').attr("disabled", disabled);
        $('.btnCalc_' + idprefix).attr("disabled", disabled);
        $('#CONTRACT_' + idprefix + '_EXCEED_UNIT_PRICE').attr("disabled", disabled);
        $('#CONTRACT_' + idprefix + '_DEDUCTION_UNIT_PRICE').attr("disabled", disabled);
        //$('#CONTRACT_' + idprefix + '_UNIT_TIME').attr("disabled", disabled);
        $('#CONTRACT_' + idprefix + '_UNIT_PRICE').attr("disabled", disabled);
        $('#CONTRACT_' + idprefix + '_QTY').attr("disabled", disabled);
        //$('#CONTRACT_' + idprefix + '_CONT_AMOUNT').attr("disabled", disabled);
    }

    function controlAdjTypeElementsVisibility($target, isVisibility) {
        var opacity = isVisibility ? '1' : '0';
        $target.find('.calc-unit').css('opacity', opacity);
        $target.find('.base-time-lower').css('opacity', opacity);
        $target.find('.base-time-upper').css('opacity', opacity);
        $target.find('.exceed-unit-price').css('opacity', opacity);
        $target.find('.deduction-unit-price').css('opacity', opacity);
        $target.find('.lbUnit').css('opacity', opacity);

        $target.find('.calc-unit').prop('disabled', !isVisibility);
        $target.find('.base-time-lower').prop('disabled', !isVisibility);
        $target.find('.base-time-upper').prop('disabled', !isVisibility);
        $target.find('.exceed-unit-price').prop('disabled', !isVisibility);
        $target.find('.deduction-unit-price').prop('disabled', !isVisibility);
        $target.find('.lbUnit').prop('disabled', !isVisibility);
    }

    function resetArrCoad() {
        var $parentElement = $('.special-sales-content');
        var $tbLeftRows = $parentElement.find('.col-left tbody tr');

        for (var i = 0; i < $tbLeftRows.length; i++) {
            var prefix = 'CONTRACT_OAD[' + i + '].';
            var $tbLeftTargetRow = $($parentElement.find('.col-left tbody tr')[i]);
            var $tbCenterTargetRow = $($parentElement.find('.col-center tbody tr')[i]);
            var $tbRightTargetRow = $($parentElement.find('.col-right tbody tr')[i]);


            $tbLeftTargetRow.find('.cont-seq-no').attr('name', prefix + 'CONT_SEQ_NO');
            $tbLeftTargetRow.find('.cont-apt-seq-no').attr('name', prefix + 'CONT_APT_SEQ_NO');
            $tbLeftTargetRow.find('.dsp-order').attr('name', prefix + 'DSP_ORDER');
            $tbLeftTargetRow.find('.target-ym').attr('name', prefix + 'TARGET_YM');

            $tbCenterTargetRow.find('.coad-payment-method-type').attr('name', prefix + 'PAYMENT_METHOD_TYPE');
            $tbCenterTargetRow.find('.unit-price').attr('name', prefix + 'UNIT_PRICE');
            $tbCenterTargetRow.find('.qty').attr('name', prefix + 'QTY');
            $tbCenterTargetRow.find('.amount').attr('name', prefix + 'AMOUNT');
            $tbCenterTargetRow.find('.adjustment-condition').attr('name', prefix + 'ADJUSTMENT_CONDITION');
            $tbCenterTargetRow.find('.calc-unit').attr('name', prefix + 'OUTER_TIME_CALC_UNIT');
            $tbCenterTargetRow.find('.base-time-lower').attr('name', prefix + 'BASE_TIME_LOWER');
            $tbCenterTargetRow.find('.base-time-upper').attr('name', prefix + 'BASE_TIME_UPPER');
            $tbCenterTargetRow.find('.exceed-unit-price').attr('name', prefix + 'EXCEED_UNIT_PRICE');
            $tbCenterTargetRow.find('.deduction-unit-price').attr('name', prefix + 'DEDUCTION_UNIT_PRICE');
            $tbCenterTargetRow.find('.unit-time').attr('name', prefix + 'UNIT_TIME');

            $($parentElement.find('.col-right tbody tr')[i]).find('.del-flg').attr('name', prefix + 'DEL_FLG');
        }
    }

    function resetArrCopd() {
        var $parentElement = $('.special-payment-content');
        var $tbLeftRows = $parentElement.find('.col-left tbody tr');

        for (var i = 0; i < $tbLeftRows.length; i++) {
            var prefix = 'CONTRACT_OPD[' + i + '].';
            var $tbLeftTargetRow = $($parentElement.find('.col-left tbody tr')[i]);
            var $tbCenterTargetRow = $($parentElement.find('.col-center tbody tr')[i]);
            var $tbRightTargetRow = $($parentElement.find('.col-right tbody tr')[i]);


            $tbLeftTargetRow.find('.cont-seq-no').attr('name', prefix + 'CONT_SEQ_NO');
            $tbLeftTargetRow.find('.cont-plc-seq-no').attr('name', prefix + 'CONT_PLC_SEQ_NO');
            $tbLeftTargetRow.find('.dsp-order').attr('name', prefix + 'DSP_ORDER');
            $tbLeftTargetRow.find('.target-ym').attr('name', prefix + 'TARGET_YM');

            $tbCenterTargetRow.find('.copd-payment-method-type').attr('name', prefix + 'PAYMENT_METHOD_TYPE');
            $tbCenterTargetRow.find('.unit-price-copd').attr('name', prefix + 'UNIT_PRICE');
            $tbCenterTargetRow.find('.qty-copd').attr('name', prefix + 'QTY');
            $tbCenterTargetRow.find('.amount-copd').attr('name', prefix + 'AMOUNT');
            $tbCenterTargetRow.find('.adjustment-condition-copd').attr('name', prefix + 'ADJUSTMENT_CONDITION');
            $tbCenterTargetRow.find('.calc-unit-copd').attr('name', prefix + 'OUTER_TIME_CALC_UNIT');
            $tbCenterTargetRow.find('.base-time-lower-copd').attr('name', prefix + 'BASE_TIME_LOWER');
            $tbCenterTargetRow.find('.base-time-upper-copd').attr('name', prefix + 'BASE_TIME_UPPER');
            $tbCenterTargetRow.find('.exceed-unit-price-copd').attr('name', prefix + 'EXCEED_UNIT_PRICE');
            $tbCenterTargetRow.find('.deduction-unit-price-copd').attr('name', prefix + 'DEDUCTION_UNIT_PRICE');
            $tbCenterTargetRow.find('.unit-time-copd').attr('name', prefix + 'UNIT_TIME');

            $($parentElement.find('.col-right tbody tr')[i]).find('.del-flg').attr('name', prefix + 'DEL_FLG');
        }
    }
}());