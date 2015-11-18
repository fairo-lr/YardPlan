<%@ Page Language="C#" AutoEventWireup="true"   CodeFile="Vessel_BerthNEW.aspx.cs" Inherits="testPDF" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<META HTTP-EQUIV="Cache-Control" CONTENT="no-cache, must-revalidate">
<head runat="server">
    <title>无标题页</title>
    <script type="text/javascript" src="scripts/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="scripts/jquery.rotate.js"></script>
    <style type="text/css">
        .smallBtn{
        width:20px;
        }
    </style>
    <script type="text/javascript">
        var g_present = 100;
        $(document).ready(function () {
			 $("#imgBerath").css("width","140%");
             $("#imgBerath").css("height","140%");
            //$("#imgBerath").rotate(-90);
            $("#plus").click(function () {
                $("#imgBerath").css("width",(g_present+5)+"%");
                $("#imgBerath").css("height",(g_present+5)+"%");
                g_present += 5;
            });
            $("#mins").click(function () {
                $("#imgBerath").css("width",(g_present-5)+"%");
                $("#imgBerath").css("height",(g_present-5)+"%");
                g_present -= 5;
            });
            $("#print").click(function () {
                CallPrint("imgHold");
            });
        });
        
    function CallPrint(strid)
    {
	
     $("#imgBerath").rotate(90);
     var prtContent = document.getElementById(strid);
     var WinPrint = window.open('','','letf=0,top=0,width=1,height=1,toolbar=0,scrollbars=0,status=0,menubar=0');
     WinPrint.document.write(prtContent.innerHTML);
     WinPrint.document.close();
     WinPrint.focus();
     WinPrint.print();
     WinPrint.close();
     //prtContent.innerHTML=strOldOne;
	 
    }
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
    <div><input type="button" class="smallBtn"  id="plus" value="+" />
    &nbsp;&nbsp;<input type="button" class="smallBtn" id="mins" value="-" />
    &nbsp;&nbsp;&nbsp;&nbsp;
    <a id="downPdf" runat="Server" target="_blank" >PDF下载</a>
    </div>
    <div id="imgHold" style="width:100%;height:100%">
    <asp:Image ID="imgBerath"  runat="Server"  Width="90%" Height="90%"   />
    </div>
    </form>
</body>
</html>
