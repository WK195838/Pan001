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
using System.Data.SqlClient;
using PanPacificClass;

public partial class GLR0160    : System.Web.UI.Page
{
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLR0160";
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

        DrpCompany.SelectedChanged += new UserControl_CompanyList.SelectedIndexChanged(SelectIndex_Change);
        ScriptManager.RegisterStartupScript(ScriptManager1, this.GetType(), "", @"JQ();", true);
        txtVoucherDateS.CssClass = "JQCalendar";
        txtVoucherDateE.CssClass = "JQCalendar";
		if (!IsPostBack)
        {
           
			txtAcctNo1.Text = "11";
			txtAcctNo2.Text = "560101";
            DrpCompany.SelectValue = "20";
            GetCodeDef();
           
        }
        else
        {
            if (ViewState["Sourcedata"] != null)
            {
                //設定縮放功能
                //找出控制項的值

                //int intPageZoom = (((DropDownList)this.CrystalReportViewer1.Controls[2].Controls[15]).SelectedIndex + 1) * 25;
                //CrystalReportViewer1.Zoom(intPageZoom);               
                cryReportSource.ReportDocument.SetDataSource((DataTable)ViewState["Sourcedata"]);
                CrystalReportViewer1.DataBind();
            }

        }

       
        ScriptManager1.RegisterPostBackControl(btnQuery);

    }

   

    public void clsWait() {
        JsUtility.DoJavascript("closeWait();", this.Page, "");
    }


    protected void btnQuery_Click(object sender, ImageClickEventArgs e)
    {

        _MyDBM = new DBManger();
        _MyDBM.New();
        string strSQL="";

        SqlCommand sqlcmd = new SqlCommand();
        DataTable ResultDt;

		//-------------------------------------------------------------------------------
		// 畫面查詢條件檢查
		//-------------------------------------------------------------------------------

        

        //// Check 起迄會計科目1
        if (txtAcctNo1.Text == "")
        {
            txtAcctNo1.Focus();
            JsUtility.ClientMsgBoxAjax("起始會計科目不可空白！", UpdatePanel1, "a");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
            return;
        }
        if (String.Compare(txtAcctNo1.Text, txtAcctNo2.Text) > 0)
        {           
            JsUtility.ClientMsgBoxAjax("起始會計科目不可大於迄止會計科目！", UpdatePanel1, "a");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
            return;
        }
        

		// Check 起迄日期

        

		string myFromDate =_UserInfo.SysSet.ToADDate(txtVoucherDateS.Text); 


		if (myFromDate == "1912/01/01")
		{
			
			JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "a");
			JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
			return;
		}
        myFromDate=myFromDate.Replace("/","");



        string myToDate = _UserInfo.SysSet.ToADDate(txtVoucherDateE.Text);
        if (myToDate == "1912/01/01")
		{

            JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "b");
			JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
			return;
		}
        myToDate = myToDate.Replace("/", "");



		//-------------------------------------------------------------------------------
        // 取出報表參數相關資訊並設值給報表參數，並計算產生報表結果資料
        //-------------------------------------------------------------------------------
        // 取出公司全名
        

        string myCompanyName = "";
        strSQL = "Select CompanyName From Company Where Company = '"+DrpCompany.SelectValue+"'";
        DataTable DT = _MyDBM.ExecuteDataTable(strSQL);

        if (DT.Rows.Count != 0) myCompanyName = DT.Rows[0]["CompanyName"].ToString();
		//執行計算損益表資料之Stored Procedure 

        sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = DrpCompany.SelectValue;
        sqlcmd.Parameters.Add("@FromAcno1",SqlDbType.Char).Value=txtAcctNo1.Text.Trim();
        sqlcmd.Parameters.Add("@ToAcno1", SqlDbType.Char).Value = txtAcctNo2.Text.Trim(); ;
        sqlcmd.Parameters.Add("@FromAcno2",SqlDbType.Char).Value=txtAcctNo3.Text.Trim();
        sqlcmd.Parameters.Add("@ToAcno2",SqlDbType.Char).Value=txtAcctNo4.Text.Trim();
        sqlcmd.Parameters.Add("@FromDate",SqlDbType.Char).Value=myFromDate;
        sqlcmd.Parameters.Add("@ToDate",SqlDbType.Char).Value=myToDate;
        sqlcmd.Parameters.Add("@ColID1", SqlDbType.Char).Value = ddlTip1.SelectedValue;
        sqlcmd.Parameters.Add("@FromIndex1",SqlDbType.Char).Value=txtStart1.Text.Trim();
        sqlcmd.Parameters.Add("@ToIndex1", SqlDbType.Char).Value = txtEnd1.Text.Trim(); ;
        sqlcmd.Parameters.Add("@SubTotCtl1",SqlDbType.Bit).Value=cbxAmt1.Checked==true? 1:0;
        sqlcmd.Parameters.Add("@Seqctl1", SqlDbType.Char).Value = TxtSeq1.Text;
        sqlcmd.Parameters.Add("@ColID2",SqlDbType.Char).Value=ddlTip2.SelectedValue;
        sqlcmd.Parameters.Add("@FromIndex2",SqlDbType.Char).Value=txtStart2.Text.Trim();
        sqlcmd.Parameters.Add("@ToIndex2",SqlDbType.Char).Value=txtEnd2.Text.Trim();
        sqlcmd.Parameters.Add("@SubTotCtl2", SqlDbType.Bit).Value = cbxAmt2.Checked == true ? 1 : 0;
        sqlcmd.Parameters.Add("@Seqctl2", SqlDbType.Char).Value = TxtSeq2.Text;
        sqlcmd.Parameters.Add("@ColID3",SqlDbType.Char).Value=ddlTip3.SelectedValue.Trim();
        sqlcmd.Parameters.Add("@FromIndex3",SqlDbType.Char).Value=txtStart3.Text.Trim();
        sqlcmd.Parameters.Add("@ToIndex3",SqlDbType.Char).Value=txtEnd3.Text.Trim();
        sqlcmd.Parameters.Add("@SubTotCtl3", SqlDbType.Bit).Value = cbxAmt3.Checked == true ? 1 : 0;
        sqlcmd.Parameters.Add("@Seqctl3", SqlDbType.Char).Value = TxtSeq3.Text;
        sqlcmd.Parameters.Add("@ColID4",SqlDbType.Char).Value=ddlTip4.SelectedValue.Trim();
        sqlcmd.Parameters.Add("@FromIndex4",SqlDbType.Char).Value=txtStart4.Text.Trim();
        sqlcmd.Parameters.Add("@ToIndex4",SqlDbType.Char).Value=txtEnd4.Text.Trim();
        sqlcmd.Parameters.Add("@SubTotCtl4", SqlDbType.Bit).Value = cbxAmt4.Checked == true?1:0;
        sqlcmd.Parameters.Add("@Seqctl4", SqlDbType.Char).Value = TxtSeq4.Text;
        sqlcmd.Parameters.Add("@ColID5",SqlDbType.Char).Value=ddlTip5.SelectedValue.Trim();
        sqlcmd.Parameters.Add("@FromIndex5",SqlDbType.Char).Value=txtStart5.Text.Trim();
        sqlcmd.Parameters.Add("@ToIndex5",SqlDbType.Char).Value=txtEnd5.Text.Trim();
        sqlcmd.Parameters.Add("@SubTotCtl5", SqlDbType.Bit).Value = cbxAmt5.Checked == true ? 1 : 0;
        sqlcmd.Parameters.Add("@Seqctl5", SqlDbType.Char).Value = TxtSeq5.Text;
        sqlcmd.Parameters.Add("@SortChoice", SqlDbType.Char).Value = ddlSort.SelectedValue;


        ResultDt = _MyDBM.ExecStoredProcedure("dbo.sp_GLR0160",sqlcmd.Parameters);
               
		
		// 設值給報表參數


        CrystalDecisions.Web.Parameter cparam = new CrystalDecisions.Web.Parameter();
        cparam.Name = "CompanyName";
        cparam.DefaultValue = myCompanyName;        
        cryReportSource.Report.Parameters.Add(cparam);

        CrystalDecisions.Web.Parameter FromDateparam = new CrystalDecisions.Web.Parameter();
        FromDateparam.Name = "FromDate";
        FromDateparam.DefaultValue = myFromDate;
        cryReportSource.Report.Parameters.Add(FromDateparam);


        CrystalDecisions.Web.Parameter ToDateparam = new CrystalDecisions.Web.Parameter();
        ToDateparam.Name = "ToDate";
        ToDateparam.DefaultValue = myToDate;
        cryReportSource.Report.Parameters.Add(ToDateparam);
        	
		// 決定報表樣式
        if (ResultDt.Rows.Count > 0)
        {

            cryReportSource.ReportDocument.SetDataSource(ResultDt);

            CrystalReportViewer1.DataBind();
            CrystalReportViewer1.Visible = true;
            ViewState["Sourcedata"] = ResultDt;
        }
        else
        {  
            JsUtility.ClientMsgBoxAjax("查無相關資料！", UpdatePanel1, "");     
        }

	
		
		JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面
    }

    protected void btnClear_Click(object sender, ImageClickEventArgs e)
    {

    }

	

	// 傳回 公司別代號
	public string GetCompany()
	{
        return "";
        //return wddlCompany.Text;
	}
	// 傳回 起始科目欄位值
	public string GetFromAcno()
	{
        return "";
        //return wrvFromAcno.Text;
	}
	// 傳回 迄止科目欄位值
	public string GetToAcno()
    {
        return "";
        //return wrvToAcno.Text;
	}
    //protected void wddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //}
    public void checkMeNext(Int16 dest) {
        DropDownList me, Next, Perv,tmpddl,thisddl;
        me = (DropDownList)this.UpdatePanel1.FindControl("ddlTip" + dest.ToString());
        Next = null;
        Perv = null;
        tmpddl = null;
        thisddl=null;
        if (me.SelectedIndex > 0)
        {

            //if (dest <= 4) enbTCdt(Convert.ToInt16(dest + 1));
            if (dest == 1) {
                Next = (DropDownList)this.UpdatePanel1.FindControl("ddlTip" + Convert.ToString(dest + 1));
                enbTCdt(Convert.ToInt16(dest+1));
            }else
            {
                Perv = (DropDownList)this.UpdatePanel1.FindControl("ddlTip" + Convert.ToString(dest - 1));
                if (Perv.SelectedIndex == 0)
                {
                    Perv.SelectedIndex = me.SelectedIndex;
                    enbFCdt(dest);
                }
                if (dest <= 4)
                {
                    Next = (DropDownList)this.UpdatePanel1.FindControl("ddlTip" + Convert.ToString(dest + 1));
                    enbTCdt(Convert.ToInt16(dest + 1));
                }
            }

        }
        if (me.SelectedIndex == 0)
        {
            for (Int16 i = 5; i >= dest; i--) {
                thisddl = (DropDownList)this.UpdatePanel1.FindControl("ddlTip" + Convert.ToString(i - 1));
                tmpddl = (DropDownList)this.UpdatePanel1.FindControl("ddlTip" + Convert.ToString(i));
                if (tmpddl.SelectedIndex > 0 && thisddl.SelectedIndex == 0) {
                    chgDdlState(Convert.ToInt16(i), Convert.ToInt16(i - 1));
                    enbFCdt(Convert.ToInt16(i));
                }
                if (tmpddl.SelectedIndex == 0) {
                    enbFCdt(Convert.ToInt16(i));
                }
            }
            enbTCdt(Convert.ToInt16(dest+1));            
            //Perv = (DropDownList)this.UpdatePanel1.FindControl("ddlTip" + Convert.ToString(dest - 1));
            //Next = (DropDownList)this.UpdatePanel1.FindControl("ddlTip" + Convert.ToString(dest + 1));
            //if ((dest <= 4) && (Next != null)) if (Next.SelectedIndex == 0) { enbFCdt(Convert.ToInt16(dest + 1)); };
            //if ((dest >= 1) && (Perv != null)) if (Perv.SelectedIndex == 0) { enbFCdt(dest); };
        }
    }
    public void chgDdlState(Int16 src,Int16 dest) {
        ((Literal)this.UpdatePanel1.FindControl("lblTip" + dest.ToString())).Text = ((Literal)this.UpdatePanel1.FindControl("lblTip" + src.ToString())).Text;
        ((Literal)this.UpdatePanel1.FindControl("lblTip" + src.ToString())).Text = "";
        ((DropDownList)this.UpdatePanel1.FindControl("ddlTip" + dest.ToString())).SelectedIndex = ((DropDownList)this.UpdatePanel1.FindControl("ddlTip" + src.ToString())).SelectedIndex ;
        ((DropDownList)this.UpdatePanel1.FindControl("ddlTip" + src.ToString())).SelectedIndex = 0;
        ((TextBox)this.UpdatePanel1.FindControl("txtStart" + dest.ToString())).Text  = ((TextBox)this.UpdatePanel1.FindControl("txtStart" + src.ToString())).Text ;
        ((TextBox)this.UpdatePanel1.FindControl("txtStart" + src.ToString())).Text = "";
        ((TextBox)this.UpdatePanel1.FindControl("txtEnd" + dest.ToString())).Text = ((TextBox)this.UpdatePanel1.FindControl("txtEnd" + src.ToString())).Text ;
        ((TextBox)this.UpdatePanel1.FindControl("txtEnd" + src.ToString())).Text = "";
        ((CheckBox)this.UpdatePanel1.FindControl("cbxAmt" + dest.ToString())).Checked = ((CheckBox)this.UpdatePanel1.FindControl("cbxAmt" + src.ToString())).Checked ;
        ((CheckBox)this.UpdatePanel1.FindControl("cbxAmt" + src.ToString())).Checked = false;
    }
    public void enbTCdt(Int16 dest) {
        ((Literal)this.UpdatePanel1.FindControl("lblTip" + dest.ToString())).Text = "";
        ((DropDownList)this.UpdatePanel1.FindControl("ddlTip" + dest.ToString())).Enabled = true;
        ((TextBox)this.UpdatePanel1.FindControl("txtStart" + dest.ToString())).Enabled = true;
        ((TextBox)this.UpdatePanel1.FindControl("txtEnd" + dest.ToString())).Enabled = true;
        ((CheckBox)this.UpdatePanel1.FindControl("cbxAmt" + dest.ToString())).Enabled = true;
        
    }

    public void enbFCdt(Int16 dest)
    {
        ((Literal)this.UpdatePanel1.FindControl("lblTip" + dest.ToString())).Text = "";
        ((DropDownList)this.UpdatePanel1.FindControl("ddlTip" + dest.ToString())).Enabled = false;
        ((TextBox)this.UpdatePanel1.FindControl("txtStart" + dest.ToString())).Enabled = false;
        ((TextBox)this.UpdatePanel1.FindControl("txtEnd" + dest.ToString())).Enabled = false;
        ((CheckBox)this.UpdatePanel1.FindControl("cbxAmt" + dest.ToString())).Enabled = false;

    }

    protected void ddlTip2_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblTip2.Text = "(" + ddlTip2.SelectedValue + ")" + ddlTip2.SelectedItem;
        checkMeNext(2);
    }

    protected void ddlTip3_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblTip3.Text = "(" + ddlTip3.SelectedValue + ")" + ddlTip3.SelectedItem;
        checkMeNext(3);
    }

    protected void ddlTip4_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblTip4.Text = "(" + ddlTip4.SelectedValue + ")" + ddlTip4.SelectedItem;
        checkMeNext(4);
    }

    protected void ddlTip5_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblTip5.Text = "(" + ddlTip5.SelectedValue + ")" + ddlTip5.SelectedItem;
        checkMeNext(5);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        enbTCdt(2);

    }

    protected void ddlTip1_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblTip1.Text = "(" + ddlTip1.SelectedValue + ")" + ddlTip1.SelectedItem;
        checkMeNext(1);
    }


    /// <summary>
    /// 依照公司別更換代碼
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SelectIndex_Change(object sender, UserControl_CompanyList.SelectEventArgs e)
    {
        GetCodeDef();

    }

    /// <summary>
    /// 讀取欄位定義
    /// </summary>
    protected void GetCodeDef()
    {
        _MyDBM = new DBManger();
        _MyDBM.New();
        DataTable DT = new DataTable();
        string strSQL = "select ColId,ColId+'-'+ColName as ColName From GLColDef Where Company='" + DrpCompany.SelectValue + "'";

        DT = _MyDBM.ExecuteDataTable(strSQL);

      
        DataRow ndr = DT.NewRow();
        ndr["ColName"] = "請選擇欄位名稱";
        ndr["ColId"] = "";
        DT.Rows.InsertAt(ndr, 0);
        bindAllddlTip(DT);   
    
    }

    protected void wddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetCodeDef();  
    }

    public void bindAllddlTip(DataTable colDt) {
        for (Int16 i = 1; i <= 5; i ++ )
        {
            bindddlTip(((DropDownList)this.UpdatePanel1.FindControl("ddlTip" + i.ToString())), colDt);
        }
    }

    public void bindddlTip(DropDownList ddl,DataTable dt ) {
        ddl.DataTextField = "ColName";
        ddl.DataValueField = "ColId";
        ddl.DataSource = dt;
        ddl.DataBind();
    }

    protected void txtAcctNo1_TextChanged(object sender, EventArgs e)
    {
        enTxtAcctNo(1);
    }
    public void enTxtAcctNo(Int16 dest) {
        TextBox me = ((TextBox)this.UpdatePanel1.FindControl("txtAcctNo" + dest.ToString()));
        if (dest <= 3) {
            TextBox Next = ((TextBox)this.UpdatePanel1.FindControl("txtAcctNo" + (dest + 1).ToString()));
            if (me.Text.Length > 0) Next.Enabled = true;
            if (me.Text.Length == 0) Next.Enabled = false;
        }
        if (dest >= 2) {
            TextBox Perv = ((TextBox)this.UpdatePanel1.FindControl("txtAcctNo" + (dest - 1).ToString()));
            if (Perv.Text.Length == 0) me.Enabled = false;
        } 
    }
    protected void txtAcctNo2_TextChanged(object sender, EventArgs e)
    {
        enTxtAcctNo(2);
    }
    protected void txtAcctNo3_TextChanged(object sender, EventArgs e)
    {
        enTxtAcctNo(3);
    }
    protected void txtAcctNo4_TextChanged(object sender, EventArgs e)
    {
        enTxtAcctNo(4);
    }
    public void getList(Int16 dest)
    {
       
    }
  
  
  
  
   
   
   
 
  
}
