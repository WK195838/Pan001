using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


public partial class Basic_ImportTimeClock : System.Web.UI.Page
{
    UserInfo _UserInfo = new UserInfo();
    SysSetting _SysSet = new SysSetting();
    DBManger _MyDBM;

    string _ProgramId = "PYI001";

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
    protected void bunOK_Click(object sender, EventArgs e)
    {
        try
        {

            if (FileUpload1.HasFile && string.IsNullOrEmpty(CompanyList1.SelectValue) == false)
            {
                if (FileUpload1.FileName.EndsWith(".xls") == true)
                {
                    //上傳
                    string fileName = FileUpload1.FileName;
                    string mDirectory = Server.MapPath("~") + @"\Import\TimeClock\";
                    string savePath = mDirectory + CompanyList1.SelectValue + "-" + System.DateTime.Now.ToString("yyyyMMdd HHmmss") + ".xls";

                    if (System.IO.Directory.Exists(mDirectory) == false) Directory.CreateDirectory(mDirectory);

                    FileUpload1.SaveAs(savePath);
                    //處理上傳檔案
                    ExcelManger mExcel = new ExcelManger();
                    string[] arrImport = mExcel.ImportPanTimeClock(savePath, "Sheet4", 1, 0, 8, "AttendanceSheet", CompanyList1.SelectValue);
                    lbl_Msg.Text = arrImport[0];

                    SqlCommand MyCmdQY = new SqlCommand();
                    MyCmdQY.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = Request.Url.ToString().Trim();
                    MyCmdQY.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "I";
                    MyCmdQY.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "Import";
                    MyCmdQY.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = arrImport[0] + " " + arrImport[1];
                    MyCmdQY.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.ToString("yyyyMMdd HHmmss");
                    MyCmdQY.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
                    _MyDBM.DataChgLog(MyCmdQY.Parameters);

                    //導入出勤異常表
                    if (arrImport[2] == "true")
                    {
                        AE_alert.Visible = true;
                    }

                }
                else
                    lbl_Msg.Text = "僅能上傳 Excel 檔案";
            }
            else
                lbl_Msg.Text = "請選擇正確檔案及公司";

        }
        catch (Exception ex)
        {
            SysSetting _SysSet = new SysSetting();
            string ErrorLog = "卡鐘匯入檔案時發生錯誤" + "Error Message:" + ex.Message + "\n\r File:" + FileUpload1.FileName + "\n\r";

            string LogFile = _SysSet.WriteToLogs("卡鐘", ErrorLog);
            lbl_Msg.Text = ex.Message;

            SqlCommand MyCmdQY = new SqlCommand();
            MyCmdQY.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = Request.Url.ToString().Trim();
            MyCmdQY.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "I";
            MyCmdQY.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "Import";
            MyCmdQY.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = ErrorLog + " " + LogFile;
            MyCmdQY.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.ToString("yyyyMMdd HHmmss");

            MyCmdQY.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
            _MyDBM.DataChgLog(MyCmdQY.Parameters);
        }

    }

}
