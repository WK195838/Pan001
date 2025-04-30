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

public partial class UserControl_CodeList : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// 設定代碼List
    /// </summary>
    /// <param name="CodeID"></param>
    public void SetCodeList(string CodeID)
    {
        SetCodeList(CodeID, 5, "請選擇");
    }

    /// <summary>
    /// 設定下拉單(資料來源為指定代碼)
    /// </summary>
    /// <param name="CodeID">代碼編號</param>
    /// <param name="ShowKind">設定格式(0:代碼,1:名稱,2:代碼-名稱;3:代碼,4:名稱,5:代碼-名稱;當ShowKind=3,4,5時會顯示"請選擇")</param>
    public void SetCodeList(string CodeID, int ShowKind)
    {
        SetCodeList(CodeID, ShowKind, "請選擇");
    }

    /// <summary>
    /// 設定下拉單(資料來源為指定代碼)
    /// </summary>
    /// <param name="CodeID">代碼編號</param>
    /// <param name="ShowKind">設定格式(0:代碼,1:名稱,2:代碼-名稱;3:代碼,4:名稱,5:代碼-名稱;當ShowKind=3,4,5時會顯示"strDefItem"設定的文字)</param>
    /// <param name="strDefItem">設定未選擇下拉單時之顯示</param>
    public void SetCodeList(string CodeID, int ShowKind, string strDefItem)
    {
        if (ddlCodeList != null)
        {
            ddlCodeList.Items.Clear();
        }
        else
        {
            ddlCodeList = new DropDownList();
        }
        DataTable CL = DBSetting.CodeList(CodeID);
        CreateList(CL, ShowKind, strDefItem);
    }

    /// <summary>
    /// 設定下拉單(資料來源為指定TABLE)
    /// </summary>
    /// <param name="TableName">TABLE</param>
    /// <param name="CodeField">下拉單的值要用的欄位</param>
    /// <param name="showField">下拉單顯示文字要用的欄位</param>
    /// <param name="strQueryFor">下拉單條件</param>
    /// <param name="ShowKind">設定格式(0:代碼,1:名稱,2:代碼-名稱;3:代碼,4:名稱,5:代碼-名稱;當ShowKind=3,4,5時會顯示"請選擇")</param>
    public void SetDTList(string TableName, string CodeField, string showField, string strQueryFor, int ShowKind)
    {
        SetDTList(TableName, CodeField, showField, strQueryFor, ShowKind, "請選擇");        
    }

    /// <summary>
    /// 設定下拉單(資料來源為指定TABLE)
    /// </summary>
    /// <param name="TableName">TABLE</param>
    /// <param name="CodeField">下拉單的值要用的欄位</param>
    /// <param name="showField">下拉單顯示文字要用的欄位</param>
    /// <param name="strQueryFor">下拉單條件</param>
    /// <param name="ShowKind">設定格式(0:代碼,1:名稱,2:代碼-名稱;3:代碼,4:名稱,5:代碼-名稱;當ShowKind=3,4,5時會顯示"strDefItem"設定的文字)</param>
    /// /// <param name="strDefItem">設定未選擇下拉單時之顯示</param>
    public void SetDTList(string TableName, string CodeField, string showField, string strQueryFor, int ShowKind, string strDefItem)
    {
        if (ddlCodeList != null)
        {
            ddlCodeList.Items.Clear();
        }
        else
        {
            ddlCodeList = new DropDownList();
        }
        DataTable CL = DBSetting.DTList(TableName, CodeField, showField, strQueryFor);
        CreateList(CL, ShowKind, strDefItem);
    }

    protected void CreateList(DataTable CL, int ShowKind, string strDefItem)
    {
        ListItem LI = new ListItem();

        if (CL != null)
        {
            for (int i = 0; i < CL.Rows.Count; i++)
            {
                LI = new ListItem();
                //0:代碼,1:名稱,2:代碼-名稱
                LI.Text = CL.Rows[i][ShowKind % 3].ToString().Trim();
                LI.Value = CL.Rows[i][0].ToString().Trim();

                ddlCodeList.Items.Add(LI);
            }

            if ((ShowKind / 3) >= 1)
            {
                //加入空的選項
                LI = new ListItem();
                //0:代碼,1:名稱,2:代碼-名稱
                LI.Text = strDefItem;
                LI.Value = "";
                ddlCodeList.Items.Insert(0, LI);
            }
        }
    }

    /// <summary>
    /// 傳回選擇項目名稱
    /// </summary>
    public string SelectedCodeName
    {
        get
        {
            if (ddlCodeList.SelectedItem != null)
                return ddlCodeList.SelectedItem.Text;
            else
                return "";
        }
        set
        {
            string strValue = "";
            if (!string.IsNullOrEmpty(value))
            {
                strValue = value.Trim();
            }
            int i = 0;
            for (i = 0; i < ddlCodeList.Items.Count; i++)
            {
                if (ddlCodeList.Items[i].Text.Trim() == strValue)
                {
                    ddlCodeList.SelectedIndex = i;
                    break;
                }
            }

            if (ddlCodeList.SelectedItem != null)
            {
                if ((i == ddlCodeList.Items.Count) && (ddlCodeList.SelectedItem.Text != strValue))
                {
                    ddlCodeList.SelectedItem.Text = strValue;
                }
            }
        }
    }

    /// <summary>
    ///傳回選擇代碼
    /// </summary>
    public string SelectedCode
    {
        get
        {
            if (ddlCodeList.SelectedItem != null)
                return ddlCodeList.SelectedItem.Value;
            else
                return "";
        }
        set
        {
            string strValue = "";
            if (!string.IsNullOrEmpty(value))
            {
                strValue = value.Trim();
            }
            for (int i = 0; i < ddlCodeList.Items.Count; i++)
            {
                if (ddlCodeList.Items[i].Value.Trim() == strValue)
                {
                    ddlCodeList.SelectedIndex = i;
                    break;
                }
            }
        }
    }

    public string Text
    {
        get
        {
            return SelectedCode;
        }
        set
        {
            SelectedCode = value;
        }
    }

    public bool Enabled
    {
        get
        {
            return this.ddlCodeList.Enabled;
        }
        set
        {
            this.ddlCodeList.Enabled = value;
        }
    }

    public string CLClientID()
    {
        return ddlCodeList.ClientID;
    }

    public bool AutoPostBack
    {
        get
        {
            return this.ddlCodeList.AutoPostBack;
        }
        set
        {
            this.ddlCodeList.AutoPostBack = value;
        }
    }

    public void StyleAdd(string Key, string Value)
    {
        this.ddlCodeList.Style.Add(Key, Value);
    }

    //public EventHandler SelectedIndexChanged
    //{
    //    get
    //    {
    //        return null;
    //    }
    //    set
    //    {
    //        ddlCodeList.SelectedIndexChanged += value;
    //    }
    //}

    //For FW4.0改用下列寫法
    public event EventHandler SelectedIndexChanged;

    protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
	{
        if (SelectedIndexChanged != null)    // 這一行一定要加
            SelectedIndexChanged(sender, e);
	}
}
