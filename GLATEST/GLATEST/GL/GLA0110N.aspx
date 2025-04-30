<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/GLA.master" CodeFile="GLA0110N.aspx.cs" Inherits="GLA0110N" %>

<%@ Register Src="~/UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc4" %>

<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
   
    <script type="text/javascript" language="javascript">
    <!--
//      window.onload = function() 
//      {
//          drowJournailList();
//      }

//      function __doPostBack(eventTarget, eventArgument) {
//　　    var theform = document.form1;

//　　    theform.__EVENTTARGET.value = eventTarget;
//　　    theform.__EVENTARGUMENT.value = eventArgument;
//　　    theform.submit();
        //　　  }

        //2013/10/21 KAYA:原程式為了組出摘要而重製.NET的__doPostBack,此段造成按鈕不能正常POSTBACK,故將其抽換為其它寫法
//　　  function __doPostBack() {
//　　    var theform = document.form1;
//　　    theform.submit();
//　　  }

//取得科目代號並重畫傳票分錄明細
      function GetAcctnoPopupDialog(dH, dW, dCompany, dTable, dFields, tAcctNo, lAcctNo, tReValue) 
      { 				
	      var rURL;
	      var wStyle;
	      //alert(dTable);
		    rURL= "AcctnoPopupDialog.aspx?Company=" + dCompany;
		    rURL+="&Table=" + dTable;
		    rURL+="&Fields=" + dFields;
    		
		    wStyle= "dialogHeight: " + dH + "px;";
		    wStyle+="dialogWidth: " + dW + "px;";
		    wStyle+="dialogTop: px;";
		    wStyle+="dialogLeft: px;";
		    wStyle+="edge: Sunken;";
		    wStyle+="center: Yes;";
		    wStyle+="help: No;";
		    wStyle+="resizable: yes;";
		    wStyle+="status: No;";
		    wStyle+="scroll: No;";		    
		    var ReturnValue = window.showModalDialog(rURL, "", wStyle);
		    //alert(ReturnValue);
		    var t_AcctNo = document.getElementById(tAcctNo);
		    var l_AcctNo = document.getElementById(lAcctNo);
		    var t_ReValue = document.getElementById(tReValue);
    						
		    if (ReturnValue != null)
            {
                var index = ReturnValue.indexOf(',');
                var ReturnList = ReturnValue.split(',');
    			  
			    if (index > -1) 
		        {
		            t_ReValue.innerText = ReturnValue
		            t_AcctNo.innerText = ReturnList[0];  //科目代號
		            l_AcctNo.innerText = ReturnList[1];  //科目名稱
		            //重畫傳票分錄明細
		            //alert(ReturnValue);
		            drowJournailDetail(ReturnValue);
		            //__doPostBack(t_AcctNo.id,"TextChanged");
		            
		        } else {
		            t_ReValue.innerText = "";
		        }
		        return true;
                   
		    }
        return false;
      }	
      //畫傳票分錄明細
      function drowJournailDetail(txtResult) {
          var companyName = "cphEEOC_DrpCompanyList_companyList";
          //alert("drowJournailDetail" + txtResult);
          //重挑選科目相關「借方金額、貸方金額、比率、摘要」皆清空
          document.getElementById('<%=txtDesc1.ClientID %>').innerText = "";
          document.getElementById('<%=txtDesc2.ClientID %>').innerText = "";
          document.getElementById('<%=txtDesc3.ClientID %>').innerText = "";
          document.getElementById('<%=txtDesc4.ClientID %>').innerText = "";
          document.getElementById('<%=txtDesc5.ClientID %>').innerText = "";
          document.getElementById('<%=txtDesc6.ClientID %>').innerText = "";
          document.getElementById('<%=txtDesc7.ClientID %>').innerText = "";
          document.getElementById('<%=lblDesc1.ClientID %>').innerText = "";
          document.getElementById('<%=lblDesc2.ClientID %>').innerText = "";
          document.getElementById('<%=lblDesc3.ClientID %>').innerText = "";
          document.getElementById('<%=lblDesc4.ClientID %>').innerText = "";
          document.getElementById('<%=lblDesc5.ClientID %>').innerText = "";
          document.getElementById('<%=lblDesc6.ClientID %>').innerText = "";
          document.getElementById('<%=lblDesc7.ClientID %>').innerText = "";
          document.getElementById('<%=txtValueText1.ClientID %>').innerText = "";
          document.getElementById('<%=txtValueText2.ClientID %>').innerText = "";
          document.getElementById('<%=txtValueText3.ClientID %>').innerText = "";
          document.getElementById('<%=txtValueText4.ClientID %>').innerText = "";
          document.getElementById('<%=txtValueText5.ClientID %>').innerText = "";
          document.getElementById('<%=txtValueText6.ClientID %>').innerText = "";
          document.getElementById('<%=txtValueText7.ClientID %>').innerText = "";
          document.getElementById('<%=txtReValue1.ClientID %>').innerText = "";
          document.getElementById('<%=txtReValue2.ClientID %>').innerText = "";
          document.getElementById('<%=txtReValue3.ClientID %>').innerText = "";
          document.getElementById('<%=txtReValue4.ClientID %>').innerText = "";
          document.getElementById('<%=txtReValue5.ClientID %>').innerText = "";
          document.getElementById('<%=txtReValue6.ClientID %>').innerText = "";
          document.getElementById('<%=txtReValue7.ClientID %>').innerText = "";
          document.getElementById('<%=txtDBMoney.ClientID %>').innerText = "";
          document.getElementById('<%=txtCRMoney.ClientID %>').innerText = "";
          document.getElementById('<%=txtValueText10.ClientID %>').innerText = "";
          document.getElementById('<%=txtP2.ClientID %>').innerText = "";
          //txtResult來自[選擇資料項]對話視窗
          //alert(txtResult);
          var rows = txtResult.split(',');
        if (rows.length > 0) {
          //第一行
          if (rows[5] != "") {
            document.getElementById('<%=txtDesc1.ClientID%>').style.display = "inline";
            document.getElementById('<%=txtDesc1.ClientID %>').innerText = rows[19];
            document.getElementById('<%=txtValueText1.ClientID %>').style.display = "inline";
            document.getElementById('<%=lblDesc1.ClientID %>').style.display = "inline";
            if (rows[12] == "Y") {
              document.getElementById('<%=txtInputYN1.ClientID %>').innerText = rows[12];
            } else {
              document.getElementById('<%=txtInputYN1.ClientID %>').innerText = "";
              rows[12] = "N";
            }
            document.getElementById('<%=txtReValue1.ClientID %>').innerText = rows[5] + "\x08" + rows[19] + "\x08" + rows[12] + "\x08" + rows[26];
            //document.getElementById("txtReValue1").innerText = document.getElementById("txtReValue1").value.replace("&nbsp;","N"); 
            document.getElementById('<%=txtValueText1.ClientID %>').innerText = "";
            document.getElementById('<%=lblDesc1.ClientID %>').innerText = "";
            if (rows[26] == "Y") {
              if (document.getElementById('<%=txtJournailOPMode.ClientID %>').value == "NEW" || document.getElementById('<%=txtJournailOPMode.ClientID %>').value == "EDIT") {
                document.getElementById('<%=imgICO1.ClientID %>').style.display = "inline";
                document.getElementById('<%=imgICO1.ClientID %>').style.cursor = "hand";
              } else {
                document.getElementById('<%=imgICO1.ClientID %>').style.display = "none";
              }
              document.getElementById('<%=imgICO1.ClientID %>').onclick= function () {  
                GetPopupDialog(530, 400, 
                    document.getElementById(companyName).options[document.getElementById(companyName).selectedIndex].value, 
                    rows[5], "CodeCode,CodeName", 
                    "<%=txtValueText1.ClientID %>", 
                    "<%=lblDesc1.ClientID %>", 
                    "<%=txtReValue1.ClientID %>",
                    document.getElementById('<%=txtReValue1.ClientID %>').value);
              }
              //alert(rows[5]);
            } else {
              document.getElementById('<%=imgICO1.ClientID %>').style.display = "none";
            }
          } else {
            document.getElementById('<%=txtDesc1.ClientID%>').style.display = "none";
            document.getElementById('<%=txtValueText1.ClientID %>').style.display = "none";
            document.getElementById('<%=lblDesc1.ClientID %>').style.display = "none";
            document.getElementById('<%=imgICO1.ClientID %>').style.display = "none";
            document.getElementById('<%=txtInputYN1.ClientID %>').innerText = "";
            document.getElementById('<%=txtReValue1.ClientID %>').innerText = "\x08\x08N\x08N";
          }
          //__doPostBack("txtValueText1","TextChanged");
          //第二行
          if (rows[6] != "") {
            document.getElementById('<%=txtDesc2.ClientID %>').style.display = "inline";
            document.getElementById('<%=txtDesc2.ClientID %>').innerText = rows[20];
            document.getElementById('<%=txtValueText2.ClientID %>').style.display = "inline";
            document.getElementById('<%=lblDesc2.ClientID %>').style.display = "inline";
            if (rows[13] == "Y") {
              document.getElementById('<%=txtInputYN2.ClientID %>').innerText = rows[13];
            } else {
              document.getElementById('<%=txtInputYN2.ClientID %>').innerText = "";
              rows[13] = "N";
            }
            document.getElementById('<%=txtReValue2.ClientID %>').innerText = rows[6] + "\x08" + rows[20] + "\x08" + rows[13] + "\x08" + rows[27];
            //document.getElementById("txtReValue2").innerText = document.getElementById("txtReValue2").value.replace("&nbsp;","N"); 
            document.getElementById('<%=txtValueText2.ClientID %>').innerText = "";
            document.getElementById('<%=lblDesc2.ClientID %>').innerText = "";
            if (rows[27] == "Y") {
              if (document.getElementById('<%=txtJournailOPMode.ClientID %>').value == "NEW" || document.getElementById("txtJournailOPMode").value == "EDIT") {
                document.getElementById('<%=imgICO2.ClientID %>').style.display = "inline";
                document.getElementById('<%=imgICO2.ClientID %>').style.cursor = "hand";
              } else {
                document.getElementById('<%=imgICO2.ClientID %>').style.display = "none";
              }
              document.getElementById('<%=imgICO2.ClientID%>').onclick= function () {  
                GetPopupDialog(530, 400, 
                
                    document.getElementById(companyName).options[document.getElementById(companyName).selectedIndex].value, 
                    rows[6], "CodeCode,CodeName", 
                    "<%=txtValueText2.ClientID %>", 
                    "<%=lblDesc2.ClientID %>", 
                    "<%=txtReValue2.ClientID %>",
                    document.getElementById('<%=txtReValue2.ClientID %>').value);
              }
            } else {
              document.getElementById('<%=imgICO2.ClientID %>').style.display = "none";
            }
          } else {
            document.getElementById('<%=txtDesc2.ClientID %>').style.display = "none";
            document.getElementById('<%=txtValueText2.ClientID %>').style.display = "none";
            document.getElementById('<%=lblDesc2.ClientID %>').style.display = "none";
            document.getElementById('<%=imgICO2.ClientID %>').style.display = "none";
            document.getElementById('<%=txtInputYN2.ClientID %>').innerText = "";
            document.getElementById('<%=txtReValue2.ClientID %>').innerText = "\x08\x08N\x08N";
          }
          //__doPostBack("txtValueText2","TextChanged");
          //第三行
          if (rows[7] != "") {
            document.getElementById('<%=txtDesc3.ClientID %>').style.display = "inline";
            document.getElementById('<%=txtDesc3.ClientID %>').innerText = rows[21];
            document.getElementById('<%=txtValueText3.ClientID %>').style.display = "inline";
            document.getElementById('<%=lblDesc3.ClientID %>').style.display = "inline";
            if (rows[14] == "Y") {
              document.getElementById('<%=txtInputYN3.ClientID %>').innerText = rows[14];
            } else {
              document.getElementById('<%=txtInputYN3.ClientID %>').innerText = "";
              rows[14] = "N";
            }
            document.getElementById('<%=txtReValue3.ClientID %>').innerText = rows[7] + "\x08" + rows[21] + "\x08" + rows[14] + "\x08" + rows[28];
            //document.getElementById("txtReValue3").innerText = document.getElementById("txtReValue3").value.replace("&nbsp;","N"); 
            document.getElementById('<%=txtValueText3.ClientID %>').innerText = "";
            document.getElementById('<%=lblDesc3.ClientID %>').innerText = "";
            if ((rows[28] == "Y")) {
              if (document.getElementById('<%=txtJournailOPMode.ClientID %>').value == "NEW" || document.getElementById('<%=txtJournailOPMode.ClientID %>').value == "EDIT") {
                document.getElementById('<%=imgICO3.ClientID %>').style.display = "inline";
                document.getElementById('<%=imgICO3.ClientID %>').style.cursor = "hand";
              } else {
                document.getElementById('<%=imgICO3.ClientID %>').style.display = "none";
              }
              document.getElementById('<%=imgICO3.ClientID%>').onclick= function () {  
                GetPopupDialog(700, 400, 
                  document.getElementById(companyName).options[document.getElementById(companyName).selectedIndex].value, 
                    rows[7], "CodeCode,CodeName", 
                    "<%=txtValueText3.ClientID %>", 
                    "<%=lblDesc3.ClientID %>", 
                    "<%=txtReValue3.ClientID %>",
                    document.getElementById('<%=txtReValue3.ClientID %>').value);
              }
            } else {
              document.getElementById('<%=imgICO3.ClientID %>').style.display = "none";
            }
          } else {
            document.getElementById('<%=txtDesc3.ClientID %>').style.display = "none";
            document.getElementById('<%=txtValueText3.ClientID %>').style.display = "none";
            document.getElementById('<%=lblDesc3.ClientID %>').style.display = "none";
            document.getElementById('<%=imgICO3.ClientID %>').style.display = "none";
            document.getElementById('<%=txtInputYN3.ClientID %>').innerText = "";
            document.getElementById('<%=txtReValue3.ClientID %>').innerText = "\x08\x08N\x08N";
          }
          //__doPostBack("txtValueText3","TextChanged");
          //第四行
          if (rows[8] != "") {
            document.getElementById('<%=txtDesc4.ClientID %>').style.display = "inline";
            document.getElementById('<%=txtDesc4.ClientID %>').innerText = rows[22];
            document.getElementById('<%=txtValueText4.ClientID %>').style.display = "inline";
            document.getElementById('<%=lblDesc4.ClientID %>').style.display = "inline";
            if (rows[15] == "Y") {
              document.getElementById('<%=txtInputYN4.ClientID %>').innerText = rows[15];
            } else {
              document.getElementById('<%=txtInputYN4.ClientID %>').innerText = "";
              rows[15] = "N";
            }
            document.getElementById('<%=txtReValue4.ClientID %>').innerText = rows[8] + "\x08" + rows[22] + "\x08" + rows[15] + "\x08" + rows[29];
            //document.getElementById("txtReValue4").innerText = document.getElementById("txtReValue4").value.replace("&nbsp;","N"); 
            document.getElementById('<%=txtValueText4.ClientID %>').innerText = "";
            document.getElementById('<%=lblDesc4.ClientID %>').innerText = "";
            if (rows[29] == "Y") {
              if (document.getElementById('<%=txtJournailOPMode.ClientID %>').value == "NEW" || document.getElementById('<%=txtJournailOPMode.ClientID %>').value == "EDIT") {
                document.getElementById('<%=imgICO4.ClientID %>').style.display = "inline";
                document.getElementById('<%=imgICO4.ClientID %>').style.cursor = "hand";
              } else {
                document.getElementById('<%=imgICO4.ClientID %>').style.display = "none";
              }
              document.getElementById('<%=imgICO4.ClientID %>').onclick= function () {  
                GetPopupDialog(530, 400, 
                   document.getElementById(companyName).options[document.getElementById(companyName).selectedIndex].value, 
                    rows[8], "CodeCode,CodeName", 
                    "<%=txtValueText4.ClientID %>", 
                    "<%=lblDesc4.ClientID %>", 
                    "<%=txtReValue4.ClientID %>",
                    document.getElementById('<%=txtReValue4.ClientID %>').value);
              }
            } else {
              document.getElementById('<%=imgICO4.ClientID %>').style.display = "none";
            }
          } else {
            document.getElementById('<%=txtDesc4.ClientID %>').style.display = "none";
            document.getElementById('<%=txtValueText4.ClientID %>').style.display = "none";
            document.getElementById('<%=lblDesc4.ClientID %>').style.display = "none";
            document.getElementById('<%=imgICO4.ClientID %>').style.display = "none";
            document.getElementById('<%=txtInputYN4.ClientID %>').innerText = "";
            document.getElementById('<%=txtReValue4.ClientID %>').innerText = "\x08\x08N\x08N";
          }
          //__doPostBack("txtValueText4","TextChanged");
          //第五行
          if (rows[9] != "") {
            document.getElementById('<%=txtDesc5.ClientID %>').style.display = "inline";
            document.getElementById('<%=txtDesc5.ClientID %>').innerText = rows[23];
            document.getElementById('<%=txtValueText5.ClientID %>').style.display = "inline";
            document.getElementById('<%=lblDesc5.ClientID %>').style.display = "inline";
            if (rows[16] == "Y") {
              document.getElementById('<%=txtInputYN5.ClientID %>').innerText = rows[16];
            } else {
              document.getElementById('<%=txtInputYN5.ClientID %>').innerText = "";
              rows[16] = "N";
            }
            document.getElementById('<%=txtReValue5.ClientID %>').innerText = rows[9] + "\x08" + rows[23] + "\x08" + rows[16] + "\x08" + rows[30];
            //document.getElementById("txtReValue5").innerText = document.getElementById("txtReValue5").value.replace("&nbsp;","N"); 
            document.getElementById('<%=txtValueText5.ClientID %>').innerText = "";
            document.getElementById('<%=lblDesc5.ClientID %>').innerText = "";
            if (rows[30] == "Y") {
              if (document.getElementById('<%=txtJournailOPMode.ClientID %>').value == "NEW" || document.getElementById('<%=txtJournailOPMode.ClientID %>').value == "EDIT") {
                document.getElementById('<%=imgICO5.ClientID %>').style.display = "inline";
                document.getElementById('<%=imgICO5.ClientID %>').style.cursor = "hand";
              } else {
                document.getElementById('<%=imgICO5.ClientID %>').style.display = "none";
            }
              document.getElementById('<%=imgICO5.ClientID %>').onclick= function () {  
                GetPopupDialog(530, 400, 
                   document.getElementById(companyName).options[document.getElementById(companyName).selectedIndex].value, 
                    rows[9], "CodeCode,CodeName", 
                    "<%=txtValueText5.ClientID %>", 
                    "<%=lblDesc5.ClientID %>", 
                    "<%=txtReValue5.ClientID %>",
                    document.getElementById('<%=txtReValue5.ClientID %>').value);
              }
            } else {
              document.getElementById('<%=imgICO5.ClientID %>').style.display = "none";
            }
          } else {
            document.getElementById('<%=txtDesc5.ClientID %>').style.display = "none";
            document.getElementById('<%=txtValueText5.ClientID %>').style.display = "none";
            document.getElementById('<%=lblDesc5.ClientID %>').style.display = "none";
            document.getElementById('<%=imgICO5.ClientID %>').style.display = "none";
            document.getElementById('<%=txtInputYN5.ClientID %>').innerText = "";
            document.getElementById('<%=txtReValue5.ClientID %>').innerText = "\x08\x08N\x08N";
          }
          //__doPostBack("txtValueText5","TextChanged");
          //第六行
          if (rows[10] != "") {
            document.getElementById('<%=txtDesc6.ClientID %>').style.display = "inline";
            document.getElementById('<%=txtDesc6.ClientID %>').innerText = rows[24];
            document.getElementById('<%=txtValueText6.ClientID %>').style.display = "inline";
            document.getElementById('<%=lblDesc6.ClientID %>').style.display = "inline";
            if (rows[17] == "Y") {
              document.getElementById('<%=txtInputYN6.ClientID %>').innerText = rows[17];
            } else {
              document.getElementById('<%=txtInputYN6.ClientID %>').innerText = "";
              rows[17] = "N";
            }
            document.getElementById('<%=txtReValue6.ClientID %>').innerText = rows[10] + "\x08" + rows[24] + "\x08" + rows[17] + "\x08" + rows[31];
            //document.getElementById("txtReValue6").innerText = document.getElementById("txtReValue6").value.replace("&nbsp;","N"); 
            document.getElementById('<%=txtValueText6.ClientID %>').innerText = "";
            document.getElementById('<%=lblDesc6.ClientID %>').innerText = "";
            if (rows[31] == "Y") {
              if (document.getElementById('<%=txtJournailOPMode.ClientID %>').value == "NEW" || document.getElementById('<%=txtJournailOPMode.ClientID %>').value == "EDIT") {
                document.getElementById('<%=imgICO6.ClientID %>').style.display = "inline";
                document.getElementById('<%=imgICO6.ClientID %>').style.cursor = "hand";
              } else {
                document.getElementById('<%=imgICO6.ClientID %>').style.display = "none";
              }
              document.getElementById('<%=imgICO6.ClientID %>').onclick= function () {  
                GetPopupDialog(530, 400, 
                   document.getElementById(companyName).options[document.getElementById(companyName).selectedIndex].value, 
                    rows[10], "CodeCode,CodeName", 
                    "<%=txtValueText6.ClientID %>", 
                    "<%=lblDesc6.ClientID %>", 
                    "<%=txtReValue6.ClientID %>",
                    document.getElementById('<%=txtReValue6.ClientID %>').value);
              }
            } else {
              document.getElementById('<%=imgICO6.ClientID %>').style.display = "none";
            }
          } else {
            document.getElementById('<%=txtDesc6.ClientID %>').style.display = "none";
            document.getElementById('<%=txtValueText6.ClientID %>').style.display = "none";
            document.getElementById('<%=lblDesc6.ClientID%>').style.display = "none";
            document.getElementById('<%=imgICO6.ClientID %>').style.display = "none";
            document.getElementById('<%=txtInputYN6.ClientID %>').innerText = "";
            document.getElementById('<%=txtReValue6.ClientID %>').innerText = "\x08\x08N\x08N";
          }
          //__doPostBack("txtValueText6","TextChanged");
          //第七行
          if (rows[11] != "") {
            document.getElementById('<%=txtDesc7.ClientID %>').style.display = "inline";
            document.getElementById('<%=txtDesc7.ClientID %>').innerText = rows[25];
            document.getElementById('<%=txtValueText7.ClientID %>').style.display = "inline";
            document.getElementById('<%=lblDesc7.ClientID %>').style.display = "inline";
            if (rows[18] == "Y") {
              document.getElementById('<%=txtInputYN7.ClientID %>').innerText = rows[18];
            } else {
              document.getElementById('<%=txtInputYN7.ClientID %>').innerText = "";
              rows[18] = "N";
            }
            document.getElementById('<%=txtReValue7.ClientID %>').innerText = rows[11] + "\x08" + rows[25] + "\x08" + rows[18] + "\x08" + rows[32];
            //document.getElementById("txtReValue7").innerText = document.getElementById("txtReValue7").value.replace("&nbsp;","N"); 
            document.getElementById('<%=txtValueText7.ClientID %>').innerText = "";
            document.getElementById('<%=lblDesc7.ClientID %>').innerText = "";
            if (rows[32] == "Y") {
              if (document.getElementById('<%=txtJournailOPMode.ClientID %>').value == "NEW" || document.getElementById('<%=txtJournailOPMode.ClientID %>').value == "EDIT") {
                document.getElementById('<%=imgICO7.ClientID %>').style.display = "inline";
                document.getElementById('<%=imgICO7.ClientID %>').style.cursor = "hand";
              } else {
                document.getElementById('<%=imgICO7.ClientID %>').style.display = "none";
              }
              document.getElementById('<%=imgICO7.ClientID %>').onclick= function () {  
                GetPopupDialog(530, 400, 
                    document.getElementById(companyName).options[document.getElementById(companyName).selectedIndex].value, 
                    rows[11], "CodeCode,CodeName", 
                    "<%=txtValueText7.ClientID %>", 
                    "<%=lblDesc7.ClientID %>", 
                    "<%=txtReValue7.ClientID %>",
                    document.getElementById('<%=txtReValue7.ClientID %>').value);
              }
            } else {
              document.getElementById('<%=imgICO7.ClientID %>').style.display = "none";
            }
          } else {
            document.getElementById('<%=txtDesc7.ClientID %>').style.display = "none";
            document.getElementById('<%=txtValueText7.ClientID %>').style.display = "none";
            document.getElementById('<%=lblDesc7.ClientID %>').style.display = "none";
            document.getElementById('<%=imgICO7.ClientID%>').style.display = "none";
            document.getElementById('<%=txtInputYN7.ClientID %>').innerText = "";
            document.getElementById('<%=txtReValue7.ClientID %>').innerText = "\x08\x08N\x08N";
          }
//          if (document.getElementById("txtJournailOPMode").value == "NEW" || document.getElementById("txtJournailOPMode").value == "EDIT")
//          {
//            document.getElementById("txtValueText1").readOnly = false;
//            document.getElementById("txtValueText2").readOnly = false;
//            document.getElementById("txtValueText3").readOnly = false;
//            document.getElementById("txtValueText4").readOnly = false;
//            document.getElementById("txtValueText5").readOnly = false;
//            document.getElementById("txtValueText6").readOnly = false;
//            document.getElementById("txtValueText7").readOnly = false;
//          } else {
//            document.getElementById("txtValueText1").readOnly = true;
//            document.getElementById("txtValueText2").readOnly = true;
//            document.getElementById("txtValueText3").readOnly = true;
//            document.getElementById("txtValueText4").readOnly = true;
//            document.getElementById("txtValueText5").readOnly = true;
//            document.getElementById("txtValueText6").readOnly = true;
//            document.getElementById("txtValueText7").readOnly = true;
//          }
          //__doPostBack("txtValueText7","TextChanged");
        }
        return false;
      }
      
      function formatCurrency(strValue) {
	      strValue = strValue.toString().replace(/\$|\,/g,'');
	      dblValue = parseFloat(strValue);

	      blnSign = (dblValue == (dblValue = Math.abs(dblValue)));
	      dblValue = Math.floor(dblValue*100+0.50000000001);
	      intCents = dblValue%100;
	      strCents = intCents.toString();
	      dblValue = Math.floor(dblValue/100).toString();
	      if(intCents<10)
		      strCents = "0" + strCents;
	      for (var i = 0; i < Math.floor((dblValue.length-(1+i))/3); i++)
	          dblValue = dblValue.substring(0, dblValue.length - (4 * i + 3)) + ',' +
		      dblValue.substring(dblValue.length-(4*i+3));
	      //return (((blnSign)?'':'-') + '$' + dblValue + '.' + strCents);
	      return (((blnSign)?'':'-') + dblValue + '.' + strCents);
      }
      
      function GetPopupDialog(dH, dW, dCompany, dCodeID, dFields, tCode, lCode, tReValue, dHDesc) 
      { 				
	      var rURL;
	      var wStyle;
    			  
		    rURL= "PopupDialog.aspx?Company=" + dCompany;
		    rURL+="&CodeID=" + dCodeID;
		    rURL+="&Fields=" + dFields;
		    //alert(rURL);
    		
		    wStyle= "dialogHeight: " + dH + "px;";
		    wStyle+="dialogWidth: " + dW + "px;";
		    wStyle+="dialogTop: px;";
		    wStyle+="dialogLeft: px;";
		    wStyle+="edge: Sunken;";
		    wStyle+="center: Yes;";
		    wStyle+="help: No;";
		    wStyle+="resizable: yes;";
		    wStyle+="status: No;";
		    //wStyle+="scroll: No;";
		    wStyle+="overflow-x:hidden;";//橫向捲軸
		    wStyle += "overflow-y:scroll;"; //直向捲軸
		    //alert(wStyle);
		    var ReturnValue = window.showModalDialog(rURL, "", wStyle);
		    //alert(ReturnValue);
		    var t_Code = document.getElementById(tCode);
		    var l_Code = document.getElementById(lCode);
		    var t_ReValue = document.getElementById(tReValue);
    						
		    if (ReturnValue != null)
            {
                var index = ReturnValue.indexOf(',');
                var ReturnList = ReturnValue.split(',');
    			  
			      if (index > -1) 
		        {
		            t_ReValue.innerText = dHDesc + "\x08" + ReturnList[0] + "\x08" + ReturnList[1];
		            t_Code.innerText = ReturnList[0];  //代號
		            l_Code.innerText = ReturnList[1];  //名稱
		            //__doPostBack(t_Code.id,"TextChanged");

		            //摘要組合
		            AsmMark();
		        } else {
		            t_ReValue.innerText = "\x08\x08N\x08N";
		        }
		        return true;   
		    }
        return false;
      }

      //選擇一筆分錄
      function SelectItem(sRow)
      {
        var companyName = "cphEEOC_DrpCompanyList_companyList";
        //alert(sRow);
        document.getElementById('<%=txtOPMode.ClientID %>').innerText = "select";
        document.getElementById('<%=txtSingleJournailzing.ClientID %>').innerText = sRow;
        document.getElementById('<%=lblLegend.ClientID %>').innerText = " 傳票分錄明細 ";
        //選擇就關閉按鈕
        document.getElementById('<%=imgbtnOKJournailzing.ClientID %>').style.display = "none";
        document.getElementById('<%= imgbtnReset.ClientID %>').style.display = "none";

        var Items = sRow.split('\x06');
        for (var j = 0; j < Items.length; j++) {
          //------------------傳票分錄列表--------------------
            if (j == 0) {
            document.getElementById('<%=lblSNo.ClientID %>').style.display = "";
            document.getElementById('<%=txtSNo.ClientID %>').style.display = "";
            document.getElementById('<%=txtSNo.ClientID %>').innerText = Items[j];
            document.getElementById('<%=txtOPMode.ClientID %>').innerText = "select=" + Items[j];
          } else if (j == 1) {
            document.getElementById('<%=txtAcctNo.ClientID %>').innerText = Items[j];
          } else if (j == 3) {
            document.getElementById('<%=lblAcctNo.ClientID %>').innerText = Items[j];
          } else if (j == 4) {
            document.getElementById('<%=txtValueText10.ClientID %>').innerText = Items[j];
          //借方金額
          } else if (j == 5) {
            if (parseFloat(Items[j]) == 0.00) {
              document.getElementById('<%=txtDBMoney.ClientID %>').innerText = "";
            } else {
              document.getElementById('<%=txtDBMoney.ClientID %>').innerText = formatCurrency(Items[j]);
            }
          //貸方金額
          } else if (j == 6) {
            if (parseFloat(Items[j]) == 0.00) {
              document.getElementById('<%=txtCRMoney.ClientID %>').innerText = "";
            } else {
              document.getElementById('<%=txtCRMoney.ClientID %>').innerText = formatCurrency(Items[j]);
            }
          //------------------傳票分錄明細--------------------
          } else if (j == 7) {
            var jnlRow = Items[j].split('\x05');
            for (var i = 0; i < jnlRow.length - 1 ; i++) {
              //alert(jnlRow[i]);
              var jnlCol = jnlRow[i].split('\x08');
              var x = i + 1;
              var jnltxtDesc = "cphEEOC_txtDesc" + x;
              var jnltxtInputYN = "cphEEOC_txtInputYN" + x;
              var jnlimgICO = "cphEEOC_imgICO" + x;
              var jnltxtValueText = "cphEEOC_txtValueText" + x;
              var jnllblDesc = "cphEEOC_lblDesc" + x;
              var jnltxtReValue = "cphEEOC_txtReValue" + x;              
              document.getElementById(jnltxtReValue).innerText = "";
              
              //txtDesc
              if (jnlCol[1] != "") {
                document.getElementById(jnltxtDesc).style.display = "inline";
                document.getElementById(jnltxtDesc).innerText = jnlCol[1];
              } else {
                document.getElementById(jnltxtDesc).style.display = "none";
              }
              //txtInputYN
              if (jnlCol[2] == "Y") {
                document.getElementById(jnltxtInputYN).style.display = "inline";
                document.getElementById(jnltxtInputYN).innerText = jnlCol[2];
              } else {
                document.getElementById(jnltxtInputYN).innerText = "";
              }

              //imgICO
              if (jnlCol[3] == "Y") {
                  if (document.getElementById('<%=txtJournailOPMode.ClientID %>').value == "NEW" || document.getElementById('<%=txtJournailOPMode.ClientID %>').value == "EDIT") {
                      document.getElementById(jnlimgICO).style.display = "inline";
                      document.getElementById(jnlimgICO).style.cursor = "hand";
                  } else {
                      document.getElementById(jnlimgICO).style.display = "none";
                  }
                  if (i >= 0 && i < 7) {
                      document.getElementById(jnltxtReValue).innerText = jnlCol[0] + "\x08" + jnlCol[1] + "\x08" + jnlCol[2] + "\x08" + jnlCol[3];
                      (function (arg, jnltxtValueText, jnllblDesc, jnltxtReValue) {
                          document.getElementById(jnlimgICO).onclick = function () {
                              GetPopupDialog(530, 400,
                  document.getElementById(companyName).options[document.getElementById(companyName).selectedIndex].value,
                    arg,
                    "CodeCode,CodeName", jnltxtValueText, jnllblDesc, jnltxtReValue,
                    document.getElementById(jnltxtReValue).value);
                          }
                      })(jnlCol[0], jnltxtValueText, jnllblDesc, jnltxtReValue);
                  }

              } else {
                  if (i < 7)
                      document.getElementById(jnltxtReValue).innerText = jnlCol[0] + "\x08" + jnlCol[1] + "\x08" + jnlCol[2] + "\x08" + jnlCol[3];
                  document.getElementById(jnlimgICO).style.display = "none";
              }
                           
              if (jnlCol[1] != "") {
                //txtValueText
                document.getElementById(jnltxtValueText).style.display = "inline";
                document.getElementById(jnltxtValueText).innerText = jnlCol[4];
                if (document.getElementById('<%=txtJournailOPMode.ClientID %>').value == "NEW" || document.getElementById('<%=txtJournailOPMode.ClientID %>').value == "EDIT")
                {
                  document.getElementById(jnltxtValueText).readOnly = false;
                } else {
                  document.getElementById(jnltxtValueText).readOnly = true;
                }
                //lblDesc
                document.getElementById(jnllblDesc).style.display = "inline";
                document.getElementById(jnllblDesc).innerText = jnlCol[5];
              } else {
                document.getElementById(jnltxtValueText).style.display = "none";
                document.getElementById(jnllblDesc).style.display = "none";
              }

              //alert(jnlRow[i]);
            }
          }
        }
      }
      
      //新增一筆分錄
      function AddItem(txtResult)
      {
        document.getElementById('<%=txtOPMode.ClientID %>').innerText = "add=";
        document.getElementById('<%=txtSingleJournailzing.ClientID %>').innerText = "";
        document.getElementById('<%=lblLegend.ClientID %>').innerText = " 傳票分錄明細 [新增] ";
        document.getElementById('<%=imgbtnOKJournailzing.ClientID %>').style.display="inline";
        document.getElementById('<%=imgbtnReset.ClientID %>').style.display="inline";        
        
        return false;
      }
      
      //編輯一筆分錄
      function EditItem(txtResult) 
      {
        txtResult = document.getElementById(txtResult);
        if (txtResult.value != "") {
            var sItem = txtResult.value.split('\x06');
          
          //SelectItem(txtResult.value);
          //alert('編輯分錄' + txtResult.value);
          document.getElementById('<%=txtOPMode.ClientID %>').innerText = "edit=" + sItem[0];
          document.getElementById('<%=lblLegend.ClientID %>').innerText = " 傳票分錄明細 [編輯] ";
          document.getElementById('<%=imgbtnOKJournailzing.ClientID %>').style.display="inline";
          document.getElementById('<%=imgbtnReset.ClientID %>').style.display = "inline";
        } else {
          alert('請先選擇一筆分錄 !!!');
        }
        return false;
      }
      
      //刪除一筆分錄
      function DeleteItem(txtResult,txtAllResult)
      {
        if (confirm("是否確定刪除此筆分錄")) {
          txtResult = document.getElementById(txtResult);
          if (txtResult.value != "") {
              var sItem = txtResult.value.split('\x06');
            
            //SelectItem(txtResult.value);
            //alert('刪除分錄' + txtResult.value);
            document.getElementById('<%=txtOPMode.ClientID %>').innerText = "delete=" + sItem[0];
            document.getElementById('<%=lblLegend.ClientID %>').innerText = " 傳票分錄明細 [刪除] ";
                    
            txtAllResult = document.getElementById(txtAllResult);
            var rows = txtAllResult.value.split('\x07');
            var RemoveItem;
            for (var i = 0; i < rows.length - 1; i++) {
                var Items = rows[i].split('\x06');
                if (Items[0] == sItem[0]) {
                    RemoveItem = i;
                }
            }
            var NewResult = "";
            for (var i = 0; i < rows.length - 1; i++) {
                if (RemoveItem != i) {
                    NewResult += rows[i] + "\x07";
                }
            }
            txtResult.value = '';
            txtAllResult.value = NewResult;
            drowJournailList(txtAllResult);
          } else {
            alert('請先選擇一筆分錄 !!!');
          }
          return false;
        }
      }
      
      //刪除所有分錄
      function DeleteAllItem(txtResult,txtAllResult) {     
        txtResult.value = '';      
        txtAllResult.value = '';
      
        drowJournailList(txtAllResult);
      }  
      
      //畫傳票分錄列表
      function drowJournailList(txtResult) {
        var outShowTable = "";
//        //Step 1：畫出表頭部份
//        outShowTable = "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" style=\"width: 100%;\">";
//        outShowTable += "  <tr style=\"height: 22px; background-color: #0066ff; color: #ffffff;\">"
//        outShowTable += "    <td style=\"width: 30px; text-align: center;\">NO</td>";
//        outShowTable += "    <td style=\"width: 80px; text-align: center;\">借方科目</td>";
//        outShowTable += "    <td style=\"width: 80px; text-align: center;\">貸方科目</td>";
//        outShowTable += "    <td style=\"width: 120px; text-align: center;\">科目名稱</td>";
//        outShowTable += "    <td style=\"width: 300px; text-align: center;\">摘要</td>";
//        outShowTable += "    <td style=\"width: 100px; text-align: center;\">借方金額</td>";
//        outShowTable += "    <td style=\"width: 100px; text-align: center;\">貨方金額</td>";
//        outShowTable += "    <td style=\"display: none;\"></td>";
//        outShowTable += "  </tr>";
//        outShowTable += "</table>";
        
        //Step 2：畫出表身部份
        var rows = txtResult.value.split('\x07');               
        var cDBAmt = 0; //借方金額合計
        var cCRAmt = 0; //貸方金額合計
        if (rows.length > 0) {
          outShowTable += "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" style=\"width: 100%;\">";
          for (var i = 0; i < rows.length - 1; i++) {
            if (((i + 1) % 2) == 1) {
              outShowTable += "<tr style=\"cursor:hand; height: 22px; background-color: #eff3fb;\" onmouseover=\"this.style.backgroundColor='silver'\" onmouseout=\"this.style.backgroundColor='#eff3fb'\" title=\"選擇此筆分錄\">";
            } else {
              outShowTable += "<tr style=\"cursor:hand; height: 22px; background-color: #ffffff;\" onmouseover=\"this.style.backgroundColor='silver'\" onmouseout=\"this.style.backgroundColor='#ffffff'\" title=\"選擇此筆分錄\">";
            }

            var Items = rows[i].split('\x06');
            for (var j = 0; j < Items.length; j++) {
              //No
              if (j == 0) {
                outShowTable += "  <td style=\"width: 30px; text-align: center;\" onclick=\"SelectItem('" + rows[i] + "');\">";
                outShowTable += "    " + Items[j] + "</td>";
              //借方科目
              } else if (j == 1) {
                outShowTable += "  <td style=\"width: 80px; text-align: center;\" onclick=\"SelectItem('" + rows[i] + "');\">";
                if (parseFloat(Items[5]) == 0.00) {
                  outShowTable += "    &nbsp;</td>";
                } else {
                  outShowTable += "    &nbsp;" + Items[j] + "</td>";
                }
              //貸方科目
              } else if (j == 2) {
                outShowTable += "  <td style=\"width: 80px; text-align: center;\" onclick=\"SelectItem('" + rows[i] + "');\">";
                if (parseFloat(Items[6]) == 0.00) {
                  outShowTable += "    &nbsp;</td>";
                } else {
                  outShowTable += "    &nbsp;" + Items[j] + "</td>";
                }
              //科目名稱
              } else if (j == 3) {
                outShowTable += "  <td nowrap=\"noWrap\" style=\"width: 180px; text-align: left;\" onclick=\"SelectItem('" + rows[i] + "');\">";
                outShowTable += "    &nbsp;" + Items[j] + "</td>";
              //摘要
              } else if (j == 4) {
                outShowTable += "  <td nowrap=\"noWrap\" style=\"width: 300px; text-align: left;\" onclick=\"SelectItem('" + rows[i] + "');\">";
                outShowTable += "    &nbsp;" + Items[j] + "</td>";
              //借方金額
              } else if (j == 5) {
                outShowTable += "  <td style=\"width: 100px; text-align:right;\" onclick=\"SelectItem('" + rows[i] + "');\">";
                if (parseFloat(Items[j]) == 0.00) {
                  outShowTable += "    &nbsp;</td>";
                } else {
                  outShowTable += "    " + formatCurrency(Items[j]) + "&nbsp;</td>";
                }
                cDBAmt += parseFloat(Items[j]);
              //貨方金額
              } else if (j == 6) {
                outShowTable += "  <td style=\"width: 100px; text-align:right;\" onclick=\"SelectItem('" + rows[i] + "');\">";
                if (parseFloat(Items[j]) == 0.00) {
                  outShowTable += "    &nbsp;</td>";
                } else {
                  outShowTable += "    " + formatCurrency(Items[j]) + "&nbsp;</td>";
                }
                cCRAmt += parseFloat(Items[j]);
              } else if (j == 7) {
                outShowTable += "<td style=\"display: none;\"><input id=\"txtJournail_\"" + Items[0] + " type=\"text\" /></td>";
              }
            }
            outShowTable += "</tr>"; 
          }
        } else {
          outShowTable += "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" style=\"width: 100%;\">";
          outShowTable += "<tr style=\"height: 22px; background-color: #d1ddf1;\">";
          outShowTable += "  <td style=\"text-align: center; font-weight: bold; background-color: #ffffff;\" colspan=\"7\">此傳票無任何分錄 !!!</td>";
          outShowTable += "</tr>";
        }
        outShowTable += "</table>";
        
//        //Step 3：畫出表尾部份
//        outShowTable += "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" style=\"width: 100%;\">";
//        outShowTable += "<tr style=\"height: 22px; background-color: #d1ddf1;\">";
//        outShowTable += "  <td style=\"width: 30px;\"></td>"
//        outShowTable += "  <td style=\"width: 80px;\"></td>"
//        outShowTable += "  <td style=\"width: 80px;\"></td>"
//        outShowTable += "  <td style=\"width: 120px;\"></td>"
//        outShowTable += "  <td style=\"width: 300px; text-align: right; font-weight: bold;\">合計&nbsp;</td>";
//        outShowTable += "  <td style=\"width: 100px; text-align: right;\">" + formatCurrency(cDBAmt) + "&nbsp;</td>";
//        outShowTable += "  <td style=\"width: 100px; text-align: right;\">" + formatCurrency(cCRAmt) + "&nbsp;</td>";
//        outShowTable += "</tr>";
//        outShowTable += "</table>";
        if(document.getElementById('<%=txtDBAMT.ClientID %>')!=null)
        {
         document.getElementById('<%=txtDBAMT.ClientID %>').innerText = formatCurrency(cDBAmt);
        }
        if(document.getElementById('<%=txtCRAMT.ClientID %>')!=null)
        {
         document.getElementById('<%=txtCRAMT.ClientID %>').innerText = formatCurrency(cCRAmt);
        }
        //alert(outShowTable);
        if(document.getElementById('divGLResult')!=null)
        {
         document.getElementById('divGLResult').innerHTML = outShowTable;
        }
        return false;
      }

      //摘要組合
      function AsmMark() {
          var theMark = document.getElementById("<%=txtValueText10.ClientID %>"); //document.getElementById("txtValueText10").innerText;          
          theMark.value = "";
          for (var i = 0; i < 7; i++) {
              //alert(jnlRow[i]);
              var theLinkChar = "＋";
              var x = i + 1;
              var jnltxtValueText = "<%=txtValueText10.ClientID %>".replace("txtValueText10", "") + "txtValueText" + x;
              var jnllblDesc = "<%=txtValueText10.ClientID %>".replace("txtValueText10", "") + "lblDesc" + x;
              var jnltxtDesc = "<%=txtValueText10.ClientID %>".replace("txtValueText10", "") + "txtDesc" + x;
              //alert(x + ":jnllblDesc=" + document.getElementById(jnllblDesc).value + "; jnltxtDesc=" + document.getElementById(jnltxtDesc).value)
              var theValue = document.getElementById(jnltxtValueText).value.replace(" ", "").replace("\x05", "").replace("\x06", "").replace("\x07", "").replace("\x08", "");
              var theDesc = document.getElementById(jnllblDesc).value.replace(" ", "");
              var thedisplay = document.getElementById(jnltxtDesc).style.display;
              if (thedisplay == "inline") {                  
                  if (theDesc != "") {
                      if (theMark.value != "") theMark.value = theMark.value + theLinkChar;
                      theMark.value = theMark.value + theDesc;
                  } else if (theValue != "") {
                      if (theMark.value != "") theMark.value = theMark.value + theLinkChar;
                      theMark.value = theMark.value + theValue;
                  }
              }
          }
      }
  //-->  	
  </script>
    <script language="javascript" type="text/javascript" src="~/Pages/pagefunction.js"></script>   
  

  
    <div id="main">
      <asp:ScriptManager ID="ScriptManager1" runat="server">
      </asp:ScriptManager>
        <uc1:StyleTitle ID="StyleTitle1" runat="server" ShowHome="false" ShowUser="false"
            Title="傳票登錄" ShowBackToPre="false" />
      <fieldset>
        <legend><%=JournailMode%></legend>
          <div class="basic" style="width:100%;">
            <table class="dialog_body" border="1" cellpadding="0" cellspacing="0" style="width: 100%">
              <tr>
                <td style="text-align: right; font-weight: bold;">
                  公司別&nbsp;</td>
                <td colspan="3">
                  <uc4:CompanyList ID="DrpCompanyList" runat="server" />
                  <asp:TextBox ID="lblCompany" runat="server" BorderStyle="None" BorderWidth="0px" CssClass="TextAlignLeftBold"
                    Width="200px" Wrap="False"></asp:TextBox></td>
                <td style="text-align: right; font-weight: bold;">
                  傳票號碼&nbsp;</td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtJournailNo" runat="server" AutoPostBack="True"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                  <asp:TextBox ID="txtJournailOPMode" runat="server" /></td>
              </tr>
              <tr>
                <td style="text-align: right; font-weight: bold;">
                  傳票日期&nbsp;</td>
                <td>
                  <asp:TextBox ID="txtJournailDate" runat="server" Width="88px"></asp:TextBox>
                    <%--<asp:ImageButton ID="ibOW_JournailDate" runat="server" SkinID="Calendar1" />--%>
                </td>
                <td style="text-align: right; font-weight: bold;">
                  傳票類別&nbsp;</td>
                <td>
                  &nbsp;<asp:DropDownList ID="ddlstJournailType" runat="server">
                    <asp:ListItem Value="0">0. 其他</asp:ListItem>
                    <asp:ListItem Value="1">1. 收入</asp:ListItem>
                    <asp:ListItem Value="2">2. 支出</asp:ListItem>
                    <asp:ListItem Selected="True" Value="3">3. 轉帳</asp:ListItem>
                  </asp:DropDownList>
                  <asp:TextBox ID="txtJournailType" runat="server" BorderStyle="None" BorderWidth="0px"
                    Width="80px"></asp:TextBox>
                </td>
                <td style="text-align: right; font-weight: bold;">
                  &nbsp;</td>
                <td>
                  <asp:TextBox ID="txtReturnDate" runat="server" Width="82px" Visible="False"></asp:TextBox>
                    <%--<asp:ImageButton ID="ibOW_RevDate" runat="server" SkinID="Calendar1" Visible="False" />--%>
                    &nbsp;&nbsp;
                </td>
              </tr>
            </table>
          </div>
          <div class="basic" style="width:100%;">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
              <tr style="background-color:#efefef;">
                <td>
                  <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                      <asp:ImageButton ID="imgbtnCloseJournail" runat="server" ImageUrl="~/Image/ButtonPics/trnback.gif" Visible="False" AlternateText="" /><asp:ImageButton ID="imgbtnPrintAndSaveJournail" runat="server" ImageUrl="~/Image/ButtonPics/saveprint.gif" OnClick="imgbtnPrintAndSaveJournail_Click" Visible="False" />
                      <asp:ImageButton ID="imgbtnSaveJourmail" runat="server" ImageUrl="~/Image/ButtonPics/savepaper.gif" OnClick="imgbtnSaveJourmail_Click" AlternateText="分錄製作完畢"  />
                      <asp:ImageButton ID="imgbtnNewJourail" runat="server" ImageUrl="~/Image/ButtonPics/newglpaper.gif" OnClick="imgbtnNewJourail_Click" AlternateText="新增傳票" />
                      <asp:ImageButton ID="imgbtnCopyToNewJourail" runat="server" ImageUrl="~/Image/ButtonPics/copynewglpaper.gif" OnClick="imgbtnCopyToNewJourail_Click" AlternateText="複製新增傳票" />
                    </ContentTemplate>
                  </asp:UpdatePanel>
                </td>
                <td style="text-align: right;">
                  <img id="imgDeleteJournailzing" runat="server" src="~/Image/ButtonPics/deleteItem.gif" alt="" />
                  <img id="imgEditJournailzing" runat="server" src="~/Image/ButtonPics/editItem.gif" alt="" />
                  <img id="imgAddJournailzing" runat="server" src="~/Image/ButtonPics/addItem.gif" alt="" />&nbsp;
                </td>
              </tr>
            </table>
          </div>
          <fieldset id="content" runat="server">
            <legend>&nbsp;傳票分錄列表&nbsp;</legend>
            <div class="basic" style="width:100%;">
              <%--//Step 1：畫出表頭部份--%>
              <table border="1" cellpadding="0" cellspacing="0" style="width: 100%;" class="dialog_body">
                <tr style="height: 22px; background-color: #dcdcdc; color: #003489;">
                  <td style="width: 30px; text-align: center;" nowrap="noWrap">NO</td>
                  <td style="width: 80px; text-align: center;" nowrap="noWrap">借方科目</td>
                  <td style="width: 80px; text-align: center;" nowrap="noWrap">貸方科目</td>
                  <td style="width: 180px; text-align: center;" nowrap="noWrap">科目名稱</td>
                  <td style="width: 300px; text-align: center;" nowrap="noWrap">摘要</td>
                  <td style="width: 100px; text-align: center;" nowrap="noWrap">借方金額</td>
                  <td style="width: 100px; text-align: center;" nowrap="noWrap">貨方金額</td>
                  <td style="display: none;" nowrap="noWrap"></td>
                </tr>
              </table>
              <asp:Panel ID="Panelx" runat="server" Width="100%" Wrap="False">
                <div id="divGLResult"></div>
              </asp:Panel>
              <%--//Step 3：畫出表尾部份--%>
              <table border="1" cellpadding="0" cellspacing="0" style="width: 100%;" class="dialog_body">
                <tr style="height: 22px;">
                  <td style="width: 673px; text-align: right; font-weight: bold;">合計</td>
                  <td style="width: 100px; text-align: right;">
                    <asp:TextBox ID="txtDBAMT" runat="server" Width="94px" MaxLength="17" CssClass="TextAlignRight" BorderStyle="None" BorderWidth="0px"></asp:TextBox></td>
                  <td style="width: 100px; text-align: right;">
                    <asp:TextBox ID="txtCRAMT" runat="server" Width="94px" MaxLength="17" CssClass="TextAlignRight" BorderStyle="None" BorderWidth="0px"></asp:TextBox></td>
                </tr>
              </table>
            </div>
          </fieldset>
          
          <fieldset id="Fieldset1" runat="server">
            <%--<legend>&nbsp;傳票分錄明細&nbsp;</legend>--%>
            <legend id="Legend1"><asp:Label ID="lblLegend" runat="server" Text="&nbsp;傳票分錄明細&nbsp;" /></legend>
      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
          <ContentTemplate>
            <div class="basic" style="width:100%;">
              <table class="dialog_body" border="1" cellpadding="0" cellspacing="0" style="width: 100%">
                <tr>
                  <td style="text-align: right; font-weight: bold; height: 24px;">
                    <asp:Label ID="lblSNo" runat="server" Text="序號" style=" display:none;"/>&nbsp;</td>
                  <td style="height: 24px">
                    &nbsp;<asp:TextBox ID="txtSNo" runat="server" Width="22px" style=" display:none;"></asp:TextBox>
                    &nbsp;&nbsp;</td>
                  <td style="text-align: right; font-weight: bold;">
                    科目&nbsp;</td>
                  <td colspan="3">
                    &nbsp;<img id="imgAcctNo" runat="server" src="~/Image/ButtonPics/Query.gif" alt="" /><asp:TextBox ID="txtAcctNo" runat="server" Width="80px" AutoPostBack="True" OnTextChanged="txtAcctNo_TextChanged"></asp:TextBox>&nbsp;<asp:TextBox ID="lblAcctNo" runat="server" BorderStyle="None" BorderWidth="0px" CssClass="TextAlignLeftBold" Width="300px" Wrap="False"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td style="text-align: right; font-weight: bold;">
                    借方金額&nbsp;</td>
                  <td colspan="3">
                    &nbsp;<asp:TextBox ID="txtDBMoney" runat="server" Width="120px" MaxLength="17" CssClass="TextAlignRight"></asp:TextBox>&nbsp;
                  </td>
                  <td style="text-align: right; font-weight: bold;">
                    憑證未到&nbsp;</td>
                  <td>
                    &nbsp;<asp:TextBox ID="txtP1" runat="server" Width="15px" Enabled="False">Y</asp:TextBox>
                    <strong>(Y-未到)</strong></td>
                </tr>
                <tr>
                  <td style="text-align: right; font-weight: bold;">
                    貸方金額&nbsp;</td>
                  <td>
                    &nbsp;<asp:TextBox ID="txtCRMoney" runat="server" Width="120px" MaxLength="17" CssClass="TextAlignRight"></asp:TextBox>&nbsp;
                  </td>
                  <td style="text-align: right; font-weight: bold;">
                    比率&nbsp;</td>
                  <td>
                    &nbsp;<asp:TextBox ID="txtP2" runat="server" Width="22px"></asp:TextBox><strong>％</strong></td>
                  <td style="text-align: right; font-weight: bold;">
                    分攤碼&nbsp;</td>
                  <td>
                    &nbsp;<asp:TextBox ID="txtP3" runat="server" Width="15px" Enabled="False">N</asp:TextBox>
                    <strong>(Y/N)</strong></td>
                </tr>
              </table>
            </div>
            &nbsp;
            <div class="basic" style="width:100%;">
              <table  class="dialog_body" border="1" cellpadding="0" cellspacing="0" style="width: 100%; background-color:White;">
                <tr>
                  <td style="width: 20px; text-align: center; font-weight: bold;">
                    1</td>
                  <td style="width: 120px; text-align: right">
                    <asp:TextBox ID="txtDesc1" runat="server" BorderStyle="None" BorderWidth="0px" CssClass="TextAlignRightBold" Width="100px"></asp:TextBox>&nbsp;</td>
                  <td style="width: 25px; text-align: center">
                    &nbsp;<asp:TextBox ID="txtInputYN1" runat="server" Width="15px" BorderWidth="0px" CssClass="TextAlignCenterBold"></asp:TextBox></td>
                  <td style="width: 24px; height:24px;">
                    &nbsp;<img id="imgICO1" runat="server" src="~/Image/ButtonPics/Query.gif" alt="" visible="true" />
                  </td>
                  <td>
                    
                    &nbsp;<asp:TextBox ID="txtValueText1" runat="server" Width="100px" onblur="javascript:AsmMark();"></asp:TextBox>
                    &nbsp;<asp:TextBox ID="lblDesc1" runat="server" CssClass="TextAlignLeftBold" BorderStyle="None" BorderWidth="0px" Width="200px"></asp:TextBox><asp:TextBox ID="txtReValue1" runat="server" Width="100px"></asp:TextBox>
                      
                  </td>
                </tr>                
                <tr>
                  <td style="width: 20px; text-align: center; font-weight: bold;">
                    2</td>
                  <td style="width: 120px; text-align: right">
                    <asp:TextBox ID="txtDesc2" runat="server" BorderStyle="None" BorderWidth="0px" CssClass="TextAlignRightBold" Width="100px"></asp:TextBox>&nbsp;</td>
                  <td style="width: 25px; text-align: center">
                    &nbsp;<asp:TextBox ID="txtInputYN2" runat="server" BorderWidth="0px" Width="15px" CssClass="TextAlignCenterBold"></asp:TextBox></td>
                  <td style="width: 24px; height:24px;">
                    &nbsp;<img id="imgICO2" runat="server" src="~/Image/ButtonPics/Query.gif" alt="" visible="true" /></td>
                  <td>
                    &nbsp;<asp:TextBox ID="txtValueText2" runat="server" Width="100px" onblur="javascript:AsmMark();"></asp:TextBox>
                    &nbsp;<asp:TextBox ID="lblDesc2" runat="server" BorderStyle="None" BorderWidth="0px" CssClass="TextAlignLeftBold" Width="200px"></asp:TextBox><asp:TextBox ID="txtReValue2" runat="server" Width="100px"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td style="width: 20px; text-align: center; font-weight: bold;">
                    3</td>
                  <td style="width: 120px; text-align: right">
                    <asp:TextBox ID="txtDesc3" runat="server" BorderStyle="None" BorderWidth="0px" CssClass="TextAlignRightBold" Width="100px"></asp:TextBox>&nbsp;</td>
                  <td style="width: 25px; text-align: center">
                    &nbsp;<asp:TextBox ID="txtInputYN3" runat="server" BorderWidth="0px" Width="15px" CssClass="TextAlignCenterBold"></asp:TextBox></td>
                  <td style="width: 24px; height:24px;">
                    &nbsp;<img id="imgICO3" runat="server" src="~/Image/ButtonPics/Query.gif" alt="" visible="true" /></td>
                  <td>
                    &nbsp;<asp:TextBox ID="txtValueText3" runat="server" Width="100px" onblur="javascript:AsmMark();"></asp:TextBox>
                    &nbsp;<asp:TextBox ID="lblDesc3" runat="server" BorderStyle="None" BorderWidth="0px" CssClass="TextAlignLeftBold" Width="200px"></asp:TextBox><asp:TextBox ID="txtReValue3" runat="server" Width="100px"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td style="width: 20px; text-align: center; font-weight: bold;">
                    4</td>
                  <td style="width: 120px; text-align: right">
                    <asp:TextBox ID="txtDesc4" runat="server" BorderStyle="None" BorderWidth="0px" CssClass="TextAlignRightBold" Width="100px"></asp:TextBox>&nbsp;</td>
                  <td style="width: 25px; text-align: center">
                    &nbsp;<asp:TextBox ID="txtInputYN4" runat="server" BorderWidth="0px" Width="15px" CssClass="TextAlignCenterBold"></asp:TextBox></td>
                  <td style="width: 24px; height:24px;">
                    &nbsp;<img id="imgICO4" runat="server" src="~/Image/ButtonPics/Query.gif" alt="" visible="true" /></td>
                  <td>
                    &nbsp;<asp:TextBox ID="txtValueText4" runat="server" Width="100px" onblur="javascript:AsmMark();"></asp:TextBox>
                    &nbsp;<asp:TextBox ID="lblDesc4" runat="server" BorderStyle="None" BorderWidth="0px" CssClass="TextAlignLeftBold" Width="200px"></asp:TextBox><asp:TextBox ID="txtReValue4" runat="server" Width="100px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                  <td style="width: 20px; text-align: center; font-weight: bold;">
                    5</td>
                  <td style="width: 120px; text-align: right">
                    <asp:TextBox ID="txtDesc5" runat="server" BorderStyle="None" BorderWidth="0px" CssClass="TextAlignRightBold" Width="100px"></asp:TextBox>&nbsp;</td>
                  <td style="width: 25px; text-align: center; height: 24px;">
                    &nbsp;<asp:TextBox ID="txtInputYN5" runat="server" BorderWidth="0px" Width="15px" CssClass="TextAlignCenterBold"></asp:TextBox></td>
                  <td style="width: 24px; height: 24px;">
                    &nbsp;<img id="imgICO5" runat="server" src="~/Image/ButtonPics/Query.gif" alt="" visible="true" /></td>
                  <td>
                    &nbsp;<asp:TextBox ID="txtValueText5" runat="server" Width="100px" onblur="javascript:AsmMark();"></asp:TextBox>
                    &nbsp;<asp:TextBox ID="lblDesc5" runat="server" BorderStyle="None" BorderWidth="0px" CssClass="TextAlignLeftBold" Width="200px"></asp:TextBox><asp:TextBox ID="txtReValue5" runat="server" Width="100px"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td style="width: 20px; text-align: center; font-weight: bold;">
                    6</td>
                  <td style="width: 120px; text-align: right">
                    <asp:TextBox ID="txtDesc6" runat="server" BorderStyle="None" BorderWidth="0px" CssClass="TextAlignRightBold" Width="100px"></asp:TextBox>&nbsp;</td>
                  <td style="width: 25px; text-align: center; height: 24px;">
                    &nbsp;<asp:TextBox ID="txtInputYN6" runat="server" BorderWidth="0px" Width="15px" CssClass="TextAlignCenterBold"></asp:TextBox></td>
                  <td style="width: 24px; height: 24px;">
                    &nbsp;<img id="imgICO6" runat="server" src="~/Image/ButtonPics/Query.gif" alt="" visible="true" /></td>
                  <td>
                    &nbsp;<asp:TextBox ID="txtValueText6" runat="server" Width="100px" onblur="javascript:AsmMark();"></asp:TextBox>
                    &nbsp;<asp:TextBox ID="lblDesc6" runat="server" BorderStyle="None" BorderWidth="0px" CssClass="TextAlignLeftBold" Width="200px"></asp:TextBox><asp:TextBox ID="txtReValue6" runat="server" Width="100px"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td style="width: 20px; text-align: center; font-weight: bold;">
                    7</td>
                  <td style="width: 120px; text-align: right">
                    <asp:TextBox ID="txtDesc7" runat="server" BorderStyle="None" BorderWidth="0px" CssClass="TextAlignRightBold" Width="100px"></asp:TextBox>&nbsp;</td>
                  <td style="width: 25px; text-align: center; height: 24px;">
                    &nbsp;<asp:TextBox ID="txtInputYN7" runat="server" BorderWidth="0px" Width="15px" CssClass="TextAlignCenterBold"></asp:TextBox></td>
                  <td style="width: 24px; height:24px;">
                    &nbsp;<img id="imgICO7" runat="server" src="~/Image/ButtonPics/Query.gif" alt="" visible="true" /></td>
                  <td style="height: 26px">
                    &nbsp;<asp:TextBox ID="txtValueText7" runat="server" Width="100px" onblur="javascript:AsmMark();"></asp:TextBox>
                    &nbsp;<asp:TextBox ID="lblDesc7" runat="server" BorderStyle="None" BorderWidth="0px" CssClass="TextAlignLeftBold" Width="200px"></asp:TextBox><asp:TextBox ID="txtReValue7" runat="server" Width="100px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                  <td style="height: 16px; text-align: right; font-weight: bold;" colspan="2">
                    資金代碼&nbsp;</td>
                  <td style="width: 25px; height: 16px; text-align: center">
                    &nbsp;<asp:TextBox ID="txtInputYN8" runat="server" BorderWidth="0px" Width="15px" CssClass="TextAlignCenterBold"></asp:TextBox></td>
                  <td style="width: 24px; height: 24px">
                    &nbsp;<img id="imgICO8" runat="server" src="~/Image/ButtonPics/Query.gif" alt="" visible="true" /></td>
                  <td>
                    &nbsp;<asp:TextBox ID="txtValueText8" runat="server" Width="100px"></asp:TextBox>&nbsp;<asp:TextBox ID="lblDesc8" runat="server" BorderStyle="None" BorderWidth="0px" CssClass="TextAlignLeftBold" Width="200px"></asp:TextBox>&nbsp;<asp:TextBox ID="txtReValue8" runat="server" Width="100px"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td style="height: 16px; text-align: right; font-weight: bold;" colspan="2">
                    沖帳傳票號碼&nbsp;</td>
                  <td style="width: 25px; height: 16px; text-align: center">
                    &nbsp;<asp:TextBox ID="txtInputYN9" runat="server" BorderWidth="0px" Width="15px" CssClass="TextAlignCenterBold"></asp:TextBox></td>
                  <td style="width: 24px; height: 24px">
                    &nbsp;<img id="imgICO9" runat="server" src="~/Image/ButtonPics/Query.gif" alt="" visible="true" /></td>
                  <td>
                    &nbsp;<asp:TextBox ID="txtValueText9" runat="server" Width="100px"></asp:TextBox>&nbsp;<asp:TextBox ID="lblDesc9" runat="server" BorderStyle="None" BorderWidth="0px" CssClass="TextAlignLeftBold" Width="200px"></asp:TextBox>&nbsp;<asp:TextBox ID="txtReValue9" runat="server" Width="100px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                  <td style="text-align: right; font-weight: bold; height:24px;" colspan="2">
                    摘要&nbsp;</td>
                  <td style="width: 25px; text-align: center">
                    &nbsp;</td>
                  <td style="width: 24px">
                    &nbsp;</td>
                  <td>
                    &nbsp;<asp:TextBox ID="txtValueText10" runat="server" Width="400px"></asp:TextBox></td>
                </tr>
              </table>
              <asp:TextBox ID="txtReValue" runat="server" Width="400px" />
                <asp:TextBox ID="txtSingleJournailzing" runat="server" AutoPostBack="True" />
                <asp:TextBox ID="txtOPMode" runat="server" ViewStateMode="Enabled" />
                <asp:TextBox ID="txtAllJournailzing" runat="server"/>
            </div>
          </ContentTemplate>
        </asp:UpdatePanel>
            <div class="basic" style="width:100%;">
              <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                <tr>
                  <td style="text-align:center;">
                    <asp:ImageButton ID="imgbtnOKJournailzing" runat="server" ImageUrl="~/Image/ButtonPics/subconfirm.gif" OnClick="imgbtnOKJournailzing_Click" AlternateText="確定分錄" />
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                    <asp:ImageButton ID="imgbtnReset" runat="server" ImageUrl="~/Image/ButtonPics/clear_reinput.gif" OnClick="imgbtnReset_Click" AlternateText="清除重填" /></td>
                </tr>
              </table>
            </div>
          </fieldset>
      </fieldset>
        &nbsp;
      <br />
      <br />
    </div>
</asp:Content>

