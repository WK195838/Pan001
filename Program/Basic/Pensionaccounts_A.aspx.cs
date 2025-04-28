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

public partial class Pensionaccounts_A : System.Web.UI.Page
{
    //#####################################//
    #region 變數宣告
    string Ssql = "";
    SysSetting SysSet = new SysSetting();
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM009";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    #endregion
    //#####################################//
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
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/Pensionaccounts.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript ( UP1 , this.GetType ( ) , "" , @"JQ();" , true );
        lbl_Msg.Text = "";
        

        bool blCheckLogin = _UserInfo.AuthLogin;
        if ((blCheckLogin == false) || (ConfigurationManager.AppSettings["EnableProgramAuth"] != null && ConfigurationManager.AppSettings["EnableProgramAuth"].ToString() == "true"))
        {
            bool blCheckProgramAuth = false;
            if (blCheckLogin == false)
                ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg("UnLogin");
            else
            {
                blCheckProgramAuth = _UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, "Add");
                if (blCheckProgramAuth == false)
                    ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg("NoRight");
            }
        }

        if ( !Page.IsPostBack )
        {
            try
            {
                ( ( TextBox ) DetailsView1.Rows[ 5 ].Cells[ 1 ].Controls[ 0 ] ).Text = SysSet.isTWCalendar ? SysSet.TWToday : DateTime.Today.ToShortDateString ( );
            }
            catch
            {
            }
        }

        btnSaveGo.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='GO';");
        btnSaveExit.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='EXIT';");
        btnCancel.Attributes.Add("onclick", "javascript:window.close();");;
    }

    //  公司下拉式選單連動程式
    void DDL_SelectedIndexChanged ( object sender , EventArgs e )
    {
        DropDownList DDL1 = ( DropDownList ) sender;
        string tmp = ( ( DropDownList ) sender ).SelectedValue;

        if ( DDL1.ID == "Company" )
        {
            string SQLstr = "SELECT EmployeeId,EmployeeName FROM Personnel_Master Where Company ='" + tmp + "'";
            DataTable DT2 = _MyDBM.ExecuteDataTable ( SQLstr );
            DropDownList DDL = ( DropDownList ) DetailsView1.FindControl ( "EmployeeId" );
            DDL.Items.Clear ( );
            ( ( TextBox ) DetailsView1.Rows[ 2 ].Cells[ 1 ].Controls[ 0 ] ).Text = "";
            if ( DT2.Rows.Count < 1 ) DDL.Items.Add ( new ListItem ( "無資料" , " " ) );
            else
            {
                DDL.Items.Add ( new ListItem ( "無資料" , "" ) );
                for ( int i2 = 0 ; i2 < DT2.Rows.Count ; i2++ )
                {
                    DDL.Items.Add ( new ListItem ( DT2.Rows[ i2 ][ "EmployeeId" ].ToString ( ) + " " + DT2.Rows[ i2 ][ "EmployeeName" ].ToString ( ) , DT2.Rows[ i2 ][ "EmployeeId" ].ToString ( ) ) );
                }
            }
        }
        if ( DDL1.ID == "EmployeeId" )
        {
            DropDownList CompanyDDL = ( DropDownList ) DetailsView1.FindControl ( "Company" );
            string SQLstr = "SELECT [IDNo] FROM [dbo].[Personnel_Master] Where Company = '" + CompanyDDL.SelectedValue + "' And [EmployeeId]='" + DDL1.SelectedValue.Trim ( ) + "'";
            DataTable DT2 = _MyDBM.ExecuteDataTable ( SQLstr );
            if ( DT2.Rows.Count > 0 )
            {
                ( ( TextBox ) DetailsView1.Rows[ 2 ].Cells[ 1 ].Controls[ 0 ] ).Text = DT2.Rows[ 0 ][ "IDNo" ].ToString ( );
            }
        }
    }


    protected void DetailsView1_ItemCreated(object sender, EventArgs e)
    {
        int[] MaxLeg = { 0, 0, 1, 8, 0, 0, 3, 3, 2, 2, 1, 1, 1, 1, 2, 0 };
        
        for (int i = 0; i < DetailsView1.Rows.Count; i++)
        {//修改欄位顯示名稱
            Ssql = "Select dbo.GetColumnTitle('Pensionaccounts_Master','" + DetailsView1.Rows[i].Cells[0].Text + "')";

            DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
            if (DT.Rows.Count > 0)
            {
                TextBox tb = new TextBox ( );
                if ( DetailsView1.Rows[ i ].Cells[ 1 ].Controls[ 0 ].GetType ( ).Name == "TextBox" )
                {
                    tb = ( TextBox ) DetailsView1.Rows[ i ].Cells[ 1 ].Controls[ 0 ];
                }

                tb.Style.Add("width", "145px");

                //  公司下拉式選單
                if (DetailsView1.Rows[i].Cells[0].Text == "Company")
                {
                    DropDownList DDL = new DropDownList();
                    DataTable DT2 = _MyDBM.ExecuteDataTable("SELECT Company,CompanyName FROM Company");
                    for (int i2 = 0; i2 < DT2.Rows.Count; i2++)
                    {
                        DDL.Items.Add ( new ListItem ( DT2.Rows[ i2 ][ "Company" ].ToString ( ) + " " + DT2.Rows[ i2 ][ "CompanyName" ].ToString ( ) , DT2.Rows[ i2 ][ "Company" ].ToString ( ) ) );
                    }
                    DDL.ID = "Company";
                    DDL.AutoPostBack = true;
                    DDL.Style.Add("width", "150px");
                    DDL.SelectedIndexChanged += new EventHandler(DDL_SelectedIndexChanged);
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                    if ( Request.QueryString != null && Request.QueryString.Count > 0 )
                        DDL.SelectedValue = Request.QueryString[ 0 ];
                    else
                        DDL.SelectedIndex = 0;
                }

                //  員工下拉式選單
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("EmployeeId"))
                {
                    DropDownList DDL = new DropDownList();
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    string SQLstr = "SELECT EmployeeId,EmployeeName FROM Personnel_Master Where Company ='" + Request.QueryString[0] + "'";
                    DataTable DT2 = _MyDBM.ExecuteDataTable(SQLstr);
                    if ( DT2.Rows.Count < 1 ) DDL.Items.Add ( "無資料" );
                    else
                    {
                        DDL.Items.Add ( "請選擇" );
                        for ( int i2 = 0 ; i2 < DT2.Rows.Count ; i2++ )
                        {
                            DDL.Items.Add ( new ListItem ( DT2.Rows[ i2 ][ "EmployeeId" ].ToString ( ) + " " + DT2.Rows[ i2 ][ "EmployeeName" ].ToString ( ) , DT2.Rows[ i2 ][ "EmployeeId" ].ToString ( ) ) );
                        }
                    }
                    DDL.ID = "EmployeeId";
                    DDL.AutoPostBack = true;
                    DDL.SelectedIndexChanged += new EventHandler ( DDL_SelectedIndexChanged );
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                }

                switch ( i )
                {
                    //勞工退休金個人專戶
                    case 2:
                        tb.MaxLength = 12;
                        break;
                    case 5:
                        //tb.ReadOnly = true;
                        break;
                    case 6:
                    case 7:
                        tb.MaxLength = 3;
                        break;
                    case 8:
                        tb.MaxLength = 6;
                        break;
                    case 9:
                        ASP.usercontrol_salaryym_ascx YM1 = new ASP.usercontrol_salaryym_ascx ( );
                        YM1 = ( ASP.usercontrol_salaryym_ascx ) LoadControl ( "~/UserControl/SalaryYM.ascx" );
                        YM1.SetOtherYMList ( DateTime.Today.Year - 50 , DateTime.Today.Year , "" );
                        YM1.ID = "YM1";
                        tb.Visible = false;
                        tb.Parent.Controls.Add ( YM1 );
                        break;
                    case 10:
                        ASP.usercontrol_salaryym_ascx YM2 = new ASP.usercontrol_salaryym_ascx ( );
                        YM2 = ( ASP.usercontrol_salaryym_ascx ) LoadControl ( "~/UserControl/SalaryYM.ascx" );
                        YM2.SetOtherYMList ( DateTime.Today.Year - 50 , DateTime.Today.Year , "" );
                        YM2.Visible = true;
                        YM2.ID = "YM2";
                        tb.Visible = false;
                        tb.Parent.Controls.Add ( YM2 );
                        break;


                }

                


                //  退休金制度選擇碼
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("OldnewCode"))
                {
                    tb.Visible = false;
                    RadioButtonList rbl = new RadioButtonList();
                    rbl.ID = "OldnewCode_rbl";
                    rbl.RepeatDirection = RepeatDirection.Horizontal;
                    for (int i2 = 0; i2 < 2; i2++)
                    {
                        ListItem li = new ListItem();
                        switch (i2)
                        {
                            case 0:
                                li.Text = "新制";
                                li.Value = "1";
                                break;
                            case 1:
                                li.Text = "舊制";
                                li.Value = "2";
                                break;
                        }
                        rbl.SelectedValue = "1";
                        rbl.Items.Add(li);

                    }
                    DetailsView1.Rows[i].Cells[1].Controls.Add(rbl);
                }

                //  新制選擇日期
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("NewSystem_Date"))
                {
                    tb.CssClass = "JQCalendar";
                }

                //  生效日期選擇原件
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("EffectiveDate"))
                {
                    tb.CssClass = "JQCalendar";
                }

                DetailsView1.Rows[i].Cells[0].Text = DT.Rows[0][0].ToString().Trim();
                
                //  異動類別設定
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("TrxType"))
                {
                    DetailsView1.Rows[i].Cells[0].Text = "異動類別";
                    tb.Visible = false;
                    RadioButtonList rbl = new RadioButtonList();
                    rbl.ID = "rbl";
                    rbl.RepeatDirection = RepeatDirection.Horizontal;
                    rbl.Items.Add ( new ListItem ( "加保" , "1" ) );
                    rbl.Items.Add ( new ListItem ( "調整" , "2" ) );
                    rbl.Items.Add ( new ListItem ( "退保" , "3" ) );
                    rbl.SelectedValue = "1";
                    DetailsView1.Rows[i].Cells[1].Controls.Add(rbl);
                }

            }
        }
    }

    protected void DetailsView1_DataBound ( object sender , EventArgs e )
    {

    }


    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {

        string Err = "", InsertItem = "", InsertValue = "";
        DetailsView dv = (DetailsView)sender;

        


        //  @   檢查資料是否重複    @
        if (Err == "")
        {
            string tmp1 = ((DropDownList)dv.Rows[0].Cells[1].Controls[1]).SelectedItem.Text;
            if (tmp1.Contains(" ")) tmp1 = tmp1.Remove(tmp1.IndexOf(" "));
            string tmp2 = ((DropDownList)dv.Rows[1].Cells[1].Controls[1]).SelectedItem.Text;
            if (tmp2.Contains(" ")) tmp2 = tmp2.Remove(tmp2.IndexOf(" "));

            if (!ValidateData(tmp1, tmp2)) Err = "資料重複!!";
        }

        if (Err == "")
        {
            for (int i = 0; i < dv.Rows.Count; i++)
            {
                #region @       下拉式選單      @
                if (i == 0 || i == 1)
                {
                    DropDownList ddl = (DropDownList)dv.Rows[i].Cells[1].Controls[1];
                    string tmp = ddl.SelectedItem.Text;
                    if (tmp.Contains(" ")) tmp = tmp.Remove(tmp.IndexOf(" "));
                    e.Values[i] = tmp;
                }

                #endregion

                #region @   RadioList
                if (i == 3 || i == 4)
                {
                    RadioButtonList rdl = (RadioButtonList)dv.Rows[i].Cells[1].Controls[1];
                    string tmp = rdl.SelectedItem.Value;
                    if (tmp.Contains(" ")) tmp = tmp.Remove(tmp.IndexOf(" "));
                    e.Values[i] = tmp;
                }
                #endregion

                #region @   日期格式轉換    @
                if (( i == 5 || i == 11 ) && e.Values[ i ] != null )
                {
                    e.Values[i] = _UserInfo.SysSet.ToADDate(e.Values[i].ToString());
                }
                #endregion
                if ( i == 9 || i == 10  )
                {
                    ASP.usercontrol_salaryym_ascx YM = ( ASP.usercontrol_salaryym_ascx ) dv.Rows[ i ].Cells[ 1 ].Controls[ 1 ];
                    
                    e.Values[ i ] = YM.SelectSalaryYM;
                }

                decimal decTemp = 0;
                try
                {//固定資料存入DB之格式為比率
                    decTemp = Convert.ToDecimal(e.Values["Emp_rate"]);
                    if (decTemp > 1)
                        e.Values["Emp_rate"] = decTemp / 100;
                }
                catch { }
                try
                {//固定資料存入DB之格式為比率
                    decTemp = Convert.ToDecimal(e.Values["CompanyRate"]);
                    if (decTemp > 1)
                        e.Values["CompanyRate"] = decTemp / 100;
                }
                catch { }

                try
                {//準備寫入LOG用之參數                                
                    InsertItem += DetailsView1.Rows[i].Cells[0].Text.Trim() + "|";
                    InsertValue += e.Values[i].ToString() + "|";
                }
                catch
                { }
            }
        }


        if (Err != "")
        {//檢核失敗時,要顯示訊息
            ShowMsgBox1.Message = Err;
            lbl_Msg.Text = Err;
            e.Cancel = true;
        }
        else
        {//檢核成功後要做的處理
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
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "PersonnelAdjustment";
            MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "A";
            MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = (InsertItem.Length * 2 > 255) ? "ALL" : InsertItem;
            MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = InsertValue;
            MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
            //此時不設定異動結束時間
            //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
            MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion
        }

    }

    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        StringBuilder str;

        if (e.Exception == null)
        {
            lbl_Msg.Text = "新增成功!!";
        }
        else
        {
            lbl_Msg.Text = "新增失敗!!  原因: " + e.Exception.Message;
            e.ExceptionHandled = true;
        }

        #region 完成異動後,更新LOG資訊
        MyCmd.Parameters["@SQLcommand"].Value = (e.Exception == null) ? "Success" : lbl_Msg.Text;
        MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        if (e.Exception != null)
        {
            return;
        }

        if (hid_InserMode.Value.Trim().Equals("EXIT"))
        {
            ScriptManager.RegisterStartupScript ( this.Page , this.Page.GetType ( ) , "msg" , "window.close();window.opener.location='Pensionaccounts.aspx';" , true );
        }
    }


    private bool ValidateData(string id1, string id2)
    {
        Ssql = "Select * From Pensionaccounts_Master Where Company=" + SqlName(id1) +
            "And EmployeeId =" + SqlName(id2);
        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);

        if (tb.Rows.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    private string SqlName(string str)
    {
        return "'" + str + "' ";
    }


    protected void btnSave_Click(object sender, ImageClickEventArgs e)
    {
        DetailsView1.InsertItem(true);
    }
}
