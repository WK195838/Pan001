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
using System.Threading;

/// <summary>
/// 2013/11/15 與技術長核對沖帳應更新之資料&結果
/// </summary>
public partial class GL_GLA0170 : System.Web.UI.Page
{
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _SystemId = "EBOSGL";
    string _ProgramId = "GLA0170";
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
        if (string.IsNullOrEmpty(_UserInfo.UData.UserId))
        {
            //取得登入資訊(Cookies亦可使用Session指定)        
            _UserInfo.UData = _UserInfo.GetUData(Session.SessionID);
        }
        string strScript;
        _MyDBM = new DBManger();
        _MyDBM.New();
        //GeneralModalPopup.Ajax.ModalPopup.RegisterLoadingWebButton(LoadingPanel);
        cnn = _MyDBM.GetConnectionString();
        DrpCompanyList.SelectValue = Session["Company"].ToString();

        txtPostedDate.Text = GetPostedDate(DrpCompanyList.SelectValue);
        ScriptManager.RegisterStartupScript(ScriptManager1, this.GetType(), "", @"JQ();", true);
        txtPostingDate.CssClass = "JQCalendar";
        if (!IsPostBack)
        {
            txtPostingDate.Text = _UserInfo.SysSet.ToTWDate(DateTime.Now);
            //txtCompanyName.Attributes.Add("ReadOnly", "ReadOnly");
            txtPostedDate.Attributes.Add("ReadOnly", "ReadOnly");
            txtStatusDesc.Attributes.Add("ReadOnly", "ReadOnly");
            txtTemp.Attributes.Add("style", "display:none");
            this.Title = "傳票過帳作業";
            // 取得傳票來源
            txtVourSrc.SetCodeList("AH17", 4, "全部");
        }
    }
    protected void imgbtnReset_Click(object sender, ImageClickEventArgs e)
    {
        txtStatusDesc.Text = "";
    }
    protected void imgbtnPosting_Click(object sender, ImageClickEventArgs e)
    {
        //doLedgerPosting(DrpCompanyList.SelectValue.Trim(),_UserInfo.SysSet.FormatADDate(txtPostingDate.Text.ToString().Trim()),txtVourSrc.Text.ToString().Trim());
        doSPLedgerPosting(DrpCompanyList.SelectValue.Trim(), _UserInfo.SysSet.FormatADDate(txtPostingDate.Text.ToString().Trim()), txtVourSrc.Text.ToString().Trim());
    }

    private void doSPLedgerPosting(string tCompany, string tPostingDate, string tSource)
    {
        _MyDBM = new DBManger();
        _MyDBM.New();

        SqlCommand sqlcmd = new SqlCommand();
        DataTable DT = new DataTable();

        //2013/11/22經技術長測試過帳無誤
        string sql = "sp_GLA0170";

        sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = tCompany;
        sqlcmd.Parameters.Add("@PostingDate", SqlDbType.Char).Value = tPostingDate.Replace("/","");
        sqlcmd.Parameters.Add("@Source", SqlDbType.VarChar).Value = tSource;
        sqlcmd.Parameters.Add("@PostingUser", SqlDbType.VarChar).Value = _UserInfo.UData.UserId;
        
        DT = _MyDBM.ExecStoredProcedure(sql, sqlcmd.Parameters);
        if (DT.Rows.Count > 0)
        {
            if (DT.Rows[0][0] != null)
                txtStatusDesc.Text = DT.Rows[0][0].ToString();
            else
                txtStatusDesc.Text = "";
            txtPostedDate.Text = GetPostedDate(DrpCompanyList.SelectValue);
        }
    }
        
    /// <summary>
    /// 執行分錄過帳作業
    /// </summary>
    /// <param name="tCompany">公司別</param>
    /// <param name="tVoucherNo">傳票編號</param>
    /// <param name="tYearMonth">科目餘額的年月</param>
    /// <param name="tPeriodNo">科目餘額的期數</param>
    /// <returns></returns>
    /// <remarks></remarks>
    protected void doJournailPosting(string tCompany, string tVoucherNo, string tYear, string tPeriodNo, string tAcctNo, 
        string tIndex01, string tIdx01, string tIndex02, string tIdx02, string tIndex03, string tIdx03,
        string tIndex04, string tIdx04, string tIndex05, string tIdx05,
        string tAllocationCode, string tAcctCtg, decimal tDBAmt, decimal tCRAmt)
    {
        //Company+Acctyear+AcctNo+Index01+Index02+Index03+Index04+Index05+AlocFlag 唯一值
        //先判斷GLAcctBal是是已有資料，如果沒有則新增一筆
        string sql1 = "SELECT Company, Acctyear, AcctNo, Index01, Index02, Index03, Index04, Index05, AlocFlag";
        sql1 += " FROM GLAcctBal";
        sql1 += " WHERE Company=@Company AND Acctyear=@Acctyear AND AcctNo=@AcctNo";
        sql1 += "   AND Index01=@Index01 AND Index02=@Index02 AND Index03=@Index03";
        sql1 += "   AND Index04=@Index04 AND Index05=@Index05 AND AlocFlag=@AlocFlag";
        DataTable dt1 = new DataTable();
        SqlDataAdapter adpt1 = new SqlDataAdapter(sql1, cnn);
        #region
        try
        {
            adpt1.SelectCommand.Parameters.Add("@Company", SqlDbType.Char, 2).Value = tCompany;
            adpt1.SelectCommand.Parameters.Add("@Acctyear", SqlDbType.Decimal).Value = Convert.ToDecimal(tYear);
            adpt1.SelectCommand.Parameters.Add("@AcctNo", SqlDbType.Char, 8).Value = tAcctNo;
            adpt1.SelectCommand.Parameters.Add("@Index01", SqlDbType.Char, 20).Value = tIndex01;
            adpt1.SelectCommand.Parameters.Add("@Index02", SqlDbType.Char, 20).Value = tIndex02;
            adpt1.SelectCommand.Parameters.Add("@Index03", SqlDbType.Char, 20).Value = tIndex03;
            adpt1.SelectCommand.Parameters.Add("@Index04", SqlDbType.Char, 20).Value = tIndex04;
            adpt1.SelectCommand.Parameters.Add("@Index05", SqlDbType.Char, 20).Value = tIndex05;
            adpt1.SelectCommand.Parameters.Add("@AlocFlag", SqlDbType.Char, 1).Value = tAllocationCode;

            adpt1.Fill(dt1);
            if (dt1.Rows.Count > 0)
            {

            }
            else
            {
                //GLAcctBal新增一筆資料
                string sql2 = "INSERT INTO GLAcctBal";
                sql2 += " (Company,Acctyear,AcctNo,Index01,Index02,Index03,Index04,Index05,";
                sql2 += "  Idx01,Idx02,Idx03,Idx04,Idx05,AlocFlag,LstChgUser,LstChgDateTime,";
                sql2 += "  M01dbamt,M02dbamt,M03dbamt,M04dbamt,M05dbamt,M06dbamt,M07dbamt,M08dbamt,M09dbamt,M10dbamt,M11dbamt,M12dbamt,M13dbamt,";
                sql2 += "  M01cramt,M02cramt,M03cramt,M04cramt,M05cramt,M06cramt,M07cramt,M08cramt,M09cramt,M10cramt,M11cramt,M12cramt,M13cramt";
                sql2 += ") VALUES (";
                sql2 += "  @Company,@Acctyear,@AcctNo,@Index01,@Index02,@Index03,@Index04,@Index05,";
                sql2 += "  @Idx01,@Idx02,@Idx03,@Idx04,@Idx05,@AlocFlag,@LstChgUser,@LstChgDateTime,";
                sql2 += "  @M01dbamt,@M02dbamt,@M03dbamt,@M04dbamt,@M05dbamt,@M06dbamt,@M07dbamt,@M08dbamt,@M09dbamt,@M10dbamt,@M11dbamt,@M12dbamt,@M13dbamt,";
                sql2 += "  @M01cramt,@M02cramt,@M03cramt,@M04cramt,@M05cramt,@M06cramt,@M07cramt,@M08cramt,@M09cramt,@M10cramt,@M11cramt,@M12cramt,@M13cramt)";
                try
                {
                    DataTable dt2 = new DataTable();
                    SqlDataAdapter adpt2 = new SqlDataAdapter(sql2, cnn);
                    #region
                    adpt2.SelectCommand.Parameters.Add("@Company", SqlDbType.Char, 2).Value = tCompany;
                    adpt2.SelectCommand.Parameters.Add("@Acctyear", SqlDbType.Decimal).Value = Convert.ToDecimal(tYear);
                    adpt2.SelectCommand.Parameters.Add("@AcctNo", SqlDbType.Char, 8).Value = tAcctNo;
                    adpt2.SelectCommand.Parameters.Add("@Index01", SqlDbType.Char, 20).Value = tIndex01;
                    adpt2.SelectCommand.Parameters.Add("@Index02", SqlDbType.Char, 20).Value = tIndex02;
                    adpt2.SelectCommand.Parameters.Add("@Index03", SqlDbType.Char, 20).Value = tIndex03;
                    adpt2.SelectCommand.Parameters.Add("@Index04", SqlDbType.Char, 20).Value = tIndex04;
                    adpt2.SelectCommand.Parameters.Add("@Index05", SqlDbType.Char, 20).Value = tIndex05;
                    adpt2.SelectCommand.Parameters.Add("@Idx01", SqlDbType.Char, 2).Value = tIdx01;
                    adpt2.SelectCommand.Parameters.Add("@Idx02", SqlDbType.Char, 2).Value = tIdx02;
                    adpt2.SelectCommand.Parameters.Add("@Idx03", SqlDbType.Char, 2).Value = tIdx03;
                    adpt2.SelectCommand.Parameters.Add("@Idx04", SqlDbType.Char, 2).Value = tIdx04;
                    adpt2.SelectCommand.Parameters.Add("@Idx05", SqlDbType.Char, 2).Value = tIdx05;
                    adpt2.SelectCommand.Parameters.Add("@AlocFlag", SqlDbType.Char, 1).Value = tAllocationCode;
                    adpt2.SelectCommand.Parameters.Add("@LstChgUser", SqlDbType.VarChar, 20).Value = "";    // Session["fLoginUser"].ToString();
                    adpt2.SelectCommand.Parameters.Add("@LstChgDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.ToShortDateString();
                    adpt2.SelectCommand.Parameters.Add("@M01dbamt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M02dbamt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M03dbamt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M04dbamt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M05dbamt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M06dbamt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M07dbamt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M08dbamt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M09dbamt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M10dbamt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M11dbamt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M12dbamt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M13dbamt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M01cramt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M02cramt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M03cramt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M04cramt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M05cramt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M06cramt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M07cramt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M08cramt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M09cramt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M10cramt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M11cramt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M12cramt", SqlDbType.Decimal).Value = 0;
                    adpt2.SelectCommand.Parameters.Add("@M13cramt", SqlDbType.Decimal).Value = 0;
                    #endregion
                    adpt2.SelectCommand.Connection.Open();
                    adpt2.SelectCommand.ExecuteNonQuery();
                    adpt2.SelectCommand.Connection.Close();
                }
                catch (InvalidCastException e)
                {
                    throw (e);
                }
            }
        }
        catch (InvalidCastException e)
        {
            throw (e);
        }
        finally
        {

        }
        #endregion
        //Update DBamt(借方金額), CRAmt(貸方金額)
        string sql3 = "UPDATE GLAcctBal SET ";
        //2013/11/15 簡化之
        sql3 += "M" + tPeriodNo + "dbamt=M" + tPeriodNo + "dbamt+" + tDBAmt + ",";
        sql3 += "M" + tPeriodNo + "cramt=M" + tPeriodNo + "cramt+" + tCRAmt + ",";
        //2013/11/15 修正：科目屬性決定在餘額欄時應加或減
        if (tAcctCtg == "1")
            sql3 += "M" + tPeriodNo + "bal=IsNull(M" + tPeriodNo + "bal,0)+" + tDBAmt + "-" + tCRAmt + "";
        else
            sql3 += "M" + tPeriodNo + "bal=IsNull(M" + tPeriodNo + "bal,0)-" + tDBAmt + "+" + tCRAmt + "";
        #region 為防萬一保留前人寫法
        //if (tPeriodNo == "01")
        //{
        //    sql3 += "M01dbamt=M01dbamt+" + tDBAmt + ",";
        //    sql3 += "M01cramt=M01cramt+" + tCRAmt;
        //}
        //if (tPeriodNo == "02")
        //{
        //    sql3 += "M02dbamt=M02dbamt+" + tDBAmt + ",";
        //    sql3 += "M02cramt=M02cramt+" + tCRAmt;
        //}
        //if (tPeriodNo == "03")
        //{
        //    sql3 += "M03dbamt=M03dbamt+" + tDBAmt + ",";
        //    sql3 += "M03cramt=M03cramt+" + tCRAmt;
        //}
        //if (tPeriodNo == "04")
        //{
        //    sql3 += "M04dbamt=M04dbamt+" + tDBAmt + ",";
        //    sql3 += "M04cramt=M04cramt+" + tCRAmt;
        //}
        //if (tPeriodNo == "05")
        //{
        //    sql3 += "M05dbamt=M05dbamt+" + tDBAmt + ",";
        //    sql3 += "M05cramt=M05cramt+" + tCRAmt;
        //}
        //if (tPeriodNo == "06")
        //{
        //    sql3 += "M06dbamt=M06dbamt+" + tDBAmt + ",";
        //    sql3 += "M06cramt=M06cramt+" + tCRAmt;
        //}
        //if (tPeriodNo == "07")
        //{
        //    sql3 += "M07dbamt=M07dbamt+" + tDBAmt + ",";
        //    sql3 += "M07cramt=M07cramt+" + tCRAmt;
        //}
        //if (tPeriodNo == "08")
        //{
        //    sql3 += "M08dbamt=M08dbamt+" + tDBAmt + ",";
        //    sql3 += "M08cramt=M08cramt+" + tCRAmt;
        //}
        //if (tPeriodNo == "09")
        //{
        //    sql3 += "M09dbamt=M09dbamt+" + tDBAmt + ",";
        //    sql3 += "M09cramt=M09cramt+" + tCRAmt;
        //}
        //if (tPeriodNo == "10")
        //{
        //    sql3 += "M10dbamt=M10dbamt+" + tDBAmt + ",";
        //    sql3 += "M10cramt=M10cramt+" + tCRAmt;
        //}
        //if (tPeriodNo == "11")
        //{
        //    sql3 += "M11dbamt=M11dbamt+" + tDBAmt + ",";
        //    sql3 += "M11cramt=M11cramt+" + tCRAmt;
        //}
        //if (tPeriodNo == "12")
        //{
        //    sql3 += "M12dbamt=M12dbamt+" + tDBAmt + ",";
        //    sql3 += "M12cramt=M12cramt+" + tCRAmt;
        //}
        //if (tPeriodNo == "13")
        //{
        //    sql3 += "M13dbamt=M13dbamt+" + tDBAmt + ",";
        //    sql3 += "M13cramt=M13cramt+" + tCRAmt;
        //}
        #endregion
        sql3 += " WHERE Company=@Company AND Acctyear=@Acctyear AND AcctNo=@AcctNo";
        sql3 += "   AND Index01=@Index01 AND Index02=@Index02 AND Index03=@Index03";
        sql3 += "   AND Index04=@Index04 AND Index05=@Index05 AND AlocFlag=@AlocFlag";

        try
        {
            DataTable dt3 = new DataTable();
            SqlDataAdapter adpt3 = new SqlDataAdapter(sql3, cnn);
            adpt3.SelectCommand.Parameters.Add("@Company", SqlDbType.Char, 2).Value = tCompany;
            adpt3.SelectCommand.Parameters.Add("@Acctyear", SqlDbType.Decimal).Value = Convert.ToDecimal(tYear);
            adpt3.SelectCommand.Parameters.Add("@AcctNo", SqlDbType.Char, 8).Value = tAcctNo;
            adpt3.SelectCommand.Parameters.Add("@Index01", SqlDbType.Char, 20).Value = tIndex01;
            adpt3.SelectCommand.Parameters.Add("@Index02", SqlDbType.Char, 20).Value = tIndex02;
            adpt3.SelectCommand.Parameters.Add("@Index03", SqlDbType.Char, 20).Value = tIndex03;
            adpt3.SelectCommand.Parameters.Add("@Index04", SqlDbType.Char, 20).Value = tIndex04;
            adpt3.SelectCommand.Parameters.Add("@Index05", SqlDbType.Char, 20).Value = tIndex05;
            adpt3.SelectCommand.Parameters.Add("@AlocFlag", SqlDbType.Char, 1).Value = tAllocationCode;

            adpt3.SelectCommand.Connection.Open();
            adpt3.SelectCommand.ExecuteNonQuery();
            adpt3.SelectCommand.Connection.Close();
        }
        catch (InvalidCastException e)
        {
            throw (e);
        }

    }

    /// <summary>
    /// 執行沖立帳
    /// </summary>
    /// <param name="tCompany">公司</param>
    /// <param name="tVoucherNo">傳票號碼</param>
    /// <param name="tvoucherDate">傳票日期</param>
    /// <param name="tOffsetJunNo">沖帳傳票號碼</param>
    /// <param name="tAcctNo">會計科目</param>
    /// <param name="tVoucherSeqNo">傳票序號</param>
    /// <param name="tindex01">欄位一</param>
    /// <param name="tindex02">欄位二</param>
    /// <param name="tindex03">欄位三</param>
    /// <param name="tindex04">欄位四</param>
    /// <param name="tindex05">欄位五</param>
    /// <param name="tDBamt">借方金額</param>
    /// <param name="tCRamt">貸方金額</param>
    /// <param name="tAcctCtg">借貸屬性</param>
    protected void doJournailBalance ( string tCompany,string tVoucherNo,string tvoucherDate ,string tOffsetJunNo,string tAcctNo,
        string tVoucherSeqNo, string tindex01,string tindex02, string tindex03,string tindex04, string tindex05
        , decimal tDBamt,decimal tCRamt,string tAcctCtg )
   {

       _MyDBM = new DBManger();
       _MyDBM.New();
        decimal A_AMT=0;
        decimal B_AMT=0;
        decimal DecOffsetAmt=0;
        string strStartJournalNo;
        string strStartSeqNo;
        string strOffsetJournalNo;


       string strSQL = "";
       //
       if (tAcctCtg == "1" )
       {//借屬性
           #region
           if (tDBamt > 0)
           {
               #region 檢查立帳資料
               if (CheckOffsetHead(tCompany,"", tAcctNo,tindex01, tindex02, tindex03, tindex04, tindex05,
                 out A_AMT, out B_AMT, out strStartJournalNo,out strStartSeqNo,out DecOffsetAmt) == false)
               {
                   //立帳正帳
                   InsertOffsetHead(tCompany, tAcctNo, tVoucherNo, tindex01, tindex02, tindex03, tindex04, tindex05,
                       tDBamt, 0, "", tvoucherDate, strStartSeqNo);

               }
               else
               {//立帳變沖帳沖負帳
                   #region
                   if (A_AMT <= B_AMT)
                   {
                       //沖帳
                       //是否指定號碼沖帳
                       if (tOffsetJunNo != "")
                       {
                           //傳入指定的傳票號碼                        
                           CheckOffsetHead(tCompany, tOffsetJunNo, tAcctNo, tindex01, tindex02, tindex03, tindex04, tindex05,
                               out A_AMT, out B_AMT, out strStartJournalNo, out strStartSeqNo, out DecOffsetAmt);
                       }
                       else
                       {
                           CheckOffsetHead(tCompany, "", tAcctNo, tindex01, tindex02, tindex03, tindex04, tindex05,
                               out A_AMT, out B_AMT, out strStartJournalNo, out strStartSeqNo, out DecOffsetAmt);
                       }
                       
                       //新增沖帳
                       InsertOffsetDetail(tCompany, tAcctNo, strStartJournalNo, tindex01, tindex02, tindex03, tindex04, tindex05,
                           tOffsetJunNo, "", tDBamt * -1, strStartSeqNo, tVoucherNo);

                       //沖帳後更新立帳餘額
                       updateOffsetHead(DecOffsetAmt + (tDBamt * -1), tvoucherDate.Substring(0, 8), tAcctNo, strStartJournalNo, tindex01,
                           tindex02, tindex03, tindex04, tindex04, tindex05, strStartSeqNo);

                       //檢查是否有餘額
                       if (A_AMT - (tDBamt * -1) != 0)
                       {
                           //有餘額則檢查是否有可沖傳票

                           if (CheckOffsetHead(tCompany, "", tAcctNo, tindex01, tindex02, tindex03, tindex04, tindex05,
                         out A_AMT, out B_AMT, out strStartJournalNo, out strStartSeqNo, out DecOffsetAmt) == true)
                           {
                               //沖帳
                           }
                           else
                           {
                               //立付帳                        
                           }
                       }                       
                   }
                   else
                   {
                       //沖帳
                       InsertOffsetDetail(tCompany, tAcctNo, tVoucherNo, tindex01, tindex02, tindex03, tindex04, tindex05,
                       tOffsetJunNo, "", tDBamt, tVoucherSeqNo, tVoucherSeqNo);
                       //
                   }
                   #endregion
               }
               #endregion
           }

           //沖帳
           if (tCRamt > 0)
           {
               #region 檢查是否有立帳資料
               if (CheckOffsetHead(tCompany, "", tAcctNo, tindex01, tindex02, tindex03, tindex04, tindex05,
                     out A_AMT, out B_AMT, out strStartJournalNo,out strStartSeqNo,out DecOffsetAmt) == true)
               {
                   //有沖帳
                   InsertOffsetDetail(tCompany, tAcctNo, tVoucherNo, tindex01, tindex02, tindex03, tindex04, tindex05,
                 tOffsetJunNo, "", tCRamt, tVoucherSeqNo, tVoucherSeqNo);


               }
               else
               {
                   //無立負帳
                   InsertOffsetHead(tCompany, tAcctNo, tVoucherNo, tindex01, tindex02, tindex03, tindex04, tindex05,
                       tCRamt * -1, 0, "", tvoucherDate, tVoucherSeqNo);
               }
               #endregion
           }
           #endregion
       }
       else
       {//貸屬性(AcctCtg == "2")
           #region 
           //立帳負帳
           if (tCRamt > 0)
           {
               #region 檢查立帳資料
               if (CheckOffsetHead(tCompany,tVoucherNo, tAcctNo, tindex01, tindex02, tindex03, tindex04, tindex05,out A_AMT,out B_AMT,
                    out tVoucherSeqNo,out strStartJournalNo,out DecOffsetAmt) == false)
               {
                   //立負帳
                   InsertOffsetHead(tCompany, tAcctNo, tVoucherNo, tindex01, tindex02, tindex03, tindex04, tindex05,
                       tCRamt * -1, 0, "", tvoucherDate, tVoucherSeqNo);
               }
               else
               {
                   //沖帳負賬
                   InsertOffsetDetail(tCompany, tAcctNo, tVoucherNo, tindex01, tindex02, tindex03, tindex04, tindex05,
                      tOffsetJunNo, "", tCRamt * -1, tVoucherSeqNo, tVoucherSeqNo);

               }
               #endregion
           }

           //沖帳
           if (tDBamt > 0)
           {
               #region 檢查是否有立帳資料
               if (CheckOffsetHead(tCompany,tVoucherNo, tAcctNo, tindex01, tindex02, tindex03, tindex04, tindex05,out A_AMT,out B_AMT,
                  out strStartJournalNo, out tVoucherSeqNo, out DecOffsetAmt) == true)
               {
                   //有沖負帳
                   InsertOffsetDetail(tCompany, tAcctNo, tVoucherNo, tindex01, tindex02, tindex03, tindex04, tindex05,tOffsetJunNo, "", tDBamt * -1, tVoucherSeqNo, tVoucherSeqNo);
               }
               else
               {
                   //無立負帳
                   InsertOffsetHead(tCompany, tAcctNo, tVoucherNo, tindex01, tindex02, tindex03, tindex04, tindex05,tDBamt * -1, 0, "", tvoucherDate, tVoucherSeqNo);
               }
               #endregion
           }
           #endregion
       }
        


   }

    /// <summary>
    /// 檢查立帳
    /// </summary>
    /// <returns></returns>
    protected bool CheckOffsetHead(string tcompany,string tVoucherNo,string tAcctNo,string tindex01,
        string tindex02,string tindex03,string tindex04,string tindex05,out decimal A_AMT,
        out decimal B_AMT, out string tStartJournalNo,out string tStartJournalseq, out decimal tOffsetAmt)
    {
        _MyDBM = new DBManger();
        _MyDBM.New();

        SqlCommand sqlcmd = new SqlCommand();

        bool bolData = false;
        A_AMT=0;
        B_AMT=0;
        tOffsetAmt = 0;
        tStartJournalNo = "";
        tStartJournalseq = "";
        

        
      string  strSQL = @"SELECT StartAmt,(StartAmt-OffsetAmt)*-1 AS B_AMT,OffsetAmt,
StartJournalNo,StartJournalseq
FROM GLOffsethead WHERE company=@comapny AND 
AcctNo=@AcctNo
AND Index01=@index01 AND Index02=@index02 AND 
Index03=@index03 AND Index04=@index04 AND Index05=index05
AND (StartAmt-OffsetAmt)*-1<0 ";
     

      sqlcmd.Parameters.Add("@comapny", SqlDbType.Char).Value = tcompany;
      sqlcmd.Parameters.Add("@AcctNo", SqlDbType.Char).Value = tAcctNo;     
      sqlcmd.Parameters.Add("@index01", SqlDbType.Char).Value = tindex01;
      sqlcmd.Parameters.Add("@index02", SqlDbType.Char).Value = tindex02;
      sqlcmd.Parameters.Add("@index03", SqlDbType.Char).Value = tindex03;
      sqlcmd.Parameters.Add("@index04", SqlDbType.Char).Value = tindex04;
      sqlcmd.Parameters.Add("@index05", SqlDbType.Char).Value = tindex05;

      if (tVoucherNo != "")
      {
          strSQL += " AND StartJournalNo=@StartJournalNo  ";

          sqlcmd.Parameters.Add("@StartJournalNo", SqlDbType.Char).Value = tVoucherNo;
      }
        

      DataTable DT = _MyDBM.ExecuteDataTable(strSQL,sqlcmd.Parameters,CommandType.Text);
      if (DT.Rows.Count > 0)
      {
          bolData = true;

          A_AMT=decimal.Parse(DT.Rows[0]["StartAmt"].ToString());
          B_AMT=decimal.Parse(DT.Rows[0]["B_AMT"].ToString());
          tStartJournalNo = DT.Rows[0]["StartJournalNo"].ToString();
          //2013/11/15 補上缺漏的
          if (DT.Rows[0]["StartJournalseq"] != null) tStartJournalseq = DT.Rows[0]["StartJournalseq"].ToString();
          tOffsetAmt = decimal.Parse(DT.Rows[0]["OffsetAmt"].ToString());
      }

      return bolData;
    
    }


    /// <summary>
    /// 檢查沖帳後餘額
    /// </summary>
    /// <param name="tcompany"></param>
    /// <param name="tAcctNo"></param>
    /// <param name="tindex01"></param>
    /// <param name="tindex02"></param>
    /// <param name="tindex03"></param>
    /// <param name="tindex04"></param>
    /// <param name="tindex05"></param>
    /// <param name="A_AMT"></param>
    /// <param name="B_AMT"></param>
    /// <returns></returns>
    protected bool CheckOffsetDetail(string tcompany, string tAcctNo, string tindex01,
        string tindex02, string tindex03, string tindex04, string tindex05, out decimal AMT)
    {
        _MyDBM = new DBManger();
        _MyDBM.New();

        SqlCommand sqlcmd = new SqlCommand();
        AMT = 0;

        bool bolData = false;


        string strSQL = @"SELECT StartAmt,StartAmt-OffsetAmt AS AMT
FROM GLOffsethead WHERE company=@comapny AND 
AcctNo=@AcctNo
AND Index01=@index01 AND Index02=@index02 AND 
Index03=@index03 AND Index04=@index04 AND Index05=index05
AND (StartAmt-OffsetAmt)<0";

        sqlcmd.Parameters.Add("@comapny", SqlDbType.Char).Value = tcompany;
        sqlcmd.Parameters.Add("@AcctNo", SqlDbType.Char).Value = tAcctNo;
        sqlcmd.Parameters.Add("@index01", SqlDbType.Char).Value = tindex01;
        sqlcmd.Parameters.Add("@index02", SqlDbType.Char).Value = tindex02;
        sqlcmd.Parameters.Add("@index03", SqlDbType.Char).Value = tindex03;
        sqlcmd.Parameters.Add("@index04", SqlDbType.Char).Value = tindex04;
        sqlcmd.Parameters.Add("@index05", SqlDbType.Char).Value = tindex05;



        DataTable DT = _MyDBM.ExecuteDataTable(strSQL, sqlcmd.Parameters, CommandType.Text);
        if (DT.Rows.Count > 0)
        {
            bolData = true;
            AMT = decimal.Parse(DT.Rows[0]["AMT"].ToString());           

        }



        return bolData;


    
    }


    /// <summary>
    /// 新增立帳資料
    /// </summary>
    protected void InsertOffsetHead( string tCompany,string tAcctNo,string tStartJournalNo,string tindex01,string tindex02,string tindex03
        ,string tindex04,string tindex05,decimal tStartAmt,decimal tOffsetAmt,string tOffsetDate,
        string tStartJournalDate,string tStartJournalseq )
    {
        _MyDBM = new DBManger();
        _MyDBM.New();

        SqlCommand sqlcmd = new SqlCommand();

      string  strSQL = @" INSERT INTO GLOffsethead
         (Company,AcctNo,StartjournalNo,index01,index02,index03,index04,index05,StartAmt,
          OffsetAmt,OffsetDate,StartJournalDate,StartJournalSeq,LstchgUser,LstChgDateTime)
          VALUES ( @Company,@AcctNo,@StartjournalNo,@index01,@index02,@index03,@index04,@index05,@StartAmt,
          @OffsetAmt,@OffsetDate,@StartJournalDate,@StartJournalSeq,@LstchgUser,@LstChgDateTime) ";

         sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = tCompany;
         sqlcmd.Parameters.Add("@AcctNo", SqlDbType.Char).Value = tAcctNo;
         sqlcmd.Parameters.Add("@StartjournalNo", SqlDbType.Char).Value = tStartJournalNo;
         sqlcmd.Parameters.Add("@index01", SqlDbType.Char).Value = tindex01;
         sqlcmd.Parameters.Add("@index02", SqlDbType.Char).Value = tindex02;
         sqlcmd.Parameters.Add("@index03", SqlDbType.Char).Value = tindex03;
         sqlcmd.Parameters.Add("@index04", SqlDbType.Char).Value = tindex04;
         sqlcmd.Parameters.Add("@index05", SqlDbType.Char).Value = tindex05;
         sqlcmd.Parameters.Add("@StartAmt", SqlDbType.Decimal).Value = tStartAmt;
         sqlcmd.Parameters.Add("@OffsetAmt", SqlDbType.Decimal).Value = tOffsetAmt;
         //2013/11/15 補上缺漏的參數---start
         sqlcmd.Parameters.Add("@OffsetDate", SqlDbType.Char).Value = tOffsetDate;
         sqlcmd.Parameters.Add("@StartJournalDate", SqlDbType.Char).Value = tStartJournalDate;
         decimal theDec = 0;
         decimal.TryParse(tStartJournalseq, out theDec);
         sqlcmd.Parameters.Add("@StartJournalSeq", SqlDbType.Decimal).Value = theDec;         
         sqlcmd.Parameters.Add("@LstChgUser", SqlDbType.Char).Value = _UserInfo.UData.UserId;
         //2013/11/15 補上缺漏的參數---end
         sqlcmd.Parameters.Add("@LstChgDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.ToShortDateString();

         _MyDBM.ExecuteCommand(strSQL,sqlcmd.Parameters,CommandType.Text);
    
    
    }

    /// <summary>
    /// 更新立帳資料
    /// </summary>
    protected void updateOffsetHead(decimal tOffsetAmt,string tOffsetDate, string tCompany
        ,string tAcctNo,string tStartJournalNo, string tindex01,string tindex02,string tindex03,string tindex04,
        string tindex05,string tStartJournalseq)
    {
        _MyDBM = new DBManger();
        _MyDBM.New();
        SqlCommand sqlcmd = new SqlCommand();

      string   strSQL = @"UPDATE GLOffsethead SET OffsetAmt=@OffsetAmt,OffSetDate=@OffsetDate,
                Where Company=@company AND AcctNo=@AcctNo,
                AND StartJounrnalNo=@StartJournalno AND index01=@index01 AND index02=@index02 AND index03=@index03 
                AND index04=@index04 AND index05=@index05 AND StartJournalSeq=@StartJournalSeq";

             sqlcmd.Parameters.Add("@OffsetAmt", SqlDbType.Char).Value = tOffsetAmt;
             sqlcmd.Parameters.Add("@OffsetDate", SqlDbType.Char).Value = tOffsetDate;            
             sqlcmd.Parameters.Add("@company", SqlDbType.Char).Value = tCompany;
             sqlcmd.Parameters.Add("@AcctNo", SqlDbType.Char).Value = tAcctNo;
             sqlcmd.Parameters.Add("@StartJournalno", SqlDbType.Char).Value = tStartJournalNo;
             sqlcmd.Parameters.Add("@index01", SqlDbType.Char).Value = tindex01;
             sqlcmd.Parameters.Add("@index02", SqlDbType.Char).Value = tindex02;
             sqlcmd.Parameters.Add("@index03", SqlDbType.Char).Value = tindex03;
             sqlcmd.Parameters.Add("@index04", SqlDbType.Char).Value = tindex04;
             sqlcmd.Parameters.Add("@index05", SqlDbType.Char).Value = tindex05;
             sqlcmd.Parameters.Add("@StartJournalSeq", SqlDbType.Char).Value = tStartJournalseq;

             _MyDBM.ExecuteCommand(strSQL,sqlcmd.Parameters,CommandType.Text);


    }

    /// <summary>
    /// 新增沖帳資料
    /// </summary>
    protected void InsertOffsetDetail(string tcompany,string tAcctNo,string tStsrtJournalNo,string tindex01,
          string tindex02,string tindex03,string tindex04, string tindex05,string tOffsetJournalNo,
          string tOffsetJournalDate,decimal tOffsetAmt, string tStartJournalseq,string tOffsetJournalseq)
    {

        _MyDBM = new DBManger();
        _MyDBM.New();

        SqlCommand sqlcmd = new SqlCommand( );

       string  strSQL = @"INSERT INTO GLOffsetdetail (Company,Acctno,StartJournalNo,index01,index02,index03,index04,index05,
                OffsetJournalNo,OffsetJournalDate,OffsetAmt,StartJournalSeq,OffsetJournalSeq,LstChgUser,LstChgDateTime)
                VALUES(@Company,@Acctno,@StartJournalNo,@index01,@index02,@index03,@index04,@index05,
                @OffsetJournalNo,@OffsetJournalDate,@OffsetAmt,@StartJournalSeq,@OffsetJournalSeq,@LstChgUser,@LstChgDateTime ) ";

               sqlcmd.Parameters.Add("@Company",SqlDbType.Char).Value=tcompany;
               sqlcmd.Parameters.Add("@Acctno", SqlDbType.Char).Value = tAcctNo;
               sqlcmd.Parameters.Add("@StartJournalNo", SqlDbType.Char).Value =tStsrtJournalNo;
               sqlcmd.Parameters.Add("@index01", SqlDbType.Char).Value = tindex01;
               sqlcmd.Parameters.Add("@index02", SqlDbType.Char).Value = tindex02;
               sqlcmd.Parameters.Add("@index03", SqlDbType.Char).Value = tindex03;
               sqlcmd.Parameters.Add("@index04", SqlDbType.Char).Value = tindex04;
               sqlcmd.Parameters.Add("@index05", SqlDbType.Char).Value = tindex05;
               sqlcmd.Parameters.Add("@OffsetJournalNo", SqlDbType.Char).Value = tOffsetJournalNo;
               sqlcmd.Parameters.Add("@OffsetJournalDate", SqlDbType.Char).Value = tOffsetJournalDate;
               sqlcmd.Parameters.Add("@OffsetAmt", SqlDbType.Char).Value = tOffsetAmt;               
               sqlcmd.Parameters.Add("@StartJournalSeq", SqlDbType.Char).Value = tStartJournalseq;
               sqlcmd.Parameters.Add("@OffsetJournalseq", SqlDbType.Char).Value = tOffsetJournalseq;
               //2013/11/15 補上缺漏的參數---start
               sqlcmd.Parameters.Add("@LstChgUser", SqlDbType.Char).Value = _UserInfo.UData.UserId;
               //2013/11/15 補上缺漏的參數---end
               sqlcmd.Parameters.Add("@LstChgDateTime", SqlDbType.Char).Value = DateTime.Now.ToShortDateString();

               _MyDBM.ExecuteCommand(strSQL, sqlcmd.Parameters, CommandType.Text);

    }





    /// <summary>
    /// 取得公司簡稱
    /// </summary>
    /// <param name="tCompany">公司別</param>
    /// <returns>公司簡稱</returns>
    /// <remarks></remarks>
    protected string GetCompanyName(string tCompany)
    {
        string tCompanyName = "";
        string sql = "select CompanyShortName from Company where Company=@Company";
        DataTable dt = new DataTable();
        SqlDataAdapter adpt = new SqlDataAdapter(sql, cnn);
        try
        {
            adpt.SelectCommand.Parameters.Add("@Company", SqlDbType.Char, 2).Value = tCompany;

            adpt.Fill(dt);
            tCompanyName = dt.Rows[0]["CompanyShortName"].ToString();
            //Response.Write(sqlReturn);
        }
        catch (InvalidCastException e)
        {
            throw (e);
        }
        finally
        {

        }

        return tCompanyName;
    }
    /// <summary>
    /// 取得最後過帳日期
    /// </summary>
    /// <param name="tCompany">公司別</param>
    /// <returns>(String)最後過帳日期(格式：yyyy/MM/dd)</returns>
    /// <remarks></remarks>
    protected string GetPostedDate(string tCompany)
    {
        string tPostedDate = "";
        string sql = "select LastPostDate from GLParm where Company=@Company";
        DataTable dt = new DataTable();
        SqlDataAdapter adpt = new SqlDataAdapter(sql, cnn);
        try
        {
            adpt.SelectCommand.Parameters.Add("@Company", SqlDbType.Char, 2).Value = tCompany;

            adpt.Fill(dt);
            tPostedDate = _UserInfo.SysSet.FormatDate(dt.Rows[0]["LastPostDate"].ToString());
            //Response.Write(sqlReturn);
        }
        catch (InvalidCastException e)
        {
            throw (e);
        }
        finally
        {

        }

        return tPostedDate;
    }

    /// <summary>
    /// 轉換日期格式 ConvertDateFormatToString
    /// </summary>
    /// <param name="tString">yyyy/MM/dd(傳入的日期字串)</param>
    /// <returns>yyyyMMdd(格式化的的日期字串)</returns>
    /// <remarks>yyyyMMdd</remarks>
    protected string ConvertDateFormatToString(string tString)
    {
        if (tString == "")
        {
            return "";
        }
        else
        {
            return tString.Replace("/","");
        }
    }


}
