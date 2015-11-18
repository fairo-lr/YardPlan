<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselSearch.aspx.cs" Inherits="BusySearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>堆场日报查询</title>
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
    function bodyLoad()
    {
        $('iptTime').value = $("hdDate").value;
        
        var tab=document.getElementById("hdTab").value;
       // alert(tab);
        switch(tab)
        {
            case "1":
                showTab('tabs11','tabContent11','tabDiv','tabsHead'); 
                break; 
            case "2":
                showTab('tabs12','tabContent12','tabDiv','tabsHead'); 
                break;
            case "3":
                showTab('tabs13','tabContent13','tabDiv','tabsHead'); 
                break;
            case "4":
                showTab('tabs14','tabContent14','tabDiv','tabsHead');
                break;
            case "5":
                showTab('tabs15','tabContent15','tabDiv','tabsHead');
                break;
            default:
                showTab('tabs11','tabContent11','tabDiv','tabsHead');       
                break;          
        }
        
       // showTab('tabs4','tabContent4','tabDiv2','tabsHead2')
    };

    function setTime()
    {
        $('iptTime').value = $dp.cal.getDateStr();  
        window.location="VesselSearch.aspx?date="+$('iptTime').value;
       
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
</style>

    <script language="javascript" type="text/jscript"> 

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

//显示tab（tabHeadId：tab头中当前的超链接；tabContentId要显示的层ID） 

function showTab(tabHeadId,tabContentId,tabDiv,tabsHead) 
{ 
    //tab层 
    var tabDiv = document.getElementById(tabDiv); 
    //将tab层中所有的内容层设为不可见 
    
    //遍历tab层下的所有子节点 
    var taContents = tabDiv.childNodes; 
    for(i=0; i<taContents.length; i++) 
    { 
        //将所有内容层都设为不可见 

        if(taContents[i].id!=null && taContents[i].id != tabsHead) 
        { 
            taContents[i].style.display = 'none'; 

        } 
    } 

    //将要显示的层设为可见 
    document.getElementById(tabContentId).style.display = 'block'; 

    //遍历tab头中所有的超链接 
    var tabHeads = document.getElementById(tabsHead).getElementsByTagName('a'); 
    for(i=0; i<tabHeads.length; i++) 
    { 
        //将超链接的样式设为未选的tab头样式 
        tabHeads[i].className='tabs'; 
    } 
    //将当前超链接的样式设为已选tab头样式 
    document.getElementById(tabHeadId).className='curtab'; 
    document.getElementById(tabHeadId).blur(); 
   document.getElementById("hdTab").value=tabHeadId.substr(5,1);
  //alert(document.getElementById("hdTab").value);  
} 
    
    </script>

    <!--添加全船作业信息的TAB页-------------------------------------------------------------------->
</head>
<body onload="bodyLoad();" style="font-size: 9pt">
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
                    width: 960px;">
                    <tr>
                        <td style="border-bottom: #b2c9d3 1px solid; height: 202px; width: 960px;">                        
                            <table style="width: 960px; height: 128px" cellpadding="0" cellspacing="0">
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
                            <table width="960px" cellpadding="0" cellspacing="0" style="font-size: 9pt">
                                <tr>
                                <td style="height: 16px">
                                <asp:Label ID="Label12" runat="server" Text="48小时作业计划" Font-Bold="True" Width="120px" />
                                </td>
                                    <td style="width: 66px; text-align: right; height: 16px;">
                                        <asp:Label ID="Label10" runat="server" Text="作业日期：" Font-Bold="True" Width="65px"></asp:Label></td>
                                    <td style="width: 140px; height: 16px;">
                                        <asp:Label ID="lblDate" Text=" " runat="server" Width="140px" Font-Underline="True"
                                            ForeColor="Blue"></asp:Label></td>
                                       <td style="width: 66px; text-align: right; height: 16px;">
                                        <asp:Label ID="Label8" runat="server" Font-Bold="True"  Text="堆场主任：" Visible="false" Width="65px"></asp:Label></td>
                                    <td style="width: 45px; height: 16px;">
                                        <asp:Label ID="lblSuper" runat="server" Text="Label" Font-Underline="True" Visible="false" Width="40px"></asp:Label></td>
                                    <td style="width: 66px; text-align: right; height: 16px;">
                                        <asp:Label ID="Label5" runat="server" Font-Bold="True" Text="计划员：" Width="65px"></asp:Label></td>
                                    <td style="width: 45px; height: 16px;">
                                        <asp:Label ID="lblSuperVisor" runat="server" Font-Underline="True" Text=" " Width="40px"></asp:Label></td>
                                 <td style="height: 16px">
                                   </td> 
                                    <td style="width: 180px;text-align:right; height: 16px;">
                                        <asp:Label ID="lblFinalTimeShow" runat="server" Font-Bold="True" Text="堆场日报提交时间："></asp:Label></td>
                                    <td style="width: 120px;text-align:left; height: 16px;">
                                        <asp:Label ID="lblFinalTime" runat="server" Font-Bold="False" Font-Size="9pt" Text="   " Font-Underline="True" ForeColor="Blue"></asp:Label></td>
                                </tr>
                            </table>
                            <asp:SqlDataSource ID="SqlDsInformation" runat="server" ConnectionString="<%$ ConnectionStrings:YardPlanConnectionString1 %>"
                                SelectCommand="SELECT [vi_id], [vi_date], [vi_supervisor], [vi_vesselpeople], [vi_recordtime], [vi_submit] FROM [VesselInfo] WHERE convert(varchar(10),vi_date,120)=@date">
                                <SelectParameters>
                                    <asp:Parameter Name="date" Type="String" />                                    
                                </SelectParameters>
                            </asp:SqlDataSource>
                      
                           </td> 
                          </tr>
                          <tr>
                       <td style=" width: 100%;"> 
                            
                             <div id="tabDiv">
                        <!--tab头-->
                        <div id="tabsHead" style="width: 100%">
                            <a id="tabs11" runat="Server" class="curtab" href="javascript:showTab('tabs11','tabContent11','tabDiv','tabsHead')">
                               航线</a> <a id="tabs12" runat="Server" class="tabs" href="javascript:showTab('tabs12','tabContent12','tabDiv','tabsHead')">
                                潮汐表</a> <a id="tabs13" runat="Server" class="tabs" href="javascript:showTab('tabs13','tabContent13','tabDiv','tabsHead')">
                                        注意事项</a> 
                        </div>                                                    
                        <div id="tabContent11" runat="server" style="display: block">
                            <table   cellpadding="0" cellspacing="0" >
                                <tr>
                                    <td align="left" style="height: 16px">
                                   <br /> 
                                        <asp:Label ID="Lable2" runat="server"  Text="航线" Width="150px" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td align="right" style="height: 16px">
                                        <asp:Label ID="lblVisselTime"  runat="server" Text=" "  ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                <td  colspan="2" style="width:100%">
                                  <asp:GridView ID="gvLine" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDsLine"
                                CellPadding="4" ForeColor="#333333" GridLines="None" EmptyDataText="工班基本信息无数据！"
                                CellSpacing="1" BackColor="#B2C9D3" Width="960px">
                                <FooterStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                <RowStyle ForeColor="#333333" BackColor="White" HorizontalAlign="Center" />
                                <EditRowStyle BackColor="#999999" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                <AlternatingRowStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                                <EmptyDataRowStyle BackColor="White" />
                                      <Columns>
                                          <asp:BoundField DataField="lw_id" HeaderText="ID" InsertVisible="False" ReadOnly="True"
                                              SortExpression="lw_id" />
                                          <asp:BoundField DataField="lw_date" HeaderText="日期" SortExpression="lw_date" />
                                          <asp:BoundField DataField="lw_Line" HeaderText="航线" SortExpression="lw_Line" />
                                          <asp:BoundField DataField="lw_BeforePort" HeaderText="上一港" SortExpression="lw_BeforePort" />
                                          <asp:BoundField DataField="lw_NextPort" HeaderText="下一港" SortExpression="lw_NextPort" />
                                          <asp:BoundField DataField="lw_ETA" HeaderText="ETA" SortExpression="lw_ETA" />
                                          <asp:BoundField DataField="lw_ETB" HeaderText="ETB" SortExpression="lw_ETB" />
                                          <asp:BoundField DataField="lw_ETD" HeaderText="ETD" SortExpression="lw_ETD" />
                                          <asp:BoundField DataField="lw_suffocation" HeaderText="截熏蒸" SortExpression="lw_suffocation" />
                                          <asp:BoundField DataField="lw_gateIn" HeaderText="截进场" SortExpression="lw_gateIn" />
                                          <asp:BoundField DataField="lw_Custom" HeaderText="海关截单" SortExpression="lw_Custom" />
                                          <asp:BoundField DataField="lw_terminal" HeaderText="码头截单" SortExpression="lw_terminal" />
                                          <asp:BoundField DataField="lw_recordtime" HeaderText="登记时间" SortExpression="lw_recordtime" />
                                      </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDsLine" runat="server" ConnectionString="<%$ ConnectionStrings:YardPlanConnectionString1 %>" SelectCommand="SELECT [lw_id], [lw_date], [lw_Line], [lw_BeforePort], [lw_NextPort], [lw_ETA], [lw_ETB], [lw_ETD], [lw_suffocation], [lw_gateIn], [lw_Custom], [lw_terminal], [lw_recordtime] FROM [LineWeek] ">
                            </asp:SqlDataSource>
                           <br /> 
                                </td>
                                </tr>
                            </table>
                           </div> 
                        <div id="tabContent12" runat="server" style="display: block">
                            <table  cellpadding="0" cellspacing="0" >                             
                                <tr>
                                    <td align="left"  style=" height: 16px; ">   
                                   <br />                                
                                        <asp:Label ID="Label3" runat="server" Text="潮汐表" Width="150px" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td align="right"  style=" height: 16px; ">
                                        <br />
                                        <asp:Label ID="lblYardTime" runat="server" Text=" " Width="270px" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                   <td  colspan="2" style="width:100%">
                                       <asp:GridView ID="gvTide"  runat="server" AutoGenerateColumns="False" DataSourceID="SqlDsTide"
                                CellPadding="4" ForeColor="#333333" GridLines="None" EmptyDataText="无数据！"
                                BorderColor="LightGray" CellSpacing="1" BackColor="#B2C9D3" Width="960px">
                                <FooterStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                <RowStyle ForeColor="#333333" BackColor="White" HorizontalAlign="Center" />
                                <EditRowStyle BackColor="#E0E0E0" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                <AlternatingRowStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                                <EmptyDataRowStyle BackColor="White" />
                                           <Columns>
                                               <asp:BoundField DataField="td_id" HeaderText="ID" InsertVisible="False" ReadOnly="True"
                                                   SortExpression="td_id" />
                                               <asp:BoundField DataField="td_height" HeaderText="潮高" SortExpression="td_height" />
                                               <asp:BoundField DataField="td_time" HeaderText="潮时" SortExpression="td_time" />
                                               <asp:BoundField DataField="td_date" HeaderText="日期" SortExpression="td_date" />
                                           </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDsTide" runat="server" ConnectionString="<%$ ConnectionStrings:YardPlanConnectionString1 %>"
                                SelectCommand="SELECT [td_id], [td_height], [td_time], [td_date] FROM [Tide]  where convert(varchar(10),td_time,120)=@date	or convert(varchar(10),dateadd(day,-1,td_time),120)=@date">
                                <SelectParameters>
                                <asp:Parameter Name="date" Type="string"  />
                                </SelectParameters>
                                </asp:SqlDataSource> 
                                    <br /> 
                                </td>
                                </tr>
                            </table>
                            </div>
                        <div id="tabContent13" runat="server" style="display: block"> 
                                   <table  cellpadding="0" cellspacing="0" >
                                <tr>
                                    <td align="left" style=" height: 16px; ">
                                        <br />
                                        <asp:Label ID="Label4" runat="server" Height="16px" Text="注意事项" Width="150px" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td align="right"  style=" height: 16px; ">
                                        <br />
                                        <asp:Label ID="lblPlanTime" runat="server" ForeColor="Red" Text=" " Width="270px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                          <td  colspan="2" style="width:100%">
                               <asp:GridView ID="gvRemark"  runat="server" AutoGenerateColumns="False" DataSourceID="SqlDsRemark"
                                CellPadding="4" ForeColor="#333333" GridLines="None" Width="960px" EmptyDataText="机械使用数量无数据！"
                                BorderColor="LightGray" CellSpacing="1" BackColor="#B2C9D3">
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle ForeColor="#333333" BackColor="White" HorizontalAlign="Center" />
                                <EditRowStyle BackColor="#999999" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                <AlternatingRowStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                                <EmptyDataRowStyle BackColor="White" />
                                   <Columns>
                                       <asp:BoundField DataField="rm_id" HeaderText="ID" InsertVisible="False" ReadOnly="True"
                                           SortExpression="rm_id" />
                                       <asp:BoundField DataField="rm_Attention" HeaderText="注意事项" SortExpression="rm_Attention" />
                                       <asp:BoundField DataField="rm_passs" HeaderText="是否过期" SortExpression="rm_passs" />
                                       <asp:BoundField DataField="rm_recordtime" HeaderText="记录时间" SortExpression="rm_recordtime" />
                                   </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDsRemark" runat="server" ConnectionString="<%$ ConnectionStrings:YardPlanConnectionString1 %>"
                                SelectCommand="SELECT [rm_id], [rm_Attention], [rm_passs], [rm_recordtime] FROM [Remark]">
                            </asp:SqlDataSource>
                                      <br />  
                                </td>
                                </tr>
                            </table>
                           </div>                                  
                    </div> 
                            <br />        
                   </td>
                  </tr>
                  <tr>
              <td style="border-bottom: #b2c9d3 1px solid; width: 960px;">                               
                           <asp:Label ID="Label6" runat="server" Font-Bold="True" Height="16px" Text="船舶信息"
                                Width="150px"></asp:Label> 
                            <asp:GridView ID="gvVesselWeek" runat="server" AutoGenerateColumns="False"
                                DataSourceID="SqlDsVesselWeek" EmptyDataText="没有可显示的数据记录。" Width="960px"                                 
                                CellPadding="4" ForeColor="#333333" GridLines="None" 
                                BorderColor="LightGray" CellSpacing="1" BackColor="#B2C9D3">           
                                   <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle ForeColor="#333333" BackColor="White" HorizontalAlign="Center" />
                                <EditRowStyle BackColor="#999999" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                <AlternatingRowStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                                <EmptyDataRowStyle BackColor="White" />                     
                                <Columns>
                                    <asp:BoundField DataField="Date" HeaderText="日期" SortExpression="Date" Visible="False" />
                                    <asp:BoundField DataField="vcode" HeaderText="船东" SortExpression="vcode" />
                                    <asp:BoundField DataField="Line" HeaderText="航线" SortExpression="Line" />
                                    <asp:BoundField DataField="Customer" HeaderText="海关截单" SortExpression="Customer" />
                                    <asp:BoundField DataField="vname" HeaderText="船名" SortExpression="vname" />
                                    <asp:BoundField DataField="ivoyage" HeaderText="进口航次" SortExpression="ivoyage" />
                                    <asp:BoundField DataField="expvoyage" HeaderText="出口航次" SortExpression="expvoyage" />
                                    <asp:BoundField DataField="ETA" HeaderText="ETA" SortExpression="ETA" />
                                    <asp:BoundField DataField="ETB" HeaderText="ETB" SortExpression="ETB" />
                                    <asp:BoundField DataField="ETD" HeaderText="ETD" SortExpression="ETD" />
                                    <asp:BoundField DataField="fTime" HeaderText="完工时间" SortExpression="fTime" />
                                    <asp:BoundField DataField="owner" HeaderText="持箱人" SortExpression="owner" />
                                    <asp:BoundField DataField="CIF" HeaderText="卸船重箱" SortExpression="CIF" />
                                   <asp:BoundField DataField="CIE" HeaderText="卸船空箱" SortExpression="CIE" />
                                   <asp:BoundField DataField="COF" HeaderText="装船重箱" SortExpression="COF" />  
                                  <asp:BoundField DataField="COE" HeaderText="装船空箱" SortExpression="COE" /> 
                                    <asp:BoundField DataField="vSum" HeaderText="总量" SortExpression="vSum" />
                                    <asp:BoundField DataField="Remark" HeaderText="备注" SortExpression="Remark" />
                                    <asp:BoundField DataField="wnum" HeaderText="作业路数" SortExpression="wnum" />
                                    <asp:BoundField DataField="vTime" HeaderText="船时" SortExpression="vTime" />
                                    <asp:BoundField DataField="Finsh" HeaderText="完工标志" SortExpression="Finsh" />
                                    <asp:BoundField DataField="Gold" HeaderText="目标" SortExpression="Gold" />
                                </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDsVesselWeek" runat="server" ConnectionString="<%$ ConnectionStrings:YardPlanConnectionString1 %>"
                                ProviderName="<%$ ConnectionStrings:YardPlanConnectionString1.ProviderName %>"
                                SelectCommand="SELECT [Date]&#13;&#10;      ,[vcode]&#13;&#10;      ,[Line]&#13;&#10;      ,[Customer]&#13;&#10;      ,[vname]&#13;&#10;      ,[ivoyage]&#13;&#10;      ,[expvoyage]&#13;&#10;      ,[ETA]&#13;&#10;      ,[ETB]&#13;&#10;      ,[ETD]&#13;&#10;      ,[fTime]&#13;&#10;      ,[owner]&#13;&#10;      ,[CIF]&#13;&#10;      ,[CIE]&#13;&#10;      ,[COF]&#13;&#10;      ,[COE]&#13;&#10;       ,[vSum]&#13;&#10;      ,[Remark]&#13;&#10;      ,[wnum]&#13;&#10;      ,[vTime]&#13;&#10;      ,[Finsh]&#13;&#10;      ,[Gold]&#13;&#10;  FROM [YardPlan].[dbo].[vesselWeekInfo_vw] WHERE date=@date">
                                <SelectParameters >
                                    <asp:Parameter Name="date" Type="String"  />
                                </SelectParameters>
                            </asp:SqlDataSource>
                             <br />                            
                        </td> 
                  </tr>
                  <tr>
                    <td style="border-bottom: #b2c9d3 1px solid; width: 960px;">  
                     <br />       
                            <asp:Label ID="Label1" runat="server" Font-Bold="True" Height="16px" Text="泊位图"
                                Width="150px"></asp:Label>
                       <iframe id="framePage"  frameborder="0" scrolling="auto"  style="width: 960px; height: 597px;" src="default.aspx" >
                                PortPage
                                </iframe> 
                          <br />
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
