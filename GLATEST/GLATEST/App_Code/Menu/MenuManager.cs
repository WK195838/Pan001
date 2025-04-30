using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// MenuManager 的摘要描述
/// </summary>
public class MenuManager
{
    #region 管理物件屬性
    private string _errMsg;
    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public string ErrorMessage
    {
        set { _errMsg = value; }
        get { return _errMsg; }
    }
    private bool _err;
    /// <summary>
    /// 是否有錯誤
    /// </summary>
    public bool HasError
    {
        set { _err = value; }
        get { return _err; }
    }
    /// <summary>
    /// 共用資料庫物件
    /// </summary>
    private DBManger _db;
    #endregion

    /// <summary>
    /// 建構式
    /// </summary>
	public MenuManager()
	{
        _db = new DBManger();
        _db.New(DBManger.ConnectionString.IBosDB);

	}
    public MenuManager(DBManger.ConnectionString dbenum)
    {
        _db = new DBManger();
        _db.New(dbenum);
    }
    public List<MenuData> CreateLeftMenuLT()
    {
        List<MenuData> datas = new List<MenuData>();
        // ------ 左方Menu ------
        // 增加泛型的另一種寫法
        datas.Add(new MenuData() {
            ParentProgId = "GLATEST",
            location = "Left",
            ProgId = "GLA01",
            ProgSort = 1,
            ProgMode = "M",
            ProgName = "日常帳務",
            ProgUrl = "GLA0110T.aspx?ProgId=GLA01",//GLA0110T 過渡版
            ProgDesc = "日常帳務(GLA)"
        });
        datas.Add(new MenuData()
        {
            ParentProgId = "GLATEST",
            location = "Left",
            ProgId = "GLA02",
            ProgSort = 2,
            ProgMode = "M",
            ProgName = "日常作業",
            ProgUrl = "GLI01C0.aspx?ProgId=GLA02",
            ProgDesc = "日常作業(GLA)"
        });
        // 增加泛型的傳統寫法
        MenuData data;
        data = new MenuData();
        data.ParentProgId = "GLATEST";      //會計總帳
        data.location = "Left";
        data.ProgId = "GLR01";
        data.ProgSort = 3;
        data.ProgMode = "M";
        data.ProgName = "報表管理1";
        data.ProgUrl = "GLR02A0.aspx?ProgId=GLR01";
        data.ProgDesc = "報表管理1(GLR)";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLATEST";      //會計總帳
        data.location = "Left";
        data.ProgId = "GLR02";
        data.ProgSort = 4;
        data.ProgMode = "M";
        data.ProgName = "報表管理2";
        data.ProgUrl = "GLR0210.aspx?ProgId=GLR02";
        data.ProgDesc = "報表管理2(GLR)";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLATEST";      //會計總帳
        data.location = "Left";
        data.ProgId = "GLR03";
        data.ProgSort = 5;
        data.ProgMode = "M";
        data.ProgName = "報表管理3";
        data.ProgUrl = "GLR0300.aspx?ProgId=GLR03";
        data.ProgDesc = "報表管理3(GLR)";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLATEST";      //會計總帳
        data.location = "Left";
        data.ProgId = "GLC01";
        data.ProgSort = 6;
        data.ProgMode = "M";
        data.ProgName = "結帳處理";
        data.ProgUrl = "GLA0510.aspx?ProgId=GLC01";
        data.ProgDesc = "結帳處理(GLC)";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLATEST";      //會計總帳
        data.location = "Left";
        data.ProgSort = 7;
        data.ProgId = "GLD01";
        data.ProgMode = "M";
        data.ProgName = "資料設定";
        data.ProgUrl = "GLA0410.aspx?ProgId=GLD01";
        data.ProgDesc = "資料設定(GLD)";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLATEST";      //會計總帳
        data.location = "Left";
        data.ProgId = "GLB01";
        data.ProgSort = 8;
        data.ProgMode = "M";
        data.ProgName = "總帳基本資料";
        data.ProgUrl = "GLA0370.aspx?ProgId=GLB01";
        data.ProgDesc = "總帳基本資料(GLB)";
        datas.Add(data);
        // ------ 上方Menu1 ------
        // 日常帳務
        data = new MenuData();
        data.ParentProgId = "GLA01";
        data.location = "Top1";
        data.ProgId = "GLA0101";
        data.ProgSort = 1;
        data.ProgMode = "P";
        data.ProgName = "傳票登錄";
        data.ProgUrl = "GLA0110T.aspx?ProgId=GLA01";//GLA0110T 過渡版
        data.ProgDesc = "傳票登錄";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLA01";
        data.location = "Top1";
        data.ProgId = "GLA0102";
        data.ProgSort = 2;
        data.ProgMode = "P";
        data.ProgName = "傳票更正作業";
        data.ProgUrl = "GLA0130.aspx?ProgId=GLA01";
        data.ProgDesc = "傳票更正作業";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLA01";
        data.location = "Top1";
        data.ProgId = "GLA0103";
        data.ProgSort = 3;
        data.ProgMode = "P";
        data.ProgName = "傳票核准作業";
        data.ProgUrl = "GLA0140.aspx?ProgId=GLA01";
        data.ProgDesc = "傳票核准作業";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLA01";
        data.location = "Top1";
        data.ProgId = "GLA0104";
        data.ProgSort = 4;
        data.ProgMode = "P";
        data.ProgName = "傳票列印";
        data.ProgUrl = "GLR01J0.aspx?ProgId=GLA01";
        data.ProgDesc = "傳票列印";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLA01";
        data.location = "Top1";
        data.ProgId = "GLA0105";
        data.ProgSort = 5;
        data.ProgMode = "P";
        data.ProgName = "傳票核准取消作業";
        data.ProgUrl = "GLA0150.aspx?ProgId=GLA01";
        data.ProgDesc = "傳票核准取消作業";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLA01";
        data.location = "Top1";
        data.ProgId = "GLA0106";
        data.ProgSort = 6;
        data.ProgMode = "P";
        data.ProgName = "傳票過帳作業";
        data.ProgUrl = "GLA0170.aspx?ProgId=GLA01";
        data.ProgDesc = "傳票過帳作業";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLA01";
        data.location = "Top1";
        data.ProgId = "GLA0107";
        data.ProgSort = 7;
        data.ProgMode = "P";
        data.ProgName = "傳票查詢作業";
        data.ProgUrl = "GLI01B0.aspx?ProgId=GLA01";
        data.ProgDesc = "傳票查詢作業";
        datas.Add(data);
        // 日常作業
        data = new MenuData();
        data.ParentProgId = "GLA02";
        data.location = "Top1";
        data.ProgId = "GLA0201";
        data.ProgSort = 1;
        data.ProgMode = "P";
        data.ProgName = "分類帳查詢作業";
        data.ProgUrl = "GLI01C0.aspx?ProgId=GLA02";
        data.ProgDesc = "分類帳查詢作業";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLA02";
        data.location = "Top1";
        data.ProgId = "GLA0202";
        data.ProgSort = 2;
        data.ProgMode = "P";
        data.ProgName = "科目餘額查詢";
        data.ProgUrl = "GLI01M0.aspx?ProgId=GLA02";
        data.ProgDesc = "科目餘額查詢";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLA02";
        data.location = "Top1";
        data.ProgId = "GLA0203";
        data.ProgSort = 3;
        data.ProgMode = "P";
        data.ProgName = "日計表";
        data.ProgUrl = "GLR01G0.aspx?ProgId=GLA02";
        data.ProgDesc = "日計表";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLA02";
        data.location = "Top1";
        data.ProgId = "GLA0204";
        data.ProgSort = 4;
        data.ProgMode = "P";
        data.ProgName = "日記帳";
        data.ProgUrl = "GLR01H0.aspx?ProgId=GLA02";
        data.ProgDesc = "日記帳";
        datas.Add(data);
        // 報表管理1
        data = new MenuData();
        data.ParentProgId = "GLR01";
        data.location = "Top1";
        data.ProgId = "GLR0101";
        data.ProgSort = 1;
        data.ProgMode = "P";
        data.ProgName = "分類帳";
        data.ProgUrl = "GLR02A0.aspx?ProgId=GLR01";
        data.ProgDesc = "分類帳";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLR01";
        data.location = "Top1";
        data.ProgId = "GLR0102";
        data.ProgSort = 2;
        data.ProgMode = "P";
        data.ProgName = "試算表";
        data.ProgUrl = "GLR02B0.aspx?ProgId=GLR01";
        data.ProgDesc = "試算表";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLR01";
        data.location = "Top1";
        data.ProgId = "GLR0103";
        data.ProgSort = 3;
        data.ProgMode = "P";
        data.ProgName = "傳票張數統計";
        data.ProgUrl = "GLR02Q0.aspx?ProgId=GLR01";
        data.ProgDesc = "傳票張數統計";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLR01";
        data.location = "Top1";
        data.ProgId = "GLR0104";
        data.ProgSort = 4;
        data.ProgMode = "P";
        data.ProgName = "傳票刪除清單";
        data.ProgUrl = "GLR02R0.aspx?ProgId=GLR01";
        data.ProgDesc = "傳票刪除清單";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLR01";
        data.location = "Top1";
        data.ProgId = "GLR0105";
        data.ProgSort = 5;
        data.ProgMode = "P";
        data.ProgName = "明細帳-科目";
        data.ProgUrl = "GLB020122.aspx?ProgId=GLR01";
        data.ProgDesc = "明細帳-科目";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLR01";
        data.location = "Top1";
        data.ProgId = "GLR0105";
        data.ProgSort = 5;
        data.ProgMode = "P";
        data.ProgName = "明細帳-區間";
        data.ProgUrl = "GLB020123.aspx?ProgId=GLR01";
        data.ProgDesc = "明細帳-區間";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLR01";
        data.location = "Top1";
        data.ProgId = "GLR0106";
        data.ProgSort = 6;
        data.ProgMode = "P";
        data.ProgName = "沖帳明細表";
        data.ProgUrl = "GLR0150.aspx?ProgId=GLR01";
        data.ProgDesc = "沖帳明細表";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLR01";
        data.location = "Top1";
        data.ProgId = "GLR0107";
        data.ProgSort = 7;
        data.ProgMode = "P";
        data.ProgName = "專案明細表";
        data.ProgUrl = "GLR0160.aspx?ProgId=GLR01";
        data.ProgDesc = "專案明細表";
        datas.Add(data);
        // 報表管理2
        data = new MenuData();
        data.ParentProgId = "GLR02";
        data.location = "Top1";
        data.ProgId = "GLR0201";
        data.ProgSort = 1;
        data.ProgMode = "P";
        data.ProgName = "損益表";
        data.ProgUrl = "GLR0210.aspx?ProgId=GLR02";
        data.ProgDesc = "損益表";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLR02";
        data.location = "Top1";
        data.ProgId = "GLR0202";
        data.ProgSort = 2;
        data.ProgMode = "P";
        data.ProgName = "月份損益表";
        data.ProgUrl = "GLR0220.aspx?ProgId=GLR02";
        data.ProgDesc = "月份損益表";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLR02";
        data.location = "Top1";
        data.ProgId = "GLR0203";
        data.ProgSort = 3;
        data.ProgMode = "P";
        data.ProgName = "年度損益表";
        data.ProgUrl = "GLR0230.aspx?ProgId=GLR02";
        data.ProgDesc = "年度損益表";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLR02";
        data.location = "Top1";
        data.ProgId = "GLR0204";
        data.ProgSort = 4;
        data.ProgMode = "P";
        data.ProgName = "期間損益表";
        data.ProgUrl = "GLR0240.aspx?ProgId=GLR02";
        data.ProgDesc = "期間損益表";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLR02";
        data.location = "Top1";
        data.ProgId = "GLR0205";
        data.ProgSort = 5;
        data.ProgMode = "P";
        data.ProgName = "資產負債表";
        data.ProgUrl = "GLR0250.aspx?ProgId=GLR02";
        data.ProgDesc = "資產負債表";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLR02";
        data.location = "Top1";
        data.ProgId = "GLR0206";
        data.ProgSort = 6;
        data.ProgMode = "P";
        data.ProgName = "月份比較資產負債表";
        data.ProgUrl = "GLR0260.aspx?ProgId=GLR02";
        data.ProgDesc = "月份比較資產負債表";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLR02";
        data.location = "Top1";
        data.ProgId = "GLR0207";
        data.ProgSort = 7;
        data.ProgMode = "P";
        data.ProgName = "年度比較資產負債表";
        data.ProgUrl = "GLR0270.aspx?ProgId=GLR02";
        data.ProgDesc = "年度比較資產負債表";
        datas.Add(data);
        // 報表管理2
        data = new MenuData();
        data.ParentProgId = "GLR03";
        data.location = "Top1";
        data.ProgId = "GLR0301";
        data.ProgSort = 1;
        data.ProgMode = "P";
        data.ProgName = "科目別比較表";
        data.ProgUrl = "GLR0300.aspx?ProgId=GLR03";
        data.ProgDesc = "科目別比較表";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLR03";
        data.location = "Top1";
        data.ProgId = "GLR0302";
        data.ProgSort = 2;
        data.ProgMode = "P";
        data.ProgName = "科目別期間比較表";
        data.ProgUrl = "GLR0310.aspx?ProgId=GLR03";
        data.ProgDesc = "科目別期間比較表";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLR03";
        data.location = "Top1";
        data.ProgId = "GLR0303";
        data.ProgSort = 3;
        data.ProgMode = "P";
        data.ProgName = "部門別比較表";
        data.ProgUrl = "GLR0320.aspx?ProgId=GLR03";
        data.ProgDesc = "部門別比較表";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLR03";
        data.location = "Top1";
        data.ProgId = "GLR0304";
        data.ProgSort = 4;
        data.ProgMode = "P";
        data.ProgName = "年度比較報表";
        data.ProgUrl = "GLR0330.aspx?ProgId=GLR03";
        data.ProgDesc = "年度比較報表";
        datas.Add(data);
        // 結帳處理
        data = new MenuData();
        data.ParentProgId = "GLC01";
        data.location = "Top1";
        data.ProgId = "GLC0101";
        data.ProgSort = 1;
        data.ProgMode = "P";
        data.ProgName = "月底轉結";
        data.ProgUrl = "GLA0510.aspx?ProgId=GLC01";
        data.ProgDesc = "月底轉結";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLC01";
        data.location = "Top1";
        data.ProgId = "GLC0102";
        data.ProgSort = 2;
        data.ProgMode = "P";
        data.ProgName = "年底轉結";
        data.ProgUrl = "GLA0520.aspx?ProgId=GLC01";
        data.ProgDesc = "年底轉結";
        datas.Add(data);
        // 結帳處理
        data = new MenuData();
        data.ParentProgId = "GLD01";
        data.location = "Top1";
        data.ProgId = "GLD0101";
        data.ProgSort = 1;
        data.ProgMode = "P";
        data.ProgName = "會計科目資料維護";
        data.ProgUrl = "GLA0410.aspx?ProgId=GLD01";
        data.ProgDesc = "會計科目資料維護";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLD01";
        data.location = "Top1";
        data.ProgId = "GLD0102";
        data.ProgSort = 2;
        data.ProgMode = "P";
        data.ProgName = "會計期間設定";
        data.ProgUrl = "GLA0360.aspx?ProgId=GLD01";
        data.ProgDesc = "會計期間設定";
        datas.Add(data);
        data = new MenuData();
        data.ParentProgId = "GLD01";
        data.location = "Top1";
        data.ProgId = "GLD0103";
        data.ProgSort = 3;
        data.ProgMode = "P";
        data.ProgName = "會計科目資料維護";
        data.ProgUrl = "GLA0420.aspx?ProgId=GLD01";
        data.ProgDesc = "會計科目資料維護";
        datas.Add(data); 
        data = new MenuData();
        data.ParentProgId = "GLD01";
        data.location = "Top1";
        data.ProgId = "GLD0104";
        data.ProgSort = 3;
        data.ProgMode = "P";
        data.ProgName = "沖帳組合資料維護";
        data.ProgUrl = "GLA0430.aspx?ProgId=GLD01";
        data.ProgDesc = "沖帳組合資料維護";
        datas.Add(data);        
        data = new MenuData();
        data.ParentProgId = "GLD01";
        data.location = "Top1";
        data.ProgId = "GLD0105";
        data.ProgSort = 4;
        data.ProgMode = "P";
        data.ProgName = "報表格式設定";
        data.ProgUrl = "GLA0350.aspx?ProgId=GLD01";
        data.ProgDesc = "報表格式設定";
        datas.Add(data);
        // 結帳處理
        data = new MenuData();
        data.ParentProgId = "GLB01";
        data.location = "Top1";
        data.ProgId = "GLB0101";
        data.ProgSort = 1;
        data.ProgMode = "P";
        data.ProgName = "公司結構設定";
        data.ProgUrl = "GLA0370.aspx?ProgId=GLB01";
        data.ProgDesc = "公司結構設定";
        datas.Add(data);

        return datas;
    }
}

/// <summary>
/// 左方選單
/// </summary>
[Serializable]
public class MenuData
{
    #region 資料物件屬性
    /// <summary>
    /// 系統 ID 或 上層程式 ID
    /// </summary>
    public string ParentProgId { set; get; }
    /// <summary>
    /// 系統位置 Left / Top1 / Top2
    /// </summary>
    public string location { set; get; }
    /// <summary>
    /// 程式 ID
    /// </summary>
    public string ProgId { set; get; }
    /// <summary>
    /// 程式排序
    /// </summary>
    public int ProgSort { set; get; }
    /// <summary>
    /// 程式型態    M:Menu P:Program
    /// </summary>
    public string ProgMode { set; get; }
    /// <summary>
    /// 程式名稱
    /// </summary>
    public string ProgName { set; get; }
    /// <summary>
    /// 程式URL
    /// </summary>
    public string ProgUrl { set; get; }
    /// <summary>
    /// 程式說明
    /// </summary>
    public string ProgDesc { set; get; }
    /// <summary>
    /// 自行增加的HTML語法
    /// </summary>
    public string ProgHtml { set; get; }
    #endregion
    public MenuData()
    {
    }
}