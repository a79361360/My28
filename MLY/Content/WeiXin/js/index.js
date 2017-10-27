var scount;
var tztzms = 0; //截止投注秒数在top模板里ajax付值
var kjms = 0;   //截止开奖秒数在top模板里ajax付值
var kjms1 = 0;       //未开奖1
var kjms2 = 0;      //未开奖2
var kjms3 = 0;      //未开奖3
var iCount;     //setInterval附值变量,在top模板里ajax付值
var iCount1;
var gameid;    //游戏ID
var idex;
var jiezhis;
var tzkjs = 0;
var iskj;
var slee = 0;
var stopTz = 0; //停止投注
function setsc() {
    if (tztzms > 0) {
        $("#tztzms").html(tztzms);  //投注截止秒数
        $("#msspan").html(kjms);//开奖截止秒数
        $(".time").eq(2).html(kjms1 + "s");
        $(".time").eq(1).html(kjms2 + "s");
        $(".time").eq(0).html(kjms3 + "s");
    } else {
        if (kjms > 0) {
            $("#tztzms").html(0);   //投注截止秒数
            $("#msspan").html(kjms);    //开奖截止秒数
            $(".time").eq(2).html(kjms1 + "s");
            $(".time").eq(1).html(kjms2 + "s");
            $(".time").eq(0).html(kjms3 + "s");

                //$("#lasttd" + dqqs).html("<span class=\"yitouz\">投注结束</span>");
                stopTz = 1;
        }else {
            if (iskj != 1) {
                tztzms = 3;
                slee++;
                $('.tab p').css('display', 'none');
                $('.kjz').css('display', 'block');
                if (slee == 3) {
                    slee = 0;
                    if (gameid == 1 || gameid == 2) {
                        $.ajax({
                            type: "POST",
                            url:"/WxJs28/Get90sx",
                            data: { "gameid": gameid },
                            dataType: 'json',
                            success: function (data) {
                                var gamename = "PC28";
                                if (gameid == 1) gamename = "极速28";
                                var preissue = parseInt(data.Iss) - 1;
                                var str = "<p>" + gamename + "第<span id=\"syq\">" + data.Iss + "</span>期，离开奖时间还有<span id=\"msspan\">" + data.seconds + "</span>秒</p>";
                                str += "<p>第<span>" + preissue + "</span>期，开奖结果：<span id=\"lotresult\">" + data.Issue + "=</span><span id=\"resultNums\" class=\"label label-danger labels ml\">" + data.number + "</span></p>";
                                str += "<p>我的金币：<span class=\"jb\">" +  parseFloat(data.Jb).toLocaleString() + "</span></p>";
                                str += "<span style=\"display:none;\" class=\"kjz black\">第<em class=\"positive large bianchu\">" + data.Iss + "</em>期正在开奖中</span>";
                                $(".tab").html(str);
                                kjms = data.seconds+1;
                                if (kjms > 0) {
                                    kjms = kjms - 1;
                                    if (kjms == 0) {
                                        $('.kjz').css('display', 'block');
                                    }

                                };
                            }
                        });
                    }
                }
            }
        }
    }
    
    kjms = kjms - 1;
    tztzms = tztzms - 1;
    if (kjms1 > 0) {
        kjms1 = kjms1 - 1;
        if (kjms1 == 0) {
            //$(".time").eq(2).html(kjms1 + "s");
            idex = 0;
            $("#refresh" + idex).html("<span class=\"label label-warning btn-index\"><a href=\"\" class=\"white\" onclick=\"window.location.href = window.location.href\">刷新</a></span>");
        }
    }

    if (kjms2 > 0) {
        kjms2 = kjms2 - 1;
        if (kjms2 == 0) {
            //$(".time").eq(1).html(kjms2 + "s");
            idex = 1;
            $("#refresh" + idex).html("<span class=\"label label-warning btn-index\"><a href=\"\" class=\"white\" onclick=\"window.location.href = window.location.href\">刷新</a></span>");
        }
    }
    if (kjms3 > 0) {
        kjms3 = kjms3 - 1;
        if (kjms3 == 0) {
           // $(".time").eq(0).html(kjms3 + "s");
            idex = 2;
            $("#refresh" + idex).html("<span class=\"label label-warning btn-index\"><a href=\"\" class=\"white\" onclick=\"window.location.href = window.location.href\">刷新</a></span>");
        }
    }

}

function tzds() {
    if (jiezhis >= 0) {
        $(".tz-s").html(jiezhis);
        $(".tzkj").html(tzkjs);
       
    } else {
        if (tzkjs >= 0) {
            $(".tzkj").html(tzkjs);
            if (tzkjs == 0) {
                $('.tz-show').css('display', 'none');
                $('.hold').css('display', 'block');
            }
        }else{
            if (iskj != 1) {
                    $('.hold').css('display', 'none');
                    $('.kj').css('display', 'block');
                    iskj = 0;
                    slee++;
                    if (slee == 2) {
                        slee = 0;
                        if (gameid == 1||gameid == 2) {
                            $.ajax({
                                type: "POST",
                                url: "/WxJs28/Get90sx",
                                data: { "gameid": gameid },
                                dataType: 'json',
                                success: function (data) {
                                    if (data.jizhis < 0) { data.jizhis = 0; }
                                    var str = "<p class=\"wl throw-head tz-show\">第<span class=\"positive tz-qs\">" + data.Iss + "</span>期，离投注截止时间还有<span class=\"positive tz-s\">" + data.jizhis + "</span>秒<br>离开奖时间还有<span class=\"positive tzkj\">60</span>秒</p>";
                                    str += "<p class=\"wl throw-head hold\" style=\"display:none;\">第<span class=\"positive tz-qs\">" + data.Iss + "</span>期，截止投注等待开奖</p>";
                                    str += "<p class=\"wl throw-head kj\" style=\"display:none;\">第<span class=\"positive tz-qs\">" + data.Iss + "</span>期，正在开奖</p>";
                                    
                                    $(".rank-qq").html(str);
                                    tzkjs = data.seconds;
                                    jiezhis = data.jizhis;
                                 
                                    if (jiezhis > 0) {
                                        $(".tz-s").html(jiezhis);
                                        $(".tzkj").html(tzkjs);
                                     
                                    } else {
                                        if (tzkjs > 0) {
                                                $(".tzkj").html(tzkjs);
                                         
                                        } else {
                                            $('.hold').css('display', 'none');
                                            $('.kj').css('display', 'block');
                                        }
                                        jiezhis = jiezhis - 1;
                                        tzkjs = tzkjs - 1;
                                    }
                                }
                            });
                        }
                 }
             }
        }
    }
    jiezhis = jiezhis - 1;
    tzkjs = tzkjs - 1;
}


function sleep(d) {
    for (var t = Date.now() ; Date.now() - t <= d;);
}


//自动投注
function Stop_Auto(AId, gameid, typeid) {
    $.ajax({
        type: "POST",
        url: "/WxJs28/Stop_Automatic",
        data: { "AId": AId, "gameid": gameid },
        dataType: 'json',
        success: function (data) {
            try {
                if (typeid == 1 && data.msg == 1) {
                    $('.tab-auto').css('display','none');
                }else{
                   if(typeid == 2&&data.msg == 1){
                      window.location.href = window.location.href;
                   }
                   else {
                       alert('停止失败！');
                    }
                }
            } catch (e) {
              
               location.href = "/admin/error";
            }
        }
    });
}

//自动投注
function SaveAutomatic(gameid) {
    var obj = document.getElementById("Open") //定位id
    if (obj == null) {
        ds.dialog.alert('请先添加投注模板！');
        return;
    }
    var value = obj.options[obj.selectedIndex].value; // 选中值
    var TempWin = document.getElementById("TempWin" + value + "");
    var Winvalue = TempWin.options[TempWin.selectedIndex].value;
    var TempLose = document.getElementById("TempLose" + value + "");
    var Losevalue = TempLose.options[TempLose.selectedIndex].value;
    var model = $("#Open option").map(function () { return $(this).val(); }).get();
    var strmodel = model.join(",");
    var winlist = new Array();
    var losslist = new Array();
    for (var i = 0; i < model.length; i++) {
        winlist.push($("#TempWin" + model[i]).val());
        losslist.push($("#TempLose" + model[i]).val());
    }
    var start = $("#start").val();
    var end = $("#end").val();
    var MinJb = $("#MinJb").val();
    var MaxJb = $("#MaxJb").val();
    if (MaxJb == 0) {
        MaxJb = 99999999999;
    }
    if (value == "") {
        ds.dialog.alert('请选择投注模式！');
    } else if (parseInt(MinJb) >= parseInt(MaxJb)) {
        ds.dialog.alert('最大金币应该大于最小金币！');
        return false;
    }
    else if (parseInt(end) < parseInt(start)) {
        ds.dialog.alert('结束期必须大于开始期！');
    }
    else {
        $.ajax({
            type: "POST",
            url: "/Tz/Set_Automatic",
            data: { "ATempId": value, "start": start, "end": end, "MinJb": MinJb, "MaxJb": MaxJb, "Winvalue": Winvalue, "Losevalue": Losevalue, "gameid": gameid, "model": strmodel, "winlist": winlist.join(","), "losslist": losslist.join(",") },
            dataType: 'json',
            success: function (data) {
                try {
                    if (data.msg == 1) {
                        ds.dialog.alert('自动投注成功!', function () { window.location.href = window.location.href;});
                       
                    }
                    else if (data.msg == -1) {
                        ds.dialog.alert('开始期必须大于当前期！');
                    }
                    else if (data.msg == -2) { 
                        ds.dialog.alert('结束期必须大于开始期！');
                    }
                    else if (data.msg == -3) {
                        ds.dialog.alert('最大金币应大于最小金币！');
                    }
                    else if (data.msg == -4) {
                        ds.dialog.alert('用户状态错误,请重新登入后再操作！');
                    }
                    else if (data.msg == -5) {
                        ds.dialog.alert('自动投注已开启,请勿重新开启！');
                    }
                    else {
                        ds.dialog.alert('投注失败！');
                    }
                } catch (e) {
                    location.href = "/admin/error";
                }
            }
        });
    }
}



//提交兑换,脚本验证判断
 function subduihuan () {
    var money = $("#txt_money").val();
    var ldinput = $("#txt_ld").val();    //乐豆
    var pwd = $("#yzm").val();  //密码
    var mx = 5000;
    if (money != "" && ldinput != "") {
        ds.dialog.alert('金币和乐豆不可同时兑换！！');
        $("#xxxs").html("");
   
    } else {
        if (money == "" && ldinput == "") {
            ds.dialog.alert('金币和乐豆至少兑换一种！！！');
            $("#xxxs").html("");
        } else {

            if (pwd == "") {
                ds.dialog.alert('密码不能为空！');
                $("#yzm").val(""); return;
            }
            if (money != "") {
                type = 1;
                if (isNaN(money) || money < mx || money == 0) {
                    ds.dialog.alert('您输入的金额不符合规则,最低兑换金额为5000！');
                    $("#xxxs").html("");
                    $("#txt_money").val(""); return;
                }
                if (parseFloat(money) > parseFloat(jb)) {
                    ds.dialog.alert('超出身上金币数量');
                    $("#xxxs").html("");
                    $("#txt_money").val(""); return;
                }
                duihuanjb(type, money, pwd);
            }

            if (ldinput != "") {
                type = 2;
                if (isNaN(ldinput) || ldinput < mx || ldinput == 0) {
                    ds.dialog.alert('您输入的金额不符合规则,最低兑换金额为5000！');
                    $("#xxxs").html("");
                    $("#txt_ld").val(""); return;
                }
                if (parseFloat(ldinput) > parseFloat(ld)) {
                    ds.dialog.alert('超出银行乐豆数量');
                    $("#xxxs").html("");
                    $("#txt_ld").val(""); return;
                }
                duihuanjb(type, ldinput, pwd);
            }

        }
     }
}


//兑换
function duihuanjb(type, money, pwd) {
    $.ajax({
        type: "POST",
        url: "/WxJs28/MoneyExchange",
        data: { "type": type, "money": money, "pwd": pwd },
        dataType: 'json',
        success: function (data) {
            try {
                if (data.code == "1000") {
                    ds.dialog.alert("兑换成功");
                } else if (data.code == "-1001") {
                   
                    ds.dialog.alert(data.tips, 1, true, true);
                    return;
                }
                else {
                    ds.dialog.alert(data.tips, 1, true, true);
                }
                setTimeout('window.location.href = window.location.href', 2000);
            } catch (e) {
                ds.dialog.alert('兑换失败');
                location.href = "/Shared/Err";
            }
        }
    });
}
//注册页面
function check_register() {
        var telphone = $("#phonenum").val();
        if (telphone == "") {
            ds.dialog.alert('手机号码不能为空');
        }
        else {
            if (telRuleCheck2(telphone)) {
            }
            else {
                ds.dialog.alert('手机号码格式不正确');
            };
        };
        var telphone = $("#phonenum").val();
        var mima1 = $("#mima1").val();
        var mima2 = $("#mima2").val();
        var yzm = $("#inputyzm").val();
        if (telphone == "") {
            ds.dialog.alert('手机号码不能为空');

        }
        else {
            if (telRuleCheck2(telphone)) {
                if (mima1.length < 6) {
                    ds.dialog.alert('密码至少为6位');
                } else {
                    if (mima1 != mima2) {
                        ds.dialog.alert('两次输入的密码不一致');
                    } else {
                        if (yzm == "") {
                            ds.dialog.alert('请输入验证码');
                        }
                        else { reguser(telphone, mima1, mima2, yzm); }
                    }
                }
            }
            else {
                ds.dialog.alert('手机号码格式不正确');
            };
        };
}

//手机验证码
function get_yzm() {
    var phone = $("#phonenum").val();
    if (telRuleCheck2(phone)) {
            if (telRuleCheck2(phone)) {
                $.ajax({
                    type: "POST",
                    url: "/WxJs28/SendPhoneMsg",
                    dataType: 'json',
                    data: { phone: phone },
                    success: function (data) {
                        if (data.code == "-1001")
                        { ds.dialog.alert('该手机号码已被注册'); }
                        else if (data.code == "1000") {
                            $("#phonenum").attr("readonly", "readonly");
                            settime();
                        }
                    }
                });
            }
    } else { ds.dialog.alert('手机号码格式不正确'); }
 
}

//注册并登入
reguser = function (phone, m1, m2, yzm) {
    $.ajax({
        type: "POST",
        url: "/WxJs28/RegisterWxUser",
        dataType: 'json',
        data: { phone: phone, pwd: m1, pwdd: m2, yzm: yzm },
        success: function (data) {
            if (data.code == 1000) {
                window.location.href = "/WxJs28/Index";
            } else {
                ds.dialog.alert("注册失败." + data.tips);
            }
        }
    });
}

//验证码发送倒计时
var countdown = 120;
function settime() {
    if (countdown == 0) {
        $(".getnum").attr("disabled", false);
        $(".getnum").val("获取验证码");
        countdown = 120;
        return;
    } else {
        $(".getnum").attr("disabled", true);
        $(".getnum").val("重新发送(" + countdown + ")");
        countdown--;
    }
    setTimeout(function () {
        settime()
    }, 1000)
}

//手机号码格式验证   
telRuleCheck2 = function (string) {
    var pattern = /^1[34578]\d{9}$/;
    if (pattern.test(string)) {
        return true;
    }
    return false;
};
//验证登录
function check_login() {
    var username = $("#username").val();
    var userpwd = $("#inputPassword").val();
    var yzm = $("#yzm").val();
    if (username == "") {
        ds.dialog.alert('请输入账号!');
    } else {
        if (userpwd == "")
            ds.dialog.alert('请输入密码!');
        else {
            if (yzm == "") {
                ds.dialog.alert('请输入验证码!');
            }
            else {
                $("#telphone_tip").text("");
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/Login/WxLogin",
                    dataType: 'json',
                    data: { UserName: username, UserPwd: userpwd, yzm: yzm },
                    success: function (data) {
                        if (data.code == 1000) {
                            window.location.href = "/WxJs28/Index";
                        } else {
                            ds.dialog.alert(data.tips);
                            $("#btn_login").text("登录");
                        }
                    }
                });

            }
        }
    }
}
//底部
function numSep(num) {
    return num.toString().replace(/(\d{1,3})(?=(\d{3})+(?:$|\.))/g, "$1,");
}

$(function () {

    $('.footmenu .item1 > span').on('tap', function (e) {
        $('.footmenu .item-pop').removeClass('active');
        $(this).parent().find('.item-pop').toggleClass('active');
    });

    $('.footmenu').on('tap', '.item1', function (e) {
        e.stopImmediatePropagation();
        return false;
    });
    $('.clickimg').on('touchend', function (e) {
        e.preventDefault()
        $('.more-pop').toggleClass('active')
    })

    $('.closemode').on('touchend', function (e) {
        e.preventDefault()
        $('.more-pop').removeClass('active');
        $('.self-set-pop').removeClass('active');
    })

    $('.more-pop .self-mode').on('touchend', function (e) {
        $('.self-set-pop').toggleClass('active');
    })

    $('.standard-mode').on('touchend', function (e) {
        e.preventDefault()
        $('.more-pop').addClass('active');
        $('.self-set-pop').removeClass('active');
    })

});