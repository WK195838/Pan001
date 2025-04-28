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

public partial class UserControl_MonthList : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void initList()
    {
        MonthList.SelectedIndex = DateTime.Today.Month - 1;
    }

    public void SetSpecialList(string DefMonth, string nonChose)
    {
        if ((!string.IsNullOrEmpty(nonChose)) && (!string.IsNullOrEmpty(MonthList.Items[0].Value)))
        {
            ListItem theLI = new ListItem();
            theLI.Text = nonChose;
            theLI.Value = "";
            MonthList.Items.Insert(0, theLI);
        }

        if (!string.IsNullOrEmpty(DefMonth))
        {
            try
            {
                SelectMonth = (Convert.ToInt32(DefMonth) - 1).ToString().PadLeft(2, '0');
            }
            catch { }            
        }
    }

    public void SetInvMonth ( )
    {
        MonthList.Items.Clear ( );
        for ( int i = 1 ; i < 12 ; i+=2 )
        {
            string [ ] Month = { i.ToString ( ).PadLeft ( 2 , '0' ) , ( i + 1 ).ToString ( ).PadLeft ( 2 , '0' )};
            MonthList.Items.Add ( new ListItem ( Month [ 0 ] + "-" + Month [ 1 ] + "月" , Month [ 0 ] ) );
        }
    }


    public string SelectMonth
    {
        get
        {
            return MonthList.SelectedItem.Text;
        }
        set
        {
            for (int i = 0; i < MonthList.Items.Count; i++)
            {
                if (MonthList.Items[i].Text == value)
                {
                    MonthList.SelectedIndex = i;
                    break;
                }
            }
        }
    }

    public int SelectedMonth
    {
        get
        {
            int M = 0;
            try
            {
                M = Convert.ToInt32(MonthList.SelectedItem.Value);
            }
            catch { }

            return M;
        }
        set
        {
            for (int i = 0; i < MonthList.Items.Count; i++)
            {
                if ( MonthList.Items [ i ].Value == value.ToString ( ).PadLeft ( 2 , '0' ) )
                {
                    MonthList.SelectedIndex = i;
                    break;
                }
            }
        }
    }



    public int SelectedIndex
    {
        get
        {
            return MonthList.SelectedIndex + 1;
        }
        set
        {
            MonthList.SelectedIndex = value - 1;
        }
    }

    public string MListClientID()
    {
        return MonthList.ClientID;
    }

    public bool AutoPostBack
    {
        get
        {
            return MonthList.AutoPostBack;
        }
        set
        {
            MonthList.AutoPostBack = value;
        }
    }

    public bool Enabled
    {
        get
        {
            return this.MonthList.Enabled;
        }
        set
        {
            this.MonthList.Enabled = value;
        }
    }
}
