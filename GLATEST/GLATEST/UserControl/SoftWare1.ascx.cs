using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_SoftWare1 : System.Web.UI.UserControl, IproSoftWare1
{
    #region Iprovider 成員
    //為了將usercontrol值帶回底層頁面
    public event SoftWare1EventHandler SoftWare1Changed;
    private SoftWare1Data data;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
       

        //註冊相關js
        RegisterJS();
        //Check PostBack
        LoadData();
    }
    private void LoadData()
    {
        if (Request.Form[Stool.EventArgument] == "doSoftWare1PostBack")
        {
            string arg = Request.Form[Stool.EventTarget];
            if (!string.IsNullOrEmpty(arg))
            {
                doSetData(arg);
            }
        }
    }
    private void doSetData(string arg)
    {
        if (SoftWare1Changed != null)
        {
            char Delimiter = '＠';
            string jsonString = string.Empty;
            string[] str = arg.Split(Delimiter);
            data = new SoftWare1Data();
            if (str.Length > 0)
            {
                data.CodeCode = str[0].ToString();
                data.CodeName = str[1].ToString();
                
            }
            //將值透過interface傳到頁面
            SoftWare1Changed(this, data);
        }
        
    }
    /// <summary>
    /// 動態註冊javascript，如果頁面上有ScriptManager要透過Scriptmanager，每次Page重Load都要執行
    /// 由於畫面上可能會有多個相同的UserControl，要避免執行的function相衝突，所以function Name也都設計成unique的方式
    /// </summary>
    private void RegisterJS()
    {
        string guid = System.Guid.NewGuid().ToString().Replace("-", "_");
        string MyJS = @"
        $(document).ready(function () {
            //alert('" + guid + @"');
            SetPageValue_" + guid + @"();
            doGetData_" + guid + @"();
       
            function SetPageValue_" + guid + @"() {
                var selectedValue = """";
                selectedValue = ""20"";
                $('#" + hfCompany.ClientID + @"').val(selectedValue);
            }
            function GetChangeTxtQuery_" + guid + @"() {
                var inputText = $('#" + txtQuery.ClientID + @"').val();
                $('#" + hfQuery.ClientID + @"').val(inputText);
            }
            $('#" + btnQuery.ClientID + @"').click(function () {
                $('#" + hfPageNumber.ClientID + @"').val(""1"")
                doGetData_" + guid + @"();
            });
            $('#" + txtQuery.ClientID + @"').blur(function () {
                GetChangeTxtQuery_" + guid + @"();
            });
            $('#" + selectPage.ClientID + @"').change(function () {
                var pagenumber = $('#" + selectPage.ClientID + @" option:selected').val();
                $('#" + hfPageNumber.ClientID + @"').val(pagenumber);
                doGetData_" + guid + @"();
            });
            $('#" + imgFirst.ClientID + @"').click(function () {
                var pagenumber = ""1"";
                $('#" + hfPageNumber.ClientID + @"').val(pagenumber);
                doGetData_" + guid + @"();
            });
            $('#" + imgPrevious.ClientID + @"').click(function () {
                var pagenumber = $('#" + hfPageNumber.ClientID + @"').val();
                $('#" + hfPageNumber.ClientID + @"').val(parseInt(pagenumber) - 1);
                doGetData_" + guid + @"();
            });
            $('#" + imgNext.ClientID + @"').click(function () {
                var pagenumber = $('#" + hfPageNumber.ClientID + @"').val();
                $('#" + hfPageNumber.ClientID + @"').val(parseInt(pagenumber) + 1);
                doGetData_" + guid + @"();
            });
            $('#" + imgLast.ClientID + @"').click(function () {
                var pagenumber = $('#" + hfAllPageCount.ClientID + @"').val();
                $('#" + hfPageNumber.ClientID + @"').val(pagenumber);
                doGetData_" + guid + @"();
            });
        });
        function doGetData_" + guid + @"() {
            var Delimiter = ""＠"";
            var Company=$('#" + hfCompany.ClientID + @"').val();      
            var query = $('#" + hfQuery.ClientID + @"').val();      
                var parmwhere = ""1=1"";
                parmwhere += "" AND ("";
                parmwhere += "" CodeName LIKE '%"" + query + ""%' and  CodeID = 'SFT'"";
                parmwhere += "")"";                  
            var pagesize = $('#" + hfPageSize.ClientID + @"').val();                   
            var pagenumber = $('#" + hfPageNumber.ClientID + @"').val();               
            var tablename = 'CodeDesc';
            var keyname = 'CodeCode';
            var parm = pagesize + Delimiter + pagenumber + Delimiter + tablename + Delimiter + keyname + Delimiter + parmwhere;
            //alert(parm);
            var divGrid = $('#" + divGrid.ClientID + @"');
            $.ajax({
                type: 'POST',
                url: '" + Page.ResolveUrl("~/Services/WSDialogData.asmx/GetDialogDatas") + @"',
                data: '{""parms"":""' + parm + '""}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (msg) {
                    //alert(msg)
                    var datas = msg.d;
                    //alert(datas);
                    var allpagecnt = '';
                    var str = '';
                    if (datas != '' && datas.length > 0) {
                        str += ""<table id='table1' class='datatable' border='0' cellpadding='0' cellspacing='0'>"";
                        str += ""    <tr>"";
                        str += ""        <th>軟體編號</th>"";
                        str += ""        <th>軟體名稱</th>"";
                        str += ""    </tr>"";
                        var dataArray = $.parseJSON(datas);
                        $.each(dataArray, function (i, item) {
                            allpagecount = item.AllPageCount;
                            var CodeCode = $.trim(item.CodeCode);
                            var CodeName = $.trim(item.CodeName);
                            var title = CodeCode + "", "" +CodeName;
                            var id = '""' + CodeCode + Delimiter + CodeName+'""';
                            var argument = '""doSoftWare1PostBack""';
                            var doPostBack = ""__doPostBack("" + id + "","" + argument + "");"";    
                            str += ""<tr class='row' onclick='"" + doPostBack + ""' title='"" + title + ""'>"";
                            str += ""<td>&nbsp;"" + CodeCode + ""&nbsp;</td>"";
                            str += ""<td>&nbsp;"" + CodeName + ""&nbsp;</td>"";
                            str += ""</tr>"";
                        });
                        str += ""</table>"";
                        //alert(str);
                        $('#" + divGrid.ClientID + @"').empty();
                        $('#" + divGrid.ClientID + @"').append(str);
                        $('#" + lbPageNumber.ClientID + @"').text(pagenumber);
                        $('#" + hfAllPageCount.ClientID + @"').val(allpagecount);
                        $('#" + lbAllPageCount.ClientID + @"').text(allpagecount);
                        if (parseInt(allpagecount) <= 1) {
                            $('#" + divNavigation.ClientID + @"').attr(""style"", ""display:none"");
                        }
                        else {
                            $('#" + divNavigation.ClientID + @"').attr(""style"", ""display:block"");
                            if (parseInt(pagenumber) == 1) {
                                $('#" + imgFirst.ClientID + @"').css(""display"", ""none"");
                                $('#" + imgPrevious.ClientID + @"').css(""display"", ""none"");
                            }
                            else {
                                $('#" + imgFirst.ClientID + @"').css(""display"", ""block"");
                                $('#" + imgPrevious.ClientID + @"').css(""display"", ""block"");
                            }
                            //第最後一頁
                            if (parseInt(pagenumber) == parseInt(allpagecount)) {
                                $('#" + imgNext.ClientID + @"').css(""display"", ""none"");
                                $('#" + imgLast.ClientID + @"').css(""display"", ""none"");
                            }
                            else {
                                $('#" + imgNext.ClientID + @"').css(""display"", ""block"");
                                $('#" + imgLast.ClientID + @"').css(""display"", ""block"");
                            }
                            //下拉式頁數
                            var op = """";
                            for (var i = 1; i <= parseInt(allpagecount); i++) {
                                if (i == parseInt(pagenumber)) {
                                    op += ""<option value='"" + i + ""' selected='selected'>"" + i.toString() + ""</option>"";
                                }
                                else {
                                    op += ""<option value='"" + i + ""'>"" + i.toString() + ""</option>"";
                                }
                            }
                            $('#" + selectPage.ClientID + @"').empty();
                            $('#" + selectPage.ClientID + @"').append(op);
                        }
                    }
                    else {
                        str = """";
                    str +=""<table id=\'tableBUCase_Sales1\' class=\'datatable\' border=\'0\' cellpadding=\'0\' cellspacing=\'0\'>"";
                    str +=""   <tr>"";
                    str +=""        <td>查無任何資料!!!</td>"";
                    str +=""    </tr>"";
                    str +=""</table>"";
                    $('#" + divGrid.ClientID + @"').empty();
                    $('#" + divGrid.ClientID + @"').append(str);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(XMLHttpRequest.responseText + '\\n' + textStatus + '\\n' + errorThrown);
                }
            });
        }";

        if (ScriptManager.GetCurrent(this.Page) != null)
        {
            ScriptManager.RegisterClientScriptBlock(Page.Page, Page.GetType(), "MyJS" + guid, MyJS, true);
        }
        else
        {
            this.Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "MyJS" + guid, MyJS, true);
           
        }
    }
}