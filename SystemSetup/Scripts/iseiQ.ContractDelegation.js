/*!
 * File: iseiQ.ContractDelegation.js
 * Company: i-Enter Asia
 * Copyright © 2015 i-Enter Asia. All rights reserved.
 * Project: iseiQ
 * Author: TrungNT
 * Created date: 2015/06/23
 */

var ContractDelegation = (function () {
    return {
        ValidateData: ValidateData,

        AddCoai: AddCoai,
        DeleteCoai: DeleteCoai,
        CalcAmountCoai: CalcAmountCoai,

        DeleteCoad: DeleteCoad,
        DeleteCopd: DeleteCopd,
        AddCoad: AddCoad,
        AddCopd: AddCopd,

        CalcAmountCoad: CalcAmountCoad,
        OnClickBtnCalcCoad: OnClickBtnCalcCoad,

        CalcAmountCopd: CalcAmountCopd,
        OnClickBtnCalcCopd: OnClickBtnCalcCopd,
        CalcTotalAmountOP: CalcTotalAmountOP,
        CalcProfit: CalcProfit,

        OnClickBtnCalcOA: OnClickBtnCalcOA,
        OnClickBtnCalcOP: OnClickBtnCalcOP,

        AddSupplier: AddSupplier,
        DeleteSupplier: DeleteSupplier,
        SelectCustomer: SelectCustomer,
        SelectCustomerPIC: SelectCustomerPIC,
        SelectSupplier: SelectSupplier,
        SelectSupplierPIC: SelectSupplierPIC,
        ApplyCustomer: ApplyCustomer,
        ApplyPartnerStaff: ApplyPartnerStaff,

        changeCoadAdjType: changeCoadAdjType,
        changeCopdAdjType: changeCopdAdjType
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

        // Project Name
        var prjNameDesc = $('#CONTRACT_PRJ_DESC_NAME').val();
        if (prjNameDesc.length === 0) // required Project name
            invalidMess.push(Constant.PROJECT_DETAIL_NAME + Constant.ERR_REQUIRED);
        else if (prjNameDesc.length > Constant.MAX_PRJ_NAME) // valid string length
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.PROJECT_DETAIL_NAME).replace('{1}', Constant.MAX_PRJ_NAME));

        // Product Name
        $('.nomen-coai').each(function () {
            var productName = $(this).val();
            if (productName.length === 0) {
                invalidMess.push(Constant.PRODUCT_NAME + Constant.ERR_REQUIRED);
                return;
            } else if (productName.length > Constant.MAX_PRODUCT_NAME) {
                invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.PRODUCT_NAME).replace('{1}', Constant.MAX_PRODUCT_NAME));
                return;
            }
        });

        // Validate unit price
        $('.unit-price-coai').each(function () {
            var unitprice = $(this).val().replace(/,/g, '');
            if (unitprice.length === 0) {
                invalidMess.push(Constant.UNIT_PRICE + Constant.ERR_REQUIRED);
                return;
            } else if ((parseInt(unitprice) < Constant.MIN_UNIT_PRICE) || (parseInt(unitprice) > Constant.MAX_UNIT_PRICE)) {
                invalidMess.push(Constant.UNIT_PRICE + Constant.ERR_RANGE_UNIT_PRICE);
                return;
            }
        });

        // Validate quantity
        $('.qty-coai').each(function () {
            var quantity = $(this).val();
            if (quantity.length === 0) {
                invalidMess.push(Constant.QUANTITY + Constant.ERR_REQUIRED);
                return;
            } else if ((quantity < Constant.MIN_QUANTITY) || (quantity > Constant.MAX_QUANTITY)) {
                invalidMess.push(Constant.QUANTITY + Constant.ERR_RANGE_QUANTITY);
                return;
            }
            else if (quantity === 0) {
                invalidMess.push(Constant.QUANTITY + Constant.ERR_SPECIAL_QUANTITY);
                return;
            }
        });

        // Validate unit type
        $('.unit-type-coai').each(function () {
            var unitType = $(this).val();
            if (unitType.length === 0) {
                invalidMess.push(Constant.UNIT_TYPE + Constant.ERR_REQUIRED);
                return;
            }
        });

        // Validate amount
        $('.amount-coai').each(function () {
            var Amount = $(this).val().replace(/,/g, '');
            if (Amount.length === 0)
            {
                invalidMess.push(Constant.AMOUNT + Constant.ERR_REQUIRED);
                return false;
            }
            else if ((parseInt(Amount) < Constant.MIN_AMOUNT) || (parseInt(Amount) > Constant.MAX_AMOUNT))
            {
                invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.AMOUNT).replace('{1}', Constant.MAX_AMOUNT_LENGTH));
            }
        });

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
        else if (startDate.length > 0) {
            errStartDate = iseiQ.utility.validDate(startDate, Constant.DATE_FORMAT, Constant.WORK_PERIOD_START);
            if (errStartDate != null) {
                existInvalidDate = true;
                invalidMess.push(errStartDate);
            }
        }
        else if (errStartDate == null && startDate.length > Constant.MAX_DATE) { // valid string length
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
        else if (endDate.length > 0) {
            var errEndDate = iseiQ.utility.validDate(endDate, Constant.DATE_FORMAT, Constant.WORK_PERIOD_END);
            if (errEndDate != null) {
                existInvalidDate = true;
                invalidMess.push(errEndDate);
            }
        }
        else if (errEndDate == null && endDate.length > Constant.MAX_DATE) {
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

        // 22667.15
        if ($('#CONTRACT_OA_PAYMENT_METHOD_TYPE').val() == PaymentMethodType.Duration)
        {
            // Base Time lower
            var basetimelower = $('#CONTRACT_OA_BASE_TIME_LOWER').val().trim();
            if (basetimelower.length > Constant.MAX_BASE_TIME) // valid string length
                invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.BASE_TIME_LOWER).replace('{1}', Constant.MAX_BASE_TIME));

            // Base time upper
            var basetimeupper = $('#CONTRACT_OA_BASE_TIME_UPPER').val().trim();
            if (basetimeupper.length > Constant.MAX_BASE_TIME) // valid string length
                invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.BASE_TIME_LOWER).replace('{1}', Constant.MAX_BASE_TIME));

            var paymentMethodType = $('#CONTRACT_OA_PAYMENT_METHOD_TYPE').val();
            var errCompareBase = iseiQ.utility.checkCompareBasetime(basetimelower, basetimeupper, paymentMethodType);
            if (errCompareBase) invalidMess.push(errCompareBase);

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
        if (closingDayOA == 0) {
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

        ////////////////////////////////////////////////////////////////////////////////
        // >> Validate div-coad
        // Validate target ym
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
                    if (errDuplicateYmCoad == null) {
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
                    // validate deposit plan date
                    var depositPlanDate = $($specialSalesDataRowsCenter[index]).find('input.deposit-plan-date-hdd').val();
                    if (depositPlanDate.length === 0) {
                        invalidMess.push(Constant.DEPOSIT_PLAN_DATE + Constant.ERR_REQUIRED);
                    } else if (depositPlanDate.length > 10) { // valid string length
                        //existInvalidDate = true;
                        invalidMess.push(Constant.ERR_FORMAT_DATE.replace('{0}', Constant.DEPOSIT_PLAN_DATE));
                    } else { // validate start date
                        var errDepositPlanDate = iseiQ.utility.validDate(depositPlanDate, Constant.DATE_FORMAT, Constant.DEPOSIT_PLAN_DATE);
                        if (errDepositPlanDate != null) {
                            invalidMess.push(errDepositPlanDate);
                        }
                    }

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

                    if ($('#CONTRACT_OA_PAYMENT_METHOD_TYPE').val() == PaymentMethodType.Duration)
                    {
                        var basetimelowerCoad = $($specialSalesDataRowsCenter[index]).find('input.base-time-lower').val();
                        var basetimeupperCoad = $($specialSalesDataRowsCenter[index]).find('input.base-time-upper').val();
                        var paymentMethodType = $('#CONTRACT_OA_PAYMENT_METHOD_TYPE').val();
                        var errCompareBase = iseiQ.utility.checkCompareBasetime(basetimelowerCoad, basetimeupperCoad, paymentMethodType);
                        if (errCompareBase) invalidMess.push(errCompareBase);
                    }

                    if ($('#CONTRACT_OA_PAYMENT_METHOD_TYPE').val() == PaymentMethodType.Duration
                        || $('#CONTRACT_OA_PAYMENT_METHOD_TYPE').val() == PaymentMethodType.Hour)
                    {
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
        // >> Supplier validation
        if ($('#COUNT_SUPPLIERS').val() > 0)
        {
            $('div.div-suppliers').each(function () {
                var divParent = $(this);
                // Supplier
                var supplierDspName = divParent.find('.op-bp-dis-name').val().trim();
                if (supplierDspName.length === 0) // required customer name
                    invalidMess.push(Constant.SUPPLIER_NAME + Constant.ERR_REQUIRED);

                // Supplier PIC
                var supplierPicName = divParent.find('.op-bp-staff-name').val().trim();
                if (supplierPicName.length === 0) // required customer name
                    invalidMess.push(Constant.SUPPLIER_PIC + Constant.ERR_REQUIRED);

                // Supplier Staff Id
                var staffIdOp = divParent.find('.ddl-staff-id').val();
                if (staffIdOp == null) // required Staff name
                    invalidMess.push(Constant.STAFF_ID_OP + Constant.ERR_REQUIRED);

                if ($('.op-payment-method-type').val() == PaymentMethodType.Duration)
                {
                    // Base Time lower
                    var basetimelower = divParent.find('.base-time-lower').val().trim();
                    if (basetimelower.length > Constant.MAX_BASE_TIME) // valid string length
                        invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.BASE_TIME_LOWER).replace('{1}', Constant.MAX_BASE_TIME));

                    // Base time upper
                    var basetimeupper = divParent.find('.base-time-upper').val().trim();
                    if (basetimeupper.length > Constant.MAX_BASE_TIME) // valid string length
                        invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.BASE_TIME_UPPER).replace('{1}', Constant.MAX_BASE_TIME));

                    var paymentMethodType = $('.op-payment-method-type').val();
                    var errCompareBase = iseiQ.utility.checkCompareBasetime(basetimelower, basetimeupper, paymentMethodType);
                    if (errCompareBase) invalidMess.push(errCompareBase);

                    // Exceed unit price
                    var exceedUnitPrice = divParent.find('.exceed-unit-price').val().replace(/,/g, '');
                    if (exceedUnitPrice.length > Constant.MAX_EXCEED_UNIT) // valid string length
                        invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.EXCEED_UNIT).replace('{1}', Constant.MAX_EXCEED_UNIT));

                    // Deduction unit price
                    var deductionUnitPrice = divParent.find('.deduction-unit-price').val().replace(/,/g, '');
                    if (deductionUnitPrice.length > Constant.MAX_EXCEED_UNIT) {
                        invalidMess.push(Constant.DEDUCTION_UNIT + Constant.ERR_RANGE_UNIT_PRICE);
                    }
                }
                
                if ($('.op-payment-method-type').val() == PaymentMethodType.Duration
                    || $('.op-payment-method-type').val() == PaymentMethodType.Hour)
                {
                    // Unit Time OP
                    var UnitTimeOP = divParent.find('.unit-time-op').val();
                    if ((UnitTimeOP < Constant.UNIT_TIME_DATA_MIN) || (UnitTimeOP > Constant.UNIT_TIME_DATA_MAX)) {
                        invalidMess.push(Constant.UNIT_TIME + Constant.ERR_RANGE_VALUE_UNITTIME);
                    }
                }
                
                // Unit Price OP
                var UnitPriceOP = divParent.find('.unit-price-op').val().replace(/,/g, '');
                if (UnitPriceOP.length === 0) {
                    invalidMess.push(Constant.UNIT_PRICE + Constant.ERR_REQUIRED);
                } else if ((parseInt(UnitPriceOP) < Constant.MIN_UNIT_PRICE) || (parseInt(UnitPriceOP) > Constant.MAX_UNIT_PRICE)) {
                    invalidMess.push(Constant.UNIT_PRICE + Constant.ERR_RANGE_UNIT_PRICE);
                }

                // Tax rate
                var TaxRate = divParent.find('.tax-rate-op').val();
                if (TaxRate.length === 0) {
                    invalidMess.push(Constant.TAXRATE + Constant.ERR_REQUIRED);
                } else if (TaxRate.length > Constant.MAX_TAXRATE) {// valid string length
                    invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.TAXRATE).replace('{1}', Constant.MAX_TAXRATE));
                }                

                // Closing day
                var closingDayOP = divParent.find('.hdd-closing-day-op').val();
                if (closingDayOP == 0) {
                    invalidMess.push(Constant.CLOSING_DAY + Constant.ERR_REQUIRED_CHOOSE);
                }

                // Payment site type
                var paymentSiteTypeOP = divParent.find('.op-payment-site-type').val();
                if (paymentSiteTypeOP == 0) {
                    invalidMess.push(Constant.PAYMENT_SITE_TYPE + Constant.ERR_REQUIRED_CHOOSE);
                }

                // Payment day
                var paymentDayOP = divParent.find('.op-payment-day').val();
                if (paymentDayOP == 0) {
                    invalidMess.push(Constant.PAYMENT_DAY + Constant.ERR_REQUIRED_CHOOSE);
                }

                // Closing day cannot bigger than payment day. Refs #28177
                if (PaymentSiteType.ThisMonth === paymentSiteTypeOP && parseInt(closingDayOP) > parseInt(paymentDayOP)) {
                    invalidMess.push(Constant.ERR_CLOSING_DAY_BIGGER_THAN_PAYMENT_DAY);
                }

                //var divCopdList = divParent.find('div.div-copd');
                var $specialPaymentContent = divParent.find('.special-payment-content');
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
                            $targetYm.parents('tbody').children('tr:not(.hide)').find('.target-ym').each(function () {
                                if (!$(this).is($targetYm)) {
                                    var currentTargetYM = $(this).val();
                                    if (iseiQ.utility.validDate(currentTargetYM, Constant.YM_FORMAT, Constant.TARGET_YM) == null
                                        && currentTargetYM == $targetYm.val()
                                        && errDuplicateYmCopd == null) {
                                        errDuplicateYmCopd = Constant.ERR_TARGETYM_DUPLICATE;
                                        invalidMess.push(errDuplicateYmCopd);
                                    }
                                }
                            });
                        }

                        if ($targetYm.val().length > 0) {
                            // validate deposit plan date
                            var paymentPlanDate = $($specialPaymentDataRowsCenter[index]).find('input.payment-plan-date-hdd').val();
                            if (paymentPlanDate.length === 0) {
                                invalidMess.push(Constant.PAYMENT_PLAN_DATE + Constant.ERR_REQUIRED);
                            } else if (paymentPlanDate.length > 10) { // valid string length
                                //existInvalidDate = true;
                                invalidMess.push(Constant.ERR_FORMAT_DATE.replace('{0}', Constant.PAYMENT_PLAN_DATE));
                            } else { // validate start date
                                var errDepositPlanDate = iseiQ.utility.validDate(paymentPlanDate, Constant.DATE_FORMAT, Constant.PAYMENT_PLAN_DATE);
                                if (errDepositPlanDate != null) {
                                    invalidMess.push(errDepositPlanDate);
                                }
                            }
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

                            if ($('.op-payment-method-type').val() == PaymentMethodType.Duration)
                            {
                                var basetimelowerCopd = $($specialPaymentDataRowsCenter[index]).find('input.base-time-lower-copd').val();
                                var basetimeupperCopd = $($specialPaymentDataRowsCenter[index]).find('input.base-time-upper-copd').val();
                                var paymentMethodType = $('.op-payment-method-type').val();
                                var errCompareBase = iseiQ.utility.checkCompareBasetime(basetimelowerCopd, basetimeupperCopd, paymentMethodType);
                                if (errCompareBase) invalidMess.push(errCompareBase);
                            }

                            if ($('.op-payment-method-type').val() == PaymentMethodType.Duration
                                || $('.op-payment-method-type').val() == PaymentMethodType.Hour)
                            {
                                // Validate unit time
                                var UnitTimeCopd = $($specialPaymentDataRowsCenter[index]).find('input.unit-time-copd').val();
                                if (UnitTimeCopd.length === 0) {
                                    invalidMess.push(Constant.UNIT_TIME + Constant.ERR_REQUIRED);
                                } else if ((UnitTimeCopd < Constant.UNIT_TIME_DATA_MIN) || (UnitTimeCopd > Constant.UNIT_TIME_DATA_MAX)) {
                                    invalidMess.push(Constant.UNIT_TIME + Constant.ERR_RANGE_VALUE_UNITTIME);
                                }
                            }                                                       
                        }
                    });
                }

                // Placement Note
                //var plcNote = divParent.find('.plc-note-op').val();
                //if (plcNote.length > Constant.MAX_NOTE) // valid string length
                //    invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.NOTE).replace('{1}', Constant.MAX_NOTE));
            });
        }
       
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
    
    // COAI >>
    // OnClick .btnAddOAI
    function AddCoai() {
        var $parentElement = $('div.div-coai');
        $parentElement.find('tbody tr').last().after($parentElement.find('tbody tr').first().prop('outerHTML'));
        var $TargetNew = $parentElement.find('tbody tr').last();
        $TargetNew.show();
        $TargetNew.find('.btnDeleteOAI').show();
        ResetInitValueCoai($TargetNew);
        resetArrCoai();
    }

    function ResetInitValueCoai(addedTarget) {
        addedTarget.find('input.cont-seq-no-coai').attr('title', '').attr('value', 0);
        addedTarget.find('input.cont-apt-seq-no-coai').attr('title', '').attr('value', 0);
        addedTarget.find('input.dsp-order-coai').attr('title', '').attr('value', 0);
        addedTarget.find('input.product-seq-no-coai').attr('title', '').attr('value', 0);
        addedTarget.find('input.item-seq-no-coai').attr('title', '').attr('value', 0);
        addedTarget.find('input.nomen-coai').attr('title', '').attr('value', "");
        addedTarget.find('input.qty-coai').attr('title', '').attr('value', 1);
        addedTarget.find('input.unit-type-coai').attr('title', '').attr('value', '式');
        addedTarget.find('input.amount-coai').attr('title', '').attr('value', 0);
        addedTarget.find('input.unit-price-coai').attr('title', '').attr('value', 0);
        addedTarget.find('input.del-flg').attr('title', '').attr('value', 0);
    }

    //OnClick .btnDeleteOAI
    function DeleteCoai(control) {
        var $targetDel = $(control).parents('tr');
        if ($targetDel.find('.cont-seq-no-coai').val() == 0) {
            $targetDel.remove();
            resetArrCoai();
        }
        else {
            $targetDel.hide();
            $targetDel.find('.del-flg').val(1);
        }
        CalcAmountCoai(control);
        Contract.OnChangeAmountCoai();
    }

    // OnChange .unit-price-coai , qty-coai
    function CalcAmountCoai(control) {
        var $parentElement = $(control).parents('tr');
        var price = $parentElement.find('.unit-price-coai').val();
        var quantity = $parentElement.find('.qty-coai').val();
        //if (price.length > 0 && quantity.length > 0) {
            var unitprice = iseiQ.utility.convertMoneyToInt(price,true);
            var amount = iseiQ.utility.roundByType($('#RoundingType').val(),unitprice * quantity);

            $parentElement.find('.amount-coai').val(iseiQ.utility.convertIntToMoney(amount));
        //}
        OnChangeAmountCoai();
    }

    // OnChange .amount-coai
    function OnChangeAmountCoai() {
        var total = 0;
        $('.amount-coai').each(function () {
            if ($(this).parents('tr').css('display') !== 'none') {
                total += parseInt($(this).val().replace(/,/g, ''));
            }
        });
        $('#CONTRACT_CONT_AMOUNT_TOTAL').val(iseiQ.utility.convertIntToMoney(total));
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

    // OnClick .btnDeleteCoad
    function DeleteCoad(control) {
        var $parentElement = $('.special-sales-content');
        var $tbRightTargetRow = $(control).parents('tr');
        var index = $tbRightTargetRow.index();
        var $tbLeftTargetRow = $($parentElement.find('.col-left tbody tr')[index]);
        var $tbCenterTargetRow = $($parentElement.find('.col-center tbody tr')[index]);

        if ($tbLeftTargetRow.find('.cont-seq-no').val() == 0) {
            if ($parentElement.find('.col-left tbody tr').length > 1) {
                $tbLeftTargetRow.remove();
                $tbCenterTargetRow.remove();
                $tbRightTargetRow.remove();

                resetArrCoad();
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

    // OnClick .btnAddCoad
    function AddCoad() {
        var $parentElement = $('.special-sales-content');
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
        ResetInitValueCoad($parentElement);
        resetArrCoad();
        iseiQ.utility.InitDatepickerMonths();
        iseiQ.utility.InitDatepickerDays();
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

    // OnClick btnCalc_OP
    function OnClickBtnCalcOP(control) {
        var supplier = $(control).parents('div.div-suppliers');

        var calculation_type = supplier.find('.calc-unit').val();
        var unit_price = iseiQ.utility.convertMoneyToInt(supplier.find('.unit-price-op').val());
        var base_time_lower = parseFloat(supplier.find('.base-time-lower').val());
        var base_time_upper = parseFloat(supplier.find('.base-time-upper').val());
        var average = (base_time_lower + base_time_upper) / 2;
        var is_included_tax = $('#IN_TAX_4').prop("checked");
        var tax_rate = $('.tax-rate-op').val();

        var refObj = { exceedUnitPrice: 0, deductionUnitPrice: 0 };
        ContractSES.CalcExceedDedutionUnitPrice(refObj, calculation_type, unit_price, base_time_lower, base_time_upper, is_included_tax, tax_rate);
        supplier.find('.exceed-unit-price').val(refObj.exceedUnitPrice);
        supplier.find('.deduction-unit-price').val(refObj.deductionUnitPrice);
    }

    // OnClick .btnAddCopd
    function AddCopd(control) {
        var $parentElement = $(control).parents('.div-suppliers').find('.special-payment-content');
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

        resetArrCopd(control);
        iseiQ.utility.InitDatepickerMonths();
        iseiQ.utility.InitDatepickerDays();
    }

    // OnClick .btnDeleteCopd
    function DeleteCopd(control) {
        var $parentElement = $(control).parents('div.special-payment-content');
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
    function OnClickBtnCalcCopd(control) {
        var $clickedButton = $('.special-payment-content .btnCalc_OPD_clicked').removeClass('btnCalc_OPD_clicked');
        var $parentElement = $clickedButton.parents('tr');
        var calculation_type = $parentElement.find('.calc-unit-copd').val();
        //var amount = iseiQ.utility.convertMoneyToInt($parentElement.find('.amount-copd').val());
        var unit_price = iseiQ.utility.convertMoneyToInt($parentElement.find('.unit-price-copd').val());
        var base_time_lower = parseFloat($parentElement.find('.base-time-lower-copd').val());
        var base_time_upper = parseFloat($parentElement.find('.base-time-upper-copd').val());
        var is_included_tax = $('#IN_TAX_4').prop("checked");
        var tax_rate = $('.tax-rate-op').val();

        var refObj = { exceedUnitPrice: 0, deductionUnitPrice: 0 };
        ContractSES.CalcExceedDedutionUnitPrice(refObj, calculation_type, unit_price, base_time_lower, base_time_upper, is_included_tax, tax_rate);
        $parentElement.find('.exceed-unit-price-copd').val(refObj.exceedUnitPrice);
        $parentElement.find('.deduction-unit-price-copd').val(refObj.deductionUnitPrice);
    }

    //Add Supplier
    function AddSupplier() {
        if ($('div.div-suppliers:not(.hide)').length > 0) {
            $('.in-tax-flg-cb').each(function () {
                $(this).removeAttr('name');
            });
            $('div.div-suppliers').last().after($('div.div-suppliers').first().prop('outerHTML'));
            var $parentElement = $('div.div-suppliers').last();

            var $tbLeftFirstRow = $parentElement.find('.col-left tbody tr:first');
            var $tbCenterFirstRow = $parentElement.find('.col-center tbody tr:first');
            var $tbRightFirstRow = $parentElement.find('.col-right tbody tr:first');

            $parentElement.find('.col-left tbody tr').remove();
            $parentElement.find('.col-center tbody tr').remove();
            $parentElement.find('.col-right tbody tr').remove();

            $parentElement.find('.col-left tbody').html($tbLeftFirstRow.prop('outerHTML'));
            $parentElement.find('.col-center tbody').html($tbCenterFirstRow.prop('outerHTML'));
            $parentElement.find('.col-right tbody').html($tbRightFirstRow.prop('outerHTML'));

            $parentElement.find('.col-left tbody tr:first').addClass('hide');
            $parentElement.find('.col-center tbody tr:first').addClass('hide');
            $parentElement.find('.col-right tbody tr:first').addClass('hide');

            if ($parentElement.find('.btnSelectSupplier').length == 0) {
                $parentElement.find('.op-bp-dis-name').after('<button class="btnSelectSupplier btn btn-default" type="button"><i class="btn-icon btn-search-gray"></i></button>');
            }
            $parentElement.find('.btnSelectSupplierPIC').prop('disabled', true);
            ResetInitValueOp($parentElement);
            ResetSuppliersArr();
            $parentElement.find('.divDeleteSupplier').removeClass('hide');
            $parentElement.find('.op-payment-method-type').change();
        }
        else {
            $('div.div-suppliers').removeClass('hide');
            $('div.div-suppliers').find('.divDeleteSupplier').removeClass('hide');
            var $parentElement = $('div.div-suppliers').last();
            ResetInitValueOp($parentElement);
        }
        $('#COUNT_SUPPLIERS').val(parseInt($('#COUNT_SUPPLIERS').val()) + 1);
        $('.btnAddSupplier').prop("disabled", true);
    }

    function DeleteSupplier(control) {
        if ($('div.div-suppliers').length > 1)
        {
            $(control).parents('.div-suppliers').remove();
            ResetSuppliersArr();
            $(control).parents('.div-suppliers').find('.op-payment-method-type').change();
        }
        else
        {
            $('div.div-suppliers').addClass('hide');
            ResetInitValueOp($(control).parents('.div-suppliers'));
            ResetSuppliersArr();
            $('div.div-suppliers').find('.op-payment-method-type').change();
            var $specialPayment = $('.div-suppliers').find('.special-payment-content');
            $specialPayment.find('.col-left tbody tr:first').addClass('hide');
            $specialPayment.find('.col-center tbody tr:first').addClass('hide');
            $specialPayment.find('.col-right tbody tr:first').addClass('hide');
            $specialPayment.find('.col-left tbody tr:not(.hide)').remove();
            $specialPayment.find('.col-center tbody tr:not(.hide)').remove();
            $specialPayment.find('.col-right tbody tr:not(.hide)').remove();
            ResetInitValueCopd($specialPayment);
        }
        $('#COUNT_SUPPLIERS').val(parseInt($('#COUNT_SUPPLIERS').val()) - 1);
        CalcTotalAmountOP();
        $('.btnAddSupplier').prop("disabled", false);
    }

    //////////////////////////////////////////////////////////////////////
    // Apply functions
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
        // Set OrderAcceptance FLAG
        $('#isOrderAcceptance').val("2");
        $('#nameOpBpDspName').val($(control).parents('div.div-suppliers').find('input.op-bp-dis-name').attr('name'));
        CallSearchCustomer();
    }

    //btnSelectSupplierPIC
    function SelectSupplierPIC(control) {
        $('#isOrderAcceptance').val("2");
        $('#nameOpBpDspName').val($(control).parents('div.div-suppliers').find('input.op-bp-dis-name').attr('name'));

        var res = new Array();
        res.push({
            BP_NAME_DISP: $(control).parents('div.div-suppliers').find('input.op-bp-dis-name').val(),
            BP_SEQ_NO: $(control).parents('div.div-suppliers').find('input.op-bp-seq-no').val()
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
        else 
        {
            var nameOpBpDspName = $('#nameOpBpDspName').val();
            var divSupplier = $('input[name="' + nameOpBpDspName + '"]').parents('div.div-suppliers');
            if (divSupplier.find('.op-bp-seq-no').val() == '2' && divSupplier.find('.op-bp-seq-no').val() != iseiQ.utility.htmlDecode(res[0].BP_SEQ_NO))
            {
                // Display selected customer
                divSupplier.find('.op-bp-staff-name').val("");
                divSupplier.find('.op-bp-staff-seq-no').val("");
                divSupplier.find('.op-bp-dis-name').val(iseiQ.utility.htmlDecode(res[0].BP_NAME_DISP));
                divSupplier.find('.op-bp-seq-no').val(iseiQ.utility.htmlDecode(res[0].BP_SEQ_NO));
                divSupplier.find('.prv-note-op ').val(iseiQ.utility.htmlDecode(result.BP_PLC_PRIVATE_NOTE));
                divSupplier.find('.btnSelectSupplierPIC').removeAttr("disabled");
            }            
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
            var nameOpBpDspName = $('#nameOpBpDspName').val();
            var divSupplier = $('input[name="' + nameOpBpDspName + '"]').parents('div.div-suppliers');

            divSupplier.find('.op-bp-staff-name').val(iseiQ.utility.htmlDecode(res[0].BP_STAFF_NAME));
            divSupplier.find('.op-bp-staff-seq-no').val(iseiQ.utility.htmlDecode(res[0].BP_STAFF_SEQ_NO));
            divSupplier.find('.op-bp-dis-name').val(iseiQ.utility.htmlDecode(res[0].BP_NAME_DISP));
            divSupplier.find('.op-bp-seq-no').val(iseiQ.utility.htmlDecode(res[0].BP_SEQ_NO));
            divSupplier.find('.prv-note-op ').val(iseiQ.utility.htmlDecode(result.BP_PLC_PRIVATE_NOTE));
            divSupplier.find('.btnSelectSupplierPIC').removeAttr("disabled");
        }
    }

    // Clac Total Cont Amount OP
    function CalcTotalAmountOP() {
        var totalPaymentAmount = 0;
        $('.unit-price-op').each(function () {
            totalPaymentAmount = totalPaymentAmount + parseInt($(this).val().replace(/,/g, ''));
        });
        $('.txtAmountTotal').text(iseiQ.utility.convertIntToMoney(totalPaymentAmount));
    }

    function CalcProfit() {
        var totalPaymentAmount = $('.txtAmountTotal').text().replace(/,/g, '');
        var totalCoaiAmount = 0;
        $('.amount-coai').each(function () {
            totalCoaiAmount = totalCoaiAmount + parseInt($(this).val().replace(/,/g, ''));
        });
        var totalProfit = totalCoaiAmount - totalPaymentAmount;
        $('.txtProfitAmount').text(iseiQ.utility.convertIntToMoney(totalProfit));
    }

    ///////////////////////////////////////////////////////////////////
    // private functions
    // Action reset array OAI Delegation
    function resetArrCoai() {
        $CoaiArr = $('div.div-coai tbody tr');

        for (var i = 0; i < $CoaiArr.length; i++) {
            var targetCoai = $CoaiArr[i];
            var prefix = 'CONTRACT_OAI[' + i + '].';
            $(targetCoai).find('.cont-seq-no-coai').attr('name', prefix + 'CONT_SEQ_NO');
            $(targetCoai).find('.cont-apt-seq-no-coai').attr('name', prefix + 'CONT_APT_SEQ_NO');
            $(targetCoai).find('.dsp-order-coai').attr('name', prefix + 'DSP_ORDER');
            $(targetCoai).find('.product-seq-no-coai').attr('name', prefix + 'PRODUCT_SEQ_NO');

            $(targetCoai).find('.item-seq-no-coai').attr('name', prefix + 'ITEM_SEQ_NO');
            $(targetCoai).find('.nomen-coai').attr('name', prefix + 'NOMEN');
            $(targetCoai).find('.unit-price-coai').attr('name', prefix + 'UNIT_PRICE');
            $(targetCoai).find('.qty-coai').attr('name', prefix + 'QTY');
            $(targetCoai).find('.unit-type-coai').attr('name', prefix + 'UNIT_TYPE');
            $(targetCoai).find('.amount-coai').attr('name', prefix + 'AMOUNT');
            $(targetCoai).find('.taxable-flg-coai').attr('name', prefix + 'TAXABLE_FLG');
            $(targetCoai).find('.del-flg').attr('name', prefix + 'DEL_FLG');
        }
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

            $tbCenterTargetRow.find('.deposit-plan-date-hdd').attr('name', prefix + 'DEPOSIT_PLAN_DATE');
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

    function resetArrCopd(control) {
        var $parentElement = $(control).parents('.div-suppliers').find('.special-payment-content');
        var $tbLeftRows = $parentElement.find('.col-left tbody tr');

        var name = $(control).parents('.div-suppliers').find('.op-bp-dis-name').attr('name');
        var subName = name.substr(0, $(control).parents('.div-suppliers').find('.op-bp-dis-name').attr('name').indexOf('.'));

        for (var i = 0; i < $tbLeftRows.length; i++) {
            var prefix = subName + '.CONTRACT_OPD[' + i + '].';
            var $tbLeftTargetRow = $($parentElement.find('.col-left tbody tr')[i]);
            var $tbCenterTargetRow = $($parentElement.find('.col-center tbody tr')[i]);
            var $tbRightTargetRow = $($parentElement.find('.col-right tbody tr')[i]);

            $tbLeftTargetRow.find('.cont-seq-no').attr('name', prefix + 'CONT_SEQ_NO');
            $tbLeftTargetRow.find('.cont-plc-seq-no').attr('name', prefix + 'CONT_PLC_SEQ_NO');
            $tbLeftTargetRow.find('.dsp-order').attr('name', prefix + 'DSP_ORDER');
            $tbLeftTargetRow.find('.target-ym').attr('name', prefix + 'TARGET_YM');

            $tbCenterTargetRow.find('.payment-plan-date-hdd').attr('name', prefix + 'PAYMENT_PLAN_DATE');
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

    function ResetSuppliersArr() {
        var SuppliersArr = $('div.div-suppliers');
        for (var i = 0; i < SuppliersArr.length; i++) {
            var TargetSupplier = SuppliersArr[i];
            var prefix = 'CONTRACT_SUPPLIERS[' + i + '].CONTRACT_OP.';

            $(TargetSupplier).find('.op-cont-seq-no').attr('name', prefix + 'CONT_SEQ_NO');
            $(TargetSupplier).find('.op-cont-plc-seq-no').attr('name', prefix + 'CONT_PLC_SEQ_NO');
            $(TargetSupplier).find('.op-bp-dis-name').attr('name', prefix + 'BP_DSP_NAME');
            $(TargetSupplier).find('.op-bp-seq-no').attr('name', prefix + 'BP_SEQ_NO');
            $(TargetSupplier).find('.op-bp-staff-name').attr('name', prefix + 'BP_STAFF_NAME');
            $(TargetSupplier).find('.op-bp-staff-seq-no').attr('name', prefix + 'BP_STAFF_SEQ_NO');
            $(TargetSupplier).find('.ddl-group-id').attr('name', prefix + 'GROUP_ID');
            $(TargetSupplier).find('.ddl-staff-id').attr('name', prefix + 'STAFF_ID');
            $(TargetSupplier).find('.op-payment-method-type').attr('name', prefix + 'PAYMENT_METHOD_TYPE');
            $(TargetSupplier).find('.calc-unit').attr('name', prefix + 'OUTER_TIME_CALC_UNIT');
            $(TargetSupplier).find('.base-time-lower').attr('name', prefix + 'BASE_TIME_LOWER');
            $(TargetSupplier).find('.base-time-upper').attr('name', prefix + 'BASE_TIME_UPPER');
            $(TargetSupplier).find('.exceed-unit-price').attr('name', prefix + 'EXCEED_UNIT_PRICE');
            $(TargetSupplier).find('.deduction-unit-price').attr('name', prefix + 'DEDUCTION_UNIT_PRICE');
            $(TargetSupplier).find('.unit-time-op').attr('name', prefix + 'UNIT_TIME');
            $(TargetSupplier).find('.unit-price-op').attr('name', prefix + 'UNIT_PRICE');
            $(TargetSupplier).find('.in-tax-flg-cb').attr('name', prefix + 'IN_TAX_FLG_RB');
            $(TargetSupplier).find('.tax-rate-op').attr('name', prefix + 'TAX_RATE');
            $(TargetSupplier).find('.qty-op').attr('name', prefix + 'QTY');
            $(TargetSupplier).find('.cont-amount-op').attr('name', prefix + 'CONT_AMOUNT');
            $(TargetSupplier).find('.op-closing-day').attr('name', prefix + 'CLOSING_DAY');
            $(TargetSupplier).find('.op-payment-site-type').attr('name', prefix + 'PAYMENT_SITE_TYPE');
            $(TargetSupplier).find('.op-payment-day').attr('name', prefix + 'PAYMENT_DAY');
            $(TargetSupplier).find('.plc-note-op').attr('name', prefix + 'PLACEMENT_NOTE');
            $(TargetSupplier).find('.prv-note-op').attr('name', prefix + 'PRIVATE_NOTE');
        }
    }

    function ResetInitValueOp(TargetSupplier)
    {
        var name = $(TargetSupplier).find('input.op-bp-dis-name').attr('name');
        var prefix = name.substr(0, name.lastIndexOf('.') + 1);

        $(TargetSupplier).find('.op-cont-seq-no').val(0);
        $(TargetSupplier).find('.op-cont-plc-seq-no').val(0);
        $(TargetSupplier).find('.op-bp-dis-name').val('');
        $(TargetSupplier).find('.op-bp-seq-no').val(0);
        $(TargetSupplier).find('.op-bp-staff-name').val('');
        $(TargetSupplier).find('.op-bp-staff-seq-no').val(0);
        $(TargetSupplier).find('.ddl-group-id').val(0).change();
        $(TargetSupplier).find('.op-payment-method-type').val(1);
        $(TargetSupplier).find('.calc-unit').val('');
        $(TargetSupplier).find('.base-time-lower').val(0);
        $(TargetSupplier).find('.base-time-upper').val(0);
        $(TargetSupplier).find('.exceed-unit-price').val(0);
        $(TargetSupplier).find('.deduction-unit-price').val(0);
        $(TargetSupplier).find('.unit-time-op').val(30);
        $(TargetSupplier).find('.unit-price-op').val(0);
        $(TargetSupplier).find('.tax-rate-op').val($('#defaultTaxRate').val());
        $(TargetSupplier).find('.qty-op').val(1);
        $(TargetSupplier).find('.cont-amount-op').val(0);
        $('#CONTRACT_OA_CLOSING_DAY').trigger('change');
        $(TargetSupplier).find('.op-payment-site-type').val('');
        $(TargetSupplier).find('.op-payment-day').val('');
        $(TargetSupplier).find('.plc-note-op').val('');
        $(TargetSupplier).find('.prv-note-op').val('');
    }

    function ResetInitValueCoad($parentElement) {
        var $tbLeftLastRow = $parentElement.find('.col-left tr:last');
        var $tbCenterLastRow = $parentElement.find('.col-center tr:last');
        var $tbRightLastRow = $parentElement.find('.col-right tr:last');

        $tbLeftLastRow.find('.cont-seq-no').val(0);
        $tbLeftLastRow.find('.cont-apt-seq-no').val(0);
        $tbLeftLastRow.find('.dsp-order').val(0);
        $tbLeftLastRow.find('.target-ym').val('');

        $tbCenterLastRow.find('.deposit-plan-date-hdd').val('');
        $tbCenterLastRow.find('.deposit-plan-date-lb').text('');
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
        $tbLeftLastRow.find('.cont-apt-seq-no').val(0);
        $tbLeftLastRow.find('.dsp-order').val(0);
        $tbLeftLastRow.find('.target-ym').val('');

        $tbCenterLastRow.find('.payment-plan-date-hdd').val('');
        $tbCenterLastRow.find('.payment-plan-date-lb').text('');
        $tbCenterLastRow.find('.unit-price-copd').val(0);
        $tbCenterLastRow.find('.qty-copd').val(1);
        $tbCenterLastRow.find('.amount-copd').val(0);
        $tbCenterLastRow.find('.adjustment-condition-copd').val('');
        $tbCenterLastRow.find('.calc-unit-copd').val($parentElement.parents('.div-suppliers').find('.calc-unit').val());
        $tbCenterLastRow.find('.base-time-lower-copd').val(0);
        $tbCenterLastRow.find('.base-time-upper-copd').val(0);
        $tbCenterLastRow.find('.exceed-unit-price-copd').val($parentElement.parents('.div-suppliers').find('.exceed-unit-price').val());
        $tbCenterLastRow.find('.deduction-unit-price-copd').val($parentElement.parents('.div-suppliers').find('.deduction-unit-price').val());
        $tbCenterLastRow.find('.unit-time-copd').val($parentElement.parents('.div-suppliers').find('.unit-time-op').val());

        $parentElement.find('.col-right tr:last .del-flg').val(0);
    }

    function changeCoadAdjType(parent, time_index) {
        if (time_index == TimeIndex.CONTRACT_PERIOD_START) {
            $(parent).find('.base-time-lower').css('opacity', '1');
            $(parent).find('.base-time-upper').css('opacity', '1');
            $(parent).find('.exceed-unit-price').css('opacity', '1');
            $(parent).find('.deduction-unit-price').css('opacity', '1');
            $(parent).find('.calc-unit').css('opacity', '1');
            $(parent).find('.lbUnit').css('opacity', '1');
            $(parent).find('.btnCalc_OAD').css('opacity', '1');
            $(parent).find('.unit-time').css('opacity', '1');
            $(parent).find('.lbUnitTime').css('opacity', '1');
            $(parent).find('.unit_time_type').text('円/月');

            $(parent).find('.base-time-lower').prop('disabled', false);
            $(parent).find('.base-time-upper').prop('disabled', false);
            $(parent).find('.exceed-unit-price').prop('disabled', false);
            $(parent).find('.deduction-unit-price').prop('disabled', false);
            $(parent).find('.calc-unit').prop('disabled', false);
            $(parent).find('.btnCalc_OAD').prop('disabled', false);
            $(parent).find('.btn-calculate').prop('disabled', false);
            $(parent).find('.unit-time').prop('disabled', false);
            $(parent).find('.lbUnit').prop('disabled', false);
        }
        else {
            $(parent).find('.base-time-lower').css('opacity', '0');
            $(parent).find('.base-time-upper').css('opacity', '0');
            $(parent).find('.exceed-unit-price').css('opacity', '0');
            $(parent).find('.deduction-unit-price').css('opacity', '0');
            $(parent).find('.calc-unit').css('opacity', '0');
            $(parent).find('.lbUnit').css('opacity', '0');
            $(parent).find('.btnCalc_OAD').css('opacity', '0');
            $(parent).find('.unit-time').css('opacity', '0');
            $(parent).find('.lbUnitTime').css('opacity', '0');
            $(parent).find('.unit_time_type').text('円/月');

            $(parent).find('.base-time-lower').prop('disabled', true);
            $(parent).find('.base-time-upper').prop('disabled', true);
            $(parent).find('.exceed-unit-price').prop('disabled', true);
            $(parent).find('.deduction-unit-price').prop('disabled', true);
            $(parent).find('.calc-unit').prop('disabled', true);
            $(parent).find('.btnCalc_OAD').prop('disabled', true);
            $(parent).find('.btn-calculate').prop('disabled', true);
            $(parent).find('.unit-time').prop('disabled', true);
            $(parent).find('.lbUnit').prop('disabled', true);
            if (time_index == TimeIndex.DELIVERY_DATE) {
                $(parent).find('.unit-time').css('opacity', '1');
                $(parent).find('.unit-time').prop('disabled', false);
                $(parent).find('.lbUnitTime').css('opacity', '1');
                $(parent).find('.unit_time_type').text('円/h');
            }
        }
    }

    function changeCopdAdjType(parent, time_index) {
        if (time_index == TimeIndex.CONTRACT_PERIOD_START) {
            $(parent).find('.base-time-lower-copd').css('opacity', '1');
            $(parent).find('.base-time-upper-copd').css('opacity', '1');
            $(parent).find('.exceed-unit-price-copd').css('opacity', '1');
            $(parent).find('.deduction-unit-price-copd').css('opacity', '1');
            $(parent).find('.btnCalc_OPD').css('opacity', '1');
            $(parent).find('.lbUnit').css('opacity', '1');
            $(parent).find('.calc-unit-copd').css('opacity', '1');
            $(parent).find('.unit_time_type').text('円/月');

            $(parent).find('.base-time-lower-copd').prop('disabled', false);
            $(parent).find('.base-time-upper-copd').prop('disabled', false);
            $(parent).find('.exceed-unit-price-copd').prop('disabled', false);
            $(parent).find('.deduction-unit-price-copd').prop('disabled', false);
            $(parent).find('.calc-unit-copd').prop('disabled', false);
            $(parent).find('.btnCalc_OPD').prop('disabled', false);
            $(parent).find('.btn-calculate').prop('disabled', false);
            $(parent).find('.lbUnit').prop('disabled', false);
            $(parent).find('.unit-time-copd, .lbUnitTimeCopd').css('opacity', '1').prop('disabled', false);
        }
        else {
            $(parent).find('.base-time-lower-copd').css('opacity', '0');
            $(parent).find('.base-time-upper-copd').css('opacity', '0');
            $(parent).find('.exceed-unit-price-copd').css('opacity', '0');
            $(parent).find('.deduction-unit-price-copd').css('opacity', '0');
            $(parent).find('.btnCalc_OPD').css('opacity', '0');
            $(parent).find('.lbUnit').css('opacity', '0');
            $(parent).find('.calc-unit-copd').css('opacity', '0');
            $(parent).find('.unit_time_type').text('円/月');

            $(parent).find('.base-time-lower-copd').prop('disabled', true);
            $(parent).find('.base-time-upper-copd').prop('disabled', true);
            $(parent).find('.exceed-unit-price-copd').prop('disabled', true);
            $(parent).find('.deduction-unit-price-copd').prop('disabled', true);
            $(parent).find('.calc-unit-copd').prop('disabled', true);
            $(parent).find('.btnCalc_OPD').prop('disabled', true);
            $(parent).find('.btn-calculate').prop('disabled', true);
            $(parent).find('.lbUnit').prop('disabled', true);

            $(parent).find('.unit-time-copd, .lbUnitTimeCopd').css('opacity', '0').prop('disabled', true);

            if (time_index == TimeIndex.DELIVERY_DATE) {
                $(parent).find('.unit-time-copd, .lbUnitTimeCopd').css('opacity', '1').prop('disabled', false);
                $(parent).find('.unit_time_type').text('円/h');
            }
        }
    }
}());