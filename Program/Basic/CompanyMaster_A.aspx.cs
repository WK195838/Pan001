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


public partial class CompanyMaster_A : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYB011";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    string[] FileName = { 
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

        btnSaveGo.Attributes.Add( "onclick", "javascript:" + hid_InserMode.ClientID + ".value='GO';" );
        btnSaveExit.Attributes.Add( "onclick", "javascript:" + hid_InserMode.ClientID + ".value='EXIT';" );
        btnCancel.Attributes.Add( "onclick", "javascript:window.close();" );


        bool blCheckLogin = _UserInfo.AuthLogin;
        if ((blCheckLogin == false) || (ConfigurationManager.AppSettings["EnableProgramAuth"] != null && ConfigurationManager.AppSettings["EnableProgramAuth"].ToString() == "true"))
        {
            bool blCheckProgramAuth = false;
            if (blCheckLogin == false)
                ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg("UnLogin");
            else
            {
                blCheckProgramAuth = _UserInfo.CheckPermission(SysSetting.SystemName.SYS, _ProgramId, "Add");
                if (blCheckProgramAuth == false)
                    ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg("NoRight");
            }
        }
        if(!Page.IsPostBack)
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

        //InsertParameters
        for ( int i = 0; i < FileName.Length; i++ )
        {
            Parameter Name = new Parameter();
            Name.Name = FileName[i];
            SqlDataSource1.InsertParameters.Add( Name );
        }

    }

    //SqlDataSource 資料設定
    protected void SetSqlDataSource()
    {
        string tmp = "";

        //InsertCommand
        tmp = "INSERT INTO Company(";
        for ( int i = 0; i < FileName.Length; i++ )
            tmp += FileName[i] + ",";

        tmp = tmp.TrimEnd( ',' ) + ") VALUES (";

        for ( int i = 0; i < FileName.Length; i++ )
            tmp += "@" + FileName[i] + ",";

        tmp = tmp.TrimEnd( ',' ) + ") ";
        SqlDataSource1.InsertCommand = tmp;


    }

    protected void DetailsView1_ItemCreated(object sender, EventArgs e)
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
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("DependentsNum") || DetailsView1.Rows[i].Cells[0].Text.Contains("SpecialSeniority"))
                {//數字欄位需靠右對齊
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("text-align", "right");
                    //預設為0
                    tb.Text = "0";
                }

                DetailsView1.Rows[i].Cells[0].Text = DT.Rows[0][0].ToString().Trim();
                //日期欄位
                if (DT.Rows[0][0].ToString().Trim().Contains("日"))
                {//日期欄位靠右對齊
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("text-align", "right");

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

    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        string Err = "";
        string InsertItem = "", InsertValue = "";

        if (e.Values["Company"] == null)
        {
            Err = "公司代號不可空白<br>";
        }

        if (e.Values["CompanyName"] == null)
        {
            Err += "公司全名不可空白<br>";
        }

        if (Err.Equals(""))
        {
            if (!ValidateData(e.Values["Company"].ToString()))
            {
                Err += "公司代號重覆!!";
            }
        }

        DetailsView dv = (DetailsView)sender;
        for (int i = 0; i < dv.Rows.Count; i++)
        {

            if (e.Values[i] == null)
            {                
                e.Values[i] = " ";
            }
            //日期欄位
            if (dv.Rows[i].Cells[0].Text.Contains("日"))
            {
                e.Values[i] = _UserInfo.SysSet.FormatADDate(e.Values[i].ToString());
            }

            try
            {//準備寫入LOG用之參數                                
                InsertItem += DetailsView1.Rows[i].Cells[0].Text.Trim() + "|";
                InsertValue += e.Values[i].ToString() + "|";
            }
            catch
            { }
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
            str = new StringBuilder();
            str.Append("<script language=javascript>");
            str.Append("window.opener.location='CompanyMaster.aspx';");
            str.Append("window.close();");
            str.Append("</script>");
            Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", str.ToString());
        }
    }

    private bool ValidateData(string Company)
    {
        Ssql = "Select * From Company Where Company='" + Company +"'";
        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);
        return ( tb.Rows != null && tb.Rows.Count > 0 ) ? false : true;
    }
    protected void btnSave_Click(object sender, ImageClickEventArgs e)
    {
        DetailsView1.InsertItem(true);
    }
}
