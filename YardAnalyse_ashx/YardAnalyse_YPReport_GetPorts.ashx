<%@ WebHandler Language="C#" Class="YardAnalyse_YPReport_GetPorts" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.Collections.Generic; 

public class YardAnalyse_YPReport_GetPorts : IHttpHandler {

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
           AND YRP_LDUNLDPORT2 != '') A
 WHERE A.line = '{0}'
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
            output = SetHTML(portList);
        }
        
        context.Response.Write(output);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }


    public string SetHTML(List<YA_PortData> portList)
    {
        string td_bay_20GP = "<td class='bay-20GP'>{0}</td>";
        string td_bay_40GP = "<td class='bay-40GP'>{0}</td>";
        string td_bay_40HQ = "<td class='bay-40HQ'>{0}</td>";

        string td_20GP = "<td class='20GP'>{0}</td>";
        string td_40GP = "<td class='40GP'>{0}</td>";
        string td_40HQ = "<td class='40HQ'>{0}</td>";

        string html = "";
        for (int i = 0; i < portList.Count; i++)
        {
            string lnecd=portList[i].Lnecd;
            string port = portList[i].Port;

            string s201 = "<td>&nbsp;</td>", s401 = "<td>&nbsp;</td>", s40s1 = "<td>&nbsp;</td>";
            string s20 = "<td>&nbsp;</td>", s40 = "<td>&nbsp;</td>", s40s = "<td>&nbsp;</td>";
                
            for (int j = 0; j < portList[i].CtypeCount; j++)
            {
                CTYPE ctype = portList[i].GetType(j);
                
                switch (ctype.Type)
                {
                    case "20GP":
                        s201 = string.Format(td_20GP, ctype.GetSumInfo());
                        s20 = string.Format(td_bay_20GP, ctype.GetBayInfo());
                        break;
                    case "40GP":
                        s401 = string.Format(td_40GP, ctype.GetSumInfo());
                        s40 = string.Format(td_bay_40GP, ctype.GetBayInfo());
                        break;
                    case "40HQ":
                        s40s1 = string.Format(td_40HQ, ctype.GetSumInfo());
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
    
    
    
    
    public List<CTYPE> GetCtype(string lnecd, string port) {
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
           AND YRP_LDUNLDPORT2 != '') A
 WHERE A.line = '{0}'
   AND A.port = '{1}' ", lnecd, port);        
        #endregion

        DBHelper helper = new DBHelper();
        DataTable table = helper.ExecuteAdapter(selectSQL);
        if (table.Rows.Count!=0)
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
        //获取该航线、该卸货港、该箱型的贝位需求历史值
        List<string> sumList = new List<string>();
        string getSumSQL = string.Format(@"
SELECT DISTINCT bay
  FROM (SELECT YRP_LNECD line,
               YRP_LDUNLDPORT1 port,
               YRP_CNTR_SIZE + YRP_CNTR_HEIGHT ctype,
               YRP_AREA + YRP_BAY bay
          FROM ya_rainbow_pict
         WHERE CONVERT(INT, YRP_BAY) >= YRP_SQUEUE
           AND YRP_LDUNLDPORT1 != ''
           AND YRP_CNTR_SIZE IN ('20', '40')
        UNION ALL
        SELECT YRP_LNECD line,
               YRP_LDUNLDPORT2 port,
               YRP_CNTR_SIZE + YRP_CNTR_HEIGHT ctype,
               YRP_AREA + YRP_BAY bay
          FROM ya_rainbow_pict
         WHERE CONVERT(INT, YRP_BAY) >= YRP_SQUEUE
           AND YRP_LDUNLDPORT2 != ''
           AND YRP_CNTR_SIZE IN ('20', '40')) A
 WHERE A.line = '{0}'
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
    
    
    
    public List<double> GetCnstSum(string lnecd,string port, string ctype) {
        //获取该航线、该卸货港、该箱型的贝位需求历史值
        List<double> sumList = new List<double>();
        string getSumSQL = string.Format(@"
select top 4 /*v_lnecd lnecd,
                    v_pot_ldunldport port,       
                    v_count count,
       CONVERT(varchar(10), v_recordtime, 120) recordtime,*/ 
	cast(SUM(v_count) * 1.0 / 22 as dec(10, 1)) sum 
  from ya_voyagecnstsum
 where v_lnecd = '{0}'
   and v_pot_ldunldport = '{1}'
   and v_type in ('{2}')
 group by v_lnecd, v_pot_ldunldport, v_type, v_count, v_recordtime
 order by v_type, v_recordtime desc ", lnecd, port, ctype);

        DBHelper helper = new DBHelper();
        DataTable table = helper.ExecuteAdapter(getSumSQL);
        if (table.Rows.Count != 0)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                sumList.Add(Convert.ToDouble(table.Rows[i]["sum"]));
            }
        }
        return sumList;
    }
}