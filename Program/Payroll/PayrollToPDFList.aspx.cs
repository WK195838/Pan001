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
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using Ionic.Zip;
using System.Net.Mail;
using System.Net.Mime;

public partial class PayrollToPDFList : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM024";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    Payroll thePayroll = new Payroll();
    Payroll.PayrolList PayrollLt = new Payroll.PayrolList();
    Decimal dFixedSalary = 0, dNonFixedAmount = 0, dOtherNT = 0, dOtherWT = 0, dMAmount = 0;
    string sKind = "H";
    string PayrollTable = "PayrollWorking";
    string sCompany, sSYM, sDepartment;
    string path = "", pdfpwd = "";
    bool blCheckBatch = false;
    public static string sEmployeeId = string.Empty;
    public static string outputMsg = string.Empty;
    string PDFKind = "PayRollToPDF";
    public int iPdfPageCount = 0;

    protected void Page_PreInit(object sender, EventArgs e)
    {//頁面初始化前,先設定好系統使用的佈景主題(主要是決定LOGO)
        if (Session["Theme"] != null)
            Page.Theme = Session["Theme"].ToString();

        if (Session["MasterPage"] != null)
            Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";

        if (!_UserInfo.SysSet.GetConfigString("SYSMode").Contains("OfficialVersion"))
            PayrollTable = "PayrolltestWorking";
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _MyDBM = new DBManger();
        _MyDBM.New();
        //DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/PayrollControl.aspx'");
        //if (DT.Rows.Count > 0)
        //    _ProgramId = DT.Rows[0][0].ToString();
        path = Server.MapPath(Request.Url.AbsolutePath);
        path = path.Remove(path.LastIndexOf("\\"));
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString(), Page.ResolveUrl("~/Pages/pagefunction.js").ToString());
        ////用於執行等待畫面
        //Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "B", Page.ResolveUrl("~/Pages/Busy.js").ToString());
        //// 需要執行等待畫面的按鈕
        //btntopdf.Attributes.Add("onClick", "drawWait('')");
        //ScriptManager1.RegisterPostBackControl(btntopdf);

        lbl_Msg.Text = "";

        if ((Request["erpadm"] != null && Request["erpadm"].ToString() == "y") || (_UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, "Batch") != false))
            blCheckBatch = true;

        if (_UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, "Print") == true)
            showPrint.Style.Clear();
        else
            rbPrint.SelectedValue = "N";

        if (blCheckBatch)
        {
            erpadmS1.Style.Clear();
        }
        else
        {
            erpadmS1.Style.Add("display", "none");
        }

        if (!Page.IsPostBack)
        {
            #region 登入者之權限管理
            if (_UserInfo.AuthLogin == false)
            {
                Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=UnLogin");
            }
            else
            {
                if (_UserInfo.CheckPermission(_ProgramId) == false)
                    Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
            }
            OldId.Text = _UserInfo.UData.EmployeeId.Trim();
            OldId.Enabled = false;
            if (OldId.Text.Length == 0)
            {
                if (_UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, "Batch") != false)
                    OldId.Enabled = true;
                else
                    Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=UserNoEID");
            }
            #endregion            
            SalaryYM1.SetSalaryYM(SearchList1.CompanyValue.Trim());//月份
            SalaryYM2.SetSalaryYM(SearchList1.CompanyValue.Trim());//月份
            if (PayrollTable.Contains("test"))
                StyleTitle1.Title += "(測試模式)";
        }
        else
        {
            lbl_Msg.Text = "";
            SearchList1.ShowResignEmp = (rbResignC.SelectedValue == "") ? 2 : (rbResignC.SelectedValue == "Y") ? 0 : 1;
            SearchList1.ReSetList();
        }

        if (SearchList1.CompanyValue.Trim() != "")
        {
            //sCompany = SearchList1.CompanyValue.Trim() + " - " + DBSetting.CompanyName(SearchList1.CompanyValue.Trim());
            sCompany = DBSetting.CompanyName(SearchList1.CompanyValue.Trim());
            PayrollLt.Company = SearchList1.CompanyValue.Trim();
        }

        //if (!string.IsNullOrEmpty(_UserInfo.UData.EmployeeId))
        //{
        //    sEmployeeId = _UserInfo.UData.EmployeeId + " - " + DBSetting.PersonalName(SearchList1.CompanyValue.Trim(), _UserInfo.UData.EmployeeId);
        //    PayrollLt.EmployeeId = _UserInfo.UData.EmployeeId;
        //}

        if (SalaryYM1.SelectSalaryYM.Trim() != null)
        {
            sSYM = (int.Parse(SalaryYM1.SelectSalaryYM.Trim().Substring(0, 4)) - 1911).ToString() + "年" + SalaryYM1.SelectSalaryYM.Trim().Substring(4, 2) + "月";
        }


        PayrollLt.PeriodCode = "0";
        PayrollLt.SalaryYM = Convert.ToInt32(SalaryYM1.SelectSalaryYM.Trim());
        rbPDFPwd.Attributes.Add("onclick", "javascript:document.getElementById(\'" + tbPDFPwd.ClientID + "\').value='';");
    }

    protected void btntopdf_Click(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";

        //判斷PDF下載密碼是否為身分證字號
        if (Chk_ID() != true)
        {
            return;
        }
        else if (Changdata(2) != true)
        {//下載前先檢查與更新 DataTable
            return;
        }
        //lbl_Msg.Text = "下載完成!";
        //下載時間
        String DownLoadTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        #region 寫入Log
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = PDFKind;
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "Q";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "下載薪資單";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "來源IP:" + Request.UserHostAddress;
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = DownLoadTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = SearchList1.EmployeeValue.Trim();// _UserInfo.UData.UserId
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        //設定共用資訊        
        PayrollLt.DeCodeKey = "dbo.DLMyPDF" + DateTime.Now.ToString("yyyyMMddHHmm");
        thePayroll.BeforeQuery(PayrollLt.DeCodeKey);

        #region 設定下載目錄及LOG檔
        string FullPathFiles = path + (path.EndsWith("\\") ? "" : "\\") + "DL\\下載薪資PDF.log";
        System.IO.FileInfo fileInfo = new System.IO.FileInfo(FullPathFiles);
        if (!fileInfo.Directory.Exists)
        {
            fileInfo.Directory.Create();
        }
        else
        {//先清空原資料夾內容
            try
            {
                fileInfo.Directory.Delete(true);
                fileInfo.Directory.Create();
            }
            catch { }
        }
        #endregion

        if (SearchList1.EmployeeValue.Replace("%", "").Trim() != "" || blCheckBatch == false)
        {
            #region 一般員工使用時,僅提供單一資料下載
            if (blCheckBatch != false && SearchList1.EmployeeValue.Replace("%", "").Trim() != "")
                PayrollLt.EmployeeId = SearchList1.EmployeeValue.Trim();
            else
                PayrollLt.EmployeeId = OldId.Text.Trim();
            DownLoadPDF(PayrollLt.EmployeeId, DownLoadTime);
            //檔案下載                 
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + PayrollLt.Company + "-" + PayrollLt.EmployeeId + "_" + SalaryYM1.SelectSalaryYM + "-" + SalaryYM2.SelectSalaryYM + ".pdf");
            //Response.ContentType = "application/octet-stream";
            //Response.Flush();            
            FullPathFiles = PayrollLt.Company + "-" + PayrollLt.EmployeeId + "_" + SalaryYM1.SelectSalaryYM + "-" + SalaryYM2.SelectSalaryYM + ".pdf";
            fileInfo = new System.IO.FileInfo(path + (path.EndsWith("\\") ? "DL\\" : "\\DL\\") + FullPathFiles);
            Response.Clear();
            Response.Write("<script language=javascript>");
            Response.Write("<!--");
            Response.Write(Convert.ToChar(13));
            if (lbl_Msg.Text.Trim() != "")
                Response.Write("alert('" + lbl_Msg.Text + "');");
            //導向PDF:endResponse必須設為false以將本頁應執行之動作完成
            //Response.Redirect("DL//" + PayrollLt.Company + "-" + PayrollLt.EmployeeId + "_" + SalaryYM1.SelectSalaryYM + "-" + SalaryYM2.SelectSalaryYM + ".pdf", false);
            if (cbEmail.Checked != false || !fileInfo.Exists)
                FullPathFiles = Request.Url.AbsolutePath;
            else
                FullPathFiles = "DL/" + FullPathFiles;
            Response.Write("window.location='" + FullPathFiles + "'");
            Response.Write("//-->");
            Response.Write(Convert.ToChar(13));
            Response.Write("</script>");
            //Response.Redirect(Request.Url.AbsolutePath, false);   
            #endregion
        }
        else
        {
            #region 全公司或指定部門批次產生PDF檔
            Ssql = "SELECT PM.EmployeeId,CAST(PayRollPW AS varchar) as PayRollPW ,[IDNo],[EmployeeName] " +
            " from [Personnel_Master] PM Left join PersonnelSecurity PS On PM.[Company]=PS.[Company] And PM.[EmployeeId]=PS.[EmployeeId] " +
            " where PM.Company='" + SearchList1.CompanyValue.Trim() + "' ";

            if (SearchList1.DepartmentValue.Replace("%", "").Trim() != "")
            {
                Ssql += " and PM.[DeptId]='" + SearchList1.DepartmentValue.Trim() + "'";
            }

            if (rbResignC.SelectedValue == "Y")
                Ssql += " And Upper(IsNull(ResignCode,'')) != 'Y' ";
            else if (rbResignC.SelectedValue == "N")
                Ssql += " And Upper(IsNull(ResignCode,'')) = 'Y' ";

            DataTable dtb = _MyDBM.ExecuteDataTable(Ssql);
            if (dtb != null && dtb.Rows.Count > 0)
            {
                foreach (DataRow dr in dtb.Rows)
                {
                    PayrollLt.EmployeeId = dr["EmployeeId"].ToString().Trim();
                    if (dr["PayRollPW"] != null && dr["PayRollPW"].ToString() != "")
                        pdfpwd = dr["PayRollPW"].ToString();
                    else
                        pdfpwd = "";
                    DownLoadPDF(PayrollLt.EmployeeId, DownLoadTime, dr["EmployeeName"].ToString().Trim(), dr["IDNo"].ToString().Trim(), pdfpwd);
                }
            }
            else
            {
                lbl_Msg.Text = "[" + SearchList1.CompanyText.Trim() + "]";
                if (SearchList1.DepartmentValue.Replace("%", "").Trim() != "")
                {
                    lbl_Msg.Text += "所屬之[" + SearchList1.DepartmentText.Trim() + "]";
                }
                lbl_Msg.Text += "查無員工資料!";
                _UserInfo.SysSet.WriteToLogs("下載薪資PDF", lbl_Msg.Text);
            }
            #endregion
        }

        if (SearchList1.EmployeeValue.Replace("%", "").Trim() == "")
        {
            #region 壓縮與下載
            try
            {
                string filename = "DL\\" + System.IO.Path.GetFileName(SalaryYM1.SelectSalaryYM.Trim() + "-PayRoll") + ".zip";
                ////將 path 下之目錄"DL"壓縮至 filename 檔案中的 PayRoll 目錄 並存至伺服器之指定位置 path
                //using (ZipFile zip = new ZipFile(path + (path.EndsWith("\\") ? "" : "\\") + filename))
                //{
                //    zip.AddItem(path + (path.EndsWith("\\") ? "" : "\\") + "DL", "PayRoll");
                //    zip.Save();
                //}

                //將 path 下之目錄"DL"壓縮至 filename 檔案中的 PayRoll 目錄 並直接提供另存新檔
                Response.Clear();
                String ReadmeText = "This is a zip file dynamically generated at "
                  + System.DateTime.Now.ToString("G");
                //Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "filename=" + filename);
                using (ZipFile zip = new ZipFile(System.Text.Encoding.Default))
                {//System.Text.Encoding.Default必設:否則若壓內容之檔名有中文時,會出現亂碼
                    //設定壓縮檔密碼
                    zip.Password = this.OldPassword.Text.Trim();
                    //加入指定資料夾下所有檔案並指定目錄
                    zip.AddItem(path + (path.EndsWith("\\") ? "" : "\\") + "DL", "PayRoll");
                    //加入即時新增之檔案
                    zip.AddEntry("Readme.txt", "請查閱[下載薪資PDF.log]以便獲得此次下載之問題紀錄");
                    zip.Save(Response.OutputStream);
                }
                if (fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Delete(true);
                }
            }
            catch (Exception ex)
            {
                _UserInfo.SysSet.WriteToLogs("下載薪資PDF", "ZIP壓縮時發生錯誤:" + ex.Message);
            }
            #endregion
        }
        thePayroll.AfterQuery(PayrollLt.DeCodeKey);
        //寫入異動結束時間
        MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        _MyDBM.DataChgLog(MyCmd.Parameters);

        //沒有 Response.End() 的話就要觸發 Exception 以中止頁面顯示,否則下載的壓縮檔會是空的!!        
        Response.End();
        
        ////因為用Response產生壓縮檔供下載的關係,下列關閉及導向語法皆無法作用;故不提供執行中的等待畫面
        //Response.Flush();
        ////關閉執行等待畫面
        //PanPacificClass.JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");
        //StringBuilder str = new StringBuilder();
        //str.Append("<script language=javascript>");
        //str.Append("window.location='" + this.Request.Url.AbsoluteUri + "';");
        //str.Append("</script>");
        //Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", str.ToString());
        //ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "unloadPI", "window.location='" + this.Request.Url.AbsoluteUri + "';", true);
        //Response.Redirect(this.Request.Url.AbsoluteUri);
    }

    private void DownLoadPDF(string ID, string DownLoadTime)
    {
        DownLoadPDF(ID, DownLoadTime, "", "", "");
    }

    private void DownLoadPDF(string ID, string DownLoadTime, string theName, string theIdNo, string thePwd)
    {
        string strMsg = "";
        iTextSharp.text.Document doc = new Document(PageSize.A4);
        #region 產生加密之PDF
        try
        {
            //MemoryStream Memory = new MemoryStream();
            //PdfWriter writer = PdfWriter.GetInstance(doc, Memory);            
            if (hasdata(ID))
            {
                string strEMail = "";
                pdfpwd = "";
                #region 原本不用找計薪當月的部門,所以可以直接使用傳入的資訊,現在改為一率抓資料庫
                //if (theName != "" && theIdNo != "")
                //{
                //    //找出此薪資單員工下載密碼
                //    if (thePwd != "")
                //        pdfpwd = _UserInfo.SysSet.rtnTrans(thePwd);
                //    else
                //        pdfpwd = theIdNo;
                //    //找出此薪資單員工姓名
                //    sEmployeeId = ID.Trim() + " - " + theName;
                //    strEMail = DBSetting.PersonalEMail(SearchList1.CompanyValue.Trim(), ID);
                //}
                //else
                #endregion
                {
                    Ssql = "SELECT CAST(PayRollPW AS varchar) as PayRollPW ,[IDNo],[EmployeeName],IsNull(Email,'') As [Email],[DeptId] " +
                        " from (select Company,EmployeeId,EmployeeName,EnglishName " +
                        " ,[IDNo],[Email] " +
                        " ,IsNull((Select Top 1 DepCode_F From PersonnelAdjustment " +
                        "  Where Company=pm.Company and EmployeeId=pm.EmployeeId " +
                        "  And Convert(varchar,EffectiveDate,112) >= ('" + SalaryYM1.SelectSalaryYM + "31') " +
                        "  order by EffectiveDate),DeptId) As [DeptId] From Personnel_Master pm) PM " +
                        " Left join PersonnelSecurity PS On PM.[Company]=PS.[Company] And PM.[EmployeeId]=PS.[EmployeeId] " +
                        //在[PersonnelSecurity]中,只有公司是[Pan-Pacific]才是用於薪資下載的AD帳號,其它屬於登入用帳號不用自動寄信
                        " And PS.[CompanyCode]='Pan-Pacific' " +
                        " where PM.Company='" + SearchList1.CompanyValue.Trim() +
                        "' and PM.EmployeeId='" + ID.Trim() + "'";

                    DataTable dtb = _MyDBM.ExecuteDataTable(Ssql);
                    if (dtb != null && dtb.Rows.Count > 0)
                    {
                        //找出此薪資單員工下載密碼
                        if (dtb.Rows[0]["PayRollPW"] != null && dtb.Rows[0]["PayRollPW"].ToString() != "")
                            pdfpwd = _UserInfo.SysSet.rtnTrans(dtb.Rows[0]["PayRollPW"].ToString());
                        else
                            pdfpwd = dtb.Rows[0]["IDNo"].ToString();

                        //找出此薪資單員工姓名
                        sEmployeeId = ID.Trim() + " - " + dtb.Rows[0]["EmployeeName"].ToString().Trim();

                        if (dtb.Rows[0]["Email"] != null && dtb.Rows[0]["Email"].ToString() != "")
                            strEMail = dtb.Rows[0]["Email"].ToString().Trim();

                        //取得部門名稱(計薪當月)
                        if (dtb.Rows[0]["DeptId"] != null && dtb.Rows[0]["DeptId"].ToString() != "")
                        {
                            sDepartment = dtb.Rows[0]["DeptId"].ToString().Trim();
                            sDepartment = DBSetting.DepartmentName(SearchList1.CompanyValue.Trim(), sDepartment);
                        }
                    }
                    else
                    {
                        if (theName != "" && theIdNo != "")
                        {
                            //找出此薪資單員工下載密碼
                            if (thePwd != "")
                                pdfpwd = _UserInfo.SysSet.rtnTrans(thePwd);
                            else
                                pdfpwd = theIdNo;
                            //找出此薪資單員工姓名
                            sEmployeeId = ID.Trim() + " - " + theName;
                            strEMail = DBSetting.PersonalEMail(SearchList1.CompanyValue.Trim(), ID);
                        }
                        sDepartment = "";
                    }
                }
                //若計薪當月部門名稱為空,則取得最新所屬部門名稱
                if (string.IsNullOrEmpty(sDepartment))
                    sDepartment = DBSetting.PersonalDepartmentName(SearchList1.CompanyValue.Trim(), ID);

                if (rbPDFPwd.SelectedValue != "d" && blCheckBatch != false)
                {
                    if (rbPDFPwd.SelectedValue == "s")
                        pdfpwd = (tbPDFPwd.Text.Trim().Length != 0) ? tbPDFPwd.Text.Trim() : OldPassword.Text.Trim();
                    else
                        pdfpwd = "";
                }

                if (pdfpwd != "" || rbPDFPwd.SelectedValue == "n")
                {
                    #region 產生PDF
                    string FileName = PayrollLt.Company + "-" + ID + "_" + SalaryYM1.SelectSalaryYM + "-" + SalaryYM2.SelectSalaryYM + ".pdf";
                    string FullPathFiles = path + (path.EndsWith("\\") ? "" : "\\") + "DL\\" + FileName;
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(FullPathFiles);
                    if (!fileInfo.Directory.Exists)
                    {
                        fileInfo.Directory.Create();
                    }
                    else if (fileInfo.Exists)
                    {
                        fileInfo.Delete();
                    }

                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(FullPathFiles, FileMode.Create));
                    #region PDF保全相關設定(2012/07/03增修)
                    if (rbPDFPwd.SelectedValue == "n")
                    {//不等於N時,PDF以個密碼加密;等於n時不設密碼
                        pdfpwd = "";
                    }
                    //權限密碼
                    string ownerpass = "";
                    //不允許列印,不允許複製.....
                    int iPermission = 0;

                    if (rbPrint.SelectedValue == "Y")
                    {//允許列印
                        iPermission = PdfWriter.AllowPrinting;
                    }

                    //PDF是否以個密碼加密,權限密碼設定(空白即隨機給定,故無法使用),是否允許列印...
                    writer.SetEncryption(PdfWriter.STRENGTH128BITS, pdfpwd, ownerpass, iPermission);
                    #endregion

                    #region 設置PDF的標題列信息，一些屬性設置，在Document.Open 之前完成
                    doc.AddAuthor(_UserInfo.UData.CompanyName);
                    doc.AddCreationDate();
                    doc.AddCreator(_UserInfo.UData.EmployeeId);
                    doc.AddSubject("人事薪資系統");
                    doc.AddTitle("個人薪資單");
                    //doc.AddKeywords("ASP.NET,PDF,iTextSharp,DeltaCat,三角猫");
                    //自定义头
                    doc.AddHeader("Expires", "0");
                    #endregion

                    //打开document
                    doc.Open();

                    iPdfPageCount = 0;
                    int iYMs = Convert.ToInt32(SalaryYM1.SelectSalaryYM);
                    int iYMe = Convert.ToInt32(SalaryYM2.SelectSalaryYM);
                    for (int iTheYM = iYMs; iTheYM <= iYMe; iTheYM++)
                    {
                        int tryMonth = iTheYM % 100;
                        if (tryMonth >= 1 && tryMonth <= 12)
                        {
                            #region 預先計算一些金額:避免畫PDF時反復計算
                            PayrollLt.SalaryYM = iTheYM;

                            //固定金額:取得經常性薪資項目後,再減去請假扣款
                            dFixedSalary = (decimal)(thePayroll.GetPersonalYMRegularPay(PayrollLt));
                            //dFixedSalary -= (decimal)(thePayroll.GetPersonalHoursSalary(PayrollLt) * thePayroll.GetLeaveHours(PayrollLt.Company, PayrollLt.EmployeeId, PayrollLt.SalaryYM.ToString()));
                            //應扣金額
                            dMAmount = GetMAmount();
                            //其它應稅:含應稅調整,應稅加班
                            dOtherWT = Convert.ToDecimal(thePayroll.GetPayrollNWTSalary(PayrollLt, "Y"));
                            //其它免稅:含免稅調整,免稅加班
                            dOtherNT = Convert.ToDecimal(thePayroll.GetPayrollNWTSalary(PayrollLt, "N"));
                            //非固定金額:目前應為其它應稅項目,不含其他扣款
                            dNonFixedAmount = dOtherWT;
                            #endregion

                            doc = DrawPDF(doc);
                        }
                    }                                   
                    
                    //关闭document
                    doc.Close();
                    
                    // doc.Close();
                    //打开PDF，看效果
                    //Process.Start("chap03.pdf");
                    #endregion
                    #region 寫入PersonnelSecurity LOG(其實有檔案型LOG,這個可以不記)
                    //將下載人員與時間寫進 PersonnelSecurity Table內
                    String Sql;
                    DataTable InserTable;
                    //下載人員
                    String DownLoadPs = OldId.Text.Trim();
                    Sql = "IF NOT EXISTS (SELECT * FROM [PersonnelSecurity] where Company='" + SearchList1.CompanyValue.Trim() + "' And EmployeeId='" + ID.Trim() + "') " +
                        " INSERT INTO PersonnelSecurity(Company,EmployeeId,CompanyCode,ERPID,PayRollPW,DownLoadTime,DownLoadPersonnel) " +
                        " VALUES('" + SearchList1.CompanyValue.Trim() + "','" + ID + "','',''" +
                        " ,CAST('" + _UserInfo.SysSet.rtnHash(pdfpwd) + "' as varbinary)" +
                        " ,CAST('" + DownLoadTime + "' as smalldatetime),'" + DownLoadPs + "')" +
                        " else" +
                        " Update PersonnelSecurity Set DownLoadTime=CAST('" + DownLoadTime + "' as smalldatetime), DownLoadPersonnel='" + DownLoadPs + "' where Company='" + SearchList1.CompanyValue.Trim() + "' And EmployeeId='" + ID.Trim() + "'";

                    InserTable = _MyDBM.ExecuteDataTable(Sql);
                    #endregion
                    #region 寄送E-MAIL
                    if (cbEmail.Checked != false)
                    {
                        try
                        {
                            //For 測試使用
                            //if (!string.IsNullOrEmpty(strEMail))
                            //    strEMail = _UserInfo.SysSet.GetConfigString("MailAdmin1");
                            EMail("", strEMail, "薪資條收件人", FullPathFiles, (string.IsNullOrEmpty(strEMail) ? "此員工未設定電子信箱" : ""));
                        }
                        catch (Exception ex)
                        {
                            strEMail = _UserInfo.SysSet.GetConfigString("MailAdmin1");
                            EMail("", strEMail, "財務部門", FullPathFiles, ex.Message);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 未產生PDF,寫入ERROR LOG
                    lbl_Msg.Text = "部份員工無法設定下載密碼!";
                    strMsg = "員工代號[" + ID + "]無法設定下載密碼!";
                    _UserInfo.SysSet.WriteToLogs("下載薪資PDF", strMsg);
                    _UserInfo.SysSet.WriteTofiles(path + "\\DL", "下載薪資PDF.log", strMsg);
                    #endregion
                }
            }
            else
            {
                #region 未產生PDF,寫入ERROR LOG
                lbl_Msg.Text = "部份員工查無資料!";
                strMsg = "員工代號[" + ID + "]查無資料!" + (checkResign(ID) ? "離職員工" : "");
                _UserInfo.SysSet.WriteToLogs("下載薪資PDF", strMsg);
                _UserInfo.SysSet.WriteTofiles(path + "\\DL", "下載薪資PDF.log", strMsg);
                #endregion
            }
        }
        catch (DocumentException de)
        {
            lbl_Msg.Text = de.Message;
            #region 未產生PDF,寫入ERROR LOG
            strMsg = "員工代號[" + ID + "]資料有誤!" + lbl_Msg.Text;
            _UserInfo.SysSet.WriteToLogs("下載薪資PDF", strMsg);
            _UserInfo.SysSet.WriteTofiles(path + "\\DL", "下載薪資PDF.log", strMsg);
            #endregion
            //Console.ReadKey();
        }
        catch (IOException io)
        {
            lbl_Msg.Text = io.Message;
            #region 未產生PDF,寫入ERROR LOG
            strMsg = "員工代號[" + ID + "]資料有誤!" + lbl_Msg.Text;
            _UserInfo.SysSet.WriteToLogs("下載薪資PDF", strMsg);
            _UserInfo.SysSet.WriteTofiles(path + "\\DL", "下載薪資PDF.log", strMsg);
            #endregion
            //Console.ReadKey();
        }

        #endregion
    }

    protected iTextSharp.text.Document DrawPDF(iTextSharp.text.Document doc)
    {
        if (CheckSalaryYM() == false)
            return doc;
        else
            iPdfPageCount++;

        if (iPdfPageCount > 1)
        {
            //開啟新的一頁
            doc.NewPage();
        }       

        #region 設置PDF字型及字體大小
        string fontPath = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\..\Fonts\KAIU.TTF";
        //微軟正黑體            
        BaseFont bfChineseThin = BaseFont.CreateFont(@"c:\WINDOWS\fonts\msjh.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        //载入字体
        BaseFont baseFT = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        iTextSharp.text.Font Titlefont = new iTextSharp.text.Font(baseFT, 20);
        iTextSharp.text.Font font = new iTextSharp.text.Font(bfChineseThin, 12);
        iTextSharp.text.Font font2 = new iTextSharp.text.Font(bfChineseThin, 12);
        iTextSharp.text.Font fontsmall = new iTextSharp.text.Font(bfChineseThin, 11);
        font2.Color = Color.RED;
        #endregion

        #region 頁首欄
        //設定表格需要幾攔幾列 (如果不設列,可能會在使用RowSpan或ColSpan發生錯誤)
        iTextSharp.text.Table Tb = new iTextSharp.text.Table(6, 4);
        //設定表格的Padding
        Tb.WidthPercentage = 100;
        Tb.Padding = 0;
        Tb.Cellpadding = 3;

        //Tb.Spacing = 3;
        //自動填滿欄位(如果沒有填滿欄位,不會畫出欄位的線條)       
        Tb.AutoFillEmptyCells = true;
        //Tb.Width = 50;
        //抬頭
        Paragraph Title = new Paragraph("個人薪資單", Titlefont);
        //內容水平置中
        Title.SetAlignment("center");

        //插入圖片
        string imageUrl = path.Remove(path.LastIndexOf("\\")) + "\\Image\\LOGO.JPG";
        iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(new Uri(imageUrl));
        //設定圖檔縮放大小
        jpg.ScaleToFit(60f, 60f);
        jpg.SetAbsolutePosition(520, 800);
        Title.AddSpecial(jpg);

        //Cell內容
        iTextSharp.text.Cell Tc;
        Tc = new iTextSharp.text.Cell(new Phrase("公    司:", font));
        //內容水平置中
        Tc.HorizontalAlignment = Element.ALIGN_CENTER;
        //內容高度置中 (Top,Middle感覺不到有沒有移動)
        Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
        Tc.Rowspan = 1;
        Tc.Colspan = 1;
        // Tc.Bottom = Element.ALIGN_BOTTOM;

        //將Cell加入表格
        Tb.AddCell(Tc);

        Tc = new iTextSharp.text.Cell(new Phrase(sCompany, font));
        //內容水平置中
        Tc.HorizontalAlignment = Element.ALIGN_LEFT;
        //內容高度置中(Top,Middle感覺不到有沒有移動)
        Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
        Tc.Rowspan = 1;
        Tc.Colspan = 5;

        Tb.AddCell(Tc);

        Tc = new iTextSharp.text.Cell(new Phrase("部    門:", font));
        //內容水平置中
        Tc.HorizontalAlignment = Element.ALIGN_CENTER;
        //內容高度置中 (Top,Middle感覺不到有沒有移動)
        Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
        Tc.Rowspan = 1;
        Tc.Colspan = 1;
        // Tc.Bottom = Element.ALIGN_BOTTOM;

        //將Cell加入表格
        Tb.AddCell(Tc);

        Tc = new iTextSharp.text.Cell(new Phrase(sDepartment, font));
        //內容水平置中
        Tc.HorizontalAlignment = Element.ALIGN_LEFT;
        //內容高度置中(Top,Middle感覺不到有沒有移動)
        Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
        Tc.Rowspan = 1;
        Tc.Colspan = 5;

        Tb.AddCell(Tc);

        Tc = new iTextSharp.text.Cell(new Phrase("計薪年月:", font));
        //內容水平置中
        Tc.HorizontalAlignment = Element.ALIGN_CENTER;
        //內容高度置中 (Top,Middle感覺不到有沒有移動)
        Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
        Tc.Rowspan = 1;
        Tc.Colspan = 1;

        //將Cell加入表格
        Tb.AddCell(Tc);
        //抓出正確年月
        sSYM = PayrollLt.SalaryYM.ToString().Trim();
        sSYM = (int.Parse(sSYM.Substring(0, 4)) - 1911).ToString() + "年" + sSYM.Substring(4, 2) + "月";
        Tc = new iTextSharp.text.Cell(new Phrase(sSYM, font));
        //內容水平置中
        Tc.HorizontalAlignment = Element.ALIGN_LEFT;
        //內容高度置中(Top,Middle感覺不到有沒有移動)
        Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
        Tc.Rowspan = 1;
        Tc.Colspan = 5;

        Tb.AddCell(Tc);

        #region 每月只發放一次的公司,不顯示期別
        int iPeriodHight = 0;
        try
        {
            string[] PeriodList = _UserInfo.SysSet.GetConfigString("SalaryDate").Split(new char[] { ',' });
            if (PeriodList.Length > 1)
            {
                iPeriodHight = 30;
                Tc = new iTextSharp.text.Cell(new Phrase("期　　別:", font));
                //內容水平置中
                Tc.HorizontalAlignment = Element.ALIGN_CENTER;
                //內容高度置中 (Top,Middle感覺不到有沒有移動)
                Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
                Tc.Rowspan = 1;
                Tc.Colspan = 1;

                //將Cell加入表格
                Tb.AddCell(Tc);
                Tc = new iTextSharp.text.Cell(new Phrase("25日", font));
                //內容水平置中
                Tc.HorizontalAlignment = Element.ALIGN_LEFT;
                //內容高度置中(Top,Middle感覺不到有沒有移動)
                Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
                Tc.Rowspan = 1;
                Tc.Colspan = 5;

                Tb.AddCell(Tc);
            }
        }
        catch { }
        #endregion

        Tc = new iTextSharp.text.Cell(new Phrase("員　　工:", font));
        //內容水平置中
        Tc.HorizontalAlignment = Element.ALIGN_CENTER;
        //內容高度置中 (Top,Middle感覺不到有沒有移動)
        Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
        Tc.Rowspan = 1;
        Tc.Colspan = 1;

        //將Cell加入表格
        Tb.AddCell(Tc);
        Tc = new iTextSharp.text.Cell(new Phrase(sEmployeeId, font));
        //內容水平置中
        Tc.HorizontalAlignment = Element.ALIGN_LEFT;
        //內容高度置中(Top,Middle感覺不到有沒有移動)
        Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
        Tc.Rowspan = 1;
        Tc.Colspan = 5;

        Tb.AddCell(Tc);

        //加入抬頭
        doc.Add(Title);
        //把表格加入文件
        doc.Add(Tb);
        #endregion

        #region 切割線
        Graphic gp1 = new Graphic();
        gp1.MoveTo(35, 618 - iPeriodHight);
        gp1.LineTo(560, 618 - iPeriodHight);
        gp1.Stroke();
        doc.Add(gp1);
        #endregion

        #region 調薪欄
        int AdjustSalaryHight1 = 0;
        int AdjustSalaryHight2 = 0;
        DataTable dt = GetAdjustSalary();
        if (dt != null && dt.Rows.Count > 0)
        {
            //設定表格需要幾攔幾列 (如果不設列,可能會在使用RowSpan或ColSpan發生錯誤)
            iTextSharp.text.Table TAS = new iTextSharp.text.Table(5, dt.Rows.Count);
            //設定表格的Padding
            TAS.WidthPercentage = 100;
            TAS.Padding = 0;
            TAS.Cellpadding = 3;

            //Tb.Spacing = 3;
            //自動填滿欄位(如果沒有填滿欄位,不會畫出欄位的線條)       
            TAS.AutoFillEmptyCells = true;
            //Tb.Width = 50;

            //設定標題列
            string[] strASTitle = { "調薪項目", "調薪原因", "調薪前金額", "調薪後金額", "調薪說明" };
            for (int i = 0; i < 5; i++)
            {//調薪項目 調薪原因 調薪前金額 調薪後金額 調薪說明 
                #region //Cell內容
                Tc = new Cell();
                Tc = new iTextSharp.text.Cell(new Phrase(strASTitle[i], font));
                //內容水平置中
                Tc.HorizontalAlignment = Element.ALIGN_CENTER;
                //內容高度置中 (Top,Middle感覺不到有沒有移動)
                Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
                Tc.Rowspan = 1;
                Tc.Colspan = 1;
                // Tc.Bottom = Element.ALIGN_BOTTOM;

                //將Cell加入表格
                TAS.AddCell(Tc);
                #endregion
            }

            #region 設定調薪內容列
            string strValue = "";
            for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
            {
                for (int i = 0; i < 5; i++)
                {//調薪項目 調薪原因 調薪前金額 調薪後金額 調薪說明 
                    strValue = "";
                    try
                    {
                        if (dt.Rows[iRow][i] != null)
                        {
                            strValue = dt.Rows[iRow][i].ToString();
                            if (i == 2 || i == 3)
                            {
                                strValue = Convert.ToDecimal(strValue).ToString("N0");
                            }
                        }
                    }
                    catch { }
                    #region //Cell內容
                    Tc = new Cell();
                    Tc = new iTextSharp.text.Cell(new Phrase(strValue, font));
                    //內容水平置中
                    Tc.HorizontalAlignment = Element.ALIGN_CENTER;
                    //內容高度置中 (Top,Middle感覺不到有沒有移動)
                    Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
                    Tc.Rowspan = 1;
                    Tc.Colspan = 1;
                    // Tc.Bottom = Element.ALIGN_BOTTOM;

                    //將Cell加入表格
                    TAS.AddCell(Tc);
                    #endregion
                }
            }
            #endregion
            //把表格加入文件
            doc.Add(TAS);

            AdjustSalaryHight1 = (dt.Rows.Count + 1) * 30 + 15;
            AdjustSalaryHight2 = (dt.Rows.Count + 1) * 30 + 25;
        }
        #endregion

        #region 切割線
        Graphic gp = new Graphic();
        gp.MoveTo(35, 618 - AdjustSalaryHight1 - iPeriodHight);
        gp.LineTo(560, 618 - AdjustSalaryHight1 - iPeriodHight);
        gp.Stroke();
        doc.Add(gp);
        #endregion
        //準備明細要用的標題名稱
        string[] item1 = { "基本薪俸", "職務加給", "其他扣款", "伙食津貼", "其他應稅調整", "勞保費", "應稅加班費", "其他免稅調整", "健保費", "免稅加班費", "代扣所得稅", "福利金" };
        #region 上半
        iTextSharp.text.Table Tb2 = new iTextSharp.text.Table(6, 4);
        Tb2.WidthPercentage = 100;
        Tb2.Padding = 0;
        Tb2.Cellpadding = 3;
        //Tb.Spacing = 3;
        //自動填滿欄位(如果沒有填滿欄位,不會畫出欄位的線條)       
        Tb2.AutoFillEmptyCells = true;
        for (int i = 0; i < item1.Length; i++)
        {
            if (item1[i].ToString() != "")
            {
                Tc = new iTextSharp.text.Cell(new Phrase(item1[i].ToString(), font));

                //內容水平置中
                Tc.HorizontalAlignment = Element.ALIGN_CENTER;
                //內容高度置中 (Top,Middle感覺不到有沒有移動)
                Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
                //Tc.Border = 0;
                Tc.Rowspan = 1;
                Tc.Colspan = 1;
                //Tc.Bottom = 1;

                //將Cell加入表格
                Tb2.AddCell(Tc);
                string payamount = "";
                if ((i % 3) == 2 || item1[i].Contains("扣"))
                {
                    font2.Color = Color.RED;
                    if (FindPayroll(item1[i].ToString()) == 0)
                    {
                        payamount = "0";
                    }
                    else
                    {
                        payamount = string.Format("{0:0,0}", FindPayroll(item1[i].ToString()));
                    }

                    Tc = new iTextSharp.text.Cell(new Phrase(payamount, font2));
                }
                else
                {
                    if (FindPayroll(item1[i].ToString()) == 0)
                    {
                        payamount = "0";
                    }
                    else
                    {
                        payamount = string.Format("{0:0,0}", FindPayroll(item1[i].ToString()));
                    }
                    Tc = new iTextSharp.text.Cell(new Phrase(payamount, font));
                }
                //Tc2 = new iTextSharp.text.Cell(new Phrase(i.ToString(), font));
                //內容水平置中
                Tc.HorizontalAlignment = Element.ALIGN_RIGHT;
                //內容高度置中(Top,Middle感覺不到有沒有移動)
                Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
                Tc.Rowspan = 1;
                Tc.Colspan = 1;

                Tb2.AddCell(Tc);
            }
        }
        doc.Add(Tb2);
        #endregion
        #region 切割線
        Graphic gp2 = new Graphic();
        gp2.MoveTo(35, 488 - AdjustSalaryHight2 - iPeriodHight);
        gp2.LineTo(560, 488 - AdjustSalaryHight2 - iPeriodHight);
        gp2.Stroke();
        doc.Add(gp2);
        #endregion
        #region 下半
        iTextSharp.text.Table Tb3 = new iTextSharp.text.Table(6, 5);
        Tb3.WidthPercentage = 100;
        Tb3.Padding = 0;
        Tb3.Cellpadding = 3;
        //Tb.Spacing = 3;
        //自動填滿欄位(如果沒有填滿欄位,不會畫出欄位的線條)       
        Tb3.AutoFillEmptyCells = true;
        string[] item2 = { "固定薪資", "應稅加班時數", "事假時數", "非固定金額", "免稅加班時數", "傷病假時數", "免稅所得", "值班時數", "生理假時數", "應扣金額", "", "家庭照顧假時數", "應發金額", "", "" };
        for (int i = 0; i < item2.Length; i++)
        {
            if (item2[i].ToString() != "")
            {
                if (item2[i].ToString() == "家庭照顧假時數")
                    Tc = new iTextSharp.text.Cell(new Phrase(item2[i].ToString(), fontsmall));
                else
                    Tc = new iTextSharp.text.Cell(new Phrase(item2[i].ToString().Replace("扣薪", ""), font));

                //內容水平置中
                Tc.HorizontalAlignment = Element.ALIGN_CENTER;
                //內容高度置中 (Top,Middle感覺不到有沒有移動)
                Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
                Tc.Rowspan = 1;
                Tc.Colspan = 1;
                //將Cell加入表格
                Tb3.AddCell(Tc);
                string Amount = "";
                int iAmount = 0;

                if (item2[i].ToString().Contains("假時數"))
                {
                    Amount = GetLeaveofabsence(item2[i].ToString().Trim());
                    Amount = Amount.Replace(".0", "");
                }
                else if (item2[i].ToString().Contains("時數"))
                {
                    Amount = BindSalaryData(item2[i].ToString().Trim());
                }
                else
                {
                    iAmount = payrollcal(item2[i].ToString().Trim());
                    if (iAmount == 0)
                    {
                        Amount = "0";
                    }
                    else
                    {
                        Amount = string.Format("{0:0,0}", Math.Abs(iAmount));
                    }
                }

                if (item2[i].ToString().Contains("扣") || item2[i].ToString().Contains("假時數") || iAmount < 0)
                {
                    font2.Color = Color.RED;
                    Tc = new iTextSharp.text.Cell(new Phrase(Amount, font2));
                }
                else
                {

                    Tc = new iTextSharp.text.Cell(new Phrase(Amount, font));
                }
                //內容水平置中
                Tc.HorizontalAlignment = Element.ALIGN_RIGHT;
                //內容高度置中(Top,Middle感覺不到有沒有移動)
                Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
                Tc.Rowspan = 1;
                Tc.Colspan = 1;
                Tb3.AddCell(Tc);
            }
            else
            {
                Tc = new iTextSharp.text.Cell(new Phrase(" ", font));
                //Tc.Border = 1;
                Tc.Rowspan = 1;
                Tc.Colspan = 1;
                Tb3.AddCell(Tc);
                Tc = new iTextSharp.text.Cell();
                Tc.Border = 1;
                Tc.Rowspan = 1;
                Tc.Colspan = 1;
                Tb3.AddCell(Tc);
            }
        }
        doc.Add(Tb3);
        #endregion

        #region 切割線
        Graphic gp4 = new Graphic();
        gp4.MoveTo(35, 318 - AdjustSalaryHight2 - iPeriodHight);
        gp4.LineTo(560, 318 - AdjustSalaryHight2 - iPeriodHight);
        gp4.Stroke();
        doc.Add(gp4);
        #endregion

        #region 退休金欄
        int PensionaccountHight1 = 0;
        int PensionaccountHight2 = 0;
        dt = GetPensionaccount();
        if (dt != null && dt.Rows.Count > 0)
        {
            //設定表格需要幾攔幾列 (如果不設列,可能會在使用RowSpan或ColSpan發生錯誤)
            iTextSharp.text.Table TAS = new iTextSharp.text.Table(4, dt.Rows.Count);
            //設定表格的Padding
            TAS.WidthPercentage = 100;
            TAS.Padding = 0;
            TAS.Cellpadding = 3;

            //Tb.Spacing = 3;
            //自動填滿欄位(如果沒有填滿欄位,不會畫出欄位的線條)       
            TAS.AutoFillEmptyCells = true;
            //Tb.Width = 50;

            //設定標題列
            string[] strASTitle = { "當月企提金額", "企提累計金額", "當月自提金額", "自提累計金額" };
            for (int i = 0; i < 4; i++)
            {//調薪項目 調薪原因 調薪前金額 調薪後金額 調薪說明 
                #region //Cell內容
                Tc = new Cell();
                Tc = new iTextSharp.text.Cell(new Phrase(strASTitle[i], font));
                //內容水平置中
                Tc.HorizontalAlignment = Element.ALIGN_CENTER;
                //內容高度置中 (Top,Middle感覺不到有沒有移動)
                Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
                Tc.Rowspan = 1;
                Tc.Colspan = 1;
                // Tc.Bottom = Element.ALIGN_BOTTOM;

                //將Cell加入表格
                TAS.AddCell(Tc);
                #endregion
            }

            #region 設定退休金內容列
            string strValue = "";
            for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
            {
                for (int i = 0; i < 4; i++)
                {//"企提金額", "自提金額", "企提累計金額", "自提累計金額" 
                    strValue = "";
                    try
                    {
                        if (dt.Rows[iRow][i] != null)
                        {
                            strValue = dt.Rows[iRow][i].ToString();
                            //if (i == 2 || i == 3)
                            {
                                strValue = Convert.ToDecimal(strValue).ToString("N0");
                            }
                        }
                    }
                    catch { }
                    #region //Cell內容
                    Tc = new Cell();
                    Tc = new iTextSharp.text.Cell(new Phrase(strValue, font));
                    //內容水平置中
                    Tc.HorizontalAlignment = Element.ALIGN_CENTER;
                    //內容高度置中 (Top,Middle感覺不到有沒有移動)
                    Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
                    Tc.Rowspan = 1;
                    Tc.Colspan = 1;
                    // Tc.Bottom = Element.ALIGN_BOTTOM;

                    //將Cell加入表格
                    TAS.AddCell(Tc);
                    #endregion
                }
            }
            #endregion
            //把表格加入文件
            doc.Add(TAS);

            PensionaccountHight1 = (dt.Rows.Count + 1) * 30 + 15;
            PensionaccountHight2 = (dt.Rows.Count + 1) * 30 + 25;
        }
        #endregion

        //頁尾
        Paragraph PageEnd = new Paragraph("公司為密薪制;請勿違反規定，談論薪資獎金。", font2);
        //內容水平置中
        PageEnd.SetAlignment("left");
        doc.Add(PageEnd);

        #region 目前沒用到的功能
        //Graphic gp2 = new Graphic();
        //gp2.MoveTo(35, 528);
        //gp2.LineTo(560, 528);
        //gp2.Stroke();
        //doc.Add(gp2);
        //doc.Add(Tb2);
        //開啟新的一頁
        //doc.NewPage();

        ////加入抬頭
        //doc.Add(Title);
        ////把表格加入文件
        //doc.Add(Tb);

        //PDF讀檔寫入另外一個PDF
        //PdfContentByte cb = writer.DirectContent;
        //PdfImportedPage newPage;
        //int iPageNum = reader.NumberOfPages;
        //for (int j = 1; j <= iPageNum; j++)
        //{
        //    doc.NewPage();
        //    newPage = writer.GetImportedPage(reader, j);
        //    cb.AddTemplate(newPage, 0, 0);
        //}
        #endregion

        return doc;
    }

    protected void ChangPassWord_Click(object sender, EventArgs e)
    {
        Changdata(1);
    }

    private int FindPayroll(string item)
    {
        Decimal Amount = 0;
        string[] salaryitem = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "20", "21", "22", "23" };
        switch (item)
        {
            case "基本薪俸":
                Amount = Convert.ToDecimal(thePayroll.GetPersonalSalaryItem(PayrollLt, salaryitem[0]));
                break;
            case "伙食津貼":
                Amount = Convert.ToDecimal(thePayroll.GetPersonalSalaryItem(PayrollLt, salaryitem[1]));
                break;
            case "代扣所得稅":
                Amount = Convert.ToDecimal(thePayroll.GetPersonalSalaryItem(PayrollLt, salaryitem[2]));
                break;
            case "健保費":
                Amount = Convert.ToDecimal(thePayroll.GetPersonalSalaryItem(PayrollLt, salaryitem[3]));
                break;
            case "勞保費":
                Amount = Convert.ToDecimal(thePayroll.GetPersonalSalaryItem(PayrollLt, salaryitem[4]));
                break;
            case "退休金自提金額":
                Amount = Convert.ToDecimal(thePayroll.GetPersonalSalaryItem(PayrollLt, salaryitem[5]));
                break;
            case "福利金":
                Amount = Convert.ToDecimal(thePayroll.GetPersonalSalaryItem(PayrollLt, salaryitem[6]));
                break;
            case "應稅加班費":
                Amount = Convert.ToDecimal(thePayroll.GetPersonalSalaryItem(PayrollLt, salaryitem[7]));
                break;
            case "免稅加班費":
                Amount = Convert.ToDecimal(thePayroll.GetPersonalSalaryItem(PayrollLt, salaryitem[8]));
                break;
            case "職務加給":
                Amount = Convert.ToDecimal(thePayroll.GetPersonalSalaryItem(PayrollLt, salaryitem[9]));
                break;
            case "其他扣款":
                Amount = Convert.ToDecimal(thePayroll.GetPersonalSalaryItem(PayrollLt, salaryitem[10]));
                break;
            case "其他應稅調整":
                Amount = Convert.ToDecimal(thePayroll.GetPersonalSalaryItem(PayrollLt, salaryitem[11]));
                break;
            case "其他免稅調整":
                Amount = Convert.ToDecimal(thePayroll.GetPersonalSalaryItem(PayrollLt, salaryitem[12]));
                break;
        }
        return Convert.ToInt32(Amount);
    }
    private int payrollcal(string item)
    {
        int calAmount = 0;
        switch (item)
        {
            case "固定薪資":
                //固定金額
                calAmount = Convert.ToInt32(dFixedSalary);
                break;
            case "非固定金額":
                calAmount = Convert.ToInt32(dNonFixedAmount);
                break;
            case "免稅所得":
                calAmount = Convert.ToInt32(dOtherNT);
                break;
            case "應扣金額":
                calAmount = Convert.ToInt32(dMAmount);
                break;
            case "應發金額":
                calAmount = Convert.ToInt32((dFixedSalary + dOtherWT + dOtherNT - dMAmount));
                break;
            default:
                break;
        }
        return calAmount;
    }
    private decimal GetMAmount()
    {
        decimal dMAmount = 0;
        try
        {//所得稅
            dMAmount += Convert.ToDecimal(FindPayroll("代扣所得稅"));
        }
        catch { }
        try
        {//健保費
            dMAmount += Convert.ToDecimal(FindPayroll("健保費"));
        }
        catch { }
        try
        {//勞保費
            dMAmount += Convert.ToDecimal(FindPayroll("勞保費"));
        }
        catch { }
        try
        {//福利金
            dMAmount += Convert.ToDecimal(FindPayroll("福利金"));
        }
        catch { }
        try
        {//其他扣款
            dMAmount += Convert.ToDecimal(FindPayroll("其他扣款"));
        }
        catch { }
        return dMAmount;
    }

    private DataTable GetAdjustSalary()
    {
        DataTable dt = null;

        Ssql = " SELECT IsNull((select SalaryId+'-'+SalaryName From SalaryStructure_Parameter where SalaryId=AdjustSalaryItem), [AdjustSalaryItem]) as AdjustSalaryItem " +
            " ,IsNull((SELECT [CodeName] FROM [CodeDesc] where [CodeID]= 'PY#AdjCode' and [CodeCode]=[AdjustSalaryReasonCode]),'') as AdjustSalaryReasonCode " +
            " ," + PayrollLt.DeCodeKey + "([OldlSalary]) as OldlSalary," + PayrollLt.DeCodeKey + "([NewSalary]) as NewSalary " +
            " ,[AdjustSalaryReason],EffectiveDate,ApproveDate " +
            " FROM [AdjustSalary_Master] " +
            " Where Company='" + PayrollLt.Company.Trim() + "' and [AdjustSalaryItem] != '00' ";
        //計薪年月=生效年月
        Ssql += string.Format(" And Substring(Convert(varchar,EffectiveDate,112),1,6) = '{0}'", PayrollLt.SalaryYM.ToString());
        //員工帳號
        Ssql += string.Format(" And EmployeeId = '{0}'", PayrollLt.EmployeeId.Trim());

        DataTable theDT = _MyDBM.ExecuteDataTable(Ssql + " Order By Company,EmployeeId");

        if (theDT != null)
        {
            if (theDT.Rows.Count > 0)
            {
                dt = theDT;
            }
        }
        return dt;
    }

    private bool CheckSalaryYM()
    {
        try
        {
            Ssql = "SELECT count(*) " +
                " FROM Payroll_History_Heading " +
                " Where Company='" + PayrollLt.Company.Trim() + "' ";
            //計薪年月=生效年月
            Ssql += string.Format(" And SalaryYM = '{0}'", PayrollLt.SalaryYM.ToString());
            //員工帳號
            Ssql += string.Format(" And EmployeeId = '{0}'", PayrollLt.EmployeeId.Trim());

            DataTable theDT = _MyDBM.ExecuteDataTable(Ssql);

            if (theDT != null)
            {
                if (theDT.Rows.Count > 0 && (int)theDT.Rows[0][0] > 0)
                {
                    return true;
                }
            }
        }
        catch { }
        return false;
    }

    private DataTable GetPensionaccount()
    {
        DataTable dt = null;

        Ssql = " Sum(" + PayrollLt.DeCodeKey + "(ActualAmount_C))  ";

        Ssql = "SELECT Sum(Case when salary_month = '" + PayrollLt.SalaryYM.ToString() + "' then " + PayrollLt.DeCodeKey + "(ActualAmount_C) else 0 end) as YMC" +
            " , Sum(" + PayrollLt.DeCodeKey + "(ActualAmount_C)) as TotalC " +
            " , Sum(Case when salary_month = '" + PayrollLt.SalaryYM.ToString() + "' then " + PayrollLt.DeCodeKey + "(ActualAmount_S) else 0 end) as YMS " +
            " , Sum(" + PayrollLt.DeCodeKey + "(ActualAmount_S)) as TotalS " +
            " FROM Pensionaccounts_Detail " +
            " Where Company='" + PayrollLt.Company.Trim() + "' ";
        //計薪年月=生效年月
        Ssql += string.Format(" And salary_month <= '{0}'", PayrollLt.SalaryYM.ToString());
        //員工帳號
        Ssql += string.Format(" And EmployeeId = '{0}'", PayrollLt.EmployeeId.Trim());

        DataTable theDT = _MyDBM.ExecuteDataTable(Ssql);

        if (theDT != null)
        {
            if (theDT.Rows.Count > 0)
            {
                dt = theDT;
            }
        }
        return dt;
    }
    
    private string BindSalaryData(string item)
    {
        string strColumnName;

        Ssql = "SELECT [Company],[EmployeeId],[SalaryYM],[PeriodCode],[Paydays],[LeaveHours_deduction],[TaxRate],[ResignCode]" +
            ",[HI_Person],[WT_Overtime],[NT_Overtime],[OnWatch],[Dependent1_IDNo],[Dependent2_IDNo],[Dependent3_IDNo]" +
            "," + PayrollLt.DeCodeKey + "([BaseSalary]) As BaseSalary" +
            "," + PayrollLt.DeCodeKey + "([LI_Fee]) As LI_Fee " +
            "," + PayrollLt.DeCodeKey + "([HI_Fee]) As HI_Fee " +
            "," + PayrollLt.DeCodeKey + "([NT_P]) As NT_P" +
            "," + PayrollLt.DeCodeKey + "([WT_P_Salary])As WT_P_Salary" +
            "," + PayrollLt.DeCodeKey + "([NT_M]) As NT_M " +
            "," + PayrollLt.DeCodeKey + "([WT_P_Bonus]) As WT_P_Bonus" +
            "," + PayrollLt.DeCodeKey + "([WT_M_Salary]) As WT_M_Salary" +
            "," + PayrollLt.DeCodeKey + "([WT_M_Bonus]) As WT_M_Bonus" +
            "," + PayrollLt.DeCodeKey + "([P1_borrowing]) As P1_borrowing" +
            "," + PayrollLt.DeCodeKey + "([WT_Overtime_Fee]) As WT_Overtime_Fee" +
            "," + PayrollLt.DeCodeKey + "([NT_Overtime_Fee]) As NT_Overtime_Fee" +
            "," + PayrollLt.DeCodeKey + "([OnWatch_Fee]) As OnWatch_Fee" +
            "," + PayrollLt.DeCodeKey + "([Dependent1_HI_Fee]) As Dependent1_HI_Fee" +
            "," + PayrollLt.DeCodeKey + "([Dependent2_HI_Fee]) As Dependent2_HI_Fee" +
            "," + PayrollLt.DeCodeKey + "([Dependent3_HI_Fee]) As Dependent3_HI_Fee" +
            "  FROM " + (sKind.Equals("W") ? PayrollTable : "Payroll_History") + "_Heading Where Company='" + PayrollLt.Company.Trim() + "'";
        //計薪年月
        Ssql += string.Format(" And SalaryYM = {0}", PayrollLt.SalaryYM.ToString());
        //計薪期別
        Ssql += string.Format(" And PeriodCode = '{0}'", PayrollLt.PeriodCode.Trim());
        //員工帳號
        Ssql += string.Format(" And EmployeeId = '{0}'", PayrollLt.EmployeeId.Trim());

        DataTable theDT = _MyDBM.ExecuteDataTable(Ssql + " Order By Company,EmployeeId");

        if (theDT != null)
        {
            if (theDT.Rows.Count > 0)
            {
                for (int i = 0; i < theDT.Columns.Count; i++)
                {
                    //strTemp = "N0";
                    strColumnName = theDT.Columns[i].ColumnName.Trim();
                    //tempTitleLab = (Label)this.Form.FindControl("lblTitle_" + strColumnName);
                    if (strColumnName != null)
                    {
                        Ssql = "Select dbo.GetColumnTitle('" + (sKind.Equals("W") ? PayrollTable : "Payroll_History") + "_Heading','" + strColumnName.Trim() + "')";
                        DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
                        if (DT.Rows.Count > 0)
                        {//欄位標題名稱
                            //tempTitleLab.Text = DT.Rows[0][0].ToString().Trim();
                            if (DT.Rows[0][0].ToString().Contains(item))
                            {
                                return theDT.Rows[0][strColumnName].ToString();
                            }
                        }
                    }
                }
            }
        }
        return "0.0";
    }

    private bool hasdata(string ID)
    {
        if (SearchList1.CompanyValue.Replace("%", "").Trim() == "")
        {
            lbl_Msg.Text = "請選擇公司";
            return false;
        }
        else if (ID.Replace("%", "").Trim() == "" && OldId.Text.Trim() == "")
        {
            lbl_Msg.Text = "請選擇員工或輸入員工代號";
            return false;
        }

        Ssql = "SELECT * FROM " + (sKind.Equals("W") ? PayrollTable : "Payroll_History") + "_Heading Where Company='" + PayrollLt.Company.Trim() + "'";
        //計薪年月
        Ssql += string.Format(" And SalaryYM Between {0} and {1}", SalaryYM1.SelectSalaryYM, SalaryYM2.SelectSalaryYM);
        //計薪期別
        Ssql += string.Format(" And PeriodCode = '{0}'", PayrollLt.PeriodCode.Trim());
        //員工帳號
        Ssql += string.Format(" And EmployeeId = '{0}'", PayrollLt.EmployeeId.Trim());

        DataTable theDT = _MyDBM.ExecuteDataTable(Ssql + " Order By Company,EmployeeId");

        if (theDT != null)
        {
            if (theDT.Rows.Count <= 0)
            {
                lbl_Msg.Text = "此計薪年月尚無薪資單";
                return false;
            }
        }
        else
        {
            lbl_Msg.Text = "此計薪年月尚無薪資單";
            return false;
        }

        return true;
    }

    private bool checkResign(string ID)
    {
        Ssql = "SELECT Count(*) FROM [Personnel_Master] " +
            " where [HireDate]<=IsNull([ResignDate],dateadd(d,-1,[HireDate])) And Company='" + PayrollLt.Company.Trim() + "'";
        //員工帳號
        Ssql += string.Format(" And EmployeeId = '{0}'", ID.Trim());

        DataTable theDT = _MyDBM.ExecuteDataTable(Ssql);

        if (theDT != null)
        {
            if (theDT.Rows[0][0].ToString().Equals("0"))
            {
                //lbl_Msg.Text = "未離職";
                return false;
            }
        }
        else
        {
            //lbl_Msg.Text = "資料有誤";
            return false;
        }

        return true;
    }

    private Boolean Changdata(int a)
    {
        DateTime StartDateTime = DateTime.Now;
        DataTable CheckData;
        DataTable DataCheck;
        String SQL;
        String NerPwd;
        String Sql;
        bool blCheck = true;

        lbl_Msg.Text = "";
        //若選單與輸入ID未輸入時,使資料一致
        if (blCheckBatch == false)
        {
            if (SearchList1.EmployeeValue.Replace("%", "").Trim() != "" && OldId.Text.Trim() == "")
            {
                OldId.Text = SearchList1.EmployeeValue.Replace("%", "");
            }
            else if (SearchList1.EmployeeValue.Replace("%", "").Trim() == "" && OldId.Text.Trim() != "")
            {
                SearchList1.EmployeeValue = OldId.Text;
            }
        }

        #region 檢核輸入資料
        if (SearchList1.EmployeeValue.Replace("%", "").Trim() == "" && OldId.Text.Trim() == "")
        {
            lbl_Msg.Text = "請輸入帳號或選擇員工 !!";
            blCheck = false;
        }
        else if (OldPassword.Text.Trim() == "")
        {
            lbl_Msg.Text = "PDF下載密碼尚未輸入,請確認!!";
            blCheck = false;
        }
        else if (Chk_PDFDown(OldPassword.Text) != true && (a != 1))
        {//當下載密碼等於身分證字號
            if (lbl_Msg.Text == "")
                lbl_Msg.Text = "PDF下載密碼不可是身分證字號,請先更新密碼 !!";
            blCheck = false;
        }
        else if (a != 2)
        {
            if (SearchList1.EmployeeValue.Replace("%", "").Trim() == "")
            {
                lbl_Msg.Text = "變更密碼需單一設定!請先指定待變更密碼之員工!!";
                blCheck = false;
            }
            if (this.NewPassword.Text.Trim() == "")
            {
                lbl_Msg.Text = "PDF密碼變更尚未輸入新密碼,請確認!!";
                blCheck = false;
            }
            else if (this.NewPassword.Text.Trim() == this.OldPassword.Text.Trim())
            {//檢核舊密碼與新密碼
                lbl_Msg.Text = "PDF新密碼與原PDF下載密碼不可相同!!請重新輸入PDF新密碼....";
                NewPassword.Text = "";
                blCheck = false;
            }
            else if (this.NewPassword.Text.Trim() != this.NewPassWord2.Text.Trim())
            {//檢核新密碼一致性問題
                lbl_Msg.Text = "PDF新密碼與確認密碼不同!!請確認....!!";
                blCheck = false;
            }
            else if (Chk_PDFDown(NewPassword.Text) != true)
            {//當新密碼等於身分證字號
                if (lbl_Msg.Text == "")
                    lbl_Msg.Text = "PDF密碼變更不可是身分證字號,請更換新密碼 !!";
                blCheck = false;
            }
        }
        #endregion

        if (blCheck != true)
        {
            outputMsg = lbl_Msg.Text;
            return blCheck;
        }
        else
        {
            sEmployeeId = SearchList1.EmployeeValue.Replace("%", "").Trim();
            if (blCheckBatch != false && OldId.Text.Trim().Length > 0)
            {//找出系統登入帳號之密碼
                Sql = "SELECT (SELECT Distinct [Company] from [PersonnelSecurity] where [CompanyCode]=U.[CompanyCode]) as [Company] " +
                " ,[UserId] as ERPID,IsNull(EmployeeId,'') as [EmployeeId],Cast([Password] as varchar) AS PayRollPW " +
                " FROM [UC_User] U Left Join (select * from [PersonnelSecurity] where Company='" + SearchList1.CompanyValue.Trim() + "') PS " +
                " On U.[CompanyCode]=PS.[CompanyCode] And U.[UserId]=PS.[ERPID] " +
                " Where [SiteId]='ePayroll' and [Enable]=1 " +
                " And UserId='" + OldId.Text.Trim() + "' ";

                lbl_Msg.Text = "帳號[" + OldId.Text.Trim() + "]已凍結或不存在!";
            }
            else
            {//找出薪資下載密碼
                Sql = "Select A.Company, Rtrim(A.ERPID) as ERPID, Rtrim(A.Employeeid) AS Employeeid,IsNull(Cast(A.PayRollPW as varchar),'') AS PayRollPW,";
                Sql += " C.EmployeeName,C.IdNo,C.DeptId  From PersonnelSecurity A,Personnel_Master C  ";
                Sql += " Where C.Company='" + SearchList1.CompanyValue.Trim() + "' And C.EmployeeId='" + sEmployeeId.ToUpper() + "' AND A.Company=C.Company And A.EmployeeId=C.EmployeeId ";
            }

            CheckData = _MyDBM.ExecuteDataTable(Sql);
            if (CheckData.Rows.Count >= 1)
            {
                lbl_Msg.Text = "";
                sEmployeeId = CheckData.Rows[0]["Employeeid"].ToString();
                if (CheckData.Rows[0]["PayRollPW"].ToString() == "")
                {
                    if (OldPassword.Text.Trim() != CheckData.Rows[0]["IdNo"].ToString().Trim())
                    {
                        lbl_Msg.Text = "PDF下載密碼輸入錯誤,請重新輸入!!";
                        outputMsg = lbl_Msg.Text;
                        return false;
                    }
                }
                else if (OldPassword.Text.Trim() != _UserInfo.SysSet.rtnTrans(CheckData.Rows[0]["PayRollPW"].ToString().Trim()))
                {
                    lbl_Msg.Text = "PDF下載密碼輸入錯誤,請重新輸入!!";
                    outputMsg = lbl_Msg.Text;
                    return false;
                }
                else if (blCheckBatch != false && SearchList1.DepartmentValue.Replace("%", "").Trim() != "" && SearchList1.EmployeeText.Contains("無資料"))
                {//檢查部門內是否有員工資料
                    lbl_Msg.Text = SearchList1.DepartmentText + "無員工資料!請重新指定!!";
                    outputMsg = lbl_Msg.Text;
                    return false;
                }
            }
            else if (blCheckBatch != false || sEmployeeId == "")
            {
                outputMsg = lbl_Msg.Text;
                return false;
            }

            NerPwd = OldPassword.Text;

            if (a != 2 && (sEmployeeId != "" || (blCheckBatch != false && OldId.Text.Trim().Length > 0)))
            {//a=1為變更密碼
                NerPwd = this.NewPassWord2.Text;

                //判斷是否為新增或更新
                SQL = "Select Count(*) Count1 From PersonnelSecurity ";
                if (blCheckBatch != false && OldId.Text.Trim().Length > 0)
                    Ssql = " Where ERPID='" + OldId.Text.Trim() + "'";
                else
                    Ssql = " Where Company='" + SearchList1.CompanyValue.Trim() + "' And EmployeeId='" + sEmployeeId + "'";

                SQL += Ssql;
                DataCheck = _MyDBM.ExecuteDataTable(SQL);

                //寫入Log
                MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = PDFKind;
                MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "薪資單下載密碼";
                MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "來源IP:" + Request.UserHostAddress;
                MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
                //此時不設定異動結束時間
                //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
                MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = OldId.Text.Trim();

                try
                {
                    if (Convert.ToInt32(DataCheck.Rows[0]["Count1"].ToString()) > 1)
                    {
                        lbl_Msg.Text = "帳號重覆!PDF密碼無法在此修改";
                    }
                    else if (Convert.ToInt32(DataCheck.Rows[0]["Count1"].ToString()) == 1)
                    {
                        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "U";
                        _MyDBM.DataChgLog(MyCmd.Parameters);
                        //更新密碼 
                        if (this.NewPassword.Text != "")
                        {
                            SQL = "UPdate PersonnelSecurity Set PayRollPW=CAST('" + _UserInfo.SysSet.rtnHash(NerPwd) + "'AS varbinary) " + Ssql;
                            _MyDBM.ExecuteDataTable(SQL);
                        }
                        lbl_Msg.Text = "PDF密碼已更新完成 !!";
                    }
                    else
                    {
                        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "A";
                        _MyDBM.DataChgLog(MyCmd.Parameters);
                        //將資料新增至 PersonnelSecurity
                        SQL = "INSERT INTO PersonnelSecurity(Company,EmployeeId,CompanyCode,ERPID,PayRollPW) " +
                            " VALUES('" + SearchList1.CompanyValue.Trim() + "','" + sEmployeeId + "','','', CAST('" + _UserInfo.SysSet.rtnHash(NerPwd) + "' as varbinary))";
                        _MyDBM.ExecuteDataTable(SQL);

                        lbl_Msg.Text = "已將您的PDF密碼資料新增完成 !!";
                    }
                }
                catch (Exception ex)
                {
                    blCheck = false;
                    lbl_Msg.Text = "PDF密碼變更失敗!" + ex.Message;
                }
                outputMsg = lbl_Msg.Text;
                MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
                _MyDBM.DataChgLog(MyCmd.Parameters);
            }
        }
        return blCheck;
    }

    /// <summary>
    /// 判斷PDF下載密碼是否為身分證字號(一般使用者才要檢核)
    /// </summary>
    private bool Chk_PDFDown(string thePassword)
    {
        //判斷是否密碼是否為身分證字號
        bool blCheck = true;
        DataTable DownPDf;
        String Sql = "";

        if (blCheckBatch == false)
        {
            if (OldId.Text.Trim().Length == 0)
                OldId.Text = SearchList1.EmployeeValue.Replace("%", "").Trim();

            Sql = " Select Rtrim(C.Employeeid) as Employeeid ,C.IdNo,C.EmployeeName";
            Sql += " From Personnel_Master C ";
            Sql += " Where C.Company='" + SearchList1.CompanyValue.Trim() + "' ";
            Sql += "   And C.EmployeeId='" + OldId.Text + "' ";
            DownPDf = _MyDBM.ExecuteDataTable(Sql);

            if (DownPDf.Rows.Count == 0)
            {
                lbl_Msg.Text = "帳號不存在!!請重新輸入!";
                _UserInfo.SysSet.WriteToLogs(PDFKind, "帳號" + OldId.Text.Trim() + "不存在!|來源IP:" + Request.UserHostAddress);
                blCheck = false;
            }
            else if (DownPDf.Rows[0]["IdNo"].ToString() == thePassword)
            {//當身分證字號等於舊密碼
                lbl_Msg.Text = "密碼不可與身份証號相同!請先修改密碼!";
                blCheck = false;
            }

            sEmployeeId = OldId.Text;
            PayrollLt.EmployeeId = sEmployeeId;
        }
        return blCheck;
    }

    /// <summary>
    /// 檢核帳密是否正確或是否為身份証字號
    /// </summary>
    /// <returns></returns>
    private Boolean Chk_ID()
    {
        DataTable DownPDf;
        String Sql = "";
        lbl_Msg.Text = "";
        bool blCheck = true;
        string strQuery = "";
        strQuery = " Where A.Company='" + SearchList1.CompanyValue.Replace("%", "") + "'";

        if (OldPassword.Text != "")
        {
            if (Chk_PDFDown(OldPassword.Text) != false)
            {
                if (blCheckBatch == false)
                {
                    Sql = " Select Cast(A.PayRollPW as varchar)AS PayRollPW,ErrFrequency";
                    Sql += " From PersonnelSecurity A ";
                    Sql += " Left join Personnel_Master C On A.Company=C.Company And A.EmployeeId=C.EmployeeId ";
                    strQuery += " And A.EmployeeId='" + OldId.Text + "'";
                    Sql += strQuery;
                }
                else
                {
                    Sql = "SELECT [EmployeeId],Cast([Password] as varchar) AS PayRollPW, IsNull(ErrFrequency,0) as ErrFrequency " +
                        " FROM [UC_User] U Left Join (select * from [PersonnelSecurity] where Company='" + SearchList1.CompanyValue.Trim() + "') A " +
                        " On U.[CompanyCode]=A.[CompanyCode] And U.[UserId]=A.[ERPID] " +
                        " Where [SiteId]='ePayroll' and [Enable]=1 " +
                        " And UserId='" + OldId.Text.Trim() + "' ";
                    strQuery += " And A.ERPID='" + OldId.Text + "'";
                }

                DownPDf = _MyDBM.ExecuteDataTable(Sql);
                if (DownPDf.Rows.Count >= 1)
                {
                    int iErrFrequency = 0;
                    try
                    {
                        iErrFrequency = Convert.ToInt32(DownPDf.Rows[0]["ErrFrequency"].ToString());
                    }
                    catch { }

                    if (iErrFrequency >= 3)
                    {
                        lbl_Msg.Text = "PDF下載密碼已鎖定或錯誤已超過3次! 請先洽管理人員解鎖。";
                        blCheck = false;
                    }
                    else if (OldPassword.Text.Trim() != _UserInfo.SysSet.rtnTrans(DownPDf.Rows[0]["PayRollPW"].ToString()))
                    {
                        Sql = " Update PersonnelSecurity Set ErrFrequency=ErrFrequency+1 ,LastErrTime=GetDate() ";
                        Sql += strQuery.Replace("A.", "");

                        _MyDBM.ExecuteDataTable(Sql);
                        lbl_Msg.Text = "PDF下載密碼錯誤，請確認帳密是否有誤!!(密碼大小寫視為不同)";
                        blCheck = false;
                    }
                    else if (blCheckBatch == false)
                    {//個入下載帳密正確時,自動清空錯誤次數
                        Sql = " Update PersonnelSecurity Set ErrFrequency=0 ";
                        Sql += strQuery.Replace("A.", "");

                        _MyDBM.ExecuteDataTable(Sql);
                    }
                }
                else
                {
                    lbl_Msg.Text = "帳號不存在!!請重新輸入!";
                    //寫入Log
                    _UserInfo.SysSet.WriteToLogs(PDFKind, "帳號" + OldId.Text.Trim() + "不存在!|來源IP:" + Request.UserHostAddress);
                    blCheck = false;
                }
            }
            else
            {
                blCheck = false;
            }
        }
        return blCheck;
    }

    /// <summary>
    /// 發送電子郵件
    /// </summary>
    /// <param name="Subject">主旨</param>
    /// <param name="MailTo">寄送對象</param>
    /// <param name="EMailName">寄送對象名稱</param>
    /// <param name="file">附件</param>
    /// <param name="Context">內文</param>
    private void EMail(string Subject, string MailTo, string EMailName, string file, string Context)
    {
        if (string.IsNullOrEmpty(MailTo) || MailTo.Length == 0)
        {
            MailTo = _UserInfo.SysSet.GetConfigString("MailAdmin1");
        }

        if (MailTo.Length != 0)
        {
            MailMessage mail = new MailMessage();
            string MailfromName = "泛太服務信箱代發郵件", MailfromEmail = _UserInfo.SysSet.GetConfigString("MailDomainTW");
            if (!MailTo.Contains("@"))
            {
                Subject = "員工電子信箱有誤:" + MailTo;
                Context = "附件為該員工的" + SalaryYM1.SelectSalaryYMName + "薪資單";
                MailTo = MailfromEmail;
            }
            if (Subject.Length == 0)
                Subject = "您的" + SalaryYM1.SelectSalaryYMName + "薪資單";
            if (Context.Length == 0)
                Context = "附件為您的" + SalaryYM1.SelectSalaryYMName + "薪資單，密碼若未曾修改則請用預設值！(預設值請洽人事管理者)";
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(file);
            Attachment data = null;
            if (fileInfo.Exists)
            {
                //using System.Net.Mime;
                // Create  the file attachment for this e-mail message.
                data = new Attachment(file, MediaTypeNames.Application.Octet);
                #region 下面這段中文系統就不要加了,會引起MS的BUG,中文無法解析的問題
                //// Add time stamp information for the file.
                //ContentDisposition disposition = data.ContentDisposition;
                //disposition.CreationDate = System.IO.File.GetCreationTime(file);
                //disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                //disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
                #endregion
                // Add the file attachment to this e-mail message.
                mail.Attachments.Add(data);
            }
            else
            {
                Context = "請至指定網址查詢您的" + SalaryYM1.SelectSalaryYMName + "薪資單，密碼若未曾修改則請用預設值！(預設值請洽人事管理者)";
            }
            mail.From = new MailAddress(MailfromEmail, MailfromName);
            string[] MailToList = MailTo.Split(new char[] { ',', ';' });
            foreach (string theEMail in MailToList)
            {
                if (theEMail.Contains("@"))
                    mail.To.Add(new MailAddress(theEMail, EMailName));
            }
            mail.Subject = Subject;
            mail.Body = Context;
            mail.IsBodyHtml = false;
            mail.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient(_UserInfo.SysSet.GetConfigString("MailServer"));
            try
            {
                smtp.Send(mail);
                lbl_Msg.Text = "郵件發送完成!";
            }
            catch (Exception ex)
            {
                lbl_Msg.Text = "部份員工郵件發送失敗!";
                string strMsg = "員工[" + EMailName + "]發送失敗!" + ex.Message;
                _UserInfo.SysSet.WriteToLogs("下載薪資PDF", strMsg);
                _UserInfo.SysSet.WriteTofiles(path + "\\DL", "下載薪資PDF.log", strMsg);
            }
            data.Dispose();
        }
    }

    /// <summary>
    /// 取得請假時數明細(部分)
    /// </summary>
    /// <returns></returns>
    private string GetLeaveofabsence(string item)
    {
        Ssql = " Sum(" + PayrollLt.DeCodeKey + "(ActualAmount_C))  ";

        Ssql = @"SELECT L.[Company]
      ,[EmployeeId]
      ,[事假時數]=Sum(Case when [LeaveType_Id]='1' then [hours]+[days]*8 else 0 end)
      ,[傷病假時數]=Sum(Case when [LeaveType_Id]='2' then [hours]+[days]*8 else 0 end)
      ,[公傷假時數]=Sum(Case when [LeaveType_Id]='7' then [hours]+[days]*8 else 0 end)
      ,[生理假時數]=Sum(Case when [LeaveType_Id]='E' then [hours]+[days]*8 else 0 end)
      ,[家庭照顧假時數]=Sum(Case when [LeaveType_Id]='F' then [hours]+[days]*8 else 0 end)      
      FROM [Leave_Trx] L
      left join [LeaveType_Basic] LB on L.[Company]=LB.[Company] and [LeaveType_Id]=[Leave_Id]
      where L.[Company]='" + PayrollLt.Company.Trim() + @"'
  ";
        //計薪年月=生效年月
        Ssql += string.Format(" And Payroll_Processingmonth = {0}", PayrollLt.SalaryYM.ToString());
        //員工帳號
        Ssql += string.Format(" And EmployeeId = '{0}'", PayrollLt.EmployeeId.Trim());
        Ssql += " group by  L.[Company],[EmployeeId] ";

        DataTable theDT = _MyDBM.ExecuteDataTable(Ssql);

        if (theDT != null)
        {
            if (theDT.Rows.Count > 0)
            {
                try
                {
                    if (theDT.Rows[0][item] != null)
                    {
                        return theDT.Rows[0][item].ToString();
                    }
                }
                catch { }
            }
        }
        //return dt;
        return "0";
    }
}
