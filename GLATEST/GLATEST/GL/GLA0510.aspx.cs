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
using System.Collections.Generic;
using PanPacificClass;


public partial class GLA0510 : System.Web.UI.Page
{
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLA0510";
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
        _MyDBM = new DBManger();
        _MyDBM.New();
        txtAcctYear.Attributes.Add("ReadOnly", "ReadOnly");
        txtAcctPeriod.Attributes.Add("ReadOnly", "ReadOnly");                

        if (!IsPostBack)
        {   //暫時將closeYYYY and close YM 為零者剔除
            string sdCompSQL = "SELECT Company,  Company + ' ' +  CompanyName AS CompanyName FROM Company";
            sdCompSQL += " WHERE Company in (SELECT Distinct company from GLParm where CloseYYYY<>0 and CloseYM<>0)";

            DrpCompanyList.SelectValue = Session["Company"].ToString();
            DrpCompanyList.StyleAdd("width", "270px");
            DrpCompanyList.AutoPostBack = true;
            DrpCompanyList.SelectedChanged += new UserControl_CompanyList.SelectedIndexChanged(ddlCompany_SelectedIndexChanged);
        }
        ddlCompany_SelectedIndexChanged(null, null);
        //btnCheckDDL_Click(null,null);       
    }
    
    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        _MyDBM = new DBManger();
        _MyDBM.New();
        //if (IsPostBack)
        {
            if (DrpCompanyList.SelectValue != "")
            {
                string tCompany = DrpCompanyList.SelectValue.ToString();
                Session["tCompany"] = tCompany;

                try
                {
                    //object tYearMonth ;
                    string sqlGetYM = "select convert(char(6),closeYM) as CloseYM from glparm where company='" + tCompany + "'";

                    DataTable dt1 = new DataTable();
                    dt1 = _MyDBM.ExecuteDataTable(sqlGetYM);
                    if (dt1 != null && dt1.Rows.Count > 0)
                    {
                        Session["AcctYear"] = dt1.Rows[0][0].ToString();
                        string tYear = dt1.Rows[0][0].ToString().Substring(0, 4);
                        string tPeriod = dt1.Rows[0][0].ToString().Substring(4, 2);

                        ViewState["AcctLastPeriod"] = tYear.ToString() + "-" + tPeriod;
                        this.lblLastPeriod.Text = ViewState["AcctLastPeriod"].ToString();

                        ////---------------------
                        string sql = @"SELECT PeriodBegin,PeriodEnd,NextYear,NextPeriodBegin,NextPeriodEnd
                  FROM dbo.fnGetAccPeriodInfo('" + tCompany + "'," + tYear + ",'" + tPeriod + "')";

                        DataTable dt2 = new DataTable();
                        dt2 = _MyDBM.ExecuteDataTable(sql);
                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            this.lblLastDateBegin.Text = dt2.Rows[0]["PeriodBegin"].ToString();// +"-" + dt2.Rows[0]["PeriodEnd"].ToString();
                            this.lblLastDateEnd.Text = dt2.Rows[0]["PeriodEnd"].ToString();
                            this.txtAcctYear.Text = dt2.Rows[0]["NextYear"].ToString();
                            this.lblAcctPeriodBegin.Text = dt2.Rows[0]["NextPeriodBegin"].ToString();// +"-" + dt2.Rows[0]["NextPeriodEnd"].ToString();
                            this.lblAcctPeriodEnd.Text = dt2.Rows[0]["NextPeriodEnd"].ToString();
                            this.txtAcctPeriod.Text = dt2.Rows[0]["NextPeriodBegin"].ToString().Substring(0, 4) + "-" + dt2.Rows[0]["NextPeriodBegin"].ToString().Substring(4, 2);
                            this.lblLastPeriod.Text = ViewState["AcctLastPeriod"].ToString();

                            Session["NextPeriod"] = dt2.Rows[0]["NextPeriodBegin"].ToString().Substring(4, 2);
                            Session["NextYear"] = dt2.Rows[0]["NextYear"].ToString();
                        }
                    }
                }
                catch (InvalidCastException ex)
                {
                    throw (ex);

                }
                finally
                {


                }
            }

        btnCheckDDL_Click(null, null);
       
    }
    //protected string GetPeriod(Comp)
    //{

  }

    //*月結執行
    protected void btnClose_Click(object sender, EventArgs e)
    {
        _MyDBM = new DBManger();
        _MyDBM.New();
        
        //string cnn = PanUtility.PanDB.GetEEPCnnString();
              
        try
        {
            string sqlUptH = "";
            string sqlUptD = "";

            string UpPeriod = Session["NextPeriod"].ToString();
            string UpYr = Session["NextYear"].ToString();
            //string UpYM = ViewState["NextYM"].ToString();
            string strCompany = Session["tCompany"].ToString();
          
            sqlUptH = @"Update GLAcctPeriod SET
                      PeriodClose" + UpPeriod + "='Y' WHERE (Company='" + strCompany + "')";
            
            sqlUptD = @"Update GLParm SET CloseYM=" + UpYr + UpPeriod +  
                "WHERE (Company='" + strCompany + "')";

            //更新細項
            _MyDBM.ExecuteCommand(sqlUptH);

            //更新結轉月
            _MyDBM.ExecuteCommand(sqlUptD);
                       

            //doClientMsgBox.ClientMsgBox("結轉成功。", Page, "");
            JsUtility.ClientMsgBoxAjax("本月份結轉成功。", UpdatePanel1 , "");

            DrpCompanyList.SelectValue = "";
            ddlCompany_SelectedIndexChanged(null, null);

            //Response.Redirect("GLA0510.aspx");
            //doClientMsgBoxAjax.ClientMsgBoxAjax("結轉成功。", UpdatePanel5, "");
            //Response.AddHeader("Refresh", "0"); 
            //Response.Write( "<script language=javascript>window.location.href=GLA510.aspx;</script>" ); 
 
        }
        catch (InvalidCastException ex)
        {
            //throw (ex);
            //doClientMsgBoxAjax.ClientMsgBoxAjax(ex.ToString(), UpdatePanel1, "");
            JsUtility.ClientMsgBoxAjax(ex.ToString(), UpdatePanel1 , "");
        }
        finally
        {
          
        }

    }

    //判斷選擇值決定轉結作業按鈕之enabled
    protected void btnCheckDDL_Click(object sender, EventArgs e)
    {
        if (txtAcctYear.Text.ToString() != "" && DrpCompanyList.SelectValue != "")
        {
            this.btnClose.Enabled = true;//.Attributes.Add("style", "");
        }
        else //if (ddlCompany.SelectedIndex = 0)
        {
            //btnClose.Attributes.Add("Enabled","false");
            this.btnClose.Enabled = false;
            this.lblLastDateBegin.Text = "";
            this.lblLastDateEnd.Text = "";
            this.txtAcctYear.Text = "";
            this.lblAcctPeriodBegin.Text = "";
            this.lblAcctPeriodEnd.Text = "";
            this.txtAcctPeriod.Text = "";
            this.lblLastPeriod.Text = "";
        }
    }
}
