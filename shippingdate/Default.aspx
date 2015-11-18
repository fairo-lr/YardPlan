<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <script type="text/javascript" src="scripts/jquery-1.8.0.min.js"></script>
    <style type="text/css">
    .InTradeVslBck{
        background-color:#99CCFF;
        height:40px;
        position:absolute;
        cursor:pointer
    }
    .ForTradeVslBck{
        background-color:#FF99CC;
        height:15px;
        position:absolute;
        cursor:pointer
    }
    .bollardImg{
        width:1730px;
        border-top:3px solid black;
        border-left:3px solid black;
        border-right:3px solid black;
        height:10px;
        position:absolute;
        top:730px;
        left:80px;
    }
    .bollardNum{
        border-left:3px solid black;
        width:1730px;
        position:absolute;
        top:740px;
        left:80px;
    }
    .bollarDistan1{
        width:1730px;
        position:absolute;
        top:760px;
        left:80px;
    }
    .bollarDistan2{
        width:1730px;
        position:absolute;
        top:780px;
        left:80px;
    }
    .bollardItem1{
        border-right:2px solid Black;
        width:10px;
        height:10px;
        line-height:10px;
        float:left;
    }
    .bollardItem2{
        border-right:1px solid Black;
        width:10px;
        height:5px;
        line-height:5px;
        float:left;
    }
    .bollardItem3{
        border-right:2px solid Red;
        width:10px;
        height:10px;
        line-height:10px;
        float:left;
    }
    .bollardName1{
        float:left;
        width:23px;
        font-size:11px;
        font-weight:bold;
        text-align:center;
    }
    .bollardName2{
        float:left;
        width:23px;
        font-size:11px;
        font-weight:bold;
        text-align:center;
        color:red;
    }
    .bollardDis1{
        float:left;
        width:46px;
        font-size:xx-small;
        text-align:center;
    }
    .bollardDis2{
        float:left;
        width:46px;
        font-size:xx-small;
        text-align:center;
        color:red;
    }
    .weekRad{
        border-left:3px solid black;
        border-bottom:3px solid black;        
        width:20px;
        _widht:20px;
        height:710px;
        position:absolute;
        top:0px;
        left:40px;
    }
    .weekDay{
        border-top:2px solid black;
        height:100px;
        width:20px;
        float:left;
    }
    .weekDay2{
        border-top:2px solid black;
        border-right:3px solid black;
        height:100px;
        width:17px;
        float:left;
    }
    .weekCont{
        border-top:1px solid black;
        width:40px;
        position:relative;
        left:-20px;
    }
    .weekHour1{
        height:15px;
        _height:10px;
        width:17px;
        margin:0px;
        padding:0px;
        font-size:small;
    }
    .weekHour2{
        border-top:1px solid black;
        height:15px;
        _heigt:10px;
        width:17px;
        margin:0px;
        padding:0px;
        font-size:small;
    }
    .vslTailLeft{
        height:10px;
        width:10px;
        border-right :3px solid black;
        position:absolute;
        margin:0px;
        padding:0px;
        font-size:0px;            
    }
    .vslHeadLeft{
        height:0px;
        width:0px;
        ling-height:0px;
        left:0px;
        border-left:5px dashed transparent;
        border-top:5px dashed transparent;
        border-right:5px solid black;
        border-bottom:5px dashed transparent;
        position:absolute;  
        margin:0px;
        padding:0px;
        font-size:0px;                
    }
    .vslTailRight{
        height:10px;
        width:10px;
        left:0px;
        border-left :3px solid black;
        position:absolute;
        margin:0px;
        padding:0px;
        font-size:0px;           
    }
    .vslHeadRight{
        height:0px;
        width:0px;
        ling-height:0px;
        border-left:5px solid black;
        border-top:5px dashed transparent;
        border-right:5px dashed transparent;
        border-bottom:5px dashed transparent;
        position:absolute;  
        margin:0xp;
        padding:0px;
        font-size:0px;               
    }
    #tip{
        width:300px;
        background-color:#FFFF99;
        position:absolute;
        z-index:1000;
    }
    #tip #vslInfo{
        list-style:none;
    }
    </style>
    <script type="text/javascript">
    var g_weekDay = ["星期日","星期一","星期二","星期三","星期四","星期五","星期六"];
    var g_vlengUnit = 1.2;
    var g_rectangleArr;
    var g_forVslHieght = 45;
    var g_intVslHieght = 15;
    $(document).ready(function () {
        $("#tip").hide();
        var $bollardLst = "";
        var $boolardNamelst = "";
        var $boolardDiss1 = "";
        var $boolardDiss2 = "";        
        var t_revser = false;
        var t_nums = 75;
        var t_disIndex = 0;
        /****************************************************************
        **生成X轴
        *****************************************************************/
        for(var i = 0; i < 150; i++){
            if (i%2==0){
                $bollardLst +=(t_revser?"<div class='bollardItem1'></div>":"<div class='bollardItem3'></div>");
                $boolardNamelst +=(t_revser?"<div class='bollardName1'>"+(t_nums--)+"</div>":"<div class='bollardName2'>"+(t_nums--)+"</div>");
                if (i==0){
                    $boolardDiss1 += (t_revser?"":"<div style='width:12px;float:left'>&nbsp</div><div class='bollardDis1'>"+(1224.2-(t_disIndex)*33.3).toFixed(1)+"</div>");                    
                    $boolardDiss2 += (t_revser?"":"<div style='width:34px' class='bollardDis2'>"+(1241.2-(t_disIndex++)*33.3).toFixed(1)+"</div>");
                }else if (i == 148){
                    $boolardDiss1 += (t_revser?"<div style='width:15px' class='bollardDis1'>"+(1224.2-(t_disIndex)*33.3).toFixed(1)+"</div>":"");                    
                    $boolardDiss2 += (t_revser?"":"<div style='width:40px' class='bollardDis2'>"+(1241.2-(t_disIndex++)*33.3).toFixed(1)+"</div>");
                }else{
                    $boolardDiss1 += (t_revser?"":"<div class='bollardDis1'>"+(1224.2-(t_disIndex)*33.3).toFixed(1)+"</div>");                                    
                    $boolardDiss2 += (t_revser?"":"<div  class='bollardDis2'>"+(1241.2-(t_disIndex++)*33.3).toFixed(1)+"</div>");  
                }          
                t_revser = ~t_revser;
            }else{
                $bollardLst +="<div class='bollardItem2'></div>";
            }
        }
        $(".bollardImg").append($bollardLst);
        $(".bollardNum").append($boolardNamelst);
        $(".bollarDistan1").append($boolardDiss1);
        $(".bollarDistan2").append($boolardDiss2);
        /****************************************************************
        **生成Y轴
        *****************************************************************/
        var $t_lstWeek = "";
        var t_now = new Date();
        var t_firseIndex = t_now.getDay()==0?6:t_now.getDay()-1
        for(var j = 0; j < 7; j++){
            //$t_lstWeek += "<div class='weekCont'><div class='weekDay' style='float:left'>"+g_weekDay[t_firseIndex]+"</div><div class='weekDay'><div class='weekHour1'><label>24</label></div><div class='weekHour2'><label>20</label></div><div class='weekHour2'>16</div><div class='weekHour2'>12</div><div class='weekHour2'>8</div><div class='weekHour2'>4</div></div></div>"
            $t_lstWeek += "<div class='weekCont'><div class='weekDay' style='float:left'>"+g_weekDay[t_firseIndex]+"</div><div class='weekDay2'><div class='weekHour1'>24</div><div class='weekHour2'>20</div><div class='weekHour2'>16</div><div class='weekHour2'>12</div><div class='weekHour2'>8</div><div class='weekHour2'>4</div></div></div>" ;        
            t_firseIndex = (--t_firseIndex)<0?6:t_firseIndex;
        }
        $(".weekRad").append($t_lstWeek).find(".weekDay2").find("div:even").css("color","Red");
        /****************************************************************
        **获取船舶数据
        *****************************************************************/
        $.getJSON("GetLstVslHandler.ashx",{},function (re,st) {
                    g_rectangleArr = new Array();
            for(var l =0; l < re.length; l++){
                var rectangle= new Object();
                rectangle.Xpst = 1730-re[l].EdPost*1.3885+80/*偏移量*/;
                rectangle.Ypst = (42-re[l].DayOrder/4)*16.9;
                rectangle.XLength = re[l].Vlength*g_vlengUnit;
                rectangle.YHeight = re[l].Vlength > 150?g_forVslHieght:g_intVslHieght;
                re[l].rectangle = rectangle;
               
                DrawVessels(re[l]);
            }
        })
        
    })
    /***********************************************************************
    **将船舶绘出来
    ***********************************************************************/
    function DrawVessels(vsl){
        //var $vslDiv = $("<div id='"+vsl.Vname+"'><div id='vTail'></div><div id='vHead'></div><nobr></norb></div>");
        var $vslDiv = $("<div id='"+vsl.Vname+"'><div id='vTail'></div><div id='vHead'></div><div id='content'></div></div>");        
        vsl.isMoved = false;
        vsl.rectangle.Ypst = vsl.rectangle.Ypst-(vsl.Vlength > 150?g_forVslHieght:g_intVslHieght);//船体宽度的偏移
        while(IsIntersecting(vsl.rectangle,g_rectangleArr)){
            vsl.rectangle.Ypst = vsl.rectangle.Ypst-g_intVslHieght;
            vsl.isMoved = true;
            //alert(vsl.VCHname);
        }
        
        $vslDiv.addClass(vsl.Intrade=="N"?"ForTradeVslBck":"InTradeVslBck")
        .css("width",vsl.rectangle.XLength+"px")
        .css("height",vsl.rectangle.YHeight+"px")
        .find("#content").html(vsl.Vlength > 150?vsl.Voyage+" "+vsl.VCHname+" "+vsl.Vname+" "+vsl.Vlength+"M"+"</br>"+vsl.SlnName+"   LP:"+vsl.PrePort+"   NP:"+vsl.NextPort:vsl.VCHname+"  "+vsl.Vlength+"M").end()
        .css("top",vsl.rectangle.Ypst+"px")
        .css("left",vsl.rectangle.Xpst+"px");
        g_rectangleArr.push(vsl.rectangle);
        var $divTail = $("<div id='vTail'></div>");
        var $divHead = $("<div id='vHead'></div>");
        //$vslDiv.append("<div id='vTail'></div>");//.append($divHead);
        if (vsl.BreDire == "R"){
            $vslDiv.find("#vTail").addClass("vslTailRight")
            .css("top",vsl.Vlength>150?"35px":"5px");
            $vslDiv.find("#vHead").addClass("vslHeadRight")
            .css("left",vsl.rectangle.XLength-10+"px")
            .css("top",vsl.Vlength>150?"35px":"5px");
        }else{
            $vslDiv.find("#vTail").addClass("vslTailLeft")
            .css("left",vsl.rectangle.XLength-13+"px")
            .css("top",vsl.Vlength>150?"35px":"5px");
            $vslDiv.find("#vHead").addClass("vslHeadLeft")
            .css("top",vsl.Vlength>150?"35px":"5px");
        }
        //$vslDiv.find("nobr").html(vsl.isMoved?$vslDiv.html()+"  *":$vslDiv.html()); 
        $vslDiv.find("#content")
        .css("white-space","nowrap")
        .css("text-overflow","ellipsis")
        .css("overflow","hidden")
        .css("width","100%")
        .html(vsl.isMoved?$vslDiv.find("#content").html()+"  *":$vslDiv.find("#content").html()); 
        
        $vslDiv.hover(function (event) {
            //alert(vsl.Vname);
            $("#vslInfo").find("li").remove();
            $("#vslInfo").append("<li>船名："+vsl.Vname+"</li>")
                        .append("<li>航次："+vsl.Voyage+"</li>")
                        .append("<li>船长："+vsl.Vlength+"</li>")
                        .append("<li>起始码："+vsl.StPost+"</li>")
                        .append("<li>结束码："+vsl.EdPost+"</li>")
                        .append("<li>靠泊方向："+vsl.BreDire+"</li>")
                        .append("<li>靠泊时间："+vsl.BreTm+"</li>")
                        .append("<li>内外贸："+vsl.Intrade+"</li>")
                        .append("<li>航线名称："+vsl.SlnName+"</li>")
                        .append("<li>上一港："+vsl.PrePort+"</li>")
                        .append("<li>下一港："+vsl.NextPort+"</li>")
                        .append("<li>重叠移动："+(vsl.isMoved?"Y":"N"));
                        //alert(event.pageX+"----"+event.pageY);
            $("#tip")
            .css("left",(event.pageX+300>2010?event.pageX-200:event.pageX+2)+"px")
            .css("top",(event.pageY+250>810?event.pageY-200:event.pageY)+"px")
            .show();
            
        },function () {
            $("#tip").hide();
        });
        $("#divBody").append($vslDiv);
    }
    /**********************************************************************
    **重叠检测
    ***********************************************************************/
    function IsIntersecting(p_rec,p_recArray){
        var t_centerX = p_rec.Xpst + p_rec.XLength/2.0;
        var t_centerY = p_rec.Ypst + p_rec.YHeight/2.0;
        for(var i = 0; i < p_recArray.length; ++i){
            var t_EXcenterX = p_recArray[i].Xpst + p_recArray[i].XLength / 2.0;
            var t_EXcenterY = p_recArray[i].Ypst + p_recArray[i].YHeight / 2.0;
            if ((Math.abs(t_centerX - t_EXcenterX) <= p_rec.XLength/ 2.0 + p_recArray[i].XLength / 2.0 && Math.abs(t_EXcenterY - t_centerY) <= p_rec.YHeight / 2.0 + p_recArray[i].YHeight / 2.0)){
                return true;
            }
        }
        return false;
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divBody" style=" " >
        <div id="tip">
            <ul id="vslInfo">
            </ul>
        </div>
        <div class="weekRad"></div>
        <div class="bollardImg"></div>
        <div class="bollardNum"></div>
        <div class="bollarDistan1"></div>
        <div class="bollarDistan2"></div>
    </div>
    </form>
</body>
</html>
