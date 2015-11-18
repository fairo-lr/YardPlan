using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Data.OleDb;
using System.Drawing;
using System.Data.SqlClient;
using System.IO;//ToExcel

public partial class BusyManager : System.Web.UI.Page
{  
    #region Sql
    const string strSqlSelect = @"
SELECT [vi_id], [vi_date], [vi_supervisor], [vi_vesselpeople], [vi_recordtime], [vc_recordtime], [vi_submit] 
FROM [VesselInfo] left join (SELECT [vc_date] ,max([vc_recordtime]) [vc_recordtime]   
        FROM [YardPlan].[dbo].[VesselContainer]group by [vc_date]) vc on [vi_date] = [vc_date]
    WHERE 1=1
    --ToDoMore 
    order by vi_date desc
";

    #endregion

    #region 对象   
    private string SelectSql
    {
        get
        {
            return hdSql.Value;            
        }
        set
        {
            hdSql.Value = value;
        }
    }
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        check_privillage();
        if (!IsPostBack)
        {
            SelectSql = strSqlSelect;
            SqlDsVesselWeek.SelectCommand = strSqlSelect;
        }
        else 
        {
            SqlDsVesselWeek.SelectCommand = SelectSql;
        }
    }


    protected void gvVesselWeek_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "btnDetail")
        {
            int iIndex = Convert.ToInt32(e.CommandArgument);
            string strUrl = fnGetSearchUrl(iIndex);
            Response.Write("<script>window.open('" + strUrl + "');location='javascript:history.go(-1);'</script>");
            //Response.Redirect(newurl);
        }
        if (e.CommandName == "btnEdit")
        {
            int iIndex = Convert.ToInt32(e.CommandArgument);
            string strUrl = fnGetUpdateUrl(iIndex);
            Response.Redirect(strUrl);
        }

    }

    //获取编辑updateUrl的字符串
    private string fnGetUpdateUrl(int Index)
    {
        int iIndex = Index;
        string strDate = "";
        string strUrl = "";

        GridViewRow rowIndex = gvVesselWeek.Rows[iIndex];
        strDate =Convert.ToDateTime(rowIndex.Cells[1].Text).ToString("yyyy-MM-dd").Substring(0, 10).ToString();
        strUrl = string.Format("Vessel_NewReport.aspx?date={0}", strDate);

        return strUrl;
    }

    //获取详细SearchUrl的字符串
    private string fnGetSearchUrl(int Index)
    {
        int iIndex = Index;
        string strDate = "";
        string strUrl = "";

        GridViewRow rowIndex = gvVesselWeek.Rows[iIndex];
        strDate = Convert.ToDateTime(rowIndex.Cells[1].Text).ToString("yyyy-MM-dd").Substring(0, 10).ToString();
        strUrl = string.Format("VesselSearchnew.aspx?date={0}", strDate);
        return strUrl;
    }

    //点击查询
    protected void btnSearch_Click(object sender, EventArgs e)
    {        
        SelectSql = strSqlSelect;//还原
        string strSelect = SelectSql;//查询值  
        string t_year = this.ipYear.Value;
        string t_month = this.ipMonth.Value;
        string t_sttm = this.iptStartTime.Value;
        string t_edtm = this.iptEndTime.Value;
        
       
        //-------------查询语句---------------------
        if(t_year !="") //年不为空
        {
            strSelect=strSelect.Replace("--ToDoMore",string.Format(@"--ToDoMore 
                           and year(vi_date)={0} ",t_year));
        }

        if(t_month!="")//月不为空
        {
            strSelect=strSelect.Replace("--ToDoMore",string.Format(@"--ToDoMore 
                            and month(vi_date) = {0} ",t_month));            

        }   
        if(t_sttm!="" && t_edtm=="")//具体时间
        {
               strSelect=strSelect.Replace("--ToDoMore",string.Format(@"--ToDoMore 
                            and convert(varchar(10),vi_date,120)='{0}' ",t_sttm));
        }
        else if(t_sttm=="" && t_edtm !="")
        {
                 strSelect=strSelect.Replace("--ToDoMore",string.Format(@"--ToDoMore 
                            and convert(varchar(10),vi_date,120)='{0}' ",t_edtm));
        }
        else if(t_sttm!="" && t_edtm!="")
        {
               strSelect=strSelect.Replace("--ToDoMore",string.Format(@"--ToDoMore 
                            and convert(varchar(10),vi_date,120)>='{0}'
                            and convert(varchar(10),vi_date,120)<='{1}'",t_sttm,t_edtm));

        }

        if(ddlName.SelectedValue!="0")//人员
        {
            strSelect =strSelect.Replace("ToDoMore",string.Format(@"--ToDoMore
                            and vi_vesselpeople='{0}'", ddlName.SelectedValue));
        }

        SelectSql = strSelect;
        SqlDsVesselWeek.SelectCommand = SelectSql;
        
    }
    
    protected void btnAdd_Click(object sender, EventArgs e)
    {     
        string strRedirect = "";        
        strRedirect = string.Format("Vessel_NewReport.aspx");
        Response.Redirect(strRedirect);

    }
    protected void gvVesselWeek_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int i = 0;
        string t_superName = "";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {        
            LinkButton btn = e.Row.Cells[7].Controls[0] as LinkButton;
            string strUrl = string.Format("VesselSearchnew.aspx?date={0}", Convert.ToDateTime(e.Row.Cells[1].Text).ToString("yyyy-MM-dd").Substring(0, 10));
            btn.Attributes.Add("onclick", "window.open('" + strUrl + "'); return false;");            
        }

    }

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

    //清空查询内容
    protected void btnClean_Click(object sender, EventArgs e)
    {    
        Response.Redirect("VesselManager.aspx");
        
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }
    protected void gvVesselWeek_DataBound(object sender, EventArgs e)
    {

    }
    protected void SqlDsVesselWeek_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        hdtabs1.Value = e.AffectedRows.ToString();
        
    }
}
