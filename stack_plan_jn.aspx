<%@ Page Language="C#" AutoEventWireup="true" CodeFile="stack_plan_jn.aspx.cs" Inherits="stack_plan_jn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>堆存计划审计</title>
    <style type="text/css">
        .marginLeft10{
            margin-left:20px;
        }
        .title{
            font-size:x-large;
            font-weight:bold;
        }
        .hidden{
            display:none;
        }

        .jntable td{
            border:1px solid #006699;
            width:120px;
            text-align:center;
        }
        .jntable thead td{
            background-color:#006699;
            color:white;
            font-weight:bold;
            text-align:center
        }
    </style>

    <script type="text/javascript" src="scripts/jquery-1.8.0.min.js"></script>

    <script type="text/javascript" src="scripts/json2.js"></script>

    <script type="text/javascript">
    $(document).ready(function () {
        $("#GridView1 img").css("cursor","pointer").click(function () {
            if ($(this).attr("src") == "images/003_39.png"){
                $(this).attr("src","images/003_40.png");
                var t_planid = $(this).parent().prev().html();
                var $img = $(this);
                $.get("GetStackJNLstHandler.ashx",{"planid":t_planid},function (data) {
                    var t_arr = JSON.parse(data);
                    var $tr = $("<tr class='"+t_planid+"'><td colspan='10'></td></tr>")
                    var $jntable = $("<table cellspacing='0' class='jntable'><thead><tr><td>序号</td><td>开始倍</td><td>结束倍</td><td>修改时间</td><td>员工</td><td>操作</td></tr><thead><tbody></tbody></table>")
                    $tr.find("td").append($jntable);
                    for(var i = 0; i < t_arr.length; ++i ){
                        var $jntr = $("<tr></tr>");
                        $jntr.append("<td>"+t_arr[i].Range+"</td>");
                        $jntr.append("<td>"+t_arr[i].Stbay+"</td>");
                        $jntr.append("<td>"+t_arr[i].Edbay+"</td>");
                        $jntr.append("<td>"+t_arr[i].Modtm+"</td>");
                        $jntr.append("<td>"+t_arr[i].Empno+"</td>");
                        $jntr.append("<td>"+t_arr[i].Oper+"</td>");
                        $jntable.find("tbody").append($jntr);
                    }
                    $tr.insertAfter($img.parent().parent());
                    
                })
            }else{
                $(this).attr("src","images/003_39.png");
                var t_planid = $(this).parent().prev().html();
                $("tr."+t_planid).remove();
            }
        });
    });
    </script>

</head>
<body style="font-size: 9pt">
    <form id="form1" runat="server">
        <div>
            <label class='title'>
                船舶堆存计划审计</label>
            <br />
            <br />
            <div style="border: 1px solid #006699; width: 1200px; height: 40px; padding-top: 10px;">
                <label class='marginLeft10' style="text-align: center">
                    船名航次:</label>
                <asp:DropDownList ID="ddlVsl" DataSourceID="sdsVsl" runat="Server" DataTextField="vname"
                    DataValueField="vocid">
                </asp:DropDownList>
                <label class='marginLeft10'>
                    堆场类型</label>
                <asp:DropDownList ID="ddlstackType" runat="Server">
                    <asp:ListItem Value="USP">卸船计划</asp:ListItem>
                    <asp:ListItem Value="EFS">出口重箱计划</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnSearch" CssClass="marginLeft10" runat="Server" Text="查询" OnClick="btnSearch_Click" />
            </div>
            <br />
            <div>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    DataSourceID="SqlDataSource1" GridLines="None" AllowSorting="True" ForeColor="#333333"
                    BorderColor="LightGray" CellSpacing="1" Font-Size="9pt" BackColor="#B2C9D3" Width="1200px"
                    EmptyDataText="" OnRowCreated="GridView1_RowCreated">
                    <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                    <Columns>
                        <asp:BoundField DataField="卸货港" HeaderText="卸货港" SortExpression="卸货港">
                            <HeaderStyle VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="尺寸" HeaderText="尺寸" SortExpression="尺寸">
                            <HeaderStyle VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="箱型" HeaderText="箱型" SortExpression="箱型">
                            <HeaderStyle VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="高度" HeaderText="高度" SortExpression="高度">
                            <HeaderStyle VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="状态" HeaderText="状态" SortExpression="状态">
                            <HeaderStyle VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="特殊标志" HeaderText="特殊标志" SortExpression="特殊标志">
                            <HeaderStyle VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="危品" HeaderText="危品" SortExpression="危品">
                            <HeaderStyle VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="超限" HeaderText="超限" SortExpression="超限">
                            <HeaderStyle VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="冻柜" HeaderText="冻柜" SortExpression="冻柜">
                            <HeaderStyle VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="箱量" HeaderText="箱量" SortExpression="箱量">
                            <HeaderStyle VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="planid" HeaderText="planid" SortExpression="planid">
                            <HeaderStyle VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="审计">
                            <HeaderStyle VerticalAlign="Middle" />
                        </asp:BoundField>
                    </Columns>
                    <RowStyle BackColor="White" ForeColor="#333333" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle ForeColor="#284775" HorizontalAlign="Center" BackColor="#E0E0E0" />
                    <EditRowStyle BackColor="#999999" />
                    <EmptyDataRowStyle BackColor="White" />
                </asp:GridView>
            </div>
        </div>
        </div>
        <asp:SqlDataSource ProviderName="System.Data.OracleClient" ID="sdsVsl" runat="Server"
            ConnectionString="Data Source=xsctture;Persist Security Info=True;User ID=xsctwebusr;Password=xsctwebusr;Unicode=True">
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource1" ProviderName="System.Data.OracleClient" ConnectionString="Data Source=xsctture;Persist Security Info=True;User ID=xsctwebusr;Password=xsctwebusr;"
            runat="server"></asp:SqlDataSource>
    </form>
</body>
</html>
