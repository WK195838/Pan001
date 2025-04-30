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

public partial class Prompt_List : System.Web.UI.Page
{
    public SysSetting SysSet = new SysSetting();
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    string theValue, theTable, theRetColum, theShowColums, theOrderColums;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _MyDBM = new DBManger();
        _MyDBM.New();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //將Javascript動態引用進MasterPage中(直接寫在頁面上可能會有路徑問題)
        //用於一般常用JS
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "A", Page.ResolveUrl("~/Pages/pagefunction.js").ToString());

        Navigator1.BindGridView = GridView1;

        #region 取得參數
        if (Request["theTable"] != null)
        {
            theTable = Request["theTable"].ToString();
        }

        if (Request["theRetColum"] != null)
        {
            theRetColum = Request["theRetColum"].ToString();
        }

        if (Request["theShowColums"] != null)
        {
            theShowColums = Request["theShowColums"].ToString();
        }

        if (Request["theOrderColums"] != null)
        {
            theOrderColums = Request["theOrderColums"].ToString();
        }
        #endregion

        if (Request["theValue"] != null)
        {
            theValue = Request["theValue"].ToString();
            Session["SelectValue"] = theValue;
        }

        if (!Page.IsPostBack)
        {
            if (string.IsNullOrEmpty(theTable) || string.IsNullOrEmpty(theRetColum) || string.IsNullOrEmpty(theShowColums))
            {
                ShowMsgBox1.Message = SysSet.ErrMsg("Parameterless");
            }
            else
            {
                BindData();
            }
        }

        StyleTitle1.ShowBackToPre = false;
        Navigator1.DataBind();
    }
    
    private void BindData()
    {
        string[] theRetList = theRetColum.Split(new char[] { ',' });
        string[] theShowList = theShowColums.Split(new char[] { ',' });
        string Ssql;

        Ssql = "SELECT " + theRetColum + "," + theShowColums + " FROM " + theTable + " Where 0=0";
        
        if (tbQuery1.Text.Length > 0)
        {            
            Ssql += string.Format(" And " + theRetList[0] + " like '%{0}%'", tbQuery1.Text);
        }

        if (tbQuery2.Text.Length > 0)
        {
            string strTemp = "";
            for (int i = 0; i < theShowList.Length; i++)
            {
                strTemp = theShowList[i];
                if (strTemp.ToUpper().Contains(" AS "))
                    strTemp = strTemp.Substring(0, strTemp.ToUpper().IndexOf(" AS "));

                if (i == 0)
                {
                    Ssql += string.Format(" And ({0} like '%{1}%'", strTemp, tbQuery2.Text);
                }
                else
                {
                    Ssql += string.Format(" Or {0} like '%{1}%' ", strTemp, tbQuery2.Text);
                }
            }
            Ssql += ")";
        }

        //SDS_GridView.SelectCommand = Ssql + " Order By " + theShowColums;
        if (!string.IsNullOrEmpty(theOrderColums))
            Ssql += " Order By " + theOrderColums;
        GridView1.DataSource = _MyDBM.ExecuteDataTable(Ssql);
        GridView1.DataBind();

        Navigator1.BindGridView = GridView1;
        //Navigator1.SQL = SDS_GridView.SelectCommand;
        Navigator1.DataBind();

        if (GridView1.Rows.Count == 0)
        {
            Panel_Empty.Visible = true;
        }
        else
        {
            Panel_Empty.Visible = false;
        }
    }

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            #region 設定回傳與顯示的欄位
            if (theRetColum == null)
            {
                if (Request["theRetColum"] != null)
                {//指定要回傳的欄位(允許複數,以逗號分隔)
                    theRetColum = Request["theRetColum"].ToString();
                }
                else
                {
                    theRetColum = "";
                }

                if (Request["theShowColums"] != null)
                {//指定要顯示的欄位(允許複數,以逗號分隔;若與要回傳的欄位重複時,顯示欄位需用AS關鍵字另取別名)
                    theShowColums = Request["theShowColums"].ToString();
                }
                else
                {
                    theShowColums = "";
                }
            }
            #endregion
            int i = 0;
            string strTemp = "", ListTemp = "";
            string[] theRetList = theRetColum.Split(new char[] { ',' });

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].CssClass = "paginationRowEdgeLl";
                e.Row.Cells[e.Row.Cells.Count - 1].CssClass = "paginationRowEdgeRl";

                #region 設定標題列
                ListTemp = "";
                string[] theShowList = theShowColums.Split(new char[] { ',' });
                for (i = 0; i < theShowList.Length; i++)
                {//列出要顯示的欄位
                    strTemp = theShowList[i];
                    if (strTemp.ToUpper().Contains(" AS "))
                        strTemp = strTemp.Substring(strTemp.ToUpper().IndexOf(" AS ") + 4);
                    ListTemp += "," + strTemp;
                }
                string[] theTitle = ListTemp.Split(new char[] { ',' });

                for (i = e.Row.Cells.Count - 1; i >= 0; i--)
                {//修改欄位顯示名稱
                    if (theTitle.Length - (e.Row.Cells.Count - i) > 0)
                    {
                        DataTable DT = _MyDBM.ExecuteDataTable("Select dbo.GetColumnTitle('" + theTable + "','" + theTitle[theTitle.Length - (e.Row.Cells.Count - i)] + "')");
                        if (DT.Rows.Count > 0)
                        {
                            e.Row.Cells[i].Text = DT.Rows[0][0].ToString().Trim();
                            if (!Label2.Text.Contains("搜尋:"))
                            {
                                if (!Label2.Text.Contains(e.Row.Cells[i].Text))
                                {
                                    if (Label2.Text.Length > 0)
                                        Label2.Text = e.Row.Cells[i].Text + "、" + Label2.Text;
                                    else
                                        Label2.Text = e.Row.Cells[i].Text;
                                }
                            }
                        }
                    }
                    else
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }

                DataTable DT2 = _MyDBM.ExecuteDataTable("Select dbo.GetColumnTitle('" + theTable + "','" + theRetList[0] + "')");
                if (DT2.Rows.Count > 0)
                {
                    e.Row.Cells[1].Text = DT2.Rows[0][0].ToString().Trim();                    
                }

                if (Label1.Text.Length == 0)
                {
                    Label1.Text = "搜尋回傳值:"; //+ e.Row.Cells[0].Text;
                }
                if (!Label2.Text.Contains("搜尋:"))
                {
                    Label2.Text = "搜尋:<BR>" + Label2.Text;
                }
                
                //for (i = 0; i < theRetList.Length; i++)
                //    e.Row.Cells[i].Visible = false;
                #endregion
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "setnewcolor(this);");
                e.Row.Attributes.Add("onmouseout", "setoldcolor(this);");
                i = 0;
                for (i = 0; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].CssClass = "Grid_GridLine";
                }

                i = e.Row.Cells.Count - 1;
                if (i > 0)
                {
                    e.Row.Cells[i - 1].Style.Add("text-align", "right");
                    e.Row.Cells[i - 1].Style.Add("width", "100px");
                }
                e.Row.Cells[i].Style.Add("text-align", "left");

                #region 設定顯示資料
                for (i = 1; i <= theRetList.Length; i++)
                    e.Row.Cells[i].Visible = false;
                #endregion
            }
            else if (e.Row.RowType == DataControlRowType.Pager)
            {
                e.Row.Visible = false;
            }
        }
        catch
        { }
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //回傳值的事件
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //傳回指定欄位值
            //string rValue = "";
            //for (int i = 0; i < e.Row.Cells.Count; i++)
            //{
            //    rValue += e.Row.Cells[i].Text.ToString().Trim() + ",";
            //}
            //e.Row.Attributes.Add("onclick", "return ReValue('" + rValue + "');");
            string strSelect = "";
            string[] theRetList = theRetColum.Split(new char[] { ',' });
            for (int i = 0; i < theRetList.Length; i++)
            {
                strSelect += e.Row.Cells[i].Text.Trim() + ":";
            }            
            e.Row.Attributes.Add("onclick", "window.returnValue = '" + strSelect + ":" + Session["SelectValue"] + "';window.close();");
        }
    }
    
    protected void btnQuery_Click(object sender, ImageClickEventArgs e)
    {
        BindData();
    }
}
