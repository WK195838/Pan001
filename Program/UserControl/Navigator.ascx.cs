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
using System.Collections.Generic;


public partial class SystemControl_Navigator : System.Web.UI.UserControl
{
    protected int _PageSize = 10;
    protected int _PageCount = 0;
    protected int _CurrentPage = 0;
    protected int _RecordCount = 0;
    private string _SQL;

    protected DataTable _dt;

    public DataTable DataSource
    {
        get { return _dt; }
        set { _dt = value; }
    }
    public event EventHandler BindEvent;
    public int PageSize
    {
        get { return _PageSize; }
        set
        {
            if (_PageSize == value)
                return;
            _PageSize = value;
            BindPageDdl();
        }
    }

    public string SQL
    {
        get { return _SQL; }
        set
        {
            _SQL = value;
            _SQLCount = _SQL.Substring(_SQL.ToLower().IndexOf(" from ", 0));
            int index = _SQLCount.ToLower().IndexOf(" order by", 0);
            if (index > 0)
            {
                _SQLCount = _SQLCount.Substring(0, index);
            }
            _SQLCount = "select count(*) " + _SQLCount;

            ViewState[this.ClientID + "_" + "SQL"] = _SQL;
            ViewState[this.ClientID + "_" + "SQLCount"] = _SQLCount;
            ViewState[this.ClientID + "_" + "_PageCount"] = _PageCount = 0;
            ViewState[this.ClientID + "_" + "_CurrentPage"] = _CurrentPage = 0;
            ViewState[this.ClientID + "_" + "_RecordCount"] = _RecordCount = 0;

        }
    }
    private string _SQLCount;

    //public string SQLCount
    //{
    //    get { return _SQLCount; }
    //    set { _SQLCount = value; }
    //}

    private MyButton _NextButton;

    public MyButton NextButton
    {
        get { return _NextButton; }
        set { _NextButton = value; }
    }

    public struct MyButton
    {
        public string Title;
        public string Image;

    }
    private GridView _BindGridView;

    public GridView BindGridView
    {
        get { return _BindGridView; }
        set
        {
            _BindGridView = value;
            if (_BindGridView != null)
            {
                _BindGridView.RowDataBound += new GridViewRowEventHandler(_BindGridView_RowDataBound);
                _BindGridView.DataBound += new EventHandler(_BindGridView_DataBound);
                _BindGridView.AllowPaging = false;
                _PageSize = _BindGridView.PageSize;
            }
        }
    }
    //protected override void OnInit(EventArgs e)
    //{
    //    base.OnInit(e);
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindPageDdl();
        }
        else
        {
            _SQL = (string)ViewState[this.ClientID + "_" + "SQL"];
            _SQLCount = (string)ViewState[this.ClientID + "_" + "SQLCount"];
            _PageSize = (int)ViewState[this.ClientID + "_" + "_PageSize"]; ;
            _PageCount = (int)ViewState[this.ClientID + "_" + "_PageCount"]; ;
            _CurrentPage = (int)ViewState[this.ClientID + "_" + "_CurrentPage"]; ;
            _RecordCount = (int)ViewState[this.ClientID + "_" + "_RecordCount"]; ;

        }
    }

    private void BindPageDdl()
    {

        ArrayList aNum = new ArrayList();
        aNum.Add(10);
        aNum.Add(20);
        aNum.Add(30);
        aNum.Add(50);
        aNum.Add(100);
        aNum.Add(200);
        aNum.Add(300);
        aNum.Add(500);
        aNum.Add(1000);
        if (!aNum.Contains(_PageSize))
        {
            aNum.Insert(0, _PageSize);
        }
        ddl_PageSize.DataSource = aNum;
        ddl_PageSize.DataBind();
        ddl_PageSize.SelectedIndex = ddl_PageSize.Items.IndexOf(new ListItem(_PageSize.ToString(), _PageSize.ToString()));

    }

    protected override void OnPreRender(EventArgs e)
    {
        txt_GotoPage.Text = (_CurrentPage + 1).ToString();
        base.OnPreRender(e);
        lbl_Desc.Text = String.Format("筆，共{0}筆，第", _RecordCount);
        lbl_Desc2.Text = String.Format("頁，共{0}頁", _PageCount);

        ViewState[this.ClientID + "_" + "SQL"] = _SQL;
        ViewState[this.ClientID + "_" + "SQLCount"] = _SQLCount;
        ViewState[this.ClientID + "_" + "_PageSize"] = _PageSize;
        ViewState[this.ClientID + "_" + "_PageCount"] = _PageCount;
        ViewState[this.ClientID + "_" + "_CurrentPage"] = _CurrentPage;
        ViewState[this.ClientID + "_" + "_RecordCount"] = _RecordCount;

    }

    void _BindGridView_DataBound(object sender, EventArgs e)
    {
        //throw new Exception("The method or operation is not implemented.");
    }
    //private DBSBase m_DB;

    //public DBSBase DB
    //{
    //    get { return m_DB; }
    //    set { m_DB = value; }
    //}


    void _BindGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //throw new Exception("The method or operation is not implemented.");
    }

    protected override void OnDataBinding(EventArgs e)
    {
        base.OnDataBinding(e);
    }

    public override void DataBind()
    {
        base.DataBind();
        if (IsPostBack)
        {
            _SQL = (string)ViewState[this.ClientID + "_" + "SQL"];
            _SQLCount = (string)ViewState[this.ClientID + "_" + "SQLCount"];
            _PageSize = (int)ViewState[this.ClientID + "_" + "_PageSize"]; ;
            _PageCount = (int)ViewState[this.ClientID + "_" + "_PageCount"]; ;
            _CurrentPage = (int)ViewState[this.ClientID + "_" + "_CurrentPage"]; ;
            _RecordCount = (int)ViewState[this.ClientID + "_" + "_RecordCount"]; ;
        }

        //_MyCommon.DR = _MyCommon.DB.Query(_SQLCount);
        //_MyCommon.DR.Read();

        //_RecordCount = (int)_MyCommon.DR[0];

        if ((_RecordCount % _PageSize) == 0)
            _PageCount = (_RecordCount / _PageSize);
        else
            _PageCount = (_RecordCount / _PageSize) + 1;

        _CurrentPage = 0;

        MyBind();



    }

    private void MyBind()
    {
        if (_CurrentPage == 0)
        {
            btn_First.Visible = false;
            btn_Prev.Visible = false;
        }
        else
        {
            btn_First.Visible = true;
            btn_Prev.Visible = true;
        }
        if (_CurrentPage == _PageCount - 1)
        {
            btn_Next.Visible = false;
            btn_Last.Visible = false;
        }
        else
        {
            btn_Next.Visible = true;
            btn_Last.Visible = true;
        }

        //_dt = _MyCommon.DB.Query(_SQL, "bindTable", _CurrentPage * _PageSize, _PageSize);

        DBManger dbm = new DBManger();
        dbm.New();
        System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
        //MyCmd.Parameters.Add("@ls_level", SqlDbType.Int).Value = 0;
        _dt = dbm.ExecuteDataTable(_SQL, MyCmd.Parameters, CommandType.Text);
                
        if (_BindGridView != null)
        {
            _BindGridView.DataSource = _dt;
            _BindGridView.DataBind();
        }

        if (BindEvent != null)
        {
            BindEvent(this, new EventArgs());
        }

    }

    protected void btn_First_Click(object sender, ImageClickEventArgs e)
    {
        _CurrentPage = 0;
        ViewState[this.ClientID + "_" + "_CurrentPage"] = _CurrentPage;
        MyBind();
    }

    protected void btn_Prev_Click(object sender, ImageClickEventArgs e)
    {
        _CurrentPage--;
        ViewState[this.ClientID + "_" + "_CurrentPage"] = _CurrentPage;
        MyBind();
    }

    protected void btn_Next_Click(object sender, ImageClickEventArgs e)
    {
        _CurrentPage++;
        ViewState[this.ClientID + "_" + "_CurrentPage"] = _CurrentPage;
        MyBind();
    }
    protected void btn_Last_Click(object sender, ImageClickEventArgs e)
    {
        _CurrentPage = _PageCount - 1;
        ViewState[this.ClientID + "_" + "_CurrentPage"] = _CurrentPage;
        MyBind();
    }
    protected void ddl_PageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        _PageSize = Int16.Parse(ddl_PageSize.SelectedValue);
        ViewState[this.ClientID + "_" + "_PageSize"] = _PageSize;
        DataBind();
    }
    protected void btn_GotoPage_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            _CurrentPage = Int16.Parse(txt_GotoPage.Text) - 1;
            if (_CurrentPage < 0)
                _CurrentPage = 0;
            if (_CurrentPage > _PageCount - 1)
                _CurrentPage = _PageCount - 1;
            MyBind();
        }
        catch { }
    }
}
