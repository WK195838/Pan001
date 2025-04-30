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

public partial class GLA0350    : System.Web.UI.Page
{
    string Ssql = "";   
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLA0350";
    DBManger _MyDBM;
    int saveon = 0;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    string sCompany="", sReportType="",sReportID="";
   // Payroll py = new Payroll();
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
    private void AuthRight(GridView thisGridView)
    {
        //驗證權限
        bool Find = false;
        bool SetCss = false;
        int i = 0;

        string[] Auth = { "Delete", "Modify", "Add" };

        if (ConfigurationManager.AppSettings["EnableProgramAuth"] != null && ConfigurationManager.AppSettings["EnableProgramAuth"].ToString() == "true")
        {
            for (i = 0; i < Auth.Length; i++)
            {
                Find = _UserInfo.CheckPermission(_ProgramId, Auth[i]);
                if (i < (Auth.Length - 1))
                {//刪/修/詳
                    thisGridView.Columns[i].Visible = Find;
                    //設定標題樣式
                    if (Find && (SetCss == false))
                    {
                        SetCss = true;
                        thisGridView.Columns[i].HeaderStyle.CssClass = "paginationRowEdgeLl";
                    }
                }
                else
                {//新增
                    thisGridView.ShowFooter = Find;
                }
            }

            //因為是附加在頁籤的功能,所以一定可以查詢
            ////查詢(執行)
            //if ((_UserInfo.CheckPermission(_ProgramId)) || Find)
            //{
            //    Find = true;
            //}
            //else
            //{
            //    Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
            //}

            //版面樣式調整
            if (SetCss == false)
            {
                thisGridView.Columns[(Auth.Length - 1)].HeaderStyle.CssClass = "paginationRowEdgeLl";
            }

        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {        
        lbl_Msg.Text = "";
        lbl_Msg2.Text = "";
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



        if (Request["Company"] != null && Request["ReportType"] != null && Request["ReportID"] != null)
        {
            sCompany = Request["Company"].Trim();
            sReportType = Request["ReportType"].Trim();
            sReportID = Request["ReportID"].Trim();
        }
        else
        {
            blInsertMod = true;
            listview.Visible = false;
            btnMedit.Visible = false;
        }

        btnSaveExit.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='EXIT';");
        btnCancel.Attributes.Add("onclick", "javascript:window.close();");

        if (Request["Kind"] != null)
        {
            blReadOnly = Request["Kind"].Equals("Query");
        }

        //DetailsView1.DefaultMode = (blInsertMod ? DetailsViewMode.Insert : DetailsViewMode.Edit);

        if (!Page.IsPostBack)
        {
            hid_IsInsertExit.Value = "";
            saveon = 0;
            CompanyList1.SelectValue = "20";
            bindReportType();
            BindMasterData( sCompany,sReportType,sReportID);
            BindDetailData();

        }
        else
        {

            if (Request.Form["__EVENTARGUMENT"] != null)
            {

                if (Request.Form["__EVENTARGUMENT"].ToString().Contains("Edit"))
                {
                    BindMasterData(sCompany, sReportType, sReportID);
                    BindDetailData();
                }
                else if (!string.IsNullOrEmpty(hid_IsInsertExit.Value))
                {

                    string ddlId = hid_IsInsertExit.Value.Replace("_", "$") + "$btAddNew";
                    if (Request.Form[ddlId + ".x"] != null)
                    {
                        //新增
                        btnEmptyNew_Click(sender, e);
                        hid_IsInsertExit.Value = "";
                        BindDetailData();

                    }
                }
                else
                {
                    string openbtnID = "";
                    foreach (string str in Request.Form)
                    {
                        if (str != null)
                        {
                            Control c = Page.FindControl(str.Replace(".x", "")); ;
                            if (c is ImageButton)
                            {
                                openbtnID = c.ID;
                                break;
                            }
                        }
                    }

                    if (openbtnID == "btnSaveExit" || openbtnID == "btnMedit")
                    {
                        BindDetailData();
                    }
                    
                    //   BindDetailData();

                }
            }

        }
        Navigator1.BindGridView = GridView1;
        //InsertBaseItem();
        //hid_IsInsertExit.Value = "";

        #region 查詢顯示控管
        if (blReadOnly)
        { 
        }
        #endregion
    }

    /// <summary>
    /// 取得報表種類
    /// </summary>
    private void bindReportType()
    {
        _MyDBM = new DBManger();
        _MyDBM.New();

        string strSQL = "select vReportType.ReportType,vReportType.TypeAndDesc from vReportType";

        DataTable dt = _MyDBM.ExecuteDataTable(strSQL);

        DrpReportType.DataSource = dt;
        DrpReportType.DataValueField = "ReportType";
        DrpReportType.DataTextField = "TypeAndDesc";
        DrpReportType.DataBind();
       
    } 



   

  
    private bool ValidateData2(string Company, string ReportType,string ReportID,string SeqNo,string AcctNoStart)
    {
        SqlCommand sqlcmd = new SqlCommand();

        Ssql = @"Select * FROM GLReportDefDetail 
               WHERE Company=@company AND ReportType=@ReportType AND
               ReportID=@ReportID AND SeqNo=@SeqNo AND AcctNoStart=@AcctNoStart";

        sqlcmd.Parameters.Add("@company", SqlDbType.Char).Value = Company;
        sqlcmd.Parameters.Add("@ReportType", SqlDbType.Char).Value = ReportType;
        sqlcmd.Parameters.Add("@ReportID", SqlDbType.Char).Value = ReportID;
        sqlcmd.Parameters.Add("@SeqNo", SqlDbType.Char).Value = SeqNo;
        sqlcmd.Parameters.Add("@AcctNoStart", SqlDbType.Char).Value = AcctNoStart;
        

        DataTable tb = _MyDBM.ExecuteDataTable(Ssql,sqlcmd.Parameters,CommandType.Text);

        if (tb.Rows.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }  

   

    /// <summary>
    /// 關閉主檔寫入功能
    /// </summary>
    private void disableitem()
    {
        CompanyList1.Enabled = false;
        DrpReportType.Enabled = false;
        txtRpeortID.Enabled=false;
        txtReportName.Enabled=false;
        DrpFromat.Enabled = false;
    }


    /// <summary>
    /// 開啟主檔寫入功能
    /// </summary>
    private void Enableitem()
    {
       
        txtReportName.Enabled=true;
        DrpFromat.Enabled = true;
    }




    protected void btnSave_Click(object sender, ImageClickEventArgs e)
    {
        SaveMasterdata(blInsertMod);
       

    }

    private void BindMasterData(string strcompany,string strreporttype,string strreportID)
    {
        string strSQL = @"SELECT * FROM GLREportDefHead WHERE company= @company
         AND ReportType=@ReportType AND ReportID=@ReportID  ";

        SqlCommand sqlcmd = new SqlCommand();

        sqlcmd.Parameters.Add("@company",SqlDbType.Char).Value=strcompany;
        sqlcmd.Parameters.Add("@ReportType", SqlDbType.Char).Value = strreporttype;
        sqlcmd.Parameters.Add("@ReportID", SqlDbType.Char).Value = strreportID;

        DataTable Dt = _MyDBM.ExecuteDataTable(strSQL,sqlcmd.Parameters,CommandType.Text);

        //固定資料
        if (Dt.Rows.Count > 0)
        {
            CompanyList1.SelectValue = Dt.Rows[0]["company"].ToString();
            DrpReportType.SelectedValue = Dt.Rows[0]["ReportType"].ToString();
            txtRpeortID.Text = Dt.Rows[0]["ReportID"].ToString();
            txtReportName.Text = Dt.Rows[0]["ReportName"].ToString();
            DrpFromat.SelectedValue = Dt.Rows[0]["FormatID"].ToString();
            disableitem();
            btnSaveExit.Visible = false;
        }


    
    }


    private void SaveMasterdata(bool savemode )
    { 
     //用savemode決定新增或更新
        _MyDBM = new DBManger();
        _MyDBM.New();

        SqlCommand sqlcmd = new SqlCommand();
        string strSQL = "";

        //檢查資料
        
        //報表代號

        if (txtRpeortID.Text.Trim() == "")
        {
            JsUtility.ClientMsgBox("請填報表代號！", this.Page, "");
            return;
        }
        else if(txtRpeortID.Text.Trim().Length>3)
        {
            JsUtility.ClientMsgBox("報表代號不得大於3碼！", this.Page, "");
            return;
        }
        
        //報表名稱
        if (txtReportName.Text.Trim() == "")
        { 
          JsUtility.ClientMsgBox("請填報表名稱！", this.Page, "");
            return;
        }
        else if (txtRpeortID.Text.Trim().Length > 32)
        {
            JsUtility.ClientMsgBox("報表名稱長度不可超過32個字！", this.Page, "");
            return;
        }
        




        //新增
        if (savemode)
        {
            strSQL = @"INSERT INTO GLReportDefHead 
         (Company,ReportType,ReportID,ReportName,FormatID,LstChgUser,LstChgDateTime )
         VALUES
         (@Company,@ReportType,@ReportID,@ReportName,@FormatID,@LstChgUser,@LstChgDateTime )";


            if (ValidateData(CompanyList1.SelectValue,DrpReportType.SelectedValue,txtRpeortID.Text.Trim() )==true)
            {
                //秀出已有相關資料訊息
                JsUtility.ClientMsgBox("資料已存在！", this.Page, "");

                return ;
            }

           
        }
        else
        {
            strSQL = @"UPDATE GLReportDefHead SET ReportName=@ReportName,
                     FormatID=@FormatID,LstChgUser=@LstChgUser,LstChgDateTime=@LstChgDateTime
                     WHERE Company=@Company AND ReportType=@ReportType AND ReportID=@ReportID";        
        }
        

        sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = CompanyList1.SelectValue;
        sqlcmd.Parameters.Add("@ReportType", SqlDbType.Char).Value = DrpReportType.SelectedValue;
        sqlcmd.Parameters.Add("@ReportID", SqlDbType.Char).Value = txtRpeortID.Text;
        sqlcmd.Parameters.Add("@ReportName", SqlDbType.NVarChar).Value = txtReportName.Text;
        sqlcmd.Parameters.Add("@FormatID", SqlDbType.Char).Value = DrpFromat.SelectedValue;
        sqlcmd.Parameters.Add("@LstChgUser", SqlDbType.VarChar).Value = _UserInfo.UData.UserId;
        sqlcmd.Parameters.Add("@LstChgDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.ToShortDateString();


        _MyDBM.ExecuteCommand(strSQL, sqlcmd.Parameters, CommandType.Text);
        listview.Visible = true;
        disableitem();
        btnSaveExit.Visible = false;
        btnMedit.Visible = true;
    
    }
    



    private void BindDetailData()
    {


        SDS_GridView.SelectParameters.Clear();
       
        SDS_GridView.SelectParameters.Add("company",DbType.StringFixedLength,CompanyList1.SelectValue);
        SDS_GridView.SelectParameters.Add("ReportType", DbType.StringFixedLength,DrpReportType.SelectedValue);
        SDS_GridView.SelectParameters.Add("ReportID", DbType.StringFixedLength, txtRpeortID.Text);
          

       
        GridView1.DataBind();
        //Navigator1.BindGridView = GridView1;
        //Navigator1.DataBind();
    }

    //新增

    protected void btnEmptyNew_Click(object sender, EventArgs e)
    {
        //GridView1$ctl12$tmpAddtxt01
        //GridView1$ctl12$TmpDrpPMcode
        //GridView1$ctl12$tmpAddtxt03
        //GridView1$ctl12$tmpAddtxt04
        //GridView1$ctl12$TmpchkSumTotal
        //GridView1$ctl12$TmpchkPrint
        //GridView1$ctl12$tmpAddtxt07
        //GridView1$ctl12$tmpAddtxt08

        string HeadID = hid_IsInsertExit.Value.Replace("_", "$");
        string strseqNo = Request.Form[HeadID + "$txtaddseq"] != null ? Request.Form[HeadID + "$txtaddseq"].ToString().Trim() : "";
        string strAcctstartNo = Request.Form[HeadID + "$txtaddAcctNoFrom"] != null ? Request.Form[HeadID + "$txtaddAcctNoFrom"].ToString().Trim() : "";
        string strAcctEndNo = Request.Form[HeadID + "$txtaddAcctNoEnd"] != null ? Request.Form[HeadID + "$txtaddAcctNoEnd"] : "";
        string strPMCode=Request.Form[HeadID + "$DrpPMcode"]!=null ? Request.Form[HeadID + "$DrpPMcode"].ToString() :"";
        string strTotalCode="";
        string strPrintcode="";
        string stritemDesc = Request.Form[HeadID + "$txtadditem"] != null ? Request.Form[HeadID + "$txtadditem"] : "";
        string strexpensecode = Request.Form[HeadID + "$txtexpensecode"] != null ? Request.Form[HeadID + "$txtexpensecode"] : "";

         


        //新增資料

        //檢查欄位是否正確

        //序號
        if (strseqNo == "")
        {
            JsUtility.ClientMsgBox("必須輸入序號！", this.Page, "");
            return;

        }
        else if(strseqNo.Length>4)
        {
            JsUtility.ClientMsgBox("序號必須在4碼內！", this.Page, "");
            return;        
        }
        
        //開始

        if (strAcctstartNo == "")
        {
            JsUtility.ClientMsgBox("必須輸入起始會計號碼！", this.Page, "");
            return;

        }
        else if (strAcctstartNo.Length > 8)
        {
            JsUtility.ClientMsgBox("會計號碼起必須在8碼內！", this.Page, "");
            return;
        }
        
        //結束
        if (strAcctEndNo.Length>8)
        {
            JsUtility.ClientMsgBox("會計科目迄必須在8碼內！", this.Page, "");
            return;

        }
        //項目

        if (stritemDesc.Length>50)
        {
            JsUtility.ClientMsgBox("項目名稱不得超過50個字！", this.Page, "");
            return;

        }

        //費用別

        if (strexpensecode.Length > 10)
        {
            JsUtility.ClientMsgBox("部門費用別不得超過10個字！", this.Page, "");
            return;

        }







        //檢查細項資料是否存在
        //false代表有資料
        if (ValidateData2(sCompany,sReportType,sReportID,strseqNo,strAcctstartNo)==false)
        {
            JsUtility.ClientMsgBox("資料已存在！", this.Page, "");
            return;
        }

       




        if (hid_IsInsertExit.Value != "")
        {
           

            if (Request.Form[HeadID + "$chksumcode"] != null)
             {
                 if (Request.Form[HeadID + "$chksumcode"].ToString() == "on")
              {
               strTotalCode="V";             
              }
             
             }

             if (Request.Form[HeadID + "$chkprintcode"] != null)
             {
                 if (Request.Form[HeadID + "$chkprintcode"].ToString() == "on")
              {
               strPrintcode="V";             
              }
             
             }
                     




            //新增
            if (lbl_Msg2.Text == "")
            {
                SDS_GridView.InsertParameters.Clear();
                SDS_GridView.InsertParameters.Add("Company", sCompany);
                SDS_GridView.InsertParameters.Add("ReportType", sReportType);
                SDS_GridView.InsertParameters.Add("ReportID", sReportID);
                SDS_GridView.InsertParameters.Add("seqNo",strseqNo);
                SDS_GridView.InsertParameters.Add("PMCode",DbType.Decimal,strPMCode  );
                SDS_GridView.InsertParameters.Add("AcctNoStart",strAcctstartNo );
                SDS_GridView.InsertParameters.Add("AcctNoEnd", strAcctEndNo);
                SDS_GridView.InsertParameters.Add("sunTotcode",strTotalCode );
                SDS_GridView.InsertParameters.Add("PrtDetailCode", strPrintcode );
                SDS_GridView.InsertParameters.Add("ItemDesc", stritemDesc);
                SDS_GridView.InsertParameters.Add("ExpenseCode", strexpensecode);
                SDS_GridView.InsertParameters.Add("LstChgUser", _UserInfo.UData.UserId);
                SDS_GridView.InsertParameters.Add("LstChgDateTime",DateTime.Now.ToShortDateString() );
                

                #region 開始異動前,先寫入LOG
                //TableName	異動資料表	varchar	60
                //TrxType	異動類別(A/U/D)	char	1
                //ChangItem	異動項目(欄位)	varchar	255
                //SQLcommand	異動項目(異動值/指令)	varchar	2000
                //ChgStartDateTime	異動開始時間	smalldatetime	
                //ChgStopDateTime	異動結束時間	smalldatetime	
                //ChgUser	使用者代號	nvarchar	32
                DateTime StartDateTime = DateTime.Now;
                MyCmd.Parameters.Clear();
                MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "GLReportDefDetail";
                MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "A";
                MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
                MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
                MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
                //此時不設定異動結束時間
                //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
                MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
                _MyDBM.DataChgLog(MyCmd.Parameters);
                #endregion

                int i = 0;
                try
                {
                    i = SDS_GridView.Insert();
                }
                catch (Exception ex)
                {
                    lbl_Msg2.Text = ex.Message;
                }
                if (i == 1)
                {
                    JsUtility.ClientMsgBox(i.ToString() + " 個資料列 " + "新增成功!!", this.Page, "");
                    //lbl_Msg2.Text = i.ToString() + " 個資料列 " + "新增成功!!";
                    //DetailsView1.InsertItem(true);
                }
                else
                {
                   // lbl_Msg2.Text = "新增失敗!!" + lbl_Msg2.Text;
                    JsUtility.ClientMsgBox("新增失敗!!", this.Page, "");

                }

                #region 完成異動後,更新LOG資訊
                MyCmd.Parameters["@SQLcommand"].Value = (i == 1) ? "Success" : lbl_Msg2.Text;
                MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
                _MyDBM.DataChgLog(MyCmd.Parameters);
                #endregion

                //BindData();
                //showPanel();
            }
            else
            {
                //BindData();
            }
        }
        //hid_IsInsertExit.Value = "";
    } 
   
    private bool ValidateData(string strCompany, string strReportType, string strReportID)
    {
        //判斷資料是否重覆
        Ssql = @"SELECT * FROM GLReportDefHead WHERE Company=@Company AND ReportType=@ReportType
              AND ReportID=@ReportID";

        SqlCommand sqlcmd = new SqlCommand();

        sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = strCompany;
        sqlcmd.Parameters.Add("@ReportType", SqlDbType.Char).Value = strReportType;
        sqlcmd.Parameters.Add("@ReportID", SqlDbType.Char).Value = strReportID;


        DataTable tb = _MyDBM.ExecuteDataTable(Ssql,sqlcmd.Parameters,CommandType.Text);

        if (tb.Rows.Count == 0)
        {
            return false;
        }
        return true;
    }

    protected void btnDelete_Click(object sender, EventArgs e)
   {
        LinkButton btnDelete = (LinkButton)sender;
               
        string L2PK = btnDelete.Attributes["L2PK"].ToString().Trim();
        string L3PK = btnDelete.Attributes["L3PK"].ToString().Trim();
        SqlCommand sqlcmd = new SqlCommand();
 

        #region 開始異動前,先寫入LOG
        //TableName	異動資料表	varchar	60
        //TrxType	異動類別(A/U/D)	char	1
        //ChangItem	異動項目(欄位)	varchar	255
        //SQLcommand	異動項目(異動值/指令)	varchar	2000
        //ChgStartDateTime	異動開始時間	smalldatetime	
        //ChgStopDateTime	異動結束時間	smalldatetime	
        //ChgUser	使用者代號	nvarchar	32
        DateTime StartDateTime = DateTime.Now;
        MyCmd.Parameters.Clear();
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "GLReportDefDetail";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "D";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion
        

        string sql = @" DELETE  FROM GLReportDefDetail 
                     WHERE Company=@company AND ReportType=@ReportType AND
                     ReportID=@ReportID AND SeqNo=@SeqNo AND AcctNoStart=@AcctNoStart";

        sqlcmd.Parameters.Add("@company", SqlDbType.Char).Value = sCompany;
        sqlcmd.Parameters.Add("@ReportType", SqlDbType.Char).Value = sReportType;
        sqlcmd.Parameters.Add("@ReportID", SqlDbType.Char).Value = sReportID;
        sqlcmd.Parameters.Add("@SeqNo", SqlDbType.Char).Value = L2PK;
        sqlcmd.Parameters.Add("@AcctNoStart", SqlDbType.Char).Value = L3PK;


        int result = _MyDBM.ExecuteCommand(sql,sqlcmd.Parameters,CommandType.Text);

        if (result > 0)
        {
            lbl_Msg2.Text = "資料刪除成功 !!";
            JsUtility.ClientMsgBox("資料刪除成功!!", this.Page, "");

            //Navigator1.DataBind();
        }
        else
        {
            lbl_Msg2.Text = "資料刪除失敗 !!";
            JsUtility.ClientMsgBox("資料刪除失敗!!", this.Page, "");

        }

        #region 完成異動後,更新LOG資訊
        MyCmd.Parameters["@SQLcommand"].Value = (result > 0) ? "Success" : lbl_Msg2.Text;
        MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        BindDetailData();
        //showPanel();
    }

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].CssClass = "paginationRowEdgeLl";
            e.Row.Cells[e.Row.Cells.Count - 1].CssClass = "paginationRowEdgeRl";
        }
        else if ((e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowType == DataControlRowType.Footer) || (e.Row.RowType == DataControlRowType.EmptyDataRow))
        {
            e.Row.Attributes.Add("onmouseover", "setnewcolor(this);");
            e.Row.Attributes.Add("onmouseout", "setoldcolor(this);");
            int i = 0;
            for (i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].CssClass = "Grid_GridLine";
            }

            //i = e.Row.Cells.Count - 1;
            //if (i > 0)
            //{
            //    e.Row.Cells[i - 1].Style.Add("text-align", "right");
            //    e.Row.Cells[i - 1].Style.Add("width", "100px");
            //}
            //e.Row.Cells[i].Style.Add("text-align", "left");

        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {
            //e.Row.Style.Add("display", "none");
            e.Row.Visible = false;
        }
    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        //BindData();
    }
    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        //BindData();
    }

    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string Err = "", tempOldValue = "";
        string UpdateItem = "", UpdateValue = "";
        string strsunTotcode = "", strPrintcode = "";

        

        UpdateValue = "(Key=";
        //將此筆資料的KEY值找出
        for (int i = 0; i < e.Keys.Count; i++)
        {
            //e.Keys[i] = e.Keys[i].ToString().Substring(0, 2).Trim();
            UpdateValue += e.Keys[i].ToString().Trim() + "|";
        }
        UpdateValue += ")";

        DropDownList drppmcode = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("DrpeditPMcode");
        CheckBox chktotal = (CheckBox)GridView1.Rows[e.RowIndex].FindControl("chkeditsuntot");
        CheckBox chkprint = (CheckBox)GridView1.Rows[e.RowIndex].FindControl("chkeditPrint");

        strsunTotcode = chktotal.Checked == true ? "V" : "";
        strPrintcode = chkprint.Checked == true ? "V" : "";


        e.NewValues["PMCode"] = drppmcode.SelectedValue;
        e.NewValues["SunTotcode"] = strsunTotcode;
        e.NewValues["PrtDetailCode"] = strPrintcode;
                

       
        //結束會計號碼
        if (e.NewValues["AcctNoEnd"].ToString().Trim().Length > 8)
        {
            lbl_Msg2.Text = "結束號碼不得超過8個字!!";
            e.Cancel = true;
            return;
        }

        //項目名稱

        if (e.NewValues["ItemDesc"].ToString().Trim().Length > 50)
        {
            lbl_Msg2.Text = "項目名稱不得超過50個字!!";
            e.Cancel = true;
            return;
        }
                
        
        //部門費用別
        if (e.NewValues["ExpenseCode"].ToString().Trim().Length > 10)
        {
            lbl_Msg2.Text = "項目名稱不得超過10個字!!";
            e.Cancel = true;
            return;
        }



        
        SDS_GridView.UpdateParameters.Clear();

       // SDS_GridView.UpdateParameters.Add("PMCode", drppmcode.SelectedValue);
      //  SDS_GridView.UpdateParameters.Add("AcctNoEnd",e.NewValues[2].ToString().Trim());
      //  SDS_GridView.UpdateParameters.Add("SunTotcode", strsunTotcode);
       // SDS_GridView.UpdateParameters.Add("PrtDetailCode", strPrintcode);
      //  SDS_GridView.UpdateParameters.Add("ItemDesc", e.NewValues[5].ToString().Trim());
      //  SDS_GridView.UpdateParameters.Add("ExpenseCode", e.NewValues[6].ToString().Trim());      
        SDS_GridView.UpdateParameters.Add("LstChgUser", _UserInfo.UData.UserId);
        SDS_GridView.UpdateParameters.Add("LstChgDateTime",DateTime.Now.ToShortDateString());
        SDS_GridView.UpdateParameters.Add("Company",e.Keys[0].ToString().Trim());
        SDS_GridView.UpdateParameters.Add("ReportType",e.Keys[1].ToString().Trim());
        SDS_GridView.UpdateParameters.Add("ReportID",e.Keys[2].ToString().Trim());
        SDS_GridView.UpdateParameters.Add("seqNo",e.Keys[3].ToString().Trim());
        SDS_GridView.UpdateParameters.Add("AcctNoStart",e.Keys[4].ToString().Trim());

        //string updatecmd = SDS_GridView.UpdateCommand;
        //string view = SDS_GridView.OldValuesParameterFormatString;

        for (int i = 0; i < e.NewValues.Count; i++)
        {
            try
            {
                tempOldValue = e.OldValues[i].ToString().Trim();
               
                if (string.IsNullOrEmpty(e.NewValues[i].ToString()))
                {//將空欄位放入半形空格
                    e.NewValues[i] = " ";
                }
               
                if (e.NewValues[i].ToString().Trim() != tempOldValue)
                {  
                    try
                    {
                        UpdateItem += GridView1.HeaderRow.Cells[i + 1].Text.Trim() + "|";
                        UpdateValue += e.OldValues[i].ToString().Trim() + "->" + e.NewValues[i].ToString().Trim() + "|";
                    }
                    catch
                    { }
                }
            }
            catch
            { }
        }

       

        #region 開始異動前,先寫入LOG
        //TableName	異動資料表	varchar	60
        //TrxType	異動類別(A/U/D)	char	1
        //ChangItem	異動項目(欄位)	varchar	255
        //SQLcommand	異動項目(異動值/指令)	varchar	2000
        //ChgStartDateTime	異動開始時間	smalldatetime	
        //ChgStopDateTime	異動結束時間	smalldatetime	
        //ChgUser	使用者代號	nvarchar	32
        DateTime StartDateTime = DateTime.Now;
        MyCmd.Parameters.Clear();
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "GLReportDefDetail";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "U";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = UpdateItem;
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = UpdateValue;
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion
        //try
        //{
           
        //    //SDS_GridView.Update();
        //}
        //catch (Exception ex)
        //{
        //    Err= ex.Message;
        //}

    }
    protected void GridView1_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        if (e.Exception == null)
        {
            GridView1.EditIndex = -1;
            lbl_Msg2.Text = e.AffectedRows.ToString() + " 個資料列 " + "更新成功!!";
           // BindDetailData();
        }
        else
        {
            lbl_Msg2.Text = "更新失敗!!  原因: " + e.Exception.Message;
            e.ExceptionHandled = true;
        }

        #region 完成異動後,更新LOG資訊
        MyCmd.Parameters["@SQLcommand"].Value = (e.Exception == null) ? "Success" : lbl_Msg2.Text;
        MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        if (e.Exception != null)
            return;

        //showPanel();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string strValue = "";

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
           
            if ((e.Row.RowState == System.Web.UI.WebControls.DataControlRowState.Edit)
                || (e.Row.RowState == (System.Web.UI.WebControls.DataControlRowState.Alternate | System.Web.UI.WebControls.DataControlRowState.Edit)))
            {
                #region 修改用
                //確認
                if (e.Row.Cells[1].Controls[0] != null)
                {
                    ImageButton IB = (ImageButton)e.Row.Cells[1].Controls[0];
                    IB.Attributes.Add("onclick", "return confirm('確定要修改資料嗎?');");
                    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                    IB.Style.Add("filter", "alpha(opacity=50)");
                }
                //取消
                if (e.Row.Cells[1].Controls[2] != null)
                {
                    ImageButton IB = ((ImageButton)e.Row.Cells[1].Controls[2]);
                    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                    IB.Style.Add("filter", "alpha(opacity=50)");
                }

                //加減項
                if (e.Row.FindControl("DrpeditPMcode")!=null)
                {
                    DropDownList drp = (DropDownList)e.Row.FindControl("DrpeditPMcode");
                    HiddenField hid = (HiddenField)e.Row.FindControl("Hideditpmcode");


                    if (hid.Value != "0" && hid.Value != "")
                    {
                        drp.SelectedValue = hid.Value;
                    
                    }               
                }

                //合計碼

                if (e.Row.FindControl("chkeditsuntot") != null)
                {
                    CheckBox chk = (CheckBox)e.Row.FindControl("chkeditsuntot");
                    HiddenField hid = (HiddenField)e.Row.FindControl("Hideditsuntot");


                    if (hid.Value=="V"  )
                    {
                        chk.Checked=true;
                    }
                }

                //列印明細

                if (e.Row.FindControl("chkeditPrint") != null)
                {
                    CheckBox chk2 = (CheckBox)e.Row.FindControl("chkeditPrint");
                    HiddenField hid2 = (HiddenField)e.Row.FindControl("HideditPrint");


                    if (hid2.Value == "V")
                    {
                        chk2.Checked = true;
                    }
                }
                              


                #endregion
              
            }
            else
            {
                #region 查詢用
                //e.Row.Cells[4].Text = "&nbsp;&nbsp;&nbsp;&nbsp;" + e.Row.Cells[4].Text;

                if (e.Row.Cells[1].Controls[0] != null)
                {
                    ImageButton IB = (ImageButton)e.Row.Cells[1].Controls[0];
                    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                    IB.Style.Add("filter", "alpha(opacity=50)");
                }
                #endregion
                
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            #region 新增用欄位
            strValue = "";

            for (int i = 2; i < e.Row.Cells.Count; i++)
            {

              
                //序號
                if (i == 2)
                {                     

                    TextBox tmpAddTxt = new TextBox();
                    tmpAddTxt.ID = "txtaddseq";
                    tmpAddTxt.Text = "";
                    e.Row.Cells[i].Controls.Add(tmpAddTxt);                                 
                }


                //加減項
                if (i == 3)
                {
                    DropDownList DrpPMcode = new DropDownList();
                    ListItem lsA = new ListItem("加","1");
                    ListItem lsD = new ListItem("減", "-1");
                    DrpPMcode.Items.Add(lsA);
                    DrpPMcode.Items.Add(lsD);
                    DrpPMcode.ID = "DrpPMcode";
                    e.Row.Cells[i].Controls.Add(DrpPMcode);     
                
                }
                //會計起始號碼
                 if (i == 4)
                {                     

                    TextBox tmpAddTxt = new TextBox();
                    tmpAddTxt.ID = "txtaddAcctNoFrom";
                    tmpAddTxt.Text = "";
                    e.Row.Cells[i].Controls.Add(tmpAddTxt);                                 
                }

                //會計結束號碼

                if (i == 5)
                {

                    TextBox tmpAddTxt = new TextBox();
                    tmpAddTxt.ID = "txtaddAcctNoEnd";
                    tmpAddTxt.Text = "";
                    e.Row.Cells[i].Controls.Add(tmpAddTxt);
                }

                



                //合計碼
                if (i == 6)
                {
                    CheckBox chkSumTotal = new CheckBox();
                    chkSumTotal.ID = "chksumcode";
                    chkSumTotal.Text = "";
                    e.Row.Cells[i].Controls.Add(chkSumTotal);
                   // strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
                }

                //列印明細
                if (i == 7)
                {
                    CheckBox chkPrint = new CheckBox();
                    chkPrint.ID = "chkprintcode";
                    chkPrint.Text = "";
                    e.Row.Cells[i].Controls.Add(chkPrint);
                    // strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
                }



                //項目
                if (i == 8)
                {

                    TextBox tmpAddTxt = new TextBox();
                    tmpAddTxt.ID = "txtadditem";
                    tmpAddTxt.Text = "";
                    e.Row.Cells[i].Controls.Add(tmpAddTxt);
                }

             
                //部門份用別

                if (i == 9)
                {

                    TextBox tmpAddTxt = new TextBox();
                    tmpAddTxt.ID = "txtexpensecode";
                    tmpAddTxt.Text = "";
                    e.Row.Cells[i].Controls.Add(tmpAddTxt);
                }



            }

            ImageButton btAddNew = new ImageButton();
            btAddNew.ID = "btAddNew";
            btAddNew.SkinID = "NewAdd";
            btAddNew.CommandName = "Insert";
            btAddNew.OnClientClick = "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));";
            e.Row.Cells[1].Controls.Add(btAddNew);
            #endregion
        }
        else if (e.Row.RowType == DataControlRowType.EmptyDataRow)
        {
            //權限
            e.Row.Visible = GridView1.ShowFooter;
            #region 新增用欄位

            strValue = "";

           

            ImageButton btnNew = (ImageButton)e.Row.FindControl("btnNew");
            if (btnNew != null)
                btnNew.Attributes.Add("onclick", "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));");
            #endregion
        }
    }

  
    protected void btnMedit_Click(object sender, ImageClickEventArgs e)
    {
        //編輯主檔
        Enableitem();
        btnSaveExit.Visible = true;
        btnMedit.Visible = false;
    }

   
}