<%@ WebHandler Language="C#" Class="YardAnalyse_Insert_RainbowPict" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;

public class YardAnalyse_Insert_RainbowPict : IHttpHandler
{

    public void ProcessRequest(HttpContext context) {
        context.Response.ContentType = "text/plain";

        string area = context.Request["area"].ToString();
        string bay = context.Request["bay"].ToString();
        string squeue = context.Request["squeue"].ToString();
        string lnecd = context.Request["lnecd"].ToString();
        string ldunldport1 = context.Request["ldunldport1"].ToString();
        string ldunldport2 = context.Request["ldunldport2"].ToString();
        string height = context.Request["height"].ToString();
        string size = context.Request["size"].ToString();

        SqlCommand cmd = new SqlCommand("ya_set_rainbow_data");
        cmd.CommandType = CommandType.StoredProcedure;

        //往存储过程中添加参数       
        cmd.Parameters.Add("@ROWNUM", SqlDbType.Int).Direction = ParameterDirection.Output;
        cmd.Parameters.Add("@AREA", SqlDbType.VarChar).Value = area;
        cmd.Parameters.Add("@BAY", SqlDbType.VarChar).Value = bay;
        cmd.Parameters.Add("@SQUEUE", SqlDbType.Int).Value = Convert.ToInt32(squeue);
        cmd.Parameters.Add("@LNECD", SqlDbType.VarChar).Value = lnecd;
        cmd.Parameters.Add("@CNTRSIZE", SqlDbType.VarChar).Value = size;
        cmd.Parameters.Add("@CNTRHEIGHT", SqlDbType.VarChar).Value = height;
        cmd.Parameters.Add("@LDUNLDPORT1", SqlDbType.VarChar).Value = ldunldport1;
        cmd.Parameters.Add("@LDUNLDPORT2", SqlDbType.VarChar).Value = ldunldport2;
        
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