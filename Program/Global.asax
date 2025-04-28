<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Security" %>
<%@ Import Namespace="System.Security.Principal" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // 應用程式啟動時執行的程式碼
        ///網站實體路徑(系統自動找出網頁所在位置)
        Application["rootPath"] = Server.MapPath("");
        ///網站實體根目錄
        Application["WebrootFolder"] = "ERP";
        ///設定DeBug資料夾路徑:可改
        ///A寫法:把LOG寫在發生錯誤的程式所在資料夾下
        Application["DeBugLog"] = Server.MapPath("").Remove(Server.MapPath("").LastIndexOf("\\")) + String.Format("\\DeBugLog\\");
        ///B寫法:把LOG寫在統一指定的資料夾下,注意設定資料夾讀取權限
        Application["DeBugLog"] = String.Format("C:\\DeBugLog\\");
        ///C寫法:把LOG寫在網站實體根目錄的指定資料夾下
        Application["DeBugLog"] = Server.MapPath("").Remove(Server.MapPath("").LastIndexOf(Application["WebrootFolder"].ToString()));
        Application["DeBugLog"] = Application["DeBugLog"].ToString().Remove(Application["DeBugLog"].ToString().LastIndexOf("\\")) + String.Format("\\DeBugLog\\");
        ///網路虛擬目錄:預設為實體目錄名稱,需同IIS虛擬目錄設定
        Application["WebSite"] = Application["WebrootFolder"];
        ///網站伺服器根目錄
        Application["Domain"] = "http://10.10.10.125/";
        ///開發環境描述
        Application["website_Desc"] = "【正式環境】供人事財務人員使用";
        ///網站標題
        Application["website_Title"] = "泛太資訊 EBOS ERP On Cloud 網站";
        ///預設之錯誤訊息頁面
        Application["ErrorPage"] = "/Pages/AppErrorMessage.aspx";
    }

    void Application_AuthenticateRequest(object sender, EventArgs e)
    {
        string cookieName = FormsAuthentication.FormsCookieName;
        HttpCookie authCookie = Context.Request.Cookies[cookieName];

        if (null == authCookie)
        {
            //There is no authentication cookie.
            return;
        }
        FormsAuthenticationTicket authTicket = null;
        try
        {
            authTicket = FormsAuthentication.Decrypt(authCookie.Value);
        }
        catch (Exception ex)
        {
            //Write the exception to the Event Log.
            return;
        }
        if (null == authTicket)
        {
            //Cookie failed to decrypt.
            return;
        }
        //When the ticket was created, the UserData property was assigned a
        //pipe-delimited string of group names.
        string[] groups = authTicket.UserData.Split(new char[] { '|' });
        //Create an Identity.
        GenericIdentity id = new GenericIdentity(authTicket.Name, "LdapAuthentication");
        //This principal flows throughout the request.
        GenericPrincipal principal = new GenericPrincipal(id, groups);
        Context.User = principal;
    }

    void Application_End(object sender, EventArgs e) 
    {
        //  應用程式關閉時執行的程式碼

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // 發生未處理錯誤時執行的程式碼
        String Message = "";
        Exception ex = Server.GetLastError();

        if (ex != null)
        {
            if ((ex.StackTrace.Trim().Contains("ServerUtility.Transfer") == true) && (ex.StackTrace.Trim().Contains("global_asax.Session_End") == true))
            {
                //連線逾時
            }
            else
            {
                Message = "發生錯誤的網頁:{0}錯誤訊息:{1}堆疊內容:{2}";
                try
                {
                    Message = String.Format(Message, Request.Path + System.Environment.NewLine, ex.GetBaseException().Message + System.Environment.NewLine, System.Environment.NewLine + ex.StackTrace);

                    if (Request.Path.Contains("Main.aspx") || Request.Path.Contains("Welcome.aspx"))
                    {
                        Response.Write("連線逾時");
                    }
                    else if (Request.Path.Contains("space.gif") && ex.GetBaseException().Message.Contains("檔案不存在"))
                    {
                        //什麼都不用做.懶得去查這個圖檔為何老是找錯路徑了
                    }
                    else if (ex.GetBaseException().Message.Contains("並未將物件參考設定為物件的執行個體"))
                    {
                        Response.Redirect("/" + Application["WebSite"] + Application["ErrorPage"] + "?ErrMsg=TimeOut");
                        //寫入文字檔
                        WriteToLogs(Message);
                    }
                    else
                    {
                        //寫入文字檔
                        //System.IO.File.AppendAllText(Application("rootPath") & String.Format("Log\\{0}.txt", DateTime.Now.ToString("yyyyMMdd_HHmmss")), Message);

                        Response.Write("系統錯誤,請聯絡系統管理員!!" + System.Environment.NewLine + Message);
                        WriteToLogs("App_Error : " + sender.ToString() + e.ToString() + " at " + DateTime.Now.ToString() + "\r\n" + Message);
                    }
                }
                catch (Exception ex2)
                {
                    WriteToLogs("App_Error : " + sender.ToString() + e.ToString() + " at " + DateTime.Now.ToString() + "\r\n" + ex.GetBaseException().Message + "\r\n" + ex.Message);
                }
            }
            //清除Error  
            Server.ClearError();
        }
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // 啟動新工作階段時執行的程式碼

    }

    void Session_End(object sender, EventArgs e) 
    {
        // 工作階段結束時執行的程式碼。 
        // 注意: 只有在 Web.config 檔將 sessionstate 模式設定為 InProc 時，
        // 才會引發 Session_End 事件。如果將工作階段模式設定為 StateServer 
        // 或 SQLServer，就不會引發這個事件。
        try
        {
            Server.Transfer("/" + Application["WebSite"] + Application["ErrorPage"] + "?ErrMsg=TimeOut");
        }
        catch { }
    }

    void WriteToLogs(String Msg)
    {
        String path = Application["DeBugLog"].ToString() + String.Format("Log_{0}.txt", DateTime.Now.ToString("yyyyMMdd_HHmmss"));
        try
        {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(path);
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
                //fileInfo = new System.IO.FileInfo("C:\\" + String.Format("Log_{0}.txt", DateTime.Now.ToString("yyyyMMdd_HHmmss")));
            }

            System.IO.StreamWriter sw = fileInfo.AppendText();
            sw.WriteLine(Msg);
            sw.Flush();
            sw.Close();
        }
        catch { }
    }
</script>
