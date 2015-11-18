<%@ WebHandler Language="C#" Class="YardAnalyse_Get_RainbowPict" %>

using System;
using System.Web;
using System.Text;
using System.Data;

public class YardAnalyse_Get_RainbowPict : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "application/json";
        
        string area = context.Request["area"].ToString();
        string bay = context.Request["bay"].ToString() == "" ? "%" : context.Request["bay"].ToString();//若没有贝位，就查整个箱区

        string selectSQL = string.Format(@"         
SELECT YSC_COLOR
	,YRP_AREA
	,YRP_BAY
	,YRP_SQUEUE
	,YRP_LNECD
	,YRP_CNTR_SIZE
	,YRP_CNTR_HEIGHT
	,YRP_LDUNLDPORT1
	,YRP_LDUNLDPORT2
FROM ya_rainbow_pict
LEFT JOIN ya_shipline_color ON YRP_LNECD = YSC_YRP_LNECD
WHERE SUBSTRING(YRP_AREA, 0, 3) LIKE '{0}'
	AND YRP_BAY LIKE '{1}'
	AND YRP_CNTR_SIZE NOT IN (
		'40'
		,'45'
		)

UNION ALL

SELECT YSC_COLOR
	,YRP_AREA
	,YRP_BAY
	,YRP_SQUEUE
	,YRP_LNECD
	,YRP_CNTR_SIZE
	,YRP_CNTR_HEIGHT
	,YRP_LDUNLDPORT1
	,YRP_LDUNLDPORT2
FROM ya_rainbow_pict
LEFT JOIN ya_shipline_color ON YRP_LNECD = YSC_YRP_LNECD
WHERE SUBSTRING(YRP_AREA, 0, 3) LIKE '{0}'
	AND YRP_BAY LIKE '{1}'
	AND YRP_CNTR_SIZE IN (
		'40'
		,'45'
		)
	AND CONVERT(INT, YRP_BAY) > YRP_SQUEUE
ORDER BY YRP_BAY DESC;
 ", area, bay);
       

        StringBuilder output = new StringBuilder("");
        DBHelper helper = new DBHelper();
        DataTable table = helper.ExecuteAdapter(selectSQL);
        if (table.Rows.Count != 0)
        {
            output = DataTableToJSON(table);
        }
        else
        {
            //数据库无此箱区资料
            output.Append("");
        }
        context.Response.Write(output.ToString());
    }

    public bool IsReusable
    {
        get
        {
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