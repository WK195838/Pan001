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

public partial class PTA0150_M : System.Web.UI.Page
{
    string Ssql = "";
    string Ssql1 = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PTA0150";
    DBManger _MyDBM;    
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    string strCompanyCode = "", strCustomerCode = "";
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

        //int icalendersYear = DateTime.Now.AddYears(-80).Year - 1911;
        //int icalendereYear = DateTime.Now.AddYears(5).Year - 1911;


        //DrpCompany.SelectedIndex += new UserControl_CompanyList.SelectedIndexChanged(DrpCompany_SelectedIndex);
        //string strScript = "return GetPromptTheDate(" + txtBirthday.ClientID + ","+icalendersYear.ToString() +","+icalendereYear.ToString()+");";
        //ibOW_Birthday.Attributes.Add("onclick", strScript);
        //strScript = "return GetPromptTheDate(" + txtOnBoardDate.ClientID + ",60,"+icalendereYear.ToString()+");";
        //ibOW_onBoard.Attributes.Add("onclick", strScript);
       
        
            btnSaveGo.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='GO';");
            if (Request["CompanyCode"] != null && Request["CustomerCode"] != null)
            {
                strCompanyCode = Request["CompanyCode"].Trim();
                strCustomerCode = Request["CustomerCode"].Trim();
               
                btnSaveGo.Visible = false;
                btnSaveGo.Enabled = false;
            }

            else
            {
                strCompanyCode = "";            
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
           // bindDep(strCompanyCode);
            BindEditData(strCompanyCode,strCustomerCode );
          

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
            txtCustomerCode.Attributes.Add("ReadOnly", "ReadOnly");          
            txtCustomerSname.Attributes.Add("ReadOnly", "ReadOnly");
            txtCustomerFname.Attributes.Add("ReadOnly", "ReadOnly");
            txtBilltoAddress.Attributes.Add("ReadOnly", "ReadOnly");
            txtShiptoAddress.Attributes.Add("ReadOnly", "ReadOnly");
            txtResponser.Attributes.Add("ReadOnly", "ReadOnly");
            txtContact01.Attributes.Add("ReadOnly", "ReadOnly"); 
            txtTelno01.Attributes.Add("ReadOnly", "ReadOnly");
            txtContact02.Attributes.Add("ReadOnly", "ReadOnly");
            txtTelno02.Attributes.Add("ReadOnly", "ReadOnly");
            txtFaxno.Attributes.Add("ReadOnly", "ReadOnly");
            DrpFL.Enabled = false;
            txtPayment.Attributes.Add("ReadOnly", "ReadOnly");
            txtHQCode.Attributes.Add("ReadOnly", "ReadOnly");
            txtSalesID.Attributes.Add("ReadOnly", "ReadOnly");         
            txtRemark.Attributes.Add("ReadOnly", "ReadOnly");

          
        }

        //編輯
        else if(Mode=="E")
        {
            DrpCompany.Enabled = false;
            txtCustomerCode.Attributes.Add("ReadOnly", "ReadOnly");

           
        }         
    
    }
       

   

    /// <summary>
    /// 讀取修改資料
    /// </summary>
    /// <param name="strCompanyCode"></param>
    /// <param name="strCustomerCode"></param>
    private void BindEditData(string strCompanyCode, string strCustomerCode)

    {
        _MyDBM = new DBManger();
        _MyDBM.New();

        SqlCommand sqlcmd = new SqlCommand();
        DataTable DT = new DataTable();


        Ssql1 = "SELECT * FROM Customer WHERE CompanyCode=@CompanyCode AND CustomerCode=@CustomerCode";

        sqlcmd.Parameters.Add("@CompanyCode", SqlDbType.Char).Value = strCompanyCode;
        sqlcmd.Parameters.Add("@CustomerCode", SqlDbType.Char).Value = strCustomerCode;

        
         DT = _MyDBM.ExecuteDataTable(Ssql1,sqlcmd.Parameters,CommandType.Text);

        if (DT.Rows.Count > 0)
        {
            DrpCompany.SelectValue = DT.Rows[0]["CompanyCode"].ToString();
            txtCustomerCode.Text = DT.Rows[0]["CustomerCode"].ToString().Trim();
            txtCustomerSname.Text = DT.Rows[0]["CustomerSname"].ToString().Trim();
            txtCustomerFname.Text = DT.Rows[0]["CustomerFname"].ToString().Trim();
            txtBilltoAddress.Text = DT.Rows[0]["BilltoAddress"].ToString().Trim();
            txtShiptoAddress.Text = DT.Rows[0]["ShiptoAddress"].ToString().Trim();
            txtResponser.Text = DT.Rows[0]["Responser"].ToString().Trim();
            txtContact01.Text = DT.Rows[0]["Contact01"].ToString().Trim();
            txtTelno01.Text = DT.Rows[0]["Telno01"].ToString().Trim();
            txtContact02.Text = DT.Rows[0]["Contact02"].ToString().Trim();
            txtTelno02.Text = DT.Rows[0]["Telno02"].ToString().Trim();
            txtFaxno.Text = DT.Rows[0]["Faxno"].ToString().Trim();
            DrpFL.SelectedValue = DT.Rows[0]["F_L"].ToString().Trim();
            txtPayment.Text = DT.Rows[0]["Payment"].ToString().Trim();
            txtChopNo.Text = DT.Rows[0]["ChopNo"].ToString().Trim();
            txtHQCode.Text = DT.Rows[0]["HQCode"].ToString().Trim();
            txtSalesID.Text = DT.Rows[0]["SalesID"].ToString().Trim();
            txtStatementDay.Text = DT.Rows[0]["StatementDay"].ToString().Trim();
            txtCustomerFname01.Text = DT.Rows[0]["CustomerFname01"].ToString().Trim();
            DrpInvoiceType.SelectedValue = DT.Rows[0]["InvoiceType"].ToString().Trim();
            DrpTaxCode.SelectedValue = DT.Rows[0]["TaxCode"].ToString().Trim();
            DrpCheckNo.SelectedValue = DT.Rows[0]["CheckNo"].ToString().Trim();
            txtCreditLimit.Text = DT.Rows[0]["CreditLimit"].ToString().Trim();
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

            SqlCommand  sqlcmd = new SqlCommand();
                     
            if (Mode == true)
            {
                //新增
                Ssql = @"INSERT INTO Customer (CompanyCode,CustomerCode,CustomerSname,CustomerFname,BilltoAddress,
                        ShiptoAddress,Responser,Contact01,Telno01,Contact02,Telno02,Faxno,F_L,Payment,
                        Chopno,HQCode,SalesID,StatementDay,CustomerFname01,InvoiceType,TaxCode,CheckNo,CreditLimit,Remark,
                        LstChgUser,LstChgDateTime ) VALUES (@CompanyCode,@CustomerCode,@CustomerSname,@CustomerFname,@BilltoAddress,
                        @ShiptoAddress,@Responser,@Contact01,@Telno01,@Contact02,@Telno02,@Faxno,@F_L,@Payment,
                        @Chopno,@HQCode,@SalesID,@StatementDay,@CustomerFname01,@InvoiceType,@TaxCode,@CheckNo,@CreditLimit,@Remark,
                        @LstChgUser,@LstChgDateTime ) ";

            }
            else
            {

                //修改
                Ssql = @"UPDATE Customer SET CustomerSname=@CustomerSname,CustomerFname=@CustomerFname,BilltoAddress=@BilltoAddress,
                         ShiptoAddress=@ShiptoAddress,Responser=@Responser,Contact01=@Contact01,Telno01=@Telno01,
                         Contact02=@Contact02,Telno02=@Telno02,Faxno=@Faxno,F_L=@F_L,Payment=@Payment,
                         Chopno=@Chopno,HQCode=@HQCode,SalesID=@SalesID,StatementDay=@StatementDay,
                         CustomerFname01=@CustomerFname01,InvoiceType=@InvoiceType,TaxCode=@TaxCode,
                         CheckNo=@CheckNo,CreditLimit=@CreditLimit,Remark=@Remark,LstChgUser=@LstChgUser,
                         LstChgDateTime=@LstChgDateTime Where CompanyCode=@CompanyCode AND CustomerCode=@CustomerCode ";                
            }
                    
            
            //新增
            sqlcmd.Parameters.Add("@CompanyCode", SqlDbType.Char).Value = DrpCompany.SelectValue;
            sqlcmd.Parameters.Add("@CustomerCode", SqlDbType.VarChar).Value = txtCustomerCode.Text.Trim();
            sqlcmd.Parameters.Add("@CustomerSname", SqlDbType.VarChar).Value = txtCustomerSname.Text.Trim();
            sqlcmd.Parameters.Add("@CustomerFname", SqlDbType.VarChar).Value = txtCustomerFname.Text.Trim();
            sqlcmd.Parameters.Add("@BilltoAddress", SqlDbType.VarChar).Value = txtBilltoAddress.Text.Trim();
            sqlcmd.Parameters.Add("@ShiptoAddress", SqlDbType.VarChar).Value = txtShiptoAddress.Text.Trim();
            sqlcmd.Parameters.Add("@Responser", SqlDbType.VarChar).Value = txtResponser.Text.Trim();
            sqlcmd.Parameters.Add("@Contact01", SqlDbType.VarChar).Value = txtContact01.Text.Trim();
            sqlcmd.Parameters.Add("@Telno01", SqlDbType.VarChar).Value = txtTelno01.Text.Trim();
            sqlcmd.Parameters.Add("@Contact02", SqlDbType.VarChar).Value = txtContact02.Text.Trim();
            sqlcmd.Parameters.Add("@Telno02", SqlDbType.VarChar).Value = txtTelno02.Text.Trim();
            sqlcmd.Parameters.Add("@Faxno", SqlDbType.VarChar).Value = txtFaxno.Text.Trim();
            sqlcmd.Parameters.Add("@F_L", SqlDbType.VarChar).Value = DrpFL.SelectedValue.Trim();
            sqlcmd.Parameters.Add("@Payment", SqlDbType.VarChar).Value = txtPayment.Text.Trim();
            sqlcmd.Parameters.Add("@ChopNo", SqlDbType.VarChar).Value = txtChopNo.Text.Trim();
            sqlcmd.Parameters.Add("@HQCode", SqlDbType.VarChar).Value = txtHQCode.Text.Trim();
            sqlcmd.Parameters.Add("@SalesID", SqlDbType.VarChar).Value = txtSalesID.Text.Trim();
            sqlcmd.Parameters.Add("@StatementDay", SqlDbType.Decimal).Value = txtStatementDay.Text.Trim()!="" ? decimal.Parse(txtStatementDay.Text.Trim()):1;
            sqlcmd.Parameters.Add("@CustomerFname01", SqlDbType.VarChar).Value = txtCustomerFname01.Text.Trim();
            sqlcmd.Parameters.Add("@InvoiceType", SqlDbType.Char).Value = DrpInvoiceType.SelectedValue.Trim();
            sqlcmd.Parameters.Add("@TaxCode", SqlDbType.Char).Value = DrpTaxCode.SelectedValue.Trim();
            sqlcmd.Parameters.Add("@CheckNo", SqlDbType.Char).Value = DrpCheckNo.SelectedValue.Trim();
            sqlcmd.Parameters.Add("@CreditLimit", SqlDbType.Decimal).Value =  txtCreditLimit.Text.Trim()!="" ? decimal.Parse(txtCreditLimit.Text.Trim()):0;
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
        decimal  i= 0;
        decimal cr=0;



        //檢查公司
      if (DrpCompany.SelectValue == "")
      {

          strMessage += "．必須選擇公司資料<br>";
          ierro++;

      }

        //客戶代號
      if (txtCustomerCode.Text.Trim() == "")
      {
          strMessage += "．必須填寫客戶代號<br>";
          ierro++;      
      }
      else if (txtCustomerCode.Text.Trim().Length > 10)
      {
          strMessage += "．客戶代號必須小於10碼<br>";
          ierro++;     
      }         
        
        //客戶簡稱
      if (txtCustomerSname.Text.Trim() == "")
      {
          strMessage += "．必須填寫客戶簡稱<br>";
          ierro++;         
      }
      else if (txtCustomerSname.Text.Trim().Length > 14)
      {
          strMessage += "．客戶簡稱必須小於14個字<br>";
          ierro++;
      }
        
        //客戶名稱
      if (txtCustomerFname.Text.Trim()=="")
      {
          strMessage += "．必需填寫客戶名稱<br>";
          ierro++;
      }
      else if (txtCustomerFname.Text.Trim().Length > 42)
      {
          strMessage += "．客戶名稱必須小於42個字<br>";
          ierro++;      
      }
        
        //發票地址
      if (txtBilltoAddress.Text.Trim() == "")
      {
          strMessage += "．必須填寫發票地址<br>";
          ierro++;
      }
      else if (txtBilltoAddress.Text.Trim().Length > 62)
      {
          strMessage += "．發票地址必須小於62個字<br>";
          ierro++;
      }


        //送貨地址

      if (txtShiptoAddress.Text.Trim() == "")
      {
          strMessage += "．必須填寫發票地址<br>";
          ierro++;
      }
      else if (txtShiptoAddress.Text.Trim().Length > 11)
      {
          strMessage += "．發票地址必須小於62個字<br>";
          ierro++;
      }

      //負責人

      if (txtResponser.Text.Trim().Length > 14)
      {
          strMessage += "．負責人必須小於62個字<br>";
          ierro++;      
      }
        

      //聯絡人一
      if (txtContact01.Text.Trim()=="")
      {
          strMessage += "．必須填寫聯絡人一<br>";
          ierro++;          
      }
      else if (txtContact01.Text.Trim().Length > 14)
      {
          strMessage += "．聯絡人一必須小於14個字<br>";
          ierro++;      
      }
 


      //電話一
      if (txtTelno01.Text.Trim()=="")
      {
          strMessage += "．必須填寫電話一<br>";
          ierro++;
      }
      else if (txtTelno01.Text.Trim().Length > 15)
      {
          strMessage += "．電話一必須小於15個字<br>";
          ierro++;       
      }

      //聯絡人二
      if (txtContact02.Text.Trim() == "")
      {
          strMessage += "．必須填寫聯絡人二<br>";
          ierro++;
      }
      else if (txtContact02.Text.Trim().Length > 15)
      {
          strMessage += "．聯絡人二必須小於15個字<br>";
          ierro++;
      }

      //電話二
      if (txtTelno02.Text.Trim().Length > 15)
      {
          strMessage += "．電話二必須小於15個字<br>";
          ierro++;
      }
      //傳真號碼

      if (txtFaxno.Text.Trim().Length>15)
      {
          strMessage += "．傳真號碼必須小於15個字<br>";
          ierro++;
      }

      //付款條件
      if (txtPayment.Text.Trim().Length > 8)
      {
          strMessage += "．付款條件必須小於8個字<br>";
          ierro++;
      }

      //統一編號
     if (txtChopNo.Text.Trim().Length > 8)
      {
          strMessage += "．統一編號必須小於8個字<br>";
          ierro++;
      }
      
      //總公司編號

   
       if (txtHQCode.Text.Trim().Length > 10)
      {
          strMessage += "．總公司編號必須小於10個字<br>";
          ierro++;
      }
      
      //業務員編號

  
     if (txtSalesID.Text.Trim().Length > 10)
      {
          strMessage += "．業務員編號必須小於10個字<br>";
          ierro++;
      }

      //對帳日期
      if (txtStatementDay.Text.Trim() == "")
      {
          strMessage += "．必須填寫對帳日期<br>";
          ierro++;
      }
      else if (txtStatementDay.Text.Trim().Length > 2)
      {
          strMessage += "．對帳日期必須小於2個字<br>";
          ierro++;
      }
      else if (decimal.TryParse(txtStatementDay.Text.Trim(), out i)==true)
      {
          if (i < 1 || i > 31)
          {
              strMessage += "．對帳日期必須介於1~31<br>";
              ierro++;
          }
      }
      else
      {
          strMessage += "．對帳日期必須是數字<br>";
          ierro++;        
      }



      //英文名稱

   
      if (txtCustomerFname01.Text.Trim().Length > 42)
      {
          strMessage += "．英文名稱必須小於42個字<br>";
          ierro++;
      }
   
      //信用額度
   
       if (txtCreditLimit.Text.Trim().Length > 42)
      {
          strMessage += "．信用額度必須小於9個字<br>";
          ierro++;
      }
       else if (decimal.TryParse(txtCreditLimit.Text.Trim(), out cr) == false)
       {
           strMessage += "．信用額度必須是數字<br>";
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
          
        txtCustomerCode.Text = "";      
        txtCustomerSname.Text = "";
        txtCustomerFname.Text = "";
        txtBilltoAddress.Text = "";
        txtShiptoAddress.Text = "";
        txtResponser.Text = "";        
        txtContact01.Text = "";
        txtTelno01.Text = "";
        txtContact02.Text = "";
        txtTelno02.Text = "";
        txtFaxno.Text = "";
        DrpFL.SelectedIndex = 0;
        txtPayment.Text = "";
        txtChopNo.Text = "";
        txtHQCode.Text = "";
        txtSalesID.Text = "";
        txtStatementDay.Text = "";
        txtCustomerFname01.Text = "";
        DrpInvoiceType.SelectedIndex = 0;
        DrpCheckNo.SelectedIndex = 0;
        DrpTaxCode.SelectedIndex = 0;
        txtCreditLimit.Text = "";
        txtRemark.Text = "";
      
    
    
    }

    private bool ValidateData(string strCompanyCode, string strCustomerCode)
    {
        //判斷資料是否重覆
        Ssql = "SELECT * FROM Customer  WHERE CompanyCode = '" + strCompanyCode + "' AND CustomerCode='" + strCustomerCode + "'";

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
                strCompanyCode = DrpCompany.SelectValue;
                strCustomerCode = txtCustomerCode.Text.Trim();
           
                //檢查是否有資料
                if (ValidateData(strCompanyCode, strCustomerCode) == true)
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
                strCompanyCode = "";
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
        strScript += "window.opener.location='PTA0150.aspx?Company=" + strCompanyCode + "';";
        strScript+="window.close();</script> " ;



        if (blInsertMod == true)
        {
            strCompanyCode = DrpCompany.SelectValue;
            strCustomerCode = txtCustomerCode.Text.Trim();
          
            //檢查是否有資料
            if (CheckData() == true)
            {
                if (ValidateData(strCompanyCode, strCustomerCode) == true)
                {
                    checki= UpdateData(true);
                    Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", strScript);
                    //JsUtility.ClientMsgBox("存檔成功！", this.Page, "");
                   
                    strCompanyCode = "";
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