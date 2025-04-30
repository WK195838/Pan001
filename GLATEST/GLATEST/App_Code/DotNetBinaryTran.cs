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
/// DotNetBinaryTran 的摘要描述
/// </summary>
public class DotNetBinaryTran
{
    string theDeCode = "+-*/.1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ:;!@#$%^&()'[]{}_| =?\\";
    string theEnCode = "3! 3# 3~ 3% 3$ 1) 1* 1+ 1, 1- 1. 1/ 10 11 1( 2P 2Q 2R 2S 2T 2U 2V 2W 2X 2Y 2Z 2[ 2\\ 2] 2^ 2_ 2` 2a 2b 2c 2d 2e 2f 2g 2h 2i 20 21 22 23 24 25 26 27 28 29 2: 2; 2< 2= 2> 2? 2@ 2A 2B 2C 2D 2E 2F 2G 2H 2I 30 31 3u 36 3w 3x 3y 3T 3z 3| 3} 3{ 3Q 3S 3q 3s 3U 3r 3t 33 35 3R";
    char[] DeCode;
    string[] EnCode;

	public DotNetBinaryTran()
	{
        DeCode = theDeCode.ToCharArray();
        EnCode = theEnCode.Split(new char[] { ' ' });
	}

    /// <summary>
    /// 加密為Binary
    /// </summary>
    /// <param name="Pswd"></param>
    /// <returns></returns>
    public byte[] rtnBinary(String Pswd)
    {
        char[] temp = rtnHash(Pswd).ToCharArray();
        byte[] HashStr = new byte[temp.Length];
        for (int i = 0; i < temp.Length; i++) 
        {
            HashStr[i] = Convert.ToByte(temp[i]);
        }        
        return HashStr;
    }

    /// <summary>
    /// 字串解密
    /// </summary>
    /// <param name="strEnCode"></param>
    /// <returns></returns>
    public string rtnCode(string strEnCode)
    {
        string HashStr = strEnCode;
        if (strEnCode.Length >= 2 && strEnCode.Length % 2 == 0)
        {
            HashStr = "";
            for (int i = 0; i < strEnCode.Length; i++)
            {
                HashStr += GetDeCode(strEnCode.Substring(i, 2));
                i++;
            }
        }
        else
        {
            HashStr = "";
        }

        return HashStr.ToString();
    }

    /// <summary>
    /// 字串加密
    /// </summary>
    /// <param name="strDeCode"></param>
    /// <returns></returns>
    public string rtnHash(string strDeCode)
    {
        string HashStr = "";
        char[] tempHash = strDeCode.ToCharArray();
        for (int i = 0; i < tempHash.Length; i++)
        {
            HashStr += GetEnCode(tempHash[i]);
        }
        return HashStr.ToString();
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="theHash"></param>
    /// <returns></returns>
    protected string GetDeCode(string theHash)
    {
        string rtnCode = "";
        for (int i = 0; i < EnCode.Length; i++)
        {
            if (EnCode[i].Equals(theHash))
                rtnCode = DeCode[i].ToString();
        }
        if (rtnCode.Equals("") && (!theHash.Equals("")))
        {
            if (theHash.StartsWith("?") && (theHash.Length == 2))
                rtnCode = theHash.Substring(1, 1);
            else
                rtnCode = theHash.Remove(1);
        }
        return rtnCode;
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="theChar"></param>
    /// <returns></returns>
    protected string GetEnCode(char theChar)
    {
        string rtnCode = "?" + theChar.ToString();
        for (int i = 0; i < DeCode.Length; i++)
        {
            if (DeCode[i].Equals(theChar))
                rtnCode = EnCode[i];
        }
        return rtnCode;
    }
}
