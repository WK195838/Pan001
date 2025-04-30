using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.Security;
using System.Web.SessionState;
using PanPacificClass;

public partial class GLA0110 : System.Web.UI.Page
{
    public string JournailMode = "";
    string cnn = string.Empty;   
    string mCalendarType;         //年制
    string mJournalNoType;        //傳票編號方式   
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLA0110";
    DBManger _MyDBM;
    //供比對用：
    //static char[] splitChar = { ',', ';', '|', '^' };
    static char[] splitChar = { Convert.ToChar(6), Convert.ToChar(7), Convert.ToChar(5), Convert.ToChar(8) };
    static string[] splitStr = { Convert.ToChar(6).ToString(), Convert.ToChar(7).ToString(), Convert.ToChar(5).ToString(), Convert.ToChar(8).ToString() };

    protected void Page_PreInit(object sender, EventArgs e)
    {
        //Page.Theme = "Theme_09";
        //if (Session["Theme"] != null)
        //    Page.Theme = Session["Theme"].ToString();

        //if (Session["MasterPage"] != null)
        //    Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";
    }



    protected void Page_Init(object sender, EventArgs e)
    {
        //cnn = PanUtility.PanDB.GetEEPCnnString();
        //cnn = ConfigurationManager.AppSettings["sqlConEBOS"];
        ////測試用
        //txtAllJournailzing.Text = "1,610304,610304,推銷費用－捐贈,摘要A,10000.5,0,";
        //txtAllJournailzing.Text += "11^部門別^N^Y^2100^金融服務業|";   //第一行
        //txtAllJournailzing.Text += "^^N^N^^|";                         //第二行
        //txtAllJournailzing.Text += "24^供應廠商^Y^Y^0007^行家|";       //第三行
        //txtAllJournailzing.Text += "^^N^N^^|";                         //第四行
        //txtAllJournailzing.Text += "19^月份^N^N^10^|";                 //第五行
        //txtAllJournailzing.Text += "^^N^N^^|";                         //第六行
        //txtAllJournailzing.Text += "^^N^N^^|";                         //第七行
        //txtAllJournailzing.Text += ";";
        //txtAllJournailzing.Text += "2,510402,510402,系統導入成本,摘要B,0,5500,";
        //txtAllJournailzing.Text += "11^部門別^N^Y^2200^工商服務業|"; //第一行
        //txtAllJournailzing.Text += "24^供應廠商^N^Y^0099^新力|";     //第二行
        //txtAllJournailzing.Text += "^^N^N^^|";                       //第三行
        //txtAllJournailzing.Text += "12^客戶別^N^Y^0012^聯合|";       //第四行
        //txtAllJournailzing.Text += "^^N^N^^|";                       //第五行
        //txtAllJournailzing.Text += "^^N^N^^|";                       //第六行
        //txtAllJournailzing.Text += "^^N^N^^|";                       //第七行
        //txtAllJournailzing.Text += ";";
        //txtAllJournailzing.Text += "3,111101,111101,零用金,摘要C,0,4500.5,";
        //txtAllJournailzing.Text += "^^N^N^^|";                       //第一行
        //txtAllJournailzing.Text += "^^N^N^^|";                       //第二行
        //txtAllJournailzing.Text += "^^N^N^^|";                       //第三行
        //txtAllJournailzing.Text += "^^N^N^^|";                       //第四行
        //txtAllJournailzing.Text += "^^N^N^^|";                       //第五行
        //txtAllJournailzing.Text += "^^N^N^^|";                       //第六行
        //txtAllJournailzing.Text += "^^N^N^^|";                       //第七行
        //txtAllJournailzing.Text += ";";


        
        if (Session["VoucherNo"] == null || Session["VoucherNo"].ToString().Trim() == "")
        {
            //string tCompany = Request.QueryString["Company"].ToString();
            //string tVoucherNo = Request.QueryString["VoucherNo"].ToString();
            //string tOPMode = Request.QueryString["OPMode"].ToString();
            //Session["Company"] = tCompany;
            //Session["VoucherNo"] = tVoucherNo;
            //Session["OPMode"] = tOPMode;
            //if (Session["VoucherNo"].ToString().Trim() == "")
            //{
                //暫先寫死在Master
                //Session["Company"] = "20";              //公司別
                Session["VoucherNo"] = "";   //"890131025";     //傳票號碼
                Session["OPMode"] = "new";              //作業模式
            //}
        }
        if (!Page.IsPostBack)
        {
            txtJournailOPMode.Text = Session["OPMode"].ToString().Trim().ToUpper();
            if (Session["Company"] != null && Session["VoucherNo"] != null)
            {
                //載入傳票資料
                GetJournailData(Session["Company"].ToString(), Session["VoucherNo"].ToString());
                //第一次進入Page，先配置畫面
                doPageInit();
                //作業權限設定畫面的[Button]及[各位欄位].Readonly
                doPageRight(Session["OPMode"].ToString());
            }
        }
        _MyDBM= new DBManger();
        _MyDBM.New();
        cnn = _MyDBM.GetConnectionString();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "1", Page.ResolveUrl("~/Scripts/jquery-1.4.4.min.js").ToString());
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "2", Page.ResolveUrl("~/Scripts/jquery-ui-1.8.7.custom.min.js").ToString());
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "3", Page.ResolveUrl("~/Scripts/ui.datepicker.js").ToString());
        //用於一般常用JS
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "A", Page.ResolveUrl("~/Pages/pagefunction.js").ToString());

        if (_UserInfo.SysSet.GetCalendarSetting().Equals("Y"))
        {//決定是否使用民國年
            Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "4", Page.ResolveUrl("~/Scripts/ui.datepicker.tw.js").ToString());
        }
        //用於執行等待畫面
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "B", Page.ResolveUrl("~/Pages/Busy.js").ToString());
         

        if (string.IsNullOrEmpty(_UserInfo.UData.UserId))
        {
            //取得登入資訊(Cookies亦可使用Session指定)        
            _UserInfo.UData = _UserInfo.GetUData(Session.SessionID);
        }

        string strScript = string.Empty;
        ScriptManager.RegisterStartupScript(ScriptManager1, this.GetType(), "", @"JQ();", true);
        txtJournailDate.CssClass = "JQCalendar";
        txtReturnDate.CssClass = "JQCalendar";
        JournailMode = "&nbsp;傳票資料&nbsp;&nbsp;[";
        if (Session["OPMode"].ToString().ToUpper() == "NEW")
        {
            JournailMode += "新增";
        }
        else if (Session["OPMode"].ToString().ToUpper() == "EDIT")
        {
            JournailMode += "編輯";
        }
        else if (Session["OPMode"].ToString().ToUpper() == "NULLIFY")
        {
            JournailMode += "作廢";
        }
        else
        {
            JournailMode += "查詢";
        }
        JournailMode += "]&nbsp;";
        //重畫分錄的Table表
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "onlaod", "drowJournailList(document.getElementById('" + txtAllJournailzing.ClientID + "'));", true);
        // GLbody.Attributes.Add("onload", "return drowJournailList(document.getElementById('" + txtAllJournailzing.ClientID + "'));");

        //設定[image]觸發的Javascript Function
        //新增分錄
        imgAddJournailzing.Attributes.Add("onclick", "return AddItem('" + txtSingleJournailzing.ClientID + "');");
        imgAddJournailzing.Attributes.Add("style", "cursor:hand;");
        //編輯分錄
        imgEditJournailzing.Attributes.Add("onclick", "return EditItem('" + txtSingleJournailzing.ClientID + "');");
        imgEditJournailzing.Attributes.Add("style", "cursor:hand;");
        //刪除分錄
        imgDeleteJournailzing.Attributes.Add("onclick", "return DeleteItem('" + txtSingleJournailzing.ClientID + "','" + txtAllJournailzing.ClientID + "');");
        imgDeleteJournailzing.Attributes.Add("style", "cursor:hand;");

        ////設定[imageButton]觸發的Javascript Function
        //imgbtnOKJournailzing.Attributes.Add("onclick", "return OKJournailzing('" + txtSingleJournailzing.ClientID + "');");
       
        if (!Page.IsPostBack)
        {      
            //imgAcctNo加入onclick事件GetAcctnoPopupDialog()
            imgAcctNo.Attributes.Add("onclick", "return GetAcctnoPopupDialog(550, 400, '" + txtCompany.Text.ToString().Trim() + "', 'GLAcctDef', 'CodeCode,CodeName', '" + txtAcctNo.ClientID + "','" + lblAcctNo.ClientID + "','" + txtReValue.ClientID + "');");
            imgAcctNo.Attributes.Add("style", "cursor:hand;");
        }

        imgbtnCloseJournail.Attributes.Add("onclick", "window.close();");

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //傳票分錄 Session("DisplayContent") == "Y" 顯示 否則 隱藏
        //if (Session["DisplayContent"] == "Y")
        //{
        //    content.Visible = true;
        //}
        //else
        //{
        if (txtAllJournailzing.Text.ToString().Trim() == "")
        {
            content.Visible = false;
        }
        else
        {
            //Session["DisplayContent"] = "Y";
            content.Visible = true;
        }
        //}
        string[] tString = txtAllJournailzing.Text.ToString().Split(splitChar[1]);
        if (tString.Length > 6)
        {
            Panelx.Attributes.Add("Height", "145px");
            Panelx.Attributes.Add("ScrollBars", "Vertical");
            //Panelx.Height = "145px";
            //Panelx.ScrollBars = "Vertical";
        }
        else
        {
            Panelx.Attributes.Add("Height", "");
            Panelx.Attributes.Add("ScrollBars", "None");
            //Panelx.Height = "";
            //Panelx.ScrollBars = "None";
        }
    }
    protected void txtAcctNo_TextChanged(object sender, EventArgs e)
    {
        doTextChanged();
    }

    //
    protected void doPageInit()
    {

        for (int i = 1; i < 8; i++)
        {
            System.Web.UI.HtmlControls.HtmlImage theimgICO = (System.Web.UI.HtmlControls.HtmlImage)this.FindControl(txtJournailType.UniqueID.Replace("txtJournailType", "imgICO" + i.ToString()));
            TextBox theValueText = (TextBox)this.FindControl(txtJournailType.UniqueID.Replace("txtJournailType", "txtValueText" + i.ToString()));
            TextBox thelblDesc = (TextBox)this.FindControl(txtJournailType.UniqueID.Replace("txtJournailType", "lblDesc" + i.ToString()));
            TextBox thetxtDesc = (TextBox)this.FindControl(txtJournailType.UniqueID.Replace("txtJournailType", "txtDesc" + i.ToString()));
            TextBox thetxtInputYN = (TextBox)this.FindControl(txtJournailType.UniqueID.Replace("txtJournailType", "txtInputYN" + i.ToString()));
            TextBox thetxtReValue = (TextBox)this.FindControl(txtJournailType.UniqueID.Replace("txtJournailType", "txtReValue" + i.ToString()));
            thetxtReValue.Attributes.Add("style", "display:none");
            thetxtDesc.Attributes.Add("ReadOnly", "ReadOnly");
            thetxtDesc.Attributes.Add("style", "display:none");
            thetxtInputYN.Attributes.Add("ReadOnly", "ReadOnly");
            theimgICO.Attributes.Add("style", "display:none");
            theValueText.Attributes.Add("style", "display:none");
            thelblDesc.Attributes.Add("ReadOnly", "ReadOnly");
            thelblDesc.Attributes.Add("style", "display:none");
        }

        //設定傳回字串欄位為隱藏狀態
        txtReValue.Attributes.Add("style", "display:none");
        txtReValue8.Attributes.Add("style", "display:none");
        txtReValue9.Attributes.Add("style", "display:none");
        //傳票分錄用的暫存字串
        txtSingleJournailzing.Attributes.Add("style", "display:none");
        txtOPMode.Attributes.Add("style", "display:none");
        txtAllJournailzing.Attributes.Add("style", "display:none");
        txtJournailOPMode.Attributes.Add("style", "display:none");

        //設定TextBox為ReadOnly狀態
        lblAcctNo.Attributes.Add("ReadOnly", "ReadOnly");

        txtJournailType.Attributes.Add("ReadOnly", "ReadOnly");
        txtJournailType.Attributes.Add("style", "display:none");

        txtInputYN8.Attributes.Add("ReadOnly", "ReadOnly");
        txtInputYN9.Attributes.Add("ReadOnly", "ReadOnly");

        lblDesc8.Attributes.Add("ReadOnly", "ReadOnly");
        lblDesc9.Attributes.Add("ReadOnly", "ReadOnly");

        lblDesc8.Attributes.Add("style", "display:none");
        lblDesc9.Attributes.Add("style", "display:none");  




        ////設定傳回字串欄位為隱藏狀態
        //txtReValue.Attributes.Add("style", "display:none");
        //txtReValue1.Attributes.Add("style", "display:none");
        //txtReValue2.Attributes.Add("style", "display:none");
        //txtReValue3.Attributes.Add("style", "display:none");
        //txtReValue4.Attributes.Add("style", "display:none");
        //txtReValue5.Attributes.Add("style", "display:none");
        //txtReValue6.Attributes.Add("style", "display:none");
        //txtReValue7.Attributes.Add("style", "display:none");
        //txtReValue8.Attributes.Add("style", "display:none");
        //txtReValue9.Attributes.Add("style", "display:none");
        ////傳票分錄用的暫存字串
        //txtSingleJournailzing.Attributes.Add("style", "display:none");
        //txtOPMode.Attributes.Add("style", "display:none");
        //txtAllJournailzing.Attributes.Add("style", "display:none");
        //txtJournailOPMode.Attributes.Add("style", "display:none");

        ////設定TextBox為ReadOnly狀態
        //lblAcctNo.Attributes.Add("ReadOnly", "ReadOnly");

        //txtJournailType.Attributes.Add("ReadOnly", "ReadOnly");
        //txtDesc2.Attributes.Add("ReadOnly", "ReadOnly");
        //txtDesc3.Attributes.Add("ReadOnly", "ReadOnly");
        //txtDesc4.Attributes.Add("ReadOnly", "ReadOnly");
        //txtDesc5.Attributes.Add("ReadOnly", "ReadOnly");
        //txtDesc6.Attributes.Add("ReadOnly", "ReadOnly");
        //txtDesc7.Attributes.Add("ReadOnly", "ReadOnly");
        //txtJournailType.Attributes.Add("style", "display:none");
        //txtDesc2.Attributes.Add("style", "display:none");
        //txtDesc3.Attributes.Add("style", "display:none");
        //txtDesc4.Attributes.Add("style", "display:none");
        //txtDesc5.Attributes.Add("style", "display:none");
        //txtDesc6.Attributes.Add("style", "display:none");
        //txtDesc7.Attributes.Add("style", "display:none");

        //txtInputYN1.Attributes.Add("ReadOnly", "ReadOnly");
        //txtInputYN2.Attributes.Add("ReadOnly", "ReadOnly");
        //txtInputYN3.Attributes.Add("ReadOnly", "ReadOnly");
        //txtInputYN4.Attributes.Add("ReadOnly", "ReadOnly");
        //txtInputYN5.Attributes.Add("ReadOnly", "ReadOnly");
        //txtInputYN6.Attributes.Add("ReadOnly", "ReadOnly");
        //txtInputYN7.Attributes.Add("ReadOnly", "ReadOnly");
        //txtInputYN8.Attributes.Add("ReadOnly", "ReadOnly");
        //txtInputYN9.Attributes.Add("ReadOnly", "ReadOnly");

        //imgICO1.Attributes.Add("style", "display:none");
        //imgICO2.Attributes.Add("style", "display:none");
        //imgICO3.Attributes.Add("style", "display:none");
        //imgICO4.Attributes.Add("style", "display:none");
        //imgICO5.Attributes.Add("style", "display:none");
        //imgICO6.Attributes.Add("style", "display:none");
        //imgICO7.Attributes.Add("style", "display:none");

        //txtValueText1.Attributes.Add("style", "display:none");
        //txtValueText2.Attributes.Add("style", "display:none");
        //txtValueText3.Attributes.Add("style", "display:none");
        //txtValueText4.Attributes.Add("style", "display:none");
        //txtValueText5.Attributes.Add("style", "display:none");
        //txtValueText6.Attributes.Add("style", "display:none");
        //txtValueText7.Attributes.Add("style", "display:none");
        
        //lblDesc1.Attributes.Add("ReadOnly", "ReadOnly");
        //lblDesc2.Attributes.Add("ReadOnly", "ReadOnly");
        //lblDesc3.Attributes.Add("ReadOnly", "ReadOnly");
        //lblDesc4.Attributes.Add("ReadOnly", "ReadOnly");
        //lblDesc5.Attributes.Add("ReadOnly", "ReadOnly");
        //lblDesc6.Attributes.Add("ReadOnly", "ReadOnly");
        //lblDesc7.Attributes.Add("ReadOnly", "ReadOnly");
        //lblDesc8.Attributes.Add("ReadOnly", "ReadOnly");
        //lblDesc9.Attributes.Add("ReadOnly", "ReadOnly");
        //lblDesc1.Attributes.Add("style", "display:none");
        //lblDesc2.Attributes.Add("style", "display:none");
        //lblDesc3.Attributes.Add("style", "display:none");
        //lblDesc4.Attributes.Add("style", "display:none");
        //lblDesc5.Attributes.Add("style", "display:none");
        //lblDesc6.Attributes.Add("style", "display:none");
        //lblDesc7.Attributes.Add("style", "display:none");
        //lblDesc8.Attributes.Add("style", "display:none");
        //lblDesc9.Attributes.Add("style", "display:none");  
    }

    protected void ClearItem()
    {
        //分錄內容：會計科目
        txtSNo.Text = ""; txtAcctNo.Text = ""; lblAcctNo.Text = ""; txtDBMoney.Text = ""; txtCRMoney.Text = ""; txtP2.Text = "";
        //分錄雜項
        txtValueText8.Text = ""; txtValueText9.Text = ""; txtValueText10.Text = "";
        txtReValue8.Text = ""; txtReValue9.Text = "";
        for (int i = 1; i < 8; i++)
        {//分錄明細：會計科目之明細
            System.Web.UI.HtmlControls.HtmlImage theimgICO = (System.Web.UI.HtmlControls.HtmlImage)this.FindControl(txtJournailType.UniqueID.Replace("txtJournailType", "imgICO" + i.ToString()));
            TextBox theValueText = (TextBox)this.FindControl(txtJournailType.UniqueID.Replace("txtJournailType", "txtValueText" + i.ToString()));
            TextBox thelblDesc = (TextBox)this.FindControl(txtJournailType.UniqueID.Replace("txtJournailType", "lblDesc" + i.ToString()));
            TextBox thetxtDesc = (TextBox)this.FindControl(txtJournailType.UniqueID.Replace("txtJournailType", "txtDesc" + i.ToString()));
            TextBox thetxtInputYN = (TextBox)this.FindControl(txtJournailType.UniqueID.Replace("txtJournailType", "txtInputYN" + i.ToString()));
            TextBox thetxtReValue = (TextBox)this.FindControl(txtJournailType.UniqueID.Replace("txtJournailType", "txtReValue" + i.ToString()));
            #region 動態分錄先做隱藏＆清空
            if (i == 1)
            {
                txtJournailType.Attributes.Add("style", "display:none");
                //txtJournailType.Text = "";
            }
            theimgICO.Attributes.Remove("onclick");
            theimgICO.Attributes.Add("style", "display:none");
            theValueText.Attributes.Add("style", "display:none");
            theValueText.Text = "";
            thelblDesc.Attributes.Add("style", "display:none");
            thelblDesc.Text = "";
            thetxtDesc.Attributes.Add("style", "display:none");
            thetxtDesc.Text = "";
            thetxtInputYN.Attributes.Add("style", "display:none");
            thetxtInputYN.Text = "";
            thetxtReValue.Text = "";
            #endregion
        }
    }

    /// <summary>
    /// 作業模式設定欄位是否可以維護
    /// </summary>
    /// <param name="tMode">作業模式</param>
    /// <returns></returns>
    /// <remarks>tMode="new" 新增傳票</remarks>
    /// <remarks>tMode="edit" 編輯傳票</remarks>
    /// <remarks>tMode="nullify" 作廢傳票</remarks>
    /// <remarks>tMode="query" 查詢傳票</remarks>
    protected void doPageRight(string tMode)
    {
        switch (tMode.ToUpper())
        {
            case "QUERY"://查詢傳票
                #region
                //傳票表頭
                txtCompany.Attributes.Add("ReadOnly", "ReadOnly");
                txtJournailNo.Attributes.Add("ReadOnly", "ReadOnly");
                //ibOW_JournailDate.Attributes.Add("style", "display:none");
                // imgbtnJournailDate.Attributes.Add("style", "display:none");
                txtJournailDate.Attributes.Add("ReadOnly", "ReadOnly");
                txtJournailType.Attributes.Add("style", "display:inline");
                txtJournailType.Text = ddlstJournailType.SelectedItem.ToString();
                ddlstJournailType.Visible = false;
                //ddlstJournailType.Attributes.Add("ReadOnly", "ReadOnly");
                //ibOW_RevDate.Attributes.Add("style", "display:none");
                //imgbtnReturnDate.Attributes.Add("style", "display:none");
                txtReturnDate.Attributes.Add("ReadOnly", "ReadOnly");
                imgbtnPrintAndSaveJournail.Attributes.Add("style", "display:none");
                imgbtnSaveJourmail.Attributes.Add("style", "display:none");
                imgbtnNewJourail.Attributes.Add("style", "display:none");
                imgbtnCopyToNewJourail.Attributes.Add("style", "display:none");
                //imgDeleteJournailzing.Attributes.Add("style", "display:none");
                //imgAddJournailzing.Attributes.Add("style", "display:none");
                //imgEditJournailzing.Attributes.Add("style", "display:none");
                imgDeleteJournailzing.Visible = false;
                imgEditJournailzing.Visible = false;
                imgAddJournailzing.Visible = false;

                //傳票分錄明細
                txtSNo.Attributes.Add("ReadOnly", "ReadOnly");
                imgAcctNo.Visible = false;
                txtAcctNo.Attributes.Add("ReadOnly", "ReadOnly");
                txtDBMoney.Attributes.Add("ReadOnly", "ReadOnly");
                txtCRMoney.Attributes.Add("ReadOnly", "ReadOnly");
                txtP1.Attributes.Add("ReadOnly", "ReadOnly");
                txtP2.Attributes.Add("ReadOnly", "ReadOnly");
                txtP3.Attributes.Add("ReadOnly", "ReadOnly");

                imgICO8.Visible = false;
                txtValueText8.Attributes.Add("ReadOnly", "ReadOnly");
                imgICO9.Visible = false;
                txtValueText9.Attributes.Add("ReadOnly", "ReadOnly");
                imgbtnOKJournailzing.Attributes.Add("style", "display:none");
                imgbtnReset.Attributes.Add("style", "display:none");
                #endregion
                break;
            case "EDIT"://更正傳票
                #region
                imgbtnNewJourail.Attributes.Add("style", "display:none");
                imgbtnCopyToNewJourail.Attributes.Add("style", "display:none");
                #endregion
                break;
        }
    }

    /// <summary>
    /// 將陣列資料轉成DataTable
    /// </summary>
    /// <param name="DataArray"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    protected static DataTable GetDataTable(string[][] DataArray)
    {
        DataTable myDataTable=new DataTable();
        DataColumn myDataColumn;
        if (DataArray !=null)
        {
            for (int i=1; i<=DataArray.GetLength(1)+1; i++)
            {
                myDataColumn = new DataColumn();
                myDataColumn.DataType = System.Type.GetType("System.String");
                myDataColumn.ColumnName = i.ToString();
                myDataTable.Columns.Add(myDataColumn);
            }

            DataRow row;
            for (int i=0; i<=DataArray.GetLength(0)-1; i++)
            {
                row = myDataTable.NewRow();
                for (int j=0; j<=DataArray.GetLength(1)-1; j++)
                {
                    row[j]=DataArray[i][j];
                }
                myDataTable.Rows.Add(row);
            }
        }

        return myDataTable;
    }
    /// <summary>
    /// 將暫存欄位的資料轉成陣列
    /// </summary>
    /// <param name="al">投資標的資料內容</param>
    /// <returns></returns>
    /// <remarks></remarks>
    protected static string[][] GetTempArray(string al)
    {
        // 取得字尾碼，欄位分隔符號
        char mskCol = splitChar[0];
        char mskRow = splitChar[1];

        string[][] TempArray = null;
        if (al.Length > 0)
        {
            string[] ArrayTemp = al.Split(mskRow);
            //TempArray = new string[al.Count - 1, ArrayTemp.Length - 1] { };
            for (int i = 0; i < ArrayTemp.Length; i++)
            {
                TempArray[i] = ArrayTemp[i].ToString().Split(mskCol);
                //TempArray[i] = new string[ArrayTemp.Length - 1] { };
                //ArrayTemp = al[i].ToString().Split(mskCol);
                //for (int j = 0; j <= ArrayTemp.Length - 1; j++)
                //    TempArray[i, j] = ArrayTemp[j];
            }
        }

        return TempArray;
    }

    protected void imgbtnReset_Click(object sender, ImageClickEventArgs e)
    {
        //txtReValue暫存字串清空
        txtReValue.Text = "";
    }
    
    protected void txtValueText1_TextChanged(object sender, EventArgs e)
    {
        //doTextChanged();
    }
    protected void txtValueText2_TextChanged(object sender, EventArgs e)
    {
        //doTextChanged();
    }
    protected void txtValueText3_TextChanged(object sender, EventArgs e)
    {
        //doTextChanged();
    }
    protected void txtValueText4_TextChanged(object sender, EventArgs e)
    {
        //doTextChanged();
    }
    protected void txtValueText5_TextChanged(object sender, EventArgs e)
    {
        //doTextChanged();
    }
    protected void txtValueText6_TextChanged(object sender, EventArgs e)
    {
        //doTextChanged();
    }
    protected void txtValueText7_TextChanged(object sender, EventArgs e)
    {
        //doTextChanged();
    }
    /// <summary>
    /// 設定各項動機TextChange物件是否顯示
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    /// <remarks></remarks>
    protected void doTextChanged()
    {
        #region 會計科目變更時，重畫分錄明細
        for (int i = 1; i < 8; i++)
        {
            System.Web.UI.HtmlControls.HtmlImage theimgICO = (System.Web.UI.HtmlControls.HtmlImage)this.FindControl(txtJournailType.UniqueID.Replace("txtJournailType", "imgICO" + i.ToString()));
            TextBox theValueText = (TextBox)this.FindControl(txtJournailType.UniqueID.Replace("txtJournailType", "txtValueText" + i.ToString()));
            TextBox thelblDesc = (TextBox)this.FindControl(txtJournailType.UniqueID.Replace("txtJournailType", "lblDesc" + i.ToString()));
            TextBox thetxtDesc = (TextBox)this.FindControl(txtJournailType.UniqueID.Replace("txtJournailType", "txtDesc" + i.ToString()));
            TextBox thetxtInputYN = (TextBox)this.FindControl(txtJournailType.UniqueID.Replace("txtJournailType", "txtInputYN" + i.ToString()));
            TextBox thetxtReValue = (TextBox)this.FindControl(txtJournailType.UniqueID.Replace("txtJournailType", "txtReValue" + i.ToString()));
            #region 動態分錄先做隱藏＆清空
            if (i == 1)
            {
                txtJournailType.Attributes.Add("style", "display:none");
                //txtJournailType.Text = "";
            }
            theimgICO.Attributes.Remove("onclick");
            theimgICO.Attributes.Add("style", "display:none");
            theValueText.Attributes.Add("style", "display:none");
            theValueText.Text = "";
            thelblDesc.Attributes.Add("style", "display:none");
            thelblDesc.Text = "";
            thetxtDesc.Attributes.Add("style", "display:none");
            thetxtDesc.Text = "";
            thetxtInputYN.Attributes.Add("style", "display:none");
            thetxtInputYN.Text = "";            
            #endregion
            string[] spl;
            //要先將待回傳值存著才不會被清空
            spl = thetxtReValue.Text.ToString().Split(splitChar[3]);
            //清空待回傳值
            thetxtReValue.Text = "";
            if (spl.Length > 3)
            {//不為空時時,至少能切成4組字串
                #region 將暫存資料放回原位並打開顯示
                thetxtDesc.Text = spl[1];
                if (spl[2] != "N")
                    thetxtInputYN.Text = spl[2];
                if (spl.Length > 4)
                    theValueText.Text = spl[4];
                else if (Request.Form[theValueText.UniqueID] != null)
                    theValueText.Text = Request.Form[theValueText.UniqueID];
                if (i == 1)
                    txtJournailType.Attributes.Add("style", "display:inline");
                else
                    thetxtDesc.Attributes.Add("style", "display:inline");
                thetxtInputYN.Attributes.Add("style", "display:inline");
                theValueText.Attributes.Add("style", "display:inline");
                if (Session["OPMode"].ToString().ToUpper() != "NEW" && Session["OPMode"].ToString().ToUpper() != "EDIT")
                {
                    theValueText.Attributes.Add("ReadOnly", "ReadOnly");
                }
                thelblDesc.Attributes.Add("style", "display:inline");
                if ((spl[3] == "Y") && (Session["OPMode"].ToString().ToUpper() == "NEW" || Session["OPMode"].ToString().ToUpper() == "EDIT"))
                {
                    #region 重組按鈕開窗參數
                    if (Session["OPMode"].ToString().ToUpper() == "NEW" || Session["OPMode"].ToString().ToUpper() == "EDIT")
                    {
                        theimgICO.Attributes.Add("style", "display:inline");
                        theimgICO.Attributes.Add("style", "cursor:hand;");
                    }
                    else
                    {
                        theimgICO.Attributes.Add("style", "display:none");
                    }
                    theimgICO.Attributes.Add("onclick", "return GetPopupDialog(530, 400, '"
                        + txtCompany.Text.ToString().Trim() + "', '" + spl[0] + "', 'CodeCode,CodeName', '"
                        + theValueText.ClientID + "','" + thelblDesc.ClientID + "','" + thetxtReValue.ClientID + "','"
                        + thetxtReValue.Text.ToString().Trim() + "');");
                    #endregion
                }
                thelblDesc.Text = GetJournailzingName(txtCompany.Text.ToString().Trim(),
                                                spl[0],
                                                theValueText.Text.ToString().Trim());
                //改變的值存回暫存欄位
                thetxtReValue.Text = spl[0] + splitChar[3] + spl[1] + splitChar[3] + spl[2] + splitChar[3] + spl[3] + splitChar[3]
                    + theValueText.Text.ToString().Trim() + splitChar[3] + thelblDesc.Text.ToString().Trim();
                #endregion
            }
        }
        #endregion
    }

    /// <summary>
    /// 取得動態行列分錄明細名稱
    /// </summary>
    /// <param name="tCompany">公司別</param>
    /// <param name="tColID">行列代號</param>
    /// <param name="tCodeCode">動態分錄代號</param>
    /// <returns>分錄明細名稱</returns>
    /// <remarks></remarks>
    protected string GetJournailzingName(string tCompany, string tColID, string tCodeCode)
    {
        _MyDBM = new DBManger();
        _MyDBM.New();
        cnn = _MyDBM.GetConnectionString();       
        string tCodeName = "";
        string sql = "select CodeName=dbo.fnGetAcnoIdxName(@Company,@ColID,@CodeCode)";
        DataTable dt = new DataTable();
        SqlDataAdapter adpt = new SqlDataAdapter(sql, cnn);
        try
        {
            adpt.SelectCommand.Parameters.Add("@Company", SqlDbType.Char, 2).Value = tCompany;
            adpt.SelectCommand.Parameters.Add("@ColID", SqlDbType.Char, 2).Value = tColID;
            adpt.SelectCommand.Parameters.Add("@CodeCode", SqlDbType.Char, 20).Value = tCodeCode;

            adpt.Fill(dt);
            tCodeName = dt.Rows[0]["CodeName"].ToString();
            //Response.Write(sqlReturn);
        }
        catch (InvalidCastException e)
        {
            throw (e);
        }
        finally
        {
            
        }

        return tCodeName;
    }
    protected void imgbtnOKJournailzing_Click(object sender, ImageClickEventArgs e)
    {
        bool blcheck = true;
        string ErrMsg = "";
        if (txtDBMoney.Text.ToString().Trim() == "" && txtCRMoney.Text.ToString().Trim() == "")
        {
            blcheck = false;
            ErrMsg = "分錄儲存失敗!借方金額/貸方金額請擇一填寫!";
        }
        else
        {
            #region 借貸金額檢核
            float iDBM = 0;
            float iCRM = 0;
            if (txtDBMoney.Text.ToString().Trim() != "" && float.TryParse(txtDBMoney.Text.ToString().Trim().Replace(",", ""), out iDBM) == false)
            {
                blcheck = false;
                ErrMsg = "分錄儲存失敗!借方金額非數字!";
            }

            if (txtCRMoney.Text.ToString().Trim() != "" && float.TryParse(txtCRMoney.Text.ToString().Trim().Replace(",", ""), out iCRM) == false)
            {
                blcheck = false;
                ErrMsg = "分錄儲存失敗!貸方金額非數字!";
            }

            if (blcheck == true && iDBM > 0 && iCRM > 0)
            {
                blcheck = false;
                ErrMsg = "分錄儲存失敗!借方金額與貸方金額僅可擇一填寫!";
            }
            #endregion
        }
        if (blcheck == false)
        {
            JsUtility.ClientMsgBoxAjax(ErrMsg, UpdatePanel1, "Message");
            return;
        }
        string[] tOPMode = txtOPMode.Text.ToString().Split('=');
        txtSingleJournailzing.Text = txtSNo.Text.ToString().Trim() + splitChar[0];
        txtSingleJournailzing.Text += txtAcctNo.Text.ToString().Trim().Replace(splitStr[0], "") + splitChar[0];
        txtSingleJournailzing.Text += txtAcctNo.Text.ToString().Trim().Replace(splitStr[0], "") + splitChar[0];
        txtSingleJournailzing.Text += lblAcctNo.Text.ToString().Trim().Replace(splitStr[0], "") + splitChar[0];
        txtSingleJournailzing.Text += txtValueText10.Text.ToString().Trim().Replace(splitStr[0], "") + splitChar[0];
        if (txtDBMoney.Text.ToString().Trim() == "")
            txtSingleJournailzing.Text += "0" + splitChar[0];
        else
            txtSingleJournailzing.Text += txtDBMoney.Text.ToString().Trim().Replace(",", "").Replace(splitStr[0], "") + splitChar[0];
        if (txtCRMoney.Text.ToString().Trim() == "")
            txtSingleJournailzing.Text += "0" + splitChar[0];
        else
            txtSingleJournailzing.Text += txtCRMoney.Text.ToString().Trim().Replace(",", "").Replace(splitStr[0], "") + splitChar[0];
        #region 處理會計科目之分錄明細
        //第一行
        string[] tReValue = txtReValue1.Text.ToString().Replace(splitStr[0], "").Split(splitChar[3]);
        #region
        if (tReValue.Length < 4)
        {
            JsUtility.ClientMsgBoxAjax("明細資料不足!!", UpdatePanel2, "Message");
            return;
        }

        txtSingleJournailzing.Text += tReValue[0] + splitChar[3];
        txtSingleJournailzing.Text += tReValue[1] + splitChar[3];
        txtSingleJournailzing.Text += tReValue[2] + splitChar[3];
        txtSingleJournailzing.Text += tReValue[3].Trim() + splitChar[3];
        txtSingleJournailzing.Text += txtValueText1.Text.ToString().Trim() + splitChar[3];
        txtSingleJournailzing.Text += lblDesc1.Text.ToString().Trim() + splitChar[2];
        #endregion
        //第二行
        tReValue = txtReValue2.Text.ToString().Replace(splitStr[0], "").Split(splitChar[3]);
        #region
        txtSingleJournailzing.Text += tReValue[0] + splitChar[3];
        txtSingleJournailzing.Text += tReValue[1] + splitChar[3];
        txtSingleJournailzing.Text += tReValue[2] + splitChar[3];
        txtSingleJournailzing.Text += tReValue[3].Trim() + splitChar[3];
        txtSingleJournailzing.Text += txtValueText2.Text.ToString().Trim() + splitChar[3];
        txtSingleJournailzing.Text += lblDesc2.Text.ToString().Trim() + splitChar[2];
        #endregion
        //第三行
        tReValue = txtReValue3.Text.ToString().Replace(splitStr[0], "").Split(splitChar[3]);
        #region
        txtSingleJournailzing.Text += tReValue[0] + splitChar[3];
        txtSingleJournailzing.Text += tReValue[1] + splitChar[3];
        txtSingleJournailzing.Text += tReValue[2] + splitChar[3];
        txtSingleJournailzing.Text += tReValue[3].Trim() + splitChar[3];
        txtSingleJournailzing.Text += txtValueText3.Text.ToString().Trim() + splitChar[3];
        txtSingleJournailzing.Text += lblDesc3.Text.ToString().Trim() + splitChar[2];
        #endregion
        //第四行
        tReValue = txtReValue4.Text.ToString().Replace(splitStr[0], "").Split(splitChar[3]);
        #region
        txtSingleJournailzing.Text += tReValue[0] + splitChar[3];
        txtSingleJournailzing.Text += tReValue[1] + splitChar[3];
        txtSingleJournailzing.Text += tReValue[2] + splitChar[3];
        txtSingleJournailzing.Text += tReValue[3].Trim() + splitChar[3];
        txtSingleJournailzing.Text += txtValueText4.Text.ToString().Trim() + splitChar[3];
        txtSingleJournailzing.Text += lblDesc4.Text.ToString().Trim() + splitChar[2];
        #endregion
        //第五行
        tReValue = txtReValue5.Text.ToString().Replace(splitStr[0], "").Split(splitChar[3]);
        #region
        txtSingleJournailzing.Text += tReValue[0] + splitChar[3];
        txtSingleJournailzing.Text += tReValue[1] + splitChar[3];
        txtSingleJournailzing.Text += tReValue[2] + splitChar[3];
        txtSingleJournailzing.Text += tReValue[3].Trim() + splitChar[3];
        txtSingleJournailzing.Text += txtValueText5.Text.ToString().Trim() + splitChar[3];
        txtSingleJournailzing.Text += lblDesc5.Text.ToString().Trim() + splitChar[2];
        #endregion
        //第六行
        tReValue = txtReValue6.Text.ToString().Replace(splitStr[0], "").Split(splitChar[3]);
        #region
        txtSingleJournailzing.Text += tReValue[0] + splitChar[3];
        txtSingleJournailzing.Text += tReValue[1] + splitChar[3];
        txtSingleJournailzing.Text += tReValue[2] + splitChar[3];
        txtSingleJournailzing.Text += tReValue[3].Trim() + splitChar[3];
        txtSingleJournailzing.Text += txtValueText6.Text.ToString().Trim() + splitChar[3];
        txtSingleJournailzing.Text += lblDesc6.Text.ToString().Trim() + splitChar[2];
        #endregion
        //第七行
        tReValue = txtReValue7.Text.ToString().Replace(splitStr[0], "").Split(splitChar[3]);
        #region
        txtSingleJournailzing.Text += tReValue[0] + splitChar[3];
        txtSingleJournailzing.Text += tReValue[1] + splitChar[3];
        txtSingleJournailzing.Text += tReValue[2] + splitChar[3];
        txtSingleJournailzing.Text += tReValue[3].Trim() + splitChar[3];
        txtSingleJournailzing.Text += txtValueText7.Text.ToString().Trim() + splitChar[3];
        txtSingleJournailzing.Text += lblDesc7.Text.ToString().Trim() + splitChar[2];
        #endregion
        #endregion
        txtSingleJournailzing.Text += splitStr[1];

        int SelItem = 0;
        if (tOPMode[0] == "add")            //新增分錄
        {
            txtAllJournailzing.Text += txtSingleJournailzing.Text.ToString().Trim();
        }
        else if (tOPMode[0] == "edit")      //編輯分錄
        {
            string[] sItem = txtAllJournailzing.Text.ToString().Trim().Split(splitChar[1]);

            for (int i = 0; i < sItem.Length - 1; i++)
            {
                string[] Items = sItem[i].Split(splitChar[0]);
                if (Items[0] == tOPMode[1])
                    SelItem = i;
            }

            txtAllJournailzing.Text = "";
            for (int i = 0; i < sItem.Length - 1; i++)
            {
                if (SelItem != i)
                    txtAllJournailzing.Text += sItem[i] + splitChar[1];
                else
                    txtAllJournailzing.Text += txtSingleJournailzing.Text;

            }
        }

        //if (tOPMode[0] != "add")   //若是新增則保持在新增狀態
        //{
        txtOPMode.Text = "select=";
        lblLegend.Text = " 傳票分錄明細 ";
        //}
        //else
        //{
        //    lblLegend.Text = " 傳票分錄明細 [新增] ";
        //}
        //重排序號        
        #region
        string[] txtJourAll = txtAllJournailzing.Text.Split(splitChar[1]);
        string[] a = new string[txtJourAll.Length - 1];
        //string msg = "";
        for (int i = 0; i < txtJourAll.Length - 1; i++)
        {
            string[] txtJourSub = txtJourAll[i].Trim().Split(splitChar[0]);

            //msg += i.ToString() + "---";

            //oAjax.ClientMsgBoxAjax(i.ToString(), UpdatePanel2, "");
            if (Convert.ToDecimal(txtJourSub[5].ToString()) - Convert.ToDecimal(txtJourSub[6].ToString()) > 0)   //借方
            {
                a[i] = "1" + txtJourSub[1].Trim() + splitChar[0] + txtJourAll[i].Trim();
            }
            else    //貸方
            {
                a[i] = "9" + txtJourSub[1].Trim() + splitChar[0] + txtJourAll[i].Trim();
            }

        }
        //oAjax.ClientMsgBoxAjax(msg, UpdatePanel2, "A");

        string sMsg = "";
        ArrayList al = new ArrayList();
        al.AddRange(a);
        //20131014 KAYA:因為USER反應序號編列與實際登打順序有差異；經與技術長討論後決議，將依使用者輸入順序為主，不另排序
        //al.Sort(0, al.Count, null);
        for (int i = 0; i < al.Count; i++)
        {
            //Response.Write(al[i].ToString() + "<br>");
            int idx = al[i].ToString().IndexOf(splitChar[0], al[i].ToString().IndexOf(splitChar[0]) + 1);
            int len = al[i].ToString().Length;
            al[i] = al[i].ToString().Substring(idx, len - idx) + splitChar[1];

            sMsg += Convert.ToString(i + 1) + al[i].ToString();
            //oAjax.ClientMsgBoxAjax(Convert.ToString(i + 1) + al[i].ToString() + ";", UpdatePanel2, "A" + i.ToString());
        }

        //oAjax.ClientMsgBoxAjax(sMsg, UpdatePanel2, "A1");
        txtAllJournailzing.Text = sMsg;
        #endregion
        ClearItem();
        //-----------
    }
    /// <summary>
    /// 取得傳票資料
    /// </summary>
    /// <param name="tCompany">公司別</param>
    /// <param name="tVoucherNo">行列代號</param>
    /// <returns></returns>
    /// <remarks></remarks>
    protected void GetJournailData(string tCompany, string tVoucherNo)
    {

       

       
        string sql = "SELECT";
        sql += " A.Company,C.CompanyShortName,A.VoucherNo,A.VoucherSeqNo,A.AcctNo,B.AcctDesc1,A.Index01,A.Idx01";
        sql += ",A.Index02,A.Idx02,A.Index03,A.Idx03,A.Index04,A.Idx04,A.Index05,A.Idx05,A.Index06,A.Idx06";
        sql += ",A.Index07,A.Idx07,A.JournalDate,A.DBAmt,A.CRAmt,A.JournalDesc,A.JournalSourceCode,A.PostFlag,A.ApvlFlag";
        sql += ",A.DletFlag,A.ReturnDate,A.CashCode,A.CashAmt,A.OffsetJrnNo,A.VoucherEntryDate,A.CreateUser";
        sql += ",A.JournalType,A.AllocationCode,A.DocFlag,A.LstChgUser,A.LstChgDateTime,B.AcctDesc1,B.AcctType";
        sql += ",B.AcctCtg,B.AcctClass,B.ASpecialAcct,B.Inputctl1,B.Inputctl2,B.Inputctl3,B.Inputctl4";
        sql += ",B.Inputctl5, B.Inputctl6, B.Inputctl7";
        sql += ",Idx01Name=dbo.fnGetAcnoIdx(A.Company,A.Idx01),Idx02Name=dbo.fnGetAcnoIdx(A.Company,A.Idx02)";
        sql += ",Idx03Name=dbo.fnGetAcnoIdx(A.Company,A.Idx03),Idx04Name=dbo.fnGetAcnoIdx(A.Company,A.Idx04)";
        sql += ",Idx05Name=dbo.fnGetAcnoIdx(A.Company,A.Idx05),Idx06Name=dbo.fnGetAcnoIdx(A.Company,A.Idx06)";
        sql += ",Idx07Name=dbo.fnGetAcnoIdx(A.Company,A.Idx07)";
        sql += ",Idx01YN=dbo.fnGetAcnoIdxYN(A.Company,A.Idx01),Idx02YN=dbo.fnGetAcnoIdxYN(A.Company,A.Idx02)";
        sql += ",Idx03YN=dbo.fnGetAcnoIdxYN(A.Company,A.Idx03),Idx04YN=dbo.fnGetAcnoIdxYN(A.Company,A.Idx04)";
        sql += ",Idx05YN=dbo.fnGetAcnoIdxYN(A.Company,A.Idx05),Idx06YN=dbo.fnGetAcnoIdxYN(A.Company,A.Idx06)";
        sql += ",Idx07YN=dbo.fnGetAcnoIdxYN(A.Company,A.Idx07)";
        sql += " FROM GLVoucherDetail AS A";
        sql += " INNER JOIN GLAcctDef AS B ON A.Company = B.Company AND A.AcctNo = B.AcctNo";
        sql += " INNER JOIN Company AS C ON A.Company = C.Company";
        sql += " WHERE (A.Company = @Company) AND (A.VoucherNo = @VoucherNo) AND (B.AcctClass = @AcctClass)";

        _MyDBM = new DBManger();
        _MyDBM.New();
        cnn = _MyDBM.GetConnectionString();
        
        DataTable dt = new DataTable();
        SqlDataAdapter adpt = new SqlDataAdapter(sql, cnn);
        try
        {
            adpt.SelectCommand.Parameters.Add("@Company", SqlDbType.Char, 2).Value = tCompany;
            adpt.SelectCommand.Parameters.Add("@VoucherNo", SqlDbType.Char, 10).Value = tVoucherNo;
            adpt.SelectCommand.Parameters.Add("@AcctClass", SqlDbType.Char, 10).Value = "1";

            adpt.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                //傳票表頭
                txtCompany.Text = dt.Rows[0]["Company"].ToString();
                lblCompany.Text = dt.Rows[0]["CompanyShortName"].ToString();
                txtJournailNo.Text = dt.Rows[0]["VoucherNo"].ToString();
                if (dt.Rows[0]["JournalDate"].ToString().Trim() != "" && dt.Rows[0]["JournalDate"].ToString().Trim() != "0")
                {
                    txtJournailDate.Text = _UserInfo.SysSet.ToTWDate(ConvertStringToDateFormat(dt.Rows[0]["JournalDate"].ToString().Trim()));
                }
                else
                {
                    txtJournailDate.Text = "";
                }               
                     //ConvertStringToDateFormat(dt.Rows[0]["JournalDate"].ToString());
                if (dt.Rows[0]["JournalType"].ToString().Trim() == "") {
                    ddlstJournailType.SelectedValue = "0";
                }
                else 
                {
                    ddlstJournailType.SelectedValue = dt.Rows[0]["JournalType"].ToString().Trim();
                }

                if (dt.Rows[0]["ReturnDate"].ToString().Trim() != "" && dt.Rows[0]["ReturnDate"].ToString().Trim()!="0")
                {
                    txtReturnDate.Text = _UserInfo.SysSet.ToTWDate(ConvertStringToDateFormat(dt.Rows[0]["ReturnDate"].ToString().Trim())); //ConvertStringToDateFormat(dt.Rows[0]["ReturnDate"].ToString());
                }
                else
                {
                    txtReturnDate.Text = "";
                }


              
                txtAllJournailzing.Text = "";
                for (int i = 0; i < dt.Rows.Count ; i++)
                {
                    //傳票列表
                    txtAllJournailzing.Text += dt.Rows[i]["VoucherSeqNo"].ToString().Trim() + splitChar[0];
                    txtAllJournailzing.Text += dt.Rows[i]["AcctNo"].ToString().Trim() + splitChar[0] + dt.Rows[i]["AcctNo"].ToString().Trim() + splitChar[0];
                    txtAllJournailzing.Text += dt.Rows[i]["AcctDesc1"].ToString().Trim() + splitChar[0];
                    txtAllJournailzing.Text += dt.Rows[i]["JournalDesc"].ToString().Trim() + splitChar[0];
                    txtAllJournailzing.Text += dt.Rows[i]["DBAmt"].ToString().Trim() + splitChar[0];
                    txtAllJournailzing.Text += dt.Rows[i]["CRAmt"].ToString().Trim() + splitChar[0];
                    for (int j = 1; j <= 7; j++)
                    {
                        #region 第1到第7行
                        txtAllJournailzing.Text += dt.Rows[i]["Idx0" + j.ToString()].ToString().Trim() + splitChar[3];
                        txtAllJournailzing.Text += dt.Rows[i]["Idx0" + j.ToString() + "Name"].ToString().Trim() + splitChar[3];
                        txtAllJournailzing.Text += dt.Rows[i]["Inputctl" + j.ToString()].ToString().Trim() + splitChar[3];
                        txtAllJournailzing.Text += dt.Rows[i]["Idx0" + j.ToString() + "YN"].ToString().Trim() + splitChar[3];
                        txtAllJournailzing.Text += dt.Rows[i]["Index0" + j.ToString()].ToString().Trim() + splitChar[3];
                        txtAllJournailzing.Text += GetJournailzingName(tCompany
                            , dt.Rows[i]["Idx0" + j.ToString()].ToString().Trim(), dt.Rows[i]["Index0" + j.ToString()].ToString().Trim()).Trim() + splitChar[2];
                        #endregion
                    }
                    //結束
                    txtAllJournailzing.Text += splitChar[1];
                }
            }
            
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
    /// <summary>
    /// 轉換日期格式
    /// </summary>
    /// <param name="tString">yyyyMMdd(傳入的日期字串)</param>
    /// <returns>yyyy/MM/dd(格式化的的日期字串)</returns>
    /// <remarks>yyyy/MM/dd</remarks>
    protected string ConvertStringToDateFormat(string tString)
    {
        Int32 iString = Convert.ToInt32(tString);
        if (iString == 0)
        {
            return "";
        }
        else
        {
            return string.Format("{0:0000/00/00}", iString);
        }
    }
    
    protected void imgbtnSaveJourmail_Click(object sender, ImageClickEventArgs e)
    {
        SaveJourmail();
    }

    protected void imgbtnPrintAndSaveJournail_Click(object sender, ImageClickEventArgs e)
    {
        SaveJourmail();
    }

    protected void imgbtnCopyToNewJourail_Click(object sender, ImageClickEventArgs e)
    {
        Session["OPMode"] = "New";
    }
    protected void imgbtnNewJourail_Click(object sender, ImageClickEventArgs e)
    {
        //傳票分錄隱藏
        //Session["DisplayContent"] = "N";
        //txtSingleJournailzing暫存字串清空
        Session["OPMode"] = "New";
        txtSingleJournailzing.Text = "";
    }
    //傳票存檔
    protected void SaveJourmail()
    {

        string strDBAMT = string.Empty;
        string strCRAMT = string.Empty;
        double dobDBATM;
        double dobCRAMT;

        //轉換民國為西元年
        string strJournailDate = "";
        string strReturnDate = "";

        //借貸方金額不相等

        if (txtDBAMT.Text.Trim() != "" && txtCRAMT.Text.Trim() != "")
        {
            strDBAMT = txtDBAMT.Text.ToString().Replace(",", "");
            strCRAMT = txtCRAMT.Text.ToString().Replace(",", "");

            if (Double.TryParse(strDBAMT,out dobDBATM) && Double.TryParse(strCRAMT, out dobCRAMT))
            {
                if (dobDBATM -dobCRAMT != 0)
                {
                   // PanUtility.JsUtility.ClientMsgBoxAjax("借貸方金額不相等!!!", UpdatePanel2, "");
                    JsUtility.ClientMsgBoxAjax("借貸方金額不相等!!!", UpdatePanel2, "");
                    return;
                }
                
            }
            else
            {
               // PanUtility.JsUtility.ClientMsgBoxAjax("借貸方金額必須是數字!!!", UpdatePanel2, "");
                JsUtility.ClientMsgBoxAjax("借貸方金額必須是數字!!!", UpdatePanel2, "");
                return;
            
            }          
        }
        else
        {
            //PanUtility.JsUtility.ClientMsgBoxAjax("借貸方金額不可為空白!!!", UpdatePanel2, "");
            JsUtility.ClientMsgBoxAjax("借貸方金額不可為空白!!!", UpdatePanel2, "");
            return;
        }



        //公司別不可以空白
        if (txtCompany.Text.Trim() == "")
        {
           // PanUtility.JsUtility.ClientMsgBoxAjax("公司別不可以空白!!!", UpdatePanel2, "");
            JsUtility.ClientMsgBoxAjax("公司別不可以空白!!!", UpdatePanel2, "");

            return;
        }
        //傳票日期不可以空白
        if (txtJournailDate.Text.Trim() == "")
        {
            //PanUtility.JsUtility.ClientMsgBoxAjax("傳票日期不可以空白!!!", UpdatePanel2, "");
            JsUtility.ClientMsgBoxAjax("傳票日期不可以空白!!!", UpdatePanel2, "");

            return;
        }

        //檢查傳票日期格式

        strJournailDate=_UserInfo.SysSet.ToADDate(txtJournailDate.Text.Trim());
        if (strJournailDate == "1912/01/01")
        {
            JsUtility.ClientMsgBoxAjax("傳票日期格式錯誤,請輸入正確日期!!!", UpdatePanel2, "");
            return;        
        }


        //檢查迴轉日期格式
        if (txtReturnDate.Text.Trim() != "")
        {
            strReturnDate = _UserInfo.SysSet.ToADDate(txtReturnDate.Text.Trim());
            if (strReturnDate == "1912/01/01")
            {
                JsUtility.ClientMsgBoxAjax("迴轉日期格式錯誤,請輸入正確日期!!!", UpdatePanel2, "");
                return;            
            }
        }



        //傳票未建立任何分錄
        if (txtAllJournailzing.Text.Trim() == "")
        {
           // PanUtility.JsUtility.ClientMsgBoxAjax("傳票未建立任何分錄!!!", UpdatePanel2, "");
            JsUtility.ClientMsgBoxAjax("傳票未建立任何分錄!!!", UpdatePanel2, "");
            return;
        }
        //傳票日期的結帳期數是否結帳
        if (GetPeriodClose(txtCompany.Text.Trim(), strJournailDate.Replace("/", "")) == "Y")
        {
            //PanUtility.JsUtility.ClientMsgBoxAjax("傳票日期已結帳不可以存檔!!!", UpdatePanel2, "");
            JsUtility.ClientMsgBoxAjax("傳票日期已結帳不可以存檔!!!", UpdatePanel2, "");

            return;
        }
        //傳票已經 Approval 不可以編輯
        if (GetApprovalCode(txtCompany.Text.Trim(), Session["OPMode"].ToString(), Session["VoucherNo"].ToString()) == "Y")
        {
           JsUtility.ClientMsgBoxAjax("傳票已 Approval 不可以編輯!!!", UpdatePanel2, "");         

            return;
        }

        if (Session["OPMode"].ToString().ToUpper() == "NEW")
        {
            txtJournailNo.Text = GetVoucherNo(txtCompany.Text.Trim(),
                                ddlstJournailType.SelectedValue.ToString().Trim(),
                                strJournailDate.Trim().Replace("/", ""));
            Session["VoucherNo"] = txtJournailNo.Text.Trim();
        }
        else
        {
            txtJournailNo.Text = Session["VoucherNo"].ToString();
        
        }
        string sString = "";
        sString += "document.getElementById(\"txtJournailNo\").innerText = " + txtJournailNo.Text + ";";
        //oAjax.DoJavascriptAjax(sString, UpdatePanel2, "");
        //oAjax.ClientMsgBoxAjax(txtJournailNo.Text, UpdatePanel2, "");

        bool tReturn = false;

        


        tReturn = UpdateVoucherData(txtCompany.Text.Trim(),
                            Session["OPMode"].ToString().Trim(),
                            this.txtJournailNo.Text.Trim());
        

        if (tReturn)
        {
            JsUtility.ClientMsgBoxAjax("傳票存檔成功!!!", UpdatePanel2, "Message");
            Session["OPMode"] = "Query";
        }
        else
        {
            JsUtility.ClientMsgBoxAjax("傳票存檔失敗!!!", UpdatePanel2, "Message");
        }

    }
    /// <summary>
    /// 取得傳票日期的結帳期數的 PeriodCloseXX 的值
    /// </summary>
    /// <param name="tCompany">公司別</param>
    /// <param name="tBaseDate">傳票日期 (yyyy/MM/dd)</param>
    /// <returns>是否已結帳(Y/N)</returns>
    /// <remarks></remarks>
    protected string GetPeriodClose(string tCompany, string tBaseDate)
    {
        string tPeriodCloase = "N";
        string sql = "select PeriodString=dbo.fnAccPeriodDates(@Company,@BaseDate)";
        DataTable dt = new DataTable();
        SqlDataAdapter adpt = new SqlDataAdapter(sql, cnn);
        try
        {
            adpt.SelectCommand.Parameters.Add("@Company", SqlDbType.Char, 2).Value = tCompany;
            adpt.SelectCommand.Parameters.Add("@BaseDate", SqlDbType.Char, 8).Value = tBaseDate;

            adpt.Fill(dt);
            if (dt.Rows[0]["PeriodString"] != null)
            {
                string[] tPeriodList = dt.Rows[0]["PeriodString"].ToString().Split('-');
                if (tPeriodList.Length > 0)
                {
                    tPeriodCloase = tPeriodList[4].ToString().Trim();
                    if (tPeriodCloase != "Y")
                        tPeriodCloase = "N";
                }
            }
        }
        catch (InvalidCastException e)
        {
            throw (e);
        }
        finally
        {

        }
        return tPeriodCloase;
    }
    /// <summary>
    /// 取得傳票的 ApprovalCode 值
    /// </summary>
    /// <param name="tCompany">公司別</param>
    /// <param name="tOPMode">作業模式</param>
    /// <param name="tVoucherNo">傳票編號</param>
    /// <returns>ApprovalCode</returns>
    /// <remarks>已經Approval的傳票不可以編輯/刪除/作廢</remarks>
    protected string GetApprovalCode(string tCompany, string tOPMode, string tVoucherNo)
    {
        string tApprovalCode = "N";
        if (tOPMode.ToUpper() == "EDIT" || tOPMode.ToUpper() == "NULLIFY")
        {
            string sql = "select ApprovalCode from GLVoucherHead";
            sql += " where Company=@Company";
            sql += "   and VoucherNo=@VoucherNo";
            DataTable dt = new DataTable();
            SqlDataAdapter adpt = new SqlDataAdapter(sql, cnn);
            try
            {
                adpt.SelectCommand.Parameters.Add("@Company", SqlDbType.Char, 2).Value = tCompany;
                adpt.SelectCommand.Parameters.Add("@VoucherNo", SqlDbType.Char, 10).Value = tVoucherNo;

                adpt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    tApprovalCode = dt.Rows[0]["ApprovalCode"].ToString().Trim();
                    if (tApprovalCode != "Y")
                        tApprovalCode = "N";
                }
            }
            catch (InvalidCastException e)
            {
                throw (e);
            }
            finally
            {

            }
        }
        return tApprovalCode;
    }
    /// <summary>
    /// 儲存傳票的資料
    /// </summary>
    /// <param name="tCompany">公司別</param>
    /// <param name="tOPMode">作業模式</param>
    /// <param name="tVoucherNo">傳票編號</param>
    /// <returns>ApprovalCode</returns>
    /// <remarks>已經Approval的傳票不可以編輯/刪除/作廢</remarks>
    protected bool UpdateVoucherData(string tCompany, string tOPMode, string tVoucherNo)
    {
        bool tReturn = false;

        //新增傳票取得"傳票編號"
        if (tOPMode.ToUpper() == "NEW" || tOPMode.ToUpper() == "EDIT")
        {
            tVoucherNo = txtJournailNo.Text.Trim();
            //oAjax.ClientMsgBoxAjax(tVoucherNo, UpdatePanel2, "Message1");
        }
        //SqlConnection sqlCnn = new SqlConnection(cnn);
        //sqlCnn.Open();
        //SqlCommand sqlCmd = new SqlCommand();
        //SqlTransaction sqlTrans = sqlCnn.BeginTransaction();
        //sqlCmd.Connection = sqlCnn;

        //編輯模式先刪除該傳票編號的資料，再新增傳票
        //if (tOPMode.ToUpper() == "EDIT")
        //{
        tReturn = DeleteVoucherData("GLVoucherHead", tCompany, tVoucherNo);
        //oAjax.ClientMsgBoxAjax(tReturn.ToString(), UpdatePanel2, "Message2");
        tReturn = DeleteVoucherData("GLVoucherDetail", tCompany, tVoucherNo);
        //oAjax.ClientMsgBoxAjax(tReturn.ToString(), UpdatePanel2, "Message3");
        //}

      

        //新增傳票主檔
        tReturn = InsertGLVoucherHead(tCompany, tVoucherNo);
        //oAjax.ClientMsgBoxAjax(tReturn.ToString(), UpdatePanel2, "Message4");

        //新增傳票明細檔
        string[] Items = txtAllJournailzing.Text.Trim().Split(splitChar[1]);
        if (Items.Length > 0)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i].ToString() != "")
                    tReturn = InsertGLVoucherDetail(tCompany, tVoucherNo, Convert.ToDecimal(i), Items[i].ToString());
                //oAjax.ClientMsgBoxAjax(tReturn.ToString(), UpdatePanel2, "Message5");
            }
        }

        //更新傳票編號設定檔
        if (tOPMode.ToUpper() == "NEW")
        {
            tReturn = UpdateGLVoucherNo(tCompany, tVoucherNo, mJournalNoType);
            //oAjax.ClientMsgBoxAjax(tReturn.ToString(), UpdatePanel2, "Message6");
        }

        return tReturn;
    }
    /// <summary>
    /// 取得傳票的號碼
    /// </summary>
    /// <param name="tCompany">公司別</param>
    /// <param name="tVoucherType">傳票類別</param>
    /// <param name="tVoucherDate">傳票日期(西元年 yyyyMMdd)</param>
    /// <returns>傳票號碼</returns>
    /// <remarks></remarks>
    protected string GetVoucherNo(string tCompany, string tVoucherType, string tVoucherDate)
    {
        //宣告變數
        string tVoucherNo = "";         //傳票號碼
        string tCalendarType = "";      //年制設定
        string tJournalNoType = "";     //傳票編碼方式
        int tJrnSeqNo = 0;               //目前傳票數
        //取得傳票的編號原則
        string sql = "select Company,CalendarType,JournalNoType from GLParm";
        sql += " where Company=@Company";
        DataTable dt = new DataTable();
        SqlDataAdapter adpt = new SqlDataAdapter(sql, cnn);
        try
        {
            adpt.SelectCommand.Parameters.Add("@Company", SqlDbType.Char, 2).Value = tCompany;

            adpt.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                tCalendarType = dt.Rows[0]["CalendarType"].ToString().Trim();
                tJournalNoType = dt.Rows[0]["JournalNoType"].ToString().Trim();
            }
        }
        catch (InvalidCastException e)
        {
            throw (e);
            //錯誤回傳空字串
            return tVoucherNo;
        }
        //年制設定 or 傳票編碼方式 = 空字串則回傳空字串
        if (tCalendarType == "" || tJournalNoType == "")
            return tVoucherNo;
        //傳票編碼方式 = "3" and 傳票類別 = 空字串則回傳空字串
        if (tVoucherType == "" && tJournalNoType == "3")
            return tVoucherNo;

        //取得當日的傳票號碼的最後一號，如果當日無傳票資料新增一筆號碼資料
        sql = "select JrnSeqno from GLVoucherNo";
        sql += " where  Company=@Company";
        sql += " and JrnDate=@JrnDate";
        sql += " and JrnType=@JrnType";
        
       
        DataTable dt1 = new DataTable();
        SqlDataAdapter adpt1 = new SqlDataAdapter(sql, cnn);
        
        try
        {
            adpt1.SelectCommand.Parameters.Add("@Company", SqlDbType.Char, 2).Value = tCompany;
            adpt1.SelectCommand.Parameters.Add("@JrnDate", SqlDbType.Decimal, 8).Value = Convert.ToDecimal(tVoucherDate);
            if (tJournalNoType == "3")
                adpt1.SelectCommand.Parameters.Add("@JrnType", SqlDbType.Char, 1).Value = tVoucherType;
            else
                adpt1.SelectCommand.Parameters.Add("@JrnType", SqlDbType.Char, 1).Value = "";
            adpt1.Fill(dt1);
            if (dt1.Rows.Count > 0)
            {
                tJrnSeqNo = Convert.ToInt16(dt1.Rows[0]["JrnSeqno"].ToString());
            }
            else
            {
                sql = "insert into GLVoucherNo";
                sql += " (Company,JrnDate,JrnSeqno,Jrntype,LstChgDate,LstChgUser";
                sql += ") values (";
                sql += "@Company,@JrnDate,@JrnSeqno,@Jrntype,@LstChgDate,@LstChgUser)";
                try
                {
                    DataTable dt2 = new DataTable();
                    SqlDataAdapter adpt2 = new SqlDataAdapter(sql, cnn);
                    adpt2.SelectCommand.Parameters.Add("@Company", SqlDbType.Char, 2).Value = tCompany;
                    adpt2.SelectCommand.Parameters.Add("@JrnDate", SqlDbType.Decimal, 8).Value = Convert.ToDecimal(tVoucherDate);
                    adpt2.SelectCommand.Parameters.Add("@JrnSeqno", SqlDbType.Decimal, 4).Value = tJrnSeqNo;
                    if (tJournalNoType == "3")
                        adpt2.SelectCommand.Parameters.Add("@Jrntype", SqlDbType.Char, 1).Value = tVoucherType;
                    else
                        adpt2.SelectCommand.Parameters.Add("@Jrntype", SqlDbType.Char, 1).Value = "";
                    adpt2.SelectCommand.Parameters.Add("@LstChgDate", SqlDbType.SmallDateTime).Value = DateTime.Now.ToShortDateString();
                    adpt2.SelectCommand.Parameters.Add("@LstChgUser", SqlDbType.VarChar, 20).Value = _UserInfo.UData.UserId;    // Session["fLoginUser"].ToString();

                    adpt2.SelectCommand.Connection.Open();
                    adpt2.SelectCommand.ExecuteNonQuery();
                    adpt2.SelectCommand.Connection.Close();
                }
                catch (InvalidCastException e)
                {
                    throw (e);
                    //錯誤回傳空字串
                    return tVoucherNo;
                }
            }

        }
        catch (InvalidCastException e)
        {
            throw (e);
            //錯誤回傳空字串
            return tVoucherNo;
        }

        //組合傳票號碼
        string tYear = "";
        string tMonthDate = "";

        mCalendarType = tCalendarType;
        if (tCalendarType == "1")       //年制 = 西元年
            tYear = tVoucherDate.Substring(2, 2);
        else if (tCalendarType == "2")  //年制 = 中國年
            tYear = Convert.ToString(Convert.ToInt16(tVoucherDate.Substring(0, 4)) - 1911).Substring(1,2);
        tMonthDate = tVoucherDate.Substring(4, 4);

        mJournalNoType = tJournalNoType;
        tJrnSeqNo += 1;
        if (tJournalNoType == "1")      //年月日+3碼
            tVoucherNo = tYear + tMonthDate + tJrnSeqNo.ToString("000");
        else if (tJournalNoType == "2") //年月日+4碼
            tVoucherNo = tYear + tMonthDate + tJrnSeqNo.ToString("0000");
        else if (tJournalNoType == "3") //年月日+傳票類別+3碼
            tVoucherNo = tYear + tMonthDate + tVoucherType + tJrnSeqNo.ToString("000");

        //回傳傳票號碼
        return tVoucherNo;
    }
    //刪除傳票主檔，傳票明細檔
    protected bool DeleteVoucherData(string tTable, string tCompany, string tVoucherNo)
    {
        bool tReturn = false;
        string sql = "";
        sql += "delete " + tTable;
        sql += " where Company=@Company";
        sql += "   and VoucherNo=@VoucherNo";
        
        SqlConnection sqlCnn = new SqlConnection(cnn);
        SqlCommand sqlCmd = new SqlCommand();
        sqlCnn.Open();
        
        try
        {
            sqlCmd.Connection = sqlCnn;
            sqlCmd.CommandText = sql;
            sqlCmd.Parameters.Add("@Company", SqlDbType.Char, 2).Value = tCompany;
            sqlCmd.Parameters.Add("@VoucherNo", SqlDbType.Char, 10).Value = tVoucherNo;
            sqlCmd.ExecuteNonQuery();

            tReturn = true;
        }
        catch (InvalidCastException e)
        {
            throw (e);
            return tReturn;
        }
        finally
        {
            sqlCmd.Parameters.Clear();
            sqlCnn.Close();
        }

        return tReturn;
    }
    //新增傳票主檔
    protected bool InsertGLVoucherHead(string tCompany, string tVoucherNo)
    {
        bool tReturn = false;
        //轉換民國年為西元年
        string strjournaildate ="";

        strjournaildate = _UserInfo.SysSet.ToADDate(txtJournailDate.Text.Trim());
        strjournaildate = strjournaildate.Replace("/", "");


        string[] Items = txtAllJournailzing.Text.Trim().Split(splitChar[1]);
        string sql = "";
        sql += "insert into GLVoucherHead (";
        sql += " Company,VoucherNo,VoucherDate,VoucherEntryDate,VoucherOwner,VoucherType,VoucherSource";
        sql += ",VoucherAloc,JournalCnt,ApprovalCode,PostCode,RevDate,DletFlag,DocFlag,LstChgUser,LstChgDateTime";
        sql += ") values (";
        sql += " @Company,@VoucherNo,@VoucherDate,@VoucherEntryDate,@VoucherOwner,@VoucherType,@VoucherSource";
        sql += ",@VoucherAloc,@JournalCnt,@ApprovalCode,@PostCode,@RevDate,@DletFlag,@DocFlag,@LstChgUser,@LstChgDate";
        sql += ")";

        SqlConnection sqlCnn = new SqlConnection(cnn);
        sqlCnn.Open();
        SqlCommand sqlCmd = new SqlCommand();
        sqlCmd.Connection = sqlCnn;
        sqlCmd.CommandText = sql;
        sqlCmd.Parameters.Add("@Company", SqlDbType.Char, 2).Value = tCompany;
        sqlCmd.Parameters.Add("@VoucherNo", SqlDbType.Char, 10).Value = tVoucherNo;
        sqlCmd.Parameters.Add("@VoucherDate", SqlDbType.Char, 8).Value = strjournaildate;//txtJournailDate.Text.Trim().Replace("/","");
        sqlCmd.Parameters.Add("@VoucherEntryDate", SqlDbType.Char, 8).Value = strjournaildate;//txtJournailDate.Text.Trim().Replace("/", "");
        sqlCmd.Parameters.Add("@VoucherOwner", SqlDbType.VarChar, 20).Value = _UserInfo.UData.UserId;   // Session["fLoginUser"].ToString();
        sqlCmd.Parameters.Add("@VoucherType", SqlDbType.Char, 1).Value = ddlstJournailType.SelectedValue.ToString().Trim();
        sqlCmd.Parameters.Add("@VoucherSource", SqlDbType.Char, 2).Value = "GL";
        sqlCmd.Parameters.Add("@VoucherAloc", SqlDbType.Char, 1).Value = txtP3.Text.Trim();
        sqlCmd.Parameters.Add("@JournalCnt", SqlDbType.Decimal, 3).Value = 2;
        sqlCmd.Parameters.Add("@ApprovalCode", SqlDbType.Char, 1).Value = "N";//預設應該為N或空白
        sqlCmd.Parameters.Add("@PostCode", SqlDbType.Char, 1).Value = "N";
        sqlCmd.Parameters.Add("@RevDate", SqlDbType.Char, 8).Value = "";
        sqlCmd.Parameters.Add("@DletFlag", SqlDbType.Char, 1).Value = "";
        sqlCmd.Parameters.Add("@DocFlag", SqlDbType.Char, 1).Value = "";
        sqlCmd.Parameters.Add("@LstChgDate", SqlDbType.SmallDateTime).Value = DateTime.Now.ToShortDateString();
        sqlCmd.Parameters.Add("@LstChgUser", SqlDbType.VarChar, 20).Value = _UserInfo.UData.UserId; // Session["fLoginUser"].ToString();
        try
        {
            sqlCmd.ExecuteNonQuery();
            tReturn = true;
        }
        catch (InvalidCastException e)
        {
            throw (e);
            return tReturn;
        }
        finally
        {
            sqlCmd.Parameters.Clear();
            sqlCnn.Close();
        }
        return tReturn;
    }
    //新增傳票明細檔
    protected bool InsertGLVoucherDetail(string tCompany, string tVoucherNo, decimal tVoucherSeqNo, string tJournalString)
    {
        string strjournaildate = "";
        strjournaildate = _UserInfo.SysSet.ToADDate(txtJournailDate.Text.Trim());
        strjournaildate = strjournaildate.Replace("/", "");
        if (strjournaildate == "19120101")
        {
            strjournaildate = "";
        }


        //迴轉期暫不顯示
        string strReturndate = "";

        strReturndate = _UserInfo.SysSet.ToADDate(txtReturnDate.Text.Trim());
        strReturndate = strReturndate.Replace("/","");

        if (strReturndate == "19120101")
        {
            strReturndate = "";

        }



        bool tReturn = false;
        string[] sItems = tJournalString.Trim().Split(splitChar[0]);
        string[] Jrn = sItems[7].Trim().Split(splitChar[2]);
        string sql = "";
        sql += "insert into GLVoucherDetail (";
        sql += " Company,VoucherNo,VoucherSeqNo,AcctNo,Index01,Idx01,Index02,Idx02,Index03,Idx03";
        sql += ",Index04,Idx04,Index05,Idx05,Index06,Idx06,Index07,Idx07,JournalDate";
        sql += ",DBAmt,CRAmt,JournalDesc,JournalSourceCode,PostFlag,ApvlFlag,DletFlag";
        sql += ",ReturnDate,CashCode,CashAmt,OffsetJrnNo,VoucherEntryDate,JournalType";
        sql += ",AllocationCode,DocFlag,CreateUser,LstChgUser,LstChgDateTime";
        sql += ") values (";
        sql += " @Company,@VoucherNo,@VoucherSeqNo,@AcctNo,@Index01,@Idx01,@Index02,@Idx02,@Index03,@Idx03";
        sql += ",@Index04,@Idx04,@Index05,@Idx05,@Index06,@Idx06,@Index07,@Idx07,@VoucherDate";
        sql += ",@DBAmt,@CRAmt,@JournalDesc,@VoucherSource,@PostCode,@ApprovalCode,@DletFlag";
        sql += ",@RevDate,@CashCode,@CashAmt,@OffsetJrnNo,@VoucherEntryDate,@VoucherType";
        sql += ",@VoucherAloc,@DocFlag,@CreateUser,@LstChgUser,@LstChgDate";
        sql += ")";
        
        SqlConnection sqlCnn = new SqlConnection(cnn);
        sqlCnn.Open();
        SqlCommand sqlCmd = new SqlCommand();
        sqlCmd.Connection = sqlCnn;
        sqlCmd.CommandText = sql;
        sqlCmd.Parameters.Add("@Company", SqlDbType.Char, 2).Value = tCompany;
        sqlCmd.Parameters.Add("@VoucherNo", SqlDbType.Char, 10).Value = tVoucherNo;
        //----------------------
        sqlCmd.Parameters.Add("@VoucherSeqNo", SqlDbType.Decimal).Value = tVoucherSeqNo;

        //----------------------
        sqlCmd.Parameters.Add("@AcctNo", SqlDbType.Char, 8).Value = sItems[1].Trim();
        //分錄明細
        string[] Jrn0 = Jrn[0].Trim().Split(splitChar[3]);
        sqlCmd.Parameters.Add("@Index01", SqlDbType.Char, 20).Value = Jrn0[4].Trim();
        sqlCmd.Parameters.Add("@Idx01", SqlDbType.Char, 2).Value = Jrn0[0].Trim();
        string[] Jrn1 = Jrn[1].Trim().Split(splitChar[3]);
        sqlCmd.Parameters.Add("@Index02", SqlDbType.Char, 20).Value = Jrn1[4].Trim();
        sqlCmd.Parameters.Add("@Idx02", SqlDbType.Char, 2).Value = Jrn1[0].Trim();
        string[] Jrn2 = Jrn[2].Trim().Split(splitChar[3]);
        sqlCmd.Parameters.Add("@Index03", SqlDbType.Char, 20).Value = Jrn2[4].Trim();
        sqlCmd.Parameters.Add("@Idx03", SqlDbType.Char, 2).Value = Jrn2[0].Trim();
        string[] Jrn3 = Jrn[3].Trim().Split(splitChar[3]);
        sqlCmd.Parameters.Add("@Index04", SqlDbType.Char, 20).Value = Jrn3[4].Trim();
        sqlCmd.Parameters.Add("@Idx04", SqlDbType.Char, 2).Value = Jrn3[0].Trim();
        string[] Jrn4 = Jrn[4].Trim().Split(splitChar[3]);
        sqlCmd.Parameters.Add("@Index05", SqlDbType.Char, 20).Value = Jrn4[4].Trim();
        sqlCmd.Parameters.Add("@Idx05", SqlDbType.Char, 2).Value = Jrn4[0].Trim();
        string[] Jrn5 = Jrn[5].Trim().Split(splitChar[3]);
        if (Jrn5[4].Trim() == "")
            Jrn5[4] = "0";
        sqlCmd.Parameters.Add("@Index06", SqlDbType.Decimal).Value = Convert.ToDecimal(Jrn5[4].Trim());
        sqlCmd.Parameters.Add("@Idx06", SqlDbType.Char, 2).Value = Jrn5[0].Trim();
        string[] Jrn6 = Jrn[6].Trim().Split(splitChar[3]);
        sqlCmd.Parameters.Add("@Index07", SqlDbType.Char, 20).Value = Jrn6[4].Trim();
        sqlCmd.Parameters.Add("@Idx07", SqlDbType.Char, 2).Value = Jrn6[0].Trim();
        //--------
        sqlCmd.Parameters.Add("@VoucherDate", SqlDbType.Char, 8).Value = strjournaildate; //txtJournailDate.Text.Trim().Replace("/", "");
        if (sItems[5].Trim() == "")
            sItems[5] = "0";
        sqlCmd.Parameters.Add("@DBAmt", SqlDbType.Decimal).Value = Convert.ToDecimal(sItems[5].Trim());
        if (sItems[6].Trim() == "")
            sItems[6] = "0";
        sqlCmd.Parameters.Add("@CRAmt", SqlDbType.Decimal).Value = Convert.ToDecimal(sItems[6].Trim());
        sqlCmd.Parameters.Add("@JournalDesc", SqlDbType.NVarChar, 50).Value = sItems[4].Trim();
        sqlCmd.Parameters.Add("@VoucherSource", SqlDbType.Char, 2).Value = "GL";
        sqlCmd.Parameters.Add("@PostCode", SqlDbType.Char, 1).Value = "N";
        sqlCmd.Parameters.Add("@ApprovalCode", SqlDbType.Char, 1).Value = "N";//預設應該為N或空白
        sqlCmd.Parameters.Add("@DletFlag", SqlDbType.Char, 1).Value = "";
        sqlCmd.Parameters.Add("@RevDate", SqlDbType.Char, 8).Value = strReturndate;
        sqlCmd.Parameters.Add("@CashCode", SqlDbType.Char, 8).Value = "";
        sqlCmd.Parameters.Add("@CashAmt", SqlDbType.Decimal).Value = Convert.ToDecimal("0");
        sqlCmd.Parameters.Add("@OffsetJrnNo", SqlDbType.Char, 10).Value = "";
        sqlCmd.Parameters.Add("@VoucherEntryDate", SqlDbType.Char, 10).Value = DateTime.Now.ToString("yyyyMMdd");
        sqlCmd.Parameters.Add("@VoucherType", SqlDbType.Char, 1).Value = ddlstJournailType.SelectedValue.ToString().Trim();
        sqlCmd.Parameters.Add("@VoucherAloc", SqlDbType.Char, 1).Value = txtP3.Text.Trim();
        sqlCmd.Parameters.Add("@DocFlag", SqlDbType.Char, 1).Value = "";
        sqlCmd.Parameters.Add("@CreateUser", SqlDbType.VarChar, 20).Value = _UserInfo.UData.UserId; // Session["fLoginUser"].ToString();
        sqlCmd.Parameters.Add("@LstChgDate", SqlDbType.SmallDateTime).Value = DateTime.Now.ToShortDateString();
        sqlCmd.Parameters.Add("@LstChgUser", SqlDbType.VarChar, 20).Value = _UserInfo.UData.UserId; // Session["fLoginUser"].ToString();
        try
        {
            sqlCmd.ExecuteNonQuery();
            tReturn = true;
        }
        catch (InvalidCastException e)
        {
            throw (e);
            return tReturn;
        }
        finally
        {
            sqlCmd.Parameters.Clear();
            sqlCnn.Close();
        }
        return tReturn;
    }
    //更新傳票編號設定檔
    protected bool UpdateGLVoucherNo(string tCompany, string tVoucherNo, string tJournalNoType)
    {
        bool tReturn = false;
        string sql = "";
        sql += "update GLVoucherNo set";
        sql += " JrnSeqno=@JrnSeqno";
        sql += ",LstChgDate=@LstChgDate";
        sql += ",LstChgUser=@LstChgUser";
        sql += " where Company=@Company";
        sql += "   and JrnDate=@VoucherDate";
        if (tJournalNoType == "3")
            sql += "   and Jrntype=@VoucherType";

        SqlConnection sqlCnn = new SqlConnection(cnn);
        SqlCommand sqlCmd = new SqlCommand();
        sqlCnn.Open();

        try
        {
            sqlCmd.Connection = sqlCnn;
            sqlCmd.CommandText = sql;

            sqlCmd.Parameters.Add("@Company", SqlDbType.Char, 2).Value = tCompany;
            if (tJournalNoType == "3")
            {
                sqlCmd.Parameters.Add("@JrnSeqno", SqlDbType.Decimal).Value = Convert.ToDecimal(tVoucherNo.Substring(7, 3));
                sqlCmd.Parameters.Add("@VoucherType", SqlDbType.Char, 1).Value = ddlstJournailType.SelectedValue.ToString().Trim();
            }
            else if (tJournalNoType == "2")
            {
                sqlCmd.Parameters.Add("@JrnSeqno", SqlDbType.Decimal).Value = Convert.ToDecimal(tVoucherNo.Substring(6, 4));
                sqlCmd.Parameters.Add("@VoucherType", SqlDbType.Char, 1).Value = "";
            }
            else if (tJournalNoType == "1")
            {
                sqlCmd.Parameters.Add("@JrnSeqno", SqlDbType.Decimal).Value = Convert.ToDecimal(tVoucherNo.Substring(6, 3));
                sqlCmd.Parameters.Add("@VoucherType", SqlDbType.Char, 1).Value = "";
            }
            sqlCmd.Parameters.Add("@LstChgDate", SqlDbType.SmallDateTime).Value = DateTime.Now.ToString();
            sqlCmd.Parameters.Add("@LstChgUser", SqlDbType.VarChar, 20).Value = _UserInfo.UData.UserId;  // Session["fLoginUser"].ToString();

            //2013/08/26 因年份只取後兩位，民國100年後加上1911會發生錯誤，故改為以下判斷 
            if (Convert.ToDecimal(txtJournailDate.Text.Substring(0, 3)) > 99)
            {
                sqlCmd.Parameters.Add("@VoucherDate", SqlDbType.Decimal).Value =
                    Convert.ToDecimal(((Convert.ToDecimal(txtJournailDate.Text.Substring(0, 3)) + 1911).ToString() + tVoucherNo.Substring(2, 4)));
            }
            else
            {
                sqlCmd.Parameters.Add("@VoucherDate", SqlDbType.Decimal).Value =
                       Convert.ToDecimal(((Convert.ToDecimal(tVoucherNo.Substring(0, 2)) + 1911).ToString() + tVoucherNo.Substring(2, 4)));
            }

            //oAjax.ClientMsgBoxAjax(tCompany, UpdatePanel2, "Message10");
            //oAjax.ClientMsgBoxAjax((Convert.ToDecimal(((Convert.ToDecimal(tVoucherNo.Substring(0, 2)) + 1911).ToString() + tVoucherNo.Substring(2, 4)))).ToString(), UpdatePanel2, "Message11");
            //oAjax.ClientMsgBoxAjax((Convert.ToDecimal(tVoucherNo.Substring(6, 3))).ToString(), UpdatePanel2, "Message12");

            sqlCmd.ExecuteNonQuery();
            tReturn = true;
        }
        catch (InvalidCastException e)
        {
            throw (e);
            return tReturn;
        }
        finally
        {
            sqlCmd.Parameters.Clear();
            sqlCnn.Close();
        }
        return tReturn;
    }

}
