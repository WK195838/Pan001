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
using System.IO;
using System.Text.RegularExpressions;

public class Global
{
    public static string str = "";
    public static string strcom = "";
    public static string TbName = ""; //暫存新增過的資料表名稱

}

public partial class CreatTable : System.Web.UI.Page
{
    string _ProgramId = "SYS000";
    UserInfo _UserInfo = new UserInfo();
    protected void Page_PreInit(object sender, EventArgs e)
    {//頁面初始化前,先設定好系統使用的佈景主題(主要是決定LOGO)
        if (Session["Theme"] != null)
            Page.Theme = Session["Theme"].ToString();

        if (Session["MasterPage"] != null)
            Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (_UserInfo.AuthLogin == false)
            {
                Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=UnLogin");
            }
            else
            {
                //等特殊程式,特別開放權限方式(比對角色與帳號成功,則不用檢核權限)
                if (!(_UserInfo.UData.Role.ToLower().Contains("administrator") && _UserInfo.UData.UserId.ToLower().Equals("admin")))
                    if (_UserInfo.CheckPermission(_ProgramId) == false)
                        Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
            }
        }
    }

    #region --/ 執行前檢查作業 /--
    public bool CheckAll() 
    {
        if (Check_Input() && Check_LinkSql())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
  
    // 檢查輸入 //
    public bool Check_Input()
    {
        bool n = true;

        if (TB_FileName.Text.Length < 1)
            TextBox1.Text = "";
        else
            TextBox1.Text = "目前已上傳檔案為 " + TB_FileName.Text + "\r";

        if (IsPostBack)
        {
            Print("開始檢查與修正各項資料..");

            if (TB_Server.Text.Length < 1)
            {
                Print("*請輸入伺服器位置!");
                n = false;
            }
            //else
            //    TB_Server.Text = CheckText(TB_Server.Text);

            if (TB_DataBase.Text.Length < 1)
            {
                Print("*請輸入資料庫名稱!");
                n = false;
            }
            else
                TB_DataBase.Text = CheckText(TB_DataBase.Text);

            if (TB_Name.Text.Length < 1)
            {
                Print("*請輸入帳號!");
                n = false;
            }
            else
                TB_Name.Text = CheckText(TB_Name.Text);

            if (TB_Password.Text.Length < 1)
            {
                Print("*請輸入密碼!");
                n = false;
            }
            else
                TB_Password.Text = CheckText(TB_Password.Text);

            if (FileUpload1.HasFile || TB_FileName.Text.Length > 1)
            {
                if (FileUpload1.HasFile)
                {
                    TB_temp1.Text = "";
                    string temp = FileUpload1.FileName.Remove(0, FileUpload1.FileName.LastIndexOf('.'));
                    if (temp == ".csv" || temp == ".CSV")
                    {
                        FileUpload1.SaveAs(Server.MapPath(FileUpload1.FileName));
                        Print(FileUpload1.FileName);
                        Print("檔案已上傳");
                        TB_temp1.Text = Server.MapPath(FileUpload1.FileName);
                        TB_FileName.Text = FileUpload1.FileName;
                    }
                    else
                    {
                        Print("*請選擇 csv 檔案!");
                        n = false;
                    }
                }
            }
            else
            {
                Print("*請選擇檔案!");
                n = false;
            }

            if (n)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    // 檢查伺服器連線 //
    private bool Check_LinkSql()
    {
        Global.strcom = @"Data Source=" + TB_Server.Text + ";Initial Catalog=" + TB_DataBase.Text + ";Persist Security Info=True;User ID=" + TB_Name.Text + ";Password=" + TB_Password.Text;
        SqlConnection SqlConn = new SqlConnection(Global.strcom);
        if (SqlConn.State != ConnectionState.Open)
        {
            TextBox1.Text = "";
            Print("建立連線.....");
            try
            {
                SqlConn.Open();
                SqlConn.Close();
                return true;

            }
            catch
            {
                Print("連線失敗，請檢查輸入的資料是否正確");
                return false;
            }

        }
        else
        {
            Print("已連線.....");
            return true;
        }
    }

    // 修正使用者輸入的文字 //
    public String CheckText(String str)
    {
        return Regex.Replace(str, @"[\W_]+", "");
    }
    #endregion
    

    #region ---/ 按鈕事件 /---

    protected void BT_CreateTable_Click(object sender, EventArgs e)
    {
        if (CheckAll())
        {
            Global.TbName = "";
            Readcsv(1);
        }
    }
    protected void BT_DropTable_Click(object sender, EventArgs e)
    {
        if (CheckAll())
        {
            if (Global.TbName.Length < 1)
            {
                TextBox1.Text = "未建立新資料表\r";
                return;
            }
            string[] tablename = Global.TbName.TrimEnd(',').Split(new char[] { ',' });
            int i = 0;
            for (int ai = 0; ai < tablename.Length; ai++)
            {
                
                if(Drop_Table(tablename[ai]))
                {
                    i++;
                }
            }
            TextBox1.Text += "共刪除 " + i.ToString() + " 個資料表";
            Global.TbName = "";
        }
    }
    protected void BT_UpdataTable_Click(object sender, EventArgs e)
    {
        if (CheckAll())
        {
            Global.TbName = "";
            Readcsv(2);
        }
    }
    protected void BT_ReduceTable_Click(object sender, EventArgs e)
    {
        if (CheckAll())
        {

        }
    }

    #endregion


    //  !讀取 CSV 檔案  //
    public void Readcsv(int n)
    {
        StreamReader ReadFile = new StreamReader(TB_temp1.Text, System.Text.Encoding.Default);
        string SReadData = "";
        int i = 0;
        int CTC = 0; //CreateTableCounter

        if (ReadFile.Peek() != -1)
        {
            // 第一欄是否為欄位名稱
            if (CB_HDR.Checked)
            {
                SReadData = ReadFile.ReadLine();
                i++;
            }

            string Name = "";
            string Name_Description = "";
            string Field = "";
            string FieldD = "";
            string Field_Description = "";

            //讀到檔案結束為止
            while (ReadFile.Peek() != -1)
            {
                SReadData = ReadFile.ReadLine();
                string[] tmpData = SReadData.Split(new char[] { ',' });
                i++;

                
                // 判斷欄位數量
                int sw = 0;
                if (tmpData.Length < 4)
                    sw = 1;
                else if (tmpData.Length > 7)
                    sw = 2;
                else
                    sw = 3;
                switch (sw)
                {
                    case 1:
                        Global.str = "第 " + i.ToString() + " 行資料不足";
                        Print(Global.str);
                        break;
                    case 2:
                        Global.str = "第 " + i.ToString() + " 行資料有誤";
                        Print(Global.str);
                        break;
                    case 3:
                        #region ---/ 分析資料

                        if (tmpData[0] != Name) // 判斷是否為同一資料表
                        {
                            if (Name.Length > 0)
                            {
                                Field = Field.Remove(Field.LastIndexOf(':'));
                                CTC = AnalyticsData(CTC,n, Name, Name_Description, Field, FieldD, Field_Description);
                            }

                            Name = tmpData[0]; // 先存取表單名稱
                            Field = ""; // 清空欄位名稱
                            FieldD = ""; //要加入描述的欄位名稱
                            Field_Description = ""; //清空欄位描述
                        }

                        for (int i1 = 0; i1 < tmpData.Length; i1++)
                        {
                            tmpData[i1] = tmpData[i1].Trim();
                        }

                        if ((tmpData[4].ToLower().ToString() == "char" || tmpData[4].ToString() == "smalldatetime") && tmpData.Length == 7 && tmpData[6].Length != 0)
                        {
                            TextBox1.Text += "\r第 " + i.ToString() + " 筆資料 "+tmpData[0]+"\r欄位"+tmpData[2]+" 型態為 " + tmpData[4].ToString() + " 欄位 NUM_SCALE 應為 NULL \r\r";
                            tmpData[6] = "";
                        }
                        if (tmpData.Length==7 && tmpData[6].Length != 0)
                        {
                            Field += "[" + tmpData[2] + "] [" + tmpData[4].ToLower() + "] ( " + tmpData[5]+ " , " + tmpData[6] + " ) " + " :";
                            FieldD += tmpData[2] + ":";
                            Name_Description = tmpData[1];
                            Field_Description += tmpData[3] + ":";
                        }
                        else if (tmpData[4].Length != 0 && tmpData[5].ToString().Length != 0 && tmpData[4].ToString() != "smalldatetime")
                        {
                            Field += "[" + tmpData[2] + "] [" + tmpData[4].ToLower() + "] ( " + tmpData[5] + " ) " + " :";
                            FieldD += tmpData[2] + ":";
                            Name_Description = tmpData[1];
                            Field_Description += tmpData[3] + ":";
                            //TextBox1.Text += Field + "\r";
                        }
                        else if (tmpData[2].Length != 0 && tmpData[4].Length != 0)
                        {
                            Field += "[" + tmpData[2] + "] [" + tmpData[4].ToLower() + "] " + " :";
                            FieldD += tmpData[2] + ":";
                            Name_Description = tmpData[1];
                            Field_Description += tmpData[3] + ":";
                            //TextBox1.Text += " 2: " + tmpData.Length + "\r";
                        }


                        if (ReadFile.EndOfStream)
                        {
                            Field = Field.Remove(Field.LastIndexOf(':'));
                            CTC = AnalyticsData(CTC,n, Name, Name_Description, Field, FieldD, Field_Description);
                        }
                        #endregion
                        break;
                    default:
                        Print("錯誤，請聯絡管理員");
                        break;
                }
            }
            //關閉檔案 
            ReadFile.Close();

            if (n == 1)
                TextBox1.Text += "共建立 " + CTC.ToString() + " 個資料表\r";
        }
        else 
        {
            Print("檔案無資料.");
        }
        
    }

   
    //  !分析讀入的檔案  //
    public int AnalyticsData(int i,int n, string Name, string Name_Description, string Field, string FieldD, string Field_Description) 
    {
        string tablename = Name;

        if (n == 1)
        {
            if (!CheckTable(tablename)) // 資料表不存在時
            {
                i = Create_Table(i,Name, Name_Description, Field, FieldD, Field_Description);
            }
            else if (CB_Cover.Checked) // 資料表存在且勾選重建時
            {
                if (Drop_Table(tablename))
                {
                    i = Create_Table(i,Name, Name_Description, Field, FieldD, Field_Description);
                }
            }
            else // 資料表存在但未勾選重建時
            {
                TextBox1.Text += tablename + "已存在\r";
            }

            return i;
        }

        if (n == 2)
        {
            if (!CheckTable(tablename)) // 資料表不存在時
            {
                i = Create_Table(i, Name, Name_Description, Field, FieldD, Field_Description);
            }
            else // 資料表存在
            {
                i = Alter_Table(i, Name, Name_Description, Field, FieldD, Field_Description);
            }

            return i;
        }

        return 0;
    }
    
    // ~檢查資料表是否存在  //
    public bool CheckTable(string TableName) 
    {
        string strSQL = "select name from dbo.sysobjects where id = object_id(N'[dbo].[" + TableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1";
        SqlConnection SqlConn = new SqlConnection(Global.strcom);
        SqlCommand cmd = new SqlCommand(strSQL,SqlConn);
        SqlConn.Open();
        object dr = cmd.ExecuteScalar();

        if (dr == null)
        {
            SqlConn.Close();
            return false;
        }
        else
        {
            SqlConn.Close();
            return true;
        } 
    }

    //  @建立表單    //
    public int Create_Table(int i,string Name, string Name_Description, string Field, string FieldD, string Field_Description)
    {
        SqlConnection SqlConn = new SqlConnection(Global.strcom);
        SqlConn.Open();
        Field = Field.Replace(":", ",");

        #region ---/建立資料表/---
        try
        {
            

            string strSQL = "CREATE TABLE [dbo].[" + Name + "]( " + Field + " ) ";
            //TextBox1.Text += strSQL+ "\r\r"; //檢視輸出時的SQL內容
            SqlCommand cmd = new SqlCommand(strSQL, SqlConn);
            cmd.CommandText = strSQL;
            cmd.ExecuteNonQuery();
            Global.str = "建立 " + Name+" 資料表";
            Global.TbName += Name + ":";
            Print(Global.str);
            i++;
        }
        catch (SqlException ex)
        {
            TextBox1.Text += "建立資料表發生錯誤" + ex.Message+"\r";
            
        }
        #endregion

        #region ---/加入資料表敘述/---
        try
        {
            string SqlWTbValue = "if not exists(SELECT * FROM ::fn_listextendedproperty (NULL, 'user', 'dbo', 'table', N'" + Name +
                "', NULL, NULL)) begin   exec sp_addextendedproperty N'MS_Description', N'" + Name_Description +
                "', N'user', N'dbo', N'table', N'" + Name +
                "', NULL, NULL end else begin   exec sp_updateextendedproperty N'MS_Description', N'" + Name_Description +
                "', N'user', dbo, 'table', N'" + Name +
                "', NULL, NULL end";
            SqlCommand cmd = new SqlCommand(SqlWTbValue, SqlConn);
            cmd.CommandText = SqlWTbValue;
            cmd.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            TextBox1.Text = TextBox1.Text + "加入資料表敘述發生錯誤" + ex.Message;

        }
        #endregion

        #region ---/加入欄位敘述/---
        string[] tmpStrData1 = FieldD.Split(new char[] { ':' });
        string[] tmpStrData2 = Field_Description.Split(new char[] { ':' });

        for (int ai = 0; ai < tmpStrData1.Length - 1; ai++)
        {
            try
            {
                string SqlWTbCValue = "IF not exists(SELECT * FROM ::fn_listextendedproperty (NULL, 'user', 'dbo', 'table', '" + Name 
                    + "', 'column', '" + tmpStrData1[ai] + "')) BEGIN  exec sp_addextendedproperty 'MS_Description', '" + tmpStrData2[ai] 
                    + "', 'user', 'dbo', 'table', '" + Name + "', 'column', '" + tmpStrData1[ai] 
                    + "' END  ELSE BEGIN  exec sp_updateextendedproperty 'MS_Description', '" + tmpStrData2[ai] 
                    + "', 'user', 'dbo', 'table', '" + Name + "', 'column', '" + tmpStrData1[ai] + "' END";

                SqlCommand cmd = new SqlCommand(SqlWTbCValue, SqlConn);
                cmd.CommandText = SqlWTbCValue;
                //TextBox1.Text = SqlWTbCValue;
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                TextBox1.Text = TextBox1.Text + "加入欄位敘述發生錯誤" + ex.Message;

            }

        }
        #endregion


        SqlConn.Close();
        return i;
    }

    //  @刪除資料表  //
    public bool Drop_Table(string TableName)
    {
        SqlConnection SqlConn = new SqlConnection(Global.strcom);
        SqlConn.Open();
        string strSQL = "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + TableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[" + TableName + "] ";
        try
        {
            SqlCommand cmd = new SqlCommand(strSQL, SqlConn);
            cmd.CommandText = strSQL;
            cmd.ExecuteNonQuery();
            TextBox1.Text += "刪除 " + TableName + " 資料表\r";
            return true;
        }
        catch(SqlException ex)
        {
            Print(ex.Message);
            return false;
        }
        finally
        {
            SqlConn.Close();
        }
        
    }

    #region ---// 更新資料表區 //---
    
    //  @更新資料表   //
    public int Alter_Table(int i, string Name, string Name_Description, string Field, string FieldD, string Field_Description)
    {
        // 取出資料庫內資料表的內容
        String strSQL = "select * from information_schema.columns where table_name=@table_name";
        SqlConnection SqlConn = new SqlConnection(Global.strcom);
        SqlConn.Open();
        SqlCommand cmd = new SqlCommand(strSQL, SqlConn);
        cmd.Parameters.Add(new SqlParameter("@table_name", Name));
        string[] Fieldname = FieldD.TrimEnd(':').Split(new char[] { ':' });
        string[] Fielddescription = Field_Description.Split(new char[] { ':' });
        string[] Fielddata = Field.Split(new char[] { ':' });
        
        //-----------輸出資料表欄位內容至GridView----------//
        DataSet ds1 = new DataSet();
        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            da.Fill(ds1, "schema");
        }
        GridView1.DataSource = ds1.Tables["schema"];
        GridView1.DataBind();
        ds1.Dispose();
        //------------------------------------------------//


        for (int ai = 0; ai < Fieldname.Length; ai++)
        {
            int sw = 0; // 判斷是否有相同的資料
            SqlDataReader ds = cmd.ExecuteReader();

            while (ds.Read())
            {
                if (Fieldname[ai] == ds[3].ToString())
                {
                    string tmp1 = "";
                    string tmp2 = Fielddata[ai].Replace("[", "").Replace("]", "").Replace("(", "").Replace(")", "").Trim();

                    tmp1 += ds[3].ToString() + " ";
                    tmp1 += ds[7].ToString() + "  ";
                    if (ds[7].ToString() == "numeric" || ds[7].ToString() == "decimal")
                    {
                        tmp1 += ds[10].ToString() + " ";
                        if (ds[10].ToString().ToString().Length > 0)
                        {
                            tmp1 += ", "+ds[12].ToString() + " ";
                        }
                    }
                    else
                    {
                        tmp1 += ds[8].ToString();
                    }
                    tmp1 += ds[6].ToString().Replace("NO", "NOT NULL").Replace("YES", string.Empty);

                    if (tmp2.IndexOf("decimal") != -1 || tmp2.IndexOf("numeric") != -1)
                    {
                        if(tmp2.IndexOf(",") == -1)
                        {
                            tmp2 += " , 0";
                        }
                    }

                    tmp1 = tmp1.Trim();
                    tmp2 = tmp2.Trim();

                    //Print(tmp1);
                    //Print(tmp2);
                    //Print(tmp1.CompareTo(tmp2).ToString());
                    //Print(Fielddata[ai].Replace("[", string.Empty).Replace("]", string.Empty));
                    if (tmp1.CompareTo(tmp2) != 0)
                    {
                        //Print(Fielddata[ai].Replace("[", string.Empty).Replace("]", string.Empty));
                        Modify_Data(Name, Fielddata[ai].Replace("[", string.Empty).Replace("]", string.Empty));
                    }
                    sw = 1;
                }
            }
            if (sw == 0)
            {
                Print("新增資料");
                Print(Fieldname[ai]);
                Add_Data(Name, Fieldname[ai], Fielddata[ai], Fielddescription[ai]);
                
            }
            ds.Close();
        }


        SqlConn.Close();  

        return 0;
    }

    // 新增資料至資料表 //
    public bool Add_Data(string TableName,string FieldName,string Field,string FieldDescription)
    {
        SqlConnection SqlConn = new SqlConnection(Global.strcom);
        SqlConn.Open();
        string strSQL = "ALTER table "+ TableName +" add "+Field;
        try
        {
            SqlCommand cmd = new SqlCommand(strSQL, SqlConn);
            cmd.CommandText = strSQL;
            cmd.ExecuteNonQuery();
            TextBox1.Text += "新增 " +Field+" 至 "+ TableName + " 資料表\r";

            try
            {
                string SqlWTbCValue = "IF not exists(SELECT * FROM ::fn_listextendedproperty (NULL, 'user', 'dbo', 'table', '" + TableName
                    + "', 'column', '" + Field + "')) BEGIN  exec sp_addextendedproperty 'MS_Description', '" + FieldDescription
                    + "', 'user', 'dbo', 'table', '" + TableName + "', 'column', '" + FieldName
                    + "' END  ELSE BEGIN  exec sp_updateextendedproperty 'MS_Description', '" + FieldDescription
                    + "', 'user', 'dbo', 'table', '" + TableName + "', 'column', '" + FieldName + "' END";

                SqlCommand cmd2 = new SqlCommand(SqlWTbCValue, SqlConn);
                cmd2.CommandText = SqlWTbCValue;
                //TextBox1.Text = SqlWTbCValue;
                cmd2.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                TextBox1.Text = TextBox1.Text + "加入欄位敘述發生錯誤" + ex.Message+"\r";

            }
            return true;

        }
        catch (SqlException ex)
        {
            Print(ex.Message);
            return false;
        }
        finally
        {
            SqlConn.Close();
        }
    }
    // 修改資料至資料表 //
    public bool Modify_Data(string TableName, string Field)
    {
        SqlConnection SqlConn = new SqlConnection(Global.strcom);
        SqlConn.Open();
        string strSQL = "ALTER TABLE " + TableName + "  ALTER COLUMN " + Field;
        try
        {
            SqlCommand cmd = new SqlCommand(strSQL, SqlConn);
            cmd.CommandText = strSQL;
            cmd.ExecuteNonQuery();
            TextBox1.Text += "新增 " + Field + " 至 " + TableName + " 資料表\r";
            return true;
        }
        catch (SqlException ex)
        {
            Print(ex.Message);
            return false;
        }
        finally
        {
            SqlConn.Close();
        }
    }
    #endregion

    #region ---/ 復原資料表 /---

    #endregion

    #region ---/ 其他副程式 /---

    //  顯示 - 加入新的輸入字串
    public void Print(String Str)
    {
        TextBox1.Text = TextBox1.Text + Str + "\r";
    }

    

    #endregion

    //------ 測試使用
    protected void clearall_Click(object sender, EventArgs e)
    {
        Global.str = "";
        Global.strcom = "";
        TB_DataBase.Text = "";
        TB_FileName.Text = "";
        TB_Name.Text = "";
        TB_Password.Text = "";
        TB_Server.Text = "";
        TB_temp1.Text = "";
        TextBox1.Text = "";
        CB_Cover.Checked = false;
        CB_HDR.Checked = false;
        GridView1.DataSource = null;
        GridView1.DataBind();
    }
    protected void qset_Click(object sender, EventArgs e)
    {
        Global.str = "";
        Global.strcom = "";
        TB_DataBase.Text = "test";
        TB_FileName.Text = "ForCreateSample.csv";
        TB_Name.Text = "daniel";
        TB_Password.Text = "sa";
        TB_Server.Text = @"ATIAN\SQLEXPRESS";
        TB_temp1.Text = @"D:\ATien 的文件\Visual Studio 2005\WebSites\W_CreateTable_0810\ForCreateSample.csv";
        TextBox1.Text = "";
        CB_Cover.Checked = true;
        CB_HDR.Checked = true;
    }
}





