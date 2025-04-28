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

/// <summary>
/// 2012/07/25 kaya 修正是否在職切換時人員清單更新
/// 2012/07/30 kaya 修正是部門切換時,部門清單不再更新
/// </summary>
public partial class SearchList : System.Web.UI.UserControl
{
    public delegate void SelectedIndexChanged(object sender, SelectEventArgs e);
    public event SelectedIndexChanged SelectedChanged;
    public int ShowResignEmp = 0;

    DBManger _MyDBM;
    
    public class SelectEventArgs : EventArgs
    {
        public string Comp,TComp;
        public string Dept,TDept;
        public string Emp,TEmp;
        public string type;
    }

    public enum theDropDownList : int
    {
        CompanyList = 1,
        DepartmentsList = 2,
        EmployeeList = 3
    }

    #region SelectValue
    public string CompanyValue
    {
        get
        {
            if ( CompanyList.Items.Count > 0 )  return this.CompanyList.SelectedItem.Value;
            else return "";
        }
        set
        {
            for ( int i = 0 ; i < CompanyList.Items.Count ; i++ )
            {
                if ( CompanyList.Items[ i ].Value == value )
                {
                    CompanyList.SelectedIndex = i;
                    CompanyList_SelectedIndexChanged ( null , null );
                    break;
                }
            }
        }
    }
    public string DepartmentValue
    {
        get
        {
            if ( DepartmentsList.Items.Count > 0 )  return this.DepartmentsList.SelectedItem.Value;
            else return "";
        }
        set
        {
            for ( int i = 0 ; i < DepartmentsList.Items.Count ; i++ )
            {
                if ( DepartmentsList.Items[ i ].Value == value )
                {
                    DepartmentsList.SelectedIndex = i;
                    DepartmentsList_SelectedIndexChanged ( null , null );
                    break;
                }
            }
        }
    }
    public string EmployeeValue
    {
        get
        {
            if ( EmployeeList.Items.Count > 0 ) return this.EmployeeList.SelectedItem.Value;
            else return "";
        }
        set
        {
            for ( int i = 0 ; i < EmployeeList.Items.Count ; i++ )
            {
                if ( EmployeeList.Items[ i ].Value == value )
                {
                    EmployeeList.SelectedIndex = i;
                    EmployeeList_SelectedIndexChanged ( null , null );
                    break;
                }
            }
        }
    } 
    #endregion

    #region SeletctText
    public string CompanyText { get { return this.CompanyList.SelectedItem.Text; } }
    public string DepartmentText { get { return this.DepartmentsList.SelectedItem.Text; } }
    public string EmployeeText { get { return this.EmployeeList.SelectedItem.Text; } }
    #endregion

    public ListItemCollection LeaveType_Basic = new ListItemCollection();

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (!IsPostBack)    loadCompany();
    }

    private void loadCompany()
    {
        _MyDBM = new DBManger();
        _MyDBM.New();
        CompanyList.Items.Clear ( );
        DataTable DT = _MyDBM.ExecuteDataTable ( "SELECT Company,CompanyName FROM Company Order by Company" );
        if ( DT.Rows.Count > 0 )
        {
            CompanyList.Items.Add ( Li ( "請選擇" , "" ) );
            for ( int i = 0 ; i < DT.Rows.Count ; i++ )
            {
                string Company = DT.Rows[ i ][ "Company" ].ToString ( );
                string CompanyName = DT.Rows[ i ][ "CompanyName" ].ToString ( );
                CompanyList.Items.Add ( Li ( Company + " - " + CompanyName , Company ) );
            }
        }
        else    CompanyList.Items.Add ( Li ( "無資料" , "" ) );

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (CompanyList.Items.Count == 0) loadCompany();
    }

    public void ReSetList()
    {
        _MyDBM = new DBManger();
        _MyDBM.New();
        string TSql = "";

        #region 部門
        if (Request.Form[0].Contains(CompanyList.UniqueID))
        {
            //狀態父階 SystemCode=300; 子階不為[取消]者
            TSql = "SELECT DepCode,DepName FROM Department Where Company ='" + CompanyValue + "' and IsNull(DepStatus,'') != '5F6FF116-19C3-44F0-B9BD-90A361FA9088' Order by DepCode";
            SetDropDownList(DepartmentsList, DepTb, TSql, "DepCode", "DepName", null, null);
        }
        #endregion

        #region 人事
        if (Request.Form[0].Contains(CompanyList.UniqueID) || Request.Form[0].Contains(DepartmentsList.UniqueID) || Request.Form[0].Contains("rbResignC"))
        {
            TSql = "SELECT EmployeeId,EmployeeName FROM Personnel_Master Where Company ='" + CompanyValue +
                "' And DeptId Like '" + DepartmentValue + "'";
            if (ShowResignEmp == 0)
                TSql += " And Upper(IsNull(ResignCode,'')) != 'Y' ";
            else if (ShowResignEmp == 1)
                TSql += " And Upper(IsNull(ResignCode,'')) = 'Y' ";
            TSql += " Order by EmployeeId";
            SetDropDownList(EmployeeList, EmpTb, TSql, "EmployeeId", "EmployeeName", null, null);
        }
        #endregion
    }

    #region 提供下拉式選單
    public DropDownList Company
    {
        get
        {
            return CompanyList;
        }
    }
    public DropDownList Employee
    {
        get
        {
            return EmployeeList;
        }
    }
    public DropDownList Department
    {
        get
        {
            return DepartmentsList;
        }
    }
    #endregion


    #region 提供整個Row
    public HtmlGenericControl DepRow
    {
        get
        {
            return DepartmentsRow;
        }
    }

    public HtmlGenericControl EmpRow
    {
        get
        {
            return EmployeeRow;
        }
    }



    #endregion

    #region 提供 TextBox
    public TextBox CompanyTb
    {
        get
        {
            return ComTb;
        }
    }
    public TextBox DepartmentTb
    {
        get
        {
            return DepTb;
        }
    }
    public TextBox EmployeeTb
    {
        get
        {
            return EmpTb;
        }
    }
    #endregion

    // 下拉式選單事件
    protected void CompanyList_SelectedIndexChanged ( object sender , EventArgs e )
    {
        _MyDBM = new DBManger ( );
        _MyDBM.New ( );
        string TSql = "";

        #region 部門
        //狀態父階 SystemCode=300; 子階不為[取消]者
        TSql = "SELECT DepCode,DepName FROM Department Where Company ='" + CompanyValue + "' and IsNull(DepStatus,'') != '5F6FF116-19C3-44F0-B9BD-90A361FA9088' Order by DepCode";
        SetDropDownList ( DepartmentsList , DepTb , TSql , "DepCode" , "DepName" , sender , e );
        #endregion

        #region 人事
        TSql = "SELECT EmployeeId,EmployeeName FROM Personnel_Master Where Company ='" + CompanyValue +
            "' And DeptId Like '" + DepartmentValue + "'";
        if (ShowResignEmp == 0)
            TSql += " And Upper(IsNull(ResignCode,'')) != 'Y' ";
        else if (ShowResignEmp == 1)
            TSql += " And Upper(IsNull(ResignCode,'')) = 'Y' ";
        TSql += " Order by EmployeeId";
        SetDropDownList ( EmployeeList , EmpTb , TSql , "EmployeeId" , "EmployeeName" , sender , e );
        #endregion

        #region 假期
        DataTable DT = _MyDBM.ExecuteDataTable ( "select Leave_Id ,Leave_Desc from LeaveType_Basic where Company='" + CompanyValue + "' Order by Leave_Id" );
        if ( DT.Rows.Count > 0 )
        {
            for ( int i = 0 ; i < DT.Rows.Count ; i++ )
            {
                string Leave_Id = DT.Rows[ i ][ "Leave_Id" ].ToString ( ).Trim ( );
                string Leave_Desc = DT.Rows[ i ][ "Leave_Desc" ].ToString ( ).Trim ( );
                LeaveType_Basic.Add ( Li ( Leave_Id + " - " + Leave_Desc , Leave_Id ) );
            }
        }
        #endregion

        if ( SelectedChanged != null ) SelectedChanged ( this , GetEA ( "Company" ) );
    }

    protected void DepartmentsList_SelectedIndexChanged ( object sender , EventArgs e )
    {
        #region 人事
        string TSql = "SELECT EmployeeId,EmployeeName FROM Personnel_Master Where Company ='" + CompanyValue + "' And DeptId Like '" + DepartmentValue + "'";
        if (ShowResignEmp == 0)
            TSql += " And Upper(IsNull(ResignCode,'')) != 'Y' ";
        else if (ShowResignEmp == 1)
            TSql += " And Upper(IsNull(ResignCode,'')) = 'Y' ";
        TSql += " Order by EmployeeId";
        SetDropDownList ( EmployeeList , EmpTb , TSql , "EmployeeId" , "EmployeeName" , sender ,e);
        #endregion

        if ( SelectedChanged != null ) SelectedChanged ( this , GetEA ( "Department" ) );
    }

    protected void EmployeeList_SelectedIndexChanged ( object sender , EventArgs e )
    {
        if ( SelectedChanged != null ) SelectedChanged ( this , GetEA ( " Employee" ) );
    }

    /// <summary>
    /// 取得指定控制項之ID
    /// </summary>
    /// <param name="theDL">SearchList.theDropDownList/1/2/3</param>    
    public string GetID(theDropDownList theDL)
    {
        string retID = "";
        switch (theDL)
        {
            case theDropDownList.CompanyList:
                retID = CompanyList.UniqueID;
                break;
            case theDropDownList.DepartmentsList:
                retID = DepartmentsList.UniqueID;
                break;
            case theDropDownList.EmployeeList:
                retID = EmployeeList.UniqueID;
                break;
        }
        return retID;
    }

    // 下拉式選單設定 小程式
    private void SetDropDownList ( DropDownList DDL , TextBox TB , string Sql , string Id , string Name , object sender , EventArgs e )
    {
        _MyDBM = new DBManger ( );
        _MyDBM.New ( );
        DataTable DT = _MyDBM.ExecuteDataTable ( Sql );
        DDL.Items.Clear ( );
        if ( DT.Rows.Count > 0 )
        {
            DDL.Items.Add ( Li ( "全部" , "%%" ) );
            for ( int i = 0 ; i < DT.Rows.Count ; i++ )
            {
                string DDLId = DT.Rows[ i ][ Id ].ToString ( ).Trim ( );
                string DDLName = DT.Rows[ i ][ Name ].ToString ( ).Trim ( );
                DDL.Items.Add ( Li ( DDLId + " - " + DDLName , DDLId ) );
            }

            SetNData ( DDL , TB , true );
        }
        else
            SetNData ( DDL , TB , false );
    }

    // EventArgs 小程式
    private SelectEventArgs GetEA (string type )
    {
        SelectEventArgs EventArgs = new SelectEventArgs ( );
        EventArgs.Comp = CompanyValue;
        EventArgs.Dept = DepartmentValue;
        EventArgs.Emp = EmployeeValue;
        EventArgs.TComp = CompanyText;
        EventArgs.TDept = DepartmentText;
        EventArgs.TEmp = EmployeeText;
        EventArgs.type = type;
        return EventArgs;
    }

    // ListItem 小程式
    private ListItem Li (string Text,string Value )
    {
        ListItem li = new ListItem ( );
        li.Text = Text;
        li.Value = Value;
        return li;
    }

    // 資料設定 小程式
    private void SetNData (DropDownList DDL, TextBox TB , bool Enabled )
    {
        if ( !Enabled ) DDL.Items.Add ( Li ( "無資料" , "" ) );
        DDL.Enabled = Enabled;
        TB.Enabled = Enabled;
        TB.BackColor = System.Drawing.ColorTranslator.FromHtml ( Enabled ? "#FFFFFF" : "#CCCCCC" );
    }

}
