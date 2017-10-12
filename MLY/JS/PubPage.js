function createPage(_pageNum, _pageUrl, _pageSum, _page) {
    var pageStr = "";
    var myPage = new Object;
    myPage.pageCount = (_pageSum % _pageNum) == 0 ? _pageSum / _pageNum : parseInt(_pageSum / _pageNum) + 1;          //总页数
    myPage.page = _page;                //当前页
    myPage.pageNum = _pageNum;             //每页显示的条数
    myPage.pageSum = _pageSum;             //全部条数
    myPage.pageUrl = _pageUrl;           //分页的Url  如：/account/proList/page/1

    myPage.load = function () {

        pageStr += "<div style=\" font-size:14px; margin-bottom:10px;\" class=\"pager\">";
        //pageStr += "<span>共<font>" + this.pageSum + "</font>条记录，当前显示第<font>" + this.page + "</font>页</span><span class=\"pageCurrent\">";
        pageStr += "第 " + this.page + "/" + myPage.pageCount + " 页&nbsp;共 " + myPage.pageSum + " 条&nbsp;每页显示 20 条&nbsp;&nbsp;";

        var pageRp = this.pageUrl.replace("page", "々").split('々');

        if (this.page == 1) {
            pageStr += "<span class=\"disabled\">&lt;</span>";
            pageStr += "<span class=\"current\">1</span>";
        } else {
            pageStr += "<a href='#' style=\"cursor:pointer;\" onclick=\"proSearch('" + _pageUrl + "" + (this.page - 1) + pageRp[1] + "')\">&lt;</a>";
            pageStr += "<a href='#' style=\"cursor:pointer;\" onclick=\"proSearch('" + _pageUrl + "" + "1" + pageRp[1] + "')\">1</a>";
        }
        if (this.pageCount > 6) {
            if (this.page - 3 > 1) { //1页后面是否需要省略号
                pageStr += "<a class=\"current\">...</a>";
                var pcs = this.page - 2;
                if (this.pageCount - this.page <= 1)
                    pcs -= 2 - (this.pageCount - this.page);
                for (var k = pcs; k <= this.page; k++) {

                    if (k == this.page)
                        pageStr += "<span class=\"current\">" + k + "</span>";
                    else
                        pageStr += "<a  href='#'  style=\"cursor:pointer;\" onclick=\"proSearch('" + _pageUrl + "" + k + pageRp[1] + "')\">" + k + "</a>";
                }
            } else {
                for (var j = 2; j <= this.page; j++) {
                    if (j == this.page)
                        pageStr += "<span class=\"current\">" + j + "</span>";
                    else
                        pageStr += "<a href='#' style=\"cursor:pointer;\" onclick=\"proSearch('" + _pageUrl + "" + j + pageRp[1] + "')\">" + j + "</a>";
                }
            }

            if (this.pageCount > this.page + 3) { //最后一页前面是否需要省略号
                var pc = this.page + 2;
                if (this.page <= 2)
                    pc += (3 - this.page);
                for (var m = this.page + 1; m <= pc; m++) {
                    pageStr += "<a href='#'  style=\"cursor:pointer;\" onclick=\"proSearch('" + _pageUrl + "" + m + pageRp[1] + "')\">" + m + "</a>";
                }
                pageStr += "<a href='#' style=\"cursor:pointer;\" class=\"current\">...</a>";
                pageStr += "<a href='#' style=\"cursor:pointer;\" onclick=\"proSearch('" + _pageUrl + "" + (this.pageCount) + pageRp[1] + "')\">" + this.pageCount + "</a>";
            } else {
                for (var l = this.page + 1; l <= this.pageCount; l++) {
                    pageStr += "<a href='#' style=\"cursor:pointer;\" onclick=\"proSearch('" + _pageUrl + "" + l + pageRp[1] + "')\">" + l + "</a>";
                }
            }
        } else {
            for (var i = 2; i <= this.pageCount; i++) {
                if (this.page == i)
                    pageStr += "<span class=\"current\">" + i + "</span>";
                else
                    pageStr += "<a href='#' style=\"cursor:pointer;\" onclick=\"proSearch('" + _pageUrl + "" + i + pageRp[1] + "')\">" + i + "</a>";
            }
        }
        if (this.pageCount == this.page)
            pageStr += "<a href='#' style=\"cursor:pointer;\" class=\"disabled\">&gt;</a>";
        else
            pageStr += "<a href='#' style=\"cursor:pointer;\" onclick=\"proSearch('" + _pageUrl + "" + (this.page + 1) + pageRp[1] + "')\">&gt;</a>";

        pageStr += "</div>";
        $("#proPage").html("");
        $("#proPage").append(pageStr);
    };
    return myPage;
}
function proSearch(page)
{
    page = page.replace("undefined", " ");
    var url = page.substring(0, page.length - 1);
    window.location.href = url;
}

function createPage2(_pageNum, _pageUrl, _pageSum, _page) {
    document.write("<link href=\"/Content/purPage.css\" rel=\"stylesheet\" type=\"text/css\" />");
    var myPage = new Object;
    myPage.pageCount = (_pageSum % _pageNum) == 0 ? _pageSum / _pageNum : parseInt(_pageSum / _pageNum) + 1;          //总页数
    myPage.page = _page;                //当前页
    myPage.pageNum = _pageNum;             //每页显示的条数
    myPage.pageSum = _pageSum;             //全部条数
    myPage.pageUrl = _pageUrl;           //分页的Url  如：/account/proList/page/1

    myPage.load = function () {
        document.write("<div style=\"width: 100%;margin: 0 auto;overflow: hidden;\"><div class=\"purPage\">");
        document.write("<span>共<font>" + this.pageSum + "</font>条记录，当前显示第<font>" + this.page + "</font>页</span><span class=\"pageCurrent\">");

        var pageRp = this.pageUrl.replace("page", "々").split('々');

        if (this.page == 1) {
            document.write("<a class=\"NoperPage\"></a>");
            document.write("<a class=\"pageSel\" href=\"" + pageRp[0] + "1" + pageRp[1] + "\">1</a>");
        } else {
            document.write("<a class=\"perPage\" href=\"" + pageRp[0] + (this.page - 1) + pageRp[1] + "\"></a>");
            document.write("<a href=\"" + pageRp[0] + "1" + pageRp[1] + "\">1</a>");
        }
        if (this.pageCount > 6) {
            if (this.page - 3 > 1) { //1页后面是否需要省略号
                document.write("<a class='d'>...</a>");
                var pcs = this.page - 2;
                if (this.pageCount - this.page <= 1)
                    pcs -= 2 - (this.pageCount - this.page);
                for (var k = pcs; k <= this.page; k++) {

                    if (k == this.page)
                        document.write("<a class=\"pageSel\" href=\"" + pageRp[0] + k + pageRp[1] + "\">" + k + "</a>");
                    else
                        document.write("<a href=\"" + pageRp[0] + k + pageRp[1] + "\">" + k + "</a>");
                }
            } else {
                for (var j = 2; j <= this.page; j++) {
                    if (j == this.page)
                        document.write("<a class=\"pageSel\" href=\"" + pageRp[0] + j + pageRp[1] + "\">" + j + "</a>");
                    else
                        document.write("<a href=\"" + pageRp[0] + j + pageRp[1] + "\">" + j + "</a>");
                }
            }

            if (this.pageCount > this.page + 3) { //最后一页前面是否需要省略号
                var pc = this.page + 2;
                if (this.page <= 2)
                    pc += (3 - this.page);
                for (var m = this.page + 1; m <= pc; m++) {
                    document.write("<a href=\"" + pageRp[0] + m + pageRp[1] + "\">" + m + "</a>");
                }
                document.write("<a class='d'>...</a>");
                document.write("<a href=\"" + pageRp[0] + (this.pageCount) + pageRp[1] + "\">" + this.pageCount + "</a>");
            } else {
                for (var l = this.page + 1; l <= this.pageCount; l++) {
                    document.write("<a href=\"" + pageRp[0] + l + pageRp[1] + "\">" + l + "</a>");
                }
            }
        } else {
            for (var i = 2; i <= this.pageCount; i++) {
                if (this.page == i)
                    document.write("<a class=\"pageSel\" href=\"" + pageRp[0] + i + pageRp[1] + "\">" + i + "</a>");
                else
                    document.write("<a href=\"" + pageRp[0] + i + pageRp[1] + "\">" + i + "</a>");
            }
        }
        if (this.pageCount == this.page)
            document.write("<a class=\"NonextPage\"></a>");
        else
            document.write("<a class=\"nextPage\"  href=\"" + pageRp[0] + (this.page + 1) + pageRp[1] + "\"></a>");

        document.write("</span></div></div>");
    };
    return myPage;
}

