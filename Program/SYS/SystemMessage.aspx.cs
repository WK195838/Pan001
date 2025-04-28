using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PanPacificClass;

public partial class SYS_SystemMessage : System.Web.UI.Page
{    
    /// <summary>
    /// 中央處理器ID
    /// </summary>
    public string CpuID;
    /// <summary>
    /// 網卡位址
    /// </summary>
    public string MacAddress;
    /// <summary>
    /// 硬碟ID
    /// </summary>
    public string DiskID;
    /// <summary>
    /// IP位址
    /// </summary>
    public string IpAddress;
    /// <summary>
    /// OS登入帳號
    /// </summary>
    public string LoginUserName;
    /// <summary>
    /// 電腦名稱
    /// </summary>
    public string ComputerName;
    /// <summary>
    /// 電腦系統類型
    /// </summary>
    public string SystemType;
    /// <summary>
    /// 實體記憶體大小(單位：G)
    /// </summary>
    public string TotalPhysicalMemory;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        ComputerAuth CA = new ComputerAuth();
        ComputerName = CA.ComputerName;
        CpuID = CA.CpuID;
        DiskID = CA.DiskID;
        IpAddress = CA.IpAddress;
        LoginUserName = CA.LoginUserName;
        MacAddress = CA.MacAddress;
        SystemType = CA.SystemType;
        TotalPhysicalMemory = CA.TotalPhysicalMemory;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PanPacificClass.ComputerAuth CA = new PanPacificClass.ComputerAuth();
        if (CA.CheckComputerAuth() == false)
            Response.Write("系統授權檢核失敗<hr/>");

        Response.Write("<br>電腦版本資訊<hr/>");
        Response.Write(ComputerName);
        Response.Write("<hr/>");

        if (System.Web.HttpContext.Current.Application["CPUID"] != null)
            Response.Write(System.Web.HttpContext.Current.Application["CPUID"].ToString() + "<hr/>");
        else
            Response.Write("CpuID 取得失敗<hr/>");
        Response.Write(CpuID);
        Response.Write("<hr/>");

        if (System.Web.HttpContext.Current.Application["DiskID"] != null)
            Response.Write(System.Web.HttpContext.Current.Application["DiskID"].ToString() + "<hr/>");
        else
            Response.Write("DiskID 取得失敗<hr/>");
        Response.Write(DiskID);
        Response.Write("<hr/>");

        if (System.Web.HttpContext.Current.Application["IpAddress"] != null)
            Response.Write(System.Web.HttpContext.Current.Application["IpAddress"].ToString() + "<hr/>");
        else
            Response.Write("IpAddress 取得失敗<hr/>");
        Response.Write(IpAddress);
        Response.Write("<hr/>");

        if (System.Web.HttpContext.Current.Application["LoginUserName"] != null)
            Response.Write(System.Web.HttpContext.Current.Application["LoginUserName"].ToString() + "<hr/>");
        else
            Response.Write("LoginUserName 取得失敗<hr/>");
        Response.Write(LoginUserName);
        Response.Write("<hr/>");

        if (System.Web.HttpContext.Current.Application["MacAddress"] != null)
            Response.Write(System.Web.HttpContext.Current.Application["MacAddress"].ToString() + "<hr/>");
        else
            Response.Write("MacAddress 取得失敗<hr/>");
        Response.Write(MacAddress);
        Response.Write("<hr/>");

        if (System.Web.HttpContext.Current.Application["SystemType"] != null)
            Response.Write(System.Web.HttpContext.Current.Application["SystemType"].ToString() + "<hr/>");
        else
            Response.Write("SystemType 取得失敗<hr/>");
        Response.Write(SystemType);
        Response.Write("<hr/>");

        if (System.Web.HttpContext.Current.Application["TotalPhysicalMemory"] != null)
            Response.Write(System.Web.HttpContext.Current.Application["TotalPhysicalMemory"].ToString() + "<hr/>");
        else
            Response.Write("TotalPhysicalMemory 取得失敗<hr/>");
        Response.Write(TotalPhysicalMemory);
        Response.Write("<hr/>");
    }
}