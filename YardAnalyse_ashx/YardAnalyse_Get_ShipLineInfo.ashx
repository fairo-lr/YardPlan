<%@ WebHandler Language="C#" Class="YardAnalyse_Get_ShipLineInfo" %>

using System;
using System.Web;
using System.Text;
using System.Data;

/// <summary>
/// 获取航线信息  
/// </summary> 
public class YardAnalyse_Get_ShipLineInfo : IHttpHandler {
 
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "application/json";

        string selectSQL = @" 
        SELECT [YSC_YRP_LNECD] LNECD
        ,[YSC_COLOR] COLOR
        FROM [ya_shipline_color] 
        ORDER BY LNECD ";
        
        StringBuilder output = new StringBuilder("");
        DBHelper helper = new DBHelper();
        DataTable table = helper.ExecuteAdapter(selectSQL);
        if (table.Rows.Count != 0)
        {
            output = DataTableToJSON(table);
        }
        else
        {
            //数据库无航线信息
            output.Append("");
        }
        context.Response.Write(output.ToString()); 
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    //Datatable转Json
    public StringBuilder DataTableToJSON(DataTable dt)
    {
        string columnName;
        StringBuilder builder = new StringBuilder();
        if (dt != null)
        {
            builder.Append("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                builder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    columnName = dt.Columns[j].ColumnName;
                    builder.Append("\"" + columnName + "\":");
                    builder.Append("\"" + dt.Rows[i][columnName] + "\",");
                }
                builder.Append("},");
            }
            builder.Append("]");
            builder = builder.Replace(",}", "}").Replace(",]", "]"); //去掉最后面多余的逗号
        }
        return builder;
    }

}