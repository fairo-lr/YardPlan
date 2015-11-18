<%@ WebHandler Language="C#" Class="YardAnalyse_Get_BayInfo" %>

using System;
using System.Web;

public class YardAnalyse_Get_BayInfo : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {       
        context.Response.ContentType = "text/plain";

        string selectSQL = @"
";
        
        context.Response.Write("Hello World");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}  