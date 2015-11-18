<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Vessel_NewReport.aspx.cs"
    Inherits="Vessel_NewReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>创建船期日报</title>
   <!--连接外界数据--> 
    <link href="style/DefaultStyle.css" rel="stylesheet" type="text/css" />
    <link href="style/NewReportStyle.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" src="My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript" language="javascript">
   // JScript 文件
var g_urlArray=["Vessel_Basic.aspx","Vessel_Plan.aspx","Vessel_Port.aspx"]
    function $(id) {
        return document.getElementById(id);
    }
    var g_tabIndex = 1;
       
    function changTab(index) {
   //样式1 
        var table  = $("tabTable");
        $("img"+g_tabIndex).style.visibility = "hidden";
        table.rows[0].cells[g_tabIndex - 1].style.backgroundColor="#006699";
        table.rows[0].cells[g_tabIndex - 1].style.color="White";
        g_tabIndex = index;
        table.rows[0].cells[g_tabIndex - 1].style.backgroundColor="#E8F7FC";
        table.rows[0].cells[g_tabIndex - 1].style.color="Black";

        var startTime = $('iptStartTime').value;       
      
         $("framePage").src=g_urlArray[g_tabIndex-1]+"?date="+startTime;//+"&random="+Math.random();  ////+"&tab="+hdShift;
        $("img"+g_tabIndex).style.visibility = "visible";
        //设置提示块
        $("divTip").style.visibility="visible";
        $("divTip").style.left = ((g_tabIndex - 1)*230 + 80) +"px";

    }
   
  
   
  function checkClick()
  {
     return confirm("是否最终提交");
  }  
    
   function setStartTime()
   { 
       $('iptStartTime').value = $dp.cal.getDateStr();  
        window.location="Vessel_NewReport.aspx?date="+$('iptStartTime').value;
   } 
   
function bodyLoad(){
     var table  = $("tabTable");
        table.rows[0].cells[g_tabIndex - 1].style.backgroundColor="#E8F7FC";
        table.rows[0].cells[g_tabIndex - 1].style.color="Black";        
        $("iptStartTime").value=$("hdStartTime").value;                
        var startTime = $('iptStartTime').value;
       $("framePage").src=g_urlArray[g_tabIndex-1]+"?date="+startTime;
        $("img"+g_tabIndex).style.visibility = "visible";
  
             //设置显示 
         var show=document.getElementById('hdShow').value;
        if(show=="hide")
        {
              table.style.display="none";
              $("framePage").style.display="none";                 
        } 
        else
        {
        }
        //设置提示块
        $("divTip").style.visibility="visible";
        $("divTip").style.left = "120px";
        if ($("framePage").attachEvent){
            $("framePage").attachEvent("onload", function(){
                $("img"+g_tabIndex).style.visibility="hidden";
                $("divTip").style.visibility="hidden";
                $("img"+g_tabIndex).style.visibility = "hidden";
            });
        } else {
            $("framePage").onload = function(){
                $("img"+g_tabIndex).style.visibility="hidden";
                $("divTip").style.visibility="hidden";

            };
        }
   
 
    } 
    </script>

</head>
<body onload="bodyLoad()">
    <form id="form1" runat="server">
      <div>
            <label class='title'>
                &nbsp; &nbsp; &nbsp;&nbsp;
                船期日报</label>
            <asp:HyperLink ID="lnkBusyManager" NavigateUrl="VesselManager.aspx" runat="server"
                Height="19px" Width="80px">船期日报管理</asp:HyperLink>
            <asp:HyperLink ID="hlkPreview" runat="server" Visible="false" CssClass="preReport" Height="19px" Width="80px"
                Target="_blank">预览船期报表</asp:HyperLink>            
            <asp:LinkButton ID="hypAddBusy" runat="server" Text="" ForeColor="Red" OnClick="hypAddBusy_Click"></asp:LinkButton>
            <div class="mianTableDiv">
                <table id="mainTable" border="0" cellspacing="0" cellpadding="0" style="left: 306px; top: 39px">
                    <tr>
                        <td class="tdRight">
                            <label>
                                日期：</label>
                        </td>
                        <td>
                            <input id="iptStartTime" name="StartTime" class="Wdate" onclick="WdatePicker({onpicked:setStartTime})" runat="server" />
                        </td>
                    </tr>
                   <tr style="display:none;">
                        <!-- 分割线 -->
                        <td class="tdLine" colspan="2">
                        </td>
                    </tr>  
                    <tr>
                        <td class="tdRight" style="height: 22px">
                            <label>
                                值班主任：</label>
                        </td>
                        <td style="height: 22px">
                            <asp:DropDownList ID="ddlSupervisor" runat="Server" Width="67px" Enabled="False">
                                <asp:ListItem Selected="True" Value="肖鑫">肖鑫</asp:ListItem>                              
                            </asp:DropDownList>
                        </td>
                    </tr>
                      <tr style="display:none;">
                        <!-- 分割线 -->
                        <td class="tdLine" colspan="2">
                        </td>
                    </tr> 
                    <tr>
                        <td class="tdRight">
                            <label>
                                当班船期员：</label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlVesselPeople" runat="Server" Width="67px" DataSourceID="SqlDsYardPeople" DataTextField="YP_Name" DataValueField="YP_ID" OnDataBound="ddlVesselPeople_DataBound" OnPreRender="ddlVesselPeople_PreRender">                                
                            </asp:DropDownList><asp:SqlDataSource ID="SqlDsYardPeople" runat="server" ConnectionString="<%$ ConnectionStrings:YardPlanConnectionString1 %>"
                                SelectCommand="SELECT [YP_ID], [YP_Name], [YP_State] FROM [YardPeople]"></asp:SqlDataSource>
                        </td>
                    </tr>
                </table>
                <table id="btnTable" border="0" cellspacing="0" cellpadding="0" style="left: 451px; top: 171px">
                    <tr>
                        <td align="center" style="width: 150px; margin-right: 30px; height: 24px;">
                            <asp:Button runat="Server" ID="btnSave" Text="船舶作业信息保存" Width="120px" OnClick="btnSave_Click" /></td>
                        <td align="center" style="width: 150px; margin-left: 30px; height: 24px;">
                            <asp:Button runat="server" ID="btnEndShift" Text="确认工班结束" OnClientClick="return checkClick()" Width="100px" OnClick="btnEndShift_Click" Visible="false" /></td>
                        <td align="left" style="width: 150px; height: 24px;">
                            <asp:Label ID="labBtnMessage" runat="Server" ForeColor="Red"></asp:Label></td>
                    </tr>
                </table>
            </div>
            <div>        
                <table id="tabTable" border="0" cellspacing="0" cellpadding="0" width="100%">
                    <tr style="line-height: 30px; border-top: solid 1px #FFffff; ">
                        <td onclick="changTab(1);">
                            基础信息
                            <img id="img1" style="visibility: hidden;" src="images/18.gif" />
                        </td>
                        <td onclick="changTab(2);">
                            周船舶信息
                            <img id="img2" style="visibility: hidden;" src="images/18.gif" />
                        </td>
                         <td onclick="changTab(3);">
                            件杂货
                            <img id="img3" style="visibility: hidden;" src="images/18.gif" />
                        </td>
                      
           
                    </tr>
                </table>
               
            </div>
            <div>
                <iframe id="framePage"  frameborder="0" scrolling="auto"  style="width: 1180px; height: 560px;"></iframe>
            </div>
        </div>
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
        <asp:HiddenField ID="hdStartTime" runat="Server" />
        &nbsp;&nbsp;
        <asp:SqlDataSource ID="SqlDsVesselInfo" runat="server" ConnectionString="<%$ ConnectionStrings:YardPlanConnectionString1 %>"
            SelectCommand="SELECT [vi_id], [vi_date], [vi_supervisor], [vi_vesselpeople], [vi_recordtime], [vi_submit] FROM [VesselInfo]" DeleteCommand="DELETE FROM [VesselInfo] WHERE [vi_id] = @vi_id" 
            InsertCommand="INSERT INTO [VesselInfo] ([vi_date], [vi_supervisor], [vi_vesselpeople], [vi_recordtime], [vi_submit]) VALUES (@vi_date, @vi_supervisor, @vi_vesselpeople, @vi_recordtime, @vi_submit)" 
            UpdateCommand="UPDATE [VesselInfo] SET [vi_date] = @vi_date, [vi_supervisor] = @vi_supervisor, [vi_vesselpeople] = @vi_vesselpeople, [vi_recordtime] = @vi_recordtime, [vi_submit] = @vi_submit WHERE [vi_id] = @vi_id">
            <DeleteParameters>
                <asp:Parameter Name="vi_id" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="vi_date" Type="DateTime" />
                <asp:Parameter Name="vi_supervisor" Type="String" />
                <asp:Parameter Name="vi_vesselpeople" Type="String" />
                <asp:Parameter Name="vi_recordtime" Type="DateTime" />
                <asp:Parameter Name="vi_submit" Type="String" />
                <asp:Parameter Name="vi_id" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="vi_date" Type="DateTime" />
                <asp:Parameter Name="vi_supervisor" Type="String" />
                <asp:Parameter Name="vi_vesselpeople" Type="String" />
                <asp:Parameter Name="vi_recordtime" Type="DateTime" />
                <asp:Parameter Name="vi_submit" Type="String" />
            </InsertParameters>
        </asp:SqlDataSource>
        <asp:HiddenField ID="hdisHasRecord" runat="server" />
        <div id='divTip' style="  width:140px" >数据正在生成，请等待！
        </div>
  <asp:HiddenField ID="hdShift" runat="server" Value="tab1" />
        <asp:HiddenField ID="hdShow" runat="server" Value="show" />
    </form>
</body>
</html>
