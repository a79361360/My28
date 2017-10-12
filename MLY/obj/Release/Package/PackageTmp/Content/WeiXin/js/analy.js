$(function () {
    analywxlogin();
})
//到达微信登入页面的数量
analywxlogin = function () {
    $.get("/ComExpend/AnalyWxUser", { type: 1 }, function () {

    })
}