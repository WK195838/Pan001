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
using FormsAuth;


public partial class PersonnelMaster : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM001";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();

    protected void Page_PreInit(object sender, EventArgs e)
    {//頁面初始化前,先設定好系統使用的佈景主題(主要是決定LOGO)
        if (Session["Theme"] != null)
            Page.Theme = Session["Theme"].ToString();

        if (Session["MasterPage"] != null)
            Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _MyDBM = new DBManger();
        _MyDBM.New();
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = 'Payroll' And RTrim(ProgramPath)='Main/PersonnelMaster.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
    }

    private void AuthRight()
    {
        //驗證權限
        bool Find = false;
        bool SetCss = false;
        int i = 0;

        string[] Auth = { "Delete", "Modify", "Detail", "Add" };

        if (ConfigurationManager.AppSettings["EnableProgramAuth"] != null && ConfigurationManager.AppSettings["EnableProgramAuth"].ToString() == "true")
        {            
            for (i = 0; i < Auth.Length; i++)
            {
                Find = _UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, Auth[i]);
                if (i < (Auth.Length - 1))
                {//刪/修/詳
                    GridView1.Columns[i].Visible = Find;
                    //設定標題樣式
                    if (Find && (SetCss == false))
                    {
                        SetCss = true;
                        GridView1.Columns[i].HeaderStyle.CssClass = "paginationRowEdgeLl";
                    }
                }
                else
                {//新增
                    btnNew.Visible = Find;
                    btnEmptyNew.Visible = Find;
                }
            }

            //查詢(執行)
            if ((_UserInfo.CheckPermission(_ProgramId)) || Find)
            {
                Find = true;
            }
            else
            {
                Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
            }

            //版面樣式調整
            if (SetCss == false)
            {
                GridView1.Columns[(Auth.Length - 1)].HeaderStyle.CssClass = "paginationRowEdgeLl";
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";//清空訊息

        Navigator1.BindGridView = GridView1;

        CompanyList1.SelectedIndex += new UserControl_CompanyList.SelectedIndexChanged ( CompanyList1_SelectedIndex );

        if (!Page.IsPostBack)
        {
            if (_UserInfo.AuthLogin == false)
            {
                Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=UnLogin");
            }
            else
            {
                if (_UserInfo.CheckPermission(_ProgramId) == false)
                    Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
            }
            //預設公司
            //CompanyList1.SelectValue = "";
            CL_ResignC.SetCodeList("PY#ResignC", 4, "全部");
            BindData();
            showPanel();
            AuthRight();
        }
        else
        {
            BindData();
        }

        btnNew.Attributes.Add("onclick", "javascript:var win =window.open('PersonnelMaster_M.aspx?addCompany='+" + this.CompanyList1.CompanyListClientID() + ".value" +
                                                                                                "+'&addDep='+" + this.DepList.ClientID + ".value" +
                                                                                                "+'&addEmployeeID='+" + this.tbEmployeeId.ClientID + ".value" +
                                                                                                "+'&addEmployeeName='+" + this.tbEmployeeName.ClientID + ".value" +
                                                                                                 ",'','width=1010px,height=730px,scrollbars=yes,resizable=yes');");
        btnEmptyNew.Attributes.Add("onclick", "javascript:var win =window.open('PersonnelMaster_M.aspx','','width=1010px,height=730px,scrollbars=yes,resizable=yes');");
    }

    void CompanyList1_SelectedIndex ( object sender , UserControl_CompanyList.SelectEventArgs e )
    {
        this.DepList.Items.Clear ( );

        DepList.Items.Add ( Li("全部","%%") );
        
        for ( int i = 0 ; i < CompanyList1.Department_Basic.Count ; i++ )
        {
            DepList.Items.Add ( CompanyList1.Department_Basic[i] );
        }

        BindData ( );
    }

    private ListItem Li ( string Text , string Value )
    {
        ListItem li = new ListItem ( );
        li.Text = Text;
        li.Value = Value;
        return li;
    }

    private void showPanel ( )
    {
        Panel_Empty.Visible = hasData ( ) ? false : true;
    }

    private bool hasData()
    {
        Ssql = "SELECT * FROM Personnel_Master Where 0=0";
        if (tbEmployeeId.Text.Length > 0)
        {
            Ssql += string.Format(" And EmployeeId like '%{0}%'", _UserInfo.SysSet.CleanSQL(tbEmployeeId.Text).ToString());
        }

        if (tbEmployeeName.Text.Length > 0)
        {
            Ssql += string.Format(" And EmployeeName like '%{0}%'", _UserInfo.SysSet.CleanSQL(tbEmployeeName.Text).ToString());
        }

        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);

        return tb.Rows.Count > 0 ? true : false;
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        LinkButton link;
        Label lbl;

        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                //lbl = (Label)e.Row.FindControl("lbl_ProgramType");

                //switch (lbl.Text)
                //{
                //    case "P":
                //        lbl.Text = "程式";
                //        break;

                //    case "M":
                //        lbl.Text = "選單";
                //        break;
                //}

                if ((e.Row.RowState == System.Web.UI.WebControls.DataControlRowState.Edit)
                || (e.Row.RowState == (System.Web.UI.WebControls.DataControlRowState.Alternate | System.Web.UI.WebControls.DataControlRowState.Edit)))
                {
                    //修改時                    
                }
                else
                {
                    //查詢時                   
                    //DataRowView tehDRV = (DataRowView)DataBinder.GetDataItem(e.Row);

                    //for (int i = 3; i < e.Row.Cells.Count - 1; i++)
                    //    e.Row.Cells[i].Text = "&nbsp;&nbsp;&nbsp;&nbsp;" + e.Row.Cells[i].Text;
                    //((LiteralControl)e.Row.Cells[6].Controls[0]).Text += "&nbsp;&nbsp;&nbsp;&nbsp;";
                    try
                    {
                        //e.Row.Cells[ 3 ].Text = ( ( DropDownList ) CompanyList1.FindControl ( "companyList" ) ).Items.FindByValue ( e.Row.Cells[ 3 ].Text.Trim ( ) ).Text;
                        e.Row.Cells[ 3 ].Text = DepList.Items.FindByValue ( e.Row.Cells[ 3 ].Text.Trim ( ) ).Text;
                    }
                    catch{}

                }

                try
                {
                    //已有計薪確認資料者,不可刪
                    int iHasPayCount = int.Parse(DataBinder.Eval(e.Row.DataItem, "HasPayCount").ToString());
                    if (iHasPayCount != 0)
                    {
                        e.Row.Cells[0].Text = "";
                    }
                }
                catch { }

                link = (LinkButton)e.Row.FindControl("btnSelect");
                link.Attributes.Add("onclick", "javascript:var win =window.open('PersonnelMaster_M.aspx?Kind=Query&Company=" + GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim() + "&EmployeeId=" + GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().Trim() + "','','width=1010px,height=730px,scrollbars=yes,resizable=yes');");

                link = (LinkButton)e.Row.FindControl("btnEdit");
                link.Attributes.Add("onclick", "javascript:var win =window.open('PersonnelMaster_M.aspx?Company=" + GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim() + "&EmployeeId=" + GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().Trim() + "','','width=1010px,height=730px,scrollbars=yes,resizable=yes');");
                break;

            case DataControlRowType.Header:

                link = (LinkButton)e.Row.FindControl("btnNew");
                if (link != null)
                {
                    //指定位置用top=100px,left=100px,
                    link.Attributes.Add("onclick", "javascript:var win =window.open('PersonnelMaster_M.aspx','','width=1010px,height=730px,scrollbars=yes,resizable=yes');");
                }
                    break;
        }
    }

    private void BindData()
    {
        Ssql = "SELECT * " +
            " ,(SELECT count(*) FROM [Payroll_History_Heading] where [Company] = Personnel_Master.Company and [EmployeeId] = Personnel_Master.EmployeeId) HasPayCount " +
            " FROM Personnel_Master Where Company='" + CompanyList1.SelectValue.Trim() + "' And DeptId like '" + DepList.SelectedValue.Trim() + "'";
        

        
        if (tbEmployeeId.Text.Length > 0)
        {
            Ssql += string.Format(" And EmployeeId like '%{0}%'", _UserInfo.SysSet.CleanSQL(tbEmployeeId.Text).ToString());
        }

        if (tbEmployeeName.Text.Length > 0)
        {
            Ssql += string.Format(" And (EmployeeName like '%{0}%' Or EnglishName like '%{0}%' Or TitleCode like '%{0}%')", _UserInfo.SysSet.CleanSQL(tbEmployeeName.Text).ToString());
        }

        if (Request["DefRC"] != null)
        {
            CL_ResignC.SelectedCode = Request["DefRC"].ToString().ToUpper();
            switch (Request["DefRC"].ToString().ToUpper())
            {
                case "Y"://已離職
                    Ssql += " And ResignCode in ('Y')";
                    CL_ResignC.Enabled = false;
                    break;
                case "N"://在職中,含離職待生效
                    Ssql += " And IsNull(ResignCode,'N') in ('N','W')";
                    CL_ResignC.Enabled = false;
                    break;
                default:
                    break;
            }
        }

        if (CL_ResignC.SelectedCode.Trim().Length != 0 && CL_ResignC.Enabled != false)
            Ssql += " And IsNull(ResignCode,'N') = '" + CL_ResignC.SelectedCode + "'";

        SDS_GridView.SelectCommand = Ssql+ " Order By Company,EmployeeId" ;
        GridView1.DataBind();

        Navigator1.BindGridView = GridView1;
        //Navigator1.SQL = SDS_GridView.SelectCommand;
        Navigator1.DataBind();
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";

        if (string.IsNullOrEmpty(CompanyList1.SelectValue))
        {
            lbl_Msg.Text = "請選擇先公司!";
        }
        else
        {
            BindData();
            showPanel();
        }
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //#region 開始異動前,先寫入LOG
        ////TableName	異動資料表	varchar	60
        ////TrxType	異動類別(A/U/D)	char	1
        ////ChangItem	異動項目(欄位)	varchar	255
        ////SQLcommand	異動項目(異動值/指令)	varchar	2000
        ////ChgStartDateTime	異動開始時間	smalldatetime	
        ////ChgStopDateTime	異動結束時間	smalldatetime	
        ////ChgUser	使用者代號	nvarchar	32
        //DateTime StartDateTime = DateTime.Now;
        //MyCmd.Parameters.Clear();
        //MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "Personnel_Master";
        //MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "D";
        //MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        //MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
        //MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        ////此時不設定異動結束時間
        ////MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        //MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        //_MyDBM.DataChgLog(MyCmd.Parameters);
        //#endregion
    }

    protected void GridView1_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        //if (e.Exception == null)
        //{
        //    lbl_Msg.Text = e.AffectedRows.ToString() + " 個資料列 " + "刪除成功!!";

        //    #region
        //    string theFileName = "Pic_" + Request["Company"].ToString().Trim() + "_" + Request["EmployeeId"].ToString().Trim();
        //    string savePath = Request.ServerVariables["APPL_PHYSICAL_PATH"].ToString() + "Main\\" + _UserInfo.SysSet.GetConfigString("picture") + "\\";
        //    string theTempPicFileStyle, theTempPic;
        //    theTempPicFileStyle = _UserInfo.SysSet.chekPic(savePath + theFileName);
        //    theTempPic = savePath + theFileName + theTempPicFileStyle;
        //    if (System.IO.File.Exists(theTempPic))
        //    {//刪除照片
        //        System.IO.File.Delete(theTempPic);
        //    }
        //    theTempPicFileStyle = _UserInfo.SysSet.chekPic(savePath + theFileName + "_new");
        //    theTempPic = savePath + theFileName + "_new" + theTempPicFileStyle;            
        //    if (System.IO.File.Exists(theTempPic))
        //    {//刪除照片暫存檔      
        //        System.IO.File.Delete(theTempPic);
        //    }
        //    #endregion

        //    BindData();
        //}
        //else
        //{
        //    lbl_Msg.Text = "刪除失敗!!  原因: " + e.Exception.Message;
        //    e.ExceptionHandled = true;
        //}

        //#region 完成異動後,更新LOG資訊
        //MyCmd.Parameters["@SQLcommand"].Value = (e.Exception == null) ? "Success" : lbl_Msg.Text;
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        //_MyDBM.DataChgLog(MyCmd.Parameters);
        //#endregion

        //if (e.Exception != null)
        //    return;

        //showPanel();
    }
   

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        LinkButton btnDelete = (LinkButton)sender;
        string L1PK = btnDelete.Attributes["L1PK"].ToString();
        string L2PK = btnDelete.Attributes["L2PK"].ToString();
        string sql = "Delete From Personnel_Master Where Company='" + L1PK + "' And EmployeeId='" + L2PK + "'";

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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "Personnel_Master";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "D";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        int result = 0;

        try
        {
            result = _MyDBM.ExecuteCommand(sql.ToString());
        }
        catch (Exception ex)
        {
            lbl_Msg.Text = ex.Message;
        }

        if (result > 0)
        {
            lbl_Msg.Text = "資料刪除成功 !!";

            #region
            string theFileName = "Pic_" + L1PK.Trim() + "_" + L2PK.Trim();
            string savePath = Request.ServerVariables["APPL_PHYSICAL_PATH"].ToString() + "Main\\" + _UserInfo.SysSet.GetConfigString("picture") + "\\";
            string theTempPicFileStyle, theTempPic;
            theTempPicFileStyle = _UserInfo.SysSet.chekPic(savePath + theFileName);
            theTempPic = savePath + theFileName + theTempPicFileStyle;
            if (System.IO.File.Exists(theTempPic))
            {//刪除照片
                System.IO.File.Delete(theTempPic);
            }
            theTempPicFileStyle = _UserInfo.SysSet.chekPic(savePath + theFileName + "_new");
            theTempPic = savePath + theFileName + "_new" + theTempPicFileStyle;
            if (System.IO.File.Exists(theTempPic))
            {//刪除照片暫存檔      
                System.IO.File.Delete(theTempPic);
            }
            #endregion

            BindData();
            Navigator1.DataBind();
        }
        else
        {
            if (lbl_Msg.Text.Length > 0)
                lbl_Msg.Text = "資料刪除失敗 !!" + lbl_Msg.Text;
            else
                lbl_Msg.Text = "無資料可刪除!";
        }

        #region 完成異動後,更新LOG資訊
        MyCmd.Parameters["@SQLcommand"].Value = (result > 0) ? "Success" : lbl_Msg.Text;
        MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        showPanel();
    }

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "setnewcolor(this);");
            e.Row.Attributes.Add("onmouseout", "setoldcolor(this);");
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].CssClass = "Grid_GridLine";
                e.Row.Cells[i].Style.Add("text-align", "left");
            }

           

        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {
            //e.Row.Style.Add("display", "none");
            e.Row.Visible = false;
        }
    }
}
