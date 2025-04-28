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
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYB007";
    int saveon = 0;
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
                blCheckProgramAuth = _UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, "Modify");
                if (blCheckProgramAuth == false)
                    ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg("NoRight");
            }
        }
        if (!Page.IsPostBack)
        {
            saveon = 0;
        }
        btnCancel.Attributes.Add("onclick", "javascript:window.close();");
    }

    protected void DetailsView1_ItemCreated(object sender, EventArgs e)
    {
        //ImageButton IB;
        //switch (DetailsView1.CurrentMode)
        //{
        //    case DetailsViewMode.Edit:
        //        IB = (ImageButton)DetailsView1.FindControl("btnCancel");
        //        if (IB != null)
        //            IB.Attributes.Add("onclick", "javascript:window.close();");
        //        break;

        //}
        if (Page.IsPostBack && saveon==0)
        {

                for (int i = 0; i < DetailsView1.Rows.Count; i++)
                {//修改欄位顯示名稱
                    Ssql = "Select dbo.GetColumnTitle('SalaryStructure_Parameter','" + DetailsView1.Rows[i].Cells[0].Text + "')";

                    DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
                    #region
                    if (DT.Rows.Count > 0)
                    {
                        if (DetailsView1.Rows[i].Cells[0].Text.Contains("NWTax"))
                        {//下拉式選單

                            DropDownList DDL = new DropDownList();
                            TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                            string[] name ={ "應稅", "免稅" };
                            string[] code ={ "Y", "N" };
                            for (int j = 0; j < 2; j++)
                            {
                                ListItem li = new ListItem();
                                li.Text = code[j].Trim() + " - " + name[j].Trim();
                                li.Value = code[j].Trim();
                                DDL.Items.Add(li);
                            }
                            DDL.ID = "ddl01";
                            //DDL.Items.Add("Y- 應稅");
                            //DDL.Items.Add("N- 免稅");
                            DDL.Style.Add("width", "150px");
                            tb.Visible = false;
                            tb.Style.Add("width", "1px");
                            DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                            DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);

                        }
                        if (DetailsView1.Rows[i].Cells[0].Text.Contains("PMType"))
                        {//下拉式選單

                            DropDownList DDL = new DropDownList();
                            TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                            string[] name ={ "加項", "減項" };
                            string[] code ={ "A", "B" };
                            for (int j = 0; j < 2; j++)
                            {
                                ListItem li = new ListItem();
                                li.Text = code[j].Trim() + " - " + name[j].Trim();
                                li.Value = code[j].Trim();
                                DDL.Items.Add(li);
                            }
                            DDL.ID = "ddl02";
                            //DDL.Items.Add("A- 加項");
                            //DDL.Items.Add("B- 減項");
                            DDL.Style.Add("width", "150px");
                            tb.Visible = false;
                            tb.Style.Add("width", "1px");
                            //string tmp = "";
                            //if ( tmp== "A")
                            //{
                            //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 加項";
                            //}
                            //else
                            //{
                            //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 減項";
                            //}
                            DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                            DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                        }
                        if (DetailsView1.Rows[i].Cells[0].Text.Contains("ItemType"))
                        {//下拉式選單

                            DropDownList DDL = new DropDownList();
                            TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                            string[] name ={ "系統設定", "固定項目", "變動項目" };
                            string[] code ={ "0", "1", "2" };
                            for (int j = 0; j < 3; j++)
                            {
                                ListItem li = new ListItem();
                                li.Text = code[j].Trim() + " - " + name[j].Trim();
                                li.Value = code[j].Trim();
                                DDL.Items.Add(li);
                            }
                            DDL.ID = "ddl03";
                            //DDL.Items.Add("0- 系統設定");
                            //DDL.Items.Add("1- 固定項目");
                            //DDL.Items.Add("2- 變動項目");
                            DDL.Style.Add("width", "150px");
                            tb.Visible = false;
                            tb.Style.Add("width", "1px");
                            //string tmp = "";
                            //if ( == "0")
                            //{
                            //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 系統設定";
                            //}
                            //else if (((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text == "1")
                            //{
                            //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 固定項目";
                            //}
                            //else
                            //{
                            //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 變動項目";
                            //}
                            DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                            DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                        }
                        if (DetailsView1.Rows[i].Cells[0].Text.Contains("SalaryType"))
                        {//下拉式選單

                            DropDownList DDL = new DropDownList();
                            TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                            string[] name ={ "固定金額", "薪資比率", "變動金額" };
                            string[] code ={ "A", "B", "C" };
                            for (int j = 0; j < 3; j++)
                            {
                                ListItem li = new ListItem();
                                li.Text = code[j].Trim() + " - " + name[j].Trim();
                                li.Value = code[j].Trim();
                                DDL.Items.Add(li);
                            }
                            DDL.ID = "ddl04";
                            //DDL.Items.Add("A- 固定金額");
                            //DDL.Items.Add("B- 薪資比率");
                            //DDL.Items.Add("C- 變動金額");
                            DDL.Style.Add("width", "150px");
                            tb.Visible = false;
                            tb.Style.Add("width", "1px");
                            //string tmp = "";
                            //if ( == "A")
                            //{
                            //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 固定金額";
                            //}
                            //else if (((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text == "B")
                            //{
                            //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 薪資比率";
                            //}
                            //else
                            //{
                            //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 變動金額";
                            //}
                            DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                            DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                        }
                        if (DetailsView1.Rows[i].Cells[0].Text.Contains("Properties"))
                        {//下拉式選單

                            DropDownList DDL = new DropDownList();
                            TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                            string[] name ={ "薪資", "成本", "公司負擔", "公司獎金" };
                            string[] code ={ "1", "2", "3", "5" };
                            for (int j = 0; j < 4; j++)
                            {
                                ListItem li = new ListItem();
                                li.Text = code[j].Trim() + " - " + name[j].Trim();
                                li.Value = code[j].Trim();
                                DDL.Items.Add(li);
                            }
                            DDL.ID = "ddl05";
                            //DDL.Items.Add("1- 薪資");
                            //DDL.Items.Add("2- 成本");
                            //DDL.Items.Add("3- 公司負擔");
                            //DDL.Items.Add("5- 公司獎金");
                            DDL.Style.Add("width", "150px");
                            tb.Visible = false;
                            tb.Style.Add("width", "1px");
                            //string tmp = "";
                            //if (((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text == "1")
                            //{
                            //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 薪資";
                            //}
                            //else if (((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text == "2")
                            //{
                            //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 成本";
                            //}
                            //else if (((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text == "3")
                            //{
                            //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 公司負擔";
                            //}
                            //else
                            //{
                            //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 公司獎金";
                            //}
                            DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                            DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                        }
                        if (DetailsView1.Rows[i].Cells[0].Text.Contains("P1CostSalaryItem"))
                        {//下拉式選單

                            DropDownList DDL = new DropDownList();
                            TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                            string[] name ={ "上期借支", "上期其它", "非上期項目" };
                            string[] code ={ "A", "B", "C" };
                            for (int j = 0; j < 3; j++)
                            {
                                ListItem li = new ListItem();
                                li.Text = code[j].Trim() + " - " + name[j].Trim();
                                li.Value = code[j].Trim();
                                DDL.Items.Add(li);
                            }
                            DDL.ID = "ddl06";
                            //DDL.Items.Add("A- 上期借支");
                            //DDL.Items.Add("B- 上期其它");
                            //DDL.Items.Add("C- 非上期項目");
                            DDL.Style.Add("width", "150px");
                            tb.Visible = false;
                            tb.Style.Add("width", "1px");
                            //string tmp = "";
                            //if (((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text == "A")
                            //{
                            //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 上期借支";
                            //}
                            //else if (((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text == "B")
                            //{
                            //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 上期其它";
                            //}
                            //else
                            //{
                            //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 非上期項目";
                            //}

                            DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                            DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                        }
                        if (DetailsView1.Rows[i].Cells[0].Text.Contains("RegularPay"))
                        {//下拉式選單

                            DropDownList DDL = new DropDownList();
                            TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                            string[] name ={ "是", "否" };
                            string[] code ={ "Y", "N" };
                            for (int j = 0; j < 2; j++)
                            {
                                ListItem li = new ListItem();
                                li.Text = code[j].Trim() + " - " + name[j].Trim();
                                li.Value = code[j].Trim();
                                DDL.Items.Add(li);
                            }
                            DDL.ID = "ddl07";
                            //DDL.Items.Add("Y- 是");
                            //DDL.Items.Add("N- 否");
                            DDL.Style.Add("width", "150px");
                            tb.Visible = false;
                            tb.Style.Add("width", "1px");
                            //string tmp = "";
                            //if (((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text == "Y")
                            //{
                            //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 是";
                            //}
                            //else
                            //{
                            //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 否";
                            //}
                            DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                            DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                        }
                        if (DetailsView1.Rows[i].Cells[0].Text.Contains("FunctionId"))
                        {
                            TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                            tb.MaxLength = 10;
                            tb.Visible = false;
                            tb.Style.Add("width", "1px");
                            //為公司欄位增加提示視窗
                            ////DataTable dt = _MyDBM.ExecuteDataTable("Select CodeID From CodeMaster Where CodeID='Function' OR CodeID='SqlComm' OR CodeID='function' OR CodeID='sqlcomm' ");
                            DropDownList DDL = new DropDownList();
                            DDL.ID = "ddl08";
                            DDL.Style.Add("width", "150px");
                            //for (int j = 0; j < dt.Rows.Count; j++)
                            //{
                            //    DDL.Items.Add(dt.Rows[j]["CodeID"].ToString());
                            //}
                            DataTable dt = _MyDBM.ExecuteDataTable("Select * From CodeDesc Where CodeID like 'function' OR CodeID like 'sqlcomm'");


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
                            DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                            DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                        }
                        if (DetailsView1.Rows[i].Cells[0].Text.Contains("ParameterList"))
                        {
                            TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                            tb.Style.Add("width", "250px");
                            tb.MaxLength = 500;
                            //為公司欄位增加提示視窗
                            ImageButton btOpenList = new ImageButton();
                            btOpenList.ID = "btOpen" + i.ToString();
                            btOpenList.SkinID = "OpenWin1";
                            //Company,CompanyShortName,CompanyName,ChopNo
                            btOpenList.OnClientClick = "return GetPromptWin3(" + DetailsView1.Rows[i].Cells[1].Controls[0].ClientID + ",'400','450','SalaryStructure_Parameter','SalaryId','SalaryId As 薪資代碼','SalaryId');";
                            //btOpenList.OnClientClick = "return GetPromptWin3(" + DetailsView1.Rows[i].Cells[1].Controls[0].ClientID + ",'400','450','SalaryStructure_Parameter','SalaryId','BaseItem','SalaryId');";
                            DetailsView1.Rows[i].Cells[1].Controls.Add(btOpenList);
                        }
                        if (DetailsView1.Rows[i].Cells[0].Text.Contains("BaseItem"))
                        {//數字欄位需靠右對齊
                            DropDownList DDL = new DropDownList();
                            TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
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
                            DDL.ID = "ddl09";
                            //DDL.Items.Add("Y- 應稅");
                            //DDL.Items.Add("N- 免稅");
                            DDL.Style.Add("width", "150px");
                            tb.Visible = false;
                            tb.Style.Add("width", "1px");
                            DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                            DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                        }
                    }
                    #endregion
                }
            
        }
    }

    protected void DetailsView1_DataBound(object sender, EventArgs e)
    {
        for (int i = 0; i < DetailsView1.Rows.Count; i++)
        {//修改欄位顯示名稱
            Ssql = "Select dbo.GetColumnTitle('SalaryStructure_Parameter','" + DetailsView1.Rows[i].Cells[0].Text + "')";

            DataTable DT = _MyDBM.ExecuteDataTable(Ssql);

            if (DT.Rows.Count > 0)
            {
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("SalaryName"))
                {//數字欄位需靠右對齊
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("width", "145px");
                    tb.MaxLength = 14;
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("FixedAmount"))
                {//數字欄位需靠右對齊
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("text-align", "right");
                    tb.Style.Add("width", "145px");
                    tb.MaxLength = 7;
                    //預設為0
                    
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("SalaryRate"))
                {//數字欄位需靠右對齊
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("text-align", "right");
                    tb.Style.Add("width", "145px");
                    tb.MaxLength = 4;
                    //預設為0
                   
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("BaseItem"))
                {//數字欄位需靠右對齊
                    DropDownList DDL = new DropDownList();
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
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
                    DDL.ID = "ddl09";
                    //DDL.Items.Add("Y- 應稅");
                    //DDL.Items.Add("N- 免稅");
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("P1SalaryMasterList"))
                {//數字欄位需靠右對齊
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("width", "145px");
                    tb.Style.Add("text-align", "right");
                    tb.MaxLength = 2;
                    //預設為0
                 
                }
                else if (DetailsView1.Rows[i].Cells[0].Text.Contains("SalaryMasterList"))
                {//數字欄位需靠右對齊
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("width", "145px");
                    tb.Style.Add("text-align", "right");
                    tb.MaxLength = 2;
                    //預設為0
                
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("P1Payroll"))
                {//數字欄位需靠右對齊
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("width", "145px");
                    tb.Style.Add("text-align", "right");
                    tb.MaxLength = 2;
                    //預設為0
                 
                }
                else if (DetailsView1.Rows[i].Cells[0].Text.Contains("Payroll"))
                {//數字欄位需靠右對齊
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("width", "145px");
                    tb.Style.Add("text-align", "right");
                    tb.MaxLength = 2;
                    //預設為0
                 
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("AcctNo"))
                {//數字欄位需靠右對齊
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("width", "145px");
                    tb.MaxLength = 8;
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("NWTax"))
                {//下拉式選單

                    DropDownList DDL = new DropDownList();
                    TextBox tb=(TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    string[] name ={ "應稅", "免稅" };
                    string[] code ={ "Y", "N" };
                    for (int j = 0; j < 2; j++)
                    {
                        ListItem li = new ListItem();
                        li.Text = code[j].Trim() + " - " + name[j].Trim();
                        li.Value = code[j].Trim();
                        DDL.Items.Add(li);
                    }
                    DDL.ID = "ddl01";
                    //DDL.Items.Add("Y- 應稅");
                    //DDL.Items.Add("N- 免稅");
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);

                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("PMType"))
                {//下拉式選單

                    DropDownList DDL = new DropDownList();
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    string[] name ={ "加項", "減項" };
                    string[] code ={ "A", "B" };
                    for (int j = 0; j < 2; j++)
                    {
                        ListItem li = new ListItem();
                        li.Text = code[j].Trim() + " - " + name[j].Trim();
                        li.Value = code[j].Trim();
                        DDL.Items.Add(li);
                    }
                    DDL.ID = "ddl02";
                    //DDL.Items.Add("A- 加項");
                    //DDL.Items.Add("B- 減項");
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    //string tmp = "";
                    //if ( tmp== "A")
                    //{
                    //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 加項";
                    //}
                    //else
                    //{
                    //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 減項";
                    //}
                    DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("ItemType"))
                {//下拉式選單

                    DropDownList DDL = new DropDownList();
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    string[] name ={ "系統設定", "固定項目", "變動項目" };
                    string[] code ={ "0", "1", "2" };
                    for (int j = 0; j < 3; j++)
                    {
                        ListItem li = new ListItem();
                        li.Text = code[j].Trim() + " - " + name[j].Trim();
                        li.Value = code[j].Trim();
                        DDL.Items.Add(li);
                    }
                    DDL.ID = "ddl03";
                    //DDL.Items.Add("0- 系統設定");
                    //DDL.Items.Add("1- 固定項目");
                    //DDL.Items.Add("2- 變動項目");
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    //string tmp = "";
                    //if ( == "0")
                    //{
                    //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 系統設定";
                    //}
                    //else if (((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text == "1")
                    //{
                    //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 固定項目";
                    //}
                    //else
                    //{
                    //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 變動項目";
                    //}
                    DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("SalaryType"))
                {//下拉式選單

                    DropDownList DDL = new DropDownList();
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    string[] name ={ "固定金額", "薪資比率", "變動金額" };
                    string[] code ={ "A", "B", "C" };
                    for (int j = 0; j < 3; j++)
                    {
                        ListItem li = new ListItem();
                        li.Text = code[j].Trim() + " - " + name[j].Trim();
                        li.Value = code[j].Trim();
                        DDL.Items.Add(li);
                    }
                    DDL.ID = "ddl04";
                    //DDL.Items.Add("A- 固定金額");
                    //DDL.Items.Add("B- 薪資比率");
                    //DDL.Items.Add("C- 變動金額");
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    //string tmp = "";
                    //if ( == "A")
                    //{
                    //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 固定金額";
                    //}
                    //else if (((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text == "B")
                    //{
                    //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 薪資比率";
                    //}
                    //else
                    //{
                    //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 變動金額";
                    //}
                    DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("Properties"))
                {//下拉式選單

                    DropDownList DDL = new DropDownList();
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    string[] name ={ "薪資", "成本", "公司負擔", "公司獎金" };
                    string[] code ={ "1", "2", "3", "5" };
                    for (int j = 0; j < 4; j++)
                    {
                        ListItem li = new ListItem();
                        li.Text = code[j].Trim() + " - " + name[j].Trim();
                        li.Value = code[j].Trim();
                        DDL.Items.Add(li);
                    }
                    DDL.ID = "ddl05";
                    //DDL.Items.Add("1- 薪資");
                    //DDL.Items.Add("2- 成本");
                    //DDL.Items.Add("3- 公司負擔");
                    //DDL.Items.Add("5- 公司獎金");
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    //string tmp = "";
                    //if (((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text == "1")
                    //{
                    //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 薪資";
                    //}
                    //else if (((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text == "2")
                    //{
                    //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 成本";
                    //}
                    //else if (((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text == "3")
                    //{
                    //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 公司負擔";
                    //}
                    //else
                    //{
                    //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 公司獎金";
                    //}
                    DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("P1CostSalaryItem"))
                {//下拉式選單

                    DropDownList DDL = new DropDownList();
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    string[] name ={ "上期借支", "上期其它", "非上期項目" };
                    string[] code ={ "A", "B", "C" };
                    for (int j = 0; j < 3; j++)
                    {
                        ListItem li = new ListItem();
                        li.Text = code[j].Trim() + " - " + name[j].Trim();
                        li.Value = code[j].Trim();
                        DDL.Items.Add(li);
                    }
                    DDL.ID = "ddl06";
                    //DDL.Items.Add("A- 上期借支");
                    //DDL.Items.Add("B- 上期其它");
                    //DDL.Items.Add("C- 非上期項目");
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    //string tmp = "";
                    //if (((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text == "A")
                    //{
                    //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 上期借支";
                    //}
                    //else if (((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text == "B")
                    //{
                    //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 上期其它";
                    //}
                    //else
                    //{
                    //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 非上期項目";
                    //}

                    DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("RegularPay"))
                {//下拉式選單

                    DropDownList DDL = new DropDownList();
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    string[] name ={ "是", "否" };
                    string[] code ={ "Y", "N" };
                    for (int j = 0; j < 2; j++)
                    {
                        ListItem li = new ListItem();
                        li.Text = code[j].Trim() + " - " + name[j].Trim();
                        li.Value = code[j].Trim();
                        DDL.Items.Add(li);
                    }
                    DDL.ID = "ddl07";
                    //DDL.Items.Add("Y- 是");
                    //DDL.Items.Add("N- 否");
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    //string tmp = "";
                    //if (((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text == "Y")
                    //{
                    //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 是";
                    //}
                    //else
                    //{
                    //    tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- 否";
                    //}
                    DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("FunctionId"))
                {
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.MaxLength = 10;
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    //為公司欄位增加提示視窗
                    ////DataTable dt = _MyDBM.ExecuteDataTable("Select CodeID From CodeMaster Where CodeID='Function' OR CodeID='SqlComm' OR CodeID='function' OR CodeID='sqlcomm' ");
                    DropDownList DDL = new DropDownList();
                    DDL.ID = "ddl08";
                    DDL.Style.Add("width", "150px");
                    //for (int j = 0; j < dt.Rows.Count; j++)
                    //{
                    //    DDL.Items.Add(dt.Rows[j]["CodeID"].ToString());
                    //}
                    DataTable dt = _MyDBM.ExecuteDataTable("Select * From CodeDesc Where CodeID like 'function' OR CodeID like 'sqlcomm'");


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
                    DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("ParameterList"))
                {
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("width", "250px");
                    tb.MaxLength = 500;
                    //為公司欄位增加提示視窗
                    ImageButton btOpenList = new ImageButton();
                    btOpenList.ID = "btOpen" + i.ToString();
                    btOpenList.SkinID = "OpenWin1";
                    //Company,CompanyShortName,CompanyName,ChopNo
                    btOpenList.OnClientClick = "return GetPromptWin3(" + DetailsView1.Rows[i].Cells[1].Controls[0].ClientID + ",'400','450','SalaryStructure_Parameter','SalaryId','SalaryId As 薪資代碼','SalaryId');";
                    //btOpenList.OnClientClick = "return GetPromptWin3(" + DetailsView1.Rows[i].Cells[1].Controls[0].ClientID + ",'400','450','SalaryStructure_Parameter','SalaryId','BaseItem','SalaryId');";
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
    protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        string Err = "", tempOldValue = "";
        string UpdateItem = "", UpdateValue = "";
        string ddlId = "DetailsView1$ddl0";
        UpdateValue = "(Key=";
        //將此筆資料的KEY值找出
        for (int i = 0; i < e.Keys.Count; i++)
        {
            UpdateValue += e.Keys[i].ToString().Trim() + "|";
        }
        UpdateValue += ")";

        DetailsView dv = ((DetailsView)sender);
        //因為前1個是ReadOnly所以要扣掉,最後一個是按鈕列也要扣掉
        for (int i = 0; i < dv.Rows.Count - 2; i++)
        {
            try
            {
                if (e.OldValues[i] == null)
                {
                    tempOldValue = "";
                }
                else
                {
                    tempOldValue = e.OldValues[i].ToString().Trim();
                }
                if (e.NewValues[i] == null)
                {
                    e.NewValues[i] = "";
                }
                else
                {
                    e.NewValues[i] = e.NewValues[i].ToString().Trim();
                }
                //薪資比率位數判斷
                if (dv.Rows[i].Cells[0].Text.Contains("SalaryRate") || dv.Rows[i].Cells[0].Text.Contains("薪資比率"))
                {
                    if (e.NewValues[i-1].ToString().IndexOf(".") != 1)
                    {
                        Err += "薪資比率!!格式錯誤<br>";
                    }
                    else if (e.NewValues[i-1].ToString() == "0" || e.NewValues[i].ToString() == " ")
                    {
                        e.NewValues[i-1] = "0.00";
                    }
                }
                if (dv.Rows[i + 2].Cells[0].Text.Trim().Contains("ParameterList") || dv.Rows[i + 2].Cells[0].Text.Trim().Contains("參數"))
                {

                    if (Request.Form[ddlId + "8"].ToString() == "不選擇公式")
                    {
                        if (e.NewValues[i + 1] != null)
                        {
                            Err += "要輸入或選擇參數，請先選擇公式<br>";
                        }
                    }
                    else
                    {
                        string Fomula = Request.Form[ddlId + "8"].ToString();
                        string strChar = "";
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
                        if (e.NewValues[i + 1] != null)
                        {
                            #region ---判斷參數位數---
                            string t = e.NewValues[i + 1].ToString().Substring(e.NewValues[i + 1].ToString().Trim().Length - 1, 1); //e.Values[i].ToString().Substring((e.Values[i].ToString().Trim().Length - 6), (e.Values[i].ToString().Trim().Length - 1));
                            if (t == ",")
                            {
                                string temp = e.NewValues[i + 1].ToString().Substring(0, e.NewValues[i + 1].ToString().Trim().Length - 1);
                                string[] para = temp.ToString().Split(new char[] { ',' });
                                if (para.Length > strChar.Length)
                                {
                                    Err += "選擇或輸入的參數過多<br>";
                                }
                                else if (para.Length < strChar.Length)
                                {
                                    e.NewValues[i + 1] = e.NewValues[i + 1].ToString().Substring(0, e.NewValues[i + 1].ToString().Trim().Length);
                                    for (int k = 0; k < (strChar.Length - para.Length); k++)
                                    {
                                        e.NewValues[i + 1] += "0,";
                                    }
                                    e.NewValues[i + 1] = e.NewValues[i + 1].ToString().Substring(0, e.NewValues[i + 1].ToString().Trim().Length - 1);
                                }
                                else
                                {
                                    e.NewValues[i + 1] = e.NewValues[i + 1].ToString().Substring(0, e.NewValues[i + 1].ToString().Trim().Length - 1);
                                }
                            }
                            else
                            {
                                string temp = e.NewValues[i + 1].ToString().Substring(0, e.NewValues[i + 1].ToString().Trim().Length);
                                string[] para = temp.ToString().Split(new char[] { ',' });
                                if (para.Length > strChar.Length)
                                {
                                    Err += "選擇或輸入的參數過多<br>";
                                }
                                else if (para.Length < strChar.Length)
                                {
                                    e.NewValues[i + 1] = e.NewValues[i + 1].ToString().Substring(0, e.NewValues[i + 1].ToString().Trim().Length);
                                    for (int k = 0; k < (strChar.Length - para.Length); k++)
                                    {
                                        e.NewValues[i + 1] += ",0";
                                    }
                                    e.NewValues[i + 1] = e.NewValues[i + 1].ToString().Substring(0, e.NewValues[i + 1].ToString().Trim().Length);
                                }
                                else
                                {
                                    e.NewValues[i + 1] = e.NewValues[i + 1].ToString().Substring(0, e.NewValues[i + 1].ToString().Trim().Length);
                                }
                            }
                            #endregion

                        }
                        else
                        {
                            Err += "請輸入參數<br>";
                        }
                    }
                }
                if (i == 1)
                {
                    e.NewValues[i] = Request.Form[ddlId + "1"].ToString();
                }
                if (i == 2)
                {
                    e.NewValues[i] = Request.Form[ddlId + "2"].ToString();
                }
                if (i == 3)
                {
                    e.NewValues[i] = Request.Form[ddlId + "3"].ToString();
                }
                if (i == 4)
                {
                    e.NewValues[i] = Request.Form[ddlId + "4"].ToString();
                }
                if (i == 7)
                {
                    e.NewValues[i] = Request.Form[ddlId + "9"].ToString();
                }
                if (i == 8)
                {
                    e.NewValues[i] = Request.Form[ddlId + "5"].ToString();
                }
                if (i == 14)
                {
                    if (Request.Form[ddlId + "6"].ToString() == "C")
                    {
                        if (e.NewValues[i - 2].ToString() != "0" && e.NewValues[i - 1].ToString() != "0")
                        {
                            Err += "選擇非上期項目，上期總表與上期薪資單的值必須為零<br>";
                        }
                        else if (e.NewValues[i - 2].ToString() == "0" && e.NewValues[i - 1].ToString() != "0")
                        {
                            Err += "選擇非上期項目，上期薪資單的值必須為零<br>";
                        }
                        else if (e.NewValues[i - 2].ToString() != "0" && e.NewValues[i - 1].ToString() == "0")
                        {
                            Err += "選擇非上期項目，上期總表的值必須為零<br>";
                        }
                        else
                        {
                            e.NewValues[i] = Request.Form[ddlId + "6"].ToString();
                        }

                    }
                    else
                    {
                        e.NewValues[i] = Request.Form[ddlId + "6"].ToString();
                    }
                }
                if (i == 15)
                {
                    e.NewValues[i] = Request.Form[ddlId + "7"].ToString();
                }
                if (i == 16)
                {
                    e.NewValues[i] = Request.Form[ddlId + "8"].ToString();
                }
                if (i == 5 || i == 9 || i == 10 || i == 12 || i == 13)
                {
                    if (string.IsNullOrEmpty(e.NewValues[i].ToString()))
                    {//將數字空欄位放入0
                        e.NewValues[i] = "0";
                    }
                }
                if (i == 6)
                {
                    if (string.IsNullOrEmpty(e.NewValues[i].ToString()))
                    {//將空欄位放入半形空格
                        e.NewValues[i] = "0.000";
                    }
                }
                if (string.IsNullOrEmpty(e.NewValues[i].ToString()))
                {//將空欄位放入半形空格
                    e.NewValues[i] = " ";
                }


                if (e.NewValues[i].ToString().Trim() != tempOldValue)
                {
                    try
                    {
                        UpdateItem += dv.Rows[i + 2].Cells[0].Text.Trim() + "|";
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
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "SalaryStructure_Parameter";
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
        saveon = 1;
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
        str.Append("window.opener.location='SalaryStructureParameter.aspx';");
        str.Append("</script>");
        Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", str.ToString());
    }

    protected void btnSave_Click(object sender, ImageClickEventArgs e)
    {
        DetailsView1.UpdateItem(true);
    }
}
