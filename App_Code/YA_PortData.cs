using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Data;

/// <summary>
///YA_PortData：YardAnalyse_YardPlanReport的卸货港信息数据类 
/// </summary>
public class YA_PortData
{
    public YA_PortData()
    {
        //构造函数1
    }
    public YA_PortData(string lnecd,string port)
	{
        m_lnecd = lnecd;
        m_port = port;
    }

    #region 属性
    private string m_lnecd;
    private string m_port;
    public List<CTYPE>  typeList=new List<CTYPE>();  
    //public List<double> cntrSum=new List<double>();
    #endregion

    #region 方法

    public string Lnecd { get { return m_lnecd; } }
    public string Port { get { return m_port; } }
    public int CtypeCount { get { return typeList.Count; } }

    public void ClearTypeList()
    {
        typeList.Clear();
    }

    public void InsertTypeList(CTYPE type)
    {
        typeList.Add(type);
    }

    public CTYPE GetType(int index)
    {
        if (index >= 0 && index < typeList.Count)
            return typeList[index];
        else
            return null;
    }

    public void Ctype(List<CTYPE> types)
    {
        typeList.Clear();
        typeList = types;
    }

    #endregion
} 
 
public class CTYPE 
{
    public CTYPE() { }
    public CTYPE(string type)
    {
        m_type = type;
    }

    #region 属性
    private string m_type;
    public List<string> bayList = new List<string>(); //BAYLIST
    public List<double> cntrSum = new List<double>();//需求贝数

    #endregion

    
    #region 方法

    public string Type { get { return m_type; } }

    public void ClearbayList()
    {
        bayList.Clear();
    }

    public void InsertbayList(string bay)
    {
        bayList.Add(bay);
    }

    public void ClearcntrSum()
    {
        cntrSum.Clear();
    }

    public void InsertCntrSum(double sum)
    {
        cntrSum.Add(sum);
    }

    public void CntrSum(List<double> sum)
    {
        cntrSum.Clear();
        cntrSum = sum;
    }

    public void BayList(List<string> bays)
    {
        bayList.Clear();
        bayList = bays;
    }

    public string GetBayInfo()
    {
        string info = "";

        if (bayList.Count != 0)
        {
            info="<div>" + bayList.Count + "</div>";
            for (int i = 0; i < bayList.Count; i++)
            {
                info += "<span>" + bayList[i].ToString() + "</span>";
                if ((i + 1) % 5 == 0) { info += "<br/>"; }
            }
        }

        return info == "" ? "&nbsp;" : info;
    }

    public string GetSumInfo()
    {
        string info = "";
        if (cntrSum.Count != 0)
        {
            info = "<div>" + SumTotal + "</div>";
            for (int i = 0; i < cntrSum.Count; i++)
            {
                info += "<span>" + cntrSum[i].ToString() + "</span>";
                if ((i + 1) % 5 == 0) { info += "<br/>"; }
            }                
        }
        
        return info==""?"&nbsp;":info;
    }

    public double SumTotal
    {
        get
        {
            double total = 0.0;
            for (int i = 0; i < cntrSum.Count; i++)
            {
                total += cntrSum[i];
            }
            return Math.Round(total / cntrSum.Count,2);
        }
    }

    #endregion
    
     

}

