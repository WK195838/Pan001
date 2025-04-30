using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using PanPacificClass;

public partial class GLA0150 : System.Web.UI.Page
{
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _SystemId = "EBOSGL";
    string _ProgramId = "GLA0150";
    DBManger _MyDBM;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        //Page.Theme = "Theme_09";
        //if (Session["Theme"] != null)
        //    Page.Theme = Session["Theme"].ToString();

        //if (Session["MasterPage"] != null)
        //    Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";
    }

   
    protected void Page_Load(object sender, EventArgs e)
    {
        //this.txtCreateDateS.Attributes.Add("ReadOnly", "ReadOnly");
        //this.txtCreateDateE.Attributes.Add("ReadOnly", "ReadOnly");
        //this.txtVourDateS.Attributes.Add("ReadOnly", "ReadOnly");
        //this.txtVourDateE.Attributes.Add("ReadOnly", "ReadOnly");
        this.Title = "傳票取消核准作業";
        string strScript;
       // string sdSQL = "SELECT Company, Company + ' ' + CompFName AS CompanyName FROM Company";
        _MyDBM = new DBManger();
        _MyDBM.New();
        cnn = _MyDBM.GetConnectionString();
        
      //  DataTable dtComp = new DataTable();
      //  SqlDataAdapter adptComp = new SqlDataAdapter(sdSQL, cnn);
        ScriptManager.RegisterStartupScript(ScriptManager1, this.GetType(), "", @"JQ();", true);
        txtCreateDateS.CssClass = "JQCalendar";
        txtCreateDateE.CssClass = "JQCalendar";
        txtVourDateS.CssClass = "JQCalendar";
        txtVourDateE.CssClass = "JQCalendar";
        if (!IsPostBack)
        {
            DrpCompanyList.SelectValue = Session["Company"].ToString();
            // 取得傳票來源
            txtVourSrc.SetCodeList("AH17", 4, "全部");
        }
        else
        {
            //尋找觸發事件的按鈕
            string openbtnID = "";
            foreach (string str in Request.Form)
            {
                if (str != null)
                {
                    Control c = Page.FindControl(str);
                    if (c is Button)
                    {
                        openbtnID = c.ID;
                        break;
                    }
                }
            }

            //如果不是選取與開啟細項動作則可重載資料
            if (!Request.Form["__EVENTTARGET"].ToString().Contains("ckbID") && openbtnID != "btnDetail")
            {
                BindData();
                
            }

        }

        CheckGridCount();
        NavigatorPager.BindGridView = GridView1;
    }

    protected void FillDropDownList(DropDownList DDL, String SetValue, String SetText, DataTable dt)
    {
        DDL.DataSource = dt;
        DDL.DataTextField = SetText;
        DDL.DataValueField = SetValue;
        DDL.DataBind();
    }

    protected void CheckGridCount()
    {
        if (GridView1.Rows.Count <= 0)
        {
            btnCheck.Attributes.Add("style", "display:none");
            btnCheckAll.Attributes.Add("style", "display:none");

        }
        else
        {
            btnCheck.Attributes.Add("style", "display:block");
            btnCheckAll.Attributes.Add("style", "display:block");
        }

    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
       
            BindData();
    }


    /// <summary>
    /// 建立資料
    /// </summary>
    protected void BindData()
    {

        this.lblComp.Text = DrpCompanyList.SelectValue.Trim();
        Session["Company"] = DrpCompanyList.SelectValue.Trim(); ;
        //Session["VoucherNo"] = "980803012";

        try
        {
            string tCompany = DrpCompanyList.SelectValue.Trim();

            //記得改成stored procedure
            string sql = "SELECT";
            sql += " RTrim(H.VoucherNo) as VoucherNo,H.VoucherOwner+' '+isnull(U.UserName,' ') as VoucherOwner";
            sql += ",H.VoucherEntryDate,H.VoucherDate,isnull(H.RevDate,' ') as RevDate";
            sql += " from GLVoucherHead H";
            sql += " LEFT JOIN UC_User U on H.VoucherOwner=U.UserID and U.SiteId='" + _SystemId + "'";
            sql += " WHERE " ;
            sql += " H.ApprovalCode='Y' AND (H.PostCode=' ' OR IsNull(H.PostCode,'')='')";
            sql += " AND (H.Company = '" + tCompany + "') ";
            #region 非必填之查詢條件
            string strVoucherNoS = txtVourNoS.Text.Trim();
            string strVoucherNoE = txtVourNoE.Text.Trim();
            string strOwner = txtCreateUser.Text.Trim();
            string strVoucherSrc = txtVourSrc.SelectedCode.Trim();
            //將民國轉為西年
            string strVoucherDateS = _UserInfo.SysSet.FormatADDate(txtVourDateS.Text.ToString().Trim()).Replace("/", "");
            string strVoucherDateE = _UserInfo.SysSet.FormatADDate(txtVourDateE.Text.ToString().Trim()).Replace("/", "");
            string strVoucherEntryDateS = _UserInfo.SysSet.FormatADDate(txtCreateDateS.Text.ToString().Trim()).Replace("/", "");
            string strVoucherEntryDateE = _UserInfo.SysSet.FormatADDate(txtCreateDateE.Text.ToString().Trim()).Replace("/", "");

            //因若錯誤或空白傳回19120101 所以若是19120101則視為空白
            if (strVoucherDateS == "19120101")
            {
                strVoucherDateS = "";
            }

            if (strVoucherDateE == "19120101")
            {
                strVoucherDateE = "";
            }

            if (strVoucherEntryDateS == "19120101")
            {
                strVoucherEntryDateS = "";
            }

            if (strVoucherEntryDateE == "19120101")
            {
                strVoucherEntryDateE = "";
            }
            if (strOwner != "")
            {
                sql += " AND VoucherOwner like '%" + strOwner + "%'";
            }

            if (strVoucherSrc != "")
            {
                sql += " AND VoucherSource like '%" + strVoucherSrc + "%'";
            }

            if (strVoucherNoS != "" && strVoucherNoE != "")
            {
                sql += " AND (VoucherNo Between " + strVoucherNoS + " AND " + strVoucherNoE + ")";
            }
            else if (strVoucherNoS != "")
            {
                sql += " AND VoucherNo >= " + strVoucherNoS + "";
            }
            else if (strVoucherNoE != "")
                sql += " AND VoucherNo <= " + strVoucherNoE + "";


            if (strVoucherEntryDateS != "" && strVoucherEntryDateE != "")
                sql += " AND (VoucherEntryDate Between " + strVoucherEntryDateS + " AND " + strVoucherEntryDateE + ")";
            else if (strVoucherEntryDateS != "")
                sql += " AND VoucherEntryDate >= " + strVoucherEntryDateS + "";
            else if (strVoucherEntryDateE != "")
                sql += " AND VoucherEntryDate <= " + strVoucherEntryDateE + "";

            if (strVoucherDateS != "" && strVoucherDateE != "")
                sql += " AND (VoucherDate Between " + strVoucherDateS + " AND " + strVoucherDateE + ")";
            else if (strVoucherDateS != "")
                sql += " AND VoucherDate >= " + strVoucherDateS + "";
            else if (strVoucherDateE != "")
                sql += " AND VoucherDate <= " + strVoucherDateE + "";
            #endregion
            //篩選掉沒Detail之資料
            sql += " AND (H.VoucherNo IN (SELECT VoucherNo From GLVoucherDetail))";
            sql += " order by VoucherNo ";
            //DataTable dt = new DataTable();
            //SqlDataAdapter adpt = new SqlDataAdapter(sql, cnn);

            //adpt.Fill(dt);

            //GridView1.DataSource = dt;            
            sdVoucher.ConnectionString = cnn;
            sdVoucher.SelectCommand = sql;
            GridView1.DataBind();
            NavigatorPager.BindGridView = GridView1;
            NavigatorPager.DataBind();


        }
        catch (InvalidCastException ex)
        {
            //throw (ex);
            JsUtility.ClientMsgBoxAjax(ex.ToString(), UpdatePanel1, "");

        }
        finally
        {

        }

    
    
    }

    protected string GetDate(string selectVal, int tType)
    {
        string tVal = "";
        switch (tType)
        {
            case 1:
                if (selectVal.Trim() == "")
                    tVal = "19500101";
                else
                    tVal = DateTime.Parse(selectVal).ToString("yyyyMMdd");
                break;
            case 2:
                if (selectVal.Trim() == "")
                    tVal = "99991231";
                else
                    tVal = DateTime.Parse(selectVal).ToString("yyyyMMdd");
                break;
            case 3:
                if (selectVal.Trim() == "")
                    tVal = "0";
                else
                    tVal = selectVal.Trim();
                break;
            case 4:
                if (selectVal.Trim() == "")
                    tVal = "9999999999";
                else
                    tVal = selectVal.Trim();
                break;
            case 5:
                if (selectVal.Trim() == "")
                    tVal = "%%";
                else
                    tVal = selectVal.Trim();
                break;
        }
        return tVal;
    }

    //checkbox改變
    protected void ckbID_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ckb = sender as CheckBox;

        if (ckb.Checked == true)
        {
            this.hidAll.Value += ((HiddenField)ckb.Parent.FindControl("HidID")).Value + ",";
        }
        else
        {
            string[] param = this.hidAll.Value.Split(new char[] { ',' } , StringSplitOptions.RemoveEmptyEntries);

            this.hidAll.Value = "";

            foreach (string item in param)
            {
                if (item != ((HiddenField)ckb.Parent.FindControl("HidID")).Value)
                {
                    this.hidAll.Value += item + ",";
                }
            }
        }
    }

    //查看選取項目
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Write(this.hidAll.Value);
    }

    //checkbox綁定
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Style["cursor"] = "hand";
            e.Row.Attributes.Add("onmouseover", "oldColor=this.style.backgroundColor;this.style.backgroundColor='silver'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=oldColor");

            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].CssClass = "Grid_GridLine";
                e.Row.Cells[i].Style.Add("text-align", "center");
            }       
                       
            string[] param = this.hidAll.Value.Split(new char[] { ',' } , StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in param)
            {
                if (item == ((HiddenField)e.Row.Cells[0].FindControl("HidID")).Value)
                {
                    ((CheckBox)e.Row.Cells[0].FindControl("ckbID")).Checked = true;
                    break;
                }
            }
        }
        CheckGridCount();
    }

    //checkbox全選
    protected void linkbtnAll_Click(object sender, EventArgs e)
    {
        string[] param = this.hidAll.Value.Split(new char[] { ',' } , StringSplitOptions.RemoveEmptyEntries);

        int cnt;

        foreach (GridViewRow item in this.GridView1.Rows)
        {
            cnt = 0;
            foreach (string str in param)
            {
                if (((HiddenField)item.Cells[0].FindControl("HidID")).Value == str)
                {
                    cnt++;
                }
            }

            if (cnt == 0)
            {
                this.hidAll.Value += ((HiddenField)item.Cells[0].FindControl("HidID")).Value + ",";
            }
        }

        this.DataBind();
        BindData();
    }

    //checkbox取消全選
    protected void linkbtnNo_Click(object sender, EventArgs e)
    {
        string[] param = this.hidAll.Value.Split(new char[] { ',' } , StringSplitOptions.RemoveEmptyEntries);

        //int cnt;

        foreach (GridViewRow item in this.GridView1.Rows)
        {
            for (int i = 0; i < param.Length; i++)
            {
                if (((HiddenField)item.Cells[0].FindControl("HidID")).Value == param[i])
                {
                    param[i] = "";
                }
            }
        }

        this.hidAll.Value = "";

        for (int i = 0; i < param.Length; i++)
        {
            if (param[i] != "")
            {
                this.hidAll.Value += param[i] + ",";
            }
        }

        this.DataBind();
        BindData();
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        //    //GridView1.DataSource = (DataTable)Session["GLA0140"];
        BindData();
        GridView1.DataBind();
    }
    protected void btnCheck_Click(object sender, EventArgs e)
    {
        _MyDBM = new DBManger();       
        SqlConnection sqlCnn = new SqlConnection(_MyDBM.GetConnectionString());
        SqlCommand sqlCmd = new SqlCommand();
        sqlCmd.Connection = sqlCnn;
        sqlCnn.Open();

        try
        {
            string sqlUptH = "";
            string sqlUptD = "";

            if (this.hidAll.Value != "")
            {
                string[] aryVourNo = ((string)hidAll.Value).Split(',');
                //Response.Write(aryVourNo.Length);

                //SqlTransaction sqlTrans = new SqlTransaction();

                int i;

                for (i = 0; i < aryVourNo.Length; i++)
                {
                    sqlUptH = "Update GLVoucherHead SET";
                    sqlUptH += " ApprovalCode='N'";
                    sqlUptH += " WHERE (Company='" + lblComp.Text + "') AND";
                    sqlUptH += " (VoucherNo='" + aryVourNo[i].ToString().Trim() + "') AND";
                    sqlUptH += " (PostCode=' ' OR PostCode IS NULL)";

                    sqlUptD = "Update GLVoucherDetail SET";
                    sqlUptD += " ApvlFlag='N'";
                    sqlUptD += " WHERE (Company='" + lblComp.Text + "') AND";
                    sqlUptD += " (VoucherNo='" + aryVourNo[i].ToString().Trim() + "')";
                    //sqlUptD += " AND (DletFlag=' ' OR DletFlag IS NULL)";

                    //sqlTrans.Connection = sqlCnn;
                    //sqlCmd.Transaction = sqlTrans;

                    sqlCmd.CommandText = sqlUptH;
                    sqlCmd.ExecuteNonQuery();

                    sqlCmd.CommandText = sqlUptD;
                    sqlCmd.ExecuteNonQuery();
                    //sqlTrans.Commit;
                }
                //Response.Write("ok");            
                JsUtility.ClientMsgBoxAjax("已取消核准。", UpdatePanel1, "");
            }

        }
        catch (InvalidCastException ex)
        {
            //throw (ex);
           
            JsUtility.ClientMsgBoxAjax(ex.ToString(), UpdatePanel1, "");
            //sqlTrans.Rollback;
        }
        finally
        {
            sqlCmd.Dispose();
            sqlCnn.Close();
        }
        BindData();
    }

    protected void btnDetail_Click(object sender, EventArgs e)
    {
        GridViewRow gvr = (GridViewRow)((TableCell)((Button)sender).Parent).Parent;
        Session["VoucherNo"] = gvr.Cells[1].Text;
        //Session["VoucherNo"] = "810103004";
        //doClientMsgBoxAjax.ClientMsgBoxAjax(Session["VoucherNo"].ToString(), UpdatePanel1, "");
        Session["OPMode"] = "Query";
        Session["FromURL"] = "";
        callWOP("GLA0110.aspx", UpdatePanel1, "");
    }

    public void callWOP(string url, System.Web.UI.UpdatePanel upPanel, string key)
    {
        string script = "";

        url = url.Replace("'", "");
        if (key == "")
        {
            Random autoRand = new Random();
            key = "clientScriptMessage" + System.Environment.TickCount.ToString() + autoRand.Next()
                + System.Environment.TickCount.ToString();
        }
        script += "wop('" + url + "');";
        System.Web.UI.ScriptManager.RegisterStartupScript(upPanel, upPanel.GetType(), key, script, true);
    }

}
