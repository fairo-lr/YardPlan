<%@ WebHandler Language="C#" Class="YardAnalyse_Delete_ShipLine" %>

using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;

public class YardAnalyse_Delete_ShipLine : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string lnecd = context.Request["lnecd"].ToString();  

        SqlCommand cmd = new SqlCommand("ya_delete_shipline");
        cmd.CommandType = CommandType.StoredProcedure;

        //往存储过程中添加参数       
        cmd.Parameters.Add("@ROWNUM", SqlDbType.Int).Direction = ParameterDirection.Output;
        cmd.Parameters.Add("@LNECD", SqlDbType.VarChar).Value = lnecd; 
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