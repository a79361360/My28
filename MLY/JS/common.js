$('.arrow').on('tap', function () {
    window.history.back();
})


$('body').on('tap', '.close,.showmsg-sure',function () {
    $('.showmsg').css('display', 'none')
});

function showMsg(flag, content, issue_id) {
    var title = title || "";
    var content = content || "";
    $('.showmsg').css('display','block');

    $('.showmsg-title').html("提示");
    $('.showmsg-content').html(content);
	switch (flag) {
		case "3":
		    document.getElementById("box").innerHTML = "<a class=\"sure showmsg-sure\" onclick=\"javascript:window.location.href='/';\">确定</a>";
		    break;
	    case "4":
	        document.getElementById("box").innerHTML = "<a class=\"sure showmsg-sure\" onclick=\"javascript:document.location.reload();\">确定</a>";
	        break;
	}
	
}
function TrendTop()
{
    if ($('#limit').val() > 0)
    {
        var count = $('#limit').val();
        window.location.href = "/Tz/Trend/" + count + "";
    }
}





