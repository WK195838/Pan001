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

public partial class UserControl_PeriodList : System.Web.UI.UserControl
{
    public SysSetting SysSet = new SysSetting();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void defaulPeriod()
    {
        string[] sDlist = SysSet.GetConfigString("SalaryDate").Split(',');
        ddlPeriod.Items.Clear();
        ListItem li;
        for (int i = 0; i < sDlist.Length; i++)
        {
            li = new ListItem();

            li.Text = sDlist[i];
            li.Value = sDlist[i];

            ddlPeriod.Items.Add(li);
        }
    }

    public string selectPeriod
    {
        get
        {
            return ddlPeriod.SelectedItem.Text;
        }
        set
        {
            for (int i = 0; i < ddlPeriod.Items.Count; i++)
            {
                if (ddlPeriod.Items[i].Text == value)
                {
                    ddlPeriod.SelectedIndex = i;
                    break;
                }
            }
        }
    }
    public int selectIndex
    {
        get
        {
            return ddlPeriod.SelectedIndex;
        }
        set
        {
            ddlPeriod.SelectedIndex = value;
        }
    }

    public bool Enabled
    {
        get
        {
            return this.ddlPeriod.Enabled;
        }
        set
        {
            this.ddlPeriod.Enabled = value;
        }
    }
}
