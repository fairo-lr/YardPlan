<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Vessel_Basic.aspx.cs" Inherits="YardPlan_Vessel_Basic" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>船期基础信息</title>
       <style type="text/css"> 
#loader_container {text-align:center;position:absolute;top:30%;width:250px;left: 480px;}
#loader {filter:alpha(opacity=70); font-family:Tahoma, Helvetica, sans;font-size:12px;color:#000000; background-color:#FFFFFF;padding:10px 0 16px 0;margin:0 auto;display:block;width:250px;border:3px solid #333333;text-align:left; z-index:2;}
#progress { height:5px;font-size:1px;width:4px;position:relative;top:1px;left:0px;background-color:#333333;}
#loader_bg {background-color:#cccccc;position:relative;top:8px;left:8px;height:7px;width:233px;font-size:1px;}


/*设置超链接样式*/ 
a 
{ 
color: #5086a5; 
text-decoration: none; 
font-size: 12px; 
} 

a:hover 
{ 
color: #5086a5; 
text-decoration: underline; 
font-size: 12px; 
} 

a:visited 
{ 
color: #5086a5; 
font-size: 12px; 
} 

/*整个tab层居中，宽度为600px*/ 

#tabDiv 
{ 
width:100%; 
margin:1em auto; 
padding-bottom: 10px; 
border-right: #b2c9d3 1px solid; 
border-top: #b2c9d3 1px solid; 
border-left: #b2c9d3 1px solid; 
border-bottom: #b2c9d3 1px solid; 
background: #ffffff; 
} 

/*tab头的样式*/ 

#tabsHead 
{ 
padding-left: 0px; 
height: 26px; 
background-color: #e8f7fc; 
font-size: 1em; 
margin: 1px 0px 0px; 
color: #5086a5; 
line-height: 26px; 
font-family:Arial,sans-serif;
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
}
 
 p 
{ 
font-size:9pt; 
margin-left:20pt; 
} 

TH {font-family:Arial,sans-serif; font-size:12px; font-weight:bold; text-align:center; background-color: #006699; color: #FFFFFF;}
TD.a {font-family:Arial,sans-serif; font-size:12px; font-weight:bold; text-align:center; background-color: #006699; color: #FFFFFF;}
 TD.ji {font-family:Arial,sans-serif; font-size:14px;font-weight:normal; text-align:right; background-color: #FFFFFF; color: #003366;}
 TD.ou {font-family:Arial,sans-serif; font-size:14px;font-weight:normal; text-align:right; background-color: #E0E0E0; color: #003366;}

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
</STYLE>
  <script language="javascript" type="text/javascript" src="My97DatePicker/WdatePicker.js"></script>
    <script language="javascript" type="text/jscript"> 
function fnCheck()
{
    return confirm('将初始化所有航线的数据，是否确定？');
}
//显示tab（tabHeadId：tab头中当前的超链接；tabContentId要显示的层ID） 
function showTab(tabHeadId,tabContentId) 
{ 
    document.getElementById("hdTab").value=tabHeadId.substring(4);
   //alert(document.getElementById("hdTab").value);
    //tab层 
    var tabDiv = document.getElementById("tabDiv"); 
    //将tab层中所有的内容层设为不可见 
    //遍历tab层下的所有子节点 
    var taContents = tabDiv.childNodes; 
    for(i=0; i<taContents.length; i++) 
    { 
        //将所有内容层都设为不可见 
        if(taContents[i].id!=null && taContents[i].id != 'tabsHead') 
        { 
            taContents[i].style.display = 'none'; 
        } 
    }   
    //将要显示的层设为可见 
    document.getElementById(tabContentId).style.display = 'block'; 
    //遍历tab头中所有的超链接 
    var tabHeads = document.getElementById('tabsHead').getElementsByTagName('a'); 

    for(i=0; i<tabHeads.length; i++)
    { 
        //将超链接的样式设为未选的tab头样式 
        tabHeads[i].className='tabs'; 
    } 
    //将当前超链接的样式设为已选tab头样式 
    document.getElementById(tabHeadId).className='curtab'; 
    document.getElementById(tabHeadId).blur(); 
} 

function bodyLoad()
{//后台有调用
      var tab=document.getElementById("hdTab").value-1;
     //tab层 
 
    var tabDiv = document.getElementById("tabDiv"); 
    //将tab层中所有的内容层设为不可见 
    //遍历tab层下的所有子节点 
    var taContents = tabDiv.childNodes; 
    for(i=0; i<taContents.length; i++) 
    { 
        //将所有内容层都设为不可见 
        if(taContents[i].id!=null && taContents[i].id != 'tabsHead' ) 
        { 
            taContents[i].style.display = 'none'; 
        }  
    }   
     document.getElementById('tabContent'+(tab+1)).style.display = 'block';  
    
    
    var tabHeads = document.getElementById('tabsHead').getElementsByTagName('a'); 
    for(i=0; i<tabHeads.length; i++)
    { 
        //将超链接的样式设为未选的tab头样式 
        tabHeads[i].className='tabs'; 
    } 
    //将当前超链接的样式设为已选tab头样式 
   //alert("SSS"); 
    document.getElementById('tabs'+(tab+1)).className='curtab'; 
 }
</script> 
</head>
<body onload="bodyLoad();" >
    <form id="form1" runat="server">
    <div>
   <div id="tabDiv"> 
    <!--tab头--> 
    <div id="tabsHead">
        <!--tab控件--> 
        <a class="curtab" id="tabs1" href="javascript:showTab('tabs1','tabContent1')">航线信息</a> 
        <a class="tabs" id="tabs2" href="javascript:showTab('tabs2','tabContent2')">潮汐表</a> 
        <a class="tabs" id="tabs3" href="javascript:showTab('tabs3','tabContent3')">注意事项</a> 
        <a class="tabs" id="tabs4" href="javascript:showTab('tabs4','tabContent4')">目标区间</a> 
         <a class="tabs" id="tabs5" href="javascript:showTab('tabs5','tabContent5')">船期人员</a> 
    </div> 
    <!------------------------------航线信息----------------------------------->
    <div id="tabContent1" style="display:block" runat="server">   
         <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" BackColor="#B2C9D3"
            DataKeyNames="lw_id" DataSourceID="SqlDataSource1" DefaultMode="Insert" Height="50px"
            Width="551px" 
            CellPadding="3" CellSpacing="1" Font-Size="9pt" GridLines="None"                      
            ToolTip="日期输入：yyyy-MM-dd" >
            <RowStyle BackColor="White" ForeColor="Black" />
            <FooterStyle BackColor="#006699" Font-Bold="True" />
            <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="White" />
            <AlternatingRowStyle BackColor="#E0E0E0" BorderStyle="Solid" ForeColor="Black" /> 
            <Fields>
                <asp:BoundField DataField="lw_id" HeaderText="ID" InsertVisible="False" ReadOnly="True"
                    SortExpression="lw_id">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="lw_date" HeaderText="日期" SortExpression="lw_date" Visible="false">
                    <ItemStyle BackColor="#E0E0E0" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle BackColor="#E0E0E0" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="lw_Line" HeaderText="航线" SortExpression="lw_Line" >
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="lw_BeforePort" HeaderText="上一港" SortExpression="lw_BeforePort" >
                    <ItemStyle BackColor="#E0E0E0" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle BackColor="#E0E0E0" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="lw_NextPort" HeaderText="下一港" SortExpression="lw_NextPort" >
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="lw_ETA" HeaderText="ETA" SortExpression="lw_ETA" >
                    <ItemStyle BackColor="#E0E0E0" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle BackColor="#E0E0E0" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="lw_ETB" HeaderText="ETB" SortExpression="lw_ETB" >
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="lw_ETD" HeaderText="ETD" SortExpression="lw_ETD" >
                    <ItemStyle BackColor="#E0E0E0" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle BackColor="#E0E0E0" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="lw_suffocation" HeaderText="截熏蒸" SortExpression="lw_suffocation" >
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="lw_gateIn" HeaderText="进箱时间" SortExpression="lw_gateIn" >
                    <ItemStyle BackColor="#E0E0E0" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle BackColor="#E0E0E0" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="lw_Custom" HeaderText="海关截单" SortExpression="lw_Custom">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="lw_terminal" HeaderText="码头截箱" SortExpression="lw_terminal" >
                    <ItemStyle BackColor="#E0E0E0" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle BackColor="#E0E0E0" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="lw_recordtime" HeaderText="记录时间" SortExpression="lw_recordtime" Visible="false" >
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                 <asp:BoundField DataField="lw_lineCode" HeaderText="航线内码" SortExpression="lw_lineCode" >
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                 <asp:CommandField ButtonType="Button" ShowInsertButton="True">
                    <FooterStyle HorizontalAlign="Center" />
                    <ItemStyle BackColor="#006699" HorizontalAlign="Center" />
                </asp:CommandField>
            </Fields>
        </asp:DetailsView>
        <asp:Button ID="btnLine" runat="server" OnClick="btnLine_Click" Text="初始化航线" OnClientClick="return fnCheck()" /><br />
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" DataKeyNames="lw_id" DataSourceID="SqlDataSource1"
            EmptyDataText="没有可显示的数据记录。"  BackColor="#B2C9D3" CellPadding="3"
             CellSpacing="1"        Font-Size="9pt" Width="970px" 
            ForeColor="#333333" GridLines="None">            
            <RowStyle BackColor="White" ForeColor="Black" />
            <FooterStyle BackColor="#006699" Font-Bold="True" />
            <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="LightSkyBlue" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="White" />
            <AlternatingRowStyle BackColor="#E0E0E0" BorderStyle="Solid" ForeColor="Black" />
            <EmptyDataRowStyle BackColor="White" ForeColor="Red" />
            <Columns>
                <asp:BoundField DataField="lw_id" HeaderText="ID" ReadOnly="True" SortExpression="lw_id" Visible="False" />
                <asp:BoundField DataField="lw_date" HeaderText="日期" SortExpression="lw_date" ReadOnly="True"  Visible="False" />
                <asp:BoundField DataField="lw_Line" HeaderText="航线" SortExpression="lw_Line" >
                    <ControlStyle Width="80px" />
                </asp:BoundField>
                <asp:BoundField DataField="lw_BeforePort" HeaderText="上一港" SortExpression="lw_BeforePort" >
                    <ControlStyle Width="80px" />
                </asp:BoundField>
                <asp:BoundField DataField="lw_NextPort" HeaderText="下一港" SortExpression="lw_NextPort" >
                    <ControlStyle Width="80px" />
                </asp:BoundField>
                <asp:BoundField DataField="lw_ETA" HeaderText="ETA" SortExpression="lw_ETA" >
                    <ControlStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="lw_ETB" HeaderText="ETB" SortExpression="lw_ETB" >
                    <ControlStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="lw_ETD" HeaderText="ETD" SortExpression="lw_ETD" >
                    <ControlStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="lw_suffocation" HeaderText="截熏蒸" SortExpression="lw_suffocation" >
                    <ControlStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="lw_gateIn" HeaderText="截箱" SortExpression="lw_gateIn" >
                    <ControlStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="lw_Custom" HeaderText="海关截单" SortExpression="lw_Custom" >
                    <ControlStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="lw_terminal" HeaderText="码头截单" SortExpression="lw_terminal" >
                    <ControlStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="lw_recordtime" HeaderText="记录时间" Visible="false"  SortExpression="lw_recordtime" >
                    <ControlStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="lw_lineCode" HeaderText="航线内码" SortExpression="lw_lineCode" >
                    <ControlStyle Width="100px" />
                </asp:BoundField>
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:YardPlanConnectionString1 %>"
            DeleteCommand="DELETE FROM [LineWeek] WHERE [lw_id] = @lw_id" InsertCommand="INSERT INTO [LineWeek] ([lw_date], [lw_Line], [lw_BeforePort], [lw_NextPort], [lw_ETA], [lw_ETB], [lw_ETD], [lw_suffocation], [lw_gateIn], [lw_Custom], [lw_terminal] ,[lw_lineCode]) VALUES (@lw_date, @lw_Line, @lw_BeforePort, @lw_NextPort, @lw_ETA, @lw_ETB, @lw_ETD, @lw_suffocation, @lw_gateIn, @lw_Custom, @lw_terminal,@lw_lineCode)"
            ProviderName="<%$ ConnectionStrings:YardPlanConnectionString1.ProviderName %>"
            SelectCommand="SELECT [lw_id], [lw_date], [lw_Line], [lw_BeforePort], [lw_NextPort], [lw_ETA], [lw_ETB], [lw_ETD], [lw_suffocation], [lw_gateIn], [lw_Custom], [lw_terminal], [lw_recordtime], [lw_lineCode] FROM [LineWeek] order by lw_id desc"
            UpdateCommand="UPDATE [LineWeek] SET [lw_date] = @lw_date, [lw_Line] = @lw_Line, [lw_BeforePort] = @lw_BeforePort, [lw_NextPort] = @lw_NextPort, [lw_ETA] = @lw_ETA, [lw_ETB] = @lw_ETB, [lw_ETD] = @lw_ETD, [lw_suffocation] = @lw_suffocation, [lw_gateIn] = @lw_gateIn, [lw_Custom] = @lw_Custom, [lw_terminal] = @lw_terminal, [lw_lineCode] = @lw_lineCode, [lw_recordtime] = getDate() WHERE [lw_id] = @lw_id">
            <InsertParameters>
                <asp:Parameter Name="lw_date" Type="DateTime" />
                <asp:Parameter Name="lw_Line" Type="String" />
                <asp:Parameter Name="lw_BeforePort" Type="String" />
                <asp:Parameter Name="lw_NextPort" Type="String" />
                <asp:Parameter Name="lw_ETA" Type="DateTime" />
                <asp:Parameter Name="lw_ETB" Type="DateTime" />
                <asp:Parameter Name="lw_ETD" Type="DateTime" />
                <asp:Parameter Name="lw_suffocation" Type="DateTime" />
                <asp:Parameter Name="lw_gateIn" Type="DateTime" />
                <asp:Parameter Name="lw_Custom" Type="DateTime" />
                <asp:Parameter Name="lw_terminal" Type="DateTime" />
                <asp:Parameter Name="lw_recordtime" Type="DateTime" />
                <asp:Parameter Name="lw_lineCode" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="lw_date" Type="DateTime" />
                <asp:Parameter Name="lw_Line" Type="String" />
                <asp:Parameter Name="lw_BeforePort" Type="String" />
                <asp:Parameter Name="lw_NextPort" Type="String" />
                <asp:Parameter Name="lw_ETA" Type="DateTime" />
                <asp:Parameter Name="lw_ETB" Type="DateTime" />
                <asp:Parameter Name="lw_ETD" Type="DateTime" />
                <asp:Parameter Name="lw_suffocation" Type="DateTime" />
                <asp:Parameter Name="lw_gateIn" Type="DateTime" />
                <asp:Parameter Name="lw_Custom" Type="DateTime" />
                <asp:Parameter Name="lw_terminal" Type="DateTime" />
                <asp:Parameter Name="lw_recordtime" Type="DateTime" />
                <asp:Parameter Name="lw_id" Type="Int32" />
                  <asp:Parameter Name="lw_lineCode" Type="String" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Name="lw_id" Type="Int32" />
            </DeleteParameters>
        </asp:SqlDataSource>
    </div>   
      <!------------------------------潮汐表----------------------------------->  
        <div id="tabContent2" style="display:none" runat="server"> 
            <asp:DetailsView ID="DetailsView2" runat="server" AutoGenerateRows="False" DataSourceID="SqlDataSource2"
            DefaultMode="Insert" Height="50px" Width="482px" BackColor="#B2C9D3"
            CellPadding="3" CellSpacing="1" Font-Size="9pt" GridLines="None"                      
            ToolTip="日期输入：yyyy-MM-dd HH:mi" OnItemInserting="DetailsView2_ItemInserting" >
             <RowStyle BackColor="White" ForeColor="Black" />
            <FooterStyle BackColor="#006699" Font-Bold="True" />
            <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="White" />
            <AlternatingRowStyle BackColor="#E0E0E0" BorderStyle="Solid" ForeColor="Black" />  
            <Fields>
                <asp:BoundField DataField="td_id" HeaderText="ID" InsertVisible="False" ReadOnly="True"
                    SortExpression="td_id" >
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="td_date" HeaderText="日期" SortExpression="td_date" Visible="False">
                    <ItemStyle BackColor="#E0E0E0" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle BackColor="#E0E0E0" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="潮时" SortExpression="td_time">
                    <EditItemTemplate>                   
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("td_time") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                   　<input id="iptStartTime" name="StartTime" class="Wdate" runat="server" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" />                          
                    </InsertItemTemplate>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                    <ItemTemplate>
                                 <asp:Label ID="Label1" runat="server" Text='<%# Bind("td_time") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="潮高" SortExpression="td_height">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("td_height") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                      &nbsp;   &nbsp; <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("td_height") %>'></asp:TextBox>
                    </InsertItemTemplate>
                    <ItemStyle BackColor="#E0E0E0" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle BackColor="#E0E0E0" HorizontalAlign="Right" VerticalAlign="Middle" />
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("td_height") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
               <asp:CommandField ButtonType="Button" ShowInsertButton="True">
                    <FooterStyle HorizontalAlign="Center" />
                    <ItemStyle BackColor="#006699" HorizontalAlign="Center" />
                </asp:CommandField>
            </Fields>
        </asp:DetailsView>
            <asp:GridView ID="GridView2" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" DataKeyNames="td_id" DataSourceID="SqlDataSource2"
            EmptyDataText="没有可显示的数据记录。"  BackColor="#B2C9D3" CellPadding="3"
             CellSpacing="1"        Font-Size="9pt" Width="970px" 
            ForeColor="#333333" GridLines="None">            
            <RowStyle BackColor="White" ForeColor="Black" />
            <FooterStyle BackColor="#006699" Font-Bold="True" />
            <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="LightSkyBlue" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="White" />
            <AlternatingRowStyle BackColor="#E0E0E0" BorderStyle="Solid" ForeColor="Black" />
            <EmptyDataRowStyle BackColor="White" ForeColor="Red" />
            <Columns>
                <asp:BoundField DataField="td_id" HeaderText="ID" ReadOnly="True" SortExpression="td_id"  Visible="false" />
                <asp:BoundField DataField="td_date" HeaderText="日期" SortExpression="td_date" ReadOnly="true" Visible="false" />
                <asp:BoundField DataField="td_time" HeaderText="潮时" SortExpression="td_time" />
                <asp:BoundField DataField="td_height" HeaderText="潮高" SortExpression="td_height" />
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
            </Columns>
        </asp:GridView>                
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:YardPlanConnectionString1 %>"
            DeleteCommand="DELETE FROM [Tide] WHERE [td_id] = @td_id" InsertCommand="INSERT INTO [Tide] ([td_date], [td_time], [td_height]) VALUES (@td_date, @td_time, @td_height)"
            ProviderName="<%$ ConnectionStrings:YardPlanConnectionString1.ProviderName %>"
            SelectCommand="SELECT [td_id], [td_date], [td_time], [td_height] FROM [Tide] order by td_id desc"
            UpdateCommand="UPDATE [Tide] SET [td_date] = @td_date, [td_time] = @td_time, [td_height] = @td_height WHERE [td_id] = @td_id">
            <InsertParameters>
                <asp:Parameter Name="td_date" Type="DateTime" />
                <asp:Parameter Name="td_time" Type="DateTime" />
                <asp:Parameter Name="td_height" Type="Int32" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="td_date" Type="DateTime" />
                <asp:Parameter Name="td_time" Type="DateTime" />
                <asp:Parameter Name="td_height" Type="Int32" />
                <asp:Parameter Name="td_id" Type="Int32" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Name="td_id" Type="Int32" />
            </DeleteParameters>
        </asp:SqlDataSource>
    </div>
        <!------------------------------注意事项----------------------------------->  
        <div id="tabContent3" style="display:none" runat="server"> 
        <asp:DetailsView ID="DetailsView3" runat="server" AutoGenerateRows="False" DataSourceID="SqlDataSource3"
            DefaultMode="Insert" Height="50px" Width="481px" BackColor="#B2C9D3"
            CellPadding="3" CellSpacing="1" Font-Size="9pt" GridLines="None" OnItemInserting="DetailsView3_ItemInserting"   >
             <RowStyle BackColor="White" ForeColor="Black" />
            <FooterStyle BackColor="#006699" Font-Bold="True" />
            <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="White" />
            <AlternatingRowStyle BackColor="#E0E0E0" BorderStyle="Solid" ForeColor="Black" />  
            <Fields>
                <asp:BoundField DataField="rm_id" HeaderText="ID" InsertVisible="False" ReadOnly="True"
                    SortExpression="rm_id" >
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="rm_Attention" HeaderText="注意事项" SortExpression="rm_Attention" >
                    <ItemStyle  HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle  HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="rm_recordtime" HeaderText="记录时间" SortExpression="rm_recordtime" ReadOnly="True" Visible="False">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="是否过期" SortExpression="rm_passs">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("rm_passs") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                         <asp:DropDownList ID="ddlPass" runat="server" Width="150px" >
                                <asp:ListItem Value="N" Selected="true" Text="N">  </asp:ListItem> 
                                <asp:ListItem Value="Y" Text="Y"></asp:ListItem>
                         </asp:DropDownList>                        
                    </InsertItemTemplate>
                    <ItemStyle BackColor="#E0E0E0" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle BackColor="#E0E0E0" HorizontalAlign="Right" VerticalAlign="Middle" />
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("rm_passs") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                      <asp:CommandField ButtonType="Button" ShowInsertButton="True">
                    <FooterStyle HorizontalAlign="Center" />
                    <ItemStyle BackColor="#006699" HorizontalAlign="Center" />
                </asp:CommandField>
            </Fields>
        </asp:DetailsView>        
        <asp:GridView ID="GridView3" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" DataKeyNames="rm_id" DataSourceID="SqlDataSource3"
            EmptyDataText="没有可显示的数据记录。"  BackColor="#B2C9D3" CellPadding="3"
             CellSpacing="1"        Font-Size="9pt" Width="970px" 
            ForeColor="#333333" GridLines="None">            
            <RowStyle BackColor="White" ForeColor="Black" />
            <FooterStyle BackColor="#006699" Font-Bold="True" />
            <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="LightSkyBlue" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="White" />
            <AlternatingRowStyle BackColor="#E0E0E0" BorderStyle="Solid" ForeColor="Black" />
            <EmptyDataRowStyle BackColor="White" ForeColor="Red" />
            <Columns>
                <asp:BoundField DataField="rm_id" HeaderText="ID" ReadOnly="True" SortExpression="rm_id" Visible="False" />
                <asp:BoundField DataField="rm_Attention" HeaderText="注意事项" SortExpression="rm_Attention" >
                    <ControlStyle Width="593px" />
                    <ItemStyle Width="593px" />
                    <HeaderStyle Width="593px" />
                </asp:BoundField>
                <asp:BoundField DataField="rm_passs" HeaderText="是否过期" SortExpression="rm_passs" />
                <asp:BoundField DataField="rm_recordtime" HeaderText="记录时间" SortExpression="rm_recordtime" ReadOnly="True" />
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:YardPlanConnectionString1 %>"
            DeleteCommand="DELETE FROM [Remark] WHERE [rm_id] = @rm_id" InsertCommand="INSERT INTO [Remark] ([rm_Attention],  [rm_passs]) VALUES (@rm_Attention, @rm_passs)"
            ProviderName="<%$ ConnectionStrings:YardPlanConnectionString1.ProviderName %>"
            SelectCommand="SELECT [rm_id], [rm_Attention], [rm_recordtime], [rm_passs] FROM [Remark] order by rm_id desc"
            UpdateCommand="UPDATE [Remark] SET [rm_Attention] = @rm_Attention, [rm_recordtime] = getDate(), [rm_passs] = @rm_passs WHERE [rm_id] = @rm_id">
            <InsertParameters>
                <asp:Parameter Name="rm_Attention" Type="String" />
                <asp:Parameter Name="rm_recordtime" Type="DateTime" />
                <asp:Parameter Name="rm_passs" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="rm_Attention" Type="String" />
                <asp:Parameter Name="rm_recordtime" Type="DateTime" />
                <asp:Parameter Name="rm_passs" Type="String" />
                <asp:Parameter Name="rm_id" Type="Int32" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Name="rm_id" Type="Int32" />
            </DeleteParameters>
        </asp:SqlDataSource>
            <asp:HiddenField ID="hdTab" runat="server" Value="1" />
        </div> 
      <!------------------------------目标区间----------------------------------->  
           <div id="tabContent4" style="display:none" runat="server"> 
                  <asp:DetailsView ID="DetailsView4" runat="server" AutoGenerateRows="False" DataKeyNames="gu_id"
            DataSourceID="SqlDataSource4" DefaultMode="Insert" Height="50px" Width="481px" 
            BackColor="#B2C9D3"    CellPadding="3" CellSpacing="1" Font-Size="9pt" GridLines="None"  >
             <RowStyle BackColor="White" ForeColor="Black" />
            <FooterStyle BackColor="#006699" Font-Bold="True" />
            <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="White" />
            <AlternatingRowStyle BackColor="#E0E0E0" BorderStyle="Solid" ForeColor="Black" />              
            <Fields>
                <asp:BoundField DataField="gu_id" HeaderText="gu_id" ReadOnly="True" Visible="false" SortExpression="gu_id" >
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="gu_minUnit" HeaderText="起始区间" SortExpression="gu_minUnit" >
                    <ItemStyle  HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle  HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="gu_maxUnit" HeaderText="结束区间" SortExpression="gu_maxUnit" >
                    <ItemStyle BackColor="#E0E0E0" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle BackColor="#E0E0E0" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="gu_goldUnit" HeaderText="目标箱量" SortExpression="gu_goldUnit" >
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="gu_goldLine" HeaderText="所属航线" SortExpression="gu_goldLine" >
                   <ItemStyle BackColor="#E0E0E0" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle BackColor="#E0E0E0" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:CommandField ShowInsertButton="True" ButtonType="Button" >
                    <FooterStyle HorizontalAlign="Center" />
                    <ItemStyle BackColor="#006699" HorizontalAlign="Center" />
                </asp:CommandField>
            </Fields>
        </asp:DetailsView>
        <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False" DataKeyNames="gu_id" 
            DataSourceID="SqlDataSource4"  AllowPaging="True" AllowSorting="True"            
            EmptyDataText="没有可显示的数据记录。"  BackColor="#B2C9D3" CellPadding="3"
             CellSpacing="1"        Font-Size="9pt" Width="970px" 
            ForeColor="#333333" GridLines="None">            
            <RowStyle BackColor="White" ForeColor="Black" />
            <FooterStyle BackColor="#006699" Font-Bold="True" />
            <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="LightSkyBlue" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="White" />
            <AlternatingRowStyle BackColor="#E0E0E0" BorderStyle="Solid" ForeColor="Black" />
            <EmptyDataRowStyle BackColor="White" ForeColor="Red" />
            <Columns>
                <asp:BoundField DataField="gu_id" HeaderText="gu_id" ReadOnly="True" SortExpression="gu_id" Visible="False" />
                <asp:BoundField DataField="gu_minUnit" HeaderText="起始区间：" SortExpression="gu_minUnit" />
                <asp:BoundField DataField="gu_maxUnit" HeaderText="结束区间" SortExpression="gu_maxUnit" />
                <asp:BoundField DataField="gu_goldUnit" HeaderText="目标箱量" SortExpression="gu_goldUnit" />
                <asp:BoundField DataField="gu_goldLine" HeaderText="所属航线" SortExpression="gu_goldLine" />
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:YardPlanConnectionString1 %>"
            DeleteCommand="DELETE FROM [GoldUnit] WHERE [gu_id] = @gu_id" InsertCommand="INSERT INTO [GoldUnit] ([gu_minUnit], [gu_maxUnit], [gu_goldUnit], [gu_goldLine]) VALUES (@gu_minUnit, @gu_maxUnit, @gu_goldUnit,@gu_goldLine)"
            SelectCommand="SELECT [gu_id], [gu_minUnit], [gu_maxUnit], [gu_goldUnit], [gu_goldLine] FROM [GoldUnit]"
            UpdateCommand="UPDATE [GoldUnit] SET [gu_minUnit] = @gu_minUnit, [gu_maxUnit] = @gu_maxUnit, [gu_goldUnit] = @gu_goldUnit ,[gu_goldLine]=@gu_goldLine WHERE [gu_id] = @gu_id">
            <DeleteParameters>
                <asp:Parameter Name="gu_id" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="gu_minUnit" Type="Int32" />
                <asp:Parameter Name="gu_maxUnit" Type="Int32" />
                <asp:Parameter Name="gu_goldUnit" Type="Int32" />
                 <asp:Parameter Name="gu_goldLine" Type="String" />
                <asp:Parameter Name="gu_id" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="gu_minUnit" Type="Int32" />
                <asp:Parameter Name="gu_maxUnit" Type="Int32" />
                <asp:Parameter Name="gu_goldUnit" Type="Int32" />
                 <asp:Parameter Name="gu_goldLine" Type="String" />
            </InsertParameters>
        </asp:SqlDataSource> 
               <br />
              </div> 
               <!------------------------------人员----------------------------------->  
        <div id="tabContent5" style="display:none" runat="server">  
               <asp:DetailsView ID="DetailsView5" runat="server" AutoGenerateRows="False" DataSourceID="SqlDataSource5"
            DefaultMode="Insert" Height="50px" Width="481px" BackColor="#B2C9D3"
            CellPadding="3" CellSpacing="1" Font-Size="9pt" GridLines="None" OnItemInserting="DetailsView5_ItemInserting"   >
                   <RowStyle BackColor="White" ForeColor="Black" />
                   <FooterStyle BackColor="#006699" Font-Bold="True" />
                   <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
                   <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                   <EditRowStyle BackColor="White" />
                   <AlternatingRowStyle BackColor="#E0E0E0" BorderStyle="Solid" ForeColor="Black" />
                   <Fields>
                       <asp:BoundField DataField="YP_ID" HeaderText="员工工号" ReadOnly="True" SortExpression="YP_ID" >
                    <ItemStyle  HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle  HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                       <asp:BoundField DataField="YP_Name" HeaderText="员工姓名" SortExpression="YP_Name" >
                    <ItemStyle BackColor="#E0E0E0" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle BackColor="#E0E0E0" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                    <asp:CommandField ShowInsertButton="True" ButtonType="Button" >
                    <FooterStyle HorizontalAlign="Center" />
                    <ItemStyle BackColor="#006699" HorizontalAlign="Center" />
                </asp:CommandField>
                   </Fields>
               </asp:DetailsView>
               <asp:GridView ID="GridView5" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" DataSourceID="SqlDataSource5" DataKeyNames="YP_ID"
            EmptyDataText="没有可显示的数据记录。"  BackColor="#B2C9D3" CellPadding="3"
             CellSpacing="1"        Font-Size="9pt" Width="970px" 
            ForeColor="#333333" GridLines="None">
                   <RowStyle BackColor="White" ForeColor="Black" />
                   <FooterStyle BackColor="#006699" Font-Bold="True" />
                   <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
                   <SelectedRowStyle BackColor="LightSkyBlue" Font-Bold="True" ForeColor="#333333" />
                   <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                   <EditRowStyle BackColor="White" />
                   <AlternatingRowStyle BackColor="#E0E0E0" BorderStyle="Solid" ForeColor="Black" />
                   <EmptyDataRowStyle BackColor="White" ForeColor="Red" />
                   <Columns>
                       <asp:BoundField DataField="YP_ID" HeaderText="员工工号" ReadOnly="True" SortExpression="YP_ID" />
                       <asp:BoundField DataField="YP_Name" HeaderText="员工姓名" SortExpression="YP_Name" />
                       <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                   </Columns>
               </asp:GridView>
               <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:YardPlanConnectionString1 %>"
            DeleteCommand="DELETE FROM [YardPeople]&#13;&#10;      WHERE [YP_ID] = @YP_ID" InsertCommand="INSERT INTO [YardPeople]&#13;&#10;       ([YP_ID] ,[YP_Name])&#13;&#10;     VALUES (@YP_ID,@YP_Name)"
            ProviderName="<%$ ConnectionStrings:YardPlanConnectionString1.ProviderName %>"
            SelectCommand="SELECT YP_ID, YP_Name, YP_State FROM YardPeople ORDER BY YP_ID"
            UpdateCommand="UPDATE [YardPeople]&#13;&#10;   SET [YP_Name] = @YP_Name&#13;&#10; WHERE [YP_ID] = @YP_ID">
                   <InsertParameters>
                       <asp:Parameter Name="YP_ID" />
                       <asp:Parameter Name="YP_Name" />
                   </InsertParameters>
                   <UpdateParameters>
                       <asp:Parameter Name="YP_Name" />
                       <asp:Parameter Name="YP_ID" />
                   </UpdateParameters>
                   <DeleteParameters>
                       <asp:Parameter Name="YP_ID" />
                   </DeleteParameters>
               </asp:SqlDataSource>
          </div> 
    
    </div>  
</div>

    </form>
</body>
</html>
