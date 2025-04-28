const adCmdText = 1 
const filename="Holiday_check_Log.txt"	'Log file
const sharemoneyfilename = "下午台股共用額度轉檔.bat"
const IP1="10.103.3.121"
const IP2="10.103.3.122"
const Port1="9009"
const Port2="9009"
const DNS="OLTSTEST"
const ID="oltstest"
const PWD="oltstest"
const defauleCCS_FolderLocation="\ccs"

on error resume next
 
Dim OBJdbConnection
Dim str
Dim ctyID
Dim Today
Dim RS

Set OBJdbConnection = CreateObject("ADODB.Connection")
Set RS = CreateObject ("ADODB.Recordset")
Set SQLStmt = CreateObject("ADODB.Command")
OBJdbConnection.ConnectionString = "provider=msdaora.1;data source=" + DNS + ";user id=" + ID + ";password=" + PWD
OBJdbConnection.Open
  
set RS=createobject("adodb.recordset")
sqlstr = "select count(*),sysdate,'US' as countryID from Holiday_t " & _
	 "where to_char(holiday,'YYYYMMDD') = to_char(SYSDATE,'YYYYMMDD') and countryID='US' " & _
	 "union ALL " & _
	 "select count(*),sysdate,'JP' as countryID from Holiday_t " & _
	 "where to_char(holiday,'YYYYMMDD') = to_char(SYSDATE,'YYYYMMDD') and countryID='JP' " & _
	 "union ALL " & _
	 "select count(*),sysdate,'KR' as countryID from Holiday_t " & _
	 "where to_char(holiday,'YYYYMMDD') = to_char(SYSDATE,'YYYYMMDD') and countryID='KR' " & _
	 "union ALL " & _
	 "select count(*),sysdate,'TH' as countryID from Holiday_t " & _
	 "where to_char(holiday,'YYYYMMDD') = to_char(SYSDATE,'YYYYMMDD') and countryID='TH' " & _
	 "union ALL " & _
	 "select count(*),sysdate,'SG' as countryID from Holiday_t " & _
	 "where to_char(holiday,'YYYYMMDD') = to_char(SYSDATE,'YYYYMMDD') and countryID='SG' " & _
	 "union ALL " & _
	 "select count(*),sysdate,'HK' as countryID from Holiday_t " & _
	 "where to_char(holiday,'YYYYMMDD') = to_char(SYSDATE,'YYYYMMDD') and countryID='HK'" & _
	 "union ALL " & _
	 "select count(*),sysdate,'SA' as countryID from Holiday_t " & _
	 "where to_char(holiday,'YYYYMMDD') = to_char(SYSDATE,'YYYYMMDD') and countryID='SA'" & _
	 "union ALL " & _
	 "select count(*),sysdate,'SZ' as countryID from Holiday_t " & _
	 "where to_char(holiday,'YYYYMMDD') = to_char(SYSDATE,'YYYYMMDD') and countryID='SZ'" & _
	 "union ALL " & _	 	 
	 "select count(*),sysdate,'TW' as contryID from Holiday_t " & _
	 "where to_char(holiday,'YYYYMMDD') = to_char(SYSDATE,'YYYYMMDD') and countryID='TW' "

SQLStmt.CommandText = sqlstr
SQLStmt.CommandType = adCmdText
SQLStmt.ActiveConnection = OBJdbConnection

Set RS = SQLStmt.Execute
Set fs = CreateObject("Scripting.FileSystemObject")
Set writefile=fs.OpenTextFile(filename,8,True)

RS.movefirst

Do While NOT RS.EOF
	str=RS.fields(0).value
	Today=RS.fields(1).value
	ctyID=RS.fields(2).value
	'wscript.echo str & " - " & Today & " - " & ctyID			'for Debug
	'************************* Generate Open_Market.bat
	
        If (str = "0" AND ctyID <> "TW") then
	  'wscript.echo Today & " is not a Holiday for [" & ctyID & "]"		'for Debug
	  writefile.write Today & " is not a Holiday for [" & ctyID & "]" & vbCrLf
	  openmarketfilename = ctyID & "_open_market.bat"
	  set openmarketfileSet = CreateObject("Scripting.FileSystemObject")
	  set A = fs.GetFile(openmarketfilename)
	  A.Delete
	  set openmarketfile = openmarketfileSet.OpenTextFile(openmarketfilename,8,True)
	  openmarketfile.write ":This file is Generated on - " & Today & " and Today is [NOT] a Holiday for [" & ctyID & "]" & vbCrLf
	  openmarketfile.write "c:" & vbCrLf
	  openmarketfile.write "cd c:\ccs\openmarket" & vbCrLf
'	  openmarketfile.write "openmarket " & ctyID & " 0" & vbCrLf
'	  openmarketfile.write "sqlplus " & ID & "/" & PWD & "@" & DNS & " @" & defauleCCS_FolderLocation & "\db-jobs\sql\backpreorders_" & ctyID &".sql" & vbCrLf
'	  openmarketfile.write "sleep 20" & vbCrLf
'	  openmarketfile.write "openmarket " & ctyID & " 1" & vbCrLf
'	  openmarketfile.write "sleep 20" & vbCrLf
	  openmarketfile.write "openmarket " & ctyID & " 3" & vbCrLf
	  openmarketfile.write "sqlplus " & ID & "/" & PWD & "@" & DNS & " @" & defauleCCS_FolderLocation & "\db-jobs\sql\ccs_openMARKETjob_" & ctyID &".sql" & vbCrLf
	  openmarketfile.write "sleep 600"
	  openmarketfile.close
	  set A = nothing
	  set openmarketfile = nothing
	  set openmarketfileSet = nothing
	ElseIf(str = "0") Then
	  writefile.write Today & " is not a Holiday for [" & ctyID & "]" & vbCrLf
	  Set sharemoneyfileSet = CreateObject("Scripting.FileSystemObject")
	  Set A = fs.GetFile(sharemoneyfilename)
	  A.Delete
	  Set sharemoneyfile = sharemoneyfileSet.OpenTextFile(sharemoneyfilename,8,True)
	  sharemoneyfile.write ":This file is Generated on - " & Today & " and Today is [NOT] a Holiday for [" & ctyID & "]" & vbCrLf
	  sharemoneyfile.write "c:"  & vbCrLf
	  sharemoneyfile.write "CD \CCS\DB-JOBS"  & vbCrLf
	  sharemoneyfile.write "noon_tblcredit.bat " & ID & "/" & PWD & "@" & DNS & vbCrLf
	  sharemoneyfile.close
	  Set A = nothing
	  Set sharemoneyfile = nothing
  	  Set sharemoneyfileSet = nothing
	ElseIf(ctyID = "TW") Then
	  'wscript.echo Today & " is a Holiday for [" & ctyID & "]"		'for Debug
	  writefile.write Today & " is a Holiday for [" & ctyID & "]" & vbCrLf
  	  Set sharemoneyfileSet = CreateObject("Scripting.FileSystemObject")
	  Set A = fs.GetFile(sharemoneyfilename)
	  A.Delete
	  Set sharemoneyfile = sharemoneyfileSet.OpenTextFile(sharemoneyfilename,8,True)
	  sharemoneyfile.write ":This file is Generated on - " & Today & " and Today is a Holiday for [" & ctyID & "]" & vbCrLf
	  sharemoneyfile.close
	  Set A = nothing
	  Set sharemoneyfile = nothing
	  Set sharemoneyfileSet = nothing
	Else
	  'wscript.echo Today & " is a Holiday for [" & ctyID & "]"		'for Debug
	  writefile.write Today & " is a Holiday for [" & ctyID & "]" & vbCrLf
	  openmarketfilename = ctyID & "_open_market.bat"
	  set openmarketfileSet = CreateObject("Scripting.FileSystemObject")
	  set A = fs.GetFile(openmarketfilename)
	  A.Delete
	  set openmarketfile = openmarketfileSet.OpenTextFile(openmarketfilename,8,True)
	  openmarketfile.write ":This file is Generated on - " & Today & " and Today is a Holiday for [" & ctyID & "]" & vbCrLf
	  openmarketfile.close
	  set A = nothing
	  set openmarketfile = nothing
	  set openmarketfileSet = nothing
	End if
	'************************* End Generate Open_Market.bat
	RS.movenext

Loop

writefile.close

set writefile=nothing
set fs=nothing
rs.close
set rs=nothing
OBJdbConnection.close
set OBJdbConnection=nothing
