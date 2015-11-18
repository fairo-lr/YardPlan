<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselSearchNew.aspx.cs" Inherits="VesselSearchNew" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>船期日报查询</title>
    <link href="style/newStyle.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript" src="My97DatePicker/WdatePicker.js">
    </script>

    <script language="javascript" type="text/javascript">
    function $(id) {
        return document.getElementById(id);
    }
    function setTime()
    {
        $('iptTime').value = $dp.cal.getDateStr();   
    }

    function setTime()
    {
        $('iptTime').value = $dp.cal.getDateStr();  
        window.location="VesselSearchNew.aspx?date="+$('iptTime').value;
        
       
    }
    </script>

    <!--添加全船作业信息的TAB页-------------------------------------------------------------------->
    <style type="text/css">    
#loader_container {text-align:center;position:absolute;top:30%;width:250px;left: 480px;}
#loader {filter:alpha(opacity=70); font-family:Tahoma, Helvetica, sans;font-size:12px;color:#000000; background-color:#FFFFFF;padding:10px 0 16px 0;margin:0 auto;display:block;width:250px;border:3px solid #333333;text-align:left; z-index:2;}
#progress { height:5px;font-size:1px;width:4px;position:relative;top:1px;left:0px;background-color:#333333;}
#loader_bg {background-color:#cccccc;position:relative;top:8px;left:8px;height:7px;width:233px;font-size:1px;}
/*设置超链接样式*/ 

/*整个tab层居中，宽度为600px*/ 
#tabDiv
{ 
width:100%; 
padding-bottom: 0px; 
border-right: #b2c9d3 1px solid; 
border-top: #b2c9d3 1px solid; 
border-left: #b2c9d3 1px solid; 
border-bottom: #b2c9d3 1px solid; 
background: #ffffff; 
} 

#tabDiv3
{ 
width:100%; 
padding-bottom: 0px; 
border-right: #b2c9d3 1px solid; 
border-top: #b2c9d3 1px solid; 
border-left: #b2c9d3 1px solid; 
border-bottom: #b2c9d3 1px solid; 
background: #ffffff; 
} 

/*tab头的样式*/ 
#tabsHead,#tabsHead2
{ 
padding-left: 0px; 
/*height: 26px; 
background-color: #006699;
font-size: 1em;  */
margin: 1px 0px 0px; 
line-height: 26px; 
font-family:Arial,sans-serif;
align:right;
} 

/*已选tab头（超链接）的样式*/ 

.curtab 
{ 
padding-top: 0px; 
padding-right: 10px; 
padding-bottom: 0px; 
padding-left: 10px; 
border-right: #b2c9d3 1px solid; 
font-weight: bold; 
float: left; 
cursor: pointer; 
background: #ffffff; 
font-family:Arial,sans-serif;
color: #0B3A8A; 
} 

/*未选tab头（超链接）的样式*/ 

.tabs 
{ 
border-right: #c1d8e0 1px solid; 
padding-top: 0px; 
padding-right: 10px; 
padding-bottom: 0px; 
padding-left: 10px; 
font-weight: normal; 
float: left; 
cursor: pointer; 
font-family:Arial,sans-serif;
color: #ffffff; 
background-color:#006699;
} 

p 
{ 
font-size:9pt; 
margin-left:20pt; 
} 

#wrap {
    position:absolute;
    width:70px;
    height:50px;
    /*left:expression((body.clientWidth-150)/2);top:expression((body.clientHeight-100)/2);*/
    left:expression(body.clientWidth-80);top:55;
}

#box {
    width:70px;
    height:50px;
    filter:alpha(opacity=80);
    opacity:1;
    background:#fff;
    border:1px #000 solid;
    padding:10px;
    font-size:14px;
    line-height:170%;
    position:absolute;
    font-family:Tahoma, Helvetica, sans;
}

.situationTimeClass
{
    position:absolute;
    right:-113px;
    padding-top:3px;
    padding-bottom:4px
}
.situationTimeClass2
{
    position:absolute;
    right:5px;
    padding-top:3px;
    padding-bottom:10px
}
.situationTitleClass
{
padding-top:3px;
padding-bottom:4px;
padding-left:3px;
}
.situationTitleClass2
{
padding-top:3px;
padding-bottom:10px;
padding-left:3px;
}
.reportCss
{   
    heigth:0pt; 
   overflow:auto; 
  scrollbars:0; 
   overflow:hidden;
   <%-- overflow-y:auto;
    overflow-x:auto;--%>
} 
.hiddenY
        {
            overflow-y: hidden;  //去掉坚滚动条
            overflow-x: auto;
        }
</style>

    <script language="javascript" type="text/jscript">     
   //             $(document).ready(function() {                                                 
//    FormAjust();                                                                
//});                                                                            

function FormAjust() {                                                    
    var oReportDiv =document.getElementById('ReportViewr1');// $("div[id$='ReportViewer1']");    
   
   alert('XXX');
  alert(<%=this.ReportViewer1.ClientID %> );
  alert(document.getElementById('ReportViewer1')); 
  alert(document.getElementById('ReportViewr1').firstChild);
//   alert(oReoprtDiv); 
//    var constheight = 15;                                             
//    
//           oReportDiv.css("overflow", "hidden");    
//                        
//                        var innerDiv = $(oReportDiv).find("div").get(0);    
//                        var table = $(oReportDiv).find("table").get(0);       
//                        var tableHeight = $(table).attr("offsetHeight");      
//                        var tableDiv = $(table).find("div").get(0);          
//                        var tableWidth = $($(tableDiv).find("table").get(0)).attr("offsetWidth"); 
//                        $$(innerDiv).css("height", tableHeight + constheight);                     
//                        $$(innerDiv).css("width", tableWidth + constheight * 6);           
}


function setScoll()
{
    var div=document.getElementById("ReportViewer1");
    var scollTop=document.documentElement.scrollTop||document.body.scrollTop||0;
   div.style.top=scollTop+'px'; 
}

function diffDate(p_edtm,p_sttm,type)
{   
    //alert('xx');
    now = new Date();
    var nowYearED;
    var nowYearST;
    if (p_sttm.substr(0,2) >p_edtm.substr(0,2) ){
        nowYearED = now.getFullYear()-1;
        nowYearST = now.getFullYear();
    }else{
        nowYearED = now.getFullYear();
        nowYearST = now.getFullYear();
    }
    
    t_edtm = new Date(now.getFullYear(),p_edtm.substr(0,2)-1,p_edtm.substr(2,2),p_edtm.substr(5,2),p_edtm.substr(7,2));
    t_sttm = new Date(now.getFullYear(),p_sttm.substr(0,2)-1,p_sttm.substr(2,2),p_sttm.substr(5,2),p_sttm.substr(7,2));
    //alert(((t_edtm - t_sttm)/1000/60).toFixed(2));	
   // alert(this);
   showToolTip(event.clientX,event.clientY,((t_edtm - t_sttm)/1000/3600).toFixed(2),type);
}

function mouseout()
{
	$('tooltipDiv').style.visibility="hidden";
}

function showToolTip(mouseX,mouseY,text,type)
{
    var str="";
	if (type==1){
	    str="靠泊耗时："+text+" 小时";
	}else{
	    str = "离泊耗时："+text+" 小时";
	};
	//alert(document.documentElement.scrollTop);
	$('tooltipDiv').innerHTML = str;
	$('tooltipDiv').style.left = mouseX;
	$('tooltipDiv').style.top = mouseY+document.documentElement.scrollTop;
	$('tooltipDiv').style.visibility="visible";
}

    function bodyLoad()    
 { //   FormAjust();                   
     $('iptTime').value = $("hdDate").value;
   } 
  
   
    </script>

    <!--添加全船作业信息的TAB页-------------------------------------------------------------------->
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
</head>
<body onload="bodyLoad();setScoll()"  style="font-size: 9pt">
    <form id="form1" runat="server">
        <div id="tooltipDiv" style="visibility: hidden; position: absolute; border: solid 1px black;
            width: 110px; height: 26px; top: 53px; left: 609px; background-color: #ffffe1;
            z-index: 100">
        </div>
        &nbsp;
        <div style="position: absolute; left: 15px;">
            <asp:HiddenField ID="hdDate" runat="server" />
            <div style="text-align: left">
                <table border="0" cellpadding="0" cellspacing="0" style="border-right: #b2c9d3 1px solid;
                    border-top: #B2C9D3 1px solid; margin: 1px; border-left: #b2c9d3 1px solid; border-bottom: #b2c9d3 1px solid;
                    width: 1200px;">
                    <tr>
                        <td style="border-bottom: #b2c9d3 1px solid; height: 202px; width: 1199px;">                        
                            <table style="width: 1200px; height: 128px" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="6"  style="height: 101px">
                                        <asp:Image ID="Image1" runat="server" Height="99px" ImageUrl="images/xscttitle.png" Width="516px" />
                                    </td>
                                    <td colspan="4" style="vertical-align: bottom; text-align: right; height: 101px; width: 444px; font-size:9pt;">
                                   <div style="vertical-align:bottom;">
                                    <asp:HyperLink ID="hypControlDaily" NavigateUrl="~/ControlDailySearch.aspx" runat="server" Height="19px" Font-Size="9pt">中控日报查询</asp:HyperLink>
                                        <asp:HyperLink ID="lnkBusyManager" NavigateUrl="VesselManager.aspx" Font-Size="9pt"  runat="server" Height="19px" >船期计划管理</asp:HyperLink>                                                                 
                                        </div> 
                                        <br />
                                        <label>日期：</label>
                                        <input id="iptTime" name="searchTime" class="Wdate" onclick="WdatePicker( {onpicked:setTime})"style="width: 93px" />
                                        <asp:Button ID="btnBefore" runat="server" Text="上一工班" OnClick="btnBefore_Click" />
                                        <asp:Button ID="btnAfter"   runat="server" Text="下一工班" OnClick="btnAfter_Click" />

                                        </td>
                                        
                                </tr>
                                <tr>
                                    <td colspan="2" style="width: 71px; height: 19px">
                                    </td>
                                    <td colspan="1" style="width: 127px; height: 19px">
                                    </td>
                                    <td style="width: 120px; height: 19px">
                                    </td>
                                    <td style="width: 100px; height: 19px">
                                    </td>
                                    <td style="width: 57px; height: 19px">
                                    </td>
                                    <td style="width: 130px; height: 19px">
                                    </td>
                                    <td style="width: 100px; height: 19px; text-align: right">
                                    </td>
                                    <td style="width: 220px; height: 19px; text-align: right">
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table width="1200px" cellpadding="0" cellspacing="0" style="font-size: 9pt">
                                <tr>
                                <td style="height: 16px;width:92px;">
                                <asp:Label ID="Label12" runat="server" Text="周船期计划" Font-Bold="True" Width="90px" />
                                </td>
                                    <td style="width: 66px; text-align: right; height: 16px;">
                                        <asp:Label ID="Label10" runat="server" Text="作业日期：" Font-Bold="True" Width="65px"></asp:Label></td>
                                    <td style="width: 80px; height: 16px;">
                                        <asp:Label ID="lblDate" Text=" " runat="server" Width="80px" Font-Underline="True"
                                            ForeColor="Blue"></asp:Label></td>                                      
                                    <td style="width: 66px; text-align: right; height: 16px;">
                                        <asp:Label ID="Label5" runat="server" Font-Bold="True" Text="计划员：" Width="65px"></asp:Label></td>
                                    <td style="width: 45px; height: 16px;">
                                        <asp:Label ID="lblSuperVisor" runat="server" Font-Underline="True" Text=" " Width="40px"></asp:Label></td>
                                  <td style="width: 66px; text-align: right; height: 16px;">
                                        <asp:Label ID="Label8" runat="server" Font-Bold="True"  Text="船期主任：" Visible="false" Width="65px"></asp:Label></td>
                                    <td style="width: 45px; height: 16px;">
                                        <asp:Label ID="lblSuper" runat="server" Text="Label" Font-Underline="True" Visible="false" Width="40px"></asp:Label></td>
                                    <td style="height: 16px; width:480px;">
                                   </td> 
                                    <td style="width: 130px;text-align:right; height: 16px;">
                                        <asp:Label ID="lblFinalTimeShow" runat="server" Font-Bold="True" Text="船期计划提交时间："></asp:Label></td>
                                    <td style="width: 120px;text-align:left; height: 16px;">
                                        <asp:Label ID="lblFinalTime" runat="server" Font-Bold="False" Font-Size="9pt" Text="   " Font-Underline="True" ForeColor="Blue"></asp:Label></td>
                                </tr>
                            </table>
                            <asp:SqlDataSource ID="SqlDsInformation" runat="server" ConnectionString="<%$ ConnectionStrings:YardPlanConnectionString1 %>"
                                SelectCommand="SELECT [vi_id], max([vi_date]) [vi_date], max([vi_supervisor]) [vi_supervisor], max([vi_vesselpeople]) [vi_vesselpeople], max([vc_recordtime]) [vi_recordtime], max([vi_submit]) [vi_submit] FROM [VesselInfo],[vesselContainer] WHERE vi_date=vc_date and convert(varchar(10),vi_date,120)=@date group by vi_id ">
                                <SelectParameters>
                                    <asp:Parameter Name="date" Type="String" />                                    
                                </SelectParameters>
                            </asp:SqlDataSource>
                      
                           </td> 
                          </tr>
                         
                  <tr>
              <td style="border-bottom: #b2c9d3 1px solid; width: 1200px;height:800px;">   
             <br />                         
                           <asp:Label ID="Label6" runat="server" Font-Bold="True" Height="16px" Text="船舶作业计划"
                                Width="150px"></asp:Label><asp:Label ID="Label2" runat="server" Font-Bold="False" Height="16px"
                                Width="700px"></asp:Label>
                                <asp:HyperLink ID="hypLink" runat="server" Target="_blank" NavigateUrl="vesselmonthshow.aspx" text="船舶作业计划" Width="100px" ></asp:HyperLink>
                                <asp:Label ID="lblVesselTime" style=" text-align:right;" runat="server"  Font-Size="9pt" Height="16px" Text="Time" Width="200px"></asp:Label>                            <%-- <span onmouseover='scrollb=setInterval("ReportViewer1.scrollLeft-=10",100)'                               style="background-color:LightBlue; border:solid 1pt black; cursor:pointer;"                              onmouseout=clearInterval(scrollb)>◀向左</span>          &nbsp;   &nbsp;                        <span onmouseover='scrollb=setInterval("ReportViewer1.scrollLeft+=10",100)'                         style="background-color:LightBlue; border:solid 1pt black; cursor:pointer;"                         onmouseout=clearInterval(scrollb)>向右▶</span>&nbsp;&nbsp;                         <span onmouseover='scrollb=setInterval("ReportViewer1.scrollTop-=10",100)'                         style="background-color:LightBlue; border:solid 1pt black; cursor:pointer;"                         onmouseout=clearInterval(scrollb)>向上▲</span>&nbsp;&nbsp;      <span onmouseover='scrollb=setInterval("ReportViewer1.scrollTop+=10",100)'      style="background-color:LightBlue; border:solid 1pt black; cursor:pointer;"                  onmouseout=clearInterval(scrollb)>向下▼</span>--%>
                                <rsweb:ReportViewer  id="ReportViewer1" runat="server" Width="1200px" Height="742px" Font-Size="8pt" 
                                ProcessingMode="Remote" Font-Names="Verdana" ShowParameterPrompts="False"   CssClass="reportCss" ShowDocumentMapButton="False" OnLoad="ReportViewer1_Load">
                                  <ServerReport ReportPath="/YardPlan/VesselWeekInfo" ReportServerUrl="http://172.30.12.20/reportserver$ms$dpm2007$/"  />
                              </rsweb:ReportViewer> 
                          
                             <br /> 
                             <br />                            
                  <asp:Panel ID="Panel1" runat="server" Height="50px" Width="125px">
                  </asp:Panel>
                        </td> 
                  </tr>
                  <tr>
                    <td style="border-bottom: #b2c9d3 1px solid; width: 1199px; height: 800px;">                      
                     <br />       
                     <!-----------------泊位图------------------------------------------------------>
                            <asp:Label ID="Label1" runat="server" Font-Bold="True" Height="16px" Text="泊位图"
                                Width="150px"></asp:Label><asp:Label ID="Label3" runat="server" Font-Bold="False" Height="16px"
                                Width="700px"></asp:Label>
                                <asp:HyperLink ID="hypBerth" runat="server" Target="_blank"  NavigateUrl="Vessel_BerthNew.aspx" text=" 泊 位 图 " Width="100px" ></asp:HyperLink>
                                <asp:Label ID="lblBerthTime" style=" text-align:right;" runat="server"  Font-Size="9pt" Height="16px" Text="Time" Width="200px"></asp:Label>
                                
                       <iframe id="framePage"  frameborder="0" scrolling="auto"  style="width: 1200px; height: 742px;" src="Vessel_BerthNew.aspx?date=<%=this.hdDate.Value %>" >
                                PortPage
                                </iframe> 
                          <br />
                        <asp:Panel ID="Panel2" runat="server" Height="50px" Width="125px">
                        </asp:Panel>
                            </td>
                    </tr>        
                 </table>                
                <asp:HiddenField ID="hdTab" runat="server" />
                <asp:AccessDataSource ID="AccessDataSource1" runat="server" ConflictDetection="CompareAllValues"
                    DataFile="../user_mng.mdb" DeleteCommand="DELETE FROM [user_mng] WHERE [user_id] = ? AND [user_name] = ? AND [user_psw] = ?"
                    InsertCommand="INSERT INTO [user_mng] ([user_id], [user_name], [user_psw]) VALUES (?, ?, ?)"
                    OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [user_id], [user_name], [user_psw] FROM [user_mng] ORDER BY [user_id]"
                    UpdateCommand="UPDATE [user_mng] SET [user_name] = ?, [user_psw] = ? WHERE [user_id] = ? AND [user_name] = ? AND [user_psw] = ?">
                    <InsertParameters>
                        <asp:Parameter Name="user_id" Type="Int32" />
                        <asp:Parameter Name="user_name" Type="String" />
                        <asp:Parameter Name="user_psw" Type="String" />
                    </InsertParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="original_user_id" Type="Int32" />
                        <asp:Parameter Name="original_user_name" Type="String" />
                        <asp:Parameter Name="original_user_psw" Type="String" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="user_name" Type="String" />
                        <asp:Parameter Name="user_psw" Type="String" />
                        <asp:Parameter Name="original_user_id" Type="Int32" />
                        <asp:Parameter Name="original_user_name" Type="String" />
                        <asp:Parameter Name="original_user_psw" Type="String" />
                    </UpdateParameters>
                </asp:AccessDataSource>
                <asp:GridView ID="gvInfo" runat="server" DataSourceID="SqlDsInformation" AutoGenerateColumns="False" Visible="False">
                    <Columns>
                        <asp:BoundField DataField="vi_id" HeaderText="vi_id" InsertVisible="False" ReadOnly="True"
                            SortExpression="vi_id" />
                        <asp:BoundField DataField="vi_date" HeaderText="vi_date" SortExpression="vi_date" />
                        <asp:BoundField DataField="vi_supervisor" HeaderText="vi_supervisor" SortExpression="vi_supervisor" />
                        <asp:BoundField DataField="vi_vesselpeople" HeaderText="vi_vesselpeople" SortExpression="vi_vesselpeople" />
                        <asp:BoundField DataField="vi_recordtime" HeaderText="vi_recordtime" SortExpression="vi_recordtime" />
                        <asp:BoundField DataField="vi_submit" HeaderText="vi_submit" SortExpression="vi_submit" />
                    </Columns>
                </asp:GridView>
                <br />
            </div>        
           </div> 
        
    </form>
</body>
</html>
