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

public partial class GLA0430_M : System.Web.UI.Page
{
    string Ssql = "";
    string Ssql1 = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLA0430";
    DBManger _MyDBM;
    int saveon = 0;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    string strCompany="",strOffsetID=""  ;
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
            if (Request["Company"] != null && Request["OffsetID"] != null)
            {
                strCompany = Request["Company"].Trim();
                strOffsetID = Request["OffsetID"].Trim();
                btnSaveGo.Visible = false;
                btnSaveGo.Enabled = false;
            }

            else
            {
                strCompany = "";
                strOffsetID = "";
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
            saveon = 0;
            BindEditData(strCompany, strOffsetID);

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
           
            txtoffsetID.Attributes.Add("ReadOnly", "ReadOnly");
          
            txtDescription.Attributes.Add("ReadOnly", "ReadOnly");
          
                   
            TxtSeqctl1.Attributes.Add("ReadOnly", "ReadOnly");
            TxtSeqctl2.Attributes.Add("ReadOnly", "ReadOnly");
            TxtSeqctl3.Attributes.Add("ReadOnly", "ReadOnly");
            TxtSeqctl4.Attributes.Add("ReadOnly", "ReadOnly");
            TxtSeqctl5.Attributes.Add("ReadOnly", "ReadOnly");           
            ChkSubtotctl1.Enabled = false;
            ChkSubtotctl2.Enabled = false;
            ChkSubtotctl3.Enabled = false;
            ChkSubtotctl4.Enabled = false;
            ChkSubtotctl5.Enabled = false;
             
        }

        //編輯
        else if(Mode=="E")
        {
            DrpCompany.Enabled = false;          
            txtoffsetID.Attributes.Add("ReadOnly", "ReadOnly");       
        }         
    
    }
   
   
  
    

   

    /// <summary>
    /// 讀取修改資料
    /// </summary>
    /// <param name="strCompany"></param>
    /// <param name="strAccTNo"></param>
    private void BindEditData (string strCompany,string strAccTNo)

    {
        _MyDBM = new DBManger();
        _MyDBM.New();

        SqlCommand sqlcmd = new SqlCommand();
        DataTable DT = new DataTable();

        Ssql1 = "SELECT * FROM GLOffsetDef WHERE Company=@Company AND OffsetID=@OffsetID";

        sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = strCompany;
        sqlcmd.Parameters.Add("@OffsetID", SqlDbType.Char).Value = strAccTNo;

         DT = _MyDBM.ExecuteDataTable(Ssql1,sqlcmd.Parameters,CommandType.Text);

        if (DT.Rows.Count > 0)
        {
            DrpCompany.SelectValue = DT.Rows[0]["Company"].ToString();
            txtoffsetID.Text = DT.Rows[0]["OffsetID"].ToString();
            txtDescription.Text = DT.Rows[0]["Description"].ToString();             
            TxtSeqctl1.Text = DT.Rows[0]["Seqctl1"].ToString();
            TxtSeqctl2.Text = DT.Rows[0]["Seqctl2"].ToString();
            TxtSeqctl3.Text = DT.Rows[0]["Seqctl3"].ToString();
            TxtSeqctl4.Text = DT.Rows[0]["Seqctl4"].ToString();
            TxtSeqctl5.Text = DT.Rows[0]["Seqctl5"].ToString();
            ChkSubtotctl1.Checked = DT.Rows[0]["Subtotctl1"].ToString().Trim() == "V" ? true : false;
            ChkSubtotctl2.Checked = DT.Rows[0]["Subtotctl2"].ToString().Trim() == "V" ? true : false;
            ChkSubtotctl3.Checked = DT.Rows[0]["Subtotctl3"].ToString().Trim() == "V" ? true : false;
            ChkSubtotctl4.Checked = DT.Rows[0]["Subtotctl4"].ToString().Trim() == "V" ? true : false;
            ChkSubtotctl5.Checked = DT.Rows[0]["Subtotctl5"].ToString().Trim() == "V" ? true : false;   

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
            string seqValue="";
            string ctrlValue = "";
            CheckBox[] checkarr = new CheckBox[5] { ChkSubtotctl1, ChkSubtotctl2, ChkSubtotctl3, ChkSubtotctl4, ChkSubtotctl5 };    

            SqlCommand  sqlcmd = new SqlCommand();
            seqValue = TxtSeqctl1.Text + TxtSeqctl2.Text + TxtSeqctl3.Text + TxtSeqctl4.Text + TxtSeqctl5.Text;

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
                Ssql = @"INSERT INTO GLOffsetDef ( Company,OffsetID,Description,Seqctl,Seqctl1,Seqctl2,Seqctl3,Seqctl4,Seqctl5,
SubTotCtl,SubTotCtl1,SubTotCtl2,SubTotCtl3,SubTotCtl4,SubTotCtl5,LstChgUser,LstChgDateTime)
VALUES(@Company,@OffsetID,@Description,@Seqctl,@Seqctl1,@Seqctl2,@Seqctl3,@Seqctl4,@Seqctl5,
@SubTotCtl,@SubTotCtl1,@SubTotCtl2,@SubTotCtl3,@SubTotCtl4,@SubTotCtl5,@LstChgUser,@LstChgDateTime) ";

               
                //新增
                sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = DrpCompany.SelectValue;
                sqlcmd.Parameters.Add("@OffsetID", SqlDbType.Char).Value = txtoffsetID.Text;
                sqlcmd.Parameters.Add("@Description", SqlDbType.Char).Value = txtDescription.Text;
                sqlcmd.Parameters.Add("@Seqctl", SqlDbType.Decimal).Value = seqValue != "" ? decimal.Parse(seqValue) : 0;
                sqlcmd.Parameters.Add("@Seqctl1", SqlDbType.Decimal).Value =TxtSeqctl1.Text.Trim()!=""? decimal.Parse(TxtSeqctl1.Text):0;
                sqlcmd.Parameters.Add("@Seqctl2", SqlDbType.Decimal).Value =TxtSeqctl2.Text.Trim()!=""? decimal.Parse(TxtSeqctl2.Text):0;
                sqlcmd.Parameters.Add("@Seqctl3", SqlDbType.Decimal).Value =TxtSeqctl3.Text.Trim()!=""? decimal.Parse(TxtSeqctl3.Text):0;
                sqlcmd.Parameters.Add("@Seqctl4", SqlDbType.Decimal).Value =TxtSeqctl4.Text.Trim()!=""? decimal.Parse(TxtSeqctl4.Text):0;
                sqlcmd.Parameters.Add("@Seqctl5", SqlDbType.Decimal).Value =TxtSeqctl5.Text.Trim()!=""? decimal.Parse(TxtSeqctl5.Text):0;
                sqlcmd.Parameters.Add("@Subtotctl", SqlDbType.Char).Value = ctrlValue.Trim();
                sqlcmd.Parameters.Add("@Subtotctl1", SqlDbType.Char).Value = ChkSubtotctl1.Checked? "V":" ";
                sqlcmd.Parameters.Add("@Subtotctl2", SqlDbType.Char).Value = ChkSubtotctl2.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@Subtotctl3", SqlDbType.Char).Value = ChkSubtotctl3.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@Subtotctl4", SqlDbType.Char).Value = ChkSubtotctl4.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@Subtotctl5", SqlDbType.Char).Value = ChkSubtotctl5.Checked ? "V" : " ";               
                sqlcmd.Parameters.Add("@LstChgUser", SqlDbType.Char).Value = _UserInfo.UData.UserId;
                sqlcmd.Parameters.Add("@LstChgDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.ToShortDateString();
            }
            else
            {

                //修改
                Ssql = @" UPDATE GLOffsetDef SET Description=@Description,Seqctl=@seqctl,Seqctl1=@seqctl1,Seqctl2=@seqctl2,Seqctl3=@seqctl3
                 ,Seqctl4=@seqctl4,Seqctl5=@seqctl5,Subtotctl=@subtotctl,Subtotctl1=@subtotctl1,Subtotctl2=@subtotctl2,Subtotctl3=@subtotctl3
                 ,Subtotctl4=@subtotctl4,Subtotctl5=@subtotctl5,LstChgUser=@LstChgUser,LstChgDateTime=@LstChgDateTime Where Company=@company AND OffsetID=@OffsetID ";



                sqlcmd.Parameters.Add("@Description", SqlDbType.Char).Value = txtDescription.Text;               
                sqlcmd.Parameters.Add("@seqctl", SqlDbType.Decimal).Value = 0;
                sqlcmd.Parameters.Add("@seqctl1", SqlDbType.Decimal).Value =TxtSeqctl1.Text.Trim()!=""? decimal.Parse(TxtSeqctl1.Text):0;
                sqlcmd.Parameters.Add("@seqctl2", SqlDbType.Decimal).Value = TxtSeqctl2.Text.Trim()!=""?decimal.Parse(TxtSeqctl2.Text):0;
                sqlcmd.Parameters.Add("@seqctl3", SqlDbType.Decimal).Value = TxtSeqctl3.Text.Trim()!=""?decimal.Parse(TxtSeqctl3.Text):0;
                sqlcmd.Parameters.Add("@seqctl4", SqlDbType.Decimal).Value = TxtSeqctl4.Text.Trim()!=""?decimal.Parse(TxtSeqctl4.Text):0;
                sqlcmd.Parameters.Add("@seqctl5", SqlDbType.Decimal).Value = TxtSeqctl5.Text.Trim()!=""?decimal.Parse(TxtSeqctl5.Text):0;
                sqlcmd.Parameters.Add("@subtotctl", SqlDbType.Char).Value = "";
                sqlcmd.Parameters.Add("@subtotctl1", SqlDbType.Char).Value = ChkSubtotctl1.Checked ? "V":"";
                sqlcmd.Parameters.Add("@subtotctl2", SqlDbType.Char).Value = ChkSubtotctl2.Checked ? "V" : "";
                sqlcmd.Parameters.Add("@subtotctl3", SqlDbType.Char).Value = ChkSubtotctl3.Checked ? "V" : "";
                sqlcmd.Parameters.Add("@subtotctl4", SqlDbType.Char).Value = ChkSubtotctl4.Checked ? "V" : "";
                sqlcmd.Parameters.Add("@subtotctl5", SqlDbType.Char).Value = ChkSubtotctl5.Checked ? "V" : "";              
                sqlcmd.Parameters.Add("@LstChgUser", SqlDbType.Char).Value = _UserInfo.UData.UserId;
                sqlcmd.Parameters.Add("@LstChgDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.ToShortDateString();
                sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = DrpCompany.SelectValue;
                sqlcmd.Parameters.Add("@OffSetID", SqlDbType.Char).Value = txtoffsetID.Text;


               
            }

            _MyDBM.ExecuteCommand(Ssql, sqlcmd.Parameters, CommandType.Text);
        
        }
           
    

   
    private bool CheckData()
    {
        string strMessage = "";
        string strsubmsg = "";

        LabMessage.Text = "";
        bool bolcheck=true;

        int ierro = 0;



        //檢查公司
      if (DrpCompany.SelectValue == "")
      {

          strMessage += "．必須選擇公司資料<br>";
          ierro++;

      }                 

        //會計科目
      if (txtoffsetID.Text.Trim() == "")
      {

          strMessage += "．必須填入沖帳代號<br>";
          ierro++;
      }
      else if(txtoffsetID.Text.Trim().Length>2)
      {

          strMessage += "．沖帳代號不得超過2碼<br>";
          ierro++;
      }
        
        //說明
      if (txtDescription.Text.Trim().Length > 50)
      {

          strMessage += "．會計科目不得超過50個字<br>";
          ierro++;
      }
        


        //控制碼1
      if (TxtSeqctl1.Text.Trim() != "")
      {
          if (Checksort(TxtSeqctl1.Text, "控制碼1", out strsubmsg) == false)
          {
              ierro++;
          }
          strMessage += strsubmsg; 
      }

        //控制碼2
      if (TxtSeqctl2.Text.Trim() != "")
      {
          if (Checksort(TxtSeqctl1.Text, "控制碼2", out strsubmsg) == false)
          {
              ierro++;
          }
          strMessage += strsubmsg;
      }
        
        //控制碼3
      if (TxtSeqctl3.Text.Trim() != "")
      {
          if (Checksort(TxtSeqctl3.Text, "控制碼3", out strsubmsg) == false)
          {
              ierro++;
          }
          strMessage += strsubmsg;
      }


        //控制碼4

      if (TxtSeqctl4.Text.Trim() != "")
      {
          if (Checksort(TxtSeqctl4.Text, "控制碼4", out strsubmsg) == false)
          {
              ierro++;
          
          }
          strMessage += strsubmsg;
      }
        //控制碼5
      if (TxtSeqctl5.Text.Trim() != "")
      {
          if (Checksort(TxtSeqctl5.Text, "控制碼5", out strsubmsg) == false)
          {
              ierro++;
          }
          strMessage += strsubmsg;
      }

      if (ierro > 0)
      {
          bolcheck = false;
      }


      LabMessage.Text = strMessage;

      return bolcheck;
    }

    /// <summary>
    /// 檢查排序資料
    /// </summary>
    /// <param name="strcheck"></param>
    /// <returns></returns>
    bool Checksort(string strcheck,string strkind, out string stroutmsg )
    {
      string  strMessage="";
      decimal dresult; 

        //檢查長度
        if (strcheck.Trim().Length > 1)
        {
            strMessage += "．" + strkind + "不得超過2個字 <br>";
            stroutmsg=strMessage;
            return false;
        }

        //檢查數字
        if(decimal.TryParse(strcheck.Trim(),out dresult)==false)
        {
            strMessage += "．" + strkind + "須為數字<br>";
            stroutmsg = strMessage;
            return false;
        }

        stroutmsg = strMessage;
        return true;
    
    }

    private bool ValidateData(string strCompany, string strOffsetID)
    {
        //判斷資料是否重覆
        Ssql = "SELECT * FROM GLOffsetDef  WHERE Company = '" + strCompany + "' And OffsetID='" + strOffsetID + "'";

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
                strOffsetID = txtoffsetID.Text;
                //檢查是否有資料
                if (ValidateData(strCompany, strOffsetID) == true)
                {
                    UpdateData(true);

                }

                else
                {
                    //秀錯誤訊息
                    JsUtility.ClientMsgBox("會計科目已存在！", this.Page, "");

                }
            }
            strOffsetID = "";
            strCompany = "";

           
        }


    }



    protected void btnSaveExit_Click(object sender, ImageClickEventArgs e)
    {
        //新增與修改

        string strScript = "";

        strScript = "<script language=javascript>";
        strScript += "window.opener.location='GLA0430.aspx?Company=" + strCompany + "';";
        strScript+="window.close();</script> " ;



        if (blInsertMod == true)
        {
            strCompany = DrpCompany.SelectValue;
            strOffsetID = txtoffsetID.Text;
            //檢查是否有資料
            if (CheckData() == true)
            {
                if (ValidateData(strCompany, strOffsetID) == true)
                {
                    UpdateData(true);
                    Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", strScript);
                    JsUtility.ClientMsgBox("存檔成功！", this.Page, "");
                    strOffsetID = "";
                    strCompany = "";
                }

                else
                {
                    //秀錯誤訊息
                    JsUtility.ClientMsgBox("會計科目已存在！", this.Page, "");

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