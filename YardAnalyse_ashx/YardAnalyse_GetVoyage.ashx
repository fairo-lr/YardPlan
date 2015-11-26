<%@ WebHandler Language="C#" Class="YardAnalyse_GetVoyage" %>

using System;
using System.Web;
using System.Data;

public class YardAnalyse_GetVoyage : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        string selectSQL = "";
        string lnecd = context.Request["lnecd"].ToString();        
        #region sql
        if (lnecd=="")
        {
             selectSQL = string.Format(@"
 select distinct case
                  when ysc_relate_line is not null then
                   ysc.ysc_yrp_lnecd + '-' + ysc.ysc_relate_line
                  else
                   vcs.v_lnecd
                end as lnecd1,
                case
                  when ysc_relate_line is not null then
                   ysc.ysc_yrp_lnecd + '-' + ysc.ysc_relate_line
                  else
                   vcs.v_lnecd
                end as lnecd2
  from ya_VoyageCnstSum vcs
  left join ya_shipline_color ysc on (ysc.ysc_relate_line = vcs.v_lnecd or
                                     ysc.ysc_yrp_lnecd = vcs.v_lnecd)
 order by lnecd1");
        }
        else
        {
            selectSQL = string.Format(@"
 SELECT distinct V_OCRRID,
                vs_vesselcname + ' ' + vs.vs_ivoyage + ' / ' +
                vs.vs_expvoyage,
                vcs.v_recordtime
  FROM (select distinct case
                          when ysc_relate_line is not null then
                           ysc.ysc_yrp_lnecd + '-' + ysc.ysc_relate_line
                          else
                           vcs.v_lnecd
                        end as lnecd,
                        vcs.v_ocrrid,
                        vcs.v_recordtime
          from ya_VoyageCnstSum vcs
          left join ya_shipline_color ysc on (ysc.ysc_relate_line =
                                             vcs.v_lnecd or ysc.ysc_yrp_lnecd =
                                             vcs.v_lnecd)) vcs,
       vessel vs
 where vcs.v_ocrrid = vs.vs_vesselcode
   and vcs.lnecd = '{0}'
   and vs_finsh is not null
 order by vcs.v_recordtime desc ", lnecd);
        }

        #endregion

        DBHelper helper = new DBHelper();
        DataTable table = helper.ExecuteAdapter(selectSQL);

        string options = "<option value=''></option>";

        if (table.Rows.Count != 0)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                options += "<option value='" + table.Rows[i][0].ToString() + "'>" + table.Rows[i][1].ToString() + "</option>";
            }
        }

        context.Response.Write(options);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}