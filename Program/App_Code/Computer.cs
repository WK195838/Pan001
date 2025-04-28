using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Computer 計算機
/// </summary>
public static class Computer
{
    static string MsgBox;

    /// <summary>
    /// 計算機
    /// </summary>
    /// <param name="strFormula">計算式</param>
    /// <param name="retMsg">計算過程或錯誤訊息</param>
    /// <returns>計算結果</returns>
    public static decimal ComputerFormula(string strFormula, out string retMsg)
    {
        decimal reCalculator = 0;
        reCalculator = Calculator(strFormula);
        retMsg = MsgBox;
        return reCalculator;
    }

    //計算機
    private static decimal Calculator(string str)
    {
        //  第一步    加大括弧
        str = "{" + str + "}";
        MsgBox += "1. 加大括弧 ：" + str + "\r\n";

        //  第二步    判斷有無錯誤
        MsgBox += "2.檢查運算式：";
        switch (CKStr(str))
        {
            case -1:
                MsgBox += "運算符號有錯誤!\r\n";
                return 0;
            case 0:
                MsgBox += "運算式無誤\r\n";
                break;
            case 1:
                MsgBox += "括弧數量有誤!\r\n";
                return 0;
            case 2:
                MsgBox += "數值不正確，可能是小數點有錯誤\r\n";
                return 0;
        }

        //  第三步    整理運算式
        MsgBox += "3. 整理運算式：\r\n" + str + "\r\n to \r\n";
        str = Format(str);
        MsgBox += str + "\r\n";
        //  第四步    消化最小範圍的括弧，一直到沒有括弧
        MsgBox += "4. 找出最小範圍的括弧處理，直到沒有括弧為止：\r\n";
        int n = 0;
        while (n == 0)
        {
            n = -1;
            if (str.Contains("("))
            {
                n = 0;
                //找最小括弧的左括弧
                int ltmp = str.Remove(str.IndexOf(")")).LastIndexOf("(");
                string tmp = str.Substring(ltmp, str.IndexOf(")") - ltmp + 1);
                MsgBox += "－－－－－－－－－－－－－－－－－－－－\r\n";
                MsgBox += "處理： " + tmp + "\r\n";
                str = str.Replace(tmp, Calculation(tmp));
                MsgBox += "處理後： " + str + "\r\n";
                str = Format(str);
            }
        }
        str = str.Replace("+0", "");
        decimal dec = 0;
        decimal.TryParse(str, out dec);

        return dec;
    }
    //判斷運算式是否有錯誤
    private static int CKStr(string str)
    {
        //不可出現的組合
        string[] tmp ={ "+*", "-*", "+/", "-/", "**", "*/", "/*", "//", "(*", "(/", "+)", "-)", "*)", "/)", ")(", "=", ".." };
        for (int i = 0; i < tmp.Length; i++)
        {
            if (str.Contains(tmp[i]))
            {
                MsgBox = "運算式中出現：　" + tmp[i] + " 請修正\r\n";
                return -1;
            }
        }

        string[] num ={ "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        for (int i = 0; i < num.Length; i++)
        {
            if (str.Contains(")" + num[i]))
            {
                MsgBox = "運算式中出現：　\")" + num[i] + "\" 請修正，\")\"後必須接運算符號\r\n";
                return -1;
            }
            else if (str.Contains(num[i] + "("))
            {
                MsgBox = "運算式中出現：　\"" + num[i] + "(\" 請修正，\"(\"前必須接運算符號\r\n";
                return -1;
            }
        }

        string[] tmp2 = { "{/", "{*", "/}", "*}", "-}", "+}" };
        for (int i = 0; i < tmp2.Length; i++)
        {
            if (str.Contains(tmp2[i]))
            {
                if (i < 2)
                {
                    MsgBox = "運算式開頭出現：　\"" + tmp2[i].Replace("{", "") + " \"請修正\r\n";
                }
                else
                {
                    MsgBox = "運算式尾端出現：　\"" + tmp2[i].Replace("}", "") + "\" 請修正\r\n";
                }

                return -1;
            }
        }
        //檢查小數點
        string[] op = { "+", "-", "*", "/", "(", ")", "{", "}" };
        string tmpstr = str;
        for (int i = 0; i < op.Length; i++)
        {
            tmpstr = tmpstr.Replace(op[i], ",");
        }
        while (tmpstr.Contains(",,"))
        {
            tmpstr = tmpstr.Replace(",,", ",");
        }
        string[] words = tmpstr.Split(new char[] { ',' });

        decimal dout = 0;
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length > 0)
            {
                if (!decimal.TryParse(words[i], out dout))
                {
                    MsgBox = "錯誤， \"" + words[i] + "\" 此數值有誤請修正\r\n";
                    return 2;
                }
            }
        }

        MsgBox += " \r\n";


        //計算左右括弧是否正確
        int L = 0, R = 0;

        for (int i = 0; i < str.Length; i++)
        {
            if (str.Substring(i, 1) == "(")
                L++;
            if (str.Substring(i, 1) == ")")
                R++;
        }

        if (L == R)
            return 0;
        else if (L > R)
        {
            MsgBox = "錯誤，缺少" + ((int)(L - R)).ToString() + " 個右括弧\r\n";
            return 1;
        }
        else
        {
            MsgBox = "錯誤，缺少" + ((int)(R - L)).ToString() + " 個左括弧\r\n";
            return 1;
        }
    }

    private static string Format(string str)
    {
        int n = 0;
        string[] tmp1 ={ "++", "+-", "-+", "--", "*+", "*-", "/+", "/-", "{", "}", "()" };
        string[] tmp2 ={ "+", "-", "-", "-", "*", "*#", "/", "/#", "(", ")", "" };
        while (n == 0)
        {
            n = -1;
            for (int i = 0; i < tmp1.Length; i++)
            {
                if (str.Contains(tmp1[i]))
                {
                    str = str.Replace(tmp1[i], tmp2[i]);
                    n = 0;
                }
            }
        }
        str = str.Replace("+0", "").Replace("-", "+0-");
        return str;
    }

    //計算沒有括弧的內容
    private static string Calculation(string str)
    {
        //  去除括弧
        if (str.Contains("("))
        {
            str = str.Replace("(", "").Replace(")", "");
            MsgBox += "(1) 去除括弧： " + str + "\r\n";
        }
        //  切割成更小的運算式
        MsgBox += "(2) 切割：\r\n";
        if (str.Contains("+"))
            return Calculation(str, "+").ToString();
        else if (str.Contains("-"))
            return Calculation(str, "-").ToString();
        else if (str.Contains("*"))
            return Calculation(str, "*").ToString();
        else if (str.Contains("/"))
            return Calculation(str, "/").ToString();
        else
        {
            decimal dec = 0;
            decimal.TryParse(str.Contains("#") ? str.Replace("#", "-") : str, out dec);
            MsgBox += "(2.2) 最小單位： " + dec.ToString() + "\r\n";
            return dec.ToString();
        }
    }
    //將運算式再切割成更小的運算式
    private static decimal Calculation(string str, string op)
    {
        string lstr = str.Substring(0, str.IndexOf(op));
        string rstr = str.Substring(str.IndexOf(op) + 1, str.Length - str.IndexOf(op) - 1);
        MsgBox += "(2.1)切割為：\r\n \"" + lstr + "\"" + op + " \"" + rstr + "\"\r\n";
        switch (op)
        {
            case "+":
                return decimal.Parse(Calculation(lstr)) + decimal.Parse(Calculation(rstr));
            case "-":
                return decimal.Parse(Calculation(lstr)) - decimal.Parse(Calculation(rstr));
            case "*":
                return decimal.Parse(Calculation(lstr)) * decimal.Parse(Calculation(rstr));
            case "/":
                return decimal.Parse(Calculation(lstr)) / decimal.Parse(Calculation(rstr));
            default:
                return 0;
        }

    }

    /// <summary>
    /// 四捨五入
    /// </summary>
    /// <param name="mfloat"></param>
    /// <returns></returns>
    public static int Round(double mdouble)
    {//原本寫法 Math.Round(mdouble, 0) 是5捨6入,現改為Math.Round(mdouble, 0, System.MidpointRounding.AwayFromZero) 是4捨5入
        return (int)(Math.Round(mdouble, 0, System.MidpointRounding.AwayFromZero));
    }

}
