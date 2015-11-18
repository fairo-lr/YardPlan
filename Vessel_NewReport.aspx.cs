using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Vessel_NewReport : System.Web.UI.Page
{
    #region 基础代码
    //返回记录数
    private string fnMsGetValue(string strSelect)
    {
        string strReturn = "";
        string strCount = strSelect;
        System.Data.SqlClient.SqlConnection sqlConn = new SqlConnection(
            ConfigurationManager.ConnectionStrings["YardPlanConnectionString1"].ConnectionString);
        System.Data.SqlClient.SqlCommand sqlComm = new SqlCommand(
            strCount, sqlConn);
        sqlConn.Open();
        System.Data.SqlClient.SqlDataReader sqlData = sqlComm.ExecuteReader();

        if (sqlData.Read())
        {
            strReturn = sqlData.GetValue(0) + "";

        }
        sqlData.Dispose();
        sqlComm.Dispose();
        sqlConn.Dispose();

        return strReturn;

    }

    //返回修改记录数
    private int fnMsSetSql(string strSelect)
    {
        int iReturn = 0;
        string strCount = string.Format(strSelect);
        System.Data.SqlClient.SqlConnection sqlConn = new SqlConnection(
            ConfigurationManager.ConnectionStrings["YardPlanConnectionString1"].ConnectionString);
        System.Data.SqlClient.SqlCommand sqlComm = new SqlCommand(
            strCount, sqlConn);
        sqlConn.Open();
        iReturn = sqlComm.ExecuteNonQuery();

        sqlComm.Dispose();
        sqlConn.Dispose();

        return iReturn;

    }

    //根据SQL返回数据的值，如果为空返回的是null
    private string[,] FnGetValue(string sql, string[,] arrValue)
    {
        string strSql = sql;
        int m = arrValue.GetLength(0);
        int n = arrValue.GetLength(1);

        string[,] arrDetails = new string[m, n];

        System.Data.OracleClient.OracleConnection oraConn =
            new System.Data.OracleClient.OracleConnection(
            ConfigurationManager.ConnectionStrings["oraConnectionString"].ConnectionString);
        System.Data.OracleClient.OracleCommand oraComm = new System.Data.OracleClient.OracleCommand(strSql, oraConn);

        oraConn.Open();
        System.Data.OracleClient.OracleDataReader oraData = oraComm.ExecuteReader();

        //读取相关数据
        for (int i = 0; i < m; i++)
        {
            if (oraData.Read())
            {
                for (int j = 0; j < n; j++)
                {
                    arrDetails[i, j] = oraData.IsDBNull(j) ? null : oraData.GetValue(j).ToString();
                }
            }
            else
            {
                break;
            }
        }
        oraData.Close();
        oraComm.Cancel();
        oraConn.Close();
        oraData.Dispose();
        oraComm.Dispose();
        oraConn.Dispose();
        // arrReturn = fnSetLeftingArr(arrDetails);
        return arrDetails;
    }
    #endregion

    #region 权限测试
    public void check_privillage()
    {
        string Select_SQL, Select_SQL2, Select_SQL3, Update_SQL1;
        OleDbConnection SqlConnection1 = new OleDbConnection();   
        SqlConnection1.ConnectionString = AccessDataSource1.ConnectionString;
        Select_SQL = "select module_name,page_name,user_name from module_user_mng,user_mng,module_mng where module_mng.module_id = module_user_mng.module_id and user_mng.user_id = module_user_mng.user_id and page_name='" + System.IO.Path.GetFileName(Request.PhysicalPath) + "' and user_name='" + Session["xsctintranet"] + "'";

        SqlConnection1.Open();
        OleDbCommand SqlCommand = SqlConnection1.CreateCommand();
        SqlCommand.CommandText = Select_SQL;
        OleDbDataReader SqlDataReader = SqlCommand.ExecuteReader();
        if (SqlDataReader.HasRows)
        {
            while (SqlDataReader.Read())
            {
                OleDbCommand SqlCommand2 = SqlConnection1.CreateCommand();
                Update_SQL1 = "insert into visit_mng (visit_module_name,visit_user_name,visit_date,visit_ip,visit_module_page) VALUES ('" + SqlDataReader.GetString(0) + "','" + SqlDataReader.GetString(2) + "','" + DateTime.Now.ToString() + "','" + Page.Request.UserHostAddress + "','" + SqlDataReader.GetString(1) + "')";
                SqlCommand2.CommandText = Update_SQL1;
                SqlCommand2.ExecuteNonQuery();

            }
        }
        else
        {
            OleDbCommand SqlCommand2 = SqlConnection1.CreateCommand();
            Select_SQL2 = "select * from module_mng where page_name='" + System.IO.Path.GetFileName(Request.PhysicalPath) + "'";
            SqlCommand2.CommandText = Select_SQL2;
            OleDbDataReader SqlDataReader2 = SqlCommand2.ExecuteReader();
            if (SqlDataReader2.HasRows)
            {
                Response.Write("您没有访问该页面的权限！");
                SqlDataReader2.Close();
                SqlDataReader.Close();
                SqlConnection1.Close();
                Response.End();
            }
            else
            {
                OleDbCommand SqlCommand3 = SqlConnection1.CreateCommand();
                Select_SQL3 = "select * from user_mng where user_name='" + Session["xsctintranet"] + "'";
                SqlCommand3.CommandText = Select_SQL3;
                OleDbDataReader SqlDataReader3 = SqlCommand3.ExecuteReader();
                if (SqlDataReader3.HasRows)
                { }
                else
                {
                    Response.Write("您没有访问该页面的权限！");
                    SqlDataReader3.Close();
                    SqlDataReader2.Close();
                    SqlDataReader.Close();
                    SqlConnection1.Close();
                    Response.End();
                }
                SqlDataReader3.Close();
            }
            SqlDataReader2.Close();
        }
        SqlDataReader.Close();
        SqlConnection1.Close();
    }
    #endregion

    private string strDate = string.Empty; 

    protected void Page_Load(object sender, EventArgs e)
    {
        check_privillage();

        //首次载入
        GetDate();
        if (!IsPostBack)
        {          
            string strVesselPeople = GetBusyVesselPeople(strDate);
            if (!string.IsNullOrEmpty(strVesselPeople))
            {
                ddlVesselPeople.ClearSelection();
                if (strVesselPeople == "肖鑫" || strVesselPeople == "林燕珍" ||
                    strVesselPeople == "姚雪倩" || strVesselPeople == "王秋")
                {
                    
                   //ddlVesselPeople.ClearSelection();.SelectedValue;
                   //ddlVesselPeople.Items;
                   // ddlVesselPeople.Items.FindByValue(strVesselPeople).Selected = true;
                }           
            }
            hdStartTime.Value = strDate;         
            this.hlkPreview.Visible = true;
            this.hlkPreview.NavigateUrl = "VesselSearchNew.aspx?date=" + strDate;
        }
        
        //判断是否为空，前台显示
        if (!string.IsNullOrEmpty(GetBusyVesselPeople(strDate)))
        {
            hdShow.Value = "show";
        }
        else
        {
            hdShow.Value = "hide";

        }

    }

    //获取其值。
    private void GetDate()
    {
        strDate = Request.QueryString["date"] + "" == ""
            ? System.DateTime.Now.ToString("yyyy-MM-dd")
            : Request.QueryString["date"] + "";

        string date = "";
        if (Request.QueryString["date"] + "" == "")
        {
            date = fnGetSubmitDate();//最终提交下一天
            if (!string.IsNullOrEmpty(date))
            {
                if (Convert.ToDateTime(date).ToString("yyyy-MM-dd") != strDate)
                {
                    hypAddBusy.Text = "点击添加" +
                        System.DateTime.Parse(date).AddDays(1).ToString("yyyy-MM-dd").Substring(0, 10);
                }
            }
        }  
    }

    //获取人员信息
    protected string GetBusyVesselPeople(string p_date)
    {
        #region SQL
        string t_sql =
            string.Format(@"SELECT  [vi_vesselpeople] FROM [VesselInfo]
        where vi_date='{0}'", p_date);
        #endregion
        
        return fnMsGetValue(t_sql);

    }


    private string fnGetSubmitDate()
    {
        #region SQL
        string t_sql = "SELECT [vi_date] FROM [VesselInfo] where vi_submit='Y' order by vi_date desc";
        #endregion
        return fnMsGetValue(t_sql);

    }

    private bool isSubmit(String p_date)
    {
        #region SQL
        string t_sql = string.Format(
            "SELECT [vi_submit] FROM [VesselInfo] where vi_date='{0}'", p_date);
        #endregion
        return fnMsGetValue(t_sql) == "Y" ? true : false;

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strDate = hdStartTime.Value;
        string strSupervisor = ddlSupervisor.SelectedValue.ToString();
        string strVesselPeople = ddlVesselPeople.SelectedItem.Text.ToString();//.SelectedValue.ToString();
        string strName = GetBusyVesselPeople(strDate);        

        if (string.IsNullOrEmpty(strName))
        {
            InsertRecord(strDate, strSupervisor, strVesselPeople, "N");            
        }
        else
        {
            UpdateRecord(strDate, strVesselPeople, "Null");
        }

        this.labBtnMessage.Text = "船舶作业信息保存成功！";
        hdShow.Value = "show";

    }

    protected void InsertRecord(string strDate, string strSupervisor, string strVesselPeople
        , string strIsSubmit)
    {
        #region sql
        String mssql = string.Format(@"INSERT INTO [VesselInfo] 
      ([vi_date], [vi_supervisor], [vi_vesselpeople], [vi_recordtime], [vi_submit]) 
     VALUES('{0}','{1}','{2}',getDate(),'{3}')", strDate, strSupervisor
                            , strVesselPeople, strIsSubmit);
        #endregion
        fnMsSetSql(mssql);
    }

    protected void UpdateRecord(string strDate, string strVesselPeople, string strIsSubmit)
    {
        #region sql
        String mssql = string.Format(@"UPDATE [VesselInfo] 
    SET [vi_vesselpeople] ='{0}' --,  [vi_submit] = '{1}'
    WHERE [vi_Date]='{2}' ", strVesselPeople, strIsSubmit, strDate);
        #endregion
        fnMsSetSql(mssql);

    }

    protected void btnEndShift_Click(object sender, EventArgs e)
    {        
        string strVesselPeople = ddlVesselPeople.SelectedItem.Text;//.SelectedValue;
        string strSupervisor = ddlSupervisor.SelectedValue;
     
        if (string.IsNullOrEmpty(GetBusyVesselPeople(strDate)))
        {
            InsertRecord(strDate, strSupervisor, strVesselPeople, "Y");
            InsertRecord(strDate, strSupervisor, strVesselPeople,"Y");
            InsertRecord(strDate, strSupervisor, strVesselPeople, "Y" );
            InsertRecord(strDate, strSupervisor, strVesselPeople, "Y");
            this.labBtnMessage.Text = "工班最终提交成功！";
        }
        else
        {
            UpdateRecord(strDate, strVesselPeople, "Y");
        }
        Response.Redirect("VesselManager.aspx");
    }




    protected void hypAddBusy_Click(object sender, EventArgs e)
    {
        string date = "";
        if (Request.QueryString["date"] + "" == "")
        {
            date = fnGetSubmitDate();//最终提交下一天
            if (date != "")
            {
                hypAddBusy.Text = "点击添加" +
                    System.DateTime.Parse(date).AddDays(1).ToString("yyyy-MM-dd").Substring(0, 10);
            }
            strDate = System.DateTime.Now.ToString("yyyy-MM-dd");
        }
        Response.Redirect("Vessel_NewReport.aspx?date="
            + System.DateTime.Parse(date).AddDays(1).ToString("yyyy-MM-dd").Substring(0, 10));
    }


    protected void ddlVesselPeople_PreRender(object sender, EventArgs e)
    {
        string strVesselPeople = GetBusyVesselPeople(strDate);
        if (!string.IsNullOrEmpty(strVesselPeople))
        {
            foreach (ListItem item in ddlVesselPeople.Items)
            {
                if (item.Text == strVesselPeople)
                {
                    ddlVesselPeople.ClearSelection();
                    ddlVesselPeople.Items.FindByText(strVesselPeople).Selected = true;
                    break;
                }                
            }
            
        }
    }
    protected void ddlVesselPeople_DataBound(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlVesselPeople.Items.Add(new ListItem("无", "0"));
            ddlVesselPeople.ClearSelection();
            ddlVesselPeople.Items.FindByText("无").Selected = true;

        }
    }
}

