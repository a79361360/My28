$(function () {
    $('.menu span').on('tap', function () {
        $('.popmenu').toggleClass('active');
    })
    $('.popmenu').on('tap', 'a', function () {
        //$('.popmenu').toggleClass('active');
    })
});

var PageNumber = 1;
var swiper = new Swiper('.swiper-container', {
    scrollbar: '.swiper-scrollbar',
    direction: 'vertical',
    slidesPerView: 'auto',
    mousewheelControl: true,
    autoHeight: true,
    freeMode: true,
    onTouchEnd: function (swiper, event) {
        var pulldistance = swiper.getWrapperTranslate();
        var height = swiper.height;
        if (pulldistance < 40 && pulldistance > -40) {
            $('.pull').html('下拉刷新');
            $('.push').html('加载中');
        }
        if (pulldistance > 40) {
            //刷新位置
            //$('.pull').html('正在刷新');
            //$.ajax({
            //    type: "get",
            //    datatype: 'json',
            //    async: false, //同步
            //    url: "/Luck28Mobile/ajaxIndex?g28=" + g28,
            //    success: function (data) {
            //        $(".swiper-slide").html(data);
            //        swiper.update();
            //    }
            //});
        }

        if (pulldistance + $('.swiper-slide').height() - $('.table').height() < -40) {
            //加载位置
            $.ajax({
                type: "get",
                datatype: 'json',
                async: false, //同步
                url: "/Home/GetJs28PageLast",
                error: function () {
                },
                success: function (data) {
                    if (data == '') {
                        window.location.href = "/";
                    } else {
                        eval('data=' + 'data');
                        $(".btop").append(data.data);
                        swiper.update();
                    }
                }
            });
        }
    },
    onProgress: function (swiper, progress) {
        var pulldistance = swiper.getWrapperTranslate();
        var height = $('.swiper-slide').height() - $('.table').height();
        if (pulldistance > 40) {
            $('.pull').html('松开刷新');
        }
        if (pulldistance < -40) {
            $('.push').html('正在加载中。。。');
        }
        if (pulldistance < 40 && pulldistance + height > -40) {
            $('.pull').html('下拉刷新');
            $('.push').html('加载中');
        }


    }
});

var lottery_start_time;//倒计时
var lottery_start_issue;
var lottery_last_issue;
var lotteryNum;
var parseparam;
parseparam = issue_info();
lottery_start_time = parseparam.countDown;
lottery_start_issue = parseparam.lotteryIssue;
lottery_last_issue = parseparam.last;
lotteryNum = parseparam.lotteryNum;

function issue_info() {
    var flag;
    $.ajax({
        type: "get",
        datatype: 'json',
        async: false, //同步
        url: "/Home/Js28Top1",
        error: function () {
        },
        success: function (data) {
            if (data == '') {
                window.location.href = "/";
            } else {
                eval('data=' + 'data');
                var data = data
                parseparam = data;
                flag = data;
            }
        }
    });
    return flag;
}

//倒计时
var stop_auto_reload;
var stop_auto_reload_up; //截

//开奖--------------------------------
function get_count_down() {
    if (lottery_start_time > 0) {
        lottery_start_time = lottery_start_time - 1;  //
        var help_note = "";
        help_note = "第<em>" + lottery_start_issue + "</em>期，离开奖时间还有<em>" + lottery_start_time + "</em>秒<br>";
        if (lottery_last_issue) {
            help_note += "第<em>" + lottery_last_issue + "</em>期，开奖结果：<em>" + lotteryNum + "</em>";
        }
        $("#left_title").html(help_note);
        stop_auto_reload = setTimeout(function () {
            get_count_down();
        }, 1000);
        clearTimeout(stop_auto_reload_up);
    } else {
        $("#left_title").html("第<em>" + lottery_start_issue + "</em>期正在开奖中！");
        clearTimeout(stop_auto_reload);
        stop_auto_reload_up = setTimeout(function () {
            parseparam = issue_info();
            lottery_start_time = parseparam.countDown;
            lottery_start_issue = parseparam.lotteryIssue;
            lottery_last_issue = parseparam.last;
            lotteryNum = parseparam.lotteryNum;
            window.location.reload();
            get_count_down();
        }, 1000);
    }
}
$(document).ready(function () {
    get_count_down();
});
function sleep(d) {
    for (var t = Date.now() ; Date.now() - t <= d;);
}




