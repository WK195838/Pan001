//檢查金額
function Amountcheck(id, time, tip) {
    var re = /^\d+\.?\d*$/;
    var str = document.getElementById(id).value;
    if (!re.test(time.value)) {
        document.getElementById(id).value = str.substr(0, str.length - 1);
        document.getElementById(tip).innerHTML = "*";
    }
    else {
        document.getElementById(tip).innerHTML = "";
    }
}

//檢查數字
function numcheck(id, time, tip) {
    var re = /^\d+$/;
    var str = document.getElementById(id).value;
    if (!re.test(time.value)) {
        document.getElementById(id).value = str.substr(0, str.length - 1);
        document.getElementById(tip).innerHTML = "*";
    }
    else {
        document.getElementById(tip).innerHTML = "";
    }
}

