//document.write("<script language='javascript' src='../Scripts/jquery-1.4.4.min.js'></script>");
//document.write("<script language='javascript' src='../Scripts/jquery.calculation.js'></script>");
//document.write("<script language='javascript' src='../Scripts/jquery-ui-1.8.7.custom.min.js'></script>");
/// <reference path="../Scripts/jquery-1.4.4.min.js" />

///////////////////////////////////////////////////////////////////////////////////////////
//  clickDatePicker()：jquery日曆選擇
///////////////////////////////////////////////////////////////////////////////////////////	
function clickDatePicker(txtID) {
    //$.datepicker.setDefaults( $.datepicker.regional[ "zh-TW" ] );
    $("#" + txtID).datepicker({ changeMonth: true, changeYear: true, duration: 'fast', closeText: 'X', showButtonPanel: true, dateFormat: 'yy/mm/dd', monthNamesShort: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'], dayNamesMin: ['日', '一', '二', '三', '四', '五', '六'] });
    $("#" + txtID).focus();
    //$("#" + txtID).datepicker();
}
///////////////////////////////////////////////////////////////////////////////////////////
//  blockUI()：jquery 暫止輸入
///////////////////////////////////////////////////////////////////////////////////////////
function blockUI(btn) {
    $(document).ready(function () {
        $('#' + btn).click(function () {
            $.blockUI({ css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff'
            }
            });
        });

    });
}
///////////////////////////////////////////////////////////////////////////////////////////
//  calid()：jquery四則計算
///////////////////////////////////////////////////////////////////////////////////////////

function calid() {
    $('.GetCost').bind('keyup', recalc);
    $('.PredictPYear').bind('keyup', recalc);
    $('.OtherDepreciation').bind('keyup', recalc);
    $('.AccumDepreciation').bind('keyup', recalc);
    $('.Amount').bind('keyup', recalc);
    $('.SAmount').bind('keyup', recalc);
    $('.MAmount').bind('keyup', recalc);
    $('.DiscountMonth').bind('change', recalc);
    $('.PPYear').bind('change', recalc);
    $('.NPredictPYear').bind('keyup', recalc);
    $('.Reestimate').bind('keyup', recalc);
    $('.Maintenance').bind('keyup', recalc);
    $('.ADepreciation').bind('keyup', recalc);
    $('.AD').bind('keyup', recalc);
    $('.GC').bind('keyup', recalc);
    $('.M').bind('keyup', recalc);
    $('.RE').bind('keyup', recalc);
    $('.ER').bind('keyup', recalc);
    recalc();

    $(".JQCalendar").datepicker({
        changeMonth: true,
        changeYear: true,
        showOtherMonths: true,
        selectOtherMonths: true
    });
    //alertSize();
}

//function alertSize() {
//    var myWidth = 0, myHeight = 0;
//    if (typeof (window.innerWidth) == 'number') {
//        //Non-IE
//        myWidth = window.innerWidth;
//        myHeight = window.innerHeight;
//    } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
//        //IE 6+ in 'standards compliant mode'
//        myWidth = document.documentElement.clientWidth;
//        myHeight = document.documentElement.clientHeight;
//    } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
//        //IE 4 compatible
//        myWidth = document.body.clientWidth;
//        myHeight = document.body.clientHeight;
//    }
////    window.alert('Width = ' + myWidth);
////    window.alert('Height = ' + myHeight);
//}

function recalc() {
    //預估殘值=取得成本/(原定年限+1)
    $('.EstimateResidual').calc('Cost / (PYear+1)',
    {
        Cost: $('.GetCost'),
        PYear: $('.PredictPYear')
    },
    function (s) { return s.toFixed(0); }
    );
    //每月折舊數=(取得成本-預估殘值)/(原定年限總月數)
    $('.MonthProvision').calc('(Cost-ER) / (PYear*12)',
    {
        Cost: $('.GetCost'),
        PYear: $('.PredictPYear'),
        ER: $('.EstimateResidual')
    },
    function (s) { return s.toFixed(0); }
    );
    //折舊剩餘值計算=取得成本+改良大修+增減重估-累積折舊-預估殘值
    $('.DR').calc('famUGC+famUM+famURE-famUAD-famUER',
    {
        famUGC: $('.GC'),
        famUM: $('.M'),
        famURE: $('.RE'),
        famUAD: $('.AD'),
        famUER: $('.ER')
    },
    function (s) { return s.toFixed(0); }
    );
    //每月折舊數=(取得成本-預估殘值)/(原定年限總月數)
    $('.MP').calc('(famUGC-famUER) / (famUPYear*12)',
    {
        famUGC: $('.GC'),
        famUPYear: $('.PredictPYear'),
        famUER: $('.ER')
    },
    function (s) { return s.toFixed(0); }
    );
    //每月提列=(折舊剩餘值)/(月數)
    $('.MP').calc('(famUDR) / (famUMouth)',
    {
        famUDR: $('.DR'),
        famUMouth: $('.DMonth')
    },
    function (s) { return s.toFixed(0); }
    );
    //折舊剩餘值計算=取得成本+改良大修+增減重估-累積折舊-預估殘值
    $('.DepreciationResidual').calc('GetCost+Maintenance+Reestimate-AD-ER',
    {
        GetCost: $('.GetCost'),
        Maintenance: $('.Maintenance'),
        Reestimate: $('.Reestimate'),
        AD: $('.ADepreciation'),
        ER: $('.EstimateResidual')
    },
    function (s) { return s.toFixed(0); }
    );

    //    //每月提列計算=折舊剩餘值+改良大修+增減重估-累積折舊-預估殘值
    //    $('.MonthProvision').calc('GetCost+Maintenance+Reestimate-AD-ER',
    //    {
    //	    GetCost: $('.GetCost'),
    //	    Maintenance: $('.Maintenance'),
    //	    Reestimate: $('.Reestimate'),
    //	    AD: $('.ADepreciation'),
    //	    ER: $('.EstimateResidual')
    //    },	
    //    function (s){return s.toFixed(0);}
    //    );
    //損益=(畸零折舊+累積折舊)-取得成本+出售金額
    $('.ProfitLoss').calc('(OD+AD)-GCost+SAmount',
    {
        OD: $('.OtherDepreciation'),
        AD: $('.AccumDepreciation'),
        GCost: $('.GetCost'),
        SAmount: $('.SAmount')
    },
    function (s) { return s.toFixed(0); }
    );
//    //損益=(畸零折舊+累積折舊)-取得成本+出售金額
//    $('.IProfitLoss').calc('(faaSOD+faaSAD)-faaSGCost+faaSSAmount',
//    {
//        faaSOD: $('.IOtherDepreciation'),
//        faaSAD: $('.IAccumDepreciation'),
//        faaSGCost: $('.IGetCost'),
//        faaSSAmount: $('.ISAmount')
//    },
//    function (s) { return s.toFixed(0); }
//    );
    //損失金額=取得成本-(畸零折舊+累積折舊)
    $('.LAmount').calc('GCost-(OD+AD)',
    {
        OD: $('.OtherDepreciation'),
        AD: $('.AccumDepreciation'),
        GCost: $('.GetCost')
    },
    function (s) { return s.toFixed(0); }
    );
    //殘值增加計算=維修金額/(新定或原定年限*12-已折月份)/12+1
    $('.Residual').calc('MAmount/(Year*12-DMonth)/12+1',
    {
        DMonth: $('.DiscountMonth'),
        Year: $('.PPYear'),
        MAmount: $('.MAmount')
    },
    function (s) { return s.toFixed(0); }
    );
    //調整的預估殘值計算
    $('.NEstimateResidual').calc('Reestimate / (NPYear+1)',
    {
        Reestimate: $('.Reestimate'),
        NPYear: $('.NPredictPYear')
    },
    function (s) { return s.toFixed(0); }
    );
    //每月提列=(折舊剩餘值)/(月數)
    $('.MProvision').calc('(MCost) / (MPYear)',
    {
        MCost: $('.DR'),
        MPYear: $('.Year')
    },
    function (s) { return s.toFixed(0); }
    );

}
