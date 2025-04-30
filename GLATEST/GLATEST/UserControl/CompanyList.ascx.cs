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
    public event SelectedIndexChanged SelectedChanged;       

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
                    //companyList_SelectedIndexChanged(null, null);
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
        

    public string CompanyListClientID()
    {
        return companyList.ClientID;
    }

    public void StyleAdd(string Key, string Value)
    {
        this.companyList.Style.Add(Key, Value);        
    }    
}
