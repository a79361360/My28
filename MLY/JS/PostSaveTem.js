function postModel(data) {
    var name = data.newmodelname;
    var model_id = data.model_id;
    $.ajax({
        type: "POST",
        url: "/Tz/Set_Template_Save/",
        data: data,
        success: function (data) {
            if (data.msg == "1") {
                alert("保存成功!");
                window.location.href = window.location.href;
            }
            else {
                switch (data.msg) {
                    case -1:
                        alert("您输入的 \"" + name + "\" 模式名称已存在");
                        break;
                    case -2:
                        alert("请输入模式名称");
                        break;
                    case -3:
                        alert("投注总额必须为10-50000000金币");
                        break;
                    case -4:
                        alert("您的模式上限为20个，不可以再添加");
                        break;
                    case -5:
                        alert("当前模式正在运行中,模式保存失败");
                        break;
                    default:
                        $('.base_box_03').css('display', "none");
                        break;
                }
            }
        }
    });
}

function DeteTem(gameid)
{
    var model_id = $('#model_id').val();
    if (model_id != "") {
        if (confirm("是否确定删除")) {
            $.ajax({
                type: "POST",
                url: "/Tz/DeleteTem/",
                data: { id: model_id, gameid: gameid },
                success: function (data) {
                    if (data.msg == 1) {
                        window.location.href = window.location.href;
                    }
                    else if (data.msg == -1) {
                        alert("该模板在自动投注中,无法删除");
                    } else {
                        alert("未知错误.删除失败!");
                    }
                }
            });
        }
    } else {
        alert("未选中要删除的模式.");
    }
}

function config(gameid) {
    var TemName = $('#TemName').val();
    var number = "";
    var jb = "";
    num = parseInt($('#ztzspan').html());
    
    if (TemName == "新模板名称") {
        alert("请输入模式名称");
        return;
    }
   
    if (isNaN(num)) {
        num = 0;
    }
    if (num <= 0 || num > 50000000) {
        alert("保存失败，投注总额必须为10-50000000金币")
    } else if (TemName == "") {
        alert("请输入模式名称");
    } else if (TemName == "\\") {
        alert("无效的模式名称");
    } else{
        ////成功
        //$('.table-select').find('input').each(function () {
        //    var txt_value = $.trim($(this).val()).replace(/,/gi, "");
        //    str.push(txt_value);
        //});
        //var str_mdp_coin = str.join(",");
        for (var i = 0; i < 28; i++) {
            var txt_value = $("#n" + i + "").val().replace(/,/gi, "");
            if (txt_value != "") {
                number += i + ",";
                jb += txt_value + ",";
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

function SaveAutomatic(gameid) {
    var obj = document.getElementById("Open") //定位id
    if (obj == null)
    {
        alert("请先添加投注模板！");
        return;
    }
    var value = obj.options[obj.selectedIndex].value; // 选中值

    var TempWin = document.getElementById("TempWin" + value + "");
    var Winvalue = TempWin.options[TempWin.selectedIndex].value;

    var TempLose = document.getElementById("TempLose" + value + "");
    var Losevalue = TempLose.options[TempLose.selectedIndex].value;

    //var str = $("#Open option").map(function () { return $(this).val(); }).get().join(",")
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
    if (MaxJb == 0)
    {
        MaxJb = 99999999999;
    }
    if (value == "") {
        alert("请选择投注模式！");
    } else if (parseInt(MinJb) >= parseInt(MaxJb)) {
        alert("最大金币应该大于最小金币！");
        return false;
    }
    else if (parseInt(end) < parseInt(start)) {
        alert("结束期必须大于开始期！");
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
                        alert("自动投注成功！");
                        window.location.href = window.location.href;
                    }
                    else if (data.msg == -1) {
                        alert("开始期必须大于当前期！");
                    }
                    else if (data.msg == -2) {
                        alert("结束期必须大于开始期！");
                    }
                    else if (data.msg == -3) {
                        alert("最大金币应大于最小金币！");
                    }
                    else if (data.msg == -4) {
                        ds.dialog.alert('用户状态错误,请重新登入后再操作！');
                    }
                    else if (data.msg == -5) {
                        ds.dialog.alert('自动投注已开启,请勿重新开启！');
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

function Stop_Auto(AId,gameid) {
    $.ajax({
        type: "POST",
        url: "/Tz/Stop_Automatic",
        data: { "AId": AId, "gameid": gameid },
        dataType: 'json',
        success: function (data) {
            try {
                if (data.msg == 1) {
                    alert("停止投注成功！");
                    window.location.href = window.location.href;
                }
                else {
                    alert("停止失败！");
                }
            } catch (e) {
                location.href = "/admin/error";
            }
        }
    });
}