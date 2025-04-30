//**************************************************************************************
//* 功　能 : 出現執行中，請稍候之畫面
//* 參　數 : sTaskName ....... 執行之工作名稱，若傳入空白，則由系統預設為作業功能執行中
//**************************************************************************************
function drawWait(sTaskName){
    if(document.getElementById('divWait')){
        var obj = document.getElementById('divWait');
        obj.style.top = 0;
        obj.style.left = 0;
        obj.style.width = "100%";
        obj.style.height = "99%";
        obj.style.display = 'inline';
        if (sTaskName == "") sTaskName = "程式執行中";
        var html = "";
        html += '<div style="width: 100%; height: 100%;z-index: -999; CURSOR: wait ; background: url(../images/busybg.png);">';
        html += '<center>';
        html += '<ul style="height:220px; ">';
        html += '<li>&nbsp;</li>';
        html += '</ul>';
        html += '<ul style="font-size:14px; width:210px; height:100px; font-family:新細明體; line-height:20px; color:#FFFFCC; background:#000;-moz-opacity:0.8; opacity:0.8;">';
        html += '<li>&nbsp;</li>';
        html += '<li style="background-image: url(../images/busy.gif); text-align:center; background-repeat: no-repeat; background-position: center;">&nbsp;</li>';
        html += '<li style="height: 60px; text-align:center;"><br >' + sTaskName + ', 請稍候 ...</li>';
        html += '<li></li>';
        html += '</ul></center></div>';
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