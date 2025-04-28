using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace GeneralModalPopup
{
    public partial class Ajax 
    {
        public class ModalPopup
        {
           /// <summary>
            /// 建立載入中視窗
           /// </summary>
            /// <param name="PopupControl">要做為載入中視窗的Panel</param>
           /// <param name="LEFT">距離左邊px</param>
           /// <param name="TOP">距離頂端PX</param>
            public static void RegisterLoadingWebButton(Panel PopupControl,int LEFT,int TOP)
            {
                Panel panel = new Panel();
                panel.ID = PopupControl.ID + "_panel";
                panel.Style["display"] = "none";
                panel.Style["position"] = "absolute";
                panel.Style["z-index"] = "141";
                panel.Style["LEFT"] = "0px";
                panel.Style["TOP"] = "0px";
                panel.Width = Unit.Pixel(1280);
                panel.Height = Unit.Pixel(1024);
                panel.Style["border"] = "1px solid #2F4F4F";
                panel.Style["color"] = "#ffffff";
                panel.Style["background-color"] = "#000000";
                panel.Style["font-weight"] = "bold";
                panel.Style["cursor"] = "not-allowed";
                panel.Style["filter"] = "Alpha(opacity=30)";
                //PopupControl.Page.Form.Controls.Add(panel);
               // Content ctn = (Content)PopupControl.Page.Master.FindControl("Content1");
                PopupControl.Style["display"] = "none";
                PopupControl.Style["position"] = "absolute";
                PopupControl.Style["z-index"] = "200";
                PopupControl.Style["LEFT"] = LEFT.ToString()+"px";
                PopupControl.Style["TOP"] = TOP.ToString()+"px";                
                PopupControl.Page.Controls.Add(panel);                
           


                string js = "$get('" + panel.ClientID + "').style.display='block';";
                js += "$get('" + PopupControl.ClientID + "').style.display='block';";
                PopupControl.Page.ClientScript.RegisterOnSubmitStatement(PopupControl.GetType(), "_RegisterLoadingWebButton", js);
            }

            /// <summary>
            /// 建立載入中視窗
            /// </summary>
            /// <param name="PopupControl">要做為載入中視窗的Panel</param>
            /// <remarks></remarks>
            public static void RegisterLoadingWebButton(Panel PopupControl)
            {
               RegisterLoadingWebButton(PopupControl, 350, 230);

            }






        //    /// <summary>
        //    /// 以ModalPopupExtender建立MsgBox
        //    /// </summary>
        //    /// <remarks></remarks>
        //    public class MsgBox {
        //        //建立一個事件
        //        /// <summary>
        //        /// 當 MsgBox 顯示完畢之後,要觸發的後端事件
        //        /// </summary>
        //        /// <param name="ReturnValue">確定或取消(True 或 Flase)</param>
        //        /// <remarks></remarks>
        //        event OnMsgBox(string ReturnValue);

        //        //建立虛擬欄位
        //        HiddenField HiddenField1 = new HiddenField();
        //        AjaxControlToolkit.ModalPopupExtender ModalPopupExtender1 = new AjaxControlToolkit.ModalPopupExtender();

        //        Panel MsgBoxPanel;
        //        Button OkButton;
        //        Button CancelButton;
        //        Label MsgLabel;
        //        Label TitleLabel;
                
        //        /// <summary>
        //        /// 顯示MsgBox
        //        /// </summary>
        //        /// <param name="msg">要顯示的訊息</param>
        //        /// <param name="title">視窗抬頭</param>
        //        /// <param name="ModalBackgroundStyle">背景CSS</param>
        //        /// <param name="x">視窗X位置</param>
        //        /// <param name="y">視窗Y位置</param>
        //        /// <remarks></remarks>
        //        Sub Show(ByVal msg As String, Optional ByVal title As String = "詢問視窗", Optional ByVal ModalBackgroundStyle As String = "", Optional ByVal x As Integer = 100, Optional ByVal y As Integer = 100)
        //            Dim js As String
        //            'OKScript
        //            js = "<script>"
        //            js += "function _onMsgBoxOK(v){"
        //            js += "$get('" & Me.HiddenField1.ClientID & "').value=v;"
        //            '模擬Postback
        //            js += MsgBoxPanel.Page.ClientScript.GetPostBackEventReference(MsgBoxPanel.Page.FindControl(Me.ModalPopupExtender1.TargetControlID), "")
        //            js += "}"
        //            js += "</script>"
        //            MsgBoxPanel.Page.ClientScript.RegisterStartupScript(GetType(String), "showModalWindow_MsgBox", js)
        //            'Show Dialog
        //            Me.ModalPopupExtender1.TargetControlID = Me.HiddenField1.ClientID
        //            Me.ModalPopupExtender1.PopupControlID = MsgBoxPanel.ID
        //            Me.ModalPopupExtender1.BackgroundCssClass = ModalBackgroundStyle
        //            Me.ModalPopupExtender1.DropShadow = True
        //            Me.ModalPopupExtender1.OkControlID = OkButton.ID
        //            Me.ModalPopupExtender1.OnOkScript = "_onMsgBoxOK(true)"
        //            Me.ModalPopupExtender1.OnCancelScript = "_onMsgBoxOK(false)"
        //            Me.ModalPopupExtender1.CancelControlID = CancelButton.ID
        //            Me.ModalPopupExtender1.PopupDragHandleControlID = TitleLabel.ID
        //            Me.ModalPopupExtender1.X = x
        //            Me.ModalPopupExtender1.Y = y
        //            Me.ModalPopupExtender1.OkControlID = OkButton.ID
        //            Me.MsgLabel.Text = msg
        //            Me.TitleLabel.Text = title
        //            '顯示對話視窗
        //            Me.ModalPopupExtender1.Show()
        //        }

        //        /// <summary>
        //        /// 初始化程式(請撰寫於Page_Load)
        //        /// </summary>
        //        /// <param name="MsgBoxPanel">做為MsgBox的主要視窗面板</param>
        //        /// <param name="MsgLabel">顯示訊息的Label</param>
        //        /// <param name="TitleLabel">顯示視窗抬頭的Label</param>
        //        /// <param name="OkButton">OK按鈕</param>
        //        /// <param name="CancelButton">Cancel按鈕</param>
        //        /// <remarks></remarks>
        //        public static Initial(Panel MsgBoxPanel, Label MsgLabel, Label TitleLabel, Button OkButton, Button CancelButton) {
        //            //建立隱藏欄位
        //            ModalPopupExtender1.ID = MsgBoxPanel.ID & "_ModalPopupExtender1";
        //            HiddenField1.ID = MsgBoxPanel.ID & "_HiddenField1";

        //            MsgBoxPanel.Parent.Controls.Add(HiddenField1);
        //            MsgBoxPanel.Parent.Controls.Add(ModalPopupExtender1);

        //            //設定Extender Control
        //            this.ModalPopupExtender1.TargetControlID = this.HiddenField1.ClientID;
        //            this.ModalPopupExtender1.PopupControlID = MsgBoxPanel.ID

        //            //保留
        //            this.MsgBoxPanel = MsgBoxPanel;
        //            this.MsgLabel = MsgLabel;
        //            this.TitleLabel = TitleLabel;
        //            this.OkButton = OkButton;
        //            this.CancelButton = CancelButton;

        //            //檢查是否有Postback
        //            if (MsgBoxPanel.Page.Request.Form("__EVENTTARGET") = this.HiddenField1.ClientID)
        //                 RaiseEvent OnMsgBox(MsgBoxPanel.Page.Request.Form(this.HiddenField1.ClientID));
        //        }

        //    }
        }
    }
}