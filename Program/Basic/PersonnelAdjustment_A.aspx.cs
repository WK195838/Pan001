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

public partial class Basic_PersonnelAdjustment_A : System.Web.UI.Page
{
    #region #   變數宣告
    string Ssql = "";
    SysSetting SysSet = new SysSetting();
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM006";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    #endregion

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
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/PersonnelAdjustment.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";
        ScriptManager.RegisterStartupScript ( UP1 , this.GetType ( ) , "" , @"fn();" , true ); 
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
 
        btnSaveGo.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='GO';");
        btnSaveExit.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='EXIT';");
        btnCancel.Attributes.Add("onclick", "javascript:window.close();");
    }
    // 畫面物件建立事件
    protected void DetailsView1_ItemCreated(object sender, EventArgs e)
    {
        for (int i = 0; i < DetailsView1.Rows.Count; i++)
        {//修改欄位顯示名稱
            Ssql = "Select dbo.GetColumnTitle('PersonnelAdjustment','" + DetailsView1.Rows[i].Cells[0].Text + "')";

            DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
            if (DT.Rows.Count > 0)
            {
                TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                DropDownList DDL = new DropDownList();
                DDL.Style.Add("width", "220px");
                DDL.AutoPostBack = true;

                string text = DetailsView1.Rows[i].Cells[0].Text;
                tb.Width = 220;

                switch (text)
                {
                    case "Company"://  公司
                    case "EmployeeId"://  員工
                    case "DepCode_T"://  部門（至）
                    case "Title_T"://  職稱（至）
                    case "Level_T"://  職等（至）
                    case "SalarySystem_T"://  薪制（至）
                    case "Class_T"://  班別（至）
                        DDL.ID = text;
                        DDL.SelectedIndexChanged += new EventHandler(DDL_SelectedIndexChanged);
                        if (text == "Company")
                        {
                            DataTable DT2 = _MyDBM.ExecuteDataTable("SELECT Company,CompanyName FROM Company");
                            DDL.Items.Add(GetListItem("請選擇" , ""));
                            for (int i2 = 0 ; i2 < DT2.Rows.Count ; i2++)
                            {
                                DDL.Items.Add(GetListItem(DT2.Rows[i2]["Company"].ToString() + " - " + DT2.Rows[i2]["CompanyName"].ToString() , DT2.Rows[i2]["Company"].ToString()));
                            }
                        }
                        else
                        {
                            DDL.Enabled = false;
                        }

                        
                        tb.Visible = false;
                        DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                        break;

                    case "DepCode_F"://  部門（自）
                    case "Title_F"://  職稱（自）
                    case "Level_F"://  職等（自）
                    case "SalarySystem_F"://  薪制（自）
                    case "Class_F"://  班別（自）
                        tb.Enabled = false;
                        break;

                    //  調動類別
                    case "AdjustmentCategory":
                        DataTable DT3 = _MyDBM.ExecuteDataTable("SELECT CodeDesc.CodeName FROM CodeDesc Where CodeID = 'PY#Adjustm'");
                        tb.Visible = false;
                        RadioButtonList rbl = new RadioButtonList();
                        rbl.ID = "OldnewCode_rbl";
                        rbl.RepeatDirection = RepeatDirection.Horizontal;
                        for (int i2 = 0; i2 < DT3.Rows.Count; i2++)
                        {
                            rbl.SelectedIndex = 0;
                            rbl.Items.Add(GetListItem(DT3.Rows[i2]["CodeName"].ToString() , i2 == 0 ? "A" : i2 == 1 ? "B" : i2 == 2 ? "C" : i2 == 3 ? "D" : ""));
                        }
                        rbl.SelectedIndexChanged += new EventHandler(rbl_SelectedIndexChanged);
                        DetailsView1.Rows[i].Cells[1].Controls.Add(rbl);
                        break;

                    //  生效日期
                    case "EffectiveDate":
                        tb.CssClass = "JQCalendar";
                        break;


                    //  原因
                    case "ResignReason":
                        break;

                    //  主檔更新
                    case "MasterUpdate":
                        tb.Text = "否";
                        DetailsView1.Rows[i].Visible = false;
                        break;

                }

                DetailsView1.Rows[i].Cells[0].Text = DT.Rows[0][0].ToString().Trim();
            }
        }
        if (!Page.IsPostBack)
        {

            DDLSelectSet(((DropDownList)DetailsView1.Rows[0].Cells[1].Controls[1]) , Request.QueryString["C"]);
            DDL_SelectedIndexChanged(((DropDownList)DetailsView1.Rows[0].Cells[1].Controls[1]) , e);
            if (Request.QueryString["E"].Length > 3)
            {
                DDLSelectSet(((DropDownList)DetailsView1.Rows[1].Cells[1].Controls[1]) , Request.QueryString["E"]);
                DDL_SelectedIndexChanged(((DropDownList)DetailsView1.Rows[1].Cells[1].Controls[1]) , e);
            }
        }
    }
    //  選單變化觸發事件
    void rbl_SelectedIndexChanged(object sender , EventArgs e)
    {
       
    }

    //  下拉式選單觸發事件
    void DDL_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList DL = (DropDownList)sender;
        string tmp = DL.SelectedItem.ToString();
        tmp = tmp.Contains(" ") ? tmp.Remove(tmp.IndexOf(" ")) : tmp;
        DataTable DT = new DataTable();

        DropDownList[] RDDL = {
            (DropDownList)DetailsView1.Rows[5].Cells[1].Controls[1],
            (DropDownList)DetailsView1.Rows[7].Cells[1].Controls[1],
            (DropDownList)DetailsView1.Rows[9].Cells[1].Controls[1],
            (DropDownList)DetailsView1.Rows[11].Cells[1].Controls[1],
            (DropDownList)DetailsView1.Rows[13].Cells[1].Controls[1]
        };

        TextBox[] TBOX = {
            (TextBox)DetailsView1.Rows[4].Cells[1].Controls[0],
            (TextBox)DetailsView1.Rows[6].Cells[1].Controls[0],
            (TextBox)DetailsView1.Rows[8].Cells[1].Controls[0],
            (TextBox)DetailsView1.Rows[10].Cells[1].Controls[0],
            (TextBox)DetailsView1.Rows[12].Cells[1].Controls[0],
        };

        TextBox T4 = (TextBox)DetailsView1.Rows[4].Cells[1].Controls[0];
        TextBox T6 = (TextBox)DetailsView1.Rows[6].Cells[1].Controls[0];
        TextBox T8 = (TextBox)DetailsView1.Rows[8].Cells[1].Controls[0];
        TextBox T10 = (TextBox)DetailsView1.Rows[10].Cells[1].Controls[0];
        TextBox T12 = (TextBox)DetailsView1.Rows[12].Cells[1].Controls[0];
        
        switch (DL.ID)
        { 
            // 公司
            case "Company":
                DropDownList DDL = ((DropDownList)DetailsView1.Rows[1].Cells[1].Controls[1]);
                DDL.Items.Clear();
                SetDDL ( "SELECT EmployeeId,EmployeeName FROM Personnel_Master Where Company ='" + tmp + "' Order by EmployeeId" , DDL , "EmployeeId" , "EmployeeName" );
                TBOX[0].Text = "";
                break;

            //  員工
            case"EmployeeId":
               
            //取得選取的公司編號
                string tmpCompany = ((DropDownList)DetailsView1.Rows[0].Cells[1].Controls[1]).SelectedValue;
                
            //資料庫查詢陣列
                String[] SqlCmd = { 
                    //0部門
                    "SELECT DepCode,DepName FROM Department Where Company ='" + tmpCompany + "'",
                    //1職稱
                    "Select CodeCode,CodeName from CodeDesc Where CodeID ='PY#TitleCo'",
                    //2職等
                    "SELECT SalaryLevel_CheckStandard.Level FROM SalaryLevel_CheckStandard",
                    //3薪制
                    "Select CodeCode,CodeName from CodeDesc Where CodeID ='PY#PayCode'",
                    //4班別
                    "SELECT CodeCode,CodeName FROM CodeDesc  Where CodeID ='PY#Shift'"
                };

                for (int i = 0; i < SqlCmd.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            SetDDL(SqlCmd[i], RDDL[i], "DepCode","DepName");
                            break;
                        case 1:
                        case 3:
                        case 4:
                            SetDDL(SqlCmd[i], RDDL[i], "CodeCode", "CodeName");
                            break;
                        case 2:
                            SetDDL(SqlCmd[i], RDDL[i], "Level", "");
                            break;
                    }

                    TBOX[i].Text = "";
                }


                DT = _MyDBM.ExecuteDataTable("Select A.DepCode,A.DepName,B.TitleCode,B.Grade,B.PayCode,B.Shift from Department as A ,Personnel_Master as B Where A.DepCode = B.DeptId and  B.EmployeeId = '" + tmp + 
                    "' and A.Company = B.Company and B.Company = '"+ tmpCompany + "'");
                if (DT.Rows.Count > 0)
                {
                    //部門（自）
                    T4.Text = DT.Rows[0]["DepCode"].ToString()+ " - " + DT.Rows[0]["DepName"].ToString();
                    //部門（至）
                    //DDLSelectSet(RDDL[0] , DT.Rows[0]["DepCode"].ToString());

                    //職稱（自）- 職稱（至）
                    DataTable DT2 = _MyDBM.ExecuteDataTable("Select CodeCode,CodeName from CodeDesc as C Where C.CodeId = 'PY#TitleCo' and C.CodeCode = '" + DT.Rows[0]["TitleCode"].ToString().Trim() + "'");
                    if (DT2.Rows.Count > 0)
                    {
                        T6.Text = DT2.Rows[0]["CodeCode"].ToString() + " - " + DT2.Rows[0]["CodeName"].ToString();
                        //DDLSelectSet(RDDL[1] , DT2.Rows[0]["CodeCode"].ToString());
                    }
                    else
                        T6.Text = DT.Rows[0]["TitleCode"].ToString();

                    //職等（自）
                    T8.Text = DT.Rows[0]["Grade"].ToString();
                    //職等（至）
                    //DDLSelectSet(RDDL[2] , DT.Rows[0]["Grade"].ToString());

                    //薪制（至）
                    //DDLSelectSet(RDDL[3] , DT.Rows[0]["PayCode"].ToString());
                    //薪制（自）
                    T10.Text = RDDL[3].SelectedItem.Text;


                    //班別（至）
                    //DDLSelectSet(RDDL[4] , DT.Rows[0]["Shift"].ToString());
                    //班別（自）
                    T12.Text = RDDL[4].SelectedItem.Text;
                    

                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        RDDL[i].SelectedIndex = 0;
                        TBOX[i].Text = "無資料";
                    }
                }
                
                break;
        }


    }

    //設定下拉式選單預選功能
    private void DDLSelectSet(DropDownList DDL,string Value)
    {
        DDL.SelectedIndex = DDL.Items.IndexOf(DDL.Items.FindByValue(Value));
    }

    //下拉式選單快速設定
    private void SetDDL(string SqlCmd,DropDownList DDL,string Id,string Name)
    {
        DataTable DT = _MyDBM.ExecuteDataTable(SqlCmd);
        DDL.Items.Clear();
        if (DT.Rows.Count < 1)
            DDL.Items.Add(GetListItem("無資料", ""));
        else
        {
            DDL.Items.Add(GetListItem("請選擇", ""));
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                DDL.Items.Add( GetListItem( Name == "" ? DT.Rows[i][Id].ToString() : DT.Rows[i][Id].ToString() + " - " + DT.Rows[i][Name].ToString() , DT.Rows[i][Id].ToString() ) );
                DDL.Enabled = true;
            }
        }
    }

    private ListItem GetListItem(string text,string value)
    {
        ListItem li = new ListItem();
        li.Text = text;
        li.Value = value.Trim();
        return li;
    }


    //新增中
    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {

        string Err = "", InsertItem = "", InsertValue = "";
        DetailsView dv = (DetailsView)sender;
      
        //整理所有要存入的數值
        for (int i = 0; i < e.Values.Count - 2; i++)
        {
            if (dv.Rows[i].Cells[1].Controls.Count > 1 && i!= 2 && i!=3)
            {
                e.Values[i] = ((DropDownList)dv.Rows[i].Cells[1].Controls[1]).SelectedValue.Trim();
            }
            else if (i == 2)
            {
                e.Values[i] = ((RadioButtonList)dv.Rows[i].Cells[1].Controls[1]).SelectedValue.Trim();
            }
            else
            {
                try
                {
                     
                    if ( e.Values[ i ].ToString ( ).Contains ( " " ) )
                        e.Values[ i ] = e.Values[ i ].ToString ( ).Remove ( e.Values[ i ].ToString ( ).IndexOf ( " " ) );
                    if ( e.Values[ i ].ToString ( ).Contains ( "請選擇" ) )
                        e.Values[ i ] = "";
                }
                catch
                {

                }
            }
        }

        //檢查下拉式選單是否有值
        int[ ] ddllist = { 0 , 1 , 3 , 5 , 7 , 9 , 11 , 13 };
        int n = 0;
        for ( int i = 0 ; i < ddllist.Length ; i++ )
        {
            //0 1 3 一定要有值
            //0,1,3,5,7,9,11,13
            if (e.Values[ddllist[i]] == null || e.Values[ddllist[i]].ToString() == "")
            {
                if (i == 0 || i == 1)
                {
                    Err += dv.Rows[ddllist[i]].Cells[0].Text + " 未選擇. <br />";
                }
                else
                    n++;
            }
           
        }
        if ( n == 5 )
            Err += " 至少要有一項異動 <br />";

        //檢查日期欄位是否正確
        try
        {
            DateTime.Parse(e.Values[3].ToString());
            e.Values[3] = SysSet.FormatADDate(e.Values[3].ToString());
        }
        catch
        {
            Err += "日期格式錯誤，應為民國 yy/MM/dd<br />";
        }


        //  @   檢查資料是否重複    @
        if (Err == "")
        {
            if (!ValidateData(e.Values[0].ToString(),e.Values[1].ToString(), e.Values[2].ToString(),e.Values[3].ToString() )) Err = "資料重複!!";
        }

        if (Err == "")
        {
            for (int i = 0; i < dv.Rows.Count; i++)
            {
                //主檔更新欄位轉換成 Y/N
                if (i == dv.Rows.Count - 1)
                {
                    e.Values[i] = e.Values[i].ToString().Replace("否", "N");
                }

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
            ScriptManager.RegisterStartupScript ( this.Page , this.Page.GetType ( ) , "msg" , "window.close();window.opener.location='LaborInsurance.aspx';" , true );
        }
    }

    private bool ValidateData(string id1,string id2,string id3,string id4)
    {
        Ssql = "Select * From PersonnelAdjustment Where Company=" + SqlName(id1) +
            "And EmployeeId =" + SqlName(id2) +
            "And AdjustmentCategory ="+SqlName(id3)+
            "And (Convert(varchar,EffectiveDate,111) =" + SqlName(id4)+")";

        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);
        return tb.Rows.Count > 0 ? false : true;


    }

    private string SqlName(string str)
    {
       return "'"+str+"' ";
    }


    protected void btnSave_Click(object sender, ImageClickEventArgs e)
    {
        DetailsView1.InsertItem(true);
    }
}
