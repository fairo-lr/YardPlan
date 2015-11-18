<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselManager.aspx.cs"
    Inherits="BusyManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>船期日报管理</title>
    <link href="style/newStyle.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript" src="My97DatePicker/WdatePicker.js">
    </script>
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
background-color: #006699; 
font-size: 1em; 
margin: 1px 0px 0px; 
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

</style>
        <script language="javascript" type="text/jscript"> 
function tabLoad()
{
        alert("tabLoad");
}

//显示tab（tabHeadId：tab头中当前的超链接；tabContentId要显示的层ID） 

function showTab(tabHeadId,tabContentId,btn) 
{    
    //设置隐藏域的记录当前TAB页
    document.getElementById("hdTab").value=tabHeadId+"|"+tabContentId;
    //this.btnToExcel.Visable=false;
    //统计记录条数
//    alert(document.getElementById("hd"+tabHeadId).value);
    document.getElementById("lblCount").innerHTML="共 "+ document.getElementById("hd"+tabHeadId).value+" 条记录";

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
    function onBodyLoad()
    {
        
    	var tabValue = document.getElementById("hdTab").value;
    	if (tabValue != ""){
    	    showTab(tabValue.split("|")[0],tabValue.split("|")[1]);
    	}
    }
    
    //2012-04-06 Colin 鼠标进入后显示公式
    function formulaMouseOver(str)
    {
    	document.getElementById('tooltipDiv').innerHTML = str;
    	document.getElementById('tooltipDiv')
    	document.getElementById('tooltipDiv').style.left = event.clientX;
	    document.getElementById('tooltipDiv').style.top = event.clientY+document.documentElement.scrollTop;
	    document.getElementById('tooltipDiv').style.visibility="visible";
    }
    
    function formulaMouseOut()
    {
        document.getElementById('tooltipDiv').style.visibility="hidden";
    }


    </script>
</head>
<body onload="onBodyLoad()">
    <form id="form1" runat="server">
    <div id="tooltipDiv" style="visibility:hidden;position:absolute; border:solid 1px black;top:200px;left:100px; background-color:#ffffe1;z-index:100"></div>
        <div>
        <asp:HiddenField id="hdTab" runat="server" Value="tabs1|tabContent1" />
        <asp:HiddenField id="hdtabs1" runat="server" />
        <asp:HiddenField id="hdtabs2" runat="server" />
        <asp:HiddenField id="hdtabs3" runat="server" />
        <asp:HiddenField id="hdtabs4" runat="server" />        
            <table style="border-right: #b2c9d3 1px solid; padding-right: 1px; border-top: #b2c9d3 1px solid;
                padding-left: 1px; padding-bottom: 1px; margin: 1px; border-left: #b2c9d3 1px solid;
                padding-top: 1px; border-bottom: #b2c9d3 1px solid; font-size:9pt;" width="970px">
                <tr>
                    <td colspan="3" style="width: 952px">
                        <p><asp:Image ID="Image1" runat="server" Height="99px" ImageUrl="~/images/xscttitle.png"
                            Width="570px" /><label style="font-size:15pt; font-weight:bold">船期日报管理</label><br />
                    </p>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="width: 970px; height: 73px;">
                        <br />
                        <table border="0.5px" cellspacing="0"  width="970px"cellpadding="0" style="font-size:9pt" >
                            <tr>
                                <td style="width: 150px; height: 26px;">
                                    <label>年度：</label><input id="ipYear" name="ipYear" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy'})"
                                            style="width: 100px" runat="server" /></td>
                                <td style="width: 130px; height: 26px;">
                                    <label>
                                        月份：</label><input id="ipMonth" name="ipMonth" class="Wdate" onclick="WdatePicker({ dateFmt: 'MM'})"
                                            style="width: 60px" runat="server" /></td>
                                    <td style="width: 290px; height: 26px;">
                                        <label>
                                            日期：</label><input id="iptStartTime" name="StartTime" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd'})"
                                                style="width: 100px" runat="server" />到
                                        <input id="iptEndTime" name="EndTime" class="Wdate" onclick="WdatePicker({dateFmt: 'yyyy-MM-dd'})"
                                            style="width: 100px" runat="server" /></td>
                                    <%--<td style="width: 130px; height: 26px;">
                                        <label>
                                            班组：</label><asp:DropDownList ID="ddlShift" runat="server" Width="80px">
                                                <asp:ListItem Selected="True" Value="0">所有</asp:ListItem>
                                                <asp:ListItem Value="夜班">夜班</asp:ListItem>
                                                <asp:ListItem Value="白班">白班</asp:ListItem>
                                            </asp:DropDownList></td>--%>
                                    <td style="width: 140px; height: 26px;">
                                        <label>
                                            船期员：</label><asp:DropDownList ID="ddlName" runat="server" Width="80px" >
                                                <asp:ListItem Value="0">所有</asp:ListItem>
                                                <asp:ListItem Value="肖鑫">肖鑫</asp:ListItem>
                                               <asp:ListItem Value="林燕珍">林燕珍</asp:ListItem>
                                                <asp:ListItem Value="姚雪倩">姚雪倩</asp:ListItem>
                                                <asp:ListItem Value="王秋">王秋</asp:ListItem>
                                                <asp:ListItem Value="无">无</asp:ListItem>
                                            </asp:DropDownList></td>                          
                     <td style="width: 50px; height: 26px;" align="center">
                                    <asp:Button ID="btnSearch" runat="server" Text="查 询" OnClick="btnSearch_Click" Width="45px"
                                        Height="25px" /></td>
                                <td align="center" style="width: 50px; height: 26px">
                                    <asp:Button ID="btnClean" runat="server" Text="清 空" OnClick="btnClean_Click" Height="25px"
                                        Width="45px" /></td>
                                <td style="width: 50px; height: 26px;" align="center">
                                    <asp:Button ID="btnAdd" runat="server" Text="添 加" Width="45px" Height="25px" OnClick="btnAdd_Click" /></td>                                                              
                            </tr>         
                        </table>
                        
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align: right; width: 1190px;">
                        <label id="lblCount" runat="server" style="font-size:9pt"></label></td>
                </tr>
                <tr>
                    <td colspan="3" style="width: 970px;">
                                <div id="tabDiv">
                <!--tab头-->
                <div id="tabsHead" style="width: 970px; text-align: right;">
                    <a class="curtab" id="tabs1" href="javascript:showTab('tabs1','tabContent1','btnToExcel')">船期日报明细</a>&nbsp;
                                    </div>
                <div id="tabContent1"  style="display: block"  runat="server">
                    <asp:GridView ID="gvVesselWeek" runat="server" AllowPaging="True" AllowSorting="True"
                        AutoGenerateColumns="False" DataKeyNames="vi_id" DataSourceID="SqlDsVesselWeek"
                        EmptyDataText="没有可显示的数据记录。"  CellPadding="4"
                            GridLines="None"  OnRowCommand="gvVesselWeek_RowCommand" ForeColor="#333333" BorderColor="LightGray"
                            CellSpacing="1" Font-Size="9pt" BackColor="#B2C9D3" OnRowDataBound="gvVesselWeek_RowDataBound"
                            PageSize="16" Width="970px" OnDataBound="gvVesselWeek_DataBound" >
                            <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                            <RowStyle BackColor="White" ForeColor="#333333" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                            <AlternatingRowStyle ForeColor="#284775" HorizontalAlign="Center" BackColor="#E0E0E0" />
                            <EditRowStyle BackColor="#999999" />
                            <EmptyDataRowStyle BackColor="White" /> 
                        <Columns>
                            <asp:BoundField DataField="vi_id" HeaderText="vi_id" ReadOnly="True" SortExpression="vi_id" Visible="False" />
                            <asp:BoundField DataField="vi_date" HeaderText="船期日期" SortExpression="vi_date" />
                            <asp:BoundField DataField="vi_supervisor" HeaderText="主任" SortExpression="vi_supervisor" />
                            <asp:BoundField DataField="vi_vesselpeople" HeaderText="堆场员" SortExpression="vi_vesselpeople" />
                            <asp:BoundField DataField="vi_recordtime" HeaderText="船期创建时间" SortExpression="vi_recordtime" />
                           <asp:BoundField DataField="vc_recordtime" HeaderText="船期更新时间" SortExpression="vc_recordtime" /> 
                            <asp:BoundField DataField="vi_submit" HeaderText="是否最终提交" SortExpression="vi_submit" />
                              <asp:ButtonField Text="详细"  CommandName="btnDetail">
                                    <ItemStyle Width="25px" />
                                </asp:ButtonField>
                                <asp:ButtonField CommandName="btnEdit" Text="修改">
                                    <ItemStyle Width="25px" />
                                </asp:ButtonField> 
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDsVesselWeek" runat="server" ConnectionString="<%$ ConnectionStrings:YardPlanConnectionString1 %>"
                        DeleteCommand="DELETE FROM [VesselInfo] WHERE [vi_id] = @vi_id" InsertCommand="INSERT INTO [VesselInfo] ([vi_date], [vi_supervisor], [vi_vesselpeople], [vi_recordtime], [vi_submit]) VALUES (@vi_date, @vi_supervisor, @vi_vesselpeople, @vi_recordtime, @vi_submit)"
                        ProviderName="<%$ ConnectionStrings:YardPlanConnectionString1.ProviderName %>"
                        SelectCommand="SELECT [vi_id], [vi_date], [vi_supervisor], [vi_vesselpeople], [vi_recordtime], [vc_recordtime], [vi_submit] FROM [VesselInfo] left join (SELECT [vc_date] ,max([vc_recordtime]) [vc_recordtime]   FROM [YardPlan].[dbo].[VesselContainer]group by [vc_date]) vc on [vi_date] = [vc_date] "
                        UpdateCommand="UPDATE [VesselInfo] SET [vi_date] = @vi_date, [vi_supervisor] = @vi_supervisor, [vi_vesselpeople] = @vi_vesselpeople, [vi_recordtime] = @vi_recordtime, [vi_submit] = @vi_submit WHERE [vi_id] = @vi_id" OnSelected="SqlDsVesselWeek_Selected">
                        <InsertParameters>
                            <asp:Parameter Name="vi_date" Type="DateTime" />
                            <asp:Parameter Name="vi_supervisor" Type="String" />
                            <asp:Parameter Name="vi_vesselpeople" Type="String" />
                            <asp:Parameter Name="vi_recordtime" Type="DateTime" />
                            <asp:Parameter Name="vi_submit" Type="String" />
                        </InsertParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="vi_date" Type="DateTime" />
                            <asp:Parameter Name="vi_supervisor" Type="String" />
                            <asp:Parameter Name="vi_vesselpeople" Type="String" />
                            <asp:Parameter Name="vi_recordtime" Type="DateTime" />
                            <asp:Parameter Name="vi_submit" Type="String" />
                            <asp:Parameter Name="vi_id" Type="Int32" />
                        </UpdateParameters>
                        <DeleteParameters>
                            <asp:Parameter Name="vi_id" Type="Int32" />
                        </DeleteParameters>
                    </asp:SqlDataSource>
                    &nbsp;&nbsp;
                    <asp:HiddenField ID="hdSql" runat="server" />
                        </div>  
                       </div>
                </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdTime" runat="server" />
            &nbsp;
          <!--------导出到Excel------------------------------------------------------------------>
            <asp:GridView ID="gvToExcel" runat="server" BackColor="White" BorderColor="#3366CC"
                BorderStyle="None" BorderWidth="1px" CellPadding="4" DataSourceID="SqlDsVesselWeek"
                Visible="False" Width="304px" DataKeyNames="vi_id">
                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                <RowStyle BackColor="White" ForeColor="#003399" />
                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
            </asp:GridView>                 
            <asp:AccessDataSource ID="AccessDataSource1" runat="server" ConflictDetection="CompareAllValues"
                DataFile="~/user_mng.mdb" DeleteCommand="DELETE FROM [user_mng] WHERE [user_id] = ? AND [user_name] = ? AND [user_psw] = ?"
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
        </div>
    </form>
</body>
</html>
