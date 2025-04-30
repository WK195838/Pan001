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

public partial class PTA0170_M : System.Web.UI.Page
{
    string Ssql = "";
    string Ssql1 = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PTA0170";
    DBManger _MyDBM;    
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    string strCompany = "",strEmployeeID="";
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
        ScriptManager.RegisterStartupScript(ScriptManager1, this.GetType(), "", @"JQ();", true);
        txtBirthday.CssClass = "JQCalendar";
        txtOnBoardDate.CssClass = "JQCalendar";

        DrpCompany.SelectedChanged += new UserControl_CompanyList.SelectedIndexChanged(DrpCompany_SelectedIndex);
       
        
            btnSaveGo.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='GO';");
            if (Request["Company"] != null && Request["EmployeeID"] != null)
            {
                strCompany = Request["Company"].Trim();
                strEmployeeID = Request["EmployeeID"].Trim();
               
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
            bindDep(strCompany);
            BindEditData(strCompany,strEmployeeID );
          

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

    void DrpCompany_SelectedIndex(object sender, UserControl_CompanyList.SelectEventArgs e)
    {
        bindDep(DrpCompany.SelectValue);
        //throw new NotImplementedException();
    }

    private void changefunction(string Mode )
    {
        //唯讀 
        if (Mode == "R")
        {
            DrpCompany.Enabled = false;
            txtEmployeeID.Attributes.Add("ReadOnly", "ReadOnly");          
            txtEmployeeName.Attributes.Add("ReadOnly", "ReadOnly");
            txtEmployeeEName.Attributes.Add("ReadOnly", "ReadOnly");
            DrpBlood.Enabled = false;
            txtIDNo.Attributes.Add("ReadOnly", "ReadOnly");
            txtBirthday.Attributes.Add("ReadOnly", "ReadOnly"); 
            txtCommAddress.Attributes.Add("ReadOnly", "ReadOnly"); 
            txtPermAddress.Attributes.Add("ReadOnly", "ReadOnly"); 
            txtCommTEL.Attributes.Add("ReadOnly", "ReadOnly");
            txtConctPsn.Attributes.Add("ReadOnly", "ReadOnly");
            txtContTel.Attributes.Add("ReadOnly", "ReadOnly");
            txtOnBoardDate.Attributes.Add("ReadOnly", "ReadOnly");
            DrpDepartment.Enabled = false;
            txtRemark.Attributes.Add("ReadOnly", "ReadOnly");

          
        }

        //編輯
        else if(Mode=="E")
        {
            DrpCompany.Enabled = false;
            txtEmployeeID.Attributes.Add("ReadOnly", "ReadOnly");

           
        }         
    
    }
       

   

    /// <summary>
    /// 讀取修改資料
    /// </summary>
    /// <param name="strCompany"></param>
    /// <param name="strVendorID"></param>
    private void BindEditData(string strCompany, string strEmployeeID)

    {
        _MyDBM = new DBManger();
        _MyDBM.New();

        SqlCommand sqlcmd = new SqlCommand();
        DataTable DT = new DataTable();


        Ssql1 = "SELECT * FROM Employee WHERE Company=@Company AND EmployeeID=@EmployeeID";

        sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = strCompany;
        sqlcmd.Parameters.Add("@EmployeeID", SqlDbType.Char).Value = strEmployeeID;

        
         DT = _MyDBM.ExecuteDataTable(Ssql1,sqlcmd.Parameters,CommandType.Text);

        if (DT.Rows.Count > 0)
        {
            DrpCompany.SelectValue = DT.Rows[0]["Company"].ToString();
            txtEmployeeID.Text = DT.Rows[0]["EmployeeID"].ToString().Trim();
            txtEmployeeName.Text = DT.Rows[0]["EmployeeName"].ToString().Trim();
            txtEmployeeEName.Text = DT.Rows[0]["EmployeeEName"].ToString().Trim();
            DrpBlood.SelectedValue = DT.Rows[0]["Blood"].ToString().Trim();                  
            txtIDNo.Text = DT.Rows[0]["IDNo"].ToString().Trim();
            if (DT.Rows[0]["Sex"].ToString() != "")
            {
                DrpSex.SelectedValue = DT.Rows[0]["Sex"].ToString().Trim();
            }
            if (DT.Rows[0]["Birthday"].ToString().Trim() != "" && DT.Rows[0]["Birthday"].ToString().Trim() != "0")
            {
                txtBirthday.Text = _UserInfo.SysSet.FormatDate(DT.Rows[0]["Birthday"].ToString().Trim());
            }
            txtCommAddress.Text = DT.Rows[0]["CommAddress"].ToString().Trim();
            txtPermAddress.Text = DT.Rows[0]["PermAddress"].ToString().Trim();
            txtCommTEL.Text = DT.Rows[0]["CommTEL"].ToString().Trim();
            txtPermTel.Text = DT.Rows[0]["PermTel"].ToString().Trim();
            txtConctPsn.Text = DT.Rows[0]["ConctPsn"].ToString().Trim();
            txtContTel.Text = DT.Rows[0]["ContTel"].ToString().Trim();
            if (DT.Rows[0]["OnBoardDate"].ToString().Trim() != "" && DT.Rows[0]["OnBoardDate"].ToString().Trim() != "0")
            {
                txtOnBoardDate.Text = _UserInfo.SysSet.FormatDate(DT.Rows[0]["OnBoardDate"].ToString().Trim());
            }           
            DrpDepartment.SelectedValue = DT.Rows[0]["Department"].ToString().Trim();
            txtRemark.Text = DT.Rows[0]["Remark"].ToString().Trim();

        }



        
    }

  
    /// <summary>
    /// 新增新改資料
    /// </summary>
    /// <param name="Mode">true新增false維修</param>
        private int UpdateData(bool Mode)
        {
            _MyDBM = new DBManger();
            _MyDBM.New();

            int result = 0;

            string strbirthday = "";
            string stronBoardDate = "";
           
                

            SqlCommand  sqlcmd = new SqlCommand();
                     



            if (Mode == true)
            {
                //新增
                Ssql = @"INSERT INTO  Employee (Company,EmployeeID,EmployeeName,EmployeeEName,Blood,Sex,Birthday, 
                         CommAddress,PermAddress,CommTEL,PermTel,ConctPsn,ContTel,OnBoardDate,Department,
                         Remark,LstChgUser,LstChgDateTime) VALUES( @Company,@EmployeeID,@EmployeeName,
                         @EmployeeEName,@Blood,@Sex,@Birthday,@CommAddress,@PermAddress,@CommTEL,@PermTel,
                         @ConctPsn,@ContTel,@OnBoardDate,@Department,@Remark,@LstChgUser,@LstChgDateTime ) ";

            }
            else
            {

                //修改
                Ssql = @"UPDATE Employee SET  EmployeeName=@EmployeeName,EmployeeEName=@EmployeeEName,Blood=@Blood,
                       Sex=@Sex,Birthday=@Birthday,CommAddress=@CommAddress,PermAddress=@PermAddress,CommTEL=@CommTEL,PermTel=@PermTel,
                       ConctPsn=@ConctPsn,ContTel=@ContTel,OnBoardDate=@OnBoardDate,Department=@Department,
                       Remark=@Remark,LstChgUser=@LstChgUser,LstChgDateTime=@LstChgDateTime
                       WHERE Company=@Company AND EmployeeID=@EmployeeID ";        


               
            }

            if (txtBirthday.Text.Trim() != "")
            {
                strbirthday = _UserInfo.SysSet.ToADDate(txtBirthday.Text.Trim());
                strbirthday = strbirthday.Replace("/", "");
            }

            if (txtOnBoardDate.Text.Trim() != "")
            {
                stronBoardDate = _UserInfo.SysSet.ToADDate(txtOnBoardDate.Text.Trim());
                stronBoardDate = stronBoardDate.Replace("/","");

            }
            
            //新增
            sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = DrpCompany.SelectValue;
            sqlcmd.Parameters.Add("@EmployeeID", SqlDbType.VarChar).Value = txtEmployeeID.Text.Trim();
            sqlcmd.Parameters.Add("@EmployeeName", SqlDbType.VarChar).Value = txtEmployeeName.Text.Trim();
            sqlcmd.Parameters.Add("@EmployeeEName", SqlDbType.VarChar).Value = txtEmployeeEName.Text.Trim();
            sqlcmd.Parameters.Add("@Blood", SqlDbType.VarChar).Value = DrpBlood.SelectedValue.Trim();
            sqlcmd.Parameters.Add("@Sex", SqlDbType.VarChar).Value = DrpSex.SelectedValue.Trim();
            sqlcmd.Parameters.Add("@Birthday", SqlDbType.Decimal).Value = decimal.Parse(strbirthday);
            sqlcmd.Parameters.Add("@CommAddress", SqlDbType.NVarChar).Value = txtCommAddress.Text.Trim();
            sqlcmd.Parameters.Add("@PermAddress", SqlDbType.NVarChar).Value = txtPermAddress.Text.Trim();
            sqlcmd.Parameters.Add("@CommTEL", SqlDbType.VarChar).Value = txtCommTEL.Text.Trim();
            sqlcmd.Parameters.Add("@PermTel", SqlDbType.VarChar).Value = txtPermTel.Text.Trim();
            sqlcmd.Parameters.Add("@ConctPsn", SqlDbType.VarChar).Value = txtConctPsn.Text.Trim();
            sqlcmd.Parameters.Add("@ContTel", SqlDbType.VarChar).Value = txtContTel.Text.Trim();
            sqlcmd.Parameters.Add("@OnBoardDate", SqlDbType.Decimal).Value = decimal.Parse(stronBoardDate);
            sqlcmd.Parameters.Add("@Department", SqlDbType.Char).Value = DrpDepartment.SelectedValue;
            sqlcmd.Parameters.Add("@Remark", SqlDbType.VarChar).Value = txtRemark.Text.Trim();
            sqlcmd.Parameters.Add("@LstChgUser", SqlDbType.VarChar).Value = _UserInfo.UData.UserId;
            sqlcmd.Parameters.Add("@LstChgDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.ToShortDateString();


           result= _MyDBM.ExecuteCommand(Ssql, sqlcmd.Parameters, CommandType.Text);
           return result;
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

        //員工代號
      if (txtEmployeeID.Text.Trim() == "")
      {
          strMessage += "．必須填寫員工代號<br>";
          ierro++;      
      }
      else if (txtEmployeeID.Text.Trim().Length > 10)
      {
          strMessage += "．員工代號必須小於10碼<br>";
          ierro++;     
      } 
         
        
        //員工姓名
      if (txtEmployeeName.Text.Trim() == "")
      {
          strMessage += "．必須填寫員工姓名<br>";
          ierro++;         
      }
      else if (txtEmployeeName.Text.Trim().Length > 14)
      {
          strMessage += "．員工姓名必須小於14個字<br>";
          ierro++;
      }
        
        //英文姓名
      if (txtEmployeeEName.Text.Trim().Length > 50)
      {
          strMessage += "．英文姓名必須小於50個字<br>";
          ierro++;
      }             

        //身分證號

      if (txtIDNo.Text.Trim().Length>11)
      {
          strMessage += "．身分證號必須小於11個字<br>";
          ierro++;
      }

      //出生日期
      if (txtBirthday.Text.Trim()!="")
      {
          //檢查日期格式
          if (Checkdatetime(txtBirthday.Text.Trim())==false)
          {
              strMessage += "．出生日期格式錯誤<br>";
              ierro++;
          }
      }

      //通訊地址
      if (txtCommAddress.Text.Trim().Length > 62)
      {
          strMessage += "．通訊地址必須小於62個字<br>";
          ierro++;
      }

      //戶籍地址
      if (txtPermAddress.Text.Trim().Length > 62)
      {
          strMessage += "．戶籍地址必須小於62個字<br>";
          ierro++;
      }
        
        //通訊電話
      if (txtCommTEL.Text.Trim().Length > 15)
      {
          strMessage += "．通訊電話必須小於15個字<br>";
          ierro++;
      }
        
        //戶籍電話
      if (txtPermTel.Text.Trim().Length > 15)
      {
          strMessage += "．戶籍電話必須小於15個字<br>";
          ierro++;      
      }
         //聯絡人
      if (txtConctPsn.Text.Trim().Length>14)
      {
          strMessage += "．聯絡人必須小於15個字<br>";
          ierro++;
      }
        //聯絡人電話
      if (txtContTel.Text.Trim().Length > 14)
      {
          strMessage += "．聯絡人電話必須小於15個字<br>";
          ierro++;
      
      }

        //到職日期      
      if (txtOnBoardDate.Text.Trim()!="")
      {
          //檢查日期格式
          if (Checkdatetime(txtOnBoardDate.Text.Trim()) == false)
          {
              strMessage += "．到職日期格式錯誤<br>";
              ierro++;
          }          
      }

        //部門

      if (DrpDepartment.SelectedItem.Value.Length > 6)
      {
          strMessage += "．部門代號不可超過6碼<br>";
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
        txtEmployeeID.Text = "";      
        txtEmployeeName.Text = "";
        txtEmployeeEName.Text = "";
        DrpBlood.SelectedIndex = 0;
        txtIDNo.Text = "";
        DrpSex.SelectedIndex = 0;
        txtBirthday.Text = "";       
        txtCommAddress.Text = "";
        txtPermAddress.Text = "";
        txtCommTEL.Text = "";
        txtPermTel.Text = "";
        txtConctPsn.Text = "";
        txtContTel.Text = "";
        txtOnBoardDate.Text = "";
        DrpDepartment.SelectedIndex = 0;
        txtRemark.Text = "";   
    
    }

    private bool ValidateData(string strCompany,string strEmployee)
    {
        //判斷資料是否重覆
        Ssql = "SELECT * FROM Employee  WHERE Company = '" + strCompany + "' AND EmployeeID='"+strEmployeeID +"'";

        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);

        if (tb.Rows.Count > 0)
        {
            return false;
        }
        return true;
    }






    protected void btnSaveGo_Click(object sender, ImageClickEventArgs e)
    {
        int chki = 0;
        //只有新增
        if (blInsertMod == true)
        {
            if (CheckData()==true)
            {
                //檢查資料正確姓
                strCompany = DrpCompany.SelectValue;
                strEmployeeID = txtEmployeeID.Text.Trim();
           
                //檢查是否有資料
                if (ValidateData(strCompany, strEmployeeID) == true)
                {
                  chki= UpdateData(true);

                }

                else
                {
                    //秀錯誤訊息
                    JsUtility.ClientMsgBox("廠商資料已存在！", this.Page, "");
                    return;
                }
            }

            if (chki > 0)
            {
                strCompany = "";
                JsUtility.ClientMsgBox("存檔成功！", this.Page, "");
                Clear();
            }
            else
            {
                JsUtility.ClientMsgBox("存檔失敗！", this.Page, "");
            }

           
        }
    }



    protected void btnSaveExit_Click(object sender, ImageClickEventArgs e)
    {
        //新增與修改
        int checki = 0;
        string strScript = "";

        strScript = "<script language=javascript>";
        strScript += "window.opener.location='PTA0170.aspx?Company=" + strCompany + "';";
        strScript+="window.close();</script> " ;



        if (blInsertMod == true)
        {
            strCompany = DrpCompany.SelectValue;
            strEmployeeID = txtEmployeeID.Text.Trim();
          
            //檢查是否有資料
            if (CheckData() == true)
            {
                if (ValidateData(strCompany, strEmployeeID) == true)
                {
                    checki= UpdateData(true);
                    Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", strScript);
                    //JsUtility.ClientMsgBox("存檔成功！", this.Page, "");
                   
                    strCompany = "";
                }

                else
                {
                    //秀錯誤訊息
                    JsUtility.ClientMsgBox("員工資料已存在！", this.Page, "");
                    return;
                }

                if (checki == 0)
                {
                    JsUtility.ClientMsgBox("存檔失敗！", this.Page, "");
                    return;
                }

            }
        }
        else
        {
            //檢查是否有資料
            if (CheckData() == true)
            {
                checki = UpdateData(false);

                if (checki > 0)
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", strScript);
                }
                else
                {
                    JsUtility.ClientMsgBox("修改失敗！", this.Page, "");
                    return;
                }
            }
          
        }

    }


    /// <summary>
    /// 取得部門代碼
    /// </summary>
    /// <param name="strCompany"></param>
    private void bindDep(string strCompany)
    {
        string strSQL = "SELECT company ,Ltrim(Rtrim(DepCode)) AS DepCode,DepName FROM Department Where  Company='"+strCompany +"'";
        DataTable Dt = new DataTable();

        Dt = _MyDBM.ExecuteDataTable(strSQL);

        DrpDepartment.Items.Clear();
        if (Dt.Rows.Count > 0)
        {
            DrpDepartment.DataSource = Dt;
            DrpDepartment.DataValueField = "DepCode";
            DrpDepartment.DataTextField = "DepName";
            DrpDepartment.DataBind();
        }

        ListItem li = new ListItem();
        li.Value = "";
        li.Text = "-請選擇-";
        DrpDepartment.Items.Insert(0, li);
    }

    private bool Checkdatetime(string strDate)
    {
        bool bolresult = true;

        if (_UserInfo.SysSet.ToADDate(strDate) == "1912/01/01")
        {
            bolresult = false;         
        }
        return bolresult; 
    }
}