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
    [5, 15, 25],//5尾
    [6, 16, 26],//6尾
    [7, 17, 27],//7尾
    [8, 18],//8尾
    [9, 19],//9尾
	[0, 1, 2, 3, 4, 10, 11, 12, 13, 14, 20, 21, 22, 23, 24],//小尾
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

var maxnum = parseInt(50000000); // 最大投注额
var limit_msg = "投注上限" + maxnum + "金币，请重新投注！！";
$(document).ready(function () {
    //点击投注模式
    $(".mode_lottery").on('touchend',function (e) {
        e.preventDefault();
        var i = $(this).attr("attr");
        clear();
        if (i) {
            setValue(i)
        }
        recount_coins();
    });

    //点击简易投注模式
  /*  $(".mode_lottery1").click(function () {
        var i = $(this).attr("attr");
        clear();
        if (i) setValue(i);
        recount_coins();
    })
*/
    //点击号码
    $('table').on('tap', '.label', function (e) {
        var This = $(this);
        var input = This.parents('tr').find('input');
        if(This.hasClass('active')){
            unsetSelectCss(input);
            recount_coins();
            input.val('');
        }else{
            if (!input.attr("readonly")) {
                var index = parseInt(This.html());
                input.val(nub[index]);
                setSelectCss(input);
                recount_coins();
            }
        }

    });

    //点击反选按钮
    $(".fanxuan").on('touchend',function (e) {
        un_select();
    })
    //点击清除按钮
    $(".qingchu").on('touchend',function (e) {
        clear();
    })

    $('.pei').on('click', 'span', function () {
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
    $(".touzhuclear").on('touchend',function (e) {
        e.preventDefault();
        clear();
    })
    //刷新赔率
    $(".refreshpeiv").on('touchend',function (e) {
        e.preventDefault();
        refresh_odds(lottery_id);
    });
    //上期投注-----------------------没用到--------------------------no
    $(".touzhu1").eq(1).on('touchend',function (e) {
        last_lottery();
    })
    //点击整体的倍数
    $(".double_insert").on('touchend', function (e) {
        e.preventDefault();
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
    $('.table1').find('input[type="text"]').keyup(function () {
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
            $(this).parents('tr').find('.label').addClass('active');
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
    if(typeof i=="number"){
        $('.table1').find('tr').eq(i).find('.label').addClass('active');
       // $(i).parents('tr').find('.label').addClass('active');
    }else{
        $(i).parents('tr').find('.label').addClass('active');
    }

}

function unsetSelectCss(i) {
    $(i).parents('tr').find('input').removeClass('selected');
    $(i).parents('tr').find('.label').removeClass('active');
}

//标准投注模式设定方法---------------------------------------------no
function setValue(num) {
    for (var i = 0; i < mode[num].length; i++) {
        var id_num = mode[num][i];
        var tr = $('.table1').find('tr').eq(mode[num][i]);
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
    $("input[type='text']").not('#model_name').each(function () {
        if (!$(this).attr("readonly")) {
            $(this).removeClass('selected');
            $(this).parents('tr').find('.label').removeClass('active');
            $(this).val('');
        }
    });

    $('#total_sum').val(0);

    $("#totalvalue").val("0");
    $("#totalvalue2").val("0");
    $("#total_md_lottery").html("0");
    $("#total_md_lottery2").html("0");
    $("#now_total").val("0");
}

//数字加千分符号
function number_format(n) {
    re = /(\d{1,3})(?=(\d{3})+(?:$|\.))/g
    return n.replace(re, "$1,");
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
            $(this).parents('tr').find('.label').removeClass('active');
            $(this).val('');
        } else {
           // console.log( $(this).parents('tr').find('.label'))
            $(this).parents('tr').find('.label').trigger('tap');
            $(this).parents('tr').find('.label').addClass('active');
           //window.setTimeout(function(){

          // },300)

        }

    });
    recount_coins();
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
    //if (left_time_out == "-1") {
    //    showmsg("1", "该期已经截止投注！", last_issue_id);
    //    return false;
    //}
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
                $("#txt" + i).addClass("selected").val(number_format(this));
            }
        });
    recount_coins();
}



function check_md() {

    $.ajax({
        type: "get",
        async: false, //同步
        url: "/Shared/Get_UserJbMoney",
        error: function () {
        },
        success: function (data) {
            flag = data.jb;
            console.log(data.jb)
            console.log(flag)
        }
    });
    return flag;
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

//取消投注
function cancel() {
    $('#depend_parent').val('0');
    $('#shade_div_id').hide();
    $('#message_box').hide();
    $('#message_casesave').hide();
    $('#message_modesh').hide();
    $('#used_leave_modou_num').val('');
}

function setSuoha() {
    recount_coins();
    var lotteryVal = parseInt($("#totalvalue").val());
    if (lotteryVal) {
       // $("#message_modesh").show();
        var used_md = parseInt($('#used_leave_modou_num').val());
        var md = check_md();
        console.log(md)
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
    $(".table1").find(".selected").each(function () {
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

// 改变号码压注状态
function number_bg(n, i) {
	//var str = $("#mumber_div"+i).attr("class");
	//if(str != "across_par1_bg1"){
		if(n > 0){
			$("#mumber_pa1_"+i).attr("class","no_left_bg_02");
			$("#mumber_div"+i).attr("class","across_par1_bg1");//across_par1_bg1
			$("#luck_img_"+i).show();
		}else{
			$("#mumber_pa1_"+i).attr("class","no_left_bg");
			$("#mumber_div"+i).attr("class","across_par1_no");//across_par1_no
			$("#luck_img_"+i).hide();
			$("#luck_img_"+i).hide();
			$("#txt"+i).val('');
		}
	//}
}

// 载入模式
function modeLoad(obj,i) {
    $.ajax({
		type:"POST",
		url: "/WxJs28/TempTz",
		//data: {"gameid":1,"tempid":i},
		data: { gameid: 1, tempid: i },
		success: function (msg) {
		
			if(msg != "0" && msg != "-1") {
				var arr = msg.data.split(',');
				var sum = 0;
				$(".form-control").removeClass('selected');
				$(".labelgrey").removeClass('active');
				for (loop = 0 ; loop < 28 ; loop++) {
			        var n = parseInt(arr[loop]);
			        //投注金额
			        if(n == 0){
			        	$("#txt"+loop).val('');
			        } else {
			            $("#txt" + loop).addClass('selected').val(numSep(n));
			            $("#txt" + loop).parent().prev().children().addClass('active');
			        }
			        //总投注金额
			        sum = sum + n;  
			        //改变选中状态
			        number_bg(n,loop);
			    }
			    sum = numSep(sum);
			    $("#total_md_lottery").html(sum);
				recount_coins();
			} else {
				showMsg('1', "请先登录，再进行操作",1);
			}
		}
	});
	//$("#modes_id").val(i);
	//$("#modes_name").val($(obj).html());
	//$("#modes_name_1").html($(obj).html());
	//if($(obj).html().length > 10) {
	//	$("#modes_name_2").html($(obj).html().substr(0,10)+"...");
	//} else {
	//	$("#modes_name_2").html($(obj).html());
	//}
	//$("#btn_saveas").show();
    //$("#btn_del").show();
    //$('#b_save').removeAttr('onclick');
    //$('#b_save').unbind('click');
    //$('#b_save').bind('click', function(){
    //	modeUp();
    //});
    //改变颜色
    $(obj).siblings().removeClass("a2");
    $(obj).addClass("a2");

}

//------北京28自定义投注列表
function modeLoadbj(obj, i) {
    $.ajax({
        type: "POST",
        url: "/WxJs28/TempTz",
        //data: {"gameid":1,"tempid":i},
        data: { gameid: 2, tempid: i },
        success: function (msg) {

            if (msg != "0" && msg != "-1") {
                var arr = msg.data.split(',');
                var sum = 0;
                $(".form-control").removeClass('selected');
                $(".labelgrey").removeClass('active');
                for (loop = 0 ; loop < 28 ; loop++) {
                    var n = parseInt(arr[loop]);
                    //投注金额
                    if (n == 0) {
                        $("#txt" + loop).val('');
                    } else {
                        $("#txt" + loop).addClass('selected').val(numSep(n));
                        $("#txt" + loop).parent().prev().children().addClass('active');
                    }
                    //总投注金额
                    sum = sum + n;
                    //改变选中状态
                    number_bg(n, loop);
                }
                sum = numSep(sum);
                $("#total_md_lottery").html(sum);
                recount_coins();
            } else {
                showMsg('1', "请先登录，再进行操作", 1);
            }
        }
    });
    $(obj).siblings().removeClass("a2");
    $(obj).addClass("a2");

}

//极速28自定义投注列表
function modeLoad1(i,id) {
    if (i == 0) {
        return;
    } else {
    $.ajax({
        type: "POST",
        url: "/WxJs28/TempTz",
        //data: {"gameid":1,"tempid":i},
        data: { gameid: id, tempid: i },
        success: function (msg) {
            if (msg != "0" && msg != "-1") {
                var arr = msg.data.split(',');
                var sum = 0;
                for (loop = 0 ; loop < 28 ; loop++) {
                    var n = parseInt(arr[loop]);
                    //投注金额
                    if (n == 0) {
                        $("#txt" + loop).val('');
                    } else {
                        $("#txt" + loop).addClass('selected').val(numSep(n));
                        $("#txt" + loop).parent().prev().children().addClass('active');
                    }
                    //总投注金额
                    sum = sum + n;
                    //改变选中状态
                    number_bg(n, loop);
                }
                sum = numSep(sum);
                $("#tzze").val(sum);
                $("#msmc").val(msg.tempname)
            } else {
                showMsg('1', "请先登录，再进行操作", 1);
            }
        }
    });
    }
}

// 上期投注
function last_lottery() {
	$.ajax({
		type: "post",
		url: "/WxJs28/SqTz",
		data: { "gameid": "1" },
		dataType: 'json',
		error: function() {
		},
		success: function (data, textStatus) {
		    var data1 = data.data;
		    console.log(data1);
		    data && lottery_mode(data1.split(","));
		    
		}
	});
}
//上期投注
$(".touzhu1").on('touchend',function(e) {
	last_lottery();
})

var html = "";
for (var i = 0; i <= 27; i++) {
    var m = i;
    if (i < 10) {
        var m = 0 + '' + i;
    }
    html +=
        '<tr>\
        <td><span class="label label-danger labels">' + m + '</span></td>\
    <td><input type="text" class="form-control" id="txt'+i+'"/></td>\
    <td class="peirrv black">1001.15</td>\
    <td class="pei"><span val="0.5">×0.5</span><span val="2">×2</span><span val="10">×10</span></td>\
    </tr>';
}

