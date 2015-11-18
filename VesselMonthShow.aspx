<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselMonthShow.aspx.cs" Inherits="VesselMonthShow" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>船舶作业计划</title>
</head>
<body>
    <form id="form1" runat="server">   
    <div>
<%--    <div id="mainPanel" style="background-color:  border-left:1px solid scrollbar; overflow: hidden; height: 100%;">
        <rsweb:ReportViewer ID="ReportViewer2" runat="server" Font-Names="Verdana" 
    Font-Size="8pt" processingmode="Remote" 
    style="width:100%; overflow:hidden;">   
     <ServerReport ReportPath="/YardPlan/VesselWeekInfo" ReportServerUrl="http://172.30.12.20/reportserver$ms$dpm2007$/"  />
    </rsweb:ReportViewer>    
</div> --%>
       <rsweb:Reportviewer id="ReportViewer1" runat="server" processingmode="Remote" Width="100%" Height="745px">     
        <ServerReport ReportPath="/YardPlan/VesselWeekInfo" ReportServerUrl="http://172.30.12.20/reportserver$ms$dpm2007$/"  />
        </rsweb:Reportviewer> 
    
    </div>
    </form>
</body>
</html>
