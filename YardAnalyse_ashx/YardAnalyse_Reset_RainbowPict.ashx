<%@ WebHandler Language="C#" Class="YardAnalyse_Reset_RainbowPict" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;

public class YardAnalyse_Reset_RainbowPict : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        string area = context.Request["area"].ToString();

        SqlCommand cmd = new SqlCommand("ya_reset_rainbow_area");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add("@ROWNUM", SqlDbType.Int).Direction = ParameterDirection.Output;
        cmd.Parameters.Add("@AREA", SqlDbType.VarChar).Value = area;

        DBHelper helper = new DBHelper();
        int rownum = helper.ExecuteProcedure(cmd);
        context.Response.Write(rownum);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}