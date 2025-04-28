using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Drawing;

public partial class MonthStar : System.Web.UI.Page
{
    UserInfo _UserInfo = new UserInfo();
    protected void Page_Load(object sender, EventArgs e)
    {
        string strScript = "";
        strScript = "<script Language=JavaScript>";
        #region 依視窗大小設定背景大小(需更新畫面,且若超過圖片大小時會留白)
        strScript += "" +
            "document.getElementById(\'" + divStar.ClientID + "\').style.height=(document.documentElement.clientHeight - 160);" +
            "document.getElementById(\'" + divStar.ClientID + "\').style.width=(document.documentElement.clientWidth - 30);" +
            "document.getElementById(\'divBG\').style.height=(document.documentElement.clientHeight - 160);" +
            "document.getElementById(\'divBG\').style.width=(document.documentElement.clientWidth - 30);" +
            "";
        #endregion
        strScript += " </script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "load1", strScript);
        //MonthList1.Visible = false;
        if (!Page.IsPostBack)
        {
            MonthList1.SetSpecialList("", "本日星座");
            ShowStar("");
            SetEmailFrom();
        }
        else
        {
            if (MonthList1.SelectedMonth > 0)
                ShowStar(MonthList1.SelectedMonth.ToString().PadLeft(2, '0') + "01");
            else
                ShowStar("");
        }        
    }

    private void SetEmailFrom()
    {
        string UserName = "", UserEmail = "";
        DBSetting.PersonalData PD = DBSetting.PersonalDateList(_UserInfo.UData.Company, _UserInfo.UData.EmployeeId);

        if (!string.IsNullOrEmpty(PD.EnglishName))
            UserName = PD.EnglishName.Trim();
        else if (!string.IsNullOrEmpty(PD.EmployeeName))
            UserName = PD.EmployeeName.Trim();
        else
            UserName = _UserInfo.UData.UserName.Trim();
        if (!string.IsNullOrEmpty(PD.Email))
            UserEmail = PD.Email.Trim();

        txtMailFromName.Text = System.Web.HttpUtility.HtmlEncode(UserName);
        txtMailFrom.Text = System.Web.HttpUtility.HtmlEncode(((UserEmail.Length > 0) ? UserEmail : "panservice@pan-pacific.com.tw"));
        txtSub.Text = "生日快樂!";
        txtContext.Text = "生日快樂!\r\n　　By " + System.Web.HttpUtility.HtmlEncode(UserName);        
    }

    protected void ShowStar(string strMMdd)
    {
        bool isDefBGJPG = false;
        string strBGJPG = "";
        divStar.InnerHtml = "";
        if (string.IsNullOrEmpty(strMMdd))
            strMMdd = DateTime.Today.ToString("MMdd");
        //設定背景大小
        //divStar.Style.Add("width", "980px");
        //divStar.Style.Add("height", "458px");
        //取得星座背景圖
        strBGJPG = DBSetting.GetStarData(strMMdd, "Background");
        divStar.Style.Add("background-image", "url(Image/Star/" + strBGJPG + ")");
        divStar.Style.Add("background-repeat", "no-repeat");
        divStar.Style.Add("position", "absolute");
                
        string StarStartDate = "", StarEndDate = "",sQuery="";
        StarStartDate = DBSetting.GetStarData(strMMdd, "DateStart");
        StarEndDate = DBSetting.GetStarData(strMMdd, "DateEnd");
        string strStarWord = DBSetting.GetStarData(strMMdd, "Name") + "壽星";
        if (strBGJPG.ToLower().Equals(StarStartDate.PadLeft(4, '0') + "-" + StarEndDate.PadLeft(4, '0') + ".jpg"))
        {
            isDefBGJPG = true;
        }
        
        if (StarStartDate.Length > StarEndDate.Length)
            sQuery = " And ( (DatePart(month,[BirthDate])*100+DatePart(day,[BirthDate])) > " + StarStartDate +
                " OR (DatePart(month,[BirthDate])*100+DatePart(day,[BirthDate])) < " + StarEndDate + ")";
        else
            sQuery = " And (DatePart(month,[BirthDate])*100+DatePart(day,[BirthDate])) Between " + StarStartDate + " And " + StarEndDate;
        DataTable DT = DBSetting.DTList("Personnel_Master", "EmployeeId", "EmployeeName+'|'+IsNull(EnglishName,'')+'|'+Convert(char(2),DatePart(month,[BirthDate]))+'|'+Convert(char(2),DatePart(day,[BirthDate]))+'|'+IsNull(Email,'')", " Company='" + _UserInfo.UData.Company + "' " + sQuery);
        if (DT != null)
        {
            if (DT.Rows.Count > 0)
            {
                string html = "", htmlEnd = "";
                int iTop = 0, iRight = 0, iHeight = 0, iWeight = 0, iSumTop = 0, iSumLeft = 0;
                bool blCanEmail = false;
                string strShow = "";
                string ImgfilePath = "";
                Random r = new Random();
                html = "<ul>";
                divStar.InnerHtml += html;
                
                //星座祝賀詞
                html = "<li style='list-style-type:none;'>" +
                    "<p style='font-family:" + r.Next(20) + ",fantasy,sans-serif; font-style:italic;height:50px;line-height:50px; font-weight:bold; color: Red; font-size:25pt;position:absolute;top:20px;left:10px'>" +
                    strStarWord +
                    "</p></li>";
                if (isDefBGJPG.Equals(true))//使用系統預設圖檔時,顯示星座祝賀詞
                    divStar.InnerHtml += html;

                for (int i = 0; i < 2; i++)
                {
                    iTop = r.Next(50) + i * 350;
                    iRight = 80 + r.Next(260);
                    strShow = "HB" + (i + 1).ToString().PadLeft(2, '0') + ".gif";
                    html = "<li style='list-style-type:none;'><div style='position:absolute;top:" + iTop + "px;left:" + iRight + "px;height:100px;'>";
                    html += "<img alt='' src='Image/Star/" + strShow + "' style='vertical-align:top" + (strStarWord.Contains("摩羯") ? ";display:none" : "") + ";' />";
                    html += "</div></li>";
                    divStar.InnerHtml += html;
                }
                html = "</ul>";
                divStar.InnerHtml += html;
                html = "<ul style='float:right;width:90%;'>";
                divStar.InnerHtml += html;

                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    blCanEmail = false;
                    //取出照片檔實際大小
                    ImgfilePath = "Main/" + _UserInfo.SysSet.GetConfigString("picture") + "/Pic_" + _UserInfo.UData.Company + "_" + DT.Rows[i][0].ToString().Trim() + ".jpg";
                    Bitmap tempImg = GetImg(ref ImgfilePath);                    
                    //設定照片高度以順排版
                    iHeight = 138;
                    //設定每張照片X軸定位
                    iSumLeft = ((i % 5) > 0 ? iSumLeft : 0) + iWeight + 40;
                    //設定照片Y軸定位基準
                    iSumTop = iHeight + 35;
                    //依比例算出應顯示之寬度                        
                    iWeight = (tempImg.Width * iHeight) / tempImg.Height;
                    tempImg.Dispose();

                    iTop = ((DT.Rows.Count < 6) ? 200 : 100) + ((i + 0) / 5 * iSumTop);
                    iRight = r.Next(10) + iSumLeft;
                    string[] strPersonal = DT.Rows[i][1].ToString().Split('|');
                    strShow = (strPersonal[1].Trim().Length > 0 ? strPersonal[1].Trim() : strPersonal[0].Trim());
                    strShow += " " + strPersonal[2].Trim() + "." + strPersonal[3].Trim();
                    html = "";
                    html = "<li style='float:left;list-style-type:none'><div id='test" + i.ToString() + "' style='float:left;position:absolute;Top:" + iTop + "px;left:" + iRight + "px;'>";
                    if (strPersonal[4].Trim().Length > 0)
                    {//系統沒有該人員E-MAIL時,不提供留言功能
                        blCanEmail = true;
                        //"<a href='mailto:" + strPersonal[4].Trim() + "?subject=" + strShow + "%A5%CD%A4%E9%A7%D6%BC%D6' alt='留言給壽星'>" +
                        html += "<a href='#' alt='留言給壽星' onClick='javascript:document.all." + divEMailBG.ClientID + ".style.display=\"inline\";" +
                            "document.all." + divEMail.ClientID + ".style.top=\"250px\";" +
                            "document.all." + divEMail.ClientID + ".style.right=\"250px\";" +
                            //下列限IE適用:訊息視窗出現在滑鼠位置
                            //"document.all." + divEMail.ClientID + ".style.top=event.clientY + document.body.scrollTop;" +
                            //"document.all." + divEMail.ClientID + ".style.right=1024- event.clientX + document.body.scrollLeft;" +
                            "document.all." + hfMailto.ClientID + ".value=\"" + strPersonal[4].Trim() + "\";" +
                            "document.all." + hfStarName.ClientID + ".value=\"" + strShow + "\";" +
                            "'>";
                    }
                    html += "<img alt='" + strShow + "' src='" + ImgfilePath + "' style='vertical-align:top;width:" + iWeight.ToString() + "px; height:" + iHeight.ToString() + "px;' id='Rotate" + i.ToString() + "' />";
                    if (blCanEmail.Equals(true))
                    {//系統沒有該人員E-MAIL時,不提供留言功能
                        html += "</a>";
                    }
                    //html += "<p><img id=\"iRotate" + i.ToString() + "\" alt=\"" + DT.Rows[i][1].ToString().Trim() +
                    //    "\" src=\"Main/PersonalPicture/Pic_" + _UserInfo.UData.Company + "_" + DT.Rows[i][0].ToString().Trim() + ".jpg\" /></p>";
                    //為方便管理,將自訂義之Javascript一起放最後面
                    htmlEnd += " $(document).ready(function() {	 ";
                    //固定:初始化旋轉
                    //htmlEnd += " $('#Rotate" + i.ToString() + "').rotate({angle:" + ((i % 2 == 0) ? "-" : "") + "5});  ";                    
                    //滑鼠動態:iPad上Apple瀏覽器不適用(無滑鼠事件)
                    htmlEnd += " $('#Rotate" + i.ToString() + "').rotate({angle:" + ((i % 2 == 0) ? "-" : "") + "5,maxAngle:5,minAngle:-5," +
                        " bind:" +
                        " [" +
                        " {\"mouseover\":function(){$(this).rotateAnimation(" + ((i % 2 == 0) ? "-" : "") + "5);}}," +
                        " {\"mouseout\":function(){$(this).rotateAnimation(" + ((i % 2 == 0) ? "" : "-") + "5);}}" +
                        " ]" +
                        " });";
                    htmlEnd += " });";
                    html += "<p style='font-family:" + r.Next(20) + ",fantasy,sans-serif; font-style:italic; font-weight:bold; color: Red; text-align:center;background:#FFF;'>" + strShow + "</p></div></li>";
                    if ((i + 1) % 5 == 0)
                    {
                        html += "</ul><ul style='float:right;width:90%;'>";
                    }

                    //if ((i + 1) % 3 == 0)
                    //    html += "</ul><div style='clear:both'></div><ul style='float:right;width:550px'>";
                    divStar.InnerHtml += html;
                }
                html = "</ul>";
                divStar.InnerHtml += html;
                divStar.InnerHtml += "<script type=\"text/javascript\">";
                divStar.InnerHtml += htmlEnd;
                divStar.InnerHtml += "</script>";
            }
        }
    }

    private void Send()
    {
        MailMessage mail = new MailMessage();
        string MailfromName = "泛太服務信箱代發郵件", MailfromEmail = "panservice@pan-pacific.com.tw";
        string Subject = hfStarName.Value + "生日快樂!";
        if (txtMailFromName.Text.Trim().Length > 0)
        {
            MailfromName = txtMailFromName.Text.Trim();
        }

        if (txtMailFrom.Text.Trim().Length > 0)
        {
            string[] MailtoList = txtMailFrom.Text.Trim().Split(new char[] { '<', '/', '>', ',', ';' });
            for (int i = 0; i < MailtoList.Length; i++)
            {
                if (!string.IsNullOrEmpty(MailtoList[i]))
                {
                    if (MailtoList[i].Contains("@"))
                        MailfromEmail = MailtoList[i];
                    else if ((i == 0) && (MailfromName.Length == 0))
                        MailfromName = MailtoList[i];
                    else
                        MailfromName += "&" + MailtoList[i];
                }
            }
        }
        mail.From = new MailAddress(MailfromEmail, MailfromName);
        mail.To.Add(new MailAddress(hfMailto.Value, hfStarName.Value));
        if (!string.IsNullOrEmpty(txtSub.Text.Trim()))
        {
            Subject = txtSub.Text.Trim();
        }
        mail.Subject = Subject;
        if (!string.IsNullOrEmpty(txtContext.Text.Trim()))
        {
            Subject = txtContext.Text.Trim();
        }
        mail.Body = Subject;
        mail.IsBodyHtml = false;
        mail.Priority = MailPriority.High;
        SmtpClient smtp = new SmtpClient("10.10.10.2");
        smtp.Send(mail);
        divEMail.Attributes.Add("display", "none");        
    }

    protected void SentMail_Click(object sender, EventArgs e)
    {
        if (txtSub.Text.Trim().Length == 0 || txtContext.Text.Trim().Length == 0)
        {
            divMsg.Style.Remove("display");
            divMsg.Style.Add("display", "inline");
        }
        txtMailFrom.Text = System.Web.HttpUtility.HtmlDecode(txtMailFrom.Text).Trim().Replace("<", ",").Replace(">", ",");
        Send();
        //divMsg.Attributes.Add("display", "none");
    }
    protected void Clear_Click(object sender, EventArgs e)
    {
        txtSub.Text = "";
        txtContext.Text = "";
        txtMailFrom.Text = "";        
        divEMail.Attributes.Add("display", "none");
        divMsg.Style.Remove("display");
        divMsg.Style.Add("display", "none");
    }

    private Bitmap GetImg(ref string filePath)
    {
        string serverfilePath = Server.MapPath(filePath);
        if (!System.IO.File.Exists(serverfilePath))
        {            
            filePath = "Main/" + _UserInfo.SysSet.GetConfigString("picture") + "/picture.jpg";
            serverfilePath = Server.MapPath(filePath);
        }        
        return new Bitmap(serverfilePath);
    }
}