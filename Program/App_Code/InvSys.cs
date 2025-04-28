using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// InvSys 的摘要描述
/// </summary>
public class InvSys
{
    UserInfo _UserInfo = new UserInfo ( );

	public InvSys()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
	}

    /// <summary>
    /// ListItem 工具，會回傳設定好的 ListItem
    /// </summary>
    /// <param name="name">設定名稱</param>
    /// <param name="value">設定值</param>
    /// <returns>ListItem</returns>
    public ListItem Li ( string name , string value )
    {
        ListItem li = new ListItem ();
        li.Text = name;
        li.Value = value;
        return li;
    }


    /// <summary>
    /// 快速設定開新視窗的javascript，需要輸入物件、路徑與視窗大小的設定值
    /// </summary>
    /// <param name="Url"></param>
    /// <param name="WinSize"></param>
    /// <returns></returns>
    public void SetWindowOpen ( object sender , string Url , string width , string height )
    {

        int top = (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - int.Parse ( height )) / 2;
        int left = (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - int.Parse(width)) / 2 ;
        top = top > 0 ? top : 10;
        left = left > 0 ? left : 10;

        string Str = "javascript:var win =window.open('" + Url + "','','width=" + width + ",height=" + height + ",top=" + top + ",left="+ left +",scrollbars=no,resizable=yes');";
        
        switch ( sender.GetType ( ).Name )
        {
            case "Button":
                Button BT = ( Button ) sender;
                BT.Attributes.Add ( "onclick" , Str );
                break;
            case "LinkButton":
                LinkButton LB = ( LinkButton ) sender;
                LB.Attributes.Add ( "onclick" , Str );
                break;
            case "ImageButton":
                ImageButton IB = ( ImageButton ) sender;
                IB.Attributes.Add ( "onclick" , Str );
                break;
        }
    }

    public bool CheckDt ( DataTable Dt )
    {
        return ( Dt != null && Dt.Rows.Count > 0 ) ? true : false;
    }

    /// <summary>
    /// GridView 快速設定
    /// </summary>
    /// <param name="GV"></param>
    public void SetGVStyle ( GridView GV )
    {
        GV.AllowPaging = true;
        GV.AllowSorting = true;
        GV.AutoGenerateColumns = false;
        GV.CellPadding = 0;
        GV.CellSpacing = 0;
        GV.CssClass = "GridView";
        GV.Attributes.Add ( "bordercolor" , "#666666" );
        GV.GridLines = GridLines.Both;
        GV.PagerSettings.Position = PagerPosition.Bottom;
        GV.PagerStyle.HorizontalAlign = HorizontalAlign.Center;
        GV.PagerStyle.VerticalAlign = VerticalAlign.Middle;
    }

    /// <summary>
    /// DetilsView 快速設定，Mode A 、 U
    /// </summary>
    /// <param name="DV"></param>
    /// <param name="Mode"></param>
    public void SetDVStyle ( DetailsView DV , string Mode )
    {
        DV.AutoGenerateRows = false;
        DV.CellPadding = 0;
        DV.CellSpacing = 0;
        DV.CssClass = "DV";
        DV.AlternatingRowStyle.CssClass = "DVA";

        switch ( Mode )
        {
            case "A":
                DV.DefaultMode = DetailsViewMode.Insert;
                break;
            case "U":
                DV.DefaultMode = DetailsViewMode.Edit;
                break;
        }
    }

    public string FormatDate (string Date )
    {
        if ( Date.Contains ( "/" ) )
        {
        string[] str= Date.Split('/');
        if ( str[ 0 ].Length < 4 )
            str[ 0 ] = int.Parse ( str[ 0 ] ) + 1911 + "";
        return str[0] +"/"+ str[1] +"/"+ str[2];
        }
        else
        {
            return Date;
        }

    }

    public string RocDare ( string Date )
    {
        if ( Date.Contains ( "/" ) )
        {
            string[ ] str = Date.Split ( '/' );
            if ( str[ 0 ].Length > 3 )
                str[ 0 ] = int.Parse ( str[ 0 ] ) - 1911 + "";
            return str[ 0 ] + "/" + str[ 1 ] + "/" + str[ 2 ];
        }
        else
        {
            return Date;
        }
    }

    /// <summary>
    /// 轉換至民國年
    /// </summary>
    /// <param name="Date"></param>
    /// <returns></returns>
    public string DateTransform ( string Date )
    {
    
        if ( Date.Contains ( "/" ) )
        {
            string [ ] str = Date.Split ( '/' );
            if ( str [ 0 ].Length > 3 )
                str [ 0 ] = int.Parse ( str [ 0 ] ) - 1911 + "";
            return str [ 0 ] + "/" + str [ 1 ] + "/" + str [ 2 ];
        }
        else
        {
            return Date;
        }
    }


}