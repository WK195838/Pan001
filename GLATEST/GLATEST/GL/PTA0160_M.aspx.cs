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

public partial class PTA0160_M : System.Web.UI.Page
{
    string Ssql = "";
    string Ssql1 = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PTA0160";
    DBManger _MyDBM;    
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    string strCompany = "",strVendorID="";
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
            if (Request["Company"] != null && Request["VendorID"] != null)
            {
                strCompany = Request["Company"].Trim();
                strVendorID = Request["VendorID"].Trim();
               
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
       
        if (!Page.IsPostBack)
        {          
           
            BindEditData(strCompany,strVendorID );

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
            txtVendorID.Attributes.Add("ReadOnly", "ReadOnly");
            txtVendSName.Attributes.Add("ReadOnly", "ReadOnly");
            txtVendFName.Attributes.Add("ReadOnly", "ReadOnly");
            txtAddess1.Attributes.Add("ReadOnly", "ReadOnly");
            txtAddess2.Attributes.Add("ReadOnly", "ReadOnly");
            txtResponsor.Attributes.Add("ReadOnly", "ReadOnly"); 
            txtContPsn01.Attributes.Add("ReadOnly", "ReadOnly"); 
            txtTel1.Attributes.Add("ReadOnly", "ReadOnly"); 
            txtContPsn02.Attributes.Add("ReadOnly", "ReadOnly"); 
            txtTel2.Attributes.Add("ReadOnly", "ReadOnly"); 
            txtFaxN0.Attributes.Add("ReadOnly", "ReadOnly"); 
            DrpFL.Enabled=false;
            DrpPrintTitle.Enabled=false;
            DrpCheckType.Enabled=false;
            txtRemark.Attributes.Add("ReadOnly", "ReadOnly");

          
        }

        //編輯
        else if(Mode=="E")
        {
            DrpCompany.Enabled = false;
            txtVendorID.Attributes.Add("ReadOnly", "ReadOnly");

           
        }         
    
    }
       

   

    /// <summary>
    /// 讀取修改資料
    /// </summary>
    /// <param name="strCompany"></param>
    /// <param name="strVendorID"></param>
    private void BindEditData(string strCompany,string strVendorID)

    {
        _MyDBM = new DBManger();
        _MyDBM.New();

        SqlCommand sqlcmd = new SqlCommand();
        DataTable DT = new DataTable();
        

        Ssql1 = "SELECT * FROM Vendor WHERE Company=@Company AND VendorID=@VendorID";

        sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = strCompany;
        sqlcmd.Parameters.Add("@VendorID", SqlDbType.Char).Value = strVendorID;

        
         DT = _MyDBM.ExecuteDataTable(Ssql1,sqlcmd.Parameters,CommandType.Text);

        if (DT.Rows.Count > 0)
        {
            DrpCompany.SelectValue = DT.Rows[0]["Company"].ToString();
            txtVendorID.Text = DT.Rows[0]["VendorID"].ToString().Trim();
            txtVendSName.Text = DT.Rows[0]["VendSName"].ToString().Trim();
            txtVendFName.Text = DT.Rows[0]["VendFName"].ToString().Trim();
            txtAddess1.Text = DT.Rows[0]["Addess1"].ToString().Trim();
            txtAddess2.Text = DT.Rows[0]["Address2"].ToString().Trim();
            txtResponsor.Text = DT.Rows[0]["Responsor"].ToString().Trim();
            txtContPsn01.Text = DT.Rows[0]["ContPsn01"].ToString().Trim();
            txtTel1.Text = DT.Rows[0]["Tel01"].ToString().Trim();
            txtContPsn02.Text = DT.Rows[0]["ContPsn02"].ToString().Trim();
            txtTel2.Text = DT.Rows[0]["Tel02"].ToString().Trim();
            txtFaxN0.Text = DT.Rows[0]["FaxN0"].ToString().Trim();
            DrpFL.SelectedValue = DT.Rows[0]["F_L"].ToString().Trim();
            DrpPrintTitle.SelectedValue = DT.Rows[0]["PrinuTitle"].ToString();
            DrpCheckType.SelectedValue = DT.Rows[0]["CheckType"].ToString();
            txtRemark.Text = DT.Rows[0]["Remark"].ToString().Trim();

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
                Ssql = @" INSERT INTO Vendor (Company,VendorID,VendSName,VendFName,Addess1,Address2,Responsor,
                    ContPsn01,Tel01,ContPsn02,Tel02,FaxN0,F_L,PrinuTitle,CheckType,Remark,LstChgUser,LstChgDateTime)
                   VALUES(@Company,@VendorID,@VendSName,@VendFName,@Addess1,@Address2,@Responsor,
                   @ContPsn01,@Tel01,@ContPsn02,@Tel02,@FaxN0,@F_L,@PrinuTitle,@CheckType,@Remark,@LstChgUser,@LstChgDateTime ) ";
                               
                //新增
                sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = DrpCompany.SelectValue;
                sqlcmd.Parameters.Add("@VendorID", SqlDbType.VarChar).Value = txtVendorID.Text.Trim();
                sqlcmd.Parameters.Add("@VendSName", SqlDbType.VarChar).Value = txtVendSName.Text.Trim();
                sqlcmd.Parameters.Add("@VendFName", SqlDbType.VarChar).Value = txtVendFName.Text.Trim();
                sqlcmd.Parameters.Add("@Addess1", SqlDbType.VarChar).Value = txtAddess1.Text.Trim();
                sqlcmd.Parameters.Add("@Address2", SqlDbType.VarChar).Value = txtAddess2.Text.Trim();
                sqlcmd.Parameters.Add("@Responsor", SqlDbType.VarChar).Value = txtResponsor.Text.Trim();
                sqlcmd.Parameters.Add("@ContPsn01", SqlDbType.VarChar).Value = txtContPsn01.Text.Trim();
                sqlcmd.Parameters.Add("@Tel01", SqlDbType.VarChar).Value = txtTel1.Text.Trim();
                sqlcmd.Parameters.Add("@ContPsn02", SqlDbType.VarChar).Value = txtContPsn02.Text.Trim();
                sqlcmd.Parameters.Add("@Tel02", SqlDbType.VarChar).Value = txtTel2.Text.Trim();
                sqlcmd.Parameters.Add("@FaxN0", SqlDbType.VarChar).Value = txtFaxN0.Text.Trim();
                sqlcmd.Parameters.Add("@F_L", SqlDbType.Char).Value = DrpFL.SelectedValue;
                sqlcmd.Parameters.Add("@PrinuTitle", SqlDbType.Char).Value = DrpPrintTitle.SelectedValue;
                sqlcmd.Parameters.Add("@CheckType", SqlDbType.Char).Value = DrpCheckType.SelectedValue;
                sqlcmd.Parameters.Add("@Remark", SqlDbType.VarChar).Value = txtRemark.Text;                                  
                sqlcmd.Parameters.Add("@LstChgUser", SqlDbType.Char).Value = _UserInfo.UData.UserId;
                sqlcmd.Parameters.Add("@LstChgDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.ToShortDateString();
            }
            else
            {

                //修改
                Ssql = @"UPDATE Vendor SET  VendSName=@VendSName,VendFName=@VendFName,Addess1=@Addess1,
                        Address2=@Address2,Responsor=@Responsor,ContPsn01=@ContPsn01,Tel01=@Tel01,
                        ContPsn02=@ContPsn02,Tel02=@Tel02,FaxN0=@FaxN0,F_L=@F_L,PrinuTitle=@PrinuTitle
                       ,CheckType=@CheckType,Remark=@Remark,LstChgUser=@LstChgUser,LstChgDateTime=@LstChgDateTime
                       WHERE company=@Company AND VendorID=@VendorID ";

                sqlcmd.Parameters.Add("@VendSName", SqlDbType.VarChar).Value = txtVendSName.Text.Trim();
                sqlcmd.Parameters.Add("@VendFName", SqlDbType.VarChar).Value = txtVendFName.Text.Trim();
                sqlcmd.Parameters.Add("@Addess1", SqlDbType.VarChar).Value = txtAddess1.Text.Trim();
                sqlcmd.Parameters.Add("@Address2", SqlDbType.VarChar).Value = txtAddess2.Text.Trim();
                sqlcmd.Parameters.Add("@Responsor", SqlDbType.VarChar).Value = txtResponsor.Text.Trim();
                sqlcmd.Parameters.Add("@ContPsn01", SqlDbType.VarChar).Value = txtContPsn01.Text.Trim();
                sqlcmd.Parameters.Add("@Tel01", SqlDbType.VarChar).Value = txtTel1.Text.Trim();
                sqlcmd.Parameters.Add("@ContPsn02", SqlDbType.VarChar).Value = txtContPsn02.Text.Trim();
                sqlcmd.Parameters.Add("@Tel02", SqlDbType.VarChar).Value = txtTel2.Text.Trim();
                sqlcmd.Parameters.Add("@FaxN0", SqlDbType.VarChar).Value = txtFaxN0.Text.Trim();
                sqlcmd.Parameters.Add("@F_L", SqlDbType.Char).Value = DrpFL.SelectedValue;
                sqlcmd.Parameters.Add("@PrinuTitle", SqlDbType.Char).Value = DrpPrintTitle.SelectedValue;
                sqlcmd.Parameters.Add("@CheckType", SqlDbType.Char).Value = DrpCheckType.SelectedValue;
                sqlcmd.Parameters.Add("@Remark", SqlDbType.VarChar).Value = txtRemark.Text;                  
                sqlcmd.Parameters.Add("@LstChgUser", SqlDbType.Char).Value = _UserInfo.UData.UserId;
                sqlcmd.Parameters.Add("@LstChgDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.ToShortDateString();
                sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = DrpCompany.SelectValue;
                sqlcmd.Parameters.Add("@VendorID", SqlDbType.VarChar).Value = txtVendorID.Text.Trim();
               


               
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

        //廠商代碼
      if (txtVendorID.Text.Trim() == "")
      { 
          strMessage += "．必須填寫廠商代碼<br>";
          ierro++;      
      }
      else if (txtVendSName.Text.Trim().Length>10)
      {
          strMessage += "．廠商代碼必須小於10碼<br>";
          ierro++;     
      }
        
        //廠商簡稱   
      if (txtVendSName.Text.Trim().Length > 10)
      {
          strMessage += "．廠商簡稱必須小於10個字<br>";
          ierro++;
      }
        
        //廠商名稱
      if (txtVendFName.Text.Trim().Length > 42)
      {
          strMessage += "．廠商名稱必須小於42個字<br>";
          ierro++;
      }
        
        //廠商地址1
      if (txtAddess1.Text.Trim().Length > 62)
      {
          strMessage += "．廠商地址1必須小於62個字<br>";
          ierro++;
      }
        
        //廠商地址2
      if (txtAddess2.Text.Trim().Length > 62)
      {
          strMessage += "．廠商地址1必須小於62個字<br>";
          ierro++;
      }
        
        //負責人
      if (txtResponsor.Text.Trim().Length > 14)
      {
          strMessage += "．負責人必須小於14個字<br>";
          ierro++;
      }


        //聯絡人1
      if (txtContPsn01.Text.Trim().Length > 14)
      {
          strMessage += "．聯絡人1必須小於14個字<br>";
          ierro++;
      }

        //電話1

      if (txtTel1.Text.Trim().Length > 15)
      {
          strMessage += "．電話1必須小於15個字<br>";
          ierro++;
      }
        //聯絡人2
      if (txtContPsn02.Text.Trim().Length > 14)
      {
          strMessage += "．聯絡人2必須小於14個字<br>";
          ierro++;
      }
        //電話2

      if (txtTel2.Text.Trim().Length > 15)
      {
          strMessage += "．電話2必須小於15個字<br>";
          ierro++;
      }
        //傳真號碼

      if (txtFaxN0.Text.Trim().Length > 15)
      {
          strMessage += "．傳真號碼必須小於15個字<br>";
          ierro++;
      }
        
        //備註

      if (txtRemark.Text.Trim().Length > 62)
      {
          strMessage += "．備註必須小於62個字<br>";
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
        txtVendorID.Text = "";
        txtVendSName.Text = "";
        txtVendFName.Text = "";
        txtAddess1.Text = "";
        txtAddess2.Text = "";
        txtResponsor.Text = "";
        txtContPsn01.Text = "";
        txtTel1.Text = "";
        txtContPsn02.Text = "";
        txtTel2.Text = "";
        txtFaxN0.Text = "";
        DrpFL.SelectedIndex=0;
        DrpPrintTitle.SelectedIndex=0;
        DrpCheckType.SelectedIndex=0;
        txtRemark.Text = "";
      
    
    
    }

    private bool ValidateData(string strCompany,string strVendorID)
    {
        //判斷資料是否重覆
        Ssql = "SELECT * FROM Vendor  WHERE Company = '" + strCompany + "' AND VendorID='"+strVendorID +"'";

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
                strVendorID = txtVendorID.Text.Trim();
           
                //檢查是否有資料
                if (ValidateData(strCompany,strVendorID) == true)
                {
                    UpdateData(true);

                }

                else
                {
                    //秀錯誤訊息
                    JsUtility.ClientMsgBox("廠商資料已存在！", this.Page, "");

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
        strScript += "window.opener.location='PTA0160.aspx?Company=" + strCompany + "';";
        strScript+="window.close();</script> " ;



        if (blInsertMod == true)
        {
            strCompany = DrpCompany.SelectValue;
            strVendorID = txtVendorID.Text.Trim();
          
            //檢查是否有資料
            if (CheckData() == true)
            {
                if (ValidateData(strCompany,strVendorID) == true)
                {
                    UpdateData(true);
                    Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", strScript);
                    //JsUtility.ClientMsgBox("存檔成功！", this.Page, "");
                   
                    strCompany = "";
                }

                else
                {
                    //秀錯誤訊息
                    JsUtility.ClientMsgBox("廠商資料已存在！", this.Page, "");

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