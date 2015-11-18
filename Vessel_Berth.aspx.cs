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

public partial class Vessel_Berth : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        string t_date = String.Empty;
        if (this.Request.QueryString["date"] != null)
        {
            t_date = this.Request.QueryString["date"].ToString() + "";
            this.hdDate.Value = t_date;
        }
        else
        {
            this.hdDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
        }

    }
}
