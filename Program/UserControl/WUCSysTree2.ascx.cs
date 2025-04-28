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

public partial class WUCSysTree2 : System.Web.UI.UserControl
{
    protected string TmpLastMenuCode = "";
    //TreeView預設最深層數
    protected int iTreeMaxDepth = 6;
    //若未設定時,預設TreeView之展開深度
    public int iExpandDepth = 2;
    /// <summary>
    /// 是否在標題顯示項目編號
    /// </summary>
    public bool ShowListNumber = true;
    public string DBCS = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!Page.IsPostBack)
        //{
        //    //
        //}
    }      

    /// <summary>
    /// 建立包含不同系統別之樹狀目錄
    /// </summary>
    /// <param name="ls_SiteId">系統代號</param>
    /// <param name="ls_CompanyCode">公司代號</param>
    /// <param name="ls_UserId">使用者代號</param>
    /// <param name="ls_DefaultProgramId">預設功能代號</param>
    /// 
    public void BuildTreeNode(string ls_SiteId, string ls_CompanyCode, string ls_UserId, string ls_DefaultProgramId)
    {
        string ls_LevelProgramId = "";

        DBManger dbm = new DBManger();
        switch (DBCS)
        {
            case "":
                dbm.New();
                break;
            default:
                dbm.New(DBCS);
                break;
        }

        string strSQL = "sp_view_Menu";

        System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
        MyCmd.Parameters.Add("@ls_level", SqlDbType.Int).Value = 0;
        MyCmd.Parameters.Add("@ls_SiteId", SqlDbType.NVarChar, 16).Value = ls_SiteId;
        MyCmd.Parameters.Add("@ls_CompanyCode", SqlDbType.NVarChar, 16).Value = ls_CompanyCode;
        MyCmd.Parameters.Add("@ls_UserId", SqlDbType.NVarChar, 32).Value = ls_UserId;
        MyCmd.Parameters.Add("@ls_LevelProgramId", SqlDbType.NVarChar, 16).Value = ls_LevelProgramId;
        MyCmd.Parameters.Add("@ls_DefaultProgramId", SqlDbType.NVarChar, 16).Value = ls_DefaultProgramId;

        DataTable dt = dbm.ExecStoredProcedure(strSQL, MyCmd.Parameters);

        //'根節點 
        TreeNode RootNode;
        TreeNode RootNode1;
        tvMenu.Nodes.Clear();
        for (int iRoot = 0; iRoot < dt.Rows.Count; iRoot++)
        {
            RootNode = new TreeNode();
            RootNode.Text = "[SYS" + (ShowListNumber ? "-" + (iRoot + 1).ToString() + "]" : "]") + dt.Rows[iRoot]["ProgramDesc"].ToString();
            RootNode.Value = dt.Rows[iRoot]["ProgramId"].ToString();
            RootNode.ToolTip = "請點選下方的系統項目";
            RootNode.NavigateUrl = "\\" + Application["WebSite"].ToString() + "\\" + dt.Rows[iRoot]["ProgramPath"].ToString().TrimEnd();

            //塞入TREEVIEW
            tvMenu.Nodes.Add(RootNode);
            
            TmpLastMenuCode = "";

            if (dt.Rows[iRoot]["SubMenu"].ToString().Equals("Y"))
                RootNode1 = BuildTreeNodes(dbm, strSQL, RootNode, 1, dt.Rows[iRoot]["MenuCode"].ToString().Trim(), ls_CompanyCode, ls_UserId, dt.Rows[iRoot]["ProgramId"].ToString(), ls_DefaultProgramId, "");
        }

        //初始時只展開至第2層
        tvMenu.ExpandDepth = iExpandDepth;
    }

    /// <summary>
    /// 建立單一系統別之樹狀目錄
    /// </summary>
    /// <param name="ls_SiteId">系統代號</param>
    /// <param name="ls_CompanyCode">公司代號</param>
    /// <param name="ls_UserId">使用者代號</param>
    /// <param name="ls_DefaultProgramId">預設功能代號</param>
    public void BuildSysTreeNode(string ls_SiteId, string ls_CompanyCode, string ls_UserId, string ls_DefaultProgramId)
    {
        string ls_LevelProgramId = "";

        DBManger dbm = new DBManger();
        switch (DBCS)
        {
            case "":
                dbm.New();
                break;
            default:
                dbm.New(DBCS);
                break;
        }
        string strSQL = "sp_view_Menu";

        System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
        MyCmd.Parameters.Add("@ls_level", SqlDbType.Int).Value = 2;
        MyCmd.Parameters.Add("@ls_SiteId", SqlDbType.NVarChar, 16).Value = ls_SiteId;
        MyCmd.Parameters.Add("@ls_CompanyCode", SqlDbType.NVarChar, 16).Value = ls_CompanyCode;
        MyCmd.Parameters.Add("@ls_UserId", SqlDbType.NVarChar, 32).Value = ls_UserId;
        MyCmd.Parameters.Add("@ls_LevelProgramId", SqlDbType.NVarChar, 16).Value = ls_LevelProgramId;
        MyCmd.Parameters.Add("@ls_DefaultProgramId", SqlDbType.NVarChar, 16).Value = ls_DefaultProgramId;

        DataTable dt = dbm.ExecStoredProcedure(strSQL, MyCmd.Parameters);

        //'根節點 
        TreeNode RootNode;
        TreeNode RootNode1;
        tvMenu.Nodes.Clear();
        for (int iRoot = 0; iRoot < dt.Rows.Count; iRoot++)
        {
            RootNode = new TreeNode();
            RootNode.Text = (ShowListNumber ? (iRoot + 1).ToString() + "." : "") + dt.Rows[iRoot]["ProgramDesc"].ToString() + "(" + dt.Rows[iRoot]["ProgramId"].ToString().TrimEnd() + ")";
            RootNode.Value = dt.Rows[iRoot]["ProgramId"].ToString();
           
            if (dt.Rows[iRoot]["SubMenu"].ToString().Equals("Y"))
                RootNode.ToolTip = "請展開後，點選子功能";
            else
                RootNode.ToolTip = dt.Rows[iRoot]["ProgramName"].ToString();
            string tempPath = dt.Rows[iRoot]["ProgramPath"].ToString().TrimEnd();
            if (string.IsNullOrEmpty(tempPath))
            {
                RootNode.NavigateUrl = "";
            }
            else
            {
                RootNode.NavigateUrl = "\\" + Application["WebSite"].ToString() + "\\" + (string.IsNullOrEmpty(tempPath) ? "MonthStar.aspx" : tempPath);
            }
            //塞入TREEVIEW
            tvMenu.Nodes.Add(RootNode);

            TmpLastMenuCode = "";

           if (dt.Rows[iRoot]["SubMenu"].ToString().Equals("Y"))
               RootNode1 = BuildTreeNodes(dbm, strSQL, RootNode, 2, ls_SiteId, ls_CompanyCode, ls_UserId, dt.Rows[iRoot]["ProgramId"].ToString(), ls_DefaultProgramId, (iRoot + 1).ToString());
        }

        //初始時只展開至第2層
        tvMenu.ExpandDepth = iExpandDepth;
    }
    
    /// <summary>
    /// 建立指定節點所屬子節點
    /// </summary>
    /// <param name="dbm">資料庫連線管理</param>
    /// <param name="strSQL">SQL命令(使用SP)</param>
    /// <param name="RootNode">指定節點</param>
    /// <param name="iTreeLevel">指定層數</param>
    /// <param name="ls_SiteId">系統代號</param>
    /// <param name="ls_CompanyCode">公司代號</param>
    /// <param name="ls_UserId">使用者代號</param>
    /// <param name="ls_LevelProgramId">指定層級之母程式代號</param>
    /// <param name="ls_DefaultProgramId">預設功能代號</param>
    /// <returns>一串樹狀節點</returns>
    private TreeNode BuildTreeNodes(DBManger dbm, string strSQL, TreeNode RootNode, int iTreeLevel, string ls_SiteId, string ls_CompanyCode, string ls_UserId, string ls_LevelProgramId, string ls_DefaultProgramId, string strShowTitle)
    {
        //iTreeLevel 決定顯示的層數
        if (iTreeLevel <= iTreeMaxDepth)
        {
            int iChild = 0;
            int iSowTreeLevel = iTreeLevel;
            string strTitle = "";

            System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
            MyCmd.Parameters.Add("@ls_level", SqlDbType.Int).Value = iTreeLevel;
            MyCmd.Parameters.Add("@ls_SiteId", SqlDbType.NVarChar, 16).Value = ls_SiteId;
            MyCmd.Parameters.Add("@ls_CompanyCode", SqlDbType.NVarChar, 16).Value = ls_CompanyCode;
            MyCmd.Parameters.Add("@ls_UserId", SqlDbType.NVarChar, 32).Value = ls_UserId;
            MyCmd.Parameters.Add("@ls_LevelProgramId", SqlDbType.NVarChar, 16).Value = ls_LevelProgramId;
            MyCmd.Parameters.Add("@ls_DefaultProgramId", SqlDbType.NVarChar, 16).Value = ls_DefaultProgramId;

            DataTable dtLevel = dbm.ExecStoredProcedure(strSQL, MyCmd.Parameters);

            dtLevel = dbm.ExecStoredProcedure(strSQL, MyCmd.Parameters);

            for (int iLevel = 0; iLevel < dtLevel.Rows.Count; iLevel++)
            {
                if (iLevel == 0)
                {
                    TmpLastMenuCode = dtLevel.Rows[iLevel]["MenuCode"].ToString().TrimEnd();
                }
                else
                {
                    if ((TmpLastMenuCode.Equals(dtLevel.Rows[iLevel]["MenuCode"].ToString().TrimEnd())) || ((iLevel + 1) == dtLevel.Rows.Count) || (dtLevel.Rows[iLevel]["SubMenu"].ToString().Trim().Equals("Y")))
                    {
                        iTreeLevel -= 1;
                    }
                    else
                    {
                        iTreeLevel += 1;
                        TmpLastMenuCode = dtLevel.Rows[iLevel]["MenuCode"].ToString().TrimEnd();
                    }
                }

                strTitle = strShowTitle + ((strShowTitle.Trim().Length > 0) ? "-" : "") + (iLevel + 1).ToString();
                TreeNode ParentNode = new TreeNode();
                //ParentNode.Text = iSowTreeLevel.ToString() + "-" + (iLevel + 1).ToString() + "." + dtLevel.Rows[iLevel]["ProgramDesc"].ToString().TrimEnd() + "(" + dtLevel.Rows[iLevel]["ProgramId"].ToString().TrimEnd() + ")";
                ParentNode.Text = (ShowListNumber ? strTitle + "." : "") + dtLevel.Rows[iLevel]["ProgramDesc"].ToString().TrimEnd() + "(" + dtLevel.Rows[iLevel]["ProgramId"].ToString().TrimEnd() + ")";
                ParentNode.Value = dtLevel.Rows[iLevel]["ProgramId"].ToString().TrimEnd();
                ParentNode.ToolTip = "請展開後，點選子功能";
                ParentNode.NavigateUrl = "\\" + Application["WebSite"].ToString() + "\\" + dtLevel.Rows[iLevel]["ProgramPath"].ToString().TrimEnd();

                if (dtLevel.Rows[iLevel]["SubMenu"].ToString().Trim().Equals("Y"))
                {
                   // iTreeLevel += 1;
                    //RootNode.ChildNodes.Add(BuildTreeNodes(dbm, strSQL, ParentNode, iTreeLevel, ls_SiteId, ls_CompanyCode, ls_UserId, dtLevel.Rows[iLevel]["ProgramId"].ToString(), ls_DefaultProgramId, strTitle));
                }

                if (dtLevel.Rows[iLevel]["SubMenu"].ToString().Trim().Equals("N"))
                {
                    iChild += 1;
                    iTreeLevel -= 1;

                    //'建立新的子節點
                    TreeNode ChildNode = new TreeNode();

                    //'設定節點顯示文字  1-1 ProgramName
                    //ChildNode.Text = iSowTreeLevel.ToString() + "-" + (iLevel + 1).ToString() + "." + dtLevel.Rows[iLevel]["ProgramDesc"].ToString().TrimEnd();
                    ChildNode.Text = (ShowListNumber ? strTitle + "." : "") + dtLevel.Rows[iLevel]["ProgramDesc"].ToString().TrimEnd();
                    
                    //'設定節點編號資料
                    ChildNode.Value = iLevel.ToString();
                    //'設定節點所連結的網頁，透過 javascript 來控制另一個框架的網頁內容。
                    string strUrl = dtLevel.Rows[iLevel]["ProgramPath"].ToString().TrimEnd();
                    if (strUrl.Contains("http://") || strUrl.Contains("https://"))
                    {
                        ChildNode.NavigateUrl = dtLevel.Rows[iLevel]["ProgramPath"].ToString().TrimEnd();
                    }
                    else
                    {
                        ChildNode.NavigateUrl = "\\" + Application["WebSite"].ToString() + "\\" + dtLevel.Rows[iLevel]["ProgramPath"].ToString().TrimEnd();
                    }
                    ChildNode.ToolTip = dtLevel.Rows[iLevel]["ProgramName"].ToString().TrimEnd();

                    RootNode.ChildNodes.Add(ChildNode);
                }
            }

            iTreeLevel -= 1;
            dtLevel.Dispose();
        }

        return RootNode;
    }

    /// <summary>
    /// TreeView預設最深層數
    /// </summary>
    public int TreeMaxDepth
    {
        get
        {
            return iTreeMaxDepth;
        }
        set
        {
            if (value <= 9)
                iTreeMaxDepth = value;
            else
                iTreeMaxDepth = 9;
        }
    }

    /// <summary>
    /// 若未設定,則預設展開深度 2
    /// </summary>
    public int ExpandDepth{

        get{
            return tvMenu.ExpandDepth;
        }
        set{
            tvMenu.ExpandDepth = value;
        }        
    }

    public string TreeViewSkinId
    {
        get
        {
            return tvMenu.SkinID;
        }
        set
        {
            tvMenu.SkinID = value;
        }
    }

    public string LineImagesFolder
    {
        get
        {
            return tvMenu.LineImagesFolder;
        }
        set
        {
            tvMenu.LineImagesFolder = value;
        }
    }
}
