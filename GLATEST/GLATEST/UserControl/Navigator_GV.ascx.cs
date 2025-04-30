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

public partial class SystemControl_Navigator_GV : System.Web.UI.UserControl
{
    protected int _PageSize = 10;
    protected int _RecordCount = 0;

    public event EventHandler BindEvent;
    //public int PageSize
    //{
    //    get { return _PageSize; }
    //    set
    //    {
    //        if (_PageSize == value)
    //            return;
    //        _PageSize = value;
    //        BindPageDdl();
    //    }
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
            //if (_BindGridView != null)
            //{
            //    _BindGridView.RowDataBound += new GridViewRowEventHandler(_BindGridView_RowDataBound);
            //    _BindGridView.DataBound += new EventHandler(_BindGridView_DataBound);
            //    _BindGridView.AllowPaging = false;
            //}
        }
    }

    public bool PageSizeEnble
    {
        get
        {
            return ddl_PageSize.Enabled;
        }
        set
        {
            ddl_PageSize.Enabled = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //_BindGridView.DataBinding += new EventHandler(_BindGridView_DataBinding);
        _BindGridView.DataBound += new EventHandler(_BindGridView_DataBound);
        _PageSize = _BindGridView.PageSize;
        if (!IsPostBack)
        {
            BindPageDdl();
            _BindGridView_DataBound(sender, e);
        }
    }

    //protected override void OnDataBinding(EventArgs e)
    //{

    //    base.OnDataBinding(e);
    //}


    //void _BindGridView_DataBinding(object sender, EventArgs e)
    //{
    //}

    void _BindGridView_DataBound(object sender, EventArgs e)
    {
        SetPage();
        MyBind();
    }

    private void SetPage()
    {
        txt_GotoPage.Text = (_BindGridView.PageIndex + 1).ToString();

        //lbl_Desc.Text = String.Format("筆，共{0}筆，第", _RecordCount);
        lbl_Desc.Text = "筆，第";
        lbl_Desc2.Text = String.Format("頁，共{0}頁", _BindGridView.PageCount);
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
        //ViewState[this.ClientID + "_" + "_PageSize"] = _PageSize;
        //ViewState[this.ClientID + "_" + "_PageCount"] = _PageCount;
        //ViewState[this.ClientID + "_" + "_CurrentPage"] = _CurrentPage;
        //ViewState[this.ClientID + "_" + "_RecordCount"] = _RecordCount;

        //txt_GotoPage.Text = (_CurrentPage + 1).ToString();
        //base.OnPreRender(e);
        //lbl_Desc.Text = String.Format("筆，共{0}筆，第", _RecordCount);
        //lbl_Desc2.Text = String.Format("頁，共{0}頁", _PageCount);
    }



    void _BindGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //throw new Exception("The method or operation is not implemented.");
    }

    //public override void DataBind()
    //{
    //    base.DataBind();
    //    if (IsPostBack)
    //    {
    //        _SQL = (string)ViewState[this.ClientID + "_" + "SQL"];
    //        _SQLCount = (string)ViewState[this.ClientID + "_" + "SQLCount"];
    //        _PageSize = (int)ViewState[this.ClientID + "_" + "_PageSize"]; ;
    //        _PageCount = (int)ViewState[this.ClientID + "_" + "_PageCount"]; ;
    //        _CurrentPage = (int)ViewState[this.ClientID + "_" + "_CurrentPage"]; ;
    //        _RecordCount = (int)ViewState[this.ClientID + "_" + "_RecordCount"]; ;
    //    }

    //    _MyCommon.DR = _MyCommon.DB.Query(_SQLCount);
    //    _MyCommon.DR.Read();

    //    _RecordCount = (int)_MyCommon.DR[0];

    //    if ((_RecordCount % _PageSize) == 0)
    //        _PageCount = (_RecordCount / _PageSize);
    //    else
    //        _PageCount = (_RecordCount / _PageSize) + 1;

    //    _CurrentPage = 0;

    //    MyBind();


    //}

    private void MyBind()
    {
        if (_BindGridView.PageIndex == 0)
        {
            btn_First.Visible = false;
            btn_Prev.Visible = false;
        }
        else
        {
            btn_First.Visible = true;
            btn_Prev.Visible = true;
        }
        if (_BindGridView.PageIndex == _BindGridView.PageCount - 1)
        {
            btn_Next.Visible = false;
            btn_Last.Visible = false;
        }
        else
        {
            btn_Next.Visible = true;
            btn_Last.Visible = true;
        }


        if (BindEvent != null)
        {
            BindEvent(this, new EventArgs());
        }

    }

    protected void btn_First_Click(object sender, ImageClickEventArgs e)
    {
        _BindGridView.PageIndex = 0;
        SetPage();
        MyBind();
    }

    protected void btn_Prev_Click(object sender, ImageClickEventArgs e)
    {
        if (_BindGridView.PageIndex > 0)
        {
            _BindGridView.PageIndex--;
            SetPage();
            MyBind();
        }
    }

    protected void btn_Next_Click(object sender, ImageClickEventArgs e)
    {
        if (_BindGridView.PageIndex < _BindGridView.PageCount - 1)
        {
            _BindGridView.PageIndex++;
            SetPage();
            MyBind();
        }
    }
    protected void btn_Last_Click(object sender, ImageClickEventArgs e)
    {
        if (_BindGridView.PageCount > 0)
        {
            _BindGridView.PageIndex = _BindGridView.PageCount - 1;
            SetPage();
            MyBind();
        }
    }

    protected void ddl_PageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        _PageSize = Int16.Parse(ddl_PageSize.SelectedValue);
        _BindGridView.PageSize = _PageSize;
    }
    protected void btn_GotoPage_Click(object sender, ImageClickEventArgs e)
    {
        int gotoPage = 0;
        try
        {
            gotoPage = Int16.Parse(txt_GotoPage.Text) - 1;
            if (gotoPage < 0)
                gotoPage = 0;
            if (gotoPage > _BindGridView.PageCount - 1)
                gotoPage = _BindGridView.PageCount - 1;
            _BindGridView.PageIndex = gotoPage;
            SetPage();
            MyBind();
        }
        catch { }
    }

}
