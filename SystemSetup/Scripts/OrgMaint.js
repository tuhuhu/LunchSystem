
var tree

var _grouptype = null;
var _targetPos = 0;
var result = null;

function setGroupType(obj) {
    tree = obj;
    var rootNode = tree.get_node("#");

    if (rootNode.children != null) {

        //int ‘Î‰ž‚Ì‚½‚ß10Œ…
        _grouptype = new Array("0", "0", "0", "0", "0", "0", "0", "0", "0", "0");
        _targetPos = 0;
        result = new Array();
        searchChildNode(rootNode);

    }

    for (var i = 0; i < result.length; i++) {
        alert(result[i]);
    }

}

function searchChildNode(targetNode) {
    var pos = 0;
    _grouptype[_targetPos] = inc(_grouptype[_targetPos]);
    pos = _targetPos;
    for (var i = 0; i < targetNode.children.length; i++) {
        var id = targetNode.children[i];
        var newNode = tree.get_node(id);

        var dom = $(newNode.text);

        //result.push(dom.attr('data-group-name') + "-" + _grouptype[_targetPos]);

        dom.attr('data-new-group-type', _grouptype[_targetPos]);

        newNode.text = dom.prop('outerHTML');

        var isparent = tree.is_parent(newNode);
        if (newNode != null && isparent) {
            _targetPos = _targetPos + 1;

            _grouptype[_targetPos] = _grouptype[_targetPos - 1] + "0";

            searchChildNode(newNode);

            _targetPos = pos;

            _grouptype[_targetPos] = inc(_grouptype[_targetPos]);

        } else {
            _grouptype[_targetPos] = inc(_grouptype[_targetPos]);
        }
    }
}

function inc(val) {
    var ans = Number(val) + 1;
    return ans.toString();
}

function addnode(ref, id) {

    var node = ref.get_node(id);
    var dom = $(node.text)

    dom.attr('data-group-name', 'new org');
    dom.attr('data-group-name-kana', 'new org');
    dom.attr('data-any-group-code', '');
    dom.attr('data-group-id', '0');
    dom.attr('data-group-type', '');
    dom.attr('data-up-group-id', '');
    dom.attr('data-new-group-type', '');
    dom.attr('data-ins-date', '');
    dom.attr('data-ins-username', '');
    dom.attr('data-upd-date', '');
    dom.attr('data-upd-username', '');

    dom.html('new org');
    var text = dom.prop('outerHTML');

    var sel = ref.create_node(node, { "type": "text", "icon": " ", "text": text });

    if (sel != "") {
        ref.edit(true);
        ref.open_node(node);
        ref.deselect_node(node);
        ref.select_node(ref.get_node(sel));
    }
}

function updatenode(ref, gname, gnamekana, anyGroupCd) {
    sel = ref.get_selected(false);
    if (!sel.length) { return false; }
    sel = sel[0];       //IDŽæ“¾

    var node = ref.get_node(sel);
    var dom = $(node.text)

    dom.attr('data-group-name', gname);
    dom.attr('data-group-name-kana', gnamekana);
    dom.attr('data-any-group-code', anyGroupCd);
    dom.html(escapeHTML(gname));
    var text = dom.prop('outerHTML');
    //alert(text);
    sel = ref.rename_node(sel, text);

}
function escapeHTML(str) {
    return str.replace(/[&"<>,']/g, function (c) {
        console.log('esc:' + c);
        return {
            '&': '&amp;',
            '"': '&quot;',
            '<': '&lt;',
            '>': '&gt;',
            ',': '&#044;',
            '\'': '&#039;'
        }[c];
    });
}
function unescapeHTML(str) {
    return str.replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&quot;/g, '"').replace(/&#039;/g, '\'').replace(/&#044;/g, ',').replace(/&amp;/g, '&');
}


function jsGetData(tree) {

    var rootNode = tree.get_node("#");

    var data = [];

    for (var i = 0; i < rootNode.children_d.length; i++) {
        var targetNode = tree.get_node(rootNode.children_d[i]);
        var dom = $(targetNode.text);
        var groupid = dom.attr('data-group-id');
        var groupname = dom.attr('data-group-name');
        var groupnamekana = dom.attr('data-group-name-kana');
        var anyGroupCd = dom.attr('data-any-group-code');
        var grouptype = dom.attr('data-group-type');
        var upgroupid = dom.attr('data-up-group-id');
        var newgrouptype = dom.attr('data-new-group-type');

        var obj = {
            "GROUP_ID": groupid,
            "GROUP_TYPE": grouptype,
            "GROUP_NAME": groupname,
            "GROUP_NAME_KANA": groupnamekana,
            "ANY_GROUP_CD": anyGroupCd,
            "UP_GROUP_ID": upgroupid,
            "GROUP_TYPE_NEW": newgrouptype,
            "COMPANY_CD": $("#SEARCH_COMPANY_CD").val(),
        };
        data.push(obj);
    }


    return data;
}