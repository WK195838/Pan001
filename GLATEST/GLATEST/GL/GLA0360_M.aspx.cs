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
using System.Text;
using System.Data.SqlClient;
using PanPacificClass;

public partial class GLA0360_M : System.Web.UI.Page
{
    string Ssql = "";
    string Ssql1 = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLA0360";
    DBManger _MyDBM;   
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    string strCompany="",strAcctYear=""  ;//, sDepositBank, sDepositBankAccount
    bool blInsertMod;
   
    protected void Page_PreInit(object sender, EventArgs e)
    {//頁面初始化前,先設定好系統使用的佈景主題(主要是決定LOGO)
        //if (Session["Theme"] != null)
        //    Page.Theme = Session["Theme"].ToString();

        //if (Session["MasterPage"] != null)
        //    Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _MyDBM = new DBManger();
        _MyDBM.New();
    }
   //private void AuthRight(GridView thisGridView)
   // {
   //     //驗證權限
   //     bool Find = false;
   //     bool SetCss = false;
   //     int i = 0;

   //     string[] Auth = { "Delete", "Modify", "Add" };

   //     if (ConfigurationManager.AppSettings["EnableProgramAuth"] != null && ConfigurationManager.AppSettings["EnableProgramAuth"].ToString() == "true")
   //     {
   //         for (i = 0; i < Auth.Length; i++)
   //         {
   //             Find = _UserInfo.CheckPermission(_ProgramId, Auth[i]);
   //             if (i < (Auth.Length - 1))
   //             {//刪/修/詳
   //                 thisGridView.Columns[i].Visible = Find;
   //                 //設定標題樣式
   //                 if (Find && (SetCss == false))
   //                 {
   //                     SetCss = true;
   //                     thisGridView.Columns[i].HeaderStyle.CssClass = "paginationRowEdgeLl";
   //                 }
   //             }
   //             else
   //             {//新增
   //                 thisGridView.ShowFooter = Find;
   //             }
   //         }

   //         //因為是附加在頁籤的功能,所以一定可以查詢
   //         ////查詢(執行)
   //         //if ((_UserInfo.CheckPermission(_ProgramId)) || Find)
   //         //{
   //         //    Find = true;
   //         //}
   //         //else
   //         //{
   //         //    Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
   //         //}

   //         //版面樣式調整
          

   //     }
   // }
    protected void Page_Load(object sender, EventArgs e)
    {
        //將Javascript動態引用進MasterPage中(直接寫在頁面上會有路徑問題)        
        //用於JQuery,例如:JQ日曆
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "1", Page.ResolveUrl("~/Scripts/jquery-1.4.4.min.js").ToString());
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "2", Page.ResolveUrl("~/Scripts/jquery-ui-1.8.7.custom.min.js").ToString());
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "3", Page.ResolveUrl("~/Scripts/ui.datepicker.js").ToString());
        if (_UserInfo.SysSet.GetCalendarSetting().Equals("Y"))
        {//決定是否使用民國年
            Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "4", Page.ResolveUrl("~/Scripts/ui.datepicker.tw.js").ToString());
        }
        ScriptManager.RegisterStartupScript(UpdatePanel2, this.GetType(), "", @"JQ();", true);

        int icalendersYear = DateTime.Now.AddYears(-5).Year - 1911;
        int icalendereYear = DateTime.Now.AddYears(5).Year - 1911;
        
        //bool blCheckLogin = _UserInfo.AuthLogin;
        //if ((blCheckLogin == false) || (ConfigurationManager.AppSettings["EnableProgramAuth"] != null && ConfigurationManager.AppSettings["EnableProgramAuth"].ToString() == "true"))
        //{
        //    bool blCheckProgramAuth = false;
        //    if (blCheckLogin == false)
        //        ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg("UnLogin");
        //    else
        //    {
        //        blCheckProgramAuth = _UserInfo.CheckPermission(_ProgramId, "Add");
        //        if (blCheckProgramAuth == false)
        //            ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg("NoRight");
        //    }
        //}
        //判斷新增或修改模式,預設為[false]=[修改]
        blInsertMod = false;
        //判斷是否唯讀模式
        bool blReadOnly = false;

        //建立日期選單
        //string strScript = "return GetPromptTheDate(" + TxtPeriodBegin01.ClientID + "," + icalendersYear.ToString() + "," + icalendereYear.ToString() + ");";
        //ibOW_PeriodDate.Attributes.Add("onclick", strScript);

        //strScript = "return GetPromptTheDate(" + TxtPeriodEnd01.ClientID+ "," + icalendersYear.ToString() + "," + icalendereYear.ToString() + ");";
        //ibOW_PeriodEndDate1.Attributes.Add("onclick", strScript);     

        LabMessage.Text = "";
       
        
            btnSaveGo.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='GO';");
            if (Request["Company"] != null && Request["AcctYear"] != null)
            {
                strCompany = Request["Company"].Trim();
                strAcctYear = Request["AcctYear"].Trim();
                btnSaveGo.Visible = false;
                btnSaveGo.Enabled = false;
            }

            else
            {
                strCompany = "";
                strAcctYear = "";
                blInsertMod = true;
            }
                
      

        btnSaveExit.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='EXIT';");
        btnCancel.Attributes.Add("onclick", "javascript:window.close();");

        if (Request["Kind"] != null)
        {
            blReadOnly = Request["Kind"].Equals("Query");
        }
       
        if (!Page.IsPostBack)
        {
            if (strCompany != "" && strAcctYear != "")
            {
                BindEditData(strCompany, strAcctYear);
                if (blInsertMod == false)
                {
                    changefunction("E");
                }
                if (blReadOnly == true)
                {
                    changefunction("R");
                }
            }
        }
        else
        {
          

        }


        if (blReadOnly == true)
        {
            btnSaveGo.Visible = false;
            btnSaveGo.Enabled = false;
            btnSaveExit.Visible = false;
            btnSaveExit.Enabled = false;

        }    
    
     
    }

    private void changefunction(string Mode )
    { 
    //唯讀
        if (Mode == "R")
        {
            DrpCompany.Enabled = false;
            TxtAcctYear.Attributes.Add("ReadOnly","ReadOnly");
            TxtPeriodBegin01.Attributes.Add("ReadOnly", "ReadOnly");
            TxtPeriodEnd02.Attributes.Add("ReadOnly", "ReadOnly");
            TxtPeriodEnd03.Attributes.Add("ReadOnly", "ReadOnly");
            TxtPeriodEnd04.Attributes.Add("ReadOnly", "ReadOnly");
            TxtPeriodEnd05.Attributes.Add("ReadOnly", "ReadOnly");
            TxtPeriodEnd06.Attributes.Add("ReadOnly", "ReadOnly");
            TxtPeriodEnd07.Attributes.Add("ReadOnly", "ReadOnly");
            TxtPeriodEnd08.Attributes.Add("ReadOnly", "ReadOnly");
            TxtPeriodEnd09.Attributes.Add("ReadOnly", "ReadOnly");
            TxtPeriodEnd10.Attributes.Add("ReadOnly", "ReadOnly");
            TxtPeriodEnd11.Attributes.Add("ReadOnly", "ReadOnly");
            TxtPeriodEnd12.Attributes.Add("ReadOnly", "ReadOnly");
            TxtPeriodEnd13.Attributes.Add("ReadOnly", "ReadOnly");
        }

        //編輯
        else if(Mode=="E")
        {
            DrpCompany.Enabled = false;
            TxtAcctYear.Attributes.Add("ReadOnly", "ReadOnly");  
         


        
        }

   

    
    
    
    
    }
   
   
  
    

   

    /// <summary>
    /// 讀取修改資料
    /// </summary>
    /// <param name="strCompany"></param>
    /// <param name="strAcctYear"></param>
    private void BindEditData (string strCompany,string strAcctYear)

    {
        _MyDBM = new DBManger();
        _MyDBM.New();

        SqlCommand sqlcmd = new SqlCommand();
        DataTable DT = new DataTable();

        Ssql1 = "SELECT * FROM GLAcctPeriod WHere Company=@Company AND AcctYear=@AcctYear ";

        sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = strCompany;
        sqlcmd.Parameters.Add("@AcctYear", SqlDbType.Char).Value = strAcctYear;

         DT = _MyDBM.ExecuteDataTable(Ssql1,sqlcmd.Parameters,CommandType.Text);

        if (DT.Rows.Count > 0)
        {
            DrpCompany.SelectValue = DT.Rows[0]["Company"].ToString();
            TxtAcctYear.Text = (int.Parse(DT.Rows[0]["AcctYear"].ToString())-1911).ToString();
            if(DT.Rows[0]["PeriodBegin01"].ToString().Trim()!="" && DT.Rows[0]["PeriodBegin01"].ToString().Trim()!="0")
            {
              TxtPeriodBegin01.Text = _UserInfo.SysSet.ToTWDate(DT.Rows[0]["PeriodBegin01"].ToString());
            }
            if (DT.Rows[0]["PeriodEnd01"].ToString().Trim() != "" && DT.Rows[0]["PeriodEnd01"].ToString().Trim() != "0")
            {
              TxtPeriodEnd01.Text = _UserInfo.SysSet.ToTWDate(DT.Rows[0]["PeriodEnd01"].ToString());
            }

            if (DT.Rows[0]["PeriodEnd02"].ToString().Trim() != "" && DT.Rows[0]["PeriodEnd02"].ToString().Trim() != "0")
            {
                TxtPeriodEnd02.Text = _UserInfo.SysSet.ToTWDate(DT.Rows[0]["PeriodEnd02"].ToString());
            }
            if (DT.Rows[0]["PeriodEnd03"].ToString().Trim() != "" && DT.Rows[0]["PeriodEnd03"].ToString().Trim() != "0")
            {
                TxtPeriodEnd03.Text = _UserInfo.SysSet.ToTWDate(DT.Rows[0]["PeriodEnd03"].ToString());
            }
            if (DT.Rows[0]["PeriodEnd04"].ToString().Trim() != "" && DT.Rows[0]["PeriodEnd04"].ToString().Trim() != "0")
            {
                TxtPeriodEnd04.Text = _UserInfo.SysSet.ToTWDate(DT.Rows[0]["PeriodEnd04"].ToString());
            }
            if (DT.Rows[0]["PeriodEnd05"].ToString().Trim() != "" && DT.Rows[0]["PeriodEnd05"].ToString().Trim() != "0")
            {
                TxtPeriodEnd05.Text = _UserInfo.SysSet.ToTWDate(DT.Rows[0]["PeriodEnd05"].ToString());
            }
            if (DT.Rows[0]["PeriodEnd06"].ToString().Trim() != "" && DT.Rows[0]["PeriodEnd06"].ToString().Trim() != "0")
            {
                TxtPeriodEnd06.Text = _UserInfo.SysSet.ToTWDate(DT.Rows[0]["PeriodEnd06"].ToString());
            }
            if (DT.Rows[0]["PeriodEnd07"].ToString().Trim() != "" && DT.Rows[0]["PeriodEnd07"].ToString().Trim() != "0")
            {
                TxtPeriodEnd07.Text = _UserInfo.SysSet.ToTWDate(DT.Rows[0]["PeriodEnd07"].ToString());
            }

            if (DT.Rows[0]["PeriodEnd08"].ToString().Trim() != "" && DT.Rows[0]["PeriodEnd08"].ToString().Trim() != "0")
            {
                TxtPeriodEnd08.Text = _UserInfo.SysSet.ToTWDate(DT.Rows[0]["PeriodEnd08"].ToString());
            }

            if (DT.Rows[0]["PeriodEnd09"].ToString().Trim() != "" && DT.Rows[0]["PeriodEnd09"].ToString().Trim() != "0")
            {
                TxtPeriodEnd09.Text = _UserInfo.SysSet.ToTWDate(DT.Rows[0]["PeriodEnd09"].ToString());
            }
            if (DT.Rows[0]["PeriodEnd10"].ToString().Trim() != "" && DT.Rows[0]["PeriodEnd10"].ToString().Trim() != "0")
            {
                TxtPeriodEnd10.Text = _UserInfo.SysSet.ToTWDate(DT.Rows[0]["PeriodEnd10"].ToString());
            }
            if (DT.Rows[0]["PeriodEnd11"].ToString().Trim() != "" && DT.Rows[0]["PeriodEnd11"].ToString().Trim() != "0")
            {
                TxtPeriodEnd11.Text = _UserInfo.SysSet.ToTWDate(DT.Rows[0]["PeriodEnd11"].ToString());
            }
            if (DT.Rows[0]["PeriodEnd12"].ToString().Trim() != "" && DT.Rows[0]["PeriodEnd12"].ToString().Trim() != "0")
            {
                TxtPeriodEnd12.Text = _UserInfo.SysSet.ToTWDate(DT.Rows[0]["PeriodEnd12"].ToString());
            }
            if (DT.Rows[0]["PeriodEnd13"].ToString().Trim() != "" && DT.Rows[0]["PeriodEnd13"].ToString().Trim() != "0")
            {
                TxtPeriodEnd13.Text = _UserInfo.SysSet.ToTWDate(DT.Rows[0]["PeriodEnd13"].ToString());
            }
            LabPeriodClose1.Text = DT.Rows[0]["PeriodClose01"].ToString();
            LabPeriodClose2.Text = DT.Rows[0]["PeriodClose02"].ToString();
            LabPeriodClose3.Text = DT.Rows[0]["PeriodClose03"].ToString();
            LabPeriodClose4.Text = DT.Rows[0]["PeriodClose04"].ToString();
            LabPeriodClose5.Text = DT.Rows[0]["PeriodClose05"].ToString();
            LabPeriodClose6.Text = DT.Rows[0]["PeriodClose06"].ToString();
            LabPeriodClose7.Text = DT.Rows[0]["PeriodClose07"].ToString();
            LabPeriodClose8.Text = DT.Rows[0]["PeriodClose08"].ToString();
            LabPeriodClose9.Text = DT.Rows[0]["PeriodClose09"].ToString();
            LabPeriodClose10.Text = DT.Rows[0]["PeriodClose10"].ToString();
            LabPeriodClose11.Text = DT.Rows[0]["PeriodClose11"].ToString();
            LabPeriodClose12.Text = DT.Rows[0]["PeriodClose12"].ToString();
            LabPeriodClose13.Text = DT.Rows[0]["PeriodClose13"].ToString();
            LabPeriodBegin2.Text = TxtPeriodEnd01.Text != "" ? _UserInfo.SysSet.ToTWDate(NextSDate(TxtPeriodEnd01.Text)) : "";
            LabPeriodBegin3.Text = TxtPeriodEnd02.Text != "" ? _UserInfo.SysSet.ToTWDate(NextSDate(TxtPeriodEnd02.Text)) : "";
            LabPeriodBegin4.Text = TxtPeriodEnd03.Text != "" ? _UserInfo.SysSet.ToTWDate(NextSDate(TxtPeriodEnd03.Text)) : "";
            LabPeriodBegin5.Text = TxtPeriodEnd04.Text != "" ? _UserInfo.SysSet.ToTWDate(NextSDate(TxtPeriodEnd04.Text)) : "";
            LabPeriodBegin6.Text = TxtPeriodEnd05.Text != "" ? _UserInfo.SysSet.ToTWDate(NextSDate(TxtPeriodEnd05.Text)) : "";
            LabPeriodBegin7.Text = TxtPeriodEnd06.Text != "" ? _UserInfo.SysSet.ToTWDate(NextSDate(TxtPeriodEnd06.Text)) : "";
            LabPeriodBegin8.Text = TxtPeriodEnd07.Text != "" ? _UserInfo.SysSet.ToTWDate(NextSDate(TxtPeriodEnd07.Text)) : "";
            LabPeriodBegin9.Text = TxtPeriodEnd08.Text != "" ? _UserInfo.SysSet.ToTWDate(NextSDate(TxtPeriodEnd08.Text)) : "";
            LabPeriodBegin10.Text = TxtPeriodEnd09.Text != "" ? _UserInfo.SysSet.ToTWDate(NextSDate(TxtPeriodEnd09.Text)) : "";
            LabPeriodBegin11.Text = TxtPeriodEnd10.Text != "" ? _UserInfo.SysSet.ToTWDate(NextSDate(TxtPeriodEnd10.Text)) : "";
            LabPeriodBegin12.Text = TxtPeriodEnd11.Text != "" ? _UserInfo.SysSet.ToTWDate(NextSDate(TxtPeriodEnd11.Text)) : "";
            LabPeriodBegin13.Text = TxtPeriodEnd12.Text != "" ? _UserInfo.SysSet.ToTWDate(NextSDate(TxtPeriodEnd12.Text)) : "";

        }

        
    }

  
    /// <summary>
    /// 新增新改資料
    /// </summary>
    /// <param name="Mode">true新增false維修</param>
        private void UpdateData(bool Mode)
        {
            _MyDBM = new DBManger();
            _MyDBM.New();

            SqlCommand  sqlcmd = new SqlCommand();

            if (Mode == true)
            {

                //新增
                Ssql = @"INSERT INTO GLAcctPeriod (Company,AcctYear,PeriodClose01,PeriodBegin01,
                         PeriodEnd01,PeriodClose02,PeriodEnd02,PeriodClose03,PeriodEnd03,PeriodClose04,
                         PeriodEnd04,PeriodClose05,PeriodEnd05,PeriodClose06,PeriodEnd06,PeriodClose07,
                         PeriodEnd07,PeriodClose08,PeriodEnd08,PeriodClose09,PeriodEnd09,
                         PeriodClose10,PeriodEnd10,PeriodClose11,PeriodEnd11,PeriodClose12,PeriodEnd12,
                         PeriodClose13,PeriodEnd13,LstChgUser,LstChgDateTime )
                         VALUES (@Company,@AcctYear,@PeriodClose01,@PeriodBegin01,
                         @PeriodEnd01,@PeriodClose02,@PeriodEnd02,@PeriodClose03,@PeriodEnd03,@PeriodClose04,
                         @PeriodEnd04,@PeriodClose05,@PeriodEnd05,@PeriodClose06,@PeriodEnd06,@PeriodClose07,
                         @PeriodEnd07,@PeriodClose08,@PeriodEnd08,@PeriodClose09,@PeriodEnd09,
                         @PeriodClose10,@PeriodEnd10,@PeriodClose11,@PeriodEnd11,@PeriodClose12,@PeriodEnd12,
                         @PeriodClose13,@PeriodEnd13,@LstChgUser,@LstChgDateTime ) ";

                //新增
               
                sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = DrpCompany.SelectValue;
                sqlcmd.Parameters.Add("@AcctYear", SqlDbType.Decimal).Value = decimal.Parse(TxtAcctYear.Text)+1911;
                sqlcmd.Parameters.Add("@PeriodClose01", SqlDbType.Char).Value =" ";
                sqlcmd.Parameters.Add("@PeriodBegin01", SqlDbType.Char).Value = TxtPeriodBegin01.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodBegin01.Text).Replace("/",""):"0";
                sqlcmd.Parameters.Add("@PeriodEnd01", SqlDbType.Char).Value = TxtPeriodEnd01.Text!=""? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd01.Text).Replace("/", ""):"0"; 
                sqlcmd.Parameters.Add("@PeriodClose02", SqlDbType.Char).Value = " ";
                sqlcmd.Parameters.Add("@PeriodEnd02", SqlDbType.Char).Value = TxtPeriodEnd02.Text!=""? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd02.Text).Replace("/", ""):"0";
                sqlcmd.Parameters.Add("@PeriodClose03", SqlDbType.Char).Value = " ";
                sqlcmd.Parameters.Add("@PeriodEnd03", SqlDbType.Char).Value = TxtPeriodEnd03.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd03.Text).Replace("/", ""):"0";
                sqlcmd.Parameters.Add("@PeriodClose04", SqlDbType.Char).Value = " ";
                sqlcmd.Parameters.Add("@PeriodEnd04", SqlDbType.Char).Value = TxtPeriodEnd04.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd04.Text).Replace("/", ""): "0";
                sqlcmd.Parameters.Add("@PeriodClose05", SqlDbType.Char).Value = " ";
                sqlcmd.Parameters.Add("@PeriodEnd05", SqlDbType.Char).Value = TxtPeriodEnd05.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd05.Text).Replace("/", ""): "0";
                sqlcmd.Parameters.Add("@PeriodClose06", SqlDbType.Char).Value = " ";
                sqlcmd.Parameters.Add("@PeriodEnd06", SqlDbType.Char).Value = TxtPeriodEnd06.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd06.Text).Replace("/", ""): "0";
                sqlcmd.Parameters.Add("@PeriodClose07", SqlDbType.Char).Value = " ";
                sqlcmd.Parameters.Add("@PeriodEnd07", SqlDbType.Char).Value = TxtPeriodEnd07.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd07.Text).Replace("/", ""):"0";
                sqlcmd.Parameters.Add("@PeriodClose08", SqlDbType.Char).Value = " ";
                sqlcmd.Parameters.Add("@PeriodEnd08", SqlDbType.Char).Value = TxtPeriodEnd08.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd08.Text).Replace("/", ""):"0";
                sqlcmd.Parameters.Add("@PeriodClose09", SqlDbType.Char).Value = " ";
                sqlcmd.Parameters.Add("@PeriodEnd09", SqlDbType.Char).Value = TxtPeriodEnd09.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd09.Text).Replace("/", ""): "0";
                sqlcmd.Parameters.Add("@PeriodClose10", SqlDbType.Char).Value = " ";
                sqlcmd.Parameters.Add("@PeriodEnd10", SqlDbType.Char).Value = TxtPeriodEnd10.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd10.Text).Replace("/", ""): "0";
                sqlcmd.Parameters.Add("@PeriodClose11", SqlDbType.Char).Value = " ";
                sqlcmd.Parameters.Add("@PeriodEnd11", SqlDbType.Char).Value = TxtPeriodEnd11.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd11.Text).Replace("/", ""): "0";
                sqlcmd.Parameters.Add("@PeriodClose12", SqlDbType.Char).Value = " ";
                sqlcmd.Parameters.Add("@PeriodEnd12", SqlDbType.Char).Value = TxtPeriodEnd12.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd12.Text).Replace("/", ""): "0";
                sqlcmd.Parameters.Add("@PeriodClose13", SqlDbType.Char).Value = " ";
                sqlcmd.Parameters.Add("@PeriodEnd13", SqlDbType.Char).Value = TxtPeriodEnd13.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd13.Text).Replace("/", ""): "0"; 
                sqlcmd.Parameters.Add("@LstChgUser", SqlDbType.Char).Value = _UserInfo.UData.UserId;
                sqlcmd.Parameters.Add("@LstChgDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.ToShortDateString();
            }
            else
            {

                //修改
                Ssql = @"UPDATE GLAcctPeriod SET PeriodBegin01=@PeriodBegin01,
                       PeriodEnd01=@PeriodEnd01,PeriodEnd02=@PeriodEnd02,PeriodEnd03=@PeriodEnd03,
                       PeriodEnd04=@PeriodEnd04,PeriodEnd05=@PeriodEnd05,PeriodEnd06=@PeriodEnd06,
                       PeriodEnd07=@PeriodEnd07,PeriodEnd08=@PeriodEnd08,PeriodEnd09=@PeriodEnd09,
                       PeriodEnd10=@PeriodEnd10,PeriodEnd11=@PeriodEnd11,PeriodEnd12=@PeriodEnd12,
                       PeriodEnd13=@PeriodEnd13,LstChgUser=@LstChgUser,LstChgDateTime=@LstChgDateTime 
                       Where AcctYear=@AcctYear ";

                sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = DrpCompany.SelectValue;
                sqlcmd.Parameters.Add("@AcctYear", SqlDbType.Decimal).Value = decimal.Parse(TxtAcctYear.Text) + 1911;              
                sqlcmd.Parameters.Add("@PeriodBegin01", SqlDbType.Char).Value = TxtPeriodBegin01.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodBegin01.Text).Replace("/", ""):"0";
                sqlcmd.Parameters.Add("@PeriodEnd01", SqlDbType.Char).Value = TxtPeriodEnd01.Text != ""? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd01.Text).Replace("/", ""):"0";              
                sqlcmd.Parameters.Add("@PeriodEnd02", SqlDbType.Char).Value = TxtPeriodEnd02.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd02.Text).Replace("/", ""):"0";              
                sqlcmd.Parameters.Add("@PeriodEnd03", SqlDbType.Char).Value = TxtPeriodEnd03.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd03.Text).Replace("/", ""):"0";           
                sqlcmd.Parameters.Add("@PeriodEnd04", SqlDbType.Char).Value = TxtPeriodEnd04.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd04.Text).Replace("/", ""):"0";        
                sqlcmd.Parameters.Add("@PeriodEnd05", SqlDbType.Char).Value = TxtPeriodEnd05.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd05.Text).Replace("/", ""):"0";            
                sqlcmd.Parameters.Add("@PeriodEnd06", SqlDbType.Char).Value = TxtPeriodEnd06.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd06.Text).Replace("/", ""):"0";               
                sqlcmd.Parameters.Add("@PeriodEnd07", SqlDbType.Char).Value = TxtPeriodEnd07.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd07.Text).Replace("/", ""):"0";              
                sqlcmd.Parameters.Add("@PeriodEnd08", SqlDbType.Char).Value = TxtPeriodEnd08.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd08.Text).Replace("/", ""):"0";                
                sqlcmd.Parameters.Add("@PeriodEnd09", SqlDbType.Char).Value = TxtPeriodEnd09.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd09.Text).Replace("/", ""):"0";        
                sqlcmd.Parameters.Add("@PeriodEnd10", SqlDbType.Char).Value = TxtPeriodEnd10.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd10.Text).Replace("/", ""):"0";             
                sqlcmd.Parameters.Add("@PeriodEnd11", SqlDbType.Char).Value = TxtPeriodEnd11.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd11.Text).Replace("/", ""):"0";            
                sqlcmd.Parameters.Add("@PeriodEnd12", SqlDbType.Char).Value = TxtPeriodEnd12.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd12.Text).Replace("/", ""):"0";       
                sqlcmd.Parameters.Add("@PeriodEnd13", SqlDbType.Char).Value = TxtPeriodEnd13.Text!="" ? _UserInfo.SysSet.FormatADDate(TxtPeriodEnd13.Text).Replace("/", ""):"0";                            
                sqlcmd.Parameters.Add("@LstChgUser", SqlDbType.Char).Value = "";
                sqlcmd.Parameters.Add("@LstChgDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.ToShortDateString();
            


               
            }

            _MyDBM.ExecuteCommand(Ssql, sqlcmd.Parameters, CommandType.Text);
        
        }
           
    

    //protected void btnEmptyNew_Click(object sender, EventArgs e)
    //{
    //    string temId = hid_IsInsertExit.Value.Replace("_", "$") + "$tbAddNew0";
    //    string ddlId = hid_IsInsertExit.Value.Replace("_", "$") + "$ddl0";
    //    string detailId = "DetailsView1$";
    //    string Csstr = "";
    //    string Esstr = "";
    //    ////新增資料
    //    //if (String.IsNullOrEmpty(Request.Form[temId + "4"].ToString()) || String.IsNullOrEmpty(Request.Form[temId + "5"].ToString()) || String.IsNullOrEmpty(Request.Form[temId + "3"].ToString()))
    //    //{
    //    //    return;
    //    //}
    //    if (hid_IsInsertExit.Value != "")
    //    {
    //        if (Request.Form[temId + "2"].ToString().Length > 7)
    //        {
    //            lbl_Msg2.Text = "新增失敗!!  原因: 金額超過7個字";

    //        }
    //        //if (Request.Form[detailId + "DDL01"].ToString() != "請選擇" && Request.Form[detailId + "DDL02"].ToString() != "請選擇")
    //        //{
    //        //    Csstr = Request.Form[detailId + "DDL01"].Substring(0, Request.Form[detailId + "DDL01"].IndexOf(" "));
    //        //    Esstr = Request.Form[detailId + "DDL02"].Substring(0, Request.Form[detailId + "DDL02"].IndexOf(" "));
    //        //}
    //        if (!ValidateData(sCompany, sEmployeeId, Request.Form[ddlId + "1"].ToString().Trim()))
    //        {
    //            lbl_Msg2.Text = "新增失敗!!  原因: 資料重覆";

    //        }
    //        //if (!ValidateSS(Request.Form[ddlId + "1"].ToString(), Request.Form[ddlId + "2"].ToString(), Request.Form[ddlId + "3"].ToString()))
    //        //{
    //        //    lbl_Msg2.Text = "新增失敗!!  原因: 調薪項目、原因或薪資別與薪資項目不符";
    //        //}

    //        //新增
    //        if (lbl_Msg2.Text == "")
    //        {
    //            SDS_GridView.InsertParameters.Clear();
    //            SDS_GridView.InsertParameters.Add("Company", sCompany);
    //            SDS_GridView.InsertParameters.Add("EmployeeId", sEmployeeId);
    //            SDS_GridView.InsertParameters.Add("SalaryItem", Request.Form[ddlId + "1"].ToString());
    //            SDS_GridView.InsertParameters.Add("Amount", py.EnCodeAmount(decimal.Parse(Request.Form[temId + "2"].ToString())));
    //            //Company	公司編號
    //            //EmployeeId	員工編號
    //            //DepositBank	存入銀行 
    //            //DepositBankAccount	存入帳號
    //            //Period2DepositDate	下期存入日期
    //            //Period1DepositDate	上期存入日期

    //            #region 開始異動前,先寫入LOG
    //            //TableName	異動資料表	varchar	60
    //            //TrxType	異動類別(A/U/D)	char	1
    //            //ChangItem	異動項目(欄位)	varchar	255
    //            //SQLcommand	異動項目(異動值/指令)	varchar	2000
    //            //ChgStartDateTime	異動開始時間	smalldatetime	
    //            //ChgStopDateTime	異動結束時間	smalldatetime	
    //            //ChgUser	使用者代號	nvarchar	32
    //            DateTime StartDateTime = DateTime.Now;
    //            MyCmd.Parameters.Clear();
    //            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "Payroll_Master_Detail";
    //            MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "A";
    //            MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
    //            MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
    //            MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
    //            //此時不設定異動結束時間
    //            //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
    //            MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
    //            _MyDBM.DataChgLog(MyCmd.Parameters);
    //            #endregion

    //            int i = 0;
    //            try
    //            {
    //                i = SDS_GridView.Insert();
    //            }
    //            catch (Exception ex)
    //            {
    //                lbl_Msg2.Text = ex.Message;
    //            }
    //            if (i == 1)
    //            {
    //                lbl_Msg2.Text = i.ToString() + " 個資料列 " + "新增成功!!";
    //                //DetailsView1.InsertItem(true);
    //            }
    //            else
    //            {
    //                lbl_Msg2.Text = "新增失敗!!" + lbl_Msg2.Text;
    //            }

    //            #region 完成異動後,更新LOG資訊
    //            MyCmd.Parameters["@SQLcommand"].Value = (i == 1) ? "Success" : lbl_Msg2.Text;
    //            MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
    //            _MyDBM.DataChgLog(MyCmd.Parameters);
    //            #endregion

    //            BindData();
    //            //showPanel();
    //        }
    //        else
    //        {
    //            BindData();
    //        }
    //    }
    //    //hid_IsInsertExit.Value = "";
    //} 



    /// <summary>
    /// 檢查欄位資料是否正確
    /// </summary>
    /// <returns></returns>
    private bool CheckData()
    {
        string strMessage = "";
        string strsubmsg = "";
        LabMessage.Text="";
      
        bool bolcheck=true;
        int ierro = 0;
        int iyear=0;
        

        //檢查公司
        if (DrpCompany.SelectValue == "")
        {
            strMessage += "．必須選擇公司資料<br>";
            ierro++;
        }
        //年度
        if (TxtAcctYear.Text.Trim() == "")
        {
            strMessage += "．必須填寫年度資料<br>";
            ierro++;         
        }
        //檢查數字
        else if (TxtAcctYear.Text.Trim().Length > 3)
        {
            strMessage += "．年度不得超過3碼<br>";
            ierro++;        
        }
        else if (int.TryParse(TxtAcctYear.Text.Trim(), out iyear) == false)
        {
            strMessage += "．年度必須為數字<br>";
            ierro++;
        }  //檢查長度 1~999
        else if (iyear <= 0 || iyear>999)
        {
            strMessage += "．年度必須介於1至999<br>";
            ierro++;        
        }

       

        
        //期間1
        if (TxtPeriodBegin01.Text.Trim() != "" && TxtPeriodEnd01.Text.Trim() != "")
        {
            if (checkDateRange(TxtPeriodBegin01.Text.Trim(), TxtPeriodEnd01.Text.Trim(), out strsubmsg) == false)
            {
                strMessage = strMessage + "．期間1" + strsubmsg;
                ierro++;
            }
        }
        else
        {
            strMessage += "．必須填寫期間1資料<br>";
            ierro++;        
        }
        //期間2

        if (LabPeriodBegin2.Text.Trim() != "" && TxtPeriodEnd02.Text.Trim() != "")
        {
            if (checkDateRange(LabPeriodBegin2.Text.Trim(), TxtPeriodEnd02.Text.Trim(), out strsubmsg) == false)
            {
                strMessage = strMessage + "．期間2" + strsubmsg;
                ierro++;
            }
        }
        else
        {
            strMessage += "．必須填寫期間2資料<br>";
            ierro++;
        }
        //期間3
        if (LabPeriodBegin3.Text.Trim() != "" && TxtPeriodEnd03.Text.Trim() != "")
        {
            if (checkDateRange(LabPeriodBegin3.Text.Trim(), TxtPeriodEnd03.Text.Trim(), out strsubmsg) == false)
            {
                strMessage = strMessage + "．期間3" + strsubmsg;
                ierro++;
            }
        }
        else
        {
            strMessage += "．必須填寫期間3資料<br>";
            ierro++;
        }

        //期間4

        if (LabPeriodBegin4.Text.Trim() != "" && TxtPeriodEnd04.Text.Trim() != "")
        {
            if (checkDateRange(LabPeriodBegin4.Text.Trim(), TxtPeriodEnd04.Text.Trim(), out strsubmsg) == false)
            {
                strMessage = strMessage + "．期間4" + strsubmsg;
                ierro++;
            }
        }
        else
        {
            strMessage += "．必須填寫期間4資料<br>";
            ierro++;
        }
        
        //期間5
        if (LabPeriodBegin5.Text.Trim() != "" && TxtPeriodEnd05.Text.Trim() != "")
        {
            if (checkDateRange(LabPeriodBegin5.Text.Trim(), TxtPeriodEnd05.Text.Trim(), out strsubmsg) == false)
            {
                strMessage = strMessage + "．期間5" + strsubmsg;
                ierro++;
            }
        }
        else
        {
            strMessage += "．必須填寫期間5資料<br>";
            ierro++;
        }

        //期間6
        if (LabPeriodBegin6.Text.Trim() != "" && TxtPeriodEnd06.Text.Trim() != "")
        {
            if (checkDateRange(LabPeriodBegin6.Text.Trim(), TxtPeriodEnd06.Text.Trim(), out strsubmsg) == false)
            {
                strMessage = strMessage + "．期間6" + strsubmsg;
                ierro++;
            }
        }
        else
        {
            strMessage += "．必須填寫期間6資料<br>";
            ierro++;
        }
        
        //期間7
        if (LabPeriodBegin7.Text.Trim() != "" && TxtPeriodEnd07.Text.Trim() != "")
        {
            if (checkDateRange(LabPeriodBegin7.Text.Trim(), TxtPeriodEnd07.Text.Trim(), out strsubmsg) == false)
            {
                strMessage = strMessage + "．期間7" + strsubmsg;
                ierro++;
            }
        }
        else
        {
            strMessage += "．必須填寫期間7資料<br>";
            ierro++;
        }
        //期間8
        if (LabPeriodBegin8.Text.Trim() != "" && TxtPeriodEnd08.Text.Trim() != "")
        {
            if (checkDateRange(LabPeriodBegin8.Text.Trim(), TxtPeriodEnd08.Text.Trim(), out strsubmsg) == false)
            {
                strMessage = strMessage + "．期間8" + strsubmsg;
                ierro++;
            }
        }
        else
        {
            strMessage += "．必須填寫期間8資料<br>";
            ierro++;
        }
        
        //期間9
        if (LabPeriodBegin9.Text.Trim() != "" && TxtPeriodEnd09.Text.Trim() != "")
        {
            if (checkDateRange(LabPeriodBegin9.Text.Trim(), TxtPeriodEnd09.Text.Trim(), out strsubmsg) == false)
            {
                strMessage = strMessage + "．期間9" + strsubmsg;
                ierro++;
            }
        }
        else
        {
            strMessage += "．必須填寫期間9資料<br>";
            ierro++;
        }

        //期間10
        if (LabPeriodBegin10.Text.Trim() != "" && TxtPeriodEnd10.Text.Trim() != "")
        {
            if (checkDateRange(LabPeriodBegin10.Text.Trim(), TxtPeriodEnd10.Text.Trim(), out strsubmsg) == false)
            {
                strMessage = strMessage + "．期間10" + strsubmsg;
                ierro++;
            }
        }
        else
        {
            strMessage += "．必須填寫期間10資料<br>";
            ierro++;
        }
        //期間11
        if (LabPeriodBegin11.Text.Trim() != "" && TxtPeriodEnd11.Text.Trim() != "")
        {
            if (checkDateRange(LabPeriodBegin11.Text.Trim(), TxtPeriodEnd11.Text.Trim(), out strsubmsg) == false)
            {
                strMessage = strMessage + "．期間11" + strsubmsg;
                ierro++;
            }
        }
        else
        {
            strMessage += "．必須填寫期間11資料<br>";
            ierro++;
        }

        //期間12
        if (LabPeriodBegin12.Text.Trim() != "" && TxtPeriodEnd12.Text.Trim() != "")
        {
            if (checkDateRange(LabPeriodBegin12.Text.Trim(), TxtPeriodEnd12.Text.Trim(), out strsubmsg) == false)
            {
                strMessage = strMessage + "．期間12" + strsubmsg;
                ierro++;
            }
        }
        else
        {
            strMessage += "．必須填寫期間12資料<br>";
            ierro++;
        }

        //期間13

        if (LabPeriodBegin13.Text.Trim() != "" && TxtPeriodEnd13.Text.Trim() != "")
        {
            if (checkDateRange(LabPeriodBegin13.Text.Trim(), TxtPeriodEnd13.Text.Trim(), out strsubmsg) == false)
            {
                strMessage = strMessage + "．期間13" + strsubmsg;
                ierro++;
            }
        }
        


    
      if (ierro > 0)
      {
          bolcheck = false;
      }


      LabMessage.Text = strMessage;

      return bolcheck;
    }


    private bool checkDateRange (string strStartdate ,string EndDate, out string strMessage)
    {

        
        bool checkresult = true;
        string strRmsg = "";
        DateTime DateWDate  ;
        DateTime DateWEDate  ;
        //檢查日期格式
        DateWDate = _UserInfo.SysSet.GetADDate(strStartdate);

        if (DateWDate == Convert.ToDateTime("1912/01/01"))
        {
            checkresult = false;
            strRmsg += "日期格式錯誤<br>";
            strMessage = strRmsg;
            return checkresult;
            
        }
        DateWEDate = _UserInfo.SysSet.GetADDate(EndDate);
        if (DateWEDate == Convert.ToDateTime("1912/01/01"))
        {
            checkresult = false;
            strRmsg += "日期格式錯誤<br>";
            strMessage = strRmsg;
            return checkresult;
        }

        //檢查日期間隔是否正常

        if (DateTime.Compare(DateWDate, DateWEDate) > 0)
        {
            checkresult = false;
            strRmsg += "日期間隔錯誤<br>";
            strMessage = strRmsg;
            return checkresult;        
        }
        

        strMessage = strRmsg;
        return checkresult;
    }



    private bool ValidateData(string strCompany, string strAcctYear)
    {
        //判斷資料是否重覆
        Ssql = "SELECT * FROM GLAcctPeriod WHERE Company = '" + strCompany + "' And AcctYear='" + strAcctYear + "'";

        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);

        if (tb.Rows.Count > 0)
        {
            return false;
        }
        return true;
    }






    protected void btnSaveGo_Click(object sender, ImageClickEventArgs e)
    {
        //只有新增
        if (blInsertMod == true)
        {
            if (CheckData()==true)
            {
                //檢查資料正確姓
                strCompany = DrpCompany.SelectValue;
                strAcctYear = TxtAcctYear.Text;
                //檢查是否有資料
                if (ValidateData(strCompany, strAcctYear) == true)
                {
                    UpdateData(true);
                }
                else
                {
                    //秀錯誤訊息
                    JsUtility.ClientMsgBoxAjax("會計期間設定已存在！", UpdatePanel2, "");
                    //JsUtility.ClientMsgBox("會計期間設定已存在！", this.Page, "");
                }
            }
            strAcctYear = "";
            strCompany = "";

           
        }


    }



    protected void btnSaveExit_Click(object sender, ImageClickEventArgs e)
    {
        //新增與修改

        string strScript = "";

       // strScript = "<script language=javascript>";
        strScript = "window.opener.location='GLA0360.aspx?Company=" + strCompany + "';";
        strScript+="window.close();" ;



        if (blInsertMod == true)
        {
           
            //檢查是否有資料
            if (CheckData() == true)
            {
                strCompany = DrpCompany.SelectValue;
                strAcctYear = (int.Parse(TxtAcctYear.Text)+1911).ToString();

                if (ValidateData(strCompany, strAcctYear) == true)
                {
                    UpdateData(true);
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "msg", strScript,true );
                 
                   // Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", strScript);

                    JsUtility.ClientMsgBoxAjax("存檔成功！",UpdatePanel2,"");
                    //JsUtility.ClientMsgBox("存檔成功！", this.Page, "");
                    strAcctYear = "";
                    strCompany = "";
                }

                else
                {
                    //秀錯誤訊息
                    JsUtility.ClientMsgBoxAjax("會計期間設定已存在！", UpdatePanel2, "");
                    //JsUtility.ClientMsgBox("會計期間設定已存在！", this.Page, "");

                }
            }
        }
        else
        {
            //檢查是否有資料
            if (CheckData() == true)
            {
                UpdateData(false);
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "msg", strScript, true);
                //Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", strScript);
            }
          
        }

    }
    protected void TxtPeriodEnd01_TextChanged(object sender, EventArgs e)
    {
        string Wdate = "";
        if (TxtPeriodEnd01.Text.Trim() != "")
        {
            if (CheckDatefromat(TxtPeriodEnd01.Text.Trim(), out Wdate) == true)
            {
                LabPeriodBegin2.Text = _UserInfo.SysSet.FormatDate(NextSDate(TxtPeriodEnd01.Text.Trim()));
            }
            else
            {
                LabMessage.Text = "日期格式錯誤";            
            }
        }

    }


    private bool CheckDatefromat(string RDate, out string WestDate)
    {
       string strWDate=_UserInfo.SysSet.ToADDate(RDate);

       if (strWDate == "1912/01/01")
       {
           WestDate = "";
           return false;       
       }
        else
       {
           WestDate = strWDate;
           return true;
       }
    }

    private string NextSDate(string strPerEndDate )
    {
        string strReturnDate="";
        string strWPreDate = "";
        if (CheckDatefromat(strPerEndDate, out strWPreDate) == true)
        {
           strReturnDate= DateTime.Parse(strWPreDate).AddDays(1).ToString("yyyy/MM/dd");       
        }
        
        return strReturnDate;
    }

    protected void TxtPeriodEnd02_TextChanged(object sender, EventArgs e)
    {
        string Wdate = "";
        if (TxtPeriodEnd02.Text.Trim() != "")
        {            
            if (CheckDatefromat(TxtPeriodEnd02.Text.Trim(), out Wdate) == true)
            {
             LabPeriodBegin3.Text = _UserInfo.SysSet.FormatDate(NextSDate(TxtPeriodEnd02.Text.Trim()));
            }
            else
            {
                LabMessage.Text = "日期格式錯誤";
            }
        }

    }
    protected void TxtPeriodEnd03_TextChanged(object sender, EventArgs e)
    {
        string Wdate = "";
        if (TxtPeriodEnd03.Text.Trim() != "")
        {
          if (CheckDatefromat(TxtPeriodEnd03.Text.Trim(), out Wdate) == true)
          {
            LabPeriodBegin4.Text = _UserInfo.SysSet.FormatDate(NextSDate(TxtPeriodEnd03.Text.Trim()));
          }
             else
           {
                 LabMessage.Text = "日期格式錯誤";
           }
        }
    }


    protected void TxtPeriodEnd04_TextChanged(object sender, EventArgs e)
    {
        string Wdate = "";
        if (TxtPeriodEnd04.Text.Trim() != "")
        {
            if (CheckDatefromat(TxtPeriodEnd04.Text.Trim(), out Wdate) == true)
            {
                LabPeriodBegin5.Text = _UserInfo.SysSet.FormatDate(NextSDate(TxtPeriodEnd04.Text.Trim()));
            }
            else
            {
                LabMessage.Text = "日期格式錯誤";
            }

        }
    }


    protected void TxtPeriodEnd05_TextChanged(object sender, EventArgs e)
    {
        string Wdate = "";
        if (TxtPeriodEnd05.Text.Trim() != "")
        {
            if (CheckDatefromat(TxtPeriodEnd05.Text.Trim(), out Wdate) == true)
            {
                LabPeriodBegin6.Text = _UserInfo.SysSet.FormatDate(NextSDate(TxtPeriodEnd05.Text.Trim()));
            }
            else
            {
                LabMessage.Text = "日期格式錯誤";
            }
        }

    }
    protected void TxtPeriodEnd06_TextChanged(object sender, EventArgs e)
    {
        string Wdate = "";
        if (TxtPeriodEnd06.Text.Trim() != "")
        {
            if (CheckDatefromat(TxtPeriodEnd06.Text.Trim(), out Wdate) == true)
            {
                LabPeriodBegin7.Text = _UserInfo.SysSet.FormatDate(NextSDate(TxtPeriodEnd06.Text.Trim()));
            }
            else
            {
                LabMessage.Text = "日期格式錯誤";
            }
        }
    }
    protected void TxtPeriodEnd07_TextChanged(object sender, EventArgs e)
    {
        string Wdate = "";
        if (TxtPeriodEnd07.Text.Trim() != "")
        {
           if (CheckDatefromat(TxtPeriodEnd07.Text.Trim(), out Wdate) == true)
            {
             LabPeriodBegin8.Text = _UserInfo.SysSet.FormatDate(NextSDate(TxtPeriodEnd07.Text.Trim()));
            }
           else
            {
              LabMessage.Text = "日期格式錯誤";
            }
        }
    }
    protected void TxtPeriodEnd08_TextChanged(object sender, EventArgs e)
    {
        string Wdate = "";
        if (TxtPeriodEnd08.Text.Trim() != "")
        {
            if (CheckDatefromat(TxtPeriodEnd08.Text.Trim(), out Wdate) == true)
            {
                LabPeriodBegin9.Text = _UserInfo.SysSet.FormatDate(NextSDate(TxtPeriodEnd08.Text.Trim()));
            }
            else
            {
                LabMessage.Text = "日期格式錯誤";
            }

        }

    }
    protected void TxtPeriodEnd09_TextChanged(object sender, EventArgs e)
    {
        string Wdate = "";
        if (TxtPeriodEnd09.Text.Trim() != "")
        {
            if (CheckDatefromat(TxtPeriodEnd09.Text.Trim(), out Wdate) == true)
            {
                LabPeriodBegin10.Text = _UserInfo.SysSet.FormatDate(NextSDate(TxtPeriodEnd09.Text.Trim()));
            }
            else
            {
                LabMessage.Text = "日期格式錯誤";
            }
        }
    }
    protected void TxtPeriodEnd10_TextChanged(object sender, EventArgs e)
    {
        string Wdate = "";
        if (TxtPeriodEnd10.Text.Trim() != "")
        {
            if (CheckDatefromat(TxtPeriodEnd10.Text.Trim(), out Wdate) == true)
            {
                LabPeriodBegin11.Text = _UserInfo.SysSet.FormatDate(NextSDate(TxtPeriodEnd10.Text.Trim()));
            }
            else
            {
                LabMessage.Text = "日期格式錯誤";
            }
        }

    }
    protected void TxtPeriodEnd11_TextChanged(object sender, EventArgs e)
    {
        string Wdate = "";
        if (TxtPeriodEnd11.Text.Trim() != "")
        {
            if (CheckDatefromat(TxtPeriodEnd11.Text.Trim(), out Wdate) == true)
            {
                LabPeriodBegin12.Text = _UserInfo.SysSet.FormatDate(NextSDate(TxtPeriodEnd11.Text.Trim()));
            }
            else
            {
                LabMessage.Text = "日期格式錯誤";
            }
        }

    }
    protected void TxtPeriodEnd12_TextChanged(object sender, EventArgs e)
    {
        string Wdate = "";
        if (TxtPeriodEnd12.Text.Trim() != "")
        {
            if (CheckDatefromat(TxtPeriodEnd12.Text.Trim(), out Wdate) == true)
            {
                LabPeriodBegin13.Text = _UserInfo.SysSet.FormatDate(NextSDate(TxtPeriodEnd12.Text.Trim()));
            }
            else
            {
                LabMessage.Text = "日期格式錯誤";
            }
        }

    }
}