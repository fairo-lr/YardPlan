// JScript 文件
function $(id) {
    return document.getElementById(id);
}

var trInnerHTML = '<td style="width:100px">'+
'<input name="StartTime" onpropertychange="dateChange(this);"'+
' style="width: 80px" class="Wdate" onclick="WdatePicker({dateFmt:\'MM-dd HH:mm\'})" />\</td >'+
'<td style="width:100px" ><select name="ddlType" onpropertychange="typeChange(this);" style="width:80px">'+
'<option selected="selected"></option><option>刮擦</option><option>碰撞</option></select></td>'+
'<td style="width:520px"><input name="Record" onchange="recordChange(this);"'+
' type="text" style="width:500px" /></td>'+
'<td style="width:120px"><input type="text" readonly="readonly"  style="width:50px; position:relative; left:3px;z-index:1000" /><input style="width:75px; position:relative;z-index:0 ;right:0px " name="Picture3" onchange="recordChange(this);showURL(this)" type="file"  /></td>'+
'<td style="width:120px"><input type="text" readonly="readonly"  style="width:50px; position:relative; left:3px;z-index:1000" /><input style="width:75px; position:relative;z-index:0 ;right:0px " name="Picture3" onchange="recordChange(this);showURL(this)" type="file"  /></td>'+
'<td style="width:120px"><input type="text" readonly="readonly"  style="width:50px; position:relative; left:3px;z-index:1000" /><input style="width:75px; position:relative;z-index:0 ;right:0px " name="Picture3" onchange="recordChange(this);showURL(this)" type="file"  /></td>'+
'<td style="width:100px" >'+
'<input type="button" onclick="btnDelClick(this);" value="删除" style="width:80px" /></td>';
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
        row.insertCell(0).innerHTML ='<input name="StartTime"  onblur="dateChange(this);" style="width: 80px" class="Wdate" onclick="WdatePicker({dateFmt:\'MM-dd HH:mm\'})" />';
        row.insertCell(1).innerHTML ='<select name="ddlType" onpropertychange="typeChange(this);" style="width:80px"><option selected="selected"></option><option>机损事故</option>\
                            <option>箱损事故</option>\
                            <option>船损事故</option>\
                            <option>货损事故</option>\
                            <option>设施损坏事故</option>\
                            <option>交通事故</option>\
                            <option>业务差错</option>\
                            <option>工伤事故</option>\
                            <option>事故苗子</option></select>';
        row.insertCell(2).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" style="width:500px" />';       
        row.insertCell(3).innerHTML='<input type="text" readonly="readonly" style="width:50px; position:relative; left:3px;z-index:1000" /><input style="width:75px; position:relative;z-index:0 ;right:0px " name="Picture1" onchange="recordChange(this);showURL(this)" type="file" />';
        row.insertCell(4).innerHTML='<input type="text" readonly="readonly" style="width:50px; position:relative; left:3px;z-index:1000" /><input style="width:75px; position:relative;z-index:0 ;right:0px " name="Picture2" onchange="recordChange(this);showURL(this)" type="file"  />';
        row.insertCell(5).innerHTML='<input type="text" readonly="readonly" style="width:50px; position:relative; left:3px;z-index:1000" /><input style="width:75px; position:relative;z-index:0 ;right:0px " name="Picture3" onchange="recordChange(this);showURL(this)" type="file"  />';
        row.insertCell(6).innerHTML ='<input type="button" onclick="btnDelClick(this);" value="删除" style="width:80px" />';
    }
}

function showURL(obj)
{    
//    alert( obj.value);
//   alert(obj.value.filename );
//  alert(basename($upfile)); 
//   alert(obj.filename.value); 
 //   alert(obj.value.lastIndexOf("\\"));

    obj.parentNode.firstChild.value=obj.value.substr(obj.value.lastIndexOf("\\")+1,obj.value.length-obj.value.lastIndexOf('\\')-1);
    //alert(obj.parentNode.firstChild.value);
    obj.parentNode.firstChild.title = obj.value;
//   var tab = $("conTab");
//    var length = tab.rows.length;    
//  document 
//   for(var i = 1; i < length-1; ++i){       
//        alert(tab.rows[i].cells[3].value); 
//        alert(tab.rows[i].cells[4].value); 
//        alert(tab.rows[i].cells[5].value); 
//    }
   // alert("showUrl");
}


//添加新的行
function addNewRowWithValue( happTM,type,record ,picture,picture2,picture3) {

    //alert(picture);
   //alert(picture.substring( 15));
    var tab = $("conTab");
        var row = tab.insertRow(g_rowCount);
        row.insertCell(0).innerHTML ='<input name="StartTime"  onblur="dateChange(this);" style="width: 80px" class="Wdate" value="'+happTM+'" onclick="WdatePicker({dateFmt:\'MM-dd HH:mm\'})" />';
        row.insertCell(1).innerHTML ='';
        row.insertCell(2).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" value="'+record+'" style="width:500px" />';
        row.insertCell(3).innerHTML='<input type="text" readonly="readonly" value="'+picture.substring( 15)+'" title="'+picture.substring( 15)+'" style="width:50px; position:relative; left:3px;z-index:1000" /><input style="width:75px; position:relative;z-index:0 ;right:0px " name="Picture3" onchange="recordChange(this);showURL(this)" type="file"  />';
        row.insertCell(4).innerHTML='<input type="text" readonly="readonly" value="'+picture2.substring( 15)+'"  title="'+picture2.substring( 15)+'" style="width:50px; position:relative; left:3px;z-index:1000" /><input style="width:75px; position:relative;z-index:0 ;right:0px " name="Picture3" onchange="recordChange(this);showURL(this)" type="file"  />';
        row.insertCell(5).innerHTML='<input type="text" readonly="readonly" value="'+picture3.substring( 15)+'"  title="'+picture3.substring( 15)+'" style="width:50px; position:relative; left:3px;z-index:1000" /><input style="width:75px; position:relative;z-index:0 ;right:0px " name="Picture3" onchange="recordChange(this);showURL(this)" type="file"  />';
     row.insertCell(6).innerHTML ='<input type="button" onclick="btnDelClick(this);" value="删除" style="width:80px" />';
      var ddlHtml = '<select name="ddlType" onpropertychange="typeChange(this);" style="width:80px"><option selected="selected"></option><option>机损事故</option>\
                            <option>箱损事故</option>\
                            <option>船损事故</option>\
                            <option>货损事故</option>\
                            <option>设施损坏事故</option>\
                            <option>交通事故</option>\
                            <option>业务差错</option>\
                            <option>工伤事故</option>\
                            <option>事故苗子</option></select>';
      if ( ddlHtml.indexOf(type) != -1){
        
           ddlHtml = ddlHtml.replace('<option selected="selected">','<option>');
           row.cells[1].innerHTML = ddlHtml.replace('<option>'+type+'</option>','<option selected="selected">'+type+'</option>');
           //alert(row.cells[1].innerHTML);
        }else if (type.length == 0){
            row.cells[1].innerHTML = ddlHtml;
        } else{
            ddlHtml = ddlHtml.replace('<option selected="selected">','<option></option><option selected="selected">'+type);
            row.cells[1].innerHTML = ddlHtml;
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

   // alert('xxx');
   var tab = $("conTab");
    var length = tab.rows.length;
    var hdVlue = "";
   //alert(length); 
   //alert("sssss");  
  
  //alert(document.getElementByID("hdValue").value);
  //alert("dddd");
  document 
   for(var i = 1; i < length-1; ++i){       
        hdVlue += tab.rows[i].cells[0].childNodes[0].value+",";       
        hdVlue += tab.rows[i].cells[1].childNodes[0].options[tab.rows[i].cells[1].childNodes[0].selectedIndex].text+",";
       // alert(tab.rows[i].cells[1].childNodes[0].options[tab.rows[i].cells[1].childNodes[0].selectedIndex].text+",");
        hdVlue += tab.rows[i].cells[2].childNodes[0].value+",";           
        //alert(tab.rows[i].cells[3].value+",");        
        var p =tab.rows[i].cells[3];
        hdVlue += tab.rows[i].cells[3].value +",";
        hdVlue += tab.rows[i].cells[4].value+",";
        hdVlue += tab.rows[i].cells[5].value+"|" ;                            

   }
   $("hdValue").value = hdVlue;

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
        addNewRowWithValue(items[0],items[1],items[2],items[3],items[4],items[5])
        g_rowCount = g_rowCount+1;
   }
   $("hdValue").value = "";
   var row = tab.insertRow(tab.rows.length);
        row.insertCell(0).innerHTML ='<input name="StartTime"  onblur="dateChange(this);" style="width: 80px" class="Wdate" onclick="WdatePicker({dateFmt:\'MM-dd HH:mm\'})" />';
        row.insertCell(1).innerHTML ='<select name="ddlType" onpropertychange="typeChange(this);" style="width:80px"><option selected="selected"></option><option>机损事故</option>\
                            <option>箱损事故</option>\
                            <option>船损事故</option>\
                            <option>货损事故</option>\
                            <option>设施损坏事故</option>\
                            <option>交通事故</option>\
                            <option>业务差错</option>\
                            <option>工伤事故</option>\
                            <option>事故苗子</option></select>';
        row.insertCell(2).innerHTML ='<input name="Record" onchange="recordChange(this);" type="text" style="width:500px" />';       
        row.insertCell(3).innerHTML='<input type="text" readonly="readonly" style="width:50px; position:relative; left:3px;z-index:1000" /><input style="width:75px; position:relative;z-index:0 ;right:0px " name="Picture1" onchange="recordChange(this);showURL(this)" type="file" />';
        row.insertCell(4).innerHTML='<input type="text" readonly="readonly" style="width:50px; position:relative; left:3px;z-index:1000" /><input style="width:75px; position:relative;z-index:0 ;right:0px " name="Picture2" onchange="recordChange(this);showURL(this)" type="file"  />';
        row.insertCell(5).innerHTML='<input type="text" readonly="readonly" style="width:50px; position:relative; left:3px;z-index:1000" /><input style="width:75px; position:relative;z-index:0 ;right:0px " name="Picture3" onchange="recordChange(this);showURL(this)" type="file"  />';
        row.insertCell(6).innerHTML ='<input type="button" onclick="btnDelClick(this);" value="删除" style="width:80px" />';
}



