/*顯示民國年*/
$.extend($.datepicker, {
    formatDate: function (format, date, settings) {
        var d = date.getDate();
		var m = date.getMonth()+1;
		var y = date.getFullYear();			
		var fm = function(v){			
		    return (v<10 ? '0' : '')+v;
		};			
		return (y-1911) +'/'+ fm(m) +'/'+ fm(d);
    },
    parseDate: function (format, value, settings) {
        var v = new String(value);
        var Y,M,D;
        if(v.length==9){/*100/12/15*/
            Y = v.substring(0,3)-0+1911;
            M = v.substring(4,6)-0-1;
            D = v.substring(7,9)-0;
            return (new Date(Y,M,D));
        }else if(v.length==8){/*98/12/15*/
            Y = v.substring(0,2)-0+1911;
            M = v.substring(3,5)-0-1;
            D = v.substring(6,8)-0;
            return (new Date(Y,M,D));
        }
        return (new Date());
    },
    formatYear:function(v){
    	return '民國'+(v-1911)+'年';
		}
});

function SetDatePicker(name){

    var img;
    if(datepicker_img_base_path){
        img = datepicker_img_base_path();
    }

    $(name).datepicker({
        dateFormat: 'yyy/mm/dd', 
        showOn: 'button', 
        buttonImageOnly: true, 
        buttonImage: img,
        formatYear:function(v){return '民國'+(v-1911)+'年';}
    });

}