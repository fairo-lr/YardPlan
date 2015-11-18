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
using System.Data.OracleClient;

public partial class stack_plan_jn : System.Web.UI.Page
{
    string m_oraConnstr = "Data Source=xsctture;Persist Security Info=True;User ID=xsctwebusr;Password=xsctwebusr;Unicode=True";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BingDDLVessl();
            //BindStackview();
        }
    }

    protected void BingDDLVessl()
    {
        #region sql

        string t_sql = @"select voc.voc_ocrrid vocid,
      voc.voc_vls_vnamecd || ' / ' || voc.voc_ivoyage || ' / ' || voc.voc_expvoyage vname
  from ps_vessel_occurences     voc,
       ps_vessel_berthes        vbt,
       ps_vessels               vls,
       ps_vessel_import_exports vie,
       ps_service_lines         sln
 where voc.voc_ocrrid = vbt.vbt_voc_ocrrid
      --and voc.voc_ocrrid = '123971'
   and voc.voc_vls_vnamecd = vls.vls_vnamecd
   and voc.voc_ocrrid = vie.vie_vbt_voc_ocrrid
   and vie.vie_iefg = 'I'
   and vie.vie_sln_slineid = sln.sln_slineid
   and vie.vie_intrade is null
   and voc.voc_vls_vnamecd NOT IN
       ('FANG Z 3', 'ZE YUAN', 'X YUAN19', 'FANG Z19', 'SHUN Y15')
   and vbt.vbt_pbthtm > sysdate - 15
   and vbt.vbt_pdpttm < sysdate + 15
-- and (vbt.vbt_adpttm is null or vbt.vbt_adpttm > sysdate -15)
 order by voc.voc_vls_vnamecd
";
        #endregion

        this.sdsVsl.SelectCommand = t_sql;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string t_vocid = this.ddlVsl.SelectedValue;
        string t_type = this.ddlstackType.SelectedValue;
        BindStackview(t_vocid,t_type);
    }

    protected bool IsArchive(string p_vocid,string p_stackType)
    {
        #region sql
        string t_sql = @"select * from ps_stack_plans spl where spl.spl_voc_ocrrid='"+p_vocid+@"' and spl.spl_type='"+p_stackType+@"'";
        #endregion
        bool t_result = false;
        using (OracleConnection t_conn = new OracleConnection(m_oraConnstr))
        {
            t_conn.Open();
            OracleCommand t_comm = new OracleCommand(t_sql, t_conn);
            OracleDataReader t_reader = t_comm.ExecuteReader();
            
            t_result = t_reader.HasRows;
        }

        return t_result;
    }

    protected void BindStackview(string p_vocid,string p_type)
    {
        #region sql
        string t_sql = @"SELECT ulp.ulp_pot_portcd 卸货港,
       csz.csz_csizenm 尺寸,
       ctp.ctp_commcode 箱型,
       nvl(hei.hei_cheightdsp, '8''6""') 高度,
       sts.sts_cdchange 状态,
       rv.rv_meaning 特殊标志,
       spl.spl_dnggfg 危品,
       spl.spl_ovlmtfg 超限,
       spl.Spl_Rfcfg 冻柜,
       spl.spl_camt 箱量,
       spl.spl_yplanid planid
  FROM PS_STACK_PLANS       SPL,
       PS_CONTAINER_SIZES   CSZ,
       PS_CONTAINER_HEIGHTS HEI,
       PS_CONTAINER_TYPES   CTP,
       ps_container_status  sts,
       ps_ref_codes         rv,
       PS_UNLOADING_PORTS   ulp
 WHERE SPL.SPL_VOC_OCRRID = '" + p_vocid + @"'
   AND SPL.SPL_TYPE = '" + p_type + @"'
   AND SPL.SPL_CSZ_CSIZECD = CSZ.CSZ_CSIZECD
   AND SPL.SPL_HEI_CHEIGHTCD = HEI.HEI_CHEIGHTCD(+)
   AND SPL.SPL_CTP_ISOCD = CTP.CTP_ISOCD
   and spl.spl_sts_cstatusid = sts.sts_cstatusid
   and rv.rv_domain(+) = '运输方式'
   and rv.rv_low_value(+) = spl.spl_regioncd
   and spl.spl_ulp_portid = ulp.ulp_portid(+)
   and ulp.ulp_voc_ocrrid(+) = '" + p_vocid + @"'
union
SELECT ulp.ulp_pot_portcd 卸货港,
       csz.csz_csizenm 尺寸,
       ctp.ctp_commcode 箱型,
       nvl(hei.hei_cheightdsp, '8''6""') 高度,
       sts.sts_cdchange 状态,
       rv.rv_meaning 特殊标志,
       sph.sph_dnggfg 危品,
       sph.sph_ovlmtfg 超限,
       sph.sph_Rfcfg 冻柜,
       sph.sph_camt 箱量,
       sph.sph_yplanid planid
  FROM ps_stack_plans_h     sph,
       PS_CONTAINER_SIZES   CSZ,
       PS_CONTAINER_HEIGHTS HEI,
       PS_CONTAINER_TYPES   CTP,
       ps_container_status  sts,
       ps_ref_codes         rv,
       PS_UNLOADING_PORTS   ulp
 WHERE sph.sph_VOC_OCRRID = '" + p_vocid + @"'
   AND sph.sph_TYPE = '" + p_type + @"'
   AND sph.sph_CSZ_CSIZECD = CSZ.CSZ_CSIZECD
   AND sph.sph_HEI_CHEIGHTCD = HEI.HEI_CHEIGHTCD(+)
   AND sph.sph_CTP_ISOCD = CTP.CTP_ISOCD
   and sph.sph_sts_cstatusid = sts.sts_cstatusid
   and rv.rv_domain(+) = '运输方式'
   and rv.rv_low_value(+) = sph.sph_regioncd
   and sph.sph_ulp_portid = ulp.ulp_portid(+)
   and ulp.ulp_voc_ocrrid(+) = '" + p_vocid + "'";
        #endregion
        this.SqlDataSource1.SelectCommand = t_sql;
    }
    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[10].Attributes.Add("class", "hidden");
        if (e.Row.RowType == DataControlRowType.DataRow) 
        {
            Image t_image = new Image();
            t_image.ImageUrl="images/003_39.png";
            e.Row.Cells[11].Controls.Add(t_image);
        }
    }
}
