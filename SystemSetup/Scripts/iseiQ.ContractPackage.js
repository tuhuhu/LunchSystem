﻿/*!
 * File: iseiQ.ContractPackage.js
 * Company: i-Enter Asia
 * Copyright © 2015 i-Enter Asia. All rights reserved.
 * Project: iseiQ
 * Author: TrungNT
 * Created date: 2015/06/23
 */

var ContractPackage = (function () {
    return {
        ValidateData: ValidateData,
        OnChangeFieldDate: OnChangeFieldDate,
        OnChangeStartDate: OnChangeStartDate,
        OnChangeDeliveryDate: OnChangeDeliveryDate,
        OnChangeAcceptanceDate: OnChangeAcceptanceDate,
        AddRevenue: AddRevenue,
        DeleteRevenue: DeleteRevenue,

        AddCoad: AddCoad,
        DeleteCoad: DeleteCoad,
        ResetInitValueCoad: ResetInitValueCoad,
        AddCopd: AddCopd,
        DeleteCopd: DeleteCopd,
        ResetInitValueCopd: ResetInitValueCopd,

        CalcAmountCoai: CalcAmountCoai,
        OnChangeAmountCoai: OnChangeAmountCoai,

        AddSupplier: AddSupplier,
        DeleteSupplier: DeleteSupplier,
        SelectCustomer: SelectCustomer,
        SelectCustomerPIC: SelectCustomerPIC,
        SelectSupplier: SelectSupplier,
        SelectSupplierPIC: SelectSupplierPIC,
        ApplyCustomer: ApplyCustomer,
        ApplyPartnerStaff: ApplyPartnerStaff,
        CalcTotalAmountOP: CalcTotalAmountOP,
        OnChangePaymentAmount: OnChangePaymentAmount
        
    };


    // Validate data
    function ValidateData() {
        var invalidMess = [];
        
        // Project Name
        var prjName = $('#CONTRACT_PRJ_NAME').val();
        if (prjName === null || prjName.length === 0) // required Project name
            invalidMess.push(Constant.PROJECT_NAME + Constant.ERR_REQUIRED);
        else if (prjName.length > Constant.MAX_PRJ_NAME) // valid string length
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.PROJECT_NAME).replace('{1}', Constant.MAX_PRJ_NAME));

        // Project Name
        var prjNameDesc = $('#CONTRACT_PRJ_DESC_NAME').val();
        if (prjNameDesc === null || prjNameDesc.length === 0) // required Project name
            invalidMess.push(Constant.PROJECT_DETAIL_NAME + Constant.ERR_REQUIRED);
        else if (prjNameDesc.length > Constant.MAX_PRJ_NAME) // valid string length
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.PROJECT_DETAIL_NAME).replace('{1}', Constant.MAX_PRJ_NAME));

        var conTaxRate = $('#CONTRACT_TAX_RATE').val();
        if (conTaxRate.length === 0) {
            invalidMess.push(Constant.TAX_RATE + Constant.ERR_REQUIRED);
        }

        // Cont Amount Total
        var contAmountTotal = $('#CONTRACT_CONT_AMOUNT_TOTAL').val().replace(/,/g, '');
        if (contAmountTotal === null || contAmountTotal.length === 0) // required 
        {
            invalidMess.push(Constant.CONT_AMOUNT_TOTAL + Constant.ERR_REQUIRED);
        }
        else if ((parseInt(contAmountTotal) < Constant.MIN_AMOUNT) || (parseInt(contAmountTotal) > Constant.MAX_AMOUNT))
        {
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.CONT_AMOUNT_TOTAL).replace('{1}', Constant.MAX_AMOUNT_LENGTH));
        }

        // Cont Amount 
        var contAmount = $('#CONTRACT_CONT_TAX').val().replace(/,/g, '');
        if (contAmount === null || contAmount.length === 0) // required 
        {
            invalidMess.push(Constant.TAX + Constant.ERR_REQUIRED);
        }
        else if ((parseInt(contAmount) < Constant.MIN_AMOUNT) || (parseInt(contAmount) > Constant.MAX_AMOUNT))
        {
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.TAX).replace('{1}', Constant.MAX_AMOUNT_LENGTH));
        }
        

        ////////////////////////////////////////////////////////////////////////
        // > coai
        // Product Name
        $('.nomen-coai').each(function () {
            var productName = $(this).val();
            if (productName === null || productName.length === 0) {
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
            if (unitprice === null || unitprice.length === 0) {
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
            if (unitType === null || unitType.length === 0) {
                invalidMess.push(Constant.UNIT_TYPE + Constant.ERR_REQUIRED);
                return;
            }
        });

        // Validate amount
        $('.amount-coai').each(function () {
            var Amount = $(this).val().replace(/,/g, '');
            if (Amount.length === 0) {
                invalidMess.push(Constant.AMOUNT + Constant.ERR_REQUIRED);
                return false;
            }
            else if ((parseInt(Amount) < Constant.MIN_AMOUNT) || (parseInt(Amount) > Constant.MAX_AMOUNT)) {
                invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.AMOUNT).replace('{1}', Constant.MAX_AMOUNT_LENGTH));
            }
        });


        /////////////////////////////////////////////////////////////////////////
        // > coa proc
        var $coaProcess = $('div.div-coa-proc');
        var $coaProcessDataRowsLeft = $coaProcess.find('.col-left tbody tr:not(.hide)');
        var countRow = $coaProcessDataRowsLeft.length;
        var $coaProcessDataRowsCenter = $coaProcess.find('.col-center tbody tr:not(.hide)');
        var errDuplicateYmCoaProcess = null;

        var closingDayOA = $('#CONTRACT_OA_CLOSING_DAY').val();
        var startDate = $('#CONTRACT_OA_START_DATE').val().trim();
        var acceptanceDate = $('#CONTRACT_OA_ACCEPTANCE_DATE').val().trim();

        if ($coaProcess.find('tbody tr:not(.hide)').length > 0) {
            var totalSaleAmount = 0;
            $coaProcessDataRowsLeft.each(function (index, element) {
                // Validate target ym
                var $targetYm = $($coaProcessDataRowsLeft[index]).find('input.proceeds-ym');
                if ((countRow > 1 || $('#CONTRACT_CONT_SEQ_NO').val() != 0)
                    && ($targetYm.val() === null || $targetYm.val().length === 0))
                {
                    invalidMess.push(Constant.ERR_REQUIRE_PROCESS_YM);
                }
                else if (countRow == 1 && $targetYm.val() !== null && $targetYm.val().length !== 0)
                {
                    if ($targetYm.val().length > 7)
                    {
                        invalidMess.push(Constant.ERR_FORMAT_YM.replace('{0}', Constant.TARGET_YM));
                    }
                    else
                    {
                        var errFormatYM = iseiQ.utility.validDate($targetYm.val(), Constant.YM_FORMAT, Constant.TARGET_YM);
                        if (errFormatYM != null) {
                            invalidMess.push(errFormatYM);
                        }
                        if (!iseiQ.utility.isValidTargetYM(startDate, acceptanceDate, $targetYm.val(), closingDayOA)) {
                            // Target YM out of range of project duration
                            invalidMess.push(Constant.ERR_PROCESSYM_INVALID);
                        }
                        if (errDuplicateYmCoaProcess == null) {
                            $targetYm.parents('tbody').find('.proceeds-ym').each(function () {
                                if (!$(this).is($targetYm)) {
                                    var currentTargetYM = $(this).val();
                                    if (iseiQ.utility.validDate(currentTargetYM, Constant.YM_FORMAT, Constant.TARGET_YM) == null
                                        && currentTargetYM == $targetYm.val()) {
                                        errDuplicateYmCoaProcess = Constant.ERR_TARGETYM_DUPLICATE;
                                        invalidMess.push(errDuplicateYmCoaProcess);
                                    }
                                }
                            });
                        }
                    }
                }

                // Validate Amount
                var Amount = $($coaProcessDataRowsCenter[index]).find('input.amount').val().replace(/,/g, '');
                if (Amount === null || Amount.length === 0) {
                    invalidMess.push(Constant.AMOUNT + Constant.ERR_REQUIRED);
                }
                else if ((parseInt(Amount) < Constant.MIN_AMOUNT) || (parseInt(Amount) > Constant.MAX_AMOUNT))
                {
                    invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.AMOUNT).replace('{1}', Constant.MAX_AMOUNT_LENGTH));
                }
                else
                {
                    totalSaleAmount += iseiQ.utility.convertMoneyToInt(Amount, false);
                }

                // Validate receivingInspectionDate
                var receivingInspectionDate = $($coaProcessDataRowsCenter[index]).find('input.receiving-inspection-date').val();
                if ((countRow > 1 || $('#CONTRACT_CONT_SEQ_NO').val() != 0)
                    && (receivingInspectionDate === null || receivingInspectionDate.length === 0))
                {
                    invalidMess.push(Constant.RECEIVING_INSPECTION_DATE + Constant.ERR_REQUIRED);
                }
                else if (countRow == 1 && receivingInspectionDate !== null && receivingInspectionDate.length === 0)
                {
                    if (receivingInspectionDate.length > 10) // valid string length
                    { 
                        //existInvalidDate = true;
                        invalidMess.push(Constant.ERR_FORMAT_DATE.replace('{0}', Constant.RECEIVING_INSPECTION_DATE));
                    }
                    else
                    {
                        // validate start date
                        var errInspectionDate = iseiQ.utility.validDate(receivingInspectionDate, Constant.DATE_FORMAT, Constant.RECEIVING_INSPECTION_DATE);
                        if (errInspectionDate != null) {
                            invalidMess.push(errInspectionDate);
                        }
                    }
                }                           
            });

            //Check if total sale amount is equal to contract amount
            if ((countRow > 1 || (countRow == 1 && $($coaProcessDataRowsLeft[0]).find('.proceeds-ym').val().length != 0))                
                && iseiQ.utility.convertMoneyToInt($('#CONTRACT_CONT_AMOUNT_TOTAL').val(), false) != totalSaleAmount) {
                invalidMess.push(Constant.ERR_MISMATCH_SALE_AMOUNT_CONTRACT_AMOUNT);
            }            
        }

        //27075
        var totalBillingAmount = 0;
        $('div.split-billing-content').find('.col-center tbody tr:not(.hide)').each(function () {
            totalBillingAmount += iseiQ.utility.convertMoneyToInt($(this).find('.base-billing-amount').val());
        });
        if(iseiQ.utility.convertMoneyToInt($('#CONTRACT_CONT_AMOUNT_TOTAL').val()) != totalBillingAmount)
        {
            invalidMess.push(Constant.ERR_MISMATCH_BILLING_AMOUNT_CONTRACT_AMOUNT);
        }

        ///////////////////////////////////////////
        // >> Sales
        // Customer
        var customerDspName = $('#CONTRACT_OA_BP_DSP_NAME').val().trim();
        if (customerDspName === null || customerDspName.length === 0) // required customer name
            invalidMess.push(Constant.BILLING + Constant.ERR_REQUIRED);

        // Customer PIC
        var customerPicName = $('#CONTRACT_OA_BP_STAFF_NAME').val().trim();
        if (customerPicName === null || customerPicName.length === 0) // required staff name
            invalidMess.push(Constant.CUSTOMER_PIC_NAME + Constant.ERR_REQUIRED);

        // Staff Id
        var staffIdOA = $('#CONTRACT_OA_STAFF_ID').val();
        if (staffIdOA === null || staffIdOA.length === 0) // required customer name
            invalidMess.push(Constant.STAFF_ID + Constant.ERR_REQUIRED);

        // Closing day
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

        // Start date
        // CONTRACT_OA_START_DATE require
        //var startDate = $('#CONTRACT_OA_START_DATE').val().trim();
        var errStartDate = null;
        if (startDate === null || startDate.length === 0) { // required start date
            existInvalidDate = true;
            errStartDate = Constant.START_DATE + Constant.ERR_REQUIRED;
            invalidMess.push(errStartDate);
        }
        else if (startDate.length > 0) {
            errStartDate = iseiQ.utility.validDate(startDate, Constant.DATE_FORMAT, Constant.START_DATE);
            if (errStartDate != null) {
                existInvalidDate = true;
                invalidMess.push(errStartDate);
            }
        }
        else if (errStartDate == null && startDate.length > Constant.MAX_DATE) { // valid string length
            existInvalidDate = true;
            errInvalid = Constant.START_DATE + Constant.ERR_INCORRECT_DATE;
            invalidMess.push(errInvalid);
        }

        // Delivery Date
        // CONTRACT_OA_DELIVERY_DATE require
        var deliveryDate = $('#CONTRACT_OA_DELIVERY_DATE').val().trim();
        var errDeliveryDate = null;
        if (deliveryDate === null || deliveryDate.length === 0) { // required delivery date
            errDeliveryDate = Constant.DELIVERY_DATE + Constant.ERR_REQUIRED;
            invalidMess.push(errDeliveryDate);
        }
        else if (deliveryDate.length > 0) {
            errDeliveryDate = iseiQ.utility.validDate(deliveryDate, Constant.DATE_FORMAT, Constant.DELIVERY_DATE);
            if (errDeliveryDate != null) {
                invalidMess.push(errDeliveryDate);
            }
            else if (!iseiQ.utility.compareDate(startDate, deliveryDate, Constant.DATE_FORMAT)) {
                errDeliveryDate = Constant.ERR_COMPARE_TOW_DATE.replace('{0}', Constant.DELIVERY_DATE).replace('{1}', Constant.WORK_PERIOD_START);
                invalidMess.push(errDeliveryDate);
            }
        }
        else if (errDeliveryDate == null && deliveryDate.length > Constant.MAX_DATE) {
            errInvalid = Constant.DELIVERY_DATE + Constant.ERR_INCORRECT_DATE;
            invalidMess.push(errInvalid);
        }

        // Acceptance Date
        // CONTRACT_OA_ACCEPTANCE_DATE require
        //var acceptanceDate = $('#CONTRACT_OA_ACCEPTANCE_DATE').val().trim();
        var errAcceptanceDate = null;
        if (acceptanceDate === null || acceptanceDate.length === 0) { // required delivery date        
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
                errAcceptanceDate = Constant.ERR_COMPARE_TOW_DATE.replace('{0}', Constant.ACCEPTANCE_DATE).replace('{1}', Constant.DELIVERY_DATE);
                invalidMess.push(errAcceptanceDate);
            }
        }
        else if (errAcceptanceDate == null && acceptanceDate.length > Constant.MAX_DATE) {
            invalidMess.push(Constant.ACCEPTANCE_DATE + Constant.ERR_INCORRECT_DATE);
        }

        // Delivery location
        // CONTRACT_OA_DELIVERY_LOCATION require max length 25
        var DeliveryLocationOA = $('#CONTRACT_OA_DELIVERY_LOCATION').val();
        if (DeliveryLocationOA.length > Constant.MAX_DELIVERY_LOCATION)
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.DELIVERY_LOCATION).replace('{1}', Constant.MAX_DELIVERY_LOCATION));

        // Deliverables
        //CONTRACT_OA_DELIVERABLES require max length 25
        var DeliverablesOA = $('#CONTRACT_OA_DELIVERABLES').val();
        if (DeliverablesOA.length > Constant.MAX_DELIVERABLES)
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.DELIVERABLES).replace('{1}', Constant.MAX_DELIVERABLES));

        ///////////////////////////////////////////////////////////////////////////////
        // >> Validate div-coad
        var $splitBillingContent = $('div.split-billing-content');
        var $splitBillingDataRowsLeft = $splitBillingContent.find('.col-left tbody tr:not(.hide)');
        var $splitBillingDataRowsCenter = $splitBillingContent.find('.col-center tbody tr:not(.hide)');
        var errDuplicateYmBilling = null;
        var countRowBilling = $splitBillingDataRowsLeft.length;
        //var countTargetYm = 0;
        $splitBillingDataRowsLeft.each(function (index, element) {
            var $targetYm = $($splitBillingDataRowsLeft[index]).find('input.billing-ym');
            if ((countRowBilling > 1 || $('#CONTRACT_CONT_SEQ_NO').val() != 0)
                && ($targetYm.val() === null || $targetYm.val().length === 0))
            {
                invalidMess.push(Constant.ERR_REQUIRE_BILLING_YM);
            }
            else if ($targetYm.val() !== null && $targetYm.val().length !== 0)
            {                
                // Validate target ym
                if ($targetYm.val().length > 7) {
                    invalidMess.push(Constant.ERR_FORMAT_YM.replace('{0}', Constant.TARGET_YM));
                } else {
                    var errFormatYM = iseiQ.utility.validDate($targetYm.val(), Constant.YM_FORMAT, Constant.TARGET_YM);
                    if (errFormatYM != null) {
                        invalidMess.push(errFormatYM);
                    }
                    if (!iseiQ.utility.isValidTargetYM(startDate, acceptanceDate, $targetYm.val(), closingDayOA)) {
                        // Target YM out of range of project duration
                        invalidMess.push(Constant.ERR_BILLING_YM_INVALID);
                    }
                    if (errDuplicateYmBilling == null) {
                        $targetYm.parents('tbody').find('tr:not(.hide)').find('.billing-ym').each(function () {
                            if (!$(this).is($targetYm)) {
                                var currentTargetYM = $(this).val();
                                if (iseiQ.utility.validDate(currentTargetYM, Constant.YM_FORMAT, Constant.TARGET_YM) == null
                                    && currentTargetYM == $targetYm.val()) {
                                    errDuplicateYmBilling = Constant.ERR_TARGETYM_DUPLICATE;
                                    invalidMess.push(errDuplicateYmBilling);
                                }
                            }
                        });
                    }
                }
            }

            // Amount
            // .base-billing-amount
            var Amount = $($splitBillingDataRowsCenter[index]).find('.base-billing-amount').val().replace(/,/g, '');
            if (Amount === null || Amount.length === 0)
                invalidMess.push(Constant.AMOUNT + Constant.ERR_REQUIRED);
            else if ((parseInt(Amount) < Constant.MIN_AMOUNT) || (parseInt(Amount) > Constant.MAX_AMOUNT)) {
                invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.AMOUNT).replace('{1}', Constant.MAX_AMOUNT_LENGTH));
            }

            // Tax rate
            // .taxrate
            var TaxRate = $($splitBillingDataRowsCenter[index]).find('.taxrate').val();
            if ((countRowBilling > 1 || $('#CONTRACT_CONT_SEQ_NO').val() != 0)
                && (TaxRate === null || TaxRate.length === 0))
            {
                invalidMess.push(Constant.TAX_RATE + Constant.ERR_REQUIRED);
            }
            else if (countRowBilling == 1 && (TaxRate === null || TaxRate.length === 0))
            {
                $($splitBillingDataRowsCenter[index]).find('.taxrate').val($('#CONTRACT_TAX_RATE').val());
            }
                
            

            // Receiving inspection plan date
            // .receiving-inspection-plan-date
            var ReceivingInspectionPlanDate = $($splitBillingDataRowsCenter[index]).find('.receiving-inspection-plan-date').val().trim();
            var errReceivingInspectionDate = null;
            if ((countRowBilling > 1  || $('#CONTRACT_CONT_SEQ_NO').val() != 0)
                && (ReceivingInspectionPlanDate === null || ReceivingInspectionPlanDate.length === 0)) {
                errReceivingInspectionDate = Constant.RECEIVING_INSPECTION_DATE + Constant.ERR_REQUIRED;
                invalidMess.push(errReceivingInspectionDate);
            }
            else if (ReceivingInspectionPlanDate !== null && ReceivingInspectionPlanDate.length !== 0)
            {            
                var receivingInspectionPlanYM = ReceivingInspectionPlanDate.substr(0, 7);
                var billingYM = $($splitBillingDataRowsLeft[index]).find('.billing-ym').val().trim();
                errReceivingInspectionDate = iseiQ.utility.validDate(ReceivingInspectionPlanDate, Constant.DATE_FORMAT, Constant.RECEIVING_INSPECTION_DATE);

                if (errReceivingInspectionDate != null) {
                    invalidMess.push(errReceivingInspectionDate);
                }
                else if (ReceivingInspectionPlanDate.length > Constant.MAX_DATE) {
                    errInvalid = Constant.RECEIVING_INSPECTION_DATE + Constant.ERR_INCORRECT_DATE;
                    invalidMess.push(errInvalid);
                }
                else if (receivingInspectionPlanYM != billingYM) {
                    invalidMess.push(Constant.ERR_RECEIVING_INSPECTION_PLAN_DATE);
                }
            }

            // Deposit plan date
            // .deposit-plan-date
            var depositPlanDate = $($splitBillingDataRowsCenter[index]).find('.deposit-plan-date').val();
            var errDepositPlanDate = null;
            // Validate depositPlanDate
            var depositPlanDate = $($splitBillingDataRowsCenter[index]).find('input.deposit-plan-date').val();
            if ((countRowBilling > 1  || $('#CONTRACT_CONT_SEQ_NO').val() != 0)
                && (depositPlanDate === null || depositPlanDate.length === 0)) {
                //if (depositPlanDate === null || depositPlanDate.length === 0) {
                invalidMess.push(Constant.DEPOSIT_PLAN_DATE + Constant.ERR_REQUIRED);
            }
            else if (depositPlanDate !== null && depositPlanDate.length !== 0)
            {
                if (depositPlanDate.length > 10) // valid string length
                {
                    invalidMess.push(Constant.ERR_FORMAT_DATE.replace('{0}', Constant.DEPOSIT_PLAN_DATE));
                }
                else {
                    var errDepositPlanDate = iseiQ.utility.validDate(depositPlanDate, Constant.DATE_FORMAT, Constant.DEPOSIT_PLAN_DATE);
                    if (errDepositPlanDate != null) {
                        invalidMess.push(errDepositPlanDate);
                    }
                }
            }                       
        });
        ///////////////////////////////////////////////////////////////////////////
        // >> Supplier validation
        if ($('#COUNT_SUPPLIERS').val() > 0)
        {
            $('div.div-suppliers').each(function () {
                var divParent = $(this);
                // Supplier
                var supplierDspName = divParent.find('.op-bp-dis-name').val().trim();
                if (supplierDspName === null || supplierDspName.length === 0) // required supplier name
                    invalidMess.push(Constant.SUPPLIER_NAME + Constant.ERR_REQUIRED);

                // Supplier PIC
                var supplierPicName = divParent.find('.op-bp-staff-name').val().trim();
                if (supplierPicName === null || supplierPicName.length === 0) // required supplier pic name
                    invalidMess.push(Constant.SUPPLIER_PIC + Constant.ERR_REQUIRED);

                //// Staff Id
                var staffIdOp = divParent.find('.ddl-staff-id').val();
                if (staffIdOp === null || staffIdOp.length === 0) // required staff
                    invalidMess.push(Constant.STAFF_ID_OP + Constant.ERR_REQUIRED);

                // Closing day
                var closingDayOP = divParent.find('.closing-day-op').val();
                if (closingDayOP == 0) {
                    invalidMess.push(Constant.CLOSING_DAY + Constant.ERR_REQUIRED_CHOOSE);
                }

                // Payment site type
                var paymentSiteTypeOP = divParent.find('.payment-site-type-op').val();
                if (paymentSiteTypeOP == 0) {
                    invalidMess.push(Constant.PAYMENT_SITE_TYPE + Constant.ERR_REQUIRED_CHOOSE);
                }

                // Payment day
                var paymentDayOP = divParent.find('.payment-day-op').val();
                if (paymentDayOP == 0) {
                    invalidMess.push(Constant.PAYMENT_DAY + Constant.ERR_REQUIRED_CHOOSE);
                }

                // Closing day cannot bigger than payment day. Refs #28177
                if (PaymentSiteType.ThisMonth === paymentSiteTypeOP && parseInt(closingDayOP) > parseInt(paymentDayOP)) {
                    invalidMess.push(Constant.ERR_CLOSING_DAY_BIGGER_THAN_PAYMENT_DAY);
                }

                // Contract amount op
                var contAmountOP = divParent.find('.cont-amount-op').val().replace(/,/g, '');
                if (contAmountOP.length === 0) {
                    invalidMess.push(Constant.AMOUNT + Constant.ERR_REQUIRED);
                    return false;
                }
                else if ((parseInt(contAmountOP) < Constant.MIN_AMOUNT) || (parseInt(contAmountOP) > Constant.MAX_AMOUNT)) {
                    invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.AMOUNT).replace('{1}', Constant.MAX_AMOUNT_LENGTH));
                }
                

                // Start date
                // .start-date require
                var startDate = divParent.find('.start-date').val();
                var errStartDate = null;
                if (startDate === null || startDate.length === 0) { // required start date
                    errStartDate = Constant.START_DATE + Constant.ERR_REQUIRED;
                    invalidMess.push(errStartDate);
                }
                else if (startDate.length > 0) {
                    errStartDate = iseiQ.utility.validDate(startDate, Constant.DATE_FORMAT, Constant.START_DATE);
                    if (errStartDate != null) {
                        invalidMess.push(errStartDate);
                    }
                }
                else if (errStartDate == null && startDate.length > Constant.MAX_DATE) {
                    errInvalid = Constant.START_DATE + Constant.ERR_INCORRECT_DATE;
                    invalidMess.push(errInvalid);
                }

                // Delivery Date
                // .delivery-date require
                var deliveryDate = divParent.find('.delivery-date').val().trim();
                var errDeliveryDate = null;
                if (deliveryDate === null || deliveryDate.length === 0) { // required delivery date
                    errDeliveryDate = Constant.DELIVERY_DATE + Constant.ERR_REQUIRED;
                    invalidMess.push(errDeliveryDate);
                }
                else if (deliveryDate.length > 0) {
                    errDeliveryDate = iseiQ.utility.validDate(deliveryDate, Constant.DATE_FORMAT, Constant.DELIVERY_DATE);
                    if (errDeliveryDate != null) {
                        invalidMess.push(errDeliveryDate);
                    }
                    else if (!iseiQ.utility.compareDate(startDate, deliveryDate, Constant.DATE_FORMAT)) {
                        errDeliveryDate = Constant.ERR_COMPARE_TOW_DATE.replace('{0}', Constant.DELIVERY_DATE).replace('{1}', Constant.START_DATE);
                        invalidMess.push(errDeliveryDate);
                    }
                }
                else if (errDeliveryDate == null && deliveryDate.length > Constant.MAX_DATE) {
                    errInvalid = Constant.DELIVERY_DATE + Constant.ERR_INCORRECT_DATE;
                    invalidMess.push(errInvalid);
                }

                // Acceptance Date
                // .acceptance-date require
                var acceptanceDate = divParent.find('.acceptance-date').val().trim();
                var errAcceptanceDate = null;
                if (acceptanceDate === null || acceptanceDate.length === 0) { // required delivery date
                    errAcceptanceDate = Constant.ACCEPTANCE_DATE + Constant.ERR_REQUIRED;
                    invalidMess.push(errAcceptanceDate);
                }
                else if (acceptanceDate.length > 0) {
                    errAcceptanceDate = iseiQ.utility.validDate(acceptanceDate, Constant.DATE_FORMAT, Constant.ACCEPTANCE_DATE);
                    if (errAcceptanceDate != null) {
                        invalidMess.push(errAcceptanceDate);
                    }
                    else if (!iseiQ.utility.compareDate(deliveryDate, acceptanceDate, Constant.DATE_FORMAT)) {
                        errAcceptanceDate = Constant.ACCEPTANCE_DATE + Constant.ERR_COMPARE_ACCEPTANCE_DATE;
                        invalidMess.push(errAcceptanceDate);
                    }
                }
                else if (errAcceptanceDate == null && acceptanceDate.length > Constant.MAX_DATE) {
                    invalidMess.push(Constant.ACCEPTANCE_DATE + Constant.ERR_INCORRECT_DATE);
                }

                // Delivery location
                // .delivery-location-op require max length 25
                var DeliveryLocationOp = divParent.find('.delivery-location-op').val().trim();
                if (DeliveryLocationOp.length > Constant.MAX_DELIVERY_LOCATION) // valid string length
                    invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.DELIVERY_LOCATION).replace('{1}', Constant.MAX_DELIVERY_LOCATION));

                // Deliverables
                // .deliverables-op require max length 25
                var DeliverablesOP = divParent.find('.deliverables-op').val().trim();
                if (DeliverablesOP.length > Constant.MAX_DELIVERABLES) // valid string length
                    invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.DELIVERABLES).replace('{1}', Constant.MAX_DELIVERABLES));

                var $splitPaymentContent = divParent.find('div.split-payment-content');
                var $splitPaymentContentDataRowsLeft = $splitPaymentContent.find('.col-left tbody tr:not(.hide)');
                var $splitPaymentContentDataRowsCenter = $splitPaymentContent.find('.col-center tbody tr:not(.hide)');
                var errDuplicateYmPayment = null;
                $splitPaymentContentDataRowsLeft.each(function (index, element) {
                    // Validate target ym
                    var $targetYm = $($splitPaymentContentDataRowsLeft[index]).find('input.payment-ym');
                    if ($targetYm.val() === null || $targetYm.val().length === 0) {
                        invalidMess.push(Constant.ERR_REQUIRE_PAYMENT_YM);
                    } else if ($targetYm.val().length > 7) {
                        invalidMess.push(Constant.ERR_FORMAT_YM.replace('{0}', Constant.TARGET_YM));
                    } else {
                        var errFormatYM = iseiQ.utility.validDate($targetYm.val(), Constant.YM_FORMAT, Constant.TARGET_YM);
                        if (errFormatYM != null) {
                            invalidMess.push(errFormatYM);
                        }
                        if (!iseiQ.utility.isValidTargetYM(startDate, acceptanceDate, $targetYm.val(), closingDayOP)) {
                            // Target YM out of range of project duration
                            invalidMess.push(Constant.ERR_PAYMENT_YM_INVALID);
                        }
                        if (errDuplicateYmPayment == null) {
                            $targetYm.parents('tbody').find('tr:not(.hide)').find('.target-ym').each(function () {
                                if (!$(this).is($targetYm)) {
                                    var currentTargetYM = $(this).val();
                                    if (iseiQ.utility.validDate(currentTargetYM, Constant.YM_FORMAT, Constant.TARGET_YM) == null
                                        && currentTargetYM == $targetYm.val()) {
                                        errDuplicateYmPayment = Constant.ERR_TARGETYM_DUPLICATE;
                                        invalidMess.push(errDuplicateYmPayment);
                                    }
                                }
                            });
                        }
                    }
                    // Amount
                    // .base-payment-amount
                    var Amount = $($splitPaymentContentDataRowsCenter[index]).find('.base-payment-amount').val().replace(/,/g, '');
                    if (Amount === null || Amount.length === 0)
                        invalidMess.push(Constant.AMOUNT + Constant.ERR_REQUIRED);
                    else if ((parseInt(Amount) < Constant.MIN_AMOUNT) || (parseInt(Amount) > Constant.MAX_AMOUNT))
                    {
                        invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.AMOUNT).replace('{1}', Constant.MAX_AMOUNT_LENGTH));
                    }

                    // Tax rate
                    // .taxrate
                    var TaxRate = $($splitPaymentContentDataRowsCenter[index]).find('.taxrate').val();
                    if (TaxRate === null || TaxRate.length === 0)
                        invalidMess.push(Constant.TAX_RATE + Constant.ERR_REQUIRED);

                    // Receiving inspection plan date
                    // .receiving-inspection-plan-date
                    var ReceivingInspectionPlanDate = $($splitPaymentContentDataRowsCenter[index]).find('.receiving-inspection-plan-date').val().trim();
                    var errReceivingInspectionDate = null;
                    if (ReceivingInspectionPlanDate === null || ReceivingInspectionPlanDate.length === 0) { // required start date
                        errReceivingInspectionDate = Constant.RECEIVING_INSPECTION_DATE + Constant.ERR_REQUIRED;
                        invalidMess.push(errReceivingInspectionDate);
                    }
                    else if (ReceivingInspectionPlanDate.length > 0) {
                        var receivingInspectionPlanYM = ReceivingInspectionPlanDate.substr(0, 7);
                        var paymentYM = $($splitPaymentContentDataRowsLeft[index]).find('.payment-ym').val().trim();
                        errReceivingInspectionDate = iseiQ.utility.validDate(ReceivingInspectionPlanDate, Constant.DATE_FORMAT, Constant.RECEIVING_INSPECTION_DATE);

                        if (errReceivingInspectionDate != null) {;
                            invalidMess.push(errReceivingInspectionDate);
                        }
                        else if (ReceivingInspectionPlanDate.length > Constant.MAX_DATE) {
                            errInvalid = Constant.RECEIVING_INSPECTION_DATE + Constant.ERR_INCORRECT_DATE;
                            invalidMess.push(errInvalid);
                        }
                        else if (receivingInspectionPlanYM != paymentYM) {
                            invalidMess.push(Constant.ERR_RECEIVING_INSPECTION_PLAN_DATE);
                        }
                    }

                    // Payment plan date
                    // .payment-plan-date
                    var paymentPlanDate = $($splitPaymentContentDataRowsCenter[index]).find('.payment-plan-date').val().trim();
                    var errPaymentPlanDate = null;
                    if (paymentPlanDate === null || paymentPlanDate.length === 0) { // required
                        errPaymentPlanDate = Constant.PAYMENT_PLAN_DATE + Constant.ERR_REQUIRED;
                        invalidMess.push(errPaymentPlanDate);
                    }
                    else if (paymentPlanDate.length > 0) {
                        errPaymentPlanDate = iseiQ.utility.validDate(paymentPlanDate, Constant.DATE_FORMAT, Constant.PAYMENT_PLAN_DATE);
                        if (errPaymentPlanDate != null) {
                            invalidMess.push(errPaymentPlanDate);
                        }
                    }
                    else if (errPaymentPlanDate == null && paymentPlanDate.length > Constant.MAX_DATE) {
                        errInvalid = Constant.PAYMENT_PLAN_DATE + Constant.ERR_INCORRECT_DATE;
                        invalidMess.push(errInvalid);
                    }
                });
            });
        }       
        
        ////////////////////////////////////////////////////////////////////////
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

    // OnChange .receiving-inspection-date, .receiving-inspection-plan-date, .deposit-plan-date
    function OnChangeFieldDate(control)
    {
        var controlName = '';
        if ($(control).hasClass('receiving-inspection-date'))
        {
            controlName = Constant.RECEIVING_INSPECTION_DATE;
        }
        else if ($(control).hasClass('receiving-inspection-plan-date'))
        {
            controlName = Constant.RECEIVING_INSPECTION_DATE;
        }
        else if ($(control).hasClass('deposit-plan-date'))
        {
            controlName = Constant.DEPOSIT_PLAN_DATE;
        }
        else if ($(control).hasClass('payment-plan-date')) {
            controlName = Constant.PAYMENT_PLAN_DATE;
        }

        var DateValue = $(control).val();
        var errInvalid;

        errInvalid = iseiQ.utility.validDate(DateValue, Constant.DATE_FORMAT, controlName);
        if (errInvalid != null) {
            iseiQ.utility.showMessageModal(errInvalid, true);
            return;
        }

        if (DateValue.length > Constant.MAX_DATE) { // valid string length
            errInvalid = controlName + Constant.ERR_INCORRECT_DATE;
            iseiQ.utility.showMessageModal(errInvalid, true, 2);
            return;
        }
    }

    // OnChange .start-date
    function OnChangeStartDate(control)
    {
        var startDate = $(control).val();
        var errInvalid;
        if (startDate.length === 0) {
            return;
        }

        errInvalid = iseiQ.utility.validDate(startDate, Constant.DATE_FORMAT, Constant.START_DATE);
        if (errInvalid != null) {
            iseiQ.utility.showMessageModal(errInvalid, true);
            return;
        }

        if (startDate.length > Constant.MAX_DATE) { // valid string length
            errInvalid = Constant.START_DATE + Constant.ERR_INCORRECT_DATE;
            iseiQ.utility.showMessageModal(errInvalid, true);
            return;
        }
    }

    // OnChange .delivery-date
    function OnChangeDeliveryDate(control)
    {
        var deliveryDate = $(control).val().trim();
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
        var startDate = $(control).parents('div.highlight-zone').find('.start-date').val().trim();
        if (startDate.length > 0 && !iseiQ.utility.compareDate(startDate, deliveryDate, Constant.DATE_FORMAT)) {
            iseiQ.utility.showMessageModal(Constant.DELIVERY_DATE + Constant.ERR_COMPARE_DELIVERY_DATE, true);
            return;
        }
    }

    // OnChange .acceptance-date
    function OnChangeAcceptanceDate(control)
    {
        var acceptanceDate = $(control).val();
        var errInvalid;
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

        //var deliveryDate = $('#DELIVERY_DATE').val();
        var deliveryDate = $(control).parents('div.highlight-zone').find('.delivery-date').val().trim();
        var errInvalidDeliveryDate = iseiQ.utility.validDate(deliveryDate, Constant.DATE_FORMAT, Constant.DELIVERY_DATE);

        if (errInvalidDeliveryDate != null) {
            iseiQ.utility.showMessageModal(errInvalidDeliveryDate, true);
            return;
        }
        var startDate = $(control).parents('div.highlight-zone').find('.start-date').val().trim();
        if (startDate.length > 0 && !iseiQ.utility.compareDate(startDate, acceptanceDate, Constant.DATE_FORMAT)) {
            iseiQ.utility.showMessageModal(Constant.ACCEPTANCE_DATE + Constant.ERR_COMPARE_DATE, true);
            return;
        }
        if (deliveryDate.length > 0 && !iseiQ.utility.compareDate(deliveryDate, acceptanceDate, Constant.DATE_FORMAT)) {
            iseiQ.utility.showMessageModal(Constant.ERR_COMPARE_TOW_DATE.replace('{0}', Constant.ACCEPTANCE_DATE).replace('{1}', Constant.DELIVERY_DATE), true);
            return;
        }
    }    

    ///////////////////////////////////////////////////////////////////////
    // Coa process >>
    // OnClick .btnAddRevenue
    function AddRevenue()
    {
        var $parentElement = $('.div-coa-proc');
        var $tbLeftLastRow = $parentElement.find('.col-left tbody tr:last');
        var $tbCenterLastRow = $parentElement.find('.col-center tbody tr:last');
        var $tbRightLastRow = $parentElement.find('.col-right tbody tr:last');
        $tbLeftLastRow.after($tbLeftLastRow.prop('outerHTML'));
        $tbCenterLastRow.after($tbCenterLastRow.prop('outerHTML'));
        $tbRightLastRow.after($tbRightLastRow.prop('outerHTML'));
        $parentElement.find('.col-left tbody tr:last').removeClass('hide');
        $parentElement.find('.col-center tbody tr:last').removeClass('hide');
        $parentElement.find('.col-right tbody tr:last').removeClass('hide');
        $parentElement.find('.col-right tbody tr .btnDeleteRevenue').show();
        ResetInitValueRevenue($parentElement);
        resetArrRevenue();
        iseiQ.utility.InitDatepickerMonths();
        iseiQ.utility.InitDatepickerDays();
    }

    // OnClick .btnDeleteRevenue
    function DeleteRevenue(control)
    {
        var $parentElement = $('.div-coa-proc');
        var $tbRightTargetRow = $(control).parents('tr');
        var index = $tbRightTargetRow.index();
        var $tbLeftTargetRow = $($parentElement.find('.col-left tbody tr')[index]);
        var $tbCenterTargetRow = $($parentElement.find('.col-center tbody tr')[index]);
        $tbLeftTargetRow.remove();
        $tbCenterTargetRow.remove();
        $tbRightTargetRow.remove();
        if ($parentElement.find('.proceeds-ym').length == 1) {
            $parentElement.find('.col-right tbody tr:first .btnDeleteRevenue').hide();
        }
        resetArrRevenue();
    }

    function resetArrRevenue()
    {
        var $parentElement = $('.div-coa-proc');
        var $tbLeftRows = $parentElement.find('.col-left tbody tr');
        for (var i = 0; i < $tbLeftRows.length; i++) {
            var prefix = 'CONTRACT_OAPROC[' + i + '].';
            var $tbLeftTargetRow = $($parentElement.find('.col-left tbody tr')[i]);
            var $tbCenterTargetRow = $($parentElement.find('.col-center tbody tr')[i]);
            var $tbRightTargetRow = $($parentElement.find('.col-right tbody tr')[i]);
            $tbLeftTargetRow.find('.cont-seq-no').attr('name', prefix + 'CONT_SEQ_NO');
            $tbLeftTargetRow.find('.cont-apt-seq-no').attr('name', prefix + 'CONT_APT_SEQ_NO');
            $tbLeftTargetRow.find('.proceeds-ym').attr('name', prefix + 'PROCEEDS_YM');
            $tbCenterTargetRow.find('.amount').attr('name', prefix + 'AMOUNT');
            $tbCenterTargetRow.find('.receiving-inspection-date').attr('name', prefix + 'RECEIVING_INSPECTION_DATE');
            $tbRightTargetRow.find('.del-flg').attr('name', prefix + 'DEL_FLG');
        }
    }

    function ResetInitValueRevenue($parentElement)
    {
        var $tbLeftLastRow = $parentElement.find('.col-left tr:last');
        var $tbCenterLastRow = $parentElement.find('.col-center tr:last');
        var $tbRightLastRow = $parentElement.find('.col-right tr:last');
        $tbLeftLastRow.find('.cont-seq-no').val(0);
        $tbLeftLastRow.find('.cont-apt-seq-no').val(0);
        $tbLeftLastRow.find('.target-ym').val('');
        $tbCenterLastRow.find('.amount').val(0);
        $tbCenterLastRow.find('.receiving-inspection-date').val('');
        $tbRightLastRow.find('.del-flg').val(0);
    }

    ///////////////////////////////////////////////////////////////////////
    // COAI >>
    // OnChange .unit-price-coai , qty-coai
    function CalcAmountCoai(control) {
        var $parentElement = $(control).parents('tr');
        var price = $parentElement.find('.unit-price-coai').val();
        var quantity = $parentElement.find('.qty-coai').val();
        //if (price.length > 0 && quantity.length > 0) {
        var unitprice = iseiQ.utility.convertMoneyToInt(price,true);
        var amount = iseiQ.utility.roundByType($('#RoundingType').val(), unitprice * quantity);
        $parentElement.find('.amount-coai').val(iseiQ.utility.convertIntToMoney(amount));
        //}
        OnChangeAmountCoai();
    }

    // OnChange .amount-coai
    function OnChangeAmountCoai() {
        var total = 0, totalAmountExcludedTax = 0, totalTax = 0;
        $('.amount-coai').each(function () {
            if ($(this).parents('tr').css('display') !== 'none') {
                total += parseInt($(this).val().replace(/,/g, ''));
                if ($(this).parents('tr').find('select.taxable-flg-coai').val() === '1') {
                    totalAmountExcludedTax += parseInt($(this).val().replace(/,/g, ''));
                }
            }
            $('#CONTRACT_CONT_TAXABLE_AMOUNT_TEMP').val(totalAmountExcludedTax);
            var taxRate = $('#CONTRACT_TAX_RATE').val();
            if (taxRate.length !== 0 & taxRate.indexOf('-') === -1) {
                totalTax = iseiQ.utility.roundByType($('#RoundingType').val(), (totalAmountExcludedTax * taxRate / 100));
            }
            else
            {
                totalTax = 0;
            }
            if (isNaN(totalTax)) {
                $('#CONTRACT_CONT_TAX').val(0);
            } else {
                $('#CONTRACT_CONT_TAX').val(iseiQ.utility.convertIntToMoney(totalTax));
            }
        });
        $('#CONTRACT_CONT_AMOUNT_TOTAL').val(iseiQ.utility.convertIntToMoney(total));
        if($('.div-coa-proc').find('.col-left tbody tr:not(.hide)').length == 1)
        {
            $('.div-coa-proc').find('.col-center tbody tr:not(.hide)').find('.amount').val(total);
        }
    }
    
    // OnClick .btnAddCoad
    function AddCoad() {
        var $parentElement = $('.split-billing-content');
        var $tbLeftLastRow = $parentElement.find('.col-left tbody tr:last');
        var $tbCenterLastRow = $parentElement.find('.col-center tbody tr:last');
        var $tbRightLastRow = $parentElement.find('.col-right tbody tr:last');
        $tbLeftLastRow.after($tbLeftLastRow.prop('outerHTML'));
        $tbCenterLastRow.after($tbCenterLastRow.prop('outerHTML'));
        $tbRightLastRow.after($tbRightLastRow.prop('outerHTML'));
        $parentElement.find('.col-left tbody tr:last').removeClass('hide');
        $parentElement.find('.col-center tbody tr:last').removeClass('hide');
        $parentElement.find('.col-right tbody tr:last').removeClass('hide');
        $parentElement.find('.col-right tbody tr .btnDeleteCoad').show();

        // remove disabled, readonly
        $parentElement.find('.col-left tbody tr:last').find('button').removeAttr("disabled");
        $parentElement.find('.col-left tbody tr:last').find('input').removeAttr("readonly");
        $parentElement.find('.col-center tbody tr:last').find('button').removeAttr("disabled");
        $parentElement.find('.col-center tbody tr:last').find('input').removeAttr("readonly");        
        
        
        ResetInitValueCoad($parentElement);
        resetArrCoad();
        iseiQ.utility.InitDatepickerMonths();
        iseiQ.utility.InitDatepickerDays();
        $tbCenterLastRow.find('.deposit-plan-date').parents(".datepicker-days").datepicker('update');
    }

    // OnClick .btnDeleteCoad
    function DeleteCoad(control)
    {
        var $parentElement = $('.split-billing-content');
        var $tbRightTargetRow = $(control).parents('tr');
        var index = $tbRightTargetRow.index();
        var $tbLeftTargetRow = $($parentElement.find('.col-left tbody tr')[index]);
        var $tbCenterTargetRow = $($parentElement.find('.col-center tbody tr')[index]);

        $tbLeftTargetRow.remove();
        $tbCenterTargetRow.remove();
        $tbRightTargetRow.remove();
        if ($parentElement.find('.billing-ym').length == 1)
        {
            $parentElement.find('.col-right tbody tr:first .btnDeleteCoad').hide();
        }        
        resetArrCoad();
    }

    // OnClick .btnAddCopd
    function AddCopd(control)
    {
        var $parentElement = $(control).parents('.div-suppliers').find('.split-payment-content');
        var $tbLeftLastRow = $parentElement.find('.col-left tbody tr:last');
        var $tbCenterLastRow = $parentElement.find('.col-center tbody tr:last');
        var $tbRightLastRow = $parentElement.find('.col-right tbody tr:last');
        $tbLeftLastRow.after($tbLeftLastRow.prop('outerHTML'));
        $tbCenterLastRow.after($tbCenterLastRow.prop('outerHTML'));
        $tbRightLastRow.after($tbRightLastRow.prop('outerHTML'));
        $parentElement.find('.col-left tbody tr:last').removeClass('hide');
        $parentElement.find('.col-center tbody tr:last').removeClass('hide');
        $parentElement.find('.col-right tbody tr:last').removeClass('hide');
        $parentElement.find('.col-right tbody tr .btnDeleteCopd').show();
                
        ResetInitValueCopd($parentElement);
        resetArrCopd(control);
        iseiQ.utility.InitDatepickerMonths();
        iseiQ.utility.InitDatepickerDays();
        $tbCenterLastRow.find('.payment-plan-date').parents(".datepicker-days").datepicker('update');
    }

    // OnClick .btnDeleteCopd
    function DeleteCopd(control)
    {
        var $parentElement = $(control).parents('div.split-payment-content');
        var $tbRightTargetRow = $(control).parents('tr');
        var index = $tbRightTargetRow.index();
        var $tbLeftTargetRow = $($parentElement.find('.col-left tbody tr')[index]);
        var $tbCenterTargetRow = $($parentElement.find('.col-center tbody tr')[index]);
        if ($tbLeftTargetRow.find('.cont-payment-seq-no').val() == 0) {
            $tbLeftTargetRow.remove();
            $tbCenterTargetRow.remove();
            $tbRightTargetRow.remove();
        }
        else {
            $tbLeftTargetRow.addClass('hide');
            $tbCenterTargetRow.addClass('hide');
            $tbRightTargetRow.addClass('hide').find('.del-flg').val(1);
        }
        if ($parentElement.find('.col-left tbody tr:not(.hide)').find('.payment-ym').length == 1) {
            $parentElement.find('.col-right tbody tr:not(.hide):first .btnDeleteCopd').hide();
        }
        resetArrCopd($parentElement);
        OnChangePaymentAmount($parentElement.find('.base-payment-amount'));
    }

    //Add payments
    function AddSupplier()
    {
        if ($('div.div-suppliers:not(.hide)').length > 0) {
            $('div.div-suppliers').last().after($('div.div-suppliers').first().prop('outerHTML'));
            var $parentElement = $('div.div-suppliers').last();
            var firstCopd = $parentElement.find('div.split-payment-content').first().prop('outerHTML');
            $parentElement.find('div.split-payment-content').after(firstCopd);
            $parentElement.find('div.split-payment-content').first().remove();

            var $tbLeftFirstRow = $parentElement.find('.col-left tbody tr:first');
            var $tbCenterFirstRow = $parentElement.find('.col-center tbody tr:first');
            var $tbRightFirstRow = $parentElement.find('.col-right tbody tr:first');

            $parentElement.find('.col-left tbody tr').remove();
            $parentElement.find('.col-center tbody tr').remove();
            $parentElement.find('.col-right tbody tr').remove();

            $parentElement.find('.col-left tbody').html($tbLeftFirstRow.prop('outerHTML'));
            $parentElement.find('.col-center tbody').html($tbCenterFirstRow.prop('outerHTML'));
            $parentElement.find('.col-right tbody').html($tbRightFirstRow.prop('outerHTML'));

            if ($parentElement.find('.btnSelectSupplier').length == 0) {
                $parentElement.find('.op-bp-dis-name').after('<button class="btn btn-default btnSelectSupplier" type="button"><i class="btn-icon btn-search-gray"></i></button>');
            }
            if ($parentElement.find('.btnSelectSupplierPIC').length != 0) {
                $parentElement.find('.btnSelectSupplierPIC').replaceWith('<button class="btn btn-default btnSelectCustomerPIC" type="button" disabled><i class="btn-icon btn-search-gray"></i></button>');
            }

            ResetInitValueOp($parentElement);
            ResetSuppliersArr();
            ResetInitValueCopd($parentElement.find('.split-payment-content'));
            $($parentElement).find('.delivery-location-op').val($('.init-delivery-location').val());
            $($parentElement).find('.deliverables-op').val($('.init-deliverables').val());
            $parentElement.find('.divDeleteSupplier').removeClass('hide');
        }
        else {
            $('div.div-suppliers').removeClass('hide');
            $('div.div-suppliers').find('.divDeleteSupplier').removeClass('hide');
            var $parentElement = $('div.div-suppliers').last();
            $($parentElement).find('.delivery-location-op').val($('.init-delivery-location').val());
            $($parentElement).find('.deliverables-op').val($('.init-deliverables').val());
        }

        $('#COUNT_SUPPLIERS').val(parseInt($('#COUNT_SUPPLIERS').val()) + 1);
        iseiQ.utility.InitDatepickerMonths();
        iseiQ.utility.InitDatepickerDays();
    }

    // btnDeleteSupplier
    function DeleteSupplier(control) {
        if ($('div.div-suppliers').length > 1) {
            $(control).parents('.div-suppliers').remove();
            ResetSuppliersArr();
        }
        else {
            $('div.div-suppliers').addClass('hide');
            ResetInitValueOp($(control).parents('.div-suppliers'));
            ResetSuppliersArr();
            var $splitPayment = $('.div-suppliers').find('.split-payment-content');
            $splitPayment.find('.col-left tbody tr:gt(0)').remove();
            $splitPayment.find('.col-center tbody tr:gt(0)').remove();
            $splitPayment.find('.col-right tbody tr:gt(0)').remove();
            ResetInitValueCopd($splitPayment);
        }
        $('#COUNT_SUPPLIERS').val(parseInt($('#COUNT_SUPPLIERS').val()) - 1);
        CalcTotalAmountOP();
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
        // Set OrderAcceptance FLAG
        $('#isOrderAcceptance').val("2");
        $('#nameOpBpDspName').val($(control).parents('div.div-suppliers').find('input.op-bp-dis-name').attr('name'));
        CallSearchCustomer();
    }

    //btnSelectSupPIC
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
        } else {
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
        $('.cont-amount-op').each(function () {
            totalPaymentAmount = totalPaymentAmount + parseInt($(this).val().replace(/,/g, ''));
        });
        $('.txtAmountTotal').text(iseiQ.utility.convertIntToMoney(totalPaymentAmount));
    }

    function OnChangePaymentAmount(control) {
        var total = 0;
        var parent = $(control).parents('.div-suppliers');
        $(parent).find('.base-payment-amount').each(function () {
            if (!$(this).parents('tr').hasClass('hide')) {
                total += $(this).val().length == 0 ? 0 : parseInt($(this).val().replace(/,/g, ''));
            }
        });
        $(parent).find('.cont-amount-op').val(iseiQ.utility.convertIntToMoney(total));
        CalcTotalAmountOP();
    }

    ///////////////////////////////////////////////
    // >> Private function

    function resetArrCoad()
    {
        var $parentElement = $('.split-billing-content');
        var $tbLeftRows = $parentElement.find('.col-left tbody tr');
        for (var i = 0; i < $tbLeftRows.length; i++) {
            var prefix = 'CONTRACT_BILLING[' + i + '].';
            var $tbLeftTargetRow = $($parentElement.find('.col-left tbody tr')[i]);
            var $tbCenterTargetRow = $($parentElement.find('.col-center tbody tr')[i]);
            var $tbRightTargetRow = $($parentElement.find('.col-right tbody tr')[i]);
            $tbLeftTargetRow.find('.cont-bill-seq-no').attr('name', prefix + 'CONT_BILLING_SEQ_NO');
            $tbLeftTargetRow.find('.billing-ym').attr('name', prefix + 'BILLING_YM');
            $tbCenterTargetRow.find('.base-billing-amount').attr('name', prefix + 'BASE_BILLING_AMOUNT');
            $tbCenterTargetRow.find('.taxrate').attr('name', prefix + 'TAX_RATE');
            $tbCenterTargetRow.find('.receiving-inspection-plan-date').attr('name', prefix + 'RECEIVING_INSPECTION_PLAN_DATE');
            $tbCenterTargetRow.find('.deposit-plan-date').attr('name', prefix + 'DEPOSIT_PLAN_DATE');
            $tbRightTargetRow.find('.del-flg').attr('name', prefix + 'DEL_FLG');
        }
    }

    function resetArrCopd(control)
    {
        var $parentElement = $(control).parents('.div-suppliers').find('.split-payment-content');
        var $tbLeftRows = $parentElement.find('.col-left tbody tr');
        var name = $(control).parents('.div-suppliers').find('.op-bp-dis-name').attr('name');
        var subName = name.substr(0, $(control).parents('.div-suppliers').find('.op-bp-dis-name').attr('name').indexOf('.'));
        for (var i = 0; i < $tbLeftRows.length; i++) {
            var prefix = subName + '.CONTRACT_PAYMENT[' + i + '].';
            var $tbLeftTargetRow = $($parentElement.find('.col-left tbody tr')[i]);
            var $tbCenterTargetRow = $($parentElement.find('.col-center tbody tr')[i]);
            var $tbRightTargetRow = $($parentElement.find('.col-right tbody tr')[i]);
            $tbLeftTargetRow.find('.cont-payment-seq-no').attr('name', prefix + 'CONT_PAYMENT_SEQ_NO');
            $tbLeftTargetRow.find('.payment-ym').attr('name', prefix + 'PAYMENT_YM');
            $tbCenterTargetRow.find('.base-payment-amount').attr('name', prefix + 'BASE_PAYMENT_AMOUNT');
            $tbCenterTargetRow.find('.taxrate').attr('name', prefix + 'TAX_RATE');
            $tbCenterTargetRow.find('.receiving-inspection-plan-date').attr('name', prefix + 'RECEIVING_INSPECTION_PLAN_DATE');
            $tbCenterTargetRow.find('.payment-plan-date').attr('name', prefix + 'PAYMENT_PLAN_DATE');
            $tbRightTargetRow.find('.del-flg').attr('name', prefix + 'DEL_FLG');
        }
    }

    function ResetSuppliersArr()
    {
        var SuppliersArr = $('div.div-suppliers');
        for (var i = 0; i < SuppliersArr.length; i++) {
            var TargetSupplier = SuppliersArr[i];
            var prefix = 'CONTRACT_SUPPLIERS[' + i + '].CONTRACT_OP.';
            //var index = $('div.div-suppliers').length;
            //var prefix = 'CONTRACT_SUPPLIERS[' + (index - 1) + '].CONTRACT_OP.';
            $(TargetSupplier).find('.op-cont-seq-no').attr('name', prefix + 'CONT_SEQ_NO');
            $(TargetSupplier).find('.op-cont-plc-seq-no').attr('name', prefix + 'CONT_PLC_SEQ_NO');
            $(TargetSupplier).find('.op-bp-dis-name').attr('name', prefix + 'BP_DSP_NAME');
            $(TargetSupplier).find('.op-bp-seq-no').attr('name', prefix + 'BP_SEQ_NO');
            $(TargetSupplier).find('.op-bp-staff-name').attr('name', prefix + 'BP_STAFF_NAME');
            $(TargetSupplier).find('.op-bp-staff-seq-no').attr('name', prefix + 'BP_STAFF_SEQ_NO');

            $(TargetSupplier).find('.ddl-group-id').attr('name', prefix + 'GROUP_ID');
            $(TargetSupplier).find('.ddl-staff-id').attr('name', prefix + 'STAFF_ID');

            $(TargetSupplier).find('.payment-site-type-op').attr('name', prefix + 'PAYMENT_SITE_TYPE');
            $(TargetSupplier).find('.closing-day-op').attr('name', prefix + 'CLOSING_DAY');
            $(TargetSupplier).find('.payment-day-op').attr('name', prefix + 'PAYMENT_DAY');

            $(TargetSupplier).find('.start-date').attr('name', prefix + 'START_DATE');
            $(TargetSupplier).find('.delivery-date').attr('name', prefix + 'DELIVERY_DATE');
            $(TargetSupplier).find('.acceptance-date').attr('name', prefix + 'ACCEPTANCE_DATE');

            $(TargetSupplier).find('.delivery-location-op').attr('name', prefix + 'DELIVERY_LOCATION');
            $(TargetSupplier).find('.deliverables-op').attr('name', prefix + 'DELIVERABLES');
            $(TargetSupplier).find('.cont-amount-op').attr('name', prefix + 'CONT_AMOUNT');

            $(TargetSupplier).find('.plc-note-op').attr('name', prefix + 'PLACEMENT_NOTE');
            $(TargetSupplier).find('.prv-note-op').attr('name', prefix + 'PRIVATE_NOTE');
            resetArrCopd($(TargetSupplier).find('.split-payment-content'));
        }
        
    }

    function ResetInitValueOp(TargetSupplier) {
        var name = $(TargetSupplier).find('input.op-bp-dis-name').attr('name');
        var prefix = name.substr(0, name.lastIndexOf('.') + 1);

        $(TargetSupplier).find('.op-cont-seq-no').val(0);
        $(TargetSupplier).find('.op-cont-plc-seq-no').val(0);
        $(TargetSupplier).find('.op-bp-dis-name').val('');
        $(TargetSupplier).find('.op-bp-seq-no').val('0');
        $(TargetSupplier).find('.op-bp-staff-name').val('');
        $(TargetSupplier).find('.op-bp-staff-seq-no').val('0');

        $(TargetSupplier).find('.ddl-group-id').val('').change();

        $(TargetSupplier).find('.payment-site-type-op').val('');
        $(TargetSupplier).find('.closing-day-op').val(31);
        $(TargetSupplier).find('.payment-day-op').val('');

        $(TargetSupplier).find('.start-date').val('');
        $(TargetSupplier).find('.delivery-date').val('');
        $(TargetSupplier).find('.acceptance-date').val('');

        $(TargetSupplier).find('.delivery-location-op').val('');
        $(TargetSupplier).find('.deliverables-op').val('');
        $(TargetSupplier).find('.cont-amount-op').val('0');

        $(TargetSupplier).find('.plc-note-op').val('');
        $(TargetSupplier).find('.prv-note-op').val('');
    }

    function ResetInitValueCoad($parentElement)
    {
        var $tbLeftLastRow = $parentElement.find('.col-left tr:last');
        var $tbCenterLastRow = $parentElement.find('.col-center tr:last');
        var $tbRightLastRow = $parentElement.find('.col-right tr:last');
        $tbLeftLastRow.find('.cont-bill-seq-no').val(0);
        $tbLeftLastRow.find('.billing-ym').val('');
        $tbLeftLastRow.find('.billing-ym').removeAttr('value');
        $tbCenterLastRow.find('.base-billing-amount').val(0);
        $tbCenterLastRow.find('.taxrate').val($('#defaultTaxRate').val());
        $tbCenterLastRow.find('.receiving-inspection-plan-date').val('');
        $tbCenterLastRow.find('.deposit-plan-date').val('');
        $tbCenterLastRow.find('.deposit-plan-date').removeClass("unchangeable");
        
        $tbRightLastRow.find('.del-flg').val(0);
    }

    function ResetInitValueCopd($parentElement)
    {
        var $tbLeftLastRow = $parentElement.find('.col-left tr:last');
        var $tbCenterLastRow = $parentElement.find('.col-center tr:last');
        var $tbRightLastRow = $parentElement.find('.col-right tr:last');
        $tbLeftLastRow.find('.cont-payment-seq-no').val(0);
        $tbLeftLastRow.find('.payment-ym').val('');
        $tbLeftLastRow.find('.payment-ym').removeAttr('value');
        $tbCenterLastRow.find('.base-payment-amount').val(0);
        $tbCenterLastRow.find('.taxrate').val($('#defaultTaxRate').val());
        $tbCenterLastRow.find('.receiving-inspection-plan-date').val('');
        $tbCenterLastRow.find('.payment-plan-date').val('');
        $tbCenterLastRow.find('.payment-plan-date').removeClass("unchangeable");
        
        $tbRightLastRow.find('.del-flg').val(0);
    }
}());