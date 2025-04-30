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

public partial class GLA0420_M  : System.Web.UI.Page
{
    string Ssql = "";
    string Ssql1 = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLA0420";
    DBManger _MyDBM;   
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    string strCompany="",strColID=""  ;
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
            if (Request["Company"] != null && Request["ColID"] != null)
            {
                strCompany = Request["Company"].Trim();
                strColID = Request["ColID"].Trim();
                btnSaveGo.Visible = false;
                btnSaveGo.Enabled = false;
            }

            else
            {
                strCompany = "";
                strColID = "";
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
            
            BindEditData(strCompany, strColID);

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
            txtColID.Attributes.Add("ReadOnly", "ReadOnly");                   
            TxtColName.Attributes.Add("ReadOnly", "ReadOnly");
            TxtChkPro.Attributes.Add("ReadOnly", "ReadOnly");         
            TxtParmIn.Attributes.Add("ReadOnly", "ReadOnly");
            TxtDataFieldName1.Attributes.Add("ReadOnly", "ReadOnly");
            TxtDataFieldName2.Attributes.Add("ReadOnly", "ReadOnly");
            TxtKeyFieldName1.Attributes.Add("ReadOnly", "ReadOnly");
            TxtKeyFieldName2.Attributes.Add("ReadOnly", "ReadOnly");
             
        }

        //編輯
        else if(Mode=="E")
        {
            DrpCompany.Enabled = false;          
            txtColID.Attributes.Add("ReadOnly", "ReadOnly");       
        }         
    
    }
   
   
  
    

   

    /// <summary>
    /// 讀取修改資料
    /// </summary>
    /// <param name="strCompany"></param>
    /// <param name="strAccTNo"></param>
    private void BindEditData (string strCompany,string strColID)

    {
        _MyDBM = new DBManger();
        _MyDBM.New();

        SqlCommand sqlcmd = new SqlCommand();
        DataTable DT = new DataTable();

        Ssql1 = "SELECT * FROM GLColDef WHERE Company=@Company AND ColID=@ColID";

        sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = strCompany;
        sqlcmd.Parameters.Add("@ColID", SqlDbType.Char).Value = strColID;

         DT = _MyDBM.ExecuteDataTable(Ssql1,sqlcmd.Parameters,CommandType.Text);

        if (DT.Rows.Count > 0)
        {
            DrpCompany.SelectValue = DT.Rows[0]["Company"].ToString();
            txtColID.Text = DT.Rows[0]["ColID"].ToString();            
            TxtColName.Text = DT.Rows[0]["ColName"].ToString();
            TxtChkPro.Text = DT.Rows[0]["ChkProgram"].ToString();
            TxtParmIn.Text = DT.Rows[0]["ParmIn"].ToString();
            TxtKeyFieldName1.Text = DT.Rows[0]["KeyFieldName1"].ToString();
            TxtKeyFieldName2.Text = DT.Rows[0]["KeyFieldName2"].ToString();
            TxtDataFieldName1.Text = DT.Rows[0]["DataFieldName1"].ToString();
            TxtDataFieldName2.Text = DT.Rows[0]["DataFieldName2"].ToString();
           
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
                Ssql = @"INSERT INTO GLColDef (Company,ColID,ColName,ChkProgram,ParmIn,KeyFieldName1,KeyFieldName2,
DataFieldName1,DataFieldName2,LstChgUser,LstChgDateTime) VALUES
(@Company,@ColID,@ColName,@ChkProgram,@ParmIn,@KeyFieldName1,@KeyFieldName2,
@DataFieldName1,@DataFieldName2,@LstChgUser,@LstChgDateTime)";

                //新增
                sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = DrpCompany.SelectValue;
                sqlcmd.Parameters.Add("@ColID", SqlDbType.Char).Value = txtColID.Text.Trim();
                sqlcmd.Parameters.Add("@ColName", SqlDbType.Char).Value = txtColID.Text;
                sqlcmd.Parameters.Add("@ChkProgram", SqlDbType.VarChar).Value = TxtChkPro.Text;
                sqlcmd.Parameters.Add("@ParmIn", SqlDbType.VarChar).Value = TxtParmIn.Text;
                sqlcmd.Parameters.Add("@KeyFieldName1", SqlDbType.VarChar).Value = TxtKeyFieldName1.Text;
                sqlcmd.Parameters.Add("@KeyFieldName2", SqlDbType.VarChar).Value = TxtKeyFieldName2.Text;
                sqlcmd.Parameters.Add("@DataFieldName1", SqlDbType.VarChar).Value = TxtDataFieldName1.Text;
                sqlcmd.Parameters.Add("@DataFieldName2", SqlDbType.VarChar).Value = TxtDataFieldName2.Text;                        
                sqlcmd.Parameters.Add("@LstChgUser", SqlDbType.VarChar).Value = _UserInfo.UData.UserId;
                sqlcmd.Parameters.Add("@LstChgDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.ToShortDateString();
            }
            else
            {

                //修改
                Ssql = @"UPDATE  GLColDef SET ColName=@ColName,ChkProgram=@ChkProgram,ParmIn=@ParmIn,KeyFieldName1=@KeyFieldName1,
                KeyFieldName2=@KeyFieldName2,DataFieldName1=@DataFieldName1,DataFieldName2=@DataFieldName2,
                LstChgUser=@LstChgUser,LstChgDateTime=@LstChgDateTime WHERE Company=@Company 
                AND ColID=@ColID ";

                sqlcmd.Parameters.Add("@ColName", SqlDbType.VarChar).Value = TxtColName.Text.Trim();
                sqlcmd.Parameters.Add("@ChkProgram", SqlDbType.VarChar).Value = TxtChkPro.Text.Trim();
                sqlcmd.Parameters.Add("@ParmIn", SqlDbType.VarChar).Value = TxtParmIn.Text.Trim();
                sqlcmd.Parameters.Add("@KeyFieldName1", SqlDbType.VarChar).Value =TxtKeyFieldName1.Text.Trim();
                sqlcmd.Parameters.Add("@KeyFieldName2", SqlDbType.VarChar).Value = TxtKeyFieldName2.Text.Trim();
                sqlcmd.Parameters.Add("@DataFieldName1", SqlDbType.VarChar).Value = TxtDataFieldName1.Text.Trim();
                sqlcmd.Parameters.Add("@DataFieldName2", SqlDbType.VarChar).Value = TxtDataFieldName2.Text.Trim();                        
                sqlcmd.Parameters.Add("@LstChgUser", SqlDbType.VarChar).Value = _UserInfo.UData.UserId;
                sqlcmd.Parameters.Add("@LstChgDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.ToShortDateString();
                sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = DrpCompany.SelectValue;
                sqlcmd.Parameters.Add("@ColID", SqlDbType.Char).Value = txtColID.Text.Trim();


               
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

        //欄位代碼
      if (txtColID.Text.Trim() == "")
      {

          strMessage += "．必須填入欄位代號<br>";
          ierro++;
      }
      else if(txtColID.Text.Trim().Length>2)
      {

          strMessage += "．欄位代號不得超過2碼<br>";
          ierro++;
      }
        //欄位名稱
      if (TxtColName.Text.Trim() == "")
      {
          strMessage += "．必須填入欄位名稱<br>";
          ierro++;       
          
      }
      else if (TxtColName.Text.Trim().Length > 50)
      {
          strMessage += "．欄位名稱不得超過50個字<br>";
          ierro++;     
      }
        
      //檢核程式
      if (TxtChkPro.Text.Trim().Length>20)
      {
          strMessage += "．檢核程式不得超過20個字<br>";
          ierro++;    
      }

      //傳入參數
      if (TxtParmIn.Text.Trim().Length>20)
      {
          strMessage += "．傳入參數不得超過20個字<br>";
          ierro++;
      }

      //鍵值欄位1
      if (TxtKeyFieldName1.Text.Trim().Length>20)
      {
          strMessage += "．鍵值欄位1不得超過20個字<br>";
          ierro++;
      }

      //鍵值欄位2
      if (TxtKeyFieldName2.Text.Trim().Length > 20)
      {
          strMessage += "．鍵值欄位2不得超過20個字<br>";
          ierro++;
      }

      //資料欄位1
      if (TxtDataFieldName1.Text.Trim().Length>20)
      {
          strMessage += "．資料欄位1不得超過20個字<br>";
          ierro++;
      }

      //資料欄位2
      if (TxtDataFieldName2.Text.Trim().Length > 20)
      {
          strMessage += "．資料欄位2不得超過20個字<br>";
          ierro++;
      }  
    


      
     

      if (ierro > 0)
      {
          bolcheck = false;
      }


      LabMessage.Text = strMessage;

      return bolcheck;
    }

    

    private bool ValidateData(string strCompany, string strColID)
    {
        //判斷資料是否重覆
        Ssql = "SELECT * FROM GLColDef  WHERE Company = '" + strCompany + "' And ColID='" + strColID + "'";

        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);

        if (tb.Rows.Count > 0)
        {
            return false;
        }
        return true;
    }

    private void SetDeafult()
    { 
     //重設資料值
        DrpCompany.SelectValue = "";
        txtColID.Text = "";
        TxtColName.Text = "";
        TxtChkPro.Text = "";
        TxtDataFieldName1.Text = "";
        TxtDataFieldName2.Text = "";
        TxtKeyFieldName1.Text = "";
        TxtKeyFieldName2.Text = "";
        TxtParmIn.Text = "";
    
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
                strColID = txtColID.Text;
                //檢查是否有資料
                if (ValidateData(strCompany, strColID) == true)
                {
                    UpdateData(true);

                }

                else
                {
                    //秀錯誤訊息
                    JsUtility.ClientMsgBox("欄位名稱資料已存在！", this.Page, "");

                }
            }
            JsUtility.ClientMsgBox("存檔成功！", this.Page, "");
            strColID = "";
            strCompany = "";
            SetDeafult();



           
        }


    }



    protected void btnSaveExit_Click(object sender, ImageClickEventArgs e)
    {
        //新增與修改

        string strScript = "";

        strScript = "<script language=javascript>";
        strScript += "window.opener.location='GLA0420.aspx?Company=" + strCompany + "';";
        strScript+="window.close();</script> " ;



        if (blInsertMod == true)
        {
            strCompany = DrpCompany.SelectValue;
            strColID = txtColID.Text;
            //檢查是否有資料
            if (CheckData() == true)
            {
                if (ValidateData(strCompany, strColID) == true)
                {
                    UpdateData(true);
                    Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", strScript);
                    JsUtility.ClientMsgBox("存檔成功！", this.Page, "");
                    strColID = "";
                    strCompany = "";
                }

                else
                {
                    //秀錯誤訊息
                    JsUtility.ClientMsgBox("欄位名稱資料已存在！", this.Page, "");

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