<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Vessel_Month.aspx.cs"
    Inherits="Safety_Accident" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>月度船期</title>
    <link rel="Stylesheet" href="style/DefaultStyle.css" type="text/css" />
    <script language="javascript" type="text/javascript" src="My97DatePicker/WdatePicker.js">   
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>         
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                DataSourceID="SqlDataSource1" EmptyDataText="没有可显示的数据记录。"  BackColor="#B2C9D3" CellPadding="3"
             CellSpacing="1"        Font-Size="9pt" Width="970px" DataKeyNames="vm_id"
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
                    <asp:BoundField DataField="vm_id" HeaderText="vm_id" InsertVisible="False" ReadOnly="True"      SortExpression="vm_id"/>
                          <asp:BoundField DataField="vm_date" HeaderText="日期" SortExpression="vm_date" InsertVisible="False" />
                    <asp:BoundField DataField="vm_line" HeaderText="航线" SortExpression="vm_line" />
                    <asp:BoundField DataField="vm_customer" HeaderText="船东" SortExpression="vm_customer" />
                    <asp:BoundField DataField="vm_chname" HeaderText="中文船名" SortExpression="vm_chname" />
                    <asp:BoundField DataField="vm_ename" HeaderText="英文船名" SortExpression="vm_ename" />
                    <asp:BoundField DataField="vm_ivoyage" HeaderText="进口航次" SortExpression="vm_ivoyage" />
                    <asp:BoundField DataField="vm_expvoyage" HeaderText="出口航次" SortExpression="vm_expvoyage" />
                   <asp:BoundField DataField="vm_berthtime" HeaderText="计划靠泊" SortExpression="vm_berthtime" /> 
                   <asp:BoundField DataField="vm_departtime" HeaderText="计划离泊" SortExpression="vm_departtime" />
                    <asp:BoundField DataField="vm_internal" HeaderText="内贸" SortExpression="vm_internal" />
                    <asp:BoundField DataField="vm_recordtime" HeaderText="记录时间" SortExpression="vm_recordtime" InsertVisible="False" />
                     <asp:CommandField ShowEditButton="True" />
                    <asp:CommandField ShowDeleteButton="True" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:YardPlanConnectionString1 %>"
                DeleteCommand="DELETE FROM [VesselMonth] WHERE [vm_id] = @vm_id" 
                ProviderName="<%$ ConnectionStrings:YardPlanConnectionString1.ProviderName %>"
                SelectCommand="SELECT [vm_id], [vm_date], [vm_line], [vm_customer], [vm_chname], [vm_ename], [vm_ivoyage], [vm_expvoyage], [vm_departtime], [vm_berthtime], [vm_internal], [vm_recordtime] FROM [VesselMonth] where convert(varchar(7),vm_date,120)=convert(varchar(7),@date,120) order by vm_id asc"
                UpdateCommand="UPDATE [VesselMonth] SET  [vm_line] = @vm_line, [vm_customer] = @vm_customer, [vm_chname] = @vm_chname, [vm_ename] = @vm_ename, [vm_ivoyage] = @vm_ivoyage, [vm_expvoyage] = @vm_expvoyage, [vm_departtime] = @vm_departtime, [vm_berthtime] = @vm_berthtime, [vm_internal] = @vm_internal, [vm_recordtime] = getDate() WHERE [vm_id] = @vm_id">
                <SelectParameters>
                    <asp:Parameter Name="date" Type="DateTime" />
                </SelectParameters>              
                <UpdateParameters>             
                    <asp:Parameter Name="vm_line" Type="String" />
                    <asp:Parameter Name="vm_customer" Type="String" />
                    <asp:Parameter Name="vm_chname" Type="String" />
                    <asp:Parameter Name="vm_ename" Type="String" />
                    <asp:Parameter Name="vm_ivoyage" Type="String" />
                    <asp:Parameter Name="vm_expvoyage" Type="String" />
                    <asp:Parameter Name="vm_departtime" Type="DateTime" />
                    <asp:Parameter Name="vm_berthtime" Type="DateTime" />
                    <asp:Parameter Name="vm_internal" Type="String" />             
                    <asp:Parameter Name="vm_id" Type="Int32" />
                </UpdateParameters>
                <DeleteParameters>
                    <asp:Parameter Name="vm_id" Type="Int32" />
                </DeleteParameters>
            </asp:SqlDataSource>
            <asp:Button ID="btnMonth" runat="server" OnClick="btnMonth_Click" Text="生成月度信息" /><br />
            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False"
                DataSourceID="SqlDataSource2" EmptyDataText="没有可显示的数据记录。"  BackColor="#B2C9D3" CellPadding="3"
             CellSpacing="1"        Font-Size="9pt" Width="970px" 
            ForeColor="#333333" GridLines="None">
                <FooterStyle BackColor="#006699" Font-Bold="True" />
                <EmptyDataRowStyle BackColor="White" ForeColor="Red" />
                <RowStyle BackColor="White" ForeColor="Black" />
                <EditRowStyle BackColor="White" />
                <SelectedRowStyle BackColor="LightSkyBlue" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="#E0E0E0" BorderStyle="Solid" ForeColor="Black" />
                <Columns>
                    <asp:BoundField DataField="航线" HeaderText="航线" SortExpression="航线" />
                    <asp:BoundField DataField="中文船名" HeaderText="中文船名" ReadOnly="True" SortExpression="中文船名" />
                    <asp:BoundField DataField="英文船名" HeaderText="英文船名" ReadOnly="True" SortExpression="英文船名" />
                    <asp:BoundField DataField="进口航次" HeaderText="进口航次" SortExpression="进口航次" />
                    <asp:BoundField DataField="出口航次" HeaderText="出口航次" SortExpression="出口航次" />
                    <asp:BoundField DataField="靠泊" HeaderText="靠泊" SortExpression="靠泊" />
                    <asp:BoundField DataField="离泊" HeaderText="离泊" SortExpression="离泊" />
                    <asp:BoundField DataField="计划靠泊" HeaderText="计划靠泊" SortExpression="计划靠泊" />
                    <asp:BoundField DataField="计划离泊" HeaderText="计划离泊" SortExpression="计划离泊" />
                    <asp:BoundField DataField="开港时间" HeaderText="开港时间" SortExpression="开港时间" />
                    <asp:BoundField DataField="进箱截止" HeaderText="进箱截止" SortExpression="进箱截止" />
                    <asp:BoundField DataField="截熏蒸箱时间" HeaderText="截熏蒸箱时间" SortExpression="截熏蒸箱时间" />
                    <asp:BoundField DataField="海关截单时间" HeaderText="海关截单时间" SortExpression="海关截单时间" />
                    <asp:BoundField DataField="码头截单时间" HeaderText="码头截单时间" SortExpression="码头截单时间" />
                    <asp:BoundField DataField="航线名称" HeaderText="航线名称" SortExpression="航线名称" />
                    <asp:BoundField DataField="代理" HeaderText="代理" SortExpression="代理" />
                    <asp:BoundField DataField="船舶ID" HeaderText="船舶ID" SortExpression="船舶ID" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:oraConnectionString %>"
                ProviderName="<%$ ConnectionStrings:oraConnectionString.ProviderName %>" SelectCommand="SELECT sln_lne_rtcd 航线, &#13;&#10;       vls_vchnnm 中文船名, vls_vengnm 英文船名, a.voc_ivoyage 进口航次, a.voc_expvoyage 出口航次     &#13;&#10;       ,decode(sign(to_number(decode(vbt_abthtm,null,sysdate,vbt_abthtm)-sysdate)),'-1','Y','N') 靠泊&#13;&#10;       ,decode(sign(to_number(decode(vbt_adpttm,null,sysdate,vbt_adpttm)-sysdate)),'-1','Y','N') 离泊&#13;&#10;       ,to_char(VBT_PBTHTM,'yy.mm.dd hh24:mi') 计划靠泊, to_char(VBT_PDPTTM,'yy.mm.dd hh24:mi') 计划离泊         &#13;&#10;       ,to_char(a.voc_rcvsttm+7,'yy.mm.dd hh24:mi') 开港时间 &#13;&#10;       ,to_char(a.voc_rcvedtm,'yy.mm.dd hh24:mi') 进箱截止,to_char(b.VOC_END_SUFFOCATION,'yy.mm.dd hh24:mi') 截熏蒸箱时间&#13;&#10;       ,to_char(b.VOC_CUSEDTM,'yy.mm.dd hh24:mi') 海关截单时间,to_char(b.VOC_TEREDTM,'yy.mm.dd hh24:mi') 码头截单时间&#13;&#10;      ,LNE_RTCHNNM 航线名称 ,cst_cstmnm 代理,a.VOC_OCRRID 船舶ID  &#13;&#10;FROM ps_vessels,ps_vie_voc_vwf a,ps_shipping_lines,ps_service_lines,ps_customers,PS_VESSEL_OCCURENCES b &#13;&#10;WHERE a.voc_ocrrid=b.voc_ocrrid and vls_vnamecd=a.voc_vls_vnamecd  AND vie_iefg = 'E' &#13;&#10;      AND VLS_VTYPE <>'BAR' and not ((vls_vengnm ='IBN KHALIKKAN' and vio_voyage='657W') or (vls_vengnm ='IBN ABDOUN' &#13;&#10;      and vio_voyage='658W') or (vls_vengnm ='MAERSK KOKURA' and vio_voyage='0902') or (vls_vengnm ='MAERSK SENANG'  &#13;&#10;      and vio_voyage='0902')) &#13;&#10;      AND a.voc_rcvsttm IS NOT NULL&#13;&#10;      and SLN_SLINEID=VIE_SLN_SLINEID and LNE_RTCD=SLN_LNE_RTCD and sln_agt_cstmcd=cst_cstmcd      &#13;&#10;  &#13;&#10;                and vie_intrade is null &#13;&#10;  &#13;&#10;                     and vbt_pbthtm>=add_months(to_date(to_char(sysdate,'yyyy-mm'),'yyyy-mm'),-1) &#13;&#10;                      and vbt_pbthtm<add_months(to_date(to_char(sysdate,'yyyy-mm'),'yyyy-mm'),-1+1) &#13;&#10;  &#13;&#10;order by a.VBT_PBTHTM">
            </asp:SqlDataSource>
         
        </div>
    </form>
</body>
</html>
