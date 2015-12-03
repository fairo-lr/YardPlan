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
    public List<SUMTYPE> cntrSum = new List<SUMTYPE>();//需求贝数

    #endregion
    
    #region 方法

    public string Type
    {
        get { return m_type; }
    }

   
    public void InsertbayList(string bay)
    {
        bayList.Add(bay);
    }   

    public void CntrSum(List<SUMTYPE> sumList)
    {
        cntrSum.Clear();
        cntrSum = sumList;
    }

    public void BayList(List<string> bays)
    {
        bayList.Clear();
        bayList = bays;
    }

    public string GetBayInfo()
    {
        string str = "";
        if (bayList.Count != 0)
        {
            for (int i = 0; i < bayList.Count; i++)
            {
                if (i == 0) { str += "<span class='span-title'>当前贝位</span><br/>"; }
                str += string.Format("<span>{0}</span>", bayList[i].ToString());
                if ((i + 1) % 5 == 0) { str += "<br/>"; }
            }
        }
        return str == "" ? "&nbsp;" : str;
    }

    public string GetSumInfo()
    {
        string str = "";
        if (cntrSum.Count != 0)
        { 
            for (int i = 0; i < cntrSum.Count; i++)
            {
                if (i == 0) { str += "<span class='span-title'>需求贝数<br/>记录日期</span>"; }
                str += string.Format("<span>{0}<br/>{1}</span>", cntrSum[i].Sum.ToString(), cntrSum[i].Date);
                if ((i + 1) % 5 == 0) { str += "<br/>"; }
            }
        } 
        return str == "" ? "&nbsp;" : str;
    }


    public string GetSumTotal()
    {
        return string.Format("<div>{0}</div>", SumTotal);
    }

    public string GetBayTotal()
    {
        if (bayList.Count != 0)
            return Convert.ToDouble(bayList.Count) >= SumTotal ?
            string.Format("<div style='color:green;'>{0}<div>", bayList.Count) : string.Format("<div style='color:red;'>{0}<div>", bayList.Count);
        else
            return "<div>0<div>";
    }


    /// <summary>
    /// 计算需求贝数平均值
    /// </summary>
    public double SumTotal
    {
        get
        {
            double total = 0.0;
            for (int i = 0; i < cntrSum.Count; i++)
            {
                total += cntrSum[i].Sum;
            }
            return Math.Round(total / cntrSum.Count, 2);
        }
    } 
    #endregion
}

public class SUMTYPE
{
    /// <summary>
    /// 历史堆存记录
    /// </summary>
    /// <param name="sum">堆存量</param>
    /// <param name="date">记录日期</param>
    public SUMTYPE(double sum, string date)
    {
        m_sum = sum;
        m_date = date;
    }
    
    #region 属性
    private double m_sum;
    private string m_date;
    #endregion

    #region 方法
    public double Sum
    {
        get { return m_sum; }
    }
    public string Date
    {
        get { return m_date; }
    }
    #endregion
}


