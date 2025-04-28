using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// FindName 的摘要描述
/// </summary>
public class FASys
{

    public FASys()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
	}
    public void SetGridViewStyle(GridView GV)
    {
        GV. CssClass="FA_GV";
        GV.AutoGenerateColumns=false;
        GV.AllowSorting = true;
        GV.Attributes.Add("bordercolor", "#9A9A9B");
        GV.GridLines = GridLines.Both;
        GV.PagerSettings.Position = PagerPosition.Bottom;
        GV.PagerStyle.HorizontalAlign = HorizontalAlign.Center;
        GV.PagerStyle.VerticalAlign = VerticalAlign.Middle;

    }
    // 設定Table名稱:tablename 需要回傳欄位:retValue where判斷參數:FieldName 傳入參數:PaId 
    /// <summary>
    /// 尋找代號相關名稱
    /// </summary>
    /// <param name="TableName">設定Table名稱</param>
    /// <param name="RetValue">需要回傳欄位</param>
    /// <param name="FieldName">where判斷參數</param>
    /// <param name="PaId">傳入參數</param>
    /// <param name="Other">傳入其它條件</param>
    /// <param name="reStatus">設定回傳值的樣式</param>
    /// <returns></returns>
    public string findName(string TableName, string RetValue, string FieldName, string PaId, string Other,int reStatus)
    {
        string[] retV = RetValue.Split(new char[] { ',' });
        string re = "";
        DataTable dt = DTable(TableName, RetValue, FieldName, PaId, Other);
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                switch (reStatus)
                {
                    //回傳一個值
                    case 0:
                        for (int i = 0; i < retV.Length; i++)
                        {
                            if (i != 0)
                            {
                                re += " - ";

                            }
                            re += dt.Rows[0][retV[i].ToString()].ToString();
                        }
                        break;
                    //回傳 代號與名稱
                    case 1:
                        for (int i = 0; i < retV.Length; i++)
                        {
                            if (i != 0)
                            {
                                re += " - ";

                            }
                            re += dt.Rows[0][retV[i].ToString()].ToString();
                        }
                        break;
                    //回傳n個值
                    case 2:
                        for (int i = 0; i < retV.Length; i++)
                        {

                            re += dt.Rows[0][retV[i].ToString()].ToString();
                            re += ",";
                        }
                        re = re.TrimEnd(',');
                        break;
                }

                return re;
            }
        }
        return re;
    }
    public DropDownList DDList(DropDownList ddl, string TableName, string RetValue, string FieldName, string PaId,string Other,int FuncNum,int RetNum)
    {
        string[] retV = RetValue.Split(new char[] { ',' });
        string re = "";
        DBManger _MyDBM = new DBManger();
        _MyDBM.New();
        DataTable dt = DTable(TableName, RetValue, FieldName, PaId,Other);
        //ddl.Style.Add("width", ""+width+"px");
        switch (FuncNum)
        {
            case 0:
                break;
            case 1:
                ddl.Items.Add(new ListItem("無", ""));
                break;
            case 2:
                ddl.Items.Add(new ListItem("請選擇", ""));
                break;
            default:
                break;
        }

        if (dt != null)
        {
            switch (RetNum)
            {
                case 1:
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {

                        ddl.Items.Add(new ListItem(dt.Rows[j][retV[1].ToString()].ToString().Trim(), dt.Rows[j][retV[0].ToString()].ToString().Trim()));
                    }
                    break;
                case 2:
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {

                        ddl.Items.Add(new ListItem(dt.Rows[j][retV[0].ToString()].ToString().Trim() + " - " + dt.Rows[j][retV[1].ToString()].ToString().Trim(), dt.Rows[j][retV[0].ToString()].ToString().Trim()));
                    }
                    break;
            }
        }
        return ddl;
    }
    public string[] BreakDownYM(string YM)
    {
        if (YM.Length > 5)
        {
            string year = YM.Substring(0, 4);
            string month = YM.Substring(4, 2);
            return new string[] { YM, year, month };
        }
        else
        {
             string year = "";
             string month = "";
            return new string[] { YM, year, month };
        }
    }
    public string[] BreakDownDate(string DateTime)
    {
        string bdDate = Convert.ToDateTime(DateTime).ToShortDateString();//.Replace("/", "").Substring(0, 8);
        string year =  Convert.ToDateTime(DateTime).Year.ToString("D4");
        string month = Convert.ToDateTime(DateTime).Month.ToString("D2");
        string day = Convert.ToDateTime(DateTime).Day.ToString("D2");
        string date = year + month + day;
        return new string[] { date, year, month, day , bdDate};
    }
    private DataTable DTable(string TableName, string RetValue, string FieldName, string PaId,string Other)
    {
        DBManger _MyDBM = new DBManger();
        DataTable dt = new DataTable();
        _MyDBM.New();
        string Ssql = "";
        string[] retV = RetValue.Split(new char[] { ',' });
        string[] FName = FieldName.Split(new char[] { ',' });
        string[] PId = PaId.Split(new char[] { ',' });

        if (TableName.Length > 0 && retV.Length > 0)
        {
            Ssql = "Select " + RetValue + " From " + TableName + " Where 0=0 ";
            if (FName.Length == PId.Length)
            {
                if (FieldName != "" )
                {
                    for (int i = 0; i < FName.Length; i++)
                    {
                        Ssql += " And " + FName[i].ToString() + "='" + PId[i].ToString() + "'";
                    }
                }
            }
            Ssql += " "+Other;
            dt = _MyDBM.ExecuteDataTable(Ssql);
        }
        return dt;
    }
    public string TWtoADDate(string TWDate)
    {
        if (TWDate.Trim() != "")
        {
            string[] TDate = TWDate.Split(new char[] { '/' });
            string ADYear = (int.Parse(TDate[0].ToString()) + 1911).ToString("D4");
            string ADMonth = int.Parse(TDate[1].ToString()).ToString("D2");
            string ADDay = int.Parse(TDate[2].ToString()).ToString("D2");
            return ADYear + "/" + ADMonth + "/" + ADDay;
        }
        return "";
    }
    public string ADtoTWDate(string ADDate)
    {
        if (ADDate.Trim() != "")
        {
            ADDate = DateTime.Parse(ADDate).ToShortDateString();
            string[] ADate = ADDate.Split(new char[] { '/' });
            string TWYear = (int.Parse(ADate[0].ToString()) - 1911).ToString("D2");
            string TWMonth= int.Parse(ADate[1].ToString()).ToString("D2");
            string TWDay = int.Parse(ADate[2].ToString()).ToString("D2");
            return TWYear + "/" + TWMonth + "/" + TWDay;
        }
        return "";
    }
    public void findButton(object sender, GridViewRowEventArgs e, string OpenName, string[] GVKey, int WBWidth, int WBHeight, string scrollbars, string resizable)
    {
        for (int i = 0; i < e.Row.Cells.Count; i++)
        {
            if (((DataControlFieldCell)(e.Row.Cells[i])).ContainingField.GetType().Name == "TemplateField")
            {
                string Name = "";
                LinkButton link;
                for (int j = 0; j < e.Row.Cells[i].Controls.Count; j++)
                {
                    Name = e.Row.Cells[i].Controls[j].GetType().Name;
                    if (Name == "LinkButton")
                    {
                        switch (e.Row.Cells[i].Controls[j].ID.ToUpper())
                        {
                            case "BTNSELECT":
                                link = (LinkButton)e.Row.Cells[i].Controls[j];
                                link.Attributes.Add("onclick", RetJsString(sender, e, OpenName, "I", GVKey, WBWidth, WBHeight, scrollbars, resizable));
                                break;
                            case "BTNEDIT":
                                link = (LinkButton)e.Row.Cells[i].Controls[j];
                                link.Attributes.Add("onclick", RetJsString(sender, e, OpenName, "U", GVKey, WBWidth, WBHeight, scrollbars, resizable));
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            //e.Row.Cells[i];" + OpenName + "_I.aspx" + para + "','','" + WinOpen + "
        }
    }
    private string RetJsString(object sender, GridViewRowEventArgs e, string OpenName, string Type, string[] GVKey, int WBWidth, int WBHeight, string Scrollbars, string Resizable)
    {
        string TransPM = "";
        TransPM += OpenName+"_"+Type+".aspx?";
        for (int i = 0; i < GVKey.Length; i++)
        {
            TransPM += GVKey[i].ToString() + "=" + ((GridView)((CompositeDataBoundControl)sender)).DataKeys[e.Row.RowIndex].Values[GVKey[i].ToString()].ToString().Trim();
            TransPM += "&";
        }
        TransPM = TransPM.TrimEnd('&');
        string Left = ((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - WBWidth) / 2).ToString() + "px";
        string Top = ((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - WBHeight) / 2).ToString() + "px";
        string Width = WBWidth.ToString() + "px";
        string Height = WBHeight.ToString() + "px";
        string RetValue = "javascript:var win =window.open('" + TransPM + "','','width=" + Width + ",height=" + Height + ",top=" + Top + ",left=" + Left + ",scrollbars=" + Scrollbars + ",resizable=" + Resizable + "');";
        return RetValue;
    }
    public void OpenJSAdd(object sender, string OpenName, int WBWidth, int WBHeight, string Scrollbars, string Resizable)
    {
        string Left = ((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - WBWidth) / 2).ToString() + "px";
        string Top = ((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - WBHeight) / 2).ToString() + "px";
        string Width = WBWidth.ToString() + "px";
        string Height = WBHeight.ToString() + "px";
        string TransPM = "";
        TransPM += OpenName + "_A.aspx";
        string Url = "javascript:var win =window.open('" + TransPM + "','','width=" + Width + ",height=" + Height + ",top=" + Top + ",left=" + Left + ",scrollbars=" + Scrollbars + ",resizable=" + Resizable + "');";
        ((ImageButton)sender).Attributes.Add("onclick", Url);
    }

}
