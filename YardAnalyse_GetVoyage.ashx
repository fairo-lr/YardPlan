<%@ WebHandler Language="C#" Class="YardAnalyse_GetVoyage" %>

using System;
using System.Web;
using System.Data;

public class YardAnalyse_GetVoyage : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
         
        string sql = context.Request["sql"].ToString();   

        DBHelper helper = new DBHelper();
        DataTable table = helper.ExecuteAdapter(sql);

        string options = "<option value=''></option>";
        if (table.Rows.Count != 0)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                options += "<option value='" + table.Rows[i][0].ToString() + "'>" + table.Rows[i][1].ToString() + "</option>";
            }
        }
      
        context.Response.Write(options);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}