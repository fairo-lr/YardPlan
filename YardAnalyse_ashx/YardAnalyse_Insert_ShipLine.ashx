<%@ WebHandler Language="C#" Class="YardAnalyse_Insert_ShipLine" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;


public class YardAnalyse_Insert_ShipLine : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        string lnecd = context.Request["lnecd"].ToString();
        string color = context.Request["color"].ToString();
        string ports = context.Request["ports"].ToString();

        SqlCommand cmd = new SqlCommand("ya_insert_shipline");
        cmd.CommandType = CommandType.StoredProcedure;

        //往存储过程中添加参数       
        cmd.Parameters.Add("@ROWNUM", SqlDbType.Int).Direction = ParameterDirection.Output;
        cmd.Parameters.Add("@LNECD", SqlDbType.VarChar).Value = lnecd;
        cmd.Parameters.Add("@COLOR", SqlDbType.VarChar).Value = color;
        cmd.Parameters.Add("@PORTS", SqlDbType.VarChar).Value = ports;

        DBHelper helper = new DBHelper();
        int rownum = helper.ExecuteProcedure(cmd);
        context.Response.Write(rownum);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}