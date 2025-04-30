using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using DBClass;


/// <summary>
/// GLA0110TManager 的摘要描述
/// </summary>
public class GLA0110TManager
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
    private IBosDB _db;
    /// <summary>
    /// 共用資料庫物件
    /// </summary>
    private DBManger _DBManger;
    #endregion

    public GLA0110TManager()
    {
        _db = new IBosDB(this);
        _DBManger = new DBManger();
        _DBManger.New(DBManger.ConnectionString.IBosDB);
    }
    /// <summary>
    /// 取得合約管理表單的生命週期狀態 (回傳:狀態Unid&狀態名稱)
    /// </summary>
    /// <param name="DocMode">表單模式(CreateForm)</param>
    /// <returns></returns>
    public string[] GetDocLifeCycleStatusUnid(string DocMode)
    {
        string[] arrLifeCycleStatus = new string[2];
        string strTableStatusIn = string.Empty;
        string strStatusInternalName = string.Empty;

        switch (DocMode)
        {
            case "CreateForm":    //填寫中
                strTableStatusIn = "*All";
                strStatusInternalName = "Drafting";
                break;

        }
        SqlDataReader sdr;
        string strSql;

        return arrLifeCycleStatus;
    }
    /// <summary>
    /// 取得資料表CSA_SEContract資料結構陣列
    /// </summary>
    /// <returns></returns>
    public string[] GetContractSchema()
    {
        string[] str
            = new string[] { "Company","VoucherNo","VoucherDate","VoucherEntryDate","VoucherOwner","VoucherType","VoucherSource","VoucherAloc"
      ,"JournalCnt","ApprovalCode","PostCode","RevDate","DletFlag","DocFlag","LstChgUser","LstChgDateTime","UserName"};
        return str;
    }
    /// <summary>
    /// 取得資料表CSA_SEContract資料物件List集合
    /// </summary>
    /// <param name="ContractUnid">待辦事項識別碼</param>
    /// <returns></returns>
    public List<GLA0110TData> GetGLA0110TLT(string VoucherNo)
    {
        List<GLA0110TData> dataList = new List<GLA0110TData>();

        GLA0110TData data;

        DataTable dt = this.GetGLA0110TDT(VoucherNo);
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                data = this.GetGLA0110TData(row);
                dataList.Add(data);
            }
        }

        return dataList; 
    }
    /// <summary>
    /// 取得檢視表(vGLVoucherHead)資料物件
    /// </summary>
    /// <returns></returns>
    public DataTable GetGLA0110TDT(string VoucherNo)
    {
        DataTable dt = null;
        SqlString sql;

        try
        {
            sql = new SqlString(SqlString.SqlCommandType.Select, "vGLVoucherHead");
            sql.Add("*");
            sql.Where.Add("LstChgDateTime", DateTime.Now.ToString("yyyyMMdd"));
            sql.Where.Add("VoucherNo", VoucherNo);
            dt = _db.Query(sql.ToString(), "vGLVoucherHead");
            _err = false;
        }
        catch (Exception ex)
        {
            _errMsg = ex.Message;
            _err = true;
        }

        return dt;
    }
    
    /// <summary>
    /// 將DataRow轉換成資料物件
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    private GLA0110TData GetGLA0110TData(DataRow row)
    {
        GLA0110TData data = new GLA0110TData();
        data.Company = row["Company"].ToString();//公司別
        data.VoucherNo = row["VoucherNo"].ToString();//
        data.VoucherDate =  row["VoucherDate"].ToString();//
        data.VoucherEntryDate =  row["VoucherEntryDate"].ToString();//
        data.VoucherOwner = row["VoucherOwner"].ToString();//
        data.VoucherType = row["VoucherType"].ToString();//
        data.VoucherSource = row["VoucherSource"].ToString();//
        data.VoucherAloc = row["VoucherAloc"].ToString();//
        data.JournalCnt = Ftool.DecimalTryParse(row["JournalCnt"].ToString());//
        data.ApprovalCode = row["ApprovalCode"].ToString();//
        data.PostCode = row["PostCode"].ToString();//
        data.RevDate = row["RevDate"].ToString();//
        data.DletFlag = row["DletFlag"].ToString();//
        data.DocFlag = row["DocFlag"].ToString();//
        data.LstChgUser = row["LstChgUser"].ToString();//
        data.LstChgDateTime = row["LstChgDateTime"].ToString();//
        return data;
    }

    /// <summary>
    /// 新增GLA0110TData資料
    /// </summary>
    /// <param name="data">傳票主檔</param>
    /// <returns></returns>
    public int Insert(GLA0110TData data)
    {
        StringBuilder sb = new StringBuilder();
        SqlString sql;
        int effectcount = 0;

        try
        {
            sql = new SqlString(SqlString.SqlCommandType.Insert, "GLVoucherHead");
            sql.Add("Company", data.Company);//公司別
            sql.Add("VoucherNo", data.VoucherNo);//
            sql.Add("VoucherDate", data.VoucherDate);//
            sql.Add("VoucherEntryDate", data.VoucherEntryDate);//
            sql.Add("VoucherOwner", data.VoucherOwner);//
            sql.Add("VoucherType", data.VoucherType);//
            sql.Add("VoucherSource", data.VoucherSource);//
            sql.Add("VoucherAloc", data.VoucherAloc);//
            sql.Add("JournalCnt", data.JournalCnt);//
            sql.Add("ApprovalCode", data.ApprovalCode);//
            sql.Add("PostCode", data.PostCode);//
            sql.Add("RevDate", data.RevDate);//
            sql.Add("DletFlag", data.DletFlag);//
            sql.Add("DocFlag", data.DocFlag);//
            sql.Add("LstChgUser", data.LstChgUser);//
            sql.Add("LstChgDateTime", data.LstChgDateTime);//
            sb.Append(sql.ToString());

            effectcount = _db.Execute(sb.ToString());
            _err = false;
        }
        catch (Exception ex)
        {
            _errMsg = ex.Message;
            _err = true;
        }
        return effectcount;
    }

    

    /// <summary>
    /// 取得相關連資料表資料物件List集合
    /// </summary>
    /// <param name="GLA0110TR"></param>
    /// <returns></returns>
    public List<GLA0110TRData> GetGLA0110TRLT(string VoucherSeqNo)
    {
        List<GLA0110TRData> dataList = new List<GLA0110TRData>();
        GLA0110TRData Rdata;

        DataTable dt = this.GetGLA0110TRDT(VoucherSeqNo);
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                Rdata = this.GetGLA0110TRDate(row);
                dataList.Add(Rdata);
            }
        }
        return dataList;
    }
    /// <summary>
    /// 取得資料表資料物件DataTable
    /// </summary>
    /// <param name="GLA0110TR"></param>
    /// <returns></returns>
    public DataTable GetGLA0110TRDT(string VoucherSeqNo)
    {
        DataTable dt = null;
        SqlString sql;

        try
        {
            sql = new SqlString(SqlString.SqlCommandType.Select, "vGLVoucherDetail");
            sql.Add("*");
            sql.Where.Add("convert(varchar,VoucherNo)", VoucherSeqNo);
            dt = _db.Query(sql.ToString(), "vGLVoucherDetail");
            _err = false;
        }
        catch (Exception ex)
        {
            _errMsg = ex.Message;
            _err = true;
        }

        return dt;
    }
    /// <summary>
    /// 將DataRow轉換成資料物件
    /// </summary>
    /// <param name="dr"></param>
    /// <returns></returns>
    private GLA0110TRData GetGLA0110TRDate(DataRow row)
    {
        GLA0110TRData Rdata = new GLA0110TRData();
        Rdata.Company = row["Company"].ToString();
        Rdata.VoucherNo = row["VoucherNo"].ToString();
        Rdata.VoucherSeqNo = Ftool.DecimalTryParse(row["VoucherSeqNo"].ToString());
        Rdata.AcctNo = row["AcctNo"].ToString();
        Rdata.AcctDesc1 = row["AcctDesc1"].ToString();
        Rdata.AcctType = row["AcctType"].ToString();
        //Rdata.JournalDate = row["JoumalDate"].ToString();
        Rdata.DBAmt = Ftool.DecimalTryParse(row["DBAmt"].ToString());
        Rdata.CRAmt = Ftool.DecimalTryParse(row["CRAmt"].ToString());
        Rdata.AbsValue = row["AbsValue"].ToString();
        Rdata.AcctCtg = row["AcctCtg"].ToString();
        Rdata.PostFlag = row["PostFlag"].ToString();
        Rdata.ApvlFlag = row["ApvlFlag"].ToString();
        Rdata.DAcctNo = row["DAcctNo"].ToString();
        Rdata.CAcctNo = row["CAcctNo"].ToString();
        Rdata.Index01 = row["Index01"].ToString();
        Rdata.Index02 = row["Index02"].ToString();
        Rdata.Index03 = row["Index03"].ToString();
        Rdata.Index04 = row["Index04"].ToString();
        Rdata.Index05 = row["Index05"].ToString();
        Rdata.Index06 = Ftool.DecimalTryParse(row["Index06"].ToString());
        Rdata.Index07 = row["Index07"].ToString();
        Rdata.Idx01 = row["Idx01"].ToString();
        Rdata.Idx02 = row["Idx02"].ToString();
        Rdata.Idx03 = row["Idx03"].ToString();
        Rdata.Idx04 = row["Idx04"].ToString();
        Rdata.Idx05 = row["Idx05"].ToString();
        Rdata.Idx06 = row["Idx06"].ToString();
        Rdata.Idx07 = row["Idx07"].ToString();

        return Rdata;
    }
    public string[] GetGLA0110TRSchema()
    {
        string[] str =
            new string[] {
                "Company" ,"VoucherNo" ,"VoucherSeqNo" ,"AcctNo" ,"AcctDesc1",
                "AcctType"  ,"DBAmt","CRAmt","AbsValue","AcctCtg","PostFlag","ApvlFlag",
               "DAcctNo","CAcctNo","Index01","Index02","Index03","Index04","Index05","Index06","Index07",
               "Idx01","Idx02","Idx03","Idx04","Idx05","Idx06","Idx07"

            };
        return str;
    }

    /// <summary>
    /// 新增GLA0110TRData資料
    /// </summary>
    /// <param name="data">分錄記錄</param>
    /// <returns></returns>
    public int Insert(GLA0110TRData Rdata)
    {
        StringBuilder sb = new StringBuilder();
        SqlString sql;
        int effectcount = 0;

        try
        {
            sql = new SqlString(SqlString.SqlCommandType.Insert, "GLVoucherDetail");
            sql.Add("Company", Rdata.Company);//公司別
            sql.Add("VoucherNo", Rdata.VoucherNo);//
            sql.Add("VoucherSeqNo", Rdata.VoucherSeqNo);//
            sql.Add("AcctNo", Rdata.AcctNo);//
            sql.Add("Idx01", Rdata.Idx01);//
            sql.Add("Idx02", Rdata.Idx02);//
            sql.Add("Idx03", Rdata.Idx03);//
            sql.Add("Idx04", Rdata.Idx04);//
            sql.Add("Idx05", Rdata.Idx05);//
            sql.Add("Idx06", Rdata.Idx06);//
            sql.Add("Idx07", Rdata.Idx07);//
            sql.Add("Index01", Rdata.Index01);//
            sql.Add("Index02", Rdata.Index02);//
            sql.Add("Index03", Rdata.Index03);//
            sql.Add("Index04", Rdata.Index04);//
            sql.Add("Index05", Rdata.Index05);//
            sql.Add("Index06", Rdata.Index06);//
            sql.Add("Index07", Rdata.Index07);//
            sql.Add("DBAmt", Rdata.DBAmt);//
            sql.Add("CRAmt", Rdata.CRAmt);//
            sql.Add("ApvlFlag", Rdata.ApvlFlag);//
            sql.Add("JournalDate", Rdata.JournalDate);
            sql.Add("ReturnDate", Rdata.ReturnDate);//
            sql.Add("VoucherEntryDate", Rdata.VoucherEntryDate);//
            sql.Add("CreateUser", Rdata.CreateUser);//
            sql.Add("LstChgUser", Rdata.LstChgUser);//
            sb.Append(sql.ToString());

            effectcount = _db.Execute(sb.ToString());
            _err = false;
        }
        catch (Exception ex)
        {
            _errMsg = ex.Message;
            _err = true;
        }
        return effectcount;
    }
}

    [Serializable]
    public class GLA0110TData
    {
        #region 資料物件屬性

        private string _Company;
        /// <summary>
        /// _Company (公司別)
        /// </summary>
        public string Company
        {
            set { _Company = value; }
            get { return _Company; }
        }

        private string _VoucherNo;
        /// <summary>
        /// VoucherNo (傳票編號)
        /// </summary>
        public string VoucherNo
        {
            set { _VoucherNo = value; }
            get { return _VoucherNo; }
        }

        private string _VoucherDate;
        /// <summary>
        /// VoucherDate (傳票日期)
        /// </summary>
        public string VoucherDate
        {
            set { _VoucherDate = value; }
            get { return _VoucherDate; }
        }

        private string _VoucherEntryDate;
        /// <summary>
        /// VoucherEntryDate ()
        /// </summary>
        public string VoucherEntryDate
        {
            set { _VoucherEntryDate = value; }
            get { return _VoucherEntryDate; }
        }

        private string _VoucherOwner;
        /// <summary>
        /// VoucherOwner ()
        /// </summary>
        public string VoucherOwner
        {
            set { _VoucherOwner = value; }
            get { return _VoucherOwner; }
        }

        private string _VoucherType;
        /// <summary>
        /// VoucherType ()
        /// </summary>
        public string VoucherType
        {
            set { _VoucherType = value; }
            get { return _VoucherType; }
        }

        private string _VoucherSource;
        /// <summary>
        /// VoucherSource ()
        /// </summary>
        public string VoucherSource
        {
            set { _VoucherSource = value; }
            get { return _VoucherSource; }
        }

        private string _VoucherAloc;
        /// <summary>
        /// VoucherAloc ()
        /// </summary>
        public string VoucherAloc
        {
            set { _VoucherAloc = value; }
            get { return _VoucherAloc; }
        }

        private decimal _JournalCnt;
        /// <summary>
        /// JournalCnt ()
        /// </summary>
        public decimal JournalCnt
        {
            set { _JournalCnt = value; }
            get { return _JournalCnt; }
        }

        private string _ApprovalCode;
        /// <summary>
        /// ApprovalCode ()
        /// </summary>
        public string ApprovalCode
        {
            set { _ApprovalCode = value; }
            get { return _ApprovalCode; }
        }

        private string _PostCode;
        /// <summary>
        /// PostCode ()
        /// </summary>
        public string PostCode
        {
            set { _PostCode = value; }
            get { return _PostCode; }
        }

        private string _RevDate;
        /// <summary>
        /// RevDate ()
        /// </summary>
        public string RevDate
        {
            set { _RevDate = value; }
            get { return _RevDate; }
        }

        private string _DletFlag;
        /// <summary>
        /// DletFlag ()
        /// </summary>
        public string DletFlag
        {
            set { _DletFlag = value; }
            get { return _DletFlag; }
        }

        private string _DocFlag;
        /// <summary>
        /// DocFlag ()
        /// </summary>
        public string DocFlag
        {
            set { _DocFlag = value; }
            get { return _DocFlag; }
        }

        private string _LstChgUser;
        /// <summary>
        /// LstChgUser ()
        /// </summary>
        public string LstChgUser
        {
            set { _LstChgUser = value; }
            get { return _LstChgUser; }
        }

        private string _LstChgDateTime;
        /// <summary>
        /// LstChgDateTime ()
        /// </summary>
        public string LstChgDateTime
        {
            set { _LstChgDateTime = value; }
            get { return _LstChgDateTime; }
        }

        private string _UserName;
        /// <summary>
        /// UserName ()
        /// </summary>
        public string UserName
        {
            set { _UserName = value; }
            get { return _UserName; }
        }

        #endregion

        /// </summary>
        /// 建構式
        /// </summary>
        public GLA0110TData()
        {
        }
    }

    [Serializable]
    public class GLA0110TRData
    {
        #region 資料物件屬性

        private string _Company;
        /// <summary>
        /// _Company (公司別)
        /// </summary>
        public string Company
        {
            set { _Company = value; }
            get { return _Company; }
        }

        private string _VoucherNo;
        /// <summary>
        /// VoucherNo (傳票編號)
        /// </summary>
        public string VoucherNo
        {
            set { _VoucherNo = value; }
            get { return _VoucherNo; }
        }

        private Decimal _VoucherSeqNo;
        /// <summary>
        /// VoucherSeqNo ()
        /// </summary>
        public Decimal VoucherSeqNo
        {
            set { _VoucherSeqNo = value; }
            get { return _VoucherSeqNo; }
        }

        private string _AcctNo;
        /// <summary>
        /// AcctNo ()
        /// </summary>
        public string AcctNo
        {
            set { _AcctNo = value; }
            get { return _AcctNo; }
        }

        private string _AcctDesc1;
        /// <summary>
        /// AcctDesc1 ()
        /// </summary>
        public string AcctDesc1
        {
            set { _AcctDesc1 = value; }
            get { return _AcctDesc1; }
        }

        private string _AcctType;
        /// <summary>
        /// AcctType ()
        /// </summary>
        public string AcctType
        {
            set { _AcctType = value; }
            get { return _AcctType; }
        }

        private string _JournalDate;
        /// <summary>
        /// JournalDate ()
        /// </summary>
        public string JournalDate
        {
            set { _JournalDate = value; }
            get { return _JournalDate; }
        }

        private Decimal _DBAmt;
        /// <summary>
        /// DBAmt ()
        /// </summary>
        public Decimal DBAmt
        {
            set { _DBAmt = value; }
            get { return _DBAmt; }
        }

        private Decimal _CRAmt;
        /// <summary>
        /// CRAmt ()
        /// </summary>
        public Decimal CRAmt
        {
            set { _CRAmt = value; }
            get { return _CRAmt; }
        }

        private string _AbsValue;
        /// <summary>
        /// AbsValue ()
        /// </summary>
        public string AbsValue
        {
            set { _AbsValue = value; }
            get { return _AbsValue; }
        }

        private string _AcctCtg;
        /// <summary>
        /// AcctCtg ()
        /// </summary>
        public string AcctCtg
        {
            set { _AcctCtg = value; }
            get { return _AcctCtg; }
        }

        private string _PostFlag;
        /// <summary>
        /// PostFlag ()
        /// </summary>
        public string PostFlag
        {
            set { _PostFlag = value; }
            get { return _PostFlag; }
        }

        private string _ApvlFlag;
        /// <summary>
        /// ApvlFlag ()
        /// </summary>
        public string ApvlFlag
        {
            set { _ApvlFlag = value; }
            get { return _ApvlFlag; }
        }

        private string _Idx01;
        /// <summary>
        /// Idx01()
        /// </summary>
        public string Idx01
        {
            set { _Idx01 = value; }
            get { return _Idx01; }
        }

        private string _Idx02;
        /// <summary>
        /// Idx02()
        /// </summary>
        public string Idx02
        {
            set { _Idx02 = value; }
            get { return _Idx02; }
        }

        private string _Idx03;
        /// <summary>
        /// Idx03()
        /// </summary>
        public string Idx03
        {
            set { _Idx03 = value; }
            get { return _Idx03; }
        }

        private string _Idx04;
        /// <summary>
        /// Idx04()
        /// </summary>
        public string Idx04
        {
            set { _Idx04 = value; }
            get { return _Idx04; }
        }

        private string _Idx05;
        /// <summary>
        /// Idx05()
        /// </summary>
        public string Idx05
        {
            set { _Idx05 = value; }
            get { return _Idx05; }
        }

        private string _Idx06;
        /// <summary>
        /// Idx06()
        /// </summary>
        public string Idx06
        {
            set { _Idx06 = value; }
            get { return _Idx06; }
        }

        private string _Idx07;
        /// <summary>
        /// Idx07()
        /// </summary>
        public string Idx07
        {
            set { _Idx07 = value; }
            get { return _Idx07; }
        }

        private string _Index01;
        /// <summary>
        /// Index01()
        /// </summary>
        public string Index01
        {
            set { _Index01 = value; }
            get { return _Index01; }
        }

        private string _Index02;
        /// <summary>
        /// Index02()
        /// </summary>
        public string Index02
        {
            set { _Index02 = value; }
            get { return _Index02; }
        }

        private string _Index03;
        /// <summary>
        /// Index03()
        /// </summary>
        public string Index03
        {
            set { _Index03 = value; }
            get { return _Index03; }
        }

        private string _Index04;
        /// <summary>
        /// Index04()
        /// </summary>
        public string Index04
        {
            set { _Index04 = value; }
            get { return _Index04; }
        }

        private string _Index05;
        /// <summary>
        /// Index05()
        /// </summary>
        public string Index05
        {
            set { _Index05 = value; }
            get { return _Index05; }
        }

        private Decimal _Index06;
        /// <summary>
        /// Index06()
        /// </summary>
        public Decimal Index06
        {
            set { _Index06 = value; }
            get { return _Index06; }
        }

        private string _Index07;
        /// <summary>
        /// Index07()
        /// </summary>
        public string Index07
        {
            set { _Index07 = value; }
            get { return _Index07; }
        }

        private string _ReturnDate;
        /// <summary>
        /// ReturnDate()
        /// </summary>
        public string ReturnDate
        {
            set { _ReturnDate = value; }
            get { return _ReturnDate; }
        }

        private string _VoucherEntryDate;
        /// <summary>
        /// VoucherEntryDate()
        /// </summary>
        public string VoucherEntryDate
        {
            set { _VoucherEntryDate = value; }
            get { return _VoucherEntryDate; }
        }

        private string _CreateUser;
        /// <summary>
        /// CreateUser()
        /// </summary>
        public string CreateUser
        {
            set { _CreateUser = value; }
            get { return _CreateUser; }
        }

        private string _LstChgUser;
        /// <summary>
        /// LstChgUser()
        /// </summary>
        public string LstChgUser
        {
            set { _LstChgUser = value; }
            get { return _LstChgUser; }
        }

        private string _DAcctNo;
        /// <summary>
        /// DAcctNo ()
        /// </summary>
        public string DAcctNo
        {
            set { _DAcctNo = value; }
            get { return _DAcctNo; }
        }

        private string _CAcctNo;
        /// <summary>
        /// CAcctNo ()
        /// </summary>
        public string CAcctNo
        {
            set { _CAcctNo = value; }
            get { return _CAcctNo; }
        }
        #endregion

        /// </summary>
        /// 建構式
        /// </summary>
        public GLA0110TRData()
        {
        }
    }
