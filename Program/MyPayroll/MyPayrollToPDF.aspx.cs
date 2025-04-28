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
using System.Data.SqlClient;
//using PanPacificClass;

public partial class MyPayrollToPDF : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM021";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    Payroll thePayroll = new Payroll();
    Payroll.PayrolList PayrollLt = new Payroll.PayrolList();
    Decimal dFixedSalary = 0, dNonFixedAmount = 0, dOtherNT = 0, dOtherWT = 0, dMAmount = 0, dHI2Amt = 0;
    DataTable dtHI2 = new DataTable();
    string sKind = "W";
    string sCompany, sSYM, sDepartment;
    string path = "", pdfpwd = "";
    bool blCheckBatch = false;
    public static string sEmployeeId = string.Empty;
    public static string outputMsg = string.Empty;
    string PDFKind = "MyPayRollToPDF";
    public int iPdfPageCount = 0;
    string PayrollTable = "PayrollWorking";
    //標題補充文字
    string ShowTitle = "";
    string ShowMode = "(測試模式)";
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
        ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", @"JQ();", true);
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "1", Page.ResolveUrl("~/Scripts/jquery-1.4.4.min.js").ToString());
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "2", Page.ResolveUrl("~/Scripts/jquery-ui-1.8.7.custom.min.js").ToString());
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "3", Page.ResolveUrl("~/Scripts/ui.datepicker.js").ToString());
        if (_UserInfo.SysSet.GetCalendarSetting().Equals("Y"))
        {//決定是否使用民國年
            Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "4", Page.ResolveUrl("~/Scripts/ui.datepicker.tw.js").ToString());
        }
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString(), Page.ResolveUrl("~/Pages/pagefunction.js").ToString());
        //用於執行等待畫面
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "B", Page.ResolveUrl("~/Pages/Busy.js").ToString());
        // 需要執行等待畫面的按鈕
        btntopdf.Attributes.Add("onClick", "drawWait('')");

        lbl_Msg.Text = "";

        if ((Request["erpadm"] != null && Request["erpadm"].ToString() == "y") || (_UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, "Batch") != false))
            blCheckBatch = true;

        //if (_UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, "Print") == true)
        //    showPrint.Style.Clear();
        //else
        //    rbPrint.SelectedValue = "N";

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
            //if (_UserInfo.AuthLogin == false)
            //{
            //    Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=UnLogin");
            //}
            //else
            //{
            //    if (_UserInfo.CheckPermission(_ProgramId) == false)
            //        Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
            //}
            //OldId.Text = _UserInfo.UData.EmployeeId.Trim();
            //OldId.Enabled = false;
            //if (OldId.Text.Length == 0)
            //{
            //    if (_UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, "Batch") != false)
            //        OldId.Enabled = true;
            //    else
            //        Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=UserNoEID");
            //}
            #endregion
            CL_Bonus.SetCodeList("PY#Bonus");
            //起迄預設為系統當月底
            DateTime theMonthLastDay = DateTime.Today.AddMonths(1).AddDays(-DateTime.Today.Day);
            tbAmtDateS.Text = _UserInfo.SysSet.FormatDate(theMonthLastDay.ToString("yyyy/MM/dd"));
            tbAmtDateE.Text = tbAmtDateS.Text;
        }
        else
        {
            lbl_Msg.Text = "";
            //SearchList1.ShowResignEmp = (rbResignC.SelectedValue == "") ? 2 : (rbResignC.SelectedValue == "Y") ? 0 : 1;
            //SearchList1.ReSetList();
        }

        if (SearchList1.CompanyValue.Trim() != "")
        {
            //sCompany = SearchList1.CompanyValue.Trim() + " - " + DBSetting.CompanyName(SearchList1.CompanyValue.Trim());
            sCompany = DBSetting.CompanyName(SearchList1.CompanyValue.Trim());
            PayrollLt.Company = SearchList1.CompanyValue.Trim();
        }

        rbPDFPwd.Attributes.Add("onclick", "javascript:document.getElementById(\'" + tbPDFPwd.ClientID + "\').value='';");
        ScriptManager1.RegisterPostBackControl(btntopdf);
        tbAmtDateS.CssClass = "JQCalendar";
        tbAmtDateE.CssClass = "JQCalendar";
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
        //寫入Log
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = PDFKind;
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "Q";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "下載薪資單";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "來源IP:" + Request.UserHostAddress;
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = DownLoadTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = SearchList1.EmployeeValue.Trim();// _UserInfo.UData.UserId
        _MyDBM.DataChgLog(MyCmd.Parameters);

        //設定共用資訊        
        PayrollLt.DeCodeKey = "dbo.DLMyPDF" + DateTime.Now.ToString("yyyyMMddHHmm");
        thePayroll.BeforeQuery(PayrollLt.DeCodeKey);

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

        if (SearchList1.EmployeeValue.Replace("%", "").Trim() != "" || blCheckBatch == false)
        {//一般員工使用時,僅提供單一資料下載
            if (blCheckBatch != false && SearchList1.EmployeeValue.Replace("%", "").Trim() != "")
                PayrollLt.EmployeeId = SearchList1.EmployeeValue.Trim();
            else
                PayrollLt.EmployeeId = OldId.Text.Trim();
            DownLoadPDF(PayrollLt.EmployeeId, DownLoadTime);
            //檔案下載                 
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + PayrollLt.Company + "-" + PayrollLt.EmployeeId + "_" + PayrollLt.SalaryYM + ".pdf");
            //Response.ContentType = "application/octet-stream";
            //Response.Flush();

            FullPathFiles = PayrollLt.Company + "-" + PayrollLt.EmployeeId + "_" + CL_Bonus.SelectedCode + "_" + tbAmtDateS.Text + "-" + tbAmtDateE.Text + ".pdf";
            FullPathFiles = FullPathFiles.Replace("/", "");
            fileInfo = new System.IO.FileInfo(path + (path.EndsWith("\\") ? "DL\\" : "\\DL\\") + FullPathFiles);
            Response.Clear();
            Response.Write("<script language=javascript>");
            Response.Write("<!--");
            Response.Write(Convert.ToChar(13));
            if (lbl_Msg.Text.Trim() != "")
                Response.Write("alert('" + lbl_Msg.Text + "');");
            //導向PDF:endResponse必須設為false以將本頁應執行之動作完成
            //Response.Redirect("DL//" + PayrollLt.Company + "-" + PayrollLt.EmployeeId + "_" + CL_Bonus.SelectedCode + ".pdf", false);
            if (cbEmail.Checked != false || !fileInfo.Exists)
                FullPathFiles = Request.Url.AbsolutePath;
            else
                FullPathFiles = "DL/" + FullPathFiles;

            Response.Write("window.location='" + FullPathFiles + "'");
            Response.Write("//-->");
            Response.Write(Convert.ToChar(13));
            Response.Write("</script>");
            //Response.Redirect(Request.Url.AbsolutePath, false);            
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

            //if (rbResignC.SelectedValue == "Y")
            //    Ssql += " And Upper(IsNull(ResignCode,'')) != 'Y' ";
            //else if (rbResignC.SelectedValue == "N")
            //    Ssql += " And Upper(IsNull(ResignCode,'')) = 'Y' ";

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
                string filename = "DL\\" + System.IO.Path.GetFileName(CL_Bonus.SelectedCode.Trim() + "-Bonus") + ".zip";
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

        Response.End();
    }

    private void DownLoadPDF(string ID, string DownLoadTime)
    {
        DownLoadPDF(ID, DownLoadTime, "", "", "");
    }

    private void DownLoadPDF(string ID, string DownLoadTime, string theName, string theIdNo, string thePwd)
    {
        string strMsg = "";
        iTextSharp.text.Document doc = new Document(PageSize.A4);
        #region 預先計算一些金額:避免畫PDF時反復計算
        sSYM = (tbAmtDateS.Text + "-" + tbAmtDateE.Text).Replace("/", "");
        //呼叫hasdata時會自動重算dNonFixedAmount,dMAmount
        //應扣金額:獎金稅額 dMAmount;
        //非固定金額:獎金 dNonFixedAmount;
        #endregion
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
                        "  And Convert(varchar,EffectiveDate,112) between '" + _UserInfo.SysSet.FormatADDate(tbAmtDateS.Text) + "' and '" + _UserInfo.SysSet.FormatADDate(tbAmtDateE.Text) + "' " +
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
                    string FileName = PayrollLt.Company + "-" + ID + "_" + CL_Bonus.SelectedCode + "_" + sSYM + ".pdf";
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

                    //if (rbPrint.SelectedValue == "Y")
                    //{//允許列印
                    //    iPermission = PdfWriter.AllowPrinting;
                    //}

                    //PDF是否以個密碼加密,權限密碼設定(空白即隨機給定,故無法使用),是否允許列印...
                    writer.SetEncryption(PdfWriter.STRENGTH128BITS, pdfpwd, ownerpass, iPermission);
                    #endregion

                    #region 设置PDF的头信息，一些属性设置，在Document.Open 之前完成


                    doc.AddAuthor(_UserInfo.UData.CompanyName);
                    doc.AddCreationDate();
                    doc.AddCreator(_UserInfo.UData.EmployeeId);
                    doc.AddSubject("人事薪資系統");
                    doc.AddTitle("獎金單");
                    //doc.AddKeywords("ASP.NET,PDF,iTextSharp,DeltaCat,三角猫");
                    //自定义头
                    doc.AddHeader("Expires", "0");
                    #endregion
                    string fontPath = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\..\Fonts\KAIU.TTF";
                    //载入字体
                    BaseFont baseFT = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font Titlefont = new iTextSharp.text.Font(baseFT, 20);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(baseFT, 12);
                    iTextSharp.text.Font font2 = new iTextSharp.text.Font(baseFT, 12);
                    font2.Color = Color.RED;

                    //打开document
                    doc.Open();

                    iPdfPageCount = 0;
                    int iYMs = Convert.ToInt32(_UserInfo.SysSet.FormatADDate(tbAmtDateS.Text).Replace("/", ""));
                    int iYMe = Convert.ToInt32(_UserInfo.SysSet.FormatADDate(tbAmtDateE.Text).Replace("/", ""));
                    for (int iTheYM = iYMs; iTheYM <= iYMe; iTheYM++)
                    {
                        int tryMonth = (iTheYM / 100) % 100;
                        int tryDay = iTheYM % 100;
                        if ((tryMonth >= 1 && tryMonth <= 12) && (tryDay >= 1 && tryDay <= 31))
                        {
                            doc = DrawPDF(doc, iTheYM.ToString().Insert(6, "/").Insert(4, "/"));
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
                        {   //For 測試使用
                            //if (!string.IsNullOrEmpty(strEMail))
                            //    strEMail = _UserInfo.SysSet.GetConfigString("MailAdmin1");
                            EMail("", strEMail, "獎金單收件人", FullPathFiles, (string.IsNullOrEmpty(strEMail) ? "此員工未設定電子信箱" : ""));
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

    private DataSet GetHI2(string DeCodeKey, string theCompany, string theEmployee, string theAmtDate)
    {
        string Ssql = "";
        Ssql = "SP_PRA_BonusHI2PDF";
        SqlCommand sqlcmd = new SqlCommand();
        sqlcmd.Parameters.Add("@ls_Company", System.Data.SqlDbType.VarChar).Value = theCompany;
        sqlcmd.Parameters.Add("@ls_EmployeeId", System.Data.SqlDbType.VarChar).Value = theEmployee;
        sqlcmd.Parameters.Add("@ls_AmtDate", System.Data.SqlDbType.VarChar).Value = theAmtDate;
        sqlcmd.Parameters.Add("@ls_PayrollTable", System.Data.SqlDbType.VarChar).Value = PayrollTable;
        sqlcmd.Parameters.Add("@ls_Key", System.Data.SqlDbType.VarChar).Value = DeCodeKey;
        sqlcmd.Parameters.Add("@ls_ControlDown", System.Data.SqlDbType.VarChar).Value = "Y";
        DataSet Ds = new DataSet();
        int iRet = _MyDBM.ExecStoredProcedure(Ssql, sqlcmd.Parameters, out Ds);
        return Ds;
    }

    protected iTextSharp.text.Document DrawPDF(iTextSharp.text.Document doc, string thePayDate)
    {
        DateTime tryDay = new DateTime();
        if (DateTime.TryParse(thePayDate, out tryDay) == false)
            return doc;
        else if (CheckSalaryYM(thePayDate) == false)
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
        iTextSharp.text.Font fontSub1 = new iTextSharp.text.Font(bfChineseThin, 10);
        iTextSharp.text.Font fontSub2 = new iTextSharp.text.Font(bfChineseThin, 9);
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
        Paragraph Title = new Paragraph("獎金單", Titlefont);
        //內容水平置中
        Title.SetAlignment("center");

        //插入圖片
        string imageUrl = path.Remove(path.LastIndexOf("\\")) + "\\Image\\LOGO.JPG";
        iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(new Uri(imageUrl));
        //設定圖檔縮放大小
        jpg.ScaleToFit(60f, 60f);
        jpg.SetAbsolutePosition(520, 800);
        Title.AddSpecial(jpg);

        //Tc = new iTextSharp.text.Cell(jpg);
        ////內容水平置中
        //Tc.HorizontalAlignment = Element.ALIGN_RIGHT;
        ////內容高度置中 (Top,Middle感覺不到有沒有移動)
        //Tc.VerticalAlignment = Element.ALIGN_TOP;
        //Tc.Rowspan = 1;
        //Tc.Colspan = 1;

        ////將Cell加入表格
        //Tb.AddCell(Tc);

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

        Tc = new iTextSharp.text.Cell(new Phrase("發放日期:", font));
        //內容水平置中
        Tc.HorizontalAlignment = Element.ALIGN_CENTER;
        //內容高度置中 (Top,Middle感覺不到有沒有移動)
        Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
        Tc.Rowspan = 1;
        Tc.Colspan = 1;

        //將Cell加入表格
        Tb.AddCell(Tc);
        Tc = new iTextSharp.text.Cell(new Phrase(thePayDate, font));
        //內容水平置中
        Tc.HorizontalAlignment = Element.ALIGN_LEFT;
        //內容高度置中(Top,Middle感覺不到有沒有移動)
        Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
        Tc.Rowspan = 1;
        Tc.Colspan = 5;

        Tb.AddCell(Tc);
        Tc = new iTextSharp.text.Cell(new Phrase("獎　　金:", font));
        //內容水平置中
        Tc.HorizontalAlignment = Element.ALIGN_CENTER;
        //內容高度置中 (Top,Middle感覺不到有沒有移動)
        Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
        Tc.Rowspan = 1;
        Tc.Colspan = 1;

        //將Cell加入表格
        Tb.AddCell(Tc);
        Tc = new iTextSharp.text.Cell(new Phrase(CL_Bonus.SelectedCodeName.Trim(), font));
        //內容水平置中
        Tc.HorizontalAlignment = Element.ALIGN_LEFT;
        //內容高度置中(Top,Middle感覺不到有沒有移動)
        Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
        Tc.Rowspan = 1;
        Tc.Colspan = 5;

        Tb.AddCell(Tc);
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
        Graphic gp2 = new Graphic();
        gp2.MoveTo(35, 595);
        gp2.LineTo(560, 595);
        gp2.Stroke();
        doc.Add(gp2);
        #endregion
        //準備明細要用的標題名稱
        string[] item2 = { "獎金總額", "", "", "代扣所得稅", "", "", "代扣補充保費", "", "", "實發金額", "", "" };
        string[] item2Value = { dNonFixedAmount.ToString("N0"), "", "", dMAmount.ToString("N0"), "", "", dHI2Amt.ToString("N0"), "", "", (dNonFixedAmount - dMAmount - dHI2Amt).ToString("N0"), "", "" };
        #region 下半
        iTextSharp.text.Table Tb3 = new iTextSharp.text.Table(6, 4);
        Tb3.WidthPercentage = 100;
        Tb3.Padding = 0;
        Tb3.Cellpadding = 3;
        //Tb.Spacing = 3;
        //自動填滿欄位(如果沒有填滿欄位,不會畫出欄位的線條)       
        Tb3.AutoFillEmptyCells = true;
        for (int i = 0; i < item2.Length; i++)
        {
            if (item2[i].ToString() != "")
            {
                Tc = new iTextSharp.text.Cell(new Phrase(item2[i].ToString(), font));

                //內容水平置中
                Tc.HorizontalAlignment = Element.ALIGN_CENTER;
                //內容高度置中 (Top,Middle感覺不到有沒有移動)
                Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
                Tc.Rowspan = 1;
                Tc.Colspan = 1;

                //將Cell加入表格
                Tb3.AddCell(Tc);

                if (item2[i].ToString().Contains("扣"))
                {
                    Tc = new iTextSharp.text.Cell(new Phrase(item2Value[i], font2));
                }
                else
                {
                    Tc = new iTextSharp.text.Cell(new Phrase(item2Value[i], font));
                }

                //內容水平置中
                Tc.HorizontalAlignment = Element.ALIGN_RIGHT;
                //內容高度置中(Top,Middle感覺不到有沒有移動)
                Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
                Tc.Rowspan = 1;
                Tc.Colspan = 5;//配合空白區使用;若空白區開啟,則此應設為1
                Tb3.AddCell(Tc);
            }
            else
            {
                #region 空白區
                //Tc = new iTextSharp.text.Cell();
                //Tc.Border = 0;
                //Tc.Rowspan = 1;
                //Tc.Colspan = 1;
                //Tb3.AddCell(Tc);
                //Tc = new iTextSharp.text.Cell();
                //Tc.Border = 0;
                //Tc.Rowspan = 1;
                //Tc.Colspan = 1;
                //Tb3.AddCell(Tc);
                #endregion
            }
        }
        doc.Add(Tb3);
        #endregion

        #region 補充保費計算表
        try
        {//取得補充保費計算表
            DataSet ds = GetHI2(PayrollLt.DeCodeKey, PayrollLt.Company, sEmployeeId.Remove(sEmployeeId.IndexOf("-") - 1), thePayDate);
            dtHI2 = ds.Tables[0];
        }
        catch
        {
            dtHI2 = null;
        }

        if (dtHI2 != null)
        {
            #region 切割線
            Graphic gp3 = new Graphic();
            gp3.MoveTo(35, 455);
            gp3.LineTo(560, 455);
            gp3.Stroke();
            doc.Add(gp3);
            #endregion

            //子標題頭
            Paragraph SubTitle = new Paragraph("補充保費計算表", font);
            //內容水平靠左
            SubTitle.SetAlignment("Left");
            SubTitle.SpacingBefore = 20;
            SubTitle.SpacingAfter = -10;
            //加入抬頭
            doc.Add(SubTitle);

            //準備[補充保費計算表]要用的標題名稱
            string[] item3 = { "項目", "給付\r\n日期", "當月\r\n投保金額\r\n(A)", "4倍投保\r\n金額\r\n(B=AX4)"
                                 , "單次\r\n獎金金額\r\n(C)", "累計\r\n獎金金額\r\n(D)"
                                 , "累計超過\r\n4倍投保\r\n金額之獎金\r\n(E=D-B)", "補充保險費\r\n費基\r\n(F)\r\nmin(E,C)"
                                 , "補充保險費\r\n金額\r\n(G=F*" + ((ViewState["HI62Rate"] != null) ? ViewState["HI62Rate"] : "2") + "%)" };

            #region 補充保費計算表
            iTextSharp.text.Table Tb4 = new iTextSharp.text.Table(9, dtHI2.Rows.Count + 1);
            Tb4.WidthPercentage = 100;
            Tb4.Padding = 0;
            Tb4.Cellpadding = 3;
            //Tb.Spacing = 3;
            //自動填滿欄位(如果沒有填滿欄位,不會畫出欄位的線條)       
            Tb4.AutoFillEmptyCells = true;
            //標題列
            for (int i = 0; i < item3.Length; i++)
            {
                if (item3[i].ToString() != "")
                {
                    Tc = new iTextSharp.text.Cell(new Phrase(item3[i].ToString(), fontSub1));
                    //內容水平置中
                    Tc.HorizontalAlignment = Element.ALIGN_CENTER;
                    //內容高度置中 (Top,Middle感覺不到有沒有移動)
                    Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //將Cell加入表格
                    Tb4.AddCell(Tc);
                }
            }
            //內容列
            for (int iRow = 0; iRow < dtHI2.Rows.Count; iRow++)
            {
                for (int i = 2; i < dtHI2.Columns.Count - 1; i++)
                {
                    string strValue = dtHI2.Rows[iRow][i].ToString();
                    if (i > 3)
                    {
                        try
                        {
                            strValue = ((decimal)dtHI2.Rows[iRow][i]).ToString("N0");
                        }
                        catch { }
                    }

                    if (i == 3)
                    {
                        Tc = new iTextSharp.text.Cell(new Phrase(strValue, fontSub2));
                    }
                    else
                    {
                        Tc = new iTextSharp.text.Cell(new Phrase(strValue, fontSub1));
                    }
                    if (i < 4)
                    {
                        //內容水平置中
                        Tc.HorizontalAlignment = Element.ALIGN_CENTER;
                    }
                    else
                    {
                        //內容水平靠右
                        Tc.HorizontalAlignment = Element.ALIGN_RIGHT;
                    }
                    //if (i == 3) Tc.NoWrap = true;

                    //內容高度置中(Top,Middle感覺不到有沒有移動)
                    Tc.VerticalAlignment = Element.ALIGN_MIDDLE;
                    Tb4.AddCell(Tc);
                }
            }

            doc.Add(Tb4);
            #endregion
        }
        #endregion

        //頁尾
        Paragraph PageEnd = new Paragraph("辛苦一年了，好好犒賞自己一下吧！", font);
        //內容水平置中
        PageEnd.SetAlignment("left");
        doc.Add(PageEnd);

        PageEnd = new Paragraph("公司為密薪制;請勿違反規定，談論薪資獎金。", font2);
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

    private bool CheckSalaryYM(string thePayDate)
    {
        try
        {
            Ssql = "SELECT count(*) FROM BonusMaster Where Company='" + PayrollLt.Company.Trim() + "'" +
                " and IsNull(Del_Mark,'') = '' " +
                " and IsNull(ControlDown,'') = 'Y'	";

            //獎金日期
            Ssql += string.Format(" And [AmtDate] = Convert(Datetime,'{0}') ", thePayDate);
            //員工帳號
            if (PayrollLt.EmployeeId.Replace("%", "").Trim() != "")
                Ssql += string.Format(" And EmployeeId = '{0}'", PayrollLt.EmployeeId.Trim());

            Ssql += string.Format(" And CostId = '{0}'", CL_Bonus.SelectedCode.Trim());

            DataTable theDT = _MyDBM.ExecuteDataTable(Ssql);

            if (PayrollLt.EmployeeId == "A1001")
            {
            }

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

    protected void ChangPassWord_Click(object sender, EventArgs e)
    {
        Changdata(1);
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

        if (string.IsNullOrEmpty(PayrollLt.DeCodeKey))
            Ssql = "SELECT * FROM BonusMaster Where Company='" + PayrollLt.Company.Trim() + "'" +
                " and IsNull(Del_Mark,'') = '' " +
                " and IsNull(ControlDown,'') = 'Y'	";
        else
            Ssql = "SELECT Sum(" + PayrollLt.DeCodeKey + "([CostAmt])) As dNonFixedAmount " +
                " ,Sum(" + PayrollLt.DeCodeKey + "([Pay_AMT])) As dAmount " +
                " ,Sum(" + PayrollLt.DeCodeKey + "([HI2])) as HI2Amt " +
                " ,DepId,DepName FROM BonusMaster Where Company='" + PayrollLt.Company.Trim() + "'" +
                " and IsNull(Del_Mark,'') = '' " +
                " and IsNull(ControlDown,'') = 'Y'	";
        //發放日期起
        try
        {
            Ssql += string.Format(" And [AmtDate] >= Convert(Datetime,'{0}') ", Convert.ToDateTime(_UserInfo.SysSet.FormatADDate(tbAmtDateS.Text)).ToString("yyyy/MM/dd"));
        }
        catch { }
        //發放日期迄
        try
        {
            Ssql += string.Format(" And [AmtDate] <= Convert(Datetime,'{0}') ", Convert.ToDateTime(_UserInfo.SysSet.FormatADDate(tbAmtDateE.Text)).ToString("yyyy/MM/dd"));
        }
        catch { }

        //員工帳號
        if (PayrollLt.EmployeeId.Replace("%", "").Trim() != "")
            Ssql += string.Format(" And EmployeeId = '{0}'", PayrollLt.EmployeeId.Trim());

        Ssql += string.Format(" And CostId = '{0}'", CL_Bonus.SelectedCode.Trim());

        if (!string.IsNullOrEmpty(PayrollLt.DeCodeKey))
            Ssql += "Group By DepId,DepName";
        DataTable theDT = _MyDBM.ExecuteDataTable(Ssql);

        if (theDT != null)
        {
            if (theDT.Rows.Count <= 0)
            {
                lbl_Msg.Text = "查無獎金發放資料!";
                return false;
            }
            else
            {
                if (!string.IsNullOrEmpty(PayrollLt.DeCodeKey))
                {
                    dMAmount = 0;
                    dNonFixedAmount = 0;
                    try
                    {
                        dNonFixedAmount = Convert.ToDecimal(theDT.Rows[0]["dNonFixedAmount"].ToString());
                    }
                    catch { }
                    try
                    {
                        dMAmount = Convert.ToDecimal(theDT.Rows[0]["dAmount"].ToString());
                    }
                    catch { }
                    try
                    {
                        dHI2Amt = Convert.ToDecimal(theDT.Rows[0]["HI2Amt"].ToString());
                    }
                    catch { }
                    try
                    {
                        sDepartment = theDT.Rows[0]["DepId"].ToString() + " - " + theDT.Rows[0]["DepName"].ToString();
                    }
                    catch { }
                }
            }
        }
        else
        {
            lbl_Msg.Text = "查無獎金發放資料";
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
        if (CL_Bonus.SelectedCode.Trim() == "")
        {
            lbl_Msg.Text = "請選擇獎金項目 !!";
            blCheck = false;
        }
        else if (SearchList1.EmployeeValue.Replace("%", "").Trim() == "" && OldId.Text.Trim() == "")
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
                Sql = "Select A.Company, Rtrim(A.ERPID) as ERPID, Rtrim(A.Employeeid) AS Employeeid,Cast(A.PayRollPW as varchar) AS PayRollPW,";
                Sql += " C.EmployeeName,C.IdNo,C.DeptId  From PersonnelSecurity A,Personnel_Master C  ";
                Sql += " Where C.Company='" + SearchList1.CompanyValue.Trim() + "' And C.EmployeeId='" + sEmployeeId.ToUpper() + "' AND A.Company=C.Company And A.EmployeeId=C.EmployeeId ";
            }

            CheckData = _MyDBM.ExecuteDataTable(Sql);
            if (CheckData.Rows.Count >= 1)
            {
                lbl_Msg.Text = "";
                sEmployeeId = CheckData.Rows[0]["Employeeid"].ToString();
                if (OldPassword.Text.Trim() != _UserInfo.SysSet.rtnTrans(CheckData.Rows[0]["PayRollPW"].ToString().Trim()))
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
                lbl_Msg.Text = "";
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
                Context = "附件為該員工的" + CL_Bonus.SelectedCodeName + "獎金單";
                MailTo = MailfromEmail;
            }
            if (Subject.Length == 0)
                Subject = "您的" + CL_Bonus.SelectedCodeName + "獎金單";
            if (Context.Length == 0)
                Context = "附件為您的" + CL_Bonus.SelectedCodeName + "獎金單，密碼若未曾修改則請用預設值！(預設值請洽人事管理者)";
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
                Context = "請至指定網址查詢您的" + CL_Bonus.SelectedCodeName + "獎金單，密碼若未曾修改則請用預設值！(預設值請洽人事管理者)";
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
}
