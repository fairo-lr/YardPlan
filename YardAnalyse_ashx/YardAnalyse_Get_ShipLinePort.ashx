<%@ WebHandler Language="C#" Class="YardAnalyse_Get_ShipLinePort" %>

using System;
using System.Web;
using System.Text;
using System.Data;


public class YardAnalyse_Get_ShipLinePort : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        string dbName = context.Request["dbName"].ToString();
        string lnecd = context.Request["lnecd"].ToString();
        
        string output = "";
        if (dbName == "oracle")
            output = ShipLinePortFromOra(lnecd);
        else
            output = ShipLinePortFromSql(lnecd);
       
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
    public string ShipLinePortFromOra(string lnecd)
    {
        string selectSQL = string.Format(@"
select distinct scl.SCL_POT_DSTPORTCD
  from ps_shipping_lines        lne,
       ps_service_lines         sln,
       PS_SSHIPPING_BERTH_PORTS scl
 where lne.lne_internalfg is null
   and lne.lne_rtcd = sln.sln_lne_rtcd
   and sln.sln_avlfg = 'Y'
   and scl.SCL_SCH_SLN_SLINEID = sln.sln_slineid
   and lne.lne_rtcd = '{0}'
 order by scl.SCL_POT_DSTPORTCD ", lnecd);

        DBHelper helper = new DBHelper();
        DataTable table = helper.oraExecuteAdapter(selectSQL);
        string output = "";
        if (table.Rows.Count != 0)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                string port = table.Rows[i][0].ToString();
                output += "<input type='checkbox' name='port' value='" + port + "'/>" + port + "&nbsp;&nbsp;&nbsp;&nbsp;";
            }
        }
        return output;
    }

    /// <summary>
    /// 从SqlServer获取航线名称
    /// </summary>
    /// <returns></returns>
    public string ShipLinePortFromSql(string lnecd)
    {
        string selectSQL = string.Format(@"
SELECT YSL_LDUNLDPORT PORT
  FROM YA_SHIPLINE_LDUNLDPORT
 WHERE YSL_YAC_LNECD = '{0}'
 ORDER BY PORT ", lnecd);

        DBHelper helper = new DBHelper();
        DataTable table = helper.ExecuteAdapter(selectSQL);
        string output = "<option> </option>";
        if (table.Rows.Count != 0)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                string port = table.Rows[i][0].ToString();
                output += "<option value='" + port + "'>" + port + "</option>";
            }
        }

        return output;
    }

}