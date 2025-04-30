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
using System.Web.UI.HtmlControls;
using PanPacificClass;

public partial class GLA520 : System.Web.UI.Page
{
       
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLA0520";
    DBManger _MyDBM;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        //Page.Theme = "Theme_09";
        //if (Session["Theme"] != null)
        //    Page.Theme = Session["Theme"].ToString();

        //if (Session["MasterPage"] != null)
        //    Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        _MyDBM = new DBManger();
        _MyDBM.New();


        this.btnClose.Attributes.Add("onClick", "drawWait('')");
        this.txtCloseYear.Attributes.Add("ReadOnly", "ReadOnly");
        //txtAcctPeriod.Attributes.Add("ReadOnly", "ReadOnly");

        //暫時將closeYYYY and close YM 為零者剔除


        if (!IsPostBack)
        {
            string sdCompSQL = "SELECT Company,  Company + ' ' +  CompanyName AS CompanyName FROM Company";
            sdCompSQL += " WHERE Company in (SELECT Distinct company from GLParm where CloseYYYY<>0 and CloseYM<>0)";

            DrpCompanyList.SelectValue = Session["Company"].ToString();
            DrpCompanyList.StyleAdd("width", "270px");
            DrpCompanyList.AutoPostBack = true;
            DrpCompanyList.SelectedChanged += new UserControl_CompanyList.SelectedIndexChanged(ddlCompany_SelectedIndexChanged);
        }
        ddlCompany_SelectedIndexChanged(null, null);
    }

    protected void FillDropDownList(DropDownList DDL, String SetValue, String SetText, DataTable dt)
    {
        DDL.DataSource = dt;
        DDL.DataTextField = SetText;
        DDL.DataValueField = SetValue;
        DDL.DataBind();
    }

    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        _MyDBM = new DBManger();
        _MyDBM.New();

        //if (IsPostBack)
        {
            if (DrpCompanyList.SelectValue != "")
            {

                string tCompany = DrpCompanyList.SelectValue.ToString().Trim();
                Session["tCompany"] = tCompany;                              

                try
                {
                    //object tYearMonth ;


                    string sqlGetYM = "select CloseYYYY,convert(char(6),closeYM) as CloseYM from glparm where company='" + tCompany + "'";

                    DataTable dt1 = new DataTable();
                    dt1 = _MyDBM.ExecuteDataTable(sqlGetYM);
                    if (dt1 != null && dt1.Rows.Count > 0)
                    {
                        Session["AcctYear"] = dt1.Rows[0][0].ToString();

                        string tYear = dt1.Rows[0][0].ToString();
                        string tPeriod = dt1.Rows[0][1].ToString().Substring(4, 2);

                        string sql = "SELECT LastYear,NextYear";
                        sql += " FROM dbo.fnGetAccPeriodInfo('" + tCompany + "'," + tYear + ",'" + tPeriod + "')";

                        DataTable dt2 = new DataTable();
                        dt2 = _MyDBM.ExecuteDataTable(sql);
                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            ////adpt2.SelectCommand.Parameters.Add("@Company", SqlDbType.Char, 2).Value = "'" + tCompany + "'";
                            ////adpt2.SelectCommand.Parameters.Add("@Year", SqlDbType.Decimal, 4).Value = tYear;
                            ////adpt2.SelectCommand.Parameters.Add("@PeriodNo", SqlDbType.Char, 2).Value = "'" + tMonth + "'";
                            
                            Session["LastYear"] = dt2.Rows[0]["LastYear"].ToString();
                            Session["NextYear"] = dt2.Rows[0]["NextYear"].ToString();
                            this.lblLastYear.Text = Session["LastYear"].ToString();
                            this.txtCloseYear.Text = Session["NextYear"].ToString();

                            //if (dt2.Rows[0]["LastYear"].ToString() == dt2.Rows[0]["NextYear"].ToString())
                            //{
                            //    JsUtility.ClientMsgBoxAjax("本年度月結未完成, 請檢查。", UpdatePanel1, "");
                            //    this.btnClose.Enabled = false;
                            //}
                            //else
                            //{
                            //    this.btnClose.Enabled = true;
                            //}
                        }
                    }
                }
                catch (InvalidCastException ex)
                {
                    throw (ex);
                   
                }
                finally
                {
                  
                    
                }
            }
        }
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        //this.btnClose.Enabled=false;

        if (DrpCompanyList.SelectValue == "")
        {
            JsUtility.ClientMsgBoxAjax("請先選擇轉結公司", UpdatePanel1, "a");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");
            return ;
        }

        if (txtCloseYear.Text.Trim()=="")
        {
            JsUtility.ClientMsgBoxAjax("請先填入結轉度", UpdatePanel1, "a");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");
            return;
        }






        try
        {

            //1.檢查會計年度之所有月份是否已月結
            string strReturn = CheckAcctPeriod(DrpCompanyList.SelectValue.ToString().Trim(), txtCloseYear.Text);
            if (strReturn == "N")
            {
                JsUtility.ClientMsgBoxAjax("本年度月結未完成, 請檢查。", UpdatePanel1, "a");
                JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面
                return;
            }
            else //2.若是已結清則年結
            {
                CloseAcctYear(DrpCompanyList.SelectValue.ToString().Trim(), txtCloseYear.Text);
                JsUtility.ClientMsgBoxAjax("本年度年結完成。", UpdatePanel1, "a");
                JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面
                //Response.Redirect("Default.aspx");
            }
        }
        catch (InvalidCastException ex)
        {
            JsUtility.ClientMsgBoxAjax(ex.Message, UpdatePanel1, "");
        }
        finally
        {
            //this.btnClose.Enabled = true;
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面
        }


    }

    protected string CheckAcctPeriod(string strComp, string strYear)
    {

        _MyDBM = new DBManger();
        _MyDBM.New();

        string strRTN = "";      

        try
        {
            string sqlCommand = "SELECT PeriodEnd13,PeriodClose13,PeriodEnd12,PeriodClose12 ";
            sqlCommand += "FROM GLAcctPeriod WHERE Company='" + strComp + "' AND AcctYear='" + strYear + "'";

            DataTable dtCheck = new DataTable();

            dtCheck = _MyDBM.ExecuteDataTable(sqlCommand);
            

            if (dtCheck.Rows.Count != 0)
            {
                //判斷該會計科目期數(12 or 13)
                if (dtCheck.Rows[0][0].ToString().Trim() != "0")
                {
                    if (dtCheck.Rows[0][1].ToString().Trim() == "Y")
                    {
                        strRTN = "Y";
                    }
                    else
                    {
                        strRTN = "N";
                    }
                }
                else
                {
                    if (dtCheck.Rows[0][2].ToString().Trim() != "0")
                    {
                        if (dtCheck.Rows[0][3].ToString().Trim() == "Y")
                        {
                            strRTN = "Y";
                        }
                        else
                        {
                            strRTN = "N";
                        }
                    }
                    else
                    {
                        strRTN = "N";
                    }
                }
            }
            else
            {
                strRTN = "N";
            }
        }
        catch (InvalidCastException ex)
        {
            throw (ex);
        }
        finally
        {
            //sqlCnn.Close();
        }

        return strRTN;
    }


    //年結主程式
    protected void CloseAcctYear(string strComp, string strYear)
    {
        _MyDBM = new DBManger();
        _MyDBM.New();

        cnn = _MyDBM.GetConnectionString();
        string sqlCommand = "";

     
        SqlConnection sqlCnn = new SqlConnection(cnn);
        SqlCommand sqlCmd = new SqlCommand();
        sqlCmd.Connection = sqlCnn;
        sqlCnn.Open();

        //SqlTransaction sqltrans = sqlCmd.Transaction ;
        int iYear = Convert.ToInt16(strYear);
        iYear = iYear + 1;

        string sqlCmd1 = "";
        string sqlCmd2 = "";


        try
        {
            string sType = "";
            //1.更新實帳戶資料(不含本期損益)
            sqlCommand = "SELECT a.Company,a.AcctYear,a.AcctNo,A.Index01,a.Idx01,A.Index02,a.Idx02,A.Index03,a.Idx03,A.Index04,a.Idx04,A.Index05,a.Idx05,";
            sqlCommand += "a.AlocFlag,";
            sqlCommand += "(a.LstYearBal+a.M01Bal+a.M02Bal+a.M03Bal+a.M04Bal+a.M05Bal+a.M06Bal+a.M07Bal+a.M08Bal+a.M09Bal+a.M10Bal+a.M11Bal+a.M12Bal+a.M13Bal) as WBAL,";
            sqlCommand += "(a.LstYearDBamt+a.M01dbamt+a.M02dbamt+a.M03dbamt+a.M04dbamt+a.M05dbamt+a.M06dbamt+a.M07dbamt+a.M08dbamt+a.M09dbamt+a.M10dbamt+a.M11dbamt+a.M12dbamt+a.M13dbamt) as WDBAMT,";
            sqlCommand += "(a.LstYearCRamt+a.M01cramt+a.M02cramt+a.M03cramt+a.M04cramt+a.M05cramt+a.M06cramt+a.M07cramt+a.M08cramt+a.M09cramt+a.M10cramt+a.M11cramt+a.M12cramt+a.M13cramt) as WAMTMT";
            sqlCommand += " from glacctbal a,glacctdef b,glparm c";
            sqlCommand += " where a.acctyear='" + strYear + "'";
            sqlCommand += " and a.company='" + strComp + "'";
            sqlCommand += " and a.company=b.company";
            sqlCommand += " and a.acctno=b.acctno";
            sqlCommand += " and a.company=c.company";
            sqlCommand += " and b.accttype='1'";
            sqlCommand += " and a.acctno<>c.periodplacctno";

            //DataTable dtCount = new DataTable();
            DataTable dt1 = new DataTable();
            dt1 = _MyDBM.ExecuteDataTable(sqlCommand);
                 


            int i;
            for (i = 0; i < dt1.Rows.Count; i++)
            {
                sqlCmd1 = "SELECT COUNT(*) FROM GLACCTBAL";
                sqlCmd1 += " WHERE COMPANY='" + dt1.Rows[i][0].ToString() + "' AND ACCTYEAR=" + iYear + " AND ACCTNO='" + dt1.Rows[i][2].ToString() + "'";
                sqlCmd1 += " AND Index01='" + dt1.Rows[i][3].ToString() + "' AND Idx01='" + dt1.Rows[i][4].ToString() + "'";
                sqlCmd1 += " AND Index02='" + dt1.Rows[i][5].ToString() + "' AND Idx02='" + dt1.Rows[i][6].ToString() + "'";
                sqlCmd1 += " AND Index03='" + dt1.Rows[i][7].ToString() + "' AND Idx03='" + dt1.Rows[i][8].ToString() + "'";
                sqlCmd1 += " AND Index04='" + dt1.Rows[i][9].ToString() + "' AND Idx04='" + dt1.Rows[i][10].ToString() + "'";
                sqlCmd1 += " AND Index05='" + dt1.Rows[i][11].ToString() + "' AND Idx05='" + dt1.Rows[i][12].ToString() + "'";
                sqlCmd1 += " AND AlocFlag='" + dt1.Rows[i][13].ToString() + "'";

                sqlCmd.CommandText = sqlCmd1;
                int ct = Convert.ToInt16(sqlCmd.ExecuteScalar());
             


                if (ct == 0)
                {
                    //若無該筆則寫入
                    //sqlCmd2 = "INSERT INTO GLAcctBal(Company,AcctYear,AcctNo,Index01,Idx01,Index02,Idx02,Index03,Idx03,Index04,Idx04,Index05,Idx05,";
                    //sqlCmd2 += "AlocFlag,LstYearBal,LstYearDBamt,LstYearCRamt,";
                    //sqlCmd2 += "M01Bal,M02Bal,M03Bal,M04Bal,M05Bal,M06Bal,M07Bal,M08Bal,M09Bal,M10Bal,M11Bal,M12Bal,M13Bal,";
                    //sqlCmd2 += "M01dbamt,M02dbamt,M03dbamt,M04dbamt,M05dbamt,M06dbamt,M07dbamt,M08dbamt,M09dbamt,M10dbamt,M11dbamt,M12dbamt,M13dbamt,";
                    //sqlCmd2 += "M01cramt,M02cramt,M03cramt,M04cramt,M05cramt,M06cramt,M07cramt,M08cramt,M09cramt,M10cramt,M11cramt,M12cramt,M13cramt)";
                    //sqlCmd2 += " VALUES(";
                    //sqlCmd2 += "'" + dt1.Rows[i][0].ToString() + "'," + iYear + ",'" + dt1.Rows[i][2].ToString() + "',";
                    //sqlCmd2 += "'" + dt1.Rows[i][3].ToString() + "','" + dt1.Rows[i][4].ToString() + "',";
                    //sqlCmd2 += "'" + dt1.Rows[i][5].ToString() + "','" + dt1.Rows[i][6].ToString() + "',";
                    //sqlCmd2 += "'" + dt1.Rows[i][7].ToString() + "','" + dt1.Rows[i][8].ToString() + "',";
                    //sqlCmd2 += "'" + dt1.Rows[i][9].ToString() + "','" + dt1.Rows[i][10].ToString() + "',";
                    //sqlCmd2 += "'" + dt1.Rows[i][11].ToString() + "','" + dt1.Rows[i][12].ToString() + "',";
                    //sqlCmd2 += "'" + dt1.Rows[i][13].ToString() + "',";
                    //sqlCmd2 += dt1.Rows[i][14].ToString() + "," + dt1.Rows[i][15].ToString() + "," + dt1.Rows[i][16].ToString() + ",";
                    //sqlCmd2 += "0,0,0,0,0,0,0,0,0,0,0,0,0,";
                    //sqlCmd2 += "0,0,0,0,0,0,0,0,0,0,0,0,0,";
                    //sqlCmd2 += "0,0,0,0,0,0,0,0,0,0,0,0,0";
                    //sqlCmd2 += ")";

                    sType = "1";

                }
                else
                {
                    //若有該筆則update
                    //sqlCmd2 = "Update GLAcctBal Set LstYearBal=" + dt1.Rows[i][14].ToString() + ",";
                    //sqlCmd2 += "LstYearDBamt=" + dt1.Rows[i][15].ToString() + ",";
                    //sqlCmd2 += "LstYearCRamt=" + dt1.Rows[i][16].ToString() + " ";
                    //sqlCmd2 += " WHERE Company='" + dt1.Rows[i][0].ToString() + "'";
                    //sqlCmd2 += " AND AcctYear=" + iYear;
                    //sqlCmd2 += " AND ACCTNO='" + dt1.Rows[i][2].ToString() + "'";
                    //sqlCmd2 += " AND Index01='" + dt1.Rows[i][3].ToString() + "' AND Idx01='" + dt1.Rows[i][4].ToString() + "'";
                    //sqlCmd2 += " AND Index02='" + dt1.Rows[i][5].ToString() + "' AND Idx02='" + dt1.Rows[i][6].ToString() + "'";
                    //sqlCmd2 += " AND Index03='" + dt1.Rows[i][7].ToString() + "' AND Idx03='" + dt1.Rows[i][8].ToString() + "'";
                    //sqlCmd2 += " AND Index04='" + dt1.Rows[i][9].ToString() + "' AND Idx04='" + dt1.Rows[i][10].ToString() + "'";
                    //sqlCmd2 += " AND Index05='" + dt1.Rows[i][11].ToString() + "' AND Idx05='" + dt1.Rows[i][12].ToString() + "'";
                    //sqlCmd2 += " AND AlocFlag='" + dt1.Rows[i][13].ToString() + "'";

                    sType = "2";


                }


                sqlCmd2 = "exec dbo.sp_GLA0520 '" + sType + "', '";
                sqlCmd2 += dt1.Rows[i][0].ToString() + "', ";
                sqlCmd2 += iYear + ", '";
                sqlCmd2 += dt1.Rows[i][2].ToString() + "', '";
                sqlCmd2 += dt1.Rows[i][3].ToString() + "', '";
                sqlCmd2 += dt1.Rows[i][4].ToString() + "', '";
                sqlCmd2 += dt1.Rows[i][5].ToString() + "', '";
                sqlCmd2 += dt1.Rows[i][6].ToString() + "', '";
                sqlCmd2 += dt1.Rows[i][7].ToString() + "', '";
                sqlCmd2 += dt1.Rows[i][8].ToString() + "', '";
                sqlCmd2 += dt1.Rows[i][9].ToString() + "', '";
                sqlCmd2 += dt1.Rows[i][10].ToString() + "', '";
                sqlCmd2 += dt1.Rows[i][11].ToString() + "', '";
                sqlCmd2 += dt1.Rows[i][12].ToString() + "', '";
                sqlCmd2 += dt1.Rows[i][13].ToString() + "', ";
                sqlCmd2 += dt1.Rows[i][14].ToString() + ", ";
                sqlCmd2 += dt1.Rows[i][15].ToString() + ", ";
                sqlCmd2 += dt1.Rows[i][16].ToString() + " ";

                sqlCmd.CommandText = sqlCmd2;
                sqlCmd.ExecuteNonQuery();
                //sqltrans.Commit();

            }

            //2.處理本期損益
            sqlCommand = "SELECT a.Company,a.AcctYear,a.AcctNo,A.Index01,a.Idx01,A.Index02,a.Idx02,A.Index03,a.Idx03,A.Index04,a.Idx04,A.Index05,a.Idx05,";
            sqlCommand += "a.AlocFlag,";
            sqlCommand += "(a.LstYearBal+a.M01Bal+a.M02Bal+a.M03Bal+a.M04Bal+a.M05Bal+a.M06Bal+a.M07Bal+a.M08Bal+a.M09Bal+a.M10Bal+a.M11Bal+a.M12Bal+a.M13Bal) as WBAL,";
            sqlCommand += "(a.LstYearDBamt+a.M01dbamt+a.M02dbamt+a.M03dbamt+a.M04dbamt+a.M05dbamt+a.M06dbamt+a.M07dbamt+a.M08dbamt+a.M09dbamt+a.M10dbamt+a.M11dbamt+a.M12dbamt+a.M13dbamt) as WDBAMT,";
            sqlCommand += "(a.LstYearCRamt+a.M01cramt+a.M02cramt+a.M03cramt+a.M04cramt+a.M05cramt+a.M06cramt+a.M07cramt+a.M08cramt+a.M09cramt+a.M10cramt+a.M11cramt+a.M12cramt+a.M13cramt) as WAMTMT";
            sqlCommand += " from glacctbal a,glacctdef b,glparm c";
            sqlCommand += " where a.acctyear='" + strYear + "'";
            sqlCommand += " and a.company='" + strComp + "'";
            sqlCommand += " and a.company=b.company";
            sqlCommand += " and a.acctno=b.acctno";
            sqlCommand += " and a.company=c.company";
            sqlCommand += " and b.accttype='1'";
            sqlCommand += " and a.acctno=c.periodplacctno";

            //DataTable dtCount = new DataTable();
            DataTable dt2 = new DataTable();
            dt2 = _MyDBM.ExecuteDataTable(sqlCommand);           

            int j;
            for (j = 0; j < dt2.Rows.Count; j++)
            {
                sqlCmd1 = "SELECT COUNT(*) FROM GLACCTBAL";
                sqlCmd1 += " WHERE COMPANY='" + dt2.Rows[j][0].ToString() + "' AND ACCTYEAR=" + iYear + " AND ACCTNO='" + dt2.Rows[j][2].ToString() + "'";
                sqlCmd1 += " AND Index01='" + dt2.Rows[j][3].ToString() + "' AND Idx01='" + dt2.Rows[j][4].ToString() + "'";
                sqlCmd1 += " AND Index02='" + dt2.Rows[j][5].ToString() + "' AND Idx02='" + dt2.Rows[j][6].ToString() + "'";
                sqlCmd1 += " AND Index03='" + dt2.Rows[j][7].ToString() + "' AND Idx03='" + dt2.Rows[j][8].ToString() + "'";
                sqlCmd1 += " AND Index04='" + dt2.Rows[j][9].ToString() + "' AND Idx04='" + dt2.Rows[j][10].ToString() + "'";
                sqlCmd1 += " AND Index05='" + dt2.Rows[j][11].ToString() + "' AND Idx05='" + dt2.Rows[j][12].ToString() + "'";
                sqlCmd1 += " AND AlocFlag='" + dt2.Rows[j][13].ToString() + "'";

                sqlCmd.CommandText = sqlCmd1;
                int ct = Convert.ToInt16(sqlCmd.ExecuteScalar());

                if (ct == 0)
                {
                    ////若無該筆則寫入
                    //sqlCmd2 = "INSERT INTO GLAcctBal(Company,AcctYear,AcctNo,Index01,Idx01,Index02,Idx02,Index03,Idx03,Index04,Idx04,Index05,Idx05,";
                    //sqlCmd2 += "AlocFlag,LstYearBal,LstYearDBamt,LstYearCRamt,";
                    //sqlCmd2 += "M01Bal,M02Bal,M03Bal,M04Bal,M05Bal,M06Bal,M07Bal,M08Bal,M09Bal,M10Bal,M11Bal,M12Bal,M13Bal,";
                    //sqlCmd2 += "M01dbamt,M02dbamt,M03dbamt,M04dbamt,M05dbamt,M06dbamt,M07dbamt,M08dbamt,M09dbamt,M10dbamt,M11dbamt,M12dbamt,M13dbamt,";
                    //sqlCmd2 += "M01cramt,M02cramt,M03cramt,M04cramt,M05cramt,M06cramt,M07cramt,M08cramt,M09cramt,M10cramt,M11cramt,M12cramt,M13cramt)";
                    //sqlCmd2 += " VALUES(";
                    //sqlCmd2 += "'" + dt2.Rows[j][0].ToString() + "'," + iYear + ",'" + dt2.Rows[j][2].ToString() + "',";
                    //sqlCmd2 += "'" + dt2.Rows[j][3].ToString() + "','" + dt2.Rows[j][4].ToString() + "',";
                    //sqlCmd2 += "'" + dt2.Rows[j][5].ToString() + "','" + dt2.Rows[j][6].ToString() + "',";
                    //sqlCmd2 += "'" + dt2.Rows[j][7].ToString() + "','" + dt2.Rows[j][8].ToString() + "',";
                    //sqlCmd2 += "'" + dt2.Rows[j][9].ToString() + "','" + dt2.Rows[j][10].ToString() + "',";
                    //sqlCmd2 += "'" + dt2.Rows[j][11].ToString() + "','" + dt2.Rows[j][12].ToString() + "',";
                    //sqlCmd2 += "'" + dt2.Rows[j][13].ToString() + "',";
                    //sqlCmd2 += dt2.Rows[j][14].ToString() + "," + dt2.Rows[j][15].ToString() + "," + dt2.Rows[j][16].ToString() + ",";
                    //sqlCmd2 += "0,0,0,0,0,0,0,0,0,0,0,0,0,";
                    //sqlCmd2 += "0,0,0,0,0,0,0,0,0,0,0,0,0,";
                    //sqlCmd2 += "0,0,0,0,0,0,0,0,0,0,0,0,0";
                    //sqlCmd2 += ")";

                    sType = "1";
                }
                else
                {
                    ////若有該筆則update
                    //sqlCmd2 = "Update GLAcctBal Set LstYearBal=" + dt2.Rows[j][14].ToString() + ",";
                    //sqlCmd2 += "LstYearDBamt=" + dt2.Rows[j][15].ToString() + ",";
                    //sqlCmd2 += "LstYearCRamt=" + dt2.Rows[j][16].ToString() + " ";
                    //sqlCmd2 += " WHERE Company='" + dt2.Rows[j][0].ToString() + "'";
                    //sqlCmd2 += " AND AcctYear=" + iYear;
                    //sqlCmd2 += " AND ACCTNO='" + dt2.Rows[j][2].ToString() + "'";
                    //sqlCmd2 += " AND Index01='" + dt2.Rows[j][3].ToString() + "' AND Idx01='" + dt2.Rows[j][4].ToString() + "'";
                    //sqlCmd2 += " AND Index02='" + dt2.Rows[j][5].ToString() + "' AND Idx02='" + dt2.Rows[j][6].ToString() + "'";
                    //sqlCmd2 += " AND Index03='" + dt2.Rows[j][7].ToString() + "' AND Idx03='" + dt2.Rows[j][8].ToString() + "'";
                    //sqlCmd2 += " AND Index04='" + dt2.Rows[j][9].ToString() + "' AND Idx04='" + dt2.Rows[j][10].ToString() + "'";
                    //sqlCmd2 += " AND Index05='" + dt2.Rows[j][11].ToString() + "' AND Idx05='" + dt2.Rows[j][12].ToString() + "'";
                    //sqlCmd2 += " AND AlocFlag='" + dt2.Rows[j][13].ToString() + "'";

                    sType = "2";

                }

                sqlCmd2 = "exec dbo.sp_GLA0520 '" + sType + "', '";
                sqlCmd2 += dt2.Rows[j][0].ToString() + "', ";
                sqlCmd2 += iYear + ", '";
                sqlCmd2 += dt2.Rows[j][2].ToString() + "', '";
                sqlCmd2 += dt2.Rows[j][3].ToString() + "', '";
                sqlCmd2 += dt2.Rows[j][4].ToString() + "', '";
                sqlCmd2 += dt2.Rows[j][5].ToString() + "', '";
                sqlCmd2 += dt2.Rows[j][6].ToString() + "', '";
                sqlCmd2 += dt2.Rows[j][7].ToString() + "', '";
                sqlCmd2 += dt2.Rows[j][8].ToString() + "', '";
                sqlCmd2 += dt2.Rows[j][9].ToString() + "', '";
                sqlCmd2 += dt2.Rows[j][10].ToString() + "', '";
                sqlCmd2 += dt2.Rows[j][11].ToString() + "', '";
                sqlCmd2 += dt2.Rows[j][12].ToString() + "', '";
                sqlCmd2 += dt2.Rows[j][13].ToString() + "', ";
                sqlCmd2 += dt2.Rows[j][14].ToString() + ", ";
                sqlCmd2 += dt2.Rows[j][15].ToString() + ", ";
                sqlCmd2 += dt2.Rows[j][16].ToString() + " ";

                sqlCmd.CommandText = sqlCmd2;
                sqlCmd.ExecuteNonQuery();

            }

            //3.Update GLParm set CloseYYYY to I02 +1
            sqlCmd2 = "Update GLParm SET CloseYYYY=" + iYear + " WHERE Company='"+strComp+"'";
            sqlCmd.CommandText = sqlCmd2;
            sqlCmd.ExecuteNonQuery();

        }
        catch (InvalidCastException ex)
        {
            //sqltrans.Rollback(); 
            JsUtility.ClientMsgBoxAjax(ex.Message, UpdatePanel1, "a");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面

        }
        finally
        {
            sqlCnn.Close();
        }


    }

}
