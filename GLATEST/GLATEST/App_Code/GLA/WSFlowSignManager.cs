using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DBClass;

/// <summary>
/// WSFlowSignManager 的摘要描述
/// </summary>
public class WSFlowSignManager
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
	public WSFlowSignManager()
	{
        _db = new DBManger();
        _db.New();
	}
    /// <summary>
    /// 取得 spzf_CanSign 回傳的 DataSet
    /// </summary>
    /// <returns></returns>
    public DataSet GetCanSignDS(string UserId, string Company, string FlowId, string SignDocUnid)
    {
        DataSet ds = new DataSet();

        try
        {
            int ret = GetCanSignDS(UserId, Company, FlowId, SignDocUnid, out ds);
            this._err = false;
        }
        catch (Exception ex)
        {
            this._errMsg = ex.Message;
            this._err = true;
        }

        return ds;
    }
    /// <summary>
    /// 取得 spzf_CanSign 回傳的 int && ref DataSet
    /// </summary>
    /// <returns></returns>
    public int GetCanSignDS(string UserId, string Company, string FlowId, string SignDocUnid, out DataSet ds)
    {
        int iResult = 0;
        ds = new DataSet();
        string sql = string.Format("spzf_CanSign");
        SqlCommand cmd = new SqlCommand();
        SqlParameterCollection sqlPars = cmd.Parameters;
        sqlPars.Add("@LoginAccount", SqlDbType.VarChar, 20).Value = UserId;
        sqlPars.Add("@Company", SqlDbType.Char, 2).Value = Company;
        sqlPars.Add("@FlowId", SqlDbType.VarChar, 30).Value = FlowId;
        sqlPars.Add("@SignDocUnid", SqlDbType.VarChar, 36).Value = SignDocUnid;

        try
        {
            iResult = _db.ExecStoredProcedure(sql, sqlPars, out ds);
            this._err = false;
        }
        catch (Exception ex)
        {
            this._errMsg = ex.Message;
            this._err = true;
        }

        return iResult;
    }
    /// <summary>
    /// 取得 spzf_GetNextStepSigner 回傳的 DataSet
    /// </summary>
    /// <returns></returns>
    public DataSet GetNextStepSignerDS(string Company, string FlowId, string SignDocUnid, string SingerEmid, int SignerBwseq)
    {
        DataSet ds = new DataSet();

        try
        {
            int ret = GetNextStepSignerDS(Company, FlowId, SignDocUnid, SingerEmid, SignerBwseq, out ds);
            this._err = false;
        }
        catch (Exception ex)
        {
            this._errMsg = ex.Message;
            this._err = true;
        }

        return ds;
    }
    /// <summary>
    /// 取得 spzf_GetNextStepSigner 回傳的 int && ref DataSet
    /// </summary>
    /// <returns></returns>
    public int GetNextStepSignerDS(string Company, string FlowId, string SignDocUnid, string SingerEmid, int SignerBwseq, out DataSet ds)
    {
        int iResult = 0;
        ds = new DataSet();
        string sql = string.Format("spzf_GetNextStepSigner");
        SqlCommand cmd = new SqlCommand();
        SqlParameterCollection sqlPars = cmd.Parameters;
        sqlPars.Add("@Company", SqlDbType.Char, 2).Value = Company;
        sqlPars.Add("@FlowId", SqlDbType.VarChar, 30).Value = FlowId;
        sqlPars.Add("@SignDocUnid", SqlDbType.VarChar, 36).Value = SignDocUnid;
        sqlPars.Add("@SingerEmid", System.Data.SqlDbType.VarChar, 20).Value = SingerEmid;
        sqlPars.Add("@SignerBwseq", System.Data.SqlDbType.Int).Value = SignerBwseq;

        try
        {
            iResult = _db.ExecStoredProcedure(sql, sqlPars, out ds);
            this._err = false;
        }
        catch (Exception ex)
        {
            this._errMsg = ex.Message;
            this._err = true;
        }

        return iResult;
    }
    /// <summary>
    /// 取得 spzf_GetReturnSteps 回傳的 DataSet
    /// </summary>
    /// <returns></returns>
    public DataSet GetReturnStepsDS(string Company, string FlowId, string SignDocUnid)
    {
        DataSet ds = new DataSet();

        try
        {
            int ret = GetReturnStepsDS(Company, FlowId, SignDocUnid, out ds);
            this._err = false;
        }
        catch (Exception ex)
        {
            this._errMsg = ex.Message;
            this._err = true;
        }

        return ds;
    }
    /// <summary>
    /// 取得 spzf_GetReturnSteps 回傳的 int && ref DataSet
    /// </summary>
    /// <returns></returns>
    public int GetReturnStepsDS(string Company, string FlowId, string SignDocUnid, out DataSet ds)
    {
        int iResult = 0;
        ds = new DataSet();
        string sql = string.Format("spzf_GetReturnSteps");
        SqlCommand cmd = new SqlCommand();
        SqlParameterCollection sqlPars = cmd.Parameters;
        sqlPars.Add("@Company", SqlDbType.Char, 2).Value = Company;
        sqlPars.Add("@FlowId", SqlDbType.VarChar, 30).Value = FlowId;
        sqlPars.Add("@SignDocUnid", SqlDbType.VarChar, 36).Value = SignDocUnid;

        try
        {
            iResult = _db.ExecStoredProcedure(sql, sqlPars, out ds);
            this._err = false;
        }
        catch (Exception ex)
        {
            this._errMsg = ex.Message;
            this._err = true;
        }

        return iResult;
    }
    public CanSignData GetCanSignData(string UserId, string Company, string FlowId, string SignDocUnid)
    {
        CanSignData CSData = new CanSignData();
        DataSet ds = new DataSet();
        int ret = GetCanSignDS(UserId, Company, FlowId, SignDocUnid, out ds);
        //取得回傳的DataTable
        DataTable dtReturn = (DataTable)ds.Tables[2];
        if (dtReturn != null && dtReturn.Rows.Count > 0)
        {
            CSData.ReturnCode = Ftool.IntTryParse(dtReturn.Rows[0]["ReturnCode"].ToString());
            CSData.CanNotSignDesc = dtReturn.Rows[0]["CanNotSignDesc"].ToString();
            CSData.IsFirstStep = Ftool.IntTryParse(dtReturn.Rows[0]["IsFirstStep"].ToString());
            CSData.NewFlowDoc = Ftool.IntTryParse(dtReturn.Rows[0]["NewFlowDoc"].ToString());
            CSData.LastFlowDirection = Ftool.IntTryParse(dtReturn.Rows[0]["LastFlowDirection"].ToString());
            CSData.IsFlowEnd = Ftool.IntTryParse(dtReturn.Rows[0]["IsFlowEnd"].ToString());
            CSData.StepTag = dtReturn.Rows[0]["StepTag"].ToString();
            CSData.CurrentStepId = dtReturn.Rows[0]["CurrentStepId"].ToString();  // 11/5 davey 改
            CSData.CanUndo = Ftool.IntTryParse(dtReturn.Rows[0]["CanUndo"].ToString());
        }
        if (ret > 0)
        {
            //取得送簽人員 CanSignApplicantsData 集合
            List<CanSignApplicantsData> CanSignApplicantsDataLT = new List<CanSignApplicantsData>();
            CanSignApplicantsData CanSignApplicantsData;
            DataTable dtApplicants = (DataTable)ds.Tables[0];
            if (dtApplicants != null && dtApplicants.Rows.Count > 0)
            {
                foreach (DataRow dr in dtApplicants.Rows)
                {
                    CanSignApplicantsData = GetCanSignApplicantsData(dr);
                    CanSignApplicantsDataLT.Add(CanSignApplicantsData);
                }
                CSData.CanSignApplicantsDataLT = CanSignApplicantsDataLT;
            }
            //取得申請人員 GetCanSignSignerData 集合
            List<CanSignSignerData> CanSignSignerDataLT = new List<CanSignSignerData>();
            CanSignSignerData CanSignSignerData;
            DataTable dtSigner = (DataTable)ds.Tables[1];
            if (dtSigner != null && dtSigner.Rows.Count > 0)
            {
                foreach (DataRow dr in dtSigner.Rows)
                {
                    CanSignSignerData = GetCanSignSignerData(dr);
                    CanSignSignerDataLT.Add(CanSignSignerData);
                }
                CSData.CanSignSignerDataLT = CanSignSignerDataLT;
            }
        }

        return CSData;
    }
    /// <summary>
    /// 取得送簽人員 CanSignApplicantsData
    /// </summary>
    /// <param name="dr"></param>
    /// <returns></returns>
    private CanSignApplicantsData GetCanSignApplicantsData(DataRow dr)
    {
        CanSignApplicantsData data = new CanSignApplicantsData();
        data.EmployeeId = dr["EmployeeId"].ToString();
        data.ByworkSeq = Ftool.IntTryParse(dr["ByworkSeq"].ToString());
        data.EmployeeName = dr["EmployeeName"].ToString();
        data.ShowInFlowName = dr["ShowInFlowName"].ToString();

        return data;
    }
    /// <summary>
    /// 取得申請人員 GetCanSignSignerData
    /// </summary>
    /// <param name="dr"></param>
    /// <returns></returns>
    private CanSignSignerData GetCanSignSignerData(DataRow dr)
    {
        CanSignSignerData data = new CanSignSignerData();
        data.EmployeeId = dr["EmployeeId"].ToString();
        data.ByworkSeq = Ftool.IntTryParse(dr["ByworkSeq"].ToString());
        data.EmployeeName = dr["EmployeeName"].ToString();
        data.ShowInFlowName = dr["ShowInFlowName"].ToString();

        return data;
    }
    public int GetIsCanSign(string UserId, string Company, string FlowId, string SignDocUnid)
    {
        int iResult = 0;
        string sql = string.Format("spzf_CanSign");
        SqlCommand cmd = new SqlCommand();
        SqlParameterCollection sqlPars = cmd.Parameters;
        sqlPars.Add("@LoginAccount", SqlDbType.VarChar, 20).Value = UserId;
        sqlPars.Add("@Company", SqlDbType.Char, 2).Value = Company;
        sqlPars.Add("@FlowId", SqlDbType.VarChar, 30).Value = FlowId;
        sqlPars.Add("@SignDocUnid", SqlDbType.VarChar, 36).Value = SignDocUnid;

        try
        {
            iResult = _db.ExecuteCommand(sql, sqlPars, CommandType.StoredProcedure);
            this._err = false;
        }
        catch (Exception ex)
        {
            this._errMsg = ex.Message;
            this._err = true;
        }

        return iResult;
    }
    public int ExecutePushFlow(PushFlowData data)
    {
        int iResult = 0;
        string sql = string.Format("spzf_PushFlow");
        SqlCommand cmd = new SqlCommand();
        SqlParameterCollection sqlPars = cmd.Parameters;
        sqlPars.Add("@Company", SqlDbType.Char, 2).Value = data.Company;
        sqlPars.Add("@FlowId", SqlDbType.VarChar, 30).Value = data.FlowId;
        sqlPars.Add("@SignDocUnid", SqlDbType.VarChar, 36).Value = data.SignDocUnid;
        sqlPars.Add("@Applicant", SqlDbType.VarChar, 20).Value = data.Applicant;
        sqlPars.Add("@ApntBwseq", SqlDbType.TinyInt).Value = Convert.ToByte(data.ApntBwseq);
        sqlPars.Add("@SingerEmid", SqlDbType.VarChar, 30).Value = data.SingerEmid;
        sqlPars.Add("@SignerBwseq", SqlDbType.TinyInt).Value = Convert.ToByte(data.SignerBwseq);
        sqlPars.Add("@ActionId", SqlDbType.VarChar, 20).Value = data.ActionId;
        sqlPars.Add("@FlowDirection", SqlDbType.TinyInt).Value = Convert.ToByte(data.FlowDirection);
        sqlPars.Add("@SelectedSignerEmid", System.Data.SqlDbType.VarChar, 30).Value = data.SelectedSignerEmid;
        sqlPars.Add("@SelectedSignerBwseq", System.Data.SqlDbType.TinyInt).Value = Convert.ToByte(data.SelectedSignerBwseq);
        sqlPars.Add("@SignedHistoryUnid", System.Data.SqlDbType.BigInt).Value = Ftool.IntTryParse(data.SignedHistoryUnid);
        sqlPars.Add("@SignComment", System.Data.SqlDbType.VarChar, 512).Value = data.SignComment;
        sqlPars.Add("@TargetStepId", System.Data.SqlDbType.VarChar, 30).Value = data.TargetStepId;
        sqlPars.Add("@WebHostBaseURL", System.Data.SqlDbType.VarChar, 128).Value = HttpContext.Current.Session["Domain"];

        try
        {
            iResult = _db.ExecuteCommand(sql, sqlPars, CommandType.StoredProcedure);
            this._err = false;
        }
        catch (Exception ex)
        {
            this._errMsg = ex.Message;
            this._err = true;
        }

        return iResult;
    }
}
/// <summary>
/// spzf_CanSign 物件
/// </summary>
#region spzf_CanSign 說明
/// <remarks>
///-- 預存程序 : spzf_CanSign
///-- ===============================================================================================
///-- Description : 判斷目前使用者是否可以簽核當前文件
///-- 【傳入】: 
///--						@LoginAccount varchar(20)			-- 登入帳戶
///--						@Company Char(2)					-- 公司別
///--						@FlowId varchar(30)					-- 流程代號
///--						@SignDocUnid varchar(36)			-- 簽核文件記錄編號
///-- 【傳回送簽人員Table】: 可作為送簽人員之相關資訊(有兼職時則傳回多筆)
///--						EmployeeId Varchar(20),				--> 員工代號
///--						ByworkSeq Tinyint,					--> 職務序
///--						EmployeeName Nvarchar(40),		    --> 員工姓名
///--						ShowInFlowName Nvarchar(100)	    --> 流程職務名稱
///-- 【傳回申請人員Table】: 申請人員之相關資訊(單筆)
///--						EmployeeId Varchar(20),				--> 員工代號
///--						ByworkSeq Tinyint,					--> 職務序
///--						EmployeeName Nvarchar(40),		    --> 員工姓名
///--						ShowInFlowName Nvarchar(100)	    --> 流程職務名稱
///-- 【傳回值Table】: 
///--					ReturnCode Tinyint,					--> 結果碼，0=不可簽核，>=1時表示可簽核
///--					CanNotSignDesc Varchar,				--> 沒有權限時之說明文字
///--					IsFirstStep bit						--> 是否為第一站，0=不是第一站，1=是第一站
///--					NewFlowDoc bit,						--> 0=已存在的流程文件，1=新的流程文件(尚未存在於 zfFlowMaster)
///--					LastFlowDirection  Tinyint			--> 最後簽核動作之流程方向碼(1=進; 2=退; 3=不准; 4=作廢)
///--					IsFlowEnd  bit						--> 流程是否結束，0=否; 1=是
///--					StepTag Varchar(30)					--> 站別標籤
///--					    作為識別簽核站別特定意義之用，可自行定義。因StepId在同一公司別及流程代號之下不可重覆，
///--						但同一簽核人員則可能出現在2站以上，故可以此欄位識別之。
///--						此值存於簽核對話框屬性值中，可用控制顯示不同之簽核按鈕。
///--					CanUndo bit							--> 是否有權作送簽取回，0=無權，1=有權
/// </remarks>
#endregion
[Serializable]
public class CanSignData
{
    #region 資料物件屬性
    private int _ReturnCode = 0;
    /// <summary>
    /// 結果碼，0=不可簽核，>=1時表示可簽核
    /// </summary>
    public int ReturnCode
    {
        set { _ReturnCode = value; }
        get { return _ReturnCode; }
    }

    private string _CanNotSignDesc;
    /// <summary>
    /// 沒有權限時之說明文字
    /// </summary>
    public string CanNotSignDesc
    {
        set { _CanNotSignDesc = value; }
        get { return _CanNotSignDesc; }
    }

    private int _IsFirstStep;
    /// <summary>
    /// 是否為第一站，0=不是第一站，1=是第一站
    /// </summary>
    public int IsFirstStep
    {
        set { _IsFirstStep = value; }
        get { return _IsFirstStep; }
    }

    private int _NewFlowDoc;
    /// <summary>
    /// 0=已存在的流程文件，1=新的流程文件(尚未存在於 zfFlowMaster)
    /// </summary>
    public int NewFlowDoc
    {
        set { _NewFlowDoc = value; }
        get { return _NewFlowDoc; }
    }

    private int _LastFlowDirection;
    /// <summary>
    /// 最後簽核動作之流程方向碼(1=進; 2=退; 3=不准; 4=作廢)
    /// </summary>
    public int LastFlowDirection
    {
        set { _LastFlowDirection = value; }
        get { return _LastFlowDirection; }
    }

    private int _IsFlowEnd;
    /// <summary>
    /// 流程是否結束，0=否; 1=是
    /// </summary>
    public int IsFlowEnd
    {
        set { _IsFlowEnd = value; }
        get { return _IsFlowEnd; }
    }

    private string _StepTag;
    /// <summary>
    /// 站別標籤
    /// </summary>
    public string StepTag
    {
        set { _StepTag = value; }
        get { return _StepTag; }
    }

    private string _CurrentStepId;
    /// <summary>
    /// 判斷第幾站
    /// </summary>
    public string CurrentStepId
    {
        set { _CurrentStepId = value; }
        get { return _CurrentStepId; }
    }


    private int _CanUndo;
    /// <summary>
    /// 是否有權作送簽取回，0=無權，1=有權
    /// </summary>
    public int CanUndo
    {
        set { _CanUndo = value; }
        get { return _CanUndo; }
    }

    private List<CanSignApplicantsData> _CanSignApplicantsDataLT;
    /// <summary>
    /// 送簽人員集合
    /// </summary>
    public List<CanSignApplicantsData> CanSignApplicantsDataLT
    {
        set { _CanSignApplicantsDataLT = value; }
        get { return _CanSignApplicantsDataLT; }
    }

    private List<CanSignSignerData> _CanSignSignerDataLT;
    /// <summary>
    /// 送簽人員集合
    /// </summary>
    public List<CanSignSignerData> CanSignSignerDataLT
    {
        set { _CanSignSignerDataLT = value; }
        get { return _CanSignSignerDataLT; }
    }
    #endregion
    public CanSignData()
    {
    }
}
/// <summary>
/// 送簽人員
/// </summary>
[Serializable]
public class CanSignApplicantsData
{
    #region 資料物件屬性
    private string _EmployeeId;
    /// <summary>
    /// 員工代號
    /// </summary>
    public string EmployeeId
    {
        set { _EmployeeId = value; }
        get { return _EmployeeId; }
    }

    private int _ByworkSeq;
    /// <summary>
    /// 職務序
    /// </summary>
    public int ByworkSeq
    {
        set { _ByworkSeq = value; }
        get { return _ByworkSeq; }
    }

    private string _EmployeeName;
    /// <summary>
    /// 員工姓名
    /// </summary>
    public string EmployeeName
    {
        set { _EmployeeName = value; }
        get { return _EmployeeName; }
    }

    private string _ShowInFlowName;
    /// <summary>
    /// 流程職務名稱
    /// </summary>
    public string ShowInFlowName
    {
        set { _ShowInFlowName = value; }
        get { return _ShowInFlowName; }
    }
    #endregion
    public CanSignApplicantsData()
    {
    }
}
/// <summary>
/// 申請人員
/// </summary>
[Serializable]
public class CanSignSignerData
{
    #region 資料物件屬性
    private string _EmployeeId;
    /// <summary>
    /// 員工代號
    /// </summary>
    public string EmployeeId
    {
        set { _EmployeeId = value; }
        get { return _EmployeeId; }
    }

    private int _ByworkSeq;
    /// <summary>
    /// 職務序
    /// </summary>
    public int ByworkSeq
    {
        set { _ByworkSeq = value; }
        get { return _ByworkSeq; }
    }

    private string _EmployeeName;
    /// <summary>
    /// 員工姓名
    /// </summary>
    public string EmployeeName
    {
        set { _EmployeeName = value; }
        get { return _EmployeeName; }
    }

    private string _ShowInFlowName;
    /// <summary>
    /// 流程職務名稱
    /// </summary>
    public string ShowInFlowName
    {
        set { _ShowInFlowName = value; }
        get { return _ShowInFlowName; }
    }
    #endregion
    public CanSignSignerData()
    {
    }
}

#region spzf_GetNextStepSigner 說明
//-- 預存程序 : spzf_GetNextStepSigner
//-- ===============================================================================================
//-- Description : 取得下一站之簽核人員(清單)
//-- 【傳入】: 
//--						@Company Char(2)						-- 公司別
//--						@FlowId varchar(30)						-- 流程代號
//--						@SignDocUnid varchar(36)				-- 簽核文件記錄編號
//--						@SingerEmid Varchar(20)				    -- 實簽人員工號
//--						@SignerBwseq Tinyint					-- 實簽人員職務序
//-- 【傳回簽核者Table】: 下一站之簽核人員之相關資訊(可能多筆)
//--						Seq Decimal(5,2),						--> 資料序號
//--						EmployeeId Varchar(20),		    		--> 員工代號
//--						ByworkSeq Tinyint,						--> 職務序
//--						EmployeeName Nvarchar(40),		        --> 員工姓名
//--						ShowInFlowName Nvarchar(100)	        -- 流程職務名稱
//-- 【傳回簽核動作Table】: 下一站之可用簽核動作(可能多筆)
//--						Seq Decimal(5,2),						--> 資料序號
//--						ActionId Varchar(20),					--> 動作代號
//--						ActiionName Nvarchar(20),			    --> 動作名稱
//--						FlowDirection Tinyint					--> 流程方向
//-- 【傳回值Table】: 多重簽核者規則 (1=首位簽核 2=串簽 3=並簽 4=用戶自選 5=任一簽核)
//--					    NewFlowDoc bit,					    	--> 0=已存在的流程文件，1=新的流程文件 (須先執行起流程之功能(spzf_InitializeFlow) )
//--						TargetStepId Varchar(30),				--> 下一站站別代號
//--						MutiSignerRule Tinyint					--> 多重簽核者規則
#endregion

#region spzf_GetReturnSteps 說明
//-- 預存程序 : spzf_GetReturnSteps
//-- ===============================================================================================
//-- Description:	指定流程代號及簽核文件之記錄編號後，取得可退回之流程站別及人員資料表

//-- 【傳入】: 
//--						@Company Char(2)						-- 公司別
//--						@FlowId varchar(30)						-- 流程代號
//--						@SignDocUnid varchar(36)				-- 簽核文件記錄編號

//-- 【傳回】: 可退回之流程站別及人員資料表
//--						StepSeq Decimal(5,3),					--> 流程站別順序
//--						StepId Varchar(30),						--> 流程站別代號
//--						StepName Nvarchar(30),					--> 流程站別名稱
//--						MutiSignerRule Tinyint, 				--> 簽核者工號
//--						DocSigner Varchar(20), 					--> 簽核者工號
//--						SignerBwseq Tinyint,					--> 簽核者職務序
//--						SignerFlowName Nvarchar(50),	    	--> 簽核者職務全稱
//--						Unid Bigint								--> 該筆在簽核歷史檔中的PKey值
//-- 【傳回】: 可退回之流程站別資料表
//--						StepSeq Decimal(5,3),					--> 流程站別順序
//--						StepId Varchar(30),						--> 流程站別代號
//--						StepName Nvarchar(30),					--> 流程站別名稱
#endregion

/// <summary>
/// 申請人員
#region spzf_PushFlow 說明
///-- 預存程序 : spzf_PushFlow
///-- ===============================================================================================
///-- Description :	當使用者執行簽核動作後之各流程檔案的異動處理
///--				注意：(@SignedHistoryUnid Is Not null And @SignedHistoryUnid <> 0) And @FlowDirection = 2 時，
///--					決定退回給指定人員
///-- 【傳入】: 
///--						@Company Char(2)						-- 公司別
///--						@FlowId varchar(30)						-- 流程代號
///--						@SignDocUnid varchar(36)				-- 簽核文件記錄編號
///--						@Applicant Varchar(20)					-- 申請人員工號
///--						@ApntBwseq Tinyint						-- 申請人員職務序
///--						@SingerEmid Varchar(20)			    	-- 實簽人員工號
///--						@SignerBwseq Tinyint					-- 實簽人員職務序
///--						@SelectedSignerEmid Varchar(20)	        -- 選擇的下一站簽核人員工號
///--						@SelectedSignerBwseq Tinyint	    	-- 選擇的下一站簽核人員職務序
///--						@SignedHistoryUnid Varchar(36)	    	-- 退回指定人員在簽核歷史檔中的PKey
///--						@ActionId Varchar(20)					-- 簽核動作碼
///--						@FlowDirection Tinyint					-- 簽核流程方向
///--						@SignComment Nvarchar ((512)	        -- 簽核意見
///--					    @TargetStepId Varchar(30)		    	-- 下一站站別 (簽核流程方向為退回時，則代表由使用者選擇之退回站別代號)
///--                       @WebHostBaseURL VarChar(128)            -- Session["Domain"]
///-- 【傳回值Table】: 
///--					    ErrorCode Tinyint,						--> 錯誤碼 (0＝成功， ≠0 表示失敗)
///-- 【傳回值】:  錯誤碼 (0＝成功， ≠0 表示失敗)
#endregion
/// </summary>
[Serializable]
public class PushFlowData
{
    #region 資料物件屬性
    /// <summary>
    /// 公司別
    /// </summary>
    public string Company { get; set; }
    /// <summary>
    /// 流程代號
    /// </summary>
    public string FlowId { get; set; }
    /// <summary>
    /// 簽核文件記錄編號
    /// </summary>
    public string SignDocUnid { get; set; }
    /// <summary>
    /// 申請人員工號
    /// </summary>
    public string Applicant { get; set; }
    /// <summary>
    /// 申請人員職務序
    /// </summary>
    public int ApntBwseq { get; set; }
    /// <summary>
    /// 實簽人員工號
    /// </summary>
    public string SingerEmid { get; set; }
    /// <summary>
    /// 實簽人員職務序
    /// </summary>
    public int SignerBwseq { get; set; }
    /// <summary>
    /// 選擇的下一站簽核人員工號
    /// </summary>
    public string SelectedSignerEmid { get; set; }
    /// <summary>
    /// 選擇的下一站簽核人員職務序
    /// </summary>
    public int SelectedSignerBwseq { get; set; }
    /// <summary>
    /// 退回指定人員在簽核歷史檔中的PKey
    /// </summary>
    public string SignedHistoryUnid { get; set; }
    /// <summary>
    /// 簽核動作碼
    /// </summary>
    public string ActionId { get; set; }
    /// <summary>
    /// 簽核流程方向
    /// </summary>
    public int FlowDirection { get; set; }
    /// <summary>
    /// 簽核意見
    /// </summary>
    public string SignComment { get; set; }
    /// <summary>
    /// 下一站站別 (簽核流程方向為退回時，則代表由使用者選擇之退回站別代號)
    /// </summary>
    public string TargetStepId { get; set; }
    #endregion
    public PushFlowData()
    {
    }
}