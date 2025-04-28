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

public partial class StyleTitle : System.Web.UI.UserControl
{
    string _Title = "";
    string _SubTitle = "";
    private bool _ShowBackToPre = true;

    public bool ShowBackToPre
    {
        get { return _ShowBackToPre; }
        set { _ShowBackToPre = value; }
    }

    private bool _ShowHome = true;

    public bool ShowHome
    {
        get { return _ShowHome; }
        set { _ShowHome = value; }
    }

    private bool _ShowUser = true;

    public bool ShowUser
    {
        get { return _ShowUser; }
        set { _ShowUser = value; }
    }

    public string Title
    {
        set
        {
            _Title = value;
        }
        get
        {
            return _Title;
        }
    }

    public string SubTitle
    {
        set
        {
            _SubTitle = value;
        }
        get
        {
            return _SubTitle;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lbl_User.Visible = false;
        btn_Home.Visible = false;        
        btn_back.Visible = false;
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        InitData();
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        FormatUI();

    }
    private void FormatUI()
    {
        lblTitle.Text = _Title;
        if (_SubTitle.Length > 0)
        {
            lblSubTitle.ForeColor = System.Drawing.Color.DarkBlue;
            lblSubTitle.Text = _SubTitle;
        }
        else
        {
            lblSubTitle.Text = "";
        }
        lbl_User.Visible = _ShowUser;
        btn_Home.Visible = _ShowHome;        
        btn_back.Visible = _ShowBackToPre;

        if (_ShowUser)
        {
            string sUser = "";
            ////if (_MyCommon.User.ClientL1_PK != Int64.MaxValue)
            ////{
            ////    DB_NC01F04L1 NC01F04L1 = new DB_NC01F04L1(_MyCommon);
            ////    if (NC01F04L1.GetData(_MyCommon.User.ClientL1_PK))
            ////    {
            ////        sUser = NC01F04L1.Name;
            ////    }
            ////}
            ////if (_MyCommon.User.Channle_PK != Int64.MaxValue)
            ////{
            ////    DB_CM01F01 CM01F01 = new DB_CM01F01(_MyCommon);
            ////    if (CM01F01.GetData(_MyCommon.User.Channle_PK))
            ////    {
            ////        lbl_User.ToolTip = String.Format("[{0}]:{1}", CM01F01.ChannelCode, CM01F01.ChannelCName);
            ////    }
            ////}

            //if (sUser == "")
            //{
            //    sUser = _MyCommon.User.UserName;
            //}
            //lbl_User.Text = String.Format("[{0}]:{1}", _MyCommon.User.UserID, sUser);

            //if (!String.IsNullOrEmpty(_MyCommon.User.SimulateUserId))
            //{
            //    if (_MyCommon.User.SimulateUserId == "Empty")
            //    {
            //        lbl_User.Text += "無Ageng資料,請切換身份!";
            //    }
            //    else
            //    {
            //        lbl_User.Text += String.Format("模擬USER:{0}", _MyCommon.User.SimulateUserId);
            //    }
            //}
        }
    }



    private void InitData()
    {


        if (ViewState["_StyleTitle"] == null)
        {
            ViewState["_StyleTitle"] = _Title;
        }
        else
        {
            _Title = (string)ViewState["_StyleTitle"];
        }

        if (ViewState["_StyleShowHome"] == null)
        {
            ViewState["_StyleShowHome"] = _ShowHome;
        }
        else
        {
            _ShowHome = (bool)ViewState["_StyleShowBackToPre"];
        }

        if (ViewState["_StyleShowBackToPre"] == null)
        {
            ViewState["_StyleShowBackToPre"] = _ShowBackToPre;
        }
        else
        {
            _ShowBackToPre = (bool)ViewState["_StyleShowBackToPre"];
        }

        if (ViewState["_StyleShowUser"] == null)
        {
            ViewState["_StyleShowUser"] = _ShowUser;
        }
        else
        {
            _ShowUser = (bool)ViewState["_StyleShowUser"];
        }
    }

 }
