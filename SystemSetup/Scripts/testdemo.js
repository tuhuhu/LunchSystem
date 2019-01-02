function hoge(obj,obj2) {

    var a = '';

    var b = 1;

}

function CopyRowData(index) {

    var len = $("#tbAddress tbody").children().length;
    var endindex = len - 2;//最後の行は不要なため-２

    for (var i = index; i < endindex; i++) {
        var next = i + 1;
        //一つ下の行のデータを上に移動
        var BP_ADDR_SEQ_NO = $('#ADDRESS_LIST_' + next + '__BP_ADDR_SEQ_NO').val();
        var BP_ADDR_NAME = $('#ADDRESS_LIST_' + next + '__BP_ADDR_NAME').val();
        var BP_ZIP_CD = $('#ADDRESS_LIST_' + next + '__BP_ZIP_CD').val();
        var BP_ADDR_1 = $('#ADDRESS_LIST_' + next + '__BP_ADDR_1').val();
        var BP_ADDR_2 = $('#ADDRESS_LIST_' + next + '__BP_ADDR_2').val();
        var BP_TEL = $('#ADDRESS_LIST_' + next + '__BP_TEL').val();
        var BP_FAX = $('#ADDRESS_LIST_' + next + '__BP_FAX').val();
        var DISABLE_FLG = $('#ADDRESS_LIST_' + next + '__DISABLE_FLG').val();
        var INS_DATE = $('#ADDRESS_LIST_' + next + '__INS_DATE').val();
        var INS_USER_NAME = $('#ADDRESS_LIST_' + next + '__INS_USER_NAME').val();
        var UPD_DATE = $('#ADDRESS_LIST_' + next + '__UPD_DATE').val();
        var UPD_USER_NAME = $('#ADDRESS_LIST_' + next + '__UPD_USER_NAME').val();

        $('#ADDRESS_LIST_' + i + '__BP_ADDR_SEQ_NO').val(BP_ADDR_SEQ_NO);
        $('#ADDRESS_LIST_' + i + '__BP_ADDR_NAME').val(BP_ADDR_NAME);
        $('#ADDRESS_LIST_' + i + '__BP_ZIP_CD').val(BP_ZIP_CD);
        $('#ADDRESS_LIST_' + i + '__BP_ADDR_1').val(BP_ADDR_1);
        $('#ADDRESS_LIST_' + i + '__BP_ADDR_2').val(BP_ADDR_2);
        $('#ADDRESS_LIST_' + i + '__BP_TEL').val(BP_TEL);
        $('#ADDRESS_LIST_' + i + '__BP_FAX').val(BP_FAX);
        $('#ADDRESS_LIST_' + i + '__DISABLE_FLG').val(DISABLE_FLG);
        $('#ADDRESS_LIST_' + i + '__INS_DATE').val(INS_DATE);
        $('#ADDRESS_LIST_' + i + '__INS_USER_NAME').val(INS_USER_NAME);
        $('#ADDRESS_LIST_' + i + '__UPD_DATE').val(UPD_DATE);
        $('#ADDRESS_LIST_' + i + '__UPD_USER_NAME').val(UPD_USER_NAME);

        var $targetRow = $('.link-partner-address[index="' + i + '"]').text(BP_ADDR_NAME).attr('title', BP_ADDR_NAME).parents('tr');

        var DISABLE_FLG_CELL = '';

        if (DISABLE_FLG == '1') {
            DISABLE_FLG_CELL = '@CustomerRegister.DISABLE';
        }

        $targetRow.find('.bp-zip-cd').text(BP_ZIP_CD).attr('title', BP_ZIP_CD);
        $targetRow.find('.bp-adr-1').text(BP_ADDR_1).attr('title', BP_ADDR_1);
        $targetRow.find('.bp-adr-2').text(BP_ADDR_2).attr('title', BP_ADDR_2);
        $targetRow.find('.bp-disable-flg').text(DISABLE_FLG_CELL).attr('title', DISABLE_FLG);

        $targetRow.find('.delete-customer').data('bp-addres-seq-no', BP_ADDR_SEQ_NO);

        $('.delete-customer-link[index="' + index + '"]').attr('title', BP_ADDR_NAME);
        $('.delete-customer-link[index="' + index + '"]').attr('index', i);
    }
}