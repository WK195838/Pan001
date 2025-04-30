using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

public partial class PopupDialog : System.Web.UI.Page 
{
    DBManger _MyDBM;
    string cnn = "";
    protected void Page_Load(object sender, EventArgs e)
    { 
        BindData();
    }

    //SysFunctions sFunctions = new SysFunctions();

    private void BindData()
    {
        _MyDBM= new DBManger();
        _MyDBM.New();


        cnn = _MyDBM.GetConnectionString();
        string sql = "exec dbo.sp_GetAcnoIdxPrompt @Company,@CodeID,@CodeCode";
        DataTable dt = new DataTable();
        SqlDataAdapter adpt = new SqlDataAdapter(sql, cnn); 
        try
        {
            adpt.SelectCommand.Parameters.Add("@Company", SqlDbType.Char, 2).Value = Request["Company"];
            adpt.SelectCommand.Parameters.Add("@CodeID", SqlDbType.Char, 2).Value = Request["CodeID"];
            adpt.SelectCommand.Parameters.Add("@CodeCode", SqlDbType.VarChar, 10).Value = txtQuery.Text.Trim();
            
            adpt.Fill(dt);
            string sqlReturn = dt.Rows[0]["SQLString"].ToString();
            //Response.Write(sqlReturn);

            SqlDataSource1.ConnectionString = cnn;
            SqlDataSource1.SelectCommand = sqlReturn;
            SqlDataSource1.DataBind();

            gvEmployee.DataBind();
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
    
    protected void gvEmployee_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //設定顏色
        string OnMouseOverStyle = "this.style.backgroundColor='silver';";
        string OnMouseOutStyle = "this.style.backgroundColor='@BackColor';";
        string rowBackColor = "";

        //((GridView)sender).Columns[0].HeaderText = "代號";  
        //((GridView)sender).Columns[1].HeaderText = "名稱";
        
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
            //    rowBackColor = "#dddddd";  // gvPopupDialog.RowStyle.BackColor.ToString();
            //}

            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].CssClass = "Grid_GridLine";
                e.Row.Cells[i].Style.Add("text-align", "left");
            }

            e.Row.Attributes.Add("onmouseover", OnMouseOverStyle);
            e.Row.Attributes.Add("onmouseout", OnMouseOutStyle.Replace("@BackColor", rowBackColor));
            e.Row.Attributes.Add("style", "cursor:hand;");
        }
    }

    protected void gvEmployee_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            foreach (TableCell cell in e.Row.Cells)
            {
                cell.Attributes.Add("style", "FONT-WEIGHT:normal;");
            }
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //回傳值的事件
            string rValue = "";
            for (int i = 0; i <= 1; i++)
            {
                rValue += e.Row.Cells[i].Text.ToString().Trim() + ",";
            }

            e.Row.Attributes.Add("onclick", "return ReValue('" + rValue + "');");

        }
    }

    protected void imgbtnQuery_Click(object sender, ImageClickEventArgs e)
    {
        BindData();
        
    }
}
