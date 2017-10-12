
var max = $("#maxzhi").val();
var myjf = $("#myjf").val();

$(function () {
    max = $("#maxzhi").val();   //最大值

    var regex = /^[1-9]{0,}$/;
    $(".srinput").change(function () {
        var val = $(this).val();
        var chk = $(this).parents(".items").find(".xzinputt");
        //$(this).numeral(false);
        if (!regex.test(val)) {
            val = val.replace(/\D/g, '');
            if (val && !isNaN(val)) {
                if (val > 0) {
                    $(this).val(val);

                    chk.prop("checked", true);
                } else {
                    chk.prop("checked", false);
                }
            } else {
                chk.prop("checked", false);
            }
        }
        if (!regex.test(val)) {
            //$(this).val(val.substring(0, val.length - 1));
        } else {
            $(this).parents(".items").find(".xzinputt").prop("checked", true);
        }
        $(this).parents(".items").find(".xzinputt").trigger('change');
        jszbd();
    });
    $(".xzinputt").click(function () { checkclick(this); });
    //    $(".onceclickbut").click(function () { this.onclick = function () {  return false; } });
})

function stopbuttcftj() {

}
function doinsert(gameid) {
  
    var ztz = 0;
    var dstr = "";
    $(".srinput").each(function (i, e) {
        var tempe = $(e);
        var tempcount = parseInt(tempe.val());
        if (!isNaN(tempcount) && tempcount > 0) {
            ztz += tempcount;
            if (dstr == "") {
                dstr = tempe.prop("name") + ":" + tempcount;
            } else {
                dstr += "," + tempe.prop("name") + ":" + tempcount;
            }
        }

    })

    $("#ztzspan").html(ztz);
    if (ztz == 0) {
        return;
    }
    $("#insertval").val(dstr);
    if (confirm("确定投注金币：" + ztz)) {
        $.ajaxSetup({
            async: false
        });
        var countjb = $("#ujb").val();
        var tzjb = "";
        var tznb = "";
        var countjb = 0;
        for (var i = 0; i <= 27; i++) {
            var jb = $("#ztcount" + i + "").val();
            if (jb.length != 0) {
                tzjb += jb + ",";
                tznb += i + ",";
                countjb += parseInt(jb);
            }
        }
        var str = [];
        if (tzjb == "") {
            alert("对不起，请先投注！");
            return false;
        } else if (ztz > countjb) {//limit
            //showmsg("1", limit_msg);
            alert("对不起，金币不足！");
            return false;
        }
        else if (ztz < 10) {
            alert("最小投注金额10金币！");
            return false;
        } else {
            var Issue = $("#dqIssue").val();
            if (Issue == dqqs) {
                if (stopTz == 1) {
                    alert("对不起，已经停止投注！");
                    return false;
                }
            }
            var url = "/Js28/Index/1";
            if (gameid == 2) url = "/Bj28/Index/1";
            $.ajax({
                type: "POST",
                url: "/Tz/SetBetting",
                data: { "tzjb": tzjb, "tznb": tznb, "Issue": Issue, "countjb": ztz, "gameid": gameid },
                dataType: 'json',
                success: function (data) {
                    try {
                        if (data.msg == "1") {
                            window.location.href = url;
                        }
                        else if (data.msg == "2") {
                            alert("本期已经结束！");
                            window.location.href = url;
                        }
                        else if (data.msg == "3") {
                            alert("投注金额过大或者过小！");
                        }
                        else if (data.msg == "4") {
                            alert("已经停止投注！");
                        } else if (data.msg == "-2")
                        {
                            alert("该期总投注金额大于5千W！");
                            window.location.href = url;
                        }
                        else {
                            alert("投注失败！");
                        }
                    } catch (e) {
                        location.href = "/admin/error";
                    }
                }
            });
        }
    }
}


function tz(d, b) {

    var zl1 = 0;
    for (var ii1 = 0; ii1 < b.length; ii1++) {
        zl1 += parseInt(b[ii1]);
    }

    var allje1 = 0;
    var bl1 = d / zl1;

    var j1 = 0;
    $(".srinput").each(function () {

        var i1 = parseInt(b[j1] * bl1);
        if (!isNaN(i1) && i1 > 0) {
            allje1 += i1;


        }
        j1 = j1 + 1;


    })

    if (allje1 > max) {
        ds.dialog.tips("超出投注上限！", 1, true, true);
        return;
    }






    var zl = 0;
    for (var ii = 0; ii < b.length; ii++) {
        zl += parseInt(b[ii]);
    }
    if (zl == 0) {
        return;
    }
    var allje = 0;
    var bl = d / zl;

    var j = 0;
    $(".srinput").each(function () {
        var i = parseInt(b[j] * bl);
        if (!isNaN(i) && i > 0) {
            $(this).val(i);
            allje += i;
            $(this).parents(".items").find(".xzinputt").prop("checked", true);
        } else {
            $(this).val("");
            $(this).parents(".items").find(".xzinputt").prop("checked", false);
        }
        j = j + 1;
    })


    $("#ztzspan").html(allje);
}
function takechange(name, text) {
    var as = $("#a" + name + "").val().split(',');
    var bs = $("#b" + name + "").val().split(',');
    var cs = $("#c" + name + "").val();
    var a = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
    for (var i = 0; i < as.length; i++) {
        a[parseInt(as[i])] = bs[i];
    }

    $(".jbspan").html(cs);
    $("#model_id").val(name);
    jzzz(a);
    document.getElementById("ms").innerHTML = "<a class=\"current-mode\">当前正在编辑 | <em>" + text + "</em></a><a class=\"\" href=\"javascript:fx(xyqbbl)\">反选</a>";
    $("#TemName").val(text);
}
function takechanges(name) {
    if (typeof (name) == "undefined") {
        return;
    }
    var as = $("#aa" + name + "").val().split(',');
    var bs = $("#ab" + name + "").val().split(',');
    var cs = $("#ac" + name + "").val();
    var a = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
    for (var i = 0; i < as.length; i++) {
        a[parseInt(as[i])] = bs[i];
    }

    $(".jbspan").html(cs);
    jzzz(a);
}
function jzzz(a) {
    var selectz = $("#mymsselect").find("option:selected");
    $("#chosezindex").val(selectz.attr("z"));
    $("#TemName").val(selectz.text());
    $(".current-mode em").html(selectz.text());
    tz2(a);
}
function tz2(b) {
    var allje1 = 0;
    var j1 = 0;
    $(".srinput").each(function () {
        var i1 = parseInt(b[j1]);
        if (!isNaN(i1) && i1 > 0) {
            allje1 += i1;
        }

    })
    if (allje1 > max) {
        ds.dialog.tips("超出投注上限！", 1, true, true);
        return;
    }
    
    var allje = 0;
    var j = 0;
    $(".srinput").each(function () {
        var i = parseInt(b[j]);
        if (!isNaN(i) && i > 0) {
            $(this).val(i);
            allje += i;
            $(this).parents(".items").find(".xzinputt").prop("checked", true);
        } else {
            $(this).val("");
            $(this).parents(".items").find(".xzinputt").prop("checked", false);
        }

        j = j + 1;
    })
    $("#ztzspan").html(allje);
    //选中样式
    $(".items").find(".xzinputt").trigger('change');
}

function GetSq() {
    var as = $("#a1").val().split(',');
    var bs = $("#b1").val().split(',');
    var cs = $("#c1").val();
    var a = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
    for (var i = 0; i < as.length; i++) {
        a[parseInt(as[i])] = bs[i];
    }
    $(".jbspan").html(cs);
    jzzz(a);
}
function jzzz(a) {
    var selectz = $("#mymsselect").find("option:selected");
    $("#chosezindex").val(selectz.attr("z"));
    $("#TemName").val(selectz.text());
    tz2(a);
}
function qmxx(t, s) {

    var inp = $(t).parents(".items").find(".srinput");
    var i = inp.val();
    if (!isNaN(i) && i > 0) {
        var i2 = parseInt(i * s);
        if (!isNaN(i2) && i2 > 0) {
            inp.val(i2);
        } else {
            inp.val("");
            $(t).parents(".items").find(".xzinputt").prop("checked", false);
        }
    }
    jszbd();

}
function jszbd() {
    var allbd = 0;
    $(".srinput").each(
    function () {
        var i = parseInt($(this).val());
        if (!isNaN(i) && i > 0) {
            allbd = allbd + i;
        }
    });
    $("#ztzspan").html(allbd);
    $(".items").find(".xzinputt").trigger('change');
}
function allxx(s) {
    var z1 = 0;
    $(".srinput").each(function () {
        var i1 = parseFloat($(this).val());
        if (!isNaN(i1) && i1 > 0) {
            var i21 = parseInt(i1 * s);
        }
        if (i21 > 0) {
            z1 += i21;
        }
    })
    if (z1 > max) {
        ds.dialog.tips("超出投注上限！", 1, true, true);
        return;
    }

    var ztzs = 0;
    $(".srinput").each(function () {
        var i = parseFloat($(this).val());
        if (!isNaN(i) && i > 0) {
            var i2 = parseInt(i * s);
            if (i2 > 0) {
                $(this).val(i2);
                ztzs += i2;
            } else {
                $(this).val("");
                $(this).parents(".items").find(".xzinputt").prop("checked", false);

            }
        } else {
            $(this).val("");
            $(this).parents(".items").find(".xzinputt").prop("checked", false);
        }
    });



    $("#ztzspan").html(ztzs);
}
function fx(b) {
    var allje = 0;
    $(".srinput").each(function (j, t) {
        var thisval = parseInt($(t).val());
        if (isNaN(thisval) || thisval <= 0) {
            var i = parseInt(b[j]);
            if (!isNaN(i) && i > 0) {
                $(t).val(i);
                $(t).parents(".items").find(".xzinputt").prop("checked", true);
                allje += i;
            }

        } else {
            $(t).val("");
            $(t).parents(".items").find(".xzinputt").prop("checked", false);
        }
    })
    $("#ztzspan").html(allje);
}

function souha(s) {
    var s1 = parseInt(s);
    if (s1 > max) {
        s1 = max;
    }

    var list = [];
    $(".srinput").each(function () {
        var i = parseInt($(this).val());
        if (!isNaN(i) && i > 0) {
            list.push(i);
        } else {
            list.push(0);
        }
    })





    tz(s1, list);
}
function qk() {
    $(".srinput").val("");
    $(".xzinputt").prop("checked", false);
    $("#ztzspan").html(0);
}
function sxpll(mid) {
    $.get("/game/xy28Handler.ashx?action=getpl&messid=" + mid + "&t=" + parseInt(Math.random() * 10000), function (data) {
        var list = [];
        eval("list=" + data);
        var i = 0;
        $(".pltddff").each(function () {
            $(this).html(list[i]);
            i = i + 1;

        });
    });
}
function checkclick(t) {
    if ($(t).prop("checked")) {
        var tinp = $(t).parents(".items").find(".srinput")[0];

        var i = $(".srinput").index(tinp);
        if (xyqbbl.length > i) {
            tinp.value = xyqbbl[i];
        } else {
            tinp.value = "10";
        }
        jszbd();

    } else {
        $(t).parents(".items").find(".srinput").val("");
        jszbd();

    }
}
//旧版本生成标准赔率,现在是直接生成html展示了
function load_bzpl() {

    var allje = 0;
    for (var i = 0; i < xyqbbl.length; i++) {
        allje += xyqbbl[i];
    }
    $(".pl_bzpltd").each(function (ii, th) {
        var dq = parseFloat(xyqbbl[ii]);
        $(th).html((allje / dq).toFixed(2));
        //$(th).html((allje / dq).toFixed(4));
    })
}
function qb(b) {
    tz2(b);
}
function tz_dan(b) {




    var allje = 0;
    $(".srinput").each(function (j, t) {
        var i = parseInt(b[j]);
        if (j % 2 == shuangysz) {
            i = 0;
        }
        if (!isNaN(i) && i > 0) {
            $(t).val(i);
            allje += i;
            $(t).parents(".items").find(".xzinputt").prop("checked", true);
        } else {
            $(t).val("");
            $(t).parents(".items").find(".xzinputt").prop("checked", false);
        }
    })
    $("#ztzspan").html(allje);
}
function tz_shuang(b) {
    var allje = 0;
    $(".srinput").each(function (j, t) {
        var i = parseInt(b[j]);
        if (j % 2 != shuangysz) {
            i = 0;
        }
        if (!isNaN(i) && i > 0) {
            $(t).val(i);
            allje += i;
            $(t).parents(".items").find(".xzinputt").prop("checked", true);
        } else {
            $(t).val("");
            $(t).parents(".items").find(".xzinputt").prop("checked", false);
        }
    })
    $("#ztzspan").html(allje);
}
function tz_da(b) {
    var allje = 0;
    $(".srinput").each(function (j, t) {
        var i = parseInt(b[j]);
        if (j < daks) {
            i = 0;
        }
        if (!isNaN(i) && i > 0) {
            $(t).val(i);
            allje += i;
            $(t).parents(".items").find(".xzinputt").prop("checked", true);
        } else {
            $(t).val("");
            $(t).parents(".items").find(".xzinputt").prop("checked", false);
        }
    })
    $("#ztzspan").html(allje);
}
function tz_xiao(b) {
    var allje = 0;
    $(".srinput").each(function (j, t) {
        var i = parseInt(b[j]);
        if (j >= daks) {
            i = 0;
        }
        if (!isNaN(i) && i > 0) {
            $(t).val(i);
            allje += i;
            $(t).parents(".items").find(".xzinputt").prop("checked", true);
        } else {
            $(t).val("");
            $(t).parents(".items").find(".xzinputt").prop("checked", false);
        }
    })
    $("#ztzspan").html(allje);
}
function tz_zhong(b) {
    var allje = 0;
    $(".srinput").each(function (j, t) {
        var i = parseInt(b[j]);
        if (j < zhongks || j > zhongjs) {
            i = 0;
        }
        if (!isNaN(i) && i > 0) {
            $(t).val(i);
            allje += i;
            $(t).parents(".items").find(".xzinputt").prop("checked", true);
        } else {
            $(t).val("");
            $(t).parents(".items").find(".xzinputt").prop("checked", false);
        }
    })
    $("#ztzspan").html(allje);
}
function tz_bian(b) {
    var allje = 0;
    $(".srinput").each(function (j, t) {
        var i = parseInt(b[j]);
        if (j >= zhongks && j <= zhongjs) {
            i = 0;
        }
        if (!isNaN(i) && i > 0) {
            $(t).val(i);
            allje += i;
            $(t).parents(".items").find(".xzinputt").prop("checked", true);
        } else {
            $(t).val("");
            $(t).parents(".items").find(".xzinputt").prop("checked", false);
        }
    })
    $("#ztzspan").html(allje);
}

function tz_da_dan(b) {
    var allje = 0;
    $(".srinput").each(function (j, t) {
        var i = parseInt(b[j]);
        if (j < daks || j % 2 == shuangysz) {
            i = 0;
        }
        if (!isNaN(i) && i > 0) {
            $(t).val(i);
            allje += i;
            $(t).parents(".items").find(".xzinputt").prop("checked", true);
        } else {
            $(t).val("");
            $(t).parents(".items").find(".xzinputt").prop("checked", false);
        }
    })
    $("#ztzspan").html(allje);
}
function tz_da_shuang(b) {
    var allje = 0;
    $(".srinput").each(function (j, t) {
        var i = parseInt(b[j]);
        if (j < daks || j % 2 == danysz) {
            i = 0;
        }
        if (!isNaN(i) && i > 0) {
            $(t).val(i);
            allje += i;
            $(t).parents(".items").find(".xzinputt").prop("checked", true);
        } else {
            $(t).val("");
            $(t).parents(".items").find(".xzinputt").prop("checked", false);
        }
    })
    $("#ztzspan").html(allje);
}
function tz_da_bian(b) {
    var allje = 0;
    $(".srinput").each(function (j, t) {
        var i = parseInt(b[j]);
        if (j < daks || (j >= zhongks && j <= zhongjs)) {
            i = 0;
        }
        if (!isNaN(i) && i > 0) {
            $(t).val(i);
            allje += i;
            $(t).parents(".items").find(".xzinputt").prop("checked", true);
        } else {
            $(t).val("");
            $(t).parents(".items").find(".xzinputt").prop("checked", false);
        }
    })
    $("#ztzspan").html(allje);
}
function tz_xiao_dan(b) {
    var allje = 0;
    $(".srinput").each(function (j, t) {
        var i = parseInt(b[j]);
        if (j >= daks || j % 2 == shuangysz) {
            i = 0;
        }
        if (!isNaN(i) && i > 0) {
            $(t).val(i);
            allje += i;
            $(t).parents(".items").find(".xzinputt").prop("checked", true);
        } else {
            $(t).val("");
            $(t).parents(".items").find(".xzinputt").prop("checked", false);
        }
    })
    $("#ztzspan").html(allje);
}
function tz_xiao_shuang(b) {
    var allje = 0;
    $(".srinput").each(function (j, t) {
        var i = parseInt(b[j]);
        if (j >= daks || j % 2 == danysz) {
            i = 0;
        }
        if (!isNaN(i) && i > 0) {
            $(t).val(i);
            allje += i;
            $(t).parents(".items").find(".xzinputt").prop("checked", true);
        } else {
            $(t).val("");
            $(t).parents(".items").find(".xzinputt").prop("checked", false);
        }
    })
    $("#ztzspan").html(allje);
}
function tz_xiao_bian(b) {
    var allje = 0;
    $(".srinput").each(function (j, t) {
        var i = parseInt(b[j]);
        if (j >= daks || (j >= zhongks && j <= zhongjs)) {
            i = 0;
        }
        if (!isNaN(i) && i > 0) {
            $(t).val(i);
            allje += i;
            $(t).parents(".items").find(".xzinputt").prop("checked", true);
        } else {
            $(t).val("");
            $(t).parents(".items").find(".xzinputt").prop("checked", false);
        }
    })
    $("#ztzspan").html(allje);
}
function tz_dan_bian(b) {
    var allje = 0;
    $(".srinput").each(function (j, t) {
        var i = parseInt(b[j]);
        if (j % 2 == shuangysz || (j >= zhongks && j <= zhongjs)) {
            i = 0;
        }
        if (!isNaN(i) && i > 0) {
            $(t).val(i);
            allje += i;
            $(t).parents(".items").find(".xzinputt").prop("checked", true);
        } else {
            $(t).val("");
            $(t).parents(".items").find(".xzinputt").prop("checked", false);
        }
    })
    $("#ztzspan").html(allje);
}
function tz_shuang_bian(b) {
    var allje = 0;
    $(".srinput").each(function (j, t) {
        var i = parseInt(b[j]);
        if (j % 2 == danysz || (j >= zhongks && j <= zhongjs)) {
            i = 0;
        }
        if (!isNaN(i) && i > 0) {
            $(t).val(i);
            allje += i;
            $(t).parents(".items").find(".xzinputt").prop("checked", true);
        } else {
            $(t).val("");
            $(t).parents(".items").find(".xzinputt").prop("checked", false);
        }
    })
    $("#ztzspan").html(allje);
}

function tz_wei(b, cs, ws) {
    var allje = 0;
    $(".srinput").each(function (j, t) {
        var i = parseInt(b[j]);
        if ((j - weishuc) % cs != ws) {
            i = 0;
        }
        if (!isNaN(i) && i > 0) {
            $(t).val(i);
            allje += i;
            $(t).parents(".items").find(".xzinputt").prop("checked", true);
        } else {
            $(t).val("");
            $(t).parents(".items").find(".xzinputt").prop("checked", false);
        }
    })
    $("#ztzspan").html(allje);
}
function tz_xiaowei(b) {
    var allje = 0;
    $(".srinput").each(function (j, t) {
        var i = parseInt(b[j]);
        if ((j - weishuc) % 10 >= 5) {
            i = 0;
        }
        if (!isNaN(i) && i > 0) {
            $(t).val(i);
            allje += i;
            $(t).parents(".items").find(".xzinputt").prop("checked", true);
        } else {
            $(t).val("");
            $(t).parents(".items").find(".xzinputt").prop("checked", false);
        }
    })
    $("#ztzspan").html(allje);
}
function tz_dawei(b) {
    var allje = 0;
    $(".srinput").each(function (j, t) {
        var i = parseInt(b[j]);
        if ((j - weishuc) % 10 < 5) {
            i = 0;
        }
        if (!isNaN(i) && i > 0) {
            $(t).val(i);
            allje += i;
            $(t).parents(".items").find(".xzinputt").prop("checked", true);
        } else {
            $(t).val("");
            $(t).parents(".items").find(".xzinputt").prop("checked", false);
        }
    })
    $("#ztzspan").html(allje);
}


$(function () {
    $('body').on('click', '.hama', function () {
        $(this).find('input').trigger('click');
    });


    $('body').on('focus', '.srinput', function () {
        var bbb = $(this).val();
        if (bbb == "") {
            $(this).prev().find('input').trigger('click');
        } else { }

    });

    $('body').on('change', '.srinput', function () {
        var ztz = 0;
        var dstr = "";
        $(".srinput").each(function (i, e) {
            var tempe = $(e);
            var tempcount = parseInt(tempe.val());
            if (!isNaN(tempcount) && tempcount > 0) {
                ztz += tempcount;
                if (dstr == "") {
                    dstr = tempe.prop("name") + ":" + tempcount;
                } else {
                    dstr += "," + tempe.prop("name") + ":" + tempcount;
                }
            }

        })

        $("#ztzspan").html(ztz);

    });



    $('body').on('change', '.xzinputt', function () {
        var that = $(this);
        var parent = that.parents('.items');
        if (that.prop("checked")) {
            parent.addClass('selected');
        } else {
            parent.removeClass('selected');
        }
    });

    $('.panel').on('click', 'a,span', function () {
        window.setTimeout(function () {
            $(".items").find(".xzinputt").trigger('change');
        }, 50)

    });


    $('.add-tz').on('click', 'a', function () {
        window.setTimeout(function () {
            $(".items").find(".xzinputt").trigger('change');
        }, 50)

    })


    setTimeout(function () {
        $(".items").find(".xzinputt").trigger('change');
    }, 50);

    $('.tabwidgit').on('click', ".menu span", function () {
        $(this).addClass('current').siblings().removeClass('current');
        var index = $(this).index();
        var panel = $(this).parents('.tabwidgit').find('.panel');
        panel.eq(index).addClass('current').siblings().removeClass('current');
    })
})





/*
* jQuery Form Plugin; v20131121
* http://jquery.malsup.com/form/
* Copyright (c) 2013 M. Alsup; Dual licensed: MIT/GPL
* https://github.com/malsup/form#copyright-and-license
*/
; (function (e) { "function" == typeof define && define.amd ? define(["jquery"], e) : e("undefined" != typeof jQuery ? jQuery : window.Zepto) })(function (e) { "use strict"; function t(t) { var r = t.data; t.isDefaultPrevented() || (t.preventDefault(), e(t.target).ajaxSubmit(r)) } function r(t) { var r = t.target, a = e(r); if (!a.is("[type=submit],[type=image]")) { var n = a.closest("[type=submit]"); if (0 === n.length) return; r = n[0] } var i = this; if (i.clk = r, "image" == r.type) if (void 0 !== t.offsetX) i.clk_x = t.offsetX, i.clk_y = t.offsetY; else if ("function" == typeof e.fn.offset) { var o = a.offset(); i.clk_x = t.pageX - o.left, i.clk_y = t.pageY - o.top } else i.clk_x = t.pageX - r.offsetLeft, i.clk_y = t.pageY - r.offsetTop; setTimeout(function () { i.clk = i.clk_x = i.clk_y = null }, 100) } function a() { if (e.fn.ajaxSubmit.debug) { var t = "[jquery.form] " + Array.prototype.join.call(arguments, ""); window.console && window.console.log ? window.console.log(t) : window.opera && window.opera.postError && window.opera.postError(t) } } var n = {}; n.fileapi = void 0 !== e("<input type='file'/>").get(0).files, n.formdata = void 0 !== window.FormData; var i = !!e.fn.prop; e.fn.attr2 = function () { if (!i) return this.attr.apply(this, arguments); var e = this.prop.apply(this, arguments); return e && e.jquery || "string" == typeof e ? e : this.attr.apply(this, arguments) }, e.fn.ajaxSubmit = function (t) { function r(r) { var a, n, i = e.param(r, t.traditional).split("&"), o = i.length, s = []; for (a = 0; o > a; a++) i[a] = i[a].replace(/\+/g, " "), n = i[a].split("="), s.push([decodeURIComponent(n[0]), decodeURIComponent(n[1])]); return s } function o(a) { for (var n = new FormData, i = 0; a.length > i; i++) n.append(a[i].name, a[i].value); if (t.extraData) { var o = r(t.extraData); for (i = 0; o.length > i; i++) o[i] && n.append(o[i][0], o[i][1]) } t.data = null; var s = e.extend(!0, {}, e.ajaxSettings, t, { contentType: !1, processData: !1, cache: !1, type: u || "POST" }); t.uploadProgress && (s.xhr = function () { var r = e.ajaxSettings.xhr(); return r.upload && r.upload.addEventListener("progress", function (e) { var r = 0, a = e.loaded || e.position, n = e.total; e.lengthComputable && (r = Math.ceil(100 * (a / n))), t.uploadProgress(e, a, n, r) }, !1), r }), s.data = null; var c = s.beforeSend; return s.beforeSend = function (e, r) { r.data = t.formData ? t.formData : n, c && c.call(this, e, r) }, e.ajax(s) } function s(r) { function n(e) { var t = null; try { e.contentWindow && (t = e.contentWindow.document) } catch (r) { a("cannot get iframe.contentWindow document: " + r) } if (t) return t; try { t = e.contentDocument ? e.contentDocument : e.document } catch (r) { a("cannot get iframe.contentDocument: " + r), t = e.document } return t } function o() { function t() { try { var e = n(g).readyState; a("state = " + e), e && "uninitialized" == e.toLowerCase() && setTimeout(t, 50) } catch (r) { a("Server abort: ", r, " (", r.name, ")"), s(k), j && clearTimeout(j), j = void 0 } } var r = f.attr2("target"), i = f.attr2("action"); w.setAttribute("target", p), (!u || /post/i.test(u)) && w.setAttribute("method", "POST"), i != m.url && w.setAttribute("action", m.url), m.skipEncodingOverride || u && !/post/i.test(u) || f.attr({ encoding: "multipart/form-data", enctype: "multipart/form-data" }), m.timeout && (j = setTimeout(function () { T = !0, s(D) }, m.timeout)); var o = []; try { if (m.extraData) for (var c in m.extraData) m.extraData.hasOwnProperty(c) && (e.isPlainObject(m.extraData[c]) && m.extraData[c].hasOwnProperty("name") && m.extraData[c].hasOwnProperty("value") ? o.push(e('<input type="hidden" name="' + m.extraData[c].name + '">').val(m.extraData[c].value).appendTo(w)[0]) : o.push(e('<input type="hidden" name="' + c + '">').val(m.extraData[c]).appendTo(w)[0])); m.iframeTarget || v.appendTo("body"), g.attachEvent ? g.attachEvent("onload", s) : g.addEventListener("load", s, !1), setTimeout(t, 15); try { w.submit() } catch (l) { var d = document.createElement("form").submit; d.apply(w) } } finally { w.setAttribute("action", i), r ? w.setAttribute("target", r) : f.removeAttr("target"), e(o).remove() } } function s(t) { if (!x.aborted && !F) { if (M = n(g), M || (a("cannot access response document"), t = k), t === D && x) return x.abort("timeout"), S.reject(x, "timeout"), void 0; if (t == k && x) return x.abort("server abort"), S.reject(x, "error", "server abort"), void 0; if (M && M.location.href != m.iframeSrc || T) { g.detachEvent ? g.detachEvent("onload", s) : g.removeEventListener("load", s, !1); var r, i = "success"; try { if (T) throw "timeout"; var o = "xml" == m.dataType || M.XMLDocument || e.isXMLDoc(M); if (a("isXml=" + o), !o && window.opera && (null === M.body || !M.body.innerHTML) && --O) return a("requeing onLoad callback, DOM not available"), setTimeout(s, 250), void 0; var u = M.body ? M.body : M.documentElement; x.responseText = u ? u.innerHTML : null, x.responseXML = M.XMLDocument ? M.XMLDocument : M, o && (m.dataType = "xml"), x.getResponseHeader = function (e) { var t = { "content-type": m.dataType }; return t[e.toLowerCase()] }, u && (x.status = Number(u.getAttribute("status")) || x.status, x.statusText = u.getAttribute("statusText") || x.statusText); var c = (m.dataType || "").toLowerCase(), l = /(json|script|text)/.test(c); if (l || m.textarea) { var f = M.getElementsByTagName("textarea")[0]; if (f) x.responseText = f.value, x.status = Number(f.getAttribute("status")) || x.status, x.statusText = f.getAttribute("statusText") || x.statusText; else if (l) { var p = M.getElementsByTagName("pre")[0], h = M.getElementsByTagName("body")[0]; p ? x.responseText = p.textContent ? p.textContent : p.innerText : h && (x.responseText = h.textContent ? h.textContent : h.innerText) } } else "xml" == c && !x.responseXML && x.responseText && (x.responseXML = X(x.responseText)); try { E = _(x, c, m) } catch (b) { i = "parsererror", x.error = r = b || i } } catch (b) { a("error caught: ", b), i = "error", x.error = r = b || i } x.aborted && (a("upload aborted"), i = null), x.status && (i = x.status >= 200 && 300 > x.status || 304 === x.status ? "success" : "error"), "success" === i ? (m.success && m.success.call(m.context, E, "success", x), S.resolve(x.responseText, "success", x), d && e.event.trigger("ajaxSuccess", [x, m])) : i && (void 0 === r && (r = x.statusText), m.error && m.error.call(m.context, x, i, r), S.reject(x, "error", r), d && e.event.trigger("ajaxError", [x, m, r])), d && e.event.trigger("ajaxComplete", [x, m]), d && ! --e.active && e.event.trigger("ajaxStop"), m.complete && m.complete.call(m.context, x, i), F = !0, m.timeout && clearTimeout(j), setTimeout(function () { m.iframeTarget ? v.attr("src", m.iframeSrc) : v.remove(), x.responseXML = null }, 100) } } } var c, l, m, d, p, v, g, x, b, y, T, j, w = f[0], S = e.Deferred(); if (S.abort = function (e) { x.abort(e) }, r) for (l = 0; h.length > l; l++) c = e(h[l]), i ? c.prop("disabled", !1) : c.removeAttr("disabled"); if (m = e.extend(!0, {}, e.ajaxSettings, t), m.context = m.context || m, p = "jqFormIO" + (new Date).getTime(), m.iframeTarget ? (v = e(m.iframeTarget), y = v.attr2("name"), y ? p = y : v.attr2("name", p)) : (v = e('<iframe name="' + p + '" src="' + m.iframeSrc + '" />'), v.css({ position: "absolute", top: "-1000px", left: "-1000px" })), g = v[0], x = { aborted: 0, responseText: null, responseXML: null, status: 0, statusText: "n/a", getAllResponseHeaders: function () { }, getResponseHeader: function () { }, setRequestHeader: function () { }, abort: function (t) { var r = "timeout" === t ? "timeout" : "aborted"; a("aborting upload... " + r), this.aborted = 1; try { g.contentWindow.document.execCommand && g.contentWindow.document.execCommand("Stop") } catch (n) { } v.attr("src", m.iframeSrc), x.error = r, m.error && m.error.call(m.context, x, r, t), d && e.event.trigger("ajaxError", [x, m, r]), m.complete && m.complete.call(m.context, x, r) } }, d = m.global, d && 0 === e.active++ && e.event.trigger("ajaxStart"), d && e.event.trigger("ajaxSend", [x, m]), m.beforeSend && m.beforeSend.call(m.context, x, m) === !1) return m.global && e.active--, S.reject(), S; if (x.aborted) return S.reject(), S; b = w.clk, b && (y = b.name, y && !b.disabled && (m.extraData = m.extraData || {}, m.extraData[y] = b.value, "image" == b.type && (m.extraData[y + ".x"] = w.clk_x, m.extraData[y + ".y"] = w.clk_y))); var D = 1, k = 2, A = e("meta[name=csrf-token]").attr("content"), L = e("meta[name=csrf-param]").attr("content"); L && A && (m.extraData = m.extraData || {}, m.extraData[L] = A), m.forceSync ? o() : setTimeout(o, 10); var E, M, F, O = 50, X = e.parseXML || function (e, t) { return window.ActiveXObject ? (t = new ActiveXObject("Microsoft.XMLDOM"), t.async = "false", t.loadXML(e)) : t = (new DOMParser).parseFromString(e, "text/xml"), t && t.documentElement && "parsererror" != t.documentElement.nodeName ? t : null }, C = e.parseJSON || function (e) { return window.eval("(" + e + ")") }, _ = function (t, r, a) { var n = t.getResponseHeader("content-type") || "", i = "xml" === r || !r && n.indexOf("xml") >= 0, o = i ? t.responseXML : t.responseText; return i && "parsererror" === o.documentElement.nodeName && e.error && e.error("parsererror"), a && a.dataFilter && (o = a.dataFilter(o, r)), "string" == typeof o && ("json" === r || !r && n.indexOf("json") >= 0 ? o = C(o) : ("script" === r || !r && n.indexOf("javascript") >= 0) && e.globalEval(o)), o }; return S } if (!this.length) return a("ajaxSubmit: skipping submit process - no element selected"), this; var u, c, l, f = this; "function" == typeof t ? t = { success: t } : void 0 === t && (t = {}), u = t.type || this.attr2("method"), c = t.url || this.attr2("action"), l = "string" == typeof c ? e.trim(c) : "", l = l || window.location.href || "", l && (l = (l.match(/^([^#]+)/) || [])[1]), t = e.extend(!0, { url: l, success: e.ajaxSettings.success, type: u || e.ajaxSettings.type, iframeSrc: /^https/i.test(window.location.href || "") ? "javascript:false" : "about:blank" }, t); var m = {}; if (this.trigger("form-pre-serialize", [this, t, m]), m.veto) return a("ajaxSubmit: submit vetoed via form-pre-serialize trigger"), this; if (t.beforeSerialize && t.beforeSerialize(this, t) === !1) return a("ajaxSubmit: submit aborted via beforeSerialize callback"), this; var d = t.traditional; void 0 === d && (d = e.ajaxSettings.traditional); var p, h = [], v = this.formToArray(t.semantic, h); if (t.data && (t.extraData = t.data, p = e.param(t.data, d)), t.beforeSubmit && t.beforeSubmit(v, this, t) === !1) return a("ajaxSubmit: submit aborted via beforeSubmit callback"), this; if (this.trigger("form-submit-validate", [v, this, t, m]), m.veto) return a("ajaxSubmit: submit vetoed via form-submit-validate trigger"), this; var g = e.param(v, d); p && (g = g ? g + "&" + p : p), "GET" == t.type.toUpperCase() ? (t.url += (t.url.indexOf("?") >= 0 ? "&" : "?") + g, t.data = null) : t.data = g; var x = []; if (t.resetForm && x.push(function () { f.resetForm() }), t.clearForm && x.push(function () { f.clearForm(t.includeHidden) }), !t.dataType && t.target) { var b = t.success || function () { }; x.push(function (r) { var a = t.replaceTarget ? "replaceWith" : "html"; e(t.target)[a](r).each(b, arguments) }) } else t.success && x.push(t.success); if (t.success = function (e, r, a) { for (var n = t.context || this, i = 0, o = x.length; o > i; i++) x[i].apply(n, [e, r, a || f, f]) }, t.error) { var y = t.error; t.error = function (e, r, a) { var n = t.context || this; y.apply(n, [e, r, a, f]) } } if (t.complete) { var T = t.complete; t.complete = function (e, r) { var a = t.context || this; T.apply(a, [e, r, f]) } } var j = e("input[type=file]:enabled", this).filter(function () { return "" !== e(this).val() }), w = j.length > 0, S = "multipart/form-data", D = f.attr("enctype") == S || f.attr("encoding") == S, k = n.fileapi && n.formdata; a("fileAPI :" + k); var A, L = (w || D) && !k; t.iframe !== !1 && (t.iframe || L) ? t.closeKeepAlive ? e.get(t.closeKeepAlive, function () { A = s(v) }) : A = s(v) : A = (w || D) && k ? o(v) : e.ajax(t), f.removeData("jqxhr").data("jqxhr", A); for (var E = 0; h.length > E; E++) h[E] = null; return this.trigger("form-submit-notify", [this, t]), this }, e.fn.ajaxForm = function (n) { if (n = n || {}, n.delegation = n.delegation && e.isFunction(e.fn.on), !n.delegation && 0 === this.length) { var i = { s: this.selector, c: this.context }; return !e.isReady && i.s ? (a("DOM not ready, queuing ajaxForm"), e(function () { e(i.s, i.c).ajaxForm(n) }), this) : (a("terminating; zero elements found by selector" + (e.isReady ? "" : " (DOM not ready)")), this) } return n.delegation ? (e(document).off("submit.form-plugin", this.selector, t).off("click.form-plugin", this.selector, r).on("submit.form-plugin", this.selector, n, t).on("click.form-plugin", this.selector, n, r), this) : this.ajaxFormUnbind().bind("submit.form-plugin", n, t).bind("click.form-plugin", n, r) }, e.fn.ajaxFormUnbind = function () { return this.unbind("submit.form-plugin click.form-plugin") }, e.fn.formToArray = function (t, r) { var a = []; if (0 === this.length) return a; var i = this[0], o = t ? i.getElementsByTagName("*") : i.elements; if (!o) return a; var s, u, c, l, f, m, d; for (s = 0, m = o.length; m > s; s++) if (f = o[s], c = f.name, c && !f.disabled) if (t && i.clk && "image" == f.type) i.clk == f && (a.push({ name: c, value: e(f).val(), type: f.type }), a.push({ name: c + ".x", value: i.clk_x }, { name: c + ".y", value: i.clk_y })); else if (l = e.fieldValue(f, !0), l && l.constructor == Array) for (r && r.push(f), u = 0, d = l.length; d > u; u++) a.push({ name: c, value: l[u] }); else if (n.fileapi && "file" == f.type) { r && r.push(f); var p = f.files; if (p.length) for (u = 0; p.length > u; u++) a.push({ name: c, value: p[u], type: f.type }); else a.push({ name: c, value: "", type: f.type }) } else null !== l && l !== void 0 && (r && r.push(f), a.push({ name: c, value: l, type: f.type, required: f.required })); if (!t && i.clk) { var h = e(i.clk), v = h[0]; c = v.name, c && !v.disabled && "image" == v.type && (a.push({ name: c, value: h.val() }), a.push({ name: c + ".x", value: i.clk_x }, { name: c + ".y", value: i.clk_y })) } return a }, e.fn.formSerialize = function (t) { return e.param(this.formToArray(t)) }, e.fn.fieldSerialize = function (t) { var r = []; return this.each(function () { var a = this.name; if (a) { var n = e.fieldValue(this, t); if (n && n.constructor == Array) for (var i = 0, o = n.length; o > i; i++) r.push({ name: a, value: n[i] }); else null !== n && n !== void 0 && r.push({ name: this.name, value: n }) } }), e.param(r) }, e.fn.fieldValue = function (t) { for (var r = [], a = 0, n = this.length; n > a; a++) { var i = this[a], o = e.fieldValue(i, t); null === o || void 0 === o || o.constructor == Array && !o.length || (o.constructor == Array ? e.merge(r, o) : r.push(o)) } return r }, e.fieldValue = function (t, r) { var a = t.name, n = t.type, i = t.tagName.toLowerCase(); if (void 0 === r && (r = !0), r && (!a || t.disabled || "reset" == n || "button" == n || ("checkbox" == n || "radio" == n) && !t.checked || ("submit" == n || "image" == n) && t.form && t.form.clk != t || "select" == i && -1 == t.selectedIndex)) return null; if ("select" == i) { var o = t.selectedIndex; if (0 > o) return null; for (var s = [], u = t.options, c = "select-one" == n, l = c ? o + 1 : u.length, f = c ? o : 0; l > f; f++) { var m = u[f]; if (m.selected) { var d = m.value; if (d || (d = m.attributes && m.attributes.value && !m.attributes.value.specified ? m.text : m.value), c) return d; s.push(d) } } return s } return e(t).val() }, e.fn.clearForm = function (t) { return this.each(function () { e("input,select,textarea", this).clearFields(t) }) }, e.fn.clearFields = e.fn.clearInputs = function (t) { var r = /^(?:color|date|datetime|email|month|number|password|range|search|tel|text|time|url|week)$/i; return this.each(function () { var a = this.type, n = this.tagName.toLowerCase(); r.test(a) || "textarea" == n ? this.value = "" : "checkbox" == a || "radio" == a ? this.checked = !1 : "select" == n ? this.selectedIndex = -1 : "file" == a ? /MSIE/.test(navigator.userAgent) ? e(this).replaceWith(e(this).clone(!0)) : e(this).val("") : t && (t === !0 && /hidden/.test(a) || "string" == typeof t && e(this).is(t)) && (this.value = "") }) }, e.fn.resetForm = function () { return this.each(function () { ("function" == typeof this.reset || "object" == typeof this.reset && !this.reset.nodeType) && this.reset() }) }, e.fn.enable = function (e) { return void 0 === e && (e = !0), this.each(function () { this.disabled = !e }) }, e.fn.selected = function (t) { return void 0 === t && (t = !0), this.each(function () { var r = this.type; if ("checkbox" == r || "radio" == r) this.checked = t; else if ("option" == this.tagName.toLowerCase()) { var a = e(this).parent("select"); t && a[0] && "select-one" == a[0].type && a.find("option").selected(!1), this.selected = t } }) }, e.fn.ajaxSubmit.debug = !1 });

