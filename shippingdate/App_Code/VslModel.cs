using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// VslModel 的摘要说明
/// </summary>
public class VslModel
{
	public VslModel()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    private string m_voyage;

    public string Voyage
    {
        get { return m_voyage; }
        set { m_voyage = value; }
    }
    private string m_vCHname;

    public string VCHname
    {
        get { return m_vCHname; }
        set { m_vCHname = value; }
    }
    private string m_vname;

    public string Vname
    {
        get { return m_vname; }
        set { m_vname = value; }
    }
    private string m_vlength;

    public string Vlength
    {
        get { return m_vlength; }
        set { m_vlength = value; }
    }
    private string m_stPost;

    public string StPost
    {
        get { return m_stPost; }
        set { m_stPost = value; }
    }
    private string m_edPost;

    public string EdPost
    {
        get { return m_edPost; }
        set { m_edPost = value; }
    }
    private string m_breDire;

    public string BreDire
    {
        get { return m_breDire; }
        set { m_breDire = value; }
    }
    private string m_breTm;

    public string BreTm
    {
        get { return m_breTm; }
        set { m_breTm = value; }
    }
    private string m_intrade;

    public string Intrade
    {
        get { return m_intrade; }
        set { m_intrade = value; }
    }
    private string m_slnName;

    public string SlnName
    {
        get { return m_slnName; }
        set { m_slnName = value; }
    }
    private string m_dayOrder;

    public string DayOrder
    {
        get { return m_dayOrder; }
        set { m_dayOrder = value; }
    }
    private string m_prePort;

    public string PrePort
    {
        get { return m_prePort; }
        set { m_prePort = value; }
    }
    private string m_nextPort;

    public string NextPort
    {
        get { return m_nextPort; }
        set { m_nextPort = value; }
    }
}
