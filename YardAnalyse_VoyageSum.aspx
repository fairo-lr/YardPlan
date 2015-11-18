<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YardAnalyse_VoyageSum.aspx.cs"
    Inherits="YardPlan_YardAnalyse_VoyageSum" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        *
        {
            margin: 0;
            padding: 0;
        }
        ul
        {
            list-style: none;
        }
        li
        {
            display: inline;
            width: 200px;
        }
        .wrapper
        {
            font-size: 10pt;
            width: 1000px;
        }
        .main
        {
            width: 100%;
        }
        .left
        {
            float: left;
        }
        .right
        {
            float: right;
        }
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrapper">
        <div class="main">
            <div class="left">
                <h3>
                    1</h3>
                <asp:GridView ID="gvVoyageSum" runat="server" CellPadding="4" GridLines="None" AutoGenerateColumns="false"
                    ForeColor="#333333" BorderColor="LightGray" CellSpacing="1"   Font-Size="9pt"
                    AllowPaging="false" BackColor="#B2C9D3" Width="480px"   EmptyDataText="无纪录!">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <EmptyDataRowStyle BackColor="White" />
                    <RowStyle BackColor="White" ForeColor="#333333" HorizontalAlign="Center" Height="40px"/>
                    <EditRowStyle BackColor="#999999" />
                    <SelectedRowStyle BackColor="#80FFFF" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"  Height="40px"/>
                    <AlternatingRowStyle BackColor="#E0E0E0" ForeColor="#284775" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="V_POT_LDUNLDPORT" HeaderText="卸货港" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="GP20" HeaderText="20'<br/>8'6''" HtmlEncode="false" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="GP40" HeaderText="40'<br/>8'6''" HtmlEncode="false" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="HQ40" HeaderText="40'<br/>9'6''" HtmlEncode="false" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="HQ45" HeaderText="45'<br/>9'6''" HtmlEncode="false" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="RF20" HeaderText="20'RF" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="RF40" HeaderText="40'RF" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="RF45" HeaderText="45'RF" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="总计" HeaderText="总计" ItemStyle-Width="50px" ItemStyle-BackColor="Yellow" />
                    </Columns>
                </asp:GridView>
            </div>
            <div class="right">
                <h3>
                    2</h3>
                <asp:GridView ID="gvVoyageSum2" runat="server" CellPadding="4" GridLines="None" 
                    AllowPaging="false" AutoGenerateColumns="false" ForeColor="#333333" BorderColor="LightGray"
                    CellSpacing="1" Font-Size="9pt" BackColor="#B2C9D3" Width="480px"
                    EmptyDataText="无纪录!">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <EmptyDataRowStyle BackColor="White" />
                    <RowStyle BackColor="White" ForeColor="#333333" HorizontalAlign="Center" Height="40px"/>
                    <EditRowStyle BackColor="#999999" />
                    <SelectedRowStyle BackColor="#80FFFF" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"  Height="40px" />
                    <AlternatingRowStyle BackColor="#E0E0E0" ForeColor="#284775" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="V_POT_LDUNLDPORT" HeaderText="卸货港" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="GP20" HeaderText="20'<br/>8'6''" HtmlEncode="false" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="GP40" HeaderText="40'<br/>8'6''" HtmlEncode="false" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="HQ40" HeaderText="40'<br/>9'6''" HtmlEncode="false" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="HQ45" HeaderText="45'<br/>9'6''" HtmlEncode="false" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="RF20" HeaderText="20'RF" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="RF40" HeaderText="40'RF" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="RF45" HeaderText="45'RF" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="总计" HeaderText="总计" ItemStyle-Width="50px" ItemStyle-BackColor="Yellow" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
