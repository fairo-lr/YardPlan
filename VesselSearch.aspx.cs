using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Drawing;
using System.Data.OleDb;

public partial class BusySearch : System.Web.UI.Page
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

    public static string strDate = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        fnBindDate();
        fnVesselInfo();
    }

    /// <summary>
    /// 绑定日期
    /// </summary>
    /// <param name="strDate"></param>
    private void fnVesselInfo()
    {
        int i = gvInfo.Rows.Count;
        if (i > 0)
        {
            lblDate.Text = gvInfo.Rows[0].Cells[1].Text.Substring(0, 10);
            lblSuper.Text = gvInfo.Rows[0].Cells[2].Text;
            lblSuperVisor.Text = gvInfo.Rows[0].Cells[3].Text;
            lblFinalTime.Text = gvInfo.Rows[0].Cells[4].Text;
        }
    
    }

    /// <summary>
    /// 绑定日期
    /// </summary>
    private void fnBindDate()
    {
        if (IsPostBack)
        { }
        strDate = Request.QueryString["date"] + "" == ""
          ? System.DateTime.Now.ToString("yyyy-MM-dd")
          : Request.QueryString["date"] + "";
        hdDate.Value = strDate;
        SqlDsInformation.SelectParameters["date"].DefaultValue = strDate;

        SqlDsInformation.DataBind();
        gvInfo.DataSourceID = SqlDsInformation.ID;
        gvInfo.DataBind();
        SqlDsTide.SelectParameters["date"].DefaultValue = strDate;
        SqlDsTide.DataBind();
        gvTide.DataSourceID = SqlDsTide.ID;
        gvTide.DataBind();
        SqlDsVesselWeek.SelectParameters["date"].DefaultValue = strDate;
        SqlDsVesselWeek.DataBind();
        gvVesselWeek.DataSourceID = SqlDsVesselWeek.ID;
        gvVesselWeek.DataBind();
       
    }   


    //没有数据换一个有一行空数据的数据源
    public void fnNoRecordBuild(GridView gridView, SqlDataSource sqlds)
    {
        SqlConnection connect = new SqlConnection(sqlds.ConnectionString);

        connect.Open();
        DataTable dt = new DataTable();
        System.Data.SqlClient.SqlDataAdapter adpter = new System.Data.SqlClient.SqlDataAdapter(sqlds.SelectCommand, connect);
        adpter.Fill(dt);
        DataRow dr = dt.NewRow();
        dt.Clear();
        dt.Rows.Add(dr);
        gridView.DataSourceID = "";//绑定新数据源前先将原来的清理
        gridView.DataSource = dt;       
        gridView.DataBind();

    }

    //上个工班
    protected void btnBefore_Click(object sender, EventArgs e)
    {
        string strUrl = "VesselSearch.aspx?date=" + DateTime.Parse(strDate).AddDays(-1).ToString("yyyy-MM-dd");
        Response.Redirect(strUrl);
    }

    //下个工班
    protected void btnAfter_Click(object sender, EventArgs e)
    {
        string strUrl = "VesselSearch.aspx?date=" + DateTime.Parse(strDate).AddDays(1).ToString("yyyy-MM-dd");
        Response.Redirect(strUrl);
    }
}
#region Delete
    
//    #region  全船作业信息的SQL语句

//    public string m_sqlAllVessel = @"SELECT voc.voc_vls_vnamecd 船名,
//       v.lin 航线,
//       voc.voc_ivoyage || ' / ' || voc.voc_expvoyage 航次,
//       vb.vbt_abthtm 靠泊时间,
//       v.sta 开工时间,
//       v.ent 完工时间,
//       vb.vbt_adpttm 离泊时间,
//       count(distinct vma.VMA_GCN_GCRANENO) 作业路数,
//       count(distinct tdl.tdl_trkno) 内拖数,
//       PS_S_STS_export_WITH_RE_RF_F(VOC_OCRRID) 卸船量,
//       PS_S_STS_IMPORT_WITH_RE_RF_F(VOC_OCRRID) 装船量,
//       ROUND(PS_S_GET_BTH_CNO_F(VOC_OCRRID, VBT_BERTHID) /
//             PS_S_GET_QC_OPTM_F(VOC_OCRRID, VBT_BERTHID),
//             2) 毛台时量,
//       DECODE(PS_S_GET_VOC_OPTM_F(VOC_OCRRID, VBT_BERTHID),
//              0,
//              0,
//              ROUND(PS_S_GET_BTH_CNO_F(VOC_OCRRID, VBT_BERTHID) /
//                    PS_S_GET_VOC_AOPTM_F(VOC_OCRRID, VBT_BERTHID),
//                    2)) 作业船时,
//        VDYY.VDYTIME 船舶待时,
//       --nvl(sum(round(TO_NUMBER((DECODE(gcs.gcs_stdbyedtm, null, sysdate, gcs.gcs_stdbyedtm) -gcs.gcs_stdbysttm) * 24),2)),0) 作业路待时,
//       nvl(wrstandby.故障待时, 0) 作业路待时,
//       PS_S_get_plan_cntno_F(VOC_OCRRID, 'E') || ' / ' ||
//       PS_S_get_plan_cntno_F(VOC_OCRRID, 'I') 装卸船计划量,
//       ROUND(PS_S_GET_BTH_CNO_F(VOC_OCRRID, VBT_BERTHID) /
//             PS_S_GET_QC_OPTM_F(VOC_OCRRID, VBT_BERTHID),
//             2) 毛台时量
//
//  from PS_VESSEL_OCCURENCES   voc,
//       ps_vessel_berthes      vb,
//       ps_vessel_delayes      VDY,
//       PS_VMA_GOR_VW          vma,
//       PS_TRUCK_DISPATCH_LOGS tdl,
//       --ps_v_wroutes_standby_hours gcs,
//       --ps_vpc_hpc_vw vpc,
//       (select vie_vbt_voc_ocrrid vocid,
//               sl.sln_lne_rtcd lin,
//               min(vie_awksttm) sta,
//               max(vie_awkentm) ent
//          from ps_vessel_import_exports vie, ps_service_lines sl
//         where sl.sln_slineid = vie.vie_sln_slineid
//         group by vie_vbt_voc_ocrrid, sln_lne_rtcd) v,
//       (select vma_vbt_voc_ocrrid, count(distinct vma_gcn_gcraneno) mco
//          FROM PS_VMA_GOR_VW
//         group by vma_vbt_voc_ocrrid) t,
//       (select sum(round(to_number(gcs.gcs_stdbyedtm - gcs.gcs_stdbysttm) * 24,
//                         2)) 故障待时,
//               vma.VMA_VBT_VOC_OCRRID standbyvocid
//          from ps_v_wroutes_standby_hours gcs, PS_VMA_GOR_VW vma
//         where vma.VMA_VWKRTID = gcs.gcs_vma_vwkrtid
//              --and vma.VMA_VBT_VOC_OCRRID = 116498
//           and gcs_stdbyrn = 'A4'
//         group by vma.VMA_VBT_VOC_OCRRID) wrstandby,
//(SELECT sum(round(TO_NUMBER((DECODE(VDY.VDY_DLYEDTM,
//                                           null,
//                                           sysdate,
//                                           VDY.VDY_DLYEDTM) -
//                                   VDY.VDY_DLYSTTM) * 24),
//                         2)) VDYTIME,
//               VDY.VDY_VBT_VOC_OCRRID VDYVOCID
//          FROM ps_vessel_delayes VDY
//         GROUP BY VDY.VDY_VBT_VOC_OCRRID) VDYY
// where 
//        ^TimeCondition^
//   and vb.vbt_voc_ocrrid = voc.voc_ocrrid
//   and v.vocid = voc.voc_ocrrid
//AND VDYY.VDYVOCID(+) = voc.voc_ocrrid
//   and vdy.vdy_vbt_voc_ocrrid(+) = voc.voc_ocrrid
//      --and gcs.gcs_vma_vwkrtid(+) = vma.VMA_VWKRTID
//   and vma.VMA_VBT_VOC_OCRRID = voc.voc_ocrrid
//   and tdl.tdl_wkrtid = vma.VMA_VWKRTID
//      --and vpc.VPC_VOC_OCRRID = voc.voc_ocrrid
//   and t.vma_vbt_voc_ocrrid = voc.voc_ocrrid
//      --and vpc_lduldtm is not null
//      --and gcs.gcs_stdbyrn(+) = 'A4' --桥吊故障
//   and wrstandby.standbyvocid(+) = voc.voc_ocrrid
//    and vb.vbt_adpttm is not null
// group by voc.voc_vls_vnamecd,
//          v.lin,
//          voc.voc_ivoyage,
//          voc.voc_expvoyage,
//          vb.vbt_abthtm,
//          v.sta,
//          v.ent,
//          t.mco,
//          VOC_OCRRID,
//          VBT_BERTHID,
//          wrstandby.故障待时,
//          vb.vbt_adpttm,VDYY.VDYTIME
//";

//    #endregion

//    #region 工程部机械故障SQL语句
//    string m_engineerMachineBreak = @"select tb.device_code  设备编号,
//       to_char(tb.kg_date,'yyyy-mm-dd hh24:mi')      报修时间,
//       to_char(tb.wg_date,'yyyy-mm-dd hh24:mi')         修复时间,
//       tb.wx_ghours    维修用时,
//       tb.malacc_cause 故障类型,
//       tb.wx_man       维修人,
//       tb.desc_xx      故障现象,
//       tb.wx_qk        故障处理过程,
//       tb.ylwd_yjy     遗留问题建议,
//       tb.memo         备注
//  from rep_rush tb
// where tb.kg_date between
//       ^TimeCondition^
// order by tb.tb_date ";
//    #endregion

//    #region 操作不TOPS故障登记SQL语句
//    string m_operTopsMachineBreak = @"select * from (
//(select ymm.ymm_ymc_mchno 设备,
//       ymm.ymm_mtncontent 故障原因,
//       to_char(ymm.ymm_mtnpsttm, 'yyyy-mm-dd HH24:mi') 故障开始时间,
//       to_char(decode(ymm.ymm_mtnpedtm, null, sysdate, ymm.ymm_mtnpedtm),
//               'yyyy-mm-dd HH24:mi') 故障结束时间,
//       round((decode(ymm.ymm_mtnpedtm, null, sysdate, ymm.ymm_mtnpedtm) -
//             ymm.ymm_mtnpsttm) * 24,
//             2) 故障时间
//  from ps_ymachine_maintenance_plans ymm
// where ymm.ymm_mtnpsttm between
//       ^TimeCondition^
//)
//          UNION all
//(SELECT GCS.GCS_GCN_GCRANENO 设备,
//       '桥吊故障',
//       to_char(gcs.gcs_stdbysttm, 'yyyy-mm-dd HH24:mi') 故障开始时间,
//       to_char(decode(gcs.gcs_stdbyedtm, null, sysdate, gcs.gcs_stdbyedtm),
//               'yyyy-mm-dd HH24:mi') 故障结束时间,
//       round((decode(gcs.gcs_stdbyedtm, null, sysdate, gcs.gcs_stdbyedtm) -
//             gcs.gcs_stdbysttm) * 24,
//             2) 故障时间
//  FROM PS_V_WROUTES_STANDBY_HOURS GCS
// WHERE GCS.GCS_STDBYSTTM BETWEEN
//       ^TimeCondition^
//   and gcs.gcs_stdbyrn = 'A4'
//)) order by 故障开始时间";
//    #endregion



////绑定合计总量，分别显示。
//private void fnBindVessel()
//{
//    string strConn = ConfigurationManager.ConnectionStrings["TestControlDailyConnectionString"].ConnectionString;
//    string strComm = SqlDsDuty.SelectCommand;
//    string strVelWorkTTL="";//船舶总作业量
//    string strPlanTimeTTL="";//总作业时间
//    string strAvgWorkTime="";//平均效率
//    string strVesselTTL="";//总作业量


//    //"SELECT [SD_ID], [SD_DATE], [SD_SHIFT], [SD_WKNO], [SD_MANAGER], [SD_SUPERVISOR], [SD_CONTROL], [SD_STOWPLAN],"+
//    //    " [SD_COODINATOR], [SD_CAB_MOR], [SD_CRANE], [SD_RTG], [SD_RSEH], [SD_LABOR], [SD_IN_TRK], [SD_FOREMAN], "+
//    //    "[SD_GATE], [SD_VELWORKTTL], [SD_WORKTIMETTL], [SD_AVGWORKTIME], [SD_VESSELTTL], [SD_FINALSTATE] "+
//    //    "FROM [SHIFT_DUTY] WHERE sd_date='2011-11-25' AND sd_shift='2' AND sd_wkno='1'  ORDER BY SD_ID DESC"
//    try
//    {
//        SqlConnection sqlConn = new SqlConnection(strConn);
//        SqlCommand sqlComm = new SqlCommand(strComm, sqlConn);
//        sqlConn.Open();
//        SqlDataReader sqlData = sqlComm.ExecuteReader();
//        if (sqlData.Read())
//        {
//           strVelWorkTTL=sqlData.GetValue(17).ToString();
//            strPlanTimeTTL=sqlData.GetValue(18).ToString();
//            strAvgWorkTime=sqlData.GetValue(19).ToString();
//            strVesselTTL=sqlData.GetValue(20).ToString();

//        }
//        sqlData.Dispose();
//        sqlComm.Dispose();
//        sqlConn.Dispose();

//    }
//    catch (Exception exp)
//    {
//        Response.Write(exp.Message.ToString());
//    }
//   // lblVelWorkTTL.Text = strVelWorkTTL;
//    //lblYardTimeTTL.Text = strPlanTimeTTL;
//    //lblAvgWorkTime.Text = strAvgWorkTime;
// //   lblVesselTTL.Text = strVesselTTL;
//    if (strVelWorkTTL != "" && strVesselTTL != "")
//    {
//   //     lblShifWorkTotal.Text = Convert.ToString(Convert.ToInt32(strVesselTTL) - Convert.ToInt32(strVelWorkTTL));
//    }
//   // lblVesselWorkTTL.Text = strVelWorkTTL;


//}

//绑定获取最终时间




//private void fnBindFinalTime()
//{
//    string strConn = ConfigurationManager.ConnectionStrings["TestControlDailyConnectionString"].ConnectionString;
//    string strComm = string.Format("SELECT si_finaltime FROM shift_information "+
//        "WHERE si_date='{0}' AND si_shift='{1}'", strDateTime, strShift);
//    string strInfoTime = "";

//    try
//    {
//        SqlConnection sqlConn = new SqlConnection(strConn);
//        SqlCommand sqlComm = new SqlCommand(strComm, sqlConn);
//        sqlConn.Open();
//        SqlDataReader sqlData = sqlComm.ExecuteReader();
//        if (sqlData.Read())
//        {
//            strInfoTime=Convert.ToDateTime(sqlData.GetValue(0)).ToString();
//        }
//        sqlData.Dispose();
//        sqlComm.Dispose();
//        sqlConn.Dispose();

//    }
//    catch (Exception exp)
//    {
//        Response.Write(exp.Message.ToString());            
//    }
//    lblFinalTime.Text = strInfoTime;
//}



////显示标题头ToBeContinue
//private void fnShowGridViewTitle(GridView gvTemp)
//{
//    DataTable dt = new DataTable();
//    dt.Columns.Add(" 1");
//    dt.Columns.Add("2");

//    gvTemp.DataSource = dt;
//    gvTemp.DataBind();
//    if (dt.Rows.Count == 0)
//    {
//        dt.Rows.Add(dt.NewRow());
//    }

//    //if (gvTemp.Rows.Count == 1)
//    //{
//    //    int iCount = gvTemp.Columns.Count;
//    //    gvTemp.Rows[0].Cells.Clear();
//    //    gvTemp.Rows[0].Cells.Add(new TableCell());
//    //    gvTemp.Rows[0].Cells[0].ColumnSpan = iCount;
//    //    gvTemp.Rows[0].Cells[0].Text = "None Message!";
//    //    gvTemp.Rows[0].Cells[0].Style.Add("text-align", "center");
//    //}
//}

////显示标题头//UnFinish
//private void fnShowGridViewTitle1(GridView gvTemp)
//{

//    Table tbl = new Table();
//    TableRow row = new TableRow();

//    tbl.CssClass = gvTemp.CssClass;
//    tbl.GridLines = gvTemp.GridLines;
//    tbl.BorderStyle = gvTemp.BorderStyle;
//    tbl.BorderWidth = gvTemp.BorderWidth;
//    tbl.CellPadding = gvTemp.CellPadding;
//    tbl.CellSpacing = gvTemp.CellSpacing;
//    tbl.HorizontalAlign = gvTemp.HorizontalAlign;
//    tbl.Width = gvTemp.Width;

//    foreach (DataControlField f in gvTemp.Columns)
//    {
//        TableCell cell = new TableCell();
//        cell.Text = f.HeaderText;
//        cell.CssClass = "cellCss";
//        row.Cells.Add(cell);
//    }

//    TableRow row2 = new TableRow();
//    tbl.Rows.Add(row2);

//    TableCell msgCell = new TableCell();

//    if (gvTemp.EmptyDataTemplate != null)
//    {
//        gvTemp.EmptyDataTemplate.InstantiateIn(msgCell);
//    }
//    else
//    {
//        msgCell.Text = gvTemp.EmptyDataText;
//    }



//    //gvTemp.HeaderRow = (GridViewRow)row;
//    GridViewRow gvRow = gvTemp.HeaderRow;


//    gvTemp.EmptyDataText = "wushuju";
//    //gvTemp.ShowHeader = true;
//    //  gvLine.EmptyDataRowStyle = true;
//}



//        //全船作业
//        string t_timeCondition = "";
//        string t_engineerMachineBreakCondition = "";
//        if (shift=="1")
//        {
//            t_timeCondition = @"    ((vb.vbt_abthtm between to_date('"+dateTime+@" 20:01:00','yyyy-mm-dd hh24:mi:ss') and to_date('"+ Convert.ToDateTime(dateTime).AddDays(1).ToString("yyyy-MM-dd") +@" 08:00:00','yyyy-mm-dd hh24:mi:ss')) or 
//   (vb.vbt_adpttm between to_date('" + dateTime + " 20:01:00','yyyy-mm-dd hh24:mi:ss') and to_date('" + Convert.ToDateTime(dateTime).AddDays(1).ToString("yyyy-MM-dd") + " 08:00:00','yyyy-mm-dd hh24:mi:ss')) or (vb.vbt_abthtm < to_date('" + dateTime + " 20:01:00', 'yyyy-mm-dd hh24:mi:ss') and vb.vbt_adpttm is null)) ";

//            t_engineerMachineBreakCondition = "to_date('" + dateTime + " 20:00:00','yyyy-mm-dd HH24:mi:ss') and to_date('" + Convert.ToDateTime(dateTime).AddDays(1).ToString("yyyy-MM-dd") + " 08:00:00','yyyy-mm-dd HH24:mi:ss')";

//        }
//        else
//        {
//            t_timeCondition = @"    ((vb.vbt_abthtm between to_date('" + dateTime + @" 08:01:00','yyyy-mm-dd hh24:mi:ss') and to_date('" + dateTime + @" 20:00:00','yyyy-mm-dd hh24:mi:ss')) or 
//   (vb.vbt_adpttm between to_date('" + dateTime + " 08:01:00','yyyy-mm-dd hh24:mi:ss') and to_date('" + dateTime + " 20:00:00','yyyy-mm-dd hh24:mi:ss'))  or (vb.vbt_abthtm < to_date('" + dateTime + " 08:01:00', 'yyyy-mm-dd hh24:mi:ss') and vb.vbt_adpttm is null)) ";

//            t_engineerMachineBreakCondition = "to_date('" + dateTime + " 08:00:00','yyyy-mm-dd HH24:mi:ss') and to_date('" + dateTime+ " 20:00:00','yyyy-mm-dd HH24:mi:ss')";

//        }
//        string strAllVessele = m_sqlAllVessel.Replace("^TimeCondition^", t_timeCondition);
//        //this.SqlDsAllVessel.SelectCommand = strAllVessele;

//        //工程部机械故障记录
//        string strEngMachineBreak = m_engineerMachineBreak.Replace("^TimeCondition^", t_engineerMachineBreakCondition);
//       // this.SqlDsEngineerMachineBreak.SelectCommand = strEngMachineBreak;

//        //TOPS机械故障记录
//        string strTopsMachineBreak = m_operTopsMachineBreak.Replace("^TimeCondition^", t_engineerMachineBreakCondition);
//       // this.SqlDsTopsMachineBread.SelectCommand = strTopsMachineBreak;


// ////值班人员信息
// //string strSDSql = string.Format("SELECT [SD_ID], [SD_DATE], [SD_SHIFT], [SD_WKNO], [SD_MANAGER], [SD_SUPERVISOR], [SD_CONTROL], [SD_STOWPLAN], [SD_COODINATOR], [SD_CAB_MOR], [SD_CRANE], [SD_RTG], [SD_RSEH], [SD_LABOR], [SD_IN_TRK], [SD_FOREMAN], [SD_GATE], [SD_VELWORKTTL], [SD_WORKTIMETTL], [SD_AVGWORKTIME], [SD_VESSELTTL], [SD_FINALSTATE] FROM [SHIFT_DUTY] WHERE sd_date='{0}' AND sd_shift='{1}'   ORDER BY SD_ID DESC", strDateTime, strShift, strPlanNo);
// //SqlDsDuty.SelectCommand = strSDSql;
// //船舶信息
// string strSVSql = string.Format("SELECT [SV_ID], [SV_DATE], [SV_SHIFT], [SV_WKNO], [SV_VNAMECD], [SV_VOYAGE], [SV_SLINE], [SV_ABTHTM], [SV_OPSTTM], [SV_OPEDTM], [SV_ADPTTM], [SV_CRANENUM], [SV_TRUCKNUM], [SV_LOADUNI], [SV_DISUNI], [SV_GORTIME],[SV_ALLGORTIME], [SV_PRETIME], [SV_AVATIME], [SV_GOALTIME], [SV_HATCH], [SV_NOTE], [SV_FINALSTATE], [SV_VDELAY_HOURS], [SV_GSTAN_HOURS], [SV_ISCARGO] FROM [SHIFT_VESSEL] WHERE sv_date='{0}' AND sv_shift='{1}'   ORDER BY SV_ID DESC", strDateTime, strShift, strPlanNo);
//// SqlDsLine.SelectCommand = strSVSql;

// //工作机械
// string strSWSql = string.Format("SELECT [SW_ID], [SW_DATE], [SW_SHIFT], [SW_WKNO], [SW_MV], [SW_RV],[SW_P_RV], convert(varchar(10),[SW_P_RV]) +' / '+ convert(varchar(10),[SW_RV]) SW_PRV, [SW_CK], [SW_IN], [SW_OUT], [SW_DT], [SW_XZ],[SW_P_XZ], convert(varchar(10),[SW_P_XZ])+' / ' + convert(varchar(10),[SW_XZ]) SW_PXZ, [SW_ZZ], [SW_FINALSTATE], [SW_OUT_TIME_TRK], [SW_IN_OUT],[SW_LASTSHIFT_TIMEOUT],[SW_LASTSHIFT_CKTIMEOUT], [SW_CK_TIMEOUT] FROM [SHIFT_WORKS] WHERE sw_date='{0}' AND SW_SHIFT='{1}'  ORDER BY sw_id DESC ", strDateTime, strShift, strPlanNo);
// SqlDsTide.SelectCommand = strSWSql;

//if (Request["workno"] != strPlanNo && (Request["workno"] + "") != "")
//{
//    strPlanNo = Request["workno"];
//}

//班组没有限制colin
//if (strDateTime == "" || strShift == "" || strPlanNo == "")



////获取工班信息。
//private string fnGetWorkNo(string shift, string dateTime)
//{
//    string strCheckWorkNo = "";
//    string strCheckShift = shift;
//    string strCheckDateTime = dateTime;
//    string strConn = ConfigurationManager.ConnectionStrings["TestControlDailyConnectionString"].ConnectionString;
//    string strComm = string.Format("SELECT si_wkno FROM shift_information WHERE si_shift='{0}' AND si_date='{1}'", strCheckShift, strCheckDateTime);
//    try
//    {
//        SqlConnection sqlConn = new SqlConnection(strConn);
//        SqlCommand sqlComm = new SqlCommand(strComm, sqlConn);
//        sqlConn.Open();
//        SqlDataReader sqlData = sqlComm.ExecuteReader();
//        if (sqlData.Read())
//        {
//            strCheckWorkNo = sqlData.GetValue(0) + "";
//        }
//        else
//        {
//            strCheckWorkNo = "";
//        }
//        sqlData.Dispose();
//        sqlComm.Dispose();
//        sqlConn.Dispose();
//    }
//    catch (Exception exp)
//    {

//        Response.Write(exp.Message.ToString());
//    }

//    return strCheckWorkNo;
//}


//if (fnGetWorkNo(strBeforeShift, strBeforeTime) == "")
//{
//    btnBefore.Enabled = false;
//}
//else
//{
//    btnBefore.Enabled = true;
//}

//if (fnGetWorkNo(strNextShift, strNextTime) == "")
//{
//    btnAfter.Enabled = false;
//}
//else
//{
//    btnAfter.Enabled = true;
//}


//protected void gvRemark_RowCommand(object sender, GridViewCommandEventArgs e)
//{
//    string strUrl = "";
//    strUrl = "CDmachine_time_report.aspx?date=" + strDateTime + "&shift=" + strShift;
//    if (e.CommandName == "btnTime")
//    {
//        Response.Write("<script>window.open('" + strUrl + "');location='javascript:history.go(-1);'</script>");
//    }  

//}
//protected void gvRemark_RowDataBound(object sender, GridViewRowEventArgs e)
//{
//    if (e.Row.RowType == DataControlRowType.DataRow)
//    {
//        LinkButton lb = e.Row.Cells[14].Controls[0] as LinkButton;
//        lb.ForeColor = Color.Blue;
//        string strUrl = "CDmachine_time_report.aspx?date=" + strDateTime + "&shift=" + strShift;
//        lb.Attributes.Add("onclick", "window.open('" + strUrl + "'); return false;");
//    }
//}
//protected void gvTide_RowDataBound(object sender, GridViewRowEventArgs e)
//{
//    if (e.Row.RowType == DataControlRowType.DataRow)
//    {
//        HyperLink hl1 = e.Row.Cells[17].Controls[1] as HyperLink;
//        HyperLink hl2 = e.Row.Cells[18].Controls[1] as HyperLink;
//        HyperLink hl3 = e.Row.Cells[17].Controls[3] as HyperLink;
//        HyperLink hl5 = e.Row.Cells[6].Controls[1] as HyperLink;
//        if (hl3.Text == "")
//            hl3.Text = "0";
//        HyperLink hl4 = e.Row.Cells[18].Controls[3] as HyperLink;
//        if (hl4.Text == "")
//            hl4.Text = "0";
//        hl1.NavigateUrl = "timeout.aspx?date="+strDateTime+"&shift="+strShift+"&last=0";
//        hl2.NavigateUrl = "nbdp_timeout.aspx?date=" + strDateTime + "&shift=" + strShift+"&last=0";
//        hl3.NavigateUrl = "timeout.aspx?date=" + strDateTime + "&shift=" + strShift + "&last=1";
//        hl4.NavigateUrl = "nbdp_timeout.aspx?date=" + strDateTime + "&shift=" + strShift + "&last=1";
//        hl5.NavigateUrl = "staff.aspx?date=" + strDateTime + "&shift=" + strShift;
//    }
//}
//protected void gvLine_RowDataBound(object sender, GridViewRowEventArgs e)
//{
//    if (e.Row.RowType == DataControlRowType.DataRow)
//    {
//        if (e.Row.Cells[9].Text == "&nbsp;")
//        {

//            e.Row.Cells[4].ForeColor = Color.Red;
//            e.Row.Cells[5].ForeColor = Color.Red;
//        }
//        else
//        {
//            e.Row.Cells[4].ForeColor = Color.Blue;
//            e.Row.Cells[5].ForeColor = Color.Blue;
//        }

//        TextBox txtbox = e.Row.Cells[23].Controls[1] as TextBox;
//        txtbox.ToolTip = txtbox.Text;
//        txtbox.Attributes.Add("onclick", "alert('"+txtbox.Text+"')");
//        e.Row.Cells[8].Style.Add(HtmlTextWriterStyle.TextDecoration, "underline");
//        e.Row.Cells[10].Style.Add(HtmlTextWriterStyle.TextDecoration, "underline");
//        e.Row.Cells[8].Style.Add(HtmlTextWriterStyle.Color, "blue");
//        e.Row.Cells[10].Style.Add(HtmlTextWriterStyle.Color, "blue");
//        e.Row.Cells[8].Attributes.Add("onmouseover", "diffDate('"+e.Row.Cells[8].Text+"','"+e.Row.Cells[7].Text+"',1)");
//        e.Row.Cells[8].Attributes.Add("onmouseout", "mouseout()");
//        e.Row.Cells[10].Attributes.Add("onmouseover", "diffDate('" + e.Row.Cells[10].Text + "','" + e.Row.Cells[9].Text + "',2)");
//        e.Row.Cells[10].Attributes.Add("onmouseout", "mouseout()");
//        //e.Row.Cells[8].ToolTip = "xxxxxxxxx";
//    }
//}

//protected void gvLine_DataBound(object sender, EventArgs e)
//{
//    if (gvLine.Rows.Count == 0)
//    {
//        DataTable dtEmpty = new DataTable();
//        //gvLine.DataSource = dtEmpty;
//      //  gvLine.DataBind();
//    }
//}
#endregion
#region 

//    //绑定标题的信息
//    private void fnBindSituation()
//    {
//        string strComm = string.Format(@"SELECT [日期]
//      ,[时间]      ,[班组]      ,[堆场主任]
//      ,[堆场员]      ,[交接班]      ,[记录时间]
//      ,[最终提交]      ,[机械维修]      ,[场地维修]
//      ,[提交时间]
//  FROM [Busy_Information_VW]
//                 where 日期='{0}' ", strDateTime);

//        string strConn = ConfigurationManager.ConnectionStrings["TestControlDailyConnectionString"].ConnectionString;
//        //string strComm = SqlDsInformation.SelectCommand;
//        SqlConnection sqlConn = new SqlConnection(strConn);
//        SqlCommand sqlComm = new SqlCommand(strComm, sqlConn);
//        sqlConn.Open();
//        SqlDataReader sqlDtRdr = sqlComm.ExecuteReader();
//        if (sqlDtRdr.Read())
//        {
//           txtVessel.Text = sqlDtRdr.GetValue(2) + "";
//            if (!String.IsNullOrEmpty(txtVessel.Text))
//            {
//                this.tabs11.Style.Add(HtmlTextWriterStyle.Color, "Red");
//            }
//           txtYard.Text = sqlDtRdr.GetValue(3) + "";
//           if (!String.IsNullOrEmpty(txtYard.Text))
//           {
//               this.tabs12.Style.Add(HtmlTextWriterStyle.Color, "Red");
//           }
//            txtMachine.Text= sqlDtRdr.GetValue(4) + "";
//            if (!String.IsNullOrEmpty(txtMachine.Text))
//            {
//                this.tabs13.Style.Add(HtmlTextWriterStyle.Color, "Red");
//            }
//            txtSpecial.Text = sqlDtRdr.GetValue(5) + "";
//            if (!String.IsNullOrEmpty(txtSpecial.Text))
//            {
//                this.tabs14.Style.Add(HtmlTextWriterStyle.Color, "Red");
//            }
//            txtTraffic.Text = sqlDtRdr.GetValue(6) + "";
//            if (!String.IsNullOrEmpty(txtTraffic.Text))
//            {
//                this.tabs15.Style.Add(HtmlTextWriterStyle.Color, "Red");
//            }
//            txtISPS.Text = sqlDtRdr.GetValue(7) + "";
//            if (!String.IsNullOrEmpty(txtISPS.Text))
//            {
//                this.tabs16.Style.Add(HtmlTextWriterStyle.Color, "Red");
//            }
//        }
//        sqlDtRdr.Dispose();
//        sqlComm.Dispose();
//        sqlConn.Dispose();

//    }


////将传入的日期和工班获取到。
//// public static string strShift = "";
////// public static string strPlanNo = "";
//// public string m_isSysSubmit = "";

//// public string strToday = "";
//// public string strTomorrow = "";
//   if (!IsPostBack)
//        {
//        }
//        fnSetEmptyDate();
//        fnSetDaily();
//        fnBindDaily(strDateTime);
//        hypControlDaily.NavigateUrl = string.Format("../ControlDailySearch.aspx?date={0}&shift={1}"
//            , strDateTime, strShift == "A" ? '2' : '1');

//        //设置日期
//        this.hdDate.Value = strDateTime;

//        strToday = System.DateTime.Parse(strDateTime).ToString("MM-dd");
//        strTomorrow = System.DateTime.Parse(strToday).AddDays(1).ToString("MM-dd");
//        fnBindInformation();
//        //fnBindFinalTime();
//        fnBindEachTime();
//        //fnBindVessel();
//        fnCheckButton(strDateTime);
//        fnCheckInfoShift();
//        fnCheckUser();//To Be Continue...
//        if (fnCheckBIFinalState(strDateTime, strShift))
//        {
//            //lblFinalTimeShow.Text = "堆场日报结束时间:";
//            //lblFinalTime.ForeColor = Color.Blue;
//            //lblVisselTime.Visible = true;
//            //lblSituationTime.Visible = true;
//            //lblYardTime.Visible = true;
//            //lblPlanTime.Visible = true;

//        }
//        else//最终提交
//        {

//            // lblVisselTime.Visible = false;
//            //lblSituationTime.Visible = false;
//            //lblYardTime.Visible = false;
//            //lblPlanTime.Visible = false;
//        }
    //显示具体时间。
/*
    private void fnBindEachTime()
    {
        //lblVisselTime.Text = "111";
        ////lblSituationTime.Text = "222";
        //lblYardTime.Text = "333";
        //lblPlanTime.Text = "444";

        //string strVessel = "";
        //string strSituation = "";
        //string strYard = "";
        //string strPlan = "";

        //string strConn = ConfigurationManager.ConnectionStrings["TestControlDailyConnectionString"].ConnectionString;
        //string strComm = "";
        ////string strDutyComm = string.Format("SELECT sd_finaltime FROM shift_duty WHERE sd_date='{0}' AND sd_shift='{1}'", strDateTime, strShift);
        //string strVesselComm = string.Format("SELECT [记录时间] FROM [busy_vessel_vw] where 日期='{0}' and 工班='{1}' order by  记录时间 desc ", strDateTime, strShift);
        //string strSituationComm = string.Format("SELECT [记录时间] FROM [busy_situation_vw] where 日期='{0}' and 工班='{1}' order by  记录时间 desc ", strDateTime, strShift);
        //string strYardComm = string.Format("SELECT [记录时间] FROM [busy_accident_vw] where 日期='{0}' and 工班='{1}' order by  记录时间 desc ", strDateTime, strShift);
        //string strPlanComm = string.Format("SELECT [记录时间] FROM [busy_breakrule_vw] where 日期='{0}' and 工班='{1}' order by  记录时间 desc ", strDateTime, strShift);

        //SqlConnection sqlConn = new SqlConnection(strConn);
        //SqlCommand sqlComm = new SqlCommand(strComm, sqlConn);
        //SqlDataReader sqlData = null;
        //sqlConn.Open();

        //sqlComm.CommandText = strVesselComm;//岸边
        //sqlData = sqlComm.ExecuteReader();
        //if (sqlData.Read())
        //{
        //    strVessel = sqlData.GetValue(0).ToString();
        //}
        //sqlData.Dispose();

        //sqlComm.CommandText = strSituationComm;//安全
        //sqlData = sqlComm.ExecuteReader();
        //if (sqlData.Read())
        //{
        //    strSituation = sqlData.GetValue(0).ToString();
        //}
        //sqlData.Dispose();

        //sqlComm.CommandText = strYardComm;//事故
        //sqlData = sqlComm.ExecuteReader();
        //if (sqlData.Read())
        //{
        //    strYard = sqlData.GetValue(0).ToString();
        //}
        //sqlData.Dispose();

        //sqlComm.CommandText = strPlanComm;//违章
        //sqlData = sqlComm.ExecuteReader();
        //if (sqlData.Read())
        //{
        //    strPlan = sqlData.GetValue(0).ToString();
        //}
        //sqlData.Dispose();

        //sqlComm.Dispose();
        //sqlConn.Dispose();



        //strVessel = strVessel != "" ? "数据提交时间:" + strVessel : "";
        //strSituation = strSituation != "" ? "数据提交时间:" + strSituation : "";
        //strYard = strYard != "" ? "数据提交时间:" + strYard : "";
        //strPlan = strPlan != "" ? "数据提交时间:" + strPlan : "";

        //lblVisselTime.Text = strVessel;
        //lblSituationTime.Text = strSituation;
        //lblYardTime.Text = strPlan;
        //lblPlanTime.Text = strPlan;
    }
 
    //检查是否为最终提交
    private bool fnCheckBIFinalState(string dateTime, string shift)
    {
        bool blnFinalState = false;
        string strDateTime = dateTime;
        string strShift = shift;

        string strFinalStateConn = "";
        string strFinalStateComm = "";
        string strState = "";

        strFinalStateConn = ConfigurationManager.ConnectionStrings["TestControlDailyConnectionString"].ConnectionString;
        strFinalStateComm = string.Format(@"SELECT 最终提交 FROM busy_information_vw 
                                        where 日期='{0}'", strDateTime);
        try
        {
            SqlConnection sqlConn = new SqlConnection(strFinalStateConn);
            SqlCommand sqlComm = new SqlCommand(strFinalStateComm, sqlConn);
            sqlConn.Open();

            SqlDataReader sqlDtRdr = sqlComm.ExecuteReader();
            if (sqlDtRdr.Read())
            {
                strState = Convert.ToString(sqlDtRdr.GetValue(0));
            }

            if (strState.ToUpper() == "Y" )
            {
                blnFinalState = true;
            }


        }
        catch (Exception exp)
        {            
            Response.Write(exp.Message.ToString());

        }
        return blnFinalState;
    }


    //通过ID判断上下工班记录是否可用
    private void fnCheckInfoShift()
    {
        //int iInfoID = fnGetInfoID(strDateTime);
        System.DateTime.Parse(strDateTime).AddDays(1).ToString();
        if (fnGetInfoID(
            System.DateTime.Parse(strDateTime).AddDays(1).ToString()
            ) != 0)//下个工班
        {
            btnAfter.Enabled = true;
        }
        else
        {
            if (fnGetInfoID(
            System.DateTime.Parse(strDateTime).AddDays(2).ToString()
            ) != 0)//下下个工班
            {
                btnAfter.Enabled = true;
            }
            else
            {
                btnAfter.Enabled = false;
            }
        }

        if (fnGetInfoID(
            System.DateTime.Parse(strDateTime).AddDays(-1).ToString()
            ) != 0)//上个工班
        {
            btnBefore.Enabled = true;
        }
        else
        {
            if (fnGetInfoID(
            System.DateTime.Parse(strDateTime).AddDays(-2).ToString()
            ) != 0)//上上个工班
            {
                btnBefore.Enabled = true;
            }
            else
            {
                btnBefore.Enabled = false;
            }
        }


    }

    //通过时间来判断是否上下工班是否可用
    private void fnCheckButton(string strDateTime)
    {        
        //  string strInfoDateTime = "";
        //string strInfoShift = strShift;
        string strBeforeTime = Convert.ToDateTime(strDateTime).AddDays(-1).ToString();
         string strBeforeTime2 = Convert.ToDateTime(strDateTime).AddDays(-2).ToString();
        if (fnGetInfoID(strBeforeTime) != 0)
        {
            btnBefore.Enabled = true; 
        }
        else 
        {
            if (fnGetInfoID(strBeforeTime2) != 0)
            {
                btnBefore.Enabled = true;
            }
            else 
            {
                btnBefore.Enabled = false;
            }
        }
       // string strBeforeShift = "";
        string strNextTime = Convert.ToDateTime(strDateTime).AddDays(1).ToString();
        string strNextTime2 = Convert.ToDateTime(strDateTime).AddDays(2).ToString();
        if (fnGetInfoID(strNextTime) != 0)
        {
            btnAfter.Enabled = true;
        }
        else
        {
            if (fnGetInfoID(strNextTime2) != 0)
            {
                btnAfter.Enabled = true;
            }
            else
            {
                btnAfter.Enabled = false;
            }
        }

        //string strNextShift = "";

       // DateTime dtTemp = Convert.ToDateTime(strDateTime);

        //if (strInfoShift == "2")//Day
        //{
        //    strBeforeShift = "1";
        //    strBeforeTime = dtTemp.AddDays(-1).ToShortDateString();
        //    strNextShift = "1";
        //    strNextTime = dtTemp.ToShortDateString();
        //}
        //else if (strInfoShift == "1")//Night
        //{
        //    strBeforeShift = "2";
        //    strBeforeTime = dtTemp.ToShortDateString();
        //    strNextShift = "2";
        //    strNextTime = dtTemp.AddDays(1).ToShortDateString();
        //}

    }

    private void fnCheckUser()
    {
        //throw new Exception("The method or operation is not implemented.");
    }
    
    //绑定标题的信息
    private void fnBindInformation()
    {
        string strInfoDate = "";
        string strInfoShift = "";
        string strInfoWeather = "";
        string strTime = "";
        string strInfoSuperVision = ""; //堆场员      
        string strInfoSuper="" ;//值班主任
        string strInfoManager = "";
        string strInfoSaftAcc = "";
        string strMachine = "";
        string strYard = "";

        string strComm = string.Format(@"SELECT [日期]      ,[时间]
      ,[班组]      ,[堆场主任]      ,[堆场员]      ,[交接班]
      ,[记录时间]      ,[最终提交]      ,[机械维修]
    ,[场地维修]      ,[提交时间]
  FROM [Busy_Information_VW]
                where 日期='{0}'", strDateTime);
      
        string strConn = ConfigurationManager.ConnectionStrings["TestControlDailyConnectionString"].ConnectionString;
        //string strComm = SqlDsInformation.SelectCommand;
        SqlConnection sqlConn = new SqlConnection(strConn);
        SqlCommand sqlComm = new SqlCommand(strComm, sqlConn);
        sqlConn.Open();
        SqlDataReader sqlDtRdr = sqlComm.ExecuteReader();
        if (sqlDtRdr.Read())
        {
            strInfoDate = sqlDtRdr.GetValue(0) + "";
            //strInfoShift = sqlDtRdr.GetValue(1) + "";
            strInfoSuper = sqlDtRdr.GetValue(3) + "";
            strInfoSuperVision = sqlDtRdr.GetValue(4) + "";
            //strInfoWeather = sqlDtRdr.GetValue(3) + "";
            //strInfoSaftAcc = sqlDtRdr.GetValue(4) + "";
            strTime = sqlDtRdr.GetValue(6) + "";
            m_isSysSubmit = sqlDtRdr.GetValue(7) + "";
            //strInfoSuper = sqlDtRdr.IsDBNull(7) ? "" : sqlDtRdr.GetValue(7) + "";
            //strInfoManager = sqlDtRdr.IsDBNull(8) ? "" : sqlDtRdr.GetValue(8) + "";
            strMachine = sqlDtRdr.GetValue(8) + "";
            strYard = sqlDtRdr.GetValue(9) + "";
        }
        sqlDtRdr.Dispose();
        sqlComm.Dispose();
        sqlConn.Dispose();

        //if (strInfoShift == "1")
        //{
        //    strInfoShift = "夜班";
        //    strInfoDate = strInfoDate + "_到_" + Convert.ToDateTime(strInfoDate).AddDays(1).ToShortDateString();
        //}
        //else
        //{
        //    strInfoShift = "白班";
        //    strInfoDate = strInfoDate + "_到_" + strInfoDate;
        //}
        if (strInfoDate != "")
        {
            strInfoDate = strInfoDate + "_到_" + Convert.ToDateTime(strInfoDate).AddDays(2).ToShortDateString();
        }
        //strInfoDate = string.Format("{0}", strInfoDate);
        //strInfoShift = string.Format("{0}", strInfoShift);
        //strInfoWeather = string.Format("{0}", strInfoWeather);
        //strInfoSuperVision = string.Format("{0}", strInfoSuperVision);
        
        lblDate.Text = strInfoDate;
        //lblShift.Text = strInfoShift;
        //lblWeather.Text = strInfoWeather;
        lblSuper.Text = strInfoSuper;
        lblSuperVisor.Text = strInfoSuperVision;
        //lblSuper.Text = strInfoSuper;
        lblFinalTime.Text = strTime;
        //txtInfoSaftAcc.Text = strInfoSaftAcc;
      //  lblManager.Text = strInfoManager;
        txtMachine.Text = strMachine;
        txtYard.Text = strYard;

    }



    //根据日期显示添加的内容
    private void fnBindDaily(string dateTime)
    {
        string strDateTime = dateTime;
        //string strShift = shift;
        
        //船舶计划
        string strBISql = string.Format(@" SELECT [Bvs_id]      ,[日期]
      ,[天数]      ,[工班]      ,[船代码]      ,[中文船名]      ,[内贸标志]
      ,[计划开始时间]      ,[计划结束时间]      ,[计划靠泊时间]
      ,[计划离泊时间]      ,[当班箱量]      ,[剩余箱量]      ,[船箱量]
  FROM [Busy_Vessel_Ship_VW]
    WHERE 日期='{0}'
", strDateTime);
        SqlDsLine.SelectCommand = strBISql + " and 天数='0' and 工班='A'";
        SqlDsLine2.SelectCommand = strBISql + " and 天数='0' and 工班='B'";
        SqlDsLine3.SelectCommand = strBISql + " and 天数='1' and 工班='A'";
        SqlDsLine4.SelectCommand = strBISql + " and 天数='1' and 工班='B'";

        //堆场计划
        string strYardSql = string.Format(@"SELECT [Bvt_id]
      ,[日期]      ,[工班日期]     ,[天数]
      ,[工班]      ,[承运人]      ,[计划开始时间]
      ,[计划结束时间]      ,[计划数量]
      ,[实际数量]      ,[等级]
  FROM [TestControlDaily].[dbo].[Busy_Working_Type_Vw]
        where  日期='{0}'   ", strDateTime);
        SqlDsTide.SelectCommand = strYardSql + " and 天数='0' and 工班='A'";
        SqlDsTide2.SelectCommand = strYardSql + " and 天数='0' and 工班='B'";
        SqlDsTide3.SelectCommand = strYardSql + " and 天数='1' and  工班='A'";
        SqlDsTide4.SelectCommand = strYardSql + " and 天数='1' and  工班='B'";
        
        //堆场计划
        string strBRSql = string.Format(@"SELECT [Bvp_id]
      ,[日期]      ,[天数]      ,[工班]  as 班组      ,[班组日期]
      ,[计划查验]      ,[计划查验归位]      ,[计划熏蒸]
      ,[计划转栈]      ,[计划进场]      ,[计划出场] ,[计划内贸进场]      ,[计划内贸出场]
      ,[查验]      ,[查验归位]      ,[熏蒸]
      ,[转栈]      ,[进场]      ,[出场]
  FROM [Busy_Vessel_Plan_vw]
                         where 日期='{0}' ", strDateTime);
        SqlDsRemark.SelectCommand = strBRSql+" and 天数='0' and 工班='A'";
        SqlDsRemark2.SelectCommand = strBRSql + " and 天数='0' and 工班='B'";
        SqlDsRemark3.SelectCommand = strBRSql + " and 天数='1' and 工班='A'";
        SqlDsRemark4.SelectCommand = strBRSql + " and 天数='1' and 工班='B'";

        SqlDsBay.SelectParameters["Bb_Date"].DefaultValue = strDateTime;
        
        if (gvBay.Rows.Count == 0)
        {
            SqlDsBay.SelectCommand = @"SELECT 	distinct  [Bb_Date]      ,[Bb_PlanType]      ,[Bb_Plan]      ,[Bb_cntrSize]
      ,[Bb_cntrHeight]      ,[Bb_cntrType]      ,[Bb_cstmnm]      ,[Bb_planStartTime]      ,[Bb_PlanEndTime]      ,[Bb_PlanNo]
      ,[Bb_PlanTo]      ,[Bb_PlanNum]	 ,dbo.getBay_fn([Bb_Planid],[Bb_Date]) Bb_Bay      ,[bb_recordtime]
  FROM [TestControlDaily].[dbo].[Busy_Bay]";
         //   SqlDsBay.SelectParameters.Remove(SqlDsBay.SelectParameters["Bb_Date"]);

            fnNoRecordBuild(gvBay, SqlDsBay);

        }

        ////作业类型
        //fnBindSituation();
        //数据源为空时１
        if (gvLine2.Rows.Count == 0)
        {
            fnNoRecordBuild(gvLine2, SqlDsLine2);
 
        }
        if (gvTide2.Rows.Count == 0)
        {
            fnNoRecordBuild(gvTide2, SqlDsTide2);

        }
        if (gvRemark2.Rows.Count == 0)
        {
            fnNoRecordBuild(gvRemark2, SqlDsRemark2);

        }
        //数据源为空时
        if (gvLine3.Rows.Count == 0)
        {
            fnNoRecordBuild(gvLine3, SqlDsLine3);

        }
        if (gvTide3.Rows.Count == 0)
        {
            fnNoRecordBuild(gvTide3, SqlDsTide3);

        }
        if (gvRemark3.Rows.Count == 0)
        {
            fnNoRecordBuild(gvRemark3, SqlDsRemark3);

        }
        //数据源为空时
        if (gvLine4.Rows.Count == 0)
        {
            fnNoRecordBuild(gvLine4, SqlDsLine4);

        }
        if (gvTide4.Rows.Count == 0)
        {
            fnNoRecordBuild(gvTide4, SqlDsTide4);

        }
        if (gvRemark4.Rows.Count == 0)
        {
            fnNoRecordBuild(gvRemark4, SqlDsRemark4);

        }
        //数据源为空时
        if (gvLine.Rows.Count == 0)
        {
            fnNoRecordBuild(gvLine, SqlDsLine);

        }
        if (gvTide.Rows.Count == 0)
        {
            fnNoRecordBuild(gvTide, SqlDsTide);

        }
        if (gvRemark.Rows.Count == 0)
        {
            fnNoRecordBuild(gvRemark, SqlDsRemark);

        }
    }
*/
    /*
     
        if (strDateTime == "" || strShift == "")
        {
            //fnSetEmptyDate();
            Response.Write("日期、工班至少一个为空,请选择后进入！");
            Response.End();
        }

        if (strShift == "2")
        {
            strShift = "A";
        }
        else if(strShift=="1")
        {
            strShift = "B";
        }
       
    }


    //设置最新的时间为最终提交的时间
    private void fnSetEmptyDate()
    {
        string strConn = ConfigurationManager.ConnectionStrings["TestControlDailyConnectionString"].ConnectionString;
        string strComm = string.Format(@"SELECT [BI_Date]
           ,[BI_SHIFT] ,[BI_day]
      ,[BI_SUPERVISOR]
      ,[BI_YardPeople]
      ,[BI_HANDOVER]
      ,[BI_RECORDTM]
      ,[BI_ISSUBMIT]
      ,[BI_MACHINE]
      ,[BI_YARD]
  FROM [Busy_Information] WHERE BI_ISSUBMIT='Y'
        ORDER BY BI_date DESC");
        try
        {
            SqlConnection sqlConn = new SqlConnection(strConn);
            SqlCommand sqlComm = new SqlCommand(strComm, sqlConn);
            sqlConn.Open();
            SqlDataReader sqlData = sqlComm.ExecuteReader();
            if (sqlData.Read())
            {
                strDateTime = Convert.ToDateTime(sqlData.GetValue(0)).ToString("yyyy-MM-dd");// string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(sqlData.GetValue(0)).ToShortDateString());
                strShift = sqlData.GetValue(1).ToString();
                //strPlanNo = sqlData.GetValue(2).ToString();
            }
            sqlData.Dispose();
            sqlComm.Dispose();
            sqlConn.Dispose();

        }
        catch (Exception exp)
        {
            exp.Message.ToString();
        }

    }

    //上一个工班时间
    protected void btnBefore_Click(object sender, EventArgs e)
    {
        // fnPreviousShift(); 通过日期和时间
        string[] arrString = new string[2];
        string strURL = "";
        string beforeDate = "";
        beforeDate = System.DateTime.Parse(strDateTime).AddDays(-1).ToShortDateString();
        int iInfoID = fnGetInfoID(beforeDate);

        //iInfoID = iInfoID - 1;
        if (0 != iInfoID)    //上一个工班时间
        {
            arrString = fnGetInfoMessage(iInfoID);
            strDateTime = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(arrString[0]));
            strShift = arrString[1];

            strURL = string.Format("BusySearch.aspx?date={0}&shift={1}", strDateTime, strShift);
            Response.Redirect(strURL);
        }
        else
        {
            iInfoID = fnGetInfoID(System.DateTime.Parse(strDateTime).AddDays(-2).ToShortDateString());
            if (0 != iInfoID)    //上一个工班时间
            {
                arrString = fnGetInfoMessage(iInfoID);
                strDateTime = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(arrString[0]));
                strShift = arrString[1];

                strURL = string.Format("BusySearch.aspx?date={0}&shift={1}", strDateTime, strShift);
                Response.Redirect(strURL);
            }
        }
    }

    ////通过日期和时间，上一个工班：1夜班2白班，白班减一。 
    //private void fnPreviousShift()
    //{
    //    DateTime endDate = DateTime.Now;
    //    DateTime nowDate = Convert.ToDateTime(strDateTime);
    //    string strInfoDateTime = strDateTime;
    //    string strInfoShift = strShift;
    //    string strInfoWorkNo = "";

    //    if (nowDate.CompareTo(endDate) < 0)
    //    {
    //        if (strInfoShift == "1")//夜班
    //        {
    //            strInfoShift = "2";
    //        }
    //        else if (strInfoShift == "2")//白班
    //        {
    //            strInfoShift = "1";
    //            nowDate = nowDate.AddDays(-1);
    //        }
    //        strInfoDateTime = nowDate.ToShortDateString();
       
    //    }
    //    else
    //    {
    //        //Do Nothing
    //    }

    //    if (strInfoWorkNo != "")
    //    {
    //        string strUrl = string.Format("BusySearch.aspx?date={0}&shift={1}", strInfoDateTime, strInfoShift);
    //        Response.Redirect(strUrl);
    //    }
    //}

    //下一个工班事件
    protected void btnAfter_Click(object sender, EventArgs e)
    {
        // fnNextShift();
        string[] arrString = new string[2];
        string strURL = "";
        int iInfoID = fnGetInfoID(strDateTime);

        iInfoID = iInfoID + 1;
        if (0!= fnGetInfoID(System.DateTime.Parse(strDateTime).AddDays(1).ToShortDateString()))//下个工班
        {
            arrString = fnGetInfoMessage(fnGetInfoID(System.DateTime.Parse(strDateTime).AddDays(1).ToShortDateString()));

            strDateTime = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(arrString[0]));
            strShift = arrString[1];
            
            strURL = string.Format("BusySearch.aspx?date={0}&shift={1}", strDateTime, strShift);
            Response.Redirect(strURL);
        }
        else
        {
            iInfoID = iInfoID + 1;
            if (0 != fnGetInfoID(System.DateTime.Parse(strDateTime).AddDays(2).ToShortDateString()))//下个工班
            {
                arrString = fnGetInfoMessage(fnGetInfoID(System.DateTime.Parse(strDateTime).AddDays(2).ToShortDateString()));

                strDateTime = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(arrString[0]));
                strShift = arrString[1];
                
                strURL = string.Format("BusySearch.aspx?date={0}&shift={1}", strDateTime, strShift);
                Response.Redirect(strURL);
            }
            else
            {
                    
            }
        }
    }

    //获取Shift_Information的ID
    private int fnGetInfoID(string dateTime)
    {
        int InfoID = 0;
        string strDateTimeInfo = dateTime;
        //string strShiftInfo = shift;
       // string strPlanNoInfo = workNo;
        string strConn = ConfigurationManager.ConnectionStrings["TestControlDailyConnectionString"].ConnectionString;
        string strComm = string.Format(@"SELECT [BI_ID]         FROM [Busy_INFORMATION] 
                    where [BI_DATE]='{0}'  order by BI_ID desc "
                , strDateTimeInfo);
        try
        {
            SqlConnection sqlConn = new SqlConnection(strConn);
            SqlCommand sqlComm = new SqlCommand(strComm, sqlConn);
            sqlConn.Open();
            SqlDataReader sqlData = sqlComm.ExecuteReader();
            if (sqlData.Read())
            {
                InfoID = Convert.ToInt32(sqlData.GetValue(0));
            }
            sqlData.Dispose();
            sqlComm.Dispose();
            sqlConn.Dispose();
        }
        catch (Exception exp)
        {
            Response.Write(exp.Message.ToString());
        }

        return InfoID;
    }

    //检测工班信息ID是否存在
    private bool fnCheckInfoID(int shiftInfoID)
    {
        int iInfoID = shiftInfoID;
        bool blnCheck = false;
        string strConn = ConfigurationManager.ConnectionStrings["TestControlDailyConnectionString"].ConnectionString;
        string strComm = string.Format(@"SELECT [BI_ID]         FROM  [Busy_Information]
                    where bi_id={0}", iInfoID);
        try
        {
            SqlConnection sqlConn = new SqlConnection(strConn);
            SqlCommand sqlComm = new SqlCommand(strComm, sqlConn);
            sqlConn.Open();
            SqlDataReader sqlData = sqlComm.ExecuteReader();

            blnCheck = sqlData.Read();

            sqlData.Dispose();
            sqlComm.Dispose();
            sqlConn.Dispose();
        }
        catch (Exception exp)
        {
            Response.Write(exp.Message.ToString());
        }
        return blnCheck;
    }

    //通过ID获取工班信息；
    private string[] fnGetInfoMessage(int shiftInfoID)
    {
        int iInfoID = shiftInfoID;
        string[] arrInfo = new string[2];

        string strConn = ConfigurationManager.ConnectionStrings["TestControlDailyConnectionString"].ConnectionString;
        string strComm = string.Format(@"SELECT [BI_DATE],  [BI_SHIFT] 
                              FROM [Busy_INFORMATION] 
                            where [BI_ID]='{0}' ", iInfoID);
        try
        {
            SqlConnection sqlConn = new SqlConnection(strConn);
            SqlCommand sqlComm = new SqlCommand(strComm, sqlConn);
            sqlConn.Open();
            SqlDataReader sqlData = sqlComm.ExecuteReader();

            if (sqlData.Read())
            {
                arrInfo[0] = sqlData.GetValue(0).ToString();
                arrInfo[1] = sqlData.GetValue(1).ToString();
                
            }

            sqlData.Dispose();
            sqlComm.Dispose();
            sqlConn.Dispose();
        }
        catch (Exception exp)
        {
            Response.Write(exp.Message.ToString());
        }
        return arrInfo;
    }

    ////根据日期，下一个工班：1夜班2白班，夜班加一。
    //private void fnNextShift()
    //{
    //    DateTime endDate = DateTime.Now;
    //    DateTime nowDate = Convert.ToDateTime(strDateTime);
    //    string strInfoDateTime = strDateTime;
    //    string strInfoShift = strShift;
    //    string strInfoWorkNo = "";

    //    if (nowDate.CompareTo(endDate) < 0)
    //    {
    //        if (strInfoShift == "1")//夜班
    //        {
    //            strInfoShift = "2";
    //            nowDate = nowDate.AddDays(1);
    //        }
    //        else if (strInfoShift == "2")//白班
    //        {
    //            strInfoShift = "1";

    //        }

    //        strInfoDateTime = nowDate.ToShortDateString();
  
    //    }
    //    else
    //    {

    //    }     
    //       string strUrl = string.Format("BusySearch.aspx?date={0}&shift={1}", strInfoDateTime, strInfoShift);
    //       Response.Redirect(strUrl);   
    //}



    protected void gvLine_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    TextBox txtBox = e.Row.Cells[18].Controls[1] as TextBox;
        //    //故障内容显示
        //    txtBox.ToolTip = txtBox.Text;
        //    txtBox.Attributes.Add("onclick", "alert('" + txtBox.Text + "')");
             
 
        //}
    }
    protected void gvTide_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //if ("btnPicture1" == e.CommandName)
        //{
        //    int index = Convert.ToInt32(e.CommandArgument);
           
        //    string strUrl1 = ((GridView)(sender)).DataKeys[index].Values[2].ToString();
        //    string strUrl2 = ((GridView)(sender)).DataKeys[index].Values[3].ToString();
        //    string strUrl3 = ((GridView)(sender)).DataKeys[index].Values[4].ToString();

        //    fnSetPictureEnable(strUrl1, hdP1, btnP1);
        //    fnSetPictureEnable(strUrl2, hdP2, btnP2);
        //    fnSetPictureEnable(strUrl3, hdP3, btnP3);

        //    if(!string.IsNullOrEmpty(strUrl1))
        //    {
        //        imgYard.ImageUrl = strUrl1;
        //        pnlPicture.Visible = true;
        //    }
        //    else if (!string.IsNullOrEmpty(strUrl2))
        //    {
        //        imgYard.ImageUrl = strUrl2;
        //        pnlPicture.Visible = true;
        //    }
        //    else
        //    {
        //        imgYard.ImageUrl = strUrl3;
        //        pnlPicture.Visible = true;
        //    }

        //}

        #region

        ////设置图片显示
        //if ("btnPicture1" == e.CommandName)
        //{
        //    int index = Convert.ToInt32(e.CommandArgument);
        //    string strUrl = ((GridView)(sender)).DataKeys[index].Values[2].ToString();
        //    if (!string.IsNullOrEmpty(strUrl))
        //    {
        //        pnlPicture.Visible = true;
        //        imgYard.ImageUrl = strUrl;
                                
        //    }
        //}
        //else if ("btnPicture2" == e.CommandName)
        //{
        //    int index = Convert.ToInt32(e.CommandArgument);
        //    string strUrl = ((GridView)(sender)).DataKeys[index].Values[3].ToString();
        //    if (!string.IsNullOrEmpty(strUrl))
        //    {
        //      pnlPicture.Visible = true;
        //        imgYard.ImageUrl = strUrl;

        //    }
        //}
        //else if ("btnPicture3" == e.CommandName)
        //{
        //    int index = Convert.ToInt32(e.CommandArgument);
        //    string strUrl = ((GridView)(sender)).DataKeys[index].Values[4].ToString();
        //    if (!string.IsNullOrEmpty(strUrl))
        //    {
        //        pnlPicture.Visible = true;
        //        imgYard.ImageUrl = strUrl;

        //    }
        //}
        #endregion

    }
    ////设置按钮可用
    //private void fnSetPictureEnable(string strUrl, HiddenField hdPictrue, LinkButton btnPicture)
    //{
    //    hdPictrue.Value = strUrl;
    //    if (string.IsNullOrEmpty(hdPictrue.Value.ToString()))
    //    {
    //        btnPicture.Enabled = false;
    //    }
    //    else 
    //    {
    //        btnPicture.Enabled = true;
    //    }
    //}

    //按钮绑定控件
    protected void gvTide_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if(DataControlRowType.DataRow==e.Row.RowType)
        //{
            
        //      if (string.IsNullOrEmpty(((System.Data.DataRowView)(e.Row.DataItem)).Row.ItemArray[6].ToString())&&
        //          string.IsNullOrEmpty(((System.Data.DataRowView)(e.Row.DataItem)).Row.ItemArray[7].ToString())&&
        //          string.IsNullOrEmpty(((System.Data.DataRowView)(e.Row.DataItem)).Row.ItemArray[8].ToString())
        //          )
        //      {
        //          ((System.Web.UI.WebControls.TableRow)(e.Row)).Cells[6].Enabled = false;
        //           ((System.Web.UI.WebControls.TableRow)(e.Row)).Cells[6].Text="无记录";
        //      }
        //}
        ////设置按钮可否使用
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    if (string.IsNullOrEmpty(((System.Data.DataRowView)(e.Row.DataItem)).Row.ItemArray[6].ToString()))
        //    {
        //        ((System.Web.UI.WebControls.TableRow)(e.Row)).Cells[6].Enabled = false;
        //    }
        //    if (string.IsNullOrEmpty(((System.Data.DataRowView)(e.Row.DataItem)).Row.ItemArray[7].ToString()))
        //    {
        //        ((System.Web.UI.WebControls.TableRow)(e.Row)).Cells[7].Enabled = false;

        //    }
        //    if (string.IsNullOrEmpty(((System.Data.DataRowView)(e.Row.DataItem)).Row.ItemArray[8].ToString()))
        //    {
        //        ((System.Web.UI.WebControls.TableRow)(e.Row)).Cells[8].Enabled = false;

        //    }
       // }
    }
//    protected void btnHide_Click(object sender, EventArgs e)
//    {
//        pnlPicture.Visible = false;
//    }
//    //按钮绑定数据
//    protected void btnP1_Click(object sender, EventArgs e)
//    {
//        imgYard.ImageUrl = hdP1.Value.ToString();
//    }
//    protected void btnP2_Click(object sender, EventArgs e)
//    {
//        imgYard.ImageUrl = hdP2.Value.ToString();
//    }
//    protected void btnP3_Click(object sender, EventArgs e)
//    {
//        imgYard.ImageUrl = hdP3.Value.ToString();
//    }
//    protected void LinkButton1_Click(object sender, EventArgs e)
//    {

//    }
     *  private void fnSetDaily()
    {
        string strDay = "";//根据日期判断

        if (Request["date"] != strDateTime && (Request["date"] + "") != "")
        {
            strDay = Convert.ToDateTime(Request["date"] + "").AddDays(-1).ToString("yyyy-MM-dd");
            if (strDay == strDateTime)
            {
                strDateTime = strDay;
                hdTab.Value = "3";
            }
            else
            {
                strDateTime = Request["date"];
            }
        }

        if (Request["shift"] != strShift && (Request["shift"] + "") != "")
        {
            strShift = Request["shift"];
        }
    }
     */
#endregion
