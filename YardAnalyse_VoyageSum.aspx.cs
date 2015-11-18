using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class YardPlan_YardAnalyse_VoyageSum : System.Web.UI.Page
{
    #region SQL2
    static string SQL2 = @"
select v.V_OCRRID,
       V.V_LNECD,
       V.V_POT_LDUNLDPORT, 
       cast(SUM(V.GP20)*1.0/22 as dec(10,1)) AS 'GP20',
       cast(SUM(V.GP40)*1.0/22 as dec(10,1)) AS 'GP40',
       cast(SUM(V.HQ40)*1.0/22 as dec(10,1)) AS 'HQ40',
       cast(SUM(V.HQ45)*1.0/22 as dec(10,1)) AS 'HQ45',
       cast(SUM(V.RF20)*1.0/22 as dec(10,1)) AS 'RF20',
       cast(SUM(V.RF40)*1.0/22 as dec(10,1)) AS 'RF40',
       cast(SUM(V.RF45)*1.0/22 as dec(10,1)) AS 'RF45'
  from (SELECT V_OCRRID,
               V_LNECD,
               V_POT_LDUNLDPORT,
               case
                 when v1.v_type = '20GP' THEN
				  v1.v_count
                 else
                  0
               END AS 'GP20',
               case
                 WHEN v1.v_type = '40GP' THEN
                  v1.v_count
                 else
                  0
               END AS 'GP40',
               case
                 WHEN v1.v_type = '40HQ' THEN
                  v1.v_count
                 else
                  0
               END AS 'HQ40',
               case
                 WHEN v1.v_type = '45HQ' THEN
                  v1.v_count
                 else
                  0
               END AS 'HQ45',
               case
                 WHEN v1.v_type = '20RF' THEN
                  v1.v_count
                 else
                  0
               END AS 'RF20',
               case
                 WHEN v1.v_type = '40RF' THEN
                  v1.v_count
                 else
                  0
               END AS 'RF40',
               case
                 WHEN v1.v_type = '45RF' THEN
                  v1.v_count
                 else
                  0
               END AS 'RF45'
          FROM VoyageCnstSum v1) V
 where v.v_ocrrid = '{0}'
 group by V.V_OCRRID, V.V_LNECD, V.V_POT_LDUNLDPORT

";


    #endregion

    #region SQL1
    static string SQL1 = @"

select v.V_OCRRID,
       V.V_LNECD,
       V.V_POT_LDUNLDPORT,
       SUM(V.GP20) AS GP20,
       SUM(V.GP40) AS GP40,
       SUM(V.HQ40) AS HQ40,
       SUM(V.HQ45) AS HQ45,
       SUM(V.RF20) AS RF20,
       SUM(V.RF40) AS RF40,
       SUM(V.RF45) AS RF45
  from (SELECT V_OCRRID,
               V_LNECD,
               V_POT_LDUNLDPORT,
               case
                 when v1.v_type = '20GP' THEN
                  v1.v_count
                 else
                  0
               END AS 'GP20',
               case
                 WHEN v1.v_type = '40GP' THEN
                  v1.v_count
                 else
                  0
               END AS 'GP40',
               case
                 WHEN v1.v_type = '40HQ' THEN
                  v1.v_count
                 else
                  0
               END AS 'HQ40',
               case
                 WHEN v1.v_type = '45HQ' THEN
                  v1.v_count
                 else
                  0
               END AS 'HQ45',
               case
                 WHEN v1.v_type = '20RF' THEN
                  v1.v_count
                 else
                  0
               END AS 'RF20',
               case
                 WHEN v1.v_type = '40RF' THEN
                  v1.v_count
                 else
                  0
               END AS 'RF40',
               case
                 WHEN v1.v_type = '45RF' THEN
                  v1.v_count
                 else
                  0
               END AS 'RF45'
          FROM VoyageCnstSum v1) V
 where v.v_ocrrid = '{0}'
 group by V.V_OCRRID, V.V_LNECD, V.V_POT_LDUNLDPORT
 
";

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        string ocrrid =  Request.QueryString["ocrrid"];
        DataBind(gvVoyageSum, string.Format(SQL1, ocrrid));
        DataBind(gvVoyageSum2, string.Format(SQL2, ocrrid));
    }

    protected void DataBind(GridView gv, string sql)
    {
        DBHelper helper = new DBHelper();
        DataTable table = helper.ExecuteAdapter(sql);
        gv.DataSource = table;

        table.Columns.Add(GetTableColSum());
        table.Rows.Add(GetTableRowSum(table));
        gv.DataBind();
    }

    protected DataRow GetTableRowSum(DataTable table)
    {
        //增加总计行
        DataRow row = table.NewRow();
        row[0] = DBNull.Value;
        row[1] = DBNull.Value;
        row[2] = "总计";

        int i = 3;
        while (i < table.Columns.Count)
        {
            row[i] = table.Compute("Sum(" + table.Columns[i].ColumnName + ")", "true");
            i++;
        }
        return row;
    }

    protected DataColumn GetTableColSum()
    {
        //增加总计列
        string expression = "GP20+GP40+HQ40+HQ45+RF20+RF40+RF45";
        System.Type dataType = System.Type.GetType("System.Decimal");
        DataColumn col = new DataColumn("总计", dataType, expression, MappingType.Attribute);
        return col;
    }

}
