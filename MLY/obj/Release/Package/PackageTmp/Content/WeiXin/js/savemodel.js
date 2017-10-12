//微信JS28确定投注
function tzsert(gameid) {
    var ztz = 0;
    ztz = $("#total_md_lottery").html();
    if (ztz == 0||ztz=="") {
        ds.dialog.alert("对不起，请先投注")
        return;
    }
    $.ajaxSetup({
        async: false
    });
    var ztz1 = ztz.replaceAll(",", "");
    var ztz2 = parseInt(ztz1);
    var tzjb = "";
    var tznb = "";
    var countjb = $("#jbzs").text();
    var countjb1 = countjb.replaceAll(",", "");
    var countjb2 = parseInt(countjb1);
    for (var i = 0; i <= 27; i++) {
        var jb2 = $("#txt" + i).val();
        var jb1 = jb2.replaceAll(",", "");
        if (jb1.length != 0) {
            tzjb += parseInt(jb1) + ",";
            tznb += i + ",";
        }
    }
    var str = [];
    if (tzjb == "") {
        ds.dialog.alert("对不起，请先投注！");
        return false;
    } else if (ztz2 > countjb2) {
        ds.dialog.alert("对不起，金币不足！");
        return false;
    }
    else if (ztz2 < 10) {
        ds.dialog.alert("最小投注金额10金币！");
        return false;
    } else {
        var Issue = $("#qishu").text();
        var dqqs = $("#dqIssue").val();
        if (Issue == dqqs) { }
        var url = "/WxJs28/Index";
        if (gameid == 2) url = "/WxBj28/Index";
        if (ds.dialog.confirm("确定投注金币：" + ztz, function () {
            $.ajax({
                type: "POST",
                url: "/Tz/SetBetting",
                data: { "tzjb": tzjb, "tznb": tznb, "Issue": dqqs, "countjb": ztz2, "gameid": gameid, "wx": 1 },
                dataType: 'json',
                success: function (data) {
                    try {
                        if (data.msg == "1") {
                            window.location.href = url;
                        }
                       else if (data.msg == "2") {
          
                            ds.dialog.alert("本期已经结束！");
                            window.location.href = url;
                        }
                        else if (data.msg == "3") {
                            ds.dialog.alert("投注金额过大或者过小！");
                        }
                        else if (data.msg == "4") {
                            ds.dialog.alert("已经停止投注！");
                        } else if (data.msg == "-2") {
                            ds.dialog.alert("该期总投注金额大于5千W！");
                            window.location.href = url;
                        }
                        else {
                            ds.dialog.alert("投注失败！");
                        }
                    } catch (e) {
                        location.href = "/admin/error";
                    }
                }
            });
        }, function () { this.close(); }));
    };
}
///---------去掉字符串中的逗号-----------------//
String.prototype.replaceAll = function (AFindText, ARepText) {
    raRegExp = new RegExp(AFindText, "g");
    return this.replace(raRegExp, ARepText);
}

//-------------------确定添加投注模式---------------------//
function config1(gameid) {

    var msmc = $('#msmc').val();
    if (msmc == 0) {
        ds.dialog.alert("请输入模式名称");
    }
    else {
        var TemName = msmc;
        var number = "";
        var jb = "";
        var ztz = $("#tzze").val();
        var ztz1 = ztz.replaceAll(",", "");
        var num = parseInt(ztz1);
        if (isNaN(num)) {
            num = 0;
        }
        if (num == "" || num <= 0 || num > 50000000) {
            ds.dialog.alert("保存失败，投注总额必须为10-50000000金币")
        } else if (TemName == "") {
            ds.dialog.alert("请输入模式名称");
        } else if (TemName == "\\") {
            ds.dialog.alert("无效的模式名称");
        } else {
            for (var i = 0; i < 28; i++) {
                var txt_value = $("#txt" + i + "").val().replace(/,/gi, "");
                if (txt_value != "") {
                    number += i + ",";
                    jb += parseInt(txt_value) + ",";
                }
            }
            var model_id = $('#model_id').val();
            number = number.substring(0, number.length - 1);
            jb = jb.substring(0, jb.length - 1);
            //提交的数据
            data = { num: num, newmodelname: TemName, number: number, jb: jb, model_id: model_id, gameid: gameid };
            postModel(data);
        }
    }
}

//----------------投注模式保存判断-----------//
function postModel(data) {

    var name = data.newmodelname;
    var model_id = data.model_id;
    var gameid = data.gameid;
    $.ajax({
        type: "POST",
        url: "/Tz/Set_Template_Save/",
        data: data,
        success: function (data) {
            if (data.msg == "1") {
                ds.dialog.alert("保存成功!", function () {
                    if (gameid == 1)
                    { window.location.href = "/WxJs28/selflist"; }
                    else if (gameid == 2)
                    { window.location.href = "/WxBj28/selflist"; }
                });
            }
            else {
                switch (data.msg) {
                    case -1:
                        ds.dialog.alert("您输入的 \"" + name + "\" 模式名称已存在");
                        break;
                    case -2:
                        ds.dialog.alert("请输入模式名称");
                        break;
                    case -3:
                        ds.dialog.alert("投注总额必须为10-50000000金币");
                        break;
                    case -4:
                        ds.dialog.alert("您的模式上限为20个，不可以再添加");
                        break;
                    case -5:
                        ds.dialog.alert("当前模式正在运行中,模式保存失败");
                        break;
                    default:
                        $('.base_box_03').css('display', "none");
                        break;
                }
            }
        }
    });
}
//删除模式
function Delmodel(gameid) {
    var model_id = $('#model_id').val();
    if (model_id != "") {
        ds.dialog.confirm("是否确定删除", function () {
            $.ajax({
                type: "POST",
                url: "/Tz/DeleteTem/",
                data: { id: model_id, gameid: gameid },
                success: function (data) {
                    if (data.msg == 1) {
                        if (gameid == 1)
                        { window.location.href = "/WxJs28/selflist"; }
                        else if (gameid == 2)
                        { window.location.href = "/WxBj28/selflist"; }
                    }
                    else if (data.msg == -1) {
                        ds.dialog.alert("该模板在自动投注中,无法删除");
                    } else {
                        ds.dialog.alert("未知错误.删除失败!");
                    }
                }
            });
        }, function () { });


    } else {
        ds.dialog.alert("未选中要删除的模式.");
    }
}
