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

public partial class Basic_SalaryStructureParameter_A : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYB007";
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
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/SalaryStructureParameter.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
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
        btnSaveGo.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='GO';");
        btnSaveExit.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='EXIT';");
        btnCancel.Attributes.Add("onclick", "javascript:window.close();");
    }

    protected void DetailsView1_ItemCreated(object sender, EventArgs e)
    {
        //ImageButton IB;
        //switch (DetailsView1.CurrentMode)
        //{
        //    case DetailsViewMode.Insert:
        //        IB = (ImageButton)DetailsView1.FindControl("btnSaveGo");
        //        if (IB != null)
        //            IB.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='GO';");
        //        IB = (ImageButton)DetailsView1.FindControl("btnSaveExit");
        //        if (IB != null)
        //            IB.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='EXIT';");
        //        //hid_InserMode.Value = "";
        //        IB = (ImageButton)DetailsView1.FindControl("btnCancel");
        //        if (IB != null)
        //            IB.Attributes.Add("onclick", "javascript:window.close();");
        //        break;
        //}

        for (int i = 0; i < DetailsView1.Rows.Count; i++)
        {//修改欄位顯示名稱
            Ssql = "Select dbo.GetColumnTitle('SalaryStructure_Parameter','" + DetailsView1.Rows[i].Cells[0].Text + "')";

            DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
            if (DT.Rows.Count > 0)
            {
                TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                tb.Style.Add("width","145px");
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("SalaryId"))
                {//數字欄位需靠右對齊
                    tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.MaxLength = 2;
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("SalaryName"))
                {//數字欄位需靠右對齊
                    tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.MaxLength = 14;
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("FixedAmount"))
                {//數字欄位需靠右對齊
                    tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("text-align", "right");
                    tb.MaxLength = 7;
                    //預設為0
                    tb.Text = "0";
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("SalaryRate"))
                {//數字欄位需靠右對齊
                    tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("text-align", "right");
                    tb.MaxLength = 4;
                    //預設為0
                    tb.Text = "0.00";
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("BaseItem"))
                {//數字欄位需靠右對齊
                    DropDownList DDL = new DropDownList();
                    DataTable dt = _MyDBM.ExecuteDataTable("select SalaryId,SalaryName from SalaryStructure_Parameter order by SalaryId");
                    DDL.Items.Add(Li("無", ""));
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                DDL.Items.Add(Li(dt.Rows[j]["SalaryId"].ToString() + " - " + dt.Rows[j]["SalaryName"].ToString(), dt.Rows[j]["SalaryId"].ToString()));
                            }
                        }
                    }
                    //DDL.Items.Add("Y- 應稅");
                    //DDL.Items.Add("N- 免稅");
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("P1SalaryMasterList"))
                {//數字欄位需靠右對齊
                    tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("text-align", "right");
                    tb.MaxLength = 2;
                    //預設為0
                    tb.Text = "0";
                }
                else if (DetailsView1.Rows[i].Cells[0].Text.Contains("SalaryMasterList"))
                {//數字欄位需靠右對齊
                    tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("text-align", "right");
                    tb.MaxLength = 2;
                    //預設為0
                    tb.Text = "0";
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("P1Payroll"))
                {//數字欄位需靠右對齊
                    tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("text-align", "right");
                    tb.MaxLength = 2;
                    //預設為0
                    tb.Text = "0";
                }  
                else if (DetailsView1.Rows[i].Cells[0].Text.Contains("Payroll"))
                {//數字欄位需靠右對齊
                    tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("text-align", "right");
                    tb.MaxLength = 2;
                    //預設為0
                    tb.Text = "0";
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("AcctNo"))
                {//數字欄位需靠右對齊
                    tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.MaxLength = 8;
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("NWTax"))
                {//下拉式選單
                   
                    DropDownList DDL = new DropDownList();
                    string[] name={"應稅","免稅"};
                    string[] code={"Y","N"};
                    for (int j = 0; j < 2; j++)
                    {
                        ListItem li = new ListItem();
                        li.Text = code[j].Trim() + " - " + name[j].Trim();
                        li.Value = code[j].Trim();
                        DDL.Items.Add(li);
                    }
                    //DDL.Items.Add("Y- 應稅");
                    //DDL.Items.Add("N- 免稅");
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);

                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("PMType"))
                {//下拉式選單
                 
                    DropDownList DDL = new DropDownList();
                    string[] name ={ "加項", "減項" };
                    string[] code ={ "A", "B" };
                    for (int j = 0; j < 2; j++)
                    {
                        ListItem li = new ListItem();
                        li.Text = code[j].Trim() + " - " + name[j].Trim();
                        li.Value = code[j].Trim();
                        DDL.Items.Add(li);
                    }
                    //DDL.Items.Add("A- 加項");
                    //DDL.Items.Add("B- 減項");
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("ItemType"))
                {//下拉式選單
                    
                    DropDownList DDL = new DropDownList();
                    string[] name ={ "系統設定", "固定項目","變動項目" };
                    string[] code ={ "0", "1","2" };
                    for (int j = 0; j < 3; j++)
                    {
                        ListItem li = new ListItem();
                        li.Text = code[j].Trim() + " - " + name[j].Trim();
                        li.Value = code[j].Trim();
                        DDL.Items.Add(li);
                    }
                    //DDL.Items.Add("0- 系統設定");
                    //DDL.Items.Add("1- 固定項目");
                    //DDL.Items.Add("2- 變動項目");
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("SalaryType"))
                {//下拉式選單
                   
                    DropDownList DDL = new DropDownList();
                    string[] name ={ "固定金額", "薪資比率","變動金額" };
                    string[] code ={ "A", "B","C" };
                    for (int j = 0; j < 3; j++)
                    {
                        ListItem li = new ListItem();
                        li.Text = code[j].Trim() + " - " + name[j].Trim();
                        li.Value = code[j].Trim();
                        DDL.Items.Add(li);
                    }
                    //DDL.Items.Add("A- 固定金額");
                    //DDL.Items.Add("B- 薪資比率");
                    //DDL.Items.Add("C- 變動金額");
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("Properties"))
                {//下拉式選單
            
                    DropDownList DDL = new DropDownList();
                    string[] name ={ "薪資", "成本", "公司負擔", "公司獎金" };
                    string[] code ={ "1", "2", "3", "5" };
                    for (int j = 0; j < 4; j++)
                    {
                        ListItem li = new ListItem();
                        li.Text = code[j].Trim() + " - " + name[j].Trim();
                        li.Value = code[j].Trim();
                        DDL.Items.Add(li);
                    }
                    //DDL.Items.Add("1- 薪資");
                    //DDL.Items.Add("2- 成本");
                    //DDL.Items.Add("3- 公司負擔");
                    //DDL.Items.Add("5- 公司獎金");
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("P1CostSalaryItem"))
                {//下拉式選單
      
                    DropDownList DDL = new DropDownList();
                    string[] name ={ "上期借支", "上期其它","非上期項目" };
                    string[] code ={ "A", "B","C" };
                    for (int j = 0; j < 3; j++)
                    {
                        ListItem li = new ListItem();
                        li.Text = code[j].Trim() + " - " + name[j].Trim();
                        li.Value = code[j].Trim();
                        DDL.Items.Add(li);
                    }
                    //DDL.Items.Add("A- 上期借支");
                    //DDL.Items.Add("B- 上期其它");
                    //DDL.Items.Add("C- 非上期項目");
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("RegularPay"))
                {//下拉式選單

                    DropDownList DDL = new DropDownList();
                    string[] name ={ "是", "否" };
                    string[] code ={ "Y", "N" };
                    for (int j = 0; j < 2; j++)
                    {
                        ListItem li = new ListItem();
                        li.Text = code[j].Trim() + " - " + name[j].Trim();
                        li.Value = code[j].Trim();
                        DDL.Items.Add(li);
                    }
                    //DDL.Items.Add("Y- 是");
                    //DDL.Items.Add("N- 否");
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                }
                //0- 系統設定 1- 固定項目 2- 變動項目
                //A- 固定金額 B- 薪資比率 C- 變動金額
                //1- 薪資     2- 成本     3- 公司負擔 5- 公司獎金
                //A- 上期借支 B- 上期其它

                if (DetailsView1.Rows[i].Cells[0].Text.Contains("FunctionId"))
                {
                    tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.MaxLength = 10;
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    //為公司欄位增加提示視窗
                    DataTable dt = _MyDBM.ExecuteDataTable("Select * From CodeDesc Where CodeID like 'function' OR CodeID like 'sqlcomm'");
                    DropDownList DDL = new DropDownList();
                    DDL.ID = "DropDownList" + i.ToString();
                    DDL.Style.Add("width", "150px");

                    DDL.Items.Add("不選擇公式");
                    //DDL.Items.Add("C*(A+B)-A");
                    //DDL.Items.Add("C*(A+B)");
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        ListItem li = new ListItem();
                        li.Text = dt.Rows[j]["CodeCode"].ToString() + ":" + dt.Rows[j]["CodeName"].ToString();
                        li.Value = dt.Rows[j]["CodeName"].ToString();
                        DDL.Items.Add(li);
                    }
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("ParameterList"))
                {
                    tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("width", "250px");
                    tb.MaxLength = 500;
                    //為參數欄位增加提示視窗
                    ImageButton btOpenList = new ImageButton();
                    btOpenList.ID = "btOpen" + i.ToString();
                    btOpenList.SkinID = "OpenWin1";
                    //Company,CompanyShortName,CompanyName,ChopNo
                    //btOpenList.OnClientClick = "return GetPromptWin3(" + DetailsView1.Rows[i].Cells[1].Controls[0].ClientID + ",'400','450','CodeMaster','CodeID','CodeDecs','CodeID');";
                    btOpenList.OnClientClick = "return GetPromptWin3(" + DetailsView1.Rows[i].Cells[1].Controls[0].ClientID + ",'400','450','SalaryStructure_Parameter','SalaryId','SalaryId As 薪資代碼','SalaryId');";
                    DetailsView1.Rows[i].Cells[1].Controls.Add(btOpenList);

                }
                DetailsView1.Rows[i].Cells[0].Text = DT.Rows[0][0].ToString().Trim();

            }
        }
    }
    private ListItem Li(string text, string value)
    {
        ListItem li = new ListItem();
        li.Text = text;
        li.Value = value;
        return li;
    }
    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        string Err = "";
        string InsertItem = "", InsertValue = "";
        //e.Values.
        if (e.Values["SalaryId"] == null)
        {
            Err = "薪資代碼不可空白<br>";
        }
      
        
        //if (e.Values["EmployeeId"] == null)
        //{
        //    Err += "員工代號不可空白<br>";
        //}
    //if(e.Values["SalaryId"])
        if (Err.Equals(""))
        {
            if (!ValidateData(e.Values["SalaryId"].ToString()))//, e.Values["EmployeeId"].ToString()
            {
                Err += "代號重覆!!已有此薪資代碼!<br>";
            }
            if (e.Values["SalaryId"].ToString().Substring(0, 1) == "0" || e.Values["SalaryId"].ToString().Substring(0, 1) == "1" )
            {
                if (Convert.ToInt32(e.Values["SalaryId"]) < 20)
                {
                    Err += "此薪資代碼為保留碼!! 請輸入其它薪資代碼";
                }
            }
        }
    

        DetailsView dv = (DetailsView)sender;
        for (int i = 0; i < dv.Rows.Count; i++)
        {
            #region ---下拉式選單---
            //if (e.Values[i] == null)
            //{
                if (i == 2)
                {
                    DropDownList ddl = (DropDownList)dv.Rows[i].Cells[1].Controls[1];
                    e.Values[i] = ddl.SelectedValue.Trim();
                }
                if (i == 3)
                {
                    DropDownList ddl = (DropDownList)dv.Rows[i].Cells[1].Controls[1];
                    e.Values[i] = ddl.SelectedValue.Trim();
                }
                if (i == 4)
                {
                    DropDownList ddl = (DropDownList)dv.Rows[i].Cells[1].Controls[1];
                    e.Values[i] = ddl.SelectedValue.Trim();
                }
                if (i == 5)
                {
                    DropDownList ddl = (DropDownList)dv.Rows[i].Cells[1].Controls[1];
                    e.Values[i] = ddl.SelectedValue.Trim();
                }
                if (i == 8)
                {
                    DropDownList ddl = (DropDownList)dv.Rows[i].Cells[1].Controls[1];
                    e.Values[i] = ddl.SelectedValue.Trim();
                }
                if (i == 9)
                {
                    DropDownList ddl = (DropDownList)dv.Rows[i].Cells[1].Controls[1];
                    e.Values[i] = ddl.SelectedValue.Trim();
                }
                if (i == 15)
                {
                    DropDownList ddl = (DropDownList)dv.Rows[i].Cells[1].Controls[1];
                    if (ddl.SelectedValue.Trim() == "C")
                    {
                        if (e.Values[i - 2].ToString() != "0" && e.Values[i - 1].ToString() != "0")
                        {
                            Err += "選擇非上期項目，上期總表與上期薪資單的值必須為零<br>";
                        }
                        else if (e.Values[i - 2].ToString() == "0" && e.Values[i - 1].ToString() != "0")
                        {
                            Err += "選擇非上期項目，上期薪資單的值必須為零<br>";
                        }
                        else if (e.Values[i - 2].ToString() != "0" && e.Values[i - 1].ToString() == "0")
                        {
                            Err += "選擇非上期項目，上期總表的值必須為零<br>";
                        }
                        else
                        {
                            e.Values[i] = "C";
                        }
                    }
                    else
                    {
                        e.Values[i] = ddl.SelectedValue.Trim();
                    }
                }
                if (i == 16)
                {//下拉式選單

                    DropDownList ddl = (DropDownList)dv.Rows[i].Cells[1].Controls[1];
                    e.Values[i] = ddl.SelectedValue.Trim();
                }
                if (i == 17)
                {
                    DropDownList ddl = (DropDownList)dv.Rows[i].Cells[1].Controls[1];
                    e.Values[i] = ddl.SelectedValue.Trim();
                }
                if (i == 6 || i == 10 || i == 11 || i == 13 || i == 14)
                {
                    if (e.Values[i] == null)
                    {//將數字空欄位放入0
                        e.Values[i] = "0";
                    }
                }
                if (i == 1 || i == 8 || i == 12)
                {
                    if (e.Values[i] == null)
                    {//將數字空欄位放入0
                        e.Values[i] = " ";
                    }
                }

            //}
            #endregion
            //薪資比率
            if (dv.Rows[i].Cells[0].Text.Contains("SalaryRate") || dv.Rows[i].Cells[0].Text.Contains("薪資比率"))
            {
                if (e.Values[i].ToString().IndexOf(".") != 1)
                {
                    Err += "薪資比率!!格式錯誤<br>";
                }
                else if (e.Values[i].ToString() == "0" || e.Values[i].ToString() == " ")
                {
                    e.Values[i] = "0.00";
                }
            }
            if (dv.Rows[i].Cells[0].Text.Contains("ParameterList") || dv.Rows[i].Cells[0].Text.Contains("參數"))
            {
                #region ---參數位數判斷---
                DropDownList ddl = (DropDownList)dv.Rows[i - 1].Cells[1].Controls[1];
                string Fomula = ddl.SelectedItem.Text;
                string strChar = "";
                if (e.Values[i - 1].ToString() == "不選擇公式")
                {
                    if (e.Values[i] != null)
                    {
                        Err += "要輸入或選擇參數，請先選擇公式<br>";
                    }
                }
                else
                {
                    for (int j = 0; j < Fomula.Length; j++)
                    {
                        switch (Fomula[j].ToString())
                        {
                            case "(":
                            case ")":
                            case "+":
                            case "-":
                            case "*":
                            case "/":
                                break;
                            default:
                                if (strChar.IndexOf(Fomula[j].ToString()) == -1)
                                {
                                    strChar += Fomula[j].ToString();
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                        }
                    }
                    
                    //檢查輸入參數個數是否等於公式參數
                    if (e.Values[i] != null)
                    {
                        #region ---參數個數與公式所需參數判斷---
                        string t = e.Values[i].ToString().Substring(e.Values[i].ToString().Trim().Length - 1, 1); //e.Values[i].ToString().Substring((e.Values[i].ToString().Trim().Length - 6), (e.Values[i].ToString().Trim().Length - 1));
                        if (t == ",")
                        {
                            string temp = e.Values[i].ToString().Substring(0, e.Values[i].ToString().Trim().Length - 1);
                            string[] para = temp.ToString().Split(new char[] { ',' });
                            if (para.Length > strChar.Length)
                            {
                                Err += "選擇或輸入的參數過多<br>";
                            }
                            else if (para.Length < strChar.Length)
                            {
                                e.Values[i] = e.Values[i].ToString().Substring(0, e.Values[i].ToString().Trim().Length);
                                for (int k = 0; k < (strChar.Length - para.Length); k++)
                                {
                                    e.Values[i] += "0,";
                                }
                                e.Values[i] = e.Values[i].ToString().Substring(0, e.Values[i].ToString().Trim().Length - 1);
                            }
                            else
                            {
                                e.Values[i] = e.Values[i].ToString().Substring(0, e.Values[i].ToString().Trim().Length - 1);
                            }
                        }
                        else
                        {
                            string temp = e.Values[i].ToString().Substring(0, e.Values[i].ToString().Trim().Length);
                            string[] para = temp.ToString().Split(new char[] { ',' });
                            if (para.Length > strChar.Length)
                            {
                                Err += "選擇或輸入的參數過多<br>";
                            }
                            else if (para.Length < strChar.Length)
                            {
                                e.Values[i] = e.Values[i].ToString().Substring(0, e.Values[i].ToString().Trim().Length);
                                for (int k = 0; k < (strChar.Length - para.Length); k++)
                                {
                                    e.Values[i] += ",0";
                                }
                                e.Values[i] = e.Values[i].ToString().Substring(0, e.Values[i].ToString().Trim().Length);
                            }
                            else
                            {
                                e.Values[i] = e.Values[i].ToString().Substring(0, e.Values[i].ToString().Trim().Length);
                            }
                        }
                        #endregion
                    }

                    else
                    {
                        Err += "請輸入參數";
                    }
                }
                #endregion
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
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "SalaryStructure_Parameter";
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

        //hid_Company.Value = txt_Company.Text;   
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
            str.Append("window.opener.location='SalaryStructureParameter.aspx';");
            str.Append("window.close();");
            str.Append("</script>");
            Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", str.ToString());
        }
    }

    private bool ValidateData(string SalaryId)
    {
        Ssql = "Select * From SalaryStructure_Parameter Where SalaryId='" + SalaryId+ "'";//  And EmployeeId='" + EmployeeId + "'
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
    protected void btnSave_Click(object sender, ImageClickEventArgs e)
    {
        DetailsView1.InsertItem(true);
    }
}
