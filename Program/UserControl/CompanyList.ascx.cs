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


public partial class UserControl_CompanyList : System.Web.UI.UserControl
{
    public delegate void SelectedIndexChanged(object sender, SelectEventArgs e);
    public event SelectedIndexChanged SelectedIndex;

    

    DBManger _MyDBM;
    TextModeEnum _TextMode = TextModeEnum.NumShortName;

    public class SelectEventArgs : EventArgs
    {
        /// <summary>
        /// 目前選取參數值
        /// </summary>
        public string SelectValue;
        /// <summary>
        /// 目前選取文字
        /// </summary>
        public string SelectText;

        public ListItemCollection Personnel_Master;
        public ListItemCollection Department_Basic;
    }

    /// <summary>
    /// 文字狀態
    /// </summary>
    public TextModeEnum TextMode
    {
        get
        {
            return this._TextMode;
        }
        set
        {
            this._TextMode = value;
        }
    }

    /// <summary>
    /// 目前選取參數值
    /// </summary>
    public string SelectValue
    {
        get
        {
            return this.companyList.SelectedItem.Value;
        }
        set 
        {
            for (int i = 0; i < companyList.Items.Count; i++)
            {
                if (companyList.Items[i].Value == value)
                {
                    companyList.SelectedIndex = i;
                    companyList_SelectedIndexChanged(null, null);
                    break;
                }
            }            
        }
    }

    /// <summary>
    /// 目前選取文字
    /// </summary>
    public string SelectText
    {
        get
        {
            return this.companyList.SelectedItem.Text;
        }

    }

    public bool AutoPostBack
    { 
        get
        {
            return this.companyList.AutoPostBack ;
        }
        set 
        {
            this.companyList.AutoPostBack = value;
        }
    }

    public bool Enabled
    {
        get
        {
            return this.companyList.Enabled;
        }
        set
        {
            this.companyList.Enabled = value;
        }
    }

    public ListItemCollection Personnel_Master = new ListItemCollection ();
    public ListItemCollection Department_Basic = new ListItemCollection ();
    public ListItemCollection LeaveType_Basic = new ListItemCollection();

    public enum TextModeEnum
    {
        /// <summary>
        /// 顯示公司編號
        /// </summary>
        Num =0,
        /// <summary>
        /// 顯示公司名稱
        /// </summary>
        Name = 1,
        /// <summary>
        /// 顯示公司編號及公司簡稱
        /// </summary>
        NumShortName=2,
        /// <summary>
        /// 顯示公司編號及公司名稱
        /// </summary>
        NumName =3
    }



    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        if (!IsPostBack)
        {
            loadCompany();
        }
    }

    private void loadCompany()
    {
        companyList.Items.Clear();
        companyList.Items.Add("");

        _MyDBM = new DBManger();
        _MyDBM.New();
        DataTable DT = _MyDBM.ExecuteDataTable("SELECT * FROM Company Order by Company");

        for (int i = 0; i < DT.Rows.Count; i++)
        {
            ListItem li = new ListItem();

            string Company = DT.Rows[i]["Company"].ToString();
            string CompanyShortName = DT.Rows[i]["CompanyShortName"].ToString();
            string CompanyName = DT.Rows[i]["CompanyName"].ToString();

            li.Value = Company;

            switch (_TextMode)
            {
                case TextModeEnum.Num: li.Text = Company; break;
                case TextModeEnum.Name: li.Text = CompanyName; break;
                case TextModeEnum.NumShortName: li.Text = Company + " - " + CompanyShortName; break;
                case TextModeEnum.NumName:
                default: li.Text = Company + " - " + CompanyName; break;
            }


            companyList.Items.Add(li);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (companyList.Items.Count == 0) loadCompany();
    }
        
    protected void companyList_SelectedIndexChanged(object sender, EventArgs e)
    {
        _MyDBM = new DBManger();
        _MyDBM.New();

        Personnel_Master.Clear();
        Department_Basic.Clear();
        LeaveType_Basic.Clear();

        #region 人事
        DataTable DT = _MyDBM.ExecuteDataTable("SELECT EmployeeId,EmployeeName FROM Personnel_Master Where Company ='" + SelectValue + "' Order by EmployeeId");
        for (int i = 0; i < DT.Rows.Count; i++)
        {
            ListItem li = new ListItem();
            li.Value = DT.Rows[i][0].ToString();

            string EmployeeId = DT.Rows[i]["EmployeeId"].ToString();
            string EmployeeName = DT.Rows[i]["EmployeeName"].ToString();

            switch (_TextMode)
            {
                case TextModeEnum.Num: li.Text = EmployeeId; break;
                case TextModeEnum.Name: li.Text = EmployeeName; break;
                case TextModeEnum.NumShortName:
                case TextModeEnum.NumName:
                default: li.Text = EmployeeId + " - " + EmployeeName; break;
            }

            li.Value = EmployeeId.Trim();

            Personnel_Master.Add(li);
        }
        #endregion

        #region 部門
        DT = _MyDBM.ExecuteDataTable("SELECT DepCode,DepName FROM Department Where Company ='" + SelectValue + "' Order by DepCode");
        for (int i = 0; i < DT.Rows.Count; i++)
        {
            ListItem li = new ListItem();
            string DepCode = DT.Rows[i]["DepCode"].ToString();
            string DepName = DT.Rows[i]["DepName"].ToString();

       

            switch (_TextMode)
            {
                case TextModeEnum.Num: li.Text = DepCode; break;
                case TextModeEnum.Name: li.Text = DepName; break;
                case TextModeEnum.NumShortName:
                case TextModeEnum.NumName:
                default: li.Text = DepCode + " - " + DepName; break;
            }

            li.Value = DepCode.Trim();

            Department_Basic.Add(li);
        }
        #endregion

        #region 假期
        DT = _MyDBM.ExecuteDataTable("select Leave_Id ,Leave_Desc from LeaveType_Basic where Company='" + SelectValue + "' Order by Leave_Id");
        for (int i = 0; i < DT.Rows.Count; i++)
        {
            ListItem li = new ListItem();
            string Leave_Id = DT.Rows[i]["Leave_Id"].ToString();
            string Leave_Desc = DT.Rows[i]["Leave_Desc"].ToString();

            li.Value = Leave_Id;

            switch (_TextMode)
            {
                case TextModeEnum.Num: li.Text = Leave_Id; break;
                case TextModeEnum.Name: li.Text = Leave_Desc; break;
                case TextModeEnum.NumShortName:
                case TextModeEnum.NumName:
                default: li.Text = Leave_Id + " - " + Leave_Desc; break;
            }
            li.Value = Leave_Id.Trim();
            LeaveType_Basic.Add(li);
        }
        #endregion

        SelectEventArgs EventArgs = new SelectEventArgs();
        EventArgs.Department_Basic = Department_Basic;
        EventArgs.Personnel_Master = Personnel_Master;
        EventArgs.SelectText = SelectText;
        EventArgs.SelectValue = SelectValue;

        if (SelectedIndex != null) 
            SelectedIndex(this, EventArgs);

         
    }

    public string CompanyListClientID()
    {
        return companyList.ClientID;
    }
}
