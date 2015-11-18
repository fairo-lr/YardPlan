using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Data;

//数据库操作类 
public class DBHelper
{
    private static string sqlConnectString = "Data Source=172.30.12.1;Initial Catalog=YardPlan;Persist Security Info=True;User ID=sa;Password=xsct.albb40DD";
    private static string oraConnectString = "Data Source=xsctture;Persist Security Info=True;User ID=xsctwebusr;Password=xsctwebusr;Unicode=True";

    public DataTable ExecuteAdapter(string sql)
    {
        DataTable table = new DataTable();
        SqlConnection con = new SqlConnection(sqlConnectString);
        SqlCommand cmd = new SqlCommand(sql, con);
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        try
        {
            con.Open();
            adapter.Fill(table);
            con.Close();
        }
        catch (Exception err)
        {
            con.Close();
            throw (err);
        }

        return table;
    }

    /// <summary>
    /// 执行存储过程
    /// </summary>
    /// <param name="sql">存储过程名称</param>
    /// <returns>影响行数</returns>
    public int ExecuteProcedure(SqlCommand cmd)
    {
        int rownum = 0;
        SqlConnection con = new SqlConnection(sqlConnectString);
        cmd.Connection= con; 

        try
        {
            con.Open();
            cmd.ExecuteNonQuery();
            rownum = Convert.ToInt32(cmd.Parameters["@ROWNUM"].Value);
            con.Close();
        }
        catch (Exception err)
        {
            con.Close();
            throw (err);
        }
        
        return rownum;
    }


    public DataTable oraExecuteAdapter(string sql)
    {
        DataTable table = new DataTable();
        OracleConnection con = new OracleConnection(oraConnectString);
        OracleCommand cmd = new OracleCommand(sql, con);
        OracleDataAdapter adapter = new OracleDataAdapter(cmd);
        try
        {
            con.Open();
            adapter.Fill(table);
            con.Close();
        }
        catch (Exception err)
        {
            con.Close();
            throw (err);
        }

        return table;
    }

    public void ExecuteReader(string sql)
    {
        SqlConnection con = new SqlConnection(sqlConnectString);
        SqlCommand cmd = new SqlCommand(sql, con);
        //SqlDataReader reader;
        SqlDataAdapter adapter;
        try
        { 
            con.Open();

            /*
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                YardDensityData temp = new YardDensityData();
                temp.Date = Convert.ToDateTime(reader["Time"]);
                temp.Empty = Convert.ToInt32(reader["empty"]);
                temp.LadenEmpty = Convert.ToInt32(reader["laden_empty"]);
                temp.LadenFull = Convert.ToInt32(reader["laden_full"]);
                temp.Span = Convert.ToInt32(reader["span"].ToString() == "" ? "0" : reader["span"]);
                temp.Week = temp.Date.DayOfWeek.ToString("d");
                temp.IntWeek = temp.GetIntByWeek();
                temp.Total = temp.GetTotal();

                list.Add(temp);
            }
            */
            con.Close();
        }
        catch (Exception err)
        {
            con.Close();
            throw (err);
        }

        // return list;
    }

}