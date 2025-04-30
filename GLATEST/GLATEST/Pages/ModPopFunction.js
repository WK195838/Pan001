 function GetModelPopupDialog(dH, dW, dTable, dFields, dKey, tCode, lCode) 
    { 				
        var rURL;
        var wStyle;
    		  
        rURL = "ModelPopupDialog.aspx";
        rURL+= "?Table=" + dTable;
        rURL+= "&Fields=" + dFields;
        rURL+= "&Key=" + dKey;
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
        wStyle+="scroll: No;";
    	
        var ReturnValue = window.showModalDialog(rURL, "", wStyle);
        //alert(ReturnValue);
        var t_Code = document.getElementById(tCode);
        var l_Code = document.getElementById(lCode);
    					
        if (ReturnValue != null)
        {	
	          var index = ReturnValue.indexOf(','); 
	          var ReturnList = ReturnValue.split(',');
    		  
	          if (index > -1) 
                {
                    t_Code.innerText = ReturnList[0];  //代號
                    l_Code.innerText = ReturnList[1];  //名稱
                } else {

                }
                return true;   
        }
        return false;
    }	
    function GetAcctnoPopupDialog(dH, dW, dCompany, dTable, dFields, tAcctNo, lAcctNo) 
    { 				
        var rURL;
        var wStyle;
    		  
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
        				
        if (ReturnValue != null)
        {	
            var index = ReturnValue.indexOf(','); 
            var ReturnList = ReturnValue.split(',');
        	  
            if (index > -1) 
            {
                t_AcctNo.innerText = ReturnList[0];  //科目代號
                l_AcctNo.innerText = ReturnList[1];  //科目名稱
            }
            return true;   
        }
        return false;
    }	