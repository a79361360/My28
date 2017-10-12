var data,
		myScroll,
		pullDownEl, pullDownOffset,
		pullUpEl, pullUpOffset,
		generatedCount = 0;
var page = 2;
	function pullDownAction () {
	    window.location.href = window.location.href;
	}

	function pullUpAction() {
	    $.ajax({
	        type: "POST",
	        url: "/WxJs28/MoneyLog",
	        data: { "page": page },
	        dataType: 'json', 
	        success: function (data) {
	            setTimeout(function () {
	                myScroll.refresh();
	            }, 0);
	            $(".divx .tb2").append(data.data);
	            page++;
	           
	        }
	    });

	}

	//初始化绑定iScroll控件 
	document.addEventListener('touchmove', function (e) { e.preventDefault(); }, false);
	document.addEventListener('DOMContentLoaded', loaded, false);

	function loaded() {
		pullDownEl = document.getElementById('pullDown');
		pullDownOffset = pullDownEl.offsetHeight;
		pullUpEl = document.getElementById('pullUp');	
		pullUpOffset = pullUpEl.offsetHeight;
		
		/**
		 * 初始化iScroll控件
		 */
		myScroll = new iScroll('wrapper', {
			vScrollbar : false,
			topOffset : pullDownOffset,
			onRefresh : function () {
			    if (pullDownEl.className.match('loading')) {
			        pullDownEl.className = '';
			        pullDownEl.querySelector('.pullDownLabel').innerHTML = '下拉刷新...';
			    } else {

			        if (pullUpEl.className.match('loading')) {
			            pullUpEl.className = '';
			            pullUpEl.querySelector('.pullUpLabel').innerHTML = '上拉加载更多...';
			        }
			    }
			},
			onScrollMove: function () {
			    if (this.y > 5 && !pullDownEl.className.match('flip')) {
			        pullDownEl.className = 'flip';
			        pullDownEl.querySelector('.pullDownLabel').innerHTML = '松手刷新...';
			        this.minScrollY = 0;
			    } else {
			        if (this.y < (this.maxScrollY - 5) && !pullUpEl.className.match('flip')) {
			            pullUpEl.className = 'flip';
			            pullUpEl.querySelector('.pullUpLabel').innerHTML = '松手刷新...';
			        }
			    }
			},
			onScrollEnd: function () {
			    if (pullDownEl.className.match('flip')) {
			        pullDownEl.className = 'loading';
			        pullDownEl.querySelector('.pullDownLabel').innerHTML = '加载中...';
			        pullDownAction();
			    } else {
			        if (pullUpEl.className.match('flip')) {
			            pullUpEl.className = 'loading';
			            pullUpEl.querySelector('.pullUpLabel').innerHTML = '加载中...';
			            pullUpAction();
			        }
			    }
			}
		});
	}