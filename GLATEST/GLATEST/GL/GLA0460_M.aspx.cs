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

public partial class GLA0460_M : System.Web.UI.Page
{
    string Ssql = "";
    string Ssql1 = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLA0460";
    DBManger _MyDBM;    
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    string strCompany = "";
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

        txtLastPostDate.Attributes.Add("ReadOnly", "ReadOnly");
        txtCloseYM.Attributes.Add("ReadOnly", "ReadOnly");
        txtCloseYYYY.Attributes.Add("ReadOnly", "ReadOnly");             
       
        
            btnSaveGo.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='GO';");
            if (Request["Company"] != null )
            {
                strCompany = Request["Company"].Trim();
               
                btnSaveGo.Visible = false;
                btnSaveGo.Enabled = false;
            }

            else
            {
                strCompany = "";            
                blInsertMod = true;
            }
                
      

        btnSaveExit.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='EXIT';");
        btnCancel.Attributes.Add("onclick", "javascript:window.close();");

        if (Request["Kind"] != null)
        {
            blReadOnly = Request["Kind"].Equals("Query");
        }

        ScriptManager.RegisterStartupScript(ScriptManager1, this.GetType(), "", @"JQ();", true);
        txtLastPostDate.CssClass = "JQCalendar";
        if (!Page.IsPostBack)
        {          
           
            BindEditData(strCompany );

            if (blInsertMod==false)
            {            
             changefunction("E");
            }



            if (blReadOnly == true)
            {
                changefunction("R");

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
            ChkApprovalCode.Enabled = false;
            txtAccuPLAcctNo.Attributes.Add("ReadOnly","ReadOnly");
            txtPeriodPLAcctNo.Attributes.Add("ReadOnly", "ReadOnly");
            txtCashCodefrom.Attributes.Add("ReadOnly", "ReadOnly");
            txtCashCodeto.Attributes.Add("ReadOnly", "ReadOnly");
            DrpCalendarType.Enabled=false;
            DrpAcctPeriod.Enabled=false;
            DrpDateFormat.Enabled=false;
            txtLastPostDate.Attributes.Add("ReadOnly", "ReadOnly");           
            txtCloseYM.Attributes.Add("ReadOnly", "ReadOnly");            
            txtCloseYYYY.Attributes.Add("ReadOnly", "ReadOnly");


          
        }

        //編輯
        else if(Mode=="E")
        {
            DrpCompany.Enabled = false;
           
        }         
    
    }
       

   

    /// <summary>
    /// 讀取修改資料
    /// </summary>
    /// <param name="strCompany"></param>
    /// <param name="strAccTNo"></param>
    private void BindEditData(string strCompany)

    {
        _MyDBM = new DBManger();
        _MyDBM.New();

        SqlCommand sqlcmd = new SqlCommand();
        DataTable DT = new DataTable();
        string strROCYear = "",strCloseY="";

        Ssql1 = "SELECT * FROM GLParm WHERE Company=@Company";

        sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = strCompany;
        
         DT = _MyDBM.ExecuteDataTable(Ssql1,sqlcmd.Parameters,CommandType.Text);

        if (DT.Rows.Count > 0)
        {
            DrpCompany.SelectValue = DT.Rows[0]["Company"].ToString();
            ChkApprovalCode.Checked = DT.Rows[0]["ApprovalCode"].ToString()=="Y" ? true:false ;
            txtAccuPLAcctNo.Text = DT.Rows[0]["AccuPLAcctNo"].ToString();
            txtPeriodPLAcctNo.Text = DT.Rows[0]["PeriodPLAcctNo"].ToString();
            txtCashCodefrom.Text = DT.Rows[0]["CashCodefrom"].ToString();
            txtCashCodeto.Text = DT.Rows[0]["CashCodeto"].ToString();
            DrpCalendarType.SelectedValue = DT.Rows[0]["CalendarType"].ToString();
            DrpAcctPeriod.SelectedValue = DT.Rows[0]["AcctPeriod"].ToString();
            DrpDateFormat.SelectedValue = DT.Rows[0]["DateFormat"].ToString();
            txtLastPostDate.Text = DT.Rows[0]["LastPostDate"].ToString()!="0" ? _UserInfo.SysSet.FormatDate(DT.Rows[0]["LastPostDate"].ToString()) : "";

            if (DT.Rows[0]["CloseYM"].ToString() != "0" && DT.Rows[0]["CloseYM"].ToString() != "")
            {
                 strROCYear = DT.Rows[0]["CloseYM"].ToString();
                int year= int.Parse(strROCYear.Substring(0, 4)) - 1911;
                strROCYear = year.ToString() + "/" + strROCYear.Substring(4, 2);            
            }
            txtCloseYM.Text = strROCYear;

            if (DT.Rows[0]["CloseYYYY"].ToString() != "0" && DT.Rows[0]["CloseYYYY"].ToString() != "")
            {
                strCloseY = (int.Parse(DT.Rows[0]["CloseYYYY"].ToString())-1911).ToString();
               
            }
            txtCloseYYYY.Text = strCloseY;
           

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
                Ssql = @"INSERT INTO GLParm (Company,ApprovalCode,AccuPLAcctNo,PeriodPLAcctNo,CashCodefrom,CashCodeto,
                         CalendarType,AcctPeriod,DateFormat,LastPostDate,CloseYYYY,CloseYM,PostCtlCode,
                         JournalnoType,PostCtlType,LstChgUser,LstChgDateTime) VALUES
                         (@Company,@ApprovalCode,@AccuPLAcctNo,@PeriodPLAcctNo,@CashCodefrom,@CashCodeto,
                         @CalendarType,@AcctPeriod,@DateFormat,@LastPostDate,@CloseYYYY,@CloseYM,@PostCtlCode,
                         @JournalnoType,@PostCtlType,@LstChgUser,@LstChgDateTime ) ";
                               
                //新增
                sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = DrpCompany.SelectValue;
                sqlcmd.Parameters.Add("@ApprovalCode", SqlDbType.Char).Value = ChkApprovalCode.Checked==true ? "Y":"N";
                sqlcmd.Parameters.Add("@AccuPLAcctNo", SqlDbType.Char).Value = txtAccuPLAcctNo.Text;
                sqlcmd.Parameters.Add("@PeriodPLAcctNo", SqlDbType.Char).Value = txtPeriodPLAcctNo.Text;
                sqlcmd.Parameters.Add("@CashCodefrom", SqlDbType.Char).Value = txtCashCodefrom.Text;
                sqlcmd.Parameters.Add("@CashCodeto", SqlDbType.Char).Value = txtCashCodeto.Text;
                sqlcmd.Parameters.Add("@CalendarType", SqlDbType.Char).Value = DrpCalendarType.SelectedValue;
                sqlcmd.Parameters.Add("@AcctPeriod", SqlDbType.Char).Value = DrpAcctPeriod.SelectedValue;
                sqlcmd.Parameters.Add("@DateFormat", SqlDbType.Char).Value = DrpDateFormat.SelectedValue;
                sqlcmd.Parameters.Add("@LastPostDate", SqlDbType.Decimal).Value = 0;
                sqlcmd.Parameters.Add("@CloseYYYY", SqlDbType.Decimal).Value =  0;
                sqlcmd.Parameters.Add("@CloseYM", SqlDbType.Decimal).Value = 0;
                sqlcmd.Parameters.Add("@PostCtlCode", SqlDbType.Char).Value = "1";  
                sqlcmd.Parameters.Add("@JournalnoType", SqlDbType.Char).Value ="1" ;
                sqlcmd.Parameters.Add("@PostCtlType", SqlDbType.Char).Value = "1";  
                                  
                sqlcmd.Parameters.Add("@LstChgUser", SqlDbType.Char).Value = _UserInfo.UData.UserId;
                sqlcmd.Parameters.Add("@LstChgDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.ToShortDateString();
            }
            else
            {

                //修改
                Ssql = @"UPDATE GLParm SET ApprovalCode=@ApprovalCode,AccuPLAcctNo=@AccuPLAcctNo,
                   PeriodPLAcctNo=@PeriodPLAcctNo,CashCodefrom=@CashCodefrom,CashCodeto=@CashCodeto,
                   CalendarType=@CalendarType,AcctPeriod=@AcctPeriod,DateFormat=@DateFormat,                   
                   LstChgUser=@LstChgUser,LstChgDateTime=@LstChgDateTime WHERE company=@Company ";
                
                
                sqlcmd.Parameters.Add("@ApprovalCode", SqlDbType.Char).Value = ChkApprovalCode.Checked == true ? "Y" : "N";
                sqlcmd.Parameters.Add("@AccuPLAcctNo", SqlDbType.Char).Value = txtAccuPLAcctNo.Text;
                sqlcmd.Parameters.Add("@PeriodPLAcctNo", SqlDbType.Char).Value = txtPeriodPLAcctNo.Text;
                sqlcmd.Parameters.Add("@CashCodefrom", SqlDbType.Char).Value = txtCashCodefrom.Text;
                sqlcmd.Parameters.Add("@CashCodeto", SqlDbType.Char).Value = txtCashCodeto.Text;
                sqlcmd.Parameters.Add("@CalendarType", SqlDbType.Char).Value = DrpCalendarType.SelectedValue;
                sqlcmd.Parameters.Add("@AcctPeriod", SqlDbType.Char).Value = DrpAcctPeriod.SelectedValue;
                sqlcmd.Parameters.Add("@DateFormat", SqlDbType.Char).Value = DrpDateFormat.SelectedValue;                            
                sqlcmd.Parameters.Add("@LstChgUser", SqlDbType.Char).Value = _UserInfo.UData.UserId;
                sqlcmd.Parameters.Add("@LstChgDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.ToShortDateString();
                sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = DrpCompany.SelectValue;
               


               
            }

            _MyDBM.ExecuteCommand(Ssql, sqlcmd.Parameters, CommandType.Text);
        
        }
           
    

   
    private bool CheckData()
    {
        string strMessage = "";
        LabMessage.Text = "";
        bool bolcheck=true;

        int ierro = 0;



        //檢查公司
      if (DrpCompany.SelectValue == "")
      {

          strMessage += "．必須選擇公司資料<br>";
          ierro++;

      }

        //累計盈虧科目
      if (txtAccuPLAcctNo.Text.Trim() == "")
      { 
       strMessage += "．必須填寫累計盈虧科目<br>";
          ierro++;      
      }
      else if (txtAccuPLAcctNo.Text.Trim().Length>8)
      {
          strMessage += "．累計盈虧科目必須小於8碼<br>";
          ierro++;     
      }
        
        //本期損益科目

      if (txtPeriodPLAcctNo.Text.Trim() == "")
      {
          strMessage += "．必須填寫本期損益科目<br>";
          ierro++;
      }
      else if (txtPeriodPLAcctNo.Text.Trim().Length > 8)
      {
          strMessage += "．本期損益科目必須小於8碼<br>";
          ierro++;
      }
        //資金代碼來源    
       if (txtCashCodefrom.Text.Trim().Length > 8)
      {
          strMessage += "．資金代碼-來源必須小於8碼<br>";
          ierro++;
      }
        
        //資金代碼用途
       if (txtCashCodeto.Text.Trim().Length > 8)
       {
           strMessage += "．資金代碼-用途必須小於8碼<br>";
           ierro++;
       }
                  
     

      if (ierro > 0)
      {
          bolcheck = false;
      }


      LabMessage.Text = strMessage;

      return bolcheck;
    }


    //設為預設值
    private void Clear()
    {
        DrpCompany.SelectValue = "";
        ChkApprovalCode.Checked = false;
        txtAccuPLAcctNo.Text = "";
        txtPeriodPLAcctNo.Text = "";
        txtCashCodefrom.Text = "";
        txtCashCodeto.Text = "";
        DrpCalendarType.SelectedIndex = 0;
        DrpAcctPeriod.SelectedIndex = 0;
        DrpDateFormat.SelectedIndex = 0;
        txtLastPostDate.Text = "";
        txtCloseYM.Text = "";
        txtCloseYYYY.Text = "";
    
    }

    private bool ValidateData(string strCompany)
    {
        //判斷資料是否重覆
        Ssql = "SELECT * FROM GLParm  WHERE Company = '" + strCompany + "'";

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
           
                //檢查是否有資料
                if (ValidateData(strCompany) == true)
                {
                    UpdateData(true);

                }

                else
                {
                    //秀錯誤訊息
                    JsUtility.ClientMsgBox("內設資料已存在！", this.Page, "");

                }
            }
          
            strCompany = "";
            JsUtility.ClientMsgBox("存檔成功！", this.Page, "");
            Clear();

           
        }


    }



    protected void btnSaveExit_Click(object sender, ImageClickEventArgs e)
    {
        //新增與修改

        string strScript = "";

        strScript = "<script language=javascript>";
        strScript += "window.opener.location='GLA0460.aspx?Company=" + strCompany + "';";
        strScript+="window.close();</script> " ;



        if (blInsertMod == true)
        {
            strCompany = DrpCompany.SelectValue;
          
            //檢查是否有資料
            if (CheckData() == true)
            {
                if (ValidateData(strCompany) == true)
                {
                    UpdateData(true);
                    Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", strScript);
                    //JsUtility.ClientMsgBox("存檔成功！", this.Page, "");
                   
                    strCompany = "";
                }

                else
                {
                    //秀錯誤訊息
                    JsUtility.ClientMsgBox("內設資料已存在！", this.Page, "");

                }
            }
        }
        else
        {
            //檢查是否有資料
            if (CheckData() == true)
            {
                UpdateData(false);
                Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", strScript);
            }
          
        }

    }
}