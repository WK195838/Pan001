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
using PanPacificClass;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;


public partial class GLB020122    : System.Web.UI.Page
{

    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLB020122";
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
        // 需要執行等待畫面的按鈕
        btnQuery.Attributes.Add("onClick", "drawWait('')");
        CrystalReportViewer1.DisplayGroupTree = false;
        CrystalReportViewer1.HasToggleGroupTreeButton = false;

        int icalendersYear = DateTime.Now.AddYears(-5).Year - 1911;
        int icalendereYear = DateTime.Now.AddYears(5).Year - 1911;
        ScriptManager.RegisterStartupScript(ScriptManager1, this.GetType(), "", @"JQ();", true);
        TxtvoucherSDate.CssClass = "JQCalendar";
        TxtvoucherEDate.CssClass = "JQCalendar";
        TxtcreateSDate.CssClass = "JQCalendar";
        TxtcreateEDate.CssClass = "JQCalendar";
        if (!IsPostBack)
        {
            DrpCompanyName.SelectValue = "20";
           
        }
        else
        {
            if (ViewState["Sourcedata"] != null)
            {
                //設定縮放功能
                //找出控制項的值

                //int intPageZoom = (((DropDownList)this.CrystalReportViewer1.Controls[2].Controls[15]).SelectedIndex + 1) * 25;
                //CrystalReportViewer1.Zoom(intPageZoom);

                CryReportSource.ReportDocument.SetDataSource((DataTable)ViewState["Sourcedata"]);
                CrystalReportViewer1.DataBind();
            }

        }

        //傳票開始
        string strScript = "return GetPromptTheDate(" + TxtvoucherSDate.ClientID + "," + icalendersYear.ToString() + "," + icalendereYear.ToString() + ");";
        //ibOW_SDate.Attributes.Add("onclick", strScript);
        //傳票結束
        strScript = "return GetPromptTheDate(" + TxtvoucherEDate.ClientID + "," + icalendersYear.ToString() + "," + icalendereYear.ToString() + ");";
        //ibOW_EDate.Attributes.Add("onclick", strScript);

        //傳票開始
        strScript = "return GetPromptTheDate(" + TxtcreateSDate.ClientID + "," + icalendersYear.ToString() + "," + icalendereYear.ToString() + ");";
        //ibOW_MSDate.Attributes.Add("onclick", strScript);

        //傳票結束
        strScript = "return GetPromptTheDate(" + TxtcreateEDate.ClientID + "," + icalendersYear.ToString() + "," + icalendereYear.ToString() + ");";
        //ibOW_MEDate.Attributes.Add("onclick", strScript);
       
        ScriptManager1.RegisterPostBackControl(btnQuery);
    }

   

    protected void btnQuery_Click(object sender, ImageClickEventArgs e)
    {
        BindData();  
    }


    protected void BindData()
    {
        //-------------------------------------------------------------------------------
        // 畫面查詢條件檢查
        //-------------------------------------------------------------------------------
         string strSQL = "";
         DataTable Dt;
         SqlCommand sqlcmd = new SqlCommand();
        
         _MyDBM = new DBManger();
         _MyDBM.New();

         string myAcctName = "";
         string myVoucherSdate = "";
         string myVoucherEdate = "";
         string myVoucherCreatDateS = "";
         string myVoucherCreatDateE = "";
        

        //檢查公司

        //檢查會計科目
         myAcctName = TxtAcctno.Text.Trim();
        
        //轉換傳票起始日期

         if (TxtvoucherSDate.Text.Trim() != "")
         {
             myVoucherSdate = _UserInfo.SysSet.ToADDate(TxtvoucherSDate.Text.Trim());

             if (myVoucherSdate == "1912/01/01")
             {
                 JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "");
                 JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面
                 TxtvoucherSDate.Focus();
                 return;
             }
             myVoucherSdate = myVoucherSdate.Replace("/","");
         }

         //轉換傳票結束日期

         if (TxtvoucherEDate.Text.Trim() != "")
         {
             myVoucherEdate = _UserInfo.SysSet.ToADDate(TxtvoucherEDate.Text.Trim());

             if (myVoucherEdate == "1912/01/01")
             {
                 JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "");
                 JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面
                 TxtvoucherEDate.Focus();
                 return;
             }
             myVoucherEdate = myVoucherEdate.Replace("/","");
         }
       
        //轉換製票起始日期
         if (TxtcreateSDate.Text.Trim()!="")
         {
             myVoucherCreatDateS = _UserInfo.SysSet.ToADDate(TxtcreateSDate.Text.Trim());

             if (myVoucherCreatDateS == "1912/01/01")
             {
                 JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "");
                 JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面
                 TxtcreateSDate.Focus();
                 return;
             }

             myVoucherCreatDateS = myVoucherCreatDateS.Replace("/", "");
         }

         //轉換製票結束日期
         if (TxtcreateEDate.Text.Trim()!="")
         {
             myVoucherCreatDateE = _UserInfo.SysSet.ToADDate(TxtcreateEDate.Text.Trim());

             if (myVoucherCreatDateE == "1912/01/01")
             {
                 JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "");
                 JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面
                 TxtcreateSDate.Focus();
                 return;
             }
             myVoucherCreatDateE = myVoucherCreatDateE.Replace("/", "");
         }





        //-------------------------------------------------------------------------------
        // 取出報表參數相關資訊並設值給報表參數
        //-------------------------------------------------------------------------------
        // 取出公司全名
        string myCompanyName = "";
        strSQL = "Select CompanyName From Company Where Company = '"+DrpCompanyName.SelectValue+"'";
        Dt = _MyDBM.ExecuteDataTable(strSQL);

        if (Dt.Rows.Count != 0) myCompanyName = Dt.Rows[0]["CompanyName"].ToString();
                    
        

        // 設值給報表參數
        CrystalDecisions.Web.Parameter cparam = new CrystalDecisions.Web.Parameter();
        cparam.Name = "CompanyName";
        cparam.DefaultValue = myCompanyName;
        CryReportSource.Report.Parameters.Add(cparam);

        CrystalDecisions.Web.Parameter AcctNparam = new CrystalDecisions.Web.Parameter();
        AcctNparam.Name = "AcctNoName";
        AcctNparam.DefaultValue = myAcctName;
        CryReportSource.Report.Parameters.Add(AcctNparam);

        CrystalDecisions.Web.Parameter voucherSparam = new CrystalDecisions.Web.Parameter();
        voucherSparam.Name = "voucherStartDate";
        voucherSparam.DefaultValue = myVoucherSdate;
        CryReportSource.Report.Parameters.Add(voucherSparam);

        CrystalDecisions.Web.Parameter voucherEparam = new CrystalDecisions.Web.Parameter();
        voucherEparam.Name = "voucherEndDate";
        voucherEparam.DefaultValue = myVoucherEdate;
        CryReportSource.Report.Parameters.Add(voucherEparam);

        CrystalDecisions.Web.Parameter voucherCSparam = new CrystalDecisions.Web.Parameter();
        voucherCSparam.Name = "voucherCreatDateS";
        voucherCSparam.DefaultValue = myVoucherCreatDateS;
        CryReportSource.Report.Parameters.Add(voucherCSparam);

        CrystalDecisions.Web.Parameter voucherCEparam = new CrystalDecisions.Web.Parameter();
        voucherCEparam.Name = "voucherCreatDateE";
        voucherCEparam.DefaultValue = myVoucherCreatDateE;
        CryReportSource.Report.Parameters.Add(voucherCEparam);

     
        
        // 執行計算損益表資料之Stored Procedure
            


        sqlcmd.Parameters.Add("@Company",SqlDbType.Char).Value=DrpCompanyName.SelectValue;
        sqlcmd.Parameters.Add("@AcctNo",SqlDbType.Char).Value=TxtAcctno.Text.Trim();
        sqlcmd.Parameters.Add("@CreateUser", SqlDbType.Char).Value = txtcreater.Text.Trim();
        sqlcmd.Parameters.Add("@VoucherSDate",SqlDbType.Char).Value=myVoucherSdate;
        sqlcmd.Parameters.Add("@VoucherEDate",SqlDbType.Char).Value=myVoucherEdate;
        sqlcmd.Parameters.Add("@VoucherEntrySDate",SqlDbType.Char).Value=myVoucherCreatDateS;
        sqlcmd.Parameters.Add("@VoucherEntryEDate",SqlDbType.Char).Value=myVoucherCreatDateE;
        sqlcmd.Parameters.Add("@index01S",SqlDbType.Char).Value=Txtindex1S.Text.Trim();
        sqlcmd.Parameters.Add("@index01E",SqlDbType.Char).Value=Txtindex1E.Text.Trim();
        sqlcmd.Parameters.Add("@Seqctl1",SqlDbType.Char).Value=txtseq1.Text.Trim();
        sqlcmd.Parameters.Add("@Sum1",SqlDbType.Bit).Value=chkindex1.Checked==true?1:0;
        sqlcmd.Parameters.Add("@index02S",SqlDbType.Char).Value=Txtindex2S.Text.Trim();
        sqlcmd.Parameters.Add("@index02E",SqlDbType.Char).Value=Txtindex2E.Text.Trim();
        sqlcmd.Parameters.Add("@Seqctl2",SqlDbType.Char).Value=txtseq2.Text.Trim();
        sqlcmd.Parameters.Add("@Sum2",SqlDbType.Bit).Value=chkindex2.Checked==true?1:0;
        sqlcmd.Parameters.Add("@index03S",SqlDbType.Char).Value=Txtindex3S.Text.Trim();
        sqlcmd.Parameters.Add("@index03E",SqlDbType.Char).Value=Txtindex3E.Text.Trim();
        sqlcmd.Parameters.Add("@Seqctl3",SqlDbType.Char).Value=txtseq3.Text.Trim();
        sqlcmd.Parameters.Add("@Sum3",SqlDbType.Bit).Value=chkindex3.Checked==true?1:0;
        sqlcmd.Parameters.Add("@index04S",SqlDbType.Char).Value=Txtindex4S.Text.Trim();
        sqlcmd.Parameters.Add("@index04E",SqlDbType.Char).Value=Txtindex4E.Text.Trim();
        sqlcmd.Parameters.Add("@Seqctl4",SqlDbType.Char).Value=txtseq4.Text.Trim();
        sqlcmd.Parameters.Add("@Sum4",SqlDbType.Bit).Value=chkindex4.Checked==true?1:0;
        sqlcmd.Parameters.Add("@index05S",SqlDbType.Char).Value=Txtindex5S.Text.Trim();
        sqlcmd.Parameters.Add("@index05E",SqlDbType.Char).Value=Txtindex5E.Text.Trim();
        sqlcmd.Parameters.Add("@Seqctl5",SqlDbType.Char).Value=txtseq5.Text.Trim();
        sqlcmd.Parameters.Add("@Sum5",SqlDbType.Bit).Value=chkindex5.Checked==true?1:0;
               


        Dt = _MyDBM.ExecStoredProcedure("dbo.sp_GLB020122",sqlcmd.Parameters);

        if (Dt.Rows.Count > 0)
        {
            CryReportSource.ReportDocument.SetDataSource(Dt);
            CrystalReportViewer1.DataBind();
            ViewState["Sourcedata"] = Dt;
            Dt.Dispose();
        }
        else
        {
            JsUtility.ClientMsgBoxAjax("查無相關資料！", UpdatePanel1, "");

        }
             
    
      
        JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面
    
    
    }
    
  

    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        BindAcctName();


    }

    /// <summary>
    /// 取得科目設定與名稱
    /// </summary>
    private void BindAcctName()
    {
        _MyDBM = new DBManger();
        _MyDBM.New();
        DataTable Dt = new DataTable();
        SqlCommand sqlcmd = new SqlCommand();

        string strSQL = @"SELECT Company,AcctNo,AcctDesc1,
 dbo.fnGetAcnoIdx(@Company,idx01) AS idx01Name,
dbo.fnGetAcnoIdx(@Company,idx02) AS idx02Name,
dbo.fnGetAcnoIdx(@Company,idx03) AS idx03Name,
dbo.fnGetAcnoIdx(@Company,idx04) AS idx04Name,
dbo.fnGetAcnoIdx(@Company,idx05) AS idx05Name,
seqctl1,seqctl2,seqctl3,seqctl4,seqctl5,
Subtotctl1,Subtotctl2,Subtotctl3,Subtotctl4,
Subtotctl5,ColCtl

FROM GLACCTDEF WHERE Company=@Company AND AccTNo=@AcctNo";

        sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = DrpCompanyName.SelectValue;
        sqlcmd.Parameters.Add("@AcctNo", SqlDbType.Char).Value = TxtAcctno.Text;

        try
        {

            Dt = _MyDBM.ExecuteDataTable(strSQL,sqlcmd.Parameters,CommandType.Text);

            if (Dt.Rows.Count > 0)
            {
                labacctName.Text = Dt.Rows[0]["AcctDesc1"].ToString();
                LabTitle1.Text = Dt.Rows[0]["idx01Name"].ToString();
                LabTitle2.Text = Dt.Rows[0]["idx02Name"].ToString();
                LabTitle3.Text = Dt.Rows[0]["idx03Name"].ToString();
                LabTitle4.Text = Dt.Rows[0]["idx04Name"].ToString();
                LabTitle5.Text = Dt.Rows[0]["idx05Name"].ToString();
                txtseq1.Text = Dt.Rows[0]["seqctl1"].ToString();
                txtseq2.Text = Dt.Rows[0]["seqctl2"].ToString();
                txtseq3.Text = Dt.Rows[0]["seqctl3"].ToString();
                txtseq4.Text = Dt.Rows[0]["seqctl4"].ToString();
                txtseq5.Text = Dt.Rows[0]["seqctl5"].ToString();

                if (Dt.Rows[0]["Subtotctl1"] != null  )
                {
                    if (Dt.Rows[0]["Subtotctl1"].ToString().Trim() != "")
                    {
                        chkindex1.Checked = true;
                    }
                    else
                    {
                        chkindex1.Checked = false;
                    }
                }
                else
                {
                    chkindex1.Checked = false;
                }


                if (Dt.Rows[0]["Subtotctl2"] != null )
                {

                    if (Dt.Rows[0]["Subtotctl2"].ToString().Trim() != "")
                    {
                        chkindex2.Checked = true;
                    }
                    else
                    {
                        chkindex2.Checked = false;
                    }
                }
                else
                { 
                  chkindex2.Checked = false;
                }
                

                if (Dt.Rows[0]["Subtotctl3"] != null )
                {
                    if (Dt.Rows[0]["Subtotctl3"].ToString().Trim() != "")
                    {
                        chkindex3.Checked = true;
                    }
                    else
                    {
                        chkindex3.Checked = false;
                    }
                    
                }
                else
                {
                    chkindex3.Checked = false;
                }

                if (Dt.Rows[0]["Subtotctl4"] != null )
                {
                    if (Dt.Rows[0]["Subtotctl4"].ToString().Trim() != "")
                    {
                        chkindex4.Checked = true;
                    }
                    else
                    {
                        chkindex4.Checked = false;
                    }
                }
                else
                {
                    chkindex4.Checked = false;
                }

                if (Dt.Rows[0]["Subtotctl5"] != null )
                {

                    if (Dt.Rows[0]["Subtotctl5"].ToString().Trim() != "")
                    {
                        chkindex5.Checked = true;
                    }
                    else
                    {
                        chkindex5.Checked = false;
                    }
                }
                else
                {
                    chkindex5.Checked = false;
                }


                DivDisplay.Visible = true;

            }
            else
            {
                
                JsUtility.ClientMsgBoxAjax("查無相關會計科目！", UpdatePanel1, "");
            }
        }

        catch(Exception ex)
        {
            throw ex;

        }



    
    
    }
}
