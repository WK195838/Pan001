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

public partial class UserControl_FindException : System.Web.UI.UserControl
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    //string _ProgramId = "PYB005";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    string tablesc = "";
    string companyvalue = "";
    string EmployeeIdvalue = "";
    string AttendanceDatevalue = "";
    string Ssql1 = "";


    public string Username
    {
        set
        {
            if (value=="adm01")
            {
                BindData(null);
            }
            else
            {
                BindData(value);
            }
        }
    }

    public string DataSource
    {
        get
        {
            return tablesc;
        }
        set
        {
            tablesc = value;
        }
    }
    //public string Company
    //{
    //    set
    //    {
    //        companyvalue = value;
    //    }
    //}
    //public string EmployeeId
    //{
    //    set
    //    {
    //        EmployeeIdvalue = value;
    //    }
    //}
    //public string AttendanceDate
    //{
    //    set
    //    {
    //        AttendanceDatevalue = value;
    //    }
    //}
    public bool finddata
    {
        get 
        {
            return hasData();
        }
        set
        {
            hasData();
        }
    }
    public string selectcommand
    {
        set 
        {
            Ssql1 = value;
            BindData(null);
        }
    }


    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _MyDBM = new DBManger();
        _MyDBM.New();
        if (_UserInfo.SysSet.isTWCalendar)
        {
            System.Globalization.CultureInfo cag = new System.Globalization.CultureInfo("zh-TW");
            cag.DateTimeFormat.Calendar = new System.Globalization.TaiwanCalendar();
            System.Threading.Thread.CurrentThread.CurrentCulture = cag;
        }
        //DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/PayrollControl.aspx'");
        //if (DT.Rows.Count > 0)
        //    _ProgramId = DT.Rows[0][0].ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Navigator1.BindGridView = GridView1;
        Navigator1.DataBind();
        //BindData(null);
    }

    private bool hasData()
    {
        Ssql = "SELECT * FROM " + tablesc + " Where 0=0";

        Ssql += Ssql1;
        //if (txtDate.Text.Length > 0)
        //{
        //    Ssql += string.Format(" And Convert(varchar,HolidayDate,111) like '%{0}%'", _UserInfo.SysSet.GetADDate(txtDate.Text));
        //}
        //Ssql += string.Format(" And Convert(varchar,HolidayDate,111) like '%{0}%'", YearList1.SelectADYear);

        //if (txtCompany.Text.Length > 0)
        //{
        //    Ssql += string.Format(" And Company like '%{0}%'", txtCompany.Text);
        //}

        MyCmd.Parameters.Clear();
        //MyCmd.Parameters.Add("@ls_level", SqlDbType.Int).Value = 0;
        DataTable tb = _MyDBM.ExecuteDataTable(Ssql, MyCmd.Parameters, CommandType.Text);

        if (tb.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
 
    private void BindData(string username)
    {
        Ssql = "SELECT * FROM " + tablesc + " Where 0=0 ";//"+  +"

        if (string.IsNullOrEmpty(username))
        {
            Ssql += Ssql1;
        }
        else
        {
            if (username.Length > 0)
            {
                Ssql += string.Format(" And EmployeeId like '%{0}%'", username.Trim());
            }
        }

        SDS_GridView.SelectCommand = Ssql;

        GridView1.DataBind();
        Navigator1.BindGridView = GridView1;
        Navigator1.DataBind();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string strValue = "";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Cells[3].Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

            if ((e.Row.RowState == System.Web.UI.WebControls.DataControlRowState.Edit)
                || (e.Row.RowState == (System.Web.UI.WebControls.DataControlRowState.Alternate | System.Web.UI.WebControls.DataControlRowState.Edit)))
            {
                #region 修改用
                ////確認
                //if (e.Row.Cells[1].Controls[0] != null)
                //{
                //    ImageButton IB = (ImageButton)e.Row.Cells[1].Controls[0];
                //    IB.Attributes.Add("onclick", "return confirm('確定要修改資料嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "');");

                //    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                //    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                //    IB.Style.Add("filter", "alpha(opacity=50)");
                //}
                ////取消
                //if (e.Row.Cells[1].Controls[2] != null)
                //{
                //    ImageButton IB = ((ImageButton)e.Row.Cells[1].Controls[2]);
                //    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                //    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                //    IB.Style.Add("filter", "alpha(opacity=50)");
                //}
                #endregion
                //string c = ;
            }
            else
            {
                #region 查詢用
                ////e.Row.Cells[4].Text = "&nbsp;&nbsp;&nbsp;&nbsp;" + e.Row.Cells[4].Text;

                //if (e.Row.Cells[1].Controls[0] != null)
                //{
                //    ImageButton IB = (ImageButton)e.Row.Cells[1].Controls[0];
                //    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                //    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                //    IB.Style.Add("filter", "alpha(opacity=50)");
                //}

                #endregion
                string sCom = e.Row.Cells[0].Text;
                if (DBSetting.CompanyName(e.Row.Cells[0].Text).ToString() != "")
                {
                    e.Row.Cells[0].Text = e.Row.Cells[0].Text + " - " + DBSetting.CompanyName(e.Row.Cells[0].Text).ToString();
                }
                if (DBSetting.PersonalName(sCom, e.Row.Cells[1].Text) != "")
                {
                    e.Row.Cells[1].Text = e.Row.Cells[1].Text + " - " + DBSetting.PersonalName(sCom, e.Row.Cells[1].Text).ToString();
                }
                //e.Row.Cells[1].Text = (int.Parse(e.Row.Cells[1].Text.Substring(0, 4).ToString()) - 1911).ToString() + e.Row.Cells[1].Text.Substring(4, 2);
                if (e.Row.Cells[2].Text.Contains("/"))
                {
                    e.Row.Cells[2].Text = Convert.ToDateTime(e.Row.Cells[2].Text).ToString("yy/MM/dd");
                }
                //if (e.Row.Cells[3].Text.Contains("/"))
                //{
                //    e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text).ToString("yy/MM/dd");
                //}
                if (!(string.IsNullOrEmpty(e.Row.Cells[5].Text.Trim())))
                {
                    if (e.Row.Cells[5].Text != "&nbsp;")//
                    {
                        if (e.Row.Cells[5].Text.Length > 0)
                        {
                            e.Row.Cells[5].Text = e.Row.Cells[5].Text.Trim().PadLeft(4, '0').Insert(2, " : ");

                        }
                    }
                }
                if (!(string.IsNullOrEmpty(e.Row.Cells[6].Text.Trim())))
                {
                    if (e.Row.Cells[6].Text != "&nbsp;")//!(string.IsNullOrEmpty(e.Row.Cells[5].Text.Trim()))
                    {
                        if (e.Row.Cells[6].Text.Length > 0)
                        {
                            e.Row.Cells[6].Text = e.Row.Cells[6].Text.Trim().PadLeft(4, '0').Insert(2, " : ");

                        }
                    }
                }
                
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {

            strValue = "";
            e.Row.Visible = false;
            #region 新增用欄位
            //for (int i = 2; i < e.Row.Cells.Count; i++)
            //{
            //    if (i == 2 || i == 4 || i == 5)
            //    {
            //        TextBox tbAddNew = new TextBox();
            //        tbAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
            //        if (i == 4 || i == 5)
            //        {
            //            tbAddNew.Style.Add("text-align", "right");
            //            tbAddNew.Style.Add("width", "70px");
            //        }
            //        e.Row.Cells[i].Controls.Add(tbAddNew);
            //        strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
            //        if (i == 4 || i == 5)
            //        {//為日期欄位增加小日曆元件
            //            ImageButton btOpenCal = new ImageButton();
            //            btOpenCal.ID = "btOpenCal" + i.ToString();
            //            btOpenCal.SkinID = "Calendar1";
            //            btOpenCal.OnClientClick = "return GetPromptDate(" + tbAddNew.ClientID + ");";
            //            e.Row.Cells[i].Controls.Add(btOpenCal);
            //        }
            //        if (i == 2)
            //        {
            //            ImageButton btOpenList = new ImageButton();
            //            btOpenList.ID = "btOpen" + i.ToString();
            //            btOpenList.SkinID = "OpenWin1";
            //            //Company,CompanyShortName,CompanyName,ChopNo
            //            btOpenList.OnClientClick = "return GetPromptWin1(" + tbAddNew.ClientID + ",'400','450','Company_Master','Company,CompanyShortName','CompanyShortName As 公司簡稱,CompanyName,ChopNo','Company');";
            //            e.Row.Cells[i].Controls.Add(btOpenList);
            //        }
            //    }


            //    if (i == 3)
            //    {
            //        DropDownList ddlAddNew = new DropDownList();
            //        ddlAddNew.ID = "YearMonth" + (i - 1).ToString().PadLeft(2, '0');
            //        int nowyear = int.Parse(DateTime.Now.Year.ToString()) - 1911;
            //        for (int n = (nowyear - 1); n < (nowyear + 11); n++)
            //        {
            //            string tmp = n.ToString("D2");
            //            ddlAddNew.Items.Add(tmp);
            //        }
            //        ddlAddNew.SelectedValue = nowyear.ToString();
            //        e.Row.Cells[i].Controls.Add(ddlAddNew);
            //        strValue += "checkColumns(" + ddlAddNew.ClientID + ") && ";
            //        DropDownList ddlAddNew2 = new DropDownList();
            //        ddlAddNew2.ID = "YearMonth" + i.ToString().PadLeft(2, '0');
            //        for (int n = 1; n < 13; n++)
            //        {
            //            string tmp = n.ToString("D2");
            //            ddlAddNew2.Items.Add(tmp);
            //        }
            //        ddlAddNew2.SelectedValue = (DateTime.Now.Month.ToString("D2"));
            //        e.Row.Cells[i].Controls.Add(ddlAddNew2);
            //        strValue += "checkColumns(" + ddlAddNew2.ClientID + ") && ";
            //    }

            //}

            //ImageButton btAddNew = new ImageButton();
            //btAddNew.ID = "btAddNew";
            //btAddNew.SkinID = "NewAdd";
            //btAddNew.CommandName = "Insert";
            //btAddNew.OnClientClick = "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));";
            //e.Row.Cells[1].Controls.Add(btAddNew);
            #endregion
        }
        else if (e.Row.RowType == DataControlRowType.EmptyDataRow)
        {
            //權限
            //e.Row.Visible = GridView1.ShowFooter;
            e.Row.Visible = false;
            #region 新增用欄位

            //strValue = "";

            //for (int i = 1; i < 5; i++)
            //{
            //    if (i == 1 || i == 3 || i == 4)
            //    {
            //        TextBox tbAddNew = (TextBox)e.Row.FindControl("tbAddNew0" + i.ToString());
            //        if (i == 3 || i == 4)
            //        {
            //            tbAddNew.Style.Add("text-align", "right");
            //            tbAddNew.Style.Add("width", "70px");
            //        }
            //        strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";

            //        if (i == 3 || i == 4)
            //        {//為日期欄位增加小日曆元件
            //            ImageButton btnCalendar = (ImageButton)e.Row.FindControl("btnCalendar" + (i - 2).ToString());
            //            if (btnCalendar != null)
            //                btnCalendar.Attributes.Add("onclick", "return GetPromptDate(" + tbAddNew.ClientID + ");");
            //        }
            //        if (i == 1)
            //        {
            //            ImageButton btnOpen = (ImageButton)e.Row.FindControl("btnOpen" + i.ToString());
            //            //btOpenList.SkinID = "OpenWin1";
            //            //Company,CompanyShortName,CompanyName,ChopNo
            //            if (btnOpen != null)
            //            {
            //                //btnOpen.SkinID = "OpenWin1";
            //                btnOpen.Attributes.Add("onclick", "return GetPromptWin1(" + tbAddNew.ClientID + ",'400','450','Company_Master','Company,CompanyShortName','CompanyShortName As 公司簡稱,CompanyName,ChopNo','Company');");
            //            }
            //            //e.Row.Cells[i].Controls.Add(btOpenList);
            //        }
            //    }
            //    if (i == 2)
            //    {
            //        DropDownList ddlAddNew = new DropDownList();
            //        ddlAddNew = (DropDownList)e.Row.FindControl("YearMonth" + i.ToString().PadLeft(2, '0'));
            //        int nowyear = int.Parse(DateTime.Now.Year.ToString()) - 1911;
            //        for (int n = (nowyear - 1); n < (nowyear + 11); n++)
            //        {
            //            string tmp = n.ToString("D2");
            //            ddlAddNew.Items.Add(tmp);
            //        }
            //        ddlAddNew.SelectedValue = nowyear.ToString();
            //        //e.Row.Cells[i].Controls.Add(ddlAddNew);
            //        strValue += "checkColumns(" + ddlAddNew.ClientID + ") && ";
            //        DropDownList ddlAddNew2 = new DropDownList();
            //        ddlAddNew2 = (DropDownList)e.Row.FindControl("YearMonth" + (i + 1).ToString().PadLeft(2, '0'));
            //        for (int n = 1; n < 13; n++)
            //        {
            //            string tmp = n.ToString("D2");
            //            ddlAddNew2.Items.Add(tmp);
            //        }
            //        ddlAddNew2.SelectedValue = (DateTime.Now.Month.ToString("D2"));

            //        //e.Row.Cells[i].Controls.Add(ddlAddNew2);
            //        strValue += "checkColumns(" + ddlAddNew2.ClientID + ") && ";
            //    }
            //}

            //ImageButton btnNew = (ImageButton)e.Row.FindControl("btnNew");
            //if (btnNew != null)
            //    btnNew.Attributes.Add("onclick", "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));");
            #endregion
        }
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
            //e.Row.Cells[i].Style.Add("text-align", "right");
            //e.Row.Cells[i].Style.Add("width", "100px");

        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {
            //e.Row.Style.Add("display", "none");
            e.Row.Visible = false;
        }
    }
}
