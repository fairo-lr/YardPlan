using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Safety_Accident : System.Web.UI.Page
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

    #region  SQL
    string strSql = @"SELECT sln_lne_rtcd 航线, 
       vls_vchnnm 中文船名, vls_vengnm 英文船名, a.voc_ivoyage 进口航次, a.voc_expvoyage 出口航次     
       ,decode(sign(to_number(decode(vbt_abthtm,null,sysdate,vbt_abthtm)-sysdate)),'-1','Y','N') 靠泊
       ,decode(sign(to_number(decode(vbt_adpttm,null,sysdate,vbt_adpttm)-sysdate)),'-1','Y','N') 离泊
       ,to_char(VBT_PBTHTM,'yy.mm.dd hh24:mi') 计划靠泊, to_char(VBT_PDPTTM,'yy.mm.dd hh24:mi') 计划离泊         
       ,to_char(a.voc_rcvsttm+7,'yy.mm.dd hh24:mi') 开港时间 
       ,to_char(a.voc_rcvedtm,'yy.mm.dd hh24:mi') 进箱截止,to_char(b.VOC_END_SUFFOCATION,'yy.mm.dd hh24:mi') 截熏蒸箱时间
       ,to_char(b.VOC_CUSEDTM,'yy.mm.dd hh24:mi') 海关截单时间,to_char(b.VOC_TEREDTM,'yy.mm.dd hh24:mi') 码头截单时间
      ,LNE_RTCHNNM 航线名称 ,cst_cstmnm 代理,a.VOC_OCRRID 船舶ID  
FROM ps_vessels,ps_vie_voc_vwf a,ps_shipping_lines,ps_service_lines,ps_customers,PS_VESSEL_OCCURENCES b 
WHERE a.voc_ocrrid=b.voc_ocrrid and vls_vnamecd=a.voc_vls_vnamecd  AND vie_iefg = 'E' 
      AND VLS_VTYPE <>'BAR' and not ((vls_vengnm ='IBN KHALIKKAN' and vio_voyage='657W') or (vls_vengnm ='IBN ABDOUN' 
      and vio_voyage='658W') or (vls_vengnm ='MAERSK KOKURA' and vio_voyage='0902') or (vls_vengnm ='MAERSK SENANG'  
      and vio_voyage='0902')) 
      AND a.voc_rcvsttm IS NOT NULL
      and SLN_SLINEID=VIE_SLN_SLINEID and LNE_RTCD=SLN_LNE_RTCD and sln_agt_cstmcd=cst_cstmcd        
      and vie_intrade is null 
      and vbt_pbthtm>=add_months(to_date(to_char(sysdate,'yyyy-mm'),'yyyy-mm'),-1) 
     and vbt_pbthtm<add_months(to_date(to_char(sysdate,'yyyy-mm'),'yyyy-mm'),-1+1) 
  
order by a.VBT_PBTHTM";
    #endregion
    string strDate = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        GetDate();
    }

    private void GetDate()
    {
        strDate = Request.QueryString["date"] + "" == ""
            ? System.DateTime.Now.ToString("yyyy-MM-dd")
            : Request.QueryString["date"] + "";
        SqlDataSource1.SelectParameters["date"].DefaultValue = strDate;
        GridView1.DataBind();
    }

    protected void btnMonth_Click(object sender, EventArgs e)
    {
        string strSql = @"--清除数据
Delete From [YardPlan].[dbo].[VesselMonth] where convert(varchar(7),vm_date,120)='"+strDate.Substring(0,7)+@"'
--生成数据
INSERT INTO [YardPlan].[dbo].[VesselMonth]
           ([vm_date]           ,[vm_line]           ,[vm_customer]
           ,[vm_chname]           ,[vm_ename]           ,[vm_ivoyage]
           ,[vm_expvoyage]    ,[vm_berthtime]		   ,[vm_departtime]
           ,[vm_internal]           ,[vm_recordtime])
SELECT '" + strDate+@"'
		,[line]      ,[customer]      ,[chname]
      ,[ename]      ,[ivoyage]      ,[expvoyage]
      ,[pbthtm]      ,[pdpttm]      ,[intrade]
	,getDate()
  FROM [YardPlan].[dbo].[ora_MonthWeek_VW]";
        fnMsSetSql(strSql);
        GridView1.DataBind();//更新后的数据

    }
}
    #region Del
    /*
     * 
     * 
    

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
        if (strShift == "2")
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
        }
        //      strDate= System.DateTime.Parse("2012-09-17").ToString();
        SqlDsYard.SelectParameters["Bvp_date"].DefaultValue = strDate;
        SqlDsYard.SelectParameters["Bvp_shift"].DefaultValue = strShift;
        SqlDsYard.SelectParameters["Bvp_day"].DefaultValue = strDay;

        SqlDsYard.InsertParameters["Bvp_date"].DefaultValue = strDate;
        SqlDsYard.InsertParameters["Bvp_shift"].DefaultValue = strShift;
        SqlDsYard.InsertParameters["Bvp_day"].DefaultValue = strDay;
        
      //  SqlDsYard.DataBind();

    }



    protected void btnCountYard_Click(object sender, EventArgs e)
    {
        string[,] arrTotal = new string[4, 2];
        string[,] arrInCheck = new string[1, 1];
        string[,] arrCheckBackPlan = new string[1, 1];
        string[,] arrCheckBackTrue = new string[1, 1];
        arrInCheck[0, 0] = "0";
        arrCheckBackTrue[0, 0] = "0";
        arrCheckBackPlan[0, 0] = "0";

        string sttm = strDay == "0" ? strDate : System.DateTime.Parse(strDate).AddDays(1).ToString("yyyy-MM-dd").Substring(0, 10);
        sttm = strShift == "A" ? sttm + " 08" : sttm + " 20";
        string strIn = "";
        string strInnerIn = "";
        string strOut = "";
        string strInnerOut = "";
        string strCheck = "";//查验 

        string strCheckBack = "";//查验归位
        string strSuffocation = "";//熏蒸即场内查验        
        string strStack = ""; //转栈 即重箱转栈
        #region OracleSql

        string oraInCheck = @"  select count(箱ID)
      FROM (SELECT 　箱ID, 箱号, 计划号
              from (select dvp.dvp_planno,
                           plc.plc_cntrid  箱ID,
                           plc.plc_cntrno  箱号,
                           rc.rv_meaning,
                           dvp_opsttm      计划开始时间,
                           dvp_opedtm      计划结束时间,
                           rsp.rsp_pstatus,
                           rsp.rsp_planid  计划号
                      from ps_devan_plans              dvp,
                           ps_plan_containers          plc,
                           ps_ref_codes                rc,
                           ps_shifting_plans           rsp,
                           ps_shifting_operatings      sop,
                           ps_shifting_plan_containers rsc
                     where dvp_opmode like '4%'
                       and dvp.dvp_opprc = 'CC'
                       and rsc.rsc_cntrid = plc.plc_cntrid
                       and rsc.rsc_rsp_plannid = rsp.rsp_planid
                       and dvp.dvp_planno = plc.plc_dvp_planno
                       and dvp.dvp_opmode = rc.rv_low_value
                       and rsp.rsp_sop_id = sop.sop_id(+)
                       and dvp.dvp_planno = rsp.rsp_dvp_planno
                       and ((dvp.dvp_opsttm between to_date('@date','yyyy-mm-dd hh24')  and to_date('@date','yyyy-mm-dd hh24')+1/2 ) or
                           (rsp.rsp_ptime <= to_date('@date','yyyy-mm-dd hh24')  and
                           --rsp.rsp_pstatus = 1) or rsp.rsp_pstatus = 1 or
                           rsc.rsc_pstatus = 1) or rsc.rsc_pstatus = 1 or
                           sop.sop_ptime between to_date('@date','yyyy-mm-dd hh24')  and to_date('@date','yyyy-mm-dd hh24')+1/2 ))
            UNION
            select rsc.rsc_cntrid 箱ID,
                   rsc.rsc_cntrno 箱号,
                   rsp.rsp_planid 计划号
              from ps_yard_container_activities iya,
                   ps_shifting_plan_containers  rsc,
                   ps_shifting_plans            rsp
             where iya.iya_opprc = 'YY'
               and iya.iya_opmode = '62'
               and rsc.rsc_rsp_plannid = rsp.rsp_planid
               and rsp.rsp_dvp_planno is not null
               and iya.iya_iyc_cntrid = rsc.rsc_cntrid
               and iya.iya_rdctm between to_date('@date','yyyy-mm-dd hh24') and to_date('@date','yyyy-mm-dd hh24')+1/2
               and (iya.iya_pmiloc = rsc.rsc_trnsfiloc or
                   iya.iya_rmiloc = rsc.rsc_trnsfiloc))";

        string oraCheckBackPlan = @"   select count(箱ID)
  --    INTO O_PLAN
      from (select spc.rsc_cntrid 箱ID, spc.rsc_cntrno 箱号
              from ps_shifting_plans sp,
                   ps_shifting_plan_containers spc,
                   ps_shifting_operatings sop,
                   (select *
                      from ps_yard_containers          iyc,
                           ps_shifting_plan_containers rsc
                     where iyc.iyc_cntrid = rsc.rsc_cntrid) iyc,
                   (select *
                      from ps_history_containers       hsc,
                           ps_shifting_plan_containers rsc
                     where hsc.hsc_hcid = rsc.rsc_cntrid) hsc
             where spc.rsc_rsp_plannid = sp.rsp_planid
               and ((iyc.iyc_cntrid = spc.rsc_cntrid and
                   iyc.iyc_outtm is null) or
                   (iyc.iyc_cntrid = spc.rsc_cntrid and
                   iyc.iyc_outtm > to_date('@date','yyyy-mm-dd hh24')+1/2) or
                   (iyc.iyc_cntrid = spc.rsc_cntrid and
                   iyc.iyc_type = 'OBF') OR
                   (iyc.iyc_cntrid = spc.rsc_cntrid and
                   sop.sop_trucknum is not null and
                   iyc.iyc_outtm between to_date('@date','yyyy-mm-dd hh24') and to_date('@date','yyyy-mm-dd hh24')+1/2) OR
                   (hsc.hsc_hcid = spc.rsc_cntrid and
                   hsc.hsc_outytm is null) or
                   (hsc.hsc_hcid = spc.rsc_cntrid and
                   hsc.hsc_outytm > to_date('@date','yyyy-mm-dd hh24')+1/2) OR
                   (hsc.hsc_hcid = spc.rsc_cntrid and hsc.Hsc_Type = 'OBF') or
                   (hsc.hsc_hcid = spc.rsc_cntrid and
                   sop.sop_trucknum is not null and
                   hsc.hsc_outytm between to_date('@date','yyyy-mm-dd hh24') and to_date('@date','yyyy-mm-dd hh24')+1/2))
               and sp.rsp_sop_id = sop.sop_id(+)
               and rsp_dvp_planno is null
                  --and sop.sop_trucknum is not null
                  --and (iyc.iyc_outtm is null or hsc.hsc_outytm is null)
               and (sop.sop_ptime between to_date('@date','yyyy-mm-dd hh24') and to_date('@date','yyyy-mm-dd hh24')+1/2 or
                   (rsp_shsttm <= to_date('@date','yyyy-mm-dd hh24') and spc.RSC_PSTATUS = 1) or
                   rsp_pstatus = 1)
            union
            select rsc.rsc_cntrid 箱ID, rsc.rsc_cntrno 箱号
              from ps_yard_container_activities iya,
                   ps_shifting_plan_containers  rsc,
                   ps_shifting_plans            rsp
             where iya.iya_opprc = 'YY'
               and iya.iya_opmode = '62'
               and rsc.rsc_rsp_plannid = rsp.rsp_planid
               and rsp.rsp_dvp_planno is null
               and iya.iya_iyc_cntrid = rsc.rsc_cntrid
               and iya.iya_rdctm between to_date('@date','yyyy-mm-dd hh24') and to_date('@date','yyyy-mm-dd hh24')+1/2
               and (iya.iya_pmiloc = rsc.rsc_trnsfiloc or
                   iya.iya_rmiloc = rsc.rsc_trnsfiloc))";

        string oraCheckBackTrue = @"  select count(iya_iyc_cntrid)
     -- into O_COMP
      from (select iya.iya_iyc_cntrid
              from ps_yard_container_activities iya,
                   ps_shifting_plan_containers  rsc,
                   ps_shifting_plans            rsp
             where iya.iya_opprc = 'YY'
               and iya.iya_opmode = '62'
               and rsc.rsc_rsp_plannid = rsp.rsp_planid
               and rsp.rsp_dvp_planno is null
               and iya.iya_iyc_cntrid = rsc.rsc_cntrid
               and iya.iya_rdctm between to_date('@date','yyyy-mm-dd hh24') AND to_date('@date','yyyy-mm-dd hh24')+1/2
               and (iya.iya_pmiloc = rsc.rsc_trnsfiloc or
                   iya.iya_rmiloc = rsc.rsc_trnsfiloc)
            union all
            select hca.hca_hsc_hcid
              from ps_h_container_activities     hca,
                   ps_shifting_plan_containers_h rsc,
                   ps_shifting_plans_h           rsp
             where hca.hca_opprc = 'YY'
               and hca.hca_opmode = '62'
               and rsc.rsc_rsp_plannid = rsp.rsp_planid
               and rsp.rsp_dvp_planno is null
               and hca.hca_hsc_hcid = rsc.rsc_cntrid
               and hca.hca_rdctm between to_date('@date','yyyy-mm-dd hh24') AND to_date('@date','yyyy-mm-dd hh24')+1/2
               and (hca.hca_pmiloc = rsc.rsc_trnsfiloc or
                   hca.hca_rmiloc = rsc.rsc_trnsfiloc))";

        string oraStack = @"       --PS_GET_INOUTMOVESTACK_IT_F
 select 作业方式, count(作业方式) 数量
        from (select hsc.hsc_ctnrno    箱号,
                     oth.oth_trk_trkno 集卡号,
                     oth.oth_inytm     集卡进闸时间,
                     --hca.hca_rdctm 机械确认时间,
                     oth.oth_outytm 集卡出闸时间,
                     round(TO_NUMBER(decode(oth.oth_outytm,
                                            null,
                                            sysdate,
                                            oth.oth_outytm) - oth.oth_inytm) * 24 * 60,
                           2) 逗留时间,
                     decode(oth.oth_drfg,
                            '1',
                            decode(ps_web_get_mod_f(hsc_outymd, '作业方式'),
                                   '单提空箱',
                                   '移场',
                                   '提中转箱',
                                   '转栈',
                                   '出场'),
                            '2',
                            decode(ps_web_get_mod_f(hsc_inYMODE, '作业方式'),
                                   '中转箱转码头',
                                   '转栈',
                                   '进箱')) 作业方式
                from ps_out_c_truck_activities_h   oth,
                     ps_c_trucks_drag_containers_h tch,
                     ps_history_containers         hsc,
                     ps_operate_instructions_h     oih
              --ps_h_container_activities     hca         --colin 2012-08-29
               where oth.oth_seq = tch.tch_oth_seq
                 and hsc.hsc_hcid = tch.tch_cntrid
                 and hsc.hsc_type <> 'HSC'
                 --and hca.hca_hsc_hcid = hsc.hsc_hcid    --colin 2012-08-29
                 and tch.tch_cancelrn is null
                    --and tch.tch_opinstrid = hca.hca_ois_opinstrid --colin 2012-08-29
                 and tch.tch_opinstrid = oih.oih_opinstrid
                    -- AND oth.oth_trn_trnscomcd <> 'CYZX'
                    --and ((oth.oth_drfg='2'and to_char(oth.oth_outytm,'yyyy-mm-dd HH24:mi')=to_char(hca.hca_rdctm,'yyyy-mm-dd HH24:mi'))or(oth.oth_drfg='1'and to_char(oth.oth_outytm,'yyyy-mm-dd HH24:mi')= to_char(hsc.hsc_outytm,'yyyy-mm-dd HH24:mi'))) 
                    --and hca.hca_opmode not in ('81', '82') --colin 2012-08-29
                    --and hca.hca_opprc = decode(oth.oth_drfg, '1', 'YT', '2', 'TY') --colin 2012-08-29
                 and oih.oih_opmode not in ('81', '82')
                 and oih.oih_opprc =
                     decode(oth.oth_drfg, '1', 'YT', '2', 'TY')
                 and oth.oth_returnfg is null
                 and hsc.hsc_transea_flag = '3' --区分内贸
                    
                 and oth.oth_inytm between to_date('@date','yyyy-mm-dd hh24') AND to_date('@date','yyyy-mm-dd hh24')+1/2
              union
              select c.iyc_cntrno    箱号,
                     t.ota_trk_trkno 集卡号,
                     t.ota_inytm     集卡进闸时间,
                     --iya.iya_rdctm 机械确认时间,
                     t.ota_outytm 集卡出闸时间,
                     round(TO_NUMBER(decode(t.ota_outytm,
                                            null,
                                            sysdate,
                                            t.ota_outytm) - t.ota_inytm) * 24 * 60,
                           2) 逗留时间,
                     decode(t.ota_drfg,
                            '1',
                            decode(ps_web_get_mod_f(IYC_OUTYMODE, '作业方式'),
                                   '单提空箱',
                                   '移场',
                                   '提中转箱',
                                   '转栈',
                                   '出场'),
                            '2',
                            decode(ps_web_get_mod_f(IYC_inYMODE, '作业方式'),
                                   '中转箱转码头',
                                   '转栈',
                                   '进箱')) 作业方式
                from ps_out_c_truck_activities   t,
                     ps_c_trucks_drag_containers d,
                     ps_yard_containers          c,
                     ps_operate_instructions_i   ois
              --ps_yard_container_activities iya          --colin 2012-08-29
               where t.ota_seq = d.tkc_ota_seq
                 and c.iyc_cntrid = d.tkc_cntrid
                 AND c.iyc_type <> 'HSC'
                 --and iya.iya_iyc_cntrid = c.iyc_cntrid  --colin 2012-08-29
                 and d.tkc_cancelrn is null
                 and c.iyc_transea_flag = '3' --区分内贸标志
                    --and d.tkc_opinstrid = iya.iya_ois_opinstrid   --colin 2012-08-29
                 and d.tkc_opinstrid = ois.ois_opinstrid
                    --and iya.iya_opmode not in ('81', '82')        --colin 2012-08-29
                    --AND IYA.IYA_OPPRC = decode(t.ota_drfg, '1', 'YT', '2', 'TY') --colin 2012-08-29
                 and ois.ois_opmode not in ('81', '82')
                 and ois.ois_opprc =
                     decode(t.ota_drfg, '1', 'YT', '2', 'TY')
                 and T.OTA_RETURNFG is null
                    -- AND T.OTA_TRN_TRNSCOMCD <> 'CYZX'
                    --and ((t.ota_drfg='2'and to_char(t.ota_outytm,'yyyy-mm-dd HH24:mi')=to_char(iya.iya_rdctm,'yyyy-mm-dd HH24:mi'))or(t.ota_drfg='1'and to_char(t.ota_outytm,'yyyy-mm-dd HH24:mi')= to_char(c.iyc_outtm,'yyyy-mm-dd HH24:mi'))) 
                    
                 and t.ota_inytm between to_date('@date','yyyy-mm-dd hh24') AND to_date('@date','yyyy-mm-dd hh24')+1/2
               order by 作业方式, 逗留时间 desc)
       group by 作业方式
 ";
        #endregion

        oraInCheck = oraInCheck.Replace("@date", sttm);
        oraStack = oraStack.Replace("@date", sttm);
        oraCheckBackPlan = oraCheckBackPlan.Replace("@date", sttm);
        oraCheckBackTrue = oraCheckBackTrue.Replace("@date", sttm);

        

        arrTotal = FnGetValue(oraStack, arrTotal);
        arrInCheck = FnGetValue(oraInCheck, arrInCheck);
        //arrCheckBackPlan = FnGetValue(oraCheckBackPlan, arrCheckBackPlan);
        //arrCheckBackTrue = FnGetValue(oraCheckBackTrue, arrCheckBackTrue);

        strSuffocation = arrInCheck[0, 0];
   
        arrCheckBackPlan[0, 0] = "0";
        arrCheckBackTrue[0, 0] = "0";
        strCheckBack = fnOraFunction(@"ps_get_yard_ck_f"
            , sttm, DateTime.Parse(sttm + ":00:00").AddHours(12).ToString("yyyy-MM-dd HH")
            , ref arrCheckBackPlan[0, 0], ref arrCheckBackTrue[0, 0]);
        //查验归位=计划查验归位-实际查验归位
        strCheckBack = ((int)(int.Parse(arrCheckBackPlan[0, 0])
            - int.Parse(arrCheckBackTrue[0, 0]))).ToString();
        for (int i = 0; i < arrTotal.GetLength(0); i++)
        {
            switch (arrTotal[i, 0])
            {
                case "转栈":
                    strStack = arrTotal[i, 1];
                    break;
                default:
                    break;
            }
        }

        #region SqlServer
        string sqlSelect = string.Format(@"SELECT *  
FROM [Busy_Vessel_Plan] 
WHERE [Bvp_date]='{0}' AND [Bvp_day]='{1}' AND [Bvp_shift]='{2}'"
        , strDate, strDay, strShift);

        string sqlDelete = string.Format(@"DELETE  
FROM [Busy_Vessel_Plan] 
WHERE [Bvp_date]='{0}' AND [Bvp_day]='{1}' AND [Bvp_shift]='{2}'"
        , strDate, strDay, strShift);

        string sqlIn = string.Format(@"
select sum(sw_in)/4.0 
from controlDaily.dbo.shift_works
where (sw_date=convert(datetime,'{0}',120)-7
		or sw_date=convert(datetime,'{0}',120)-14
		or sw_date=convert(datetime,'{0}',120)-21
		or sw_date=convert(datetime,'{0}',120)-28)
	 and  sw_shift='{1}'--白班", strDate, (strShift == "A" ? "2" : "1"));
        string sqlOut = string.Format(@"
select sum(sw_out)/4.0
from controlDaily.dbo.shift_works
where (sw_date=convert(datetime,'{0}',120)-7
		or sw_date=convert(datetime,'{0}',120)-14
		or sw_date=convert(datetime,'{0}',120)-21
		or sw_date=convert(datetime,'{0}',120)-28)
	 and  sw_shift='{1}'--白班", strDate, (strShift == "A" ? "2" : "1"));
        string sqlInnerIn = string.Format(@"
select sum(sw_it_in)/4.0        
from controlDaily.dbo.shift_works
where (sw_date=convert(datetime,'{0}',120)-7
		or sw_date=convert(datetime,'{0}',120)-14
		or sw_date=convert(datetime,'{0}',120)-21
		or sw_date=convert(datetime,'{0}',120)-28)
	 and  sw_shift='{1}'--白班", strDate, (strShift == "A" ? "2" : "1"));
        string sqlInnerOut = string.Format(@"
select sum(sw_it_out)/4.0
from controlDaily.dbo.shift_works
where (sw_date=convert(datetime,'{0}',120)-7
		or sw_date=convert(datetime,'{0}',120)-14
		or sw_date=convert(datetime,'{0}',120)-21
		or sw_date=convert(datetime,'{0}',120)-28)
	 and  sw_shift='{1}'--白班", strDate, (strShift == "A" ? "2" : "1"));
        string sqlCheck = string.Format(@"
select sum(sw_ck)/4.0
from controlDaily.dbo.shift_works
where (sw_date=convert(datetime,'{0}',120)-7
		or sw_date=convert(datetime,'{0}',120)-14
		or sw_date=convert(datetime,'{0}',120)-21
		or sw_date=convert(datetime,'{0}',120)-28)
	 and  sw_shift='{1}'--白班", strDate, (strShift == "A" ? "2" : "1"));
        #endregion

        strIn = fnMsGetValue(sqlIn);
        strOut = fnMsGetValue(sqlOut);
        strInnerIn = fnMsGetValue(sqlInnerIn);
        strInnerOut = fnMsGetValue(sqlInnerOut);
        strCheck = fnMsGetValue(sqlCheck);


        int count = fnMsGetCount(sqlSelect);
        if (count != 0)
        {
            fnMsSetSql(sqlDelete);
        }



        string sqlInsert = string.Format(@"INSERT INTO Busy_Vessel_Plan(Bvp_date, Bvp_day, Bvp_shift
                    ,  Bvp_check_plan, Bvp_Checkback_plan, bvp_suffocation_plan, bvp_transfer_plan
                     , bvp_in_plan, bvp_out_plan,bvp_innerIn_plan,bvp_innerOut_plan ) VALUES 
                    ('{0}','{1}', '{2}','{3}' ,'{4}' ,'{5}' , '{6}', '{7}', '{8}','{9}','{10}')", strDate, strDay, strShift
                      , strCheck, strCheckBack, strSuffocation, strStack, strIn, strOut, strInnerIn, strInnerOut);

        fnMsSetSql(sqlInsert);

        gvYardPlan.DataBind();
        gvYard.DataBind();

    }

    private string fnOraFunction(string functionName, string startTime, string endTime
        ,ref string outValue1,ref  string outValue2)
    {  
        
        System.Data.OracleClient.OracleConnection oraConn =
            new System.Data.OracleClient.OracleConnection(
            ConfigurationManager.ConnectionStrings["oraConnectionString"].ConnectionString);
        System.Data.OracleClient.OracleCommand oraComm = new System.Data.OracleClient.OracleCommand();

        oraComm.Connection = oraConn;
        //oraComm.CommandText = functionName;
        oraComm.CommandText =/*"@result :="+ */ /*   functionName
//            + @"(i_sttm => @i_sttm,
//                                    i_edtm => @i_edtm,
//                                    o_plan => @o_plan,
//                                    o_comp => @o_comp);";
        + @"(sttm,edtm,plan,comp)";
        oraComm.CommandType = CommandType.StoredProcedure;
        //oraComm.Parameters.Add(DateTime.Parse(startTime.Substring(0,13)+":00:00"));
        //oraComm.Parameters.Add(DateTime.Parse(endTime.Substring(0, 13) + ":00:00"));
        //oraComm.Parameters.Add(outValue1);
        //oraComm.Parameters.Add(outValue2);

    

        System.Data.OracleClient.OracleParameter parSttm = new System.Data.OracleClient.OracleParameter();
        parSttm.OracleType = System.Data.OracleClient.OracleType.DateTime;// DbType.Date;
        parSttm.Direction = ParameterDirection.Input;
        parSttm.ParameterName = "i_sttm";
        parSttm.Value = DateTime.Parse(startTime.Substring(0, 13) + ":00:00").ToString("yyyy-MM-dd HH:mm:ss");
        oraComm.Parameters.Add(parSttm);

        System.Data.OracleClient.OracleParameter parEdtm = new System.Data.OracleClient.OracleParameter();
        parEdtm.OracleType = System.Data.OracleClient.OracleType.DateTime;// DbType.Date;
        parEdtm.Direction = ParameterDirection.Input;
        parEdtm.ParameterName = "i_edtm";
        parEdtm.Value = DateTime.Parse(endTime.Substring(0, 13) + ":00:00").ToString("yyyy-MM-dd HH:mm:ss");
        oraComm.Parameters.Add(parEdtm);

        System.Data.OracleClient.OracleParameter parOut1 = new System.Data.OracleClient.OracleParameter();
        parOut1.OracleType = System.Data.OracleClient.OracleType.Float;
        parOut1.Direction = ParameterDirection.Output;
        parOut1.ParameterName = "o_plan";
        oraComm.Parameters.Add(parOut1);

        System.Data.OracleClient.OracleParameter parOut2 = new System.Data.OracleClient.OracleParameter();
        parOut2.OracleType = System.Data.OracleClient.OracleType.Float;
        parOut2.Direction = ParameterDirection.Output;
        parOut2.ParameterName = "o_comp";
        oraComm.Parameters.Add(parOut2);

        System.Data.OracleClient.OracleParameter parReturn = new System.Data.OracleClient.OracleParameter();
        parReturn.OracleType = System.Data.OracleClient.OracleType.Float;
        parReturn.Direction = ParameterDirection.ReturnValue;
        parReturn.ParameterName = "result";
        oraComm.Parameters.Add(parReturn);

        oraComm.CommandText = functionName + "(" + parSttm.ParameterName
            + ", " + parEdtm + ", " + parOut1.ParameterName + ", " +
            parOut2.ParameterName + ")";
        oraComm.CommandText = functionName;
        oraConn.Open();
        
        oraComm.ExecuteOracleScalar();//.ExecuteNonQuery();
        //System.Data.OracleClient.OracleDataReader oraData = oraComm.ExecuteReader();
        outValue1 = oraComm.Parameters["o_plan"].Value.ToString();
        outValue2 = oraComm.Parameters["o_comp"].Value.ToString();
       // oraData.Close();
        oraComm.Cancel();
        oraConn.Close();
        //oraData.Dispose();
        oraComm.Dispose();
        oraConn.Dispose();
        // arrReturn = fnSetLeftingArr(arrDetails);
        return "0";
        //return arrDetails;
    }

    //返回记录数
    private string fnMsGetValue(string strSelect)
    {
        string strReturn = "";
        string strCount = (strSelect);
        System.Data.SqlClient.SqlConnection sqlConn = new SqlConnection(
            ConfigurationManager.ConnectionStrings["TestControlDailyConnectionString"].ConnectionString);
        System.Data.SqlClient.SqlCommand sqlComm = new SqlCommand(
            strCount, sqlConn);
        sqlConn.Open();
        System.Data.SqlClient.SqlDataReader sqlData = sqlComm.ExecuteReader();

        if (sqlData.Read())
        {
            strReturn = sqlData.GetValue(0).ToString();

        }
        sqlData.Dispose();
        sqlComm.Dispose();
        sqlConn.Dispose();

        return strReturn;

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

    protected void btnShiftYard_Click(object sender, EventArgs e)
    {

    }
    protected void gvYard_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
     * 
    //保存事件
    //如果没有数据插入相关的数据，如果有则更新，否则删除.    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        m_date = ViewState["date"].ToString();
        m_shift = ViewState["shift"].ToString();
        string[] arrContent = new string[50];
        arrContent = fnGetContent(m_date, m_shift,arrContent);
  
         if (!string.IsNullOrEmpty(this.hdValue.Value))
        {             
            string[] t_arrRecord = this.hdValue.Value.Substring(0, this.hdValue.Value.Length - 1).Split('|');
            //DeleteByDate(m_date, m_shift);       
            for (int i = 0; i < t_arrRecord.Length; ++i)
            {                
                String[] t_arrItme = t_arrRecord[i].Split(',');

                String t_happenTm = GenerateRightTM(m_date, m_shift, t_arrItme[0]);                       
                int AccidentID = 0;        
                if (!string.IsNullOrEmpty(t_arrItme[2]))
                {
                    AccidentID = fnGetAccidentID(m_date, m_shift, t_arrItme[2]);//用事故记录来匹配
                    if (AccidentID == 0)
                    {
                        InsertRecord(i, m_date, m_shift, t_happenTm, t_arrItme[1], t_arrItme[2]);//,t_arrItme[3]);
                        AccidentID = fnGetAccidentID(m_date, m_shift, t_arrItme[2]);//获取ID
                        SaveImages(m_date, m_shift, i,AccidentID);//保存照片并更新
                    }
                    else
                    {
                        UpdateRecord(AccidentType.happendtm, t_happenTm,AccidentID);
                        UpdateRecord(AccidentType.type, t_arrItme[1],AccidentID);
                        SaveImages(m_date, m_shift, i, AccidentID);//保存照片
                        //UpdateRecord(AccidentType.content, t_arrItme[2]);
                    }
                }
                for (int j = 0; j < arrContent.Length ; j++)
                {
                    if (t_arrItme[2] == arrContent[j])
                    {
                        arrContent[j] = "";//匹配到相同的数据，清空用于删除
                     
                    }
                }


            }
            for (int i = 0; i < arrContent.Length; i++)//删除相关的数据
            {
                if (!string.IsNullOrEmpty(arrContent[i]))
                {
                    DeleteByCOntent(m_date, m_shift, arrContent[i]);
                }
            }

            this.labBtnMessage.Text = "保存成功！";
        }
        else
        {
            DeleteByDate(m_date, m_shift);
            this.labBtnMessage.Text = "全部删除成功";
        }
        BindValue(m_date,m_shift);
        //SaveImages(m_date,m_shift);
    }

    //清空多余的数据
    private void DeleteByCOntent(string strDate, string strShift, string strContent)
    {
        #region SQL

        string t_sql = "delete safety_accident where sa_date=@date and sa_shift=@shift and sa_content=@content";

        #endregion
        SqlParameter[] t_arrParam = { new SqlParameter(), new SqlParameter() ,new SqlParameter()};
        t_arrParam[0].ParameterName = "@date";
        t_arrParam[0].Value = strDate;
        t_arrParam[1].ParameterName = "@shift";
        t_arrParam[1].Value = strShift;
        t_arrParam[2].ParameterName="@content";
        t_arrParam[2].Value=strContent;

        SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, t_sql, t_arrParam);
    }

    private string[] fnGetContent(string strDate, string strShift,string[] strContent)
    {       
        string strSql =
@"SELECT  SA_Content
FROM SAFETY_ACCIDENT 
WHERE  SA_SHIFT=@shift
    and SA_DATE=@date";
        
        SqlParameter[] t_arrParam = { new SqlParameter(), new SqlParameter()};
        t_arrParam[0].ParameterName = "@date";
        t_arrParam[0].Value = strDate;
        t_arrParam[1].ParameterName = "@shift";
        t_arrParam[1].Value = strShift;

        SqlDataReader t_reader = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, t_arrParam);
        int i = 0;
        while (t_reader.Read() )
        {
            if (i < strContent.Length)
            {
                strContent[i] = t_reader.GetValue(0).ToString();
                i++;
            }
        }
        t_reader.Close();
        return strContent;

      
    }

    //获取桥吊ID
    private int  fnGetAccidentID(string strDate, string strShift, string strContent)
    {
        int iReturn = 0;
        string strSql =
@"SELECT  SA_ID
FROM SAFETY_ACCIDENT 
WHERE SA_CONTENT=@content
    and SA_SHIFT=@shift
    and SA_DATE=@date";
        SqlParameter[] t_arrParam = { new SqlParameter(), new SqlParameter(),new SqlParameter() };
        t_arrParam[0].ParameterName = "@date";
        t_arrParam[0].Value = strDate;
        t_arrParam[1].ParameterName = "@shift";
        t_arrParam[1].Value = strShift;
        t_arrParam[2].ParameterName = "@content";
        t_arrParam[2].Value = strContent;
        SqlDataReader t_reader = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, t_arrParam);
        if (t_reader.Read())
            iReturn = int.Parse(t_reader.GetValue(0).ToString());
        t_reader.Close();
        return iReturn;
    
    }

    //保存文件
    private Boolean SaveImages(string strDate, string strShift,int iRow, int iaccidentID)
    { 
        iRow = iRow * 3;//三行
        ///'遍历File表单元素        
        HttpFileCollection files = HttpContext.Current.Request.Files;

        /// '状态信息
        System.Text.StringBuilder strMsg = new System.Text.StringBuilder();
        try
        {
            if (iRow < files.Count)
            {
                for (int i = 0; i < 3; i++)
                {
                    HttpPostedFile postedFile = files[iRow + i];
                    string fileName, fileExtension;
                    fileName = System.IO.Path.GetFileName(postedFile.FileName);
                    if (fileName != "")
                    {
                        fileExtension = System.IO.Path.GetExtension(fileName);
                        strMsg.Append("上传的文件类型：" + postedFile.ContentType.ToString() + "<br>");
                        strMsg.Append("客户端文件地址：" + postedFile.FileName + "<br>");
                        strMsg.Append("上传文件的文件名：" + fileName + "<br>");
                        strMsg.Append("上传文件的扩展名：" + fileExtension + "<br><hr>");
                        ///'可根据扩展名字的不同保存到不同的文件夹
                        ///注意：可能要修改你的文件夹的匿名写入权限。
                        int iP = iRow / 3 * 10 + i + 11;
                        Random rdm = new Random();
                        //大于1M
                        if (postedFile.ContentLength / 1024 / 1024 > 0)
                        {
                            lblMessage.Text = fileName + "大于1MB,取消此照片保存！";
                            
                        }
                        else
                        {
                            string name = "picture" + strDate + "_" + strShift + "_" + iP.ToString() + "_" + rdm.Next(1000, 10000).ToString() + fileExtension;
                            postedFile.SaveAs(System.Web.HttpContext.Current.Request.MapPath("UploadedImages/") +
                              name);
                            switch (i)
                            {
                                case 0:
                                    UpdateRecord(AccidentType.picture, "UploadedImages/" + name, iaccidentID);
                                    break;
                                case 1:
                                    UpdateRecord(AccidentType.picture2, "UploadedImages/" + name, iaccidentID);
                                    break;
                                case 2:
                                    UpdateRecord(AccidentType.picture3, "UploadedImages/" + name, iaccidentID);
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            strStatus.Text = strMsg.ToString();
            return true;

        }
        catch (Exception exp)
        {
            strStatus.Text = exp.Message;
            return false;
        }
    }

    //保存文件
    private Boolean SaveImages(string strDate,string strShift)
    {
        ///'遍历File表单元素
        HttpFileCollection files = HttpContext.Current.Request.Files;

        /// '状态信息
        System.Text.StringBuilder strMsg = new System.Text.StringBuilder();
        strMsg.Append("上传的文件分别是：<hr color=red>");
        try
        {
            for (int iFile = 0; iFile < files.Count; iFile++)
            {
                ///'检查文件扩展名字
                HttpPostedFile postedFile = files[iFile];
                string fileName, fileExtension;
                fileName = System.IO.Path.GetFileName(postedFile.FileName);
                if (fileName != "")
                {
                    fileExtension = System.IO.Path.GetExtension(fileName);
                    strMsg.Append("上传的文件类型：" + postedFile.ContentType.ToString() + "<br>");
                    strMsg.Append("客户端文件地址：" + postedFile.FileName + "<br>");
                    strMsg.Append("上传文件的文件名：" + fileName + "<br>");
                    strMsg.Append("上传文件的扩展名：" + fileExtension + "<br><hr>");
                    ///'可根据扩展名字的不同保存到不同的文件夹
                    ///注意：可能要修改你的文件夹的匿名写入权限。
                    int iP = iFile / 3 * 10 + iFile;
                    string name = "picture" + strDate + strShift + iP.ToString() + ".jpg";
                    postedFile.SaveAs(System.Web.HttpContext.Current.Request.MapPath("UploadedImages/") +
                      name);
                }
            }
            strStatus.Text = strMsg.ToString();
            return true;
        }
        catch (System.Exception Ex)
        {
            strStatus.Text = Ex.Message;
            return false;
        }
    }

    private void UpdateRecord(AccidentType type, string typeValue,int accidentID)
    {
        #region SQL
        String t_sql = "";
        #endregion
        
//@"INSERT INTO [SAFETY_ACCIDENT]
//           ([SA_DATE]
//           ,[SA_SHIFT]
//           ,[SA_HAPPENTM]
//           ,[SA_CONTENT]
//           ,[SA_RECORDTM]
//           ,[SA_TYPE]
//        --  ,[SA_PICTURE]
//)
//     VALUES(@date,@shift,@happentm,@p_record,getdate(),@type--,@picture
//)";

        switch (type)
        {
            case AccidentType.happendtm:
                t_sql = @"update safety_accident set SA_HAPPENTM=@typeValue WHERE 
                 sa_id=@accidentID";
                break;
            case AccidentType.content:
                t_sql = @"update safety_accident set SA_CONTENT=@typeValue WHERE 
                sa_id=@accidentID";
                break;
            case AccidentType.type:
                t_sql = @"update safety_accident set SA_TYPE=@typeValue WHERE 
                 sa_id=@accidentID";
                break;
            case AccidentType.picture:
                t_sql = @"update safety_accident set SA_PICTURE=@typeValue WHERE 
                 sa_id=@accidentID";
                break;
            case AccidentType.picture2:
                t_sql = @"update safety_accident set SA_PICTURE2=@typeValue WHERE 
                 sa_id=@accidentID";
                break;
            case AccidentType.picture3:
                t_sql = @"update safety_accident set SA_PICTURE3=@typeValue WHERE 
                sa_id=@accidentID";
                break;
                
            default:
                t_sql = "";
                break;
        }
        SqlParameter[] t_arrParam = { new SqlParameter(), new SqlParameter() };//,new SqlParameter()};
        t_arrParam[0].ParameterName = "@accidentID";
        t_arrParam[0].Value = accidentID.ToString();
        t_arrParam[1].ParameterName = "@typeValue";
        t_arrParam[1].Value = typeValue;
        //t_arrParam[2].ParameterName = "@typeValue";
        //t_arrParam[2].Value = str;
        //t_arrParam[3].ParameterName = "@type";
        //t_arrParam[3].Value = p_type;
        //  t_arrParam[5].ParameterName = "@picture";
        //t_arrParam[5].Value = null;

        SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, t_sql, t_arrParam);
    }

    private void InsertRecord(int iRow,String p_date, String p_shift, String p_happentm, String p_type, String p_record)
    {
        string strPicture = "UploadedImages/picture" + p_date+"_" + p_shift+"_"+ ((int)(iRow * 10 + 11)).ToString();
        string strPicture2 = "UploadedImages/picture" + p_date+"_" + p_shift +"_"+ ((int)(iRow * 10 + 12)).ToString();
        string strPicture3 = "UploadedImages/picture" + p_date +"_"+ p_shift +"_"+ ((int)(iRow * 10 + 13)).ToString();
        #region SQL
        String t_sql = string.Format(@"INSERT INTO [SAFETY_ACCIDENT]
           ([SA_DATE]
           ,[SA_SHIFT]
           ,[SA_HAPPENTM]
           ,[SA_CONTENT]
           ,[SA_RECORDTM]
           ,[SA_TYPE]
       --   ,[SA_PICTURE]
         -- ,[SA_PICTURE2]
          --,[SA_PICTURE3]
)
     VALUES(@date,@shift,@happentm,@p_record,getdate(),@type--,@picture         '{0}','{1}','{2}'
        
)",strPicture,strPicture2,strPicture3);

        #endregion

        SqlParameter[] t_arrParam = { new SqlParameter(), new SqlParameter(), new SqlParameter()
            , new SqlParameter()       , new SqlParameter()};
        t_arrParam[0].ParameterName = "@date";
        t_arrParam[0].Value = p_date;
        t_arrParam[1].ParameterName = "@shift";
        t_arrParam[1].Value = p_shift;
        t_arrParam[2].ParameterName = "@happentm";
        t_arrParam[2].Value = p_happentm;
        t_arrParam[3].ParameterName ="@p_record";
        t_arrParam[3].Value = p_record;
        t_arrParam[4].ParameterName = "@type";
        t_arrParam[4].Value= p_type;
        
      //  t_arrParam[5].ParameterName = "@picture";
        //t_arrParam[5].Value = null;

        SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionStringLocalTransaction,CommandType.Text,t_sql,t_arrParam);
    }

    private string GenerateRightTM(String p_date, String p_shift, String p_happenTM)
    {
        if (!String.IsNullOrEmpty(p_happenTM) && !string.IsNullOrEmpty(p_date))
        {
            return p_date.Substring(0, 4) + "-" + p_happenTM + ":00";
            //if (p_shift == "2")
            //{
            //    return p_date.Substring(0, 4) + "-" + p_happenTM + ":00";
            //}
            //else if (p_shift == "1")
            //{
            //    if (Convert.ToInt32(p_happenTM.Substring(6, 2)) < 8)
            //    {
            //        return Convert.ToDateTime(p_date).AddDays(1).ToString("yyyy-MM-dd") + " " + p_happenTM.Substring(6, 5) + ":00";

            //    }
            //    else if (Convert.ToInt32(p_happenTM.Substring(6, 2)) > 20)
            //    {
            //        return p_date.Substring(0, 4) + "-" + p_happenTM + ":00";
            //    }
            //    return "";
            //}
            //else
            //    return "";
            //    //return Convert.ToDateTime(

        }
        else
        {
            return string.Empty;
        }
    }

    private string GetValue(String p_date, String p_shift)
    {
        #region SQL

        string t_sql = @"SELECT [SA_ID]
      ,[SA_DATE]
      ,[SA_SHIFT]
      ,[SA_HAPPENTM]
      ,[SA_CONTENT]
      ,[SA_RECORDTM]
      ,[SA_TYPE]
      ,[SA_PICTURE]
      ,[SA_PICTURE2]
       ,[SA_PICTURE3]
  FROM  [SAFETY_ACCIDENT] where sa_date=@date and sa_shift=@shift";

        #endregion

        SqlParameter[] t_arrParam = { new SqlParameter(), new SqlParameter() };
        t_arrParam[0].ParameterName = "@date";
        t_arrParam[0].Value = p_date;
        t_arrParam[1].ParameterName = "@shift";
        t_arrParam[1].Value = p_shift;
        SqlDataReader t_reader = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, t_sql, t_arrParam);
        String t_result = string.Empty;

        while(t_reader.Read())
        {
            t_result += ((t_reader.IsDBNull(3)||string.IsNullOrEmpty(t_reader.GetString(3)))?"":Convert.ToDateTime(t_reader.GetString(3)).ToString("MM-dd HH:mm"))+ ",";
            t_result += t_reader.IsDBNull(6)?"":t_reader.GetString(6)+",";
            t_result += (t_reader.IsDBNull(4) ? "" : t_reader.GetString(4))+",";
           // t_result += ",,";//Picture123
            t_result += (t_reader.IsDBNull(7) ? "" : t_reader.GetString(7))+",";
            t_result += (t_reader.IsDBNull(8) ? "" : t_reader.GetString(8))+",";
            t_result += (t_reader.IsDBNull(9) ? "" : t_reader.GetString(9));
            t_result += "|";

        }
        t_reader.Close();
        return t_result;

    }

    private void BindValue(String p_date, string p_shift)
    {
       
        this.hdValue.Value = GetValue(p_date, p_shift);
     //   Response.Write(hdValue.Value);
    }

    private void DeleteByDate(String p_date, String p_shift)
    {
        #region SQL

        string t_sql = "delete safety_accident where sa_date=@date and sa_shift=@shift";

        #endregion
        SqlParameter[] t_arrParam = { new SqlParameter(), new SqlParameter() };
        t_arrParam[0].ParameterName = "@date";
        t_arrParam[0].Value = p_date;
        t_arrParam[1].ParameterName = "@shift";
        t_arrParam[1].Value = p_shift;

        SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, t_sql, t_arrParam);
    }
}
*/
    #endregion
