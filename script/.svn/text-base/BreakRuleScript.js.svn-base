// JScript 文件
function $(id) {
    return document.getElementById(id);
}

var trInnerHTML = '<td style="width:100px"><input name="StartTime" onpropertychange="dateChange(this);" style="width: 80px" class="Wdate" onclick="WdatePicker({dateFmt:\'MM-dd HH:mm\'})" />\</td ><td style="width:100px" ><select name="ddlType" onpropertychange="typeChange(this);" style="width:80px"><option selected="selected"></option><option>刮擦</option><option>碰撞</option></select></td><td style="width:600px"><input name="Record" onchange="recordChange(this);" type="text" style="width:580px" /></td><td style="width:100px" ><input type="button" onclick="btnDelClick(this);" value="删除" style="width:80px" /></td>';
var g_rowCount;
//日期更改
function dateChange(object) {
    if(object.value=="")
    {
        
    }
    else{
        addNewRow(object);
    }
}


//事故类型更改
function  typeChange(object) {

    if(object.selectedIndex != 0)
    {
        addNewRow(object);
    }
}
//记录更改
function recordChange(object) {
    if (object.value.length != 0)
    {
        addNewRow(object);
    }
}

//删除记录
function btnDelClick(object) {
    deleteRow(object);
}

//添加新的行
function addNewRow(object ) {

    var tab = $("conTab");
    var rowIndex = object.parentElement.parentElement.rowIndex;
    
    //如果是最后一行就添加一个新行
    if(rowIndex +1 == tab.rows.length)
    {
        
        var row = tab.insertRow(tab.rows.length);
        row.insertCell(0).innerHTML ='<label style="width:40px;font-weight:bold">'+(tab.rows.length-1)+'</label>';
        row.insertCell(1).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" style="width:90px" />';
        row.insertCell(2).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" style="width:90px" />';
        row.insertCell(3).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" style="width:70px" />';
        row.insertCell(4).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" style="width:70px" />';
        row.insertCell(5).innerHTML ='<select name="ddlType" onpropertychange="typeChange(this);" style="width:100px"><option selected="selected"></option><option>交通违章</option>\
                            <option>违章指挥</option>\
                            <option>违章作业</option>\
                            <option>违反劳动纪律</option></select>';
        row.insertCell(6).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" style="width:330px" />';
        row.insertCell(7).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" style="width:120px" />';
        row.insertCell(8).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" style="width:70px" />';
        row.insertCell(9).innerHTML ='<input type="button" onclick="btnDelClick(this);" value="删除" style="width:80px" />';
        
        row.cells[0].style.textAlign="center";
    }
}

//添加新的行
function addNewRowWithValue( no,name,depart,yard,machine,type,content,result,fines ) {

    var tab = $("conTab");
        var row = tab.insertRow(g_rowCount);
        row.insertCell(0).innerHTML ='<label style="width:40px;font-weight:bold">'+no+'</label>';
        row.insertCell(1).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" style="width:90px" value="'+name+'" />';
        row.insertCell(2).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" style="width:90px" value="'+depart+'" />';
        row.insertCell(3).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" style="width:70px" value="'+yard+'" />';
        row.insertCell(4).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" style="width:70px" value="'+machine+'"/>';
        row.insertCell(5).innerHTML="";
        row.insertCell(6).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" style="width:330px" value="'+content+'" />';
        row.insertCell(7).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" style="width:120px" value="'+result+'"/>';
        row.insertCell(8).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" style="width:70px" value="'+fines+'"/>';
        row.insertCell(9).innerHTML ='<input type="button" onclick="btnDelClick(this);" value="删除" style="width:80px" />';
        row.cells[0].style.textAlign="center";
      var ddlHtml = '<select name="ddlType" onpropertychange="typeChange(this);" style="width:100px"><option selected="selected"></option><option>交通违章</option>\
                            <option>违章指挥</option>\
                            <option>违章作业</option>\
                            <option>违反劳动纪律</option></select>';
      if ( ddlHtml.indexOf(type) != -1){
        
           ddlHtml = ddlHtml.replace('<option selected="selected">','<option>');
           row.cells[5].innerHTML = ddlHtml.replace('<option>'+type+'</option>','<option selected="selected">'+type+'</option>');
           //alert(row.cells[1].innerHTML);
        }else if (type.length == 0){
            row.cells[5].innerHTML = ddlHtml;
        } else{
            ddlHtml = ddlHtml.replace('<option selected="selected">','<option></option><option selected="selected">'+type);
            row.cells[5].innerHTML = ddlHtml;
        }
}

//删除行
function deleteRow(object) {
    var tab = $("conTab");
    var rowIndex = object.parentElement.parentElement.rowIndex;
    if (tab.rows.length > 2 && rowIndex!= (tab.rows.length-1)){
        tab.deleteRow(rowIndex);
    }
}

//保存按钮的客户端事件
function btnClientClikc(){
   var tab = $("conTab");
    var length = tab.rows.length;
    var hdVlue = "";
   for(var i = 1; i < length-1; ++i){
        hdVlue += tab.rows[i].cells[0].childNodes[0].innerHTML+",";
        hdVlue += tab.rows[i].cells[1].childNodes[0].value+",";
        hdVlue += tab.rows[i].cells[2].childNodes[0].value+",";
        hdVlue += tab.rows[i].cells[3].childNodes[0].value+",";
        hdVlue += tab.rows[i].cells[4].childNodes[0].value+","
        hdVlue += tab.rows[i].cells[5].childNodes[0].options[tab.rows[i].cells[5].childNodes[0].selectedIndex].text+",";
        hdVlue += tab.rows[i].cells[6].childNodes[0].value+","
        hdVlue += tab.rows[i].cells[7].childNodes[0].value+","
        hdVlue += tab.rows[i].cells[8].childNodes[0].value+"|";
   }
   $("hdValue").value = hdVlue;
   //alert(hdVlue);
}

function bodyLoad() {
   var value = $("hdValue").value;
   var arrValue = value.split('|');
   var tab = $("conTab");
   g_rowCount = 1;
   tab.deleteRow(g_rowCount);
   for(var i = 0; i < arrValue.length-1; ++i){
    var items = arrValue[i].split(',');
        //alert(items);
        addNewRowWithValue(items[0],items[1],items[2],items[3],items[4],items[5],items[6],items[7],items[8])
        g_rowCount = g_rowCount+1;
   }
   $("hdValue").value = "";
        var row = tab.insertRow(tab.rows.length);
        row.insertCell(0).innerHTML ='<label style="width:40px;font-weight:bold">'+(tab.rows.length-1)+'</label>';
        row.insertCell(1).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" style="width:90px" />';
        row.insertCell(2).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" style="width:90px" />';
        row.insertCell(3).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" style="width:70px" />';
        row.insertCell(4).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" style="width:70px" />';
        row.insertCell(5).innerHTML ='<select name="ddlType" onpropertychange="typeChange(this);" style="width:100px"><option selected="selected"></option><option>交通违章</option>\
                            <option>违章指挥</option>\
                            <option>违章作业</option>\
                            <option>违反劳动纪律</option></select>';
        row.insertCell(6).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" style="width:330px" />';
        row.insertCell(7).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" style="width:120px" />';
        row.insertCell(8).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" style="width:70px" />';
        row.insertCell(9).innerHTML ='<input type="button" onclick="btnDelClick(this);" value="删除" style="width:80px" />';
        row.cells[0].style.textAlign="center";
}



