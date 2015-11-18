<%@ WebHandler Language="C#" Class="GetLstVslHandler" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Configuration;
using Newtonsoft.Json;
using System.Data.SqlClient;
public class GetLstVslHandler : IHttpHandler {
    string m_sqlConn = ConfigurationManager.ConnectionStrings["YardPlanConnectionString1"].ConnectionString;
    
    public void ProcessRequest (HttpContext context) {
        string t_Strconn = ConfigurationManager.ConnectionStrings["oraConnectionString"].ConnectionString;
        string t_date = context.Request["date"] + "";
        #region
        string t_sql = @"select voc.voc_ivoyage || ' / ' || voc.voc_expvoyage voyage,
       vls.vls_vchnnm,
       vls.vls_vnamecd,
       vls.vls_vlength 船长,
       nvl(vbt.vbt_astartpst, vbt.vbt_pstartpst) 起始起码,
       nvl(vbt.vbt_aendpst, vbt.vbt_pendpst) 终止尺码,
       nvl(vbt.vbt_abthdirc, vbt.vbt_pbthdirc) 靠泊方向,
       nvl(vbt.vbt_abthtm, vbt.vbt_pbthtm) 靠泊时间,
       vie.vie_intrade 内贸,
       sln.sln_vcm_cstmcd || '-' || sln.sln_lne_rtcd 航线,
       case
         when round((nvl(vbt.vbt_abthtm, vbt.vbt_pbthtm) -
                    sysdate) * 24,
                    0) < 0 then
          0
         else
          round((nvl(vbt.vbt_abthtm, vbt.vbt_pbthtm) -
                sysdate) * 24,
                0)
       end 时间,
       pot.prepot,
       pot.nxtpot,
       VLS.VLS_BKSTBAY,
       VLS.VLS_BKEDBAY,
       voc.voc_ocrrid       
                         
  from ps_vessel_berthes        vbt,
       ps_vessel_occurences     voc,
       ps_vessels               vls,
       ps_vessel_import_exports vie,
       ps_service_lines         sln,
       (
         SELECT pot1.pot_cty_countrycd||pot1.pot_portcd prepot,
          pot2.pot_cty_countrycd||pot2.pot_portcd nxtpot,
          sln.sln_lne_rtcd,sln.sln_slineid FROM
          (
            select 
                case when LAG(scl.scl_pot_portcd) over(partition by scl.scl_sch_sln_slineid order by scl.scl_portseq) is null then
                  first_value(scl.scl_pot_portcd) over(partition by scl.scl_sch_sln_slineid order by scl.scl_portseq desc)
                  else
                    LAG(scl.scl_pot_portcd) over(partition by scl.scl_sch_sln_slineid order by scl.scl_portseq) end  preprt,
                case when LEAD(scl.scl_pot_portcd) over(partition by scl.scl_sch_sln_slineid order by scl.scl_portseq) is null then
                  first_value(scl.scl_pot_portcd) over(partition by scl.scl_sch_sln_slineid order by scl.scl_portseq)
                  else
                    LEAD(scl.scl_pot_portcd) over(partition by scl.scl_sch_sln_slineid order by scl.scl_portseq) end nextprt,
                 scl.*
            from PS_SSHIPPING_BERTH_PORTS scl
           --and scl.scl_sch_sln_slineid = '9996'
          ) scl,
          ps_service_lines sln,
          ps_ports pot1,
          ps_ports pot2
          where scl.scl_pot_portcd = 'XIA'
          and sln.sln_slineid = scl.scl_sch_sln_slineid
          and scl.preprt = pot1.pot_portcd
          and scl.nextprt = pot2.pot_portcd
       ) pot
 where vbt.vbt_pbthtm between sysdate - 1 and sysdate + 7
   and vbt.vbt_voc_ocrrid = voc.voc_ocrrid
   and vls.vls_vnamecd = voc.voc_vls_vnamecd
   and voc.voc_ocrrid = vie.vie_vbt_voc_ocrrid
   and vie.vie_iefg = 'I'
   and sln.sln_slineid = vie.vie_sln_slineid
   --and vbt.vbt_adpttm is null
   and pot.sln_slineid =sln.sln_slineid
 order by round((nvl(vbt.vbt_abthtm, vbt.vbt_pbthtm) -
                to_date(to_char(sysdate, 'yyyy-mm-dd') || ' 00:00:00',
                         'yyyy-mm-dd hh24:mi:ss')) * 24,
                0)";
#endregion
        if (!string.IsNullOrEmpty(t_date)){
            t_sql = t_sql.Replace("sysdate", "to_date('" + t_date + "','yyyy-mm-dd')");
        }
        List<VslModel> t_lstVM = new List<VslModel>();
        using(OracleConnection t_conn = new OracleConnection(t_Strconn))
        {
            t_conn.Open();
            OracleCommand t_comm = new OracleCommand(t_sql, t_conn);
            OracleDataReader t_reader = t_comm.ExecuteReader();
            while(t_reader.Read())
            {
                VslModel t_vm = new VslModel();
                t_vm.Voyage = t_reader.GetString(0);
                t_vm.VCHname = t_reader.GetString(1);
                t_vm.Vname = t_reader.GetString(2);
                t_vm.Vlength = t_reader.IsDBNull(3)?"?":t_reader.GetValue(3).ToString();
                t_vm.StPost = t_reader.IsDBNull(4)?"?":t_reader.GetValue(4).ToString();
                t_vm.EdPost = t_reader.IsDBNull(5) ? "?" : t_reader.GetValue(5).ToString();
                t_vm.BreDire = t_reader.IsDBNull(6) ? "?" : (t_reader.GetString(6) == "R" ? "R" : "L");
                //t_vm.BreDire = t_reader.IsDBNull(6) ? "?" : t_reader.GetString(6) ;
                t_vm.BreTm = t_reader.IsDBNull(7) ? "?" : t_reader.GetDateTime(7).ToString("yyyy-MM-dd HH:mm:ss");
                t_vm.Intrade = t_reader.IsDBNull(8) ? "N" : t_reader.GetString(8);
                t_vm.SlnName = t_reader.GetString(9);
                t_vm.DayOrder = t_reader.GetValue(10).ToString();
                t_vm.PrePort = t_reader.GetString(11);
                t_vm.NextPort = t_reader.GetString(12);
                t_vm.Bkstpost = t_reader.IsDBNull(13) ? "0" : t_reader.GetValue(13).ToString();
                t_vm.Bkedpost = t_reader.IsDBNull(14) ? "0" : t_reader.GetValue(14).ToString();
                t_vm.IsFinish = getIsFinish(t_reader.GetValue(15).ToString(), (string.IsNullOrEmpty(t_date) ? DateTime.Now.ToString("yyyy-MM-dd") : t_date));
                t_lstVM.Add(t_vm);
            }
            t_reader.Close();
        }
        string t_result = JsonConvert.SerializeObject(t_lstVM);
        context.Response.Write(t_result);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
    
    public string getIsFinish(string p_vocid, string p_date)
    {
        string t_sql = @"select vs_finsh from Vessel
where vs_vesselcode= '" +p_vocid+@"'
and vs_date = '"+p_date+"'";
        string t_result = "N";
        using(SqlConnection t_conn = new SqlConnection(m_sqlConn))
        {
            t_conn.Open();
            SqlCommand t_comm = new SqlCommand(t_sql, t_conn);
            SqlDataReader t_reader = t_comm.ExecuteReader();
            if (t_reader.HasRows){
                t_reader.Read();
                t_result = t_reader.IsDBNull(0) ?"N": t_reader.GetValue(0).ToString();
            }
            t_reader.Close();
            t_conn.Close();
        }

        t_result = string.IsNullOrEmpty(t_result) ? "N" : t_result;
        return t_result;
    }

}