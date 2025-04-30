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

public partial class GLA0450_M : System.Web.UI.Page
{
    string Ssql = "";
    string Ssql1 = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLA0450";
    DBManger _MyDBM;    
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    string strCompany = "", strSubLedCode = "";
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

       
        
            btnSaveGo.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='GO';");
            if (Request["Company"] != null && Request["SubLedCode"] != null)
            {
                strCompany = Request["Company"].Trim();
                strSubLedCode = Request["SubLedCode"].Trim();
                btnSaveGo.Visible = false;
                btnSaveGo.Enabled = false;
            }

            else
            {
                strCompany = "";
                strSubLedCode = "";
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
           
            BindEditData(strCompany, strSubLedCode);

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
            txtSubLedCode.Attributes.Add("ReadOnly", "ReadOnly");          
            txtDescription.Attributes.Add("ReadOnly", "ReadOnly");                
                
            ChkSubledctl1.Enabled = false;
            ChkSubledctl2.Enabled = false;
            ChkSubledctl3.Enabled = false;
            ChkSubledctl4.Enabled = false;
            ChkSubledctl5.Enabled = false;
             
        }

        //編輯
        else if(Mode=="E")
        {
            DrpCompany.Enabled = false;          
            txtSubLedCode.Attributes.Add("ReadOnly", "ReadOnly");       
        }         
    
    }
       

   

    /// <summary>
    /// 讀取修改資料
    /// </summary>
    /// <param name="strCompany"></param>
    /// <param name="strAccTNo"></param>
    private void BindEditData(string strCompany, string strSubLedCode)

    {
        _MyDBM = new DBManger();
        _MyDBM.New();

        SqlCommand sqlcmd = new SqlCommand();
        DataTable DT = new DataTable();

        Ssql1 = "SELECT * FROM GLSubLedCode WHERE Company=@Company AND SubLedCode=@SubLedCode";

        sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = strCompany;
        sqlcmd.Parameters.Add("@SubLedCode", SqlDbType.Char).Value = strSubLedCode;

         DT = _MyDBM.ExecuteDataTable(Ssql1,sqlcmd.Parameters,CommandType.Text);

        if (DT.Rows.Count > 0)
        {
            DrpCompany.SelectValue = DT.Rows[0]["Company"].ToString();
            txtSubLedCode.Text = DT.Rows[0]["SubLedCode"].ToString();
            txtDescription.Text = DT.Rows[0]["Description"].ToString();
            ChkSubledctl1.Checked = DT.Rows[0]["Subledctl1"].ToString().Trim() == "V" ? true : false;
            ChkSubledctl2.Checked = DT.Rows[0]["Subledctl2"].ToString().Trim() == "V" ? true : false;
            ChkSubledctl3.Checked = DT.Rows[0]["Subledctl3"].ToString().Trim() == "V" ? true : false;
            ChkSubledctl4.Checked = DT.Rows[0]["Subledctl4"].ToString().Trim() == "V" ? true : false;
            ChkSubledctl5.Checked = DT.Rows[0]["Subledctl5"].ToString().Trim() == "V" ? true : false;
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
            string ctrlValue = "";
            CheckBox[] checkarr = new CheckBox[5] { ChkSubledctl1, ChkSubledctl2, ChkSubledctl3, ChkSubledctl4, ChkSubledctl5 };    

            SqlCommand  sqlcmd = new SqlCommand();
           

             foreach (CheckBox chkb in checkarr)
             {
                 if (chkb.Checked)
                 {
                     ctrlValue += "V";
                 }
                 else
                 {
                     ctrlValue += " ";
                 }             
             }



            if (Mode == true)
            {

                //新增
                Ssql = @"INSERT INTO GLSubLedCode (Company,SubLedCode,Description,SubLedCtl,SubLedCtl1,SubLedCtl2
                        ,SubLedCtl3,SubLedCtl4,SubLedCtl5,LstChgUser,LstChgDateTime) VALUES
                        (@Company,@SubLedCode,@Description,@SubLedCtl,@SubLedCtl1,@SubLedCtl2
                        ,@SubLedCtl3,@SubLedCtl4,@SubLedCtl5,@LstChgUser,@LstChgDateTime ) ";
                               
                //新增
                sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = DrpCompany.SelectValue;
                sqlcmd.Parameters.Add("@SubLedCode", SqlDbType.Char).Value = txtSubLedCode.Text;
                sqlcmd.Parameters.Add("@Description", SqlDbType.Char).Value = txtDescription.Text;
                sqlcmd.Parameters.Add("@SubLedCtl", SqlDbType.Char).Value = ctrlValue;
                sqlcmd.Parameters.Add("@SubLedCtl1", SqlDbType.Char).Value = ChkSubledctl1.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@SubLedCtl2", SqlDbType.Char).Value = ChkSubledctl2.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@SubLedCtl3", SqlDbType.Char).Value = ChkSubledctl3.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@SubLedCtl4", SqlDbType.Char).Value = ChkSubledctl4.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@SubLedCtl5", SqlDbType.Char).Value = ChkSubledctl5.Checked ? "V" : " ";               
                sqlcmd.Parameters.Add("@LstChgUser", SqlDbType.Char).Value = _UserInfo.UData.UserId;
                sqlcmd.Parameters.Add("@LstChgDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.ToShortDateString();
            }
            else
            {

                //修改
                Ssql = @"UPDATE GLSubLedCode SET Description=@Description,SubLedCtl=@SubLedCtl,
                       SubLedCtl1=@SubLedCtl1,SubLedCtl2=@SubLedCtl2,SubLedCtl3=@SubLedCtl3,
                       SubLedCtl4=@SubLedCtl4,SubLedCtl5=@SubLedCtl5,LstChgUser=@LstChgUser,
                       LstChgDateTime=@LstChgDateTime WHERE company=@Company AND SubLedCode=@SubLedCode ";

                sqlcmd.Parameters.Add("@Description", SqlDbType.Char).Value = txtDescription.Text;

                sqlcmd.Parameters.Add("@SubLedCtl", SqlDbType.Char).Value = ctrlValue;
                sqlcmd.Parameters.Add("@SubLedCtl1", SqlDbType.Char).Value = ChkSubledctl1.Checked ? "V" : "";
                sqlcmd.Parameters.Add("@SubLedCtl2", SqlDbType.Char).Value = ChkSubledctl2.Checked ? "V" : "";
                sqlcmd.Parameters.Add("@SubLedCtl3", SqlDbType.Char).Value = ChkSubledctl3.Checked ? "V" : "";
                sqlcmd.Parameters.Add("@SubLedCtl4", SqlDbType.Char).Value = ChkSubledctl4.Checked ? "V" : "";
                sqlcmd.Parameters.Add("@SubLedCtl5", SqlDbType.Char).Value = ChkSubledctl5.Checked ? "V" : "";              
                sqlcmd.Parameters.Add("@LstChgUser", SqlDbType.Char).Value = _UserInfo.UData.UserId;
                sqlcmd.Parameters.Add("@LstChgDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.ToShortDateString();
                sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = DrpCompany.SelectValue;
                sqlcmd.Parameters.Add("@SubLedCode", SqlDbType.Char).Value = txtSubLedCode.Text;


               
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

      //分類帳碼
      if (txtSubLedCode.Text.Trim() == "")
      {

          strMessage += "．必須填入分類帳碼<br>";
          ierro++;
      }
      else if(txtSubLedCode.Text.Trim().Length>1)
      {

          strMessage += "．分類帳碼不得超過1碼<br>";
          ierro++;
      }
        
        //說明
      if (txtDescription.Text.Trim().Length > 50)
      {

          strMessage += "．會計科目不得超過50個字<br>";
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
        txtSubLedCode.Text = "";
        txtDescription.Text = "";
        ChkSubledctl1.Checked = false;
        ChkSubledctl2.Checked = false;
        ChkSubledctl3.Checked = false;
        ChkSubledctl4.Checked = false;
        ChkSubledctl5.Checked = false;
    
    
    }

    private bool ValidateData(string strCompany, string strSubLedCode)
    {
        //判斷資料是否重覆
        Ssql = "SELECT * FROM GLSubLedCode WHERE Company = '" + strCompany + "' And SubLedCode='" + strSubLedCode + "'";

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
                strSubLedCode = txtSubLedCode.Text;
                //檢查是否有資料
                if (ValidateData(strCompany, strSubLedCode) == true)
                {
                    UpdateData(true);

                }

                else
                {
                    //秀錯誤訊息
                    JsUtility.ClientMsgBox("分類帳碼資料已存在！", this.Page, "");

                }
            }
            strSubLedCode = "";
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
        strScript += "window.opener.location='GLA0450.aspx?Company=" + strCompany + "';";
        strScript+="window.close();</script> " ;



        if (blInsertMod == true)
        {
            strCompany = DrpCompany.SelectValue;
            strSubLedCode = txtSubLedCode.Text;
            //檢查是否有資料
            if (CheckData() == true)
            {
                if (ValidateData(strCompany, strSubLedCode) == true)
                {
                    UpdateData(true);
                    Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", strScript);
                    //JsUtility.ClientMsgBox("存檔成功！", this.Page, "");
                    strSubLedCode = "";
                    strCompany = "";
                }

                else
                {
                    //秀錯誤訊息
                    JsUtility.ClientMsgBox("分類帳碼資料已存在！", this.Page, "");

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