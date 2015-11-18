// JScript 文件

/*模拟ToolTip的鼠标离开时间*/
function mouseout()
{
	$('tooltipDiv').style.visibility="hidden";
}

//2012-04-06 Colin 鼠标进入后显示公式
function formulaMouseOver(str)
{
	$('tooltipDiv').innerHTML = str;
	$('tooltipDiv').style.left = event.clientX;
    $('tooltipDiv').style.top = event.clientY+document.documentElement.scrollTop;
    $('tooltipDiv').style.visibility="visible";
}
