<%@ WebHandler Language="C#" Class="YardAnalyse_Get_RainbowPict" %>

using System;
using System.Web;
using System.Text;
using System.Data;

public class YardAnalyse_Get_RainbowPict : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/text";

        string area = context.Request["area"].ToString();
        string bay = context.Request["bay"].ToString() == "" ? "%" : context.Request["bay"].ToString();//若没有贝位，就查整个箱区
        
        #region SQL
        string bayInfoSQL = string.Format(@"         
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
ORDER BY YRP_BAY DESC
 ", area, bay);

        //获得箱区的所有贝位
        string squeueSql = string.Format(@"
SELECT YRP_SQUEUE	 
FROM ya_rainbow_pict
WHERE YRP_AREA='{0}'
ORDER BY YRP_SQUEUE DESC", area);
        #endregion

        DBHelper helper = new DBHelper();
        DataTable tbTable = helper.ExecuteAdapter(bayInfoSQL);   
        DataTable tfTable=helper.ExecuteAdapter(squeueSql);     
        string tableHtml = "";

        if (tbTable.Rows.Count != 0)
        {
            tableHtml = string.Format("<table class='area' id='{0}'>{1}</table>", area, thead(area, tfTable.Rows.Count) + tbody(tbTable) + tfoot(tfTable));
        } 
        
        context.Response.Write(tableHtml);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    } 
    
    public string thead(string area, int count)
    {
        return string.Format("<thead class='area-head'><tr><th colspan='{1}'>{0}</th></tr></thead>", area, count);
    }

    public string tbody(DataTable dt)
    {
        string td = "";
        string tdModel = "<td style='background-color:{0};width:{11}px;' area='{1}' bay='{2}' lnecd='{3}' size='{4}' height='{5}' port1='{6}' port2='{7}' colspan='{8}' squeue='{10}'>{9}</td>";
        
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string content = dt.Rows[i]["YRP_LDUNLDPORT1"] + "<br/>" + dt.Rows[i]["YRP_LDUNLDPORT2"] + "<br/>" + dt.Rows[i]["YRP_CNTR_HEIGHT"] + "<br/>" + dt.Rows[i]["YRP_CNTR_SIZE"];
            
            if (dt.Rows[i]["YRP_CNTR_SIZE"].ToString() == "40" || dt.Rows[i]["YRP_CNTR_SIZE"].ToString() == "45")
            {
                td += string.Format(tdModel, dt.Rows[i]["YSC_COLOR"], dt.Rows[i]["YRP_AREA"], dt.Rows[i]["YRP_BAY"], dt.Rows[i]["YRP_LNECD"], dt.Rows[i]["YRP_CNTR_SIZE"]
                    , dt.Rows[i]["YRP_CNTR_HEIGHT"], dt.Rows[i]["YRP_LDUNLDPORT1"], dt.Rows[i]["YRP_LDUNLDPORT2"], 2, content, dt.Rows[i]["YRP_SQUEUE"], 66);
            }
            else
            {
                td += string.Format(tdModel, dt.Rows[i]["YSC_COLOR"], dt.Rows[i]["YRP_AREA"], dt.Rows[i]["YRP_BAY"], dt.Rows[i]["YRP_LNECD"], dt.Rows[i]["YRP_CNTR_SIZE"]
                    , dt.Rows[i]["YRP_CNTR_HEIGHT"], dt.Rows[i]["YRP_LDUNLDPORT1"], dt.Rows[i]["YRP_LDUNLDPORT2"], 1, content, dt.Rows[i]["YRP_SQUEUE"], 33);
            }
        }
        return string.Format("<tbody><tr>{0}</tr></tbody>", td);
    }
    
    public string tfoot(DataTable dt)
    {
        string tf = "";
        string tfModel = "<th>{0}</th>";
        
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            tf += string.Format(tfModel, dt.Rows[i]["YRP_SQUEUE"]);
        }
        return string.Format("<tfoot class='area-foot'><tr>{0}</tr></tfoot>", tf);
    }
}