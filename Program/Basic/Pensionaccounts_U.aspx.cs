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

public partial class Basic_SalaryStructureParameter_U : System.Web.UI.Page
{
    #region 變數宣告
    //#---------------------------#//
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo ( );
    string _ProgramId = "PYM009";
    DBManger _MyDBM;
    SysSetting SysSet = new SysSetting ( );
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand ( );
    //#---------------------------#//
    #endregion

    protected void Page_PreInit ( object sender , EventArgs e )
    {//頁面初始化前,先設定好系統使用的佈景主題(主要是決定LOGO)
        if ( Session[ "Theme" ] != null )
            Page.Theme = Session[ "Theme" ].ToString ( );

        if ( Session[ "MasterPage" ] != null )
            Page.MasterPageFile = "~/" + Session[ "MasterPage" ].ToString ( ) + ".master";
    }

    protected override void OnInit ( EventArgs e )
    {
        base.OnInit ( e );
        _MyDBM = new DBManger ( );
        _MyDBM.New ( );
        DataTable DT = _MyDBM.ExecuteDataTable ( "Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/SalaryStructureParameter.aspx'" );
        if ( DT.Rows.Count > 0 )
            _ProgramId = DT.Rows[ 0 ][ 0 ].ToString ( );
    }

    protected void Page_Load ( object sender , EventArgs e )
    {
        ScriptManager.RegisterStartupScript ( UP1 , this.GetType ( ) , "" , @"JQ();" , true );
        lbl_Msg.Text = "";
        bool blCheckLogin = _UserInfo.AuthLogin;
        if ( ( blCheckLogin == false ) || ( ConfigurationManager.AppSettings[ "EnableProgramAuth" ] != null && ConfigurationManager.AppSettings[ "EnableProgramAuth" ].ToString ( ) == "true" ) )
        {
            bool blCheckProgramAuth = false;
            if ( blCheckLogin == false )
                ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg ( "UnLogin" );
            else
            {
                blCheckProgramAuth = _UserInfo.CheckPermission ( _ProgramId , "Modify" );
                if ( blCheckProgramAuth == false )
                    ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg ( "NoRight" );
            }
        }

        btnCancel.Attributes.Add ( "onclick" , "javascript:window.close();" );
    }

    protected void DetailsView1_ItemCreated ( object sender , EventArgs e )
    {
        for ( int i = 0 ; i < DetailsView1.Rows.Count ; i++ )
        {
            RadioButtonList rbl = new RadioButtonList ( );
            rbl.RepeatDirection = RepeatDirection.Horizontal;

            string[ ] n = { DetailsView1.Rows[ i ].Cells[ 0 ].Text };

            switch ( n[ 0 ] )
            {
                //退休金制度選擇碼
                case "OldnewCode":
                    rbl.ID = "OldnewCode";
                    for ( int i2 = 0 ; i2 < 2 ; i2++ )
                    {
                        ListItem li = new ListItem ( );
                        li.Text = i2 == 0 ? "新制" : "舊制";
                        li.Value = i2 == 0 ? "1" : "2";
                        rbl.Items.Add ( li );
                    }
                    DetailsView1.Rows[ i ].Cells[ 1 ].Controls.Add ( rbl );
                    break;

                //日期元件
                case "EffectiveDate":
                case "NewSystem_Date":
                    break;

                //異動類別設定
                case "TrxType":
                    rbl.ID = "TrxType";
                    for ( int i2 = 0 ; i2 < 3 ; i2++ )
                    {
                        ListItem li = new ListItem ( );
                        li.Text = i2 == 0 ? "加保" : i2 == 1 ? "調整" : "退保";
                        li.Value = i2 == 0 ? "0" : i2 == 1 ? "1" : "2";
                        rbl.Items.Add ( li );
                    }
                    DetailsView1.Rows[ i ].Cells[ 1 ].Controls.Add ( rbl );
                    break;

                //員工自提率變更年度
                case "EmpRate_changeyear":
                    ASP.usercontrol_salaryym_ascx YM1 = new ASP.usercontrol_salaryym_ascx ( );
                    YM1 = ( ASP.usercontrol_salaryym_ascx ) LoadControl ( "~/UserControl/SalaryYM.ascx" );
                    YM1.ID = "EmpRate_changeyear";
                    DetailsView1.Rows[ i ].Cells[ 1 ].Controls.Add ( YM1 );
                    break;


                //企業提撥率變更年度
                case "CompanyRate_changeyear":
                    ASP.usercontrol_salaryym_ascx YM2 = new ASP.usercontrol_salaryym_ascx ( );
                    YM2 = ( ASP.usercontrol_salaryym_ascx ) LoadControl ( "~/UserControl/SalaryYM.ascx" );
                    YM2.ID = "CompanyRate_changeyear";
                    DetailsView1.Rows[ i ].Cells[ 1 ].Controls.Add ( YM2 );
                    break;


            }
        }
    }

    protected void DetailsView1_DataBound ( object sender , EventArgs e )
    {
        for ( int i = 0 ; i < DetailsView1.Rows.Count ; i++ )
        {

            //修改欄位顯示名稱
            Ssql = "Select dbo.GetColumnTitle('Pensionaccounts_Master','" + DetailsView1.Rows[ i ].Cells[ 0 ].Text + "')";
            
            if ( DetailsView1.Rows[ i ].Cells[ 0 ].Text == "TrxType" )
                Ssql = "Select dbo.GetColumnTitle('Pensionaccounts_Transaction','" + DetailsView1.Rows[ i ].Cells[ 0 ].Text + "')";
            DataTable DT = _MyDBM.ExecuteDataTable ( Ssql );
            DataTable DT2 = new DataTable ( );
            TextBox tb = new TextBox ( );
            if ( DT.Rows.Count > 0 )
            {
                string[ ] n ={ DetailsView1.Rows[ i ].Cells[ 0 ].Text , DetailsView1.Rows[ i ].Cells[ 1 ].Text };
                DetailsView1.Rows[ i ].Cells[ 0 ].Width = 200;


                if ( i != 0 && i != 1 && i != 2 && i != 12 )
                {
                    try
                    { 
                    tb = ( TextBox ) DetailsView1.Rows[ i ].Cells[ 1 ].Controls[ 0 ];
                    tb.Text = tb.Text.Trim ( );
                    tb.Style.Add ( "width" , "140px" );
                    tb.Style.Add ( "text-align" , "right" );
                    }
                    catch { }
                }

                switch ( n[ 0 ] )
                {
                    case "Company":
                    case "EmployeeId":
                        Ssql = "Select ";
                        Ssql += n[ 0 ] == "Company" ? " CompanyName From Company Where Company" : "EmployeeName From Personnel_Master Where EmployeeId";
                        Ssql += " = '" + n[ 1 ] + "'";
                        DT2 = _MyDBM.ExecuteDataTable ( Ssql );
                        if ( DT2.Rows.Count > 0 )
                            DetailsView1.Rows[ i ].Cells[ 1 ].Text += " " + DT2.Rows[ 0 ][ 0 ].ToString ( ).Trim ( );
                        break;

                    //退休金制度設定
                    //異動類別設定
                    case "OldnewCode":
                    case "TrxType":
                        tb.Visible = false;
                        ( ( RadioButtonList ) tb.Parent.FindControl ( n[ 0 ] ) ).SelectedValue = tb.Text;
                        break;

                    //日期格式轉換
                    case "NewSystem_Date":
                    case "EffectiveDate":
                        tb.CssClass = "JQCalendar";
                        tb.Text = SysSet.FormatDate ( tb.Text );
                        break;

                    //員工自提率變更年度
                    case "EmpRate_changeyear":
                        tb.Visible = false;
                        ASP.usercontrol_salaryym_ascx YM1 = ( ASP.usercontrol_salaryym_ascx ) tb.Parent.FindControl ( "EmpRate_changeyear" );
                        YM1.SetOtherYMList ( DateTime.Today.Year - 50 , DateTime.Today.Year , "" );
                        YM1.SelectSalaryYM = tb.Text;
                        break;
                    //企業提撥率變更年度
                    case "CompanyRate_changeyear":
                        tb.Visible = false;
                        ASP.usercontrol_salaryym_ascx YM2 = ( ASP.usercontrol_salaryym_ascx ) tb.Parent.FindControl ( "CompanyRate_changeyear" );
                        YM2.SetOtherYMList ( DateTime.Today.Year - 50 , DateTime.Today.Year , "" );
                        YM2.SelectSalaryYM = tb.Text;
                        break;
                    //
                    case "ActualTotalamount_S":
                    case "ActualTotalamount_C":
                        decimal s;
                        if ( decimal.TryParse ( n[ 1 ] , out s ) )
                        {
                            DetailsView1.Rows[ i ].Cells[ 1 ].Text = s.ToString ( "#,0" );
                        }
                        break;
                }

                DetailsView1.Rows[ i ].Cells[ 0 ].Text = DT.Rows[ 0 ][ 0 ].ToString ( ).Trim ( );
            }
        }
    }

    protected void DetailsView1_ItemUpdating ( object sender , DetailsViewUpdateEventArgs e )
    {
        string Err = "" , tempOldValue = "";
        string UpdateItem = "" , UpdateValue = "";
        UpdateValue = "(Key=";
        //將此筆資料的KEY值找出
        for ( int i = 0 ; i < e.Keys.Count ; i++ )
        {
            UpdateValue += e.Keys[ i ].ToString ( ).Trim ( ) + "|";
        }
        UpdateValue += ")";

        DetailsView dv = ( ( DetailsView ) sender );
        //因為前1個是ReadOnly所以要扣掉,最後一個是按鈕列也要扣掉
        for ( int i = 0 ; i < dv.Rows.Count - 4 ; i++ )
        {
            try
            {
                //去空白
                if ( e.OldValues[ i ] == null )
                {
                    tempOldValue = "";
                }
                else
                {
                    tempOldValue = e.OldValues[ i ].ToString ( ).Trim ( );
                }
                if ( e.NewValues[ i ] == null )
                {
                    e.NewValues[ i ] = "";
                }
                else
                {
                    e.NewValues[ i ] = e.NewValues[ i ].ToString ( ).Trim ( );
                }
            }
            catch
            { }
            //修正新制選擇日期
            if ( i == 3 || i == 9 )
            {
                e.NewValues[ i ] = SysSet.FormatADDate ( e.NewValues[ i ].ToString ( ) );
            }

        }

        //存入選擇的選項
        e.NewValues[ "TrxType" ] = ( ( RadioButtonList ) dv.Rows[ 3 ].Cells[ 1 ].Controls[ 1 ] ).SelectedIndex.ToString ( );
        e.NewValues[ "OldnewCode" ] = ( ( RadioButtonList ) dv.Rows[ 4 ].Cells[ 1 ].Controls[ 1 ] ).SelectedValue.ToString ( );
        e.NewValues[ "EmpRate_changeyear" ] = ( ( ASP.usercontrol_salaryym_ascx ) dv.Rows[ 9 ].Cells[ 1 ].FindControl ( "EmpRate_changeyear" ) ).SelectSalaryYM;
        e.NewValues[ "CompanyRate_changeyear" ] = ( ( ASP.usercontrol_salaryym_ascx ) dv.Rows[ 10 ].Cells[ 1 ].FindControl ( "CompanyRate_changeyear" ) ).SelectSalaryYM;

        decimal decTemp = 0;
        try
        {//固定資料存入DB之格式為比率
            decTemp = Convert.ToDecimal(e.NewValues["Emp_rate"]);
            if (decTemp > 1)
                e.NewValues["Emp_rate"] = decTemp / 100;
        }
        catch { }
        try
        {//固定資料存入DB之格式為比率
            decTemp = Convert.ToDecimal(e.NewValues["CompanyRate"]);
            if (decTemp > 1)
                e.NewValues["CompanyRate"] = decTemp / 100;
        }
        catch { }

        if ( Err != "" )
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
            MyCmd.Parameters.Clear ( );
            MyCmd.Parameters.Add ( "@TableName" , SqlDbType.VarChar , 60 ).Value = "SalaryStructure_Parameter";
            MyCmd.Parameters.Add ( "@TrxType" , SqlDbType.Char , 1 ).Value = "U";
            MyCmd.Parameters.Add ( "@ChangItem" , SqlDbType.VarChar , 255 ).Value = ( UpdateItem.Length * 2 > 255 ) ? "長度:" + UpdateItem.Length.ToString ( ) : UpdateItem;
            MyCmd.Parameters.Add ( "@SQLcommand" , SqlDbType.VarChar , 2000 ).Value = UpdateValue;
            MyCmd.Parameters.Add ( "@ChgStartDateTime" , SqlDbType.SmallDateTime ).Value = StartDateTime;
            //此時不設定異動結束時間
            //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
            MyCmd.Parameters.Add ( "@ChgUser" , SqlDbType.VarChar , 32 ).Value = _UserInfo.UData.UserId;
            _MyDBM.DataChgLog ( MyCmd.Parameters );
            #endregion
        }

    }

    protected void DetailsView1_ItemUpdated ( object sender , DetailsViewUpdatedEventArgs e )
    {
        StringBuilder str;

        if ( e.Exception == null )
        {
            //
        }
        else
        {
            lbl_Msg.Text = "更新失敗!!  原因: " + e.Exception.Message;
            e.ExceptionHandled = true;
        }

        #region 完成異動後,更新LOG資訊
        MyCmd.Parameters[ "@SQLcommand" ].Value = ( e.Exception == null ) ? "Success" : lbl_Msg.Text;
        MyCmd.Parameters.Add ( "@ChgStopDateTime" , SqlDbType.SmallDateTime ).Value = DateTime.Now;
        _MyDBM.DataChgLog ( MyCmd.Parameters );
        #endregion

        if ( e.Exception != null )
        {
            return;
        }
        ScriptManager.RegisterStartupScript ( this.Page , this.Page.GetType ( ) , "msg" , "window.close();window.opener.location='Pensionaccounts.aspx?Company=" + Request.QueryString [ 0 ] + "';" , true );
    }

    protected void btnSave_Click ( object sender , ImageClickEventArgs e )
    {
        DetailsView1.UpdateItem ( true );
    }
}
