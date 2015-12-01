<%@ WebHandler Language="C#" Class="YardAnalyse_YPReport_GetPorts" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.Collections.Generic;

public class YardAnalyse_YPReport_GetPorts : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string lnecd = context.Request["lnecd"].ToString();

        string selectSQL = string.Format(@"
SELECT DISTINCT port
  FROM (SELECT YRP_LNECD line, YRP_LDUNLDPORT1 port
          FROM ya_rainbow_pict
         WHERE CONVERT(INT, YRP_BAY) >= YRP_SQUEUE
           AND YRP_LDUNLDPORT1 != ''
        UNION ALL
        SELECT YRP_LNECD, YRP_LDUNLDPORT2
          FROM ya_rainbow_pict
         WHERE CONVERT(INT, YRP_BAY) >= YRP_SQUEUE
           AND YRP_LDUNLDPORT2 != '') A,
       (select case
                 when ysc.ysc_relate_line is not null then
                  ysc.ysc_yrp_lnecd + '-' + ysc.ysc_relate_line
                 else
                  ysc.ysc_yrp_lnecd
               end as lnecd,
               ysc.ysc_yrp_lnecd value
          from ya_shipline_color ysc) B
 WHERE A.line = B.value
   AND B.lnecd = '{0}'
 ", lnecd);

        DBHelper helper = new DBHelper();
        DataTable table = helper.ExecuteAdapter(selectSQL);
        string output = "";
        if (table.Rows.Count != 0)
        {
            YA_PortData tempPort = new YA_PortData();
            List<YA_PortData> portList = new List<YA_PortData>();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                tempPort = new YA_PortData(lnecd, table.Rows[i]["port"].ToString());
                //获取箱型 
                tempPort.Ctype(GetCtype(lnecd, tempPort.Port));
                portList.Add(tempPort);
            }
            output = SetHTML2(portList);
        }

        context.Response.Write(output);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public string SetHTML2(List<YA_PortData> portList)
    {
        string html = "";
        string tr_model = @"<tr style='background-color:#e0e0e0;'><td rowspan='2' class='port'>{0}</td>
<td>{1}</td><td class='total'>{2}</td><td>{3}</td><td class='total'>{4}</td><td>{5}</td><td class='total'>{6}</td>
</tr><tr><td>{7}</td><td class='total'>{8}</td><td>{9}</td><td class='total'>{10}</td><td>{11}</td><td class='total'>{12}</td></tr>";        
        
        for (int i = 0; i < portList.Count; i++)
        {
            string port = portList[i].Port;

            string GP20_sum = "&nbsp;", GP20_sum_total = "&nbsp;", GP20_bay = "&nbsp;", GP20_bay_total = "&nbsp;", GP40_sum = "&nbsp;", GP40_sum_total = "&nbsp;"
                , GP40_bay = "&nbsp;", GP40_bay_total = "&nbsp;", HQ40_sum = "&nbsp;", HQ40_sum_total = "&nbsp;", HQ40_bay = "&nbsp;", HQ40_bay_total = "&nbsp;";

            for (int j = 0; j < portList[i].CtypeCount; j++)
            {
                CTYPE ctype = portList[i].GetType(j);

                switch (ctype.Type)
                {
                    case "20GP":
                        GP20_sum = ctype.GetSumInfo();
                        GP20_bay = ctype.GetBayInfo();
                        GP20_sum_total = ctype.GetSumTotal();
                        GP20_bay_total = ctype.GetBayTotal();
                        break;
                    case "40GP":
                        GP40_sum = ctype.GetSumInfo();
                        GP40_bay = ctype.GetBayInfo();
                        GP40_sum_total = ctype.GetSumTotal();
                        GP40_bay_total = ctype.GetBayTotal();
                        break;
                    case "40HQ":
                        HQ40_sum = ctype.GetSumInfo();
                        HQ40_bay = ctype.GetBayInfo();
                        HQ40_sum_total = ctype.GetSumTotal();
                        HQ40_bay_total = ctype.GetBayTotal();
                        break;
                }
            }
            html += string.Format(tr_model, port, GP20_sum, GP20_sum_total, GP40_sum, GP40_sum_total, HQ40_sum, HQ40_sum_total, GP20_bay, GP20_bay_total
                , GP40_bay, GP40_bay_total, HQ40_bay, HQ40_bay_total);
        }
        return html;
    }

    #region old code
    public string SetHTML(List<YA_PortData> portList)
    {
        string td_bay_20GP = "<td class='bay-20GP'>{0}</td>";
        string td_bay_40GP = "<td class='bay-40GP'>{0}</td>";
        string td_bay_40HQ = "<td class='bay-40HQ'>{0}</td>";

        string td_sum_20GP = "<td class='20GP'>{0}</td>";
        string td_sum_40GP = "<td class='40GP'>{0}</td>";
        string td_sum_40HQ = "<td class='40HQ'>{0}</td>";

        string html = "";
        for (int i = 0; i < portList.Count; i++)
        {
            string lnecd = portList[i].Lnecd;
            string port = portList[i].Port;

            string s201 = "<td>&nbsp;</td>", s401 = "<td>&nbsp;</td>", s40s1 = "<td>&nbsp;</td>";
            string s20 = "<td>&nbsp;</td>", s40 = "<td>&nbsp;</td>", s40s = "<td>&nbsp;</td>";

            for (int j = 0; j < portList[i].CtypeCount; j++)
            {
                CTYPE ctype = portList[i].GetType(j);

                switch (ctype.Type)
                {
                    case "20GP":
                        s201 = string.Format(td_sum_20GP, ctype.GetSumInfo());
                        s20 = string.Format(td_bay_20GP, ctype.GetBayInfo());
                        break;
                    case "40GP":
                        s401 = string.Format(td_sum_40GP, ctype.GetSumInfo());
                        s40 = string.Format(td_bay_40GP, ctype.GetBayInfo());
                        break;
                    case "40HQ":
                        s40s1 = string.Format(td_sum_40HQ, ctype.GetSumInfo());
                        s40s = string.Format(td_bay_40HQ, ctype.GetBayInfo());
                        break;
                }
            }
            string tr1 = "<tr style='background-color:#e0e0e0;'>" + string.Format("<td rowspan='2' class='port'>{0}</td>", port) + s201 + s401 + s40s1 + "</tr>";
            string tr2 = "<tr>" + s20 + s40 + s40s + "</tr>";
            html += tr1 + tr2;
        }
        return html;
    }
    #endregion
    
    
    public List<CTYPE> GetCtype(string lnecd, string port)
    {
        List<CTYPE> ctypeList = new List<CTYPE>();
        #region SQL
        string selectSQL = string.Format(@"
SELECT DISTINCT YRP_CNTR_SIZE + YRP_CNTR_HEIGHT ctype
  FROM (SELECT YRP_LNECD line,
               YRP_LDUNLDPORT1 port,
               YRP_CNTR_SIZE,
               YRP_CNTR_HEIGHT
          FROM ya_rainbow_pict
         WHERE CONVERT(INT, YRP_BAY) >= YRP_SQUEUE
           AND YRP_LDUNLDPORT1 != ''
        UNION ALL
        SELECT YRP_LNECD, YRP_LDUNLDPORT2, YRP_CNTR_SIZE, YRP_CNTR_HEIGHT
          FROM ya_rainbow_pict
         WHERE CONVERT(INT, YRP_BAY) >= YRP_SQUEUE
           AND YRP_LDUNLDPORT2 != '') A,
       (select case
                 when ysc.ysc_relate_line is not null then
                  ysc.ysc_yrp_lnecd + '-' + ysc.ysc_relate_line
                 else
                  ysc.ysc_yrp_lnecd
               end as lnecd,
               ysc.ysc_yrp_lnecd value
          from ya_shipline_color ysc) B
 WHERE A.line = B.value
   AND B.lnecd = '{0}'
   AND A.port = '{1}' ", lnecd, port);
        #endregion

        DBHelper helper = new DBHelper();
        DataTable table = helper.ExecuteAdapter(selectSQL);
        if (table.Rows.Count != 0)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                CTYPE tempType = new CTYPE(table.Rows[i]["ctype"].ToString());

                tempType.BayList(GetBaylist(lnecd, port, tempType.Type));
                tempType.CntrSum(GetCnstSum(lnecd, port, tempType.Type));

                ctypeList.Add(tempType);
            }
        }
        return ctypeList;
    } 
    
    public List<string> GetBaylist(string lnecd, string port, string ctype)
    {
        //获取该航线、该卸货港、该箱型的计划贝位
        List<string> sumList = new List<string>();
        string getSumSQL = string.Format(@"
SELECT DISTINCT bay
  FROM (SELECT YRP_LNECD line,
               YRP_LDUNLDPORT1 port,
               CASE 
			WHEN YRP_CNTR_HEIGHT != 'G/H'
				THEN YRP_CNTR_SIZE + YRP_CNTR_HEIGHT
			ELSE YRP_CNTR_SIZE + 'HQ'
			END AS ctype,
               YRP_AREA + YRP_BAY bay
          FROM ya_rainbow_pict
         WHERE CONVERT(INT, YRP_BAY) >= YRP_SQUEUE
           AND YRP_LDUNLDPORT1 != ''
           AND YRP_CNTR_SIZE IN ('20', '40')
        UNION ALL
        SELECT YRP_LNECD line,
               YRP_LDUNLDPORT2 port,
               CASE 
			WHEN YRP_CNTR_HEIGHT != 'G/H'
				THEN YRP_CNTR_SIZE + YRP_CNTR_HEIGHT
			ELSE YRP_CNTR_SIZE + 'HQ'
			END AS ctype,
               YRP_AREA + YRP_BAY bay
          FROM ya_rainbow_pict
         WHERE CONVERT(INT, YRP_BAY) >= YRP_SQUEUE
           AND YRP_LDUNLDPORT2 != ''
           AND YRP_CNTR_SIZE IN ('20', '40')) A,
       (select case
                 when ysc.ysc_relate_line is not null then
                  ysc.ysc_yrp_lnecd + '-' + ysc.ysc_relate_line
                 else
                  ysc.ysc_yrp_lnecd
               end as lnecd,
               ysc.ysc_yrp_lnecd value
          from ya_shipline_color ysc) B
 WHERE A.line = B.value
   AND B.lnecd = '{0}'
   AND A.port = '{1}'
   AND A.ctype = '{2}'
 ORDER BY bay ", lnecd, port, ctype);

        DBHelper helper = new DBHelper();
        DataTable table = helper.ExecuteAdapter(getSumSQL);
        if (table.Rows.Count != 0)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                sumList.Add(table.Rows[i]["bay"].ToString());
            }
        }
        return sumList;
    }



    public List<SUMTYPE> GetCnstSum(string lnecd, string port, string ctype)
    {
        //获取该航线、该卸货港、该箱型的贝位需求历史值
        List<SUMTYPE> sumList = new List<SUMTYPE>();
        string getSumSQL = string.Format(@"
SELECT CONVERT(VARCHAR(5), cast(dates.DATE AS DATETIME), 1) recordtime
	,isnull(cast(SUM(a.v_count) * 1.0 / 22 AS DEC(10, 1)), 0) sum
FROM (
	SELECT DISTINCT TOP 4 CONVERT(VARCHAR(10), v_recordtime, 120) AS DATE
	FROM ya_VoyageCnstSum vcs
	LEFT JOIN ya_shipline_color ysc ON (
			ysc.ysc_relate_line = vcs.v_lnecd
			OR ysc.ysc_yrp_lnecd = vcs.v_lnecd
			)
	WHERE CASE 
			WHEN ysc_relate_line IS NOT NULL
				THEN ysc.ysc_yrp_lnecd + '-' + ysc.ysc_relate_line
			ELSE vcs.v_lnecd
			END = '{0}'
	ORDER BY DATE DESC
	) dates
LEFT JOIN (
	SELECT CASE 
			WHEN ysc_relate_line IS NOT NULL
				THEN ysc.ysc_yrp_lnecd + '-' + ysc.ysc_relate_line
			ELSE vcs.v_lnecd
			END AS lnecd
		,vcs.V_OCRRID
		,vcs.V_LNECD
		,vcs.V_POT_LDUNLDPORT port
		,vcs.V_TYPE
		,vcs.V_COUNT
		,CONVERT(VARCHAR(10), v_recordtime, 120) DATE
	FROM ya_VoyageCnstSum vcs
	LEFT JOIN ya_shipline_color ysc ON (
			ysc.ysc_relate_line = vcs.v_lnecd
			OR ysc.ysc_yrp_lnecd = vcs.v_lnecd
			)
	WHERE CASE 
			WHEN ysc_relate_line IS NOT NULL
				THEN ysc.ysc_yrp_lnecd + '-' + ysc.ysc_relate_line
			ELSE vcs.v_lnecd
			END = '{0}'
		AND V_POT_LDUNLDPORT = '{1}'
		AND v_type = '{2}'
	) a ON a.DATE = dates.DATE
GROUP BY CONVERT(VARCHAR(5), cast(dates.DATE AS DATETIME), 1)
ORDER BY recordtime ", lnecd, port, ctype);

        DBHelper helper = new DBHelper();
        DataTable table = helper.ExecuteAdapter(getSumSQL);
        if (table.Rows.Count != 0)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                SUMTYPE tempSum = new SUMTYPE(Convert.ToDouble(table.Rows[i]["sum"]), table.Rows[i]["recordtime"].ToString());
                sumList.Add(tempSum);
            }
        }
        return sumList;
    }
}