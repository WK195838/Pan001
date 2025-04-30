using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class ImportSalaryGradeRankData : System.Web.UI.Page
{
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYI004";
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
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";

        if (!Page.IsPostBack)
        {
            if (_UserInfo.AuthLogin == false)
            {
                Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=UnLogin");
            }
            else
            {
                if (_UserInfo.CheckPermission(_ProgramId) == false)
                    Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
            }
        }
        else
        {
            //BindData();
        }
    }


    protected void btnImportDefault01_Click(object sender, EventArgs e)
    {
        MyCmd.Parameters.Clear();
        try
        {
            string mDirectory = Server.MapPath("~") + @"\Import\Template\";
            string savePath = mDirectory + "SalaryGrade01.xls";

            if (System.IO.File.Exists(savePath))
            {
                //處理上傳檔案
                ExcelManger mExcel = new ExcelManger();
                string[] arrImport = mExcel.ImportSalaryGradeRankData(savePath, "Sheet1", 1, 1, 30, "SalaryPoint");
                lbl_Msg.Text = arrImport[0];

                MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = arrImport[0] + " " + arrImport[1];
            }
            else
            {
                lbl_Msg.Text = "找不到薪職等級之預設檔案!請洽網站管理員";
                return;
            }

        }
        catch (Exception ex)
        {

            SysSetting _SysSet = new SysSetting();
            string ErrorLog = "匯入預設檔案時發生錯誤" + "Error Message:" + ex.Message + "\n\r File:" + FU_SGRData.FileName + "\n\r";

            string LogFile = _SysSet.WriteToLogs("薪職等級", ErrorLog);
            lbl_Msg.Text = ex.Message;
                        
            MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = ErrorLog + " " + LogFile;            
        }

        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "SalaryGradeRankData";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "I";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "SalaryPoint-薪職等級";
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
    }

    protected void btnImportDefault02_Click(object sender, EventArgs e)
    {
        MyCmd.Parameters.Clear();
        try
        {
            string mDirectory = Server.MapPath("~") + @"\Import\Template\";
            string savePath = mDirectory + "SalaryGrade02.xls";

            if (System.IO.File.Exists(savePath))
            {
                //處理上傳檔案
                ExcelManger mExcel = new ExcelManger();
                string[] arrImport = mExcel.ImportSalaryGradeRankData(savePath, "Sheet1", 1, 1, 30, "BaseSalary");
                lbl_Msg.Text = arrImport[0];

                MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = arrImport[0] + " " + arrImport[1];
            }
            else
            {
                lbl_Msg.Text = "找不到本薪等級之預設檔案!請洽網站管理員";
                return;
            }

        }
        catch (Exception ex)
        {

            SysSetting _SysSet = new SysSetting();
            string ErrorLog = "匯入預設檔案時發生錯誤" + "Error Message:" + ex.Message + "\n\r File:" + FU_SGRData.FileName + "\n\r";

            string LogFile = _SysSet.WriteToLogs("本薪等級", ErrorLog);
            lbl_Msg.Text = ex.Message;

            MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = ErrorLog + " " + LogFile;
        }

        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "SalaryGradeRankData";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "I";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "BaseSalary-本薪等級";
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
    }

    protected void btnImportGrade_Click(object sender, EventArgs e)
    {
        MyCmd.Parameters.Clear();
        try
        {
            if (FU_SGRData.HasFile)
            {
                if (FU_SGRData.FileName.EndsWith(".xls") == true)
                {
                    //上傳
                    string fileName = FU_SGRData.FileName;
                    string mDirectory = Server.MapPath("~") + @"\Import\SalaryGradeRank\";
                    string savePath = mDirectory + "Grade_" + System.DateTime.Now.ToString("yyyyMMdd HHmmss") + ".xls";

                    if (System.IO.Directory.Exists(mDirectory) == false) System.IO.Directory.CreateDirectory(mDirectory);

                    FU_SGRData.SaveAs(savePath);
                    //處理上傳檔案
                    ExcelManger mExcel = new ExcelManger();
                    string[] arrImport = mExcel.ImportSalaryGradeRankData(savePath, "Sheet1", 1, 1, 30, "SalaryPoint");
                    lbl_Msg.Text = arrImport[0];

                    MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = arrImport[0] + " " + arrImport[1];
                }
                else
                {
                    lbl_Msg.Text = "上傳檔案格式不正確!請參閱Excel範本修改並存成 .xls 格式之檔案";
                    return;
                }
            }
            else
            {
                lbl_Msg.Text = "請選擇正確檔案";
                return;
            }

        }
        catch (Exception ex)
        {

            SysSetting _SysSet = new SysSetting();
            string ErrorLog = "匯入檔案時發生錯誤" + "Error Message:" + ex.Message + "\n\r File:" + FU_SGRData.FileName + "\n\r";

            string LogFile = _SysSet.WriteToLogs("薪職等級", ErrorLog);
            lbl_Msg.Text = ex.Message;
                        
            MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = ErrorLog + " " + LogFile;            
        }

        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "SalaryGradeRankData";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "I";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "SalaryPoint-薪職等級";
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
    }

    protected void btnImportBS_Click(object sender, EventArgs e)
    {
        MyCmd.Parameters.Clear();
        try
        {
            if (FU_SGRData.HasFile)
            {
                if (FU_SGRData.FileName.EndsWith(".xls") == true)
                {
                    //上傳
                    string fileName = FU_SGRData.FileName;
                    string mDirectory = Server.MapPath("~") + @"\Import\SalaryGradeRank\";
                    string savePath = mDirectory + "BS_" + System.DateTime.Now.ToString("yyyyMMdd HHmmss") + ".xls";

                    if (System.IO.Directory.Exists(mDirectory) == false) System.IO.Directory.CreateDirectory(mDirectory);

                    FU_SGRData.SaveAs(savePath);
                    //處理上傳檔案
                    ExcelManger mExcel = new ExcelManger();
                    string[] arrImport = mExcel.ImportSalaryGradeRankData(savePath, "Sheet1", 1, 1, 30, "BaseSalary");
                    lbl_Msg.Text = arrImport[0];

                    MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = arrImport[0] + " " + arrImport[1];
                }
                else
                {
                    lbl_Msg.Text = "上傳檔案格式不正確!請參閱Excel範本修改並存成 .xls 格式之檔案";
                    return;
                }
            }
            else
            {
                lbl_Msg.Text = "請選擇正確檔案";
                return;
            }

        }
        catch (Exception ex)
        {

            SysSetting _SysSet = new SysSetting();
            string ErrorLog = "匯入檔案時發生錯誤" + "Error Message:" + ex.Message + "\n\r File:" + FU_SGRData.FileName + "\n\r";

            string LogFile = _SysSet.WriteToLogs("本薪等級", ErrorLog);
            lbl_Msg.Text = ex.Message;

            MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = ErrorLog + " " + LogFile;
        }

        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "SalaryGradeRankData";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "I";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "BaseSalary-本薪等級";

        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
    }
}
