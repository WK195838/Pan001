var currentUpdateEvent;
var addStartDate;
var addEndDate;
var globalAllDay;


var date = new Date();
var d = date.getDate();
var m = date.getMonth();
var y = date.getFullYear();


$(document).ready(function () {
    var calendar = $('#calendar').fullCalendar({
        theme: true,
        height: 600,
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        buttonText: { //按鈕文字
            prev: '&nbsp;&#9668;&nbsp;',  // left triangle
            next: '&nbsp;&#9658;&nbsp;',  // right triangle
            prevYear: '&nbsp;&lt;&lt;&nbsp;', // <<
            nextYear: '&nbsp;&gt;&gt;&nbsp;', // >>
            today: '今天',
            month: '月',
            week: '週',
            day: '日'
        },
        firstDay: 0, //Sunday=0, Monday=1, Tuesday=2, etc.
        weekends: true, //weekends or not.
        allDayText: '全日',
        monthNames: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],
        monthNamesShort: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12'],
        dayNames: ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'],
        dayNamesShort: ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'],
        columnFormat: {
            month: 'ddd',    // Mon
            week: 'M/d ddd', // Mon 9/7
            day: 'M/d dddd'  // Monday 9/7
        },
        titleFormat: {
            month: 'yyyy年 MMMM',                             // September 2009
            week: "yyyy, MMM / d[ yyyy]{ '&#8212;'[ MMM] d}", // Sep 7 - 13 2009
            day: 'yyyy, MMM / d dddd'                  // Tuesday, Sep 8, 2009
        },
        timeFormat: {
            // for agendaWeek and agendaDay
            agenda: 'H:mm{ - H:mm}', // 5:00 - 18:30

            // for all other views
            '': 'H:mm'            // 19
        },
        axisFormat: 'H(:mm)',
        //minTime: '8:30', //可安排起始時間
        //maxTime: '18:30', //可安排結束時間
        //eventClick: updateEvent,
        //selectable: true,
        selectHelper: true,
        //select: selectDate,
        editable: true,
        events: "../UserControl/MMS/mmsCalendarJsonResponse.ashx?roomunid=" + Param,
        eventClick: function (event) {
            if (event.url) {
                window.open(event.url);
                return false;
            }
        },
        //eventDrop: eventDropped,
        //eventResize: eventResized,
        disableDragging: true,
        disableResizing: true,
        eventRender: function (event, element, monthView) {
            //使day number隨放假日與否變換顏色 -- 2012/11/23 by Desmond
            if (event.className == "holiday") {
                var one_day = 1000 * 60 * 60 * 24;
                var _Diff = Math.ceil((event.start.getTime() - monthView.visStart.getTime()) / (one_day));
                //alert(_Diff);
                var dayClass = ".fc-day" + _Diff;
                $(dayClass).addClass('holiday-color');
            }
            else if (event.className == "workday") {
                var one_day = 1000 * 60 * 60 * 24;
                var _Diff = Math.ceil((event.start.getTime() - monthView.visStart.getTime()) / (one_day));
                //alert(_Diff);
                var dayClass = ".fc-day" + _Diff;
                $(dayClass).addClass('workday-color');
            }
            //Tooltip
            element.qtip({
                content: event.description,
                position: { corner: { tooltip: 'bottomLeft', target: 'topLeft'} },
                style: {
                    border: {
                        width: 1,
                        radius: 3,
                        color: '#FF8A32'
                    },
                    padding: 10,
                    textAlign: 'center',
                    tip: true, // Give it a speech bubble tip with automatic corner detection
                    name: 'cream' // Style it according to the preset 'cream' style
                }
            });
        }
    });
    //週末加上classname "weekend" -- 2012/11/23 by Desmond
    $('table > tbody > tr > td:nth-child(6n+1)').addClass('weekend');
    //按"上個月"按鈕時清除css class
    $(".fc-button-prev span").click(function () {
        $('.holiday-color').removeClass('holiday-color');
        $('.workday-color').removeClass('workday-color');
    });
    //按"下個月"按鈕時清除css class -- 2012/11/23 by Desmond
    $(".fc-button-next span").click(function () {
        $('.holiday-color').removeClass('holiday-color');
        $('.workday-color').removeClass('workday-color');
    });   
});

//function updateEvent(event, element) {
//    //alert(event.description);

//    if ($(this).data("qtip")) $(this).qtip("destroy");

//    currentUpdateEvent = event;

//    $('#updatedialog').dialog('open');

//    $("#eventName").val(event.title);
//    $("#eventDesc").val(event.description);
//    $("#eventId").val(event.id);
//    $("#eventStart").text("" + event.start.toLocaleString());

//    if (event.end === null) {
//        $("#eventEnd").text("");
//    }
//    else {
//        $("#eventEnd").text("" + event.end.toLocaleString());
//    }

//}

//function updateSuccess(updateResult) {
//    //alert(updateResult);
//}

//function deleteSuccess(deleteResult) {
//    //alert(deleteResult);
//}

//function addSuccess(addResult) {
//// if addresult is -1, means event was not added
////    alert("added key: " + addResult);

//    if (addResult != -1) {
//        $('#calendar').fullCalendar('renderEvent',
//						{
//						    title: $("#addEventName").val(),
//						    start: addStartDate,
//						    end: addEndDate,
//						    id: addResult,
//						    description: $("#addEventDesc").val(),
//						    allDay: globalAllDay
//						},
//						true // make the event "stick"
//					);


//        $('#calendar').fullCalendar('unselect');
//    }

//}

//function UpdateTimeSuccess(updateResult) {
//    //alert(updateResult);
//}


//function selectDate(start, end, allDay) {

//    $('#addDialog').dialog('open');


//    $("#addEventStartDate").text("" + start.toLocaleString());
//    $("#addEventEndDate").text("" + end.toLocaleString());


//    addStartDate = start;
//    addEndDate = end;
//    globalAllDay = allDay;
//    //alert(allDay);

//}

//function updateEventOnDropResize(event, allDay) {

//    //alert("allday: " + allDay);
//    var eventToUpdate = {
//        id: event.id,
//        start: event.start

//    };

//    if (allDay) {
//        eventToUpdate.start.setHours(0, 0, 0);

//    }

//    if (event.end === null) {
//        eventToUpdate.end = eventToUpdate.start;

//    }
//    else {
//        eventToUpdate.end = event.end;
//        if (allDay) {
//            eventToUpdate.end.setHours(0, 0, 0);
//        }
//    }

//    eventToUpdate.start = eventToUpdate.start.format("dd-MM-yyyy tt hh:mm:ss");
//    eventToUpdate.end = eventToUpdate.end.format("dd-MM-yyyy tt hh:mm:ss");

//    PageMethods.UpdateEventTime(eventToUpdate, UpdateTimeSuccess);

//}

//function eventDropped(event, dayDelta, minuteDelta, allDay, revertFunc) {

//    if ($(this).data("qtip")) $(this).qtip("destroy");

//    updateEventOnDropResize(event, allDay);



//}

//function eventResized(event, dayDelta, minuteDelta, revertFunc) {

//    if ($(this).data("qtip")) $(this).qtip("destroy");

//    updateEventOnDropResize(event);

//}

//function checkForSpecialChars(stringToCheck) {
//    //var pattern = /[^A-Za-z0-9 ]/;
//    //return pattern.test(stringToCheck);
//    return false;
//}

//$(document).ready(function() {

//    // update Dialog
//    $('#updatedialog').dialog({
//        autoOpen: false,

//        width: 400,
//        modal: true,
//        buttons: {
//            "修改": function() {
//                //alert(currentUpdateEvent.title);
//                var eventToUpdate = {
//                    id: currentUpdateEvent.id,
//                    title: $("#eventName").val(),
//                    description: $("#eventDesc").val()
//                };

//                if (checkForSpecialChars(eventToUpdate.title) || checkForSpecialChars(eventToUpdate.description)) {
//                    alert("please enter characters: A to Z, a to z, 0 to 9, spaces");
//                }
//                else {
//                    PageMethods.UpdateEvent(eventToUpdate, updateSuccess);
//                    $(this).dialog("close");

//                    currentUpdateEvent.title = $("#eventName").val();
//                    currentUpdateEvent.description = $("#eventDesc").val();
//                    $('#calendar').fullCalendar('updateEvent', currentUpdateEvent);
//                }

//            },
//            "刪除": function() {

//                if (confirm("do you really want to delete this event?")) {

//                    PageMethods.deleteEvent($("#eventId").val(), deleteSuccess);
//                    $(this).dialog("close");
//                    $('#calendar').fullCalendar('removeEvents', $("#eventId").val());
//                }

//            }

//        }
//    });

//    //add dialog
//    $('#addDialog').dialog({
//        autoOpen: false,
//        
//        modal: true,
//        buttons: {
//            "新增": function() {

//                //alert("sent:" + addStartDate.format("dd-MM-yyyy hh:mm:ss tt") + "==" + addStartDate.toLocaleString());
//                var eventToAdd = {
//                    title: $("#addEventName").val(),
//                    description: $("#addEventDesc").val(),
//                    start: addStartDate.format("dd-MM-yyyy hh:mm:ss tt"),
//                    end: addEndDate.format("dd-MM-yyyy hh:mm:ss tt")

//                };

//                if (checkForSpecialChars(eventToAdd.title) || checkForSpecialChars(eventToAdd.description)) {
//                    alert("please enter characters: A to Z, a to z, 0 to 9, spaces");
//                }
//                else {
//                    //alert("sending " + eventToAdd.title);

//                    PageMethods.addEvent(eventToAdd, addSuccess);
//                    $(this).dialog("close");
//                }

//            }

//        }
//    });


    
