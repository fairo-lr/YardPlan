<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Vessel_Plan.aspx.cs" Inherits="Busy_Plan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>   
       <style id="Style1" type="text/css" runat="server">
  
.fixColleft  
{ } 
.fixColleft1  
  { 
    z-index:100; 
    left:  expression(this.offsetParent.scrollLeft); 
    position:  relative 
  } 
    .fixedHeader { 
      overflow: auto;
      }  

 </style> 
   <script language="javascript" type="text/javascript" >
  function btnCheck(message)
  {
    return confirm(message);
  }
   </script> 
</head>
<body  >
    <form id="form1" runat="server">
    <div>    
        <table cellpadding="0" cellspacing="0" >
    
           <tr>
                <td style="height: 19px">
                  <asp:Label ID="lblWebTitle" runat="server" Font-Size="9pt" Text="Web系统船期计划" Font-Bold="True"></asp:Label>
                    &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;
                    <asp:Button ID="Button3" runat="server" Font-Size="9pt" OnClick="Button3_Click" Text="更新昨日箱量" />&nbsp;
                    <asp:Button ID="Button4" runat="server" OnClick="Button4_Click" Text="更新昨日卸船箱量" />&nbsp;
                    <asp:Button ID="Button5" runat="server" OnClick="Button5_Click" Text="更新昨日装船箱量" />
                    &nbsp;&nbsp;<asp:Button ID="Button6" runat="server" OnClick="Button6_Click" Text="船期确认展示" />&nbsp;
                    <asp:HyperLink ID="hypNewPage" runat="server" Target="_parent">在新窗口编辑</asp:HyperLink></td> 
            
                <td style="text-align: right; height: 19px;">
                    <asp:Label ID="lblWebTime" runat="server" Font-Size="9pt"></asp:Label></td>
           </tr> 
            <tr>
                <td colspan="2">
               
                <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource4" DataKeyNames="vs_id,vc_id,vs_vesselcode,vs_recordtime"
            EmptyDataText="没有可显示的数据记录。"  BackColor="#B2C9D3" CellPadding="3"
             CellSpacing="1"        Font-Size="9pt" 
            ForeColor="#333333" GridLines="None" OnRowCommand="GridView4_RowCommand" OnPreRender="GridView4_PreRender" OnDataBound="GridView4_DataBound" OnRowDataBound="GridView4_RowDataBound">            
            <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="LightSkyBlue" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="White" />
            <AlternatingRowStyle BackColor="#E0E0E0" BorderStyle="Solid" ForeColor="Black" />
            <EmptyDataRowStyle BackColor="White" ForeColor="Red" />
               <Columns>       
                  <asp:TemplateField>
                <HeaderTemplate>
                    <asp:CheckBox ID="cbSelAll" runat="server" Text="全选" AutoPostBack="True" OnCheckedChanged="cbSelAll4_CheckedChanged" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="cbSel4" runat="server" />
                </ItemTemplate>
                      <ItemStyle Width="30px" />
            </asp:TemplateField>  
                <asp:BoundField DataField="vs_id" HeaderText="ID" ReadOnly="True" SortExpression="vs_id" Visible="False" >
                    <ControlStyle Width="40px" />
                </asp:BoundField>
                  <asp:BoundField DataField="vc_id" HeaderText="ID" ReadOnly="True" SortExpression="vs_id" Visible="False" >
                    <ControlStyle Width="40px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_date" HeaderText="日期" SortExpression="vs_date" Visible="False" >
                    <ControlStyle Width="40px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_line" HeaderText="航线" SortExpression="vs_line" ReadOnly="True" >
                    <ControlStyle Width="40px" />
                    <ItemStyle Width="30px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_customer" HeaderText="船东" SortExpression="vs_customer"  ReadOnly="True" Visible="False">
                    <ControlStyle Width="40px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_vesselcname" HeaderText="中文船名" SortExpression="vs_vesselcname"  ReadOnly="True">
                    <ControlStyle Width="60px" />
                    <ItemStyle Width="60px" />
                    <HeaderStyle Width="60px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_vesselename" HeaderText="英文船名" SortExpression="vs_vesselename"  ReadOnly="True">
                    <ControlStyle  Width="60px" />
                    <ItemStyle Width="60px" />
                    <HeaderStyle Width="60px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_vesselcode" HeaderText="船舶ID" SortExpression="vs_vesselcode" Visible="False" >
                    <ControlStyle Width="40px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_ivoyage" HeaderText="进口航次" SortExpression="vs_ivoyage"  ReadOnly="True">
                    <ControlStyle Width="30px" />
                    <ItemStyle Width="30px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_expvoyage" HeaderText="出口航次" SortExpression="vs_expvoyage"  ReadOnly="True">
                    <ControlStyle Width="30px" />
                    <ItemStyle Width="30px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_berthLength" HeaderText="靠泊吃水" SortExpression="vs_berthLength"  ReadOnly="True">
                    <ControlStyle Width="40px" />
                    <ItemStyle Width="30px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_departLength" HeaderText="离泊吃水" SortExpression="vs_departLength"  ReadOnly="True">
                    <ControlStyle Width="40px" />
                    <ItemStyle Width="30px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_proformaETA" HeaderText="预到港" SortExpression="vs_proformaETA"  ReadOnly="True" Visible="False" >
                    <ControlStyle Width="40px" />
                    <ItemStyle Width="30px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_proformaETB" HeaderText="预靠泊" SortExpression="vs_proformaETB"  ReadOnly="True" Visible="False">
                    <ControlStyle Width="40px" />
                    <ItemStyle Width="30px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_proformaETD" HeaderText="预离港" SortExpression="vs_proformaETD"  ReadOnly="True" Visible="False">
                    <ControlStyle Width="40px" />
                    <ItemStyle Width="30px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_forecastETA" HeaderText="确到港" SortExpression="vs_forecastETA"  ReadOnly="True">
                    <ControlStyle Width="40px" />
                    <ItemStyle Width="30px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_forecastETB" HeaderText="确靠泊" SortExpression="vs_forecastETB"  ReadOnly="True">
                    <ControlStyle Width="40px" />
                    <ItemStyle Width="30px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_forecastETD" HeaderText="确离港" SortExpression="vs_forecastETD"  ReadOnly="True">
                    <ControlStyle Width="40px" />
                    <ItemStyle Width="30px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_finshtime" HeaderText="完工时间" SortExpression="vs_finshtime" Visible="False" >
                    <ControlStyle Width="40px" />
                    <ItemStyle Width="30px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_internalfg" HeaderText="内支线" SortExpression="vs_internalfg"  Visible="False">
                    <ControlStyle Width="40px" />
                    <ItemStyle Width="30px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_remark" HeaderText="备注" SortExpression="vs_remark"  Visible="False">
                    <ControlStyle Width="40px" />
                    <ItemStyle Width="30px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_longpole" HeaderText="长杆" SortExpression="vs_longpole" >
                    <ControlStyle Width="40px" />
                    <ItemStyle Width="20px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_worknum" HeaderText="作业数" SortExpression="vs_worknum" >
                    <ControlStyle Width="40px" />
                    <ItemStyle Width="20px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_vesseltime" HeaderText="预计船时" SortExpression="vs_vesseltime" >
                    <ControlStyle Width="40px" />
                    <ItemStyle Width="20px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_gold" HeaderText="目标" SortExpression="vs_gold" >
                    <ControlStyle Width="40px" />
                    <ItemStyle Width="20px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_finsh" HeaderText="标志" SortExpression="vs_finsh" >
                    <ControlStyle Width="40px" />
                    <ItemStyle Width="20px" />
                </asp:BoundField>
                <asp:BoundField DataField="vs_recordtime" HeaderText="记录时间" SortExpression="vs_recordtime" Visible="False" >
                    <ControlStyle Width="40px" />
                </asp:BoundField>       
                <asp:BoundField DataField="vc_id" HeaderText="ID" ReadOnly="True" SortExpression="vc_id" Visible="False" />
                <asp:BoundField DataField="vc_date" HeaderText="日期" SortExpression="vc_date" Visible="False" >
                    <ControlStyle Width="40px" />
                </asp:BoundField>
                <asp:BoundField DataField="vc_vesselename" HeaderText="英文船名" SortExpression="vc_vesselename" Visible="False" >
                    <ControlStyle Width="60px" />
                    <ItemStyle Width="60px" />
                    <HeaderStyle Width="60px" />
                </asp:BoundField>
                <asp:BoundField DataField="vc_vesselcname" HeaderText="中文船名" SortExpression="vc_vesselcname" Visible="False" >
                    <ControlStyle Width="60px" />
                    <ItemStyle Width="60px" />
                    <HeaderStyle Width="60px" />
                </asp:BoundField>
                <asp:BoundField DataField="vc_vesselcode" HeaderText="船舶ID" SortExpression="vc_vesselcode" Visible="False" >
                    <ControlStyle Width="40px" />
                </asp:BoundField>
                <asp:BoundField DataField="vc_ivoyage" HeaderText="进口航次" SortExpression="vc_ivoyage" Visible="False" >
                    <ControlStyle Width="40px" />
                </asp:BoundField>
                <asp:BoundField DataField="vc_expvoyage" HeaderText="出口航次" SortExpression="vc_expvoyage" Visible="False" >
                    <ControlStyle Width="40px" />
                </asp:BoundField>
                <asp:BoundField DataField="vc_cntrowner" HeaderText="箱主" SortExpression="vc_cntrowner" >
                    <ControlStyle Width="40px" />
                    <ItemStyle Width="30px" />
                </asp:BoundField>
                <asp:BoundField DataField="vc_20IF" HeaderText="20卸重" SortExpression="vc_20IF" >
                    <ControlStyle Width="40px"  BorderColor="Yellow" BorderStyle="Solid" BorderWidth="1px"/>
                    <ItemStyle Width="20px"  />
                </asp:BoundField>
                <asp:BoundField DataField="vc_40IF" HeaderText="40卸重" SortExpression="vc_40IF" >
                    <ControlStyle Width="40px"  BorderColor="Yellow" BorderStyle="Solid" BorderWidth="1px" />
                    <ItemStyle Width="20px" />
                </asp:BoundField>
                <asp:BoundField DataField="vc_45IF" HeaderText="45卸重" SortExpression="vc_45IF" >
                    <ControlStyle Width="40px" BorderColor="Yellow" BorderStyle="Solid" BorderWidth="1px" />
                    <ItemStyle Width="20px" />
                </asp:BoundField>
                <asp:BoundField DataField="vc_20IE" HeaderText="20卸空" SortExpression="vc_20IE" >
                    <ControlStyle Width="40px" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px"/>
                    <ItemStyle Width="20px"  />
                </asp:BoundField>
                <asp:BoundField DataField="vc_40IE" HeaderText="40卸空" SortExpression="vc_40IE" >
                    <ControlStyle Width="40px"  BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px"/>
                    <ItemStyle Width="20px" />
                </asp:BoundField>
                <asp:BoundField DataField="vc_45IE" HeaderText="45卸空" SortExpression="vc_45IE" >
                    <ControlStyle Width="40px" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px"/>
                    <ItemStyle Width="20px"  />
                </asp:BoundField>
                <asp:BoundField DataField="vc_20OF" HeaderText="20装重" SortExpression="vc_20OF" >
                    <ControlStyle Width="40px"  BorderColor="Green" BorderStyle="Solid" BorderWidth="1px"/>
                    <ItemStyle Width="20px" />
                </asp:BoundField>
                <asp:BoundField DataField="vc_40OF" HeaderText="40装重" SortExpression="vc_40OF" >
                    <ControlStyle Width="40px" BorderColor="Green" BorderStyle="Solid" BorderWidth="1px"/>
                    <ItemStyle Width="20px" />
                </asp:BoundField>
                <asp:BoundField DataField="vc_45OF" HeaderText="45装重" SortExpression="vc_45OF" >
                    <ControlStyle Width="40px" BorderColor="Green" BorderStyle="Solid" BorderWidth="1px"/>
                    <ItemStyle Width="20px" />
                </asp:BoundField>
                <asp:BoundField DataField="vc_20OE" HeaderText="20装空" SortExpression="vc_20OE" >
                    <ControlStyle Width="40px" BorderColor="Red" BorderStyle="Solid" BorderWidth="1px" />
                    <ItemStyle Width="20px" />
                </asp:BoundField>
                <asp:BoundField DataField="vc_40OE" HeaderText="40装空" SortExpression="vc_40OE" >
                    <ControlStyle Width="40px" BorderColor="Red" BorderStyle="Solid" BorderWidth="1px"  />
                    <ItemStyle Width="20px" />
                </asp:BoundField>
                <asp:BoundField DataField="vc_45OE" HeaderText="45装空" SortExpression="vc_45OE" >
                    <ControlStyle Width="40px" BorderColor="Red" BorderStyle="Solid" BorderWidth="1px" />
                    <ItemStyle Width="20px" />
                </asp:BoundField>
                <asp:BoundField DataField="vc_sum" HeaderText="小计" SortExpression="vc_sum" InsertVisible="False" >
                    <ControlStyle Width="40px" />
                    <ItemStyle Width="20px" />
                </asp:BoundField>
                <asp:BoundField DataField="vc_recordtime" HeaderText="vc_recordtime" SortExpression="vc_recordtime" Visible="False" >
                    <ControlStyle Height="40px" />
                </asp:BoundField>
                <asp:ButtonField CommandName="Edit" Text="编辑" ButtonType="Button" />
                  <asp:ButtonField CommandName="btnVessel" Text="更新船舶" Visible="False" ButtonType="Button" />                     
                  <asp:ButtonField CommandName="btnContainer" Text="更新箱量" Visible="False" ButtonType="Button" />                                                                            
                  <asp:ButtonField CommandName="Cancel" Text="取消"  Visible="False" ButtonType="Button"/>
                  <asp:ButtonField CommandName="btnDeleteVessel" Text="删除" ButtonType="Button" />
            </Columns>
             <FooterStyle BackColor="#006699" Font-Bold="True" />
            <RowStyle BackColor="White" ForeColor="Black" />
        </asp:GridView>
             <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:YardPlanConnectionString1 %>"
            SelectCommand="SELECT vs_id, vs_date, vs_line, vs_customer, vs_vesselename, vs_vesselcname, vs_vesselcode, vs_ivoyage, vs_expvoyage,vs_berthLength,vs_departLength,replace(datename(dw,vs_proformaETA),'星期','周')+'/'+REPLACE(convert(varchar(5),vs_proformaETA,114),':','') vs_proformaETA,replace(datename(dw,vs_proformaETB),'星期','周')+'/'+REPLACE(convert(varchar(5),vs_proformaETB,114),':','') vs_proformaETB,replace(datename(dw,vs_proformaETD),'星期','周')+'/'+REPLACE(convert(varchar(5),vs_proformaETD,114),':','') vs_proformaETD,replace(datename(dd,vs_forecastETA),'星期','周')+'/'+REPLACE(convert(varchar(5),vs_forecastETA,114),':','') vs_forecastETA,replace(datename(dd,vs_forecastETB),'星期','周')+'/'+REPLACE(convert(varchar(5),vs_forecastETB,114),':','') vs_forecastETB,replace(datename(dd,vs_forecastETD),'星期','周')+'/'+REPLACE(convert(varchar(5),vs_forecastETD,114),':','') vs_forecastETD,convert(varchar(16),vs_finshtime,120) vs_finshtime,vs_remark, vs_longpole, vs_worknum, vs_vesseltime, vs_gold, vs_finsh,convert(varchar(16), vs_recordtime,120) vs_recordtime, VC_id, vc_date, vc_vesselename, vc_vesselcname, vc_vesselcode, vc_ivoyage, vc_expvoyage, vc_cntrowner, vc_20IF, vc_20IE, vc_20OF, vc_20OE, vc_40IF, vc_40IE, vc_40OF, vc_40OE, vc_45IF, vc_45IE, vc_45OF, vc_45OE, vc_sum, vc_recordtime, vs_internalfg,vs_intrade FROM fullVesselWeek_vw WHERE (vs_date = @date) order by vs_intrade,vs_internalfg,vs_forecastETA asc"
           DeleteCommand="DELETE FROM [Vessel] WHERE [vs_id] = @vs_id DELETE FROM [VesselContainer] WHERE [vc_id] = @vc_id"
            InsertCommand="INSERT INTO [Vessel] ([vs_date], [vs_line], [vs_customer], [vs_vesselename], [vs_vesselcname], [vs_vesselcode], [vs_ivoyage], [vs_expvoyage], [vs_proformaETA], [vs_proformaETB], [vs_proformaETD], [vs_forecastETA], [vs_forecastETB], [vs_forecastETD], [vs_finshtime], [vs_remark], [vs_longpole], [vs_worknum], [vs_vesseltime], [vs_gold], [vs_finsh], [vs_recordtime]) VALUES (@vs_date, @vs_line, @vs_customer, @vs_vesselename, @vs_vesselcname, @vs_vesselcode, @vs_ivoyage, @vs_expvoyage, @vs_proformaETA, @vs_proformaETB, @vs_proformaETD, @vs_forecastETA, @vs_forecastETB, @vs_forecastETD, @vs_finshtime, @vs_remark, @vs_longpole, @vs_worknum, @vs_vesseltime, @vs_gold, @vs_finsh, @vs_recordtime);INSERT INTO [VesselContainer] ([vc_date], [vc_vesselename], [vc_vesselcname], [vc_vesselcode], [vc_ivoyage], [vc_expvoyage], [vc_cntrowner], [vc_20IF], [vc_20IE], [vc_20OF], [vc_20OE], [vc_40IF], [vc_40IE], [vc_40OF], [vc_40OE], [vc_45IF], [vc_45IE], [vc_45OF], [vc_45OE], [vc_sum], [vc_recordtime]) VALUES (@vc_date, @vc_vesselename, @vc_vesselcname, @vc_vesselcode, @vc_ivoyage, @vc_expvoyage, @vc_cntrowner, @vc_20IF, @vc_20IE, @vc_20OF, @vc_20OE, @vc_40IF, @vc_40IE, @vc_40OF, @vc_40OE, @vc_45IF, @vc_45IE, @vc_45OF, @vc_45OE, @vc_sum, @vc_recordtime)"
            ProviderName="<%$ ConnectionStrings:YardPlanConnectionString1.ProviderName %>"
             UpdateCommand ="INSERT INTO [VesselContainer] ([vc_date], [vc_vesselename], [vc_vesselcname], [vc_vesselcode], [vc_ivoyage], [vc_expvoyage], [vc_cntrowner], [vc_20IF], [vc_20IE], [vc_20OF], [vc_20OE], [vc_40IF], [vc_40IE], [vc_40OF], [vc_40OE], [vc_45IF], [vc_45IE], [vc_45OF], [vc_45OE], [vc_sum], [vc_recordtime]) VALUES ('2013-02-18', @vs_vesselename, @vs_vesselcname, @vs_vesselcode, @vs_ivoyage, @vs_expvoyage, @vc_cntrowner, @vc_20IF, @vc_20IE, @vc_20OF, @vc_20OE, @vc_40IF, @vc_40IE, @vc_40OF, @vc_40OE, @vc_45IF, @vc_45IE, @vc_45OF, @vc_45OE, @vc_sum, @vc_recordtime)" OnSelected="SqlDataSource4_Selected" >
           <SelectParameters>
                <asp:Parameter Name="date" Type="DateTime" /> 
           </SelectParameters> 
            <InsertParameters>
                <asp:Parameter Name="vs_date" Type="DateTime" />
                <asp:Parameter Name="vs_line" Type="String" />
                <asp:Parameter Name="vs_customer" Type="String" />
                <asp:Parameter Name="vs_vesselename" Type="String" />
                <asp:Parameter Name="vs_vesselcname" Type="String" />
                <asp:Parameter Name="vs_vesselcode" Type="String" />
                <asp:Parameter Name="vs_ivoyage" Type="String" />
                <asp:Parameter Name="vs_expvoyage" Type="String" />
                <asp:Parameter Name="vs_proformaETA" Type="DateTime" />
                <asp:Parameter Name="vs_proformaETB" Type="DateTime" />
                <asp:Parameter Name="vs_proformaETD" Type="DateTime" />
                <asp:Parameter Name="vs_forecastETA" Type="DateTime" />
                <asp:Parameter Name="vs_forecastETB" Type="DateTime" />
                <asp:Parameter Name="vs_forecastETD" Type="DateTime" />
                <asp:Parameter Name="vs_finshtime" Type="DateTime" />
                <asp:Parameter Name="vs_remark" Type="String" />
                <asp:Parameter Name="vs_longpole" Type="Int32" />
                <asp:Parameter Name="vs_worknum" Type="String" />
                <asp:Parameter Name="vs_vesseltime" Type="Int32" />
                <asp:Parameter Name="vs_gold" Type="String" />
                <asp:Parameter Name="vs_finsh" Type="String" />
                <asp:Parameter Name="vs_recordtime" Type="DateTime" />
                <asp:Parameter Name="vc_date" Type="DateTime" />
                <asp:Parameter Name="vc_vesselename" Type="String" />
                <asp:Parameter Name="vc_vesselcname" Type="String" />
                <asp:Parameter Name="vc_vesselcode" Type="String" />
                <asp:Parameter Name="vc_ivoyage" Type="String" />
                <asp:Parameter Name="vc_expvoyage" Type="String" />
                <asp:Parameter Name="vc_cntrowner" Type="String" />
                <asp:Parameter Name="vc_20IF" Type="Int32" />
                <asp:Parameter Name="vc_20IE" Type="Int32" />
                <asp:Parameter Name="vc_20OF" Type="Int32" />
                <asp:Parameter Name="vc_20OE" Type="Int32" />
                <asp:Parameter Name="vc_40IF" Type="Int32" />
                <asp:Parameter Name="vc_40IE" Type="Int32" />
                <asp:Parameter Name="vc_40OF" Type="Int32" />
                <asp:Parameter Name="vc_40OE" Type="Int32" />
                <asp:Parameter Name="vc_45IF" Type="Int32" />
                <asp:Parameter Name="vc_45IE" Type="Int32" />
                <asp:Parameter Name="vc_45OF" Type="Int32" />
                <asp:Parameter Name="vc_45OE" Type="Int32" />
                <asp:Parameter Name="vc_sum" Type="Int32" />
                <asp:Parameter Name="vc_recordtime" Type="DateTime" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="vs_line" Type="String" />
                <asp:Parameter Name="vs_customer" Type="String" />
                <asp:Parameter Name="vs_vesselename" Type="String" />
                <asp:Parameter Name="vs_vesselcname" Type="String" />
                <asp:Parameter Name="vs_vesselcode" Type="String" />
                <asp:Parameter Name="vs_ivoyage" Type="String" />
                <asp:Parameter Name="vs_expvoyage" Type="String" />
                <asp:Parameter Name="vs_proformaETA" Type="DateTime" />
                <asp:Parameter Name="vs_proformaETB" Type="DateTime" />
                <asp:Parameter Name="vs_proformaETD" Type="DateTime" />
                <asp:Parameter Name="vs_forecastETA" Type="DateTime" />
                <asp:Parameter Name="vs_forecastETB" Type="DateTime" />
                <asp:Parameter Name="vs_forecastETD" Type="DateTime" />
                <asp:Parameter Name="vs_finshtime" Type="DateTime" />
                <asp:Parameter Name="vs_remark" Type="String" />
                <asp:Parameter Name="vs_longpole" Type="Int32" />
                <asp:Parameter Name="vs_worknum" Type="String" />
                <asp:Parameter Name="vs_vesseltime" Type="String" />
                <asp:Parameter Name="vs_gold" Type="String" />
                <asp:Parameter Name="vs_finsh" Type="String" />
                <asp:Parameter Name="vs_recordtime" Type="DateTime" />
                <asp:Parameter Name="vs_id" Type="Int32" />
                <asp:Parameter Name="vc_vesselename" Type="String" />
                <asp:Parameter Name="vc_vesselcname" Type="String" />
                <asp:Parameter Name="vc_vesselcode" Type="String" />
                <asp:Parameter Name="vc_ivoyage" Type="String" />
                <asp:Parameter Name="vc_expvoyage" Type="String" />
                <asp:Parameter Name="vc_cntrowner" Type="String" />
                <asp:Parameter Name="vc_20IF" Type="Int32" />
                <asp:Parameter Name="vc_20IE" Type="Int32" />
                <asp:Parameter Name="vc_20OF" Type="Int32" />
                <asp:Parameter Name="vc_20OE" Type="Int32" />
                <asp:Parameter Name="vc_40IF" Type="Int32" />
                <asp:Parameter Name="vc_40IE" Type="Int32" />
                <asp:Parameter Name="vc_40OF" Type="Int32" />
                <asp:Parameter Name="vc_40OE" Type="Int32" />
                <asp:Parameter Name="vc_45IF" Type="Int32" />
                <asp:Parameter Name="vc_45IE" Type="Int32" />
                <asp:Parameter Name="vc_45OF" Type="Int32" />
                <asp:Parameter Name="vc_45OE" Type="Int32" />
                <asp:Parameter Name="vc_sum" Type="Int32" />
                <asp:Parameter Name="vc_recordtime" Type="DateTime" />
                <asp:Parameter Name="vc_id" Type="Int32" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Name="vs_id" Type="Int32" />
                <asp:Parameter Name="vc_id" Type="Int32" />
            </DeleteParameters>
        </asp:SqlDataSource> 
                </td>
            </tr>
                    <tr>
              <td colspan="2">   <br />
                    &nbsp;&nbsp;
        <asp:Button ID="Button1" runat="server" Text="生成周船期"      OnClientClick="return btnCheck('选中的记录如果有记录，将覆盖，是否确定')" OnClick="Button1_Click" />
                    &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="Button2" runat="server" Text="更新周箱量" OnClientClick="return btnCheck('选中的记录如果有记录，将覆盖，是否确定')" OnClick="Button2_Click" /><br /> 
              </td>
           </tr>  
            <tr>
                <td style="height: 19px">
                    <asp:Label ID="lblTopsTitle" runat="server" Font-Size="9pt" Text="生产系统船期计划" Font-Bold="True"></asp:Label></td> 
                <td style="height: 19px; text-align: right">
                    <asp:Label ID="lblTopsTime" runat="server" Font-Size="9pt"></asp:Label></td>
            </tr>
            <tr>
                <td  colspan="2">                
            <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" 
                DataSourceID="SqlDataSource3" EmptyDataText="没有可显示的数据记录。"  BackColor="#B2C9D3" CellPadding="3"
             CellSpacing="1"        Font-Size="9pt" DataKeyNames="OCRRID"
            ForeColor="#333333" GridLines="None" OnPreRender="GridView3_PreRender">
                <FooterStyle BackColor="#006699" Font-Bold="True" />
                <EmptyDataRowStyle BackColor="White" ForeColor="Red" />
                <RowStyle BackColor="White" ForeColor="Black" />
                <EditRowStyle BackColor="White" />
                <SelectedRowStyle BackColor="LightSkyBlue" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="#E0E0E0" BorderStyle="Solid" ForeColor="Black" />
                <Columns>
             <asp:TemplateField>
                <HeaderTemplate>
                    <asp:CheckBox ID="cbSelAll" runat="server" Text="全选" AutoPostBack="True" OnCheckedChanged="cbSelAll3_CheckedChanged" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="cbSel3" runat="server" />
                </ItemTemplate>
                 <ItemStyle Width="30px" />
            </asp:TemplateField> 
                <asp:BoundField DataField="OCRRID" HeaderText="OCRRID" SortExpression="OCRRID" Visible="False"/>
                    <asp:BoundField DataField="line" HeaderText="航线" SortExpression="line" />
                    <asp:BoundField DataField="cstmcd" HeaderText="船东" SortExpression="cstmcd" />
                    <asp:BoundField DataField="中文船名" HeaderText="船名" SortExpression="中文船名" />
                    <asp:BoundField DataField="航次" HeaderText="航次" SortExpression="航次" />
                   <asp:BoundField DataField="吃水" HeaderText="吃水" SortExpression="吃水" /> 
                    <asp:BoundField DataField="ETA" HeaderText="ETA" SortExpression="ETA" >
                        <ItemStyle Width="60px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ETB" HeaderText="ETB" SortExpression="ETB" >
                        <ItemStyle Width="60px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ETD" HeaderText="ETD" SortExpression="ETD" >
                        <ItemStyle Width="60px" />
                    </asp:BoundField>                   
                    <asp:BoundField DataField="箱主" HeaderText="箱主" SortExpression="箱主" />
                    <asp:BoundField DataField="卸船重箱(20\40\45)" HeaderText="卸船重箱(20\40\45)" SortExpression="卸船重箱(20\40\45)" />
                    <asp:BoundField DataField="卸船空箱(20\40\45)" HeaderText="卸船空箱(20\40\45)" SortExpression="卸船空箱(20\40\45)" />
                    <asp:BoundField DataField="装船重箱(20\40\45)" HeaderText="装船重箱(20\40\45)" SortExpression="装船重箱(20\40\45)" />
                    <asp:BoundField DataField="装船空箱(20\40\45)" HeaderText="装船空箱(20\40\45)" SortExpression="装船空箱(20\40\45)" />
                    <asp:BoundField DataField="总量" HeaderText="总量" SortExpression="总量" />
                   <asp:BoundField DataField="gcranenum" HeaderText="作业数" SortExpression="gcranenum" /> 
                </Columns>
            
            </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:YardPlanConnectionString1 %>"
                ProviderName="<%$ ConnectionStrings:YardPlanConnectionString1.ProviderName %>" SelectCommand="SELECT WP.OCRRID, WP.line, WP.cstmcd, WP.ename + ' / ' + WP.cname AS 中文船名, WP.ivoyage + ' / ' + WP.expvoyage AS 航次,convert(varchar,berthLength)+'/'+convert(varchar,departLength) 吃水, /*REPLACE(DATENAME(dw, WP.PROETA), '星期', '周') + '/' +replace(convert(varchar(5), WP.PROETA,114),':','')   +	 ',' +*/ REPLACE(DATENAME(dd, WP.FORETA), '星期', '周')  + '/'+replace(convert(varchar(5), WP.FORETA,114),':','')  AS ETA, /*REPLACE(DATENAME(dw, WP.PROETB), '星期', '周') + '/' + replace(convert(varchar(5), WP.PROETB,114),':','')  + 	',' +*/ REPLACE(DATENAME(dd, WP.FORETB), '星期', '周')  + '/'+ replace(convert(varchar(5), WP.FORETB,114),':','')     AS ETB, /*REPLACE(DATENAME(dw, WP.PROETD), '星期', '周') + '/' + replace(convert(varchar(5), WP.PROETD,114),':','')    + 	',' +*/ REPLACE(DATENAME(dd, WP.FORETD), '星期', '周')  + '/'+ replace(convert(varchar(5), WP.FORETD,114),':','')    AS ETD,gcranenum,WC.copercd AS 箱主, CONVERT (VARCHAR, ISNULL(WC.IF20, 0)) + '\' + CONVERT (VARCHAR, ISNULL(WC.IF40, 0)) + '\' + CONVERT (VARCHAR, ISNULL(WC.IF45, 0)) AS '卸船重箱(20\40\45)', CONVERT (VARCHAR, ISNULL(WC.IE20, 0)) + '\' + CONVERT (VARCHAR, ISNULL(WC.IE40, 0)) + '\' + CONVERT (VARCHAR, ISNULL(WC.IE45, 0)) AS '卸船空箱(20\40\45)', CONVERT (VARCHAR, ISNULL(WC.OF20, 0)) + '\' + CONVERT (VARCHAR, ISNULL(WC.OF40, 0)) + '\' + CONVERT (VARCHAR, ISNULL(WC.OF45, 0)) AS '装船重箱(20\40\45)', CONVERT (VARCHAR, ISNULL(WC.OE20, 0)) + '\' + CONVERT (VARCHAR, ISNULL(WC.OE40, 0)) + '\' + CONVERT (VARCHAR, ISNULL(WC.OE45, 0)) AS '装船空箱(20\40\45)', ISNULL(WC.IF20, 0) + ISNULL(WC.IE20, 0) + ISNULL(WC.OF20, 0) + ISNULL(WC.OE20, 0) + ISNULL(WC.IF40, 0) + ISNULL(WC.IE40, 0) + ISNULL(WC.OF40, 0) + ISNULL(WC.OE40, 0) + ISNULL(WC.IF45, 0) + ISNULL(WC.IE45, 0) + ISNULL(WC.OF45, 0) + ISNULL(WC.OE45, 0) AS 总量 FROM ora_WeekPlan_VW AS WP LEFT OUTER JOIN ora_WeekContainer_VW AS WC ON WP.OCRRID = WC.ocrrid ORDER BY wp.intrade,WP.internalfg,wp.FORETA asc">
            </asp:SqlDataSource>                 
                </td>                
            </tr>
      
        </table><div>
        &nbsp; <asp:Label ID="Label2" runat="server" Font-Size="9pt" ForeColor="Red" Text="字体红色表示：抵港时间在两天之内！"></asp:Label><br />
        &nbsp;<asp:Label ID="Label1" runat="server" Text="背景蓝色代表由于时间的更新照成Web数据和生产不一致的内容！" Font-Size="9pt"  ForeColor="Red"></asp:Label><br /></div>
       </div>     
   
    </form>
</body>
</html>