$('.b-b-b').on('tap', function () {
    $('.more-pop').toggleClass('active')
})

$('.more-pop .closemode').on('tap', function () {
    
    $('.more-pop').toggleClass('active');
})

$('.more-pop .self-mode').on('tap', function () {
    $('.self-set-pop').toggleClass('active');
})

$('.self-set-pop .closemode').on('tap', function () {
    $('.self-set-pop').toggleClass('active');
})
$('.self-set-pop .self-modez').on('tap', function () {
    $('.self-set-pop').toggleClass('active');
})



var isnot = true;
var nub = new Array(1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 63, 69, 73, 75, 75, 73, 69, 63, 55, 45, 36, 28, 21, 15, 10, 6, 3, 1);
var nub1 = new Array(1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 63, 69, 73, 75, 1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 63, 69, 73, 75);
var mode = new Array([0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27],//全包
    [1, 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27],//单
    [0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26],//双
    [14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27],//大
    [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13],//小
    [10, 11, 12, 13, 14, 15, 16, 17],//中
    [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27],//边
    [15, 17, 19, 21, 23, 25, 27],//大单
    [1, 3, 5, 7, 9, 11, 13],//小单
    [14, 16, 18, 20, 22, 24, 26],//大双
    [0, 2, 4, 6, 8, 10, 12],//小双
    [18, 19, 20, 21, 22, 23, 24, 25, 26, 27],//大边
    [0, 1, 2, 3, 4, 5, 6, 7, 8, 9],//小边
    [1, 3, 5, 7, 9, 19, 21, 23, 25, 27],//单边
    [0, 2, 4, 6, 8, 18, 20, 22, 24, 26],//双边
    [0, 10, 20],//0尾
    [1, 11, 21],//1尾
    [2, 12, 22],//2尾
    [3, 13, 23],//3尾
    [4, 14, 24],//4尾
    [0, 1, 2, 3, 4, 10, 11, 12, 13, 14, 20, 21, 22, 23, 24],//小尾
    [5, 15, 25],//5尾
    [6, 16, 26],//6尾
    [7, 17, 27],//7尾
    [8, 18],//8尾
    [9, 19],//9尾
    [5, 6, 7, 8, 9, 15, 16, 17, 18, 19, 25, 26, 27],//大尾
    [0, 3, 6, 9, 12, 15, 18, 21, 24, 27],//3余0
    [1, 4, 7, 10, 13, 16, 19, 22, 25],//3余1
    [2, 5, 8, 11, 14, 17, 20, 23, 26],//3余2
    [0, 4, 8, 12, 16, 20, 24],//4余0
    [1, 5, 9, 13, 17, 21, 25],//4余1
    [2, 6, 10, 14, 18, 22, 26],//4余2
    [3, 7, 11, 15, 19, 23, 27],//4余3
    [0, 5, 10, 15, 20, 25],//5余0
    [1, 6, 11, 16, 21, 26],//5余1
    [2, 7, 12, 17, 22, 27],//5余2
    [3, 8, 13, 18, 23],//5余3
    [4, 9, 14, 19, 24]//5余4
);


var maxnum = parseInt(70000000); // 最大投注额
var limit_msg = "投注上限" + maxnum + "金币，请重新投注！！";
$(document).ready(function () {
    var Seconds = $("#Seconds").val()
    get_count_down(Seconds);
    //点击投注模式
    $(".mode_lottery").tap(function () {
        var i = $(this).attr("attr");
        clear();
        if (i) {
            setValue(i)
        }
        recount_coins();
    });



    //倒计时
    var stop_auto_reload;
    var stop_auto_reload_up; //截

    //开奖--------------------------------
    function get_count_down(lottery_start_time) {
        var lottery_start_issue = $("#Issue").val();
        if (lottery_start_issue != 0) {
            if (lottery_start_time > 0) {
                lottery_start_time = lottery_start_time - 1;  //
                var help_note = "";
                help_note = "第<em>" + lottery_start_issue + "</em>期，离开奖时间还有<em>" + lottery_start_time + "</em>秒<br>";
                $("#left_title").html(help_note);
                stop_auto_reload = setTimeout(function () {
                    get_count_down(lottery_start_time);
                }, 1000);
                clearTimeout(stop_auto_reload_up);
            } else {
                $("#left_title").html("第<em>" + lottery_start_issue + "</em>期正在开奖中！");
                clearTimeout(stop_auto_reload);
                stop_auto_reload_up = setTimeout(function () {
                    showMsg('1', "投注已经到期！");
                    window.location.href = "/";
                    get_count_down(lottery_start_time);
                }, 1000);
            }
        }
    }




    //点击简易投注模式
    /*  $(".mode_lottery1").click(function () {
          var i = $(this).attr("attr");
          clear();
          if (i) setValue(i);
          recount_coins();
      })
  */
    //点击号码
    $('table').on('tap', '.num', function () {
        var This = $(this);
        var input = This.parents('tr').find('input');
        if (!input.attr("readonly")) {
            var index = parseInt(This.html());
            input.val(nub[index]);
            setSelectCss(input);
            recount_coins();
        }
    })

    //点击反选按钮
    $(".fanxian").tap(function () {
        un_select();
    })
    //点击清除按钮
    $(".qingchu").tap(function () {
        clear();
    })

    $('.pei').on('tap', 'span', function () {
        var This = $(this);
        var peilv = This.attr('val');
        var input = This.parents('tr').find('input');
        if (!input.attr("readonly")) {
            var txt_value = $.trim(input.val()).replace(/,/gi, "");
            if (txt_value && !isNaN(txt_value)) {
                var new_value = Math.floor(txt_value * peilv);
                if (new_value > maxnum) {
                    showmsg("1", limit_msg);
                    return false;
                }
                input.val(number_format(new_value + ""));
            }
        }
        recount_coins();
    });

    /*----------------------------------------------------------------------不知道作用*/
    $(".touzhuclear").click(function () {
        clear();
    })
    //刷新赔率
    $(".refreshpeiv").click(function () {
        refresh_odds(lottery_id);
    });
    //上期投注-----------------------没用到--------------------------no
    $(".touzhu1").eq(1).click(function () {
        last_lottery();
    })
    //点击整体的倍数
    $(".double_insert").tap(function () {
        var This = $(this);
        var peilv = parseFloat(This.attr('val'));
        if (peilv > 0) {
            setAllvalue(peilv);
            recount_coins();
        } else {//梭哈
            setSuoha();
        }

    });


    //输入投注数据
    $('.table-select').find('input[type="text"]').keyup(function () {
        var regex = /^[1-9]\d{0,}$/;
        var val = $(this).val();
        if (!regex.test(val)) {
            val = val.replace(/\D/g, '');
            $(this).val(val);
        }
        if (!regex.test(val)) {
            $(this).val(val.substring(1));
            recount_coins();
        } else {
            // $('#checkboxd' + val).attr("checked", true);
            $(this).parents('tr').find('input').addClass('selected');
            recount_coins();
        }

    }).blur(function () { //移开
        if ($(this).val() > maxnum) {
            showmsg("1", limit_msg);
            return false;
        }
        $(this).val(number_format($(this).val()));
        if ($(this).val()) {
            setSelectCss(this);
        } else {
            unsetSelectCss(this);
        }

    }).focus(function () {//选中
        if ($(this).val().indexOf(",") > -1) {
            domvalue = $(this).val().replace(/,/gi, "");
            $(this).val(domvalue);
        } else {
        }

    });


});

function setSelectCss(i) {
    $(i).parents('tr').find('input').addClass('selected');
}

function unsetSelectCss(i) {
    $(i).parents('tr').find('input').removeClass('selected');
}

//标准投注模式设定方法---------------------------------------------no
function setValue(num) {
    for (var i = 0; i < mode[num].length; i++) {
        var id_num = mode[num][i];
        var tr = $('.table-select').find('tr').eq(mode[num][i]);
        var input = tr.find('input');
        if (!input.attr("readonly")) {
            input.val(nub[id_num]);
            setSelectCss(input);
            if (!input.val()) {
                unsetSelectCss(input);
            }
        } else {
            if (!input.val()) {
                unsetSelectCss(id_num);
            }
        }
    }
}

//清除方法
function clear() {
    $("input[type='text']").each(function () {
        if (!$(this).attr("readonly")) {
            $(this).removeClass('selected');
            $(this).val('');
        }
    });

    $("#totalvalue").val("0");
    $("#totalvalue2").val("0");
    $("#total_md_lottery").html("0");
    $("#total_md_lottery2").html("0");
    $("#now_total").val("0");
}

//数字加千分符号
function number_format(n) {
    return n;
    //re = /(\d{1,3})(?=(\d{3})+(?:$|\.))/g
    //return n.replace(re, "$1,");
}

//设置所有投注
function setAllvalue(peilv) {
    $("input[type='text']").each(function () {
        if ($(this).hasClass('selected') && !$(this).attr("readonly")) {
            var txt_value = $.trim($(this).val()).replace(/,/gi, "");
            if (txt_value && !isNaN(txt_value)) {
                var new_value = Math.floor(txt_value * peilv);
                $(this).val(number_format(new_value + ""));
            }
        }
    });
}

//反选
function un_select() {
    $("input[type='text']").each(function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
            $(this).val('');
        } else {
            $(this).parents('tr').find('.num').trigger('tap');
        }

    });
    recount_coins();
}
// 上期投注
function last_lottery() {
    $.ajax({
        type: "get",
        url: "/Luck28Mobile/ajaxBuy?do=lastLottery&g28=" + g28,
        error: function () {
        },
        success: function (data, textStatus) {
            data && lottery_mode(data.split(","));
        }
    });
}
// 自定义模式
function personmode(id) {
    $.ajax({
        type: "get",
        url: "/Luck28Mobile/ajaxModel?do=detail&g28=" + g28 + "&model_id=" + id,
        error: function () {
        },
        success: function (data, textStatus) {
            //  set_lottery_val(data.split(","));
            lottery_mode(data.split(","));
        }
    });
}

// 刷新赔率
function refresh_odds(id) {
    $.ajax({
        type: "get",
        url: "/Luck28Mobile/ajaxBuy?do=odds&g28=" + g28 + "&Luck28Id=" + id,
        error: function () {
        },
        success: function (data, textStatus) {
            set_odds("", data.split(",")); //当前赔率
        }
    });
}

//页面载入时执行
function set_lottery_val(arr, flag) {
    if (left_time_out == "-1") {
        showmsg("1", "该期已经截止投注！", last_issue_id);
        return false;
    }
    else if (is_clear_mdp == "1") {

    }
    // clear();
    $.each(arr,
        function (i) {
            if (this != "" && this != 0) {
                if ($("#txt" + i).attr("readonly")) {
                    unsetSelectCss(i);
                    return;
                }
                if (flag) {
                    unsetSelectCss(i);
                    $("#checkboxd" + i).attr("disabled", true);
                    $("#txt" + i).attr("readonly", true).attr("disabled", true);
                    $('#click_number' + i).removeClass('no_left_bg');
                    $('#click_number' + i).addClass('no_left_bg_02');
                } else {
                    setSelectCss(i);
                    $("#checkboxd" + i).attr("checked", true);
                }
                $("#txt" + i).val(number_format(this));
            } else {
                unsetSelectCss(i);
            }
        });
    recount_coins();
}

//自定义投注模式
function lottery_mode(arr, flag) {
    if (left_time_out == "-1") {
        showmsg("1", "该期已经截止投注！", last_issue_id);
        return false;
    }
    clear();
    $.each(arr,
        function (i) {
            if (this != "" && this != 0) {
                if ($("#txt" + i).attr("readonly")) {
                    unsetSelectCss(i);
                    return;
                }
                if (flag) {
                    unsetSelectCss(i);
                    $("#checkboxd" + i).attr("disabled", true);
                    $("#txt" + i).attr("readonly", true).attr("disabled", true);
                    $('#click_number' + i).removeClass('no_left_bg');
                    $('#click_number' + i).addClass('no_left_bg_02');
                } else {
                    setSelectCss(i);
                    $("#checkboxd" + i).attr("checked", true);
                }
                $("#txt" + i).val(number_format(this));
            }
        });
    recount_coins();
}

var first = 0;
//取总的投注比特币
function recount_coins() {
    var total = 0;
    var now_total = 0;
    $('.selected').each(function () {
        var txt_value = $.trim($(this).val()).replace(/,/gi, "");
        if (txt_value && !isNaN(txt_value)) {
            total += parseInt(txt_value);
            if (!$(this).attr("readonly")) {
                now_total += parseInt(txt_value);
            }
        }
    });
    $("#totalvalue").val(number_format(total + ""));
    $("#totalvalue2").val(number_format(total + ""));
    $(".orange").val(number_format(total + ""));
    $("#total_md_lottery").html(number_format(total + ""));
    $("#total_md_lottery2").html(number_format(total + ""));
    $("#now_total").val(number_format(now_total + "")); //now throw
    if (now_total > maxnum) {
        //showmsg("1", limit_msg);
        //return false;
    }
}

function set_odds(last_odds_ary, this_odds_ary) {
    if (last_odds_ary != "") {
        $.each(last_odds_ary, function (i) {
            var v = this + "";
            $('.peirrv').eq(i).html(v);
            // $("#last_lottery_odds" + i).text(v); //上期赔率
        })
    }
    if (this_odds_ary != "") {
        $.each(this_odds_ary, function (i) {
            var v = this + "";
            $('.peirrv').eq(i).html(v);
            // $("#this_lottery_odds" + i).text(v); //当前赔率
        })

    }
}

// TODO 判断是否当前正在投注的期数
function judge_issue(issue) {
    var now_issue;
    $.ajax({
        type: "get",
        async: false, //同步
        url: "/Luck28Mobile/judge?g28=" + g28 + "&Luck28Id=" + issue,
        error: function () {
        },
        success: function (data) {
            now_issue = parseInt(data);
        }
    });
    return now_issue;
}

// TODO 实时获取现有的比特币-----------------------------------------------------开的时候打开
function check_md() {
    var flag = $("#leave_modou_num").html().replace(/,/gi, "");;
    return flag;
}
function check_auto() {
    var flag;
    $.ajax({
        type: "get",
        async: false, //同步
        url: "/Luck28Mobile/ajaxAutoPlay",
        data: "do=isauto&g28=" + g28,
        //error: function() {},
        success: function (data) {
            flag = parseInt(data);
        }
    });
    return flag;
}
//确认投注
function comform() {

    var tzjb = "";
    var tznb = "";
    var countjb = 0;
    for (var i = 0; i <= 27; i++) {
        var jb = $("#" + i + "").val();
        if (jb.length != 0) {
            tzjb += jb + ",";
            tznb += i + ",";
            countjb += parseInt(jb);
        }
    }
    var str = [];
    if (tzjb == "") {
        showMsg('1', "对不起，请先投注！");
        return false;
    } else if (countjb > maxnum) {//limit
        //showmsg("1", limit_msg);
        showMsg('1', limit_msg);
        return false;
    }
    else if (countjb < 10)
    {
        showMsg('1', "最小投注金额10金币！");
        return false;
    }
    else if (countjb > check_md()) {
        showMsg('1', "您的金币余额不足！");
        return false;
    } else {
        var Issue = $("#Issue").val();

        $.ajax({
            type: "POST",

            url: "/Tz/SetBetting",
            data: { "tzjb": tzjb, "tznb": tznb, "Issue": Issue, "countjb": countjb },
            dataType: 'json',
            success: function (data) {
                try {
                    if (data.msg == "1") {
                        showMsg('3', "投注成功！");
                    }
                    else if (data.msg == "2") {
                        showMsg('3', "本期已经结束！");
                    }
                    else if (data.msg == "3") {
                        showMsg('1', "投注金额过大或者过小！");
                    }
                    else {
                        showMsg('1', "投注失败！");
                    }
                } catch (e) {
                    location.href = "/admin/error";
                }
            }
        });
    }
}

function SaveAutomatic() {
    var obj = document.getElementById("Open") //定位id
    var value = obj.options[obj.selectedIndex].value; // 选中值

    var TempWin = document.getElementById("TempWin" + value + "");
    var Winvalue = TempWin.options[TempWin.selectedIndex].value;

    var TempLose = document.getElementById("TempLose" + value + "");
    var Losevalue = TempLose.options[TempLose.selectedIndex].value;

    var start = $("#start").val();
    var end = $("#end").val();
    var MinJb = $("#MinJb").val();
    var MaxJb = $("#MaxJb").val();
    if (value == "") {
        showMsg('1', "请选择投注模式！");
    } else if (parseInt(MinJb) >= parseInt(MaxJb)) {
        showMsg('1', "最大金币应该大于最小金币！");
        return false;
    }
    else if (parseInt(end) < parseInt(start)) {
        showMsg('1', "结束期必须大于开始期！");
    }
    else {
        $.ajax({
            type: "POST",
            url: "/Tz/Set_Automatic",
            data: { "ATempId": value, "start": start, "end": end, "MinJb": MinJb, "MaxJb": MaxJb, "Winvalue": Winvalue, "Losevalue": Losevalue},
            dataType: 'json',
            success: function (data) {
                try {
                    if (data.msg == 1) {
                        showMsg('4', "自动投注成功！");
                    }
                    else if (data.msg == -1) {
                        showMsg('1', "开始期必须大于当前期！");
                    }
                    else if (data.msg == -2) {
                        showMsg('1', "结束期必须大于开始期！");
                    }
                    else if (data.msg == -3)
                    {
                        showMsg('1', "最大金币应大于最小金币！");
                    }
                    else if (data.msg == -4) {
                        showMsg('用户状态错误,请重新登入后再操作！');
                    }
                    else if (data.msg == -5) {
                        showMsg('自动投注已开启,请勿重新开启！');
                    }
                    else {
                        showMsg('1', "投注失败！");
                    }
                } catch (e) {
                    location.href = "/admin/error";
                }
            }
        });
    }
}

//保存模式

$(function () {
    $('.tzbtn').tap(function () {
        if (isnot) {
            comform();
            isnot = false;
        }
    });
});

function Stop_Auto(AId)
{
    $.ajax({
        type: "POST",
        url: "/Tz/Stop_Automatic",
        data: { "AId": AId },
        dataType: 'json',
        success: function (data) {
            try {
                if (data.msg == 1) {
                    showMsg('4', "停止投注成功！");
                }
                else {
                    showMsg('1', "停止失败！");
                }
            } catch (e) {
                location.href = "/admin/error";
            }
        }
    });
}

$(function () {
    $('.sure-dh').tap(function () {
        var AId = $("#AId").val();
        if (AId == "") {
            SaveAutomatic();
        } else {
            Stop_Auto(AId);
        }
    });
});

//取消投注
function cancel() {
    $('#depend_parent').val('0');
    $('#shade_div_id').hide();
    $('#message_box').hide();
    $('#message_casesave').hide();
    $('#message_modesh').hide();
    $('#used_leave_modou_num').val('');
}

//确认投注
function save_data() {
    $('#save_28data').removeAttr('onclick');
    $('#save_28data').unbind('click');
    $("input[name='mdp_coin']").attr("disabled", false);
    var form1 = document.getElementById("form1"); //父层
    form1.submit();
}

//滚动
function set_boxsize(id) {
    document.getElementById(id).style.top = (document.documentElement.scrollTop + document.body.scrollTop + (document.documentElement.clientHeight - document.getElementById(id).offsetHeight) / 2) + "px";
    document.getElementById(id).style.left = (document.documentElement.scrollLeft + (document.documentElement.clientWidth - document.getElementById(id).offsetWidth) / 2) + "px";
}

//window.onscroll = set_boxsize;

function showshade() {
    //弹出笼罩层
    var bodyheight = 1002;
    var shade_div_id = document.getElementById("shade_div_id");
    shade_div_id.style.display = 'block';
    shade_div_id.style.height = parseInt(bodyheight) + 'px';
}
//投注后信息返回
function showmsg(flag, msg, issue_id) {
    $("#message_box").css("display", "");
    set_boxsize("message_box");
    switch (flag) {
        case "0":
            if (readcookie("handflag") == "1") {
                setcookie("handflag", "2");
            }
            window.location.href = "/Luck28Mobile?g28=" + g28;
            break;
        case "1":
            //投注失败
            $("#box_title").html('提示信息');
            $("#box_contents").html(msg);
            $("#box_confirm").html($.trim('<a href="javascript:;" onClick="cancel();"  class="a1">确定</a>'));
            //$("#submit_ok").addClass('mode_box_btn');
            break;
        case "4":
            $("#box_title").html('提示信息');
            $("#box_contents").html(msg);
            $("#box_confirm").html($.trim('<a href="javascript:;" onClick="cancel();document.location.reload();"  class="a1">确定</a>'));
            //$("#submit_ok").addClass('mode_box_btn');
            break;
        case "2":
            //确认投注
            $("#box_contents").html(msg);
            $("#box_confirm").html($.trim('<a href="javascript:;" id="save_28data" onClick="point_touch_submit(this.form1, save_data);"  class="a1">确定</a><a href="javascript:;" onclick="cancel();" class="a2">取消</a>'));
            //$("#submit_ok").addClass('mode_box_btn');
            //$("#submit_no").addClass('mode_box_btn');
            break;
        case "3":
            //提示过期
            $("#box_contents").html(msg);
            $("#box_confirm").html($.trim('<a href="javascript:;" onclick="go_url(' + issue_id + ');"  class="a1">确定</a><a href="javascript:;" onclick="cancel();" class="a2">取消</a>'));
            //$("#submit_ok").addClass('mode_box_btn');
            //$("#submit_no").addClass('mode_box_btn');
            break;
    }
    showshade();
}


function go_url(id) {
    document.location.href = '/Luck28Mobile/buy?g28=' + g28 + '&Luck28Id=' + id;
}

function setSuoha() {
    var lotteryVal = parseInt($("#totalvalue").val());
    if (lotteryVal) {
        // $("#message_modesh").show();
        var used_md = parseInt($('#used_leave_modou_num').val());
        var md = check_md();
        $('#old_leave_modou_num').val(md);
        $('#leave_modou_num').html(number_format(md + ""));
        md = used_md ? used_md : md;
        md = md >= maxnum ? maxnum : md;
        $('#use_leave_modou_num').val(number_format(md + ""));
        $('#used_leave_modou_num').val(md);
        //set_boxsize('message_modesh');
        //showshade();
        setSmallSuoha();
        checkSuoha();
    }
}
//设置梭哈值
function setSmallSuoha() {
    $('#old_leave_modou_num').val('');
    var old_md = parseInt($('#old_leave_modou_num').val());
    var md = old_md ? old_md : check_md();
    md = md >= maxnum ? maxnum : md;
    $('#use_leave_modou_num').val(md);
    $('#used_leave_modou_num').val(md);
}


function checkSuoha() {
    var used_md = parseInt($('#used_leave_modou_num').val());
    var sel_md = parseInt($("#totalvalue").val().replace(/,/gi, ""));
    var total = compare_md = 0;
    $(".table-select").find(".selected").each(function () {
        if (!$(this).attr("readonly")) {
            var txt_value = $.trim($(this).val()).replace(/,/gi, "");
            if (txt_value && !isNaN(txt_value)) {
                var new_value = Math.floor((txt_value / sel_md) * used_md);
                if (new_value > maxnum) {
                    // showmsg("1", limit_msg);
                    cancel();
                    return false;
                }
                total += new_value;
                $(this).val(number_format(new_value + ""));
            }
        }
    });

    recount_coins();
    cancel();
}

function GetProbability() {
    var Issue = $("#Issue").val()
    var flag;
    $.ajax({
        type: "POST",
        url: "/Tz/Probability/",
        data: { "id": Issue},
        dataType: 'json',
        success: function (data) {
            try {
                var datas = data.msg;
                flag = datas;
                $('.table-select').html(flag)
            } catch (e) {
                location.href = "/admin/error";
            }
        }
    });
}

GetProbability();


