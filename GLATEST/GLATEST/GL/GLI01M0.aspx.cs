using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using PanPacificClass;


public partial class GLI01M0 : System.Web.UI.Page
{
   
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLI01M0";
    DBManger _MyDBM;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        //Page.Theme = "Theme_09";
        //if (Session["Theme"] != null)
        //    Page.Theme = Session["Theme"].ToString();

        //if (Session["MasterPage"] != null)
        //    Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";
    }

   
    
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtAcctYear.Text = DateTime.Now.AddYears(-1).Year.ToString();
        }
       // txtAcctYear.Attributes.Add("onkeypress", "function checkColumns(this)");
        
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            DrpCompanyList.SelectValue = Session["Company"].ToString();
            DrpCompanyList.StyleAdd("width", "270px");
            DrpCompanyList.AutoPostBack = true;
            DrpCompanyList.SelectedChanged += new UserControl_CompanyList.SelectedIndexChanged(ddlCompany_SelectedIndexChanged);
            ddlAcc.StyleAdd("width", "270px");
        }
        else
        {
            if (Request.Form[0].Contains(DrpCompanyList.UniqueID))
                BindAccDrop(DrpCompanyList.SelectValue);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string CompTmp = DrpCompanyList.SelectValue;
            BindAccDrop(CompTmp);
        }
    }

    /// <summary>
    /// 建立帳號下拉單
    /// </summary>
    /// <param name="CompTmp">公司代碼</param>
    protected void BindAccDrop(string CompTmp)
    {
        ddlAcc.SetDTList("GLAcctDef", "AcctNo", "AcctDesc1", "Company='" + CompTmp + "'", 2);
        //Response.Write("1:" + DateTime.Now.ToLongTimeString());
    }

    void ddlCompany_SelectedIndexChanged(object sender, UserControl_CompanyList.SelectEventArgs e)
    {
        string CompTmp = DrpCompanyList.SelectValue;
        BindAccDrop(CompTmp);
        //Response.Write("2:" + DateTime.Now.ToLongTimeString());
    }


    /// <summary>
    /// 檢查輸入年份
    /// </summary>
    /// <param name="checkDate"></param>
    /// <returns></returns>
    private string checkYear(string checkDate)
    {
        int idate;
        if (checkDate == "")
        {
            return "請輸入年份。";    
        
        }

        if (int.TryParse(checkDate,out idate) == false)
        {
            return "請輸入正確數字。";
        }

        if (checkDate.Length != 4)
        {
            return "請輸入完整年份";        
        }

        return "";
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string strerrMsg = checkYear(txtAcctYear.Text.Trim());
        if (strerrMsg!= "")
        {
            JsUtility.ClientMsgBoxAjax(strerrMsg, UpdatePanel1, "");
            return;
        }


        string tYear = txtAcctYear.Text;
        string tCompany = DrpCompanyList.SelectValue.Trim();
        string tAcct = ddlAcc.SelectedCode.Trim();
        string tFlag = ddlAlocFlag.SelectedValue;
        string tDate = "";
        string strSQL = "";

        _MyDBM = new DBManger();
        _MyDBM.New();

        try
        {
            strSQL = "select LastPeriodEnd from dbo.fnGetAccPeriodInfo('" + tCompany + "'," + Convert.ToDecimal(tYear) + ",'01')";
            //doClientMsgBoxAjax.ClientMsgBoxAjax(strSQL, UpdatePanel2, "");


            DataTable dtP = _MyDBM.ExecuteDataTable(strSQL);

            if (dtP.Rows.Count != 0)
            {
                tDate = dtP.Rows[0]["LastPeriodEnd"].ToString().Trim();
               
            }
            else
            {
                JsUtility.ClientMsgBoxAjax(strSQL + ":error", UpdatePanel1, "");
               
            }
            
        }
        catch (InvalidCastException ex)
        {
            throw (ex);
           
        }
        finally
        {

        }

        
////----------------
        //tDate = Convert.ToString(Convert.ToInt16(txtAcctYear.Text)-1) + "1231";
        //doClientMsgBoxAjax.ClientMsgBoxAjax(tDate, UpdatePanel1, "");


        strSQL = @"SELECT AcctType,AcctCtg from GLAcctDef
              WHERE Company=@tCompany AND AcctNo=@tAcctNo";
        //strSQL += " WHERE Company='" + tCompany + "' AND";
        //strSQL += " AcctNo='131102'";
        //strSQL += " AcctNo='" + tAcct + "'";
        SqlCommand Sqlcmd = new SqlCommand();
        Sqlcmd.Parameters.Add("@tCompany", SqlDbType.Char).Value = tCompany;
        Sqlcmd.Parameters.Add("@tAcctNo", SqlDbType.Char).Value = tAcct;

        //doClientMsgBoxAjax.ClientMsgBoxAjax(strSQL, UpdatePanel2, "");

        DataTable dt = _MyDBM.ExecuteDataTable(strSQL, Sqlcmd.Parameters, CommandType.Text);        

        string tType = "";
        string tCtg = "";     
    
        //string[][] aryNew = new Array();
        //string[][] aryNew;

        if (dt.Rows.Count == 1)
        {
            tType = dt.Rows[0]["AcctType"].ToString();
            tCtg = dt.Rows[0]["AcctCtg"].ToString();

            //doClientMsgBoxAjax.ClientMsgBoxAjax(tType + "/" + tCtg, UpdatePanel2, "");

        }
        else
        {
            JsUtility.ClientMsgBoxAjax("此科目定義檔有誤, 請檢查。", UpdatePanel1, "");
            return;
        }

        //AcctType
        if (tType == "1")
        {
            Session["AcctLastAmt"] = GetLastAmt(tCompany, tDate, "1", tAcct);
            //doClientMsgBoxAjax.ClientMsgBoxAjax(Session["AcctLastAmt"].ToString(), UpdatePanel2, "");
            GetNewAmt(tCompany, tYear, tAcct, tCtg, Convert.ToDecimal(Session["AcctLastAmt"]), tFlag);
        }
        else if (tType == "2")
        {
            Session["AcctLastAmt"] = "0.00";
            //doClientMsgBoxAjax.ClientMsgBoxAjax(Session["AcctLastAmt"].ToString(), UpdatePanel2, "");
            GetNewAmt(tCompany, tYear, tAcct, tCtg, 0, tFlag);
        }
        else
        {
            JsUtility.ClientMsgBoxAjax("此科目定義檔有誤, 請檢查。", UpdatePanel1, "");
            return;
        }

       
        this.lbl_LastAmt.Text = TransData(Convert.ToDecimal(Session["AcctLastAmt"]));
        

    }
    /// <summary>
    /// 取得總合
    /// </summary>
    /// <param name="tCompany">公司代號</param>
    /// <param name="tDate">日期</param>
    /// <param name="tCode">定義碼</param>
    /// <param name="tAcctNo">帳號名稱</param>
    /// <returns></returns>
    protected string GetLastAmt(string tCompany, string tDate, string tCode, string tAcctNo)
    {

        _MyDBM = new DBManger();
        _MyDBM.New();

        string tLastAmt;  
        try
        {

          
        string sql = "select LastAmt=isnull(dbo.fnGetAccAmt(@Company,@Date,@Code,@AcctNo1,@AcctNo2,'--'),'0.00')";
        SqlCommand sqlcmd = new SqlCommand();

        sqlcmd.Parameters.Add("@Company", SqlDbType.Char, 2).Value = tCompany.ToString().Trim();
        sqlcmd.Parameters.Add("@Date", SqlDbType.Char, 8).Value = tDate.ToString().Trim();
        sqlcmd.Parameters.Add("@Code", SqlDbType.Char, 1).Value = tCode.ToString().Trim();
        sqlcmd.Parameters.Add("@AcctNo1", SqlDbType.Char, 8).Value = tAcctNo.ToString().Trim();
        sqlcmd.Parameters.Add("@AcctNo2", SqlDbType.Char, 8).Value = tAcctNo.ToString().Trim();
        DataTable dt =_MyDBM.ExecuteDataTable(sql,sqlcmd.Parameters,CommandType.Text);

        if (dt != null && dt.Rows.Count != 0)
            {
                tLastAmt = dt.Rows[0]["LastAmt"].ToString();
                //Response.Write(sqlReturn);
            }
            else
            {
                tLastAmt = "0.00";
                //doClientMsgBoxAjax.ClientMsgBoxAjax("此科目無, 請檢查。", UpdatePanel2, "");
            }
        }
        catch (InvalidCastException e)
        {
            throw (e);
        }
        finally
        {

        }

        return tLastAmt;
    }

    protected void GetNewAmt(string tCompany, string tYear, string tAcct, string tCtg, Decimal tLastAmt, string tFlag)
    {

        _MyDBM = new DBManger();
        _MyDBM.New();

        string tNewAmt = "";
        string sql = "SELECT ";
        int i;
        string sTmp = "";
        for (i = 1; i <= 13; i++)
        {
            sql += "isnull(sum(M" + i.ToString("00") + "dbamt),0),isnull(sum(M" + i.ToString("00") + "cramt),0),";
            
        }
        sql = sql.TrimEnd(',');
        sql += " FROM GLAcctbal";
        sql += " WHERE Company=@tcompany";
        sql += " AND AcctYear=@tAccYear";
        sql += " AND AcctNo=@tAcctNo";
        sql += " AND AlocFlag=AlocFlag";

        SqlCommand sqlcmd = new SqlCommand();

        sqlcmd.Parameters.Add("@tcompany", SqlDbType.Char).Value = tCompany.Trim();
        sqlcmd.Parameters.Add("@tAccYear", SqlDbType.Int).Value = Convert.ToInt32(tYear.Trim());
        sqlcmd.Parameters.Add("@tAcctNo", SqlDbType.Char).Value = tAcct.Trim();
        sqlcmd.Parameters.Add("@AlocFlag", SqlDbType.Char).Value = tFlag;
        

        //Response.Write(sql);

        DataTable dt = _MyDBM.ExecuteDataTable(sql,sqlcmd.Parameters,CommandType.Text);              

        if (dt.Rows.Count != 0)
        {

            //aryValue[0][0] = dt.Rows[0][0].ToString();
            //aryValue[0][1] = dt.Rows[0][1].ToString();
            this.lbl_M01db.Text = TransData(Convert.ToDecimal(dt.Rows[0][0]));
            this.lbl_M01cr.Text = TransData(Convert.ToDecimal(dt.Rows[0][1]));
            this.lbl_M01.Text = AddTableValue(tCtg, dt.Rows[0][0].ToString(), dt.Rows[0][1].ToString(), Session["AcctLastAmt"].ToString());
            ////this.lbl_M01.Text = TransData(Convert.ToDecimal(dt.Rows[0][0]) - Convert.ToDecimal(dt.Rows[0][1]));

            //aryValue[1][0] = dt.Rows[0][2].ToString();
            //aryValue[1][1] = dt.Rows[0][3].ToString();
            this.lbl_M02db.Text = TransData(Convert.ToDecimal(dt.Rows[0][2]));
            this.lbl_M02cr.Text = TransData(Convert.ToDecimal(dt.Rows[0][3]));
            this.lbl_M02.Text = AddTableValue(tCtg, dt.Rows[0][2].ToString(), dt.Rows[0][3].ToString(), this.lbl_M01.Text);

            //aryValue[2][0] = dt.Rows[0][4].ToString();
            //aryValue[2][1] = dt.Rows[0][5].ToString();
            this.lbl_M03db.Text = TransData(Convert.ToDecimal(dt.Rows[0][4]));
            this.lbl_M03cr.Text = TransData(Convert.ToDecimal(dt.Rows[0][5]));
            this.lbl_M03.Text = AddTableValue(tCtg, dt.Rows[0][4].ToString(), dt.Rows[0][5].ToString(), this.lbl_M02.Text);

            //aryValue[3][0] = dt.Rows[0][6].ToString();
            //aryValue[3][1] = dt.Rows[0][7].ToString();
            this.lbl_M04db.Text = TransData(Convert.ToDecimal(dt.Rows[0][6]));
            this.lbl_M04cr.Text = TransData(Convert.ToDecimal(dt.Rows[0][7]));
            this.lbl_M04.Text = AddTableValue(tCtg, dt.Rows[0][6].ToString(), dt.Rows[0][7].ToString(), this.lbl_M03.Text);

            //aryValue[4][0] = dt.Rows[0][8].ToString();
            //aryValue[4][1] = dt.Rows[0][9].ToString();
            this.lbl_M05db.Text = TransData(Convert.ToDecimal(dt.Rows[0][8]));
            this.lbl_M05cr.Text = TransData(Convert.ToDecimal(dt.Rows[0][9]));
            this.lbl_M05.Text = AddTableValue(tCtg, dt.Rows[0][8].ToString(), dt.Rows[0][9].ToString(), this.lbl_M04.Text);

            //aryValue[5][0] = dt.Rows[0][10].ToString();
            //aryValue[5][1] = dt.Rows[0][11].ToString();
            this.lbl_M06db.Text = TransData(Convert.ToDecimal(dt.Rows[0][10]));
            this.lbl_M06cr.Text = TransData(Convert.ToDecimal(dt.Rows[0][11]));
            this.lbl_M06.Text = AddTableValue(tCtg, dt.Rows[0][10].ToString(), dt.Rows[0][11].ToString(), this.lbl_M05.Text);

            //aryValue[6][0] = dt.Rows[0][12].ToString();
            //aryValue[6][1] = dt.Rows[0][13].ToString();
            this.lbl_M07db.Text = TransData(Convert.ToDecimal(dt.Rows[0][12]));
            this.lbl_M07cr.Text = TransData(Convert.ToDecimal(dt.Rows[0][13]));
            this.lbl_M07.Text = AddTableValue(tCtg, dt.Rows[0][12].ToString(), dt.Rows[0][13].ToString(), this.lbl_M06.Text);

            //aryValue[7][0] = dt.Rows[0][14].ToString();
            //aryValue[7][1] = dt.Rows[0][15].ToString();
            this.lbl_M08db.Text = TransData(Convert.ToDecimal(dt.Rows[0][14]));
            this.lbl_M08cr.Text = TransData(Convert.ToDecimal(dt.Rows[0][15]));
            this.lbl_M08.Text = AddTableValue(tCtg, dt.Rows[0][14].ToString(), dt.Rows[0][15].ToString(), this.lbl_M07.Text);

            //aryValue[8][0] = dt.Rows[0][16].ToString();
            //aryValue[8][1] = dt.Rows[0][17].ToString();
            this.lbl_M09db.Text = TransData(Convert.ToDecimal(dt.Rows[0][16]));
            this.lbl_M09cr.Text = TransData(Convert.ToDecimal(dt.Rows[0][17]));
            this.lbl_M09.Text = AddTableValue(tCtg, dt.Rows[0][16].ToString(), dt.Rows[0][17].ToString(), this.lbl_M08.Text);

            //aryValue[9][0] = dt.Rows[0][18].ToString();
            //aryValue[9][1] = dt.Rows[0][19].ToString();
            this.lbl_M10db.Text = TransData(Convert.ToDecimal(dt.Rows[0][18]));
            this.lbl_M10cr.Text = TransData(Convert.ToDecimal(dt.Rows[0][19]));
            this.lbl_M10.Text = AddTableValue(tCtg, dt.Rows[0][18].ToString(), dt.Rows[0][19].ToString(), this.lbl_M09.Text);

            //aryValue[10][0] = dt.Rows[0][20].ToString();
            //aryValue[10][1] = dt.Rows[0][21].ToString();
            this.lbl_M11db.Text = TransData(Convert.ToDecimal(dt.Rows[0][20]));
            this.lbl_M11cr.Text = TransData(Convert.ToDecimal(dt.Rows[0][21]));
            this.lbl_M11.Text = AddTableValue(tCtg, dt.Rows[0][20].ToString(), dt.Rows[0][21].ToString(), this.lbl_M10.Text);

            //aryValue[11][0] = dt.Rows[0][22].ToString();
            //aryValue[11][1] = dt.Rows[0][23].ToString();
            this.lbl_M12db.Text = TransData(Convert.ToDecimal(dt.Rows[0][22]));
            this.lbl_M12cr.Text = TransData(Convert.ToDecimal(dt.Rows[0][23]));
            this.lbl_M12.Text = AddTableValue(tCtg, dt.Rows[0][22].ToString(), dt.Rows[0][23].ToString(), this.lbl_M11.Text);

            //aryValue[12][0] = dt.Rows[0][24].ToString();
            //aryValue[12][1] = dt.Rows[0][25].ToString();
            this.lbl_M13db.Text = TransData(Convert.ToDecimal(dt.Rows[0][24]));
            this.lbl_M13cr.Text = TransData(Convert.ToDecimal(dt.Rows[0][25]));
            this.lbl_M13.Text = AddTableValue(tCtg, dt.Rows[0][24].ToString(), dt.Rows[0][25].ToString(), this.lbl_M12.Text);

            //AddTableValue(tCtg);
        }
        else
        {

            JsUtility.ClientMsgBoxAjax("無符合年度之資料，請重新查詢。", UpdatePanel1, "");
        }
        //return aryValue;
       
    }

    protected string TransData(Decimal tValue)
    {
        string tRtnValue = "";
        
        tRtnValue = tValue.ToString("#,0.00");
       
        return tRtnValue;
    }

    protected string AddTableValue(string tCtg,string sDB,string sCR,string sAmt)
    {
        string rtnVal = "";    
        if (tCtg == "1")
            {
                rtnVal = TransData(Convert.ToDecimal(sAmt) + Convert.ToDecimal(sDB) - Convert.ToDecimal(sCR));
            }
        else
            {
                rtnVal = TransData(Convert.ToDecimal(sAmt) + Convert.ToDecimal(sCR) - Convert.ToDecimal(sDB));
            }
        return rtnVal;
    }
}
