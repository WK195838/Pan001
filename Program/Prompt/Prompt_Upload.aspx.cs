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

public partial class Prompt_Upload : System.Web.UI.Page
{
    SysSetting SysSet = new SysSetting();
    string theFileKind, theSavePath, theFileName, theobjValue;
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 取得參數
        if (Request["theFileKind"] != null)
        {
            theFileKind = Request["theFileKind"].ToString();
        }

        if (Request["theSavePath"] != null)
        {
            theSavePath = Request["theSavePath"].ToString();
        }

        if (Request["theFileName"] != null)
        {
            theFileName = Request["theFileName"].ToString();
        }


        if (Request["objValue"] != null)
        {
            theobjValue = Request["objValue"].ToString();
        }
        #endregion
    }

    protected void btUpload_Click(object sender, EventArgs e)
    {
        string theSaveName = SysSet.rtnTrans(theSavePath.Replace('＼', '\\')) + theFileName;
        string theSaveStyle = "";
        try
        {
            if (fu_Picture.HasFile)
            {
                String fileName1 = fu_Picture.FileName;
                string FileExtension = System.IO.Path.GetExtension(fileName1).ToLower();
                if (fileName1.Contains(" ") == true)
                {
                    lbl_Msg.Text = "上傳檔名不得有特殊字元";
                }
                else if (FileExtension.Equals(".exe") || FileExtension.Equals(".bat"))
                {
                    lbl_Msg.Text = "不得上傳執行檔";
                }
                else
                {
                    theSaveStyle = fileName1;
                    string oldFileStyle = "";
                    if (theFileKind.Equals("picture"))
                    {
                        theSaveStyle = fileName1.Substring(fileName1.LastIndexOf('.'));
                        UserInfo _UserInfo = new UserInfo();
                        oldFileStyle = _UserInfo.SysSet.chekPic(theSaveName);
                        if (string.IsNullOrEmpty(oldFileStyle))
                            oldFileStyle = theSaveStyle;
                    }
                    else if (theFileKind.Equals("ReCover"))
                    {
                        theSaveStyle = "";                        
                    }

                    if (System.IO.File.Exists(theSaveName + oldFileStyle))
                    {
                        try
                        {
                            System.IO.FileInfo f = new System.IO.FileInfo(theSaveName + oldFileStyle);
                            f.Attributes = System.IO.FileAttributes.Normal;
                            System.IO.File.Delete(theSaveName + oldFileStyle);
                        }
                        catch { }
                    }
                    fu_Picture.SaveAs(theSaveName + theSaveStyle);
                    lbl_Msg.Text = "上傳成功";

                    string strScript = "<script Language=JavaScript>window.returnValue = '" + theSaveStyle + ":';";
                    if (theFileKind.Equals("picture"))
                    {//重新整理父視窗,在要顯示上傳資料或圖片時使用
                        //因為改用showModalDialog避免POSTBACK,所以下列這段就不能用了
                        //strScript += "opener.location.reload();";
                    }
                    else if (theFileKind.Equals("ReCover"))
                    {
                        //因為是覆寫指定檔案,所以不用回傳檔名
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(theobjValue))
                            strScript += "opener.document.form1." + theobjValue.Replace("_", "$") + ".value='" + theSaveStyle + "';";
                        //strScript += "alert('" + theobjValue + "');";
                        //strScript += "alert(opener.document.form1." + theobjValue.Replace("_","$") + ".value);";
                    }
                    strScript += "window.close();</script>";
                    this.ClientScript.RegisterStartupScript(this.GetType(), "window.close", strScript);
                }
            }
        }
        catch (Exception ex)
        {
            lbl_Msg.Text = ex.Message;
        }
    }


}
