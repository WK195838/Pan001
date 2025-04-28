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

public partial class CostCenter_M : System.Web.UI.Page
{
    string Ssql = "";
    string Ssql1 = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM020";
    DBManger _MyDBM;   
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    string sCompany=""; 
    string sEmployeeId="";
    Payroll py = new Payroll();
    public  string vCompany = "";
    public  string vDeptId = "";
    public  string vDeptName = "";
    public  string vEmployeeId = "";
    public  string SsCompany = "";
    public  string Lmesg = "";
    public  int Total_R = 0;

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
    }

    private void AuthRight(GridView thisGridView)
    {
        //驗證權限
        bool Find = false;
        bool SetCss = false;
        int i = 0;

        string[] Auth = { "Delete", "Modify", "Add", "Detail" };

        if (ConfigurationManager.AppSettings["EnableProgramAuth"] != null && ConfigurationManager.AppSettings["EnableProgramAuth"].ToString() == "true")
        {
            for (i = 0; i < Auth.Length; i++)
            {
                Find = _UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, Auth[i]);
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
        lbl_Msg3.Text = "";
        Lmesg = "";
        StyleTitle1.ShowBackToPre = false;
        StyleTitle2.ShowBackToPre = false;
        //關閉存檔圖樣 
        btnSaveGo.Visible = false;
        btnSaveExit.Visible = false;

        //20110124 日期選項新作法
        ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", @"JQ();", true);
        ScriptManager.RegisterStartupScript(UpdatePanel2, this.GetType(), "", @"JQ();", true);

        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "1", Page.ResolveUrl("~/Scripts/jquery-1.4.4.min.js").ToString());
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "2", Page.ResolveUrl("~/Scripts/jquery-ui-1.8.7.custom.min.js").ToString());
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "3", Page.ResolveUrl("~/Scripts/ui.datepicker.js").ToString());
        if (_UserInfo.SysSet.GetCalendarSetting().Equals("Y"))
        {//決定是否使用民國年
            Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "4", Page.ResolveUrl("~/Scripts/ui.datepicker.tw.js").ToString());
        }
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString(), Page.ResolveUrl("~/Pages/pagefunction.js").ToString()); 


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
        //判斷新增或修改模式,預設為[false]=[修改]
        bool blInsertMod = false;
        //判斷是否唯讀模式
        bool blReadOnly = false;
               
        try
        {
            if (Request["Company"] != null) 
            {
                sCompany = Request["Company"].Trim();
            }
            if (Request["EmployeeId"] != null)
            {
                sEmployeeId = Request["EmployeeId"].Trim();
            }
            //btnSaveGo.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='GO';");
        }
        catch
        {}
              blInsertMod = true;
                if (Request["Kind"] != null)
                {
                    if (Request["Kind"].ToString() == "Query" || Request["Kind"].ToString() == "M")
                    {
                        blInsertMod = false;                        
                    }
                    if (Request["Kind"].ToString() == "Query")
                    {
                        blReadOnly = true;
                    }
                }

                if (Page.IsPostBack == true)
                {
                    sCompany = Request.Form["CompanyList1$companyList"].ToString();
                    vDeptId = Request.Form[DeptCode.CLClientID().Replace("_","$")].ToString();
                    sEmployeeId = Request.Form[EmpleeoyCode.CLClientID().Replace("_", "$")].ToString();
                }
                else 
                {
                
                }


                if (string.IsNullOrEmpty(sCompany))
                {
                    sCompany = "";
                }
           
                Ssql = string.Format(" Where Company = '{0}'", _UserInfo.SysSet.CleanSQL(sCompany.Trim()));

                if (string.IsNullOrEmpty(sEmployeeId))
                {
                    sEmployeeId = "";
                }
                
                //if(!string.IsNullOrEmpty (vDeptId))
                //{
                //    Ssql += string.Format(" And DeptId = '{0}'", _UserInfo.SysSet.CleanSQL(vDeptId.Trim()));
                //}

                  Ssql += string.Format(" And EmployeeId = '{0}'", _UserInfo.SysSet.CleanSQL(sEmployeeId.Trim()));


                Ssql = "Select (Select Count(*) from [Personnel_Master] " + Ssql + ") PMCount,(Select IsNull(DeptId,'') from [Personnel_Master]" + Ssql + ") DeptId";

                DataTable TB = _MyDBM.ExecuteDataTable(Ssql);
              
                if (TB.Rows[0][0].ToString().Equals("0"))
                {
                    blInsertMod = true; 
                }               
                else
                {                   
                    vDeptId = TB.Rows[0][1].ToString();
                }
               
                CompanyList1.Enabled = blInsertMod;        


            CompanyList1.SelectValue = sCompany;

            //部門
            DeptCode.SetDTList("Department", "DepCode", "DepName", "Company='" + sCompany + "'", 5);

            if (!string.IsNullOrEmpty(vDeptId))
            {
                DeptCode.SelectedCode = vDeptId;
            } 

            DeptCode.Enabled = blInsertMod;

            DeptCode.AutoPostBack = true;

           //員工
            EmpleeoyCode.SetDTList("Personnel_Master", "EmployeeId", "EmployeeName", "Company='" + sCompany + "' and DeptId='" + vDeptId + "'", 5);

            if (!string.IsNullOrEmpty(sEmployeeId))            
            {
                EmpleeoyCode.SelectedCode = sEmployeeId;
            }
            EmpleeoyCode.Enabled = blInsertMod;

            EmpleeoyCode.AutoPostBack = true;
            //btnSaveExit.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='EXIT';");
            btnCancel.Attributes.Add("onclick", "javascript:window.close();");

        if (!Page.IsPostBack)
        {
            //查詢相關資料 
            BindData();
        }
        else
        {
            if (!string.IsNullOrEmpty(hid_updateid.Value))
            {
                hid_IsInsertExit.Value = hid_IsInsertExit.Value.Replace("_", "$");
                if (Request.Form[hid_IsInsertExit.Value + "$ctl02"] != null)
                {
                    GridView1RowUpdate();
                }
            }
            else if (!string.IsNullOrEmpty(hid_IsInsertExit.Value))
            {
                string ddlId = hid_IsInsertExit.Value.Replace("_", "$") + "$tbAddNew0";
                if (Request.Form[ddlId + "3$ddlCodeList"] != null )
                {
                    //新增
                    btnEmptyNew_Click(sender, e);
                    hid_IsInsertExit.Value = "";
                }               
            }

            //查詢相關資料 
            BindData();
        }
        Navigator1.BindGridView = GridView1; 

        #region 查詢顯示控管
        if (blReadOnly)
        {
            //DetailsView1.DefaultMode = DetailsViewMode.ReadOnly;
            btnSaveExit.Visible = false;
            btnSaveGo.Visible = false;
            GridView theGv = (GridView)this.Form.FindControl("GridView1");

            if (theGv != null)
            {
                theGv.ShowFooter = false;
                theGv.Columns[0].Visible = false;
                theGv.Columns[1].Visible = false;
            }

        #endregion

        }
        //計算總分攤比例
        if (!string.IsNullOrEmpty(sCompany) && !string.IsNullOrEmpty(sEmployeeId)) 
        {
            Check_Sum_Total(sCompany, sEmployeeId);
        }
        
    }  

    protected DataTable Check_Data()
    {
        string Sqlstr = "";
        string strWhere = "";

        Sqlstr = " Select Company,DeptId,DeptName,Rtrim(EmployeeId)+'-'+EmployeeName as EmployeeId,EmployeeName ,ApportionId, ApportionName,B_effective as B_effective_2,Rtrim(Balance) as Balance, ";

        Sqlstr += " (SELECT SUM(CAST(Balance AS int)) AS Balance_2 FROM CostCenter WHERE (Company = '" + Request["Company"] + "') AND (EmployeeId = '" + Request["EmployeeId"] + "')) AS Balance_2,";

        Sqlstr += " cast(Cast(Left(Convert(varchar(10),B_effective,111),4)as int)-1911 as varchar)+Right(Convert(varchar(10),B_effective,111),6) as B_effective";

        Sqlstr += " From CostCenter Where 1=1 ";

        if (Request["Company"] != null)
        {
            strWhere += string.Format(" And Company like '{0}%'", Request["Company"]);
        }
        if (Request["EmployeeId"] != null)
        {
            strWhere += string.Format(" And EmployeeId like '{0}%'", Request["EmployeeId"]);
        }
        if (Request["DeptId"] != null)
        {
            strWhere += string.Format(" And DeptId like '{0}%'", Request["DeptId"]);
        }
        Sqlstr += strWhere + " ORDER BY EmployeeId";

        DataTable DFP = _MyDBM.ExecuteDataTable(Sqlstr);
       
        return DFP;
    }   

    protected void btnSave_Click(object sender, ImageClickEventArgs e)
    {
        //switch (DetailsView1.CurrentMode)
        //{
        //    case DetailsViewMode.Insert:
        //        DetailsView1.InsertItem(true);
        //        break;
        //    case DetailsViewMode.Edit:
        //        DetailsView1.UpdateItem(true);
        //        break;
        //}
    }

    #region 查詢與編輯使用 BindData
    private void BindData()
    {//查詢與編輯使用
        
        string strWhere = "";       

        //GridView 資料 
            Ssql1 = " Select Company,Rtrim(DeptId)+'-'+Rtrim(DeptName) as DeptId ,DeptName,Rtrim(EmployeeId)+'-'+EmployeeName as EmployeeId ,Rtrim(ApportionId)+'-'+Rtrim(ApportionName) as ApportionId, ApportionName,B_effective as B_effective_2,Rtrim(Balance) as Balance, ";
            Ssql1 += " cast(Cast(Left(Convert(varchar(10),B_effective,111),4)as int)-1911 as varchar)+Right(Convert(varchar(10),B_effective,111),6) as B_effective";
            Ssql1 += " From CostCenter Where 1=1 ";
            
            if (CompanyList1.SelectValue != null)
                {
                    strWhere += string.Format(" And Company ='{0}'", CompanyList1.SelectValue);
                }
            if (EmpleeoyCode.SelectedCode != null)
                {
                    strWhere += string.Format(" And EmployeeId='{0}'",EmpleeoyCode.SelectedCode);
                }

            if (DeptCode.SelectedCode != null)
                {
                    strWhere += string.Format(" And DeptId='{0}'", DeptCode.SelectedCode);
                }
          

         Ssql1 += strWhere + " ORDER BY EmployeeId";
         SDS_GridView.SelectCommand = Ssql1;
         GridView1.DataBind();
         Navigator1.BindGridView = GridView1;
         Navigator1.DataBind();       
    }
    #endregion

    #region 新增資料
    //<summary 新增資料>
    //新增資料
    //</summary>

    protected void btnEmptyNew_Click(object sender, EventArgs e)
    {
        string temId= "",vSql ="";
        string vNDepName = "", AppName = "", AppId = "", EmployeeName = "";
        DataTable DT4;
         
        int P=0;
        if (!string.IsNullOrEmpty(hid_IsInsertExit.Value))
        {

        temId = hid_IsInsertExit.Value.Replace("_", "$") + "$tbAddNew0";  

        //部門名稱

        vSql = "Select DepName From Department Where 1=1 and Company='" + CompanyList1.SelectValue + "'and DepCode='" + vDeptId + "' and Deptype='1'";

        DT4 = _MyDBM.ExecuteDataTable(vSql);
    
        if (DT4.Rows.Count > 0) 
        {
            vNDepName = DT4.Rows[0][0].ToString();        
        }

        //員工 
        vSql = "Select EmployeeName From Personnel_Master Where 1=1 and Company='" + CompanyList1.SelectValue + "'and EmployeeId='" + sEmployeeId + "' ";

        DT4 = _MyDBM.ExecuteDataTable(vSql);

        if (DT4.Rows.Count > 0)
        {
            EmployeeName = DT4.Rows[0][0].ToString();
        }

        //分攤部門代碼
        if (Request.Form[temId + "3$ddlCodeList"] != null)
        {
            AppId = Request.Form[temId + "3$ddlCodeList"].ToString();
        }
        else 
        {
            AppId = Request.Form[temId + "2$ddlCodeList"].ToString();
        }

        //分攤部門名稱

        vSql = "";

        vSql = "Select DepCode+'-'+DepName as DepCoName From Department Where 1=1 and Company='" + CompanyList1.SelectValue + "'and DepCode='" + AppId + "' and Deptype='3'";

        DT4 = _MyDBM.ExecuteDataTable(vSql);

        if (DT4.Rows.Count > 0)
        {
           AppName = DT4.Rows[0][0].ToString();
        }

        //分攤比例
        string Bal = Request.Form[temId + "4"].ToString();

        //生效日期
        string B_Date = _UserInfo.SysSet.FormatADDate(Request.Form[temId + "5"].ToString());

        //2011/02/17 Kevenyan 依據儀測 User 需求新增功能!(EmpleeoyCode=true and DeptCode=true)or(EmpleeoyCode=true and DeptCode=false) 不一定選(true:不選 false:有選)
       
        if ((string.IsNullOrEmpty(DeptCode.SelectedCode) && string.IsNullOrEmpty(EmpleeoyCode.SelectedCode)) || (!string.IsNullOrEmpty(DeptCode.SelectedCode) && string.IsNullOrEmpty(EmpleeoyCode.SelectedCode)))
        {
            #region 新增全公司或全部門資料
            //找尋全公司共有多少人
            string ssql = "Select A.Company,A.DeptId,B.DepName,A.EmployeeId,A.EmployeeName  ";
            ssql += " From Personnel_Master A ";
            ssql += " inner join Department B ON A.Company=B.Company and A.DeptId=B.DepCode ";
            ssql += " Where A.Company='" + CompanyList1.SelectValue + "' ";

            if (!string.IsNullOrEmpty(DeptCode.SelectedCode))
            {
                ssql += " And A.DeptId='" + DeptCode.SelectedCode + "'";
            }

            if (!string.IsNullOrEmpty(EmpleeoyCode.SelectedCode))
            {
                ssql += " And A.EmployeeId='" + EmpleeoyCode.SelectedCode + "'";
            }
            DataTable DRP = _MyDBM.ExecuteDataTable(ssql);

           //進行新增或更新處理 
            try
            {
                if (DRP.Rows.Count > 0)
                {                   
                  Insert_AllNew(CompanyList1.SelectValue, DeptCode.SelectedCode, EmpleeoyCode.SelectedCode, Bal, B_Date, AppId, DRP.Rows.Count, DRP);
                }

                if ((string.IsNullOrEmpty(DeptCode.SelectedCode) && string.IsNullOrEmpty(EmpleeoyCode.SelectedCode)))
                {
                    lbl_Msg3.Text = "新增資料處理成功!! <BR>全公司分攤至：" + AppName.ToString() + " ,分攤比例為：" + Bal + "% <BR>請依需求進行相關微調....! ";
                }
                if ((!string.IsNullOrEmpty(DeptCode.SelectedCode) && string.IsNullOrEmpty(EmpleeoyCode.SelectedCode)))
                {
                    lbl_Msg3.Text = "新增資料處理成功!! <BR>全部門分攤至：" + AppName.ToString() + "  ,分攤比例為：" + Bal + "% <BR>請依需求進行相關微調....! ";
                }
                BindData();
            }
            catch (Exception ex)
            {
                lbl_Msg3.Text = "新增資料處理失敗!!" + ex.ToString();
            }
        }

        #endregion
        else
        {
            #region 新增單筆資料
            //新增單筆資料
          if (hid_IsInsertExit.Value != "")
          {          
           #region 檢查總分攤比例是否超過100%
                // 檢查是否總分攤比例是否超過100%
            if (!ValidateData2(sCompany, sEmployeeId, Bal, "") || (int.TryParse(Bal, out P) == true && Convert.ToInt32(Bal) > 100))
                {
                    //判斷是否為整數
                    int.TryParse(Bal, out P); 
                    lbl_Msg3.Text = "新增失敗!!  原因: 分攤比例已超過 100% , 無法新增!! ";
                }
                #endregion
                else
                {
                    #region 檢查資料是否重覆
                    //檢查資料是否重覆
                    if (!ValidateData(sCompany, sEmployeeId, AppId, vDeptId))
                    {
                        lbl_Msg3.Text = "新增失敗!!  原因: 資料重複,無法新增!! ";
                    }
                    #endregion
                    else
                    {
                        #region 單筆開始新增
                        //新增
                        SDS_GridView.InsertParameters.Clear();
                        SDS_GridView.InsertParameters.Add("Company", sCompany);
                        SDS_GridView.InsertParameters.Add("DeptId", vDeptId);
                        SDS_GridView.InsertParameters.Add("DeptName", vNDepName);
                        SDS_GridView.InsertParameters.Add("EmployeeId", sEmployeeId);
                        SDS_GridView.InsertParameters.Add("EmployeeName", EmployeeName);
                        SDS_GridView.InsertParameters.Add("ApportionId", AppId);
                        SDS_GridView.InsertParameters.Add("ApportionName", AppName.Substring(AppName.IndexOf("-") + 1).Trim());
                        SDS_GridView.InsertParameters.Add("Balance", Bal);
                        SDS_GridView.InsertParameters.Add("B_effective", B_Date);

                        string strLogMsg = "";
                        try
                        {
                            strLogMsg = ":" + AppId + "|" + AppName + "|" + Bal + "|" + B_Date;
                        }
                        catch { }

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
                        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "CostCenter";
                        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "A";
                        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "部門:" + vDeptId;
                        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "員工:" + sEmployeeId + strLogMsg;
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
                            lbl_Msg3.Text = ex.Message;
                        }
                        if (i == 1)
                        {
                            //lbl_Msg3.ForeColor = ConsoleColor.Blue;
                            lbl_Msg3.Text = i.ToString() + " 個資料列 " + "新增成功!! ";
                            //DetailsView1.InsertItem(true);
                            BindData();
                            //計算總分攤比例
                            if (!string.IsNullOrEmpty(sCompany) && !string.IsNullOrEmpty(sEmployeeId))
                            {
                                Check_Sum_Total(sCompany, sEmployeeId);
                            }
                        }
                        else
                        {
                            lbl_Msg3.Text = "新增失敗!!";
                        }

                        #region 完成異動後,更新LOG資訊
                        MyCmd.Parameters["@SQLcommand"].Value = (i == 1) ? "Success" : lbl_Msg2.Text;
                        MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
                        _MyDBM.DataChgLog(MyCmd.Parameters);
                        #endregion
                        #endregion
                    }
                }
          }
            #endregion
        }
      }       
    }

  #endregion

    #region 新增全公司或全部門資料
    protected void Insert_AllNew(string comp,string Dep,string Empoly,string Balance,string Bdate,string AppotID,Int32 Count_Total,DataTable DFP) 
    {
        string ssql;
        string Sqlstr;
        DataTable DT5;

        //依據全公司或全部門多少人進行處理
        for (int j = 0; j < Count_Total; j++)
        {
          #region 檢查 Table_CostCenter 是否有相關資料

            //檢查 Table_CostCenter 是否有相關資料
            ssql = "Select A.Company,A.DeptId,A.DeptName,A.EmployeeId,A.EmployeeName,A.ApportionId,A.ApportionName,A.Balance,Convert(varchar(16),A.B_effective,111) B_effective  ";
            ssql += " From CostCenter A ";
            ssql += " Inner Join Personnel_Master B ON A.Company=B.company and B.DeptId=A.DeptId and B.EmployeeId=A.EmployeeId ";
            ssql += " Where 1=1 ";
            ssql += " And A.Company='" + comp + "' ";

            if (!string.IsNullOrEmpty(Dep))
            {
                ssql += " And A.DeptId='" + Dep + "'";
            }
            else 
            {
                ssql += " And A.DeptId='" + DFP.Rows[j]["DeptId"].ToString().Trim() + "'";
            }

            if (!string.IsNullOrEmpty(Empoly))
            {
                ssql += " And A.EmployeeId='" + Empoly + "'";
            }
            else
            {
                ssql += " And A.EmployeeId='" + DFP.Rows[j]["EmployeeId"].ToString().Trim() + "'";
            }

            ssql += " and A.ApportionId='" + AppotID + "' Order by DeptId";

            DT5 = _MyDBM.ExecuteDataTable(ssql);

            #endregion

          if (DT5.Rows.Count > 0)
          {
              #region 更新資料
              //進行更新資料
            for (int i = 0; i < DT5.Rows.Count; i++)
            {
                string strLogMsg = "";
                try
                {
                    //strLogMsg = ":" + AppId + "|" + AppName + "|" + Bal + "|" + B_Date;
                    strLogMsg = comp + ":" + Dep + "|" + DT5.Rows[i]["EmployeeId"].ToString().Trim() + "|" + Balance + "|" + Bdate + "|" + AppotID;
                }
                catch
                {
                    strLogMsg = "批次新增";
                }

                #region 開始異動前,先寫入LOG
                DateTime StartDateTime = DateTime.Now;
                MyCmd.Parameters.Clear();
                MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "CostCenter_M";
                MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "U";
                MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
                MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = strLogMsg;
                MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
                MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
                _MyDBM.DataChgLog(MyCmd.Parameters);
                #endregion

                Sqlstr = " Update CostCenter  ";
                Sqlstr += " Set  Balance=cast((Select Balance";
                Sqlstr += "                From CostCenter C ";   
                Sqlstr += "               Where C.Company='01' ";
                Sqlstr += "                 and C.DeptId='" + DT5.Rows[i]["DeptId"].ToString().Trim() + "' ";
                Sqlstr += "                 and C.EmployeeId='" + DT5.Rows[i]["EmployeeId"].ToString().Trim() + "' ";
                Sqlstr += "                 and C.ApportionId='" + DT5.Rows[i]["ApportionId"].ToString().Trim() + "' ";
                Sqlstr += "                 and C.B_effective=Cast('" + DT5.Rows[i]["B_effective"].ToString().Trim() + "' as smalldatetime)) as char) ,";
                Sqlstr += "   ApportionId='" + AppotID + "' ,";
                Sqlstr += "   ApportionName=(Select DepName From Department where DepCode='" + AppotID + "' and DepType='3' and company='" + comp + "') ,";
                Sqlstr += "   B_effective=Cast('" + Bdate + "' as smalldatetime)";
                Sqlstr += " Where 1=1";
                Sqlstr += " And Company='" + comp + "' ";

                if (!string.IsNullOrEmpty(Dep))
                {
                    Sqlstr += " and DeptId='" + Dep + "'";
                }
                else 
                {
                    Sqlstr += " And DeptId='" + DFP.Rows[j]["DeptId"].ToString().Trim() + "'";
                }

                if (!string.IsNullOrEmpty(Empoly))
                {
                    Sqlstr += " And EmployeeId='" + Empoly + "'";
                }
                else
                {
                    Sqlstr += " And EmployeeId='" + DFP.Rows[j]["EmployeeId"].ToString().Trim() + "'";
                }

                Sqlstr += " and ApportionId='" + AppotID + "'";

               // Sqlstr += " and B_effective=Cast('" + Bdate + "' as smalldatetime)";

                DT5 = _MyDBM.ExecuteDataTable(Sqlstr);
            }
              #endregion
          }
          else
          {
              string strLogMsg = "";
              try
              {
                  //strLogMsg = ":" + AppId + "|" + AppName + "|" + Bal + "|" + B_Date;
                  strLogMsg = comp + ":" + Dep + "|" + DFP.Rows[j]["EmployeeId"].ToString().Trim() + "|" + Balance + "|" + Bdate + "|" + AppotID;
              }
              catch
              {
                  strLogMsg = "批次新增";
              }

              #region 開始異動前,先寫入LOG
              DateTime StartDateTime = DateTime.Now;
              MyCmd.Parameters.Clear();
              MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "CostCenter_M";
              MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "A";
              MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
              MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = strLogMsg;
              MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
              MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
              _MyDBM.DataChgLog(MyCmd.Parameters);
              #endregion

              #region 新增資料
              //進行新增資料
              //同步檢查全公司或全部門相關人員分配是否合理
              Sqlstr = " Insert Into CostCenter(Company,DeptId,DeptName,EmployeeId,EmployeeName,ApportionId,ApportionName,Balance,B_effective) ";
              Sqlstr += " Select A.Company,";
              Sqlstr += "        A.DeptId, ";
              Sqlstr += "        B.DepName, ";
              Sqlstr += "        A.EmployeeId, ";
              Sqlstr += "        A.EmployeeName, ";
              Sqlstr += "        (Select DepCode From Department where DepCode='" + AppotID + "' and DepType='3' and company='" + comp + "') as ApportionId, ";
              Sqlstr += "        (Select DepName From Department where DepCode='" + AppotID + "' and DepType='3' and company='" + comp + "') as ApportionName, ";
              Sqlstr += "         Case When (100-Sum(Cast(C.Balance as decimal(9, 2))))< Cast('" + Balance + "'as decimal(9, 2)) then 100-Sum(Cast(C.Balance as decimal(9, 2))) ";
              Sqlstr += "              When 100-Sum(Cast(C.Balance as decimal(9, 2))) > Cast('" + Balance + "' as decimal(9, 2)) then '" + Balance + "' ";
              Sqlstr += "              When Isnull(100-Sum(Cast(C.Balance as decimal(9, 2))),0)=0 then '" + Balance + "' End Balance,";
              Sqlstr += "        Cast('" + Bdate + "' as Smalldatetime) as B_effective ";
              Sqlstr += "   From Personnel_Master A ";
              Sqlstr += "   Inner Join Department B ON A.Company=B.Company and A.DeptId=B.DepCode";
              Sqlstr += "   Left outer join CostCenter C ON C.Company=A.company and C.Deptid=A.DeptId and C.EmployeeId=A.EmployeeId";
              Sqlstr += "  Where 1=1";
              Sqlstr += "    and A.company='" + comp + "' ";
              Sqlstr += "    and A.DeptId=B.DepCode ";
              Sqlstr += "    and A.DeptId='" + DFP.Rows[j]["DeptId"].ToString().Trim() + "'";
              Sqlstr += "    and A.EmployeeId='" + DFP.Rows[j]["EmployeeId"].ToString().Trim() + "'";
              Sqlstr += "    GROUP BY A.company,A.DeptId,B.DepName,A.EmployeeId,A.EmployeeName ";

              DT5 = _MyDBM.ExecuteDataTable(Sqlstr);

            

              #endregion
          }
        }
        return; 
    }
    #endregion

    #region 更新資料
    /// <summary>
    /// 修改項目 
    /// </summary>
    //更新資料
    protected void GridView1RowUpdate()
    {//更新資料
        //string Err = "";
        string DDLP = "";
        DDLP = hid_IsInsertExit.Value.Replace("_", "$");
        string ID = hid_updateid.Value;
        string Value = Request.Form[DDLP + "$ctl02"];
        string Dep = Request.Form[DDLP + "$ctl03"];
        string Balan = Request.Form[DDLP + "$ctl04"];
        string B_effective = Request.Form[DDLP + "$ctl05"];
        string Sqlsttr = "";
        int i = 0;
        int P = 0;
        //if (hid_IsInsertExit.Value != "")
        //{ }
        if (i >= 0)
        {
            string strLogMsg = "";
            try
            {
                //strLogMsg = ":" + AppId + "|" + AppName + "|" + Bal + "|" + B_Date;
                strLogMsg = Value + ":" + Dep + "|" + Value + "|" + Balan + "|" + B_effective;
            }
            catch {
                strLogMsg = Value;
            }

                #region 開始異動前,先寫入LOG
                DateTime StartDateTime = DateTime.Now;
                MyCmd.Parameters.Clear();
                MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "CostCenter_M";
                MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "U";
                MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = ID;
                MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = strLogMsg;
                MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
                MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
                _MyDBM.DataChgLog(MyCmd.Parameters);
                #endregion

                #region 檢查是否總分攤比例是否超過100%
                // 檢查是否總分攤比例是否超過100%
                if (!ValidateData2(sCompany, sEmployeeId, Balan, Dep.Substring(0, Dep.IndexOf("-")).Trim()) || (int.TryParse(Balan, out P) == true && Convert.ToInt32(Balan) > 100))
                {
                    lbl_Msg3.Text = "修改失敗!!  原因: 分攤比例已超過 100% , 無法修改!! ";
                }
                #endregion
                else
                {
                    #region 檢查資料是否重覆
                    ////檢查資料是否重覆
                    //if (!ValidateData(sCompany, sEmployeeId, Dep.Substring(0, Dep.IndexOf("-")).Trim(), vDeptId))
                    //{
                    //    lbl_Msg3.Text = "修改失敗!!  原因: 資料重複,無法修改!! ";
                    //}
                    #endregion
                    //else
                    //{
                        try
                        {
                            Sqlsttr = " UPDATE CostCenter SET Balance=Cast(Cast('" + Balan.Trim() + "' as decimal(9, 2)) as varchar),";
                            Sqlsttr += " B_effective=Cast('" + _UserInfo.SysSet.FormatADDate(B_effective) + "' as SmallDatetime)";
                            Sqlsttr += " WHERE Company='" + sCompany + "' And EmployeeId='" + EmpleeoyCode.SelectedCode + "' And DeptId='" + DeptCode.SelectedCode + "'";
                            Sqlsttr += " and ApportionId='" + Dep.Substring(0, Dep.IndexOf("-")).Trim() + "'";
                            i = _MyDBM.ExecuteCommand(Sqlsttr);

                            if (i > 0)
                                Lmesg = "修改成功!<br>已更新[" + Value + "]項目的資料共" + i.ToString() + "筆 ";

                            else
                                Lmesg = "修改失敗!<br>找不到[" + Value + "]項目的資料 ";

                            lbl_Msg3.Text = Lmesg;
                        }
                        catch (Exception ex)
                        {
                            lbl_Msg3.Text = ex.Message;
                        }
                    //}
                }            

            #region 完成異動後,更新LOG資訊
            MyCmd.Parameters["@SQLcommand"].Value = (i == 1) ? "Success" : lbl_Msg3.Text.Replace("<br>", "");
            MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion
        }

        hid_updateid.Value = "";
        hid_IsInsertExit.Value = "";
        GridView1.EditIndex = -1;
        BindData();
    }
    #endregion
 
    #region 刪除項目

    /// <summary 刪除項目>
    /// 刪除項目   
    /// </summary>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        LinkButton btnDelete = (LinkButton)sender;
        string Ename = EmpleeoyCode.SelectedCode + EmpleeoyCode.SelectedCodeName;

        string L1PK = sCompany;
        string L2PK = EmpleeoyCode.SelectedCode;
        string L3PK = DeptCode.SelectedCode;
        string L4PK = btnDelete.Attributes["L4PK"].ToString();
        string L5PK = btnDelete.Attributes["L5PK"].ToString().Substring(0, btnDelete.Attributes["L5PK"].ToString().IndexOf("-")).Trim();
        string L6PK = btnDelete.Attributes["L6PK"].ToString();

        string strLogMsg = "";
        try
        {
            //strLogMsg = ":" + AppId + "|" + AppName + "|" + Bal + "|" + B_Date;
            strLogMsg = L3PK + "-" + L2PK + ":" + L4PK + "|" + L5PK + "|" + L6PK + "|";
        }
        catch { }

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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "CostCenter";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "D";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = strLogMsg;
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        string sql = "Delete From CostCenter Where Company='" + L1PK + "' And EmployeeId='" + L2PK + "' And DeptId='" + L3PK + "' And ApportionId='" + L5PK + "' And B_effective=Cast('" + _UserInfo.SysSet.FormatADDate(L4PK) + "' as SmallDateTime) and Balance='" + L6PK + "'";
      
        int result = _MyDBM.ExecuteCommand(sql.ToString());

        if (result > 0)
        {
            Lmesg = "資料刪除成功 !!<br>已刪除[" + Ename + "]項目的資料共" + result.ToString() + "筆 ";
            lbl_Msg3.Text = Lmesg;  
            //showSalaryLevel();
            //Navigator1.DataBind();
        }
        else
        {
            lbl_Msg3.Text = "資料刪除失敗 !!";
        }

       

        #region 完成異動後,更新LOG資訊
        MyCmd.Parameters["@SQLcommand"].Value = (result > 0) ? "Success" : lbl_Msg2.Text;
        MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        GridView1.EditIndex = -1;
        BindData();

        //計算總分攤比例
        if (!string.IsNullOrEmpty(L1PK) && !string.IsNullOrEmpty(L2PK))
        {
            Check_Sum_Total(L1PK, L2PK);
        }
        //showPanel();
    }    
     #endregion


    #region 檢查資料是否重覆
    //檢查資料是否重覆
    private bool ValidateData(string Company, string EmployeeId, string ApportionId, string DepCode)
    {
        Ssql = "Select * From CostCenter Where Company='" + Company + "'" +
               " And EmployeeId ='" + EmployeeId + "'" +
               " And ApportionId ='" + ApportionId + "'" +
               " And DeptId='" + DepCode + "'";

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
    #endregion

    #region 檢查是否總分攤比例是否超過100%

    private bool ValidateData2(string Company, string EmployeeId, string Balance,string ApporId)
    {//檢查是否總分攤比例是否超過100%
        int n;
        if (string.IsNullOrEmpty(ApporId))
        {//新增時加總
            Ssql = "Select Case When Sum(cast(A.Balance as decimal(9, 2)))";
            //判斷是否為整數
            int.TryParse(Balance, out n);
            if (int.TryParse(Balance, out n) != true)
            {
                //"非整數"; 
                Ssql += "+cast('" + Balance.Trim() + "' as decimal(9, 2))";
            }
            else
            {
                //"整數"; 
                Ssql += "+cast('" + Balance.Trim() + "' as int)";
            }

            Ssql += " >100 then '1' Else '0' end Count_B From CostCenter A";

        }
        else 
        {
          //更新時,由分攤部門代號,先行扣除舊比例,最後再依User輸入值做加總判斷是否大於100
            Ssql = " Select Case When Sum(cast(A.Balance as decimal(9, 2)))";

            if (int.TryParse(Balance, out n) != true)
            {
                //"非整數";
                Ssql += "+cast('" + Balance.Trim() + "' as decimal(9, 2))";
            }
            else
            {
                //"整數";  
                Ssql += "+cast('" + Balance.Trim() + "' as int)";
            }

            Ssql += "-cast(PM.Balance as int) >100 then '1' Else '0' end Count_B From CostCenter A ";

            Ssql += " Inner join (Select Company,EmployeeId,ApportionId,Balance From CostCenter) PM On PM.Company=A.Company and PM.EmployeeId=A.EmployeeId and PM.ApportionId='" + ApporId + "'";
         }
        Ssql += " Where A.Company='" + Company.TrimEnd() + "' And A.EmployeeId='" + EmployeeId.TrimEnd() + "' ";

        if (!string.IsNullOrEmpty(ApporId))
        {
            Ssql += " Group by PM.Balance";
        }

        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);

        if (Convert.ToInt32(tb.Rows[0]["Count_B"]) > 0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }
    #endregion

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (!String.IsNullOrEmpty(e.CommandName))
        {
            try
            {
                //GridViewRow gvRow = null;
                switch (e.CommandName)
                {
                    case "Delete":
                    case "Update":
                    case "Insert":
                        break;
                    case "Edit":
                    case "Cancel":
                    default:
                        lbl_Msg.Text = "";
                        break;
                }
            }
            catch (Exception ex)
            {
                lbl_Msg.Text = ex.Message;
            }
        }
    }

    #region 表格欄位處理
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
        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {          
            e.Row.Visible = false;
        }
        
    }
    #endregion

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        BindData();
    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        BindData();
    }
    
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string strValue = ""; 
        string sqlstr="";

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            
            string tempValue = e.Row.Cells[2].Text.Trim();

            sqlstr = "Select Company,Rtrim(DeptId)+'-'+Rtrim(DeptName) as DeptId, DeptName ,Rtrim(EmployeeId)+'-'+EmployeeName as EmployeeId,Rtrim(ApportionId)+'-'+Rtrim(ApportionName) as ApportionId,ApportionName,Balance,B_effective FROM CostCenter Where DeptId='" + tempValue.Substring(0, tempValue.IndexOf("-")).Trim() + "'";

            DataTable dt4 = _MyDBM.ExecuteDataTable(sqlstr);           
                       

            if ((e.Row.RowState == System.Web.UI.WebControls.DataControlRowState.Edit)||(e.Row.RowState == (System.Web.UI.WebControls.DataControlRowState.Alternate | System.Web.UI.WebControls.DataControlRowState.Edit)))
            {               
                #region 修改用
                    //確認
                    if (e.Row.Cells[1].Controls[0] != null)
                    {
                        ImageButton IB = (ImageButton)e.Row.Cells[1].Controls[0];
                        IB.Attributes.Add("onclick", "return (confirm('確定要修改資料嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "') && SaveValue(" + hid_updateid.ClientID + ",'" + tempValue.Trim() + "'));");
                        IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                        IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                        IB.Style.Add("filter", "alpha(opacity=50)");
                    }
                    //取消
                    if (e.Row.Cells[1].Controls[2] != null)
                    {
                        ImageButton IB = ((ImageButton)e.Row.Cells[1].Controls[2]);
                        IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                        IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                        IB.Style.Add("filter", "alpha(opacity=50)");
                    }

                    e.Row.Cells[3].Enabled = false;

                    //分攤部門
                    e.Row.Cells[4].Enabled = false;

                    //分攤比例
                    TextBox TB2 = ((TextBox)e.Row.Cells[5].Controls[0]);

                    if (TB2 != null)
                    {
                        TB2.Style.Add("text-align", "right");
                        TB2.MaxLength = 6;
                    }

                    //生效日期
                    TextBox tbAddNew = ((TextBox)e.Row.Cells[6].Controls[0]);
                    //為日期欄位增加小日曆元件
                    tbAddNew.CssClass = "JQCalendar";
                    //需要加日期開窗的欄位
                    strValue += "checkDate(" + tbAddNew.ClientID + ") && ";


                    #endregion
               
            }
            else
            {
                #region 查詢用
                if (e.Row.Cells[0].Controls[0] != null)
                {//刪除鈕
                    LinkButton LB = (LinkButton)e.Row.Cells[0].Controls[1];
                }

                if (e.Row.Cells[1].Controls[0] != null)
                {//編輯鈕
                    ImageButton IB = (ImageButton)e.Row.Cells[1].Controls[0];
                    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                    IB.Style.Add("filter", "alpha(opacity=50)");
                }

                #endregion
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {           
            #region 有值時使用 新增用欄位
                    for (int i = 2; i < e.Row.Cells.Count; i++)
                    {
                        if (i == 2)
                        {
                            //下拉式選單(部門)
                            TextBox tbAddNew4 = new TextBox();
                            tbAddNew4.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                            tbAddNew4.Text = DeptCode.SelectedCodeName;
                            e.Row.Cells[i].Controls.Add(tbAddNew4);
                            e.Row.Cells[i].Enabled = false;
                        }
                        if (i == 3)
                        {
                            //下拉式選單(員工)
                            TextBox tbAddNew4 = new TextBox();
                            tbAddNew4.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                            tbAddNew4.Text = EmpleeoyCode.SelectedCodeName;
                            e.Row.Cells[i].Controls.Add(tbAddNew4);
                            e.Row.Cells[i].Enabled = false;
                        }
                        if (i == 4)
                        {
                            //下拉式選單(分攤至部門)
                            ASP.usercontrol_codelist_ascx ddlAddNew = new ASP.usercontrol_codelist_ascx();
                            ddlAddNew = (ASP.usercontrol_codelist_ascx)LoadControl("~/UserControl/CodeList.ascx");
                            Ssql = (string.IsNullOrEmpty(CompanyList1.SelectValue) ? "" : "Company='" + CompanyList1.SelectValue + "' and Deptype='3'");
                            ddlAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                            ddlAddNew.SetDTList("Department", "DepCode", "DepName", Ssql, 5);
                            e.Row.Cells[i].Controls.Add(ddlAddNew);
                            strValue += "checkColumns(" + ddlAddNew.CLClientID() + ") && ";

                        }
                        if (i == 5)
                        {
                            //分攤比例
                            TextBox tbAddNew = new TextBox();
                            tbAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                            tbAddNew.Style.Add("text-align", "right");
                            tbAddNew.Style.Add("width", "150px");
                            tbAddNew.MaxLength = 6;
                            e.Row.Cells[i].Controls.Add(tbAddNew);
                            strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
                        }
                        if (i == 6)
                        {
                            //生效日期                   
                            TextBox tbAddNew3 = new TextBox();
                            tbAddNew3.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                            ////為日期欄位增加小日曆元件
                            tbAddNew3.CssClass = "JQCalendar";
                            e.Row.Cells[i].Controls.Add(tbAddNew3);
                            ////需要加日期開窗的欄位
                            strValue += "checkDate(" + tbAddNew3.ClientID + ") && ";
                        }

                    }

                    ImageButton btAddNew = new ImageButton();
                    btAddNew.ID = "btAddNew";
                    btAddNew.SkinID = "NewAdd";
                    btAddNew.CommandName = "Insert";
                    btAddNew.OnClientClick = "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));";
                    e.Row.Cells[1].Controls.Add(btAddNew);

                    #endregion
         
        }
        else if (e.Row.RowType == DataControlRowType.EmptyDataRow)
        {            
            //權限
            e.Row.Visible = GridView1.ShowFooter;

            #region 空值時使用 新增用欄位
                    for (int i = 2; i < 6; i++)
                    {

                        if (i == 3)
                        {
                            //下拉式選單(分攤至部門)
                            //初始化下拉單
                            ASP.usercontrol_codelist_ascx ddlAddNew = (ASP.usercontrol_codelist_ascx)e.Row.FindControl("tbAddNew0" + i.ToString());
                            Ssql = (string.IsNullOrEmpty(CompanyList1.SelectValue) ? "" : "Company='" + CompanyList1.SelectValue + "' and Deptype='3'");
                            ddlAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                            ddlAddNew.SetDTList("Department", "DepCode", "DepName", Ssql, 5);
                            strValue += "checkColumns(" + ddlAddNew.CLClientID() + ") && ";
                        }

                        if (i == 4)
                        {
                            //分攤比例  
                            TextBox tbAddNew = (TextBox)e.Row.FindControl("tbAddNew" + i.ToString().PadLeft(2, '0'));
                            tbAddNew.Style.Add("text-align", "right");
                            tbAddNew.Style.Add("width", "150px");
                            tbAddNew.MaxLength = 6;
                            strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
                        }
                        if (i == 5)
                        {

                            TextBox tbAddNew = (TextBox)e.Row.FindControl("tbAddNew0" + i.ToString());

                            //為日期欄位增加小日曆元件
                            tbAddNew.CssClass = "JQCalendar";
                            //需要加日期開窗的欄位
                            strValue += "checkDate(" + tbAddNew.ClientID + ") && ";
                        }

                    }

                    ImageButton btnNew = (ImageButton)e.Row.FindControl("btnNew");
                    if (btnNew != null)
                        btnNew.Attributes.Add("onclick", "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.UniqueID + "'));");

                    #endregion                                
        }
    }

    #region 單一員工,資料庫加總比例
    protected void Check_Sum_Total(string company, string EmployeeId) 
    {
      //單一員工,資料庫加總比例

        string Sqlstr = "";
        string strWhere = "";

        Sqlstr = " Select SUM(CAST(Balance AS decimal(9, 2))) AS Balance_2 FROM CostCenter WHERE 1=1 ";

        if (!string.IsNullOrEmpty(company))
        {
            strWhere += string.Format(" And Company ='" + company + "'");
        }
        if (!string.IsNullOrEmpty(EmployeeId))
        {
            strWhere += string.Format(" And EmployeeId ='"+EmployeeId+"'");
        }
     
        Sqlstr += strWhere; 

        DataTable DFP = _MyDBM.ExecuteDataTable(Sqlstr);
        Decimal i=0;
        try
        {
            i = Convert.ToDecimal(DFP.Rows[0]["Balance_2"].ToString());
        }
        catch 
        {
            i = 0;
        }

        lbl_Msg2.Text = "剩餘分攤比例：" + (100 - i).ToString() + " %";
        Balance_3.Text = i.ToString() + " %";
    }
    #endregion
}