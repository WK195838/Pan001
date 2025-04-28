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

public partial class UserControl_YearList : System.Web.UI.UserControl
{
    public delegate void SelectedIndexChanged(object sender, EventArgs e);
    public event SelectedIndexChanged TextChanged;

    public SysSetting SysSet = new SysSetting();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //init();
        }

        //onchange沒作用,所以改用onkeyup=>按下enter會自動送出
        //ddlYear.Attributes.Add("onchange", "abgne('" + btn.ClientID + "');");
        ddlYear.Attributes.Add("onkeyup", "abgne('" + btn.ClientID + "');");
    }

    public void initList()
    {
        SetYearList(SysSet.YearB, SysSet.YearE, "");
    }

    /// <summary>
    /// 初始化年度下拉單
    /// </summary>
    /// <param name="YearB">起始年度</param>
    /// <param name="YearE">終止年度</param>
    /// <param name="DefYear">預設年度</param>
    public void SetYearList(int YearB, int YearE, string DefYear)
    {
        SetYearList(YearB, YearE, DefYear, "");
    }

    /// <summary>
    /// 初始化年度下拉單
    /// </summary>
    /// <param name="YearB">起始年度</param>
    /// <param name="YearE">終止年度</param>
    /// <param name="DefYear">預設年度</param>
    /// <param name="nonChose">設定不選擇之空項目顯示名稱(若為空值表示無空項目)</param>
    public void SetYearList(int YearB, int YearE, string DefYear, string nonChose)
    {
        ddlYear.Items.Clear();
        ListItem theLI;
        int i = 0;
        if (YearB <= YearE)
        {
            for (i = YearB; i <= YearE; i++)
            {
                theLI = new ListItem();
                theLI.Text = i.ToString();
             //   if (SysSet.isTWCalendar)
                if (YearB <1900 && YearE<1900)
                    theLI.Value = (i + 1911).ToString();
                ddlYear.Items.Add(theLI);
            }
        }
        else
        {
            for (i = YearB; i >= YearE; i--)
            {
                theLI = new ListItem();
                theLI.Text = i.ToString();
             //   if (SysSet.isTWCalendar )
                if (YearB < 1900 && YearE < 1900)
                    theLI.Value = (i + 1911).ToString();
                ddlYear.Items.Add(theLI);
            }
        }

        if (!string.IsNullOrEmpty(nonChose))
        {
            theLI = new ListItem();
            theLI.Text = nonChose;
            theLI.Value = "";
            ddlYear.Items.Insert(0, theLI);
        }

        if (!string.IsNullOrEmpty(DefYear))
        {
            if (SysSet.isTWCalendar)
                SelectYear = DefYear;
            else
                SelectADYear = DefYear;
        }
        else if (string.IsNullOrEmpty(nonChose))
            SelectADYear = DateTime.Today.Year.ToString();
    }

    public string SelectADYear
    {
        get
        {
            if (ddlYear.SelectedItem != null)
                return ddlYear.SelectedItem.Value;
            else
                return "";
        }
        set
        {
            for (int i = 0; i < ddlYear.Items.Count; i++)
            {
                if (ddlYear.Items[i].Value == value)
                {
                    ddlYear.SelectedIndex = i;
                    break;
                }
            }
                
        }
    }

    public string SelectYear
    {
        get
        {
            return ddlYear.SelectedItem.Text;
        }
        set
        {
            for (int i = 0; i < ddlYear.Items.Count; i++)
            {
                if (ddlYear.Items[i].Text == value)
                {
                    ddlYear.SelectedIndex = i;
                    break;
                }
            }
        }
    }

    public bool AutoPostBack
    {
        get
        {
            return ddlYear.AutoPostBack;
        }
        set
        {
            ddlYear.AutoPostBack = value;
        }
    }

    public string YListClientID()
    {
        return ddlYear.ClientID;
    }

    public bool PostBack
    {
        get
        {
            return ddlYear.AutoPostBack;
        }
        set
        {
            ddlYear.AutoPostBack = value;
        }
    }

    public bool Enabled
    {
        get
        {
            return this.ddlYear.Enabled;
        }
        set
        {
            this.ddlYear.Enabled = value;
        }
    }

    protected void ddlYear_TextChanged(object sender, EventArgs e)
    {
        if (TextChanged != null)
            TextChanged(this, e);
    }
}
