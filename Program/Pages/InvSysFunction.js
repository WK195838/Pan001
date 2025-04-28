//@ 計算機
function recalc() {
    $('.total_item').calc('qty * price', { qty: $('.qty_item'), price: $('.price_item') }, function (s) { return s.toFixed(0); });
    $('.TmpTax').calc('TmpTax * 1', { TmpTax: $('.TmpTax') }, function (s) { return s.toFixed(0); });
    $('.Net').calc('Sum + TmpTax', { Sum: $('.Sum'), TmpTax: $('.TmpTax') }, function (s) { return s.toFixed(0); });
    $('.InputTax').calc('InputTax * 1', { InputTax: $('.InputTax') }, function (s) { return s.toFixed(0); });
    $('.Total').calc('Net + InputTax', { Net: $('.Net'), InputTax: $('.InputTax') }, function (s) { return s.toFixed(0); });
}

function fn() {

    //@ 計算觸發欄位
    $('.qty_item').bind('keyup', recalc);
    $('.price_item').bind('keyup', recalc);
    $('.TmpTax').bind('keyup', recalc);
    recalc();

    //@ TEXTBOX自動調整高度
    $(".resizable").TextAreaExpander();

    $(".JQCalendar").datepicker({
        changeMonth: true,
        changeYear: true,
        showOtherMonths: true,
        selectOtherMonths: true
    });

    //@ GirdView 光棒效果
    $(".GV tr:odd").addClass("odd");     //奇數行設定為 "odd" 樣式
    $(".GV tr:even").addClass("even");   //偶數行設定為 "even" 樣式
    $(".GV tr").mouseover(function () { $(this).addClass("over"); })     //當 mouseover 時加入 "over" 樣式
    .mouseout(function () { $(this).removeClass("over"); })   //當 mouseout 時移除 "over" 樣式
    //.click(function(){$(this).toggleClass("tr_chouse");}) //當 click 加入或移除 "tr_chouse" 樣式，實現資料列選取



    tb_init('a.thickbox, area.thickbox, input.thickbox'); //pass where to apply thickbox
    imgLoader = new Image(); // preload image
    imgLoader.src = tb_pathToImage;

    $("#tabs").tabs();

    $(".tb").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "WebService.asmx/FetchCompanyList",
                data: "{ 'Company': '" + request.term + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            value: item.Company, Address: item.Address, Contact: item.Contact, Phone: item.Phone
                        }
                    }))
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {

            $(".AUTO1").attr("value", ui.item.Address);
            //$("#<%= txtContact.ClientID %>").attr("value", ui.item.Contact);
            //$("#<%= txtPhone.ClientID %>").attr("value", ui.item.Phone);

            return true;
        },
        delay: 300
    });



}