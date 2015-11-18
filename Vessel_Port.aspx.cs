using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OracleClient;
using System.Data.SqlClient;

public partial class Busy_Vessel : System.Web.UI.Page
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

    private string strDate = string.Empty;
    #region 靠泊信息
    private string sqlBerth = @"select voc.voc_ivoyage || ' / ' || voc.voc_expvoyage voyage,
       vls.vls_vchnnm,
       vls.vls_vnamecd,
       vls.vls_vlength 船长,
       nvl(vbt.vbt_astartpst, vbt.vbt_pstartpst) 起始尺码,
       nvl(vbt.vbt_aendpst, vbt.vbt_pendpst) 终止尺码,
       nvl(vbt.vbt_abthdirc, vbt.vbt_pbthdirc) 靠泊方向,
       nvl(vbt.vbt_abthtm, vbt.vbt_pbthtm) 靠泊时间,
       vie.vie_intrade 内贸,
       sln.sln_vcm_cstmcd || '-' || sln.sln_lne_rtcd 航线,
       case
         when round((nvl(vbt.vbt_abthtm, vbt.vbt_pbthtm) - sysdate) * 24, 0) < 0 then
          0
         else
          round((nvl(vbt.vbt_abthtm, vbt.vbt_pbthtm) - sysdate) * 24, 0)
       end 时间,
       pot.prepot,
       pot.nxtpot
  from ps_vessel_berthes vbt,
       ps_vessel_occurences voc,
       ps_vessels vls,
       ps_vessel_import_exports vie,
       ps_service_lines sln,
       (SELECT pot1.pot_cty_countrycd || pot1.pot_portcd prepot,
               pot2.pot_cty_countrycd || pot2.pot_portcd nxtpot,
               sln.sln_lne_rtcd,
               sln.sln_slineid
          FROM (select case
                         when LAG(scl.scl_pot_portcd)
                          over(partition by scl.scl_sch_sln_slineid order by
                                   scl.scl_portseq) is null then
                          first_value(scl.scl_pot_portcd)
                          over(partition by scl.scl_sch_sln_slineid order by
                               scl.scl_portseq desc)
                         else
                          LAG(scl.scl_pot_portcd)
                          over(partition by scl.scl_sch_sln_slineid order by
                               scl.scl_portseq)
                       end preprt,
                       case
                         when LEAD(scl.scl_pot_portcd)
                          over(partition by scl.scl_sch_sln_slineid order by
                                   scl.scl_portseq) is null then
                          first_value(scl.scl_pot_portcd)
                          over(partition by scl.scl_sch_sln_slineid order by
                               scl.scl_portseq)
                         else
                          LEAD(scl.scl_pot_portcd)
                          over(partition by scl.scl_sch_sln_slineid order by
                               scl.scl_portseq)
                       end nextprt,
                        scl.scl_sch_sln_slineid,
                        scl.scl_pot_portcd
                  from PS_SSHIPPING_BERTH_PORTS scl
                --and scl.scl_sch_sln_slineid = '9996'
                ) scl,
               ps_service_lines sln,
               ps_ports pot1,
               ps_ports pot2
         where scl.scl_pot_portcd = 'XIA'
           and sln.sln_slineid = scl.scl_sch_sln_slineid
           and scl.preprt = pot1.pot_portcd
           and scl.nextprt = pot2.pot_portcd) pot
 where vbt.vbt_pbthtm between sysdate - 1 and sysdate + 7
   and vbt.vbt_voc_ocrrid = voc.voc_ocrrid
   and vls.vls_vnamecd = voc.voc_vls_vnamecd
   and voc.voc_ocrrid = vie.vie_vbt_voc_ocrrid
   and vie.vie_iefg = 'I'
   and sln.sln_slineid = vie.vie_sln_slineid
   and vbt.vbt_adpttm is null
   and pot.sln_slineid = sln.sln_slineid
 order by round((nvl(vbt.vbt_abthtm, vbt.vbt_pbthtm) -
                to_date(to_char(sysdate, 'yyyy-mm-dd') || ' 00:00:00',
                         'yyyy-mm-dd hh24:mi:ss')) * 24,
                0)
";
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    { 
        GetDate();
        if (IsPostBack)
        {
            
        }
    }

    private void GetDate()
    {
        strDate = Request.QueryString["date"] + "" == ""
            ? System.DateTime.Now.ToString("yyyy-MM-dd")
            : Request.QueryString["date"] + "";
        SqlDataSource1.SelectParameters["date"].DefaultValue = strDate;        
    }

    //保存信息到泊位图，无用
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string sqlInsert = @"INSERT INTO [YardPlan].[dbo].[VesselPort]
           ([vp_date]           ,[vp_length]           ,[vp_startport]
           ,[vp_vesselcode]           ,[vp_chname]           ,[vp_ename]
           ,[vp_ivoyage]           ,[vp_expvoyage]           ,[vp_betrthtime]
           ,[vp_departtime]           ,[vp_prePort]           ,[vp_nextPort]
           ,[vp_berthside]           ,[vp_recordtime])
SELECT '"+strDate+@"'      ,[vlength]      ,[startport] 
	  ,[ocrrid]      	  ,[vchname]  	  ,[vename]
      ,[ivoyage]      ,[expvoyage]	,[pTime] sttm
    , dateadd(hh,convert(int,[worktime]),[pTime])	  endtime       
      ,[preport]      ,[nextport]      ,[portside]
	,getDate()
  FROM [YardPlan].[dbo].[ora_WeekPort_VW]
";
        fnMsGetValue(sqlInsert);
        GridView1.DataBind();
        
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Insert":
                SqlDataSource1.InsertParameters["vp_date"].DefaultValue = strDate;
                
                break;
            case "Edit":
                break;
            case "Update":
               // SqlDataSource1.UpdateParameters["vp_date"].DefaultValue = strDate;
                
                break;

            default:
                break;
        } 

    }
    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        SqlDataSource1.InsertParameters["vp_date"].DefaultValue = strDate;

        string strBerthTime = ((HtmlInputText)DetailsView1.Controls[0].FindControl("iptBerthTime")).Value;
        SqlDataSource1.InsertParameters["vp_betrthtime"].DefaultValue = strBerthTime;

        string strDepartTime = ((HtmlInputText)DetailsView1.Controls[0].FindControl("iptDepartTime")).Value;
        SqlDataSource1.InsertParameters["vp_departtime"].DefaultValue = strDepartTime;

        string strSide = ((DropDownList)DetailsView1.Controls[0].FindControl("ddlSide")).SelectedValue;
        SqlDataSource1.InsertParameters["vp_berthside"].DefaultValue = strSide;

        string strVesselCode = System.DateTime.Now.Millisecond.ToString();
        SqlDataSource1.InsertParameters["vp_vesselcode"].DefaultValue = strVesselCode;

    }
    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {

    }

    //更新昨日箱量
    protected void Button2_Click(object sender, EventArgs e)
    {
        string sqlInsertPort = @"INSERT INTO [YardPlan].[dbo].[VesselPort]
           ([vp_date]
           ,[vp_length]
           ,[vp_startport]
           ,[vp_vesselcode]
           ,[vp_chname]
           ,[vp_ename]
           ,[vp_ivoyage]
           ,[vp_expvoyage]
           ,[vp_betrthtime]
           ,[vp_departtime]
           ,[vp_prePort]
           ,[vp_nextPort]
           ,[vp_berthside]
           ,[vp_recordtime]
           ,[vp_width]
           ,[vp_endport])
SELECT dateadd(dd,1,[vp_date])
      ,[vp_length]
      ,[vp_startport]
      ,[vp_vesselcode]
      ,[vp_chname]
      ,[vp_ename]
      ,[vp_ivoyage]
      ,[vp_expvoyage]
      ,[vp_betrthtime]
      ,[vp_departtime]
      ,[vp_prePort]
      ,[vp_nextPort]
      ,[vp_berthside]
      ,[vp_recordtime]
      ,[vp_width]
      ,[vp_endport]
  FROM [YardPlan].[dbo].[VesselPort]
where dateadd(dd,1,vp_date)='"+strDate+"'";

        fnMsSetSql(sqlInsertPort);
        GridView1.DataBind();
    }
}

#region
/*
     private string strDate
    {
        set { hdDate.Value = value; }
        get { return hdDate.Value.ToString(); }
    }
  
    private string strShift
    {
        set { hdShift.Value = value; }
        get { return hdShift.Value.ToString(); }
    }

    private string strDay
    {
        set { hdDay.Value = value; }
        get { return hdDay.Value.ToString(); }

    }

    //获取其值。
    private void GetDate()
    {
        strDate = Request.QueryString["date"] + "" == ""
            ? System.DateTime.Now.ToShortDateString()
            : Request.QueryString["date"] + "";
        strShift = Request.QueryString["shift"] + "" == ""
            ? "A" : Request.QueryString["shift"] + "";
        strDay = Request.QueryString["day"] + "" == ""
            ? "0" : Request.QueryString["day"] + "";

        if(strShift=="2")
        {
            strShift = "A";
        }
        else if (strShift == "1")
        {
            strShift = "B";
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetDate();
            string strSelect = SqlDsVessel.SelectCommand;

        }

        SqlDsVessel.SelectParameters["Bvs_date"].DefaultValue = strDate;
        SqlDsVessel.SelectParameters["Bvs_Shift"].DefaultValue = strShift;
        SqlDsVessel.SelectParameters["Bvs_day"].DefaultValue = strDay;
                
        SqlDsVessel.InsertParameters["Bvs_date"].DefaultValue = strDate;
        SqlDsVessel.InsertParameters["Bvs_Shift"].DefaultValue = strShift;
        SqlDsVessel.InsertParameters["Bvs_day"].DefaultValue = strDay;
       // SqlDsVessel.InsertParameters["Bvs_intrace"].DefaultValue = "Y";

  
  }

    //生成船舶信息
    protected void btnCountVessel_Click(object sender, EventArgs e)
    {
        string[,] arrTable = new string[100, 10];
        string strSelectDay="";
        if (strDay == "0")
        {
            strSelectDay = strDate.Substring(0, 10);
        }
        else if (strDay == "1")
        {
            strSelectDay = System.DateTime.Parse(strDate).AddDays(1).ToString("yyyy-MM-dd").Substring(0, 10);
        }

        if ("A" == strShift)
        {
            strSelectDay = strSelectDay + " 08";
        }
        else 
        {
            strSelectDay = strSelectDay + " 20";
        }
        //查询生产库
        string strSql = string.Format(@"  select distinct voc.voc_ocrrid 船ID, vie.vie_vnamecd 船代码
        ,vls.vls_vengnm 英文船名,vls.vls_vchnnm 中文船名
       ,ps_web_get_p_vessel_unit_f(voc.voc_ocrrid) 箱量,nvl(vie.vie_intrade ,'N') 内贸
       ,vie.vie_pwksttm 计划开工, vie.vie_pwkentm 计划完工,vbt.vbt_pbthtm 计划靠泊, vbt.vbt_pdpttm 计划离泊
from ps_vessel_import_exports vie
     ,ps_vessel_occurences voc
     ,ps_vessels vls
     ,ps_vessel_berthes vbt
where vie.vie_vbt_voc_ocrrid=voc.voc_ocrrid
      and vls.vls_vnamecd=voc.voc_vls_vnamecd
      and vbt.vbt_voc_ocrrid=voc.voc_ocrrid
      and vie.vie_pwkentm >=to_date('{0}','yyyy-mm-dd hh24')
      and vie.vie_pwksttm<=to_date('{0}','yyyy-mm-dd hh24')+1/2
      and nvl(vie.vie_intrade,'N')='N' 
      UNION ALL
 select distinct voc.voc_ocrrid 船ID, vie.vie_vnamecd 船代码
        ,vls.vls_vengnm 英文船名,vls.vls_vchnnm 中文船名
       ,ps_web_get_p_vessel_unit_f(voc.voc_ocrrid) 箱量,nvl(vie.vie_intrade ,'N') 内贸
       ,vie.vie_pwksttm 计划开工, vie.vie_pwkentm 计划完工,vbt.vbt_pbthtm 计划靠泊, vbt.vbt_pdpttm 计划离泊
from ps_vessel_import_exports vie
     ,ps_vessel_occurences voc
     ,ps_vessels vls
     ,ps_vessel_berthes vbt
where vie.vie_vbt_voc_ocrrid=voc.voc_ocrrid
      and vls.vls_vnamecd=voc.voc_vls_vnamecd
      and vbt.vbt_voc_ocrrid=voc.voc_ocrrid
      and vls.vls_vnamecd!='NEI MAO'
      and vbt.vbt_pdpttm >=to_date('{0}','yyyy-mm-dd hh24')
      and vbt.vbt_pbthtm<=to_date('{0}','yyyy-mm-dd hh24')+1/2
      and nvl(vie.vie_intrade,'N')='Y' 
           
      ", strSelectDay);

        //判断是否已经存在相关记录，存在则删除后在插入
        string sqlSelect = string.Format(@"
            SELECT [Bvs_date], [Bvs_shift], [Bvs_vnamecd], [Bvs_vchname], [Bvs_intrace], [Bvs_vessel_count]
                             , [Bvs_PlanStartTime], [Bvs_PlanEndTime],Bvs_PlanArriveTime,Bvs_PlanDepartTime
            FROM [Busy_Vessel_Ship]
             WHERE [Bvs_date]='{0}' and  [Bvs_shift]='{1}' and Bvs_day='{2}'"
            , strDate, strShift, strDay);
        string sqlDel = string.Format(@"
            Delete From Busy_Vessel_Ship
             WHERE [Bvs_date]='{0}' and  [Bvs_shift]='{1}' and Bvs_day='{2}'"
            , strDate, strShift, strDay);

        int count = fnMsGetCount(sqlSelect);
        
        if (count != 0)
        {
            fnMsSetSql(sqlDel);
        }

        arrTable = FnGetValue(strSql, arrTable);

        for (int i = 0; i < arrTable.GetLength(0); i++)
        {
            if (!string.IsNullOrEmpty(arrTable[i, 0]))
            {
                string strInsert = string.Format(@"INSERT INTO [Busy_Vessel_Ship]
           ([Bvs_date]    ,[Bvs_day]  ,[Bvs_shift]     ,[Bvs_vnamecd]
           ,[Bvs_vchname]       ,[Bvs_intrace]    ,[Bvs_vessel_count]
           ,[Bvs_PlanStartTime]        ,[Bvs_PlanEndTime],bvs_planarriveTime,Bvs_PlanDepartTime)
     VALUES
           ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", strDate, strDay, strShift, arrTable[i, 1]
                        , arrTable[i, 3], arrTable[i, 5], arrTable[i, 4], arrTable[i, 6], arrTable[i, 7], arrTable[i, 8], arrTable[i, 9]);

                fnMsSetSql(strInsert);

            }
            else
            {
                break;
            }
        }


        //有件杂货跨工班则添加。
        string strInVessel = string.Format(@"
INSERT INTO [Busy_Vessel_Ship]([Bvs_date],[Bvs_day]
      ,[Bvs_shift],[Bvs_vnamecd]      ,[Bvs_vchname],[Bvs_intrace]
      ,[Bvs_shift_count]      ,[Bvs_vessel_count]      ,[Bvs_PlanStartTime]
      ,[Bvs_PlanEndTime]      ,[bvs_recordtime]      ,[Bvs_PlanArriveTime]
      ,[Bvs_PlanDepartTime])
SELECT DISTINCT '{0}','{1}'   ,'{2}'
,[Bvs_vnamecd]      ,[Bvs_vchname],[Bvs_intrace]
      ,[Bvs_shift_count]      ,[Bvs_vessel_count]      ,[Bvs_PlanStartTime]
      ,[Bvs_PlanEndTime]      ,[bvs_recordtime]      ,[Bvs_PlanArriveTime]
      ,[Bvs_PlanDepartTime]
FROM [Busy_Vessel_Ship]
WHERE 
	 [Bvs_PlanArriveTime]<= DateAdd(hour,12,Convert(datetime,'{3}:00',120)) and 
	 [Bvs_PlanDepartTime]>= Convert(DateTime,'{3}:00',120)  AND
    	-- [Bvs_date]='{0}'  AND
	 [Bvs_intrace] is NULL
    ", strDate, strDay, strShift,strSelectDay);

        fnMsSetSql(strInVessel);

        //绑定显示修改后的数据
        gvVessel.DataBind();
 
    }

    //返回记录数
    private int fnMsGetCount(string strSelect)
    {
        int iReturn = 0;
        string strCount = string.Format("SELECT count(*) FROM ({0}) a", strSelect);
        System.Data.SqlClient.SqlConnection sqlConn = new SqlConnection(
            ConfigurationManager.ConnectionStrings["TestControlDailyConnectionString"].ConnectionString);
        System.Data.SqlClient.SqlCommand sqlComm = new SqlCommand(
            strCount, sqlConn);
        sqlConn.Open();
        System.Data.SqlClient.SqlDataReader sqlData = sqlComm.ExecuteReader();

        if (sqlData.Read())
        {
            iReturn = int.Parse(sqlData.GetValue(0).ToString());

        }
        sqlData.Dispose();
        sqlComm.Dispose();
        sqlConn.Dispose();

        return iReturn;

    }

    //返回修改记录数
    private int fnMsSetSql(string strSelect)
    {
        int iReturn = 0;
        string strCount = string.Format(strSelect);
        System.Data.SqlClient.SqlConnection sqlConn = new SqlConnection(
            ConfigurationManager.ConnectionStrings["TestControlDailyConnectionString"].ConnectionString);
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

    //根据插入支线船
    protected void btnShiftVessel_Click(object sender, EventArgs e)
    {
        gvVessel.DataSourceID = "";
        gvVessel.DataBind();
    }
    protected void gvVessel_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Insert")
        {
            gvVessel.DataSourceID = "";
            gvVessel.DataBind();
        }

    }
    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        gvVessel.DataSourceID = SqlDsVessel.ID.ToString();
        gvVessel.DataBind();
    }
    protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
        {
            gvVessel.DataSourceID = SqlDsVessel.ID.ToString();
            gvVessel.DataBind();
        }
        else if (e.CommandName == "Insert")
        {
 
        }
    }
    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {

    }*/
/*
    #region 船舶信息

    public class SafeVesselMod
{
    public SafeVesselMod()
    {

    }

    private string m_VslName;

    public string VslName
    {
        get { return m_VslName; }
        set { m_VslName = value; }
    }
    private string m_VslVoyage;

    public string VslVoyage
    {
        get { return m_VslVoyage; }
        set { m_VslVoyage = value; }
    }
    private string m_Sline;

    public string Sline
    {
        get { return m_Sline; }
        set { m_Sline = value; }
    }
    private string m_Abthtm;

    public string Abthtm
    {
        get { return m_Abthtm; }
        set { m_Abthtm = value; }
    }
    private string m_Adpttm;

    public string Adpttm
    {
        get { return m_Adpttm; }
        set { m_Adpttm = value; }
    }
    private string m_Opsttm;

    public string Opsttm
    {
        get { return m_Opsttm; }
        set { m_Opsttm = value; }
    }
    private string m_Opentm;

    public string Opentm
    {
        get { return m_Opentm; }
        set { m_Opentm = value; }
    }
    private string m_Ropebindtm;

    public string Ropebindtm
    {
        get { return m_Ropebindtm; }
        set { m_Ropebindtm = value; }
    }
    private string m_Laderdowntm;

    public string Laderdowntm
    {
        get { return m_Laderdowntm; }
        set { m_Laderdowntm = value; }
    }
    private string m_Inspecarritm;

    public string Inspecarritm
    {
        get { return m_Inspecarritm; }
        set { m_Inspecarritm = value; }
    }
    private string m_InspecUptm;

    public string InspecUptm
    {
        get { return m_InspecUptm; }
        set { m_InspecUptm = value; }
    }
    private string m_InspecDwontm;

    public string InspecDwontm
    {
        get { return m_InspecDwontm; }
        set { m_InspecDwontm = value; }
    }
    private string m_WorkUpTm;

    public string WorkUpTm
    {
        get { return m_WorkUpTm; }
        set { m_WorkUpTm = value; }
    }

    private string m_LastPort=string.Empty;

    public string LastPort
    {
        get { return m_LastPort; }
        set { m_LastPort = value; }
    }

    private string m_nextPort=string.Empty;

    public string NextPort
    {
        get { return m_nextPort; }
        set { m_nextPort = value; }
    }


    private string m_WorkerDownTm;

    public string WorkerDownTm
    {
        get { return m_WorkerDownTm; }
        set { m_WorkerDownTm = value; }
    }




}
    #endregion

    string m_date = string.Empty;
    string m_shift = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
                
            m_date = Request.QueryString["date"] + "";
            m_shift = Request.QueryString["shift"] + "";
            //m_date = "2012-08-12";
            //m_shift = "1";
            ViewState["date"] = m_date;
            ViewState["shift"] = m_shift;
            if (!string.IsNullOrEmpty(m_date) && !string.IsNullOrEmpty(m_shift))
            {
                BindList(m_date, m_shift);
            }
        }
        else
        {
            int i = 0;
        }
    }

    private void BindList(string p_date, string p_shift)
    {
        List<SafeVesselMod> t_lstSVM = GetlstVesselFromOralce(p_date, p_shift);//Oracle获取
       t_lstSVM = GetlsgVslFrmSqlServer(p_date, p_shift,t_lstSVM);//SQL 获取
        
       //foreach (SafeVesselMod  t_sv in t_lstSVM)
        //for(int i=0;i<t_lstSVM.Count;i++)
       for (int i = t_lstSVM.Count-1; i >= 0; i--) 
       {
           SafeVesselMod t_sv = t_lstSVM[i];
           string strShif = p_shift;
           string strDate = p_date;           
           
           if (string.IsNullOrEmpty(t_sv.Ropebindtm) && // !string.IsNullOrEmpty(t_sv.Opentm)&&
                 !IsPostBack)//下船时间为空且工班的数据
           {
               if (p_shift == "2")//白班
               {
                   strDate = DateTime.Parse(p_date).AddDays(-1).ToShortDateString();
                   strShif = "1";
               }
               else//夜班
               {
                   strShif = "2";
                   strDate = p_date;
               }
               t_sv = GetlsgVslFrmSqlServer(strDate,strShif,t_sv);
               t_lstSVM[i] = t_sv;
               //t_lstSVM = GetlsgVslFrmSqlServer(p_date, p_shift, t_lstSVM);
           }

       }
        this.repVessleInfo.DataSource = t_lstSVM;
        this.repVessleInfo.DataBind();
    }


    // colin 2012-7-11 获取生产库的船舶数据
    private List<SafeVesselMod> GetlstVesselFromOralce(string p_date, string p_shift)
    {
        #region SQL
        string t_sql = @"SELECT voc.voc_vls_vnamecd || case
         when v.intrade is null then
          ''
         else
          ' / 内贸'
       end 船名,
       v.lin 航线,
       voc.voc_ivoyage || ' / ' || voc.voc_expvoyage 航次,
       vb.vbt_abthtm 靠泊时间,
       v.sta 开工时间,
       v.ent 完工时间,
       vb.vbt_adpttm 离泊时间

  from PS_VESSEL_OCCURENCES voc,
       ps_vessel_berthes    vb,
       --ps_v_wroutes_standby_hours gcs,
       --ps_vpc_hpc_vw vpc,
       (select vie_vbt_voc_ocrrid vocid,
               sl.sln_lne_rtcd lin,
               min(vie_awksttm) sta,
               max(vie_awkentm) ent,
               vie.vie_intrade intrade
          from ps_vessel_import_exports vie, ps_service_lines sl
         where sl.sln_slineid = vie.vie_sln_slineid
         group by vie_vbt_voc_ocrrid, sln_lne_rtcd, vie.vie_intrade) v
 where ((vb.vbt_abthtm between
       :time1 and
       :time2) or
       (vb.vbt_adpttm between
       :time1 and
       :time2) or
       (vb.vbt_abthtm <
       :time1 and
       vb.vbt_adpttm is null))
   and vb.vbt_voc_ocrrid = voc.voc_ocrrid
   and v.vocid = voc.voc_ocrrid
   and v.intrade is null
   --and vb.vbt_adpttm is not null
 group by voc.voc_vls_vnamecd,
          v.lin,
          voc.voc_ivoyage,
          voc.voc_expvoyage,
          vb.vbt_abthtm,
          v.sta,
          v.ent,
          VOC_OCRRID,
          VBT_BERTHID,
          vb.vbt_adpttm,
          v.intrade
";
        #endregion
        String t_time1 = string.Empty;
        String t_time2 = string.Empty;
        if (p_shift == "2")
        {
            t_time1 = p_date + " 08:00:00";
            t_time2 = p_date + " 20:00:00";
        }
        else if (p_shift == "1")
        {
            t_time1 = p_date + " 20:00:00";
            t_time2 = Convert.ToDateTime(p_date).AddDays(1).ToString("yyyy-MM-dd") + " 08:00:00";
        }

        OracleParameter[] t_arrParam = {new OracleParameter(),new OracleParameter()};
        t_arrParam[0].ParameterName = ":time1";
        t_arrParam[0].OracleType = OracleType.DateTime;
        t_arrParam[0].Value = t_time1;
        t_arrParam[1].ParameterName = ":time2";
        t_arrParam[1].OracleType = OracleType.DateTime;
        t_arrParam[1].Value = t_time2;
        
        OracleDataReader t_reader = OraHelper.ExecuteReader(OraHelper.ConnectionStringLocalTransaction,CommandType.Text,t_sql,t_arrParam);
        List<SafeVesselMod> t_lstSV = new List<SafeVesselMod>();
        while(t_reader.Read())
        {
            SafeVesselMod t_sv = new SafeVesselMod();
            t_sv.VslName = t_reader.GetString(0);
            t_sv.VslVoyage = t_reader.GetString(2);
            t_sv.Sline = t_reader.GetString(1);
            t_sv.Abthtm = t_reader.IsDBNull(3) ? "" : t_reader.GetDateTime(3).ToString("MM-dd HH:mm");
            t_sv.Opsttm = t_reader.IsDBNull(4) ? "" : t_reader.GetDateTime(4).ToString("MM-dd HH:mm");
            t_sv.Opentm = t_reader.IsDBNull(5) ? "" : t_reader.GetDateTime(5).ToString("MM-dd HH:mm");
            t_sv.Adpttm = t_reader.IsDBNull(6) ? "" : t_reader.GetDateTime(6).ToString("MM-dd HH:mm");
            t_lstSV.Add(t_sv);
        }
        t_reader.Close();
        return t_lstSV;
    }

    // colin 2012-7-11 从SQLSERVER获取手动输入的时间
    private List<SafeVesselMod> GetlsgVslFrmSqlServer(string p_date, string p_shift, List<SafeVesselMod> t_lstSVM)
    {
        //List<SafeVesselMod> t_lstSVM = GetlstVesselFromOralce(p_date, p_shift);
        //change this  to add value
        #region SQL
        String t_sql = @"SELECT [SV_ID]      ,[SV_DATE]      ,[SV_SHIFT]
      ,[SV_VNAMECD]      ,[SV_VOYAGE]      ,[SV_SLINE]      ,[SV_PORT]
      ,[SV_ABTHTM]      ,[SV_OPSTTM]      ,[SV_OPEDTM]      ,[SV_ADPTTM]
      ,[SV_ROPEBINDTM]      ,[SV_LADERDOWNTM]      ,[SV_INSPECARRITM]
      ,[SV_INSPECUPTM]      ,[SV_INSPECDOWNTM]      ,[SV_WORKERUPTM]
      ,[SV_RECORDTM]      ,[SV_WORKERDOWN]      ,[SV_NEXTPORT]
  FROM [SAFETY_VESSEL] sv 
    where sv.sv_date=@date and sv_shift=@shift and sv.sv_vnamecd=@vnamecd 
            and sv.sv_voyage=@voyage";

        #endregion

        foreach(SafeVesselMod t_svm in t_lstSVM)
        {
            SqlParameter[] t_arrParram = { new SqlParameter(), new SqlParameter(), new SqlParameter(),new SqlParameter() };
            t_arrParram[0].ParameterName = "@date";
            t_arrParram[0].Value = p_date;
            t_arrParram[1].ParameterName = "@shift";
            t_arrParram[1].Value = p_shift;
            t_arrParram[2].ParameterName = "@vnamecd";
            t_arrParram[2].Value = t_svm.VslName;
            t_arrParram[3].ParameterName = "@voyage";
            t_arrParram[3].Value = t_svm.VslVoyage;

            SqlDataReader t_reader = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, t_sql, t_arrParram);
            if (t_reader.HasRows)
            {
                t_reader.Read();
                t_svm.LastPort = (t_reader.IsDBNull(6) ? "" : t_reader.GetString(6)).Replace(" ","");
                t_svm.Ropebindtm = t_reader.IsDBNull(11) ? "" : t_reader.GetString(11);
                t_svm.Laderdowntm = t_reader.IsDBNull(12) ? "" : t_reader.GetString(12);
                t_svm.Inspecarritm = t_reader.IsDBNull(13) ? "" : t_reader.GetString(13);
                t_svm.InspecUptm = t_reader.IsDBNull(14) ? "" : t_reader.GetString(14);
                t_svm.InspecDwontm = t_reader.IsDBNull(15) ? "" : t_reader.GetString(15);
                t_svm.WorkUpTm = t_reader.IsDBNull(16) ? "" : t_reader.GetString(16);
                t_svm.NextPort = (t_reader.IsDBNull(19) ? "" : t_reader.GetString(19)).Replace(" ","");
                t_svm.WorkerDownTm = t_reader.IsDBNull(18) ? "" : t_reader.GetString(18);
                t_reader.Close();
            }           
        }
        return t_lstSVM;
    }

    //获取SqlServer的时间
    private SafeVesselMod GetlsgVslFrmSqlServer(string p_date, string p_shift, SafeVesselMod t_lstSVM)
    {
        //List<SafeVesselMod> t_lstSVM = GetlstVesselFromOralce(p_date, p_shift);
        //change this  to add value
        #region SQL
        String t_sql = @"SELECT [SV_ID]      ,[SV_DATE]      ,[SV_SHIFT]
      ,[SV_VNAMECD]      ,[SV_VOYAGE]      ,[SV_SLINE]      ,[SV_PORT]
      ,[SV_ABTHTM]      ,[SV_OPSTTM]      ,[SV_OPEDTM]      ,[SV_ADPTTM]
      ,[SV_ROPEBINDTM]      ,[SV_LADERDOWNTM]      ,[SV_INSPECARRITM]
      ,[SV_INSPECUPTM]      ,[SV_INSPECDOWNTM]      ,[SV_WORKERUPTM]
      ,[SV_RECORDTM]      ,[SV_WORKERDOWN]      ,[SV_NEXTPORT]
  FROM [SAFETY_VESSEL] sv 
    where sv.sv_date=@date and sv_shift=@shift and sv.sv_vnamecd=@vnamecd 
            and sv.sv_voyage=@voyage";

        #endregion
        
        //foreach (SafeVesselMod t_svm in t_lstSVM)
        {
            SafeVesselMod t_svm = t_lstSVM;

            SqlParameter[] t_arrParram = { new SqlParameter(), new SqlParameter(), new SqlParameter(), new SqlParameter() };
            t_arrParram[0].ParameterName = "@date";
            t_arrParram[0].Value = p_date;
            t_arrParram[1].ParameterName = "@shift";
            t_arrParram[1].Value = p_shift;
            t_arrParram[2].ParameterName = "@vnamecd";
            t_arrParram[2].Value = t_svm.VslName;
            t_arrParram[3].ParameterName = "@voyage";
            t_arrParram[3].Value = t_svm.VslVoyage;

            SqlDataReader t_reader = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, t_sql, t_arrParram);
            if (t_reader.HasRows)
            {
                t_reader.Read();
                t_svm.LastPort = (t_reader.IsDBNull(6) ? "" : t_reader.GetString(6)).Replace(" ", "");
                t_svm.Ropebindtm = t_reader.IsDBNull(11) ? "" : t_reader.GetString(11);
                t_svm.Laderdowntm = t_reader.IsDBNull(12) ? "" : t_reader.GetString(12);
                t_svm.Inspecarritm = t_reader.IsDBNull(13) ? "" : t_reader.GetString(13);
                t_svm.InspecUptm = t_reader.IsDBNull(14) ? "" : t_reader.GetString(14);
                t_svm.InspecDwontm = t_reader.IsDBNull(15) ? "" : t_reader.GetString(15);
                t_svm.WorkUpTm = t_reader.IsDBNull(16) ? "" : t_reader.GetString(16);
                t_svm.NextPort = (t_reader.IsDBNull(19) ? "" : t_reader.GetString(19)).Replace(" ", "");
                t_svm.WorkerDownTm = t_reader.IsDBNull(18) ? "" : t_reader.GetString(18);
                t_reader.Close();
            }

            t_lstSVM = t_svm;
        }
        return t_lstSVM;
    }

    private bool isExist(string p_date, string p_shift, string p_vnamced, string p_voyage)
    {
        #region SQL
        String t_sql = @"select * from safety_vessel sv where sv.sv_date=@date and sv_shift=@shift and sv.sv_vnamecd=@vnamecd and sv.sv_voyage=@voyage";
        #endregion
        SqlParameter[] t_arrParram = { new SqlParameter(), new SqlParameter(), new SqlParameter(), new SqlParameter() };
        t_arrParram[0].ParameterName = "@date";
        t_arrParram[0].Value = p_date;
        t_arrParram[1].ParameterName = "@shift";
        t_arrParram[1].Value = p_shift;
        t_arrParram[2].ParameterName = "@vnamecd";
        t_arrParram[2].Value = p_vnamced;
        t_arrParram[3].ParameterName = "@voyage";
        t_arrParram[3].Value = p_voyage;

        SqlDataReader t_reader = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction,CommandType.Text,t_sql,t_arrParram);
        bool t_result = t_reader.HasRows;
        t_reader.Close();
        return t_result;
    }

    private void Insert(string p_date, string p_shift, SafeVesselMod p_svm)
    {
        #region SQL

        String t_sql = "insert into safety_vessel values (@date,@shift,@vname,@voyage,@sline,@lastport,@abthtm,@opsttm,@opedtm,@adpttm,@ropetm,@ladetm,@spectarrtm,@spectuptm,@spectdowntm,@workerupwork,getdate(),@workerdown,@nextport)";
        #endregion
        SqlParameter[] t_arrParam = {new SqlParameter("@date",SqlDbType.DateTime),new SqlParameter("@shift",p_shift),new SqlParameter("@vname",p_svm.VslName),new SqlParameter("@voyage",p_svm.VslVoyage),
            new SqlParameter("@sline",p_svm.Sline),new SqlParameter("@lastport",p_svm.LastPort),new SqlParameter("@abthtm",p_svm.Abthtm),new SqlParameter("@adpttm",p_svm.Adpttm),
        new SqlParameter("@opsttm",p_svm.Opsttm),new SqlParameter("@opedtm",p_svm.Opentm),new SqlParameter("@ropetm",p_svm.Ropebindtm),new SqlParameter("@ladetm",p_svm.Laderdowntm),
            new SqlParameter("@spectarrtm",p_svm.Inspecarritm),new SqlParameter("@spectuptm",p_svm.InspecUptm),new SqlParameter("@spectdowntm",p_svm.InspecDwontm),new SqlParameter("@workerupwork",p_svm.WorkUpTm), new SqlParameter("@nextport",p_svm.NextPort),new SqlParameter("@workerdown",p_svm.WorkerDownTm)};
        t_arrParam[0].Value = p_date;
        SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, t_sql, t_arrParam);
        
    }

    private void DeleteByDate(string p_date, string p_shift)
    {
    #region SQL
        string t_sql = "delete from safety_vessel  where sv_date=@date and sv_shift=@shift";
    #endregion
        SqlParameter[] t_arrParam = { new SqlParameter(), new SqlParameter() };
        t_arrParam[0].ParameterName = "@date";
        t_arrParam[0].Value = p_date;
        t_arrParam[1].ParameterName = "@shift";
        t_arrParam[1].Value = p_shift;

        SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, t_sql, t_arrParam);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string t_result = this.hdTableValue.Value;
        string[] t_arrResult = t_result.Split('|');

        m_date = ViewState["date"].ToString();
        m_shift = ViewState["shift"].ToString();
        DeleteByDate(m_date, m_shift);

        for (int i = 0; i < t_arrResult.Length-1 ;++i )
        {
            String[] t_item = t_arrResult[i].Split(',');
            SafeVesselMod t_SVM = new SafeVesselMod();
            t_SVM.VslName = t_item[0];
            t_SVM.Sline = t_item[1];
            t_SVM.VslVoyage = t_item[2];
            t_SVM.Abthtm = t_item[3] == "&nbsp;" ? "" : t_item[3];
            t_SVM.Opsttm = t_item[4] == "&nbps;"? "" :t_item[4];
            t_SVM.Opentm = t_item[5]=="&nbsp;"? "": t_item[5];
            t_SVM.Adpttm = t_item[6] == "&nbsp;" ? "" : t_item[6]; 
            t_SVM.LastPort = t_item[7].Replace(" ","");
            t_SVM.NextPort = t_item[8].Replace(" ", "");
            t_SVM.Ropebindtm = t_item[9] == "&nbsp;" ? "" : t_item[9]; ;
            t_SVM.Laderdowntm = t_item[10] == "&nbsp;" ? "" : t_item[10]; ;
            t_SVM.Inspecarritm = t_item[11] == "&nbsp;" ? "" : t_item[11]; ;
            t_SVM.InspecUptm = t_item[12] == "&nbsp;" ? "" : t_item[12]; ;
            t_SVM.InspecDwontm = t_item[13] == "&nbsp;" ? "" : t_item[13]; ;
            t_SVM.WorkUpTm = t_item[14] == "&nbsp;" ? "" : t_item[14]; ;
            t_SVM.WorkerDownTm = t_item[15] == "&nbsp;" ? "" : t_item[15]; ;
            Insert(m_date, m_shift, t_SVM);
        }

        //重新绑定
        BindList(m_date, m_shift);
    }
}
*/
#endregion
