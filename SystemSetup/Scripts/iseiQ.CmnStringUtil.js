
function replaceALL(ValueString, TargetString, ReplaceString) {
    var ReturnString = String(ValueString);
    while (ReturnString.indexOf(TargetString) != -1) {
        ReturnString = ReturnString.replace(TargetString, ReplaceString);
    }
    return ReturnString;
}

/**
  null ��ʂ̕�����ɕϊ����܂��B

  targetString  : �Ώە�����
  defaultString : �f�t�H���g������

  �߂�l�F �Ώە����� null �łȂ��ꍇ�́A�Ώە���������̂܂ܕԂ��B
           �Ώە����� null �̏ꍇ�́A �f�t�H���g�������Ԃ��B
**/
function nvl(targetString, defaultString) {
    var value = targetString;

    if (value == null) {
        value = defaultString;
    }

    return value;
}

