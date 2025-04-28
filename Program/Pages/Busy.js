//**************************************************************************************
//* 功　能 : 出現執行中，請稍候之畫面
//* 參　數 : sTaskName ....... 執行之工作名稱，若傳入空白，則由系統預設為作業功能執行中
//**************************************************************************************
function drawWait(sTaskName){
    if(document.getElementById('divWait')){
        var l_Top = Math.round(window.screen.height/2-140);
        var l_Left = Math.round(window.screen.width/2-150);
        var obj = document.getElementById('divWait');
        obj.style.top = 0;
        obj.style.left = 0;
        obj.style.width = "100%";
        obj.style.height = "100%";
        if (sTaskName == "") sTaskName = "程式執行中";
        var html = "";
        html += '<div style="width: 100%; height: 100%;z-index: -999; CURSOR: wait ; background: url(../image/PanUI/busybg.png);" >';
        html += '<center>';
        html += '<ul style="height:300px; ">';
        html += '<li>';
        html += '&nbsp;</li>';
        html += '</ul>';
        html += '<ul style="font-size: 16px; Top:300px; width: 200px; height: 100px; font-family: 新細明體; line-height: 22px;color:#FFFFCC;background: #222;">';
        html += '<li>';
        html += '&nbsp;</li>';
        html += '<li style="background-image: url(../image/PanUI/busy.gif); background-repeat: no-repeat; background-position: 10pt center; padding-left: 5px;">';
        html += '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + sTaskName;
        html += '</li>';
        html += '<lis tyle="height: 60px;">';
        html += '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;請稍候．．';
        html += '</li>';
        html += '<li>';
        html += '</li>';
        html += '</ul>';
        html += '</center>';
        html += '</div>';
        obj.innerHTML = html;
    }
}
//**************************************************************************************
//* 功　能 : 關閉執行等待之畫面
//**************************************************************************************
function closeWait(){
    if(document.getElementById('divWait')){
        var obj = document.getElementById('divWait');
        obj.style.top = 0;
        obj.style.left = 0;
        obj.style.width = "0";
        obj.style.height = "0";
        obj.innerHTML = '';
    }
}