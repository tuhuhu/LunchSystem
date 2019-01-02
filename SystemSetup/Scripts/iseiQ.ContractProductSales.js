/*!
 * File: iseiQ.ContractProductSales.js
 * Company: i-Enter Asia
 * Copyright © 2015 i-Enter Asia. All rights reserved.
 * Project: iseiQ
 * Author: TrungNT
 * Created date: 2015/09/16
 */

$(document).off('div.ContractProductSales div.div-suppliers input.tax-rate');
$(document).on('blur', 'div.ContractProductSales div.div-suppliers input.tax-rate', function () {
    Contract.CalcTotalPaymentAmount();
});

$(document).off('div.ContractProductSales div.div-suppliers div.div-copd div.col-center table.dataTable tbody tr td select.taxable-flg-copd');
$(document).on('change', 'div.ContractProductSales div.div-suppliers div.div-copd div.col-center table.dataTable tbody tr td select.taxable-flg-copd', function () {
    Contract.CalcTotalPaymentAmount();
});

var ContractProductSales = (function () {

    return {
        ValidateData: ValidateData,
        OnChangeAcceptanceDateContract: OnChangeAcceptanceDateContract,

        AddSupplier: AddSupplier,
        DeleteSupplier: DeleteSupplier,
        SelectCustomer: SelectCustomer,
        SelectCustomerPIC: SelectCustomerPIC,
        SelectSupplier: SelectSupplier,
        SelectSupplierPIC: SelectSupplierPIC,
        ApplyCustomer: ApplyCustomer,
        ApplyPartnerStaff: ApplyPartnerStaff,

        AddCoad: AddCoad,
        DeleteCoad: DeleteCoad,
        CalcAmountCoai: CalcAmountCoai,
        OnChangeAmountCoai: OnChangeAmountCoai,
        CalcAmountCopi: CalcAmountCopi,

        AddCopd: AddCopd,
        DeleteCopd: DeleteCopd,
        CalcAmountCopd: CalcAmountCopd,
        CalcTotalPaymentAmount: CalcTotalPaymentAmount
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

        //var deliv = $('#CONTRACT_DELIVERABLES').val();
        //if (deliv.length === 0) // required Deliverables
        //    invalidMess.push(Constant.DELIVERABLES + Constant.ERR_REQUIRED);
        //else if (deliv.length > Constant.MAX_DELIVERABLES) // valid string length
        //    invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.DELIVERABLES).replace('{1}', Constant.MAX_DELIVERABLES));

        //// Delivery location
        //var deliLocation = $('#CONTRACT_DELIVERY_LOCATION').val().trim();
        //if (deliLocation.length === 0) // required Deliverables
        //    invalidMess.push(Constant.DELIVERY_LOCATION + Constant.ERR_REQUIRED);
        //else if (deliLocation.length > Constant.MAX_DELIVERY_LOCATION) // valid string length
        //    invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.DELIVERY_LOCATION).replace('{1}', Constant.MAX_DELIVERY_LOCATION));

        // Initialize date
        var startDate = $('#CONTRACT_PRJ_PERIOD_START').val().trim();
        var endDate = $('#CONTRACT_PRJ_PERIOD_END').val().trim();
        var deliveryDate = $('#CONTRACT_DELIVERY_DATE').val().trim();
        var acceptanceDate = $('#CONTRACT_ACCEPTANCE_DATE').val();
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
            else if (startDate.length > 0 && !iseiQ.utility.compareDate(startDate, deliveryDate, Constant.DATE_FORMAT)) {
                errDeliveryDate = Constant.ERR_COMPARE_TOW_DATE.replace('{0}', Constant.DELIVERY_DATE).replace('{1}', Constant.WORK_PERIOD_START);
                invalidMess.push(errDeliveryDate);
            }
            else if (!iseiQ.utility.compareDate(deliveryDate, endDate, Constant.DATE_FORMAT)) {
                errDeliveryDate = Constant.ERR_COMPARE_OVE_DATE.replace('{0}', Constant.DELIVERY_DATE).replace('{1}', Constant.WORK_PERIOD_END)
                invalidMess.push(errDeliveryDate);
            }
        }
        else if (errDeliveryDate == null && deliveryDate.length > Constant.MAX_DATE) { // valid string length
            errInvalid = Constant.DELIVERY_DATE + Constant.ERR_INCORRECT_DATE;
            invalidMess.push(errInvalid);
        }

        // Validate Acceptance date
        var errAcceptanceDate = null;
        if (acceptanceDate.length === 0) { // required delivery date
            errAcceptanceDate = Constant.ACCEPTANCE_DATE + Constant.ERR_REQUIRED;
            invalidMess.push(errAcceptanceDate);
        }
        else if (acceptanceDate.length > 0) {
            errAcceptanceDate = iseiQ.utility.validDate(acceptanceDate, Constant.DATE_FORMAT, Constant.ACCEPTANCE_DATE);
            if (errAcceptanceDate != null) {
                invalidMess.push(errAcceptanceDate);
            }
            else if (startDate.length > 0 && !iseiQ.utility.compareDate(startDate, acceptanceDate, Constant.DATE_FORMAT)) {
                errAcceptanceDate = Constant.ERR_COMPARE_TOW_DATE.replace('{0}', Constant.ACCEPTANCE_DATE).replace('{1}', Constant.WORK_PERIOD_START);
                invalidMess.push(errAcceptanceDate);
            }
            else if (!iseiQ.utility.compareDate(acceptanceDate, endDate, Constant.DATE_FORMAT)) {
                errAcceptanceDate = Constant.ERR_COMPARE_OVE_DATE.replace('{0}', Constant.ACCEPTANCE_DATE).replace('{1}', Constant.WORK_PERIOD_END);
                invalidMess.push(errAcceptanceDate);
            }
            if (deliveryDate.length > 0 && !iseiQ.utility.compareDate(deliveryDate, acceptanceDate, Constant.DATE_FORMAT)) {
                errAcceptanceDate = Constant.ERR_COMPARE_TOW_DATE.replace('{0}', Constant.ACCEPTANCE_DATE).replace('{1}', Constant.DELIVERY_DATE);
                invalidMess.push(errAcceptanceDate);
            }
        }
        else if (errAcceptanceDate == null && acceptanceDate.length > Constant.MAX_DATE) { // valid string length
            errInvalid = Constant.ACCEPTANCE_DATE + Constant.ERR_INCORRECT_DATE;
            invalidMess.push(errInvalid);
        }

        // compare period
        if (!existInvalidDate &&
            !iseiQ.utility.compareDate(startDate, endDate, Constant.DATE_FORMAT)) {
            existInvalidDate = true;
            invalidMess.push(Constant.ERR_COMPARE_TOW_DATE.replace('{0}', Constant.WORK_PERIOD_END).replace('{1}', Constant.WORK_PERIOD_START));
        }

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

        // Cont Amount Total
        var AmountOA = $('#CONTRACT_OA_CONT_AMOUNT').val().replace(/,/g, '');
        if (AmountOA === null || AmountOA.length === 0) // required 
        {
            invalidMess.push(Constant.CONT_AMOUNT_TOTAL + Constant.ERR_REQUIRED);
        }
        else if ((parseInt(AmountOA) < Constant.MIN_AMOUNT) || (parseInt(AmountOA) > Constant.MAX_AMOUNT)) {
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.CONT_AMOUNT_TOTAL).replace('{1}', Constant.MAX_AMOUNT_LENGTH));
        }

        // Cont Amount 
        var OATax = $('#CONTRACT_OA_TAX').val().replace(/,/g, '');
        if (OATax === null || OATax.length === 0) // required 
        {
            invalidMess.push(Constant.TAX + Constant.ERR_REQUIRED);
        }
        else if ((parseInt(OATax) < Constant.MIN_AMOUNT) || (parseInt(OATax) > Constant.MAX_AMOUNT)) {
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.TAX).replace('{1}', Constant.MAX_AMOUNT_LENGTH));
        }

        // Tax rate
        var TaxRate = $('#CONTRACT_OA_TAX_RATE_PS').val();
        if (TaxRate.length === 0)
            invalidMess.push(Constant.TAXRATE + Constant.ERR_REQUIRED);
        else if (TaxRate.length > Constant.MAX_TAXRATE) // valid string length
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.TAXRATE).replace('{1}', Constant.MAX_TAXRATE));

        // Check coai
        var listCoais = $('div.div-sale-items div.col-center .dataTable tbody tr');
        listCoais.each(function () {
            if (!$(this).hasClass('hide')) {
                // Validate Unit price
                var unitprice = $(this).find('input.unit-price-coai').val().replace(/,/g, '');
                if (unitprice.length === 0) {
                    invalidMess.push(Constant.UNIT_PRICE + Constant.ERR_REQUIRED);
                } else if ((parseInt(unitprice) < Constant.MIN_UNIT_PRICE) || (parseInt(unitprice) > Constant.MAX_UNIT_PRICE)) {
                    invalidMess.push(Constant.UNIT_PRICE + Constant.ERR_RANGE_UNIT_PRICE);
                }

                // Validate quantity
                var qty = $(this).find('input.qty-coai').val();
                if (qty.length === 0)
                    invalidMess.push(Constant.QUANTITY + Constant.ERR_REQUIRED);
                else if (qty.length > Constant.MAX_QUANTITY_LENGTH) // valid string length
                    invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.QUANTITY).replace('{1}', Constant.MAX_QUANTITY_LENGTH));

                // Validate Amount
                var Amount = $(this).find('input.amount-coai').val().replace(/,/g, '');
                if (Amount.length === 0) {
                    invalidMess.push(Constant.AMOUNT + Constant.ERR_REQUIRED);
                }
                else if ((parseInt(Amount) < Constant.MIN_AMOUNT) || (parseInt(Amount) > Constant.MAX_AMOUNT)) {
                    invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.AMOUNT).replace('{1}', Constant.MAX_AMOUNT_LENGTH));
                }
            }
        });

        // Acceptance Note
        var aptNote = $('#CONTRACT_OA_ACCEPTANCE_NOTE').val();
        if (aptNote.length > Constant.MAX_NOTE) // valid string length
            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.NOTE).replace('{1}', Constant.MAX_NOTE));

        // Check supplier
        ///////////////////////////////////////////
        // >> Supplier validation
        if ($('#COUNT_SUPPLIERS').val() > 0) {
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

                // Closing day
                var closingDayOP = divParent.find('.op-closing-day').val();
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

                // Amount
                var AmountOP = divParent.find('.cont-amount-op').val().replace(/,/g, '');
                if (AmountOP.length === 0) {
                    invalidMess.push(Constant.AMOUNT + Constant.ERR_REQUIRED);
                }
                else if ((parseInt(AmountOP) < Constant.MIN_AMOUNT) || (parseInt(AmountOP) > Constant.MAX_AMOUNT))
                {
                    invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.AMOUNT).replace('{1}', Constant.MAX_AMOUNT_LENGTH));
                }

                // Tax rate
                var TaxRateOp = divParent.find('.tax-rate').val();
                if (TaxRateOp.length === 0)
                    invalidMess.push(Constant.TAXRATE + Constant.ERR_REQUIRED);
                else if (TaxRateOp.length > Constant.MAX_TAXRATE) // valid string length
                    invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.TAXRATE).replace('{1}', Constant.MAX_TAXRATE));

                // Cont Amount 
                var TaxOP = divParent.find('.tax').val().replace(/,/g, '');
                if (TaxOP === null || TaxOP.length === 0) // required 
                {
                    invalidMess.push(Constant.TAX + Constant.ERR_REQUIRED);
                }
                else if ((parseInt(TaxOP) < Constant.MIN_AMOUNT) || (parseInt(TaxOP) > Constant.MAX_AMOUNT)) {
                    invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.TAX).replace('{1}', Constant.MAX_AMOUNT_LENGTH));
                }

                // Copd validation
                var listCopds = divParent.find('div.div-copd div.col-center table.dataTable tbody tr');
                listCopds.each(function () {
                    if (!$(this).hasClass('hide')) {
                        // Validate Unit price
                        var unitprice = $(this).find('input.unit-price-copd').val().replace(/,/g, '');
                        if (unitprice.length === 0) {
                            invalidMess.push(Constant.UNIT_PRICE + Constant.ERR_REQUIRED);
                        } else if ((parseInt(unitprice) < Constant.MIN_UNIT_PRICE) || (parseInt(unitprice) > Constant.MAX_UNIT_PRICE)) {
                            invalidMess.push(Constant.UNIT_PRICE + Constant.ERR_RANGE_UNIT_PRICE);
                        }

                        // Validate quantity
                        var qty = $(this).find('input.qty-copd').val();
                        if (qty.length === 0)
                            invalidMess.push(Constant.QUANTITY + Constant.ERR_REQUIRED);
                        else if (qty.length > Constant.MAX_QUANTITY_LENGTH) // valid string length
                            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.QUANTITY).replace('{1}', Constant.MAX_QUANTITY_LENGTH));

                        // Validate Amount  
                        var Amount = $(this).find('input.amount-copd').val().replace(/,/g, '');
                        if (Amount.length === 0)
                        {
                            invalidMess.push(Constant.AMOUNT + Constant.ERR_REQUIRED);
                        }                            
                        else if ((parseInt(Amount) < Constant.MIN_AMOUNT) || (parseInt(Amount) > Constant.MAX_AMOUNT))
                        {
                            invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.AMOUNT).replace('{1}', Constant.MAX_AMOUNT_LENGTH));
                        }                            
                    }
                });
                // Placement Note
                var plcNote = divParent.find('.plc-note-op').val();
                if (plcNote.length > Constant.MAX_NOTE) // valid string length
                    invalidMess.push(Constant.ERR_STRING_LENGTH.replace('{0}', Constant.NOTE).replace('{1}', Constant.MAX_NOTE));
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

    function OnChangeAcceptanceDateContract()
    {
        var acceptanceDate = $('#CONTRACT_ACCEPTANCE_DATE').val();
        var errInvalid;

        if (acceptanceDate.length === 0) {
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

        var deliveryDate = $('#CONTRACT_DELIVERY_DATE').val();
        var errInvalidDeliveryDate = iseiQ.utility.validDate(deliveryDate, Constant.DATE_FORMAT, Constant.DELIVERY_DATE);

        if (errInvalidDeliveryDate != null) {
            iseiQ.utility.showMessageModal(errInvalidDeliveryDate, true);
            return;
        }

        if (deliveryDate.length > Constant.MAX_DATE) { // valid string length
            errInvalid = Constant.DELIVERY_DATE + Constant.ERR_INCORRECT_DATE;
            iseiQ.utility.showMessageModal(errInvalid, true);
            return;
        }
        if (startDate.length > 0 && !iseiQ.utility.compareDate(startDate, acceptanceDate, Constant.DATE_FORMAT)) {
            var errAcceptanceDate = Constant.ERR_COMPARE_TOW_DATE.replace('{0}', Constant.ACCEPTANCE_DATE).replace('{1}', Constant.WORK_PERIOD_START);
            iseiQ.utility.showMessageModal(errAcceptanceDate, true);
            return;
        }
        if (deliveryDate.length > 0 && !iseiQ.utility.compareDate(deliveryDate, acceptanceDate, Constant.DATE_FORMAT)) {
            var errAcceptanceDate = Constant.ERR_COMPARE_TOW_DATE.replace('{0}', Constant.ACCEPTANCE_DATE).replace('{1}', Constant.DELIVERY_DATE);
            iseiQ.utility.showMessageModal(errAcceptanceDate,true);
            return;
        }
        if (endDate.length > 0 && !iseiQ.utility.compareDate(acceptanceDate, endDate, Constant.DATE_FORMAT)) {
            var errAcceptanceDate = Constant.ERR_COMPARE_OVE_DATE.replace('{0}', Constant.ACCEPTANCE_DATE).replace('{1}', Constant.WORK_PERIOD_END);
            iseiQ.utility.showMessageModal(errAcceptanceDate, true);
            return;
        }
        
    }
    ///////////////////////////////////////////////////////////////////
    // Coad
    // .btnAddCoad (div-coai)
    function AddCoad()
    {
        var $parentElement = $('.div-sale-items');
        var $tbLeftLastRow = $parentElement.find('.col-left tbody tr:last');
        var $tbCenterLastRow = $parentElement.find('.col-center tbody tr:last');
        var $tbRightLastRow = $parentElement.find('.col-right tbody tr:last');
        $tbLeftLastRow.after($tbLeftLastRow.prop('outerHTML'));
        $tbCenterLastRow.after($tbCenterLastRow.prop('outerHTML'));
        $tbRightLastRow.after($tbRightLastRow.prop('outerHTML'));
        $parentElement.find('.col-left tbody tr:last').removeClass('hide');
        $parentElement.find('.col-center tbody tr:last').removeClass('hide');
        $parentElement.find('.col-right tbody tr:last').removeClass('hide');
        $parentElement.find('.col-right tbody tr:last .btnDeleteCoad').show();
        ResetInitValueCoad($parentElement);
        resetArrCoad();
        iseiQ.utility.InitDatepickerMonths();
        iseiQ.utility.InitDatepickerDays();
    }

    // .btnDeleteCoad (div-coai)
    function DeleteCoad(control)
    {
        var $parentElement = $('.div-sale-items');
        var $tbRightTargetRow = $(control).parents('tr');
        var index = $tbRightTargetRow.index();
        var $tbLeftTargetRow = $($parentElement.find('.col-left tbody tr')[index]);
        var $tbCenterTargetRow = $($parentElement.find('.col-center tbody tr')[index]);
        if ($tbLeftTargetRow.find('.cont-seq-no-coai').val() == 0) {
            if ($parentElement.find('.col-left tbody tr').length > 1) {
                $tbLeftTargetRow.remove();
                $tbCenterTargetRow.remove();
                $tbRightTargetRow.remove();
            }
            else {
                $tbLeftTargetRow.addClass('hide').find('input:text, input:hidden, select').removeAttr('name');
                $tbCenterTargetRow.addClass('hide').find('input:text, input:hidden, select').removeAttr('name');
                $tbRightTargetRow.addClass('hide').find('input:text, input:hidden, select').removeAttr('name');
            }
        }
        else {
            $tbLeftTargetRow.addClass('hide');
            $tbCenterTargetRow.addClass('hide');
            $tbRightTargetRow.addClass('hide').find('.del-flg').val(1);
        }
        resetArrCoad();
        OnChangeAmountCoai();
    }

    // Reset Init value Coai
    function ResetInitValueCoad($parentElement)
    {

        var $tbLeftLastRow = $parentElement.find('.col-left tr:last');
        //var $tbCenterLastRow = $parentElement.find('.col-center tr:last');
        var $tbRightLastRow = $parentElement.find('.col-right tr:last');
        $tbLeftLastRow.find('.cont-seq-no-coai').val(0);
        $tbLeftLastRow.find('.cont-apt-seq-no-coai').val(0);
        $tbLeftLastRow.find('.dsp-order-coai').val(0);
        $tbLeftLastRow.find('.product-seq-no-coai').val(0);
        $tbLeftLastRow.find('.item-seq-no-coai').val(0);
        $tbLeftLastRow.find('.nomen-coai').val('');
        $tbLeftLastRow.find('.unit-price-coai').val(0);
        $tbLeftLastRow.find('.qty-coai').val(1);
        $tbLeftLastRow.find('.unit-type-coai').val('式');
        $tbLeftLastRow.find('.amount-coai').val(0);
        $tbLeftLastRow.find('.taxable-flg-coai').val(1);
        $tbRightLastRow.find('.del-flg').val(0);
    }

    // Action reset array div.div-coai
    function resetArrCoad()
    {
        var $parentElement = $('.div-sale-items');
        var $tbLeftRows = $parentElement.find('.col-left tbody tr');
        for (var i = 0; i < $tbLeftRows.length; i++) {
            var prefix = 'CONTRACT_OAI[' + i + '].';
            var $tbLeftTargetRow = $($parentElement.find('.col-left tbody tr')[i]);
            var $tbRightTargetRow = $($parentElement.find('.col-right tbody tr')[i]);
            $tbLeftTargetRow.find('.cont-seq-no-coai').attr('name', prefix + 'CONT_SEQ_NO');
            $tbLeftTargetRow.find('.cont-apt-seq-no-coai').attr('name', prefix + 'CONT_APT_SEQ_NO');
            $tbLeftTargetRow.find('.dsp-order-coai').attr('name', prefix + 'DSP_ORDER');
            $tbLeftTargetRow.find('.product-seq-no-coai').attr('name', prefix + 'PRODUCT_SEQ_NO');
            $tbLeftTargetRow.find('.item-seq-no-coai').attr('name', prefix + 'ITEM_SEQ_NO');
            $tbLeftTargetRow.find('.nomen-coai').attr('name', prefix + 'NOMEN');
            $tbLeftTargetRow.find('.unit-price-coai').attr('name', prefix + 'UNIT_PRICE');
            $tbLeftTargetRow.find('.qty-coai').attr('name', prefix + 'QTY');
            $tbLeftTargetRow.find('.unit-type-coai').attr('name', prefix + 'UNIT_TYPE');
            $tbLeftTargetRow.find('.amount-coai').attr('name', prefix + 'AMOUNT');
            $tbLeftTargetRow.find('.taxable-flg-coai').attr('name', prefix + 'TAXABLE_FLG');
            $tbRightTargetRow.find('.del-flg').attr('name', prefix + 'DEL_FLG');
        }
    }

    // OnChange .unit-price-coai , qty-coai
    function CalcAmountCoai(control) {
        var $parentElement = $(control).parents('tr');
        var price = $parentElement.find('.unit-price-coai').val();
        var quantity = $parentElement.find('.qty-coai').val();
        //if (price.length > 0 && quantity.length > 0) {
            var unitprice = iseiQ.utility.convertMoneyToInt(price, true);
            var amount = iseiQ.utility.roundByType($('#RoundingType').val(), unitprice * quantity);
            $parentElement.find('.amount-coai').val(iseiQ.utility.convertIntToMoney(amount));
        //}
        OnChangeAmountCoai();
    }

    // OnChange .unit-price-copd , qty-copd
    function CalcAmountCopi(control) {
        var $parentElement = $(control).parents('tr');
        var price = $parentElement.find('.unit-price-copd').val();
        var quantity = $parentElement.find('.qty-copd').val();
        //if (price.length > 0 && quantity.length > 0) {
        var unitprice = iseiQ.utility.convertMoneyToInt(price, true);
        var amount = iseiQ.utility.roundByType($('#RoundingType').val(), unitprice * quantity);
        $parentElement.find('.amount-copd').val(iseiQ.utility.convertIntToMoney(amount));
        //}
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
            var taxRate = $('#CONTRACT_OA_TAX_RATE_PS').val();
            if (taxRate.length !== 0 & taxRate.indexOf('-') === -1) {
                totalTax = iseiQ.utility.roundByType($('#RoundingType').val(), (totalAmountExcludedTax * taxRate / 100));
            }
            else {
                totalTax = 0;
            }
            if (isNaN(totalTax)) {
                $('#CONTRACT_OA_TAX').val(0);
            } else {
                $('#CONTRACT_OA_TAX').val(iseiQ.utility.convertIntToMoney(totalTax));
            }
        });
        $('#CONTRACT_OA_CONT_AMOUNT').val(iseiQ.utility.convertIntToMoney(total));
    }

    ////////////////////////////////////////////////////////////////////
    // Supplier
    //Add Supplier
    function AddSupplier()
    {
        if ($('div.div-suppliers:not(.hide)').length > 0) {
            //Get status of tax radio buttons.
            var taxCheckedValue = false;
            if ($('div.div-suppliers').first().find('.in-tax-flg-cb').length) {
                $('div.div-suppliers').first().find('.in-tax-flg-cb').each(function () {
                    if ($(this).is(':checked')) {
                        taxCheckedValue = $(this).val();
                    }
            });
            }
            $('div.div-suppliers').last().after($('div.div-suppliers').first().prop('outerHTML'));
            //There seems to be a bug with .prop method that make radio buttons lose their checked status even though
            //their attributes "checked" are still there. The following code block is placed here to ensure that the
            //checked states are intact.
            if (taxCheckedValue === "true") {
                if ($('div.div-suppliers').first().find('.in-tax-flg-cb').length) {
                    $('div.div-suppliers').first().find('.in-tax-flg-cb').last().prop('checked', true);
                }
            } else {
                if ($('div.div-suppliers').first().find('.in-tax-flg-cb').length) {
                    $('div.div-suppliers').first().find('.in-tax-flg-cb').first().prop('checked', true);
                }
            }
            var $parentElement = $('div.div-suppliers').last();
            var firstCopd = $parentElement.find('div.div-copd').first().prop('outerHTML');
            $parentElement.find('div.div-copd').after(firstCopd);
            $parentElement.find('div.div-copd').first().remove();

            var $tbLeftFirstRow = $parentElement.find('.col-left tbody tr:first');
            //var $tbCenterFirstRow = $parentElement.find('.col-center tbody tr:first');
            var $tbRightFirstRow = $parentElement.find('.col-right tbody tr:first');

            $parentElement.find('.col-left tbody tr').remove();
            //$parentElement.find('.col-center tbody tr').remove();
            $parentElement.find('.col-right tbody tr').remove();

            $parentElement.find('.col-left tbody').html($tbLeftFirstRow.prop('outerHTML'));
            //$parentElement.find('.col-center tbody').html($tbCenterFirstRow.prop('outerHTML'));
            $parentElement.find('.col-right tbody').html($tbRightFirstRow.prop('outerHTML'));

            if ($parentElement.find('.btnSelectSupplier').length == 0) {
                $parentElement.find('.op-bp-dis-name').after('<button class="btnSelectSupplier btn btn-default" type="button"><i class="btn-icon btn-search-gray"></i></button>');
            }
            if ($parentElement.find('.btnSelectSupplierPIC').length) {
                $parentElement.find('.btnSelectSupplierPIC').replaceWith('<button class="btnSelectSupplierPIC btn btn-default" type="button" disabled><i class="btn-icon btn-search-gray"></i></button>');
            }
            //Remove the name attributes of tax radio buttons. The proper names will be added later.
            if ($parentElement.find('.in-tax-flg-cb').length) {
                $parentElement.find('.in-tax-flg-cb').removeAttr('name');
            }

            ResetInitValueOp($parentElement);
            ResetSuppliersArr();
            ResetInitValueCopd($parentElement.find('.div-copd'));
            $parentElement.find('.divDeleteSupplier').removeClass('hide');
        }
        else {
            $('div.div-suppliers').removeClass('hide');
            $('div.div-suppliers').find('.divDeleteSupplier').removeClass('hide');
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
            var $splitPayment = $('.div-suppliers').find('.div-copd');
            $splitPayment.find('.col-left tbody tr:gt(0)').remove();
            $splitPayment.find('.col-center tbody tr:gt(0)').remove();
            $splitPayment.find('.col-right tbody tr:gt(0)').remove();
            ResetInitValueCopd($splitPayment);
        }
        $('#COUNT_SUPPLIERS').val(parseInt($('#COUNT_SUPPLIERS').val()) - 1);
        CalcTotalPaymentAmount();
    }

    // Reset name of new Supplier
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

            $(TargetSupplier).find('.op-closing-day').attr('name', prefix + 'CLOSING_DAY');
            $(TargetSupplier).find('.op-payment-site-type').attr('name', prefix + 'PAYMENT_SITE_TYPE');
            $(TargetSupplier).find('.op-payment-day').attr('name', prefix + 'PAYMENT_DAY');

            $(TargetSupplier).find('.cont-amount-op').attr('name', prefix + 'CONT_AMOUNT');
            $(TargetSupplier).find('.tax-rate').attr('name', prefix + 'TAX_RATE');
            $(TargetSupplier).find('.taxable-amount-temp').attr('name', prefix + 'TAXABLE_AMOUNT_TEMP');
            $(TargetSupplier).find('.tax').attr('name', prefix + 'TAX');

            $(TargetSupplier).find('.plc-note-op').attr('name', prefix + 'PLACEMENT_NOTE');
            $(TargetSupplier).find('.prv-note-op').attr('name', prefix + 'PRIVATE_NOTE');
            resetArrCopd($(TargetSupplier).find('.div-copd'));
        }        
    }

    // Reset values of new Supplier
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
        $(TargetSupplier).find('.op-closing-day').val(31);
        $(TargetSupplier).find('.op-payment-site-type').val('');
        $(TargetSupplier).find('.op-payment-day').val('');
        $(TargetSupplier).find('.cont-amount-op').val(0);
        $(TargetSupplier).find('.tax-rate').val($('#defaultTaxRate').val());
        $(TargetSupplier).find('.taxable-amount-temp').val(0);
        $(TargetSupplier).find('.tax').val(0);
        $(TargetSupplier).find('.plc-note-op').val('');
        $(TargetSupplier).find('.prv-note-op').val('');
    }

    ////////////////////////////////////////////////////////////////////
    // Copd
    // .btnAddCopd
    function AddCopd(control)
    {
        var $parentElement = $(control).parents('.div-suppliers').find('.div-copd');
        var $tbLeftLastRow = $parentElement.find('.col-left tbody tr:last');
        var $tbCenterLastRow = $parentElement.find('.col-center tbody tr:last');
        var $tbRightLastRow = $parentElement.find('.col-right tbody tr:last');
        $tbLeftLastRow.after($tbLeftLastRow.prop('outerHTML'));
        $tbCenterLastRow.after($tbCenterLastRow.prop('outerHTML'));
        $tbRightLastRow.after($tbRightLastRow.prop('outerHTML'));
        $parentElement.find('.col-left tbody tr:last').removeClass('hide');
        $parentElement.find('.col-center tbody tr:last').removeClass('hide');
        $parentElement.find('.col-right tbody tr:last').removeClass('hide');
        $parentElement.find('.col-right tbody tr:last .btnDeleteCopd').show();
        ResetInitValueCopd($parentElement);
        resetArrCopd(control);
        iseiQ.utility.InitDatepickerMonths();
        iseiQ.utility.InitDatepickerDays();
    }

    // OnClick .btnDeleteCopd
    function DeleteCopd(control)
    {
        var $parentElement = $(control).parents('div.div-copd');
        var $tbRightTargetRow = $(control).parents('tr');
        var index = $tbRightTargetRow.index();
        var $tbLeftTargetRow = $($parentElement.find('.col-left tbody tr')[index]);
        var $tbCenterTargetRow = $($parentElement.find('.col-center tbody tr')[index]);
        if ($tbLeftTargetRow.find('.cont-seq-no').val() == 0) {
            if ($parentElement.find('.col-left tbody tr').length > 1) {
                $tbLeftTargetRow.remove();
                $tbCenterTargetRow.remove();
                $tbRightTargetRow.remove();
            }
            else {
                $tbLeftTargetRow.addClass('hide').find('input:text, input:hidden, select').removeAttr('name');
                $tbCenterTargetRow.addClass('hide').find('input:text, input:hidden, select').removeAttr('name');
                $tbRightTargetRow.addClass('hide').find('input:text, input:hidden, select').removeAttr('name');
            }
        }
        else {
            $tbLeftTargetRow.addClass('hide');
            $tbCenterTargetRow.addClass('hide');
            $tbRightTargetRow.addClass('hide').find('.del-flg').val(1);
        }
        resetArrCopd($parentElement);
        CalcAmountCopd($parentElement);
    }

    // reset name Array Copd
    function resetArrCopd(control)
    {
        var $parentElement = $(control).parents('.div-suppliers').find('.div-copd');
        var $tbLeftRows = $parentElement.find('.col-left tbody tr');
        var name = $(control).parents('.div-suppliers').find('.op-bp-dis-name').attr('name');
        var subName = name.substr(0, $(control).parents('.div-suppliers').find('.op-bp-dis-name').attr('name').indexOf('.'));
        for (var i = 0; i < $tbLeftRows.length; i++) {
            var prefix = subName + '.CONTRACT_OPI[' + i + '].';
            var $tbLeftTargetRow = $($parentElement.find('.col-left tbody tr')[i]);
            var $tbRightTargetRow = $($parentElement.find('.col-right tbody tr')[i]);
            $tbLeftTargetRow.find('.cont-seq-no').attr('name', prefix + 'CONT_SEQ_NO');
            $tbLeftTargetRow.find('.cont-plc-seq-no').attr('name', prefix + 'CONT_PLC_SEQ_NO');
            $tbLeftTargetRow.find('.dsp-order').attr('name', prefix + 'DSP_ORDER');
            $tbLeftTargetRow.find('.nomen-copd').attr('name', prefix + 'NOMEN');
            $tbLeftTargetRow.find('.unit-price-copd').attr('name', prefix + 'UNIT_PRICE');
            $tbLeftTargetRow.find('.qty-copd').attr('name', prefix + 'QTY');
            $tbLeftTargetRow.find('.unit-type-copd').attr('name', prefix + 'UNIT_TYPE');
            $tbLeftTargetRow.find('.amount-copd').attr('name', prefix + 'AMOUNT');
            $tbLeftTargetRow.find('.taxable-flg-copd').attr('name', prefix + 'TAXABLE_FLG');
            $tbRightTargetRow.find('.del-flg').attr('name', prefix + 'DEL_FLG');
        }
    }

    // Reset Init value Copd
    function ResetInitValueCopd($parentElement)
    {
        var $tbLeftLastRow = $parentElement.find('.col-left tr:last');
        //var $tbCenterLastRow = $parentElement.find('.col-center tr:last');
        var $tbRightLastRow = $parentElement.find('.col-right tr:last');
        $tbLeftLastRow.find('.cont-seq-no').val(0);
        $tbLeftLastRow.find('.cont-apt-seq-no').val(0);
        $tbLeftLastRow.find('.dsp-order').val(0);
        $tbLeftLastRow.find('.nomen-copd').val('');
        $tbLeftLastRow.find('.unit-price-copd').val(0);
        $tbLeftLastRow.find('.qty-copd').val(1);
        $tbLeftLastRow.find('.unit-type-copd').val('式');
        $tbLeftLastRow.find('.amount-copd').val(0);
        $tbLeftLastRow.find('.taxable-flg-copd').val(1);
        $tbRightLastRow.find('.del-flg').val(0);
    }

    // OnChange Copd.unit-price-copd, Copd.qty-copd
    function CalcAmountCopd(control)
    {
        var total = 0;
        var parent = $(control).parents('.div-suppliers');
        var totalAmount = 0;
        $(parent).find('.unit-price-copd').each(function () {
            if (!$(this).parents('tr').hasClass('hide')) {
                var price = $(this).val();
                var quantity = $(this).parent('td').parent('tr').find('.qty-copd').val();
                var unitprice = iseiQ.utility.convertMoneyToInt(price, true);
                var amount = unitprice * quantity;
                amount = Math.round(amount);
                totalAmount += amount;
                var stramount = iseiQ.utility.convertIntToMoney(amount);
                $(this).parent('td').parent('tr').find('.amount-copd').val(stramount);
            }
        });
        $(parent).find('.cont-amount-op').val(iseiQ.utility.convertIntToMoney(totalAmount));
        CalcTotalPaymentAmount();
    }

    function CalcTotalPaymentAmount()
    {
        var totalPaymentAmount = 0;
        $('div.div-suppliers').each(function () {
            var totalAmountExcludedTax = 0, totalTax = 0;
            var amount = iseiQ.utility.convertMoneyToInt($(this).find('.cont-amount-op').val(), true);
            totalPaymentAmount += amount;
            $(this).find('div.div-copd div.col-left table.dataTable tbody tr').each(function () {
                if ($(this).find('select.taxable-flg-copd').val() === '1') {
                    var taxExcludedAmount = iseiQ.utility.convertMoneyToInt($(this).find('input.amount-copd').val(), true);
                    totalAmountExcludedTax += taxExcludedAmount;
                }
            });
            $(this).find('input.taxable-amount-temp').val(totalAmountExcludedTax);
            var taxRate = $(this).find('input.tax-rate').val();
            if (taxRate.length !== 0 & taxRate.indexOf('-') === -1) {
                totalTax = iseiQ.utility.roundByType($('#RoundingType').val(), (totalAmountExcludedTax * taxRate / 100));
            }
            else {
                totalTax = 0;
            }
            if (isNaN(totalTax)) {
                $(this).find('input.tax').val(0);
            }
            else {
                $(this).find('input.tax').val(iseiQ.utility.convertIntToMoney(totalTax));
            }
        });
        $('.txtAmountTotal').text(iseiQ.utility.convertIntToMoney(totalPaymentAmount));
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
        //alert($('#nameOpBpDspName').val());
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
}());