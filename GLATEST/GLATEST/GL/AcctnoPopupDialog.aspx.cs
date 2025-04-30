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

public partial class AcctnoPopupDialog : System.Web.UI.Page
{
    UserInfo _UserInfo = new UserInfo();
   // string _ProgramId = "";
    DBManger _MyDBM;
    string cnn = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        _MyDBM = new DBManger();
        _MyDBM.New();
        cnn = _MyDBM.GetConnectionString();

        string sql = "SELECT DISTINCT [AcctNo], [AcctDesc1], [AcctType], [AcctCtg], [ASpecialAcct]";
          sql += ", [Idx01], [Idx02], [Idx03], [Idx04], [Idx05], [Idx06], [Idx07]";
          sql += ", [Inputctl1], [Inputctl2], [Inputctl3], [Inputctl4], [Inputctl5], [Inputctl6], [Inputctl7]";
          sql += ", [Idx01Name]=dbo.fnGetAcnoIdx(@Company,[Idx01]), [Idx02Name]=dbo.fnGetAcnoIdx(@Company,[Idx02])";
          sql += ", [Idx03Name]=dbo.fnGetAcnoIdx(@Company,[Idx03]), [Idx04Name]=dbo.fnGetAcnoIdx(@Company,[Idx04])";
          sql += ", [Idx05Name]=dbo.fnGetAcnoIdx(@Company,[Idx05]), [Idx06Name]=dbo.fnGetAcnoIdx(@Company,[Idx06])";
          sql += ", [Idx07Name]=dbo.fnGetAcnoIdx(@Company,[Idx07])";
          sql += ", [Idx01YN]=dbo.fnGetAcnoIdxYN(@Company,[Idx01]), [Idx02YN]=dbo.fnGetAcnoIdxYN(@Company,[Idx02])";
          sql += ", [Idx03YN]=dbo.fnGetAcnoIdxYN(@Company,[Idx03]), [Idx04YN]=dbo.fnGetAcnoIdxYN(@Company,[Idx04])";
          sql += ", [Idx05YN]=dbo.fnGetAcnoIdxYN(@Company,[Idx05]), [Idx06YN]=dbo.fnGetAcnoIdxYN(@Company,[Idx06])";
          sql += ", [Idx07YN]=dbo.fnGetAcnoIdxYN(@Company,[Idx07])";
          sql += ", [Company], [AcctClass] ";
          sql += " FROM [GLAcctDef] ";
          sql += " WHERE (([AcctClass] = @AcctClass) AND ([Company] = @Company)) ";
        if (txtQuery.Text.Trim() != "")
            sql += " AND ([AcctNo] like '%" + txtQuery.Text.Trim() + "%' or [AcctDesc1] like '%" + txtQuery.Text.Trim() + "%') ";
          sql += " ORDER BY [AcctNo]";
        try
        {
            if (!Page.IsPostBack)
            {
                SqlDSPopupDialog.SelectParameters.Add("AcctClass", "1");
                SqlDSPopupDialog.SelectParameters.Add("Company", Request["Company"]);
            }

            SqlDSPopupDialog.ConnectionString = cnn;
            SqlDSPopupDialog.SelectCommand = sql;
            SqlDSPopupDialog.DataBind();
            gvPopupDialog.DataBind();
        }
        catch (InvalidCastException e)
        {
            throw (e);
        }
        finally
        {
            //dt.Dispose;
            //adpt.Dispose;
        }
    }

    //GridView中要將隱藏的欄位值取出來最好將這些欄位放在後面
    protected void gvPopupDialog_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            foreach (TableCell cell in e.Row.Cells)
            {
                cell.Attributes.Add("style", "FONT-WEIGHT:normal;");
            }
           
            //隱藏表頭 
            for (int i = 5; i <= 34; i++)
            {
                e.Row.Cells[i].Visible = false;
            }
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //隱藏資料
            for (int i = 5; i <= 34; i++)
            {
                e.Row.Cells[i].Visible = false;
            }

            //回傳值的事件
            string rValue = "";
            for (int i = 0; i <= 34; i++)
            {
                rValue += e.Row.Cells[i].Text.ToString().Trim() + ",";
            }

            e.Row.Attributes.Add("onclick", "return ReValue('" + rValue + "');");
            
        }
    }
    protected void gvPopupDialog_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //設定顏色
        string OnMouseOverStyle = "this.style.backgroundColor='silver';";
        string OnMouseOutStyle = "this.style.backgroundColor='@BackColor';";
        string rowBackColor = "";

        ((GridView)sender).Columns[0].HeaderText = "科目";  //((GridView)sender).Columns[i].HeaderText.Replace("▲", "");
        ((GridView)sender).Columns[1].HeaderText = "科目名稱";
        ((GridView)sender).Columns[2].HeaderText = "類別";
        ((GridView)sender).Columns[3].HeaderText = "屬性";
        ((GridView)sender).Columns[4].HeaderText = "特定";

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //if (e.Row.RowState == DataControlRowState.Alternate)
            //{
            //    //rowBackColor = "#d9f8cb" 
            //    rowBackColor = "#ffffff";   //gvPopupDialog.AlternatingRowStyle.BackColor.ToString();
            //}   
            //else 
            //{
            //    //rowBackColor = "#eef8cb"
            //    rowBackColor = "#eff3fb";  // gvPopupDialog.RowStyle.BackColor.ToString();
            //}
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].CssClass = "Grid_GridLine";
               
            }


            e.Row.Attributes.Add("onmouseover", OnMouseOverStyle);
            e.Row.Attributes.Add("onmouseout", OnMouseOutStyle.Replace("@BackColor", rowBackColor));
            e.Row.Attributes.Add("style", "cursor:hand;");
        }
    }
    protected void imgbtnQuery_Click(object sender, ImageClickEventArgs e)
    {
        BindData();
    }
}
