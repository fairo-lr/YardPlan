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

public partial class Busy_Plan : System.Web.UI.Page
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

    #region SQL
    string strSQL = @"SELECT 
s.sln_lne_rtcd 航线,
s.sln_vcm_cstmcd 船公司,
vls_vchnnm||'   '||vls_vengnm 船名,
b.voc_ivoyage 进口航次,
b.voc_expvoyage 出口航次,
'周'||substr(to_char(b.voc_arrivetm,'day'),3,1)||'/'||to_char(b.voc_arrivetm,'hh24:mi') PROETA,
'周'||substr(to_char(b.voc_arrivetm+1/24,'day'),3,1)||'/'||to_char(b.voc_arrivetm+1/24,'hh24:mi') PROETB,
'周'||substr(to_char(b.voc_dporttm,'day'),3,1)||'/'||to_char(b.voc_dporttm,'hh24:mi') PROETD,
to_char(VBT_PBTHTM-1/24,'DD')||'/'||to_char(VBT_PBTHTM-1/24,'hh24:mi') FORETA,
to_char(VBT_PBTHTM,'DD')||'/'||to_char(VBT_PBTHTM,'hh24:mi') FORETB,
to_char(VBT_PDPTTM,'DD')||'/'||to_char(VBT_PDPTTM,'hh24:mi') FORETD,
to_char(b.VOC_END_SUFFOCATION,'day')||'/'||to_char(b.VOC_END_SUFFOCATION,'hh24:mi') 截熏蒸,
to_char(a.voc_rcvedtm,'day')||'/'||to_char(a.voc_rcvedtm,'hh24:mi') 进箱,
to_char(a.voc_rcvedtm,'day')||'/'||to_char(a.voc_rcvedtm,'hh24:mi') 截箱箱,
to_char(b.VOC_CUSEDTM,'day')||'/'||to_char(b.VOC_CUSEDTM,'hh24:mi') 海关截单,
to_char(b.VOC_TEREDTM,'day')||'/'||to_char(b.VOC_TEREDTM,'hh24:mi') 码头截单,
a.VOC_OCRRID
FROM ps_vessels,ps_vie_voc_vwf a,ps_shipping_lines ,ps_service_lines s,ps_customers c,PS_VESSEL_OCCURENCES b
 WHERE a.voc_ocrrid=b.voc_ocrrid and vls_vnamecd=a.voc_vls_vnamecd
  AND a.voc_rcvsttm IS NOT NULL AND vie_iefg = 'E'-- AND VLS_VTYPE<>'BAR' 
  and b.VOC_TEREDTM>to_date('2000-01-01','YYYY-MM-DD') 
  and SLN_SLINEID=VIE_SLN_SLINEID
   and LNE_RTCD=SLN_LNE_RTCD 
   and sln_agt_cstmcd=cst_cstmcd 
   and vbt_pbthtm>=sysdate+7 
   order by a.VBT_PBTHTM
";
    #endregion

    private string strDate = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        GetDate();
        checkButton();
        if (!IsPostBack)
        {

        }
        else
        {
        }
        Button1.Click += new EventHandler(updateReport);
        Button2.Click += new EventHandler(updateReport);
        Button3.Click += new EventHandler(updateReport);
        Button4.Click += new EventHandler(updateReport);
        Button5.Click += new EventHandler(updateReport);
        Button6.Click += new EventHandler(updateReport);
        GridView4.RowCommand += new GridViewCommandEventHandler(updateReport);

    }
    protected void updateReport(object sender, EventArgs e)
    {
        updateBackReport();
    }

    private void checkButton()
    {
        string sqlsubmit = @"SELECT top 1 vi_submit
  FROM [YardPlan].[dbo].[VesselInfo]
where [vi_date] ='" + strDate + @"'
order by vi_date desc ";
        string isSubmit = fnMsGetValue(sqlsubmit);
        if (isSubmit == "Y")
        {
            Button6.Text = "船期确认已更新！";
            Button6.Enabled = false;
        }
    }

    private void GetDate()
    {
        strDate = Request.QueryString["date"] + "" == ""
            ? System.DateTime.Now.ToString("yyyy-MM-dd")
            : Request.QueryString["date"] + "";
        //SqlDataSource1.SelectParameters["date"].DefaultValue = strDate;
        //SqlDataSource2.SelectParameters["date"].DefaultValue = strDate;
        SqlDataSource4.SelectParameters["date"].DefaultValue = strDate;
        hypNewPage.NavigateUrl = "Vessel_Plan.aspx?date" + strDate;

        lblTopsTitle.Text = "生产系统船期计划: " + System.DateTime.Now.ToString("yyyy-MM-dd") + "到"
            + System.DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
        lblWebTitle.Text = "Web系统船期计划: " + strDate + "到"
            + System.DateTime.Parse(strDate).AddDays(7).ToString("yyyy-MM-dd");

        //GridView1.DataBind();
        //GridView2.DataBind();    
        //GridView4.DataBind();

    }

    //更新船期
    protected void Button1_Click(object sender, EventArgs e)
    {

        for (int i = GridView3.Rows.Count - 1; i >= 0; i--)
        {
            if (((CheckBox)(GridView3.Rows[i].FindControl("cbSel3"))).Checked == true)
            {
                string strVesselID = GridView3.DataKeys[i].Values[0].ToString();
                #region
                string sqlInsertVessel = @"
declare @findID int;
DECLARE @longpole int;
DECLARE @finsh varchar(10);
BEGIN TRANSACTION
SELECT    @longpole =[vs_longpole]     , @finsh=[vs_finsh] 
 FROM [Vessel]
where vs_date='" + strDate + @"' and vs_vesselcode='" + strVesselID + @"';
    DELETE FROM [YardPlan].[dbo].[Vessel] WHERE vs_date='" + strDate + @"' and vs_vesselcode='" + strVesselID + @"'
INSERT INTO [YardPlan].[dbo].[Vessel]
           ([vs_date]           ,[vs_line]           ,[vs_customer]
           ,[vs_vesselename]           ,[vs_vesselcname]           ,[vs_vesselcode]
           ,[vs_ivoyage]           ,[vs_expvoyage]           ,[vs_proformaETA]
           ,[vs_proformaETB]           ,[vs_proformaETD]           ,[vs_forecastETA]
           ,[vs_forecastETB]           ,[vs_forecastETD]           ,[vs_finshtime]
           ,[vs_remark]           ,[vs_longpole]           ,[vs_worknum]
           ,[vs_vesseltime]           ,[vs_gold]           ,[vs_finsh]
        ,vs_internalfg,vs_intrade,vs_berthLength,vs_departLength
           )
       SELECT '" + strDate + @"', line, cstmcd, ename,cname,
	wp.OCRRID,ivoyage,expvoyage, PROETA,PROETB,
	PROETD,FORETA,FORETB, FORETD,
	'','',isNull(@longpole,''),gcranenum,allsum*60/
datediff(mi,FORETB,FORETD),
dbo.getVesselGold_fn(allsum,line)
,@finsh,internalfg,intrade,berthLength,departLength		
From ora_WeekPlan_VW as wp left join (SELECT [ocrrid]    
      ,sum(isnull([IF20],0)
	+isNull([IF40],0)
      +isNull([IF45],0)
      +isNull([IE20],0)
      +isNull([IE40],0)
      +isNull([IE45],0)
      +isNull([OF20],0)
      +isNull([OF40],0)
      +isNull([OF45],0)
      +isNull([OE20],0)
      +isNull([OE40],0)
      +isNull([OE45],0)) allsum
  FROM [YardPlan].[dbo].[ora_WeekContainer_VW]
group by [ocrrid]) as wc
on  wp.OCRRID=wc.OCRRID
where wp.OCRRID='" + strVesselID + @"'
commit;";
                #endregion
                fnMsSetSql(sqlInsertVessel);
            }
        }

        GridView3.DataBind();
        GridView4.DataBind();
    }

    //更新箱量    
    protected void Button2_Click(object sender, EventArgs e)
    {
        for (int i = GridView3.Rows.Count - 1; i >= 0; i--)
        {
            if (((CheckBox)(GridView3.Rows[i].FindControl("cbSel3"))).Checked == true)
            {
                string strVesselID = GridView3.DataKeys[i].Values[0].ToString();
                #region
                string sqlInsertContainer = @"
     --Delete删除数据
          DELETE FROM [YardPlan].[dbo].[VesselContainer] WHERE vc_date='" + strDate
           + @"' and vc_vesselcode='" + strVesselID + @"'
    --插入数据
        INSERT INTO [YardPlan].[dbo].[VesselContainer]
           ([vc_date]           ,[vc_vesselename]           ,[vc_vesselcname]
           ,[vc_vesselcode]           ,[vc_ivoyage]           ,[vc_expvoyage]
           ,[vc_cntrowner]           ,[vc_20IF]           ,[vc_20IE]
           ,[vc_20OF]           ,[vc_20OE]           ,[vc_40IF]
           ,[vc_40IE]           ,[vc_40OF]           ,[vc_40OE]
           ,[vc_45IF]           ,[vc_45IE]           ,[vc_45OF]
           ,[vc_45OE]           ,[vc_sum]           ,[vc_recordtime])    
SELECT '" + strDate + @"',[ename] ,[cname]
	,wp.[ocrrid]       ,[ivoyage]      ,[expvoyage]
      ,[copercd]      ,isNull([IF20],0)  ,isNull([IE20],0)
	  ,isNull([OF20],0)  ,isNull([OE20],0)      ,isNull([IF40],0)
	  ,isNull([IE40],0), isNull([OF40],0)     ,isNull([OE40],0)
      ,isNull([IF45],0) ,isNull([IE45],0) ,isNull([OF45],0) ,isNull([OE45],0)
	  ,ISNULL([IF20],0)+ISNULL([IE20],0)+
		ISNULL([OF20],0)+ISNULL([OE20],0)+
		ISNULL([IF40],0)+ISNULL([IE40],0)+
		ISNULL([OF40],0)+ISNULL([OE40],0)+
        ISNULL([IF45],0)+ISNULL([IE45],0)+
		ISNULL([OF45],0)+ISNULL([OE45],0)
		,GETDATE()
  FROM [YardPlan].[dbo].[ora_WeekPlan_VW] AS WP
	LEFT JOIN [YardPlan].[dbo].[ora_WeekContainer_VW] AS WC
  on wp.[ocrrid]=wc.[ocrrid]
where wp.OCRRID='" + strVesselID + "'";

                #endregion
                fnMsSetSql(sqlInsertContainer);

            }
        }
        GridView3.DataBind();
        GridView4.DataBind();
    }

    protected void cbSelAll4_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cbAll = (CheckBox)sender;
        if (cbAll.Text == "全选")
        {
            foreach (GridViewRow gvr in GridView4.Rows)
            {
                CheckBox cbSel = (CheckBox)gvr.Cells[0].FindControl("cbSel4");
                cbSel.Checked = cbAll.Checked;
            }
        }

        foreach (GridViewRow gvr in GridView4.Rows)
        {
            CheckBox cbSel = (CheckBox)gvr.Cells[0].FindControl("cbSel4");
            if (cbSel.Checked == false)
            {
                //...
            }
        }
    }

    protected void cbSelAll3_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cbAll = (CheckBox)sender;
        if (cbAll.Text == "全选")
        {
            foreach (GridViewRow gvr in GridView3.Rows)
            {
                CheckBox cbSel = (CheckBox)gvr.Cells[0].FindControl("cbSel3");
                cbSel.Checked = cbAll.Checked;
            }
        }

        foreach (GridViewRow gvr in GridView3.Rows)
        {
            CheckBox cbSel = (CheckBox)gvr.Cells[0].FindControl("cbSel3");
            if (cbSel.Checked == false)
            {
                //...
            }
        }
    }

    protected void GridView4_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        #region  SQL
        string strUpdateVessel = @"UPDATE [Vessel] 
                SET  [vs_remark] = @vs_remark, [vs_longpole] = @vs_longpole
            , [vs_worknum] = @vs_worknum, [vs_vesseltime] = @vs_vesseltime, [vs_gold] = @vs_gold
    , [vs_finsh] = @vs_finsh, [vs_recordtime] = getDate() WHERE [vs_id] = @vs_id;
update vesselContainer set vc_recordtime =getDate()
where vc_date=(select vs_date from vessel where vs_id=@vs_id);
";

        string strUpdateContainer = @"UPDATE [VesselContainer] SET [vc_vesselename] = @vs_vesselename
    , [vc_vesselcname] = @vs_vesselcname, [vc_vesselcode] = @vs_vesselcode
, [vc_ivoyage] = @vs_ivoyage, [vc_expvoyage] = @vs_expvoyage
, [vc_cntrowner] = @vc_cntrowner, [vc_20IF] = @vc_20IF, [vc_20IE] = @vc_20IE
, [vc_20OF] = @vc_20OF, [vc_20OE] = @vc_20OE, [vc_40IF] = @vc_40IF
, [vc_40IE] = @vc_40IE, [vc_40OF] = @vc_40OF, [vc_40OE] = @vc_40OE
, [vc_45IF] = @vc_45IF, [vc_45IE] = @vc_45IE, [vc_45OF] = @vc_45OF
, [vc_45OE] = @vc_45OE
,vc_recordtime=getDate()
, [vc_sum] = ( isNull(@vc_20OF,0)+isNull(@vc_40OF,0)+isNull(@vc_45OF,0)
        +isNull(@vc_20OE,0)+isNull(@vc_40OE,0)+isNull(@vc_45OE,0)
        +isNull(@vc_20IF,0)+isNull(@vc_40IF,0)+isNull(@vc_45IF,0)
        +isNull(@vc_20IE,0)+isNull(@vc_40IE,0)+isNull(@vc_45IE,0) )
WHERE [vc_id] = @vc_id;
update vessel set [vs_vesseltime] =  ( isNull(@vc_20OF,0)+isNull(@vc_40OF,0)+isNull(@vc_45OF,0)
        +isNull(@vc_20OE,0)+isNull(@vc_40OE,0)+isNull(@vc_45OE,0)
        +isNull(@vc_20IF,0)+isNull(@vc_40IF,0)+isNull(@vc_45IF,0)
        +isNull(@vc_20IE,0)+isNull(@vc_40IE,0)+isNull(@vc_45IE,0) )*60/
(select datediff(mi, vs_forecastETB, vs_forecastETD)from vessel where vs_id=@vs_id)
where [vs_id] = @vs_id;
update vesselContainer set vc_recordtime =getDate()
where vc_date=(select  vc_date from vesselContainer  where vc_id=@vc_id);";

        string strInsertVessel = @"INSERT INTO [Vessel] ([vs_date], [vs_line], [vs_customer], [vs_vesselename]
            , [vs_vesselcname], [vs_vesselcode], [vs_ivoyage], [vs_expvoyage], [vs_proformaETA], [vs_proformaETB]
        , [vs_proformaETD], [vs_forecastETA], [vs_forecastETB], [vs_forecastETD], [vs_finshtime]
        , [vs_remark], [vs_longpole], [vs_worknum], [vs_vesseltime], [vs_gold], [vs_finsh], [vs_recordtime])
    VALUES (@vs_date, @vs_line, @vs_customer, @vs_vesselename, @vs_vesselcname, @vs_vesselcode
    , @vs_ivoyage, @vs_expvoyage, @vs_proformaETA, @vs_proformaETB, @vs_proformaETD, @vs_forecastETA
    , @vs_forecastETB, @vs_forecastETD, @vs_finshtime, @vs_remark, @vs_longpole
    , @vs_worknum, @vs_vesseltime, @vs_gold, @vs_finsh, @vs_recordtime);";

        string strInsertContainer = @"INSERT INTO [VesselContainer] ([vc_date], [vc_vesselename]
    , [vc_vesselcname], [vc_vesselcode], [vc_ivoyage], [vc_expvoyage]
    , [vc_cntrowner], [vc_20IF], [vc_20IE], [vc_20OF], [vc_20OE], [vc_40IF]
    , [vc_40IE], [vc_40OF], [vc_40OE], [vc_45IF], [vc_45IE], [vc_45OF], [vc_45OE]
    , [vc_sum], [vc_recordtime]) VALUES ('" + strDate + @"', @vs_vesselename   
    , @vs_vesselcname, @vs_vesselcode, @vs_ivoyage, @vs_expvoyage
    , @vc_cntrowner, @vc_20IF, @vc_20IE, @vc_20OF, @vc_20OE, @vc_40IF
    , @vc_40IE, @vc_40OF, @vc_40OE, @vc_45IF, @vc_45IE, @vc_45OF
    , @vc_45OE, @vc_sum, getDate())";

        string strDeleteVessel = @"DELETE FROM [VesselContainer] WHERE vc_date='" + strDate + @"'
 and vc_vesselcode=(select vs_vesselcode from vessel 
	where vs_id=@vs_id )
DELETE FROM [Vessel] WHERE [vs_id] = @vs_id ";
        #endregion

        int iRow = int.Parse(e.CommandArgument.ToString());
        //修改样式
        if (e.CommandName == "Edit")
        {
            GridView4.Columns[54].Visible = false;//Delete
            GridView4.Columns[53].Visible = true;//Cancel
            GridView4.Columns[52].Visible = true;//Container
            GridView4.Columns[51].Visible = true;//Vessel
            GridView4.Columns[50].Visible = false;//Edit
            GridView4.Columns[1].Visible = true;//vs_id
            GridView4.Columns[2].Visible = true;//vc_id
            for (int i = 15; i < 17; i++)
            {
                GridView4.Columns[i].Visible = false;
            }
        }
        else
        {
            GridView4.Columns[54].Visible = true;//禁用删除
            GridView4.Columns[53].Visible = false;
            GridView4.Columns[52].Visible = false;
            GridView4.Columns[51].Visible = false;
            GridView4.Columns[50].Visible = true;
            GridView4.Columns[1].Visible = false;
            GridView4.Columns[2].Visible = false;
            for (int i = 15; i < 17; i++)
            {
                GridView4.Columns[i].Visible = true;
            }
        }

        //修改相关的内容
        if (e.CommandName == "Edit")
        {

        }
        else if (e.CommandName == "btnVessel")
        {
            SqlDataSource4.UpdateCommand = strUpdateVessel;
            SqlDataSource4.DataBind();

            GridView4.UpdateRow(iRow, false);

            GridView4.DataBind();
        }
        else if (e.CommandName == "btnContainer")
        {
            SqlDataSource4.UpdateCommand = strUpdateContainer;
            SqlDataSource4.DataBind();

            GridView4.UpdateRow(iRow, false);
            GridView4.DataBind();
        }
        else if (e.CommandName == "btnDeleteVessel")
        {
            SqlDataSource4.DeleteCommand = strDeleteVessel;
            SqlDataSource4.DataBind();
            fnMsSetSql(strDeleteVessel.Replace("@vs_id", GridView4.DataKeys[iRow].Values[0].ToString()));
            //GridView4.DataKeys[iRow].Values[1] = 3;
            //GridView4.DeleteRow(iRow);
            GridView4.DataBind();
        }
        else if (e.CommandName == "Cancel")
        {

        }
        GridView4.DataBind();
    }

    //合并行
    public void MergeRows(GridView gridView, int KeyIndex, int Length)
    {
        int VisableColumn = 0;
        for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridViewRow row = gridView.Rows[rowIndex];
            GridViewRow previousRow = gridView.Rows[rowIndex + 1];
            //用于判断的列是否一致 

            if (gridView.DataKeys[rowIndex].Values[KeyIndex].ToString()
                == gridView.DataKeys[rowIndex + 1].Values[KeyIndex].ToString())
            {
                //选择从起始到现在的行
                for (int i = 0; i < row.Cells.Count && i < Length; i++)
                {
                    row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                           previousRow.Cells[i].RowSpan + 1;
                    previousRow.Cells[i].Visible = false;
                }
                VisableColumn++;     //统计列
            }
        }

        for (int i = 0; i < gridView.Rows.Count; i++)
        {
            GridViewRow gvRow = gridView.Rows[i];
            if (gvRow.Cells[KeyIndex].RowSpan == 0 && gvRow.Cells[KeyIndex].Visible == true)
            {                //没有没有重复记录               
                VisableColumn--;
                gvRow.Style.Remove(HtmlTextWriterStyle.BackgroundColor);
                gvRow.Style.Add(HtmlTextWriterStyle.BackgroundColor,
                    (VisableColumn % 2 == 0 ? gridView.RowStyle.BackColor.Name
                    : "#E0E0E0"));
            }
            else if (gvRow.Cells[KeyIndex].RowSpan != 0 && gvRow.Cells[KeyIndex].Visible == true)
            {               // 重复记录的表头
                VisableColumn--;
                gvRow.Style.Remove(HtmlTextWriterStyle.BackgroundColor);
                gvRow.Style.Add(HtmlTextWriterStyle.BackgroundColor,
                    (VisableColumn % 2 == 0 ? gridView.RowStyle.BackColor.Name
                    : "#E0E0E0"));
            }
            else if (gvRow.Cells[KeyIndex].Visible == false)
            {            //非表头的重复记录            

                gvRow.Style.Remove(HtmlTextWriterStyle.BackgroundColor);
                gvRow.Style.Add(HtmlTextWriterStyle.BackgroundColor,
                    (VisableColumn % 2 == 0 ? gridView.RowStyle.BackColor.Name
                    : "#E0E0E0"));
            }
            if (gridView.ID == "GridView3")
            {
                //两天内的红色显示
                string strArr = gvRow.Cells[7].Text.Split('/').GetValue(0).ToString();
                int iDate = System.DateTime.Today.AddDays(2).Day - Convert.ToInt32(strArr);
                if (iDate > 0 && iDate < 7)
                {
                    gvRow.Style.Add(HtmlTextWriterStyle.Color, "Red");
                }
            }

        }
    }

    //比较列
    private void CompareRows(GridView GridView4, GridView GridView3)
    {

        for (int rowIndex = GridView4.Rows.Count - 1; rowIndex >= 0; rowIndex--)
        {
            GridViewRow gvRow = GridView4.Rows[rowIndex];
            string VesselID = GridView4.DataKeys[rowIndex].Values[2].ToString();
            string cntrOwner = gvRow.Cells[35].Text;
            for (int cmpIndex = GridView3.Rows.Count - 1; cmpIndex >= 0; cmpIndex--)
            {
                GridViewRow cmpRow = GridView3.Rows[cmpIndex];
                string cmpID = GridView3.DataKeys[cmpIndex].Values[0].ToString();
                string cmpOwner = cmpRow.Cells[10].Text;
                if (VesselID == cmpID)
                {
                    #region 船舶信息根据VID
                    string[] eta = cmpRow.Cells[7].Text.Split(',');
                    string[] etb = cmpRow.Cells[8].Text.Split(',');
                    string[] etd = cmpRow.Cells[9].Text.Split(',');

                    for (int i = 1; i <= 1; i++)
                    {
                        if (gvRow.Cells[13 + i * 3].Text != eta[i - 1])
                        {
                            gvRow.Cells[13 + i * 3].BackColor = System.Drawing.Color.LightBlue;
                            cmpRow.Cells[7].BackColor = System.Drawing.Color.LightBlue;
                        }
                        if (gvRow.Cells[14 + i * 3].Text != etb[i - 1])
                        {
                            gvRow.Cells[14 + i * 3].BackColor = System.Drawing.Color.LightBlue;
                            cmpRow.Cells[8].BackColor = System.Drawing.Color.LightBlue;
                        }
                        if (gvRow.Cells[15 + i * 3].Text != etd[i - 1])
                        {
                            gvRow.Cells[15 + i * 3].BackColor = System.Drawing.Color.LightBlue;
                            cmpRow.Cells[9].BackColor = System.Drawing.Color.LightBlue;
                        }
                    }
                    #endregion
                    #region 根据持箱人
                    if (cntrOwner.Trim() == cmpOwner.Trim())
                    {
                        //箱信息
                        string[] cif = cmpRow.Cells[11].Text.Split('\\');
                        string[] cie = cmpRow.Cells[12].Text.Split('\\');
                        string[] cof = cmpRow.Cells[13].Text.Split('\\');
                        string[] coe = cmpRow.Cells[14].Text.Split('\\');
                        for (int i = 0; i < 3; i++)
                        {
                            if (cif[i] != gvRow.Cells[gvRow.Cells.Count - 19 + i].Text.Trim())
                            {
                                gvRow.Cells[gvRow.Cells.Count - 19 + i].BackColor = System.Drawing.Color.LightBlue;
                                cmpRow.Cells[11].BackColor = System.Drawing.Color.LightBlue;
                            }
                            if (cie[i] != gvRow.Cells[gvRow.Cells.Count - 16 + i].Text.Trim())
                            {
                                gvRow.Cells[gvRow.Cells.Count - 16 + i].BackColor = System.Drawing.Color.LightBlue;
                                cmpRow.Cells[12].BackColor = System.Drawing.Color.LightBlue;
                            }

                            if (cof[i] != gvRow.Cells[gvRow.Cells.Count - 13 + i].Text.Trim())
                            {
                                gvRow.Cells[gvRow.Cells.Count - 13 + i].BackColor = System.Drawing.Color.LightBlue;
                                cmpRow.Cells[13].BackColor = System.Drawing.Color.LightBlue;
                            }

                            if (coe[i] != gvRow.Cells[gvRow.Cells.Count - 10 + i].Text.Trim())
                            {
                                gvRow.Cells[gvRow.Cells.Count - 10 + i].BackColor = System.Drawing.Color.LightBlue;
                                cmpRow.Cells[14].BackColor = System.Drawing.Color.LightBlue;
                            }
                        }


                        // break;
                    }
                    //else
                    //{
                    //    //continue;
                    //}

                }
            }
                    #endregion
        }
    }

    protected void GridView4_PreRender(object sender, EventArgs e)
    {
        if (GridView4.DataKeys.Count != 0)
        {
            lblWebTime.Text = GridView4.DataKeys[0].Values[3].ToString();
        }
        //绑定头部
        for (int i = 0; i < 11; i++)
        {
            this.GridView4.Columns[i].HeaderStyle.CssClass = "fixColleft1";
            this.GridView4.Columns[i].ItemStyle.CssClass = "fixColleft1";
        }
        this.GridView4.Columns[7].ControlStyle.Width = Unit.Parse("60px");
        this.GridView4.Columns[8].ControlStyle.Width = Unit.Parse("60px");
        MergeRows(GridView4, 2, 28);

        CompareRows(GridView4, GridView3);

        //GridView4.DataBind();
    }




    protected void GridView3_PreRender(object sender, EventArgs e)
    {
        lblTopsTime.Text = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        for (int i = 0; i < 6; i++)
        {
            this.GridView3.Columns[i].HeaderStyle.CssClass = "fixColleft1";
            this.GridView3.Columns[i].ItemStyle.CssClass = "fixColleft1";
        }
        MergeRows(GridView3, 0, 10);

        //    GridView3.DataBind();
    }
    protected void SqlDataSource4_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {

    }
    protected void GridView4_DataBound(object sender, EventArgs e)
    {
        //updateBackReport();
        //GridView4.Columns[GridView4.Columns.Count - 1];
        //((Button)GridView4.Columns[GridView4.Columns.Count - 1]
        //    ).Attributes.Add("OnClientClick", "return btnCheck('将覆盖原有船期信息，是否确定')");
    }
    protected void GridView4_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //GridView4.Rows[e.Row.RowIndex].FindControl("btnDeleteVessel");
        }
    }

    // 更新昨日箱量
    protected void Button3_Click(object sender, EventArgs e)
    {
        for (int i = GridView4.Rows.Count - 1; i >= 0; i--)
        {
            if (((CheckBox)(GridView4.Rows[i].FindControl("cbSel4"))).Checked == true)
            {
                string strVesselID = GridView4.DataKeys[i].Values[2].ToString();


                SqlConnection conn = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["YardPlanConnectionString1"].ConnectionString);
                SqlCommand comm = new SqlCommand("[UpdateWeekContainer]", conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add(new SqlParameter("date", strDate));
                comm.Parameters.Add(new SqlParameter("vesselId", int.Parse(strVesselID)));
                conn.Open();
                comm.ExecuteNonQuery();
                comm.Dispose();
                conn.Dispose();
            }
        }

        GridView3.DataBind();
        GridView4.DataBind();
        //  updateBackReport();
    }

    //卸船In
    protected void Button4_Click(object sender, EventArgs e)
    {
        for (int i = GridView4.Rows.Count - 1; i >= 0; i--)
        {
            if (((CheckBox)(GridView4.Rows[i].FindControl("cbSel4"))).Checked == true)
            {
                string strVesselID = GridView4.DataKeys[i].Values[2].ToString();
                SqlConnection conn = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["YardPlanConnectionString1"].ConnectionString);
                SqlCommand comm = new SqlCommand("[UpdateWeekContainerIn]", conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add(new SqlParameter("date", strDate));
                comm.Parameters.Add(new SqlParameter("vesselId", int.Parse(strVesselID)));
                conn.Open();
                comm.ExecuteNonQuery();
                comm.Dispose();
                conn.Dispose();
            }
        }

        GridView3.DataBind();
        GridView4.DataBind();
    }

    //装船Out
    protected void Button5_Click(object sender, EventArgs e)
    {
        for (int i = GridView4.Rows.Count - 1; i >= 0; i--)
        {
            if (((CheckBox)(GridView4.Rows[i].FindControl("cbSel4"))).Checked == true)
            {
                string strVesselID = GridView4.DataKeys[i].Values[2].ToString();
                SqlConnection conn = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["YardPlanConnectionString1"].ConnectionString);
                SqlCommand comm = new SqlCommand("[UpdateWeekContainerOut]", conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add(new SqlParameter("date", strDate));
                comm.Parameters.Add(new SqlParameter("vesselId", int.Parse(strVesselID)));
                conn.Open();
                comm.ExecuteNonQuery();
                comm.Dispose();
                conn.Dispose();
            }
        }

        GridView3.DataBind();
        GridView4.DataBind();
    }
    /// <summary>
    /// 船期确认展示
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button6_Click(object sender, EventArgs e)
    {
        #region sql
        String mssql = string.Format(@"UPDATE [VesselInfo] 
    SET   [vi_submit] = '{0}'
    WHERE [vi_Date]='{1}' ", "Y", strDate);
        #endregion
        fnMsSetSql(mssql);
        //updateBackReport();
        Button6.Text = "船期确认已更新！";
        Button6.Enabled = false;
        lblWebTitle.Text += "已更新";
        //srs.SaveExcel();
    }
    /// <summary>
    /// 更新备用版本
    /// </summary>
    protected void updateBackReport()
    {
        SaveReportingService srs = new SaveReportingService();
        srs.SaveMhtml();
    }
}


//        #region 
//        string strSql = @"--Delete删除数据
//    DELETE FROM [YardPlan].[dbo].[Vessel] WHERE vs_date='" + strDate + @"'
//            --Insert插入数据
//            INSERT INTO [YardPlan].[dbo].[Vessel]
//           ([vs_date]           ,[vs_line]           ,[vs_customer]
//           ,[vs_vesselename]           ,[vs_vesselcname]           ,[vs_vesselcode]
//           ,[vs_ivoyage]           ,[vs_expvoyage]           ,[vs_proformaETA]
//           ,[vs_proformaETB]           ,[vs_proformaETD]           ,[vs_forecastETA]
//           ,[vs_forecastETB]           ,[vs_forecastETD]           ,[vs_finshtime]
//           ,[vs_remark]           ,[vs_longpole]           ,[vs_worknum]
//           ,[vs_vesseltime]           ,[vs_gold]           ,[vs_finsh]
//        ,vs_internalfg,vs_intrade,vs_berthLength,vs_departLength
//           )
//       SELECT '" + strDate + @"', line, cstmcd, ename,cname,
//	wp.OCRRID,ivoyage,expvoyage, PROETA,PROETB,
//	PROETD,FORETA,FORETB, FORETD,
//	'','','',gcranenum,allsum*60/
//datediff(mi,FORETB,FORETD),
//dbo.getVesselGold_fn(allsum)
//,'',internalfg,intrade,berthLength,departLength		
//From ora_WeekPlan_VW as wp left join (SELECT [ocrrid]    
//      ,sum(isnull([IF20],0)
//	+isNull([IF40],0)
//      +isNull([IF45],0)
//      +isNull([IE20],0)
//      +isNull([IE40],0)
//      +isNull([IE45],0)
//      +isNull([OF20],0)
//      +isNull([OF40],0)
//      +isNull([OF45],0)
//      +isNull([OE20],0)
//      +isNull([OE40],0)
//      +isNull([OE45],0)) allsum
//  FROM [YardPlan].[dbo].[ora_WeekContainer_VW]
//group by [ocrrid]) as wc
//on  wp.OCRRID=wc.OCRRID
//    --Delete删除数据
//          DELETE FROM [YardPlan].[dbo].[VesselContainer] WHERE vc_date='" + strDate + @"' 
//    --插入数据
//        INSERT INTO [YardPlan].[dbo].[VesselContainer]
//           ([vc_date]           ,[vc_vesselename]           ,[vc_vesselcname]
//           ,[vc_vesselcode]           ,[vc_ivoyage]           ,[vc_expvoyage]
//           ,[vc_cntrowner]           ,[vc_20IF]           ,[vc_20IE]
//           ,[vc_20OF]           ,[vc_20OE]           ,[vc_40IF]
//           ,[vc_40IE]           ,[vc_40OF]           ,[vc_40OE]
//           ,[vc_45IF]           ,[vc_45IE]           ,[vc_45OF]
//           ,[vc_45OE]           ,[vc_sum]           ,[vc_recordtime])    
//SELECT '" + strDate + @"',[ename] ,[cname]
//	,wp.[ocrrid]       ,[ivoyage]      ,[expvoyage]
//      ,[copercd]      ,isNull([IF20],0)  ,isNull([IE20],0)
//	  ,isNull([OF20],0)  ,isNull([OE20],0)      ,isNull([IF40],0)
//	  ,isNull([IE40],0), isNull([OF40],0)     ,isNull([OE40],0)
//      ,isNull([IF45],0) ,isNull([IE45],0) ,isNull([OF45],0) ,isNull([OE45],0)
//	  ,ISNULL([IF20],0)+ISNULL([IE20],0)+
//		ISNULL([OF20],0)+ISNULL([OE20],0)+
//		ISNULL([IF40],0)+ISNULL([IE40],0)+
//		ISNULL([OF40],0)+ISNULL([OE40],0)+
//        ISNULL([IF45],0)+ISNULL([IE45],0)+
//		ISNULL([OF45],0)+ISNULL([OE45],0)
//		,GETDATE()
//  FROM [YardPlan].[dbo].[ora_WeekPlan_VW] AS WP
//	LEFT JOIN [YardPlan].[dbo].[ora_WeekContainer_VW] AS WC
//  on wp.[ocrrid]=wc.[ocrrid]";
//    #region
//                string sqlUpdateContainer = @"
//    UPDATE [YardPlan].[dbo].[VesselContainer] vc
//	,[YardPlan].[dbo].[VesselContainer] bf
//   SET vc.[vc_20IF] = bf.[vc_20IF]
//      ,vc.[vc_20IE] = bf.[vc_20IE]
//      ,vc.[vc_20OF] = bf.[vc_20OF]
//      ,vc.[vc_20OE] = bf.[vc_20OE]
//      ,vc.[vc_40IF] = bf.[vc_20OE]
//      ,vc.[vc_40IE] = bf.[vc_20OE]
//      ,vc.[vc_40OF] = bf.[vc_20OE]
//      ,vc.[vc_40OE] = bf.[vc_20OE]
//      ,vc.[vc_45IF] = bf.[vc_20OE]
//      ,vc.[vc_45IE] = bf.[vc_20OE]
//      ,vc.[vc_45OF] = bf.[vc_20OE]
//      ,vc.[vc_45OE] = bf.[vc_20OE]
//      ,vc.[vc_sum] = bf.[vc_20OE]      
// WHERE dateadd(dd,-1,vc.[vc_date])=bf.[vc_date]
//		and vc.[vc_cntrowner]=bf.[vc_cntrowner]
//		and vc.vc_vesselcode='" + strVesselID + @"'
//		and vc.vc_date='" + strDate + "'";
//                #endregion
//#endregion

