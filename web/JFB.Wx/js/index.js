$(function() {

	(function() {

		var btn = $(".index-btn");
		var dialog = $(".dialog");
		btn.tap(function() {
			dialog.show();
		})

		var bar = $(".tab-bar .tab-list");
		var item = $(".tab-content .tab-content-item");
		bar.tap(function() {
			var _index = $(this).index();
			$(this).addClass("active").siblings().removeClass("active");
			item.eq(_index).addClass("active").siblings().removeClass("active");
		})

	})()
	
	

})