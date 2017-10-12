var scount;
var tztzms = 0; //截止投注秒数在top模板里ajax付值
var kjms = 0;   //截止开奖秒数在top模板里ajax付值
var iCount;     //setInterval附值变量,在top模板里ajax付值
var indexis;    //游戏ID
var gx = 0;
var slee = 0;
var stopTz = 0; //停止投注
function setsc() {
    gx++;
    if ((tztzms == 65) && (indexis == 1 || indexis == 2)) {

        //Jb18747
        var iss = $("#DqIssue").val();
        var url = "/Js28/Get_TzJb";
        if (indexis == 2) url = "/Bj28/Get_TzJb";
        $.ajax({
            type: "POST",
            url: url,
            data: { "iss": iss },
            dataType: 'json',
            success: function (data) {
                if (data.msg == 0) {
                    gx = 0;
                    gx = -5;
                } else {
                    $("#Jb" + iss + "").text(data.msg);
                    if (data.txt == 1) {
                        if (indexis == 1) {
                            $("#lasttd" + iss).children("a").html("<a href=\"/Tz/insert/" + iss + "\"><span class=\"yitouz\">已投注</span></a>");
                        } else {

                            $("#lasttd" + iss).children("a").html("<a href=\"/Bj28/insert/" + iss + "\"><span class=\"yitouz\">已投注</span></a>");
                        }
                        $(".jb").html("<img src=\"/img/gold.png\"/>" + data.Jb + "");
                        $(".yhld .iright em").html(data.Jb); //兑换页面
                        gx = 100;
                    }
                }
            }
        });
    }
    if (tztzms > 0) {
        $("#tztzms").html(tztzms);  //投注截止秒数
        $("#msspan").html(kjms);    //开奖截止秒数
    } else {
        if (kjms > 0) {
            $("#tztzms").html(0);   //投注截止秒数
            $("#msspan").html(kjms);    //开奖截止秒数
            $("#lasttd" + dqqs).html("<span class=\"yitouz\">投注结束</span>");
            stopTz = 1;
           
        } else {
            if (iskj != 1) {
                tztzms = 3;
                slee++;
                $('.kjq,.xyjg').css('display', 'none');
                $('.kjz').css('display', 'block');
                $("#msspanmmm").html("开奖中...");
                $("#msspan").html("");
                //$("#lasttd" + dqqs).html("<img src=\"/image/kjing.gif\" />");
                $("#lasttd" + dqqs).html("<span class=\"yitouz\">开奖中</span>");
                if (slee ==3)
                {
                    slee = 0;

                    if (indexis == 1 || indexis == 2) {
                        var url = "/Js28/Get_CaChe";
                        if (indexis == 2) url = "/Bj28/Get_CaChe";
                        var iss = $("#DqIssue").val();
                        $.ajax({
                            type: "POST",
                            url: url,
                            data: { "iss": iss },
                            dataType: 'json',
                            success: function (data) {
                                gx = 0;
                                if (data.zjb != "True") {
                                    if (indexis == 2)
                                        tztzms = 10;
                                    else
                                        window.location.href = window.location.href;
                                    
                                } else {
                                    clearInterval(iCount);
                                    var xiss = parseInt(data.iss) + 1;
                                    dqqs = xiss;
                                    $("#DqIssue").val(xiss);
                                    var tr = $("#Winning tr").eq(4);
                                    tr.after(data.h1.replace("yitouz", "color1").replace("开奖中", "已开奖"));
                                    var tr = $("#Winning tr").eq(4);
                                    tr.last().remove();
                                    var tr = $("#Winning tr").eq(0);
                                    tr.after("<tr id=\"a1ac\">" + tr.html() + "</tr>");
                                    var tr = $("#Winning tr").eq(0);
                                    tr.after(data.h2);
                                    var tr = $("#Winning tr").eq(0);
                                    tr.last().remove();
                                    var tr = $("#Winning tr").eq(20);
                                    tr.last().remove();
                                    TopJz(indexis);
                                }

                                zdtzgg();
                            }
                        });
                    }


                }
            }
        }
    }
    kjms = kjms - 1;
    tztzms = tztzms - 1;
}
//AJAX更新自动投注信息
function zdtzgg() {
    $.ajax({
        type: "POST",
        url: "/Shared/Getzdtz",
        data: { "gameid": indexis },
        dataType: 'json',
        success: function (data) {
            if (data.code == 1000) {
                $(".zddz em").eq(0).html(data.danqian);
                $(".zddz em").eq(1).html(data.success);
                $(".zddz em").eq(2).html(data.loser);
                $(".zddz span").eq(0).html(data.jbsm);
            } else { }
           
        }
    });
}

function TopJz(gameid)
{
    $.ajax({
        type: "POST",
        url: "/Shared/Get90sx",
        data: { "gameid": gameid },
        dataType: 'json',
        success: function (data) {
            // document.getElementById("diqi").innerHTML = data.msg;
            document.getElementById("tztzms").innerHTML = data.jizhis;
            document.getElementById("msspan").innerHTML = data.seconds;
            $(".bianchu").html(data.Iss);
            var syq = parseInt(data.Iss) - 1;
            $("#syq").html(syq);
            if (parseInt(data.number) < 0) {
                $("#lotresult" + dqqs).html("<span class=\"lotresult\">-</span>");
            } else {
                $("#lotresult" + dqqs).html(data.Issue + "=<span id=\"resultNums\" class=\"resultNum\">" + data.number + "</span>");
            }

            $(".jb").html("<img src=\"/img/gold.png\"/>" + data.Jb + "");   //外面
            $(".yhld .iright em").html(data.Jb); //兑换页面
            tztzms = data.jizhis;
            kjms = data.seconds;
            iCount = setInterval(setsc, 1000);
            $('.kjq,.xyjg').css('display', 'block');
            $('.kjz').css('display', 'none');
            //兑换部分Init
            //TopInit();
        }
    });
}
function sleep(d) {
    for (var t = Date.now() ; Date.now() - t <= d;);
}
function playsund() {
    var ua = navigator.userAgent.toLowerCase();
    if (ua.match(/msie ([\d.]+)/)) {
        jQuery('#game_playsond_div').html('<object classid="clsid:22D6F312-B0F6-11D0-94AB-0080C74C7E95"><param name="AutoStart" value="1" /><param name="Src" value="/image/security.mp3" /></object>');
    }
    else if (ua.match(/firefox\/([\d.]+)/)) {
        jQuery('#game_playsond_div').html('<embed src="/image/security.mp3" type="audio/mp3" hidden="true" loop="false" mastersound></embed>');
    }
    else if (ua.match(/chrome\/([\d.]+)/)) {
        jQuery('#game_playsond_div').html('<audio src="/image/security.mp3" type="audio/mp3" autoplay="autoplay" hidden="true"></audio>');
    }
    else if (ua.match(/opera.([\d.]+)/)) {
        jQuery('#game_playsond_div').html('<embed src="/image/security.mp3" hidden="true" loop="false"><noembed><bgsounds src="/image/security.mp3"></noembed>');
    }
    else if (ua.match(/version\/([\d.]+).*safari/)) {
        jQuery('#game_playsond_div').html('<audio src="/image/security.mp3" type="audio/mp3" autoplay="autoplay" hidden="true"></audio>');
    }
    else {
        jQuery('#game_playsond_div').html('<embed src="/image/security.mp3" type="audio/mp3" hidden="true" loop="false" mastersound></embed>');
    }
}

function sx() {
    window.location = window.location;
}

$(function () {
    $('.close').on('click', function () {
        $(this).parents('.pop').css('display', "none")
    });

    //$('.btn_qd').on('click', function () {
    //    $('#msg').parents('.pop').css('display', "block");
    //})

    $('.gamecontableevent').on('mouseenter', 'tr', function () {
        $(this).addClass('bg')
    });

    $('.gamecontableevent').on('mouseleave', 'tr', function () {
        $(this).removeClass('bg')
    });
    //点击全部
    $('.popcontent .br button').on('click', function () {
        var type = $('.qd').attr("dhtype");
        if (type == 1) {
            $('#txt_money').val($('.yhld .iright em').text());
        } if (type == 2) {
            $('#txt_money').val($('.yhld .ilft em').text());
        }
    })
    //全局ajax不使用缓存
    $.ajaxSetup({ cache: false });
});
//初始化兑换方法
initduhuan = function () {
    getUjbmoney();
    $('.qd').unbind("click");
    $('.qd').click(function () {
        var type = $(this).attr("dhtype");
        subduihuan(type);
    });
}
//取得当前用户的钱
getUjbmoney = function () {
    $.getJSON("/Shared/Get_UserJbMoney", { time: new Date() }, function (ret) {
        if (ret.code == 1000) {
            $(".yhld .ilft em").html(ret.money);
            $(".jinri .jb").html("<img src=\"/img/gold.png\" />" + ret.jb);
            $(".yhld .iright em").html(ret.jb);
            $("#hqjy").html(ret.exp);
        }
    });
}
//提交兑换,脚本验证判断
subduihuan = function (type) {
    var money = $("#txt_money").val();
    var ld = $(".yhld .ilft em").text();    //乐豆
    var jb = $(".yhld .iright em").text();  //金币
    var yzm = $("#yzm").val();  //验证码
    
    var mx = 5000;
    if (money == "" || isNaN(money) || money < mx || money == 0) {
        ds.dialog.tips("您输入的金额不符合规则,最低兑换金额为5000！", 1, true, true);
        $("#txt_money").val(""); return;
    }
    if (yzm == "") {
        ds.dialog.tips("验证码不能为空！", 1, true, true);
        $("#yzm").val(""); return;
    }
    if (type == 1 && parseFloat(money) > parseFloat(jb)) { ds.dialog.tips("超出身上金币数量.", 1, true, true); $("#txt_money").val(""); return; }
    if (type == 2 && parseFloat(money) > parseFloat(ld)) { ds.dialog.tips("超出银行乐豆数量", 1, true, true); $("#txt_money").val(""); return; }
    $('.qd').hide();
    duihuanjb(type, money, yzm);
}
//ajax提交
duihuanjb = function (type, money, yzm) {
    $.ajax({
        type: "POST",
        url: "/Shared/DuiHuanMoney",
        data: { "type": type, "money": money, "yzm": yzm },
        //data: { "type": type, "money": money, "yzm": yzm, test: "1" },
        dataType: 'json',
        success: function (data) {
            try {
                if (data.code == "1000") {
                    ds.dialog.tips("兑换成功", 1, true, true);
                } else if (data.code == "-1001") {
                    $('.qd').show();
                    ds.dialog.tips(data.tips, 1, true, true);
                    return;
                }
                else {
                    ds.dialog.tips(data.tips, 1, true, true);
                }
                setTimeout('window.location.href = window.location.href', 2000);
            } catch (e) {
                ds.dialog.tips("兑换失败", 1, true, true);
                location.href = "/Shared/Err";
            }
        }
    });
}
//验证码URL
function chgUrl(url) {
    var timestamp = (new Date()).valueOf();
    if ((url.indexOf("?") >= 0)) {
        url = url + "&tamp=" + timestamp;
    } else {
        url = url + "?timestamp=" + timestamp;
    }
    return url;
}
//验证码img
function changeImg(id, url, type) {
    var imgSrc = $("#" + id);
    var src = url + "?type=" + type;
    imgSrc.attr("src", chgUrl(src));
}

//通知客户展示个人信息
senddialoguinfo = function (sid) {
    var url = "/Shared/SendShowUinfo?showuserid=" + sid;
    $.getJSON(url, function (ret) {
        if (ret.code == 1000) {
        } else {
        }
    })
}
