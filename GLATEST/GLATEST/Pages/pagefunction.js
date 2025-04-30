// JScript 檔
///////////////////////////////////////////////////////////////////////////////////////////
//  版面控制
///////////////////////////////////////////////////////////////////////////////////////////
var WebSite = "/ERP/";
if (document.getElementById("ctl00_hid_WebSite") != null)
    WebSite = "/"+document.getElementById("ctl00_hid_WebSite").value+"/";
window.defaultStatus = "Welcome to ERP";

	// menu 選單開關
	function hide_menu() 
	{	
		var frameobj;
		//var imgobj;
		frameobj = document.getElementById("tdMenu");
		//alert(frameobj.style.display);
		frameobj2 = document.getElementById("tdContent");
		imgobj = document.getElementById("hd_menu_pic");
		if (frameobj.style.display != "none") {
			frameobj.style.display = "none";
			frameobj2.style.width = "100%";
			imgobj.src = WebSite+"App_Themes/images/expand.png";
			imgobj.alt = "開啟功能選單";
		}
		else {
		    frameobj.style.display = "inherit";
			frameobj2.style.width = "80%";
			imgobj.src = WebSite+"App_Themes/images/collapse.png";
			imgobj.alt = "關閉功能選單";
		}
	}

	// title 選單開關
	function hide_title() 
	{
		var frameobj;
		frameobj = document.getElementById("trTitle");
		imgobj = document.getElementById("hd_title_pic");
		if (frameobj.style.display != "none") {
			frameobj.style.display = "none";
			imgobj.src = WebSite+"App_Themes/images/Down.png";			
			imgobj.alt = "開啟標題區域";
		}
		else {
		    frameobj.style.display = "inherit";
			imgobj.src = WebSite+"App_Themes/images/Up.png";
			imgobj.alt = "關閉標題區域";
		}
	}
	
	//欄位變色
	var _oldcolor;
	function setnewcolor(source)
    {
        _oldcolor=source.style.backgroundColor;
        source.style.backgroundColor="#cccccc";
    }

	function setoldcolor(source)
    {
        source.style.backgroundColor= _oldcolor
    }
///////////////////////////////////////////////////////////////////////////////////////////
//  SaveValue()：設定新增用欄位之ClientID,注意,不可使用保留字 status 做為物件名稱
///////////////////////////////////////////////////////////////////////////////////////////
function SaveValue(obj,setValue)
{
    obj.value = setValue;
    return true;
}	    

///////////////////////////////////////////////////////////////////////////////////////////
// checkColumns()：欄位檢核不可為空
///////////////////////////////////////////////////////////////////////////////////////////    
	function checkColumns(source)
    {
        if (source.value == "")
        {
            alert("資料不可為空");
            source.focus();  
            return false;
        }
        return true;
    }

///////////////////////////////////////////////////////////////////////////////////////////
// setDefvalue()：欄位檢核為空時,設定預設值
///////////////////////////////////////////////////////////////////////////////////////////    
    function setDefvalue(source, value) {
    if (source.value == "") {
        source.value = value;
    }
    return true;
}

///////////////////////////////////////////////////////////////////////////////////////////
//日期檢核
///////////////////////////////////////////////////////////////////////////////////////////
function sysY2k(number)
{return (number<1000) ? parseInt(number)+1900 : number;}

//民國年轉為西元年(不足千年即視為民國年)
function y2k(number)
{return (number<1000) ? parseInt(number)+1911 : number;}

//檢核日期是否存在,接受民國與西元年 
function checkDateExist(day, month, year)
{
 var today=new Date();
 year=((!year) ? sysY2k(today.getYear()):y2k(year));
 month=((!month) ? today.getMonth():month-1);
 
 if(!day){return false;}
 var test=new Date(year,month,day);
 
// alert(test);
// alert("y2k(test.getYear()):"+y2k(test.getYear()) +"< === > year:"+year);
// alert("test.getMonth():"+test.getMonth() +"< === > month:"+month);
// alert("test.getDate():"+test.getDate() +"< === > day:"+day);
 
 if((sysY2k(test.getYear()) == year) && (month == test.getMonth()) && (day == test.getDate()))
 {return true;}
  else
 {return false;}
}

//檢核是否符合yyyy/MM/dd格式並檢核日期是否存在,接受民國與西元年
function checkDate(theDates)
{
 var datePattern2=/^\d{2}\/(0[1-9]|1[0-2])\/(3[0-1]|[0-2][0-9])$/; 
 var datePattern3=/^\d{3}\/(0[1-9]|1[0-2])\/(3[0-1]|[0-2][0-9])$/; 
 var datePattern=/^\d{4}\/(0[1-9]|1[0-2])\/(3[0-1]|[0-2][0-9])$/; 
 
 var dates = theDates.value
 
 if (dates.length == 0)
 {
 return;
 }
 
 if(dates.match(datePattern) || dates.match(datePattern2) || dates.match(datePattern3))
 {
//  alert(dates.length);
//  alert(dates);  
  if (dates.length == 8)
  {
    //alert(dates.substring(6, 8)+"/"+dates.substring(3, 5)+"/"+dates.substring(0, 2));
    if(checkDateExist(dates.substring(6, 8), dates.substring(3, 5), dates.substring(0, 2))==true)
    {
    //alert("輸入日期正確！");
    return true;
    }    
  }
  else if (dates.length == 9)
  {
    if(checkDateExist(dates.substring(7, 9), dates.substring(4, 6), dates.substring(0, 3))==true)
    {
    //alert("輸入日期正確！");
    return true;
    }
  }
  else
  {
    if(checkDateExist(dates.substring(8, 10), dates.substring(5, 7), dates.substring(0, 4))==true)
    {
    //alert("輸入日期正確！");
    return true;
    }
  }     

   alert("輸入的日期不存在。");
   theDates.focus();
   return false;
 }
 else
 {
  alert("日期格式錯誤!\n請依照下列格式輸入日期：\n\n yyyy/MM/dd \n\n年份應為2-3碼民國年或4碼西元年");
  theDates.focus();
  return false;
 }
}
    
///////////////////////////////////////////////////////////////////////////////////////////
//  GetPromptProgram()：打開日期資料提示畫面
///////////////////////////////////////////////////////////////////////////////////////////
	function GetPromptDate(objValue) 
	{
	    return GetPromptTheDate(objValue,0,0);
	}
///////////////////////////////////////////////////////////////////////////////////////////
//  GetPromptTheDate(回傳欄位,起始年份,終止年份)：依傳入之指定年份起迄,打開日期資料提示畫面
///////////////////////////////////////////////////////////////////////////////////////////
	function GetPromptTheDate(objValue,bYear,eYear) 
		{ 
			var temp_data1,temp_data2; 
			var index;
			var RealURL;				
			  
			RealURL="../Prompt/Prompt_Date.aspx?theValue=" + encodeURI(objValue.value) + "&BeginYear=" + bYear + "&EndYear=" + eYear;
		    ReturnValue = window.showModalDialog(RealURL, "", "dialogHeight: 280px; dialogWidth: 300px; dialogTop: px; dialogLeft: px; edge: Sunken; center: Yes; help: No; resizable: yes; status: No; scroll: auto;");
			
		    if (ReturnValue != null)
               {               
                  index = ReturnValue.indexOf(':'); 
  			      if (index>-1) 
			         {
			  	 	    temp_data1 =ReturnValue.substring(0, index);
					    temp_data2 =ReturnValue.substring(index+1, ReturnValue.length);
					
			 		    objValue.value = temp_data1;
					 //if (t2!='')
					 //   t2.value = temp_data2;
			         }
			      else
				     {
			 		    objValue.value = ReturnValue;
				 	 //if (t2!='')
				 	 //   t2.value = "";
				     }				
                 }
           return false;
	    } 
///////////////////////////////////////////////////////////////////////////////////////////
//  GetPromptTheDate(回傳欄位,起始年份,終止年份)：依傳入之指定年份起迄,打開日期資料提示畫面
///////////////////////////////////////////////////////////////////////////////////////////
	function GetPromptTheDateTo2(objValue,objValue2,bYear,eYear) 
		{ 
			var temp_data1,temp_data2; 
			var index;
			var RealURL;				
			  
			RealURL="../Prompt/Prompt_Date.aspx?theValue=" + encodeURI(objValue.value) + "&BeginYear=" + bYear + "&EndYear=" + eYear;
		    ReturnValue = window.showModalDialog(RealURL, "", "dialogHeight: 280px; dialogWidth: 300px; dialogTop: px; dialogLeft: px; edge: Sunken; center: Yes; help: No; resizable: yes; status: No; scroll: auto;");
			
		    if (ReturnValue != null)
               {               
                  index = ReturnValue.indexOf(':'); 
  			      if (index>-1) 
			         {
			  	 	    temp_data1 =ReturnValue.substring(0, index);
					    temp_data2 =ReturnValue.substring(index+1, ReturnValue.length);
					
			 		    objValue.value = temp_data1;
			 		    objValue2.value = temp_data1;
					 //if (t2!='')
					 //   t2.value = temp_data2;
			         }
			      else
				     {
			 		    objValue.value = ReturnValue;
				 	 //if (t2!='')
				 	 //   t2.value = "";
				     }				
                 }
           return false;
	    } 	    
///////////////////////////////////////////////////////////////////////////////////////////
//  GetPromptWin()：打開資料提示畫面
///////////////////////////////////////////////////////////////////////////////////////////
	function GetPromptWin(objValue,theValue1,theValue2,theValue3,theValue4)
		{ 
			var temp_data1,temp_data2; 
			var index;
			var RealURL;				
			
			RealURL="../Prompt/Prompt_List.aspx?theTable=" + encodeURI(theValue1) + "&theRetColum=" + encodeURI(theValue2) + "&theShowColums=" + encodeURI(theValue3) + "&theOrderColums=" + encodeURI(theValue4)+ "&theValue=" + encodeURI(objValue.value) + "";
		    ReturnValue = window.showModalDialog(RealURL, "", "dialogHeight: 350px; dialogWidth: 550px; dialogTop: px; dialogLeft: px; edge: Sunken; center: Yes; help: No; resizable: yes; status: No; scroll: auto;");
			
		    if (ReturnValue != null)
               {               
                  index = ReturnValue.indexOf(':'); 
  			      if (index>-1) 
			         {
			  	 	    temp_data1 =ReturnValue.substring(0, index);
					    temp_data2 =ReturnValue.substring(index+1, ReturnValue.length);
					
			 		    objValue.value = temp_data1;
					 //if (t2!='')
					 //   t2.value = temp_data2;
			         }
			      else
				     {
			 		    objValue.value = ReturnValue;
				 	 //if (t2!='')
				 	 //   t2.value = "";
				     }				
                 }
           return false;
	    } 

///////////////////////////////////////////////////////////////////////////////////////////
//  GetPromptWin1()：打開資料提示畫面,傳回1個值
///////////////////////////////////////////////////////////////////////////////////////////
	function GetPromptWin1(objValue1,dHeight,dWidth,theValue1,theValue2,theValue3,theValue4)
		{ 
			var temp_data1,temp_data2; 
			var index;
			var RealURL;				
			
			RealURL="../Prompt/Prompt_List.aspx?theTable=" + encodeURI(theValue1) + "&theRetColum=" + encodeURI(theValue2) + "&theShowColums=" + encodeURI(theValue3) + "&theOrderColums=" + encodeURI(theValue4)+ "&theValue=" + encodeURI(objValue1.value) + "";
		    ReturnValue = window.showModalDialog(RealURL, "", "dialogHeight: " + dHeight + "px; dialogWidth: " + dWidth + "px; dialogTop: px; dialogLeft: px; edge: Sunken; center: Yes; help: No; resizable: yes; status: No; scroll: auto;");
			
		    if (ReturnValue != null)
               {               
                  index = ReturnValue.indexOf(':'); 
  			      if (index>-1) 
			         {
			  	 	    temp_data1 =ReturnValue.substring(0, index);
					    temp_data2 =ReturnValue.substring(index+1, ReturnValue.length);
					
			 		    objValue1.value = temp_data1;
			         }
			      else
				     {
			 		    objValue1.value = ReturnValue;
				     }				
                 }
           return false;
	    }

///////////////////////////////////////////////////////////////////////////////////////////
//  GetPromptWin2()：打開資料提示畫面,傳回2個值
///////////////////////////////////////////////////////////////////////////////////////////
	function GetPromptWin2(objValue1,objValue2,dHeight,dWidth,theValue1,theValue2,theValue3,theValue4)
		{ 
			var temp_data1,temp_data2; 
			var index;
			var RealURL;				
			
			RealURL="../Prompt/Prompt_List.aspx?theTable=" + encodeURI(theValue1) + "&theRetColum=" + encodeURI(theValue2) + "&theShowColums=" + encodeURI(theValue3) + "&theOrderColums=" + encodeURI(theValue4)+ "&theValue=" + encodeURI(objValue1.value) + "";
		    ReturnValue = window.showModalDialog(RealURL, "", "dialogHeight: "+dHeight+"px; dialogWidth: "+dWidth+"px; dialogTop: px; dialogLeft: px; edge: Sunken; center: Yes; help: No; resizable: yes; status: No; scroll: auto;");
			
		    if (ReturnValue != null)
               {               
                  index = ReturnValue.indexOf(':'); 
  			      if (index>-1) 
			         {
			  	 	    temp_data1 =ReturnValue.substring(0, index);
					    temp_data2 =ReturnValue.substring(index+1, ReturnValue.length);
					
			 		    objValue1.value = temp_data1;
			 		    objValue2.value = temp_data2;
			         }
			      else
				     {
			 		    objValue1.value = ReturnValue;
				     }				
                 }
           return false;
	    }


///////////////////////////////////////////////////////////////////////////////////////////
//  ExportExcel()：打開匯出成Excel畫面
///////////////////////////////////////////////////////////////////////////////////////////
function ExportExcel(HeaderLine,QueryLine,DataTitel,DataBody)
{
    var URL;
    var ReturnValue;       
    URL = "../Prompt/ExporttoExcel.aspx?HeaderLine="+HeaderLine+"&QueryLine="+QueryLine+"&DataTitel="+DataTitel+"&DataBody="+DataBody;
    window.open(URL, "匯出Excel", "status=no, menubar=yes, scrollbars=yes, resizable=yes, copyhistory=yes, width=800, height=600");
}

///////////////////////////////////////////////////////////////////////////////////////////
//  DSExportToExcel()：打開匯出成Excel畫面
///////////////////////////////////////////////////////////////////////////////////////////
function DSExportToExcel() {
    var URL;
    var ReturnValue;
    URL = "../Prompt/ExporttoExcel.aspx";
    window.open(URL, "匯出Excel", "status=no, menubar=yes, scrollbars=yes, resizable=yes, copyhistory=yes, width=800, height=600");
}

///////////////////////////////////////////////////////////////////////////////////////////
//  Upload()：打開上傳畫面
///////////////////////////////////////////////////////////////////////////////////////////
function Upload(objValue1,theFileKind,theSavePath,theFileName)
{
    var ReturnValue;       
    var temp_data1,temp_data2; 
	var index;
	var RealURL;				
	
	RealURL="../Prompt/Prompt_Upload.aspx?theFileKind="+theFileKind+"&theSavePath="+theSavePath+"&theFileName="+theFileName+"&objValue="+objValue1;
	ReturnValue = window.open(RealURL, "上傳", "status=no, menubar=yes, scrollbars=yes, resizable=yes, copyhistory=yes, width=300, height=200");
	//在打開新視窗成功後,即執行此段

//	if (ReturnValue != null)
//	{
//	    location.reload();
//	}

//    ReturnValue = window.showModalDialog(RealURL, "", "dialogHeight: 200px; dialogWidth: 300px; dialogTop: px; dialogLeft: px; edge: Sunken; center: Yes; help: No; resizable: yes; status: No; scroll: auto;");
//	
//    if (ReturnValue != null)
//       {               
//          index = ReturnValue.indexOf(':'); 
//	      if (index>-1) 
//	         {
//	  	 	    temp_data1 =ReturnValue.substring(0, index);
//			    temp_data2 =ReturnValue.substring(index+1, ReturnValue.length);
//			
//	 		    objValue1.value = temp_data1;
//	         }
//	      else
//		     {
//	 		    objValue1.value = ReturnValue;
//		     }				
//         }
//   return false;
}	    
///////////////////////////////////////////////////////////////////////////////////////////
//  UploadTo()：使用showModalDialog打開上傳畫面
///////////////////////////////////////////////////////////////////////////////////////////
	function UploadTo(objValue,isReload,theFileKind,theSavePath,theFileName) 
		{ 
            var ReturnValue;       
            var temp_data1,temp_data2; 
	        var index;
	        var RealURL;					
			  
			RealURL="../Prompt/Prompt_Upload.aspx?theFileKind="+theFileKind+"&theSavePath="+theSavePath+"&theFileName="+theFileName;
		    ReturnValue = window.showModalDialog(RealURL, "", "dialogHeight: 280px; dialogWidth: 50px; dialogTop: px; dialogLeft: px; edge: Sunken; center: Yes; help: No; resizable: yes; status: No; scroll: auto;");
			
		    if (ReturnValue != null)
            {
               if (isReload == "Y")
               {
                  window.location.reload();                  
               }
              index = ReturnValue.indexOf(':'); 
		      if (index>-1) 
		         {
		  	 	    temp_data1 =ReturnValue.substring(0, index);
				    temp_data2 =ReturnValue.substring(index+1, ReturnValue.length);
				
		 		    objValue.value = temp_data1;
		         }
		      else
			     {
		 		    objValue.value = ReturnValue;
			     }				
             }
                 
           return false;
	    } 

///////////////////////////////////////////////////////////////////////////////////////////
//  OpenDayCalendar()：打開日行事曆
///////////////////////////////////////////////////////////////////////////////////////////
function OpenDayCalendar(theDate,theCompany,theSeqNo,theDeptId,theEmployeeId,theCategory,theStatus)
{
    var URL;
    var ReturnValue;       
    URL = "Prompt/Calendar.aspx?theDate="+theDate+"&theCompany="+theCompany+"&theSeqNo="+theSeqNo+"&theDeptId="+theDeptId+"&theEmployeeId="+theEmployeeId+"&theCategory="+theCategory+"&theStatus="+theStatus;
    window.open(URL, "日行事曆", "status=no, menubar=yes, scrollbars=yes, resizable=yes, copyhistory=yes, width=800, height=600");
}

///////////////////////////////////////////////////////////////////////////////////////////
//  slideLine()：控制跑馬燈輪播
///////////////////////////////////////////////////////////////////////////////////////////
function slideLine(box, stf, delay, speed, h) {
    //取得id
    var slideBox = document.getElementById(box);
    //預設值 delay:幾毫秒滾動一次(1000毫秒=1秒)
    //       speed:數字越小越快，h:高度
    var delay = delay || 1000, speed = speed || 20, h = h || 20;
    var tid = null, pause = false;
    //setInterval 跟setTimeout的用法可以咕狗研究一下~
    var s = function () { tid = setInterval(slide, speed); }
    //主要動作的地方
    var slide = function () {
        //當滑鼠移到上面的時候就會暫停
        if (pause) return;
        //滾動條往下滾動 數字越大會越快但是看起來越不連貫，所以這邊用1
        slideBox.scrollTop += 1;
        //滾動到一個高度(h)的時候就停止
        if (slideBox.scrollTop % h == 0) {
            //跟 setInterval搭配使用的
            clearInterval(tid);
            //將剛剛滾動上去的前一項加回到整列的最後一項
            slideBox.appendChild(slideBox.getElementsByTagName(stf)[0]);
            //再重設滾動條到最上面
            slideBox.scrollTop = 0;
            //延遲多久再執行一次
            setTimeout(s, delay);
        }
    }
    //滑鼠移上去會暫停 移走會繼續動
    slideBox.onmouseover = function () { pause = true; }
    slideBox.onmouseout = function () { pause = false; }
    //起始的地方，沒有這個就不會動囉
    setTimeout(s, delay);
}

///////////////////////////////////////////////////////////////////////////////////////////
//  checkCapsLock()：檢查大小寫
///////////////////////////////////////////////////////////////////////////////////////////
function checkCapsLock(e,theinput) {
    var fKeyCode = 0;
    var myShiftKey = true;
    var msg = '已按下shift鍵或企圖使用大寫';
    var myMsg = '注意!英文已鎖定大寫狀態!!';
    if (document.all) {
        fKeyCode = e.keyCode;
        myShiftKey = e.shiftKey;
    }
//    else if (document.layers) {
//        fKeyCode = e.which;
//        myShiftKey = (fKeyCode == 16) ? true : false;
//    }
//    else if (document.getElementById) {
//        fKeyCode = e.which;
//        myShiftKey = (fKeyCode == 16) ? true : false;
//    }
    //沒有按住shift鍵的情況下輸入大寫
    if ((fKeyCode >= 65 && fKeyCode <= 90) && !myShiftKey) {
        if (theinput.value == "")
        { alert(myMsg); }
    }
    
    //按住shift鍵的情況下輸入小寫
    if ((fKeyCode >= 97 && fKeyCode <= 122) && myShiftKey) {        
        if (theinput.value == "")
        { alert(myMsg); }
    }
}