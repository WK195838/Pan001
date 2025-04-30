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
using System.Data.SqlClient;

/// <summary>
/// 人事異動更新程式
/// </summary>
public class CheckTransaction
{
    UserInfo _UserInfo = new UserInfo ( );
    SysSetting _SysSet = new SysSetting ( );
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand ( );
    DBManger _MyDBM = new DBManger ( );

    public CheckTransaction()
    {
        _MyDBM.New();
    }

    /// <summary>
    ///  人事計薪系統每日作業
    /// </summary>
	public void ct()
	{
        string strSQL = "SELECT Count(*) FROM [DataChangeLog] where [ChgUser]='sp_ePayroll_daily' And Convert(char,[ChgStartDateTime],112)=Convert(char,GetDate(),112)";
        DataTable Dt = _MyDBM.ExecuteDataTable(strSQL);
        if (Dt != null && Dt.Rows.Count == 1 && Dt.Rows[0][0].ToString().Equals("0"))
        {
            _MyDBM.ExecStoredProcedure("dbo.sp_ePayroll_daily");
        }
	}
}
