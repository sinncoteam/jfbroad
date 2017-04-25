(function(){
	function setFont(){
		var num=16;
		var html=document.documentElement;
		var hWidth=html.getBoundingClientRect().width;
		html.style.fontSize=hWidth/num+"px";
	}
	setFont();
	window.onresize=function(){
		setFont();
	}
})()
