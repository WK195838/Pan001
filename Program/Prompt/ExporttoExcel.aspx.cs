using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Prompt_ExporttoExcel : System.Web.UI.Page
{
    string Ssql = "";    
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    public SysSetting SysSet = new SysSetting();

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _MyDBM = new DBManger();
        _MyDBM.New();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["ExportDT"] != null)
        {
            if (Session["ExportDTTitile"] != null)
                WriteExcTable(Session["ExportDTTitile"].ToString());
            else
                WriteExcTable("");
        }
        else if (Request["DataBody"] == null)
        {
            Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=Parameterless");
        }
        else
        {
            //除了資料命令外,其它都可以為null或空
            //WriteExcTable("資料報表標題", "條件名稱1,條件內容1,條件名稱2,條件內容2...", "資料欄位名稱,若未傳入則使用DB中的欄位描述,(必須是單一資料表才找得出)", "SELECT * FROM HOLIDAY");
            //WriteExcTable(null, null, null, "SELECT * FROM HOLIDAY");
            WriteExcTable(Request["HeaderLine"], Request["QueryLine"], Request["DataTitel"], Request["DataBody"]);
        }
    }

    private void WriteExcTable(string strTemp)
    {
        GridView gvExport = new GridView();
        string theDS = Session["ExportDT"].GetType().Name;
        if (theDS.Contains("GridView"))
            gvExport = (GridView)Session["ExportDT"];
        else if (theDS.Contains("DataTable"))
            gvExport.DataSource = (DataTable)Session["ExportDT"];
        else
        {
            Response.Write("<script>alert('資料格式有誤,請洽系統管理員');</script>");
            Response.End();
            return;
        }
        gvExport.DataBind();

        string strExportFilename = "ExportedData";

        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=" + strExportFilename + ".xls");
        Response.ContentType = "application/vnd.ms-excel";
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        
        if (theDS.Contains("GridView"))
        {
            Response.Charset = "BIG5";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("BIG5");
            
            #region 開始填入 Excel 表格中
            // 印出 Titile
            //Response.Write(htmlWrite.NewLine); 
            if (strTemp.Length != 0)
                Response.Write(strTemp.Replace(",", "\t"));
            else
                for (int j = 0; j < gvExport.Columns.Count; j++)
                    Response.Write(gvExport.Columns[j].HeaderText.Replace("<font color='#FFFFFF'>", "").Replace("</font>", "").Replace("</br>", "").Trim() + "\t");

            Response.Write(htmlWrite.NewLine);
            //印出內容
            for (int i = 0; i < gvExport.Rows.Count; i++)
            {
                for (int j = 0; j < gvExport.Columns.Count; j++)
                    Response.Write(gvExport.Rows[i].Cells[j].Text.Replace("&nbsp;", "").Trim() + "\t");   // 加入"\t"是為了換格
                Response.Write(htmlWrite.NewLine);  // 換行            
            }
            //Response.End();
            #endregion
        }
        else
        {            
            ////if (strTemp.Length != 0)
            ////    Response.Write(strTemp);            
            //gvExport.RenderControl(htmlWrite);
            //Response.Write(stringWrite.ToString().Replace("<div>", "").Replace("</div>", ""));
            //Response.End();
            string strHeaderLine = "匯出資料";

            Response.Charset = "utf-8";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            
            Response.Write("<meta http-equiv=Content-Type content=text/html;charset=utf-8>");

            MyHtmlWrite("<!DOCTYPE HTML PUBLIC ^-//W3C//DTD HTML 4.0 Transitional//EN^>");
            MyHtmlWrite("<HTML xmlns:o=^urn:schemas-microsoft-com:office:office^ xmlns:x=^urn:schemas-microsoft-com:office:excel^>");
            MyHtmlWrite("<head runat='server' >");
            MyHtmlWrite("<TITLE>" + strHeaderLine + "</TITLE>");
            MyHtmlWrite("<meta name=^GENERATOR^ content=^Microsoft Visual Studio .NET 7.1^>");
            MyHtmlWrite("<meta name=^CODE_LANGUAGE^ content=^Visual Basic .NET 7.1^>");
            MyHtmlWrite("<meta name=^vs_defaultClientScript^ content=^JavaScript^>");
            MyHtmlWrite("<meta name=^vs_targetSchema^ content=^http://schemas.microsoft.com/intellisense/ie5^>");
            MyHtmlWrite("</head>");

            MyHtmlWrite("<body>");
            MyHtmlWrite("<TABLE borderColor=^#808080^ border=^1^>");
            MyHtmlWrite("<Thead>");
            
            int iCol, iRow;
            DataRow dr;
            Object[] ary;

            MyHtmlWrite("<TR>");
            MyHtmlWrite("<td colspan=^" + gvExport.Rows[0].Cells.Count.ToString() + "^ height=^22^ style=^height: 16.5pt; text-align: right; color: windowtext; font-size: 12.0pt; font-weight: 400; font-style: normal; text-decoration: none; , serif; vertical-align: middle; white-space: nowrap; border-left: medium none; border-right: medium none;" +
                "border-top: medium none; border-bottom: .5pt solid gray; padding-left: 1px; padding-right: 1px; padding-top: 1px^>列印日期 : " + DateTime.Now.ToString("yyyy/MM/dd") + "</TH>");
            MyHtmlWrite("</TR>");
                       

            MyHtmlWrite("<TR>");
            //輸出標題列
            if (!string.IsNullOrEmpty(strTemp))
            {
                string[] strDTlist = strTemp.Split(new char[] { ',' });
                for (iCol = 0; iCol < strDTlist.Length; iCol++)
                {
                    MyHtmlWrite("<TH BGCOLOR=^blue^>");
                    GetHeader(strDTlist[iCol]);
                }
            }

            MyHtmlWrite("</TR>");
            MyHtmlWrite("</Thead>");
            MyHtmlWrite("<TBODY>");
            //將數據導出到相應的儲存格
            for (iRow = 0; iRow < gvExport.Rows.Count; iRow++)
            {
                MyHtmlWrite("<TR>");
                for (int j = 0; j < gvExport.Rows[0].Cells.Count; j++)
                    MyHtmlWrite("<TD align=^left^ x:str><B><FONT face=^Arial^ color=^#800000^ size=^2^>" + gvExport.Rows[iRow].Cells[j].Text.Replace("&nbsp;", "").Trim() + "</FONT></B></TD>");                
                MyHtmlWrite("</TR>");
            }

            MyHtmlWrite("<TBODY>");
            MyHtmlWrite("<TR></TR><TR>");
            MyHtmlWrite("<TH colspan=^" + gvExport.Rows[0].Cells.Count.ToString() + "^ style=^height: 20.0pt; width: 1024pt; color: BLUE; font-size: 8.0pt; font-weight: 700; text-align: right; font-style: normal; text-decoration: none; vertical-align: middle; white-space: nowrap; border: medium none; padding-left: 1px; padding-right: 1px; padding-top: 1px^>" +
                        "<B><FONT face=^新細明體^ type=^string^>泛太資訊科技開發股份有限公司 Pan Pacific Information Technology and Development Inc. 授權提供</FONT></B></TH>");
            MyHtmlWrite("</TR>");
            MyHtmlWrite("</TBODY>");
            MyHtmlWrite("</TABLE>");
            MyHtmlWrite("</body>");
            MyHtmlWrite("</HTML>");
        }
    }

    private void WriteExcTable(string strHeaderLine, string strQueryLine, string strDataTitel, string strDataBody)
    {
        int iCol, iRow;
        DataRow dr;
        Object[] ary;        
        strDataBody = SysSet.rtnTrans(strDataBody.Replace('＼', '\\'));
        //取得資料
        string strTable = "";
        if (strDataBody.ToUpper().IndexOf("FROM") > 0)
        {
            GetData(strDataBody);
            strTable = strDataBody.Substring(strDataBody.ToUpper().IndexOf("FROM") + 4);
            if (strTable.ToUpper().IndexOf("WHERE") > 0)
                strTable = strTable.Remove(strTable.ToUpper().IndexOf("WHERE"));
        }
        else if (Session[strDataBody] != null)
        {
            Session["MyDataTable"] = Session[strDataBody];
        }

        if (Session["MyDataTable"] == null)
        {
            Response.Write("<javascript>alert('資料傳輸已逾時，請先重新查詢再匯出!');</javascript>");
        }
        else
        {
            DataTable PrintDT = (DataTable)Session["MyDataTable"];

            string[] DTShow = null;
            string[] DTListPart = null;
            int iColCount = PrintDT.Columns.Count;
            if (!string.IsNullOrEmpty(strDataTitel))
            {
                DTListPart = strDataTitel.Split(new char[] { ';' });
                if (DTListPart.Length > 1)
                {
                    DTShow = DTListPart[1].Split(new char[] { ',' });
                    foreach (string check in DTShow)
                        if (check == "")
                            iColCount--;
                }
            }

            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            Response.AddHeader("Content-Disposition", "attachment; filename=ExportData.xls;");
            MyHtmlWrite("<!DOCTYPE HTML PUBLIC ^-//W3C//DTD HTML 4.0 Transitional//EN^>");
            MyHtmlWrite("<HTML xmlns:o=^urn:schemas-microsoft-com:office:office^ xmlns:x=^urn:schemas-microsoft-com:office:excel^>");
            MyHtmlWrite("<head runat='server' >");
            MyHtmlWrite("<TITLE>" + strHeaderLine + "</TITLE>");
            MyHtmlWrite("<meta name=^GENERATOR^ content=^Microsoft Visual Studio .NET 7.1^>");
            MyHtmlWrite("<meta name=^CODE_LANGUAGE^ content=^Visual Basic .NET 7.1^>");
            MyHtmlWrite("<meta name=^vs_defaultClientScript^ content=^JavaScript^>");
            MyHtmlWrite("<meta name=^vs_targetSchema^ content=^http://schemas.microsoft.com/intellisense/ie5^>");
            MyHtmlWrite("</head>");

            MyHtmlWrite("<body>");
            MyHtmlWrite("<TABLE borderColor=^#808080^ border=^1^>");

            MyHtmlWrite("<Thead>");
            MyHtmlWrite("<TR>");
            MyHtmlWrite("<TH colspan=^" + iColCount.ToString() + "^ style=^height: 33.0pt; width: 1024pt; color: #99CC00; font-size: 16.0pt; font-weight: 700;" +
                " text-align: center; font-style: normal; text-decoration: none; vertical-align: middle; white-space: nowrap; border: medium none; padding-left: 1px; " +
                "padding-right: 1px; padding-top: 1px^>" +
                "<B><FONT face=^新細明體^ type=^string^>" + strHeaderLine + "</FONT></B></TH>");
            MyHtmlWrite("</TR>");

            //輸出條件列
            if (!string.IsNullOrEmpty(strQueryLine))
            {
                string[] strQLlist = strQueryLine.Split(new char[] { ',' });
                for (iCol = 0; iCol < strQLlist.Length; iCol++)
                {
                    if (iCol % 2 == 0)
                        MyHtmlWrite("<TR>");
                    if (iCol % 2 == 0)
                        MyHtmlWrite("<TH style=^text-align: left;^>");
                    else
                        MyHtmlWrite("<TH style=^text-align: left;^ colspan=^" + (iColCount - 1).ToString() + "^ >");
                    MyHtmlWrite("<B><FONT color=^BLUE^ face=^Arial^ size=^2^>" + strQLlist[iCol] + "</FONT></B></TH>");
                    if (iCol % 2 == 1)
                        MyHtmlWrite("</TR>");
                }
            }

            MyHtmlWrite("<TR>");

            MyHtmlWrite("<td colspan=^" + iColCount.ToString() + "^ height=^22^ style=^height: 16.5pt; text-align: right; color: windowtext; font-size: 12.0pt; font-weight: 400; font-style: normal; text-decoration: none; , serif; vertical-align: middle; white-space: nowrap; border-left: medium none; border-right: medium none;" +
                "border-top: medium none; border-bottom: .5pt solid gray; padding-left: 1px; padding-right: 1px; padding-top: 1px^>列印日期 : " + DateTime.Now.ToString("yyyy/MM/dd") + "</TH>");
            MyHtmlWrite("</TR>");

            MyHtmlWrite("<TR>");


            //輸出標題列
            if (string.IsNullOrEmpty(strDataTitel))
            {
                for (iCol = 0; iCol < PrintDT.Columns.Count; iCol++)
                {
                    Ssql = "Select dbo.GetColumnTitle('" + strTable.Trim() + "','" + PrintDT.Columns[iCol].ColumnName + "')";

                    DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
                    if (DT.Rows.Count > 0)
                    {
                        MyHtmlWrite("<TH BGCOLOR=^blue^>");
                        GetHeader(DT.Rows[0][0].ToString());
                    }
                }
            }
            else
            {
                string[] strDTlist = DTListPart[0].Split(new char[] { ',' });
                for (iCol = 0; iCol < strDTlist.Length; iCol++)
                {
                    MyHtmlWrite("<TH BGCOLOR=^blue^>");
                    GetHeader(strDTlist[iCol]);
                }
            }

            MyHtmlWrite("</TR>");
            MyHtmlWrite("</Thead>");
            MyHtmlWrite("<TBODY>");

            //將數據導出到相應的儲存格
            for (iRow = 0; iRow < PrintDT.Rows.Count; iRow++)
            {
                MyHtmlWrite("<TR>");
                dr = PrintDT.Rows[iRow];
                ary = dr.ItemArray;
                for (iCol = 0; iCol < ary.Length; iCol++)
                {
                    if (DTShow != null)
                    {
                        if (DTShow[iCol] != "")
                        {
                            string tempValue = "", tempAlign = "left", tempStyle = "x:str";
                            tempValue = ary[iCol].ToString();
                            switch (DTShow[iCol])
                            {
                                case "S":
                                case "L":
                                    break;
                                case "R":
                                    tempAlign = "right";
                                    break;
                                case "C":
                                    tempAlign = "center";
                                    break;                                
                                case "":
                                    tempValue = "{DISABLE}";
                                    break;
                                default:
                                    tempValue = ary[iCol].ToString();
                                    decimal tempDec = 0;
                                    if (decimal.TryParse(tempValue, out tempDec))
                                    {
                                        tempValue = tempDec.ToString(DTShow[iCol]);
                                        tempAlign = "right";
                                        tempStyle = "";
                                    }
                                    break;
                            }
                            if (tempValue != "{DISABLE}")
                                MyHtmlWrite("<TD align=^" + tempAlign + "^ " + tempStyle + "><B><FONT face=^Arial^ color=^#800000^ size=^2^>" + tempValue + "</FONT></B></TD>");                            
                        }
                    }
                    else
                        MyHtmlWrite("<TD align=^left^ x:str><B><FONT face=^Arial^ color=^#800000^ size=^2^>" + ary[iCol].ToString() + "</FONT></B></TD>");
                }
                MyHtmlWrite("</TR>");
            }
            MyHtmlWrite("<TR></TR><TR>");
            MyHtmlWrite("<TH colspan=^" + iColCount.ToString() + "^ style=^height: 20.0pt; width: 1024pt; color: BLUE; font-size: 8.0pt; font-weight: 700; text-align: right; font-style: normal; text-decoration: none; vertical-align: middle; white-space: nowrap; border: medium none; padding-left: 1px; padding-right: 1px; padding-top: 1px^>" +
                        "<B><FONT face=^新細明體^ type=^string^>泛太資訊科技開發股份有限公司 Pan Pacific Information Technology and Development Inc. 授權提供</FONT></B></TH>");
            MyHtmlWrite("</TR>");
            MyHtmlWrite("</TBODY>");
            MyHtmlWrite("</TABLE>");
            MyHtmlWrite("</body>");
            MyHtmlWrite("</HTML>");
        }
    }

    private void MyHtmlWrite(string strInput)
    {
        strInput = strInput.Replace("^", "" + (char)34);
        Response.Write(strInput + "" + (char)13);
    }

    private void GetData(string theExcelData)
    {
        //資料列填入至 MyDataTable中的"Location"資料表中
        Session["MyDataTable"]  = _MyDBM.ExecuteDataTable(theExcelData);
    }

    private void GetHeader(string strTitle)
    {
        MyHtmlWrite("<B><FONT color=^white^ face=^Arial^ size=^2^>" + strTitle + "</FONT></B></TH>");
    }
}
