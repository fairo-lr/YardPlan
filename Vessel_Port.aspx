<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Vessel_Port.aspx.cs" Inherits="Busy_Vessel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>件杂货</title>

    <script language="javascript" type="text/javascript" src="My97DatePicker/WdatePicker.js">
   function setStartTime()
   { 
       $('iptStartTime').value = $dp.cal.getDateStr();  
        window.location="Vessel_NewReport.aspx?date="+$('iptStartTime').value;
   } 
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
   <div>
       <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" BackColor="#B2C9D3"
           CellPadding="3" CellSpacing="1" DataKeyNames="lw_id" DataSourceID="SqlDataSource1"
           DefaultMode="Insert" Font-Size="9pt" GridLines="None" Height="50px" ToolTip="靠泊长度输入整数"
           Width="551px" OnItemInserting="DetailsView1_ItemInserting" OnItemInserted="DetailsView1_ItemInserted">
           <RowStyle BackColor="White" ForeColor="Black" />
           <FooterStyle BackColor="#006699" Font-Bold="True" />
           <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
           <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
           <EditRowStyle BackColor="White" />
           <AlternatingRowStyle BackColor="#E0E0E0" BorderStyle="Solid" ForeColor="Black" />
       <Fields>
               <asp:BoundField DataField="vp_id" HeaderText="vp_id" InsertVisible="False" ReadOnly="True"
                   SortExpression="vp_id" >
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
               <asp:BoundField DataField="vp_date" HeaderText="日期" SortExpression="vp_date" Visible="False" >
                    <ItemStyle BackColor="#E0E0E0" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle BackColor="#E0E0E0" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
               <asp:BoundField DataField="vp_vesselcode" HeaderText="船舶ID" SortExpression="vp_vesselcode"  Visible="false">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
               <asp:BoundField DataField="vp_chname" HeaderText="中文船名" SortExpression="vp_chname" >
                    <ItemStyle BackColor="#E0E0E0" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle BackColor="#E0E0E0" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
               <asp:BoundField DataField="vp_ename" HeaderText="英文船名" SortExpression="vp_ename" >
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
               <asp:BoundField DataField="vp_startport" HeaderText="起始码" SortExpression="vp_startport" >
                    <ItemStyle BackColor="#E0E0E0" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle BackColor="#E0E0E0" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="vp_endport" HeaderText="结束码" SortExpression="vp_endport" >
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="vp_width" HeaderText="船宽" SortExpression="vp_width" >
                    <ItemStyle BackColor="#E0E0E0" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle BackColor="#E0E0E0" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
               <asp:BoundField DataField="vp_length" HeaderText="船长" SortExpression="vp_length" >
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
               <asp:BoundField DataField="vp_ivoyage" HeaderText="进口航次" SortExpression="vp_ivoyage" >
                    <ItemStyle BackColor="#E0E0E0" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle BackColor="#E0E0E0" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
               <asp:BoundField DataField="vp_expvoyage" HeaderText="出口航次" SortExpression="vp_expvoyage" >
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
           <asp:TemplateField HeaderText="靠泊时间" SortExpression="vp_betrthtime">
               <EditItemTemplate>
                   <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("vp_betrthtime") %>'></asp:TextBox>
               </EditItemTemplate>
               <InsertItemTemplate>
                                   <input id="iptBerthTime" name="StartTime" class="Wdate" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" runat="server" /> 
                   
               </InsertItemTemplate>
               <ItemStyle BackColor="#E0E0E0" HorizontalAlign="Left" VerticalAlign="Middle" />
               <HeaderStyle BackColor="#E0E0E0" HorizontalAlign="Right" VerticalAlign="Middle" />
               <ItemTemplate>
                   <asp:Label ID="Label2" runat="server" Text='<%# Bind("vp_betrthtime") %>'></asp:Label>
               </ItemTemplate>
           </asp:TemplateField>
           <asp:TemplateField HeaderText="离泊时间" SortExpression="vp_departtime">
               <EditItemTemplate>
                   <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("vp_departtime") %>'></asp:TextBox>
               </EditItemTemplate>
               <InsertItemTemplate>
                          <input id="iptDepartTime" name="StartTime" class="Wdate" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" runat="server" /> 
               
               </InsertItemTemplate>
               <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
               <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
               <ItemTemplate>
                   <asp:Label ID="Label3" runat="server" Text='<%# Bind("vp_departtime") %>'></asp:Label>
               </ItemTemplate>
           </asp:TemplateField>
               <asp:BoundField DataField="vp_prePort" HeaderText="上一港" SortExpression="vp_prePort" >
                    <ItemStyle BackColor="#E0E0E0" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle BackColor="#E0E0E0" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
               <asp:BoundField DataField="vp_nextPort" HeaderText="下一港" SortExpression="vp_nextPort" >
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
           <asp:TemplateField HeaderText="靠泊方向" SortExpression="vp_berthside">
               <EditItemTemplate>
                   <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("vp_berthside") %>'></asp:TextBox>
               </EditItemTemplate>
               <InsertItemTemplate>
                        <asp:DropDownList ID="ddlSide" runat="server" Width="150px" >
                                <asp:ListItem Value="L" Selected="true" Text="L">  </asp:ListItem> 
                                <asp:ListItem Value="R" Text="R"></asp:ListItem>
                         </asp:DropDownList>                     
               </InsertItemTemplate>
               <ItemStyle BackColor="#E0E0E0" HorizontalAlign="Left" VerticalAlign="Middle" />
               <HeaderStyle BackColor="#E0E0E0" HorizontalAlign="Right" VerticalAlign="Middle" />
               <ItemTemplate>
                   <asp:Label ID="Label1" runat="server" Text='<%# Bind("vp_berthside") %>'></asp:Label>
               </ItemTemplate>
           </asp:TemplateField>
               <asp:BoundField DataField="vp_recordtime" HeaderText="记录时间" SortExpression="vp_recordtime" Visible="False" >
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                 <asp:CommandField ButtonType="Button" ShowInsertButton="True">
                    <FooterStyle HorizontalAlign="Center" />
                    <ItemStyle BackColor="#006699" HorizontalAlign="Center" />
                </asp:CommandField>
           </Fields>
       </asp:DetailsView>
       <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="添加昨日件杂货" />
       <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" 
             DataKeyNames="vp_id" AutoGenerateColumns="False" DataSourceID="SqlDataSource1"
           EmptyDataText="没有可显示的数据记录。"  BackColor="#B2C9D3" CellPadding="3"
             CellSpacing="1"        Font-Size="9pt" 
            ForeColor="#333333" GridLines="None" OnRowCommand="GridView1_RowCommand">            
            <RowStyle BackColor="White" ForeColor="Black" />
            <FooterStyle BackColor="#006699" Font-Bold="True" />
            <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="LightSkyBlue" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="White" />
            <AlternatingRowStyle BackColor="#E0E0E0" BorderStyle="Solid" ForeColor="Black" />
            <EmptyDataRowStyle BackColor="White" ForeColor="Red" />
           <Columns>
               <asp:BoundField DataField="vp_id" HeaderText="ID" InsertVisible="True" ReadOnly="True"
                   SortExpression="vp_id" Visible="False" />
               <asp:BoundField DataField="vp_date" HeaderText="日期" SortExpression="vp_date" InsertVisible="False" Visible="False" />
               <asp:BoundField DataField="vp_vesselcode" HeaderText="船舶ID" SortExpression="vp_vesselcode" >
                   <ControlStyle Width="50px" />
               </asp:BoundField>
               <asp:BoundField DataField="vp_chname" HeaderText="中文船名" SortExpression="vp_chname" >
                   <ControlStyle Width="100px" />
               </asp:BoundField>
               <asp:BoundField DataField="vp_ename" HeaderText="英文船名" SortExpression="vp_ename" >
                   <ControlStyle Width="100px" />
               </asp:BoundField>
               <asp:BoundField DataField="vp_startport" HeaderText="起始码" SortExpression="vp_startport" >
                   <ControlStyle Width="50px" />
               </asp:BoundField>
                <asp:BoundField DataField="vp_endport" HeaderText="结束码" SortExpression="vp_endport" >
                    <ControlStyle Width="50px" />
                </asp:BoundField>
               <asp:BoundField DataField="vp_width" HeaderText="船宽" SortExpression="vp_width" >
                   <ControlStyle Width="50px" />
               </asp:BoundField> 
               <asp:BoundField DataField="vp_length" HeaderText="船长" SortExpression="vp_length" >
                   <ControlStyle Width="50px" />
               </asp:BoundField>
               <asp:BoundField DataField="vp_ivoyage" HeaderText="进口航次" SortExpression="vp_ivoyage" >
                   <ControlStyle Width="50px" />
               </asp:BoundField>
               <asp:BoundField DataField="vp_expvoyage" HeaderText="出口航次" SortExpression="vp_expvoyage" >
                   <ControlStyle Width="50px" />
               </asp:BoundField>
               <asp:BoundField DataField="vp_betrthtime" HeaderText="靠泊时间" SortExpression="vp_betrthtime" >
                   <ControlStyle Width="100px" />
               </asp:BoundField>
               <asp:BoundField DataField="vp_departtime" HeaderText="离泊时间" SortExpression="vp_departtime" >
                   <ControlStyle Width="100px" />
               </asp:BoundField>
               <asp:BoundField DataField="vp_prePort" HeaderText="上一港" SortExpression="vp_prePort" >
                   <ControlStyle Width="50px" />
               </asp:BoundField>
               <asp:BoundField DataField="vp_nextPort" HeaderText="下一港" SortExpression="vp_nextPort" >
                   <ControlStyle Width="50px" />
               </asp:BoundField>
               <asp:BoundField DataField="vp_berthside" HeaderText="靠泊方向" SortExpression="vp_berthside" >
                   <ControlStyle Width="50px" />
               </asp:BoundField>
               <asp:BoundField DataField="vp_recordtime" HeaderText="登记时间" SortExpression="vp_recordtime" InsertVisible="False" Visible="False" />
               <asp:CommandField ShowEditButton="True" ButtonType="Button" />
               <asp:CommandField ShowDeleteButton="True" ButtonType="Button" />
           </Columns>
       </asp:GridView>
       <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:YardPlanConnectionString1 %>"
           DeleteCommand="DELETE FROM [VesselPort] WHERE [vp_id] = @vp_id" 
           InsertCommand="INSERT INTO VesselPort(vp_date, vp_length, vp_startport, vp_vesselcode, vp_chname, vp_ename, vp_ivoyage, vp_expvoyage, vp_betrthtime, vp_departtime, vp_prePort, vp_nextPort, vp_berthside, vp_recordtime, vp_width, vp_endport) VALUES (@vp_date, @vp_length, @vp_startport, @vp_vesselcode, @vp_chname, @vp_ename, @vp_ivoyage, @vp_expvoyage, @vp_betrthtime, @vp_departtime, @vp_prePort, @vp_nextPort, @vp_berthside, GETDATE(), @vp_width, @vp_endport)"
           ProviderName="<%$ ConnectionStrings:YardPlanConnectionString1.ProviderName %>"
           SelectCommand="SELECT vp_id, vp_date, vp_length, vp_startport, vp_vesselcode, vp_chname, vp_ename, vp_ivoyage, vp_expvoyage, vp_betrthtime, vp_departtime, vp_prePort, vp_nextPort, vp_berthside, vp_recordtime, vp_width, vp_endport FROM VesselPort WHERE (vp_date = @date)"
           UpdateCommand="UPDATE VesselPort SET vp_length = @vp_length, vp_startport = @vp_startport, vp_vesselcode = @vp_vesselcode, vp_chname = @vp_chname, vp_ename = @vp_ename, vp_ivoyage = @vp_ivoyage, vp_expvoyage = @vp_expvoyage, vp_betrthtime = @vp_betrthtime, vp_departtime = @vp_departtime, vp_prePort = @vp_prePort, vp_nextPort = @vp_nextPort, vp_berthside = @vp_berthside, vp_endport = @vp_endport, vp_width = @vp_width, vp_recordtime = GETDATE() WHERE (vp_id = @vp_id)">
          <SelectParameters>
              <asp:Parameter Name="date" Type="DateTime" />
          </SelectParameters> 
           <InsertParameters>
               <asp:Parameter Name="vp_date" Type="DateTime" />
               <asp:Parameter Name="vp_length" Type="Int32" />
               <asp:Parameter Name="vp_startport" Type="Int32" />
               <asp:Parameter Name="vp_vesselcode" Type="String" />
               <asp:Parameter Name="vp_chname" Type="String" />
               <asp:Parameter Name="vp_ename" Type="String" />
               <asp:Parameter Name="vp_ivoyage" Type="String" />
               <asp:Parameter Name="vp_expvoyage" Type="String" />
               <asp:Parameter Name="vp_betrthtime" Type="DateTime" />
               <asp:Parameter Name="vp_departtime" Type="DateTime" />
               <asp:Parameter Name="vp_prePort" Type="String" />
               <asp:Parameter Name="vp_nextPort" Type="String" />
               <asp:Parameter Name="vp_berthside" Type="String" />           
              <asp:Parameter Name="vp_width" Type="Int32" />
               <asp:Parameter Name="vp_endport" Type="Int32" /> 
           </InsertParameters>
           <UpdateParameters>
               <asp:Parameter Name="vp_length" Type="Int32" />
               <asp:Parameter Name="vp_startport" Type="Int32" />
               <asp:Parameter Name="vp_vesselcode" Type="String" />
               <asp:Parameter Name="vp_chname" Type="String" />
               <asp:Parameter Name="vp_ename" Type="String" />
               <asp:Parameter Name="vp_ivoyage" Type="String" />
               <asp:Parameter Name="vp_expvoyage" Type="String" />
               <asp:Parameter Name="vp_betrthtime" Type="DateTime" />
               <asp:Parameter Name="vp_departtime" Type="DateTime" />
               <asp:Parameter Name="vp_prePort" Type="String" />
               <asp:Parameter Name="vp_nextPort" Type="String" />
               <asp:Parameter Name="vp_berthside" Type="String" />
               <asp:Parameter Name="vp_endport" Type="Int32" />
               <asp:Parameter Name="vp_width" Type="Int32" /> 
               <asp:Parameter Name="vp_id" Type="Int32" />
           </UpdateParameters>
           <DeleteParameters>
               <asp:Parameter Name="vp_id" Type="Int32" />
           </DeleteParameters>
       </asp:SqlDataSource>
       &nbsp;
       <asp:Button ID="Button1" runat="server" OnClick="btnSave_Click" Text="保存泊位信息" Visible="False" />
       <asp:GridView ID="GridView2" runat="server" AllowPaging="True" AllowSorting="True" DataSourceID="SqlDataSource2"
           EmptyDataText="没有可显示的数据记录。"  BackColor="#B2C9D3" CellPadding="3"
             CellSpacing="1"        Font-Size="9pt" Width="970px" 
            ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Visible="False">
           <FooterStyle BackColor="#006699" Font-Bold="True" />
           <EmptyDataRowStyle BackColor="White" ForeColor="Red" />
           <RowStyle BackColor="White" ForeColor="Black" />
           <EditRowStyle BackColor="White" />
           <SelectedRowStyle BackColor="LightSkyBlue" Font-Bold="True" ForeColor="#333333" />
           <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
           <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
           <AlternatingRowStyle BackColor="#E0E0E0" BorderStyle="Solid" ForeColor="Black" />
          <Columns>
               <asp:BoundField DataField="Voyage" HeaderText="航次" ReadOnly="True" SortExpression="Voyage" />
               <asp:BoundField DataField="VLS_VCHNNM" HeaderText="中文船名" SortExpression="VLS_VCHNNM" />
               <asp:BoundField DataField="VLS_VNAMECD" HeaderText="英文船名" SortExpression="VLS_VNAMECD" />
               <asp:BoundField DataField="船长" HeaderText="船长" SortExpression="船长" />
               <asp:BoundField DataField="起始尺码" HeaderText="起始尺码" SortExpression="起始尺码" />
               <asp:BoundField DataField="终止尺码" HeaderText="终止尺码" SortExpression="终止尺码" />
               <asp:BoundField DataField="靠泊方向" HeaderText="靠泊方向" SortExpression="靠泊方向" />
               <asp:BoundField DataField="靠泊时间" HeaderText="靠泊时间" SortExpression="靠泊时间" />
               <asp:BoundField DataField="内贸" HeaderText="内贸" SortExpression="内贸" />
               <asp:BoundField DataField="航线" HeaderText="航线" SortExpression="航线" />
               <asp:BoundField DataField="时间" HeaderText="时间" SortExpression="时间" />
               <asp:BoundField DataField="PREPOT" HeaderText="上一港" SortExpression="PREPOT" />
               <asp:BoundField DataField="NXTPOT" HeaderText="下一港" SortExpression="NXTPOT" />
             
           </Columns> 
       </asp:GridView>
       <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:oraConnectionString %>"
           ProviderName="<%$ ConnectionStrings:oraConnectionString.ProviderName %>" SelectCommand="select voc.voc_ivoyage || ' / ' || voc.voc_expvoyage voyage,&#13;&#10;       vls.vls_vchnnm,&#13;&#10;       vls.vls_vnamecd,&#13;&#10;       vls.vls_vlength 船长,&#13;&#10;       nvl(vbt.vbt_astartpst, vbt.vbt_pstartpst) 起始起码,&#13;&#10;       nvl(vbt.vbt_aendpst, vbt.vbt_pendpst) 终止尺码,&#13;&#10;       nvl(vbt.vbt_abthdirc, vbt.vbt_pbthdirc) 靠泊方向,&#13;&#10;       nvl(vbt.vbt_abthtm, vbt.vbt_pbthtm) 靠泊时间,&#13;&#10;       vie.vie_intrade 内贸,&#13;&#10;       sln.sln_vcm_cstmcd || '-' || sln.sln_lne_rtcd 航线,&#13;&#10;       case&#13;&#10;         when round((nvl(vbt.vbt_abthtm, vbt.vbt_pbthtm) - sysdate) * 24, 0) < 0 then&#13;&#10;          0&#13;&#10;         else&#13;&#10;          round((nvl(vbt.vbt_abthtm, vbt.vbt_pbthtm) - sysdate) * 24, 0)&#13;&#10;       end 时间,&#13;&#10;       pot.prepot,&#13;&#10;       pot.nxtpot&#13;&#10;  from ps_vessel_berthes vbt,&#13;&#10;       ps_vessel_occurences voc,&#13;&#10;       ps_vessels vls,&#13;&#10;       ps_vessel_import_exports vie,&#13;&#10;       ps_service_lines sln,&#13;&#10;       (SELECT pot1.pot_cty_countrycd || pot1.pot_portcd prepot,&#13;&#10;               pot2.pot_cty_countrycd || pot2.pot_portcd nxtpot,&#13;&#10;               sln.sln_lne_rtcd,&#13;&#10;               sln.sln_slineid&#13;&#10;          FROM (select case&#13;&#10;                         when LAG(scl.scl_pot_portcd)&#13;&#10;                          over(partition by scl.scl_sch_sln_slineid order by&#13;&#10;                                   scl.scl_portseq) is null then&#13;&#10;                          first_value(scl.scl_pot_portcd)&#13;&#10;                          over(partition by scl.scl_sch_sln_slineid order by&#13;&#10;                               scl.scl_portseq desc)&#13;&#10;                         else&#13;&#10;                          LAG(scl.scl_pot_portcd)&#13;&#10;                          over(partition by scl.scl_sch_sln_slineid order by&#13;&#10;                               scl.scl_portseq)&#13;&#10;                       end preprt,&#13;&#10;                       case&#13;&#10;                         when LEAD(scl.scl_pot_portcd)&#13;&#10;                          over(partition by scl.scl_sch_sln_slineid order by&#13;&#10;                                   scl.scl_portseq) is null then&#13;&#10;                          first_value(scl.scl_pot_portcd)&#13;&#10;                          over(partition by scl.scl_sch_sln_slineid order by&#13;&#10;                               scl.scl_portseq)&#13;&#10;                         else&#13;&#10;                          LEAD(scl.scl_pot_portcd)&#13;&#10;                          over(partition by scl.scl_sch_sln_slineid order by&#13;&#10;                               scl.scl_portseq)&#13;&#10;                       end nextprt,&#13;&#10;                        scl.scl_sch_sln_slineid,&#13;&#10;                        scl.scl_pot_portcd&#13;&#10;                  from PS_SSHIPPING_BERTH_PORTS scl&#13;&#10;                --and scl.scl_sch_sln_slineid = '9996'&#13;&#10;                ) scl,&#13;&#10;               ps_service_lines sln,&#13;&#10;               ps_ports pot1,&#13;&#10;               ps_ports pot2&#13;&#10;         where scl.scl_pot_portcd = 'XIA'&#13;&#10;           and sln.sln_slineid = scl.scl_sch_sln_slineid&#13;&#10;           and scl.preprt = pot1.pot_portcd&#13;&#10;           and scl.nextprt = pot2.pot_portcd) pot&#13;&#10; where vbt.vbt_pbthtm between sysdate - 1 and sysdate + 7&#13;&#10;   and vbt.vbt_voc_ocrrid = voc.voc_ocrrid&#13;&#10;   and vls.vls_vnamecd = voc.voc_vls_vnamecd&#13;&#10;   and voc.voc_ocrrid = vie.vie_vbt_voc_ocrrid&#13;&#10;   and vie.vie_iefg = 'I'&#13;&#10;   and sln.sln_slineid = vie.vie_sln_slineid&#13;&#10;   and vbt.vbt_adpttm is null&#13;&#10;   and pot.sln_slineid = sln.sln_slineid&#13;&#10; order by round((nvl(vbt.vbt_abthtm, vbt.vbt_pbthtm) -&#13;&#10;                to_date(to_char(sysdate, 'yyyy-mm-dd') || ' 00:00:00',&#13;&#10;                         'yyyy-mm-dd hh24:mi:ss')) * 24,&#13;&#10;                0)&#13;&#10;">
       </asp:SqlDataSource>
       &nbsp;
       <a href="../YardPlan/Vessel_Port.aspx"></a>
  
   </div> 
    </form>
</body>
</html>
