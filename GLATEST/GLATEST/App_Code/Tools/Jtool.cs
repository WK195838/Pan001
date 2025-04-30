using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Reflection;

/// <summary>
/// Jtool 的摘要描述
/// </summary>
public static class Jtool
{
    /// <summary>
    /// 擴充方法: 將目前執行中物件轉化成JSON格式字串。
    /// </summary>
    /// <typeparam name="T">要進行JSON序列化的物件型別。</typeparam>
    /// <param name="obj">要進行JSON序列化的物件。</param>
    /// <returns>傳回JSON格式字串內容。</returns>
    public static string ObjectToJson<T>(this T obj)
    {
        System.Web.Script.Serialization.JavaScriptSerializer _JSON = new System.Web.Script.Serialization.JavaScriptSerializer();
        return _JSON.Serialize(obj);
    }

    /// <summary>
    /// 擴充方法: 將JSON字串內容還原成物件。
    /// </summary>
    /// <typeparam name="T">要進行還原的物件型別。</typeparam>
    /// <param name="json">包含還原物件的JSON字串內容。</param>
    public static T JSONToObject<T>(this T obj, string json)
    {
        System.Web.Script.Serialization.JavaScriptSerializer _JSON = new System.Web.Script.Serialization.JavaScriptSerializer();
        obj = _JSON.Deserialize<T>(json);
        return obj;
    }
    /// <summary> 
    /// List 轉換成 Json
    /// </summary> 
    /// <typeparam name="T"></typeparam> 
    /// <param name="jsonName"></param> 
    /// <param name="list"></param> 
    /// <returns></returns> 
    public static string ListToJson<T>(IList<T> list, string jsonName)
    {
        StringBuilder Json = new StringBuilder();
        if (string.IsNullOrEmpty(jsonName))
            jsonName = list[0].GetType().Name;
        Json.Append("{\"" + jsonName + "\":[");
        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                T obj = Activator.CreateInstance<T>();
                PropertyInfo[] pi = obj.GetType().GetProperties();
                Json.Append("{");
                for (int j = 0; j < pi.Length; j++)
                {
                    Type type = pi[j].GetValue(list[i], null).GetType();
                    Json.Append("\"" + pi[j].Name.ToString() + "\":" + StringFormat(pi[j].GetValue(list[i], null).ToString(), type));

                    if (j < pi.Length - 1)
                    {
                        Json.Append(",");
                    }
                }
                Json.Append("}");
                if (i < list.Count - 1)
                {
                    Json.Append(",");
                }
            }
        }
        Json.Append("]}");

        return Json.ToString();
    }

    /// <summary> 
    /// List 轉換成 Json
    /// </summary> 
    /// <typeparam name="T"></typeparam> 
    /// <param name="list"></param> 
    /// <returns></returns> 
    public static string ListToJson<T>(IList<T> list)
    {
        object obj = list[0];

        return ListToJson<T>(list, obj.GetType().Name);
    }
    /// <summary>  
    /// 物件 轉換成 Json
    /// </summary>  
    /// <param name="jsonObject">物件</param>  
    /// <returns>Json字串</returns>  
    public static string ToJson(object jsonObject)
    {
        string jsonString = "{";
        PropertyInfo[] propertyInfo = jsonObject.GetType().GetProperties();
        for (int i = 0; i < propertyInfo.Length; i++)
        {
            object objectValue = propertyInfo[i].GetGetMethod().Invoke(jsonObject, null);
            string value = string.Empty;
            if (objectValue is DateTime || objectValue is Guid || objectValue is TimeSpan)
            {
                value = "'" + objectValue.ToString() + "'";
            }
            else if (objectValue is string)
            {
                value = "'" + ToJson(objectValue.ToString()) + "'";
            }
            else if (objectValue is IEnumerable)
            {
                value = ToJson((IEnumerable)objectValue);
            }
            else
            {
                value = ToJson(objectValue.ToString());
            }
            jsonString += "\"" + ToJson(propertyInfo[i].Name) + "\":" + value + ",";
        }
        jsonString.Remove(jsonString.Length - 1, jsonString.Length);

        return jsonString + "}";
    }
    /// <summary>  
    /// 陣列 轉換成 Json
    /// </summary>  
    /// <param name="array">陣列</param>  
    /// <returns>Json字串</returns>  
    public static string ToJson(IEnumerable array)
    {
        string jsonString = "[";
        foreach (object item in array)
        {
            jsonString += ToJson(item) + ",";
        }
        jsonString.Remove(jsonString.Length - 1, jsonString.Length);

        return jsonString + "]";
    }
    /// <summary>  
    /// 陣列 轉換成 Json  
    /// </summary>  
    /// <param name="array">陣列</param>  
    /// <returns>Json字串</returns>  
    public static string ToArrayString(IEnumerable array)
    {
        string jsonString = "[";
        foreach (object item in array)
        {
            jsonString = ToJson(item.ToString()) + ",";
        }
        jsonString.Remove(jsonString.Length - 1, jsonString.Length);

        return jsonString + "]";
    }
    /// <summary>  
    /// DataView 轉換成 Json
    /// </summary>  
    /// <param name="dv">DataView</param>  
    /// <returns>Json字串</returns> 
    public static string ToJson(DataView dv)
    {
        StringBuilder jsonString = new StringBuilder();
        jsonString.Append("[");
        DataTable dt = new DataTable();
        dt = dv.ToTable();
        DataRowCollection drc = dt.Rows;
        for (int i = 0; i < drc.Count; i++)
        {
            jsonString.Append("{");
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                string strKey = dt.Columns[j].ColumnName;
                string strValue = drc[i][j].ToString();
                Type type = dt.Columns[j].DataType;
                jsonString.Append("\"" + strKey + "\":");
                strValue = StringFormat(strValue, type);
                if (j < dt.Columns.Count - 1)
                {
                    jsonString.Append(strValue + ",");
                }
                else
                {
                    jsonString.Append(strValue);
                }
            }
            jsonString.Append("},");
        }
        jsonString.Remove(jsonString.Length - 1, 1);
        jsonString.Append("]");

        return jsonString.ToString();
    }
    /// <summary>  
    /// DataTable 轉換成 Json
    /// </summary>  
    /// <param name="table">DataTable</param>  
    /// <returns>Json字串</returns>  
    public static string ToJson(DataTable dt)
    {
        StringBuilder jsonString = new StringBuilder();
        jsonString.Append("[");
        DataRowCollection drc = dt.Rows;
        for (int i = 0; i < drc.Count; i++)
        {
            jsonString.Append("{");
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                string strKey = dt.Columns[j].ColumnName;
                string strValue = drc[i][j].ToString();
                Type type = dt.Columns[j].DataType;
                jsonString.Append("\"" + strKey + "\":");
                strValue = StringFormat(strValue, type);
                if (j < dt.Columns.Count - 1)
                {
                    jsonString.Append(strValue + ",");
                }
                else
                {
                    jsonString.Append(strValue);
                }
            }
            jsonString.Append("},");
        }
        jsonString.Remove(jsonString.Length - 1, 1);
        jsonString.Append("]");

        return jsonString.ToString();
    }
    /// <summary> 
    /// DataTable 轉換成 Json  
    /// </summary> 
    /// <param name="jsonName">Json名稱</param> 
    /// <param name="dt">DataTable</param> 
    /// <returns>Json字串</returns> 
    public static string ToJson(DataTable dt, string jsonName)
    {
        StringBuilder Json = new StringBuilder();
        if (string.IsNullOrEmpty(jsonName))
            jsonName = dt.TableName;
        Json.Append("{\"" + jsonName + "\":[");
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Json.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Type type = dt.Rows[i][j].GetType();
                    Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j].ToString(), type));
                    if (j < dt.Columns.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
                Json.Append("}");
                if (i < dt.Rows.Count - 1)
                {
                    Json.Append(",");
                }
            }
        }
        Json.Append("]}");

        return Json.ToString();
    }
    /// <summary>  
    /// SqlDataReader 轉換成 Json  
    /// </summary>  
    /// <param name="dataReader">SqlDataReader</param>  
    /// <returns>Json字串</returns>  
    public static string ToJson(SqlDataReader dataReader)
    {
        StringBuilder jsonString = new StringBuilder();
        jsonString.Append("[");
        while (dataReader.Read())
        {
            jsonString.Append("{");
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                Type type = dataReader.GetFieldType(i);
                string strKey = dataReader.GetName(i);
                string strValue = dataReader[i].ToString();
                jsonString.Append("\"" + strKey + "\":");
                strValue = StringFormat(strValue, type);
                if (i < dataReader.FieldCount - 1)
                {
                    jsonString.Append(strValue + ",");
                }
                else
                {
                    jsonString.Append(strValue);
                }
            }
            jsonString.Append("},");
        }
        dataReader.Close();
        jsonString.Remove(jsonString.Length - 1, 1);
        jsonString.Append("]");

        return jsonString.ToString();
    }
    /// <summary>  
    /// DataSet 轉換成 Json  
    /// </summary>  
    /// <param name="dataSet">DataSet</param>  
    /// <returns>Json字串</returns>  
    public static string ToJson(DataSet dataSet)
    {
        string jsonString = "{";
        foreach (DataTable table in dataSet.Tables)
        {
            jsonString += "\"" + table.TableName + "\":" + ToJson(table) + ",";
        }
        jsonString = jsonString.TrimEnd(',');

        return jsonString + "}";
    }
    /// <summary> 
    /// 過濾特殊字元
    /// </summary> 
    /// <param name="s"></param> 
    /// <returns></returns> 
    private static string String2Json(String s)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < s.Length; i++)
        {
            char c = s.ToCharArray()[i];
            switch (c)
            {
                case '\"':
                    sb.Append("\\\""); break;
                case '\\':
                    sb.Append("\\\\"); break;
                case '/':
                    sb.Append("\\/"); break;
                case '\b':
                    sb.Append("\\b"); break;
                case '\f':
                    sb.Append("\\f"); break;
                case '\n':
                    sb.Append("\\n"); break;
                case '\r':
                    sb.Append("\\r"); break;
                case '\t':
                    sb.Append("\\t"); break;
                default:
                    sb.Append(c); break;
            }
        }

        return sb.ToString();
    }
    /// <summary> 
    /// 格式化字串型、日期型、布林型 
    /// </summary> 
    /// <param name="str"></param> 
    /// <param name="type"></param> 
    /// <returns></returns> 
    private static string StringFormat(string str, Type type)
    {
        if (type == typeof(string))
        {
            str = String2Json(str);
            str = "\"" + str + "\"";
        }
        else if (type == typeof(DateTime))
        {
            str = "\"" + str + "\"";
        }
        else if (type == typeof(bool))
        {
            str = str.ToLower();
        }
        else if (type != typeof(string) && string.IsNullOrEmpty(str))
        {
            str = "\"" + str + "\"";
        }

        return str;
    }
}