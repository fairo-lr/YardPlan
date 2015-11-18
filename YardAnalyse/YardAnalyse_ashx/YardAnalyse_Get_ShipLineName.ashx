<%@ WebHandler Language="C#" Class="YardAnalyse_Get_ShipLineName" %>

using System;
using System.Web;
using System.Text;
using System.Data;

public class YardAnalyse_Get_ShipLineName : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        string dbName = context.Request["dbName"].ToString();//从哪个数据库取数据

        string output = "";
        
        if (dbName == "oracle")
            output = ShipLineNameFromOra();
        else
            output = ShipLineNameFromSql();

        context.Response.Write(output);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    /// <summary>
    /// 从oracle中获取航线名称
    /// </summary>
    /// <returns></returns>
    public string ShipLineNameFromOra()
    {
        string selectSQL = string.Format(@"
select distinct lne.lne_rtcd
  from ps_shipping_lines lne, ps_service_lines sln
 where lne.lne_internalfg is null
   and lne.lne_rtcd = sln.sln_lne_rtcd
   and sln.sln_avlfg = 'Y'
 order by lne_rtcd ");

        DBHelper helper = new DBHelper();
        DataTable table = helper.oraExecuteAdapter(selectSQL);
        string output = "<option> </option>";
        if (table.Rows.Count != 0)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                string lnecd = table.Rows[i][0].ToString();
                output += "<option value='" + lnecd + "'>" + lnecd + "</option>";
            }
        }
        return output;
    }

    /// <summary>
    /// 从SqlServer获取航线名称
    /// </summary>
    /// <returns></returns>
    public string ShipLineNameFromSql()
    {
        string selectSQL = @"
SELECT [YSC_YRP_LNECD] LNECD 
  FROM [ya_shipline_color] 
 ORDER BY LNECD";

        DBHelper helper = new DBHelper();
        DataTable table = helper.ExecuteAdapter(selectSQL);
        string output = "<option> </option>";
        if (table.Rows.Count != 0)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                string lnecd = table.Rows[i][0].ToString();
                output += "<option value='" + lnecd + "'>" + lnecd + "</option>";
            }
        }
        
        return output;
    }
}