$(function(){
	
	$(document).on("touchmove",function(event){
		event.preventDefault();
	})
	
	//获取url参数的函数
	function GetQueryString(name) {
		var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
		var r = window.location.search.substr(1).match(reg);
		if(r != null) return unescape(r[2]);
		return null;
	}
	
	var lx=GetQueryString("lx");
    var xl;
    if(lx==1){
    	xl=[{
    		txt:'北区路入口',
    		fx:'left',
    		type:'type1'
    	},{
    		txt:'世贸中心',
    		fx:'right',
    		type:'type2'
    	},{
    		txt:'合景聚融',
    		fx:'left',
    		type:'type2'
    	},{
    		txt:'英利国际',
    		fx:'left',
    		type:'type2'
    	},{
    		txt:'解放东路出口',
    		fx:'right',
    		type:'type1'
    	}];
    }else if(lx==2){
    	xl=[{
    		txt:'储奇门入口',
    		fx:'left',
    		type:'type1'
    	},{
    		txt:'融创北区',
    		fx:'left',
    		type:'type2'
    	},{
    		txt:'解放东路出口',
    		fx:'right',
    		type:'type1'
    	}];
    }else{
    	return false;
    }
	var game={
		xl:xl,
		mm:0,
		total:1,
		total1:0,
		num:0,
		speed:100,
		bgspeed:8000,
		timer:null,
		time:0,
		lplist:null,
		init:function(){
			var _this=this;
			this.mm = parseInt(9000/ (this.xl.length-2));//不加速的情况多少秒跑完全程
			this.num=Math.floor(Math.random()*(this.xl.length-2))+1;
			$(".xs").css({
					"-webkit-animation":"none"
				})
			$(".bg3").text(_this.xl[0].txt);
			$(".start-btn.btn1").tap(function(){
				$(this).hide();
				$(".xt-container .xt").css({
					"-webkit-animation":"bgScroll 8000ms linear infinite;"
				})
				$(".xs").css({
					"-webkit-animation":"xs 8000ms linear infinite;"
				})
				_this.run();
			})
			$(".start-btn.btn2").tap(function(){
				if(_this.total > _this.xl.length-2){
					return false;
				}	
				_this.mm-=200;		
				if(_this.mm<=parseInt(9000/ (_this.xl.length-2) /2)){
					_this.mm=parseInt(9000/ (_this.xl.length-2) /2);
				}else{
					_this.bgspeed-=300;
					if(_this.bgspeed<=4000){
						_this.bgspeed=4000;
					}
				}
				$(".xt-container .xt").css({
					"-webkit-animation":"bgScroll "+_this.bgspeed+"ms linear infinite;"
				})
				$(".xs").css({
					"-webkit-animation":"xs "+_this.bgspeed+"ms linear infinite;"
				})
			})
			$(".sorry-btn.btn1").tap(function(){
				_this.mreset();
				$(".sorry-dialog").hide();
				$(".start-btn.btn1").show();
				$(".time").text("0s");
			})
			$(".win-btn").tap(function(){
				$(this).parents(".dialog").hide();
				$("#fx-dialog").show();
			})
			$(".start-btn").on("touchstart",function(){
				$(this).css({
					"background":"url('/jfb/images/start1.png')",
					"background-size":"100% 100%"
				});
			})
			$(".start-btn").on("touchend",function(){
				$(this).css({
					"background":"url('/jfb/images/start.png')",
					"background-size":"100% 100%"
				});
			})
			$(".games-btn").tap(function(){
				$(this).parents(".dialog").hide();
				if($(this).hasClass("true-btn")){
					$(".xt-container .xt").css({
						"-webkit-animation":"bgScroll "+_this.bgspeed+"ms linear infinite;"
					})
					$(".xs").css({
						"-webkit-animation":"xs "+_this.bgspeed+"ms linear infinite;"
					})
					_this.run();
				}else{
					$(".time").text("0s");
					$("#sorry-dialog").show();
				}
				
			})
		},
		run:function(){
			var _this=this;		
				$(".bg3").hide();
				_this.timer=setInterval(function(){
					_this.total1+=_this.speed;
					_this.time++;
					$(".time").text(_this.time+"S");
					console.log(_this.mm);
					if(_this.lplist == null){
						if(_this.total1%_this.mm==0){
							if(_this.num == _this.total){
								$("#ques-dialog").show();
								$(".xt-container .xt").css({
									"-webkit-animation":"none"
								})
								$(".xs").css({
									"-webkit-animation":"none"
								})
								clearInterval(_this.timer);
							}
							_this.lplist = new lp(_this.xl[_this.total]);
						}
					}else if(_this.lplist != null){
						if(_this.total1 % _this.mm == 0){
							_this.lplist.lp.remove();
							_this.lplist = null;
							game.total++;
							
							if(_this.total >= _this.xl.length-1){	
								$(".bg3").show().text(_this.xl[_this.xl.length-1].txt)
								setTimeout(function(){
									if(_this.time >= 120){
										$("#time-dialog").show();
									}else{
									    getredpack();
									}
									$(".xt-container .xt").css({
										"-webkit-animation":"none"
									})
									$(".xs").css({
										"-webkit-animation":"none"
									})
									clearInterval(_this.timer);	
								},1000)
							}
						}
					}
				},1000);
		},
		mreset:function(){
			this.mm = parseInt(9000/ (this.xl.length-2))
			this.num=Math.floor(Math.random()*(this.xl.length-2))+1;
			if(this.lplist != null){
				this.lplist.lp.remove();
				this.lplist = null;
			}	
			this.total=1;
			this.total1=0;
			this.speed=100;
			this.bgspeed=8000;
			this.timer=null;
			this.time=0;
			this.lplist=null;
		}
	}
	game.init();
	
	//路牌类
	
	function lp(obj){
		this.txt=obj.txt;
		this.fx=obj.fx;
		this.type=obj.type;
		this.lp;
		this.mm=game.mm;
		this.txtCon;
		this.init();	
	}
	lp.prototype.init=function(){
		this.creadE();
	}
	lp.prototype.creadE=function(){
		var _this=this;
		if(this.type=="type1"){
			this.lp=$('<div class="lp">');
		}else{
			this.lp=$('<div class="lp1">')
		}
		if(this.fx=="left"){
			this.lp.addClass('left');
		}else{
			this.lp.addClass("right");
		}
		this.txtCon=$('<p class="lp-text">'+this.txt+'</p>');
		this.lp.append(this.txtCon);
		$("body").append(this.lp);	
	}	
})
