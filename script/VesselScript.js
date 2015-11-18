// JScript 文件
function $(id) {
    return document.getElementById(id);
}
var g_insertCount="";
function addAnotherVessle() {
            //if ($("hdCount").value == "0")
            //    $("labMessage").style.visibility="hidden";
            
            var tab = $("conTab");
            var rowCount = tab.rows.length;
           
            //记录插入的位置
            g_insertCount += rowCount+";";
            var row = tab.insertRow(rowCount);
            if (rowCount%2 == 0)
                row.style.backgroundColor="#E0E0E0";
            else
                row.style.backgroundColor="white";
            
            row.insertCell(0).innerHTML ='<input type="text"  style="width:70px" value="" />';
            row.insertCell(1).innerHTML ='<input type="text" style="width:34px" value="" />';
            row.insertCell(2).innerHTML ='<input type="text"  style="width:90px" value="" />';
            row.insertCell(3).innerHTML ='<input style="width:60px" id="abtTime"  class="Wdate" onclick="WdatePicker({dateFmt:\'MMdd HHmm\',qsEnabled:false})" />';
            row.insertCell(4).innerHTML ='<input style="width:60px" id="adpTime" class="Wdate" onclick="WdatePicker({dateFmt:\'MMdd HHmm\',qsEnabled:false})" />';
            row.insertCell(5).innerHTML ='<input style="width:60px" id="startWorkTime" class="Wdate" onclick="WdatePicker({dateFmt:\'MMdd HHmm\',qsEnabled:false})" />';
            row.insertCell(6).innerHTML ='<input style="width:60px" id="endWorkTime" class="Wdate" onclick="WdatePicker({dateFmt:\'MMdd HHmm\',qsEnabled:false})" />';
            row.insertCell(8).innerHTML ='<input type="text"  style="width:90px" value="" />';
            row.insertCell(9).innerHTML ='<input type="text"  style="width:90px" value="" />';
            row.insertCell(10).innerHTML ='<input style="width:70px" id="robetime"  class="Wdate" onclick="WdatePicker({dateFmt:\'MMdd HHmm\',qsEnabled:false})" />';
            row.insertCell(11).innerHTML ='<input style="width:70px" id="ladertime"  class="Wdate" onclick="WdatePicker({dateFmt:\'MMdd HHmm\',qsEnabled:false})" />';
            row.insertCell(12).innerHTML ='<input style="width:70px" id="spectarrtime"  class="Wdate" onclick="WdatePicker({dateFmt:\'MMdd HHmm\',qsEnabled:false})" />';
            row.insertCell(13).innerHTML ='<input style="width:70px" id="spectuptime"  class="Wdate" onclick="WdatePicker({dateFmt:\'MMdd HHmm\',qsEnabled:false})" />';
            row.insertCell(14).innerHTML ='<input style="width:70px" id="spectdowntime"  class="Wdate" onclick="WdatePicker({dateFmt:\'MMdd HHmm\',qsEnabled:false})" />';
            row.insertCell(15).innerHTML ='<input style="width:70px" id="workeruptime"  class="Wdate" onclick="WdatePicker({dateFmt:\'MMdd HHmm\',qsEnabled:false})" />';
            row.insertCell(16).innerHTML ='<input style="width:70px" id="workeruptime"  class="Wdate" onclick="WdatePicker({dateFmt:\'MMdd HHmm\',qsEnabled:false})" />';
            //alert("xxx");
          //alert(row.cells.length);
            for(var i =0; i < row.cells.length;++i){
                //row.cells[i].style="height:13px;rules:cols;border:1px solid #CCCCCC;"
                row.cells[i].style.height="13px";
                row.cells[i].style.rules="cols";
                row.cells[i].style.borderBottom="1px solid #b2c9d3";
                row.cells[i].style.borderRight="1px solid #b2c9d3";
                
            }
            
            //设置DIV的高度
            
            var height = $("divOvery").style.height;
            
            height = height.substring(0,height.length-2);
            alert(height);
            $("divOvery").style.height = (parseInt(height)+30)+"px";
            
        }
function hideDateSel(){
    $dp.hide();
}

function myonpicked(dp) {
    alert('xxxxx');
}

function btnSaveClientClick() {
    var tab = $("conTab");
    var hdValue="";
    var rowCount = tab.rows.length;
    for ( var i = 1; i < rowCount; ++i){
        //if (g_insertCount.indexOf(i) == -1){
            for (var k = 0; k <= 6; ++k){
                hdValue += tab.rows[i].cells[k].childNodes[0].innerHTML+",";
            }
                        
            for (var k = 7; k <= 15; ++k){
                hdValue += tab.rows[i].cells[k].childNodes[0].value+",";
            }
            hdValue += tab.rows[i].cells[12].childNodes[0].value;
            hdValue +="|";
            //alert(hdValue);
        //}
    }
        //查看是否有插入新的记录
//    if(g_insertCount!=""){
//        //alert(g_insertCount);
//        var inserRecord = g_insertCount.split(';');
//        var tab = $("conTab");
//        for ( var i = 0; i < inserRecord.length -1 ; ++i){
//            for(var j = 0; j <= 19;++j){
//                hdValue += tab.rows[inserRecord[i]].cells[j].childNodes[0].value+",";
////                        hdValue += tab.rows[inserRecord[i]].cells[j].innerHTML+",";
//            }
//            hdValue +="Y"
//            hdValue +="|";
//        }
//        //alert(hdValue );
//    }
    
    $("hdTableValue").value=hdValue;
    return true;
}