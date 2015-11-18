<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YardAnalyse_RainbowPict.aspx.cs"
    Inherits="YardAnalyse_RainbowPict" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>堆存计划-彩虹图</title>  
    <link rel="stylesheet" type="text/css" href="YardAnalyse_rainbow.css" />
    <script type="text/javascript" src="scripts/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="scripts/YardAnalyse_rainbow_main.js"></script>
    <script type="text/javascript" src="scripts/jqueryui/js/jquery-ui-1.9.2.custom.min.js"></script>
    <script type="text/javascript" src="scripts/json2.js"></script>
    <link rel="stylesheet" href="scripts/jqueryui/css/custom-theme/jquery-ui-1.9.2.custom.min.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrapper">
        <div class="ship">
            <div class="ship-add">
                <span class="add">
                    <asp:Button ID="btnShipLineAdd" runat="server" Text="新增" /></span></div>
            <div class="ship-info">
            </div>
        </div>
        <div class="main">
            <!--箱区排列分布-->
            <div class="div-col">
                <div id="div-E1">
                    无E1箱区资料
                </div>
                <div id="div-E2">
                    无E2箱区资料
                </div>
                <div id="div-E3">
                    无E3箱区资料
                </div>
                <div id="div-E4">
                    无E4箱区资料
                </div>
                <div id="div-E5">
                    无E5箱区资料
                </div>
                <div id="div-E6">
                    无E6箱区资料
                </div>
                <div id="div-ED">
                    无ED箱区资料
                </div>
                <div id="div-EC">
                    无EC箱区资料
                </div>
            </div>
            <div class="div-col">
                <div id="div-D1">
                    无D1箱区资料
                </div>
                <div id="div-D2">
                    无D2箱区资料
                </div>
                <div id="div-D3">
                    无D3箱区资料
                </div>
                <div id="div-D4">
                    无D4箱区资料
                </div>
                <div id="div-D5">
                    无D5箱区资料
                </div>
                <div id="div-D6">
                    无D6箱区资料
                </div>
                <div id="div-DD">
                    无DD箱区资料
                </div>
                <div id="div-DC">
                    无DC箱区资料
                </div>
            </div>
            <div class="div-col">
                <div id="div-C1">
                    无C1箱区资料
                </div>
                <div id="div-C2">
                    无C2箱区资料
                </div>
                <div id="div-C3">
                    无C3箱区资料
                </div>
                <div id="div-C4">
                    无C4箱区资料
                </div>
                <div id="div-C5">
                    无C5箱区资料
                </div>
                <div id="div-C6">
                    无C6箱区资料
                </div>
                <div id="div-CD">
                    无CD箱区资料
                </div>
                <div id="div-CC">
                    无CC箱区资料
                </div>
            </div>
            <div class="div-col">
                <div id="div-B1">
                    无B1箱区资料
                </div>
                <div id="div-B2">
                    无B2箱区资料
                </div>
                <div id="div-B3">
                    无B3箱区资料
                </div>
                <div id="div-B4">
                    无B4箱区资料
                </div>
                <div id="div-B5">
                    无B5箱区资料
                </div>
            </div>
            <div class="div-col">
                <div id="div-A1">
                    无A1箱区资料
                </div>
            </div>
        </div>
    </div>
    </form>
    <div id="dialogArea" style="display: none;">
        <h3 id="dialogArea_name">
        </h3>
        <ul>
            <li><span>航线：</span><select id="dialogArea_lnecd">
            </select></li>
            <li><span>卸货港1：</span><select id="dialogArea_port1">
            </select></li>
            <li><span>卸货港2：</span><select id="dialogArea_port2">
            </select></li>
            <li><span>箱高：</span><select id="dialogArea_height">
                <option>GP</option>
                <option>HQ</option>
            </select></li>
            <li><span>箱尺寸：</span><select id="dialogArea_size">
                <option>20</option>
                <option>40</option>
                <option>45</option>
            </select></li>
        </ul>
        <div id="message" style="color: Red; font-size: 9pt; display: none;">
            更新成功！
        </div>
    </div>
    <div id="dialogShip" style="display: none;">
        <ul>
            <li><span>航线名称：</span><select id="dialogShip_lnecd"></select></li>
            <li><span>航线颜色：</span><select id="dialogShip_color" style="width: 100px;">
                <option value="#4285F4" style="background-color: #4285F4;"></option>
                <option value="#EA4335" style="background-color: #EA4335;"></option>
                <option value="#34A853" style="background-color: #34A853;"></option>
                <option value="#FFFF00" style="background-color: #FFFF00;"></option>
                <option value="#FBBC05" style="background-color: #FBBC05;"></option>
                <option value="#00FFFF" style="background-color: #00FFFF;"></option>
                <option value="#00CCFF" style="background-color: #00CCFF;"></option>
                <option value="#99CC00" style="background-color: #99CC00;"></option>
                <option value="#FF6600" style="background-color: #FF6600;"></option>
                <option value="#FF99CC" style="background-color: #FF99CC;"></option>
                <option value="#33CCCC" style="background-color: #33CCCC;"></option>
            </select></li>
            <li><span>航线卸货港：</span>
                <div id="dialogShip_port">
                </div>
            </li>
        </ul>
        <div id="Div2" style="color: Red; font-size: 9pt; display: none;">
            更新成功！
        </div>
    </div>
   
</body>
</html>
