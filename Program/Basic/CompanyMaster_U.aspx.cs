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

public partial class CompanyMaster_U : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYB011";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    string[ ] FileName = { 
        "Company",
        "CompanyShortName",
        "CompanyName",
        "EnCompanyShortName",
        "EnCompanyName",
        "ChopNo",
        "TaxId",
        "CompanyTEL",
        "CompanyTEL2",
        "FactTEL",
        "FactTEL2",
        "Email",
        "CompanyAddress",
        "EnCompanyAddress",
        "FactAddress",
        "EnFactAddress",
        "CompType",
        "Boss",
        "TaxNo"
      ,"contact"
      ,"contactTEL"
      ,"HICode"
      ,"HI2EMail"                         
                         };

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
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/CompanyMaster.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";
        btnCancel.Attributes.Add( "onclick", "javascript:window.close();" );
        bool blCheckLogin = _UserInfo.AuthLogin;
        if ((blCheckLogin == false) || (ConfigurationManager.AppSettings["EnableProgramAuth"] != null && ConfigurationManager.AppSettings["EnableProgramAuth"].ToString() == "true"))
        {
            bool blCheckProgramAuth = false;
            if (blCheckLogin == false)
                ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg("UnLogin");
            else
            {
                blCheckProgramAuth = _UserInfo.CheckPermission(SysSetting.SystemName.SYS, _ProgramId, "Modify");
                if (blCheckProgramAuth == false)
                    ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg("NoRight");
            }
        }

        if ( !Page.IsPostBack )
            SetFileName();

        SetSqlDataSource();
    }

    //DetailsView 資料設定
    protected void SetFileName()
    {

        for ( int i = 0; i < FileName.Length; i++ )
        {
            BoundField Data = new BoundField();
            Data.DataField = FileName[i];
            Data.HeaderText = FileName[i];
            Data.SortExpression = FileName[i];
            DetailsView1.Fields.Add( Data );
        }

        //SelectParameters
        QueryStringParameter Key = new QueryStringParameter();
        Key.Name = FileName[0];
        Key.QueryStringField = FileName[0];
        SqlDataSource1.SelectParameters.Add( Key );



        //UpdateParameters
        for ( int i = 0; i < FileName.Length; i++ )
        {
            Parameter Name = new Parameter();
            Name.Name = FileName[i];
            SqlDataSource1.UpdateParameters.Add( Name );
        }

    }

    //SqlDataSource 資料設定
    protected void SetSqlDataSource()
    {
        string tmp = "";


        //SelectCommand
        SqlDataSource1.SelectCommand = "SELECT Company.* FROM Company WHERE Company = @Company";


        //UpdateCommand
        tmp = "UPDATE Company SET ";
        //KEY 為 Company 所以 0 跳過
        for ( int i = 1; i < FileName.Length; i++ )
        {
            tmp += FileName[i] + " = @" + FileName[i] + ",";
        }
        tmp = tmp.TrimEnd( ',' ) + " Where " + FileName[0] + " = @" + FileName[0];

        SqlDataSource1.UpdateCommand = tmp;
    }






    protected void DetailsView1_ItemCreated(object sender, EventArgs e)
    {

    }

    protected void DetailsView1_DataBound(object sender, EventArgs e)
    {
        for (int i = 0; i < DetailsView1.Rows.Count; i++)
        {
            //設定textbox的欄位限制與大小
            TextBox tmp = ( TextBox ) DetailsView1.Rows[ i ].Cells[ 1 ].Controls[ 0 ];
            tmp.Width = 400;
            int[] Max = { 2, 20, 100, 50, 200, 8, 10, 15, 15, 15, 15, 40, 100, 500, 100, 500, 1, 12, 4, 5, 15, 9, 30 };
            tmp.MaxLength = Max[ i ];

            //修改欄位顯示名稱
            Ssql = "Select dbo.GetColumnTitle('Company','" + DetailsView1.Rows[i].Cells[0].Text + "')";
            DataTable DT = _MyDBM.ExecuteDataTable(Ssql);

            if (DT.Rows != null && DT.Rows.Count > 0)
            {
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("DependentsNum") || DetailsView1.Rows[i].Cells[0].Text.Contains("SpecialSeniority")
                    || DT.Rows[0][0].ToString().Trim().Contains("日"))
                {//數字欄位需靠右對齊
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("text-align", "right");                 
                }
                DetailsView1.Rows[i].Cells[0].Width = 200;
                TextBox tmptb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                tmptb.Width = 300;
                DetailsView1.Rows[i].Cells[0].Text = DT.Rows[0][0].ToString().Trim();
                if (DT.Rows[0][0].ToString().Trim().Contains("日"))
                {//將日期欄位格式化
                    try
                    {
                        TextBox tbDate = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                        tbDate.Text = _UserInfo.SysSet.FormatDate(tbDate.Text);
                    }
                    catch { }

                    //為日期欄位增加小日曆元件
                    ImageButton btOpenCal = new ImageButton();
                    btOpenCal.ID = "btOpenCal" + i.ToString();
                    btOpenCal.SkinID = "Calendar1";
                    btOpenCal.OnClientClick = "return GetPromptDate(" + DetailsView1.Rows[i].Cells[1].Controls[0].ClientID + ");";
                    DetailsView1.Rows[i].Cells[1].Controls.Add(btOpenCal);
                }                
            }
        }
    }

    protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        string Err = "", tempOldValue = "";
        string UpdateItem = "", UpdateValue = "";

        UpdateValue = "(Key=";
        //將此筆資料的KEY值找出
        for (int i = 0; i < e.Keys.Count; i++)
        {
            UpdateValue += e.Keys[i].ToString().Trim() + "|";
        }
        UpdateValue += ")";

        DetailsView dv = ((DetailsView)sender);
        //因為前2個是ReadOnly所以要扣掉,最後一個是按鈕列也要扣掉
        for (int i = 0; i < dv.Rows.Count - 2; i++)
        {
            try
            {
                tempOldValue = e.OldValues[i].ToString().Trim();
                e.NewValues[i] = e.NewValues[i].ToString().Trim();
                if (dv.Rows[i + 1].Cells[0].Text.Trim().Contains("日"))
                {//將日期欄位格式為化為西元日期
                    e.NewValues[i] = _UserInfo.SysSet.FormatADDate(e.NewValues[i].ToString());
                    tempOldValue = _UserInfo.SysSet.FormatADDate(_UserInfo.SysSet.FormatDate(tempOldValue));
                }
                
                if (string.IsNullOrEmpty(e.NewValues[i].ToString()))
                {//將空欄位放入半形空格
                    e.NewValues[i] = " ";
                }

                if (e.NewValues[i].ToString().Trim() != tempOldValue)
                {
                    try
                    {
                        UpdateItem += dv.Rows[i + 1].Cells[0].Text.Trim() + "|";
                        UpdateValue += e.OldValues[i].ToString().Trim() + "->" + e.NewValues[i].ToString().Trim() + "|";
                    }
                    catch
                    { }
                }
            }
            catch { }
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
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "Company";
            MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "U";
            MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = (UpdateItem.Length * 2 > 255) ? "長度:" + UpdateItem.Length.ToString() : UpdateItem;
            MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = UpdateValue;
            MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
            //此時不設定異動結束時間
            //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
            MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion
        }
    }

    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        StringBuilder str;

        if (e.Exception == null)
        {
            //
        }
        else
        {
            lbl_Msg.Text = "更新失敗!!  原因: " + e.Exception.Message;
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

        str = new StringBuilder();
        str.Append("<script language=javascript>");
        str.Append("window.close();");
        str.Append("window.opener.location='CompanyMaster.aspx';");
        str.Append("</script>");
        Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", str.ToString());
    }

    protected void btnSave_Click(object sender, ImageClickEventArgs e)
    {
        DetailsView1.UpdateItem(true);
    }
}
