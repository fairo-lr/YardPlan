// JScript 文件
/*Tab分页的函数*/
function showTab(tabHeadId,tabContentId,tabDiv,tabsHead) 
{ 
    //tab层 
    var tabDiv = document.getElementById(tabDiv); 
    //将tab层中所有的内容层设为不可见 
    
    //遍历tab层下的所有子节点 
    var taContents = tabDiv.childNodes; 
    for(i=0; i<taContents.length; i++) 
    { 
        //将所有内容层都设为不可见 

        if(taContents[i].id!=null && taContents[i].id != tabsHead) 
        { 
            taContents[i].style.display = 'none'; 

        } 
    } 

    //将要显示的层设为可见 
    document.getElementById(tabContentId).style.display = 'block'; 

    //遍历tab头中所有的超链接 
    var tabHeads = document.getElementById(tabsHead).getElementsByTagName('a'); 
    for(i=0; i<tabHeads.length; i++) 
    { 
        //将超链接的样式设为未选的tab头样式 
        tabHeads[i].className='tabs'; 
    } 
    //将当前超链接的样式设为已选tab头样式 
    document.getElementById(tabHeadId).className='curtab'; 
    document.getElementById(tabHeadId).blur(); 
} 
