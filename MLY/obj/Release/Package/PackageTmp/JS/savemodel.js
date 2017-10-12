/**
 * Created by Administrator on 2016/4/11 0011.
 */
function modeSave() {
    var radio_id = $(".model-pop").find("input[name=radio]:checked").val();
    if (radio_id == 0) {
        var name = $("#modes_name").val();
        if (name != "") {
            //改变颜色
            $("#my_modes_name").find("a[class='a2']").each(function () {
                $(this).removeClass("a2").addClass("a3");
            }
            );
            $("#b_save").val("提交中...");
            $('#b_save').removeAttr('onclick');
            $('#b_save').unbind('click');
            var i = 0;
            var arr = new Array();
            $("input[name='SMONEY']").each(function () {
                var num = this.value.replace(/\D/g, '');
                if (num == "" || num == null) {
                    arr[i] = 0;
                } else {
                    arr[i] = parseInt(num);
                }
                i++;
            });
            $.ajax({
                type: "POST",
                url: "model.php",
                data: "do=savemodes&g28=" + g28 + "&name=" + name + "&arr=" + arr + "&tmp=" + Math.random(),
                success: function (msg) {
                    $("#b_save").val("确认");
                    $('#b_save').removeAttr('onclick');
                    $('#b_save').unbind('click');
                    $('#b_save').bind('click', function () {
                        modeSave();
                    });
                    switch (msg) {
                        case "0":
                            back_msg("您输入的 \"" + name + "\" 模式名称已存在", 0);
                            break;
                        case "-1":
                            back_msg("请输入模式名称", 0);
                            break;
                        case "-2":
                            back_msg("投注总额必须为1-" + numSep(max_total) + "魔豆", 1);
                            break;
                        case "-3":
                            back_msg("请先登录，再进行操作", 1);
                            break;
                        case "-4":
                            back_msg("您的模式上限为20个，不可以再添加", 1);
                            break;
                        default:
                            var id = parseInt(msg);
                            $("#modes_id").val(id);
                            $("<a id=\"sp_modes_" + id + "\" style=\"cursor:pointer\" class=\"a2\" onclick=\"modeLoad(this," + id + ")\">" + $("#modes_name").val() + "</a>").insertAfter($("#add_mode"));
                            $("#drop_modes").append("<option value='" + id + "'>" + name + "</option>");
                            $("#modes_name_1").html(name);
                            if (name.length > 10) {
                                $("#modes_name_2").html(name.substr(0, 10) + "...");
                            } else {
                                $("#modes_name_2").html(name);
                            }
                            //$("#btn_saveas").show();
                            $("#btn_del").show();
                            $('#b_save').removeAttr('onclick');
                            $('#b_save').unbind('click');
                            $('#b_save').bind('click', function () {
                                modeUp();
                            });
                            back_msg("\"" + name + "\" 模式添加成功", 1);
                            break;
                    }
                }
            });
        } else {
            alert("请输入模式名称", 0);
        }
    } else {
        $("#modes_id").val($("#drop_modes").val());
        $("#modes_name").val($("#drop_modes").find("option:selected").text());
        modeUp();
    }

}

function numClearSep(num) {
    return parseInt(num.toString().replace(/,/g, ""));
}


$(function () {
    var number = "";
    var jb = "";
    var num = 0;
    var newmodelname = '';
    var drop_modes = "";
    var data = "";

    function config() {
        num = parseInt(numClearSep($('.orange').val()));
        if (isNaN(num))
        {
            num = 0;
        }
        if (num <= 0 || num > maxnum) {
            showMsg('1', "保存失败，投注总额必须为10-" + numClearSep(maxnum) + "金币")
        } else if (newmodelname == "") {
            showMsg('1', "请输入模式名称");
        } else {
            ////成功
            //$('.table-select').find('input').each(function () {
            //    var txt_value = $.trim($(this).val()).replace(/,/gi, "");
            //    str.push(txt_value);
            //});
            //var str_mdp_coin = str.join(",");
            for (var i = 0; i < 28; i++) {
                var txt_value = $("#" + i + "").val().replace(/,/gi, "");
                if (txt_value != "") {
                    number += i + ",";
                    jb += txt_value + ",";
                }
            }
            var model_id = $('#model_id').val();
            number = number.substring(0, number.length - 1);
            jb = jb.substring(0, jb.length - 1);
            //提交的数据
            data = { num: num, newmodelname: newmodelname, number: number, jb: jb, model_id: model_id };
            postModel(data);
        }
    }
    // 保存
    $('.savemode').tap(function () {
        newmodelname = $.trim($('#newmodelname').val());
        num = parseInt(numClearSep($('.orange').val()));
        if (num <= 0 || num > maxnum) {
            showMsg('1', "保存失败，投注总额必须为1-" + numClearSep(maxnum) + "金币")
        } else if ($.inArray(newmodelname, hasyed) >= 0) {
            showMsg('1', "模式已存在");
        } else {
            config();
        }
    });
    // 另存为
    $('.model-pop .sure').tap(function () {
        var radio_id = $(".model-pop").find("input[name=radio]:checked").val();
        if (radio_id == "add") {
            newmodelname = $.trim($('#modes_name').val());
            if ($.inArray(newmodelname, hasyed) >= 0) {
                showMsg('1', "模式已存在");
            } else {
                $('#model_id').val("");
                config();
            }

        } else {
            newmodelname = $.trim($('#newmodelname').val());
            num = parseInt(numClearSep($('.orange').val()));
            if (isNaN(num)) {
                num = 0;
            }
            if (num <= 0 || num > maxnum) {
                showMsg('1', "保存失败，投注总额必须为1-" + numClearSep(maxnum) + "金币")
            } else if (newmodelname == "") {
                showMsg('1', "请输入模式名称");
            } else {
                //成功
                for (var i = 0; i < 28; i++) {
                    var txt_value = $("#" + i + "").val().replace(/,/gi, "");
                    if (txt_value != "") {
                        number += i + ",";
                        jb += txt_value + ",";
                    }
                }
                var obj = document.getElementById("drop_modes") //定位id
                var value = obj.options[obj.selectedIndex].value; // 选中值
                var model_id = value;
                number = number.substring(0, number.length - 1);
                jb = jb.substring(0, jb.length - 1);
                //提交的数据
                data = { num: num, newmodelname: newmodelname, number: number, jb: jb, model_id: model_id };

                postModel(data);
            }
        }

    });


    //提交的的位置
    function postModel(data) {
        var name = data.newmodelname;
        var model_id = data.model_id;

        $.ajax({
            type: "POST",
            url: "/Tz/Set_Template_Save/",
            data: data,
            success: function (data) {
                console.log(data.msg)
                if (data.msg =="1") {
                    showMsg('4', "\"" + name + "\" 模式保存成功");
                }
                else {
                    switch (data.msg) {
                        case -1:
                            showMsg('1', "您输入的 \"" + name + "\" 模式名称已存在");
                            break;
                        case -2:
                            showMsg('1', "请输入模式名称");
                            break;
                        case -3:
                            showMsg('1', "投注总额必须为10-" + numSep(max_total) + "金币");
                            break;
                        case -4:
                            showMsg('1', "您的模式上限为20个，不可以再添加");
                            break;
                        case -5:
                            showMsg('1', "当前模式正在使用中,模式保存失败");
                            break;
                        default:
                            break;
                    }
                }
            }
        });
    }

});