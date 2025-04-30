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

public partial class GLA0380_M : System.Web.UI.Page
{
    string Ssql = "";   
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLA0380";
    DBManger _MyDBM;
    int saveon = 0;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    string sCompany="", sHDepartment="";
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

        CompanyList1.SelectedChanged += new UserControl_CompanyList.SelectedIndexChanged(CompanyList1_SelectedIndex);
       

        if (Request["Company"] != null && Request["HDepartment"] != null )
        {
            sCompany = Request["Company"].Trim();
            sHDepartment = Request["HDepartment"].Trim();
            
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
            if (sCompany != "" && sHDepartment!="")
            {
                CompanyList1.SelectValue = sCompany;
                bindDepdata(sCompany);
                DrpHDepartment.SelectedValue = sHDepartment;
            }
            
            BindMasterData( sCompany,sHDepartment);
            BindDetailData();

        }
        else
        {

            if (Request.Form["__EVENTARGUMENT"] != null)
            {

                if (Request.Form["__EVENTARGUMENT"].ToString().Contains("Edit"))
                {
                    BindMasterData(sCompany, sHDepartment);
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
                        if (sCompany == "" && sHDepartment == "")
                        {
                            sCompany = CompanyList1.SelectValue;
                            sHDepartment = DrpHDepartment.SelectedValue;
                        }
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

    void CompanyList1_SelectedIndex(object sender, UserControl_CompanyList.SelectEventArgs e)
    {
        bindDepdata(CompanyList1.SelectValue );
    }

    private void bindDepdata(string strCompany )
    {
        DrpHDepartment.Items.Clear();
        string strSQL = "SELECT Company,rtrim(DepCode) AS DepCode, rtrim(DepCode)+'-'+rtrim(DepName) AS DepName FROM Department Where Company='" + strCompany.Trim() + "' AND DepType='2'";
        DataTable DT = new DataTable();
        DT= _MyDBM.ExecuteDataTable(strSQL);
        if (DT.Rows.Count > 0)
        {
            DrpHDepartment.DataSource = DT;
            DrpHDepartment.DataTextField = "DepName";
            DrpHDepartment.DataValueField = "DepCode";
            DrpHDepartment.DataBind();       
        }       
    }

    private void bindDeatilDepdata(string strCompany ,DropDownList DDL)
    {
        DDL.Items.Clear();
        string strSQL = "SELECT Company,rtrim(DepCode) AS DepCode,rtrim(DepCode)+'-'+rtrim(DepName) AS DepName FROM Department Where Company='" + strCompany.Trim() + "' AND DepType='1'";
        DataTable DT = new DataTable();
        DT = _MyDBM.ExecuteDataTable(strSQL);
        if (DT.Rows.Count > 0)
        {
            DDL.DataSource = DT;
            DDL.DataTextField = "DepName";
            DDL.DataValueField = "DepCode";
            DDL.DataBind();
        }
    }

   

  
    private bool ValidateData2(string Company,string HDepartment ,string DDepartment)
    {
        SqlCommand sqlcmd = new SqlCommand();

        Ssql = @"Select * FROM GLDeptSturDetail
               WHERE Company=@company AND HDepartment=@HDepartment AND DDepartment=@DDepartment ";

        sqlcmd.Parameters.Add("@company", SqlDbType.Char).Value = Company;
        sqlcmd.Parameters.Add("@HDepartment", SqlDbType.Char).Value = HDepartment;
        sqlcmd.Parameters.Add("@DDepartment", SqlDbType.Char).Value = DDepartment;

        DataTable tb = new DataTable();

         tb = _MyDBM.ExecuteDataTable(Ssql,sqlcmd.Parameters,CommandType.Text);

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
        DrpHDepartment.Enabled = false;
        txtRemark.Enabled=false;
      
        
    }


    /// <summary>
    /// 開啟主檔寫入功能
    /// </summary>
    private void Enableitem()
    {
       
       txtRemark.Enabled=true;
       
    }




    protected void btnSave_Click(object sender, ImageClickEventArgs e)
    {
        SaveMasterdata(blInsertMod);
       

    }

    private void BindMasterData(string strcompany,string strHDepartment)
    {
        string strSQL = @"SELECT * FROM GLDeptSturHead WHERE company= @company
         AND HDepartment=@HDepartment ";

        SqlCommand sqlcmd = new SqlCommand();

        sqlcmd.Parameters.Add("@company",SqlDbType.Char).Value=strcompany;
        sqlcmd.Parameters.Add("@HDepartment", SqlDbType.Char).Value = strHDepartment;      

        DataTable Dt = _MyDBM.ExecuteDataTable(strSQL,sqlcmd.Parameters,CommandType.Text);

        //固定資料
        if (Dt.Rows.Count > 0)
        {
            CompanyList1.SelectValue = Dt.Rows[0]["company"].ToString();
            DrpHDepartment.SelectedValue = Dt.Rows[0]["HDepartment"].ToString();
            txtRemark.Text = Dt.Rows[0]["Remark"].ToString();
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

        //檢查備註

        //新增
        if (savemode)
        {
            strSQL = @"INSERT INTO GLDeptSturHead (Company,HDepartment,Remark,LstChgUser,LstChgDateTime)
                     VALUES (@Company,@HDepartment,@Remark,@LstChgUser,@LstChgDateTime)";


            if (ValidateData(CompanyList1.SelectValue,DrpHDepartment.SelectedValue )==true)
            {
                //秀出已有相關資料訊息
                JsUtility.ClientMsgBox("資料已存在！", this.Page, "");

                return ;
            }

           
        }
        else
        {
            strSQL = @"UPDATE GLDeptSturHead SET Remark=@Remark,LstChgUser=@LstChgUser,LstChgDateTime=@LstChgDateTime
                       WHERE Company=@Company AND  HDepartment=@HDepartment";        
        }
        

        sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = CompanyList1.SelectValue;
        sqlcmd.Parameters.Add("@HDepartment", SqlDbType.Char).Value = DrpHDepartment.SelectedValue;
        sqlcmd.Parameters.Add("@Remark", SqlDbType.Char).Value = txtRemark.Text.Trim();
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
        SDS_GridView.SelectParameters.Add("HDepartment", DbType.StringFixedLength,DrpHDepartment.SelectedValue);
      
       
        GridView1.DataBind();
        //Navigator1.BindGridView = GridView1;
        //Navigator1.DataBind();
    }

    //新增

    protected void btnEmptyNew_Click(object sender, EventArgs e)
    {
        //GridView1$ctl05$DrpDepAdd  

        string HeadID = hid_IsInsertExit.Value.Replace("_", "$");       
        string strDDepartment = Request.Form[HeadID + "$DrpDepAdd"] != null ? Request.Form[HeadID + "$DrpDepAdd"].ToString().Trim() : "";
        
      
         


        //新增資料

        //檢查欄位是否正確

        //明細成本中心
        if (strDDepartment == "")
        {
            JsUtility.ClientMsgBox("必須選擇成本中心！", this.Page, "");
            return;

        }
      
        
       
   
    
     







        //檢查細項資料是否存在
        //false代表有資料
        if (ValidateData2(sCompany, sHDepartment, strDDepartment) == false)
        {
            JsUtility.ClientMsgBox("資料已存在！", this.Page, "");
            return;
        }

       




        if (hid_IsInsertExit.Value != "")
        {                 




            //新增
            if (lbl_Msg2.Text == "")
            {
                SDS_GridView.InsertParameters.Clear();
                SDS_GridView.InsertParameters.Add("Company", sCompany);
                SDS_GridView.InsertParameters.Add("HDepartment", sHDepartment);
                SDS_GridView.InsertParameters.Add("DDepartment", strDDepartment);               
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
                MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "GLDeptSturDetail";
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
   
    private bool ValidateData(string strCompany, string strHDepartment)
    {
        //判斷資料是否重覆
        Ssql = @"SELECT * FROM GLDeptSturHead WHERE Company=@Company AND HDepartment=@HDepartment";
            

        SqlCommand sqlcmd = new SqlCommand();

        sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = strCompany;
        sqlcmd.Parameters.Add("@HDepartment", SqlDbType.Char).Value = strHDepartment;
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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "GLDeptSturDetail";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "D";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion       

        string sql = @" DELETE  FROM GLDeptSturDetail 
                     WHERE Company=@company AND HDepartment=@HDepartment AND DDepartment=@DDepartment";

        sqlcmd.Parameters.Add("@company", SqlDbType.Char).Value = sCompany;
        sqlcmd.Parameters.Add("@HDepartment", SqlDbType.Char).Value = sHDepartment;
        sqlcmd.Parameters.Add("@DDepartment", SqlDbType.Char).Value = L2PK;

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
        //加入下拉選單
        if (e.Row.RowType == DataControlRowType.EmptyDataRow)
        {
            if(e.Row.FindControl("DrpDepAdd")!=null)
            {
                DropDownList drpddep = (DropDownList)e.Row.FindControl("DrpDepAdd");
                bindDeatilDepdata(sCompany, drpddep);           
            }        
        }




    }
  

 
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string strValue = "";

        if (e.Row.RowType == DataControlRowType.DataRow)
        {           
                #region 查詢用
                //e.Row.Cells[4].Text = "&nbsp;&nbsp;&nbsp;&nbsp;" + e.Row.Cells[4].Text;

            if (e.Row.FindControl("btnDelete")!=null)
                {
                    LinkButton LB = (LinkButton)e.Row.FindControl("btnDelete");
                    LB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                    LB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                    LB.Style.Add("filter", "alpha(opacity=50)");
                }
                #endregion

            if (DBSetting.DepartmentName(e.Row.Cells[1].Text, e.Row.Cells[2].Text.Trim()) != null)
            {
                e.Row.Cells[2].Text = e.Row.Cells[2].Text.Trim() + " - " + DBSetting.DepartmentName(e.Row.Cells[1].Text, e.Row.Cells[2].Text.Trim()).ToString();
            }

            if (DBSetting.DepartmentName(e.Row.Cells[1].Text, e.Row.Cells[3].Text.Trim()) != null)
            {
                e.Row.Cells[3].Text = e.Row.Cells[3].Text.Trim() + " - " + DBSetting.DepartmentName(e.Row.Cells[1].Text, e.Row.Cells[3].Text.Trim()).ToString();
            }            
            e.Row.Cells[1].Text = e.Row.Cells[1].Text + " - " + DBSetting.CompanyName(e.Row.Cells[1].Text).ToString();
                
            
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            #region 新增用欄位
            strValue = "";                              
                                          
                //部門選單
                DropDownList DrpDepartAdd = new DropDownList();
                DrpDepartAdd.ID = "DrpDepAdd";
                bindDeatilDepdata(sCompany, DrpDepartAdd);              
                e.Row.Cells[3].Controls.Add(DrpDepartAdd);                     

            ImageButton btAddNew = new ImageButton();
            btAddNew.ID = "btAddNew";
            btAddNew.SkinID = "NewAdd";
            btAddNew.CommandName = "Insert";
            btAddNew.OnClientClick = "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));";
            e.Row.Cells[0].Controls.Add(btAddNew);
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