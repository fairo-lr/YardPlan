//弹出框的基本配置
var g_DialogAreaModel = {
    position: [500, 200],
    resizable: false,
    width: 250,
    height: 350,
    overlay: {
        backgroundColor: '#000',
        opacity: 0.5
    },
    buttons: {
        "清空": function () { InitialBay($(this)); },
        "关闭": function () { $(this).dialog('destroy'); },
        "保存": function () { UpdateRainbowData($(this)); }

    }
};

var g_DialogShipModel = {
    position: [500, 200],
    resizable: false,
    width: 300,
    height: 430,
    overlay: {
        backgroundColor: '#000',
        opacity: 0.5
    },
    buttons: {
        "保存": function () { UpdateShipLineData($(this)); },
        "关闭": function () { $(this).dialog('destroy'); }
    }
};

//箱区列表
//如果新增箱区，需要在这里添加箱区名称
var g_AreaNames = new Array("E1", "E2", "E3", "E4", "E5", "E6", "ED", "EC", "D1", "D2", "D3", "D4",
"D5", "D6", "DD", "DC", "C1", "C2", "C3", "C4", "C5", "C6", "CD", "CC", "B1", "B2", "B3", "B4", "B5", "A1");

var areaCookie;

function SetCookie(area, bay, squeue, lnecd, ldunldport1, ldunldport2, height, size) {
    this.area = area;
    this.bay = bay;
    this.squeue = squeue;
    this.lnecd = lnecd;
    this.port1 = ldunldport1;
    this.port2 = ldunldport2;
    this.height = height;
    this.size = size;
    return this;
}
/****************************************************/
$(document).ready(function () {
    //InitialArea();
    SetShipLineInfo(); //初始化航线信息

    for (var i = 0; i < g_AreaNames.length; i++) {
        GetAreaTable(g_AreaNames[i]); //初始化箱区信息
    }

    $(".area").find("td").live('dblclick', function () {
        areaCookie = SetCookie($(this).attr("area"), $(this).attr("bay"), $(this).attr("squeue"), $(this).attr("lnecd"), $(this).attr("port1"), $(this).attr("port2"), $(this).attr("height"), $(this).attr("size"));
        $("#dialogArea").dialog($.extend(g_DialogAreaModel, { create: InitDialogArea() }));
    });

    $("#btnShipLineAdd").live('click', function () {
        //增加新航线
        GetShipLineInfo("oracle", null);
        $("#dialogShip").dialog(g_DialogShipModel);
        return false;
    });

    $("#dialogShip_lnecd").live('change', function () {
        //更新卸货港
        var lnecd = $(this).val();
        jQuery.ajax({
            type: "POST",
            url: "YardAnalyse_ashx/YardAnalyse_Get_ShipLinePort.ashx",
            data: { "dbName": "oracle", "lnecd": lnecd },
            success: function (output) {
                if (output != "") {
                    $("#dialogShip_port").empty().append(output);
                }
            }
        });
    });

    $("#dialogArea_lnecd").live('change', function () {
        //更新卸货港
        var lnecd = $(this).val();
        GetShipLinePortInfo("sqlserver", lnecd);
    });

    $(".area-head").live('dblclick', function () {
        var area = $(this).find("th").text();
        if (window.confirm('是否初始化 ' + area + ' 箱区？')) {
            InitialArea(area);
            return true;
        } else {
            return false;
        }

    });

    $(".scroll").find("span").live('click', function () {
        //导航
        var name = $(this).text();
        var pos = $("#" + name).offset();
        $("body,html").animate({ scrollLeft: pos.left,
            scrollTop: pos.top
        }, 0);
    });

    $(".ship-info").find("span").live('dblclick', function () {
        var lnecd = $(this).text();
        if (window.confirm('是否删除 ' + lnecd + ' 航线？')) {
            DeleteShipLineData(lnecd);
            return true;
        } else {
            return false;
        }
    });
});

function DeleteShipLineData(lnecd) {
    jQuery.ajax({
        type: "POST",
        url: "YardAnalyse_ashx/YardAnalyse_Delete_ShipLine.ashx",
        data: { "lnecd": lnecd },
        success: function (output) {
            SetShipLineInfo();
        }
    });
}

function UpdateShipLineData($this) {
    var lnecd = $this.find("#dialogShip_lnecd").val();
    var color = $this.find("#dialogShip_color").val();
    var name = $this.find("#dialogShip_name").val();
    var relate_lnecd = $this.find("#dialogShip_lnecd2").val();
    var ports = "";
    $this.find("#dialogShip_port").find("input").each(function () {
        if ($(this).attr("checked") == "checked") {
            ports += $(this).val() + ",";
        }
    });
    //添加到数据库
    jQuery.ajax({
        type: "POST",
        url: "YardAnalyse_ashx/YardAnalyse_Insert_ShipLine.ashx",
        data: { "lnecd": lnecd, "color": color, "ports": ports, "name": name, "relate_lnecd": relate_lnecd },
        success: function (output) {
            SetShipLineInfo();
            $("#shipmessage").show().hide(2000);
        }
    });
}


function GetShipLineInfo(dbsource, lnecd) {
    //从dbsource数据库中获取航线名，不同数据库对应不同DOM
    jQuery.ajax({
        type: "POST",
        url: "YardAnalyse_ashx/YardAnalyse_Get_ShipLineName.ashx",
        data: { "dbName": dbsource },
        success: function (output) {
            if (output != "<option> </option>") {
                if (dbsource == "sqlserver") {
                    $("#dialogArea_lnecd").empty().append(output).val(lnecd);
                }
                else {
                    $("#dialogShip_lnecd").empty().append(output); 
                    $("#dialogShip_lnecd2").empty().append(output);
                }
            }
        }
    });
}

function GetShipLinePortInfo(dbsource, lnecd, port1, port2) {
    jQuery.ajax({
        type: "POST",
        url: "YardAnalyse_ashx/YardAnalyse_Get_ShipLinePort.ashx",
        data: { "dbName": dbsource, "lnecd": lnecd },
        success: function (output) {
            if (output != "") {
                $("#dialogArea_port1").empty().append(output).val(port1);
                $("#dialogArea_port2").empty().append(output).val(port2);
            }
        }
    });
}

//fairo
InitDialogArea = function () {
    $("#dialogArea_name").text(areaCookie.area + areaCookie.bay);
    $("#dialogArea_height").val(areaCookie.height);
    $("#dialogArea_size").val(areaCookie.size);
    GetShipLineInfo("sqlserver", areaCookie.lnecd);
    GetShipLinePortInfo("sqlserver", areaCookie.lnecd, areaCookie.port1, areaCookie.port2);
};

/*
function GetShipLineInfo2(dbsource) {
//新增航线-从orcl获取新航线信息
jQuery.ajax({
type: "POST",
url: "YardAnalyse_ashx/YardAnalyse_Get_ShipLineName.ashx",
data: { "dbName": "oracle" },
success: function (output) {
if (output != "<option> </option>") {
$("#dialogShip_lnecd").empty().append(output);
}
}
});
}*/

function SetShipLineInfo() {
    jQuery.ajax({
        type: "POST",
        datatype: "JSON",
        url: "YardAnalyse_ashx/YardAnalyse_Get_ShipLineInfo.ashx",
        data: null,
        success: function (output) {
            if (output != "") {
                var html = "";
                for (var i = 0; i < output.length; i++) {
                    html += "<span style='background-color: " + output[i]["COLOR"] + ";'>" + output[i]["LNECD"] + "</span>";
                }
                $(".ship-info").empty().append(html);
            }
        }
    });
}

function SetShipLineHTML(shipline) {
    var html = "";
    for (var i = 0; i < shipline.length; i++) {
        html += "<span style='background-color: " + shipline[i]["COLOR"] + ";'>" + shipline[i]["LNECD"] + "</span>";
    }
    $(".ship-info").empty().append(html);
}


function GetAreaTable(area) {
    jQuery.ajax({
        type: "POST",
        datatype: "JSON",
        url: "YardAnalyse_ashx/YardAnalyse_Get_RainbowPict.ashx",
        data: { "area": area, "bay": null },
        success: function (output) {
            if (output != "") {
                SetAreaTableHTML(output);
            }
        }
    });
}

function SetAreaTableHTML(table) {
    //设置箱区HTML代码
    var thead = "<table class='area' id='{0}'><thead class='area-head'><tr><th colspan='33'>{0}</th></tr></thead>";
    var tfoot = "<tfoot class='area-foot'><tr><th>65</th><th>63</th><th>61</th><th>59</th><th>57</th><th>55</th><th>53</th><th>51</th><th>49</th><th>47</th><th>45</th><th>43</th><th>41</th><th>39</th><th>37</th><th>35</th><th>33</th><th>31</th><th>29</th><th>27</th><th>25</th><th>23</th><th>21</th><th>19</th><th>17</th><th>15</th><th>13</th><th>11</th><th>09</th><th>07</th><th>05</th><th>03</th><th>01</th></tr></tfoot></table>";
    thead = String.format(thead, table[0]["YRP_AREA"]);

    var tbody = "<tr>";
    var tdHTML = "<td style='background-color:{0};width:{11}px;' area='{1}' bay='{2}' lnecd='{3}' size='{4}' height='{5}' port1='{6}' port2='{7}' colspan='{8}' squeue='{10}'>{9}</td>";
    for (var i = 0; i < table.length; i++) {
        var content = table[i]["YRP_LDUNLDPORT1"] + "<br/>" + table[i]["YRP_LDUNLDPORT2"] + "<br/>" + table[i]["YRP_CNTR_HEIGHT"] + "<br/>" + table[i]["YRP_CNTR_SIZE"];
        if (table[i]["YRP_CNTR_SIZE"] == "40" || table[i]["YRP_CNTR_SIZE"] == "45") {
            tbody += String.format(tdHTML, table[i]["YSC_COLOR"], table[i]["YRP_AREA"], table[i]["YRP_BAY"], table[i]["YRP_LNECD"], table[i]["YRP_CNTR_SIZE"], table[i]["YRP_CNTR_HEIGHT"], table[i]["YRP_LDUNLDPORT1"], table[i]["YRP_LDUNLDPORT2"], 2, content, table[i]["YRP_SQUEUE"], 66);
        }
        else {
            tbody += String.format(tdHTML, table[i]["YSC_COLOR"], table[i]["YRP_AREA"], table[i]["YRP_BAY"], table[i]["YRP_LNECD"], table[i]["YRP_CNTR_SIZE"], table[i]["YRP_CNTR_HEIGHT"], table[i]["YRP_LDUNLDPORT1"], table[i]["YRP_LDUNLDPORT2"], 1, content, table[i]["YRP_SQUEUE"], 33);
        }
    }
    tbody += "</tr>";
    var tableHTML = thead + tbody + tfoot;
    $("#div-" + table[0]["YRP_AREA"]).empty().append(tableHTML);
}

function UpdateRainbowData($this) {
    //更新数据
    Set_RainbowPict_Ashx(areaCookie.area, areaCookie.squeue, $this.find("#dialogArea_lnecd").val(), $this.find("#dialogArea_port1").val(), $this.find("#dialogArea_port2").val(),
    $this.find("#dialogArea_height").val(), $this.find("#dialogArea_size").val());
    //    var area = areaCookie.area;
    //    var bay = "";
    //    var squeue = areaCookie.squeue;
    //    var lnecd = $this.find("#dialogArea_lnecd").val();
    //    var ldunldport1 = $this.find("#dialogArea_port1").val();
    //    var ldunldport2 = $this.find("#dialogArea_port2").val();
    //    var height = $this.find("#dialogArea_height").val();
    //    var size = $this.find("#dialogArea_size").val();
}

function Set_RainbowPict_Ashx(area, squeue, lnecd, ldunldport1, ldunldport2, height, size) {
    jQuery.ajax({
        type: "POST",
        url: "YardAnalyse_ashx/YardAnalyse_Insert_RainbowPict.ashx",
        data: { "area": area, "bay": '', "squeue": squeue, "lnecd": lnecd,
            "ldunldport1": ldunldport1, "ldunldport2": ldunldport2,
            "height": height, "size": size
        },
        success: function (rownum) {
            if (rownum != 0) {
                GetAreaTable(area);
                $("#areamessage").show().hide(2000);

            }
        }
    });
}

function InitialBay($this) {
    //清空贝位信息
    Set_RainbowPict_Ashx(areaCookie.area, areaCookie.squeue, '', '', '', '', '');

    $this.find("#dialogArea_lnecd").val("");
    $this.find("#dialogArea_port1").val("");
    $this.find("#dialogArea_port2").val("");
    $this.find("#dialogArea_height").val("");
    $this.find("#dialogArea_size").val("");
}


function InitialArea(area) {
    //清空箱区信息
    for (var j = 1; j <= 65; j += 2) {
        var area = area;
        var bay = j;
        var squeue = j;
        var lnecd = '';
        var ldunldport1 = '';
        var ldunldport2 = '';
        var height = '';
        var size = '';
        jQuery.ajax({
            type: "POST",
            url: "YardAnalyse_ashx/YardAnalyse_Insert_RainbowPict.ashx",
            data: { "area": area, "bay": bay, "squeue": squeue, "lnecd": lnecd,
                "ldunldport1": ldunldport1, "ldunldport2": ldunldport2,
                "height": height, "size": size
            },
            success: function (rownum) {
                if (rownum != 0) {
                    GetAreaTable(area);
                }
            }
        });
    }
}


//string format 函数
String.format = function () {
    if (arguments.length == 0) {
        return null;
    }
    var str = arguments[0];
    for (var i = 1; i < arguments.length; i++) {

        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
}