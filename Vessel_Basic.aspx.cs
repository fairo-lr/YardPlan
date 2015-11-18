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

public partial class YardPlan_Vessel_Basic : System.Web.UI.Page
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

    /*
     * GridView1: LineWeek
     * GridView2: Tide 
     * GridView3: Remark
     */

    private string strDate = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        GetDate();
        if (!IsPostBack)
        {

        }
        else
        {
            ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>bodyLoad</script>");

        }

    }
    //获取其值。
    private void GetDate()
    {
        strDate = Request.QueryString["date"] + "" == ""
            ? System.DateTime.Now.ToString("yyyy-MM-dd")
            : Request.QueryString["date"] + "";
        SqlDataSource1.UpdateParameters["lw_date"].DefaultValue = strDate;
        SqlDataSource1.InsertParameters["lw_date"].DefaultValue = strDate;
        SqlDataSource2.UpdateParameters["td_date"].DefaultValue = strDate;
        SqlDataSource2.InsertParameters["td_date"].DefaultValue = strDate;

    }

    //生成本周信息
    protected void btnLine_Click(object sender, EventArgs e)
    {
        string sqlInsert = @"
delete from [YardPlan].[dbo].[LineWeek]
    INSERT INTO [YardPlan].[dbo].[LineWeek]
           ([lw_date]           ,[lw_Line]           ,[lw_BeforePort]
           ,[lw_NextPort]           ,[lw_ETA]           ,[lw_ETB]
           ,[lw_ETD]           ,[lw_suffocation]           ,[lw_gateIn]
           ,[lw_Custom]           ,[lw_terminal]           ,[lw_recordtime])
    SELECT '" +strDate+@"'	  ,[line]      ,[beforePort]
      ,[nextPort]      ,[PROETA]      ,[PROETB]
      ,[PROETD]      ,[SUFFOCATION]      ,[rcvedtm]
      ,[CUSEDTM]      ,[TEREDTM]	  ,getDate()
  FROM [YardPlan].[dbo].[ora_LineWeek_VW]";
        fnMsSetSql(sqlInsert);
        GridView1.DataBind();
        
    }

    //插入数据时候
    protected void DetailsView2_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        string strTime = ((HtmlInputText)DetailsView2.Controls[0].FindControl("iptStartTime")).Value;
        SqlDataSource2.InsertParameters["td_time"].DefaultValue = strTime;

    }
    protected void DetailsView3_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        string strPass = ((DropDownList)DetailsView3.Controls[0].FindControl("ddlPass")).SelectedValue;
        SqlDataSource3.InsertParameters["rm_passs"].DefaultValue = strPass;
    }
    protected void DetailsView5_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {

    }
}

//    private string strDate
//    {
//        set { hdDate.Value = value; }
//        get { return hdDate.Value.ToString(); }
//    }

//    //private string strShift
//    //{
//    //    set { hdShift.Value = value; }
//    //    get { return hdShift.Value.ToString(); }
//    //}

//    //private string strDay
//    //{
//    //    set { hdDay.Value = value; }
//    //    get { return hdDay.Value.ToString(); }

//    //}

//    //获取其值。
//    private void GetDate()
//    {
//        strDate = Request.QueryString["date"] + "" == ""
//            ? System.DateTime.Now.ToShortDateString()
//            : Request.QueryString["date"] + "";
//        //strShift = Request.QueryString["shift"] + "" == ""
//        //    ? "A" : Request.QueryString["shift"] + "";
//        //strDay = Request.QueryString["day"] + "" == ""
//        //    ? "0" : Request.QueryString["day"] + "";

//    }

//    protected void Page_Load(object sender, EventArgs e)
//    {
//        if (!IsPostBack)
//        {
//            GetDate();

//        }
//        SqlDsBay.SelectParameters["Bb_Date"].DefaultValue = strDate;

//    }

//    protected void btnCountBay_Click(object sender, EventArgs e)
//    {

//        string[,] arrTable = new string[500, 14];
//        //查询生产库
//        string strSql = string.Format(@" 
// --门门
//select vur_cop_copercd 类型
//,decode(dvp.dvp_opmode,'03','单提','02','门/门','') 计划 
//,decode(spl_csz_csizecd,'2','20','4','40','9','45') 尺寸
//,spl_hei_cheightcd 箱高
//,spl_ctp_isocd 箱型
//,vur_cop_copercd||decode(dvp.dvp_opmode,'03',decode(bfe_address,null,bu.bfe_buffernm,bfe_address),'02',cst_cstmnm,'')
//     ||decode(vuc.vur_csz_csizecd ,'2','20','4','40','9','45')
//     ||decode(ct.ctp_commcode ,'GP',decode(vuc.vur_hei_cheightcd,'5','HQ','6','HQ','4','HQ','E','HQ','GP'),ct.ctp_commcode)  承运人类型
//,dvp.dvp_opsttm  起始时间
//,dvp.dvp_opedtm 结束时间
//,vur_dvp_planno 计划号
//,sp.spl_regioncd 流向
//,vur_camt 作业量
//,spr_stbay 起始贝
//,spr_edbay 结束贝
//,spl_yplanid 计划ID
//from PS_STACK_PLANS sp
//     , PS_STACK_PLAN_AREAS spa
//     ,ps_van_use_c_requirements vuc
//     ,ps_devan_plans dvp
//     ,ps_customers cs
//     ,ps_buffers bu
//    ,ps_container_types ct
//where sp.spl_yplanid=spa.spr_spl_yplanid
//      and ct.ctp_isocd=vuc.vur_ctp_isocd
//and (vur_dvp_planno=spl_dvp_planno or vur_userqseq=spl_dvp_planno)
//and dvp.dvp_planno=vur_dvp_planno
//and dvp.dvp_opmode='02'
//and cs.cst_cstmcd=dvp_trn_trnscomcd
// and sp.spl_regioncd=bu.bfe_buffercd(+)
// and dvp.dvp_opedtm >=to_date('{0}','yyyy-mm-dd')
// and dvp.dvp_opsttm <=to_date('{0}','yyyy-mm-dd')+2
//and spl_type in ('PEP')
//UNION ALL 
//--单提
//select distinct vur_cop_copercd 类型
//,decode(dvp.dvp_opmode,'03','单提','02','门/门','') 计划 
//,decode(spl_csz_csizecd,'2','20','4','40','9','45') 尺寸
//,spl_hei_cheightcd 箱高
//,spl_ctp_isocd 箱型
//,vur_cop_copercd||decode(dvp.dvp_opmode,'03',decode(bfe_address,null,bu.bfe_buffernm,bfe_address),'02',cst_cstmnm,'')
//     ||decode(vuc.vur_csz_csizecd ,'2','20','4','40','9','45')
//     ||decode(ct.ctp_commcode ,'GP',decode(vuc.vur_hei_cheightcd,'5','HQ','6','HQ','4','HQ','E','HQ','GP'),ct.ctp_commcode)  承运人类型
//,dvp.dvp_opsttm  起始时间
//,dvp.dvp_opedtm 结束时间
//,vur_dvp_planno 计划号
//,sp.spl_regioncd 流向
//,vur_camt 作业量
//,spr_stbay 起始贝
//,spr_edbay 结束贝
//,spl_yplanid 计划ID
//from PS_STACK_PLANS sp,ps_van_use_c_requirements vuc,ps_devan_plans dvp,ps_customers cs,ps_buffers bu,ps_container_types ct
//    ,ps_plan_containers plc,ps_stack_plan_areas spa,ps_yard_containers iyc
//where spa.spr_spl_yplanid=sp.spl_yplanid
//and  (vur_dvp_planno=spl_dvp_planno or vur_userqseq=spl_dvp_planno)
//and dvp.dvp_planno=vur_dvp_planno
//and plc.plc_dvp_planno(+)=dvp.dvp_planno
//and cs.cst_cstmcd=dvp_trn_trnscomcd
//and ct.ctp_isocd=vuc.vur_ctp_isocd
// and dvp_bfe_buffercd=bu.bfe_buffercd(+)
// and dvp.dvp_opedtm >=to_date('{0}','yyyy-MM-dd hh24')
// and dvp.dvp_opsttm <=to_date('{0}','yyyy-MM-dd hh24')+2
// and plc.plc_dvp_planno is not  null
//  and iyc.iyc_cntrid=plc.plc_cntrid
//  and iyc_type='SAC'
// and spl_type in ('PEP') 
//and dvp.dvp_opmode='03'
//--and vur_acamt is not null
//and to_number(vur_camt)-to_number(decode(vur_acamt,null,'0',vur_acamt))<>0
//
//union all
//
//select vur_cop_copercd 类型
//,decode(dvp.dvp_opmode,'03','单提','02','门/门','') 计划 
//,decode(spl_csz_csizecd,'2','20','4','40','9','45') 尺寸
//,spl_hei_cheightcd 箱高
//,spl_ctp_isocd 箱型
//,vur_cop_copercd||decode(dvp.dvp_opmode,'03',decode(bfe_address,null,bu.bfe_buffernm,bfe_address),'02',cst_cstmnm,'')
//     ||decode(vuc.vur_csz_csizecd ,'2','20','4','40','9','45')
//     ||decode(ct.ctp_commcode ,'GP',decode(vuc.vur_hei_cheightcd,'5','HQ','6','HQ','4','HQ','E','HQ','GP'),ct.ctp_commcode)  承运人类型
//,dvp.dvp_opsttm  起始时间
//,dvp.dvp_opedtm 结束时间
//,vur_dvp_planno 计划号
//,sp.spl_regioncd 流向
//,vur_camt 作业量
//,spr_stbay 起始贝
//,spr_edbay 结束贝
//,spl_yplanid 计划ID
//from PS_STACK_PLANS sp,ps_van_use_c_requirements vuc,ps_devan_plans dvp,ps_customers cs,ps_buffers bu,ps_container_types ct
//    ,ps_plan_containers plc ,ps_stack_plan_areas spa
//where sp.spl_yplanid=spa.spr_spl_yplanid    
//and (vur_dvp_planno=spl_dvp_planno or vur_userqseq=spl_dvp_planno)
//and dvp.dvp_planno=vur_dvp_planno
//and plc.plc_dvp_planno(+)=dvp.dvp_planno
//and cs.cst_cstmcd=dvp_trn_trnscomcd
//and ct.ctp_isocd=vuc.vur_ctp_isocd
// and dvp_bfe_buffercd=bu.bfe_buffercd(+)
// and dvp.dvp_opedtm >=to_date('{0}','yyyy-MM-dd hh24')
//and dvp.dvp_opsttm<= to_date('{0}','yyyy-MM-dd hh24')+2
// and plc.plc_dvp_planno is  null
//and spl_type in ('PEP') 
//and dvp.dvp_opmode='03'
//--and vur_acamt is not null
//and to_number(vur_camt)-to_number(decode(vur_acamt,null,'0',vur_acamt))<>0 
//order by 2,4
//
//", strDate.Substring(0, 10));

//        //判断是否已经存在相关记录，存在则删除后在插入
//        string sqlSelect = string.Format(@"
//           SELECT [Bb_id], [Bb_Date], [Bb_PlanType], [Bb_cntrSize], [Bb_cntrHeight], [Bb_cntrType], [Bb_cstmnm]
//                , [Bb_planStartTime], [Bb_PlanEndTime], [Bb_PlanNo], [Bb_PlanTo], [Bb_PlanNum], [Bb_startBay]
//                , [Bb_endBay] 
//            FROM [Busy_Bay]
//             WHERE [Bb_Date]='{0}'"
//            , strDate);
//        string sqlDel = string.Format(@"
//            Delete From Busy_Bay
//             WHERE [Bb_Date]='{0}' "
//            , strDate);

//        int count = fnMsGetCount(sqlSelect);

//        if (count != 0)
//        {
//            fnMsSetSql(sqlDel);
//        }

//        arrTable = FnGetValue(strSql, arrTable);

//        for (int i = 0; i < arrTable.GetLength(0); i++)
//        {
//            if (!string.IsNullOrEmpty(arrTable[i, 0]))
//            {
//                string strInsert = string.Format(@"INSERT INTO 
//        [Busy_Bay] ([Bb_Date], [Bb_PlanType],[Bb_Plan], [Bb_cntrSize], [Bb_cntrHeight], [Bb_cntrType]
//        , [Bb_cstmnm], [Bb_planStartTime], [Bb_PlanEndTime], [Bb_PlanNo], [Bb_PlanTo]
//        , [Bb_PlanNum]    , [Bb_startBay], [Bb_endBay], [Bb_Planid]) 
//     VALUES
//           ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')"
//                    , strDate, arrTable[i, 0], arrTable[i, 1]
//                     , arrTable[i, 2], arrTable[i, 3], arrTable[i, 4], arrTable[i, 5], arrTable[i, 6]
//                       , arrTable[i, 7], arrTable[i, 8], arrTable[i, 9], arrTable[i, 10], arrTable[i, 11]
//                         , arrTable[i, 12], arrTable[i, 13]);

//                fnMsSetSql(strInsert);

//            }
//            else
//            {
//                break;
//            }
//        }
//        //gvVessel.Dispose();
//        gvBay.DataBind();

//        lblCount.Text = fnMsGetCount(sqlSelect).ToString();
//        //Response.Write(fnMsGetCount(sqlSelect));
//        // SqlDsVessel.DataBind();
//    }


//    //返回记录数
//    private int fnMsGetCount(string strSelect)
//    {
//        int iReturn = 0;
//        string strCount = string.Format("SELECT count(*) FROM ({0}) a", strSelect);
//        System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(
//            ConfigurationManager.ConnectionStrings["TestControlDailyConnectionString"].ConnectionString);
//        System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand(
//            strCount, sqlConn);
//        sqlConn.Open();
//        System.Data.SqlClient.SqlDataReader sqlData = sqlComm.ExecuteReader();

//        if (sqlData.Read())
//        {
//            iReturn = int.Parse(sqlData.GetValue(0).ToString());

//        }
//        sqlData.Dispose();
//        sqlComm.Dispose();
//        sqlConn.Dispose();

//        return iReturn;

//    }

//    //返回修改记录数
//    private int fnMsSetSql(string strSelect)
//    {
//        int iReturn = 0;
//        string strCount = string.Format(strSelect);
//        System.Data.SqlClient.SqlConnection sqlConn = new SqlConnection(
//            ConfigurationManager.ConnectionStrings["TestControlDailyConnectionString"].ConnectionString);
//        System.Data.SqlClient.SqlCommand sqlComm = new SqlCommand(
//            strCount, sqlConn);
//        sqlConn.Open();
//        iReturn = sqlComm.ExecuteNonQuery();

//        sqlComm.Dispose();
//        sqlConn.Dispose();

//        return iReturn;

//    }

//    //根据SQL返回数据的值，如果为空返回的是null
//    private string[,] FnGetValue(string sql, string[,] arrValue)
//    {
//        string strSql = sql;
//        int m = arrValue.GetLength(0);
//        int n = arrValue.GetLength(1);

//        string[,] arrDetails = new string[m, n];

//        System.Data.OracleClient.OracleConnection oraConn =
//            new System.Data.OracleClient.OracleConnection(
//            ConfigurationManager.ConnectionStrings["oraConnectionString"].ConnectionString);
//        System.Data.OracleClient.OracleCommand oraComm = new System.Data.OracleClient.OracleCommand(strSql, oraConn);

//        oraConn.Open();
//        System.Data.OracleClient.OracleDataReader oraData = oraComm.ExecuteReader();

//        //读取相关数据
//        for (int i = 0; i < m; i++)
//        {
//            if (oraData.Read())
//            {
//                for (int j = 0; j < n; j++)
//                {
//                    arrDetails[i, j] = oraData.IsDBNull(j) ? null : oraData.GetValue(j).ToString();
//                }
//            }
//            else
//            {
//                break;
//            }
//        }
//        oraData.Close();
//        oraComm.Cancel();
//        oraConn.Close();
//        oraData.Dispose();
//        oraComm.Dispose();
//        oraConn.Dispose();
//        // arrReturn = fnSetLeftingArr(arrDetails);
//        return arrDetails;
//    }