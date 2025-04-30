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

public partial class GLA0410_M : System.Web.UI.Page
{
    string Ssql = "";
    string Ssql1 = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLA0410";
    DBManger _MyDBM;
    int saveon = 0;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    string strCompany="",strAcctNo=""  ;//, sDepositBank, sDepositBankAccount
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
            if (Request["Company"] != null && Request["Acctno"] != null)
            {
                strCompany = Request["Company"].Trim();
                strAcctNo = Request["Acctno"].Trim();
                btnSaveGo.Visible = false;
                btnSaveGo.Enabled = false;
            }

            else
            {
                strCompany = "";
                strAcctNo = "";
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
            BindEditData(strCompany,strAcctNo);

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
            DrpAcctType.Enabled = false;
            DrpAcctCtg.Enabled = false;
            TxtAcctNo.Attributes.Add("ReadOnly", "ReadOnly");
            TxtAcctCName.Attributes.Add("ReadOnly", "ReadOnly");
            TxtAcctEName.Attributes.Add("ReadOnly", "ReadOnly");
            txtAcctClass.Attributes.Add("ReadOnly", "ReadOnly");
            TxtASpecialAcct.Attributes.Add("ReadOnly", "ReadOnly");
            txtOffsetID.Attributes.Add("ReadOnly", "ReadOnly");
            txtIdx01.Attributes.Add("ReadOnly", "ReadOnly");
            TxtIdx02.Attributes.Add("ReadOnly", "ReadOnly");
            TxtIdx03.Attributes.Add("ReadOnly", "ReadOnly");
            TxtIdx04.Attributes.Add("ReadOnly", "ReadOnly");
            TxtIdx05.Attributes.Add("ReadOnly", "ReadOnly");
            TxtIdx06.Attributes.Add("ReadOnly", "ReadOnly");
            txtIdx07.Attributes.Add("ReadOnly", "ReadOnly");
            ChkInputctl1.Enabled = false;
            ChkInputctl2.Enabled = false;
            ChkInputctl3.Enabled = false;
            ChkInputctl4.Enabled = false;
            ChkInputctl5.Enabled = false;
            ChkInputctl6.Enabled = false;
            ChkInputctl7.Enabled = false;          
            TxtSeqctl1.Attributes.Add("ReadOnly", "ReadOnly");
            TxtSeqctl2.Attributes.Add("ReadOnly", "ReadOnly");
            TxtSeqctl3.Attributes.Add("ReadOnly", "ReadOnly");
            TxtSeqctl4.Attributes.Add("ReadOnly", "ReadOnly");
            TxtSeqctl5.Attributes.Add("ReadOnly", "ReadOnly");
            TxtSubledCode.Attributes.Add("ReadOnly", "ReadOnly");
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
           // DrpAcctType.Enabled = false;
           // DrpAcctCtg.Enabled = false;
            TxtAcctNo.Attributes.Add("ReadOnly", "ReadOnly");
           // TxtAcctCName.Attributes.Add("ReadOnly", "ReadOnly");
           // txtAcctClass.Attributes.Add("ReadOnly", "ReadOnly");
           // TxtASpecialAcct.Attributes.Add("ReadOnly", "ReadOnly");


        
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

        Ssql1 = "SELECT * FROM GLAcctDef WHERE Company=@Company AND AcctNo=@AcctNo";

        sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = strCompany;
        sqlcmd.Parameters.Add("@AcctNo", SqlDbType.Char).Value = strAccTNo;

         DT = _MyDBM.ExecuteDataTable(Ssql1,sqlcmd.Parameters,CommandType.Text);

        if (DT.Rows.Count > 0)
        {
            DrpCompany.SelectValue = DT.Rows[0]["Company"].ToString();
            TxtAcctNo.Text = DT.Rows[0]["AcctNo"].ToString();
            TxtAcctCName.Text = DT.Rows[0]["AcctDesc1"].ToString();
            TxtAcctEName.Text = DT.Rows[0]["AcctDesc2"].ToString();
            DrpAcctType.SelectedValue = DT.Rows[0]["AcctType"].ToString();
            DrpAcctCtg.SelectedValue = DT.Rows[0]["AcctCtg"].ToString();
            txtAcctClass.Text = DT.Rows[0]["AcctClass"].ToString();
            TxtASpecialAcct.Text = DT.Rows[0]["ASpecialAcct"].ToString();
            ChkInputctl1.Checked = DT.Rows[0]["Inputctl1"].ToString().Trim() == "V" ? true : false;
            ChkInputctl2.Checked = DT.Rows[0]["Inputctl1"].ToString().Trim() == "V" ? true : false;
            ChkInputctl3.Checked = DT.Rows[0]["Inputctl1"].ToString().Trim() == "V" ? true : false;
            ChkInputctl4.Checked = DT.Rows[0]["Inputctl1"].ToString().Trim() == "V" ? true : false;
            ChkInputctl5.Checked = DT.Rows[0]["Inputctl1"].ToString().Trim() == "V" ? true : false;
            ChkInputctl6.Checked = DT.Rows[0]["Inputctl1"].ToString().Trim() == "V" ? true : false;
            ChkInputctl7.Checked = DT.Rows[0]["Inputctl1"].ToString().Trim() == "V" ? true : false;          
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

            SqlCommand  sqlcmd = new SqlCommand();

            if (Mode == true)
            {

                //新增
                Ssql = @"INSERT INTO GLAcctDef (Company,AcctNo,AcctDesc1,AcctDesc2,AcctType,AcctCtg,AcctClass
              ,ASpecialAcct,OffsetID,Idx01,Idx02,Idx03,Idx04,Idx05,Idx06,Idx07,Inputctl,Inputctl1
              ,Inputctl2,Inputctl3,Inputctl4,Inputctl5,Inputctl6,Inputctl7,Seqctl,Seqctl1,Seqctl2,Seqctl3
              ,Seqctl4,Seqctl5,Subtotctl,Subtotctl1,Subtotctl2,Subtotctl3,Subtotctl4,Subtotctl5,SubledCode,LstChgUser,LstChgDateTime)
              VALUES (@Company,@AcctNo,@AcctDesc1,@AcctDesc2,@AcctType,@AcctCtg,@AcctClass,@ASpecialAcct,@OffsetID,@Idx01,@Idx02,
              @Idx03,@Idx04,@Idx05,@Idx06,@Idx07,@Inputctl,@Inputctl1,@Inputctl2,@Inputctl3,@Inputctl4,@Inputctl5,@Inputctl6,
              @Inputctl7,@Seqctl,@Seqctl1,@Seqctl2,@Seqctl3,@Seqctl4,@Seqctl5,@Subtotctl,
              @Subtotctl1,@Subtotctl2,@Subtotctl3,@Subtotctl4,@Subtotctl5,@SubledCode,@LstChgUser,@LstChgDateTime) ";


                //新增
                sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = DrpCompany.SelectValue;
                sqlcmd.Parameters.Add("@AcctNo", SqlDbType.Char).Value = TxtAcctNo.Text;
                sqlcmd.Parameters.Add("@AcctDesc1", SqlDbType.Char).Value = TxtAcctCName.Text;
                sqlcmd.Parameters.Add("@AcctDesc2", SqlDbType.Char).Value = TxtAcctEName.Text;
                sqlcmd.Parameters.Add("@AcctType", SqlDbType.Char).Value = DrpAcctType.SelectedValue;
                sqlcmd.Parameters.Add("@AcctCtg", SqlDbType.Char).Value = DrpAcctCtg.SelectedValue;
                sqlcmd.Parameters.Add("@AcctClass", SqlDbType.Char).Value = txtAcctClass.Text;
                sqlcmd.Parameters.Add("@ASpecialAcct", SqlDbType.Char).Value = TxtASpecialAcct.Text;
                sqlcmd.Parameters.Add("@OffsetID", SqlDbType.Char).Value = txtOffsetID.Text;
                sqlcmd.Parameters.Add("@Idx01", SqlDbType.Char).Value = txtIdx01.Text;
                sqlcmd.Parameters.Add("@Idx02", SqlDbType.Char).Value = TxtIdx02.Text;
                sqlcmd.Parameters.Add("@Idx03", SqlDbType.Char).Value = TxtIdx03.Text;
                sqlcmd.Parameters.Add("@Idx04", SqlDbType.Char).Value = TxtIdx04.Text;
                sqlcmd.Parameters.Add("@Idx05", SqlDbType.Char).Value = TxtIdx05.Text;
                sqlcmd.Parameters.Add("@Idx06", SqlDbType.Char).Value = TxtIdx06.Text;
                sqlcmd.Parameters.Add("@Idx07", SqlDbType.Char).Value = txtIdx07.Text;
                sqlcmd.Parameters.Add("@Inputctl", SqlDbType.Char).Value = "";
                sqlcmd.Parameters.Add("@Inputctl1", SqlDbType.Char).Value = ChkInputctl1.Checked ? "V":" ";
                sqlcmd.Parameters.Add("@Inputctl2", SqlDbType.Char).Value = ChkInputctl2.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@Inputctl3", SqlDbType.Char).Value = ChkInputctl3.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@Inputctl4", SqlDbType.Char).Value = ChkInputctl4.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@Inputctl5", SqlDbType.Char).Value = ChkInputctl5.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@Inputctl6", SqlDbType.Char).Value = ChkInputctl6.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@Inputctl7", SqlDbType.Char).Value = ChkInputctl7.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@Seqctl", SqlDbType.Decimal).Value = 0;
                sqlcmd.Parameters.Add("@Seqctl1", SqlDbType.Decimal).Value =TxtSeqctl1.Text.Trim()!=""? decimal.Parse(TxtSeqctl1.Text):0;
                sqlcmd.Parameters.Add("@Seqctl2", SqlDbType.Decimal).Value =TxtSeqctl2.Text.Trim()!=""? decimal.Parse(TxtSeqctl2.Text):0;
                sqlcmd.Parameters.Add("@Seqctl3", SqlDbType.Decimal).Value =TxtSeqctl3.Text.Trim()!=""? decimal.Parse(TxtSeqctl3.Text):0;
                sqlcmd.Parameters.Add("@Seqctl4", SqlDbType.Decimal).Value =TxtSeqctl4.Text.Trim()!=""? decimal.Parse(TxtSeqctl4.Text):0;
                sqlcmd.Parameters.Add("@Seqctl5", SqlDbType.Decimal).Value =TxtSeqctl5.Text.Trim()!=""? decimal.Parse(TxtSeqctl5.Text):0;
                sqlcmd.Parameters.Add("@Subtotctl", SqlDbType.Char).Value = "";
                sqlcmd.Parameters.Add("@Subtotctl1", SqlDbType.Char).Value = ChkSubtotctl1.Checked? "V":" ";
                sqlcmd.Parameters.Add("@Subtotctl2", SqlDbType.Char).Value = ChkSubtotctl2.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@Subtotctl3", SqlDbType.Char).Value = ChkSubtotctl3.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@Subtotctl4", SqlDbType.Char).Value = ChkSubtotctl4.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@Subtotctl5", SqlDbType.Char).Value = ChkSubtotctl5.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@SubledCode", SqlDbType.Char).Value = TxtSubledCode.Text;
                sqlcmd.Parameters.Add("@LstChgUser", SqlDbType.Char).Value = "";
                sqlcmd.Parameters.Add("@LstChgDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.ToShortDateString();
            }
            else
            {

                //修改
                Ssql = @" UPDATE GLAcctDef SET AcctDesc2=@AcctDesc2,OffsetID=@offsetID,Idx01=@idx01,Idx02=@idx02,Idx03=@idx03,Idx04=@idx04,Idx05=@idx05,Idx06=@idx06,
                 Idx07=@idx07,Inputctl=@Inputctl,Inputctl1=@inputctl1,Inputctl2=@inputctl2,Inputctl3=@inputctl3,Inputctl4=@inputctl4,
                  Inputctl5=@inputctl5,Inputctl6=@inputctl6,Inputctl7=@inputctl7,Seqctl=@seqctl,Seqctl1=@seqctl1,Seqctl2=@seqctl2,Seqctl3=@seqctl3
                 ,Seqctl4=@seqctl4,Seqctl5=@seqctl5,Subtotctl=@subtotctl,Subtotctl1=@subtotctl1,Subtotctl2=@subtotctl2,Subtotctl3=@subtotctl3
                 ,Subtotctl4=@subtotctl4,Subtotctl5=@subtotctl5,SubledCode=@SubledCode,LstChgUser=@LstChgUser,LstChgDateTime=@LstChgDateTime Where Company=@company AND AcctNo=@AcctNo ";

               

                sqlcmd.Parameters.Add("@AcctDesc2", SqlDbType.Char).Value = TxtAcctEName.Text;
                sqlcmd.Parameters.Add("@OffsetID", SqlDbType.Char).Value = txtOffsetID.Text;
                sqlcmd.Parameters.Add("@Idx01", SqlDbType.Char).Value = txtIdx01.Text;
                sqlcmd.Parameters.Add("@Idx02", SqlDbType.Char).Value = TxtIdx02.Text;
                sqlcmd.Parameters.Add("@Idx03", SqlDbType.Char).Value = TxtIdx03.Text;
                sqlcmd.Parameters.Add("@Idx04", SqlDbType.Char).Value = TxtIdx04.Text;
                sqlcmd.Parameters.Add("@Idx05", SqlDbType.Char).Value = TxtIdx05.Text;
                sqlcmd.Parameters.Add("@Idx06", SqlDbType.Char).Value = TxtIdx06.Text;
                sqlcmd.Parameters.Add("@Idx07", SqlDbType.Char).Value = txtIdx07.Text;
                sqlcmd.Parameters.Add("@inputctl", SqlDbType.Char).Value = "";
                sqlcmd.Parameters.Add("@inputctl1", SqlDbType.Char).Value = ChkInputctl1.Checked ? "V" : " "; 
                sqlcmd.Parameters.Add("@inputctl2", SqlDbType.Char).Value = ChkInputctl2.Checked ? "V" : " "; 
                sqlcmd.Parameters.Add("@inputctl3", SqlDbType.Char).Value = ChkInputctl3.Checked ? "V" : " "; 
                sqlcmd.Parameters.Add("@inputctl4", SqlDbType.Char).Value = ChkInputctl4.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@inputctl5", SqlDbType.Char).Value = ChkInputctl5.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@inputctl6", SqlDbType.Char).Value = ChkInputctl6.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@inputctl7", SqlDbType.Char).Value = ChkInputctl7.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@seqctl", SqlDbType.Decimal).Value = 0;
                sqlcmd.Parameters.Add("@seqctl1", SqlDbType.Decimal).Value =TxtSeqctl1.Text.Trim()!=""? decimal.Parse(TxtSeqctl1.Text):0;
                sqlcmd.Parameters.Add("@seqctl2", SqlDbType.Decimal).Value = TxtSeqctl2.Text.Trim()!=""?decimal.Parse(TxtSeqctl2.Text):0;
                sqlcmd.Parameters.Add("@seqctl3", SqlDbType.Decimal).Value = TxtSeqctl3.Text.Trim()!=""?decimal.Parse(TxtSeqctl3.Text):0;
                sqlcmd.Parameters.Add("@seqctl4", SqlDbType.Decimal).Value = TxtSeqctl4.Text.Trim()!=""?decimal.Parse(TxtSeqctl4.Text):0;
                sqlcmd.Parameters.Add("@seqctl5", SqlDbType.Decimal).Value = TxtSeqctl5.Text.Trim()!=""?decimal.Parse(TxtSeqctl5.Text):0;
                sqlcmd.Parameters.Add("@subtotctl", SqlDbType.Char).Value = "";
                sqlcmd.Parameters.Add("@subtotctl1", SqlDbType.Char).Value = ChkSubtotctl1.Checked ? "V":" ";
                sqlcmd.Parameters.Add("@subtotctl2", SqlDbType.Char).Value = ChkSubtotctl2.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@subtotctl3", SqlDbType.Char).Value = ChkSubtotctl3.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@subtotctl4", SqlDbType.Char).Value = ChkSubtotctl4.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@subtotctl5", SqlDbType.Char).Value = ChkSubtotctl5.Checked ? "V" : " ";
                sqlcmd.Parameters.Add("@SubledCode", SqlDbType.Char).Value = TxtSubledCode.Text;
                sqlcmd.Parameters.Add("@LstChgUser", SqlDbType.Char).Value = "";
                sqlcmd.Parameters.Add("@LstChgDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.ToShortDateString();
                sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = DrpCompany.SelectValue;
                sqlcmd.Parameters.Add("@AcctNo", SqlDbType.Char).Value = TxtAcctNo.Text;


               
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
      if (TxtAcctNo.Text.Trim() == "")
      {

          strMessage += "．必須填入會計科目<br>";
          ierro++;
      }
      else if(TxtAcctNo.Text.Trim().Length>8)
      {

          strMessage += "．會計科目不得超過8碼<br>";
          ierro++;
      }
        
        //會計科目中文名稱
      if (TxtAcctCName.Text.Trim() == "")
      {

          strMessage += "．必須填入會計科目<br>";
          ierro++;
      }
      else if (TxtAcctCName.Text.Trim().Length > 32)
      {

          strMessage += "．會計科目不得超過32個字<br>";
          ierro++;
      }
        
        //科目等級
      if (txtAcctClass.Text.Trim() == "")
      {

          strMessage += "．必須填入科目等級<br>";
          ierro++;
      }
      else if (txtAcctClass.Text.Trim().Length > 1)
      {

          strMessage += "．會計等級不得超過1個字<br>";
          ierro++;
      }
        
        //特定科目
      if (TxtASpecialAcct.Text.Trim() == "")
      {

          strMessage += "．必須填入特定<br>";
          ierro++;
      }
      else if (TxtASpecialAcct.Text.Trim().Length > 1)
      {
        
          strMessage += "．特定科目不得超過1個字 <br>";
          ierro++;
      }
        
        //沖帳代碼
      if (txtOffsetID.Text.Trim() != "")
      {
          if (txtOffsetID.Text.Trim().Length > 2)
          {

              strMessage += "．沖帳代碼不得超過2個字 <br>";
              ierro++;
          }     
      }
        
        //成本中心
      if (txtIdx01.Text.Trim() != "")
      {
          if (txtIdx01.Text.Trim().Length > 2)
          {
              strMessage += "．成本中心不得超過2個字 <br>";
              ierro++;
          }
      }

        //對象別
      if (TxtIdx02.Text.Trim() != "")
      {
          if (TxtIdx02.Text.Trim().Length > 2)
          {

              strMessage += "．對象別不得超過2個字<br>";
              ierro++;
          }
      }
        //相關號碼1
      if (TxtIdx03.Text.Trim() != "")
      {
          if (TxtIdx03.Text.Trim().Length > 2)
          {

              strMessage += "．相關號碼1不得超過2個字<br>";
              ierro++;
          }
      }

        //相關號碼2
      if (TxtIdx04.Text.Trim() != "")
      {
          if (TxtIdx04.Text.Trim().Length > 2)
          {

              strMessage += "．相關號碼2不得超過2個字 <br>";
              ierro++;
          }
      }
        //日期
      if (TxtIdx05.Text.Trim() != "")
      {
          if (TxtIdx05.Text.Trim().Length > 2)
          {
              ierro++;
              strMessage += "．日期不得超過2個字<br>";
          }
      }

        //數量
      if (TxtIdx06.Text.Trim() != "")
      {
          if (TxtIdx06.Text.Trim().Length > 2)
          {
              ierro++;
              strMessage += "．數量不得超過2個字 <br>";
          }
      }
        //其他
      if (txtIdx07.Text.Trim() != "")
      {
          if (txtIdx07.Text.Trim().Length > 2)
          {
              ierro++;
              strMessage += "．其他不得超過2個字<br>";
          }
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

    private bool ValidateData(string strCompany, string strAcctNo)
    {
        //判斷資料是否重覆
        Ssql = "SELECT * FROM GLAcctDef WHERE Company = '" + strCompany + "' And Acctno='" + strAcctNo + "'";

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
                strAcctNo = TxtAcctNo.Text;
                //檢查是否有資料
                if (ValidateData(strCompany, strAcctNo) == true)
                {
                    UpdateData(true);

                }

                else
                {
                    //秀錯誤訊息
                    JsUtility.ClientMsgBox("會計科目已存在！", this.Page, "");

                }
            }
            strAcctNo = "";
            strCompany = "";

           
        }


    }



    protected void btnSaveExit_Click(object sender, ImageClickEventArgs e)
    {
        //新增與修改

        string strScript = "";

        strScript = "<script language=javascript>";
        strScript += "window.opener.location='GLA0410.aspx?Company=" + strCompany + "';";
        strScript+="window.close();</script> " ;



        if (blInsertMod == true)
        {
            strCompany = DrpCompany.SelectValue;
            strAcctNo = TxtAcctNo.Text;
            //檢查是否有資料
            if (CheckData() == true)
            {
                if (ValidateData(strCompany, strAcctNo) == true)
                {
                    UpdateData(true);
                    Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", strScript);
                    JsUtility.ClientMsgBox("存檔成功！", this.Page, "");
                    strAcctNo = "";
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