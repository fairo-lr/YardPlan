// JScript 文件
var g_urlArray=["Busy_Vessel.aspx","Busy_WorkingType.aspx","Busy_Yard.aspx"]
    function $(id) {
        return document.getElementById(id);
    }
    var g_tabIndex = 1;
    function changTab(index) {
        var table  = $("tabTable");
        $("img"+g_tabIndex).style.visibility = "hidden";
        table.rows[0].cells[g_tabIndex - 1].style.backgroundColor="#006699";
        table.rows[0].cells[g_tabIndex - 1].style.color="White";
        g_tabIndex = index
        table.rows[0].cells[g_tabIndex - 1].style.backgroundColor="#E8F7FC";
        table.rows[0].cells[g_tabIndex - 1].style.color="Black";
        var startTime = $('iptStartTime').value;
        var selectIndex =  $('dllShift').selectedIndex+1;
        //var workno = $('ddlWorkNo').selectedIndex+1;
        //if ($("hdisHasRecord").value=="0"){
        //    $("framePage").src="shift_informationNotSave.aspx";
        //}else{
        //  $("framePage").src=g_urlArray[g_tabIndex-1]+"?date="+startTime+"&shift="+selectIndex+"&workno="+workno+"&random="+Math.random();  
        //};
        $("framePage").src=g_urlArray[g_tabIndex-1]+"?date="+startTime+"&shift="+selectIndex+"&random="+Math.random();  
        $("img"+g_tabIndex).style.visibility = "visible";
        //设置提示块
        $("divTip").style.visibility="visible";
        $("divTip").style.left = ((g_tabIndex - 1)*230 + 80) +"px";

    }
    
function bodyLoad(){
     var table  = $("tabTable");
        table.rows[0].cells[g_tabIndex - 1].style.backgroundColor="#E8F7FC";
        table.rows[0].cells[g_tabIndex - 1].style.color="Black";
        $("iptStartTime").value=$("hdStartTime").value;
        $("ipEndTime").value=$("hdEndTime").value;
        
        var startTime = $('iptStartTime').value;
        var selectIndex =  $('dllShift').selectedIndex+1;
        //var workno = $('ddlWorkNo').selectedIndex+1;
        //if ($("hdisHasRecord").value=="0"){
        //    $("framePage").src="shift_informationNotSave.aspx";
            
        //}else{
        //$("framePage").src="shift_vessel.aspx?date="+startTime+"&shift="+selectIndex+"&workno="+workno;
       // }; 
       $("framePage").src="Busy_vessel.aspx?date="+startTime+"&shift="+selectIndex;
       //$("framePage").src="Default.aspx";
        $("img"+g_tabIndex).style.visibility = "visible";
        
        //设置提示快
        $("divTip").style.visibility="visible";
        $("divTip").style.left = "120px";
        if ($("framePage").attachEvent){
            $("framePage").attachEvent("onload", function(){
                $("img"+g_tabIndex).style.visibility="hidden";
                $("divTip").style.visibility="hidden";

            });
        } else {
            $("framePage").onload = function(){
                $("img"+g_tabIndex).style.visibility="hidden";
                $("divTip").style.visibility="hidden";

            };
        }
    }
